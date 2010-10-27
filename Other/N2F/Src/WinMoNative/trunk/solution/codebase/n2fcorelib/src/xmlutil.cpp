#include "stdafx.h"

#include <msxml2.h>
#include <xmlutil.h>


N2FCORE_API CString XmlUtil::BSTRToCString( BSTR bstrString )
{
	CString result;
	if ( NULL != bstrString )
		result = CString(bstrString);

	return result;
}
N2FCORE_API CComBSTR XmlUtil::CStringToBSTR( CString& csString )
{
	CComBSTR result(csString);

	return result;
}

N2FCORE_API bool XmlUtil::GetNodeName( IXMLDOMNode* node, CString& name )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	name.Empty();

	if ( NULL == node )
		return result;

	BSTR bstrName = NULL;
	hr = node->get_nodeName( &bstrName );

	name = XmlUtil::BSTRToCString(bstrName);

	result = (S_OK == hr);
	return result;
}

N2FCORE_API bool XmlUtil::GetNodeValue( IXMLDOMNode* node, CString& value )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	value.Empty();

	if ( NULL == node )
		return result;

	CComVariant varValue;
	varValue.InternalClear();

	hr = node->get_nodeValue( &varValue );

	value = XmlUtil::BSTRToCString(varValue.bstrVal);

	result = (S_OK == hr);
	return result;
}

N2FCORE_API bool XmlUtil::GetNodeText( IXMLDOMNode* node, CString& text )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	text.Empty();

	if ( NULL == node )
		return result;

	BSTR bstrText = NULL;

	hr = node->get_text( &bstrText );

	text = XmlUtil::BSTRToCString(bstrText);

	result = (S_OK == hr);
	return result;
}

N2FCORE_API bool XmlUtil::SetNodeValue( IXMLDOMNode* node, CString& value )
{
	bool result = false;

	if ( NULL == node )
		return result;

	HRESULT hr = E_FAIL;
	CComBSTR bstrValue(value);
	hr = node->put_text( bstrValue );

	result = (S_OK == hr);
	return result;
}

N2FCORE_API bool XmlUtil::GetNodeByName( IXMLDOMNode* parentNode, CString& name, CComPtr<IXMLDOMNode>& node )
{
	bool result = false;

	node = NULL;

	if ( NULL == parentNode  )
		return result;

	CComPtr<IXMLDOMNode>	tempNode;
	HRESULT hr = E_FAIL;

	hr = parentNode->get_firstChild( &tempNode );
	while ( SUCCEEDED(hr) && NULL != tempNode )
	{
		CString tempNodeName;
		XmlUtil::GetNodeName( tempNode, tempNodeName );

		if ( name == tempNodeName )
		{
			node = tempNode;
			result = true;
			break;
		}

		CComPtr<IXMLDOMNode> nextNode;
		hr = tempNode->get_nextSibling( &nextNode );
		tempNode = NULL;
		tempNode = nextNode;
	}

	return result;
}
