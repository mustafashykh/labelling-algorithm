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
        public int R, G, B = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            Bitmap bmp;
            if(file.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(file.FileName);
                pictureBox1.Image = bmp;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.R = 0;
            this.G = 0;
            this.B = 0;
            LinkedList<Obj> list = new LinkedList<Obj>();

            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap temp = new Bitmap(pictureBox1.Image);
            temp = greyScale(temp);

            for (int y = 0; y < temp.Height; y++)
            {
                for (int x = 0; x < temp.Width; x++)
                {
                    
                    if (temp.GetPixel(x,y) == Color.FromArgb(0,0,0))
                    {

                        if (!caseHandeler(temp, x, y))
                        {
                            temp = indexPixel(temp, x, y);
                            Obj obj = new Obj();
                            obj.list.AddFirst(Color.FromArgb(temp.GetPixel(x, y).R, temp.GetPixel(x, y).G, temp.GetPixel(x, y).B));
                            list.AddLast(new Obj());
                            
                        }else
                        {
                            Color color = newColor(temp, x, y);
                            Console.WriteLine(color.R + "," + color.G + "," + color.B);

                            temp.SetPixel(x,y, color);
                        }
                    }
                }
            }



            for (int x = 0; x < temp.Width; x++)
            {
                for(int y = 0; y < temp.Height; y++)
                {
                    if(temp.GetPixel(x,y).B == 1 ) 
                    {
                        
                        temp.SetPixel(x, y, Color.Blue);
                    }
                    if (temp.GetPixel(x, y).B == 2 )
                    {

                        temp.SetPixel(x, y, Color.Green);
                    }
                }
            }

            Console.WriteLine(this.B);
            Console.WriteLine(list.Count);
            pictureBox1.Image = temp;

            
        }


        private Color newColor(Bitmap bmp, int x, int y)
        {
            int max = 0;
            int total = 0;

            //for left pixel
            if (bmp.GetPixel(x - 1, y) != Color.FromArgb(255, 255, 255))
            {
                total = bmp.GetPixel(x - 1, y).R + bmp.GetPixel(x - 1, y).G + bmp.GetPixel(x - 1, y).B;

                if (total > max)
                {
                    max = 1;
                }
            }


            if (bmp.GetPixel(x - 1, y - 1) != Color.FromArgb(255, 255, 255))
            {

                total = bmp.GetPixel(x - 1, y - 1).R + bmp.GetPixel(x - 1, y - 1).G + bmp.GetPixel(x - 1, y - 1).B;

                if (total > max)
                {
                    max = 2;
                }
            }


            if (bmp.GetPixel(x, y - 1) != Color.FromArgb(255, 255, 255))
            {
                total = bmp.GetPixel(x, y - 1).R + bmp.GetPixel(x, y - 1).G + bmp.GetPixel(x, y - 1).B;
                if (total > max)
                {
                    max = 3;
                }
            }


            if (bmp.GetPixel(x + 1, y - 1) != Color.FromArgb(255, 255, 255))
            {
                total = bmp.GetPixel(x + 1, y - 1).R + bmp.GetPixel(x + 1, y - 1).G + bmp.GetPixel(x + 1, y - 1).B;
                if (total > max)
                {
                    max = 4;

                }
            }
                

            //Console.WriteLine(max);
            Color color;
            switch (max)
            {
                case 1:
                    color = Color.FromArgb(bmp.GetPixel(x - 1, y).R, bmp.GetPixel(x - 1, y).G, bmp.GetPixel(x - 1, y).B);
                    return color;
                case 2:
                    color = Color.FromArgb(bmp.GetPixel(x - 1, y - 1).R, bmp.GetPixel(x - 1, y - 1).G, bmp.GetPixel(x - 1, y - 1).B);
                    return color;
                case 3:
                    color = Color.FromArgb(bmp.GetPixel(x, y - 1).R, bmp.GetPixel(x, y - 1).G, bmp.GetPixel(x, y - 1).B);
                    return color;
                case 4:
                    color = Color.FromArgb(bmp.GetPixel(x + 1, y - 1).R, bmp.GetPixel(x + 1, y - 1).G, bmp.GetPixel(x + 1, y - 1).B);
                    return color;
                default:
                    return Color.Black;
            }


        }

        //Clear
        private Bitmap indexPixel(Bitmap bmp, int x , int y)
        {
            if(this.B < 255)
            {
                this.B = this.B + 1;
            }
            else if (this.G < 255)
            {
                this.G = this.G + 1;
            }else if(this.R < 255)
            {
                this.R = this.R + 1;
            }

            bmp.SetPixel(x, y, Color.FromArgb(R, G, B));
            return bmp;
        }


        //CLear
        private bool caseHandeler(Bitmap bmp, int x , int y)
        {
           
            if(bmp.GetPixel(x-1,y) != Color.FromArgb(255,255,255))
            {
                return true;
            }else if(bmp.GetPixel(x - 1, y-1) != Color.FromArgb(255, 255, 255))
            {
                return true;
            }
            else if (bmp.GetPixel(x , y - 1) != Color.FromArgb(255, 255, 255))
            {
                return true;
            }
            else if (bmp.GetPixel(x + 1, y - 1) != Color.FromArgb(255, 255, 255))
            {
                return true;
            }

            return false;
        }

        
        //Clear
        private Bitmap greyScale(Bitmap bmp)
        {
            for (int w = 0; w < bmp.Width; w++)
            {
                for (int h = 0; h < bmp.Height; h++)
                {
                    int total;
                    int red = bmp.GetPixel(w, h).R;
                    int green = bmp.GetPixel(w, h).G;
                    int blue = bmp.GetPixel(w, h).B;

                    total = (red + green + blue) / 3;

                    bmp.SetPixel(w, h, Color.FromArgb(total, total, total));
                }
            }

            for (int w = 0; w < bmp.Width; w++)
            {
                for (int h = 0; h < bmp.Height; h++)
                {
                    if (bmp.GetPixel(w, h) == Color.Black || bmp.GetPixel(w, h).R < 200)
                    {
                        bmp.SetPixel(w, h, Color.FromArgb(0,0,0));
                    }
                    else
                    {
                        bmp.SetPixel(w, h, Color.White);
                    }
                }
            }

            return bmp;
        }
    }


    class Obj
    {
        public LinkedList<Color> list = new LinkedList<Color>();
    }
}
