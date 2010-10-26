// $Header: /Ink/Scribble/Test/Progress.cs 3     11/19/04 5:17p Bernd.helzer $
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sample
{
	/// <summary>
	/// Summary description for Progress.
	/// </summary>
	public class Progress : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar fProgressBar;
		private System.Windows.Forms.Button fCancelButton;
		private bool fCancel = false;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Progress()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public bool Cancel
		{
			get { return fCancel; }
		}

		public int Complete
		{
			get { return fProgressBar.Value; }
			set { fProgressBar.Value = value; }
		}

		public Agilix.Ink.ProgressCallback GetProgressCallback()
		{
			return new Agilix.Ink.ProgressCallback(Callback);
		}

		private bool Callback(int percent)
		{
			Complete = percent;
			return Cancel;
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
			this.fProgressBar = new System.Windows.Forms.ProgressBar();
			this.fCancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// fProgressBar
			// 
			this.fProgressBar.Location = new System.Drawing.Point(8, 16);
			this.fProgressBar.Name = "fProgressBar";
			this.fProgressBar.Size = new System.Drawing.Size(224, 23);
			this.fProgressBar.TabIndex = 0;
			// 
			// fCancelButton
			// 
			this.fCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.fCancelButton.Location = new System.Drawing.Point(84, 48);
			this.fCancelButton.Name = "fCancelButton";
			this.fCancelButton.TabIndex = 1;
			this.fCancelButton.Text = "Cancel";
			this.fCancelButton.Click += new System.EventHandler(this.fCancelButton_Click);
			// 
			// Progress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(242, 80);
			this.ControlBox = false;
			this.Controls.Add(this.fCancelButton);
			this.Controls.Add(this.fProgressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Progress";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Progress";
			this.ResumeLayout(false);

		}
		#endregion

		private void fCancelButton_Click(object sender, System.EventArgs e)
		{
			fCancel = true;
		}
	}
}
