using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for importForm.
	/// </summary>
	public class ImportForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button ok;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.TextBox importsText;
		private System.Windows.Forms.Button fileButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button cancel;
		private GraphControl graph;

		public ImportForm(GraphControl graph)
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
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ImportForm));
			this.ok = new System.Windows.Forms.Button();
			this.importsText = new System.Windows.Forms.TextBox();
			this.fileButton = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ok.Location = new System.Drawing.Point(8, 157);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(72, 24);
			this.ok.TabIndex = 1;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.okClick);
			// 
			// importsText
			// 
			this.importsText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.importsText.Location = new System.Drawing.Point(0, 0);
			this.importsText.Multiline = true;
			this.importsText.Name = "importsText";
			this.importsText.Size = new System.Drawing.Size(288, 149);
			this.importsText.TabIndex = 2;
			this.importsText.Text = "";
			// 
			// fileButton
			// 
			this.fileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.fileButton.Location = new System.Drawing.Point(184, 157);
			this.fileButton.Name = "fileButton";
			this.fileButton.Size = new System.Drawing.Size(96, 24);
			this.fileButton.TabIndex = 3;
			this.fileButton.Text = "Import file...";
			this.fileButton.Click += new System.EventHandler(this.fileClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Libraries (*.dll;*.exe) | *.dll; *.exe";
			this.openFileDialog.Multiselect = true;
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(96, 157);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(72, 24);
			this.cancel.TabIndex = 4;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancelClick);
			// 
			// ImportForm
			// 
			this.AcceptButton = this.ok;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(288, 184);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.fileButton);
			this.Controls.Add(this.importsText);
			this.Controls.Add(this.ok);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(296, 168);
			this.Name = "ImportForm";
			this.ShowInTaskbar = false;
			this.Text = "Import Assemblies";
			this.ResumeLayout(false);

		}
		#endregion

		private void okClick(object sender, System.EventArgs e)
		{
			int n;
			string[] lines, buf;
			lines = importsText.Lines;
			n = lines.Length;
			while ((n > 0) && (lines[n-1] == "")) {n--;}
			buf = new string[n];
			for (int i = 0; i < n; i++) {
				buf[i] = lines[i];
			}
			graph.Model.CompilerImports = buf;
			this.Hide();
		}

		public void Reset() {
			importsText.Lines = graph.Model.CompilerImports;
		}

		private void fileClick(object sender, System.EventArgs e) {
			DialogResult res = openFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				int i;
				string[] lines = new string[importsText.Lines.Length + openFileDialog.FileNames.Length];
				for (i = 0; i < importsText.Lines.Length; i++) {
					lines[i] = importsText.Lines[i];
				}
				for (;i < lines.Length; i++) {
					lines[i] = openFileDialog.FileNames[i-importsText.Lines.Length];
				}
				importsText.Lines = lines;
			}

		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Hide();
		}

	}
}
