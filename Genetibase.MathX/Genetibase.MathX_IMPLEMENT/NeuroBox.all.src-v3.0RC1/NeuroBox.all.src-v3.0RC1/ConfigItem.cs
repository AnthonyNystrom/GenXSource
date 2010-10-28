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

namespace NeuroBox
{
    public class ConfigItem<T> //where T : IEquatable<T>
    {
        private ConfigNode _node;
        private readonly T _defaultValue;
        private T _cache;
        private readonly string _id;
        private bool _override;

        public ConfigItem(ConfigNode node, string id)
        {
            _node = node;
            _defaultValue = default(T);
            _id = id;
            _override = false;
            _node.ConfigChanged += Node_ConfigChanged;
            _cache = DirectValue;
        }

        public ConfigItem(ConfigNode node, string id, T defaultValue)
        {
            _node = node;
            _defaultValue = defaultValue;
            _id = id;
            _override = false;
            _node.ConfigChanged += Node_ConfigChanged;
            _cache = DirectValue;
        }

        void Node_ConfigChanged(object sender, ConfigChangedEventArgs e)
        {
            if(e.Id == _id || e.All)
                _cache = DirectValue;
        }

        public bool Override
        {
            get { return _override; }
            set
            {
                if(value == false && _override == true)
                    _node.ResetValue(_id);
                _override = value;
            }
        }

        public T DirectValue
        {
            get
            {
                object value;
                if(_node.TryGetValue(_id, out value))
                    return (T)value;
                else
                    return _defaultValue;
            }
        }

        public T Value
        {
            get { return _cache; }
            set
            {
                _override = true;
                _node[_id] = value;
            }
        }

        public T DefaultValue
        {
            get { return _defaultValue; }
        }

        public void Reset()
        {
            Override = false;
        }

        /// <summary>
        /// Resets the value or explicitly sets it to the default value if a parent overrides the property.
        /// </summary>
        public void SetToDefaultValue()
        {
            if(_node.IsValueSet(_id))
            {
                if(!GenericEquals(_node[_id],_defaultValue))
                    Value = _defaultValue;
                else
                {
                    Override = false;
                    if(!GenericEquals(Value,_defaultValue)) // ensure no parrent did override
                        Value = _defaultValue;
                }
            }
        }

        protected bool GenericEquals(object obj, T value)
        {
            IEquatable<T> obj_eq = obj as IEquatable<T>;
            if(obj_eq != null)
                return obj_eq.Equals(value);

            IComparable obj_cp = obj as IComparable;
            if(obj_cp != null)
                return obj_cp.CompareTo(value) == 0;

            return obj == (object)value;
        }
    }
}
