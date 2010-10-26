/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
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
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    //import org.openscience.cdk.tools.LoggingTool;

    /// <summary> Writer that outputs in the HIN format.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Rajarshi Guha <rajarshi@presidency.com>
    /// </author>
    /// <cdk.created>  2004-01-27 </cdk.created>
    public class HINWriter : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new HINFormat();
            }

        }

        internal static System.IO.StreamWriter writer;
        //private LoggingTool logger; 

        /// <summary> Constructor.</summary>
        /// <param name="out">the stream to write the HIN file to.
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public HINWriter(System.IO.StreamWriter out_Renamed)
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

        public HINWriter(System.IO.Stream output)
            : this(new System.IO.StreamWriter(output, System.Text.Encoding.Default))
        {
        }

        public HINWriter()
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
                if (typeof(IMolecule).Equals(interfaces[i]))
                    return true;
                if (typeof(ISetOfMolecules).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        public override void write(IChemObject object_Renamed)
        {
            if (object_Renamed is IMolecule)
            {
                try
                {
                    ISetOfMolecules som = object_Renamed.Builder.newSetOfMolecules();
                    som.addMolecule((IMolecule)object_Renamed);
                    writeMolecule(som);
                }
                catch (System.Exception ex)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    throw new CDKException("Error while writing HIN file: " + ex.Message, ex);
                }
            }
            else if (object_Renamed is ISetOfMolecules)
            {
                try
                {
                    writeMolecule((ISetOfMolecules)object_Renamed);
                }
                catch (System.IO.IOException ex)
                {
                    //
                }
            }
            else
            {
                throw new CDKException("HINWriter only supports output of Molecule or SetOfMolecule classes.");
            }
        }

        /// <summary> writes all the molecules supplied in a SetOfMolecules class to
        /// a single HIN file. You can also supply a single Molecule object
        /// as well
        /// </summary>
        /// <param name="mol">the Molecule to write
        /// </param>
        private void writeMolecule(ISetOfMolecules som)
        {

            //int na = 0;
            //String info = "";
            System.String sym = "";
            double chrg = 0.0;
            //boolean writecharge = true;

            for (int molnum = 0; molnum < som.MoleculeCount; molnum++)
            {

                IMolecule mol = som.getMolecule(molnum);

                try
                {

                    int natom = mol.AtomCount;
                    int nbond = mol.getBondCount();

                    System.String molname = "mol " + (molnum + 1) + " " + ((System.String)mol.getProperty(CDKConstants.TITLE));

                    //UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.BufferedWriter.write' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
                    writer.Write(molname.ToCharArray(), 0, molname.Length);
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();

                    // Loop through the atoms and write them out:
                    IAtom[] atoms = mol.Atoms;
                    IBond[] bonds = mol.Bonds;

                    for (int i = 0; i < natom; i++)
                    {

                        System.String line = "atom ";
                        IAtom a = atoms[i];

                        sym = a.Symbol;
                        chrg = a.getCharge();
                        Point3d p3 = a.getPoint3d();

                        line = line + ((System.Int32)(i + 1)).ToString() + " - " + sym + " ** - " + ((double)chrg).ToString() + " " + p3.x.ToString() + " " + p3.y.ToString() + " " + p3.z.ToString() + " ";

                        System.String buf = "";
                        int ncon = 0;
                        for (int j = 0; j < nbond; j++)
                        {
                            IBond b = bonds[j];
                            if (b.contains(a))
                            {
                                // current atom is in the bond so lets get the connected atom
                                IAtom ca = b.getConnectedAtom(a);
                                double bo = b.Order;
                                int serial = -1;
                                System.String bt = "";

                                // get the serial no for this atom
                                serial = mol.getAtomNumber(ca);

                                if (bo == 1)
                                    bt = new System.Text.StringBuilder("s").ToString();
                                else if (bo == 2)
                                    bt = new System.Text.StringBuilder("d").ToString();
                                else if (bo == 3)
                                    bt = new System.Text.StringBuilder("t").ToString();
                                else if (bo == 1.5)
                                    bt = new System.Text.StringBuilder("a").ToString();
                                buf = buf + ((System.Int32)(serial + 1)).ToString() + " " + bt + " ";
                                ncon++;
                            }
                        }
                        line = line + " " + ((System.Int32)ncon).ToString() + " " + buf;
                        //UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.BufferedWriter.write' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
                        writer.Write(line.ToCharArray(), 0, line.Length);
                        //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                        writer.WriteLine();
                    }
                    System.String buf2 = "endmol " + (molnum + 1);
                    //UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.BufferedWriter.write' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
                    writer.Write(buf2.ToCharArray(), 0, buf2.Length);
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();
                }
                catch (System.IO.IOException e)
                {
                    throw e;
                }
            }
        }
    }
}