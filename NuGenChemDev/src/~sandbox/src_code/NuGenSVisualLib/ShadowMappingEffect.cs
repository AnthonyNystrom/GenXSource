using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Chem;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Lighting;
using Microsoft.DirectX;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.Effects
{
    class ShadowMappingEffectSettings : RenderingEffectSettings
    {
        public override RenderingEffect GetEffect(Device refDevice)
        {
            return new ShadowMappingEffect(refDevice, HashTableSettings.Instance, 0);
        }
    }

    class ShadowMappingEffect : GeometryRenderingEffect
    {
        protected Effect effect;
        protected EffectHandle renderShadow, renderScene, renderLines;
        Texture rt1;

        public ShadowMappingEffect(Device device, HashTableSettings settings, ushort lod)
            : base("Shadow Map", device, settings, 0, 3, lod)
        {
            efxType = EffectType.Shading;
            oReqs[0] = new OutputRequirements(DeviceType.Hardware, true, new Version(1, 1), new Version(1, 1));
            oReqs[1] = new OutputRequirements(DeviceType.Hardware, true, new Version(1, 1), new Version(2, 0));
        }

        public override void LoadResources()
        {
            string base_path = (string)settings["Base.Path"];
            // load effect
            string errors;
            effect = Effect.FromFile(device, base_path + "Media/Effects/ShadowMap.fx", null, null,
                                      ShaderFlags.None, null, out errors);

            if (errors.Length > 0)
                throw new Exception("HLSL compile error");
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
            renderShadow = effect.GetTechnique("ShadowMap");
            renderScene = effect.GetTechnique("std_basicDirLightSpecNoTexture");
            renderLines = effect.GetTechnique("std_basicDirLightSpecNoTextureLine");
        }

        public override void SetupWithLights(LightingSetup setup)
        {
        }

        public override void OnReset()
        {
        }

        public override void RenderFrame(GraphicsPipeline3D pipeline, MoleculeRenderingScheme scheme, bool alphaPass)
        {
            Surface rt0 = device.GetRenderTarget(0);
            Surface ds0 = device.DepthStencilSurface;
            
            // draw shadow to rt1 and ds1
            rt1 = new Texture(device, 1024, 1024, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            Surface ds1 = device.CreateDepthStencilSurface(1024, 1024, DepthFormat.D16, MultiSampleType.None, 0, true);

            device.SetRenderTarget(0, rt1.GetSurfaceLevel(0));
            device.DepthStencilSurface = ds1;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);

            effect.Technique = renderShadow;

            // calc projection needed for scene
            float radius = scheme.SceneRadius + 0.3f;
            Vector3 lightDir = new Vector3(-0.3f, -1, 0);
            float distance = (float)(radius / Math.Tan(Math.PI / 4)) * 2;
            Vector3 lightPos = (-lightDir * distance) + scheme.SceneOrigin;

            Matrix lightProj = Matrix.PerspectiveFovLH((float)(Math.PI / 4),
                                                       (float)rt0.Description.Width / (float)rt0.Description.Height,
                                                       distance - radius, distance + radius);
            Matrix lightView = Matrix.LookAtLH(lightPos, scheme.SceneOrigin, new Vector3(0, 1, 0));
            Matrix lightViewProjectionMatrix = /*Matrix.Invert(pipeline.WorldMatrix * pipeline.ViewMatrix) **/ lightView * lightProj;

            effect.SetValue("xWorldViewProjection", pipeline.WorldMatrix * pipeline.ViewMatrix * pipeline.ProjectionMatrix);
            effect.SetValue("xWorld", Matrix.Identity);
            effect.SetValue("xMaxDepth", distance + radius);
            effect.SetValue("xLightPos", new Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            effect.SetValue("xLightDir", new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            effect.SetValue("xLightWorldViewProjection", lightViewProjectionMatrix);
            effect.SetValue("g_fCosTheta", (float)Math.PI / 4);

            Matrix iv = Matrix.Invert(pipeline.WorldMatrix * pipeline.ViewMatrix);
            effect.SetValue("viewInverse", iv);

            /*effect.SetValue("g_mWorldView", lightView);
            effect.SetValue("g_mProj", lightProj);*/
            
            
            //device.Clear(ClearFlags.ZBuffer, 0x000000ff, 1, 0);
            //device.RenderState.CullMode = Cull.Clockwise;
            device.RenderState.ZBufferEnable = true;

            effect.Begin(FX.None);
            effect.BeginPass(0);

            // render everything from the scheme
            scheme.RenderAtoms(false, true);
            //scheme.RenderBonds(false, true);

            /*CustomVertex.PositionNormal[] floor = new CustomVertex.PositionNormal[4];
            floor[0] = new CustomVertex.PositionNormal(new Vector3(6, -3, 6), new Vector3(0, 1, 0));
            floor[2] = new CustomVertex.PositionNormal(new Vector3(-6, -3, 6), new Vector3(0, 1, 0));
            floor[1] = new CustomVertex.PositionNormal(new Vector3(6, -3, -6), new Vector3(0, 1, 0));
            floor[3] = new CustomVertex.PositionNormal(new Vector3(-6, -3, -6), new Vector3(0, 1, 0));*/

            /*device.VertexFormat = CustomVertex.PositionNormal.Format;
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, floor);*/

            effect.EndPass();
            effect.End();

            device.SetRenderTarget(0, rt0);
            device.DepthStencilSurface = ds0;


            // render scene mapped with shadow
            //device.RenderState.CullMode = Cull.CounterClockwise;
            effect.Technique = renderScene;
            effect.SetValue("xShadowMap", rt1);

            /*effect.SetValue("xWorld", pipeline.WorldMatrix);
            effect.SetValue("xLightWorldViewProjection", pipeline.WorldMatrix * lightViewProjectionMatrix);*/

            /*effect.SetValue("g_mWorldView", pipeline.WorldMatrix * pipeline.ViewMatrix);
            effect.SetValue("g_mProj", pipeline.ProjectionMatrix);
            effect.SetValue("g_mViewToLightProj", lightViewProjectionMatrix);
            effect.SetValue("g_vLightPos", new Vector4(lightPos.X, lightPos.Y, lightPos.Z, 1));
            effect.SetValue("g_vLightDir", new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 1));
            effect.SetValue("g_fCosTheta", (float)Math.PI / 4);*/

            effect.Begin(FX.None);
            effect.BeginPass(0);

            // render everything from the scheme
            scheme.RenderAtoms(false, true);

            if (scheme.LightBonds)
                scheme.RenderBonds(false, false);
            
            // draw floor
            /*device.VertexFormat = CustomVertex.PositionNormal.Format;
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, floor);*/

            effect.EndPass();
            effect.End();

            effect.Technique = renderLines;

            effect.Begin(FX.None);
            effect.BeginPass(0);

            scheme.RenderBonds(false, true);

            effect.EndPass();
            effect.End();

            //TextureLoader.Save("c:/shadow.dds", ImageFileFormat.Dds, rt1);

            rt1.Dispose();
            ds1.Dispose();
        }
    }
}
