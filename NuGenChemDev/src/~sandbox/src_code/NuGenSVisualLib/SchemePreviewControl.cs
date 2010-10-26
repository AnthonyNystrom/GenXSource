using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using System.Threading;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Chem;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK;
using javax.vecmath;
using NuGenSVisualLib.Rendering;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Pipelines;
using Org.OpenScience.CDK.Config;
using NuGenSVisualLib.Rendering.ThreeD;

namespace NuGenSVisualLib
{
    partial class SchemePreviewControl : UserControl
    {
        OutputSettings outSettings;
        PresentParameters pParams;
        Device device;

        public enum Status
        {
            NoData,
            Generating,
            Rendering
        }

        Status status;
        int loadProgress;
        Thread generatorThread;

        bool updateBonds, updateAtoms;

        Microsoft.DirectX.Direct3D.Font deviceFont;

        IAtom[] atoms;
        IBond[] bonds;

        GraphicsPipeline3D pipeline;
        Matrix projMat, viewMat;

        public event EventHandler OnNewPreview;
        bool wantPreview;

        MoleculeSceneManager sceneManger;

        CompleteOutputDescription latestCoDesc;

        public SchemePreviewControl()
        {
            InitializeComponent();

            this.Font = new System.Drawing.Font("SansSerif", 24, FontStyle.Bold);

            generatorThread = new Thread(GeneratePreviewProcess);

            CreatePreviewMolecule();
        }

        private void CreatePreviewMolecule()
        {
            atoms = new IAtom[] { new Atom("O", new Point3d(0, 0, 0)), new Atom("H", new Point3d(0, 0, 1)) };

            ElementPTFactory elements = ElementPTFactory.Instance;
            foreach (IAtom atom in atoms)
            {
                PeriodicTableElement pe = elements.getElement(atom.Symbol);
                if (pe != null)
                {
                    atom.AtomicNumber = pe.AtomicNumber;
                    atom.Properties["PeriodicTableElement"] = pe;
                    atom.Properties["Period"] = int.Parse(pe.Period);
                }
            }

            bonds = new IBond[] { new Bond(atoms[0], atoms[1], 1) };

            projMat = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, Width / Height, 0.1f, 20);
            viewMat = Matrix.LookAtLH(new Vector3(2, 3, -1), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
        }

        #region Properties
        public OutputSettings OutSettings
        {
            get { return outSettings; }
            set
            {
                outSettings = value;
                OnNewDevice();
            }
        }

        public Status PreviewStatus
        {
            get { return status; }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;

                // calc text clr
                int R = value.R + 128;
                int G = value.G + 128;
                int B = value.B + 128;
                if (R > 255)
                    R -= 255;
                if (G > 255)
                    G -= 255;
                if (B > 255)
                    B -= 255;
                ForeColor = Color.FromArgb(R, G, B);
            }
        }

        public CompleteOutputDescription CODesc
        {
            get { return latestCoDesc; }
        }
        #endregion

        #region Events

        private void OnNewDevice()
        {
            if (outSettings != null)
                outSettings.CreateDevice(this, out device, out pParams);

            if (device != null)
            {
                pipeline = new GraphicsFixedPipeline3D(device);
                deviceFont = new Microsoft.DirectX.Direct3D.Font(device, Font);

                sceneManger = new MoleculeSceneManager(device, outSettings);
            }
            else
            {
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Enabled)
            {
                lock (this)
                {
                    if (device != null)
                    {
                        device.Dispose();
                        device = null;
                    }
                }
                Refresh();
            }

            base.OnEnabledChanged(e);
        }
        #endregion

        public void SetScheme(MoleculeRenderingScheme scheme, CompleteOutputDescription coDesc)
        {
            latestCoDesc = new CompleteOutputDescription(coDesc);
            sceneManger.SetScheme(scheme);
            sceneManger.SetOutputDesc(coDesc);

            scheme.device = device;
            // build preview via thread
            if (generatorThread.ThreadState == ThreadState.Running)
            {
                // abort thread
                generatorThread.Abort();
                generatorThread.Join();
            }
            generatorThread = new Thread(GeneratePreviewProcess);
            generatorThread.Start();

            wantPreview = true;
        }

        protected void GeneratePreviewProcess()
        {
            try
            {
                status = Status.Generating;
                loadProgress = 0;

                Thread.Sleep(500);

                if (updateBonds || updateAtoms)
                {
                    sceneManger.UpdateAtoms(latestCoDesc, atoms);
                    loadProgress = 50;
                    OnPaint(null);
                }
                else
                {
                    sceneManger.OnNewDataSource(atoms, bonds, new Vector3(0, 0, 0.5f), new Bounds3D(new Vector3(), new Vector3(), 4));
                    loadProgress = 50;
                    OnPaint(null);
                }

                loadProgress = 100;

                status = Status.Rendering;
                OnPaint(null);
            }
            catch (ThreadAbortException tae)
            {
                // clean up
                status = Status.NoData;
                //scheme.Clear();
            }
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
                Texture rt = null;
                Surface oldRt = null;
                Matrix proj = projMat;
                if (Monitor.TryEnter(this, 0))
                {
                    while (true)
                    {
                        device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackColor, 1.0f, 0);

                        if (status == Status.NoData)
                        {
                            device.BeginScene();
                            // draw text
                            deviceFont.DrawText(null, "No preview available", new Rectangle(0, 0, Width, Height), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, ForeColor);
                            device.EndScene();
                        }
                        else if (status == Status.Generating)
                        {
                            device.BeginScene();
                            // draw loading text & progress bar
                            deviceFont.DrawText(null, "Generating preview", new Rectangle(0, 0, Width, Height), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, ForeColor);

                            //CustomVertex.TransformedColored[] progTris = new CustomVertex.TransformedColored[4];
                            //int foreClrInt = ForeColor.ToArgb();
                            //float height = 1.0f / (Height * 0.1f);
                            //float width = 1.0f / ((Width / 100f) * (float)loadProgress);
                            //progTris[0].Color = progTris[1].Color = foreClrInt;
                            //progTris[1].Position = new Vector4(width, 0, 0, 1);

                            //progTris[2].Color = progTris[3].Color = foreClrInt;
                            //progTris[2].Position = new Vector4(width, height, 0, 1);
                            //progTris[3].Position = new Vector4(0, height, 0, 1);

                            //device.RenderState.Lighting = false;
                            //device.VertexFormat = CustomVertex.TransformedColored.Format;
                            //device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, progTris);

                            device.EndScene();
                        }
                        else
                        {
                            device.TestCooperativeLevel();

                            // render preview
                            pipeline.ProjectionMatrix = proj;
                            pipeline.ViewMatrix = viewMat;
                            pipeline.WorldMatrix = Matrix.Identity;

                            // NOTE: Need to allow SM bypass of lighting??
                            device.Lights[0].Direction = new Vector3(-1, -1, 0);
                            device.Lights[0].DiffuseColor = ColorValue.FromColor(Color.White);
                            device.Lights[0].Type = LightType.Directional;
                            device.Lights[0].Enabled = true;

                            sceneManger.RenderSceneFrame(pipeline, this.Width, this.Height);
                        }

                        device.Present();

                        if (wantPreview && status == Status.Rendering)
                        {
                            // do another render into new RT surface
                            rt = new Texture(device, 128, 128, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);

                            oldRt = device.GetRenderTarget(0);
                            device.SetRenderTarget(0, rt.GetSurfaceLevel(0));

                            proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, 1f, 1f, 40);

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

                    Monitor.Exit(this);
                }
            }
        }

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    if (device == null)
        //        base.OnPaintBackground(e);
        //}

//        public void UpdateBonds()
//        {
//            // pass to thread
//            lock (this)
//            {
//                if (generatorThread.ThreadState == ThreadState.Running || generatorThread.ThreadState == ThreadState.WaitSleepJoin)
//                {
//                    //generatorThread.Abort();
//                    //generatorThread.Join();
//                }
//                else
//                {
//                    status = Status.Generating;
//
//                    updateBonds = true;
//                    generatorThread = new Thread(GeneratePreviewProcess);
//                    generatorThread.Start();
//                }
//            }
//        }

//        public void UpdateAtoms(CompleteOutputDescription coDesc)
//        {
//            // pass to thread
//            lock (this)
//            {
//                latestCoDesc = coDesc;
//                if (generatorThread.ThreadState == ThreadState.Running || generatorThread.ThreadState == ThreadState.WaitSleepJoin)
//                {
//                    //generatorThread.Abort();
//                    //generatorThread.Join();
//                }
//                else
//                {
//                    status = Status.Generating;
//
//                    updateAtoms = true;
//                    generatorThread = new Thread(GeneratePreviewProcess);
//                    generatorThread.Start();
//                }
//            }
//        }

        public void UpdateScheme(bool wantAtomUpdate, bool wantBondUpdate, bool threaded)
        {
            if (threaded)
            {
            }
            else
            {
                lock (this)
                {
                    status = Status.Generating;
                    loadProgress = 0;
                }
                Thread.Sleep(500);

                wantPreview = true;
                if (wantAtomUpdate)
                {
                    sceneManger.UpdateAtoms(latestCoDesc, atoms);
                    loadProgress += 50;
                    OnPaint(null);
                }
                if (wantBondUpdate)
                {
                    sceneManger.UpdateBonds(latestCoDesc, bonds);
                    loadProgress += 50;
                    OnPaint(null);
                }

                loadProgress = 100;

                status = Status.Rendering;
                OnPaint(null);
            }
        }
    }
}