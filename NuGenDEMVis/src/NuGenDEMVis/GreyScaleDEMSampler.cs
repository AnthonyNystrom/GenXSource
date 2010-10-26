using System.Drawing;
using Genetibase.RasterDatabase;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Raster.Samplers
{
    class GreyScaleDEMSampler : DiffuseMapDEMSampler
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
            SizeF scale = new SizeF(area.Area.Width / size.Width, area.Area.Height / size.Height);
            PointF startPos = area.Area.Location;
            PointF pos = startPos;

            Bitmap bitmap = new Bitmap(size.Width, size.Height);

            if (area.Data is byte[])
            {
                for (int y = 0; y < size.Height; y++)
                {
                    pos.X = startPos.X;
                    for (int x = 0; x < size.Width; x++)
                    {
                        // take sample
                        byte value = (byte)area[(int)pos.X, (int)pos.Y];
                        // TODO: Use unmanaged direct access
                        bitmap.SetPixel(x, y, Color.FromArgb(255, value, value, value));

                        pos.X += scale.Width;
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
                        if (rawValue > 0)
                        {
                            byte value = (byte)((rawValue / area.MaxDataValue) * 255);
                            // TODO: Use unmanaged direct access
                            bitmap.SetPixel(x, y, Color.FromArgb(255, value, value, value));
                        }
                        pos.X += scale.Width;
                    }
                    pos.Y += scale.Height;
                }
            }

            return bitmap;
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
            float height = (float)ftAreaTemp[ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width), ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height)] * 2;
            float value = height / ftAreaTemp.MaxDataValue;
            return new Vector4(value, value, value, 1);
        }

        private Vector4 FillTextureByte(Vector2 texCoord, Vector2 texelSz)
        {
            byte height = (byte)ftAreaTemp[ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width), ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height)];
            float value = height / 256f;
            return new Vector4(value, value, value, 1);
            /*float r, g, b, xSq, ySq, a;
            xSq = 2f * texCoord.X - 1f; xSq *= xSq;
            ySq = 2f * texCoord.Y - 1f; ySq *= ySq;
            a = (float)Math.Sqrt(xSq + ySq);
            if (a > 1.0f)
            {
                a = 1.0f - (a - 1.0f);
            }
            else if (a < 0.2f)
            {
                a = 0.2f;
            }
            r = 1 - texCoord.X;
            g = 1 - texCoord.Y;
            b = texCoord.X;
            return new Vector4(r, g, b, a);*/
        }
    }
}