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
    public class ConfigNode
    {
        private ConfigNode _parentNode;
        private IConfigurable _host;
        private Dictionary<string, object> _localConfig;

        public event EventHandler<ConfigChangedEventArgs> ConfigChanged;

        private void ParentNode_ConfigChanged(object sender, ConfigChangedEventArgs e)
        {
            if(ConfigChanged != null)
                ConfigChanged(sender, e);
        }

        public ConfigNode()
        {
            _parentNode = null;
            _localConfig = new Dictionary<string, object>(2);
        }
        private ConfigNode(ConfigNode parent)
        {
            _parentNode = parent;
            _localConfig = new Dictionary<string, object>(2);
            if(_parentNode != null)
                _parentNode.ConfigChanged += ParentNode_ConfigChanged;
        }

        public static void AttachRootNodeToHost(IConfigurable host)
        {
            ConfigNode cn = new ConfigNode();
            cn.AttatchToHost(host);
        }

        internal bool IsValueSet(string id)
        {
            return _localConfig.ContainsKey(id) || _parentNode != null && _parentNode.IsValueSet(id);
        }

        internal void ResetValue(string id)
        {
            if(_localConfig.ContainsKey(id))
            {
                _localConfig.Remove(id);
                if(ConfigChanged != null)
                    ConfigChanged(this, new ConfigChangedEventArgs(id, false));
            }
        }

        internal bool TryGetValue(string id, out object value)
        {
            if(_localConfig.TryGetValue(id, out value))
                return true;
            if(_parentNode != null)
                return _parentNode.TryGetValue(id, out value);
            value = null;
            return false;
        }

        internal object this[string id]
        {
            get
            {
                object v;
                if(_localConfig.TryGetValue(id, out v))
                    return v;
                else
                    return _parentNode[id];
            }
            set
            {
                if(value == null && _localConfig.ContainsKey(id))
                    _localConfig.Remove(id);
                else
                    _localConfig[id] = value;
                if(ConfigChanged != null)
                    ConfigChanged(this, new ConfigChangedEventArgs(id, false));
            }
        }

        public bool IsRoot
        {
            get { return _parentNode == null; }
        }
        public ConfigNode Parent
        {
            get { return _parentNode; }
        }
        
        public void AssignNewParent(ConfigNode newParent)
        {
            if(_parentNode != null)
                _parentNode.ConfigChanged -= ParentNode_ConfigChanged;
            _parentNode = newParent;
            if(_parentNode != null)
                _parentNode.ConfigChanged += ParentNode_ConfigChanged;
            if(ConfigChanged != null)
                ConfigChanged(this, new ConfigChangedEventArgs(string.Empty, true));
        }

        public void CopyParentToLocal()
        {
            if(_parentNode != null)
                foreach(KeyValuePair<string, object> property in _parentNode._localConfig)
                    if(!_localConfig.ContainsKey(property.Key))
                        _localConfig.Add(property.Key, property.Value);
        }

        public ConfigNode BuildChild()
        {
            return new ConfigNode(this);
        }
        public ConfigNode BuildChildForHost(IConfigurable host)
        {
            ConfigNode cn = new ConfigNode(this);
            cn.AttatchToHost(host);
            return cn;
        }

        public ConfigNode BuildSibling()
        {
            return new ConfigNode(_parentNode);
        }
        public ConfigNode BuildSiblingForHost(IConfigurable host)
        {
            ConfigNode cn = new ConfigNode(_parentNode);
            cn.AttatchToHost(host);
            return cn;
        }

        public IConfigurable Host
        {
            get { return _host; }
        }
        public string Name
        {
            get { return _host.Name; }
        }

        public void AttatchToHost(IConfigurable host)
        {
            _host = host;
            _host.Rebind(this);
        }
    }
}
