/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using javax.vecmath;

namespace Org.Jmol.Viewer
{
	class PhosphorusMonomer : Monomer
	{
		public override sbyte ProteinStructureType
		{
            get { return 0; }
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'phosphorusOffsets'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly sbyte[] phosphorusOffsets = new sbyte[] { 0 };

        public static Monomer validateAndAllocate(Chain chain, string group3, int seqcode, int firstIndex, int lastIndex, int[] specialAtomIndexes, Atom[] atoms)
		{
			//    System.out.println("PhosphorusMonomer.validateAndAllocate");
			if (firstIndex != lastIndex || specialAtomIndexes[JmolConstants.ATOMID_NUCLEIC_PHOSPHORUS] != firstIndex)
				return null;
			return new PhosphorusMonomer(chain, group3, seqcode, firstIndex, lastIndex, phosphorusOffsets);
		}
		
		////////////////////////////////////////////////////////////////

        public PhosphorusMonomer(Chain chain, string group3, int seqcode, int firstAtomIndex, int lastAtomIndex, sbyte[] offsets)
            : base(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, offsets)
		{ }

        public virtual bool isPhosphorusMonomer()
		{
			return true;
		}

        public override Atom getAtom(sbyte specialAtomID)
		{
			return (specialAtomID == JmolConstants.ATOMID_NUCLEIC_PHOSPHORUS?LeadAtom:null);
		}

        public override Point3f getAtomPoint(sbyte specialAtomID)
		{
			return (specialAtomID == JmolConstants.ATOMID_NUCLEIC_PHOSPHORUS?LeadAtomPoint:null);
		}

        public override bool isConnectedAfter(Monomer possiblyPreviousMonomer)
		{
			if (possiblyPreviousMonomer == null)
				return true;
			if (!(possiblyPreviousMonomer is PhosphorusMonomer))
				return false;
			// 1PN8 73:d and 74:d are 7.001 angstroms apart
			float distance = LeadAtomPoint.distance(possiblyPreviousMonomer.LeadAtomPoint);
			return distance <= 7.1f;
		}
	}
}