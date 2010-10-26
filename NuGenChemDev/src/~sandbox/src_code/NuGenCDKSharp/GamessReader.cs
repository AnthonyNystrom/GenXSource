/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 11:39:20 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6669 $
*
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
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> A reader for GAMESS log file.
    /// 
    /// <p><b>Expected behaviour</b>: 
    /// <br>The "GamessReader" object is able to read GAMESS output log file format. 
    /// 
    /// <p><b>Limitations</b>: <br>This reader was developed from a small set of 
    /// example log files, and therefore, is not guaranteed to properly read all 
    /// GAMESS output. If you have problems, please contact the author of this code, 
    /// not the developers of GAMESS.
    /// 
    /// <!-- <p><b>State information</b>: <br> [] -->
    /// <!-- <p><b>Dependencies</b>: <br> [all OS/Software/Hardware dependencies] -->
    /// 
    /// <p><b>Implementation</b>
    /// <br>Available feature(s):
    /// <ul>
    /// <li><b>Molecular coordinates</b>: Each set of coordinates is added to the ChemFile in the order they are found.</li>
    /// </ul>
    /// Unavailable feature(s):
    /// <ul>
    /// <!--	<li><b>GAMESS version number</b>: The version number can be retrieved.</li> -->
    /// <!--	<li><b>Point group symetry information</b>: The point group is associated with the set of molecules.</li> -->
    /// <!--	<li><b>MOPAC charges</b>: The point group is associated with the set of molecules.</li> -->
    /// <li><b>Energies</b>: They are associated with the previously read set of coordinates.</li>
    /// <li><b>Normal coordinates of vibrations</b>: They are associated with the previously read set of coordinates.</li>
    /// </ul>
    /// 
    /// <!-- <p><b>Security:</b> -->
    /// 
    /// <p><b>References</b>: 
    /// <br><a href="http://www.msg.ameslab.gov/GAMESS/GAMESS.html">GAMESS</a> is a 
    /// quantum chemistry program by Gordon research group atIowa State University.
    /// 
    /// </summary>
    /// <cdk.module>   experimental </cdk.module>
    /// <cdk.keyword>  Gamess </cdk.keyword>
    /// <cdk.keyword>  file format </cdk.keyword>
    /// <cdk.keyword>  output </cdk.keyword>
    /// <cdk.keyword>  log file </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <author>  Bradley A. Smith
    /// 
    /// </author>
    /// <seealso cref="GamessWriter(Reader) -->">
    /// </seealso>
    //TODO Update class comments with appropriate information.
    //TODO Update "see" tag with reference to GamessWriter when it will be implemented.
    //TODO Update "author" tag with appropriate information. 
    public class GamessReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            /* (non-Javadoc) (Javadoc is automaticly inherited from the link below)
            * @see org.openscience.cdk.io.ChemObjectIO#accepts(org.openscience.cdk.ChemObject)
            */
            //TODO Update comment with appropriate information to comply Constructor's documentation. 


            get
            {
                return new GamessFormat();
            }

        }

        /// <summary> Boolean constant used to specify that the coordinates are given in Bohr units.</summary>
        public const bool BOHR_UNIT = true;

        /// <summary> Double constant that contains the convertion factor from Bohr unit to 
        /// &Aring;ngstrom unit.
        /// </summary>
        //TODO Check the accuracy of this comment.
        public const double BOHR_TO_ANGSTROM = 0.529177249;

        /// <summary> Boolean constant used to specify that the coordinates are given in &Aring;ngstrom units.</summary>
        public const bool ANGSTROM_UNIT = false;

        /// <summary> The "BufferedReader" object used to read data from the "file system" file.
        /// 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.io.GamessReader.GamessReader(Reader)">
        /// </seealso>
        //TODO Improve field comment.
        //TODO Answer the question : When is it opened and when is it closed?
        private System.IO.StreamReader input;

        /// <summary> Constructs a new "GamessReader" object given a "Reader" object as input.
        /// 
        /// <p>The "Reader" object may be an instantiable object from the "Reader" 
        /// hierarchy.
        /// <br>For more detail about the "Reader" objects that are really accepted 
        /// by this "GamessReader" see <code>accepts(IChemObject)</code> method
        /// documentation.
        /// 
        /// </summary>
        /// <param name="inputReader		The">"Reader" object given as input parameter.
        /// 
        /// </param>
        /// <seealso cref="accepts(Class)">
        /// </seealso>
        /// <seealso cref="java.io.Reader">
        /// 
        /// </seealso>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public GamessReader(System.IO.StreamReader inputReader)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            this.input = new System.IO.StreamReader(inputReader.BaseStream, inputReader.CurrentEncoding);
        }

        public GamessReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public GamessReader()
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
            }
            return false;
        }

        /* (non-Javadoc) (Javadoc is automaticly inherited from the link below)
        * @see org.openscience.cdk.io.ChemObjectReader#read(org.openscience.cdk.ChemObject)
        */
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                try
                {
                    return (IChemObject)readChemFile((IChemFile)object_Renamed);
                }
                catch (System.IO.IOException e)
                {
                    return null;
                }
            }
            else
            {
                throw new CDKException("Only supported is reading of ChemFile objects.");
            }
        }

        /// <summary> Reads data from the "file system" file through the use of the "input" 
        /// field, parses data and feeds the ChemFile object with the extracted data.
        /// 
        /// </summary>
        /// <returns> A ChemFile containing the data parsed from input.
        /// 
        /// </returns>
        /// <throws	IOException	may>  be thrown buy the <code>this.input.readLine()</code> instruction. </throws	IOException	may>
        /// <summary> 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.io.GamessReader.input">
        /// </seealso>
        //TODO Answer the question : Is this method's name appropriate (given the fact that it do not read a ChemFile object, but return it)? 
        private IChemFile readChemFile(IChemFile file)
        {
            IChemSequence sequence = file.Builder.newChemSequence(); // TODO Answer the question : Is this line needed ?
            IChemModel model = file.Builder.newChemModel(); // TODO Answer the question : Is this line needed ?
            ISetOfMolecules moleculeSet = file.Builder.newSetOfMolecules();

            model.SetOfMolecules = moleculeSet; //TODO Answer the question : Should I do this?
            sequence.addChemModel(model); //TODO Answer the question : Should I do this?
            file.addChemSequence(sequence); //TODO Answer the question : Should I do this?

            System.String currentReadLine = this.input.ReadLine();
            while (this.input.Peek() != -1 == true && (currentReadLine != null))
            {

                /*
                * There are 2 types of coordinate sets: 
                * - bohr coordinates sets		(if statement)
                * - angstr???m coordinates sets	(else statement)
                */
                if (currentReadLine.IndexOf("COORDINATES (BOHR)") >= 0)
                {

                    /* 
                    * The following line do no contain data, so it is ignored.
                    */
                    this.input.ReadLine();
                    moleculeSet.addMolecule(this.readCoordinates(file.Builder.newMolecule(), GamessReader.BOHR_UNIT));
                    //break; //<- stops when the first set of coordinates is found.
                }
                else if (currentReadLine.IndexOf(" COORDINATES OF ALL ATOMS ARE (ANGS)") >= 0)
                {

                    /* 
                    * The following 2 lines do no contain data, so it are ignored.
                    */
                    this.input.ReadLine();
                    this.input.ReadLine();

                    moleculeSet.addMolecule(this.readCoordinates(file.Builder.newMolecule(), GamessReader.ANGSTROM_UNIT));
                    //break; //<- stops when the first set of coordinates is found.
                }
                currentReadLine = this.input.ReadLine();
            }
            return file;
        }

        /// <summary> Reads a set of coordinates from the "file system" file through the use of 
        /// the "input" field, scales coordinate to angstr???m unit, builds each atom with 
        /// the right associated coordinates, builds a new molecule with these atoms
        /// and returns the complete molecule.
        /// 
        /// <p><b>Implementation</b>:
        /// <br>Dummy atoms are ignored.
        /// 
        /// </summary>
        /// <param name="coordinatesUnits	The">unit in which coordinates are given.
        /// 
        /// </param>
        /// <throws	IOException	may>  be thrown by the "input" object. </throws	IOException	may>
        /// <summary> 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.io.GamessReader.input">
        /// </seealso>
        //TODO Update method comments with appropriate information.
        private IMolecule readCoordinates(IMolecule molecule, bool coordinatesUnits)
        {

            /*
            * Coordinates must all be given in angstr???ms.
            */
            double unitScaling = GamessReader.scalesCoordinatesUnits(coordinatesUnits);

            System.String retrievedLineFromFile;

            while (this.input.Peek() != -1 == true)
            {
                retrievedLineFromFile = this.input.ReadLine();
                /* 
                * A coordinate set is followed by an empty line, so when this line 
                * is reached, there are no more coordinates to add to the current set.
                */
                if ((retrievedLineFromFile == null) || (retrievedLineFromFile.Trim().Length == 0))
                {
                    break;
                }

                int atomicNumber;
                System.String atomicSymbol;

                //StringReader sr = new StringReader(retrievedLineFromFile);
                SupportClass.StreamTokenizerSupport token = new SupportClass.StreamTokenizerSupport(new System.IO.StringReader(retrievedLineFromFile));

                /*
                * The first token is ignored. It contains the atomic symbol and may 
                * be concatenated with a number.
                */
                token.NextToken();

                if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    atomicNumber = (int)token.nval;
                    atomicSymbol = this.identifyAtomicSymbol(atomicNumber);
                    /* 
                    * Dummy atoms are assumed to be given with an atomic number set
                    * to zero. We will do not add them to the molecule.
                    */
                    if (atomicNumber == 0)
                    {
                        continue;
                    }
                }
                else
                {
                    throw new System.IO.IOException("Error reading coordinates");
                }

                /*
                * Atom's coordinates are stored in an array.
                */
                double[] coordinates = new double[3];
                for (int i = 0; i < coordinates.Length; i++)
                {
                    if (token.NextToken() == SupportClass.StreamTokenizerSupport.TT_NUMBER)
                    {
                        coordinates[i] = token.nval * unitScaling;
                    }
                    else
                    {
                        throw new System.IO.IOException("Error reading coordinates");
                    }
                }
                IAtom atom = molecule.Builder.newAtom(atomicSymbol, new Point3d(coordinates[0], coordinates[1], coordinates[2]));
                molecule.addAtom(atom);
            }
            return molecule;
        }

        /// <summary> Identifies the atomic symbol of an atom given its default atomic number.
        /// 
        /// <p><b>Implementation</b>:
        /// <br>This is not a definitive method. It will probably be replaced with a 
        /// more appropriate one. Be advised that as it is not a definitive version, 
        /// it only recognise atoms from Hydrogen (1) to Argon (18).
        /// 
        /// </summary>
        /// <param name="atomicNumber	The">atomic number of an atom.
        /// 
        /// </param>
        /// <returns>	The Symbol corresponding to the atom or "null" is the atom was not recognised.
        /// </returns>
        //TODO Update method comments with appropriate information.
        private System.String identifyAtomicSymbol(int atomicNumber)
        {
            System.String symbol;
            switch (atomicNumber)
            {

                case 1:
                    symbol = "H";
                    break;

                case 2:
                    symbol = "He";
                    break;

                case 3:
                    symbol = "Li";
                    break;

                case 4:
                    symbol = "Be";
                    break;

                case 5:
                    symbol = "B";
                    break;

                case 6:
                    symbol = "C";
                    break;

                case 7:
                    symbol = "N";
                    break;

                case 8:
                    symbol = "O";
                    break;

                case 9:
                    symbol = "F";
                    break;

                case 10:
                    symbol = "Ne";
                    break;

                case 11:
                    symbol = "Na";
                    break;

                case 12:
                    symbol = "Mg";
                    break;

                case 13:
                    symbol = "Al";
                    break;

                case 14:
                    symbol = "Si";
                    break;

                case 15:
                    symbol = "P";
                    break;

                case 16:
                    symbol = "S";
                    break;

                case 17:
                    symbol = "Cl";
                    break;

                case 18:
                    symbol = "Ar";
                    break;

                default:
                    symbol = null;
                    break;

            }
            return symbol;
        }

        /// <summary> Scales coordinates to &Aring;ngstr&ouml;m unit if they are given in Bohr unit. 
        /// If coordinates are already given in &Aring;ngstr&ouml;m unit, then no modifications
        /// are performed.
        /// 
        /// </summary>
        /// <param name="coordinatesUnits	<code>BOHR_UNIT</code>">if coordinates are given in Bohr unit and <code>ANGSTROM_UNIT</code> 
        /// if they are given in &Aring;ngstr&ouml;m unit.
        /// 
        /// </param>
        /// <returns>	The scaling convertion factor: 1 if no scaling is needed and <code>BOHR_TO_ANGSTROM</code> if scaling has to be performed.
        /// 
        /// </returns>
        /// <seealso cref="org.openscience.cdk.PhysicalConstants.BOHR_TO_ANGSTROM">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.io.GamessReader.BOHR_UNIT">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.io.GamessReader.ANGSTROM_UNIT">
        /// </seealso>
        //TODO Update method comments with appropriate information.
        private static double scalesCoordinatesUnits(bool coordinatesUnits)
        {
            if (coordinatesUnits == GamessReader.BOHR_UNIT)
            {
                return PhysicalConstants.BOHR_TO_ANGSTROM;
            }
            else
            {
                //condition is: (coordinatesUnits == GamessReader.ANGTROM_UNIT)
                return (double)1;
            }
        }

        /* (non-Javadoc) (Javadoc is automaticly inherited from the link below)
        * @see org.openscience.cdk.io.ChemObjectIO#close()
        */
        //TODO Answer the question : What are all concerned ressources ? 
        public override void close()
        {
            /* 
            * Closes the BufferedReader used to read the file content.
            */
            input.Close();
        }
    }
}