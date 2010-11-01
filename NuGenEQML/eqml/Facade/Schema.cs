using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Facade;

namespace Facade
{
    public class Schema
    {
        public Schema(string MathMLSchema)
        {
            Stream stream = ResourceLoader.GetStream("Binary", MathMLSchema);

            if (stream != null)
            {
                this.schemaSet_ = new XmlSchemaSet();
                this.isValid_ = true;
                this.readOK_ = true;
                this.LoadSchema(MathMLSchema, stream);
            }
        }

        public bool ValidateFile(string file, Encoding encoding)
        {
            this.isValid_ = true;
            if (this.schemaSet_ != null)
            {
                string xml = "";
                try
                {
                    StreamReader reader = new StreamReader(file, encoding);
                    xml = reader.ReadToEnd();
                    reader.Close();
                }
                catch
                {
                    this.isValid_ = false;
                }
                if (xml.Length == 0)
                {
                    this.isValid_ = false;
                }
                else
                {
                    return this.Validate(xml);
                }
            }
            return this.isValid_;
        }

        public bool Validate(string sXML)
        {
            this.isValid_ = true;

            sXML = sXML.Replace("&", "@");
            
            this.errorMessage_ = "";
            this.lineNumber = 0;
            this.linePos = 0;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemaSet_;
            settings.ValidationEventHandler += new ValidationEventHandler(this.ValidateHandler);
            
            XmlReader reader = XmlReader.Create(new StringReader(sXML), settings);
            
            bool foundRoot = false;
            string elname = "";
            string nspace = "";
            try
            {
                while (reader.Read() && this.isValid_)
                {
                    if (foundRoot)
                    {
                        continue;
                    }
                    try
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }
                        
                        elname = reader.Name.ToString();
                        nspace = reader.NamespaceURI.ToString();
                        
                            if (elname != "math" && (elname.IndexOf(":math") == -1))
                            {
                                this.errorMessage_ = this.errorMessage_ + "The document root element [" + elname + "] is not valid.";
                                this.lineNumber = 0;
                                this.linePos = 0;
                                this.isValid_ = false;
                            }
                            if (nspace != "" && nspace != "http://www.w3.org/1998/Math/MathML")
                            {
                                this.errorMessage_ = this.errorMessage_ + "The document root namespace [" + nspace + "] is not valid.";
                                this.lineNumber = 0;
                                this.linePos = 0;
                                this.isValid_ = false;
                            }
                        
                        foundRoot = true;
                    }
                    catch
                    {
                    }
                }
            }
            catch (XmlException ex)
            {
                this.errorMessage_ = this.errorMessage_ + Environment.NewLine + ex.Message + Environment.NewLine;
                this.lineNumber = ex.LineNumber;
                this.linePos = ex.LinePosition;
                this.isValid_ = false;
            }
            reader.Close();
            return this.isValid_;
        }

        private bool LoadSchema(string resourceName, Stream stream)
        {
            XmlTextReader reader = new XmlTextReader(stream);
            try
            {
                XmlSchema schema = XmlSchema.Read(reader, new ValidationEventHandler(this.LoadHandler));
                if (schema != null)
                {
                    this.schemaSet_.Add(schema);
                }
            }
            catch (XmlException xmlException)
            {
                this.readOK_ = false;
                this.errorMessage_ += xmlException.Message + Environment.NewLine + "in: " + resourceName +  " ";
                this.lineNumber = xmlException.LineNumber;
                this.linePos = xmlException.LinePosition;
            }
            catch (XmlSchemaException xmlSchemaException)
            {
                this.readOK_ = false;
                this.errorMessage_ += xmlSchemaException.Message + Environment.NewLine + "in: " + resourceName + " ";
                this.lineNumber = xmlSchemaException.LineNumber;
                this.linePos = xmlSchemaException.LinePosition;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return this.readOK_;
        }

        private void LoadHandler(object sender, ValidationEventArgs arguments)
        {
            try
            {
                this.errorMessage_ = arguments.Message;
                this.lineNumber = arguments.Exception.LineNumber;
                this.linePos = arguments.Exception.LinePosition;
                this.readOK_ = false;
            }
            catch
            {
            }
        }

        private void ValidateHandler(object sender, ValidationEventArgs arguments)
        {
            this.errorMessage_ = arguments.Message;
            try
            {
                this.lineNumber = arguments.Exception.LineNumber;
            }
            catch
            {
            }
            try
            {
                this.linePos = arguments.Exception.LinePosition;
            }
            catch
            {
            }
            this.isValid_ = false;
        }


        public bool IsValid
        {
            get
            {
                if (this.readOK_ && this.isValid_)
                {
                    return false;
                }
                return true;
            }
        }

        public string Message
        {
            get
            {
                return this.errorMessage_;
            }
        }

        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        public int LinePos
        {
            get
            {
                return this.linePos;
            }
        }
        
        private int lineNumber = 0;
        private int linePos = 0;
        private string errorMessage_ = "";
        private bool isValid_ = true;
        private bool readOK_ = true;
        private XmlSchemaSet schemaSet_;
    }
}