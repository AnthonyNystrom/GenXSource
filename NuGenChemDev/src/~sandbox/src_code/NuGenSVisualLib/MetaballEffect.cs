using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Pipelines;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Rendering.Chem;
using Microsoft.DirectX;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Lighting;

namespace NuGenSVisualLib.Rendering.Effects
{
    class MetaballEffect : GeometryRenderingEffect
    {
        Effect effect;
//        VolumeTexture vTexture;
        EffectHandle technique;

        public MetaballEffect(Device device, HashTableSettings settings, ushort lod)
            : base("Metaball", device, settings, 0, 1,lod)
        { }

        public override void LoadResources()
        {
            // load effect files
            string errors;
            effect = Effect.FromFile(device, "D:/~Work/NuGenChemDev/cvs/Media/Effects/metaballOutlines.fx", null, null,
                                     ShaderFlags.NotCloneable, null, out errors);

            if (errors.Length > 0)
                throw new Exception("HLSL compile error");

            // load volume texture
            //vTexture = TextureLoader.FromVolumeFile(device, "D:/~Work/NuGenChemDev/cvs/Media/VolumeTextures/Volume1.dds");
        }

        public override void UnLoadResources()
        {
            if (effect != null)
                effect.Dispose();
            //if (vTexture != null)
            //    vTexture.Dispose();
        }

        public override void CheckDeviceCompatibility(OutputSettings settings)
        {
        }

        public override void SetupForDevice(OutputSettings settings)
        {
            technique = effect.GetTechnique("outlinePerPixel");
        }

    //    public override void RenderFrame(GraphicsPipeline3D pipeline, BufferedGeometryData[] gBuffers)
    //    {
    //        Matrix wvp = pipeline.WorldMatrix * pipeline.ViewMatrix * pipeline.ProjectionMatrix;
    //        Matrix iv = Matrix.Invert(pipeline.ViewMatrix);
    //        effect.SetValue("worldViewProj", wvp);
    //        effect.SetValue("viewInverse", iv);
    //        effect.SetValue("worldInverseTranspose", Matrix.TransposeMatrix(iv));

    //        effect.SetValue("diffuseTexture2", vTexture);
    //        effect.SetValue("lightDir", new float[] { 1, 1, -1, 1 });
    //        effect.SetValue("lightColor", new float[] { 1, 1, 1, 1 });
    //        effect.SetValue("materialDiffuse", new float[] { 1, 1, 1, 1 });

    //        effect.SetValue("noiseScale", 0.1f);

    //        effect.Technique = technique;

    //        effect.Begin(FX.None);
    //        effect.BeginPass(0);

    //        foreach (BufferedGeometryData buffer in gBuffers)
    //        {
    //            buffer.DrawBuffers(pipeline);
    //        }

    //        effect.EndPass();
    //        effect.End();
    //    }

        public override void RenderFrame(GraphicsPipeline3D pipeline, MoleculeRenderingScheme scheme, bool alphaPass)
        {
            Matrix wvp = pipeline.WorldMatrix * pipeline.ViewMatrix * pipeline.ProjectionMatrix;
            Matrix iv = Matrix.Invert(pipeline.ViewMatrix);
            effect.SetValue("worldViewProj", wvp);
            effect.SetValue("viewInverse", iv);
            effect.SetValue("worldInverseTranspose", Matrix.TransposeMatrix(Matrix.Invert(pipeline.WorldMatrix)));

            // do light passes
            effect.Technique = technique;
            effect.Begin(FX.None);
            effect.BeginPass(0);

            // render everything from the scheme
            scheme.RenderAtoms(false, false);
            scheme.RenderBonds(false, false);

            effect.EndPass();
            effect.End();
        }

        public override void SetupWithLights(LightingSetup setup)
        {
        }

        public override void OnReset()
        {
        }
    }
}
