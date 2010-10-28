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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace NeuroBox.PatternMatchingDemo
{
	public class VisualBit : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panelOff;
		private System.Windows.Forms.Panel panelOn;
		private System.ComponentModel.Container components = null;

		public event EventHandler CheckedChanged;

		public VisualBit()
		{
			InitializeComponent();
		}

		public bool IsChecked
		{
			get {return panelOn.Visible;}
			set
			{
				if(panelOn.Visible != value)
				{
					if(value)
					{
						panelOn.Visible = true;
						panelOff.Visible = false;
					}
					else
					{
						panelOff.Visible = true;
						panelOn.Visible = false;
					}
					if(CheckedChanged != null)
						CheckedChanged(this,EventArgs.Empty);
				}
			}
		}

		public void Switch()
		{
			if(panelOn.Visible)
			{
				panelOff.Visible = true;
				panelOn.Visible = false;
			}
			else
			{
				panelOn.Visible = true;
				panelOff.Visible = false;
			}
			if(CheckedChanged != null)
				CheckedChanged(this,EventArgs.Empty);
		}

		/// <summary> 
		/// Die verwendeten Ressourcen bereinigen.
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

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelOff = new System.Windows.Forms.Panel();
			this.panelOn = new System.Windows.Forms.Panel();
			this.panelOff.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelOff
			// 
			this.panelOff.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.panelOff.Location = new System.Drawing.Point(0, 0);
			this.panelOff.Name = "panelOff";
			this.panelOff.Size = new System.Drawing.Size(12, 12);
			this.panelOff.Click += new System.EventHandler(this.panelOff_Click);
			// 
			// panelOn
			// 
			this.panelOn.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.panelOn.Location = new System.Drawing.Point(0, 0);
			this.panelOn.Name = "panelOn";
			this.panelOn.Size = new System.Drawing.Size(12, 12);
			this.panelOn.Visible = false;
			this.panelOn.Click += new System.EventHandler(this.panelOn_Click);
			// 
			// VisualBit
			// 
			this.Controls.Add(this.panelOff);
			this.Controls.Add(this.panelOn);
			this.Name = "VisualBit";
			this.Size = new System.Drawing.Size(12, 12);
			this.panelOff.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void panelOn_Click(object sender, System.EventArgs e)
		{
			Switch();
		}

		private void panelOff_Click(object sender, System.EventArgs e)
		{
			Switch();
		}
	}
}
