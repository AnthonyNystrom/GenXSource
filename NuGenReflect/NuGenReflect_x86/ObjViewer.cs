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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace Genetibase.Debug.ObjViewer
{
	public class ObjViewer : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ListView lvProps;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colValue;
		private System.Windows.Forms.Label lblKind;
		private DevComponents.DotNetBar.PanelEx panelEx1;
		private System.ComponentModel.Container components = null;

		public ObjViewer()
		{
			InitializeComponent();

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjViewer));
            this.lvProps = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colValue = new System.Windows.Forms.ColumnHeader();
            this.lblName = new System.Windows.Forms.Label();
            this.lblKind = new System.Windows.Forms.Label();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvProps
            // 
            this.lvProps.AccessibleDescription = null;
            this.lvProps.AccessibleName = null;
            resources.ApplyResources(this.lvProps, "lvProps");
            this.lvProps.BackgroundImage = null;
            this.lvProps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue});
            this.lvProps.Font = null;
            this.lvProps.FullRowSelect = true;
            this.lvProps.GridLines = true;
            this.lvProps.Name = "lvProps";
            this.lvProps.UseCompatibleStateImageBehavior = false;
            this.lvProps.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            resources.ApplyResources(this.colName, "colName");
            // 
            // colValue
            // 
            resources.ApplyResources(this.colValue, "colValue");
            // 
            // lblName
            // 
            this.lblName.AccessibleDescription = null;
            this.lblName.AccessibleName = null;
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = null;
            this.lblName.Name = "lblName";
            this.lblName.Click += new System.EventHandler(this.lblName_Click);
            // 
            // lblKind
            // 
            this.lblKind.AccessibleDescription = null;
            this.lblKind.AccessibleName = null;
            resources.ApplyResources(this.lblKind, "lblKind");
            this.lblKind.BackColor = System.Drawing.Color.Transparent;
            this.lblKind.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblKind.Name = "lblKind";
            // 
            // panelEx1
            // 
            this.panelEx1.AccessibleDescription = null;
            this.panelEx1.AccessibleName = null;
            resources.ApplyResources(this.panelEx1, "panelEx1");
            this.panelEx1.Controls.Add(this.lblName);
            this.panelEx1.Controls.Add(this.lblKind);
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
            // ObjViewer
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.lvProps);
            this.Controls.Add(this.panelEx1);
            this.Font = null;
            this.Name = "ObjViewer";
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		public void ReadNode(object obj, string title, string subtitle)
		{
			Clear();

			if (obj == null)
			{
				return;
			}

			lblName.Text = subtitle;

			lblKind.Text = title;

			ShowObjectProps(obj);

		}

		void Clear()
		{
			lblName.Text="";
			lblKind.Text = "";
			lvProps.Items.Clear();
		}

		void AddProp(string k, string v)
		{
			lvProps.Items.Add(new ListViewItem(new string[] {k,v}));
		}

		void ShowObjectProps(object o)
		{
			Type type = o.GetType();

			PropertyInfo[] pi = type.GetProperties();

			for(int i=0; i < pi.Length; ++i)
			{
				object ret = null;
				object[] atts;
				try
				{
					atts = pi[i].GetCustomAttributes(typeof(ObjViewerAttribute), true);

					ret = type.InvokeMember(pi[i].Name, BindingFlags.GetProperty, null, o, new object [] {});

					if (atts.Length == 0)
					{
						AddProp(pi[i].Name, (ret==null)?"null":ret.ToString());
					}
					else
					{
						ObjViewerAttribute att = (ObjViewerAttribute)atts[0];
						if(att.Hide == false)
						{
							string sName, sVal;

							if (att.Rename)
							{
								sName = att.Name;
							}
							else
							{
								sName = pi[i].Name;
							}

							sVal = ret.ToString();

							if (att.Hex)
							{
								if (ret is Int16) sVal = "0x"+((Int16)ret).ToString("X");
								if (ret is Int32) sVal = "0x"+((Int32)ret).ToString("X8");
								if (ret is Int64) sVal = "0x"+((Int64)ret).ToString("X");
								if (ret is UInt16) sVal = "0x"+((UInt16)ret).ToString("X");
								if (ret is UInt32) sVal = "0x"+((UInt32)ret).ToString("X8");
								if (ret is UInt64) sVal = "0x"+((UInt64)ret).ToString("X");
							}

							AddProp(sName, sVal);
						}
					}
				}
				catch//(Exception e)
				{
//					ret = e;
				}
			}
		}

        private void lblName_Click(object sender, EventArgs e)
        {

        }
	}
	/// <summary>
	/// This is a custom attribute that can be used to indicate how the ObjViewer should treat a 
	/// property of any class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=true, Inherited=true)]
	public class ObjViewerAttribute : Attribute
	{
		bool _hide;
		bool _rename;
		string _name;
		bool _hex;

		public bool Hide{get{return _hide;} set{_hide = value;}}
		public bool Hex{get{return _hex;} set{_hex = value;}}
		public bool Rename{get{return _rename;} set{_rename = value;}}
		public string Name{get{return _name;} set{_name = value;}}

		public ObjViewerAttribute(string name)
		{
			_hide = false;
			_name = name;
			_rename = true;
		}

		public ObjViewerAttribute(bool show)
		{
			_hide = !show;
			_rename = false;
		}

		public ObjViewerAttribute()
		{
			_hide = false;
			_rename = false;
		}
	}

	/// <summary>
	/// This is a simple yet incredibly useful control that, given an instance of any type, displays
	/// its properties.  If the type is one we control, we can use ObjViewerAttribute to control which
	/// props are displayed and how.  Simple yet brilliant -- it's tha perfect crime!
	/// </summary>

}
