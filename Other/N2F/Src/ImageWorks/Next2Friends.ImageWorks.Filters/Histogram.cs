using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Next2Friends.ImageWorks;

namespace Next2Friends.ImageWorks.Filters
{
    public abstract class Histogram
    {
        private List<ByteRGB>[] buckets = new List<ByteRGB>[256];

        public List<ByteRGB> this[int arg]
        {
            get
            {
                return buckets[arg];
            }
        }

        public BitmapSource Source { get; set; }

        public Histogram(BitmapSource source)
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<ByteRGB>();
            }

            Source = source;

            MakeHistogram();
        }

        protected abstract void MakeHistogram();
    }
}
