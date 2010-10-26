/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-02 11:17:35 +0200 (Tue, 02 May 2006) $
* $Revision: 6123 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sf.net
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
*  Foundation, 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
*/
using System;
using Support;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Config.Isotopes
{
    /// <summary> Reads an isotope list in CML2 format. An example definition is:
    /// <pre>
    /// <isotopeList id="H">
    /// <isotope id="H1" isotopeNumber="1" elementTyp="H">
    /// <abundance dictRef="cdk:relativeAbundance">100.0</abundance>
    /// <scalar dictRef="cdk:exactMass">1.00782504</scalar>
    /// <scalar dictRef="cdk:atomicNumber">1</scalar>
    /// </isotope>
    /// <isotope id="H2" isotopeNumber="2" elementTyp="H">
    /// <abundance dictRef="cdk:relativeAbundance">0.015</abundance>
    /// <scalar dictRef="cdk:exactMass">2.01410179</scalar>
    /// <scalar dictRef="cdk:atomicNumber">1</scalar>
    /// </isotope>
    /// </isotopeList>
    /// </pre> 
    /// 
    /// </summary>
    /// <cdk.module>  core </cdk.module>
    public class IsotopeHandler : XmlSaxDefaultHandler
    {
        /// <summary> Returns the isotopes read from the XML file.
        /// 
        /// </summary>
        /// <returns> A Vector object with all isotopes
        /// </returns>
        virtual public System.Collections.ArrayList Isotopes
        {
            get
            {
                return isotopes;
            }

        }

        //private LoggingTool //logger;
        private System.String currentChars;
        private System.Collections.ArrayList isotopes;

        private IIsotope workingIsotope;
        private System.String currentElement;
        private System.String dictRef;

        private IChemObjectBuilder builder;

        /// <summary> Constructs an IsotopeHandler used by the IsotopeReader.
        /// 
        /// </summary>
        /// <param name="builder">The IChemObjectBuilder used to create new IIsotope's.
        /// </param>
        public IsotopeHandler(IChemObjectBuilder builder)
        {
            //logger = new LoggingTool(this);
            this.builder = builder;
        }

        // SAX Parser methods

        public override void startDocument()
        {
            isotopes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        }

        public override void endElement(System.String uri, System.String local, System.String raw)
        {
            //logger.debug("end element: ", raw);
            if ("isotope".Equals(local))
            {
                if (workingIsotope != null)
                    isotopes.Add(workingIsotope);
                workingIsotope = null;
            }
            else if ("isotopeList".Equals(local))
            {
                currentElement = null;
            }
            else if ("abundance".Equals(local))
            {
                try
                {
                    workingIsotope.NaturalAbundance = System.Double.Parse(currentChars);
                }
                catch (System.FormatException exception)
                {
                    //logger.error("The abundance value is incorrect: ", currentChars);
                    //logger.debug(exception);
                }
            }
            else if ("scalar".Equals(local))
            {
                try
                {
                    if ("cdk:exactMass".Equals(dictRef))
                    {
                        workingIsotope.setExactMass(System.Double.Parse(currentChars));
                    }
                    else if ("cdk:atomicNumber".Equals(dictRef))
                    {
                        workingIsotope.AtomicNumber = System.Int32.Parse(currentChars);
                    }
                }
                catch (System.FormatException exception)
                {
                    //logger.error("The ", dictRef, " value is incorrect: ", currentChars);
                    //logger.debug(exception);
                }
            }
        }

        public override void startElement(System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            currentChars = "";
            dictRef = "";
            //logger.debug("startElement: ", raw);
            //logger.debug("uri: ", uri);
            //logger.debug("local: ", local);
            //logger.debug("raw: ", raw);
            if ("isotope".Equals(local))
            {
                workingIsotope = createIsotopeOfElement(currentElement, atts);
            }
            else if ("isotopeList".Equals(local))
            {
                currentElement = getElementSymbol(atts);
            }
            else if ("abundance".Equals(local))
            {
                //logger.warn("Disregarding dictRef for now...");
            }
            else if ("scalar".Equals(local))
            {
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if ("dictRef".Equals(atts.GetFullName(i)))
                    {
                        dictRef = atts.GetValue(i);
                    }
                }
            }
        }

        public override void characters(System.Char[] chars, int start, int length)
        {
            currentChars += new System.String(chars, start, length);
        }

        private IIsotope createIsotopeOfElement(System.String currentElement, SaxAttributesSupport atts)
        {
            IIsotope isotope = builder.newIsotope(currentElement);
            for (int i = 0; i < atts.GetLength(); i++)
            {
                try
                {
                    if ("id".Equals(atts.GetFullName(i)))
                    {
                        isotope.ID = atts.GetValue(i);
                    }
                    else if ("isotopeNumber".Equals(atts.GetFullName(i)))
                    {
                        isotope.MassNumber = System.Int32.Parse(atts.GetValue(i));
                    }
                    else if ("elementType".Equals(atts.GetFullName(i)))
                    {
                        isotope.Symbol = atts.GetValue(i);
                    }
                }
                catch (System.FormatException exception)
                {
                    //logger.error("Value of isotope@", atts.GetFullName(i), " is not as expected.");
                    //logger.debug(exception);
                }
            }
            return isotope;
        }

        private System.String getElementSymbol(SaxAttributesSupport atts)
        {
            for (int i = 0; i < atts.GetLength(); i++)
            {
                if ("id".Equals(atts.GetFullName(i)))
                {
                    return atts.GetValue(i);
                }
            }
            return "";
        }
    }
}