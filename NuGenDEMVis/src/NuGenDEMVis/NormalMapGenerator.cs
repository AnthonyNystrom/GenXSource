using System.Drawing;
using System.IO;
using Genetibase.NuGenDEMVis.GIS;
using Genetibase.RasterDatabase;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using OSGeo.GDAL;

namespace Genetibase.NuGenDEMVis
{
    class NormalMapGenerator
    {
        byte[] rData, gData, bData;
        Size ftNativeSz, ftTexSz;
        SizeF ftScaleTemp;
        Size ftShiftTemp;
        GraphicsStream texStream;
        float[] heights;

        public static void GenerateMap(float[] hmValues, Size hmSize, out Vector3[] nmValues)
        {
            // TODO: How does height relate to xy dimensions?
            nmValues = new Vector3[hmSize.Width * hmSize.Height];
            // calculate normal values for each pixel
            float xScale = 0.25f;
            float yScale = 0.25f;
            for (int y = 0; y < hmSize.Height - 1; y++)
            {
                int vPos = y * hmSize.Width;
                float yActual = y * yScale;
                for (int x = 0; x < hmSize.Width - 1; x++)
                {
                    // sample as quad (2 tris)
                    int px1 = vPos + x;
                    int px2 = vPos + x + 1;
                    int px3 = vPos + x + hmSize.Width;
                    float xActual = x * xScale;
                    Vector3 v0 = new Vector3(xActual, hmValues[px1], yActual);
                    Vector3 v1 = new Vector3(xActual + xScale, hmValues[px2], yActual);
                    Vector3 v2 = new Vector3(xActual, hmValues[px3], yActual + yScale);

                    Vector3 e1 = v1 - v0, e2 = v2 - v0;
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    nmValues[px1] += normal;
                    nmValues[px2] += normal;
                    nmValues[px3] += normal;

                    px1 = vPos + x + 1;
                    px2 = vPos + x + hmSize.Width + 1;
                    px3 = vPos + x + hmSize.Width;
                    v0 = new Vector3(xActual + xScale, hmValues[px1], yActual);
                    v1 = new Vector3(xActual + xScale, hmValues[px2], yActual + yScale);
                    v2 = new Vector3(xActual, hmValues[px3], yActual + yScale);

                    e1 = v1 - v0;
                    e2 = v2 - v0;
                    normal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    nmValues[px1] += normal;
                    nmValues[px2] += normal;
                    nmValues[px3] += normal;
                }
            }

            // average values
            // edges
            float value = 1f / 3f;
            for (int y = 0; y < 1; y++)
            {
                int row = (y * hmSize.Height) - y;
                int pos = row * hmSize.Width;
                for (int x = 1; x < hmSize.Width - 1; x++)
                {
                    nmValues[pos + x] *= value;
                }
            }
            for (int x = 0; x < 1; x++)
            {
                int pos = (x * hmSize.Width) - x;
                for (int y = 1; y < hmSize.Height - 1; y++)
                {
                    nmValues[pos + (y * hmSize.Width)] *= value;
                }
            }
            // corners
            nmValues[hmSize.Width - 1] *= 0.5f;
            nmValues[(hmSize.Height - 1) * hmSize.Width] *= 0.5f;
            // insides
            value = 1f / 6f;
            for (int y = 1; y < hmSize.Height - 1; y++)
            {
                int vPos = y * hmSize.Width;
                for (int x = 1; x < hmSize.Width - 1; x++)
                {
                    nmValues[vPos + x] *= value;
                }
            }
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

                rData = new byte[nativeSz.Width * nativeSz.Height];
                gData = new byte[nativeSz.Width * nativeSz.Height];
                bData = new byte[nativeSz.Width * nativeSz.Height];
                rBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, rData, nativeSz.Width, nativeSz.Height, 0, 0);
                gBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, gData, nativeSz.Width, nativeSz.Height, 0, 0);
                bBand.ReadRaster(0, 0, nativeSz.Width, nativeSz.Height, bData, nativeSz.Width, nativeSz.Height, 0, 0);

                /*ftScaleTemp = new SizeF(1, 1);// (float)area.Area.Width / area.DataSize.Width, (float)area.Area.Height / area.DataSize.Height);
                ftScaleTemp = new SizeF(ftScaleTemp.Width * area.Area.Width, ftScaleTemp.Height * area.Area.Height);
                ftShiftTemp = new Size(area.Area.Location);
                ftNativeSz = nativeSz;*/
                ftScaleTemp = new SizeF((float)area.Area.Width / size.Width, (float)area.Area.Height / size.Height);
                ftShiftTemp = new Size(area.Area.Location);
                if (area.Data is byte[])
                {
                    // to heights
                    //TextureLoader.FillTexture(texture, FillTextureByteToHeights);
                    // generate normal acc
                    ftTexSz = size;
                    //texStream = texture.LockRectangle(0, LockFlags.None);
                    /*Texture normalTex = new Texture(gDevice, size.Width, size.Height, 0, Usage.None, Format.A8R8G8B8, Pool.Managed);
                    TextureLoader.FillTexture(normalTex, FillTextureByteToNormalAcc);*/
                    heights = new float[size.Width * size.Height];
                    int index = 0;
                    //SizeF nativeScale = new SizeF((float)nativeSz.Width / size.Width, (float)nativeSz.Height / size.Height);
                    for (int y = 0; y < size.Height; y++)
                    {
                        int yNative = (int)(ftShiftTemp.Height + (y * ftScaleTemp.Width));
                        for (int x = 0; x < size.Width; x++)
                        {
                            int xNative = (int)(ftShiftTemp.Width + (x * ftScaleTemp.Width));
                            int indexNative = (yNative * nativeSz.Width) + xNative;

                            float rValue = rData[indexNative] / 255f;
                            float gValue = gData[indexNative] / 255f;
                            float bValue = bData[indexNative] / 255f;

                            heights[index++] = (rValue + gValue + bValue) / 3f;
                        }
                    }
                    /*texture.UnlockRectangle(0);
                    texture.Dispose();*/

                    // calculate normals
                    Vector3[] normals;
                    GenerateMap(heights, size, out normals);

                    // write to texture
                    texStream = texture.LockRectangle(0, LockFlags.None);
                    foreach (Vector3 normal in normals)
                    {
                        texStream.WriteByte((byte)(((normal.Z / 2) + 0.5f) * 255));
                        texStream.WriteByte((byte)(((-normal.Y / 2) + 0.5f) * 255));
                        texStream.WriteByte((byte)(((normal.X / 2) + 0.5f) * 255));
                        texStream.WriteByte(255);
                    }
                    /*foreach (float height in heights)
                    {
                        texStream.WriteByte(0);
                        texStream.WriteByte((byte)(height * 255));
                        texStream.WriteByte(0);
                        texStream.WriteByte(255);
                    }*/
                    texture.UnlockRectangle(0);
                }
                return texture;
            }
        }

//        private Vector4 FillTextureByteToNormalAcc(Vector2 texCoord, Vector2 texelSz)
//        {
//            // simply accumulate normals
//            int x = (int)(texCoord.X * ftTexSz.Width);
//            int y = (int)(texCoord.Y * ftTexSz.Height);
//
//            int index = (y * ftTexSz.Width) + x;
//
//            // get heights for points to sample
//            texStream.Seek(index * 16, SeekOrigin.Begin);
//            float height = (float)texStream.Read(typeof(float));
//
//            float r = rData[index] / 255f;
//            float g = gData[index] / 255f;
//            float b = bData[index] / 255f;
//
//            return new Vector4((r + g + b) / 6f, 0, 0, 1);
//        }

//        private Vector4 FillTextureByteToHeights(Vector2 texCoord, Vector2 texelSz)
//        {
//            // simply turn RGB to heights
//            int x = ftShiftTemp.Width + (int)(texCoord.X * ftScaleTemp.Width);
//            int y = ftShiftTemp.Height + (int)(texCoord.Y * ftScaleTemp.Height);
//
//            int index = (y * ftNativeSz.Width) + x;
//
//            // sample as quad (2 tris)
//            int px1 = vPos + x;
//            int px2 = vPos + x + 1;
//            int px3 = vPos + x + hmSize.Width;
//            Vector3 v0 = new Vector3(x, hmValues[px1], y);
//            Vector3 v1 = new Vector3(x, hmValues[px2], y);
//            Vector3 v2 = new Vector3(x, hmValues[px3], y);
//
//            Vector3 e1 = v1 - v0, e2 = v2 - v0;
//            Vector3 normal = Vector3.Normalize(Vector3.Cross(e1, e2));
//
//            nmValues[px1] += normal;
//            nmValues[px2] += normal;
//            nmValues[px3] += normal;
//
//            float r = rData[index] / 255f;
//            float g = gData[index] / 255f;
//            float b = bData[index] / 255f;
//
//            return new Vector4((r + g + b) / 6f, 0, 0, 1);
//        }
    }
}