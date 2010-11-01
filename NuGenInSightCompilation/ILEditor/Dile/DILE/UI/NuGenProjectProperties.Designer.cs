namespace Dile.UI
{
	partial class NuGenProjectProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenProjectProperties));
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.settingsDisplayerControl = new Dile.Controls.NuGenSettingsDisplayerControl();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.uiButton2);
            this.buttonsPanel.Controls.Add(this.uiButton1);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonsPanel.Location = new System.Drawing.Point(0, 325);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(632, 31);
            this.buttonsPanel.TabIndex = 1;
            // 
            // settingsDisplayerControl
            // 
            this.settingsDisplayerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsDisplayerControl.Location = new System.Drawing.Point(0, 0);
            this.settingsDisplayerControl.Name = "settingsDisplayerControl";
            this.settingsDisplayerControl.Size = new System.Drawing.Size(632, 325);
            this.settingsDisplayerControl.TabIndex = 0;
            // 
            // uiButton1
            // 
            this.uiButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uiButton1.Location = new System.Drawing.Point(469, 3);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 23);
            this.uiButton1.TabIndex = 2;
            this.uiButton1.Text = "OK";
            this.uiButton1.Click += new System.EventHandler(this.okButton_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uiButton2.Location = new System.Drawing.Point(554, 3);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(75, 23);
            this.uiButton2.TabIndex = 3;
            this.uiButton2.Text = "Cancel";
            // 
            // NuGenProjectProperties
            // 
            this.ClientSize = new System.Drawing.Size(632, 356);
            this.Controls.Add(this.settingsDisplayerControl);
            this.Controls.Add(this.buttonsPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NuGenProjectProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Properties";
            this.buttonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.NuGenSettingsDisplayerControl settingsDisplayerControl;
        private System.Windows.Forms.Panel buttonsPanel;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
	}
}