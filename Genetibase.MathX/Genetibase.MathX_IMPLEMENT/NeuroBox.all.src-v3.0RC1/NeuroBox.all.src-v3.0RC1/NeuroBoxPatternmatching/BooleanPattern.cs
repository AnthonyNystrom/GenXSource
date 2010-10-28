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
    /// A Pattern is a single boolean input-output association.
    /// You may use such patterns to traing a neural network to its association.
    /// </summary>
    [Serializable]
    public class BooleanPattern : Pattern
    {
        private bool[] _inputPattern = null;
        private bool[] _outputTraining = null;

        /// <summary>
        /// Instanciates a new pattern using the given input - training association.
        /// </summary>
        /// <param name="title">The name/title of the pattern.</param>
        /// <param name="input">The input pattern data (gets copied).</param>
        /// <param name="training">The output training data (gets copied).</param>
        /// <param name="classification">The position of the favoured output neuron in the output layer.</param>
        public BooleanPattern(string title, bool[] input, bool[] training, int classification)
            : base(title, classification)
        {
            _inputPattern = new bool[input.Length];
            input.CopyTo(_inputPattern, 0);
            _outputTraining = new bool[training.Length];
            training.CopyTo(_outputTraining, 0);
        }

        /// <summary>
        /// Instanciates a new pattern using the given input - training association.
        /// </summary>
        /// <param name="title">The name/title of the pattern.</param>
        /// <param name="input">The input pattern data (gets copied).</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
        /// <param name="classification">The position of the favoured output neuron in the output layer.</param>
        public BooleanPattern(string title, bool[] input, int numberOfClasses, int classification)
            : base(title, classification)
        {
            _inputPattern = new bool[input.Length];
            input.CopyTo(_inputPattern, 0);
            _outputTraining = new bool[numberOfClasses];
            for(int i = 0; i < numberOfClasses; i++)
                OutputTraining[i] = (i == classification);
        }

        /// <summary>
        /// The input pattern data.
        /// </summary>
        public bool[] InputPattern
        {
            get { return _inputPattern; }
        }

        /// <summary>
        /// The output training data.
        /// </summary>
        public bool[] OutputTraining
        {
            get { return _outputTraining; }
        }

        #region Conversion
        /// <summary>
        /// Create a double pattern based on this pattern.
        /// </summary>
        /// <param name="inputLow">The input value for 'false'.</param>
        /// <param name="inputHigh">The input value for 'true'.</param>
        /// <param name="outputLow">The output value for 'false'.</param>
        /// <param name="outputHigh">The output value for 'true'.</param>
        public DoublePattern ToDoublePattern(double inputLow, double inputHigh, double outputLow, double outputHigh)
        {
            double[] input = new double[_inputPattern.Length];
            double[] output = new double[_outputTraining.Length];
            for(int i = 0; i < input.Length; i++)
                input[i] = _inputPattern[i] ? inputHigh : inputLow;
            for(int i = 0; i < output.Length; i++)
                output[i] = _outputTraining[i] ? outputHigh : outputLow;
            return new DoublePattern(Title, input, output, Classification);
        }

        /// <summary>
        /// Create a double pattern based on this pattern.
        /// </summary>
        /// <param name="config">The configuration instance.</param>
        public DoublePattern ToDoublePattern(BasicConfig config)
        {
            double[] input = new double[_inputPattern.Length];
            double[] output = new double[_outputTraining.Length];
            for(int i = 0; i < input.Length; i++)
                input[i] = config.ConvertInput(_inputPattern[i]);
            for(int i = 0; i < output.Length; i++)
                output[i] = config.ConvertOutput(_outputTraining[i]);
            return new DoublePattern(Title, input, output, Classification);
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
            for(int i = 0; i < _inputPattern.Length && i < vector.Length; i++)
                vector[i] = config.ConvertInput(_inputPattern[i]);
        }
        /// <summary>
        /// Copy output training data to an array.
        /// </summary>
        /// <param name="vector">The target array.</param>
        /// <param name="config">The configuration instance.</param>
        public override void SyncTrainingTo(double[] vector, BasicConfig config)
        {
            for(int i = 0; i < _outputTraining.Length && i < vector.Length; i++)
                vector[i] = config.ConvertOutput(_outputTraining[i]);
        }
        /// <summary>
        /// Copy input pattern data from an array.
        /// </summary>
        /// <param name="vector">The target array.</param>
        /// <param name="config">The configuration instance.</param>
        public override void SyncInputFrom(double[] vector, BasicConfig config)
        {
            for(int i = 0; i < _inputPattern.Length && i < vector.Length; i++)
                _inputPattern[i] = config.ConvertInput(vector[i]);
        }
        /// <summary>
        /// Copy output training data from an array.
        /// </summary>
        /// <param name="vector">The target array.</param>
        /// <param name="config">The configuration instance.</param>
        public override void SyncTrainingFrom(double[] vector, BasicConfig config)
        {
            for(int i = 0; i < _outputTraining.Length && i < vector.Length; i++)
                _outputTraining[i] = config.ConvertOutput(vector[i]);
        }
        #endregion

        #region Direct Data Sync
        /// <summary>
        /// Read input pattern data from an array.
        /// </summary>
        /// <param name="bind">The source array.</param>
        public void CopyInputFrom(bool[] bind)
        {
            bind.CopyTo(_inputPattern, 0);
        }
        /// <summary>
        /// Read output training data from an array.
        /// </summary>
        /// <param name="bind">The source array.</param>
        public void CopyTrainingFrom(bool[] bind)
        {
            bind.CopyTo(_outputTraining, 0);
        }
        /// <summary>
        /// Copy input pattern data to an array.
        /// </summary>
        /// <param name="bind">The target array.</param>
        public void CopyInputTo(bool[] bind)
        {
            _inputPattern.CopyTo(bind, 0);
        }
        /// <summary>
        /// Copy output training data to an array.
        /// </summary>
        /// <param name="bind">The target array.</param>
        public void CopyTrainingTo(bool[] bind)
        {
            _outputTraining.CopyTo(bind, 0);
        }
        #endregion

    }
}