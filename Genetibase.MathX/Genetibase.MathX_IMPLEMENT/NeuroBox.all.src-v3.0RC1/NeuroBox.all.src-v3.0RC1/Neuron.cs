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
using System.Collections.Generic;

namespace NeuroBox
{
	/// <summary>
	/// Neurons are the nodes of neural networks, as well as their interface
	/// to the (concrete) world around the network. Neurons are connected
	/// together using synapses.
	/// </summary>
	[Serializable]
	public class Neuron : IConfigurable
	{
        private ConfigNode _node;
        private BasicConfig _config;
		private Layer _layer;
		private double _cActivity; //current activity
		private double _cOutput; //current output
		private double _cDelta, _cTrain;
		private double _cDeltaBatch; //offline delta sum
		private int _cBatchCount;
		private List<Synapse> _sourceSynapses; //synapses feeding this neuron
        private List<Synapse> _targetSynapses; //synapses this neuron feeds.
		private object _tag;
		private string _title;
		private double _cBiasNeuronWeight;

		/// <summary>Fired if the neuron's activity changed.</summary>
		public event EventHandler<ValueChangedEventArgs> ActivityChanged;
		/// <summary>Fired if the neuron's output changed.</summary>
        public event EventHandler<ValueChangedEventArgs> OutputChanged;
		/// <summary>Fired if a new target synapse is added.</summary>
        public event EventHandler<SynapseEventArgs> TargetSynapseAdded;
		/// <summary>Fired if a target synapse is removed.</summary>
        public event EventHandler<SynapseEventArgs> TargetSynapseRemoved;

		/// <summary>
		/// Instanciate a new neuron. You'll hardly need to call this as the preferred way to create neurons is using the layer or even network classes.
		/// </summary>
		/// <param name="layer">The layer to refer this neuron to.</param>
		/// <param name="config">The configuration instance.</param>
		public Neuron(ConfigNode node, Layer layer)
		{
            node.AttatchToHost(this);
			_cDelta = 0;
			_cTrain = 0;
			_cActivity = 0;
			_cOutput = 0;
            _cBiasNeuronWeight = _config.RandomInitialWeight();
			_layer = layer;
            _sourceSynapses = new List<Synapse>();
            _targetSynapses = new List<Synapse>();
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

		/// <summary>
		/// This neuron's name.
		/// </summary>
		public string Title
		{
			set {_title = value;}
			get {return _title;}
		}

        public double BiasNeuronWeight
        {
            get { return _cBiasNeuronWeight; }
            set { _cBiasNeuronWeight = value; }
        }
		
		#region Network Architecture
		/// <summary>
		/// Connect this neuron forward to another (specified) neuron.
		/// </summary>
		/// <param name="target">The target neuron.</param>
		/// <returns>The new connection synapse.</returns>
		public Synapse ConnectToNeuron(Neuron target)
		{
			Synapse s = new Synapse(_node.BuildSibling(),this,target);
			_targetSynapses.Add(s);
			target._sourceSynapses.Add(s);
			if(TargetSynapseAdded != null)
				TargetSynapseAdded(this,new SynapseEventArgs(s));
			return s;
		}
		/// <summary>
		/// Connect this neuron forward to another (specified) neuron.
		/// </summary>
		/// <param name="target">The target neuron.</param>
		/// <param name="weight">The weight of the synapse.</param>
		/// <returns>The new connection synapse.</returns>
		public Synapse ConnectToNeuron(Neuron target, double weight)
		{
            Synapse s = new Synapse(_node.BuildSibling(), this, target, weight);
			_targetSynapses.Add(s);
			target._sourceSynapses.Add(s);
			if(TargetSynapseAdded != null)
				TargetSynapseAdded(this,new SynapseEventArgs(s));
			return s;
		}
		/// <summary>
		/// Remove a connection from this neuron forward to another neuron.
		/// </summary>
		/// <param name="s">The synapse feeding this neuron.</param>
		public void DisconnectToNeuron(Synapse s)
		{
			_targetSynapses.Remove(s);
			s.TargetNeuron._sourceSynapses.Remove(s);
			if(TargetSynapseRemoved != null)
				TargetSynapseRemoved(this,new SynapseEventArgs(s));
		}
		/// <summary>
		/// Remove all connections from this neuron forward to another (specified) neuron.
		/// </summary>
		/// <param name="source">The neuron feeding this neuron.</param>
		public void DisconnectToNeuron(Neuron source)
		{
			for(int i=0;i<_targetSynapses.Count;i++)
			{
				Synapse s = _targetSynapses[i];
				if(s.TargetNeuron == source)
				{
					_targetSynapses.Remove(s);
					s.TargetNeuron._sourceSynapses.Remove(s);
					if(TargetSynapseRemoved != null)
						TargetSynapseRemoved(this,new SynapseEventArgs(s));
					i--;
				}
			}
		}
		/// <summary>
		/// Remove all connections from this neuron to other neurons.
		/// </summary>
		public void DisconnectToAllNeurons()
		{
			for(int i=_targetSynapses.Count-1;i>-1;i--)
				DisconnectToNeuron(_targetSynapses[i]);
		}
		/// <summary>
		/// Remove all connections (to and from this neuron).
		/// </summary>
		public void IsolateNeuron()
		{
			DisconnectToAllNeurons();
			for(int i=_sourceSynapses.Count-1;i>-1;i--)
			{
				Synapse s = _sourceSynapses[i];
				s.SourceNeuron.DisconnectToNeuron(s);
			}
		}
		/// <summary>
		/// The Layer the neuron is placed in.
		/// </summary>
		public Layer Layer
		{
			get {return(_layer);}
			set {_layer = value;}
		}
		/// <summary>
		/// Access to all incoming synapses.
		/// </summary>
		/// <remarks>
		/// Do NOT use this collection to modify network structure!
		/// </remarks>
		public List<Synapse> SourceSynapses  //TODO: consider change to ReadOnlyCollection<Synapse>
		{
			get {return _sourceSynapses;}
		}
		#endregion

		#region Propagation
		/// <summary>
		/// The current activity of the neuron.
		/// </summary>
		/// <remarks>
		/// This value may be between positive and negative infinity,
		/// however shall not be too far away from 0 (-5..5 ist best).
		/// The Activity on input neurons is autmatically bound to
		/// the input array. On hidden and output neurons it's evaluated
		/// using the process of network propagation.
		/// </remarks>
		public double CurrentActivity
		{
			get {return(_cActivity);}
			set
			{
				double old = _cActivity;
				_cActivity = value;
				if(!_config.QuietModeEnable.Value && _cActivity != old && ActivityChanged != null)
					ActivityChanged(this,new ValueChangedEventArgs(_cActivity,old));
			}
		}
		/// <summary>
		/// The current output of the neuron.
		/// </summary>
		/// <remarks>
		/// This value is automatically determined using the activation function.
		/// </remarks>
		public double CurrentOutput
		{
			get	{return _cOutput;}
		}
		/*
		public double CurrentOnNeuronDifferentialFlow
		{
			get {return cDelta * cOnNeuronWeight;}
		}*/
		/// <summary>
		/// Reset the neuron activity and output.
		/// </summary>
		public virtual void ResetStatus()
		{
			double oldActivity = _cActivity;
			_cActivity = 0;
            if(!_config.QuietModeEnable.Value && oldActivity != 0 && ActivityChanged != null)
				ActivityChanged(this,new ValueChangedEventArgs(0,oldActivity));
			double oldOutput = _cOutput;
			_cOutput = 0;
            if(!_config.QuietModeEnable.Value && oldOutput != 0 && OutputChanged != null)
				OutputChanged(this,new ValueChangedEventArgs(0,oldOutput));
			_cDelta = 0;
		}
		/// <summary>
		/// Propagate the neuron.
		/// </summary>
		/// <remarks>
		/// To publish the propagated data, you need to call PublishPropagation afterwards.
		/// </remarks>
		public virtual void Propagate()
		{
			if(_sourceSynapses.Count > 0)
			{
				double old = _cActivity;
				_cActivity = 0;
				if(_config.BiasNeuronEnable.Value && _sourceSynapses.Count!=0)
					_cActivity = _config.BiasNeuronOutput.Value*_cBiasNeuronWeight;
				for(int i=0;i<_sourceSynapses.Count;i++)
				{
					Synapse s = _sourceSynapses[i];
					_cActivity += s.CalculateCurrentFlow();
				}
                if(!_config.QuietModeEnable.Value && _cActivity != old && ActivityChanged != null)
					ActivityChanged(this,new ValueChangedEventArgs(_cActivity,old));
			}
		}
		/// <summary>
		/// Publish the neuron propagation to the output.
		/// </summary>
		public virtual void PublishPropagation()
		{
			double old = _cOutput;
			_cOutput = _config.Activate(_cActivity);
            if(!_config.QuietModeEnable.Value && _cOutput != old && OutputChanged != null)
				OutputChanged(this,new ValueChangedEventArgs(_cOutput,old));
		}
		#endregion

		#region BackPropagation
		/// <summary>
		/// The current error function delta of this neuron.
		/// </summary>
		public double CurrentDelta
		{
			get {return(_cDelta);}
		}
		/// <summary>
		/// The 'should be' training pattern for backpropagation.
		/// </summary>
		/// <remarks>
		/// This value is considered only on non-feeding neurons (eg. output neurons).
		/// </remarks>
		public double CurrentTraining
		{
			get {return(_cTrain);}
			set {_cTrain = value;}
		}
		/// <summary>
		/// Backpropagate this neuron using the current training pattern.
		/// </summary>
		public void BackPropagate()
		{
			EvaluateGradient();
			OptimizeWeights();
		}
		/// <summary>
		/// Evaluate the gradient for the current pattern.
		/// </summary>
		protected virtual void EvaluateGradient()
		{
			if(_targetSynapses.Count==0) //Output Neuron
				_cDelta = _config.ActivateDerivative(_cActivity, _cOutput) * (_cTrain - _cOutput);
			else //Input or Hidden Neuron
			{
				_cDelta = 0;
				for(int i=0;i<_targetSynapses.Count;i++)
				{
					Synapse s = _targetSynapses[i];
					_cDelta += s.CalculateCurrentFlowDifferential();
				}
				_cDelta *= _config.ActivateDerivative(_cActivity, _cOutput);
			}
		}
		/// <summary>
		/// Optimize the weights based on the published gradient data.
		/// </summary>
        public void OptimizeWeights()
        {
            for(int i = 0; i < _sourceSynapses.Count; i++)
                (_sourceSynapses[i]).OptimizeWeights();
            if(_config.BiasNeuronEnable.Value)
            {
                double buffer = _cBiasNeuronWeight;
                if(_config.WeightDecayEnable.Value)
                    _cBiasNeuronWeight -= _cBiasNeuronWeight * _config.WeightDecay.Value;
                if(_config.ManhattanTrainingEnable.Value)
                    _cBiasNeuronWeight += _config.LearningRate.Value * _config.BiasNeuronOutput.Value * Math.Sign(_cDelta);
                else
                    _cBiasNeuronWeight += _config.LearningRate.Value * _config.BiasNeuronOutput.Value * _cDelta;
                _cBiasNeuronWeight += _config.PreventWeightSymmetrySummand();
            }
        }
		/// <summary>
		/// Batch. Analyze the current pattern.
		/// </summary>
		public void BatchEvaluateGradient()
		{
			EvaluateGradient();
			_cDeltaBatch += _cDelta;
			_cBatchCount++;
		}
		/// <summary>
		/// Batch. Publish all analyzed pattern data.
		/// </summary>
		public void BatchPublishGradient()
		{
			_cDelta = _cDeltaBatch/_cBatchCount;
			_cBatchCount = 0;
			_cDeltaBatch = 0;
		}
		
		#endregion

		#region Batch Backpropagation
		
		#endregion

        #region IConfigurable Members
        string IConfigurable.Name
        {
            get { return "Neuron"; }
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
