using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ScreenCapture
{
    class Effects
    {
        private static int roundedDia = 50;

        public static int RoundedDiameter
        {
            get { return Effects.roundedDia; }
            set { Effects.roundedDia = value; }
        }

        public static Bitmap RoundImage( Image bitmap )
        {
            Bitmap retImage = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics g = Graphics.FromImage(retImage);
            g.Clear(Color.Transparent);
            Brush brush = new TextureBrush(bitmap);
            FillRoundedRectangle(g, new Rectangle(0, 0, bitmap.Width, bitmap.Height), roundedDia, brush);

            g.Dispose();
            brush.Dispose();

            return retImage;
        }

        public static void FillRoundedRectangle(Graphics g, Rectangle r, int d, Brush b)
        {
            SmoothingMode mode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.FillPie(b, r.X, r.Y, d, d, 180, 90);
            g.FillPie(b, r.X + r.Width - d, r.Y, d, d, 270, 90);
            g.FillPie(b, r.X, r.Y + r.Height - d, d, d, 90, 90);
            g.FillPie(b, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            g.FillRectangle(b, r.X + d / 2, r.Y, r.Width - d, d / 2);
            g.FillRectangle(b, r.X, r.Y + d / 2, r.Width, r.Height - d);
            g.FillRectangle(b, r.X + d / 2, r.Y + r.Height - d / 2, r.Width - d, d / 2);
            g.SmoothingMode = mode;
        }

        public static Image Bevel(Image img)
        {

            Image bitmapNew = (Image)img.Clone();

            int widTh, heTh;
            widTh = bitmapNew.Width; heTh = bitmapNew.Height;

            int BevW = 10, LowA = 0, HighA = 180, Dark = 80, Light = 255;
            // hilight color, low and high

            Color clrHi1 = Color.FromArgb(LowA, Light, Light, Light);
            Color clrHi2 = Color.FromArgb(HighA, Light, Light, Light);
            Color clrDark1 = Color.FromArgb(LowA, Dark, Dark, Dark);
            Color clrDark2 = Color.FromArgb(HighA, Dark, Dark, Dark);

            LinearGradientBrush b; Rectangle rectSide;
            Graphics g = Graphics.FromImage(bitmapNew);
            Size szHorz = new Size(widTh, BevW);
            Size szVert = new Size(BevW, heTh);

            szHorz += new Size(0, 2); szVert += new Size(2, 0);
            rectSide = new Rectangle(new Point(0, heTh - BevW), szHorz);
            b = new LinearGradientBrush(rectSide, clrDark1, clrDark2, LinearGradientMode.Vertical);
            rectSide.Inflate(0, -1);
            g.FillRectangle(b, rectSide);

            rectSide = new Rectangle(new Point(widTh - BevW, 0), szVert);
            b.Dispose();
            b = new LinearGradientBrush(rectSide, clrDark1, clrDark2, LinearGradientMode.Horizontal);
            rectSide.Inflate(-1, 0);
            g.FillRectangle(b, rectSide);
            szHorz -= new Size(0, 2); szVert -= new Size(2, 0);
            
            rectSide = new Rectangle(new Point(0, 0), szHorz);
            b.Dispose();
            b = new LinearGradientBrush(rectSide, clrHi2, clrHi1, LinearGradientMode.Vertical);
            g.FillRectangle(b, rectSide);

            rectSide = new Rectangle(new Point(0, 0), szVert);
            b.Dispose();
            b = new LinearGradientBrush(rectSide, clrHi2, clrHi1, LinearGradientMode.Horizontal);
            g.FillRectangle(b, rectSide);

            // dispose graphics objects and return bitmap

            b.Dispose();
            g.Dispose();
            return bitmapNew;
        }

        public Bitmap FeatherRectangle(Bitmap img, Color color, int Offset)
        {
            int Width = img.Width;
            int Height = img.Height;

            Bitmap whiteBitmap = new Bitmap(Width, Height);
            int initialTransparency = 255;
            int transparencyStep = initialTransparency / Offset;

            Bitmap retBitmap = (Bitmap)img.Clone();

            Graphics g = Graphics.FromImage(whiteBitmap);
            // Make it white
            g.FillRectangle(Brushes.White, 0, 0, retBitmap.Width, retBitmap.Height);
            g.Dispose();

            g = Graphics.FromImage(retBitmap);

            // The top
            for (int i = 0; i <= Width; i++)
            {
                initialTransparency = 255;
                for (int j = 0; j <= Offset; j++)
                {
                    if (initialTransparency >= 0)
                    {
                        Console.WriteLine(initialTransparency);
                        whiteBitmap.SetPixel(i, j, Color.FromArgb(initialTransparency, Color.Gray));
                        initialTransparency -= 1;
                    }
                }
            }

            initialTransparency = 255;
            // The bottom
            /*for (int i = 0; i <= Width; i++)
            {			
                for (int j = Height; j > Height - Offset; j--)
                {
                    if( initialTransparency >= 0 )
                    {
                        Color c = whiteBitmap.GetPixel(i,j);
                        whiteBitmap.SetPixel(i,j, Color.FromArgb(initialTransparency, c) );
                        initialTransparency -= transparencyStep;				
                    }
                    else
                    {
                        break;
                    }
                }
            }*/

            // the rest of the image is transparent
            for (int i = Offset; i < Width - Offset; i++)
            {
                for (int j = Offset; j < Height - Offset; j++)
                {
                    Color c = whiteBitmap.GetPixel(i, j);
                    whiteBitmap.SetPixel(i, j, Color.FromArgb(0, c));
                }
            }

            // Draw the original image

            g.DrawImage(whiteBitmap, 0, 0);
            g.Dispose();

            return retBitmap;
        }

        public Bitmap DropShadow(Bitmap img, int Offset, int Steps)
        {
            int White = 255;

            int Width = img.Width;
            int Height = img.Height;

            Bitmap retBitmap = new Bitmap(Width + Offset, Height + Offset);
            Graphics g = Graphics.FromImage(retBitmap);

            // Make it white
            g.FillRectangle(Brushes.White, 0, 0, retBitmap.Width, retBitmap.Height);

            int stepOffset = White / Steps;

            // Draw the shadow
            for (int i = 0; i <= Steps; i++)
            {
                Brush b = new SolidBrush(Color.FromArgb(255, White, White, White));
                g.FillRectangle(b, Offset, Offset, Width - i, Height - i);

                White -= stepOffset;

                // Get rid of the old brush
                b.Dispose();
            }

            // Draw the original image

            g.DrawImage(img, 0, 0);
            g.Dispose();

            return retBitmap;
        }


        public Image DropShadow(Image img)
        {

            Image newImage = new Bitmap(img.Width, img.Height);

            Graphics g = Graphics.FromImage(newImage);



            for (int i = 10; i >= 0; i--)
            {
                g.DrawImage(img, new Rectangle(i, i, img.Width - i, img.Height - i), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                break;
            }


            g.Dispose();



            return newImage;

        }
    }
}
