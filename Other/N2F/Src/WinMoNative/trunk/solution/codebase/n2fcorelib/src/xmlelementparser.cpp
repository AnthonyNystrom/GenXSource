#include "stdafx.h"
#include <xmlelementparser.h>


N2FCORE_API XmlElementParser::XmlElementParser( )
{

}

N2FCORE_API XmlElementParser::~XmlElementParser()
{

}

N2FCORE_API  bool XmlElementParser::SaveElement( CString& fileName )
{
	if ( NULL == iDomElement )
		return false;

	HRESULT hr = E_FAIL;
	
	CComPtr<IXMLDOMDocument>	document;

	hr = iDomElement->get_ownerDocument( &document );
	if ( FAILED(hr) || (NULL == document) )
		return false;


	// TODO: should save updated dom somehow

	CComVariant variantValue( fileName );
	hr = document->save( variantValue );
	if ( FAILED(hr) )
	{
		LOGMSG("Failed to update DOM document %s", fileName);
	}

	return SUCCEEDED(hr);
}

N2FCORE_API  void XmlElementParser::AssignDOMElement( IXMLDOMElement *element )
{
	iDomElement = NULL;
	ASSERT( element != NULL );
	iDomElement = element;
}

N2FCORE_API  bool XmlElementParser::Parse()
{
	bool result = false;
	HRESULT hr = E_FAIL;

	if ( NULL == iDomElement )
	{
		ASSERT(FALSE);
		return result;
	}

	CComPtr<IXMLDOMNode> node;
	CComPtr<IXMLDOMNodeList> list;

	hr = iDomElement->get_childNodes( &list );
	if ( FAILED(hr) || NULL == list )
	{
		return result;
	}

	long cntNodes;
	list->get_length( &cntNodes );

	LOGMSG("Enumerating nodes: %d", cntNodes);

	for ( long i = 0; i < cntNodes; ++i )
	{
		node = NULL;
		list->get_item( i, &node );
		if ( this->IsParentNodeDesired( node ) )
		{
			// have match, should parse childs
			this->ParseChildrenForNode( node );
		}
	}

	LOGMSG("Finished enumerating nodes");

	result = true;
	return result;
}

N2FCORE_API  bool XmlElementParser::ParseNode( IXMLDOMNode *domNode )
{
	// should be never be here
	ASSERT(FALSE);
	return true;
}


N2FCORE_API  bool XmlElementParser::ParseChildrenForNode( IXMLDOMNode *domNode )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	if ( NULL == domNode )
		return result;

	IXMLDOMNode *pNextNode = NULL;
	IXMLDOMNode *pChildNode = NULL;

	hr = domNode->get_firstChild( &pChildNode );

	while ( NULL != pChildNode )
	{
		if ( this->IsChildNodeDesired( pChildNode ) )
			this->ParseNode( pChildNode );

		hr = pChildNode->get_nextSibling( &pNextNode );
		if ( FAILED(hr) )
			break;
		pChildNode = pNextNode;
	}

	result = (S_OK == hr);
	return result;
}

