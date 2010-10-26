namespace NuGenSVisualLib.Rendering.Chem.Schemes
{
    partial class BallAndStickSchemeSUI
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
            this.label12 = new System.Windows.Forms.Label();
            this.integerUpDown1 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.integerUpDown2 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Stick Thickness:";
            // 
            // integerUpDown1
            // 
            this.integerUpDown1.Location = new System.Drawing.Point(104, 14);
            this.integerUpDown1.Maximum = 5;
            this.integerUpDown1.Name = "integerUpDown1";
            this.integerUpDown1.Size = new System.Drawing.Size(50, 20);
            this.integerUpDown1.TabIndex = 11;
            this.integerUpDown1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.integerUpDown1.ValueChanged += new System.EventHandler(this.integerUpDown1_ValueChanged);
            // 
            // integerUpDown2
            // 
            this.integerUpDown2.Location = new System.Drawing.Point(242, 14);
            this.integerUpDown2.Name = "integerUpDown2";
            this.integerUpDown2.Size = new System.Drawing.Size(59, 20);
            this.integerUpDown2.TabIndex = 12;
            this.integerUpDown2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.integerUpDown2.ValueChanged += new System.EventHandler(this.integerUpDown2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(175, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Atom Glow:";
            // 
            // uiCheckBox1
            // 
            this.uiCheckBox1.Location = new System.Drawing.Point(15, 40);
            this.uiCheckBox1.Name = "uiCheckBox1";
            this.uiCheckBox1.Size = new System.Drawing.Size(139, 23);
            this.uiCheckBox1.TabIndex = 15;
            this.uiCheckBox1.Text = "Electron Charge Clouds";
            this.uiCheckBox1.CheckedChanged += new System.EventHandler(this.uiCheckBox1_CheckedChanged);
            // 
            // BallAndStickSchemeSUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uiCheckBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.integerUpDown2);
            this.Controls.Add(this.integerUpDown1);
            this.Controls.Add(this.label12);
            this.Name = "BallAndStickSchemeSUI";
            this.Size = new System.Drawing.Size(390, 68);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown integerUpDown1;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown integerUpDown2;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
    }
}
