/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-12 15:46:26 +0200 (Wed, 12 Jul 2006) $
*  $Revision: 6641 $
*
*  Copyright (C) 2002-2003  The Jmol Development Team
*  Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
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
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Support;
using Org.OpenScience.CDK.Config;
using javax.vecmath;
using Org.OpenScience.CDK.Tools.Manipulator;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> A reader for Gaussian98 output. Gaussian 98 is a quantum chemistry program
    /// by Gaussian, Inc. (http://www.gaussian.com/).
    /// <p/>
    /// <p>Molecular coordinates, energies, and normal coordinates of vibrations are
    /// read. Each set of coordinates is added to the ChemFile in the order they are
    /// found. Energies and vibrations are associated with the previously read set
    /// of coordinates.
    /// <p/>
    /// <p>This reader was developed from a small set of example output files, and
    /// therefore, is not guaranteed to properly read all Gaussian98 output. If you
    /// have problems, please contact the author of this code, not the developers of
    /// Gaussian98.
    /// 
    /// </summary>
    /// <author>  Bradley A. Smith <yeldar@home.com>
    /// </author>
    /// <author>  Egon Willighagen
    /// </author>
    /// <author>  Christoph Steinbeck
    /// </author>
    /// <cdk.module>  io </cdk.module>
    public class Gaussian98Reader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new Gaussian98Format();
            }

        }
        /// <summary>  Gets the iOSettings attribute of the Gaussian98Reader object
        /// 
        /// </summary>
        /// <returns> The iOSettings value
        /// </returns>
        override public IOSetting[] IOSettings
        {
            get
            {
                IOSetting[] settings = new IOSetting[1];
                settings[0] = readOptimizedStructureOnly;
                return settings;
            }

        }

        private System.IO.StreamReader input;
        //private LoggingTool //logger;
        private int atomCount = 0;
        private System.String lastRoute = "";

        /// <summary> Customizable setting</summary>
        private BooleanIOSetting readOptimizedStructureOnly;


        /// <summary> Constructor for the Gaussian98Reader object</summary>
        public Gaussian98Reader()
            : this((StreamReader)null)
        {
        }

        public Gaussian98Reader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        /// <summary> Sets the reader attribute of the Gaussian98Reader object
        /// 
        /// </summary>
        /// <param name="input">The new reader value
        /// </param>
        /// <throws>  CDKException Description of the Exception </throws>
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


        /// <summary> Create an Gaussian98 output reader.
        /// 
        /// </summary>
        /// <param name="input">source of Gaussian98 data
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public Gaussian98Reader(System.IO.StreamReader input)
        {
            //logger = new LoggingTool(this);
            if (input is System.IO.StreamReader)
            {
                this.input = (System.IO.StreamReader)input;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
            initIOSettings();
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Description of the Method
        /// 
        /// </summary>
        /// <param name="object">Description of the Parameter
        /// </param>
        /// <returns> Description of the Return Value
        /// </returns>
        /// <throws>  CDKException Description of the Exception </throws>
        public override IChemObject read(IChemObject object_Renamed)
        {
            customizeJob();

            if (object_Renamed is IChemFile)
            {
                IChemFile file = (IChemFile)object_Renamed;
                try
                {
                    file = readChemFile(file);
                }
                catch (System.IO.IOException exception)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    throw new CDKException("Error while reading file: " + exception.ToString(), exception);
                }
                return file;
            }
            else
            {
                throw new CDKException("Reading of a " + object_Renamed.GetType().FullName + " is not supported.");
            }
        }


        /// <summary> Description of the Method
        /// 
        /// </summary>
        /// <throws>  IOException Description of the Exception </throws>
        public override void close()
        {
            input.Close();
        }


        /// <summary> Read the Gaussian98 output.
        /// 
        /// </summary>
        /// <returns> a ChemFile with the coordinates, energies, and
        /// vibrations.
        /// </returns>
        /// <throws>  IOException  if an I/O error occurs </throws>
        /// <throws>  CDKException Description of the Exception </throws>
        private IChemFile readChemFile(IChemFile chemFile)
        {
            IChemSequence sequence = chemFile.Builder.newChemSequence();
            IChemModel model = null;
            System.String line = input.ReadLine();
            System.String levelOfTheory;
            System.String description;
            int modelCounter = 0;

            // Find first set of coordinates by skipping all before "Standard orientation"
            while (input.Peek() != -1 && (line != null))
            {
                if (line.IndexOf("Standard orientation:") >= 0)
                {

                    // Found a set of coordinates
                    model = chemFile.Builder.newChemModel();
                    readCoordinates(model);
                    break;
                }
                line = input.ReadLine();
            }
            if (model != null)
            {

                // Read all other data
                line = input.ReadLine().Trim();
                while (input.Peek() != -1 && (line != null))
                {
                    if (line.IndexOf("#") == 0)
                    {
                        // Found the route section
                        // Memorizing this for the description of the chemmodel
                        lastRoute = line;
                        modelCounter = 0;
                    }
                    else if (line.IndexOf("Standard orientation:") >= 0)
                    {

                        // Found a set of coordinates
                        // Add current frame to file and create a new one.
                        if (!readOptimizedStructureOnly.Set)
                        {
                            sequence.addChemModel(model);
                        }
                        else
                        {
                            //logger.info("Skipping frame, because I was told to do");
                        }
                        fireFrameRead();
                        model = chemFile.Builder.newChemModel();
                        modelCounter++;
                        readCoordinates(model);
                    }
                    else if (line.IndexOf("SCF Done:") >= 0)
                    {

                        // Found an energy
                        model.setProperty(CDKConstants.REMARK, line.Trim());
                    }
                    else if (line.IndexOf("Harmonic frequencies") >= 0)
                    {

                        // Found a set of vibrations
                        // readFrequencies(frame);
                    }
                    else if (line.IndexOf("Total atomic charges") >= 0)
                    {
                        readPartialCharges(model);
                    }
                    else if (line.IndexOf("Magnetic shielding") >= 0)
                    {

                        // Found NMR data
                        readNMRData(model, line);
                    }
                    else if (line.IndexOf("GINC") >= 0)
                    {

                        // Found calculation level of theory
                        levelOfTheory = parseLevelOfTheory(line);
                        //logger.debug("Level of Theory for this model: " + levelOfTheory);
                        description = lastRoute + ", model no. " + modelCounter;
                        model.setProperty(CDKConstants.DESCRIPTION, description);
                    }
                    else
                    {
                        ////logger.debug("Skipping line: " + line);
                    }
                    line = input.ReadLine();
                }

                // Add last frame to file
                sequence.addChemModel(model);
                fireFrameRead();
            }
            chemFile.addChemSequence(sequence);

            return chemFile;
        }


        /// <summary> Reads a set of coordinates into ChemFrame.
        /// 
        /// </summary>
        /// <param name="model">Description of the Parameter
        /// </param>
        /// <throws>  IOException  if an I/O error occurs </throws>
        /// <throws>  CDKException Description of the Exception </throws>
        private void readCoordinates(IChemModel model)
        {
            ISetOfMolecules moleculeSet = model.Builder.newSetOfMolecules();
            IMolecule molecule = model.Builder.newMolecule();
            System.String line = input.ReadLine();
            line = input.ReadLine();
            line = input.ReadLine();
            line = input.ReadLine();
            while (input.Peek() != -1)
            {
                line = input.ReadLine();
                if ((line == null) || (line.IndexOf("-----") >= 0))
                {
                    break;
                }
                int atomicNumber;
                System.IO.StringReader sr = new System.IO.StringReader(line);
                SupportClass.StreamTokenizerSupport token = new SupportClass.StreamTokenizerSupport(sr);
                token.NextToken();

                // ignore first token
                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    atomicNumber = (int)token.nval;
                    if (atomicNumber == 0)
                    {

                        // Skip dummy atoms. Dummy atoms must be skipped
                        // if frequencies are to be read because Gaussian
                        // does not report dummy atoms in frequencies, and
                        // the number of atoms is used for reading frequencies.
                        continue;
                    }
                }
                else
                {
                    throw new CDKException("Error while reading coordinates: expected integer.");
                }
                token.NextToken();

                // ignore third token
                double x;
                double y;
                double z;
                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    x = token.nval;
                }
                else
                {
                    throw new System.IO.IOException("Error reading x coordinate");
                }
                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    y = token.nval;
                }
                else
                {
                    throw new System.IO.IOException("Error reading y coordinate");
                }
                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    z = token.nval;
                }
                else
                {
                    throw new System.IO.IOException("Error reading z coordinate");
                }
                System.String symbol = "Du";
                try
                {
                    symbol = IsotopeFactory.getInstance(model.Builder).getElementSymbol(atomicNumber);
                }
                catch (System.Exception exception)
                {
                    throw new CDKException("Could not determine element symbol!", exception);
                }
                IAtom atom = model.Builder.newAtom(symbol);
                atom.setPoint3d(new Point3d(x, y, z));
                molecule.addAtom(atom);
            }
            /*
            *  this is the place where we store the atomcount to
            *  be used as a counter in the nmr reading
            */
            atomCount = molecule.AtomCount;
            moleculeSet.addMolecule(molecule);
            model.SetOfMolecules = moleculeSet;
        }


        /// <summary> Reads partial atomic charges and add the to the given ChemModel.
        /// 
        /// </summary>
        /// <param name="model">Description of the Parameter
        /// </param>
        /// <throws>  CDKException Description of the Exception </throws>
        /// <throws>  IOException  Description of the Exception </throws>
        private void readPartialCharges(IChemModel model)
        {
            //logger.info("Reading partial atomic charges");
            ISetOfMolecules moleculeSet = model.SetOfMolecules;
            IMolecule molecule = moleculeSet.getMolecule(0);
            System.String line = input.ReadLine();
            // skip first line after "Total atomic charges"
            while (input.Peek() != -1)
            {
                line = input.ReadLine();
                //logger.debug("Read charge block line: " + line);
                if ((line == null) || (line.IndexOf("Sum of Mulliken charges") >= 0))
                {
                    //logger.debug("End of charge block found");
                    break;
                }
                System.IO.StringReader sr = new System.IO.StringReader(line);
                SupportClass.StreamTokenizerSupport tokenizer = new SupportClass.StreamTokenizerSupport(sr);
                if (tokenizer.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    int atomCounter = (int)tokenizer.nval;

                    tokenizer.NextToken();
                    // ignore the symbol

                    double charge;
                    if (tokenizer.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                    {
                        charge = tokenizer.nval;
                        //logger.debug("Found charge for atom " + atomCounter + ": " + charge);
                    }
                    else
                    {
                        throw new CDKException("Error while reading charge: expected double.");
                    }
                    IAtom atom = molecule.getAtomAt(atomCounter - 1);
                    atom.setCharge(charge);
                }
            }
        }

        /// <summary>  Reads a set of vibrations into ChemFrame.
        /// 
        /// </summary>
        /// <param name="model">           Description of the Parameter
        /// </param>
        /// <exception cref="IOException"> if an I/O error occurs
        /// </exception>
        //	private void readFrequencies(IChemModel model) throws IOException
        //	{
        /*
        *  FIXME: this is yet to be ported
        *  String line;
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  while ((line != null) && line.startsWith(" Frequencies --")) {
        *  Vector currentVibs = new Vector();
        *  StringReader vibValRead = new StringReader(line.substring(15));
        *  StreamTokenizer token = new StreamTokenizer(vibValRead);
        *  while (token.nextToken() != StreamTokenizer.TT_EOF) {
        *  Vibration vib = new Vibration(Double.toString(token.nval));
        *  currentVibs.addElement(vib);
        *  }
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  for (int i = 0; i < frame.getAtomCount(); ++i) {
        *  line = input.readLine();
        *  StringReader vectorRead = new StringReader(line);
        *  token = new StreamTokenizer(vectorRead);
        *  token.nextToken();
        *  / ignore first token
        *  token.nextToken();
        *  / ignore second token
        *  for (int j = 0; j < currentVibs.size(); ++j) {
        *  double[] v = new double[3];
        *  if (token.nextToken() == StreamTokenizer.TT_NUMBER) {
        *  v[0] = token.nval;
        *  } else {
        *  throw new IOException("Error reading frequency");
        *  }
        *  if (token.nextToken() == StreamTokenizer.TT_NUMBER) {
        *  v[1] = token.nval;
        *  } else {
        *  throw new IOException("Error reading frequency");
        *  }
        *  if (token.nextToken() == StreamTokenizer.TT_NUMBER) {
        *  v[2] = token.nval;
        *  } else {
        *  throw new IOException("Error reading frequency");
        *  }
        *  ((Vibration) currentVibs.elementAt(j)).addAtomVector(v);
        *  }
        *  }
        *  for (int i = 0; i < currentVibs.size(); ++i) {
        *  frame.addVibration((Vibration) currentVibs.elementAt(i));
        *  }
        *  line = input.readLine();
        *  line = input.readLine();
        *  line = input.readLine();
        *  }
        */
        //	}


        /// <summary> Reads NMR nuclear shieldings.
        /// 
        /// </summary>
        /// <param name="model">    Description of the Parameter
        /// </param>
        /// <param name="labelLine">Description of the Parameter
        /// </param>
        /// <throws>  CDKException Description of the Exception </throws>
        private void readNMRData(IChemModel model, System.String labelLine)
        {
            IAtomContainer ac = ChemModelManipulator.getAllInOneContainer(model);
            // Determine label for properties
            System.String label;
            if (labelLine.IndexOf("Diamagnetic") >= 0)
            {
                label = "Diamagnetic Magnetic shielding (Isotropic)";
            }
            else if (labelLine.IndexOf("Paramagnetic") >= 0)
            {
                label = "Paramagnetic Magnetic shielding (Isotropic)";
            }
            else
            {
                label = "Magnetic shielding (Isotropic)";
            }
            int atomIndex = 0;
            for (int i = 0; i < atomCount; ++i)
            {
                try
                {
                    System.String line = input.ReadLine().Trim();
                    while (line.IndexOf("Isotropic") < 0)
                    {
                        if (line == null)
                        {
                            return;
                        }
                        line = input.ReadLine().Trim();
                    }
                    SupportClass.Tokenizer st1 = new SupportClass.Tokenizer(line);

                    // Find Isotropic label
                    while (st1.HasMoreTokens())
                    {
                        if (st1.NextToken().Equals("Isotropic"))
                        {
                            break;
                        }
                    }

                    // Find Isotropic value
                    while (st1.HasMoreTokens())
                    {
                        if (st1.NextToken().Equals("="))
                            break;
                    }
                    double shielding = System.Double.Parse(st1.NextToken());
                    //logger.info("Type of shielding: " + label);
                    ac.getAtomAt(atomIndex).setProperty(CDKConstants.ISOTROPIC_SHIELDING, (System.Object)shielding);
                    ++atomIndex;
                }
                catch (System.Exception exc)
                {
                    //logger.debug("failed to read line from gaussian98 file where I expected one.");
                }
            }
        }


        /// <summary> Select the theory and basis set from the first archive line.
        /// 
        /// </summary>
        /// <param name="line">Description of the Parameter
        /// </param>
        /// <returns> Description of the Return Value
        /// </returns>
        private System.String parseLevelOfTheory(System.String line)
        {
            System.Text.StringBuilder summary = new System.Text.StringBuilder();
            summary.Append(line);
            try
            {

                do
                {
                    line = input.ReadLine().Trim();
                    summary.Append(line);
                }
                while (!(line.IndexOf("@") >= 0));
            }
            catch (System.Exception exc)
            {
                //logger.debug("syntax problem while parsing summary of g98 section: ");
                //logger.debug(exc);
            }
            //logger.debug("parseLoT(): " + summary.ToString());
            SupportClass.Tokenizer st1 = new SupportClass.Tokenizer(summary.ToString(), "\\");

            // Must contain at least 6 tokens
            if (st1.Count < 6)
            {
                return null;
            }

            // Skip first four tokens
            for (int i = 0; i < 4; ++i)
            {
                st1.NextToken();
            }

            return st1.NextToken() + "/" + st1.NextToken();
        }


        /// <summary> Description of the Method</summary>
        private void initIOSettings()
        {
            readOptimizedStructureOnly = new BooleanIOSetting("ReadOptimizedStructureOnly", IOSetting.LOW, "Should I only read the optimized structure from a geometry optimization?", "false");
        }


        /// <summary> Description of the Method</summary>
        private void customizeJob()
        {
            fireIOSettingQuestion(readOptimizedStructureOnly);
        }
    }
}