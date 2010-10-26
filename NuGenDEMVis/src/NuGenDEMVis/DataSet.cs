using System.Drawing;
using Genetibase.NuGenDEMVis.GIS;
using OSGeo.GDAL;

namespace Genetibase.NuGenDEMVis.Data
{
    public struct AreaD
    {
        public double PositionX, PositionY;
        public double Width, Height;
    }

    /// <summary>
    /// Encapsulates a virtual set of layered data for a certain area
    /// </summary>
    class TopologyDataset
    {
        AreaD worldArea;

        TopologyDataset[] Subsets;
    }

    interface IDataSource
    {
        AreaD WorldArea { get; }
        void Sample(Point pOffset, Size size, ref byte[] buffer);
        void Sample(Point pOffset, Size size, ref double[] buffer);
    }

    abstract class DataSource : IDataSource
    {
        protected AreaD worldArea;

        public DataSource(AreaD worldArea)
        {
            this.worldArea = worldArea;
        }

        #region IDataSource Members

        public AreaD WorldArea
        {
            get { return worldArea; }
        }

        public abstract void Sample(Point pOffset, Size size, ref byte[] buffer);
        public abstract void Sample(Point pOffset, Size size, ref double[] buffer);
        #endregion
    }

    class GDALDataSource : DataSource
    {
        Dataset data;
        GDAL_Info info;

        public GDALDataSource(Dataset dataset, AreaD worldArea)
            : base(worldArea)
        {
            data = dataset;
            info = new GDAL_Info(dataset);
        }

        public override void Sample(Point pOffset, Size size, ref byte[] buffer)
        {
            Band band = data.GetRasterBand(1);
            band.ReadRaster(pOffset.X, pOffset.Y, size.Width, size.Height, buffer, size.Width, size.Height, 0, 0);
        }

        public override void Sample(Point pOffset, Size size, ref double[] buffer)
        {
            Band band = data.GetRasterBand(1);
            band.ReadRaster(pOffset.X, pOffset.Y, size.Width, size.Height, buffer, size.Width, size.Height, 0, 0);
        }
    }
}