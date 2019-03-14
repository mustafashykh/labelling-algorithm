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
            

            int index = 1;
            
            //Nested For loops for horizontal Scan
            for(int i = 1; i < bmp.Height-1; i++)
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
                            ///Console.WriteLine("Not Same");
                            if (upper[i,j] < left[i, j])
                            {
                                label[i, j] = upper[i, j];
                                
                            }
                            else
                            {
                                label[i, j] = left[i, j];
                                    
                            }

                            //index--;
                        }
                    }
                }
            }


            int max = label.Cast<int>().Max();

            Console.WriteLine(max);


            Color[] ColorList = new Color[max];


            Random rand = new Random();
            for(int i = 0; i < max; i++)
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

            pictureBox1.Image = bmp;
            
        }


        
    }

    
}
