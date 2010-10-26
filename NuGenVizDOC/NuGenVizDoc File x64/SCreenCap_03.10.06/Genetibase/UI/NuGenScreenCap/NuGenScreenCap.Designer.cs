namespace Genetibase.UI
{
    partial class NuGenScreenCap
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
            this.picBoxMain = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contrastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTransparency = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.sharpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFeather = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWash = new System.Windows.Forms.ToolStripMenuItem();
            this.grayScaleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDropShadow = new System.Windows.Forms.ToolStripMenuItem();
            this.bevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundEdgesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redrawTimer = new System.Timers.Timer();
            this.pnlEffects = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.trkEffects = new System.Windows.Forms.TrackBar();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redrawTimer)).BeginInit();
            this.pnlEffects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkEffects)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoxMain
            // 
            this.picBoxMain.ContextMenuStrip = this.contextMenuStrip1;
            this.picBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxMain.Location = new System.Drawing.Point(0, 0);
            this.picBoxMain.Name = "picBoxMain";
            this.picBoxMain.Size = new System.Drawing.Size(292, 266);
            this.picBoxMain.TabIndex = 0;
            this.picBoxMain.TabStop = false;
            this.picBoxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseDown);
            this.picBoxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseMove);
            this.picBoxMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picBoxMain_Paint);
            this.picBoxMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveAs,
            this.refreshToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.contrastMenuItem,
            this.brightnessMenuItem,
            this.mnuTransparency,
            this.toolStripSeparator3,
            this.sharpenMenuItem,
            this.blurToolStripMenuItem,
            this.toolStripSeparator4,
            this.mnuFeather,
            this.mnuWash,
            this.grayScaleMenuItem,
            this.mnuDropShadow,
            this.bevelToolStripMenuItem,
            this.roundEdgesToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 358);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(181, 22);
            this.mnuSaveAs.Text = "Save As";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.copyToolStripMenuItem.Text = "Capture and Return";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // contrastMenuItem
            // 
            this.contrastMenuItem.Name = "contrastMenuItem";
            this.contrastMenuItem.Size = new System.Drawing.Size(181, 22);
            this.contrastMenuItem.Text = "Contrast";
            this.contrastMenuItem.Click += new System.EventHandler(this.contrastMenuItem_Click);
            // 
            // brightnessMenuItem
            // 
            this.brightnessMenuItem.Name = "brightnessMenuItem";
            this.brightnessMenuItem.Size = new System.Drawing.Size(181, 22);
            this.brightnessMenuItem.Text = "Brightness";
            this.brightnessMenuItem.Click += new System.EventHandler(this.brightnessMenuItem_Click);
            // 
            // mnuTransparency
            // 
            this.mnuTransparency.Name = "mnuTransparency";
            this.mnuTransparency.Size = new System.Drawing.Size(181, 22);
            this.mnuTransparency.Text = "Transparency";
            this.mnuTransparency.Click += new System.EventHandler(this.mnuTransparency_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(178, 6);
            // 
            // sharpenMenuItem
            // 
            this.sharpenMenuItem.Name = "sharpenMenuItem";
            this.sharpenMenuItem.Size = new System.Drawing.Size(181, 22);
            this.sharpenMenuItem.Text = "Sharpen";
            this.sharpenMenuItem.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // blurToolStripMenuItem
            // 
            this.blurToolStripMenuItem.Name = "blurToolStripMenuItem";
            this.blurToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.blurToolStripMenuItem.Text = "Blur";
            this.blurToolStripMenuItem.Click += new System.EventHandler(this.blurToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(178, 6);
            // 
            // mnuFeather
            // 
            this.mnuFeather.Name = "mnuFeather";
            this.mnuFeather.Size = new System.Drawing.Size(181, 22);
            this.mnuFeather.Text = "Feather";
            this.mnuFeather.Click += new System.EventHandler(this.mnuFeather_Click);
            // 
            // mnuWash
            // 
            this.mnuWash.Name = "mnuWash";
            this.mnuWash.Size = new System.Drawing.Size(181, 22);
            this.mnuWash.Text = "Wash";
            this.mnuWash.Click += new System.EventHandler(this.mnuWash_Click);
            // 
            // grayScaleMenuItem
            // 
            this.grayScaleMenuItem.Name = "grayScaleMenuItem";
            this.grayScaleMenuItem.Size = new System.Drawing.Size(181, 22);
            this.grayScaleMenuItem.Text = "GrayScale";
            this.grayScaleMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // mnuDropShadow
            // 
            this.mnuDropShadow.Name = "mnuDropShadow";
            this.mnuDropShadow.Size = new System.Drawing.Size(181, 22);
            this.mnuDropShadow.Text = "Drop Shadow";
            this.mnuDropShadow.Click += new System.EventHandler(this.mnuDropShadow_Click);
            // 
            // bevelToolStripMenuItem
            // 
            this.bevelToolStripMenuItem.Name = "bevelToolStripMenuItem";
            this.bevelToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.bevelToolStripMenuItem.Text = "Bevel";
            this.bevelToolStripMenuItem.Click += new System.EventHandler(this.bevelToolStripMenuItem_Click);
            // 
            // roundEdgesToolStripMenuItem
            // 
            this.roundEdgesToolStripMenuItem.Name = "roundEdgesToolStripMenuItem";
            this.roundEdgesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.roundEdgesToolStripMenuItem.Text = "Round Edges";
            this.roundEdgesToolStripMenuItem.Click += new System.EventHandler(this.roundEdgesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // redrawTimer
            // 
            this.redrawTimer.Enabled = true;
            this.redrawTimer.SynchronizingObject = this;
            this.redrawTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.redrawTimer_Elapsed);
            // 
            // pnlEffects
            // 
            this.pnlEffects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEffects.Controls.Add(this.btnOK);
            this.pnlEffects.Controls.Add(this.trkEffects);
            this.pnlEffects.Location = new System.Drawing.Point(50, 83);
            this.pnlEffects.Name = "pnlEffects";
            this.pnlEffects.Size = new System.Drawing.Size(198, 28);
            this.pnlEffects.TabIndex = 1;
            this.pnlEffects.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(165, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(32, 27);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // trkEffects
            // 
            this.trkEffects.AutoSize = false;
            this.trkEffects.Location = new System.Drawing.Point(-1, 2);
            this.trkEffects.Name = "trkEffects";
            this.trkEffects.Size = new System.Drawing.Size(170, 27);
            this.trkEffects.TabIndex = 1;
            this.trkEffects.Scroll += new System.EventHandler(this.trkEffects_Scroll);
            // 
            // NuGenScreenCap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlEffects);
            this.Controls.Add(this.picBoxMain);
            this.DoubleBuffered = true;
            this.Name = "NuGenScreenCap";
            this.Size = new System.Drawing.Size(292, 266);
            this.Load += new System.EventHandler(this.NuGenScreenCap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.redrawTimer)).EndInit();
            this.pnlEffects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trkEffects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grayScaleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blurToolStripMenuItem;
        private System.Windows.Forms.Panel pnlEffects;
        private System.Windows.Forms.TrackBar trkEffects;
        private System.Windows.Forms.ToolStripMenuItem brightnessMenuItem;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolStripMenuItem contrastMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem sharpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuDropShadow;
        private System.Windows.Forms.ToolStripMenuItem mnuTransparency;
        private System.Windows.Forms.ToolStripMenuItem mnuWash;
        private System.Windows.Forms.ToolStripMenuItem mnuFeather;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
