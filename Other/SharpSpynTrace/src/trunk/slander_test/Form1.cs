using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace slander_test
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Genetibase.Debug.NuGenWindowFinder windowFinder1;
		private System.Windows.Forms.TextBox pid_;
		private System.Windows.Forms.TextBox tid_;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox hwnd_;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox is_managed_;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox classname_;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.windowFinder1 = new Genetibase.Debug.NuGenWindowFinder();
			this.pid_ = new System.Windows.Forms.TextBox();
			this.tid_ = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.hwnd_ = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.is_managed_ = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.classname_ = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// windowFinder1
			// 
			this.windowFinder1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("windowFinder1.BackgroundImage")));
			this.windowFinder1.Location = new System.Drawing.Point(96, 16);
			this.windowFinder1.Name = "windowFinder1";
			this.windowFinder1.Size = new System.Drawing.Size(112, 96);
			this.windowFinder1.TabIndex = 0;
			this.windowFinder1.ActiveWindowChanged += new System.EventHandler(this.windowFinder1_ActiveWindowChanged);
			// 
			// pid_
			// 
			this.pid_.Location = new System.Drawing.Point(128, 168);
			this.pid_.Name = "pid_";
			this.pid_.Size = new System.Drawing.Size(152, 22);
			this.pid_.TabIndex = 1;
			this.pid_.Text = "";
			// 
			// tid_
			// 
			this.tid_.Location = new System.Drawing.Point(128, 208);
			this.tid_.Name = "tid_";
			this.tid_.Size = new System.Drawing.Size(152, 22);
			this.tid_.TabIndex = 1;
			this.tid_.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 168);
			this.label1.Name = "label1";
			this.label1.TabIndex = 2;
			this.label1.Text = "Process ID";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 208);
			this.label2.Name = "label2";
			this.label2.TabIndex = 2;
			this.label2.Text = "Thread ID";
			// 
			// hwnd_
			// 
			this.hwnd_.Location = new System.Drawing.Point(128, 128);
			this.hwnd_.Name = "hwnd_";
			this.hwnd_.Size = new System.Drawing.Size(152, 22);
			this.hwnd_.TabIndex = 1;
			this.hwnd_.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 128);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "HWND";
			// 
			// is_managed_
			// 
			this.is_managed_.Location = new System.Drawing.Point(88, 280);
			this.is_managed_.Name = "is_managed_";
			this.is_managed_.TabIndex = 3;
			this.is_managed_.Text = "Is Managed";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 248);
			this.label4.Name = "label4";
			this.label4.TabIndex = 2;
			this.label4.Text = "Class Name";
			// 
			// classname_
			// 
			this.classname_.Location = new System.Drawing.Point(128, 248);
			this.classname_.Name = "classname_";
			this.classname_.Size = new System.Drawing.Size(152, 22);
			this.classname_.TabIndex = 1;
			this.classname_.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(296, 312);
			this.Controls.Add(this.is_managed_);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pid_);
			this.Controls.Add(this.windowFinder1);
			this.Controls.Add(this.tid_);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.hwnd_);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.classname_);
			this.Name = "Form1";
			this.Text = "Form1";
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

		private void windowFinder1_ActiveWindowChanged(object sender, System.EventArgs e)
		{
//			MessageBox.Show("Ha");

			hwnd_.Text = windowFinder1.Window.HWND.ToString();
			pid_.Text = windowFinder1.Window.ProcessID.ToString();
			tid_.Text = windowFinder1.Window.ThreadID.ToString();
			classname_.Text = windowFinder1.Window.ClassName;
			is_managed_.Checked = windowFinder1.Window.IsManaged;
		}
	}
}
