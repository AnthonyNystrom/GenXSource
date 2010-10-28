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

namespace NeuroBox.FunctionFitting
{
    public class DynamicSampleProvider : ISampleProvider
    {
        private TargetFunction _funct;
        private ICoordinateGenerator _generator;

        public DynamicSampleProvider(TargetFunction funct, ICoordinateGenerator generator)
        {
            _funct = funct;
            _generator = generator;
        }

        public TargetFunction TargetFunction
        {
            get { return _funct; }
            set { _funct = value; }
        }

        public ICoordinateGenerator Generator
        {
            get { return _generator; }
            set { _generator = value; }
        }

        public int Count
        {
            get { return _generator.Count; }
        }

        public bool IsDeterministic
        {
            get { return _generator.IsDeterministic; }
        }

        public IEnumerator<Sample> GetEnumerator()
        {
            foreach(double[] coordinate in _generator)
                yield return new Sample(coordinate, _funct(coordinate));
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
