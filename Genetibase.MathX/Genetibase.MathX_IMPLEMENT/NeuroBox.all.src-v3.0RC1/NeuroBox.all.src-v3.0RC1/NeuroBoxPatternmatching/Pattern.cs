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
using NeuroBox;

namespace NeuroBox.PatternMatching
{
	/// <summary>
	/// A Pattern is a single input-output association.
	/// You may use such patterns to traing a neural network to its association.
	/// </summary>
	[Serializable]
	public abstract class Pattern
	{
		private string _title;
		private int _classification;

		/// <summary>
		/// Instanciates a new (abstract) pattern.
		/// </summary>
		/// <param name="title">A short pattern description</param>
		/// <param name="classification">The favourie output neuron index (in the output layer).</param>
		protected Pattern(string title, int classification)
		{
			_title = title;
			_classification = classification;
		}

		/// <summary>
		/// The name/title of this pattern.
		/// </summary>
		public string Title
		{
			get {return _title;}
		}

		/// <summary>
		/// The position of the favoured output neuron in the output layer.
		/// </summary>
		public int Classification
		{
			get {return _classification;}
		}

		#region Basic Network Sync
		/// <summary>
		/// Copy input pattern data to an array.
		/// </summary>
		/// <param name="vector">The target array.</param>
		/// <param name="config">The configuration instance.</param>
        public abstract void SyncInputTo(double[] vector, BasicConfig config);
		/// <summary>
		/// Copy output training data to an array.
		/// </summary>
		/// <param name="vector">The target array.</param>
		/// <param name="config">The configuration instance.</param>
        public abstract void SyncTrainingTo(double[] vector, BasicConfig config);
		/// <summary>
		/// Copy input pattern data from an array.
		/// </summary>
		/// <param name="vector">The target array.</param>
		/// <param name="config">The configuration instance.</param>
        public abstract void SyncInputFrom(double[] vector, BasicConfig config);
		/// <summary>
		/// Copy output training data from an array.
		/// </summary>
		/// <param name="vector">The target array.</param>
		/// <param name="config">The configuration instance.</param>
        public abstract void SyncTrainingFrom(double[] vector, BasicConfig config);
		#endregion
	}
}
