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
using Support;

namespace Org.OpenScience.CDK.Dict
{
    /// <summary> Dictionary with entries.
    /// 
    /// <p>FIXME: this should be replace by a uptodate Dictionary Schema
    /// DOM type thing.
    /// 
    /// </summary>
    /// <author>      Egon Willighagen
    /// </author>
    /// <cdk.created>     2003-08-23 </cdk.created>
    /// <cdk.keyword>     dictionary </cdk.keyword>
    public class Dictionary
    {
        virtual public Entry[] Entries
        {
            get
            {
                int size = entries.Count;
                Entry[] entryArray = new Entry[size];
                System.Collections.IEnumerator elements = entries.Values.GetEnumerator();
                int counter = 0;
                //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                while (elements.MoveNext() && counter < size)
                {
                    //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                    entryArray[counter] = (Entry)elements.Current;
                    counter++;
                }
                return entryArray;
            }

        }
        virtual public System.String NS
        {
            get
            {
                return ownNS;
            }

            set
            {
                ownNS = value;
            }

        }

        private System.Collections.Hashtable entries;
        private System.String ownNS = null;

        public Dictionary()
        {
            entries = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public static Dictionary unmarshal(System.IO.StreamReader reader)
        {
            //LoggingTool //logger = new LoggingTool(typeof(Dictionary));
            DictionaryHandler handler = new DictionaryHandler();
            XmlSAXDocumentManager parser = null;
            try
            {
                parser = XmlSAXDocumentManager.NewInstance();
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.debug("Using " + parser);
            }
            catch (System.Exception e)
            {
                //logger.error("Could not instantiate any JAXP parser!");
                //logger.debug(e);
            }

            try
            {
                parser.setFeature("http://xml.org/sax/features/validation", false);
                //logger.debug("Deactivated validation");
            }
            //UPGRADE_TODO: Class 'org.xml.sax.SAXException' was converted to 'System.Xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
            catch (System.Xml.XmlException e)
            {
                //logger.warn("Cannot deactivate validation.");
                //logger.debug(e);
            }
            parser.setContentHandler(handler);
            Dictionary dict = null;
            try
            {
                parser.parse(new XmlSourceSupport(reader));
                dict = handler.Dictionary;
            }
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("IOException: " + e.ToString());
                //logger.debug(e);
            }
            //UPGRADE_TODO: Class 'org.xml.sax.SAXException' was converted to 'System.Xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
            catch (System.Xml.XmlException saxe)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("SAXException: " + saxe.GetType().FullName);
                //logger.debug(saxe);
            }
            return dict;
        }

        public virtual void addEntry(Entry entry)
        {
            entries[entry.ID.ToLower()] = entry;
        }

        public virtual bool hasEntry(System.String id)
        {
            return entries.ContainsKey(id);
        }

        public virtual Entry getEntry(System.String id)
        {
            return (Entry)entries[id];
        }

        public virtual int size()
        {
            return entries.Count;
        }
    }
}