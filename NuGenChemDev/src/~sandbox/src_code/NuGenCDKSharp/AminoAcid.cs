/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
*
* Copyright (C) 2005-2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK
{
    /// <summary> A AminoAcid is Monomer which stores additional amino acid specific 
    /// informations, like the N-terminus atom.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen <e.willighagen@science.ru.nl>
    /// </author>
    /// <cdk.created>  2005-08-11 </cdk.created>
    /// <cdk.keyword>  amino acid </cdk.keyword>
    [Serializable]
    public class AminoAcid : Monomer, IAminoAcid, ICloneable
    {

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -5032283549467862509L;

        /// <summary>The atom that constitutes the N-terminus. </summary>
        private IAtom nTerminus;
        /// <summary>The atom that constitutes the C-terminus. </summary>
        private IAtom cTerminus;

        /// <summary> Contructs a new AminoAcid.</summary>
        public AminoAcid()
            : base()
        {
        }

        /// <summary> Retrieves the N-terminus atom.
        /// 
        /// </summary>
        /// <returns> The Atom that is the N-terminus
        /// 
        /// </returns>
        /// <seealso cref="addNTerminus(IAtom)">
        /// </seealso>
        public virtual IAtom getNTerminus()
        {
            return nTerminus;
        }

        /// <summary> Add an Atom and makes it the N-terminus atom.
        /// 
        /// </summary>
        /// <param name="atom"> The Atom that is the N-terminus
        /// 
        /// </param>
        /// <seealso cref="getNTerminus">
        /// </seealso>
        public virtual void addNTerminus(IAtom atom)
        {
            base.addAtom(atom);
            nTerminus = atom;
        }

        /// <summary> Marks an Atom as being the N-terminus atom. It assumes that the Atom
        /// is already added to the AminoAcid.
        /// 
        /// </summary>
        /// <param name="atom"> The Atom that is the N-terminus
        /// 
        /// </param>
        /// <seealso cref="addNTerminus">
        /// </seealso>
        private void setNTerminus(IAtom atom)
        {
            nTerminus = atom;
        }

        /// <summary> Retrieves the C-terminus atom.
        /// 
        /// </summary>
        /// <returns> The Atom that is the C-terminus
        /// 
        /// </returns>
        /// <seealso cref="addCTerminus(IAtom)">
        /// </seealso>
        public virtual IAtom getCTerminus()
        {
            return cTerminus;
        }

        /// <summary> Add an Atom and makes it the C-terminus atom.
        /// 
        /// </summary>
        /// <param name="atom"> The Atom that is the C-terminus
        /// 
        /// </param>
        /// <seealso cref="getCTerminus">
        /// </seealso>
        public virtual void addCTerminus(IAtom atom)
        {
            base.addAtom(atom);
            setCTerminus(atom);
        }

        /// <summary> Marks an Atom as being the C-terminus atom. It assumes that the Atom
        /// is already added to the AminoAcid.
        /// 
        /// </summary>
        /// <param name="atom"> The Atom that is the C-terminus
        /// 
        /// </param>
        /// <seealso cref="addCTerminus">
        /// </seealso>
        private void setCTerminus(IAtom atom)
        {
            cTerminus = atom;
        }

        /// <summary> Clones this AminoAcid object.
        /// 
        /// </summary>
        /// <returns>    The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            AminoAcid clone = (AminoAcid)base.Clone();
            // copying the new N-terminus and C-terminus pointers
            if (getNTerminus() != null)
                clone.setNTerminus(clone.getAtomAt(getAtomNumber(getNTerminus())));
            if (getCTerminus() != null)
                clone.setCTerminus(clone.getAtomAt(getAtomNumber(getCTerminus())));
            return clone;
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder(32);
            stringContent.Append("AminoAcid(");
            stringContent.Append(this.GetHashCode()).Append(", ");
            if (nTerminus == null)
            {
                stringContent.Append("N: null, ");
            }
            else
            {
                stringContent.Append("N: ").Append(nTerminus.GetHashCode()).Append(", ");
            }
            if (cTerminus == null)
            {
                stringContent.Append("C: null, ");
            }
            else
            {
                stringContent.Append("C: ").Append(cTerminus.GetHashCode()).Append(", ");
            }
            stringContent.Append(base.ToString());
            stringContent.Append(')');
            return stringContent.ToString();
        }
    }
}