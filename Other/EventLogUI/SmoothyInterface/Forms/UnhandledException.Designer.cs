namespace SmoothyInterface.Forms
{
	partial class UnhandledException
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnhandledException));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tbInfo = new System.Windows.Forms.TextBox();
			this.tbExceptionDetails = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::EventMonitor.Properties.Resources.afraid;
			this.pictureBox1.Location = new System.Drawing.Point(15, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(37, 40);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// tbInfo
			// 
			this.tbInfo.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.tbInfo.Location = new System.Drawing.Point(67, 12);
			this.tbInfo.Multiline = true;
			this.tbInfo.Name = "tbInfo";
			this.tbInfo.ReadOnly = true;
			this.tbInfo.Size = new System.Drawing.Size(292, 48);
			this.tbInfo.TabIndex = 1;
			this.tbInfo.Text = "Smooth has caught an unhandled exception.  The details are below.  If you can, pl" +
				"ease report this error to the author at the originating site.";
			// 
			// tbExceptionDetails
			// 
			this.tbExceptionDetails.BackColor = System.Drawing.Color.LightSalmon;
			this.tbExceptionDetails.Location = new System.Drawing.Point(15, 76);
			this.tbExceptionDetails.Multiline = true;
			this.tbExceptionDetails.Name = "tbExceptionDetails";
			this.tbExceptionDetails.ReadOnly = true;
			this.tbExceptionDetails.Size = new System.Drawing.Size(344, 183);
			this.tbExceptionDetails.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(145, 265);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// UnhandledException
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(371, 295);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tbExceptionDetails);
			this.Controls.Add(this.tbInfo);
			this.Controls.Add(this.pictureBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "UnhandledException";
			this.Text = "Unhandled Exception";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox tbInfo;
		private System.Windows.Forms.TextBox tbExceptionDetails;
		private System.Windows.Forms.Button btnOK;
	}
}