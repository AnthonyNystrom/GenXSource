using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.UI.NuGenImageWorks
{
    class Operations
    {
        public static Bitmap Crop(Bitmap imgPhoto, int X,int Y,int Width,int Height)
        {
            Bitmap b = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(b);

            g.DrawImage(imgPhoto,new Rectangle(0, 0, Width, Height),X,Y,Width,Height,GraphicsUnit.Pixel);

            g.Dispose();
            return b;
        }
    }
}
