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
	/// Controler Interface to decouple trainer implementations from the abstract Controler class.
	/// </summary>
	/// <remarks>
	/// Do not use this interface for your own controler abstraction
	/// as it is rather incomplete for such applications.
	/// </remarks>
	public interface IControler
	{
		/// <summary>The current network instance.</summary>
		Network NeuralNetwork {get;}
		/// <summary>The training configuration set.</summary>
        TrainingConfig TrainingConfiguration { get;}
        /// <summary>The basic network configuration set.</summary>
        BasicConfig BasicConfiguration { get;}
		/// <summary>The count of loaded patterns.</summary>
		int PatternCount {get;}
		/// <summary>Select a pattern.</summary>
		/// <param name="position">The index of the pattern.</param>
		void SelectPattern(int position);
		void SelectShuffledPattern(int position);
		/// <summary>Calculate the current network/pattern.</summary>
		/// <returns>The position of the output neuron with the biggest output.</returns>
		int CalculateCurrentNetwork();
        int CalculateCurrentNetwork(out double outputLeadMargin);
		/// <summary>Count matching (successful trained) patterns.</summary>
		/// <returns>The number of matching patterns</returns>
		int CountSuccessfulPatterns();
	}
}
