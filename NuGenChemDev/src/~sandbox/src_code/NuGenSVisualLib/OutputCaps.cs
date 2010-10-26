using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Devices
{
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

    public class OutputCaps
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

        public class OutputCapsCompatibility
        {
            internal bool supportDepthFormat;
            internal bool supportDeviceFormat;
            internal bool supportDeviceType;

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
        }

        public OutputCapsCompatibility CheckCompatibility(OutputRequirements req)
        {
            OutputCapsCompatibility comp = new OutputCapsCompatibility();
            comp.supportDeviceType = Manager.CheckDeviceType(adapter, req.DeviceType, req.DeviceFormat, req.DeviceFormat, req.Windowed);
            comp.supportDeviceFormat = true;// Manager.CheckDeviceFormat(adapter, req.DeviceType, req.DeviceFormat, Usage.RenderTarget, ResourceType.Surface, DepthFormat.D16);
            comp.supportDepthFormat = Manager.CheckDepthStencilMatch(adapter, req.DeviceType, req.DeviceFormat, req.DeviceFormat, req.DepthFormat);

            return comp;
        }

        public static OutputCaps GetAdapterCaps(int adapter, DeviceType deviceType, Format format)
        {
            OutputCaps caps = new OutputCaps();
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

        public static OutputCaps GetDefaultAdapterCaps(OutputRequirements req)
        {
            return GetAdapterCaps(Manager.Adapters.Default.Adapter, req.DeviceType, req.DeviceFormat);
        }

        public RequirementsCompatibility CheckCapabilities(OutputRequirements reqs)
        {
            RequirementsCompatibility rComp = new RequirementsCompatibility();

            // check shaders
            Version buildZero = new Version();
            rComp.PS = !(reqs.PixelShader != null && reqs.PixelShader.CompareTo(this.pShaderVersion) > 0);
            rComp.VS = !(reqs.VertexShader != null && reqs.VertexShader.CompareTo(this.vShaderVersion) > 0);

            rComp.Mark();

            return rComp;
        }
    }
}