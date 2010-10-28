#region Copyright 2006 Christoph Rüegg [GNU Public License]
/*
A Logic Multi Operator Training Demonstration using NeuroBox Neural Network Library
Copyright (c) 2006, Christoph Daniel Rueegg, http://cdrnet.net/.
All rights reserved.

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

namespace NeuroBox.Demo.LogicMultiTraining
{
	public class Backend
	{
        Network network;
        ConfigNode node;
        BasicConfig config;

		double[] input = new double [2]; //Represents the current input vektor
		double[] currentOutput = new double [16]; //Represents the current output vektor

		double inputT = 5, inputF = -5;
		bool groupedHiddenLayer = false;

		double[] outTT = new double[16]; // X=True  Y=True
		double[] outTF = new double[16]; // X=True  Y=False
		double[] outFT = new double[16]; // X=False Y=True
		double[] outFF = new double[16]; // X=False Y=False
		string[] name = new string[16];

		public Backend()
		{
			//Prepare the default configuration set.
            node = new ConfigNode();
            config = new BasicConfig(node);
			config.BiasNeuronEnable.Value = true;
			config.WeightDecayEnable.Value = true;
			config.FlatspotEliminationEnable.Value = true;
			config.BiasNeuronOutput.Value = 0.9d;
			config.LearningRate.Value = 0.3;
			config.WeightDecay.Value = 0.005;
			InitOutputs(1d,-1d);
		}

		private void InitOutputs(double t, double f)
		{
			outFF[0] =f; outFT[0] =f; outTF[0] =f; outTT[0] =t; name[0] = "AND";
			outFF[1] =t; outFT[1] =t; outTF[1] =t; outTT[1] =f; name[1] = "NAND";
			outFF[2] =f; outFT[2] =t; outTF[2] =t; outTT[2] =t; name[2] = "OR";
			outFF[3] =t; outFT[3] =f; outTF[3] =f; outTT[3] =f; name[3] = "NOR";
			outFF[4] =f; outFT[4] =t; outTF[4] =t; outTT[4] =f; name[4] = "XOR (antivalence)";
			outFF[5] =t; outFT[5] =f; outTF[5] =f; outTT[5] =t; name[5] = "XNOR (equivalence)";
			outFF[6] =t; outFT[6] =t; outTF[6] =t; outTT[6] =t; name[6] = "TRUE";
			outFF[7] =f; outFT[7] =f; outTF[7] =f; outTT[7] =f; name[7] = "FALSE";
			outFF[8] =f; outFT[8] =f; outTF[8] =t; outTT[8] =t; name[8] = "X";
			outFF[9] =f; outFT[9] =t; outTF[9] =f; outTT[9] =t; name[9] = "Y";
			outFF[10]=t; outFT[10]=t; outTF[10]=f; outTT[10]=f; name[10] = "NOT X";
			outFF[11]=t; outFT[11]=f; outTF[11]=t; outTT[11]=f; name[11] = "NOT Y";
			outFF[12]=f; outFT[12]=t; outTF[12]=f; outTT[12]=f; name[12] = "Inhibition X";
			outFF[13]=f; outFT[13]=f; outTF[13]=t; outTT[13]=f; name[13] = "Inhibition Y";
			outFF[14]=t; outFT[14]=t; outTF[14]=f; outTT[14]=t; name[14] = "Implication X";
			outFF[15]=t; outFT[15]=f; outTF[15]=t; outTT[15]=t; name[15] = "Implication Y";
		}

		public void BuildNetwork()
		{
			network = new Network(node);

			if(!groupedHiddenLayer)
			{
				network.AddLayer(32); //Hidden layer with 32 neurons
				network.AddLayer(16); //Output layer with 16 neuron

				network.BindInputLayer(input); //Bind Input Data
			
				network.PushUnboundTraining(outFF);
				network.AutoLinkFeedforward(); //Create synapses between the layers for typical feedforward networks.
			}
			else
			{
				network.AddLayer(64); //Hidden layer with 64 neurons
				network.AddLayer(16); //Output layer with 16 neuron

				network.BindInputLayer(input); //Bind Input Data
			
				network.PushUnboundTraining(outFF);

				network.FirstLayer.CrossLinkForward();
				Layer hidden = network.FirstLayer.TargetLayer;
				Layer output = network.LastLayer;
				for(int i=0;i<output.Count;i++)
				{
					hidden[i*4].ConnectToNeuron(output[i]);
					hidden[i*4+1].ConnectToNeuron(output[i]);
					hidden[i*4+2].ConnectToNeuron(output[i]);
					hidden[i*4+3].ConnectToNeuron(output[i]);
				}
			}
		}

		/// <summary>
		/// Train all 4 patterns one time.
		/// </summary>
		public void TrainEpoche()
		{
			input[0] = inputF; //Update the bound network input
			input[1] = inputF;
			network.PushUnboundTraining(outFF);
			network.TrainCurrentPattern(true,false); 

			input[0] = inputF;
			input[1] = inputT;
			network.PushUnboundTraining(outFT);
			network.TrainCurrentPattern(true,false);

			input[0] = inputT;
			input[1] = inputF;
			network.PushUnboundTraining(outTF);
			network.TrainCurrentPattern(true,false);

			input[0] = inputT;
			input[1] = inputT;
			network.PushUnboundTraining(outTT);
			network.TrainCurrentPattern(true,false);
		}

		/// <summary>
		/// Collect the propagated output for all 4 patterns.
		/// </summary>
		public double[] EvaluateOutputs()
		{
			double[] output = new double[4*16];

			input[0] = inputF;
			input[1] = inputF;
			network.CalculateFeedforward();
			network.CollectOutput().CopyTo(output,0);

			input[0] = inputF;
			input[1] = inputT;
			network.CalculateFeedforward();
			network.CollectOutput().CopyTo(output,16);

			input[0] = inputT;
			input[1] = inputF;
			network.CalculateFeedforward();
			network.CollectOutput().CopyTo(output,32);

			input[0] = inputT;
			input[1] = inputT;
			network.CalculateFeedforward();
			network.CollectOutput().CopyTo(output,48);

			return output;
		}

		public string[] PatternNames()
		{
			return name;
		}

		public double GetOutputFF(int index)
		{
			return outFF[index];
		}
		public double GetOutputFT(int index)
		{
			return outFT[index];
		}
		public double GetOutputTF(int index)
		{
			return outTF[index];
		}
		public double GetOutputTT(int index)
		{
			return outTT[index];
		}

		public bool BiasNeuron
		{
			get {return config.BiasNeuronEnable.Value;}
			set {config.BiasNeuronEnable.Value = value;}
		}

		public Network Network
		{
			get {return network;}
		}

		public int HiddenNeuronCount
		{
			get {return network.FirstLayer.TargetLayer.Count;}
		}

		public bool GroupedHiddenLayer
		{
			get {return groupedHiddenLayer;}
			set {groupedHiddenLayer = value;}
		}
	}
}
