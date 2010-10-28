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

using NeuroBox;

namespace NeuroBox.PatternMatchingDemo
{
	public class ConfigurationEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PropertyGrid propGrid;
		private System.ComponentModel.Container components = null;

		public ConfigurationEditor()
		{
			InitializeComponent();
		}

        //public void BindToConfiguration(Configuration config)
        //{
        //    propGrid.SelectedObject = config;
        //}

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
			this.propGrid = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// propGrid
			// 
			this.propGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.propGrid.CommandsVisibleIfAvailable = true;
			this.propGrid.LargeButtons = false;
			this.propGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propGrid.Location = new System.Drawing.Point(0, 0);
			this.propGrid.Name = "propGrid";
			this.propGrid.Size = new System.Drawing.Size(352, 323);
			this.propGrid.TabIndex = 2;
			this.propGrid.Text = "Configuration";
			this.propGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// ConfigurationEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 323);
			this.Controls.Add(this.propGrid);
			this.Name = "ConfigurationEditor";
			this.Text = "Configuration Editor";
			this.Load += new System.EventHandler(this.ConfigurationEditor_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ConfigurationEditor_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
