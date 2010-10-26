/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
* 
* Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*  */
using System;
using Org.OpenScience.CDK.Interfaces;
using Support;

namespace Org.OpenScience.CDK
{
    /// <summary> Subclass of Molecule to store Polymer specific attributes that a Polymer has.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Edgar Luttmann <edgar@uni-paderborn.de>
    /// </author>
    /// <author>       Martin Eklund <martin.eklund@farmbio.uu.se>
    /// </author>
    /// <cdk.created>  2001-08-06 </cdk.created>
    /// <cdk.keyword>  polymer </cdk.keyword>
    /// <cdk.bug>      1117765 </cdk.bug>
    [Serializable]
    public class Polymer : Molecule, IPolymer
    {
        /// <summary> Return the number of monomers present in the Polymer.
        /// 
        /// </summary>
        /// <returns> number of monomers
        /// </returns>
        virtual public int MonomerCount
        {
            get
            {
                return monomers.Count;
            }

        }
        /// <summary> Returns a collection of the names of all <code>Monomer</code>s in this
        /// polymer.
        /// 
        /// </summary>
        /// <returns> a <code>Collection</code> of all the monomer names.
        /// </returns>
        virtual public System.Collections.ICollection MonomerNames
        {
            get
            {
                return new CSGraphT.SupportClass.HashSetSupport(monomers);
            }

        }
        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide/serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -2596790658835319339L;

        private System.Collections.Hashtable monomers; // the list of all the contained Monomers. 

        /// <summary> Contructs a new Polymer to store the Monomers.</summary>
        public Polymer()
            : base()
        {
            monomers = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        }

        /// <summary> Adds the atom oAtom to a specified Monomer.
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        /// <param name="oMonomer"> The monomer the atom belongs to
        /// </param>
        public virtual void addAtom(IAtom oAtom, IMonomer oMonomer)
        {

            if (!contains(oAtom))
            {
                base.addAtom(oAtom);

                if (oMonomer != null)
                {
                    // Not sure what's better here...throw nullpointer exception?
                    oMonomer.addAtom(oAtom);

                    if (!monomers.ContainsKey(oMonomer.MonomerName))
                    {
                        monomers[oMonomer.MonomerName] = oMonomer;
                    }
                }
            }
            /* notifyChanged() is called by addAtom in
            AtomContainer */
        }

        /// <summary> Retrieve a Monomer object by specifying its name.
        /// 
        /// </summary>
        /// <param name="cName"> The name of the monomer to look for
        /// </param>
        /// <returns> The Monomer object which was asked for
        /// </returns>
        public virtual IMonomer getMonomer(System.String cName)
        {
            return (Monomer)monomers[cName];
        }

        /// <summary> Removes a particular monomer, specified by its name.
        /// 
        /// </summary>
        /// <param name="name">The name of the monomer to remove
        /// </param>
        public virtual void removeMonomer(System.String name)
        {
            if (monomers.ContainsKey(name))
            {
                Monomer monomer = (Monomer)monomers[name];
                this.remove(monomer);
                monomers.Remove(name);
            }
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder();
            stringContent.Append("Polymer(");
            stringContent.Append(this.GetHashCode()).Append(", ");
            //        stringContent.append("N:").append(getStrandName()).append(", ");
            //        stringContent.append("T:").append(getStrandType()).append(", ");
            stringContent.Append(base.ToString());
            stringContent.Append(')');
            return stringContent.ToString();
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}