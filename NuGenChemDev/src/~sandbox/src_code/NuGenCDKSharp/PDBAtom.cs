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
using javax.vecmath;

namespace Org.OpenScience.CDK.Protein.Data
{
    /// <summary> Represents the idea of an atom as used in PDB files. It contains extra fields
    /// normally associated with atoms in such files.
    /// 
    /// </summary>
    /// <cdk.module>  pdb </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="Atom">
    /// </seealso>
    [Serializable]
    public class PDBAtom : Atom, System.ICloneable
    {
        virtual public System.String Record
        {
            get
            {
                return record;
            }

            set
            {
                record = value;
            }

        }
        virtual public double TempFactor
        {
            get
            {
                return tempFactor;
            }

            set
            {
                tempFactor = value;
            }

        }
        virtual public System.String ResName
        {
            get
            {
                return resName;
            }

            set
            {
                resName = value;
            }

        }
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
        virtual public System.String Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
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
        virtual public System.String AltLoc
        {
            get
            {
                return altLoc;
            }

            set
            {
                altLoc = value;
            }

        }
        virtual public System.String SegID
        {
            get
            {
                return segID;
            }

            set
            {
                segID = value;
            }

        }
        virtual public int Serial
        {
            get
            {
                return serial;
            }

            set
            {
                serial = value;
            }

        }
        virtual public System.String ResSeq
        {
            get
            {
                return resSeq;
            }

            set
            {
                resSeq = value;
            }

        }
        virtual public bool Oxt
        {
            get
            {
                return oxt;
            }

            set
            {
                oxt = value;
            }

        }
        virtual public bool HetAtom
        {
            get
            {
                return hetAtom;
            }

            set
            {
                hetAtom = value;
            }

        }
        virtual public double Occupancy
        {
            get
            {
                return occupancy;
            }

            set
            {
                occupancy = value;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href="http://java.sun.com/products/jdk/1.1/docs/guide/serialization/spec/version.doc.html">details</a>.
        /// </summary>
        private const long serialVersionUID = 7670650135045832543L;

        private System.String record;
        private double tempFactor;
        private System.String resName;
        private System.String iCode;
        private double occupancy;
        private System.String name;
        private System.String chainID;
        private System.String altLoc;
        private System.String segID;
        private int serial;
        private System.String resSeq;
        private bool oxt;
        private bool hetAtom;

        public PDBAtom(System.String symbol)
            : base(symbol)
        {
            initValues();
        }

        public PDBAtom(System.String symbol, Point3d coordinate)
            : base(symbol, coordinate)
        {
            initValues();
        }

        private void initValues()
        {
            record = null;
            tempFactor = -1.0;
            resName = null;
            iCode = null;
            occupancy = -1.0;
            name = null;
            chainID = null;
            altLoc = null;
            segID = null;
            serial = 0;
            resSeq = null;

            oxt = false;
            hetAtom = false;
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
            description.Append("PDBAtom(");
            description.Append(this.GetHashCode()).Append(", ");
            description.Append("altLoc=").Append(AltLoc).Append(", ");
            description.Append("chainID=").Append(ChainID).Append(", ");
            description.Append("iCode=").Append(ICode).Append(", ");
            description.Append("name=").Append(Name).Append(", ");
            description.Append("resName=").Append(ResName).Append(", ");
            description.Append("resSeq=").Append(ResSeq).Append(", ");
            description.Append("segID=").Append(SegID).Append(", ");
            description.Append("serial=").Append(Serial).Append(", ");
            description.Append("tempFactor=").Append(TempFactor).Append(", ");
            description.Append("oxt=").Append(Oxt).Append(", ");
            description.Append("hetatm=").Append(HetAtom).Append(", ");
            description.Append(base.ToString());
            description.Append(")");
            return description.ToString();
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}