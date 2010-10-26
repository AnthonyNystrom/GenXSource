/* $RCSfile$
* $Author: kaihartmann $ 
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
* 
* Copyright (C) 2005-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.Interfaces;
using System.IO;
using Org.OpenScience.CDK.Exception;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> An output Writer that writes molecular data into the
    /// <a href="http://www.tripos.com/data/support/mol2.pdf">Tripos Mol2 format</a>.
    /// Writes the atoms and the bonds only at this moment.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Egon Willighagen
    /// </author>
    public class Mol2Writer : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new Mol2Format();
            }

        }

        internal static System.IO.StreamWriter writer;
        //private LoggingTool //logger;

        public Mol2Writer()
            : this((StreamWriter)null)
        {
        }

        /// <summary> Constructs a new Mol2 writer.</summary>
        /// <param name="out">the stream to write the Mol2 file to.
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public Mol2Writer(System.IO.StreamWriter out_Renamed)
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

        public Mol2Writer(System.IO.Stream output)
            : this(new System.IO.StreamWriter(output, System.Text.Encoding.Default))
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
            }
            return false;
        }

        public override void write(IChemObject object_Renamed)
        {
            if (object_Renamed is IMolecule)
            {
                try
                {
                    writeMolecule((IMolecule)object_Renamed);
                }
                catch (System.Exception ex)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    throw new CDKException("Error while writing Mol2 file: " + ex.Message, ex);
                }
            }
            else
            {
                throw new CDKException("Mol2Writer only supports output of Molecule classes.");
            }
        }

        /// <summary> Writes a single frame in XYZ format to the Writer.
        /// 
        /// </summary>
        /// <param name="mol">the Molecule to write
        /// </param>
        public virtual void writeMolecule(IMolecule mol)
        {

            try
            {

                /*
                #        Name: benzene 
                #        Creating user name: tom 
                #        Creation time: Wed Dec 28 00:18:30 1988 
				
                #        Modifying user name: tom 
                #        Modification time: Wed Dec 28 00:18:30 1988*/

                //logger.debug("Writing header...");
                if (mol.getProperty(CDKConstants.TITLE) != null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    writer.Write("#        Name: " + mol.getProperty(CDKConstants.TITLE) + "\n");
                }
                // FIXME: add other types of meta data
                //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                writer.WriteLine();

                /*
                @<TRIPOS>MOLECULE 
                benzene 
                12 12 1  0       0 
                SMALL 
                NO_CHARGES */

                //logger.debug("Writing molecule block...");
                writer.Write("@<TRIPOS>MOLECULE\n");
                writer.Write(mol.ID + "\n");
                writer.Write(mol.AtomCount + " " + mol.getBondCount() + "\n"); // that's the minimum amount of info required the format
                writer.Write("SMALL\n"); // no biopolymer
                writer.Write("NO CHARGES\n"); // other options include Gasteiger charges

                /*
                @<TRIPOS>ATOM 
                1       C1      1.207   2.091   0.000   C.ar    1       BENZENE 0.000 
                2       C2      2.414   1.394   0.000   C.ar    1       BENZENE 0.000 
                3       C3      2.414   0.000   0.000   C.ar    1       BENZENE 0.000 
                4       C4      1.207   -0.697  0.000   C.ar    1       BENZENE 0.000 
                5       C5      0.000   0.000   0.000   C.ar    1       BENZENE 0.000 
                6       C6      0.000   1.394   0.000   C.ar    1       BENZENE 0.000 
                7       H1      1.207   3.175   0.000   H       1       BENZENE 0.000 
                8       H2      3.353   1.936   0.000   H       1       BENZENE 0.000 
                9       H3      3.353   -0.542  0.000   H       1       BENZENE 0.000 
                10      H4      1.207   -1.781  0.000   H       1       BENZENE 0.000 
                11      H5      -0.939  -0.542  0.000   H       1       BENZENE 0.000 
                12      H6      -0.939  1.936   0.000   H       1       BENZENE 0.000 */

                // write atom block
                //logger.debug("Writing atom block...");
                writer.Write("@<TRIPOS>ATOM\n");
                IAtom[] atoms = mol.Atoms;
                for (int i = 0; i < atoms.Length; i++)
                {
                    writer.Write(i + " " + atoms[i].ID + " ");
                    if (atoms[i].getPoint3d() != null)
                    {
                        writer.Write(atoms[i].X3d + " ");
                        writer.Write(atoms[i].Y3d + " ");
                        writer.Write(atoms[i].Z3d + " ");
                    }
                    else if (atoms[i].getPoint2d() != null)
                    {
                        writer.Write(atoms[i].X2d + " ");
                        writer.Write(atoms[i].Y2d + " ");
                        writer.Write(" 0.000 ");
                    }
                    else
                    {
                        writer.Write("0.000 0.000 0.000 ");
                    }
                    writer.Write(atoms[i].Symbol + "\n"); // FIXME: should use perceived Mol2 Atom Types!
                }

                /*
                @<TRIPOS>BOND 
                1       1       2       ar 
                2       1       6       ar 
                3       2       3       ar 
                4       3       4       ar 
                5       4       5       ar 
                6       5       6       ar 
                7       1       7       1 
                8       2       8       1 
                9       3       9       1 
                10      4       10      1 
                11      5       11      1 
                12      6       12      1*/

                // write bond block
                //logger.debug("Writing bond block...");
                writer.Write("@<TRIPOS>BOND\n");
                IBond[] bonds = mol.Bonds;
                for (int i = 0; i < bonds.Length; i++)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    writer.Write(i + " " + mol.getAtomNumber(bonds[i].getAtomAt(0)) + " " + mol.getAtomNumber(bonds[i].getAtomAt(1)) + " " + ((int)bonds[i].Order) + "\n");
                }
            }
            catch (System.IO.IOException e)
            {
                throw e;
            }
        }
    }
}