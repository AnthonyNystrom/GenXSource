using System;
using System.Drawing;

namespace Genetibase.NuGenDEMVis.Data
{
    public class DataSourceInfo
    {
        public struct DataBandInfo
        {
            public string Name;
            public Image Image;
            public double MaxValue;
            public double MinValue;
            public double NODATAValue;

            /// <summary>
            /// Initializes a new instance of the DataBandInfo structure.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="image"></param>
            /// <param name="maxValue"></param>
            /// <param name="minValue"></param>
            /// <param name="nodataValue"></param>
            public DataBandInfo(string name, Image image, double maxValue,
                                double minValue, double nodataValue)
            {
                Name = name;
                Image = image;
                MaxValue = maxValue;
                MinValue = minValue;
                NODATAValue = nodataValue;
            }
        }

        public Size Resolution;
        public ushort Bpp;
        public string BppType;
        public DataBandInfo[] Bands;

        public bool SupportsRGB()
        {
            short numBandMatches = 0;
            // check for right bands to be present
            foreach (DataBandInfo band in Bands)
            {
                // Note: Is there a better way to identify bands?
                if (band.Name == "RedBand")
                    numBandMatches++;
                else if (band.Name == "GreenBand")
                    numBandMatches++;
                else if (band.Name == "BlueBand")
                    numBandMatches++;
            }
            return numBandMatches == 3;
        }

        public void GetRGABands(out int r, out int g, out int b)
        {
            r = g = b = -1;
            for (int i = 0; i < Bands.Length; i++)
            {
                // Note: Is there a better way to identify bands?
                if (Bands[i].Name == "RedBand")
                    r = i;
                else if (Bands[i].Name == "GreenBand")
                    g = i;
                else if (Bands[i].Name == "BlueBand")
                    b = i;
            }
        }
    }

    public interface IDataSourceReader : IDisposable
    {
        void OpenFile(string path, FileType fileType);
        void CloseFile();
        DataSourceInfo Info { get; }
        string File { get; }
    }

    public abstract class DataSourceReader : IDataSourceReader
    {
        protected DataSourceInfo info;
        protected string file;

        #region IDataSourceReader Members

        public abstract void OpenFile(string path, FileType fileType);
        public abstract void CloseFile();

        public DataSourceInfo Info
        {
            get { return info; }
        }

        public string File
        {
            get { return file; }
        }
        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            CloseFile();
        }
        #endregion
    }

    class ImageDataReader : DataSourceReader
    {
        Image imageCache;

        public override void OpenFile(string path, FileType fileType)
        {
            CloseFile();
            // TODO: Check file-type?
            imageCache = Image.FromFile(path);
        }

        public override void CloseFile()
        {
            if (imageCache != null)
            {
                imageCache.Dispose();
                imageCache = null;
            }
        }
    }
}