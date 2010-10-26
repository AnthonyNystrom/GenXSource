using Genetibase.NuGenDEMVis.GIS;
using Genetibase.NuGenDEMVis.Properties;
using OSGeo.GDAL;

namespace Genetibase.NuGenDEMVis.Data
{
    /// <summary>
    /// Profile for use on (RGBA etc.) images
    /// </summary>
    class ClrIntensityDataProfile : DataProfile
    {
        public ClrIntensityDataProfile(FileType[] availableTypes)
            : base("Color Intensity", "Based on color intensity of the image.",
                   availableTypes, new string[] { "jpg", "bmp", "png" })
        {
            // TODO: ^Could do with file type categories / profiles??

            subProfiles = new SubProfile[] { new SubProfile("Grey-Scale", Resource1.ClrIntensityProfile_GreyScaleDesc, new GreyScaleFilter()),
                                             new SubProfile("Full-Spectrum", Resource1.ClrIntensityProfile_FullSpectrum, new FullSpecrumFilter()) };
        }

        class GreyScaleFilter : IProfileDataFilter
        {
            #region IProfileDataFilter Members

            public object FilterData(IDataSourceReader dataSrc)
            {
                if (dataSrc is GDALReader)
                {
                    GDALReader reader = (GDALReader)dataSrc;
                    Band redBand = reader.GetRasterBand(1);
                    
                    byte[] data = new byte[reader.Info.Resolution.Width * reader.Info.Resolution.Height];
                    redBand.ReadRaster(0, 0, reader.Info.Resolution.Width, reader.Info.Resolution.Height,
                                       data, reader.Info.Resolution.Width, reader.Info.Resolution.Height,
                                       0, 0);
                    return data;
                }
                return null;
            }
            #endregion
        }

        class FullSpecrumFilter : IProfileDataFilter
        {
            #region IProfileDataFilter Members

            public object FilterData(IDataSourceReader dataSrc)
            {
                if (dataSrc is GDALReader)
                {
                    // get as many bands as available and average values
                    GDALReader reader = (GDALReader)dataSrc;
                    int[] avrData = new int[reader.Info.Resolution.Width * reader.Info.Resolution.Height];
                    byte[] tempData = new byte[reader.Info.Resolution.Width * reader.Info.Resolution.Height];
                    int numBands = reader.Info.Bands.Length;
                    for (int band = 0; band < numBands; band++)
                    {
                        Band bandData = reader.GetRasterBand(band + 1);
                        
                        bandData.ReadRaster(0, 0, reader.Info.Resolution.Width, reader.Info.Resolution.Height,
                                            tempData, reader.Info.Resolution.Width, reader.Info.Resolution.Height,
                                            0, 0);

                        // add to averages
                        for (int i = 0; i < tempData.Length; i++)
                        {
                            avrData[i] += tempData[i];
                        }
                    }

                    // average data into final byte[]
                    for (int i = 0; i < avrData.Length; i++)
                    {
                        tempData[i] = (byte)(avrData[i] / numBands);
                    }
                    return tempData;
                }
                return null;
            }
            #endregion
        }

    }
}