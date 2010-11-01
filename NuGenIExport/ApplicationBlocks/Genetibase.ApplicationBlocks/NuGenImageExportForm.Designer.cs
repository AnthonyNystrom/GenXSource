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
				if (components != null)
					components.Dispose();
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.fileFormatBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenFileFormatBlock();
			this.imageTypeBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenImageTypeBlock();
			this.resolutionBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenResolutionBlock();
			this.cancelButton = new System.Windows.Forms.Button();
			this.goButton = new System.Windows.Forms.Button();
			this.dialogBlock = new Genetibase.Controls.NuGenDialogBlock();
			this.pictureBoxPanel = new System.Windows.Forms.Panel();
			this.pictureBox = new Genetibase.Controls.NuGenPictureBox();
			this.outputBlock = new Genetibase.ApplicationBlocks.ImageExport.NuGenOutputBlock();
			this.flowLayoutPanel1.SuspendLayout();
			this.dialogBlock.SuspendLayout();
			this.pictureBoxPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.fileFormatBlock);
			this.flowLayoutPanel1.Controls.Add(this.imageTypeBlock);
			this.flowLayoutPanel1.Controls.Add(this.resolutionBlock);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 60);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3, 3, 5, 0);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(145, 341);
			this.flowLayoutPanel1.TabIndex = 27;
			// 
			// fileFormatBlock
			// 
			this.fileFormatBlock.Location = new System.Drawing.Point(6, 6);
			this.fileFormatBlock.Name = "fileFormatBlock";
			this.fileFormatBlock.Size = new System.Drawing.Size(136, 122);
			this.fileFormatBlock.TabIndex = 22;
			// 
			// imageTypeBlock
			// 
			this.imageTypeBlock.Location = new System.Drawing.Point(6, 134);
			this.imageTypeBlock.Name = "imageTypeBlock";
			this.imageTypeBlock.Size = new System.Drawing.Size(136, 95);
			this.imageTypeBlock.TabIndex = 23;
			// 
			// resolutionBlock
			// 
			this.resolutionBlock.Enabled = false;
			this.resolutionBlock.Location = new System.Drawing.Point(6, 235);
			this.resolutionBlock.Name = "resolutionBlock";
			this.resolutionBlock.Size = new System.Drawing.Size(136, 100);
			this.resolutionBlock.TabIndex = 21;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(455, 9);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 15;
			this.cancelButton.Text = "&Close";
			// 
			// goButton
			// 
			this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.goButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.goButton.Location = new System.Drawing.Point(375, 9);
			this.goButton.Name = "goButton";
			this.goButton.Size = new System.Drawing.Size(75, 23);
			this.goButton.TabIndex = 14;
			this.goButton.Text = "&Export";
			this.goButton.Click += new System.EventHandler(this.goButton_Click);
			// 
			// dialogBlock
			// 
			this.dialogBlock.Controls.Add(this.goButton);
			this.dialogBlock.Controls.Add(this.cancelButton);
			this.dialogBlock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dialogBlock.Location = new System.Drawing.Point(2, 401);
			this.dialogBlock.Name = "dialogBlock";
			this.dialogBlock.Size = new System.Drawing.Size(535, 40);
			this.dialogBlock.TabIndex = 0;
			// 
			// pictureBoxPanel
			// 
			this.pictureBoxPanel.Controls.Add(this.pictureBox);
			this.pictureBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxPanel.Location = new System.Drawing.Point(147, 60);
			this.pictureBoxPanel.Name = "pictureBoxPanel";
			this.pictureBoxPanel.Padding = new System.Windows.Forms.Padding(5, 2, 7, 7);
			this.pictureBoxPanel.Size = new System.Drawing.Size(390, 341);
			this.pictureBoxPanel.TabIndex = 29;
			// 
			// pictureBox
			// 
			this.pictureBox.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(5, 2);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(378, 332);
			this.pictureBox.TabIndex = 21;
			// 
			// outputBlock
			// 
			this.outputBlock.DirectoryName = "";
			this.outputBlock.Dock = System.Windows.Forms.DockStyle.Top;
			this.outputBlock.Filename = "";
			this.outputBlock.Location = new System.Drawing.Point(2, 2);
			this.outputBlock.Name = "outputBlock";
			this.outputBlock.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.outputBlock.Size = new System.Drawing.Size(535, 58);
			this.outputBlock.TabIndex = 28;
			// 
			// NuGenImageExportForm
			// 
			this.AcceptButton = this.goButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(537, 441);
			this.Controls.Add(this.pictureBoxPanel);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.dialogBlock);
			this.Controls.Add(this.outputBlock);
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
			this.flowLayoutPanel1.ResumeLayout(false);
			this.dialogBlock.ResumeLayout(false);
			this.pictureBoxPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private NuGenResolutionBlock resolutionBlock;
		private NuGenFileFormatBlock fileFormatBlock;
		private NuGenImageTypeBlock imageTypeBlock;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button goButton;
		private Genetibase.Controls.NuGenDialogBlock dialogBlock;
		private NuGenOutputBlock outputBlock;
		private System.Windows.Forms.Panel pictureBoxPanel;
		private Genetibase.Controls.NuGenPictureBox pictureBox;
	}
}
