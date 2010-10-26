/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-02 21:10:27 +0200 (dim., 02 avr. 2006) $
* $Revision: 4881 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
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
	abstract class Polymer
	{
		public int[] LeadAtomIndices
		{
			get
			{
				if (atomIndices == null)
				{
					atomIndices = new int[monomerCount];
					for (int i = monomerCount; --i >= 0; )
						atomIndices[i] = monomers[i].LeadAtomIndex;
				}
				return atomIndices;
			}
		}

		public virtual Point3f InitiatorPoint
		{
			get
			{
				return monomers[0].InitiatorAtom.point3f;
			}
		}

		public virtual Point3f TerminatorPoint
		{
			get
			{
				return monomers[monomerCount - 1].TerminatorAtom.point3f;
			}
		}

		public virtual bool Nucleic
		{
			/*
			boolean isProtein() { return monomers[0].isProtein(); }
			return false;
			}
			*/
			
			get
			{
				return monomers[0].Nucleic;
			}
		}

		public virtual Point3f[] LeadMidpoints
		{
			get
			{
				if (leadMidpoints == null)
					calcLeadMidpointsAndWingVectors();
				return leadMidpoints;
			}
		}

		public virtual Vector3f[] WingVectors
		{
			get
			{
				if (leadMidpoints == null)
				// this is correct ... test on leadMidpoints
					calcLeadMidpointsAndWingVectors();
				return wingVectors; // wingVectors might be null ... before autocalc
			}
		}
		
		internal Model model;
		internal Monomer[] monomers;
		internal int monomerCount;
		
		private int[] atomIndices;
		
		public Polymer(Monomer[] monomers)
		{
			this.monomers = monomers;
			this.monomerCount = monomers.Length;
			for (int i = monomerCount; --i >= 0; )
				monomers[i].Polymer = this;
			model = monomers[0].chain.model;
			model.addPolymer(this);
		}
		
		// these arrays will be one longer than the polymerCount
		// we probably should have better names for these things
		// holds center points between alpha carbons or sugar phosphoruses
		internal Point3f[] leadMidpoints;
		// holds the vector that runs across the 'ribbon'
		internal Vector3f[] wingVectors;
		
		public static Polymer allocatePolymer(Group[] groups, int firstGroupIndex)
		{
			//    System.out.println("allocatePolymer()");
			Monomer[] monomers;
			monomers = getAminoMonomers(groups, firstGroupIndex);
			if (monomers != null)
			{
				//      System.out.println("an AminoPolymer");
				return new AminoPolymer(monomers);
			}
			monomers = getAlphaMonomers(groups, firstGroupIndex);
			if (monomers != null)
			{
				//      System.out.println("an AlphaPolymer");
				return new AlphaPolymer(monomers);
			}
			monomers = getNucleicMonomers(groups, firstGroupIndex);
			if (monomers != null)
			{
				//      System.out.println("a NucleicPolymer");
				return new NucleicPolymer(monomers);
			}
			monomers = getPhosphorusMonomers(groups, firstGroupIndex);
			if (monomers != null)
			{
				//      System.out.println("an AlphaPolymer");
				return new PhosphorusPolymer(monomers);
			}
			System.Console.Out.WriteLine("Polymer.allocatePolymer() ... why am I here?");
			throw new System.NullReferenceException();
		}
		
		public static Monomer[] getAlphaMonomers(Group[] groups, int firstGroupIndex)
		{
			AlphaMonomer previous = null;
			int count = 0;
			for (int i = firstGroupIndex; i < groups.Length; ++i, ++count)
			{
				Group group = groups[i];
				if (!(group is AlphaMonomer))
					break;
				AlphaMonomer current = (AlphaMonomer) group;
				if (current.polymer != null)
					break;
				if (!current.isConnectedAfter(previous))
					break;
				previous = current;
			}
			if (count == 0)
				return null;
			Monomer[] monomers = new Monomer[count];
			for (int j = 0; j < count; ++j)
				monomers[j] = (AlphaMonomer) groups[firstGroupIndex + j];
			return monomers;
		}
		
		public static Monomer[] getAminoMonomers(Group[] groups, int firstGroupIndex)
		{
			AminoMonomer previous = null;
			int count = 0;
			for (int i = firstGroupIndex; i < groups.Length; ++i, ++count)
			{
				Group group = groups[i];
				if (!(group is AminoMonomer))
					break;
				AminoMonomer current = (AminoMonomer) group;
				if (current.polymer != null)
					break;
				if (!current.isConnectedAfter(previous))
					break;
				previous = current;
			}
			if (count == 0)
				return null;
			Monomer[] monomers = new Monomer[count];
			for (int j = 0; j < count; ++j)
				monomers[j] = (AminoMonomer) groups[firstGroupIndex + j];
			return monomers;
		}
		
		public static Monomer[] getPhosphorusMonomers(Group[] groups, int firstGroupIndex)
		{
			PhosphorusMonomer previous = null;
			int count = 0;
			for (int i = firstGroupIndex; i < groups.Length; ++i, ++count)
			{
				Group group = groups[i];
				if (!(group is PhosphorusMonomer))
					break;
				PhosphorusMonomer current = (PhosphorusMonomer) group;
				if (current.polymer != null)
					break;
				if (!current.isConnectedAfter(previous))
					break;
				previous = current;
			}
			if (count == 0)
				return null;
			Monomer[] monomers = new Monomer[count];
			for (int j = 0; j < count; ++j)
				monomers[j] = (PhosphorusMonomer) groups[firstGroupIndex + j];
			return monomers;
		}
		
		public static Monomer[] getNucleicMonomers(Group[] groups, int firstGroupIndex)
		{
			NucleicMonomer previous = null;
			int count = 0;
			for (int i = firstGroupIndex; i < groups.Length; ++i, ++count)
			{
				Group group = groups[i];
				if (!(group is NucleicMonomer))
					break;
				NucleicMonomer current = (NucleicMonomer) group;
				if (current.polymer != null)
					break;
				if (!current.isConnectedAfter(previous))
					break;
				previous = current;
			}
			if (count == 0)
				return null;
			Monomer[] monomers = new Monomer[count];
			for (int j = 0; j < count; ++j)
				monomers[j] = (NucleicMonomer) groups[firstGroupIndex + j];
			return monomers;
		}
		
		public virtual int getIndex(Monomer monomer)
		{
			int i;
			for (i = monomerCount; --i >= 0; )
				if (monomers[i] == monomer)
					break;
			return i;
		}
		
		public virtual int getIndex(char chainID, int seqcode)
		{
			int i;
			for (i = monomerCount; --i >= 0; )
				if (monomers[i].seqcode == seqcode && monomers[i].chain.chainID == chainID)
					break;
			return i;
		}
		
		public Point3f getLeadPoint(int monomerIndex)
		{
			return monomers[monomerIndex].LeadAtomPoint;
		}
		
		public Atom getLeadAtom(int monomerIndex)
		{
			return monomers[monomerIndex].LeadAtom;
		}
		
		public virtual void getLeadMidPoint(int groupIndex, Point3f midPoint)
		{
			if (groupIndex == monomerCount)
			{
				--groupIndex;
			}
			else if (groupIndex > 0)
			{
				midPoint.set_Renamed(getLeadPoint(groupIndex));
				midPoint.add(getLeadPoint(groupIndex - 1));
				midPoint.scale(0.5f);
				return ;
			}
			midPoint.set_Renamed(getLeadPoint(groupIndex));
		}
		
		public virtual bool hasWingPoints()
		{
			return false;
		}
		
		// this might change in the future ... if we calculate a wing point
		// without an atom for an AlphaPolymer
		public Point3f getWingPoint(int polymerIndex)
		{
			return monomers[polymerIndex].WingAtomPoint;
		}

        public virtual void addSecondaryStructure(sbyte type, char startChainID, int startSeqcode, char endChainID, int endSeqcode)
		{ }

        public virtual void calculateStructures()
		{ }

        public virtual void calcHydrogenBonds(System.Collections.BitArray bsA, System.Collections.BitArray bsB)
		{
			// subclasses should override if they know how to calculate hbonds
		}

        public void calcLeadMidpointsAndWingVectors()
		{
			//    System.out.println("Polymer.calcLeadMidpointsAndWingVectors");
			int count = monomerCount;
			leadMidpoints = new Point3f[count + 1];
			wingVectors = new Vector3f[count + 1];
			bool _hasWingPoints = hasWingPoints();
			
			Vector3f vectorA = new Vector3f();
			Vector3f vectorB = new Vector3f();
			Vector3f vectorC = new Vector3f();
			Vector3f vectorD = new Vector3f();
			
			Point3f leadPointPrev, leadPoint;
			leadMidpoints[0] = InitiatorPoint;
			leadPoint = getLeadPoint(0);
			Vector3f previousVectorD = null;
			for (int i = 1; i < count; ++i)
			{
				leadPointPrev = leadPoint;
				leadPoint = getLeadPoint(i);
				Point3f midpoint = new Point3f(leadPoint);
				midpoint.add(leadPointPrev);
				midpoint.scale(0.5f);
				leadMidpoints[i] = midpoint;
				if (_hasWingPoints)
				{
					vectorA.sub(leadPoint, leadPointPrev);
					vectorB.sub(leadPointPrev, getWingPoint(i - 1));
					vectorC.cross(vectorA, vectorB);
					vectorD.cross(vectorA, vectorC);
					vectorD.normalize();
					if (previousVectorD != null && previousVectorD.angle(vectorD) > System.Math.PI / 2)
						vectorD.scale(- 1);
					previousVectorD = wingVectors[i] = new Vector3f(vectorD);
				}
			}
			leadMidpoints[count] = TerminatorPoint;
			if (!_hasWingPoints)
			{
				if (count < 3)
				{
					wingVectors[1] = unitVectorX;
				}
				else
				{
					// auto-calculate wing vectors based upon lead atom positions only
					// seems to work like a charm! :-)
					Point3f next, current, prev;
					prev = leadMidpoints[0];
					current = leadMidpoints[1];
					Vector3f previousVectorC = null;
					for (int i = 1; i < count; ++i)
					{
						next = leadMidpoints[i + 1];
						vectorA.sub(prev, current);
						vectorB.sub(next, current);
						vectorC.cross(vectorA, vectorB);
						vectorC.normalize();
						if (previousVectorC != null && previousVectorC.angle(vectorC) > System.Math.PI / 2)
							vectorC.scale(- 1);
						previousVectorC = wingVectors[i] = new Vector3f(vectorC);
						prev = current;
						current = next;
					}
				}
			}
			wingVectors[0] = wingVectors[1];
			wingVectors[count] = wingVectors[count - 1];
			
			/*
			for (int i = 0; i < wingVectors.length; ++i) {
			if (wingVectors[i] == null) {
			System.out.println("que? wingVectors[" + i + "] == null?");
			System.out.println("hasWingPoints=" + hasWingPoints +
			" wingVectors.length=" + wingVectors.length +
			" count=" + count);
			
			}
			else if (Float.isNaN(wingVectors[i].x)) {
			System.out.println("wingVectors[" + i + "]=" + wingVectors[i]);
			}
			}
			*/
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitVectorX '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f unitVectorX = new Vector3f(1, 0, 0);
		
        //internal virtual void  findNearestAtomIndex(int xMouse, int yMouse, Closest closest, short[] mads)
        //{
        //    for (int i = monomerCount; --i >= 0; )
        //    {
        //        if (mads[i] > 0 || mads[i + 1] > 0)
        //            monomers[i].findNearestAtomIndex(xMouse, yMouse, closest, mads[i], mads[i + 1]);
        //    }
        //}

        public int selectedMonomerCount;
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsNull '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Collections.BitArray bsNull = new System.Collections.BitArray(64);
        public System.Collections.BitArray bsSelectedMonomers;

        public virtual void calcSelectedMonomersCount(System.Collections.BitArray bsSelected)
		{
			selectedMonomerCount = 0;
			if (bsSelectedMonomers == null)
				bsSelectedMonomers = new System.Collections.BitArray(64);
			else
			{
				//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
				bsSelectedMonomers.And(bsNull);
			}
			for (int i = monomerCount; --i >= 0; )
			{
				if (monomers[i].isSelected(bsSelected))
				{
					++selectedMonomerCount;
					SupportClass.BitArraySupport.Set(bsSelectedMonomers, i);
				}
			}
		}

        public virtual int getSelectedMonomerIndex(Monomer monomer)
		{
			int selectedMonomerIndex = 0;
			for (int i = 0; i < monomerCount; ++i)
			{
				if (bsSelectedMonomers.Get(i))
				{
					if (monomers[i] == monomer)
						return selectedMonomerIndex;
					++selectedMonomerIndex;
				}
			}
			return - 1;
		}
	}
}