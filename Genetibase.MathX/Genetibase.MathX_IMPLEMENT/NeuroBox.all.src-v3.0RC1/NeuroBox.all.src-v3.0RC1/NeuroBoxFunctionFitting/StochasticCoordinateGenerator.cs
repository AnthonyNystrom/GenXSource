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

using MathNet.Numerics;
using MathNet.Numerics.Generators;

namespace NeuroBox.FunctionFitting
{
    public class StochasticCoordinateGenerator : ICoordinateGenerator
    {
        private readonly IRealGenerator[] _generators;
        private readonly int _dimensions;
        private int _count = 50;

        public StochasticCoordinateGenerator(IRealGenerator[] distributions) : this(distributions, 50) { }
        public StochasticCoordinateGenerator(IRealGenerator[] distributions, int count)
        {
            _generators = distributions;
            _dimensions = distributions.Length;
            _count = count;
        }
        public StochasticCoordinateGenerator(double[] mean, double[] sigma) : this(mean, sigma, 50) { }
        public StochasticCoordinateGenerator(double[] mean, double[] sigma, int count)
        {
            if(mean.Length != sigma.Length)
                throw new ArgumentException("Mean and Sigma array length must be equal.", "sigma");
            _dimensions = mean.Length;
            _generators = new NormalGenerator[_dimensions];
            for(int i = 0; i < _dimensions; i++)
                _generators[i] = new NormalGenerator(mean[i], sigma[i]);
            _count = count;
        }
        public StochasticCoordinateGenerator(double mean, double sigma) : this(mean, sigma, 50) { }
        public StochasticCoordinateGenerator(double mean, double sigma, int count)
        {
            _dimensions = 1;
            _generators = new NormalGenerator[] { new NormalGenerator(mean, sigma) };
            _count = count;
        }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public bool IsDeterministic
        {
            get { return false; }
        }

        public IRealGenerator[] Distributions
        {
            get { return _generators; }
        }

        public IEnumerator<double[]> GetEnumerator()
        {
            for(int i = 0; i < _count; i++)
            {
                double[] coordinate = new double[_dimensions];
                for(int j = 0; j < _dimensions; j++)
                    coordinate[j] = _generators[j].Next();
                yield return coordinate;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
