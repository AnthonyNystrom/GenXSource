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

namespace NeuroBox.PatternMatching.Grid2D
{
    /// <summary>
    /// A specialized pattern for grid based applications.
    /// </summary>
    [Serializable]
    public class DoubleGrid2DPattern : DoublePattern
    {
        private int _width;
        private int _height;

        /// <summary>
        /// Instanciates a new pattern using the given input - training association.
        /// </summary>
        /// <param name="title">The name/title of the pattern.</param>
        /// <param name="x">The width of the input grid.</param>
        /// <param name="y">The height of the input grid.</param>
        /// <param name="input">The input pattern data.</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
        /// <param name="classification">The output neuron associated with this pattern (zero based).</param>
        /// <param name="config">The network configuration instance (needed to generate an appropriate training vector)</param>
        /// <remarks>The input array needs to be of the same size as the product of x and y.</remarks>
        public DoubleGrid2DPattern(string title, int width, int height, double[] input, int numberOfClasses, int classification, BasicConfig config)
            : base(title, input, numberOfClasses, classification, config)
        {
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// The pattern grid width.
        /// </summary>
        public int PatternWidth
        {
            get { return _width; }
        }

        /// <summary>
        /// The pattern grid height.
        /// </summary>
        public int PatternHeight
        {
            get { return _height; }
        }
    }
}
