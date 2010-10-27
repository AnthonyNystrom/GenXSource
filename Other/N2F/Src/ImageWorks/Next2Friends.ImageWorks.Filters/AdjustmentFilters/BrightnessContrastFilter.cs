using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Next2Friends.ImageWorks.AdjustmentFilters
{
    public class BrightnessContrastFilter : PixelAdjustment
    {
        public int Brightness { get; set; }
        public int Contrast { get; set; }

        public override bool IsActive
        {
            get
            {
                return (Brightness != 0 || Contrast != 0);
            }
        }

        public BrightnessContrastFilter(int brightness, int contrast)
        {
            Brightness = brightness;
            Contrast = contrast;
        }

        protected override void PerformOperation(ref byte b, ref byte g, ref byte r)
        {
            if (!(Brightness < -255 || Brightness > 255) && Brightness != 0)
            {                
                int bright = Brightness;

                //Set the red brightness
                int nVal = r + bright;

                if (nVal < 0)
                {
                    nVal = 0;
                }
                else if (nVal > 255)
                {
                    nVal = 255;
                }

                r = (byte)nVal;

                //Set the green brightness
                nVal = g + bright;

                if (nVal < 0)
                {
                    nVal = 0;
                }
                else if (nVal > 255)
                {
                    nVal = 255;
                }

                g = (byte)nVal;

                //Set the blue brightness
                nVal = b + bright;

                if (nVal < 0)
                {
                    nVal = 0;
                }
                else if (nVal > 255)
                {
                    nVal = 255;
                }

                b = (byte)nVal;
            }

            if (!(Contrast < -100 || Contrast > 100) && Contrast != 0)
            {                
                double pixel = 0, dcontrast = (100.0 + Contrast) / 100.0;

                dcontrast *= dcontrast;

                byte blue = b;
                byte green = g;
                byte red = r;

                pixel = red / 255.0;
                pixel -= 0.5;
                pixel *= dcontrast;
                pixel += 0.5;
                pixel *= 255;
                if (pixel < 0) pixel = 0;
                if (pixel > 255) pixel = 255;
                r = (byte)pixel;

                pixel = green / 255.0;
                pixel -= 0.5;
                pixel *= dcontrast;
                pixel += 0.5;
                pixel *= 255;
                if (pixel < 0) pixel = 0;
                if (pixel > 255) pixel = 255;
                g = (byte)pixel;

                pixel = blue / 255.0;
                pixel -= 0.5;
                pixel *= dcontrast;
                pixel += 0.5;
                pixel *= 255;
                if (pixel < 0) pixel = 0;
                if (pixel > 255) pixel = 255;
                b = (byte)pixel;
            }         
        }
    }
}
