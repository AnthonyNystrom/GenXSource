/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Genetibase.Debug
{

	/// <summary>
	/// Dialog that selects an assembly from the GAC.  It enumerates assemblies by actually looking
	/// for DLLs within the GAC directory.  Later versions of the framework will probably provide
	/// a better way to do this.
	/// </summary>
	internal class GACPicker : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;
		private Janus.Windows.EditControls.UIButton btnOK;
		private Janus.Windows.EditControls.UIButton btnCxl;
		private System.Windows.Forms.ListView lvAsms;

		Assembly _assembly;
		private System.Windows.Forms.ColumnHeader AssemblyName;
		private System.Windows.Forms.ColumnHeader Version;
		private System.Windows.Forms.ColumnHeader Culture;
		private DevComponents.DotNetBar.PanelEx panelEx1;
		ArrayList _asms;

		public GACPicker()
		{
			InitializeComponent();

			_asms = new ArrayList();
		}

		public Assembly Assembly{get{return _assembly;}}

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GACPicker));
            this.btnOK = new Janus.Windows.EditControls.UIButton();
            this.btnCxl = new Janus.Windows.EditControls.UIButton();
            this.lvAsms = new System.Windows.Forms.ListView();
            this.AssemblyName = new System.Windows.Forms.ColumnHeader();
            this.Version = new System.Windows.Forms.ColumnHeader();
            this.Culture = new System.Windows.Forms.ColumnHeader();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(288, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCxl
            // 
            this.btnCxl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCxl.BackColor = System.Drawing.SystemColors.Control;
            this.btnCxl.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCxl.Location = new System.Drawing.Point(360, 8);
            this.btnCxl.Name = "btnCxl";
            this.btnCxl.Size = new System.Drawing.Size(72, 24);
            this.btnCxl.TabIndex = 2;
            this.btnCxl.Text = "Cancel";
            this.btnCxl.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // lvAsms
            // 
            this.lvAsms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AssemblyName,
            this.Version,
            this.Culture});
            this.lvAsms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAsms.Location = new System.Drawing.Point(0, 0);
            this.lvAsms.Name = "lvAsms";
            this.lvAsms.Size = new System.Drawing.Size(440, 576);
            this.lvAsms.TabIndex = 3;
            this.lvAsms.UseCompatibleStateImageBehavior = false;
            this.lvAsms.View = System.Windows.Forms.View.Details;
            this.lvAsms.DoubleClick += new System.EventHandler(this.btnOK_Click);
            // 
            // AssemblyName
            // 
            this.AssemblyName.Text = "Assembly Name";
            this.AssemblyName.Width = 180;
            // 
            // Version
            // 
            this.Version.Text = "Version";
            this.Version.Width = 100;
            // 
            // Culture
            // 
            this.Culture.Text = "Culture";
            this.Culture.Width = 120;
            // 
            // panelEx1
            // 
            this.panelEx1.Controls.Add(this.btnCxl);
            this.panelEx1.Controls.Add(this.btnOK);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx1.Location = new System.Drawing.Point(0, 576);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(440, 40);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 4;
            // 
            // GACPicker
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(440, 616);
            this.ControlBox = false;
            this.Controls.Add(this.lvAsms);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GACPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Asssembly from GAC";
            this.Load += new System.EventHandler(this.GACPicker_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			ListViewItem it = lvAsms.SelectedItems[0];

			if (it == null)
			{
				MessageBox.Show("Please select an assembly.");
				return;
			}

			try
			{
				_assembly = Assembly.Load(it.SubItems[3].Text);
			}
			catch//(Exception ee)
			{
				MessageBox.Show("Unable to load selected assembly!");
				return;
			}

			this.DialogResult = DialogResult.OK;
			this.Close();

		}

		private void GACPicker_Load(object sender, System.EventArgs e)
		{
			string root = Environment.SystemDirectory;
			
			int n = root.LastIndexOf("\\");
			root = root.Substring(0, n);
			root += "\\Assembly\\GAC";

			try
			{
				RecurseDirs(root);
			}
			catch//(Exception ee)
			{
				MessageBox.Show("Unable to enumerate assemblies.  Does this account have permission to read %systemroot%\\system32\\assemblies?");
			}

			foreach (Assembly asm in _asms)
			{
				lvAsms.Items.Add(new ListViewItem(
					new string[]{ 
									asm.GetName().Name,
									asm.GetName().Version.ToString(),
									asm.GetName().CultureInfo.Name,
									asm.FullName
								}
					));
			}
		}

		void RecurseDirs(string dir)
		{
			Assembly asm;
			string[] files = Directory.GetFiles(dir);

			for(int i=0;i<files.Length;++i)
			{
				asm = null;

				try
				{
					asm = Assembly.LoadFrom(files[i]);
				}
				catch{}
				
				if (asm != null)
				{
					_asms.Add(asm);
				}
			}

			string[] dirs = Directory.GetDirectories(dir);

			for(int i=0; i< dirs.Length;++i)
			{
				RecurseDirs(dirs[i]);
			}
		}
	}
	
}