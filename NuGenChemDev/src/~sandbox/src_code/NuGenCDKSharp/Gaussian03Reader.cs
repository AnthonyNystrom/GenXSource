/* $RCSfile$
* $Author: rajarshi $
* $Date: 2006-06-10 17:12:48 +0200 (Sat, 10 Jun 2006) $
* $Revision: 6416 $
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
*  This library is distributed in the hope that it will be useful,
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
    /// <summary> A reader for Gaussian03 output.
    /// Gaussian 03 is a quantum chemistry program
    /// by Gaussian, Inc. (http://www.gaussian.com/).
    /// <p/>
    /// <p>Molecular coordinates, energies, and normal coordinates of
    /// vibrations are read. Each set of coordinates is added to the
    /// ChemFile in the order they are found. Energies and vibrations
    /// are associated with the previously read set of coordinates.
    /// <p/>
    /// <p>This reader was developed from a small set of
    /// example output files, and therefore, is not guaranteed to
    /// properly read all Gaussian03 output. If you have problems,
    /// please contact the author of this code, not the developers
    /// of Gaussian03.
    /// <p/>
    /// <p>This code was adaptated by Jonathan from Gaussian98Reader written by
    /// Bradley, and ported to CDK by Egon.
    /// 
    /// </summary>
    /// <author>  Jonathan C. Rienstra-Kiracofe <jrienst@emory.edu>
    /// </author>
    /// <author>  Bradley A. Smith <yeldar@home.com>
    /// </author>
    /// <author>  Egon Willighagen
    /// </author>
    /// <cdk.module>  io </cdk.module>
    public class Gaussian03Reader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new Gaussian03Format();
            }

        }

        private System.IO.StreamReader input;
        //private LoggingTool //logger;

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public Gaussian03Reader(System.IO.StreamReader reader)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            input = new System.IO.StreamReader(reader.BaseStream, reader.CurrentEncoding);
            //logger = new LoggingTool(this);
        }

        public Gaussian03Reader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public Gaussian03Reader()
            : this((StreamReader)null)
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader reader)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
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
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
                if (typeof(IChemSequence).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemSequence)
            {
                return readChemSequence((IChemSequence)object_Renamed);
            }
            else if (object_Renamed is IChemFile)
            {
                return readChemFile((IChemFile)object_Renamed);
            }
            else
            {
                throw new CDKException("Object " + object_Renamed.GetType().FullName + " is not supported");
            }
        }

        public override void close()
        {
            input.Close();
        }

        private IChemFile readChemFile(IChemFile chemFile)
        {
            IChemSequence sequence = readChemSequence(chemFile.Builder.newChemSequence());
            chemFile.addChemSequence(sequence);
            return chemFile;
        }

        private IChemSequence readChemSequence(IChemSequence sequence)
        {
            IChemModel model = null;

            try
            {
                System.String line = input.ReadLine();
                //String levelOfTheory = null;

                // Find first set of coordinates
                while (input.Peek() != -1 && (line != null))
                {
                    if (line.IndexOf("Standard orientation:") >= 0)
                    {

                        // Found a set of coordinates
                        model = sequence.Builder.newChemModel();
                        try
                        {
                            readCoordinates(model);
                        }
                        catch (System.IO.IOException exception)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            throw new CDKException("Error while reading coordinates: " + exception.ToString(), exception);
                        }
                        break;
                    }
                    line = input.ReadLine();
                }
                if (model != null)
                {
                    // Read all other data
                    line = input.ReadLine();
                    while (input.Peek() != -1 && (line != null))
                    {
                        if (line.IndexOf("Standard orientation:") >= 0)
                        {
                            // Found a set of coordinates
                            // Add current frame to file and create a new one.
                            sequence.addChemModel(model);
                            fireFrameRead();
                            model = sequence.Builder.newChemModel();
                            readCoordinates(model);
                        }
                        else if (line.IndexOf("SCF Done:") >= 0)
                        {
                            // Found an energy
                            model.setProperty("org.openscience.cdk.io.Gaussian03Reaer:SCF Done", line.Trim());
                        }
                        else if (line.IndexOf("Harmonic frequencies") >= 0)
                        {
                            // Found a set of vibrations
                            try
                            {
                                readFrequencies(model);
                            }
                            catch (System.IO.IOException exception)
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                throw new CDKException("Error while reading frequencies: " + exception.ToString(), exception);
                            }
                        }
                        else if (line.IndexOf("Mulliken atomic charges") >= 0)
                        {
                            readPartialCharges(model);
                        }
                        else if (line.IndexOf("Magnetic shielding") >= 0)
                        {
                            // Found NMR data
                            try
                            {
                                readNMRData(model, line);
                            }
                            catch (System.IO.IOException exception)
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                throw new CDKException("Error while reading NMR data: " + exception.ToString(), exception);
                            }
                        }
                        else if (line.IndexOf("GINC") >= 0)
                        {
                            // Found calculation level of theory
                            //levelOfTheory = parseLevelOfTheory(line);
                            // FIXME: is doing anything with it?
                        }
                        line = input.ReadLine();
                    }

                    // Add current frame to file
                    sequence.addChemModel(model);
                    fireFrameRead();
                }
            }
            catch (System.IO.IOException exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                throw new CDKException("Error while reading general structure: " + exception.ToString(), exception);
            }
            return sequence;
        }

        /// <summary> Reads a set of coordinates into ChemModel.
        /// 
        /// </summary>
        /// <param name="model">the destination ChemModel
        /// </param>
        /// <throws>  IOException if an I/O error occurs </throws>
        private void readCoordinates(IChemModel model)
        {
            IAtomContainer container = model.Builder.newAtomContainer();
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
                int atomicNumber = 0;
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
                    throw new System.IO.IOException("Error reading coordinates");
                }
                token.NextToken();

                // ignore third token
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;
                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    x = token.nval;
                }
                else
                {
                    throw new System.IO.IOException("Error reading coordinates");
                }
                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    y = token.nval;
                }
                else
                {
                    throw new System.IO.IOException("Error reading coordinates");
                }
                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    z = token.nval;
                }
                else
                {
                    throw new System.IO.IOException("Error reading coordinates");
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
                container.addAtom(atom);
            }
            ISetOfMolecules moleculeSet = model.Builder.newSetOfMolecules();
            moleculeSet.addMolecule(model.Builder.newMolecule(container));
            model.SetOfMolecules = moleculeSet;
        }

        /// <summary> Reads partial atomic charges and add the to the given ChemModel.</summary>
        private void readPartialCharges(IChemModel model)
        {
            //logger.info("Reading partial atomic charges");
            ISetOfMolecules moleculeSet = model.SetOfMolecules;
            IMolecule molecule = moleculeSet.getMolecule(0);
            System.String line = input.ReadLine(); // skip first line after "Total atomic charges"
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

                    tokenizer.NextToken(); // ignore the symbol

                    double charge = 0.0;
                    if (tokenizer.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                    {
                        charge = (double)tokenizer.nval;
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

        /// <summary> Reads a set of vibrations into ChemModel.
        /// 
        /// </summary>
        /// <param name="model">the destination ChemModel
        /// </param>
        /// <throws>  IOException if an I/O error occurs </throws>
        private void readFrequencies(IChemModel model)
        {
            /* This is yet to be ported. Vibrations don't exist yet in CDK.
            String line = input.readLine();
            line = input.readLine();
            line = input.readLine();
            line = input.readLine();
            line = input.readLine();
            while ((line != null) && line.startsWith(" Frequencies --")) {
            Vector currentVibs = new Vector();
            StringReader vibValRead = new StringReader(line.substring(15));
            StreamTokenizer token = new StreamTokenizer(vibValRead);
            while (token.nextToken() != StreamTokenizer.TT_EOF) {
            Vibration vib = new Vibration(Double.toString(token.nval));
            currentVibs.addElement(vib);
            }
            line = input.readLine(); // skip "Red. masses"
            line = input.readLine(); // skip "Rfc consts"
            line = input.readLine(); // skip "IR Inten"
            while (!line.startsWith(" Atom AN")) {
            // skip all lines upto and including the " Atom AN" line
            line = input.readLine(); // skip
            }
            for (int i = 0; i < frame.getAtomCount(); ++i) {
            line = input.readLine();
            StringReader vectorRead = new StringReader(line);
            token = new StreamTokenizer(vectorRead);
            token.nextToken();
			
            // ignore first token
            token.nextToken();
			
            // ignore second token
            for (int j = 0; j < currentVibs.size(); ++j) {
            double[] v = new double[3];
            if (token.nextToken() == StreamTokenizer.TT_NUMBER) {
            v[0] = token.nval;
            } else {
            throw new IOException("Error reading frequency");
            }
            if (token.nextToken() == StreamTokenizer.TT_NUMBER) {
            v[1] = token.nval;
            } else {
            throw new IOException("Error reading frequency");
            }
            if (token.nextToken() == StreamTokenizer.TT_NUMBER) {
            v[2] = token.nval;
            } else {
            throw new IOException("Error reading frequency");
            }
            ((Vibration) currentVibs.elementAt(j)).addAtomVector(v);
            }
            }
            for (int i = 0; i < currentVibs.size(); ++i) {
            frame.addVibration((Vibration) currentVibs.elementAt(i));
            }
            line = input.readLine();
            line = input.readLine();
            line = input.readLine();
            } */
        }

        /// <summary> Reads NMR nuclear shieldings.</summary>
        private void readNMRData(IChemModel model, System.String labelLine)
        {
            /* FIXME: this is yet to be ported. CDK does not have shielding stuff.
            // Determine label for properties
            String label;
            if (labelLine.indexOf("Diamagnetic") >= 0) {
            label = "Diamagnetic Magnetic shielding (Isotropic)";
            } else if (labelLine.indexOf("Paramagnetic") >= 0) {
            label = "Paramagnetic Magnetic shielding (Isotropic)";
            } else {
            label = "Magnetic shielding (Isotropic)";
            }
            int atomIndex = 0;
            for (int i = 0; i < frame.getAtomCount(); ++i) {
            String line = input.readLine().trim();
            while (line.indexOf("Isotropic") < 0) {
            if (line == null) {
            return;
            }
            line = input.readLine().trim();
            }
            StringTokenizer st1 = new StringTokenizer(line);
			
            // Find Isotropic label
            while (st1.hasMoreTokens()) {
            if (st1.nextToken().equals("Isotropic")) {
            break;
            }
            }
			
            // Find Isotropic value
            while (st1.hasMoreTokens()) {
            if (st1.nextToken().equals("=")) {
            break;
            }
            }
            double shielding = Double.valueOf(st1.nextToken()).doubleValue();
            NMRShielding ns1 = new NMRShielding(label, shielding);
            ((org.openscience.jmol.Atom)frame.getAtomAt(atomIndex)).addProperty(ns1);
            ++atomIndex;
            } */
        }

        /// <summary> Select the theory and basis set from the first archive line.</summary>
        /*private String parseLevelOfTheory(String line) {
		
        StringTokenizer st1 = new StringTokenizer(line, "\\");
		
        // Must contain at least 6 tokens
        if (st1.countTokens() < 6) {
        return null;
        }
		
        // Skip first four tokens
        for (int i = 0; i < 4; ++i) {
        st1.nextToken();
        }
        return st1.nextToken() + "/" + st1.nextToken();
        }*/
    }
}