using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        left[i, j]= label[i, j - 1];
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
                            label[i, j] = left[i, j];
                        }else if (left[i,j] != upper[i,j] && left[i, j] != 0 && upper[i, j] != 0)
                        {
                            
                            if (upper[i,j] < left[i, j])
                            {
                                label[i, j] = upper[i, j];

                                temp = search(upper[i, j]);
                                //bool overwritten = deepSearch(upper[i,j]); 
                                if (temp == null )
                                {
                                    temp = new obj();
                                    temp.id = upper[i, j];
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
                            } else
                            {
                                label[i, j] = left[i, j];

                                temp = search(left[i, j]);
                                //bool overwritten = deepSearch(left[i, j]);
                            
                                if (temp == null )
                                {
                                    temp = new obj();
                                    temp.id = left[i, j];
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

            Console.WriteLine(rememberList.Count);

            for (int i = rememberList.Count-1 ; i >= 0; i--)
            {
                //Console.WriteLine(rememberList[0].id);
                label = indexHandeler(rememberList[i].id, rememberList[i].list, label, bmp.Height, bmp.Width);
            }

            //int numberOfColors = 
            IEnumerable<int> max = label.Cast<int>().Distinct();

            Console.WriteLine("Number of Objects:\t "+(max.Count()-1));
            label1.Text = "Objects ="+ (max.Count() - 1);
            int c = label.Cast<int>().Max();

            Color[] ColorList = new Color[c];

            Random rand = new Random();
            for(int i = 0; i < c-1; i++)
            {
                ColorList[i] = Color.FromArgb(rand.Next(0,256), rand.Next(0, 256), rand.Next(0, 256));
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


            //Console.WriteLine("Number of detected objects:\t"+ColorList.Count());
            pictureBox1.Image = bmp;
            
        }


        //helper Method
        private bool deepSearch(int num)
        {
            for(int x = 0; x < rememberList.Count-1; x++)
            {
                if (rememberList[x].list.Contains(num))
                {
                    return true;
                }
            }
            return false;
        }
        private int[,] indexHandeler(int num,List<int> list, int[,] arr, int height, int width)
        {

            for(int i = 1; i < height-1; i++)
            {
                for(int j = 1; j < width-1; j++)
                {
                    if (list.Contains(arr[i, j]))
                    {
                        arr[i, j] = num;
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
