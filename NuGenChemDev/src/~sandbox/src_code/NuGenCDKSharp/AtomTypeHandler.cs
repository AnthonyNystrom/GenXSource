/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
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

namespace Org.OpenScience.CDK.Config.AtomTypes
{
    /// <summary> SAX Handler for the AtomTypeReader.
    /// 
    /// </summary>
    /// <seealso cref="AtomTypeReader">
    /// 
    /// </seealso>
    /// <cdk.module>  core </cdk.module>
    public class AtomTypeHandler : XmlSaxDefaultHandler
    {
        /// <summary> Returns a Vector with read IAtomType's.
        /// 
        /// </summary>
        /// <returns> The read IAtomType's.
        /// </returns>
        virtual public System.Collections.ArrayList AtomTypes
        {
            get
            {
                return atomTypes;
            }

        }

        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_UNSET '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_UNSET = 0;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_MAXBONDORDER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_MAXBONDORDER = 1;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_BONDORDERSUM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_BONDORDERSUM = 2;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_HYBRIDIZATION '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_HYBRIDIZATION = 3;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_FORMALNEIGHBOURCOUNT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_FORMALNEIGHBOURCOUNT = 4;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_VALENCY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_VALENCY = 5;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_DA '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_DA = 6;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_SPHERICALMATCHER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_SPHERICALMATCHER = 7;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_CHEMICALGROUPCONSTANT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_CHEMICALGROUPCONSTANT = 8;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_RINGSIZE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_RINGSIZE = 9;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_ISAROMATIC '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_ISAROMATIC = 10;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_FORMALCHARGE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_FORMALCHARGE = 11;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SCALAR_VANDERWAALSRADIUS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int SCALAR_VANDERWAALSRADIUS = 12;

        //private LoggingTool //logger;
        private System.String currentChars;
        private System.Collections.ArrayList atomTypes;
        private int scalarType;
        private IAtomType atomType;

        private static IChemObjectBuilder builder;

        /// <summary> Constructs a new AtomTypeHandler and will create IAtomType
        /// implementations using the given IChemObjectBuilder.
        /// 
        /// </summary>
        /// <param name="build">The IChemObjectBuilder used to create the IAtomType's.
        /// </param>
        public AtomTypeHandler(IChemObjectBuilder build)
        {
            //logger = new LoggingTool(this);
            builder = build;
        }

        // SAX Parser methods

        /* public void doctypeDecl(String name, String publicId, String systemId) {
        //logger.info("DocType root element: " + name);
        //logger.info("DocType root PUBLIC: " + publicId);
        //logger.info("DocType root SYSTEM: " + systemId);
        } */

        public override void startDocument()
        {
            atomTypes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            scalarType = SCALAR_UNSET;
            atomType = null;
        }

        public override void endElement(System.String uri, System.String local, System.String raw)
        {
            //NOPMD
            //logger.debug("END Element: ", raw);
            //logger.debug("  uri: ", uri);
            //logger.debug("  local: ", local);
            //logger.debug("  raw: ", raw);
            //logger.debug("  chars: ", currentChars.Trim());
            if ("atomType".Equals(local))
            {
                atomTypes.Add(atomType);
            }
            else if ("scalar".Equals(local))
            {
                currentChars.Trim();
                try
                {
                    if (scalarType == SCALAR_BONDORDERSUM)
                    {
                        atomType.BondOrderSum = System.Double.Parse(currentChars);
                    }
                    else if (scalarType == SCALAR_MAXBONDORDER)
                    {
                        atomType.MaxBondOrder = System.Double.Parse(currentChars);
                    }
                    else if (scalarType == SCALAR_FORMALNEIGHBOURCOUNT)
                    {
                        atomType.FormalNeighbourCount = System.Int32.Parse(currentChars);
                    }
                    else if (scalarType == SCALAR_VALENCY)
                    {
                        atomType.Valency = System.Int32.Parse(currentChars);
                    }
                    else if (scalarType == SCALAR_FORMALCHARGE)
                    {
                        atomType.setFormalCharge(System.Int32.Parse(currentChars));
                    }
                    else if (scalarType == SCALAR_HYBRIDIZATION)
                    {
                        if ("sp1".Equals(currentChars))
                        {
                            atomType.Hybridization = CDKConstants.HYBRIDIZATION_SP1;
                        }
                        else if ("sp2".Equals(currentChars))
                        {
                            atomType.Hybridization = CDKConstants.HYBRIDIZATION_SP2;
                        }
                        else if ("sp3".Equals(currentChars))
                        {
                            atomType.Hybridization = CDKConstants.HYBRIDIZATION_SP3;
                        }
                        else
                        {
                            //logger.warn("Unrecognized hybridization in config file: ", currentChars);
                        }
                    }
                    else if (scalarType == SCALAR_DA)
                    {
                        if ("A".Equals(currentChars))
                        {
                            atomType.setFlag(CDKConstants.IS_HYDROGENBOND_ACCEPTOR, true);
                        }
                        else if ("D".Equals(currentChars))
                        {
                            atomType.setFlag(CDKConstants.IS_HYDROGENBOND_DONOR, true);
                        }
                        else
                        {
                            //logger.warn("Unrecognized H-bond donor/acceptor pattern in config file: ", currentChars);
                        }
                    }
                    else if (scalarType == SCALAR_SPHERICALMATCHER)
                    {
                        atomType.setProperty(CDKConstants.SPHERICAL_MATCHER, currentChars);
                    }
                    else if (scalarType == SCALAR_RINGSIZE)
                    {
                        atomType.setProperty(CDKConstants.PART_OF_RING_OF_SIZE, (System.Object)System.Int32.Parse(currentChars));
                    }
                    else if (scalarType == SCALAR_CHEMICALGROUPCONSTANT)
                    {
                        atomType.setProperty(CDKConstants.CHEMICAL_GROUP_CONSTANT, (System.Object)System.Int32.Parse(currentChars));
                    }
                    else if (scalarType == SCALAR_ISAROMATIC)
                    {
                        atomType.setFlag(CDKConstants.ISAROMATIC, true);
                    }
                    else if (scalarType == SCALAR_VANDERWAALSRADIUS)
                    {
                        atomType.VanderwaalsRadius = System.Double.Parse(currentChars);
                    }
                }
                catch (System.Exception exception)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.error("Value (", currentChars, ") is not off the expected type: ", exception.Message);
                    //logger.debug(exception);
                }
                scalarType = SCALAR_UNSET;
            }
            currentChars = "";
        }

        public override void startElement(System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            currentChars = "";
            //logger.debug("START Element: ", raw);
            //logger.debug("  uri: ", uri);
            //logger.debug("  local: ", local);
            //logger.debug("  raw: ", raw);

            if ("atomType".Equals(local))
            {
                atomType = builder.newAtomType("R");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if ("id".Equals(atts.GetFullName(i)))
                    {
                        atomType.AtomTypeName = atts.GetValue(i);
                    }
                }
            }
            else if ("atom".Equals(local))
            {
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if ("elementType".Equals(atts.GetFullName(i)))
                    {
                        atomType.Symbol = atts.GetValue(i);
                    }
                    else if ("formalCharge".Equals(atts.GetFullName(i)))
                    {
                        try
                        {
                            atomType.setFormalCharge(System.Int32.Parse(atts.GetValue(i)));
                        }
                        catch (System.FormatException exception)
                        {
                            //logger.error("Value of <atom> @", atts.GetFullName(i), " is not an integer: ", atts.GetValue(i));
                            //logger.debug(exception);
                        }
                    }
                }
            }
            else if ("scalar".Equals(local))
            {
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if ("dictRef".Equals(atts.GetFullName(i)))
                    {
                        if ("cdk:maxBondOrder".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_MAXBONDORDER;
                        }
                        else if ("cdk:bondOrderSum".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_BONDORDERSUM;
                        }
                        else if ("cdk:hybridization".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_HYBRIDIZATION;
                        }
                        else if ("cdk:formalNeighbourCount".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_FORMALNEIGHBOURCOUNT;
                        }
                        else if ("cdk:valency".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_VALENCY;
                        }
                        else if ("cdk:formalCharge".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_FORMALCHARGE;
                        }
                        else if ("cdk:DA".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_DA;
                        }
                        else if ("cdk:sphericalMatcher".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_SPHERICALMATCHER;
                        }
                        else if ("cdk:ringSize".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_RINGSIZE;
                        }
                        else if ("cdk:ringConstant".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_CHEMICALGROUPCONSTANT;
                        }
                        else if ("cdk:aromaticAtom".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_ISAROMATIC;
                        }
                        else if ("emboss:vdwrad".Equals(atts.GetValue(i)))
                        {
                            scalarType = SCALAR_VANDERWAALSRADIUS;
                        }
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