namespace Genetibase.ApplicationBlocks.ImageExport
{
	partial class NuGenFileFormatBlock
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
			this.fileFormatGroup = new System.Windows.Forms.GroupBox();
			this.exifCheckBox = new System.Windows.Forms.CheckBox();
			this.wmfCheckBox = new System.Windows.Forms.CheckBox();
			this.emfCheckBox = new System.Windows.Forms.CheckBox();
			this.bmpCheckBox = new System.Windows.Forms.CheckBox();
			this.gifCheckBox = new System.Windows.Forms.CheckBox();
			this.jpegCheckBox = new System.Windows.Forms.CheckBox();
			this.pngCheckBox = new System.Windows.Forms.CheckBox();
			this.tiffCheckBox = new System.Windows.Forms.CheckBox();
			this.fileFormatGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileFormatGroup
			// 
			this.fileFormatGroup.Controls.Add(this.exifCheckBox);
			this.fileFormatGroup.Controls.Add(this.wmfCheckBox);
			this.fileFormatGroup.Controls.Add(this.emfCheckBox);
			this.fileFormatGroup.Controls.Add(this.bmpCheckBox);
			this.fileFormatGroup.Controls.Add(this.gifCheckBox);
			this.fileFormatGroup.Controls.Add(this.jpegCheckBox);
			this.fileFormatGroup.Controls.Add(this.pngCheckBox);
			this.fileFormatGroup.Controls.Add(this.tiffCheckBox);
			this.fileFormatGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fileFormatGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.fileFormatGroup.Location = new System.Drawing.Point(0, 0);
			this.fileFormatGroup.Name = "fileFormatGroup";
			this.fileFormatGroup.Size = new System.Drawing.Size(140, 122);
			this.fileFormatGroup.TabIndex = 9;
			this.fileFormatGroup.TabStop = false;
			this.fileFormatGroup.Text = "File Format";
			// 
			// exifCheckBox
			// 
			this.exifCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.exifCheckBox.Location = new System.Drawing.Point(8, 64);
			this.exifCheckBox.Name = "exifCheckBox";
			this.exifCheckBox.Size = new System.Drawing.Size(64, 24);
			this.exifCheckBox.TabIndex = 3;
			this.exifCheckBox.Text = "E&XIF";
			// 
			// wmfCheckBox
			// 
			this.wmfCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.wmfCheckBox.Location = new System.Drawing.Point(80, 88);
			this.wmfCheckBox.Name = "wmfCheckBox";
			this.wmfCheckBox.Size = new System.Drawing.Size(48, 24);
			this.wmfCheckBox.TabIndex = 8;
			this.wmfCheckBox.Text = "&WMF";
			// 
			// emfCheckBox
			// 
			this.emfCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.emfCheckBox.Location = new System.Drawing.Point(8, 40);
			this.emfCheckBox.Name = "emfCheckBox";
			this.emfCheckBox.Size = new System.Drawing.Size(64, 24);
			this.emfCheckBox.TabIndex = 2;
			this.emfCheckBox.Text = "&EMF";
			// 
			// bmpCheckBox
			// 
			this.bmpCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bmpCheckBox.Location = new System.Drawing.Point(8, 16);
			this.bmpCheckBox.Name = "bmpCheckBox";
			this.bmpCheckBox.Size = new System.Drawing.Size(64, 24);
			this.bmpCheckBox.TabIndex = 1;
			this.bmpCheckBox.Text = "&BMP";
			// 
			// gifCheckBox
			// 
			this.gifCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gifCheckBox.Location = new System.Drawing.Point(8, 88);
			this.gifCheckBox.Name = "gifCheckBox";
			this.gifCheckBox.Size = new System.Drawing.Size(64, 24);
			this.gifCheckBox.TabIndex = 4;
			this.gifCheckBox.Text = "&GIF";
			// 
			// jpegCheckBox
			// 
			this.jpegCheckBox.Checked = true;
			this.jpegCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.jpegCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.jpegCheckBox.Location = new System.Drawing.Point(80, 16);
			this.jpegCheckBox.Name = "jpegCheckBox";
			this.jpegCheckBox.Size = new System.Drawing.Size(48, 24);
			this.jpegCheckBox.TabIndex = 5;
			this.jpegCheckBox.Text = "&JPEG";
			// 
			// pngCheckBox
			// 
			this.pngCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.pngCheckBox.Location = new System.Drawing.Point(80, 40);
			this.pngCheckBox.Name = "pngCheckBox";
			this.pngCheckBox.Size = new System.Drawing.Size(48, 24);
			this.pngCheckBox.TabIndex = 6;
			this.pngCheckBox.Text = "&PNG";
			// 
			// tiffCheckBox
			// 
			this.tiffCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.tiffCheckBox.Location = new System.Drawing.Point(80, 64);
			this.tiffCheckBox.Name = "tiffCheckBox";
			this.tiffCheckBox.Size = new System.Drawing.Size(48, 24);
			this.tiffCheckBox.TabIndex = 7;
			this.tiffCheckBox.Text = "&TIFF";
			// 
			// NuGenFileFormatBlock
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.fileFormatGroup);
			this.Name = "NuGenFileFormatBlock";
			this.Size = new System.Drawing.Size(140, 122);
			this.fileFormatGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox fileFormatGroup;
		private System.Windows.Forms.CheckBox exifCheckBox;
		private System.Windows.Forms.CheckBox wmfCheckBox;
		private System.Windows.Forms.CheckBox emfCheckBox;
		private System.Windows.Forms.CheckBox bmpCheckBox;
		private System.Windows.Forms.CheckBox gifCheckBox;
		private System.Windows.Forms.CheckBox jpegCheckBox;
		private System.Windows.Forms.CheckBox pngCheckBox;
		private System.Windows.Forms.CheckBox tiffCheckBox;
	}
}
