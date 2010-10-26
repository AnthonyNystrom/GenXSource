/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 18:30:39 +0200 (lun., 27 mars 2006) $
* $Revision: 4782 $
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
	class Group
	{
		public virtual string Group1
		{
			get
			{
				if (groupID >= JmolConstants.predefinedGroup1Names.Length)
					return "?";
				return JmolConstants.predefinedGroup1Names[groupID];
			}
		}

		public virtual char ChainID
		{
			get { return chain.chainID; }
		}

		public virtual int PolymerLength
		{
			get { return 0; }
		}

		public virtual int PolymerIndex
		{
			get { return - 1; }
		}

		public virtual sbyte ProteinStructureType
		{
			get { return JmolConstants.PROTEIN_STRUCTURE_NONE; }
		}

		public virtual bool Protein
		{
			get { return false; }
		}

		public virtual bool Nucleic
		{
			get { return false; }
		}

		public virtual bool Dna
		{
			get { return false; }
		}

		public virtual bool Rna
		{
			get { return false; }
		}

		public virtual bool Purine
		{
            get { return false; }
		}

		public virtual bool Pyrimidine
		{
			get { return false; }
		}

		public virtual int Resno
		{
			////////////////////////////////////////////////////////////////
			// seqcode stuff
			////////////////////////////////////////////////////////////////
			get { return seqcode >> 8; }
		}

		public virtual bool Hetero
		{
			get { // just look at the first atom of the group
                return chain.frame.atoms[firstAtomIndex].Hetero;
            }
        }
		
		internal Chain chain;
		internal int seqcode;
		internal short groupID;
		internal int firstAtomIndex = - 1;
		internal int lastAtomIndex;

        public Group(Chain chain, string group3, int seqcode, int firstAtomIndex, int lastAtomIndex)
		{
			this.chain = chain;
			this.seqcode = seqcode;
			
			if (group3 == null)
				group3 = "";
			this.groupID = getGroupID(group3);
			this.firstAtomIndex = firstAtomIndex;
			this.lastAtomIndex = lastAtomIndex;
		}

        public bool isGroup3(string group3)
        {
            return group3Names[groupID].ToUpper().Equals(group3.ToUpper());
        }

        public string Group3
		{
            get { return group3Names[groupID]; }
		}

        public static string getGroup3(short groupID)
		{
			return group3Names[groupID];
		}

        public short GroupID
		{
            get { return groupID; }
		}

        public bool isGroup3Match(string strWildcard)
		{
			int cchWildcard = strWildcard.Length;
			int ichWildcard = 0;
			string group3 = group3Names[groupID];
			int cchGroup3 = group3.Length;
			if (cchWildcard < cchGroup3)
				return false;
			while (cchWildcard > cchGroup3)
			{
				// wildcard is too long
				// so strip '?' from the beginning and the end, if possible
				if (strWildcard[ichWildcard] == '?')
				{
					++ichWildcard;
				}
				else if (strWildcard[ichWildcard + cchWildcard - 1] != '?')
				{
					return false;
				}
				--cchWildcard;
			}
			for (int i = cchGroup3; --i >= 0; )
			{
				char charWild = strWildcard[ichWildcard + i];
				if (charWild == '?')
					continue;
				if (charWild != group3[i])
					return false;
			}
			return true;
		}
		
		////////////////////////////////////////////////////////////////
		// static stuff for group ids
		////////////////////////////////////////////////////////////////
		
		private static Hashtable htGroup = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

        public static string[] group3Names = new string[128];
        public static short group3NameCount = 0;
		
		public static short addGroup3Name(string group3)
		{
			lock (typeof(Group))
			{
				if (group3NameCount == group3Names.Length)
					group3Names = Util.doubleLength(group3Names);
				short groupID = group3NameCount++;
				group3Names[groupID] = group3;
				htGroup[group3] = (short) groupID;
				return groupID;
			}
		}

        public static short getGroupID(string group3)
		{
			if (group3 == null)
				return - 1;
			short groupID = lookupGroupID(group3);
			return (groupID != - 1)?groupID:addGroup3Name(group3);
		}

        public static short lookupGroupID(string group3)
		{
			if (group3 != null)
			{
				System.Int16 boxedGroupID = (System.Int16) htGroup[group3];
				//UPGRADE_TODO: The 'System.Int16' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (boxedGroupID != null)
					return (short) boxedGroupID;
			}
			return - 1;
		}

        public int Seqcode
		{
            get { return seqcode; }
		}

        public string SeqcodeString
		{
            get { return getSeqcodeString(seqcode); }
		}

        public static int getSeqcode(int sequenceNumber, char insertionCode)
		{
			if (sequenceNumber == System.Int32.MinValue)
				return sequenceNumber;
			if (!((insertionCode >= 'A' && insertionCode <= 'Z') || (insertionCode >= 'a' && insertionCode <= 'z') || (insertionCode >= '0' && insertionCode <= '9')))
			{
				if (insertionCode != ' ' && insertionCode != '\x0000')
					System.Console.Out.WriteLine("unrecognized insertionCode:" + insertionCode);
				insertionCode = '\x0000';
			}
			return (sequenceNumber << 8) + insertionCode;
		}

        public static string getSeqcodeString(int seqcode)
		{
			if (seqcode == System.Int32.MinValue)
				return null;
			return (seqcode & 0xFF) == 0?"" + (seqcode >> 8):"" + (seqcode >> 8) + '^' + (char) (seqcode & 0xFF);
		}

        public void selectAtoms(BitArray bs)
		{
			for (int i = firstAtomIndex; i <= lastAtomIndex; ++i)
				SupportClass.BitArraySupport.Set(bs, i);
		}

        public virtual bool isSelected(BitArray bs)
		{
			for (int i = firstAtomIndex; i <= lastAtomIndex; ++i)
				if (bs.Get(i))
					return true;
			return false;
		}
		
		public override string ToString()
		{
			return "[" + Group3 + "-" + SeqcodeString + "]";
		}

		static Group()
		{
			{
				for (int i = 0; i < JmolConstants.predefinedGroup3Names.Length; ++i)
				{
					addGroup3Name(JmolConstants.predefinedGroup3Names[i]);
				}
			}
		}
	}
}