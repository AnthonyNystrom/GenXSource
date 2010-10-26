/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK.Dict
{
    /// <summary> Class for unmarshalling a dictionary shema file.</summary>
    public class DictionaryHandler : XmlSaxDefaultHandler
    {
        virtual public Dictionary Dictionary
        {
            get
            {
                return dict;
            }

        }

        private bool inEntry = false;
        private bool inMetadataList = false;
        internal Entry entry;

        /// <summary>Used to store all chars between two tags </summary>
        private System.String currentChars;

        internal Dictionary dict;

        public DictionaryHandler()
        {
        }

        public virtual void doctypeDecl(System.String name, System.String publicId, System.String systemId)
        {
        }

        public override void startDocument()
        {
            dict = new Dictionary();
        }

        public override void endElement(System.String uri, System.String local, System.String raw)
        {
            if ("entry".Equals(local) && !"bibtex:entry".Equals(raw) && inEntry)
            {
                dict.addEntry(entry);
                inEntry = false;
            }
            else if ("metadataList".Equals(local) && inMetadataList)
            {
                inMetadataList = false;
            }
        }

        public override void startElement(System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {
            currentChars = "";
            if ("entry".Equals(local) && !"bibtex:entry".Equals(raw) && !inEntry)
            {
                inEntry = true;
                entry = new Entry();
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if (atts.GetFullName(i).Equals("id"))
                    {
                        entry.ID = atts.GetValue(i);
                    }
                    else if (atts.GetFullName(i).Equals("term"))
                    {
                        entry.Label = atts.GetValue(i);
                    }
                }
            }
            if ("metadataList".Equals(local) && !inMetadataList)
            {
                inMetadataList = true;
            }

            // if we're in a metadataList then look at individual
            // metadata nodes and check for any whose content refers
            // to QSAR metadata and save that. Currently it does'nt 
            // differentiate between descriptorType or descriptorClass.
            // Do we need to differentiate?
            //
            // RG: I think so and so I save a combination of the dictRef attribute
            // and the content attribute
            if ("metadata".Equals(local) && inMetadataList)
            {
                for (int i = 0; i < atts.GetLength() - 1; i += 2)
                {

                    System.String dictRefValue = "";
                    if (atts.GetFullName(i).Equals("dictRef"))
                    {
                        dictRefValue = atts.GetValue(i);
                    }
                    if (atts.GetFullName(i + 1).Equals("content"))
                    {
                        System.String content = atts.GetValue(i + 1);
                        if (content.IndexOf("qsar-descriptors-metadata:") == 0)
                        {
                            entry.setDescriptorMetadata(dictRefValue + "/" + content);
                        }
                    }
                }
            }
        }


        public override void characters(System.Char[] ch, int start, int length)
        {
            currentChars += new System.String(ch, start, length);
        }
    }
}