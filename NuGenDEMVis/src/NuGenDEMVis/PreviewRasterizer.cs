using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Genetibase.NuGenDEMVis.Data;

namespace Genetibase.NuGenDEMVis.Raster
{
    class PreviewRasterizer
    {
        public static Image DrawRotatedBandPreview(DataSourceInfo.DataBandInfo[] bands, int imgSize, DataSourceInfo info)
        {
            Image img = new Bitmap(info != null ? imgSize * 2 : imgSize, imgSize);
            DrawRotateBandPreviewToImg(bands, img, info);
            return img;
        }

        public static void DrawRotateBandPreviewToImg(DataSourceInfo.DataBandInfo[] bands, Image image, DataSourceInfo info)
        {
            int sz = info != null ? image.Width / 2 : image.Width;
            int rectSz = (int)Math.Sqrt(((float)sz * sz * 0.8f) / 2f);

            Graphics g = Graphics.FromImage(image);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.TranslateTransform(sz / 2, sz / 2);
            Rectangle rect = new Rectangle(-rectSz / 2,
                                           -rectSz / 2,
                                           rectSz,
                                           rectSz);
            float angle = 0;
            int fontSz = 16;
            Font font = new Font("Tahoma", 16);
            for (int i = bands.Length - 1; i >= 0; i--)
            {
                angle += -22.5f;
                if (i != 0)
                    g.RotateTransform(angle);

                if (bands[i].Image != null)
                    g.DrawImage(bands[i].Image, rect);
                else
                {
                    g.FillRectangle(Brushes.White, rect);
                    g.DrawRectangle(Pens.Black, rect);
                }

                if (i == 0 && bands[i].Name != null && bands[i].Name.Length > 0)
                {
                    SizeF strSz;
                    while (true)
                    {
                        strSz = g.MeasureString(bands[i].Name, font);
                        if (strSz.Width > (rectSz * 0.8))
                            font = new Font("Tahoma", --fontSz);
                        else 
                            break;
                    }
                    
                    g.DrawString(bands[i].Name, font, Brushes.Black, -strSz.Width / 2, -strSz.Height / 2);
                }

                if (i != 0)
                    g.RotateTransform(-angle);
            }

            if (info != null)
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                font = new Font("Tahoma", 10);
                g.TranslateTransform(sz / 2, 0);
                g.DrawString(string.Format("{0} x {1}", info.Resolution.Width, info.Resolution.Height),
                             font, Brushes.Black, 10, -sz / 4);
                g.DrawString(info.Bpp + "-bit", font, Brushes.Black, 10, sz / 4);
            }

            g.Flush();
            g.Dispose();
        }
    }
}