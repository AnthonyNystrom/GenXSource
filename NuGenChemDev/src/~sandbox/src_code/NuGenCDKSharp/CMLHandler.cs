/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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
*
*/
using System;
using Support;
using Org.OpenScience.CDK.IO.CML.CDOPI;

namespace Org.OpenScience.CDK.IO.CML
{
    /// <summary> SAX2 implementation for CML XML fragment reading. CML Core is supported
    /// as well is the CRML module.
    /// 
    /// <p>Data is stored into the Chemical Document Object which is passed when
    /// instantiating this class. This makes it possible that programs that do not
    /// use CDK for internal data storage, use this CML library.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    public class CMLHandler : XmlSaxDefaultHandler
    {
        private ICMLModule conv;
        //private LoggingTool logger;
        private bool debug = true;

        private System.Collections.Hashtable userConventions;

        // this is a problem under MSFT ie jvm
        //private Stack xpath;
        private CMLStack xpath;
        private CMLStack conventionStack;


        /// <summary> Constructor for the CMLHandler.
        /// 
        /// </summary>
        /// <param name="cdo">The Chemical Document Object in which data is stored
        /// 
        /// </param>
        public CMLHandler(IChemicalDocumentObject cdo)
        {
            //logger = new LoggingTool(this);
            conv = new CMLCoreModule(cdo);
            userConventions = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            xpath = new CMLStack();
            conventionStack = new CMLStack();
        }

        public virtual void registerConvention(System.String convention, ICMLModule conv)
        {
            userConventions[convention] = conv;
        }

        /// <summary> Implementation of the characters() procedure overwriting the DefaultHandler interface.
        /// 
        /// </summary>
        /// <param name="ch">       characters to handle
        /// </param>
        public override void characters(System.Char[] ch, int start, int length)
        {
            //if (debug)
                //logger.debug(new System.String(ch, start, length));
            conv.characterData(xpath, ch, start, length);
        }

        public virtual void doctypeDecl(System.String name, System.String publicId, System.String systemId)
        {
        }

        /// <summary> Calling this procedure signals the end of the XML document.</summary>
        public override void endDocument()
        {
            conv.endDocument();
        }

        public override void endElement(System.String uri, System.String local, System.String raw)
        {
            //if (debug)
            //    logger.debug("</" + raw + ">");
            conv.endElement(xpath, uri, local, raw);
            xpath.pop();
            conventionStack.pop();
        }

        public virtual IChemicalDocumentObject returnCDO()
        {
            return conv.returnCDO();
        }

        public override void startDocument()
        {
            conv.startDocument();
            conventionStack.push("CML");
        }

        public override void startElement(System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            xpath.push(local);
            //if (debug)
            //    logger.debug("<", raw, "> -> ", xpath);
            // Detect CML modules, like CRML and CCML
            if (local.StartsWith("reaction"))
            {
                // e.g. reactionList, reaction -> CRML module
                //logger.info("Detected CRML module");
                conv = new CMLReactionModule(conv);
                conventionStack.push(conventionStack.current());
            }
            else
            {
                // assume CML Core

                // Detect conventions
                System.String convName = "";
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if (atts.GetFullName(i).Equals("convention"))
                    {
                        convName = atts.GetValue(i);
                    }
                }
                if (convName.Length > 0)
                {
                    if (convName.Equals(conventionStack.current()))
                    {
                        //logger.debug("Same convention as parent");
                    }
                    else
                    {
                        //logger.info("New Convention: ", convName);
                        if (convName.Equals("CML"))
                        {
                            /* Don't reset the convention handler to CMLCore,
                            becuase all handlers should extend this handler,
                            and use it for any content other then specifically
                            put into the specific convention */
                        }
                        else if (convName.Equals("PDB"))
                        {
                            conv = new PDBConvention(conv);
                        }
                        else if (convName.Equals("PMP"))
                        {
                            conv = new PMPConvention(conv);
                        }
                        else if (convName.Equals("MDLMol"))
                        {
                            //if (debug)
                            //    logger.debug("MDLMolConvention instantiated...");
                            conv = new MDLMolConvention(conv);
                        }
                        else if (convName.Equals("JMOL-ANIMATION"))
                        {
                            conv = new JMOLANIMATIONConvention(conv);
                        }
                        else if (userConventions.ContainsKey(convName))
                        {
                            //unknown convention. userConvention?
                            ICMLConvention newconv = (ICMLConvention)userConventions[convName];
                            newconv.inherit(conv);
                            conv = newconv;
                        }
                        else
                        {
                            //logger.warn("Detected unknown convention: ", convName);
                        }
                    }
                    conventionStack.push(convName);
                }
                else
                {
                    // no convention set/reset: take convention of parent
                    conventionStack.push(conventionStack.current());
                }
            }
            //if (debug)
            //    logger.debug("ConventionStack: ", conventionStack);
            conv.startElement(xpath, uri, local, raw, atts);
        }
    }
}