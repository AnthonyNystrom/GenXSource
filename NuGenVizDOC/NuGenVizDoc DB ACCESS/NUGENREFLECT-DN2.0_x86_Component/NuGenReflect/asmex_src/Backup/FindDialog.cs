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

namespace Genetibase.Debug
{
	/// <summary>
	/// Dialog for finding things -- ie for displaying/editing a FindState.
	/// </summary>
	internal class FindDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbStr;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.EditControls.UIButton btnCancel;
		private Janus.Windows.EditControls.UIButton btnOK;
		private System.Windows.Forms.CheckBox cbStartAtRoot;
		private System.Windows.Forms.CheckBox cbWhole;
		private System.Windows.Forms.CheckBox checkBox8;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbType;
		private System.Windows.Forms.CheckBox cbMethod;
		private System.Windows.Forms.CheckBox cbParameter;
		private System.Windows.Forms.CheckBox cbField;
		private System.Windows.Forms.CheckBox cbResource;
		private System.Windows.Forms.CheckBox cbEvent;
		private System.Windows.Forms.CheckBox cbProperty;
		private System.Windows.Forms.CheckBox cbCaseful;
		private DevComponents.DotNetBar.PanelEx panelEx1;
		
		private System.ComponentModel.Container components = null;

		public FindDialog(FindState f)
		{
			InitializeComponent();

			Find = f;	
		}

		public FindState Find
		{
			get
			{
				FindState f = new FindState();
				f.String = tbStr.Text;

				if (cbType.Checked)
					f.FindTypes = true;
				if (cbMethod.Checked)
					f.FindMethods = true;
				if (cbParameter.Checked)
					f.FindParameters = true;
				if (cbField.Checked)
					f.FindFields = true;
				if (cbResource.Checked)
					f.FindResources = true;
				if (cbProperty.Checked)
					f.FindProperties = true;
				if (cbEvent.Checked)
					f.FindEvents = true;

				if (cbWhole.Checked)
					f.WholeWord = true;

				if (cbStartAtRoot.Checked)
					f.StartAtRoot = true;

				if (cbCaseful.Checked)
					f.Caseful = true;

				return f;
			}

			set
			{
				if (value == null)
					value = new FindState();

				tbStr.Text = value.String;

				if (value.FindTypes)
					cbType.Checked = true;
				if (value.FindMethods)
					cbMethod.Checked = true;
				if (value.FindParameters)
					cbParameter.Checked = true;
				if (value.FindFields)
					cbField.Checked = true;
				if (value.FindEvents)
					cbEvent.Checked = true;
				if (value.FindResources)
					cbResource.Checked = true;
				if (value.FindProperties)
					cbProperty.Checked = true;
				if (value.FindEvents)
					cbEvent.Checked = true;


				if (value.WholeWord)
					cbWhole.Checked = true;

				if (value.StartAtRoot)
					cbStartAtRoot.Checked = true;

				if (value.Caseful)
					cbCaseful.Checked = true;

			}

		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FindDialog));
			this.tbStr = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new Janus.Windows.EditControls.UIButton();
			this.btnOK = new Janus.Windows.EditControls.UIButton();
			this.cbStartAtRoot = new System.Windows.Forms.CheckBox();
			this.cbWhole = new System.Windows.Forms.CheckBox();
			this.cbType = new System.Windows.Forms.CheckBox();
			this.cbMethod = new System.Windows.Forms.CheckBox();
			this.cbParameter = new System.Windows.Forms.CheckBox();
			this.cbField = new System.Windows.Forms.CheckBox();
			this.cbResource = new System.Windows.Forms.CheckBox();
			this.checkBox8 = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbProperty = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbCaseful = new System.Windows.Forms.CheckBox();
			this.cbEvent = new System.Windows.Forms.CheckBox();
			this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panelEx1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbStr
			// 
			this.tbStr.Location = new System.Drawing.Point(80, 8);
			this.tbStr.Name = "tbStr";
			this.tbStr.Size = new System.Drawing.Size(336, 20);
			this.tbStr.TabIndex = 0;
			this.tbStr.Text = "";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 24);
			this.label1.TabIndex = 8;
			this.label1.Text = "Search For:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(344, 176);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 24);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.SystemColors.Control;
			this.btnOK.Location = new System.Drawing.Point(272, 176);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(72, 24);
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// cbStartAtRoot
			// 
			this.cbStartAtRoot.BackColor = System.Drawing.Color.Transparent;
			this.cbStartAtRoot.Location = new System.Drawing.Point(256, 64);
			this.cbStartAtRoot.Name = "cbStartAtRoot";
			this.cbStartAtRoot.Size = new System.Drawing.Size(152, 24);
			this.cbStartAtRoot.TabIndex = 12;
			this.cbStartAtRoot.Text = "Start From Root Of Tree";
			// 
			// cbWhole
			// 
			this.cbWhole.BackColor = System.Drawing.Color.Transparent;
			this.cbWhole.Location = new System.Drawing.Point(16, 48);
			this.cbWhole.Name = "cbWhole";
			this.cbWhole.Size = new System.Drawing.Size(152, 24);
			this.cbWhole.TabIndex = 13;
			this.cbWhole.Text = "Match Entire Name Only";
			// 
			// cbType
			// 
			this.cbType.BackColor = System.Drawing.Color.Transparent;
			this.cbType.Location = new System.Drawing.Point(16, 24);
			this.cbType.Name = "cbType";
			this.cbType.TabIndex = 14;
			this.cbType.Text = "Types";
			// 
			// cbMethod
			// 
			this.cbMethod.BackColor = System.Drawing.Color.Transparent;
			this.cbMethod.Location = new System.Drawing.Point(16, 48);
			this.cbMethod.Name = "cbMethod";
			this.cbMethod.TabIndex = 15;
			this.cbMethod.Text = "Methods";
			// 
			// cbParameter
			// 
			this.cbParameter.BackColor = System.Drawing.Color.Transparent;
			this.cbParameter.Location = new System.Drawing.Point(16, 72);
			this.cbParameter.Name = "cbParameter";
			this.cbParameter.TabIndex = 16;
			this.cbParameter.Text = "Parameters";
			// 
			// cbField
			// 
			this.cbField.BackColor = System.Drawing.Color.Transparent;
			this.cbField.Location = new System.Drawing.Point(120, 24);
			this.cbField.Name = "cbField";
			this.cbField.TabIndex = 18;
			this.cbField.Text = "Fields";
			// 
			// cbResource
			// 
			this.cbResource.Location = new System.Drawing.Point(120, 72);
			this.cbResource.Name = "cbResource";
			this.cbResource.TabIndex = 20;
			this.cbResource.Text = "Resources";
			// 
			// checkBox8
			// 
			this.checkBox8.BackColor = System.Drawing.Color.Transparent;
			this.checkBox8.Location = new System.Drawing.Point(256, 112);
			this.checkBox8.Name = "checkBox8";
			this.checkBox8.TabIndex = 21;
			this.checkBox8.Text = "Public Only";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.cbResource);
			this.groupBox1.Controls.Add(this.cbProperty);
			this.groupBox1.Controls.Add(this.cbParameter);
			this.groupBox1.Controls.Add(this.cbField);
			this.groupBox1.Controls.Add(this.cbEvent);
			this.groupBox1.Controls.Add(this.cbType);
			this.groupBox1.Controls.Add(this.cbMethod);
			this.groupBox1.Location = new System.Drawing.Point(8, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 128);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search For:";
			// 
			// cbProperty
			// 
			this.cbProperty.Location = new System.Drawing.Point(16, 96);
			this.cbProperty.Name = "cbProperty";
			this.cbProperty.TabIndex = 25;
			this.cbProperty.Text = "Properties";
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.cbCaseful);
			this.groupBox2.Controls.Add(this.cbWhole);
			this.groupBox2.Location = new System.Drawing.Point(240, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(176, 128);
			this.groupBox2.TabIndex = 23;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Options:";
			// 
			// cbCaseful
			// 
			this.cbCaseful.Location = new System.Drawing.Point(16, 96);
			this.cbCaseful.Name = "cbCaseful";
			this.cbCaseful.TabIndex = 25;
			this.cbCaseful.Text = "Case Sensitive";
			// 
			// cbEvent
			// 
			this.cbEvent.BackColor = System.Drawing.Color.Transparent;
			this.cbEvent.Location = new System.Drawing.Point(120, 48);
			this.cbEvent.Name = "cbEvent";
			this.cbEvent.TabIndex = 24;
			this.cbEvent.Text = "Events";
			// 
			// panelEx1
			// 
			this.panelEx1.Controls.Add(this.cbStartAtRoot);
			this.panelEx1.Controls.Add(this.label1);
			this.panelEx1.Controls.Add(this.checkBox8);
			this.panelEx1.Controls.Add(this.groupBox1);
			this.panelEx1.Controls.Add(this.btnCancel);
			this.panelEx1.Controls.Add(this.groupBox2);
			this.panelEx1.Controls.Add(this.tbStr);
			this.panelEx1.Controls.Add(this.btnOK);
			this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelEx1.Location = new System.Drawing.Point(0, 0);
			this.panelEx1.Name = "panelEx1";
			this.panelEx1.Size = new System.Drawing.Size(426, 208);
			this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			this.panelEx1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
			this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
			this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
			this.panelEx1.Style.GradientAngle = 90;
			this.panelEx1.TabIndex = 25;
			// 
			// FindDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(426, 208);
			this.ControlBox = false;
			this.Controls.Add(this.panelEx1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FindDialog";
			this.Text = "Find";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.panelEx1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
