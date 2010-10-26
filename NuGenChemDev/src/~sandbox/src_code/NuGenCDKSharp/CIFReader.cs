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
*
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using javax.vecmath;
using Org.OpenScience.CDK.Geometry;
using Support;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> This is not a reader for the CIF and mmCIF crystallographic formats.
    /// It is able, however, to extract some content from such files. 
    /// It's very ad hoc, not written
    /// using any dictionary. So please complain if something is not working.
    /// In addition, the things it does read are considered experimental.
    /// 
    /// <p>The CIF example on the IUCR website has been tested, as well as Crambin (1CRN)
    /// in the PDB database.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format, CIF </cdk.keyword>
    /// <cdk.keyword>  file format, mmCIF </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <author>   E.L. Willighagen
    /// </author>
    /// <cdk.created>  2003-10-12 </cdk.created>
    public class CIFReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new CIFFormat();
            }

        }

        private System.IO.StreamReader input;
        //private LoggingTool //logger;

        private ICrystal crystal = null;
        // cell parameters
        private double a = 0.0;
        private double b = 0.0;
        private double c = 0.0;
        private double alpha = 0.0;
        private double beta = 0.0;
        private double gamma = 0.0;

        /// <summary> Create an CIF like file reader.
        /// 
        /// </summary>
        /// <param name="input">source of CIF data
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public CIFReader(System.IO.StreamReader input)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            //this.//logger = new LoggingTool(this);
        }

        public CIFReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public CIFReader()
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

        public override bool accepts(System.Type testClass)
        {
            System.Type[] interfaces = testClass.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Read a ChemFile from input
        /// 
        /// </summary>
        /// <returns> the content in a ChemFile object
        /// </returns>
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                IChemFile cf = (IChemFile)object_Renamed;
                try
                {
                    cf = readChemFile(cf);
                }
                catch (System.IO.IOException e)
                {
                    //logger.error("Input/Output error while reading from input.");
                }
                return cf;
            }
            else
            {
                throw new CDKException("Only supported is reading of ChemFile.");
            }
        }

        /// <summary> Read the ShelX from input. Each ShelX document is expected to contain
        /// one crystal structure.
        /// 
        /// </summary>
        /// <returns> a ChemFile with the coordinates, charges, vectors, etc.
        /// </returns>
        private IChemFile readChemFile(IChemFile file)
        {
            IChemSequence seq = file.Builder.newChemSequence();
            IChemModel model = file.Builder.newChemModel();
            crystal = file.Builder.newCrystal();

            System.String line = input.ReadLine();
            bool end_found = false;
            while (input.Peek() != -1 && line != null && !end_found)
            {
                if (line.StartsWith("#"))
                {
                    //logger.warn("Skipping comment: " + line);
                    // skip comment lines
                }
                else if (line.Length == 0)
                {
                    //logger.debug("Skipping empty line");
                    // skip empty lines
                }
                else if (!(line.StartsWith("_") || line.StartsWith("loop_")))
                {
                    //logger.warn("Skipping unrecognized line: " + line);
                    // skip line
                }
                else
                {

                    /* determine CIF command */
                    System.String command = "";
                    int spaceIndex = line.IndexOf(" ");
                    if (spaceIndex != -1)
                    {
                        // everything upto space is command
                        try
                        {
                            command = new System.Text.StringBuilder(line.Substring(0, (spaceIndex) - (0))).ToString();
                        }
                        catch (System.ArgumentOutOfRangeException sioobe)
                        {
                            // disregard this line
                            break;
                        }
                    }
                    else
                    {
                        // complete line is command
                        command = line;
                    }

                    //logger.debug("command: " + command);
                    if (command.StartsWith("_cell"))
                    {
                        processCellParameter(command, line);
                    }
                    else if (command.Equals("loop_"))
                    {
                        processLoopBlock();
                    }
                    else if (command.Equals("_symmetry_space_group_name_H-M"))
                    {
                        System.String value_Renamed = line.Substring(29).Trim();
                        crystal.SpaceGroup = value_Renamed;
                    }
                    else
                    {
                        // skip command
                        //logger.warn("Skipping command: " + command);
                        line = input.ReadLine();
                        if (line.StartsWith(";"))
                        {
                            //logger.debug("Skipping block content");
                            line = input.ReadLine().Trim();
                            while (!line.Equals(";"))
                            {
                                line = input.ReadLine().Trim();
                                //logger.debug("Skipping block line: " + line);
                            }
                            line = input.ReadLine();
                        }
                    }
                }
                line = input.ReadLine();
            }
            //logger.info("Adding crystal to file with #atoms: " + crystal.AtomCount);
            model.Crystal = crystal;
            seq.addChemModel(model);
            file.addChemSequence(seq);
            return file;
        }

        private void processCellParameter(System.String command, System.String line)
        {
            command = command.Substring(6); // skip the "_cell." part
            if (command.Equals("length_a"))
            {
                System.String value_Renamed = line.Substring(14).Trim();
                a = parseIntoDouble(value_Renamed);
                possiblySetCellParams(a, b, c, alpha, beta, gamma);
            }
            else if (command.Equals("length_b"))
            {
                System.String value_Renamed = line.Substring(14).Trim();
                b = parseIntoDouble(value_Renamed);
                possiblySetCellParams(a, b, c, alpha, beta, gamma);
            }
            else if (command.Equals("length_c"))
            {
                System.String value_Renamed = line.Substring(14).Trim();
                c = parseIntoDouble(value_Renamed);
                possiblySetCellParams(a, b, c, alpha, beta, gamma);
            }
            else if (command.Equals("angle_alpha"))
            {
                System.String value_Renamed = line.Substring(17).Trim();
                alpha = parseIntoDouble(value_Renamed);
                possiblySetCellParams(a, b, c, alpha, beta, gamma);
            }
            else if (command.Equals("angle_beta"))
            {
                System.String value_Renamed = line.Substring(16).Trim();
                beta = parseIntoDouble(value_Renamed);
                possiblySetCellParams(a, b, c, alpha, beta, gamma);
            }
            else if (command.Equals("angle_gamma"))
            {
                System.String value_Renamed = line.Substring(17).Trim();
                gamma = parseIntoDouble(value_Renamed);
                possiblySetCellParams(a, b, c, alpha, beta, gamma);
            }
        }

        private void possiblySetCellParams(double a, double b, double c, double alpha, double beta, double gamma)
        {
            if (a != 0.0 && b != 0.0 && c != 0.0 && alpha != 0.0 && beta != 0.0 && gamma != 0.0)
            {
                //logger.info("Found and set crystal cell parameters");
                Vector3d[] axes = CrystalGeometryTools.notionalToCartesian(a, b, c, alpha, beta, gamma);

                crystal.A = axes[0];
                crystal.B = axes[1];
                crystal.C = axes[2];
            }
        }

        private void processLoopBlock()
        {
            System.String line = input.ReadLine().Trim();
            if (line.StartsWith("_atom"))
            {
                //logger.info("Found atom loop block");
                processAtomLoopBlock(line);
            }
            else
            {
                //logger.warn("Skipping loop block");
                skipUntilEmptyOrCommentLine(line);
            }
        }

        private void skipUntilEmptyOrCommentLine(System.String line)
        {
            // skip everything until empty line, or comment line
            while (line != null && line.Length > 0 && line[0] != '#')
            {
                line = input.ReadLine().Trim();
            }
        }

        private void processAtomLoopBlock(System.String firstLine)
        {
            int atomLabel = -1; // -1 means not found in this block
            int atomSymbol = -1;
            int atomFractX = -1;
            int atomFractY = -1;
            int atomFractZ = -1;
            int atomRealX = -1;
            int atomRealY = -1;
            int atomRealZ = -1;
            System.String line = firstLine.Trim();
            int headerCount = 0;
            bool hasParsableInformation = false;
            while (line != null && line[0] == '_')
            {
                headerCount++;
                if (line.Equals("_atom_site_label") || line.Equals("_atom_site_label_atom_id"))
                {
                    atomLabel = headerCount;
                    hasParsableInformation = true;
                    //logger.info("label found in col: " + atomLabel);
                }
                else if (line.StartsWith("_atom_site_fract_x"))
                {
                    atomFractX = headerCount;
                    hasParsableInformation = true;
                    //logger.info("frac x found in col: " + atomFractX);
                }
                else if (line.StartsWith("_atom_site_fract_y"))
                {
                    atomFractY = headerCount;
                    hasParsableInformation = true;
                    //logger.info("frac y found in col: " + atomFractY);
                }
                else if (line.StartsWith("_atom_site_fract_z"))
                {
                    atomFractZ = headerCount;
                    hasParsableInformation = true;
                    //logger.info("frac z found in col: " + atomFractZ);
                }
                else if (line.Equals("_atom_site.Cartn_x"))
                {
                    atomRealX = headerCount;
                    hasParsableInformation = true;
                    //logger.info("cart x found in col: " + atomRealX);
                }
                else if (line.Equals("_atom_site.Cartn_y"))
                {
                    atomRealY = headerCount;
                    hasParsableInformation = true;
                    //logger.info("cart y found in col: " + atomRealY);
                }
                else if (line.Equals("_atom_site.Cartn_z"))
                {
                    atomRealZ = headerCount;
                    hasParsableInformation = true;
                    //logger.info("cart z found in col: " + atomRealZ);
                }
                else if (line.Equals("_atom_site.type_symbol"))
                {
                    atomSymbol = headerCount;
                    hasParsableInformation = true;
                    //logger.info("type_symbol found in col: " + atomSymbol);
                }
                else
                {
                    //logger.warn("Ignoring atom loop block field: " + line);
                }
                line = input.ReadLine().Trim();
            }
            if (hasParsableInformation == false)
            {
                //logger.info("No parsable info found");
                skipUntilEmptyOrCommentLine(line);
            }
            else
            {
                // now that headers are parsed, read the data
                while (line != null && line.Length > 0 && line[0] != '#')
                {
                    //logger.debug("new row");
                    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(line);
                    if (tokenizer.Count < headerCount)
                    {
                        //logger.warn("Column count mismatch; assuming continued on next line");
                        //logger.debug("Found #expected, #found: " + headerCount + ", " + tokenizer.Count);
                        tokenizer = new SupportClass.Tokenizer(line + input.ReadLine());
                    }
                    int colIndex = 0;
                    // process one row
                    IAtom atom = crystal.Builder.newAtom("C");
                    Point3d frac = new Point3d();
                    Point3d real = new Point3d();
                    bool hasFractional = false;
                    bool hasCartesian = false;
                    while (tokenizer.HasMoreTokens())
                    {
                        colIndex++;
                        System.String field = tokenizer.NextToken();
                        //logger.debug("Parsing col,token: " + colIndex + "=" + field);
                        if (colIndex == atomLabel)
                        {
                            if (atomSymbol == -1)
                            {
                                // no atom symbol found, use label
                                System.String element = extractFirstLetters(field);
                                atom.Symbol = element;
                            }
                            atom.ID = field;
                        }
                        else if (colIndex == atomFractX)
                        {
                            hasFractional = true;
                            frac.x = parseIntoDouble(field);
                        }
                        else if (colIndex == atomFractY)
                        {
                            hasFractional = true;
                            frac.y = parseIntoDouble(field);
                        }
                        else if (colIndex == atomFractZ)
                        {
                            hasFractional = true;
                            frac.z = parseIntoDouble(field);
                        }
                        else if (colIndex == atomSymbol)
                        {
                            atom.Symbol = field;
                        }
                        else if (colIndex == atomRealX)
                        {
                            hasCartesian = true;
                            //logger.debug("Adding x3: " + parseIntoDouble(field));
                            real.x = parseIntoDouble(field);
                        }
                        else if (colIndex == atomRealY)
                        {
                            hasCartesian = true;
                            //logger.debug("Adding y3: " + parseIntoDouble(field));
                            real.y = parseIntoDouble(field);
                        }
                        else if (colIndex == atomRealZ)
                        {
                            hasCartesian = true;
                            //logger.debug("Adding x3: " + parseIntoDouble(field));
                            real.z = parseIntoDouble(field);
                        }
                    }
                    if (hasCartesian)
                    {
                        Vector3d a = crystal.A;
                        Vector3d b = crystal.B;
                        Vector3d c = crystal.C;
                        frac = CrystalGeometryTools.cartesianToFractional(a, b, c, real);
                        atom.setFractionalPoint3d(frac);
                    }
                    if (hasFractional)
                    {
                        atom.setFractionalPoint3d(frac);
                    }
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.debug("Adding atom: " + atom);
                    crystal.addAtom(atom);

                    // look up next row
                    line = input.ReadLine().Trim();
                }
            }
        }

        /// <summary> Process double in the format: '.071(1)'.</summary>
        private double parseIntoDouble(System.String value_Renamed)
        {
            double returnVal = 0.0;
            if (value_Renamed[0] == '.')
                value_Renamed = "0" + value_Renamed;
            int bracketIndex = value_Renamed.IndexOf("(");
            if (bracketIndex != -1)
            {
                value_Renamed = value_Renamed.Substring(0, (bracketIndex) - (0));
            }
            try
            {
                returnVal = System.Double.Parse(value_Renamed);
            }
            catch (System.Exception exception)
            {
                //logger.error("Could not parse double string: " + value_Renamed);
            }
            return returnVal;
        }

        private System.String extractFirstLetters(System.String value_Renamed)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            for (int i = 0; i < value_Renamed.Length; i++)
            {
                if (System.Char.IsDigit(value_Renamed[i]))
                {
                    break;
                }
                else
                {
                    result.Append(value_Renamed[i]);
                }
            }
            return result.ToString();
        }

        public override void close()
        {
            input.Close();
        }
    }
}