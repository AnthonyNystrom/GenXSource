using System;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Shaders
{
    public class ShaderHLSL
    {
        readonly Effect effect;
        EffectHandle technique;

        /// <summary>
        /// Initializes a new instance of the ShaderHLSL class.
        /// </summary>
        public ShaderHLSL(Device gDevice, string file)
        {
            string compileErrors;
            effect = Effect.FromFile(gDevice, file, null, null, ShaderFlags.None, null, out compileErrors);
            if (compileErrors != null && compileErrors.Length > 0)
                throw new Exception("Errors compiling HLSL shader effect - " + compileErrors);
        }

        public Effect Effect
        {
            get { return effect; }
        }
    }
}