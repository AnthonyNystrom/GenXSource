using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.NuGenTransform
{

    /*
     * Provides the functionality for seperating an image into on and off pixels based
     * on the provided critera.
     * 
     * Can match Hue, Saturation, Value, Background values, and Intensity values
     * */
    public class NuGenDiscretize
    {
        private Bitmap bmp;

        private int[] pixelData;

        private DiscretizeSettings settings;               

        private Color bgColor;

        // hue, saturation and value upper bounds. the lower bounds are all zero. hopefully
        // these agree with gimp and other important tools
        const int DiscretizeIntensityMax = 100;
        const int DiscretizeForegroundMax = 100;
        const int DiscretizeHueMax = 360;
        const int DiscretizeSaturationMax = 100;
        const int DiscretizeValueMax = 100;

        //Constructs this discretize using the image as the source of data
        public NuGenDiscretize(Image img, DiscretizeSettings settings)
        {
            this.settings = settings;

            this.bmp = new Bitmap(img);            
        }

        //Calculates the background color, which is defined to be the color which
        // occurs most frequently in the image
        public Color GetBackgroundColor()
        {
            pixelData = new int[bmp.Width * bmp.Height];
            //Scans the image and places all of the colors found in the counts array
            ScanImage(bmp);

            this.bgColor = GetMostFrequentColor();

            return bgColor;
        }

        private Color GetMostFrequentColor()
        {

            int curCount = 0;
            int maxCount = 0;

            int maxColor = pixelData[0];
            int lastColor = pixelData[0];

            Array.Sort(pixelData);

            foreach (int color in pixelData)
            {
                if (color != lastColor)
                {
                    if (curCount > maxCount)
                    {
                        maxCount = curCount;
                        maxColor = color;
                    }
                }

                curCount++;
            }          

            byte red = (byte)(maxColor >> 16); //...rrrrrrrr = rrrrrrrr (red)
            byte green = (byte)(maxColor >> 8 & 255); // 255 = 0..0011111111 ...xxxggg & 11111111 = 0..000ggg = gggggggg (green)
            byte blue = (byte)(maxColor & 255); // 255 = 0..0011111111  x..xxbbbbbbbb & 0..0011111111 = 0..00bbbbbbbb = bbbbbbbb (blue)

            return Color.FromArgb(red, green, blue);
        }

        //Scans an image's pixels, switching based on the pixel format of the image
        private void ScanImage(Bitmap b)
        {
            switch (b.PixelFormat)
            {
                case PixelFormat.Format32bppArgb: ScanImage32bpp(b); break;
                case PixelFormat.Format32bppPArgb: ScanImage32bpp(b); break;
                case PixelFormat.Format32bppRgb: ScanImage32bpp(b); break;

                case PixelFormat.Format24bppRgb: ScanImage24bpp(b); break;

                case PixelFormat.Format1bppIndexed: ScanImage1bpp(b); break;
            }
        }

        private void ScanImage24bpp(Bitmap b)
        {
            int width = b.Width;
            int height = b.Height;

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                    ImageLockMode.ReadOnly, b.PixelFormat);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        pixelData[y * (width - 1) + x] = p[0] << 16 | p[1] << 8 | p[2];

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);
        }

        private void ScanImage32bpp(Bitmap b)
        {
            int width = b.Width;
            int height = b.Height;

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                    ImageLockMode.ReadOnly, b.PixelFormat);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 4;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {                        
                        pixelData[y * (width - 1) + x] = p[0]<<16 | p[1]<<8 | p[2];

                        p += 4;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
        }

        private void ScanImage1bpp(Bitmap b)
        {
            int width = b.Width;
            int height = b.Height;

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
        ImageLockMode.ReadOnly, b.PixelFormat);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        if (p[0] == 1)
                        {
                            pixelData[y * (width - 1) + x] = 0;
                        }
                        else
                        {
                            pixelData[y * (width - 1) + x] = 1;
                        }

                        p++;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);
        }

        //Discretizes a color based on its foreground value
        public int DiscretizeValueForeground(int x, int y)
        {
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, bmp.PixelFormat);

            if (bgColor == null)
                GetBackgroundColor();

            int value = DiscretizeValueForeground(x, y, bgColor, bmData);

            bmp.UnlockBits(bmData);

            return value;
        }

        //Discretizes a color based on its foreground value and takes a backgroudn color to use, rather than
        // calculating the current background color or using the stored one
        public int DiscretizeValueForeground(int x, int y, Color referenceColor)
        {
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, bmp.PixelFormat);

            int value = DiscretizeValueForeground(x, y, referenceColor, bmData);

            bmp.UnlockBits(bmData);

            return value;
        }

        //Discretizes a value using the foreground method taking a bitmap data object, this method provides
        // the best performance when looping through an entire image since it doesn't require the bitmap
        // to be locked repeatedly
        public int DiscretizeValueForeground(int x, int y, BitmapData bmData)
        {
            if (bgColor == null)
                GetBackgroundColor();

            return DiscretizeValueForeground(x, y, bgColor, bmData);            
        }


        //Discretizes a value using the foreground method taking a bitmap data object, this method provides
        // the best performance when looping through an entire image since it doesn't require the bitmap
        // to be locked repeatedly
        public int DiscretizeValueForeground(int x, int y, Color referenceColor, BitmapData bmData)
        {

            int r, g, b;
            NuGenImageProcessor.GetPixelAt(bmData, x, y, out r, out g, out b);

            int rBg = referenceColor.R;
            int gBg = referenceColor.G;
            int bBg = referenceColor.B;
       
            double distance = Math.Sqrt((r - rBg) * (r - rBg) + (g - gBg) * (g - gBg) + (b - bBg) * (b - bBg));
            int value = (int)(distance * DiscretizeForegroundMax / Math.Sqrt(255 * 255 + 255 * 255 + 255 * 255) + 0.5);

            if (value < 0)
                value = 0;
            if (ColorAttributeMax(settings.discretizeMethod) < value)
                value = ColorAttributeMax(settings.discretizeMethod);

            return value;
        }

        //Gets the maximum color value for the current mode
        public int ColorAttributeMax(DiscretizeMethod method)
        {
            switch (method)
            {
                case DiscretizeMethod.DiscretizeNone:
                    return 0;
                case DiscretizeMethod.DiscretizeIntensity:
                    return DiscretizeIntensityMax;
                case DiscretizeMethod.DiscretizeForeground:
                    return DiscretizeForegroundMax;
                case DiscretizeMethod.DiscretizeHue:
                    return DiscretizeHueMax;
                case DiscretizeMethod.DiscretizeSaturation:
                    return DiscretizeSaturationMax;
                case DiscretizeMethod.DiscretizeValue:
                default:
                    return DiscretizeValueMax;
            }
        }

        //Tells if a pixel is on based on its color
        public bool PixelIsOn(int value, GridRemovalSettings settings)
        {
            return PixelIsOn(value, settings.foregroundThresholdLow, settings.foregroundThresholdHigh);
        }

        //Tells if a pixel is on based on its value, if it lands inside the threshold it is on.
        private bool PixelIsOn(int value, int low, int high)
        {
            if (low < high)
                return ((low <= value) && (value <= high));
            else
                return ((low <= value) || (value <= high));
        }

        //Switches the pixel is on methods based on the current discretize mode
        public bool PixelIsOn(int value)
        {
            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeNone:
                    throw new InvalidOperationException("Can't discretize in a no discretize setting");
                case DiscretizeMethod.DiscretizeIntensity:
                    return PixelIsOn(value, settings.intensityThresholdLow, settings.intensityThresholdHigh);
                case DiscretizeMethod.DiscretizeForeground:
                    return PixelIsOn(value, settings.foregroundThresholdLow, settings.foregroundThresholdHigh);
                case DiscretizeMethod.DiscretizeHue:
                    return PixelIsOn(value, settings.hueThresholdLow, settings.hueThresholdHigh);
                case DiscretizeMethod.DiscretizeSaturation:
                    return PixelIsOn(value, settings.saturationThresholdLow, settings.saturationThresholdHigh);
                case DiscretizeMethod.DiscretizeValue:
                    return PixelIsOn(value, settings.valueThresholdLow, settings.valueThresholdHigh);
            }

            return false;
        }

        //Tells whether a processed pixel is on, it is only only if it is black, off it is white
        public static bool ProcessedPixelIsOn(BitmapData mbData, int x, int y)
        {
            if (x > mbData.Width || y > mbData.Height)
                return false;

            int r, g, b;
            NuGenImageProcessor.GetPixelAt(mbData, x, y, out r, out g, out b);
            if (r == 0 && g == 0 && b == 0)
                return true;

            return false;
        }

        //Discretizes a value using the current method in the settings
        public int DiscretizeValue(int x, int y)
        {
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                        ImageLockMode.ReadOnly, bmp.PixelFormat);

            int value = DiscretizeValue(x, y, bmData);

            bmp.UnlockBits(bmData);

            return value;
        }

        //Discretizes a value and takes the bitmap data object so that when looping through the entire
        // image the bits do not need to be repeatedly locked
        public int DiscretizeValue(int x, int y, BitmapData bmData)
        {            
            int h, s, v, r, g, b;

            NuGenImageProcessor.GetPixelAt(bmData, x, y, out r, out g, out b);
            NuGenImageProcessor.GetHSVFromRGB(r, g, b, out h, out s, out v);

            double intensity;

            // convert hue from 0 to 359, saturation from 0 to 255, value from 0 to 255

            int value = 0;

            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeForeground:
                    break;
                case DiscretizeMethod.DiscretizeHue:
                    {
                        value = h * DiscretizeHueMax / 359;
                        break;
                    }
                case DiscretizeMethod.DiscretizeIntensity:
                    {
                        intensity = Math.Sqrt(r*r + g*g + b*b);
                        value = (int) (intensity * DiscretizeIntensityMax / Math.Sqrt(255*255 + 255*255 + 255*255) + 0.5);
                        break;
                    }
                case DiscretizeMethod.DiscretizeNone:
                    throw new InvalidOperationException("Can not discretize, discretizing is not enabled at this time");
                case DiscretizeMethod.DiscretizeSaturation:
                    {
                        value = s * DiscretizeSaturationMax / 255;
                        break;
                    }
                case DiscretizeMethod.DiscretizeValue:
                    {
                        value = v * DiscretizeValueMax / 255;
                        break;
                    }
            }

            if(value < 0)
                value = 0;
            if(ColorAttributeMax(settings.discretizeMethod) < value)
                value = ColorAttributeMax(settings.discretizeMethod);

            return value;
        }

        //Discretizes the image, switching through the different modes of discretization
        public void Discretize()
        {
            int value;

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, bmp.PixelFormat);

            for(int x = 0; x<bmp.Width; x++)
            {
                for(int y = 0; y<bmp.Height; y++)
                {
                    if (settings.discretizeMethod == DiscretizeMethod.DiscretizeForeground)
                        value = DiscretizeValueForeground(x, y, bmData);
                    else
                        value = DiscretizeValue(x, y, bmData);

                    if (PixelIsOn(value))
                    {
                        NuGenImageProcessor.SetPixelAt(bmData, x, y, 0, 0, 0);
                    }
                    else
                    {
                        NuGenImageProcessor.SetPixelAt(bmData, x, y, 255, 255, 255);
                    }

                }
            }

            bmp.UnlockBits(bmData);
        }

        public Image GetImage()
        {
            return bmp;
        }

        public void SetImage(Image img)
        {
            bmp = new Bitmap(img);
        }

        public DiscretizeSettings Settings
        {
            get
            {
                return settings;
            }

            set
            {
                settings = value;
            }
        }
    }
}
