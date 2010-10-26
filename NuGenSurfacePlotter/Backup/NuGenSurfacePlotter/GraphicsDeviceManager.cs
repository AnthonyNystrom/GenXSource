using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace NuGenCRBase.Managed.MDX1.Direct3D
{
    class GraphicsDeviceManager
    {
        public static bool CheckAdapterMeetsRequirements(int adapter, GraphicsDevice3DRequirements requirements)
        {
            // check formats against device type
            foreach (Format format in requirements.RtFormats)
            {
                if (!Manager.CheckDeviceType(adapter, requirements.DeviceType, requirements.DisplayFormat,
                                             format, requirements.Windowed))
                {
                    return false;
                }
            }
            // multisample
            if (!Manager.CheckDeviceMultiSampleType(adapter, requirements.DeviceType, requirements.RtFormats[0],
                                                    requirements.Windowed, requirements.MultiSample))
            {
                return false;
            }
            // depthstencil formats
            foreach (DepthFormat format in requirements.DepthFormats)
            {
                if (!Manager.CheckDepthStencilMatch(adapter, requirements.DeviceType, requirements.DisplayFormat,
                                                    requirements.RtFormats[0], format))
                {
                    return false;
                }
            }

            // rts
            Caps caps = Manager.GetDeviceCaps(0, requirements.DeviceType);
            if (caps.NumberSimultaneousRts < requirements.NumRTs)
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

        public static GraphicsDevice3DOutputDescription CreateOutputDescription(int adapter,
                                                                                GraphicsDevice3DRequirements minReqs,
                                                                                GraphicsDevice3DRequirements maxReqs)
        {
            GraphicsDevice3DRequirements requirements = CheckReqs(adapter, minReqs, maxReqs);

            CreateFlags cFlags;
            if (requirements.Pure)
                cFlags = CreateFlags.PureDevice;
            else if (requirements.HardwareTnL)
                cFlags = CreateFlags.HardwareVertexProcessing;
            else
                cFlags = CreateFlags.SoftwareVertexProcessing;
            return new GraphicsDevice3DOutputDescription(adapter, requirements.Windowed, requirements.DeviceType,
                                                         requirements.RtFormats[0], requirements.MultiSample,
                                                         requirements.DepthFormats[0], cFlags);
        }

        private static GraphicsDevice3DRequirements CheckReqs(int adapter, GraphicsDevice3DRequirements minReqs,
                                                              GraphicsDevice3DRequirements maxReqs)
        {
            DeviceType devType = minReqs.DeviceType;
            if (Manager.CheckDeviceType(adapter, maxReqs.DeviceType, minReqs.DisplayFormat, minReqs.RtFormats[0], maxReqs.Windowed))
                devType = maxReqs.DeviceType;

            Format[] rtFormats = minReqs.RtFormats;
            bool useMax = true;
            foreach (Format format in maxReqs.RtFormats)
            {
                if (!Manager.CheckDeviceType(adapter, devType, minReqs.DisplayFormat,
                                             format, minReqs.Windowed))
                {
                    useMax = false;
                    break;
                }
            }
            if (useMax)
                rtFormats = maxReqs.RtFormats;

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

            return new GraphicsDevice3DRequirements(ms, devType, rtFormats, minReqs.NumRTs, minReqs.Windowed, minReqs.DepthFormats, pure, tnl, null, null, Format.Unknown);
        }

        public static bool CreateGraphicsDevice3D(GraphicsDevice3DOutputDescription desc, Control deviceTarget,
                                                  out GraphicsDevice3DOutputDescription outDesc, out Device device,
                                                  out PresentParameters pParams)
        {
            try
            {
                pParams = new PresentParameters();
                if (desc.DepthFormat != DepthFormat.Unknown)
                {
                    pParams.EnableAutoDepthStencil = true;
                    pParams.AutoDepthStencilFormat = desc.DepthFormat;
                }
                if (desc.Format != Format.Unknown)
                    pParams.BackBufferFormat = desc.Format;
                pParams.MultiSample = desc.MultiSample;
                pParams.SwapEffect = SwapEffect.Discard;
                pParams.Windowed = desc.Windowed;

                device = new Device(desc.Adapter, desc.Type, deviceTarget, desc.CreateFlags, pParams);

                // capture actual final output description
                outDesc = new GraphicsDevice3DOutputDescription(desc.Adapter, desc.Windowed,
                                                                device.CreationParameters.DeviceType,
                                                                device.DisplayMode.Format,
                                                                device.GetBackBuffer(0, 0, BackBufferType.Mono).Description.MultiSampleType,
                                                                device.PresentationParameters.AutoDepthStencilFormat,
                                                                desc.CreateFlags);

                return true;
            }
            catch (Exception)
            { }

            pParams = null;
            device = null;
            outDesc = null;
            return false;
        }
    }
}