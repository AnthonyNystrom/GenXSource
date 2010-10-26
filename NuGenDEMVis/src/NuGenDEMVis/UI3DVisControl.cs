using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Genetibase.NuGenDEMVis.Data;
using Genetibase.NuGenDEMVis.GIS;
using Genetibase.NuGenDEMVis.Graph;
using Genetibase.NuGenDEMVis.Raster;
using Genetibase.NuGenDEMVis.Raster.Samplers;
using Genetibase.NuGenDEMVis.Rendering;
using Genetibase.NuGenDEMVis;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Settings;
using Genetibase.RasterDatabase;
using Genetibase.VisUI.Controls;
using Genetibase.VisUI.Rendering;
using Janus.Windows.ButtonBar;
using Microsoft.DirectX.Direct3D;
using NuGenVisUI;

namespace Genetibase.NuGenDEMVis.Controls
{
    partial class UI3DVisControl : UI3DControl
    {
        CrossSectionInfo crossSection;
        RasterDatabase.RasterDatabase database;
        VisRenderingContext3D rContext;
        DigitalElevationMap dem;

        readonly List<DataSourceItem> diffuseDataSrcs;
        readonly List<DataSourceItem> heightDataSrcs;

        GDALReader reader;

        public UI3DVisControl()
        {
            InitializeComponent();

            diffuseDataSrcs = new List<DataSourceItem>();
            heightDataSrcs = new List<DataSourceItem>();

            // create profiles
            profiles = new GraphicsProfile[1];
            profiles[0] = new DEMGraphicsProfile("CPU Basic Profile", "Basic rendering of CPU DEM",
                                              new GraphicsDeviceRequirements(MultiSampleType.None, DeviceType.Hardware,
                                                                             new Format[] { Format.X8R8G8B8 }, 1, true,
                                                                             new DepthFormat[] { DepthFormat.D16 },
                                                                             false, false),
                                              new GraphicsDeviceRequirements(MultiSampleType.FourSamples, DeviceType.Hardware,
                                                                             new Format[] { Format.X8R8G8B8 }, 1, true,
                                                                             new DepthFormat[] { DepthFormat.D16 },
                                                                             true, true,
                                                                             new Version(1, 1), new Version(2, 0)));
        }

        public void LoadVisualization(DataProfile dataProfile, DataProfile.SubProfile subProfile,
                                      IDataSourceReader dataSrcReader, DataSourceInfo dataSrcInfo)
        {
            renderFrame = true;
            renderingThread.Start();

            controlStatus = ControlStatus.Loading;
            rContext.LoadingLayer.SetProgress(0);
            rContext.LoadingLayer.SetText("Loading... DEM Geometry");
            rContext.LoadingLayer.Visible = true;
            Render();

            // load data from dlg into database
            database = new RasterDatabase.RasterDatabase();
            object filteredData = subProfile.Filter.FilterData(dataSrcReader);
            if (filteredData is byte[])
            {
                DataLayer dataLayer = new DataLayer("DEM", 8, "byte");
                dataLayer.AddArea(new ByteArea(new Rectangle(new Point(), dataSrcReader.Info.Resolution),
                                  new RectangleF(0, 0, 1, 1),
                                  (byte[])filteredData, dataSrcReader.Info.Resolution));
                database.AddLayer(dataLayer);
            }
            else if (filteredData is float[])
            {
                DataLayer dataLayer = new DataLayer("DEM", 32, "float");
                dataLayer.AddArea(new FloatArea(new Rectangle(new Point(), dataSrcReader.Info.Resolution),
                                  new RectangleF(0, 0, 1, 1),
                                  (float[])filteredData, dataSrcReader.Info.Resolution));
                database.AddLayer(dataLayer);
            }

            // load data source(s)
            DataSourceItem item = new DataSourceItem(dataSrcInfo.BppType,
                                                     (Bitmap)PreviewRasterizer.DrawRotatedBandPreview(dataSrcInfo.Bands,
                                                                                                      64, dataSrcInfo));
            heightDataSrcs.Add(item);

            // load diffuse sources
            // grey-scale for height
            GreyScaleDEMSampler srcImgSampler = new GreyScaleDEMSampler();
            Bitmap srcImg = srcImgSampler.GenerateBitmap(new Size(64, 64), database.Layers[0].Areas[0]);
            item = new DataSourceItem("Height", srcImg);
            item.DEMSampler = srcImgSampler;
                                      //(Bitmap)PreviewRasterizer.DrawRotatedBandPreview(new DataSourceInfo.DataBandInfo[] { new DataSourceInfo.DataBandInfo("Source", srcImg) },
                                                                                       //64, null));
            diffuseDataSrcs.Add(item);

            // load colour image if possible as other source (i.e the original if an RGB img)
            if (dataSrcReader.Info.SupportsRGB())
            {
                item = new DataSourceItem("SourceRGB",
                                          SourceDataDiffuseSampler.SampleRGBDiffuseMap((GDALReader)dataSrcReader,
                                                                                       new Size(64, 64)));
                diffuseDataSrcs.Add(item);
                reader = (GDALReader)dataSrcReader;
            }

            // height band range
            HeightBandRange hbr = new HeightBandRange();
            hbr.AddBand(0, Color.DarkGreen);
            hbr.AddBand(0.25f, Color.Blue);
            hbr.AddBand(0.4f, Color.DarkGreen);
            hbr.AddBand(0.5f, Color.Green);
            hbr.AddBand(0.8f, Color.Gray);
            hbr.AddBand(1, Color.White);
            HeightBandDEMSampler hBandSampler = new HeightBandDEMSampler(hbr);
            item = new DataSourceItem("HeightBands",
                                      hBandSampler.GenerateBitmap(new Size(64, 64), database.Layers[0].Areas[0]));
            item.DEMSampler = hBandSampler;
            diffuseDataSrcs.Add(item);

            // load database into visualization
            dem = new DigitalElevationMap(database.Area.Size, database, rContext.DevIf, (GDALReader)dataSrcReader/*,
                                          hBandSampler.GenerateBitmap(new Size(512, 512),
                                          database.Layers[0].Areas[0])*/
                                                                        );
            rContext.SetDEM(dem);

            // setup geometry layers
            geometryLayers.Add(new GeometryVisLayer("Diffuse", item.Thumbnail, true));
            geometryLayers.Add(new GeometryVisLayer("Overlay Grid", null, true));

            rContext.DevIf.LocalSettings["GeometryVisLayer.Diffuse"] = geometryLayers[0];
            rContext.DevIf.LocalSettings["GeometryVisLayer.Overlay"] = geometryLayers[1];

            // now load default texture
            SetDiffuseSource(0);

            rContext.LoadingLayer.SetProgress(100);
            Render();
            Thread.Sleep(500);

            rContext.LoadingLayer.Visible = false;
            controlStatus = ControlStatus.Idle;
            Render();
        }

        public override void Init(HashTableSettings settings, ICommonDeviceInterface cdi)
        {
            base.Init(settings, cdi);

            renderContext = rContext = new VisRenderingContext3D(settings, this, (CommonDeviceInterface)cdi,
                                                                 profiles[0]);
            renderContext.BackColor = BackColor;
        }

        public CrossSectionInfo CrossSection
        {
            get { return crossSection; }
        }

        public void SetDEMFillMode(FillMode fillMode)
        {
            rContext.SetDEMFillMode(fillMode);
            Render();
        }

        public ButtonBarItem[] GetHeightSources()
        {
            ButtonBarItem[] items = new ButtonBarItem[heightDataSrcs.Count];
            for (int i = 0; i < heightDataSrcs.Count; i++)
            {
                items[i] = new ButtonBarItem(heightDataSrcs[i].Name);
                items[i].Image = heightDataSrcs[i].Thumbnail;
            }
            return items;
        }

        public ButtonBarItem[] GetDiffuseSources()
        {
            ButtonBarItem[] items = new ButtonBarItem[diffuseDataSrcs.Count];
            for (int i = 0; i < diffuseDataSrcs.Count; i++)
            {
                items[i] = new ButtonBarItem(diffuseDataSrcs[i].Name);
                items[i].Image = diffuseDataSrcs[i].Thumbnail;
            }
            return items;
        }

        public void SetDiffuseSource(int index)
        {
            // rebuild texture tree from new source
            rContext.LoadingLayer.SetProgress(0);
            rContext.LoadingLayer.SetText("Loading... Diffuse Texture");

            bool selfContained = false;
            if (controlStatus != ControlStatus.Loading)
            {
                controlStatus = ControlStatus.Loading;
                rContext.LoadingLayer.Visible = true;
                selfContained = true;
            }
            Render();

            if (diffuseDataSrcs[index].DEMSampler != null)
                dem.RebuildDiffuseTextures(diffuseDataSrcs[index].DEMSampler);
            else
                dem.RebuildDiffuseTextures(reader);

            rContext.LoadingLayer.SetProgress(100);
            Render();

            if (selfContained)
            {
                Thread.Sleep(500);

                controlStatus = ControlStatus.Idle;
                rContext.LoadingLayer.Visible = false;
                Render();
            }
        }

        public bool HeightShadingEnabled
        {
            get { return (bool)Settings["GeometryVis.HeightShadingEnabled"]; }
            set { Settings["GeometryVis.HeightShadingEnabled"] = value; }
        }

        public Color HeightShadingColor
        {
            get { return (Color)Settings["GeometryVis.HeightShadingClr"]; }
            set { Settings["GeometryVis.HeightShadingClr"] = value; }
        }

        public void NewHeightMeasurer()
        {
            throw new NotImplementedException();
        }
    }
}