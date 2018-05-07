using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp1.Form1;

namespace NeyRo
{
    [Serializable]
    public class Ney
    {
        public int[,] weight_r;
        public int[,] weight_b;
        public int[,] weight_g;
        public int[] size;
        private const float limit = 255f;
        public string name;
        public int sum;
        public GImage Gm;
        public Ney(int sizex, int sizey, string nam, GImage gm)
        {
            name = nam;
            size = new[] { sizex, sizey };
            weight_r = new int[sizex, sizey];
            weight_b = new int[sizex, sizey];
            weight_g = new int[sizex, sizey];
            Gm = gm;
        }

        public float SumCheck(int[,] input_r, int[,] input_b, int[,] input_g) {
            float sum = 0;
            for (int i = 0; i < size[1]; i++)
            {
                for (int j = 0; j < size[0]; j++)
                {
                    sum = input_r[i, j] == weight_r[i, j] ? weight_r[i, j] * rate1T : -weight_r[i, j] * rate1T;
                }
            }
            for (int i = 0; i < size[1]; i++)
            {
                for (int j = 0; j < size[0]; j++)
                {
                    sum = input_b[i, j] == weight_b[i, j] ? weight_b[i, j] * rate1T : -weight_r[i, j] * rate1T;
                }
            }
            for (int i = 0; i < size[1]; i++)
            {
                for (int j = 0; j < size[0]; j++)
                {
                    sum = input_g[i, j] == weight_g[i, j] ? weight_g[i, j] * rate1T : -weight_r[i, j] * rate1T;
                }
            }
            return sum;
        }

        public void STUDYKPISUKABLET(int[,] input_r, int[,] input_b, int[,] input_g, bool iser, bool isittest = false)
        {
            if (iser)
            {
                for (int i = 0; i < size[1]; i++)
                {
                    for (int j = 0; j < size[0]; j++)
                    {
                        weight_r[i, j] = (input_r[i, j] + weight_r[i, j]) / 2;
                    }
                }
                for (int i = 0; i < size[1]; i++)
                {
                    for (int j = 0; j < size[0]; j++)
                    {
                        weight_b[i, j] = (input_b[i, j] + weight_b[i, j]) / 2;
                    }
                }
                for (int i = 0; i < size[1]; i++)
                {
                    for (int j = 0; j < size[0]; j++)
                    {
                        weight_g[i, j] = (input_g[i, j] + weight_g[i, j]) / 2;
                    }
                }
            }
            else
            {
                if (!isittest)
                {
                    for (int i = 0; i < size[1]; i++)
                    {
                        for (int j = 0; j < size[0]; j++)
                        {
                            weight_r[i, j] -= (int)(input_r[i, j] * rate2F);
                        }
                    }
                    for (int i = 0; i < size[1]; i++)
                    {
                        for (int j = 0; j < size[0]; j++)
                        {
                            weight_b[i, j] -= (int)(input_b[i, j] * rate2F);
                        }
                    }
                    for (int i = 0; i < size[1]; i++)
                    {
                        for (int j = 0; j < size[0]; j++)
                        {
                            weight_g[i, j] -= (int)(input_g[i, j] * rate2F);
                        }
                    }
                }
            }
        }

        public Bitmap CreateImage(spector sp)
        {
            Bitmap bt = new Bitmap(size[0], size[1]);
            switch (sp) {
                case spector.red:
                    for (var x = 0; x < size[0]; x++)
                    {
                        for (var y = 0; y < size[1]; y++)
                        {
                            bt.SetPixel(y, x, Color.FromArgb(weight_r[y, x] > 225 ? 225 : weight_r[y, x] < 0 ? 0 : weight_r[y, x],0,0));
                        }
                    }
                    return bt;
                case spector.blue:
                    for (var x = 0; x < size[0]; x++)
                    {
                        for (var y = 0; y < size[1]; y++)
                        {
                            bt.SetPixel(y, x, Color.FromArgb(0, weight_b[y, x] > 225 ? 225 : weight_b[y, x] < 0 ? 0 : weight_b[y, x], 0));
                        }
                    }
                    return bt;
                case spector.green:
                    for (var x = 0; x < size[0]; x++)
                    {
                        for (var y = 0; y < size[1]; y++)
                        {
                            bt.SetPixel(y, x, Color.FromArgb(0, 0, weight_g[y, x] > 225 ? 225 : weight_g[y, x] < 0 ? 0 : weight_g[y, x]));
                        }
                    }
                    return bt;
                case spector.rgb:
                    for (var x = 0; x < size[0]; x++)
                    {
                        for (var y = 0; y < size[1]; y++)
                        {
                            bt.SetPixel(y, x, Color.FromArgb(weight_r[y, x] > 225 ? 225 : weight_r[y, x] < 0 ? 0 : weight_r[y, x], weight_b[y, x] > 225 ? 225 : weight_b[y, x] < 0 ? 0 : weight_b[y, x], weight_g[y, x] > 225 ? 225 : weight_g[y, x] < 0 ? 0 : weight_g[y, x]));
                        }
                    }
                    return bt;
                default:
                    return bt;
            }
        }

        public bool Res(float sum){
            return sum > limit ? true : false;
        }

        public override string ToString()
        {
            return $"{name}";
        }

    }
}
