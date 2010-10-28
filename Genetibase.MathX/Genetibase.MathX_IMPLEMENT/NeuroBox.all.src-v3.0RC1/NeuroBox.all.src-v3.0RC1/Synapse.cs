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
    /// A Synapse is the (directed) connection between Neurons.
    /// All the information of a neural network ist stored in
    /// the existance, direction and weight of those synapses.
    /// </summary>
    [Serializable]
    public class Synapse : IConfigurable
    {
        private ConfigNode _node;
        private BasicConfig _config;
        private Neuron _sourceNeuron; //neurons feeding this synapse
        private Neuron _targetNeuron; //neurons that synapse feeds
        private double _weight; //weight factor
        private double _weightBuffer = 0; //Buffer for MomentumTerm Optimization
        private object _tag;

        /// <summary>
        /// Fired if the synapse's weight changed.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> WeightChanged;

        /// <summary>
        /// Instanciate a new synapse. You'll hardly need to call this as the preferred way to create neurons is using the neuron, layer or even network classes.
        /// </summary>
        /// <param name="source">The source neuron feeding this synapse.</param>
        /// <param name="target">The target neuron fed by this synapse.</param>
        /// <param name="weight">The weight of the synapse.</param>
        public Synapse(ConfigNode node, Neuron source, Neuron target, double weight)
        {
            node.AttatchToHost(this);
            _sourceNeuron = source;
            _targetNeuron = target;
            _weight = weight;
        }
        /// <summary>
        /// Instanciate a new synapse. You'll hardly need to call this as the preferred way to create neurons is using the neuron, layer or even network classes.
        /// </summary>
        /// <param name="source">The source neuron feeding this synapse.</param>
        /// <param name="target">The target neuron fed by this synapse.</param>
        public Synapse(ConfigNode node, Neuron source, Neuron target)
        {
            node.AttatchToHost(this);
            _sourceNeuron = source;
            _targetNeuron = target;
            _weight = _config.RandomInitialWeight();
        }

        /// <summary>
        /// Use this tag for whatever you want.
        /// </summary>
        public object Tag
        {
            set { _tag = value; }
            get { return _tag; }
        }

        public BasicConfig BasicConfiguration
        {
            get { return _config; }
        }

        /// <summary>
        /// The Synapse's weight.
        /// </summary>
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        #region Network Architecture
        /// <summary>
        /// The target neuron fed by this synapse.
        /// </summary>
        public Neuron TargetNeuron
        {
            get { return _targetNeuron; }
        }
        /// <summary>
        /// The source neuron feeding this synapse.
        /// </summary>
        public Neuron SourceNeuron
        {
            get { return _sourceNeuron; }
        }
        /// <summary>
        /// Removes this synapse connection between the source and the target neuron.
        /// </summary>
        public void Disconnect()
        {
            _sourceNeuron.DisconnectToNeuron(this);
        }
        #endregion

        #region Propagation and Backpropagation
        /// <summary>
        /// Calculates the product of the source neuron's output and the synapse's weight.
        /// </summary>
        public double CalculateCurrentFlow()
        {
            return _sourceNeuron.CurrentOutput * _weight;
        }
        /// <summary>
        /// Calculates the product of the target neuron's delta and the synapse's weight.
        /// </summary>
        public double CalculateCurrentFlowDifferential()
        {
            return _targetNeuron.CurrentDelta * _weight;
        }
        /// <summary>
        /// Train this synapse.
        /// </summary>
        public void OptimizeWeights()
        {
            double buffer = _weight;
            if(_config.WeightDecayEnable.Value)
                _weight -= _weight * _config.WeightDecay.Value;
            if(_config.MomentumTermEnable.Value)
                _weight += _weightBuffer * _config.MomentumTerm.Value;
            _weightBuffer = buffer;
            if(_config.ManhattanTrainingEnable.Value)
                _weight += _config.LearningRate.Value * _sourceNeuron.CurrentOutput * Math.Sign(_targetNeuron.CurrentDelta);
            else
                _weight += _config.LearningRate.Value * _sourceNeuron.CurrentOutput * _targetNeuron.CurrentDelta;
            _weight += _config.PreventWeightSymmetrySummand();
            if(WeightChanged != null && !_config.QuietModeEnable.Value && buffer != _weight)
                WeightChanged(this, new ValueChangedEventArgs(_weight, buffer));
        }
        #endregion

        #region IConfigurable Members
        string IConfigurable.Name
        {
            get { return "Synapse"; }
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
