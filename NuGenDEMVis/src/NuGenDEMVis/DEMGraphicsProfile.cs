using System;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.VisUI.Rendering;

namespace Genetibase.NuGenDEMVis.Rendering
{
    public class DEMGraphicsProfile : GraphicsProfile
    {
        public static readonly Version ShaderOverlayMinPS = new Version(2, 0);

        public DEMGraphicsProfile(string name, string desc, GraphicsDeviceRequirements minReqs,
                                  GraphicsDeviceRequirements[] recommendedVars)
            : base(name, desc, minReqs, recommendedVars)
        { }

        public DEMGraphicsProfile(string name, string desc, GraphicsDeviceRequirements minReqs,
                                  GraphicsDeviceRequirements recommendedVar)
            : base(name, desc, minReqs, recommendedVar)
        { }

        public bool SupportsShaderOverlay
        {
            get { return RecommendedVariation.PixelShader >= ShaderOverlayMinPS; }
        }
    }
}