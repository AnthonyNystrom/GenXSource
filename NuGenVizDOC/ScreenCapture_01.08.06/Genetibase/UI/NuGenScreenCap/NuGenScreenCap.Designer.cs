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
                
                if( this.selectedImage != null )
                    this.selectedImage.Dispose();

                if (this.selectedImageNoEffects != null)
                    this.selectedImageNoEffects.Dispose();

                if( b!= null )
                    b.Dispose();
                
                if( p!= null )
                    p.Dispose();

                if( p2 != null )
                    p2.Dispose();

                if( coordsBrush != null )
                    coordsBrush.Dispose();

                b = null;
                p = null;
                p2 = null;
                coordsBrush = null;

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
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contrastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTransparency = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.sharpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFlip = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFishEye = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWaterMark = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFloorReflection = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBox = new System.Windows.Forms.ToolStripMenuItem();
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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
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
            this.refreshToolStripMenuItem,
            this.mnuCopy,
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.contrastMenuItem,
            this.brightnessMenuItem,
            this.mnuTransparency,
            this.toolStripSeparator3,
            this.sharpenMenuItem,
            this.blurToolStripMenuItem,
            this.toolStripSeparator6,
            this.mnuRotate,
            this.mnuFlip,
            this.toolStripSeparator5,
            this.mnuFishEye,
            this.mnuWaterMark,
            this.mnuFloorReflection,
            this.mnuBox,
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 524);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.copyToolStripMenuItem.Text = "Save As";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // contrastMenuItem
            // 
            this.contrastMenuItem.Name = "contrastMenuItem";
            this.contrastMenuItem.Size = new System.Drawing.Size(160, 22);
            this.contrastMenuItem.Text = "Contrast";
            this.contrastMenuItem.Click += new System.EventHandler(this.contrastMenuItem_Click);
            // 
            // brightnessMenuItem
            // 
            this.brightnessMenuItem.Name = "brightnessMenuItem";
            this.brightnessMenuItem.Size = new System.Drawing.Size(160, 22);
            this.brightnessMenuItem.Text = "Brightness";
            this.brightnessMenuItem.Click += new System.EventHandler(this.brightnessMenuItem_Click);
            // 
            // mnuTransparency
            // 
            this.mnuTransparency.Name = "mnuTransparency";
            this.mnuTransparency.Size = new System.Drawing.Size(160, 22);
            this.mnuTransparency.Text = "Transparency";
            this.mnuTransparency.Click += new System.EventHandler(this.mnuTransparency_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(157, 6);
            // 
            // sharpenMenuItem
            // 
            this.sharpenMenuItem.Name = "sharpenMenuItem";
            this.sharpenMenuItem.Size = new System.Drawing.Size(160, 22);
            this.sharpenMenuItem.Text = "Sharpen";
            this.sharpenMenuItem.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // blurToolStripMenuItem
            // 
            this.blurToolStripMenuItem.Name = "blurToolStripMenuItem";
            this.blurToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.blurToolStripMenuItem.Text = "Blur";
            this.blurToolStripMenuItem.Click += new System.EventHandler(this.blurToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuRotate
            // 
            this.mnuRotate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftToolStripMenuItem,
            this.rightToolStripMenuItem});
            this.mnuRotate.Name = "mnuRotate";
            this.mnuRotate.Size = new System.Drawing.Size(160, 22);
            this.mnuRotate.Text = "Rotate";
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Image = global::Genetibase.UI.Properties.Resources.RotateRight;
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.leftToolStripMenuItem.Text = "Left";
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Image = global::Genetibase.UI.Properties.Resources.RotateLeft;
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.rightToolStripMenuItem.Text = "Right";
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            // 
            // mnuFlip
            // 
            this.mnuFlip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verticalToolStripMenuItem,
            this.horizontalToolStripMenuItem});
            this.mnuFlip.Name = "mnuFlip";
            this.mnuFlip.Size = new System.Drawing.Size(160, 22);
            this.mnuFlip.Text = "Flip";
            // 
            // verticalToolStripMenuItem
            // 
            this.verticalToolStripMenuItem.Image = global::Genetibase.UI.Properties.Resources.flipver;
            this.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            this.verticalToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.verticalToolStripMenuItem.Text = "Vertical";
            this.verticalToolStripMenuItem.Click += new System.EventHandler(this.verticalToolStripMenuItem_Click);
            // 
            // horizontalToolStripMenuItem
            // 
            this.horizontalToolStripMenuItem.Image = global::Genetibase.UI.Properties.Resources.fliphor;
            this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.horizontalToolStripMenuItem.Text = "Horizontal";
            this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.horizontalToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuFishEye
            // 
            this.mnuFishEye.Name = "mnuFishEye";
            this.mnuFishEye.Size = new System.Drawing.Size(160, 22);
            this.mnuFishEye.Text = "Fish Eye";
            this.mnuFishEye.Click += new System.EventHandler(this.MnuFishEyeClick);
            // 
            // mnuWaterMark
            // 
            this.mnuWaterMark.Name = "mnuWaterMark";
            this.mnuWaterMark.Size = new System.Drawing.Size(160, 22);
            this.mnuWaterMark.Text = "Water Mark";
            this.mnuWaterMark.Click += new System.EventHandler(this.mnuWaterMark_Click);
            // 
            // mnuFloorReflection
            // 
            this.mnuFloorReflection.Name = "mnuFloorReflection";
            this.mnuFloorReflection.Size = new System.Drawing.Size(160, 22);
            this.mnuFloorReflection.Text = "Floor Reflection";
            this.mnuFloorReflection.Click += new System.EventHandler(this.MnuFloorReflectionClick);
            // 
            // mnuBox
            // 
            this.mnuBox.Name = "mnuBox";
            this.mnuBox.Size = new System.Drawing.Size(160, 22);
            this.mnuBox.Text = "Box";
            this.mnuBox.Click += new System.EventHandler(this.MnuBoxClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuFeather
            // 
            this.mnuFeather.Name = "mnuFeather";
            this.mnuFeather.Size = new System.Drawing.Size(160, 22);
            this.mnuFeather.Text = "Feather";
            this.mnuFeather.Click += new System.EventHandler(this.mnuFeather_Click);
            // 
            // mnuWash
            // 
            this.mnuWash.Name = "mnuWash";
            this.mnuWash.Size = new System.Drawing.Size(160, 22);
            this.mnuWash.Text = "Wash";
            this.mnuWash.Click += new System.EventHandler(this.mnuWash_Click);
            // 
            // grayScaleMenuItem
            // 
            this.grayScaleMenuItem.Name = "grayScaleMenuItem";
            this.grayScaleMenuItem.Size = new System.Drawing.Size(160, 22);
            this.grayScaleMenuItem.Text = "GrayScale";
            this.grayScaleMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // mnuDropShadow
            // 
            this.mnuDropShadow.Name = "mnuDropShadow";
            this.mnuDropShadow.Size = new System.Drawing.Size(160, 22);
            this.mnuDropShadow.Text = "Drop Shadow";
            this.mnuDropShadow.Click += new System.EventHandler(this.mnuDropShadow_Click);
            // 
            // bevelToolStripMenuItem
            // 
            this.bevelToolStripMenuItem.Name = "bevelToolStripMenuItem";
            this.bevelToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.bevelToolStripMenuItem.Text = "Bevel";
            this.bevelToolStripMenuItem.Click += new System.EventHandler(this.bevelToolStripMenuItem_Click);
            // 
            // roundEdgesToolStripMenuItem
            // 
            this.roundEdgesToolStripMenuItem.Name = "roundEdgesToolStripMenuItem";
            this.roundEdgesToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.roundEdgesToolStripMenuItem.Text = "Round Edges";
            this.roundEdgesToolStripMenuItem.Click += new System.EventHandler(this.roundEdgesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
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
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Bitmap Files|*.bmp|Jpeg Files|*.jpeg;*.jpg|Png Files|*.png|Gif Files|*.gif|Tiff F" +
                "iles|*.tiff;*.tif|Icon Files|*.ico";
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(160, 22);
            this.mnuCopy.Text = "Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // NuGenScreenCap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlEffects);
            this.Controls.Add(this.picBoxMain);
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
        private System.Windows.Forms.ToolStripMenuItem horizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuFlip;
        private System.Windows.Forms.ToolStripMenuItem mnuRotate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mnuFloorReflection;
        private System.Windows.Forms.ToolStripMenuItem mnuFishEye;
        private System.Windows.Forms.ToolStripMenuItem mnuBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;

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
        private System.Windows.Forms.ToolStripMenuItem mnuWaterMark;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        
        
    }
}
