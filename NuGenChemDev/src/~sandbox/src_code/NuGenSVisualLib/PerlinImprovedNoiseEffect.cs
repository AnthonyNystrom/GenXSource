using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Chem;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Lighting;
using Microsoft.DirectX;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.Effects
{
    class PerlinImprovedNoiseEffect : GeometryRenderingEffect
    {
        protected Effect effect;
        protected EffectHandle technique;

        Texture permTexture, permTexture2d, gradTexture, permGradTexture;

        public PerlinImprovedNoiseEffect(Device device, HashTableSettings settings)
            : base("Perlin Noise", device, settings, 0, 0, 0)
        {
            efxType = EffectType.Shading;
            oReqs[0] = new OutputRequirements(DeviceType.Hardware, true, new Version(1, 1), new Version(2, 1));
        }

        public override void LoadResources()
        {
            string base_path = (string)settings["Base.Path"];
            // load effect
            string errors;
            effect = Effect.FromFile(device, base_path + "Media/Effects/noise.fx", null, null, ShaderFlags.NotCloneable, null, out errors);
            
            if (errors.Length > 0)
                throw new Exception("HLSL compile error");

            string temp = null;
            FindAnnotationString(effect, "permTexture", "function", ref temp);
            EffectHandle function = effect.GetFunction(temp);
            FindAnnotationString(effect, "permTexture", "width", ref temp);
            int width = Int32.Parse(temp);
            FindAnnotationString(effect, "permTexture", "height", ref temp);
            int height = Int32.Parse(temp);
            
            // generate textures
            Surface surface = device.GetRenderTarget(0);

            GenerateTexture(ref permTexture, width, height, function);

            device.SetRenderTarget(0, surface);
        }

        private void GenerateTexture(ref Texture texture, int width, int height, EffectHandle function)
        {
            texture = new Texture(device, width, height, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);

            device.SetRenderTarget(0, texture.GetSurfaceLevel(0));
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);

            device.BeginScene();

            device.EndScene();

            device.Present();
        }

        public override void UnLoadResources()
        {
            if (effect != null)
                effect.Dispose();
        }

        public override void CheckDeviceCompatibility(OutputSettings settings)
        {
        }

        public override void SetupForDevice(OutputSettings settings)
        {
            // pick techniques required
            string noiseType = "Turbulence";
            string techStr = string.Format("noise{0}", noiseType);
            technique = effect.GetTechnique(techStr);
        }

        public override void SetupWithLights(LightingSetup setup)
        {
        }

        public override void OnReset()
        {
        }

        public override void RenderFrame(GraphicsPipeline3D pipeline, MoleculeRenderingScheme scheme, bool alphaPass)
        {
            Matrix wvp = pipeline.WorldMatrix * pipeline.ViewMatrix * pipeline.ProjectionMatrix;
            effect.SetValue("worldViewProj", wvp);

            // do passes
            effect.Technique = technique;
            effect.Begin(FX.None);
            effect.BeginPass(0);

            // render everything from the scheme
            scheme.RenderAtoms(false, false);
            scheme.RenderBonds(false, false);

            effect.EndPass();
            effect.End();
        }
    }
}
