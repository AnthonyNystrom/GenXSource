/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
    /// <author>  Egon Willighagen <elw38@cam.ac.uk>
    /// 
    /// </author>
    /// <cdk.module>  io </cdk.module>
    public class CMLReactionModule : CMLCoreModule
    {
        public CMLReactionModule(IChemicalDocumentObject cdo)
            : base(cdo)
        {
        }

        public CMLReactionModule(ICMLModule conv)
            : base(conv)
        {
            //logger.debug("New CML-Reaction Module!");
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
            if ("reaction".Equals(local))
            {
                cdo.startObject("Reaction");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    System.String value_Renamed = atts.GetValue(i);
                    if (att.Equals("id"))
                    {
                        cdo.setObjectProperty("Reaction", "id", value_Renamed);
                    }
                }
            }
            else if ("reactionList".Equals(local))
            {
                cdo.startObject("SetOfReactions");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    System.String value_Renamed = atts.GetValue(i);
                    if (att.Equals("id"))
                    {
                        cdo.setObjectProperty("SetOfReactions", "id", value_Renamed);
                    }
                }
            }
            else if ("reactant".Equals(local))
            {
                cdo.startObject("Reactant");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    System.String value_Renamed = atts.GetValue(i);
                    if (att.Equals("id"))
                    {
                        cdo.setObjectProperty("Reactant", "id", value_Renamed);
                    }
                }
            }
            else if ("product".Equals(local))
            {
                cdo.startObject("Product");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    System.String value_Renamed = atts.GetValue(i);
                    if (att.Equals("id"))
                    {
                        cdo.setObjectProperty("Product", "id", value_Renamed);
                    }
                }
            }
            else if ("molecule".Equals(local))
            {
                // do nothing for now
                base.newMolecule();
            }
            else
            {
                base.startElement(xpath, uri, local, raw, atts);
            }
        }

        public override void endElement(CMLStack xpath, System.String uri, System.String local, System.String raw)
        {
            if ("reaction".Equals(local))
            {
                cdo.endObject("Reaction");
            }
            else if ("reactionList".Equals(local))
            {
                cdo.endObject("SetOfReactions");
            }
            else if ("reactant".Equals(local))
            {
                cdo.endObject("Reactant");
            }
            else if ("product".Equals(local))
            {
                cdo.endObject("Product");
            }
            else if ("molecule".Equals(local))
            {
                //logger.debug("Storing Molecule");
                base.storeData();
                // do nothing else but store atom/bond information
            }
            else
            {
                base.endElement(xpath, uri, local, raw);
            }
        }

        public override void characterData(CMLStack xpath, char[] ch, int start, int length)
        {
            base.characterData(xpath, ch, start, length);
        }
    }
}