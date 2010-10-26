namespace Genetibase.NuGenDEMVis.UI
{
    partial class DataProfileControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.uiComboBox1 = new Janus.Windows.EditControls.UIComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataProfilePreviewControl1 = new Genetibase.NuGenDEMVis.UI.DataProfilePreviewControl();
            this.uiComboBox2 = new Janus.Windows.EditControls.UIComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Profile:";
            // 
            // uiComboBox1
            // 
            this.uiComboBox1.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.uiComboBox1.Location = new System.Drawing.Point(59, 12);
            this.uiComboBox1.Name = "uiComboBox1";
            this.uiComboBox1.Size = new System.Drawing.Size(174, 20);
            this.uiComboBox1.TabIndex = 2;
            this.uiComboBox1.Text = "uiComboBox1";
            this.uiComboBox1.SelectedIndexChanged += new System.EventHandler(this.uiComboBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 38);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(216, 46);
            this.textBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Preview:";
            // 
            // dataProfilePreviewControl1
            // 
            this.dataProfilePreviewControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataProfilePreviewControl1.Location = new System.Drawing.Point(68, 98);
            this.dataProfilePreviewControl1.Name = "dataProfilePreviewControl1";
            this.dataProfilePreviewControl1.Size = new System.Drawing.Size(434, 72);
            this.dataProfilePreviewControl1.TabIndex = 5;
            // 
            // uiComboBox2
            // 
            this.uiComboBox2.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.uiComboBox2.Location = new System.Drawing.Point(328, 12);
            this.uiComboBox2.Name = "uiComboBox2";
            this.uiComboBox2.Size = new System.Drawing.Size(174, 20);
            this.uiComboBox2.TabIndex = 6;
            this.uiComboBox2.Text = "uiComboBox2";
            this.uiComboBox2.SelectedIndexChanged += new System.EventHandler(this.uiComboBox2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(261, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sub-Profile:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(264, 38);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(238, 46);
            this.textBox2.TabIndex = 8;
            // 
            // DataProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uiComboBox2);
            this.Controls.Add(this.dataProfilePreviewControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.uiComboBox1);
            this.Controls.Add(this.label1);
            this.Name = "DataProfileControl";
            this.Size = new System.Drawing.Size(515, 192);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIComboBox uiComboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private DataProfilePreviewControl dataProfilePreviewControl1;
        private Janus.Windows.EditControls.UIComboBox uiComboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
    }
}
