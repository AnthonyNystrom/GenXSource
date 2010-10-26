/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
*
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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
    /// <summary> A Single Electron is an orbital which is occupied by only one electron.
    /// A radical in CDK is represented by an AtomContainer that contains an Atom
    /// and a SingleElectron type ElectronContainer:
    /// <pre>
    /// AtomContainer radical = new org.openscience.cdk.AtomContainer();
    /// Atom carbon = new Atom("C");
    /// carbon.setImplicitHydrogens(3);
    /// radical.addElectronContainer(new SingleElectron(carbon));
    /// </pre> 
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  radical </cdk.keyword>
    /// <cdk.keyword>  electron, unpaired </cdk.keyword>
    [Serializable]
    public class SingleElectron : ElectronContainer, ISingleElectron, ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
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
        /// <param name="atom">the Atom this SingleElectron will be associated with
        /// 
        /// </param>
        /// <seealso cref="getAtom">
        /// </seealso>
        virtual public IAtom Atom
        {
            get
            {
                return (Atom)this.atom;
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
        private const long serialVersionUID = 7796574734668490940L;

        /// <summary>Number of electron for this class is defined as one. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'electronCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        new protected internal int electronCount = 1;

        /// <summary>The atom with which this single electron is associated. </summary>
        protected internal IAtom atom;

        /// <summary> Constructs an single electron orbital on an Atom.
        /// 
        /// </summary>
        /// <param name="atom">The atom to which the single electron belongs.
        /// </param>
        public SingleElectron(IAtom atom)
        {
            this.atom = atom;
        }

        /// <summary> Constructs an single electron orbital with an associated Atom.</summary>
        public SingleElectron()
        {
            this.atom = null;
        }
        /// <summary> Returns the number of electrons in this SingleElectron.
        /// 
        /// </summary>
        /// <returns> The number of electrons in this SingleElectron.
        /// </returns>
        public override int getElectronCount()
        {
            return this.electronCount;
        }

        /// <summary> Returns true if the given atom participates in this SingleElectron.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be tested if it participates in this bond
        /// </param>
        /// <returns>     true if this SingleElectron is associated with the atom
        /// </returns>
        public virtual bool contains(IAtom atom)
        {
            return (this.atom == atom) ? true : false;
        }

        /// <summary> Returns a one line string representation of this SingleElectron.
        /// This method is conform RFC #9.
        /// 
        /// </summary>
        /// <returns>    The string representation of this SingleElectron
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder();
            stringContent.Append("SingleElectron(");
            stringContent.Append(this.GetHashCode());
            if (atom != null)
            {
                stringContent.Append(", ");
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                stringContent.Append(atom.ToString());
            }
            stringContent.Append(')');
            return stringContent.ToString();
        }

        /// <summary> Clones this SingleElectron object, including a clone of the atom for which the
        /// SingleElectron is defined.
        /// 
        /// </summary>
        /// <returns>    The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            SingleElectron clone = (SingleElectron)base.Clone();
            // clone the Atom
            if (atom != null)
            {
                clone.atom = (IAtom)((IAtom)atom).Clone();
            }
            return clone;
        }
    }
}