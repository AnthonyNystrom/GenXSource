using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Maths.Volumes
{
    public class IsosurfaceGenerator2D
    {
        public static Bitmap GenerateBitmapSurface(int width, int height, float xScale, float yScale,
                                                   IVolumeScene scene)
        {
            // create bitmap
            Bitmap bitmap = new Bitmap(width, height);

            // map volume per-pixel
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!scene.IsOutside(new Vector3(x, y, 0)))
                        bitmap.SetPixel(x, y, Color.Black);
                }
            }

            return bitmap;
        }
    }
}