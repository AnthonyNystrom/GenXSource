/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-12 15:46:26 +0200 (Wed, 12 Jul 2006) $
* $Revision: 6641 $
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
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Support;
using javax.vecmath;
using Org.OpenScience.CDK.Geometry;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> It reads Z matrices like in Gaussian input files. It seems that it cannot
    /// handle Z matrices where values are given via a stringID for which the value
    /// is given later.
    /// 
    /// </summary>
    /// <cdk.module>  experimental </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format, Z-matrix </cdk.keyword>
    public class ZMatrixReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new ZMatrixFormat();
            }

        }

        private System.IO.StreamReader input;

        /// <summary> Constructs a ZMatrixReader from a Reader that contains the
        /// data to be parsed.
        /// 
        /// </summary>
        /// <param name="input">  Reader containing the data to read
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public ZMatrixReader(System.IO.StreamReader input)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
        }

        public ZMatrixReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public ZMatrixReader()
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

        /// <summary>  Returns a IChemObject of type object bye reading from
        /// the input. 
        /// 
        /// The function supports only reading of ChemFile's.
        /// 
        /// </summary>
        /// <param name="object"> IChemObject that types the class to return.
        /// </param>
        /// <throws>     Exception when a IChemObject is requested that cannot be read. </throws>
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
                return (IChemObject)readChemFile((IChemFile)object_Renamed);
            else
                throw new CDKException("Only ChemFile objects can be read.");
        }

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
                while (line.StartsWith("#"))
                    line = input.ReadLine();
                /*while (input.ready() && line != null) 
                {*/
                //        System.out.println("lauf");
                // parse frame by frame
                tokenizer = new SupportClass.Tokenizer(line, "\t ,;");

                System.String token = tokenizer.NextToken();
                number_of_atoms = System.Int32.Parse(token);
                System.String info = input.ReadLine();

                IChemModel chemModel = file.Builder.newChemModel();
                ISetOfMolecules setOfMolecules = file.Builder.newSetOfMolecules();

                IMolecule m = file.Builder.newMolecule();
                m.setProperty(CDKConstants.TITLE, info);

                System.String[] types = new System.String[number_of_atoms];
                double[] d = new double[number_of_atoms]; int[] d_atom = new int[number_of_atoms]; // Distances
                double[] a = new double[number_of_atoms]; int[] a_atom = new int[number_of_atoms]; // Angles
                double[] da = new double[number_of_atoms]; int[] da_atom = new int[number_of_atoms]; // Diederangles
                //Point3d[] pos = new Point3d[number_of_atoms]; // calculated positions

                int i = 0;
                while (i < number_of_atoms)
                {
                    line = input.ReadLine();
                    //          System.out.println("line:\""+line+"\"");
                    if (line == null)
                        break;
                    if (line.StartsWith("#"))
                    {
                        // skip comment in file
                    }
                    else
                    {
                        d[i] = 0d; d_atom[i] = -1;
                        a[i] = 0d; a_atom[i] = -1;
                        da[i] = 0d; da_atom[i] = -1;

                        tokenizer = new SupportClass.Tokenizer(line, "\t ,;");
                        int fields = tokenizer.Count;

                        if (fields < System.Math.Min(i * 2 + 1, 7))
                        {
                            // this is an error but cannot throw exception
                        }
                        else if (i == 0)
                        {
                            types[i] = tokenizer.NextToken();
                            i++;
                        }
                        else if (i == 1)
                        {
                            types[i] = tokenizer.NextToken();
                            d_atom[i] = (System.Int32.Parse(tokenizer.NextToken())) - 1;
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            d[i] = (System.Double.Parse(tokenizer.NextToken()));
                            i++;
                        }
                        else if (i == 2)
                        {
                            types[i] = tokenizer.NextToken();
                            d_atom[i] = (System.Int32.Parse(tokenizer.NextToken())) - 1;
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            d[i] = (System.Double.Parse(tokenizer.NextToken()));
                            a_atom[i] = (System.Int32.Parse(tokenizer.NextToken())) - 1;
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            a[i] = (System.Double.Parse(tokenizer.NextToken()));
                            i++;
                        }
                        else
                        {
                            types[i] = tokenizer.NextToken();
                            d_atom[i] = (System.Int32.Parse(tokenizer.NextToken())) - 1;
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            d[i] = (System.Double.Parse(tokenizer.NextToken()));
                            a_atom[i] = (System.Int32.Parse(tokenizer.NextToken())) - 1;
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            a[i] = (System.Double.Parse(tokenizer.NextToken()));
                            da_atom[i] = (System.Int32.Parse(tokenizer.NextToken())) - 1;
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            da[i] = (System.Double.Parse(tokenizer.NextToken()));
                            i++;
                        }
                    }
                }

                // calculate cartesian coordinates
                Point3d[] cartCoords = ZMatrixTools.zmatrixToCartesian(d, d_atom, a, a_atom, da, da_atom);

                for (i = 0; i < number_of_atoms; i++)
                {
                    m.addAtom(file.Builder.newAtom(types[i], cartCoords[i]));
                }

                //        System.out.println("molecule:\n"+m);

                setOfMolecules.addMolecule(m);
                chemModel.SetOfMolecules = setOfMolecules;
                chemSequence.addChemModel(chemModel);
                line = input.ReadLine();
                file.addChemSequence(chemSequence);
            }
            catch (System.IO.IOException e)
            {
                // should make some noise now
                file = null;
            }
            return file;
        }

        public override void close()
        {
            input.Close();
        }
    }
}