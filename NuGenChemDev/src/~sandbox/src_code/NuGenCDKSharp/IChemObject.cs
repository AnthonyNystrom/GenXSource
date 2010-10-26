/* $RCSfile$
* $Author: egonw $
* $Date: 2006-06-07 19:53:16 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6353 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using System.Collections;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> The base class for all chemical objects in this cdk. It provides methods for
    /// adding listeners and for their notification of events, as well a a hash
    /// table for administration of physical or chemical properties
    /// 
    /// </summary>
    /// <author>         egonw
    /// </author>
    /// <cdk.module>     interfaces </cdk.module>
    public interface IChemObject : ICloneable
    {
        /// <summary> Returns the number of ChemObjectListeners registered with this object.
        /// 
        /// </summary>
        /// <returns>    the number of registered listeners.
        /// </returns>
        int ListenerCount
        {
            get;
        }
        
        /// <summary> Returns the flag that indicates wether notification messages are sent around.
        /// 
        /// </summary>
        /// <returns> true if messages are sent.
        /// </returns>
        /// <seealso cref="setNotification(boolean)">
        /// </seealso>
        /// <summary> Set a flag to use or not use notification. By default it should be set
        /// to true.
        /// 
        /// </summary>
        /// <param name="bool">if true, then notification messages are sent.
        /// </param>
        /// <seealso cref="getNotification()">
        /// </seealso>
        bool Notification
        {
            get;
            set;
        }
        
        /// <summary>  Returns a Map with the IChemObject's properties.
        /// 
        /// </summary>
        /// <returns>    The object's properties as an Hashtable
        /// </returns>
        /// <seealso cref="setProperties">
        /// </seealso>
        /// <summary> Sets the properties of this object.
        /// 
        /// </summary>
        /// <param name="properties"> a Hashtable specifying the property values
        /// </param>
        /// <seealso cref="getProperties">
        /// </seealso>
        Hashtable Properties
        {
            get;
            set;
        }

        /// <summary> Returns the identifier (ID) of this object.
        /// 
        /// </summary>
        /// <returns>    a String representing the ID value
        /// </returns>
        /// <seealso cref="setID">
        /// </seealso>
        /// <summary> Sets the identifier (ID) of this object.
        /// 
        /// </summary>
        /// <param name="identifier"> a String representing the ID value
        /// </param>
        /// <seealso cref="getID">
        /// </seealso>
        String ID
        {
            get;
            set;
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
        bool[] Flags
        {
            get;
            set;
        }

        /// <summary> Returns a ChemObjectBuilder for the data classes that extend
        /// this class.
        /// 
        /// </summary>
        /// <returns> The IChemObjectBuilder matching this IChemObject
        /// </returns>
        IChemObjectBuilder Builder
        {
            get;
        }

        /// <summary> Use this to add yourself to this IChemObject as a listener. In order to do
        /// so, you must implement the ChemObjectListener Interface.
        /// 
        /// </summary>
        /// <param name="col"> the ChemObjectListener
        /// </param>
        /// <seealso cref="removeListener">
        /// </seealso>
        void addListener(IChemObjectListener col);

        /// <summary> Use this to remove a ChemObjectListener from the ListenerList of this
        /// IChemObject. It will then not be notified of change in this object anymore.
        /// 
        /// </summary>
        /// <param name="col"> The ChemObjectListener to be removed
        /// </param>
        /// <seealso cref="addListener">
        /// </seealso>
        void removeListener(IChemObjectListener col);

        /// <summary> This should be triggered by an method that changes the content of an object
        /// to that the registered listeners can react to it.
        /// </summary>
        void notifyChanged();

        /// <summary> This should be triggered by an method that changes the content of an object
        /// to that the registered listeners can react to it. This is a version of
        /// notifyChanged() which allows to propagate a change event while preserving
        /// the original origin.
        /// 
        /// </summary>
        /// <param name="evt"> A ChemObjectChangeEvent pointing to the source of where
        /// the change happend
        /// </param>
        void notifyChanged(IChemObjectChangeEvent evt);

        /// <summary> Sets a property for a IChemObject.
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
        void setProperty(System.Object description, System.Object property);

        /// <summary> Removes a property for a IChemObject.
        /// 
        /// </summary>
        /// <param name="description"> The object description of the property (most likely a
        /// unique string)
        /// </param>
        /// <seealso cref="setProperty">
        /// </seealso>
        /// <seealso cref="getProperty">
        /// </seealso>
        void removeProperty(System.Object description);

        /// <summary> Returns a property for the IChemObject.
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
        Object getProperty(Object description);

        /// <summary> Sets the value of some flag.
        /// 
        /// </summary>
        /// <param name="flag_type">  Flag to set
        /// </param>
        /// <param name="flag_value"> Value to assign to flag
        /// </param>
        /// <seealso cref="getFlag">
        /// </seealso>
        void setFlag(int flag_type, bool flag_value);

        /// <summary> Returns the value of some flag.
        /// 
        /// </summary>
        /// <param name="flag_type"> Flag to retrieve the value of
        /// </param>
        /// <returns>            true if the flag <code>flag_type</code> is set
        /// </returns>
        /// <seealso cref="setFlag">
        /// </seealso>
        bool getFlag(int flag_type);

        /// <summary> Returns a one line description of this IChemObject.
        /// 
        /// </summary>
        /// <returns> a String representation of this object
        /// </returns>
        String ToString();

        /// <summary> Returns a deep clone of this IChemObject.
        /// 
        /// </summary>
        /// <returns> Object the clone of this IChemObject.
        /// </returns>
        /// <throws>  CloneNotSupportedException if the IChemObject cannot be cloned </throws>
        //Object Clone();
    }
}