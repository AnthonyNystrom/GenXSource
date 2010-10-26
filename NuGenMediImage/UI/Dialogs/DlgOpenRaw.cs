using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Dialogs
{
	/// <summary>
	/// Summary description for dlgOpenRaw.
	/// </summary>
	public class DlgOpenRaw : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.NumericUpDown numWidth;
		public System.Windows.Forms.NumericUpDown numHeight;
		public System.Windows.Forms.NumericUpDown numSlices;
		public System.Windows.Forms.NumericUpDown numOffset;
		public System.Windows.Forms.NumericUpDown numBits;
		public System.Windows.Forms.Label lblFileSize;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnView;
		public System.Windows.Forms.CheckBox chkBoxPlanar;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DlgOpenRaw()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblFileSize = new System.Windows.Forms.Label();
			this.numWidth = new System.Windows.Forms.NumericUpDown();
			this.numHeight = new System.Windows.Forms.NumericUpDown();
			this.numSlices = new System.Windows.Forms.NumericUpDown();
			this.numOffset = new System.Windows.Forms.NumericUpDown();
			this.numBits = new System.Windows.Forms.NumericUpDown();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnView = new System.Windows.Forms.Button();
			this.chkBoxPlanar = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numSlices)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numOffset)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numBits)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Width";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Height";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Slices";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 23);
			this.label4.TabIndex = 3;
			this.label4.Text = "Offset";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 144);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 23);
			this.label5.TabIndex = 4;
			this.label5.Text = "Bits per voxel";
			// 
			// lblFileSize
			// 
			this.lblFileSize.Location = new System.Drawing.Point(8, 176);
			this.lblFileSize.Name = "lblFileSize";
			this.lblFileSize.Size = new System.Drawing.Size(144, 23);
			this.lblFileSize.TabIndex = 5;
			this.lblFileSize.Text = "File Size:";
			// 
			// numWidth
			// 
			this.numWidth.Location = new System.Drawing.Point(88, 16);
			this.numWidth.Maximum = new System.Decimal(new int[] {
																	 32767,
																	 0,
																	 0,
																	 0});
			this.numWidth.Name = "numWidth";
			this.numWidth.Size = new System.Drawing.Size(64, 20);
			this.numWidth.TabIndex = 6;
			// 
			// numHeight
			// 
			this.numHeight.Location = new System.Drawing.Point(88, 48);
			this.numHeight.Maximum = new System.Decimal(new int[] {
																	  32767,
																	  0,
																	  0,
																	  0});
			this.numHeight.Name = "numHeight";
			this.numHeight.Size = new System.Drawing.Size(64, 20);
			this.numHeight.TabIndex = 7;
			// 
			// numSlices
			// 
			this.numSlices.Location = new System.Drawing.Point(88, 80);
			this.numSlices.Minimum = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  0});
			this.numSlices.Name = "numSlices";
			this.numSlices.Size = new System.Drawing.Size(64, 20);
			this.numSlices.TabIndex = 8;
			this.numSlices.Value = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			// 
			// numOffset
			// 
			this.numOffset.Location = new System.Drawing.Point(88, 112);
			this.numOffset.Name = "numOffset";
			this.numOffset.Size = new System.Drawing.Size(64, 20);
			this.numOffset.TabIndex = 9;
			// 
			// numBits
			// 
			this.numBits.Increment = new System.Decimal(new int[] {
																	  4,
																	  0,
																	  0,
																	  0});
			this.numBits.Location = new System.Drawing.Point(88, 144);
			this.numBits.Maximum = new System.Decimal(new int[] {
																	24,
																	0,
																	0,
																	0});
			this.numBits.Minimum = new System.Decimal(new int[] {
																	8,
																	0,
																	0,
																	0});
			this.numBits.Name = "numBits";
			this.numBits.Size = new System.Drawing.Size(64, 20);
			this.numBits.TabIndex = 10;
			this.numBits.Value = new System.Decimal(new int[] {
																  24,
																  0,
																  0,
																  0});
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(8, 240);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 11;
			this.btnCancel.Text = "Cancel";
			// 
			// btnView
			// 
			this.btnView.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnView.Location = new System.Drawing.Point(88, 240);
			this.btnView.Name = "btnView";
			this.btnView.TabIndex = 12;
			this.btnView.Text = "View";
			// 
			// chkBoxPlanar
			// 
			this.chkBoxPlanar.Location = new System.Drawing.Point(8, 208);
			this.chkBoxPlanar.Name = "chkBoxPlanar";
			this.chkBoxPlanar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkBoxPlanar.TabIndex = 13;
			this.chkBoxPlanar.Text = "Planar RGB        ";
			// 
			// DlgOpenRaw
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(170, 272);
			this.ControlBox = false;
			this.Controls.Add(this.chkBoxPlanar);
			this.Controls.Add(this.btnView);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.numBits);
			this.Controls.Add(this.numOffset);
			this.Controls.Add(this.numSlices);
			this.Controls.Add(this.numHeight);
			this.Controls.Add(this.numWidth);
			this.Controls.Add(this.lblFileSize);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "DlgOpenRaw";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Open Raw";
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numSlices)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numOffset)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numBits)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
