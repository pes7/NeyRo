using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parabollian;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*
         * y=sqrt(10^4-x^2)+abs(tg(x)*k)-m*2
         * y=sin(x)*m+abs(tg(x)*k)-m*2
         */
        Bitmap original;
        OLua lua = new OLua();
        string image = "11.png";
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = $"{255 - trackBar1.Value}";
            lua.Lua["y"] = 0f;
            lua.Lua["x"] = 0f;
            lua.Lua["step"] = 0.2f;
            original = new Bitmap(Image.FromFile(image, true));
        }

        Bitmap green, blue, red, post;
        private void button1_Click(object sender, EventArgs e)
        {
            bool i1 = false, i2 = false, i3 = false;
            startTh(() => { green = GenColor(new Bitmap(Image.FromFile(image, true)), Specture.Green); i1 = true; });
            startTh(() => { blue = GenColor(new Bitmap(Image.FromFile(image, true)), Specture.Blue); i2 = true; });
            startTh(() => { red = GenColor(new Bitmap(Image.FromFile(image, true)), Specture.Red); i3 = true; });
            while (true)
            {
                if (i1 == true && i2 == true && i3 == true)
                {
                    post = new Bitmap(1, 1);
                    Show();
                    break;
                }
            }
        }

        private void CreateGraphic()
        {
            float start_x = original.Width / 2, start_y = original.Height / 2;
            float y = 0;
            float x = 0;
            float step = 1f;
            string s = lua.Lua["step"].ToString();
            float.TryParse(s, out step);
            Brush pn = Brushes.Black;
            Bitmap bt = new Bitmap(original.Width, original.Height);
            Graphics gr = Graphics.FromImage(bt);
            for (x = -start_x; x <= start_x; x = x + step)
            {
                lua.Lua["x"] = x;
                lua.Lua.DoString(textBox1.Text);
                object j = lua.Lua["y"];
                y = float.Parse(j.ToString());
                if(!float.IsInfinity(y) && start_y +- y < bt.Height && start_y +- y > 0)
                    gr.FillRectangle(pn, new RectangleF(new PointF(start_x + x, start_y + -y), new SizeF(1, 1)));
            }
            pictureBox5.Image = bt;
        }

        
        private Bitmap changeOrig(Bitmap green, Bitmap blue, Bitmap red)
        {
            Bitmap bt = new Bitmap(original.Width,original.Height, PixelFormat.Format32bppArgb);
            //bt.MakeTransparent(Color.FromArgb(0,255,255,255));
            /*
            switch (Choosed)
            {
                case Specture.Red:
                    bt = new Bitmap(red); 
                    break;
                case Specture.Blue:
                    bt = new Bitmap(blue);
                    break;
                case Specture.Green:
                    bt = new Bitmap(green);
                    break;
            }
            */
            float start_x = original.Width / 2, start_y = original.Height / 2;
            float y = 0;
            float x = 0;
            float step = 1f;
            string s = lua.Lua["step"].ToString();
            float.TryParse(s, out step);
            Brush pn = Brushes.Black;
            for (x = -start_x; x < start_x; x = x + step)
            {
                lua.Lua["x"] = x;
                lua.Lua.DoString(textBox1.Text);
                object j = lua.Lua["y"];
                y = float.Parse(j.ToString());
                //Choose color
                float coef = 0.8f;
                if (!float.IsInfinity(y))
                {
                    var ky = (int)(start_y + (-1) * y);
                    var kx = (int)(start_x + x);
                    if (ky >= 0 && ky < bt.Height && kx >= 0 && kx < bt.Width)
                    {
                        /*
                        var sB = blue.GetPixel(kx, ky);
                        var sG = green.GetPixel(kx, ky);
                        var sR = red.GetPixel(kx, ky);

                        int r_n = og1.R + (int)(dss.R * coef);
                        int r = r_n > 255 ? 255 : r_n < 0 ? 0 : r_n;
                        int g_n = og1.G + (int)(dss.G * coef);
                        int g = r_n > 255 ? 255 : r_n < 0 ? 0 : r_n;
                        int b_n = og1.B + (int)(dss.B * coef);
                        int b = r_n > 255 ? 255 : r_n < 0 ? 0 : r_n;
                        */
                        var dss = original.GetPixel(kx,ky);
                        var og1 = bt.GetPixel(kx, ky);
                        int r = (int)(dss.R );
                        int g = (int)(dss.G );
                        int b =(int)(dss.B );
                        if (!dss.IsEmpty)
                        {
                            var c = Color.FromArgb(r, g, b);
                            bt.SetPixel((int)(start_x + x), (int)(start_y + (-1) * y), c);
                        }
                    }
                }
            }
            return bt;
        } 
        

        private void startTh(Action act)
        {
            Thread th = new Thread(new ThreadStart(act));
            th.Start();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ColorCut = 255 - trackBar1.Value;
            label1.Text = $"{ColorCut}";
        }
        private int ColorCut = 255;

        private void button2_Click(object sender, EventArgs e)
        {
            up();
            CreateGraphic();
        }

        private void up()
        {
            lua.Lua["step"] = 1f / float.Parse(textBox2.Text);
            lua.Lua["k"] = trackBar3.Value / 100f;
            lua.Lua["m"] = trackBar2.Value;
        }

        Specture Choosed = Specture.Red;
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Choosed = Specture.Red;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Choosed = Specture.Blue;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Choosed = Specture.Green;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            up();
            post = changeOrig(green,blue,red);
            Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox4.Image.Save("u.png");
        }

        private void Show()
        {
            if(green != null && blue != null && red != null && post != null)
            {
                pictureBox1.Image = green;
                pictureBox2.Image = blue;
                pictureBox3.Image = red;
                pictureBox4.Image = post;
            }
        }

        public enum Specture { Green, Blue, Red};
        public Bitmap GenColor(Bitmap orig, Nullable<Specture> color = null)
        {
            Bitmap bt = new Bitmap(orig.Width, orig.Height);
            if(color == null)
            {
                bt = new Bitmap(orig);
                return bt;
            }
            byte sp;
            for (int i = 0; i < orig.Height; i++)
            {
                for (int j = 0; j < orig.Width; j++)
                {
                    switch (color.Value)
                    {
                        case Specture.Green:
                           sp = orig.GetPixel(j, i).G;
                            if (sp > ColorCut)
                                sp = 0;
                            bt.SetPixel(j,i, Color.FromArgb(0, sp, 0));
                            break;
                        case Specture.Blue:
                            sp = orig.GetPixel(j, i).B;
                            if (sp > ColorCut)
                                sp = 0;
                            bt.SetPixel(j, i, Color.FromArgb(0, 0, sp));
                            break;
                        case Specture.Red:
                            sp = orig.GetPixel(j, i).R;
                            if (sp > ColorCut)
                                sp = 0;
                            bt.SetPixel(j, i, Color.FromArgb(sp, 0, 0));
                            break;
                    }
                }
            }
            return bt;
        }
    }
}
