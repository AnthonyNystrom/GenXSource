namespace NormIllumMethods.Visual
{
    partial class Visual
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Visual));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMethods = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMultiscaleRetinex = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIsotropic = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAnisotropic = new System.Windows.Forms.ToolStripMenuItem();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.contMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contMnuMultRetinex = new System.Windows.Forms.ToolStripMenuItem();
            this.contMnuIsotropic = new System.Windows.Forms.ToolStripMenuItem();
            this.contMnuAnisotropic = new System.Windows.Forms.ToolStripMenuItem();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSave = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picOriginal = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.picFiltered = new System.Windows.Forms.PictureBox();
            this.labInitial = new System.Windows.Forms.Label();
            this.labNormalised = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.contMnu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFiltered)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuMethods});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(810, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenImage,
            this.mnuSaveImage,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(35, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuOpenImage
            // 
            this.mnuOpenImage.Image = global::Visual.Properties.Resources.folder_out;
            this.mnuOpenImage.Name = "mnuOpenImage";
            this.mnuOpenImage.Size = new System.Drawing.Size(144, 22);
            this.mnuOpenImage.Text = "Open Image";
            this.mnuOpenImage.Click += new System.EventHandler(this.mnuOpenImage_Click);
            // 
            // mnuSaveImage
            // 
            this.mnuSaveImage.Image = global::Visual.Properties.Resources.disk;
            this.mnuSaveImage.Name = "mnuSaveImage";
            this.mnuSaveImage.Size = new System.Drawing.Size(144, 22);
            this.mnuSaveImage.Text = "Save Image";
            this.mnuSaveImage.Visible = false;
            this.mnuSaveImage.Click += new System.EventHandler(this.mnuSaveImage_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Image = global::Visual.Properties.Resources.close;
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(144, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuMethods
            // 
            this.mnuMethods.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMultiscaleRetinex,
            this.mnuIsotropic,
            this.mnuAnisotropic});
            this.mnuMethods.Name = "mnuMethods";
            this.mnuMethods.Size = new System.Drawing.Size(60, 20);
            this.mnuMethods.Text = "Methods";
            this.mnuMethods.Visible = false;
            // 
            // mnuMultiscaleRetinex
            // 
            this.mnuMultiscaleRetinex.Name = "mnuMultiscaleRetinex";
            this.mnuMultiscaleRetinex.Size = new System.Drawing.Size(197, 22);
            this.mnuMultiscaleRetinex.Text = "Multiscale Retinex";
            this.mnuMultiscaleRetinex.Click += new System.EventHandler(this.mnuMultiscaleRetinex_Click);
            // 
            // mnuIsotropic
            // 
            this.mnuIsotropic.Name = "mnuIsotropic";
            this.mnuIsotropic.Size = new System.Drawing.Size(197, 22);
            this.mnuIsotropic.Text = "Isotropic Smoothing";
            this.mnuIsotropic.Click += new System.EventHandler(this.mnuIsotropic_Click);
            // 
            // mnuAnisotropic
            // 
            this.mnuAnisotropic.Name = "mnuAnisotropic";
            this.mnuAnisotropic.Size = new System.Drawing.Size(197, 22);
            this.mnuAnisotropic.Text = "Anisotrophic Smoothing";
            this.mnuAnisotropic.Click += new System.EventHandler(this.mnuAnisotropic_Click);
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            this.ofd.Filter = resources.GetString("ofd.Filter");
            this.ofd.FilterIndex = 10;
            // 
            // contMnu
            // 
            this.contMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contMnuMultRetinex,
            this.contMnuIsotropic,
            this.contMnuAnisotropic});
            this.contMnu.Name = "contMnu";
            this.contMnu.Size = new System.Drawing.Size(192, 70);
            // 
            // contMnuMultRetinex
            // 
            this.contMnuMultRetinex.Name = "contMnuMultRetinex";
            this.contMnuMultRetinex.Size = new System.Drawing.Size(191, 22);
            this.contMnuMultRetinex.Text = "Multiscale Retinex";
            this.contMnuMultRetinex.Click += new System.EventHandler(this.contMnuMultRetinex_Click);
            // 
            // contMnuIsotropic
            // 
            this.contMnuIsotropic.Name = "contMnuIsotropic";
            this.contMnuIsotropic.Size = new System.Drawing.Size(191, 22);
            this.contMnuIsotropic.Text = "Isotropic Smoothing";
            this.contMnuIsotropic.Click += new System.EventHandler(this.contMnuIsotropic_Click);
            // 
            // contMnuAnisotropic
            // 
            this.contMnuAnisotropic.Name = "contMnuAnisotropic";
            this.contMnuAnisotropic.Size = new System.Drawing.Size(191, 22);
            this.contMnuAnisotropic.Text = "Anisotropic Smoothing";
            this.contMnuAnisotropic.Click += new System.EventHandler(this.contMnuAnisotropic_Click);
            // 
            // sfd
            // 
            this.sfd.Filter = "Windows Bitmap (*.BMP)|*.BMP|Graphics Interchange Format (*.GIF)|*.GIF|Joint Phot" +
                "ographic Experts Group (*.JPG;*.JIF;*.JPEG)|*.JPG;*.JIF;*.JPEG|Portable Network " +
                "Graphics(*.PNG)|*.PNG";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpen,
            this.toolStripSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(810, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOpen.Image")));
            this.toolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripOpen.Text = "toolStripButton1";
            this.toolStripOpen.ToolTipText = "Open Image";
            this.toolStripOpen.Click += new System.EventHandler(this.toolStripOpen_Click);
            // 
            // toolStripSave
            // 
            this.toolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSave.Image")));
            this.toolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripSave.Text = "Save filtered image";
            this.toolStripSave.Visible = false;
            this.toolStripSave.Click += new System.EventHandler(this.toolStripSave_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.picOriginal);
            this.panel1.Location = new System.Drawing.Point(12, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 400);
            this.panel1.TabIndex = 4;
            // 
            // picOriginal
            // 
            this.picOriginal.ContextMenuStrip = this.contMnu;
            this.picOriginal.Location = new System.Drawing.Point(0, 0);
            this.picOriginal.Name = "picOriginal";
            this.picOriginal.Size = new System.Drawing.Size(100, 50);
            this.picOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picOriginal.TabIndex = 0;
            this.picOriginal.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.picFiltered);
            this.panel2.Location = new System.Drawing.Point(395, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(403, 400);
            this.panel2.TabIndex = 5;
            // 
            // picFiltered
            // 
            this.picFiltered.Location = new System.Drawing.Point(0, 0);
            this.picFiltered.Name = "picFiltered";
            this.picFiltered.Size = new System.Drawing.Size(100, 50);
            this.picFiltered.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picFiltered.TabIndex = 0;
            this.picFiltered.TabStop = false;
            // 
            // labInitial
            // 
            this.labInitial.AutoSize = true;
            this.labInitial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labInitial.Location = new System.Drawing.Point(24, 56);
            this.labInitial.Name = "labInitial";
            this.labInitial.Size = new System.Drawing.Size(38, 13);
            this.labInitial.TabIndex = 6;
            this.labInitial.Text = "Initial";
            this.labInitial.Visible = false;
            // 
            // labNormalised
            // 
            this.labNormalised.AutoSize = true;
            this.labNormalised.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labNormalised.Location = new System.Drawing.Point(401, 56);
            this.labNormalised.Name = "labNormalised";
            this.labNormalised.Size = new System.Drawing.Size(69, 13);
            this.labNormalised.TabIndex = 7;
            this.labNormalised.Text = "Normalised";
            this.labNormalised.Visible = false;
            // 
            // Visual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(810, 482);
            this.Controls.Add(this.labNormalised);
            this.Controls.Add(this.labInitial);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Visual";
            this.Text = "Photometric Normalisation Algorithms";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contMnu.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFiltered)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenImage;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveImage;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuMethods;
        private System.Windows.Forms.ToolStripMenuItem mnuMultiscaleRetinex;
        private System.Windows.Forms.ToolStripMenuItem mnuIsotropic;
        private System.Windows.Forms.ToolStripMenuItem mnuAnisotropic;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.ContextMenuStrip contMnu;
        private System.Windows.Forms.ToolStripMenuItem contMnuMultRetinex;
        private System.Windows.Forms.ToolStripMenuItem contMnuIsotropic;
        private System.Windows.Forms.ToolStripMenuItem contMnuAnisotropic;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.ToolStripButton toolStripSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picOriginal;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picFiltered;
        private System.Windows.Forms.Label labInitial;
        private System.Windows.Forms.Label labNormalised;
    }
}

