/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-02 13:48:44 +0200 (Sun, 02 Jul 2006) $
* $Revision: 6537 $
*
* Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.IO.Formats;
using Support;
using Org.OpenScience.CDK.Interfaces;
using System.IO;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.IO.CML;
using Org.OpenScience.CDK.IO.CML.CDOPI;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads a molecule in CML 1.x and 2.0 format.
    /// CML is an XML based application {@cdk.cite PMR99}, and this Reader
    /// applies the method described in {@cdk.cite WIL01}.
    /// 
    /// </summary>
    /// <author>       Egon L. Willighagen
    /// </author>
    /// <cdk.created>  2001-02-01 </cdk.created>
    /// <cdk.module>   io </cdk.module>
    /// <cdk.keyword>  file format, CML </cdk.keyword>
    /// <cdk.bug>      1085912 </cdk.bug>
    /// <cdk.bug>      1455346 </cdk.bug>
    public class CMLReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new CMLFormat();
            }
        }

        private XmlSAXDocumentManager parser;
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        private System.IO.StreamReader input;
        private System.String url;

        /// <summary> Define this CMLReader to take the input from a java.io.Reader
        /// class. Possible readers are (among others) StringReader and FileReader.
        /// FIXME: this can not be used in combination with Aelfred2 yet.
        /// 
        /// </summary>
        /// <param name="input">Reader type input
        /// 
        /// </param>
        /// <deprecated> XML reading should not be done with a Reader, but with an
        /// InputStream instead.
        /// </deprecated>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public CMLReader(System.IO.StreamReader input)
        {
            this.init();
            this.input = input;
        }

        /// <summary> Reads CML from an java.io.InputStream, for example the FileInputStream.
        /// 
        /// </summary>
        /// <param name="input">InputStream type input
        /// </param>
        public CMLReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public CMLReader()
            : this((StreamReader)null)
        {
        }

        /// <summary> Define this CMLReader to take the input from a java.io.Reader
        /// class. Possible readers are (among others) StringReader and FileReader.
        /// 
        /// </summary>
        /// <param name="url">String url which points to the file to be read
        /// </param>
        public CMLReader(System.String url)
        {
            this.init();
            this.url = url;
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader reader)
        {
            this.input = reader;
        }

        public override void setReader(System.IO.Stream input)
        {
            setReader(new System.IO.StreamReader(input, System.Text.Encoding.Default));
        }

        private void init()
        {
            //logger = new LoggingTool(this);

            url = ""; // make sure it is not null

            bool success = false;
            // If JAXP is prefered (comes with Sun JVM 1.4.0 and higher)
            if (!success)
            {
                try
                {
                    XmlSAXDocumentManager spf = XmlSAXDocumentManager.NewInstance();
                    spf.NamespaceAllowed = true;
                    XmlSAXDocumentManager saxParser = XmlSAXDocumentManager.CloneInstance(spf);
                    //UPGRADE_TODO: Method 'javax.xml.parsers.SAXParser.getXMLReader' was converted to 'SupportClass.XmlSAXDocumentManager' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    parser = saxParser;
                    //logger.info("Using JAXP/SAX XML parser.");
                    success = true;
                }
                catch (System.Exception e)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.warn("Could not instantiate JAXP/SAX XML reader: ", e.Message);
                    //logger.debug(e);
                }
            }
            // Aelfred is first alternative.
            if (!success)
            {
                try
                {
                    //UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
                    //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.loadClass' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                    //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                    parser = new XmlSAXDocumentManager();// (XmlSAXDocumentManager)System.Activator.CreateInstance(this.GetType().getClassLoader().loadClass("gnu.xml.aelfred2.XmlReader"));
                    //logger.info("Using Aelfred2 XML parser.");
                    success = true;
                }
                catch (System.Exception e)
                {
                    //logger.warn("Could not instantiate Aelfred2 XML reader!");
                    //logger.debug(e);
                }
            }
            // Xerces is second alternative
            if (!success)
            {
                try
                {
                    //UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
                    //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.loadClass' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                    //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                    parser = new XmlSAXDocumentManager();//(XmlSAXDocumentManager)System.Activator.CreateInstance(this.GetType().getClassLoader().loadClass("org.apache.xerces.parsers.SAXParser"));
                    //logger.info("Using Xerces XML parser.");
                    success = true;
                }
                catch (System.Exception e)
                {
                    //logger.warn("Could not instantiate Xerces XML reader!");
                    //logger.debug(e);
                }
            }
            if (!success)
            {
                //logger.error("Could not instantiate any XML parser!");
            }
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
            }

            if (typeof(IChemFile).Equals(classObject))
                return true;
            return false;
        }

        /// <summary> Read a IChemObject from input
        /// 
        /// </summary>
        /// <returns> the content in a ChemFile object
        /// </returns>
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                return readChemFile((IChemFile)object_Renamed);
            }
            else
            {
                throw new CDKException("Only supported is reading of ChemFile objects.");
            }
        }

        // private functions

        private IChemFile readChemFile(IChemFile file)
        {
            //logger.debug("Started parsing from input...");
            ChemFileCDO cdo = new ChemFileCDO(file);
            try
            {
                parser.setFeature("http://xml.org/sax/features/validation", false);
                //logger.info("Deactivated validation");
            }
            //UPGRADE_TODO: Class 'org.xml.sax.SAXException' was converted to 'System.Xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
            catch (System.Xml.XmlException e)
            {
                //logger.warn("Cannot deactivate validation.");
                return cdo;
            }
            parser.setContentHandler(new CMLHandler((IChemicalDocumentObject)cdo));
            parser.setEntityResolver(new CMLResolver());
            //UPGRADE_TODO: Method 'org.xml.sax.XMLReader.setErrorHandler' was converted to 'XmlSAXDocumentManager.SetErrorHandler' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_orgxmlsaxXMLReadersetErrorHandler_orgxmlsaxErrorHandler'"
            parser.setErrorHandler(new CMLErrorHandler());
            try
            {
                if (input == null)
                {
                    //logger.debug("Parsing from URL: ", url);
                    parser.parse(url);
                }
                else
                {
                    //logger.debug("Parsing from Reader");
                    input.BaseStream.Seek(0, SeekOrigin.Begin);
                    parser.parse(new XmlSourceSupport(input));
                }
            }
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.String error = "Error while reading file: " + e.Message;
                //logger.error(error);
                //logger.debug(e);
                throw new CDKException(error, e);
            }
            //UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
            catch (System.Xml.XmlException saxe)
            {
                //UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                System.Xml.XmlException spe = (System.Xml.XmlException)saxe;
                System.String error = "Found well-formedness error in line " + spe.LineNumber;
                //logger.error(error);
                //logger.debug(saxe);
                throw new CDKException(error, saxe);
            }
            //UPGRADE_TODO: Class 'org.xml.sax.SAXException' was converted to 'System.Xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
            //catch (System.Xml.XmlException saxe)
            //{
            //    System.String error = "Error while parsing XML: " + saxe.Message;
            //    //logger.error(error);
            //    //logger.debug(saxe);
            //    throw new CDKException(error, saxe);
            //}
            return cdo;
        }

        public override void close()
        {
            input.Close();
        }
    }
}