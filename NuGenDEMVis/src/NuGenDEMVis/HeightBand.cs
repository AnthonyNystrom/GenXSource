using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenDEMVis.Raster
{
    class HeightBandRange
    {
        readonly SortedList<float, Color> bands;

        public HeightBandRange()
        {
            bands = new SortedList<float, Color>();
        }

        public void AddBand(float height, Color clr)
        {
            bands.Add(height, clr);
        }

        public void RemoveBand(float height)
        {
            bands.Remove(height);
        }

        public Bitmap DrawOutput(int length, int width)
        {
            Bitmap bitmap = new Bitmap(width, length);
            Graphics g = Graphics.FromImage(bitmap);
            DrawOutputToGraphics(g);
            g.Dispose();
            return bitmap;
        }

        public void DrawOutputToGraphics(Graphics g)
        {
            int width = (int)g.ClipBounds.Width;
            int height = (int)g.ClipBounds.Height;
            float scale = 1f / height;

            // draw each gradient set
            KeyValuePair<float, Color> previous = new KeyValuePair<float, Color>();
            bool hp = false;
            foreach (KeyValuePair<float, Color> pair in bands)
            {
                if (hp)
                {
                    int start = (int)(previous.Key * scale);
                    Rectangle rect = new Rectangle(0, start, width, (int)(pair.Key * scale) - start);
                    using (LinearGradientBrush brush = new LinearGradientBrush(rect, previous.Value, pair.Value, LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
                previous = pair;
                hp = true;
            }
        }

        public Color this[float height]
        {
            get
            {
                if (bands == null)
                    return Color.Black;

                // locate near bands
                Color belowClr = Color.Empty, aboveClr = Color.Empty;
                float belowHeight = float.NaN, aboveHeight = float.NaN;
                foreach (KeyValuePair<float, Color> pair in bands)
                {
                    if (pair.Key < height)
                    {
                        belowClr = pair.Value;
                        belowHeight = pair.Key;
                    }
                    else if (pair.Key > height)
                    {
                        aboveClr = pair.Value;
                        aboveHeight = pair.Key;
                        break;
                    }
                }

                // calculate final clr
                if (aboveHeight == float.NaN)
                    return belowClr;
                if (belowHeight == float.NaN)
                    return aboveClr;

                float clrScale = (float)(height - belowHeight) / (aboveHeight - belowHeight);

                byte A = (byte)(belowClr.A + ((aboveClr.A - belowClr.A) * clrScale));
                byte R = (byte)(belowClr.R + ((aboveClr.R - belowClr.R) * clrScale));
                byte G = (byte)(belowClr.G + ((aboveClr.G - belowClr.G) * clrScale));
                byte B = (byte)(belowClr.B + ((aboveClr.B - belowClr.B) * clrScale));

                return Color.FromArgb(A, R, G, B);
            }
        }
    }
}