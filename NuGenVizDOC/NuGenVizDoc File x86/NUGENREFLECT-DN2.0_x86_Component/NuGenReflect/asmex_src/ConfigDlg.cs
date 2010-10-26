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
using System.IO;
using System.Windows.Forms.Design;

namespace Asmex
{
	/// <summary>
	/// Boring dialog for setting startup options
	/// </summary>
	internal class ConfigDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.RadioButton rbBlank;
		private System.Windows.Forms.RadioButton rbPath;
		private System.Windows.Forms.RadioButton rbCommon;
		private System.Windows.Forms.RadioButton rbRestore;
		private System.Windows.Forms.ListView lvPath;
		private System.Windows.Forms.Button btnPathAdd;
		private System.Windows.Forms.Button btnPathRem;

		MainFrame _main;

		public ConfigDlg(MainFrame main)
		{

			InitializeComponent();

			_main = main;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConfigDlg));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbBlank = new System.Windows.Forms.RadioButton();
			this.rbPath = new System.Windows.Forms.RadioButton();
			this.rbCommon = new System.Windows.Forms.RadioButton();
			this.rbRestore = new System.Windows.Forms.RadioButton();
			this.lvPath = new System.Windows.Forms.ListView();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.btnPathAdd = new System.Windows.Forms.Button();
			this.btnPathRem = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbBlank);
			this.groupBox1.Controls.Add(this.rbPath);
			this.groupBox1.Controls.Add(this.rbCommon);
			this.groupBox1.Controls.Add(this.rbRestore);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(184, 152);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "On Startup:";
			// 
			// rbBlank
			// 
			this.rbBlank.Location = new System.Drawing.Point(16, 112);
			this.rbBlank.Name = "rbBlank";
			this.rbBlank.Size = new System.Drawing.Size(152, 32);
			this.rbBlank.TabIndex = 3;
			this.rbBlank.Text = "Blank";
			// 
			// rbPath
			// 
			this.rbPath.Location = new System.Drawing.Point(16, 80);
			this.rbPath.Name = "rbPath";
			this.rbPath.Size = new System.Drawing.Size(152, 32);
			this.rbPath.TabIndex = 2;
			this.rbPath.Text = "Show All Assemblies in Path";
			// 
			// rbCommon
			// 
			this.rbCommon.Location = new System.Drawing.Point(16, 48);
			this.rbCommon.Name = "rbCommon";
			this.rbCommon.Size = new System.Drawing.Size(152, 32);
			this.rbCommon.TabIndex = 1;
			this.rbCommon.Text = "Show Common Assemblies";
			// 
			// rbRestore
			// 
			this.rbRestore.Location = new System.Drawing.Point(16, 16);
			this.rbRestore.Name = "rbRestore";
			this.rbRestore.Size = new System.Drawing.Size(152, 32);
			this.rbRestore.TabIndex = 0;
			this.rbRestore.Text = "Restore Previous State";
			// 
			// lvPath
			// 
			this.lvPath.Location = new System.Drawing.Point(208, 40);
			this.lvPath.Name = "lvPath";
			this.lvPath.Size = new System.Drawing.Size(248, 120);
			this.lvPath.TabIndex = 1;
			this.lvPath.View = System.Windows.Forms.View.List;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(208, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Path:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(136, 176);
			this.button1.Name = "button1";
			this.button1.TabIndex = 3;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.OnOK);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(264, 176);
			this.button2.Name = "button2";
			this.button2.TabIndex = 4;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.OnCancel);
			// 
			// btnPathAdd
			// 
			this.btnPathAdd.Location = new System.Drawing.Point(368, 8);
			this.btnPathAdd.Name = "btnPathAdd";
			this.btnPathAdd.Size = new System.Drawing.Size(32, 24);
			this.btnPathAdd.TabIndex = 5;
			this.btnPathAdd.Text = "Add";
			this.btnPathAdd.Click += new System.EventHandler(this.btnPathAdd_Click);
			// 
			// btnPathRem
			// 
			this.btnPathRem.Location = new System.Drawing.Point(400, 8);
			this.btnPathRem.Name = "btnPathRem";
			this.btnPathRem.Size = new System.Drawing.Size(56, 24);
			this.btnPathRem.TabIndex = 6;
			this.btnPathRem.Text = "Remove";
			this.btnPathRem.Click += new System.EventHandler(this.btnPathRem_Click);
			// 
			// ConfigDlg
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(472, 209);
			this.Controls.Add(this.btnPathRem);
			this.Controls.Add(this.btnPathAdd);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lvPath);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ConfigDlg";
			this.Text = "Preferences";
			this.Load += new System.EventHandler(this.ConfigDlg_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OnOK(object sender, System.EventArgs e)
		{
			if (rbRestore.Checked)
			{
				_main.SUA = MainFrame.StartUpAction.Restore;
			}
			else if (rbCommon.Checked)
			{
				_main.SUA = MainFrame.StartUpAction.Common;
			}
			else if (rbPath.Checked)
			{
				_main.SUA = MainFrame.StartUpAction.Path;
			}
			else 
			{
				_main.SUA = MainFrame.StartUpAction.Blank;
			}

			_main.Path = GetPath();

			Close();
		}

		private void OnCancel(object sender, System.EventArgs e)
		{
			Close();
		}

		string[] GetPath()
		{
			string[] p = new string[lvPath.Items.Count];

			for(int i=0; i < lvPath.Items.Count; ++i)
			{
				p[i] = lvPath.Items[i].Text;
			}
		
			return p;

		}

		private void btnPathRem_Click(object sender, System.EventArgs e)
		{
			try
			{
				lvPath.Items.Remove(lvPath.SelectedItems[0]);
			}
			catch//(Exception ee)
			{
			}
		}

		private void btnPathAdd_Click(object sender, System.EventArgs e)
		{
			DirBrowser dlg = new DirBrowser();

			if(dlg.ShowDialog() == DialogResult.OK)
			{
				lvPath.Items.Add(dlg.ReturnPath);
			}

		}

		private void ConfigDlg_Load(object sender, System.EventArgs e)
		{
			switch(_main.SUA)
			{
				case MainFrame.StartUpAction.Restore:
					rbRestore.Checked=true;
					break;
				case MainFrame.StartUpAction.Common:
					rbCommon.Checked=true;
					break;
				case MainFrame.StartUpAction.Path:
					rbPath.Checked=true;
					break;
				default:
					rbBlank.Checked=true;
					break;
			}

			string[] s = _main.Path;

			for(int i=0;i<s.Length;++i)
			{
				lvPath.Items.Add(s[i]);
			}

		}
	}


	/// <summary>
	/// Use the dreaded undocumented .NET shell classes to get a folder picker
	/// </summary>
	internal class DirBrowser:FolderNameEditor
	{
		FolderBrowser fb = new FolderBrowser();

		public string Description
		{
			set { _description = value; }
			get { return _description; }
		}

		public string ReturnPath
		{
			get { return _returnPath; }
		}

		public DirBrowser() { }

		private DialogResult RunDialog()
		{
			fb.Description = this.Description;
			fb.StartLocation = FolderBrowserFolder.MyComputer;

			DialogResult r = fb.ShowDialog();
			if (r == DialogResult.OK)
				_returnPath = fb.DirectoryPath;
			else
				_returnPath = String.Empty;

			return r;
		}

		public DialogResult ShowDialog()
		{
			return RunDialog();
		}

		private string _description = "Choose Directory";
		private string _returnPath = String.Empty;
	}
}
