using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NuGenSVisualLib.Settings
{
    public abstract class PropertyTree
    {
        Dictionary<string, bool> changes;
        Dictionary<string, IPropertyTreeNodeClass> nodes;

        public PropertyTree()
        {
            changes = new Dictionary<string, bool>();
        }

        public bool CheckChange(string path)
        {
            bool value;
            if (!changes.TryGetValue(path, out value))
                return false;
            return value;
        }

        public bool CheckChange(string node, string item)
        {
            bool value;
            // test block
            if (changes.TryGetValue(node + "*", out value) && value)
                return true;
            // test full path
            changes.TryGetValue(node + item, out value);
            return value;
        }

        public void RebuildNodes()
        {
            Type thisType = this.GetType();
            PropertyInfo[] properties = thisType.GetProperties();
            nodes = new Dictionary<string, IPropertyTreeNodeClass>();
            for (int p = 0; p < properties.Length - 1; p++)
            {
                object pVal = properties[p].GetValue(this, null);
                if (pVal is IPropertyTreeNodeClass)
                    nodes.Add(properties[p].Name, (IPropertyTreeNodeClass)pVal);
            }
        }

        public void UpdateLeafNode(IPropertyTreeNodeClass cValue, IPropertyTreeNodeClass newValue)
        {
            if (cValue == null || nodes == null)
                return;
            // find property name
            Type thisType = this.GetType();
            PropertyInfo[] properties = thisType.GetProperties();
            for (int p = 0; p < properties.Length - 1; p++)
            {
                if (properties[p].GetValue(this, null) == cValue)
                {
                    // update node value
                    nodes[properties[p].Name] = newValue;
                    break;
                }
            }
        }

        public void CompareAgainst(PropertyTree previous, bool updateValues)
        {
            if (previous.GetType() != this.GetType())
                return;

            if (nodes == null)
                RebuildNodes();
            if (previous.nodes == null)
                previous.RebuildNodes();

            //changes.Clear();

            // compare all property nodes for changes
            foreach (KeyValuePair<string, IPropertyTreeNodeClass> keyPair in nodes)
            {
                // see if node itself has changed
                if (keyPair.Value.GetType() != previous.nodes[keyPair.Key].GetType())
                {
                    changes[keyPair.Key + ".*"] = true;
                    changes[keyPair.Key + "."] = true;
                }
                else
                {
                    changes[keyPair.Key + ".*"] = false;

                    // compare all properties
                    PropertyInfo[] properties = keyPair.Value.GetType().GetProperties();
                    string nodeName = keyPair.Key;
                    int numNodeChanges = 0;
                    foreach (PropertyInfo property in properties)
                    {
                        object v1 = property.GetValue(keyPair.Value, null);
                        object v2 = property.GetValue(previous.nodes[keyPair.Key], null);

                        string nspace = nodeName + "." + property.Name;

                        if (v1 is IComparable)
                        {
                            if (((IComparable)v1).CompareTo(v2) != 0)
                            {
                                // add change
                                changes[nspace] = true;
                                numNodeChanges++;
                                if (updateValues)
                                    property.SetValue(keyPair.Value, v2, null);
                            }
                            else if (changes.ContainsKey(nspace))
                                changes[nspace] = false;
                        }
                        else if (v1 != v2)
                        {
                            if (v1 == null || !v1.Equals(v2))
                            {
                                // add change
                                changes[nspace] = true;
                                numNodeChanges++;
                                if (updateValues)
                                    property.SetValue(keyPair.Value, v2, null);
                            }
                            else if (changes.ContainsKey(nspace))
                                changes[nspace] = false;
                        }
                    }

                    // add branch change if needed
                    if (numNodeChanges > 0)
                        changes[nodeName + "."] = true;
                    else
                        changes[nodeName + "."] = false;
                }
            }
        }

        public bool this[string value]
        {
            get
            {
                bool result = false;
                changes.TryGetValue(value, out result);
                return result;
            }
        }
    }

    public interface IPropertyTreeNodeClass
    { }
}