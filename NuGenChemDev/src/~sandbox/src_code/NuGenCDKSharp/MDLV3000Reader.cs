/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
*
* Copyright (C) 2002-2006  The Jmol Development Team
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
using Org.OpenScience.CDK.IO.Setting;
using System.Text.RegularExpressions;
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Support;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Config;
using javax.vecmath;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Class that implements the new MDL rxn format introduced in August 2002.
    /// The overall syntax is compatible with the old format, but I consider
    /// the format completely different, and thus implemented a separate Reader
    /// for it.
    /// 
    /// <p>This Reader should read all information, but it does not (yet). Please
    /// report any problem with information not read as a bug. Refer to the method
    /// of this class to get more insight in what is read and what is not.
    /// In addition, the cdk.log will show the bits that are not interpreted.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    /// <cdk.created>  2003-10-05 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  MDL RXN V3000 </cdk.keyword>
    /// <cdk.require>  java1.4+ </cdk.require>
    public class MDLV3000Reader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MDLV3000Format();
            }

        }
        virtual public bool Ready
        {
            get
            {
                try
                {
                    return input.Peek() != -1;
                }
                catch (System.Exception exception)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.String error = "Unexpected error while reading file: " + exception.Message;
                    //logger.error(error);
                    //logger.debug(exception);
                    throw new CDKException(error, exception);
                }
            }

        }
        override public IOSetting[] IOSettings
        {
            get
            {
                return new IOSetting[0];
            }

        }

        internal System.IO.StreamReader input = null;
        //private LoggingTool //logger = null;

        private Regex keyValueTuple;
        private Regex keyValueTuple2;

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MDLV3000Reader(System.IO.StreamReader in_Renamed)
        {
            //logger = new LoggingTool(this);
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            input = new System.IO.StreamReader(in_Renamed.BaseStream, in_Renamed.CurrentEncoding);
            initIOSettings();
            /* compile patterns */
            keyValueTuple = new Regex("\\s*(\\w+)=([^\\s]*)(.*)"); // e.g. CHG=-1
            keyValueTuple2 = new Regex("\\s*(\\w+)=\\(([^\\)]*)\\)(.*)"); // e.g. ATOMS=(1 31)
        }

        public MDLV3000Reader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public MDLV3000Reader()
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
                if (typeof(IMolecule).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IMolecule)
            {
                return readMolecule(object_Renamed.Builder);
            }
            return null;
        }

        public virtual IMolecule readMolecule(IChemObjectBuilder builder)
        {
            return builder.newMolecule(readConnectionTable(builder));
        }

        public virtual IAtomContainer readConnectionTable(IChemObjectBuilder builder)
        {
            IAtomContainer readData = builder.newAtomContainer();
            bool foundEND = false;
            while (Ready && !foundEND)
            {
                System.String command = readCommand();
                if ("END CTAB".Equals(command))
                {
                    foundEND = true;
                }
                else if ("BEGIN CTAB".Equals(command))
                {
                    // that's fine
                }
                else if ("COUNTS".Equals(command))
                {
                    // don't think I need to parse this
                }
                else if ("BEGIN ATOM".Equals(command))
                {
                    readAtomBlock(readData);
                }
                else if ("BEGIN BOND".Equals(command))
                {
                    readBondBlock(readData);
                }
                else if ("BEGIN SGROUP".Equals(command))
                {
                    readSGroup(readData);
                }
                else
                {
                    //logger.warn("Unrecognized command: " + command);
                }
            }
            return readData;
        }

        /// <summary> Reads the atoms, coordinates and charges.
        /// 
        /// <p>IMPORTANT: it does not support the atom list and its negation!
        /// </summary>
        public virtual void readAtomBlock(IAtomContainer readData)
        {
            bool foundEND = false;
            while (Ready && !foundEND)
            {
                System.String command = readCommand();
                if ("END ATOM".Equals(command))
                {
                    // FIXME: should check wether 3D is really 2D
                    foundEND = true;
                }
                else
                {
                    //logger.debug("Parsing atom from: " + command);
                    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(command);
                    IAtom atom = readData.Builder.newAtom("C");
                    // parse the index
                    try
                    {
                        System.String indexString = tokenizer.NextToken();
                        atom.ID = indexString;
                    }
                    catch (System.Exception exception)
                    {
                        System.String error = "Error while parsing atom index";
                        //logger.error(error);
                        //logger.debug(exception);
                        throw new CDKException(error, exception);
                    }
                    // parse the element
                    System.String element = tokenizer.NextToken();
                    bool isElement = false;
                    try
                    {
                        isElement = IsotopeFactory.getInstance(atom.Builder).isElement(element);
                    }
                    catch (System.Exception exception)
                    {
                        throw new CDKException("Could not determine if element exists!", exception);
                    }
                    if (isPseudoAtom(element))
                    {
                        atom = readData.Builder.newPseudoAtom(atom);
                    }
                    else if (isElement)
                    {
                        atom.Symbol = element;
                    }
                    else
                    {
                        System.String error = "Cannot parse element of type: " + element;
                        //logger.error(error);
                        throw new CDKException("(Possible CDK bug) " + error);
                    }
                    // parse atom coordinates (in Angstrom)
                    try
                    {
                        System.String xString = tokenizer.NextToken();
                        System.String yString = tokenizer.NextToken();
                        System.String zString = tokenizer.NextToken();
                        double x = System.Double.Parse(xString);
                        double y = System.Double.Parse(yString);
                        double z = System.Double.Parse(zString);
                        atom.setPoint3d(new Point3d(x, y, z));
                        atom.setPoint2d(new Point2d(x, y)); // FIXME: dirty!
                    }
                    catch (System.Exception exception)
                    {
                        System.String error = "Error while parsing atom coordinates";
                        //logger.error(error);
                        //logger.debug(exception);
                        throw new CDKException(error, exception);
                    }
                    // atom-atom mapping
                    System.String mapping = tokenizer.NextToken();
                    if (!mapping.Equals("0"))
                    {
                        //logger.warn("Skipping atom-atom mapping: " + mapping);
                    } // else: default 0 is no mapping defined

                    // the rest are key value things
                    if (command.IndexOf("=") != -1)
                    {
                        System.Collections.Hashtable options = parseOptions(exhaustStringTokenizer(tokenizer));
                        System.Collections.IEnumerator keys = options.Keys.GetEnumerator();
                        //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                        while (keys.MoveNext())
                        {
                            //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                            System.String key = (System.String)keys.Current;
                            System.String value_Renamed = (System.String)options[key];
                            try
                            {
                                if (key.Equals("CHG"))
                                {
                                    int charge = System.Int32.Parse(value_Renamed);
                                    if (charge != 0)
                                    {
                                        // zero is no charge specified
                                        atom.setFormalCharge(charge);
                                    }
                                }
                                else
                                {
                                    //logger.warn("Not parsing key: " + key);
                                }
                            }
                            catch (System.Exception exception)
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                System.String error = "Error while parsing key/value " + key + "=" + value_Renamed + ": " + exception.Message;
                                //logger.error(error);
                                //logger.debug(exception);
                                throw new CDKException(error, exception);
                            }
                        }
                    }

                    // store atom
                    readData.addAtom(atom);
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.debug("Added atom: " + atom);
                }
            }
        }

        /// <summary> Reads the bond atoms, order and stereo configuration.</summary>
        public virtual void readBondBlock(IAtomContainer readData)
        {
            bool foundEND = false;
            while (Ready && !foundEND)
            {
                System.String command = readCommand();
                if ("END BOND".Equals(command))
                {
                    foundEND = true;
                }
                else
                {
                    //logger.debug("Parsing bond from: " + command);
                    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(command);
                    IBond bond = readData.Builder.newBond();
                    // parse the index
                    try
                    {
                        System.String indexString = tokenizer.NextToken();
                        bond.ID = indexString;
                    }
                    catch (System.Exception exception)
                    {
                        System.String error = "Error while parsing bond index";
                        //logger.error(error);
                        //logger.debug(exception);
                        throw new CDKException(error, exception);
                    }
                    // parse the order
                    try
                    {
                        System.String orderString = tokenizer.NextToken();
                        int order = System.Int32.Parse(orderString);
                        if (order >= 4)
                        {
                            //logger.warn("Query order types are not supported (yet). File a bug if you need it");
                        }
                        else
                        {
                            bond.Order = (double)order;
                        }
                    }
                    catch (System.Exception exception)
                    {
                        System.String error = "Error while parsing bond index";
                        //logger.error(error);
                        //logger.debug(exception);
                        throw new CDKException(error, exception);
                    }
                    // parse index atom 1
                    try
                    {
                        System.String indexAtom1String = tokenizer.NextToken();
                        int indexAtom1 = System.Int32.Parse(indexAtom1String);
                        IAtom atom1 = readData.getAtomAt(indexAtom1 - 1);
                        bond.setAtomAt(atom1, 0);
                    }
                    catch (System.Exception exception)
                    {
                        System.String error = "Error while parsing index atom 1 in bond";
                        //logger.error(error);
                        //logger.debug(exception);
                        throw new CDKException(error, exception);
                    }
                    // parse index atom 2
                    try
                    {
                        System.String indexAtom2String = tokenizer.NextToken();
                        int indexAtom2 = System.Int32.Parse(indexAtom2String);
                        IAtom atom2 = readData.getAtomAt(indexAtom2 - 1);
                        bond.setAtomAt(atom2, 1);
                    }
                    catch (System.Exception exception)
                    {
                        System.String error = "Error while parsing index atom 2 in bond";
                        //logger.error(error);
                        //logger.debug(exception);
                        throw new CDKException(error, exception);
                    }
                    // the rest are key=value fields
                    if (command.IndexOf("=") != -1)
                    {
                        System.Collections.Hashtable options = parseOptions(exhaustStringTokenizer(tokenizer));
                        System.Collections.IEnumerator keys = options.Keys.GetEnumerator();
                        //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                        while (keys.MoveNext())
                        {
                            //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                            System.String key = (System.String)keys.Current;
                            System.String value_Renamed = (System.String)options[key];
                            try
                            {
                                if (key.Equals("CFG"))
                                {
                                    int configuration = System.Int32.Parse(value_Renamed);
                                    if (configuration == 0)
                                    {
                                        bond.Stereo = CDKConstants.STEREO_BOND_NONE;
                                    }
                                    else if (configuration == 1)
                                    {
                                        bond.Stereo = CDKConstants.STEREO_BOND_UP;
                                    }
                                    else if (configuration == 2)
                                    {
                                        bond.Stereo = CDKConstants.STEREO_BOND_UNDEFINED;
                                    }
                                    else if (configuration == 3)
                                    {
                                        bond.Stereo = CDKConstants.STEREO_BOND_DOWN;
                                    }
                                }
                                else
                                {
                                    //logger.warn("Not parsing key: " + key);
                                }
                            }
                            catch (System.Exception exception)
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                System.String error = "Error while parsing key/value " + key + "=" + value_Renamed + ": " + exception.Message;
                                //logger.error(error);
                                //logger.debug(exception);
                                throw new CDKException(error, exception);
                            }
                        }
                    }

                    // storing bond
                    readData.addBond(bond);
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.debug("Added bond: " + bond);
                }
            }
        }

        /// <summary> Reads labels.</summary>
        public virtual void readSGroup(IAtomContainer readData)
        {
            bool foundEND = false;
            while (Ready && !foundEND)
            {
                System.String command = readCommand();
                if ("END SGROUP".Equals(command))
                {
                    foundEND = true;
                }
                else
                {
                    //logger.debug("Parsing Sgroup line: " + command);
                    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(command);
                    // parse the index
                    System.String indexString = tokenizer.NextToken();
                    //logger.warn("Skipping external index: " + indexString);
                    // parse command type
                    System.String type = tokenizer.NextToken();
                    // parse the external index
                    System.String externalIndexString = tokenizer.NextToken();
                    //logger.warn("Skipping external index: " + externalIndexString);

                    // the rest are key=value fields
                    System.Collections.Hashtable options = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
                    if (command.IndexOf("=") != -1)
                    {
                        options = parseOptions(exhaustStringTokenizer(tokenizer));
                    }

                    // now interpret line
                    if (type.StartsWith("SUP"))
                    {
                        System.Collections.IEnumerator keys = options.Keys.GetEnumerator();
                        int atomID = -1;
                        System.String label = "";
                        //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                        while (keys.MoveNext())
                        {
                            //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                            System.String key = (System.String)keys.Current;
                            System.String value_Renamed = (System.String)options[key];
                            try
                            {
                                if (key.Equals("ATOMS"))
                                {
                                    SupportClass.Tokenizer atomsTokenizer = new SupportClass.Tokenizer(value_Renamed);
                                    System.Int32.Parse(atomsTokenizer.NextToken()); // should be 1, int atomCount = 
                                    atomID = System.Int32.Parse(atomsTokenizer.NextToken());
                                }
                                else if (key.Equals("LABEL"))
                                {
                                    label = value_Renamed;
                                }
                                else
                                {
                                    //logger.warn("Not parsing key: " + key);
                                }
                            }
                            catch (System.Exception exception)
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                System.String error = "Error while parsing key/value " + key + "=" + value_Renamed + ": " + exception.Message;
                                //logger.error(error);
                                //logger.debug(exception);
                                throw new CDKException(error, exception);
                            }
                            if (atomID != -1 && label.Length > 0)
                            {
                                IAtom atom = readData.getAtomAt(atomID - 1);
                                if (!(atom is IPseudoAtom))
                                {
                                    atom = readData.Builder.newPseudoAtom(atom);
                                }
                                ((IPseudoAtom)atom).Label = label;
                                readData.setAtomAt(atomID - 1, atom);
                            }
                        }
                    }
                    else
                    {
                        //logger.warn("Skipping unrecognized SGROUP type: " + type);
                    }
                }
            }
        }


        /// <summary> Reads the command on this line. If the line is continued on the next, that
        /// part is added.
        /// 
        /// </summary>
        /// <returns> Returns the command on this line.
        /// </returns>
        private System.String readCommand()
        {
            System.String line = readLine();
            if (line.StartsWith("M  V30 "))
            {
                System.String command = line.Substring(7);
                if (command.EndsWith("-"))
                {
                    command = command.Substring(0, (command.Length - 1) - (0));
                    command += readCommand();
                }
                return command;
            }
            else
            {
                throw new CDKException("Could not read MDL file: unexpected line: " + line);
            }
        }

        private System.Collections.Hashtable parseOptions(System.String string_Renamed)
        {
            System.Collections.Hashtable keyValueTuples = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            while (string_Renamed.Length >= 3)
            {
                //logger.debug("Matching remaining option string: " + string_Renamed);
                Match tuple1Matcher = keyValueTuple2.Match(string_Renamed);
                if (tuple1Matcher.Success)
                {
                    System.String key = tuple1Matcher.Groups[1].Value;
                    System.String value_Renamed = tuple1Matcher.Groups[2].Value;
                    string_Renamed = tuple1Matcher.Groups[3].Value;
                    //logger.debug("Found key: " + key);
                    //logger.debug("Found value: " + value_Renamed);
                    keyValueTuples[key] = value_Renamed;
                }
                else
                {
                    Match tuple2Matcher = keyValueTuple.Match(string_Renamed);
                    if (tuple2Matcher.Success)
                    {
                        System.String key = tuple2Matcher.Groups[1].Value;
                        System.String value_Renamed = tuple2Matcher.Groups[2].Value;
                        string_Renamed = tuple2Matcher.Groups[3].Value;
                        //logger.debug("Found key: " + key);
                        //logger.debug("Found value: " + value_Renamed);
                        keyValueTuples[key] = value_Renamed;
                    }
                    else
                    {
                        //logger.warn("Quiting; could not parse: " + string_Renamed + ".");
                        string_Renamed = "";
                    }
                }
            }
            return keyValueTuples;
        }

        public virtual System.String exhaustStringTokenizer(Support.SupportClass.Tokenizer tokenizer)
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();
            buffer.Append(" ");
            while (tokenizer.HasMoreTokens())
            {
                buffer.Append(tokenizer.NextToken());
                buffer.Append(" ");
            }
            return buffer.ToString();
        }

        public virtual System.String readLine()
        {
            System.String line = null;
            try
            {
                line = input.ReadLine();
                //logger.debug("read line: " + line);
            }
            catch (System.Exception exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.String error = "Unexpected error while reading file: " + exception.Message;
                //logger.error(error);
                //logger.debug(exception);
                throw new CDKException(error, exception);
            }
            return line;
        }

        private bool isPseudoAtom(System.String element)
        {
            if (element.Equals("R#") || element.Equals("Q") || element.Equals("A") || element.Equals("*"))
            {
                // 'star' atom
                return true;
            }
            return false;
        }

        public virtual bool accepts(IChemObject object_Renamed)
        {
            if (object_Renamed is IMolecule)
            {
                return true;
            }
            return false;
        }

        public override void close()
        {
            input.Close();
        }

        private void initIOSettings()
        {
        }
    }
}