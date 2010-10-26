using System;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.View;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore
{
    /// <summary>
    /// Encapsulates the very core of a 3D rendering control
    /// </summary>
    public abstract partial class NuGenCoreRenderViewControl : UserControl
    {
        protected Device gDevice;
        protected PresentParameters pParams;
        protected string deviceFailMsg = "MDX: No device created";
        protected Matrix projMat, viewMat;

        internal GraphicsDeviceRequirements minReqs, desiredReqs;

        protected TargetCamera camera;
        protected SphericalRotationHandler viewInputHandler;

        Point mousePos;
        bool mouseDown;

        public NuGenCoreRenderViewControl(bool delayCreateDx)
        {
            InitializeComponent();

            // defaults
            minReqs = new GraphicsDeviceRequirements(MultiSampleType.None, DeviceType.Hardware,
                                                     new Format[] { Format.X8R8G8B8 }, 1, true,
                                                     new DepthFormat[] { DepthFormat.D16 },
                                                     false, false);
            desiredReqs = new GraphicsDeviceRequirements(MultiSampleType.FourSamples, DeviceType.Hardware,
                                                         new Format[] { Format.X8R8G8B8 }, 1, true,
                                                         new DepthFormat[] { DepthFormat.D16 },
                                                         false, true);

            if (!delayCreateDx)
                CreateDxDevice();
        }

        #region Device Setup

        protected virtual void CreateDxDevice()
        {
            if (GraphicsDeviceManager.CheckAdapterMeetsRequirements(0, minReqs))
            {
                GraphicsDeviceSettings outSettings = GraphicsDeviceManager.CreateOutputDescription(0, minReqs, desiredReqs);
                if (!GraphicsDeviceManager.CreateGraphicsDevice3D(outSettings, this, out outSettings, out gDevice, out pParams))
                    deviceFailMsg = "MDX: Failed to create device";

                if (gDevice != null)
                {
                    SetupView();
                }
            }
            else
            {
                deviceFailMsg = "MDX: Adapter does not meet minimum requirements";
                throw new Exception(deviceFailMsg);
            }
        }

        protected virtual void SetupView()
        {
            float near = 0.1f;
            float far = 1000f;
            /*if (abScene != null)
            {
                far = abScene.BoundingSphere * 5;
                near = far * 0.01f;
            }*/
            projMat = Matrix.PerspectiveFovLH((float)Math.PI / 4f, (float)Width / Height, near, far);
            //viewMat = Matrix.LookAtLH(new Vector3(40, 20, 20), new Vector3(), new Vector3(0, 1, 0));
        }

        #endregion

        protected abstract void RenderDxScene();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mouseDown && viewInputHandler != null)
            {
                viewInputHandler.MoveMouse(new Point(e.X - mousePos.X, e.Y - mousePos.Y));
                RenderDxScene();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                mousePos = new Point(e.X, e.Y);
                mouseDown = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
                mouseDown = false;
        }
    }
}