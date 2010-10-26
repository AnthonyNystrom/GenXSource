using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering.Devices
{
    class OutputSettingsException : ApplicationException
    {
        public PropertyInfo setting;
        public object reqValue;
        public object capValue;

        public OutputSettingsException(PropertyInfo setting, object reqValue, object capValue)
            : base("Failed to create output setting for '" + setting + "'")
        {
            this.setting = setting;
            this.reqValue = reqValue;
            this.capValue = capValue;
        }
    }

    /// <summary>
    /// Encapsulates the settings for the ouput of a graphics device
    /// </summary>
    public class GraphicsDeviceSettings
    {
        private int adapter;
        private readonly bool windowed;
        private DeviceType devType;
        private Format devFormat;
        private MultiSampleType multisample;
        private DepthFormat depthFormat;
        CreateFlags createFlags;

        public GraphicsDeviceSettings() { }

        public GraphicsDeviceSettings(int adapter, bool windowed, DeviceType devType, Format devFormat,
                                      MultiSampleType multisample, DepthFormat depthFormat, CreateFlags createFlags)
        {
            this.adapter = adapter;
            this.windowed = windowed;
            this.devType = devType;
            this.devFormat = devFormat;
            this.multisample = multisample;
            this.depthFormat = depthFormat;
            this.createFlags = createFlags;
        }

        #region Properties

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
            get { return devType; }
        }

        public MultiSampleType MultiSample
        {
            get { return multisample; }
        }

        public Format DisplayFormat
        {
            get { return devFormat; }
        }

        public DepthFormat DepthFormat
        {
            get { return depthFormat; }
        }

        public CreateFlags CreateFlags
        {
            get { return createFlags; }
        }
        #endregion

        public void CreateDevice(Control renderOutput, out Device device, out PresentParameters pParams)
        {
            pParams = new PresentParameters();
            pParams.Windowed = windowed;
            pParams.MultiSample = multisample;
            pParams.EnableAutoDepthStencil = true;
            pParams.AutoDepthStencilFormat = depthFormat;
            pParams.SwapEffect = SwapEffect.Discard;

            device = new Device(adapter, devType, renderOutput, createFlags, pParams);
        }

        public static GraphicsDeviceSettings CreateFromRequirements(GraphicsDeviceRequirements requirements,
                                                                    GraphicsDeviceCaps caps,
                                                                    GraphicsDeviceRequirements fallbacks,
                                                                    out bool fullyMatchReq)
        {
            if (caps == null)
                caps = GraphicsDeviceCaps.GetDefaultAdapterCaps(requirements);

            GraphicsDeviceSettings settings = new GraphicsDeviceSettings();
            settings.adapter = caps.Adapter;

            Type type = typeof(GraphicsDeviceSettings);

            GraphicsDeviceCaps.OutputCapsCompatibility reqComp = caps.CheckCompatibility(requirements);
            GraphicsDeviceCaps.OutputCapsCompatibility fallbackComp = caps.CheckCompatibility(fallbacks);

            fullyMatchReq = reqComp.FullMatch;

            // check reqs against caps
            if (reqComp.SupportDeviceType)
                settings.devType = requirements.DeviceType;
            else if (fallbackComp.SupportDeviceType)
                settings.devType = fallbacks.DeviceType;
            else
                throw new OutputSettingsException(type.GetProperty("DeviceType"),
                                                  requirements.DeviceType, null);

            if (reqComp.SupportDeviceFormat)
                settings.devFormat = requirements.DisplayFormat;
            else if (fallbackComp.SupportDeviceFormat)
                settings.devFormat = fallbacks.DisplayFormat;
            else
                throw new OutputSettingsException(type.GetProperty("DeviceFormat"),
                                                  requirements.DisplayFormat, null);

            if (reqComp.SupportDepthFormat)
                settings.depthFormat = requirements.DepthFormats[0];
            else if (fallbackComp.SupportDepthFormat)
                settings.depthFormat = fallbacks.DepthFormats[0];
            else
                throw new OutputSettingsException(type.GetProperty("DepthFormat"),
                                                  requirements.DepthFormats[0], null);

            if (caps.HardwareTnL && requirements.HardwareTnL)
                settings.createFlags = CreateFlags.HardwareVertexProcessing;
            else if (!requirements.HardwareTnL)
                settings.createFlags = CreateFlags.SoftwareVertexProcessing;
            else if (!fallbacks.HardwareTnL)
                settings.createFlags = CreateFlags.SoftwareVertexProcessing;
            else
                throw new OutputSettingsException(type.GetProperty("CreateFlags"),
                                                  requirements.HardwareTnL, null);

            if (requirements.MultiSample <= caps.AntiAliasing.MaxSupported)
                settings.multisample = requirements.MultiSample;
            else if (fallbacks.MultiSample <= caps.AntiAliasing.MaxSupported)
                settings.multisample = fallbacks.MultiSample;
            else
                throw new OutputSettingsException(type.GetProperty("AntiAliasing"),
                                                  requirements.MultiSample, caps.AntiAliasing.MaxSupported);

            return settings;
        }
    }
}