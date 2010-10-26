using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Logging;
using NuGenSVisualLib.Recording;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Layers;
using NuGenSVisualLib.Rendering.Layers.Chem;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.ThreeD.DX9;
using NuGenSVisualLib.Settings;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering.ThreeD
{
    /// <summary>
    /// Encapsulates a 3D rendering platform using DX9 in a chemistry context
    /// </summary>
    class ChemRenderingContext3DDX9 : RenderingContext3DDX9, IChemRenderingContext, IRecordingRenderer
    {
        protected ChemRenderingSource3D chemRenderingSource;
        protected CompleteOutputDescription outputDescription;
        ISettings settings;
        Axis3D axis;
        Matrix rotation;
        bool drawAxis;

        GraphicsPipeline3D pipeline;

        bool recording;

        protected internal int numBonds;
        protected internal int numAtoms;
        
        ShapeStructures visibleStructures;
        ShapeStructures availableStructures;

        MoleculeSceneManager sceneManager;

        Thread loadingThread;
//        LoadingGUILayer loadingLayer;

        bool drawSelectionLayer;
        Line selectionLine;
        Vector2[] selectionArea;

        List<AtomSelectionEntity> selectedAtoms;

        internal IBond[] bonds = null;
        internal IAtom[] atoms = null;

        enum Status
        {
            Idle,
            Loading,
            Transition,
            Rendering
        }

        Status cStatus;

        private readonly List<IEntity> modeEntities;
        MoleculeControlLayer molControlLayer;

        GUILayerItem layerMouseOverItem;
        GUILayerItem layerMouseDownItem;

        public ChemRenderingContext3DDX9(ISettings settings, Control targetRenderArea,
                                         CommonDeviceInterface cdi)
            : base(targetRenderArea, GetOutputRequirements(),
                  new OutputRequirements(MultiSampleType.None, DeviceType.Software, Format.X8R8G8B8, true, DepthFormat.D16, false, null),
                  cdi)
        {
            this.settings = settings;
            
            axis = new Axis3D();
            axis.Init(device);
            rotation = Matrix.Identity;

            cStatus = Status.Idle;

            //loadingLayer = new LoadingGUILayer(devIf, 100, 100);
            //loadingLayer.LoadResources();

            selectedAtoms = new List<AtomSelectionEntity>();
            modeEntities = new List<IEntity>();
        }

        private static OutputRequirements GetOutputRequirements()
        {
            return new OutputRequirements(MultiSampleType.FourSamples, DeviceType.Hardware,
                                          Format.X8R8G8B8, true, DepthFormat.D16, true, null);
        }

        #region Properties
        
        public override IRenderingSource RenderSource
        {
            get { return renderSource; }
            set
            {
                if (value is ChemRenderingSource3D)
                    base.RenderSource = chemRenderingSource = (ChemRenderingSource3D)value;
            }
        }

        public ChemRenderingSource3D ChemRenderSource
        {
            get { return chemRenderingSource; }
            set
            {
                RenderSource3D = chemRenderingSource = value;

                // count atoms & bonds
                numBonds = numAtoms = 0;
                foreach (IChemSequence sequence in chemRenderingSource.ChemFile.ChemSequences)
                {
                    foreach (IChemModel model in sequence.ChemModels)
                    {
                        if (model.SetOfMolecules != null)
                        {
                            foreach (IMolecule molecule in model.SetOfMolecules.Molecules)
                            {
                                numBonds += molecule.Bonds.Length;
                                numAtoms += molecule.AtomCount;
                            }
                        }
                    }
                }

                RefreshDescription();
            }
        }

        public CompleteOutputDescription OutputDescription
        {
            get { return outputDescription; }
        }

        public Device Device
        {
            get { return device; }
        }

        public bool DrawAxis
        {
            get { return drawAxis; }
            set { drawAxis = value; }
        }

        public bool Recording
        {
            get { return recording; }
            set
            {
                recording = value;
                // TODO: Set refresh thread up
            }
        }
        #endregion

        public void ApplyOutputDescription(CompleteOutputDescription desc)
        {
            desc.CompareAgainst(desc, true);
            outputDescription = desc;
            
            /*if (desc.GeneralShadingDesc.AntiAliasing > 0)
                presentParams.MultiSample = MultiSampleType.FourSamples;
            else
                presentParams.MultiSample = MultiSampleType.None;*/

            try
            {
                device.Reset(presentParams);
            }
            catch (Exception e)
            {
                log.AddItem(new LogItem(string.Format("Device failed to reset with pParams={0},{1},{2},{3},{4},{5},{6}: Exception is - {7}",
                                                      presentParams.AutoDepthStencilFormat,
                                                      presentParams.BackBufferFormat,
                                                      presentParams.BackBufferHeight,
                                                      presentParams.BackBufferWidth,
                                                      presentParams.EnableAutoDepthStencil,
                                                      presentParams.MultiSample,
                                                      presentParams.SwapEffect,
                                                      e.Message),
                                        LogItem.ItemLevel.Error));
                throw;
            }

            sceneManager.SetScheme(desc.SchemeSettings.GetScheme(device));
            sceneManager.SetOutputDesc(desc);

            if (desc.GeneralShadingDesc.Wireframe)
                device.RenderState.FillMode = FillMode.WireFrame;
            else
                device.RenderState.FillMode = FillMode.Solid;

            switch (desc.GeneralShadingDesc.ShadingMode)
            {
                case NuGenSVisualLib.Rendering.Shading.GeneralShadingDesc.ShadingModes.Smooth:
                    device.RenderState.ShadeMode = ShadeMode.Phong;
                    break;
                case NuGenSVisualLib.Rendering.Shading.GeneralShadingDesc.ShadingModes.Flat:
                    device.RenderState.ShadeMode = ShadeMode.Flat;
                    break;
            }

            RefreshDescription();
        }

        private void ReLoadContentProcess()
        {
            //loadingLayer.Reset();
            cStatus = Status.Loading;
            Render();

            // collect data
            bonds = null;
            atoms = null;
            if (numAtoms > 0)
            {
                atoms = new IAtom[numAtoms];

                int atomIdx = 0;
                foreach (IChemSequence sequence in chemRenderingSource.ChemFile.ChemSequences)
                {
                    foreach (IChemModel model in sequence.ChemModels)
                    {
                        if (model.SetOfMolecules != null)
                        {
                            foreach (IMolecule molecule in model.SetOfMolecules.Molecules)
                            {
                                Array.Copy(molecule.Atoms, 0, atoms, atomIdx, molecule.Atoms.Length);
                                atomIdx += molecule.Atoms.Length;
                            }
                        }
                    }
                }
            }
            if (numBonds > 0)
            {
                bonds = new IBond[numBonds];

                int bondsIdx = 0;
                foreach (IChemSequence sequence in chemRenderingSource.ChemFile.ChemSequences)
                {
                    foreach (IChemModel model in sequence.ChemModels)
                    {
                        if (model.SetOfMolecules != null)
                        {
                            foreach (IMolecule molecule in model.SetOfMolecules.Molecules)
                            {
                                Array.Copy(molecule.Bonds, 0, bonds, bondsIdx, molecule.Bonds.Length);
                                bondsIdx += molecule.Bonds.Length;
                            }
                        }
                    }
                }
            }

            if (atoms != null || bonds != null)
                sceneManager.OnNewDataSource(atoms, bonds, renderSource3D.Origin, renderSource3D.Bounds);

            /*loadingLayer.LoadedGeometry = true;
            loadingLayer.LoadedEffects = true;

            cStatus = Status.Transition;
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddSeconds(1);
            float scale = 255f / 1000f;
            while (DateTime.Now < end)
            {
                Render();
                TimeSpan ts = end.Subtract(DateTime.Now);
                loadingLayer.Alpha = (int)(ts.TotalMilliseconds * scale);
            }*/
            cStatus = Status.Rendering;
            Render();
        }

        private void RefreshDescription()
        {
            // move to thread
            if (loadingThread == null ||
                loadingThread.ThreadState == ThreadState.Unstarted ||
                loadingThread.ThreadState != ThreadState.Running)
            {
                loadingThread = new Thread(this.ReLoadContentProcess);
                loadingThread.Start();
            }
        }

        protected override void Render()
        {
            Render(0, device.GetRenderTarget(0));
        }

        public void Render(int index, Surface renderTarget)
        {
            //Surface bb = device.GetRenderTarget(0);
            lock (device)
            {
                //device.SetRenderTarget(index, renderTarget);
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, background, 1.0f, 0);

                if (cStatus == Status.Rendering || cStatus == Status.Transition)
                {
                    // setup view
                    pipeline.ProjectionMatrix = view3D.Projection;
                    pipeline.ViewMatrix = view3D.ViewMatrix;
                    pipeline.WorldMatrix = view3D.WorldMatrix * rotation;

                    device.RenderState.Lighting = false;

                    if (renderSource != null)
                    {
                        Surface bb = device.GetBackBuffer(0, 0, BackBufferType.Mono);
                        sceneManager.RenderSceneFrame(pipeline, bb.Description.Width, bb.Description.Height);
                    }

                    //if (cStatus == Status.Transition)
                    //    loadingLayer.RenderLayer(pipeline, renderTarget.Description.Width, renderTarget.Description.Height);

                    // draw layers
                    if (layers.LayerCount > 0)
                    {
                        device.RenderState.ZBufferEnable = false;
                        foreach (ILayer layer in layers)
                        {
                            //if (layer.Visible)
                                layer.Draw();
                        }
                        device.RenderState.ZBufferEnable = true;
                    }
                }
                //else if (cStatus == Status.Loading)
                //{
                //    loadingLayer.RenderLayer(pipeline, renderTarget.Description.Width, renderTarget.Description.Height);
                //}

                if (drawSelectionLayer)
                {
                    // draw selection layer
                    selectionLine.Begin();
                    selectionLine.Draw(selectionArea, Color.Blue);
                    selectionLine.End();
                }

                device.Present();
            }
            //device.SetRenderTarget(0, bb);
        }

        public override void Dispose()
        {
            //if (loadingLayer != null)
            //    loadingLayer.Dispose();
            //if (atomRenderer != null)
            //    atomRenderer.Dispose();
        }

        public void Rotate(Matrix rot, float zoomLevel)
        {
            rotation = rot;
            view3D.Zoom = zoomLevel;
            view3D.UpdateView(renderSource3D);
            //this.outputDescription.GeneralLightingDesc.UpdateLightDirections(device, rotation);
        }

        protected override void InitializeEnvironment(CommonDeviceInterface cdi)
        {
            base.InitializeEnvironment(cdi);

            pipeline = new GraphicsFixedPipeline3D(device);//new GraphicsProgramablePipeline3D(device);

            ChemSymbolTextures.GraphicsDevice = device;

            sceneManager = new MoleculeSceneManager(device, outSettings);

            selectionLine = new Line(device);
            selectionLine.Antialias = false;
            selectionArea = new Vector2[5];

            molControlLayer = new MoleculeControlLayer(devIf, new Point(), new Size());
            molControlLayer.Show(false);
            molControlLayer.LoadResources();
            layers.InsertLayer(molControlLayer, uint.MaxValue);
        }

        public enum ShapeStructures
        {
            Ribbons     = 1,
            Cartoons    = 2
        }

        public ShapeStructures VisibleStructures
        {
            get { return visibleStructures; }
            set { visibleStructures = value; }
        }

        public ShapeStructures AvailableStructures
        {
            get { return availableStructures; }
            set { availableStructures = value; }
        }

        protected override void DeviceReset(Object sender, EventArgs e)
        {
            //if (effect != null)
            //    effect.OnReset();
        }

        public ChemEntity TrySelectAtPoint(int x, int y)
        {
            Vector3 pickRay, pickRayDir, pickRayOrigin;

            // Compute the vector of the pick ray in screen space
            pickRay.X = (((2.0f * x) / width) - 1) / view3D.Projection.M11;
            pickRay.Y = -(((2.0f * y) / height) - 1) / view3D.Projection.M22;
            pickRay.Z = 1.0f;

            Matrix matInverseView = Matrix.Invert(view3D.ViewMatrix);

            // Transform the screen space pick ray into 3D space
	        pickRayDir.X  = pickRay.X * matInverseView.M11 + pickRay.Y * matInverseView.M21 + pickRay.Z * matInverseView.M31;
	        pickRayDir.Y  = pickRay.X * matInverseView.M12 + pickRay.Y * matInverseView.M22 + pickRay.Z * matInverseView.M32;
	        pickRayDir.Z  = pickRay.X * matInverseView.M13 + pickRay.Y * matInverseView.M23 + pickRay.Z * matInverseView.M33;
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
            ChemEntity entity = sceneManager.TraceRayInScene(localOrigin, localDir);
            return entity;
        }

        public void StartSelectionArea(int x, int y)
        {
            selectionArea[0] = new Vector2(x, y);
            selectionArea[1] = new Vector2(x, y);
            selectionArea[2] = new Vector2(x, y);
            selectionArea[3] = new Vector2(x, y);
            selectionArea[4] = new Vector2(x, y);

            drawSelectionLayer = true;
        }

        public void UpdateSelectionArea(int x, int y)
        {
            selectionArea[1].X = x;
            selectionArea[2] = new Vector2(x, y);
            selectionArea[3].Y = y;
        }

        public void EndSelectionArea(int x, int y)
        {
            drawSelectionLayer = false;

            // process selection
            sceneManager.TraceFrustumInScene(pipeline.ViewMatrix * pipeline.ProjectionMatrix);
        }

        public void UpdateSelectedItems(List<ChemEntity> selected)
        {
            // ensure selection entities exist in scene for the selected objects
            if (selected == null || selected.Count == 0)
            {
                // remove any entities from scene
                sceneManager.RemoveFromPostSceneView(selectedAtoms);
                selectedAtoms.Clear();
            }
            else
            {
                bool[] inUse = new bool[selectedAtoms.Count];
                List<AtomSelectionEntity> newSelectedAtoms = new List<AtomSelectionEntity>();
                foreach (ChemEntity obj in selected)
                {
                    if (obj is AtomEntity)
                    {
                        // see if already in list
                        int idx = 0;
                        bool found = false;
                        foreach (AtomSelectionEntity entity in selectedAtoms)
                        {
                            if (entity.Atom.Atom == obj.CdkObject)
                            {
                                inUse[idx] = true;
                                newSelectedAtoms.Add(entity);
                                found = true;
                                break;
                            }
                            idx++;
                        }
                        if (!found)
                        {
                            AtomSelectionEntity entity = new AtomSelectionEntity((AtomEntity)obj);
                            entity.Init(device);
                            newSelectedAtoms.Add(entity);
                            sceneManager.AddToPostSceneView(entity);
                        }
                    }
                }
                sceneManager.RemoveFromPostSceneView(selectedAtoms, inUse);
                selectedAtoms.Clear();
                selectedAtoms = newSelectedAtoms;
            }
        }

        public void AddSelectionMovementItems()
        {
            // add bounding box to post scene
            // determine bounds
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (AtomSelectionEntity item in selectedAtoms)
            {
                if (item.Atom.BoundingBox.Position.X < min.X)
                    min.X = item.Atom.BoundingBox.Position.X;
                if (item.Atom.BoundingBox.Position.X + item.Atom.BoundingBox.Extent.X > max.X)
                    max.X = item.Atom.BoundingBox.Position.X + item.Atom.BoundingBox.Extent.X;

                if (item.Atom.BoundingBox.Position.Y < min.Y)
                    min.Y = item.Atom.BoundingBox.Position.Y;
                if (item.Atom.BoundingBox.Position.Y + item.Atom.BoundingBox.Extent.Y > max.Y)
                    max.Y = item.Atom.BoundingBox.Position.Y + item.Atom.BoundingBox.Extent.Y;

                if (item.Atom.BoundingBox.Position.Z < min.Z)
                    min.Z = item.Atom.BoundingBox.Position.Z;
                if (item.Atom.BoundingBox.Position.Z + item.Atom.BoundingBox.Extent.Z > max.Z)
                    max.Z = item.Atom.BoundingBox.Position.Z + item.Atom.BoundingBox.Extent.Z;
            }
            BoundingBoxEntity bbox = new BoundingBoxEntity(new BoundingBox(min, max), true, Color.DarkTurquoise.ToArgb());
            bbox.Init(device);
            modeEntities.Add(bbox);
            sceneManager.AddToPostScene(bbox);

            // show movement layer
            if (molControlLayer != null)
                molControlLayer.Show(true);
            Render();
        }

        public void RemoveSelectionMovementItems()
        {
            sceneManager.RemoveFromPostScene(modeEntities);
            if (molControlLayer != null)
                molControlLayer.Show(false);
            Render();
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
                        if ((item = guiLayer.TraceCollisionPointer(e.Location)) != null && item.Visible && item.Enabled)
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
                    if ((item = guiLayer.TraceCollisionPointer(e.Location)) != null && item.Visible && item.Enabled)
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
                    if ((item = guiLayer.TraceCollisionPointer(e.Location)) != null && item.Visible && item.Enabled)
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
    }
}