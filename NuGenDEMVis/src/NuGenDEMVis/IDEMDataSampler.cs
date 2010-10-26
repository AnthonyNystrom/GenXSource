using System.Drawing;
using Genetibase.RasterDatabase;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis
{
    public interface IDEMDataSampler
    {
        Bitmap GenerateBitmap(Size size, DataLayer layer);
        Bitmap GenerateBitmap(Size size, DataArea area);
        Texture GenerateTexture(Size size, DataLayer layer, Device gDevice);
        Texture GenerateTexture(Size size, DataArea area, Device gDevice);
    }
}