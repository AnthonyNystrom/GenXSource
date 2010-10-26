using Microsoft.DirectX.Direct3D;
using System;

namespace NuGenCRBase.Managed.MDX1.Direct3D
{
    class GraphicsDevice3DRequirements
    {
        MultiSampleType multiSample;
        DeviceType deviceType;
        Format displayFormat;
        Format[] rtFormats;
        int numRTs;
        bool windowed;
        DepthFormat[] depthFormats;
        bool pure;
        bool hardwareTnL;
        Version ps;
        Version vs;

        public GraphicsDevice3DRequirements(MultiSampleType multiSample, DeviceType deviceType, Format[] rtFormats,
                                            int numRTs, bool windowed, DepthFormat[] depthFormats, bool pure,
                                            bool hardwareTnL, Version vs, Version ps, Format displayFormat)
        {
            this.multiSample = multiSample;
            this.deviceType = deviceType;
            this.rtFormats = rtFormats;
            this.numRTs = numRTs;
            this.windowed = windowed;
            this.depthFormats = depthFormats;
            this.pure = pure;
            this.hardwareTnL = hardwareTnL;
            this.vs = vs;
            this.ps = ps;
            this.displayFormat = displayFormat;
        }

        public MultiSampleType MultiSample
        {
            get { return multiSample; }
        }
        public DeviceType DeviceType
        {
            get { return deviceType; }
        }
        public int NumRTs
        {
            get { return numRTs; }
        }
        public bool Windowed
        {
            get { return windowed; }
        }
        public Version PixelShader
        {
            get { return ps; }
        }
        public Version VertexShader
        {
            get { return vs; }
        }
        public Format[] RtFormats
        {
            get { return rtFormats; }
        }
        public Format DisplayFormat
        {
            get { return displayFormat; }
        }
        public DepthFormat[] DepthFormats
        {
            get { return depthFormats; }
        }
        public bool Pure
        {
            get { return pure; }
        }
        public bool HardwareTnL
        {
            get { return hardwareTnL; }
        }
    }
}