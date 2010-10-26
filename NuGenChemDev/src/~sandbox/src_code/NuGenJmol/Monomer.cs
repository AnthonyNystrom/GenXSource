/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 18:09:51 +0200 (lun., 27 mars 2006) $
* $Revision: 4780 $
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
	abstract class Monomer : Group
	{
		public virtual Polymer Polymer
		{
			set { this.polymer = value; }
		}

		public override int PolymerLength
		{
			get { return polymer == null?0:polymer.monomerCount; }
		}

		public override int PolymerIndex
		{
            get { return polymer == null ? -1 : polymer.getIndex(this); }
		}

		public override bool Dna
		{
			get { return false; }
		}

		public override bool Rna
		{
            get { return false; }
		}

        public override bool Protein
        {
            get { return this is AlphaMonomer; }
        }

		public override bool Nucleic
		{
            get { return this is PhosphorusMonomer; }
		}

		public virtual ProteinStructure Structure
		{
			////////////////////////////////////////////////////////////////
			set { }
		}
		public virtual ProteinStructure ProteinStructure
		{
            get { return null; }
		}

		public override sbyte ProteinStructureType
		{
            get { return 0; }
		}

		public virtual bool Helix
		{
            get { return false; }
		}

		public virtual bool HelixOrSheet
		{
            get { return false; }
		}

		public virtual int LeadAtomIndex
		{
            get { return firstAtomIndex + (offsets[0] & 0xFF); }
		}

		public virtual Atom LeadAtom
		{
            get { return getAtomFromOffsetIndex(0); }
		}

		public virtual Point3f LeadAtomPoint
		{
            get { return getAtomPointFromOffsetIndex(0); }
		}

		public virtual Atom WingAtom
		{
            get { return getAtomFromOffsetIndex(1); }
		}

		public virtual Point3f WingAtomPoint
		{
            get { return getAtomPointFromOffsetIndex(1); }
		}

		public virtual Atom InitiatorAtom
		{
            get { return LeadAtom; }
		}

		public virtual Atom TerminatorAtom
		{
            get { return LeadAtom; }
		}

        public Polymer polymer;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'offsets '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public sbyte[] offsets;

        public Monomer(Chain chain, string group3, int seqcode, int firstAtomIndex, int lastAtomIndex, sbyte[] interestingAtomOffsets)
            : base(chain, group3, seqcode, firstAtomIndex, lastAtomIndex)
		{
			offsets = interestingAtomOffsets;
		}
		
		////////////////////////////////////////////////////////////////

        public static sbyte[] scanForOffsets(int firstAtomIndex, int[] specialAtomIndexes, sbyte[] interestingAtomIDs)
		{
			int interestingCount = interestingAtomIDs.Length;
			sbyte[] offsets = new sbyte[interestingCount];
			for (int i = interestingCount; --i >= 0; )
			{
				int atomIndex;
				int atomID = interestingAtomIDs[i];
				// mth 2004 06 09
				// use ~ instead of - as the optional indicator
				// because I got hosed by a missing comma
				// in an interestingAtomIDs table
				if (atomID < 0)
					atomIndex = specialAtomIndexes[~ atomID]; // optional
				else
				{
					atomIndex = specialAtomIndexes[atomID]; // required
					if (atomIndex < 0)
						return null;
				}
				int offset;
				if (atomIndex < 0)
					offset = 255;
				else
				{
					offset = atomIndex - firstAtomIndex;
					if (offset < 0 || offset > 254)
					{
						System.Console.Out.WriteLine("scanForOffsets i=" + i + " atomID=" + atomID + " atomIndex:" + atomIndex + " firstAtomIndex:" + firstAtomIndex + " offset out of 0-254 range. Groups aren't organized correctly. Is this really a protein?: " + offset);
						if (atomID < 0)
						{
							offset = 255; //it was optional anyway RMH
						}
						else
						{
							//throw new NullPointerException();
						}
					}
				}
				offsets[i] = (sbyte) offset;
			}
			return offsets;
		}
		
		////////////////////////////////////////////////////////////////

        public Atom getAtomFromOffset(sbyte offset)
		{
			if (offset == - 1)
				return null;
            return chain.frame.atoms[firstAtomIndex + (offset & 0xFF)];
		}

        public Point3f getAtomPointFromOffset(sbyte offset)
		{
			if (offset == - 1)
				return null;
            return chain.frame.atoms[firstAtomIndex + (offset & 0xFF)].point3f;
		}
		
		////////////////////////////////////////////////////////////////

        public Atom getAtomFromOffsetIndex(int offsetIndex)
		{
			if (offsetIndex > offsets.Length)
				return null;
			int offset = offsets[offsetIndex] & 0xFF;
			if (offset == 255)
				return null;
            return chain.frame.atoms[firstAtomIndex + offset];
		}

        public Point3f getAtomPointFromOffsetIndex(int offsetIndex)
		{
			Atom atom = getAtomFromOffsetIndex(offsetIndex);
			return atom == null?null:atom.point3f;
		}

        public Atom getSpecialAtom(sbyte[] interestingIDs, sbyte specialAtomID)
		{
			for (int i = interestingIDs.Length; --i >= 0; )
			{
				int interestingID = interestingIDs[i];
				if (interestingID < 0)
					interestingID = - interestingID;
				if (specialAtomID == interestingID)
				{
					int offset = offsets[i] & 0xFF;
					if (offset == 255)
						return null;
                    return chain.frame.atoms[firstAtomIndex + offset];
				}
			}
			return null;
		}

        public Point3f getSpecialAtomPoint(sbyte[] interestingIDs, sbyte specialAtomID)
		{
			for (int i = interestingIDs.Length; --i >= 0; )
			{
				int interestingID = interestingIDs[i];
				if (interestingID < 0)
					interestingID = - interestingID;
				if (specialAtomID == interestingID)
				{
					int offset = offsets[i] & 0xFF;
					if (offset == 255)
						return null;
                    return chain.frame.atoms[firstAtomIndex + offset].point3f;
				}
			}
			return null;
		}

        public virtual Atom getAtom(sbyte specialAtomID)
		{
			return null;
		}

        public virtual Point3f getAtomPoint(sbyte specialAtomID)
		{
			return null;
		}

        public abstract bool isConnectedAfter(Monomer possiblyPreviousMonomer);
		
		/// <summary> Selects LeadAtom when this Monomer is clicked iff it is
		/// closer to the user.
		/// 
		/// </summary>
		/// <param name="x">
		/// </param>
		/// <param name="y">
		/// </param>
		/// <param name="closest">
		/// </param>
		/// <param name="madBegin">
		/// </param>
		/// <param name="madEnd">
		/// </param>
        //internal virtual void  findNearestAtomIndex(int x, int y, Closest closest, short madBegin, short madEnd)
        //{
        //}
	}
}