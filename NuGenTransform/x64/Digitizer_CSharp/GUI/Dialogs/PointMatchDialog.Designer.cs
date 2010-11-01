using WCL;
namespace Genetibase.NuGenTransform
{
    partial class PointMatchDialog
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.minSizeTextBox = new System.Windows.Forms.TextBox();
            this.pointSeperationTextBox = new System.Windows.Forms.TextBox();
            this.acceptedColorCombo = new System.Windows.Forms.ComboBox();
            this.rejectedColorCombo = new System.Windows.Forms.ComboBox();
            this.okButton = new VistaButton();
            this.cancelButton = new VistaButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Minimum Size";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Accepted Color";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Rejected Color";
            // 
            // minSizeTextBox
            // 
            this.minSizeTextBox.Location = new System.Drawing.Point(131, 10);
            this.minSizeTextBox.Name = "minSizeTextBox";
            this.minSizeTextBox.Size = new System.Drawing.Size(123, 20);
            this.minSizeTextBox.TabIndex = 5;
            this.minSizeTextBox.TextChanged += new System.EventHandler(this.minLengthTextBox_TextChanged);
            // 
            // pointSeperationTextBox
            // 
            this.pointSeperationTextBox.Location = new System.Drawing.Point(131, 38);
            this.pointSeperationTextBox.Name = "pointSeperationTextBox";
            this.pointSeperationTextBox.Size = new System.Drawing.Size(123, 20);
            this.pointSeperationTextBox.TabIndex = 5;
            this.pointSeperationTextBox.TextChanged += new System.EventHandler(this.pointSeperationTextBox_TextChanged);
            // 
            // acceptedColorCombo
            // 
            this.acceptedColorCombo.FormattingEnabled = true;
            this.acceptedColorCombo.Location = new System.Drawing.Point(131, 95);
            this.acceptedColorCombo.Name = "acceptedColorCombo";
            this.acceptedColorCombo.Size = new System.Drawing.Size(121, 21);
            this.acceptedColorCombo.TabIndex = 6;
            this.acceptedColorCombo.SelectedIndexChanged += new System.EventHandler(this.acceptedColorCombo_SelectedIndexChanged);
            // 
            // rejectedColorCombo
            // 
            this.rejectedColorCombo.FormattingEnabled = true;
            this.rejectedColorCombo.Location = new System.Drawing.Point(131, 123);
            this.rejectedColorCombo.Name = "rejectedColorCombo";
            this.rejectedColorCombo.Size = new System.Drawing.Size(121, 21);
            this.rejectedColorCombo.TabIndex = 6;
            this.rejectedColorCombo.SelectedIndexChanged += new System.EventHandler(this.rejectedColorCombo_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(14, 174);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(110, 174);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // PointMatchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 211);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.rejectedColorCombo);
            this.Controls.Add(this.acceptedColorCombo);
            this.Controls.Add(this.pointSeperationTextBox);
            this.Controls.Add(this.minSizeTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PointMatchDialog";
            this.Text = "Point Match Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox minSizeTextBox;
        private System.Windows.Forms.TextBox pointSeperationTextBox;
        private System.Windows.Forms.ComboBox acceptedColorCombo;
        private System.Windows.Forms.ComboBox rejectedColorCombo;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}