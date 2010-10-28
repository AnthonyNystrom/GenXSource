#region Copyright 2003 Christoph Daniel Rüegg [Modified BSD License]
/*
A Simple Training Demonstration using NeuroBox Neural Network Library
Copyright (c) 2003, Christoph Daniel Rueegg, Switzerland
http://cdrnet.net/. All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer. 

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;

using NeuroBox;
using NeuroBox.PatternMatching;
using NeuroBox.PatternMatching.Grid2D;

namespace SimpleTraining
{
	public class BuildingBlockDemo: IDemo
	{
		public BuildingBlockDemo()
		{
		}
		public void RunDemo()
		{
			Console.WriteLine("### BUILDING BLOCK DEMO ###");

			//Initialize the pattern matching building block
			//for a 2x2 grid and 2 output neurons.
			Grid2DControler gpm = new Grid2DControler(2,2,2);
			gpm.BuildNetwork(true);

			//Add 4 patterns to the pattern manager.
			//note that the input array is in row by row order,
			//and the output numers the (single!) output neuron to train for.
			gpm.Patterns.Add("first",0,new bool[] {true,false,true,false});
            gpm.Patterns.Add("second", 1, new bool[] { true, true, false, false });
            gpm.Patterns.Add("third", 1, new bool[] { false, true, false, false });
            gpm.Patterns.Add("fourth", 0, new bool[] { false, true, true, false });

			//Some configuration ...
			gpm.TrainingConfiguration.AutoTrainingEpochs.Value = 500;

			//Calculate all patterns. The CalculateCurrentNetwork method
			//returns the output neuron index that matches best.
			//First select a pattern, then calculate it:
			Console.WriteLine("# Initial Status");
			for(int i=0;i<gpm.PatternCount;i++)
			{
				gpm.SelectPattern(i);
				Console.Write(gpm.CalculateCurrentNetwork() + ": ");
				App.PrintArray(gpm.NeuralNetwork.CollectOutput());
			}
			Console.WriteLine("Successful Patterns: " + gpm.CountSuccessfulPatterns());

			//Train the third pattern one time:
			Console.WriteLine("# Third Pattern Training");
			gpm.SelectPattern(2);
			gpm.TrainCurrentNetwork();
			for(int i=0;i<gpm.PatternCount;i++)
			{
				gpm.SelectPattern(i);
				Console.Write(gpm.CalculateCurrentNetwork() + ": ");
				App.PrintArray(gpm.NeuralNetwork.CollectOutput());
			}
			Console.WriteLine("Successful Patterns: " + gpm.CountSuccessfulPatterns());

			//Autotrain all Patterns:
			Console.WriteLine("# Auto Training:");
			Console.WriteLine(gpm.AutoTrainNetwork() ? "successful" : "failed");
			for(int i=0;i<gpm.PatternCount;i++)
			{
				gpm.SelectPattern(i);
				Console.Write(gpm.CalculateCurrentNetwork() + ": ");
				App.PrintArray(gpm.NeuralNetwork.CollectOutput());
			}
			Console.WriteLine("Successful Patterns: " + gpm.CountSuccessfulPatterns());

			Console.WriteLine("=== COMPLETE ===");
			Console.WriteLine();
		}
	}
}
