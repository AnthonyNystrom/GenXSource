using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Lighting;
using Microsoft.DirectX;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.Effects
{
    class BloomEffectSettings : RenderingEffectSettings
    {
        public override RenderingEffect GetEffect(Device refDevice)
        {
            return new BloomEffect(refDevice, HashTableSettings.Instance);
        }
    }

    class BloomEffect : PostProcessingRenderingEffect
    {
        protected Effect effect;
        protected EffectHandle brightPass, blurPass, finalPass;

        protected Surface rt0;
        protected Texture sceneTarget;
        protected Surface ds0;
        protected Surface ds1;
        protected Texture brightTarget;
        protected Texture hBlurTarget;
        protected Texture fullBlurTarget;

        CustomVertex.TransformedTextured[] bpQuad;

//        Texture testTexture;

        public BloomEffect(Device device, HashTableSettings settings)
            : base("Bloom", device, settings, 0, 0, 0)
        {
            efxType = EffectType.Shading;
            oReqs[0] = new OutputRequirements(DeviceType.Hardware, true, new Version(2, 0), new Version(1, 1));

            this.numPasses = 4;
        }
        
        public override void LoadResources()
        {
            string base_path = (string)settings["Base.Path"];
            // load effect
            string errors;
            effect = Effect.FromFile(device, base_path + "Media/Effects/Bloom-new.fx", null, null,
                                      ShaderFlags.None, null, out errors);

            if (errors.Length > 0)
                throw new Exception("HLSL compile error");

            brightPass = effect.GetTechnique("std_BloomBrightPass");
            blurPass = effect.GetTechnique("std_BlurPass");
            finalPass = effect.GetTechnique("std_FinalPass");

            bpQuad = new CustomVertex.TransformedTextured[4];
            bpQuad[0].Tu = 0;
            bpQuad[0].Tv = 0;
            bpQuad[1].Tu = 1;
            bpQuad[1].Tv = 0;
            bpQuad[2].Tu = 0;
            bpQuad[2].Tv = 1;
            bpQuad[3].Tu = 1;
            bpQuad[3].Tv = 1;

            //testTexture = TextureLoader.FromFile(device, "c:/blurTest.bmp", 256, 128, 0, Usage.None, Format.X8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
        }

        public override void UnLoadResources()
        {
            if (effect != null)
                effect.Dispose();
        }

        public override void PreFramePass(int num)
        {
            if (num == -1)
            {
                // alpha source pass
                rt0 = device.GetRenderTarget(0);
                ds0 = device.DepthStencilSurface;

                sceneTarget = new Texture(device, rt0.Description.Width, rt0.Description.Height, 1,
                                  Usage.RenderTarget, rt0.Description.Format, Pool.Default);
                ds1 = device.CreateDepthStencilSurface(rt0.Description.Width, rt0.Description.Height,
                                                       (DepthFormat)ds0.Description.Format, MultiSampleType.None,
                                                       0, true);
                device.DepthStencilSurface = ds1;

                device.SetRenderTarget(0, sceneTarget.GetSurfaceLevel(0));
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);
                device.RenderState.ZBufferEnable = true;
            }
            else if (num == 0)
            {
                // bright pass
                brightTarget = new Texture(device, rt0.Description.Width / 2, rt0.Description.Height / 2, 1,
                                  Usage.RenderTarget, rt0.Description.Format, Pool.Default);
                Surface rt2surface = brightTarget.GetSurfaceLevel(0);
                device.SetRenderTarget(0, rt2surface);

                ds1 = device.CreateDepthStencilSurface(ds0.Description.Width / 2, ds0.Description.Height / 2,
                                                       (DepthFormat)ds0.Description.Format, MultiSampleType.None, 0, true);
                device.DepthStencilSurface = ds1;

                // resize FSQ
                bpQuad[0].Position = new Vector4(0, 0, 1, 1);
                bpQuad[1].Position = new Vector4(rt2surface.Description.Width, 0, 1, 1);
                bpQuad[2].Position = new Vector4(0, rt2surface.Description.Height, 1, 1);
                bpQuad[3].Position = new Vector4(rt2surface.Description.Width, rt2surface.Description.Height, 1, 1);

                device.RenderState.ZBufferEnable = false;
            }
            else if (num == 1)
            {
                // render scene to texture -- reuse alpha source surfaces
                ds1 = device.CreateDepthStencilSurface(rt0.Description.Width, rt0.Description.Height,
                                                       (DepthFormat)ds0.Description.Format, MultiSampleType.None,
                                                       0, true);
                device.DepthStencilSurface = ds1;

                device.SetRenderTarget(0, sceneTarget.GetSurfaceLevel(0));
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);
                device.RenderState.ZBufferEnable = true;
            }
            else if (num == 2)
            {
                // blur passes
                hBlurTarget = new Texture(device, rt0.Description.Width / 2, rt0.Description.Height / 2, 1,
                                  Usage.RenderTarget, rt0.Description.Format, Pool.Default);
                device.SetRenderTarget(0, hBlurTarget.GetSurfaceLevel(0));
                fullBlurTarget = new Texture(device, rt0.Description.Width / 2, rt0.Description.Height / 2, 1,
                                  Usage.RenderTarget, rt0.Description.Format, Pool.Default);
                device.RenderState.ZBufferEnable = false;
            }
            else if (num == 3)
            {
                // final pass
                device.SetRenderTarget(0, rt0);
                device.DepthStencilSurface = ds0;

                // resize FSQ
                bpQuad[0].Position = new Vector4(0, 0, 1, 1);
                bpQuad[1].Position = new Vector4(rt0.Description.Width, 0, 1, 1);
                bpQuad[2].Position = new Vector4(0, rt0.Description.Height, 1, 1);
                bpQuad[3].Position = new Vector4(rt0.Description.Width, rt0.Description.Height, 1, 1);
            }
        }

        public override void PostFramePass(int num)
        {
            if (num == -1)
            {
                ds1.Dispose();
            }
            else if (num == 0)
            {
                ds1.Dispose();
            }
            else if (num == 1)
            {
                ds1.Dispose();
            }
            else if (num == 2)
            {
                brightTarget.Dispose();
            }
            else if (num == 3)
            {
                sceneTarget.Dispose();
                hBlurTarget.Dispose();
                fullBlurTarget.Dispose();
            }
        }

        public override bool RenderScene(int pass)
        {
            if (pass == -1 || pass == 1)
                return true;

            if (pass == 0)
            {
                // do bright pass of scene texture using quad
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);
                device.RenderState.ZBufferEnable = false;

                effect.Technique = brightPass;
                effect.SetValue("g_txScene", sceneTarget);
                effect.Begin(FX.None);
                effect.BeginPass(0);

                device.Indices = null;
                device.VertexFormat = CustomVertex.TransformedTextured.Format;
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, bpQuad);

                effect.EndPass();
                effect.End();
            }
            else if (pass == 2)
            {
                // blur passes
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Blue, 1, 0);
                device.RenderState.ZBufferEnable = false;

                effect.Technique = blurPass;
                effect.SetValue("g_txBrightSource", brightTarget);
                effect.SetValue("g_hBlurMap", hBlurTarget);
                effect.Begin(FX.None);

                effect.SetValue("texelSize", new float[] { 1f / (rt0.Description.Width / 4f), 1f / (rt0.Description.Height / 4f) });

                // 2-passes (horz + vert)
                for (int i = 0; i < 2; i++)
                {
                    effect.BeginPass(i);

                    device.BeginScene();
                    device.VertexFormat = CustomVertex.TransformedTextured.Format;
                    device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, bpQuad);
                    device.EndScene();

                    effect.EndPass();

                    if (i == 0)
                    {
                        device.SetRenderTarget(0, fullBlurTarget.GetSurfaceLevel(0));
                        effect.SetValue("g_hBlurMap", hBlurTarget);
                    }
                }

                effect.End();

                //TextureLoader.Save("c:/blurPass.bmp", ImageFileFormat.Bmp, fullBlurTarget);
            }
            else if (pass == 3)
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);
                device.RenderState.ZBufferEnable = false;

                // final pass
                effect.Technique = finalPass;
                effect.SetValue("g_txScene", sceneTarget);
                effect.SetValue("g_hBlurMap", fullBlurTarget);
                effect.Begin(FX.None);
                effect.BeginPass(0);

                device.VertexFormat = CustomVertex.TransformedTextured.Format;
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, bpQuad);

                effect.EndPass();
                effect.End();

                device.RenderState.ZBufferEnable = true;
            }

            return false;
        }

        public override void CheckDeviceCompatibility(OutputSettings settings)
        {
        }

        public override void SetupForDevice(OutputSettings settings)
        {
        }

        public override void SetupWithLights(LightingSetup setup)
        {
        }

        public override void OnReset()
        {
        }
    }
}
