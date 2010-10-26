using System;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering.Devices
{
    /// <summary>
    /// Encapsulates a graphics device's compatibility with output requirements
    /// </summary>
    public class RequirementsCompatibility
    {
        public int Adapter;
        public bool PS;
        public bool VS;

        public int NumFails;

        public void Mark()
        {
            if (!PS)
                NumFails++;
            if (!VS)
                NumFails++;
        }
    }

    /// <summary>
    /// Encapsulates the output capabilities of a graphics device
    /// </summary>
    public class GraphicsDeviceCaps
    {
        private int adapter;
        private AntiAliasCaps antialiasCaps;
        private AdapterDetails adapterDetails;
        private Caps caps;
        private bool hardwareTnL;
        private VertexShaderCaps vShaderCaps;
        private PixelShaderCaps pShaderCaps;
        private Version vShaderVersion;
        private Version pShaderVersion;

        #region Properties

        public AntiAliasCaps AntiAliasing
        {
            get { return antialiasCaps; }
        }

        public int Adapter
        {
            get { return adapter; }
        }

        public AdapterDetails AdapterDetails
        {
            get { return adapterDetails; }
        }

        public Caps Caps
        {
            get { return caps; }
        }

        public bool HardwareTnL
        {
            get { return hardwareTnL; }
        }

        public VertexShaderCaps VertexShaderCaps
        {
            get { return vShaderCaps; }
        }

        public Version VertexShaderVersion
        {
            get { return vShaderVersion; }
        }

        public PixelShaderCaps FragmentShaderCaps
        {
            get { return pShaderCaps; }
        }

        public Version FragmentShaderVersion
        {
            get { return pShaderVersion; }
        }
        #endregion

        public class OutputCapsCompatibility
        {
            internal bool supportDepthFormat;
            internal bool supportDeviceFormat;
            internal bool supportDeviceType;
            internal bool fullMatch;

            public bool SupportDeviceType
            {
                get { return supportDeviceType; }
            }

            public bool SupportDeviceFormat
            {
                get { return supportDeviceFormat; }
            }

            public bool SupportDepthFormat
            {
                get { return supportDepthFormat; }
            }

            public bool FullMatch
            {
                get { return fullMatch; }
            }
        }

        public OutputCapsCompatibility CheckCompatibility(GraphicsDeviceRequirements req)
        {
            // TODO: Check we only have stuff we need in the setup process and that it works
            OutputCapsCompatibility comp = new OutputCapsCompatibility();
            comp.supportDeviceType = Manager.CheckDeviceType(adapter, req.DeviceType, req.DisplayFormat, req.DisplayFormat, req.Windowed);
            comp.supportDeviceFormat = true;// Manager.CheckDeviceFormat(adapter, req.DeviceType, req.DeviceFormat, Usage.RenderTarget, ResourceType.Surface, DepthFormat.D16);
            comp.supportDepthFormat = Manager.CheckDepthStencilMatch(adapter, req.DeviceType, req.DisplayFormat, req.DisplayFormat, req.DepthFormats[0]);

            comp.fullMatch = comp.supportDepthFormat && comp.supportDeviceFormat &&
                             comp.supportDeviceType && HardwareTnL == req.HardwareTnL &&
                             req.MultiSample <= antialiasCaps.MaxSupported;

            return comp;
        }

        public static GraphicsDeviceCaps GetAdapterCaps(int adapter, DeviceType deviceType, Format format)
        {
            GraphicsDeviceCaps caps = new GraphicsDeviceCaps();
            caps.adapter = adapter;
            caps.adapterDetails = Manager.Adapters.Default.Information;

            caps.caps = Manager.GetDeviceCaps(adapter, deviceType);
            caps.antialiasCaps = new AntiAliasCaps(adapter, deviceType, format);
            caps.hardwareTnL = caps.caps.DeviceCaps.SupportsHardwareTransformAndLight;

            caps.vShaderCaps = caps.caps.VertexShaderCaps;
            caps.vShaderVersion = caps.caps.VertexShaderVersion;
            caps.pShaderCaps = caps.caps.PixelShaderCaps;
            caps.pShaderVersion = caps.caps.PixelShaderVersion;

            return caps;
        }

        public static GraphicsDeviceCaps GetDefaultAdapterCaps(GraphicsDeviceRequirements req)
        {
            return GetAdapterCaps(Manager.Adapters.Default.Adapter, req.DeviceType, req.DisplayFormat);
        }

        public RequirementsCompatibility CheckCapabilities(GraphicsDeviceRequirements reqs)
        {
            RequirementsCompatibility rComp = new RequirementsCompatibility();

            // check shaders
            //Version buildZero = new Version();
            rComp.PS = !(reqs.PixelShader != null && reqs.PixelShader.CompareTo(pShaderVersion) > 0);
            rComp.VS = !(reqs.VertexShader != null && reqs.VertexShader.CompareTo(vShaderVersion) > 0);

            rComp.Mark();

            return rComp;
        }
    }
}