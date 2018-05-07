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
using NeyRo;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public static float rate1T = 1.3f, rate2F = 1f;
        public enum spector { red, green, blue,rgb };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rand = new Random();
        }

        public class GImage
        {
            public int[,] bits_r;
            public int[,] bits_b;
            public int[,] bits_g;
            public string str { get; set; }
            public Bitmap Bit { get; set; }
            public Size Size { get; set; }
            public GImage(Bitmap bit, Size size, string name)
            {
                Bit = bit;
                str = name;
                Size = size;
                Parse();
            }
            private void Parse()
            {
                bits_r = new int[Size.Width, Size.Height];
                bits_b = new int[Size.Width, Size.Height];
                bits_g = new int[Size.Width, Size.Height];
                for (var x = 0; x < ((Bit.Width < Size.Width) ? Bit.Width : Size.Width); x++)
                {
                    for (var y = 0; y < ((Bit.Height < Size.Height) ? Bit.Height : Size.Height); y++)
                    {
                        bits_r[x, y] = getN(spector.red, x, y);
                        bits_b[x, y] = getN(spector.blue, x, y);
                        bits_g[x, y] = getN(spector.green, x, y);
                    }
                }
            }

            private int getN(spector sp,int x, int y)
            {
                int d = 0;
                try
                {
                    switch (sp)
                    {
                        case spector.blue:
                            d = (Bit.GetPixel(x, y).B);
                            if (d >= 170)
                                d = 0;
                            return d;
                        case spector.red:
                            d = (Bit.GetPixel(x, y).R);
                            if (d >= 170)
                                d = 0;
                            return d;
                        case spector.green:
                            d = (Bit.GetPixel(x, y).G);
                            if (d >= 170)
                                d = 0;
                            return d;
                        default:
                            return 0;
                    }
                }
                catch
                {
                    return d;
                }
            }
        }

        private List<GImage> GetBitFromImage(string dir)
        {
            List<GImage> gimage = new List<GImage>();
            var files = Directory.GetFiles(dir, ".");
            foreach (var file in files)
            {
                Bitmap bit = Bitmap.FromFile(file) as Bitmap;
                gimage.Add(new GImage(bit, SizeOfPic, Path.GetFileName(file)));
            }
            return gimage;
        }

        public Size SizeOfPic = new Size(500, 500);
        private List<Ney> Neyrons { get; set; }
        private List<string> Images { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            Images = Directory.GetFiles("./image", ".").ToList();
            Neyrons = new List<Ney>();
            var f = GetBitFromImage("./image");
            int j = 0;
            foreach (var i in f)
            {
                Neyrons.Add(new Ney(SizeOfPic.Width, SizeOfPic.Height, i.str, i));
                j++;
            }
            listBox1.Items.AddRange(Neyrons.ToArray());
        }

        int Trues, Falses;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trues = 0;
            Falses = 0;
            pictureBox5.Image = Neyrons[listBox1.SelectedIndex].Gm.Bit;
            SelectedName = Neyrons[listBox1.SelectedIndex].name;
        }

        private Random rand;
        private int randInt;
        private string randName;
        private void LoadNextImage()
        {
            int r = rand.Next(0, Neyrons.Count);
            randInt = r;
            randName = Neyrons[r].name;
            recognize();
        }

        private string SelectedName;

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadNextImage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var ney = Neyrons[listBox1.SelectedIndex];
            pictureBox1.Image = ney.CreateImage(spector.red);
            pictureBox2.Image = ney.CreateImage(spector.blue);
            pictureBox3.Image = ney.CreateImage(spector.green);
            pictureBox4.Image = ney.CreateImage(spector.rgb);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = $"{trackBar1.Value}";
            rate1T = trackBar1.Value / 100f;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if(openFileDialog1.FileName != null)
            {
                Bitmap bit = Bitmap.FromFile(openFileDialog1.FileName) as Bitmap;
                var str = Path.GetFileName(openFileDialog1.FileName);
                var f = new Ney(Size.Width, Size.Height,str, new GImage(bit, Size, str));
                var ney = Neyrons[listBox1.SelectedIndex];
                float sum = ney.SumCheck(f.Gm.bits_r, f.Gm.bits_b, f.Gm.bits_g);
                MessageBox.Show(ney.Res(sum) ? "COOL" : "SUCK DICK");
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = $"{trackBar2.Value}";
            rate2F = trackBar2.Value / 100f;
        }

        public void recognize()
        {
            var ney = Neyrons[listBox1.SelectedIndex];
            float sum = ney.SumCheck(Neyrons[randInt].Gm.bits_r, Neyrons[randInt].Gm.bits_b, Neyrons[randInt].Gm.bits_g);
            if (SelectedName == randName)
                ney.STUDYKPISUKABLET(Neyrons[randInt].Gm.bits_r, Neyrons[randInt].Gm.bits_b, Neyrons[randInt].Gm.bits_g, true);
            else 
                ney.STUDYKPISUKABLET(Neyrons[randInt].Gm.bits_r, Neyrons[randInt].Gm.bits_b, Neyrons[randInt].Gm.bits_g, false);

            if (Neyrons[listBox1.SelectedIndex].Res(sum)) label1.Text = $" - True, Sum = {sum}";
            else label1.Text = $" - False, Sum = {sum}";
            float rate = (Trues != 0 && Falses != 0) ? Trues / Falses * 100f : 0f;
            if (SelectedName == randName) label2.Text = $"Real: true. AccesRate:{rate}%. Trues:{Trues}. Fales:{Falses}";
            else label2.Text = $"Real: false. AccesRate:{rate}%. Trues:{Trues}. Fales:{Falses}";
            if (ney.Res(sum) == (SelectedName == randName))
                Trues++;
            else Falses++;
        }
    }
}
