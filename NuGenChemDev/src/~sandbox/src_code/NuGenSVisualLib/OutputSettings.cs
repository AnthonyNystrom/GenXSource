using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace NuGenSVisualLib.Rendering.Devices
{
    class OutputSettingsException : ApplicationException
    {
        public PropertyInfo setting;
        public object reqValue;
        public object capValue;

        public OutputSettingsException(PropertyInfo setting, object reqValue, object capValue)
            : base("Failed to create output setting for '" + setting.ToString() + "'")
        {
            this.setting = setting;
            this.reqValue = reqValue;
            this.capValue = capValue;
        }
    }

    /// <summary>
    /// Encapsulates the settings for output
    /// </summary>
    public class OutputSettings
    {
        private int adapter;
        private bool windowed;
        private DeviceType devType;
        private Format devFormat;
        private MultiSampleType antiAliasing;
        private DepthFormat depthFormat;
        CreateFlags createFlags;

        public OutputSettings() { }

        public OutputSettings(int adapter, bool windowed, DeviceType devType, Format devFormat,
                              MultiSampleType antiAliasing, DepthFormat depthFormat, CreateFlags createFlags)
        {
            this.adapter = adapter;
            this.windowed = windowed;
            this.devType = devType;
            this.devFormat = devFormat;
            this.antiAliasing = antiAliasing;
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

        public DeviceType DeviceType
        {
            get { return devType; }
        }

        public MultiSampleType AntiAliasing
        {
            get { return antiAliasing; }
        }

        public Format DeviceFormat
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

        public void CreateDevice(Control renderOutput, out Device device, out PresentParameters pParams)
        {
            pParams = new PresentParameters();
            pParams.Windowed = windowed;
            pParams.MultiSample = antiAliasing;
            pParams.EnableAutoDepthStencil = true;
            pParams.AutoDepthStencilFormat = depthFormat;
            pParams.SwapEffect = SwapEffect.Discard;

            device = new Device(adapter, devType, renderOutput, createFlags, pParams);
        }

        public static OutputSettings CreateFromRequirements(OutputRequirements requirements, OutputCaps caps,
                                                            OutputRequirements fallbacks)
        {
            if (caps == null)
                caps = OutputCaps.GetDefaultAdapterCaps(requirements);

            OutputSettings settings = new OutputSettings();
            settings.adapter = caps.Adapter;

            Type type = typeof(OutputSettings);

            OutputCaps.OutputCapsCompatibility reqComp = caps.CheckCompatibility(requirements);
            OutputCaps.OutputCapsCompatibility fallbackComp = caps.CheckCompatibility(fallbacks);

            // check reqs against caps
            if (reqComp.SupportDeviceType)
                settings.devType = requirements.DeviceType;
            else if (fallbackComp.SupportDeviceType)
                settings.devType = fallbacks.DeviceType;
            else
                throw new OutputSettingsException(type.GetProperty("DeviceType"),
                                                  requirements.DeviceType, null);

            if (reqComp.SupportDeviceFormat)
                settings.devFormat = requirements.DeviceFormat;
            else if (fallbackComp.SupportDeviceFormat)
                settings.devFormat = fallbacks.DeviceFormat;
            else
                throw new OutputSettingsException(type.GetProperty("DeviceFormat"),
                                                  requirements.DeviceFormat, null);

            if (reqComp.SupportDepthFormat)
                settings.depthFormat = requirements.DepthFormat;
            else if (fallbackComp.SupportDepthFormat)
                settings.depthFormat = fallbacks.DepthFormat;
            else
                throw new OutputSettingsException(type.GetProperty("DepthFormat"),
                                                  requirements.DepthFormat, null);

            if (caps.HardwareTnL && requirements.HardwareTnL)
                settings.createFlags = CreateFlags.HardwareVertexProcessing;
            else if (!requirements.HardwareTnL)
                settings.createFlags = CreateFlags.SoftwareVertexProcessing;
            else if (!fallbacks.HardwareTnL)
                settings.createFlags = CreateFlags.SoftwareVertexProcessing;
            else
                throw new OutputSettingsException(type.GetProperty("CreateFlags"),
                                                  requirements.HardwareTnL, null);

            if (requirements.AntiAliasing <= caps.AntiAliasing.MaxSupported)
                settings.antiAliasing = requirements.AntiAliasing;
            else if (fallbacks.AntiAliasing <= caps.AntiAliasing.MaxSupported)
                settings.antiAliasing = fallbacks.AntiAliasing;
            else
                throw new OutputSettingsException(type.GetProperty("AntiAliasing"),
                                                  requirements.AntiAliasing, caps.AntiAliasing.MaxSupported);

            return settings;
        }
    }
}
