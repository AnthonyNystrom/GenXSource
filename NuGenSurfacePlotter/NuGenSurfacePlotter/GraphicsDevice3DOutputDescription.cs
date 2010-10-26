using Microsoft.DirectX.Direct3D;

namespace NuGenCRBase.Managed.MDX1.Direct3D
{
    class GraphicsDevice3DOutputDescription
    {
        int adapter;
        bool windowed;
        DeviceType type;
        Format format;
        MultiSampleType multiSample;
        DepthFormat depthFormat;
        CreateFlags createFlags;

        public GraphicsDevice3DOutputDescription(int adapter, bool windowed, DeviceType type,
                                                 Format format, MultiSampleType multiSample,
                                                 DepthFormat depthFormat, CreateFlags createFlags)
        {
            this.adapter = adapter;
            this.windowed = windowed;
            this.type = type;
            this.format = format;
            this.multiSample = multiSample;
            this.depthFormat = depthFormat;
            this.createFlags = createFlags;
        }

        public int Adapter
        {
            get { return adapter; }
        }
        public bool Windowed
        {
            get { return windowed; }
        }
        public DeviceType Type
        {
            get { return type; }
        }
        public Format Format
        {
            get { return format; }
        }
        public MultiSampleType MultiSample
        {
            get { return multiSample; }
        }
        public DepthFormat DepthFormat
        {
            get { return depthFormat; }
        }
        public CreateFlags CreateFlags
        {
            get { return createFlags; }
        }
    }
}