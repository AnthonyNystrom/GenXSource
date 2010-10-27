#pragma once

#include <msxml2.h>

#include <xmlelementparser.h>
#include <configuration-data.h>


class XmlParserSimpleConfig:
	public XmlElementParser
{
public:
	N2FCORE_API XmlParserSimpleConfig();
	N2FCORE_API virtual ~XmlParserSimpleConfig();

	static N2FCORE_API XmlParserSimpleConfig*	CreateStringConfigParser();
	static N2FCORE_API XmlParserSimpleConfig*	CreateGraphicsConfigParser();
	static N2FCORE_API XmlParserSimpleConfig*	CreateColorConfigParser();


	N2FCORE_API SimpleConfigItemsList&	GetResultsList();
	

protected:

	N2FCORE_API XmlParserSimpleConfig(TSimpleConfigItemType itemsType, CString& desiredParentNodeName,
							CString& desiredChildNodeName);

	N2FCORE_API virtual bool ParseNode(IXMLDOMNode *domNode);

	N2FCORE_API virtual bool IsParentNodeDesired( IXMLDOMNode *domNode );
	N2FCORE_API virtual bool IsChildNodeDesired( IXMLDOMNode *domNode );

	N2FCORE_API bool GetKnownAttributesForNode( IXMLDOMNode *node, long& id, CString& name);
	
	TSimpleConfigItemType			iItemsType;
	SimpleConfigItemsList			iResultsList;

	CString							iIDAttrName;
	CString							iNameAttrName;

	CString							iDesiredParentNodeName;
	CString							iDesiredChildNodeName;
};
