using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Filters;
using System.Windows.Media;

namespace NuGenImageWorksXBAP.AdjustmentFilters
{
    public class AutoAdjustFilter : AdjustmentFilter
    {
        public ColorHistogram ColorHistogram { get; set; }
        public BrightnessHistogram BrightnessHistogram { get; set; }

        public AutoAdjustFilter(ColorHistogram colorHistogram, 
            BrightnessHistogram brightnessHistogram)
        {
            ColorHistogram = colorHistogram;
            BrightnessHistogram = brightnessHistogram;
        }

        public override BitmapSource ExecuteFilter(BitmapSource source)
        {
            //First calculate average deviation of the source image
            int range = BrightnessHistogram.Max - BrightnessHistogram.Min;
            double factor = 256.0 / (double)range;

            byte[] bytes = new byte[source.PixelWidth * source.PixelHeight * 4];
            source.CopyPixels(bytes, source.PixelWidth * 4, 0);

            double mean = BrightnessHistogram.Mean;

            byte r, g, b;
            double h, s, l;

            for (int i = 0; i < bytes.Length; i += 4)
            {
                r = bytes[i + 2];
                g = bytes[i + 1];
                b = bytes[i];

                Utility.RGB2HSL(r, g, b, out h, out s, out l);

                double deviation = l - mean / 256.0;
                double lprime = .5 + deviation * factor;

                if (lprime < 0) lprime = 0.0;
                else if (lprime > 1) lprime = 1.0;

                Color c = Utility.HSL2RGB(h, s, lprime);

                bytes[i + 2] = c.R;
                bytes[i + 1] = c.G;
                bytes[i + 0] = c.B;
            }

            BitmapSource result = BitmapSource.Create(source.PixelWidth, source.PixelHeight, 96, 96,
                source.Format, null, bytes, source.PixelWidth * 4);

            return result;
        }
    }    
}
