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
using NeuroBox;

namespace NeuroBox.PatternMatching
{
    /// <summary>
    /// Building Block for pattern matching scenarios. This is the class to work with.
    /// </summary>
    public class Controler : IControler, IConfigurable
    {
        private ConfigNode _node;
        private TrainingConfig _config;
        private PatternSet _patterns;
        private double[] _inputPattern;
        private double[] _outputTraining;
        private Network _network;
        private INetworkStructureBuilder _builder;
        private INetworkTrainer _trainer;
        private int _currentClassification = -1;

        /// <summary>Event that fires as soon as a new pattern is selected.</summary>
        public event EventHandler<PatternPositionEventArgs> PatternSelectionChanged;
        /// <summary>Event that fires as soon as the network gets backpropagated.</summary>
        public event EventHandler NetworkTrained;
        /// <summary>Event that fires as soon as the network gets propagated.</summary>
        public event EventHandler<PositionEventArgs> NetworkCalculated;
        /// <summary>Event that fires as soon as the input data changed.</summary>
        public event EventHandler InputChanged;
        /// <summary>Event that fires as soon as the output data changed.</summary>
        public event EventHandler TrainingChanged;
        /// <summary>Event that fires as soon as the network is rebuilt.</summary>
        public event EventHandler NetworkRebuilt;

        #region Construction & Building
        /// <summary>
        /// Instanciate a new pattern matching building block.
        /// </summary>
        /// <param name="numberOfInputs">The count of input neurons.</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
        public Controler(int numberOfInputs, int numberOfClasses)
        {
            ConfigNode.AttachRootNodeToHost(this);
            _patterns = new PatternSet(numberOfClasses);
            _outputTraining = new double[numberOfClasses];
            _inputPattern = new double[numberOfInputs];
            //BuildNetwork(true);
        }

        /// <summary>
        /// Instanciate a new pattern matching building block.
        /// </summary>
        /// <param name="numberOfInputs">The count of input neurons.</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
        /// <param name="config">The configuration instance (to replace the default).</param>
        public Controler(int numberOfInputs, int numberOfClasses, ConfigNode config)
        {
            config.AttatchToHost(this);
            _patterns = new PatternSet(numberOfClasses);
            _outputTraining = new double[numberOfClasses];
            _inputPattern = new double[numberOfInputs];
            //BuildNetwork(true);
        }

        /// <summary>
        /// Instanciate a new pattern matching building block.
        /// </summary>
        /// <param name="numberOfInputs">The count of input neurons.</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
        /// <param name="builder">A network structure builder (to replace the default).</param>
        public Controler(int numberOfInputs, int numberOfClasses, INetworkStructureBuilder builder)
        {
            ConfigNode.AttachRootNodeToHost(this);
            _patterns = new PatternSet(numberOfClasses);
            _outputTraining = new double[numberOfClasses];
            _inputPattern = new double[numberOfInputs];
            _builder = builder;
            //BuildNetwork(true);
        }

        /// <summary>
        /// Instanciate a new pattern matching building block.
        /// </summary>
        /// <param name="numberOfInputs">The count of input neurons.</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
        /// <param name="config">The configuration instance (to replace the default).</param>
        /// <param name="builder">A network structure builder (to replace the default).</param>
        public Controler(int numberOfInputs, int numberOfClasses, ConfigNode config, INetworkStructureBuilder builder)
        {
            config.AttatchToHost(this);
            _patterns = new PatternSet(numberOfClasses);
            _outputTraining = new double[numberOfClasses];
            _inputPattern = new double[numberOfInputs];
            _builder = builder;
            //BuildNetwork(true);
        }

        /// <summary>
        /// Builds the Network. Call this method straight after the constructor.
        /// </summary>
        /// <param name="adaptConfiguration"></param>
        public void BuildNetwork(bool adaptConfiguration)
        {
            _network = new Network(_node);
            if(_builder == null)
                _builder = new DefaultStructureBuilder(NumberOfInputs, NumberOfClasses);
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
                _builder = new DefaultStructureBuilder(NumberOfInputs, NumberOfClasses);
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

        /// <summary>
        /// Access the network trainer agent.
        /// </summary>
        /// <remarks>
        /// This agent is responsible for a successful training the network.
        /// </remarks>
        public INetworkTrainer NetworkTrainer
        {
            get { return _trainer; }
            set { _trainer = value; }
        }

        /// <summary>
        /// Access the training configuration set.
        /// </summary>
        public TrainingConfig TrainingConfiguration
        {
            get { return _config; }
        }

        /// <summary>
        /// Access the basic network configuration set.
        /// </summary>
        public BasicConfig BasicConfiguration
        {
            get { return _config.Basic; }
        }

        /// <summary>
        /// Access the internal neural network structure.
        /// </summary>
        public Network NeuralNetwork
        {
            get { return _network; }
        }

        /// <summary>
        /// The active pattern collection.
        /// </summary>
        public PatternSet Patterns
        {
            get { return _patterns; }
        }

        public int PatternCount
        {
            get { return _patterns.Count; }
        }

        /// <summary>
        /// Access the count of input neurons.
        /// </summary>
        public int NumberOfInputs
        {
            get { return _inputPattern.Length; }
        }

        /// <summary>
        /// Access the count of output neurons.
        /// </summary>
        public int NumberOfClasses
        {
            get { return _outputTraining.Length; }
        }

        /// <summary>
        /// Sets names/titles of the output neurons.
        /// </summary>
        /// <param name="titles"></param>
        public void SetOutputTitles(params string[] titles)
        {
            _network.LastLayer.SetNeuronTitles(titles);
        }
        #endregion

        #region Pattern Selection
        /// <summary>
        /// Select a pattern.
        /// </summary>
        /// <param name="position">The index of the pattern.</param>
        public void SelectPattern(int position)
        {
            Pattern pattern = _patterns[position];
            pattern.SyncInputTo(_inputPattern, _config.Basic);
            pattern.SyncTrainingTo(_outputTraining, _config.Basic);
            if(!_config.Basic.QuietModeEnable.Value)
            {
                if(PatternSelectionChanged != null)
                    PatternSelectionChanged(this, new PatternPositionEventArgs(pattern, position));
                if(InputChanged != null)
                    InputChanged(this, EventArgs.Empty);
                if(TrainingChanged != null)
                    TrainingChanged(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Select a pattern and shuffle it (if shuffle is enabled).
        /// </summary>
        /// <param name="position">The index of the pattern.</param>
        public void SelectShuffledPattern(int position)
        {
            Pattern pattern = _patterns[position];
            pattern.SyncInputTo(_inputPattern, _config.Basic);
            if(_config.ShuffleEnable.Value)
                for(int i = 0; i < _inputPattern.Length; i++)
                    _inputPattern[i] = _config.ShuffleInputNoiseSwap(_inputPattern[i]);
            pattern.SyncTrainingTo(_outputTraining, _config.Basic);
            if(!_config.Basic.QuietModeEnable.Value)
            {
                if(PatternSelectionChanged != null)
                    PatternSelectionChanged(this, new PatternPositionEventArgs(pattern, position));
                if(InputChanged != null)
                    InputChanged(this, EventArgs.Empty);
                if(TrainingChanged != null)
                    TrainingChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Push input data to the input layer.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <remarks>
        /// Use this to push arbitrary data that is NOT stored in patterns.
        /// If you're working with patterns, use SelectPattern instead.
        /// Note that this method overwrites data in the bound vector.
        /// </remarks>
        public void PushInput(params double[] input)
        {
            input.CopyTo(_inputPattern, 0);
            if(!_config.Basic.QuietModeEnable.Value && InputChanged != null)
                InputChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Push input data to the input layer.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <remarks>
        /// Use this to push arbitrary data that is NOT stored in patterns.
        /// If you're working with patterns, use SelectPattern instead.
        /// Note that this method overwrites data in the bound vector.
        /// </remarks>
        public void PushInput(params bool[] input)
        {
            for(int i = 0; i < input.Length; i++)
                _inputPattern[i] = _config.Basic.ConvertInput(input[i]);
            if(!_config.Basic.QuietModeEnable.Value && InputChanged != null)
                InputChanged(this, EventArgs.Empty);
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
        public int CalculateCurrentNetwork()
        {
            _network.CalculateFeedforward();
            _currentClassification = _network.LastLayer.EvaluateBiggestOutputNeuron();
            if(!_config.Basic.QuietModeEnable.Value && NetworkCalculated != null)
                NetworkCalculated(this, new PositionEventArgs(_currentClassification));
            return _currentClassification;
        }

        /// <summary>
        /// Calculate/Propagate the current network/pattern.
        /// </summary>
        /// <returns>The position of the output neuron with the biggest output.</returns>
        public int CalculateCurrentNetwork(out double outputLeadMargin)
        {
            _network.CalculateFeedforward();
            _currentClassification = _network.LastLayer.EvaluateBiggestOutputNeuron(out outputLeadMargin);
            if(!_config.Basic.QuietModeEnable.Value && NetworkCalculated != null)
                NetworkCalculated(this, new PositionEventArgs(_currentClassification));
            return _currentClassification;
        }

        /// <summary>
        /// Count matching (successful trained) patterns.
        /// </summary>
        /// <returns>The number of matching patterns</returns>
        public int CountSuccessfulPatterns()
        {
            //backup input data
            double[] bkp = _inputPattern;
            _inputPattern = new double[bkp.Length];
            bkp.CopyTo(_inputPattern, 0);
            _network.FirstLayer.BindActivity(_inputPattern);
            //loop through patterns
            int c = 0;
            Pattern pattern = null;
            for(int i = 0; i < _patterns.Count; i++)
            {
                pattern = (Pattern)_patterns[i];
                pattern.SyncInputTo(_inputPattern, _config.Basic);
                _network.CalculateFeedforward();
                if(_network.LastLayer.EvaluateBiggestOutputNeuron() == pattern.Classification)
                    c++;
            }
            //restore input data and network.
            _inputPattern = bkp;
            _network.FirstLayer.BindActivity(_inputPattern);
            _network.CalculateFeedforward();
            return c;
        }
        #endregion

        #region Auto Training
        /// <summary>
        /// Train the network using a Network Trainer agent.
        /// </summary>
        /// <param name="progress">Delegate to update the progress (between 0 and 100).</param>
        /// <returns>Whether the training was successful or not.</returns>
        public bool AutoTrainNetwork(Progress progress)
        {
            int attempts = _config.AutoTrainingAttempts.Value;
            if(_trainer == null)
                _trainer = new FeedbackTrainer(); //SimpleTrainer(); //ConditionalTrainer();
            bool wasQuiet = _config.Basic.QuietModeEnable.Value;
            _config.Basic.QuietModeEnable.Value = true;
            bool succeeded = _trainer.Train(this, progress);
            while(!succeeded && --attempts > 0)
            {
                RebuildNetwork();
                succeeded = _trainer.Train(this, progress);
            }
            _config.Basic.QuietModeEnable.Value = wasQuiet;
            return succeeded;
        }

        /// <summary>
        /// Train the network using a Network Trainer agent.
        /// </summary>
        /// <param name="attempts">The count of training attemps (if predecessor fails). Default is 1 (if omitted).</param>
        /// <param name="progress">Delegate to update the progress (between 0 and 100).</param>
        /// <returns>Whether the training was successful or not.</returns>
        public bool AutoTrainNetwork(int attempts, Progress progress)
        {
            _config.AutoTrainingAttempts.Value = attempts;
            return AutoTrainNetwork(progress);
        }

        /// <summary>
        /// Train the network using a Network Trainer agent.
        /// </summary>
        /// <returns>Whether the training was successful or not.</returns>
        public bool AutoTrainNetwork()
        {
            return AutoTrainNetwork(null);
        }

        /// <summary>
        /// Train the network using a Network Trainer agent.
        /// </summary>
        /// <param name="attempts">The count of training attemps (if predecessor fails). Default is 1 (if omitted).</param>
        /// <returns>Whether the training was successful or not.</returns>
        public bool AutoTrainNetwork(int attempts)
        {
            _config.AutoTrainingAttempts.Value = attempts;
            return AutoTrainNetwork(null);
        }
        #endregion

        #region IConfigurable Members
        string IConfigurable.Name
        {
            get { return "PatternMatchingControler"; }
        }
        void IConfigurable.Rebind(ConfigNode node)
        {
            _node = node;
            if(_config == null)
                _config = new TrainingConfig(_node);
            else
                _config.Node = _node;
        }
        ConfigNode IConfigurable.Node
        {
            get { return _node; }
        }
        #endregion
    }
}
