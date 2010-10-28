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
using NeuroBox;

namespace NeuroBox.PatternMatching
{
	/// <summary>
	/// A Pattern is a single double input-output association.
	/// You may use such patterns to traing a neural network to its association.
	/// </summary>
    [Serializable]
    public class DoublePattern : Pattern
    {
        private double[] _inputPattern = null;
        private double[] _outputTraining = null;

        /// <summary>
        /// Instanciates a new pattern using the given input - training association.
        /// </summary>
        /// <param name="title">The name/title of the pattern.</param>
        /// <param name="input">The input pattern data (gets copied).</param>
        /// <param name="training">The output training data (gets copied).</param>
        /// <param name="classification">The position of the favoured output neuron in the output layer.</param>
        public DoublePattern(string title, double[] input, double[] training, int classification)
            : base(title, classification)
        {
            _inputPattern = new double[input.Length];
            input.CopyTo(_inputPattern, 0);
            _outputTraining = new double[training.Length];
            training.CopyTo(_outputTraining, 0);
        }

        /// <summary>
        /// Instanciates a new pattern.
        /// </summary>
        /// <param name="title">The name/title of the pattern.</param>
        /// <param name="input">The input pattern data (gets copied).</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
        /// <param name="classification">The position of the favoured output neuron in the output layer.</param>
        /// <param name="config">The network configuration instance (needed to generate an appropriate training vector)</param>
        public DoublePattern(string title, double[] input, int numberOfClasses, int classification, BasicConfig config)
            : base(title, classification)
        {
            _inputPattern = new double[input.Length];
            input.CopyTo(_inputPattern, 0);
            _outputTraining = new double[numberOfClasses];
            for(int i = 0; i < numberOfClasses; i++)
                OutputTraining[i] = config.ConvertOutput(i == classification);
        }

        /// <summary>
        /// The input pattern data.
        /// </summary>
        public double[] InputPattern
        {
            get { return _inputPattern; }
        }

        /// <summary>
        /// The output training data.
        /// </summary>
        public double[] OutputTraining
        {
            get { return _outputTraining; }
        }

        #region Conversion
        /// <summary>
        /// Create a boolean pattern based on this pattern.
        /// </summary>
        /// <param name="inputThreshold">The input limit between 'true' and 'false'.</param>
        /// <param name="outputThreshold">The output limit between 'true' and 'false'.</param>
        public BooleanPattern ToBooleanPattern(double inputThreshold, double outputThreshold)
        {
            bool[] input = new bool[_inputPattern.Length];
            bool[] output = new bool[_outputTraining.Length];
            for(int i = 0; i < input.Length; i++)
                input[i] = _inputPattern[i] >= inputThreshold;
            for(int i = 0; i < output.Length; i++)
                output[i] = _outputTraining[i] >= outputThreshold;
            return new BooleanPattern(Title, input, output, Classification);
        }

        /// <summary>
        /// Create a boolean pattern based on this pattern.
        /// </summary>
        /// <param name="config">The configuration instance.</param>
        public BooleanPattern ToBooleanPattern(BasicConfig config)
        {
            bool[] input = new bool[_inputPattern.Length];
            bool[] output = new bool[_outputTraining.Length];
            for(int i = 0; i < input.Length; i++)
                input[i] = config.ConvertInput(_inputPattern[i]);
            for(int i = 0; i < output.Length; i++)
                output[i] = config.ConvertOutput(_outputTraining[i]);
            return new BooleanPattern(Title, input, output, Classification);
        }
        #endregion

        #region Basic Network Sync
        /// <summary>
        /// Copy input pattern data to an array.
        /// </summary>
        /// <param name="vector">The target array.</param>
        /// <param name="config">The configuration instance.</param>
        public override void SyncInputTo(double[] vector, BasicConfig config)
        {
            _inputPattern.CopyTo(vector, 0);
        }
        /// <summary>
        /// Copy output training data to an array.
        /// </summary>
        /// <param name="vector">The target array.</param>
        /// <param name="config">The configuration instance.</param>
        public override void SyncTrainingTo(double[] vector, BasicConfig config)
        {
            _outputTraining.CopyTo(vector, 0);
        }
        /// <summary>
        /// Copy input pattern data from an array.
        /// </summary>
        /// <param name="vector">The target array.</param>
        /// <param name="config">The configuration instance.</param>
        public override void SyncInputFrom(double[] vector, BasicConfig config)
        {
            vector.CopyTo(_inputPattern, 0);
        }
        /// <summary>
        /// Copy output training data from an array.
        /// </summary>
        /// <param name="vector">The target array.</param>
        /// <param name="config">The configuration instance.</param>
        public override void SyncTrainingFrom(double[] vector, BasicConfig config)
        {
            vector.CopyTo(_outputTraining, 0);
        }
        #endregion

        #region Direct Data Sync
        /// <summary>
        /// Read input pattern data from an array.
        /// </summary>
        /// <param name="bind">The source array.</param>
        public void CopyInputFrom(double[] bind)
        {
            bind.CopyTo(_inputPattern, 0);
        }
        /// <summary>
        /// Read output training data from an array.
        /// </summary>
        /// <param name="bind">The source array.</param>
        public void CopyTrainingFrom(double[] bind)
        {
            bind.CopyTo(_outputTraining, 0);
        }
        /// <summary>
        /// Copy input pattern data to an array.
        /// </summary>
        /// <param name="bind">The target array.</param>
        public void CopyInputTo(double[] bind)
        {
            _inputPattern.CopyTo(bind, 0);
        }
        /// <summary>
        /// Copy output training data to an array.
        /// </summary>
        /// <param name="bind">The target array.</param>
        public void CopyTrainingTo(double[] bind)
        {
            _outputTraining.CopyTo(bind, 0);
        }
        #endregion

    }
}