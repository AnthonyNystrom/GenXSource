using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Lighting;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Chem;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Rendering.Effects
{
    class MonochromeEffectSettings : RenderingEffectSettings
    {
        public override RenderingEffect GetEffect(Device refDevice)
        {
            return new MonochromeEffect(refDevice, HashTableSettings.Instance);
        }
    }

    /// <summary>
    /// Provides rendering code for a monochrome/greyscale rendering effect
    /// </summary>
    class MonochromeEffect : PPixelLightEffect
    {
        public MonochromeEffect(Device device, HashTableSettings settings)
            : base(device, settings, 0)
        { }

        public override void LoadResources()
        {
            string base_path = (string)settings["Base.Path"];
            // load effect
            string errors;
            effect1 = Effect.FromFile(device, base_path + "Media/Effects/Monochrome.fx", null, null, ShaderFlags.NotCloneable, null, out errors);

            if (errors.Length > 0)
                throw new Exception("HLSL compile error");
        }
    }
}