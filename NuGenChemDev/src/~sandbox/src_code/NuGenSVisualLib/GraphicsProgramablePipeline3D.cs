using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Pipelines
{
    class GraphicsProgramablePipeline3D : GraphicsPipeline3D
    {
        Matrix world, view, proj;
        Matrix result;
        Device device;
        Material material;

        public GraphicsProgramablePipeline3D(Device device)
        {
            this.device = device;
        }

        public override Matrix WorldMatrix
        {
            get
            {
                return world;
            }
            set
            {
                world = value;
                result = Matrix.TransposeMatrix(world * view * proj);
                device.SetVertexShaderConstant(0, result);
            }
        }

        public override Matrix ViewMatrix
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
                result = Matrix.TransposeMatrix(world * view * proj);
                device.SetVertexShaderConstant(0, result);
            }
        }

        public override Matrix ProjectionMatrix
        {
            get
            {
                return proj;
            }
            set
            {
                proj = value;
                result = Matrix.TransposeMatrix(world * view * proj);
                device.SetVertexShaderConstant(0, result);
            }
        }

        public override Material Material
        {
            get
            {
                return material;
            }
            set
            {
                material = value;

                Vector4 matA = new Vector4(material.AmbientColor.Red, material.AmbientColor.Green, material.AmbientColor.Blue, 0);
                Vector4 matE = new Vector4(material.EmissiveColor.Red, material.EmissiveColor.Green, material.EmissiveColor.Blue, 0);
                Vector4 matD = new Vector4(material.DiffuseColor.Red, material.DiffuseColor.Green, material.DiffuseColor.Blue, 0);
                Vector4 matS = new Vector4(material.SpecularColor.Red, material.SpecularColor.Green, material.SpecularColor.Blue, 0.5f);

                device.SetPixelShaderConstant(4, matA);
                device.SetPixelShaderConstant(5, matE);
                device.SetPixelShaderConstant(6, matD);
                device.SetPixelShaderConstant(7, matS);
            }
        }
    }
}
