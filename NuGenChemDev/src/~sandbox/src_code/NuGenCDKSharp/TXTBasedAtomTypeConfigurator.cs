/* $RCSfile$    
* $Author: egonw $    
* $Date: 2006-05-02 11:17:35 +0200 (Tue, 02 May 2006) $    
* $Revision: 6123 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-developers@lists.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using System.Reflection;
using Support;

namespace Org.OpenScience.CDK.Config
{
    /// <summary> AtomType list configurator that uses the AtomTypes originally
    /// defined in Jmol v5. This class was added to be able to port
    /// Jmol to CDK. The AtomType's themselves seems have a computational
    /// background, but this is not clear. 
    /// 
    /// </summary>
    /// <cdk.module>  core </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      Bradley A. Smith <bradley@baysmith.com>
    /// 
    /// </author>
    /// <cdk.keyword>     atom, type </cdk.keyword>
    public class TXTBasedAtomTypeConfigurator : IAtomTypeConfigurator
    {
        /// <summary> Sets the file containing the config data.</summary>
        virtual public System.IO.Stream InputStream
        {
            set
            {
                this.ins = value;
            }

        }

        private System.String configFile = "jmol_atomtypes.txt";
        private System.IO.Stream ins = null;

        public TXTBasedAtomTypeConfigurator()
        {
        }

        /// <summary> Reads a text based configuration file.
        /// 
        /// </summary>
        /// <param name="builder">IChemObjectBuilder used to construct the IAtomType's.
        /// </param>
        /// <throws>         IOException when a problem occured with reading from the InputStream </throws>
        /// <returns>        A Vector with read IAtomType's.
        /// </returns>
        public virtual System.Collections.ArrayList readAtomTypes(IChemObjectBuilder builder)
        {
            System.Collections.ArrayList atomTypes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

            if (ins == null)
            {
                // trying the default
                //System.out.println("readAtomTypes getResourceAsStream:"
                //                   + configFile);
                //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                ins = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + configFile);
            }
            if (ins == null)
                throw new System.IO.IOException("There was a problem getting the default stream: " + configFile);

            // read the contents from file
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            //UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.StreamReader(ins, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(ins, System.Text.Encoding.Default).CurrentEncoding, false, 1024);
            SupportClass.Tokenizer tokenizer;
            System.String string_Renamed;

            while (true)
            {
                string_Renamed = reader.ReadLine();
                if (string_Renamed == null)
                {
                    break;
                }
                if (!string_Renamed.StartsWith("#"))
                {
                    System.String name = "";
                    System.String rootType = "";
                    int atomicNumber = 0, colorR = 0, colorG = 0, colorB = 0;
                    double mass = 0.0, vdwaals = 0.0, covalent = 0.0;
                    tokenizer = new SupportClass.Tokenizer(string_Renamed, "\t ,;");
                    int tokenCount = tokenizer.Count;

                    if (tokenCount == 9)
                    {
                        name = tokenizer.NextToken();
                        rootType = tokenizer.NextToken();
                        System.String san = tokenizer.NextToken();
                        System.String sam = tokenizer.NextToken();
                        System.String svdwaals = tokenizer.NextToken();
                        System.String scovalent = tokenizer.NextToken();
                        System.String sColorR = tokenizer.NextToken();
                        System.String sColorG = tokenizer.NextToken();
                        System.String sColorB = tokenizer.NextToken();

                        try
                        {
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            mass = System.Double.Parse(sam);
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            vdwaals = System.Double.Parse(svdwaals);
                            //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                            covalent = System.Double.Parse(scovalent);
                            atomicNumber = System.Int32.Parse(san);
                            colorR = System.Int32.Parse(sColorR);
                            colorG = System.Int32.Parse(sColorG);
                            colorB = System.Int32.Parse(sColorB);
                        }
                        catch (System.FormatException nfe)
                        {
                            throw new System.IO.IOException("AtomTypeTable.ReadAtypes: " + "Malformed Number");
                        }

                        IAtomType atomType = builder.newAtomType(name, rootType);
                        atomType.AtomicNumber = atomicNumber;
                        atomType.setExactMass(mass);
                        atomType.VanderwaalsRadius = vdwaals;
                        atomType.CovalentRadius = covalent;
                        System.Drawing.Color color = System.Drawing.Color.FromArgb(colorR, colorG, colorB);
                        atomType.setProperty("org.openscience.cdk.renderer.color", color);
                        atomTypes.Add(atomType);
                    }
                    else
                    {
                        throw new System.IO.IOException("AtomTypeTable.ReadAtypes: " + "Wrong Number of fields");
                    }
                }
            } // end while
            ins.Close();

            return atomTypes;
        }
    }
}