using System;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering.Devices
{
    /// <summary>
    /// Encapsulates the graphics device manager
    /// </summary>
    public class GraphicsDeviceManager
    {
        public static bool CheckAdapterMeetsRequirements(int adapter, GraphicsDeviceRequirements requirements)
        {
            // check formats against device type
            foreach (Format format in requirements.RenderTargetFormats)
            {
                if (!Manager.CheckDeviceType(adapter, requirements.DeviceType, requirements.DisplayFormat,
                                             format, requirements.Windowed))
                {
                    return false;
                }
            }
            // multisample
            if (!Manager.CheckDeviceMultiSampleType(adapter, requirements.DeviceType, requirements.RenderTargetFormats[0],
                                                    requirements.Windowed, requirements.MultiSample))
            {
                return false;
            }
            // depthstencil formats
            foreach (DepthFormat format in requirements.DepthFormats)
            {
                if (!Manager.CheckDepthStencilMatch(adapter, requirements.DeviceType, requirements.DisplayFormat,
                                                    requirements.RenderTargetFormats[0], format))
                {
                    return false;
                }
            }

            // rts
            Caps caps = Manager.GetDeviceCaps(0, requirements.DeviceType);
            if (caps.NumberSimultaneousRts < requirements.NumRenderTargets)
                return false;

            // shaders
            if (requirements.PixelShader != null && requirements.PixelShader.CompareTo(caps.PixelShaderVersion) > 0)
                return false;
            if (requirements.VertexShader != null && requirements.VertexShader.CompareTo(caps.VertexShaderVersion) > 0)
                return false;

            if (requirements.Pure && !caps.DeviceCaps.SupportsPureDevice)
                return false;
            if (requirements.HardwareTnL && !caps.DeviceCaps.SupportsHardwareTransformAndLight)
                return false;

            return true;
        }

        public static GraphicsDeviceSettings CreateOutputDescription(int adapter,
                                                                     GraphicsDeviceRequirements minReqs,
                                                                     GraphicsDeviceRequirements maxReqs)
        {
            GraphicsDeviceRequirements requirements = CheckReqs(adapter, minReqs, maxReqs);

            CreateFlags cFlags;
            if (requirements.Pure)
                cFlags = CreateFlags.PureDevice;
            else if (requirements.HardwareTnL)
                cFlags = CreateFlags.HardwareVertexProcessing;
            else
                cFlags = CreateFlags.SoftwareVertexProcessing;
            return new GraphicsDeviceSettings(adapter, requirements.Windowed, requirements.DeviceType,
                                                       requirements.RenderTargetFormats[0], requirements.MultiSample,
                                                       requirements.DepthFormats[0], cFlags);
        }

        private static GraphicsDeviceRequirements CheckReqs(int adapter, GraphicsDeviceRequirements minReqs,
                                                            GraphicsDeviceRequirements maxReqs)
        {
            DeviceType devType = minReqs.DeviceType;
            if (Manager.CheckDeviceType(adapter, maxReqs.DeviceType, minReqs.DisplayFormat,
                                        minReqs.RenderTargetFormats[0], maxReqs.Windowed))
                devType = maxReqs.DeviceType;

            Format[] rtFormats = minReqs.RenderTargetFormats;
            bool useMax = true;
            foreach (Format format in maxReqs.RenderTargetFormats)
            {
                if (!Manager.CheckDeviceType(adapter, devType, minReqs.DisplayFormat,
                                             format, minReqs.Windowed))
                {
                    useMax = false;
                    break;
                }
            }
            if (useMax)
                rtFormats = maxReqs.RenderTargetFormats;

            MultiSampleType ms = minReqs.MultiSample;
            if (Manager.CheckDeviceMultiSampleType(adapter, devType, rtFormats[0],
                                                    minReqs.Windowed, maxReqs.MultiSample))
                ms = maxReqs.MultiSample;

            Caps caps = Manager.GetDeviceCaps(adapter, devType);
            bool pure = minReqs.Pure;
            if (maxReqs.Pure && caps.DeviceCaps.SupportsPureDevice)
                pure = true;
            bool tnl = minReqs.HardwareTnL;
            if (maxReqs.HardwareTnL && caps.DeviceCaps.SupportsHardwareTransformAndLight)
                tnl = true;

            return new GraphicsDeviceRequirements(ms, devType, rtFormats, minReqs.NumRenderTargets,
                                                  minReqs.Windowed, minReqs.DepthFormats, pure, tnl);
        }

        public static bool CreateGraphicsDevice3D(GraphicsDeviceSettings settings, Control deviceTarget,
                                                  out GraphicsDeviceSettings outSettings, out Device device,
                                                  out PresentParameters pParams)
        {
            try
            {
                pParams = new PresentParameters();
                if (settings.DepthFormat != DepthFormat.Unknown)
                {
                    pParams.EnableAutoDepthStencil = true;
                    pParams.AutoDepthStencilFormat = settings.DepthFormat;
                }
                if (settings.DisplayFormat != Format.Unknown)
                    pParams.BackBufferFormat = settings.DisplayFormat;
                pParams.MultiSample = settings.MultiSample;
                pParams.SwapEffect = SwapEffect.Discard;
                pParams.Windowed = settings.Windowed;

                device = new Device(settings.Adapter, settings.Type, deviceTarget, settings.CreateFlags, pParams);

                // capture actual final output description
                outSettings = new GraphicsDeviceSettings(settings.Adapter, settings.Windowed,
                                                         device.CreationParameters.DeviceType,
                                                         device.DisplayMode.Format,
                                                         device.GetBackBuffer(0, 0, BackBufferType.Mono).Description.MultiSampleType,
                                                         device.PresentationParameters.AutoDepthStencilFormat,
                                                         settings.CreateFlags);

                return true;
            }
            catch (Exception)
            { }

            pParams = null;
            device = null;
            outSettings = null;
            return false;
        }
    }
}