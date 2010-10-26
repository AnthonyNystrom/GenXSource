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
using System.Xml;

namespace Org.OpenScience.CDK.Dict
{
    /// <summary> Dictionary with entries build from an OWL file.
    /// 
    /// </summary>
    /// <author>        Egon Willighagen <egonw@users.sf.net>
    /// </author>
    /// <cdk.created>   2005-11-18 </cdk.created>
    /// <cdk.keyword>   dictionary </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <cdk.depends>   xom-1.0.jar </cdk.depends>
    public class OWLFile : Dictionary
    {
        private static System.String rdfNS = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        private static System.String rdfsNS = "http://www.w3.org/2000/01/rdf-schema#";

        public OWLFile()
            : base()
        {
        }


        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public static new Dictionary unmarshal(System.IO.StreamReader reader)
        {
            //LoggingTool //logger = new LoggingTool(typeof(OWLFile));
            Dictionary dict = new OWLFile();
            try
            {
                //Builder parser = new Builder();
                XmlDocument doc = new XmlDocument();//parser.build(reader);
                doc.Load(reader);
                XmlElement root = (XmlElement)doc.FirstChild;// getRootElement();
                //logger.debug("Found root element: ", root.getQualifiedName());

                // Extract ownNS from root element
                //            final String ownNS = root.getBaseURI();
                //UPGRADE_NOTE: Final was removed from the declaration of 'ownNS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                System.String ownNS = root.BaseURI;// getBaseURI();
                dict.NS = ownNS;

                //logger.debug("Found ontology namespace: ", ownNS);

                // process the defined facts
                XmlNodeList entries = root.ChildNodes;
                //logger.info("Found #elements in OWL dict:", entries.size());
                for (int i = 0; i < entries.Count; i++)
                {
                    XmlElement entry = (XmlElement)entries.Item(i);// get_Renamed(i);
                    if (entry.NamespaceURI.Equals(ownNS))
                    {
                        Entry dbEntry = unmarshal(entry, ownNS);
                        dict.addEntry(dbEntry);
                        //logger.debug("Added entry: ", dbEntry);
                    }
                    else
                    {
                        //logger.debug("Found a non-fact: ", entry.getQualifiedName());
                    }
                }
            }
            //catch (ParsingException ex)
            //{
            //    //logger.error("Dictionary is not well-formed: ", ex.getMessage());
            //    //logger.debug("Error at line " + ex.getLineNumber(), ", column " + ex.getColumnNumber());
            //    dict = null;
            //}
            catch (System.IO.IOException ex)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Due to an IOException, the parser could not check:", ex.Message);
                //logger.debug(ex);
                dict = null;
            }
            return dict;
        }

        public static Entry unmarshal(XmlElement entry, System.String ownNS)
        {
            //LoggingTool //logger = new LoggingTool(typeof(OWLFile));

            // create a new entry by ID
            XmlAttribute id = entry.Attributes["ID", rdfNS];
            //logger.debug("ID: ", id);
            Entry dbEntry = new Entry(id.Value);

            // set additional, optional data
            XmlElement label = (XmlElement)entry.GetElementsByTagName("label", rdfsNS)[0];
            //logger.debug("label: ", label);
            if (label != null)
                dbEntry.Label = label.Value;

            dbEntry.ClassName = entry.Name;
            //logger.debug("class name: ", dbEntry.ClassName);

            XmlElement definition = (XmlElement)entry.GetElementsByTagName("definition", ownNS)[0];
            if (definition != null)
            {
                dbEntry.Definition = definition.Value;
            }
            XmlElement description = (XmlElement)entry.GetElementsByTagName("description", ownNS)[0];
            if (description != null)
            {
                dbEntry.Description = description.Value;
            }

            if (entry.Name.Equals("Descriptor"))
                dbEntry.RawContent = entry;

            return dbEntry;
        }
    }
}