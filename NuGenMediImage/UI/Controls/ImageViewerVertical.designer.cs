namespace Genetibase.NuGenMediImage.UI.Controls
{
    partial class ImageViewerVertical
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewerVertical));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mainRibbonGroup = new Genetibase.NuGenMediImage.UI.Controls.RibbonGroup();
            this.transPanel1 = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new Genetibase.NuGenMediImage.UI.Controls.VerticalProgressBar();
            this.outerOuterPanel = new Genetibase.NuGenMediImage.UI.Controls.FlickerFreePanel();
            this.outerPanel = new Genetibase.NuGenMediImage.UI.Controls.FlickerFreePanel();
            this.innerPanel = new Genetibase.NuGenMediImage.UI.Controls.FlickerFreePanel();
            this.picBoxClone = new Genetibase.NuGenMediImage.UI.Controls.RibbonItem();
            this.buttonDOWN = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.btnLeft = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.btnRight = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.hiddenRibbenGroup = new Genetibase.NuGenMediImage.UI.Controls.RibbonGroup();
            this.buttonUP = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.mainRibbonGroup.SuspendLayout();
            this.transPanel1.SuspendLayout();
            this.outerOuterPanel.SuspendLayout();
            this.outerPanel.SuspendLayout();
            this.innerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxClone)).BeginInit();
            this.hiddenRibbenGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainRibbonGroup
            // 
            this.mainRibbonGroup.BackColor = System.Drawing.Color.Transparent;
            this.mainRibbonGroup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainRibbonGroup.BackgroundImage")));
            this.mainRibbonGroup.Controls.Add(this.transPanel1);
            this.mainRibbonGroup.Controls.Add(this.outerOuterPanel);
            this.mainRibbonGroup.Controls.Add(this.buttonDOWN);
            this.mainRibbonGroup.Controls.Add(this.btnLeft);
            this.mainRibbonGroup.Controls.Add(this.btnRight);
            this.mainRibbonGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainRibbonGroup.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mainRibbonGroup.Location = new System.Drawing.Point(0, 0);
            this.mainRibbonGroup.Margin = new System.Windows.Forms.Padding(1);
            this.mainRibbonGroup.Name = "mainRibbonGroup";
            this.mainRibbonGroup.Size = new System.Drawing.Size(78, 435);
            this.mainRibbonGroup.TabIndex = 12;
            this.mainRibbonGroup.TabStop = false;
            this.mainRibbonGroup.SizeChanged += new System.EventHandler(this.mainRibbonGroup_SizeChanged);
            // 
            // transPanel1
            // 
            this.transPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.transPanel1.BackColor = System.Drawing.Color.Transparent;
            this.transPanel1.Controls.Add(this.lblProgress);
            this.transPanel1.Controls.Add(this.progressBar1);
            this.transPanel1.Location = new System.Drawing.Point(26, 38);
            this.transPanel1.MaximumSize = new System.Drawing.Size(42, 350);
            this.transPanel1.Name = "transPanel1";
            this.transPanel1.Size = new System.Drawing.Size(42, 350);
            this.transPanel1.TabIndex = 13;
            this.transPanel1.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.Location = new System.Drawing.Point(-1, 320);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(57, 27);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "Loading       Slices";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.BackColor = System.Drawing.Color.Transparent;
            this.progressBar1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.progressBar1.Color = System.Drawing.Color.DarkOrange;
            this.progressBar1.CustomBorderStyle = Genetibase.NuGenMediImage.UI.Controls.BorderStyles.None;
            this.progressBar1.ForeColor = System.Drawing.Color.DarkOrange;
            this.progressBar1.Location = new System.Drawing.Point(10, 3);
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(23, 309);
            this.progressBar1.Step = 10;
            this.progressBar1.Style = Genetibase.NuGenMediImage.UI.Controls.Styles.Solid;
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Value = 0;
            // 
            // outerOuterPanel
            // 
            this.outerOuterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outerOuterPanel.BackColor = System.Drawing.Color.Transparent;
            this.outerOuterPanel.Controls.Add(this.outerPanel);
            this.outerOuterPanel.Location = new System.Drawing.Point(18, 16);
            this.outerOuterPanel.Name = "outerOuterPanel";
            this.outerOuterPanel.Size = new System.Drawing.Size(60, 400);
            this.outerOuterPanel.TabIndex = 9;
            // 
            // outerPanel
            // 
            this.outerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outerPanel.BackColor = System.Drawing.Color.Transparent;
            this.outerPanel.Controls.Add(this.innerPanel);
            this.outerPanel.Location = new System.Drawing.Point(0, 0);
            this.outerPanel.Name = "outerPanel";
            this.outerPanel.Size = new System.Drawing.Size(60, 400);
            this.outerPanel.TabIndex = 9;
            this.outerPanel.Resize += new System.EventHandler(this.outerPanel_Resize);
            // 
            // innerPanel
            // 
            this.innerPanel.BackColor = System.Drawing.Color.Transparent;
            this.innerPanel.Controls.Add(this.picBoxClone);
            this.innerPanel.Location = new System.Drawing.Point(0, 0);
            this.innerPanel.Name = "innerPanel";
            this.innerPanel.Size = new System.Drawing.Size(60, 318);
            this.innerPanel.TabIndex = 8;
            // 
            // picBoxClone
            // 
            this.picBoxClone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(240)))));
            this.picBoxClone.IsPressed = false;
            this.picBoxClone.Location = new System.Drawing.Point(5, 5);
            this.picBoxClone.Margin = new System.Windows.Forms.Padding(0);
            this.picBoxClone.Name = "picBoxClone";
            this.picBoxClone.Size = new System.Drawing.Size(50, 50);
            this.picBoxClone.TabIndex = 0;
            this.picBoxClone.TabStop = false;
            // 
            // buttonDOWN
            // 
            this.buttonDOWN.BackColor = System.Drawing.Color.Transparent;
            this.buttonDOWN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDOWN.Image = ((System.Drawing.Image)(resources.GetObject("buttonDOWN.Image")));
            this.buttonDOWN.IsFlat = true;
            this.buttonDOWN.IsPressed = false;
            this.buttonDOWN.Location = new System.Drawing.Point(1, 181);
            this.buttonDOWN.Margin = new System.Windows.Forms.Padding(1);
            this.buttonDOWN.Name = "buttonDOWN";
            this.buttonDOWN.NgMediImage = null;
            this.buttonDOWN.Padding = new System.Windows.Forms.Padding(2);
            this.buttonDOWN.Size = new System.Drawing.Size(17, 24);
            this.buttonDOWN.TabIndex = 6;
            this.buttonDOWN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonDOWN.Click += new System.EventHandler(this.btnUpDown_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnLeft.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.IsFlat = true;
            this.btnLeft.IsPressed = false;
            this.btnLeft.Location = new System.Drawing.Point(35, 1);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(1);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.NgMediImage = null;
            this.btnLeft.Padding = new System.Windows.Forms.Padding(2);
            this.btnLeft.Size = new System.Drawing.Size(24, 16);
            this.btnLeft.TabIndex = 7;
            this.btnLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnLeftMouseDown);
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnLeftMouseUp);
            // 
            // btnRight
            // 
            this.btnRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRight.BackColor = System.Drawing.Color.Transparent;
            this.btnRight.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.IsFlat = true;
            this.btnRight.IsPressed = false;
            this.btnRight.Location = new System.Drawing.Point(35, 418);
            this.btnRight.Margin = new System.Windows.Forms.Padding(1);
            this.btnRight.Name = "btnRight";
            this.btnRight.NgMediImage = null;
            this.btnRight.Padding = new System.Windows.Forms.Padding(2);
            this.btnRight.Size = new System.Drawing.Size(24, 16);
            this.btnRight.TabIndex = 4;
            this.btnRight.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseDown);
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseUp);
            // 
            // hiddenRibbenGroup
            // 
            this.hiddenRibbenGroup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("hiddenRibbenGroup.BackgroundImage")));
            this.hiddenRibbenGroup.Controls.Add(this.buttonUP);
            this.hiddenRibbenGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hiddenRibbenGroup.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.hiddenRibbenGroup.Location = new System.Drawing.Point(0, 0);
            this.hiddenRibbenGroup.Margin = new System.Windows.Forms.Padding(0);
            this.hiddenRibbenGroup.Name = "hiddenRibbenGroup";
            this.hiddenRibbenGroup.Padding = new System.Windows.Forms.Padding(0);
            this.hiddenRibbenGroup.Size = new System.Drawing.Size(78, 435);
            this.hiddenRibbenGroup.TabIndex = 8;
            this.hiddenRibbenGroup.TabStop = false;
            this.hiddenRibbenGroup.SizeChanged += new System.EventHandler(this.hiddenRibbenGroup_SizeChanged);
            // 
            // buttonUP
            // 
            this.buttonUP.BackColor = System.Drawing.Color.Transparent;
            this.buttonUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUP.Image = ((System.Drawing.Image)(resources.GetObject("buttonUP.Image")));
            this.buttonUP.IsFlat = true;
            this.buttonUP.IsPressed = false;
            this.buttonUP.Location = new System.Drawing.Point(1, 181);
            this.buttonUP.Margin = new System.Windows.Forms.Padding(1);
            this.buttonUP.Name = "buttonUP";
            this.buttonUP.NgMediImage = null;
            this.buttonUP.Padding = new System.Windows.Forms.Padding(2);
            this.buttonUP.Size = new System.Drawing.Size(17, 24);
            this.buttonUP.TabIndex = 7;
            this.buttonUP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonUP.Click += new System.EventHandler(this.btnUpDown_Click);
            // 
            // ImageViewerVertical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.mainRibbonGroup);
            this.Controls.Add(this.hiddenRibbenGroup);
            this.Name = "ImageViewerVertical";
            this.Size = new System.Drawing.Size(78, 435);
            this.Load += new System.EventHandler(this.ImageViewerVertical_Load);
            this.mainRibbonGroup.ResumeLayout(false);
            this.transPanel1.ResumeLayout(false);
            this.outerOuterPanel.ResumeLayout(false);
            this.outerPanel.ResumeLayout(false);
            this.innerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxClone)).EndInit();
            this.hiddenRibbenGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RibbonButton btnLeft;
        private RibbonButton buttonDOWN;
        private RibbonItem picBoxClone;
        private RibbonButton btnRight;
        private RibbonGroup mainRibbonGroup;
        
        
        private RibbonGroup hiddenRibbenGroup;
        private RibbonButton buttonUP;
        private Genetibase.NuGenMediImage.UI.Controls.FlickerFreePanel outerPanel;
        private Genetibase.NuGenMediImage.UI.Controls.FlickerFreePanel outerOuterPanel;
        private Genetibase.NuGenMediImage.UI.Controls.FlickerFreePanel innerPanel;
        private System.Windows.Forms.Panel transPanel1;
        private System.Windows.Forms.Label lblProgress;
        private Genetibase.NuGenMediImage.UI.Controls.VerticalProgressBar progressBar1;
        private System.Windows.Forms.ToolTip toolTip1;


    }
}
