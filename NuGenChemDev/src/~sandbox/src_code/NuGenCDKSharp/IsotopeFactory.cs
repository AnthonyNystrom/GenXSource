/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-05-04 01:10:39 +0200 (Thu, 04 May 2006) $
*  $Revision: 6153 $
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
using Org.OpenScience.CDK.Config.Isotopes;
using Support;

namespace Org.OpenScience.CDK.Config
{
    /// <summary> Used to store and return data of a particular isotope. As this class is a
    /// singleton class, one gets an instance with: 
    /// <pre>
    /// IsotopeFactory ifac = IsotopFactory.getInstance(new IChemObject().getBuilder());
    /// </pre>
    /// 
    /// <p>Data about the isotopes are read from the file
    /// org.openscience.cdk.config.isotopes.xml in the cdk-standard
    /// module. Part of the data in this file was collected from
    /// the website <a href="http://www.webelements.org">webelements.org</a>.
    /// 
    /// <p>The use of this class is examplified as follows. To get information 
    /// about the major isotope of hydrogen, one can use this code:
    /// <pre>
    /// IsotopeFactory factory = IsotopeFactory.getInstance(new IChemObject().getBuilder());
    /// Isotope major = factory.getMajorIsotope("H");
    /// </pre> 
    /// 
    /// </summary>
    /// <cdk.module>  core </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2001-08-29 </cdk.created>
    /// <cdk.keyword>     isotope </cdk.keyword>
    /// <cdk.keyword>     element </cdk.keyword>
    public class IsotopeFactory
    {
        /// <summary>  Returns the number of isotopes defined by this class.
        /// 
        /// </summary>
        /// <returns>    The size value
        /// </returns>
        virtual public int Size
        {
            get
            {
                return isotopes.Count;
            }

        }

        private static IsotopeFactory ifac = null;
        private System.Collections.ArrayList isotopes = null;
        private System.Collections.Hashtable majorIsotopes = null;
        private bool debug = false;
        //private LoggingTool //logger;

        /// <summary> Private constructor for the IsotopeFactory object.
        /// 
        /// </summary>
        /// <exception cref="IOException">            A problem with reading the isotopes.xml
        /// file
        /// </exception>
        /// <exception cref="OptionalDataException">  Unexpected data appeared in the isotope
        /// ObjectInputStream
        /// </exception>
        /// <exception cref="ClassNotFoundException"> A problem instantiating the isotopes
        /// </exception>
        private IsotopeFactory(IChemObjectBuilder builder)
        {
            //logger = new LoggingTool(this);
            //logger.info("Creating new IsotopeFactory");

            System.IO.Stream ins = null;
            // ObjIn in = null;
            System.String errorMessage = "There was a problem getting org.openscience.cdk." + "config.isotopes.xml as a stream";
            try
            {
                System.String configFile = "isotopes.xml";
                //if (debug)
                    //logger.debug("Getting stream for ", configFile);
                //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                ins = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + configFile);
                    //this.GetType().getClassLoader().getResourceAsStream(configFile);
            }
            catch (System.Exception exception)
            {
                //logger.error(errorMessage);
                //logger.debug(exception);
                throw new System.IO.IOException(errorMessage);
            }
            if (ins == null)
            {
                //logger.error(errorMessage);
                throw new System.IO.IOException(errorMessage);
            }
            IsotopeReader reader = new IsotopeReader(ins, builder);
            //in = new ObjIn(ins, new Config().aliasID(false));
            //isotopes = (Vector) in.readObject();
            isotopes = reader.readIsotopes();
            //if (debug)
                //logger.debug("Found #isotopes in file: ", isotopes.Count);
            /* for (int f = 0; f < isotopes.size(); f++) {
            Isotope isotope = (Isotope)isotopes.elementAt(f);
            } What's this loop for?? */

            majorIsotopes = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        }


        /// <summary> Returns an IsotopeFactory instance.
        /// 
        /// </summary>
        /// <param name="builder">                ChemObjectBuilder used to construct the Isotope's
        /// </param>
        /// <returns>                             The instance value
        /// </returns>
        /// <exception cref="IOException">            Description of the Exception
        /// </exception>
        /// <exception cref="OptionalDataException">  Description of the Exception
        /// </exception>
        /// <exception cref="ClassNotFoundException"> Description of the Exception
        /// </exception>
        public static IsotopeFactory getInstance(IChemObjectBuilder builder)
        {
            if (ifac == null)
            {
                ifac = new IsotopeFactory(builder);
            }
            return ifac;
        }

        /// <summary> Gets an array of all isotoptes known to the IsotopeFactory for the given
        /// element symbol.
        /// 
        /// </summary>
        /// <param name="symbol"> An element symbol to search for
        /// </param>
        /// <returns>         An array of isotopes that matches the given element symbol
        /// </returns>
        public virtual IIsotope[] getIsotopes(System.String symbol)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            for (int f = 0; f < isotopes.Count; f++)
            {
                if (((IIsotope)isotopes[f]).Symbol.Equals(symbol))
                {
                    try
                    {
                        IIsotope clone = (IIsotope)((IIsotope)isotopes[f]).Clone();
                        list.Add(clone);
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.error("Could not clone IIsotope: ", e.Message);
                        //logger.debug(e);
                    }
                }
            }
            return (IIsotope[])SupportClass.ICollectionSupport.ToArray(list, new IIsotope[list.Count]);
        }


        /// <summary> Returns the most abundant (major) isotope with a given atomic number.
        /// 
        /// <p>The isotope's abundancy is for atoms with atomic number 60 and smaller
        /// defined as a number that is proportional to the 100 of the most abundant
        /// isotope. For atoms with higher atomic numbers, the abundancy is defined
        /// as a percentage.
        /// 
        /// </summary>
        /// <param name="atomicNumber"> The atomicNumber for which an isotope is to be returned
        /// </param>
        /// <returns>               The isotope corresponding to the given atomic number
        /// 
        /// </returns>
        /// <seealso cref="getMajorIsotope(String symbol)">
        /// </seealso>
        public virtual IIsotope getMajorIsotope(int atomicNumber)
        {
            IIsotope major = null;
            for (int f = 0; f < isotopes.Count; f++)
            {
                IIsotope current = (IIsotope)isotopes[f];
                if (current.AtomicNumber == atomicNumber)
                {
                    try
                    {
                        if (major == null)
                        {
                            major = (IIsotope)current.Clone();
                        }
                        else
                        {
                            if (current.NaturalAbundance > major.NaturalAbundance)
                            {
                                major = (IIsotope)current.Clone();
                            }
                        }
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.error("Could not clone IIsotope: ", e.Message);
                        //logger.debug(e);
                    }
                }
            }
            //if (major == null)
                //logger.error("Could not find major isotope for: ", atomicNumber);
            return major;
        }

        /// <summary> Checks wether the given element exists.
        /// 
        /// </summary>
        /// <param name="elementName">  The element name to test
        /// </param>
        /// <returns>               True is the element exists, false otherwise
        /// </returns>
        public virtual bool isElement(System.String elementName)
        {
            return (getElement(elementName) != null);
        }

        /// <summary>  Returns the most abundant (major) isotope whose symbol equals element.
        /// 
        /// </summary>
        /// <param name="symbol"> Description of the Parameter
        /// </param>
        /// <returns>         The Major Isotope value
        /// </returns>
        public virtual IIsotope getMajorIsotope(System.String symbol)
        {
            IIsotope major = null;
            if (majorIsotopes.ContainsValue(symbol))
            {
                major = (IIsotope)majorIsotopes[symbol];
            }
            else
            {
                for (int f = 0; f < isotopes.Count; f++)
                {
                    IIsotope current = (IIsotope)isotopes[f];
                    if (current.Symbol.Equals(symbol))
                    {
                        try
                        {
                            if (major == null)
                            {
                                major = (IIsotope)current.Clone();
                            }
                            else
                            {
                                if (current.NaturalAbundance > major.NaturalAbundance)
                                {
                                    major = (IIsotope)current.Clone();
                                }
                            }
                        }
                        //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                        catch (System.Exception e)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            //logger.error("Could not clone IIsotope: ", e.Message);
                            //logger.debug(e);
                        }
                    }
                }
                if (major == null)
                {
                    //logger.error("Could not find major isotope for: ", symbol);
                }
                else
                {
                    majorIsotopes[symbol] = major;
                }
            }
            return major;
        }

        /// <summary>  Returns an Element with a given element symbol.
        /// 
        /// </summary>
        /// <param name="symbol"> The element symbol for the requested element
        /// </param>
        /// <returns>         The configured element
        /// </returns>
        public virtual IElement getElement(System.String symbol)
        {
            IIsotope isotope = getMajorIsotope(symbol);
            return isotope;
        }


        /// <summary>  Returns an element according to a given atomic number.
        /// 
        /// </summary>
        /// <param name="atomicNumber"> The elements atomic number
        /// </param>
        /// <returns>               The Element
        /// </returns>
        public virtual IElement getElement(int atomicNumber)
        {
            IIsotope isotope = getMajorIsotope(atomicNumber);
            return isotope;
        }

        /// <summary> Returns the symbol matching the element with the given atomic number.
        /// 
        /// </summary>
        /// <param name="atomicNumber"> The elements atomic number
        /// </param>
        /// <returns>               The symbol of the Element
        /// </returns>
        public virtual System.String getElementSymbol(int atomicNumber)
        {
            IIsotope isotope = getMajorIsotope(atomicNumber);
            return isotope.Symbol;
        }

        /// <summary>  Configures an atom. Finds the correct element type
        /// by looking at the atoms element symbol.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be configured
        /// </param>
        /// <returns>       The configured atom
        /// </returns>
        public virtual IAtom configure(IAtom atom)
        {
            IIsotope isotope = getMajorIsotope(atom.Symbol);
            return configure(atom, isotope);
        }


        /// <summary>  Configures an atom to have all the data of the
        /// given isotope.
        /// 
        /// </summary>
        /// <param name="atom">    The atom to be configure
        /// </param>
        /// <param name="isotope"> The isotope to read the data from
        /// </param>
        /// <returns>          The configured atom
        /// </returns>
        public virtual IAtom configure(IAtom atom, IIsotope isotope)
        {
            atom.MassNumber = isotope.MassNumber;
            atom.Symbol = isotope.Symbol;
            atom.setExactMass(isotope.getExactMass());
            atom.AtomicNumber = isotope.AtomicNumber;
            atom.NaturalAbundance = isotope.NaturalAbundance;
            return atom;
        }


        /// <summary>  Configures atoms in an AtomContainer to 
        /// carry all the correct data according to their element type.
        /// 
        /// </summary>
        /// <param name="container"> The AtomContainer to be configured
        /// </param>
        public virtual void configureAtoms(IAtomContainer container)
        {
            for (int f = 0; f < container.AtomCount; f++)
            {
                configure(container.getAtomAt(f));
            }
        }
    }
}