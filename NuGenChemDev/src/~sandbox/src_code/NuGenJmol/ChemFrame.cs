using System;
using System.Collections.Generic;
using System.Text;
using Org.Jmol.Viewer;
using System.Collections;
using Org.OpenScience.CDK.Protein.Data;
using System.Collections.Specialized;
using javax.vecmath;
using NuGenJmol;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Org.Jmol.Bspt;

namespace Org.Jmol.Viewer//NuGenJmol
{
    class /*Chem*/Frame
    {
        internal Mmset mmset;
        internal Atom[] atoms;
        Bond[] bonds;
        Group[] groups;
        Dictionary<object, Atom> htAtomMap = new Dictionary<object,Atom>();

        internal FrameRenderer frameRenderer;
        NuGraphics3D g3d;

        internal Shape[] shapes;

        internal int atomCount;
        int groupCount;
        int bondCount;
        bool fileHasHbonds;

        static readonly int defaultGroupCount = 32;
        static readonly int ATOM_GROWTH_INCREMENT = 2000;
        Chain[] chains = new Chain[defaultGroupCount];
        string[] group3s = new string[defaultGroupCount];
        int[] seqcodes = new int[defaultGroupCount];
        int[] firstAtomIndexes = new int[defaultGroupCount];
        readonly int[] specialAtomIndexes = new int[JmolConstants.ATOMID_MAX];

        bool hasVibrationVectors_Renamed_Field;

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

        bool structuresDefined;

        int currentModelIndex;
        Model currentModel;
        char currentChainID;
        Chain currentChain;
        int currentGroupSequenceNumber;
        char currentGroupInsertionCode;

        public const int MAX_BONDS_LENGTH_TO_CACHE = 5;
        public const int MAX_NUM_TO_CACHE = 200;
        public int[] numCached = new int[MAX_BONDS_LENGTH_TO_CACHE];
        public Bond[][][] freeBonds = new Bond[MAX_BONDS_LENGTH_TO_CACHE][][];

        public BitArray elementsPresent;
        public BitArray groupsPresent;

        private const int growthIncrement = 250;

        public Vector3f vectorBA;
        public Vector3f vectorBC;

        public float maxBondingRadius = System.Single.Epsilon;
        public float maxVanderwaalsRadius = System.Single.Epsilon;

        public Bspf bspf;
        private const bool MIX_BSPT_ORDER = false;

        #region Properties

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

        //public string ModelSetTypeName
        //{
        //    get { return modelSetTypeName; }
        //}

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

        #endregion

        #region Gets

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

        #endregion

        public void Render(bool ribbons, bool cartoons)
        {
            Rectangle r = new Rectangle();
            if (ribbons)
                frameRenderer.render(g3d, ref r, this, 0, JmolConstants.SHAPE_RIBBONS);
            if (cartoons)
                frameRenderer.render(g3d, ref r, this, 0, JmolConstants.SHAPE_CARTOON);
        }

        public NuSceneBuffer3D GetG3DSceneBuffer()
        {
            return g3d.SceneBuffer;
        }

        public void ClearG3DSceneBuffer()
        {
            g3d.Clear();
        }

        public /*Chem*/Frame(Org.OpenScience.CDK.Interfaces.IChemFile chemFile, Device graphicsDevice)
        {
            shapes = new Shape[JmolConstants.SHAPE_MAX];
            this.g3d = new NuGraphics3D(graphicsDevice);
            this.frameRenderer = new FrameRenderer();

            for (int i = MAX_BONDS_LENGTH_TO_CACHE; --i > 0; ) // .GT. 0
                freeBonds[i] = new Bond[MAX_NUM_TO_CACHE][];

            // convert to jmol native
            mmset = new Mmset(this);

            // set properties
            mmset.ModelSetProperties = new System.Collections.Specialized.NameValueCollection();

            // init build
            currentModelIndex = -1;
            currentModel = null;
            currentChainID = '\uFFFF';
            currentChain = null;
            currentGroupSequenceNumber = -1;
            currentGroupInsertionCode = '\uFFFF';

            int atomCountEstimate = 0;
            for (int seq = 0; seq < chemFile.ChemSequenceCount; seq++)
            {
                for (int model = 0; model < chemFile.ChemSequences[seq].ChemModelCount; model++)
                {
                    Org.OpenScience.CDK.Interfaces.IChemModel chemModel = chemFile.ChemSequences[seq].ChemModels[model];
                    for (int atomC = 0; atomC < chemModel.SetOfMolecules.AtomContainerCount; atomC++)
                    {
                        atomCountEstimate += chemModel.SetOfMolecules.AtomContainers[atomC].AtomCount;
                    }
                }
            }

            if (atomCountEstimate <= 0)
                atomCountEstimate = ATOM_GROWTH_INCREMENT;
            atoms = new Atom[atomCountEstimate];
            bonds = new Bond[2 * atomCountEstimate];
            htAtomMap.Clear();

            // translate IChemSequence[] into Model[]
            mmset.ModelCount = chemFile.ChemSequenceCount;
            for (int seq = 0; seq < chemFile.ChemSequenceCount; ++seq)
            {
                int modelNumber = seq + 1;
                string modelName = modelNumber.ToString();
                NameValueCollection modelProperties = new NameValueCollection();    // FIXME: Loading property values
                mmset.setModelNameNumberProperties(seq, modelName, modelNumber, modelProperties);
            }

            // translate Atoms
            Dictionary<Org.OpenScience.CDK.Interfaces.IAtom, Atom> atomsList = new Dictionary<Org.OpenScience.CDK.Interfaces.IAtom, Atom>();
            //try
            //{
                for (int seq = 0; seq < chemFile.ChemSequenceCount; seq++)
                {
                    for (int model = 0; model < chemFile.ChemSequences[seq].ChemModelCount; model++)
                    {
                        Org.OpenScience.CDK.Interfaces.IChemModel chemModel = chemFile.ChemSequences[seq].ChemModels[model];
                        for (int atomC = 0; atomC < chemModel.SetOfMolecules.AtomContainerCount; atomC++)
                        {
                            for (int atomIdx = 0; atomIdx < chemModel.SetOfMolecules.AtomContainers[atomC].AtomCount; atomIdx++)
                            {
                                Org.OpenScience.CDK.Interfaces.IAtom atom = chemModel.SetOfMolecules.AtomContainers[atomC].Atoms[atomIdx];

                                sbyte elementNumber = (sbyte)atom.AtomicNumber;
                                if (elementNumber <= 0)
                                    elementNumber = JmolConstants.elementNumberFromSymbol(atom.Symbol);
                                char alternateLocation = '\0';

                                int sequenceNumber = int.MinValue;
                                char groupInsertionCode = '\0';

                                string atomName = null;
                                string group3Name = null;
                                char chainID = '\0';
                                if (atom is PDBAtom)
                                {
                                    PDBAtom pdbAtom = (PDBAtom)atom;
                                    if (pdbAtom.ResSeq != null && pdbAtom.ResSeq.Length > 0)
                                        sequenceNumber = int.Parse(pdbAtom.ResSeq);
                                    if (pdbAtom.ICode != null && pdbAtom.ICode.Length > 0)
                                        groupInsertionCode = pdbAtom.ICode[0];

                                    atomName = pdbAtom.Name;
                                    group3Name = pdbAtom.ResName;
                                    if (pdbAtom.ChainID != null && pdbAtom.ChainID.Length >= 1)
                                        chainID = pdbAtom.ChainID[0];
                                }
                                else
                                    atomName = atom.AtomTypeName;
                                
                                atomsList[atom] = AddAtom(model, atom, elementNumber, atomName, atom.getFormalCharge(),
                                                          (float)atom.getCharge(), 100, float.NaN, (float)atom.X3d, (float)atom.Y3d,
                                                          (float)atom.Z3d, false, int.MinValue, chainID, group3Name, sequenceNumber,
                                                          groupInsertionCode, float.NaN, float.NaN, float.NaN, alternateLocation, null);
                            }
                        }
                    }
                }
            //}
            //catch (Exception e)
            //{
            //    throw new ApplicationException("Problem translating atoms", e);
            //}

            fileHasHbonds = false;

            // translate bonds
            try
            {
                for (int seq = 0; seq < chemFile.ChemSequenceCount; seq++)
                {
                    for (int model = 0; model < chemFile.ChemSequences[seq].ChemModelCount; model++)
                    {
                        Org.OpenScience.CDK.Interfaces.IChemModel chemModel = chemFile.ChemSequences[seq].ChemModels[model];

                        for (int atomC = 0; atomC < chemModel.SetOfMolecules.AtomContainerCount; atomC++)
                        {
                            for (int bondIdx = 0; bondIdx < chemModel.SetOfMolecules.AtomContainers[atomC].Bonds.Length; bondIdx++)
                            {
                                Org.OpenScience.CDK.Interfaces.IBond bond = chemModel.SetOfMolecules.AtomContainers[atomC].Bonds[bondIdx];
                                Org.OpenScience.CDK.Interfaces.IAtom[] cdkAtoms = bond.getAtoms();
                                // locate translated atoms
                                Atom atom1, atom2;
                                atomsList.TryGetValue(cdkAtoms[0], out atom1);
                                atomsList.TryGetValue(cdkAtoms[1], out atom2);
                                bondAtoms(atom1, atom2, (int)bond.Order);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Problem translating bonds", e);
            }
            atomsList.Clear();

            // translate structures of PDBPolymer only
            for (int seq = 0; seq < chemFile.ChemSequenceCount; seq++)
            {
                for (int model = 0; model < chemFile.ChemSequences[seq].ChemModelCount; model++)
                {
                    Org.OpenScience.CDK.Interfaces.IChemModel chemModel = chemFile.ChemSequences[seq].ChemModels[model];

                    foreach (Org.OpenScience.CDK.Interfaces.IMolecule molecule in chemModel.SetOfMolecules.Molecules)
                    {
                        if (molecule is PDBPolymer)
                        {
                            PDBPolymer pdbPolymer = (PDBPolymer)molecule;
                            if (pdbPolymer.Structures != null && pdbPolymer.Structures.Count > 0)
                            {
                                structuresDefined = true;
                                foreach (PDBStructure pdbStruct in pdbPolymer.Structures)
                                {
                                    structuresDefined = true;
                                    mmset.defineStructure(pdbStruct.StructureType, pdbStruct.StartChainID,
                                                          pdbStruct.StartSequenceNumber, pdbStruct.StartInsertionCode,
                                                          pdbStruct.EndChainID, pdbStruct.EndSequenceNumber, pdbStruct.EndInsertionCode);
                                }
                            }
                        }
                    }
                }
            }
            autoBond(null, null);

            // build groups
            FinalizeGroupBuild();
            BuildPolymers();
            Freeze();

            finalizeBuild();

            // create ribbon shapes
            try
            {
                setShapeSize(JmolConstants.SHAPE_RIBBONS, -1, new BitArray(0));
                setShapeSize(JmolConstants.SHAPE_CARTOON, -1, new BitArray(0));
            }
            catch (Exception) { }
        }

        public void autoBond(BitArray bsA, BitArray bsB)
        {
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Float.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            if (maxBondingRadius == System.Single.Epsilon)
                findMaxRadii();
            float bondTolerance = 0.45f;// viewer.BondTolerance;
            float minBondDistance = 0.4f;//viewer.MinBondDistance;
            float minBondDistance2 = minBondDistance * minBondDistance;

            //char chainLast = '?';
            //int indexLastCA = -1;
            //Atom atomLastCA = null;

            initializeBspf();

            //long timeBegin = 0;
            //if (showRebondTimes)
            //    timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
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
                float searchRadius = myBondingRadius + maxBondingRadius + bondTolerance;
                SphereIterator iter = bspf.getSphereIterator(atom.modelIndex);
                iter.initializeHemisphere(atom, searchRadius);
                while (iter.hasMoreElements())
                {
                    Atom atomNear = (Atom)iter.nextElement();
                    if (atomNear == atom)
                        continue;
                    int atomIndexNear = atomNear.atomIndex;
                    bool isNearInSetA = (bsA == null || bsA.Get(atomIndexNear));
                    bool isNearInSetB = (bsB == null || bsB.Get(atomIndexNear));
                    if (!isNearInSetA & !isNearInSetB)
                        continue;
                    if (!(isAtomInSetA & isNearInSetB || isAtomInSetB & isNearInSetA))
                        continue;
                    short order = getBondOrder(atom, myBondingRadius, atomNear, atomNear.BondingRadiusFloat, iter.foundDistance2(), minBondDistance2, bondTolerance);
                    if (order > 0)
                        checkValencesAndBond(atom, atomNear, order);
                }
                iter.release();
            }

            //if (showRebondTimes)
            //{
            //    long timeEnd = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            //    System.Console.Out.WriteLine("Time to autoBond=" + (timeEnd - timeBegin));
            //}
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

        public void initializeBspf()
        {
            if (bspf == null)
            {
                //long timeBegin = 0;
                //if (showRebondTimes)
                //    timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
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
                //if (showRebondTimes)
                //{
                //    long timeEnd = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
                //    System.Console.Out.WriteLine("time to build bspf=" + (timeEnd - timeBegin) + " ms");
                //    bspf.stats();
                //    //        bspf.dump();
                //}
            }
        }

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
                return;
            }
            int formalChargeA = atomA.FormalCharge;
            if (formalChargeA != 0)
            {
                int formalChargeB = atomB.FormalCharge;
                if ((formalChargeA < 0 && formalChargeB < 0) || (formalChargeA > 0 && formalChargeB > 0))
                    return;
            }
            addBond(atomA.bondMutually(atomB, order, this));
        }

        public void bondAtoms(Atom atom1, Atom atom2, int order)
        {
            // skip ht lookup - pointless
            if (bondCount == bonds.Length)
                bonds = (Bond[])Util.setLength(bonds, bondCount + 2 * ATOM_GROWTH_INCREMENT);
            // note that if the atoms are already bonded then
            // Atom.bondMutually(...) will return null
            Bond bond = atom1.bondMutually(atom2, (short)order, this);
            if (bond != null)
            {
                bonds[bondCount++] = bond;
                if ((order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
                    fileHasHbonds = true;
            }
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

        private void addBond(Bond bond)
        {
            if (bond == null)
                return;
            if (bondCount == bonds.Length)
                bonds = (Bond[])Util.setLength(bonds, bondCount + growthIncrement);
            bonds[bondCount++] = bond;
        }

        private Atom AddAtom(int modelIndex, object atomUid, sbyte atomicNumber, string atomName,
                             int formalCharge, float partialCharge, int occupancy, float bfactor,
                             float x, float y, float z, bool isHetero, int atomSerial, char chainID,
                             string group3, int groupSequenceNumber, char groupInsertionCode,
                             float vectorX, float vectorY, float vectorZ, char alternateLocationID,
                             object clientAtomReference)
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
            if (groupSequenceNumber != currentGroupSequenceNumber ||
                groupInsertionCode != currentGroupInsertionCode)
            {
                currentGroupSequenceNumber = groupSequenceNumber;
                currentGroupInsertionCode = groupInsertionCode;
                startGroup(currentChain, group3, groupSequenceNumber, groupInsertionCode, atomCount);
            }

            if (atomCount == atoms.Length)
                growAtomArrays();

            Atom atom = new Atom(this,
                                 currentModelIndex,
                                 atomCount,
                                 atomicNumber,
                                 atomName,
                                 formalCharge, partialCharge,
                                 occupancy,
                                 bfactor,
                                 x, y, z,
                                 isHetero, atomSerial, chainID,
                                 vectorX, vectorY, vectorZ,
                                 alternateLocationID,
                                 clientAtomReference);

            atoms[atomCount] = atom;
            ++atomCount;
            htAtomMap[atomUid] = atom;

            return atom;
        }

        private void startGroup(Chain chain, string group3, int groupSequenceNumber,
                                char groupInsertionCode, int firstAtomIndex)
        {
            if (groupCount == group3s.Length)
            {
                chains = (Chain[])Util.doubleLength(chains);
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

        private void growAtomArrays()
        {
            int newLength = atomCount + ATOM_GROWTH_INCREMENT;
            atoms = (Atom[])Util.setLength(atoms, newLength);
            if (clientAtomReferences != null)
                clientAtomReferences = (object[])Util.setLength(clientAtomReferences, newLength);
            if (vibrationVectors != null)
                vibrationVectors = (Vector3f[])Util.setLength(vibrationVectors, newLength);
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

        private void FinalizeGroupBuild()
        {
            // run this loop in increasing order so that the
            // groups get defined going up
            groups = new Group[groupCount];
            for (int i = 0; i < groupCount; ++i)
            {
                distinguishAndPropogateGroup(i, chains[i], group3s[i], seqcodes[i],
                                             firstAtomIndexes[i],
                                             (i == groupCount - 1 ? atomCount : firstAtomIndexes[i + 1]));
                chains[i] = null;
                group3s[i] = null;
            }
        }

        private void distinguishAndPropogateGroup(int groupIndex, Chain chain, string group3,
                                                  int seqcode, int firstAtomIndex, int maxAtomIndex)
        {
            //    System.out.println("distinguish & propogate group:" +
            //                       " group3:" + group3 +
            //                       " seqcode:" + Group.getSeqcodeString(seqcode) +
            //                       " firstAtomIndex:" + firstAtomIndex +
            //                       " maxAtomIndex:" + maxAtomIndex);
            int distinguishingBits = 0;
            // clear previous specialAtomIndexes
            for (int i = JmolConstants.ATOMID_MAX; --i >= 0; )
                specialAtomIndexes[i] = Int32.MinValue;

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

        private void BuildPolymers()
        {
            for (int i = 0; i < groupCount; ++i)
            {
                Group group = groups[i];
                if (group is Monomer)
                {
                    Monomer monomer = (Monomer)group;
                    if (monomer.polymer == null)
                        Polymer.allocatePolymer(groups, i);
                }
            }
        }

        private void Freeze()
        {
            // resize arrays
            if (atomCount < atoms.Length)
            {
                atoms = (Atom[])Util.setLength(atoms, atomCount);
                if (clientAtomReferences != null)
                    clientAtomReferences = (object[])Util.setLength(clientAtomReferences, atomCount);
                if (vibrationVectors != null)
                    vibrationVectors = (Vector3f[])Util.setLength(vibrationVectors, atomCount);
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
                bonds = (Bond[])Util.setLength(bonds, bondCount);

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

            //loadShape(JmolConstants.SHAPE_BALLS);
            //loadShape(JmolConstants.SHAPE_STICKS);
            //loadShape(JmolConstants.SHAPE_HSTICKS);
            //loadShape(JmolConstants.SHAPE_MEASURES);
        }

        private void freeBondsCache()
        {
            for (int i = MAX_BONDS_LENGTH_TO_CACHE; --i > 0; )
            {
                // .GT. 0
                numCached[i] = 0;
                Bond[][] bondsCache = freeBonds[i];
                for (int j = bondsCache.Length; --j >= 0; )
                    bondsCache[j] = null;
            }
        }

        private void hackAtomSerialNumbersForAnimations()
        {
            // first, validate that all atomSerials are NaN
            if (atomSerials != null)
                return;
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

        private void findElementsPresent()
        {
            elementsPresent = new BitArray(64);
            for (int i = atomCount; --i >= 0; )
                SupportClass.BitArraySupport.Set(elementsPresent, atoms[i].elementNumber);
        }

        private void findGroupsPresent()
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

        private void finalizeBuild()
        {
            currentModel = null;
            currentChain = null;
            htAtomMap.Clear();
        }

        ////////////////////////////////////////////////////////////////
        // measurements
        ////////////////////////////////////////////////////////////////

        public float getDistance(int atomIndexA, int atomIndexB)
        {
            return atoms[atomIndexA].point3f.distance(atoms[atomIndexB].point3f);
        }

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
            return angleRadians * 180 / (float)System.Math.PI;
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
            float ai = (float)System.Math.Sqrt(ai2);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            float ci = (float)System.Math.Sqrt(ci2);
            float denom = ai * ci;
            float cross = ax * cx + ay * cy + az * cz;
            float cosang = cross * denom;
            if (cosang > 1)
                cosang = 1;
            if (cosang < -1)
                cosang = -1;

            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            float torsion = toDegrees((float)System.Math.Acos(cosang));
            float dot = ijx * cx + ijy * cy + ijz * cz;
            float absDot = System.Math.Abs(dot);
            torsion = (dot / absDot > 0) ? torsion : -torsion;
            return torsion;
        }



        public void setLabel(string label, int atomIndex) { }

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
                    return;
                }
            }
        }

        public Shape allocateShape(int shapeID)
        {
            string classBase = JmolConstants.shapeClassBases[shapeID];
            //    System.out.println("Frame.allocateShape(" + classBase + ")");
            string className = "Org.Jmol.Viewer." + classBase;

            try
            {
                //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                System.Type shapeClass = System.Type.GetType(className);
                Shape shape = (Shape)System.Activator.CreateInstance(shapeClass);
                shape.setViewerG3dFrame(/*viewer,*/ g3d, this);
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
    }
}