using System;
using System.Collections.Generic;
using System.Text;

namespace IAP_Core
{
    public sealed class ImageOperations
    {
        public static Image Image, ImageBackup;
        public static System.Drawing.Rectangle Rect;

        public static void Resize(int width, int height, bool interpolation)
        {
            ImageBackup = new Image(Image.Bitmap);

            Image.Create(width, height, ImageBackup.Bitmap.Palette);

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);
            ImageBackup.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

            double yf = (double)ImageBackup.Height / (double)Image.Height;
            double xf = (double)ImageBackup.Width / (double)Image.Width;

            for (int y = 0; y < Image.Height; y++)
            {
                for (int x = 0; x < Image.Width; x++)
                {
                    if (interpolation)
                    {
                        double fx = ((double)x / (double)(Image.Width - 1)) * (double)(ImageBackup.Width - 1);
                        double fy = ((double)y / (double)(Image.Height - 1)) * (double)(ImageBackup.Height - 1);
                        int x0 = (int)Math.Floor(fx);
                        int x1 = (int)Math.Ceiling(fx);
                        int y0 = (int)Math.Floor(fy);
                        int y1 = (int)Math.Ceiling(fy);
                        double fu = fx - x0;
                        double fv = fy - y0;

                        int val = (int)((1.0 - fu) * (1.0f - fv) * (double)ImageBackup.GetPixel(x0, y0) + fu * (1.0 - fv) * (double)ImageBackup.GetPixel(x1, y0) + (1.0 - fu) * fv * (double)ImageBackup.GetPixel(x0, y1) + fu * fv * (double)ImageBackup.GetPixel(x1, y1));

                        if (val < 0)
                            val = 0;
                        else if (val > 255)
                            val = 255;

                        Image.SetPixel(x, (byte)val);
                    }
                    else
                        Image.SetPixel(x, ImageBackup.GetPixel((int)((double)x * xf), (int)((double)y * yf)));
                }

                Image.IncLine();
            }

            ImageBackup.CloseData();
            Image.CloseData();
        }
        public static void Flip()
        {
            ImageBackup = new Image(Image.Bitmap);

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);
            ImageBackup.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, true);

            Image.IncLine(Rect.Y);
            ImageBackup.DecLine(Rect.Y);

            for (int y = 0; y < Rect.Height; y++)
            {
                ImageBackup.DecLine();

                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                    Image.SetPixel(x, ImageBackup.GetPixel(x));

                Image.IncLine();
            }

            ImageBackup.CloseData();
            Image.CloseData();
        }
        public static void Mirror()
        {
            ImageBackup = new Image(Image.Bitmap);

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);
            ImageBackup.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

            Image.IncLine(Rect.Y);
            ImageBackup.IncLine(Rect.Y);

            for (int y = 0; y < Rect.Height; y++)
            {
                int xx = 0;
                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                {
                    Image.SetPixel(x, ImageBackup.GetPixel((Rect.X + Rect.Width - 1) + xx));

                    xx--;
                }
                Image.IncLine();
                ImageBackup.IncLine();
            }

            ImageBackup.CloseData();
            Image.CloseData();
        }
        public static void Clamp(byte low, byte high)
        {
            ImageBackup = new Image(Image.Bitmap);

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.ReadWrite, false);

            Image.IncLine(Rect.Y);

            for (int y = 0; y < Rect.Height; y++)
            {
                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                    Image.SetPixel(x, Math.Max(Math.Min(Image.GetPixel(x), high), low));

                Image.IncLine();
            }

            Image.CloseData();
        }
        public static void Normalization()
        {
            ImageBackup = new Image(Image.Bitmap);

            byte min = 255, max = 0;

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

            Image.IncLine(Rect.Y);

            for (int y = 0; y < Rect.Height; y++)
            {
                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                {
                    byte val = Image.GetPixel(x);

                    min = Math.Min(min, val);
                    max = Math.Max(max, val);
                }

                Image.IncLine();
            }

            Image.CloseData();

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);

            Image.IncLine(Rect.Y);

            for (int y = 0; y < Rect.Height; y++)
            {
                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                    Image.SetPixel(x, (byte)((double)(Image.GetPixel(x) - min) * (255.0 / (double)(max - min))));

                Image.IncLine();
            }

            Image.CloseData();
        }
        public static void Equalization()
        {
            ImageBackup = new Image(Image.Bitmap);

            int[] histogram = Image.Histogram;
            double lenres = (double)histogram.Length / (double)(Image.Width * Image.Height);
            int[] LUT = new int[256];

            LUT[0] = (int)((double)histogram[0] * lenres);
            int prev = histogram[0];

            for (int i = 1; i < LUT.Length; i++)
            {
                prev += histogram[i];
                LUT[i] = (int)((double)prev * lenres);
            }

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.ReadWrite, false);

            Image.IncLine(Rect.Y);

            for (int y = 0; y < Rect.Height; y++)
            {
                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                    Image.SetPixel(x, Math.Min((byte)LUT[Image.GetPixel(x)], (byte)255));

                Image.IncLine();
            }

            Image.CloseData();
        }
        public static void Convolution(string[] kernel, int div, bool absolute)
        {
            if (kernel != null)
            {
                ImageBackup = new Image(Image.Bitmap);

                Image.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);
                ImageBackup.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

                Image.IncLine(Rect.Y);

                for (int y = 0; y < Rect.Height; y++)
                {
                    for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                    {
                        int val = 0;

                        for (int yy = 0; yy < kernel.Length; yy++)
                        {
                            string[] cols = kernel[yy].Split(' ');
                            for (int xx = 0; xx < cols.Length; xx++)
                            {
                                int ke = int.Parse(cols[xx]);

                                if (ke != 0)
                                {
                                    int sy = y + Rect.Y + (yy - (int)Math.Floor((double)kernel.Length / 2.0));
                                    int sx = x + (xx - (int)Math.Floor((double)cols.Length / 2.0));

                                    if (sy < 0)
                                        sy = 0;
                                    else if (sy >= ImageBackup.Height)
                                        sy = ImageBackup.Height - 1;
                                    if (sx < 0)
                                        sx = 0;
                                    else if (sx >= ImageBackup.Width)
                                        sx = ImageBackup.Width - 1;

                                    val += ke * (int)ImageBackup.GetPixel(sx, sy);
                                }
                            }
                        }

                        val = (int)((double)val / (double)div);

                        if (absolute)
                            Image.SetPixel(x, (byte)Math.Min(Math.Abs(val), 255));
                        else
                            Image.SetPixel(x, (byte)Math.Max(Math.Min(val, 255), 0));
                    }

                    Image.IncLine();
                }

                ImageBackup.CloseData();
                Image.CloseData();
            }
        }
        public static void Median(int size)
        {
            ImageBackup = new Image(Image.Bitmap);

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, false);
            ImageBackup.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

            Image.IncLine(Rect.Y);

            int med = (size * size) / 2;

            for (int y = 0; y < Rect.Height; y++)
            {
                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                {
                    List<byte> bytes = new List<byte>();

                    for (int yy = 0; yy < size; yy++)
                    {
                        for (int xx = 0; xx < size; xx++)
                        {
                            int sy = y + Rect.Y + (yy - (int)Math.Floor((double)size / 2.0));
                            int sx = x + (xx - (int)Math.Floor((double)size / 2.0));

                            if (sy < 0)
                                sy = 0;
                            else if (sy >= ImageBackup.Height)
                                sy = ImageBackup.Height - 1;
                            if (sx < 0)
                                sx = 0;
                            else if (sx >= ImageBackup.Width)
                                sx = ImageBackup.Width - 1;

                            bytes.Add(ImageBackup.GetPixel(sx, sy));
                        }
                    }

                    bytes.Sort();

                    Image.SetPixel(x, bytes[med]);
                }

                Image.IncLine();
            }

            ImageBackup.CloseData();
            Image.CloseData();
        }
        public static void Invert()
        {
            ImageBackup = new Image(Image.Bitmap);

            Image.OpenData(System.Drawing.Imaging.ImageLockMode.ReadWrite, false);

            Image.IncLine(Rect.Y);

            for (int y = 0; y < Rect.Height; y++)
            {
                for (int x = Rect.X; x < Rect.X + Rect.Width; x++)
                    Image.SetPixel(x, (byte)(255 - (int)Image.GetPixel(x)));

                Image.IncLine();
            }

            Image.CloseData();
        }
    }
}
