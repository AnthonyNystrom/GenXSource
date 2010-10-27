using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Next2Friends.ImageWorks.AdjustmentFilters;
using Next2Friends.ImageWorks.Filters;

namespace Next2Friends.ImageWorks.EffectsFilters
{
    public class InkSketch : EffectFilter
    {
        public int InkOutline { get; set; }
        public int Coloring { get; set; }

        private readonly int[][] conv;
        private const int size = 5;
        private const int radius = (size - 1) / 2;

        public InkSketch(int inkOutline, int coloring)
        {
            InkOutline = inkOutline;
            Coloring = coloring;

            conv = new int[5][];

            for (int i = 0; i < conv.Length; ++i)
            {
                conv[i] = new int[5];
            }

            conv[0] = new int[] { -1, -1, -1, -1, -1 };
            conv[1] = new int[] { -1, -1, -1, -1, -1 };
            conv[2] = new int[] { -1, -1, 30, -1, -1 };
            conv[3] = new int[] { -1, -1, -1, -1, -1 };
            conv[4] = new int[] { -1, -1, -5, -1, -1 };
        }

        public static new EffectFilter Default
        {
            get
            {
                return new InkSketch(50, 50);
            }
        }

        public override BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            GlowEffect glowEffect = new GlowEffect(6, -(Coloring - 50) * 2, -(Coloring - 50) * 2);
            BitmapSource glow = glowEffect.ExecuteFilter(inputImage);

            byte[] glowBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];
            glow.CopyPixels(glowBytes, inputImage.PixelWidth * 4, 0);

            byte[] srcBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];
            inputImage.CopyPixels(srcBytes, inputImage.PixelWidth * 4, 0);

            byte[] finalBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];

            int width = inputImage.PixelWidth;
            int height = inputImage.PixelHeight;

            // Create black outlines by finding the edges of objects 
            for (int y = 0; y < height; y++)
            {
                int top = y - radius;
                int bottom = y + radius + 1;

                if (top < 0)
                {
                    top = 0;
                }

                if (bottom > height)
                {
                    bottom = height;
                }

                for (int x = 0; x < width; x++)
                {
                    int left = x - radius;
                    int right = x + radius + 1;

                    if (left < 0)
                    {
                        left = 0;
                    }

                    if (right > width)
                    {
                        right = width;
                    }

                    int r = 0;
                    int g = 0;
                    int b = 0;

                    for (int v = top; v < bottom; v++)
                    {                        
                        int j = v - y + radius;

                        for (int u = left; u < right; u++)
                        {
                            int i1 = u - x + radius;
                            int w = conv[j][i1];

                            int i = v*width*4 + u*4;

                            r += srcBytes[i + 2] * w;
                            g += srcBytes[i + 1] * w;
                            b += srcBytes[i] * w;
                        }
                    }

                    if (r > 255) r = 255;
                    else if (r < 0) r = 0;

                    if (g > 255) g = 255;
                    else if (g < 0) g = 0;

                    if (b > 255) b = 255;
                    else if (b < 0) b = 0;

                    byte rb = (byte)r;
                    byte gb = (byte)g;
                    byte bb = (byte)b;
                    byte ab = (byte)255;

                    // Desaturate
                    DesaturateFilter desaturation = new DesaturateFilter();
                    desaturation.PerformOperation(ref bb, ref gb, ref rb, ref ab);

                    // Adjust Brightness and Contrast 
                    if (rb > (InkOutline * 255 / 100))
                    {
                        rb = gb = bb = 255;
                    }
                    else
                    {
                        rb = gb = bb = 0;
                    }

                    int index = y * width * 4 + x * 4;

                    //Darken blend
                    finalBytes[index] = PixelBlend.BlendDarken(glowBytes[index], bb);
                    finalBytes[index + 1] = PixelBlend.BlendDarken(glowBytes[index + 1], gb);
                    finalBytes[index + 2] = PixelBlend.BlendDarken(glowBytes[index + 2], rb);
                    finalBytes[index + 3] = 255;
                }
            }

            return BitmapSource.Create(inputImage.PixelWidth, inputImage.PixelHeight, 96, 96,
                inputImage.Format, null, finalBytes, inputImage.PixelWidth * 4);
        }
    }
}
