using System;
using WCL;
namespace Genetibase.NuGenTransform
{
    partial class GridRemovalSettingsDialog
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button1 = new VistaButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.histogram = new ImageHistogram(originalImage, discretizeSettings);
            this.histogram.ProgressUpdated = this.ProgressUpdated;
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(177, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Remove pixels of a certain color";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(13, 176);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(256, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Remove pixels close to regularly spaced gridlines";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(13, 271);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(201, 17);
            this.checkBox3.TabIndex = 0;
            this.checkBox3.Text = "Remove thin lines parallel to the axes";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(checkBox3_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(264, 201);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Gridlines...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Enabled = false;
            this.button1.Click += new System.EventHandler(button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(264, 234);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Enabled = false;
            this.textBox1.TextChanged += new System.EventHandler(this.ControlTextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Close Distance (pixels)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(264, 314);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.Enabled = false;
            this.textBox2.TextChanged += new System.EventHandler(this.ControlTextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(196, 377);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 2;
            this.textBox3.Enabled = true;
            this.textBox3.TextChanged += new System.EventHandler(this.ControlTextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 384);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Maximum gap to connect (pixels)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(90, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Minimum line distance (pixels)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Color";
            // 
            // panel1
            // 
            this.previewPanel.Location = new System.Drawing.Point(12, 413);
            this.previewPanel.Name = "panel1";
            this.previewPanel.Size = new System.Drawing.Size(351, 182);
            this.previewPanel.TabIndex = 6;
            //
            // histogram
            //
            this.histogram.Location = new System.Drawing.Point(125, 45);
            this.histogram.Size = new System.Drawing.Size(250, 125);
            histogram.MakeHistogramData();
            this.histogram.Enabled = false;
            //
            // okButton
            //
            this.okButton = new VistaButton();
            this.okButton.AutoSize = true;
            this.okButton.Text = "OK";
            this.okButton.Location = new System.Drawing.Point(12, 610);
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            //
            // cancel Button
            //
            this.cancelButton = new VistaButton();
            this.cancelButton.AutoSize = true;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Location = new System.Drawing.Point(okButton.Location.X + okButton.Width + 10, 610);
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            // 
            // GridRemovalSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 675);
            this.MaximumSize = this.ClientSize;
            this.Controls.Add(this.previewPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.histogram);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "GridRemovalSettingsDialog";
            this.Text = "GridRemovalSettingsDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ControlTextChanged(object sender, System.EventArgs e)
        {
            try
            {
                settings.thinThickness = double.Parse(textBox2.Text);
                settings.gridDistance = double.Parse(textBox1.Text);
                settings.gapSeparation = double.Parse(textBox3.Text);
            }
            catch (Exception ex)
            {
                return;
            }

            ValueChanged(true);
        }

        void button1_Click(object sender, System.EventArgs e)
        {
            GridSettingsDialog dlg = new GridSettingsDialog(gridRemovalMesh, coordSettings);

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gridRemovalMesh = dlg.Settings;

                ValueChanged(true);
            }
        }

        void checkBox3_CheckedChanged(object sender, System.EventArgs e)
        {
            textBox2.Enabled = checkBox3.Checked;
            settings.removeThinLines = checkBox3.Checked;

            ValueChanged(true);
        }

        void checkBox2_CheckedChanged(object sender, System.EventArgs e)
        {
            textBox1.Enabled = checkBox2.Checked;
            button1.Enabled = checkBox2.Checked;
            settings.removeGridlines = checkBox2.Checked;            

            ValueChanged(true);
        }

        void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            histogram.Enabled = checkBox1.Checked;
            settings.removeColor = checkBox1.Checked;

            ValueChanged(true);
        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel previewPanel;
        private ImageHistogram histogram;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;

    }
}