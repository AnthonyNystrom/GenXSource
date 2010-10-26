using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;


namespace NuGenSVisualLib.Rendering.Devices
{
    /// <summary>
    /// Encapsulates the specified requirements for output
    /// </summary>
    public class OutputRequirements
    {
        MultiSampleType antiAliasing = MultiSampleType.None;
        DeviceType devType = DeviceType.NullReference;
        Format format = Format.Unknown;
        bool windowed;
        DepthFormat depthFormat = DepthFormat.Unknown;
        bool hardwareTnL;

        Version ps;
        Version vs;

        int numRTs;
        Format[] rtFormats;

        public OutputRequirements() { }

        public OutputRequirements(MultiSampleType aa, DeviceType dType, Format format, bool windowed,
                                    DepthFormat depthFormat, bool hardwareTnL, Version vs)
        {
            this.antiAliasing = aa;
            this.devType = dType;
            this.format = format;
            this.windowed = windowed;
            this.depthFormat = depthFormat;
            this.hardwareTnL = hardwareTnL;
            this.vs = vs;
        }

        public OutputRequirements(DeviceType devType, bool hardwareTnL, Version ps, Version vs)
        {
            this.devType = devType;
            this.hardwareTnL = hardwareTnL;
            this.ps = ps;
            this.vs = vs;
        }

        public MultiSampleType AntiAliasing
        {
            get { return antiAliasing; }
        }

        public DeviceType DeviceType
        {
            get { return devType; }
        }

        public Format DeviceFormat
        {
            get { return format; }
        }

        public bool Windowed
        {
            get { return windowed; }
        }

        public DepthFormat DepthFormat
        {
            get { return depthFormat; }
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
    }
}