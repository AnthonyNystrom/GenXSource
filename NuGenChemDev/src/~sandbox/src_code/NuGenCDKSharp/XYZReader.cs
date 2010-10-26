/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.Interfaces;
using System.IO;
using Org.OpenScience.CDK.Exception;
using Support;
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads an object from XYZ formated input.
    /// 
    /// <p>This class is based on Dan Gezelter's XYZReader from Jmol
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format, XYZ </cdk.keyword>
    public class XYZReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new XYZFormat();
            }

        }

        private System.IO.StreamReader input;
        //private LoggingTool //logger;

        /// <summary> Construct a new reader from a Reader type object.
        /// 
        /// </summary>
        /// <param name="input">reader from which input is read
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public XYZReader(System.IO.StreamReader input)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            //logger = new LoggingTool(this);
        }

        public XYZReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public XYZReader()
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
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> reads the content from a XYZ input. It can only return a
        /// IChemObject of type ChemFile
        /// 
        /// </summary>
        /// <param name="object">class must be of type ChemFile
        /// 
        /// </param>
        /// <seealso cref="IChemFile">
        /// </seealso>
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

        // private procedures

        /// <summary>  Private method that actually parses the input to read a ChemFile
        /// object.
        /// 
        /// </summary>
        /// <returns> A ChemFile containing the data parsed from input.
        /// </returns>
        private IChemFile readChemFile(IChemFile file)
        {
            IChemSequence chemSequence = file.Builder.newChemSequence();

            int number_of_atoms = 0;
            SupportClass.Tokenizer tokenizer;

            try
            {
                System.String line = input.ReadLine();
                while (input.Peek() != -1 && line != null)
                {
                    // parse frame by frame
                    tokenizer = new SupportClass.Tokenizer(line, "\t ,;");

                    System.String token = tokenizer.NextToken();
                    number_of_atoms = System.Int32.Parse(token);
                    System.String info = input.ReadLine();

                    IChemModel chemModel = file.Builder.newChemModel();
                    ISetOfMolecules setOfMolecules = file.Builder.newSetOfMolecules();

                    IMolecule m = file.Builder.newMolecule();
                    m.setProperty(CDKConstants.TITLE, info);

                    for (int i = 0; i < number_of_atoms; i++)
                    {
                        line = input.ReadLine();
                        if (line == null)
                            break;
                        if (line.StartsWith("#") && line.Length > 1)
                        {
                            System.Object comment = m.getProperty(CDKConstants.COMMENT);
                            if (comment == null)
                            {
                                comment = "";
                            }
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            comment = comment.ToString() + line.Substring(1).Trim();
                            m.setProperty(CDKConstants.COMMENT, comment);
                            //logger.debug("Found and set comment: ", comment);
                        }
                        else
                        {
                            double x = 0.0f, y = 0.0f, z = 0.0f;
                            double charge = 0.0f;
                            tokenizer = new SupportClass.Tokenizer(line, "\t ,;");
                            int fields = tokenizer.Count;

                            if (fields < 4)
                            {
                                // this is an error but cannot throw exception
                            }
                            else
                            {
                                System.String atomtype = tokenizer.NextToken();
                                //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                                x = (System.Double.Parse(tokenizer.NextToken()));
                                //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                                y = (System.Double.Parse(tokenizer.NextToken()));
                                //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                                z = (System.Double.Parse(tokenizer.NextToken()));

                                if (fields == 8)
                                {
                                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                                    charge = (System.Double.Parse(tokenizer.NextToken()));
                                }

                                IAtom atom = file.Builder.newAtom(atomtype, new Point3d(x, y, z));
                                atom.setCharge(charge);
                                m.addAtom(atom);
                            }
                        }
                    }

                    setOfMolecules.addMolecule(m);
                    chemModel.SetOfMolecules = setOfMolecules;
                    chemSequence.addChemModel(chemModel);
                    line = input.ReadLine();
                }
                file.addChemSequence(chemSequence);
            }
            catch (System.IO.IOException e)
            {
                // should make some noise now
                file = null;
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Error while reading file: ", e.Message);
                //logger.debug(e);
            }
            return file;
        }

        public override void close()
        {
            input.Close();
        }
    }
}