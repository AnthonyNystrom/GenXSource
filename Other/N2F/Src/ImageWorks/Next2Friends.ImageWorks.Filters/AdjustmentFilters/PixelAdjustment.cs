using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.ImageWorks.AdjustmentFilters
{
    public abstract class PixelAdjustment
    {
        /// <summary>
        /// Indicates whether this operation should be performed
        /// </summary>
        public abstract bool IsActive { get; }
       
        protected abstract void PerformOperation(ref byte b, ref byte g, ref byte r);
        
        /// <summary>
        /// Performs this pixel transformation on the provided pixel, default behavior ignores
        /// the alpha channel unless the particular pixel filter requires it.  Skips this operation
        /// if this adjustment is not active (IsActive == false), if this method is overridden in 
        /// a subclass this functionality should be preserved.
        /// </summary>
        /// <param name="b">Blue component of the pixel</param>
        /// <param name="g">Green component of the pixel</param>
        /// <param name="r">Red component of the pixel</param>
        /// <param name="a">Alpha component of the pixel</param>
        public void PerformOperation(ref byte b, ref byte g, ref byte r, ref byte a)
        {
            if (IsActive)
            {
                PerformOperation(ref b, ref g, ref r);
            }
        }
    }
}
