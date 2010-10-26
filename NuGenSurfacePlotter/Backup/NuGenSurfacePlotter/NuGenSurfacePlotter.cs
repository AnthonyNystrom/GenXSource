using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using NuGenCRBase.Managed.MDX1.Direct3D;
using NuGenSurfacePlotter.Rendering;
using NuGenSurfacePlotter.Colors;
using System.Threading;
using NuGenCRBase.AvalonBridge;
using Microsoft.DirectX;

namespace NuGenSurfacePlotter
{
    public partial class SurfacePlotterControl : ABRenderViewControl
    {
        public enum RenderingModes
        {
            Software,
            Hardware
        }

        public enum ContentTypes
        {
            SurfacePlot,
            Avalon
        }

        private RenderingModes renderingMode;
        private ContentTypes contentType;

        private Surface3DRenderer softRender;
        private string function;
        private int colorSchema;

        private ABScene3D cachedSurfaceScene;

        private bool enablePolygonSelection;
        private Vector3[] selectedPolygon;

        public SurfacePlotterControl()
            : base(false)
        {
            InitializeComponent();

            softRender = new Surface3DRenderer(70, 35, 40, 0, 0, ClientRectangle.Width, ClientRectangle.Height, 0.5, 0, 0);
            softRender.ColorSchema = new ColorSchema(colorSchema = 0);
            ResizeRedraw = true;

            ReBuildSceneBuffers();

            SurfaceFunction = "sin(x1)*cos(x2)/(sqrt(sqrt(x1*x1+x2*x2))+1)*10";
        }

        public bool LoadModel(string file)
        {
            if (contentType == ContentTypes.SurfacePlot)
            {
                cachedSurfaceScene = abScene;
                abScene = null;
            }
            bool result = LoadABSupportedScene(file);
            if (result)
            {
                contentType = ContentTypes.Avalon;
                SetupView();
                Invalidate();
            }
            else
            {
                abScene = cachedSurfaceScene;
            }
            return result;
        }

        public void LoadSurface()
        {
            if (contentType == ContentTypes.Avalon)
            {
                contentType = ContentTypes.SurfacePlot;

                if (abScene != null)
                    abScene.Dispose();

                abScene = cachedSurfaceScene;
                SetupView();
                ResetDevice();
                Invalidate();
            }
        }

        private void ReBuildSceneBuffers()
        {
            if (device != null && contentType == ContentTypes.SurfacePlot)
            {
                if (abScene != null)
                    abScene.Dispose();

                abScene = new ABScene3D();
                
                // Geometry
                abScene.Models = softRender.ToAvalonBridgeModel();
                abScene.Models[1].Geometry.Clr = softRender.PenColor.ToArgb();
                abScene.Models[0].BuildBuffers(device);
                abScene.Models[1].BuildBuffers(device);
                abScene.CalcBounds();

                // Cameras
                abScene.Cameras = new ABCamera[1];
                abScene.Cameras[0] = new ABCameraSpherical(abScene.BoundingSphere * 2f, abScene.Origin);
                abScene.Cameras[0].Scroll(new Vector3());
                abScene.CurrentCamera = 0;
            }
        }

        #region Properties

        [DefaultValue(NuGenSurfacePlotter.SurfacePlotterControl.RenderingModes.Software)]
        public RenderingModes RenderingMode
        {
            get { return renderingMode; }
            set
            {
                renderingMode = value;
                if (renderingMode == RenderingModes.Hardware && device != null)
                {
                    DoubleBuffered = false;
                    OnPaint(null);
                }
                else
                {
                    DoubleBuffered = true;
                    Invalidate();
                }
            }
        }

        [DefaultValue(NuGenSurfacePlotter.SurfacePlotterControl.ContentTypes.SurfacePlot)]
        public ContentTypes ContentType
        {
            get { return contentType; }
        }

        [Description("The function used to generate the surface"), DefaultValue("sin(x1)*cos(x2)/(sqrt(sqrt(x1*x1+x2*x2))+1)*10")]
        public string SurfaceFunction
        {
            get { return function; }
            set
            {
                try
                {
                    if (contentType == ContentTypes.SurfacePlot && value != function)
                    {
                        softRender.SetFunction(value);
                        function = value;
                        ReBuildSceneBuffers();
                        Invalidate();
                    }
                }
                catch { }
            }
        }

        [Description("The color scheme of the surface"), DefaultValue(0)]
        public int SurfaceColorSchema
        {
            get { return colorSchema; }
            set
            {
                if (contentType == ContentTypes.SurfacePlot && value < 360 && value >= 0)
                {
                    softRender.ColorSchema = new ColorSchema(colorSchema = value);
                    UpdateSurfaceColours();
                    Invalidate();
                }
            }
        }

        private void UpdateSurfaceColours()
        {
            if (abScene != null)
            {
                softRender.UpdateAvalonBridgeModelClrs(abScene.Models);
                abScene.Models[0].UpdateBufferVClrs();
            }
        }

        public double SurfaceDensity
        {
            get { return softRender.Density; }
            set { if (contentType == ContentTypes.SurfacePlot) { softRender.Density = value; ReBuildSceneBuffers(); Invalidate(); } }
        }

        public Color SurfaceLineClr
        {
            get { return softRender.PenColor; }
            set { if (contentType == ContentTypes.SurfacePlot && abScene != null) { softRender.PenColor = value; abScene.Models[1].Geometry.Clr = value.ToArgb(); Invalidate(); } }
        }

        public bool EnablePolygonSelection
        {
            get { return enablePolygonSelection; }
            set { enablePolygonSelection = value; }
        }

        public Vector3[] SelectedPolygon
        {
            get { return selectedPolygon; }
        }

        #endregion

        #region Overrides

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetupView();
            if (contentType == ContentTypes.SurfacePlot)
                softRender.ReCalculateTransformationsCoeficients(80, 40, 40, 0, 0, ClientRectangle.Width, ClientRectangle.Height, 0.5, 0, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (Monitor.TryEnter(this, 0))
                {
                    if (renderingMode == RenderingModes.Software)
                    {
                        if (contentType == ContentTypes.SurfacePlot)
                        {
                            e.Graphics.Clear(BackColor);
                            softRender.RenderSurface(e.Graphics);
                        }
                        else
                        {
                            StringFormat strFormat = new StringFormat();
                            strFormat.Alignment = StringAlignment.Center;
                            strFormat.LineAlignment = StringAlignment.Center;
                            e.Graphics.DrawString("Rendering mode unavailable for this content type", Font,
                                                  new SolidBrush(ForeColor), new RectangleF(0, 0, Width, Height),
                                                  strFormat);
                        }
                    }
                    else
                    {
                        if (device == null)
                        {
                            // display MDX fail message
                            StringFormat strFormat = new StringFormat();
                            strFormat.Alignment = StringAlignment.Center;
                            strFormat.LineAlignment = StringAlignment.Center;
                            e.Graphics.DrawString(deviceFailMsg, Font, new SolidBrush(ForeColor), new RectangleF(0, 0, Width, Height), strFormat);
                        }
                        else if (device.CheckCooperativeLevel())
                        {
                            RenderDxScene();
                        }
                    }
                    Monitor.Exit(this);
                }
            }
            catch (Exception)
            { Monitor.Exit(this); }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (renderingMode == RenderingModes.Software)
                base.OnPaintBackground(e);
        }

        #endregion
    }
}