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
	/// Incremental Openloop Network Trainer, adjusting behavior by time.
	/// </summary>
	public class OpenloopTrainer: INetworkTrainer
	{
		/// <summary>
		/// Instanciate a new network trainer.
		/// </summary>
        public OpenloopTrainer() { }

		/// <summary>
		/// Train the network using specialized strategies.
		/// </summary>
		/// <returns>Whether the training was successful.</returns>
		public bool Train(IControler controler, Progress progress)
		{
            TrainingConfig config = controler.TrainingConfiguration;
            BasicConfig basic = config.Basic;

			int patternCount = controler.PatternCount;
            int epochs = config.AutoTrainingEpochs.Value;
            int success = (int)(patternCount * config.AutoTrainingPercentSuccessful.Value);
			int howmany = controler.CountSuccessfulPatterns();

            double LRinitial = basic.LearningRate.Value;
            double LRstep = LRinitial * 0.7 / epochs;
            double LRcurrent = LRinitial;

			if(howmany >= success)
				return true;

            int timeout = epochs;
			while(timeout-- >= 0)
            {
                basic.LearningRate.Value = LRcurrent;

				if(progress != null)
                    progress((int)(100 * (epochs - (double)timeout) / epochs));

				for(int i=0;i<patternCount;i++)
				{
					controler.SelectShuffledPattern(i);
                    controler.NeuralNetwork.TrainCurrentPattern(true, false);
				}

                howmany = controler.CountSuccessfulPatterns();
                if(howmany >= success)
                {
                    basic.LearningRate.Value = LRinitial;
                    return true;
                }

                LRcurrent -= LRstep;
			}

            // CLEAN UP:
            //controler.CalculateCurrentNetwork();
            basic.LearningRate.Value = LRinitial;
			return false;
		}
	}
}
