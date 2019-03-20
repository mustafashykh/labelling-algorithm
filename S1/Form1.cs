using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace S1
{
    public partial class Form1 : Form
    {
        List<obj> rememberList = new List<obj>();
        


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            Bitmap bmp;
            if (file.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(file.FileName);
                pictureBox1.Image = bmp;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            //bmp = greyScale();
            int[,] label = new int[bmp.Height, bmp.Width];
            int[,] upper = new int[bmp.Height, bmp.Width];
            int[,] left = new int[bmp.Height, bmp.Width];
            


            this.rememberList = new List<obj>();
            obj temp;
            
            int index = 1;
            
            //Nested For loops for horizontal Scan
            for (int i = 1; i < bmp.Height-1; i++)
            {
                for(int j = 1; j < bmp.Width-1; j++)
                {
                    
                    if (bmp.GetPixel(j, i) == Color.Black || bmp.GetPixel(j, i).R < 10)
                    {
                        upper[i, j] = label[i - 1, j];
                        left[i, j] = label[i, j - 1];
                        

                        if(upper[i,j] == 0 && left[i,j] == 0)
                        {
                            label[i, j] = index;
                            index++;
                        }else if (upper[i, j] == 0 && left[i, j] != 0 || upper[i, j] != 0 && left[i, j] == 0)
                        {
                            if(upper[i,j] != 0)
                            {
                                label[i, j] = upper[i, j];
                                //Console.WriteLine("UHit");
                            }
                            else 
                            {
                                label[i, j] = left[i, j];
                                //Console.WriteLine("LHit");
                            }
                            
                        }
                        else if(left[i,j] == upper[i, j] && left[i,j] != 0 && upper[i,j] != 0)
                        {
                            //Console.WriteLine("Same");
                            label[i, j] = upper[i, j];
                        }else if (left[i,j] != upper[i,j] && left[i, j] != 0 && upper[i, j] != 0)
                        {
                            
                            if (upper[i,j] < left[i, j])
                            {
                                label[i, j] = upper[i, j];

                                temp = search(upper[i, j]);
                                //bool overwritten = deepSearch(upper[i,j]); 
                                if (temp == null)
                                {
                                    temp = new obj {
                                        id = upper[i, j]
                                    };
                                    temp.list.Add(left[i, j]);
                                    rememberList.Add(temp);
                                }
                                else if(temp != null)
                                {
                                    if (!(temp.list.Contains(left[i, j])))
                                    {
                                        temp.list.Add(left[i, j]);
                                    }
                                }       
                            } else if (upper[i, j] > left[i, j])
                            {
                                label[i, j] = left[i, j];
                                
                                temp = search(left[i, j]);
                                //bool overwritten = deepSearch(left[i, j]);
                            
                                if (temp == null)
                                {
                                    temp = new obj
                                    {
                                        id = left[i, j]
                                    };
                                    temp.list.Add(upper[i, j]);
                                    rememberList.Add(temp);
                                    

                                }
                                else if(temp != null)
                                {
                                    if (!(temp.list.Contains(upper[i,j])))
                                    {
                                        
                                        temp.list.Add(upper[i,j]);
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }

            //Console.WriteLine(rememberList.Count);
            this.rememberList = this.rememberList.OrderByDescending(x => x.id).ToList();
            //Console.WriteLine(this.rememberList.Count);

            for (int i = 0 ; i < this.rememberList.Count ; i++)
            {
                label = indexHandeler(this.rememberList[i].id, this.rememberList[i].list, label, bmp.Height, bmp.Width);
            }


            //This is reponsibe for error correction
            label = ErrorHandler(label,bmp.Height, bmp.Width);

            

            IEnumerable<int> max = label.Cast<int>().Distinct();
            


            //Console.WriteLine("Number of Objects:\t "+(max.Count()-1));
            label1.Text = "Objects ="+ (max.Count() - 1);
            int c = label.Cast<int>().Max();

            Color[] ColorList = new Color[c];
            Random rand = new Random();
            for(int i = 0; i < c-1; i++)
            {
                ColorList[i] = Color.FromArgb(rand.Next(0,255), rand.Next(0, 255), rand.Next(0, 255));
            }

            
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    if(label[y,x] != 0)
                    {
                        bmp.SetPixel(x, y, ColorList[label[y,x]-1]);
                    }
                }
            }

            pictureBox1.Image = bmp;
        }


        //helper Method
        private Bitmap greyScale()
        {
            Bitmap temp = new Bitmap(pictureBox1.Image);
            for (int w = 0; w < temp.Width; w++)
            {
                for (int h = 0; h < temp.Height; h++)
                {
                    int total;
                    int red = temp.GetPixel(w, h).R;
                    int green = temp.GetPixel(w, h).G;
                    int blue = temp.GetPixel(w, h).B;

                    total = (red + green + blue) / 3;

                    temp.SetPixel(w, h, Color.FromArgb(total, total, total));
                }
            }

            for (int w = 0; w < temp.Width; w++)
            {
                for (int h = 0; h < temp.Height; h++)
                {
                    if (temp.GetPixel(w, h) == Color.Black || temp.GetPixel(w, h).R < 150)
                    {
                        temp.SetPixel(w, h, Color.Black);
                    }
                    else
                    {
                        temp.SetPixel(w, h, Color.White);
                    }
                }
            }

            return temp;
        }
        private int[,] indexHandeler(int num,List<int> list, int[,] arr, int height, int width)
        {

            for(int i = 1; i < width; i++)
            {
                for(int j = 1; j < height; j++)
                {
                    if (list.Contains(arr[j, i]))
                    {
                        arr[j, i] = num;
                    }
                }
            }

            return arr;
        }


        private obj search(int num)
        {
            obj x = this.rememberList.Find(e => e.id == num);
            if (x != null)
            {
                return x;
            }
            else
            {
                return null;
            }
        }


        private int[,] corrector(int[,] list, int err, int correct, int height, int width) {
            for(int y = 1; y < height-1; y++)
            {
                for(int x = 1; x < width-1; x++)
                {
                    if(list[y,x] == err)
                    {
                        list[y, x] = correct;
                    }
                }
            }

            return list;
        }

        private int[,] ErrorHandler(int[,] list,int height, int width)
        {
            
            for(int x = 1; x < height-1; x++)
            {
                for(int y = 1; y < width-1 ; y++)
                {

                    //upper
                    if (list[x, y] > list[x - 1, y] && list[x - 1, y] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x - 1, y], height, width);
                    }

                    //upper-left
                    if (list[x, y] > list[x - 1, y -1] && list[x - 1, y - 1] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x - 1, y - 1], height, width);
                    }

                    //left
                    if (list[x, y] > list[x, y - 1] && list[x, y - 1] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x, y - 1], height, width);
                    }

                    //right
                    if (list[x, y] > list[x, y+1] && list[x, y + 1] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x, y + 1], height, width);
                    }

                    //left-bottom
                    if (list[x, y] > list[x + 1, y - 1] && list[x + 1, y - 1] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x + 1, y - 1], height, width);
                    }

                    //right-top
                    if (list[x, y] > list[x - 1, y + 1] && list[x - 1, y + 1] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x - 1, y + 1], height, width);
                    }

                    //bottom
                    if (list[x, y] > list[x + 1, y] && list[x + 1, y] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x + 1, y], height, width);
                    }

                    //right-bottom
                    if (list[x, y] > list[x + 1, y + 1] && list[x + 1, y + 1] != 0 && list[x, y] != 0)
                    {
                        list = corrector(list, list[x, y], list[x + 1, y + 1], height, width);
                    }

                }
            }

            return list;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = greyScale();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mustafasheikh1");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mustafasheikh1/labelling-algorithm");
        }
    }

    class obj
    {
        public  int id;
        public List<int> list = new List<int>();

        public bool search(int num)
        {
            if (list.Contains(num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
}
