/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 09:45:29 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6663 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*  */
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using Org.OpenScience.CDK.Config;
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Protein.Data;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Templates;
using Org.OpenScience.CDK.Graph.Rebond;
using Org.OpenScience.CDK.Tools.Manipulator;
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads the contents of a PDBFile.
    /// 
    /// <p>A description can be found at <a href="http://www.rcsb.org/pdb/static.do?p=file_formats/pdb/index.html">
    /// http://www.rcsb.org/pdb/static.do?p=file_formats/pdb/index.html</a>.
    /// 
    /// </summary>
    /// <cdk.module>   pdb </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Edgar Luttmann
    /// </author>
    /// <author>       Bradley Smith <bradley@baysmith.com>
    /// </author>
    /// <author>       Martin Eklund <martin.eklund@farmbio.uu.se>
    /// </author>
    /// <cdk.created>  2001-08-06 </cdk.created>
    /// <cdk.keyword>  file format, PDB </cdk.keyword>
    /// <cdk.bug>      1487368 </cdk.bug>
    public class PDBReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new PDBFormat();
            }

        }
        override public IOSetting[] IOSettings
        {
            get
            {
                IOSetting[] settings = new IOSetting[3];
                settings[0] = deduceBonding;
                settings[1] = useRebondTool;
                settings[2] = readConnect;
                return settings;
            }
        }

        //private LoggingTool //logger;
        private System.IO.StreamReader _oInput; // The internal used BufferedReader
        private BooleanIOSetting deduceBonding;
        private BooleanIOSetting useRebondTool;
        private BooleanIOSetting readConnect;

        private System.Collections.IDictionary atomNumberMap;

        private static AtomTypeFactory pdbFactory;

        /// <summary> 
        /// Contructs a new PDBReader that can read Molecules from a given
        /// InputStream.
        /// 
        /// </summary>
        /// <param name="oIn"> The InputStream to read from
        /// 
        /// </param>
        public PDBReader(System.IO.Stream oIn)
            : this(new System.IO.StreamReader(oIn, System.Text.Encoding.Default))
        {
        }

        /// <summary> 
        /// Contructs a new PDBReader that can read Molecules from a given
        /// Reader.
        /// 
        /// </summary>
        /// <param name="oIn"> The Reader to read from
        /// 
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public PDBReader(System.IO.StreamReader oIn)
        {
            //logger = new LoggingTool(this.GetType());
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            _oInput = oIn;
            initIOSettings();
            pdbFactory = null;
        }

        public PDBReader()
            : this((StreamReader)null)
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader input)
        {
            if (input is System.IO.StreamReader)
            {
                this._oInput = (System.IO.StreamReader)input;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this._oInput = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
        }

        public override void setReader(System.IO.Stream input)
        {
            setReader(new System.IO.StreamReader(input, System.Text.Encoding.Default));
        }

        public override bool accepts(System.Type classObject)
        {
            if (typeof(IChemFile).Equals(classObject))
                return true;

            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> 
        /// Takes an object which subclasses IChemObject, e.g. Molecule, and will
        /// read this (from file, database, internet etc). If the specific
        /// implementation does not support a specific IChemObject it will throw
        /// an Exception.
        /// 
        /// </summary>
        /// <param name="oObj"> The object that subclasses IChemObject
        /// </param>
        /// <returns>      The IChemObject read  
        /// </returns>
        /// <exception cref="CDKException"> 
        /// 
        /// </exception>
        public override IChemObject read(IChemObject oObj)
        {
            if (oObj is IChemFile)
            {
                if (pdbFactory == null)
                {
                    try
                    {
                        pdbFactory = AtomTypeFactory.getInstance("pdb_atomtypes.xml", ((IChemFile)oObj).Builder);
                    }
                    catch (System.Exception exception)
                    {
                        //logger.debug(exception);
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        throw new CDKException("Could not setup list of PDB atom types! " + exception.Message);
                    }
                }
                return readChemFile((IChemFile)oObj);
            }
            else
            {
                throw new CDKException("Only supported is reading of ChemFile objects.");
            }
        }

        /// <summary> Read a <code>ChemFile</code> from a file in PDB format. The molecules
        /// in the file are stored as <code>BioPolymer</code>s in the
        /// <code>ChemFile</code>. The residues are the monomers of the
        /// <code>BioPolymer</code>, and their names are the concatenation of the
        /// residue, chain id, and the sequence number. Separate chains (denoted by
        /// TER records) are stored as separate <code>BioPolymer</code> molecules.
        /// 
        /// <p>Connectivity information is not currently read.
        /// 
        /// </summary>
        /// <returns> The ChemFile that was read from the PDB file.
        /// </returns>
        private IChemFile readChemFile(IChemFile oFile)
        {
            int bonds = 0;
            // initialize all containers
            IChemSequence oSeq = oFile.Builder.newChemSequence();
            IChemModel oModel = oFile.Builder.newChemModel();
            ISetOfMolecules oSet = oFile.Builder.newSetOfMolecules();

            // some variables needed
            string cCol;
            PDBAtom oAtom;
            PDBPolymer oBP = new PDBPolymer();
            System.Text.StringBuilder cResidue;
            string oObj;
            IMonomer oMonomer;
            string cRead = "";
            char chain = 'A'; // To ensure stringent name giving of monomers
            IStrand oStrand;
            int lineLength = 0;

            atomNumberMap = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

            // do the reading of the Input		
            try
            {
                do
                {
                    cRead = _oInput.ReadLine();
                    //logger.debug("Read line: ", cRead);
                    if (cRead != null)
                    {
                        lineLength = cRead.Length;

                        if (lineLength < 80)
                        {
                            //logger.warn("Line is not of the expected length 80!");
                        }

                        // make sure the record name is 6 characters long
                        if (lineLength < 6)
                        {
                            cRead = cRead + "      ";
                        }
                        // check the first column to decide what to do
                        cCol = cRead.Substring(0, (6) - (0));
                        if ("ATOM  ".ToUpper().Equals(cCol.ToUpper()))
                        {
                            // read an atom record
                            oAtom = readAtom(cRead, lineLength);

                            // construct a string describing the residue
                            cResidue = new System.Text.StringBuilder(8);
                            oObj = oAtom.ResName;
                            if (oObj != null)
                            {
                                cResidue = cResidue.Append(oObj.Trim());
                            }
                            oObj = oAtom.ChainID;
                            if (oObj != null)
                            {
                                // cResidue = cResidue.append(((String)oObj).trim());
                                cResidue = cResidue.Append(System.Convert.ToString(chain));
                            }
                            oObj = oAtom.ResSeq;
                            if (oObj != null)
                            {
                                cResidue = cResidue.Append(oObj.Trim());
                            }

                            // search for an existing strand or create a new one.
                            oStrand = oBP.getStrand(System.Convert.ToString(chain));
                            if (oStrand == null)
                            {
                                oStrand = new PDBStrand();
                                oStrand.StrandName = System.Convert.ToString(chain);
                            }

                            // search for an existing monomer or create a new one.
                            oMonomer = oBP.getMonomer(cResidue.ToString(), System.Convert.ToString(chain));
                            if (oMonomer == null)
                            {
                                PDBMonomer monomer = new PDBMonomer();
                                monomer.MonomerName = cResidue.ToString();
                                monomer.MonomerType = oAtom.ResName;
                                monomer.ChainID = oAtom.ChainID;
                                monomer.ICode = oAtom.ICode;
                                oMonomer = monomer;
                            }

                            // add the atom
                            oBP.addAtom(oAtom, oMonomer, oStrand);
                            System.Object tempObject;
                            //UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
                            tempObject = atomNumberMap[(System.Int32)oAtom.Serial];
                            atomNumberMap[(System.Int32)oAtom.Serial] = oAtom;
                            if (readConnect.Set && tempObject != null)
                            {
                                //logger.warn("Duplicate serial ID found for atom: ", oAtom);
                            }
                            //						//logger.debug("Added ATOM: ", oAtom);

                            /** As HETATMs cannot be considered to either belong to a certain monomer or strand,
                            * they are dealt with seperately.*/
                        }
                        else if ("HETATM".ToUpper().Equals(cCol.ToUpper()))
                        {
                            // read an atom record
                            oAtom = readAtom(cRead, lineLength);
                            oAtom.HetAtom = true;
                            oBP.addAtom(oAtom);
                            System.Object tempObject2;
                            //UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
                            tempObject2 = atomNumberMap[(System.Int32)oAtom.Serial];
                            atomNumberMap[(System.Int32)oAtom.Serial] = oAtom;
                            if (tempObject2 != null)
                            {
                                //logger.warn("Duplicate serial ID found for atom: ", oAtom);
                            }
                            //logger.debug("Added HETATM: ", oAtom);
                        }
                        else if ("TER   ".ToUpper().Equals(cCol.ToUpper()))
                        {
                            // start new strand						
                            chain++;
                            oStrand = new PDBStrand();
                            oStrand.StrandName = System.Convert.ToString(chain);
                            //logger.debug("Added new STRAND");
                        }
                        else if ("END   ".ToUpper().Equals(cCol.ToUpper()))
                        {
                            atomNumberMap.Clear();
                            // create bonds and finish the molecule
                            if (deduceBonding.Set)
                            {
                                // OK, try to deduce the bonding patterns
                                if (oBP.AtomCount != 0)
                                {
                                    // Create bonds. If bonds could not be created, all bonds are deleted.
                                    try
                                    {
                                        if (useRebondTool.Set)
                                        {
                                            if (!createBondsWithRebondTool(oBP))
                                            {
                                                // Get rid of all potentially created bonds.
                                                //logger.info("Bonds could not be created using the RebondTool when PDB file was read.");
                                                oBP.removeAllBonds();
                                            }
                                        }
                                        else
                                        {
                                            if (!createBonds(oBP))
                                            {
                                                // Get rid of all potentially created bonds.
                                                //logger.info("Bonds could not be created when PDB file was read.");
                                                oBP.removeAllBonds();
                                            }
                                        }
                                    }
                                    catch (System.Exception exception)
                                    {
                                        //logger.info("Bonds could not be created when PDB file was read.");
                                        //logger.debug(exception);
                                    }
                                }
                            }
                            oSet.addMolecule(oBP);
                            //						oBP = new BioPolymer();					
                            //				} else if (cCol.equals("USER  ")) {
                            //						System.out.println(cLine);
                            //					System.out.println(cLine);
                            //				} else if (cCol.equals("ENDMDL")) {
                            //					System.out.println(cLine);
                        }
                        else if (cCol.Equals("MODEL "))
                        {
                            // OK, start a new model and save the current one first *if* it contains atoms
                            if (oBP.AtomCount > 0)
                            {
                                // save the model
                                oSet.addAtomContainer(oBP);
                                oModel.SetOfMolecules = oSet;
                                oSeq.addChemModel(oModel);
                                // setup a new one
                                oBP = new PDBPolymer();
                                oModel = oFile.Builder.newChemModel();
                                oSet = oFile.Builder.newSetOfMolecules();
                            }
                        }
                        else if ("REMARK".ToUpper().Equals(cCol.ToUpper()))
                        {
                            System.Object comment = oFile.getProperty(CDKConstants.COMMENT);
                            if (comment == null)
                            {
                                comment = "";
                            }
                            if (lineLength > 12)
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                comment = comment.ToString() + cRead.Substring(11).Trim() + "\n";
                                oFile.setProperty(CDKConstants.COMMENT, comment);
                            }
                            else
                            {
                                //logger.warn("REMARK line found without any comment!");
                            }
                        }
                        else if ("COMPND".ToUpper().Equals(cCol.ToUpper()))
                        {
                            string title = cRead.Substring(10).Trim();
                            oFile.setProperty(CDKConstants.TITLE, title);
                        }
                        /*************************************************************
                        * Read connectivity information from CONECT records.
                        * Only covalent bonds are dealt with. Perhaps salt bridges
                        * should be dealt with in the same way..?
                        */
                        else if (readConnect.Set && "CONECT".ToUpper().Equals(cCol.ToUpper()))
                        {
                            cRead.Trim();
                            if (cRead.Length < 16)
                            {
                                //logger.debug("Skipping unexpected empty CONECT line! : ", cRead);
                            }
                            else
                            {
                                string bondAtom = cRead.Substring(7, 5).Trim();
                                int bondAtomNo = System.Int32.Parse(bondAtom);

                                for (int b = 0; b < 9; b += (b == 5 ? 2 : 1))
                                {
                                    string bondedAtom = cRead.Substring((b * 5) + 11, 5).Trim();
                                    int bondedAtomNo;
                                    if (int.TryParse(bondedAtom, out bondedAtomNo))
                                    {
                                        bonds++;
                                        addBond(oBP, bondAtomNo, bondedAtomNo);
                                    }
                                }

                                //string bondedAtom = cRead.Substring(12, 5).Trim();
                                //int bondedAtomNo = -1;

                                //try
                                //{
                                //    bondedAtomNo = System.Int32.Parse(bondedAtom);
                                //}
                                //catch (System.Exception e)
                                //{
                                //    bondedAtomNo = -1;
                                //}

                                //if (bondedAtomNo != -1)
                                //{
                                //    bonds++;
                                //    addBond(oBP, bondAtomNo, bondedAtomNo);
                                //    //logger.warn("Bonded " + bondAtomNo + " with " + bondedAtomNo);
                                //}
                                //else
                                //{
                                //}

                                //if (cRead.Length > 17)
                                //{
                                //    bondedAtom = cRead.Substring(16, 5);
                                //    bondedAtom = bondedAtom.Trim();
                                //    try
                                //    {
                                //        bondedAtomNo = System.Int32.Parse(bondedAtom);
                                //    }
                                //    catch (System.Exception e)
                                //    {
                                //        bondedAtomNo = -1;
                                //    }

                                //    if (bondedAtomNo != -1)
                                //    {
                                //        bonds++;
                                //        addBond(oBP, bondAtomNo, bondedAtomNo);
                                //        //logger.warn("Bonded " + bondAtomNo + " with " + bondedAtomNo);
                                //    }
                                //}

                                //if (cRead.Length > 22)
                                //{
                                //    bondedAtom = cRead.Substring(22, 5);
                                //    bondedAtom = bondedAtom.Trim();
                                //    try
                                //    {
                                //        bondedAtomNo = System.Int32.Parse(bondedAtom);
                                //    }
                                //    catch (System.Exception e)
                                //    {
                                //        bondedAtomNo = -1;
                                //    }

                                //    if (bondedAtomNo != -1)
                                //    {
                                //        bonds++;
                                //        addBond(oBP, bondAtomNo, bondedAtomNo);
                                //        //logger.warn("Bonded " + bondAtomNo + " with " + bondedAtomNo);
                                //    }
                                //}

                                //if (cRead.Length > 27)
                                //{
                                //    bondedAtom = cRead.Substring(27, 5);
                                //    bondedAtom = bondedAtom.Trim();
                                //    try
                                //    {
                                //        bondedAtomNo = System.Int32.Parse(bondedAtom);
                                //    }
                                //    catch (System.Exception e)
                                //    {
                                //        bondedAtomNo = -1;
                                //    }

                                //    if (bondedAtomNo != -1)
                                //    {
                                //        bonds++;
                                //        addBond(oBP, bondAtomNo, bondedAtomNo);
                                //        //logger.warn("Bonded " + bondAtomNo + " with " + bondedAtomNo);
                                //    }
                                //}
                            }
                        }
                        /*************************************************************/
                        else if ("HELIX ".ToUpper().Equals(cCol.ToUpper()))
                        {
                            //						HELIX    1 H1A CYS A   11  LYS A   18  1 RESIDUE 18 HAS POSITIVE PHI    1D66  72
                            //						          1         2         3         4         5         6         7
                            //						01234567890123456789012345678901234567890123456789012345678901234567890123456789
                            PDBStructure structure = new PDBStructure();
                            structure.StructureType = PDBStructure.HELIX;
                            structure.StartChainID = cRead[19];
                            structure.StartSequenceNumber = System.Int32.Parse(cRead.Substring(21, (25) - (21)).Trim());
                            structure.StartInsertionCode = cRead[25];
                            structure.EndChainID = cRead[31];
                            structure.EndSequenceNumber = System.Int32.Parse(cRead.Substring(33, (37) - (33)).Trim());
                            structure.EndInsertionCode = cRead[37];
                            oBP.addStructure(structure);
                        }
                        else if ("SHEET ".ToUpper().Equals(cCol.ToUpper()))
                        {
                            PDBStructure structure = new PDBStructure();
                            structure.StructureType = PDBStructure.SHEET;
                            structure.StartChainID = cRead[21];
                            structure.StartSequenceNumber = System.Int32.Parse(cRead.Substring(22, (26) - (22)).Trim());
                            structure.StartInsertionCode = cRead[26];
                            structure.EndChainID = cRead[32];
                            structure.EndSequenceNumber = System.Int32.Parse(cRead.Substring(33, (37) - (33)).Trim());
                            structure.EndInsertionCode = cRead[37];
                            oBP.addStructure(structure);
                        }
                        else if ("TURN  ".ToUpper().Equals(cCol.ToUpper()))
                        {
                            PDBStructure structure = new PDBStructure();
                            structure.StructureType = PDBStructure.TURN;
                            structure.StartChainID = cRead[19];
                            structure.StartSequenceNumber = System.Int32.Parse(cRead.Substring(20, (24) - (20)).Trim());
                            structure.StartInsertionCode = cRead[24];
                            structure.EndChainID = cRead[30];
                            structure.EndSequenceNumber = System.Int32.Parse(cRead.Substring(31, (35) - (31)).Trim());
                            structure.EndInsertionCode = cRead[35];
                            oBP.addStructure(structure);
                        } // ignore all other commands
                    }
                }
                while (_oInput.Peek() != -1 && (cRead != null));
            }
            catch (System.Exception e)
            {
                //logger.error("Found a problem at line:\n");
                //logger.error(cRead);
                //logger.error("01234567890123456789012345678901234567890123456789012345678901234567890123456789");
                //logger.error("          1         2         3         4         5         6         7         ");
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("  error: " + e.Message);
                //logger.debug(e);
            }

            // try to close the Input
            try
            {
                _oInput.Close();
            }
            catch (System.Exception e)
            {
                //logger.debug(e);
            }

            // Set all the dependencies
            oModel.SetOfMolecules = oSet;
            oSeq.addChemModel(oModel);
            oFile.addChemSequence(oSeq);

            return oFile;
        }

        private void addBond(PDBPolymer obp, int bondAtomNo, int bondedAtomNo)
        {
            IAtom firstAtom = (PDBAtom)atomNumberMap[(System.Int32)bondAtomNo];
            IAtom secondAtom = (PDBAtom)atomNumberMap[(System.Int32)bondedAtomNo];
            if (firstAtom == null)
            {
                //logger.error("Could not find bond start atom in map with serial id: ", bondAtomNo);
            }
            if (secondAtom == null)
            {
                //logger.error("Could not find bond target atom in map with serial id: ", bondAtomNo);
            }
            obp.addBond(firstAtom.Builder.newBond(firstAtom, secondAtom, 1));
        }

        /// <summary> Create bonds when reading a protein PDB file. NB ONLY works for protein
        /// PDB files! If you want to read small molecules I recommend using molecule
        /// file format where the connectivity is explicitly stated. [This method can
        /// however reasonably easily be extended to cover e.g. nucleic acids or your
        /// favourite small molecule]. Returns 'false' if bonds could not be created
        /// due to incomplete pdb-records or other reasons. 
        /// 
        /// </summary>
        /// <param name="pol">The Biopolymer to work on
        /// </param>
        public virtual bool createBonds(IBioPolymer pol)
        {
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            System.Collections.Hashtable AAs = AminoAcids.HashMapByThreeLetterCode;
            int[][] AABondInfo = AminoAcids.aaBondInfo();
            System.Collections.Hashtable strands = pol.Strands;
            System.Collections.IEnumerator strandKeys = strands.Keys.GetEnumerator();

            //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
            while (strandKeys.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                IStrand strand = (IStrand)strands[strandKeys.Current];
                int atoms = 0;
                int atomsInLastResidue = 0;
                int atomsInPresentResidue = 0;

                while (atoms < strand.AtomCount - 1)
                {
                    PDBAtom anAtom = (PDBAtom)strand.getAtomAt(atoms);

                    // Check that we have bond info about residue/ligand, if not - exit.
                    if (!AAs.ContainsKey(anAtom.ResName))
                    {
                        return false;
                    }
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    IMonomer monomer = (IMonomer)AAs[anAtom.ResName];

                    atomsInPresentResidue = System.Int32.Parse((string)monomer.getProperty(AminoAcids.NO_ATOMS));

                    /* Check if there's something wrong with the residue record (e.g. it doesn't contain the
                    * correct number of atom records). */
                    int counter = 1;
                    while (atoms + counter < strand.AtomCount && anAtom.ResName.Equals(strand.getAtomAt(atoms + counter).getProperty(AminoAcids.RESIDUE_NAME)))
                    {
                        counter++;
                    }
                    // Check if something is wrong. Remember to deal with possible OXT atom...
                    if (counter % atomsInPresentResidue != 0 && (atoms + counter == strand.AtomCount && counter % atomsInPresentResidue != 1))
                    {
                        return false;
                    }

                    // If nothing's wrong, add bonds
                    int bondID = System.Int32.Parse((string)monomer.getProperty(AminoAcids.ID));
                    for (int l = 0; l < System.Int32.Parse((string)monomer.getProperty(AminoAcids.NO_BONDS)); l++)
                    {
                        IBond bond = pol.Builder.newBond(strand.getAtomAt(AABondInfo[bondID + l][1] + atoms), strand.getAtomAt(AABondInfo[bondID + l][2] + atoms), (double)(AABondInfo[bondID + l][3]));
                        pol.addBond(bond);
                    }

                    // If not first residue, connect residues
                    if (atomsInLastResidue != 0)
                    {
                        IBond bond = pol.Builder.newBond(strand.getAtomAt(atoms - atomsInLastResidue + 2), strand.getAtomAt(atoms), 1);
                        pol.addBond(bond);
                    }

                    atoms = atoms + atomsInPresentResidue;
                    atomsInLastResidue = atomsInPresentResidue;

                    // Check if next atom is an OXT. The reason to why this is seemingly overly complex is because
                    // not all PDB-files have ending OXT. If that were the case you could just check if
                    // atoms == mol.getAtomCount()...
                    if (strand.AtomCount < atoms && ((PDBAtom)strand.getAtomAt(atoms)).Oxt)
                    {
                        //				if(strand.getAtomCount() < atoms && ((String)strand.getAtomAt(atoms).getProperty("oxt")).equals("1"))	{
                        IBond bond = pol.Builder.newBond(strand.getAtomAt(atoms - atomsInLastResidue + 2), strand.getAtomAt(atoms), 1);
                        pol.addBond(bond);
                    }
                }
            }
            return true;
        }

        private bool createBondsWithRebondTool(IBioPolymer pol)
        {
            RebondTool tool = new RebondTool(2.0, 0.5, 0.5);
            try
            {
                //			 configure atoms
                AtomTypeFactory factory = AtomTypeFactory.getInstance("jmol_atomtypes.txt", pol.Builder);
                IAtom[] atoms = pol.Atoms;
                for (int i = 0; i < atoms.Length; i++)
                {
                    try
                    {
                        IAtomType[] types = factory.getAtomTypes(atoms[i].Symbol);
                        if (types.Length > 0)
                        {
                            // just pick the first one
                            AtomTypeManipulator.configure(atoms[i], types[0]);
                        }
                        else
                        {
                            System.Console.Out.WriteLine("Could not configure atom with symbol: " + atoms[i].Symbol);
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        System.Console.Out.WriteLine("Could not configure atom (but don't care): " + e.Message);
                        //logger.debug(e);
                    }
                }
                tool.rebond(pol);
            }
            catch (System.Exception e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Could not rebond the polymer: " + e.Message);
                //logger.debug(e);
            }
            return true;
        }

        /// <summary> Creates an <code>Atom</code> and sets properties to their values from
        /// the ATOM record. If the line is shorter than 80 characters, the information
        /// past 59 characters is treated as optional. If the line is shorter than 59
        /// characters, a <code>RuntimeException</code> is thrown.
        /// 
        /// </summary>
        /// <param name="cLine"> the PDB ATOM record.
        /// </param>
        /// <returns> the <code>Atom</code> created from the record.
        /// </returns>
        /// <throws>  RuntimeException if the line is too short (less than 59 characters). </throws>
        private PDBAtom readAtom(string cLine, int lineLength)
        {
            // a line looks like:
            // 0         1         2         3         4
            // 01234567890123456789012345678901234567890123456789012345678901234567890123456789
            // ATOM      1  O5*   C A   1      20.662  36.632  23.475  1.00 10.00      114D  45
            // ATOM   1186 1H   ALA     1      10.105   5.945  -6.630  1.00  0.00      1ALE1288

            if (lineLength < 59)
            {
                throw new System.SystemException("PDBReader error during readAtom(): line too short");
            }
            string elementSymbol = cLine.Substring(12, 4).Trim();

            if (elementSymbol.Length == 2)
            {
                // ensure that the second char is lower case
                elementSymbol = elementSymbol[0] + elementSymbol.Substring(1).ToLower();
            }
            string rawAtomName = cLine.Substring(12, 4).Trim();
            string resName = cLine.Substring(17, 3).Trim();
            try
            {
                IAtomType type = pdbFactory.getAtomType(resName + "." + rawAtomName);
                elementSymbol = type.Symbol;
            }
            catch (NoSuchAtomTypeException e)
            {
                // try 1 char
                try
                {
                    elementSymbol = rawAtomName.Substring(0, 1);//pdbFactory.getAtomType(rawAtomName.Substring(0, 1)).Symbol;
                }
                catch
                {
                    elementSymbol = "Xx";
                }
                //System.Console.Out.WriteLine("Did not recognize PDB atom type: " + resName + "." + rawAtomName);
            }
            PDBAtom oAtom = new PDBAtom(elementSymbol, new Point3d(System.Double.Parse(cLine.Substring(30, (38) - (30))), System.Double.Parse(cLine.Substring(38, (46) - (38))), System.Double.Parse(cLine.Substring(46, (54) - (46)))));

            oAtom.Record = cLine;
            oAtom.Serial = System.Int32.Parse(cLine.Substring(6, 5).Trim());
            oAtom.Name = rawAtomName.Trim();
            oAtom.AltLoc = cLine.Substring(16, 1).Trim();
            oAtom.ResName = resName;
            oAtom.ChainID = cLine.Substring(21, (22) - (21)).Trim();
            oAtom.ResSeq = cLine.Substring(22, (26) - (22)).Trim();
            oAtom.ICode = cLine.Substring(26, (27) - (26)).Trim();
            oAtom.AtomTypeName = oAtom.ResName + "." + rawAtomName;
            if (lineLength >= 59)
            {
                string frag = cLine.Substring(54, (60) - (54)).Trim();
                if (frag.Length > 0)
                {
                    oAtom.Occupancy = System.Double.Parse(frag);
                }
            }
            if (lineLength >= 65)
            {
                string frag = cLine.Substring(60, (66) - (60)).Trim();
                if (frag.Length > 0)
                {
                    oAtom.TempFactor = System.Double.Parse(frag);
                }
            }
            if (lineLength >= 75)
            {
                oAtom.SegID = cLine.Substring(72, (76) - (72)).Trim();
            }
            //		if (lineLength >= 78) {
            //            oAtom.setSymbol((new String(cLine.substring(76, 78))).trim());
            //		}
            if (lineLength >= 79)
            {
                string frag = cLine.Substring(78, (80) - (78)).Trim();
                if (frag.Length > 0)
                {
                    oAtom.setCharge(System.Double.Parse(frag));
                }
            }

            /*************************************************************************************
            * It sets a flag in the property content of an atom,
            * which is used when bonds are created to check if the atom is an OXT-record => needs
            * special treatment.
            */
            string oxt = cLine.Substring(13, (16) - (13)).Trim();

            if (oxt.Equals("OXT"))
            {
                oAtom.Oxt = true;
            }
            else
            {
                oAtom.Oxt = false;
            }
            /*************************************************************************************/

            return oAtom;
        }

        public override void close()
        {
            _oInput.Close();
        }

        private void initIOSettings()
        {
            deduceBonding = new BooleanIOSetting("DeduceBonding", IOSetting.LOW, "Should the PDBReader deduce bonding patterns?", "true");
            useRebondTool = new BooleanIOSetting("UseRebondTool", IOSetting.LOW, "Should the RebondTool be used (or a heuristic approach otherwise)?", "true");
            readConnect = new BooleanIOSetting("ReadConnectSection", IOSetting.LOW, "Should the CONECT be read?", "true");
        }

        public virtual void customizeJob()
        {
            fireIOSettingQuestion(deduceBonding);
            fireIOSettingQuestion(useRebondTool);
            fireIOSettingQuestion(readConnect);
        }
    }
}