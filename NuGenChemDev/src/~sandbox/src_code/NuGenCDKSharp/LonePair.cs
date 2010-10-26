/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
*
* Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) project
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
* 
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> A LonePair is an orbital primarily located with one Atom, containing
    /// two electrons.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  orbital </cdk.keyword>
    /// <cdk.keyword>  lone-pair </cdk.keyword>
    /// <cdk.keyword>  bond </cdk.keyword>
    [Serializable]
    public class LonePair : ElectronContainer, ILonePair, ICloneable
    {
        /// <summary> Returns the associated Atom.
        /// 
        /// </summary>
        /// <returns> the associated Atom.
        /// 
        /// </returns>
        /// <seealso cref="setAtom">
        /// </seealso>
        /// <summary> Sets the associated Atom.
        /// 
        /// </summary>
        /// <param name="atom">the Atom this lone pair will be associated with
        /// 
        /// </param>
        /// <seealso cref="getAtom">
        /// </seealso>
        virtual public IAtom Atom
        {
            get
            {
                return this.atom;
            }

            set
            {
                this.atom = value;
                notifyChanged();
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 51311422004885329L;

        /// <summary>Number of electrons in the lone pair. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'electronCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        new protected internal int electronCount = 2;

        /// <summary>The atom with which this lone pair is associated. </summary>
        protected internal IAtom atom;

        /// <summary> Constructs an unconnected lone pair.
        /// 
        /// </summary>
        public LonePair()
        {
            this.atom = null;
        }

        /// <summary> Constructs an lone pair on an Atom.
        /// 
        /// </summary>
        /// <param name="atom"> Atom to which this lone pair is connected
        /// </param>
        public LonePair(IAtom atom)
        {
            this.atom = atom;
        }

        /// <summary> Returns the number of electrons in a LonePair.
        /// 
        /// </summary>
        /// <returns> The number of electrons in a LonePair.
        /// </returns>
        public override int getElectronCount()
        {
            return this.electronCount;
        }

        /// <summary> Returns true if the given atom participates in this lone pair.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be tested if it participates in this bond
        /// </param>
        /// <returns>     true if this lone pair is associated with the atom
        /// </returns>
        public virtual bool contains(IAtom atom)
        {
            return (this.atom == atom) ? true : false;
        }

        /// <summary> Clones this LonePair object, including a clone of the atom for which the
        /// lone pair is defined.
        /// 
        /// </summary>
        /// <returns>    The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            LonePair clone = (LonePair)base.Clone();
            // clone the Atom
            if (atom != null)
            {
                clone.atom = (IAtom)((IAtom)atom).Clone();
            }
            return clone;
        }

        /// <summary> Returns a one line string representation of this LonePair.
        /// This method is conform RFC #9.
        /// 
        /// </summary>
        /// <returns>    The string representation of this LonePair
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder();
            resultString.Append("LonePair(");
            resultString.Append(this.GetHashCode());
            if (atom != null)
            {
                resultString.Append(", ");
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                resultString.Append(atom.ToString());
            }
            resultString.Append(')');
            return resultString.ToString();
        }
    }
}