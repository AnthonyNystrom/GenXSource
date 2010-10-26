/* $RCSfile$
* $Author: kaihartmann $ 
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
* 
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
*/
using System;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.IO.Formats;
using System.IO;
using Org.OpenScience.CDK.Tools;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Converts a Molecule into CDK source code that would build the same
    /// molecule. It's typical use is:
    /// <pre>
    /// StringWriter stringWriter = new StringWriter();
    /// ChemObjectWriter writer = new CDKSourceCodeWriter(stringWriter);
    /// writer.write((Molecule)molecule);
    /// writer.close();
    /// System.out.print(stringWriter.toString());
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    /// <cdk.created>  2003-10-01 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  file format, CDK source code </cdk.keyword>
    public class CDKSourceCodeWriter : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new CDKSourceCodeFormat();
            }

        }

        internal static System.IO.StreamWriter writer;
        //private LoggingTool //logger;

        /// <summary> Contructs a new CDKSourceCodeWriter.
        /// 
        /// </summary>
        /// <param name="out"> The Writer to write to
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public CDKSourceCodeWriter(System.IO.StreamWriter out_Renamed)
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

        public CDKSourceCodeWriter(System.IO.Stream out_Renamed)
            : this(new System.IO.StreamWriter(out_Renamed, System.Text.Encoding.Default))
        {
        }
        public CDKSourceCodeWriter()
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
                    //logger.error(ex.Message);
                    //logger.debug(ex);
                }
            }
            else
            {
                throw new CDKException("Only supported is writing of Molecule objects.");
            }
        }

        public virtual void writeMolecule(IMolecule molecule)
        {
            writer.Write("{\n");
            writer.Write("  Molecule mol = new Molecule();\n");
            IDCreator idCreator = new IDCreator();
            idCreator.createIDs(molecule);
            IAtom[] atoms = molecule.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                IAtom atom = atoms[i];
                writeAtom(atom);
                writer.Write("  mol.addAtom(" + atom.ID + ");\n");
            }
            IBond[] bonds = molecule.Bonds;
            for (int i = 0; i < bonds.Length; i++)
            {
                IBond bond = bonds[i];
                writeBond(bond);
                writer.Write("  mol.addBond(" + bond.ID + ");\n");
            }
            writer.Write("}\n");
        }

        public virtual void writeAtom(IAtom atom)
        {
            writer.Write("  Atom " + atom.ID + " = new Atom(\"" + atom.Symbol + "\");\n");
        }

        public virtual void writeBond(IBond bond)
        {
            writer.Write("  Bond " + bond.ID + " = new Bond(" + bond.getAtomAt(0).ID + ", " + bond.getAtomAt(1).ID + ", " + bond.Order + ");\n");
        }
    }
}