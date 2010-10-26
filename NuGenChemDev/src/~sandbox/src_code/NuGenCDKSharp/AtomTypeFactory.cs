/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 11:39:20 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6669 $
*
*  Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using System.Reflection;
using Org.OpenScience.CDK.Exception;
using System.Runtime.Remoting;

namespace Org.OpenScience.CDK.Config
{
    /// <summary>  General class for defining AtomTypes. This class itself does not define the
    /// items types; for this classes implementing the AtomTypeConfiguration
    /// interface are used.
    /// 
    /// <p>To see which AtomTypeConfigurator's CDK provides, one should check the
    /// AtomTypeConfigurator API.
    /// 
    /// <p>The AtomTypeFactory is a singleton class, which means that there exists
    /// only one instance of the class. Well, almost. For each atom type table,
    /// there is one AtomTypeFactory instance. An instance of this class is
    /// obtained with:
    /// <pre>
    /// AtomTypeFactory factory = AtomTypeFactory.getInstance(someChemObjectBuilder);
    /// </pre>
    /// For each atom type list a separate AtomTypeFactory is instantiated.
    /// 
    /// <p>To get all the atom types of an element from a specific list, this 
    /// code can be used:
    /// <pre>
    /// AtomTypeFactory factory = AtomTypeFactory.getInstance(
    /// "org/openscience/cdk/config/data/jmol_atomtypes.txt",
    /// someChemObjectBuilder
    /// );
    /// AtomType[] types = factory.getAtomTypes("C");
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  core </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2001-08-29 </cdk.created>
    /// <cdk.keyword>     atom, type </cdk.keyword>
    /// <seealso cref="IAtomTypeConfigurator">
    /// </seealso>
    public class AtomTypeFactory
    {
        /// <summary> Returns the number of atom types in this list.
        /// 
        /// </summary>
        /// <returns>    The number of atom types
        /// </returns>
        virtual public int Size
        {
            get
            {
                return atomTypes.Count;
            }

        }
        /// <summary> Gets the allAtomTypes attribute of the AtomTypeFactory object.
        /// 
        /// </summary>
        /// <returns>    The allAtomTypes value
        /// </returns>
        virtual public IAtomType[] AllAtomTypes
        {
            get
            {
                //logger.debug("Returning list of size: ", Size);
                System.Collections.ArrayList atomtypeList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                IAtomType atomType = null;
                for (int f = 0; f < atomTypes.Count; f++)
                {
                    try
                    {
                        atomType = (IAtomType)((IAtomType)atomTypes[f]).Clone();
                        atomtypeList.Add(atomType);
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.error("Could not clone IAtomType: ", e.Message);
                        //logger.debug(e);
                    }
                }
                IAtomType[] atomTypes2 = new IAtomType[atomtypeList.Count];
                atomtypeList.CopyTo(atomTypes2);
                return atomTypes2;
            }

        }

        /// <summary>  Used as an ID to describe the atom type.</summary>
        public const System.String ATOMTYPE_ID_STRUCTGEN = "structgen";
        /// <summary>  Used as an ID to describe the atom type.</summary>
        public const System.String ATOMTYPE_ID_MODELING = "modeling";
        // these are not available
        /// <summary>  Used as an ID to describe the atom type.</summary>
        public const System.String ATOMTYPE_ID_JMOL = "jmol";

        private const System.String TXT_EXTENSION = "txt";
        private const System.String XML_EXTENSION = "xml";

        //private static LoggingTool //logger;
        private static System.Collections.Hashtable tables = null;
        private System.Collections.ArrayList atomTypes = null;

        /// <summary> Private constructor for the AtomTypeFactory singleton.
        /// 
        /// </summary>
        /// <exception cref="IOException">            Thrown if something goes wrong with reading the config
        /// </exception>
        /// <exception cref="ClassNotFoundException"> Thrown if a class was not found :-)
        /// </exception>
        private AtomTypeFactory(System.String configFile, IChemObjectBuilder builder)
        {
            //if (//logger == null)
            //{
            //    //logger = new LoggingTool(this);
            //}
            atomTypes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(100));
            readConfiguration(configFile, builder);
        }

        /// <summary> Private constructor for the AtomTypeFactory singleton.
        /// 
        /// </summary>
        /// <exception cref="IOException">            Thrown if something goes wrong with reading the config
        /// </exception>
        /// <exception cref="ClassNotFoundException"> Thrown if a class was not found :-)
        /// </exception>
        private AtomTypeFactory(System.IO.Stream ins, System.String format, IChemObjectBuilder builder)
        {
            //if (//logger == null)
            //{
            //    //logger = new LoggingTool(this);
            //}
            atomTypes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(100));
            readConfiguration(ins, format, builder);
        }

        /// <summary> Method to create a default AtomTypeFactory, using the given InputStream.
        /// An AtomType of this kind is not cached.
        /// 
        /// </summary>
        /// <seealso cref="getInstance(String, IChemObjectBuilder)">
        /// </seealso>
        /// <param name="ins">                   InputStream containing the data
        /// </param>
        /// <param name="format">                String representing the possible formats ('xml' and 'txt')
        /// </param>
        /// <param name="builder">               IChemObjectBuilder used to make IChemObject instances
        /// </param>
        /// <returns>                        The AtomTypeFactory for the given data file
        /// </returns>
        /// <throws>  IOException            when the file cannot be read </throws>
        /// <throws>  ClassNotFoundException when the AtomTypeFactory cannot be found </throws>
        public static AtomTypeFactory getInstance(System.IO.Stream ins, System.String format, IChemObjectBuilder builder)
        {
            return new AtomTypeFactory(ins, format, builder);
        }

        /// <summary> Method to create a default AtomTypeFactory, using the structgen atom type list.
        /// 
        /// </summary>
        /// <seealso cref="getInstance(String, IChemObjectBuilder)">
        /// </seealso>
        /// <param name="builder">               IChemObjectBuilder used to make IChemObject instances
        /// </param>
        /// <returns>                        The AtomTypeFactory for the given data file
        /// </returns>
        /// <throws>  IOException            when the file cannot be read </throws>
        /// <throws>  ClassNotFoundException when the AtomTypeFactory cannot be found </throws>
        public static AtomTypeFactory getInstance(IChemObjectBuilder builder)
        {
            return getInstance("structgen_atomtypes.xml", builder);
        }

        /// <summary> Method to create a specialized AtomTypeFactory. Available lists in CDK are:
        /// <ul>
        /// <li>org/openscience/cdk/config/data/jmol_atomtypes.txt
        /// <li>org/openscience/cdk/config/data/mol2_atomtypes.xml
        /// <li>org/openscience/cdk/config/data/structgen_atomtypes.xml
        /// <li>org/openscience/cdk/config/data/valency_atomtypes.xml
        /// <li>org/openscience/cdk/config/data/mm2_atomtypes.xml
        /// <li>org/openscience/cdk/config/data/mmff94_atomtypes.xml
        /// </ul>
        /// 
        /// </summary>
        /// <param name="configFile">            String the name of the data file
        /// </param>
        /// <param name="builder">               IChemObjectBuilder used to make IChemObject instances
        /// </param>
        /// <returns>                        The AtomTypeFactory for the given data file
        /// </returns>
        /// <throws>  IOException            when the file cannot be read </throws>
        /// <throws>  ClassNotFoundException when the AtomTypeFactory cannot be found </throws>
        public static AtomTypeFactory getInstance(System.String configFile, IChemObjectBuilder builder)
        {
            if (tables == null)
            {
                tables = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            }
            if (!(tables.ContainsKey(configFile)))
            {
                tables[configFile] = new AtomTypeFactory(configFile, builder);
            }
            return (AtomTypeFactory)tables[configFile];
        }

        /// <summary> Read the config from a text file.
        /// 
        /// </summary>
        /// <param name="configFile"> name of the config file
        /// </param>
        /// <param name="builder">    IChemObjectBuilder used to make IChemObject instances
        /// </param>
        private void readConfiguration(System.String fileName, IChemObjectBuilder builder)
        {
            //logger.info("Reading config file from ", fileName);

            System.IO.Stream ins = null;
            {
                //try to see if this is a resource
                //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                ins = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + fileName);
                if (ins == null)
                {
                    // try to see if this configFile is an actual file
                    System.IO.FileInfo file = new System.IO.FileInfo(fileName);
                    bool tmpBool;
                    if (System.IO.File.Exists(file.FullName))
                        tmpBool = true;
                    else
                        tmpBool = System.IO.Directory.Exists(file.FullName);
                    if (tmpBool)
                    {
                        //logger.debug("configFile is a File");
                        // what's next?
                        try
                        {
                            //UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javaioFile'"
                            ins = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        }
                        catch (System.Exception exception)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            //logger.error(exception.Message);
                            //logger.debug(exception);
                        }
                    }
                    else
                    {
                        //logger.error("no stream and no file");
                    }
                }
            }

            System.String format = XML_EXTENSION;
            if (fileName.EndsWith(TXT_EXTENSION))
            {
                format = TXT_EXTENSION;
            }
            else if (fileName.EndsWith(XML_EXTENSION))
            {
                format = XML_EXTENSION;
            }
            readConfiguration(ins, format, builder);
        }

        private IAtomTypeConfigurator constructConfigurator(System.String format)
        {
            try
            {
                if (format.Equals(TXT_EXTENSION))
                {
                    ObjectHandle handle = System.Activator.CreateInstance("NuGenCDKSharp", "Org.OpenScience.CDK.Config.TXTBasedAtomTypeConfigurator");
                    return (IAtomTypeConfigurator)handle.Unwrap();
                }
                else if (format.Equals(XML_EXTENSION))
                {
                    ObjectHandle handle = System.Activator.CreateInstance("NuGenCDKSharp", "Org.OpenScience.CDK.Config.CDKBasedAtomTypeConfigurator");
                    return (IAtomTypeConfigurator)handle.Unwrap();
                }
            }
            catch (System.Exception exc)
            {
                //logger.error("Could not get instance of AtomTypeConfigurator for format ", format);
                //logger.debug(exc);
            }
            return null;
        }

        private void readConfiguration(System.IO.Stream ins, System.String format, IChemObjectBuilder builder)
        {
            IAtomTypeConfigurator atc = constructConfigurator(format);
            if (atc != null)
            {
                atc.InputStream = ins;
                try
                {
                    atomTypes = atc.readAtomTypes(builder);
                }
                catch (System.Exception exc)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.error("Could not read AtomType's from file due to: ", exc.Message);
                    //logger.debug(exc);
                }
            }
            else
            {
                //logger.debug("AtomTypeConfigurator was null!");
                atomTypes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            }
        }


        /// <summary> Get an AtomType with the given ID.
        /// 
        /// </summary>
        /// <param name="identifier">                  an ID for a particular atom type (like C$)
        /// </param>
        /// <returns>                              The AtomType for this id
        /// </returns>
        /// <exception cref="NoSuchAtomTypeException"> Thrown if the atom type does not exist.
        /// </exception>
        public virtual IAtomType getAtomType(System.String identifier)
        {
            IAtomType atomType = null;
            for (int f = 0; f < atomTypes.Count; f++)
            {
                atomType = (IAtomType)atomTypes[f];
                if (atomType.AtomTypeName.Equals(identifier))
                {
                    return atomType;
                }
            }
            throw new NoSuchAtomTypeException("The AtomType " + identifier + " could not be found");
        }


        /// <summary> Get an array of all atomTypes known to the AtomTypeFactory for the given
        /// element symbol and atomtype class.
        /// 
        /// </summary>
        /// <param name="symbol"> An element symbol to search for
        /// </param>
        /// <returns>         An array of atomtypes that matches the given element symbol
        /// and atomtype class
        /// </returns>
        public virtual IAtomType[] getAtomTypes(System.String symbol)
        {
            //logger.debug("Request for atomtype for symbol ", symbol);
            System.Collections.ArrayList atomList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            IAtomType atomType = null;
            for (int f = 0; f < this.atomTypes.Count; f++)
            {
                atomType = (IAtomType)this.atomTypes[f];
                // //logger.debug("  does symbol match for: ", atomType);
                if (atomType.Symbol.Equals(symbol))
                {
                    // //logger.debug("Atom type found for symbol: ", atomType);
                    IAtomType clone;
                    try
                    {
                        clone = (IAtomType)atomType.Clone();
                        atomList.Add(clone);
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.error("Could not clone IAtomType: ", e.Message);
                        //logger.debug(e);
                    }
                }
            }
            IAtomType[] atomTypes = new IAtomType[atomList.Count];
            atomList.CopyTo(atomTypes);
            //if (atomTypes.Length > 0)
                //logger.debug("Atomtype for symbol ", symbol, " has this number of types: " + atomTypes.Length);
            //else
                //logger.debug("No atomtype for symbol ", symbol);
            return atomTypes;
        }


        /// <summary> Configures an atom. Finds the correct element type by looking at the Atom's
        /// atom type name, and if that fails, picks the first atom type matching
        /// the Atom's element symbol..
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be configured
        /// </param>
        /// <returns>       The configured atom
        /// </returns>
        /// <throws>        CDKException when it could not recognize and configure the  </throws>
        /// <summary>               IAtom
        /// </summary>
        public virtual IAtom configure(IAtom atom)
        {
            if (atom is IPseudoAtom)
            {
                // do not try to configure PseudoAtom's
                return atom;
            }
            try
            {
                IAtomType atomType = null;
                System.String atomTypeName = atom.AtomTypeName;
                if (atomTypeName == null || atomTypeName.Length == 0)
                {
                    //logger.debug("Using atom symbol because atom type name is empty...");
                    IAtomType[] types = getAtomTypes(atom.Symbol);
                    if (types.Length > 0)
                    {
                        //logger.warn("Taking first atom type, but other may exist");
                        atomType = types[0];
                    }
                    else
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        System.String message = "Could not configure atom with unknown ID: " + atom.ToString() + " + (id=" + atom.AtomTypeName + ")";
                        //logger.warn(message);
                        throw new CDKException(message);
                    }
                }
                else
                {
                    atomType = getAtomType(atom.AtomTypeName);
                }
                //logger.debug("Configuring with atomtype: ", atomType);
                atom.Symbol = atomType.Symbol;
                atom.MaxBondOrder = atomType.MaxBondOrder;
                atom.BondOrderSum = atomType.BondOrderSum;
                atom.VanderwaalsRadius = atomType.VanderwaalsRadius;
                atom.CovalentRadius = atomType.CovalentRadius;
                atom.Hybridization = atomType.Hybridization;
                System.Object color = atomType.getProperty("org.openscience.cdk.renderer.color");
                if (color != null)
                {
                    atom.setProperty("org.openscience.cdk.renderer.color", color);
                }
                if (atomType.AtomicNumber != 0)
                {
                    atom.AtomicNumber = atomType.AtomicNumber;
                }
                else
                {
                    //logger.debug("Did not configure atomic number: AT.an=", atomType.AtomicNumber);
                }
                if (atomType.getExactMass() > 0.0)
                {
                    atom.setExactMass(atomType.getExactMass());
                }
                else
                {
                    //logger.debug("Did not configure mass: AT.mass=", atomType.AtomicNumber);
                }
            }
            catch (System.Exception exception)
            {
                //logger.warn("Could not configure atom with unknown ID: ", atom, " + (id=", atom.AtomTypeName, ")");
                //logger.debug(exception);
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                throw new CDKException(exception.ToString());
            }
            //logger.debug("Configured: ", atom);
            return atom;
        }
    }
}