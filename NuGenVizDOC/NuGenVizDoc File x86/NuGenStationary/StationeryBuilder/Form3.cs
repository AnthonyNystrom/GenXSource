using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace StationeryBuilder
{
	/// <summary>
	/// Summary description for Form3.
	/// </summary>
	public class Form3 : System.Windows.Forms.Form
	{
		private Agilix.Ink.Scribble.ScribbleBox scribbleBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form3()
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
			this.scribbleBox1 = new Agilix.Ink.Scribble.ScribbleBox();
			this.SuspendLayout();
			// 
			// scribbleBox1
			// 
			this.scribbleBox1.BackColor = System.Drawing.SystemColors.Control;
			this.scribbleBox1.ForceSwapFont = new System.Drawing.Font("Times New Roman", 10F);
			this.scribbleBox1.HighlightElementColor = System.Drawing.Color.FromArgb(((System.Byte)(189)), ((System.Byte)(208)), ((System.Byte)(220)));
			this.scribbleBox1.Location = new System.Drawing.Point(4, 4);
			this.scribbleBox1.Name = "scribbleBox1";
			this.scribbleBox1.Size = new System.Drawing.Size(472, 464);
			this.scribbleBox1.TabColor = System.Drawing.SystemColors.ControlDark;
			this.scribbleBox1.TabIndex = 0;
			this.scribbleBox1.Text = "scribbleBox1";
			// 
			// Form3
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.scribbleBox1);
			this.Name = "Form3";
			this.Text = "Form3";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
