using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Genetibase.NuGenRenderCore.Settings;
using Genetibase.VisUI.Rendering;
using Genetibase.VisUI.UI;
using Microsoft.DirectX.Direct3D;
using NuGenVisUI;

namespace Genetibase.VisUI.Controls
{
    public partial class UI3DControl : UserControl, IUI3DControl
    {
        protected readonly Thread renderingThread;
        protected bool renderFrame;
        bool rendering;
        readonly object renderLockObj = "";
        float renderTargetFPS = 20.0f;
        bool isRenderingFPS = false;

        RenderUpdateDelegate onRenderUpdate;
        double lastRenderTime;

        private ControlMode controlMode = ControlMode.ViewMovement;
        protected ControlStatus controlStatus = ControlStatus.Loading;
        private string title;
        private HashTableSettings settings;
        private bool lowDetailMovement;

        protected RenderingContext3D renderContext;
        protected RenderingSource3D renderSource;

        readonly OrbitalMovementControler viewController;

        public enum Axis
        {
            X, Y, Z
        }

        protected GraphicsProfile[] profiles;
        protected GraphicsProfile[] supportedProfiles;

        protected readonly List<ScreenVisLayer> screenLayers;
        protected readonly List<GeometryVisLayer> geometryLayers;

        List<SceneEntity> selectedEntities;

        public UI3DControl()
        {
            InitializeComponent();

            viewController = new OrbitalMovementControler();

            renderingThread = new Thread(this.RenderProcess);

            DoubleBuffered = false;

            screenLayers = new List<ScreenVisLayer>();
            geometryLayers = new List<GeometryVisLayer>();

            selectedEntities = new List<SceneEntity>();
        }

        #region IUI3DControl Members

        public event EntitySelectionDelegate OnEntitySelected;

        public ControlMode ControlMode
        {
            get { return controlMode; }
            set { controlMode = value; }
        }

        public ControlStatus Status
        {
            get { return controlStatus; }
        }

        public virtual void Init(HashTableSettings settings, ICommonDeviceInterface cdi)
        {
            this.settings = settings;
            
            // load settings
            BackColor = (Color)settings["View3D.BgClr"];

            // filter profiles into supported
            // NOTE: Profiles should be done in CDI really
            List<GraphicsProfile> sProfiles = new List<GraphicsProfile>();
            foreach (GraphicsProfile profile in profiles)
            {
                if (GraphicsDeviceManager.CheckAdapterMeetsRequirements(((CommonDeviceInterface)cdi).Adapter,
                                                                        profile.MinReqs))
                {
                    sProfiles.Add(profile);
                    // decide which recommendation to use
                    profile.RecommendedVarInUse = -1;
                    for (int i = 0; i < profile.RecommendedVariations.Length; i++)
                    {
                        if (GraphicsDeviceManager.CheckAdapterMeetsRequirements(((CommonDeviceInterface)cdi).Adapter,
                                                                                profile.RecommendedVariations[i]))
                        {
                            profile.RecommendedVarInUse = i;
                            break;
                        }
                    }
                }
            }
            // TODO: This needs to feed back to UI?
            supportedProfiles = sProfiles.ToArray();
            if (supportedProfiles.Length == 0)
                throw new Exception("None of the available graphics profiles meet the graphics devices capabilities");
        }

        public string Title
        {
            get { return title; }
            set { title = value;  }
        }

        public HashTableSettings Settings
        {
            get { return settings; }
        }

        public void TakeScreenshot(string file, ImageFileFormat format)
        {
            lock (renderLockObj)
            {
                rendering = true;
            }

            Texture tex = renderContext.RenderToTexture(null, Size.Empty);
            TextureLoader.Save(file, format, tex);

            lock (renderLockObj)
            {
                rendering = false;
            }
        }

        public void TakeScreenshot(ScreenshotSettings settings)
        {
            if (settings.Destination == ScreenshotSettings.OutputDestination.File)
            {
                lock (renderLockObj)
                {
                    rendering = true;
                }

                Texture tex = renderContext.RenderToTexture(null, settings.Resolution);
                TextureLoader.Save(settings.File, settings.Format, tex);

                lock (renderLockObj)
                {
                    rendering = false;
                }
            }
        }

        public bool LowDetailMovement
        {
            get { return lowDetailMovement; }
            set { lowDetailMovement = value; }
        }

        public double LastRenderTime
        {
            get { return lastRenderTime; }
        }

        public RenderUpdateDelegate OnRenderUpdate
        {
            get { return onRenderUpdate; }
            set { onRenderUpdate = value; }
        }

        public LayerInfo[] GetLayersInfo()
        {
            if (renderContext != null)
            {
                LayerInfo[] layersInfo = new LayerInfo[renderContext.Layers.LayerCount];
                int idx = 0;
                foreach (ILayer layer in renderContext.Layers)
                {
                	layersInfo[idx] = new LayerInfo();
                    layersInfo[idx].Size = layer.Dimensions;
                    idx++;
                }
                return layersInfo;
            }
            return null;
        }
        #endregion

        #region Events
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (renderContext != null)
                Render();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        { }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (renderContext != null && controlStatus != ControlStatus.Loading)
            {
                // pass to layers first
                if (renderContext.OnLayerMouseDown(e))
                {
                    Render();
                    return;
                }
                if (controlMode == ControlMode.ViewMovement)
                {
                    if (viewController != null && viewController.OnMouseDown(e))
                        Render();
                    else
                    {
                        // perform selection attempt
                        int internalValue;
                        EntitySelected(renderContext.TrySelect(e.X, e.Y, out internalValue), false, internalValue);
                        Render();
                    }
                }
                else if (controlMode == ControlMode.Selection)
                {
                    // Start selection area
                    //renderContext.StartSelectionArea(e.X, e.Y);
                }
                else if (controlMode == ControlMode.SelectionMovement)
                {
                    // start movement of the selected items
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (viewController != null && renderContext != null && controlStatus != ControlStatus.Loading)
            {
                if (renderContext.OnLayerMouseUp(e))
                {
                    Render();
                    return;
                }
                if (controlMode == ControlMode.Selection)
                {
                }
                else if (controlMode == ControlMode.ViewMovement)
                {
                    if (viewController.OnMouseUp(e))
                        Render();
                }
            }
        }

        private void EntitySelected(SceneEntity entity, bool add, int internalValue)
        {
            // clear old selected if req
            if (!add)
            {
                foreach (SceneEntity sceneEntity in selectedEntities)
                {
                    sceneEntity.Selected = false;
                    sceneEntity.InternalSelectedValue = -1;
                }
                selectedEntities.Clear();
            }
            if (entity != null)
            {
                selectedEntities.Add(entity);

                // toggle entity as selected
                entity.Selected = true;
                entity.InternalSelectedValue = internalValue;
            }
            // pass to UI as event
            if (OnEntitySelected != null)
                OnEntitySelected(new IEntity[] { entity }, false);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (viewController != null && renderContext != null && controlStatus != ControlStatus.Loading)
            {
                // pass to layers first
                if (renderContext.OnLayerMouseMove(e))
                {
                    Render();
                    return;
                }
                if (controlMode == ControlMode.Selection && e.Button == MouseButtons.Left)
                {
                    //renderContext.UpdateSelectionArea(e.X, e.Y);
                    Render();
                }
                else if (controlMode == ControlMode.ViewMovement)
                {
                    if (viewController.OnMouseMove(e))
                        Render();
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (viewController != null && renderContext != null && controlStatus != ControlStatus.Loading && viewController.OnMouseWheel(e))
                Render();
        }
        #endregion

        public void Render()
        {
            if (renderingThread.ThreadState != ThreadState.Unstarted)
            {
                lock (renderLockObj)
                {
                    renderFrame = true;
                    if (!rendering)
                        renderingThread.Interrupt();
                }
            }
        }

        private void RenderProcess()
        {
            try
            {
                if (isRenderingFPS)
                {
                    rendering = true;
                    int ticksPerFrame = (int)(1000f / renderTargetFPS);

                    int startFrame = 0;
                    //float rotAngle = 90f / renderTargetFPS;
                    //Matrix rot = Matrix.RotationX(0);
                    while (true)
                    {
                        startFrame = Environment.TickCount;

                        // render view
                        /*renderContext.Rotate(rot, 0);*/
                        renderContext.Render(null);

                        //rot.RotateX(rotAngle);

                        int frameTime = Environment.TickCount - startFrame;
                        if (frameTime < ticksPerFrame)
                            Thread.Sleep(ticksPerFrame - frameTime);
                    }
                }
                else
                {
                    if (renderContext != null)
                    {
                        lock (renderLockObj)
                        {
                            renderContext.OnResize(Width, Height);
                        }
                    }
                    // keep working & waiting for work until aborted
                    while (true)
                    {
                        try
                        {
                            // render frames until no need to
                            while (true)
                            {
                                lock (renderLockObj)
                                {
                                    if (renderFrame)
                                    {
                                        rendering = true;
                                        renderFrame = false;
                                    }
                                    else
                                        break;
                                }
                                // render frame
                                DateTime start = DateTime.Now;

                                renderContext.Rotate(viewController.Rotation3D, viewController.ZoomLevel);
                                renderContext.Render(null);

                                TimeSpan time = DateTime.Now - start;
                                lastRenderTime = time.TotalMilliseconds;
                                if (onRenderUpdate != null)
                                    BeginInvoke(onRenderUpdate, lastRenderTime);

                                lock (renderLockObj)
                                {
                                    rendering = false;
                                }
                            }
                            // wait for work
                            Thread.Sleep(Timeout.Infinite);
                        }
                        catch (ThreadInterruptedException) { }
                    }
                }
            }
            catch (ThreadAbortException) { }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (renderContext != null)
            {
                lock (renderLockObj)
                {
                    renderContext.OnResize(Width, Height);
                }
            }
        }

        public void ToggleAxis(Axis axis, bool on)
        {
            switch (axis)
            {
                case Axis.X:
                    viewController.LockXAxis(on);
                    break;
                case Axis.Y:
                    viewController.LockYAxis(on);
                    break;
                case Axis.Z:
                    viewController.LockZAxis(on);
                    break;
            }
        }

        public GraphicsProfile CurrentProfile
        {
            get { return supportedProfiles[0]; }
        }

        public ScreenVisLayer[] ScreenLayers
        {
            get { return screenLayers.ToArray(); }
        }

        public GeometryVisLayer[] GeometryLayers
        {
            get { return geometryLayers.ToArray(); }
        }

        public event SceneEntityUpdate OnSceneModified;

        public SceneEntity[] GetSceneEntities()
        {
            SceneEntity[] entities = new SceneEntity[renderContext.SceneManager.sceneGraphEntities.Count];
            int index = 0;
            foreach (KeyValuePair<uint, SceneEntity> item in renderContext.SceneManager.sceneGraphEntities)
            {
                entities[index++] = item.Value;
            }
            return entities;
        }
    }
}