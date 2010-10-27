using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Next2Friends.ImageWorks;

namespace Next2Friends.ImageWorks.Filters
{
    public class ColorHistogram : Histogram
    {
        private List<ByteRGB>[] redBuckets = new List<ByteRGB>[256];
        private List<ByteRGB>[] greenBuckets = new List<ByteRGB>[256];
        private List<ByteRGB>[] blueBuckets = new List<ByteRGB>[256];

        public List<ByteRGB>[] Red
        {
            get
            {
                return redBuckets;
            }
        }

        public List<ByteRGB>[] Green
        {
            get
            {
                return greenBuckets;
            }
        }

        public List<ByteRGB>[] Blue
        {
            get
            {
                return blueBuckets;
            }
        }

        public ColorHistogram(BitmapSource source) : base(source){}

        protected override void MakeHistogram()
        {
            Initialize();

            byte[] bytes = new byte[Source.PixelWidth * Source.PixelHeight * 4];
            Source.CopyPixels(bytes, Source.PixelWidth * 4, 0);

            byte r, g, b;

            for (int i = 0; i < bytes.Length; i += 4)
            {
                r = bytes[i + 2];
                g = bytes[i + 1];
                b = bytes[i];

                ByteRGB toAdd = new ByteRGB(r, g, b);

                this[r].Add(toAdd);
                this[g].Add(toAdd);
                this[b].Add(toAdd);

                redBuckets[r].Add(toAdd);
                greenBuckets[g].Add(toAdd);
                blueBuckets[b].Add(toAdd);
            }
        }

        private void Initialize()
        {
            for (int i = 0; i < 256; i++)
            {
                redBuckets[i] = new List<ByteRGB>();
                greenBuckets[i] = new List<ByteRGB>();
                blueBuckets[i] = new List<ByteRGB>();
            }
        }
    }
}
