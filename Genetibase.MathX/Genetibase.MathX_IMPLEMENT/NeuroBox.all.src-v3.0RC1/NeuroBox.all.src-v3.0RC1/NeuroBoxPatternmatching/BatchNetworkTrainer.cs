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
using System.Collections;
using NeuroBox;

namespace NeuroBox.PatternMatching
{
	/*
	/// <summary>
	/// Default simple network trainer.
	/// </summary>
	public class BatchNetworkTrainer: INetworkTrainer
	{
		/// <summary>The configuration instance.</summary>
		protected GridConfiguration config;
		/// <summary>The neural network.</summary>
		protected Network network;
		/// <summary>Delegate to count successful trained patterns.</summary>
		protected Count CountSuccess;
		/// <summary>Delegate to count available patterns.</summary>
		protected Count CountPatterns;
		/// <summary>Delegate to select a pattern.</summary>
		protected Select SelectPattern;
		/// <summary>Delegate to calculate a pattern and return the position of the best output neuron.</summary>
		protected Position CalculateAndReturnBest;
		/// <summary>Delegate to update the progress.</summary>
		protected Select SelectProgress;

		/// <summary>
		/// Instanciate a new network trainer.
		/// </summary>
		/// <param name="config">The configuration instance.</param>
		/// <param name="network">The neural network.</param>
		/// <param name="countSuccess">Delegate to count successful trained patterns.</param>
		/// <param name="countPatterns">Delegate to count available patterns.</param>
		/// <param name="selectPattern">Delegate to select a pattern.</param>
		/// <param name="calculateAndReturnBest">Delegate to calculate a pattern and return the position of the best output neuron.</param>
		/// <param name="selectProgress">Delegate to update the progress.</param>
		public BatchNetworkTrainer(GridConfiguration config, Network network,
			Count countSuccess, Count countPatterns, Select selectPattern,
			Position calculateAndReturnBest, Select selectProgress)
		{
			this.config = config;
			this.network = network;
			this.CountSuccess = countSuccess;
			this.CountPatterns = countPatterns;
			this.SelectPattern = selectPattern;
			this.SelectProgress = selectProgress;
			this.CalculateAndReturnBest = calculateAndReturnBest;
		}

		/// <summary>
		/// Refresh the neural network instance.
		/// </summary>
		/// <param name="network">The new network.</param>
		public void RefreshNetwork(Network network)
		{
			this.network = network;
		}

		/// <summary>
		/// Train the network using specialized strategies.
		/// </summary>
		/// <returns>Whether the training was successful or not.</returns>
		public bool Train()
		{
			int patternCount = CountPatterns();
			int howmany = CountSuccess();
			if(howmany >= config.AutoTrainingSuccess)
				return true;
			int timeout = config.AutoTrainingEpochs;
			while(timeout-- >= 0)
			{
				if(SelectProgress != null)
					SelectProgress((int)(100*(config.AutoTrainingEpochs-(double)timeout)/config.AutoTrainingEpochs));
				for(int i=0;i<patternCount;i++)
				{
					SelectPattern(i);
					network.BatchAnalyzeCurrentPattern();
				}
				network.BatchOptimizePatterns();
				howmany = CountSuccess();
				if(howmany >= config.AutoTrainingSuccess)
					return true;
			}
			//clean up:
			CalculateAndReturnBest();
			return false;
		}
	}
	*/
}
