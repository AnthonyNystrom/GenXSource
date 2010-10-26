using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Effects
{
    /*class GlowEffect : RenderingEffect
    {
        public GlowEffect(Device device)
            : base(device)
        { }

        /// <summary>
        /// Renders the scene to a small alpha surface
        /// </summary>
        class GlowPass1 : RenderingPass
        {
            GlowEffect effect;
            Texture target;
            Texture glowTarget;

            public GlowPass1(GlowEffect effect)
                : base(true, true)
            {
                this.effect = effect;
            }

            public override void Begin()
            {
                // setup small render target
                Device device = effect.device;
                Surface bBuffer = device.GetBackBuffer(0, 0, BackBufferType.Mono);
                int width = (int)((float)bBuffer.Description.Width / 2.0f);
                int height = (int)((float)bBuffer.Description.Height / 2.0f);
                Format format = bBuffer.Description.Format;

                target = new Texture(device, width, height, 1, Usage.RenderTarget, format, Pool.Managed);
                //target = device.CreateRenderTarget(width, height, format, MultiSampleType.None, 0, false);

                device.SetRenderTarget(0, target.GetSurfaceLevel(0));

                // setup output
                device.TextureState[0].ColorOperation = TextureOperation.SelectArg1;
                device.TextureState[0].ColorArgument1 = TextureArgument.Diffuse;
                device.TextureState[0].AlphaOperation = TextureOperation.SelectArg1;
                device.TextureState[0].AlphaArgument1 = TextureArgument.Diffuse;

                device.TextureState[1].ColorOperation = TextureOperation.Disable;
                device.TextureState[1].AlphaOperation = TextureOperation.Disable;

                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.SourceBlend = Blend.One;
                device.RenderState.DestinationBlend = Blend.Zero;

                device.SetTexture(0, null);

                device.SamplerState[0].MinFilter = TextureFilter.Linear;
                device.SamplerState[0].MagFilter = TextureFilter.Linear;
                device.SamplerState[0].MipFilter = TextureFilter.Point;

                device.RenderState.ZBufferEnable = true;
                device.RenderState.ZBufferFunction = Compare.LessEqual;
            }

            public override void End()
            {
                Device device = effect.device;

                // Multiply texture alpha * RGB to get the glow sources
                //device.SetRenderTarget(0, glowTarget.GetSurfaceLevel(0));
                //device.DepthStencilSurface = null;

                //// Select quad geometry with half-texel size offset to sample from texel centers
                //TextureDisplay
                ////m_pTextureDisplay->SetStateForRendering(m_TID_HalfSizeToBlurHoriz);
                //// D3DTOP_BLENDTEXTUREALPHA reads the alpha at it's stage and does s = arg1 * alpha + arg2 * alpha
                //device.TextureState[0].ColorOperation = TextureOperation.BlendTextureAlpha;
                //device.TextureState[0].ColorArgument2 = TextureArgument.TFactor;
                //device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
                //device.TextureState[0].AlphaOperation = TextureOperation.SelectArg1;    // texture alpha
                //device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
                //device.TextureState[1].ColorOperation = TextureOperation.Disable;
                //device.TextureState[1].AlphaOperation = TextureOperation.Disable;
                //device.RenderState.TextureFactor = 0x00000000;          // ARGB black
                
                //device.SetTexture(0, target);
                
                //m_pTextureDisplay->Render(m_TID_HalfSizeToBlurHoriz, false, false);
            }
        }

        //class GlowPass2 : RenderingPass
        //{
        //    public override void Begin()
        //    {
        //        throw new Exception("The method or operation is not implemented.");
        //    }

        //    public override void End()
        //    {
        //        throw new Exception("The method or operation is not implemented.");
        //    }
        //}
    }*/
}
