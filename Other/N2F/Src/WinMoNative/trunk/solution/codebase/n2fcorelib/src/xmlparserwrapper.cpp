#include "stdafx.h"

#include <objsafe.h>
#include <objbase.h>
#include <xmlparserwrapper.h>



N2FCORE_API XmlParserWrapper::XmlParserWrapper()
{
	
}

N2FCORE_API XmlParserWrapper::~XmlParserWrapper()
{
	
}

N2FCORE_API bool XmlParserWrapper::Initialize()
{
	bool result = false;
	HRESULT hr = E_FAIL;

	iXmlDoc = NULL;

	hr = iXmlDoc.CoCreateInstance( __uuidof(DOMDocument) );
	if ( FAILED(hr) || NULL == iXmlDoc )
	{
		iXmlDoc = NULL;
		return result;
	}

	hr = iXmlDoc->put_async( VARIANT_FALSE );
	
	CComQIPtr<IObjectSafety, &IID_IObjectSafety> objSafity( iXmlDoc );
	if ( NULL != objSafity )
	{
		DWORD dwSupported = 0, dwEnabled = 0;
		objSafity->GetInterfaceSafetyOptions( IID_IXMLDOMDocument, &dwSupported, &dwEnabled );
		objSafity->SetInterfaceSafetyOptions( IID_IXMLDOMDocument, dwSupported, 0 );
	}

	result = (hr == S_OK);
	return result;

}

N2FCORE_API bool XmlParserWrapper::ParseXmlFile( CString& filePath, XmlElementParser *elementParser )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	if ( NULL == iXmlDoc || NULL == elementParser )
	{
		ASSERT(FALSE);
		return false;
	}

	VARIANT_BOOL bSuccess = VARIANT_FALSE;
	hr = iXmlDoc->load( CComVariant(filePath), &bSuccess );
	if ( FAILED(hr) || VARIANT_FALSE == bSuccess )
	{
		LOGMSG("Failed to load xml file by the path: %s", filePath);
		return result;
	}

	CComPtr<IXMLDOMElement> iRootElement;
	hr = iXmlDoc->get_documentElement( &iRootElement );
	if ( FAILED(hr) || NULL ==iRootElement )
	{
		LOGMSG("Empty xml file!");
		return result;
	}

	elementParser->AssignDOMElement(iRootElement);

	result = elementParser->Parse();

	return result;
}





