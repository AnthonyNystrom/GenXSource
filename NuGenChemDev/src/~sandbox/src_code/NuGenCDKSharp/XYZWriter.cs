/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-07-14 11:39:20 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6669 $
* 
* Copyright (C) 2002  The Jmol Development Team
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Bradley A. Smith <bradley@baysmith.com>
    /// </author>
    /// <author>   J. Daniel Gezelter
    /// </author>
    /// <author>   Egon Willighagen
    /// </author>
    public class XYZWriter : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new XYZFormat();
            }

        }

        internal static System.IO.StreamWriter writer;
        //private LoggingTool //logger;

        /// <summary> Constructor.
        /// 
        /// </summary>
        /// <param name="out">the stream to write the XYZ file to.
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public XYZWriter(System.IO.StreamWriter out_Renamed)
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

        public XYZWriter(System.IO.Stream output)
            : this(new System.IO.StreamWriter(output, System.Text.Encoding.Default))
        {
        }

        public XYZWriter()
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
                    throw new CDKException("Error while writing XYZ file: " + ex.Message, ex);
                }
            }
            else
            {
                throw new CDKException("XYZWriter only supports output of Molecule classes.");
            }
        }

        /// <summary> writes a single frame in XYZ format to the Writer.</summary>
        /// <param name="mol">the Molecule to write
        /// </param>
        public virtual void writeMolecule(IMolecule mol)
        {

            System.String st = "";
            bool writecharge = true;

            try
            {

                System.String s1 = ((System.Int32)mol.AtomCount).ToString();
                //UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.BufferedWriter.write' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
                writer.Write(s1.ToCharArray(), 0, s1.Length);
                //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                writer.WriteLine();

                System.String s2 = null; // FIXME: add some interesting comment
                if (s2 != null)
                {
                    //UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.BufferedWriter.write' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
                    writer.Write(s2.ToCharArray(), 0, s2.Length);
                }
                //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                writer.WriteLine();

                // Loop through the atoms and write them out:
                IAtom[] atoms = mol.Atoms;
                for (int i = 0; i < atoms.Length; i++)
                {

                    IAtom a = atoms[i];
                    st = a.Symbol;

                    Point3d p3 = a.getPoint3d();
                    if (p3 != null)
                    {
                        st = st + "\t" + p3.x.ToString() + "\t" + p3.y.ToString() + "\t" + p3.z.ToString();
                    }

                    if (writecharge)
                    {
                        double ct = a.getCharge();
                        st = st + "\t" + ct;
                    }

                    //UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.BufferedWriter.write' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
                    writer.Write(st.ToCharArray(), 0, st.Length);
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();
                }
            }
            catch (System.IO.IOException e)
            {
                //            throw e;
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Error while writing file: ", e.Message);
                //logger.debug(e);
            }
        }
    }
}