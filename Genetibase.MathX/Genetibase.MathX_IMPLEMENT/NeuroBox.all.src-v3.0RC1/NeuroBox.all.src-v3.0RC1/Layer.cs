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
using System.Collections.ObjectModel;
using System.Collections;

namespace NeuroBox
{
    /// <summary>
    /// In feedforward networks, synapses are organized in layers.
    /// </summary>
    [Serializable]
    public class Layer : Collection<Neuron>, IConfigurable
    {
        private ConfigNode _node;
        private BasicConfig _config;
        private Layer _sourceLayer = null;
        private Layer _targetLayer = null;
        private LayerBehaviorFlags _behavior;
        private double[] _binding = null;
        private double[] _training = null;
        private object _tag;
        private string _title;

        /// <summary>Fired if a new neuron is added.</summary>
        public event EventHandler<NeuronEventArgs> NeuronAdded;
        /// <summary>Fired if a neuron is removed.</summary>
        public event EventHandler<NeuronEventArgs> NeuronRemoved;
        /// <summary>Fired if a feeded layer is appended.</summary>
        public event EventHandler<LayerEventArgs> LayerAppended;

        /// <summary>
        /// Instanciate a new layer of input neurons. You'll hardly need to call this as the preferred way to create layers is using the network classes.
        /// </summary>
        /// <param name="neurons">A list of neurons to put into this layer.</param>
        /// <param name="binding">The input data to bind to this input layer.</param>
        /// <param name="config">The configuration instance.</param>
        public Layer(ConfigNode node, IEnumerable<Neuron> neurons, double[] binding)
        {
            node.AttatchToHost(this);
            _behavior = LayerBehaviorFlags.Bound;
            _binding = binding;
            AddRange(neurons);
        }
        /// <summary>
        /// Instanciate a new layer of input neurons. You'll hardly need to call this as the preferred way to create layers is using the network classes.
        /// </summary>
        /// <param name="neuronCount">The count of neurons this input layer shall create.</param>
        /// <param name="binding">The input data to bind to this input layer.</param>
        /// <param name="config">The configuration instance.</param>
        public Layer(ConfigNode node, int neuronCount, double[] binding)
        {
            node.AttatchToHost(this);
            _behavior = LayerBehaviorFlags.Bound;
            _binding = binding;
            AddRange(neuronCount);
        }
        /// <summary>
        /// Instanciate a new layer of non-input neurons. You'll hardly need to call this as the preferred way to create layers is using the network classes.
        /// </summary>
        /// <param name="neurons">A list of neurons to put into this layer.</param>
        /// <param name="source">The layer that will feed/point to this layer.</param>
        /// <param name="config">The configuration instance.</param>
        public Layer(ConfigNode node, IEnumerable<Neuron> neurons, Layer source)
        {
            node.AttatchToHost(this);
            _behavior = LayerBehaviorFlags.Feeded;
            _sourceLayer = source;
            AddRange(neurons);
            source.FeedLayer(this);
        }
        /// <summary>
        /// Instanciate a new layer of non-input neurons. You'll hardly need to call this as the preferred way to create layers is using the network classes.
        /// </summary>
        /// <param name="neuronCount">The count of neurons this layer shall create.</param>
        /// <param name="source">The layer that will feed/point to this layer.</param>
        /// <param name="config">The configuration instance.</param>
        public Layer(ConfigNode node, int neuronCount, Layer source)
        {
            node.AttatchToHost(this);
            _behavior = LayerBehaviorFlags.Feeded;
            _sourceLayer = source;
            AddRange(neuronCount);
            source.FeedLayer(this);
        }
        /// <summary>
        /// Instanciate a new layer of neurons. You'll hardly need to call this as the preferred way to create layers is using the network classes.
        /// </summary>
        /// <param name="neurons">A list of neurons to put into this layer.</param>
        /// <param name="config">The configuration instance.</param>
        public Layer(ConfigNode node, IEnumerable<Neuron> neurons)
        {
            node.AttatchToHost(this);
            _behavior = LayerBehaviorFlags.None;
            AddRange(neurons);
        }
        /// <summary>
        /// Instanciate a new layer of neurons. You'll hardly need to call this as the preferred way to create layers is using the network classes.
        /// </summary>
        /// <param name="neuronCount">The count of neurons this layer shall create.</param>
        /// <param name="config">The configuration instance.</param>
        public Layer(ConfigNode node, int neuronCount)
        {
            node.AttatchToHost(this);
            _behavior = LayerBehaviorFlags.None;
            AddRange(neuronCount);
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
        /// This layer's name.
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }

        /// <summary>
        /// Set the neuron's names.
        /// </summary>
        /// <param name="titles">The titles of the neurons ordered by position.</param>
        public void SetNeuronTitles(params string[] titles)
        {
            for(int i = 0; i < titles.Length && i < Count; i++)
                this[i].Title = titles[i];
        }

        #region Bindings
        /// <summary>
        /// Bind the activity of this layer's neurons to a double array.
        /// </summary>
        /// <param name="source">the array to bind to</param>
        public void BindActivity(double[] source)
        {
            _behavior |= LayerBehaviorFlags.Bound;
            _binding = source;
        }
        /// <summary>
        /// Copy the double array into this layer's neuron's activities.
        /// </summary>
        /// <remarks>
        /// Use this method if working in an unbound scenario.
        /// Note that this method overwrites data in the bound vector.
        /// </remarks>
        /// <param name="source">The array containing new activity data.</param>
        public void PushActivity(double[] source)
        {
            source.CopyTo(_binding, 0);
        }
        /// <summary>
        /// Bind the training values of this layer's neurons to a double array.
        /// </summary>
        /// <param name="training">the array to bind to</param>
        public void BindTraining(double[] training)
        {
            _training = training;
        }
        /// <summary>
        /// Copy the double array into this layer's neuron's training.
        /// </summary>
        /// <remarks>
        /// Use this method if working in an unbound scenario.
        /// Note that this method overwrites data in the bound vector.
        /// </remarks>
        /// <param name="training">The array containing new training data.</param>
        public void PushTraining(double[] training)
        {
            if(_training == null)
                _training = new double[Count];
            training.CopyTo(_training, 0);
        }
        /// <summary>
        /// Remove the bindings of this layer's neurons.
        /// </summary>
        public void RemoveActivityBinding()
        {
            _behavior &= ~LayerBehaviorFlags.Bound;
            _binding = null;
        }
        #endregion

        #region Monitoring
        /// <summary>
        /// True if you're interested in this neuron's output (eg. on output neurons).
        /// </summary>
        public bool IsMonitored
        {
            get { return (_behavior & LayerBehaviorFlags.Monitored) == LayerBehaviorFlags.Monitored; }
            set
            {
                if(value)
                    _behavior |= LayerBehaviorFlags.Monitored;
                else
                    _behavior &= ~LayerBehaviorFlags.Monitored;
            }
        }
        /// <summary>
        /// Collect neuron's values to a double array.
        /// </summary>
        /// <returns>The output array.</returns>
        public double[] CollectOutput()
        {
            double[] rsp = new double[Count];
            for(int i = 0; i < Count; i++)
                rsp[i] = this[i].CurrentOutput;
            return rsp;
        }
        /// <summary>
        /// Collect neuron's values to a double array.
        /// </summary>
        /// <param name="target">The array to copy the outputs to.</param>
        public void CollectOutput(double[] target)
        {
            for(int i = 0; i < Count; i++)
                target[i] = this[i].CurrentOutput;
        }
        #endregion

        #region Network Architecture
        private void FeedLayer(Layer layer)
        {
            _behavior |= LayerBehaviorFlags.Feeding;
            _targetLayer = layer;
            if(LayerAppended != null)
                LayerAppended(this, new LayerEventArgs(layer));
        }
        /// <summary>
        /// Create a layer that is feeded by this layer.
        /// </summary>
        /// <param name="neuronCount">The number of neuros to be placed into the new layer.</param>
        /// <returns>The new layer.</returns>
        public Layer AppendFeededLayer(int neuronCount)
        {
            return new Layer(_node.BuildSibling(), neuronCount, this);
        }
        /// <summary>
        /// Add a count of neurons to this layer.
        /// </summary>
        /// <param name="neuronCount">The count of neurons to add.</param>
        public void AddRange(int neuronCount)
        {
            for(int i = 0; i < neuronCount; i++)
                Add(new Neuron(_node.BuildChild(), this));
        }
        /// <summary>
        /// Add an array of neurons to this layer.
        /// </summary>
        /// <param name="neurons">The neurons to add.</param>
        public void AddRange(IEnumerable<Neuron> neurons)
        {
            foreach(Neuron n in neurons)
                Add(n);
        }

        protected override void InsertItem(int index, Neuron item)
        {
            base.InsertItem(index, item);
            item.Layer = this;
            if(NeuronAdded != null)
                NeuronAdded(this, new NeuronEventArgs(item));
        }
        protected override void RemoveItem(int index)
        {
            if(NeuronRemoved != null)
                NeuronRemoved(this, new NeuronEventArgs(this[index]));
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, Neuron item)
        {
            if(NeuronRemoved != null)
                NeuronRemoved(this, new NeuronEventArgs(this[index]));
            base.SetItem(index, item);
            item.Layer = this;
            if(NeuronAdded != null)
                NeuronAdded(this, new NeuronEventArgs(item));
        }

        /// <summary>
        /// Evaluates the index of the neuron with the biggest output.
        /// </summary>
        /// <returns>The neuron's index</returns>
        public int EvaluateBiggestOutputNeuron()
        {
            double d = this[0].CurrentOutput;
            int index = 0;
            for(int i = 0; i < Count; i++)
                if(d < this[i].CurrentOutput)
                {
                    index = i;
                    d = this[i].CurrentOutput;
                }
            return index;
        }
        /// <summary>
        /// Evaluates the index of the neuron with the biggest output.
        /// </summary>
        /// <returns>The neuron's index</returns>
        public int EvaluateBiggestOutputNeuron(out double outputLeadMargin)
        {
            double d = this[0].CurrentOutput;
            outputLeadMargin = d;
            int index = 0;
            for(int i = 0; i < Count; i++)
                if(d < this[i].CurrentOutput)
                {
                    index = i;
                    outputLeadMargin = this[i].CurrentOutput - d;
                    d = this[i].CurrentOutput;
                }
            return index;
        }
        /// <summary>
        /// The layer feeding/pointing to this layer.
        /// </summary>
        public Layer SourceLayer
        {
            get { return _sourceLayer; }
        }
        /// <summary>
        /// The layer that is fed/pointed to by this layer.
        /// </summary>
        public Layer TargetLayer
        {
            get { return _targetLayer; }
        }
        /// <summary>
        /// Link all neurons of this layer together.
        /// </summary>
        public void CrossLinkLayer()
        {
            if(_binding == null) //never crosslink input layer...
                for(int i = 0; i < Count; i++)
                    for(int j = 0; j < Count; j++)
                        if(i != j)
                            this[i].ConnectToNeuron(this[j]);
        }
        /// <summary>
        /// Link all neurons of this layer forward to all neurons of the next layer.
        /// </summary>
        public void CrossLinkForward()
        {
            if(_targetLayer != null)
                for(int i = 0; i < Count; i++)
                    for(int j = 0; j < _targetLayer.Count; j++)
                        this[i].ConnectToNeuron(_targetLayer[j]);
        }
        /// <summary>
        /// Link the whole network forward starting from this layer.
        /// </summary>
        /// <param name="selfLink">True if all the layers shall be self linked.</param>
        public void RecursiveCrossLinkForward(bool selfLink)
        {
            if(selfLink)
                CrossLinkLayer();
            if(_targetLayer != null)
            {
                CrossLinkForward();
                _targetLayer.RecursiveCrossLinkForward(selfLink);
            }
        }
        /// <summary>
        /// Removes all synapse from and to this layer.
        /// </summary>
        public void IsolateLayer()
        {
            for(int i = 0; i < Count; i++)
                this[i].IsolateNeuron();
        }
        /// <summary>
        /// Removes all synapsed from the network starting from this layer.
        /// </summary>
        public void RecursiveIsolateLayer()
        {
            IsolateLayer();
            if(_targetLayer != null)
                _targetLayer.RecursiveIsolateLayer();
        }
        #endregion

        #region Propagation
        /// <summary>
        /// Reset all neurons of this layer.
        /// </summary>
        public void ResetLayer()
        {
            for(int i = 0; i < Count; i++)
                this[i].ResetStatus();
        }
        /// <summary>
        /// Reset all neurons of the network starting from this layer.
        /// </summary>
        public void RecursiveResetLayer()
        {
            ResetLayer();
            if(_targetLayer != null)
                _targetLayer.RecursiveResetLayer();
        }
        /// <summary>
        /// Propagate all neurons of this layers.
        /// </summary>
        public void CalculateFeedforward()
        {
            if(_binding != null)
                for(int i = 0; i < Count && i < _binding.Length; i++)
                    this[i].CurrentActivity = _binding[i];
            else
                for(int i = 0; i < Count; i++)
                    this[i].Propagate();
            for(int i = 0; i < Count; i++)
                this[i].PublishPropagation();
        }
        /// <summary>
        /// Propagate all neurons of the network starting from this layer.
        /// </summary>
        public void RecursiveCalculateFeedforward()
        {
            CalculateFeedforward();
            if(_targetLayer != null)
                _targetLayer.RecursiveCalculateFeedforward();
        }
        #endregion

        #region BackPropagation
        /// <summary>
        /// Backpropagate all neurons of this layer.
        /// </summary>
        public void TrainCurrentPattern()
        {
            if(_training != null)
                for(int i = 0; i < Count && i < _training.Length; i++)
                    this[i].CurrentTraining = _training[i];
            for(int i = 0; i < Count; i++)
                this[i].BackPropagate();
        }
        /// <summary>
        /// Backpropagate all neurons of the network starting from this layer.
        /// </summary>
        public void RecursiveTrainCurrentPattern()
        {
            TrainCurrentPattern();
            if(_sourceLayer != null)
                _sourceLayer.RecursiveTrainCurrentPattern();
        }
        #endregion

        #region Batch BackPropagation
        /// <summary>
        /// Batch. Analyze the current pattern.
        /// </summary>
        public void RecursiveBatchEvaluateGradient()
        {
            if(_training != null)
                for(int i = 0; i < Count && i < _training.Length; i++)
                    this[i].CurrentTraining = _training[i];
            for(int i = 0; i < Count; i++)
                this[i].BatchEvaluateGradient();
            if(_sourceLayer != null)
                _sourceLayer.RecursiveBatchEvaluateGradient();
        }
        /// <summary>
        /// Batch. Publish all analyzed pattern data.
        /// </summary>
        public void RecursiveBatchPublishGradient()
        {
            for(int i = 0; i < Count; i++)
                this[i].BatchPublishGradient();
            if(_sourceLayer != null)
                _sourceLayer.RecursiveBatchPublishGradient();
        }
        /// <summary>
        /// Batch. Optimize the weights based on the published gradient data.
        /// </summary>
        public void RecursiveBatchOptimizeWeights()
        {
            for(int i = 0; i < Count; i++)
                this[i].OptimizeWeights();
            if(_sourceLayer != null)
                _sourceLayer.RecursiveBatchOptimizeWeights();
        }
        #endregion

        #region IConfigurable Members
        string IConfigurable.Name
        {
            get { return "Layer"; }
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

    /// <summary>
    /// Several Layer Behavior flags.
    /// </summary>
    [Flags]
    public enum LayerBehaviorFlags : int
    {
        /// <summary>Isolated Layer.</summary>
        None = 0,
        /// <summary>Bound to an input data array. Likely to be an Input Layer.</summary>
        Bound = 1,
        /// <summary>There's another layer feeding/pointing to this layer.</summary>
        Feeded = 2,
        /// <summary>This layer feeds/points to another layer.</summary>
        Feeding = 4,
        /// <summary>This layer is monitored. Likely to be an Output Layer.</summary>
        Monitored = 8,
        /// <summary>Not Yet Supported.</summary>
        Buffered = 16,
        /// <summary>Tis is an Input Layer.</summary>
        InputLayer = 5, //Bound, Feeding
        /// <summary>Tis is a Hidden Layer.</summary>
        HiddenLayer = 6, //Feeded, Feeding
        /// <summary>Tis is an Output Layer.</summary>
        OutputLayer = 10 //Feeded, Monitored
    }
}