/* $RCSfile$ 
* $Author: kaihartmann $ 
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
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
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Smiles;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Writes the SMILES strings to a plain text file.
    /// 
    /// </summary>
    /// <cdk.module>   smiles </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format </cdk.keyword>
    public class SMILESWriter : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new SMILESFormat();
            }

        }

        //private LoggingTool //logger;
        internal static System.IO.StreamWriter writer;

        /// <summary> Contructs a new SMILESWriter that can write a list of SMILES to a Writer
        /// 
        /// </summary>
        /// <param name="out"> The Writer to write to
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public SMILESWriter(System.IO.StreamWriter out_Renamed)
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

        public SMILESWriter(System.IO.Stream output)
            : this(new System.IO.StreamWriter(output, System.Text.Encoding.Default))
        {
        }

        public SMILESWriter()
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

        /// <summary> Contructs a new SMILESWriter that can write an list of SMILES to a given OutputStream
        /// 
        /// </summary>
        /// <param name="out"> The OutputStream to write to
        /// </param>
        public SMILESWriter(System.IO.FileStream out_Renamed)
            : this(new System.IO.StreamWriter(out_Renamed, System.Text.Encoding.Default))
        {
        }

        /// <summary> Flushes the output and closes this object</summary>
        public override void close()
        {
            writer.Flush();
            writer.Close();
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(ISetOfMolecules).Equals(interfaces[i]))
                    return true;
                if (typeof(IMolecule).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Writes the content from object to output.
        /// 
        /// </summary>
        /// <param name="object"> IChemObject of which the data is outputted.
        /// </param>
        public override void write(IChemObject object_Renamed)
        {
            if (object_Renamed is ISetOfMolecules)
            {
                writeSetOfMolecules((ISetOfMolecules)object_Renamed);
            }
            else if (object_Renamed is IMolecule)
            {
                writeMolecule((IMolecule)object_Renamed);
            }
            else
            {
                throw new CDKException("Only supported is writing of ChemFile and Molecule objects.");
            }
        }

        /// <summary> Writes a list of molecules to an OutputStream
        /// 
        /// </summary>
        /// <param name="som"> SetOfMolecules that is written to an OutputStream
        /// </param>
        public virtual void writeSetOfMolecules(ISetOfMolecules som)
        {
            IMolecule[] molecules = som.Molecules;
            writeMolecule(molecules[0]);
            for (int i = 1; i <= som.MoleculeCount - 1; i++)
            {
                try
                {
                    writeMolecule(molecules[i]);
                }
                catch (System.Exception exc)
                {
                }
            }
        }

        /// <summary> Writes the content from molecule to output.
        /// 
        /// </summary>
        /// <param name="molecule"> Molecule of which the data is outputted.
        /// </param>
        public virtual void writeMolecule(IMolecule molecule)
        {
            SmilesGenerator sg = new SmilesGenerator(molecule.Builder);
            System.String smiles = "";
            try
            {
                smiles = sg.createSMILES(molecule);
                //logger.debug("Generated SMILES: " + smiles);
                writer.Write(smiles);
                //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                writer.WriteLine();
                writer.Flush();
                //logger.debug("file flushed...");
            }
            catch (System.Exception exc)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Error while writing Molecule: ", exc.Message);
                //logger.debug(exc);
            }
        }
    }
}