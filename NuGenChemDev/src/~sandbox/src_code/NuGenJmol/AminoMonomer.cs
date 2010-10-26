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
	class AminoMonomer : AlphaMonomer
	{
		public virtual Atom NitrogenAtom
		{
			get {return getAtomFromOffsetIndex(2); }
		}

		public virtual Point3f NitrogenAtomPoint
		{
            get { return getAtomPointFromOffsetIndex(2); }
		}

		public virtual Atom CarbonylCarbonAtom
		{
            get { return getAtomFromOffsetIndex(3); }
		}

		public virtual Point3f CarbonylCarbonAtomPoint
		{
            get { return getAtomPointFromOffsetIndex(3); }
		}

		public virtual Atom CarbonylOxygenAtom
		{
            get { return WingAtom; }
		}

		public virtual Point3f CarbonylOxygenAtomPoint
		{
            get { return WingAtomPoint; }
		}

		public override Atom InitiatorAtom
		{
            get { return NitrogenAtom; }
		}

		public override Atom TerminatorAtom
		{
            get { return getAtomFromOffsetIndex(offsets[4] != -1 ? 4 : 3); }
		}
		
		// negative values are optional
		//UPGRADE_NOTE: Final was removed from the declaration of 'interestingAminoAtomIDs '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'interestingAminoAtomIDs' was moved to static method 'org.jmol.viewer.AminoMonomer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly sbyte[] interestingAminoAtomIDs;

        public static new Monomer validateAndAllocate(Chain chain, string group3, int seqcode, int firstAtomIndex, int lastAtomIndex, int[] specialAtomIndexes, Atom[] atoms)
		{
			sbyte[] offsets = scanForOffsets(firstAtomIndex, specialAtomIndexes, interestingAminoAtomIDs);
			if (offsets == null)
				return null;
			if (specialAtomIndexes[JmolConstants.ATOMID_CARBONYL_OXYGEN] < 0)
			{
				int carbonylOxygenIndex = specialAtomIndexes[JmolConstants.ATOMID_O1];
				System.Console.Out.WriteLine("I see someone who does not have a carbonyl oxygen");
				if (carbonylOxygenIndex < 0)
					return null;
				offsets[1] = (sbyte) (carbonylOxygenIndex - firstAtomIndex);
			}
			if (!isBondedCorrectly(firstAtomIndex, offsets, atoms))
				return null;
			AminoMonomer aminoMonomer = new AminoMonomer(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, offsets);
			return aminoMonomer;
		}

        public static bool isBondedCorrectly(int offset1, int offset2, int firstAtomIndex, sbyte[] offsets, Atom[] atoms)
		{
			int atomIndex1 = firstAtomIndex + (offsets[offset1] & 0xFF);
			int atomIndex2 = firstAtomIndex + (offsets[offset2] & 0xFF);
			/*
			System.out.println("isBondedCorrectly() " +
			" atomIndex1=" + atomIndex1 +
			" atomIndex2=" + atomIndex2);
			*/
			if (atomIndex1 >= atomIndex2)
				return false;
			return atoms[atomIndex1].isBonded(atoms[atomIndex2]);
		}

        public static bool isBondedCorrectly(int firstAtomIndex, sbyte[] offsets, Atom[] atoms)
		{
			return (isBondedCorrectly(2, 0, firstAtomIndex, offsets, atoms) && isBondedCorrectly(0, 3, firstAtomIndex, offsets, atoms) && isBondedCorrectly(3, 1, firstAtomIndex, offsets, atoms));
		}
		
		////////////////////////////////////////////////////////////////

        public AminoMonomer(Chain chain, string group3, int seqcode, int firstAtomIndex, int lastAtomIndex, sbyte[] offsets)
            : base(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, offsets)
		{ }

        public virtual bool isAminoMonomer()
		{
			return true;
		}
		
		////////////////////////////////////////////////////////////////

        public override Atom getAtom(sbyte specialAtomID)
		{
			return getSpecialAtom(interestingAminoAtomIDs, specialAtomID);
		}

        public override Point3f getAtomPoint(sbyte specialAtomID)
		{
			return getSpecialAtomPoint(interestingAminoAtomIDs, specialAtomID);
		}
		
		////////////////////////////////////////////////////////////////

        public override bool isConnectedAfter(Monomer possiblyPreviousMonomer)
		{
			if (possiblyPreviousMonomer == null)
				return true;
			if (!(possiblyPreviousMonomer is AminoMonomer))
				return false;
			AminoMonomer other = (AminoMonomer) possiblyPreviousMonomer;
			return other.CarbonylCarbonAtom.isBonded(NitrogenAtom);
		}
		
		////////////////////////////////////////////////////////////////
		
        //internal override void  findNearestAtomIndex(int x, int y, Closest closest, short madBegin, short madEnd)
        //{
        //    Viewer viewer = chain.frame.viewer;
        //    Atom competitor = closest.atom;
        //    Atom nitrogen = NitrogenAtom;
        //    short marBegin = (short) (madBegin / 2);
        //    if (marBegin < 1200)
        //        marBegin = 1200;
        //    int radiusBegin = viewer.scaleToScreen(nitrogen.ScreenZ, marBegin);
        //    if (radiusBegin < 4)
        //        radiusBegin = 4;
        //    Atom ccarbon = CarbonylCarbonAtom;
        //    short marEnd = (short) (madEnd / 2);
        //    if (marEnd < 1200)
        //        marEnd = 1200;
        //    int radiusEnd = viewer.scaleToScreen(nitrogen.ScreenZ, marEnd);
        //    if (radiusEnd < 4)
        //        radiusEnd = 4;
        //    Atom alpha = LeadAtom;
        //    if (alpha.isCursorOnTop(x, y, (radiusBegin + radiusEnd) / 2, competitor) || nitrogen.isCursorOnTop(x, y, radiusBegin, competitor) || ccarbon.isCursorOnTop(x, y, radiusEnd, competitor))
        //        closest.atom = alpha;
        //}

		static AminoMonomer()
		{
			interestingAminoAtomIDs = new sbyte[]{JmolConstants.ATOMID_ALPHA_CARBON, ~ JmolConstants.ATOMID_CARBONYL_OXYGEN, JmolConstants.ATOMID_AMINO_NITROGEN, JmolConstants.ATOMID_CARBONYL_CARBON, ~ JmolConstants.ATOMID_TERMINATING_OXT, ~ JmolConstants.ATOMID_O1};
		}
	}
}