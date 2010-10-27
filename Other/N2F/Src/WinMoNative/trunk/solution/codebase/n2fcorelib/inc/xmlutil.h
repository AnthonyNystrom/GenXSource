#pragma once

#include <msxml2.h>

class XmlUtil
{

public:
	static N2FCORE_API CString BSTRToCString(BSTR bstrString);
	static N2FCORE_API CComBSTR CStringToBSTR(CString& csString);

	static N2FCORE_API bool GetNodeName( IXMLDOMNode* node, CString& name );
	static N2FCORE_API bool GetNodeValue( IXMLDOMNode* node, CString& value );
	static N2FCORE_API bool GetNodeText( IXMLDOMNode* node, CString& text );
	static N2FCORE_API bool GetNodeByName( IXMLDOMNode* parentNode, CString& name, CComPtr<IXMLDOMNode>& node );

	static N2FCORE_API bool SetNodeValue( IXMLDOMNode* node, CString& value );
};