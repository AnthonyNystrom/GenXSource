using Genetibase.NuGenDEMVis.GIS;
using OSGeo.GDAL;

namespace Genetibase.NuGenDEMVis.Data
{
    /// <summary>
    /// Profile for proper DEM files
    /// </summary>
    class GISValueDataProfile : DataProfile
    {
        public GISValueDataProfile(FileType[] availableTypes)
            : base("GIS Height Value", "Based on the height values", availableTypes,
                   new string[] { "dem" })
        {
            subProfiles = new SubProfile[] { new SubProfile("Raw height values", "Uses purely the height values for direct conversion", new RawHeightFilter()) };
        }
    }

    class RawHeightFilter : DataProfile.IProfileDataFilter
    {
        #region IProfileDataFilter Members

        public object FilterData(IDataSourceReader dataSrc)
        {
            if (dataSrc is GDALReader)
            {
                GDALReader reader = (GDALReader)dataSrc;
                Band band = reader.GetRasterBand(1);

                float[] data = new float[reader.Info.Resolution.Width * reader.Info.Resolution.Height];
                band.ReadRaster(0, 0, reader.Info.Resolution.Width, reader.Info.Resolution.Height,
                                data, reader.Info.Resolution.Width, reader.Info.Resolution.Height,
                                0, 0);
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] < 0 || data[i] > 32000)
                    {
                        data[i] = 0;
                    }
                }
                return data;
            }
            return null;
        }
        #endregion
    }
}