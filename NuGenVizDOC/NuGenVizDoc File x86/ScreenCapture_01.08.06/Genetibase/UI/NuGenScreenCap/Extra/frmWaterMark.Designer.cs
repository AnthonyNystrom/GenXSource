namespace Genetibase.UI.NuGenImageWorks
{
    partial class frmWaterMark
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ribbonButtonOp5Cancel = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonButtonOp5OK = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonButton8 = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonButton7 = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonPicOp1 = new System.Windows.Forms.PictureBox();
            this.btnWMIAlign = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.btnWMTAlign = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.btnWMFont = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.ribbonTextOp1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonPicOp1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.ribbonButtonOp5Cancel);
            this.panel1.Controls.Add(this.ribbonButtonOp5OK);
            this.panel1.Controls.Add(this.ribbonButton8);
            this.panel1.Controls.Add(this.ribbonButton7);
            this.panel1.Controls.Add(this.ribbonPicOp1);
            this.panel1.Controls.Add(this.btnWMIAlign);
            this.panel1.Controls.Add(this.btnWMTAlign);
            this.panel1.Controls.Add(this.btnWMFont);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ribbonTextOp1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(249, 97);
            this.panel1.TabIndex = 0;
            // 
            // ribbonButtonOp5Cancel
            // 
            this.ribbonButtonOp5Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5Cancel.Image = global::Genetibase.UI.Properties.Resources.Cancel;
            this.ribbonButtonOp5Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5Cancel.IsFlat = true;
            this.ribbonButtonOp5Cancel.IsPressed = false;
            this.ribbonButtonOp5Cancel.Location = new System.Drawing.Point(66, 67);
            this.ribbonButtonOp5Cancel.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5Cancel.Name = "ribbonButtonOp5Cancel";
            this.ribbonButtonOp5Cancel.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5Cancel.Size = new System.Drawing.Size(68, 24);
            this.ribbonButtonOp5Cancel.TabIndex = 42;
            this.ribbonButtonOp5Cancel.Text = "Cancel";
            this.ribbonButtonOp5Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5Cancel.Click += new System.EventHandler(this.ribbonButtonOp5Cancel_Click);
            // 
            // ribbonButtonOp5OK
            // 
            this.ribbonButtonOp5OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5OK.Image = global::Genetibase.UI.Properties.Resources.OK;
            this.ribbonButtonOp5OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5OK.IsFlat = true;
            this.ribbonButtonOp5OK.IsPressed = false;
            this.ribbonButtonOp5OK.Location = new System.Drawing.Point(6, 67);
            this.ribbonButtonOp5OK.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5OK.Name = "ribbonButtonOp5OK";
            this.ribbonButtonOp5OK.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5OK.Size = new System.Drawing.Size(49, 24);
            this.ribbonButtonOp5OK.TabIndex = 41;
            this.ribbonButtonOp5OK.Text = "OK";
            this.ribbonButtonOp5OK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5OK.Click += new System.EventHandler(this.ribbonButtonOp5OK_Click);
            // 
            // ribbonButton8
            // 
            this.ribbonButton8.Image = global::Genetibase.UI.Properties.Resources.Close_small;
            this.ribbonButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton8.IsFlat = true;
            this.ribbonButton8.IsPressed = false;
            this.ribbonButton8.Location = new System.Drawing.Point(163, 25);
            this.ribbonButton8.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton8.Name = "ribbonButton8";
            this.ribbonButton8.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton8.Size = new System.Drawing.Size(57, 18);
            this.ribbonButton8.TabIndex = 40;
            this.ribbonButton8.Text = "Clear";
            this.ribbonButton8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton8.Click += new System.EventHandler(this.ribbonButton8_Click);
            // 
            // ribbonButton7
            // 
            this.ribbonButton7.Image = global::Genetibase.UI.Properties.Resources.load_small;
            this.ribbonButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton7.IsFlat = true;
            this.ribbonButton7.IsPressed = false;
            this.ribbonButton7.Location = new System.Drawing.Point(163, 6);
            this.ribbonButton7.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton7.Name = "ribbonButton7";
            this.ribbonButton7.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton7.Size = new System.Drawing.Size(67, 18);
            this.ribbonButton7.TabIndex = 39;
            this.ribbonButton7.Text = "Browse";
            this.ribbonButton7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.ribbonButton7.Click += new System.EventHandler(this.ribbonButton7_Click);
            // 
            // ribbonPicOp1
            // 
            this.ribbonPicOp1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(240)))));
            this.ribbonPicOp1.Location = new System.Drawing.Point(123, 7);
            this.ribbonPicOp1.Margin = new System.Windows.Forms.Padding(0);
            this.ribbonPicOp1.Name = "ribbonPicOp1";
            this.ribbonPicOp1.Size = new System.Drawing.Size(36, 36);
            this.ribbonPicOp1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ribbonPicOp1.TabIndex = 38;
            this.ribbonPicOp1.TabStop = false;
            // 
            // btnWMIAlign
            // 
            this.btnWMIAlign.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWMIAlign.Image = global::Genetibase.UI.Properties.Resources.text_align_left;
            this.btnWMIAlign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWMIAlign.IsFlat = true;
            this.btnWMIAlign.IsPressed = false;
            this.btnWMIAlign.Location = new System.Drawing.Point(117, 47);
            this.btnWMIAlign.Margin = new System.Windows.Forms.Padding(1);
            this.btnWMIAlign.Name = "btnWMIAlign";
            this.btnWMIAlign.Padding = new System.Windows.Forms.Padding(2);
            this.btnWMIAlign.Size = new System.Drawing.Size(119, 18);
            this.btnWMIAlign.TabIndex = 37;
            this.btnWMIAlign.Text = "Image Alignment";
            this.btnWMIAlign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWMIAlign.Click += new System.EventHandler(this.btnWMIAlign_Click);
            // 
            // btnWMTAlign
            // 
            this.btnWMTAlign.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWMTAlign.Image = global::Genetibase.UI.Properties.Resources.text_align_left;
            this.btnWMTAlign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWMTAlign.IsFlat = true;
            this.btnWMTAlign.IsPressed = false;
            this.btnWMTAlign.Location = new System.Drawing.Point(6, 47);
            this.btnWMTAlign.Margin = new System.Windows.Forms.Padding(1);
            this.btnWMTAlign.Name = "btnWMTAlign";
            this.btnWMTAlign.Padding = new System.Windows.Forms.Padding(2);
            this.btnWMTAlign.Size = new System.Drawing.Size(106, 18);
            this.btnWMTAlign.TabIndex = 36;
            this.btnWMTAlign.Text = "Text Alignment";
            this.btnWMTAlign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWMTAlign.Click += new System.EventHandler(this.btnWMTAlign_Click);
            // 
            // btnWMFont
            // 
            this.btnWMFont.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWMFont.Image = global::Genetibase.UI.Properties.Resources.font;
            this.btnWMFont.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWMFont.IsFlat = true;
            this.btnWMFont.IsPressed = false;
            this.btnWMFont.Location = new System.Drawing.Point(6, 25);
            this.btnWMFont.Margin = new System.Windows.Forms.Padding(1);
            this.btnWMFont.Name = "btnWMFont";
            this.btnWMFont.Padding = new System.Windows.Forms.Padding(2);
            this.btnWMFont.Size = new System.Drawing.Size(55, 18);
            this.btnWMFont.TabIndex = 35;
            this.btnWMFont.Text = "Font";
            this.btnWMFont.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWMFont.Click += new System.EventHandler(this.btnWMFont_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Text";
            // 
            // ribbonTextOp1
            // 
            this.ribbonTextOp1.BackColor = System.Drawing.Color.White;
            this.ribbonTextOp1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ribbonTextOp1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbonTextOp1.ForeColor = System.Drawing.Color.SlateGray;
            this.ribbonTextOp1.Location = new System.Drawing.Point(37, 7);
            this.ribbonTextOp1.Name = "ribbonTextOp1";
            this.ribbonTextOp1.Size = new System.Drawing.Size(71, 14);
            this.ribbonTextOp1.TabIndex = 33;
            this.ribbonTextOp1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ribbonTextOp1.TextChanged += new System.EventHandler(this.ribbonTextOp1_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Image files|*.bmp;*.jpg;*.gif;*.tif;*.png|All files|*.*";
            this.openFileDialog1.Title = "Load";
            // 
            // frmWaterMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(259, 107);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWaterMark";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmWaterMark";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonPicOp1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Genetibase.UI.NuGenImageWorks.RibbonButton ribbonButton8;
        private Genetibase.UI.NuGenImageWorks.RibbonButton ribbonButton7;
        private System.Windows.Forms.PictureBox ribbonPicOp1;
        private Genetibase.UI.NuGenImageWorks.RibbonButton btnWMIAlign;
        private Genetibase.UI.NuGenImageWorks.RibbonButton btnWMTAlign;
        private Genetibase.UI.NuGenImageWorks.RibbonButton btnWMFont;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ribbonTextOp1;
        private Genetibase.UI.NuGenImageWorks.RibbonButton ribbonButtonOp5Cancel;
        private Genetibase.UI.NuGenImageWorks.RibbonButton ribbonButtonOp5OK;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}