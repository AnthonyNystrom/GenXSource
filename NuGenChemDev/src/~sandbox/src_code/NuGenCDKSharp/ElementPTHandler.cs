/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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

namespace Org.OpenScience.CDK.Config.Elements
{
    /// <summary> Reads an element list in CML2 format. An example definition is:
    /// <pre>
    /// <elementType id="Li">
    /// <label dictRef="cas:id">7439-93-2</label>
    /// <scalar dataType="xsd:Integer" dictRef="cdk:group">1</scalar>
    /// <scalar dataType="xsd:Integer" dictRef="cdk:period">2</scalar>
    /// <scalar dataType="xsd:String" dictRef="cdk:name">Lithium</scalar>
    /// <scalar dataType="xsd:Integer" dictRef="cdk:atomicNumber">3</scalar>
    /// <scalar dataType="xsd:String" dictRef="cdk:chemicalSerie">Alkali Metals</scalar>
    /// <scalar dataType="xsd:String" dictRef="cdk:phase">Solid</scalar>
    /// </elementType>
    /// </pre> 
    /// 
    /// </summary>
    /// <author>      	   Miguel Rojas
    /// </author>
    /// <cdk.created>     May 8, 2005 </cdk.created>
    /// <cdk.module>      extra </cdk.module>
    public class ElementPTHandler : XmlSaxDefaultHandler
    {
        /// <summary> Returns the element read from the XML file.
        /// 
        /// </summary>
        /// <returns> A Vector object with all isotopes
        /// </returns>
        virtual public System.Collections.ArrayList Elements
        {
            get
            {
                return elements;
            }

        }
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_UNSET '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_UNSET = 0;
        //UPGRADE_NOTE: Final was removed from the declaration of 'LABEL_CAS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int LABEL_CAS = 1;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_NAME '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_NAME = 2;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_ATOMICNUMBER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_ATOMICNUMBER = 3;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_CHEMICALSERIE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_CHEMICALSERIE = 4;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_PERIOD '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_PERIOD = 5;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_GROUP '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_GROUP = 6;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_PHASE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_PHASE = 7;
        private int scalarType;
        //private LoggingTool //logger;
        private System.String currentChars;
        private System.Collections.ArrayList elements;

        public PeriodicTableElement elementType;
        public System.String currentElement;
        public System.String dictRef;

        public ElementPTHandler()
        {
            ////logger = new LoggingTool(this);
        }

        // SAX Parser methods

        public override void startDocument()
        {
            elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            scalarType = SCALAR_UNSET;
            elementType = null;
        }

        public override void endElement(System.String uri, System.String local, System.String raw)
        {
            //logger.debug("end element: ", raw);
            if ("elementType".Equals(local))
            {
                elements.Add(elementType);
            }
            else if ("scalar".Equals(local))
            {
                currentChars.Trim();
                try
                {
                    if (scalarType == LABEL_CAS)
                    {
                        elementType.CASid = currentChars;
                    }
                    else if (scalarType == SCALAR_NAME)
                    {
                        elementType.Name = currentChars;
                    }
                    else if (scalarType == SCALAR_ATOMICNUMBER)
                    {
                        elementType.AtomicNumber = System.Int32.Parse(currentChars);
                    }
                    else if (scalarType == SCALAR_CHEMICALSERIE)
                    {
                        elementType.ChemicalSerie = currentChars;
                    }
                    else if (scalarType == SCALAR_PERIOD)
                    {
                        elementType.Period = currentChars;
                    }
                    else if (scalarType == SCALAR_GROUP)
                    {
                        elementType.Group = currentChars;
                    }
                    else if (scalarType == SCALAR_PHASE)
                    {
                        elementType.Phase = currentChars;
                    }
                }
                catch (System.FormatException exception)
                {
                    //logger.error("The abundance value is incorrect: ", currentChars);
                    //logger.debug(exception);
                }
                scalarType = SCALAR_UNSET;
            }
            currentChars = "";
        }

        public override void startElement(System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            currentChars = "";
            dictRef = "";
            //logger.debug("startElement: ", raw);
            //logger.debug("uri: ", uri);
            //logger.debug("local: ", local);
            //logger.debug("raw: ", raw);
            if ("elementType".Equals(local))
            {
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if ("id".Equals(atts.GetFullName(i)))
                    {
                        elementType = new PeriodicTableElement(atts.GetValue(i));
                    }
                }
            }
            else if ("scalar".Equals(local))
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if ("dictRef".Equals(atts.GetFullName(i)))
                    {
                        if ("cas:id".Equals(atts.GetValue(i)))
                        {
                            scalarType = LABEL_CAS;
                        }
                        else if ("cdk:name".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_NAME;
                        }
                        else if ("cdk:atomicNumber".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_ATOMICNUMBER;
                        }
                        else if ("cdk:name".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_NAME;
                        }
                        else if ("cdk:chemicalSerie".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_CHEMICALSERIE;
                        }
                        else if ("cdk:period".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_PERIOD;
                        }
                        else if ("cdk:group".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_GROUP;
                        }
                        else if ("cdk:phase".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_PHASE;
                        }
                    }
                }
        }

        public override void characters(System.Char[] chars, int start, int length)
        {
            //logger.debug("character data");
            currentChars += new System.String(chars, start, length);
        }
    }
}