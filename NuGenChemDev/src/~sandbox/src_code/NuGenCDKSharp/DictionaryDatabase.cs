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
using System.Reflection;

namespace Org.OpenScience.CDK.Dict
{
    /// <summary> Database of dictionaries listing entries with compounds, fragments
    /// and entities.
    /// 
    /// </summary>
    /// <author>      Egon Willighagen
    /// </author>
    /// <cdk.created>     2003-04-06 </cdk.created>
    /// <cdk.keyword>     dictionary </cdk.keyword>
    /// <cdk.depends>     xom.jar </cdk.depends>
    public class DictionaryDatabase
    {
        /// <summary> Returns a String[] with the names of the known dictionaries.</summary>
        virtual public System.String[] DictionaryNames
        {
            get
            {
                return dictionaryNames;
            }

        }

        public const System.String DICTREFPROPERTYNAME = "org.openscience.cdk.dict";

        //private LoggingTool //logger;

        private System.String[] dictionaryNames = new System.String[] { "chemical", "elements", "descriptor-algorithms" };
        private System.String[] dictionaryTypes = new System.String[] { "xml", "xml", "owl" };

        private System.Collections.Hashtable dictionaries;

        public DictionaryDatabase()
        {
            //logger = new LoggingTool(this);

            // read dictionaries distributed with CDK
            dictionaries = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            for (int i = 0; i < dictionaryNames.Length; i++)
            {
                System.String name = dictionaryNames[i];
                System.String type = dictionaryTypes[i];
                Dictionary dictionary = readDictionary("org/openscience/cdk/dict/data/" + name, type);
                if (dictionary != null)
                {
                    dictionaries[name.ToLower()] = dictionary;
                    //logger.debug("Read dictionary: ", name);
                }
            }
        }

        private Dictionary readDictionary(System.String databaseLocator, System.String type)
        {
            Dictionary dictionary = null;
            databaseLocator += ("." + type);
            //logger.info("Reading dictionary from ", databaseLocator);
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + databaseLocator));
                    //new System.IO.StreamReader(this.GetType().getClassLoader().getResourceAsStream(databaseLocator), System.Text.Encoding.Default);
                if (type.Equals("owl"))
                {
                    dictionary = OWLFile.unmarshal(reader);
                }
                else
                {
                    // assume XML using Castor
                    dictionary = Dictionary.unmarshal(reader);
                }
            }
            catch (System.Exception exception)
            {
                dictionary = null;
                //logger.error("Could not read dictionary ", databaseLocator);
                //logger.debug(exception);
            }
            return dictionary;
        }

        /// <summary> Reads a custom dictionary into the database.</summary>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public virtual void readDictionary(System.IO.StreamReader reader, System.String name)
        {
            name = name.ToLower();
            //logger.debug("Reading dictionary: ", name);
            if (!dictionaries.ContainsKey(name))
            {
                try
                {
                    Dictionary dictionary = Dictionary.unmarshal(reader);
                    dictionaries[name] = dictionary;
                    //logger.debug("  ... loaded and stored");
                }
                catch (System.Exception exception)
                {
                    //logger.error("Could not read dictionary: ", name);
                    //logger.debug(exception);
                }
            }
            else
            {
                //logger.error("Dictionary already loaded: ", name);
            }
        }

        public virtual Dictionary getDictionary(System.String dictionaryName)
        {
            return (Dictionary)dictionaries[dictionaryName];
        }

        /// <summary> Returns a String[] with the id's of all entries in the specified database.</summary>
        public virtual System.String[] getDictionaryEntries(System.String dictionaryName)
        {
            Dictionary dictionary = getDictionary(dictionaryName);
            if (dictionary == null)
            {
                //logger.error("Cannot find requested dictionary");
                return new System.String[0];
            }
            else
            {
                // FIXME: dummy method that needs an implementation
                Entry[] entries = dictionary.Entries;
                System.String[] entryNames = new System.String[entries.Length];
                //logger.info("Found ", "" + entryNames.Length, " entries in dictionary ", dictionaryName);
                for (int i = 0; i < entries.Length; i++)
                {
                    entryNames[i] = entries[i].Label;
                }
                return entryNames;
            }
        }

        public virtual Entry[] getDictionaryEntry(System.String dictionaryName)
        {
            Dictionary dictionary = (Dictionary)dictionaries[dictionaryName];
            return dictionary.Entries;
        }

        /// <summary> Returns true if the database contains the dictionary.</summary>
        public virtual bool hasDictionary(System.String name)
        {
            return dictionaries.ContainsKey(name.ToLower());
        }

        /// <summary> Returns true if the database contains the dictionary.</summary>
        public virtual System.Collections.IEnumerator listDictionaries()
        {
            return dictionaries.Keys.GetEnumerator();
        }

        /// <summary> Returns true if the given dictionary contains the given
        /// entry.
        /// </summary>
        public virtual bool hasEntry(System.String dictName, System.String entryID)
        {
            if (hasDictionary(dictName))
            {
                Dictionary dictionary = (Dictionary)dictionaries[dictName];
                return dictionary.hasEntry(entryID.ToLower());
            }
            else
            {
                return false;
            }
        }
    }
}