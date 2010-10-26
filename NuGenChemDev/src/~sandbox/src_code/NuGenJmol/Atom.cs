/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-14 00:06:39 +0200 (ven., 14 avr. 2006) $
* $Revision: 4966 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
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
using Org.Jmol.Bspt;
using javax.vecmath;
using System.Text;
using System.Collections;

namespace Org.Jmol.Viewer
{
	sealed class Atom : Tuple
	{
        public Group Group
		{
			get { return group; }
			set { this.group = value; }
		}

		public short MadAtom
		{
			/*
			* What is a MAR?
			*  - just a term that I made up
			*  - an abbreviation for Milli Angstrom Radius
			* that is:
			*  - a *radius* of either a bond or an atom
			*  - in *millis*, or thousandths of an *angstrom*
			*  - stored as a short
			*
			* However! In the case of an atom radius, if the parameter
			* gets passed in as a negative number, then that number
			* represents a percentage of the vdw radius of that atom.
			* This is converted to a normal MAR as soon as possible
			*
			* (I know almost everyone hates bytes & shorts, but I like them ...
			*  gives me some tiny level of type-checking ...
			*  a rudimentary form of enumerations/user-defined primitive types)
			*/
			
			set
			{
				if (this.madAtom == JmolConstants.MAR_DELETED)
					return ;
				this.madAtom = convertEncodedMad(value);
			}
		}

        public int RasMolRadius
		{
            get
            {
                if (madAtom == JmolConstants.MAR_DELETED)
                    return 0;
                return madAtom / (4 * 2);
            }
		}

        public int CovalentBondCount
		{
			get
			{
				if (bonds == null)
					return 0;
				int n = 0;
				for (int i = bonds.Length; --i >= 0; )
					if ((bonds[i].order & JmolConstants.BOND_COVALENT_MASK) != 0)
						++n;
				return n;
			}
		}

        public int HbondCount
		{
			get
			{
				if (bonds == null)
					return 0;
				int n = 0;
				for (int i = bonds.Length; --i >= 0; )
					if ((bonds[i].order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
						++n;
				return n;
			}
		}

        public Bond[] Bonds
		{
			get { return bonds; }
        }

        public short ColixAtom
		{
			set
			{
                //if (value == 0)
                //    value = group.chain.frame.viewer.getColixAtomPalette(this, "cpk");
				this.colixAtom = value;
			}
		}

        public bool Translucent
		{
			set
			{
                //colixAtom = Graphics3D.Translucent = colixAtom;
			}
		}

        public Vector3f VibrationVector
		{
			get
			{
                Vector3f[] vibrationVectors = group.chain.frame.vibrationVectors;
				return vibrationVectors == null?null:vibrationVectors[atomIndex];
			}
		}

        public string Label
		{
			set { group.chain.frame.setLabel(value, atomIndex); }
		}

		public sbyte ElementNumber
		{
			get { return elementNumber; }
		}

        public string ElementSymbol
		{
			get { return JmolConstants.elementSymbols[elementNumber]; }
		}

        public string AtomNameOrNull
		{
			get
			{
                string[] atomNames = group.chain.frame.atomNames;
				return atomNames == null?null:atomNames[atomIndex];
			}
		}

        public string AtomName
		{
			get
			{
				string atomName = AtomNameOrNull;
				return (atomName != null?atomName:JmolConstants.elementSymbols[elementNumber]);
			}
		}

        public string PdbAtomName4
		{
			get
			{
				string atomName = AtomNameOrNull;
				return atomName != null?atomName:"";
			}
		}

        public string Group1
		{
			get
			{
				if (group == null)
					return null;
				return group.Group1;
			}
		}

        public int Seqcode
		{
			get
			{
				if (group == null)
					return - 1;
				return group.seqcode;
			}
		}

        public int Resno
		{
			get
			{
				if (group == null)
					return - 1;
				return group.Resno;
			}
		}

        public int AtomNumber
		{
			get
			{
                int[] atomSerials = group.chain.frame.atomSerials;
				if (atomSerials != null)
					return atomSerials[atomIndex];
                //if ((object)group.chain.frame.modelSetTypeName == (object)"xyz" && group.chain.frame.viewer.ZeroBasedXyzRasmol)
                //    return atomIndex;
				return atomIndex + 1;
			}
		}

        public bool Hetero
		{
			get { return (formalChargeAndFlags & IS_HETERO_FLAG) != 0; }
		}

        public int FormalCharge
		{
			get { return formalChargeAndFlags >> 3; }
		}

        public bool Visible
		{
			get { return (formalChargeAndFlags & VISIBLE_FLAG) != 0; }
		}

        public float PartialCharge
		{
			get
			{
                float[] partialCharges = group.chain.frame.partialCharges;
				return partialCharges == null?0:partialCharges[atomIndex];
			}
		}

        public Point3f Point3f
		{
			get { return point3f; }
		}

        public float AtomX
		{
			get { return point3f.x; }
		}

        public float AtomY
		{
			get { return point3f.y; }
		}

        public float AtomZ
		{
			get { return point3f.z; }
		}

        public short VanderwaalsMar
		{
			get { return JmolConstants.vanderwaalsMars[elementNumber]; }
		}

        public float VanderwaalsRadiusFloat
		{
			get { return JmolConstants.vanderwaalsMars[elementNumber] / 1000f; }
		}

        public short BondingMar
		{
			get { return JmolConstants.getBondingMar(elementNumber, formalChargeAndFlags >> 3); }
		}

        public float BondingRadiusFloat
		{
			get { return BondingMar / 1000f; }
		}

        public int CurrentBondCount
		{
			get
			{
				return bonds == null?0:bonds.Length;
				/*
				int currentBondCount = 0;
				for (int i = (bonds == null ? 0 : bonds.length); --i >= 0; )
				currentBondCount += bonds[i].order & JmolConstants.BOND_COVALENT;
				return currentBondCount;
				*/
			}
		}

        public short Colix
		{
			get { return colixAtom; }
		}

        public int Argb
		{
            get { return 0; /* group.chain.frame.viewer.getColixArgb(colixAtom);*/ }
		}

        public float Radius
		{
			get
			{
				if (madAtom == JmolConstants.MAR_DELETED)
					return 0;
				return madAtom / (1000f * 2);
			}
		}

        public char ChainID
		{
			get { return group.chain.chainID; }
		}

        public int Occupancy
		{
			// a percentage value in the range 0-100
			get
			{
                sbyte[] occupancies = group.chain.frame.occupancies;
				return occupancies == null?(sbyte)100:occupancies[atomIndex];
			}
		}

        public int Bfactor100
		{
			// This is called bfactor100 because it is stored as an integer
			// 100 times the bfactor(temperature) value
			get
			{
                short[] bfactor100s = group.chain.frame.bfactor100s;
				if (bfactor100s == null)
					return 0;
				return bfactor100s[atomIndex];
			}
		}

        public int PolymerLength
		{
			get { return group.PolymerLength; }
		}

        public int PolymerIndex
		{
			get { return group.PolymerIndex; }
		}

        public int SelectedGroupCountWithinChain
		{
			get { return group.chain.SelectedGroupCount; }
		}

        public int SelectedGroupIndexWithinChain
		{
			get { return group.chain.getSelectedGroupIndex(group); }
		}

        public int SelectedMonomerCountWithinPolymer
		{
			get
			{
				if (group is Monomer)
					return ((Monomer) group).polymer.selectedMonomerCount;
				return 0;
			}
		}

        public int SelectedMonomerIndexWithinPolymer
		{
			get
			{
				if (group is Monomer)
				{
					Monomer monomer = (Monomer) group;
					return monomer.polymer.getSelectedMonomerIndex(monomer);
				}
				return - 1;
			}
		}

        public int AtomIndex
		{
			get { return atomIndex; }
		}

        public Chain Chain
		{
			get { return group.chain; }
		}

        public Model Model
		{
			get { return group.chain.model; }
		}

        public int ModelIndex
		{
			get { return modelIndex; }
		}

        public bool Deleted
		{
			get { return madAtom == JmolConstants.MAR_DELETED; }
		}

        public sbyte ProteinStructureType
		{
			get { return group.ProteinStructureType; }
		}

        public short GroupID
		{
			get { return group.groupID; }
		}

        public string Seqcodestring
		{
			get { return group.SeqcodeString; }
		}

        public string ModelTag
		{
            get { return group.chain.model.modelTag; }
		}

        public int ModelTagNumber
		{
			get
			{
				try
				{
					return Int32.Parse(group.chain.model.modelTag);
				}
				catch (FormatException nfe)
				{
					return modelIndex + 1;
				}
			}
		}

        public sbyte SpecialAtomID
		{
			get
			{
                sbyte[] specialAtomIDs = group.chain.frame.specialAtomIDs;
				return specialAtomIDs == null?(sbyte)0:specialAtomIDs[atomIndex];
			}
		}

        public string Info
        {
            get { return Identity; }
        }

        public string Identity
		{
			get
			{
				StringBuilder info = new StringBuilder();
				string group3 = getGroup3();
				string seqcodestring = Seqcodestring;
				char chainID = ChainID;
				if (group3 != null && group3.Length > 0)
				{
					info.Append("[");
					info.Append(group3);
					info.Append("]");
				}
				if (seqcodestring != null)
					info.Append(seqcodestring);
				if (chainID != 0 && chainID != ' ')
				{
					info.Append(":");
					info.Append(chainID);
				}
				string atomName = AtomNameOrNull;
				if (atomName != null)
				{
					if (info.Length > 0)
						info.Append(".");
					info.Append(atomName);
				}
				if (info.Length == 0)
				{
					info.Append(ElementSymbol);
					info.Append(" ");
					info.Append(AtomNumber);
				}
                if (group.chain.frame.ModelCount > 1)
                {
                    info.Append("/");
                    info.Append(ModelTagNumber);
                }
				info.Append(" #");
				info.Append(AtomNumber);
				return "" + info;
			}
		}

		public int ScreenX
		{
            get { return screenX; }
		}

        public int ScreenY
		{
            get { return screenY; }
		}

        public int ScreenZ
		{
            get { return screenZ; }
		}

        public int ScreenD
		{
            get { return screenDiameter; }
		}

        public bool Protein
		{
			get { return group.Protein; }
		}

        public bool Nucleic
		{
			get { return group.Nucleic; }
		}

        public bool Dna
		{
            get { return group.Dna; }
		}

        public bool Rna
		{
            get { return group.Rna; }
		}

        public bool Purine
		{
            get { return group.Purine; }
		}

        public bool Pyrimidine
		{
            get { return group.Pyrimidine; }
		}

        public Hashtable PublicProperties
		{
			////////////////////////////////////////////////////////////////
			get
			{
				Hashtable ht = Hashtable.Synchronized(new Hashtable());
				ht["element"] = ElementSymbol;
				ht["x"] = (double)point3f.x;
				ht["y"] = (double)point3f.y;
				ht["z"] = (double)point3f.z;
				ht["atomIndex"] = (Int32) atomIndex;
				ht["modelIndex"] = (Int32) modelIndex;
				ht["argb"] = (Int32) Argb;
				ht["radius"] = (double) Radius;
				ht["atomNumber"] = (Int32) AtomNumber;
				return ht;
			}
		}

        public const sbyte VISIBLE_FLAG = (sbyte)(0x01);
        public const sbyte VIBRATION_VECTOR_FLAG = (sbyte)(0x02);
        public const sbyte IS_HETERO_FLAG = (sbyte)(0x04);

        public Group group;
        public int atomIndex;
        public Point3f point3f;
        public int screenX;
        public int screenY;
        public int screenZ;
        public short screenDiameter;
        public short modelIndex; // we want this here for the BallsRenderer
        public sbyte elementNumber;
        public sbyte formalChargeAndFlags;
        public sbyte alternateLocationID;
        public short madAtom;
        public short colixAtom;
        public Bond[] bonds;

        public Atom(/*Viewer viewer,*/ Frame frame, int modelIndex, int atomIndex, sbyte elementNumber, string atomName, int formalCharge, float partialCharge, int occupancy, float bfactor, float x, float y, float z, bool isHetero, int atomSerial, char chainID, float vibrationX, float vibrationY, float vibrationZ, char alternateLocationID, object clientAtomReference)
		{
            this.modelIndex = (short)modelIndex;
            this.atomIndex = atomIndex;
            this.elementNumber = elementNumber;
            this.formalChargeAndFlags = (sbyte)(formalCharge << 3);
            ////this.colixAtom = viewer.getColixAtom(this);
            this.alternateLocationID = (sbyte)alternateLocationID;
            ////MadAtom = viewer.MadAtom;
            this.point3f = new Point3f(x, y, z);
            if (isHetero)
                formalChargeAndFlags |= IS_HETERO_FLAG;
			
            if (atomName != null)
            {
                if (frame.atomNames == null)
                    frame.atomNames = new string[frame.atoms.Length];
                frame.atomNames[atomIndex] = string.Intern(atomName);
            }

            sbyte specialAtomID = lookupSpecialAtomID(atomName);
            if (specialAtomID != 0)
            {
                if (frame.specialAtomIDs == null)
                    frame.specialAtomIDs = new sbyte[frame.atoms.Length];
                frame.specialAtomIDs[atomIndex] = specialAtomID;
            }

            if (occupancy < 0)
                occupancy = 0;
            else if (occupancy > 100)
                occupancy = 100;
            if (occupancy != 100)
            {
                if (frame.occupancies == null)
                    frame.occupancies = new sbyte[frame.atoms.Length];
                frame.occupancies[atomIndex] = (sbyte)occupancy;
            }

            if (atomSerial != Int32.MinValue)
            {
                if (frame.atomSerials == null)
                    frame.atomSerials = new int[frame.atoms.Length];
                frame.atomSerials[atomIndex] = atomSerial;
            }

            if (!Single.IsNaN(partialCharge))
            {
                if (frame.partialCharges == null)
                    frame.partialCharges = new float[frame.atoms.Length];
                frame.partialCharges[atomIndex] = partialCharge;
            }
			
            if (!Single.IsNaN(bfactor) && bfactor != 0)
            {
                if (frame.bfactor100s == null)
                    frame.bfactor100s = new short[frame.atoms.Length];
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                frame.bfactor100s[atomIndex] = (short) (bfactor * 100);
            }

            if (!Single.IsNaN(vibrationX) && !Single.IsNaN(vibrationY) && !Single.IsNaN(vibrationZ))
            {
                if (frame.vibrationVectors == null)
                    frame.vibrationVectors = new Vector3f[frame.atoms.Length];
                frame.vibrationVectors[atomIndex] = new Vector3f(vibrationX, vibrationY, vibrationZ);
                formalChargeAndFlags |= VIBRATION_VECTOR_FLAG;
            }
            if (clientAtomReference != null)
            {
                if (frame.clientAtomReferences == null)
                    frame.clientAtomReferences = new object[frame.atoms.Length];
                frame.clientAtomReferences[atomIndex] = clientAtomReference;
            }
		}

        public bool isBonded(Atom atomOther)
		{
			return getBond(atomOther) != null;
		}
		
		/// <summary> Returns the count of connections to atoms found in the
		/// specified BitSet. Bond order is not considered. Hydrogen bonds
		/// are considered valid connections and are included in the count.
		/// <p>
		/// If the bs parameter is null then the total count of
		/// connections is returned;
		/// 
		/// </summary>
		/// <param name="bs">the bitset of atom indexes to be considered
		/// </param>
		/// <returns>   the count
		/// </returns>
        public int getConnectedCount(BitArray bs)
		{
			int connectedCount = 0;
			if (bonds != null)
			{
				for (int i = bonds.Length; --i >= 0; )
				{
					Bond bond = bonds[i];
					Atom otherAtom = (bond.atom1 != this)?bond.atom1:bond.atom2;
					if (bs == null || bs.Get(otherAtom.atomIndex))
						++connectedCount;
				}
			}
			return connectedCount;
		}

        public Bond getBond(Atom atomOther)
		{
            if (bonds != null)
            {
                for (int i = bonds.Length; --i >= 0; )
                {
                    Bond bond = bonds[i];
                    if ((bond.atom1 == atomOther) || (bond.atom2 == atomOther))
                        return bond;
                }
            }
			return null;
		}

        public Bond bondMutually(Atom atomOther, short order, Frame frame)
		{
			if (isBonded(atomOther))
				return null;
			Bond bond = new Bond(this, atomOther, order, frame);
			addBond(bond, frame);
			atomOther.addBond(bond, frame);
			return bond;
		}
		
		private void addBond(Bond bond, Frame frame)
		{
			if (bonds == null)
			{
				bonds = new Bond[1];
				bonds[0] = bond;
			}
			else
			{
                bonds = frame.addToBonds(bond, bonds);
			}
		}

        public void deleteBondedAtom(Atom atomToDelete)
		{
			if (bonds == null)
				return ;
			for (int i = bonds.Length; --i >= 0; )
			{
				Bond bond = bonds[i];
				Atom atomBonded = (bond.atom1 != this)?bond.atom1:bond.atom2;
				if (atomBonded == atomToDelete)
				{
					deleteBond(i);
					return ;
				}
			}
		}

        public void deleteAllBonds()
		{
			if (bonds == null)
				return ;
            for (int i = bonds.Length; --i >= 0; )
                group.chain.frame.deleteBond(bonds[i]);
			if (bonds != null)
			{
				Console.Out.WriteLine("bond delete error");
				throw new NullReferenceException();
			}
		}

        public void deleteBond(Bond bond)
		{
            for (int i = bonds.Length; --i >= 0; )
            {
                if (bonds[i] == bond)
                {
                    deleteBond(i);
                    return;
                }
            }
		}

        public void deleteBond(int i)
		{
			int newLength = bonds.Length - 1;
			if (newLength == 0)
			{
				bonds = null;
				return ;
			}
			Bond[] bondsNew = new Bond[newLength];
			int j = 0;
			for (; j < i; ++j)
				bondsNew[j] = bonds[j];
			for (; j < newLength; ++j)
				bondsNew[j] = bonds[j + 1];
			bonds = bondsNew;
		}

        public void clearBonds()
		{
			bonds = null;
		}

        public int getBondedAtomIndex(int bondIndex)
		{
			Bond bond = bonds[bondIndex];
			return (((bond.atom1 == this)?bond.atom2:bond.atom1).atomIndex & 0xFFFF);
		}

        public short convertEncodedMad(int size)
		{
			if (size == - 1000)
			{
				// temperature
				int diameter = Bfactor100 * 10 * 2;
				if (diameter > 4000)
					diameter = 4000;
				size = diameter;
			}
			else if (size == - 1001)
			// ionic
				size = (BondingMar * 2);
			else if (size < 0)
			{
				size = - size;
				if (size > 200)
					size = 200;
				size = (size * VanderwaalsMar / 50);
			}
			return (short) size;
		}
		
		// miguel 2006 03 25
		// not sure what we should do here
		// current implementation of g3d uses a short for the zbuffer coordinate
		// we could consider turning that into an int, but that would have
		// significant implications
		//
		// actually, I think that it might work out just fine. we should use
		// an int in this world, but let g3d deal with the problem of
		// something having a depth that is more than 32K ... in the same
		// sense that g3d will clip if something is not on the screen
		
		//  final static int MIN_Z = 100;
		//  final static int MAX_Z = 32766;

        public void transform(/*Viewer viewer*/)
		{
			if (madAtom == JmolConstants.MAR_DELETED)
				return ;
			Point3i screen;
            //Vector3f[] vibrationVectors;
            //if ((formalChargeAndFlags & VIBRATION_VECTOR_FLAG) == 0 || (vibrationVectors = group.chain.frame.vibrationVectors) == null)
            //    screen = viewer.transformPoint(point3f);
            //else
                //screen = viewer.transformPoint(point3f, vibrationVectors[atomIndex]);
            //screenX = screen.x;
            //screenY = screen.y;
            //screenZ = screen.z;
            //screenDiameter = viewer.scaleToScreen(screenZ, madAtom);
		}

        public string getGroup3()
		{
			return group.Group3;
		}

        public bool isGroup3(string group3)
		{
			return group.isGroup3(group3);
		}

        public bool isGroup3Match(string strWildcard)
		{
			return group.isGroup3Match(strWildcard);
		}

        public bool isAtomNameMatch(string strPattern)
		{
			string atomName = AtomNameOrNull;
			int cchAtomName = atomName == null?0:atomName.Length;
			int cchPattern = strPattern.Length;
			int ich;
			for (ich = 0; ich < cchPattern; ++ich)
			{
				char charWild = Char.ToUpper(strPattern[ich]);
				if (charWild == '?')
					continue;
				if (ich >= cchAtomName || charWild != Char.ToUpper(atomName[ich]))
					return false;
			}
			return ich >= cchAtomName;
		}

        public bool isAlternateLocationMatch(string strPattern)
		{
			if (strPattern == null)
				return true;
			if (strPattern.Length != 1)
				return false;
			return alternateLocationID == strPattern[0];
		}
		
		public float getDimensionValue(int dimension)
		{
			return (dimension == 0?point3f.x:(dimension == 1?point3f.y:point3f.z));
		}
		
		// find the longest bond to discard
		// but return null if atomChallenger is longer than any
		// established bonds
		// note that this algorithm works when maximum valence == 0
        public Bond getLongestBondToDiscard(Atom atomChallenger)
		{
			float dist2Longest = point3f.distanceSquared(atomChallenger.point3f);
			Bond bondLongest = null;
			for (int i = bonds.Length; --i >= 0; )
			{
				Bond bond = bonds[i];
				Atom atomOther = bond.atom1 != this?bond.atom1:bond.atom2;
				float dist2 = point3f.distanceSquared(atomOther.point3f);
				if (dist2 > dist2Longest)
				{
					bondLongest = bond;
					dist2Longest = dist2;
				}
			}
			//    out.println("atom at " + point3f + " suggests discard of " +
			//                       bondLongest + " dist2=" + dist2Longest);
			return bondLongest;
		}

        public string getClientAtomstringProperty(string propertyName)
		{
            //object[] clientAtomReferences = group.chain.frame.clientAtomReferences;
            return null;// ((clientAtomReferences == null || clientAtomReferences.Length <= atomIndex) ? null : (group.chain.frame.viewer.getClientAtomstringProperty(clientAtomReferences[atomIndex], propertyName)));
		}
		
		public void markDeleted()
		{
			deleteAllBonds();
			madAtom = JmolConstants.MAR_DELETED;
		}

        public void demoteSpecialAtomImposter()
		{
            group.chain.frame.specialAtomIDs[atomIndex] = 0;
		}
		
		/* ***************************************************************
		* disabled until I figure out how to generate pretty names
		* without breaking inorganic compounds
		
		// this requires a 4 letter name, in PDB format
		// only here for transition purposes
		static string calcPrettyName(string name) {
		if (name.length() < 4)
		return name;
		char chBranch = name.charAt(3);
		char chRemote = name.charAt(2);
		switch (chRemote) {
		case 'A':
		chRemote = '\u03B1';
		break;
		case 'B':
		chRemote = '\u03B2';
		break;
		case 'C':
		case 'G':
		chRemote = '\u03B3';
		break;
		case 'D':
		chRemote = '\u03B4';
		break;
		case 'E':
		chRemote = '\u03B5';
		break;
		case 'Z':
		chRemote = '\u03B6';
		break;
		case 'H':
		chRemote = '\u03B7';
		}
		string pretty = name.substring(0, 2).trim();
		if (chBranch != ' ')
		pretty += "" + chRemote + chBranch;
		else
		pretty += chRemote;
		return pretty;
		}
		*/
		
		private static Hashtable htAtom = Hashtable.Synchronized(new Hashtable());

        public static string generateStarredAtomName(string primedAtomName)
		{
			int primeIndex = primedAtomName.IndexOf('\'');
			if (primeIndex < 0)
				return null;
			return primedAtomName.Replace('\'', '*');
		}

        public static string generatePrimeAtomName(string starredAtomName)
		{
			int starIndex = starredAtomName.IndexOf('*');
			if (starIndex < 0)
				return starredAtomName;
			return starredAtomName.Replace('*', '\'');
		}

        public sbyte lookupSpecialAtomID(string atomName)
		{
			if (atomName != null)
			{
				atomName = generatePrimeAtomName(atomName);
                int boxedAtomID = -1;
                if (htAtom[atomName] != null)
                    boxedAtomID = (int)htAtom[atomName];
				if (boxedAtomID != -1)
					return (sbyte)(boxedAtomID);
			}
			return 0;
		}

        public string formatLabel(string strFormat)
		{
			if (strFormat == null || strFormat.Equals(""))
				return null;
			string strLabel = "";
			//int cch = strFormat.length();
			int ich, ichPercent;
			//UPGRADE_WARNING: Method 'java.lang.string.indexOf' was converted to 'string.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			for (ich = 0; (ichPercent = strFormat.IndexOf('%', ich)) != - 1; )
			{
				if (ich != ichPercent)
					strLabel += strFormat.Substring(ich, (ichPercent) - (ich));
				ich = ichPercent + 1;
				try
				{
					string strT = "";
					float floatT = 0;
					bool floatIsSet = false;
					bool alignLeft = false;
					if (strFormat[ich] == '-')
					{
						alignLeft = true;
						++ich;
					}
					bool zeroPad = false;
					if (strFormat[ich] == '0')
					{
						zeroPad = true;
						++ich;
					}
					char ch;
					int width = 0;
					while ((ch = strFormat[ich]) >= '0' && (ch <= '9'))
					{
						width = (10 * width) + (ch - '0');
						++ich;
					}
					int precision = - 1;
					if (strFormat[ich] == '.')
					{
						++ich;
						if ((ch = strFormat[ich]) >= '0' && (ch <= '9'))
						{
							precision = ch - '0';
							++ich;
						}
					}
					switch (ch = strFormat[ich++])
					{
						
						case 'i': 
							strT = "" + AtomNumber;
							break;
						
						case 'a': 
							strT = AtomName;
							break;
						
						case 'e': 
							strT = JmolConstants.elementSymbols[elementNumber];
							break;
						
						case 'x': 
							floatT = point3f.x;
							floatIsSet = true;
							break;
						
						case 'y': 
							floatT = point3f.y;
							floatIsSet = true;
							break;
						
						case 'z': 
							floatT = point3f.z;
							floatIsSet = true;
							break;
						
						case 'X': 
							strT = "" + atomIndex;
							break;
						
						case 'C': 
							int formalCharge = FormalCharge;
							if (formalCharge > 0)
								strT = "" + formalCharge + "+";
							else if (formalCharge < 0)
								strT = "" + (- formalCharge) + "-";
							else
								strT = "0";
							break;
						
						case 'P': 
							floatT = PartialCharge;
							floatIsSet = true;
							break;
						
						case 'V': 
							floatT = VanderwaalsRadiusFloat;
							floatIsSet = true;
							break;
						
						case 'I': 
							floatT = BondingRadiusFloat;
							floatIsSet = true;
							break;
						
						case 'b': 
						// these two are the same
						case 't': 
							floatT = Bfactor100 / 100f;
							floatIsSet = true;
							break;
						
						case 'q': 
							strT = "" + Occupancy;
							break;
						
						case 'c': 
						// these two are the same
						case 's': 
							strT = "" + ChainID;
							break;
						
						case 'L': 
							strT = "" + PolymerLength;
							break;
						
						case 'M': 
							strT = "/" + ModelTagNumber;
							break;
						
						case 'm': 
							strT = Group1;
							break;
						
						case 'n': 
							strT = getGroup3();
							break;
						
						case 'r': 
							strT = Seqcodestring;
							break;
						
						case 'U': 
							strT = Identity;
							break;
						
						case '%': 
							strT = "%";
							break;
						
						case '{':  // client property name
							//UPGRADE_WARNING: Method 'java.lang.string.indexOf' was converted to 'string.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
							int ichCloseBracket = strFormat.IndexOf('}', ich);
							if (ichCloseBracket > ich)
							{
								// also picks up -1 when no '}' is found
								string propertyName = strFormat.Substring(ich, (ichCloseBracket) - (ich));
								string value_Renamed = getClientAtomstringProperty(propertyName);
								if (value_Renamed != null)
									strT = value_Renamed;
								ich = ichCloseBracket + 1;
								break;
							}
							// malformed will fall into
							goto default;
						
						default: 
							strT = "%" + ch;
							break;
						
					}
					if (floatIsSet)
					{
						strLabel += format(floatT, width, precision, alignLeft, zeroPad);
					}
					else
					{
						strLabel += format(strT, width, precision, alignLeft, zeroPad);
					}
				}
				catch (IndexOutOfRangeException ioobe)
				{
					ich = ichPercent;
					break;
				}
			}
			strLabel += strFormat.Substring(ich);
			if (strLabel.Length == 0)
				return null;
			return string.Intern(strLabel);
		}

        public string format(float value_Renamed, int width, int precision, bool alignLeft, bool zeroPad)
		{
            return null;// format(group.chain.frame.viewer.formatDecimal(value_Renamed, precision), width, 0, alignLeft, zeroPad);
		}

        public static string format(string value_Renamed, int width, int precision, bool alignLeft, bool zeroPad)
		{
			if (value_Renamed == null)
				return "";
			if (precision > value_Renamed.Length)
				value_Renamed = value_Renamed.Substring(0, (precision) - (0));
			int padLength = width - value_Renamed.Length;
			if (padLength <= 0)
				return value_Renamed;
			StringBuilder sb = new StringBuilder();
			if (alignLeft)
				sb.Append(value_Renamed);
			for (int i = padLength; --i >= 0; )
				sb.Append((!alignLeft && zeroPad)?'0':' ');
			if (!alignLeft)
				sb.Append(value_Renamed);
			return "" + sb;
		}

        public bool isCursorOnTopOfVisibleAtom(int xCursor, int yCursor, int minRadius, Atom competitor)
		{
			return (((formalChargeAndFlags & VISIBLE_FLAG) != 0) && isCursorOnTop(xCursor, yCursor, minRadius, competitor));
		}

        public bool isCursorOnTop(int xCursor, int yCursor, int minRadius, Atom competitor)
		{
			int r = screenDiameter / 2;
			if (r < minRadius)
				r = minRadius;
			int r2 = r * r;
			int dx = screenX - xCursor;
			int dx2 = dx * dx;
			if (dx2 > r2)
				return false;
			int dy = screenY - yCursor;
			int dy2 = dy * dy;
			int dz2 = r2 - (dx2 + dy2);
			if (dz2 < 0)
				return false;
			if (competitor == null)
				return true;
			int z = screenZ;
			int zCompetitor = competitor.screenZ;
			int rCompetitor = competitor.screenDiameter / 2;
			if (z < zCompetitor - rCompetitor)
				return true;
			int dxCompetitor = competitor.screenX - xCursor;
			int dx2Competitor = dxCompetitor * dxCompetitor;
			int dyCompetitor = competitor.screenY - yCursor;
			int dy2Competitor = dyCompetitor * dyCompetitor;
			int r2Competitor = rCompetitor * rCompetitor;
			int dz2Competitor = r2Competitor - (dx2Competitor + dy2Competitor);
			return (z - Math.Sqrt(dz2) < zCompetitor - Math.Sqrt(dz2Competitor));
		}

		static Atom()
		{
			{
				for (int i = JmolConstants.specialAtomNames.Length; --i >= 0; )
				{
					string specialAtomName = JmolConstants.specialAtomNames[i];
					if (specialAtomName != null)
					{
						htAtom[specialAtomName] = i;
					}
				}
			}
		}
	}
}