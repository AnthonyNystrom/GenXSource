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

namespace NeuroBox
{
	/// <summary>
	/// Representing the whole neural network. Use this class to interact with the network.
	/// </summary>
	/// <example>
	/// <code>
	/// double[] d;
	/// Network network = new Network();
	/// network.Configuration.BackpropagationBehavior =
	///      BackpropagationBehaviorFlags.Optimized;
	/// double[] input = new double[64];
	/// double[] training = new Double[] {1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
	/// network.BindInputLayer(input);
	/// network.AddLayer(8);
	/// network.AddLayer(10);
	/// network.AutoLinkFeedforward();
	/// network.BindTraining(training);
	/// 
	/// //Calculate
	/// network.CalculateFeedforward();
	/// d = network.CollectOutput();
	/// ...
	/// 
	/// //Train
	/// network.TrainCurrentPattern(true,false);
	/// 
	/// //Train another pattern
	/// înput[2] = -5;
	/// ...
	/// training[0] = -1;
	/// training[1] = 1;
	/// network.TrainCurrentPattern(true,false);
	/// network.TrainCurrentPattern(true,false); //maybe two times...
	/// 
	/// //Calculate again..
	/// network.CalculateFeedforward();
	/// d = network.CollectOutput();
	/// ...
	/// </code>
	/// </example>
	[Serializable]
	public class Network : IConfigurable
    {
        private ConfigNode _node;
        private BasicConfig _config;
		private Layer _inputLayer;
		private Layer _lastLayer;
		private object _tag;

        private static Random _rnd = new Random();
        public static Random Random
        {
            get { return _rnd; }
        }

		/// <summary>
		/// Instanciate a new neural network.
		/// </summary>
		/// <remarks>
		/// Don't forget to bind your input data right after this.
		/// </remarks>
		public Network()
		{
            ConfigNode.AttachRootNodeToHost(this);
			_inputLayer = new Layer(_node.BuildChild(),0);
			_lastLayer = _inputLayer;
        }

        /// <summary>
        /// Instanciate a new neural network.
        /// </summary>
        /// <remarks>
        /// Don't forget to bind your input data right after this.
        /// </remarks>
        public Network(ConfigNode rootConfig)
        {
            rootConfig.AttatchToHost(this);
            _inputLayer = new Layer(_node.BuildChild(), 0);
            _lastLayer = _inputLayer;
        }

		/// <summary>
		/// Use this tag for whatever you want.
		/// </summary>
		public object Tag
		{
			set {_tag = value;}
			get {return _tag;}
		}

        public BasicConfig BasicConfiguration
        {
            get { return _config; }
        }

		#region Bindings
		/// <summary>
		/// Bind the double input array to the input layer.
		/// </summary>
		/// <param name="input">The array to bind to.</param>
		/// <returns>The input layer.</returns>
		public Layer BindInputLayer(double[] input)
		{
			if(input.Length != _inputLayer.Count)
			{
				_inputLayer.IsolateLayer();
				_inputLayer.Clear();
				_inputLayer.AddRange(input.Length);
			}
			_inputLayer.BindActivity(input);
			return _inputLayer;
		}
		/// <summary>
		/// Initializes the input layer in an unbound scenario.
		/// </summary>
		/// <param name="neuronCount">The count of input neurons</param>
		/// <returns>The input layer.</returns>
		public Layer InitUnboundInputLayer(int neuronCount)
		{
			if(neuronCount != _inputLayer.Count)
			{
				_inputLayer.IsolateLayer();
				_inputLayer.Clear();
				_inputLayer.AddRange(neuronCount);
			}
			_inputLayer.BindActivity(new double[neuronCount]);
			return _inputLayer;
		}
		/// <summary>
		/// Copy the double array into the input layer's neuron's activities.
		/// </summary>
		/// <remarks>
		/// Use this method if working in an unbound scenario.
		/// </remarks>
		/// <param name="input">The array containing new activity data.</param>
		public void PushUnboundInput(double[] input)
		{
			_inputLayer.PushActivity(input);
		}
		/// <summary>
		/// Copy the converted double array into the input layer's neuron's activities.
		/// </summary>
		/// <remarks>
		/// Use this method if working in an unbound scenario.
		/// </remarks>
		/// <param name="input">The array containing new activity data.</param>
		public void PushUnboundInput(bool[] input)
		{
			double[] d = new double[input.Length];
			for(int i=0;i<d.Length;i++)
				d[i] = _config.ConvertInput(input[i]);
			_inputLayer.PushActivity(d);
		}
		/// <summary>
		/// Bind the double training pattern array to the output layer.
		/// </summary>
		/// <returns>The output layer.</returns>
		public Layer BindTraining(double[] training)
		{
			_lastLayer.BindTraining(training);
			return _lastLayer;
		}
		/// <summary>
		/// Copy the double array into the output layer's neuron's trainings.
		/// </summary>
		/// <remarks>
		/// Use this method if working in an unbound scenario.
		/// </remarks>
		/// <param name="training">The array containing new training data.</param>
		public void PushUnboundTraining(double[] training)
		{
			_lastLayer.PushTraining(training);
		}
		/// <summary>
		/// Copy the converted double array into the output layer's neuron's trainings.
		/// </summary>
		/// <remarks>
		/// Use this method if working in an unbound scenario.
		/// </remarks>
		/// <param name="training">The array containing new training data.</param>
		public void PushUnboundTraining(bool[] training)
		{
			double[] d = new double[_lastLayer.Count];
			for(int i=0;i<d.Length;i++)
                d[i] = _config.ConvertOutput(training[i]);
			_lastLayer.PushTraining(d);
		}
		#endregion

		#region Network Structure
		/// <summary>
		/// Access the fist layer (Input Layer).
		/// </summary>
		public Layer FirstLayer
		{
			get {return _inputLayer;}
		}
		/// <summary>
		/// Access the last layer (Output Layer).
		/// </summary>
		public Layer LastLayer
		{
			get {return _lastLayer;}
		}
		/// <summary>
		/// Add a hidden or output layer to the network.
		/// </summary>
		/// <param name="neuronCount">The count of neurons to create.</param>
		/// <returns>The just created layer instance.</returns>
		public Layer AddLayer(int neuronCount)
		{
			_lastLayer.IsMonitored = false;
			_lastLayer = new Layer(_node.BuildChild(),neuronCount,_lastLayer);
			_lastLayer.IsMonitored = true;
			return _lastLayer;
		}
        public Layer AddLayer(int neuronCount, EActivationType method)
        {
            _lastLayer.IsMonitored = false;
            _lastLayer = new Layer(_node.BuildChild(), neuronCount, _lastLayer);
            _lastLayer.BasicConfiguration.ActivationType.Value = method;
            _lastLayer.IsMonitored = true;
            return _lastLayer;
        }

		/// <summary>
		/// Link all layers in feedforward style.
		/// </summary>
		public void AutoLinkFeedforward()
		{
			_inputLayer.RecursiveCrossLinkForward(false);
		}
		/// <summary>
		/// Link all layers and all neurons inside of the layers.
		/// </summary>
		public void AutoLinkFull()
		{
			_inputLayer.RecursiveCrossLinkForward(true);
		}
		/// <summary>
		/// Remove all synapses.
		/// </summary>
		public void CompleteDelink()
		{
			_inputLayer.RecursiveIsolateLayer();
		}
		#endregion

		#region Propagation
		/// <summary>
		/// Propagate the network.
		/// </summary>
		public void CalculateFeedforward()
		{
			_inputLayer.RecursiveCalculateFeedforward();
		}
		/// <summary>
		/// Reset all network propagation data.
		/// </summary>
		public void ResetNetwork()
		{
			_inputLayer.RecursiveResetLayer();
		}
		/// <summary>
		/// Collect output neuron values to a double array.
		/// </summary>
		/// <returns>The output of the network.</returns>
		public double[] CollectOutput()
		{
			return _lastLayer.CollectOutput();
		}
        /// <summary>
        /// Collect neuron's values to a double array.
        /// </summary>
        /// <param name="target">The array to copy the outputs to.</param>
        public void CollectOutput(double[] target)
        {
            _lastLayer.CollectOutput(target);
        }
		#endregion

		#region BackPropagation
		/// <summary>
		/// Backpropagate the network.
		/// </summary>
		/// <param name="calculateBefore">Propagate the network before backpropagation (recommended).</param>
		/// <param name="calculateAfter">Propagate the network after backpropagation.</param>
		public void TrainCurrentPattern(bool calculateBefore, bool calculateAfter)
		{
			if(calculateBefore)
				CalculateFeedforward();
			_lastLayer.RecursiveTrainCurrentPattern();
			if(calculateAfter)
				CalculateFeedforward();
		}
		#endregion

		#region Batch BackPropagation
		/// <summary>
		/// Analyze the current pattern for batch processing.
		/// </summary>
		public void BatchAnalyzeCurrentPattern()
		{
			_lastLayer.RecursiveBatchEvaluateGradient();
		}
		/// <summary>
		/// Optimize the synapses for all analyzed patterns (batch).
		/// </summary>
		public void BatchOptimizePatterns()
		{
			_lastLayer.RecursiveBatchPublishGradient();
			_lastLayer.RecursiveBatchOptimizeWeights();
		}
		#endregion

        #region IConfigurable Members
        string IConfigurable.Name
        {
            get { return "Network"; }
        }
        void IConfigurable.Rebind(ConfigNode node)
        {
            _node = node;
            if(_config == null)
                _config = new BasicConfig(node);
            else
                _config.Node = node;
        }
        public ConfigNode Node
        {
            get { return _node; }
        }
        #endregion
    }
}
