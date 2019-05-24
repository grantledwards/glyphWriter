using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace glyph2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand;
        glyph current;
        int idCtr;
        //int previewGridX, previewGridY;
        int previewLeftCount;
        int glyphBuffer;


        private class glyph
        {
            public string content;
            public LinkedList<syllable> syllableList;
            Random rand;
            public int code;
            public double worldX, worldY;
            public int xSize, ySize;
            public int id;
            public int previewRightCount;
            public int gridSizeX, gridSizeY;
            public editingPanelData ePD;

            public glyph(int i)
            {
                //gridSizeX = gridSizeY = 2;
                syllableList = new LinkedList<syllable>();
                id = i;
                rand = new Random();
                code = (rand.Next() % 2) + 1;
                previewRightCount = 0;
                ePD = new editingPanelData();

                xSize = 50;
                ySize = 50;
            }

            public void updatePos(int x, int y)
            {
                worldX = x;
                worldY = y;
            }

            internal int sylableCount()
            {
                if (syllableList == null)
                    return previewRightCount = 0;
                return previewRightCount = syllableList.Count();
            }

            internal void makeTokens()
            {
                int ctr = 0;
                syllableList.Clear();
                string[] tempTokens = content.Split(' ');
                int numTokens = tempTokens.Length;
                syllable newSyl;
                int coordx=0, coordy=0;
                foreach(string s in tempTokens)
                {
                    if(s != "")
                    {
                        //if(sylableList.ElementAt(ctr) == null || sylableList.ElementAt(ctr) != s)
                        newSyl = new syllable(s, ctr);
                        newSyl.box = new previewRectangle(coordx,coordy,coordx,coordy);
                        coordx++;
                        if (coordx > 1)
                        {coordx = 0;coordy++;}
                        syllableList.AddLast(newSyl);
                    }
                }
            }

            public class syllable
            {
                private string token;
                private int count;

                public syllable(string tok,int c)
                {
                    listToken = token = tok;
                    count = c;
                }

                public string listToken { set; get; }

                public previewRectangle box { set; get; }
                /*
                public void set(string input)
                {
                    token = input;
                }

                public string get()
                {
                    return token;
                }*/
            }

            public class previewRectangle
            {
                public int x1, y1, x2, y2;

                public previewRectangle(int xA,int yA,int xB,int yB)
                {
                    if(xB>xA)
                    {
                        x1 = xB;
                        x2 = xA;
                    }
                    else
                    {
                        x1 = xA;
                        x2 = xB;
                    }
                    if (yB > yA)
                    {
                        y1 = yB;
                        y2 = yA;
                    }
                    else
                    {
                        y1 = yA;
                        y2 = yB;
                    }
                }
            }
        }

        private LinkedList<glyph> glyphList;

        public MainWindow()
        {
            glyphList = new LinkedList<glyph>();
            rand = new Random();

            glyphBuffer = 10;
            //writerBox.(writerBox.Select(0, 4))

            InitializeComponent();

            
        }
        
        private int newRand(int mod)
        { return rand.Next() % mod; }

        public void Update()
        {
            workArea.Children.Clear();

            DrawAll();

            workArea.InvalidateVisual();
        }

        public void UpdateCountLabel()
        {
            boxCountLabel.Content = previewLeftCount + "/" + current.previewRightCount;
        }

        public void populateFrame()
        {
            writerBox.Text = current.content;
            syllableBox.DataContext = current.syllableList;
        }

        public void addPreviewBox(int x1, int y1, int x2, int y2)
        {

        }

        public void UpdatePreviewBox()
        {
            previewBox.Children.Clear();
            DrawPreviewLines();
            previewBox.InvalidateVisual();
        }

        public void DrawPreviewLines()
        {
            int i;
            for ( i = 0; i <= current.ePD.gridSizeX - 0; i++)
            {
                Line vert = new Line();
                vert.Stroke = getColor(5);
                if (i == 0)
                    vert.X1 = vert.X2 = 0;
                else
                    vert.X1 = vert.X2 = previewBox.Height / current.ePD.gridSizeX * (i + 0);
                vert.Y1 = 0;
                vert.Y2 = previewBox.Height;
                previewBox.Children.Add(vert);
            }
            for ( i = 0; i <= current.ePD.gridSizeY - 0; i++)
            {
                Line horiz = new Line();
                horiz.Stroke = getColor(5);
                if (i == 0)
                    horiz.Y1 = horiz.Y2 = 0;
                else
                    horiz.Y1 = horiz.Y2 = previewBox.Height / current.ePD.gridSizeY * (i + 0);
                horiz.X1 = 0;
                horiz.X2 = previewBox.Height;
                previewBox.Children.Add(horiz);
            }
        }

        public void DrawAll()
        {
            int gridX = glyphBuffer, gridY = glyphBuffer;
            foreach (glyph g in glyphList)
            {
                g.updatePos(gridX, gridY);
                drawGlyph(g,(int)g.worldX, (int)g.worldY);
                gridX += g.xSize + glyphBuffer;//60;
                if (gridX > workArea.ActualWidth - (g.xSize+5))
                {
                    gridX = glyphBuffer;
                    gridY += g.ySize + glyphBuffer;
                }
            }

          /*  Button add = new Button();
            add.Width = 50;
            add.Height = 50;
            add.Margin = new Thickness(gridX, gridY, 0, 0);
            add.Content = "+";
            //add.Click = "newButton_Click";
            workArea.Children.Add(add);*/
        }

        private void drawGlyph(glyph g, int xMargin, int yMargin)
        {
            if(current != null)
                if(current.id == g.id)
                {
                    Rectangle highlight = new Rectangle();

                    highlight.Stroke = getColor(3);
                    highlight.Margin = new Thickness(xMargin -5, yMargin -5, 0, 0);
                    highlight.RadiusX = highlight.RadiusY = 7;
                    highlight.Width = highlight.Height = g.xSize + 10;
                    workArea.Children.Add(highlight);
                }

            if(g.sylableCount()==0)
            {
                Rectangle blank = new Rectangle();
                blank.Fill = getColor(4);
                blank.Margin = new Thickness(xMargin + 10, yMargin + 10, 0, 0);
                blank.RadiusX = blank.RadiusY = 5;
                blank.Width = blank.Height = 30;
                workArea.Children.Add(blank);
            }
            else
            {
                foreach(glyph.syllable draw in g.syllableList)
                {
                    Rectangle small = new Rectangle();
                    small.Fill = getColor(2);
                    small.Stroke = getColor(0);
                    double smallXMargin = draw.box.x1 * ((50 / 2) - 2)
                        , smallYMargin = draw.box.y1 * ((50 / 2) - 2)
                        , smallXSize = (50/2)+2
                        , smallYSize = (50/2)+2;
                    small.Margin = new Thickness(xMargin+smallXMargin, yMargin+smallYMargin, 0, 0);
                    small.RadiusX = small.RadiusY = 5;
                    small.Width = smallXSize;
                    small.Height = smallYSize;
                    workArea.Children.Add(small);

                    TextBlock lab = new TextBlock();
                    lab.Text = draw.listToken;
                    lab.FontSize = 8;
                    lab.Margin = new Thickness(xMargin + smallXMargin +2, yMargin + smallYMargin+2, 0, 0);
                    lab.Foreground = getColor(0);
                    workArea.Children.Add(lab);
                }
            }
            /*switch(g.sylableCount())
            {
                case 0:
                    Rectangle blank = new Rectangle();
                    blank.Fill = getColor(4);
                    blank.Margin = new Thickness(xMargin + 10, yMargin + 10, 0, 0);
                    blank.RadiusX = blank.RadiusY = 5;
                    blank.Width = blank.Height = 30;
                    workArea.Children.Add(blank);
                    break;
                case 1:
                    Rectangle one = new Rectangle();

                    one.Fill = getColor(2);
                    one.Stroke = getColor(0);
                    one.Margin = new Thickness(xMargin, yMargin, 0, 0);
                    one.RadiusX = one.RadiusY = 10;
                    one.Width = one.Height = 50;
                    workArea.Children.Add(one);
                    break;
                case 2:
                    Rectangle two1 = new Rectangle();
                    Rectangle two2 = new Rectangle();

                    two1.Fill = getColor(2);
                    two1.Stroke = getColor(0);
                    two1.Margin = new Thickness(xMargin, yMargin, 0, 0);
                    two1.RadiusX = two1.RadiusY = 7;
                    two1.Width = 27;
                    two1.Height = 50;
                    workArea.Children.Add(two1);

                    two2.Fill = getColor(2);
                    two2.Stroke = getColor(0);
                    two2.Margin = new Thickness(xMargin+23, yMargin, 0, 0);
                    two2.RadiusX = two2.RadiusY = 7;
                    two2.Width = 27;
                    two2.Height = 50;
                    workArea.Children.Add(two2);
                    break;
                case 3:
                    Rectangle three1 = new Rectangle();
                    Rectangle three2 = new Rectangle();
                    Rectangle three3 = new Rectangle();

                    three1.Fill = getColor(2);
                    three1.Stroke = getColor(0);
                    three1.Margin = new Thickness(xMargin, yMargin, 0, 0);
                    three1.RadiusX = three1.RadiusY = 7;
                    three1.Width = 27;
                    three1.Height = 50;
                    workArea.Children.Add(three1);

                    three2.Fill = getColor(2);
                    three2.Stroke = getColor(0);
                    three2.Margin = new Thickness(xMargin + 23, yMargin, 0, 0);
                    three2.RadiusX = three2.RadiusY = 5;
                    three2.Width = 27;
                    three2.Height = 27;
                    workArea.Children.Add(three2);

                    three3.Fill = getColor(2);
                    three3.Stroke = getColor(0);
                    three3.Margin = new Thickness(xMargin + 23, yMargin+23, 0, 0);
                    three3.RadiusX = three3.RadiusY = 5;
                    three3.Width = 27;
                    three3.Height = 27;
                    workArea.Children.Add(three3);
                    break;
                case 4:
                    Rectangle temp = new Rectangle();
                    Rectangle temp2 = new Rectangle();
                    Rectangle temp3 = new Rectangle();
                    Rectangle temp4 = new Rectangle();

                    temp.Fill = getColor(2);
                    temp.Stroke = getColor(0);
                    temp.Margin = new Thickness(xMargin + 23, yMargin + 23, 0, 0);
                    temp.RadiusX = temp.RadiusY = 5;
                    temp.Width = temp.Height = 27;
                    workArea.Children.Add(temp);

                    temp2.Fill = getColor(2);
                    temp2.Stroke = getColor(0);
                    temp2.Margin = new Thickness(xMargin + 23, yMargin, 0, 0);
                    temp2.RadiusX = temp2.RadiusY = 5;
                    temp2.Width = temp2.Height = 27;
                    workArea.Children.Add(temp2);

                    temp3.Fill = getColor(2);
                    temp3.Stroke = getColor(0);
                    temp3.RadiusX = temp3.RadiusY = 5;
                    temp3.Width = temp3.Height = 27;
                    temp3.Margin = new Thickness(xMargin, yMargin + 23, 0, 0);
                    workArea.Children.Add(temp3);

                    temp4.Fill = getColor(2);
                    temp4.Stroke = getColor(0);
                    temp4.RadiusX = temp4.RadiusY = 5;
                    temp4.Width = temp4.Height = 27;
                    temp4.Margin = new Thickness(xMargin, yMargin, 0, 0);
                    workArea.Children.Add(temp4);
                    break;
                default:
                    
                    break;
            }  */
        }

        private Brush getColor(int code)
        {
            switch (code)
            {
                case 0:
                    return Brushes.Black;
                case 1:
                    return Brushes.Yellow;
                case 2:
                    return Brushes.White;
                case 3:
                    return Brushes.LightGreen;
                case 4:
                    return Brushes.LightGray;
                case 5:
                    return Brushes.Gray;
                default:
                    return Brushes.Blue;
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            current = new glyph(idCtr++);
            glyphList.AddLast(current);

            writerBox.IsEnabled = true;
            
            xGridSizeTextbox.IsEnabled = true;
            yGridSizeTextbox.IsEnabled = true;
            xGridSizeTextbox.Text = "" + current.ePD.gridSizeX;
            yGridSizeTextbox.Text = "" + current.ePD.gridSizeY;
            
            previewLeftCount = 0;

            UpdatePreviewBox();

            Update();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Update();
        }

        private void workArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double y = Mouse.GetPosition(workArea).Y;
            double x = Mouse.GetPosition(workArea).X;

            foreach(glyph g in glyphList)
            {
                if(x >= g.worldX && x <= g.worldX+50 &&
                    y >= g.worldY && y <= g.worldY + 50)
                {
                    current = g;
                    writerBox.IsEnabled = true;
                    xGridSizeTextbox.Text = "" + current.ePD.gridSizeX;
                    yGridSizeTextbox.Text = "" + current.ePD.gridSizeY;

                    UpdatePreviewBox();

                    populateFrame();
                }
            }

            Update();
        }

        bool previewIsDown = false;
        int xStartVal;
        int yStartVal;

        private void previewBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            previewIsDown = true;
            double y = Mouse.GetPosition(previewBox).Y;
            double x = Mouse.GetPosition(previewBox).X;

            xStartVal = (int)(x / (previewBox.Height / current.ePD.gridSizeX));
            yStartVal = (int)(y / (previewBox.Height / current.ePD.gridSizeY));

            boxCountLabel.Content = xStartVal + "/" + yStartVal;
            /*for (i = 0; i <= previewGridY - 0; i++)
            {
                Line horiz = new Line();
                horiz.Stroke = getColor(5);
                if (i == 0)
                    horiz.Y1 = horiz.Y2 = 0;
                else
                    horiz.Y1 = horiz.Y2 = previewBox.Height / previewGridY * (i + 0);
                horiz.X1 = 0;
                horiz.X2 = previewBox.Height;
                previewBox.Children.Add(horiz);
            }*/
        }
        private void previewBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (previewIsDown)
            {
                previewIsDown = false;
                double y = Mouse.GetPosition(previewBox).Y;
                double x = Mouse.GetPosition(previewBox).X;

                int xEndVal = (int)(x / (previewBox.Height / current.ePD.gridSizeX));
                int yEndVal = (int)(y / (previewBox.Height / current.ePD.gridSizeY));

                boxCountLabel.Content = xEndVal + "/" + yEndVal;

                addPreviewBox(xStartVal, yStartVal, xEndVal, yEndVal);
            }
        }

        private void PreviewBoxLooper()
        {/*
            if (bigTimer > 5)
            {
                bigtimer = 0;
                PreviewBoxLooper
            }
            bigTimer++;*/
        }

        private void writerBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            current.content = writerBox.Text;
            current.makeTokens();
            UpdateCountLabel();
            Update();
        }

        private void xGridSizeTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(xGridSizeTextbox.Text, out current.ePD.gridSizeX)) 
                UpdatePreviewBox();
        }

        private void yGridSizeTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(yGridSizeTextbox.Text, out current.ePD.gridSizeY))
                UpdatePreviewBox();
        }
                
        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            glyphList.Remove(current);
            writerBox.IsEnabled = false;
            xGridSizeTextbox.IsEnabled = false;
            yGridSizeTextbox.IsEnabled = false;
            Update();
        }
    }
}
