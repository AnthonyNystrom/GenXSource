//
// In order to convert some functionality to Visual C#, the Java Language Conversion Assistant
// creates "support classes" that duplicate the original functionality.  
//
// Support classes replicate the functionality of the original code, but in some cases they are 
// substantially different architecturally. Although every effort is made to preserve the 
// original architecture of the application in the converted project, the user should be aware that 
// the primary goal of these support classes is to replicate functionality, and that at times 
// the architecture of the resulting solution may differ somewhat.
//

using System;
using System.IO;
using System.Xml;

namespace Support
{

    /// <summary>
    /// This interface should be implemented by any class whose instances are intended 
    /// to be executed by a thread.
    /// </summary>
    public interface IThreadRunnable
    {
        /// <summary>
        /// This method has to be implemented in order that starting of the thread causes the object's 
        /// run method to be called in that separately executing thread.
        /// </summary>
        void Run();
    }

    /*******************************/
    /// <summary>
    /// This class will manage all the parsing operations emulating the SAX parser behavior
    /// </summary>
    public class SaxAttributesSupport
    {
        private System.Collections.ArrayList MainList;

        /// <summary>
        /// Builds a new instance of SaxAttributesSupport.
        /// </summary>
        public SaxAttributesSupport()
        {
            MainList = new System.Collections.ArrayList();
        }

        /// <summary>
        /// Creates a new instance of SaxAttributesSupport from an ArrayList of Att_Instance class.
        /// </summary>
        /// <param name="arrayList">An ArraList of Att_Instance class instances.</param>
        /// <returns>A new instance of SaxAttributesSupport</returns>
        public SaxAttributesSupport(SaxAttributesSupport List)
        {
            SaxAttributesSupport temp = new SaxAttributesSupport();
            temp.MainList = (System.Collections.ArrayList)List.MainList.Clone();
        }

        /// <summary>
        /// Adds a new attribute elment to the given SaxAttributesSupport instance.
        /// </summary>
        /// <param name="Uri">The Uri of the attribute to be added.</param>
        /// <param name="Lname">The Local name of the attribute to be added.</param>
        /// <param name="Qname">The Long(qualify) name of the attribute to be added.</param>
        /// <param name="Type">The type of the attribute to be added.</param>
        /// <param name="Value">The value of the attribute to be added.</param>
        public virtual void Add(System.String Uri, System.String Lname, System.String Qname, System.String Type, System.String Value)
        {
            Att_Instance temp_Attributes = new Att_Instance(Uri, Lname, Qname, Type, Value);
            MainList.Add(temp_Attributes);
        }

        /// <summary>
        /// Clears the list of attributes in the given AttributesSupport instance.
        /// </summary>
        public virtual void Clear()
        {
            MainList.Clear();
        }

        /// <summary>
        /// Obtains the index of an attribute of the AttributeSupport from its qualified (long) name.
        /// </summary>
        /// <param name="Qname">The qualified name of the attribute to search.</param>
        /// <returns>An zero-based index of the attribute if it is found, otherwise it returns -1.</returns>
        public virtual int GetIndex(System.String Qname)
        {
            int index = GetLength() - 1;
            while ((index >= 0) && !(((Att_Instance)(MainList[index])).att_fullName.Equals(Qname)))
                index--;
            if (index >= 0)
                return index;
            else
                return -1;
        }

        /// <summary>
        /// Obtains the index of an attribute of the AttributeSupport from its namespace URI and its localname.
        /// </summary>
        /// <param name="Uri">The namespace URI of the attribute to search.</param>
        /// <param name="Lname">The local name of the attribute to search.</param>
        /// <returns>An zero-based index of the attribute if it is found, otherwise it returns -1.</returns>
        public virtual int GetIndex(System.String Uri, System.String Lname)
        {
            int index = GetLength() - 1;
            while ((index >= 0) && !(((Att_Instance)(MainList[index])).att_localName.Equals(Lname) && ((Att_Instance)(MainList[index])).att_URI.Equals(Uri)))
                index--;
            if (index >= 0)
                return index;
            else
                return -1;
        }

        /// <summary>
        /// Returns the number of attributes saved in the SaxAttributesSupport instance.
        /// </summary>
        /// <returns>The number of elements in the given SaxAttributesSupport instance.</returns>
        public virtual int GetLength()
        {
            return MainList.Count;
        }

        /// <summary>
        /// Returns the local name of the attribute in the given SaxAttributesSupport instance that indicates the given index.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <returns>The local name of the attribute indicated by the index or null if the index is out of bounds.</returns>
        public virtual System.String GetLocalName(int index)
        {
            try
            {
                return ((Att_Instance)MainList[index]).att_localName;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the qualified name of the attribute in the given SaxAttributesSupport instance that indicates the given index.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <returns>The qualified name of the attribute indicated by the index or null if the index is out of bounds.</returns>
        public virtual System.String GetFullName(int index)
        {
            try
            {
                return ((Att_Instance)MainList[index]).att_fullName;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the type of the attribute in the given SaxAttributesSupport instance that indicates the given index.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <returns>The type of the attribute indicated by the index or null if the index is out of bounds.</returns>
        public virtual System.String GetType(int index)
        {
            try
            {
                return ((Att_Instance)MainList[index]).att_type;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the namespace URI of the attribute in the given SaxAttributesSupport instance that indicates the given index.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <returns>The namespace URI of the attribute indicated by the index or null if the index is out of bounds.</returns>
        public virtual System.String GetURI(int index)
        {
            try
            {
                return ((Att_Instance)MainList[index]).att_URI;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the value of the attribute in the given SaxAttributesSupport instance that indicates the given index.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <returns>The value of the attribute indicated by the index or null if the index is out of bounds.</returns>
        public virtual System.String GetValue(int index)
        {
            try
            {
                return ((Att_Instance)MainList[index]).att_value;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// Modifies the local name of the attribute in the given SaxAttributesSupport instance.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <param name="LocalName">The new Local name for the attribute.</param>
        public virtual void SetLocalName(int index, System.String LocalName)
        {
            ((Att_Instance)MainList[index]).att_localName = LocalName;
        }

        /// <summary>
        /// Modifies the qualified name of the attribute in the given SaxAttributesSupport instance.
        /// </summary>	
        /// <param name="index">The attribute index.</param>
        /// <param name="FullName">The new qualified name for the attribute.</param>
        public virtual void SetFullName(int index, System.String FullName)
        {
            ((Att_Instance)MainList[index]).att_fullName = FullName;
        }

        /// <summary>
        /// Modifies the type of the attribute in the given SaxAttributesSupport instance.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <param name="Type">The new type for the attribute.</param>
        public virtual void SetType(int index, System.String Type)
        {
            ((Att_Instance)MainList[index]).att_type = Type;
        }

        /// <summary>
        /// Modifies the namespace URI of the attribute in the given SaxAttributesSupport instance.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <param name="URI">The new namespace URI for the attribute.</param>
        public virtual void SetURI(int index, System.String URI)
        {
            ((Att_Instance)MainList[index]).att_URI = URI;
        }

        /// <summary>
        /// Modifies the value of the attribute in the given SaxAttributesSupport instance.
        /// </summary>
        /// <param name="index">The attribute index.</param>
        /// <param name="Value">The new value for the attribute.</param>
        public virtual void SetValue(int index, System.String Value)
        {
            ((Att_Instance)MainList[index]).att_value = Value;
        }

        /// <summary>
        /// This method eliminates the Att_Instance instance at the specified index.
        /// </summary>
        /// <param name="index">The index of the attribute.</param>
        public virtual void RemoveAttribute(int index)
        {
            try
            {
                MainList.RemoveAt(index);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                throw new System.IndexOutOfRangeException("The index is out of range");
            }
        }

        /// <summary>
        /// This method eliminates the Att_Instance instance in the specified index.
        /// </summary>
        /// <param name="indexName">The index name of the attribute.</param>
        public virtual void RemoveAttribute(System.String indexName)
        {
            try
            {
                int pos = GetLength() - 1;
                while ((pos >= 0) && !(((Att_Instance)(MainList[pos])).att_localName.Equals(indexName)))
                    pos--;
                if (pos >= 0)
                    MainList.RemoveAt(pos);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                throw new System.IndexOutOfRangeException("The index is out of range");
            }
        }

        /// <summary>
        /// Replaces an Att_Instance in the given SaxAttributesSupport instance.
        /// </summary>
        /// <param name="index">The index of the attribute.</param>
        /// <param name="Uri">The namespace URI of the new Att_Instance.</param>
        /// <param name="Lname">The local name of the new Att_Instance.</param>
        /// <param name="Qname">The namespace URI of the new Att_Instance.</param>
        /// <param name="Type">The type of the new Att_Instance.</param>
        /// <param name="Value">The value of the new Att_Instance.</param>
        public virtual void SetAttribute(int index, System.String Uri, System.String Lname, System.String Qname, System.String Type, System.String Value)
        {
            MainList[index] = new Att_Instance(Uri, Lname, Qname, Type, Value);
        }

        /// <summary>
        /// Replaces all the list of Att_Instance of the given SaxAttributesSupport instance.
        /// </summary>
        /// <param name="Source">The source SaxAttributesSupport instance.</param>
        public virtual void SetAttributes(SaxAttributesSupport Source)
        {
            MainList = Source.MainList;
        }

        /// <summary>
        /// Returns the type of the Attribute that match with the given qualified name.
        /// </summary>
        /// <param name="Qname">The qualified name of the attribute to search.</param>
        /// <returns>The type of the attribute if it exist otherwise returns null.</returns>
        public virtual System.String GetType(System.String Qname)
        {
            int temp_Index = GetIndex(Qname);
            if (temp_Index != -1)
                return ((Att_Instance)MainList[temp_Index]).att_type;
            else
                return "";
        }

        /// <summary>
        /// Returns the type of the Attribute that match with the given namespace URI and local name.
        /// </summary>
        /// <param name="Uri">The namespace URI of the attribute to search.</param>
        /// <param name="Lname">The local name of the attribute to search.</param>
        /// <returns>The type of the attribute if it exist otherwise returns null.</returns>
        public virtual System.String GetType(System.String Uri, System.String Lname)
        {
            int temp_Index = GetIndex(Uri, Lname);
            if (temp_Index != -1)
                return ((Att_Instance)MainList[temp_Index]).att_type;
            else
                return "";
        }

        /// <summary>
        /// Returns the value of the Attribute that match with the given qualified name.
        /// </summary>
        /// <param name="Qname">The qualified name of the attribute to search.</param>
        /// <returns>The value of the attribute if it exist otherwise returns null.</returns>
        public virtual System.String GetValue(System.String Qname)
        {
            int temp_Index = GetIndex(Qname);
            if (temp_Index != -1)
                return ((Att_Instance)MainList[temp_Index]).att_value;
            else
                return "";
        }

        /// <summary>
        /// Returns the value of the Attribute that match with the given namespace URI and local name.
        /// </summary>
        /// <param name="Uri">The namespace URI of the attribute to search.</param>
        /// <param name="Lname">The local name of the attribute to search.</param>
        /// <returns>The value of the attribute if it exist otherwise returns null.</returns>
        public virtual System.String GetValue(System.String Uri, System.String Lname)
        {
            int temp_Index = GetIndex(Uri, Lname);
            if (temp_Index != -1)
                return ((Att_Instance)MainList[temp_Index]).att_value;
            else
                return "";
        }

        /*******************************/
        /// <summary>
        /// This class is created to save the information of each attributes in the SaxAttributesSupport.
        /// </summary>
        public class Att_Instance
        {
            public System.String att_URI;
            public System.String att_localName;
            public System.String att_fullName;
            public System.String att_type;
            public System.String att_value;

            /// <summary>
            /// This is the constructor of the Att_Instance
            /// </summary>
            /// <param name="Uri">The namespace URI of the attribute</param>
            /// <param name="Lname">The local name of the attribute</param>
            /// <param name="Qname">The long(Qualify) name of attribute</param>
            /// <param name="Type">The type of the attribute</param>
            /// <param name="Value">The value of the attribute</param>
            public Att_Instance(System.String Uri, System.String Lname, System.String Qname, System.String Type, System.String Value)
            {
                this.att_URI = Uri;
                this.att_localName = Lname;
                this.att_fullName = Qname;
                this.att_type = Type;
                this.att_value = Value;
            }
        }
    }

    /*******************************/
    /// <summary>
    /// This interface will manage the Content events of a XML document.
    /// </summary>
    public interface XmlSaxContentHandler
    {
        /// <summary>
        /// This method manage the notification when Characters elements were found.
        /// </summary>
        /// <param name="ch">The array with the characters found.</param>
        /// <param name="start">The index of the first position of the characters found.</param>
        /// <param name="length">Specify how many characters must be read from the array.</param>
        void characters(char[] ch, int start, int length);

        /// <summary>
        /// This method manage the notification when the end document node were found.
        /// </summary>
        void endDocument();

        /// <summary>
        /// This method manage the notification when the end element node was found.
        /// </summary>
        /// <param name="namespaceURI">The namespace URI of the element.</param>
        /// <param name="localName">The local name of the element.</param>
        /// <param name="qName">The long (qualified) name of the element.</param>
        void endElement(System.String namespaceURI, System.String localName, System.String qName);

        /// <summary>
        /// This method manage the event when an area of expecific URI prefix was ended.
        /// </summary>
        /// <param name="prefix">The prefix that ends.</param>
        void endPrefixMapping(System.String prefix);

        /// <summary>
        /// This method manage the event when a ignorable whitespace node was found.
        /// </summary>
        /// <param name="Ch">The array with the ignorable whitespaces.</param>
        /// <param name="Start">The index in the array with the ignorable whitespace.</param>
        /// <param name="Length">The length of the whitespaces.</param>
        void ignorableWhitespace(char[] Ch, int Start, int Length);

        /// <summary>
        /// This method manage the event when a processing instruction was found.
        /// </summary>
        /// <param name="target">The processing instruction target.</param>
        /// <param name="data">The processing instruction data.</param>
        void processingInstruction(System.String target, System.String data);

        /// <summary>
        /// This method is not supported, it is included for compatibility.
        /// </summary>
        void setDocumentLocator(XmlSaxLocator locator);

        /// <summary>
        /// This method manage the event when a skipped entity was found.
        /// </summary>
        /// <param name="name">The name of the skipped entity.</param>
        void skippedEntity(System.String name);

        /// <summary>
        /// This method manage the event when a start document node was found.
        /// </summary>
        void startDocument();

        /// <summary>
        /// This method manage the event when a start element node was found.
        /// </summary>
        /// <param name="namespaceURI">The namespace uri of the element tag.</param>
        /// <param name="localName">The local name of the element.</param>
        /// <param name="qName">The long (qualified) name of the element.</param>
        /// <param name="atts">The list of attributes of the element.</param>
        void startElement(System.String namespaceURI, System.String localName, System.String qName, SaxAttributesSupport atts);

        /// <summary>
        /// This methods indicates the start of a prefix area in the XML document.
        /// </summary>
        /// <param name="prefix">The prefix of the area.</param>
        /// <param name="uri">The namespace URI of the prefix area.</param>
        void startPrefixMapping(System.String prefix, System.String uri);
    }

    /*******************************/
    /// <summary>
    /// This interface will manage errors during the parsing of a XML document.
    /// </summary>
    public interface XmlSaxErrorHandler
    {
        /// <summary>
        /// This method manage an error exception ocurred during the parsing process.
        /// </summary>
        /// <param name="exception">The exception thrown by the parser.</param>
        void error(System.Xml.XmlException exception);

        /// <summary>
        /// This method manage a fatal error exception ocurred during the parsing process.
        /// </summary>
        /// <param name="exception">The exception thrown by the parser.</param>
        void fatalError(System.Xml.XmlException exception);

        /// <summary>
        /// This method manage a warning exception ocurred during the parsing process.
        /// </summary>
        /// <param name="exception">The exception thrown by the parser.</param>
        void warning(System.Xml.XmlException exception);
    }

    /*******************************/
    /// <summary>
    /// Basic interface for resolving entities.
    /// </summary>
    public interface XmlSaxEntityResolver
    {
        /// <summary>
        /// Allow the application to resolve external entities.
        /// </summary>
        /// <param name="publicId">The public identifier of the external entity being referenced, or null if none was supplied.</param>
        /// <param name="systemId">The system identifier of the external entity being referenced.</param>
        /// <returns>A XmlSourceSupport object describing the new input source, or null to request that the parser open a regular URI connection to the system identifier.</returns>
        XmlSourceSupport resolveEntity(System.String publicId, System.String systemId);
    }

    /*******************************/
    /// <summary>
    /// This interface is created to emulate the SAX Locator interface behavior.
    /// </summary>
    public interface XmlSaxLocator
    {
        /// <summary>
        /// This method return the column number where the current document event ends.
        /// </summary>
        /// <returns>The column number where the current document event ends.</returns>
        int getColumnNumber();

        /// <summary>
        /// This method return the line number where the current document event ends.
        /// </summary>
        /// <returns>The line number where the current document event ends.</returns>
        int getLineNumber();

        /// <summary>
        /// This method is not supported, it is included for compatibility.	
        /// </summary>
        /// <returns>The saved public identifier.</returns>
        System.String getPublicId();

        /// <summary>
        /// This method is not supported, it is included for compatibility.		
        /// </summary>
        /// <returns>The saved system identifier.</returns>
        System.String getSystemId();
    }

    /*******************************/
    /// <summary>
    /// This class is used to encapsulate a source of Xml code in an single class.
    /// </summary>
    public class XmlSourceSupport
    {
        private System.IO.Stream bytes;
        private System.IO.StreamReader characters;
        private System.String uri;

        /// <summary>
        /// Constructs an empty XmlSourceSupport instance.
        /// </summary>
        public XmlSourceSupport()
        {
            bytes = null;
            characters = null;
            uri = null;
        }

        /// <summary>
        /// Constructs a XmlSource instance with the specified source System.IO.Stream.
        /// </summary>
        /// <param name="stream">The stream containing the document.</param>
        public XmlSourceSupport(System.IO.Stream stream)
        {
            bytes = stream;
            characters = null;
            uri = null;
        }

        /// <summary>
        /// Constructs a XmlSource instance with the specified source System.IO.StreamReader.
        /// </summary>
        /// <param name="reader">The reader containing the document.</param>
        public XmlSourceSupport(System.IO.StreamReader reader)
        {
            bytes = null;
            characters = reader;
            uri = null;
        }

        /// <summary>
        /// Construct a XmlSource instance with the specified source Uri string.
        /// </summary>
        /// <param name="source">The source containing the document.</param>
        public XmlSourceSupport(System.String source)
        {
            bytes = null;
            characters = null;
            uri = source;
        }

        /// <summary>
        /// Represents the source Stream of the XmlSource.
        /// </summary>
        public System.IO.Stream Bytes
        {
            get
            {
                return bytes;
            }
            set
            {
                bytes = value;
            }
        }

        /// <summary>
        /// Represents the source StreamReader of the XmlSource.
        /// </summary>
        public System.IO.StreamReader Characters
        {
            get
            {
                return characters;
            }
            set
            {
                characters = value;
            }
        }

        /// <summary>
        /// Represents the source URI of the XmlSource.
        /// </summary>
        public System.String Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
            }
        }
    }

    /*******************************/
    /// <summary>
    /// This class provides the base implementation for the management of XML documents parsing.
    /// </summary>
    public class XmlSaxDefaultHandler : XmlSaxContentHandler, XmlSaxErrorHandler, XmlSaxEntityResolver
    {
        /// <summary>
        /// This method manage the notification when Characters element were found.
        /// </summary>
        /// <param name="ch">The array with the characters founds</param>
        /// <param name="start">The index of the first position of the characters found</param>
        /// <param name="length">Specify how many characters must be read from the array</param>
        public virtual void characters(char[] ch, int start, int length)
        {
        }

        /// <summary>
        /// This method manage the notification when the end document node were found
        /// </summary>
        public virtual void endDocument()
        {
        }

        /// <summary>
        /// This method manage the notification when the end element node were found
        /// </summary>
        /// <param name="namespaceURI">The namespace URI of the element</param>
        /// <param name="localName">The local name of the element</param>
        /// <param name="qName">The long name (qualify name) of the element</param>
        public virtual void endElement(System.String uri, System.String localName, System.String qName)
        {
        }

        /// <summary>
        /// This method manage the event when an area of expecific URI prefix was ended.
        /// </summary>
        /// <param name="prefix">The prefix that ends</param>
        public virtual void endPrefixMapping(System.String prefix)
        {
        }

        /// <summary>
        /// This method manage when an error exception ocurrs in the parsing process
        /// </summary>
        /// <param name="exception">The exception throws by the parser</param>
        public virtual void error(System.Xml.XmlException e)
        {
        }

        /// <summary>
        /// This method manage when a fatal error exception ocurrs in the parsing process
        /// </summary>
        /// <param name="exception">The exception Throws by the parser</param>
        public virtual void fatalError(System.Xml.XmlException e)
        {
        }

        /// <summary>
        /// This method manage the event when a ignorable whitespace node were found
        /// </summary>
        /// <param name="Ch">The array with the ignorable whitespaces</param>
        /// <param name="Start">The index in the array with the ignorable whitespace</param>
        /// <param name="Length">The length of the whitespaces</param>
        public virtual void ignorableWhitespace(char[] ch, int start, int length)
        {
        }

        /// <summary>
        /// This method is not supported only is created for compatibility
        /// </summary>
        public virtual void notationDecl(System.String name, System.String publicId, System.String systemId)
        {
        }

        /// <summary>
        /// This method manage the event when a processing instruction were found
        /// </summary>
        /// <param name="target">The processing instruction target</param>
        /// <param name="data">The processing instruction data</param>
        public virtual void processingInstruction(System.String target, System.String data)
        {
        }

        /// <summary>
        /// Allow the application to resolve external entities.
        /// </summary>
        /// <param name="publicId">The public identifier of the external entity being referenced, or null if none was supplied.</param>
        /// <param name="systemId">The system identifier of the external entity being referenced.</param>
        /// <returns>A XmlSourceSupport object describing the new input source, or null to request that the parser open a regular URI connection to the system identifier.</returns>
        public virtual XmlSourceSupport resolveEntity(System.String publicId, System.String systemId)
        {
            return null;
        }

        /// <summary>
        /// This method is not supported, is include for compatibility
        /// </summary>		 
        public virtual void setDocumentLocator(XmlSaxLocator locator)
        {
        }

        /// <summary>
        /// This method manage the event when a skipped entity were found
        /// </summary>
        /// <param name="name">The name of the skipped entity</param>
        public virtual void skippedEntity(System.String name)
        {
        }

        /// <summary>
        /// This method manage the event when a start document node were found 
        /// </summary>
        public virtual void startDocument()
        {
        }

        /// <summary>
        /// This method manage the event when a start element node were found
        /// </summary>
        /// <param name="namespaceURI">The namespace uri of the element tag</param>
        /// <param name="localName">The local name of the element</param>
        /// <param name="qName">The Qualify (long) name of the element</param>
        /// <param name="atts">The list of attributes of the element</param>
        public virtual void startElement(System.String uri, System.String localName, System.String qName, SaxAttributesSupport attributes)
        {
        }

        /// <summary>
        /// This methods indicates the start of a prefix area in the XML document.
        /// </summary>
        /// <param name="prefix">The prefix of the area</param>
        /// <param name="uri">The namespace uri of the prefix area</param>
        public virtual void startPrefixMapping(System.String prefix, System.String uri)
        {
        }

        /// <summary>
        /// This method is not supported only is created for compatibility
        /// </summary>        
        public virtual void unparsedEntityDecl(System.String name, System.String publicId, System.String systemId, System.String notationName)
        {
        }

        /// <summary>
        /// This method manage when a warning exception ocurrs in the parsing process
        /// </summary>
        /// <param name="exception">The exception Throws by the parser</param>
        public virtual void warning(System.Xml.XmlException e)
        {
        }
    }
    /*******************************/
    /// <summary>
    /// This class is created for emulates the SAX LocatorImpl behaviors.
    /// </summary>
    public class XmlSaxLocatorImpl : XmlSaxLocator
    {
        /// <summary>
        /// This method returns a new instance of 'XmlSaxLocatorImpl'.
        /// </summary>
        /// <returns>A new 'XmlSaxLocatorImpl' instance.</returns>
        public XmlSaxLocatorImpl()
        {
        }

        /// <summary>
        /// This method returns a new instance of 'XmlSaxLocatorImpl'.
        /// Create a persistent copy of the current state of a locator.
        /// </summary>
        /// <param name="locator">The current state of a locator.</param>
        /// <returns>A new 'XmlSaxLocatorImpl' instance.</returns>
        public XmlSaxLocatorImpl(XmlSaxLocator locator)
        {
            setPublicId(locator.getPublicId());
            setSystemId(locator.getSystemId());
            setLineNumber(locator.getLineNumber());
            setColumnNumber(locator.getColumnNumber());
        }

        /// <summary>
        /// This method is not supported, it is included for compatibility.
        /// Return the saved public identifier.
        /// </summary>
        /// <returns>The saved public identifier.</returns>
        public virtual System.String getPublicId()
        {
            return publicId;
        }

        /// <summary>
        /// This method is not supported, it is included for compatibility.
        /// Return the saved system identifier.
        /// </summary>
        /// <returns>The saved system identifier.</returns>
        public virtual System.String getSystemId()
        {
            return systemId;
        }

        /// <summary>
        /// Return the saved line number.
        /// </summary>
        /// <returns>The saved line number.</returns>
        public virtual int getLineNumber()
        {
            return lineNumber;
        }

        /// <summary>
        /// Return the saved column number.
        /// </summary>
        /// <returns>The saved column number.</returns>
        public virtual int getColumnNumber()
        {
            return columnNumber;
        }

        /// <summary>
        /// This method is not supported, it is included for compatibility.
        /// Set the public identifier for this locator.
        /// </summary>
        /// <param name="publicId">The new public identifier.</param>
        public virtual void setPublicId(System.String publicId)
        {
            this.publicId = publicId;
        }

        /// <summary>
        /// This method is not supported, it is included for compatibility.
        /// Set the system identifier for this locator.
        /// </summary>
        /// <param name="systemId">The new system identifier.</param>
        public virtual void setSystemId(System.String systemId)
        {
            this.systemId = systemId;
        }

        /// <summary>
        /// Set the line number for this locator.
        /// </summary>
        /// <param name="lineNumber">The line number.</param>
        public virtual void setLineNumber(int lineNumber)
        {
            this.lineNumber = lineNumber;
        }

        /// <summary>
        /// Set the column number for this locator.
        /// </summary>
        /// <param name="columnNumber">The column number.</param>
        public virtual void setColumnNumber(int columnNumber)
        {
            this.columnNumber = columnNumber;
        }

        // Internal state.
        private System.String publicId;
        private System.String systemId;
        private int lineNumber;
        private int columnNumber;
    }

    /*******************************/
    /// <summary>
    /// This interface will manage the Content events of a XML document.
    /// </summary>
    public interface XmlSaxLexicalHandler
    {
        /// <summary>
        /// This method manage the notification when Characters elements were found.
        /// </summary>
        /// <param name="ch">The array with the characters found.</param>
        /// <param name="start">The index of the first position of the characters found.</param>
        /// <param name="length">Specify how many characters must be read from the array.</param>
        void comment(char[] ch, int start, int length);

        /// <summary>
        /// This method manage the notification when the end of a CDATA section were found.
        /// </summary>
        void endCDATA();

        /// <summary>
        /// This method manage the notification when the end of DTD declarations were found.
        /// </summary>
        void endDTD();

        /// <summary>
        /// This method report the end of an entity.
        /// </summary>
        /// <param name="name">The name of the entity that is ending.</param>
        void endEntity(System.String name);

        /// <summary>
        /// This method manage the notification when the start of a CDATA section were found.
        /// </summary>
        void startCDATA();

        /// <summary>
        /// This method manage the notification when the start of DTD declarations were found.
        /// </summary>
        /// <param name="name">The name of the DTD entity.</param>
        /// <param name="publicId">The public identifier.</param>
        /// <param name="systemId">The system identifier.</param>
        void startDTD(System.String name, System.String publicId, System.String systemId);

        /// <summary>
        /// This method report the start of an entity.
        /// </summary>
        /// <param name="name">The name of the entity that is ending.</param>
        void startEntity(System.String name);
    }

    /*******************************/
    /// <summary>
    /// This exception is thrown by the XmlSaxDocumentManager in the SetProperty and SetFeature 
    /// methods if a property or method couldn't be found.
    /// </summary>
    public class ManagerNotRecognizedException : System.Exception
    {
        /// <summary>
        /// Creates a new ManagerNotRecognizedException with the message specified.
        /// </summary>
        /// <param name="Message">Error message of the exception.</param>
        public ManagerNotRecognizedException(System.String Message)
            : base(Message)
        {
        }
    }

    /*******************************/
    /// <summary>
    /// This exception is thrown by the XmlSaxDocumentManager in the SetProperty and SetFeature methods 
    /// if a property or method couldn't be supported.
    /// </summary>
    public class ManagerNotSupportedException : System.Exception
    {
        /// <summary>
        /// Creates a new ManagerNotSupportedException with the message specified.
        /// </summary>
        /// <param name="Message">Error message of the exception.</param>
        public ManagerNotSupportedException(System.String Message)
            : base(Message)
        {
        }
    }

    /*******************************/
    /// <summary>
    /// This class provides the base implementation for the management of XML documents parsing.
    /// </summary>
    public class XmlSaxParserAdapter : XmlSAXDocumentManager, XmlSaxContentHandler
    {

        /// <summary>
        /// This method manage the notification when Characters element were found.
        /// </summary>
        /// <param name="ch">The array with the characters founds</param>
        /// <param name="start">The index of the first position of the characters found</param>
        /// <param name="length">Specify how many characters must be read from the array</param>
        public virtual void characters(char[] ch, int start, int length) { }

        /// <summary>
        /// This method manage the notification when the end document node were found
        /// </summary>
        public virtual void endDocument() { }

        /// <summary>
        /// This method manage the notification when the end element node were found
        /// </summary>
        /// <param name="namespaceURI">The namespace URI of the element</param>
        /// <param name="localName">The local name of the element</param>
        /// <param name="qName">The long name (qualify name) of the element</param>
        public virtual void endElement(System.String namespaceURI, System.String localName, System.String qName) { }

        /// <summary>
        /// This method manage the event when an area of expecific URI prefix was ended.
        /// </summary>
        /// <param name="prefix">The prefix that ends.</param>
        public virtual void endPrefixMapping(System.String prefix) { }

        /// <summary>
        /// This method manage the event when a ignorable whitespace node were found
        /// </summary>
        /// <param name="ch">The array with the ignorable whitespaces</param>
        /// <param name="start">The index in the array with the ignorable whitespace</param>
        /// <param name="length">The length of the whitespaces</param>
        public virtual void ignorableWhitespace(char[] ch, int start, int length) { }

        /// <summary>
        /// This method manage the event when a processing instruction were found
        /// </summary>
        /// <param name="target">The processing instruction target</param>
        /// <param name="data">The processing instruction data</param>
        public virtual void processingInstruction(System.String target, System.String data) { }

        /// <summary>
        /// Receive an object for locating the origin of events into the XML document
        /// </summary>
        /// <param name="locator">A 'XmlSaxLocator' object that can return the location of any events into the XML document</param>
        public virtual void setDocumentLocator(XmlSaxLocator locator) { }

        /// <summary>
        /// This method manage the event when a skipped entity was found.
        /// </summary>
        /// <param name="name">The name of the skipped entity.</param>
        public virtual void skippedEntity(System.String name) { }

        /// <summary>
        /// This method manage the event when a start document node were found 
        /// </summary>
        public virtual void startDocument() { }

        /// <summary>
        /// This method manage the event when a start element node were found
        /// </summary>
        /// <param name="namespaceURI">The namespace uri of the element tag</param>
        /// <param name="localName">The local name of the element</param>
        /// <param name="qName">The Qualify (long) name of the element</param>
        /// <param name="qAtts">The list of attributes of the element</param>
        public virtual void startElement(System.String namespaceURI, System.String localName, System.String qName, SaxAttributesSupport qAtts) { }

        /// <summary>
        /// This methods indicates the start of a prefix area in the XML document.
        /// </summary>
        /// <param name="prefix">The prefix of the area.</param>
        /// <param name="uri">The namespace URI of the prefix area.</param>
        public virtual void startPrefixMapping(System.String prefix, System.String uri) { }

    }


    /*******************************/
    /// <summary>
    /// Emulates the SAX parsers behaviours.
    /// </summary>
    public class XmlSAXDocumentManager
    {
        protected bool isValidating;
        protected bool namespaceAllowed;
        protected System.Xml.XmlValidatingReader reader;
        protected XmlSaxContentHandler callBackHandler;
        protected XmlSaxErrorHandler errorHandler;
        protected XmlSaxLocatorImpl locator;
        protected XmlSaxLexicalHandler lexical;
        protected XmlSaxEntityResolver entityResolver;
        protected System.String parserFileName;

        /// <summary>
        /// Public constructor for the class.
        /// </summary>
        public XmlSAXDocumentManager()
        {
            isValidating = false;
            namespaceAllowed = false;
            reader = null;
            callBackHandler = null;
            errorHandler = null;
            locator = null;
            lexical = null;
            entityResolver = null;
            parserFileName = "";
        }

        /// <summary>
        /// Returns a new instance of 'XmlSAXDocumentManager'.
        /// </summary>
        /// <returns>A new 'XmlSAXDocumentManager' instance.</returns>
        public static XmlSAXDocumentManager NewInstance()
        {
            return new XmlSAXDocumentManager();
        }

        /// <summary>
        /// Returns a clone instance of 'XmlSAXDocumentManager'.
        /// </summary>
        /// <returns>A clone 'XmlSAXDocumentManager' instance.</returns>
        public static XmlSAXDocumentManager CloneInstance(XmlSAXDocumentManager instance)
        {
            XmlSAXDocumentManager temp = new XmlSAXDocumentManager();
            temp.NamespaceAllowed = instance.NamespaceAllowed;
            temp.isValidating = instance.isValidating;
            XmlSaxContentHandler contentHandler = instance.getContentHandler();
            if (contentHandler != null)
                temp.setContentHandler(contentHandler);
            XmlSaxErrorHandler errorHandler = instance.getErrorHandler();
            if (errorHandler != null)
                temp.setErrorHandler(errorHandler);
            temp.setFeature("http://xml.org/sax/features/namespaces", instance.getFeature("http://xml.org/sax/features/namespaces"));
            temp.setFeature("http://xml.org/sax/features/namespace-prefixes", instance.getFeature("http://xml.org/sax/features/namespace-prefixes"));
            temp.setFeature("http://xml.org/sax/features/validation", instance.getFeature("http://xml.org/sax/features/validation"));
            temp.setProperty("http://xml.org/sax/properties/lexical-handler", instance.getProperty("http://xml.org/sax/properties/lexical-handler"));
            temp.parserFileName = instance.parserFileName;
            return temp;
        }

        /// <summary>
        /// Indicates whether the 'XmlSAXDocumentManager' are validating the XML over a DTD.
        /// </summary>
        public bool IsValidating
        {
            get
            {
                return isValidating;
            }
            set
            {
                isValidating = value;
            }
        }

        /// <summary>
        /// Indicates whether the 'XmlSAXDocumentManager' manager allows namespaces.
        /// </summary>
        public bool NamespaceAllowed
        {
            get
            {
                return namespaceAllowed;
            }
            set
            {
                namespaceAllowed = value;
            }
        }

        /// <summary>
        /// Emulates the behaviour of a SAX LocatorImpl object.
        /// </summary>
        /// <param name="locator">The 'XmlSaxLocatorImpl' instance to assing the document location.</param>
        /// <param name="textReader">The XML document instance to be used.</param>
        private void UpdateLocatorData(XmlSaxLocatorImpl locator, System.Xml.XmlTextReader textReader)
        {
            if ((locator != null) && (textReader != null))
            {
                locator.setColumnNumber(textReader.LinePosition);
                locator.setLineNumber(textReader.LineNumber);
                locator.setSystemId(parserFileName);
            }
        }

        /// <summary>
        /// Emulates the behavior of a SAX parsers. Set the value of a feature.
        /// </summary>
        /// <param name="name">The feature name, which is a fully-qualified URI.</param>
        /// <param name="value">The requested value for the feature.</param>
        public virtual void setFeature(System.String name, bool value)
        {
            switch (name)
            {
                case "http://xml.org/sax/features/namespaces":
                    {
                        try
                        {
                            this.NamespaceAllowed = value;
                            break;
                        }
                        catch
                        {
                            throw new ManagerNotSupportedException("The specified operation was not performed");
                        }
                    }
                case "http://xml.org/sax/features/namespace-prefixes":
                    {
                        try
                        {
                            this.NamespaceAllowed = value;
                            break;
                        }
                        catch
                        {
                            throw new ManagerNotSupportedException("The specified operation was not performed");
                        }
                    }
                case "http://xml.org/sax/features/validation":
                    {
                        try
                        {
                            this.isValidating = value;
                            break;
                        }
                        catch
                        {
                            throw new ManagerNotSupportedException("The specified operation was not performed");
                        }
                    }
                default:
                    throw new ManagerNotRecognizedException("The specified feature: " + name + " are not supported");
            }
        }

        /// <summary>
        /// Emulates the behavior of a SAX parsers. Gets the value of a feature.
        /// </summary>
        /// <param name="name">The feature name, which is a fully-qualified URI.</param>
        /// <returns>The requested value for the feature.</returns>
        public virtual bool getFeature(System.String name)
        {
            switch (name)
            {
                case "http://xml.org/sax/features/namespaces":
                    {
                        try
                        {
                            return this.NamespaceAllowed;
                        }
                        catch
                        {
                            throw new ManagerNotSupportedException("The specified operation was not performed");
                        }
                    }
                case "http://xml.org/sax/features/namespace-prefixes":
                    {
                        try
                        {
                            return this.NamespaceAllowed;
                        }
                        catch
                        {
                            throw new ManagerNotSupportedException("The specified operation was not performed");
                        }
                    }
                case "http://xml.org/sax/features/validation":
                    {
                        try
                        {
                            return this.isValidating;
                        }
                        catch
                        {
                            throw new ManagerNotSupportedException("The specified operation was not performed");
                        }
                    }
                default:
                    throw new ManagerNotRecognizedException("The specified feature: " + name + " are not supported");
            }
        }

        /// <summary>
        /// Emulates the behavior of a SAX parsers. Sets the value of a property.
        /// </summary>
        /// <param name="name">The property name, which is a fully-qualified URI.</param>
        /// <param name="value">The requested value for the property.</param>
        public virtual void setProperty(System.String name, System.Object value)
        {
            switch (name)
            {
                case "http://xml.org/sax/properties/lexical-handler":
                    {
                        try
                        {
                            lexical = (XmlSaxLexicalHandler)value;
                            break;
                        }
                        catch (System.Exception e)
                        {
                            throw new ManagerNotSupportedException("The property is not supported as an internal exception was thrown when trying to set it: " + e.Message);
                        }
                    }
                default:
                    throw new ManagerNotRecognizedException("The specified feature: " + name + " is not recognized");
            }
        }

        /// <summary>
        /// Emulates the behavior of a SAX parsers. Gets the value of a property.
        /// </summary>
        /// <param name="name">The property name, which is a fully-qualified URI.</param>
        /// <returns>The requested value for the property.</returns>
        public virtual System.Object getProperty(System.String name)
        {
            switch (name)
            {
                case "http://xml.org/sax/properties/lexical-handler":
                    {
                        try
                        {
                            return this.lexical;
                        }
                        catch
                        {
                            throw new ManagerNotSupportedException("The specified operation was not performed");
                        }
                    }
                default:
                    throw new ManagerNotRecognizedException("The specified feature: " + name + " are not supported");
            }
        }

        /// <summary>
        /// Emulates the behavior of a SAX parser, it realizes the callback events of the parser.
        /// </summary>
        private void DoParsing()
        {
            System.Collections.Hashtable prefixes = new System.Collections.Hashtable();
            System.Collections.Stack stackNameSpace = new System.Collections.Stack();
            locator = new XmlSaxLocatorImpl();
            try
            {
                UpdateLocatorData(this.locator, (System.Xml.XmlTextReader)(this.reader.Reader));
                if (this.callBackHandler != null)
                    this.callBackHandler.setDocumentLocator(locator);
                if (this.callBackHandler != null)
                    this.callBackHandler.startDocument();
                while (this.reader.Read())
                {
                    UpdateLocatorData(this.locator, (System.Xml.XmlTextReader)(this.reader.Reader));
                    switch (this.reader.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            bool Empty = reader.IsEmptyElement;
                            System.String namespaceURI = "";
                            System.String localName = "";
                            if (this.namespaceAllowed)
                            {
                                namespaceURI = reader.NamespaceURI;
                                localName = reader.LocalName;
                            }
                            System.String name = reader.Name;
                            SaxAttributesSupport attributes = new SaxAttributesSupport();
                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    System.String prefixName = (reader.Name.IndexOf(":") > 0) ? reader.Name.Substring(reader.Name.IndexOf(":") + 1, reader.Name.Length - reader.Name.IndexOf(":") - 1) : "";
                                    System.String prefix = (reader.Name.IndexOf(":") > 0) ? reader.Name.Substring(0, reader.Name.IndexOf(":")) : reader.Name;
                                    bool IsXmlns = prefix.ToLower().Equals("xmlns");
                                    if (this.namespaceAllowed)
                                    {
                                        if (!IsXmlns)
                                            attributes.Add(reader.NamespaceURI, reader.LocalName, reader.Name, "" + reader.NodeType, reader.Value);
                                    }
                                    else
                                        attributes.Add("", "", reader.Name, "" + reader.NodeType, reader.Value);
                                    if (IsXmlns)
                                    {
                                        System.String namespaceTemp = "";
                                        namespaceTemp = (namespaceURI.Length == 0) ? reader.Value : namespaceURI;
                                        if (this.namespaceAllowed && !prefixes.ContainsKey(namespaceTemp) && namespaceTemp.Length > 0)
                                        {
                                            stackNameSpace.Push(name);
                                            System.Collections.Stack namespaceStack = new System.Collections.Stack();
                                            namespaceStack.Push(prefixName);
                                            prefixes.Add(namespaceURI, namespaceStack);
                                            if (this.callBackHandler != null)
                                                ((XmlSaxContentHandler)this.callBackHandler).startPrefixMapping(prefixName, namespaceTemp);
                                        }
                                        else
                                        {
                                            if (this.namespaceAllowed && namespaceTemp.Length > 0 && !((System.Collections.Stack)prefixes[namespaceTemp]).Contains(reader.Name))
                                            {
                                                ((System.Collections.Stack)prefixes[namespaceURI]).Push(prefixName);
                                                if (this.callBackHandler != null)
                                                    ((XmlSaxContentHandler)this.callBackHandler).startPrefixMapping(prefixName, reader.Value);
                                            }
                                        }
                                    }
                                }
                            }
                            if (this.callBackHandler != null)
                                this.callBackHandler.startElement(namespaceURI, localName, name, attributes);
                            if (Empty)
                            {
                                if (this.NamespaceAllowed)
                                {
                                    if (this.callBackHandler != null)
                                        this.callBackHandler.endElement(namespaceURI, localName, name);
                                }
                                else
                                    if (this.callBackHandler != null)
                                        this.callBackHandler.endElement("", "", name);
                            }
                            break;

                        case System.Xml.XmlNodeType.EndElement:
                            if (this.namespaceAllowed)
                            {
                                if (this.callBackHandler != null)
                                    this.callBackHandler.endElement(reader.NamespaceURI, reader.LocalName, reader.Name);
                            }
                            else
                                if (this.callBackHandler != null)
                                    this.callBackHandler.endElement("", "", reader.Name);
                            if (this.namespaceAllowed && prefixes.ContainsKey(reader.NamespaceURI) && ((System.Collections.Stack)stackNameSpace).Contains(reader.Name))
                            {
                                stackNameSpace.Pop();
                                System.Collections.Stack namespaceStack = (System.Collections.Stack)prefixes[reader.NamespaceURI];
                                while (namespaceStack.Count > 0)
                                {
                                    System.String tempString = (System.String)namespaceStack.Pop();
                                    if (this.callBackHandler != null)
                                        ((XmlSaxContentHandler)this.callBackHandler).endPrefixMapping(tempString);
                                }
                                prefixes.Remove(reader.NamespaceURI);
                            }
                            break;

                        case System.Xml.XmlNodeType.Text:
                            if (this.callBackHandler != null)
                                this.callBackHandler.characters(reader.Value.ToCharArray(), 0, reader.Value.Length);
                            break;

                        case System.Xml.XmlNodeType.Whitespace:
                            if (this.callBackHandler != null)
                                this.callBackHandler.ignorableWhitespace(reader.Value.ToCharArray(), 0, reader.Value.Length);
                            break;

                        case System.Xml.XmlNodeType.ProcessingInstruction:
                            if (this.callBackHandler != null)
                                this.callBackHandler.processingInstruction(reader.Name, reader.Value);
                            break;

                        case System.Xml.XmlNodeType.Comment:
                            if (this.lexical != null)
                                this.lexical.comment(reader.Value.ToCharArray(), 0, reader.Value.Length);
                            break;

                        case System.Xml.XmlNodeType.CDATA:
                            if (this.lexical != null)
                            {
                                lexical.startCDATA();
                                if (this.callBackHandler != null)
                                    this.callBackHandler.characters(this.reader.Value.ToCharArray(), 0, this.reader.Value.ToCharArray().Length);
                                lexical.endCDATA();
                            }
                            break;

                        case System.Xml.XmlNodeType.DocumentType:
                            if (this.lexical != null)
                            {
                                System.String lname = this.reader.Name;
                                System.String systemId = null;
                                if (this.reader.Reader.AttributeCount > 0)
                                    systemId = this.reader.Reader.GetAttribute(0);
                                this.lexical.startDTD(lname, null, systemId);
                                this.lexical.startEntity("[dtd]");
                                this.lexical.endEntity("[dtd]");
                                this.lexical.endDTD();
                            }
                            break;
                    }
                }
                if (this.callBackHandler != null)
                    this.callBackHandler.endDocument();
            }
            catch (System.Xml.XmlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified file and process the events over the specified handler.
        /// </summary>
        /// <param name="filepath">The file to be used.</param>
        /// <param name="handler">The handler that manages the parser events.</param>
        public virtual void parse(System.IO.FileInfo filepath, XmlSaxContentHandler handler)
        {
            try
            {
                if (handler is XmlSaxDefaultHandler)
                {
                    this.errorHandler = (XmlSaxDefaultHandler)handler;
                    this.entityResolver = (XmlSaxDefaultHandler)handler;
                }
                if (!(this is XmlSaxParserAdapter))
                    this.callBackHandler = handler;
                else
                {
                    if (this.callBackHandler == null)
                        this.callBackHandler = handler;
                }
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(filepath.OpenRead());
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = filepath.FullName;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified file path and process the events over the specified handler.
        /// </summary>
        /// <param name="filepath">The path of the file to be used.</param>
        /// <param name="handler">The handler that manage the parser events.</param>
        public virtual void parse(System.String filepath, XmlSaxContentHandler handler)
        {
            try
            {
                if (handler is XmlSaxDefaultHandler)
                {
                    this.errorHandler = (XmlSaxDefaultHandler)handler;
                    this.entityResolver = (XmlSaxDefaultHandler)handler;
                }
                if (!(this is XmlSaxParserAdapter))
                    this.callBackHandler = handler;
                else
                {
                    if (this.callBackHandler == null)
                        this.callBackHandler = handler;
                }
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(filepath);
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = filepath;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified stream and process the events over the specified handler.
        /// </summary>
        /// <param name="stream">The stream with the XML.</param>
        /// <param name="handler">The handler that manage the parser events.</param>
        public virtual void parse(System.IO.Stream stream, XmlSaxContentHandler handler)
        {
            try
            {
                if (handler is XmlSaxDefaultHandler)
                {
                    this.errorHandler = (XmlSaxDefaultHandler)handler;
                    this.entityResolver = (XmlSaxDefaultHandler)handler;
                }
                if (!(this is XmlSaxParserAdapter))
                    this.callBackHandler = handler;
                else
                {
                    if (this.callBackHandler == null)
                        this.callBackHandler = handler;
                }
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(stream);
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = null;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified stream and process the events over the specified handler, and resolves the 
        /// entities with the specified URI.
        /// </summary>
        /// <param name="stream">The stream with the XML.</param>
        /// <param name="handler">The handler that manage the parser events.</param>
        /// <param name="URI">The namespace URI for resolve external etities.</param>
        public virtual void parse(System.IO.Stream stream, XmlSaxContentHandler handler, System.String URI)
        {
            try
            {
                if (handler is XmlSaxDefaultHandler)
                {
                    this.errorHandler = (XmlSaxDefaultHandler)handler;
                    this.entityResolver = (XmlSaxDefaultHandler)handler;
                }
                if (!(this is XmlSaxParserAdapter))
                    this.callBackHandler = handler;
                else
                {
                    if (this.callBackHandler == null)
                        this.callBackHandler = handler;
                }
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(URI, stream);
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = null;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified 'XmlSourceSupport' instance and process the events over the specified handler, 
        /// and resolves the entities with the specified URI.
        /// </summary>
        /// <param name="source">The 'XmlSourceSupport' that contains the XML.</param>
        /// <param name="handler">The handler that manages the parser events.</param>
        public virtual void parse(XmlSourceSupport source, XmlSaxContentHandler handler)
        {
            if (source.Characters != null)
                parse(source.Characters.BaseStream, handler);
            else
            {
                if (source.Bytes != null)
                    parse(source.Bytes, handler);
                else
                {
                    if (source.Uri != null)
                        parse(source.Uri, handler);
                    else
                        throw new System.Xml.XmlException("The XmlSource class can't be null");
                }
            }
        }

        /// <summary>
        /// Parses the specified file and process the events over previously specified handler.
        /// </summary>
        /// <param name="filepath">The file with the XML.</param>
        public virtual void parse(System.IO.FileInfo filepath)
        {
            try
            {
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(filepath.OpenRead());
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = filepath.FullName;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified file path and processes the events over previously specified handler.
        /// </summary>
        /// <param name="filepath">The path of the file with the XML.</param>
        public virtual void parse(System.String filepath)
        {
            try
            {
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(filepath);
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = filepath;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified stream and process the events over previusly specified handler.
        /// </summary>
        /// <param name="stream">The stream with the XML.</param>
        public virtual void parse(System.IO.Stream stream)
        {
            try
            {
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(stream);
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = null;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified stream and process the events over previusly specified handler.
        /// </summary>
        /// <param name="stream">The stream with the XML.</param>
        public virtual void parse(StreamReader stream)
        {
            try
            {
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(stream);
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = null;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified stream and processes the events over previously specified handler, and resolves the 
        /// external entities with the specified URI.
        /// </summary>
        /// <param name="stream">The stream with the XML.</param>
        /// <param name="URI">The namespace URI for resolve external etities.</param>
        public virtual void parse(System.IO.Stream stream, System.String URI)
        {
            try
            {
                System.Xml.XmlTextReader tempTextReader = new System.Xml.XmlTextReader(URI, stream);
                System.Xml.XmlValidatingReader tempValidatingReader = new System.Xml.XmlValidatingReader(tempTextReader);
                parserFileName = null;
                tempValidatingReader.ValidationType = (this.isValidating) ? System.Xml.ValidationType.DTD : System.Xml.ValidationType.None;
                tempValidatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);
                this.reader = tempValidatingReader;
                this.DoParsing();
            }
            catch (System.Xml.XmlException e)
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(e);
                throw e;
            }
        }

        /// <summary>
        /// Parses the specified 'XmlSourceSupport' and processes the events over the specified handler, and 
        /// resolves the entities with the specified URI.
        /// </summary>
        /// <param name="source">The 'XmlSourceSupport' instance with the XML.</param>
        public virtual void parse(XmlSourceSupport source)
        {
            if (source.Characters != null)
                parse(source.Characters);
            else
            {
                if (source.Bytes != null)
                    parse(source.Bytes);
                else
                {
                    if (source.Uri != null)
                        parse(source.Uri);
                    else
                        throw new System.Xml.XmlException("The XmlSource class can't be null");
                }
            }
        }

        /// <summary>
        /// Manages all the exceptions that were thrown when the validation over XML fails.
        /// </summary>
        public void ValidationEventHandle(System.Object sender, System.Xml.Schema.ValidationEventArgs args)
        {
            System.Xml.Schema.XmlSchemaException tempException = args.Exception;
            if (args.Severity == System.Xml.Schema.XmlSeverityType.Warning)
            {
                if (this.errorHandler != null)
                    this.errorHandler.warning(new System.Xml.XmlException(tempException.Message, tempException, tempException.LineNumber, tempException.LinePosition));
            }
            else
            {
                if (this.errorHandler != null)
                    this.errorHandler.fatalError(new System.Xml.XmlException(tempException.Message, tempException, tempException.LineNumber, tempException.LinePosition));
            }
        }

        /// <summary>
        /// Assigns the object that will handle all the content events.
        /// </summary>
        /// <param name="handler">The object that handles the content events.</param>
        public virtual void setContentHandler(XmlSaxContentHandler handler)
        {
            if (handler != null)
                this.callBackHandler = handler;
            else
                throw new System.Xml.XmlException("Error assigning the Content handler: a null Content Handler was received");
        }

        /// <summary>
        /// Assigns the object that will handle all the error events. 
        /// </summary>
        /// <param name="handler">The object that handles the errors events.</param>
        public virtual void setErrorHandler(XmlSaxErrorHandler handler)
        {
            if (handler != null)
                this.errorHandler = handler;
            else
                throw new System.Xml.XmlException("Error assigning the Error handler: a null Error Handler was received");
        }

        /// <summary>
        /// Obtains the object that will handle all the content events.
        /// </summary>
        /// <returns>The object that handles the content events.</returns>
        public virtual XmlSaxContentHandler getContentHandler()
        {
            return this.callBackHandler;
        }

        /// <summary>
        /// Assigns the object that will handle all the error events. 
        /// </summary>
        /// <returns>The object that handles the error events.</returns>
        public virtual XmlSaxErrorHandler getErrorHandler()
        {
            return this.errorHandler;
        }

        /// <summary>
        /// Returns the current entity resolver.
        /// </summary>
        /// <returns>The current entity resolver, or null if none has been registered.</returns>
        public virtual XmlSaxEntityResolver getEntityResolver()
        {
            return this.entityResolver;
        }

        /// <summary>
        /// Allows an application to register an entity resolver.
        /// </summary>
        /// <param name="resolver">The entity resolver.</param>
        public virtual void setEntityResolver(XmlSaxEntityResolver resolver)
        {
            this.entityResolver = resolver;
        }
    }

    /// <summary>
    /// Contains conversion support elements such as classes, interfaces and static methods.
    /// </summary>
    public class SupportClass
    {
        /// <summary>
        /// The class performs token processing in strings
        /// </summary>
        public class Tokenizer : System.Collections.IEnumerator
        {
            /// Position over the string
            private long currentPos = 0;

            /// Include demiliters in the results.
            private bool includeDelims = false;

            /// Char representation of the String to tokenize.
            private char[] chars = null;

            //The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character and the form-feed character
            private string delimiters = " \t\n\r\f";

            /// <summary>
            /// Initializes a new class instance with a specified string to process
            /// </summary>
            /// <param name="source">String to tokenize</param>
            public Tokenizer(System.String source)
            {
                this.chars = source.ToCharArray();
            }

            /// <summary>
            /// Initializes a new class instance with a specified string to process
            /// and the specified token delimiters to use
            /// </summary>
            /// <param name="source">String to tokenize</param>
            /// <param name="delimiters">String containing the delimiters</param>
            public Tokenizer(System.String source, System.String delimiters)
                : this(source)
            {
                this.delimiters = delimiters;
            }


            /// <summary>
            /// Initializes a new class instance with a specified string to process, the specified token 
            /// delimiters to use, and whether the delimiters must be included in the results.
            /// </summary>
            /// <param name="source">String to tokenize</param>
            /// <param name="delimiters">String containing the delimiters</param>
            /// <param name="includeDelims">Determines if delimiters are included in the results.</param>
            public Tokenizer(System.String source, System.String delimiters, bool includeDelims)
                : this(source, delimiters)
            {
                this.includeDelims = includeDelims;
            }


            /// <summary>
            /// Returns the next token from the token list
            /// </summary>
            /// <returns>The string value of the token</returns>
            public System.String NextToken()
            {
                return NextToken(this.delimiters);
            }

            /// <summary>
            /// Returns the next token from the source string, using the provided
            /// token delimiters
            /// </summary>
            /// <param name="delimiters">String containing the delimiters to use</param>
            /// <returns>The string value of the token</returns>
            public System.String NextToken(System.String delimiters)
            {
                //According to documentation, the usage of the received delimiters should be temporary (only for this call).
                //However, it seems it is not true, so the following line is necessary.
                this.delimiters = delimiters;

                //at the end 
                if (this.currentPos == this.chars.Length)
                    throw new System.ArgumentOutOfRangeException();
                //if over a delimiter and delimiters must be returned
                else if ((System.Array.IndexOf(delimiters.ToCharArray(), chars[this.currentPos]) != -1)
                         && this.includeDelims)
                    return "" + this.chars[this.currentPos++];
                //need to get the token wo delimiters.
                else
                    return nextToken(delimiters.ToCharArray());
            }

            //Returns the nextToken wo delimiters
            private System.String nextToken(char[] delimiters)
            {
                string token = "";
                long pos = this.currentPos;

                //skip possible delimiters
                while (System.Array.IndexOf(delimiters, this.chars[currentPos]) != -1)
                    //The last one is a delimiter (i.e there is no more tokens)
                    if (++this.currentPos == this.chars.Length)
                    {
                        this.currentPos = pos;
                        throw new System.ArgumentOutOfRangeException();
                    }

                //getting the token
                while (System.Array.IndexOf(delimiters, this.chars[this.currentPos]) == -1)
                {
                    token += this.chars[this.currentPos];
                    //the last one is not a delimiter
                    if (++this.currentPos == this.chars.Length)
                        break;
                }
                return token;
            }


            /// <summary>
            /// Determines if there are more tokens to return from the source string
            /// </summary>
            /// <returns>True or false, depending if there are more tokens</returns>
            public bool HasMoreTokens()
            {
                //keeping the current pos
                long pos = this.currentPos;

                try
                {
                    this.NextToken();
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return false;
                }
                finally
                {
                    this.currentPos = pos;
                }
                return true;
            }

            /// <summary>
            /// Remaining tokens count
            /// </summary>
            public int Count
            {
                get
                {
                    //keeping the current pos
                    long pos = this.currentPos;
                    int i = 0;

                    try
                    {
                        while (true)
                        {
                            this.NextToken();
                            i++;
                        }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        this.currentPos = pos;
                        return i;
                    }
                }
            }

            /// <summary>
            ///  Performs the same action as NextToken.
            /// </summary>
            public System.Object Current
            {
                get
                {
                    return (Object)this.NextToken();
                }
            }

            /// <summary>
            //  Performs the same action as HasMoreTokens.
            /// </summary>
            /// <returns>True or false, depending if there are more tokens</returns>
            public bool MoveNext()
            {
                return this.HasMoreTokens();
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Reset()
            {
                ;
            }
        }
        /*******************************/
        /// <summary>
        /// This method works as a handler for the Control.Layout event, it arranges the controls into the container 
        /// control in a left-to-right orientation.
        /// The location of each control will be calculated according the number of them in the container. 
        /// The corresponding alignment, the horizontal and vertical spacing between the inner controls are specified
        /// as an array of object values in the Tag property of the container.
        /// </summary>
        /// <param name="event_sender">The container control in which the controls will be relocated.</param>
        /// <param name="eventArgs">Data of the event.</param>
        public static void FlowLayoutResize(System.Object event_sender, System.Windows.Forms.LayoutEventArgs eventArgs)
        {
            System.Windows.Forms.Control container = (System.Windows.Forms.Control)event_sender;
            if (container.Tag is System.Array)
            {
                System.Object[] items = (System.Object[])container.Tag;
                if (items.Length == 3)
                {
                    container.SuspendLayout();

                    int width = container.Width;
                    int height = container.Height;
                    if (!(container is System.Windows.Forms.ScrollableControl))
                    {
                        width = container.DisplayRectangle.Width;
                        height = container.DisplayRectangle.Height;
                    }
                    else
                        if (container is System.Windows.Forms.Form)
                        {
                            width = ((System.Windows.Forms.Form)container).ClientSize.Width;
                            height = ((System.Windows.Forms.Form)container).ClientSize.Height;
                        }
                    System.Drawing.ContentAlignment alignment = (System.Drawing.ContentAlignment)items[0];
                    int horizontal = (int)items[1];
                    int vertical = (int)items[2];

                    // Split controls in several rows
                    System.Collections.ArrayList rows = new System.Collections.ArrayList();
                    System.Collections.ArrayList list = new System.Collections.ArrayList();
                    int tempWidth = 0;
                    int tempHeight = 0;
                    int totalHeight = 0;
                    for (int index = 0; index < container.Controls.Count; index++)
                    {
                        if (tempHeight < container.Controls[index].Height)
                            tempHeight = container.Controls[index].Height;

                        list.Add(container.Controls[index]);

                        if (index == 0) tempWidth = container.Controls[0].Width;

                        if (index == container.Controls.Count - 1)
                        {
                            rows.Add(list);
                            totalHeight += tempHeight + vertical;
                        }
                        else
                        {
                            tempWidth += horizontal + container.Controls[index + 1].Width;
                            if (tempWidth >= width - horizontal * 2)
                            {
                                rows.Add(list);
                                totalHeight += tempHeight + vertical;
                                tempHeight = 0;
                                list = new System.Collections.ArrayList();
                                tempWidth = container.Controls[index + 1].Width;
                            }
                        }
                    }
                    totalHeight -= vertical;

                    // Break out alignment coordinates
                    int h = 0;
                    int cx = 0;
                    int cy = 0;
                    if (((int)alignment & 0x00F) > 0)
                    {
                        h = (int)alignment;
                        cy = 1;
                    }
                    if (((int)alignment & 0x0F0) > 0)
                    {
                        h = (int)alignment >> 4;
                        cy = 2;
                    }
                    if (((int)alignment & 0xF00) > 0)
                    {
                        h = (int)alignment >> 8;
                        cy = 3;
                    }
                    if (h == 1) cx = 1;
                    if (h == 2) cx = 2;
                    if (h == 4) cx = 3;

                    int ypos = vertical;
                    if (cy == 2) ypos = height / 2 - totalHeight / 2;
                    if (cy == 3) ypos = height - totalHeight - vertical;
                    foreach (System.Collections.ArrayList row in rows)
                    {
                        int maxHeight = PlaceControls(row, width, cx, ypos, horizontal);
                        ypos += vertical + maxHeight;
                    }
                    container.ResumeLayout();
                }
            }
        }

        private static int PlaceControls(System.Collections.ArrayList controls, int width, int cx, int ypos, int horizontal)
        {
            int count = controls.Count;
            int controlsWidth = 0;
            int maxHeight = 0;
            foreach (System.Windows.Forms.Control control in controls)
            {
                controlsWidth += control.Width;
                if (maxHeight < control.Height) maxHeight = control.Height;
            }
            controlsWidth += horizontal * (count - 1);

            // Start x point
            int xpos = 0;
            if (cx == 1) xpos = horizontal; // Left
            if (cx == 2) xpos = width / 2 - controlsWidth / 2; // Center
            if (cx == 3) xpos = width - horizontal - controlsWidth; // Right

            // Place controls
            int x = xpos;
            foreach (System.Windows.Forms.Control control in controls)
            {
                int y = ypos + (maxHeight / 2) - control.Height / 2;
                control.Location = new System.Drawing.Point(x, y);
                x += control.Width + horizontal;
            }
            return maxHeight;
        }


        /*******************************/
        /// <summary>
        /// Class used to store and retrieve an object command specified as a String.
        /// </summary>
        public class CommandManager
        {
            /// <summary>
            /// Private Hashtable used to store objects and their commands.
            /// </summary>
            private static System.Collections.Hashtable Commands = new System.Collections.Hashtable();

            /// <summary>
            /// Sets a command to the specified object.
            /// </summary>
            /// <param name="obj">The object that has the command.</param>
            /// <param name="cmd">The command for the object.</param>
            public static void SetCommand(System.Object obj, System.String cmd)
            {
                if (obj != null)
                {
                    if (Commands.Contains(obj))
                        Commands[obj] = cmd;
                    else
                        Commands.Add(obj, cmd);
                }
            }

            /// <summary>
            /// Gets a command associated with an object.
            /// </summary>
            /// <param name="obj">The object whose command is going to be retrieved.</param>
            /// <returns>The command of the specified object.</returns>
            public static System.String GetCommand(System.Object obj)
            {
                System.String result = "";
                if (obj != null)
                    result = System.Convert.ToString(Commands[obj]);
                return result;
            }



            /// <summary>
            /// Checks if the Control contains a command, if it does not it sets the default
            /// </summary>
            /// <param name="button">The control whose command will be checked</param>
            public static void CheckCommand(System.Windows.Forms.ButtonBase button)
            {
                if (button != null)
                {
                    if (GetCommand(button).Equals(""))
                        SetCommand(button, button.Text);
                }
            }

            /// <summary>
            /// Checks if the Control contains a command, if it does not it sets the default
            /// </summary>
            /// <param name="button">The control whose command will be checked</param>
            public static void CheckCommand(System.Windows.Forms.MenuItem menuItem)
            {
                if (menuItem != null)
                {
                    if (GetCommand(menuItem).Equals(""))
                        SetCommand(menuItem, menuItem.Text);
                }
            }

            /// <summary>
            /// Checks if the Control contains a command, if it does not it sets the default
            /// </summary>
            /// <param name="button">The control whose command will be checked</param>
            public static void CheckCommand(System.Windows.Forms.ComboBox comboBox)
            {
                if (comboBox != null)
                {
                    if (GetCommand(comboBox).Equals(""))
                        SetCommand(comboBox, "comboBoxChanged");
                }
            }

        }
        /*******************************/
        /// <summary>
        /// This class has static methods for manage CheckBox and RadioButton controls.
        /// </summary>
        public class CheckBoxSupport
        {

            /// <summary>
            /// Receives a CheckBox instance and sets the specific text and style.
            /// </summary>
            /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
            /// <param name="text">The text to set Text property.</param>
            /// <param name="style">The style to be used to set the threeState property.</param>
            public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, int style)
            {
                checkBoxInstance.Text = text;
                if (style == 2097152)
                    checkBoxInstance.ThreeState = true;
            }

            /// <summary>
            /// Returns a new CheckBox instance with the specified text
            /// </summary>
            /// <param name="text">The text to create the CheckBox with</param>
            /// <returns>A new CheckBox instance</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text)
            {
                System.Windows.Forms.CheckBox tempCheck = new System.Windows.Forms.CheckBox();
                tempCheck.Text = text;
                return tempCheck;
            }

            /// <summary>
            /// Creates a CheckBox with a specified image.
            /// </summary>
            /// <param name="icon">The image to be used with the CheckBox.</param>
            /// <returns>A new CheckBox instance with an image.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.Drawing.Image icon)
            {
                System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
                tempCheckBox.Image = icon;
                return tempCheckBox;
            }

            /// <summary>
            /// Creates a CheckBox with a specified label and image.
            /// </summary>
            /// <param name="text">The text to be used as label.</param>
            /// <param name="icon">The image to be used with the CheckBox.</param>
            /// <returns>A new CheckBox instance with a label and an image.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, System.Drawing.Image icon)
            {
                System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
                tempCheckBox.Text = text;
                tempCheckBox.Image = icon;
                return tempCheckBox;
            }

            /// <summary>
            /// Returns a new CheckBox instance with the specified text and state
            /// </summary>
            /// <param name="text">The text to create the CheckBox with</param>
            /// <param name="checkedStatus">Indicates if the Checkbox is checked or not</param>
            /// <returns>A new CheckBox instance</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, bool checkedStatus)
            {
                System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
                tempCheckBox.Text = text;
                tempCheckBox.Checked = checkedStatus;
                return tempCheckBox;
            }

            /// <summary>
            /// Creates a CheckBox with a specified image and checked if checkedStatus is true.
            /// </summary>
            /// <param name="icon">The image to be used with the CheckBox.</param>
            /// <param name="checkedStatus">Value to be set to Checked property.</param>
            /// <returns>A new CheckBox instance.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.Drawing.Image icon, bool checkedStatus)
            {
                System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
                tempCheckBox.Image = icon;
                tempCheckBox.Checked = checkedStatus;
                return tempCheckBox;
            }

            /// <summary>
            /// Creates a CheckBox with label, image and checked if checkedStatus is true.
            /// </summary>
            /// <param name="text">The text to be used as label.</param>
            /// <param name="icon">The image to be used with the CheckBox.</param>
            /// <param name="checkedStatus">Value to be set to Checked property.</param>
            /// <returns>A new CheckBox instance.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, System.Drawing.Image icon, bool checkedStatus)
            {
                System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
                tempCheckBox.Text = text;
                tempCheckBox.Image = icon;
                tempCheckBox.Checked = checkedStatus;
                return tempCheckBox;
            }

            /// <summary>
            /// Creates a CheckBox with a specific control.
            /// </summary>
            /// <param name="control">The control to be added to the CheckBox.</param>
            /// <returns>The new CheckBox with the specific control.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.Windows.Forms.Control control)
            {
                System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
                tempCheckBox.Controls.Add(control);
                control.Location = new System.Drawing.Point(0, 18);
                return tempCheckBox;
            }

            /// <summary>
            /// Creates a CheckBox with the specific control and style.
            /// </summary>
            /// <param name="control">The control to be added to the CheckBox.</param>
            /// <param name="style">The style to be used to set the threeState property.</param>
            /// <returns>The new CheckBox with the specific control and style.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.Windows.Forms.Control control, int style)
            {
                System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
                tempCheckBox.Controls.Add(control);
                control.Location = new System.Drawing.Point(0, 18);
                if (style == 2097152)
                    tempCheckBox.ThreeState = true;
                return tempCheckBox;
            }

            /// <summary>
            /// Returns a new RadioButton instance with the specified text in the specified panel.
            /// </summary>
            /// <param name="text">The text to create the RadioButton with.</param>
            /// <param name="checkedStatus">Indicates if the RadioButton is checked or not.</param>
            /// <param name="panel">The panel where the RadioButton will be placed.</param>
            /// <returns>A new RadioButton instance.</returns>
            /// <remarks>If the panel is null the third param is ignored</remarks>
            public static System.Windows.Forms.RadioButton CreateRadioButton(System.String text, bool checkedStatus, System.Windows.Forms.Panel panel)
            {
                System.Windows.Forms.RadioButton tempCheckBox = new System.Windows.Forms.RadioButton();
                tempCheckBox.Text = text;
                tempCheckBox.Checked = checkedStatus;
                if (panel != null)
                    panel.Controls.Add(tempCheckBox);
                return tempCheckBox;
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the Text and Image properties.
            /// </summary>
            /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
            /// <param name="text">Value to be set to Text property.</param>
            /// <param name="icon">Value to be set to Image property.</param>
            public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, System.Drawing.Image icon)
            {
                checkBoxInstance.Text = text;
                checkBoxInstance.Image = icon;
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the Text and Checked properties.
            /// </summary>
            /// <param name="checkBoxInstance">CheckBox instance to be set</param>
            /// <param name="text">Value to be set to Text</param>
            /// <param name="checkedStatus">Value to be set to Checked property.</param>
            public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, bool checkedStatus)
            {
                checkBoxInstance.Text = text;
                checkBoxInstance.Checked = checkedStatus;
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the Image and Checked properties.
            /// </summary>
            /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
            /// <param name="icon">Value to be set to Image property.</param>
            /// <param name="checkedStatus">Value to be set to Checked property.</param>
            public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.Drawing.Image icon, bool checkedStatus)
            {
                checkBoxInstance.Image = icon;
                checkBoxInstance.Checked = checkedStatus;
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the Text, Image and Checked properties.
            /// </summary>
            /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
            /// <param name="text">Value to be set to Text property.</param>
            /// <param name="icon">Value to be set to Image property.</param>
            /// <param name="checkedStatus">Value to be set to Checked property.</param>
            public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, System.Drawing.Image icon, bool checkedStatus)
            {
                checkBoxInstance.Text = text;
                checkBoxInstance.Image = icon;
                checkBoxInstance.Checked = checkedStatus;
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the control specified.
            /// </summary>
            /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
            /// <param name="control">The control assiciated with the CheckBox</param>
            public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.Windows.Forms.Control control)
            {
                checkBoxInstance.Controls.Add(control);
                control.Location = new System.Drawing.Point(0, 18);
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the specific control and style.
            /// </summary>
            /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
            /// <param name="control">The control assiciated with the CheckBox.</param>
            /// <param name="style">The style to be used to set the threeState property.</param>
            public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.Windows.Forms.Control control, int style)
            {
                checkBoxInstance.Controls.Add(control);
                control.Location = new System.Drawing.Point(0, 18);
                if (style == 2097152)
                    checkBoxInstance.ThreeState = true;
            }

            /// <summary>
            /// Receives an instance of a RadioButton and sets its Text and Checked properties.
            /// </summary>
            /// <param name="RadioButtonInstance">The instance of RadioButton to be set.</param>
            /// <param name="text">The text to set Text property.</param>
            /// <param name="checkedStatus">Indicates if the RadioButton is checked or not.</param>
            public static void SetCheckbox(System.Windows.Forms.RadioButton radioButtonInstance, System.String text, bool checkedStatus)
            {
                radioButtonInstance.Text = text;
                radioButtonInstance.Checked = checkedStatus;
            }

            /// <summary>
            /// Receives an instance of a RadioButton and sets its Text and Checked properties and its containing panel
            /// </summary>
            /// <param name="CheckBoxInstance">The instance of RadioButton to be set</param>
            /// <param name="text">The text to set Text property</param>
            /// <param name="checkedStatus">Indicates if the RadioButton is checked or not</param>
            /// <param name="panel">The panel where the RadioButton will be placed</param>
            /// <remarks>If the panel is null the third param is ignored</remarks>
            public static void SetRadioButton(System.Windows.Forms.RadioButton radioButtonInstance, System.String text, bool checkedStatus, System.Windows.Forms.Panel panel)
            {
                radioButtonInstance.Text = text;
                radioButtonInstance.Checked = checkedStatus;
                if (panel != null)
                    panel.Controls.Add(radioButtonInstance);
            }

            /// <summary>
            /// Creates a CheckBox with a specified text label and style.
            /// </summary>
            /// <param name="text">The text to be used as label.</param>
            /// <param name="style">The style to be used to set the threeState property.</param>
            /// <returns>A new CheckBox instance.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, int style)
            {
                System.Windows.Forms.CheckBox checkBox = new System.Windows.Forms.CheckBox();
                checkBox.Text = text;
                if (style == 2097152)
                    checkBox.ThreeState = true;
                return checkBox;
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the Text and ThreeState properties.
            /// </summary>
            /// <param name="checkBox">CheckBox instance to be set.</param>
            /// <param name="text">Value to be set to Text property.</param>
            /// <param name="style">The style to be used to set the threeState property.</param>
            public static void setCheckBox(System.Windows.Forms.CheckBox checkBox, System.String text, int style)
            {
                checkBox.Text = text;
                if (style == 2097152)
                    checkBox.ThreeState = true;
            }

            /// <summary>
            /// Creates a CheckBox with a specified text label, image and style.
            /// </summary>
            /// <param name="text">The text to be used as label.</param>
            /// <param name="icon">The image to be used with the CheckBox.</param>
            /// <param name="style">The style to be used to set the threeState property.</param>
            /// <returns>A new CheckBox instance.</returns>
            public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, System.Drawing.Image icon, int style)
            {
                System.Windows.Forms.CheckBox checkBox = new System.Windows.Forms.CheckBox();
                checkBox.Text = text;
                checkBox.Image = icon;
                if (style == 2097152)
                    checkBox.ThreeState = true;
                return checkBox;
            }

            /// <summary>
            /// Receives a CheckBox instance and sets the Text, Image and ThreeState properties.
            /// </summary>
            /// <param name="checkBox">CheckBox instance to be set.</param>
            /// <param name="text">Value to be set to Text property.</param>
            /// <param name="icon">Value to be set to Image property.</param>
            /// <param name="style">The style to be used to set the threeState property.</param>
            public static void setCheckBox(System.Windows.Forms.CheckBox checkBox, System.String text, System.Drawing.Image icon, int style)
            {
                checkBox.Text = text;
                checkBox.Image = icon;
                if (style == 2097152)
                    checkBox.ThreeState = true;
            }

            /// <summary>
            /// The SetIndeterminate method is used to sets or clear the CheckState of the CheckBox component.
            /// </summary>
            /// <param name="indeterminate">The value to the Indeterminate state. If true, the state is set; otherwise, it is cleared.</param>
            /// <param name="checkBox">The CheckBox component to be modified.</param>
            /// <returns></returns>
            public static System.Windows.Forms.CheckBox SetIndeterminate(bool indeterminate, System.Windows.Forms.CheckBox checkBox)
            {
                if (indeterminate)
                    checkBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
                else if (checkBox.Checked)
                    checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
                else
                    checkBox.CheckState = System.Windows.Forms.CheckState.Unchecked;
                return checkBox;
            }
        }

        /*******************************/
        public class FormSupport
        {
            /// <summary>
            /// Creates a Form instance and sets the property Text to the specified parameter.
            /// </summary>
            /// <param name="Text">Value for the Form property Text</param>
            /// <returns>A new Form instance</returns>
            public static System.Windows.Forms.Form CreateForm(System.String text)
            {
                System.Windows.Forms.Form tempForm;
                tempForm = new System.Windows.Forms.Form();
                tempForm.Text = text;
                return tempForm;
            }

            /// <summary>
            /// Creates a Form instance and sets the property Text to the specified parameter.
            /// Adds the received control to the Form's Controls collection in the position 0.
            /// </summary>
            /// <param name="text">Value for the Form Text property.</param>
            /// <param name="control">Control to be added to the Controls collection.</param>
            /// <returns>A new Form instance</returns>
            public static System.Windows.Forms.Form CreateForm(System.String text, System.Windows.Forms.Control control)
            {
                System.Windows.Forms.Form tempForm;
                tempForm = new System.Windows.Forms.Form();
                tempForm.Text = text;
                tempForm.Controls.Add(control);
                tempForm.Controls.SetChildIndex(control, 0);
                return tempForm;
            }


            /// <summary>
            /// Creates a Form instance and sets the property Owner to the specified parameter.
            /// This also sets the FormBorderStyle and ShowInTaskbar properties.
            /// </summary>
            /// <param name="Owner">Value for the Form property Owner</param>
            /// <returns>A new Form instance</returns>
            public static System.Windows.Forms.Form CreateForm(System.Windows.Forms.Form owner)
            {
                System.Windows.Forms.Form tempForm;
                tempForm = new System.Windows.Forms.Form();
                tempForm.Owner = owner;
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                tempForm.ShowInTaskbar = false;
                return tempForm;
            }


            /// <summary>
            /// Creates a Form instance and sets the property Owner to the specified parameter.
            /// Sets the FormBorderStyle and ShowInTaskbar properties.
            /// Also add the received Control to the Form's Controls collection in the position 0.
            /// </summary>
            /// <param name="owner">Value for the Form property Owner</param>
            /// <param name="header">Control to be added to the Form's Controls collection</param>
            /// <returns>A new Form instance</returns>
            public static System.Windows.Forms.Form CreateForm(System.Windows.Forms.Form owner, System.Windows.Forms.Control header)
            {
                System.Windows.Forms.Form tempForm;
                tempForm = new System.Windows.Forms.Form();
                tempForm.Owner = owner;
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                tempForm.ShowInTaskbar = false;
                tempForm.Controls.Add(header);
                tempForm.Controls.SetChildIndex(header, 0);
                return tempForm;
            }

            /// <summary>
            /// Creates a Form instance and sets the FormBorderStyle property.
            /// </summary>
            /// <param name="title">The title of the Form</param>
            /// <param name="resizable">Boolean value indicating if the Form is or not resizable</param>
            /// <returns>A new Form instance</returns>
            public static System.Windows.Forms.Form CreateForm(System.String title, bool resizable)
            {
                System.Windows.Forms.Form tempForm;
                tempForm = new System.Windows.Forms.Form();
                tempForm.Text = title;
                if (resizable)
                    tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                else
                    tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                tempForm.TopLevel = false;
                tempForm.MaximizeBox = false;
                tempForm.MinimizeBox = false;
                return tempForm;
            }

            /// <summary>
            /// Creates a Form instance and sets the FormBorderStyle property.
            /// </summary>
            /// <param name="title">The title of the Form</param>
            /// <param name="resizable">Boolean value indicating if the Form is or not resizable</param>
            /// <param name="maximizable">Boolean value indicating if the Form displays the maximaze box</param>
            /// <returns>A new Form instance</returns>
            public static System.Windows.Forms.Form CreateForm(System.String title, bool resizable, bool maximizable)
            {
                System.Windows.Forms.Form tempForm;
                tempForm = new System.Windows.Forms.Form();
                tempForm.Text = title;
                if (resizable)
                    tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                else
                    tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                tempForm.TopLevel = false;
                tempForm.MaximizeBox = maximizable;
                tempForm.MinimizeBox = false;
                return tempForm;
            }

            /// <summary>
            /// Creates a Form instance and sets the FormBorderStyle property.
            /// </summary>
            /// <param name="title">The title of the Form</param>
            /// <param name="resizable">Boolean value indicating if the Form is or not resizable</param>
            /// <param name="maximizable">Boolean value indicating if the Form displays the maximaze box</param>
            /// <param name="minimizable">Boolean value indicating if the Form displays the minimaze box</param>
            /// <returns>A new Form instance</returns>
            public static System.Windows.Forms.Form CreateForm(System.String title, bool resizable, bool maximizable, bool minimizable)
            {
                System.Windows.Forms.Form tempForm;
                tempForm = new System.Windows.Forms.Form();
                tempForm.Text = title;
                if (resizable)
                    tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                else
                    tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                tempForm.TopLevel = false;
                tempForm.MaximizeBox = maximizable;
                tempForm.MinimizeBox = minimizable;
                return tempForm;
            }

            /// <summary>
            /// Receives a Form instance and sets the property Owner to the specified parameter.
            /// This also sets the FormBorderStyle and ShowInTaskbar properties.
            /// </summary>
            /// <param name="formInstance">Form instance to be set</param>
            /// <param name="Owner">Value for the Form property Owner</param>
            public static void SetForm(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner)
            {
                formInstance.Owner = owner;
                formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                formInstance.ShowInTaskbar = false;
            }

            /// <summary>
            /// Receives a Form instance, sets the Text property and adds a Control.
            /// The received Control is added in the position 0 of the Form's Controls collection.
            /// </summary>
            /// <param name="formInstance">Form instance to be set</param>
            /// <param name="text">Value to be set to the Text property.</param>
            /// <param name="control">Control to add to the Controls collection.</param>
            public static void SetForm(System.Windows.Forms.Form formInstance, System.String text, System.Windows.Forms.Control control)
            {
                formInstance.Text = text;
                formInstance.Controls.Add(control);
                formInstance.Controls.SetChildIndex(control, 0);
            }

            /// <summary>
            /// Receives a Form instance and sets the property Owner to the specified parameter.
            /// Sets the FormBorderStyle and ShowInTaskbar properties.
            /// Also adds the received Control to the Form's Controls collection in position 0.
            /// </summary>
            /// <param name="formInstance">Form instance to be set</param>
            /// <param name="owner">Value for the Form property Owner</param>
            /// <param name="header">The Control to be added to the Controls collection.</param>
            public static void SetForm(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner, System.Windows.Forms.Control header)
            {
                formInstance.Owner = owner;
                formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                formInstance.ShowInTaskbar = false;
                formInstance.Controls.Add(header);
                formInstance.Controls.SetChildIndex(header, 0);
            }
        }
        /*******************************/
        /// <summary>
        /// Writes the exception stack trace to the received stream
        /// </summary>
        /// <param name="throwable">Exception to obtain information from</param>
        /// <param name="stream">Output sream used to write to</param>
        public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
        {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }

        /*******************************/
        /// <summary>
        /// SupportClass for the BitArray class.
        /// </summary>
        public class BitArraySupport
        {
            /// <summary>
            /// Sets the specified bit to true.
            /// </summary>
            /// <param name="bits">The BitArray to modify.</param>
            /// <param name="index">The bit index to set to true.</param>
            public static void Set(System.Collections.BitArray bits, System.Int32 index)
            {
                for (int increment = 0; index >= bits.Length; increment = +64)
                {
                    bits.Length += increment;
                }

                bits.Set(index, true);
            }

            /// <summary>
            /// Returns a string representation of the BitArray object.
            /// </summary>
            /// <param name="bits">The BitArray object to convert to string.</param>
            /// <returns>A string representation of the BitArray object.</returns>
            public static System.String ToString(System.Collections.BitArray bits)
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                if (bits != null)
                {
                    for (int i = 0; i < bits.Length; i++)
                    {
                        if (bits[i] == true)
                        {
                            if (s.Length > 0)
                                s.Append(", ");
                            s.Append(i);
                        }
                    }

                    s.Insert(0, "{");
                    s.Append("}");
                }
                else
                    s.Insert(0, "null");

                return s.ToString();
            }
        }


        /*******************************/
        /// <summary>
        /// This class supports basic functionality of the JOptionPane class.
        /// </summary>
        public class OptionPaneSupport
        {
            /// <summary>
            /// This method finds the form which contains an specific control.
            /// </summary>
            /// <param name="control">The control which we need to find its form.</param>
            /// <returns>The form which contains the control</returns>
            public static System.Windows.Forms.Form GetFrameForComponent(System.Windows.Forms.Control control)
            {
                System.Windows.Forms.Form result = null;
                if (control == null) return null;
                if (control is System.Windows.Forms.Form)
                    result = (System.Windows.Forms.Form)control;
                else if (control.Parent != null)
                    result = GetFrameForComponent(control.Parent);
                return result;
            }

            /// <summary>
            /// This method finds the MDI container form which contains an specific control.
            /// </summary>
            /// <param name="control">The control which we need to find its MDI container form.</param>
            /// <returns>The MDI container form which contains the control.</returns>
            public static System.Windows.Forms.Form GetDesktopPaneForComponent(System.Windows.Forms.Control control)
            {
                System.Windows.Forms.Form result = null;
                if (control == null) return null;
                if (control.GetType().IsSubclassOf(typeof(System.Windows.Forms.Form)))
                    if (((System.Windows.Forms.Form)control).IsMdiContainer)
                        result = (System.Windows.Forms.Form)control;
                    else if (((System.Windows.Forms.Form)control).IsMdiChild)
                        result = GetDesktopPaneForComponent(((System.Windows.Forms.Form)control).MdiParent);
                    else if (control.Parent != null)
                        result = GetDesktopPaneForComponent(control.Parent);
                return result;
            }

            /// <summary>
            /// This method retrieves the message that is contained into the object that is sended by the user.
            /// </summary>
            /// <param name="control">The control which we need to find its form.</param>
            /// <returns>The form which contains the control</returns>
            public static System.String GetMessageForObject(System.Object message)
            {
                System.String result = "";
                if (message == null)
                    return result;
                else
                    result = message.ToString();
                return result;
            }


            /// <summary>
            /// This method displays a dialog with a Yes, No, Cancel buttons and a question icon.
            /// </summary>
            /// <param name="parent">The component which will be the owner of the dialog.</param>
            /// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
            /// <returns>The integer value which represents the button pressed.</returns>
            public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message)
            {
                return ShowConfirmDialog(parent, message, "Select an option...", (int)System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                    (int)System.Windows.Forms.MessageBoxIcon.Question);
            }

            /// <summary>
            /// This method displays a dialog with specific buttons and a question icon.
            /// </summary>
            /// <param name="parent">The component which will be the owner of the dialog.</param>
            /// <param name="message">The message to be displayed; if it isn't an String it displays the result value of the ToString() method of the object.</param>
            /// <param name="title">The title for the message dialog.</param>
            /// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
            /// <returns>The integer value which represents the button pressed.</returns>
            public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
                System.String title, int optiontype)
            {
                return ShowConfirmDialog(parent, message, title, optiontype, (int)System.Windows.Forms.MessageBoxIcon.Question);
            }

            /// <summary>
            /// This method displays a dialog with specific buttons and specific icon.
            /// </summary>
            /// <param name="parent">The component which will be the owner of the dialog.</param>
            /// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
            /// <param name="title">The title for the message dialog.</param>
            /// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
            /// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
            /// <returns>The integer value which represents the button pressed.</returns>
            public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
                System.String title, int optiontype, int messagetype)
            {
                return (int)System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
                    (System.Windows.Forms.MessageBoxButtons)optiontype, (System.Windows.Forms.MessageBoxIcon)messagetype);
            }

            /// <summary>
            /// This method displays a simple MessageBox.
            /// </summary>
            /// <param name="parent">The component which will be the owner of the dialog.</param>
            /// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
            public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message)
            {
                ShowMessageDialog(parent, message, "Message", (int)System.Windows.Forms.MessageBoxIcon.Information);
            }

            /// <summary>
            /// This method displays a simple MessageBox with a specific icon.
            /// </summary>
            /// <param name="parent">The component which will be the owner of the dialog.</param>
            /// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
            /// <param name="title">The title for the message dialog.</param>
            /// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
            public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message,
                System.String title, int messagetype)
            {
                System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
                    System.Windows.Forms.MessageBoxButtons.OK, (System.Windows.Forms.MessageBoxIcon)messagetype);
            }
        }


        /*******************************/
        public static bool IsDataFormatSupported(System.Windows.Forms.IDataObject data, System.Windows.Forms.DataFormats.Format format)
        {
            bool result = false;
            if ((data != null) && (format != null))
            {
                System.String[] formats = data.GetFormats(true);
                int count = formats.GetLength(0);
                for (int index = 0; index < count; index++)
                {
                    if (formats[index].Equals(format.Name))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }


        /*******************************/
        /// <summary>
        /// Support Methods for FileDialog class. Note that several methods receive a DirectoryInfo object, but it won't be used in all cases.
        /// </summary>
        public class FileDialogSupport
        {
            /// <summary>
            /// Creates an OpenFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the OpenFileDialog.</param>
            /// <returns>A new instance of OpenFileDialog.</returns>
            public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path)
            {
                System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
                temp_fileDialog.InitialDirectory = path.Directory.FullName;
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates an OpenFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the OpenFileDialog.</param>
            /// <param name="directory">Directory to get the path from.</param>
            /// <returns>A new instance of OpenFileDialog.</returns>
            public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
            {
                System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
                temp_fileDialog.InitialDirectory = path.Directory.FullName;
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates a OpenFileDialog open in a given path.
            /// </summary>		
            /// <returns>A new instance of OpenFileDialog.</returns>
            public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog()
            {
                System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
                temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates an OpenFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the OpenFileDialog</param>
            /// <returns>A new instance of OpenFileDialog.</returns>
            public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.String path)
            {
                System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
                temp_fileDialog.InitialDirectory = path;
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates an OpenFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the OpenFileDialog.</param>
            /// <param name="directory">Directory to get the path from.</param>
            /// <returns>A new instance of OpenFileDialog.</returns>
            public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.String path, System.IO.DirectoryInfo directory)
            {
                System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
                temp_fileDialog.InitialDirectory = path;
                return temp_fileDialog;
            }

            /// <summary>
            /// Modifies an instance of OpenFileDialog, to open a given directory.
            /// </summary>
            /// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
            /// <param name="path">Path to be opened by the OpenFileDialog.</param>
            /// <param name="directory">Directory to get the path from.</param>
            public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path, System.IO.DirectoryInfo directory)
            {
                fileDialog.InitialDirectory = path;
            }

            /// <summary>
            /// Modifies an instance of OpenFileDialog, to open a given directory.
            /// </summary>
            /// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
            /// <param name="path">Path to be opened by the OpenFileDialog</param>
            public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.IO.FileInfo path)
            {
                fileDialog.InitialDirectory = path.Directory.FullName;
            }

            /// <summary>
            /// Modifies an instance of OpenFileDialog, to open a given directory.
            /// </summary>
            /// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
            /// <param name="path">Path to be opened by the OpenFileDialog.</param>
            public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path)
            {
                fileDialog.InitialDirectory = path;
            }

            ///
            ///  Use the following static methods to create instances of SaveFileDialog.
            ///  By default, JFileChooser is converted as an OpenFileDialog, the following methods
            ///  are provided to create file dialogs to save files.
            ///	


            /// <summary>
            /// Creates a SaveFileDialog.
            /// </summary>		
            /// <returns>A new instance of SaveFileDialog.</returns>
            public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog()
            {
                System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
                temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates an SaveFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the SaveFileDialog.</param>
            /// <returns>A new instance of SaveFileDialog.</returns>
            public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path)
            {
                System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
                temp_fileDialog.InitialDirectory = path.Directory.FullName;
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates an SaveFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the SaveFileDialog.</param>
            /// <param name="directory">Directory to get the path from.</param>
            /// <returns>A new instance of SaveFileDialog.</returns>
            public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
            {
                System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
                temp_fileDialog.InitialDirectory = path.Directory.FullName;
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates a SaveFileDialog open in a given path.
            /// </summary>
            /// <param name="directory">Directory to get the path from.</param>
            /// <returns>A new instance of SaveFileDialog.</returns>
            public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.DirectoryInfo directory)
            {
                System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
                temp_fileDialog.InitialDirectory = directory.FullName;
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates an SaveFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the SaveFileDialog</param>
            /// <returns>A new instance of SaveFileDialog.</returns>
            public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.String path)
            {
                System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
                temp_fileDialog.InitialDirectory = path;
                return temp_fileDialog;
            }

            /// <summary>
            /// Creates an SaveFileDialog open in a given path.
            /// </summary>
            /// <param name="path">Path to be opened by the SaveFileDialog.</param>
            /// <param name="directory">Directory to get the path from.</param>
            /// <returns>A new instance of SaveFileDialog.</returns>
            public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.String path, System.IO.DirectoryInfo directory)
            {
                System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
                temp_fileDialog.InitialDirectory = path;
                return temp_fileDialog;
            }
        }
        /*******************************/
        /// <summary>
        /// Defines a method to handle basic events and convert the Action interface
        /// </summary>
        [Serializable]
        public abstract class ActionSupport
        {
            private System.Drawing.Image icon;
            private System.String description;

            /// <summary>
            /// Creates a new ActionSupport.
            /// </summary>		
            public ActionSupport()
            {
                this.description = null;
                this.icon = null;
            }


            /// <summary>
            /// Creates a new ActionSupport.
            /// </summary>
            /// <param name="description">The description for this Action</param>
            /// <param name="icon">The icon for this Action</param>
            public ActionSupport(System.String description, System.Drawing.Image icon)
            {
                this.description = description;
                this.icon = icon;
            }

            /// <summary>
            /// Creates a new ActionSupport
            /// </summary>
            /// <param name="description">The description for this Action</param>
            public ActionSupport(System.String description)
            {
                this.description = description;
                this.icon = null;
            }

            /// <summary>
            /// .NET version for the actionPerformed method.
            /// </summary>
            /// <param name="event_sender">The event raising object.</param>
            /// <param name="eventArgs">The arguments for the event.</param>
            public abstract void actionPerformed(System.Object event_sender, System.EventArgs eventArgs);

            /// <summary>
            /// The icon this Action provides for controls that use it.
            /// </summary>
            public System.Drawing.Image Icon
            {
                get
                {
                    return this.icon;
                }
                set
                {
                    this.icon = value;
                }
            }

            /// <summary>
            /// The text this Action provides for controls that use it.
            /// </summary>
            public System.String Description
            {
                get
                {
                    return this.description;
                }
                set
                {
                    this.description = value;
                }
            }
        }


        /*******************************/
        /// <summary>
        /// This method will recieve a PrintDocument object that will be associated with a new
        ///	PrintDialog object. Then the dialog is shown.
        /// </summary>
        /// <param name="document">The document object.</param>
        /// <returns>true if the DialogResult was 'OK', false otherwise.</returns>
        public static bool PrintDialogSupport(System.Drawing.Printing.PrintDocument document)
        {
            System.Windows.Forms.PrintDialog tempPrintDialog = new System.Windows.Forms.PrintDialog();
            tempPrintDialog.Document = document;
            return (tempPrintDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK);
        }


        /*******************************/
        /// <summary>
        /// This class manages different features for calendars.
        /// The different calendars are internally managed using a hashtable structure.
        /// </summary>
        public class CalendarManager
        {
            /// <summary>
            /// Field used to get or set the year.
            /// </summary>
            public const int YEAR = 1;

            /// <summary>
            /// Field used to get or set the month.
            /// </summary>
            public const int MONTH = 2;

            /// <summary>
            /// Field used to get or set the day of the month.
            /// </summary>
            public const int DATE = 5;

            /// <summary>
            /// Field used to get or set the hour of the morning or afternoon.
            /// </summary>
            public const int HOUR = 10;

            /// <summary>
            /// Field used to get or set the minute within the hour.
            /// </summary>
            public const int MINUTE = 12;

            /// <summary>
            /// Field used to get or set the second within the minute.
            /// </summary>
            public const int SECOND = 13;

            /// <summary>
            /// Field used to get or set the millisecond within the second.
            /// </summary>
            public const int MILLISECOND = 14;

            /// <summary>
            /// Field used to get or set the day of the year.
            /// </summary>
            public const int DAY_OF_YEAR = 4;

            /// <summary>
            /// Field used to get or set the day of the month.
            /// </summary>
            public const int DAY_OF_MONTH = 6;

            /// <summary>
            /// Field used to get or set the day of the week.
            /// </summary>
            public const int DAY_OF_WEEK = 7;

            /// <summary>
            /// Field used to get or set the hour of the day.
            /// </summary>
            public const int HOUR_OF_DAY = 11;

            /// <summary>
            /// Field used to get or set whether the HOUR is before or after noon.
            /// </summary>
            public const int AM_PM = 9;

            /// <summary>
            /// Field used to get or set the value of the AM_PM field which indicates the period of the day from midnight to just before noon.
            /// </summary>
            public const int AM = 0;

            /// <summary>
            /// Field used to get or set the value of the AM_PM field which indicates the period of the day from noon to just before midnight.
            /// </summary>
            public const int PM = 1;

            /// <summary>
            /// The hashtable that contains the calendars and its properties.
            /// </summary>
            static public CalendarHashTable manager = new CalendarHashTable();

            /// <summary>
            /// Internal class that inherits from HashTable to manage the different calendars.
            /// This structure will contain an instance of System.Globalization.Calendar that represents 
            /// a type of calendar and its properties (represented by an instance of CalendarProperties 
            /// class).
            /// </summary>
            public class CalendarHashTable : System.Collections.Hashtable
            {
                /// <summary>
                /// Gets the calendar current date and time.
                /// </summary>
                /// <param name="calendar">The calendar to get its current date and time.</param>
                /// <returns>A System.DateTime value that indicates the current date and time for the 
                /// calendar given.</returns>
                public System.DateTime GetDateTime(System.Globalization.Calendar calendar)
                {
                    if (this[calendar] != null)
                        return ((CalendarProperties)this[calendar]).dateTime;
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        this.Add(calendar, tempProps);
                        return this.GetDateTime(calendar);
                    }
                }

                /// <summary>
                /// Sets the specified System.DateTime value to the specified calendar.
                /// </summary>
                /// <param name="calendar">The calendar to set its date.</param>
                /// <param name="date">The System.DateTime value to set to the calendar.</param>
                public void SetDateTime(System.Globalization.Calendar calendar, System.DateTime date)
                {
                    if (this[calendar] != null)
                    {
                        ((CalendarProperties)this[calendar]).dateTime = date;
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = date;
                        this.Add(calendar, tempProps);
                    }
                }

                /// <summary>
                /// Sets the corresponding field in an specified calendar with the value given.
                /// If the specified calendar does not have exist in the hash table, it creates a 
                /// new instance of the calendar with the current date and time and then assings it 
                /// the new specified value.
                /// </summary>
                /// <param name="calendar">The calendar to set its date or time.</param>
                /// <param name="field">One of the fields that composes a date/time.</param>
                /// <param name="fieldValue">The value to be set.</param>
                public void Set(System.Globalization.Calendar calendar, int field, int fieldValue)
                {
                    if (this[calendar] != null)
                    {
                        System.DateTime tempDate = ((CalendarProperties)this[calendar]).dateTime;
                        switch (field)
                        {
                            case CalendarManager.DATE:
                                tempDate = tempDate.AddDays(fieldValue - tempDate.Day);
                                break;
                            case CalendarManager.HOUR:
                                tempDate = tempDate.AddHours(fieldValue - tempDate.Hour);
                                break;
                            case CalendarManager.MILLISECOND:
                                tempDate = tempDate.AddMilliseconds(fieldValue - tempDate.Millisecond);
                                break;
                            case CalendarManager.MINUTE:
                                tempDate = tempDate.AddMinutes(fieldValue - tempDate.Minute);
                                break;
                            case CalendarManager.MONTH:
                                //Month value is 0-based. e.g., 0 for January
                                tempDate = tempDate.AddMonths((fieldValue + 1) - tempDate.Month);
                                break;
                            case CalendarManager.SECOND:
                                tempDate = tempDate.AddSeconds(fieldValue - tempDate.Second);
                                break;
                            case CalendarManager.YEAR:
                                tempDate = tempDate.AddYears(fieldValue - tempDate.Year);
                                break;
                            case CalendarManager.DAY_OF_MONTH:
                                tempDate = tempDate.AddDays(fieldValue - tempDate.Day);
                                break;
                            case CalendarManager.DAY_OF_WEEK:
                                tempDate = tempDate.AddDays((fieldValue - 1) - (int)tempDate.DayOfWeek);
                                break;
                            case CalendarManager.DAY_OF_YEAR:
                                tempDate = tempDate.AddDays(fieldValue - tempDate.DayOfYear);
                                break;
                            case CalendarManager.HOUR_OF_DAY:
                                tempDate = tempDate.AddHours(fieldValue - tempDate.Hour);
                                break;

                            default:
                                break;
                        }
                        ((CalendarProperties)this[calendar]).dateTime = tempDate;
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        this.Add(calendar, tempProps);
                        this.Set(calendar, field, fieldValue);
                    }
                }

                /// <summary>
                /// Sets the corresponding date (day, month and year) to the calendar specified.
                /// If the calendar does not exist in the hash table, it creates a new instance and sets 
                /// its values.
                /// </summary>
                /// <param name="calendar">The calendar to set its date.</param>
                /// <param name="year">Integer value that represent the year.</param>
                /// <param name="month">Integer value that represent the month.</param>
                /// <param name="day">Integer value that represent the day.</param>
                public void Set(System.Globalization.Calendar calendar, int year, int month, int day)
                {
                    if (this[calendar] != null)
                    {
                        this.Set(calendar, CalendarManager.YEAR, year);
                        this.Set(calendar, CalendarManager.MONTH, month);
                        this.Set(calendar, CalendarManager.DATE, day);
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        this.Add(calendar, tempProps);
                        this.Set(calendar, year, month, day);
                    }
                }

                /// <summary>
                /// Sets the corresponding date (day, month and year) and hour (hour and minute) 
                /// to the calendar specified.
                /// If the calendar does not exist in the hash table, it creates a new instance and sets 
                /// its values.
                /// </summary>
                /// <param name="calendar">The calendar to set its date and time.</param>
                /// <param name="year">Integer value that represent the year.</param>
                /// <param name="month">Integer value that represent the month.</param>
                /// <param name="day">Integer value that represent the day.</param>
                /// <param name="hour">Integer value that represent the hour.</param>
                /// <param name="minute">Integer value that represent the minutes.</param>
                public void Set(System.Globalization.Calendar calendar, int year, int month, int day, int hour, int minute)
                {
                    if (this[calendar] != null)
                    {
                        this.Set(calendar, CalendarManager.YEAR, year);
                        this.Set(calendar, CalendarManager.MONTH, month);
                        this.Set(calendar, CalendarManager.DATE, day);
                        this.Set(calendar, CalendarManager.HOUR, hour);
                        this.Set(calendar, CalendarManager.MINUTE, minute);
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        this.Add(calendar, tempProps);
                        this.Set(calendar, year, month, day, hour, minute);
                    }
                }

                /// <summary>
                /// Sets the corresponding date (day, month and year) and hour (hour, minute and second) 
                /// to the calendar specified.
                /// If the calendar does not exist in the hash table, it creates a new instance and sets 
                /// its values.
                /// </summary>
                /// <param name="calendar">The calendar to set its date and time.</param>
                /// <param name="year">Integer value that represent the year.</param>
                /// <param name="month">Integer value that represent the month.</param>
                /// <param name="day">Integer value that represent the day.</param>
                /// <param name="hour">Integer value that represent the hour.</param>
                /// <param name="minute">Integer value that represent the minutes.</param>
                /// <param name="second">Integer value that represent the seconds.</param>
                public void Set(System.Globalization.Calendar calendar, int year, int month, int day, int hour, int minute, int second)
                {
                    if (this[calendar] != null)
                    {
                        this.Set(calendar, CalendarManager.YEAR, year);
                        this.Set(calendar, CalendarManager.MONTH, month);
                        this.Set(calendar, CalendarManager.DATE, day);
                        this.Set(calendar, CalendarManager.HOUR, hour);
                        this.Set(calendar, CalendarManager.MINUTE, minute);
                        this.Set(calendar, CalendarManager.SECOND, second);
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        this.Add(calendar, tempProps);
                        this.Set(calendar, year, month, day, hour, minute, second);
                    }
                }

                /// <summary>
                /// Gets the value represented by the field specified.
                /// </summary>
                /// <param name="calendar">The calendar to get its date or time.</param>
                /// <param name="field">One of the field that composes a date/time.</param>
                /// <returns>The integer value for the field given.</returns>
                public int Get(System.Globalization.Calendar calendar, int field)
                {
                    if (this[calendar] != null)
                    {
                        int tempHour;
                        switch (field)
                        {
                            case CalendarManager.DATE:
                                return ((CalendarProperties)this[calendar]).dateTime.Day;
                            case CalendarManager.HOUR:
                                tempHour = ((CalendarProperties)this[calendar]).dateTime.Hour;
                                return tempHour > 12 ? tempHour - 12 : tempHour;
                            case CalendarManager.MILLISECOND:
                                return ((CalendarProperties)this[calendar]).dateTime.Millisecond;
                            case CalendarManager.MINUTE:
                                return ((CalendarProperties)this[calendar]).dateTime.Minute;
                            case CalendarManager.MONTH:
                                //Month value is 0-based. e.g., 0 for January
                                return ((CalendarProperties)this[calendar]).dateTime.Month - 1;
                            case CalendarManager.SECOND:
                                return ((CalendarProperties)this[calendar]).dateTime.Second;
                            case CalendarManager.YEAR:
                                return ((CalendarProperties)this[calendar]).dateTime.Year;
                            case CalendarManager.DAY_OF_MONTH:
                                return ((CalendarProperties)this[calendar]).dateTime.Day;
                            case CalendarManager.DAY_OF_YEAR:
                                return (int)(((CalendarProperties)this[calendar]).dateTime.DayOfYear);
                            case CalendarManager.DAY_OF_WEEK:
                                return (int)(((CalendarProperties)this[calendar]).dateTime.DayOfWeek) + 1;
                            case CalendarManager.HOUR_OF_DAY:
                                return ((CalendarProperties)this[calendar]).dateTime.Hour;
                            case CalendarManager.AM_PM:
                                tempHour = ((CalendarProperties)this[calendar]).dateTime.Hour;
                                return tempHour > 12 ? CalendarManager.PM : CalendarManager.AM;

                            default:
                                return 0;
                        }
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        this.Add(calendar, tempProps);
                        return this.Get(calendar, field);
                    }
                }

                /// <summary>
                /// Sets the time in the specified calendar with the long value.
                /// </summary>
                /// <param name="calendar">The calendar to set its date and time.</param>
                /// <param name="milliseconds">A long value that indicates the milliseconds to be set to 
                /// the hour for the calendar.</param>
                public void SetTimeInMilliseconds(System.Globalization.Calendar calendar, long milliseconds)
                {
                    if (this[calendar] != null)
                    {
                        ((CalendarProperties)this[calendar]).dateTime = new System.DateTime(milliseconds);
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = new System.DateTime(System.TimeSpan.TicksPerMillisecond * milliseconds);
                        this.Add(calendar, tempProps);
                    }
                }

                /// <summary>
                /// Gets what the first day of the week is; e.g., Sunday in US, Monday in France.
                /// </summary>
                /// <param name="calendar">The calendar to get its first day of the week.</param>
                /// <returns>A System.DayOfWeek value indicating the first day of the week.</returns>
                public System.DayOfWeek GetFirstDayOfWeek(System.Globalization.Calendar calendar)
                {
                    if (this[calendar] != null)
                    {
                        if (((CalendarProperties)this[calendar]).dateTimeFormat == null)
                        {
                            ((CalendarProperties)this[calendar]).dateTimeFormat = new System.Globalization.DateTimeFormatInfo();
                            ((CalendarProperties)this[calendar]).dateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Sunday;
                        }
                        return ((CalendarProperties)this[calendar]).dateTimeFormat.FirstDayOfWeek;
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        tempProps.dateTimeFormat = new System.Globalization.DateTimeFormatInfo();
                        tempProps.dateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Sunday;
                        this.Add(calendar, tempProps);
                        return this.GetFirstDayOfWeek(calendar);
                    }
                }

                /// <summary>
                /// Sets what the first day of the week is; e.g., Sunday in US, Monday in France.
                /// </summary>
                /// <param name="calendar">The calendar to set its first day of the week.</param>
                /// <param name="firstDayOfWeek">A System.DayOfWeek value indicating the first day of the week
                /// to be set.</param>
                public void SetFirstDayOfWeek(System.Globalization.Calendar calendar, System.DayOfWeek firstDayOfWeek)
                {
                    if (this[calendar] != null)
                    {
                        if (((CalendarProperties)this[calendar]).dateTimeFormat == null)
                            ((CalendarProperties)this[calendar]).dateTimeFormat = new System.Globalization.DateTimeFormatInfo();

                        ((CalendarProperties)this[calendar]).dateTimeFormat.FirstDayOfWeek = firstDayOfWeek;
                    }
                    else
                    {
                        CalendarProperties tempProps = new CalendarProperties();
                        tempProps.dateTime = System.DateTime.Now;
                        tempProps.dateTimeFormat = new System.Globalization.DateTimeFormatInfo();
                        this.Add(calendar, tempProps);
                        this.SetFirstDayOfWeek(calendar, firstDayOfWeek);
                    }
                }

                /// <summary>
                /// Removes the specified calendar from the hash table.
                /// </summary>
                /// <param name="calendar">The calendar to be removed.</param>
                public void Clear(System.Globalization.Calendar calendar)
                {
                    if (this[calendar] != null)
                        this.Remove(calendar);
                }

                /// <summary>
                /// Removes the specified field from the calendar given.
                /// If the field does not exists in the calendar, the calendar is removed from the table.
                /// </summary>
                /// <param name="calendar">The calendar to remove the value from.</param>
                /// <param name="field">The field to be removed from the calendar.</param>
                public void Clear(System.Globalization.Calendar calendar, int field)
                {
                    if (this[calendar] != null)
                        this.Set(calendar, field, 0);
                }

                /// <summary>
                /// Internal class that represents the properties of a calendar instance.
                /// </summary>
                class CalendarProperties
                {
                    /// <summary>
                    /// The date and time of a calendar.
                    /// </summary>
                    public System.DateTime dateTime;

                    /// <summary>
                    /// The format for the date and time in a calendar.
                    /// </summary>
                    public System.Globalization.DateTimeFormatInfo dateTimeFormat;
                }
            }
        }
        /*******************************/
        /// <summary>
        /// Recieves a form and an integer value representing the operation to perform when the closing 
        /// event is fired.
        /// </summary>
        /// <param name="form">The form that fire the event.</param>
        /// <param name="operation">The operation to do while the form is closing.</param>
        public static void CloseOperation(System.Windows.Forms.Form form, int operation)
        {
            switch (operation)
            {
                case 0:
                    break;
                case 1:
                    form.Hide();
                    break;
                case 2:
                    form.Dispose();
                    break;
                case 3:
                    form.Dispose();
                    System.Windows.Forms.Application.Exit();
                    break;
            }
        }


        /*******************************/
        /// <summary>
        /// Contains methods to construct customized Buttons
        /// </summary>
        public class ButtonSupport
        {
            /// <summary>
            /// Creates a popup style Button with an specific text.	
            /// </summary>
            /// <param name="label">The text associated with the Button</param>
            /// <returns>The new Button</returns>
            public static System.Windows.Forms.Button CreateButton(System.String label)
            {
                System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
                tempButton.Text = label;
                tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                return tempButton;
            }

            /// <summary>
            /// Sets the an specific text for the Button
            /// </summary>
            /// <param name="Button">The button to be set</param>
            /// <param name="label">The text associated with the Button</param>
            public static void SetButton(System.Windows.Forms.ButtonBase Button, System.String label)
            {
                Button.Text = label;
                Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            }

            /// <summary>
            /// Creates a Button with an specific text and style.
            /// </summary>
            /// <param name="label">The text associated with the Button</param>
            /// <param name="style">The style of the Button</param>
            /// <returns>The new Button</returns>
            public static System.Windows.Forms.Button CreateButton(System.String label, int style)
            {
                System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
                tempButton.Text = label;
                setStyle(tempButton, style);
                return tempButton;
            }

            /// <summary>
            /// Sets the specific Text and Style for the Button
            /// </summary>
            /// <param name="Button">The button to be set</param>
            /// <param name="label">The text associated with the Button</param>
            /// <param name="style">The style of the Button</param>
            public static void SetButton(System.Windows.Forms.ButtonBase Button, System.String label, int style)
            {
                Button.Text = label;
                setStyle(Button, style);
            }

            /// <summary>
            /// Creates a standard style Button that contains an specific text and/or image
            /// </summary>
            /// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
            /// <returns>The new Button</returns>
            public static System.Windows.Forms.Button CreateButton(System.Windows.Forms.Control control)
            {
                System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
                if (control.GetType().FullName == "System.Windows.Forms.Label")
                {
                    tempButton.Image = ((System.Windows.Forms.Label)control).Image;
                    tempButton.Text = ((System.Windows.Forms.Label)control).Text;
                    tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                    tempButton.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
                }
                else
                {
                    if (control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
                    {
                        tempButton.Image = ((System.Windows.Forms.PictureBox)control).Image;
                        tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                    }
                    else
                        tempButton.Text = control.Text;
                }
                tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
                return tempButton;
            }

            /// <summary>
            /// Sets an specific text and/or image to the Button
            /// </summary>
            /// <param name="Button">The button to be set</param>
            /// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
            public static void SetButton(System.Windows.Forms.ButtonBase Button, System.Windows.Forms.Control control)
            {
                if (control.GetType().FullName == "System.Windows.Forms.Label")
                {
                    Button.Image = ((System.Windows.Forms.Label)control).Image;
                    Button.Text = ((System.Windows.Forms.Label)control).Text;
                    Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                    Button.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
                }
                else
                {
                    if (control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
                    {
                        Button.Image = ((System.Windows.Forms.PictureBox)control).Image;
                        Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                    }
                    else
                        Button.Text = control.Text;
                }
                Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            }

            /// <summary>
            /// Creates a Button with an specific control and style
            /// </summary>
            /// <param name="control">The control to be contained by the button</param>
            /// <param name="style">The style of the button</param>
            /// <returns>The new Button</returns>
            public static System.Windows.Forms.Button CreateButton(System.Windows.Forms.Control control, int style)
            {
                System.Windows.Forms.Button tempButton = CreateButton(control);
                setStyle(tempButton, style);
                return tempButton;
            }

            /// <summary>
            /// Sets an specific text and/or image to the Button
            /// </summary>
            /// <param name="Button">The button to be set</param>
            /// <param name="control">The control to be contained by the button</param>
            /// <param name="style">The style of the button</param>
            public static void SetButton(System.Windows.Forms.ButtonBase Button, System.Windows.Forms.Control control, int style)
            {
                SetButton(Button, control);
                setStyle(Button, style);
            }

            /// <summary>
            /// Sets the style of the Button
            /// </summary>
            /// <param name="Button">The Button that will change its style</param>
            /// <param name="style">The new style of the Button</param>
            /// <remarks> 
            /// If style is 0 then sets a popup style to the Button, otherwise sets a standard style to the Button.
            /// </remarks>
            public static void setStyle(System.Windows.Forms.ButtonBase Button, int style)
            {
                if ((style == 0) || (style == 67108864) || (style == 33554432))
                    Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                else if ((style == 2097152) || (style == 1048576) || (style == 16777216))
                    Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
                else
                    throw new System.ArgumentException("illegal style: " + style);
            }

            /// <summary>
            /// Selects the Button
            /// </summary>
            /// <param name="Button">The Button that will change its style</param>
            /// <param name="select">It determines if the button woll be selected</param>
            /// <remarks> 
            /// If select is true thebutton will be selected, otherwise not.
            /// </remarks>
            public static void setSelected(System.Windows.Forms.ButtonBase Button, bool select)
            {
                if (select)
                    Button.Select();
            }

            /// <summary>
            /// Receives a Button instance and sets the Text and Image properties.
            /// </summary>
            /// <param name="buttonInstance">Button instance to be set.</param>
            /// <param name="buttonText">Value to be set to Text.</param>
            /// <param name="icon">Value to be set to Image.</param>
            public static void SetStandardButton(System.Windows.Forms.ButtonBase buttonInstance, System.String buttonText, System.Drawing.Image icon)
            {
                buttonInstance.Text = buttonText;
                buttonInstance.Image = icon;
            }

            /// <summary>
            /// Creates a Button with a given text.
            /// </summary>
            /// <param name="buttonText">The text to be displayed in the button.</param>
            /// <returns>The new created button with text</returns>
            public static System.Windows.Forms.Button CreateStandardButton(System.String buttonText)
            {
                System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
                newButton.Text = buttonText;
                return newButton;
            }

            /// <summary>
            /// Creates a Button with a given image.
            /// </summary>
            /// <param name="buttonImage">The image to be displayed in the button.</param>
            /// <returns>The new created button with an image</returns>
            public static System.Windows.Forms.Button CreateStandardButton(System.Drawing.Image buttonImage)
            {
                System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
                newButton.Image = buttonImage;
                return newButton;
            }

            /// <summary>
            /// Creates a Button with a given image and a text.
            /// </summary>
            /// <param name="buttonText">The text to be displayed in the button.</param>
            /// <param name="buttonImage">The image to be displayed in the button.</param>
            /// <returns>The new created button with text and image</returns>
            public static System.Windows.Forms.Button CreateStandardButton(System.String buttonText, System.Drawing.Image buttonImage)
            {
                System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
                newButton.Text = buttonText;
                newButton.Image = buttonImage;
                return newButton;
            }
        }
        /*******************************/
        /// <summary>
        /// This method works as a handler for the Control.Layout event, it arranges the controls into a container
        /// control in a rectangular grid (rows and columns).
        /// The size and location of each inner control will be calculated according the number of them in the 
        /// container.
        /// The number of columns, rows, horizontal and vertical spacing between the inner controls will are
        /// specified as array of object values in the Tag property of the container.
        /// If the number of rows and columns specified is not enought to allocate all the controls, the number of 
        /// columns will be increased in order to maintain the number of rows specified.
        /// </summary>
        /// <param name="event_sender">The container control in which the controls will be relocated.</param>
        /// <param name="eventArgs">Data of the event.</param>
        public static void GridLayoutResize(System.Object event_sender, System.Windows.Forms.LayoutEventArgs eventArgs)
        {
            System.Windows.Forms.Control container = (System.Windows.Forms.Control)event_sender;
            if ((container.Tag is System.Drawing.Rectangle) && (container.Controls.Count > 0))
            {
                System.Drawing.Rectangle tempRectangle = (System.Drawing.Rectangle)container.Tag;

                if ((tempRectangle.X <= 0) && (tempRectangle.Y <= 0))
                    throw new System.Exception("Illegal column and row layout count specified");

                int horizontal = tempRectangle.Width;
                int vertical = tempRectangle.Height;
                int count = container.Controls.Count;

                int rows = (tempRectangle.X == 0) ? (int)System.Math.Ceiling((double)(count / tempRectangle.Y)) : tempRectangle.X;
                int columns = (tempRectangle.Y == 0) ? (int)System.Math.Ceiling((double)(count / tempRectangle.X)) : tempRectangle.Y;

                rows = (rows == 0) ? 1 : rows;
                columns = (columns == 0) ? 1 : columns;

                while (count > rows * columns) columns++;

                int width = (container.DisplayRectangle.Width - horizontal * (columns - 1)) / columns;
                int height = (container.DisplayRectangle.Height - vertical * (rows - 1)) / rows;

                int indexColumn = 0;
                int indexRow = 0;

                foreach (System.Windows.Forms.Control tempControl in container.Controls)
                {
                    int xCoordinate = indexColumn++ * (width + horizontal);
                    int yCoordinate = indexRow * (height + vertical);
                    tempControl.Location = new System.Drawing.Point(xCoordinate, yCoordinate);
                    tempControl.Width = width;
                    tempControl.Height = height;
                    if (indexColumn == columns)
                    {
                        indexColumn = 0;
                        indexRow++;
                    }
                }
            }
        }


        /*******************************/
        /// <summary>
        /// Support class for creation of Forms behaving like Dialog windows.
        /// </summary>
        public class DialogSupport
        {
            /// <summary>
            /// Creates a dialog Form.
            /// </summary>
            /// <returns>The new dialog Form instance.</returns>
            public static System.Windows.Forms.Form CreateDialog()
            {
                System.Windows.Forms.Form tempForm = new System.Windows.Forms.Form();
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                tempForm.ShowInTaskbar = false;
                return tempForm;
            }

            /// <summary>
            /// Sets dialog like properties to a Form.
            /// </summary>
            /// <param name="formInstance">Form instance to be modified.</param>
            public static void SetDialog(System.Windows.Forms.Form formInstance)
            {
                formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                formInstance.ShowInTaskbar = false;
            }

            /// <summary>
            /// Creates a dialog Form with an owner.
            /// </summary>
            /// <param name="owner">The form to be set as owner of the newly created one.</param>
            /// <returns>A new dialog Form.</returns>
            public static System.Windows.Forms.Form CreateDialog(System.Windows.Forms.Form owner)
            {
                System.Windows.Forms.Form tempForm = new System.Windows.Forms.Form();
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                tempForm.ShowInTaskbar = false;
                tempForm.Owner = owner;
                return tempForm;
            }

            /// <summary>
            /// Sets dialog like properties and an owner to a Form.
            /// </summary>
            /// <param name="formInstance">Form instance to be modified.</param>
            /// <param name="owner">The form to be set as owner of the newly created one.</param>
            public static void SetDialog(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner)
            {
                formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                formInstance.ShowInTaskbar = false;
                formInstance.Owner = owner;
            }


            /// <summary>
            /// Creates a dialog Form with an owner and a text property.
            /// </summary>
            /// <param name="owner">The form to be set as owner of the newly created one.</param>
            /// <param name="text">The title text for the form.</param>
            /// <returns>The new dialog Form.</returns>
            public static System.Windows.Forms.Form CreateDialog(System.Windows.Forms.Form owner, System.String text)
            {
                System.Windows.Forms.Form tempForm = new System.Windows.Forms.Form();
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                tempForm.ShowInTaskbar = false;
                tempForm.Owner = owner;
                tempForm.Text = text;
                return tempForm;
            }

            /// <summary>
            /// Sets dialog like properties, an owner and a text property to a Form.
            /// </summary>
            /// <param name="formInstance">Form instance to be modified.</param>
            /// <param name="owner">The form to be set as owner of the newly created one.</param>
            /// <param name="text">The title text for the form.</param>
            public static void SetDialog(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner, System.String text)
            {
                formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                formInstance.ShowInTaskbar = false;
                formInstance.Owner = owner;
                formInstance.Text = text;
            }


            /// <summary>
            /// This method sets or unsets a resizable border style to a Form.
            /// </summary>
            /// <param name="formInstance">The form to be modified.</param>
            /// <param name="sizable">Boolean value to be set.</param>
            public static void SetSizable(System.Windows.Forms.Form formInstance, bool sizable)
            {
                if (sizable)
                {
                    formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                }
                else
                {
                    formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                }
            }
        }


        /*******************************/
        /// <summary>
        /// This class contains static methods to manage TreeViews.
        /// </summary>
        public class TreeSupport
        {
            /// <summary>
            /// Creates a new TreeView from the provided HashTable.
            /// </summary> 
            /// <param name="hashTable">HashTable</param>		
            /// <returns>Returns the created tree</returns>
            public static System.Windows.Forms.TreeView CreateTreeView(System.Collections.Hashtable hashTable)
            {
                System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
                return SetTreeView(tree, hashTable);
            }

            /// <summary>
            /// Sets a TreeView with data from the provided HashTable.
            /// </summary> 
            /// <param name="treeView">Tree.</param>
            /// <param name="hashTable">HashTable.</param>
            /// <returns>Returns the set tree.</returns>		
            public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Collections.Hashtable hashTable)
            {
                foreach (System.Collections.DictionaryEntry myEntry in hashTable)
                {
                    System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

                    if (myEntry.Value is System.Collections.ArrayList)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.ArrayList)myEntry.Value);
                        root.Text = root.Text + "]";
                    }
                    else if (myEntry.Value is System.Object[])
                    {
                        root.Text = "[";
                        FillNode(root, (System.Object[])myEntry.Value);
                        root.Text = root.Text + "]";
                    }
                    else if (myEntry.Value is System.Collections.Hashtable)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.Hashtable)myEntry.Value);
                        root.Text = root.Text + "]";
                    }
                    else
                        root.Text = myEntry.ToString();

                    treeView.Nodes.Add(root);
                }
                return treeView;
            }


            /// <summary>
            /// Creates a new TreeView from the provided ArrayList.
            /// </summary> 
            /// <param name="arrayList">ArrayList.</param>		
            /// <returns>Returns the created tree.</returns>
            public static System.Windows.Forms.TreeView CreateTreeView(System.Collections.ArrayList arrayList)
            {
                System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
                return SetTreeView(tree, arrayList);
            }

            /// <summary>
            /// Sets a TreeView with data from the provided ArrayList.
            /// </summary> 
            /// <param name="treeView">Tree.</param>
            /// <param name="arrayList">ArrayList.</param>
            /// <returns>Returns the set tree.</returns>
            public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Collections.ArrayList arrayList)
            {
                foreach (System.Object arrayEntry in arrayList)
                {
                    System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

                    if (arrayEntry is System.Collections.ArrayList)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.ArrayList)arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else if (arrayEntry is System.Collections.Hashtable)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.Hashtable)arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else if (arrayEntry is System.Object[])
                    {
                        root.Text = "[";
                        FillNode(root, (System.Object[])arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else
                        root.Text = arrayEntry.ToString();


                    treeView.Nodes.Add(root);
                }
                return treeView;
            }

            /// <summary>
            /// Creates a new TreeView from the provided Object Array.
            /// </summary> 
            /// <param name="objectArray">Object Array.</param>		
            /// <returns>Returns the created tree.</returns>
            public static System.Windows.Forms.TreeView CreateTreeView(System.Object[] objectArray)
            {
                System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
                return SetTreeView(tree, objectArray);
            }

            /// <summary>
            /// Sets a TreeView with data from the provided Object Array.
            /// </summary> 
            /// <param name="treeView">Tree.</param>
            /// <param name="objectArray">Object Array.</param>
            /// <returns>Returns the created tree.</returns>
            public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Object[] objectArray)
            {
                foreach (System.Object arrayEntry in objectArray)
                {
                    System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

                    if (arrayEntry is System.Collections.ArrayList)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.ArrayList)arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else if (arrayEntry is System.Collections.Hashtable)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.Hashtable)arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else if (arrayEntry is System.Object[])
                    {
                        root.Text = "[";
                        FillNode(root, (System.Object[])arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else
                        root.Text = arrayEntry.ToString();

                    treeView.Nodes.Add(root);
                }
                return treeView;
            }

            /// <summary>
            /// Creates a new TreeView with the provided TreeNode as root.
            /// </summary> 
            /// <param name="root">Root.</param>		
            /// <returns>Returns the created tree.</returns>
            public static System.Windows.Forms.TreeView CreateTreeView(System.Windows.Forms.TreeNode root)
            {
                System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
                return SetTreeView(tree, root);
            }

            /// <summary>
            /// Sets a TreeView with the provided TreeNode as root.
            /// </summary>
            /// <param name="treeView">Tree</param>
            /// <param name="root">Root</param>
            /// <returns>Returns the created tree</returns>
            public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Windows.Forms.TreeNode root)
            {
                if (root != null)
                    treeView.Nodes.Add(root);
                return treeView;
            }

            /// <summary>
            /// Sets a TreeView with the provided TreeNode as root.
            /// </summary> 
            /// <param name="model">Root data model.</param>
            public static void SetModel(System.Windows.Forms.TreeView tree, System.Windows.Forms.TreeNode model)
            {
                tree.Nodes.Clear();
                tree.Nodes.Add(model);
            }

            /// <summary>
            /// Get the root TreeNode from a TreeView.
            /// </summary> 
            /// <param name="tree">Tree.</param>
            public static System.Windows.Forms.TreeNode GetModel(System.Windows.Forms.TreeView tree)
            {
                if (tree.Nodes.Count > 0)
                    return tree.Nodes[0];
                else
                    return null;
            }

            /// <summary>
            /// Sets a TreeNode with data from the provided ArrayList instance.
            /// </summary> 
            /// <param name="treeNode">Node.</param>
            /// <param name="arrayList">ArrayList.</param>
            /// <returns>Returns the set node.</returns>
            public static System.Windows.Forms.TreeNode FillNode(System.Windows.Forms.TreeNode treeNode, System.Collections.ArrayList arrayList)
            {
                foreach (System.Object arrayEntry in arrayList)
                {
                    System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

                    if (arrayEntry is System.Collections.ArrayList)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.ArrayList)arrayEntry);
                        root.Text = root.Text + "]";
                        treeNode.Nodes.Add(root);
                        treeNode.Text = treeNode.Text + ", " + root.Text;
                    }
                    else if (arrayEntry is System.Object[])
                    {
                        root.Text = "[";
                        FillNode(root, (System.Object[])arrayEntry);
                        root.Text = root.Text + "]";
                        treeNode.Nodes.Add(root);
                        treeNode.Text = treeNode.Text + ", " + root.Text;
                    }
                    else if (arrayEntry is System.Collections.Hashtable)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.Hashtable)arrayEntry);
                        root.Text = root.Text + "]";
                        treeNode.Nodes.Add(root);
                        treeNode.Text = treeNode.Text + ", " + root.Text;
                    }
                    else
                    {
                        treeNode.Nodes.Add(arrayEntry.ToString());
                        if (!(treeNode.Text.Equals("")))
                        {
                            if (treeNode.Text[treeNode.Text.Length - 1].Equals('['))
                                treeNode.Text = treeNode.Text + arrayEntry.ToString();
                            else
                                treeNode.Text = treeNode.Text + ", " + arrayEntry.ToString();
                        }
                        else
                            treeNode.Text = treeNode.Text + ", " + arrayEntry.ToString();
                    }
                }
                return treeNode;
            }


            /// <summary>
            /// Sets a TreeNode with data from the provided ArrayList.
            /// </summary> 
            /// <param name="treeNode">Node.</param>
            /// <param name="objectArray">Object Array.</param>
            /// <returns>Returns the set node.</returns>

            public static System.Windows.Forms.TreeNode FillNode(System.Windows.Forms.TreeNode treeNode, System.Object[] objectArray)
            {
                foreach (System.Object arrayEntry in objectArray)
                {
                    System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

                    if (arrayEntry is System.Collections.ArrayList)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.ArrayList)arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else if (arrayEntry is System.Collections.Hashtable)
                    {
                        root.Text = "[";
                        FillNode(root, (System.Collections.Hashtable)arrayEntry);
                        root.Text = root.Text + "]";
                    }
                    else
                    {
                        root.Nodes.Add(objectArray.ToString());
                        root.Text = root.Text + ", " + objectArray.ToString();
                    }
                    treeNode.Nodes.Add(root);
                    treeNode.Text = treeNode.Text + root.Text;
                }
                return treeNode;
            }

            /// <summary>		
            /// Sets a TreeNode with data from the provided Hashtable.		
            /// </summary> 		
            /// <param name="treeNode">Node.</param>		
            /// <param name="hashTable">Hash Table Object.</param>		
            /// <returns>Returns the set node.</returns>		
            public static System.Windows.Forms.TreeNode FillNode(System.Windows.Forms.TreeNode treeNode, System.Collections.Hashtable hashTable)
            {
                foreach (System.Collections.DictionaryEntry myEntry in hashTable)
                {
                    System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

                    if (myEntry.Value is System.Collections.ArrayList)
                    {
                        FillNode(root, (System.Collections.ArrayList)myEntry.Value);
                        treeNode.Nodes.Add(root);
                    }
                    else if (myEntry.Value is System.Object[])
                    {
                        FillNode(root, (System.Object[])myEntry.Value);
                        treeNode.Nodes.Add(root);
                    }
                    else
                        treeNode.Nodes.Add(myEntry.Key.ToString());
                }
                return treeNode;
            }
        }
        /*******************************/
        /// <summary>
        /// This class contains static methods to manage tab controls.
        /// </summary>
        public class TabControlSupport
        {
            /// <summary>
            /// Create a new instance of TabControl and set the alignment property.
            /// </summary>
            /// <param name="alignment">The alignment property value.</param>
            /// <returns>New TabControl instance.</returns>
            public static System.Windows.Forms.TabControl CreateTabControl(System.Windows.Forms.TabAlignment alignment)
            {
                System.Windows.Forms.TabControl tabcontrol = new System.Windows.Forms.TabControl();
                tabcontrol.Alignment = alignment;
                return tabcontrol;
            }

            /// <summary>
            /// Set the alignment property to an instance of TabControl .
            /// </summary>
            /// <param name="tabcontrol">An instance of TabControl.</param>
            /// <param name="alignment">The alignment property value.</param>
            public static void SetTabControl(System.Windows.Forms.TabControl tabcontrol, System.Windows.Forms.TabAlignment alignment)
            {
                tabcontrol.Alignment = alignment;
            }

            /// <summary>
            /// Method to add TabPages into the TabControl object.
            /// </summary>
            /// <param name="tabControl">The TabControl to be modified.</param>
            /// <param name="component">A component to be added into the new TabControl.</param>
            public static System.Windows.Forms.Control AddTab(System.Windows.Forms.TabControl tabControl, System.Windows.Forms.Control component)
            {
                System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
                tabPage.Controls.Add(component);
                tabControl.TabPages.Add(tabPage);
                return component;
            }

            /// <summary>
            /// Method to add TabPages into the TabControl object.
            /// </summary>
            /// <param name="tabControl">The TabControl to be modified.</param>
            /// <param name="TabLabel">The label for the new TabPage.</param>
            /// <param name="component">A component to be added into the new TabControl.</param>
            public static System.Windows.Forms.Control AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Windows.Forms.Control component)
            {
                System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
                tabPage.Controls.Add(component);
                tabControl.TabPages.Add(tabPage);
                return component;
            }

            /// <summary>
            /// Method to add TabPages into the TabControl object.
            /// </summary>
            /// <param name="tabControl">The TabControl to be modified.</param>
            /// <param name="component">A component to be added into the new TabControl.</param>
            /// <param name="constraints">The object that should be displayed in the tab but won't because of limitations</param>		
            public static void AddTab(System.Windows.Forms.TabControl tabControl, System.Windows.Forms.Control component, System.Object constraints)
            {
                System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
                if (constraints is System.String)
                {
                    tabPage.Text = (String)constraints;
                }
                tabPage.Controls.Add(component);
                tabControl.TabPages.Add(tabPage);
            }

            /// <summary>
            /// Method to add TabPages into the TabControl object.
            /// </summary>
            /// <param name="tabControl">The TabControl to be modified.</param>
            /// <param name="TabLabel">The label for the new TabPage.</param>
            /// <param name="constraints">The object that should be displayed in the tab but won't because of limitations</param>
            /// <param name="component">A component to be added into the new TabControl.</param>
            public static void AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Object constraints, System.Windows.Forms.Control component)
            {
                System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
                tabPage.Controls.Add(component);
                tabControl.TabPages.Add(tabPage);
            }

            /// <summary>
            /// Method to add TabPages into the TabControl object.
            /// </summary>
            /// <param name="tabControl">The TabControl to be modified.</param>
            /// <param name="tabLabel">The label for the new TabPage.</param>
            /// <param name="image">Background image for the TabPage.</param>
            /// <param name="component">A component to be added into the new TabControl.</param>
            public static void AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Drawing.Image image, System.Windows.Forms.Control component)
            {
                System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
                tabPage.BackgroundImage = image;
                tabPage.Controls.Add(component);
                tabControl.TabPages.Add(tabPage);
            }
        }


        /*******************************/
        /// <summary>
        /// Give functions to obtain information of graphic elements
        /// </summary>
        public class GraphicsManager
        {
            //Instance of GDI+ drawing surfaces graphics hashtable
            static public GraphicsHashTable manager = new GraphicsHashTable();

            /// <summary>
            /// Creates a new Graphics object from the device context handle associated with the Graphics
            /// parameter
            /// </summary>
            /// <param name="oldGraphics">Graphics instance to obtain the parameter from</param>
            /// <returns>A new GDI+ drawing surface</returns>
            public static System.Drawing.Graphics CreateGraphics(System.Drawing.Graphics oldGraphics)
            {
                System.Drawing.Graphics createdGraphics;
                System.IntPtr hdc = oldGraphics.GetHdc();
                createdGraphics = System.Drawing.Graphics.FromHdc(hdc);
                oldGraphics.ReleaseHdc(hdc);
                return createdGraphics;
            }

            /// <summary>
            /// This method draws a Bezier curve.
            /// </summary>
            /// <param name="graphics">It receives the Graphics instance</param>
            /// <param name="array">An array of (x,y) pairs of coordinates used to draw the curve.</param>
            public static void Bezier(System.Drawing.Graphics graphics, int[] array)
            {
                System.Drawing.Pen pen;
                pen = GraphicsManager.manager.GetPen(graphics);
                try
                {
                    graphics.DrawBezier(pen, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
                }
                catch (System.IndexOutOfRangeException e)
                {
                    throw new System.IndexOutOfRangeException(e.ToString());
                }
            }

            /// <summary>
            /// Gets the text size width and height from a given GDI+ drawing surface and a given font
            /// </summary>
            /// <param name="graphics">Drawing surface to use</param>
            /// <param name="graphicsFont">Font type to measure</param>
            /// <param name="text">String of text to measure</param>
            /// <returns>A point structure with both size dimentions; x for width and y for height</returns>
            public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text)
            {
                System.Drawing.Point textSize;
                System.Drawing.SizeF tempSizeF;
                tempSizeF = graphics.MeasureString(text, graphicsFont);
                textSize = new System.Drawing.Point();
                textSize.X = (int)tempSizeF.Width;
                textSize.Y = (int)tempSizeF.Height;
                return textSize;
            }

            /// <summary>
            /// Gets the text size width and height from a given GDI+ drawing surface and a given font
            /// </summary>
            /// <param name="graphics">Drawing surface to use</param>
            /// <param name="graphicsFont">Font type to measure</param>
            /// <param name="text">String of text to measure</param>
            /// <param name="width">Maximum width of the string</param>
            /// <param name="format">StringFormat object that represents formatting information, such as line spacing, for the string</param>
            /// <returns>A point structure with both size dimentions; x for width and y for height</returns>
            public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text, System.Int32 width, System.Drawing.StringFormat format)
            {
                System.Drawing.Point textSize;
                System.Drawing.SizeF tempSizeF;
                tempSizeF = graphics.MeasureString(text, graphicsFont, width, format);
                textSize = new System.Drawing.Point();
                textSize.X = (int)tempSizeF.Width;
                textSize.Y = (int)tempSizeF.Height;
                return textSize;
            }

            /// <summary>
            /// Gives functionality over a hashtable of GDI+ drawing surfaces
            /// </summary>
            public class GraphicsHashTable : System.Collections.Hashtable
            {
                /// <summary>
                /// Gets the graphics object from the given control
                /// </summary>
                /// <param name="control">Control to obtain the graphics from</param>
                /// <returns>A graphics object with the control's characteristics</returns>
                public System.Drawing.Graphics GetGraphics(System.Windows.Forms.Control control)
                {
                    System.Drawing.Graphics graphic;
                    if (control.Visible == true)
                    {
                        graphic = control.CreateGraphics();
                        SetColor(graphic, control.ForeColor);
                        SetFont(graphic, control.Font);
                    }
                    else
                    {
                        graphic = null;
                    }
                    return graphic;
                }

                /// <summary>
                /// Sets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given background color.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="color">Background color to set</param>
                public void SetBackColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
                {
                    if (this[graphic] != null)
                        ((GraphicsProperties)this[graphic]).BackColor = color;
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.BackColor = color;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Gets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
                /// </summary>
                /// <param name="graphic">Graphic element to search</param>
                /// <returns>The background color of the graphic</returns>
                public System.Drawing.Color GetBackColor(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return System.Drawing.Color.White;
                    else
                        return ((GraphicsProperties)this[graphic]).BackColor;
                }

                /// <summary>
                /// Sets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given text color.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="color">Text color to set</param>
                public void SetTextColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
                {
                    if (this[graphic] != null)
                        ((GraphicsProperties)this[graphic]).TextColor = color;
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.TextColor = color;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Gets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
                /// </summary>
                /// <param name="graphic">Graphic element to search</param>
                /// <returns>The text color of the graphic</returns>
                public System.Drawing.Color GetTextColor(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return System.Drawing.Color.White;
                    else
                        return ((GraphicsProperties)this[graphic]).TextColor;
                }

                /// <summary>
                /// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="brush">GraphicBrush to set</param>
                public void SetBrush(System.Drawing.Graphics graphic, System.Drawing.SolidBrush brush)
                {
                    if (this[graphic] != null)
                        ((GraphicsProperties)this[graphic]).GraphicBrush = brush;
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.GraphicBrush = brush;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="brush">GraphicBrush to set</param>
                public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Brush brush)
                {
                    if (this[graphic] != null)
                        ((GraphicsProperties)this[graphic]).PaintBrush = brush;
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.PaintBrush = brush;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="color">Color to set</param>
                public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Color color)
                {
                    System.Drawing.Brush brush = new System.Drawing.SolidBrush(color);
                    if (this[graphic] != null)
                        ((GraphicsProperties)this[graphic]).PaintBrush = brush;
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.PaintBrush = brush;
                        Add(graphic, tempProps);
                    }
                }


                /// <summary>
                /// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
                /// </summary>
                /// <param name="graphic">Graphic element to search</param>
                /// <returns>The HatchBrush setting of the graphic</returns>
                public System.Drawing.Drawing2D.HatchBrush GetBrush(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid, System.Drawing.Color.Black, System.Drawing.Color.Black);
                    else
                        return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid, ((GraphicsProperties)this[graphic]).GraphicBrush.Color, ((GraphicsProperties)this[graphic]).GraphicBrush.Color);
                }

                /// <summary>
                /// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
                /// </summary>
                /// <param name="graphic">Graphic element to search</param>
                /// <returns>The Brush setting of the graphic</returns>
                public System.Drawing.Brush GetPaint(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid, System.Drawing.Color.Black, System.Drawing.Color.Black);
                    else
                        return ((GraphicsProperties)this[graphic]).PaintBrush;
                }

                /// <summary>
                /// Sets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Pen.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="pen">Pen to set</param>
                public void SetPen(System.Drawing.Graphics graphic, System.Drawing.Pen pen)
                {
                    if (this[graphic] != null)
                        ((GraphicsProperties)this[graphic]).GraphicPen = pen;
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.GraphicPen = pen;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Gets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
                /// </summary>
                /// <param name="graphic">Graphic element to search</param>
                /// <returns>The GraphicPen setting of the graphic</returns>
                public System.Drawing.Pen GetPen(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return System.Drawing.Pens.Black;
                    else
                        return ((GraphicsProperties)this[graphic]).GraphicPen;
                }

                /// <summary>
                /// Sets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Font.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="Font">Font to set</param>
                public void SetFont(System.Drawing.Graphics graphic, System.Drawing.Font font)
                {
                    if (this[graphic] != null)
                        ((GraphicsProperties)this[graphic]).GraphicFont = font;
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.GraphicFont = font;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Gets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Microsoft Sans Serif with size 8.25.
                /// </summary>
                /// <param name="graphic">Graphic element to search</param>
                /// <returns>The GraphicFont setting of the graphic</returns>
                public System.Drawing.Font GetFont(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                    else
                        return ((GraphicsProperties)this[graphic]).GraphicFont;
                }

                /// <summary>
                /// Sets the color properties for a given Graphics object. If the element doesn't exist, then it adds the graphic element to the hashtable with the color properties set with the given value.
                /// </summary>
                /// <param name="graphic">Graphic element to search or add</param>
                /// <param name="color">Color value to set</param>
                public void SetColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
                {
                    if (this[graphic] != null)
                    {
                        ((GraphicsProperties)this[graphic]).GraphicPen.Color = color;
                        ((GraphicsProperties)this[graphic]).GraphicBrush.Color = color;
                        ((GraphicsProperties)this[graphic]).color = color;
                    }
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.GraphicPen.Color = color;
                        tempProps.GraphicBrush.Color = color;
                        tempProps.color = color;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Gets the color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
                /// </summary>
                /// <param name="graphic">Graphic element to search</param>
                /// <returns>The color setting of the graphic</returns>
                public System.Drawing.Color GetColor(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return System.Drawing.Color.Black;
                    else
                        return ((GraphicsProperties)this[graphic]).color;
                }

                /// <summary>
                /// This method gets the TextBackgroundColor of a Graphics instance
                /// </summary>
                /// <param name="graphic">The graphics instance</param>
                /// <returns>The color value in ARGB encoding</returns>
                public System.Drawing.Color GetTextBackgroundColor(System.Drawing.Graphics graphic)
                {
                    if (this[graphic] == null)
                        return System.Drawing.Color.Black;
                    else
                    {
                        return ((GraphicsProperties)this[graphic]).TextBackgroundColor;
                    }
                }

                /// <summary>
                /// This method set the TextBackgroundColor of a Graphics instace
                /// </summary>
                /// <param name="graphic">The graphics instace</param>
                /// <param name="color">The System.Color to set the TextBackgroundColor</param>
                public void SetTextBackgroundColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
                {
                    if (this[graphic] != null)
                    {
                        ((GraphicsProperties)this[graphic]).TextBackgroundColor = color;
                    }
                    else
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.TextBackgroundColor = color;
                        Add(graphic, tempProps);
                    }
                }

                /// <summary>
                /// Structure to store properties from System.Drawing.Graphics objects
                /// </summary>
                class GraphicsProperties
                {
                    public System.Drawing.Color TextBackgroundColor = System.Drawing.Color.Black;
                    public System.Drawing.Color color = System.Drawing.Color.Black;
                    public System.Drawing.Color BackColor = System.Drawing.Color.White;
                    public System.Drawing.Color TextColor = System.Drawing.Color.Black;
                    public System.Drawing.SolidBrush GraphicBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    public System.Drawing.Brush PaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    public System.Drawing.Pen GraphicPen = new System.Drawing.Pen(System.Drawing.Color.Black);
                    public System.Drawing.Font GraphicFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                }
            }
        }

        /*******************************/
        /// <summary>
        /// Represents the methods to support some operations over files.
        /// </summary>
        public class FileSupport
        {
            /// <summary>
            /// Creates a new empty file with the specified pathname.
            /// </summary>
            /// <param name="path">The abstract pathname of the file</param>
            /// <returns>True if the file does not exist and was succesfully created</returns>
            public static bool CreateNewFile(System.IO.FileInfo path)
            {
                if (path.Exists)
                {
                    return false;
                }
                else
                {
                    System.IO.FileStream createdFile = path.Create();
                    createdFile.Close();
                    return true;
                }
            }

            /// <summary>
            /// Compares the specified object with the specified path
            /// </summary>
            /// <param name="path">An abstract pathname to compare with</param>
            /// <param name="file">An object to compare with the given pathname</param>
            /// <returns>A value indicating a lexicographically comparison of the parameters</returns>
            public static int CompareTo(System.IO.FileInfo path, System.Object file)
            {
                if (file is System.IO.FileInfo)
                {
                    System.IO.FileInfo fileInfo = (System.IO.FileInfo)file;
                    return path.FullName.CompareTo(fileInfo.FullName);
                }
                else
                {
                    throw new System.InvalidCastException();
                }
            }

            /// <summary>
            /// Returns an array of abstract pathnames representing the files and directories of the specified path.
            /// </summary>
            /// <param name="path">The abstract pathname to list it childs.</param>
            /// <returns>An array of abstract pathnames childs of the path specified or null if the path is not a directory</returns>
            public static System.IO.FileInfo[] GetFiles(System.IO.FileInfo path)
            {
                if ((path.Attributes & System.IO.FileAttributes.Directory) > 0)
                {
                    String[] fullpathnames = System.IO.Directory.GetFileSystemEntries(path.FullName);
                    System.IO.FileInfo[] result = new System.IO.FileInfo[fullpathnames.Length];
                    for (int i = 0; i < result.Length; i++)
                        result[i] = new System.IO.FileInfo(fullpathnames[i]);
                    return result;
                }
                else return null;
            }

            /// <summary>
            /// Creates an instance of System.Uri class with the pech specified
            /// </summary>
            /// <param name="path">The abstract path name to create the Uri</param>
            /// <returns>A System.Uri instance constructed with the specified path</returns>
            public static System.Uri ToUri(System.IO.FileInfo path)
            {
                System.UriBuilder uri = new System.UriBuilder();
                uri.Path = path.FullName;
                uri.Host = String.Empty;
                uri.Scheme = System.Uri.UriSchemeFile;
                return uri.Uri;
            }

            /// <summary>
            /// Returns true if the file specified by the pathname is a hidden file.
            /// </summary>
            /// <param name="file">The abstract pathname of the file to test</param>
            /// <returns>True if the file is hidden, false otherwise</returns>
            public static bool IsHidden(System.IO.FileInfo file)
            {
                return ((file.Attributes & System.IO.FileAttributes.Hidden) > 0);
            }

            /// <summary>
            /// Sets the read-only property of the file to true.
            /// </summary>
            /// <param name="file">The abstract path name of the file to modify</param>
            public static bool SetReadOnly(System.IO.FileInfo file)
            {
                try
                {
                    file.Attributes = file.Attributes | System.IO.FileAttributes.ReadOnly;
                    return true;
                }
                catch (System.Exception exception)
                {
                    String exceptionMessage = exception.Message;
                    return false;
                }
            }

            /// <summary>
            /// Sets the last modified time of the specified file with the specified value.
            /// </summary>
            /// <param name="file">The file to change it last-modified time</param>
            /// <param name="date">Total number of miliseconds since January 1, 1970 (new last-modified time)</param>
            /// <returns>True if the operation succeeded, false otherwise</returns>
            public static bool SetLastModified(System.IO.FileInfo file, long date)
            {
                try
                {
                    long valueConstant = (new System.DateTime(1969, 12, 31, 18, 0, 0)).Ticks;
                    file.LastWriteTime = new System.DateTime((date * 10000L) + valueConstant);
                    return true;
                }
                catch (System.Exception exception)
                {
                    String exceptionMessage = exception.Message;
                    return false;
                }
            }
        }
        /*******************************/
        /// <summary>
        /// Contains methods to get an set a ToolTip
        /// </summary>
        public class ToolTipSupport
        {
            static System.Windows.Forms.ToolTip supportToolTip = new System.Windows.Forms.ToolTip();

            /// <summary>
            /// Get the ToolTip text for the specific control parameter.
            /// </summary>
            /// <param name="control">The control with the ToolTip</param>
            /// <returns>The ToolTip Text</returns>
            public static System.String getToolTipText(System.Windows.Forms.Control control)
            {
                return (supportToolTip.GetToolTip(control));
            }

            /// <summary>
            /// Set the ToolTip text for the specific control parameter.
            /// </summary>
            /// <param name="control">The control to set the ToolTip</param>
            /// <param name="text">The text to show on the ToolTip</param>
            public static void setToolTipText(System.Windows.Forms.Control control, System.String text)
            {
                supportToolTip.SetToolTip(control, text);
            }
        }

        /*******************************/
        /// <summary>
        /// Action that will be executed when a toolbar button is clicked.
        /// </summary>
        /// <param name="event_sender">The object that fires the event.</param>
        /// <param name="event_args">An EventArgs that contains the event data.</param>
        public static void ToolBarButtonClicked(System.Object event_sender, System.Windows.Forms.ToolBarButtonClickEventArgs event_args)
        {
            System.Windows.Forms.Button button = (System.Windows.Forms.Button)event_args.Button.Tag;
            button.PerformClick();
        }


//        /*******************************/
//        /// <summary>
//        /// SupportClass for the HashSet class.
//        /// </summary>
//        [Serializable]
//        public class HashSetSupport : System.Collections.ArrayList, SetSupport
//        {
//            public HashSetSupport()
//                : base()
//            {
//            }
//
//            public HashSetSupport(System.Collections.ICollection c)
//            {
//                this.AddAll(c);
//            }
//
//            public HashSetSupport(int capacity)
//                : base(capacity)
//            {
//            }
//
//            /// <summary>
//            /// Adds a new element to the ArrayList if it is not already present.
//            /// </summary>		
//            /// <param name="obj">Element to insert to the ArrayList.</param>
//            /// <returns>Returns true if the new element was inserted, false otherwise.</returns>
//            new public virtual bool Add(System.Object obj)
//            {
//                bool inserted;
//
//                if ((inserted = this.Contains(obj)) == false)
//                {
//                    base.Add(obj);
//                }
//
//                return !inserted;
//            }
//
//            /// <summary>
//            /// Adds all the elements of the specified collection that are not present to the list.
//            /// </summary>
//            /// <param name="c">Collection where the new elements will be added</param>
//            /// <returns>Returns true if at least one element was added, false otherwise.</returns>
//            public bool AddAll(System.Collections.ICollection c)
//            {
//                System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
//                bool added = false;
//
//                while (e.MoveNext() == true)
//                {
//                    if (this.Add(e.Current) == true)
//                        added = true;
//                }
//
//                return added;
//            }
//
//            /// <summary>
//            /// Returns a copy of the HashSet instance.
//            /// </summary>		
//            /// <returns>Returns a shallow copy of the current HashSet.</returns>
//            public override System.Object Clone()
//            {
//                return base.MemberwiseClone();
//            }
//        }


//        /*******************************/
//        /// <summary>
//        /// Represents a collection ob objects that contains no duplicate elements.
//        /// </summary>	
//        public interface SetSupport : System.Collections.ICollection, System.Collections.IList
//        {
//            /// <summary>
//            /// Adds a new element to the Collection if it is not already present.
//            /// </summary>
//            /// <param name="obj">The object to add to the collection.</param>
//            /// <returns>Returns true if the object was added to the collection, otherwise false.</returns>
//            new bool Add(System.Object obj);
//
//            /// <summary>
//            /// Adds all the elements of the specified collection to the Set.
//            /// </summary>
//            /// <param name="c">Collection of objects to add.</param>
//            /// <returns>true</returns>
//            bool AddAll(System.Collections.ICollection c);
//        }


        /*******************************/
        /// <summary>
        /// Support for creation and modification of combo box elements
        /// </summary>
        public class ComboBoxSupport
        {
            /// <summary>
            /// Creates a new ComboBox control with the specified items.
            /// </summary>
            /// <param name="items">Items to insert into the combo box</param>
            /// <returns>A new combo box that contains the specified items</returns>
            public static System.Windows.Forms.ComboBox CreateComboBox(System.Object[] items)
            {
                System.Windows.Forms.ComboBox combo = new System.Windows.Forms.ComboBox();
                combo.Items.AddRange(items);
                combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                return combo;
            }

            /// <summary>
            /// Sets the items property of the specified combo with the items specified.
            /// </summary>
            /// <param name="combo">ComboBox to be modified</param>
            /// <param name="items">Items to insert into the combo box</param>
            public static void SetComboBox(System.Windows.Forms.ComboBox combo, System.Object[] items)
            {
                combo.Items.AddRange(items);
                combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            }

            /// <summary>
            /// Creates a new ComboBox control with the specified collection of items.
            /// </summary>
            /// <param name="items">Items to insert into the combo box</param>
            /// <returns>A new combo box that contains the specified items collection of items</returns>
            public static System.Windows.Forms.ComboBox CreateComboBox(System.Collections.ArrayList items)
            {
                return ComboBoxSupport.CreateComboBox(items.ToArray());
            }

            /// <summary>
            /// Sets the items property of the specified combo with the items collection specified.
            /// </summary>
            /// <param name="combo">ComboBox to be modified</param>
            /// <param name="items">Collection of items to insert into the combo box</param>
            public static void SetComboBox(System.Windows.Forms.ComboBox combo, System.Collections.ArrayList items)
            {
                ComboBoxSupport.SetComboBox(combo, items.ToArray());
            }

            /// <summary>
            /// Returns an array that contains the selected item of the specified combo box
            /// </summary>
            /// <param name="combo">The combo box from which the selected item is returned</param>
            /// <returns>An array that contains the selected item of the combo box</returns>
            public static System.Object[] GetSelectedObjects(System.Windows.Forms.ComboBox combo)
            {
                System.Object[] selectedObjects = new System.Object[1];
                selectedObjects[0] = combo.SelectedItem;
                return selectedObjects;
            }

            /// <summary>
            /// Returns a value indicating if the text portion of the specified combo box is editable
            /// </summary>
            /// <param name="combo">The combo box from to check</param>
            /// <returns>True if the text portion of the combo box is editable, false otherwise</returns>
            public static bool IsEditable(System.Windows.Forms.ComboBox combo)
            {
                return !(combo.DropDownStyle == System.Windows.Forms.ComboBoxStyle.DropDownList);
            }

            /// <summary>
            /// Create a TextBox object using the ComboBox object received as parameter.
            /// </summary>
            /// <param name="comboBox"></param>
            /// <returns></returns>
            public static System.Windows.Forms.TextBox GetEditComponent(System.Windows.Forms.ComboBox comboBox)
            {
                System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
                textBox.Text = comboBox.Text;
                return textBox;
            }
        }
        /*******************************/
        /// <summary>
        /// Implements number format functions
        /// </summary>
        [Serializable]
        public class TextNumberFormat
        {

            //Current localization number format infomation
            private System.Globalization.NumberFormatInfo numberFormat;
            //Enumeration of format types that can be used
            private enum formatTypes { General, Number, Currency, Percent };
            //Current format type used in the instance
            private int numberFormatType;
            //Indicates if grouping is being used
            private bool groupingActivated;
            //Current separator used
            private System.String separator;
            //Number of maximun digits in the integer portion of the number to represent the number
            private int maxIntDigits;
            //Number of minimum digits in the integer portion of the number to represent the number
            private int minIntDigits;
            //Number of maximun digits in the fraction portion of the number to represent the number
            private int maxFractionDigits;
            //Number of minimum digits in the integer portion of the number to represent the number
            private int minFractionDigits;

            /// <summary>
            /// Initializes a new instance of the object class with the default values
            /// </summary>
            public TextNumberFormat()
            {
                this.numberFormat = new System.Globalization.NumberFormatInfo();
                this.numberFormatType = (int)TextNumberFormat.formatTypes.General;
                this.groupingActivated = true;
                this.separator = this.GetSeparator((int)TextNumberFormat.formatTypes.General);
                this.maxIntDigits = 127;
                this.minIntDigits = 1;
                this.maxFractionDigits = 3;
                this.minFractionDigits = 0;
            }

            /// <summary>
            /// Sets the Maximum integer digits value. 
            /// </summary>
            /// <param name="newValue">the new value for the maxIntDigits field</param>
            public void setMaximumIntegerDigits(int newValue)
            {
                maxIntDigits = newValue;
                if (newValue <= 0)
                {
                    maxIntDigits = 0;
                    minIntDigits = 0;
                }
                else if (maxIntDigits < minIntDigits)
                {
                    minIntDigits = maxIntDigits;
                }
            }

            /// <summary>
            /// Sets the minimum integer digits value. 
            /// </summary>
            /// <param name="newValue">the new value for the minIntDigits field</param>
            public void setMinimumIntegerDigits(int newValue)
            {
                minIntDigits = newValue;
                if (newValue <= 0)
                {
                    minIntDigits = 0;
                }
                else if (maxIntDigits < minIntDigits)
                {
                    maxIntDigits = minIntDigits;
                }
            }

            /// <summary>
            /// Sets the maximum fraction digits value. 
            /// </summary>
            /// <param name="newValue">the new value for the maxFractionDigits field</param>
            public void setMaximumFractionDigits(int newValue)
            {
                maxFractionDigits = newValue;
                if (newValue <= 0)
                {
                    maxFractionDigits = 0;
                    minFractionDigits = 0;
                }
                else if (maxFractionDigits < minFractionDigits)
                {
                    minFractionDigits = maxFractionDigits;
                }
            }

            /// <summary>
            /// Sets the minimum fraction digits value. 
            /// </summary>
            /// <param name="newValue">the new value for the minFractionDigits field</param>
            public void setMinimumFractionDigits(int newValue)
            {
                minFractionDigits = newValue;
                if (newValue <= 0)
                {
                    minFractionDigits = 0;
                }
                else if (maxFractionDigits < minFractionDigits)
                {
                    maxFractionDigits = minFractionDigits;
                }
            }

            /// <summary>
            /// Initializes a new instance of the class with the specified number format
            /// and the amount of fractional digits to use
            /// </summary>
            /// <param name="theType">Number format</param>
            /// <param name="digits">Number of fractional digits to use</param>
            private TextNumberFormat(TextNumberFormat.formatTypes theType, int digits)
            {
                this.numberFormat = System.Globalization.NumberFormatInfo.CurrentInfo;
                this.numberFormatType = (int)theType;
                this.groupingActivated = true;
                this.separator = this.GetSeparator((int)theType);
                this.maxIntDigits = 127;
                this.minIntDigits = 1;
                this.maxFractionDigits = 3;
                this.minFractionDigits = 0;
            }

            /// <summary>
            /// Initializes a new instance of the class with the specified number format,
            /// uses the system's culture information,
            /// and assigns the amount of fractional digits to use
            /// </summary>
            /// <param name="theType">Number format</param>
            /// <param name="cultureNumberFormat">Represents information about a specific culture including the number formatting</param>
            /// <param name="digits">Number of fractional digits to use</param>
            private TextNumberFormat(TextNumberFormat.formatTypes theType, System.Globalization.CultureInfo cultureNumberFormat, int digits)
            {
                this.numberFormat = cultureNumberFormat.NumberFormat;
                this.numberFormatType = (int)theType;
                this.groupingActivated = true;
                this.separator = this.GetSeparator((int)theType);
                this.maxIntDigits = 127;
                this.minIntDigits = 1;
                this.maxFractionDigits = 3;
                this.minFractionDigits = 0;
            }

            /// <summary>
            /// Returns an initialized instance of the TextNumberFormat object
            /// using number representation.
            /// </summary>
            /// <returns>The object instance</returns>
            public static TextNumberFormat getTextNumberInstance()
            {
                TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Number, 3);
                return instance;
            }

            /// <summary>
            /// Returns an initialized instance of the TextNumberFormat object
            /// using currency representation.
            /// </summary>
            /// <returns>The object instance</returns>
            public static TextNumberFormat getTextNumberCurrencyInstance()
            {
                TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Currency, 3);
                return instance.setToCurrencyNumberFormatDefaults(instance);
            }

            /// <summary>
            /// Returns an initialized instance of the TextNumberFormat object
            /// using percent representation.
            /// </summary>
            /// <returns>The object instance</returns>
            public static TextNumberFormat getTextNumberPercentInstance()
            {
                TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Percent, 3);
                return instance.setToPercentNumberFormatDefaults(instance);
            }

            /// <summary>
            /// Returns an initialized instance of the TextNumberFormat object
            /// using number representation, it uses the culture format information provided.
            /// </summary>
            /// <param name="culture">Represents information about a specific culture</param>
            /// <returns>The object instance</returns>
            public static TextNumberFormat getTextNumberInstance(System.Globalization.CultureInfo culture)
            {
                TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Number, culture, 3);
                return instance;
            }

            /// <summary>
            /// Returns an initialized instance of the TextNumberFormat object
            /// using currency representation, it uses the culture format information provided.
            /// </summary>
            /// <param name="culture">Represents information about a specific culture</param>
            /// <returns>The object instance</returns>
            public static TextNumberFormat getTextNumberCurrencyInstance(System.Globalization.CultureInfo culture)
            {
                TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Currency, culture, 3);
                return instance.setToCurrencyNumberFormatDefaults(instance);
            }

            /// <summary>
            /// Returns an initialized instance of the TextNumberFormat object
            /// using percent representation, it uses the culture format information provided.
            /// </summary>
            /// <param name="culture">Represents information about a specific culture</param>
            /// <returns>The object instance</returns>
            public static TextNumberFormat getTextNumberPercentInstance(System.Globalization.CultureInfo culture)
            {
                TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Percent, culture, 3);
                return instance.setToPercentNumberFormatDefaults(instance);
            }

            /// <summary>
            /// Clones the object instance
            /// </summary>
            /// <returns>The cloned object instance</returns>
            public System.Object Clone()
            {
                return (System.Object)this;
            }

            /// <summary>
            /// Determines if the received object is equal to the
            /// current object instance
            /// </summary>
            /// <param name="textNumberObject">TextNumber instance to compare</param>
            /// <returns>True or false depending if the two instances are equal</returns>
            public override bool Equals(Object obj)
            {
                // Check for null values and compare run-time types.
                if (obj == null || GetType() != obj.GetType())
                    return false;
                SupportClass.TextNumberFormat param = (SupportClass.TextNumberFormat)obj;
                return (numberFormat == param.numberFormat) && (numberFormatType == param.numberFormatType)
                    && (groupingActivated == param.groupingActivated) && (separator == param.separator)
                    && (maxIntDigits == param.maxIntDigits) && (minIntDigits == param.minIntDigits)
                    && (maxFractionDigits == param.maxFractionDigits) && (minFractionDigits == param.minFractionDigits);
            }


            /// <summary>
            /// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
            /// </summary>
            /// <returns>A hash code for the current Object</returns>
            public override int GetHashCode()
            {
                return numberFormat.GetHashCode() ^ numberFormatType ^ groupingActivated.GetHashCode()
                     ^ separator.GetHashCode() ^ maxIntDigits ^ minIntDigits ^ maxFractionDigits ^ minFractionDigits;
            }

            /// <summary>
            /// Formats a number with the current formatting parameters
            /// </summary>
            /// <param name="number">Source number to format</param>
            /// <returns>The formatted number string</returns>
            public System.String FormatDouble(double number)
            {
                if (this.groupingActivated)
                {
                    return SetIntDigits(number.ToString(this.GetCurrentFormatString() + this.GetNumberOfDigits(number), this.numberFormat));
                }
                else
                {
                    return SetIntDigits((number.ToString(this.GetCurrentFormatString() + this.GetNumberOfDigits(number), this.numberFormat)).Replace(this.separator, ""));
                }
            }

            /// <summary>
            /// Formats a number with the current formatting parameters
            /// </summary>
            /// <param name="number">Source number to format</param>
            /// <returns>The formatted number string</returns>
            public System.String FormatLong(long number)
            {
                if (this.groupingActivated)
                {
                    return SetIntDigits(number.ToString(this.GetCurrentFormatString() + this.minFractionDigits, this.numberFormat));
                }
                else
                {
                    return SetIntDigits((number.ToString(this.GetCurrentFormatString() + this.minFractionDigits, this.numberFormat)).Replace(this.separator, ""));
                }
            }


            /// <summary>
            /// Formats the number according to the specified number of integer digits 
            /// </summary>
            /// <param name="number">The number to format</param>
            /// <returns></returns>
            private System.String SetIntDigits(String number)
            {
                String decimals = "";
                String fraction = "";
                int i = number.IndexOf(this.numberFormat.NumberDecimalSeparator);
                if (i > 0)
                {
                    fraction = number.Substring(i);
                    decimals = number.Substring(0, i).Replace(this.numberFormat.NumberGroupSeparator, "");
                }
                else decimals = number.Replace(this.numberFormat.NumberGroupSeparator, "");
                decimals = decimals.PadLeft(this.MinIntDigits, '0');
                if ((i = decimals.Length - this.MaxIntDigits) > 0) decimals = decimals.Remove(0, i);
                if (this.groupingActivated)
                {
                    for (i = decimals.Length; i > 3; i -= 3)
                    {
                        decimals = decimals.Insert(i - 3, this.numberFormat.NumberGroupSeparator);
                    }
                }
                decimals = decimals + fraction;
                if (decimals.Length == 0) return "0";
                else return decimals;
            }

            /// <summary>
            /// Gets the list of all supported cultures
            /// </summary>
            /// <returns>An array of type CultureInfo that represents the supported cultures</returns>
            public static System.Globalization.CultureInfo[] GetAvailableCultures()
            {
                return System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures);
            }

            /// <summary>
            /// Obtains the current format representation used
            /// </summary>
            /// <returns>A character representing the string format used</returns>
            private System.String GetCurrentFormatString()
            {
                System.String currentFormatString = "n";  //Default value
                switch (this.numberFormatType)
                {
                    case (int)TextNumberFormat.formatTypes.Currency:
                        currentFormatString = "c";
                        break;

                    case (int)TextNumberFormat.formatTypes.General:
                        currentFormatString = "n";
                        break;

                    case (int)TextNumberFormat.formatTypes.Number:
                        currentFormatString = "n";
                        break;

                    case (int)TextNumberFormat.formatTypes.Percent:
                        currentFormatString = "p";
                        break;
                }
                return currentFormatString;
            }

            /// <summary>
            /// Retrieves the separator used, depending on the format type specified
            /// </summary>
            /// <param name="numberFormatType">formatType enumarator value to inquire</param>
            /// <returns>The values of character separator used </returns>
            private System.String GetSeparator(int numberFormatType)
            {
                System.String separatorItem = " ";  //Default Separator

                switch (numberFormatType)
                {
                    case (int)TextNumberFormat.formatTypes.Currency:
                        separatorItem = this.numberFormat.CurrencyGroupSeparator;
                        break;

                    case (int)TextNumberFormat.formatTypes.General:
                        separatorItem = this.numberFormat.NumberGroupSeparator;
                        break;

                    case (int)TextNumberFormat.formatTypes.Number:
                        separatorItem = this.numberFormat.NumberGroupSeparator;
                        break;

                    case (int)TextNumberFormat.formatTypes.Percent:
                        separatorItem = this.numberFormat.PercentGroupSeparator;
                        break;
                }
                return separatorItem;
            }

            /// <summary>
            /// Boolean value stating if grouping is used or not
            /// </summary>
            public bool GroupingUsed
            {
                get
                {
                    return (this.groupingActivated);
                }
                set
                {
                    this.groupingActivated = value;
                }
            }

            /// <summary>
            /// Minimum number of integer digits to use in the number format
            /// </summary>
            public int MinIntDigits
            {
                get
                {
                    return this.minIntDigits;
                }
                set
                {
                    this.minIntDigits = value;
                }
            }

            /// <summary>
            /// Maximum number of integer digits to use in the number format
            /// </summary>
            public int MaxIntDigits
            {
                get
                {
                    return this.maxIntDigits;
                }
                set
                {
                    this.maxIntDigits = value;
                }
            }

            /// <summary>
            /// Minimum number of fraction digits to use in the number format
            /// </summary>
            public int MinFractionDigits
            {
                get
                {
                    return this.minFractionDigits;
                }
                set
                {
                    this.minFractionDigits = value;
                }
            }

            /// <summary>
            /// Maximum number of fraction digits to use in the number format
            /// </summary>
            public int MaxFractionDigits
            {
                get
                {
                    return this.maxFractionDigits;
                }
                set
                {
                    this.maxFractionDigits = value;
                }
            }

            /// <summary>
            /// Sets the values of minFractionDigits and maxFractionDigits to the currency standard
            /// </summary>
            /// <param name="format">The TextNumberFormat instance to set</param>
            /// <returns>The TextNumberFormat with corresponding the default values</returns>
            private TextNumberFormat setToCurrencyNumberFormatDefaults(TextNumberFormat format)
            {
                format.maxFractionDigits = 2;
                format.minFractionDigits = 2;
                return format;
            }

            /// <summary>
            /// Sets the values of minFractionDigits and maxFractionDigits to the percent standard
            /// </summary>
            /// <param name="format">The TextNumberFormat instance to set</param>
            /// <returns>The TextNumberFormat with corresponding the default values</returns>
            private TextNumberFormat setToPercentNumberFormatDefaults(TextNumberFormat format)
            {
                format.maxFractionDigits = 0;
                format.minFractionDigits = 0;
                return format;
            }

            /// <summary>
            /// Gets the number of fraction digits thats must be used by the format methods
            /// </summary>
            /// <param name="number">The double number</param>
            /// <returns>The number of fraction digits to use</returns>
            private int GetNumberOfDigits(Double number)
            {
                int counter = 0;
                double temp = System.Math.Abs(number);
                while ((temp % 1) > 0)
                {
                    temp *= 10;
                    counter++;
                }
                return (counter < this.minFractionDigits) ? this.minFractionDigits : ((counter < this.maxFractionDigits) ? counter : this.maxFractionDigits);
            }
        }
        /*******************************/
        /// <summary>
        /// Provides overloaded methods to create and set values to an instance of System.Drawing.Pen.
        /// </summary>
        public class StrokeConsSupport
        {
            /// <summary>
            /// Creates an instance of System.Drawing.Pen with the default SolidBrush black.
            /// And then set the parameters into their corresponding properties.
            /// </summary>
            /// <param name="width">The width of the stroked line.</param>
            /// <param name="cap">The DashCap end of line style.</param>
            /// <param name="join">The LineJoin style.</param>
            /// <returns>A new instance with the values set.</returns>
            public static System.Drawing.Pen CreatePenInstance(float width, int cap, int join)
            {
                System.Drawing.Pen tempPen = new System.Drawing.Pen(System.Drawing.Brushes.Black, width);
                tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.EndCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
                return tempPen;
            }

            /// <summary>
            /// Creates an instance of System.Drawing.Pen with the default SolidBrush black.
            /// And then set the parameters into their corresponding properties.
            /// </summary>
            /// <param name="width">The width of the stroked line.</param>
            /// <param name="cap">The DashCap end of line style.</param>
            /// <param name="join">The LineJoin style.</param>
            /// <param name="miterlimit">The limit of the line.</param>
            /// <returns>A new instance with the values set.</returns>
            public static System.Drawing.Pen CreatePenInstance(float width, int cap, int join, float miterlimit)
            {
                System.Drawing.Pen tempPen = new System.Drawing.Pen(System.Drawing.Brushes.Black, width);
                tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.EndCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
                tempPen.MiterLimit = miterlimit;
                return tempPen;
            }

            /// <summary>
            /// Creates an instance of System.Drawing.Pen with the default SolidBrush black.
            /// And then set the parameters into their corresponding properties.
            /// </summary>
            /// <param name="width">The width of the stroked line.</param>
            /// <param name="cap">The DashCap end of line style.</param>
            /// <param name="join">The LineJoin style.</param>
            /// <param name="miterlimit">The limit of the line.</param>
            /// <param name="dashPattern">The array to use to make the dash.</param>
            /// <param name="dashOffset">The space between each dash.</param>
            /// <returns>A new instance with the values set.</returns>
            public static System.Drawing.Pen CreatePenInstance(float width, int cap, int join, float miterlimit, float[] dashPattern, float dashOffset)
            {
                System.Drawing.Pen tempPen = new System.Drawing.Pen(System.Drawing.Brushes.Black, width);
                tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.EndCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
                tempPen.MiterLimit = miterlimit;
                tempPen.DashPattern = dashPattern;
                tempPen.DashOffset = dashOffset;
                return tempPen;
            }

            /// <summary>
            /// Sets a Pen instance with the corresponding DashCap and LineJoin values.
            /// </summary>
            /// <param name="cap">The DashCap end of line style.</param>
            /// <param name="join">The LineJoin style.</param>
            /// <returns>A new instance with the values set.</returns>
            public static void SetPen(System.Drawing.Pen tempPen, int cap, int join)
            {
                tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.EndCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
            }

            /// <summary>
            /// Sets a Pen instance with the corresponding DashCap, LineJoin and MiterLimit values.
            /// </summary>
            /// <param name="cap">The DashCap end of line style.</param>
            /// <param name="join">The LineJoin style.</param>
            /// <param name="miterlimit">The limit of the line.</param>
            public static void SetPen(System.Drawing.Pen tempPen, int cap, int join, float miterlimit)
            {
                tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.EndCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
                tempPen.MiterLimit = miterlimit;
            }

            /// <summary>
            /// Sets a Pen instance with the corresponding DashCap, LineJoin, MiterLimit, DashPattern and 
            /// DashOffset values.
            /// </summary>
            /// <param name="cap">The DashCap end of line style.</param>
            /// <param name="join">The LineJoin style.</param>
            /// <param name="miterlimit">The limit of the line.</param>
            /// <param name="dashPattern">The array to use to make the dash.</param>
            /// <param name="dashOffset">The space between each dash.</param>
            public static void SetPen(System.Drawing.Pen tempPen, float width, int cap, int join, float miterlimit, float[] dashPattern, float dashOffset)
            {
                tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.EndCap = (System.Drawing.Drawing2D.LineCap)cap;
                tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
                tempPen.MiterLimit = miterlimit;
                tempPen.DashPattern = dashPattern;
                tempPen.DashOffset = dashOffset;
            }
        }

        /*******************************/
        public delegate void PropertyChangeEventHandler(System.Object sender, PropertyChangingEventArgs e);

        /// <summary>
        /// EventArgs for support to the contrained properties.
        /// </summary>
        public class PropertyChangingEventArgs : System.ComponentModel.PropertyChangedEventArgs
        {
            private System.Object oldValue;
            private System.Object newValue;

            /// <summary>
            /// Initializes a new PropertyChangingEventArgs instance.
            /// </summary>
            /// <param name="propertyName">Property name that fire the event.</param>
            public PropertyChangingEventArgs(System.String propertyName)
                : base(propertyName)
            {
            }

            /// <summary>
            /// Initializes a new PropertyChangingEventArgs instance.
            /// </summary>
            /// <param name="propertyName">Property name that fire the event.</param>
            /// <param name="oldVal">Property value to be replaced.</param>
            /// <param name="newVal">Property value to be set.</param>
            public PropertyChangingEventArgs(System.String propertyName, System.Object oldVal, System.Object newVal)
                : base(propertyName)
            {
                this.oldValue = oldVal;
                this.newValue = newVal;
            }

            /// <summary>
            /// Gets or sets the old value of the event.
            /// </summary>
            public System.Object OldValue
            {
                get
                {
                    return this.oldValue;
                }
                set
                {
                    this.oldValue = value;
                }
            }

            /// <summary>
            /// Gets or sets the new value of the event.
            /// </summary>
            public System.Object NewValue
            {
                get
                {
                    return this.newValue;
                }
                set
                {
                    this.newValue = value;
                }
            }
        }


        /*******************************/
        /// <summary>
        /// Provides functionality not found in .NET map-related interfaces.
        /// </summary>
        public class MapSupport
        {
            /// <summary>
            /// Determines whether the SortedList contains a specific value.
            /// </summary>
            /// <param name="d">The dictionary to check for the value.</param>
            /// <param name="obj">The object to locate in the SortedList.</param>
            /// <returns>Returns true if the value is contained in the SortedList, false otherwise.</returns>
            public static bool ContainsValue(System.Collections.IDictionary d, System.Object obj)
            {
                bool contained = false;
                System.Type type = d.GetType();

                //Classes that implement the SortedList class
                if (type == System.Type.GetType("System.Collections.SortedList"))
                {
                    contained = (bool)((System.Collections.SortedList)d).ContainsValue(obj);
                }
                //Classes that implement the Hashtable class
                else if (type == System.Type.GetType("System.Collections.Hashtable"))
                {
                    contained = (bool)((System.Collections.Hashtable)d).ContainsValue(obj);
                }
                else
                {
                    //Reflection. Invoke "containsValue" method for proprietary classes
                    try
                    {
                        System.Reflection.MethodInfo method = type.GetMethod("containsValue");
                        contained = (bool)method.Invoke(d, new Object[] { obj });
                    }
                    catch (System.Reflection.TargetInvocationException e)
                    {
                        throw e;
                    }
                    catch (System.Exception e)
                    {
                        throw e;
                    }
                }

                return contained;
            }


            /// <summary>
            /// Determines whether the NameValueCollection contains a specific value.
            /// </summary>
            /// <param name="d">The dictionary to check for the value.</param>
            /// <param name="obj">The object to locate in the SortedList.</param>
            /// <returns>Returns true if the value is contained in the NameValueCollection, false otherwise.</returns>
            public static bool ContainsValue(System.Collections.Specialized.NameValueCollection d, System.Object obj)
            {
                bool contained = false;
                System.Type type = d.GetType();

                for (int i = 0; i < d.Count && !contained; i++)
                {
                    System.String[] values = d.GetValues(i);
                    if (values != null)
                    {
                        foreach (System.String val in values)
                        {
                            if (val.Equals(obj))
                            {
                                contained = true;
                                break;
                            }
                        }
                    }
                }
                return contained;
            }

            /// <summary>
            /// Copies all the elements of d to target.
            /// </summary>
            /// <param name="target">Collection where d elements will be copied.</param>
            /// <param name="d">Elements to copy to the target collection.</param>
            public static void PutAll(System.Collections.IDictionary target, System.Collections.IDictionary d)
            {
                if (d != null)
                {
                    System.Collections.ArrayList keys = new System.Collections.ArrayList(d.Keys);
                    System.Collections.ArrayList values = new System.Collections.ArrayList(d.Values);

                    for (int i = 0; i < keys.Count; i++)
                        target[keys[i]] = values[i];
                }
            }

            /// <summary>
            /// Returns a portion of the list whose keys are less than the limit object parameter.
            /// </summary>
            /// <param name="l">The list where the portion will be extracted.</param>
            /// <param name="limit">The end element of the portion to extract.</param>
            /// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
            public static System.Collections.SortedList HeadMap(System.Collections.SortedList l, System.Object limit)
            {
                System.Collections.Comparer comparer = System.Collections.Comparer.Default;
                System.Collections.SortedList newList = new System.Collections.SortedList();

                for (int i = 0; i < l.Count; i++)
                {
                    if (comparer.Compare(l.GetKey(i), limit) >= 0)
                        break;

                    newList.Add(l.GetKey(i), l[l.GetKey(i)]);
                }

                return newList;
            }

            /// <summary>
            /// Returns a portion of the list whose keys are greater that the lowerLimit parameter less than the upperLimit parameter.
            /// </summary>
            /// <param name="l">The list where the portion will be extracted.</param>
            /// <param name="limit">The start element of the portion to extract.</param>
            /// <param name="limit">The end element of the portion to extract.</param>
            /// <returns>The portion of the collection.</returns>
            public static System.Collections.SortedList SubMap(System.Collections.SortedList list, System.Object lowerLimit, System.Object upperLimit)
            {
                System.Collections.Comparer comparer = System.Collections.Comparer.Default;
                System.Collections.SortedList newList = new System.Collections.SortedList();

                if (list != null)
                {
                    if ((list.Count > 0) && (!(lowerLimit.Equals(upperLimit))))
                    {
                        int index = 0;
                        while (comparer.Compare(list.GetKey(index), lowerLimit) < 0)
                            index++;

                        for (; index < list.Count; index++)
                        {
                            if (comparer.Compare(list.GetKey(index), upperLimit) >= 0)
                                break;

                            newList.Add(list.GetKey(index), list[list.GetKey(index)]);
                        }
                    }
                }

                return newList;
            }

            /// <summary>
            /// Returns a portion of the list whose keys are greater than the limit object parameter.
            /// </summary>
            /// <param name="l">The list where the portion will be extracted.</param>
            /// <param name="limit">The start element of the portion to extract.</param>
            /// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
            public static System.Collections.SortedList TailMap(System.Collections.SortedList list, System.Object limit)
            {
                System.Collections.Comparer comparer = System.Collections.Comparer.Default;
                System.Collections.SortedList newList = new System.Collections.SortedList();

                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        int index = 0;
                        while (comparer.Compare(list.GetKey(index), limit) < 0)
                            index++;

                        for (; index < list.Count; index++)
                            newList.Add(list.GetKey(index), list[list.GetKey(index)]);
                    }
                }

                return newList;
            }
        }


        /*******************************/
        /// <summary>
        /// This class provides functionality not found in .NET collection-related interfaces.
        /// </summary>
        public class ICollectionSupport
        {
            /// <summary>
            /// Adds a new element to the specified collection.
            /// </summary>
            /// <param name="c">Collection where the new element will be added.</param>
            /// <param name="obj">Object to add.</param>
            /// <returns>true</returns>
            public static bool Add(System.Collections.ICollection c, System.Object obj)
            {
                bool added = false;
                //Reflection. Invoke either the "add" or "Add" method.
                System.Reflection.MethodInfo method;
                try
                {
                    //Get the "add" method for proprietary classes
                    method = c.GetType().GetMethod("Add");
                    if (method == null)
                        method = c.GetType().GetMethod("add");
                    int index = (int)method.Invoke(c, new System.Object[] { obj });
                    if (index >= 0)
                        added = true;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                return added;
            }

            /// <summary>
            /// Adds all of the elements of the "c" collection to the "target" collection.
            /// </summary>
            /// <param name="target">Collection where the new elements will be added.</param>
            /// <param name="c">Collection whose elements will be added.</param>
            /// <returns>Returns true if at least one element was added, false otherwise.</returns>
            public static bool AddAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
                bool added = false;

                //Reflection. Invoke "addAll" method for proprietary classes
                System.Reflection.MethodInfo method;
                try
                {
                    method = target.GetType().GetMethod("addAll");

                    if (method != null)
                        added = (bool)method.Invoke(target, new System.Object[] { c });
                    else
                    {
                        method = target.GetType().GetMethod("Add");
                        while (e.MoveNext() == true)
                        {
                            bool tempBAdded = (int)method.Invoke(target, new System.Object[] { e.Current }) >= 0;
                            added = added ? added : tempBAdded;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                return added;
            }

            /// <summary>
            /// Removes all the elements from the collection.
            /// </summary>
            /// <param name="c">The collection to remove elements.</param>
            public static void Clear(System.Collections.ICollection c)
            {
                //Reflection. Invoke "Clear" method or "clear" method for proprietary classes
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("Clear");

                    if (method == null)
                        method = c.GetType().GetMethod("clear");

                    method.Invoke(c, new System.Object[] { });
                }
                catch (System.Exception e)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Determines whether the collection contains the specified element.
            /// </summary>
            /// <param name="c">The collection to check.</param>
            /// <param name="obj">The object to locate in the collection.</param>
            /// <returns>true if the element is in the collection.</returns>
            public static bool Contains(System.Collections.ICollection c, System.Object obj)
            {
                bool contains = false;

                //Reflection. Invoke "contains" method for proprietary classes
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("Contains");

                    if (method == null)
                        method = c.GetType().GetMethod("contains");

                    contains = (bool)method.Invoke(c, new System.Object[] { obj });
                }
                catch (System.Exception e)
                {
                    throw e;
                }

                return contains;
            }

            /// <summary>
            /// Determines whether the collection contains all the elements in the specified collection.
            /// </summary>
            /// <param name="target">The collection to check.</param>
            /// <param name="c">Collection whose elements would be checked for containment.</param>
            /// <returns>true id the target collection contains all the elements of the specified collection.</returns>
            public static bool ContainsAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = c.GetEnumerator();

                bool contains = false;

                //Reflection. Invoke "containsAll" method for proprietary classes or "Contains" method for each element in the collection
                System.Reflection.MethodInfo method;
                try
                {
                    method = target.GetType().GetMethod("containsAll");

                    if (method != null)
                        contains = (bool)method.Invoke(target, new Object[] { c });
                    else
                    {
                        method = target.GetType().GetMethod("Contains");
                        while (e.MoveNext() == true)
                        {
                            if ((contains = (bool)method.Invoke(target, new Object[] { e.Current })) == false)
                                break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                return contains;
            }

            /// <summary>
            /// Removes the specified element from the collection.
            /// </summary>
            /// <param name="c">The collection where the element will be removed.</param>
            /// <param name="obj">The element to remove from the collection.</param>
            public static bool Remove(System.Collections.ICollection c, System.Object obj)
            {
                bool changed = false;

                //Reflection. Invoke "remove" method for proprietary classes or "Remove" method
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("remove");

                    if (method != null)
                        method.Invoke(c, new System.Object[] { obj });
                    else
                    {
                        method = c.GetType().GetMethod("Contains");
                        changed = (bool)method.Invoke(c, new System.Object[] { obj });
                        method = c.GetType().GetMethod("Remove");
                        method.Invoke(c, new System.Object[] { obj });
                    }
                }
                catch (System.Exception e)
                {
                    throw e;
                }

                return changed;
            }

            /// <summary>
            /// Removes all the elements from the specified collection that are contained in the target collection.
            /// </summary>
            /// <param name="target">Collection where the elements will be removed.</param>
            /// <param name="c">Elements to remove from the target collection.</param>
            /// <returns>true</returns>
            public static bool RemoveAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.ArrayList al = ToArrayList(c);
                System.Collections.IEnumerator e = al.GetEnumerator();

                //Reflection. Invoke "removeAll" method for proprietary classes or "Remove" for each element in the collection
                System.Reflection.MethodInfo method;
                try
                {
                    method = target.GetType().GetMethod("removeAll");

                    if (method != null)
                        method.Invoke(target, new System.Object[] { al });
                    else
                    {
                        method = target.GetType().GetMethod("Remove");
                        System.Reflection.MethodInfo methodContains = target.GetType().GetMethod("Contains");

                        while (e.MoveNext() == true)
                        {
                            while ((bool)methodContains.Invoke(target, new System.Object[] { e.Current }) == true)
                                method.Invoke(target, new System.Object[] { e.Current });
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                return true;
            }

            /// <summary>
            /// Retains the elements in the target collection that are contained in the specified collection
            /// </summary>
            /// <param name="target">Collection where the elements will be removed.</param>
            /// <param name="c">Elements to be retained in the target collection.</param>
            /// <returns>true</returns>
            public static bool RetainAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = new System.Collections.ArrayList(target).GetEnumerator();
                System.Collections.ArrayList al = new System.Collections.ArrayList(c);

                //Reflection. Invoke "retainAll" method for proprietary classes or "Remove" for each element in the collection
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("retainAll");

                    if (method != null)
                        method.Invoke(target, new System.Object[] { c });
                    else
                    {
                        method = c.GetType().GetMethod("Remove");

                        while (e.MoveNext() == true)
                        {
                            if (al.Contains(e.Current) == false)
                                method.Invoke(target, new System.Object[] { e.Current });
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                return true;
            }

            /// <summary>
            /// Returns an array containing all the elements of the collection.
            /// </summary>
            /// <returns>The array containing all the elements of the collection.</returns>
            public static System.Object[] ToArray(System.Collections.ICollection c)
            {
                int index = 0;
                System.Object[] objects = new System.Object[c.Count];
                System.Collections.IEnumerator e = c.GetEnumerator();

                while (e.MoveNext())
                    objects[index++] = e.Current;

                return objects;
            }

            /// <summary>
            /// Obtains an array containing all the elements of the collection.
            /// </summary>
            /// <param name="objects">The array into which the elements of the collection will be stored.</param>
            /// <returns>The array containing all the elements of the collection.</returns>
            public static System.Object[] ToArray(System.Collections.ICollection c, System.Object[] objects)
            {
                int index = 0;

                System.Type type = objects.GetType().GetElementType();
                System.Object[] objs = (System.Object[])Array.CreateInstance(type, c.Count);

                System.Collections.IEnumerator e = c.GetEnumerator();

                while (e.MoveNext())
                    objs[index++] = e.Current;

                //If objects is smaller than c then do not return the new array in the parameter
                if (objects.Length >= c.Count)
                    objs.CopyTo(objects, 0);

                return objs;
            }

            /// <summary>
            /// Converts an ICollection instance to an ArrayList instance.
            /// </summary>
            /// <param name="c">The ICollection instance to be converted.</param>
            /// <returns>An ArrayList instance in which its elements are the elements of the ICollection instance.</returns>
            public static System.Collections.ArrayList ToArrayList(System.Collections.ICollection c)
            {
                System.Collections.ArrayList tempArrayList = new System.Collections.ArrayList();
                System.Collections.IEnumerator tempEnumerator = c.GetEnumerator();
                while (tempEnumerator.MoveNext())
                    tempArrayList.Add(tempEnumerator.Current);
                return tempArrayList;
            }
        }


        /*******************************/
        /// <summary>
        /// Adds the X and Y coordinates to the current graphics path.
        /// </summary>
        /// <param name="graphPath"> The current Graphics path</param>
        /// <param name="xCoordinate">The x coordinate to be added</param>
        /// <param name="yCoordinate">The y coordinate to be added</param>
        public static void AddPointToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath graphPath, int x, int y)
        {
            System.Drawing.PointF[] tempPointArray = new System.Drawing.PointF[graphPath.PointCount + 1];
            byte[] tempPointTypeArray = new byte[graphPath.PointCount + 1];

            if (graphPath.PointCount == 0)
            {
                tempPointArray[0] = new System.Drawing.PointF(x, y);
                System.Drawing.Drawing2D.GraphicsPath tempGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath(tempPointArray, new byte[] { (byte)System.Drawing.Drawing2D.PathPointType.Start });
                graphPath.AddPath(tempGraphicsPath, false);
            }
            else
            {
                graphPath.PathPoints.CopyTo(tempPointArray, 0);
                tempPointArray[graphPath.PointCount] = new System.Drawing.Point(x, y);

                graphPath.PathTypes.CopyTo(tempPointTypeArray, 0);
                tempPointTypeArray[graphPath.PointCount] = (byte)System.Drawing.Drawing2D.PathPointType.Line;

                System.Drawing.Drawing2D.GraphicsPath tempGraphics = new System.Drawing.Drawing2D.GraphicsPath(tempPointArray, tempPointTypeArray);
                graphPath.Reset();
                graphPath.AddPath(tempGraphics, false);
                graphPath.CloseFigure();
            }
        }
        /*******************************/
        /// <summary>
        /// Creates a GraphicsPath from two Int Arrays with a specific number of points.
        /// </summary>
        /// <param name="xPoints">Int Array to set the X points of the GraphicsPath</param>
        /// <param name="yPoints">Int Array to set the Y points of the GraphicsPath</param>
        /// <param name="pointsNumber">Number of points to add to the GraphicsPath</param>
        /// <returns>A new GraphicsPath</returns>
        public static System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(int[] xPoints, int[] yPoints, int pointsNumber)
        {
            System.Drawing.Drawing2D.GraphicsPath tempGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
            if (pointsNumber == 2)
                tempGraphicsPath.AddLine(xPoints[0], yPoints[0], xPoints[1], yPoints[1]);
            else
            {
                System.Drawing.Point[] tempPointArray = new System.Drawing.Point[pointsNumber];
                for (int index = 0; index < pointsNumber; index++)
                    tempPointArray[index] = new System.Drawing.Point(xPoints[index], yPoints[index]);

                tempGraphicsPath.AddPolygon(tempPointArray);
            }
            return tempGraphicsPath;
        }

        /*******************************/
        public class TransactionManager
        {
            public static ConnectionHashTable manager = new ConnectionHashTable();

            public class ConnectionHashTable : System.Collections.Hashtable
            {
                public System.Data.OleDb.OleDbCommand CreateStatement(System.Data.OleDb.OleDbConnection connection)
                {
                    System.Data.OleDb.OleDbCommand command = connection.CreateCommand();
                    System.Data.OleDb.OleDbTransaction transaction;
                    if (this[connection] != null)
                    {
                        ConnectionProperties Properties = ((ConnectionProperties)this[connection]);
                        transaction = Properties.Transaction;
                        command.Transaction = transaction;
                        command.CommandTimeout = 0;
                    }
                    else
                    {
                        ConnectionProperties TempProp = new ConnectionProperties();
                        TempProp.AutoCommit = true;
                        TempProp.TransactionLevel = 0;
                        command.Transaction = TempProp.Transaction;
                        command.CommandTimeout = 0;
                        Add(connection, TempProp);
                    }
                    return command;
                }

                public void Commit(System.Data.OleDb.OleDbConnection connection)
                {
                    if (this[connection] != null && !((ConnectionProperties)this[connection]).AutoCommit)
                    {
                        ConnectionProperties Properties = ((ConnectionProperties)this[connection]);
                        System.Data.OleDb.OleDbTransaction transaction = Properties.Transaction;
                        transaction.Commit();
                        if (Properties.TransactionLevel == 0)
                            Properties.Transaction = connection.BeginTransaction();
                        else
                            Properties.Transaction = connection.BeginTransaction(Properties.TransactionLevel);
                    }
                }

                public void RollBack(System.Data.OleDb.OleDbConnection connection)
                {
                    if (this[connection] != null && !((ConnectionProperties)this[connection]).AutoCommit)
                    {
                        ConnectionProperties Properties = ((ConnectionProperties)this[connection]);
                        System.Data.OleDb.OleDbTransaction transaction = Properties.Transaction;
                        transaction.Rollback();
                        if (Properties.TransactionLevel == 0)
                            Properties.Transaction = connection.BeginTransaction();
                        else
                            Properties.Transaction = connection.BeginTransaction(Properties.TransactionLevel);
                    }
                }

                public void SetAutoCommit(System.Data.OleDb.OleDbConnection connection, bool boolean)
                {
                    if (this[connection] != null)
                    {
                        ConnectionProperties Properties = ((ConnectionProperties)this[connection]);
                        if (Properties.AutoCommit != boolean)
                        {
                            Properties.AutoCommit = boolean;
                            if (!boolean)
                            {
                                if (Properties.TransactionLevel == 0)
                                    Properties.Transaction = connection.BeginTransaction();
                                else
                                    Properties.Transaction = connection.BeginTransaction(Properties.TransactionLevel);
                            }
                            else
                            {
                                System.Data.OleDb.OleDbTransaction transaction = Properties.Transaction;
                                if (transaction != null)
                                {
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    else
                    {
                        ConnectionProperties TempProp = new ConnectionProperties();
                        TempProp.AutoCommit = boolean;
                        TempProp.TransactionLevel = 0;
                        if (!boolean)
                            TempProp.Transaction = connection.BeginTransaction();
                        Add(connection, TempProp);
                    }
                }

                public System.Data.OleDb.OleDbCommand PrepareStatement(System.Data.OleDb.OleDbConnection connection, System.String sql)
                {
                    System.Data.OleDb.OleDbCommand command = this.CreateStatement(connection);
                    command.CommandText = sql;
                    command.CommandTimeout = 0;
                    return command;
                }

                public System.Data.OleDb.OleDbCommand PrepareCall(System.Data.OleDb.OleDbConnection connection, System.String sql)
                {
                    System.Data.OleDb.OleDbCommand command = this.CreateStatement(connection);
                    command.CommandText = sql;
                    command.CommandTimeout = 0;
                    return command;
                }

                public void SetTransactionIsolation(System.Data.OleDb.OleDbConnection connection, int level)
                {
                    ConnectionProperties Properties;
                    if (level == (int)System.Data.IsolationLevel.ReadCommitted)
                        SetAutoCommit(connection, false);
                    else
                        if (level == (int)System.Data.IsolationLevel.ReadUncommitted)
                            SetAutoCommit(connection, false);
                        else
                            if (level == (int)System.Data.IsolationLevel.RepeatableRead)
                                SetAutoCommit(connection, false);
                            else
                                if (level == (int)System.Data.IsolationLevel.Serializable)
                                    SetAutoCommit(connection, false);

                    if (this[connection] != null)
                    {
                        Properties = ((ConnectionProperties)this[connection]);
                        Properties.TransactionLevel = (System.Data.IsolationLevel)level;
                    }
                    else
                    {
                        Properties = new ConnectionProperties();
                        Properties.AutoCommit = true;
                        Properties.TransactionLevel = (System.Data.IsolationLevel)level;
                        Add(connection, Properties);
                    }
                }

                public int GetTransactionIsolation(System.Data.OleDb.OleDbConnection connection)
                {
                    if (this[connection] != null)
                    {
                        ConnectionProperties Properties = ((ConnectionProperties)this[connection]);
                        if (Properties.TransactionLevel != 0)
                            return (int)Properties.TransactionLevel;
                        else
                            return 2;
                    }
                    else
                        return 2;
                }

                public bool GetAutoCommit(System.Data.OleDb.OleDbConnection connection)
                {
                    if (this[connection] != null)
                        return ((ConnectionProperties)this[connection]).AutoCommit;
                    else
                        return true;
                }

                /// <summary>
                /// Sets the value of a parameter using any permitted object.  The given argument object will be converted to the
                /// corresponding SQL type before being sent to the database.
                /// </summary>
                /// <param name="command">Command object to be changed.</param>
                /// <param name="parameterIndex">One-based index of the parameter to be set.</param>
                /// <param name="parameter">The object containing the input parameter value.</param>
                public void SetValue(System.Data.OleDb.OleDbCommand command, int parameterIndex, System.Object parameter)
                {
                    if (command.Parameters.Count < parameterIndex)
                        command.Parameters.Add(command.CreateParameter());
                    command.Parameters[parameterIndex - 1].Value = parameter;
                }

                /// <summary>
                /// Sets a parameter to SQL NULL.
                /// </summary>
                /// <param name="command">Command object to be changed.</param>
                /// <param name="parameterIndex">One-based index of the parameter to be set.</param>
                /// <param name="targetSqlType">The SQL type to be sent to the database.</param>
                public void SetNull(System.Data.OleDb.OleDbCommand command, int parameterIndex, int sqlType)
                {
                    if (command.Parameters.Count < parameterIndex)
                        command.Parameters.Add(command.CreateParameter());
                    command.Parameters[parameterIndex - 1].Value = System.Convert.DBNull;
                    command.Parameters[parameterIndex - 1].OleDbType = (System.Data.OleDb.OleDbType)sqlType;
                }

                /// <summary>
                /// Sets the value of a parameter using an object.  The given argument object will be converted to the
                /// corresponding SQL type before being sent to the database.
                /// </summary>
                /// <param name="command">Command object to be changed.</param>
                /// <param name="parameterIndex">One-based index of the parameter to be set.</param>
                /// <param name="parameter">The object containing the input parameter value.</param>
                /// <param name="targetSqlType">The SQL type to be sent to the database.</param>
                public void SetObject(System.Data.OleDb.OleDbCommand command, int parameterIndex, System.Object parameter, int targetSqlType)
                {
                    if (command.Parameters.Count < parameterIndex)
                        command.Parameters.Add(command.CreateParameter());
                    command.Parameters[parameterIndex - 1].Value = parameter;
                    command.Parameters[parameterIndex - 1].OleDbType = (System.Data.OleDb.OleDbType)targetSqlType;
                }

                /// <summary>
                /// Sets the value of a parameter using an object.  The given argument object will be converted to the
                /// corresponding SQL type before being sent to the database.
                /// </summary>
                /// <param name="command">Command object to be changed.</param>
                /// <param name="parameterIndex">One-based index of the parameter to be set.</param>
                /// <param name="parameter">The object containing the input parameter value.</param>
                public void SetObject(System.Data.OleDb.OleDbCommand command, int parameterIndex, System.Object parameter)
                {
                    if (command.Parameters.Count < parameterIndex)
                        command.Parameters.Add(command.CreateParameter());
                    command.Parameters[parameterIndex - 1].Value = parameter;
                }

                /// <summary>
                /// This method is for such prepared statements verify if the Conection is autoCommit for assing the transaction to the command.
                /// </summary>
                /// <param name="command">The command to be tested.</param>
                /// <returns>The number of rows afected.</returns>
                public int ExecuteUpdate(System.Data.OleDb.OleDbCommand command)
                {
                    if (!(((ConnectionProperties)this[command.Connection]).AutoCommit))
                    {
                        command.Transaction = ((ConnectionProperties)this[command.Connection]).Transaction;
                        return command.ExecuteNonQuery();
                    }
                    else
                        return command.ExecuteNonQuery();
                }

                /// <summary>
                /// This method Closes the connection, and if the property of autocommit is true make the comit operation
                /// </summary>
                /// <param name="command"> The command to be closed</param>		
                public void Close(System.Data.OleDb.OleDbConnection Connection)
                {

                    if ((this[Connection] != null) && !(((ConnectionProperties)this[Connection]).AutoCommit))
                    {
                        Commit(Connection);
                    }
                    Connection.Close();
                }

                class ConnectionProperties
                {
                    public bool AutoCommit;
                    public System.Data.OleDb.OleDbTransaction Transaction;
                    public System.Data.IsolationLevel TransactionLevel;
                }
            }
        }


        /*******************************/
        /// <summary>
        /// Manager for the connection with the database
        /// </summary>
        public class OleDBSchema
        {
            private System.Data.DataTable schemaData = null;
            private System.Data.OleDb.OleDbConnection Connection;
            private System.Data.ConnectionState ConnectionState;

            /// <summary>
            /// Constructs a new member with the provided connection
            /// </summary>
            /// <param name="Connection">The connection to assign to the new member</param>
            public OleDBSchema(System.Data.OleDb.OleDbConnection Connection)
            {
                this.Connection = Connection;
            }

            /// <summary>
            /// Gets the Driver name of the connection
            /// </summary>
            public System.String DriverName
            {
                get
                {
                    System.String result = "";
                    OpenConnection();
                    result = this.Connection.Provider;
                    CloseConnection();
                    return result;
                }
            }

            /// <summary>
            /// Opens the connection
            /// </summary>
            private void OpenConnection()
            {
                this.ConnectionState = Connection.State;
                this.Connection.Close();
                this.Connection.Open();
                schemaData = null;
            }

            /// <summary>
            /// Closes the connection
            /// </summary>
            private void CloseConnection()
            {
                if (this.ConnectionState == System.Data.ConnectionState.Open)
                    this.Connection.Close();
            }

            /// <summary>
            /// Gets the info of the row
            /// </summary>
            /// <param name="filter">Filter to apply to the row</param>
            /// <param name="RowName">The row from which to obtain the filter</param>
            /// <returns>A new String with the info from the row</returns>
            private System.String GetMaxInfo(System.String filter, System.String RowName)
            {
                System.String result = "";
                schemaData = null;
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.DbInfoLiterals, null);
                foreach (System.Data.DataRow DataRow in schemaData.Rows)
                {
                    if (DataRow["LiteralName"].ToString() == filter)
                    {
                        result = DataRow[RowName].ToString();
                        break;
                    }
                }
                CloseConnection();
                return result;
            }

            /// <summary>
            /// Gets the catalogs from the database to which it is connected
            /// </summary>
            public System.Data.DataTable Catalogs
            {
                get
                {
                    OpenConnection();
                    schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Catalogs, null);
                    CloseConnection();
                    return schemaData;
                }
            }

            /// <summary>
            /// Gets the OleDBConnection for the current member
            /// </summary>
            /// <returns></returns>
            public System.Data.OleDb.OleDbConnection GetConnection()
            {
                return this.Connection;
            }

            /// <summary>
            /// Gets a description of the stored procedures available
            /// </summary>
            /// <param name="catalog">The catalog from which to obtain the procedures</param>
            /// <param name="schemaPattern">Schema pattern, retrieves those without the schema</param>
            /// <param name="procedureNamePattern">a procedure name pattern</param>
            /// <returns>each row but withing a procedure description</returns>
            public System.Data.DataTable GetProcedures(System.String catalog, System.String schemaPattern, System.String procedureNamePattern)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Procedures, new System.Object[] { catalog, schemaPattern, procedureNamePattern, null });
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets a collection of the descriptions of the stored procedures parameters and result columns
            /// </summary>
            /// <param name="catalog">Retrieves those without a catalog</param>
            /// <param name="schemaPattern">Schema pattern, retrieves those without the schema</param>
            /// <param name="procedureNamePattern">a procedure name pattern</param>
            /// <param name="columnNamePattern">a columng name patterm</param>
            /// <returns>Each row but withing a procedure description or column</returns>
            public System.Data.DataTable GetProcedureColumns(System.String catalog, System.String schemaPattern, System.String procedureNamePattern, System.String columnNamePattern)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Procedure_Parameters, new System.Object[] { catalog, schemaPattern, procedureNamePattern, columnNamePattern });
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets a description of the tables available for the catalog
            /// </summary>
            /// <param name="catalog">A catalog, retrieves those without a catalog</param>
            /// <param name="schemaPattern">Schema pattern, retrieves those without the schema</param>
            /// <param name="tableNamePattern">A table name pattern</param>
            /// <param name="types">a list of table types to include</param>
            /// <returns>Each row</returns>
            public System.Data.DataTable GetTables(System.String catalog, System.String schemaPattern, System.String tableNamePattern, System.String[] types)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new System.Object[] { catalog, schemaPattern, tableNamePattern, types[0] });
                if (types != null)
                {
                    for (int i = 1; i < types.Length; i++)
                    {
                        System.Data.DataTable temp_Table = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new System.Object[] { catalog, schemaPattern, tableNamePattern, types[i] });
                        for (int j = 0; j < temp_Table.Rows.Count; j++)
                        {
                            schemaData.ImportRow(temp_Table.Rows[j]);
                        }
                    }
                }
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets a description of the table rights
            /// </summary>
            /// <param name="catalog">A catalog, retrieves those without a catalog</param>
            /// <param name="schemaPattern">Schema pattern, retrieves those without the schema</param>
            /// <param name="tableNamePattern">A table name pattern</param>
            /// <returns>A description of the table rights</returns>
            public System.Data.DataTable GetTablePrivileges(System.String catalog, System.String schemaPattern, System.String tableNamePattern)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Table_Privileges, new System.Object[] { catalog, schemaPattern, tableNamePattern });
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets the table types available
            /// </summary>
            public System.Data.DataTable TableTypes
            {
                get
                {
                    OpenConnection();
                    schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                    System.Collections.ArrayList tableTypes = new System.Collections.ArrayList(schemaData.Rows.Count);

                    System.String tableType = "";
                    foreach (System.Data.DataRow DataRow in schemaData.Rows)
                    {
                        tableType = DataRow[schemaData.Columns["TABLE_TYPE"]].ToString();
                        if (!(tableTypes.Contains(tableType)))
                        {
                            tableTypes.Add(tableType);
                        }
                    }
                    schemaData = new System.Data.DataTable();
                    schemaData.Columns.Add("TABLE_TYPE");
                    for (int index = 0; index < tableTypes.Count; index++)
                    {
                        schemaData.Rows.Add(new System.Object[] { tableTypes[index] });
                    }
                    CloseConnection();
                    return schemaData;
                }
            }

            /// <summary>
            /// Gets a description of the table columns available
            /// </summary>
            /// <param name="catalog">A catalog, retrieves those without a catalog</param>
            /// <param name="schemaPattern">Schema pattern, retrieves those without the schema</param>
            /// <param name="tableNamePattern">A table name pattern</param>
            /// <param name="columnNamePattern">a columng name patterm</param>
            /// <returns>A description of the table columns available</returns>
            public System.Data.DataTable GetColumns(System.String catalog, System.String schemaPattern, System.String tableNamePattern, System.String columnNamePattern)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, new System.Object[] { catalog, schemaPattern, tableNamePattern, columnNamePattern });
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets a description of the primary keys available
            /// </summary>
            /// <param name="catalog">A catalog, retrieves those without a catalog</param>
            /// <param name="schema">Schema name, retrieves those without the schema</param>
            /// <param name="table">A table name</param>
            /// <returns>A description of the primary keys available</returns>
            public System.Data.DataTable GetPrimaryKeys(System.String catalog, System.String schema, System.String table)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Primary_Keys, new System.Object[] { catalog, schema, table });
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets a description of the foreign keys available
            /// </summary>
            /// <param name="catalog">A catalog, retrieves those without a catalog</param>
            /// <param name="schema">Schema name, retrieves those without the schema</param>
            /// <param name="table">A table name</param>
            /// <returns>A description of the foreign keys available</returns>
            public System.Data.DataTable GetForeignKeys(System.String catalog, System.String schema, System.String table)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Foreign_Keys, new System.Object[] { catalog, schema, table });
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets a description of the access rights for	a table columns
            /// </summary>
            /// <param name="catalog">A catalog, retrieves those without a catalog</param>
            /// <param name="schema">Schema name, retrieves those without the schema</param>
            /// <param name="table">A table name</param>
            /// <param name="columnNamePattern">A column name patter</param>
            /// <returns>A description of the access rights for	a table columns</returns>
            public System.Data.DataTable GetColumnPrivileges(System.String catalog, System.String schema, System.String table, System.String columnNamePattern)
            {
                OpenConnection();
                schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Column_Privileges, new System.Object[] { catalog, schema, table, columnNamePattern });
                CloseConnection();
                return schemaData;
            }

            /// <summary>
            /// Gets the provider version
            /// </summary>
            public System.String ProviderVersion
            {
                get
                {
                    System.String result = "";
                    OpenConnection();
                    result = this.Connection.ServerVersion;
                    CloseConnection();
                    return result;
                }
            }

            /// <summary>
            /// Gets the default transaction isolation integer value
            /// </summary>
            public int DefaultTransactionIsolation
            {
                get
                {
                    int result = -1;
                    OpenConnection();
                    System.Data.OleDb.OleDbTransaction Transaction = this.Connection.BeginTransaction();
                    result = (int)Transaction.IsolationLevel;
                    CloseConnection();
                    return result;
                }
            }

            /// <summary>
            /// Gets the schemata for the member
            /// </summary>
            public System.Data.DataTable Schemata
            {
                get
                {
                    OpenConnection();
                    schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Schemata, null);
                    CloseConnection();
                    return schemaData;
                }
            }

            /// <summary>
            /// Gets the provider types for the member
            /// </summary>
            public System.Data.DataTable ProviderTypes
            {
                get
                {
                    OpenConnection();
                    schemaData = this.Connection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Provider_Types, null);
                    CloseConnection();
                    return schemaData;
                }
            }

            /// <summary>
            /// Gets the catalog separator
            /// </summary>
            public System.String CatalogSeparator
            {
                get { return GetMaxInfo("Catalog_Separator", "LiteralValue"); }
            }

            /// <summary>
            /// Gets the maximum binary length permited
            /// </summary>
            public int MaxBinaryLiteralLength
            {
                get
                {
                    System.String len = GetMaxInfo("Binary_Literal", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len) * 4);
                }
            }

            /// <summary>
            /// Gets the maximum catalog name length permited
            /// </summary>
            public int MaxCatalogNameLength
            {
                get
                {
                    System.String len = GetMaxInfo("Catalog_Name", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len));
                }
            }

            /// <summary>
            /// Gets the maximum character literal length permited
            /// </summary>
            public int MaxCharLiteralLength
            {
                get
                {
                    System.String len = GetMaxInfo("Char_Literal", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len) * 4);
                }
            }

            /// <summary>
            /// Gets the maximum column name length
            /// </summary>
            public int MaxColumnNameLength
            {
                get
                {
                    System.String len = GetMaxInfo("Column_Name", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len));
                }
            }

            /// <summary>
            /// Gets the maximum cursor name length
            /// </summary>
            public int MaxCursorNameLength
            {
                get
                {
                    System.String len = GetMaxInfo("Cursor_Name", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len));
                }
            }

            /// <summary>
            /// Gets the maximum procedure name length
            /// </summary>
            public int MaxProcedureNameLength
            {
                get
                {
                    System.String len = GetMaxInfo("Procedure_Name", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len));
                }
            }

            /// <summary>
            /// Gets the maximum schema name length
            /// </summary>
            public int MaxSchemaNameLength
            {
                get
                {
                    System.String len = GetMaxInfo("Schema_Name", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len));
                }
            }

            /// <summary>
            /// Gets the maximum table name length
            /// </summary>
            public int MaxTableNameLength
            {
                get
                {
                    System.String len = GetMaxInfo("Table_Name", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return (System.Convert.ToInt32(len));
                }
            }

            /// <summary>
            /// Gets the maximum user name length
            /// </summary>
            public int MaxUserNameLength
            {
                get
                {
                    System.String len = GetMaxInfo("User_Name", "Maxlen");
                    if (len.Equals(""))
                        return 0;
                    else
                        return System.Convert.ToInt32(len);
                }
            }
        }
        /*******************************/
        /// <summary>
        /// Converts an array of sbytes to an array of chars
        /// </summary>
        /// <param name="sByteArray">The array of sbytes to convert</param>
        /// <returns>The new array of chars</returns>
        public static char[] ToCharArray(sbyte[] sByteArray)
        {
            return System.Text.UTF8Encoding.UTF8.GetChars(ToByteArray(sByteArray));
        }

        /// <summary>
        /// Converts an array of bytes to an array of chars
        /// </summary>
        /// <param name="byteArray">The array of bytes to convert</param>
        /// <returns>The new array of chars</returns>
        public static char[] ToCharArray(byte[] byteArray)
        {
            return System.Text.UTF8Encoding.UTF8.GetChars(byteArray);
        }

        /*******************************/
        /// <summary>
        /// Converts an array of sbytes to an array of bytes
        /// </summary>
        /// <param name="sbyteArray">The array of sbytes to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = null;

            if (sbyteArray != null)
            {
                byteArray = new byte[sbyteArray.Length];
                for (int index = 0; index < sbyteArray.Length; index++)
                    byteArray[index] = (byte)sbyteArray[index];
            }
            return byteArray;
        }

        /// <summary>
        /// Converts a string to an array of bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(System.String sourceString)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(sourceString);
        }

        /// <summary>
        /// Converts a array of object-type instances to a byte-type array.
        /// </summary>
        /// <param name="tempObjectArray">Array to convert.</param>
        /// <returns>An array of byte type elements.</returns>
        public static byte[] ToByteArray(System.Object[] tempObjectArray)
        {
            byte[] byteArray = null;
            if (tempObjectArray != null)
            {
                byteArray = new byte[tempObjectArray.Length];
                for (int index = 0; index < tempObjectArray.Length; index++)
                    byteArray[index] = (byte)tempObjectArray[index];
            }
            return byteArray;
        }

        /*******************************/
        /// <summary>
        /// Receives a byte array and returns it transformed in an sbyte array
        /// </summary>
        /// <param name="byteArray">Byte array to process</param>
        /// <returns>The transformed array</returns>
        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            sbyte[] sbyteArray = null;
            if (byteArray != null)
            {
                sbyteArray = new sbyte[byteArray.Length];
                for (int index = 0; index < byteArray.Length; index++)
                    sbyteArray[index] = (sbyte)byteArray[index];
            }
            return sbyteArray;
        }

        /*******************************/
        /// <summary>
        /// Reverses string values.
        /// </summary>
        /// <param name="text">The StringBuilder object containing the string to be reversed.</param>
        /// <returns>The reversed string contained in a StringBuilder object.</returns>
        public static System.Text.StringBuilder ReverseString(System.Text.StringBuilder text)
        {
            char[] tmpChar = text.ToString().ToCharArray();
            System.Array.Reverse(tmpChar);
            return new System.Text.StringBuilder(new System.String(tmpChar));
        }


        /*******************************/
        /// <summary>
        /// SupportClass for the Stack class.
        /// </summary>
        public class StackSupport
        {
            /// <summary>
            /// Removes the element at the top of the stack and returns it.
            /// </summary>
            /// <param name="stack">The stack where the element at the top will be returned and removed.</param>
            /// <returns>The element at the top of the stack.</returns>
            public static System.Object Pop(System.Collections.ArrayList stack)
            {
                System.Object obj = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                return obj;
            }
        }


        /*******************************/
        /// <summary>
        /// This class has static methods to manage collections.
        /// </summary>
        public class CollectionsSupport
        {
            /// <summary>
            /// Copies the IList to other IList.
            /// </summary>
            /// <param name="SourceList">IList source.</param>
            /// <param name="TargetList">IList target.</param>
            public static void Copy(System.Collections.IList SourceList, System.Collections.IList TargetList)
            {
                for (int i = 0; i < SourceList.Count; i++)
                    TargetList[i] = SourceList[i];
            }

            /// <summary>
            /// Replaces the elements of the specified list with the specified element.
            /// </summary>
            /// <param name="List">The list to be filled with the specified element.</param>
            /// <param name="Element">The element with which to fill the specified list.</param>
            public static void Fill(System.Collections.IList List, System.Object Element)
            {
                for (int i = 0; i < List.Count; i++)
                    List[i] = Element;
            }

            /// <summary>
            /// This class implements System.Collections.IComparer and is used for Comparing two String objects by evaluating 
            /// the numeric values of the corresponding Char objects in each string.
            /// </summary>
            class CompareCharValues : System.Collections.IComparer
            {
                public int Compare(System.Object x, System.Object y)
                {
                    return System.String.CompareOrdinal((System.String)x, (System.String)y);
                }
            }

            /// <summary>
            /// Obtain the maximum element of the given collection with the specified comparator.
            /// </summary>
            /// <param name="Collection">Collection from which the maximum value will be obtained.</param>
            /// <param name="Comparator">The comparator with which to determine the maximum element.</param>
            /// <returns></returns>
            public static System.Object Max(System.Collections.ICollection Collection, System.Collections.IComparer Comparator)
            {
                System.Collections.ArrayList tempArrayList;

                if (((System.Collections.ArrayList)Collection).IsReadOnly)
                    throw new System.NotSupportedException();

                if ((Comparator == null) || (Comparator is System.Collections.Comparer))
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort();
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[Collection.Count - 1];
                }
                else
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort(Comparator);
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[Collection.Count - 1];
                }
            }

            /// <summary>
            /// Obtain the minimum element of the given collection with the specified comparator.
            /// </summary>
            /// <param name="Collection">Collection from which the minimum value will be obtained.</param>
            /// <param name="Comparator">The comparator with which to determine the minimum element.</param>
            /// <returns></returns>
            public static System.Object Min(System.Collections.ICollection Collection, System.Collections.IComparer Comparator)
            {
                System.Collections.ArrayList tempArrayList;

                if (((System.Collections.ArrayList)Collection).IsReadOnly)
                    throw new System.NotSupportedException();

                if ((Comparator == null) || (Comparator is System.Collections.Comparer))
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort();
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[0];
                }
                else
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort(Comparator);
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[0];
                }
            }


            /// <summary>
            /// Sorts an IList collections
            /// </summary>
            /// <param name="list">The System.Collections.IList instance that will be sorted</param>
            /// <param name="Comparator">The Comparator criteria, null to use natural comparator.</param>
            public static void Sort(System.Collections.IList list, System.Collections.IComparer Comparator)
            {
                if (((System.Collections.ArrayList)list).IsReadOnly)
                    throw new System.NotSupportedException();

                if ((Comparator == null) || (Comparator is System.Collections.Comparer))
                {
                    try
                    {
                        ((System.Collections.ArrayList)list).Sort();
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                }
                else
                {
                    try
                    {
                        ((System.Collections.ArrayList)list).Sort(Comparator);
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                }
            }

            /// <summary>
            /// Shuffles the list randomly.
            /// </summary>
            /// <param name="List">The list to be shuffled.</param>
            public static void Shuffle(System.Collections.IList List)
            {
                System.Random RandomList = new System.Random(unchecked((int)System.DateTime.Now.Ticks));
                Shuffle(List, RandomList);
            }

            /// <summary>
            /// Shuffles the list randomly.
            /// </summary>
            /// <param name="List">The list to be shuffled.</param>
            /// <param name="RandomList">The random to use to shuffle the list.</param>
            public static void Shuffle(System.Collections.IList List, System.Random RandomList)
            {
                System.Object source = null;
                int target = 0;

                for (int i = 0; i < List.Count; i++)
                {
                    target = RandomList.Next(List.Count);
                    source = (System.Object)List[i];
                    List[i] = List[target];
                    List[target] = source;
                }
            }
        }


        /*******************************/
        /// <summary>
        /// This class provides auxiliar functionality to read and unread characters from a string into a buffer.
        /// </summary>
        private class BackStringReader : System.IO.StringReader
        {
            private char[] buffer;
            private int position = 1;

            /// <summary>
            /// Constructor. Calls the base constructor.
            /// </summary>
            /// <param name="stringReader">The buffer from which chars will be read.</param>
            /// <param name="size">The size of the Back buffer.</param>
            public BackStringReader(String s)
                : base(s)
            {
                this.buffer = new char[position];
            }


            /// <summary>
            /// Reads a character.
            /// </summary>
            /// <returns>The character read.</returns>
            public override int Read()
            {
                if (this.position >= 0 && this.position < this.buffer.Length)
                    return (int)this.buffer[this.position++];
                return base.Read();
            }

            /// <summary>
            /// Reads an amount of characters from the buffer and copies the values to the array passed.
            /// </summary>
            /// <param name="array">Array where the characters will be stored.</param>
            /// <param name="index">The beginning index to read.</param>
            /// <param name="count">The number of characters to read.</param>
            /// <returns>The number of characters read.</returns>
            public override int Read(char[] array, int index, int count)
            {
                int readLimit = this.buffer.Length - this.position;

                if (count <= 0)
                    return 0;

                if (readLimit > 0)
                {
                    if (count < readLimit)
                        readLimit = count;
                    System.Array.Copy(this.buffer, this.position, array, index, readLimit);
                    count -= readLimit;
                    index += readLimit;
                    this.position += readLimit;
                }

                if (count > 0)
                {
                    count = base.Read(array, index, count);
                    if (count == -1)
                    {
                        if (readLimit == 0)
                            return -1;
                        return readLimit;
                    }
                    return readLimit + count;
                }
                return readLimit;
            }

            /// <summary>
            /// Unreads a character.
            /// </summary>
            /// <param name="unReadChar">The character to be unread.</param>
            public void UnRead(int unReadChar)
            {
                this.position--;
                this.buffer[this.position] = (char)unReadChar;
            }

            /// <summary>
            /// Unreads an amount of characters by moving these to the buffer.
            /// </summary>
            /// <param name="array">The character array to be unread.</param>
            /// <param name="index">The beginning index to unread.</param>
            /// <param name="count">The number of characters to unread.</param>
            public void UnRead(char[] array, int index, int count)
            {
                this.Move(array, index, count);
            }

            /// <summary>
            /// Unreads an amount of characters by moving these to the buffer.
            /// </summary>
            /// <param name="array">The character array to be unread.</param>
            public void UnRead(char[] array)
            {
                this.Move(array, 0, array.Length - 1);
            }

            /// <summary>
            /// Moves the array of characters to the buffer.
            /// </summary>
            /// <param name="array">Array of characters to move.</param>
            /// <param name="index">Offset of the beginning.</param>
            /// <param name="count">Amount of characters to move.</param>
            private void Move(char[] array, int index, int count)
            {
                for (int arrayPosition = index + count; arrayPosition >= index; arrayPosition--)
                    this.UnRead(array[arrayPosition]);
            }
        }

        /*******************************/

        /// <summary>
        /// The StreamTokenizerSupport class takes an input stream and parses it into "tokens".
        /// The stream tokenizer can recognize identifiers, numbers, quoted strings, and various comment styles. 
        /// </summary>
        public class StreamTokenizerSupport
        {

            /// <summary>
            /// Internal constants and fields
            /// </summary>

            private const System.String TOKEN = "Token[";
            private const System.String NOTHING = "NOTHING";
            private const System.String NUMBER = "number=";
            private const System.String EOF = "EOF";
            private const System.String EOL = "EOL";
            private const System.String QUOTED = "quoted string=";
            private const System.String LINE = "], Line ";
            private const System.String DASH = "-.";
            private const System.String DOT = ".";

            private const int TT_NOTHING = -4;

            private const sbyte ORDINARYCHAR = 0x00;
            private const sbyte WORDCHAR = 0x01;
            private const sbyte WHITESPACECHAR = 0x02;
            private const sbyte COMMENTCHAR = 0x04;
            private const sbyte QUOTECHAR = 0x08;
            private const sbyte NUMBERCHAR = 0x10;

            private const int STATE_NEUTRAL = 0;
            private const int STATE_WORD = 1;
            private const int STATE_NUMBER1 = 2;
            private const int STATE_NUMBER2 = 3;
            private const int STATE_NUMBER3 = 4;
            private const int STATE_NUMBER4 = 5;
            private const int STATE_STRING = 6;
            private const int STATE_LINECOMMENT = 7;
            private const int STATE_DONE_ON_EOL = 8;

            private const int STATE_PROCEED_ON_EOL = 9;
            private const int STATE_POSSIBLEC_COMMENT = 10;
            private const int STATE_POSSIBLEC_COMMENT_END = 11;
            private const int STATE_C_COMMENT = 12;
            private const int STATE_STRING_ESCAPE_SEQ = 13;
            private const int STATE_STRING_ESCAPE_SEQ_OCTAL = 14;

            private const int STATE_DONE = 100;

            private sbyte[] attribute = new sbyte[256];
            private bool eolIsSignificant = false;
            private bool slashStarComments = false;
            private bool slashSlashComments = false;
            private bool lowerCaseMode = false;
            private bool pushedback = false;
            private int lineno = 1;

            private BackReader inReader;
            private BackStringReader inStringReader;
            private BackInputStream inStream;
            private System.Text.StringBuilder buf;


            /// <summary>
            /// Indicates that the end of the stream has been read.
            /// </summary>
            public const int TT_EOF = -1;

            /// <summary>
            /// Indicates that the end of the line has been read.
            /// </summary>
            public const int TT_EOL = '\n';

            /// <summary>
            /// Indicates that a number token has been read.
            /// </summary>
            public const int TT_NUMBER = -2;

            /// <summary>
            /// Indicates that a word token has been read.
            /// </summary>
            public const int TT_WORD = -3;

            /// <summary>
            /// If the current token is a number, this field contains the value of that number.
            /// </summary>
            public double nval;

            /// <summary>
            /// If the current token is a word token, this field contains a string giving the characters of the word 
            /// token.
            /// </summary>
            public System.String sval;

            /// <summary>
            /// After a call to the nextToken method, this field contains the type of the token just read.
            /// </summary>
            public int ttype;


            /// <summary>
            /// Internal methods
            /// </summary>

            private int read()
            {
                if (this.inReader != null)
                    return this.inReader.Read();
                else if (this.inStream != null)
                    return this.inStream.Read();
                else
                    return this.inStringReader.Read();
            }

            private void unread(int ch)
            {
                if (this.inReader != null)
                    this.inReader.UnRead(ch);
                else if (this.inStream != null)
                    this.inStream.UnRead(ch);
                else
                    this.inStringReader.UnRead(ch);
            }

            private void init()
            {
                this.buf = new System.Text.StringBuilder();
                this.ttype = StreamTokenizerSupport.TT_NOTHING;

                this.WordChars('A', 'Z');
                this.WordChars('a', 'z');
                this.WordChars(160, 255);
                this.WhitespaceChars(0x00, 0x20);
                this.CommentChar('/');
                this.QuoteChar('\'');
                this.QuoteChar('\"');
                this.ParseNumbers();
            }

            private void setAttributes(int low, int hi, sbyte attrib)
            {
                int l = System.Math.Max(0, low);
                int h = System.Math.Min(255, hi);
                for (int i = l; i <= h; i++)
                    this.attribute[i] = attrib;
            }

            private bool isWordChar(int data)
            {
                char ch = (char)data;
                return (data != -1 && (ch > 255 || this.attribute[ch] == StreamTokenizerSupport.WORDCHAR || this.attribute[ch] == StreamTokenizerSupport.NUMBERCHAR));
            }

            /// <summary>
            /// Creates a StreamToknizerSupport that parses the given string.
            /// </summary>
            /// <param name="reader">The System.IO.StringReader that contains the String to be parsed.</param>
            public StreamTokenizerSupport(System.IO.StringReader reader)
            {
                string s = "";
                for (int i = reader.Read(); i != -1; i = reader.Read())
                {
                    s += (char)i;
                }
                reader.Close();
                this.inStringReader = new BackStringReader(s);
                this.init();
            }

            /// <summary>
            /// Creates a StreamTokenizerSupport that parses the given stream.
            /// </summary>
            /// <param name="reader">Reader to be parsed.</param>
            public StreamTokenizerSupport(System.IO.StreamReader reader)
            {
                this.inReader = new BackReader(new System.IO.StreamReader(reader.BaseStream, reader.CurrentEncoding).BaseStream, 2, reader.CurrentEncoding);
                this.init();
            }

            /// <summary>
            /// Creates a StreamTokenizerSupport that parses the given stream.
            /// </summary>
            /// <param name="stream">Stream to be parsed.</param>
            public StreamTokenizerSupport(System.IO.Stream stream)
            {
                this.inStream = new BackInputStream(new System.IO.BufferedStream(stream), 2);
                this.init();
            }

            /// <summary>
            /// Specified that the character argument starts a single-line comment.
            /// </summary>
            /// <param name="ch">The character.</param>
            public virtual void CommentChar(int ch)
            {
                if (ch >= 0 && ch <= 255)
                    this.attribute[ch] = StreamTokenizerSupport.COMMENTCHAR;
            }

            /// <summary>
            /// Determines whether or not ends of line are treated as tokens.
            /// </summary>
            /// <param name="flag">True indicates that end-of-line characters are separate tokens; False indicates 
            /// that end-of-line characters are white space.</param>
            public virtual void EOLIsSignificant(bool flag)
            {
                this.eolIsSignificant = flag;
            }

            /// <summary>
            /// Return the current line number.
            /// </summary>
            /// <returns>Current line number</returns>
            public virtual int Lineno()
            {
                return this.lineno;
            }

            /// <summary>
            /// Determines whether or not word token are automatically lowercased.
            /// </summary>
            /// <param name="flag">True indicates that all word tokens should be lowercased.</param>
            public virtual void LowerCaseMode(bool flag)
            {
                this.lowerCaseMode = flag;
            }

            /// <summary>
            /// Parses the next token from the input stream of this tokenizer.
            /// </summary>
            /// <returns>The value of the ttype field.</returns>
            public virtual int NextToken()
            {
                char prevChar = (char)(0);
                char ch = (char)(0);
                char qChar = (char)(0);
                int octalNumber = 0;
                int state;

                if (this.pushedback)
                {
                    this.pushedback = false;
                    return this.ttype;
                }

                this.ttype = StreamTokenizerSupport.TT_NOTHING;
                state = StreamTokenizerSupport.STATE_NEUTRAL;
                this.nval = 0.0;
                this.sval = null;
                this.buf.Length = 0;

                do
                {
                    int data = this.read();
                    prevChar = ch;
                    ch = (char)data;

                    switch (state)
                    {
                        case StreamTokenizerSupport.STATE_NEUTRAL:
                            {
                                if (data == -1)
                                {
                                    this.ttype = TT_EOF;
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                else if (ch > 255)
                                {
                                    this.buf.Append(ch);
                                    this.ttype = StreamTokenizerSupport.TT_WORD;
                                    state = StreamTokenizerSupport.STATE_WORD;
                                }
                                else if (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR)
                                {
                                    state = StreamTokenizerSupport.STATE_LINECOMMENT;
                                }
                                else if (this.attribute[ch] == StreamTokenizerSupport.WORDCHAR)
                                {
                                    this.buf.Append(ch);
                                    this.ttype = StreamTokenizerSupport.TT_WORD;
                                    state = StreamTokenizerSupport.STATE_WORD;
                                }
                                else if (this.attribute[ch] == StreamTokenizerSupport.NUMBERCHAR)
                                {
                                    this.ttype = StreamTokenizerSupport.TT_NUMBER;
                                    this.buf.Append(ch);
                                    if (ch == '-')
                                        state = StreamTokenizerSupport.STATE_NUMBER1;
                                    else if (ch == '.')
                                        state = StreamTokenizerSupport.STATE_NUMBER3;
                                    else
                                        state = StreamTokenizerSupport.STATE_NUMBER2;
                                }
                                else if (this.attribute[ch] == StreamTokenizerSupport.QUOTECHAR)
                                {
                                    qChar = ch;
                                    this.ttype = ch;
                                    state = StreamTokenizerSupport.STATE_STRING;
                                }
                                else if ((this.slashSlashComments || this.slashStarComments) && ch == '/')
                                    state = StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT;
                                else if (this.attribute[ch] == StreamTokenizerSupport.ORDINARYCHAR)
                                {
                                    this.ttype = ch;
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                else if (ch == '\n' || ch == '\r')
                                {
                                    this.lineno++;
                                    if (this.eolIsSignificant)
                                    {
                                        this.ttype = StreamTokenizerSupport.TT_EOL;
                                        if (ch == '\n')
                                            state = StreamTokenizerSupport.STATE_DONE;
                                        else if (ch == '\r')
                                            state = StreamTokenizerSupport.STATE_DONE_ON_EOL;
                                    }
                                    else if (ch == '\r')
                                        state = StreamTokenizerSupport.STATE_PROCEED_ON_EOL;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_WORD:
                            {
                                if (this.isWordChar(data))
                                    this.buf.Append(ch);
                                else
                                {
                                    if (data != -1)
                                        this.unread(ch);
                                    this.sval = this.buf.ToString();
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_NUMBER1:
                            {
                                if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-')
                                {
                                    if (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR && System.Char.IsNumber(ch))
                                    {
                                        this.buf.Append(ch);
                                        state = StreamTokenizerSupport.STATE_NUMBER2;
                                    }
                                    else
                                    {
                                        if (data != -1)
                                            this.unread(ch);
                                        this.ttype = '-';
                                        state = StreamTokenizerSupport.STATE_DONE;
                                    }
                                }
                                else
                                {
                                    this.buf.Append(ch);
                                    if (ch == '.')
                                        state = StreamTokenizerSupport.STATE_NUMBER3;
                                    else
                                        state = StreamTokenizerSupport.STATE_NUMBER2;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_NUMBER2:
                            {
                                if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-')
                                {
                                    if (System.Char.IsNumber(ch) && this.attribute[ch] == StreamTokenizerSupport.WORDCHAR)
                                    {
                                        this.buf.Append(ch);
                                    }
                                    else if (ch == '.' && this.attribute[ch] == StreamTokenizerSupport.WHITESPACECHAR)
                                    {
                                        this.buf.Append(ch);
                                    }

                                    else if ((data != -1) && (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR && System.Char.IsNumber(ch)))
                                    {
                                        this.buf.Append(ch);
                                    }
                                    else
                                    {
                                        if (data != -1)
                                            this.unread(ch);
                                        try
                                        {
                                            this.nval = System.Double.Parse(this.buf.ToString());
                                        }
                                        catch (System.FormatException) { }
                                        state = StreamTokenizerSupport.STATE_DONE;
                                    }
                                }
                                else
                                {
                                    this.buf.Append(ch);
                                    if (ch == '.')
                                        state = StreamTokenizerSupport.STATE_NUMBER3;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_NUMBER3:
                            {
                                if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-' || ch == '.')
                                {
                                    if (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR && System.Char.IsNumber(ch))
                                    {
                                        this.buf.Append(ch);
                                    }
                                    else
                                    {
                                        if (data != -1)
                                            this.unread(ch);
                                        System.String str = this.buf.ToString();
                                        if (str.Equals(StreamTokenizerSupport.DASH))
                                        {
                                            this.unread('.');
                                            this.ttype = '-';
                                        }
                                        else if (str.Equals(StreamTokenizerSupport.DOT) && !(StreamTokenizerSupport.WORDCHAR != this.attribute[prevChar]))
                                            this.ttype = '.';
                                        else
                                        {
                                            try
                                            {
                                                this.nval = System.Double.Parse(str);
                                            }
                                            catch (System.FormatException) { }
                                        }
                                        state = StreamTokenizerSupport.STATE_DONE;
                                    }
                                }
                                else
                                {
                                    this.buf.Append(ch);
                                    state = StreamTokenizerSupport.STATE_NUMBER4;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_NUMBER4:
                            {
                                if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-' || ch == '.')
                                {
                                    if (data != -1)
                                        this.unread(ch);
                                    try
                                    {
                                        this.nval = System.Double.Parse(this.buf.ToString());
                                    }
                                    catch (System.FormatException) { }
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                else
                                    this.buf.Append(ch);
                                break;
                            }
                        case StreamTokenizerSupport.STATE_LINECOMMENT:
                            {
                                if (data == -1)
                                {
                                    this.ttype = StreamTokenizerSupport.TT_EOF;
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                else if (ch == '\n' || ch == '\r')
                                {
                                    this.unread(ch);
                                    state = StreamTokenizerSupport.STATE_NEUTRAL;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_DONE_ON_EOL:
                            {
                                if (ch != '\n' && data != -1)
                                    this.unread(ch);
                                state = StreamTokenizerSupport.STATE_DONE;
                                break;
                            }
                        case StreamTokenizerSupport.STATE_PROCEED_ON_EOL:
                            {
                                if (ch != '\n' && data != -1)
                                    this.unread(ch);
                                state = StreamTokenizerSupport.STATE_NEUTRAL;
                                break;
                            }
                        case StreamTokenizerSupport.STATE_STRING:
                            {
                                if (data == -1 || ch == qChar || ch == '\r' || ch == '\n')
                                {
                                    this.sval = this.buf.ToString();
                                    if (ch == '\r' || ch == '\n')
                                        this.unread(ch);
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                else if (ch == '\\')
                                    state = StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ;
                                else
                                    this.buf.Append(ch);
                                break;
                            }
                        case StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ:
                            {
                                if (data == -1)
                                {
                                    this.sval = this.buf.ToString();
                                    state = StreamTokenizerSupport.STATE_DONE;
                                    break;
                                }

                                state = StreamTokenizerSupport.STATE_STRING;
                                if (ch == 'a')
                                    this.buf.Append(0x7);
                                else if (ch == 'b')
                                    this.buf.Append('\b');
                                else if (ch == 'f')
                                    this.buf.Append(0xC);
                                else if (ch == 'n')
                                    this.buf.Append('\n');
                                else if (ch == 'r')
                                    this.buf.Append('\r');
                                else if (ch == 't')
                                    this.buf.Append('\t');
                                else if (ch == 'v')
                                    this.buf.Append(0xB);
                                else if (ch >= '0' && ch <= '7')
                                {
                                    octalNumber = ch - '0';
                                    state = StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ_OCTAL;
                                }
                                else
                                    this.buf.Append(ch);
                                break;
                            }
                        case StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ_OCTAL:
                            {
                                if (data == -1 || ch < '0' || ch > '7')
                                {
                                    this.buf.Append((char)octalNumber);
                                    if (data == -1)
                                    {
                                        this.sval = buf.ToString();
                                        state = StreamTokenizerSupport.STATE_DONE;
                                    }
                                    else
                                    {
                                        this.unread(ch);
                                        state = StreamTokenizerSupport.STATE_STRING;
                                    }
                                }
                                else
                                {
                                    int temp = octalNumber * 8 + (ch - '0');
                                    if (temp < 256)
                                        octalNumber = temp;
                                    else
                                    {
                                        buf.Append((char)octalNumber);
                                        buf.Append(ch);
                                        state = StreamTokenizerSupport.STATE_STRING;
                                    }
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT:
                            {
                                if (ch == '*')
                                    state = StreamTokenizerSupport.STATE_C_COMMENT;
                                else if (ch == '/')
                                    state = StreamTokenizerSupport.STATE_LINECOMMENT;
                                else
                                {
                                    if (data != -1)
                                        this.unread(ch);
                                    this.ttype = '/';
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_C_COMMENT:
                            {
                                if (ch == '*')
                                    state = StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT_END;
                                if (ch == '\n')
                                    this.lineno++;
                                else if (data == -1)
                                {
                                    this.ttype = StreamTokenizerSupport.TT_EOF;
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                break;
                            }
                        case StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT_END:
                            {
                                if (data == -1)
                                {
                                    this.ttype = StreamTokenizerSupport.TT_EOF;
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                else if (ch == '/')
                                    state = StreamTokenizerSupport.STATE_NEUTRAL;
                                else if (ch != '*')
                                    state = StreamTokenizerSupport.STATE_C_COMMENT;
                                break;
                            }
                    }
                }
                while (state != StreamTokenizerSupport.STATE_DONE);

                if (this.ttype == StreamTokenizerSupport.TT_WORD && this.lowerCaseMode)
                    this.sval = this.sval.ToLower();

                return this.ttype;
            }

            /// <summary>
            /// Specifies that the character argument is "ordinary" in this tokenizer.
            /// </summary>
            /// <param name="ch">The character.</param>
            public virtual void OrdinaryChar(int ch)
            {
                if (ch >= 0 && ch <= 255)
                    this.attribute[ch] = StreamTokenizerSupport.ORDINARYCHAR;
            }

            /// <summary>
            /// Specifies that all characters c in the range low less-equal c less-equal high are "ordinary" in this 
            /// tokenizer.
            /// </summary>
            /// <param name="low">Low end of the range.</param>
            /// <param name="hi">High end of the range.</param>
            public virtual void OrdinaryChars(int low, int hi)
            {
                this.setAttributes(low, hi, StreamTokenizerSupport.ORDINARYCHAR);
            }

            /// <summary>
            /// Specifies that numbers should be parsed by this tokenizer.
            /// </summary>
            public virtual void ParseNumbers()
            {
                for (int i = '0'; i <= '9'; i++)
                    this.attribute[i] = StreamTokenizerSupport.NUMBERCHAR;
                this.attribute['.'] = StreamTokenizerSupport.NUMBERCHAR;
                this.attribute['-'] = StreamTokenizerSupport.NUMBERCHAR;
            }

            /// <summary>
            /// Causes the next call to the nextToken method of this tokenizer to return the current value in the 
            /// ttype field, and not to modify the value in the nval or sval field.
            /// </summary>
            public virtual void PushBack()
            {
                if (this.ttype != StreamTokenizerSupport.TT_NOTHING)
                    this.pushedback = true;
            }

            /// <summary>
            /// Specifies that matching pairs of this character delimit string constants in this tokenizer.
            /// </summary>
            /// <param name="ch">The character.</param>
            public virtual void QuoteChar(int ch)
            {
                if (ch >= 0 && ch <= 255)
                    this.attribute[ch] = QUOTECHAR;
            }

            /// <summary>
            /// Resets this tokenizer's syntax table so that all characters are "ordinary." See the ordinaryChar 
            /// method for more information on a character being ordinary.
            /// </summary>
            public virtual void ResetSyntax()
            {
                this.OrdinaryChars(0x00, 0xff);
            }

            /// <summary>
            /// Determines whether or not the tokenizer recognizes C++-style comments.
            /// </summary>
            /// <param name="flag">True indicates to recognize and ignore C++-style comments.</param>
            public virtual void SlashSlashComments(bool flag)
            {
                this.slashSlashComments = flag;
            }

            /// <summary>
            /// Determines whether or not the tokenizer recognizes C-style comments.
            /// </summary>
            /// <param name="flag">True indicates to recognize and ignore C-style comments.</param>
            public virtual void SlashStarComments(bool flag)
            {
                this.slashStarComments = flag;
            }

            /// <summary>
            /// Returns the string representation of the current stream token.
            /// </summary>
            /// <returns>A String representation of the current stream token.</returns>
            public override System.String ToString()
            {
                System.Text.StringBuilder buffer = new System.Text.StringBuilder(StreamTokenizerSupport.TOKEN);

                switch (this.ttype)
                {
                    case StreamTokenizerSupport.TT_NOTHING:
                        {
                            buffer.Append(StreamTokenizerSupport.NOTHING);
                            break;
                        }
                    case StreamTokenizerSupport.TT_WORD:
                        {
                            buffer.Append(this.sval);
                            break;
                        }
                    case StreamTokenizerSupport.TT_NUMBER:
                        {
                            buffer.Append(StreamTokenizerSupport.NUMBER);
                            buffer.Append(this.nval);
                            break;
                        }
                    case StreamTokenizerSupport.TT_EOF:
                        {
                            buffer.Append(StreamTokenizerSupport.EOF);
                            break;
                        }
                    case StreamTokenizerSupport.TT_EOL:
                        {
                            buffer.Append(StreamTokenizerSupport.EOL);
                            break;
                        }
                }

                if (this.ttype > 0)
                {
                    if (this.attribute[this.ttype] == StreamTokenizerSupport.QUOTECHAR)
                    {
                        buffer.Append(StreamTokenizerSupport.QUOTED);
                        buffer.Append(this.sval);
                    }
                    else
                    {
                        buffer.Append('\'');
                        buffer.Append((char)this.ttype);
                        buffer.Append('\'');
                    }
                }

                buffer.Append(StreamTokenizerSupport.LINE);
                buffer.Append(this.lineno);
                return buffer.ToString();
            }

            /// <summary>
            /// Specifies that all characters c in the range low less-equal c less-equal high are white space 
            /// characters.
            /// </summary>
            /// <param name="low">The low end of the range.</param>
            /// <param name="hi">The high end of the range.</param>
            public virtual void WhitespaceChars(int low, int hi)
            {
                this.setAttributes(low, hi, StreamTokenizerSupport.WHITESPACECHAR);
            }

            /// <summary>
            /// Specifies that all characters c in the range low less-equal c less-equal high are word constituents.
            /// </summary>
            /// <param name="low">The low end of the range.</param>
            /// <param name="hi">The high end of the range.</param>
            public virtual void WordChars(int low, int hi)
            {
                this.setAttributes(low, hi, StreamTokenizerSupport.WORDCHAR);
            }
        }


        /*******************************/
        /// <summary>
        /// This class provides functionality to reads and unread characters into a buffer.
        /// </summary>
        public class BackReader : System.IO.StreamReader
        {
            private char[] buffer;
            private int position = 1;
            //private int markedPosition;

            /// <summary>
            /// Constructor. Calls the base constructor.
            /// </summary>
            /// <param name="streamReader">The buffer from which chars will be read.</param>
            /// <param name="size">The size of the Back buffer.</param>
            public BackReader(System.IO.Stream streamReader, int size, System.Text.Encoding encoding)
                : base(streamReader, encoding)
            {
                this.buffer = new char[size];
                this.position = size;
            }

            /// <summary>
            /// Constructor. Calls the base constructor.
            /// </summary>
            /// <param name="streamReader">The buffer from which chars will be read.</param>
            public BackReader(System.IO.Stream streamReader, System.Text.Encoding encoding)
                : base(streamReader, encoding)
            {
                this.buffer = new char[this.position];
            }

            /// <summary>
            /// Checks if this stream support mark and reset methods.
            /// </summary>
            /// <remarks>
            /// This method isn't supported.
            /// </remarks>
            /// <returns>Always false.</returns>
            public bool MarkSupported()
            {
                return false;
            }

            /// <summary>
            /// Marks the element at the corresponding position.
            /// </summary>
            /// <remarks>
            /// This method isn't supported.
            /// </remarks>
            public void Mark(int position)
            {
                throw new System.IO.IOException("Mark operations are not allowed");
            }

            /// <summary>
            /// Resets the current stream.
            /// </summary>
            /// <remarks>
            /// This method isn't supported.
            /// </remarks>
            public void Reset()
            {
                throw new System.IO.IOException("Mark operations are not allowed");
            }

            /// <summary>
            /// Reads a character.
            /// </summary>
            /// <returns>The character read.</returns>
            public override int Read()
            {
                if (this.position >= 0 && this.position < this.buffer.Length)
                    return (int)this.buffer[this.position++];
                return base.Read();
            }

            /// <summary>
            /// Reads an amount of characters from the buffer and copies the values to the array passed.
            /// </summary>
            /// <param name="array">Array where the characters will be stored.</param>
            /// <param name="index">The beginning index to read.</param>
            /// <param name="count">The number of characters to read.</param>
            /// <returns>The number of characters read.</returns>
            public override int Read(char[] array, int index, int count)
            {
                int readLimit = this.buffer.Length - this.position;

                if (count <= 0)
                    return 0;

                if (readLimit > 0)
                {
                    if (count < readLimit)
                        readLimit = count;
                    System.Array.Copy(this.buffer, this.position, array, index, readLimit);
                    count -= readLimit;
                    index += readLimit;
                    this.position += readLimit;
                }

                if (count > 0)
                {
                    count = base.Read(array, index, count);
                    if (count == -1)
                    {
                        if (readLimit == 0)
                            return -1;
                        return readLimit;
                    }
                    return readLimit + count;
                }
                return readLimit;
            }

            /// <summary>
            /// Checks if this buffer is ready to be read.
            /// </summary>
            /// <returns>True if the position is less than the length, otherwise false.</returns>
            public bool IsReady()
            {
                return (this.position >= this.buffer.Length || this.BaseStream.Position >= this.BaseStream.Length);
            }

            /// <summary>
            /// Unreads a character.
            /// </summary>
            /// <param name="unReadChar">The character to be unread.</param>
            public void UnRead(int unReadChar)
            {
                this.position--;
                this.buffer[this.position] = (char)unReadChar;
            }

            /// <summary>
            /// Unreads an amount of characters by moving these to the buffer.
            /// </summary>
            /// <param name="array">The character array to be unread.</param>
            /// <param name="index">The beginning index to unread.</param>
            /// <param name="count">The number of characters to unread.</param>
            public void UnRead(char[] array, int index, int count)
            {
                this.Move(array, index, count);
            }

            /// <summary>
            /// Unreads an amount of characters by moving these to the buffer.
            /// </summary>
            /// <param name="array">The character array to be unread.</param>
            public void UnRead(char[] array)
            {
                this.Move(array, 0, array.Length - 1);
            }

            /// <summary>
            /// Moves the array of characters to the buffer.
            /// </summary>
            /// <param name="array">Array of characters to move.</param>
            /// <param name="index">Offset of the beginning.</param>
            /// <param name="count">Amount of characters to move.</param>
            private void Move(char[] array, int index, int count)
            {
                for (int arrayPosition = index + count; arrayPosition >= index; arrayPosition--)
                    this.UnRead(array[arrayPosition]);
            }
        }


        /*******************************/
        /// <summary>
        /// Provides functionality to read and unread from a Stream.
        /// </summary>
        public class BackInputStream : System.IO.BinaryReader
        {
            private byte[] buffer;
            private int position = 1;

            /// <summary>
            /// Creates a BackInputStream with the specified stream and size for the buffer.
            /// </summary>
            /// <param name="streamReader">The stream to use.</param>
            /// <param name="size">The specific size of the buffer.</param>
            public BackInputStream(System.IO.Stream streamReader, System.Int32 size)
                : base(streamReader)
            {
                this.buffer = new byte[size];
                this.position = size;
            }

            /// <summary>
            /// Creates a BackInputStream with the specified stream.
            /// </summary>
            /// <param name="streamReader">The stream to use.</param>
            public BackInputStream(System.IO.Stream streamReader)
                : base(streamReader)
            {
                this.buffer = new byte[this.position];
            }

            /// <summary>
            /// Checks if this stream support mark and reset methods.
            /// </summary>
            /// <returns>Always false, these methods aren't supported.</returns>
            public bool MarkSupported()
            {
                return false;
            }

            /// <summary>
            /// Reads the next bytes in the stream.
            /// </summary>
            /// <returns>The next byte readed</returns>
            public override int Read()
            {
                if (position >= 0 && position < buffer.Length)
                    return (int)this.buffer[position++];
                return base.Read();
            }

            /// <summary>
            /// Reads the amount of bytes specified from the stream.
            /// </summary>
            /// <param name="array">The buffer to read data into.</param>
            /// <param name="index">The beginning point to read.</param>
            /// <param name="count">The number of characters to read.</param>
            /// <returns>The number of characters read into buffer.</returns>
            public virtual int Read(sbyte[] array, int index, int count)
            {
                int byteCount = 0;
                int readLimit = count + index;
                byte[] aux = ToByteArray(array);

                for (byteCount = 0; position < buffer.Length && index < readLimit; byteCount++)
                    aux[index++] = buffer[position++];

                if (index < readLimit)
                    byteCount += base.Read(aux, index, readLimit - index);

                for (int i = 0; i < aux.Length; i++)
                    array[i] = (sbyte)aux[i];

                return byteCount;
            }

            /// <summary>
            /// Unreads a byte from the stream.
            /// </summary>
            /// <param name="element">The value to be unread.</param>
            public void UnRead(int element)
            {
                this.position--;
                if (position >= 0)
                    this.buffer[this.position] = (byte)element;
            }

            /// <summary>
            /// Unreads an amount of bytes from the stream.
            /// </summary>
            /// <param name="array">The byte array to be unread.</param>
            /// <param name="index">The beginning index to unread.</param>
            /// <param name="count">The number of bytes to be unread.</param>
            public void UnRead(byte[] array, int index, int count)
            {
                this.Move(array, index, count);
            }

            /// <summary>
            /// Unreads an array of bytes from the stream.
            /// </summary>
            /// <param name="array">The byte array to be unread.</param>
            public void UnRead(byte[] array)
            {
                this.Move(array, 0, array.Length - 1);
            }

            /// <summary>
            /// Skips the specified number of bytes from the underlying stream.
            /// </summary>
            /// <param name="numberOfBytes">The number of bytes to be skipped.</param>
            /// <returns>The number of bytes actually skipped</returns>
            public long Skip(long numberOfBytes)
            {
                return this.BaseStream.Seek(numberOfBytes, System.IO.SeekOrigin.Current) - this.BaseStream.Position;
            }

            /// <summary>
            /// Moves data from the array to the buffer field.
            /// </summary>
            /// <param name="array">The array of bytes to be unread.</param>
            /// <param name="index">The beginning index to unread.</param>
            /// <param name="count">The amount of bytes to be unread.</param>
            private void Move(byte[] array, int index, int count)
            {
                for (int arrayPosition = index + count; arrayPosition >= index; arrayPosition--)
                    this.UnRead(array[arrayPosition]);
            }
        }


        /*******************************/
        /// <summary>
        /// Provides support for DateFormat
        /// </summary>
        public class DateTimeFormatManager
        {
            static public DateTimeFormatHashTable manager = new DateTimeFormatHashTable();

            /// <summary>
            /// Hashtable class to provide functionality for dateformat properties
            /// </summary>
            public class DateTimeFormatHashTable : System.Collections.Hashtable
            {
                /// <summary>
                /// Sets the format for datetime.
                /// </summary>
                /// <param name="format">DateTimeFormat instance to set the pattern</param>
                /// <param name="newPattern">A string with the pattern format</param>
                public void SetDateFormatPattern(System.Globalization.DateTimeFormatInfo format, System.String newPattern)
                {
                    if (this[format] != null)
                        ((DateTimeFormatProperties)this[format]).DateFormatPattern = newPattern;
                    else
                    {
                        DateTimeFormatProperties tempProps = new DateTimeFormatProperties();
                        tempProps.DateFormatPattern = newPattern;
                        Add(format, tempProps);
                    }
                }

                /// <summary>
                /// Gets the current format pattern of the DateTimeFormat instance
                /// </summary>
                /// <param name="format">The DateTimeFormat instance which the value will be obtained</param>
                /// <returns>The string representing the current datetimeformat pattern</returns>
                public System.String GetDateFormatPattern(System.Globalization.DateTimeFormatInfo format)
                {
                    if (this[format] == null)
                        return "d-MMM-yy";
                    else
                        return ((DateTimeFormatProperties)this[format]).DateFormatPattern;
                }

                /// <summary>
                /// Sets the datetimeformat pattern to the giving format
                /// </summary>
                /// <param name="format">The datetimeformat instance to set</param>
                /// <param name="newPattern">The new datetimeformat pattern</param>
                public void SetTimeFormatPattern(System.Globalization.DateTimeFormatInfo format, System.String newPattern)
                {
                    if (this[format] != null)
                        ((DateTimeFormatProperties)this[format]).TimeFormatPattern = newPattern;
                    else
                    {
                        DateTimeFormatProperties tempProps = new DateTimeFormatProperties();
                        tempProps.TimeFormatPattern = newPattern;
                        Add(format, tempProps);
                    }
                }

                /// <summary>
                /// Gets the current format pattern of the DateTimeFormat instance
                /// </summary>
                /// <param name="format">The DateTimeFormat instance which the value will be obtained</param>
                /// <returns>The string representing the current datetimeformat pattern</returns>
                public System.String GetTimeFormatPattern(System.Globalization.DateTimeFormatInfo format)
                {
                    if (this[format] == null)
                        return "h:mm:ss tt";
                    else
                        return ((DateTimeFormatProperties)this[format]).TimeFormatPattern;
                }

                /// <summary>
                /// Internal class to provides the DateFormat and TimeFormat pattern properties on .NET
                /// </summary>
                class DateTimeFormatProperties
                {
                    public System.String DateFormatPattern = "d-MMM-yy";
                    public System.String TimeFormatPattern = "h:mm:ss tt";
                }
            }
        }
        /*******************************/
        /// <summary>
        /// Gets the DateTimeFormat instance and date instance to obtain the date with the format passed
        /// </summary>
        /// <param name="format">The DateTimeFormat to obtain the time and date pattern</param>
        /// <param name="date">The date instance used to get the date</param>
        /// <returns>A string representing the date with the time and date patterns</returns>
        public static System.String FormatDateTime(System.Globalization.DateTimeFormatInfo format, System.DateTime date)
        {
            System.String timePattern = DateTimeFormatManager.manager.GetTimeFormatPattern(format);
            System.String datePattern = DateTimeFormatManager.manager.GetDateFormatPattern(format);
            return date.ToString(datePattern + " " + timePattern, format);
        }

        /*******************************/
        /// <summary>
        /// Converts the specified collection to its string representation.
        /// </summary>
        /// <param name="c">The collection to convert to string.</param>
        /// <returns>A string representation of the specified collection.</returns>
        public static System.String CollectionToString(System.Collections.ICollection c)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            if (c != null)
            {

                System.Collections.ArrayList l = new System.Collections.ArrayList(c);

                bool isDictionary = (c is System.Collections.BitArray || c is System.Collections.Hashtable || c is System.Collections.IDictionary || c is System.Collections.Specialized.NameValueCollection || (l.Count > 0 && l[0] is System.Collections.DictionaryEntry));
                for (int index = 0; index < l.Count; index++)
                {
                    if (l[index] == null)
                        s.Append("null");
                    else if (!isDictionary)
                        s.Append(l[index]);
                    else
                    {
                        isDictionary = true;
                        if (c is System.Collections.Specialized.NameValueCollection)
                            s.Append(((System.Collections.Specialized.NameValueCollection)c).GetKey(index));
                        else
                            s.Append(((System.Collections.DictionaryEntry)l[index]).Key);
                        s.Append("=");
                        if (c is System.Collections.Specialized.NameValueCollection)
                            s.Append(((System.Collections.Specialized.NameValueCollection)c).GetValues(index)[0]);
                        else
                            s.Append(((System.Collections.DictionaryEntry)l[index]).Value);

                    }
                    if (index < l.Count - 1)
                        s.Append(", ");
                }

                if (isDictionary)
                {
                    if (c is System.Collections.ArrayList)
                        isDictionary = false;
                }
                if (isDictionary)
                {
                    s.Insert(0, "{");
                    s.Append("}");
                }
                else
                {
                    s.Insert(0, "[");
                    s.Append("]");
                }
            }
            else
                s.Insert(0, "null");
            return s.ToString();
        }

        /// <summary>
        /// Tests if the specified object is a collection and converts it to its string representation.
        /// </summary>
        /// <param name="obj">The object to convert to string</param>
        /// <returns>A string representation of the specified object.</returns>
        public static System.String CollectionToString(System.Object obj)
        {
            System.String result = "";

            if (obj != null)
            {
                if (obj is System.Collections.ICollection)
                    result = CollectionToString((System.Collections.ICollection)obj);
                else
                    result = obj.ToString();
            }
            else
                result = "null";

            return result;
        }
        /*******************************/
        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static long Identity(long literal)
        {
            return literal;
        }

        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static ulong Identity(ulong literal)
        {
            return literal;
        }

        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static float Identity(float literal)
        {
            return literal;
        }

        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static double Identity(double literal)
        {
            return literal;
        }

        /*******************************/
        /// <summary>Reads a number of characters from the current source Stream and writes the data to the target array at the specified index.</summary>
        /// <param name="sourceStream">The source Stream to read from.</param>
        /// <param name="target">Contains the array of characteres read from the source Stream.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of characters to read from the source Stream.</param>
        /// <returns>The number of characters read. The number will be less than or equal to count depending on the data available in the source Stream. Returns -1 if the end of the stream is reached.</returns>
        public static System.Int32 ReadInput(System.IO.Stream sourceStream, sbyte[] target, int start, int count)
        {
            // Returns 0 bytes if not enough space in target
            if (target.Length == 0)
                return 0;

            byte[] receiver = new byte[target.Length];
            int bytesRead = sourceStream.Read(receiver, start, count);

            // Returns -1 if EOF
            if (bytesRead == 0)
                return -1;

            for (int i = start; i < start + bytesRead; i++)
                target[i] = (sbyte)receiver[i];

            return bytesRead;
        }

        /// <summary>Reads a number of characters from the current source TextReader and writes the data to the target array at the specified index.</summary>
        /// <param name="sourceTextReader">The source TextReader to read from</param>
        /// <param name="target">Contains the array of characteres read from the source TextReader.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of characters to read from the source TextReader.</param>
        /// <returns>The number of characters read. The number will be less than or equal to count depending on the data available in the source TextReader. Returns -1 if the end of the stream is reached.</returns>
        public static System.Int32 ReadInput(System.IO.TextReader sourceTextReader, sbyte[] target, int start, int count)
        {
            // Returns 0 bytes if not enough space in target
            if (target.Length == 0) return 0;

            char[] charArray = new char[target.Length];
            int bytesRead = sourceTextReader.Read(charArray, start, count);

            // Returns -1 if EOF
            if (bytesRead == 0) return -1;

            for (int index = start; index < start + bytesRead; index++)
                target[index] = (sbyte)charArray[index];

            return bytesRead;
        }

        /*******************************/
        /// <summary>
        /// Provides support for BufferdInputStream
        /// </summary>
        public class BufferedStreamManager
        {
            static public BufferedStreamsHashTable manager = new BufferedStreamsHashTable();

            /// <summary>
            /// A hastable to store and keep the tracks
            /// </summary>		
            public class BufferedStreamsHashTable : System.Collections.Hashtable
            {
                /// <summary>
                /// Reset the point of read to the marked position
                /// </summary>
                /// <param name="stream">The instance of InputStream</param>
                /// <returns>The current mark position</returns>
                public long ResetMark(System.IO.Stream stream)
                {
                    if (this[stream] != null)
                        return ((StreamProperties)this[stream]).markposition;
                    else
                        return stream.Position;
                }

                /// <summary>
                /// Marks a position into the stream
                /// </summary>
                /// <param name="index">The position that will be marked</param>
                /// <param name="stream">The stream to mark</param>
                public void MarkPosition(int index, System.IO.Stream stream)
                {
                    if (this[stream] == null)
                    {
                        StreamProperties tempProps = new StreamProperties();
                        tempProps.marklimit = index;
                        tempProps.markposition = stream.Position;
                        Add(stream, tempProps);
                    }
                    else
                    {
                        ((StreamProperties)this[stream]).marklimit = index;
                        ((StreamProperties)this[stream]).markposition = stream.Position;
                    }
                }

                /// <summary>
                /// Inner class. Used to have the properties of mark on .NET
                /// </summary>
                class StreamProperties
                {
                    public long markposition = -1;
                    public long marklimit;
                }
            }
        }
        /*******************************/
        /// <summary>
        /// Converts an angle in radians to degrees.
        /// </summary>
        /// <param name="angleInRadians">Double value of angle in radians to convert.</param>
        /// <returns>The value of the angle in degrees.</returns>
        public static double RadiansToDegrees(double angleInRadians)
        {
            double valueDegrees = 360 / (2 * System.Math.PI);
            return angleInRadians * valueDegrees;
        }

        /*******************************/
        /// <summary>
        /// Converts an angle in degrees to radians.
        /// </summary>
        /// <param name="angleInDegrees">Double value of angle in degrees to convert.</param>
        /// <returns>The value of the angle in radians.</returns>
        public static double DegreesToRadians(double angleInDegrees)
        {
            double valueRadians = (2 * System.Math.PI) / 360;
            return angleInDegrees * valueRadians;
        }

        /*******************************/
        //Provides access to a static System.Random class instance
        static public System.Random Random = new System.Random();

        /*******************************/
        /// <summary>
        /// This class contains support methods to work with GraphicsPath and Lines.
        /// </summary>
        public class Line2DSupport
        {
            /// <summary>
            /// Creates a GraphicsPath object and adds a line to it.
            /// </summary>
            /// <param name="x1">The x-coordinate of the starting point of the line.</param>
            /// <param name="y1">The y-coordinate of the starting point of the line.</param>
            /// <param name="x2">The x-coordinate of the endpoint of the line.</param>
            /// <param name="y2">The y-coordinate of the endpoint of the line.</param>
            /// <returns>Returns a GraphicsPath object containing the line.</returns>
            public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(float x1, float y1, float x2, float y2)
            {
                System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
                linePath.AddLine(x1, y1, x2, y2);
                return linePath;
            }

            /// <summary>
            /// Creates a GraphicsPath object and adds a line to it.
            /// </summary>
            /// <param name="p1">The starting point of the line.</param>
            /// <param name="p2">The endpoint of the line.</param>
            /// <returns>Returns a GraphicsPath object containing the line</returns>
            public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(System.Drawing.PointF p1, System.Drawing.PointF p2)
            {
                System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
                linePath.AddLine(p1, p2);
                return linePath;
            }

            /// <summary>
            /// Resets the specified GraphicsPath object an adds a line to it with the specified values.
            /// </summary>
            /// <param name="linePath">The GraphicsPath object to reset.</param>
            /// <param name="x1">The x-coordinate of the starting point of the line.</param>
            /// <param name="y1">The y-coordinate of the starting point of the line.</param>
            /// <param name="x2">The x-coordinate of the endpoint of the line.</param>
            /// <param name="y2">The y-coordinate of the endpoint of the line.</param>
            public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, float x1, float y1, float x2, float y2)
            {
                linePath.Reset();
                linePath.AddLine(x1, y1, x2, y2);
            }

            /// <summary>
            /// Resets the specified GraphicsPath object an adds a line to it with the specified values.
            /// </summary>
            /// <param name="linePath">The GraphicsPath object to reset.</param>
            /// <param name="p1">The starting point of the line.</param>
            /// <param name="p2">The endpoint of the line.</param>
            public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.PointF p1, System.Drawing.PointF p2)
            {
                linePath.Reset();
                linePath.AddLine(p1, p2);
            }

            /// <summary>
            /// Resets the specified GraphicsPath object an adds a line to it.
            /// </summary>
            /// <param name="linePath">The GraphicsPath object to reset.</param>
            /// <param name="newLinePath">The line to add.</param>
            public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.Drawing2D.GraphicsPath newLinePath)
            {
                linePath.Reset();
                linePath.AddPath(newLinePath, false);
            }
        }


        /*******************************/
        /// <summary>
        /// Executes the specified command and arguments in a separate process.
        /// </summary>
        /// <param name="cmd">Array containing the command to call and its arguments.</param>
        /// <returns>System.Diagnostics.Process instance for managing the subprocess.</returns>
        public static System.Diagnostics.Process ExecSupport(System.String[] cmd)
        {
            System.Diagnostics.Process returnVal;
            switch (cmd.Length)
            {
                case 0:
                    System.Diagnostics.Process.GetCurrentProcess();
                    returnVal = System.Diagnostics.Process.Start("");
                    break;
                case 1:
                    System.Diagnostics.Process.GetCurrentProcess();
                    returnVal = System.Diagnostics.Process.Start(cmd[0]);
                    break;
                default:
                    System.String temp = "";
                    for (int i = 1; i < cmd.Length; i++)
                        if (i == 1)
                            temp = cmd[i];
                        else
                            temp = temp + " " + cmd[i];
                    System.Diagnostics.Process.GetCurrentProcess();
                    returnVal = System.Diagnostics.Process.Start(cmd[0], temp);
                    break;
            }
            return returnVal;
        }


        /*******************************/
        /// <summary>
        /// Creates a new positive random number 
        /// </summary>
        /// <param name="random">The last random obtained</param>
        /// <returns>Returns a new positive random number</returns>
        public static long NextLong(System.Random random)
        {
            long temporaryLong = random.Next();
            temporaryLong = (temporaryLong << 32) + random.Next();
            if (random.Next(-1, 1) < 0)
                return -temporaryLong;
            else
                return temporaryLong;
        }
        /*******************************/
        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static int URShift(int number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2 << ~bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static int URShift(int number, long bits)
        {
            return URShift(number, (int)bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static long URShift(long number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2L << ~bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static long URShift(long number, long bits)
        {
            return URShift(number, (int)bits);
        }

        /*******************************/
        /// <summary>
        /// Shows a dialog object.
        /// </summary>
        /// <param name="dialog">Dialog to be shown.</param>
        /// <param name="visible">Indicates if the dialog should be shown.</param>
        public static void ShowDialog(System.Windows.Forms.FileDialog dialog, bool visible)
        {
            if (visible)
                dialog.ShowDialog();
        }


        /*******************************/
        /// <summary>
        /// Calculates the ascent of the font, using the GetCellAscent and GetEmHeight methods
        /// </summary>
        /// <param name="font">The Font instance used to obtain the Ascent</param>
        /// <returns>The ascent of the font</returns>
        public static int GetAscent(System.Drawing.Font font)
        {
            System.Drawing.FontFamily fontFamily = font.FontFamily;
            int ascent = fontFamily.GetCellAscent(font.Style);
            int ascentPixel = (int)font.Size * ascent / fontFamily.GetEmHeight(font.Style);
            return ascentPixel;
        }

        /*******************************/
        /// <summary>
        /// This class contains support methods to work with GraphicsPath and Arcs.
        /// </summary>
        public class Arc2DSupport
        {
            /// <summary>
            /// Specifies an OPEN arc type.
            /// </summary>
            public const int OPEN = 0;
            /// <summary>
            /// Specifies an CLOSED arc type.
            /// </summary>
            public const int CLOSED = 1;
            /// <summary>
            /// Specifies an PIE arc type.
            /// </summary>
            public const int PIE = 2;
            /// <summary>
            /// Creates a GraphicsPath object and adds an arc to it with the specified arc values and closure type.
            /// </summary>
            /// <param name="x">The x coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="y">The y coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="start">The starting angle of the arc measured in degrees.</param>
            /// <param name="extent">The angular extent of the arc measured in degrees.</param>
            /// <param name="arcType">The closure type for the arc.</param>
            /// <returns>Returns a new GraphicsPath object that contains the arc path.</returns>
            public static System.Drawing.Drawing2D.GraphicsPath CreateArc2D(float x, float y, float height, float width, float start, float extent, int arcType)
            {
                System.Drawing.Drawing2D.GraphicsPath arc2DPath = new System.Drawing.Drawing2D.GraphicsPath();
                switch (arcType)
                {
                    case OPEN:
                        arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
                        break;
                    case CLOSED:
                        arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
                        arc2DPath.CloseFigure();
                        break;
                    case PIE:
                        arc2DPath.AddPie(x, y, height, width, start * -1, extent * -1);
                        break;
                    default:
                        break;
                }
                return arc2DPath;
            }
            /// <summary>
            /// Creates a GraphicsPath object and adds an arc to it with the specified arc values and closure type.
            /// </summary>
            /// <param name="ellipseBounds">A RectangleF structure that represents the rectangular bounds of the ellipse from which the arc is taken.</param>
            /// <param name="start">The starting angle of the arc measured in degrees.</param>
            /// <param name="extent">The angular extent of the arc measured in degrees.</param>
            /// <param name="arcType">The closure type for the arc.</param>
            /// <returns>Returns a new GraphicsPath object that contains the arc path.</returns>
            public static System.Drawing.Drawing2D.GraphicsPath CreateArc2D(System.Drawing.RectangleF ellipseBounds, float start, float extent, int arcType)
            {
                System.Drawing.Drawing2D.GraphicsPath arc2DPath = new System.Drawing.Drawing2D.GraphicsPath();
                switch (arcType)
                {
                    case OPEN:
                        arc2DPath.AddArc(ellipseBounds, start * -1, extent * -1);
                        break;
                    case CLOSED:
                        arc2DPath.AddArc(ellipseBounds, start * -1, extent * -1);
                        arc2DPath.CloseFigure();
                        break;
                    case PIE:
                        arc2DPath.AddPie(ellipseBounds.X, ellipseBounds.Y, ellipseBounds.Width, ellipseBounds.Height, start * -1, extent * -1);
                        break;
                    default:
                        break;
                }

                return arc2DPath;
            }

            /// <summary>
            /// Resets the specified GraphicsPath object and adds an arc to it with the speficied values.
            /// </summary>
            /// <param name="arc2DPath">The GraphicsPath object to reset.</param>
            /// <param name="x">The x coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="y">The y coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn.</param>
            /// <param name="start">The starting angle of the arc measured in degrees.</param>
            /// <param name="extent">The angular extent of the arc measured in degrees.</param>
            /// <param name="arcType">The closure type for the arc.</param>
            public static void SetArc(System.Drawing.Drawing2D.GraphicsPath arc2DPath, float x, float y, float height, float width, float start, float extent, int arcType)
            {
                arc2DPath.Reset();
                switch (arcType)
                {
                    case OPEN:
                        arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
                        break;
                    case CLOSED:
                        arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
                        arc2DPath.CloseFigure();
                        break;
                    case PIE:
                        arc2DPath.AddPie(x, y, height, width, start * -1, extent * -1);
                        break;
                    default:
                        break;
                }
            }
        }
        /*******************************/
        /// <summary>
        /// Gets all X-axis points from the received graphics path
        /// </summary>
        /// <param name="path">Source graphics path</param>
        /// <returns>The array of X-axis values</returns>
        public static int[] GetXPoints(System.Drawing.Drawing2D.GraphicsPath path)
        {
            int[] tempIntArray = new int[path.PointCount];
            for (int index = 0; index < path.PointCount; index++)
            {
                tempIntArray[index] = (int)path.PathPoints[index].X;
            }
            return tempIntArray;
        }

        /*******************************/
        /// <summary>
        /// Gets all Y-axis points from the received graphics path
        /// </summary>
        /// <param name="path">Source graphics path</param>
        /// <returns>The array of Y-axis values</returns>
        public static int[] GetYPoints(System.Drawing.Drawing2D.GraphicsPath path)
        {
            int[] tempIntArray = new int[path.PointCount];
            for (int index = 0; index < path.PointCount; index++)
            {
                tempIntArray[index] = (int)path.PathPoints[index].Y;
            }
            return tempIntArray;
        }

        /*******************************/
        /// <summary>
        /// Support class used to handle threads
        /// </summary>
        public class ThreadClass : IThreadRunnable
        {
            /// <summary>
            /// The instance of System.Threading.Thread
            /// </summary>
            private System.Threading.Thread threadField;

            /// <summary>
            /// Initializes a new instance of the ThreadClass class
            /// </summary>
            public ThreadClass()
            {
                threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            }

            /// <summary>
            /// Initializes a new instance of the Thread class.
            /// </summary>
            /// <param name="Name">The name of the thread</param>
            public ThreadClass(System.String Name)
            {
                threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                this.Name = Name;
            }

            /// <summary>
            /// Initializes a new instance of the Thread class.
            /// </summary>
            /// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
            public ThreadClass(System.Threading.ThreadStart Start)
            {
                threadField = new System.Threading.Thread(Start);
            }

            /// <summary>
            /// Initializes a new instance of the Thread class.
            /// </summary>
            /// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
            /// <param name="Name">The name of the thread</param>
            public ThreadClass(System.Threading.ThreadStart Start, System.String Name)
            {
                threadField = new System.Threading.Thread(Start);
                this.Name = Name;
            }

            /// <summary>
            /// This method has no functionality unless the method is overridden
            /// </summary>
            public virtual void Run()
            {
            }

            /// <summary>
            /// Causes the operating system to change the state of the current thread instance to ThreadState.Running
            /// </summary>
            public virtual void Start()
            {
                threadField.Start();
            }

            /// <summary>
            /// Interrupts a thread that is in the WaitSleepJoin thread state
            /// </summary>
            public virtual void Interrupt()
            {
                threadField.Interrupt();
            }

            /// <summary>
            /// Gets the current thread instance
            /// </summary>
            public System.Threading.Thread Instance
            {
                get
                {
                    return threadField;
                }
                set
                {
                    threadField = value;
                }
            }

            /// <summary>
            /// Gets or sets the name of the thread
            /// </summary>
            public System.String Name
            {
                get
                {
                    return threadField.Name;
                }
                set
                {
                    if (threadField.Name == null)
                        threadField.Name = value;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating the scheduling priority of a thread
            /// </summary>
            public System.Threading.ThreadPriority Priority
            {
                get
                {
                    return threadField.Priority;
                }
                set
                {
                    threadField.Priority = value;
                }
            }

            /// <summary>
            /// Gets a value indicating the execution status of the current thread
            /// </summary>
            public bool IsAlive
            {
                get
                {
                    return threadField.IsAlive;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether or not a thread is a background thread.
            /// </summary>
            public bool IsBackground
            {
                get
                {
                    return threadField.IsBackground;
                }
                set
                {
                    threadField.IsBackground = value;
                }
            }

            /// <summary>
            /// Blocks the calling thread until a thread terminates
            /// </summary>
            public void Join()
            {
                threadField.Join();
            }

            /// <summary>
            /// Blocks the calling thread until a thread terminates or the specified time elapses
            /// </summary>
            /// <param name="MiliSeconds">Time of wait in milliseconds</param>
            public void Join(long MiliSeconds)
            {
                lock (this)
                {
                    threadField.Join(new System.TimeSpan(MiliSeconds * 10000));
                }
            }

            /// <summary>
            /// Blocks the calling thread until a thread terminates or the specified time elapses
            /// </summary>
            /// <param name="MiliSeconds">Time of wait in milliseconds</param>
            /// <param name="NanoSeconds">Time of wait in nanoseconds</param>
            public void Join(long MiliSeconds, int NanoSeconds)
            {
                lock (this)
                {
                    threadField.Join(new System.TimeSpan(MiliSeconds * 10000 + NanoSeconds * 100));
                }
            }

            /// <summary>
            /// Resumes a thread that has been suspended
            /// </summary>
            public void Resume()
            {
                threadField.Resume();
            }

            /// <summary>
            /// Raises a ThreadAbortException in the thread on which it is invoked, 
            /// to begin the process of terminating the thread. Calling this method 
            /// usually terminates the thread
            /// </summary>
            public void Abort()
            {
                threadField.Abort();
            }

            /// <summary>
            /// Raises a ThreadAbortException in the thread on which it is invoked, 
            /// to begin the process of terminating the thread while also providing
            /// exception information about the thread termination. 
            /// Calling this method usually terminates the thread.
            /// </summary>
            /// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted</param>
            public void Abort(System.Object stateInfo)
            {
                lock (this)
                {
                    threadField.Abort(stateInfo);
                }
            }

            /// <summary>
            /// Suspends the thread, if the thread is already suspended it has no effect
            /// </summary>
            public void Suspend()
            {
                threadField.Suspend();
            }

            /// <summary>
            /// Obtain a String that represents the current Object
            /// </summary>
            /// <returns>A String that represents the current Object</returns>
            public override System.String ToString()
            {
                return "Thread[" + Name + "," + Priority.ToString() + "," + "" + "]";
            }

            /// <summary>
            /// Gets the currently running thread
            /// </summary>
            /// <returns>The currently running thread</returns>
            public static ThreadClass Current()
            {
                ThreadClass CurrentThread = new ThreadClass();
                CurrentThread.Instance = System.Threading.Thread.CurrentThread;
                return CurrentThread;
            }
        }


        /*******************************/
        /// <summary>
        /// Obtains the int value depending of the type of modifiers that the constructor have
        /// </summary>
        /// <param name="constructor">The ConstructorInfo used to obtain the int value</param>
        /// <returns>The int value of the modifier present in the constructor. 1 if it's public, 2 if it's private, otherwise 4</returns>
        public static int GetConstructorModifiers(System.Reflection.ConstructorInfo constructor)
        {
            int temp;
            if (constructor.IsPublic)
                temp = 1;
            else if (constructor.IsPrivate)
                temp = 2;
            else
                temp = 4;
            return temp;
        }

        /*******************************/
        /// <summary>
        /// This class manages array operations.
        /// </summary>
        public class ArraySupport
        {
            /// <summary>
            /// Compares the entire members of one array whith the other one.
            /// </summary>
            /// <param name="array1">The array to be compared.</param>
            /// <param name="array2">The array to be compared with.</param>
            /// <returns>True if both arrays are equals otherwise it returns false.</returns>
            /// <remarks>Two arrays are equal if they contains the same elements in the same order.</remarks>
            public static bool Equals(System.Array array1, System.Array array2)
            {
                bool result = false;
                if ((array1 == null) && (array2 == null))
                    result = true;
                else if ((array1 != null) && (array2 != null))
                {
                    if (array1.Length == array2.Length)
                    {
                        int length = array1.Length;
                        result = true;
                        for (int index = 0; index < length; index++)
                        {
                            if (!(array1.GetValue(index).Equals(array2.GetValue(index))))
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
                return result;
            }

            /// <summary>
            /// Fills the array with an specific value from an specific index to an specific index.
            /// </summary>
            /// <param name="array">The array to be filled.</param>
            /// <param name="fromindex">The first index to be filled.</param>
            /// <param name="toindex">The last index to be filled.</param>
            /// <param name="val">The value to fill the array with.</param>
            public static void Fill(System.Array array, System.Int32 fromindex, System.Int32 toindex, System.Object val)
            {
                System.Object Temp_Object = val;
                System.Type elementtype = array.GetType().GetElementType();
                if (elementtype != val.GetType())
                    Temp_Object = System.Convert.ChangeType(val, elementtype);
                if (array.Length == 0)
                    throw (new System.NullReferenceException());
                if (fromindex > toindex)
                    throw (new System.ArgumentException());
                if ((fromindex < 0) || ((System.Array)array).Length < toindex)
                    throw (new System.IndexOutOfRangeException());
                for (int index = (fromindex > 0) ? fromindex-- : fromindex; index < toindex; index++)
                    array.SetValue(Temp_Object, index);
            }

            /// <summary>
            /// Fills the array with an specific value.
            /// </summary>
            /// <param name="array">The array to be filled.</param>
            /// <param name="val">The value to fill the array with.</param>
            public static void Fill(System.Array array, System.Object val)
            {
                Fill(array, 0, array.Length, val);
            }
        }


        /*******************************/
        /// <summary>
        /// Sets the capacity for the specified ArrayList
        /// </summary>
        /// <param name="vector">The ArrayList which capacity will be set</param>
        /// <param name="newCapacity">The new capacity value</param>
        public static void SetCapacity(System.Collections.ArrayList vector, int newCapacity)
        {
            if (newCapacity > vector.Count)
                vector.AddRange(new Array[newCapacity - vector.Count]);
            else if (newCapacity < vector.Count)
                vector.RemoveRange(newCapacity, vector.Count - newCapacity);
            vector.Capacity = newCapacity;
        }



        /*******************************/
        /// <summary>
        /// SupportClass for the SortedSet interface.
        /// </summary>
        public interface SortedSetSupport : CSGraphT.SupportClass.SetSupport
        {
            /// <summary>
            /// Returns a portion of the list whose elements are less than the limit object parameter.
            /// </summary>
            /// <param name="l">The list where the portion will be extracted.</param>
            /// <param name="limit">The end element of the portion to extract.</param>
            /// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
            SortedSetSupport HeadSet(System.Object limit);

            /// <summary>
            /// Returns a portion of the list whose elements are greater that the lowerLimit parameter less than the upperLimit parameter.
            /// </summary>
            /// <param name="l">The list where the portion will be extracted.</param>
            /// <param name="limit">The start element of the portion to extract.</param>
            /// <param name="limit">The end element of the portion to extract.</param>
            /// <returns>The portion of the collection.</returns>
            SortedSetSupport SubSet(System.Object lowerLimit, System.Object upperLimit);

            /// <summary>
            /// Returns a portion of the list whose elements are greater than the limit object parameter.
            /// </summary>
            /// <param name="l">The list where the portion will be extracted.</param>
            /// <param name="limit">The start element of the portion to extract.</param>
            /// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
            SortedSetSupport TailSet(System.Object limit);
        }


        /*******************************/
        /// <summary>
        /// SupportClass for the TreeSet class.
        /// </summary>
        [Serializable]
        public class TreeSetSupport : System.Collections.ArrayList, CSGraphT.SupportClass.SetSupport, SortedSetSupport
        {
            private System.Collections.IComparer comparator = System.Collections.Comparer.Default;

            public TreeSetSupport()
                : base()
            {
            }

            public TreeSetSupport(System.Collections.ICollection c)
                : base()
            {
                this.AddAll(c);
            }

            public TreeSetSupport(System.Collections.IComparer c)
                : base()
            {
                this.comparator = c;
            }

            /// <summary>
            /// Gets the IComparator object used to sort this set.
            /// </summary>
            public System.Collections.IComparer Comparator
            {
                get
                {
                    return this.comparator;
                }
            }

            /// <summary>
            /// Adds a new element to the ArrayList if it is not already present and sorts the ArrayList.
            /// </summary>
            /// <param name="obj">Element to insert to the ArrayList.</param>
            /// <returns>TRUE if the new element was inserted, FALSE otherwise.</returns>
            new public bool Add(System.Object obj)
            {
                bool inserted;
                if ((inserted = this.Contains(obj)) == false)
                {
                    base.Add(obj);
                    this.Sort(this.comparator);
                }
                return !inserted;
            }

            /// <summary>
            /// Adds all the elements of the specified collection that are not present to the list.
            /// </summary>		
            /// <param name="c">Collection where the new elements will be added</param>
            /// <returns>Returns true if at least one element was added to the collection.</returns>
            public bool AddAll(System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
                bool added = false;
                while (e.MoveNext() == true)
                {
                    if (this.Add(e.Current) == true)
                        added = true;
                }
                this.Sort(this.comparator);
                return added;
            }

            /// <summary>
            /// Determines whether an element is in the the current TreeSetSupport collection. The IComparer defined for 
            /// the current set will be used to make comparisons between the elements already inserted in the collection and 
            /// the item specified.
            /// </summary>
            /// <param name="item">The object to be locatet in the current collection.</param>
            /// <returns>true if item is found in the collection; otherwise, false.</returns>
            public override bool Contains(System.Object item)
            {
                System.Collections.IEnumerator tempEnumerator = this.GetEnumerator();
                while (tempEnumerator.MoveNext())
                    if (this.comparator.Compare(tempEnumerator.Current, item) == 0)
                        return true;
                return false;
            }

            /// <summary>
            /// Returns a portion of the list whose elements are less than the limit object parameter.
            /// </summary>
            /// <param name="limit">The end element of the portion to extract.</param>
            /// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
            public SortedSetSupport HeadSet(System.Object limit)
            {
                SortedSetSupport newList = new TreeSetSupport();
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.comparator.Compare(this[i], limit) >= 0)
                        break;
                    newList.Add(this[i]);
                }
                return newList;
            }

            /// <summary>
            /// Returns a portion of the list whose elements are greater that the lowerLimit parameter less than the upperLimit parameter.
            /// </summary>
            /// <param name="lowerLimit">The start element of the portion to extract.</param>
            /// <param name="upperLimit">The end element of the portion to extract.</param>
            /// <returns>The portion of the collection.</returns>
            public SortedSetSupport SubSet(System.Object lowerLimit, System.Object upperLimit)
            {
                SortedSetSupport newList = new TreeSetSupport();
                int i = 0;
                while (this.comparator.Compare(this[i], lowerLimit) < 0)
                    i++;
                for (; i < this.Count; i++)
                {
                    if (this.comparator.Compare(this[i], upperLimit) >= 0)
                        break;
                    newList.Add(this[i]);
                }
                return newList;
            }

            /// <summary>
            /// Returns a portion of the list whose elements are greater than the limit object parameter.
            /// </summary>
            /// <param name="limit">The start element of the portion to extract.</param>
            /// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
            public SortedSetSupport TailSet(System.Object limit)
            {
                SortedSetSupport newList = new TreeSetSupport();
                int i = 0;
                while (this.comparator.Compare(this[i], limit) < 0)
                    i++;
                for (; i < this.Count; i++)
                    newList.Add(this[i]);
                return newList;
            }

            #region SetSupport Members


            public bool AddItem(object obj)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }


    }
}