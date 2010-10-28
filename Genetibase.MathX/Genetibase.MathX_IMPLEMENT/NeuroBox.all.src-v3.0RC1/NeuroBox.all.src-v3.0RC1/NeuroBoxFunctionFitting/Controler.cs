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
using System.Text;

using NeuroBox;
using MathNet.Numerics.Statistics;

namespace NeuroBox.FunctionFitting
{
    /// <remarks>
    /// It is recommended to wrap non-static sample providers with the <seealso cref="CachedSampleProvider"/>
    /// unless you use a non-deterministic provider and would like to apply different samples every epoch.
    /// </remarks>
    public class Controler : IConfigurable
    {
        private ConfigNode _node;
        private TrainingConfig _config;
        private readonly double[] _inputPattern;
        private readonly double[] _outputTraining;
        private readonly double[] _output;
        private Network _network;
        private INetworkStructureBuilder _builder;
        private ISampleProvider _provider;
        private readonly int _dimensions;

        /// <summary>Event that fires as soon as the network gets backpropagated.</summary>
        public event EventHandler NetworkTrained;
        /// <summary>Event that fires as soon as the network gets propagated.</summary>
        public event EventHandler NetworkCalculated;
        /// <summary>Event that fires as soon as the input data changed.</summary>
        public event EventHandler InputChanged;
        /// <summary>Event that fires as soon as the output data changed.</summary>
        public event EventHandler TrainingChanged;
        /// <summary>Event that fires as soon as the network is rebuilt.</summary>
        public event EventHandler NetworkRebuilt;

        #region Construction & Building

        public Controler(int dimensions)
        {
            ConfigNode.AttachRootNodeToHost(this);
            _dimensions = dimensions;
            _inputPattern = new double[dimensions];
            _outputTraining = new double[1];
            _output = new double[1];
        }

        public Controler(ConfigNode config, int dimensions)
        {
            config.AttatchToHost(this);
            _dimensions = dimensions;
            _inputPattern = new double[dimensions];
            _outputTraining = new double[1];
            _output = new double[1];
        }

        /// <summary>
        /// Builds the Network. Call this method straight after the constructor.
        /// </summary>
        /// <param name="adaptConfiguration"></param>
        public void BuildNetwork(bool adaptConfiguration)
        {
            _network = new Network(_node);
            if(_builder == null)
                _builder = new DefaultStructureBuilder(_dimensions, 1);
            _builder.Network = _network;
            if(adaptConfiguration)
                _builder.AdaptConfiguration();
            _builder.ConnectNetwork();
            _network.FirstLayer.BindActivity(_inputPattern);
            _network.LastLayer.BindTraining(_outputTraining);
        }

        /// <summary>
        /// Imports a Network. Call this method instead of <see cref="BuildNetwork"/> if you want to use your own network structure.
        /// </summary>
        /// <param name="network"></param>
        /// <param name="adaptConfiguration"></param>
        public void ImportNetwork(Network network, bool adaptConfiguration)
        {
            _network = network;
            _node.AttatchToHost(_network);
            if(_builder == null)
                _builder = new DefaultStructureBuilder(_dimensions, 1);
            _builder.Network = _network;
            if(adaptConfiguration)
                _builder.AdaptConfiguration();
            _network.FirstLayer.BindActivity(_inputPattern);
            _network.LastLayer.BindTraining(_outputTraining);
        }

        /// <summary>
        /// Rebuilds a new network ready for a new training session.
        /// </summary>
        public void RebuildNetwork()
        {
            BuildNetwork(false);
            if(NetworkRebuilt != null)
                NetworkRebuilt(this, EventArgs.Empty);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Access the internal neural network structure.
        /// </summary>
        public Network NeuralNetwork
        {
            get { return _network; }
        }

        /// <summary>
        /// Access the network structure builder.
        /// </summary>
        /// <remarks>
        /// This builder is responsible for connecting the network layers together.
        /// </remarks>
        public INetworkStructureBuilder StructureBuilder
        {
            get { return _builder; }
            set { _builder = value; }
        }

        public ISampleProvider Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public int Dimensions
        {
            get { return _dimensions; }
        }
        #endregion

        #region Apply Sample
        /// <summary>
        /// Push input data to the input layer.
        /// </summary>
        public void PushInput(params double[] input)
        {
            input.CopyTo(_inputPattern, 0);
            if(!_config.Basic.QuietModeEnable.Value && InputChanged != null)
                InputChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Push training data to the output layer.
        /// </summary>
        public void PushTraining(double training)
        {
            _outputTraining[0] = training;
            if(!_config.Basic.QuietModeEnable.Value && TrainingChanged != null)
                TrainingChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Push a sample to the layers.
        /// </summary>
        public void PushSample(Sample sample)
        {
            PushInput(sample.Coordinate);
            PushTraining(sample.Value);
        }
        #endregion

        #region Propagation & Training
        /// <summary>
        /// Train the current network/pattern.
        /// </summary>
        public void TrainCurrentNetwork()
        {
            _network.TrainCurrentPattern(true, false);
            if(!_config.Basic.QuietModeEnable.Value && NetworkTrained != null)
                NetworkTrained(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calculate/Propagate the current network/pattern.
        /// </summary>
        /// <returns>The position of the output neuron with the biggest output.</returns>
        public double CalculateCurrentNetwork()
        {
            _network.CalculateFeedforward();
            _network.CollectOutput(_output);
            if(!_config.Basic.QuietModeEnable.Value && NetworkCalculated != null)
                NetworkCalculated(this, EventArgs.Empty);
            return _output[0];
        }

        public double EvaluateFunction(params double[] coordinate)
        {
            PushInput(coordinate);
            return CalculateCurrentNetwork();
        }
        #endregion

        #region Auto Training
        public double EstimateMeanSquaredError()
        {
            Accumulator accu = new Accumulator();
            foreach(Sample s in _provider)
            {
                double v = EvaluateFunction(s.Coordinate);
                accu.Add(v - s.Value);
            }
            return accu.MeanSquared;
        }

        public void TrainAllSamplesOnce()
        {
            foreach(Sample s in _provider)
            {
                PushSample(s);
                TrainCurrentNetwork();
            }
        }

        /// <summary>
        /// Trains all samples in turn until the mean squared error sinks below a given threshold, with a limited number of epochs (timeout condition).
        /// </summary>
        /// <param name="mseThreshold">Mean Squared Error Threshold</param>
        /// <param name="maxNumOfEpochs">Maximum number of epochs. Every sample is trained once per epoch.</param>
        /// <returns>True if the threshold condition was met.</returns>
        public bool TrainAllSamplesUntil(double mseThreshold, int maxNumOfEpochs)
        {
            for(int i = 0; i < maxNumOfEpochs; i++)
            {
                TrainAllSamplesOnce();
                if(EstimateMeanSquaredError() <= mseThreshold)
                    return true;
            }
            return false;
        }
        #endregion

        #region IConfigurable Members
        string IConfigurable.Name
        {
            get { return "FunctionFittingControler"; }
        }
        void IConfigurable.Rebind(ConfigNode node)
        {
            _node = node;
            if(_config == null)
                _config = new TrainingConfig(_node);
            else
                _config.Node = _node;
        }
        public ConfigNode Node
        {
            get { return _node; }
        }
        #endregion
    }
}
