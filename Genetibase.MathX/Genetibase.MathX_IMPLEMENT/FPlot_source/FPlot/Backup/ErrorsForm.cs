using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for ErrorsForm.
	/// </summary>
	public class ErrorsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox text;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private GraphControl graph;

		public ErrorsForm(GraphControl graph)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ErrorsForm));
			this.text = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// text
			// 
			this.text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.text.Location = new System.Drawing.Point(0, 0);
			this.text.Multiline = true;
			this.text.Name = "text";
			this.text.ReadOnly = true;
			this.text.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.text.Size = new System.Drawing.Size(400, 128);
			this.text.TabIndex = 0;
			this.text.Text = "";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(0, 136);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(67, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Close";
			this.button1.Click += new System.EventHandler(this.closeClick);
			// 
			// ErrorsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 163);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.text);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ErrorsForm";
			this.ShowInTaskbar = false;
			this.Text = "Compilation errors";
			this.ResumeLayout(false);

		}
		#endregion

		public void Reset() {
			text.Lines = graph.Model.Library.errors;
		}

		private void closeClick(object sender, System.EventArgs e) {
			this.Hide();
		}
	}
}
