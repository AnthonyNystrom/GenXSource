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
	public class FindDialog : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindDialog));
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
            this.cbEvent = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbCaseful = new System.Windows.Forms.CheckBox();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbStr
            // 
            this.tbStr.AccessibleDescription = null;
            this.tbStr.AccessibleName = null;
            resources.ApplyResources(this.tbStr, "tbStr");
            this.tbStr.BackgroundImage = null;
            this.tbStr.Font = null;
            this.tbStr.Name = "tbStr";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.BackgroundImage = null;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbStartAtRoot
            // 
            this.cbStartAtRoot.AccessibleDescription = null;
            this.cbStartAtRoot.AccessibleName = null;
            resources.ApplyResources(this.cbStartAtRoot, "cbStartAtRoot");
            this.cbStartAtRoot.BackColor = System.Drawing.Color.Transparent;
            this.cbStartAtRoot.BackgroundImage = null;
            this.cbStartAtRoot.Font = null;
            this.cbStartAtRoot.Name = "cbStartAtRoot";
            this.cbStartAtRoot.UseVisualStyleBackColor = false;
            // 
            // cbWhole
            // 
            this.cbWhole.AccessibleDescription = null;
            this.cbWhole.AccessibleName = null;
            resources.ApplyResources(this.cbWhole, "cbWhole");
            this.cbWhole.BackColor = System.Drawing.Color.Transparent;
            this.cbWhole.BackgroundImage = null;
            this.cbWhole.Font = null;
            this.cbWhole.Name = "cbWhole";
            this.cbWhole.UseVisualStyleBackColor = false;
            // 
            // cbType
            // 
            this.cbType.AccessibleDescription = null;
            this.cbType.AccessibleName = null;
            resources.ApplyResources(this.cbType, "cbType");
            this.cbType.BackColor = System.Drawing.Color.Transparent;
            this.cbType.BackgroundImage = null;
            this.cbType.Font = null;
            this.cbType.Name = "cbType";
            this.cbType.UseVisualStyleBackColor = false;
            // 
            // cbMethod
            // 
            this.cbMethod.AccessibleDescription = null;
            this.cbMethod.AccessibleName = null;
            resources.ApplyResources(this.cbMethod, "cbMethod");
            this.cbMethod.BackColor = System.Drawing.Color.Transparent;
            this.cbMethod.BackgroundImage = null;
            this.cbMethod.Font = null;
            this.cbMethod.Name = "cbMethod";
            this.cbMethod.UseVisualStyleBackColor = false;
            // 
            // cbParameter
            // 
            this.cbParameter.AccessibleDescription = null;
            this.cbParameter.AccessibleName = null;
            resources.ApplyResources(this.cbParameter, "cbParameter");
            this.cbParameter.BackColor = System.Drawing.Color.Transparent;
            this.cbParameter.BackgroundImage = null;
            this.cbParameter.Font = null;
            this.cbParameter.Name = "cbParameter";
            this.cbParameter.UseVisualStyleBackColor = false;
            // 
            // cbField
            // 
            this.cbField.AccessibleDescription = null;
            this.cbField.AccessibleName = null;
            resources.ApplyResources(this.cbField, "cbField");
            this.cbField.BackColor = System.Drawing.Color.Transparent;
            this.cbField.BackgroundImage = null;
            this.cbField.Font = null;
            this.cbField.Name = "cbField";
            this.cbField.UseVisualStyleBackColor = false;
            // 
            // cbResource
            // 
            this.cbResource.AccessibleDescription = null;
            this.cbResource.AccessibleName = null;
            resources.ApplyResources(this.cbResource, "cbResource");
            this.cbResource.BackgroundImage = null;
            this.cbResource.Font = null;
            this.cbResource.Name = "cbResource";
            // 
            // checkBox8
            // 
            this.checkBox8.AccessibleDescription = null;
            this.checkBox8.AccessibleName = null;
            resources.ApplyResources(this.checkBox8, "checkBox8");
            this.checkBox8.BackColor = System.Drawing.Color.Transparent;
            this.checkBox8.BackgroundImage = null;
            this.checkBox8.Font = null;
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.cbResource);
            this.groupBox1.Controls.Add(this.cbProperty);
            this.groupBox1.Controls.Add(this.cbParameter);
            this.groupBox1.Controls.Add(this.cbField);
            this.groupBox1.Controls.Add(this.cbEvent);
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.cbMethod);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbProperty
            // 
            this.cbProperty.AccessibleDescription = null;
            this.cbProperty.AccessibleName = null;
            resources.ApplyResources(this.cbProperty, "cbProperty");
            this.cbProperty.BackgroundImage = null;
            this.cbProperty.Font = null;
            this.cbProperty.Name = "cbProperty";
            // 
            // cbEvent
            // 
            this.cbEvent.AccessibleDescription = null;
            this.cbEvent.AccessibleName = null;
            resources.ApplyResources(this.cbEvent, "cbEvent");
            this.cbEvent.BackColor = System.Drawing.Color.Transparent;
            this.cbEvent.BackgroundImage = null;
            this.cbEvent.Font = null;
            this.cbEvent.Name = "cbEvent";
            this.cbEvent.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.cbCaseful);
            this.groupBox2.Controls.Add(this.cbWhole);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cbCaseful
            // 
            this.cbCaseful.AccessibleDescription = null;
            this.cbCaseful.AccessibleName = null;
            resources.ApplyResources(this.cbCaseful, "cbCaseful");
            this.cbCaseful.BackgroundImage = null;
            this.cbCaseful.Font = null;
            this.cbCaseful.Name = "cbCaseful";
            // 
            // panelEx1
            // 
            this.panelEx1.AccessibleDescription = null;
            this.panelEx1.AccessibleName = null;
            resources.ApplyResources(this.panelEx1, "panelEx1");
            this.panelEx1.Controls.Add(this.cbStartAtRoot);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.checkBox8);
            this.panelEx1.Controls.Add(this.groupBox1);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.groupBox2);
            this.panelEx1.Controls.Add(this.tbStr);
            this.panelEx1.Controls.Add(this.btnOK);
            this.panelEx1.Font = null;
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx1.Style.GradientAngle = 90;
            // 
            // FindDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.ControlBox = false;
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FindDialog";
            //this.Load += new System.EventHandler(this.FindDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
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

        //private void FindDialog_Load(object sender, EventArgs e)
        //{
        //    Genetibase.UI.NuGenUISnap SnapUIAbout = new Genetibase.UI.NuGenUISnap(this);
        //    SnapUIAbout.StickOnMove = true;
        //    SnapUIAbout.StickToScreen = true;
        //    SnapUIAbout.StickToOther = true;
        //}
	}
}
