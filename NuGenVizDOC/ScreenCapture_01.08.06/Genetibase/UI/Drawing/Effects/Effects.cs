using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Genetibase.UI.NuGenImageWorks;

namespace Genetibase.UI.Drawing
{
	
	public class FloorReflectionFilterProp
        {
            public int AlphaStart = 150;
            public int AlphaEnd = 50;

            public DockStyle DockPosition = DockStyle.Bottom;
            public int Offset = 50;

            public FloorReflectionFilterProp(int alphaStart, int alphaEnd, DockStyle dockPosition, int offset)
            {
                this.AlphaStart = alphaStart;
                this.AlphaEnd = alphaEnd;
                this.DockPosition = dockPosition;
                this.Offset = offset;
            }
        }

        public class BoxFilterProp
        {
            public int BoxDepth = 30;
            public int Angle = 0;

            public Color BoxStartColor = Color.Empty;
            public Color BoxEndColor = Color.Empty;
            public Color BackColor = Color.Empty;

            public bool DrawImageOnSides = false;

            public BoxFilterProp(int boxDepth, int angle, Color startColor, Color endColor,Color backColor, bool drawImageOnSides)
            {
                this.BoxDepth = boxDepth;
                this.Angle = angle;
                this.BoxStartColor = startColor;
                this.BoxEndColor = endColor;
                this.BackColor = backColor;
                this.DrawImageOnSides = drawImageOnSides;
            }
        }
	
    public class Effects
    {
        
        public static bool Rounded = false;

        public static Font waterMarkFont = SystemFonts.DefaultFont;
        public static ContentAlignment textAlign = ContentAlignment.BottomCenter;
        public static ContentAlignment imageAlign = ContentAlignment.TopCenter;
        public static Image waterMarkImage = null;
        public static String waterMarkText = null;


        // From NuGenImageWorks

        private static BoxFilterProp boxFilter = new BoxFilterProp(30, 0, Color.DarkBlue, Color.LightBlue,Color.White, false);
        private static FloorReflectionFilterProp floorReflectionFilter = new FloorReflectionFilterProp(150, 50, System.Windows.Forms.DockStyle.None, 50);

        public static FloorReflectionFilterProp FloorReflectionFilter
        {
            get { return Effects.floorReflectionFilter; }
            set { Effects.floorReflectionFilter = value; }
        }

        public static int curvature = 0;

        public static BoxFilterProp BoxFilter
        {
            get { return Effects.boxFilter; }
            set { Effects.boxFilter = value; }
        }
       
        // End From NuGenImageWorks


        public static Bitmap Box(Image bitmap)
        {
        	if (Effects.BoxFilter.Angle != 0)
            {
                Genetibase.BasicFilters.BoxFilter f = new Genetibase.BasicFilters.BoxFilter();
                f.DrawImageOnSides = Effects.BoxFilter.DrawImageOnSides;
                f.BoxStartColor = Effects.BoxFilter.BoxStartColor;
                f.BoxEndColor = Effects.BoxFilter.BoxEndColor;
                f.BoxDepth = (int)(Effects.BoxFilter.BoxDepth);
                f.Angle = Effects.BoxFilter.Angle;
                f.BackGroundColor = Effects.BoxFilter.BackColor;

                return (Bitmap)f.ExecuteFilter(bitmap);
            }
        	return (Bitmap)bitmap;
        }
        
        public static Bitmap FishEye(Image bitmap)
        {
        	 if (Effects.curvature != 0)
            {
                Genetibase.BasicFilters.FisheyeFilter f = new Genetibase.BasicFilters.FisheyeFilter();
                f.Curvature = (float)Effects.curvature / 100;
                return (Bitmap)f.ExecuteFilter(bitmap);
            }
        	 return (Bitmap)bitmap;
        }
        
        public static Bitmap FloorReflection(Image bitmap)
        {
        	if (Effects.FloorReflectionFilter.DockPosition != DockStyle.None)
            {
                Genetibase.BasicFilters.FloorReflectionFilter f = new Genetibase.BasicFilters.FloorReflectionFilter();
                f.AlphaEnd = Effects.FloorReflectionFilter.AlphaEnd;
                f.AlphaStart = Effects.FloorReflectionFilter.AlphaStart;
                f.DockPosition = Effects.FloorReflectionFilter.DockPosition;
                f.Offset = (int)(Effects.FloorReflectionFilter.Offset);

                return (Bitmap)f.ExecuteFilter(bitmap);
            }
        	return (Bitmap)bitmap;
        }

        public static Bitmap waterMark(Image bitmap)
        {
            WaterMark wm = new WaterMark(waterMarkText, textAlign,waterMarkFont, waterMarkImage, imageAlign);
            return (Bitmap)wm.MarkImage(bitmap);
        }
        
        public static void RoundImage(Image bitmap, int roundedDia)
        {
            Brush brush = new TextureBrush(bitmap);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);            
            
            Genetibase.UI.Drawing.Grpahics.FillRoundedRectangle(g, brush, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 30);

            g.Dispose();
            brush.Dispose();

            Rounded = true;
        }

        private static void RoundImageTransparent(Image bitmap, int roundedDia)
        {
            Brush brush = new TextureBrush(bitmap);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);

            Genetibase.UI.Drawing.Grpahics.FillRoundedRectangle(g, brush, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 30);

            g.Dispose();
            brush.Dispose();

            Rounded = true;
        }

        public static void FeatherImage(Image bitmap, Color featherColor)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            //Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen p = new Pen(featherColor);

            p.EndCap = LineCap.AnchorMask;
            p.StartCap = LineCap.AnchorMask;

            int Step = 60;
            int j = 1;
            int alpha = 0;
            int x, y;

            x = Step;
            y = Step;

            for (int i = 0; i <= Step; i++)
            {
                if (alpha >= 250)
                    alpha += 1;
                else
                    alpha += 5;

                if (alpha > 255)
                    alpha = 255;

                p.Color = Color.FromArgb(alpha, featherColor);

                //if (!Rounded)
                g.DrawRectangle(p, x, y, width - x * 2, height - y * 2);
                //else
                    //Genetibase.UI.Drawing.Grpahics.DrawRoundedRectangle(g, p,x, y, width - x * 2, height - y * 2, 30);

                j++;
                x--;
                y--;
            }

            g.Dispose();
            p.Dispose();

            g = null;
            p = null;

            if( Rounded )
                Effects.RoundImage(bitmap, 30);
        }

       /* internal static void FillRoundedRectangle(Graphics g, Rectangle r, int d, Brush b)
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
        } */       

        public static Bitmap Bevel(Bitmap img)
        {

            Bitmap bitmapNew = (Bitmap)img.Clone();

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

        public static Bitmap GrayScale(Bitmap img)
        {
            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            Bitmap b = (Bitmap)img.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - b.Width * 3;

                byte red, green, blue;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            if (Rounded)
                RoundImage(b, 30);

            return b;
        }

        public static Bitmap Transparency(Bitmap img, int alpha)
        {
            Bitmap b = (Bitmap)img.Clone();

            int Width = b.Width;
            int Heigt = b.Height;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Heigt; j++)
                {
                    Color old = b.GetPixel(i, j);
                    Color newC = Color.FromArgb(alpha, old);
                    b.SetPixel(i, j, newC);                   
                }
            }

            if (Rounded)
                RoundImage(b, 30);

            return b;
        }

        //Deprecated
        public static Bitmap DropShadow(Bitmap img, int Offset)
        {
            Bitmap b = (Bitmap)img.Clone();
            Bitmap retBitmap = new Bitmap(img.Width + Offset, img.Height + Offset);
            Graphics g = Graphics.FromImage(retBitmap);

            Effects.FillGrayForDrapShadow(b, Offset);
            Effects.RoundImageTransparent(b, 30);
            Effects.GaussianBlur(b, 4);
            
            //g.DrawImage( b, Offset, Offset);            
            //g.DrawImage(img,0,0 );
            
            g.Dispose();
            g = null;
            return retBitmap;
        }

        public static void FillGrayForDrapShadow(Bitmap b,int Offset)
        {   
            int Height = b.Height;
            int Width = b.Width;

            int count = 0;

            int i, j;

            for (i = 0; i < Width; i++)
            {
                for (j = 0; j < Offset; j++)
                {
                    b.SetPixel(i, j, Color.FromArgb(255, 128 + (Offset - j) * 10, 128 + (Offset - j) * 10, 128 + (Offset - j) * 10));
                }
            }

            for (i = 0; i < Width; i++)
            {
                count = 0;
                for (j = Height - Offset + 1; j < Height; j++)
                {
                    b.SetPixel(i, j, Color.FromArgb(255, 128 + (Offset - count) * 10, 128 + (Offset - count) * 10, 128 + (Offset - count) * 10));
                    count++;
                }
            }

            for (j = 0; j < Height; j++)            
            {
                count = 0;
                for (i = 0; i < Offset; i++)
                {
                    b.SetPixel(i, j, Color.FromArgb(255, 128 + (Offset - count) * 10, 128 + (Offset - count) * 10, 128 + (Offset - count) * 10));
                    count++;
                }
            }

            for (j = 0; j < Height; j++)
            {
                count = 0;
                for (i = Width - Offset; i < Width; i++)
                {
                    b.SetPixel(i, j, Color.FromArgb(255, 128 + (Offset - count) * 10, 128 + (Offset - count) * 10, 128 + (Offset - count) * 10));
                    count++;
                }
            }
        }

        public static void GaussianBlur(Bitmap img, int nWeight)
        {
            Bitmap b = img;
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
            m.Factor = nWeight + 12;

            if (Conv3x3(b, m))
            {
                //if (Rounded)                
                //    RoundImage(b, 30);
            }
        }


        public static Bitmap Sharpen(Bitmap img, int nWeight /* default to 11*/ )
        {
            Bitmap b = (Bitmap)img.Clone();
            ConvMatrix m = new ConvMatrix();
            m.SetAll(0);
            m.Pixel = nWeight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -2;
            m.Factor = nWeight - 8;

            if (Conv3x3(b, m))
            {
                if (Rounded)
                    RoundImage(b, 30);

                return b;
            }

            else return null;
        }


        public static Bitmap Brightness(Bitmap img, int nBrightness)
        {
            Bitmap b = (Bitmap)img.Clone();

            if (nBrightness < -255 || nBrightness > 255)
                return null;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            int nVal = 0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nVal = (int)(p[0] + nBrightness);

                        if (nVal < 0) nVal = 0;
                        if (nVal > 255) nVal = 255;

                        p[0] = (byte)nVal;

                        ++p;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            if (Rounded)
                RoundImage(b, 30);

            return b;
        }

        public static Bitmap Contrast(Bitmap img, int nContrast)
        {
            Bitmap b = (Bitmap)img.Clone();

            if (nContrast < -100) return null;
            if (nContrast > 100) return null;

            double pixel = 0, contrast = (100.0 + nContrast) / 100.0;

            contrast *= contrast;

            int red, green, blue;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

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
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        pixel = red / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[2] = (byte)pixel;

                        pixel = green / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[1] = (byte)pixel;

                        pixel = blue / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[0] = (byte)pixel;

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            if (Rounded)
                RoundImage(b, 30);

            return b;
        }

        public static Bitmap FeatherRectangle(Bitmap img, Color color, int Offset)
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

        public static Bitmap DropShadow(Bitmap img, int Offset, int Steps)
        {
            if( Rounded )
                RoundImageTransparent(img, 30);

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
                if (!Rounded)
                    g.FillRectangle(b, Offset, Offset, Width - i, Height - i);
                else
                    Genetibase.UI.Drawing.Grpahics.FillRoundedRectangle(g, b, Offset, Offset, Width - i, Height - i, 30);

                White -= stepOffset;

                // Get rid of the old brush
                b.Dispose();
            }
            
            //Effects.GaussianBlur(retBitmap, 4);            
            
            // Draw the original image
            g.DrawImage(img, 0, 0);
            g.Dispose();

            return retBitmap;
        }

        public static void Wash(Bitmap b,Color washColor)
        {
            Rectangle rect = new Rectangle( 0,0,b.Width,b.Height);
            Brush backgroundBrush = new TextureBrush(b);
            Graphics g = Graphics.FromImage(b);
            Brush newBrush = new SolidBrush(Color.FromArgb(128, washColor));

            if (!Rounded)
            {
                g.FillRectangle(backgroundBrush, rect);
                g.FillRectangle(newBrush, rect);
            }
            else
            {
                Genetibase.UI.Drawing.Grpahics.FillRoundedRectangle(g, backgroundBrush, rect,30);
                Genetibase.UI.Drawing.Grpahics.FillRoundedRectangle(g, newBrush, rect, 30);                
            }

            newBrush.Dispose();
            newBrush = null;

            g.Dispose();
            backgroundBrush.Dispose();

            g = null;
            backgroundBrush = null;
        }

        private static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
                            (pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
                            (pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }
                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }
    }
}
