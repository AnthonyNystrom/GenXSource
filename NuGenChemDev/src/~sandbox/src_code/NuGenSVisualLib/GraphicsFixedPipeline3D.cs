using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Pipelines
{
    class GraphicsFixedPipeline3D : GraphicsPipeline3D
    {
        Device device;

        public GraphicsFixedPipeline3D(Device device)
        {
            this.device = device;
        }

        public override Matrix WorldMatrix
        {
            get
            {
                return device.Transform.World;
            }
            set
            {
                device.Transform.World = value;
            }
        }

        public override Matrix ViewMatrix
        {
            get
            {
                return device.Transform.View;
            }
            set
            {
                device.Transform.View = value;
            }
        }

        public override Microsoft.DirectX.Matrix ProjectionMatrix
        {
            get
            {
                return device.Transform.Projection;
            }
            set
            {
                device.Transform.Projection = value;
            }
        }

        public override Material Material
        {
            get
            {
                return device.Material;
            }
            set
            {
                device.Material = value;
            }
        }
    }
}