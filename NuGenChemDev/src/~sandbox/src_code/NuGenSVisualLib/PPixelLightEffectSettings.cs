using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Lighting;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Chem;
using NuGenSVisualLib.Settings;
using Microsoft.DirectX;
using System.Drawing;
using NuGenNoiseLib.LibNoise;
using System.IO;

namespace NuGenSVisualLib.Rendering.Effects
{
    class PPixelLightEffectSettings : RenderingEffectSettings
    {
        // most are in lighting setup?

        public override RenderingEffect GetEffect(Device refDevice)
        {
            return new PPixelLightEffect(refDevice, HashTableSettings.Instance, 0);
        }
    }

    /// <summary>
    /// Encapsulates lighting effects for per-pixel (and per-vertex) shading
    /// </summary>
    class PPixelLightEffect : GeometryRenderingEffect
    {
        protected Effect effect1, effect2;
        protected EffectHandle technique;
        protected EffectHandle lineTechnique;
        protected EffectHandle alphaTechnique;

        protected LightingSetup setup;

        Texture testTexture;

        public PPixelLightEffect(Device device, HashTableSettings settings, ushort lod)
            : base("Adv. Lighting", device, settings, 0, 1, lod)
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
            effect1 = Effect.FromFile(device, base_path + "Media/Effects/LightEffects.fx", null, null,
                                      ShaderFlags.None, null, out errors);

            if (errors.Length > 0)
                throw new Exception("HLSL compile error");

            effect2 = Effect.FromFile(device, base_path + "Media/Effects/PPLightEffects.fx", null, null,
                                      ShaderFlags.None, null, out errors);

            if (errors.Length > 0)
                throw new Exception("HLSL compile error");

            Stream stream = null;
            Stream hmStream = null;
            NoiseTextureParams param = new NoiseTextureParams(512, 256, false, new PerlinModuleWrapper(), false);
            NoiseTextureBuilder.BuildSphericalTexture(param, ref stream, ref hmStream);
            testTexture = TextureLoader.FromStream(device, stream);
            stream.Dispose();
            //testTexture = TextureLoader.FromFile(device, "c:/earth.bmp");
        }

        public override void UnLoadResources()
        {
            if (effect1 != null)
                effect1.Dispose();
            if (effect2 != null)
                effect2.Dispose();

            //if (texture != null)
            //    texture.Dispose();
        }

        public override void CheckDeviceCompatibility(OutputSettings settings)
        {
        }

        public override void SetupForDevice(OutputSettings settings)
        {
            // pick techniques required
            bool texture = true;
            bool spec = true;
            string lightType = "Dir";
            string techStr = string.Format("std_basic{0}Light{1}Spec{2}Texture", lightType, spec ? "" : "No", texture ? "" : "No");
            if (lod == 0)
            {
                technique = effect1.GetTechnique(techStr);
                lineTechnique = effect1.GetTechnique(techStr + "Line");
                alphaTechnique = effect1.GetTechnique("std_basicAlphaTexture");
            }
            else if (lod == 1)
            {
                technique = effect2.GetTechnique(techStr);
                lineTechnique = effect2.GetTechnique(techStr + "Line");
                alphaTechnique = effect2.GetTechnique("std_basicAlphaTexture");
            }
        }

        public override void SetupWithLights(LightingSetup setup)
        {
            if (setup == null)
            {
                setup = new LightingSetup();
                DirectionalLight light = new DirectionalLight();
                light.Clr = Color.White;
                light.Direction = new Vector3(1, -1, -1);
                light.Enabled = true;
                light.Name = "light0";
                setup.lights.Add(light);
            }
            this.setup = setup;
        }

        public override void OnReset()
        {
        }

        public override void RenderFrame(GraphicsPipeline3D pipeline, MoleculeRenderingScheme scheme, bool alphaPass)
        {
            Effect cEffect;
            if (lod == 0)
                cEffect = effect1;
            else if (lod == 1)
                cEffect = effect2;
            else
                return;

            Matrix wvp = pipeline.WorldMatrix * pipeline.ViewMatrix * pipeline.ProjectionMatrix;
            Matrix iv = Matrix.Invert(pipeline.ViewMatrix);
            cEffect.SetValue("worldViewProj", wvp);
            cEffect.SetValue("viewInverse", iv);
            cEffect.SetValue("worldInverseTranspose", Matrix.TransposeMatrix(iv));
            cEffect.SetValue("shininess", 3);
            cEffect.SetValue("diffuseTexture", testTexture);

            if (alphaPass)
            {
                cEffect.Technique = alphaTechnique;
                cEffect.Begin(FX.None);
                cEffect.BeginPass(0);

                scheme.RenderAtoms(false, false);

                cEffect.EndPass();
                cEffect.End();
            }
            else
            {
                // do light passes
                cEffect.Technique = technique;
                cEffect.Begin(FX.None);
                cEffect.BeginPass(0);
                //effect.SetValue("diffuseTexture", texture);
                foreach (NuGenSVisualLib.Rendering.Lighting.Light light in setup.lights)
                {
                    if (!light.Enabled)
                        continue;
                    if (light is DirectionalLight)
                    {
                        DirectionalLight dLight = (DirectionalLight)light;
                        cEffect.SetValue("lightDir", new Vector4(dLight.Direction.X, dLight.Direction.Y, dLight.Direction.Z, 1.0f));
                        cEffect.SetValue("lightColor", ColorValue.FromColor(dLight.Clr));

                        // render everything from the scheme
                        scheme.RenderAtoms(false, false);
                        if (scheme.LightBonds)
                            scheme.RenderBonds(false, false);
                    }
                }
                cEffect.EndPass();
                cEffect.End();
            }
            //if (!scheme.LightBonds)
            //{
            //    effect.Technique = lineTechnique;
            //    effect.Begin(FX.None);
            //    effect.BeginPass(0);

            //    scheme.RenderBonds();

            //    effect.EndPass();
            //    effect.End();
            //}

            //device.SetTexture(0, null);
        }

//        public override bool DesiredGeomDataFields(out DataFields[] fields, bool exclusive)
//        {
//            fields = new DataFields[] { new DataFields(VertexFormats.Normal, "NORMAL"), /*new DataFields(VertexFormats.Texture1, "TEXTURE0"),*/ new DataFields(VertexFormats.Diffuse, "DIFFUSE") };
//            return true;
//        }
    }
}