/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* This class was based on the DTDResolver class developed by Dan Gezelter.
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
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
using Org.Xml.Sax;
using Support;
using System.Reflection;

namespace Org.OpenScience.CDK.IO.CML
{
    /// <summary> This class resolves DOCTYPE declaration for Chemical Markup Language (CML)
    /// files and uses a local version for validation. More information about
    /// CML can be found at <a href="http://www.xml-cml.org/">http://www.xml-cml.org/</a>.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    public class CMLResolver : XmlSaxEntityResolver
    {
        //private LoggingTool logger;

        public CMLResolver()
        {
            //logger = new LoggingTool(this);
        }

        /// <summary> Not implemented: always returns null.
        /// 
        /// </summary>
        public virtual XmlSourceSupport getExternalSubset(System.String name, System.String baseURI)
        {
            return null;
        }

        /// <summary> Not implemented, but uses resolveEntity(String publicId, String systemId)
        /// instead.
        /// 
        /// </summary>
        public virtual XmlSourceSupport resolveEntity(System.String name, System.String publicId, System.String baseURI, System.String systemId)
        {
            return resolveEntity(publicId, systemId);
        }

        /// <summary> Resolves SYSTEM and PUBLIC identifiers for CML DTDs.
        /// 
        /// </summary>
        /// <param name="publicId">the PUBLIC identifier of the DTD (unused)
        /// </param>
        /// <param name="systemId">the SYSTEM identifier of the DTD
        /// </param>
        /// <returns> the CML DTD as an InputSource or null if id's unresolvable
        /// </returns>
        public virtual XmlSourceSupport resolveEntity(System.String publicId, System.String systemId)
        {
            //logger.debug("CMLResolver: resolving ", publicId, ", ", systemId);
            systemId = systemId.ToLower();
            if ((systemId.IndexOf("cml-1999-05-15.dtd") != -1) || (systemId.IndexOf("cml.dtd") != -1) || (systemId.IndexOf("cml1_0.dtd") != -1))
            {
                //logger.info("File has CML 1.0 DTD");
                return getCMLType("cml1_0.dtd");
            }
            else if ((systemId.IndexOf("cml-2001-04-06.dtd") != -1) || (systemId.IndexOf("cml1_0_1.dtd") != -1) || (systemId.IndexOf("cml_1_0_1.dtd") != -1))
            {
                //logger.info("File has CML 1.0.1 DTD");
                return getCMLType("cml1_0_1.dtd");
            }
            else
            {
                //logger.warn("Could not resolve systemID: ", systemId);
                return null;
            }
        }

        /// <summary> Returns an InputSource of the appropriate CML DTD. It accepts
        /// two CML DTD names: cml1_0.dtd and cml1_0_1.dtd. Returns null
        /// for any other name.
        /// 
        /// </summary>
        /// <param name="type">the name of the CML DTD version
        /// </param>
        /// <returns> the InputSource to the CML DTD
        /// </returns>
        private XmlSourceSupport getCMLType(System.String type)
        {
            try
            {
                System.IO.Stream ins = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + type);//this.GetType().getClassLoader().getResourceAsStream("org/openscience/cdk/io/cml/data/" + type);
                return new XmlSourceSupport(new System.IO.StreamReader(new System.IO.StreamReader(ins, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(ins, System.Text.Encoding.Default).CurrentEncoding));
            }
            catch (System.Exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Error while trying to read CML DTD (" + type + "): ", e.Message);
                //logger.debug(e);
                return null;
            }
        }
    }
}