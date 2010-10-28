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

namespace SimpleTraining
{
	public class BasicBoundDemo: IDemo
	{
		public BasicBoundDemo()
		{
		}
		public void RunDemo()
		{
			Console.WriteLine("### BASIC BOUND DEMO ###");

			//Prepare you're input and training data
			//to bind to the network
			double[] input = new double[] {-5d,5d,-5d};
			double[] training = new double[] {-1,1};

			//Initialize the network manager.
			//This constructor also creates the first
			//network layer (Inputlayer).
			Network network = new Network();

			//Bind your input array (to the already
			//existing input layer)
			network.BindInputLayer(input);
			//Add the hidden layer with 4 neurons.
			network.AddLayer(4);
			//Add the output layer with 2 neurons.
			network.AddLayer(2);
			//bind your training array to the output layer.
			//Always do this AFTER creating the layers.
			network.BindTraining(training);

			//Connect the neurons together using synapses.
			//This is the easiest way to do it; I'll discuss
			//other ways in more detail in another demo.
			network.AutoLinkFeedforward();

			//Propagate the network using the bound input data.
			//Internally, this is a two round process, to
			//correctly handle feedbacks
            network.CalculateFeedforward();
			//Collect the network output and print it.
			App.PrintArray(network.CollectOutput());

			//Train the current pattern using Backpropagation (one step)!
			network.TrainCurrentPattern(false,true);
			//Print the output; the difference to (-1,1) should be
			//smaller this time!
			App.PrintArray(network.CollectOutput());

			//Same one more time:
			network.TrainCurrentPattern(false,true);
			App.PrintArray(network.CollectOutput());

			//Train another pattern:
			Console.WriteLine("# new pattern:");
			input[0] = 5d;
			input[1] = -5d;
			training[0] = 1;
			//calculate ...
			network.CalculateFeedforward();
			App.PrintArray(network.CollectOutput());
			//... and train it one time
			network.TrainCurrentPattern(false,true);
			App.PrintArray(network.CollectOutput());

			//what about the old pattern now?
			Console.WriteLine("# the old pattern again:");
			input[0] = -5d;
			input[1] = 5d;
			training[0] = -1;
			network.CalculateFeedforward();
			App.PrintArray(network.CollectOutput());

			Console.WriteLine("=== COMPLETE ===");
			Console.WriteLine();
		}
	}
}
