/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-07-12 15:46:26 +0200 (Wed, 12 Jul 2006) $
* $Revision: 6641 $
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
* 
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Interfaces;
using System.IO;
using System.Text;
using Support;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Writes a reaction to a MDL rxn or SDF file. Attention: Stoichiometric
    /// coefficients have to be natural numbers.
    /// 
    /// <pre>
    /// MDLRXNWriter writer = new MDLRXNWriter(new FileWriter(new File("output.mol")));
    /// writer.write((Molecule)molecule);
    /// writer.close();
    /// </pre>
    /// 
    /// See {@cdk.cite DAL92}.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format, MDL RXN file </cdk.keyword>
    public class MDLRXNWriter : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MDLFormat();
            }

        }

        internal static System.IO.StreamWriter writer;
        //private LoggingTool //logger;


        /// <summary> Contructs a new MDLWriter that can write an array of 
        /// Molecules to a Writer.
        /// 
        /// </summary>
        /// <param name="out"> The Writer to write to
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MDLRXNWriter(System.IO.StreamWriter out_Renamed)
        {
            //logger = new LoggingTool(this);
            try
            {
                if (out_Renamed is System.IO.StreamWriter)
                {
                    writer = (System.IO.StreamWriter)out_Renamed;
                }
                else
                {
                    writer = new System.IO.StreamWriter(out_Renamed.BaseStream, out_Renamed.Encoding);
                }
            }
            catch (System.Exception exc)
            {
            }
        }

        /// <summary> Contructs a new MDLWriter that can write an array of
        /// Molecules to a given OutputStream.
        /// 
        /// </summary>
        /// <param name="output"> The OutputStream to write to
        /// </param>
        //UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
        public MDLRXNWriter(System.IO.Stream output)
            : this(new System.IO.StreamWriter(new System.IO.StreamWriter(output, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(output, System.Text.Encoding.Default).Encoding))
        {
        }

        public MDLRXNWriter()
            : this((StreamWriter)null)
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setWriter(System.IO.StreamWriter out_Renamed)
        {
            if (out_Renamed is System.IO.StreamWriter)
            {
                writer = (System.IO.StreamWriter)out_Renamed;
            }
            else
            {
                writer = new System.IO.StreamWriter(out_Renamed.BaseStream, out_Renamed.Encoding);
            }
        }

        public override void setWriter(System.IO.Stream output)
        {
            setWriter(new System.IO.StreamWriter(output, System.Text.Encoding.Default));
        }

        /// <summary> Flushes the output and closes this object.</summary>
        public override void close()
        {
            writer.Close();
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IReaction).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Writes a IChemObject to the MDL RXN file formated output. 
        /// It can only output ChemObjects of type Reaction
        /// 
        /// </summary>
        /// <param name="object">class must be of type Molecule or SetOfMolecules.
        /// 
        /// </param>
        /// <seealso cref="org.openscience.cdk.ChemFile">
        /// </seealso>
        public override void write(IChemObject object_Renamed)
        {
            if (object_Renamed is IReaction)
            {
                writeReaction((IReaction)object_Renamed);
            }
            else
            {
                throw new CDKException("Only supported is writing Reaction objects.");
            }
        }

        /// <summary> Writes a Reaction to an OutputStream in MDL sdf format.
        /// 
        /// </summary>
        /// <param name="reaction"> A Reaction that is written to an OutputStream 
        /// </param>
        private void writeReaction(IReaction reaction)
        {
            int reactantCount = reaction.ReactantCount;
            int productCount = reaction.ProductCount;
            if (reactantCount <= 0 || productCount <= 0)
            {
                throw new CDKException("Either no reactants or no products present.");
            }

            try
            {
                writer.Write("$RXN\n");
                // reaction name
                System.String line = (System.String)reaction.getProperty(CDKConstants.TITLE);
                if (line == null)
                    line = "";
                if (line.Length > 80)
                    line = line.Substring(0, (80) - (0));
                writer.Write(line + "\n");
                // user/program/date&time/reaction registry no. line
                //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                writer.WriteLine();
                // comment line
                line = ((System.String)reaction.getProperty(CDKConstants.REMARK));
                if (line == null)
                    line = "";
                if (line.Length > 80)
                    line = line.Substring(0, (80) - (0));
                writer.Write(line + "\n");

                line = "";
                line += formatMDLInt(reactantCount, 3);
                line += formatMDLInt(productCount, 3);
                writer.Write(line + "\n");

                writeSetOfMolecules(reaction.Reactants);
                writeSetOfMolecules(reaction.Products);
            }
            catch (System.IO.IOException ex)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error(ex.Message);
                //logger.debug(ex);
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                throw new CDKException("Exception while writing MDL file: " + ex.Message, ex);
            }
        }

        /// <summary> Writes a SetOfMolecules to an OutputStream for the reaction.
        /// 
        /// </summary>
        /// <param name="som"> The SetOfMolecules that is written to an OutputStream 
        /// </param>
        private void writeSetOfMolecules(ISetOfMolecules som)
        {

            for (int i = 0; i < som.MoleculeCount; i++)
            {
                IMolecule mol = som.getMolecule(i);
                for (int j = 0; j < som.getMultiplier(i); j++)
                {
                    //MemoryStream ms = new MemoryStream();
                    //StreamWriter sw = new StreamWriter(ms);
                    writer.Write("$MOL\n");
                    MDLWriter mdlwriter = null;
                    try
                    {
                        mdlwriter = new MDLWriter(writer);
                    }
                    catch (System.Exception ex)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.error(ex.Message);
                        //logger.debug(ex);
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        throw new CDKException("Exception while creating MDLWriter: " + ex.Message, ex);
                    }
                    mdlwriter.write(mol);
                    //writer.Write(sw.ToString());
                }
            }
        }


        /// <summary> Formats an int to fit into the connectiontable and changes it 
        /// to a String.
        /// 
        /// </summary>
        /// <param name="i"> The int to be formated
        /// </param>
        /// <param name="l"> Length of the String
        /// </param>
        /// <returns>     The String to be written into the connectiontable
        /// </returns>
        private System.String formatMDLInt(int i, int l)
        {
            System.String s = "", fs = "";
            SupportClass.TextNumberFormat nf = SupportClass.TextNumberFormat.getTextNumberInstance(new System.Globalization.CultureInfo("en"));
            //UPGRADE_ISSUE: Method 'java.text.NumberFormat.setParseIntegerOnly' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextNumberFormatsetParseIntegerOnly_boolean'"
            
            //nf.setParseIntegerOnly(true);
            
            nf.setMinimumIntegerDigits(1);
            nf.setMaximumIntegerDigits(l);
            nf.GroupingUsed = false;
            s = nf.FormatLong(i);
            l = l - s.Length;
            for (int f = 0; f < l; f++)
                fs += " ";
            fs += s;
            return fs;
        }
    }
}