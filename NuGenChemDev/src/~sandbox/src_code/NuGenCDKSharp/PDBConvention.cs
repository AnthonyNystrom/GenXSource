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
    /// <summary> Implements the PDB convention used by PDB2CML.
    /// 
    /// <p>This is a lousy implementation, though. Problems that will arise:
    /// <ul>
    /// <li>when this new convention is adopted in the root element no
    /// currentFrame was set. This is done when <list sequence=""> is found
    /// <li>multiple sequences are not yet supported
    /// <li>the frame is now added when the doc is ended, which will result in problems
    /// but work for one sequence files made by PDB2CML v.??
    /// <ul>
    /// 
    /// <p>What is does:
    /// <ul>
    /// <li>work for now
    /// <li>give an idea on the API of the plugable CML import filter
    /// (a real one will be made)
    /// <li>read CML files generated with Steve Zara's PDB 2 CML converter
    /// (of which version 1999 produces invalid CML 1.0)
    /// </ul>
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public class PDBConvention : CMLCoreModule
    {
        private bool connectionTable;
        private bool isELSYM;
        private bool isBond;
        private System.String connect_root;

        public PDBConvention(IChemicalDocumentObject cdo)
            : base(cdo)
        {
        }

        public PDBConvention(ICMLModule conv)
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
            cdo.startObject("Molecule");
        }

        public override void endDocument()
        {
            storeData();
            cdo.endObject("Molecule");
            cdo.endObject("Frame");
            base.endDocument();
        }

        public override void startElement(CMLStack xpath, System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            System.String name = raw;
            isELSYM = false;

            if ("list".Equals(name))
            {

                for (int i = 0; i < atts.GetLength(); i++)
                {

                    if (atts.GetFullName(i).Equals("title") && atts.GetValue(i).Equals("sequence"))
                    {
                    }
                    else if (atts.GetFullName(i).Equals("title") && atts.GetValue(i).Equals("connections"))
                    {
                        // assume that Atom's have been read
                        //logger.debug("Assuming that Atom's have been read: storing them");
                        base.storeAtomData();
                        connectionTable = true;
                        //logger.debug("Start Connection Table");
                    }
                    else if (atts.GetFullName(i).Equals("title") && atts.GetValue(i).Equals("connect"))
                    {
                        //logger.debug("New connection");
                        isBond = true;
                    }
                    else if (atts.GetFullName(i).Equals("id") && isBond)
                    {
                        connect_root = atts.GetValue(i);
                    }

                    // ignore other list items at this moment
                }
            }
            else
            {
                base.startElement(xpath, uri, local, raw, atts);
            }
        }

        public override void endElement(CMLStack xpath, System.String uri, System.String local, System.String raw)
        {

            System.String name = raw;

            if (name.Equals("list") && connectionTable && !isBond)
            {
                //logger.debug("End Connection Table");
                connectionTable = false;
            }

            isELSYM = false;
            isBond = false;
            base.endElement(xpath, uri, local, raw);
        }

        public override void characterData(CMLStack xpath, char[] ch, int start, int length)
        {

            System.String s = new System.String(ch, start, length).Trim();

            if (isELSYM)
            {
                elsym.Add(s);
            }
            else if (isBond)
            {
                //logger.debug("CD (bond): " + s);

                if (connect_root.Length > 0)
                {

                    SupportClass.Tokenizer st = new SupportClass.Tokenizer(s);

                    while (st.HasMoreTokens())
                    {

                        System.String atom = (System.String)st.NextToken();

                        if (!atom.Equals("0"))
                        {
                            //logger.debug("new bond: " + connect_root + "-" + atom);
                            cdo.startObject("Bond");

                            int atom1 = System.Int32.Parse(connect_root) - 1;
                            int atom2 = System.Int32.Parse(atom) - 1;
                            cdo.setObjectProperty("Bond", "atom1", ((System.Int32)atom1).ToString());
                            cdo.setObjectProperty("Bond", "atom2", ((System.Int32)atom2).ToString());
                            cdo.setObjectProperty("Bond", "order", "1");
                            cdo.endObject("Bond");
                        }
                    }
                }
            }
            else
            {
                base.characterData(xpath, ch, start, length);
            }
        }
    }
}