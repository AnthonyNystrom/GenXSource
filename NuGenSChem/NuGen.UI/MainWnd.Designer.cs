namespace Sketch.UI
{
	partial class MainWnd
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.mainControl = new Genetibase.Chem.NuGenSChem.MainWindow();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // mainControl
            // 
            this.mainControl.Location = new System.Drawing.Point(-2, 0);
            this.mainControl.Name = "mainControl";
            this.mainControl.Size = new System.Drawing.Size(772, 673);
            this.mainControl.TabIndex = 1;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 675);
            this.Controls.Add(this.mainControl);
            this.DoubleBuffered = true;
            this.Name = "MainWnd";
            this.Text = "NuGen";
            this.ResumeLayout(false);

		}

		#endregion

        private Genetibase.Chem.NuGenSChem.MainWindow mainControl;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}

