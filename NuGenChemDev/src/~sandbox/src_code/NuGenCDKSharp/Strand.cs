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
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using Support;

namespace Org.OpenScience.CDK
{
    /// <summary> A Strand is an AtomContainer which stores additional strand specific
    /// informations for a group of Atoms.
    /// 
    /// </summary>
    /// <cdk.module>   data </cdk.module>
    /// <cdk.created>  2004-12-20 </cdk.created>
    /// <author>       Martin Eklund <martin.eklund@farmbio.uu.se>
    /// </author>
    /// <cdk.bug>      1117765 </cdk.bug>
    [Serializable]
    public class Strand : AtomContainer, IStrand
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Retrieve the strand name.
        /// 
        /// </summary>
        /// <returns> The name of the Strand object
        /// </returns>
        /// <seealso cref="setStrandName">
        /// </seealso>
        /// <summary> Set the name of the Strand object.
        /// 
        /// </summary>
        /// <param name="cStrandName"> The new name for this strand
        /// </param>
        /// <seealso cref="getStrandName">
        /// </seealso>
        virtual public System.String StrandName
        {
            get
            {
                return strandName;
            }

            set
            {
                strandName = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Retrieve the strand type.
        /// 
        /// </summary>
        /// <returns> The type of the Strand object
        /// </returns>
        /// <seealso cref="setStrandType">
        /// </seealso>
        /// <summary> Set the type of the Strand object.
        /// 
        /// </summary>
        /// <param name="cStrandType"> The new type for this strand
        /// </param>
        /// <seealso cref="getStrandType">
        /// </seealso>
        virtual public System.String StrandType
        {
            get
            {
                return strandType;
            }

            set
            {
                strandType = value;
            }

        }
        /// <summary> 
        /// Return the number of monomers present in the Strand.
        /// 
        /// </summary>
        /// <returns> number of monomers
        /// 
        /// </returns>
        virtual public int MonomerCount
        {
            get
            {
                return monomers.Count - 1;
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
        /// <summary> Returns a hashtable containing the monomers in the strand.
        /// 
        /// </summary>
        /// <returns> hashtable containing the monomers in the strand.
        /// </returns>
        virtual public System.Collections.Hashtable Monomers
        {
            get
            {
                return monomers;
            }

        }
        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 4200943086350928356L;

        /// <summary>The list of all Monomers in the Strand.</summary>
        private System.Collections.Hashtable monomers;
        /// <summary>The name of this strand (e.g. A, B). </summary>
        private System.String strandName;
        /// <summary>The type of this strand (e.g. PEPTIDE, DNA, RNA). </summary>
        private System.String strandType;

        /// <summary> Contructs a new Strand.</summary>
        public Strand()
            : base()
        {
            // Stand stuff
            monomers = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            Monomer oMonomer = new Monomer();
            oMonomer.MonomerName = "";
            oMonomer.MonomerType = "UNKNOWN";
            monomers[""] = oMonomer;
            strandName = "";
        }

        /// <summary> 
        /// Adds the atom oAtom without specifying a Monomer or a Strand. Therefore the
        /// atom gets added to a Monomer of type UNKNOWN in a Strand of type UNKNOWN.
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// 
        /// </param>
        public override void addAtom(IAtom oAtom)
        {
            addAtom(oAtom, getMonomer(""));
        }

        /// <summary> 
        /// Adds the atom oAtom to a specific Monomer.
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        /// <param name="oMonomer"> The monomer the atom belongs to
        /// 
        /// </param>
        public virtual void addAtom(IAtom oAtom, IMonomer oMonomer)
        {

            int atomCount = base.AtomCount;

            // Add atom to AtomContainer
            base.addAtom(oAtom);

            if (atomCount != base.AtomCount)
            {
                // ok, super did not yet contain the atom

                if (oMonomer == null)
                {
                    oMonomer = getMonomer("");
                }

                oMonomer.addAtom(oAtom);
                if (!monomers.ContainsKey(oMonomer.MonomerName))
                {
                    monomers[oMonomer.MonomerName] = oMonomer;
                }
            }
        }

        /// <summary> 
        /// Retrieve a Monomer object by specifying its name.
        /// 
        /// </summary>
        /// <param name="cName"> The name of the monomer to look for
        /// </param>
        /// <returns> The Monomer object which was asked for
        /// 
        /// </returns>
        public virtual IMonomer getMonomer(System.String cName)
        {
            return (Monomer)monomers[cName];
        }

        /// <summary> 
        /// Adds a <code>Monomer</code> to this <code>Strand</code>. All atoms and
        /// bonds in the Monomer are added. NB: The <code>Monomer</code> will *not*
        /// "automatically" be connected to the <code>Strand</code>. That has to be
        /// done "manually" (as the "connection point" is not known). 
        /// </summary>
        /// <param name="monomer">
        /// </param>
        /*public void addMonomer(Monomer monomer)	{
        if (! monomers.contains(monomer.getMonomerName())) {
        monomers.put(monomer.getMonomerName(), monomer);	// Adderas atomer etc? Nope!
        }
        }*/

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
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder(32);
            stringContent.Append("Strand(");
            stringContent.Append(this.GetHashCode());
            stringContent.Append(", N:").Append(StrandName);
            stringContent.Append(", T:").Append(StrandType).Append(", ");
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