using System;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering.Devices
{
    /// <summary>
    /// Encapsulates the requirements of a graphics device
    /// </summary>
    public class GraphicsDeviceRequirements
    {
        readonly MultiSampleType multisample = MultiSampleType.None;
        readonly DeviceType devType = DeviceType.NullReference;
//        readonly Format format = Format.Unknown;
        readonly bool windowed;
        readonly DepthFormat[] depthFormats = new DepthFormat[] { DepthFormat.Unknown };
        readonly bool hardwareTnL;

        // shader
        readonly Version ps;
        readonly Version vs;

        // render targets
        readonly int numRTs = 1;
        readonly Format[] rtFormats = null;
        readonly bool pure = false;

        public GraphicsDeviceRequirements() { }

        public GraphicsDeviceRequirements(MultiSampleType sample, DeviceType devType,/* Format format,*/ bool windowed,
                                          DepthFormat depthFormat, bool hardwareTnL, Version vs)
        {
            multisample = sample;
            this.devType = devType;
            //this.format = format;
            this.windowed = windowed;
            depthFormats[0] = depthFormat;
            this.hardwareTnL = hardwareTnL;
            this.vs = vs;
        }

        public GraphicsDeviceRequirements(DeviceType devType, bool hardwareTnL, Version ps, Version vs)
        {
            this.devType = devType;
            this.hardwareTnL = hardwareTnL;
            this.ps = ps;
            this.vs = vs;
        }

        public GraphicsDeviceRequirements(MultiSampleType sample, DeviceType devType,/* Format format,*/
                                          Format[] renderTargetFormats, int numRenderTargets, bool windowed,
                                          DepthFormat[] depthFormats, bool pure, bool tnl)
        {
            multisample = sample;
            this.devType = devType;
            //this.format = format;
            rtFormats = renderTargetFormats;
            this.windowed = windowed;
            numRTs = numRenderTargets;
            this.depthFormats = depthFormats;
            hardwareTnL = tnl;
            this.pure = pure;
        }

        public GraphicsDeviceRequirements(MultiSampleType sample, DeviceType devType,
                                          Format[] renderTargetFormats, int numRenderTargets, bool windowed,
                                          DepthFormat[] depthFormats, bool pure, bool tnl,
                                          Version vs, Version ps)
        {
            multisample = sample;
            this.devType = devType;
            //this.format = format;
            rtFormats = renderTargetFormats;
            this.windowed = windowed;
            numRTs = numRenderTargets;
            this.depthFormats = depthFormats;
            hardwareTnL = tnl;
            this.pure = pure;

            this.vs = vs;
            this.ps = ps;
        }

        #region Properties

        public MultiSampleType MultiSample
        {
            get { return multisample; }
        }

        public DeviceType DeviceType
        {
            get { return devType; }
        }

        public Format DisplayFormat
        {
            get { return rtFormats[0]; }
        }

        public bool Windowed
        {
            get { return windowed; }
        }

        public DepthFormat[] DepthFormats
        {
            get { return depthFormats; }
        }

        public bool HardwareTnL
        {
            get { return hardwareTnL; }
        }

        public Version PixelShader
        {
            get { return ps; }
        }

        public Version VertexShader
        {
            get { return vs; }
        }

        public int NumRenderTargets
        {
            get { return numRTs; }
        }

        public Format[] RenderTargetFormats
        {
            get { return rtFormats; }
        }

        public bool Pure
        {
            get { return pure; }
        }
        #endregion
    }
}