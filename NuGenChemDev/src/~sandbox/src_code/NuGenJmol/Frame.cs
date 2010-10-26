/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-06 13:46:05 +0200 (jeu., 06 avr. 2006) $
* $Revision: 4925 $
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
using System.Collections.Specialized;
using System.Collections;
using System.Drawing;

namespace Org.Jmol.Viewer
{
	sealed class Frame
	{
		private void InitBlock()
		{
			specialAtomIndexes = new int[JmolConstants.ATOMID_MAX];
			shapes = new Shape[JmolConstants.SHAPE_MAX];
			withinModelIterator = new WithinModelIterator(this);
			withinAnyModelIterator = new WithinAnyModelIterator(this);
			hbondMin2 = hbondMin * hbondMin;
			for (int i = MAX_BONDS_LENGTH_TO_CACHE; --i > 0; )
			// .GT. 0
				freeBonds[i] = new Bond[MAX_NUM_TO_CACHE][];
		}

        //public JmolAdapter ExportJmolAdapter
        //{
        //    get
        //    {
        //        if (exportJmolAdapter == null)
        //            exportJmolAdapter = new FrameExportJmolAdapter(/*viewer,*/ this);
        //        return exportJmolAdapter;
        //    }
			
        //}

        public NameValueCollection ModelSetProperties
		{
			get { return mmset.ModelSetProperties; }
			set { mmset.ModelSetProperties = value; }
		}

        public int ModelCount
		{
			get { return mmset.ModelCount; }
			set { mmset.ModelCount = value; }
		}

        public string ModelSetTypeName
		{
			get { return modelSetTypeName; }
		}

        public int ChainCount
		{
			get { return mmset.ChainCount; }
		}

        public int PolymerCount
		{
			get { return mmset.PolymerCount; }
		}

        public int GroupCount
		{
			get { return mmset.GroupCount; }
		}

        public int AtomCount
		{
			get { return atomCount; }
		}

        public Atom[] Atoms
		{
			get { return atoms; }
		}

        public int BondCount
		{
			get { return bondCount; }
		}

        public Point3f BoundBoxCenter
		{
			get
			{
				findBounds();
				return centerBoundBox;
			}
		}

        public Point3f AverageAtomPoint
		{
			get
			{
				findBounds();
				return averageAtomPoint;
			}
		}

        public Vector3f BoundBoxCornerVector
		{
			get
			{
				findBounds();
				return boundBoxCornerVector;
			}
		}

        public Point3f RotationCenter
		{
			get
			{
				findBounds();
				return rotationCenter;
			}
			set
			{
				if (value != null)
				{
					rotationCenter = value;
                    //if (viewer.WindowCentered)
                    //    rotationRadius = calcRotationRadius(rotationCenter);
				}
				else
				{
					rotationCenter = rotationCenterDefault;
					rotationRadius = rotationRadiusDefault;
				}
			}
		}

        public Point3f DefaultRotationCenter
		{
			get
			{
				findBounds();
				return rotationCenterDefault;
			}
		}

        public Point3f RotationCenterDefault
		{
			get
			{
				findBounds();
				return rotationCenterDefault;
			}
		}

        public float RotationRadius
		{
			get
			{
				findBounds();
				return rotationRadius;
			}
		}

        public int BsptCount
		{
			get
			{
				if (bspf == null)
					initializeBspf();
				return bspf.BsptCount;
			}
		}

        public float MaxVanderwaalsRadius
		{
			get
			{
				//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Float.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				if (maxVanderwaalsRadius == System.Single.Epsilon)
					findMaxRadii();
				return maxVanderwaalsRadius;
			}
		}

        public float[] NotionalUnitcell
		{
			set
			{
				if (value != null && value.Length != 6)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("notionalUnitcell length incorrect:" + value);
				}
				else
					this.notionalUnitcell = value;
			}
		}

        public float[] PdbScaleMatrix
		{
			set
			{
				if (value == null)
					return ;
				if (value.Length != 9)
				{
					System.Console.Out.WriteLine("pdbScaleMatrix.length != 9 :" + pdbScaleMatrix);
					return ;
				}
				pdbScaleMatrix = new Matrix3f(value);
				pdbScaleMatrixTranspose = new Matrix3f();
				pdbScaleMatrixTranspose.transpose(pdbScaleMatrix);
			}
		}

        public float[] PdbScaleTranslate
		{
			set
			{
				if (value == null)
					return ;
				if (value.Length != 3)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("pdbScaleTranslate.length != 3 :" + value);
					return ;
				}
				this.pdbTranslateVector = new Vector3f(value);
			}
		}

        public BitArray ElementsPresentBitSet
		{
			get { return elementsPresent; }
		}

        public BitArray GroupsPresentBitSet
		{
            get { return groupsPresent; }
		}

        public int Bfactor100Lo
		{
			get
			{
				if (!hasBfactorRange)
					calcBfactorRange();
				return bfactor100Lo;
			}
		}

        public int Bfactor100Hi
		{
			get
			{
				if (!hasBfactorRange)
					calcBfactorRange();
				return bfactor100Hi;
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'viewer '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //internal Viewer viewer;
		//UPGRADE_NOTE: Final was removed from the declaration of 'adapter '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //public JmolAdapter adapter;
		//UPGRADE_NOTE: Final was removed from the declaration of 'frameRenderer '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public FrameRenderer frameRenderer;
		
        // NOTE: these strings are interned and are lower case
		// therefore, we can do == comparisions against string constants
		// if (modelSetTypeName == "xyz")
		//UPGRADE_NOTE: Final was removed from the declaration of 'modelSetTypeName '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public string modelSetTypeName;
		
        //UPGRADE_NOTE: Final was removed from the declaration of 'mmset '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Mmset mmset;
		//UPGRADE_NOTE: Final was removed from the declaration of 'g3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //internal Graphics3D g3d;
		// the maximum BondingRadius seen in this set of atoms
		// used in autobonding
		//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Float.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        public float maxBondingRadius = System.Single.Epsilon;
		//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Float.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        public float maxVanderwaalsRadius = System.Single.Epsilon;

        public int atomCount;
        public Atom[] atoms;
		////////////////////////////////////////////////////////////////
		// these may or may not be allocated
		// depending upon the AtomSetCollection characteristics
        public object[] clientAtomReferences;
        public Vector3f[] vibrationVectors;
        public sbyte[] occupancies;
        public short[] bfactor100s;
        public float[] partialCharges;
        public string[] atomNames;
        public int[] atomSerials;
        public sbyte[] specialAtomIDs;
		////////////////////////////////////////////////////////////////

        public int bondCount;
        public Bond[] bonds;
		private const int growthIncrement = 250;
        public bool fileCoordinatesAreFractional;
        public float[] notionalUnitcell;
        public Matrix3f matrixNotional;
        public Matrix3f pdbScaleMatrix;
        public Matrix3f pdbScaleMatrixTranspose;
        public Vector3f pdbTranslateVector;
        public Matrix3f matrixEuclideanToFractional;
        public Matrix3f matrixFractionalToEuclidean;

        public bool hasVibrationVectors_Renamed_Field;
        public bool fileHasHbonds;

        public bool structuresDefined;

        public BitArray elementsPresent;

        public int groupCount;
        public Group[] groups;

        public BitArray groupsPresent;

        public bool hasBfactorRange;
        public int bfactor100Lo;
        public int bfactor100Hi;

        public Frame(/*Viewer viewer, JmolAdapter adapter,*/ object clientFile)
		{
			InitBlock();
            //this.viewer = viewer;
            //this.adapter = adapter;
			
			//long timeBegin = System.currentTimeMillis();
            //string fileTypeName = adapter.getFileTypeName(clientFile);
			// NOTE: these strings are interned and are lower case
			// therefore, we can do == comparisions against string constants
			// if (modelSetTypeName == "xyz") { }
            //this.modelSetTypeName = String.Intern(fileTypeName.ToLower());
			mmset = new Mmset(this);
            //this.frameRenderer = viewer.FrameRenderer;
            //this.g3d = viewer.Graphics3D;
			
            //initializeBuild(adapter.getEstimatedAtomCount(clientFile));
			
			/// <summary>
			/// crystal cell must come first, in case atom coordinates
			/// need to be transformed to fit in the crystal cell
			/// </summary>
            //fileCoordinatesAreFractional = adapter.coordinatesAreFractional(clientFile);
            //NotionalUnitcell = adapter.getNotionalUnitcell(clientFile);
            //PdbScaleMatrix = adapter.getPdbScaleMatrix(clientFile);
            //PdbScaleTranslate = adapter.getPdbScaleTranslate(clientFile);
			
            //ModelSetProperties = adapter.getAtomSetCollectionProperties(clientFile);
			
            //currentModelIndex = - 1;
            //int modelCount = adapter.getAtomSetCount(clientFile);
            //ModelCount = modelCount;
            //for (int i = 0; i < modelCount; ++i)
            //{
            //    int modelNumber = adapter.getAtomSetNumber(clientFile, i);
            //    string modelName = adapter.getAtomSetName(clientFile, i);
            //    if (modelName == null)
            //        modelName = "" + modelNumber;
            //    //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            //    System.Collections.Specialized.NameValueCollection modelProperties = adapter.getAtomSetProperties(clientFile, i);
            //    setModelNameNumberProperties(i, modelName, modelNumber, modelProperties);
            //}
			
            //for (JmolAdapter.AtomIterator iterAtom = adapter.getAtomIterator(clientFile); iterAtom.hasNext(); )
            //{
            //    sbyte elementNumber = (sbyte) iterAtom.ElementNumber;
            //    if (elementNumber <= 0)
            //        elementNumber = JmolConstants.elementNumberFromSymbol(iterAtom.ElementSymbol);
            //    char alternateLocation = iterAtom.AlternateLocationID;
            //    if (alternateLocation != '\x0000' && alternateLocation != 'A')
            //        continue;
            //    addAtom(iterAtom.AtomSetIndex, iterAtom.UniqueID, elementNumber, iterAtom.AtomName, iterAtom.FormalCharge, iterAtom.PartialCharge, iterAtom.Occupancy, iterAtom.Bfactor, iterAtom.X, iterAtom.Y, iterAtom.Z, iterAtom.IsHetero, iterAtom.AtomSerial, iterAtom.ChainID, iterAtom.Group3, iterAtom.SequenceNumber, iterAtom.InsertionCode, iterAtom.VectorX, iterAtom.VectorY, iterAtom.VectorZ, alternateLocation, iterAtom.ClientAtomReference);
            //}
			
            //fileHasHbonds = false;
			
            //{
            //    JmolAdapter.BondIterator iterBond = adapter.getBondIterator(clientFile);
            //    if (iterBond != null)
            //        while (iterBond.hasNext())
            //            bondAtoms(iterBond.AtomUniqueID1, iterBond.AtomUniqueID2, iterBond.EncodedOrder);
            //}
			
            //JmolAdapter.StructureIterator iterStructure = adapter.getStructureIterator(clientFile);
            //if (iterStructure != null)
            //    while (iterStructure.hasNext())
            //        defineStructure(iterStructure.StructureType, iterStructure.StartChainID, iterStructure.StartSequenceNumber, iterStructure.StartInsertionCode, iterStructure.EndChainID, iterStructure.EndSequenceNumber, iterStructure.EndInsertionCode);
            //doUnitcellStuff();
            //doAutobond();
            //finalizeGroupBuild();
            //buildPolymers();
            //freeze();
			//long msToBuild = System.currentTimeMillis() - timeBegin;
			//    System.out.println("Build a frame:" + msToBuild + " ms");
            //adapter.finish(clientFile);
            //finalizeBuild();
            //dumpAtomSetNameDiagnostics(clientFile);
		}

        public void dumpAtomSetNameDiagnostics(object clientFile)
		{
			if (true)
				return ;
            //int frameModelCount = ModelCount;
            //int adapterAtomSetCount = adapter.getAtomSetCount(clientFile);
            //System.Console.Out.WriteLine("----------------\n" + "debugging of AtomSetName stuff\n" + "\nframeModelCount=" + frameModelCount + "\nadapterAtomSetCount=" + adapterAtomSetCount + "\n -- \n");
            //for (int i = 0; i < adapterAtomSetCount; ++i)
            //{
            //    System.Console.Out.WriteLine("atomSetName[" + i + "]=" + adapter.getAtomSetName(clientFile, i) + " atomSetNumber[" + i + "]=" + adapter.getAtomSetNumber(clientFile, i));
            //}
		}
		
		private const int ATOM_GROWTH_INCREMENT = 2000;

        public int currentModelIndex;
        public Model currentModel;
        public char currentChainID;
        public Chain currentChain;
        public int currentGroupSequenceNumber;
        public char currentGroupInsertionCode;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'htAtomMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private System.Collections.Hashtable htAtomMap = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());


        public void initializeBuild(int atomCountEstimate)
		{
			currentModel = null;
			currentChainID = '\uFFFF';
			currentChain = null;
			currentGroupInsertionCode = '\uFFFF';
			
			if (atomCountEstimate <= 0)
				atomCountEstimate = ATOM_GROWTH_INCREMENT;
			atoms = new Atom[atomCountEstimate];
			bonds = new Bond[2 * atomCountEstimate];
			htAtomMap.Clear();
			initializeGroupBuild();
		}

        public void finalizeBuild()
		{
			currentModel = null;
			currentChain = null;
			htAtomMap.Clear();
		}


        public void addAtom(int modelIndex, object atomUid, sbyte atomicNumber, string atomName, int formalCharge, float partialCharge, int occupancy, float bfactor, float x, float y, float z, bool isHetero, int atomSerial, char chainID, string group3, int groupSequenceNumber, char groupInsertionCode, float vectorX, float vectorY, float vectorZ, char alternateLocationID, object clientAtomReference)
		{
			if (modelIndex != currentModelIndex)
			{
				currentModel = mmset.getModel(modelIndex);
				currentModelIndex = modelIndex;
				currentChainID = '\uFFFF';
			}
			if (chainID != currentChainID)
			{
				currentChainID = chainID;
				currentChain = currentModel.getOrAllocateChain(chainID);
				currentGroupInsertionCode = '\uFFFF';
			}
			if (groupSequenceNumber != currentGroupSequenceNumber || groupInsertionCode != currentGroupInsertionCode)
			{
				currentGroupSequenceNumber = groupSequenceNumber;
				currentGroupInsertionCode = groupInsertionCode;
				startGroup(currentChain, group3, groupSequenceNumber, groupInsertionCode, atomCount);
			}
			if (atomCount == atoms.Length)
				growAtomArrays();
			
			Atom atom = new Atom(/*viewer,*/ this, currentModelIndex, atomCount, atomicNumber, atomName, formalCharge, partialCharge, occupancy, bfactor, x, y, z, isHetero, atomSerial, chainID, vectorX, vectorY, vectorZ, alternateLocationID, clientAtomReference);
			
			atoms[atomCount] = atom;
			++atomCount;
			htAtomMap[atomUid] = atom;
		}

        public void bondAtoms(object atomUid1, object atomUid2, int order)
		{
			Atom atom1 = (Atom) htAtomMap[atomUid1];
			if (atom1 == null)
			{
				System.Console.Out.WriteLine("bondAtoms cannot find atomUid1?");
				return ;
			}
			Atom atom2 = (Atom) htAtomMap[atomUid2];
			if (atom2 == null)
			{
				System.Console.Out.WriteLine("bondAtoms cannot find atomUid2?");
				return ;
			}
			if (bondCount == bonds.Length)
				bonds = (Bond[]) Util.setLength(bonds, bondCount + 2 * ATOM_GROWTH_INCREMENT);
			// note that if the atoms are already bonded then
			// Atom.bondMutually(...) will return null
			Bond bond = atom1.bondMutually(atom2, (short) order, this);
			if (bond != null)
			{
				bonds[bondCount++] = bond;
				if ((order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
					fileHasHbonds = true;
			}
		}

        public void growAtomArrays()
		{
			int newLength = atomCount + ATOM_GROWTH_INCREMENT;
			atoms = (Atom[]) Util.setLength(atoms, newLength);
			if (clientAtomReferences != null)
				clientAtomReferences = (object[]) Util.setLength(clientAtomReferences, newLength);
			if (vibrationVectors != null)
				vibrationVectors = (Vector3f[]) Util.setLength(vibrationVectors, newLength);
			if (occupancies != null)
				occupancies = Util.setLength(occupancies, newLength);
			if (bfactor100s != null)
				bfactor100s = Util.setLength(bfactor100s, newLength);
			if (partialCharges != null)
				partialCharges = Util.setLength(partialCharges, newLength);
			if (atomNames != null)
				atomNames = Util.setLength(atomNames, newLength);
			if (atomSerials != null)
				atomSerials = Util.setLength(atomSerials, newLength);
			if (specialAtomIDs != null)
				specialAtomIDs = Util.setLength(specialAtomIDs, newLength);
		}
		
		////////////////////////////////////////////////////////////////
		// special handling for groups
		////////////////////////////////////////////////////////////////

        public const int defaultGroupCount = 32;
        public Chain[] chains = new Chain[defaultGroupCount];
        public string[] group3s = new string[defaultGroupCount];
        public int[] seqcodes = new int[defaultGroupCount];
        public int[] firstAtomIndexes = new int[defaultGroupCount];
		
		public int[] specialAtomIndexes;

        public void initializeGroupBuild()
		{
			groupCount = 0;
		}

        public void finalizeGroupBuild()
		{
			// run this loop in increasing order so that the
			// groups get defined going up
			groups = new Group[groupCount];
			for (int i = 0; i < groupCount; ++i)
			{
				distinguishAndPropogateGroup(i, chains[i], group3s[i], seqcodes[i], firstAtomIndexes[i], (i == groupCount - 1?atomCount:firstAtomIndexes[i + 1]));
				chains[i] = null;
				group3s[i] = null;
			}
		}

        public void startGroup(Chain chain, string group3, int groupSequenceNumber, char groupInsertionCode, int firstAtomIndex)
	    {
		    if (groupCount == group3s.Length)
		    {
			    chains = (Chain[]) Util.doubleLength(chains);
			    group3s = Util.doubleLength(group3s);
			    seqcodes = Util.doubleLength(seqcodes);
			    firstAtomIndexes = Util.doubleLength(firstAtomIndexes);
		    }
		    firstAtomIndexes[groupCount] = firstAtomIndex;
		    chains[groupCount] = chain;
		    group3s[groupCount] = group3;
		    seqcodes[groupCount] = Group.getSeqcode(groupSequenceNumber, groupInsertionCode);
		    ++groupCount;
	    }

        public void distinguishAndPropogateGroup(int groupIndex, Chain chain, string group3, int seqcode, int firstAtomIndex, int maxAtomIndex)
		{
			//    System.out.println("distinguish & propogate group:" +
			//                       " group3:" + group3 +
			//                       " seqcode:" + Group.getSeqcodeString(seqcode) +
			//                       " firstAtomIndex:" + firstAtomIndex +
			//                       " maxAtomIndex:" + maxAtomIndex);
			int distinguishingBits = 0;
			// clear previous specialAtomIndexes
			for (int i = JmolConstants.ATOMID_MAX; --i >= 0; )
				specialAtomIndexes[i] = System.Int32.MinValue;
			
			if (specialAtomIDs != null)
			{
				for (int i = maxAtomIndex; --i >= firstAtomIndex; )
				{
					int specialAtomID = specialAtomIDs[i];
					if (specialAtomID > 0)
					{
						if (specialAtomID < JmolConstants.ATOMID_DISTINGUISHING_ATOM_MAX)
							distinguishingBits |= 1 << specialAtomID;
						specialAtomIndexes[specialAtomID] = i;
					}
				}
			}
			
			int lastAtomIndex = maxAtomIndex - 1;
			if (lastAtomIndex < firstAtomIndex)
				throw new System.NullReferenceException();
			
			Group group = null;
			//    System.out.println("distinguishingBits=" + distinguishingBits);
			if ((distinguishingBits & JmolConstants.ATOMID_PROTEIN_MASK) == JmolConstants.ATOMID_PROTEIN_MASK)
			{
				//      System.out.println("may be an AminoMonomer");
				group = AminoMonomer.validateAndAllocate(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, specialAtomIndexes, atoms);
			}
			else if (distinguishingBits == JmolConstants.ATOMID_ALPHA_ONLY_MASK)
			{
				//      System.out.println("AlphaMonomer.validateAndAllocate");
				group = AlphaMonomer.validateAndAllocate(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, specialAtomIndexes, atoms);
			}
			else if (((distinguishingBits & JmolConstants.ATOMID_NUCLEIC_MASK) == JmolConstants.ATOMID_NUCLEIC_MASK))
			{
				group = NucleicMonomer.validateAndAllocate(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, specialAtomIndexes, atoms);
			}
			else if (distinguishingBits == JmolConstants.ATOMID_PHOSPHORUS_ONLY_MASK)
			{
				// System.out.println("PhosphorusMonomer.validateAndAllocate");
				group = PhosphorusMonomer.validateAndAllocate(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, specialAtomIndexes, atoms);
			}
			if (group == null)
				group = new Group(chain, group3, seqcode, firstAtomIndex, lastAtomIndex);
			
			chain.addGroup(group);
			groups[groupIndex] = group;
			
			////////////////////////////////////////////////////////////////
			for (int i = maxAtomIndex; --i >= firstAtomIndex; )
				atoms[i].Group = group;
		}
		
		////////////////////////////////////////////////////////////////

        public void buildPolymers()
		{
			for (int i = 0; i < groupCount; ++i)
			{
				Group group = groups[i];
				if (group is Monomer)
				{
					Monomer monomer = (Monomer) group;
					if (monomer.polymer == null)
						Polymer.allocatePolymer(groups, i);
				}
			}
		}
		////////////////////////////////////////////////////////////////
        //public FrameExportJmolAdapter exportJmolAdapter;

        public void freeze()
		{
			
			////////////////////////////////////////////////////////////////
			// resize arrays
			if (atomCount < atoms.Length)
			{
				atoms = (Atom[]) Util.setLength(atoms, atomCount);
				if (clientAtomReferences != null)
					clientAtomReferences = (object[]) Util.setLength(clientAtomReferences, atomCount);
				if (vibrationVectors != null)
					vibrationVectors = (Vector3f[]) Util.setLength(vibrationVectors, atomCount);
				if (occupancies != null)
					occupancies = Util.setLength(occupancies, atomCount);
				if (bfactor100s != null)
					bfactor100s = Util.setLength(bfactor100s, atomCount);
				if (partialCharges != null)
					partialCharges = Util.setLength(partialCharges, atomCount);
				if (atomNames != null)
					atomNames = Util.setLength(atomNames, atomCount);
				if (atomSerials != null)
					atomSerials = Util.setLength(atomSerials, atomCount);
				if (specialAtomIDs != null)
					specialAtomIDs = Util.setLength(specialAtomIDs, atomCount);
			}
			if (bondCount < bonds.Length)
				bonds = (Bond[]) Util.setLength(bonds, bondCount);
			
			freeBondsCache();
			
			////////////////////////////////////////////////////////////////
			// see if there are any vectors
			hasVibrationVectors_Renamed_Field = vibrationVectors != null;
			
			////////////////////////////////////////////////////////////////
			//
			hackAtomSerialNumbersForAnimations();
			
			if (!structuresDefined)
				mmset.calculateStructures();
			
			////////////////////////////////////////////////////////////////
			// find things for the popup menus
			findElementsPresent();
			findGroupsPresent();
			mmset.freeze();
			
			loadShape(JmolConstants.SHAPE_BALLS);
			loadShape(JmolConstants.SHAPE_STICKS);
			loadShape(JmolConstants.SHAPE_HSTICKS);
			loadShape(JmolConstants.SHAPE_MEASURES);
		}

        public void hackAtomSerialNumbersForAnimations()
		{
			// first, validate that all atomSerials are NaN
			if (atomSerials != null)
				return ;
			// now, we'll assign 1-based atom numbers within each model
			int lastModelIndex = System.Int32.MaxValue;
			int modelAtomIndex = 0;
			atomSerials = new int[atomCount];
			for (int i = 0; i < atomCount; ++i)
			{
				Atom atom = atoms[i];
				if (atom.modelIndex != lastModelIndex)
				{
					lastModelIndex = atom.modelIndex;
					modelAtomIndex = 1;
				}
				atomSerials[i] = modelAtomIndex++;
			}
		}

        public void defineStructure(string structureType, char startChainID, int startSequenceNumber, char startInsertionCode, char endChainID, int endSequenceNumber, char endInsertionCode)
		{
			structuresDefined = true;
			mmset.defineStructure(structureType, startChainID, startSequenceNumber, startInsertionCode, endChainID, endSequenceNumber, endInsertionCode);
		}

        public int getAtomIndexFromAtomNumber(int atomNumber)
		{
			for (int i = atomCount; --i >= 0; )
			{
				if (atoms[i].AtomNumber == atomNumber)
					return i;
			}
			return - 1;
		}

        public string getModelSetProperty(string propertyName)
		{
			return mmset.getModelSetProperty(propertyName);
		}

        public bool modelSetHasVibrationVectors()
		{
			return hasVibrationVectors_Renamed_Field;
		}

        public bool hasVibrationVectors()
		{
			return hasVibrationVectors_Renamed_Field;
		}

        public bool modelHasVibrationVectors(int modelIndex)
		{
			if (vibrationVectors != null)
				for (int i = atomCount; --i >= 0; )
					if (atoms[i].modelIndex == modelIndex && vibrationVectors[i] != null)
						return true;
			return false;
		}

        public int getModelNumber(int modelIndex)
		{
			return mmset.getModelNumber(modelIndex);
		}

        public string getModelName(int modelIndex)
		{
			return mmset.getModelName(modelIndex);
		}
		
		public NameValueCollection getModelProperties(int modelIndex)
		{
			return mmset.getModelProperties(modelIndex);
		}

        public string getModelProperty(int modelIndex, string propertyName)
		{
			return mmset.getModelProperty(modelIndex, propertyName);
		}

        public Model getModel(int modelIndex)
		{
			return mmset.getModel(modelIndex);
		}

        public int getModelNumberIndex(int modelNumber)
		{
			return mmset.getModelNumberIndex(modelNumber);
		}
		
		public void setModelNameNumberProperties(int modelIndex, string modelName, int modelNumber, NameValueCollection modelProperties)
		{
			mmset.setModelNameNumberProperties(modelIndex, modelName, modelNumber, modelProperties);
		}

        public int getPolymerCountInModel(int modelIndex)
		{
			return mmset.getPolymerCountInModel(modelIndex);
		}

        public Polymer getPolymerAt(int modelIndex, int polymerIndex)
		{
			return mmset.getPolymerAt(modelIndex, polymerIndex);
		}

        public Atom getAtomAt(int atomIndex)
		{
			return atoms[atomIndex];
		}

        public Point3f getAtomPoint3f(int atomIndex)
		{
			return atoms[atomIndex].point3f;
		}

        public Bond getBondAt(int bondIndex)
		{
			return bonds[bondIndex];
		}
		
		private void  addBond(Bond bond)
		{
			if (bond == null)
				return ;
			if (bondCount == bonds.Length)
				bonds = (Bond[]) Util.setLength(bonds, bondCount + growthIncrement);
			bonds[bondCount++] = bond;
		}

        public void bondAtoms(Atom atom1, Atom atom2, short order)
		{
			addBond(atom1.bondMutually(atom2, order, this));
		}

        public void bondAtoms(Atom atom1, Atom atom2, short order, BitArray bsA, BitArray bsB)
		{
			bool atom1InSetA = bsA == null || bsA.Get(atom1.atomIndex);
			bool atom1InSetB = bsB == null || bsB.Get(atom1.atomIndex);
			bool atom2InSetA = bsA == null || bsA.Get(atom2.atomIndex);
			bool atom2InSetB = bsB == null || bsB.Get(atom2.atomIndex);
            if (atom1InSetA & atom2InSetB || atom1InSetB & atom2InSetA)
                addBond(atom1.bondMutually(atom2, order, this));
		}

        public Shape allocateShape(int shapeID)
		{
			string classBase = JmolConstants.shapeClassBases[shapeID];
			//    System.out.println("Frame.allocateShape(" + classBase + ")");
			string className = "org.jmol.viewer." + classBase;
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type shapeClass = System.Type.GetType(className);
				Shape shape = (Shape) System.Activator.CreateInstance(shapeClass);
                shape.setViewerG3dFrame(/*viewer, g3d,*/ this);
				return shape;
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Could not instantiate shape:" + classBase + "\n" + e);
				SupportClass.WriteStackTrace(e, Console.Error);
			}
			return null;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'shapes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'shapes' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public Shape[] shapes;

        public void loadShape(int shapeID)
		{
			if (shapes[shapeID] == null)
				shapes[shapeID] = allocateShape(shapeID);
		}

        public void setShapeSize(int shapeID, int size, BitArray bsSelected)
		{
			if (size != 0)
				loadShape(shapeID);
			if (shapes[shapeID] != null)
				shapes[shapeID].setSize(size, bsSelected);
		}

        public void setShapeProperty(int shapeID, string propertyName, object value_Renamed, BitArray bsSelected)
		{
			// miguel 2004 11 23
			// Why was I loading this?
			// loadShape(shapeID);
			if (shapes[shapeID] != null)
				shapes[shapeID].setProperty(propertyName, value_Renamed, bsSelected);
		}

        public object getShapeProperty(int shapeID, string propertyName, int index)
		{
			return (shapes[shapeID] == null?null:shapes[shapeID].getProperty(propertyName, index));
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'averageAtomPoint '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Point3f averageAtomPoint = new Point3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'centerBoundBox '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Point3f centerBoundBox = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'boundBoxCornerVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Vector3f boundBoxCornerVector = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'minBoundBox '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Point3f minBoundBox = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'maxBoundBox '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Point3f maxBoundBox = new Point3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'centerUnitcell '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Point3f centerUnitcell = new Point3f();
		
		//  float radiusBoundBox;
        public Point3f rotationCenter;
        public float rotationRadius;
        public Point3f rotationCenterDefault;
        public float rotationRadiusDefault;

        public void increaseRotationRadius(float increaseInAngstroms)
		{
			rotationRadius += increaseInAngstroms;
		}

        public void clearBounds()
		{
			rotationCenter = null;
			rotationRadius = 0;
		}
		
		private void  findBounds()
		{
			if ((rotationCenter != null) || (atomCount <= 0))
				return ;
			calcRotationSphere();
		}
		
		private void  calcRotationSphere()
		{
			calcAverageAtomPoint();
			calcBoundBoxDimensions();
			if (notionalUnitcell != null)
				calcUnitcellDimensions();
			rotationCenter = rotationCenterDefault = centerBoundBox; //averageAtomPoint;
			rotationRadius = rotationRadiusDefault = calcRotationRadius(rotationCenterDefault);
		}
		
		private void  calcAverageAtomPoint()
		{
			Point3f average = this.averageAtomPoint;
			average.set_Renamed(0, 0, 0);
			for (int i = atomCount; --i >= 0; )
				average.add(atoms[i].point3f);
			average.scale(1f / atomCount);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitBboxPoints '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Point3f[] unitBboxPoints = new Point3f[] { new Point3f(1, 1, 1), new Point3f(1, 1, -1), new Point3f(1, -1, 1), new Point3f(1, -1, -1), new Point3f(-1, 1, 1), new Point3f(-1, 1, -1), new Point3f(-1, -1, 1), new Point3f(-1, -1, -1) };
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'bboxVertices '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Point3f[] bboxVertices = new Point3f[8];
		
		private void  calcBoundBoxDimensions()
		{
			calcAtomsMinMax(minBoundBox, maxBoundBox);
			
			centerBoundBox.add(minBoundBox, maxBoundBox);
			centerBoundBox.scale(0.5f);
			boundBoxCornerVector.sub(maxBoundBox, centerBoundBox);
			
			for (int i = 8; --i >= 0; )
			{
				Point3f bbcagePoint = bboxVertices[i] = new Point3f(unitBboxPoints[i]);
				bbcagePoint.x *= boundBoxCornerVector.x;
				bbcagePoint.y *= boundBoxCornerVector.y;
				bbcagePoint.z *= boundBoxCornerVector.z;
				bbcagePoint.add(centerBoundBox);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitCubePoints '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Point3f[] unitCubePoints = new Point3f[] { new Point3f(0, 0, 0), new Point3f(0, 0, 1), new Point3f(0, 1, 0), new Point3f(0, 1, 1), new Point3f(1, 0, 0), new Point3f(1, 0, 1), new Point3f(1, 1, 0), new Point3f(1, 1, 1) };

        public Point3f[] unitcellVertices;

        public void calcUnitcellDimensions()
		{
			unitcellVertices = new Point3f[8];
			for (int i = 8; --i >= 0; )
			{
				Point3f vertex = unitcellVertices[i] = new Point3f();
				matrixFractionalToEuclidean.transform(unitCubePoints[i], vertex);
			}
			centerUnitcell.set_Renamed(unitcellVertices[7]);
			centerUnitcell.scale(0.5f);
		}

        public float calcRotationRadius(Point3f center)
		{
			
			float maxRadius = 0;
			for (int i = atomCount; --i >= 0; )
			{
				Atom atom = atoms[i];
				float distAtom = center.distance(atom.point3f);
				float radiusVdw = atom.VanderwaalsRadiusFloat;
				float outerVdw = distAtom + radiusVdw;
				if (outerVdw > maxRadius)
					maxRadius = outerVdw;
			}
			return maxRadius;
		}

        public const int measurementGrowthIncrement = 16;
        public int measurementCount = 0;
        public Measurement[] measurements = null;
		
		/*==============================================================*
		* selection handling
		*==============================================================*/

        public bool frankClicked(int x, int y)
		{
			Shape frankShape = shapes[JmolConstants.SHAPE_FRANK];
			if (frankShape == null)
				return false;
			return frankShape.wasClicked(x, y);
		}

        public const int minimumPixelSelectionRadius = 4;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'closest '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //internal Closest closest = new Closest();
		
        //internal int findNearestAtomIndex(int x, int y)
        //{
        //    closest.atom = null;
        //    for (int i = 0; i < shapes.Length; ++i)
        //    {
        //        Shape shape = shapes[i];
        //        if (shape != null)
        //        {
        //            shapes[i].findNearestAtomIndex(x, y, closest);
        //            if (closest.atom != null)
        //                break;
        //        }
        //    }
        //    int closestIndex = (closest.atom == null?- 1:closest.atom.atomIndex);
        //    closest.atom = null;
        //    return closestIndex;
        //}
		
		// jvm < 1.4 does not have a BitSet.clear();
		// so in order to clear you "and" with an empty bitset.
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsEmpty '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public BitArray bsEmpty = new System.Collections.BitArray(64);
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsFoundRectangle '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public BitArray bsFoundRectangle = new System.Collections.BitArray(64);
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public BitArray findAtomsInRectangle(ref Rectangle rect)
		{
			//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
			bsFoundRectangle.And(bsEmpty);
			for (int i = atomCount; --i >= 0; )
			{
				Atom atom = atoms[i];
				if (rect.Contains(atom.ScreenX, atom.ScreenY))
					SupportClass.BitArraySupport.Set(bsFoundRectangle, i);
			}
			return bsFoundRectangle;
		}

        public BondIterator getBondIterator(short bondType, BitArray bsSelected)
		{
			return new SelectedBondIterator(this, bondType, bsSelected);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'SelectedBondIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        public class SelectedBondIterator : BondIterator
		{
			private void  InitBlock(Frame enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Frame enclosingInstance;
			public Frame Enclosing_Instance
			{
				get { return enclosingInstance; }
			}

            public short bondType;
            public int iBond;
            public BitArray bsSelected;
            public bool bondSelectionModeOr;

            public SelectedBondIterator(Frame enclosingInstance, short bondType, BitArray bsSelected)
			{
				InitBlock(enclosingInstance);
				this.bondType = bondType;
				this.bsSelected = bsSelected;
				iBond = 0;
                //bondSelectionModeOr = Enclosing_Instance.viewer.BondSelectionModeOr;
			}
			
			public bool hasNext()
			{
				for (; iBond < Enclosing_Instance.bondCount; ++iBond)
				{
					Bond bond = Enclosing_Instance.bonds[iBond];
					// mth 2004 10 20
					// a hack put in here to support bonds of order '0'
					// during implementation of 'bondOrder' script command
					if (bondType != JmolConstants.BOND_ALL_MASK && (bond.order & bondType) == 0)
						continue;
					bool isSelected1 = bsSelected.Get(bond.atom1.atomIndex);
					bool isSelected2 = bsSelected.Get(bond.atom2.atomIndex);
					if ((!bondSelectionModeOr & isSelected1 & isSelected2) || (bondSelectionModeOr & (isSelected1 | isSelected2)))
						return true;
				}
				return false;
			}
			
			public int nextIndex()
			{
				return iBond;
			}
			
			public Bond next()
			{
				return Enclosing_Instance.bonds[iBond++];
			}
		}

        public Bspf bspf;
		
		private const bool MIX_BSPT_ORDER = false;

        public void initializeBspf()
		{
			if (bspf == null)
			{
				long timeBegin = 0;
				if (showRebondTimes)
					timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				bspf = new Bspf(3);
				if (MIX_BSPT_ORDER)
				{
					System.Console.Out.WriteLine("mixing bspt order");
					int stride = 3;
					int step = (atomCount + stride - 1) / stride;
					for (int i = 0; i < step; ++i)
						for (int j = 0; j < stride; ++j)
						{
							int k = i * stride + j;
							if (k >= atomCount)
								continue;
							Atom atom = atoms[k];
							if (!atom.Deleted)
								bspf.addTuple(atom.modelIndex, atom);
						}
				}
				else
				{
					for (int i = atomCount; --i >= 0; )
					{
						Atom atom = atoms[i];
						if (!atom.Deleted)
							bspf.addTuple(atom.modelIndex, atom);
					}
				}
				if (showRebondTimes)
				{
					long timeEnd = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
					System.Console.Out.WriteLine("time to build bspf=" + (timeEnd - timeBegin) + " ms");
					bspf.stats();
					//        bspf.dump();
				}
			}
		}
		
		private void  clearBspf()
		{
			bspf = null;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'withinModelIterator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'withinModelIterator' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private WithinModelIterator withinModelIterator;

        public AtomIterator getWithinModelIterator(Atom atomCenter, float radius)
		{
			withinModelIterator.initialize(atomCenter.modelIndex, atomCenter, radius);
			return withinModelIterator;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'WithinModelIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        public class WithinModelIterator : AtomIterator
		{
			public WithinModelIterator(Frame enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Frame enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Frame enclosingInstance;
			public Frame Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal int bsptIndex;
			internal Tuple center;
			internal float radius;
            internal SphereIterator bsptIter;

            public void initialize(int bsptIndex, Tuple center, float radius)
			{
				Enclosing_Instance.initializeBspf();
				this.bsptIndex = bsptIndex;
				bsptIter = Enclosing_Instance.bspf.getSphereIterator(bsptIndex);
				this.center = center;
				this.radius = radius;
				bsptIter.initialize(center, radius);
			}
			
			public bool hasNext()
			{
				return bsptIter.hasMoreElements();
			}
			
			public Atom next()
			{
				return (Atom) bsptIter.nextElement();
			}
			
			public void  release()
			{
				bsptIter.release();
				bsptIter = null;
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'withinAnyModelIterator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'withinAnyModelIterator' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private WithinAnyModelIterator withinAnyModelIterator;

        public AtomIterator getWithinAnyModelIterator(Atom atomCenter, float radius)
		{
			withinAnyModelIterator.initialize(atomCenter, radius);
			return withinAnyModelIterator;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'WithinAnyModelIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        public class WithinAnyModelIterator : AtomIterator
		{
			public WithinAnyModelIterator(Frame enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Frame enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Frame enclosingInstance;
			public Frame Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public int bsptIndex;
            public Tuple center;
            public float radius;
            public SphereIterator bsptIter;

            public void initialize(Tuple center, float radius)
			{
				Enclosing_Instance.initializeBspf();
				bsptIndex = Enclosing_Instance.bspf.BsptCount;
				bsptIter = null;
				this.center = center;
				this.radius = radius;
			}
			
			public bool hasNext()
			{
				while (bsptIter == null || !bsptIter.hasMoreElements())
				{
					if (--bsptIndex < 0)
					{
						bsptIter = null;
						return false;
					}
					bsptIter = Enclosing_Instance.bspf.getSphereIterator(bsptIndex);
					bsptIter.initialize(center, radius);
				}
				return true;
			}
			
			public Atom next()
			{
				return (Atom) bsptIter.nextElement();
			}
			
			public void  release()
			{
				bsptIter.release();
				bsptIter = null;
			}
		}
		
		////////////////////////////////////////////////////////////////
		// autobonding stuff
		////////////////////////////////////////////////////////////////
        public void doAutobond()
		{
			// perform bonding if necessary
            //if (viewer.AutoBond && getModelSetProperty("noautobond") == null)
            //{
            //    if ((bondCount == 0) || ((object) modelSetTypeName == (object) "pdb" && (bondCount < (atomCount / 2))))
            //        autoBond(null, null);
            //}
		}

        public const bool showRebondTimes = false;
		
		private short getBondOrder(Atom atomA, float bondingRadiusA, Atom atomB, float bondingRadiusB, float distance2, float minBondDistance2, float bondTolerance)
		{
			//            System.out.println(" radiusA=" + bondingRadiusA +
			//                               " radiusB=" + bondingRadiusB +
			//                         " distance2=" + distance2 +
			//                         " tolerance=" + bondTolerance);
			if (bondingRadiusA == 0 || bondingRadiusB == 0)
				return 0;
			float maxAcceptable = bondingRadiusA + bondingRadiusB + bondTolerance;
			float maxAcceptable2 = maxAcceptable * maxAcceptable;
			if (distance2 < minBondDistance2)
			{
				//System.out.println("less than minBondDistance");
				return 0;
			}
			if (distance2 <= maxAcceptable2)
			{
				//System.out.println("returning 1");
				return 1;
			}
			return 0;
		}

        public void checkValencesAndBond(Atom atomA, Atom atomB, short order)
		{
			//    System.out.println("checkValencesAndBond(" +
			//                       atomA.point3f + "," + atomB.point3f + ")");
			if (atomA.CurrentBondCount > JmolConstants.MAXIMUM_AUTO_BOND_COUNT || atomB.CurrentBondCount > JmolConstants.MAXIMUM_AUTO_BOND_COUNT)
			{
				System.Console.Out.WriteLine("maximum auto bond count reached");
				return ;
			}
			int formalChargeA = atomA.FormalCharge;
			if (formalChargeA != 0)
			{
				int formalChargeB = atomB.FormalCharge;
				if ((formalChargeA < 0 && formalChargeB < 0) || (formalChargeA > 0 && formalChargeB > 0))
					return ;
			}
			addBond(atomA.bondMutually(atomB, order, this));
		}
		
		// null values for bitsets means "all"
        public void autoBond(BitArray bsA, BitArray bsB)
		{
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Float.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			if (maxBondingRadius == System.Single.Epsilon)
				findMaxRadii();
            //float bondTolerance = viewer.BondTolerance;
            //float minBondDistance = viewer.MinBondDistance;
            //float minBondDistance2 = minBondDistance * minBondDistance;
			
			//char chainLast = '?';
			//int indexLastCA = -1;
			//Atom atomLastCA = null;
			
			initializeBspf();
			
			long timeBegin = 0;
			if (showRebondTimes)
				timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			/*
			* miguel 2006 04 02
			* note that the way that these loops + iterators are constructed,
			* everything assumes that all possible pairs of atoms are going to
			* be looked at.
			* for example, the hemisphere iterator will only look at atom indexes
			* that are >= (or <= ?) the specified atom.
			* if we are going to allow arbitrary sets bsA and bsB, then this will
			* not work.
			* so, for now I will do it the ugly way.
			* maybe enhance/improve in the future.
			*/
			for (int i = atomCount; --i >= 0; )
			{
				bool isAtomInSetA = (bsA == null || bsA.Get(i));
				bool isAtomInSetB = (bsB == null || bsB.Get(i));
				if (!isAtomInSetA & !isAtomInSetB)
					continue;
				Atom atom = atoms[i];
				// Covalent bonds
				float myBondingRadius = atom.BondingRadiusFloat;
				if (myBondingRadius == 0)
					continue;
                //float searchRadius = myBondingRadius + maxBondingRadius + bondTolerance;
				SphereIterator iter = bspf.getSphereIterator(atom.modelIndex);
                //iter.initializeHemisphere(atom, searchRadius);
				while (iter.hasMoreElements())
				{
					Atom atomNear = (Atom) iter.nextElement();
					if (atomNear == atom)
						continue;
					int atomIndexNear = atomNear.atomIndex;
					bool isNearInSetA = (bsA == null || bsA.Get(atomIndexNear));
					bool isNearInSetB = (bsB == null || bsB.Get(atomIndexNear));
					if (!isNearInSetA & !isNearInSetB)
						continue;
					if (!(isAtomInSetA & isNearInSetB || isAtomInSetB & isNearInSetA))
						continue;
                    //short order = getBondOrder(atom, myBondingRadius, atomNear, atomNear.BondingRadiusFloat, iter.foundDistance2(), minBondDistance2, bondTolerance);
                    //if (order > 0)
                    //    checkValencesAndBond(atom, atomNear, order);
				}
				iter.release();
			}
			
			if (showRebondTimes)
			{
				long timeEnd = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				System.Console.Out.WriteLine("Time to autoBond=" + (timeEnd - timeBegin));
			}
		}
		
		////////////////////////////////////////////////////////////////
		// hbond code

        public float hbondMax = 3.25f;
        public float hbondMin = 2.5f;
		//UPGRADE_NOTE: The initialization of  'hbondMin2' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public float hbondMin2;

        public bool useRasMolHbondsCalculation = true;

        public void autoHbond(BitArray bsA, BitArray bsB)
		{
			if (useRasMolHbondsCalculation)
			{
				if (mmset != null)
					mmset.calcHydrogenBonds(bsA, bsB);
				return ;
			}
			initializeBspf();
			long timeBegin = 0;
			if (showRebondTimes)
				timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			for (int i = atomCount; --i >= 0; )
			{
				Atom atom = atoms[i];
				int elementNumber = atom.elementNumber;
				if (elementNumber != 7 && elementNumber != 8)
					continue;
				//float searchRadius = hbondMax;
				SphereIterator iter = bspf.getSphereIterator(atom.modelIndex);
				iter.initializeHemisphere(atom, hbondMax);
				while (iter.hasMoreElements())
				{
					Atom atomNear = (Atom) iter.nextElement();
					int elementNumberNear = atomNear.elementNumber;
					if (elementNumberNear != 7 && elementNumberNear != 8)
						continue;
					if (atomNear == atom)
						continue;
					if (iter.foundDistance2() < hbondMin2)
						continue;
					if (atom.isBonded(atomNear))
						continue;
					addBond(atom.bondMutually(atomNear, JmolConstants.BOND_H_REGULAR, this));
					System.Console.Out.WriteLine("adding an hbond between " + atom.atomIndex + " & " + atomNear.atomIndex);
				}
				iter.release();
			}
			
			if (showRebondTimes)
			{
				long timeEnd = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				System.Console.Out.WriteLine("Time to hbond=" + (timeEnd - timeBegin));
			}
		}


        public void deleteAllBonds()
		{
			for (int i = bondCount; --i >= 0; )
			{
				bonds[i].deleteAtomReferences();
				bonds[i] = null;
			}
			bondCount = 0;
		}

        public void deleteBond(Bond bond)
		{
			// what a disaster ... I hate doing this
			for (int i = bondCount; --i >= 0; )
			{
				if (bonds[i] == bond)
				{
					bonds[i].deleteAtomReferences();
					Array.Copy(bonds, i + 1, bonds, i, bondCount - i - 1);
					--bondCount;
					bonds[bondCount] = null;
					return ;
				}
			}
		}

        public void deleteBonds(BitArray bs)
		{
			int iSrc = 0;
			int iDst = 0;
			for (; iSrc < bondCount; ++iSrc)
			{
				Bond bond = bonds[iSrc];
				if (!bs.Get(iSrc))
					bonds[iDst++] = bond;
				else
					bond.deleteAtomReferences();
			}
			for (int i = bondCount; --i >= iDst; )
				bonds[i] = null;
			bondCount = iDst;
		}

        public void deleteCovalentBonds()
		{
			int indexNoncovalent = 0;
			for (int i = 0; i < bondCount; ++i)
			{
				Bond bond = bonds[i];
				if (bond == null)
					continue;
				if (!bond.Covalent)
				{
					if (i != indexNoncovalent)
					{
						bonds[indexNoncovalent++] = bond;
						bonds[i] = null;
					}
				}
				else
				{
					bond.deleteAtomReferences();
					bonds[i] = null;
				}
			}
			bondCount = indexNoncovalent;
		}

        public void deleteAtom(int atomIndex)
		{
			clearBspf();
			atoms[atomIndex].markDeleted();
		}

        public ShapeRenderer getRenderer(int shapeID)
        {
            return frameRenderer.getRenderer(shapeID);
        }

        public void doUnitcellStuff()
		{
			if (notionalUnitcell != null)
			{
				// convert fractional coordinates to cartesian
				constructFractionalMatrices();
				if (fileCoordinatesAreFractional)
					convertFractionalToEuclidean();
			}
			/*
			mth 2004 03 06
			We do not want to pack the unitcell automatically.
			putAtomsInsideUnitcell();
			*/
		}

        public void constructFractionalMatrices()
		{
			matrixNotional = new Matrix3f();
			calcNotionalMatrix(notionalUnitcell, matrixNotional);
			if (pdbScaleMatrix != null)
			{
				//      System.out.println("using PDB Scale matrix");
				matrixEuclideanToFractional = new Matrix3f();
				matrixEuclideanToFractional.transpose(pdbScaleMatrix);
				matrixFractionalToEuclidean = new Matrix3f();
				matrixFractionalToEuclidean.invert(matrixEuclideanToFractional);
			}
			else
			{
				//      System.out.println("using notional unit cell");
				matrixFractionalToEuclidean = matrixNotional;
				matrixEuclideanToFractional = new Matrix3f();
				matrixEuclideanToFractional.invert(matrixFractionalToEuclidean);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'toRadians '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        public static readonly float toRadians = (float)System.Math.PI * 2 / 360;

        public void calcNotionalMatrix(float[] notionalUnitcell, Matrix3f matrixNotional)
		{
			// note that these are oriented as columns, not as row
			// this is because we will later use the transform method,
			// which multiplies the matrix by the point, not the point by the matrix
			float gamma = notionalUnitcell[5];
			float beta = notionalUnitcell[4];
			float alpha = notionalUnitcell[3];
			float c = notionalUnitcell[2];
			float b = notionalUnitcell[1];
			float a = notionalUnitcell[0];
			
			/* some intermediate variables */
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float cosAlpha = (float) System.Math.Cos(toRadians * alpha);
			//float sinAlpha = (float)Math.sin(toRadians * alpha);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float cosBeta = (float) System.Math.Cos(toRadians * beta);
			//float sinBeta  = (float)Math.sin(toRadians * beta);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float cosGamma = (float) System.Math.Cos(toRadians * gamma);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float sinGamma = (float) System.Math.Sin(toRadians * gamma);
			
			
			// 1. align the a axis with x axis
			matrixNotional.setColumn(0, a, 0, 0);
			// 2. place the b is in xy plane making a angle gamma with a
			matrixNotional.setColumn(1, b * cosGamma, b * sinGamma, 0);
			// 3. now the c axis,
			// http://server.ccl.net/cca/documents/molecular-modeling/node4.html
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float V = a * b * c * (float) System.Math.Sqrt(1.0 - cosAlpha * cosAlpha - cosBeta * cosBeta - cosGamma * cosGamma + 2.0 * cosAlpha * cosBeta * cosGamma);
			matrixNotional.setColumn(2, c * cosBeta, c * (cosAlpha - cosBeta * cosGamma) / sinGamma, V / (a * b * sinGamma));
		}

        public void putAtomsInsideUnitcell()
		{
			/*
			* find connected-sets ... aka 'molecules'
			* convert to fractional coordinates
			* for each connected-set
			*   find its center
			*   if the center is outside the unitcell
			*     move the atoms
			* convert back to euclidean coordinates
			****************************************************************/
			convertEuclideanToFractional();
			// but for now, just do one connected-set
			Point3f adjustment = findFractionalAdjustment();
			if (adjustment.x != 0 || adjustment.y != 0 || adjustment.z != 0)
				applyFractionalAdjustment(adjustment);
			convertFractionalToEuclidean();
		}

        public void convertEuclideanToFractional()
		{
			for (int i = atomCount; --i >= 0; )
				matrixEuclideanToFractional.transform(atoms[i].point3f);
		}

        public void convertFractionalToEuclidean()
		{
			for (int i = atomCount; --i >= 0; )
				matrixFractionalToEuclidean.transform(atoms[i].point3f);
		}

        public Point3f findFractionalAdjustment()
		{
			Point3f pointMin = new Point3f();
			Point3f pointMax = new Point3f();
			calcAtomsMinMax(pointMin, pointMax);
			pointMin.add(pointMax);
			pointMin.scale(0.5f);
			
			Point3f fractionalCenter = pointMin;
			System.Console.Out.WriteLine("fractionalCenter=" + fractionalCenter);
			Point3f adjustment = pointMax;
			adjustment.set_Renamed((float) Math.Floor(fractionalCenter.x), (float) Math.Floor(fractionalCenter.y), (float) Math.Floor(fractionalCenter.z));
			return adjustment;
		}

        public void applyFractionalAdjustment(Point3f adjustment)
		{
			System.Console.Out.WriteLine("applyFractionalAdjustment(" + adjustment + ")");
			for (int i = atomCount; --i >= 0; )
				atoms[i].point3f.sub(adjustment);
		}

        public void calcAtomsMinMax(Point3f pointMin, Point3f pointMax)
		{
			float minX, minY, minZ, maxX, maxY, maxZ;
			Point3f pointT;
			pointT = atoms[0].point3f;
			minX = maxX = pointT.x;
			minY = maxY = pointT.y;
			minZ = maxZ = pointT.z;
			
			for (int i = atomCount; --i > 0; )
			{
				// note that the 0 element was set above
				pointT = atoms[i].point3f;
				float t;
				t = pointT.x;
				if (t < minX)
					minX = t;
				else if (t > maxX)
					maxX = t;

				t = pointT.y;
				if (t < minY)
					minY = t;
				else if (t > maxY)
					maxY = t;

				t = pointT.z;
				if (t < minZ)
					minZ = t;
				else if (t > maxZ)
					maxZ = t;
			}
			pointMin.set_Renamed(minX, minY, minZ);
			pointMax.set_Renamed(maxX, maxY, maxZ);
		}

        public void setLabel(string label, int atomIndex)
		{ }

        public void findElementsPresent()
		{
			elementsPresent = new BitArray(64);
			for (int i = atomCount; --i >= 0; )
				SupportClass.BitArraySupport.Set(elementsPresent, atoms[i].elementNumber);
		}

        public void findGroupsPresent()
		{
			Group groupLast = null;
			groupsPresent = new System.Collections.BitArray(64);
			for (int i = atomCount; --i >= 0; )
			{
				if (groupLast != atoms[i].group)
				{
					groupLast = atoms[i].group;
					SupportClass.BitArraySupport.Set(groupsPresent, groupLast.GroupID);
				}
			}
		}

        public void calcSelectedGroupsCount(System.Collections.BitArray bsSelected)
		{
			mmset.calcSelectedGroupsCount(bsSelected);
		}

        public void calcSelectedMonomersCount(System.Collections.BitArray bsSelected)
		{
			mmset.calcSelectedMonomersCount(bsSelected);
		}

        public void findMaxRadii()
		{
			for (int i = atomCount; --i >= 0; )
			{
				Atom atom = atoms[i];
				float bondingRadius = atom.BondingRadiusFloat;
				if (bondingRadius > maxBondingRadius)
					maxBondingRadius = bondingRadius;
				float vdwRadius = atom.VanderwaalsRadiusFloat;
				if (vdwRadius > maxVanderwaalsRadius)
					maxVanderwaalsRadius = vdwRadius;
			}
		}

        public void calcBfactorRange()
		{
			if (!hasBfactorRange)
			{
				bfactor100Lo = bfactor100Hi = atoms[0].Bfactor100;
				for (int i = atomCount; --i > 0; )
				{
					int bf = atoms[i].Bfactor100;
					if (bf < bfactor100Lo)
						bfactor100Lo = bf;
					else if (bf > bfactor100Hi)
						bfactor100Hi = bf;
				}
				hasBfactorRange = true;
			}
		}
		
		////////////////////////////////////////////////////////////////
		// measurements
		////////////////////////////////////////////////////////////////

        public float getDistance(int atomIndexA, int atomIndexB)
		{
			return atoms[atomIndexA].point3f.distance(atoms[atomIndexB].point3f);
		}

        public Vector3f vectorBA;
        public Vector3f vectorBC;

        public float getAngle(int atomIndexA, int atomIndexB, int atomIndexC)
		{
			if (vectorBA == null)
			{
				vectorBA = new Vector3f();
				vectorBC = new Vector3f();
			}
			Point3f pointA = atoms[atomIndexA].point3f;
			Point3f pointB = atoms[atomIndexB].point3f;
			Point3f pointC = atoms[atomIndexC].point3f;
			vectorBA.sub(pointA, pointB);
			vectorBC.sub(pointC, pointB);
			float angle = vectorBA.angle(vectorBC);
			float degrees = toDegrees(angle);
			return degrees;
		}

        public float getTorsion(int atomIndexA, int atomIndexB, int atomIndexC, int atomIndexD)
		{
			return computeTorsion(atoms[atomIndexA].point3f, atoms[atomIndexB].point3f, atoms[atomIndexC].point3f, atoms[atomIndexD].point3f);
		}

        public static float toDegrees(float angleRadians)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return angleRadians * 180 / (float) System.Math.PI;
		}

        public static float computeTorsion(Point3f p1, Point3f p2, Point3f p3, Point3f p4)
		{
			float ijx = p1.x - p2.x;
			float ijy = p1.y - p2.y;
			float ijz = p1.z - p2.z;
			
			float kjx = p3.x - p2.x;
			float kjy = p3.y - p2.y;
			float kjz = p3.z - p2.z;
			
			float klx = p3.x - p4.x;
			float kly = p3.y - p4.y;
			float klz = p3.z - p4.z;
			
			float ax = ijy * kjz - ijz * kjy;
			float ay = ijz * kjx - ijx * kjz;
			float az = ijx * kjy - ijy * kjx;
			float cx = kjy * klz - kjz * kly;
			float cy = kjz * klx - kjx * klz;
			float cz = kjx * kly - kjy * klx;
			
			float ai2 = 1f / (ax * ax + ay * ay + az * az);
			float ci2 = 1f / (cx * cx + cy * cy + cz * cz);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float ai = (float) System.Math.Sqrt(ai2);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float ci = (float) System.Math.Sqrt(ci2);
			float denom = ai * ci;
			float cross = ax * cx + ay * cy + az * cz;
			float cosang = cross * denom;
			if (cosang > 1)
				cosang = 1;
			if (cosang < - 1)
				cosang = - 1;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float torsion = toDegrees((float) System.Math.Acos(cosang));
			float dot = ijx * cx + ijy * cy + ijz * cz;
			float absDot = System.Math.Abs(dot);
			torsion = (dot / absDot > 0)?torsion:- torsion;
			return torsion;
		}
		
		////////////////////////////////////////////////////////////////

        public BitArray getGroupBitSet(int atomIndex)
		{
			BitArray bsGroup = new BitArray(64);
			atoms[atomIndex].group.selectAtoms(bsGroup);
			return bsGroup;
		}

        public BitArray getChainBitSet(int atomIndex)
		{
			BitArray bsChain = new BitArray(64);
			atoms[atomIndex].group.chain.selectAtoms(bsChain);
			return bsChain;
		}

        public void selectSeqcodeRange(int seqcodeA, int seqcodeB, BitArray bs)
		{
			mmset.selectSeqcodeRange(seqcodeA, seqcodeB, bs);
		}
		
		////////////////////////////////////////////////////////////////

        public const int MAX_BONDS_LENGTH_TO_CACHE = 5;
        public const int MAX_NUM_TO_CACHE = 200;
        public int[] numCached = new int[MAX_BONDS_LENGTH_TO_CACHE];
        public Bond[][][] freeBonds = new Bond[MAX_BONDS_LENGTH_TO_CACHE][][];

        public Bond[] addToBonds(Bond newBond, Bond[] oldBonds)
		{
			Bond[] newBonds;
			if (oldBonds == null)
			{
				if (numCached[1] > 0)
					newBonds = freeBonds[1][--numCached[1]];
				else
					newBonds = new Bond[1];
				newBonds[0] = newBond;
			}
			else
			{
				int oldLength = oldBonds.Length;
				int newLength = oldLength + 1;
				if (newLength < MAX_BONDS_LENGTH_TO_CACHE && numCached[newLength] > 0)
					newBonds = freeBonds[newLength][--numCached[newLength]];
				else
					newBonds = new Bond[newLength];
				newBonds[oldLength] = newBond;
				for (int i = oldLength; --i >= 0; )
					newBonds[i] = oldBonds[i];
				if (oldLength < MAX_BONDS_LENGTH_TO_CACHE && numCached[oldLength] < MAX_NUM_TO_CACHE)
					freeBonds[oldLength][numCached[oldLength]++] = oldBonds;
			}
			return newBonds;
		}

        public void freeBondsCache()
		{
			for (int i = MAX_BONDS_LENGTH_TO_CACHE; --i > 0;)
			{
				// .GT. 0
				numCached[i] = 0;
				Bond[][] bondsCache = freeBonds[i];
				for (int j = bondsCache.Length; --j >= 0; )
					bondsCache[j] = null;
			}
		}
	}
}