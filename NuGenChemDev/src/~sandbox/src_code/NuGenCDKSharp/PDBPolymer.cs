/* $RCSfile$
* $Author: egonw $
* $Date: 2006-04-19 15:01:24 +0200 (Wed, 19 Apr 2006) $
* $Revision: 6009 $
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

namespace Org.OpenScience.CDK.Protein.Data
{
    /// <summary> An entry in the PDB database. It is not just a regular protein, but the
    /// regular PDB mix of protein or protein complexes, ligands, water molecules
    /// and other species.
    /// 
    /// </summary>
    /// <cdk.module>   pdb </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen
    /// </author>
    /// <cdk.created>  2006-04-19 </cdk.created>
    /// <cdk.keyword>  polymer </cdk.keyword>
    [Serializable]
    public class PDBPolymer : BioPolymer
    {
        virtual public System.Collections.ICollection Structures
        {
            get
            {
                //		don't return the original
                return new System.Collections.ArrayList(secundairyStructures);
            }

        }
        /// <summary> Returns the monomer names in the order in which they were added.
        /// 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.interfaces.IPolymer.getMonomerNames()">
        /// </seealso>
        virtual public System.Collections.ICollection MonomerNamesInSequentialOrder
        {
            get
            {
                // don't return the original
                return new System.Collections.ArrayList(sequentialListOfMonomers);
            }

        }

        private const long serialVersionUID = 4173552834313952358L;

        internal System.Collections.IList sequentialListOfMonomers;
        internal System.Collections.IList secundairyStructures;

        /// <summary> Contructs a new Polymer to store the Monomers.</summary>
        public PDBPolymer()
            : base()
        {
            sequentialListOfMonomers = new System.Collections.ArrayList();
            secundairyStructures = new System.Collections.ArrayList();
        }

        public virtual void addStructure(PDBStructure structure)
        {
            secundairyStructures.Add(structure);
        }

        /// <summary> Adds the atom oAtom to a specified Monomer. Additionally, it keeps
        /// record of the iCode.
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        /// <param name="oMonomer"> The monomer the atom belongs to
        /// </param>
        public override void addAtom(IAtom oAtom, IMonomer oMonomer)
        {
            base.addAtom(oAtom, oMonomer);
            if (!sequentialListOfMonomers.Contains(oMonomer.MonomerName))
                sequentialListOfMonomers.Add(oMonomer.MonomerName);
        }

        /// <summary> Adds the atom oAtom to a specified Monomer of a specified Strand.
        /// Additionally, it keeps record of the iCode.
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        /// <param name="oMonomer"> The monomer the atom belongs to
        /// </param>
        public override void addAtom(IAtom oAtom, IMonomer oMonomer, IStrand oStrand)
        {
            base.addAtom(oAtom, oMonomer, oStrand);
            if (!sequentialListOfMonomers.Contains(oMonomer.MonomerName))
                sequentialListOfMonomers.Add(oMonomer.MonomerName);
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder();
            stringContent.Append("PDBPolymer(");
            stringContent.Append(this.GetHashCode()).Append(", ");
            stringContent.Append(base.ToString());
            stringContent.Append(")");
            return stringContent.ToString();
        }
    }
}