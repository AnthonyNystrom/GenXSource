/* $RCSfile: $
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (Thu, 10 Nov 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2006  The Chemistry Development Kit (CDK) project
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;

namespace Org.OpenScience.CDK.Protein.Data
{
    /// <summary> Holder for secundary protein structure elements. Lously modeled after
    /// the Jmol Structure.java.
    /// 
    /// </summary>
    /// <author>      egonw
    /// 
    /// </author>
    /// <cdk.module>  pdb </cdk.module>
    public class PDBStructure
    {
        virtual public char EndChainID
        {
            get
            {
                return endChainID;
            }

            set
            {
                this.endChainID = value;
            }

        }
        virtual public char EndInsertionCode
        {
            get
            {
                return endInsertionCode;
            }

            set
            {
                this.endInsertionCode = value;
            }

        }
        virtual public int EndSequenceNumber
        {
            get
            {
                return endSequenceNumber;
            }

            set
            {
                this.endSequenceNumber = value;
            }

        }
        virtual public char StartChainID
        {
            get
            {
                return startChainID;
            }

            set
            {
                this.startChainID = value;
            }

        }
        virtual public char StartInsertionCode
        {
            get
            {
                return startInsertionCode;
            }

            set
            {
                this.startInsertionCode = value;
            }

        }
        virtual public int StartSequenceNumber
        {
            get
            {
                return startSequenceNumber;
            }

            set
            {
                this.startSequenceNumber = value;
            }

        }
        virtual public System.String StructureType
        {
            get
            {
                return structureType;
            }

            set
            {
                this.structureType = value;
            }

        }

        public const System.String HELIX = "helix";
        public const System.String SHEET = "sheet";
        public const System.String TURN = "turn";

        private System.String structureType;
        private char startChainID;
        private int startSequenceNumber;
        private char startInsertionCode;
        private char endChainID;
        private int endSequenceNumber;
        private char endInsertionCode;
    }
}