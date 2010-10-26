/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
*
* Copyright (C) 2000-2006  The Chemistry Development Kit (CDK) project
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
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> Represents the concept of an atom parity identifying the stereochemistry
    /// around an atom, given four neighbouring atoms.
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       steinbeck
    /// </author>
    /// <cdk.created>  2000-10-02 </cdk.created>
    /// <cdk.keyword>  atom parity </cdk.keyword>
    /// <cdk.keyword>  stereochemistry </cdk.keyword>
    [Serializable]
    public class AtomParity : ChemObject, IAtomParity, ICloneable
    {
        /// <summary> Returns the atom for which this parity is defined.
        /// 
        /// </summary>
        /// <returns> The atom for which this parity is defined
        /// </returns>
        virtual public IAtom Atom
        {
            get
            {
                return centralAtom;
            }

        }
        /// <summary> Returns the four atoms that define the stereochemistry for
        /// this parity.
        /// 
        /// </summary>
        /// <returns> The four atoms that define the stereochemistry for
        /// this parity
        /// </returns>
        virtual public IAtom[] SurroundingAtoms
        {
            get
            {
                return neighbors;
            }

        }
        /// <summary> Returns the parity value.
        /// 
        /// </summary>
        /// <returns> The parity value
        /// </returns>
        virtual public int Parity
        {
            get
            {
                return parity;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -2031408037315976637L;

        private IAtom centralAtom;
        private IAtom[] neighbors;
        private int parity;

        /// <summary> Constructs an completely unset AtomParity.
        /// 
        /// </summary>
        /// <param name="centralAtom">Atom for which the parity is defined
        /// </param>
        /// <param name="first">      First Atom of four that define the stereochemistry
        /// </param>
        /// <param name="second">     Second Atom of four that define the stereochemistry
        /// </param>
        /// <param name="third">      Third Atom of four that define the stereochemistry
        /// </param>
        /// <param name="fourth">     Fourth Atom of four that define the stereochemistry
        /// </param>
        /// <param name="parity">     +1 or -1, defining the parity
        /// </param>
        public AtomParity(IAtom centralAtom, IAtom first, IAtom second, IAtom third, IAtom fourth, int parity)
        {
            this.centralAtom = centralAtom;
            this.neighbors = new Atom[4];
            this.neighbors[0] = first;
            this.neighbors[1] = second;
            this.neighbors[2] = third;
            this.neighbors[3] = fourth;
            this.parity = parity;
        }

        /// <summary> Returns a one line string representation of this AtomParity.
        /// Methods is conform RFC #9.
        /// 
        /// </summary>
        /// <returns>  The string representation of this AtomParity
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder(32);
            resultString.Append("AtomParity(");
            resultString.Append(this.GetHashCode()).Append(", ");
            resultString.Append(centralAtom.ID);
            resultString.Append(", F:[").Append(neighbors[0].ID).Append(", ");
            resultString.Append(neighbors[1].ID).Append(", ");
            resultString.Append(neighbors[2].ID).Append(", ");
            resultString.Append(neighbors[3].ID).Append("], ");
            resultString.Append(parity);
            resultString.Append(')');
            return resultString.ToString();
        }

        /// <summary> Clones this AtomParity object.
        /// 
        /// </summary>
        /// <returns>  The cloned object   
        /// </returns>
        public override System.Object Clone()
        {
            AtomParity clone = (AtomParity)base.Clone();
            // clone Atom's
            clone.centralAtom = (IAtom)centralAtom.Clone();
            clone.neighbors = new IAtom[4];
            clone.neighbors[0] = (IAtom)(neighbors[0].Clone());
            clone.neighbors[1] = (IAtom)(neighbors[1].Clone());
            clone.neighbors[2] = (IAtom)(neighbors[2].Clone());
            clone.neighbors[3] = (IAtom)(neighbors[3].Clone());
            return clone;
        }
    }
}