/* $RCSfile$ 
* $Author: kaihartmann $ 
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
*
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
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
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.Interfaces;
using javax.vecmath;
using System.IO;
using Org.OpenScience.CDK.Exception;

namespace Org.OpenScience.CDK.IO
{
    //import org.openscience.cdk.tools.LoggingTool;

    /// <summary> Rather stupid file format used for storing crystal information.
    /// 
    /// </summary>
    /// <author>  Egon Willighagen
    /// </author>
    /// <cdk.created>  2004-01-01 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.module>  extra </cdk.module>
    public class CrystClustWriter : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new CrystClustFormat();
            }
        }

        internal static System.IO.StreamWriter writer;
        //private LoggingTool logger;

        /// <summary> Constructs a new CrystClustWriter class. Output will be stored in the Writer
        /// class given as parameter.
        /// 
        /// </summary>
        /// <param name="out">Writer to redirect the output to.
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public CrystClustWriter(System.IO.StreamWriter out_Renamed)
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

        public CrystClustWriter(System.IO.Stream output)
            : this(new System.IO.StreamWriter(output, System.Text.Encoding.Default))
        {
        }

        public CrystClustWriter()
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


        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(ICrystal).Equals(interfaces[i]))
                    return true;
                if (typeof(IChemSequence).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Serializes the IChemObject to CrystClust format and redirects it to the output Writer.
        /// 
        /// </summary>
        /// <param name="object">A Molecule of SetOfMolecules object
        /// </param>
        public override void write(IChemObject object_Renamed)
        {
            if (object_Renamed is ICrystal)
            {
                write((ICrystal)object_Renamed);
            }
            else if (object_Renamed is IChemSequence)
            {
                write((IChemSequence)object_Renamed);
            }
            else
            {
                throw new UnsupportedChemObjectException("This object type is not supported.");
            }
        }


        /// <summary> Flushes the output and closes this object</summary>
        public override void close()
        {
            writer.Close();
        }

        // Private procedures

        //UPGRADE_NOTE: Access modifiers of method 'write' were changed to 'public'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1204'"
        public void write(IChemSequence cs)
        {
            int count = cs.ChemModelCount;
            for (int i = 0; i < count; i++)
            {
                write("frame: " + (i + 1) + "\n");
                write(cs.getChemModel(i).Crystal);
            }
        }

        /// <summary> Writes a single frame to the Writer.
        /// 
        /// <p>Format:
        /// <pre>
        /// line      data
        /// -------    --------------------------
        /// 1       spacegroup
        /// 2,3,4     cell parameter: a
        /// 5,6,7                     b
        /// 8,9,10                    c
        /// 11       number of atoms
        /// 12       number of asym. units
        /// 13-16     atomtype: charge, atomcoord x, y, z
        /// 17-20     idem second atom
        /// 21-24     idem third atom etc
        /// </pre>
        /// 
        /// </summary>
        /// <param name="crystal">the Crystal to serialize
        /// </param>
        //UPGRADE_NOTE: Access modifiers of method 'write' were changed to 'public'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1204'"
        public void write(ICrystal crystal)
        {

            System.String sg = crystal.SpaceGroup;
            if ("P 2_1 2_1 2_1".Equals(sg))
            {
                write("P 21 21 21 (1)\n");
            }
            else
            {
                write("P 1 (1)\n");
            }

            // output unit cell axes
            writeVector3d(crystal.A);
            writeVector3d(crystal.B);
            writeVector3d(crystal.C);

            // output number of atoms
            int noatoms = crystal.AtomCount;
            write(((System.Int32)noatoms).ToString());
            write("\n");

            // output number of asym. units (Z)
            if (sg.Equals("P1"))
            {
                write("1\n");
            }
            else
            {
                // duno
                write("1\n");
            }

            // output atoms
            for (int i = 0; i < noatoms; i++)
            {
                // output atom sumbol
                IAtom atom = crystal.getAtomAt(i);
                write(atom.Symbol);
                write(":");
                // output atom charge
                write(((double)atom.getCharge()).ToString() + "\n");
                // output coordinates
                write(((double)atom.X3d).ToString() + "\n");
                write(((double)atom.Y3d).ToString() + "\n");
                write(((double)atom.Z3d).ToString() + "\n");
            }
        }

        private void write(System.String s)
        {
            try
            {
                writer.Write(s);
            }
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.Console.Error.WriteLine("CMLWriter IOException while printing \"" + s + "\":\n" + e.ToString());
            }
        }

        private void writeVector3d(Vector3d vector)
        {
            write(vector.x.ToString());
            write("\n");
            write(vector.y.ToString());
            write("\n");
            write(vector.z.ToString());
            write("\n");
        }
    }
}