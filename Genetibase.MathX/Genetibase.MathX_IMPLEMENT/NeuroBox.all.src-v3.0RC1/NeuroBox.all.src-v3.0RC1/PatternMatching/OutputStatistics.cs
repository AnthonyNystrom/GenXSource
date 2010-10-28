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
	public class OutputStatistics : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.Container components = null;

		//private Layer layer;
		private ArrayList items;

		public OutputStatistics()
		{
			InitializeComponent();
		}

		public void Init(Grid2DControler pm)
		{
			Layer layer = pm.NeuralNetwork.LastLayer;
			layer.NeuronAdded += layer_OnNeuronAdded;
			layer.NeuronRemoved += layer_OnNeuronRemoved;
			pm.PatternSelectionChanged += pm_OnPatternSelectionChanged;
			pm.NetworkCalculated += pm_OnNetworkCalculated;

			if(items != null)
				for(int i=0;i<items.Count;i++)
					if(this[i] != null)
						Controls.Remove(this[i]);

			items = new ArrayList(layer.Count + 15);
			for(int i=0;i<layer.Count;i++)
				AddOutputNeuron(layer[i]);
		}

		protected void AddOutputNeuron(Neuron neuron)
		{
			SingleOutput so = new SingleOutput(neuron);
			so.Location = new Point(0,17*items.Count);
			items.Add(so);
			Controls.Add(so);
		}

		protected void RemoveOutputNeuron(Neuron neuron)
		{
		}

		protected void UpdateShouldBeBestNeuron(int position)
		{
			for(int i=0;i<items.Count;i++)
				this[i].ResetShouldBeBest();
			this[position].ShouldBeBest();
			for(int i=0;i<items.Count;i++)
				this[i].UpdateData();
		}

		protected void UpdateBestNeuron(int position)
		{
			for(int i=0;i<items.Count;i++)
				this[i].ResetIsBest();
			this[position].IsBest();
			for(int i=0;i<items.Count;i++)
				this[i].UpdateData();
		}

		public SingleOutput this[int index]
		{
			get {return (SingleOutput)items[index];}
		}

		public int Count
		{
			get {return items.Count;}
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
		private void InitializeComponent()
		{
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(250, 150);
			this.Name = "OutputStatistics";
			this.Size = new System.Drawing.Size(275, 128);
		}
		#endregion

		private void pm_OnPatternSelectionChanged(object sender, PatternPositionEventArgs e)
		{
			UpdateShouldBeBestNeuron(e.Pattern.Classification);
		}

		private void pm_OnNetworkCalculated(object sender, PositionEventArgs e)
		{
			UpdateBestNeuron(e.Position);
		}

		private void layer_OnNeuronAdded(object sender, NeuronEventArgs e)
		{
			AddOutputNeuron(e.Neuron);
		}

		private void layer_OnNeuronRemoved(object sender, NeuronEventArgs e)
		{
			RemoveOutputNeuron(e.Neuron);
		}
	}
}
