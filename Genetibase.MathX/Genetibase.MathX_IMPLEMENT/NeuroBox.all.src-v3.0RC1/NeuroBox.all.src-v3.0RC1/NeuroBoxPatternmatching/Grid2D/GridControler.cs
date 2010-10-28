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
using NeuroBox.PatternMatching;

namespace NeuroBox.PatternMatching.Grid2D
{
    /// <summary>
    /// Building Block for 2D Grid pattern matching scenarios. This is the class to work with.
    /// </summary>
    public class Grid2DControler : Controler
    {
        private int _width, _height;

        /// <summary>
        /// Instanciate a new 2D grid pattern matching building block.
        /// </summary>
        /// <param name="width">The input grid width.</param>
        /// <param name="height">The input grid height.</param>
        /// <param name="numberOfClasses">The count of possible outputs.</param>
        public Grid2DControler(int width, int height, int numberOfClasses)
            : base(width * height, numberOfClasses)
        {
            _width = width;
            _height = height;
            this.StructureBuilder = new DefaultGrid2DStructureBuilder(width, height, NumberOfClasses);
        }
        public Grid2DControler(int width, int height, int numberOfClasses, ConfigNode node)
            : base(width * height, numberOfClasses, node)
        {
            _width = width;
            _height = height;
            this.StructureBuilder = new DefaultGrid2DStructureBuilder(width, height, NumberOfClasses);
        }

        /// <summary>
        /// The Input Grid Width.
        /// </summary>
        public int PatternWidth
        {
            get { return _width; }
        }

        /// <summary>
        /// The Input Grid Height.
        /// </summary>
        public int PatternHeight
        {
            get { return _height; }
        }
    }
}
