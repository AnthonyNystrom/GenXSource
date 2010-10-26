using System;
using System.Drawing;
using Genetibase.NuGenDEMVis.Data;
using Genetibase.NuGenDEMVis.Properties;
using OSGeo.GDAL;

namespace Genetibase.NuGenDEMVis.GIS
{
    public class GDAL_Info : DataSourceInfo
    {
        public string Driver;
        public string Projection;
        public string[] Metadata;
        public string[] ImgMetadata;
        public string[] SubDSMetadata;
        public string[] GeoLocMetadata;
        public double UpperLeftX, UpperLeftY, UpperRightX, UpperRightY, LowerLeftX, LowerLeftY, LowerRightX, LowerRightY, CenterX, CenterY;

        public GDAL_Info() { }

        public GDAL_Info(Dataset ds)
        {
            Projection = ds.GetProjectionRef();
            Resolution = new Size(ds.RasterXSize, ds.RasterYSize);
            Bands = new DataBandInfo[ds.RasterCount];
            for (int i = 0; i < ds.RasterCount; i++)
            {
                Band band = ds.GetRasterBand(i + 1);
                Bands[i] = new DataBandInfo();
                int temp;
                band.GetMaximum(out Bands[i].MaxValue, out temp);
                band.GetMaximum(out Bands[i].MinValue, out temp);
                band.GetNoDataValue(out Bands[i].NODATAValue, out temp);
                ColorInterp clr = band.GetRasterColorInterpretation();
                switch (clr)
                {
                    case ColorInterp.GCI_RedBand:
                        Bands[i].Name = "RedBand";
                        Bands[i].Image = Resource1.red_layer;
                        BppType += "R";
                        break;
                    case ColorInterp.GCI_GreenBand:
                        Bands[i].Name = "GreenBand";
                        Bands[i].Image = Resource1.green_layer;
                        BppType += "G";
                        break;
                    case ColorInterp.GCI_BlueBand:
                        Bands[i].Name = "BlueBand";
                        Bands[i].Image = Resource1.blue_layer;
                        BppType += "B";
                        break;
                    default:
                        Bands[i].Name = clr.ToString();
                        Bands[i].Image = null;
                        BppType += "?";
                        break;
                }
                BppType += "[" + Gdal.GetDataTypeName(band.DataType) + "]";
                Bpp += (ushort)Gdal.GetDataTypeSize(band.DataType);

                if (i + 1 < ds.RasterCount)
                    BppType += ",";
            }
            BppType += " (" + Bpp + ")";
            
            Driver = ds.GetDriver().LongName;

            Metadata = ds.GetMetadata("");
            ImgMetadata = ds.GetMetadata("IMAGE_STRUCTURE");
            SubDSMetadata = ds.GetMetadata("SUBDATASETS");
            GeoLocMetadata = ds.GetMetadata("GEOLOCATION");

            GDALInfoGetPosition(ds, 0.0, 0.0, out UpperLeftX, out UpperLeftY);
            GDALInfoGetPosition(ds, 0.0, ds.RasterYSize, out UpperRightX, out UpperRightY);
            GDALInfoGetPosition(ds, ds.RasterXSize, 0.0, out LowerLeftX, out LowerLeftY);
            GDALInfoGetPosition(ds, ds.RasterXSize, ds.RasterYSize, out LowerRightX, out LowerRightY);
            GDALInfoGetPosition(ds, ds.RasterXSize / 2, ds.RasterYSize / 2, out CenterX, out CenterY);
        }

        private static void GDALInfoGetPosition(Dataset ds, double x, double y, out double dfGeoX, out double dfGeoY)
        {
            double[] adfGeoTransform = new double[6];
            ds.GetGeoTransform(adfGeoTransform);

            dfGeoX = adfGeoTransform[0] + adfGeoTransform[1] * x + adfGeoTransform[2] * y;
            dfGeoY = adfGeoTransform[3] + adfGeoTransform[4] * x + adfGeoTransform[5] * y;
        }
    }

    public class GDALReader : DataSourceReader
    {
        Dataset dataset;

        public override void OpenFile(string path, FileType fileType)
        {
            try
            {
                Gdal.AllRegister();
            }
            catch (Exception e)
            {
                throw new ApplicationException("GDAL libraries missing!", e);
            }
            Gdal.SetConfigOption("NODATA_value", "-9999");
            dataset = Gdal.Open(path, Access.GA_ReadOnly);
            info = new GDAL_Info(dataset);
        }

        public override void CloseFile()
        {
            if (dataset != null)
            {
                dataset.Dispose();
                dataset = null;
            }
        }

        public Band GetRasterBand(int nBand)
        {
            return dataset.GetRasterBand(nBand);
        }

//        public static void ReadFile(string path)
//        {
//            Gdal.AllRegister();
//            
//            Dataset ds = Gdal.Open(path, Access.GA_ReadOnly);
//
//            GDAL_Info info = new GDAL_Info();
//            info.Projection = ds.GetProjectionRef();
//            info.RasterCount = ds.RasterCount;
//            info.RasterSize = new Size(ds.RasterXSize, ds.RasterYSize);
//            
//            Driver drv = ds.GetDriver();
//            info.Driver = drv.LongName;
//
//            info.Metadata = ds.GetMetadata("");
//            info.ImgMetadata = ds.GetMetadata("IMAGE_STRUCTURE");
//            info.SubDSMetadata = ds.GetMetadata("SUBDATASETS");
//            info.GeoLocMetadata = ds.GetMetadata("GEOLOCATION");
//
//            GDALInfoGetPosition(ds, 0.0, 0.0, out info.UpperLeftX, out info.UpperLeftY);
//            GDALInfoGetPosition(ds, 0.0, ds.RasterYSize, out info.UpperRightX, out info.UpperRightY);
//            GDALInfoGetPosition(ds, ds.RasterXSize, 0.0, out info.LowerLeftX, out info.LowerLeftY);
//            GDALInfoGetPosition(ds, ds.RasterXSize, ds.RasterYSize, out info.LowerRightX, out info.LowerRightY);
//            GDALInfoGetPosition(ds, ds.RasterXSize / 2, ds.RasterYSize / 2, out info.CenterX, out info.CenterY);
//
//            SaveBitmapBuffered(ds, "c:/temp.bmp", 1);
//        }

/*
        private static void GDALInfoGetPosition(Dataset ds, double x, double y, out double dfGeoX, out double dfGeoY)
        {
            double[] adfGeoTransform = new double[6];
            ds.GetGeoTransform(adfGeoTransform);

            dfGeoX = adfGeoTransform[0] + adfGeoTransform[1] * x + adfGeoTransform[2] * y;
            dfGeoY = adfGeoTransform[3] + adfGeoTransform[4] * x + adfGeoTransform[5] * y;
        }
*/

/*
        private static void SaveBitmapBuffered(Dataset ds, string filename, int iOverview)
        {
            // Get the GDAL Band objects from the Dataset
            Band redBand = ds.GetRasterBand(1);

            if (redBand.GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
            {
                SaveBitmapPaletteBuffered(ds, filename, iOverview);
                return;
            }

            if (redBand.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
            {
                SaveBitmapGrayBuffered(ds, filename, iOverview);
                return;
            }

            if (redBand.GetRasterColorInterpretation() != ColorInterp.GCI_RedBand)
            {
                Console.WriteLine("Non RGB images are not supported by this sample! ColorInterp = " +
                    redBand.GetRasterColorInterpretation().ToString());
                return;
            }

            if (ds.RasterCount < 3)
            {
                Console.WriteLine("The number of the raster bands is not enough to run this sample");
                System.Environment.Exit(-1);
            }

            if (iOverview >= 0 && redBand.GetOverviewCount() > iOverview)
                redBand = redBand.GetOverview(iOverview);

            Band greenBand = ds.GetRasterBand(2);

            if (greenBand.GetRasterColorInterpretation() != ColorInterp.GCI_GreenBand)
            {
                Console.WriteLine("Non RGB images are not supported by this sample! ColorInterp = " +
                    greenBand.GetRasterColorInterpretation().ToString());
                return;
            }

            if (iOverview >= 0 && greenBand.GetOverviewCount() > iOverview)
                greenBand = greenBand.GetOverview(iOverview);

            Band blueBand = ds.GetRasterBand(3);

            if (blueBand.GetRasterColorInterpretation() != ColorInterp.GCI_BlueBand)
            {
                Console.WriteLine("Non RGB images are not supported by this sample! ColorInterp = " +
                    blueBand.GetRasterColorInterpretation().ToString());
                return;
            }

            if (iOverview >= 0 && blueBand.GetOverviewCount() > iOverview)
                blueBand = blueBand.GetOverview(iOverview);

            // Get the width and height of the raster
            int width = redBand.XSize;
            int height = redBand.YSize;

            // Create a Bitmap to store the GDAL image in
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            DateTime start = DateTime.Now;

            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            redBand.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
            greenBand.ReadRaster(0, 0, width, height, g, width, height, 0, 0);
            blueBand.ReadRaster(0, 0, width, height, b, width, height, 0, 0);
            TimeSpan renderTime = DateTime.Now - start;
            Console.WriteLine("SaveBitmapBuffered fetch time: " + renderTime.TotalMilliseconds + " ms");

            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    Color newColor = Color.FromArgb(Convert.ToInt32(r[i + j * width]), Convert.ToInt32(g[i + j * width]), Convert.ToInt32(b[i + j * width]));
                    bitmap.SetPixel(i, j, newColor);
                }
            }

            bitmap.Save(filename);
        }
*/

/*
        private static void SaveBitmapPaletteBuffered(Dataset ds, string filename, int iOverview)
        {
            // Get the GDAL Band objects from the Dataset
            Band band = ds.GetRasterBand(1);
            if (iOverview >= 0 && band.GetOverviewCount() > iOverview)
                band = band.GetOverview(iOverview);

            ColorTable ct = band.GetRasterColorTable();
            if (ct == null)
            {
                Console.WriteLine("   Band has no color table!");
                return;
            }

            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                Console.WriteLine("   Only RGB palette interp is supported by this sample!");
                return;
            }

            // Get the width and height of the Dataset
            int width = band.XSize;
            int height = band.YSize;

            // Create a Bitmap to store the GDAL image in
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            DateTime start = DateTime.Now;

            byte[] r = new byte[width * height];

            band.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
            TimeSpan renderTime = DateTime.Now - start;
            Console.WriteLine("SaveBitmapBuffered fetch time: " + renderTime.TotalMilliseconds + " ms");

            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    ColorEntry entry = ct.GetColorEntry(r[i + j * width]);
                    Color newColor = Color.FromArgb(Convert.ToInt32(entry.c1), Convert.ToInt32(entry.c2), Convert.ToInt32(entry.c3));
                    bitmap.SetPixel(i, j, newColor);
                }
            }

            bitmap.Save(filename);
        }
*/

/*
        private static void SaveBitmapGrayBuffered(Dataset ds, string filename, int iOverview)
        {
            // Get the GDAL Band objects from the Dataset
            Band band = ds.GetRasterBand(1);
            if (iOverview >= 0 && band.GetOverviewCount() > iOverview)
                band = band.GetOverview(iOverview);
            
            // Get the width and height of the Dataset
            int width = band.XSize;
            int height = band.YSize;

            // Create a Bitmap to store the GDAL image in
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            DateTime start = DateTime.Now;

            byte[] r = new byte[width * height];

            band.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
            TimeSpan renderTime = DateTime.Now - start;
            Console.WriteLine("SaveBitmapBuffered fetch time: " + renderTime.TotalMilliseconds + " ms");

            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    Color newColor = Color.FromArgb(Convert.ToInt32(r[i + j * width]), Convert.ToInt32(r[i + j * width]), Convert.ToInt32(r[i + j * width]));
                    bitmap.SetPixel(i, j, newColor);
                }
            }

            bitmap.Save(filename);
        }
*/
    }
}