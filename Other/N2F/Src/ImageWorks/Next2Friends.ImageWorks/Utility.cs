using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace NuGenImageWorksXBAP.UI.NuGenImageWorks
{
    public class Utility
    {
        public static void RGB2HSL(byte red, byte green, byte blue, out double h, out double s, out double l)
        {
            h = s = l = 0;

            double r = (double)red / 255;
            double g = (double)green / 255;
            double b = (double)blue / 255;

            double max = Math.Max(Math.Max(r, g), b);
            double min = Math.Min(Math.Min(r, g), b);

            l = (max + min) / 2;

            double d = (max - min);
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);

            if (max == min)
            {
                h = 0;
            }
            else if (max == r)
            {
                h = (g - b) / d + (g < b ? 6 : 0);
            }
            else if (max == g)
            {
                h = (b - r) / d + 2;
            }
            else if (max == b)
            {
                h = (r - g) / d + 4;
            }

            h /= 6;

            h *= 360.0;
        }

        public static Color HSL2RGB(double hue, double saturation, double luminance)
        {
            double r = 0.0, g = 0.0, b = 0.0;

            if (luminance == 0.0)
                r = g = b = 0.0;
            else if (saturation == 0.0)
                r = g = b = luminance;
            else
            {
                double aux2 = ((luminance <= 0.5) ? luminance * (1.0 + saturation) : luminance + saturation - (luminance * saturation));
                double aux1 = 2.0 * luminance - aux2;

                double[] t3 = new double[] { hue + 1.0 / 3.0, hue, hue - 1.0 / 3.0 };
                double[] clr = new double[] { 0.0, 0.0, 0.0 };

                for (int i = 0; i < 3; i++)
                {
                    if (t3[i] < 0)
                        t3[i] += 1.0;
                    if (t3[i] > 1)
                        t3[i] -= 1.0;

                    if (6.0 * t3[i] < 1.0)
                        clr[i] = aux1 + (aux2 - aux1) * t3[i] * 6.0;
                    else if (2.0 * t3[i] < 1.0)
                        clr[i] = aux2;
                    else if (3.0 * t3[i] < 2.0)
                        clr[i] = (aux1 + (aux2 - aux1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                    else
                        clr[i] = aux1;
                }

                r = clr[0];
                g = clr[1];
                b = clr[2];
            }

            if (r > 1.0)
                r = 1.0;
            if (g > 1.0)
                g = 1.0;
            if (b > 1.0)
                b = 1.0;

            return Color.FromScRgb(1.0f, (float)r, (float)g, (float)b);
        }

        //public static RenderTargetBitmap GetRenderTargetBitmap(Array pixels, int width, int height, PixelFormat format, int stride)
        //{
        //    return GetRenderTargetBitmap(BitmapSource.Create(width,
        //        height, 96, 96, format, null, pixels, stride));
        //}

        //public static RenderTargetBitmap GetRenderTargetBitmap(BitmapSource s)
        //{
        //    System.Windows.Controls.Image i = new System.Windows.Controls.Image();
        //    i.Source = s;
        //    RenderTargetBitmap bitmap = new RenderTargetBitmap(s.PixelWidth, s.PixelHeight, 96, 96, PixelFormats.Pbgra32);

        //    Viewbox viewbox = new Viewbox();
        //    viewbox.Child = i;
        //    viewbox.Measure(new System.Windows.Size(s.PixelWidth, s.PixelHeight));
        //    viewbox.Arrange(new Rect(0, 0, s.PixelWidth, s.PixelHeight));
        //    viewbox.UpdateLayout();

        //    bitmap.Render(i);

        //    return bitmap;
        //}        
    }
}
