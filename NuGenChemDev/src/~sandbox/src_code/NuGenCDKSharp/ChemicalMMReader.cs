/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2002-2003  The Jmol Development Team
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Support;
using Org.OpenScience.CDK.Config;
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads Ghemical (<a href="http://www.uku.fi/~thassine/ghemical/">
    /// http://www.uku.fi/~thassine/ghemical/</a>)
    /// molecular mechanics (*.mm1gp) files.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public class GhemicalMMReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new GhemicalMMFormat();
            }

        }

        //private LoggingTool //logger = null;
        private System.IO.StreamReader input = null;

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public GhemicalMMReader(System.IO.StreamReader input)
        {
            //this.//logger = new LoggingTool(this);
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
        }

        public GhemicalMMReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public GhemicalMMReader()
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

        public override void close()
        {
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemModel).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemModel)
            {
                return (IChemObject)readChemModel((IChemModel)object_Renamed);
            }
            else
            {
                throw new CDKException("Only supported is ChemModel.");
            }
        }

        private IChemModel readChemModel(IChemModel model)
        {
            int[] atoms = new int[1];
            double[] atomxs = new double[1];
            double[] atomys = new double[1];
            double[] atomzs = new double[1];
            double[] atomcharges = new double[1];

            int[] bondatomid1 = new int[1];
            int[] bondatomid2 = new int[1];
            int[] bondorder = new int[1];

            int numberOfAtoms = 0;
            int numberOfBonds = 0;

            try
            {
                System.String line = input.ReadLine();
                while (line != null)
                {
                    SupportClass.Tokenizer st = new SupportClass.Tokenizer(line);
                    System.String command = st.NextToken();
                    if ("!Header".Equals(command))
                    {
                        //logger.warn("Ignoring header");
                    }
                    else if ("!Info".Equals(command))
                    {
                        //logger.warn("Ignoring info");
                    }
                    else if ("!Atoms".Equals(command))
                    {
                        //logger.info("Reading atom block");
                        // determine number of atoms to read
                        try
                        {
                            numberOfAtoms = System.Int32.Parse(st.NextToken());
                            //logger.debug("  #atoms: " + numberOfAtoms);
                            atoms = new int[numberOfAtoms];
                            atomxs = new double[numberOfAtoms];
                            atomys = new double[numberOfAtoms];
                            atomzs = new double[numberOfAtoms];
                            atomcharges = new double[numberOfAtoms];

                            for (int i = 0; i < numberOfAtoms; i++)
                            {
                                line = input.ReadLine();
                                SupportClass.Tokenizer atomInfoFields = new SupportClass.Tokenizer(line);
                                int atomID = System.Int32.Parse(atomInfoFields.NextToken());
                                atoms[atomID] = System.Int32.Parse(atomInfoFields.NextToken());
                                //logger.debug("Set atomic number of atom (" + atomID + ") to: " + atoms[atomID]);
                            }
                        }
                        catch (System.Exception exception)
                        {
                            //logger.error("Error while reading Atoms block");
                            //logger.debug(exception);
                        }
                    }
                    else if ("!Bonds".Equals(command))
                    {
                        //logger.info("Reading bond block");
                        try
                        {
                            // determine number of bonds to read
                            numberOfBonds = System.Int32.Parse(st.NextToken());
                            bondatomid1 = new int[numberOfAtoms];
                            bondatomid2 = new int[numberOfAtoms];
                            bondorder = new int[numberOfAtoms];

                            for (int i = 0; i < numberOfBonds; i++)
                            {
                                line = input.ReadLine();
                                SupportClass.Tokenizer bondInfoFields = new SupportClass.Tokenizer(line);
                                bondatomid1[i] = System.Int32.Parse(bondInfoFields.NextToken());
                                bondatomid2[i] = System.Int32.Parse(bondInfoFields.NextToken());
                                System.String order = bondInfoFields.NextToken();
                                if ("D".Equals(order))
                                {
                                    bondorder[i] = 2;
                                }
                                else if ("S".Equals(order))
                                {
                                    bondorder[i] = 1;
                                }
                                else if ("T".Equals(order))
                                {
                                    bondorder[i] = 3;
                                }
                                else
                                {
                                    // ignore order, i.e. set to single
                                    //logger.warn("Unrecognized bond order, using single bond instead. Found: " + order);
                                    bondorder[i] = 1;
                                }
                            }
                        }
                        catch (System.Exception exception)
                        {
                            //logger.error("Error while reading Bonds block");
                            //logger.debug(exception);
                        }
                    }
                    else if ("!Coord".Equals(command))
                    {
                        //logger.info("Reading coordinate block");
                        try
                        {
                            for (int i = 0; i < numberOfAtoms; i++)
                            {
                                line = input.ReadLine();
                                SupportClass.Tokenizer atomInfoFields = new SupportClass.Tokenizer(line);
                                int atomID = System.Int32.Parse(atomInfoFields.NextToken());
                                double x = System.Double.Parse(atomInfoFields.NextToken());
                                double y = System.Double.Parse(atomInfoFields.NextToken());
                                double z = System.Double.Parse(atomInfoFields.NextToken());
                                atomxs[atomID] = x;
                                atomys[atomID] = y;
                                atomzs[atomID] = z;
                            }
                        }
                        catch (System.Exception exception)
                        {
                            //logger.error("Error while reading Coord block");
                            //logger.debug(exception);
                        }
                    }
                    else if ("!Charges".Equals(command))
                    {
                        //logger.info("Reading charges block");
                        try
                        {
                            for (int i = 0; i < numberOfAtoms; i++)
                            {
                                line = input.ReadLine();
                                SupportClass.Tokenizer atomInfoFields = new SupportClass.Tokenizer(line);
                                int atomID = System.Int32.Parse(atomInfoFields.NextToken());
                                double charge = System.Double.Parse(atomInfoFields.NextToken());
                                atomcharges[atomID] = charge;
                            }
                        }
                        catch (System.Exception exception)
                        {
                            //logger.error("Error while reading Charges block");
                            //logger.debug(exception);
                        }
                    }
                    else if ("!End".Equals(command))
                    {
                        //logger.info("Found end of file");
                        // Store atoms
                        IAtomContainer container = model.Builder.newAtomContainer();
                        for (int i = 0; i < numberOfAtoms; i++)
                        {
                            try
                            {
                                IAtom atom = model.Builder.newAtom(IsotopeFactory.getInstance(container.Builder).getElementSymbol(atoms[i]));
                                atom.AtomicNumber = atoms[i];
                                atom.setPoint3d(new Point3d(atomxs[i], atomys[i], atomzs[i]));
                                atom.setCharge(atomcharges[i]);
                                container.addAtom(atom);
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                //logger.debug("Stored atom: " + atom);
                            }
                            catch (System.Exception exception)
                            {
                                //logger.error("Cannot create an atom with atomic number: " + atoms[i]);
                                //logger.debug(exception);
                            }
                        }

                        // Store bonds
                        for (int i = 0; i < numberOfBonds; i++)
                        {
                            container.addBond(bondatomid1[i], bondatomid2[i], bondorder[i]);
                        }

                        ISetOfMolecules moleculeSet = model.Builder.newSetOfMolecules();
                        moleculeSet.addMolecule(model.Builder.newMolecule(container));
                        model.SetOfMolecules = moleculeSet;

                        return model;
                    }
                    else
                    {
                        //logger.warn("Skipping line: " + line);
                    }

                    line = input.ReadLine();
                }
            }
            catch (System.Exception exception)
            {
                //logger.error("Error while reading file");
                //logger.debug(exception);
            }

            // this should not happen, file is lacking !End command
            return null;
        }
    }
}