using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Next2Friends.ImageWorks.AdjustmentFilters;
using Next2Friends.ImageWorks.Filters;

namespace Next2Friends.ImageWorks.EffectsFilters
{
    public class GlowEffect : EffectFilter
    {
        public int Radius { get; set; }
        public int Brightness { get; set; }
        public int Contrast { get; set; }

        public GlowEffect(int radius, int brightness, int contrast)
        {
            Radius = radius;

            //Convert -100 to 100 space to -255 to 255 space
            Brightness = (int)(2.55 * brightness);
            Contrast = (int)(2.55 * contrast);
        }

        public static new EffectFilter Default
        {
            get
            {
                return new GlowEffect(6, 10, 10);
            }
        }

        public override BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            byte[] srcBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];            
            byte[] finalBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];

            inputImage.CopyPixels(srcBytes, inputImage.PixelWidth * 4, 0);            

            BrightnessContrastFilter bcFilter = new BrightnessContrastFilter(Brightness, Contrast);
            GaussianBlurFilter blurFilter = new GaussianBlurFilter(Radius);

            BitmapSource glow = blurFilter.ExecuteFilter(inputImage);

            byte[] glowBytes = new byte[glow.PixelWidth * glow.PixelHeight * 4];
            glow.CopyPixels(glowBytes, glow.PixelWidth * 4, 0);

            for (int i = 0; i < glowBytes.Length; i+=4)
            {
                byte b = glowBytes[i];
                byte g = glowBytes[i + 1];
                byte r = glowBytes[i + 2];
                byte a = glowBytes[i + 3];

                bcFilter.PerformOperation(ref b, ref g, ref r, ref a);

                finalBytes[i] = PixelBlend.BlendScreen(srcBytes[i], b);
                finalBytes[i + 1] = PixelBlend.BlendScreen(srcBytes[i + 1], g);
                finalBytes[i + 2] = PixelBlend.BlendScreen(srcBytes[i + 2], r);
                finalBytes[i + 3] = 255;
            }

            return BitmapSource.Create(inputImage.PixelWidth, inputImage.PixelHeight, 96, 96,
                inputImage.Format, null, finalBytes, inputImage.PixelWidth * 4);
        }
    }
}
