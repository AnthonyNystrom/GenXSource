/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2003-2006  The Jmol Development Team (v. 1.1.2.2)
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
using System.IO;
using javax.vecmath;
using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.Math;
using Support;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Read output files generated with the VASP software.
    /// 
    /// </summary>
    /// <cdk.module>  experimental </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Fabian Dortu <Fabian.Dortu@wanadoo.be>
    /// </author>
    public class VASPReader : DefaultChemObjectReader
    {
        private void InitBlock()
        {
            for (int i = 0; i < 3; i++)
            {
                rprim[i] = new double[3];
            }
        }
        override public IResourceFormat Format
        {
            get
            {
                return new VASPFormat();
            }

        }

        //private LoggingTool //logger = null;

        // This variable is used to parse the input file
        protected internal Support.SupportClass.Tokenizer st = new Support.SupportClass.Tokenizer("", "");
        protected internal System.String fieldVal;
        protected internal int repVal = 0;

        protected internal System.IO.StreamReader inputBuffer;

        // VASP VARIABLES
        internal int natom = 1;
        internal int ntype = 1;
        internal double[] acell = new double[3];
        internal double[][] rprim = new double[3][];
        internal System.String info = "";
        internal System.String line;
        internal System.String[] anames; //size is ntype. Contains the names of the atoms
        internal int[] natom_type; //size is natom. Contain the atomic number
        internal System.String representation; // "Direct" only so far

        /// <summary> Creates a new <code>VASPReader</code> instance.
        /// 
        /// </summary>
        /// <param name="input">a <code>Reader</code> value
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public VASPReader(System.IO.StreamReader input)
        {
            InitBlock();
            //logger = new LoggingTool(this);
            if (input is System.IO.StreamReader)
            {
                this.inputBuffer = (System.IO.StreamReader)input;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this.inputBuffer = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
        }

        public VASPReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public VASPReader()
            : this((StreamReader)null)
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader input)
        {
            if (input is System.IO.StreamReader)
            {
                this.inputBuffer = (System.IO.StreamReader)input;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this.inputBuffer = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
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

        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                IChemFile cf = (IChemFile)object_Renamed;
                try
                {
                    cf = readChemFile(cf);
                }
                catch (System.IO.IOException exception)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.String error = "Input/Output error while reading from input: " + exception.Message;
                    //logger.error(error);
                    //logger.debug(exception);
                    throw new CDKException(error, exception);
                }
                return cf;
            }
            else
            {
                throw new CDKException("Only supported is reading of ChemFile.");
            }
        }

        private IChemFile readChemFile(IChemFile file)
        {
            IChemSequence seq = readChemSequence(file.Builder.newChemSequence());
            file.addChemSequence(seq);
            return file;
        }

        private IChemSequence readChemSequence(IChemSequence sequence)
        {
            IChemModel chemModel = sequence.Builder.newChemModel();
            ICrystal crystal = null;

            // Get the info line (first token of the first line)
            //UPGRADE_ISSUE: Method 'java.io.BufferedReader.mark' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReadermark_int'"
            //inputBuffer.mark(255);
            //long pos = inputBuffer.BaseStream.Position;
            info = nextVASPToken(false);
            //System.out.println(info);
            //UPGRADE_ISSUE: Method 'java.io.BufferedReader.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderreset'"
            //inputBuffer.reset();

            // Get the number of different atom "NCLASS=X"
            //UPGRADE_ISSUE: Method 'java.io.BufferedReader.mark' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReadermark_int'"
            //inputBuffer.mark(255);
            nextVASPTokenFollowing("NCLASS");
            ntype = System.Int32.Parse(fieldVal);
            //System.out.println("NCLASS= " + ntype);
            //UPGRADE_ISSUE: Method 'java.io.BufferedReader.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderreset'"
            //inputBuffer.reset();

            // Get the different atom names
            anames = new System.String[ntype];

            nextVASPTokenFollowing("ATOM");
            for (int i = 0; i < ntype; i++)
            {
                anames[i] = fieldVal;
                nextVASPToken(false);
            }

            // Get the number of atom of each type
            int[] natom_type = new int[ntype];
            natom = 0;
            for (int i = 0; i < ntype; i++)
            {
                natom_type[i] = System.Int32.Parse(fieldVal);
                nextVASPToken(false);
                natom = natom + natom_type[i];
            }

            // Get the representation type of the primitive vectors
            // only "Direct" is recognize now.
            representation = fieldVal;
            if (representation.Equals("Direct"))
            {
                //logger.info("Direct representation");
                // DO NOTHING
            }
            else
            {
                throw new CDKException("This VASP file is not supported. Please contact the Jmol developpers");
            }

            while (nextVASPToken(false) != null)
            {

                //logger.debug("New crystal started...");

                crystal = sequence.Builder.newCrystal();
                chemModel = sequence.Builder.newChemModel();

                // Get acell
                for (int i = 0; i < 3; i++)
                {
                    acell[i] = FortranFormat.atof(fieldVal); // all the same FIX?
                }

                // Get primitive vectors
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        nextVASPToken(false);
                        rprim[i][j] = FortranFormat.atof(fieldVal);
                    }
                }

                // Get atomic position
                int[] atomType = new int[natom];
                double[][] xred = new double[natom][];
                for (int i2 = 0; i2 < natom; i2++)
                {
                    xred[i2] = new double[3];
                }
                int atomIndex = 0;

                for (int i = 0; i < ntype; i++)
                {
                    for (int j = 0; j < natom_type[i]; j++)
                    {
                        try
                        {
                            atomType[atomIndex] = IsotopeFactory.getInstance(sequence.Builder).getElement(anames[i]).AtomicNumber;
                        }
                        catch (System.Exception exception)
                        {
                            throw new CDKException("Could not determine atomic number!", exception);
                        }
                        //logger.debug("aname: " + anames[i]);
                        //logger.debug("atomType: " + atomType[atomIndex]);

                        nextVASPToken(false);
                        xred[atomIndex][0] = FortranFormat.atof(fieldVal);
                        nextVASPToken(false);
                        xred[atomIndex][1] = FortranFormat.atof(fieldVal);
                        nextVASPToken(false);
                        xred[atomIndex][2] = FortranFormat.atof(fieldVal);

                        atomIndex = atomIndex + 1;
                        // FIXME: store atom
                    }
                }

                crystal.A = new Vector3d(rprim[0][0] * acell[0], rprim[0][1] * acell[0], rprim[0][2] * acell[0]);
                crystal.B = new Vector3d(rprim[1][0] * acell[1], rprim[1][1] * acell[1], rprim[1][2] * acell[1]);
                crystal.C = new Vector3d(rprim[2][0] * acell[2], rprim[2][1] * acell[2], rprim[2][2] * acell[2]);
                for (int i = 0; i < atomType.Length; i++)
                {
                    System.String symbol = "Du";
                    try
                    {
                        symbol = IsotopeFactory.getInstance(sequence.Builder).getElement(atomType[i]).Symbol;
                    }
                    catch (System.Exception exception)
                    {
                        throw new CDKException("Could not determine element symbol!", exception);
                    }
                    IAtom atom = sequence.Builder.newAtom(symbol);
                    atom.AtomicNumber = atomType[i];
                    // convert fractional to cartesian
                    double[] frac = new double[3];
                    frac[0] = xred[i][0];
                    frac[1] = xred[i][1];
                    frac[2] = xred[i][2];
                    atom.setFractionalPoint3d(new Point3d(frac[0], frac[1], frac[2]));
                    crystal.addAtom(atom);
                }
                crystal.setProperty(CDKConstants.REMARK, info);
                chemModel.Crystal = crystal;

                //logger.info("New Frame set!");

                sequence.addChemModel(chemModel);
            } //end while

            return sequence;
        }



        /// <summary> Find the next token of an VASP file.
        /// ABINIT tokens are words separated by space(s). Characters
        /// following a "#" are ignored till the end of the line.
        /// 
        /// </summary>
        /// <returns> a <code>String</code> value
        /// </returns>
        /// <exception cref="IOException">if an error occurs
        /// </exception>
        public virtual System.String nextVASPToken(bool newLine)
        {

            System.String line;

            if (newLine)
            {
                // We ignore the end of the line and go to the following line
                if (inputBuffer.Peek() != -1)
                {
                    line = inputBuffer.ReadLine();
                    st = new SupportClass.Tokenizer(line, " =\t");
                }
            }

            while (!st.HasMoreTokens() && inputBuffer.Peek() != -1)
            {
                line = inputBuffer.ReadLine();
                st = new SupportClass.Tokenizer(line, " =\t");
            }
            if (st.HasMoreTokens())
            {
                fieldVal = st.NextToken();
                if (fieldVal.StartsWith("#"))
                {
                    nextVASPToken(true);
                }
            }
            else
            {
                fieldVal = null;
            }
            return this.fieldVal;
        } //end nextVASPToken(boolean newLine)


        /// <summary> Find the next token of a VASP file begining
        /// with the *next* line.
        /// </summary>
        public virtual System.String nextVASPTokenFollowing(System.String string_Renamed)
        {
            int index;
            System.String line;
            while (inputBuffer.Peek() != -1)
            {
                line = inputBuffer.ReadLine();
                index = line.IndexOf(string_Renamed);
                if (index > 0)
                {
                    index = index + string_Renamed.Length;
                    line = line.Substring(index);
                    st = new SupportClass.Tokenizer(line, " =\t");
                    while (!st.HasMoreTokens() && inputBuffer.Peek() != -1)
                    {
                        line = inputBuffer.ReadLine();
                        st = new SupportClass.Tokenizer(line, " =\t");
                    }
                    if (st.HasMoreTokens())
                    {
                        fieldVal = st.NextToken();
                    }
                    else
                    {
                        fieldVal = null;
                    }
                    break;
                }
            }
            return fieldVal;
        } //end nextVASPTokenFollowing(String string) 

        public override void close()
        {
            inputBuffer.Close();
        }
    }
}