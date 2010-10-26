/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.Interfaces;
using System.IO;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Config;
using Support;
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads a molecule from an Mol2 file, such as written by Sybyl.
    /// See the specs <a href="http://www.tripos.com/data/support/mol2.pdf">here</a>.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      Egon Willighagen
    /// </author>
    /// <cdk.created>     2003-08-21 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>     file format, Mol2 </cdk.keyword>
    public class Mol2Reader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new Mol2Format();
            }
        }

        internal System.IO.StreamReader input = null;
        //private LoggingTool //logger = null;

        /// <summary> Contructs a new MDLReader that can read Molecule from a given Reader.
        /// 
        /// </summary>
        /// <param name="in"> The Reader to read from
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public Mol2Reader(System.IO.StreamReader in_Renamed)
        {
            //logger = new LoggingTool(this);
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            input = new System.IO.StreamReader(in_Renamed.BaseStream, in_Renamed.CurrentEncoding);
        }

        public Mol2Reader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public Mol2Reader()
            : this((StreamReader)null)
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader input)
        {
            if (input is System.IO.StreamReader)
            {
                this.input = (System.IO.StreamReader)input;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
        }

        public override void setReader(System.IO.Stream input)
        {
            setReader(new System.IO.StreamReader(input, System.Text.Encoding.Default));
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemModel).Equals(interfaces[i]))
                    return true;
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Takes an object which subclasses IChemObject, e.g.Molecule, and will read
        /// this from from the Reader. If the specific implementation
        /// does not support a specific IChemObject it will throw an Exception.
        /// 
        /// </summary>
        /// <param name="object">The object that subclasses IChemObject
        /// </param>
        /// <returns>        The IChemObject read
        /// </returns>
        /// <exception cref="CDKException">
        /// </exception>
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                IChemFile file = (IChemFile)object_Renamed;
                IChemSequence sequence = file.Builder.newChemSequence();
                IChemModel model = file.Builder.newChemModel();
                ISetOfMolecules moleculeSet = file.Builder.newSetOfMolecules();
                moleculeSet.addMolecule(readMolecule(model.Builder.newMolecule()));
                model.SetOfMolecules = moleculeSet;
                sequence.addChemModel(model);
                file.addChemSequence(sequence);
                return file;
            }
            else if (object_Renamed is IChemModel)
            {
                IChemModel model = (IChemModel)object_Renamed;
                ISetOfMolecules moleculeSet = model.Builder.newSetOfMolecules();
                moleculeSet.addMolecule(readMolecule(model.Builder.newMolecule()));
                model.SetOfMolecules = moleculeSet;
                return model;
            }
            else
            {
                throw new CDKException("Only supported is ChemModel, and not " + object_Renamed.GetType().FullName + ".");
            }
        }

        public virtual bool accepts(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
            }
            else if (object_Renamed is IChemModel)
            {
                return true;
            }
            return false;
        }


        /// <summary> Read a Reaction from a file in MDL RXN format
        /// 
        /// </summary>
        /// <returns>  The Reaction that was read from the MDL file.
        /// </returns>
        private IMolecule readMolecule(IMolecule molecule)
        {
            AtomTypeFactory atFactory = null;
            try
            {
                atFactory = AtomTypeFactory.getInstance("mol2_atomtypes.xml", molecule.Builder);
            }
            catch (System.Exception exception)
            {
                System.String error = "Could not instantiate an AtomTypeFactory";
                //logger.error(error);
                //logger.debug(exception);
                throw new CDKException(error, exception);
            }
            try
            {
                System.String line = input.ReadLine();
                int atomCount = 0;
                int bondCount = 0;
                while (line != null)
                {
                    if (line.StartsWith("@<TRIPOS>MOLECULE"))
                    {
                        //logger.info("Reading molecule block");
                        // second line has atom/bond counts?
                        input.ReadLine(); // disregard the name line
                        System.String counts = input.ReadLine();
                        SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(counts);
                        try
                        {
                            atomCount = System.Int32.Parse(tokenizer.NextToken());
                        }
                        catch (System.FormatException nfExc)
                        {
                            System.String error = "Error while reading atom count from MOLECULE block";
                            //logger.error(error);
                            //logger.debug(nfExc);
                            throw new CDKException(error, nfExc);
                        }
                        if (tokenizer.HasMoreTokens())
                        {
                            try
                            {
                                bondCount = System.Int32.Parse(tokenizer.NextToken());
                            }
                            catch (System.FormatException nfExc)
                            {
                                System.String error = "Error while reading atom and bond counts";
                                //logger.error(error);
                                //logger.debug(nfExc);
                                throw new CDKException(error, nfExc);
                            }
                        }
                        else
                        {
                            bondCount = 0;
                        }
                        //logger.info("Reading #atoms: ", atomCount);
                        //logger.info("Reading #bonds: ", bondCount);

                        //logger.warn("Not reading molecule qualifiers");
                    }
                    else if (line.StartsWith("@<TRIPOS>ATOM"))
                    {
                        //logger.info("Reading atom block");
                        for (int i = 0; i < atomCount; i++)
                        {
                            line = input.ReadLine().Trim();
                            SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(line);
                            tokenizer.NextToken(); // disregard the id token
                            System.String nameStr = tokenizer.NextToken();
                            System.String xStr = tokenizer.NextToken();
                            System.String yStr = tokenizer.NextToken();
                            System.String zStr = tokenizer.NextToken();
                            System.String atomTypeStr = tokenizer.NextToken();
                            IAtomType atomType = atFactory.getAtomType(atomTypeStr);
                            if (atomType == null)
                            {
                                atomType = atFactory.getAtomType("X");
                                //logger.error("Could not find specified atom type: ", atomTypeStr);
                            }
                            IAtom atom = molecule.Builder.newAtom("X");
                            atom.ID = nameStr;
                            atom.AtomTypeName = atomTypeStr;
                            atFactory.configure(atom);
                            try
                            {
                                double x = System.Double.Parse(xStr);
                                double y = System.Double.Parse(yStr);
                                double z = System.Double.Parse(zStr);
                                atom.setPoint3d(new Point3d(x, y, z));
                            }
                            catch (System.FormatException nfExc)
                            {
                                System.String error = "Error while reading atom coordinates";
                                //logger.error(error);
                                //logger.debug(nfExc);
                                throw new CDKException(error, nfExc);
                            }
                            molecule.addAtom(atom);
                        }
                    }
                    else if (line.StartsWith("@<TRIPOS>BOND"))
                    {
                        //logger.info("Reading bond block");
                        for (int i = 0; i < bondCount; i++)
                        {
                            line = input.ReadLine();
                            SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(line);
                            tokenizer.NextToken(); // disregard the id token
                            System.String atom1Str = tokenizer.NextToken();
                            System.String atom2Str = tokenizer.NextToken();
                            System.String orderStr = tokenizer.NextToken();
                            try
                            {
                                int atom1 = System.Int32.Parse(atom1Str);
                                int atom2 = System.Int32.Parse(atom2Str);
                                double order = 0;
                                if ("1".Equals(orderStr))
                                {
                                    order = CDKConstants.BONDORDER_AROMATIC;
                                }
                                else if ("2".Equals(orderStr))
                                {
                                    order = CDKConstants.BONDORDER_DOUBLE;
                                }
                                else if ("3".Equals(orderStr))
                                {
                                    order = CDKConstants.BONDORDER_TRIPLE;
                                }
                                else if ("am".Equals(orderStr))
                                {
                                    order = CDKConstants.BONDORDER_SINGLE;
                                }
                                else if ("ar".Equals(orderStr))
                                {
                                    order = CDKConstants.BONDORDER_AROMATIC;
                                }
                                else if ("du".Equals(orderStr))
                                {
                                    order = CDKConstants.BONDORDER_SINGLE;
                                }
                                else if ("un".Equals(orderStr))
                                {
                                    order = CDKConstants.BONDORDER_SINGLE;
                                }
                                else if ("nc".Equals(orderStr))
                                {
                                    // not connected
                                    order = 0;
                                }
                                if (order != 0)
                                {
                                    molecule.addBond(atom1 - 1, atom2 - 1, order);
                                }
                            }
                            catch (System.FormatException nfExc)
                            {
                                System.String error = "Error while reading bond information";
                                //logger.error(error);
                                //logger.debug(nfExc);
                                throw new CDKException(error, nfExc);
                            }
                        }
                    }
                    line = input.ReadLine();
                }
            }
            catch (System.IO.IOException exception)
            {
                System.String error = "Error while reading general structure";
                //logger.error(error);
                //logger.debug(exception);
                throw new CDKException(error, exception);
            }
            return molecule;
        }

        public override void close()
        {
            input.Close();
        }
    }
}