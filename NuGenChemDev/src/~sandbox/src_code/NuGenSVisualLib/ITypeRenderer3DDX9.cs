using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Settings;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Pipelines;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering.Chem
{
    interface ITypeRenderer3DDx9 : IDisposable
    {
        void Init(Device device, ISettings settings, CompleteOutputDescription coDesc);

        void BeginDraw(GraphicsPipeline3D pipeline, GeneralShadingDesc gShading,
                       AtomShadingDesc aShading);

        void EndDraw();

        void EndDrawToBuffer();
        void DrawBuffer();
    }
}