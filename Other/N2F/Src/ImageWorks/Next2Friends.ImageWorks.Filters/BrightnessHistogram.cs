using System;
using System.Windows.Media.Imaging;

namespace Next2Friends.ImageWorks.Filters
{
    public class BrightnessHistogram : Histogram
    {
        public BrightnessHistogram(BitmapSource source) : base(source){}

        protected override void MakeHistogram()
        {
            byte[] bytes = new byte[Source.PixelWidth * Source.PixelHeight * 4];
            Source.CopyPixels(bytes, Source.PixelWidth * 4, 0);

            byte r, g, b, l, v, m;

            for (int i = 0; i < bytes.Length; i += 4)
            {
                r = bytes[i + 2];
                g = bytes[i + 1];
                b = bytes[i];

                v = Math.Max(r, g);
                v = Math.Max(v, b);

                m = Math.Min(r, g);
                m = Math.Min(m, b);

                l = (byte)((m + v) / 2.0);

                this[l].Add(new ByteRGB(r, g, b));
            }
        }

        public int Min
        {
            get
            {
                for (int i = 0; i < 256; i++)
                {
                    //Check for a non-trivial amount of pixels
                    if (this[i].Count > 5)
                    {
                        return i;
                    }
                }

                return 0;
            }
        }

        public int Max
        {
            get
            {
                for (int i = 255; i >= 0; i--)
                {
                    //Check for a non-trivial amount of pixels
                    if (this[i].Count > 5)
                    {
                        return i;
                    }
                }

                return 0;
            }
        }

        public double Mean
        {
            get
            {
                int runningTotal = 0;
                int divisor = 0;

                for (int i = 0; i < 255; i++)
                {
                    runningTotal += i * this[i].Count;
                    divisor += this[i].Count;
                }

                return (double)runningTotal / (double)divisor;
            }
        }

        public double Deviation
        {
            get
            {
                double mean = Mean;

                double runningTotal = 0;
                int divisor = 0;

                for (int i = 0; i < 255; i++)
                {
                    runningTotal += i * this[i].Count;
                    runningTotal -= i * mean;

                    divisor += this[i].Count;
                }

                return runningTotal / (double)divisor;
            }
        }
    }
}
