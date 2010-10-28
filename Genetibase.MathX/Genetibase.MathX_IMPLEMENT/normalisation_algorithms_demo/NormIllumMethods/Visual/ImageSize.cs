using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace NormIllumMethods.Visual
{
    class ImageSize
    {
        /// <summary>
        /// Resize the bitmap to power of 2 if it is necessary
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap ResizeToPower2(Bitmap bitmap) 
        {
            int w = bitmap.Width;
            int h = bitmap.Height;
            int ww = UpperPower2(w);
            int hh = UpperPower2(h);
            Bitmap outbitmap = new Bitmap(bitmap,ww,hh);
            PixelFormat format = PixelFormat.Format8bppIndexed;
            BitmapData bitmapData = outbitmap.LockBits(new Rectangle(0, 0, ww, hh), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bitmapData.Stride;
            IntPtr scan0 = bitmapData.Scan0;
            Bitmap bb =  new Bitmap(ww,hh,stride,format,scan0);
            outbitmap.UnlockBits(bitmapData);
            //ColorPalette palette = outbitmap.Palette;
            //for (int i = 0; i < 256; i++)
            //{
            //    palette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
            //}
            //outbitmap.Palette = palette;

            return (IsPower2(w) && IsPower2(h)) ? bitmap : bb;
        }

        /// <summary>
        /// Return true if the number is power of 2
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static bool IsPower2(int n)
        {
            return ((n & (n - 1)) == 0);
        }

        /// <summary>
        /// Return the smallest power of 2 greater than or equal to the specified number
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int UpperPower2(int n) 
        {
            return (int)Math.Pow(2, Math.Ceiling(Math.Log(n, 2)));
        } 
    }
}
