#region Copyright 2001-2006 Christoph Daniel Rüegg [GNU Public License]
/*
NeuroBox, a library for neural network generation, propagation and training
Copyright (c) 2001-2006, Christoph Daniel Rueegg, http://cdrnet.net/. All rights reserved.

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

namespace NeuroBox
{
	/// <summary>
	/// Event Arguments for events for double value updates.
	/// </summary>
	public class ValueChangedEventArgs: EventArgs
	{
		double newValue, oldValue;
		/// <summary>
		/// Instanciate new event arguments.
		/// </summary>
		/// <param name="newValue">The new double value.</param>
		/// <param name="oldValue">The old double value.</param>
		public ValueChangedEventArgs(double newValue, double oldValue)
		{
			this.newValue = newValue;
			this.oldValue = oldValue;
		}
		/// <summary>
		/// The new double value.
		/// </summary>
		public double NewValue
		{
			get {return newValue;}
		}
		/// <summary>
		/// The old double value.
		/// </summary>
		public double OldValue
		{
			get {return oldValue;}
		}
	}

	/// <summary>
	/// Event Arguments for events refering to a synapse.
	/// </summary>
	public class SynapseEventArgs: EventArgs
	{
		Synapse synapse;
		/// <summary>
		/// Instanciate new event arguments.
		/// </summary>
		/// <param name="synapse">The refering synapse.</param>
		public SynapseEventArgs(Synapse synapse)
		{
			this.synapse = synapse;
		}
		/// <summary>
		/// The refering neuron.
		/// </summary>
		public Synapse Synapse
		{
			get {return synapse;}
		}
	}

	/// <summary>
	/// Event Arguments for events refering to a neuron.
	/// </summary>
	public class NeuronEventArgs: EventArgs
	{
		Neuron neuron;
		/// <summary>
		/// Instanciate new event arguments.
		/// </summary>
		/// <param name="neuron">The refering neuron.</param>
		public NeuronEventArgs(Neuron neuron)
		{
			this.neuron = neuron;
		}
		/// <summary>
		/// The refering neuron.
		/// </summary>
		public Neuron Neuron
		{
			get {return neuron;}
		}
	}

	/// <summary>
	/// Event Arguments for events refering to a layer.
	/// </summary>
	public class LayerEventArgs: EventArgs
	{
		Layer layer;
		/// <summary>
		/// Instanciate new event arguments.
		/// </summary>
		/// <param name="layer">The refering layer.</param>
		public LayerEventArgs(Layer layer)
		{
			this.layer = layer;
		}
		/// <summary>
		/// The refering neuron.
		/// </summary>
		public Layer Layer
		{
			get {return layer;}
		}
	}

	/// <summary>
	/// Event Arguments for events refering to a position.
	/// </summary>
	public class PositionEventArgs: EventArgs
	{
		int position;
		/// <summary>
		/// Instanciate new event arguments.
		/// </summary>
		/// <param name="position">The refering position.</param>
		public PositionEventArgs(int position)
		{
			this.position = position;
		}
		/// <summary>
		/// The refering position.
		/// </summary>
		public int Position
		{
			get {return position;}
		}
	}

    public class ConfigChangedEventArgs : EventArgs
    {
        string id;
        bool all;

        public ConfigChangedEventArgs(string id, bool all)
        {
            this.id = id;
            this.all = all;
        }

        public string Id
        {
            get { return id; }
        }

        public bool All
        {
            get { return all; }
        }
    }
}