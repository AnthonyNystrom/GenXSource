using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for SnoopTraceControl.
	/// </summary>
	public class SnoopTraceControl : System.Windows.Forms.UserControl
	{
		private Genetibase.Debug.NuGenSnoopControl nuGenSnoopControl1;
		private System.Windows.Forms.Splitter splitter1;
		private Genetibase.Debug.NuGenOInternal nuGenOInternal1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SnoopTraceControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public NuGenSnoopControl Snoop
		{
			get
			{
				return nuGenSnoopControl1;
			}
		}
		
		public NuGenOInternal Tracer
		{
			get
			{
				return nuGenOInternal1;
			}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.nuGenSnoopControl1 = new Genetibase.Debug.NuGenSnoopControl();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.nuGenOInternal1 = new Genetibase.Debug.NuGenOInternal();
			this.SuspendLayout();
			// 
			// nuGenSnoopControl1
			// 
			this.nuGenSnoopControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.nuGenSnoopControl1.Location = new System.Drawing.Point(0, 0);
			this.nuGenSnoopControl1.Name = "nuGenSnoopControl1";
			this.nuGenSnoopControl1.RecurseIntoSubControls = true;
			this.nuGenSnoopControl1.RecursionDepth = 10;
			this.nuGenSnoopControl1.ScrollingEnabled = true;
			this.nuGenSnoopControl1.SelectedObject = null;
			this.nuGenSnoopControl1.Size = new System.Drawing.Size(824, 448);
			this.nuGenSnoopControl1.TabIndex = 0;
			this.nuGenSnoopControl1.TrackingEnabled = true;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 448);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(824, 3);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// nuGenOInternal1
			// 
			this.nuGenOInternal1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.nuGenOInternal1.Location = new System.Drawing.Point(0, 456);
			this.nuGenOInternal1.Name = "nuGenOInternal1";
			this.nuGenOInternal1.Size = new System.Drawing.Size(824, 232);
			this.nuGenOInternal1.TabIndex = 2;
			// 
			// SnoopTraceControl
			// 
			this.Controls.Add(this.nuGenOInternal1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.nuGenSnoopControl1);
			this.Name = "SnoopTraceControl";
			this.Size = new System.Drawing.Size(824, 688);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
