using WCL;
namespace Genetibase.NuGenTransform
{
    partial class SegmentsDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.minLengthTextBox = new System.Windows.Forms.TextBox();
            this.pointSeperationTextBox = new System.Windows.Forms.TextBox();
            this.lineSizeCombo = new System.Windows.Forms.ComboBox();
            this.lineColorCombo = new System.Windows.Forms.ComboBox();
            this.fillCornersCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new VistaButton();
            this.cancelButton = new VistaButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Minimum Length";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Point Separation";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fill Corners";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Line Size";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Line Color";
            // 
            // minLengthTextBox
            // 
            this.minLengthTextBox.Location = new System.Drawing.Point(131, 10);
            this.minLengthTextBox.Name = "minLengthTextBox";
            this.minLengthTextBox.Size = new System.Drawing.Size(123, 20);
            this.minLengthTextBox.TabIndex = 5;
            this.minLengthTextBox.TextChanged += new System.EventHandler(this.minLengthTextBox_TextChanged);
            // 
            // pointSeperationTextBox
            // 
            this.pointSeperationTextBox.Location = new System.Drawing.Point(131, 38);
            this.pointSeperationTextBox.Name = "pointSeperationTextBox";
            this.pointSeperationTextBox.Size = new System.Drawing.Size(123, 20);
            this.pointSeperationTextBox.TabIndex = 5;
            this.pointSeperationTextBox.TextChanged += new System.EventHandler(this.pointSeperationTextBox_TextChanged);
            // 
            // lineSizeCombo
            // 
            this.lineSizeCombo.FormattingEnabled = true;
            this.lineSizeCombo.Location = new System.Drawing.Point(131, 95);
            this.lineSizeCombo.Name = "lineSizeCombo";
            this.lineSizeCombo.Size = new System.Drawing.Size(121, 21);
            this.lineSizeCombo.TabIndex = 6;
            this.lineSizeCombo.SelectedIndexChanged += new System.EventHandler(this.lineSizeCombo_SelectedIndexChanged);
            // 
            // lineColorCombo
            // 
            this.lineColorCombo.FormattingEnabled = true;
            this.lineColorCombo.Location = new System.Drawing.Point(131, 123);
            this.lineColorCombo.Name = "lineColorCombo";
            this.lineColorCombo.Size = new System.Drawing.Size(121, 21);
            this.lineColorCombo.TabIndex = 6;
            this.lineColorCombo.SelectedIndexChanged += new System.EventHandler(this.lineColorCombo_SelectedIndexChanged);
            // 
            // fillCornersCheckBox
            // 
            this.fillCornersCheckBox.AutoSize = true;
            this.fillCornersCheckBox.Location = new System.Drawing.Point(131, 70);
            this.fillCornersCheckBox.Name = "fillCornersCheckBox";
            this.fillCornersCheckBox.Size = new System.Drawing.Size(15, 14);
            this.fillCornersCheckBox.TabIndex = 7;
            this.fillCornersCheckBox.UseVisualStyleBackColor = true;
            this.fillCornersCheckBox.CheckedChanged += new System.EventHandler(this.fillCornersCheckBox_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(15, 161);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(114, 161);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // SegmentsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 194);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fillCornersCheckBox);
            this.Controls.Add(this.lineColorCombo);
            this.Controls.Add(this.lineSizeCombo);
            this.Controls.Add(this.pointSeperationTextBox);
            this.Controls.Add(this.minLengthTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SegmentsDialog";
            this.Text = "Segments Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox minLengthTextBox;
        private System.Windows.Forms.TextBox pointSeperationTextBox;
        private System.Windows.Forms.ComboBox lineSizeCombo;
        private System.Windows.Forms.ComboBox lineColorCombo;
        private System.Windows.Forms.CheckBox fillCornersCheckBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}