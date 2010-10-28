using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for CompilerOptionsForm.
	/// </summary>
	public class CompilerOptionsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button imports;
		private System.Windows.Forms.CheckBox overflow;
		private System.Windows.Forms.CheckBox debug;
		private System.Windows.Forms.CheckBox opt;
		private System.Windows.Forms.CheckBox allowunsafe;
		private System.Windows.Forms.ComboBox warnings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private GraphControl graph;
		private ImportForm importForm;

		public CompilerOptionsForm(GraphControl graph)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
			importForm = new ImportForm(graph);
			warnings.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CompilerOptionsForm));
			this.imports = new System.Windows.Forms.Button();
			this.overflow = new System.Windows.Forms.CheckBox();
			this.debug = new System.Windows.Forms.CheckBox();
			this.opt = new System.Windows.Forms.CheckBox();
			this.allowunsafe = new System.Windows.Forms.CheckBox();
			this.warnings = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ok = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// imports
			// 
			this.imports.Location = new System.Drawing.Point(8, 16);
			this.imports.Name = "imports";
			this.imports.Size = new System.Drawing.Size(128, 23);
			this.imports.TabIndex = 0;
			this.imports.Text = "Imported libraries...";
			this.imports.Click += new System.EventHandler(this.impClick);
			// 
			// overflow
			// 
			this.overflow.Location = new System.Drawing.Point(8, 56);
			this.overflow.Name = "overflow";
			this.overflow.Size = new System.Drawing.Size(160, 24);
			this.overflow.TabIndex = 1;
			this.overflow.Text = "Check arithmetic overflow";
			// 
			// debug
			// 
			this.debug.Location = new System.Drawing.Point(8, 80);
			this.debug.Name = "debug";
			this.debug.Size = new System.Drawing.Size(160, 24);
			this.debug.TabIndex = 2;
			this.debug.Text = "Include debug info";
			// 
			// opt
			// 
			this.opt.Location = new System.Drawing.Point(8, 104);
			this.opt.Name = "opt";
			this.opt.TabIndex = 3;
			this.opt.Text = "Optimize code";
			// 
			// allowunsafe
			// 
			this.allowunsafe.Location = new System.Drawing.Point(8, 128);
			this.allowunsafe.Name = "allowunsafe";
			this.allowunsafe.Size = new System.Drawing.Size(152, 24);
			this.allowunsafe.TabIndex = 4;
			this.allowunsafe.Text = "Allow unsafe constructs";
			// 
			// warnings
			// 
			this.warnings.Items.AddRange(new object[] {
																									"No warnings",
																									"Level 1",
																									"Level 2",
																									"Level 3",
																									"Level 4"});
			this.warnings.Location = new System.Drawing.Point(88, 160);
			this.warnings.Name = "warnings";
			this.warnings.Size = new System.Drawing.Size(121, 21);
			this.warnings.TabIndex = 5;
			this.warnings.Text = "No warnings";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 160);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Warning level:";
			// 
			// ok
			// 
			this.ok.Location = new System.Drawing.Point(8, 200);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(80, 24);
			this.ok.TabIndex = 7;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.okClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(0, 184);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(216, 8);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(104, 200);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(80, 24);
			this.cancel.TabIndex = 9;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancelClick);
			// 
			// CompilerOptionsForm
			// 
			this.AcceptButton = this.ok;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(216, 227);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.warnings);
			this.Controls.Add(this.allowunsafe);
			this.Controls.Add(this.opt);
			this.Controls.Add(this.debug);
			this.Controls.Add(this.overflow);
			this.Controls.Add(this.imports);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CompilerOptionsForm";
			this.ShowInTaskbar = false;
			this.Text = "Compiler options";
			this.ResumeLayout(false);

		}
		#endregion

	
		public void Reset() {
			ListDictionary list = graph.Model.CompilerOptions;
			overflow.Checked = ((list.Contains("checked")) && ((bool)list["checked"] == true));
			debug.Checked = ((list.Contains("debug")) && ((bool)list["debug"] == true));
			opt.Checked = ((list.Contains("o")) && ((bool)list["o"] == true));
			allowunsafe.Checked = ((list.Contains("unsafe")) && ((bool)list["unsafe"] == true));
			if (list.Contains("w")) {
				warnings.SelectedIndex = int.Parse((string)list["w"]);
			} else {
				warnings.SelectedIndex = 4;
			}
		}

		public void Apply() {
			ListDictionary list = graph.Model.CompilerOptions;
			list.Clear();
			list.Add("target", "library");
			list.Add("checked", overflow.Checked);
			list.Add("debug", debug.Checked);
			list.Add("o", opt.Checked);
			list.Add("unsafe", allowunsafe.Checked);
			list.Add("w", warnings.SelectedIndex.ToString());
		}
		
		private void impClick(object sender, System.EventArgs e) {
			if (importForm.IsDisposed) importForm = new ImportForm(graph);
			importForm.Reset();
			importForm.Show();
			importForm.BringToFront();
		}

		private void okClick(object sender, System.EventArgs e) {
			Apply();
			this.Hide();
		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Hide();
		}

	}
}
