/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-07-14 11:39:20 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6669 $
* 
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
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
using Support;
using System.IO;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Tools.Manipulator;
using Org.OpenScience.CDK.Config;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Writes MDL mol files and SD files.
    /// <BR><BR>
    /// A MDL mol file contains a single molecule, whereas a MDL SD file contains
    /// one or more molecules. This class is capable of writing both mol files and
    /// SD files. The correct format is automatically chosen:
    /// <ul>
    /// <li>if {@link #write(IChemObject)} is called with a {@link org.openscience.cdk.SetOfMolecules SetOfMolecules}
    /// as an argument a SD files is written</li>
    /// <li>if one of the two writeMolecule methods (either {@link #writeMolecule(IMolecule) this one} or
    /// {@link #writeMolecule(IMolecule, boolean[]) that one}) is called the first time, a mol file is written</li>
    /// <li>if one of the two writeMolecule methods is called more than once the output is a SD file</li>
    /// </ul>
    /// <p>Thus, to write several molecules to a single SD file you can either use {@link #write(IChemObject)} and pass
    /// a {@link org.openscience.cdk.SetOfMolecules SetOfMolecules} or you can repeatedly call one of the two
    /// writeMolecule methods.
    /// <p>For writing a MDL molfile you can this code:
    /// <pre>
    /// MDLWriter writer = new MDLWriter(new FileWriter(new File("output.mol")));
    /// writer.write((Molecule)molecule);
    /// writer.close();
    /// </pre>
    /// 
    /// See {@cdk.cite DAL92}.
    /// 
    /// </summary>
    /// <cdk.module>   io </cdk.module>
    /// <cdk.keyword>  file format, MDL molfile </cdk.keyword>
    /// <cdk.bug>      1522430 </cdk.bug>
    public class MDLWriter : DefaultChemObjectWriter
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MDLFormat();
            }

        }
        /// <summary> Here you can set a map which will be used to build sd fields in the file.
        /// The entries will be translated to sd fields like this:<br>
        /// &gt; &lt;key&gt;<br>
        /// &gt; value<br>
        /// empty line<br>
        /// 
        /// </summary>
        /// <param name="map">The map to be used, map of String-String pairs
        /// </param>
        virtual public System.Collections.IDictionary SdFields
        {
            set
            {
                sdFields = value;
            }

        }

        internal static System.IO.StreamWriter writer;
        //private LoggingTool //logger;
        private int moleculeNumber;
        public System.Collections.IDictionary sdFields = null;
        //private boolean writeAromatic=true;


        /// <summary> Contructs a new MDLWriter that can write an array of 
        /// Molecules to a Writer.
        /// 
        /// </summary>
        /// <param name="out"> The Writer to write to
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MDLWriter(System.IO.StreamWriter out_Renamed)
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
            this.moleculeNumber = 1;
        }

        /// <summary> Contructs a new MDLWriter that can write an array of
        /// Molecules to a given OutputStream.
        /// 
        /// </summary>
        /// <param name="output"> The OutputStream to write to
        /// </param>
        public MDLWriter(System.IO.Stream output)
            : this(new System.IO.StreamWriter(output, System.Text.Encoding.Default))
        {
        }

        public MDLWriter()
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

        /// <summary> 
        /// Method does not do anything until now.
        /// 
        /// </summary>
        public virtual void dontWriteAromatic()
        {
            //writeAromatic=false;
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
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
                if (typeof(ISetOfMolecules).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Writes a IChemObject to the MDL molfile formated output. 
        /// It can only output ChemObjects of type ChemFile, Molecule and
        /// SetOfMolecules.
        /// 
        /// </summary>
        /// <param name="object">class must be of type ChemFile, Molecule or SetOfMolecules.
        /// 
        /// </param>
        /// <seealso cref="org.openscience.cdk.ChemFile">
        /// </seealso>
        public override void write(IChemObject object_Renamed)
        {
            if (object_Renamed is ISetOfMolecules)
            {
                writeSetOfMolecules((ISetOfMolecules)object_Renamed);
            }
            else if (object_Renamed is IChemFile)
            {
                writeChemFile((IChemFile)object_Renamed);
            }
            else if (object_Renamed is IMolecule)
            {
                try
                {
                    bool[] isVisible = new bool[((IMolecule)object_Renamed).AtomCount];
                    for (int i = 0; i < isVisible.Length; i++)
                    {
                        isVisible[i] = true;
                    }
                    writeMolecule((IMolecule)object_Renamed, isVisible);
                }
                catch (System.Exception ex)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.error(ex.Message);
                    //logger.debug(ex);
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    throw new CDKException("Exception while writing MDL file: " + ex.Message, ex);
                }
            }
            else
            {
                throw new CDKException("Only supported is writing of ChemFile, SetOfMolecules, AtomContainer and Molecule objects.");
            }
        }

        /// <summary> Writes an array of Molecules to an OutputStream in MDL sdf format.
        /// 
        /// </summary>
        /// <param name="molecules"> Array of Molecules that is written to an OutputStream 
        /// </param>
        private void writeSetOfMolecules(ISetOfMolecules som)
        {
            IMolecule[] molecules = som.Molecules;
            for (int i = 0; i < som.MoleculeCount; i++)
            {
                try
                {
                    bool[] isVisible = new bool[molecules[i].AtomCount];
                    for (int k = 0; k < isVisible.Length; k++)
                    {
                        isVisible[k] = true;
                    }
                    writeMolecule(molecules[i], isVisible);
                }
                catch (System.Exception exc)
                {
                }
            }
        }

        private void writeChemFile(IChemFile file)
        {
            IAtomContainer[] molecules = ChemFileManipulator.getAllAtomContainers(file);
            for (int i = 0; i < molecules.Length; i++)
            {
                try
                {
                    bool[] isVisible = new bool[molecules[i].AtomCount];
                    for (int k = 0; k < isVisible.Length; k++)
                    {
                        isVisible[k] = true;
                    }
                    writeMolecule(file.Builder.newMolecule(molecules[i]), isVisible);
                }
                catch (System.Exception exc)
                {
                }
            }
        }


        /// <summary> Writes a Molecule to an OutputStream in MDL sdf format.
        /// 
        /// </summary>
        /// <param name="molecule"> Molecule that is written to an OutputStream 
        /// </param>
        public virtual void writeMolecule(IMolecule molecule)
        {
            bool[] isVisible = new bool[molecule.AtomCount];
            for (int i = 0; i < isVisible.Length; i++)
            {
                isVisible[i] = true;
            }
            writeMolecule(molecule, isVisible);
        }

        /// <summary> Writes a Molecule to an OutputStream in MDL sdf format.
        /// 
        /// </summary>
        /// <param name="container"> Molecule that is written to an OutputStream
        /// </param>
        /// <param name="isVisible">Should a certain atom be written to mdl?
        /// </param>
        public virtual void writeMolecule(IMolecule container, bool[] isVisible)
        {
            System.String line = "";
            // taking care of the $$$$ signs:
            // we do not write such a sign at the end of the first molecule, thus we have to write on BEFORE the second molecule
            if (moleculeNumber == 2)
            {
                writer.Write("$$$$");
                //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                writer.WriteLine();
            }
            // write header block
            // lines get shortened to 80 chars, that's in the spec
            System.String title = (System.String)container.getProperty(CDKConstants.TITLE);
            if (title == null)
                title = "";
            if (title.Length > 80)
                title = title.Substring(0, (80) - (0));
            writer.Write(title + "\n");

            /* From CTX spec
            * This line has the format:
            * IIPPPPPPPPMMDDYYHHmmddSSssssssssssEEEEEEEEEEEERRRRRR
            * (FORTRAN: A2<--A8--><---A10-->A2I2<--F10.5-><---F12.5--><-I6-> )
            * User's first and last initials (l), program name (P),
            * date/time (M/D/Y,H:m), dimensional codes (d), scaling factors (S, s), 
            * energy (E) if modeling program input, internal registry number (R) 
            * if input through MDL form.
            * A blank line can be substituted for line 2.
            */
            writer.Write("  CDK    ");
            //UPGRADE_ISSUE: Constructor 'java.text.SimpleDateFormat.SimpleDateFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextSimpleDateFormat'"
            //UPGRADE_TODO: The equivalent in .NET for method 'java.util.Calendar.getTime' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            System.TimeZone generatedAux3 = System.TimeZone.CurrentTimeZone;
            //writer.Write(SupportClass.FormatDateTime(new SimpleDateFormat("M/d/y,H:m", new System.Globalization.CultureInfo("en-US")), SupportClass.CalendarManager.manager.GetDateTime(new System.Globalization.GregorianCalendar())));
            writer.Write('\n');

            System.String comment = (System.String)container.getProperty(CDKConstants.REMARK);
            if (comment == null)
                comment = "";
            if (comment.Length > 80)
                comment = comment.Substring(0, (80) - (0));
            writer.Write(comment + "\n");

            // write Counts line
            int upToWhichAtom = 0;
            for (int i = 0; i < isVisible.Length; i++)
            {
                if (isVisible[i])
                    upToWhichAtom++;
            }
            line += formatMDLInt(upToWhichAtom, 3);
            int numberOfBonds = 0;
            if (upToWhichAtom < container.AtomCount)
            {
                for (int i = 0; i < container.getBondCount(); i++)
                {
                    if (isVisible[container.getAtomNumber(container.getBondAt(i).getAtoms()[0])] && isVisible[container.getAtomNumber(container.getBondAt(i).getAtoms()[1])])
                        numberOfBonds++;
                }
            }
            else
            {
                numberOfBonds = container.getBondCount();
            }
            line += formatMDLInt(numberOfBonds, 3);
            line += "  0  0  0  0  0  0  0  0999 V2000\n";
            writer.Write(line);

            // write Atom block
            IAtom[] atoms = container.Atoms;
            for (int f = 0; f < atoms.Length; f++)
            {
                if (isVisible[f])
                {
                    IAtom atom = atoms[f];
                    line = "";
                    if (atom.getPoint3d() != null)
                    {
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        line += formatMDLFloat((float)atom.X3d);
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        line += formatMDLFloat((float)atom.Y3d);
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        line += (formatMDLFloat((float)atom.Z3d) + " ");
                    }
                    else if (atom.getPoint2d() != null)
                    {
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        line += formatMDLFloat((float)atom.X2d);
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        line += formatMDLFloat((float)atom.Y2d);
                        line += "    0.0000 ";
                    }
                    else
                    {
                        // if no coordinates available, then output a number
                        // of zeros
                        line += formatMDLFloat((float)0.0);
                        line += formatMDLFloat((float)0.0);
                        line += (formatMDLFloat((float)0.0) + " ");
                    }
                    if (container.getAtomAt(f) is IPseudoAtom)
                        line += formatMDLString(((IPseudoAtom)container.getAtomAt(f)).Label, 3);
                    else
                        line += formatMDLString(container.getAtomAt(f).Symbol, 3);
                    line += " 0  0  0  0  0  0  0  0  0  0  0  0";
                    writer.Write(line);
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();
                }
            }

            // write Bond block
            IBond[] bonds = container.Bonds;
            for (int g = 0; g < bonds.Length; g++)
            {
                if (upToWhichAtom == container.AtomCount || (isVisible[container.getAtomNumber(container.getBondAt(g).getAtoms()[0])] && isVisible[container.getAtomNumber(container.getBondAt(g).getAtoms()[1])]))
                {
                    IBond bond = bonds[g];
                    if (bond.getAtoms().Length != 2)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.warn("Skipping bond with more/less than two atoms: " + bond);
                    }
                    else
                    {
                        if (bond.Stereo == CDKConstants.STEREO_BOND_UP_INV || bond.Stereo == CDKConstants.STEREO_BOND_DOWN_INV)
                        {
                            // turn around atom coding to correct for inv stereo
                            line = formatMDLInt(container.getAtomNumber(bond.getAtomAt(1)) + 1, 3);
                            line += formatMDLInt(container.getAtomNumber(bond.getAtomAt(0)) + 1, 3);
                        }
                        else
                        {
                            line = formatMDLInt(container.getAtomNumber(bond.getAtomAt(0)) + 1, 3);
                            line += formatMDLInt(container.getAtomNumber(bond.getAtomAt(1)) + 1, 3);
                        }
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        line += formatMDLInt((int)bond.Order, 3);
                        line += "  ";
                        switch (bond.Stereo)
                        {

                            case CDKConstants.STEREO_BOND_UP:
                                line += "1";
                                break;

                            case CDKConstants.STEREO_BOND_UP_INV:
                                line += "1";
                                break;

                            case CDKConstants.STEREO_BOND_DOWN:
                                line += "6";
                                break;

                            case CDKConstants.STEREO_BOND_DOWN_INV:
                                line += "6";
                                break;

                            default:
                                line += "0";
                                break;

                        }
                        line += "  0  0  0 ";
                        writer.Write(line);
                        //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                        writer.WriteLine();
                    }
                }
            }

            // write formal atomic charges
            for (int i = 0; i < atoms.Length; i++)
            {
                IAtom atom = atoms[i];
                int charge = atom.getFormalCharge();
                if (charge != 0)
                {
                    writer.Write("M  CHG  1 ");
                    writer.Write(formatMDLInt(i + 1, 3));
                    writer.Write(" ");
                    writer.Write(formatMDLInt(charge, 3));
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();
                }
            }

            // write formal isotope information
            for (int i = 0; i < atoms.Length; i++)
            {
                IAtom atom = atoms[i];
                if (!(atom is IPseudoAtom))
                {
                    int atomicMass = atom.MassNumber;
                    int majorMass = IsotopeFactory.getInstance(atom.Builder).getMajorIsotope(atom.Symbol).MassNumber;
                    if (atomicMass != 0 && atomicMass != majorMass)
                    {
                        writer.Write("M  ISO  1 ");
                        writer.Write(formatMDLInt(i + 1, 3));
                        writer.Write(" ");
                        writer.Write(formatMDLInt(atomicMass, 3));
                        //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                        writer.WriteLine();
                    }
                }
            }

            // close molecule
            writer.Write("M  END");
            //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
            writer.WriteLine();
            //write sdfields, if any
            if (sdFields != null)
            {
                //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
                CSGraphT.SupportClass.SetSupport set_Renamed = new CSGraphT.SupportClass.HashSetSupport(sdFields.Keys);
                System.Collections.IEnumerator iterator = set_Renamed.GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                while (iterator.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    System.Object element = iterator.Current;
                    writer.Write("> <" + ((System.String)element) + ">");
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    writer.Write(sdFields[element].ToString());
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    writer.WriteLine();
                }
            }
            // taking care of the $$$$ signs:
            // we write such a sign at the end of all except the first molecule
            if (moleculeNumber != 1)
            {
                writer.Write("$$$$");
                //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                writer.WriteLine();
            }
            moleculeNumber++;
            writer.Flush();
        }

        /// <summary> Formats an int to fit into the connectiontable and changes it 
        /// to a String.
        /// 
        /// </summary>
        /// <param name="i"> The int to be formated
        /// </param>
        /// <param name="l"> Length of the String
        /// </param>
        /// <returns>     The String to be written into the connectiontable
        /// </returns>
        private System.String formatMDLInt(int i, int l)
        {
            System.String s = "", fs = "";
            SupportClass.TextNumberFormat nf = SupportClass.TextNumberFormat.getTextNumberInstance(new System.Globalization.CultureInfo("en"));
            //UPGRADE_ISSUE: Method 'java.text.NumberFormat.setParseIntegerOnly' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextNumberFormatsetParseIntegerOnly_boolean'"
            //nf.setParseIntegerOnly(true);
            nf.setMinimumIntegerDigits(1);
            nf.setMaximumIntegerDigits(l);
            nf.GroupingUsed = false;
            s = nf.FormatLong(i);
            l = l - s.Length;
            for (int f = 0; f < l; f++)
                fs += " ";
            fs += s;
            return fs;
        }




        /// <summary> Formats a float to fit into the connectiontable and changes it
        /// to a String.
        /// 
        /// </summary>
        /// <param name="fl"> The float to be formated
        /// </param>
        /// <returns>      The String to be written into the connectiontable
        /// </returns>
        private System.String formatMDLFloat(float fl)
        {
            System.String s = "", fs = "";
            int l;
            SupportClass.TextNumberFormat nf = SupportClass.TextNumberFormat.getTextNumberInstance(new System.Globalization.CultureInfo("en"));
            nf.setMinimumIntegerDigits(1);
            nf.setMaximumIntegerDigits(4);
            nf.setMinimumFractionDigits(4);
            nf.setMaximumFractionDigits(4);
            nf.GroupingUsed = false;
            s = nf.FormatDouble(fl);
            l = 10 - s.Length;
            for (int f = 0; f < l; f++)
                fs += " ";
            fs += s;
            return fs;
        }



        /// <summary> Formats a String to fit into the connectiontable.
        /// 
        /// </summary>
        /// <param name="s">   The String to be formated
        /// </param>
        /// <param name="le">  The length of the String
        /// </param>
        /// <returns>       The String to be written in the connectiontable
        /// </returns>
        private System.String formatMDLString(System.String s, int le)
        {
            s = s.Trim();
            if (s.Length > le)
                return s.Substring(0, (le) - (0));
            int l;
            l = le - s.Length;
            for (int f = 0; f < l; f++)
                s += " ";
            return s;
        }
    }
}