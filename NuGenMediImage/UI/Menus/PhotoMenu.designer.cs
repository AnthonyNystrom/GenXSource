using Genetibase.NuGenMediImage.UI.Controls;

namespace Genetibase.NuGenMediImage.UI.Menus
{
    partial class PhotoMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoMenu));
            this.btnLoad = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSaveAs = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.btnClose = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.btnPrint = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnHelp = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.ribbonButton6 = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.btnExit = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.savePictureFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.ForeColor = System.Drawing.Color.Black;
            this.btnLoad.Image = global::Genetibase.NuGenMediImage.Properties.Resources.load;
            this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoad.IsFlat = true;
            this.btnLoad.IsPressed = false;
            this.btnLoad.Location = new System.Drawing.Point(1, 1);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(1);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.NgMediImage = null;
            this.btnLoad.Padding = new System.Windows.Forms.Padding(2);
            this.btnLoad.Size = new System.Drawing.Size(125, 39);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load...";
            this.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.flowLayoutPanel1.Controls.Add(this.btnLoad);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox1);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveAs);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Controls.Add(this.btnPrint);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox2);
            this.flowLayoutPanel1.Controls.Add(this.btnHelp);
            this.flowLayoutPanel1.Controls.Add(this.ribbonButton6);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox3);
            this.flowLayoutPanel1.Controls.Add(this.btnExit);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(127, 324);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(3, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(121, 1);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveAs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAs.ForeColor = System.Drawing.Color.Black;
            this.btnSaveAs.Image = global::Genetibase.NuGenMediImage.Properties.Resources.save_as;
            this.btnSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveAs.IsFlat = true;
            this.btnSaveAs.IsPressed = false;
            this.btnSaveAs.Location = new System.Drawing.Point(1, 49);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(1);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.NgMediImage = null;
            this.btnSaveAs.Padding = new System.Windows.Forms.Padding(2);
            this.btnSaveAs.Size = new System.Drawing.Size(125, 39);
            this.btnSaveAs.TabIndex = 13;
            this.btnSaveAs.Text = "Save As...";
            this.btnSaveAs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Image = global::Genetibase.NuGenMediImage.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.IsFlat = true;
            this.btnClose.IsPressed = false;
            this.btnClose.Location = new System.Drawing.Point(1, 90);
            this.btnClose.Margin = new System.Windows.Forms.Padding(1);
            this.btnClose.Name = "btnClose";
            this.btnClose.NgMediImage = null;
            this.btnClose.Padding = new System.Windows.Forms.Padding(2);
            this.btnClose.Size = new System.Drawing.Size(125, 39);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrint.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Image = global::Genetibase.NuGenMediImage.Properties.Resources.exit;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.IsFlat = true;
            this.btnPrint.IsPressed = false;
            this.btnPrint.Location = new System.Drawing.Point(1, 131);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.NgMediImage = null;
            this.btnPrint.Padding = new System.Windows.Forms.Padding(2);
            this.btnPrint.Size = new System.Drawing.Size(125, 39);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Silver;
            this.pictureBox2.Location = new System.Drawing.Point(3, 174);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(121, 1);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // btnHelp
            // 
            this.btnHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Image = global::Genetibase.NuGenMediImage.Properties.Resources.help2;
            this.btnHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHelp.IsFlat = true;
            this.btnHelp.IsPressed = false;
            this.btnHelp.Location = new System.Drawing.Point(1, 179);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(1);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.NgMediImage = null;
            this.btnHelp.Padding = new System.Windows.Forms.Padding(2);
            this.btnHelp.Size = new System.Drawing.Size(125, 39);
            this.btnHelp.TabIndex = 15;
            this.btnHelp.Text = "Help";
            this.btnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ribbonButton6
            // 
            this.ribbonButton6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonButton6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbonButton6.ForeColor = System.Drawing.Color.Black;
            this.ribbonButton6.Image = global::Genetibase.NuGenMediImage.Properties.Resources.about;
            this.ribbonButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton6.IsFlat = true;
            this.ribbonButton6.IsPressed = false;
            this.ribbonButton6.Location = new System.Drawing.Point(1, 220);
            this.ribbonButton6.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton6.Name = "ribbonButton6";
            this.ribbonButton6.NgMediImage = null;
            this.ribbonButton6.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton6.Size = new System.Drawing.Size(125, 39);
            this.ribbonButton6.TabIndex = 16;
            this.ribbonButton6.Text = "About";
            this.ribbonButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Silver;
            this.pictureBox3.Location = new System.Drawing.Point(3, 263);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(121, 1);
            this.pictureBox3.TabIndex = 17;
            this.pictureBox3.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = global::Genetibase.NuGenMediImage.Properties.Resources.exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.IsFlat = true;
            this.btnExit.IsPressed = false;
            this.btnExit.Location = new System.Drawing.Point(1, 268);
            this.btnExit.Margin = new System.Windows.Forms.Padding(1);
            this.btnExit.Name = "btnExit";
            this.btnExit.NgMediImage = null;
            this.btnExit.Padding = new System.Windows.Forms.Padding(2);
            this.btnExit.Size = new System.Drawing.Size(125, 39);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Bitmap files|*.bmp|Jpeg files|*.jpg|Gif files|*.gif|Tiff files|*.tif|Png files|*." +
                "png|All files|*.*";
            this.saveFileDialog1.Title = "Save";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = resources.GetString("openFileDialog1.Filter");
            // 
            // savePictureFileDialog
            // 
            this.savePictureFileDialog.Filter = "JPEG files(*.jpg;*.jpeg)|*.jpg;*.jpeg|Bitmap files(*.bmp)|*.bmp|Annotation files(" +
                "*.anno)|*.anno";
            // 
            // PhotoMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(137, 334);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "PhotoMenu";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "PhotoMenu";
            this.Deactivate += new System.EventHandler(this.PhotoMenu_Deactivate);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PhotoMenu_Paint);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private RibbonButton btnSaveAs;
        private RibbonButton btnExit;
        private System.Windows.Forms.PictureBox pictureBox2;
        private RibbonButton btnHelp;
        private RibbonButton ribbonButton6;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog savePictureFileDialog;
        private RibbonButton btnPrint;
        private RibbonButton btnLoad;
        private RibbonButton btnClose;

    }
}
