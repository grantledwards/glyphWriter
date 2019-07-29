using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace glyph2
{
    class glyph
    {
        public string content;                      //written string
        public LinkedList<syllable> syllableList;   //list of syllable objects
        Random rand;
        public double worldX, worldY;
        public int xSize, ySize;                    //dimension of full glyph in pixels
        public int id;                              //unique id number, applied to each glyph 
        public int previewRightCount;
        public int gridSizeX, gridSizeY;            //
        public editingPanelData ePD;
            public syllable selected { set; get; }

        public int selectedListID=0;
        public int previewNum = 1;

        public glyph(int i)
        {
        //gridSizeX = gridSizeY = 2;
        content = "";
            syllableList = new LinkedList<syllable>();
            id = i;
            rand = new Random();
            previewRightCount = 0;
            ePD = new editingPanelData();

            xSize = 60;
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
            if (content == null)
                return;

            int ctr = 0;//, fake3Counter=0;
           // syllableList.
            
            string[] tempTokens = content.Trim().Split(' ');
            int numTokens = tempTokens.Length;

            if (syllableList.Count() > numTokens)
                return;

            syllableList.Clear();

            syllable newSyl;
            int coordx = 0, coordy = 0;
            foreach (string s in tempTokens)
            {
                if (s != "")
                {
                    //if(sylableList.ElementAt(ctr) == null || sylableList.ElementAt(ctr) != s)
                    newSyl = new syllable(s, ctr++);

                   // if
                    newSyl.box = new previewRectangle(coordx, coordy, coordx, coordy);
                    coordx++;
                    //newSyl needs to be different. More static

                    if (coordx > ePD.gridSizeX - 1)
                    { coordx = 0; coordy++; }
                    syllableList.AddLast(newSyl);
                }
            }
        }

        public void setSelection(int index)
        {
            if (syllableList.Count <= index)
                return;
            else
            {
                syllable temp;
                int ctr = 0;
                foreach (syllable s in syllableList)
                {
                    if(ctr == index)
                    {
                        s.isSelected = true;
                    }
                    else
                        s.isSelected = false;
                }
               // syllableList.ElementAt(index).isSelected = true;
            }
        }

        public void changeBox(int index, int x1,int y1, int x2, int y2)
        {
            if (syllableList.Count <= index)
                return;
            else
            {
                syllable temp;
                int ctr = 0;
                foreach (syllable s in syllableList)
                {
                    if (ctr == index)
                    {
                        s.box = new previewRectangle(x1, y1, x2, y2);
                        //MessageBox.Show("added):" +s.box.x1);
                        return;
                    }
                    ctr++;
                }
                // syllableList.ElementAt(index).isSelected = true;
            }
        }

        public class syllable
        {
            private string token;
            private int count;
            //private Button select;

            public syllable(string tok, int c)
            {
                listToken = token = tok;
                count = c;
                //this.select = new Button();
                //this.select.Content = "EDIT";
            }

            public string listToken { set; get; }

            public Boolean isSelected { set; get; }

            public previewRectangle box { set; get; }

            

            public StackPanel generateSylStack()
            {
                StackPanel ret = new StackPanel();
                ret.Orientation = Orientation.Horizontal;
                TextBlock tb = new TextBlock();
                tb.Text = token;
                
                ret.Children.Add(tb);

                
                // select.Click = new System.Windows.RoutedEventHandler();
                //ret.AddHandler(clic)
                // this.select.
                //ret.Children.Add(select);
                return ret;
            }

            private void select_Click(object sender, RoutedEventArgs e)
            {
                //this.select.Content += "add";
            }
        }

        public class previewRectangle
        {
            public int x1, y1, x2, y2;

            public previewRectangle(int xA, int yA, int xB, int yB)
            {
                x1 = xA;
                x2 = xB;
                y1 = yA;
                y2 = yB;
            }
        }
    }
}
