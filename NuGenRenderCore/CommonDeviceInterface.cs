using System;
using System.Collections.Generic;
using Genetibase.NuGenRenderCore.Logging;
using Genetibase.NuGenRenderCore.Resources;
using Genetibase.NuGenRenderCore.Resources;
using Genetibase.NuGenRenderCore.Settings;
using Microsoft.DirectX.Direct3D;
using ResourceManager=Genetibase.NuGenRenderCore.Resources.ResourceManager;

namespace Genetibase.NuGenRenderCore.Rendering.Devices
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
                                             new GraphicsDeviceRequirements(MultiSampleType.None,
                                                                            DeviceType.Hardware,
                                                                            new Format[] { Format.X8R8G8B8 },
                                                                            1, true,
                                                                            new DepthFormat[] { DepthFormat.D16 },
                                                                            false, true),
                                             /*new GraphicsDeviceRequirements(MultiSampleType.None,
                                                                            DeviceType.Software,
                                                                            new Format[] { Format.X8R8G8B8 },
                                                                            1, true,
                                                                            new DepthFormat[] { DepthFormat.D16 },
                                                                            false, true),*/
                                             base_path);
        }

        public abstract ResourceManager Resources { get; }
        public abstract RzLoader ResourceLoader { get; }
        public abstract ILog GeneralLog { get; }

        public const string GlobalResources = "GlobalSet";
    }

    public class CommonDeviceInterface : ICommonDeviceInterface
    {
        readonly int adapter;
        readonly GraphicsDeviceCaps outCaps;
        //readonly GraphicsDeviceSettings minSettings;
        readonly ResourceManager rzManager;
        readonly RzLoader rzLoader;
        readonly DirectFileLog log;

        public CommonDeviceInterface(int adapter, GraphicsDeviceRequirements baseReq, /*GraphicsDeviceRequirements minReq,*/
                                     string base_path)
        {
            this.adapter = adapter;
            outCaps = GraphicsDeviceCaps.GetAdapterCaps(adapter, baseReq.DeviceType, baseReq.DisplayFormat);
            //minSettings = GraphicsDeviceSettings.CreateFromRequirements(baseReq, outCaps, minReq);
            rzManager = new ResourceManager();
            rzManager.AddSet(new ResourceSet(GlobalResources));

            log = new DirectFileLog(base_path + "general.log");

            rzLoader = new RzLoader();
            rzLoader.RegisterContentLoader(new ImageContentLoader());
        }

        #region Properties

        public int Adapter
        {
            get { return adapter; }
        }

        public GraphicsDeviceCaps DeviceCaps
        {
            get { return outCaps; }
        }

//        public GraphicsDeviceSettings MinSettings
//        {
//            get { return minSettings; }
//        }
        #endregion

        public override void OpenRenderingInfoDialog()
        {
            /*RenderDeviceInfo info = new RenderDeviceInfo(minSettings, outCaps);
            info.ShowDialog();
            info.Dispose();*/
        }

        public override void OpenSupportedFormatsDlg()
        {
            /*SupportedFormatsInfoDlg dlg = new SupportedFormatsInfoDlg();
            dlg.ShowDialog();
            dlg.Dispose();*/
        }

        public override ResourceManager Resources
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

    /// <summary>
    /// Proivides an application interface to a graphics device instance
    /// </summary>
    public class DeviceInterface
    {
        private readonly Device device;
        private readonly CommonDeviceInterface cdi;
        private readonly IResourceSet resourceSet;
        private readonly HashTableSettings localSettings;

        public DeviceInterface(Device device, CommonDeviceInterface cdi, HashTableSettings localSettings)
        {
            this.device = device;
            this.cdi = cdi;
            this.localSettings = localSettings;
            resourceSet = new ResourceSet(device);
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

        public HashTableSettings LocalSettings
        {
            get { return localSettings; }
        }
        #endregion

        public IResource GetSharedResource(string url,
                                           ref List<ISharableResource> checkedOutList)
        {
            IResource[] dependans, dependancies;
            IResource rz = GetResource(url, true, ref checkedOutList, out dependans, out dependancies);
            if (dependans != null || dependancies != null)
                throw new Exception("Shared resource generated unhandled dependancies/dependants");
            return rz;
        }

        public IResource GetResource(string url, bool allowShare,
                                     ref List<ISharableResource> checkedOutList,
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
                IResource rz = cdi.ResourceLoader.LoadResource(uri, this, out dependants, out dependancies, out realRzId);
                // share if allowed
                if (allowShare && rz != null)
                {
                    resourceSet.Checkin(rz, realRzId, out sRz);
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
                    rz = (IResource)resourceSet.PeekRz(url).Value;
                return rz;
            }
            unhandledDependants = null;
            unhandledDependancies = null;
            return null;
        }
    }
}