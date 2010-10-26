/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Inchi;
using System.IO;
using Org.OpenScience.CDK.Exception;
using Support;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads the content of a IUPAC/NIST Chemical Identifier (INChI) plain text 
    /// document. This reader parses output generated with INChI 1.12beta like:
    /// <pre>
    /// 
    /// Input_File: "E:\Program Files\INChI\inchi-samples\Figure04.mol"
    /// 
    /// Structure: 1
    /// INChI=1.12Beta/C6H6/c1-2-4-6-5-3-1/h1-6H
    /// AuxInfo=1.12Beta/0/N:1,2,3,4,5,6/E:(1,2,3,4,5,6)/rA:6CCCCCC/rB:s1;d1;d2;s3;s4d5;/rC:5.6378,-4.0013,0;5.6378,-5.3313,0;4.4859,-3.3363,0;4.4859,-5.9963,0;3.3341,-4.0013,0;3.3341,-5.3313,0;
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  experimental </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    /// <cdk.created>  2004-08-01 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format, INChI </cdk.keyword>
    /// <cdk.keyword>  chemical identifier </cdk.keyword>
    /// <cdk.require>  java1.4+ </cdk.require>
    /// <summary> 
    /// </summary>
    /// <seealso cref="org.openscience.cdk.io.INChIReader">
    /// </seealso>
    public class INChIPlainTextReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new INChIPlainTextFormat();
            }

        }

        private System.IO.StreamReader input;
        private INChIContentProcessorTool inchiTool;

        /// <summary> Construct a INChI reader from a Reader object.
        /// 
        /// </summary>
        /// <param name="input">the Reader with the content
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public INChIPlainTextReader(System.IO.StreamReader input)
        {
            this.init();
            setReader(input);
            inchiTool = new INChIContentProcessorTool();
        }

        public INChIPlainTextReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public INChIPlainTextReader()
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

        /// <summary> Initializes this reader.</summary>
        private void init()
        {
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

        /// <summary> Reads a IChemObject of type object from input.
        /// Supported types are: ChemFile.
        /// 
        /// </summary>
        /// <param name="object">type of requested IChemObject
        /// </param>
        /// <returns> the content in a ChemFile object
        /// </returns>
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                return (IChemObject)readChemFile((IChemFile)object_Renamed);
            }
            else
            {
                throw new CDKException("Only supported is reading of ChemFile objects.");
            }
        }

        // private functions

        /// <summary> Reads a ChemFile object from input.
        /// 
        /// </summary>
        /// <returns> ChemFile with the content read from the input
        /// </returns>
        private IChemFile readChemFile(IChemFile cf)
        {
            // have to do stuff here
            try
            {
                System.String line = input.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith("INChI="))
                    {
                        // ok, the fun starts
                        cf = cf.Builder.newChemFile();
                        // ok, we need to parse things like:
                        // INChI=1.12Beta/C6H6/c1-2-4-6-5-3-1/h1-6H
                        //UPGRADE_NOTE: Final was removed from the declaration of 'INChI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                        System.String INChI = line.Substring(6);
                        SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(INChI, "/");
                        // ok, we expect 4 tokens
                        //UPGRADE_NOTE: Final was removed from the declaration of 'version '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                        System.String version = tokenizer.NextToken(); // 1.12Beta
                        //UPGRADE_NOTE: Final was removed from the declaration of 'formula '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                        System.String formula = tokenizer.NextToken(); // C6H6
                        //UPGRADE_NOTE: Final was removed from the declaration of 'connections '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                        System.String connections = tokenizer.NextToken().Substring(1); // 1-2-4-6-5-3-1
                        //UPGRADE_NOTE: Final was removed from the declaration of 'hydrogens '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                        System.String hydrogens = tokenizer.NextToken().Substring(1); // 1-6H

                        IAtomContainer parsedContent = inchiTool.processFormula(cf.Builder.newAtomContainer(), formula);
                        inchiTool.processConnections(connections, parsedContent, -1);

                        ISetOfMolecules moleculeSet = cf.Builder.newSetOfMolecules();
                        moleculeSet.addMolecule(cf.Builder.newMolecule(parsedContent));
                        IChemModel model = cf.Builder.newChemModel();
                        model.SetOfMolecules = moleculeSet;
                        IChemSequence sequence = cf.Builder.newChemSequence();
                        sequence.addChemModel(model);
                        cf.addChemSequence(sequence);
                    }
                    line = input.ReadLine();
                }
            }
            catch (System.Exception exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                throw new CDKException("Error while reading INChI file: " + exception.Message, exception);
            }
            return cf;
        }

        public override void close()
        {
            input.Close();
        }
    }
}