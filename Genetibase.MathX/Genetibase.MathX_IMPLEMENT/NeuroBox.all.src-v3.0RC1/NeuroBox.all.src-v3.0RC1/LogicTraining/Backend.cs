#region Copyright 2003-2006 Christoph Rüegg, Leopold Rehberger [GNU Public License]
/*
A Logic Operator Training Demonstration using NeuroBox Neural Network Library
Copyright (c) 2003-2006, Leopold Rehberger, Christoph Daniel Rueegg.
http://cdrnet.net/. All rights reserved.

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

namespace NeuroBox.Demo.LogicTraining
{
	public class Backend
	{
		Network _network;
        ConfigNode _node;
        BasicConfig _config;

		double[] _input = new double [2]; //Represents the current input vektor
		double[] _desired = new double [1]; //Represents the current desired (training) output vektor
		double[] _currentOutput = new double [1]; //Represents the current output vektor

		double[] _inputMatrix; //List of all 4 input patterns (X,Y pairs). Length 8.
		double[] _desiredMatrix; //List of all 4 desired output values. Length 4.

		public Backend()
		{
			//Prepare the default configuration set.
            _node = new ConfigNode();
            _config = new BasicConfig(_node);

            _config.BiasNeuronEnable.Value = true;
            _config.LearningRate.Value = 0.3;
		}

		public void BuildNetwork()
		{
			_network = new Network(_node);

			_network.AddLayer(4); //Hidden layer with 2 neurons
			_network.AddLayer(1); //Output layer with 1 neuron

			_network.BindInputLayer(_input); //Bind Input Data
			_network.BindTraining(_desired); //Bind desired output data

			_network.AutoLinkFeedforward(); //Create synapses between the layers for typical feedforward networks.
		}

		/// <summary>
		/// Train all 4 patterns one time.
		/// </summary>
		public void TrainEpoche()
		{
			_input[0] = _inputMatrix[0]; //Update the bound network input
			_input[1] = _inputMatrix[1];
			_desired[0] = _desiredMatrix[0]; //Update the bound network training
			_network.TrainCurrentPattern(true,false); 

			_input[0] = _inputMatrix[2];
			_input[1] = _inputMatrix[3];
			_desired[0] = _desiredMatrix[1];
			_network.TrainCurrentPattern(true,false);

			_input[0] = _inputMatrix[4];
			_input[1] = _inputMatrix[5];
			_desired[0] = _desiredMatrix[2];
			_network.TrainCurrentPattern(true,false);

			_input[0] = _inputMatrix[6];
			_input[1] = _inputMatrix[7];
			_desired[0] = _desiredMatrix[3];
			_network.TrainCurrentPattern(true,false);
		}

		/// <summary>
		/// Collect the propagated output for all 4 patterns.
		/// </summary>
		public double[] EvaluateOutputs()
		{
			double[] output = new double[4];

			_input[0] = _inputMatrix[0];
			_input[1] = _inputMatrix[1];
			_network.CalculateFeedforward();
			output[0] = _network.CollectOutput()[0];

			_input[0] = _inputMatrix[2];
			_input[1] = _inputMatrix[3];
			_network.CalculateFeedforward();
			output[1] = _network.CollectOutput()[0];

			_input[0] = _inputMatrix[4];
			_input[1] = _inputMatrix[5];
			_network.CalculateFeedforward();
			output[2] = _network.CollectOutput()[0];

			_input[0] = _inputMatrix[6];
			_input[1] = _inputMatrix[7];
			_network.CalculateFeedforward();
			output[3] = _network.CollectOutput()[0];

			return output;
		}

		public bool BiasNeuron
		{
			get {return _config.BiasNeuronEnable.Value;}
            set { _config.BiasNeuronEnable.Value = value; }
		}

		public Network Network
		{
			get {return _network;}
		}

		public void SetMatrix(double[] inputMatrix, double[] desiredMatrix)
		{
			this._inputMatrix = inputMatrix;
			this._desiredMatrix = desiredMatrix;
		}
	}
}
