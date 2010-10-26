/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
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
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Support;
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads an object from HIN formated input.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Rajarshi Guha <rajarshi@presidency.com>
    /// </author>
    /// <cdk.created>  2004-01-27 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format, HIN  </cdk.keyword>
    public class HINReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new HINFormat();
            }

        }

        private System.IO.StreamReader input;

        /// <summary> Construct a new reader from a Reader type object
        /// 
        /// </summary>
        /// <param name="input">reader from which input is read
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public HINReader(System.IO.StreamReader input)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            this.input = input;
        }

        public HINReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public HINReader()
            : this((StreamReader)null)
        {
        }

        public override void close()
        {
            input.Close();
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
            if (typeof(IChemFile).Equals(classObject))
                return true;
            return false;
        }

        /// <summary> Reads the content from a HIN input. It can only return a
        /// IChemObject of type ChemFile
        /// 
        /// </summary>
        /// <param name="object">class must be of type ChemFile
        /// 
        /// </param>
        /// <seealso cref="org.openscience.cdk.ChemFile">
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

        private System.String getMolName(System.String line)
        {
            if (line == null)
                return ("");
            SupportClass.Tokenizer st = new SupportClass.Tokenizer(line, " ");
            int ntok = st.Count;
            System.String[] toks = new System.String[ntok];
            for (int j = 0; j < ntok; j++)
            {
                toks[j] = st.NextToken();
            }
            if (toks.Length == 3)
                return (toks[2]);
            else
                return ("");
        }

        /// <summary>  Private method that actually parses the input to read a ChemFile
        /// object. In its current state it is able to read all the molecules
        /// (if more than one is present) in the specified HIN file. These are
        /// placed in a SetOfMolecules object which in turn is placed in a ChemModel
        /// which in turn is placed in a ChemSequence object and which is finally 
        /// placed in a ChemFile object and returned to the user.
        /// 
        /// </summary>
        /// <returns> A ChemFile containing the data parsed from input.
        /// </returns>
        private IChemFile readChemFile(IChemFile file)
        {
            IChemSequence chemSequence = file.Builder.newChemSequence();
            IChemModel chemModel = file.Builder.newChemModel();
            ISetOfMolecules setOfMolecules = file.Builder.newSetOfMolecules();
            System.String info;

            SupportClass.Tokenizer tokenizer;

            try
            {
                System.String line;

                // read in header info
                while (true)
                {
                    line = input.ReadLine();
                    if (line.IndexOf("mol ") == 0)
                    {
                        info = getMolName(line);
                        break;
                    }
                }


                // start the actual molecule data - may be multiple molecule
                line = input.ReadLine();
                while (true)
                {
                    if (line == null)
                        break; // end of file
                    if (line.IndexOf(';') == 0)
                        continue; // comment line

                    if (line.IndexOf("mol ") == 0)
                    {
                        info = getMolName(line);
                        line = input.ReadLine();
                    }
                    IMolecule m = file.Builder.newMolecule();
                    m.setProperty(CDKConstants.TITLE, info);

                    // Each elemnt of cons is an ArrayList of length 3 which stores
                    // the start and end indices and bond order of each bond 
                    // found in the HIN file. Before adding bonds we need to reduce
                    // the number of bonds so as not to count the same bond twice
                    System.Collections.ArrayList cons = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                    // read data for current molecule
                    int atomSerial = 0;
                    while (true)
                    {
                        if (line.IndexOf("endmol ") >= 0)
                        {
                            break;
                        }
                        if (line.IndexOf(';') == 0)
                            continue; // comment line

                        tokenizer = new SupportClass.Tokenizer(line, " ");

                        int ntoken = tokenizer.Count;
                        System.String[] toks = new System.String[ntoken];
                        for (int i = 0; i < ntoken; i++)
                            toks[i] = tokenizer.NextToken();

                        System.String sym = new System.Text.StringBuilder(toks[3]).ToString();
                        double charge = System.Double.Parse(toks[6]);
                        double x = System.Double.Parse(toks[7]);
                        double y = System.Double.Parse(toks[8]);
                        double z = System.Double.Parse(toks[9]);
                        int nbond = System.Int32.Parse(toks[10]);

                        IAtom atom = file.Builder.newAtom(sym, new Point3d(x, y, z));
                        atom.setCharge(charge);

                        for (int j = 11; j < (11 + nbond * 2); j += 2)
                        {
                            double bo = 1;
                            int s = System.Int32.Parse(toks[j]) - 1; // since atoms start from 1 in the file
                            char bt = toks[j + 1][0];
                            switch (bt)
                            {

                                case 's':
                                    bo = 1;
                                    break;

                                case 'd':
                                    bo = 2;
                                    break;

                                case 't':
                                    bo = 3;
                                    break;

                                case 'a':
                                    bo = 1.5;
                                    break;
                            }
                            System.Collections.ArrayList ar = new System.Collections.ArrayList(3);
                            ar.Add((System.Int32)atomSerial);
                            ar.Add((System.Int32)s);
                            ar.Add((double)bo);
                            cons.Add(ar);
                        }
                        m.addAtom(atom);
                        atomSerial++;
                        line = input.ReadLine();
                    }

                    // before storing the molecule lets include the connections
                    // First we reduce the number of bonds stored, since we have
                    // stored both, say, C1-H1 and H1-C1.
                    System.Collections.ArrayList blist = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                    for (int i = 0; i < cons.Count; i++)
                    {
                        System.Collections.ArrayList ar = (System.Collections.ArrayList)cons[i];

                        // make a reversed list
                        System.Collections.ArrayList arev = new System.Collections.ArrayList(3);
                        arev.Add(ar[1]);
                        arev.Add(ar[0]);
                        arev.Add(ar[2]);

                        // Now see if ar or arev are already in blist
                        if (blist.Contains(ar) || blist.Contains(arev))
                            continue;
                        else
                            blist.Add(ar);
                    }

                    // now just store all the bonds we have
                    for (int i = 0; i < blist.Count; i++)
                    {
                        System.Collections.ArrayList ar = (System.Collections.ArrayList)blist[i];
                        int s = ((System.Int32)ar[0]);
                        int e = ((System.Int32)ar[1]);
                        double bo = ((System.Double)ar[2]);
                        m.addBond(s, e, bo);
                    }

                    setOfMolecules.addMolecule(m);
                    line = input.ReadLine(); // read in the 'mol N'
                }

                // got all the molecule in the HIN file (hopefully!)
                chemModel.SetOfMolecules = setOfMolecules;
                chemSequence.addChemModel(chemModel);
                file.addChemSequence(chemSequence);
            }
            catch (System.IO.IOException e)
            {
                // should make some noise now
                file = null;
            }
            return file;
        }
    }
}