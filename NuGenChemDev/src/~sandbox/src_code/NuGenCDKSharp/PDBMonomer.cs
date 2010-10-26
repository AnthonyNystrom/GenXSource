/* $RCSfile$
* $Author: egonw $
* $Date: 2006-04-19 15:22:09 +0200 (Wed, 19 Apr 2006) $
* $Revision: 6013 $
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
*
*/
using System;

namespace Org.OpenScience.CDK.Protein.Data
{
    /// <summary> Represents the idea of an monomer as used in PDB files. It contains extra fields
    /// normally associated with atoms in such files.
    /// 
    /// </summary>
    /// <cdk.module>  pdb </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="PDBAtom">
    /// </seealso>
    [Serializable]
    public class PDBMonomer : Monomer
    {
        virtual public System.String ICode
        {
            get
            {
                return iCode;
            }

            set
            {
                iCode = value;
            }

        }
        virtual public System.String ChainID
        {
            get
            {
                return chainID;
            }

            set
            {
                chainID = value;
            }

        }

        private const long serialVersionUID = -7236625816763776733L;

        private System.String iCode;
        private System.String chainID;

        public PDBMonomer()
            : base()
        {
            initValues();
        }

        private void initValues()
        {
            iCode = null;
            chainID = null;
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
            description.Append("PDBMonomer(");
            description.Append(this.GetHashCode()).Append(", ");
            description.Append("iCode=").Append(ICode).Append(", ");
            description.Append("chainID=").Append(ChainID).Append(", ");
            description.Append(base.ToString());
            description.Append(")");
            return description.ToString();
        }
    }
}