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
using System.Diagnostics;

namespace NeuroBox.FunctionFitting
{
    public class RegularCoordinateGenerator : ICoordinateGenerator
    {
        private readonly int _dimensions;
        private double[] _min, _max;
        private int[] _numOfSamples;
        private int _totalNumOfSamples;
        private double[] _step;

        public RegularCoordinateGenerator(int dimensions, double[] min, double[] max, int[] numOfSamples)
        {
            if(min.Length != dimensions)
                throw new ArgumentException("Dimension must match the dimensions-parameter", "min");
            if(max.Length != dimensions)
                throw new ArgumentException("Dimension must match the dimensions-parameter", "max");
            if(numOfSamples.Length != dimensions)
                throw new ArgumentException("Dimension must match the dimensions-parameter", "max");

            _dimensions = dimensions;
            _min = min;
            _max = max;
            _numOfSamples = numOfSamples;
            _step = new double[dimensions];
            UpdateStep();
            UpdateTotalCount();
        }
        public RegularCoordinateGenerator(double min, double max, int numOfSamples)
        {
            _dimensions = 1;
            _min = new double[] { min };
            _max = new double[] { max };
            _numOfSamples = new int[] { numOfSamples };
            _step = new double[1];
            UpdateStep();
            UpdateTotalCount();
        }

        private void UpdateStep(int dimension)
        {
            if(_numOfSamples[dimension] < 2)
                _step[dimension] = _max[dimension] - _min[dimension];
            else
                _step[dimension] = (_max[dimension] - _min[dimension]) / (_numOfSamples[dimension]-1);
        }
        private void UpdateStep()
        {
            for(int dimension = 0; dimension < _dimensions; dimension++)
            {
                if(_numOfSamples[dimension] < 2)
                    _step[dimension] = _max[dimension] - _min[dimension];
                else
                    _step[dimension] = (_max[dimension] - _min[dimension]) / (_numOfSamples[dimension] - 1);
            }
        }
        private void UpdateTotalCount()
        {
            int cnt = 1;
            for(int dimension = 0; dimension < _dimensions; dimension++)
                cnt *= _numOfSamples[dimension];
            _totalNumOfSamples = cnt;
        }

        public int GetNumOfSamples(int dimension)
        {
            return _numOfSamples[dimension];
        }
        public double GetMin(int dimension)
        {
            return _min[dimension];
        }
        public double GetMax(int dimension)
        {
            return _max[dimension];
        }
        public void SetNumOfSamples(int dimension, int num)
        {
            _numOfSamples[dimension] = num;
            UpdateStep(dimension);
            UpdateTotalCount();
        }
        public void SetMin(int dimension, double min)
        {
            _min[dimension] = min;
            UpdateStep(dimension);
        }
        public void SetMax(int dimension, double max)
        {
            _max[dimension] = max;
            UpdateStep(dimension);
        }

        public int Count
        {
            get { return _totalNumOfSamples; }
        }

        public bool IsDeterministic
        {
            get { return true; }
        }

        private bool Increment(int[] indices, double[] coordinate)
        {
            for(int i = 0; i < _dimensions; i++)
            {
                if(indices[i] < _numOfSamples[i] - 1)
                {
                    indices[i]++;
                    coordinate[i] += _step[i];
                    return true;
                }
                if(i == _dimensions - 1)
                    return false;
                indices[i] = 0;
                coordinate[i] = _min[i];
            }
            Debug.Fail("Algorithm Error!");
            return false;
        }

        public IEnumerator<double[]> GetEnumerator()
        {
            int[] indices = new int[_dimensions];
            double[] coordinate = new double[_dimensions];

            for(int dimension = 0;dimension < _dimensions;dimension++)
                coordinate[dimension] = _min[dimension];

            do yield return coordinate;
            while(Increment(indices, coordinate));
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
