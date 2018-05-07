using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var Images = Directory.GetFiles("./image", ".").ToList();
            listBox1.Items.AddRange(Images.ToArray());
        }

        private Size SizeOfPhoto = new Size(160, 160);
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap bt = new Bitmap(SizeOfPhoto.Width, SizeOfPhoto.Height);
            Image org = Image.FromFile(listBox1.SelectedItem.ToString());
            var d = (org as Bitmap);
            for (int i = 0; i < SizeOfPhoto.Height; i++)
            {
                for (int j = 0; j < SizeOfPhoto.Width; j++)
                {
                    if ((i * j) % 2 == 0)
                        bt.SetPixel(i, j, Color.FromArgb(d.GetPixel(i, j).ToArgb()));
                    else
                        bt.SetPixel(i, j, Color.White);
                }
            }
            bt =redRose(bt);
            pictureBox1.Image = bt;
            bt.Save("lol.png");
        }

        private Bitmap redRose(Bitmap iner)
        {
            var st = @"0001000" + "\n" +
                      "0012100" + "\n" +
                      "0001000";
            const int line = 7, spline = 3;
            Point d = new Point(0, 0);
            for (int k = 0; k < SizeOfPhoto.Width; k += line)
            {
                for (int s = 0; s < SizeOfPhoto.Height; s += spline)
                {
                    for (int i = 0; i <= spline; i++)
                    {
                        for (int j = 0; j <= line; j++)
                        {
                            if (d.Y + i < SizeOfPhoto.Height && d.X + j < SizeOfPhoto.Width) {
                                switch (st[i + j]) {
                                    case '1':
                                        iner.SetPixel(d.X + j, d.Y + i, Color.Red);
                                        break;
                                    case '2':
                                        iner.SetPixel(d.X + j, d.Y + i, Color.Blue);
                                        break;
                                }
                            }
                        }
                    }
                    d.Y = s;
                }
                d.Y = 0;
                d.X = k;
            }
            return iner;
        }
    }
}
