namespace Genetibase.NuGenTransform
{
    using System.Windows.Forms;
    using System.Drawing;
    using System.IO;
    using Genetibase.UI;

    partial class NuGenForm
    {

        #region Components Declaration
        private RibbonControl ribbonBar;

        private RibbonTab actionsTab;
        private RibbonTab selectTab;

        //StatusBar
        private NuGenStatusBar statusBar;
        /****************************/

        private ComboBox curveCombo;
        private ComboBox measureCombo;

        private StatusBarPanel normalStatus;
        private StatusBarPanel permanentStatus;
        private StatusBarPanel resStatus;
        private StatusBarPanel coordsStatus;

        RibbonButton cutButton;
        RibbonButton copyButton;
        RibbonButton pasteButton;
        RibbonButton pasteAsButton;

        private NuGenMainPopupMenu menu;

        #endregion

        #region Windows Forms Initialization
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ribbonBar = new Genetibase.UI.RibbonControl();
            this.blankTab = new Genetibase.UI.RibbonTab();
            this.actionsTab = new Genetibase.UI.RibbonTab();
            this.fileButtonsGroup = new Genetibase.UI.RibbonGroup();
            this.importButton = new Genetibase.UI.RibbonButton();
            this.exportButton = new Genetibase.UI.RibbonButton();
            this.openButton = new Genetibase.UI.RibbonButton();
            this.saveButton = new Genetibase.UI.RibbonButton();
            this.editButtonsGroup = new Genetibase.UI.RibbonGroup();
            this.cutButton = new Genetibase.UI.RibbonButton();
            this.copyButton = new Genetibase.UI.RibbonButton();
            this.pasteButton = new Genetibase.UI.RibbonButton();
            this.pasteAsButton = new Genetibase.UI.RibbonButton();
            this.selectTab = new Genetibase.UI.RibbonTab();
            this.selectButtonsGroup = new Genetibase.UI.RibbonGroup();
            this.selectButton = new Genetibase.UI.RibbonButton();
            this.curveButtonsGroup = new Genetibase.UI.RibbonGroup();
            this.curvePointButton = new Genetibase.UI.RibbonButton();
            this.segmentButton = new Genetibase.UI.RibbonButton();
            this.pointMatchButton = new Genetibase.UI.RibbonButton();
            this.curveCombo = new System.Windows.Forms.ComboBox();
            this.measureButtonsGroup = new Genetibase.UI.RibbonGroup();
            this.measureButton = new Genetibase.UI.RibbonButton();
            this.measureCombo = new System.Windows.Forms.ComboBox();
            this.scaleButtonsGroup = new Genetibase.UI.RibbonGroup();
            this.axisButton = new Genetibase.UI.RibbonButton();
            this.scaleButton = new Genetibase.UI.RibbonButton();
            this.normalStatus = new System.Windows.Forms.StatusBarPanel();
            this.permanentStatus = new System.Windows.Forms.StatusBarPanel();
            this.resStatus = new System.Windows.Forms.StatusBarPanel();
            this.coordsStatus = new System.Windows.Forms.StatusBarPanel();
            this.statusBar = new Genetibase.NuGenTransform.NuGenStatusBar();
            this.ribbonBar.SuspendLayout();
            this.actionsTab.SuspendLayout();
            this.fileButtonsGroup.SuspendLayout();
            this.editButtonsGroup.SuspendLayout();
            this.selectTab.SuspendLayout();
            this.selectButtonsGroup.SuspendLayout();
            this.curveButtonsGroup.SuspendLayout();
            this.measureButtonsGroup.SuspendLayout();
            this.scaleButtonsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.permanentStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coordsStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonBar
            // 
            this.ribbonBar.Controls.Add(this.blankTab);
            this.ribbonBar.Controls.Add(this.actionsTab);
            this.ribbonBar.Controls.Add(this.selectTab);
            this.ribbonBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonBar.Location = new System.Drawing.Point(0, 0);
            this.ribbonBar.Name = "ribbonBar";
            this.ribbonBar.SelectedIndex = 1;
            this.ribbonBar.Size = new System.Drawing.Size(800, 116);
            this.ribbonBar.TabIndex = 0;
            this.ribbonBar.OnPopup += new Genetibase.UI.RibbonPopupEventHandler(this.ribbonBar_OnPopup);
            // 
            // blankTab
            // 
            this.blankTab.Location = new System.Drawing.Point(4, 25);
            this.blankTab.Name = "blankTab";
            this.blankTab.Size = new System.Drawing.Size(792, 87);
            this.blankTab.TabIndex = 0;
            this.blankTab.Text = "Menu";
            // 
            // actionsTab
            // 
            this.actionsTab.Controls.Add(this.fileButtonsGroup);
            this.actionsTab.Controls.Add(this.editButtonsGroup);
            this.actionsTab.Location = new System.Drawing.Point(4, 25);
            this.actionsTab.Name = "actionsTab";
            this.actionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.actionsTab.Size = new System.Drawing.Size(792, 87);
            this.actionsTab.TabIndex = 0;
            this.actionsTab.Text = "Actions";
            this.actionsTab.UseVisualStyleBackColor = true;
            // 
            // fileButtonsGroup
            // 
            this.fileButtonsGroup.Controls.Add(this.importButton);
            this.fileButtonsGroup.Controls.Add(this.exportButton);
            this.fileButtonsGroup.Controls.Add(this.openButton);
            this.fileButtonsGroup.Controls.Add(this.saveButton);
            this.fileButtonsGroup.Location = new System.Drawing.Point(5, 5);
            this.fileButtonsGroup.Margin = new System.Windows.Forms.Padding(1);
            this.fileButtonsGroup.Name = "fileButtonsGroup";
            this.fileButtonsGroup.Size = new System.Drawing.Size(280, 79);
            this.fileButtonsGroup.TabIndex = 1;
            this.fileButtonsGroup.TabStop = false;
            this.fileButtonsGroup.Text = "File";
            // 
            // importButton
            // 
            this.importButton.Command = null;
            this.importButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.fileimport;
            this.importButton.IsFlat = true;
            this.importButton.IsPressed = false;
            this.importButton.Location = new System.Drawing.Point(5, 5);
            this.importButton.Margin = new System.Windows.Forms.Padding(0);
            this.importButton.Name = "importButton";
            this.importButton.Padding = new System.Windows.Forms.Padding(2);
            this.importButton.Size = new System.Drawing.Size(64, 48);
            this.importButton.TabIndex = 0;
            this.importButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exportButton
            // 
            this.exportButton.Command = null;
            this.exportButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.fileexport;
            this.exportButton.IsFlat = true;
            this.exportButton.IsPressed = false;
            this.exportButton.Location = new System.Drawing.Point(74, 5);
            this.exportButton.Margin = new System.Windows.Forms.Padding(0);
            this.exportButton.Name = "exportButton";
            this.exportButton.Padding = new System.Windows.Forms.Padding(2);
            this.exportButton.Size = new System.Drawing.Size(64, 48);
            this.exportButton.TabIndex = 1;
            this.exportButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openButton
            // 
            this.openButton.Command = null;
            this.openButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.fileopen;
            this.openButton.IsFlat = true;
            this.openButton.IsPressed = false;
            this.openButton.Location = new System.Drawing.Point(143, 5);
            this.openButton.Margin = new System.Windows.Forms.Padding(0);
            this.openButton.Name = "openButton";
            this.openButton.Padding = new System.Windows.Forms.Padding(2);
            this.openButton.Size = new System.Drawing.Size(64, 48);
            this.openButton.TabIndex = 2;
            this.openButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveButton
            // 
            this.saveButton.Command = null;
            this.saveButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.filesave;
            this.saveButton.IsFlat = true;
            this.saveButton.IsPressed = false;
            this.saveButton.Location = new System.Drawing.Point(210, 5);
            this.saveButton.Margin = new System.Windows.Forms.Padding(0);
            this.saveButton.Name = "saveButton";
            this.saveButton.Padding = new System.Windows.Forms.Padding(2);
            this.saveButton.Size = new System.Drawing.Size(64, 48);
            this.saveButton.TabIndex = 3;
            this.saveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // editButtonsGroup
            // 
            this.editButtonsGroup.Controls.Add(this.cutButton);
            this.editButtonsGroup.Controls.Add(this.copyButton);
            this.editButtonsGroup.Controls.Add(this.pasteButton);
            this.editButtonsGroup.Controls.Add(this.pasteAsButton);
            this.editButtonsGroup.Location = new System.Drawing.Point(290, 5);
            this.editButtonsGroup.Margin = new System.Windows.Forms.Padding(1);
            this.editButtonsGroup.Name = "editButtonsGroup";
            this.editButtonsGroup.Size = new System.Drawing.Size(280, 79);
            this.editButtonsGroup.TabIndex = 1;
            this.editButtonsGroup.TabStop = false;
            this.editButtonsGroup.Text = "Edit";
            // 
            // cutButton
            // 
            this.cutButton.Command = null;
            this.cutButton.IsFlat = true;
            this.cutButton.IsPressed = false;
            this.cutButton.Location = new System.Drawing.Point(5, 5);
            this.cutButton.Margin = new System.Windows.Forms.Padding(0);
            this.cutButton.Name = "cutButton";
            this.cutButton.Padding = new System.Windows.Forms.Padding(2);
            this.cutButton.Size = new System.Drawing.Size(64, 48);
            this.cutButton.TabIndex = 0;
            this.cutButton.Text = "Cut";
            this.cutButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // copyButton
            // 
            this.copyButton.Command = null;
            this.copyButton.IsFlat = true;
            this.copyButton.IsPressed = false;
            this.copyButton.Location = new System.Drawing.Point(74, 5);
            this.copyButton.Margin = new System.Windows.Forms.Padding(0);
            this.copyButton.Name = "copyButton";
            this.copyButton.Padding = new System.Windows.Forms.Padding(2);
            this.copyButton.Size = new System.Drawing.Size(64, 48);
            this.copyButton.TabIndex = 1;
            this.copyButton.Text = "Copy";
            this.copyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pasteButton
            // 
            this.pasteButton.Command = null;
            this.pasteButton.IsFlat = true;
            this.pasteButton.IsPressed = false;
            this.pasteButton.Location = new System.Drawing.Point(143, 5);
            this.pasteButton.Margin = new System.Windows.Forms.Padding(0);
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Padding = new System.Windows.Forms.Padding(2);
            this.pasteButton.Size = new System.Drawing.Size(64, 48);
            this.pasteButton.TabIndex = 2;
            this.pasteButton.Text = "Paste";
            this.pasteButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pasteAsButton
            // 
            this.pasteAsButton.Command = null;
            this.pasteAsButton.IsFlat = true;
            this.pasteAsButton.IsPressed = false;
            this.pasteAsButton.Location = new System.Drawing.Point(210, 5);
            this.pasteAsButton.Margin = new System.Windows.Forms.Padding(0);
            this.pasteAsButton.Name = "pasteAsButton";
            this.pasteAsButton.Padding = new System.Windows.Forms.Padding(2);
            this.pasteAsButton.Size = new System.Drawing.Size(64, 48);
            this.pasteAsButton.TabIndex = 3;
            this.pasteAsButton.Text = "Paste As";
            this.pasteAsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // selectTab
            // 
            this.selectTab.Controls.Add(this.selectButtonsGroup);
            this.selectTab.Controls.Add(this.curveButtonsGroup);
            this.selectTab.Controls.Add(this.measureButtonsGroup);
            this.selectTab.Controls.Add(this.scaleButtonsGroup);
            this.selectTab.Location = new System.Drawing.Point(4, 25);
            this.selectTab.Name = "selectTab";
            this.selectTab.Padding = new System.Windows.Forms.Padding(3);
            this.selectTab.Size = new System.Drawing.Size(792, 87);
            this.selectTab.TabIndex = 1;
            this.selectTab.Text = "Mode";
            this.selectTab.UseVisualStyleBackColor = true;
            // 
            // selectButtonsGroup
            // 
            this.selectButtonsGroup.Controls.Add(this.selectButton);
            this.selectButtonsGroup.Location = new System.Drawing.Point(5, 5);
            this.selectButtonsGroup.Margin = new System.Windows.Forms.Padding(1);
            this.selectButtonsGroup.Name = "selectButtonsGroup";
            this.selectButtonsGroup.Size = new System.Drawing.Size(74, 79);
            this.selectButtonsGroup.TabIndex = 1;
            this.selectButtonsGroup.TabStop = false;
            this.selectButtonsGroup.Text = "Select";
            // 
            // selectButton
            // 
            this.selectButton.Command = null;
            this.selectButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitselectlarge;
            this.selectButton.IsFlat = true;
            this.selectButton.IsPressed = false;
            this.selectButton.Location = new System.Drawing.Point(5, 5);
            this.selectButton.Margin = new System.Windows.Forms.Padding(0);
            this.selectButton.Name = "selectButton";
            this.selectButton.Padding = new System.Windows.Forms.Padding(2);
            this.selectButton.Size = new System.Drawing.Size(64, 48);
            this.selectButton.TabIndex = 0;
            this.selectButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // curveButtonsGroup
            // 
            this.curveButtonsGroup.Controls.Add(this.curvePointButton);
            this.curveButtonsGroup.Controls.Add(this.segmentButton);
            this.curveButtonsGroup.Controls.Add(this.pointMatchButton);
            this.curveButtonsGroup.Controls.Add(this.curveCombo);
            this.curveButtonsGroup.Location = new System.Drawing.Point(232, 5);
            this.curveButtonsGroup.Margin = new System.Windows.Forms.Padding(1);
            this.curveButtonsGroup.Name = "curveButtonsGroup";
            this.curveButtonsGroup.Size = new System.Drawing.Size(345, 79);
            this.curveButtonsGroup.TabIndex = 2;
            this.curveButtonsGroup.TabStop = false;
            this.curveButtonsGroup.Text = "Curve Mode";
            // 
            // curvePointButton
            // 
            this.curvePointButton.Command = null;
            this.curvePointButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitcurvelarge;
            this.curvePointButton.IsFlat = true;
            this.curvePointButton.IsPressed = false;
            this.curvePointButton.Location = new System.Drawing.Point(5, 5);
            this.curvePointButton.Margin = new System.Windows.Forms.Padding(0);
            this.curvePointButton.Name = "curvePointButton";
            this.curvePointButton.Padding = new System.Windows.Forms.Padding(2);
            this.curvePointButton.Size = new System.Drawing.Size(64, 48);
            this.curvePointButton.TabIndex = 0;
            this.curvePointButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // segmentButton
            // 
            this.segmentButton.Command = null;
            this.segmentButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitsegmentlarge;
            this.segmentButton.IsFlat = true;
            this.segmentButton.IsPressed = false;
            this.segmentButton.Location = new System.Drawing.Point(74, 5);
            this.segmentButton.Margin = new System.Windows.Forms.Padding(0);
            this.segmentButton.Name = "segmentButton";
            this.segmentButton.Padding = new System.Windows.Forms.Padding(2);
            this.segmentButton.Size = new System.Drawing.Size(64, 48);
            this.segmentButton.TabIndex = 1;
            this.segmentButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pointMatchButton
            // 
            this.pointMatchButton.Command = null;
            this.pointMatchButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitmatchlarge;
            this.pointMatchButton.IsFlat = true;
            this.pointMatchButton.IsPressed = false;
            this.pointMatchButton.Location = new System.Drawing.Point(143, 5);
            this.pointMatchButton.Margin = new System.Windows.Forms.Padding(0);
            this.pointMatchButton.Name = "pointMatchButton";
            this.pointMatchButton.Padding = new System.Windows.Forms.Padding(2);
            this.pointMatchButton.Size = new System.Drawing.Size(64, 48);
            this.pointMatchButton.TabIndex = 2;
            this.pointMatchButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // curveCombo
            // 
            this.curveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.curveCombo.Location = new System.Drawing.Point(210, 30);
            this.curveCombo.Name = "curveCombo";
            this.curveCombo.Size = new System.Drawing.Size(121, 21);
            this.curveCombo.TabIndex = 3;
            // 
            // measureButtonsGroup
            // 
            this.measureButtonsGroup.Controls.Add(this.measureButton);
            this.measureButtonsGroup.Controls.Add(this.measureCombo);
            this.measureButtonsGroup.Location = new System.Drawing.Point(582, 5);
            this.measureButtonsGroup.Margin = new System.Windows.Forms.Padding(1);
            this.measureButtonsGroup.Name = "measureButtonsGroup";
            this.measureButtonsGroup.Size = new System.Drawing.Size(200, 79);
            this.measureButtonsGroup.TabIndex = 3;
            this.measureButtonsGroup.TabStop = false;
            this.measureButtonsGroup.Text = "Measure Mode";
            // 
            // measureButton
            // 
            this.measureButton.Command = null;
            this.measureButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitmeasurelarge;
            this.measureButton.IsFlat = true;
            this.measureButton.IsPressed = false;
            this.measureButton.Location = new System.Drawing.Point(5, 5);
            this.measureButton.Margin = new System.Windows.Forms.Padding(0);
            this.measureButton.Name = "measureButton";
            this.measureButton.Padding = new System.Windows.Forms.Padding(2);
            this.measureButton.Size = new System.Drawing.Size(64, 48);
            this.measureButton.TabIndex = 0;
            this.measureButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // measureCombo
            // 
            this.measureCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measureCombo.Location = new System.Drawing.Point(74, 30);
            this.measureCombo.Name = "measureCombo";
            this.measureCombo.Size = new System.Drawing.Size(121, 21);
            this.measureCombo.TabIndex = 1;
            // 
            // scaleButtonsGroup
            // 
            this.scaleButtonsGroup.Controls.Add(this.axisButton);
            this.scaleButtonsGroup.Controls.Add(this.scaleButton);
            this.scaleButtonsGroup.Location = new System.Drawing.Point(84, 5);
            this.scaleButtonsGroup.Margin = new System.Windows.Forms.Padding(1);
            this.scaleButtonsGroup.Name = "scaleButtonsGroup";
            this.scaleButtonsGroup.Size = new System.Drawing.Size(143, 79);
            this.scaleButtonsGroup.TabIndex = 1;
            this.scaleButtonsGroup.TabStop = false;
            this.scaleButtonsGroup.Text = "Graph Scale";
            // 
            // axisButton
            // 
            this.axisButton.Command = null;
            this.axisButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitaxissmall;
            this.axisButton.IsFlat = true;
            this.axisButton.IsPressed = false;
            this.axisButton.Location = new System.Drawing.Point(5, 5);
            this.axisButton.Margin = new System.Windows.Forms.Padding(0);
            this.axisButton.Name = "axisButton";
            this.axisButton.Padding = new System.Windows.Forms.Padding(2);
            this.axisButton.Size = new System.Drawing.Size(64, 48);
            this.axisButton.TabIndex = 0;
            this.axisButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scaleButton
            // 
            this.scaleButton.Command = null;
            this.scaleButton.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitscalelarge;
            this.scaleButton.IsFlat = true;
            this.scaleButton.IsPressed = false;
            this.scaleButton.Location = new System.Drawing.Point(74, 5);
            this.scaleButton.Margin = new System.Windows.Forms.Padding(0);
            this.scaleButton.Name = "scaleButton";
            this.scaleButton.Padding = new System.Windows.Forms.Padding(2);
            this.scaleButton.Size = new System.Drawing.Size(64, 48);
            this.scaleButton.TabIndex = 1;
            this.scaleButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // normalStatus
            // 
            this.normalStatus.Name = "normalStatus";
            this.normalStatus.Width = 320;
            // 
            // permanentStatus
            // 
            this.permanentStatus.Name = "permanentStatus";
            this.permanentStatus.Width = 320;
            // 
            // resStatus
            // 
            this.resStatus.Name = "resStatus";
            this.resStatus.Width = 80;
            // 
            // coordsStatus
            // 
            this.coordsStatus.Name = "coordsStatus";
            this.coordsStatus.Width = 80;
            // 
            // statusBar
            // 
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Font = new System.Drawing.Font("Helvetica", 8.25F);
            this.statusBar.Location = new System.Drawing.Point(0, 575);
            this.statusBar.Name = "statusBar";
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(800, 25);
            this.statusBar.TabIndex = 1;
            // 
            // NuGenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.ribbonBar);
            this.Controls.Add(this.statusBar);
            this.MinimumSize = new System.Drawing.Size(565, 200);
            this.Name = "NuGenForm";
            this.Text = "Form1";
            this.ribbonBar.ResumeLayout(false);
            this.actionsTab.ResumeLayout(false);
            this.fileButtonsGroup.ResumeLayout(false);
            this.editButtonsGroup.ResumeLayout(false);
            this.selectTab.ResumeLayout(false);
            this.selectButtonsGroup.ResumeLayout(false);
            this.curveButtonsGroup.ResumeLayout(false);
            this.measureButtonsGroup.ResumeLayout(false);
            this.scaleButtonsGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.normalStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.permanentStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coordsStatus)).EndInit();
            this.ResumeLayout(false);

        }

        void ribbonBar_OnPopup(object sender)
        {            
            menu.Location = ((RibbonControl)sender).PointToScreen(new Point(ribbonBar.Location.X + 5, ribbonBar.Location.Y + 22));
            menu.Show();
        }

            
        void File_Toolbar_Click(object sender, System.EventArgs args)
        {
            actionsTab.Visible = !actionsTab.Visible;
        }

        void Select_Toolbar_Click(object sender, System.EventArgs args)
        {
            selectTab.Visible = !selectTab.Visible;
        }

        void ImageScale_Toolbar_Click(object sender, System.EventArgs args)
        {
            
        }

        void DigitizeCurvePoints_Toolbar_Click(object sender, System.EventArgs args)
        {
            
        }

        void DigitizeMeasurePoints_Toolbar_Click(object sender, System.EventArgs args)
        {
            
        }

        public void EnableControls()
        {
            menu.EnableControls();

            foreach (Control c in selectTab.Controls)
            {
                c.Enabled = true;
            }
        }

        public void InitializeDefaults()
        {
            menu.InitializeDefaults();
            foreach (Control c in selectTab.Controls)
            {
                c.Enabled = false;
            }
        }
        
        void Statusbar_Click(object sender, System.EventArgs args)
        {
            statusBar.Visible = !statusBar.Visible;
        }
        #endregion

        private RibbonTab blankTab;
        private RibbonGroup fileButtonsGroup;
        private RibbonButton importButton;
        private RibbonButton exportButton;
        private RibbonButton openButton;
        private RibbonButton saveButton;
        private RibbonGroup editButtonsGroup;
        private RibbonGroup selectButtonsGroup;
        private RibbonButton selectButton;
        private RibbonGroup curveButtonsGroup;
        private RibbonButton curvePointButton;
        private RibbonButton segmentButton;
        private RibbonButton pointMatchButton;
        private RibbonGroup measureButtonsGroup;
        private RibbonButton measureButton;
        private RibbonGroup scaleButtonsGroup;
        private RibbonButton axisButton;
        private RibbonButton scaleButton;
    }
}