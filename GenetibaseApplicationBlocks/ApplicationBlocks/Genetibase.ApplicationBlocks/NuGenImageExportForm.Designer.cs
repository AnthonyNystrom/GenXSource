/* -----------------------------------------------
 * NuGenImageExportForm.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExport;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenImageExportForm
	{
		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_components != null)
					_components.Dispose();
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this._fileFormatBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenFileFormatBlock();
			this._imageTypeBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenImageTypeBlock();
			this._resolutionBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenResolutionBlock();
			this._cancelButton = new System.Windows.Forms.Button();
			this._goButton = new System.Windows.Forms.Button();
			this._dialogBlock = new Genetibase.ApplicationBlocks.NuGenDialogBlock();
			this._pictureBoxPanel = new System.Windows.Forms.Panel();
			this._pictureBox = new Genetibase.Shared.Controls.NuGenPictureBox();
			this._outputBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenOutputBlock();
			this._flowLayoutPanel.SuspendLayout();
			this._dialogBlock.SuspendLayout();
			this._pictureBoxPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this._flowLayoutPanel.Controls.Add(this._fileFormatBlock);
			this._flowLayoutPanel.Controls.Add(this._imageTypeBlock);
			this._flowLayoutPanel.Controls.Add(this._resolutionBlock);
			this._flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this._flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this._flowLayoutPanel.Location = new System.Drawing.Point(2, 60);
			this._flowLayoutPanel.Name = "flowLayoutPanel1";
			this._flowLayoutPanel.Padding = new System.Windows.Forms.Padding(3, 3, 5, 0);
			this._flowLayoutPanel.Size = new System.Drawing.Size(145, 341);
			this._flowLayoutPanel.TabIndex = 27;
			// 
			// fileFormatBlock
			// 
			this._fileFormatBlock.Location = new System.Drawing.Point(6, 6);
			this._fileFormatBlock.Name = "fileFormatBlock";
			this._fileFormatBlock.Size = new System.Drawing.Size(136, 122);
			this._fileFormatBlock.TabIndex = 22;
			// 
			// imageTypeBlock
			// 
			this._imageTypeBlock.Location = new System.Drawing.Point(6, 134);
			this._imageTypeBlock.Name = "imageTypeBlock";
			this._imageTypeBlock.Size = new System.Drawing.Size(136, 95);
			this._imageTypeBlock.TabIndex = 23;
			// 
			// resolutionBlock
			// 
			this._resolutionBlock.Enabled = false;
			this._resolutionBlock.Location = new System.Drawing.Point(6, 235);
			this._resolutionBlock.Name = "resolutionBlock";
			this._resolutionBlock.Size = new System.Drawing.Size(136, 100);
			this._resolutionBlock.TabIndex = 21;
			// 
			// cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._cancelButton.Location = new System.Drawing.Point(455, 9);
			this._cancelButton.Name = "cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 15;
			this._cancelButton.Text = "&Close";
			// 
			// goButton
			// 
			this._goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._goButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._goButton.Location = new System.Drawing.Point(375, 9);
			this._goButton.Name = "goButton";
			this._goButton.Size = new System.Drawing.Size(75, 23);
			this._goButton.TabIndex = 14;
			this._goButton.Text = "&Export";
			this._goButton.Click += new System.EventHandler(this.goButton_Click);
			// 
			// dialogBlock
			// 
			this._dialogBlock.Controls.Add(this._goButton);
			this._dialogBlock.Controls.Add(this._cancelButton);
			this._dialogBlock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._dialogBlock.Location = new System.Drawing.Point(2, 401);
			this._dialogBlock.Name = "dialogBlock";
			this._dialogBlock.Size = new System.Drawing.Size(535, 40);
			this._dialogBlock.TabIndex = 0;
			// 
			// pictureBoxPanel
			// 
			this._pictureBoxPanel.Controls.Add(this._pictureBox);
			this._pictureBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pictureBoxPanel.Location = new System.Drawing.Point(147, 60);
			this._pictureBoxPanel.Name = "pictureBoxPanel";
			this._pictureBoxPanel.Padding = new System.Windows.Forms.Padding(5, 2, 7, 7);
			this._pictureBoxPanel.Size = new System.Drawing.Size(390, 341);
			this._pictureBoxPanel.TabIndex = 29;
			// 
			// pictureBox
			// 
			this._pictureBox.BackColor = System.Drawing.SystemColors.WindowFrame;
			this._pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pictureBox.Location = new System.Drawing.Point(5, 2);
			this._pictureBox.Name = "pictureBox";
			this._pictureBox.Size = new System.Drawing.Size(378, 332);
			this._pictureBox.TabIndex = 21;
			// 
			// outputBlock
			// 
			this._outputBlock.DirectoryName = "";
			this._outputBlock.Dock = System.Windows.Forms.DockStyle.Top;
			this._outputBlock.Filename = "";
			this._outputBlock.Location = new System.Drawing.Point(2, 2);
			this._outputBlock.Name = "outputBlock";
			this._outputBlock.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this._outputBlock.Size = new System.Drawing.Size(535, 58);
			this._outputBlock.TabIndex = 28;
			// 
			// NuGenImageExportForm
			// 
			this.AcceptButton = this._goButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(537, 441);
			this.Controls.Add(this._pictureBoxPanel);
			this.Controls.Add(this._flowLayoutPanel);
			this.Controls.Add(this._dialogBlock);
			this.Controls.Add(this._outputBlock);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(543, 448);
			this.Name = "NuGenImageExportForm";
			this.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Export";
			this._flowLayoutPanel.ResumeLayout(false);
			this._dialogBlock.ResumeLayout(false);
			this._pictureBoxPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private NuGenResolutionBlock _resolutionBlock;
		private NuGenFileFormatBlock _fileFormatBlock;
		private NuGenImageTypeBlock _imageTypeBlock;
		private System.Windows.Forms.FlowLayoutPanel _flowLayoutPanel;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _goButton;
		private Genetibase.ApplicationBlocks.NuGenDialogBlock _dialogBlock;
		private NuGenOutputBlock _outputBlock;
		private System.Windows.Forms.Panel _pictureBoxPanel;
		private Genetibase.Shared.Controls.NuGenPictureBox _pictureBox;
	}
}
