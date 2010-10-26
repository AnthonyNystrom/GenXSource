using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Helpers
{
    /// <summary>
    /// Provides helper functions for rendering point sprites
    /// </summary>
    class PointSpriteHelper
    {
        public static readonly float[] zeroPoint = new float[] { 0, 0, 0 };

        public static void InitDeviceReady(Device device, bool scale, float pointSize, float pointScaleB)
        {
            device.RenderState.PointSpriteEnable = true;
            device.RenderState.PointScaleEnable = true;

            device.RenderState.PointSize = pointSize;
            // FIXME: check scaling supported
            device.RenderState.PointScaleEnable = scale;
            if (scale)
                device.RenderState.PointScaleB = pointScaleB;
        }

        public static void DeInitDevice(Device device)
        {
            device.RenderState.PointSpriteEnable = false;
            device.RenderState.PointScaleEnable = false;
        }
    }
}