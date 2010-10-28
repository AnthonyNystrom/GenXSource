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
using NeuroBox.PatternMatching;
using NeuroBox.PatternMatching.Grid2D;

namespace NeuroBox.PatternMatchingDemo
{
	public class CheckboxGrid : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.Container components = null;
		private VisualBit[] grid;
		private Grid2DControler pm;
		private int x, y;
		private bool eventLock = false;

		public CheckboxGrid()
		{
			InitializeComponent();
		}

		public void Init(Grid2DControler pm)
		{
			this.pm = pm;
			this.x = pm.PatternWidth;
			this.y = pm.PatternHeight;
			pm.PatternSelectionChanged += pm_OnPatternSelectionChanged;
			grid = new VisualBit[x*y];
			for(int i=0;i<y;i++)
				for(int j=0;j<x;j++)
				{
					VisualBit c = new VisualBit();
					c.Width = 12;
					c.Height = 12;
					c.Location = new Point(j*12,i*12);
					c.CheckedChanged += CheckBoxChange;
					grid[i*x+j] = c;
					Controls.Add(c);
				}
		}

		private void pm_OnPatternSelectionChanged(object sender, PatternPositionEventArgs e)
		{
			eventLock = true;
			BooleanPattern p = e.Pattern as BooleanPattern;
			if(p == null)
				p = (e.Pattern as DoublePattern).ToBooleanPattern(pm.BasicConfiguration);
			for(int i=0;i<p.InputPattern.Length && i<grid.Length;i++)
				grid[i].IsChecked = p.InputPattern[i];
			eventLock = false;
		}

		private void CheckBoxChange(object sender, EventArgs e)
		{
			if(!eventLock && OnGridChanged != null)
				OnGridChanged(sender,e);
		}

		public event EventHandler OnGridChanged;

		public int CountX
		{
			get {return x;}
		}

		public int CountY
		{
			get {return y;}
		}

		public bool this[int x, int y]
		{
			get {return grid[y*this.x+x].IsChecked;}
			set {grid[y*this.x+x].IsChecked = value;}
		}

		public void BulkSet(bool[] status)
		{
			eventLock = true;
			for(int i=0;i<status.Length && i<grid.Length;i++)
				grid[i].IsChecked = status[i];
			eventLock = false;
			if(OnGridChanged != null)
				OnGridChanged(this,EventArgs.Empty);
		}

		public bool[] BulkGet()
		{
			bool[] data = new bool[grid.Length];
			for(int i=0;i<grid.Length;i++)
				data[i] = grid[i].IsChecked;
			return data;
		}

		public double[] BuklGetAsDoubles(double high, double low)
		{
			double[] data = new double[grid.Length];
			for(int i=0;i<grid.Length;i++)
				data[i] = grid[i].IsChecked ? high : low;
			return data;
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

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// CheckboxGrid
			// 
			this.AutoScroll = true;
			this.Name = "CheckboxGrid";
			this.Size = new System.Drawing.Size(40, 40);

		}
		#endregion
	}
}
