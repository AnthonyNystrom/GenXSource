using System.Drawing;
using Genetibase.NuGenDEMVis.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Raster
{
    class TextureWriter
    {
        Device device;
        DataSource dataSource;
        double[] areaData;

        Vector4 FillR32FTexture(Vector2 pTexCoord, Vector2 pTexelClipSize)
        {
            return new Vector4();
        }

        public Texture WriteToR32F(Rectangle area, int numLevels, Usage usage, Pool pool)
        {
            // create texture
            Texture tex = new Texture(device, area.Width, area.Height, numLevels, usage, Format.R32F, pool);
            
            // fill with data
            //TextureLoader.FillTexture(tex, new Fill2DTextureCallback(FillR32FTexture));
            if (areaData == null || areaData.Length != area.Width * area.Height)
                areaData = new double[area.Width * area.Height];

            dataSource.Sample(area.Location, area.Size, ref areaData);
            GraphicsStream gs = tex.LockRectangle(0, LockFlags.None);

            for (int i = 0; i < areaData.Length; i++)
            {
                gs.Write((float)areaData[i]);
            }
            tex.UnlockRectangle(0);
            
            return tex;
        }
    }
}