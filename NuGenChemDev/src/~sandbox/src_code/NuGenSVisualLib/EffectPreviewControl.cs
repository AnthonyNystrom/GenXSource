using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Effects;
using NuGenSVisualLib.Rendering.Devices;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Pipelines;
using Microsoft.DirectX;
using System.Threading;
using NuGenSVisualLib.Rendering;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK;
using NuGenSVisualLib.Rendering.Chem.Schemes;
using javax.vecmath;
using NuGenSVisualLib.Rendering.ThreeD;

namespace NuGenSVisualLib
{
    public partial class EffectPreviewControl : UserControl
    {
        OutputSettings outSettings;
        PresentParameters pParams;
        Device device;

        GraphicsPipeline3D pipeline;
        Matrix projMat, viewMat;
        Microsoft.DirectX.Direct3D.Font deviceFont;

        public enum Status
        {
            NoData,
            Generating,
            Rendering
        }

        Status status;
        int loadProgress;

        MoleculeSceneManager sceneManger;

        CompleteOutputDescription latestCoDesc;
        IAtom[] atoms;

        public event EventHandler OnNewPreview;
        bool wantPreview;

        RenderingEffect effect;
        RenderingEffectSettings settings;

        public EffectPreviewControl()
        {
            InitializeComponent();

            CreatePreviewData();
        }

        private void CreatePreviewData()
        {
            atoms = new IAtom[] { new Atom("O", new Point3d(0, 0, 0)) };

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

            projMat = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, Width / Height, 0.1f, 20);
            viewMat = Matrix.LookAtLH(new Vector3(2, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            latestCoDesc = CompleteOutputDescription.New();
            latestCoDesc.SchemeSettings = new BallAndStickSchemeSettings();
            latestCoDesc.SchemeSettings.AtomLOD = 3;
        }

        public void SetEffect(RenderingEffectSettings settings)
        {
            lock (this)
            {
                this.effect = settings.GetEffect(device);
                this.settings = settings;

                sceneManger.SetEffect(this.effect);
                status = Status.Rendering;
                Invalidate();
            }
        }

        public void UpdateEffect()
        {
            lock (this)
            {
                effect.SetupForDevice(outSettings);
                Invalidate();
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
                            /*device.Lights[0].Direction = new Vector3(-1, -1, 0);
                            device.Lights[0].DiffuseColor = ColorValue.FromColor(Color.White);
                            device.Lights[0].Type = LightType.Directional;
                            device.Lights[0].Enabled = true;*/

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

        public CompleteOutputDescription CoDesc
        {
            set { latestCoDesc.AtomShadingDesc.MoleculeMaterials = value.AtomShadingDesc.MoleculeMaterials; }
        }

        public RenderingEffect Effect
        {
            get { return effect; }
        }
        #endregion

        private void OnNewDevice()
        {
            if (outSettings != null)
                outSettings.CreateDevice(this, out device, out pParams);

            if (device != null)
            {
                pipeline = new GraphicsFixedPipeline3D(device);
                deviceFont = new Microsoft.DirectX.Direct3D.Font(device, Font);

                sceneManger = new MoleculeSceneManager(device, outSettings);
                BallAndStickRenderingScheme scheme = new BallAndStickRenderingScheme(device);
                sceneManger.SetScheme(scheme);
                sceneManger.SetOutputDesc(latestCoDesc);
                sceneManger.OnNewDataSource(atoms, null, new Vector3(), new Bounds3D(new Vector3(), new Vector3(), 3));
            }
            else
            {
            }
        }

        public void UpdateView()
        {
            status = Status.Rendering;
            wantPreview = true;

            UpdateEffect();
        }
    }
}