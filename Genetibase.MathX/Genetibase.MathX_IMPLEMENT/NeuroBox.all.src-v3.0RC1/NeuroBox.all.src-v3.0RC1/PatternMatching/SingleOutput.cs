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

using NeuroBox;

namespace NeuroBox.PatternMatchingDemo
{
	public class SingleOutput : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel panelRed;
		private System.Windows.Forms.Panel panelGreen;
		private System.Windows.Forms.Label lblValue;

		private Neuron neuron;
		private bool isBest = false;
		private bool shallBeBest = false;

		public SingleOutput(Neuron neuron)
		{
			InitializeComponent();
			this.neuron = neuron;
			UpdateData();
			//neuron.OnOutputChanged += new ValueChangedEventHandler(OnNeuronOutputChanged);
		}

		public void UpdateData()
		{
			if(isBest)
				lblValue.ForeColor = Color.Red;
			else
				lblValue.ForeColor = Color.Black;
			lblTitle.Text = neuron.Title;
			lblValue.Text = Math.Round(neuron.CurrentActivity,3).ToString();
			int width = (int)(50 * neuron.CurrentOutput);
			if(neuron.CurrentOutput > 0)
			{
				panelGreen.Visible = true;
				panelRed.Visible = false;
				panelGreen.Width = width;
			}
			else
			{
				panelRed.Visible = true;
				panelGreen.Visible = false;
				panelRed.Width = -width;
				panelRed.Left = 50 + width;
			}
			lblTitle.BackColor = Color.FromKnownColor(KnownColor.Control);
			if(isBest && shallBeBest)
				lblTitle.BackColor = panelGreen.BackColor;
			else if(isBest)
				lblTitle.BackColor = panelRed.BackColor;
			else if(shallBeBest)
				lblTitle.BackColor = Color.Orange;
		}

//		public void UpdateBar()
//		{
//			lblTitle.Text = neuron.Title;
//			lblValue.Text = Math.Round(neuron.CurrentOutput,2).ToString();
//			int width = (int)(50 * neuron.CurrentOutput);
//			if(neuron.CurrentOutput > 0)
//			{
//				panelGreen.Visible = true;
//				panelRed.Visible = false;
//				panelGreen.Width = width;
//			}
//			else
//			{
//				panelRed.Visible = true;
//				panelGreen.Visible = false;
//				panelRed.Width = -width;
//				panelRed.Left = 50 + width;
//			}
//		}

		public double CurrentOutput
		{
			get {return neuron.CurrentOutput;}
		}

		public void ShouldBeBest()
		{
			shallBeBest = true;
		}
		public void IsBest()
		{
			isBest = true;
		}
		public void ResetShouldBeBest()
		{
			shallBeBest = false;
		}
		public void ResetIsBest()
		{
			isBest = false;
		}

//		private void OnNeuronOutputChanged(object sender, ValueChangedEventArgs e)
//		{
//			UpdateBar();
//		}

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
		private void InitializeComponent()
		{
			this.lblTitle = new System.Windows.Forms.Label();
			this.panelRed = new System.Windows.Forms.Panel();
			this.panelGreen = new System.Windows.Forms.Panel();
			this.lblValue = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(152, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(96, 16);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "[Title]";
			// 
			// panelRed
			// 
			this.panelRed.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.panelRed.Location = new System.Drawing.Point(0, 0);
			this.panelRed.Name = "panelRed";
			this.panelRed.Size = new System.Drawing.Size(50, 16);
			this.panelRed.TabIndex = 1;
			// 
			// panelGreen
			// 
			this.panelGreen.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.panelGreen.Location = new System.Drawing.Point(50, 0);
			this.panelGreen.Name = "panelGreen";
			this.panelGreen.Size = new System.Drawing.Size(50, 16);
			this.panelGreen.TabIndex = 2;
			// 
			// lblValue
			// 
			this.lblValue.ForeColor = System.Drawing.Color.Black;
			this.lblValue.Location = new System.Drawing.Point(100, 0);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(52, 16);
			this.lblValue.TabIndex = 3;
			this.lblValue.Text = "[#]";
			// 
			// SingleOutput
			// 
			this.Controls.Add(this.lblValue);
			this.Controls.Add(this.panelGreen);
			this.Controls.Add(this.panelRed);
			this.Controls.Add(this.lblTitle);
			this.Name = "SingleOutput";
			this.Size = new System.Drawing.Size(250, 16);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
