/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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
using Org.OpenScience.CDK.IO.CML.CDOPI;
using Support;

namespace Org.OpenScience.CDK.IO.CML
{
    /// <summary> 
    /// Implementation of the PMPMol Covention for CML.
    /// 
    /// <p>PMP stands for PolyMorph Predictor and is a module
    /// of Cerius2 (tm).
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public class PMPConvention : CMLCoreModule
    {

        public PMPConvention(IChemicalDocumentObject cdo)
            : base(cdo)
        {
        }

        public PMPConvention(ICMLModule conv)
            : base(conv)
        {
            //logger.debug("New PMP Convention!");
        }

        public override IChemicalDocumentObject returnCDO()
        {
            return this.cdo;
        }

        public override void startDocument()
        {
            base.startDocument();
            cdo.startObject("Frame");
        }

        public override void endDocument()
        {
            cdo.endObject("Frame");
            base.endDocument();
        }


        public override void startElement(CMLStack xpath, System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            //logger.debug("PMP element: name");
            base.startElement(xpath, uri, local, raw, atts);
        }

        public override void endElement(CMLStack xpath, System.String uri, System.String local, System.String raw)
        {
            base.endElement(xpath, uri, local, raw);
        }

        public override void characterData(CMLStack xpath, char[] ch, int start, int length)
        {
            System.String s = new System.String(ch, start, length).Trim();
            //logger.debug("Start PMP chardata (" + CurrentElement + ") :" + s);
            //logger.debug(" ElTitle: " + elementTitle);
            if (xpath.ToString().EndsWith("string/") && BUILTIN.Equals("spacegroup"))
            {
                System.String sg = "P1";
                // standardize space group names (see Crystal.java)
                if ("P 21 21 21 (1)".Equals(s))
                {
                    sg = "P 2_1 2_1 2_1";
                }
                cdo.setObjectProperty("Crystal", "spacegroup", sg);
            }
            else
            {
                if (xpath.ToString().EndsWith("floatArray/") && (elementTitle.Equals("a") || elementTitle.Equals("b") || elementTitle.Equals("c")))
                {
                    System.String axis = elementTitle + "-axis";
                    cdo.startObject(axis);
                    try
                    {
                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(s);
                        //logger.debug("Tokens: " + st.Count);
                        if (st.Count > 2)
                        {
                            System.String token = st.NextToken();
                            //logger.debug("FloatArray (Token): " + token);
                            cdo.setObjectProperty(axis, "x", token);
                            token = st.NextToken();
                            //logger.debug("FloatArray (Token): " + token);
                            cdo.setObjectProperty(axis, "y", token);
                            token = st.NextToken();
                            //logger.debug("FloatArray (Token): " + token);
                            cdo.setObjectProperty(axis, "z", token);
                        }
                        else
                        {
                            //logger.debug("PMP Convention error: incorrect number of cell axis fractions!\n");
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.debug("PMP Convention error: " + e.ToString());
                    }
                    cdo.endObject(axis);
                }
                else
                {
                    base.characterData(xpath, ch, start, length);
                }
            }
            //logger.debug("End PMP chardata");
        }
    }
}