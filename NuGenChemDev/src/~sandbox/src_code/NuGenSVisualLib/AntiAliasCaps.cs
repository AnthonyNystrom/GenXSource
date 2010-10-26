using System.Collections.Generic;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Devices
{
    /// <summary>
    /// Encapsulates all the anti-aliasing/multi-sampling capabilities of a device
    /// </summary>
    public class AntiAliasCaps
    {
        readonly List<MultiSampleType> availableTypes;

        public AntiAliasCaps(int adapter, DeviceType devType, Format format)
        {
            availableTypes = new List<MultiSampleType>();
            for (int i = 0; i <= 16; i++)
            {
                CheckMultiSampleType(adapter, (MultiSampleType)i, devType, format);
            }
        }

        private void CheckMultiSampleType(int adapter, MultiSampleType multiSampleType, DeviceType devType, Format format)
        {
            if (Manager.CheckDeviceMultiSampleType(adapter, devType, format, true, multiSampleType))
                availableTypes.Add(multiSampleType);
        }

        public MultiSampleType MaxSupported
        {
            get
            {
                if (availableTypes.Count > 0)
                    return availableTypes[availableTypes.Count-1];
                return (MultiSampleType)(-1);
            }
        }

        public MultiSampleType[] SupportedSamples
        {
            get { return availableTypes.ToArray(); }
        }
    }
}
