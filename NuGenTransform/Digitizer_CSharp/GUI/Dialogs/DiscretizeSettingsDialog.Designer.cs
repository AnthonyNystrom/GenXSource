using System.Drawing;
using System.Threading;
using WCL;
namespace Genetibase.NuGenTransform
{
    partial class DiscretizeSettingsDialog
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.histogram = new ImageHistogram(originalImage, settings);
            this.histogram.ProgressUpdated = this.ProgressUpdated;
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new VistaButton();
            this.cancelButton = new VistaButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.histogram);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 284);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Discretize Method Selection";
            //
            // histogram
            //
            this.histogram.Location = new System.Drawing.Point(120, 35);
            this.histogram.Name = "histogram";
            this.histogram.Size = new System.Drawing.Size(200, 200);
            this.histogram.ValueChanged = this.ValueChanged;
            histogram.MakeHistogramData();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 43);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(64, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Intensity";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 88);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(79, 17);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Foreground";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(radioButton2_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 134);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(45, 17);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Hue";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(radioButton3_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(7, 183);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(73, 17);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Saturation";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(radioButton4_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(7, 231);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(52, 17);
            this.radioButton5.TabIndex = 0;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Value";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(radioButton5_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.previewPanel.Location = new System.Drawing.Point(13, 320);
            this.previewPanel.Name = "pictureBox1";
            this.previewPanel.Size = new System.Drawing.Size(340, 177);
            this.previewPanel.TabIndex = 1;
            this.previewPanel.TabStop = false;
            this.previewPanel.AutoScroll = true;
            this.previewPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 304);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Preview";
            //
            // okButton
            //
            this.okButton.Location = new System.Drawing.Point(20, 515);
            this.okButton.Text = "OK";
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.AutoSize = true;
            //
            // cancelButton
            //
            this.cancelButton.Location = new Point(100, 515);
            this.cancelButton.Text = "Cancel";
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.AutoSize = true;
            // 
            // DiscretizeSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 560);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.previewPanel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "DiscretizeSettingsDialog";
            this.Text = "DiscretizeSettingsDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.MaximumSize = Size;
            this.PerformLayout();

        }

        void radioButton5_CheckedChanged(object sender, System.EventArgs e)
        {
            settings = histogram.Settings;
            if (settings.discretizeMethod != DiscretizeMethod.DiscretizeValue)
            {
                settings.discretizeMethod = DiscretizeMethod.DiscretizeValue;
                histogram.Settings = settings;
            }

            ValueChanged(true);
        }

        void radioButton4_CheckedChanged(object sender, System.EventArgs e)
        {
            settings = histogram.Settings;
            if (settings.discretizeMethod != DiscretizeMethod.DiscretizeSaturation)
            {
                settings.discretizeMethod = DiscretizeMethod.DiscretizeSaturation;
                histogram.Settings = settings;
            }

            ValueChanged(true);
        }

        void radioButton3_CheckedChanged(object sender, System.EventArgs e)
        {
            settings = histogram.Settings;
            if (settings.discretizeMethod != DiscretizeMethod.DiscretizeHue)
            {
                settings.discretizeMethod = DiscretizeMethod.DiscretizeHue;
                histogram.Settings = settings;
            }

            ValueChanged(true);
        }

        void radioButton2_CheckedChanged(object sender, System.EventArgs e)
        {
            settings = histogram.Settings;
            if (settings.discretizeMethod != DiscretizeMethod.DiscretizeForeground)
            {
                settings.discretizeMethod = DiscretizeMethod.DiscretizeForeground;
                histogram.Settings = settings;
            }

            ValueChanged(true);
        }

        void radioButton1_CheckedChanged(object sender, System.EventArgs e)
        {
            settings = histogram.Settings;
            if (settings.discretizeMethod != DiscretizeMethod.DiscretizeIntensity)
            {
                settings.discretizeMethod = DiscretizeMethod.DiscretizeIntensity;
                histogram.Settings = settings;
            }

            ValueChanged(true);
        }

        Thread discretizeThread = null;

        void ValueChanged(bool ignoreThread)
        {
            settings = histogram.Settings;

            if (ignoreThread && discretizeThread != null)
            {
                discretizeThread.Abort();
                discretizeThread = null;
            }

            if (discretizeThread == null)
            {
                discretizeThread = new Thread(new ThreadStart(this.DiscretizeGo));
                discretizeThread.Start();
            }

        }

        private void DiscretizeGo()
        {
            NuGenDiscretize discretize = new NuGenDiscretize(originalImage.Clone() as Image, settings);
            discretize.Discretize();
            Image img = discretize.GetImage();

            previewPanel.BackgroundImage = img;
            Refresh();

            discretizeThread = null;
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel previewPanel;        
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private ImageHistogram histogram;
    }
}