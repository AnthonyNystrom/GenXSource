using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Next2Friends.ImageWorks.AdjustmentFilters
{
    public class TemperatureFilter : PixelAdjustment
    {
        public double Temperature { get; set; }
        public override bool IsActive
        {
            get
            {
                return Temperature != 0;
            }
        }

        public TemperatureFilter(double temperature)
        {
            lock (this)
            {
                Temperature = temperature;
            }
        }

        protected override void PerformOperation(ref byte b, ref byte g, ref byte r)
        {
            double blueFactor = 0.0, redFactor = 0.0;

            lock (this)
            {
                redFactor = Math.Pow(2, Temperature / 2.0);
                blueFactor = Math.Pow(2, -(Temperature / 2.0));
            }

            double nblue = b * blueFactor;
            double nred = r * redFactor;

            if (nblue > 255)
                nblue = 255;
            if (nred > 255)
                nred = 255;

            b = (byte)nblue;
            r = (byte)nred;
        }
    }
}
