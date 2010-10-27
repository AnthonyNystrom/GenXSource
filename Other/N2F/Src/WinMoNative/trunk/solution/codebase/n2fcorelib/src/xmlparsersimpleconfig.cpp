#include "stdafx.h"

#include <xmlparsersimpleconfig.h>
#include <xmlutil.h>


N2FCORE_API XmlParserSimpleConfig::XmlParserSimpleConfig( )
		:iIDAttrName("id"),
		iNameAttrName("name")
{
	
}

N2FCORE_API XmlParserSimpleConfig::XmlParserSimpleConfig( TSimpleConfigItemType itemsType,
												CString& desiredParentNodeName,
												CString& desiredChildNodeName )
		:iItemsType(itemsType),
		iIDAttrName("id"),
		iNameAttrName("name"),
		iDesiredChildNodeName(desiredChildNodeName),
		iDesiredParentNodeName(desiredParentNodeName)
{
	
}


N2FCORE_API XmlParserSimpleConfig::~XmlParserSimpleConfig()
{

}

N2FCORE_API XmlParserSimpleConfig* XmlParserSimpleConfig::CreateStringConfigParser()
{
	XmlParserSimpleConfig *result = new XmlParserSimpleConfig( ESCTString, 
		CString("strings"), CString("string") );

	return result;
}

N2FCORE_API XmlParserSimpleConfig* XmlParserSimpleConfig::CreateGraphicsConfigParser()
{
	XmlParserSimpleConfig *result = new XmlParserSimpleConfig( ESCTGraphics, 
		CString("graphics"), CString("image") );

	return result;
}

N2FCORE_API XmlParserSimpleConfig* XmlParserSimpleConfig::CreateColorConfigParser()
{
	XmlParserSimpleConfig *result = new XmlParserSimpleConfig( ESCTColor,
		CString("colors"), CString("color") );

	return result;
}

N2FCORE_API  bool XmlParserSimpleConfig::ParseNode( IXMLDOMNode *domNode )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	if ( NULL == domNode )
		return result;

	long id;
	CString name;
	if  ( false == this->GetKnownAttributesForNode(domNode, id, name) )
	{
		return result;
	}

	CString value;

	if ( false == XmlUtil::GetNodeText( domNode, value ) )
	{
		return result;
	}
	
	
	SimpleConfigItem *item = new SimpleConfigItem;

	item->type = iItemsType;
	item->id = id;
	item->name = name;
	item->value = value;

	iResultsList.Add(item);

	result = (hr == S_OK);
	return result;
}

N2FCORE_API  bool XmlParserSimpleConfig::IsParentNodeDesired( IXMLDOMNode *domNode )
{
	CString name;
	if ( false == XmlUtil::GetNodeName(domNode, name) )
		return false;

	return ( 0 == name.CompareNoCase(iDesiredParentNodeName) );
}

N2FCORE_API  bool XmlParserSimpleConfig::IsChildNodeDesired( IXMLDOMNode *domNode )
{
	CString name;
	if ( false == XmlUtil::GetNodeName(domNode, name) )
		return false;

	return ( 0 == name.CompareNoCase(iDesiredChildNodeName) );
}

N2FCORE_API  bool XmlParserSimpleConfig::GetKnownAttributesForNode( IXMLDOMNode *node, long& id, CString& name )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	id = 0;
	name.Empty();

	if ( NULL == node )
		return result;

	CComPtr<IXMLDOMNamedNodeMap> listAttributes;
	hr = node->get_attributes( &listAttributes );
	if ( FAILED(hr) || NULL == listAttributes )
	{
		LOGMSG("Failed to resolve attributes list for specified node");
		return result;
	}

	long cntAttributes = 0;
	hr = listAttributes->get_length( &cntAttributes );
	if ( FAILED(hr) || cntAttributes < 2 )
	{
		LOGMSG("Failed to resolve attributes list count");
		return result;
	}

	BSTR valueID = NULL, valueName = NULL;
	IXMLDOMNode *nodeID = NULL, *nodeName = NULL;

	hr = listAttributes->getNamedItem(XmlUtil::CStringToBSTR(iIDAttrName), &nodeID);
	if ( FAILED(hr) || NULL == nodeID )
	{
		LOGMSG("Failed to find id attribute");
		return result;
	}

	CString csID;
	if ( false == XmlUtil::GetNodeValue(nodeID, csID) )
	{
		return result;
	}

	id = _ttol(csID);

	hr = listAttributes->getNamedItem(XmlUtil::CStringToBSTR(iNameAttrName), &nodeName);
	if ( FAILED(hr) || NULL == nodeName )
	{
		LOGMSG("Failed to find name attribute");
		return result;
	}

	CString csName;
	if ( false == XmlUtil::GetNodeValue(nodeName, csName) )
	{
		return result;
	}
	name = csName;

	result = ( S_OK == hr );
	return result;
}

N2FCORE_API SimpleConfigItemsList& XmlParserSimpleConfig::GetResultsList()
{
	return iResultsList;
}

