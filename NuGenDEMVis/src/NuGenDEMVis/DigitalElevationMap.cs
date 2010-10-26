using System.Collections.Generic;
using System.Drawing;
using Genetibase.NuGenDEMVis.GIS;
using Genetibase.NuGenDEMVis.Rendering;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Resources;
using Genetibase.NuGenRenderCore.Scene;
using Genetibase.NuGenRenderCore.Shaders;
using Genetibase.RasterDatabase;
using Genetibase.VisUI.Entities.Helpers;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenVisUI;

namespace Genetibase.NuGenDEMVis
{
    public class DigitalElevationMap : SceneEntity, IDigitalElevationMap
    {
        private Size mapSize;
        private RangeF heightRange;
        private int maxDimension;

        RasterDatabase.RasterDatabase rDb;
        VertexBuffer vBuffer;

        RasterSampler sampler;

        GpuDemGeometry geom;

        Axis3DHelper axisHelper;
        DeviceInterface devIf;
        RotationAxis3DHelper rotAxisHelper;

        TextureResource overlayTexRz;

        ShaderInterface defaultShader, hClrShader, sm3Shader;

        DEMGraphicsProfile gProfile;

        CustomVertex.PositionColored[] clipBottomSideQuad;

        VerticalPointerEntity pointerEntity;

        private float maxDataValue;

        GDALReader reader;

        /// <summary>
        /// Initializes a new instance of the DigitalElevationMap class.
        /// </summary>
        /// <param name="mapSize">Size of the map</param>
        /// <param name="rDb"></param>
        /// <param name="devIf"></param>
        /// <param name="reader"></param>
        public DigitalElevationMap(Size mapSize, RasterDatabase.RasterDatabase rDb,
                                   DeviceInterface devIf, GDALReader reader)
            : base(new Vector3(-5f, -0.5f, -5f), new Vector3(5f, 1f, 5f))
        {
            this.mapSize = mapSize;
            maxDimension = mapSize.Width > mapSize.Height ? mapSize.Width : mapSize.Height;
            this.rDb = rDb;

            this.devIf = devIf;
            this.reader = reader;

            axisHelper = new Axis3DHelper(new Vector3(2.5f, 0.5f, 2.5f));
            rotAxisHelper = new RotationAxis3DHelper(new Vector3());
        }

        private void BuildGeometry()
        {
            geom = GpuDemGeometry.CreateGeometry(rDb, gDevice, out maxDataValue, reader);

            clipBottomSideQuad = new CustomVertex.PositionColored[4];
            int clr = Color.DarkGray.ToArgb();
            clipBottomSideQuad[1] = new CustomVertex.PositionColored(0, -0.5f, 0, clr);
            clipBottomSideQuad[0] = new CustomVertex.PositionColored(0, -0.5f, 5, clr);
            clipBottomSideQuad[3] = new CustomVertex.PositionColored(5, -0.5f, 0, clr);
            clipBottomSideQuad[2] = new CustomVertex.PositionColored(5, -0.5f, 5, clr);
        }

        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
 	        base.Init(devIf, sManager);

            BuildGeometry();
            
            axisHelper.Init(devIf, sManager);
            rotAxisHelper.Init(devIf, sManager);

            List<ISharableResource> shared = new List<ISharableResource>();
            overlayTexRz = (TextureResource)devIf.GetSharedResource("file://media/ui/vis/overlay-1s.png", ref shared);

            ShaderHLSL shader;
            if (gProfile.SupportsShaderOverlay)
            {
                shader = new ShaderHLSL(gDevice, devIf.LocalSettings["Base.Path"] + @"shaders\cpu_dem.fx");
                shader.Effect.Technique = shader.Effect.GetTechnique("LitTextured");
                defaultShader = new ShaderInterface(shader);
            }
            /*shader = new ShaderHLSL(gDevice, devIf.LocalSettings["Base.Path"] + @"shaders\cpu_dem_hClr.fx");
            shader.Effect.Technique = shader.Effect.GetTechnique("CPU_DEM_HeightClr");
            hClrShader = new ShaderInterface(shader);*/

            shader = new ShaderHLSL(gDevice, devIf.LocalSettings["Base.Path"] + @"shaders\gpu_dem.fx");
            shader.Effect.Technique = shader.Effect.GetTechnique("Basic");
            sm3Shader = new ShaderInterface(shader);
            
            /*Shape shape = ShapeContentLoader.LoadShape(gDevice, NuGenDEMVis.Properties.Resource1.VerticalPointer_Shape);
            pointerEntity = new VerticalPointerEntity(shape, rDb.Layers[0], maxDataValue);
            pointerEntity.Init(devIf, sManager);
            sManager.AddEntity(pointerEntity);*/

            /*axisHelper.Init(devIf, sManager);
            sManager.AddEntity(axisHelper);

            SetChildren(new IWorldEntity[] { axisHelper, pointerEntity });*/

            //geom.RebuildDiffuseTextures(new HeightMapDEMSampler());
        }

        protected virtual RangeF DetermineMapHeightRange()
        {
            RangeF range = new RangeF(float.MinValue, float.MaxValue);
            for (int x = 0; x < mapSize.Width; x++)
            {
                for (int y = 0; y < mapSize.Height; y++)
                {
                    float value = this[x, y];
                    if (value > range.Max)
                        range.Max = value;
                    if (value < range.Min)
                        range.Min = value;
                }
            }

            return range;
        }

        public override void Render(GraphicsPipeline gPipeline)
        {
            gDevice.RenderState.Lighting = false;
            gPipeline.BeginScene();

            // just draw bounding box for now
            //NuGenRenderCore.Rendering.BoundingBox.DrawWireframe(gDevice, bBox, Color.DarkRed.ToArgb());

            // draw geometry
            if (geom != null)
            {
                geom.UseDiffuseTexture = ((GeometryVisLayer)devIf.LocalSettings["GeometryVisLayer.Diffuse"]).Enabled;

                ShaderInterface shader = null;
                /*if ((bool)devIf.LocalSettings["GeometryVis.HeightShadingEnabled"])
                {
                    shader = hClrShader;
                    shader.Effect.SetValue("MinHeight", -0.5f);
                    shader.Effect.SetValue("HeightScale", 1f);
                    shader.Effect.SetValue("HeightClr", ColorValue.FromColor((Color)devIf.LocalSettings["GeometryVis.HeightShadingClr"]));
                }
                else if (((GeometryVisLayer)devIf.LocalSettings["GeometryVisLayer.Overlay"]).Enabled)
                {
                    shader = defaultShader;
                    shader.Effect.SetValue("OverlayTexture", overlayTexRz.Texture);
                }*/
                shader = sm3Shader;

                if (shader != null)
                {
                    gPipeline.ShaderIf = shader;
                    gPipeline.ShaderEnabled = true;

                    /*shader.Effect.Begin(FX.None);
                    shader.Effect.BeginPass(0);*/

                    geom.Render(gPipeline);

                    /*shader.Effect.EndPass();
                    shader.Effect.End();*/

                    gPipeline.ShaderEnabled = false;
                }
                else
                {
                    geom.Render(gPipeline);
                }

                //gDevice.SetTexture(1, null);
            }

            /*gDevice.VertexFormat = CustomVertex.PositionColored.Format;
            gDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, clipBottomSideQuad);*/

            gPipeline.EndScene();
        }

        #region IDigitalElevationMap Members

        /// <summary>
        /// The size of the map (in pixels / discreet native dimensions)
        /// </summary>
        public Size MapSize
        {
            get { return mapSize; }
        }

        /// <summary>
        /// The range of heights in the map (typically 0.0 - 1.0)
        /// </summary>
        public RangeF HeightRange
        {
            get { return heightRange; }
        }

        public float this[int x, int y]
        {
            get { throw new System.NotImplementedException(); }
        }

        public int MaxDimension
        {
            get { return maxDimension; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }
        #endregion

        public void RebuildDiffuseTextures(IDEMDataSampler sampler)
        {
            //geom.RebuildDiffuseTextures(sampler);
        }

        public void RebuildDiffuseTextures(GDALReader gdalReader)
        {
            //geom.RebuildDiffuseTextures(gdalReader);
        }

        public void SetProfile(DEMGraphicsProfile profile)
        {
            gProfile = profile;
        }
    }
}