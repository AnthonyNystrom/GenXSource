using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Logging;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Layers;

namespace NuGenSVisualLib.Rendering.ThreeD.DX9
{
    abstract class RenderingContext3DDX9 : RenderingContext3D
    {
        protected Device device;
        protected PresentParameters presentParams;

        protected bool isDeviceLost;
        protected bool isActive;
        protected bool hasFocus;
        private bool isWindowActive = true;
        protected OutputCaps outCaps;
        protected OutputSettings outSettings;
        protected OutputRequirements outReqs;
        protected OutputRequirements outMinReqs;

        protected int width, height;

        protected ILog log;

        protected DeviceInterface devIf;

        protected LayerStack layers;

        public RenderingContext3DDX9(Control targetRenderArea, OutputRequirements oRequirements,
                                     OutputRequirements oMinReqs, CommonDeviceInterface cdi)
        {
            isDeviceLost = false;
            isActive = false;
            hasFocus = false;

            this.targetRenderArea = targetRenderArea;
            outReqs = oRequirements;
            outMinReqs = oMinReqs;
            log = cdi.GeneralLog;

            presentParams = new PresentParameters();
            outCaps = cdi.DeviceCaps;

            layers = new LayerStack();

            InitializeEnvironment(cdi);

            view3D = new RenderingView3DPer(Matrix.Identity, Matrix.Identity);
        }

        protected virtual void CreateDevice()
        {
            try
            {
                // Create the device
                device = new Device(outSettings.Adapter, outSettings.DeviceType, targetRenderArea,
                                    outSettings.CreateFlags, presentParams);
                log.AddItem(new LogItem(string.Format("Created rendering device (adapter:{0},deviceType:{1},createFlags:{2},pp:{3})",
                                                      outSettings.Adapter, outSettings.DeviceType, outSettings.CreateFlags, presentParams.AutoDepthStencilFormat),
                                        LogItem.ItemLevel.Info));

                // Setup the event handlers for our device
                device.DeviceLost += new System.EventHandler(this.DeviceLost);
                device.DeviceReset += new System.EventHandler(this.DeviceReset);
                device.Disposing += new System.EventHandler(this.DeviceDisposing);
                device.DeviceResizing += new System.ComponentModel.CancelEventHandler(this.DeviceResizing);

                // Initialize the app's device-dependent objects
                //try
                //{
                //    DeviceReset(null, null);
                    isActive = true;
                //}
                //catch
                //{
                //    // Cleanup before we try again
                //    DeviceLost(null, null);
                //    DeviceDisposing(null, null);

                //    device.Dispose();
                //    device = null;
                //    //if (this.Disposing)
                //    //    return;
                //}
            }
            catch
            {
                // FIXME: If that failed, fall back to the reference rasterizer
                throw new Exception("Failed to create desired device");
            }
        }

        protected virtual void InitializeEnvironment(CommonDeviceInterface cdi)
        {
            // Get device settings
            if (outCaps == null)
                outCaps = OutputCaps.GetDefaultAdapterCaps(outReqs);
            outSettings = OutputSettings.CreateFromRequirements(outReqs, outCaps, outMinReqs);

            // Set up the presentation parameters
            presentParams.Windowed = outReqs.Windowed;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.AutoDepthStencilFormat = outSettings.DepthFormat;
            presentParams.EnableAutoDepthStencil = (outSettings.DepthFormat != DepthFormat.Unknown);

            CreateDevice();

            devIf = new DeviceInterface(device, cdi);
        }

        public override void Render(object param)
        {
            if (isDeviceLost)
            {
                try
                {
                    // Test the cooperative level to see if it's okay to render
                    device.TestCooperativeLevel();
                }
                catch (DeviceLostException)
                {
                    // If the device was lost, do not render until we get it back
                    isWindowActive = false;
                    return;
                }
                catch (DeviceNotResetException)
                {
                    // Check if the device needs to be resized.
                    isDeviceLost = true;

                    // Reset the device and resize it
                    try
                    {
                        device.Reset(presentParams);
                    }
                    catch (Exception) { return; }

                    try
                    {
                        DeviceResizing(device, new System.ComponentModel.CancelEventArgs());
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                isDeviceLost = false;
            }

            if (isActive && device != null)
            {
                try
                {
                    Render();
                }
                catch (DeviceLostException)
                {
                    isDeviceLost = true;
                }
            }
        }

        protected abstract void Render();

        #region Window Management Functions

        public override void Dispose()
        {
            isActive = false;
            //isValid = false;

            if (device != null)
            {
                device.Dispose();
                device = null;
            }
        }

        protected void OnGotFocus(EventArgs e)
        {
            isWindowActive = true;
            hasFocus = true;
        }

        protected void OnLostFocus(EventArgs e)
        {
            hasFocus = false;
        }

        #endregion

        #region DirectX Callbacks

        protected virtual void DeviceReset(Object sender, EventArgs e)
        { }

        protected virtual void DeviceLost(Object sender, EventArgs e)
        { }

        protected virtual void DeviceDisposing(Object sender, EventArgs e)
        { }

        protected virtual void DeviceResizing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //// Check to see if we're closing or changing the form style
            //if ((isClosing_) || (isChangingFormStyle))
            //{
            //    // We are, cancel our reset, and exit
            //    e.Cancel = true;
            //    return;
            //}

            //if (!isWindowActive_)
            //    e.Cancel = true;
        }

        #endregion

        public override void OnResize(int width, int height)
        {
            this.width = width;
            this.height = height;
            view3D.SetupProjection(width, height);

            // resize layers
            foreach (ILayer layer in layers)
            {
                layer.Resize(width, height);
            }
        }

        public OutputSettings OutputSettings
        {
            get { return outSettings; }
        }

        public OutputCaps OutputCaps
        {
            get { return outCaps; }
        }
    }
}