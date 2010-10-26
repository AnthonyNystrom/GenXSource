/*  $RCSfile$
*  $Author: kaihartmann $
*  $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
*  $Revision: 6349 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using Org.OpenScience.CDK.Interfaces;
using System.IO;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Config;
using javax.vecmath;
using Support;
using Org.OpenScience.CDK.Tools.Manipulator;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads a molecule from an MDL MOL or SDF file {@cdk.cite DAL92}. An SD files
    /// is read into a ChemSequence of ChemModel's. Each ChemModel will contain one
    /// Molecule.
    /// 
    /// <p>From the Atom block it reads atomic coordinates, element types and
    /// formal charges. From the Bond block it reads the bonds and the orders.
    /// Additionally, it reads 'M  CHG', 'G  ', 'M  RAD' and 'M  ISO' lines from the
    /// property block.
    /// 
    /// <p>If all z coordinates are 0.0, then the xy coordinates are taken as
    /// 2D, otherwise the coordinates are read as 3D.
    /// 
    /// <p>The title of the MOL file is read and can be retrieved with:
    /// <pre>
    /// molecule.getProperty(CDKConstants.TITLE);
    /// </pre>
    /// 
    /// RGroups which are saved in the mdl file as R#, are renamed according to their appearance,
    /// e.g. the first R# is named R1. With PseudAtom.getLabel() "R1" is returned (instead of R#).
    /// This is introduced due to the SAR table generation procedure of Scitegics PipelinePilot.  
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <author>      Egon Willighagen
    /// </author>
    /// <cdk.created>     2000-10-02 </cdk.created>
    /// <cdk.keyword>     file format, MDL molfile </cdk.keyword>
    /// <cdk.keyword>     file format, SDF </cdk.keyword>
    public class MDLReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MDLFormat();
            }

        }
        override public IOSetting[] IOSettings
        {
            get
            {
                IOSetting[] settings = new IOSetting[1];
                settings[0] = forceReadAs3DCoords;
                return settings;
            }

        }

        internal System.IO.StreamReader input = null;
        //private LoggingTool //logger = null;

        private BooleanIOSetting forceReadAs3DCoords;

        public MDLReader()
            : this((StreamReader)null)
        {
        }

        /// <summary>  Contructs a new MDLReader that can read Molecule from a given InputStream.
        /// 
        /// </summary>
        /// <param name="in"> The InputStream to read from
        /// </param>
        public MDLReader(System.IO.Stream in_Renamed)
            : this(new System.IO.StreamReader(in_Renamed, System.Text.Encoding.Default))
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

        /// <summary>  Contructs a new MDLReader that can read Molecule from a given Reader.
        /// 
        /// </summary>
        /// <param name="in"> The Reader to read from
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MDLReader(System.IO.StreamReader in_Renamed)
        {
            //logger = new LoggingTool(this);
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            input = new System.IO.StreamReader(in_Renamed.BaseStream, in_Renamed.CurrentEncoding);
            initIOSettings();
        }


        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
                if (typeof(IChemModel).Equals(interfaces[i]))
                    return true;
                if (typeof(IMolecule).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary>  Takes an object which subclasses IChemObject, e.g. Molecule, and will read
        /// this (from file, database, internet etc). If the specific implementation
        /// does not support a specific IChemObject it will throw an Exception.
        /// 
        /// </summary>
        /// <param name="object">                             The object that subclasses
        /// IChemObject
        /// </param>
        /// <returns>                                     The IChemObject read
        /// </returns>
        /// <exception cref="CDKException">
        /// </exception>
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                return readChemFile((IChemFile)object_Renamed);
            }
            else if (object_Renamed is IChemModel)
            {
                return readChemModel((IChemModel)object_Renamed);
            }
            else if (object_Renamed is IMolecule)
            {
                return readMolecule((IMolecule)object_Renamed);
            }
            else
            {
                throw new CDKException("Only supported are ChemFile and Molecule.");
            }
        }

        private IChemModel readChemModel(IChemModel chemModel)
        {
            ISetOfMolecules setOfMolecules = chemModel.SetOfMolecules;
            if (setOfMolecules == null)
            {
                setOfMolecules = chemModel.Builder.newSetOfMolecules();
            }
            IMolecule m = readMolecule(chemModel.Builder.newMolecule());
            if (m != null)
            {
                setOfMolecules.addMolecule(m);
            }
            chemModel.SetOfMolecules = setOfMolecules;
            return chemModel;
        }

        /// <summary> Read a ChemFile from a file in MDL SDF format.
        /// 
        /// </summary>
        /// <returns>    The ChemFile that was read from the MDL file.
        /// </returns>
        private IChemFile readChemFile(IChemFile chemFile)
        {
            IChemSequence chemSequence = chemFile.Builder.newChemSequence();

            IChemModel chemModel = chemFile.Builder.newChemModel();
            ISetOfMolecules setOfMolecules = chemFile.Builder.newSetOfMolecules();
            IMolecule m = readMolecule(chemFile.Builder.newMolecule());
            if (m != null)
            {
                setOfMolecules.addMolecule(m);
            }
            chemModel.SetOfMolecules = setOfMolecules;
            chemSequence.addChemModel(chemModel);

            setOfMolecules = chemFile.Builder.newSetOfMolecules();
            chemModel = chemFile.Builder.newChemModel();
            System.String str;
            try
            {
                System.String line;
                while ((line = input.ReadLine()) != null)
                {
                    //logger.debug("line: ", line);
                    // apparently, this is a SDF file, continue with 
                    // reading mol files
                    str = new System.Text.StringBuilder(line).ToString();
                    if (str.Equals("$$$$"))
                    {
                        m = readMolecule(chemFile.Builder.newMolecule());

                        if (m != null)
                        {
                            setOfMolecules.addMolecule(m);

                            chemModel.SetOfMolecules = setOfMolecules;
                            chemSequence.addChemModel(chemModel);

                            setOfMolecules = chemFile.Builder.newSetOfMolecules();
                            chemModel = chemFile.Builder.newChemModel();
                        }
                    }
                    else
                    {
                        // here the stuff between 'M  END' and '$$$$'
                        if (m != null)
                        {
                            // ok, the first lines should start with '>'
                            System.String fieldName = null;
                            if (str.StartsWith("> "))
                            {
                                // ok, should extract the field name
                                str.Substring(2); // String content = 
                                int index = str.IndexOf("<");
                                if (index != -1)
                                {
                                    int index2 = str.Substring(index).IndexOf(">");
                                    if (index2 != -1)
                                    {
                                        fieldName = str.Substring(index + 1, (index + index2) - (index + 1));
                                    }
                                }
                                // end skip all other lines
                                while ((line = input.ReadLine()) != null && line.StartsWith(">"))
                                {
                                    //logger.debug("data header line: ", line);
                                }
                            }
                            if (line == null)
                            {
                                throw new CDKException("Expecting data line here, but found null!");
                            }
                            System.String data = line;
                            while ((line = input.ReadLine()) != null && line.Trim().Length > 0)
                            {
                                if (line.Equals("$$$$"))
                                {
                                    //logger.error("Expecting data line here, but found end of molecule: ", line);
                                    break;
                                }
                                //logger.debug("data line: ", line);
                                data += line;
                            }
                            if (fieldName != null)
                            {
                                //logger.info("fieldName, data: ", fieldName, ", ", data);
                                m.setProperty(fieldName, data);
                            }
                        }
                    }
                }
            }
            catch (CDKException cdkexc)
            {
                throw cdkexc;
            }
            catch (System.Exception exception)
            {
                System.String error = "Error while parsing SDF";
                //logger.error(error);
                //logger.debug(exception);
                throw new CDKException(error, exception);
            }
            try
            {
                input.Close();
            }
            catch (System.Exception exc)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.String error = "Error while closing file: " + exc.Message;
                //logger.error(error);
                throw new CDKException(error, exc);
            }

            chemFile.addChemSequence(chemSequence);
            return chemFile;
        }



        /// <summary>  Read a Molecule from a file in MDL sd format
        /// 
        /// </summary>
        /// <returns>    The Molecule that was read from the MDL file.
        /// </returns>
        private IMolecule readMolecule(IMolecule molecule)
        {
            //logger.debug("Reading new molecule");
            int linecount = 0;
            int atoms = 0;
            int bonds = 0;
            int atom1 = 0;
            int atom2 = 0;
            int order = 0;
            int stereo = 0;
            int RGroupCounter = 1;
            int Rnumber = 0;
            System.String[] rGroup = null;
            double x = 0.0;
            double y = 0.0;
            double z = 0.0;
            double totalZ = 0.0;
            //int[][] conMat = new int[0][0];
            //String help;
            IBond bond;
            IAtom atom;
            System.String line = "";

            try
            {
                //logger.info("Reading header");
                line = input.ReadLine(); linecount++;
                if (line == null)
                {
                    return null;
                }
                //logger.debug("Line " + linecount + ": " + line);

                if (line.StartsWith("$$$$"))
                {
                    //logger.debug("File is empty, returning empty molecule");
                    return molecule;
                }
                if (line.Length > 0)
                {
                    molecule.setProperty(CDKConstants.TITLE, line);
                }
                line = input.ReadLine(); linecount++;
                //logger.debug("Line " + linecount + ": " + line);
                line = input.ReadLine(); linecount++;
                //logger.debug("Line " + linecount + ": " + line);
                if (line.Length > 0)
                {
                    molecule.setProperty(CDKConstants.REMARK, line);
                }

                //logger.info("Reading rest of file");
                line = input.ReadLine(); linecount++;
                //logger.debug("Line " + linecount + ": " + line);
                atoms = System.Int32.Parse(line.Substring(0, (3) - (0)).Trim());
                //logger.debug("Atomcount: " + atoms);
                bonds = System.Int32.Parse(line.Substring(3, (6) - (3)).Trim());
                //logger.debug("Bondcount: " + bonds);

                // read ATOM block
                //logger.info("Reading atom block");
                for (int f = 0; f < atoms; f++)
                {
                    line = input.ReadLine(); linecount++;
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    x = System.Double.Parse(line.Substring(0, (10) - (0)).Trim());
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    y = System.Double.Parse(line.Substring(10, (20) - (10)).Trim());
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    z = System.Double.Parse(line.Substring(20, (30) - (20)).Trim());
                    totalZ += System.Math.Abs(z); // *all* values should be zero, not just the sum
                    //logger.debug("Coordinates: " + x + "; " + y + "; " + z);
                    System.String element = line.Substring(31, (34) - (31)).Trim();

                    //logger.debug("Atom type: ", element);
                    if (IsotopeFactory.getInstance(molecule.Builder).isElement(element))
                    {
                        atom = IsotopeFactory.getInstance(molecule.Builder).configure(molecule.Builder.newAtom(element));
                    }
                    else
                    {
                        //logger.debug("Atom ", element, " is not an regular element. Creating a PseudoAtom.");
                        //check if the element is R
                        rGroup = element.Split(new char[] { '^', 'R' }); // ????
                        if (rGroup.Length > 1)
                        {
                            try
                            {
                                Rnumber = System.Int32.Parse(rGroup[(rGroup.Length - 1)]);
                                RGroupCounter = Rnumber;
                            }
                            catch (System.Exception ex)
                            {
                                Rnumber = RGroupCounter;
                                RGroupCounter++;
                            }
                            element = "R" + Rnumber;
                        }
                        atom = molecule.Builder.newPseudoAtom(element);
                    }

                    // store as 3D for now, convert to 2D (if totalZ == 0.0) later
                    atom.setPoint3d(new Point3d(x, y, z));

                    // parse further fields
                    System.String massDiffString = line.Substring(34, (36) - (34)).Trim();
                    //logger.debug("Mass difference: ", massDiffString);
                    if (!(atom is IPseudoAtom))
                    {
                        try
                        {
                            int massDiff = System.Int32.Parse(massDiffString);
                            if (massDiff != 0)
                            {
                                IIsotope major = IsotopeFactory.getInstance(molecule.Builder).getMajorIsotope(element);
                                atom.AtomicNumber = major.AtomicNumber + massDiff;
                            }
                        }
                        catch (System.Exception exception)
                        {
                            //logger.error("Could not parse mass difference field");
                        }
                    }
                    else
                    {
                        //logger.error("Cannot set mass difference for a non-element!");
                    }


                    System.String chargeCodeString = line.Substring(36, (39) - (36)).Trim();
                    //logger.debug("Atom charge code: ", chargeCodeString);
                    int chargeCode = System.Int32.Parse(chargeCodeString);
                    if (chargeCode == 0)
                    {
                        // uncharged species
                    }
                    else if (chargeCode == 1)
                    {
                        atom.setFormalCharge(+3);
                    }
                    else if (chargeCode == 2)
                    {
                        atom.setFormalCharge(+2);
                    }
                    else if (chargeCode == 3)
                    {
                        atom.setFormalCharge(+1);
                    }
                    else if (chargeCode == 4)
                    {
                    }
                    else if (chargeCode == 5)
                    {
                        atom.setFormalCharge(-1);
                    }
                    else if (chargeCode == 6)
                    {
                        atom.setFormalCharge(-2);
                    }
                    else if (chargeCode == 7)
                    {
                        atom.setFormalCharge(-3);
                    }

                    try
                    {
                        // read the mmm field as position 61-63
                        System.String reactionAtomIDString = line.Substring(60, (63) - (60)).Trim();
                        //logger.debug("Parsing mapping id: ", reactionAtomIDString);
                        try
                        {
                            int reactionAtomID = System.Int32.Parse(reactionAtomIDString);
                            if (reactionAtomID != 0)
                            {
                                atom.ID = reactionAtomIDString;
                            }
                        }
                        catch (System.Exception exception)
                        {
                            //logger.error("Mapping number ", reactionAtomIDString, " is not an integer.");
                            //logger.debug(exception);
                        }
                    }
                    catch (System.Exception exception)
                    {
                        // older mol files don't have all these fields...
                        //logger.warn("A few fields are missing. Older MDL MOL file?");
                    }

                    molecule.addAtom(atom);
                }

                // convert to 2D, if totalZ == 0
                if (totalZ == 0.0 && !forceReadAs3DCoords.Set)
                {
                    //logger.info("Total 3D Z is 0.0, interpreting it as a 2D structure");
                    IAtom[] atomsToUpdate = molecule.Atoms;
                    for (int f = 0; f < atomsToUpdate.Length; f++)
                    {
                        IAtom atomToUpdate = atomsToUpdate[f];
                        Point3d p3d = atomToUpdate.getPoint3d();
                        atomToUpdate.setPoint2d(new Point2d(p3d.x, p3d.y));
                        atomToUpdate.setPoint3d(null);
                    }
                }

                // read BOND block
                //logger.info("Reading bond block");
                for (int f = 0; f < bonds; f++)
                {
                    line = input.ReadLine(); linecount++;
                    atom1 = System.Int32.Parse(line.Substring(0, (3) - (0)).Trim());
                    atom2 = System.Int32.Parse(line.Substring(3, (6) - (3)).Trim());
                    order = System.Int32.Parse(line.Substring(6, (9) - (6)).Trim());
                    if (line.Length > 12)
                    {
                        stereo = System.Int32.Parse(line.Substring(9, (12) - (9)).Trim());
                    }
                    else
                    {
                        //logger.warn("Missing expected stereo field at line: " + line);
                    }
                    //if (//logger.DebugEnabled)
                    //{
                    //    //logger.debug("Bond: " + atom1 + " - " + atom2 + "; order " + order);
                    //}
                    if (stereo == 1)
                    {
                        // MDL up bond
                        stereo = CDKConstants.STEREO_BOND_UP;
                    }
                    else if (stereo == 6)
                    {
                        // MDL down bond
                        stereo = CDKConstants.STEREO_BOND_DOWN;
                    }
                    else if (stereo == 4)
                    {
                        //MDL bond undefined
                        stereo = CDKConstants.STEREO_BOND_UNDEFINED;
                    }
                    // interpret CTfile's special bond orders
                    IAtom a1 = molecule.getAtomAt(atom1 - 1);
                    IAtom a2 = molecule.getAtomAt(atom2 - 1);
                    if (order == 4)
                    {
                        // aromatic bond
                        bond = molecule.Builder.newBond(a1, a2, CDKConstants.BONDORDER_AROMATIC, stereo);
                        // mark both atoms and the bond as aromatic
                        bond.setFlag(CDKConstants.ISAROMATIC, true);
                        a1.setFlag(CDKConstants.ISAROMATIC, true);
                        a2.setFlag(CDKConstants.ISAROMATIC, true);
                        molecule.addBond(bond);
                    }
                    else
                    {
                        bond = molecule.Builder.newBond(a1, a2, (double)order, stereo);
                        molecule.addBond(bond);
                    }
                }

                // read PROPERTY block
                //logger.info("Reading property block");
                while (true)
                {
                    line = input.ReadLine(); linecount++;
                    if (line == null)
                    {
                        throw new CDKException("The expected property block is missing!");
                    }
                    if (line.StartsWith("M  END"))
                        break;

                    bool lineRead = false;
                    if (line.StartsWith("M  CHG"))
                    {
                        // FIXME: if this is encountered for the first time, all
                        // atom charges should be set to zero first!
                        int infoCount = System.Int32.Parse(line.Substring(6, (9) - (6)).Trim());
                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(line.Substring(9));
                        for (int i = 1; i <= infoCount; i++)
                        {
                            System.String token = st.NextToken();
                            int atomNumber = System.Int32.Parse(token.Trim());
                            token = st.NextToken();
                            int charge = System.Int32.Parse(token.Trim());
                            molecule.getAtomAt(atomNumber - 1).setFormalCharge(charge);
                        }
                    }
                    else if (line.StartsWith("M  ISO"))
                    {
                        try
                        {
                            System.String countString = line.Substring(6, (9) - (6)).Trim();
                            int infoCount = System.Int32.Parse(countString);
                            SupportClass.Tokenizer st = new SupportClass.Tokenizer(line.Substring(9));
                            for (int i = 1; i <= infoCount; i++)
                            {
                                int atomNumber = System.Int32.Parse(st.NextToken().Trim());
                                int absMass = System.Int32.Parse(st.NextToken().Trim());
                                if (absMass != 0)
                                {
                                    IAtom isotope = molecule.getAtomAt(atomNumber - 1);
                                    isotope.MassNumber = absMass;
                                }
                            }
                        }
                        catch (System.FormatException exception)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            System.String error = "Error (" + exception.Message + ") while parsing line " + linecount + ": " + line + " in property block.";
                            //logger.error(error);
                            throw new CDKException("NumberFormatException in isotope information on line: " + line, exception);
                        }
                    }
                    else if (line.StartsWith("M  RAD"))
                    {
                        try
                        {
                            System.String countString = line.Substring(6, (9) - (6)).Trim();
                            int infoCount = System.Int32.Parse(countString);
                            SupportClass.Tokenizer st = new SupportClass.Tokenizer(line.Substring(9));
                            for (int i = 1; i <= infoCount; i++)
                            {
                                int atomNumber = System.Int32.Parse(st.NextToken().Trim());
                                int spinMultiplicity = System.Int32.Parse(st.NextToken().Trim());
                                if (spinMultiplicity > 1)
                                {
                                    IAtom radical = molecule.getAtomAt(atomNumber - 1);
                                    for (int j = 2; j <= spinMultiplicity; j++)
                                    {
                                        // 2 means doublet -> one unpaired electron
                                        // 3 means triplet -> two unpaired electron
                                        molecule.addElectronContainer(molecule.Builder.newSingleElectron(radical));
                                    }
                                }
                            }
                        }
                        catch (System.FormatException exception)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            System.String error = "Error (" + exception.Message + ") while parsing line " + linecount + ": " + line + " in property block.";
                            //logger.error(error);
                            throw new CDKException("NumberFormatException in radical information on line: " + line, exception);
                        }
                    }
                    else if (line.StartsWith("G  "))
                    {
                        try
                        {
                            System.String atomNumberString = line.Substring(3, (6) - (3)).Trim();
                            int atomNumber = System.Int32.Parse(atomNumberString);
                            //String whatIsThisString = line.substring(6,9).trim();

                            System.String atomName = input.ReadLine();

                            // convert Atom into a PseudoAtom
                            IAtom prevAtom = molecule.getAtomAt(atomNumber - 1);
                            IPseudoAtom pseudoAtom = molecule.Builder.newPseudoAtom(atomName);
                            if (prevAtom.getPoint2d() != null)
                            {
                                pseudoAtom.setPoint2d(prevAtom.getPoint2d());
                            }
                            if (prevAtom.getPoint3d() != null)
                            {
                                pseudoAtom.setPoint3d(prevAtom.getPoint3d());
                            }
                            AtomContainerManipulator.replaceAtomByAtom(molecule, prevAtom, pseudoAtom);
                        }
                        catch (System.FormatException exception)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            System.String error = "Error (" + exception.ToString() + ") while parsing line " + linecount + ": " + line + " in property block.";
                            //logger.error(error);
                            throw new CDKException("NumberFormatException in group information on line: " + line, exception);
                        }
                    }
                    if (!lineRead)
                    {
                        //logger.warn("Skipping line in property block: ", line);
                    }
                }
            }
            catch (CDKException exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.String error = "Error while parsing line " + linecount + ": " + line + " -> " + exception.Message;
                //logger.error(error);
                //logger.debug(exception);
                throw exception;
            }
            catch (System.Exception exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.String error = "Error while parsing line " + linecount + ": " + line + " -> " + exception.Message;
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

        private void initIOSettings()
        {
            forceReadAs3DCoords = new BooleanIOSetting("ForceReadAs3DCoordinates", IOSetting.LOW, "Should coordinates always be read as 3D?", "false");
        }

        public virtual void customizeJob()
        {
            fireIOSettingQuestion(forceReadAs3DCoords);
        }
    }
}