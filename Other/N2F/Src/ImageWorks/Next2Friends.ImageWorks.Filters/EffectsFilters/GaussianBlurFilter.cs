using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Next2Friends.ImageWorks.EffectsFilters
{
    public class GaussianBlurFilter : EffectFilter
    {
        private int _radius = 6;
        private int[] _kernel;
        private int _kernelSum;
        private int[,] _multable;
        private bool locked = false;

        public GaussianBlurFilter()
        {
            PreCalculateSomeStuff();
        }

        public GaussianBlurFilter(int radius)
        {
            _radius = radius;
            PreCalculateSomeStuff();
        }

        public static new EffectFilter Default
        {
            get
            {
                return new GaussianBlurFilter();
            }
        }

        private void PreCalculateSomeStuff()
        {
            int sz = _radius * 2 + 1;
            _kernel = new int[sz];
            _multable = new int[sz, 256];
            for (int i = 1; i <= _radius; i++)
            {
                int szi = _radius - i;
                int szj = _radius + i;
                _kernel[szj] = _kernel[szi] = (szi + 1) * (szi + 1);
                _kernelSum += (_kernel[szj] + _kernel[szi]);
                for (int j = 0; j < 256; j++)
                {
                    _multable[szj, j] = _multable[szi, j] = _kernel[szj] * j;
                }
            }
            _kernel[_radius] = (_radius + 1) * (_radius + 1);
            _kernelSum += _kernel[_radius];
            for (int j = 0; j < 256; j++)
            {
                _multable[_radius, j] = _kernel[_radius] * j;
            }
        }

        public override BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            locked = true;
            BitmapSource blurred = inputImage.Clone();

            int pixelCount = inputImage.PixelWidth * inputImage.PixelHeight;
            int[] b = new int[pixelCount];
            int[] g = new int[pixelCount];
            int[] r = new int[pixelCount];

            int[] b2 = new int[pixelCount];
            int[] g2 = new int[pixelCount];
            int[] r2 = new int[pixelCount];

            int width = inputImage.PixelWidth;
            int height = inputImage.PixelHeight;

            byte[] srcPixels = new byte[width * height * 4];
            byte[] destPixels = new byte[width * height * 4];
            inputImage.CopyPixels(srcPixels, width * 4, 0);
            blurred.CopyPixels(destPixels, width * 4, 0);

            for (int i = 0, j = 0; i < pixelCount * 4; i += 4, j++)
            {
                b[j] = srcPixels[i];
                g[j] = srcPixels[i + 1];
                r[j] = srcPixels[i + 2];
            }

            int index = 0;

            int bsum;
            int gsum;
            int rsum;
            int sum;
            int read;
            int start = 0;
            index = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bsum = gsum = rsum = sum = 0;
                    read = index - _radius;

                    for (int z = 0; z < _kernel.Length; z++)
                    {
                        if (read >= start && read < start + width)
                        {
                            bsum += _multable[z, b[read]];
                            gsum += _multable[z, g[read]];
                            rsum += _multable[z, r[read]];
                            sum += _kernel[z];
                        }
                        ++read;
                    }

                    b2[index] = (bsum / sum);
                    g2[index] = (gsum / sum);
                    r2[index] = (rsum / sum);

                    ++index;
                }
                start += width;
            }

            int tempy;
            int pixelIndex = 0;
            for (int i = 0; i < height; i++)
            {
                int y = i - _radius;
                start = y * width;
                for (int j = 0; j < width; j++)
                {
                    bsum = gsum = rsum = sum = 0;
                    read = start + j;
                    tempy = y;
                    for (int z = 0; z < _kernel.Length; z++)
                    {
                        if (tempy >= 0 && tempy < height)
                        {
                            bsum += _multable[z, b2[read]];
                            gsum += _multable[z, g2[read]];
                            rsum += _multable[z, r2[read]];
                            sum += _kernel[z];
                        }
                        read += width;
                        ++tempy;
                    }

                    destPixels[pixelIndex] = (byte)(bsum / sum);
                    destPixels[pixelIndex + 1] = (byte)(gsum / sum);
                    destPixels[pixelIndex + 2] = (byte)(rsum / sum);

                    pixelIndex += 4;
                }
            }

            blurred = BitmapSource.Create(blurred.PixelWidth, blurred.PixelHeight, 96, 96,
                blurred.Format, null, destPixels, blurred.PixelWidth * 4);

            locked = false;

            return blurred;
        }

        public int Radius
        {
            get { return _radius; }
            set
            {
                if (!locked)
                {
                    if (value < 2)
                    {
                        throw new InvalidOperationException("Radius must be greater then 1");
                    }
                    _radius = value;
                    PreCalculateSomeStuff();
                }
            }
        }
    }
}
