using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Genetibase.UI.NuGenImageWorks
{
    class Effects
    {
        private Random rand = new Random();
        
        private class HLight
        {
            public Point Location;
            public Color Color;
        }

        private Color fogtopdensity, fogcenterdensity, fogbottomdensity;
        private double fogcenter, fogfreqx, fogfreqy;
        private Color lightlight;
        private double rainnumber, rainangle, rainvelocity;
        private Color raindensity;

        private double hlightsize, hlightsizevar;
        private double hlightradius, hlightradiusvar;
        private double hlightbright;
        private double hlsteps;
        private double motionangle, motionvelocity;
        private double zoomoffsetx, zoomoffsety, zoomzoom;
        private double depthoffsetx, depthoffsety, depthradius;
        private double focusoffsetx, focusoffsety, focusscale;
        private Color filterfilter;

        private double exposuretiming, exposurecorrection;
        private double enhancesaturation, enhancecontrast;

        public Effects()
        {
            fogtopdensity = fogcenterdensity = fogbottomdensity = Color.FromArgb(0, Color.White);
            fogcenter = 0.5;
            fogfreqx = 0.32;
            fogfreqy = 0.16;
            lightlight = Color.Black;
            rainnumber = 0.5;
            rainangle = 0.333;
            rainvelocity = 0.25;
            raindensity = Color.Black;

            hlightsize = 0.5;
            hlightsizevar = 0.0;
            hlightradius = 0.5;
            hlightradiusvar = 0.5;
            hlightbright = 0.8;
            hlsteps = 0.0;
            motionangle = 0.0;
            motionvelocity = 0.0;
            zoomoffsetx = zoomoffsety = 0.0;
            zoomzoom = 0.0;
            depthoffsetx = depthoffsety = 0.0;
            depthradius = 0.0;
            focusoffsetx = focusoffsety = 0.0;
            focusscale = 0.0;
            filterfilter = Color.Black;

            exposuretiming = 0.0;
            exposurecorrection = 0.0;
            enhancesaturation = 1.0;
            enhancecontrast = 0.0;
        }

        private int Mod(int a, int b)
        {
            int n = (int)(a / b);

            a -= n * b;

            if (a < 0)
                return a + b;

            return a;
        }

        private double Noise2D(int x, int y)
        {
            int n = x + y * 57;
            n = (n << 13) ^ n;

            return (double)(1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }
        private double InterpolateNoise2D(double start, double end, double t)
        {
            return start + t * (end - start);
        }
        private double SmoothNoise(int x, int y)
        {
            double corners = (Noise2D(x - 1, y - 1) + Noise2D(x + 1, y - 1) + Noise2D(x - 1, y + 1) + Noise2D(x + 1, y + 1)) / 16.0;
            double sides = (Noise2D(x - 1, y) + Noise2D(x + 1, y) + Noise2D(x, y - 1) + Noise2D(x, y + 1)) / 8.0;
            double center = Noise2D(x, y) / 4.0;

            return corners + sides + center;
        }
        private double GetNoise2DValue(double x, double y)
        {
            int Xint = (int)x;
            int Yint = (int)y;
            double Xfrac = x - Xint;
            double Yfrac = y - Yint;

            double x0y0 = SmoothNoise(Xint, Yint);
            double x1y0 = SmoothNoise(Xint + 1, Yint);
            double x0y1 = SmoothNoise(Xint, Yint + 1);
            double x1y1 = SmoothNoise(Xint + 1, Yint + 1);

            double v1 = InterpolateNoise2D(x0y0, x1y0, Xfrac);
            double v2 = InterpolateNoise2D(x0y1, x1y1, Xfrac);
            double fin = InterpolateNoise2D(v1, v2, Yfrac);

            return fin;
        }

        private void Fog(Bitmap bitmap, Color topdensity, Color centerdensity, Color bottomdensity, double center, double freqx, double freqy)
        {
            Bitmap fog = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g = Graphics.FromImage(fog);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            Color[] col = new Color[] { topdensity, centerdensity, bottomdensity };
            float[] pos = new float[] { 0.0f, (float)center, 1.0f };

            ColorBlend blend = new ColorBlend();
            blend.Colors = col;
            blend.Positions = pos;
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            brush.InterpolationColors = blend;

            g.FillRectangle(brush, rect);

            BitmapData data = fog.LockBits(new Rectangle(0, 0, fog.Width, fog.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                int* pixels = (int*)data.Scan0;
                int stride = data.Stride / 4;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color col2 = Color.FromArgb(pixels[y * stride + x]);
                        double A = col2.A;

                        double n = 0.0;
                        double freq1 = freqx;
                        double freq2 = freqy;
                        double pers = 1.0;

                        for (int i = 0; i < 4; i++)
                        {
                            n += GetNoise2DValue((double)x * freq1, (double)y * freq2) * pers;
                            freq1 *= 2.0;
                            freq2 *= 2.0;
                            pers *= 0.5;
                        }

                        n = n * 0.5 + 0.5;

                        if (n < 0.0)
                            n = 0.0;
                        else if (n > 1.0)
                            n = 1.0;

                        A *= n;

                        pixels[y * stride + x] = Color.FromArgb((int)A, col2).ToArgb();

                        //System.Windows.Forms.Application.DoEvents();
                    }
                }
            }

            fog.UnlockBits(data);

            Graphics g2 = Graphics.FromImage(bitmap);
            g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g2.SmoothingMode = SmoothingMode.HighQuality;
            g2.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g2.CompositingQuality = CompositingQuality.HighQuality;

            g2.DrawImage(fog, 0, 0, bitmap.Width, bitmap.Height);
        }

        private void Light(Bitmap bitmap, Color light)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                int size = bitmap.Width * bitmap.Height;
                int* pixels = (int*)data.Scan0;

                for (int i = 0; i < size; i++)
                {
                    Color col = Color.FromArgb(pixels[i]);
                    double R = col.R;
                    double G = col.G;
                    double B = col.B;

                    R += (double)light.R;
                    G += (double)light.G;
                    B += (double)light.B;

                    if (R > 255.0)
                        R = 255.0;
                    else if (R < 0.0)
                        R = 0.0;

                    if (G > 255.0)
                        G = 255.0;
                    else if (G < 0.0)
                        G = 0.0;

                    if (B > 255.0)
                        B = 255.0;
                    else if (B < 0.0)
                        B = 0.0;

                    pixels[i] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();

                    //System.Windows.Forms.Application.DoEvents();
                }
            }

            bitmap.UnlockBits(data);
        }

        private void Rain(Bitmap bitmap, double number, Color density, double angle, double velocity)
        {
            Bitmap rainbitmap = new Bitmap(bitmap.Width, bitmap.Height);

            BitmapData raindata = rainbitmap.LockBits(new Rectangle(0, 0, rainbitmap.Width, rainbitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int width = rainbitmap.Width;
                int height = rainbitmap.Height;
                int* pixels = (int*)raindata.Scan0;
                int stride = raindata.Stride / 4;
                int num = (int)((double)(width * height) * number);

                for (int i = 0; i < num; i++)
                {
                    int x = rand.Next(width);
                    int y = rand.Next(height);

                    Color col = Color.FromArgb(pixels[y * stride + x]);
                    double R = col.R;
                    double G = col.G;
                    double B = col.B;

                    R += density.R;
                    G += density.G;
                    B += density.B;

                    if (R > 255.0)
                        R = 255.0;
                    else if (R < 0.0)
                        R = 0.0;

                    if (G > 255.0)
                        G = 255.0;
                    else if (G < 0.0)
                        G = 0.0;

                    if (B > 255.0)
                        B = 255.0;
                    else if (B < 0.0)
                        B = 0.0;

                    pixels[y * stride + x] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();

                    //System.Windows.Forms.Application.DoEvents();
                }
            }

            rainbitmap.UnlockBits(raindata);

            rainbitmap = Motion(rainbitmap, angle, velocity, false);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            raindata = rainbitmap.LockBits(new Rectangle(0, 0, rainbitmap.Width, rainbitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int size = bitmap.Width * bitmap.Height;
                int* pixels = (int*)data.Scan0;
                int* rainpixels = (int*)raindata.Scan0;

                for (int i = 0; i < size; i++)
                {
                    Color col = Color.FromArgb(pixels[i]);
                    Color raincol = Color.FromArgb(rainpixels[i]);

                    double R = (double)col.R / 255.0;
                    double G = (double)col.G / 255.0;
                    double B = (double)col.B / 255.0;
                    double RR = (double)raincol.R / 255.0;
                    double RG = (double)raincol.G / 255.0;
                    double RB = (double)raincol.B / 255.0;

                    R = (R + RR) - (R * RR);
                    G = (G + RG) - (G * RG);
                    B = (B + RB) - (B * RB);

                    if (R > 1.0)
                        R = 1.0;
                    else if (R < 0.0)
                        R = 0.0;

                    if (G > 1.0)
                        G = 1.0;
                    else if (G < 0.0)
                        G = 0.0;

                    if (B > 1.0)
                        B = 1.0;
                    else if (B < 0.0)
                        B = 0.0;

                    pixels[i] = Color.FromArgb((int)(R * 255.0), (int)(G * 255.0), (int)(B * 255.0)).ToArgb();
                    //System.Windows.Forms.Application.DoEvents();
                }
            }

            rainbitmap.UnlockBits(raindata);
            bitmap.UnlockBits(data);
        }

        private void Highlight(Bitmap bitmap, double size, double sizevar, double radius, double radiusvar, double bright, int steps)
        {
            List<HLight> highlights = new List<HLight>();

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                int* pixels = (int*)data.Scan0;
                int stride = data.Stride / 4;

                for (int y = 0; y < height; y += steps)
                {
                    for (int x = 0; x < width; x += steps)
                    {
                        double R = 0.0;
                        double G = 0.0;
                        double B = 0.0;

                        for (int j = -1; j < 2; j++)
                        {
                            for (int i = -1; i < 2; i++)
                            {
                                //System.Windows.Forms.Application.DoEvents();
                                int yj = y + j;
                                int xi = x + i;

                                if (xi >= width)
                                    xi = width - 1;
                                else if (xi < 0)
                                    xi = 0;

                                if (yj >= height)
                                    yj = height - 1;
                                else if (yj < 0)
                                    yj = 0;

                                Color col = Color.FromArgb(pixels[yj * stride + xi]);

                                if (j == 0 && i == 0)
                                {
                                    R -= 8 * col.R;
                                    G -= 8 * col.G;
                                    B -= 8 * col.B;
                                }
                                else
                                {
                                    R += col.R;
                                    G += col.G;
                                    B += col.B;
                                }

                            }
                        }

                        double L = 0.299 * R + 0.587 * G + 0.114 * B;

                        if (L >= 255)
                        {
                            HLight hl = new HLight();
                            hl.Location = new Point(x, y);

                            R /= 255.0;
                            G /= 255.0;
                            B /= 255.0;

                            R = (R + bright) - (R * bright);
                            G = (G + bright) - (G * bright);
                            B = (B + bright) - (B * bright);

                            R *= 255.0;
                            G *= 255.0;
                            B *= 255.0;

                            if (R > 255.0)
                                R = 255.0;
                            else if (R < 0.0)
                                R = 0.0;

                            if (G > 255.0)
                                G = 255.0;
                            else if (G < 0.0)
                                G = 0.0;

                            if (B > 255.0)
                                B = 255.0;
                            else if (B < 0.0)
                                B = 0.0;

                            hl.Color = Color.FromArgb((int)R, (int)G, (int)B);

                            highlights.Add(hl);

                        }
                    }
                }
            }

            bitmap.UnlockBits(data);

            Graphics g = Graphics.FromImage(bitmap);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.CompositingQuality = CompositingQuality.HighQuality;

            foreach (HLight highlight in highlights)
            {
                //System.Windows.Forms.Application.DoEvents();
                float s = (float)(size + rand.NextDouble() * sizevar);
                float r = (float)(radius + rand.NextDouble() * radiusvar);

                GraphicsPath gp = new GraphicsPath();
                gp.StartFigure();

                gp.AddLine(new PointF(-s, -s), new PointF(0, -r));

                gp.AddLine(gp.GetLastPoint(), new PointF(s, -s));
                gp.AddLine(gp.GetLastPoint(), new PointF(r, 0));

                gp.AddLine(gp.GetLastPoint(), new PointF(s, s));
                gp.AddLine(gp.GetLastPoint(), new PointF(0, r));

                gp.AddLine(gp.GetLastPoint(), new PointF(-s, s));
                gp.AddLine(gp.GetLastPoint(), new PointF(-r, 0));

                gp.CloseFigure();

                PathGradientBrush brush = new PathGradientBrush(gp);
                brush.CenterColor = highlight.Color;
                brush.SurroundColors = new Color[] { Color.Transparent };

                g.ResetTransform();
                g.TranslateTransform(highlight.Location.X, highlight.Location.Y);
                g.RotateTransform((float)rand.NextDouble() * 90f);
                g.FillPath(brush, gp);
            }
        }

        private Bitmap Motion(Bitmap bitmap, double angle, double velocity, bool clamp)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            Bitmap newbitmap = new Bitmap(width, height);

            double ang = angle * Math.PI / 180.0;

            double tx = velocity * Math.Cos(ang);
            double ty = -velocity * Math.Sin(ang);
            double cx = (int)(tx / 2.0);
            double cy = (int)(ty / 2.0);
            int steps = (int)velocity;
            int index = 0;

            BitmapData newdata = newbitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* newpixels = (int*)newdata.Scan0;
                int* pixels = (int*)data.Scan0;
                int stride = data.Stride / 4;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double R = 0.0;
                        double G = 0.0;
                        double B = 0.0;

                        double sum = 0.0;

                        for (int i = 0; i < steps; i++)
                        {
                            //System.Windows.Forms.Application.DoEvents();
                            double f = (double)i / (double)steps;

                            int nx = (int)(((double)x - cx) + f * tx);
                            int ny = (int)(((double)y - cy) + f * ty);

                            if (clamp)
                            {
                                if (nx >= width)
                                    nx = width - 1;
                                else if (nx < 0)
                                    nx = 0;

                                if (ny >= height)
                                    ny = height - 1;
                                else if (ny < 0)
                                    ny = 0;
                            }
                            else
                            {
                                if (nx < 0 || nx >= width)
                                    nx = Mod(nx, width);
                                if (ny < 0 || ny >= height)
                                    ny = Mod(ny, height);
                            }

                            Color col = Color.FromArgb(pixels[ny * stride + nx]);

                            R += (double)col.R;
                            G += (double)col.G;
                            B += (double)col.B;

                            sum++;
                        }

                        if (sum > 0)
                        {
                            R /= sum;
                            G /= sum;
                            B /= sum;

                            if (R > 255.0)
                                R = 255.0;
                            else if (R < 0.0)
                                R = 0.0;

                            if (G > 255.0)
                                G = 255.0;
                            else if (G < 0.0)
                                G = 0.0;

                            if (B > 255.0)
                                B = 255.0;
                            else if (B < 0.0)
                                B = 0.0;

                            newpixels[index] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                        }
                        else
                            newpixels[index] = pixels[index];

                        index++;
                    }
                }
            }

            bitmap.UnlockBits(data);
            newbitmap.UnlockBits(newdata);

            return newbitmap;
        }

        private Bitmap Zoom(Bitmap bitmap, double offsetx, double offsety, double zoom)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            Bitmap newbitmap = new Bitmap(width, height);

            int cx = width / 2 + (int)offsetx;
            int cy = height / 2 + (int)offsety;
            int index = 0;

            int steps = (int)(zoom * Math.Sqrt(cx * cx + cy * cy));

            BitmapData newdata = newbitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* newpixels = (int*)newdata.Scan0;
                int* pixels = (int*)data.Scan0;
                int stride = data.Stride / 4;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double R = 0.0;
                        double G = 0.0;
                        double B = 0.0;

                        double sum = 0.0;

                        for (int i = 0; i < steps; i++)
                        {
                            //System.Windows.Forms.Application.DoEvents();
                            double s = 1.0 - zoom * ((double)i / (double)steps);

                            int nx = (int)(((double)x - cx) * s) + cx;
                            int ny = (int)(((double)y - cy) * s) + cy;

                            if (nx >= width)
                                nx = width - 1;
                            else if (nx < 0)
                                nx = 0;

                            if (ny >= height)
                                ny = height - 1;
                            else if (ny < 0)
                                ny = 0;

                            Color col = Color.FromArgb(pixels[ny * stride + nx]);

                            R += (double)col.R;
                            G += (double)col.G;
                            B += (double)col.B;

                            sum++;
                        }

                        if (sum > 0)
                        {
                            R /= sum;
                            G /= sum;
                            B /= sum;

                            if (R > 255.0)
                                R = 255.0;
                            else if (R < 0.0)
                                R = 0.0;

                            if (G > 255.0)
                                G = 255.0;
                            else if (G < 0.0)
                                G = 0.0;

                            if (B > 255.0)
                                B = 255.0;
                            else if (B < 0.0)
                                B = 0.0;

                            newpixels[index] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                        }
                        else
                            newpixels[index] = pixels[index];

                        index++;
                    }
                }
            }

            bitmap.UnlockBits(data);
            newbitmap.UnlockBits(newdata);

            return newbitmap;
        }

        private Bitmap Depth(Bitmap bitmap, double offsetx, double offsety, double radius)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            Bitmap newbitmap = new Bitmap(width, height);

            int cx = width / 2 + (int)offsetx;
            int cy = height / 2 + (int)offsety;
            int index = 0;

            BitmapData newdata = newbitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* newpixels = (int*)newdata.Scan0;
                int* pixels = (int*)data.Scan0;
                int stride = data.Stride / 4;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double R = 0.0;
                        double G = 0.0;
                        double B = 0.0;

                        double sum = 0.0;

                        double dx = Math.Abs(x - cx);
                        double dy = Math.Abs(y - cy);

                        int steps = (int)Math.Min(Math.Sqrt(dx * dx + dy * dy) / (radius + 1), 100.0);

                        for (int i = 0; i < steps; i++)
                        {
                            //System.Windows.Forms.Application.DoEvents();
                            double f = (double)i / (double)steps;

                            int nx = (int)(((double)x - cx) + f * (double)steps) + cx;

                            if (nx >= width)
                                nx = width - 1;
                            else if (nx < 0)
                                nx = 0;

                            Color col = Color.FromArgb(pixels[y * stride + nx]);

                            R += (double)col.R;
                            G += (double)col.G;
                            B += (double)col.B;

                            sum++;
                        }

                        if (sum > 0)
                        {
                            R /= sum;
                            G /= sum;
                            B /= sum;

                            if (R > 255.0)
                                R = 255.0;
                            else if (R < 0.0)
                                R = 0.0;

                            if (G > 255.0)
                                G = 255.0;
                            else if (G < 0.0)
                                G = 0.0;

                            if (B > 255.0)
                                B = 255.0;
                            else if (B < 0.0)
                                B = 0.0;

                            newpixels[index] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                        }
                        else
                            newpixels[index] = pixels[index];

                        index++;
                    }
                }
            }

            bitmap.UnlockBits(data);
            newbitmap.UnlockBits(newdata);

            Bitmap newbitmap2 = new Bitmap(width, height);

            index = 0;

            BitmapData newdata2 = newbitmap2.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            newdata = newbitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* newpixels2 = (int*)newdata2.Scan0;
                int* newpixels = (int*)newdata.Scan0;
                int stride = newdata.Stride / 4;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double R = 0.0;
                        double G = 0.0;
                        double B = 0.0;

                        double sum = 0.0;

                        double dx = Math.Abs(x - cx);
                        double dy = Math.Abs(y - cy);

                        int steps = (int)(Math.Sqrt(dx * dx + dy * dy) / radius);

                        for (int i = 0; i < steps; i++)
                        {
                            //System.Windows.Forms.Application.DoEvents();
                            double f = (double)i / (double)steps;

                            int ny = (int)(((double)y - cy) + f * (double)steps) + cy;

                            if (ny >= height)
                                ny = height - 1;
                            else if (ny < 0)
                                ny = 0;

                            Color col = Color.FromArgb(newpixels[ny * stride + x]);

                            R += (double)col.R;
                            G += (double)col.G;
                            B += (double)col.B;

                            sum++;
                        }

                        if (sum > 0)
                        {
                            R /= sum;
                            G /= sum;
                            B /= sum;

                            if (R > 255.0)
                                R = 255.0;
                            else if (R < 0.0)
                                R = 0.0;

                            if (G > 255.0)
                                G = 255.0;
                            else if (G < 0.0)
                                G = 0.0;

                            if (B > 255.0)
                                B = 255.0;
                            else if (B < 0.0)
                                B = 0.0;

                            newpixels2[index] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                        }
                        else
                            newpixels2[index] = newpixels[index];

                        index++;
                    }
                }
            }

            newbitmap.UnlockBits(newdata);
            newbitmap2.UnlockBits(newdata2);

            return newbitmap2;
        }

        private void Focus(Bitmap bitmap, double offsetx, double offsety, double scale)
        {
            Graphics g = Graphics.FromImage(bitmap);

            Rectangle rect = new Rectangle(-1, -1, bitmap.Width + 1, bitmap.Height + 1);
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(rect);

            PathGradientBrush brush = new PathGradientBrush(gp);
            brush.CenterPoint = new PointF((float)offsetx + (float)bitmap.Width / 2f, (float)offsety + (float)bitmap.Height / 2f);
            brush.CenterColor = Color.Transparent;
            brush.SurroundColors = new Color[] { Color.Black };
            brush.FocusScales = new PointF((float)scale, (float)scale);

            g.FillEllipse(brush, rect);
            g.SetClip(gp, CombineMode.Exclude);
            g.FillRectangle(Brushes.Black, rect);
        }

        private void Filter(Bitmap bitmap, Color filter)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                int size = bitmap.Width * bitmap.Height;
                int* pixels = (int*)data.Scan0;

                for (int i = 0; i < size; i++)
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Color col = Color.FromArgb(pixels[i]);
                    double R = col.R;
                    double G = col.G;
                    double B = col.B;
                    double L = 0.299 * R + 0.587 * G + 0.114 * B;

                    double fact = L / 255.0;

                    R -= ((double)filter.R) * fact;
                    G -= ((double)filter.G) * fact;
                    B -= ((double)filter.B) * fact;

                    if (R > 255.0)
                        R = 255.0;
                    else if (R < 0.0)
                        R = 0.0;

                    if (G > 255.0)
                        G = 255.0;
                    else if (G < 0.0)
                        G = 0.0;

                    if (B > 255.0)
                        B = 255.0;
                    else if (B < 0.0)
                        B = 0.0;

                    pixels[i] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                }
            }

            bitmap.UnlockBits(data);
        }

        private void Exposure(Bitmap bitmap, double timing, double correction)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                int size = bitmap.Width * bitmap.Height;
                int* pixels = (int*)data.Scan0;

                for (int i = 0; i < size; i++)
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Color col = Color.FromArgb(pixels[i]);
                    double R = col.R;
                    double G = col.G;
                    double B = col.B;

                    R *= timing;
                    G *= timing;
                    B *= timing;

                    if (correction != 1.0)
                    {
                        R = Math.Pow(R / 255.0, correction) * 255.0;
                        G = Math.Pow(G / 255.0, correction) * 255.0;
                        B = Math.Pow(B / 255.0, correction) * 255.0;
                    }

                    if (R > 255.0)
                        R = 255.0;
                    else if (R < 0.0)
                        R = 0.0;

                    if (G > 255.0)
                        G = 255.0;
                    else if (G < 0.0)
                        G = 0.0;

                    if (B > 255.0)
                        B = 255.0;
                    else if (B < 0.0)
                        B = 0.0;

                    pixels[i] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                }
            }

            bitmap.UnlockBits(data);
        }

        public void Enhance(Bitmap bitmap, double saturation, double contrast)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                int size = bitmap.Width * bitmap.Height;
                int* pixels = (int*)data.Scan0;

                for (int i = 0; i < size; i++)
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Color col = Color.FromArgb(pixels[i]);
                    double R = col.R;
                    double G = col.G;
                    double B = col.B;

                    if (saturation < 1.0)
                    {
                        Color aux = Program.HSL2RGB(col.GetHue() / 360.0, col.GetSaturation() * saturation, col.GetBrightness());

                        R = aux.R;
                        G = aux.G;
                        B = aux.B;
                    }

                    if (contrast != 0.0)
                    {
                        double c;
                        if (contrast < -100)
                            c = 0.0;
                        else if (contrast > 100)
                            c = 2.0;
                        else
                            c = (100.0 + contrast) / 100.0;

                        c *= c;

                        double red = R / 255.0;
                        red -= 0.5;
                        red *= c;
                        red += 0.5;
                        R = red * 255.0;

                        double green = G / 255.0;
                        green -= 0.5;
                        green *= c;
                        green += 0.5;
                        G = green * 255.0;

                        double blue = B / 255.0;
                        blue -= 0.5;
                        blue *= c;
                        blue += 0.5;
                        B = blue * 255.0;
                    }

                    if (R > 255.0)
                        R = 255.0;
                    else if (R < 0.0)
                        R = 0.0;

                    if (G > 255.0)
                        G = 255.0;
                    else if (G < 0.0)
                        G = 0.0;

                    if (B > 255.0)
                        B = 255.0;
                    else if (B < 0.0)
                        B = 0.0;

                    pixels[i] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                }
            }

            bitmap.UnlockBits(data);
        }

        public Bitmap Do(Bitmap bitmap)
        {          
            try
            {
                Bitmap newbitmap = (Bitmap)bitmap.Clone();

                double mw = (double)bitmap.Width / 2.0;
                double mh = (double)bitmap.Height / 2.0;
                double mwh = Math.Sqrt(mw * mw + mh * mh) / 2.0;

                //System.Windows.Forms.Application.DoEvents();
                if (fogtopdensity.A > 0 || fogcenterdensity.A > 0 || fogbottomdensity.A > 0)
                    Fog(newbitmap, fogtopdensity, fogcenterdensity, fogbottomdensity, fogcenter, 1.0 / (100.0 * fogfreqx), 1.0 / (100.0 * fogfreqy));

                //System.Windows.Forms.Application.DoEvents();
                if (lightlight.R > 0 || lightlight.G > 0 || lightlight.B > 0)
                    Light(newbitmap, lightlight);

                //System.Windows.Forms.Application.DoEvents();
                if (raindensity.R > 0 || raindensity.G > 0 || raindensity.B > 0)
                    Rain(newbitmap, rainnumber, raindensity, rainangle * 180.0, rainvelocity * 100.0);

                //System.Windows.Forms.Application.DoEvents();
                if (hlsteps > 0.0)
                    Highlight(newbitmap, hlightsize * 2.0, hlightsizevar * 2.0, hlightradius * 20.0, hlightradiusvar * 20.0, hlightbright, 26 - (int)(hlsteps * 25.0));

                //System.Windows.Forms.Application.DoEvents();
                if (motionvelocity > 0.0)
                    newbitmap = Motion(newbitmap, motionangle * 180.0, motionvelocity * 100.0, true);

                //System.Windows.Forms.Application.DoEvents();
                if (zoomzoom > 0.0)
                    newbitmap = Zoom(newbitmap, zoomoffsetx * mw, zoomoffsety * mh, zoomzoom);

                //System.Windows.Forms.Application.DoEvents();
                if (depthradius > 0.0)
                    newbitmap = Depth(newbitmap, depthoffsetx * mw, depthoffsety * mh, (mwh + 1.0) - depthradius * mwh);

                //System.Windows.Forms.Application.DoEvents();
                if (focusscale > 0.0)
                    Focus(newbitmap, focusoffsetx * mw, focusoffsety * mh, 1.0 - focusscale);

                //System.Windows.Forms.Application.DoEvents();
                if (filterfilter.R > 0 || filterfilter.G > 0 || filterfilter.B > 0)
                    Filter(newbitmap, filterfilter);

                //System.Windows.Forms.Application.DoEvents();
                if (exposuretiming > 0.0 || exposurecorrection != 0.0)
                    Exposure(newbitmap, 1.0 + exposuretiming * 2.0, 1.0 / (1.0 + exposurecorrection * 2.0));

                //System.Windows.Forms.Application.DoEvents();
                if (enhancesaturation < 1.0 || enhancecontrast != 0.0)
                    Enhance(newbitmap, enhancesaturation, enhancecontrast * 100.0);

                return newbitmap;

            }
            catch (Exception) { }

            return bitmap;
           
        }

        public Color FogTopDensity
        {
            get { return fogtopdensity; }
            set { fogtopdensity = value; }
        }
        public Color FogCenterDensity
        {
            get { return fogcenterdensity; }
            set { fogcenterdensity = value; }
        }
        public Color FogBottomDensity
        {
            get { return fogbottomdensity; }
            set { fogbottomdensity = value; }
        }
        public double FogCenter
        {
            get { return fogcenter; }
            set { fogcenter = value; }
        }
        public double FogFreqX
        {
            get { return fogfreqx; }
            set { fogfreqx = value; }
        }
        public double FogFreqY
        {
            get { return fogfreqy; }
            set { fogfreqy = value; }
        }
        public Color LightLight
        {
            get { return lightlight; }
            set { lightlight = value; }
        }
        public double RainNumber
        {
            get { return rainnumber; }
            set { rainnumber = value; }
        }
        public double RainAngle
        {
            get { return rainangle; }
            set { rainangle = value; }
        }
        public double RainVelocity
        {
            get { return rainvelocity; }
            set { rainvelocity = value; }
        }
        public Color RainDensity
        {
            get { return raindensity; }
            set { raindensity = value; }
        }

        public double HLightSize
        {
            get { return hlightsize; }
            set { hlightsize = value; }
        }
        public double HLightSizeVar
        {
            get { return hlightsizevar; }
            set { hlightsizevar = value; }
        }
        public double HLightRadius
        {
            get { return hlightradius; }
            set { hlightradius = value; }
        }
        public double HLightRadiusVar
        {
            get { return hlightradiusvar; }
            set { hlightradiusvar = value; }
        }
        public double HLightBright
        {
            get { return hlightbright; }
            set { hlightbright = value; }
        }
        public double HLightSteps
        {
            get { return hlsteps; }
            set { hlsteps = value; }
        }
        public double MotionAngle
        {
            get { return motionangle; }
            set { motionangle = value; }
        }
        public double MotionVelocity
        {
            get { return motionvelocity; }
            set { motionvelocity = value; }
        }
        public double ZoomOffsetX
        {
            get { return zoomoffsetx; }
            set { zoomoffsetx = value; }
        }
        public double ZoomOffsetY
        {
            get { return zoomoffsety; }
            set { zoomoffsety = value; }
        }
        public double ZoomZoom
        {
            get { return zoomzoom; }
            set { zoomzoom = value; }
        }
        public double DepthOffsetX
        {
            get { return depthoffsetx; }
            set { depthoffsetx = value; }
        }
        public double DepthOffsetY
        {
            get { return depthoffsety; }
            set { depthoffsety = value; }
        }
        public double DepthRadius
        {
            get { return depthradius; }
            set { depthradius = value; }
        }
        public double FocusOffsetX
        {
            get { return focusoffsetx; }
            set { focusoffsetx = value; }
        }
        public double FocusOffsetY
        {
            get { return focusoffsety; }
            set { focusoffsety = value; }
        }
        public double FocusScale
        {
            get { return focusscale; }
            set { focusscale = value; }
        }
        public Color FilterFilter
        {
            get { return filterfilter; }
            set { filterfilter = value; }
        }

        public double ExposureTiming
        {
            get { return exposuretiming; }
            set { exposuretiming = value; }
        }
        public double ExposureCorrection
        {
            get { return exposurecorrection; }
            set { exposurecorrection = value; }
        }
        public double EnhanceSaturation
        {
            get { return enhancesaturation; }
            set { enhancesaturation = value; }
        }
        public double EnhanceContrast
        {
            get { return enhancecontrast; }
            set { enhancecontrast = value; }
        }
    }
}
