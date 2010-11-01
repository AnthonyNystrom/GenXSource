using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.NuGenTransform
{
    /*Handles all of the operations of image processing.
    * Takes some of the load off of the document so that it can focus
     * on document tasks rather than image processing tasks as well.
    */
    public class NuGenImageProcessor
    {
        private NuGenDocument doc;

        private Image processedImage;

        private Image originalImage;

        private Color bgColor;

        public NuGenImageProcessor(NuGenDocument doc)
        {
            this.doc = doc;

            originalImage = doc.OriginalImage;
            processedImage = (Image)doc.OriginalImage.Clone();
        }

        //The main processing method, runs through all of the processing logic
        public void Process()
        {
            NuGenDiscretize discretize = new NuGenDiscretize(originalImage, doc.DiscretizeSettings);
            NuGenGridRemoval gridRemoval = new NuGenGridRemoval(originalImage, discretize);
            bgColor = discretize.GetBackgroundColor();
            gridRemoval.RemoveAndConnect(doc.Transform, doc.CoordSettings,
                                            doc.GridRemovalSettings, bgColor);

            discretize.Discretize();

            processedImage = discretize.GetImage();

            NuGenSegmentCollection segments = doc.Segments;
            segments.MakeSegments(processedImage, doc.SegmentSettings);
        }

        public Color BackgroundColor
        {
            get
            {
                if (bgColor != null)
                    return bgColor;
                else
                    return Color.Black;
            }
        }

        public Image ProcessedImage
        {
            get
            {
                return processedImage;
            }
        }

        public Image OriginalImage
        {
            get
            {
                return originalImage;
            }
        }

        //Sets a pixel at a specific location to the given rgb value based on its pixel format
        public static void SetPixelAt(BitmapData bmData, int x, int y, byte r, byte g, byte b)
        {
            switch(bmData.PixelFormat)
            {
                case PixelFormat.Format24bppRgb: SetPixelAt24bpp(bmData, x, y, r, g, b); break;
                case PixelFormat.Format32bppArgb: SetPixelAt32bpp(bmData, x, y, r, g, b); break;
                case PixelFormat.Format32bppPArgb: SetPixelAt32bpp(bmData, x, y, r, g, b); break;
                case PixelFormat.Format32bppRgb: SetPixelAt32bpp(bmData, x, y, r, g, b); break;                
            }
        }

        //Gets a pixel at a specific location and sets the rgb value based on the images pixel format
        public static void GetPixelAt(BitmapData bmData, int x, int y, out int r, out int g, out int b)
        {
            if (x > bmData.Width || y > bmData.Height)
                throw new ArgumentException("The x and y values provided were outside of the bounds of the image :" + x.ToString() + "," + y.ToString());

            r = g = b = 0;

            switch (bmData.PixelFormat)
            {
                case PixelFormat.Format24bppRgb: GetPixelAt24bpp(bmData, x, y, out r, out g, out b); break;
                case PixelFormat.Format32bppArgb: GetPixelAt32bpp(bmData, x, y, out r, out g, out b); break;
                case PixelFormat.Format32bppPArgb: GetPixelAt32bpp(bmData, x, y, out r, out g, out b); break;
                case PixelFormat.Format32bppRgb: GetPixelAt32bpp(bmData, x, y, out r, out g, out b); break;
            }
        }

        public static void GetPixelAt32bpp(BitmapData bmData, int x, int y, out int r, out int b, out int g)
        {
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - bmData.Width * 4;
                p += (y * bmData.Width * 4) + (x * 4) + (nOffset * y);
                r = p[2];
                g = p[1];
                b = p[0];            
            }
        }

        public static void GetPixelAt24bpp(BitmapData bmData, int x, int y, out int r, out int g, out int b)
        {        
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - bmData.Width * 3;
                p += (y * bmData.Width * 3) + (x * 3) + (nOffset * y);
                r = p[2];
                g = p[1];
                b = p[0];   
            }
        }

        public static void SetPixelAt32bpp(BitmapData bmData, int x, int y, byte r, byte g, byte b)
        {

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - bmData.Width * 4;
                p += (y * bmData.Width * 4) + (x * 4) + (nOffset * y);
                p[0] = r;
                p[1] = g;
                p[2] = b;
            }
        }

        public static void SetPixelAt24bpp(BitmapData bmData, int x, int y, byte r, byte g, byte b)
        {
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - bmData.Width * 3;
                p += (y * bmData.Width * 3) + (x * 3) + (nOffset * y);
                p[0] = r;
                p[1] = g;
                p[2] = b;
            }
        }

        //Converts an rgb value to hsv using the classic algorithm
        public static void GetHSVFromRGB(int r, int g, int b, out int h, out int s, out int v)
        {
            float min, max, delta, hF, sF, vF, rF, gF, bF;

            rF = (float)r;
            gF = (float)g;
            bF = (float)b;

            min = Math.Min(Math.Min(rF, gF), bF);
            max = Math.Max(Math.Max(rF, gF), bF);
            vF = max;				// v

            delta = max - min;

            if (max != 0)
                sF = delta / max;		// s
            else
            {
                // r = g = b = 0		// s = 0, v is undefined
                s = 0;
                h = v = -1;
                return;
            }

            if (rF == max)
                hF = (gF - bF) / delta;		// between yellow & magenta
            else if (gF == max)
                hF = 2 + (bF - rF) / delta;	// between cyan & yellow
            else
                hF = 4 + (rF - gF) / delta;	// between magenta & cyan

            hF *= 60;				// degrees
            if (hF < 0)
                hF += 360;

            h = (int)(hF + 0.5);
            s = (int)(sF + 0.5);
            v = (int)(vF + 0.5);
        }
    }
}
