using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Next2Friends.ImageWorks.EffectsFilters
{
    public class GrayScaleFilter : EffectFilter
    {
        public override BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            byte[] bytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];
            inputImage.CopyPixels(bytes, inputImage.PixelWidth * 4, 0);

            for (int i = 0; i < bytes.Length; i+=4)
            {
                byte l = (byte)(bytes[i] * 0.11 + bytes[i + 1] * 0.59 + bytes[i + 2] * 0.30);

                bytes[i] = bytes[i + 1] = bytes[i + 2] = l;
            }

            return BitmapSource.Create(inputImage.PixelWidth, inputImage.PixelHeight, 96, 96,
                inputImage.Format, null, bytes, inputImage.PixelWidth * 4);
        }

        public static new EffectFilter Default
        {
            get
            {
                return new GrayScaleFilter();
            }
        }
    }
}
