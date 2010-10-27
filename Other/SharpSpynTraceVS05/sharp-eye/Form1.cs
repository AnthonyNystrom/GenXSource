using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace sharp_eye
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Genetibase.Debug.NuGenSpyControl nuGenSpyControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
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
				if (components != null) 
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
			this.nuGenSpyControl1 = new Genetibase.Debug.NuGenSpyControl();
			this.SuspendLayout();
			// 
			// nuGenSpyControl1
			// 
			this.nuGenSpyControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenSpyControl1.Location = new System.Drawing.Point(0, 0);
			this.nuGenSpyControl1.Name = "nuGenSpyControl1";
			this.nuGenSpyControl1.Size = new System.Drawing.Size(848, 718);
			this.nuGenSpyControl1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(848, 718);
			this.Controls.Add(this.nuGenSpyControl1);
			this.Name = "Form1";
			this.Text = "Sharp Eye";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}		
	}
}
