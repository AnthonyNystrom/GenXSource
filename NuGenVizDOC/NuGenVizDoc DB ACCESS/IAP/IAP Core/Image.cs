using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IAP_Core
{
    public class Image
    {
        private int width, height;
        private System.Drawing.Bitmap bitmap;
        private System.Drawing.Imaging.BitmapData data;
        private unsafe byte* pixels;

        public Image()
        {
        }
        public Image(System.Drawing.Bitmap bitmap)
        {
            this.bitmap = (System.Drawing.Bitmap)bitmap.Clone();

            this.width = this.bitmap.Width;
            this.height = this.bitmap.Height;
        }

        public void Create(int width, int height)
        {
            this.bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            
            this.width = this.bitmap.Width;
            this.height = this.bitmap.Height;
        }
        public void Create(int width, int height, System.Drawing.Imaging.ColorPalette palette)
        {
            this.bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            this.bitmap.Palette = palette;

            this.width = this.bitmap.Width;
            this.height = this.bitmap.Height;
        }
        public void Load(string filename)
        {
            this.bitmap = new System.Drawing.Bitmap(filename);

            this.width = this.bitmap.Width;
            this.height = this.bitmap.Height;
        }

        public void OpenData(System.Drawing.Imaging.ImageLockMode mode, bool flip)
        {
            this.data = this.bitmap.LockBits(new System.Drawing.Rectangle(0, 0, this.width, this.height), mode, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            unsafe
            {
                this.pixels = (byte*)this.data.Scan0;

                if (flip)
                    this.pixels += this.height * this.data.Stride;
            }
        }
        public void IncLine()
        {
            unsafe { this.pixels += this.data.Stride; }
        }
        public void IncLine(int val)
        {
            unsafe { this.pixels += val * this.data.Stride; }
        }
        public void DecLine()
        {
            unsafe { this.pixels -= this.data.Stride; }
        }
        public void DecLine(int val)
        {
            unsafe { this.pixels -= val * this.data.Stride; }
        }
        public void SetPixel(int x, byte val)
        {
            unsafe { this.pixels[x] = val; }
        }
        public void SetPixel(int x, int y, byte val)
        {
            unsafe { this.pixels[y * this.width + x] = val; }
        }
        public byte GetPixel(int x)
        {
            unsafe { return this.pixels[x]; }
        }
        public byte GetPixel(int x, int y)
        {
            unsafe { return this.pixels[y * this.width + x]; }
        }
        public void CloseData()
        {
            this.bitmap.UnlockBits(this.data);
        }

        public virtual void Save(string filename)
        {
            this.bitmap.Save(filename);
        }

        public void SetColor(System.Drawing.Color from, System.Drawing.Color to)
        {
            System.Drawing.Imaging.ColorPalette palette = this.bitmap.Palette;

            for (int i = 0; i < palette.Entries.Length; i++)
            {
                double f = ((double)i / (double)palette.Entries.Length);

                int r = (int)((double)from.R + ((double)to.R - (double)from.R) * f);
                int g = (int)((double)from.G + ((double)to.G - (double)from.G) * f);
                int b = (int)((double)from.B + ((double)to.B - (double)from.B) * f);

                palette.Entries[i] = System.Drawing.Color.FromArgb(r, g, b);
            }

            this.bitmap.Palette = palette;
        }

        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }
        }
        public int Width
        {
            get
            {
                return this.width;
            }
        }
        public int Height
        {
            get
            {
                return this.height;
            }
        }
        public int[] Histogram
        {
            get
            {
                int[] values = new int[256];

                this.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

                for (int y = 0; y < this.height; y++)
                {
                    for (int x = 0; x < this.width; x++)
                        values[this.GetPixel(x)]++;

                    this.IncLine();
                }

                this.CloseData();

                return values;
            }
        }
    }
}
