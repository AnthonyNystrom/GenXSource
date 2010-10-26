using System.Drawing;
using System.Drawing.Imaging;
using Genetibase.RasterDatabase;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Raster.Samplers
{
    struct Bitmap32bitARGBPixel
    {
        public byte B, G, R, A;
    }

    abstract class DiffuseMapDEMSampler : IDEMDataSampler
    {
        public abstract Bitmap GenerateBitmap(Size size, DataLayer layer);
        public abstract Bitmap GenerateBitmap(Size size, DataArea area);
        public abstract Texture GenerateTexture(Size size, DataLayer layer, Device gDevice);
        public abstract Texture GenerateTexture(Size size, DataArea area, Device gDevice);
    }

    abstract class SimpleDiffuseMapDEMSampler : DiffuseMapDEMSampler
    {
        protected abstract unsafe void SetPixel(int x, int y, byte value, Bitmap32bitARGBPixel* pixel);
        protected abstract unsafe void SetPixel(int x, int y, float height, Bitmap32bitARGBPixel* pixel);

        public override Bitmap GenerateBitmap(Size size, DataLayer layer)
        {
            throw new System.NotImplementedException();
        }

        public override Bitmap GenerateBitmap(Size size, DataArea area)
        {
            SizeF scale = new SizeF(area.Area.Width / size.Width, area.Area.Height / size.Height);
            PointF startPos = area.Area.Location;
            PointF pos = startPos;

            Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, size.Width, size.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                Bitmap32bitARGBPixel* pixels = (Bitmap32bitARGBPixel*)data.Scan0;
                if (area.Data is byte[])
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        pos.X = startPos.X;
                        for (int x = 0; x < size.Width; x++)
                        {
                            // take sample
                            byte value = (byte)area[(int)pos.X, (int)pos.Y];
                            //bitmap.SetPixel(x, y, GetPixel((int)pos.X, (int)pos.Y, value));
                            SetPixel((int)pos.X, (int)pos.Y, value, pixels);

                            pos.X += scale.Width;
                            pixels++;
                        }
                        pos.Y += scale.Height;
                    }
                }
                else if (area.Data is float[])
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        pos.X = startPos.X;
                        for (int x = 0; x < size.Width; x++)
                        {
                            // take sample
                            float rawValue = (float)area[(int)pos.X, (int)pos.Y];
                            float value = rawValue / area.MaxDataValue;
                            //bitmap.SetPixel(x, y, Color.FromArgb(255, value, value, value));
                            SetPixel((int)pos.X, (int)pos.Y, value, pixels);

                            pos.X += scale.Width;
                            pixels++;
                        }
                        pos.Y += scale.Height;
                    }
                }
            }
            bitmap.UnlockBits(data);

            return bitmap;
        }
    }
}