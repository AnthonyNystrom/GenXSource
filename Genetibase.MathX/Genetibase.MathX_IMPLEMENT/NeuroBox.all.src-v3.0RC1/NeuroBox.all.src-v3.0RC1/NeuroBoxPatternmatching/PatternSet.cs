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
using System.Collections.ObjectModel;
using System.Text;

namespace NeuroBox.PatternMatching
{
    public class PatternSet : Collection<Pattern>
    {
        private int _numberOfClasses;

        public event EventHandler<PatternEventArgs> PatternAdded;
        public event EventHandler<PatternEventArgs> PatternRemoved;

        public PatternSet(int numberOfClasses)
        {
            _numberOfClasses = numberOfClasses;
        }

        public int NumberOfClasses
        {
            get { return _numberOfClasses; }
            set { _numberOfClasses = value; }
        }

        public void AddRange(IEnumerable<Pattern> patterns)
        {
            foreach(Pattern p in patterns)
                Add(p);
        }

        public void Add(string title, int classification, params bool[] input)
        {
            BooleanPattern pattern = new BooleanPattern(title, input, _numberOfClasses, classification);
            Add(pattern);
        }

        public void Add(string title, int classification, BasicConfig config, params double[] input)
        {
            DoublePattern pattern = new DoublePattern(title, input, _numberOfClasses, classification, config);
            Add(pattern);
        }

        protected override void InsertItem(int index, Pattern item)
        {
            base.InsertItem(index, item);
            if(PatternAdded != null)
                PatternAdded(this, new PatternEventArgs(item));
        }
        protected override void RemoveItem(int index)
        {
            if(PatternRemoved != null)
                PatternRemoved(this, new PatternEventArgs(this[index]));
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, Pattern item)
        {
            if(PatternRemoved != null)
                PatternRemoved(this, new PatternEventArgs(this[index]));
            base.SetItem(index, item);
            if(PatternAdded != null)
                PatternAdded(this, new PatternEventArgs(item));
        }
    }
}
