using System;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Next2Friends.ImageWorks.Filters;

namespace Next2Friends.ImageWorks.AdjustmentFilters
{
    /// <summary>
    /// Summary description for BasicFilter.
    /// </summary>
    public class AdjustmentFilter : IFilter
    {
        private List<PixelAdjustment> pixelAdjustments = new List<PixelAdjustment>();
        public List<PixelAdjustment> PixelAdjustments
        {
            get
            {
                return pixelAdjustments;
            }
        }

        /// <summary>
        /// Executes this filter on the input image and returns the result
        /// </summary>
        /// <param name="inputImage">input image</param>
        /// <returns>transformed image</returns>
        public BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            byte[] bytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];
            inputImage.CopyPixels(bytes, inputImage.PixelWidth * 4, 0);

            for (int i = 0; i < bytes.Length; i += 4)
            {
                foreach (PixelAdjustment p in PixelAdjustments)
                {
                    if (p.IsActive)
                    {
                        p.PerformOperation(ref bytes[i], ref bytes[i + 1], ref bytes[i + 2], ref bytes[i + 3]);
                    }
                }
            }

            return BitmapSource.Create(inputImage.PixelWidth, inputImage.PixelHeight, 96, 96,
                inputImage.Format, null, bytes, inputImage.PixelWidth * 4);
        }
    }
}

