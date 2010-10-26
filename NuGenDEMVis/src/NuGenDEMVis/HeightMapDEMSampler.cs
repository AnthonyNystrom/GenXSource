using System.Drawing;
using Genetibase.RasterDatabase;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Raster.Samplers
{
    class HeightMapDEMSampler : DiffuseMapDEMSampler
    {
        DataArea ftAreaTemp;
        SizeF ftScaleTemp;
        Size ftShiftTemp;

        public override Bitmap GenerateBitmap(Size size, DataLayer layer)
        {
            throw new System.NotImplementedException();
        }

        public override Bitmap GenerateBitmap(Size size, DataArea area)
        {
            throw new System.NotImplementedException();
        }

        public override Texture GenerateTexture(Size size, DataLayer layer, Device gDevice)
        {
            throw new System.NotImplementedException();
        }

        public override Texture GenerateTexture(Size size, DataArea area, Device gDevice)
        {
            Texture texture = new Texture(gDevice, size.Width, size.Height, 0, Usage.None, Format.R32F, Pool.Managed);
            lock (this)
            {
                ftAreaTemp = area;
                ftScaleTemp = new SizeF(1, 1);
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
            // TODO: Use real heights rather than 0.0-1.0?
            float height = (float)ftAreaTemp[ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width), ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height)];
            float value = height / ftAreaTemp.MaxDataValue;
            return new Vector4(value, 0, 0, 0);
        }

        private Vector4 FillTextureByte(Vector2 texCoord, Vector2 texelSz)
        {
            byte height = (byte)ftAreaTemp[ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width), ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height)];
            float value = height / 256f;
            return new Vector4(value, 0, 0, 0);
        }
    }
}