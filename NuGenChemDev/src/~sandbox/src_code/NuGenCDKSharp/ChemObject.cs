/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6672 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Events;
using Support;

namespace Org.OpenScience.CDK
{
    /// <summary>  The base class for all chemical objects in this cdk. It provides methods for
    /// adding listeners and for their notification of events, as well a a hash
    /// table for administration of physical or chemical properties
    /// 
    /// </summary>
    /// <author>         steinbeck
    /// </author>
    /// <cdk.module>     data </cdk.module>
    [Serializable]
    public class ChemObject : IChemObject
    {
        /// <summary>  Returns the number of ChemObjectListeners registered with this object.
        /// 
        /// </summary>
        /// <returns>    the number of registered listeners.
        /// </returns>
        virtual public int ListenerCount
        {
            get
            {
                if (chemObjectListeners == null)
                    return 0;
                return lazyChemObjectListeners().Count;
            }

        }
        
        /// <summary>  Returns a Map with the IChemObject's properties.
        /// 
        /// </summary>
        /// <returns>    The object's properties as an Hashtable
        /// </returns>
        /// <seealso cref="setProperties">
        /// </seealso>
        /// <summary>  Sets the properties of this object.
        /// 
        /// </summary>
        /// <param name="properties"> a Hashtable specifying the property values
        /// </param>
        /// <seealso cref="getProperties">
        /// </seealso>
        virtual public System.Collections.Hashtable Properties
        {
            get
            {
                return lazyProperties();
            }

            set
            {
                System.Collections.IEnumerator keys = value.Keys.GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                while (keys.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                    System.Object key = keys.Current;
                    lazyProperties()[key] = value[key];
                }
                notifyChanged();
            }

        }
        
        /// <summary>  Returns the identifier (ID) of this object.
        /// 
        /// </summary>
        /// <returns>    a String representing the ID value
        /// </returns>
        /// <seealso cref="setID">
        /// </seealso>
        /// <summary>  Sets the identifier (ID) of this object.
        /// 
        /// </summary>
        /// <param name="identifier"> a String representing the ID value
        /// </param>
        /// <seealso cref="getID">
        /// </seealso>
        virtual public System.String ID
        {
            get
            {
                return this.identifier;
            }

            set
            {
                this.identifier = value;
                notifyChanged();
            }

        }
        
        /// <summary> Returns the whole set of flags.
        /// 
        /// </summary>
        /// <returns>    the flags.
        /// </returns>
        /// <seealso cref="setFlags">
        /// </seealso>
        /// <summary> Sets the whole set of flags.
        /// 
        /// </summary>
        /// <param name="flagsNew">   the new flags.
        /// </param>
        /// <seealso cref="getFlags">
        /// </seealso>
        virtual public bool[] Flags
        {
            get
            {
                return (flags);
            }

            set
            {
                flags = value;
            }

        }
        virtual public IChemObjectBuilder Builder
        {
            get
            {
                return DefaultChemObjectBuilder.Instance;
            }

        }
        virtual public bool Notification
        {
            get
            {
                return this.doNotification;
            }

            set
            {
                this.doNotification = value;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 2798134548764323328L;

        /// <summary> List for listener administration.</summary>
        private System.Collections.IList chemObjectListeners;
        /// <summary>  A hashtable for the storage of any kind of properties of this IChemObject.</summary>
        private System.Collections.Hashtable properties;
        /// <summary>  You will frequently have to use some flags on a IChemObject. For example, if
        /// you want to draw a molecule and see if you've already drawn an atom, or in
        /// a ring search to check whether a vertex has been visited in a graph
        /// traversal. Use these flags while addressing particular positions in the
        /// flag array with self-defined constants (flags[VISITED] = true). 100 flags
        /// per object should be more than enough.
        /// </summary>
        private bool[] flags;

        /// <summary>  The ID is null by default.</summary>
        private System.String identifier;


        /// <summary>  Constructs a new IChemObject.</summary>
        public ChemObject()
        {
            flags = new bool[CDKConstants.MAX_FLAG_INDEX + 1];
            chemObjectListeners = null;
            properties = null;
            identifier = null;
        }


        /// <summary>  Lazy creation of chemObjectListeners List.
        /// 
        /// </summary>
        /// <returns>    List with the ChemObjects associated.
        /// </returns>
        private System.Collections.IList lazyChemObjectListeners()
        {
            if (chemObjectListeners == null)
            {
                chemObjectListeners = new System.Collections.ArrayList();
            }
            return chemObjectListeners;
        }


        /// <summary>  Use this to add yourself to this IChemObject as a listener. In order to do
        /// so, you must implement the ChemObjectListener Interface.
        /// 
        /// </summary>
        /// <param name="col"> the ChemObjectListener
        /// </param>
        /// <seealso cref="removeListener">
        /// </seealso>
        public virtual void addListener(IChemObjectListener col)
        {
            System.Collections.IList listeners = lazyChemObjectListeners();

            if (!listeners.Contains(col))
            {
                listeners.Add(col);
            }
            // Should we throw an exception if col is already in here or
            // just silently ignore it?
        }


        /// <summary>  Use this to remove a ChemObjectListener from the ListenerList of this
        /// IChemObject. It will then not be notified of change in this object anymore.
        /// 
        /// </summary>
        /// <param name="col"> The ChemObjectListener to be removed
        /// </param>
        /// <seealso cref="addListener">
        /// </seealso>
        public virtual void removeListener(IChemObjectListener col)
        {
            if (chemObjectListeners == null)
            {
                return;
            }

            System.Collections.IList listeners = lazyChemObjectListeners();
            if (listeners.Contains(col))
            {
                listeners.Remove(col);
            }
        }

        /// <summary>  This should be triggered by an method that changes the content of an object
        /// to that the registered listeners can react to it.
        /// </summary>
        public virtual void notifyChanged()
        {
            if (Notification && ListenerCount > 0)
            {
                System.Collections.IList listeners = lazyChemObjectListeners();
                for (int f = 0; f < listeners.Count; f++)
                {
                    ((IChemObjectListener)listeners[f]).stateChanged(new ChemObjectChangeEvent(this));
                }
            }
        }

        /// <summary>  This should be triggered by an method that changes the content of an object
        /// to that the registered listeners can react to it. This is a version of
        /// notifyChanged() which allows to propagate a change event while preserving
        /// the original origin.
        /// 
        /// </summary>
        /// <param name="evt"> A ChemObjectChangeEvent pointing to the source of where
        /// the change happend
        /// </param>
        public virtual void notifyChanged(IChemObjectChangeEvent evt)
        {
            if (Notification && ListenerCount > 0)
            {
                System.Collections.IList listeners = lazyChemObjectListeners();
                for (int f = 0; f < listeners.Count; f++)
                {
                    ((IChemObjectListener)listeners[f]).stateChanged(evt);
                }
            }
        }


        /// <summary> Lazy creation of properties hash.
        /// 
        /// </summary>
        /// <returns>    Returns in instance of the properties
        /// </returns>
        private System.Collections.Hashtable lazyProperties()
        {
            if (properties == null)
            {
                properties = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            }
            return properties;
        }


        /// <summary>  Sets a property for a IChemObject.
        /// 
        /// </summary>
        /// <param name="description"> An object description of the property (most likely a
        /// unique string)
        /// </param>
        /// <param name="property">    An object with the property itself
        /// </param>
        /// <seealso cref="getProperty">
        /// </seealso>
        /// <seealso cref="removeProperty">
        /// </seealso>
        public virtual void setProperty(System.Object description, System.Object property)
        {
            lazyProperties()[description] = property;
            notifyChanged();
        }


        /// <summary>  Removes a property for a IChemObject.
        /// 
        /// </summary>
        /// <param name="description"> The object description of the property (most likely a
        /// unique string)
        /// </param>
        /// <seealso cref="setProperty">
        /// </seealso>
        /// <seealso cref="getProperty">
        /// </seealso>
        public virtual void removeProperty(System.Object description)
        {
            if (properties == null)
            {
                return;
            }
            lazyProperties().Remove(description);
        }


        /// <summary>  Returns a property for the IChemObject.
        /// 
        /// </summary>
        /// <param name="description"> An object description of the property (most likely a
        /// unique string)
        /// </param>
        /// <returns>              The object containing the property. Returns null if
        /// propert is not set.
        /// </returns>
        /// <seealso cref="setProperty">
        /// </seealso>
        /// <seealso cref="removeProperty">
        /// </seealso>
        public virtual System.Object getProperty(System.Object description)
        {
            if (properties != null)
            {
                return lazyProperties()[description];
            }
            return null;
        }

        /// <summary>  Clones this <code>IChemObject</code>. It clones the identifier, flags,
        /// properties and pointer vectors. The ChemObjectListeners are not cloned, and
        /// neither is the content of the pointer vectors.
        /// 
        /// </summary>
        /// <returns>    The cloned object
        /// </returns>
        public virtual System.Object Clone()
        {
            ChemObject clone = (ChemObject)base.MemberwiseClone();
            // clone the flags
            clone.flags = new bool[CDKConstants.MAX_FLAG_INDEX + 1];
            for (int f = 0; f < flags.Length; f++)
            {
                clone.flags[f] = flags[f];
            }
            // clone the properties
            if (properties != null)
            {
                System.Collections.Hashtable clonedHashtable = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
                System.Collections.IEnumerator keys = properties.Keys.GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                while (keys.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                    System.Object key = keys.Current;
                    if (key is IChemObject)
                    {
                        key = ((IChemObject)key).Clone();
                    }
                    System.Object value_Renamed = properties[key];
                    if (value_Renamed is IChemObject)
                    {
                        value_Renamed = ((IChemObject)value_Renamed).Clone();
                    }
                    clonedHashtable[key] = value_Renamed;
                }
                clone.properties = clonedHashtable;
            }
            // delete all listeners
            clone.chemObjectListeners = null;
            return clone;
        }


        /// <summary>  Compare a IChemObject with this IChemObject.
        /// 
        /// </summary>
        /// <param name="object"> Object of type AtomType
        /// </param>
        /// <returns>         Return true, if the atomtypes are equal
        /// </returns>
        public virtual bool compare(System.Object object_Renamed)
        {
            if (!(object_Renamed is IChemObject))
            {
                return false;
            }
            ChemObject chemObj = (ChemObject)object_Renamed;
            if ((System.Object)identifier == (System.Object)chemObj.identifier)
            {
                return true;
            }
            return false;
        }


        /// <summary>  Sets the value of some flag.
        /// 
        /// </summary>
        /// <param name="flag_type">  Flag to set
        /// </param>
        /// <param name="flag_value"> Value to assign to flag
        /// </param>
        /// <seealso cref="getFlag">
        /// </seealso>
        public virtual void setFlag(int flag_type, bool flag_value)
        {
            flags[flag_type] = flag_value;
            notifyChanged();
        }


        /// <summary>  Returns the value of some flag.
        /// 
        /// </summary>
        /// <param name="flag_type"> Flag to retrieve the value of
        /// </param>
        /// <returns>            true if the flag <code>flag_type</code> is set
        /// </returns>
        /// <seealso cref="setFlag">
        /// </seealso>
        public virtual bool getFlag(int flag_type)
        {
            return flags[flag_type];
        }

        /// <summary> Clones this <code>IChemObject</code>, but preserves references to <code>Object</code>s.
        /// 
        /// </summary>
        /// <returns>    Shallow copy of this IChemObject
        /// </returns>
        /// <seealso cref="clone">
        /// </seealso>
        public virtual System.Object shallowCopy()
        {
            System.Object copy = null;
            try
            {
                copy = base.MemberwiseClone();
            }
            catch (System.Exception e)
            {
                SupportClass.WriteStackTrace(e, System.Console.Error);
            }
            return copy;
        }

        private bool doNotification = true;
    }
}