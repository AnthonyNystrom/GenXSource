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
	class NucleicMonomer : PhosphorusMonomer
	{
		public override bool Dna
		{
            get { return !hasRnaO2Prime; }
		}

		public override bool Rna
		{
            get { return hasRnaO2Prime; }
		}

		public override bool Purine
		{
            get { return isPurine_Renamed_Field; }
		}

		public override bool Pyrimidine
		{
            get { return isPyrimidine_Renamed_Field; }
		}
		
        public virtual bool Guanine
		{
            get { return offsets[18] != -1; }
		}

		public override sbyte ProteinStructureType
		{
            get { return (hasRnaO2Prime ? JmolConstants.PROTEIN_STRUCTURE_RNA : JmolConstants.PROTEIN_STRUCTURE_DNA); }
		}

		public virtual Atom N1
		{
			////////////////////////////////////////////////////////////////
            get { return getAtomFromOffsetIndex(5); }
		}

		public virtual Atom N3
		{
            get { return getAtomFromOffsetIndex(7); }
		}

		public virtual Atom N2
		{
            get { return getAtomFromOffsetIndex(18); }
		}

		public virtual Atom O2
		{
            get { return getAtomFromOffsetIndex(9); }
		}

		public virtual Atom O6
		{
            get { return getAtomFromOffsetIndex(14); }
		}

		public virtual Atom N4
		{
            get { return getAtomFromOffsetIndex(15); }
		}

		public virtual Atom N6
		{
            get { return getAtomFromOffsetIndex(17); }
		}

		public virtual Atom O4
		{
            get { return getAtomFromOffsetIndex(13); }
		}

		public override Atom TerminatorAtom
		{
            get { return getAtomFromOffsetIndex(offsets[23] != -1 ? 23 : 24); }
		}

		public virtual Atom O3PrimeAtom
		{
            get { return getAtomFromOffsetIndex(24); }
		}

		public virtual Atom PhosphorusAtom
		{
            get { return getAtomFromOffsetIndex(25); }
		}

		public virtual Atom O5PrimeAtom
		{
            get { return getAtomFromOffsetIndex(22); }
		}

		public virtual Atom C3PrimeAtom
		{
            get { return getAtomFromOffsetIndex(26); }
		}
		
		// negative values are optional
		//UPGRADE_NOTE: Final was removed from the declaration of 'interestingNucleicAtomIDs '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'interestingNucleicAtomIDs' was moved to static method 'org.jmol.viewer.NucleicMonomer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly sbyte[] interestingNucleicAtomIDs;

        public static new Monomer validateAndAllocate(Chain chain, string group3, int seqcode, int firstAtomIndex, int lastAtomIndex, int[] specialAtomIndexes, Atom[] atoms)
		{
			sbyte[] offsets = scanForOffsets(firstAtomIndex, specialAtomIndexes, interestingNucleicAtomIDs);
			
			if (offsets == null)
				return null;
			NucleicMonomer nucleicMonomer = new NucleicMonomer(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, offsets);
			return nucleicMonomer;
		}
		
		////////////////////////////////////////////////////////////////

        public NucleicMonomer(Chain chain, string group3, int seqcode, int firstAtomIndex, int lastAtomIndex, sbyte[] offsets)
            : base(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, offsets)
		{
			if (offsets[0] == - 1)
			{
				sbyte leadOffset = offsets[20];
				if (leadOffset == - 1)
					leadOffset = offsets[21];
				if (leadOffset == - 1)
					leadOffset = offsets[22];
				offsets[0] = leadOffset;
			}
			this.hasRnaO2Prime = offsets[2] != - 1;
			this.isPyrimidine_Renamed_Field = offsets[9] != - 1;
			this.isPurine_Renamed_Field = offsets[10] != - 1 && offsets[11] != - 1 && offsets[12] != - 1;
			/*
			System.out.println("NucleicMonomer(" + this + ")" +
			" hasRnaO2Prime=" + hasRnaO2Prime +
			" isPyrimidine=" + isPyrimidine +
			" isPurine=" + isPurine +
			" offsets[3]=" + offsets[3]);
			*/
		}

        public bool hasRnaO2Prime;
        public bool isPurine_Renamed_Field;
        public bool isPyrimidine_Renamed_Field;

        public virtual bool isNucleicMonomer()
		{
			return true;
		}

        public override Atom getAtom(sbyte specialAtomID)
		{
			return getSpecialAtom(interestingNucleicAtomIDs, specialAtomID);
		}

        public override Point3f getAtomPoint(sbyte specialAtomID)
		{
			return getSpecialAtomPoint(interestingNucleicAtomIDs, specialAtomID);
		}

        public virtual void getBaseRing6Points(Point3f[] ring6Points)
		{
			for (int i = 6; --i >= 0; )
			{
				Atom atom = getAtomFromOffsetIndex(i + 3);
				atom.formalChargeAndFlags |= Atom.VISIBLE_FLAG;
				ring6Points[i] = atom.point3f;
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ring5OffsetIndexes'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly sbyte[] ring5OffsetIndexes = new sbyte[] { 3, 10, 11, 12, 8 };

        public virtual bool maybeGetBaseRing5Points(Point3f[] ring5Points)
		{
			if (isPurine_Renamed_Field)
				for (int i = 5; --i >= 0; )
				{
					Atom atom = getAtomFromOffsetIndex(ring5OffsetIndexes[i]);
					atom.formalChargeAndFlags |= Atom.VISIBLE_FLAG;
					ring5Points[i] = atom.point3f;
				}
			return isPurine_Renamed_Field;
		}
		
		////////////////////////////////////////////////////////////////

        public override bool isConnectedAfter(Monomer possiblyPreviousMonomer)
		{
			if (possiblyPreviousMonomer == null)
				return true;
			Atom myPhosphorusAtom = PhosphorusAtom;
			if (myPhosphorusAtom == null)
				return false;
			if (!(possiblyPreviousMonomer is NucleicMonomer))
				return false;
			NucleicMonomer other = (NucleicMonomer) possiblyPreviousMonomer;
			return other.O3PrimeAtom.isBonded(myPhosphorusAtom);
		}
		
		////////////////////////////////////////////////////////////////
		
        //internal override void  findNearestAtomIndex(int x, int y, Closest closest, short madBegin, short madEnd)
        //{
        //    Viewer viewer = chain.frame.viewer;
        //    Atom competitor = closest.atom;
        //    Atom lead = LeadAtom;
        //    Atom o5prime = O5PrimeAtom;
        //    Atom c3prime = C3PrimeAtom;
        //    short mar = (short) (madBegin / 2);
        //    if (mar < 1900)
        //        mar = 1900;
        //    int radius = viewer.scaleToScreen(lead.ScreenZ, mar);
        //    if (radius < 4)
        //        radius = 4;
        //    if (lead.isCursorOnTop(x, y, radius, competitor) || o5prime.isCursorOnTop(x, y, radius, competitor) || c3prime.isCursorOnTop(x, y, radius, competitor))
        //        closest.atom = lead;
        //}

		static NucleicMonomer()
		{
			interestingNucleicAtomIDs = new sbyte[]{~ JmolConstants.ATOMID_NUCLEIC_PHOSPHORUS, JmolConstants.ATOMID_NUCLEIC_WING, ~ JmolConstants.ATOMID_RNA_O2PRIME, JmolConstants.ATOMID_C5, JmolConstants.ATOMID_C6, JmolConstants.ATOMID_N1, JmolConstants.ATOMID_C2, JmolConstants.ATOMID_N3, JmolConstants.ATOMID_C4, ~ JmolConstants.ATOMID_O2, ~ JmolConstants.ATOMID_N7, ~ JmolConstants.ATOMID_C8, ~ JmolConstants.ATOMID_N9, ~ JmolConstants.ATOMID_O4, ~ JmolConstants.ATOMID_O6, ~ JmolConstants.ATOMID_N4, ~ JmolConstants.ATOMID_C5M, ~ JmolConstants.ATOMID_N6, ~ JmolConstants.ATOMID_N2, ~ JmolConstants.ATOMID_S4, ~ JmolConstants.ATOMID_H5T_TERMINUS, ~ JmolConstants.ATOMID_O5T_TERMINUS, JmolConstants.ATOMID_O5_PRIME, ~ JmolConstants.ATOMID_H3T_TERMINUS, JmolConstants.ATOMID_O3_PRIME, ~ JmolConstants.ATOMID_NUCLEIC_PHOSPHORUS, JmolConstants.ATOMID_C3_PRIME};
		}
	}
}