using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Rendering.Chem;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Lighting;

namespace NuGenSVisualLib.Rendering.Effects
{
    /// <summary>
    /// Encapsulates a certain range for level-of-detail
    /// </summary>
    public class LevelOfDetailRange
    {
        private ushort min, max;

        public LevelOfDetailRange(ushort min, ushort max)
        {
            this.min = min;
            this.max = max;
        }

        public ushort Min { get { return min; } }
        public ushort Max { get { return max; } }
    }

    public abstract class RenderingEffectSettings
    {
        protected ushort lod;

        public ushort LOD
        {
            get { return lod; }
        }

        public abstract RenderingEffect GetEffect(Device refDevice);
    }

    public struct DataFields
    {
        public VertexFormats Format;
        public string Usage;

        public DataFields(VertexFormats format, string usage)
        {
            this.Format = format;
            this.Usage = usage;
        }
    }

    /// <summary>
    /// Encapsulates a rendering (shader) effect
    /// </summary>
    public abstract class RenderingEffect : IDisposable
    {
        private string name;
        protected Device device;
        protected HashTableSettings settings;
        protected EffectType efxType;
        protected LevelOfDetailRange lodR;
        protected ushort lod;
        protected string description;

        protected OutputRequirements[] oReqs;

        public enum EffectType
        {
            Shading,
            ScreenSpace,
            PostProcess
        }

        #region Properties
     
        public string Name
        {
            get { return name; }
        }
        
        public EffectType EfxType
        {
            get { return efxType; }
        }

        public LevelOfDetailRange LODRange
        {
            get { return lodR; }
        }

        public ushort LOD
        {
            get { return lod; }
            set { lod = value; }
        }

        public string Description
        {
            get { return description;}
        }
        #endregion

        public RenderingEffect(string name, Device device, HashTableSettings settings,
                               ushort minLod, ushort maxLod, ushort lod)
        {
            this.name = name;
            this.device = device;
            this.settings = settings;
            this.lodR = new LevelOfDetailRange(minLod, maxLod);
            this.oReqs = new OutputRequirements[lodR.Max - lodR.Min + 1];
            this.lod = lod;
        }

        public abstract void LoadResources();
        public abstract void UnLoadResources();
        public abstract void CheckDeviceCompatibility(OutputSettings settings);
        public abstract void SetupForDevice(OutputSettings settings);
        public abstract void SetupWithLights(LightingSetup setup);

        public abstract void OnReset();

        public virtual OutputRequirements GetDeviceRequirements(RenderingEffectSettings settings)
        {
            if (settings.LOD <= lodR.Max || settings.LOD >= lodR.Min)
            {
                return oReqs[settings.LOD];
            }
            return null;
        }

        public virtual OutputRequirements[] GetDeviceRequirements()
        {
            return oReqs;
        }

        protected static bool FindAnnotationString(Effect effect, EffectHandle parameterHandle, string name, ref string ret)
        {
            ParameterDescription paramDesc = effect.GetParameterDescription(parameterHandle);
            for (int i = 0; i < paramDesc.Annotations; i++)
            {
                EffectHandle annotationHandle = effect.GetAnnotation(parameterHandle, i);
                if (annotationHandle != null) /* <-- Here, is this possible? */
                {
                    ParameterDescription annotationDesc = effect.GetParameterDescription(annotationHandle);
                    if (annotationDesc.Type == ParameterType.String &&
                                string.Compare(annotationDesc.Name, name, true) == 0)
                    {
                        ret = effect.GetValueString(annotationHandle);
                        return true;
                    }
                }
            }
            return false;
        }
        
        #region IDisposable Members

        public void Dispose()
        {
            UnLoadResources();
        }

        #endregion
    }

    abstract class GeometryRenderingEffect : RenderingEffect
    {
        public GeometryRenderingEffect(string name, Device device, HashTableSettings settings,
                                       ushort minLod, ushort maxLod, ushort lod)
            : base(name, device, settings, minLod, maxLod, lod)
        { }

//        public abstract bool DesiredGeomDataFields(out DataFields[] fields, bool exclusive);
        public abstract void RenderFrame(GraphicsPipeline3D pipeline, MoleculeRenderingScheme scheme, bool alphaPass);
    }

    abstract class ScreenSpaceRenderingEffect : RenderingEffect
    {
        public ScreenSpaceRenderingEffect(string name, Device device, HashTableSettings settings,
                                          ushort minLod, ushort maxLod, ushort lod)
            : base(name, device, settings, minLod, maxLod, lod)
        { }

        public virtual bool DesiredFrameData(out DataFields[] fields) { fields = null; return false; }
        public virtual void UpdateFrameData(BufferedGeometryData[] geomData, GraphicsPipeline3D pipeline) { }
        public abstract void RenderFrame(GraphicsPipeline3D pipeline);
    }

    abstract class PostProcessingRenderingEffect : RenderingEffect
    {
        protected int numPasses;

        public PostProcessingRenderingEffect(string name, Device device, HashTableSettings settings,
                                             ushort minLod, ushort maxLod, ushort lod)
            : base(name, device, settings, minLod, maxLod, lod)
        { }

        public int NumPassesRequired
        {
            get { return numPasses; }
        }

        public abstract void PreFramePass(int num);
        public abstract void PostFramePass(int num);
        public abstract bool RenderScene(int pass);
    }
}