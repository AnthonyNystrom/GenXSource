/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
using System.Collections;

namespace Org.Jmol.Viewer
{
	sealed class Chain
	{
        public int GroupCount
		{
            get { return groupCount; }
		}

        public int SelectedGroupCount
		{
			/*
			void selectSeqcodeRange(int seqcodeA, int seqcodeB, BitSet bs) {
			int indexA = getSeqcodeIndex(seqcodeA);
			if (indexA < 0)
			return;
			int indexB = getSeqcodeIndex(seqcodeB);
			if (indexB < 0)
			return;
			if (indexA > indexB) {
			int t = indexA;
			indexA = indexB;
			indexB = t;
			}
			for (int i = indexA; i <= indexB; ++i)
			groups[i].selectAtoms(bs);
			}
			
			int getSeqcodeIndex(int seqcode) {
			int i;
			for (i = groupCount; --i >= 0 && groups[i].seqcode != seqcode; )
			{}
			return i;
			}
			
			*/
			
			get { return selectedGroupCount; }
		}

        public Frame frame;
        public Model model;
        public char chainID;
        public int groupCount;
        public int selectedGroupCount;
        public BitArray bsSelectedGroups;
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsNull '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Collections.BitArray bsNull = new System.Collections.BitArray(64);
		internal Group[] groups = new Group[16];
		
		//  private Group[] mainchain;

        public Chain(Frame frame, Model model, char chainID)
		{
            this.frame = frame;
			this.model = model;
			this.chainID = chainID;
		}

        public void freeze()
		{
			groups = (Group[]) Util.setLength(groups, groupCount);
		}

        public void addGroup(Group group)
		{
			if (groupCount == groups.Length)
				groups = (Group[]) Util.doubleLength(groups);
			groups[groupCount++] = group;
		}

        public Group getGroup(int groupIndex)
		{
			return groups[groupIndex];
		}

        public void selectAtoms(BitArray bs)
		{
            Frame frame = model.mmset.frame;
            Atom[] atoms = frame.Atoms;
            for (int i = frame.AtomCount; --i >= 0; )
            {
                Atom atom = atoms[i];
                if (atom.Chain == this)
                    SupportClass.BitArraySupport.Set(bs, i);
            }
		}

        public void calcSelectedGroupsCount(BitArray bsSelected)
		{
			selectedGroupCount = 0;
			if (bsSelectedGroups == null)
				bsSelectedGroups = new System.Collections.BitArray(64);
			else
			{
				//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
				bsSelectedGroups.And(bsNull);
			}
			for (int i = groupCount; --i >= 0; )
			{
				if (groups[i].isSelected(bsSelected))
				{
					++selectedGroupCount;
					SupportClass.BitArraySupport.Set(bsSelectedGroups, i);
				}
			}
		}

        public void selectSeqcodeRange(int seqcodeA, int seqcodeB, BitArray bs)
		{
			int i = 0;
			do 
			{
				int groupIndexA = getNextSeqcodeIndex(i, seqcodeA);
				if (groupIndexA < 0)
					return ;
				int groupIndexB = getNextSeqcodeIndex(i, seqcodeB);
				if (groupIndexB < 0)
					return ;
				int indexFirst;
				int indexLast;
				if (groupIndexA <= groupIndexB)
				{
					indexFirst = groupIndexA;
					indexLast = groupIndexB;
				}
				else
				{
					indexFirst = groupIndexB;
					indexLast = groupIndexA;
				}
				for (i = indexFirst; i <= indexLast; ++i)
					groups[i].selectAtoms(bs);
			}
			while (i < groupCount);
		}

        public int getNextSeqcodeIndex(int iStart, int seqcode)
		{
			for (int i = iStart; i < groupCount; ++i)
				if (groups[i].seqcode == seqcode)
					return i;
			return - 1;
		}

        public int getSelectedGroupIndex(Group group)
		{
			int selectedGroupIndex = 0;
			for (int i = 0; i < groupCount; ++i)
			{
				if (bsSelectedGroups.Get(i))
				{
					if (groups[i] == group)
						return selectedGroupIndex;
					++selectedGroupIndex;
				}
			}
			return - 1;
		}
	}
}