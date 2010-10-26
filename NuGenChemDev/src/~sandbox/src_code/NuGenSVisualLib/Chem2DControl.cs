using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.TwoD;
using NuGenSVisualLib.Chem;
using NuGenSVisualLib.Rendering.Chem;
using NuGenSVisualLib.Rendering;
using NuGenSVisualLib.Settings;

namespace NuGenSVisualLib.Controls
{
//    /// <summary>
//    /// Encapsulates a 2D chemistry control
//    /// </summary>
//    public partial class Chem2DControl : UserControl, IChemControl
//    {
//        public enum DrawingModes
//        {
//            None,
//            GDI,
//            DirectDraw
//        }
//
//        private DrawingModes drawingMode;
//        private RenderingContext2D renderContext;
//        private ChemRenderingSource2D renderSource;
//
//        public DrawingModes DrawingMode
//        {
//            get { return drawingMode; }
//            set { ChangeContext(value); drawingMode = value; }
//        }
//
//        public string Title
//        {
//            get
//            {
//                if (renderSource != null)
//                    return renderSource.Title;
//                return "";
//            }
//        }
//
//        private void ChangeContext(DrawingModes newMode)
//        {
//            if (newMode != drawingMode)
//            {
//                if (renderContext != null)
//                    renderContext.Dispose();
//
//                if (newMode == DrawingModes.GDI)
//                {
//                    ChemRenderingContext2DGDI rc = new ChemRenderingContext2DGDI();
//                    rc.RenderSource = renderSource;
//                    rc.TargetRenderArea = this;
//                    rc.View = new RenderingView2D();
//
//                    rc.BackColor = this.BackColor;
//
//                    renderContext = rc;
//                }
//            }
//        }
//
//        public Chem2DControl()
//        {
//            InitializeComponent();
//
//            drawingMode = DrawingModes.None;
//        }
//
//        public void LoadFile(string file)
//        {
//            MoleculeLoadingResults results;
//            renderSource = new ChemRenderingSource2D(MoleculeLoader.LoadFromFile(file, null, MoleculeLoader.FileUsage.TwoD, null, out results));
//            if (renderContext != null)
//                renderContext.RenderSource = renderSource;
//        }
//
//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//        }
//
//        protected override void OnResize(EventArgs e)
//        {
//            base.OnResize(e);
//        }
//
//        protected override void OnPaint(PaintEventArgs e)
//        {
//            if (renderContext != null)
//                renderContext.Render(e.Graphics);
//
//            base.OnPaint(e);
//        }
//
//        public override Color BackColor
//        {
//            get
//            {
//                return base.BackColor;
//            }
//            set
//            {
//                base.BackColor = value;
//                if (renderContext != null)
//                    renderContext.BackColor = value;
//            }
//        }
//
//        #region IChemControl Members
//
//        public void ApplySettings(CompleteOutputDescription outputDesc)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        #endregion
//
//        #region IChemControl Members
//
//
//        public CompleteOutputDescription OutputDescription
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }
//
//        #endregion
//
//        #region IChemControl Members
//
//        public void Init(HashTableSettings settings)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        #endregion
//
//        #region IChemControl Members
//
//
//        public void OpenEditShadingDialog()
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        #endregion
//
//        #region IChemControl Members
//
//
//        public void OpenRenderingInfoDialog()
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        #endregion
//
//        #region IChemControl Members
//
//
//        public void OpenRecordingLoadingDialog()
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        public void StartRecording(NuGenSVisualLib.Recording.RecordingSettings settings)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        public void StopRecording()
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        public void OpenRecording(string filename)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        #endregion
//
//        #region IChemControl Members
//
//        public void Init(HashTableSettings settings, NuGenSVisualLib.Rendering.Devices.ICommonDeviceInterface cdi)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//
//        #endregion
//
//        #region IChemControl Members
//
//
//        public int Atoms
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }
//
//        public int Bonds
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }
//
//        #endregion
//    }
}
