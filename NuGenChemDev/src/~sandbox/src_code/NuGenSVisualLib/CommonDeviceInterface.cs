using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Logging;
using NuGenSVisualLib.Resources;

namespace NuGenSVisualLib.Rendering.Devices
{
    /// <summary>
    /// Encapsulates a common interface for a device
    /// </summary>
    public abstract class ICommonDeviceInterface
    {
        public abstract void OpenRenderingInfoDialog();
        public abstract void OpenSupportedFormatsDlg();

        public static ICommonDeviceInterface NewInterface(int adapter, string base_path)
        {
            return new CommonDeviceInterface(adapter,
                                             new OutputRequirements(MultiSampleType.None,
                                                                    DeviceType.Hardware,
                                                                    Format.X8R8G8B8,
                                                                    true, DepthFormat.D16,
                                                                    true, null),
                                             new OutputRequirements(MultiSampleType.None,
                                                                    DeviceType.Software,
                                                                    Format.X8R8G8B8,
                                                                    true,
                                                                    DepthFormat.D16,
                                                                    false, null),
                                             base_path);
        }

        public abstract NuGenSVisualLib.Resources.ResourceManager Resources { get; }
        public abstract RzLoader ResourceLoader { get; }
        public abstract ILog GeneralLog { get; }

        public const string GlobalResources = "GlobalSet";
    }

    public class CommonDeviceInterface : ICommonDeviceInterface
    {
        int adapter;
        OutputCaps outCaps;
        OutputSettings minSettings;
        NuGenSVisualLib.Resources.ResourceManager rzManager;
        RzLoader rzLoader;
        DirectFileLog log;

        public CommonDeviceInterface(int adapter, OutputRequirements baseReq, OutputRequirements minReq,
                                     string base_path)
        {
            this.adapter = adapter;
            this.outCaps = OutputCaps.GetAdapterCaps(adapter, baseReq.DeviceType, baseReq.DeviceFormat);
            this.minSettings = OutputSettings.CreateFromRequirements(baseReq, outCaps, minReq);
            rzManager = new NuGenSVisualLib.Resources.ResourceManager();
            rzManager.AddSet(new NuGenSVisualLib.Resources.ResourceSet(GlobalResources));

            log = new DirectFileLog(base_path + "general.log");

            rzLoader = new RzLoader();
            rzLoader.RegisterContentLoader(new ImageContentLoader());
        }

        #region Properties

        public int Adapter
        {
            get { return adapter; }
        }

        public OutputCaps DeviceCaps
        {
            get { return outCaps; }
        }

        public OutputSettings MinSettings
        {
            get { return minSettings; }
        }
        #endregion

        public override void OpenRenderingInfoDialog()
        {
            RenderDeviceInfo info = new RenderDeviceInfo(minSettings, outCaps);
            info.ShowDialog();
            info.Dispose();
        }

        public override void OpenSupportedFormatsDlg()
        {
            SupportedFormatsInfoDlg dlg = new SupportedFormatsInfoDlg();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        public override NuGenSVisualLib.Resources.ResourceManager Resources
        {
            get { return rzManager; }
        }

        public override RzLoader ResourceLoader
        {
            get { return rzLoader; }
        }

        public override ILog GeneralLog
        {
            get { return log; }
        }
    }

    public class DeviceInterface
    {
        private Device device;
        private CommonDeviceInterface cdi;
        private IResourceSet resourceSet;

        public DeviceInterface(Device device, CommonDeviceInterface cdi)
        {
            this.device = device;
            this.cdi = cdi;
            this.resourceSet = new ResourceSet(device);
            cdi.Resources.AddSet(resourceSet);
        }

        #region Properties

        public Device Device
        {
            get { return device; }
        }

        public CommonDeviceInterface CDI
        {
            get { return cdi; }
        }

        public IResourceSet ResourceSet
        {
            get { return resourceSet; }
        }
        #endregion

        public IResource GetSharedResource(string url,
                                           ref List<ISharableResource> checkedOutList,
                                           DeviceInterface devIf)
        {
            IResource[] dependans, dependancies;
            IResource rz = GetResource(url, true, ref checkedOutList, devIf, out dependans, out dependancies);
            if (dependans != null || dependancies != null)
                throw new Exception("Shared resource generated unhandled dependancies/dependants");
            return rz;
        }

        public IResource GetResource(string url, bool allowShare,
                                     ref List<ISharableResource> checkedOutList,
                                     DeviceInterface devIf,
                                     out IResource[] unhandledDependants,
                                     out IResource[] unhandledDependancies)
        {
            // TODO: break-up url here
            Uri uri = new Uri(url);
            if (uri.IsFile)
            {
                // look for existing
                ISharableResource sRz = resourceSet.Checkout(url);
                if (sRz != null)
                {
                    if (sRz.Value is IResource)
                    {
                        checkedOutList.Add(sRz);
                        unhandledDependants = null;
                        unhandledDependancies = null;
                        return (IResource)sRz.Value;
                    }
                    // throw away
                    resourceSet.Checkin(sRz);
                }
                // load up from fresh
                IResource[] dependants, dependancies;
                string realRzId;
                IResource rz = cdi.ResourceLoader.LoadResource(uri, devIf, out dependants, out dependancies, out realRzId);
                // share if allowed
                if (allowShare && rz != null)
                {
                    resourceSet.Checkin(rz, url, out sRz);
                    checkedOutList.Add(sRz);

                    // deal with dependants & dependancies
                    if (dependants != null)
                    {
                        foreach (IResource item in dependants)
                        {
                            resourceSet.Checkin(item, item.Id, out sRz);
                            checkedOutList.Add(sRz);
                        }
                    }
                    if (dependancies != null)
                    {
                        foreach (IResource item in dependancies)
                        {
                            resourceSet.Checkin(item, item.Id, out sRz);
                            checkedOutList.Add(sRz);
                        }
                    }
                    unhandledDependants = null;
                    unhandledDependancies = null;
                }
                else
                {
                    unhandledDependants = dependants;
                    unhandledDependancies = dependancies;
                }
                // double check we have right rz
                if (realRzId != null)
                    rz = (IResource)resourceSet.PeekRz(realRzId).Value;
                return rz;
            }
            unhandledDependants = null;
            unhandledDependancies = null;
            return null;
        }
    }
}