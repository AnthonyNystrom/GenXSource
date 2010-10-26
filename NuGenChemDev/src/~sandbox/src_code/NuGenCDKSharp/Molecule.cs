/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
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
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> Represents the concept of a chemical molecule, an object composed of 
    /// atoms connected by bonds.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2000-10-02 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>     molecule </cdk.keyword>
    [Serializable]
    public class Molecule : AtomContainer, IMolecule
    {

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 6451193093484831136L;

        /// <summary>  Creates an Molecule without Atoms and Bonds.</summary>
        public Molecule()
            : base()
        {
        }

        /// <summary>  Constructor for the Molecule object. The parameters define the
        /// initial capacity of the arrays.
        /// 
        /// </summary>
        /// <param name="atomCount"> init capacity of Atom array
        /// </param>
        /// <param name="bondCount"> init capacity of Bond array
        /// </param>
        public Molecule(int atomCount, int bondCount)
            : base(atomCount, bondCount)
        {
        }

        /// <summary> Constructs a Molecule with
        /// a shallow copy of the atoms and bonds of an AtomContainer.
        /// 
        /// </summary>
        /// <param name="container"> An Molecule to copy the atoms and bonds from
        /// </param>
        public Molecule(IAtomContainer container)
            : base(container)
        {
        }

        /// <summary> Returns a one line string representation of this Atom.
        /// Methods is conform RFC #9.
        /// 
        /// </summary>
        /// <returns>  The string representation of this Atom
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder description = new System.Text.StringBuilder();
            description.Append("Molecule(");
            description.Append(ID).Append(", ");
            description.Append(base.ToString());
            description.Append(')');
            return description.ToString();
        }
    }
}