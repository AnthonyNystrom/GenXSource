using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Devices;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Pipelines;
using System.Threading;
using NuGenSVisualLib.Rendering.Lighting;

namespace NuGenSVisualLib
{
    public partial class LightPreviewControl : UserControl
    {
        OutputSettings outSettings;
        PresentParameters pParams;
        Device device;

        GraphicsPipeline3D pipeline;
        Matrix projMat, viewMat;

        Mesh previewObj;
        Mesh lightObj;

        LightingSetup lighting;

        VertexBuffer orbitLine;
        int orbitLineSz;

        bool wantPreview;
        bool wantUpdate;
        Thread renderThread;

        enum State
        {
            Scene,
            EditingLight
        }

        State state;
        int lightEditing;
        int maxNumLights;

        public event EventHandler OnNewPreview;

        public LightPreviewControl()
        {
            InitializeComponent();

            this.maxNumLights = 4;

            state = State.EditingLight;

            this.DoubleBuffered = false;

            renderThread = new Thread(RenderProcess);
            renderThread.Start();
        }

        public OutputSettings OutSettings
        {
            get { return outSettings; }
            set
            {
                outSettings = value;
                OnNewDevice();
            }
        }

        public LightingSetup Lighting
        {
            get { return lighting; }
            set { lighting = value; Invalidate(); }
        }

        private void OnNewDevice()
        {
            if (outSettings != null)
                outSettings.CreateDevice(panel1, out device, out pParams);

            if (device != null)
            {
                pipeline = new GraphicsFixedPipeline3D(device);
                //deviceFont = new Microsoft.DirectX.Direct3D.Font(device, Font);

                if (previewObj != null)
                    previewObj.Dispose();
                previewObj = Mesh.Sphere(device, 2, 32, 32);

                lightObj = Mesh.Sphere(device, 1, 4, 4);

                CreateOrbitLine();

                OnResize(null);
            }
            else
            {
            }
        }

        private void CreateOrbitLine()
        {
            if (orbitLine != null)
                orbitLine.Dispose();

            orbitLineSz = 60;
            double angle = Math.PI * ((360 / (double)orbitLineSz) / 180);
            orbitLine = new VertexBuffer(typeof(CustomVertex.PositionOnly), orbitLineSz + 1, device, Usage.None,
                                         CustomVertex.PositionOnly.Format, Pool.Managed);
            CustomVertex.PositionOnly[] verts = (CustomVertex.PositionOnly[])orbitLine.Lock(0, LockFlags.None);

            double currentAngle = 0;
            int vIdx = 0;
            for (int i = 0; i < orbitLineSz + 1; i++)
            {
                float x = (float)Math.Cos(currentAngle);
                float y = (float)Math.Sin(currentAngle);

                verts[vIdx++].Position = new Vector3(x, 0, y);

                currentAngle += angle;
            }

            orbitLine.Unlock();
        }

        protected override void OnResize(EventArgs e)
        {
            if (e != null)
                base.OnResize(e);

            if (device != null)
            {
                projMat = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)panel1.Width / (float)panel1.Height, 0.1f, 100);
                viewMat = Matrix.LookAtLH(new Vector3(15, 15, 15), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (device == null)
                base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (device == null)
            {
                // draw text
                StringFormat strFormatting = new StringFormat();
                strFormatting.Alignment = StringAlignment.Center;
                strFormatting.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString("No preview available", Font, new SolidBrush(ForeColor),
                                      new RectangleF(0, 0, Width, Height), strFormatting);

                base.OnPaint(e);
            }
            else
            {
                if (Monitor.TryEnter(this, 0))
                {
                    RenderDx();
                    Monitor.Exit(this);
                }
            }
        }

        private void RenderDx()
        {
            Texture rt = null;
            Surface oldRt = null;
            Matrix proj = projMat;
            Matrix view = viewMat;
            while (true)
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackColor, 1.0f, 0);

                if (lighting != null)
                {
                    // setup view
                    device.Transform.Projection = proj;
                    device.Transform.View = view;
                    device.Transform.World = Matrix.Identity;

                    device.RenderState.DiffuseMaterialSource = ColorSource.Material;
                    Material mat = new Material();
                    mat.AmbientColor = ColorValue.FromColor(Color.Black);
                    mat.DiffuseColor = ColorValue.FromColor(Color.White);
                    device.Material = mat;

                    if (lighting == null)
                        device.RenderState.Lighting = false;
                    else
                    {
                        device.RenderState.Lighting = true;
                        // setup dev lights
                        for (int light = 0; light < lighting.lights.Count; light++)
                        {
                            if (lighting.lights[light].Enabled)
                            {
                                device.Lights[light].Enabled = true;
                                //device.Lights[light].AmbientColor = ColorValue.FromColor(Color.Red);
                                device.Lights[light].Direction = ((DirectionalLight)lighting.lights[light]).Direction;
                                device.Lights[light].Diffuse = lighting.lights[light].Clr;
                                device.Lights[light].Type = LightType.Directional;
                                device.Lights[light].Update();
                            }
                            else
                                device.Lights[light].Enabled = false;
                        }
                        for (int i = lighting.lights.Count; i < maxNumLights; i++)
                        {
                            device.Lights[i].Enabled = false;
                        }
                    }

                    device.BeginScene();
                    previewObj.DrawSubset(0);
                    device.EndScene();

                    device.BeginScene();
                    if (state == State.EditingLight && oldRt == null)
                    {
                        // draw tracks for movement of light
                        Vector3 point = new Vector3();
                        if (lighting.lights[lightEditing] is DirectionalLight)
                        {
                            DirectionalLight light = (DirectionalLight)lighting.lights[lightEditing];
                            // get point
                            point = light.Direction;
                        }

                        // calc rotations to point
                        float yRot = -(float)Math.Atan(point.Z / point.X);
                        if (float.IsNaN(yRot))
                            yRot = 0;
                        float xRot = -(float)Math.Atan(point.Y / point.X);
                        if (float.IsNaN(xRot))
                            xRot = 0;

                        // transfer to matrix for rendering
                        Matrix orbitRot = Matrix.RotationY(yRot) *
                                          Matrix.RotationZ(xRot);
                        //Matrix.Scaling(5, 5, 5);

                        // ^^ precalc

                        device.RenderState.Lighting = false;

                        device.Indices = null;
                        device.SetStreamSource(0, orbitLine, 0);
                        device.VertexFormat = CustomVertex.PositionOnly.Format;

                        // 'X' orbit
                        device.Transform.World = orbitRot * Matrix.Scaling(5, 5, 5);
                        device.DrawPrimitives(PrimitiveType.LineStrip, 0, orbitLineSz);

                        // 'Y' orbit
                        device.Transform.World = Matrix.RotationX((float)(Math.PI / 2)) *
                                                 orbitRot * Matrix.Scaling(5, 5, 5);
                        device.DrawPrimitives(PrimitiveType.LineStrip, 0, orbitLineSz);

                        // light rep
                        device.Transform.World = Matrix.Translation(5, 0, 0) * orbitRot;

                        Material m = device.Material;

                        device.RenderState.Lighting = true;
                        Material mt = new Material();
                        mt.AmbientColor = ColorValue.FromColor(lighting.lights[lightEditing].Clr);
                        mt.DiffuseColor = ColorValue.FromColor(lighting.lights[lightEditing].Clr);
                        device.RenderState.DiffuseMaterialSource = ColorSource.Material;
                        device.Material = mt;
                        device.RenderState.AmbientColor = lighting.lights[lightEditing].Clr.ToArgb();

                        lightObj.DrawSubset(0);

                        device.Material = m;
                    }
                    device.EndScene();
                }
                device.Present();

                if (wantPreview)
                {
                    // do another render into new RT surface
                    rt = new Texture(device, 128, 128, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);

                    oldRt = device.GetRenderTarget(0);
                    device.SetRenderTarget(0, rt.GetSurfaceLevel(0));

                    proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, 1f, 0.1f, 100);
                    view = Matrix.LookAtLH(new Vector3(7, 7, 7), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

                    wantPreview = false;
                }
                else if (oldRt != null)
                {
                    device.SetRenderTarget(0, oldRt);

                    if (OnNewPreview != null)
                        OnNewPreview(rt, null);
                    break;
                }
                else
                    break;
            }
        }

        public void RenderProcess()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        while (true)
                        {
                            lock (this)
                            {
                                lock (renderThread)
                                {
                                    if (wantUpdate)
                                        wantUpdate = false;
                                    else
                                        break;
                                }
                                RenderDx();
                            }
                        }
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (ThreadInterruptedException) { }
                }
            }
            catch (ThreadAbortException) { }
        }

        public void QueueUpdate(bool preview)
        {
            if (Monitor.TryEnter(this, 0))
            {
                wantUpdate = true;
                this.wantPreview = preview;
                renderThread.Interrupt();
                Monitor.Exit(this);
            }
            else
            {
                lock (renderThread)
                {
                    wantUpdate = true;
                    this.wantPreview = preview;
                }
            }
        }
    }
}