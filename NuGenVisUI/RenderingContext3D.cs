using System;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Logging;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Genetibase.NuGenRenderCore.Settings;
using Genetibase.VisUI.UI;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.Rendering
{
    /// <summary>
    /// Encapsulates a 3D rendering environment.
    /// </summary>
    public abstract class RenderingContext3D : IDisposable
    {
        protected DeviceInterface devIf;
        protected Device gDevice;
        protected PresentParameters presentParams;

        protected bool isDeviceLost;
        protected bool isActive;
        protected bool hasFocus;
        private bool isWindowActive = true;
        protected GraphicsDeviceCaps outCaps;
        protected GraphicsDeviceSettings outSettings;
        protected GraphicsProfile outProfile;
        protected int width, height;

        protected RenderingView3D view3D;
        protected RenderingSource3D renderSource3D;

        protected Control targetRenderArea;
        protected Color background;

        protected ILog log;

        protected LayerStack layers;
        GUILayerItem layerMouseOverItem;
        GUILayerItem layerMouseDownItem;

        protected Matrix rotation;
        protected float zoomLevel = 1;

        protected GraphicsPipeline gPipeline;

        protected GenericSceneManager<SceneEntity> sManager;

        #region Properties

        public RenderingView3D View3D
        {
            get { return view3D; }
            set
            {
                view3D = value;
            }
        }

        public RenderingSource3D RenderSource3D
        {
            get { return renderSource3D; }
            set
            {
                renderSource3D = value;
                if (value != null)
                    View3D = RenderingView3D.FromSource(value);
            }
        }

        public Color BackColor
        {
            get { return background; }
            set { background = value; }
        }

        public GraphicsDeviceSettings OutputSettings
        {
            get { return outSettings; }
        }

        public GraphicsDeviceCaps OutputCaps
        {
            get { return outCaps; }
        }

        public LayerStack Layers
        {
            get { return layers; }
        }
        #endregion

        public RenderingContext3D(Control targetRenderArea, GraphicsProfile profile, CommonDeviceInterface cdi,
                                  HashTableSettings localSettings)
        {
            isDeviceLost = false;
            isActive = false;
            hasFocus = false;

            this.targetRenderArea = targetRenderArea;
            outProfile = profile;
            log = cdi.GeneralLog;

            presentParams = new PresentParameters();
            outCaps = cdi.DeviceCaps;

            layers = new LayerStack();

            InitializeEnvironment(cdi, localSettings);

            view3D = new RenderingView3DPer(Matrix.Identity, Matrix.Identity);
            view3D.Near = 0.1f;
            view3D.Far = 25;

            int sceneSize = 60;
            sManager = new GenericSceneManager<SceneEntity>(gDevice, outSettings,
                                                            new Vector3(sceneSize / 2f, sceneSize / 2f, sceneSize / 2f), sceneSize);
        }

        protected virtual void CreateDevice()
        {
            try
            {
                // Create the device
                gDevice = new Device(outSettings.Adapter, outSettings.Type, targetRenderArea,
                                     outSettings.CreateFlags, presentParams);
                log.AddItem(new LogItem(string.Format("Created rendering device (adapter:{0},deviceType:{1},createFlags:{2},pp:{3})",
                                                      outSettings.Adapter, outSettings.Type, outSettings.CreateFlags, presentParams.AutoDepthStencilFormat),
                                        LogItem.ItemLevel.Info));

                // Setup the event handlers for our device
                gDevice.DeviceLost += this.DeviceLost;
                gDevice.DeviceReset += this.DeviceReset;
                gDevice.Disposing += this.DeviceDisposing;
                gDevice.DeviceResizing += this.DeviceResizing;

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

        protected virtual void InitializeEnvironment(CommonDeviceInterface cdi, HashTableSettings localSettings)
        {
            // Get device settings
            if (outCaps == null)
                outCaps = GraphicsDeviceCaps.GetDefaultAdapterCaps(outProfile.RecommendedVariations[0]);
            // find first recommended settings with full match
            bool fullMatch = false;
            outSettings = GraphicsDeviceSettings.CreateFromRequirements(outProfile.RecommendedVariation,
                                                                        outCaps, outProfile.MinReqs,
                                                                        out fullMatch);

            // Set up the presentation parameters
            presentParams.Windowed = outProfile.RecommendedVariations[0].Windowed;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.AutoDepthStencilFormat = outSettings.DepthFormat;
            presentParams.EnableAutoDepthStencil = (outSettings.DepthFormat != DepthFormat.Unknown);
            presentParams.MultiSample = outSettings.MultiSample;

            CreateDevice();

            devIf = new DeviceInterface(gDevice, cdi, localSettings);

            gPipeline = new GraphicsPipeline(gDevice);
        }

        public Texture RenderToTexture(object param, Size resolution)
        {
            if (isDeviceLost)
            {
                try
                {
                    // Test the cooperative level to see if it's okay to render
                    gDevice.TestCooperativeLevel();
                }
                catch (DeviceLostException)
                {
                    // If the device was lost, do not render until we get it back
                    isWindowActive = false;
                    return null;
                }
                catch (DeviceNotResetException)
                {
                    // Check if the device needs to be resized.
                    isDeviceLost = true;

                    // Reset the device and resize it
                    try
                    {
                        gDevice.Reset(presentParams);
                    }
                    catch (Exception) { return null; }

                    try
                    {
                        DeviceResizing(gDevice, new System.ComponentModel.CancelEventArgs());
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                isDeviceLost = false;
            }

            if (isActive && gDevice != null)
            {
                try
                {
                    // store current RT & DS
                    Surface rt0 = gDevice.GetRenderTarget(0);
                    Surface ds0 = gDevice.DepthStencilSurface;
                    // create texture & new DS
                    if (resolution == Size.Empty)
                        resolution = new Size(rt0.Description.Width, rt0.Description.Height);
                    Texture rtt1 = new Texture(gDevice, resolution.Width, resolution.Height, 1, Usage.RenderTarget,
                                               rt0.Description.Format, Pool.Default);
                    gDevice.SetRenderTarget(0, rtt1.GetSurfaceLevel(0));

                    Surface ds1 = gDevice.CreateDepthStencilSurface(rt0.Description.Width, rt0.Description.Height,
                                                                    (DepthFormat)ds0.Description.Format,
                                                                    ds0.Description.MultiSampleType,
                                                                    ds0.Description.MultiSampleQuality, true);
                    gDevice.DepthStencilSurface = ds1;

                    Render();

                    gDevice.SetRenderTarget(0, rt0);
                    gDevice.DepthStencilSurface = ds0;

                    return rtt1;
                }
                catch (DeviceLostException)
                {
                    isDeviceLost = true;
                }
            }
            return null;
        }

        public void Render(object param)
        {
            if (isDeviceLost)
            {
                try
                {
                    // Test the cooperative level to see if it's okay to render
                    gDevice.TestCooperativeLevel();
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
                        gDevice.Reset(presentParams);
                    }
                    catch (Exception) { return; }

                    try
                    {
                        DeviceResizing(gDevice, new System.ComponentModel.CancelEventArgs());
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                isDeviceLost = false;
            }

            if (isActive && gDevice != null)
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

        public void Dispose()
        {
            isActive = false;
            //isValid = false;

            if (gDevice != null)
            {
                gDevice.Dispose();
                gDevice = null;
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

        public virtual void OnResize(int width, int height)
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

        public bool OnLayerMouseDown(MouseEventArgs e)
        {
            if (layerMouseOverItem != null)
            {
                // use existing found item
                if (layerMouseOverItem.WantMouseClicks)
                    layerMouseOverItem.OnMouseDown(e);
                layerMouseDownItem = layerMouseOverItem;
                return true;
            }
            else
            {
                // find new item (that does not want mouseover)
                // try each layer from top to bottom
                foreach (ILayer layer in layers)
                {
                    if (layer.Enabled && layer is SimpleGUILayer)
                    {
                        SimpleGUILayer guiLayer = (SimpleGUILayer)layer;
                        GUILayerItem item;
                        if ((item = guiLayer.TraceCollisionPointer(e.Location)) != null && item.Visible/* && item.Enabled*/)
                        {
                            if (item.WantMouseClicks)
                            {
                                item.OnMouseDown(e);
                                layerMouseDownItem = item;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool OnLayerMouseUp(MouseEventArgs e)
        {
            // find new item (that does not want mouseover)
            // try each layer from top to bottom
            foreach (ILayer layer in layers)
            {
                if (layer.Enabled && layer is SimpleGUILayer)
                {
                    SimpleGUILayer guiLayer = (SimpleGUILayer)layer;
                    GUILayerItem item;
                    if ((item = guiLayer.TraceCollisionPointer(e.Location)) != null && item.Visible/* && item.Enabled*/)
                    {
                        if (item == layerMouseDownItem)
                            layerMouseDownItem.OnMouseUp(e);
                        else if (layerMouseDownItem != null)
                            layerMouseDownItem.OnMouseUp(null);
                        layerMouseDownItem = null;
                        return true;
                    }
                }
            }
            if (layerMouseOverItem != null)
                layerMouseOverItem.OnMouseUp(e);
            return false;
        }

        public bool OnLayerMouseMove(MouseEventArgs e)
        {
            // TODO: Needs to use persistent markers for mouse pos in each layer
            // try each layer from top to bottom
            foreach (ILayer layer in layers)
            {
                if (layer.Enabled && layer is SimpleGUILayer)
                {
                    SimpleGUILayer guiLayer = (SimpleGUILayer)layer;
                    GUILayerItem item;
                    if ((item = guiLayer.TraceCollisionPointer(e.Location)) != null && item.Visible/* && item.Enabled*/)
                    {
                        if (item.WantMouseOver && layerMouseOverItem != item)
                        {
                            item.OnMouseEnter();
                            if (layerMouseOverItem != null && layerMouseOverItem.WantMouseOver)
                                layerMouseOverItem.OnMouseLeave();
                            layerMouseOverItem = item;
                        }
                        return true;
                    }
                }
            }
            if (layerMouseOverItem != null)
            {
                layerMouseOverItem.OnMouseLeave();
                layerMouseOverItem = null;
                return true;
            }
            return false;
        }

        public void Rotate(Matrix rotation, float zoomLevel)
        {
            this.rotation = rotation;
            this.zoomLevel = zoomLevel;
        }

        public DeviceInterface DevIf
        {
            get { return devIf; }
        }

        public SceneEntity TrySelect(int x, int y, out int internalValue)
        {
            Vector3 pickRay, pickRayDir, pickRayOrigin;

            // Compute the vector of the pick ray in screen space
            pickRay.X = (((2.0f * x) / width) - 1) / view3D.Projection.M11;
            pickRay.Y = -(((2.0f * y) / height) - 1) / view3D.Projection.M22;
            pickRay.Z = 1.0f;

            Matrix matInverseView = Matrix.Invert(view3D.ViewMatrix);

            // Transform the screen space pick ray into 3D space
            pickRayDir.X = pickRay.X * matInverseView.M11 + pickRay.Y * matInverseView.M21 + pickRay.Z * matInverseView.M31;
            pickRayDir.Y = pickRay.X * matInverseView.M12 + pickRay.Y * matInverseView.M22 + pickRay.Z * matInverseView.M32;
            pickRayDir.Z = pickRay.X * matInverseView.M13 + pickRay.Y * matInverseView.M23 + pickRay.Z * matInverseView.M33;
            pickRayDir.Normalize();

            pickRayOrigin.X = matInverseView.M41;
            pickRayOrigin.Y = matInverseView.M42;
            pickRayOrigin.Z = matInverseView.M43;

            pickRayOrigin.Add(Vector3.Multiply(pickRayDir, 1.0f));	//	near plane

            // transform trough world space
            Matrix inverseWorld = Matrix.Invert(view3D.WorldMatrix * rotation); // scene-world == object-world

            Vector3 localOrigin = Vector3.TransformCoordinate(pickRayOrigin, inverseWorld);
            Vector3 localDir = Vector3.TransformNormal(pickRayDir, inverseWorld);

            // trace through scene via manager
            return sManager.TraceRay(localOrigin, localDir, out internalValue);
        }

        public GenericSceneManager<SceneEntity> SceneManager
        {
            get { return sManager; }
        }
    }
}