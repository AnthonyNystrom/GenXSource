using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Next2Friends.ImageWorks.AdjustmentFilters;
using Next2Friends.ImageWorks.Filters;

namespace Next2Friends.ImageWorks.EffectsFilters
{
    public class PortraitEffect : EffectFilter
    {
        public int Softness { get; set; }
        public int Lighting { get; set; }
        public int Warmth { get; set; }

        public PortraitEffect(int softness, int lighting, int warmth)
        {
            Softness = softness;
            Lighting = lighting;
            Warmth = warmth;
        }

        public static new EffectFilter Default
        {
            get
            {
                return new PortraitEffect(5, 0, 10);
            }
        }

        public override BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            byte[] sourceBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];
            byte[] destBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];

            inputImage.CopyPixels(sourceBytes, inputImage.PixelWidth * 4, 0);

            float redAdjust = 1.0f + (this.Warmth / 100.0f);
            float blueAdjust = 1.0f - (this.Warmth / 100.0f);

            GaussianBlurFilter blur = new GaussianBlurFilter(3 * Softness);
            BrightnessContrastFilter bc = new BrightnessContrastFilter(Lighting, -Lighting / 2);
            DesaturateFilter desat = new DesaturateFilter();

            inputImage = blur.ExecuteFilter(inputImage);
            inputImage.CopyPixels(destBytes, inputImage.PixelWidth * 4, 0);

            int width = inputImage.PixelWidth;
            int height = inputImage.PixelHeight;

            for (int i = 0; i < sourceBytes.Length; i += 4)
            {
                byte b = sourceBytes[i];
                byte g = sourceBytes[i + 1];
                byte r = sourceBytes[i + 2];
                byte a = sourceBytes[i + 3];

                bc.PerformOperation(ref b, ref g, ref r, ref a);
                desat.PerformOperation(ref b, ref g, ref r, ref a);

                double rD = r * redAdjust;
                double bD = r * blueAdjust;

                if (rD > 255) rD = 255;
                else if (rD < 0) rD = 0;

                if (bD > 255) bD = 255;
                else if (bD < 0) bD = 0;

                destBytes[i] = PixelBlend.BlendOverlay(b, destBytes[i]);
                destBytes[i + 1] = PixelBlend.BlendOverlay(g, destBytes[i + 1]);
                destBytes[i + 2] = PixelBlend.BlendOverlay(r, destBytes[i + 2]);
                destBytes[i + 3] = PixelBlend.BlendOverlay(a, destBytes[i + 3]);
            }

            return BitmapSource.Create(inputImage.PixelWidth, inputImage.PixelHeight, 96, 96,
                inputImage.Format, null, destBytes, inputImage.PixelWidth * 4);
        }
    }
}
