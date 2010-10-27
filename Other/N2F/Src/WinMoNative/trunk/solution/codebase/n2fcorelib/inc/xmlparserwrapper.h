#pragma once

#include <msxml2.h>

#include <xmlelementparser.h>

class XmlParserWrapper 
{
public:

	N2FCORE_API XmlParserWrapper();

	N2FCORE_API virtual ~XmlParserWrapper();

	N2FCORE_API bool Initialize();

	N2FCORE_API bool ParseXmlFile(CString& filePath, XmlElementParser *elementParser);

	

private:
	
	CComPtr<IXMLDOMDocument> iXmlDoc;
};
