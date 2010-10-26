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

namespace Org.Jmol.Viewer
{
	class AlphaPolymer:Polymer
	{
		public virtual bool Protein
		{
            get { return true; }
		}

        public AlphaPolymer(Monomer[] monomers)
            : base(monomers)
		{ }

        public override void addSecondaryStructure(sbyte type, char startChainID, int startSeqcode, char endChainID, int endSeqcode)
		{
			int indexStart, indexEnd;
			if ((indexStart = getIndex(startChainID, startSeqcode)) == - 1 || (indexEnd = getIndex(endChainID, endSeqcode)) == - 1)
				return ;
			addSecondaryStructure(type, indexStart, indexEnd);
		}

        public virtual void addSecondaryStructure(sbyte type, int indexStart, int indexEnd)
		{
			int structureCount = indexEnd - indexStart + 1;
			if (structureCount < 1)
			{
				System.Console.Out.WriteLine("structure definition error\n" + " indexStart:" + indexStart + " indexEnd:" + indexEnd);
				return ;
			}
			ProteinStructure proteinstructure = null;
			switch (type)
			{
				
				case JmolConstants.PROTEIN_STRUCTURE_HELIX: 
					proteinstructure = new Helix(this, indexStart, structureCount);
					break;
				
				case JmolConstants.PROTEIN_STRUCTURE_SHEET: 
					if (this is AminoPolymer)
						proteinstructure = new Sheet((AminoPolymer) this, indexStart, structureCount);
					break;
				
				case JmolConstants.PROTEIN_STRUCTURE_TURN: 
					proteinstructure = new Turn(this, indexStart, structureCount);
					break;
				
				default: 
					System.Console.Out.WriteLine("unrecognized secondary structure type");
					return ;
				
			}
			for (int i = indexStart; i <= indexEnd; ++i)
				monomers[i].Structure = proteinstructure;
		}

        public virtual void calcHydrogenBonds() { }
		
		/// <summary> Uses Levitt & Greer algorithm to calculate protien secondary
		/// structures using only alpha-carbon atoms.
		/// <p>
		/// Levitt and Greer <br />
		/// Automatic Identification of Secondary Structure in Globular Proteins <br />
		/// J.Mol.Biol.(1977) 114, 181-293 <br />
		/// <p>
		/// <a
		/// href='http://csb.stanford.edu/levitt/Levitt_JMB77_Secondary_structure.pdf'>
		/// http://csb.stanford.edu/levitt/Levitt_JMB77_Secondary_structure.pdf
		/// </a>
		/// </summary>
        public override void calculateStructures()
		{
			if (monomerCount < 4)
				return ;
			float[] angles = calculateAnglesInDegrees();
			sbyte[] codes = calculateCodes(angles);
			checkBetaSheetAlphaHelixOverlap(codes, angles);
			sbyte[] tags = calculateRunsFourOrMore(codes);
			extendRuns(tags);
			searchForTurns(codes, angles, tags);
			
			addStructuresFromTags(tags);
		}

        public const sbyte CODE_NADA = 0;
        public const sbyte CODE_RIGHT_HELIX = 1;
        public const sbyte CODE_BETA_SHEET = 2;
        public const sbyte CODE_LEFT_HELIX = 3;

        public const sbyte CODE_LEFT_TURN = 4;
        public const sbyte CODE_RIGHT_TURN = 5;

        public virtual float[] calculateAnglesInDegrees()
		{
			float[] angles = new float[monomerCount];
			for (int i = monomerCount - 1; --i >= 2; )
				angles[i] = Measurement.computeTorsion(monomers[i - 2].LeadAtomPoint, monomers[i - 1].LeadAtomPoint, monomers[i].LeadAtomPoint, monomers[i + 1].LeadAtomPoint);
			return angles;
		}

        public virtual sbyte[] calculateCodes(float[] angles)
		{
			sbyte[] codes = new sbyte[monomerCount];
			for (int i = monomerCount - 1; --i >= 2; )
			{
				float degrees = angles[i];
				codes[i] = ((degrees >= 10 && degrees < 120)?CODE_RIGHT_HELIX:((degrees >= 120 || degrees < - 90)?CODE_BETA_SHEET:((degrees >= - 90 && degrees < 0)?CODE_LEFT_HELIX:CODE_NADA)));
			}
			return codes;
		}

        public virtual void checkBetaSheetAlphaHelixOverlap(sbyte[] codes, float[] angles)
		{
			for (int i = monomerCount - 2; --i >= 2; )
				if (codes[i] == CODE_BETA_SHEET && angles[i] <= 140 && codes[i - 2] == CODE_RIGHT_HELIX && codes[i - 1] == CODE_RIGHT_HELIX && codes[i + 1] == CODE_RIGHT_HELIX && codes[i + 2] == CODE_RIGHT_HELIX)
					codes[i] = CODE_RIGHT_HELIX;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'TAG_NADA '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'TAG_NADA' was moved to static method 'org.jmol.viewer.AlphaPolymer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly sbyte TAG_NADA;
		//UPGRADE_NOTE: Final was removed from the declaration of 'TAG_TURN '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'TAG_TURN' was moved to static method 'org.jmol.viewer.AlphaPolymer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly sbyte TAG_TURN;
		//UPGRADE_NOTE: Final was removed from the declaration of 'TAG_SHEET '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'TAG_SHEET' was moved to static method 'org.jmol.viewer.AlphaPolymer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly sbyte TAG_SHEET;
		//UPGRADE_NOTE: Final was removed from the declaration of 'TAG_HELIX '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'TAG_HELIX' was moved to static method 'org.jmol.viewer.AlphaPolymer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly sbyte TAG_HELIX;

        public virtual sbyte[] calculateRunsFourOrMore(sbyte[] codes)
		{
			sbyte[] tags = new sbyte[monomerCount];
			sbyte tag = TAG_NADA;
			sbyte code = CODE_NADA;
			int runLength = 0;
			for (int i = 0; i < monomerCount; ++i)
			{
				// throw away the sheets ... their angle technique does not work well
				if (codes[i] == code && code != CODE_NADA && code != CODE_BETA_SHEET)
				{
					++runLength;
					if (runLength == 4)
					{
						tag = (code == CODE_BETA_SHEET?TAG_SHEET:TAG_HELIX);
						for (int j = 4; --j >= 0; )
							tags[i - j] = tag;
					}
					else if (runLength > 4)
						tags[i] = tag;
				}
				else
				{
					runLength = 1;
					code = codes[i];
				}
			}
			return tags;
		}

        public virtual void extendRuns(sbyte[] tags)
		{
			for (int i = 1; i < monomerCount - 4; ++i)
				if (tags[i] == TAG_NADA && tags[i + 1] != TAG_NADA)
					tags[i] = tags[i + 1];
			
			tags[0] = tags[1];
			tags[monomerCount - 1] = tags[monomerCount - 2];
		}

        public virtual void searchForTurns(sbyte[] codes, float[] angles, sbyte[] tags)
		{
			for (int i = monomerCount - 1; --i >= 2; )
			{
				codes[i] = CODE_NADA;
				if (tags[i] == TAG_NADA)
				{
					float angle = angles[i];
					if (angle >= - 90 && angle < 0)
						codes[i] = CODE_LEFT_TURN;
					else if (angle >= 0 && angle < 90)
						codes[i] = CODE_RIGHT_TURN;
				}
			}
			
			for (int i = monomerCount - 1; --i >= 0; )
			{
				if (codes[i] != CODE_NADA && codes[i + 1] == codes[i] && tags[i] == TAG_NADA)
					tags[i] = TAG_TURN;
			}
		}

        public virtual void addStructuresFromTags(sbyte[] tags)
		{
			int i = 0;
			while (i < monomerCount)
			{
				sbyte tag = tags[i];
				if (tag == TAG_NADA)
				{
					++i;
					continue;
				}
				int iMax;
				for (iMax = i + 1; iMax < monomerCount && tags[iMax] == tag; ++iMax)
				{
				}
				addSecondaryStructure(tag, i, iMax - 1);
				i = iMax;
			}
		}

		static AlphaPolymer()
		{
			TAG_NADA = JmolConstants.PROTEIN_STRUCTURE_NONE;
			TAG_TURN = JmolConstants.PROTEIN_STRUCTURE_TURN;
			TAG_SHEET = JmolConstants.PROTEIN_STRUCTURE_SHEET;
			TAG_HELIX = JmolConstants.PROTEIN_STRUCTURE_HELIX;
		}
	}
}