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
    /// <summary>
    /// Uses the inner provider for the first enumeration and caches all samples. After that the inner provider is freed and the cache is used for all future enumerations.
    /// </summary>
    public class CachedSampleProvider : ISampleProvider
    {
        private ISampleProvider _innerProvider;
        private StaticSampleProvider _cache;

        public CachedSampleProvider(ISampleProvider innerProvider)
        {
            _innerProvider = innerProvider;
            _cache = new StaticSampleProvider();
        }

        public int Count
        {
            get
            {
                if(_innerProvider != null)
                    return _innerProvider.Count;
                else
                    return _cache.Count;
            }
        }

        public bool IsDeterministic
        {
            get { return true; }
        }

        public IEnumerator<Sample> GetEnumerator()
        {
            if(_innerProvider != null)
                return CacheInnerEnumerator();
            else
                return _cache.GetEnumerator();            
        }

        private IEnumerator<Sample> CacheInnerEnumerator()
        {
            foreach(Sample s in _innerProvider)
            {
                _cache.Add(s);
                yield return s;
            }
            _innerProvider = null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
