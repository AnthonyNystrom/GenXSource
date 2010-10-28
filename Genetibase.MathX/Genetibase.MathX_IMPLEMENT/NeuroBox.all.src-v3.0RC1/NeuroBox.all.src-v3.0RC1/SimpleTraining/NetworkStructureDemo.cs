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
	public class NetworkStructureDemo: IDemo
	{
		public NetworkStructureDemo()
		{
		}
		public void RunDemo()
		{
			Console.WriteLine("### NETWORK STRUCTURE DEMO ###");

			//Initialize the network manager.
			//This constructor also creates the first
			//network layer (Inputlayer).
			Network network = new Network();

			//You need to initialize (the size of) the
			//input layer in an unbound scenario
			network.InitUnboundInputLayer(3);
			//Add the hidden layer with 4 neurons.
			network.AddLayer(4);
			//Add the output layer with 2 neurons.
			network.AddLayer(2);

			//Instead of calling AutoLinkFeedforward()
			//on this place, in this demo we'll connect
			//the network together by our own!

			Layer input = network.FirstLayer;
			Layer hidden = input.TargetLayer;
			Layer output = network.LastLayer;

			//First we want to connect all neurons
			//of the hidden layer to all neurons
			//of the input layer (that's exactly
			//what the AutoLinkFeedforward would
			//do - but between all layers).
			input.CrossLinkForward();

			//Then we want to achieve a lateral
			//feedback in the hidden layer
			//(AutoLinkFeedforward does NOT do this):
			hidden.CrossLinkLayer();

			//Next we want to connect the first
			//and the second Neuron of the hidden
			//Layer to the first output neuron,
			//and the third and fourth to the 2nd
			//output neuron. Some of the synapses
			//shall start with special weights:
			hidden[0].ConnectToNeuron(output[0]);
			hidden[1].ConnectToNeuron(output[0],0.5);
			hidden[2].ConnectToNeuron(output[1]);
			hidden[3].ConnectToNeuron(output[1],-1.5);

			//That's it. Now we can work with it,
			//just we did on the Basic Unbound Demo:
			network.PushUnboundInput(new bool[] {false,true,false});
			network.PushUnboundTraining(new bool[] {false,true});
			network.CalculateFeedforward();
			App.PrintArray(network.CollectOutput());
			network.TrainCurrentPattern(false,true);
			App.PrintArray(network.CollectOutput());
			network.TrainCurrentPattern(false,true);
			App.PrintArray(network.CollectOutput());

			//This demo may help you e.g. building your own
			//INetworkStructureFactory implementations
			//for the grid pattern matching building block.
			//(You may also want to check out the default
			//implementation!)

			Console.WriteLine("=== COMPLETE ===");
			Console.WriteLine();
		}
	}
}
