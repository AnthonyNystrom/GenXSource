﻿namespace Dile.UI
{
	partial class NuGenInformationPanel
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
			this.informations = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// informations
			// 
			this.informations.AcceptsTab = true;
			this.informations.AutoWordSelection = true;
			this.informations.BackColor = System.Drawing.SystemColors.Window;
			this.informations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.informations.Location = new System.Drawing.Point(0, 0);
			this.informations.Name = "informations";
			this.informations.ReadOnly = true;
			this.informations.Size = new System.Drawing.Size(292, 273);
			this.informations.TabIndex = 0;
			this.informations.Text = "";
			this.informations.WordWrap = false;
			// 
			// InformationPanel
			// 						
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.informations);
			this.Name = "InformationPanel";
			this.Text = "Information Panel";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox informations;
	}
}