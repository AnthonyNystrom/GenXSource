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
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    /// <cdk.module>  io </cdk.module>
    public class JMOLANIMATIONConvention : CMLCoreModule
    {

        //UPGRADE_NOTE: Final was removed from the declaration of 'UNKNOWN '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int UNKNOWN = -1;
        //UPGRADE_NOTE: Final was removed from the declaration of 'ENERGY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int ENERGY = 1;

        private int current;
        private System.String frame_energy;
        //new private LoggingTool //logger;

        public JMOLANIMATIONConvention(IChemicalDocumentObject cdo)
            : base(cdo)
        {
            //logger = new LoggingTool(this);
            current = UNKNOWN;
        }

        public JMOLANIMATIONConvention(ICMLModule conv)
            : base(conv)
        {
            //logger = new LoggingTool(this);
        }

        public override IChemicalDocumentObject returnCDO()
        {
            return this.cdo;
        }

        public override void startDocument()
        {
            base.startDocument();
        }

        public override void endDocument()
        {
            base.endDocument();
        }


        public override void startElement(CMLStack xpath, System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            System.String name = local;
            if (name.Equals("list"))
            {
                //logger.debug("Oke, JMOLANIMATION seems to be kicked in :)");
                cdo.startObject("Animation");
                base.startElement(xpath, uri, local, raw, atts);
            }
            else if (name.Equals("molecule"))
            {
                cdo.startObject("Frame");
                //logger.debug("New frame being parsed.");
                base.startElement(xpath, uri, local, raw, atts);
            }
            else if (name.Equals("float"))
            {
                bool isEnergy = false;
                //logger.debug("FLOAT found!");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    //logger.debug(" att: ", atts.GetFullName(i), " -> ", atts.GetValue(i));
                    if (atts.GetFullName(i).Equals("title") && atts.GetValue(i).Equals("FRAME_ENERGY"))
                    {
                        isEnergy = true;
                    }
                }
                if (isEnergy)
                {
                    // oke, this is the frames energy!
                    current = ENERGY;
                }
                else
                {
                    base.startElement(xpath, uri, local, raw, atts);
                }
            }
            else
            {
                base.startElement(xpath, uri, local, raw, atts);
            }
        }

        public override void endElement(CMLStack xpath, System.String uri, System.String local, System.String raw)
        {
            System.String name = local;
            if (current == ENERGY)
            {
                cdo.setObjectProperty("Frame", "energy", frame_energy);
                // + " " + units);
                current = UNKNOWN;
                frame_energy = "";
            }
            else if (name.Equals("list"))
            {
                base.endElement(xpath, uri, local, raw);
                cdo.endObject("Animation");
            }
            else if (name.Equals("molecule"))
            {
                base.endElement(xpath, uri, local, raw);
                cdo.endObject("Frame");
            }
            else
            {
                base.endElement(xpath, uri, local, raw);
            }
        }

        public override void characterData(CMLStack xpath, char[] ch, int start, int length)
        {
            if (current == ENERGY)
            {
                frame_energy = new System.String(ch, start, length);
            }
            else
            {
                base.characterData(xpath, ch, start, length);
            }
        }
    }
}