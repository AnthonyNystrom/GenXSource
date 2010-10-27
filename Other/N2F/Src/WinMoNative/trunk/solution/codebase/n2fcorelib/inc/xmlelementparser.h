#pragma once

#include <msxml2.h>

class XmlParserWrapper;

class XmlElementParser
{
public:

	N2FCORE_API XmlElementParser();
	N2FCORE_API virtual ~XmlElementParser();

	N2FCORE_API virtual bool SaveElement( CString& fileName );

protected:

	N2FCORE_API virtual void AssignDOMElement( IXMLDOMElement *element );
	N2FCORE_API virtual bool Parse();
	N2FCORE_API virtual bool ParseNode( IXMLDOMNode *domNode );
	N2FCORE_API virtual bool ParseChildrenForNode( IXMLDOMNode *domNode );

	N2FCORE_API virtual bool IsParentNodeDesired( IXMLDOMNode *domNode ) = 0;
	N2FCORE_API virtual bool IsChildNodeDesired( IXMLDOMNode *domNode ) = 0;

	CComPtr<IXMLDOMElement>	iDomElement;

	friend XmlParserWrapper;
};
