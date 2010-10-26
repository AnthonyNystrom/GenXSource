using System.Drawing;
using Genetibase.RasterDatabase;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Raster.Samplers
{
    class HeightBandDEMSampler : SimpleDiffuseMapDEMSampler
    {
        readonly HeightBandRange heightBand;

        DataArea ftAreaTemp;
        SizeF ftScaleTemp;
        Size ftShiftTemp;
        float ftHScaleTemp;

        /// <summary>
        /// Initializes a new instance of the HeightBandDEMSampler class.
        /// </summary>
        /// <param name="heightBand"></param>
        public HeightBandDEMSampler(HeightBandRange heightBand)
        {
            this.heightBand = heightBand;
        }

        protected override unsafe void SetPixel(int x, int y, byte value, Bitmap32bitARGBPixel* pixel)
        {
            Color clr = heightBand[value / 256f];
            pixel->A = clr.A;
            pixel->R = clr.R;
            pixel->G = clr.G;
            pixel->B = clr.B;
        }

        protected override unsafe void SetPixel(int x, int y, float height, Bitmap32bitARGBPixel* pixel)
        {
            Color clr = heightBand[height];
            pixel->A = clr.A;
            pixel->R = clr.R;
            pixel->G = clr.G;
            pixel->B = clr.B;
        }

        public override Texture GenerateTexture(Size size, DataLayer layer, Device gDevice)
        {
            throw new System.NotImplementedException();
        }

        public override Texture GenerateTexture(Size size, DataArea area, Device gDevice)
        {
            Texture texture = new Texture(gDevice, size.Width, size.Height, 0, Usage.None, Format.A8R8G8B8, Pool.Managed);
            lock (this)
            {
                ftAreaTemp = area;
                ftScaleTemp = new SizeF(1, 1);//(float)area.Area.Width / area.DataSize.Width, (float)area.Area.Height / area.DataSize.Height);
                ftScaleTemp = new SizeF(ftScaleTemp.Width * area.Area.Width, ftScaleTemp.Height * area.Area.Height);
                ftShiftTemp = new Size(area.Area.Location);
                if (area.Data is byte[])
                    TextureLoader.FillTexture(texture, FillTextureByte);
                else if (area.Data is float[])
                    TextureLoader.FillTexture(texture, FillTextureFloat);
            }
            return texture;
        }

        private Vector4 FillTextureFloat(Vector2 texCoord, Vector2 texelSz)
        {
            float height = (float)ftAreaTemp[ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width), ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height)];
            Color clr = heightBand[(height * 2) / ftAreaTemp.MaxDataValue];
            return new Vector4(clr.R / 255f, clr.G / 255f, clr.B / 255f, 1);
        }

        private Vector4 FillTextureByte(Vector2 texCoord, Vector2 texelSz)
        {
            byte height = (byte)ftAreaTemp[ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width), ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height)];
            Color clr = heightBand[height / 256f];
            return new Vector4(clr.R / 255f, clr.G / 255f, clr.B / 255f, 1);
        }
    }
}