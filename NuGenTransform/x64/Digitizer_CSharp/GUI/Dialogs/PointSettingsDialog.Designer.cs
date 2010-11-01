using WCL;
namespace Genetibase.NuGenTransform
{
    partial class PointSettingsDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pointInteriorColorCombo = new System.Windows.Forms.ComboBox();
            this.pointLineColorCombo = new System.Windows.Forms.ComboBox();
            this.pointLineSizeCombo = new System.Windows.Forms.ComboBox();
            this.sizeCombo = new System.Windows.Forms.ComboBox();
            this.shapeCombo = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lineConnectAsCombo = new System.Windows.Forms.ComboBox();
            this.lineColorCombo = new System.Windows.Forms.ComboBox();
            this.lineSizeCombo = new System.Windows.Forms.ComboBox();
            this.button1 = new VistaButton();
            this.button2 = new VistaButton();
            this.preview = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pointInteriorColorCombo);
            this.groupBox1.Controls.Add(this.pointLineColorCombo);
            this.groupBox1.Controls.Add(this.pointLineSizeCombo);
            this.groupBox1.Controls.Add(this.sizeCombo);
            this.groupBox1.Controls.Add(this.shapeCombo);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 165);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Point";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Interior Color";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Line Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Line Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Shape";
            // 
            // pointInteriorColorCombo
            // 
            this.pointInteriorColorCombo.FormattingEnabled = true;
            this.pointInteriorColorCombo.Location = new System.Drawing.Point(114, 127);
            this.pointInteriorColorCombo.Name = "pointInteriorColorCombo";
            this.pointInteriorColorCombo.Size = new System.Drawing.Size(121, 21);
            this.pointInteriorColorCombo.TabIndex = 0;
            this.pointInteriorColorCombo.SelectedIndexChanged += new System.EventHandler(this.pointInteriorColorCombo_SelectedIndexChanged);
            // 
            // pointLineColorCombo
            // 
            this.pointLineColorCombo.FormattingEnabled = true;
            this.pointLineColorCombo.Location = new System.Drawing.Point(114, 100);
            this.pointLineColorCombo.Name = "pointLineColorCombo";
            this.pointLineColorCombo.Size = new System.Drawing.Size(121, 21);
            this.pointLineColorCombo.TabIndex = 0;
            this.pointLineColorCombo.SelectedIndexChanged += new System.EventHandler(this.pointLineColorCombo_SelectedIndexChanged);
            // 
            // pointLineSizeCombo
            // 
            this.pointLineSizeCombo.FormattingEnabled = true;
            this.pointLineSizeCombo.Location = new System.Drawing.Point(114, 73);
            this.pointLineSizeCombo.Name = "pointLineSizeCombo";
            this.pointLineSizeCombo.Size = new System.Drawing.Size(121, 21);
            this.pointLineSizeCombo.TabIndex = 0;
            this.pointLineSizeCombo.SelectedIndexChanged += new System.EventHandler(this.pointLineSizeCombo_SelectedIndexChanged);
            // 
            // sizeCombo
            // 
            this.sizeCombo.FormattingEnabled = true;
            this.sizeCombo.Location = new System.Drawing.Point(114, 46);
            this.sizeCombo.Name = "sizeCombo";
            this.sizeCombo.Size = new System.Drawing.Size(121, 21);
            this.sizeCombo.TabIndex = 0;
            this.sizeCombo.SelectedIndexChanged += new System.EventHandler(this.sizeCombo_SelectedIndexChanged);
            // 
            // shapeCombo
            // 
            this.shapeCombo.FormattingEnabled = true;
            this.shapeCombo.Location = new System.Drawing.Point(114, 19);
            this.shapeCombo.Name = "shapeCombo";
            this.shapeCombo.Size = new System.Drawing.Size(121, 21);
            this.shapeCombo.TabIndex = 0;
            this.shapeCombo.SelectedIndexChanged += new System.EventHandler(this.shapeCombo_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lineConnectAsCombo);
            this.groupBox2.Controls.Add(this.lineColorCombo);
            this.groupBox2.Controls.Add(this.lineSizeCombo);
            this.groupBox2.Location = new System.Drawing.Point(13, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 105);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Line";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "ConnectAs";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Color";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Size";
            // 
            // lineConnectAsCombo
            // 
            this.lineConnectAsCombo.FormattingEnabled = true;
            this.lineConnectAsCombo.Location = new System.Drawing.Point(114, 73);
            this.lineConnectAsCombo.Name = "lineConnectAsCombo";
            this.lineConnectAsCombo.Size = new System.Drawing.Size(121, 21);
            this.lineConnectAsCombo.TabIndex = 0;
            this.lineConnectAsCombo.SelectedIndexChanged += new System.EventHandler(this.lineConnectAsCombo_SelectedIndexChanged);
            // 
            // lineColorCombo
            // 
            this.lineColorCombo.FormattingEnabled = true;
            this.lineColorCombo.Location = new System.Drawing.Point(114, 46);
            this.lineColorCombo.Name = "lineColorCombo";
            this.lineColorCombo.Size = new System.Drawing.Size(121, 21);
            this.lineColorCombo.TabIndex = 0;
            this.lineColorCombo.SelectedIndexChanged += new System.EventHandler(this.lineColorCombo_SelectedIndexChanged);
            // 
            // lineSizeCombo
            // 
            this.lineSizeCombo.FormattingEnabled = true;
            this.lineSizeCombo.Location = new System.Drawing.Point(114, 19);
            this.lineSizeCombo.Name = "lineSizeCombo";
            this.lineSizeCombo.Size = new System.Drawing.Size(121, 21);
            this.lineSizeCombo.TabIndex = 0;
            this.lineSizeCombo.SelectedIndexChanged += new System.EventHandler(this.lineSizeCombo_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(13, 403);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(179, 403);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // preview
            // 
            this.preview.Location = new System.Drawing.Point(13, 296);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(241, 100);
            this.preview.TabIndex = 2;
            // 
            // PointSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 433);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PointSettingsDialog";
            this.Text = "PointSettingsDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox pointInteriorColorCombo;
        private System.Windows.Forms.ComboBox pointLineColorCombo;
        private System.Windows.Forms.ComboBox pointLineSizeCombo;
        private System.Windows.Forms.ComboBox sizeCombo;
        private System.Windows.Forms.ComboBox shapeCombo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox lineConnectAsCombo;
        private System.Windows.Forms.ComboBox lineColorCombo;
        private System.Windows.Forms.ComboBox lineSizeCombo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel preview;
    }
}