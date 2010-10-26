namespace Genetibase.NuGenMediImage.UI.Dialogs
{
    partial class DlgEmboss
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ribbonGroup2 = new Genetibase.NuGenMediImage.UI.Controls.RibbonGroup();
            this.btnCancel = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.btnOK = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.trkEmboss = new Genetibase.NuGenMediImage.UI.Controls.ColorTrackBar();
            this.ribbonLabel3 = new Genetibase.NuGenMediImage.UI.Controls.RibbonLabel();
            this.ribbonLabel2 = new Genetibase.NuGenMediImage.UI.Controls.RibbonLabel();
            this.ribbonLabel1 = new Genetibase.NuGenMediImage.UI.Controls.RibbonLabel();
            this.pOrig = new Genetibase.NuGenMediImage.UI.Controls.CustomPictureBox();
            this.pProc = new Genetibase.NuGenMediImage.UI.Controls.CustomPictureBox();
            this.ribbonGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pOrig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pProc)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonGroup2
            // 
            this.ribbonGroup2.Controls.Add(this.btnCancel);
            this.ribbonGroup2.Controls.Add(this.btnOK);
            this.ribbonGroup2.Controls.Add(this.trkEmboss);
            this.ribbonGroup2.Controls.Add(this.ribbonLabel3);
            this.ribbonGroup2.Controls.Add(this.ribbonLabel2);
            this.ribbonGroup2.Controls.Add(this.ribbonLabel1);
            this.ribbonGroup2.Controls.Add(this.pOrig);
            this.ribbonGroup2.Controls.Add(this.pProc);
            this.ribbonGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonGroup2.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.ribbonGroup2.Location = new System.Drawing.Point(0, 0);
            this.ribbonGroup2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup2.Name = "ribbonGroup2";
            this.ribbonGroup2.Size = new System.Drawing.Size(327, 255);
            this.ribbonGroup2.TabIndex = 49;
            this.ribbonGroup2.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.ImageIndex = 9;
            this.btnCancel.IsFlat = true;
            this.btnCancel.IsPressed = false;
            this.btnCancel.Location = new System.Drawing.Point(251, 211);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(2);
            this.btnCancel.Size = new System.Drawing.Size(48, 22);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.ImageIndex = 9;
            this.btnOK.IsFlat = true;
            this.btnOK.IsPressed = false;
            this.btnOK.Location = new System.Drawing.Point(197, 211);
            this.btnOK.Margin = new System.Windows.Forms.Padding(1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Windows.Forms.Padding(2);
            this.btnOK.Size = new System.Drawing.Size(46, 22);
            this.btnOK.TabIndex = 56;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // trkEmboss
            // 
            this.trkEmboss.BarBorderColor = System.Drawing.Color.Black;
            this.trkEmboss.BarColor = System.Drawing.Color.LightGray;
            this.trkEmboss.BarOrientation = Genetibase.NuGenMediImage.UI.Controls.Orientations.Horizontal;
            this.trkEmboss.ControlCornerStyle = Genetibase.NuGenMediImage.UI.Controls.CornerStyles.Square;
            this.trkEmboss.Location = new System.Drawing.Point(81, 181);
            this.trkEmboss.Maximum = 255;
            this.trkEmboss.MaximumValueSide = Genetibase.NuGenMediImage.UI.Controls.Poles.Right;
            this.trkEmboss.Minimum = 0;
            this.trkEmboss.Name = "trkEmboss";
            this.trkEmboss.Size = new System.Drawing.Size(223, 15);
            this.trkEmboss.TabIndex = 54;
            this.trkEmboss.TrackerBorderColor = System.Drawing.Color.Gray;
            this.trkEmboss.TrackerColor = System.Drawing.Color.Gray;
            this.trkEmboss.TrackerSize = 16;
            this.trkEmboss.Value = 0;
            this.trkEmboss.ValueChanged += new Genetibase.NuGenMediImage.UI.Controls.ColorTrackBar.ValueChangedEventHandler(this.trkEmboss_ValueChanged);
            // 
            // ribbonLabel3
            // 
            this.ribbonLabel3.Location = new System.Drawing.Point(18, 178);
            this.ribbonLabel3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonLabel3.Name = "ribbonLabel3";
            this.ribbonLabel3.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonLabel3.Size = new System.Drawing.Size(59, 18);
            this.ribbonLabel3.TabIndex = 53;
            this.ribbonLabel3.Text = "Emboss";
            this.ribbonLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ribbonLabel2
            // 
            this.ribbonLabel2.Location = new System.Drawing.Point(177, 138);
            this.ribbonLabel2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonLabel2.Name = "ribbonLabel2";
            this.ribbonLabel2.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonLabel2.Size = new System.Drawing.Size(128, 17);
            this.ribbonLabel2.TabIndex = 52;
            this.ribbonLabel2.Text = "New Image";
            this.ribbonLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ribbonLabel1
            // 
            this.ribbonLabel1.Location = new System.Drawing.Point(21, 138);
            this.ribbonLabel1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonLabel1.Name = "ribbonLabel1";
            this.ribbonLabel1.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonLabel1.Size = new System.Drawing.Size(128, 17);
            this.ribbonLabel1.TabIndex = 51;
            this.ribbonLabel1.Text = "Original Image";
            this.ribbonLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pOrig
            // 
            this.pOrig.Image = null;
            this.pOrig.Location = new System.Drawing.Point(21, 6);
            this.pOrig.Name = "pOrig";
            this.pOrig.Size = new System.Drawing.Size(128, 128);
            this.pOrig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pOrig.TabIndex = 50;
            this.pOrig.TabStop = false;
            // 
            // pProc
            // 
            this.pProc.Image = null;
            this.pProc.Location = new System.Drawing.Point(176, 6);
            this.pProc.Name = "pProc";
            this.pProc.Size = new System.Drawing.Size(128, 128);
            this.pProc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pProc.TabIndex = 49;
            this.pProc.TabStop = false;
            // 
            // DlgEmboss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(327, 255);
            this.Controls.Add(this.ribbonGroup2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgEmboss";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Emboss";
            this.ribbonGroup2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pOrig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pProc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Genetibase.NuGenMediImage.UI.Controls.RibbonGroup ribbonGroup2;
        private Genetibase.NuGenMediImage.UI.Controls.RibbonButton btnCancel;
        private Genetibase.NuGenMediImage.UI.Controls.RibbonButton btnOK;
        private Genetibase.NuGenMediImage.UI.Controls.ColorTrackBar trkEmboss;
        private Genetibase.NuGenMediImage.UI.Controls.RibbonLabel ribbonLabel3;
        private Genetibase.NuGenMediImage.UI.Controls.RibbonLabel ribbonLabel2;
        private Genetibase.NuGenMediImage.UI.Controls.RibbonLabel ribbonLabel1;
        private Genetibase.NuGenMediImage.UI.Controls.CustomPictureBox pOrig;
        private Genetibase.NuGenMediImage.UI.Controls.CustomPictureBox pProc;

    }
}