#region Copyright 2001-2006 Christoph Daniel Rüegg, Tobias Finazzi [GNU Public License]
/*
A Pattern Matching Demonstration using NeuroBox Neural Network Library
Copyright (c) 2001-2006, Christoph Daniel Rueegg, Tobias Finazzi.
http://cdrnet.net/. All rights reserved.

Using Netron Library, Copyright Francois Vanderseypen, Lutz Roeder
http://netron.sourceforge.net

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics; 
using System.Data;

namespace NeuroBox.PatternMatchingDemo
{
	public class AboutInfo : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.DataGrid dataGrid;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;

		private System.ComponentModel.Container components = null;

		public AboutInfo()
		{
			InitializeComponent();

			DataTable data = new DataTable("assemblies");
			DataColumn col = data.Columns.Add("Name",typeof(string));
			data.PrimaryKey = new DataColumn[] {col};
			data.Columns.Add("Version",typeof(string));
			data.Columns.Add("Product",typeof(string));
			data.Columns.Add("Copyright",typeof(string));
			data.Columns.Add("Path",typeof(string));
			data.Columns.Add("Title",typeof(string));
			data.Columns.Add("Description",typeof(string));
			data.Columns.Add("CLR Version",typeof(string));
			dataGrid.PreferredColumnWidth = 150;
			dataGrid.DataSource = data;

			Assembly assembly = Assembly.GetExecutingAssembly();
			SortedList ht = new SortedList(31);
			RecursiveGetReferences(ht,assembly.GetName(),0);
			foreach(DictionaryEntry entry in ht)
			{
				AssemblyInfo ai = (AssemblyInfo)entry.Value;
				DataRow row = data.NewRow();
				row[0] = ai.Name.Name + " (" + ai.level + ")";
				row[1] = ai.Name.Version.ToString();
				row[2] = ai.Info.ProductName;
				row[3] = ai.Info.LegalCopyright;
				row[4] = ai.Assembly.Location;
				row[5] = ai.Info.Comments;
				row[6] = ai.Info.FileDescription;
				row[7] = ai.Assembly.ImageRuntimeVersion;
				data.Rows.Add(row);
			}

		}

		private void RecursiveGetReferences(SortedList table, AssemblyName name, int level)
		{
			if(table.ContainsKey(name.Name))
			{
				AssemblyInfo t = (AssemblyInfo)table[name.Name];
				if(t.level > level)
				{
					t.level = level;
					table[name.Name] = t;
				}
				return;
			}
			AssemblyInfo ai = new AssemblyInfo();
			ai.Assembly = Assembly.Load(name);
			ai.level = level;
			ai.Name = name;
			ai.Info = FileVersionInfo.GetVersionInfo(ai.Assembly.Location);
			table.Add(name.Name,ai);
			AssemblyName[] names = ai.Assembly.GetReferencedAssemblies();
			for(int i=0;i<names.Length;i++)
				RecursiveGetReferences(table,names[i],level+1);
		}

		private struct AssemblyInfo
		{
			public int level;
			public Assembly Assembly;
			public AssemblyName Name;
			public FileVersionInfo Info;
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

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnClose = new System.Windows.Forms.Button();
			this.lblTitle = new System.Windows.Forms.Label();
			this.dataGrid = new System.Windows.Forms.DataGrid();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnClose.Location = new System.Drawing.Point(502, 382);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(80, 24);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(8, 8);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(574, 24);
			this.lblTitle.TabIndex = 2;
			this.lblTitle.Text = "NeuroBox Pattern Matching Demo";
			// 
			// dataGrid
			// 
			this.dataGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
			this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGrid.BackColor = System.Drawing.Color.Silver;
			this.dataGrid.BackgroundColor = System.Drawing.Color.White;
			this.dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dataGrid.CaptionVisible = false;
			this.dataGrid.DataMember = "";
			this.dataGrid.FlatMode = true;
			this.dataGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid.Location = new System.Drawing.Point(8, 104);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.ReadOnly = true;
			this.dataGrid.Size = new System.Drawing.Size(574, 264);
			this.dataGrid.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Using:";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Copyright:";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(80, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(504, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Christoph Rüegg, Tobias Finazzi";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(80, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(504, 16);
			this.label4.TabIndex = 8;
			this.label4.Text = "Netron: Francois Vanderseypen, based on a prototype of Lutz Roeder";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(80, 64);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(504, 16);
			this.label5.TabIndex = 8;
			this.label5.Text = "NeuroBox: Christoph Rüegg";
			// 
			// AboutInfo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(592, 419);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.dataGrid);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutInfo";
			this.Text = "About";
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}
