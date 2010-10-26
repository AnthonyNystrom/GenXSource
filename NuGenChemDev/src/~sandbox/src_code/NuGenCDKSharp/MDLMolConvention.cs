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
    /// <summary> Implementation of the MDLMol Covention for CML.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public class MDLMolConvention : CMLCoreModule
    {
        public MDLMolConvention(IChemicalDocumentObject cdo)
            : base(cdo)
        {
        }

        public MDLMolConvention(ICMLModule conv)
            : base(conv)
        {
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
            ////logger.debug("MDLMol element: name");
            base.startElement(xpath, uri, local, raw, atts);
        }

        public override void endElement(CMLStack xpath, System.String uri, System.String local, System.String raw)
        {
            base.endElement(xpath, uri, local, raw);
        }

        public override void characterData(CMLStack xpath, char[] ch, int start, int length)
        {
            System.String s = new System.String(ch, start, length).Trim();
            if (xpath.ToString().EndsWith("string/") && BUILTIN.Equals("stereo"))
            {
                stereoGiven = true;
                if (s.Trim().Equals("W"))
                {
                    //logger.debug("CML W stereo found");
                    bondStereo.Add("1");
                }
                else if (s.Trim().Equals("H"))
                {
                    //logger.debug("CML H stereo found");
                    bondStereo.Add("6");
                }
            }
            else
            {
                base.characterData(xpath, ch, start, length);
            }
        }
    }
}