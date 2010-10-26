using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.DirectX;
using NuGenSVisualLib.Chem;
using NuGenSVisualLib.Exceptions;
using NuGenSVisualLib.Movement.ThreeD;
using NuGenSVisualLib.Recording;
using NuGenSVisualLib.Rendering.Chem.Materials;
using NuGenSVisualLib.Rendering.Chem.Schemes;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.ThreeD;
using NuGenSVisualLib.Settings;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering.Chem
{
    /// <summary>
    /// Encapsulates a 3D view control
    /// </summary>
    public partial class Chem3DControl : UserControl, IChemControl
    {
        ChemRenderingContext3DDX9 renderContext;
        private ChemRenderingSource3D renderSource;
        CompleteOutputDescription outputDesc;
        List<MoleculeMaterialsModule> modules;
        HashTableSettings settings;
        string outputSettingsXmlFile;

        private bool editMoleculeEnabled;

        MovementControlHandler3D controlHandler;

        RecordingSettings rSettings;

        Thread renderingThread;
        bool renderFrame;
        bool rendering;
        object renderLockObj = "";
        float renderTargetFPS = 20.0f;
        bool isRenderingFPS = false;

        public delegate void ChemObjectDelegate(ChemEntity entity);
        public event ChemObjectDelegate OnEntitySelected;

        RenderUpdateDelegate onRenderUpdate;
        double lastRenderTime;

        ControlMode controlMode = ControlMode.ViewMovement;

        List<ChemEntity> selected;

        string moleculeSMILESRawString;

        public Chem3DControl()
        {
            InitializeComponent();

            controlHandler = new MovementControlHandler3D();
            modules = new List<MoleculeMaterialsModule>();
            modules.Add(new MoleculeDefaultMaterials());
            modules.Add(new MoleculeRandomTemplate());
            modules.Add(new MoleculeAtomicNumberTemplate());

            renderingThread = new Thread(this.RenderProcess);

            DoubleBuffered = false;

            selected = new List<ChemEntity>();
        }

        #region Rendering Code

        private void RenderProcess()
        {
            try
            {
                if (isRenderingFPS)
                {
                    rendering = true;
                    int ticksPerFrame = (int)(1000f / renderTargetFPS);

                    int startFrame = 0;
                    float rotAngle = 90f / renderTargetFPS;
                    Matrix rot = Matrix.RotationX(0);
                    while (true)
                    {
                        startFrame = Environment.TickCount;

                        renderContext.Rotate(rot, 0);
                        renderContext.Render(null);

                        rot.RotateX(rotAngle);

                        int frameTime = Environment.TickCount - startFrame;
                        if (frameTime < ticksPerFrame)
                            Thread.Sleep(ticksPerFrame - frameTime);
                    }
                }
                else
                {
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

                                renderContext.Rotate(controlHandler.Rotation3D, controlHandler.ZoomLevel);
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
        #endregion

        private void LoadOutputSettings()
        {
            outputDesc = CompleteOutputDescription.LoadDescription(outputSettingsXmlFile);
            outputDesc.SchemeSettings = new BallAndStickSchemeSettings();
            ApplySettings(outputDesc);
        }

        public void CloseFile()
        {
            if (renderSource != null)
            {
                renderSource = null;
                if (renderContext != null)
                {
                    renderContext.ChemRenderSource = null;
                    renderContext.Render(null);
                }
            }
        }

        private void OpenSupportedFormatsDlg()
        {
            SupportedFormatsInfoDlg dlg = new SupportedFormatsInfoDlg();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        #region Properties

        public string Title
        {
            get
            {
                if (renderSource != null)
                    return renderSource.ChemFile.ID;
                return "";
            }
        }

        public CompleteOutputDescription OutputDescription
        {
            get { return outputDesc; }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                if (renderContext != null)
                    renderContext.BackColor = value;
            }
        }

        public string OutputSettingsXmlFile
        {
            get { return outputSettingsXmlFile; }
            set
            {
                outputSettingsXmlFile = value;
                if (value != null)
                    LoadOutputSettings();
            }
        }

        public bool MouseControlEnabled
        {
            get { return controlHandler.Enabled; }
            set { controlHandler.Enabled = value; }
        }

        public bool EditMoleculeEnabled
        {
            get { return editMoleculeEnabled; }
            set
            {
                editMoleculeEnabled = value;

                //if (!editMoleculeEnabled)
                //    controlHandler
            }
        }

        public bool RibbonsVisible
        {
            get
            {
                return (renderContext.VisibleStructures & ChemRenderingContext3DDX9.ShapeStructures.Ribbons) != 0;
            }
            set
            {
                // check if available
                if ((renderContext.AvailableStructures & ChemRenderingContext3DDX9.ShapeStructures.Ribbons) != 0)
                {
                    if (value)
                        renderContext.VisibleStructures = renderContext.VisibleStructures | ChemRenderingContext3DDX9.ShapeStructures.Ribbons;
                    else
                        renderContext.VisibleStructures = renderContext.VisibleStructures ^ ChemRenderingContext3DDX9.ShapeStructures.Ribbons;
                }
            }
        }

        public bool CartoonsVisible
        {
            get
            {
                return (renderContext.VisibleStructures & ChemRenderingContext3DDX9.ShapeStructures.Cartoons) != 0;
            }
            set
            {
                // check if available
                if ((renderContext.AvailableStructures & ChemRenderingContext3DDX9.ShapeStructures.Cartoons) != 0)
                {
                    if (value)
                        renderContext.VisibleStructures = renderContext.VisibleStructures | ChemRenderingContext3DDX9.ShapeStructures.Cartoons;
                    else
                        renderContext.VisibleStructures = renderContext.VisibleStructures ^ ChemRenderingContext3DDX9.ShapeStructures.Cartoons;
                }
            }
        }

        public bool RibbonsAvailable
        {
            get
            {
                return (renderContext.AvailableStructures & ChemRenderingContext3DDX9.ShapeStructures.Ribbons) != 0;
            }
        }

        public bool CartoonsAvailable
        {
            get
            {
                return (renderContext.AvailableStructures & ChemRenderingContext3DDX9.ShapeStructures.Cartoons) != 0;
            }
        }

        public HashTableSettings Settings
        {
            get { return settings; }
        }

        #endregion

        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            if (renderContext != null)
                Render();
            //else
            //    base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (controlHandler != null && renderContext != null && renderSource != null)
            {
                // pass to layers first
                if (renderContext.OnLayerMouseMove(e))
                {
                    Render();
                    return;
                }
                if (controlMode == ControlMode.Selection && e.Button == MouseButtons.Left)
                {
                    renderContext.UpdateSelectionArea(e.X, e.Y);
                    Render();
                }
                else if (controlMode == ControlMode.ViewMovement)
                {
                    if (controlHandler.OnMouseMove(e))
                        Render();
                }
            }
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (renderContext != null && renderSource != null)
            {
                // pass to layers first
                if (renderContext.OnLayerMouseDown(e))
                {
                    Render();
                    return;
                }
                if (controlMode == ControlMode.ViewMovement)
                {
                    if (e.Button == MouseButtons.Left && e.Clicks == 2)
                    {
                        ChemEntity obj = TrySelectAtPoint(e.X, e.Y);
                        if (obj != null && OnEntitySelected != null)
                            OnEntitySelected(obj);
                    }
                    else if (controlHandler != null && controlHandler.OnMouseDown(e))
                        Render();
                }
                else if (controlMode == ControlMode.Selection)
                {
                    // Start selection area
                    renderContext.StartSelectionArea(e.X, e.Y);
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
            if (controlHandler != null && renderContext != null && renderSource != null)
            {
                if (renderContext.OnLayerMouseUp(e))
                {
                    Render();
                    return;
                }
                if (controlMode == ControlMode.Selection)
                {
                    renderContext.EndSelectionArea(e.X, e.Y);
                    Render();
                }
                else if (controlMode == ControlMode.ViewMovement)
                {
                    if (controlHandler.OnMouseUp(e))
                        Render();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (controlHandler != null && renderContext != null && renderSource != null && controlHandler.OnKeyDown(e))
                Render();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (controlHandler != null && renderContext != null && renderSource != null && controlHandler.OnKeyUp(e))
                Render();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (controlHandler != null && renderContext != null && renderSource != null && controlHandler.OnMouseWheel(e))
                Render();
        }

        #endregion

        #region IChemControl Members

        public void LoadFile(string file)
        {
            MoleculeLoadingResults results = null;
            try
            {
                renderSource = new ChemRenderingSource3D(MoleculeLoader.LoadFromFile(file, settings, MoleculeLoader.FileUsage.ThreeD, null, out results));
                if (renderContext != null)
                {
                    renderContext.ChemRenderSource = renderSource;
                    renderContext.OnResize(Width, Height);
                    renderContext.Render(null);
                }
            }
            catch (UserLevelException ule)
            {
                // post to user
                MessageBox.Show(ule.Message, "Loading File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UleDlg dlg = new UleDlg(results, null);
            dlg.ShowDialog();
            dlg.Dispose();
        }

        public void LoadFile(IChemFileWrapper file)
        {
            renderSource = new ChemRenderingSource3D(file.chemFile);
            if (renderContext != null)
            {
                renderContext.ChemRenderSource = renderSource;
                renderContext.OnResize(Width, Height);
                //renderContext.Render(null);
            }

            // load structures if available
            //if (file.results.FileFormat is PDBFormat)
            //{
            //    object frame = ChemModel.CreateJMOLFrameFromCDKFile(file.chemFile, renderContext.Device);
            //    // need to render 1 struct/shape at a time
            //    NuSceneBuffer3D sceneBuffer = ChemModel.RenderShapes(frame, ChemModel.ShapesType.Ribbons);
            //    renderSource.sceneBuffer = sceneBuffer;

            //    renderContext.ribbons = new ChemEntityRibbon(sceneBuffer);
            //    renderContext.ribbons.Init(renderContext.Device, outputDesc.GeneralStructuresShadingDesc);

            //    //sceneBuffer = ChemModel.RenderShapes(frame, ChemModel.ShapesType.Cartoon);

            //    //renderContext.cartoons = new ChemEntityCartoon(sceneBuffer);
            //    //renderContext.cartoons.Init(renderContext.Device, outputDesc.GeneralStructuresShadingDesc);
            //}

            /*SmilesGenerator gen = new SmilesGenerator(file.chemFile.Builder);
            moleculeSMILESRawString = gen.createSMILES(file.chemFile.ChemSequences[0].ChemModels[0].SetOfMolecules.Molecules[0]);
            */
            renderFrame = true;
            renderingThread.Start();

            UleDlg dlg = new UleDlg(file.results, file.progress);
            dlg.ShowDialog();
            dlg.Dispose();
        }

        public void ApplySettings(CompleteOutputDescription outputDesc)
        {
            this.outputDesc = outputDesc;
            this.outputDesc.AtomShadingDesc.MoleculeMaterials = modules[0];
            this.outputDesc.BondShadingDesc.MoleculeMaterials = modules[0];
            if (renderContext != null)
            {
                renderContext.ApplyOutputDescription(outputDesc);
                renderContext.OnResize(Width, Height);
            }
        }
        
        public void Init(HashTableSettings settings, ICommonDeviceInterface cdi)
        {
            this.settings = settings;
            settings["Molecule.Shading.Material.Type"] = "BySerie";

            // load settings
            BackColor = (Color)settings["View3D.BgClr"];

            // load local modules
            foreach (ISettingsModule module in modules)
            {
                module.LoadModuleSettings(settings);
            }

            renderContext = new ChemRenderingContext3DDX9(settings, this, (CommonDeviceInterface)cdi);
            renderContext.BackColor = BackColor;

            // load default output settings
            CompleteOutputDescription desc = CompleteOutputDescription.LoadDescription(Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenSVisualLib.defaultOutput.xml"));
            desc.SchemeSettings = new BallAndStickSchemeSettings();
            desc.SchemeSettings.AtomLOD = 2;
            ApplySettings(desc);
        }
        #endregion

        #region Dialog Commands

        public void OpenEditShadingDialog()
        {
            //EditShadingDialog dlg = new EditShadingDialog(renderContext.OutputDescription.GeneralShadingDesc,
            //                                              renderContext.OutputDescription.AtomShadingDesc,
            //                                              renderContext.OutputDescription.BondShadingDesc,
            //                                              renderContext.OutputCaps,
            //                                              renderContext.Device,
            //                                              modules.ToArray());
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    renderContext.OutputDescription.GeneralShadingDesc = dlg.ResultGeneralShading;
            //    renderContext.OutputDescription.AtomShadingDesc = dlg.ResultAtomShading;
            //    renderContext.OutputDescription.BondShadingDesc = dlg.ResultBondShading;
            //    renderContext.ApplyOutputDescription(renderContext.OutputDescription);

            //    renderContext.Render(null);
            //}

            //dlg.Dispose();

            MoleculeSchemeDlg dlg = new MoleculeSchemeDlg(settings, renderContext.OutputCaps, renderContext.Device, outputDesc);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                // pass on settings to context
                renderContext.ApplyOutputDescription(dlg.FinalCoDesc);
                renderContext.Render(null);
            }
            dlg.Dispose();
        }

        public void OpenEditLightingDialog()
        {
            //EditLightingDialog dlg = new EditLightingDialog(renderContext.OutputSettings,
            //                                                renderContext.OutputDescription.GeneralLightingDesc,
            //                                                settings, renderContext.OutputCaps);
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{

            //}
            //dlg.Dispose();
        }

        public void OpenMoleculeDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            // TODO: Build filter from known formats
            int filterIdx;
            dialog.Filter = MoleculeLoader.CreateOpenFilter(out filterIdx, dialog.InitialDirectory);
            dialog.FilterIndex = filterIdx;
            if (dialog.ShowDialog() == DialogResult.OK)
                LoadFile(dialog.FileName);
            dialog.Dispose();
        }

        #endregion

        #region IChemControl Members

        public void OpenRecordingLoadingDialog()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void StartRecording(RecordingSettings settings)
        {
            ViewRecording.ViewRecorder vr = new ViewRecording.ViewRecorder();
            ViewRecording rec = new ViewRecording(vr);
            EncodingDlg dlg = new EncodingDlg(rec, RecordingSettings.DefaultsInstance, "c:/testOut.avi", renderContext);
            dlg.ShowDialog(this);

            // start recording actions
            //if (settings == null)
            //    this.rSettings = RecordingSettings.DefaultsInstance;
            //else
            //    this.rSettings = settings;

            //controlHandler.Recorder = new ViewRecording.ViewRecorder();
            //renderContext.Recording = true;
        }

        public void StopRecording()
        {
            ViewRecording.ViewRecorder recorder = controlHandler.Recorder;
            controlHandler.Recorder = null;
            renderContext.Recording = false;

            // bring up dialog
            ViewRecordingDlg dlg = new ViewRecordingDlg(rSettings, new ViewRecording(recorder));
            dlg.ShowDialog(this);
            dlg.Dispose();
        }

        public void OpenRecording(string filename)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region Toolstrip Events

        private void moleculeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMoleculeDialog();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // show rendering info
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEditShadingDialog();
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenEditLightingDialog();
        }

        private void axisToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (renderContext != null)
            {
                renderContext.DrawAxis = axisToolStripMenuItem.Checked;
                renderContext.Render(null);
            }
        }

        private void supportedFormatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportedFormatsDlg();
        }

        private void ballStickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to ball & stick scheme

        }

        private void blendedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to blended bonds
            renderContext.OutputDescription.BondShadingDesc.BlendEndClrs = blendedToolStripMenuItem.Checked;
            renderContext.ApplyOutputDescription(renderContext.OutputDescription);

            renderContext.Render(null);
        }

        private void thinLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to thin lines
            BallAndStickSchemeSettings scheme;
            if (!(renderContext.OutputDescription.SchemeSettings is BallAndStickSchemeSettings))
                renderContext.OutputDescription.SchemeSettings = new BallAndStickSchemeSettings();
            scheme = (BallAndStickSchemeSettings)renderContext.OutputDescription.SchemeSettings;

            scheme.StickThickness = 0;

            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }

        private void thickLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to thick lines
            BallAndStickSchemeSettings scheme;
            if (!(renderContext.OutputDescription.SchemeSettings is BallAndStickSchemeSettings))
                renderContext.OutputDescription.SchemeSettings = new BallAndStickSchemeSettings();
            scheme = (BallAndStickSchemeSettings)renderContext.OutputDescription.SchemeSettings;

            scheme.StickThickness = 1;

            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }

        private void drawAtomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // toggle atom rendering off
            renderContext.OutputDescription.AtomShadingDesc.Draw = drawAtomsToolStripMenuItem.Checked;
            renderContext.Render(null);
        }

        private void aToBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to A to B spacing
            renderContext.OutputDescription.BondShadingDesc.Spacing = BondShadingDesc.BondSpacings.AtoB;
            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }

        private void betweenBondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to between atoms spacing
            renderContext.OutputDescription.BondShadingDesc.Spacing = BondShadingDesc.BondSpacings.CenterSpace;
            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }

        private void roundedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to rounded bond ends
            renderContext.OutputDescription.BondShadingDesc.EndType = BondShadingDesc.BondEndTypes.Rounded;
            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }

        private void flatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to closed bond ends
            renderContext.OutputDescription.BondShadingDesc.EndType = BondShadingDesc.BondEndTypes.Closed;
            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }

        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change to point bond ends
            renderContext.OutputDescription.BondShadingDesc.EndType = BondShadingDesc.BondEndTypes.Point;
            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // change to rounded bond ends
            renderContext.OutputDescription.BondShadingDesc.EndType = BondShadingDesc.BondEndTypes.Open;
            renderContext.ApplyOutputDescription(renderContext.OutputDescription);
            renderContext.Render(null);
        }
        
        private void editSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEditShadingDialog();
        }
        #endregion

        #region IChemControl Members

        public int NumAtoms
        {
            get { if (renderContext != null) return renderContext.numAtoms; return 0; }
        }

        public int NumBonds
        {
            get { if (renderContext != null) return renderContext.numBonds; return 0; }
        }

        public ChemEntity TrySelectAtPoint(int x, int y)
        {
            selected.Clear();
            if (renderContext != null)
            {
                ChemEntity entity = renderContext.TrySelectAtPoint(x, y);
                if (entity != null)
                    selected.Add(entity);
                // update view
                renderContext.UpdateSelectedItems(selected);
                Render();

                return entity;
            }
            return null;
        }

        public void SelectMode(bool toggleOn)
        {
            controlMode = toggleOn ? ControlMode.Selection : ControlMode.ViewMovement;
        }

        public void SelectAll()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MeasureDistance()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MeasureAngle()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MeasureDihedral()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ClearMesurements()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Setup(MeasurementSettings settings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IChemObject GetRootNode()
        {
            if (renderContext != null)
                return renderContext.ChemRenderSource.ChemFile;
            return null;
        }

        public ChemEntity[] GetSelection()
        {
            return selected.ToArray();
        }

        public void TakeScreenshot(string file, System.Drawing.Imaging.ImageFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Bitmap TakeScreenshotToBitmap()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool LowDetailMovement
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
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

        public ControlMode ControlMode
        {
            get { return controlMode; }
            set
            {
                OnChangeControlMode(value);
                controlMode = value;
            }
        }

        public IChemObject[] QueryConnected(IChemObject obj)
        {
            List<IChemObject> objs = new List<IChemObject>();
            if (obj is IAtom)
            {
                // find connected bonds
                foreach (IBond bond in renderContext.bonds)
                {
                    IAtom[] atoms = bond.getAtoms();
                    foreach (IAtom atom in atoms)
                    {
                        if (atom == obj)
                        {
                            // add bond
                            objs.Add(bond);
                            // add all other atoms on this bond
                            foreach (IAtom a in atoms)
                            {
                                if (a != obj)
                                    objs.Add(a);
                            }
                        }
                    }
                }
            }
            return objs.ToArray();
        }

        #endregion

        private void OnChangeControlMode(ControlMode value)
        {
            if (value == controlMode)
                return;
            // turn off any old things
            switch (controlMode)
            {
                case ControlMode.Selection:
                    break;
                case ControlMode.SelectionMovement:
                    renderContext.RemoveSelectionMovementItems();
                    break;
                case ControlMode.SelectionRotation:
                    break;
                case ControlMode.ViewMovement:
                    break;
            }
            // turn on new items
            switch (value)
            {
                case ControlMode.Selection:
                    break;
                case ControlMode.SelectionMovement:
                    renderContext.AddSelectionMovementItems();
                    break;
                case ControlMode.SelectionRotation:
                    break;
                case ControlMode.ViewMovement:
                    break;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (renderContext != null)
                renderContext.OnResize(Width, Height);
        }

        #region IChemControl Members


        public void LoadSMILES(string smiles, bool isSMARTS)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void InsertSMILES(string smiles, bool isSMARTS)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ChemEntity[] GetAllNearby(ChemEntity entity, float radius)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ChemEntity[] GetAllNearby(Vector3 position, float radius)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ChemEntity GetNearestTo(ChemEntity entity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ChemEntity GetNearestTo(Vector3 position)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ShowMarkup
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool ShowLayers
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public ChemEntity[] QueryConnectedEntities(ChemEntity entity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string MoleculeSMILESRawString
        {
            get { return moleculeSMILESRawString; }
        }

        public SMILESUpdateDelegate OnSMILESUpdate
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool AutoBonding
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public AtomEntity NewAtom(string chemSymbol, Vector3 position)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public AtomEntity NewAtom(IAtom newAtom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAtom(AtomEntity atom, bool queryUser)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ChangeAtomElement(AtomEntity atom, string newChemSymbol)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}