using System;
using System.Collections.Generic;
using System.Text;

namespace IAP_Core
{
    public class ComplexImage : Image
    {
        public struct Complex
        {
            public double Re;
            public double Im;

            public Complex(double re, double im)
            {
                this.Re = re;
                this.Im = im;
            }
            public Complex(Complex c)
            {
                this.Re = c.Re;
                this.Im = c.Im;
            }

            public static Complex Zero
            {
                get { return new Complex(0, 0); }
            }

            public static Complex operator +(Complex a, Complex b)
            {
                return new Complex(a.Re + b.Re, a.Im + b.Im);
            }
            public static Complex operator -(Complex a, Complex b)
            {
                return new Complex(a.Re - b.Re, a.Im - b.Im);
            }
            public static Complex operator *(Complex a, Complex b)
            {
                return new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);
            }
            public static Complex operator /(Complex a, Complex b)
            {
                double divider = b.Re * b.Re + b.Im * b.Im;

                if (divider == 0)
                    throw new DivideByZeroException();

                return new Complex((a.Re * b.Re + a.Im * b.Im) / divider, (a.Im * b.Re - a.Re * b.Im) / divider);
            }

            public double Magnitude
            {
                get { return (double)System.Math.Sqrt(Re * Re + Im * Im); }
            }
            public double SquaredMagnitude
            {
                get { return (Re * Re + Im * Im); }
            }
            public double Phase
            {
                get { return (double)System.Math.Atan(Im / Re); }
            }
        }

        private Complex[,] data, databackup;

        public void Set(int min, int max, double re, double im)
        {
            this.databackup = new Complex[this.data.GetLength(0), this.data.GetLength(1)];

            for (int i = 0; i < this.databackup.GetLength(0); i++)
                for (int j = 0; j < this.databackup.GetLength(1); j++)
                    this.databackup[i, j] = this.data[i, j];

            int hw = this.Width >> 1;
            int hh = this.Height >> 1;

            for (int i = 0; i < this.Height; i++)
            {
                int y = i - hh;

                for (int j = 0; j < this.Width; j++)
                {
                    int x = j - hw;
                    int d = (int)Math.Sqrt(x * x + y * y);

                    if ((d > max) || (d < min))
                    {
                        this.data[i, j].Re = re;
                        this.data[i, j].Im = im;
                    }
                }
            }
        }
        public void Add(int min, int max, double re, double im)
        {
            this.databackup = new Complex[this.data.GetLength(0), this.data.GetLength(1)];

            for (int i = 0; i < this.databackup.GetLength(0); i++)
                for (int j = 0; j < this.databackup.GetLength(1); j++)
                    this.databackup[i, j] = this.data[i, j];

            int hw = this.Width >> 1;
            int hh = this.Height >> 1;

            for (int i = 0; i < this.Height; i++)
            {
                int y = i - hh;

                for (int j = 0; j < this.Width; j++)
                {
                    int x = j - hw;
                    int d = (int)Math.Sqrt(x * x + y * y);

                    if ((d > max) || (d < min))
                    {
                        this.data[i, j].Re += re;
                        this.data[i, j].Im += im;
                    }
                }
            }
        }
        public void Mult(int min, int max, double re, double im)
        {
            this.databackup = new Complex[this.data.GetLength(0), this.data.GetLength(1)];

            for (int i = 0; i < this.databackup.GetLength(0); i++)
                for (int j = 0; j < this.databackup.GetLength(1); j++)
                    this.databackup[i, j] = this.data[i, j];

            int hw = this.Width >> 1;
            int hh = this.Height >> 1;

            for (int i = 0; i < this.Height; i++)
            {
                int y = i - hh;

                for (int j = 0; j < this.Width; j++)
                {
                    int x = j - hw;
                    int d = (int)Math.Sqrt(x * x + y * y);

                    if ((d > max) || (d < min))
                    {
                        this.data[i, j].Re *= re;
                        this.data[i, j].Im *= im;
                    }
                }
            }
        }

        public void Rollback()
        {
            this.data = new Complex[this.databackup.GetLength(0), this.databackup.GetLength(1)];

            for (int i = 0; i < this.data.GetLength(0); i++)
                for (int j = 0; j < this.data.GetLength(1); j++)
                    this.data[i, j] = this.databackup[i, j];
        }

        public void Update()
        {
            this.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);

            double scale = Math.Sqrt(this.Width * this.Height);

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                    this.SetPixel(x, (byte)Math.Max(Math.Min(this.data[y, x].Magnitude * scale * 255.0, 255.0), 0.0));

                this.IncLine();
            }

            this.CloseData();
        }

        private void DFT2(bool inverse)
        {
            int n = this.data.GetLength(0);
            int m = this.data.GetLength(1);

            Complex[] dst = new Complex[Math.Max(n, m)];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    dst[j] = Complex.Zero;

                    double arg = -(int)(inverse ? -1 : 1) * 2.0 * Math.PI * (double)j / (double)m;

                    for (int k = 0; k < m; k++)
                    {
                        double cos = Math.Cos(k * arg);
                        double sin = Math.Sin(k * arg);

                        dst[j].Re += (float)(data[i, k].Re * cos - data[i, k].Im * sin);
                        dst[j].Im += (float)(data[i, k].Re * sin + data[i, k].Im * cos);
                    }
                }

                if (!inverse)
                {
                    for (int j = 0; j < m; j++)
                    {
                        data[i, j].Re = dst[j].Re / m;
                        data[i, j].Im = dst[j].Im / m;
                    }
                }
                else
                {
                    for (int j = 0; j < m; j++)
                    {
                        data[i, j].Re = dst[j].Re;
                        data[i, j].Im = dst[j].Im;
                    }
                }
            }

            for (int j = 0; j < m; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    dst[i] = Complex.Zero;

                    double arg = -(int)(inverse ? -1 : 1) * 2.0 * Math.PI * (double)i / (double)n;

                    for (int k = 0; k < n; k++)
                    {
                        double cos = System.Math.Cos(k * arg);
                        double sin = System.Math.Sin(k * arg);

                        dst[i].Re += (float)(data[k, j].Re * cos - data[k, j].Im * sin);
                        dst[i].Im += (float)(data[k, j].Re * sin + data[k, j].Im * cos);
                    }
                }

                if (!inverse)
                {
                    for (int i = 0; i < n; i++)
                    {
                        data[i, j].Re = dst[i].Re / n;
                        data[i, j].Im = dst[i].Im / n;
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        data[i, j].Re = dst[i].Re;
                        data[i, j].Im = dst[i].Im;
                    }
                }
            }
        }

        public Image ToImage()
        {
            DFT2(true);

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    if (((x + y) & 0x1) != 0)
                    {
                        this.data[y, x].Re *= -1.0;
                        this.data[y, x].Im *= -1.0;
                    }
                }
            }

            Image img = new Image();
            img.Create(this.Width, this.Height, this.Bitmap.Palette);

            img.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                    img.SetPixel(x, (byte)Math.Max(Math.Min(this.data[y, x].Magnitude * 255.0, 255.0), 0.0));

                img.IncLine();
            }

            img.CloseData();

            return img;
        }

        public static ComplexImage FromImage(Image image)
        {
            ComplexImage cimg = new ComplexImage();
            cimg.Create(image.Width, image.Height, image.Bitmap.Palette);
            cimg.data = new Complex[image.Height, image.Width];

            image.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

            for (int y = 0; y < cimg.Height; y++)
            {
                for (int x = 0; x < cimg.Width; x++)
                    cimg.data[y, x].Re = (double)image.GetPixel(x) / 255.0;

                image.IncLine();
            }

            image.CloseData();

            for (int y = 0; y < cimg.Height; y++)
            {
                for (int x = 0; x < cimg.Width; x++)
                {
                    if (((x + y) & 0x1) != 0)
                    {
                        cimg.data[y, x].Re *= -1.0;
                        cimg.data[y, x].Im *= -1.0;
                    }
                }
            }

            cimg.DFT2(false);

            cimg.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);

            double scale = Math.Sqrt(cimg.Width * cimg.Height);

            for (int y = 0; y < cimg.Height; y++)
            {
                for (int x = 0; x < cimg.Width; x++)
                    cimg.SetPixel(x, (byte)Math.Max(Math.Min(cimg.data[y, x].Magnitude * scale * 255.0, 255.0), 0.0));

                cimg.IncLine();
            }

            cimg.CloseData();

            return cimg;
        }
    }
}
