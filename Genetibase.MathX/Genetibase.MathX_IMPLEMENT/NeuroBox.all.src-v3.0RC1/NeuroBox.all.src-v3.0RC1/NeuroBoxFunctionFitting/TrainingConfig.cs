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
using MathNet.Numerics.Generators;

namespace NeuroBox.FunctionFitting
{
    public class TrainingConfig
    {
        private ConfigNode _config;
        private NormalGenerator _gaussian;
        private BasicConfig _basic;

        public TrainingConfig(ConfigNode config)
        {
            _config = config;
            _gaussian = new NormalGenerator(0.0, 0.5);
            _basic = new BasicConfig(config);

            _autoTrainingEpochs = new ConfigItem<int>(_config, "AutoTrainingEpochs", 400);
            _autoTrainingAttempts = new ConfigItem<int>(_config, "AutoTrainingAttempts", 1);
            _autoTrainingPercentSuccessful = new ConfigItem<double>(_config, "AutoTrainingPercentSuccessful", 1.0);

            _shuffleSwapProbability = new ConfigItem<double>(_config, "ShuffleSwapProbability", 0.05);
            _shuffleNoiseSigma = new ConfigItem<double>(_config, "ShuffleNoiseSigma", 0.5);
            _shuffleEn = new ConfigItem<bool>(_config, "ShuffleEnable", false);
        }

        public ConfigNode Node
        {
            get { return _config; }
            set
            {
                _config = value;
                _basic.Node = _config;
            }
        }

        public BasicConfig Basic
        {
            get { return _basic; }
        }

        private ConfigItem<int> _autoTrainingEpochs;
        public ConfigItem<int> AutoTrainingEpochs { get { return _autoTrainingEpochs; } }
        private ConfigItem<int> _autoTrainingAttempts;
        public ConfigItem<int> AutoTrainingAttempts { get { return _autoTrainingAttempts; } }
        private ConfigItem<double> _autoTrainingPercentSuccessful;
        public ConfigItem<double> AutoTrainingPercentSuccessful { get { return _autoTrainingPercentSuccessful; } }

        private ConfigItem<double> _shuffleSwapProbability;
        public ConfigItem<double> ShuffleSwapProbability { get { return _shuffleSwapProbability; } }
        private ConfigItem<double> _shuffleNoiseSigma;
        public ConfigItem<double> ShuffleNoiseSigma { get { return _shuffleNoiseSigma; } }
        private ConfigItem<bool> _shuffleEn;
        public ConfigItem<bool> ShuffleEnable { get { return _shuffleEn; } }

        public bool ShuffleInputSwap(bool data)
        {
            return Network.Random.NextDouble() < ShuffleSwapProbability.Value ? !data : data;
        }
        public double ShuffleInputNoise(double data)
        {
            _gaussian.Sigma = ShuffleNoiseSigma.Value;
            return data + _gaussian.Next();
        }
        public double ShuffleInputNoiseSwap(double data)
        {
            if(Network.Random.NextDouble() < ShuffleSwapProbability.Value)
                data = 0.5 * (_basic.LowInput.Value + _basic.HighInput.Value) - data;
            _gaussian.Sigma = ShuffleNoiseSigma.Value;
            return data + _gaussian.Next();
        }
        public double ShuffleInputNoiseSwap(bool data)
        {
            double ret;
            if(Network.Random.NextDouble() < ShuffleSwapProbability.Value)
                ret = data ? _basic.LowInput.Value : _basic.HighInput.Value;
            else
                ret = data ? _basic.HighInput.Value : _basic.LowInput.Value;
            _gaussian.Sigma = ShuffleNoiseSigma.Value;
            return ret + _gaussian.Next();
        }
    }
}
