using System.Drawing;
using System.Drawing.Imaging;
using Genetibase.NuGenDEMVis.GIS;
using Genetibase.RasterDatabase;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using OSGeo.GDAL;

namespace Genetibase.NuGenDEMVis
{
    struct Bitmap24bitRGBPixel
    {
        public byte R, G, B;
    }

    class SourceDataDiffuseSampler
    {
        SizeF ftScaleTemp;
        Size ftShiftTemp;
        byte[] rData, gData, bData;
        Size ftNativeSz;

        public static Bitmap SampleRGBDiffuseMap(GDALReader dataSrc, Size size)
        {
            // find bands to use
            int r, g, b;
            dataSrc.Info.GetRGABands(out r, out g, out b);
            Size nativeSz = dataSrc.Info.Resolution;

            Band rBand = dataSrc.GetRasterBand(r + 1);
            Band gBand = dataSrc.GetRasterBand(g + 1);
            Band bBand = dataSrc.GetRasterBand(b + 1);

            // Note: This *should* work
            byte[] rData = new byte[size.Width * size.Height];
            byte[] gData = new byte[size.Width * size.Height];
            byte[] bData = new byte[size.Width * size.Height];
            rBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, rData, size.Width, size.Height, 0, 0);
            gBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, gData, size.Width, size.Height, 0, 0);
            bBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, bData, size.Width, size.Height, 0, 0);

            // combine channel samples into image
            Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            BitmapData data = bitmap.LockBits(new Rectangle(new Point(), size), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                // write data to each pixel
                Bitmap24bitRGBPixel* pixels = (Bitmap24bitRGBPixel*)data.Scan0;
                for (int i = 0; i < rData.Length; i++)
                {
                    pixels->R = bData[i];
                    pixels->G = gData[i];
                    pixels->B = rData[i];

                    pixels++;
                }
            }
            bitmap.UnlockBits(data);

            return bitmap;
        }

        public Texture GenerateTexture(Size size, DataArea area, Device gDevice, GDALReader dataSrc)
        {
            Texture texture = new Texture(gDevice, size.Width, size.Height, 0, Usage.None, Format.A8R8G8B8, Pool.Managed);
            lock (this)
            {
                // find bands to use
                int r, g, b;
                dataSrc.Info.GetRGABands(out r, out g, out b);
                Size nativeSz = dataSrc.Info.Resolution;

                Band rBand = dataSrc.GetRasterBand(r + 1);
                Band gBand = dataSrc.GetRasterBand(g + 1);
                Band bBand = dataSrc.GetRasterBand(b + 1);

                // Note: This *should* work
                rData = new byte[nativeSz.Width * nativeSz.Height];
                gData = new byte[nativeSz.Width * nativeSz.Height];
                bData = new byte[nativeSz.Width * nativeSz.Height];
                rBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, rData, nativeSz.Width, nativeSz.Height, 0, 0);
                gBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, gData, nativeSz.Width, nativeSz.Height, 0, 0);
                bBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, bData, nativeSz.Width, nativeSz.Height, 0, 0);

                ftScaleTemp = new SizeF(1, 1);// (float)area.Area.Width / area.DataSize.Width, (float)area.Area.Height / area.DataSize.Height);
                ftScaleTemp = new SizeF(ftScaleTemp.Width * area.Area.Width, ftScaleTemp.Height * area.Area.Height);
                ftShiftTemp = new Size(area.Area.Location);
                ftNativeSz = nativeSz;
                if (area.Data is byte[])
                    TextureLoader.FillTexture(texture, FillTextureByte);
                /*else if (area.Data is float[])
                    TextureLoader.FillTexture(texture, FillTextureFloat);*/
            }
            return texture;
        }

        private Vector4 FillTextureByte(Vector2 texCoord, Vector2 texelSz)
        {
            int x = ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width);
            int y = ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height);

            int index = (y * ftNativeSz.Width) + x;

            byte r = rData[index];
            byte g = gData[index];
            byte b = bData[index];

            return new Vector4(r / 255f, g / 255f, b / 255f, 1);
        }
    }
}