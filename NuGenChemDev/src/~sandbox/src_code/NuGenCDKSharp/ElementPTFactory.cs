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
using System.Reflection;
using Org.OpenScience.CDK.Config.Elements;
using System.Collections;
using System.Collections.Generic;

namespace Org.OpenScience.CDK.Config
{

    /// <summary> Used to store and return data of a particular chemicalElement. As this class is a
    /// singleton class, one gets an instance with: 
    /// <pre>
    /// ElementPTFactory efac = ElementPTFactory.getInstance();
    /// </pre>
    /// 
    /// </summary>
    /// <author>      	   Miguel Rojas
    /// </author>
    /// <cdk.created>     May 8, 2005 </cdk.created>
    /// <cdk.module>      extra </cdk.module>
    public class ElementPTFactory : IEnumerable
    {
        /// <summary>  Returns an ElementPTFactory instance.
        /// 
        /// </summary>
        /// <returns>                             The instance value
        /// </returns>
        /// <exception cref="IOException">            Description of the Exception
        /// </exception>
        /// <exception cref="OptionalDataException">  Description of the Exception
        /// </exception>
        /// <exception cref="ClassNotFoundException"> Description of the Exception
        /// </exception>
        public static ElementPTFactory Instance
        {
            get
            {
                if (efac == null)
                    efac = new ElementPTFactory();
                return efac;
            }

        }
        /// <summary>  Returns the number of elements defined by this class.
        /// 
        /// </summary>
        /// <returns>    The size value
        /// </returns>
        virtual public int Size
        {
            get
            {
                return elements.Count;
            }

        }

        private static ElementPTFactory efac = null;
        private List<PeriodicTableElement> elements = null;
        private bool debug = false;
        //private LoggingTool //logger;

        /// <summary> Private constructor for the ElementPTFactory object.
        /// 
        /// </summary>
        /// <exception cref="IOException"> A problem with reading the chemicalElements.xml file
        /// </exception>
        /// <exception cref="OptionalDataException">  Unexpected data appeared in the isotope ObjectInputStream
        /// </exception>
        /// <exception cref="ClassNotFoundException"> A problem instantiating the isotopes
        /// </exception>
        private ElementPTFactory()
        {
            //logger = new LoggingTool(this);
            //logger.info("Creating new ElementPTFactory");

            System.IO.Stream ins = null;
            System.String errorMessage = "There was a problem getting org.openscience.cdk." + "config.chemicalElements.xml as a stream";
            try
            {
                System.String configFile = "chemicalElements.xml";
                //if (debug)
                    //logger.debug("Getting stream for ", configFile);
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
            ElementPTReader reader = new ElementPTReader(new System.IO.StreamReader(ins, System.Text.Encoding.Default));
            elements = reader.readElements();
            //if (debug)
                //logger.debug("Found #elements in file: ", elements.Count);
        }

        /// <summary> Returns an Element with a given element symbol.
        /// 
        /// </summary>
        /// <param name="symbol"> An element symbol to search for
        /// </param>
        /// <returns>         An array of element that matches the given element symbol
        /// </returns>
        public virtual PeriodicTableElement getElement(System.String symbol)
        {
            for (int f = 0; f < elements.Count; f++)
            {
                if (elements[f].Symbol.Equals(symbol))
                    try
                    {
                        return (PeriodicTableElement)elements[f].Clone();
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.error("Could not clone PeriodicTableElement: ", e.Message);
                        //logger.debug(e);
                    }
            }
            return null;
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

        /// <summary>  Configures an element. Finds the correct element type
        /// by looking at the element symbol.
        /// 
        /// </summary>
        /// <param name="element">    The element to be configure
        /// </param>
        /// <returns>             The configured atom
        /// </returns>
        public virtual PeriodicTableElement configure(PeriodicTableElement element)
        {
            PeriodicTableElement elementInt = getElement(element.Symbol);

            element.Symbol = elementInt.Symbol;
            element.AtomicNumber = elementInt.AtomicNumber;
            element.Name = elementInt.Name;
            element.ChemicalSerie = elementInt.ChemicalSerie;
            element.Period = elementInt.Period;
            element.Group = elementInt.Group;
            element.Phase = elementInt.Phase;
            element.CASid = elementInt.CASid;
            return element;
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        #endregion
    }
}