using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Next2Friends.ImageWorks.AdjustmentFilters
{
    public class HSLFilter : PixelAdjustment
    {
        public int Hue { get; set; }
        public double Saturation { get; set; }
        public int Lightness { get; set; }

        public override bool IsActive
        {
            get
            {
                return (Hue != 0 || Saturation != 100 || Lightness != 0);
            }            
        }

        public HSLFilter(int hue, int saturation, int lightness)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }        

        protected override void PerformOperation(ref byte b, ref byte g, ref byte r)
        {
            double satMultiplier = (double)Saturation / 100.0;
            double lightnessAdder = (double)Lightness / 100.0;
            double hueAdder = (double)Hue / 360;

            double h, s, l;
            Utility.RGB2HSL(r, g, b, out h, out s, out l);

            h += hueAdder;
            s *= satMultiplier;
            l += lightnessAdder;

            if (h > 1.0) h = 1;
            else if (h < 0.0) h = 0;

            if (s > 1.0) s = 1;
            else if (s < 0.0) s = 0;

            if (l > 1.0) l = 1;
            else if (l < 0.0) l = 0;

            Color c = Utility.HSL2RGB(h, s, l);

            b = c.B;
            g = c.G;
            r = c.R;
        }
    }
}
