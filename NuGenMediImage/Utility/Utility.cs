using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Genetibase.NuGenMediImage.Utility
{
    /// <summary>
    /// Summary description for Utility.
    /// </summary>
    public class Util
    {

        public static Bitmap Copy(Bitmap bitmap)
		{
            int w = bitmap.Width;
            int h = bitmap.Height;

            Bitmap ret = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(ret);
            g.DrawImage(bitmap, 0, 0);
            g.Dispose();

            return ret;

            //FastBitmap fOrig = new FastBitmap(bitmap);
            //FastBitmap fb = new FastBitmap(ret);

            //fb.LockBitmap();
            //fOrig.LockBitmap();

            //unsafe
            //{
            //    for (int j = 0; j < h; j++)
            //    {
            //        for (int i = 0; i < w; i++)
            //        {
            //            PixelData* pOrig = fOrig[i, j];
            //            PixelData* pb = fb[i, j];

            //            pb->red = pOrig->red;
            //            pb->green = pOrig->green;
            //            pb->blue = pOrig->blue;
            //        }
            //    }
            //}

            //fOrig.UnlockBitmap();
            //fb.UnlockBitmap();

            //return ret;
		}

        public int Read20Bits(byte b1, byte b2, byte b3, int x)
        {
            int result = 0;

            if (x == 0)
            {
                // shift the bits 8 bits to left
                result = b1 << 8;

                // OR with the b2 to insert the b2 bits in the result
                result = result | b2;

                // shift further left 4 bits to complete 20 bits
                result = result << 4;

                // remove the left most 4 bits
                byte temp = (byte)(b3 >> 4);

                // OR to add the bits
                result = result | temp;
            }

            else if (x == 1)
            {
                //left shift four bits to get rid of already used bits
                result = (byte)(b1 << 4);

                result = result << 4;
                result = result | b2;

                result = result << 8;
                result = result | b3;
            }

            return result;
        }

        public int Read12Bits(byte b1, byte b2, int x)
        {
            int result = 0;

            if (x == 0)
            {
                // shift the bits 4 bits to left
                result = b1 << 4;

                // remove the right most 4 bits
                byte temp = (byte)(b2 >> 4);

                // OR to add the bits
                result = result | temp;
            }

            else if (x == 1)
            {
                //left shift four bits to get rid of already used bits
                result = (byte)(b1 << 4);

                result = result << 4;
                result = result | b2;
            }

            return result;
        }

        public static string GetFileName(string Path)
        {
            int indexOfLast = Path.LastIndexOf("\\");

            return Path.Substring(indexOfLast + 1);
        }

        public static string GetFileNameWOExt(string Path)
        {
            int indexOfLastSlash = Path.LastIndexOf("\\");
            int indexOfLastDot = Path.LastIndexOf(".");

            if (indexOfLastDot < 0)
            {
                return Path.Substring(indexOfLastSlash + 1);
            }

            return Path.Substring(indexOfLastSlash + 1, indexOfLastDot - (indexOfLastSlash + 1));
        }

        public static string GetFileExt(string Path)
        {
            int indexOfLast = Path.LastIndexOf(".");

            return Path.Substring(indexOfLast + 1);
        }

        public static string StripFileNameExt(string Path)
        {
            int indexOfLast = Path.LastIndexOf("\\");
            return Path.Substring(0, indexOfLast + 1);
        }

    }
}
