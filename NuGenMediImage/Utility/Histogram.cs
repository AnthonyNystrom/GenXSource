using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenMediImage.Utility
{
    class Histogram
    {
        public static unsafe Bitmap GetHistogram(Bitmap sourceBitmap, int HistWidth, int HistHeight)
        {
            Bitmap histBitmap = new Bitmap(HistWidth, HistHeight);
            int[] values = new int[256];

            BitmapData data;
            byte* pixels;

            Graphics g = Graphics.FromImage(histBitmap);

            data = sourceBitmap.LockBits(new System.Drawing.Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);

            pixels = (byte*)data.Scan0;

                //if (flip)
                //    pixels += sourceBitmap.Height * data.Stride;

            for (int y = 0; y < sourceBitmap.Height; y++)
            {
                for (int x = 0; x < sourceBitmap.Width; x++)
                    values[pixels[x]]++;

                pixels += data.Stride;
            }

            sourceBitmap.UnlockBits(data);

            int max = 0;
            for (int i = 0; i < 255; i++)
                max = Math.Max(max, values[i]);

            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, HistWidth, HistHeight), Color.LightSteelBlue, Color.SteelBlue, LinearGradientMode.Vertical);

            for (int i = 0; i < 255; i++)
                g.DrawLine(new Pen(brush), i, HistHeight, i, HistHeight - (int)(HistHeight * ((double)values[i] / (double)max)));

            return histBitmap;
        }

        /// <summary>
        /// Get the size of the file as a string in format n Byte(s), n KB, n MB
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetFileSize(string FileName)
        {
            try
            {
                // Open the file for getting the size and then close it                        
                System.IO.FileInfo f = new System.IO.FileInfo(FileName);
                float size = f.Length;

                string sizeStr = "";

                if (size < 1024)
                {
                    sizeStr = size + " Byte(s)";
                }
                else if (size >= 1024 && size < (1024 * 1024))
                {
                    size = size / 1024;
                    sizeStr = Math.Round(size, 1) + " KB";
                }
                else
                {
                    size = size / (1024 * 1024);
                    sizeStr = Math.Round(size, 1) + " MB";
                }

                return sizeStr;
            }
            catch { }
            return "";
        }
    }
}
