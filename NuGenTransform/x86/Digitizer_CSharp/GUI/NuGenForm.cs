using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    /// <summary>
    /// This class is the entry point to the program and represents the opened
    /// window and contains all the viewable components within.  Handles menu
    /// commands, toolbar button presses and delegates events to the lower level
    /// classes to be processed.  Does not handle any of the events related to the
    /// interior of the window.
    /// </summary>
    public partial class NuGenForm : Form
    {
        NuGenEventHandler handler;

        public NuGenForm(NuGenEventHandler h)
        {
            menu = new NuGenMainPopupMenu(h, null);           

            RegisterEventHandler(h);
            InitializeComponent();
            InitializeDefaults();
            DisableEditMenu();

            statusBar.Panels.Add(normalStatus);
            statusBar.Panels.Add(permanentStatus);
            statusBar.Panels.Add(resStatus);
            statusBar.Panels.Add(coordsStatus);

            this.importButton.Click += handler.Import_Click;
            this.exportButton.Click += handler.Export_Click;
            this.openButton.Click += handler.Open_Click;
            this.saveButton.Click += handler.Save_Click;

            this.cutButton.Click += handler.Cut_Click;
            this.copyButton.Click += handler.Copy_Click;
            this.pasteAsButton.Click += handler.PasteAsNew_Click;
            this.pasteButton.Click += handler.Paste_Click;

            this.selectButton.Click += handler.Select_Click;

            this.axisButton.Click += handler.AxisPoint_Click;
            this.scaleButton.Click += handler.ScaleBar_Click;

            this.segmentButton.Click += handler.SegmentFill_Click;
            this.curvePointButton.Click += handler.CurvePoint_Click;
            this.pointMatchButton.Click += handler.PointMatch_Click;

            this.measureButton.Click += handler.MeasurePoint_Click;

            this.selectButton.Click += new EventHandler(toggleButton_Click);

            this.axisButton.Click += new EventHandler(toggleButton_Click);
            this.scaleButton.Click += new EventHandler(toggleButton_Click);

            this.segmentButton.Click += new EventHandler(toggleButton_Click);
            this.curvePointButton.Click += new EventHandler(toggleButton_Click);
            this.pointMatchButton.Click += new EventHandler(toggleButton_Click);

            this.measureButton.Click += new EventHandler(toggleButton_Click);

            Text = "NuGenTransform";
        }

        void toggleButton_Click(object sender, EventArgs e)
        {
            this.selectButton.IsPressed = false;

            this.axisButton.IsPressed = false;
            this.scaleButton.IsPressed = false;

            this.segmentButton.IsPressed = false;
            this.curvePointButton.IsPressed = false;
            this.pointMatchButton.IsPressed = false;

            this.measureButton.IsPressed = false;

            this.selectButton.Refresh();

            this.axisButton.Refresh();
            this.scaleButton.Refresh();

            this.segmentButton.Refresh();
            this.curvePointButton.Refresh();
            this.pointMatchButton.Refresh();

            this.measureButton.Refresh();

            ((Genetibase.UI.RibbonButton)sender).IsPressed = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            NuGenDefaultSettings.GetInstance().SaveSettings();
        }

        //Handles an evend from a key pressed other than standard ascii keyset
        protected override bool ProcessDialogKey(Keys keyData)
        {
            return handler.ProcessDialogKey(keyData);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            try
            {
                normalStatus.Width = (int)(.4 * Width);
                permanentStatus.Width = (int)(.4 * Width);
                resStatus.Width = (int)(.1 * Width);
                coordsStatus.Width = (int)(.1 * Width);
            }
            catch (Exception ex) { }
        }

        public void AddCurve(string curve)
        {
            if (curveCombo.Items.Count <= 1)
            {
                curveCombo.Enabled = true;
            }

            if(!curveCombo.Items.Contains(curve))
                curveCombo.Items.Add(curve);
        }

        public void RemoveCurve(string curve)
        {
            if (curveCombo.Items.Count <= 2)
            {
                curveCombo.Enabled = false;
            }

            curveCombo.Items.Remove(curve);
        }

        public void AddMeasure(string curve)
        {
            if (measureCombo.Items.Count <= 1)
            {
                measureCombo.Enabled = true;
            }

            measureCombo.Items.Add(curve);
        }

        public void RemoveMeasure(string curve)
        {
            if (measureCombo.Items.Count <= 2)
            {
                measureCombo.Enabled = false;
            }

            measureCombo.Items.Remove(curve);
        }

        public void CheckedPointViewOption(ViewPointSelection sel)
        {
            menu.CheckedPointViewOption(sel);
        }

        public void CheckedBackgroundOption(BackgroundSelection sel)
        {
            menu.CheckedBackgroundOption(sel);
        }

        //Status bar messages
        public void StatusNormalMessage(string msg)
        {
            normalStatus.Text = msg;
            statusBar.Refresh();
        }

        public void StatusPermanentMessage(string msg)
        {
            permanentStatus.Text = msg;
            statusBar.Refresh();
        }

        public void StatusResMessage(string msg)
        {
            resStatus.Text = msg;
            statusBar.Refresh();
        }

        public void StatusCoordMessage(string msg)
        {
            coordsStatus.Text = msg;
            statusBar.Refresh();
        }

        public void RegisterEventHandler(NuGenEventHandler h)
        {
            handler = h;
        }

        public ComboBox CurveCombo
        {
            get
            {
                return curveCombo;
            }
        }

        public ComboBox MeasureCombo
        {
            get
            {
                return measureCombo;
            }
        }

        protected override void  OnMdiChildActivate(EventArgs e)
        {
 	         base.OnMdiChildActivate(e);
             handler.View_Activated(this.ActiveMdiChild, e);            
        }

        internal void EnableEditMenu()
        {
            cutButton.Enabled = true;
            copyButton.Enabled = true;
            pasteAsButton.Enabled = true;
            pasteButton.Enabled = true;
        }

        internal void DisableEditMenu()
        {
            cutButton.Enabled = false;
            copyButton.Enabled = false;
            pasteAsButton.Enabled = false;
            pasteButton.Enabled = false;            
        }
    }
}