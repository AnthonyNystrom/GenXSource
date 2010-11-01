namespace Genetibase.ApplicationBlocks.ImageExport
{
	partial class NuGenImageTypeBlock
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
			this.imageTypeGroup = new System.Windows.Forms.GroupBox();
			this.monochromeCheckBox = new System.Windows.Forms.CheckBox();
			this.grayscaleCheckBox = new System.Windows.Forms.CheckBox();
			this.colorCheckBox = new System.Windows.Forms.CheckBox();
			this.imageTypeGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageTypeGroup
			// 
			this.imageTypeGroup.Controls.Add(this.monochromeCheckBox);
			this.imageTypeGroup.Controls.Add(this.grayscaleCheckBox);
			this.imageTypeGroup.Controls.Add(this.colorCheckBox);
			this.imageTypeGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageTypeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.imageTypeGroup.Location = new System.Drawing.Point(0, 0);
			this.imageTypeGroup.Name = "imageTypeGroup";
			this.imageTypeGroup.Size = new System.Drawing.Size(113, 95);
			this.imageTypeGroup.TabIndex = 16;
			this.imageTypeGroup.TabStop = false;
			this.imageTypeGroup.Text = "Image Type";
			// 
			// monochromeCheckBox
			// 
			this.monochromeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.monochromeCheckBox.Location = new System.Drawing.Point(8, 64);
			this.monochromeCheckBox.Name = "monochromeCheckBox";
			this.monochromeCheckBox.Size = new System.Drawing.Size(104, 24);
			this.monochromeCheckBox.TabIndex = 11;
			this.monochromeCheckBox.Text = "&Monochrome";
			// 
			// grayscaleCheckBox
			// 
			this.grayscaleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.grayscaleCheckBox.Location = new System.Drawing.Point(8, 40);
			this.grayscaleCheckBox.Name = "grayscaleCheckBox";
			this.grayscaleCheckBox.Size = new System.Drawing.Size(104, 24);
			this.grayscaleCheckBox.TabIndex = 10;
			this.grayscaleCheckBox.Text = "&Grayscale";
			// 
			// colorCheckBox
			// 
			this.colorCheckBox.Checked = true;
			this.colorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.colorCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.colorCheckBox.Location = new System.Drawing.Point(8, 16);
			this.colorCheckBox.Name = "colorCheckBox";
			this.colorCheckBox.Size = new System.Drawing.Size(104, 24);
			this.colorCheckBox.TabIndex = 9;
			this.colorCheckBox.Text = "&Color";
			// 
			// NuGenImageTypeBlock
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.imageTypeGroup);
			this.Name = "NuGenImageTypeBlock";
			this.Size = new System.Drawing.Size(113, 95);
			this.imageTypeGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox imageTypeGroup;
		private System.Windows.Forms.CheckBox monochromeCheckBox;
		private System.Windows.Forms.CheckBox grayscaleCheckBox;
		private System.Windows.Forms.CheckBox colorCheckBox;
	}
}
