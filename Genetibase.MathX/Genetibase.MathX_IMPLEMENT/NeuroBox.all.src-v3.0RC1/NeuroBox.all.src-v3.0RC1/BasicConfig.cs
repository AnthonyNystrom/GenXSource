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

namespace NeuroBox
{
    public class BasicConfig
    {
        private ConfigNode _config;

        public BasicConfig(ConfigNode config)
        {
            _config = config;

            _learningRate = new ConfigItem<double>(_config, "LearningRate", 0.2);

            _biasNeuronOutput = new ConfigItem<double>(_config, "BiasNeuronOutput", 0.90);
            _biasNeuronEn = new ConfigItem<bool>(_config, "BiasNeuronEnable", true);

            _weightDecay = new ConfigItem<double>(_config, "WeightDecay", 0.01);
            _weightDecayEn = new ConfigItem<bool>(_config, "WeightDecayEnable", true);

            _flatspotElimination = new ConfigItem<double>(_config, "FlatspotElimination", 0.05);
            _flatspotEliminationEn = new ConfigItem<bool>(_config, "FlatspotEliminationEnable", true);

            _momentumTerm = new ConfigItem<double>(_config, "MomentumTerm", 0.6); 
            _momentumTermEn = new ConfigItem<bool>(_config, "MomentumTermEnable", false);

            _manhattanTrainingEn = new ConfigItem<bool>(_config, "ManhattanTrainingEnable", false);

            _symmetryPrevention = new ConfigItem<double>(_config, "SymmetryPrevention", 0.05);
            _symmetryPreventionEn = new ConfigItem<bool>(_config, "SymmetryPreventionEnable", false);

            _initialSymmetryBreaking = new ConfigItem<double>(_config, "InitialSymmetryBreaking", 0.2);

            _deadNeuronDecayEn = new ConfigItem<bool>(_config, "DeadNeuronDecayEnabled", false);

            _activationType = new ConfigItem<EActivationType>(_config, "ActivationType", EActivationType.Symmetric);

            _activationThreshold = new ConfigItem<double>(_config, "ActivationThreshold", 0.0);

            _activationTemperature = new ConfigItem<double>(_config, "ActivationTemperature", 1.0);

            _activationFunction = new ConfigItem<ActivationFunction>(_config, "ActivationFunction", null);
            _activationFunctionDerivative = new ConfigItem<ActivationFunctionDerivative>(_config, "ActivationFunctionDerivative", null);

            _lowInput = new ConfigItem<double>(_config, "LowInput", -3.0);
            _highInput = new ConfigItem<double>(_config, "HighInput", 3.0);
            _lowOutput = new ConfigItem<double>(_config, "LowOutput", -1.0);
            _highOutput = new ConfigItem<double>(_config, "HighOutput", 1.0);

            _quietModeEn = new ConfigItem<bool>(_config, "QuietModeEnabled", false);
        }

        public ConfigNode Node
        {
            get { return _config; }
            set { _config = value; }
        }

        private ConfigItem<double> _learningRate;
        /// <summary>Learn Rate factor.</summary>
        public ConfigItem<double> LearningRate { get { return _learningRate; } }

        private ConfigItem<double> _biasNeuronOutput;
        /// <summary>Constant output of the network wide on-neuron.</summary>
        public ConfigItem<double> BiasNeuronOutput { get { return _biasNeuronOutput; } }
        private ConfigItem<bool> _biasNeuronEn;
        /// <summary>Whether to use a network wide bias neuron to break (input) layer symmetry.</summary>
        public ConfigItem<bool> BiasNeuronEnable { get { return _biasNeuronEn; } }

        private ConfigItem<double> _weightDecay;
        /// <summary>Weight Decay factor.</summary>
        /// <remarks>Should be between 0.005 and 0.03</remarks>
        public ConfigItem<double> WeightDecay { get { return _weightDecay; } }
        private ConfigItem<bool> _weightDecayEn;
        /// <summary>Whether to use weight decay optimization.</summary>
        /// <remarks>
        /// Weight Decay tries to reduce the problem of too big weights.
        /// </remarks>
        public ConfigItem<bool> WeightDecayEnable { get { return _weightDecayEn; } }

        private ConfigItem<double> _flatspotElimination;
        /// <summary>Differential shift value for flatspot elimination.</summary>
        /// <remarks>Usually small, about 0.1 (depending on the activation function)</remarks>
        public ConfigItem<double> FlatspotElimination { get { return _flatspotElimination; } }
        private ConfigItem<bool> _flatspotEliminationEn;
        /// <summary>Whether to use flat spot elimination.</summary>
        /// <remarks>
        /// Flatspot elimination adds a small value to the
        /// activation function derivative to allow learing even if the neurons
        /// are on a flat spot (big absolute activation).
        /// </remarks>
        public ConfigItem<bool> FlatspotEliminationEnable { get { return _flatspotEliminationEn; } }

        private ConfigItem<double> _momentumTerm;
        /// <summary>Alpha factor for momentum term optimization.</summary>
        /// <remarks>Should be between 0.2 and 0.99, better between 0.6 and 0.9</remarks>
        public ConfigItem<double> MomentumTerm { get { return _momentumTerm; } }
        private ConfigItem<bool> _momentumTermEn;
        /// <summary>Whether to use momentum term optimization.</summary>
        /// <remarks>
        /// MomentumTerm Optimization speeds up weight modification on plateaus and
        /// slows it down in cleft steeps.
        /// </remarks>
        public ConfigItem<bool> MomentumTermEnable { get { return _momentumTermEn; } }

        private ConfigItem<bool> _manhattanTrainingEn;
        /// <summary>Whether to use manhattan training.</summary>
        /// <remarks>
        /// Manhattan training ignores the absolute of the error
        /// function and works with its sign only.
        /// </remarks>
        public ConfigItem<bool> ManhattanTrainingEnable { get { return _manhattanTrainingEn; } }

        private ConfigItem<double> _symmetryPrevention;
        /// <summary>Symmetry Prevention Factor.</summary>
        public ConfigItem<double> SymmetryPrevention { get { return _symmetryPrevention; } }
        private ConfigItem<bool> _symmetryPreventionEn;
        /// <summary>Whether to use preventive symmetry breaking.</summary>
        public ConfigItem<bool> SymmetryPreventionEnable { get { return _symmetryPreventionEn; } }

        private ConfigItem<double> _initialSymmetryBreaking;
        /// <summary>Initial Symmetry Breaking Factor for Synapse Weight Initialization.</summary>
        public ConfigItem<double> InitialSymmetryBreaking { get { return _initialSymmetryBreaking; } }

        private ConfigItem<bool> _deadNeuronDecayEn;
        /// <summary>Whether to attenuate dead neurons.</summary>
        public ConfigItem<bool> DeadNeuronDecayEnabled { get { return _deadNeuronDecayEn; } }

        private ConfigItem<EActivationType> _activationType;
        /// <summary>The activation function type</summary>
        public ConfigItem<EActivationType> ActivationType { get { return _activationType; } }

        private ConfigItem<double> _activationThreshold;
        /// <summary>Threshold for a binary activation function with type EActivationType.Threshold.</summary>
        public ConfigItem<double> ActivationThreshold { get { return _activationThreshold; } }

        private ConfigItem<double> _activationTemperature;
        /// <summary>Temperature factor of a logistic activation function with type EActivationType.Asymmetric.</summary>
        public ConfigItem<double> ActivationTemperature { get { return _activationTemperature; } }

        private ConfigItem<ActivationFunction> _activationFunction;
        /// <summary>User defined activation function with type EActivationType.Userdefined.</summary>
        public ConfigItem<ActivationFunction> ActivationFunction { get { return _activationFunction; } }
        private ConfigItem<ActivationFunctionDerivative> _activationFunctionDerivative;
        /// <summary>Derivative of a the user defined activation function.</summary>
        public ConfigItem<ActivationFunctionDerivative> ActivationFunctionDerivative { get { return _activationFunctionDerivative; } }

        private ConfigItem<double> _lowInput;
        /// <summary>Low Input for boolean "false".</summary>
        public ConfigItem<double> LowInput { get { return _lowInput; } }
        private ConfigItem<double> _highInput;
        /// <summary>High Input for boolean "true".</summary>
        public ConfigItem<double> HighInput { get { return _highInput; } }
        private ConfigItem<double> _lowOutput;
        /// <summary>Low Output for boolean "false".</summary>
        public ConfigItem<double> LowOutput { get { return _lowOutput; } }
        private ConfigItem<double> _highOutput;
        /// <summary>High Output for boolean "true".</summary>
        public ConfigItem<double> HighOutput { get { return _highOutput; } }

        private ConfigItem<bool> _quietModeEn;
        /// <summary>Whether to throw events or be quiet.</summary>
        public ConfigItem<bool> QuietModeEnable { get { return _quietModeEn; } }

        public double Activate(double activity)
        {
            EActivationType at = ActivationType.Value;
            switch(at)
            {
                case EActivationType.Symmetric:
                    return Math.Tanh(activity);
                case EActivationType.Asymmetric:
                    return 1.0 / (1.0 + Math.Exp(-activity / ActivationTemperature.Value));
                case EActivationType.Threshold:
                    return activity >= ActivationThreshold.Value ? HighOutput.Value : LowOutput.Value;
                case EActivationType.Userdefined:
                    return ActivationFunction.Value(activity);
                default:  //case EActivationType.Linear:
                    return activity;
            }
        }

        public double ActivateDerivative(double activity, double output)
        {
            double fe = FlatspotEliminationEnable.Value ? FlatspotElimination.Value : 0;
            EActivationType at = ActivationType.Value;
            switch(at)
            {
                case EActivationType.Symmetric:
                    return fe + 1 - output * output;
                case EActivationType.Asymmetric:
                    return fe + output * (1 - output);
                case EActivationType.Threshold:
                    return activity == ActivationThreshold.Value ? double.PositiveInfinity : fe;  //TODO: hardly useful ...
                case EActivationType.Userdefined:
                    return fe + ActivationFunctionDerivative.Value(activity, output);
                default:  //case EActivationType.Linear:
                    return 1;
            }
        }

        /// <summary>
        /// Convert a boolean input to a double input according to the current configuration.
        /// </summary>
        /// <param name="data">The boolean input activity</param>
        /// <returns>The double input activity</returns>
        public double ConvertInput(bool data)
        {
            return data ? HighInput.Value : LowInput.Value;
        }

        /// <summary>
        /// Convert a double input to a boolean input according to the current configuration.
        /// </summary>
        /// <param name="data">The double input activity</param>
        /// <returns>The boolean input activity</returns>
        public bool ConvertInput(double data)
        {
            return 2 * data >= HighInput.Value + LowInput.Value;
        }

        /// <summary>
        /// Convert a boolean output to a double output according to the current configuration.
        /// </summary>
        /// <param name="data">The boolean output training</param>
        /// <returns>The double output training</returns>
        public double ConvertOutput(bool data)
        {
            return data ? HighOutput.Value : LowOutput.Value;
        }

        /// <summary>
        /// Convert a double output to a boolean output according to the current configuration.
        /// </summary>
        /// <param name="data">The double output training</param>
        /// <returns>The boolean output training</returns>
        public bool ConvertOutput(double data)
        {
            return 2 * data >= HighOutput.Value + LowOutput.Value;
        }

        /// <summary>
        /// Evaluates a synapse weight initial value using initial symmetry braking if enabled.
        /// </summary>
        /// <returns>A weight to initialize a synapse.</returns>
        public double RandomInitialWeight()
        {
            if(ActivationType.Value == EActivationType.Asymmetric)
                return Network.Random.NextDouble() * InitialSymmetryBreaking.Value;
            else
                return (Network.Random.NextDouble() * 2d - 1d) * InitialSymmetryBreaking.Value;
        }
        /// <summary>
        /// Evaluates a weight summand to prevent symapse weight symmetry.
        /// </summary>
        /// <returns>A summand to add to the actual weight.</returns>
        public double PreventWeightSymmetrySummand()
        {
            if(!SymmetryPreventionEnable.Value)
                return 0d;
            if(ActivationType.Value == EActivationType.Asymmetric)
                return Network.Random.NextDouble() * SymmetryPrevention.Value;
            else
                return (Network.Random.NextDouble() * 2d - 1d) * SymmetryPrevention.Value;
        }
    }

    public enum EActivationType : int
    {
        Symmetric = 0,
        Asymmetric = 1,
        Linear = 2,
        Threshold = 3,
        Userdefined = 64
    }

    public delegate double ActivationFunction(double activity);
    public delegate double ActivationFunctionDerivative(double activity, double output);
}
