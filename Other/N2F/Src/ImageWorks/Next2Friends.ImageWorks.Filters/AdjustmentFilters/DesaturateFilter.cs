using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.ImageWorks.AdjustmentFilters
{
    public class DesaturateFilter : PixelAdjustment
    {
        public override bool IsActive
        {
            get
            {
                return true;
            }
        }

        protected override void PerformOperation(ref byte b, ref byte g, ref byte r)
        {
            byte l = (byte)(b * 0.11 + g * 0.59 + r * 0.30);
            b = g = r = l;
        }
    }
}
