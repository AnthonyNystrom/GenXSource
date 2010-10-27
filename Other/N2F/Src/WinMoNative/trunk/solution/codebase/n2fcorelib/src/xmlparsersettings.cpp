#include "stdafx.h"

#include <xmlutil.h>
#include <xmlparsersettings.h>

N2FCORE_API	 XmlParserSettings* XmlParserSettings::CreateSettingsParser()
{
	XmlParserSettings *result = NULL;

	CString parentNodeName("settings");
	TSupportedSettingsIDs listSettings;

	listSettings.Add( ESIDUserName );
	listSettings.Add( ESIDUserPassword );
	listSettings.Add( ESIDSkinFilePath );
	listSettings.Add( ESIDLanguageFilePath );
	listSettings.Add( ESIDColorsFilePath );
	listSettings.Add( ESIDLoginServiceID );
	listSettings.Add( ESIDUploadServiceID );
	listSettings.Add( ESIDRecentUploadsNode );
	listSettings.Add( ESIDMaxRecentUploadItemsCount );
	listSettings.Add( ESIDRecentUploadsSingleItem );
	listSettings.Add( ESIDRecentUploadsItemFileTitle );
	listSettings.Add( ESIDRecentUploadsItemFilePath );
	listSettings.Add( ESIDRecentUploadsItemDateTime );
	listSettings.Add( ESIDRecentUploadsItemsFinishedFlag );


	result = new XmlParserSettings( parentNodeName, listSettings );

	return result;
}

N2FCORE_API  CString XmlParserSettings::TranslateIDToName( TSettingsID id )
{
	CString result = ESIDUnknown;

	int idx = iSettingsIDNameMap.FindKey( id );
	if ( -1 != idx )
	{
		result = iSettingsIDNameMap.GetValueAt( idx );
	}
	
	return result;
}

N2FCORE_API  TSettingsID XmlParserSettings::TranslateNameToID( CString& name )
{
	TSettingsID result = ESIDUnknown;

	int idx = iSettingsIDNameMap.FindVal( name );
	if ( -1 != idx )
	{
		result = iSettingsIDNameMap.GetKeyAt( idx );
	}

	return result;
}

N2FCORE_API XmlParserSettings::XmlParserSettings( CString& desiredParentNodeName, TSupportedSettingsIDs& supportedSettingsList )
{
	iSettingsIDNameMap.Add( ESIDUserName, CString("user-name") );
	iSettingsIDNameMap.Add( ESIDUserPassword, CString("user-password") );
	iSettingsIDNameMap.Add( ESIDSkinFilePath, CString("skin-filepath") );
	iSettingsIDNameMap.Add( ESIDLanguageFilePath, CString("language-filepath") );
	iSettingsIDNameMap.Add( ESIDColorsFilePath, CString("colors-filepath") );
	iSettingsIDNameMap.Add( ESIDLoginServiceID, CString("login-service") );
	iSettingsIDNameMap.Add( ESIDUploadServiceID, CString("upload-service") );
	iSettingsIDNameMap.Add( ESIDRecentUploadsNode, CString("recent-uploads") );
	iSettingsIDNameMap.Add( ESIDMaxRecentUploadItemsCount, CString("max-uploads-count") );
	iSettingsIDNameMap.Add( ESIDRecentUploadsSingleItem, CString("recent-upload-item") );
	iSettingsIDNameMap.Add( ESIDRecentUploadsItemFileTitle, CString("upload-filetitle") );
	iSettingsIDNameMap.Add( ESIDRecentUploadsItemFilePath, CString("upload-filepath") );
	iSettingsIDNameMap.Add( ESIDRecentUploadsItemDateTime, CString("upload-datetime") );
	iSettingsIDNameMap.Add( ESIDRecentUploadsItemsFinishedFlag, CString("upload-finished") );

	iParentNodeName = desiredParentNodeName;
	
	for ( int i = 0; i < supportedSettingsList.GetSize(); ++i )
	{
		CString resolvedName = XmlParserSettings::TranslateIDToName( supportedSettingsList[i] );
		iSupportedSettigsNames.Add( resolvedName );
		//iSettingsValues.Add( supportedSettingsList[i], resolvedName );
	}
}

N2FCORE_API  bool XmlParserSettings::IsParentNodeDesired( IXMLDOMNode *domNode )
{
	CString name;
	if ( false == XmlUtil::GetNodeName(domNode, name) )
		return false;

	return ( 0 == name.CompareNoCase(iParentNodeName) );
}

N2FCORE_API  bool XmlParserSettings::IsChildNodeDesired( IXMLDOMNode *domNode )
{
	CString name;
	if ( false == XmlUtil::GetNodeName(domNode, name) )
		return false;

	if ( -1 != iSupportedSettigsNames.Find( name ) )
		return true;

	return false;
}

N2FCORE_API  bool XmlParserSettings::ParseNode( IXMLDOMNode *domNode )
{
	bool result = false;
	HRESULT hr = E_FAIL;

	if ( NULL == domNode )
		return result;

	CString nodeName;
	if ( false == XmlUtil::GetNodeName( domNode, nodeName ) )
		return result;

	TSettingsID settingsID = this->TranslateNameToID(nodeName);

	int idx = iSettingsValues.FindKey( settingsID );
	if ( -1 != idx )
	{
		// setting value was already set - duplicated nodes in xml ?
		ASSERT( FALSE );
		return result;
	}

	if ( ESIDRecentUploadsNode == settingsID )
	{
		bool reParseResult = this->ParseRecentUploadsNode( domNode );
		return reParseResult;
	}
	else if ( ESIDRecentUploadsSingleItem == settingsID )
	{
		return this->ParseRecentUploadsSingleItem( domNode );
	}
	else if ( ESIDRecentUploadsItemFilePath == settingsID ||
		ESIDRecentUploadsItemFileTitle == settingsID ||
		ESIDRecentUploadsItemDateTime == settingsID ||
		ESIDRecentUploadsItemsFinishedFlag == settingsID )
	{
		return this->ParseRecentUploadsItemValue( settingsID, domNode );
	}

	CString nodeValue;
	if ( false == XmlUtil::GetNodeText( domNode, nodeValue ) )
	{
		return result;
	}

	iSettingsValues.Add( settingsID, nodeValue );
	result = true;

	return result;
}

N2FCORE_API  bool XmlParserSettings::ParseRecentUploadsNode( IXMLDOMNode *domNode )
{
	ASSERT( NULL != domNode );
	if ( NULL == domNode )
		return false;

	return this->ParseChildrenForNode( domNode );
}

N2FCORE_API  bool XmlParserSettings::ParseRecentUploadsSingleItem( IXMLDOMNode *domNode )
{
	ASSERT( NULL != domNode );
	if ( NULL == domNode )
		return false;

	TRecentUploadItem emptyItem;
	this->iCurrentItem = emptyItem;

	bool result = this->ParseChildrenForNode( domNode );

	this->iStoredRecentUploads.Add( this->iCurrentItem );

	return result;
}

N2FCORE_API  bool XmlParserSettings::ParseRecentUploadsItemValue( TSettingsID id, IXMLDOMNode *domNode )
{
	bool result = true;

	ASSERT( NULL != domNode );
	if ( NULL == domNode )
		return false;

	CString nodeText;
	XmlUtil::GetNodeText( domNode, nodeText );

	if ( ESIDRecentUploadsItemFilePath == id )
	{
		this->iCurrentItem.filePath = nodeText;
	}
	else if ( ESIDRecentUploadsItemFileTitle == id )
	{
		this->iCurrentItem.fileTitle = nodeText;
	}
	else if ( ESIDRecentUploadsItemDateTime == id )
	{
		SYSTEMTIME st = {0};

		_stscanf( nodeText, _T("%d:%d:%d"), &(st.wYear), &(st.wMonth), &(st.wDay) );
		this->iCurrentItem.dateTime = st;
	}
	else if ( ESIDRecentUploadsItemsFinishedFlag == id )
	{
		this->iCurrentItem.isFinished = ( 0 == nodeText.CompareNoCase(_T("true")) );
	}

	return result;
}

N2FCORE_API bool XmlParserSettings::GetSettingsValueByID( TSettingsID id, CString& value )
{
	int idx = iSettingsValues.FindKey( id );
	if ( -1 == idx )
		return false;

	value = iSettingsValues.GetValueAt( idx );
	return true;
}

N2FCORE_API bool XmlParserSettings::GetUsername( CString& userName )
{
	return this->GetSettingsValueByID( ESIDUserName, userName );
}

N2FCORE_API bool XmlParserSettings::GetPassword( CString& password )
{
	return this->GetSettingsValueByID( ESIDUserPassword, password );
}

N2FCORE_API bool XmlParserSettings::GetSkinFilePath( CString& filepath )
{
	return this->GetSettingsValueByID( ESIDSkinFilePath, filepath );
}

N2FCORE_API bool XmlParserSettings::GetLanguageFilePath( CString& filepath )
{
	return this->GetSettingsValueByID( ESIDLanguageFilePath, filepath );
}

N2FCORE_API bool XmlParserSettings::GetColorsFilePath( CString& filepath )
{
	return this->GetSettingsValueByID( ESIDColorsFilePath, filepath );
}

N2FCORE_API int XmlParserSettings::GetLoginServiceID()
{
	int result = 0;

	CString value;
	this->GetSettingsValueByID( ESIDLoginServiceID, value );

	result = _ttoi( value );

	return result;
}

N2FCORE_API int XmlParserSettings::GetUploadServiceID()
{
	int result = 0;

	CString value;
	this->GetSettingsValueByID( ESIDUploadServiceID, value );

	result = _ttoi( value );

	return result;
}

N2FCORE_API int XmlParserSettings::GetMaxRecentUploadsStoredNumber()
{
	CString value;
	this->GetSettingsValueByID( ESIDMaxRecentUploadItemsCount, value );

	int result = _ttoi( value );

	return result;
}

N2FCORE_API void XmlParserSettings::SetSettingsValueByID( TSettingsID id, CString& value )
{
	int idx = iSettingsValues.FindKey( id );
	if ( -1 == idx )
	{
		iSettingsValues.Add( id, value );
	}
	else
	{
		iSettingsValues.GetValueAt( idx ) = value;
	}
}

N2FCORE_API void XmlParserSettings::SetUsername( CString& userName )
{
	this->SetSettingsValueByID( ESIDUserName, userName );
}

N2FCORE_API void XmlParserSettings::SetPassword( CString& password )
{
	this->SetSettingsValueByID( ESIDUserPassword, password );
}



N2FCORE_API bool XmlParserSettings::SaveCurrentValues( CString& fileName )
{
	bool result = false;
	if ( NULL == iDomElement || 0 == fileName.GetLength() )
	{
		ASSERT( FALSE );
		return result;
	}

	HRESULT hr = E_FAIL;

	CComPtr<IXMLDOMNode> rootNode;
	CComPtr<IXMLDOMNodeList> rootNodesList;

	hr = iDomElement->get_firstChild( &rootNode );

	while ( SUCCEEDED(hr) && (NULL != rootNode) )
	{
		if ( this->IsParentNodeDesired( rootNode ) )
		{
			CComPtr<IXMLDOMNode> node;

			for ( int i = 0; i < iSettingsValues.GetSize(); ++i )
			{
				CString name = this->TranslateIDToName( iSettingsValues.GetKeyAt(i) );

				node = NULL;
				if ( true == XmlUtil::GetNodeByName( rootNode, name, node ) )
				{
					XmlUtil::SetNodeValue( node, iSettingsValues.GetValueAt(i) );
					break;
				}
			}

			CString nodeName = this->TranslateIDToName( ESIDRecentUploadsNode );
			CComPtr<IXMLDOMNode> ruNode;
			if ( true == XmlUtil::GetNodeByName( rootNode, nodeName, ruNode ) )
			{
				this->SaveRUItems( ruNode );
			}
		}

		CComPtr<IXMLDOMNode> nextNode;
		hr = rootNode->get_nextSibling( &nextNode );
		rootNode = NULL;
		rootNode = nextNode;
	}

	result = this->SaveElement( fileName );

	return result;
}

N2FCORE_API TRecentUploads& XmlParserSettings::GetStoredRecentUploads()
{
	return iStoredRecentUploads;
}

N2FCORE_API void XmlParserSettings::SaveRUItems( IXMLDOMNode *domNode )
{
	ASSERT( NULL != domNode );
	if ( NULL == domNode )
		return;

	HRESULT hr;

	CComPtr<IXMLDOMNodeList> allChildNodes;
	domNode->get_childNodes( &allChildNodes );

	long childCount = 0;
	allChildNodes->get_length( &childCount );

	long cntItemNodes = childCount - 1;

	CComPtr< IXMLDOMNode > firstRUItemNode;
	CString ruItemNodeName = this->TranslateIDToName( ESIDRecentUploadsSingleItem );
	CString ruItemFilePathName = this->TranslateIDToName( ESIDRecentUploadsItemFilePath );
	CString ruItemFileTitleName = this->TranslateIDToName( ESIDRecentUploadsItemFileTitle );
	CString ruItemDataTimeName = this->TranslateIDToName( ESIDRecentUploadsItemDateTime );
	CString ruItemFinishedName = this->TranslateIDToName( ESIDRecentUploadsItemsFinishedFlag );

	if ( 0 == cntItemNodes )
	{
		CComPtr<IXMLDOMDocument> ownerDocument;
		hr = domNode->get_ownerDocument( &ownerDocument );
		if ( FAILED(hr) || (NULL == ownerDocument) )
			return;

		CComPtr<IXMLDOMElement>	createdElement;
		hr = ownerDocument->createElement(CComBSTR(ruItemNodeName), &createdElement);
		if ( FAILED(hr) || (NULL == createdElement) )
			return;

		firstRUItemNode = createdElement;

		CComPtr<IXMLDOMElement> filePathElement;
		hr = ownerDocument->createElement(CComBSTR(ruItemFilePathName), &filePathElement);
		if ( FAILED(hr) || (NULL == filePathElement) )
			return;

		createdElement->appendChild( filePathElement, NULL );

		CComPtr<IXMLDOMElement> fileTitleElement;
		hr = ownerDocument->createElement(CComBSTR(ruItemFileTitleName), &fileTitleElement);
		if ( FAILED(hr) || (NULL == fileTitleElement) )
			return;

		createdElement->appendChild( fileTitleElement, NULL );

		CComPtr<IXMLDOMElement> dateTimeElement;
		hr = ownerDocument->createElement(CComBSTR(ruItemDataTimeName), &dateTimeElement);
		if ( FAILED(hr) || (NULL == dateTimeElement) )
			return;

		createdElement->appendChild( dateTimeElement, NULL );

		CComPtr<IXMLDOMElement> finishedElement;
		hr = ownerDocument->createElement(CComBSTR(ruItemFinishedName), &finishedElement);
		if ( FAILED(hr) || (NULL == finishedElement) )
			return;

		createdElement->appendChild( finishedElement, NULL );

		domNode->appendChild( firstRUItemNode, NULL );

	}
	
	if ( cntItemNodes > 0 )
	{
		for ( int i = 0; i < cntItemNodes-1; ++i )
		{
			CComPtr< IXMLDOMNode > tempNode;
			XmlUtil::GetNodeByName( domNode, ruItemNodeName, tempNode );

			domNode->removeChild( tempNode, NULL );
		}

		XmlUtil::GetNodeByName( domNode, ruItemNodeName, firstRUItemNode );
	}

	if ( iStoredRecentUploads.GetSize() == 0 )
	{
		domNode->removeChild( firstRUItemNode, NULL );
		return;
	}

	int index = 0;
	this->UpdateRUItemNode( index, firstRUItemNode );

	for ( int i = index+1; i < iStoredRecentUploads.GetSize(); ++i )
	{
		CComPtr<IXMLDOMNode> newNode;
		firstRUItemNode->cloneNode( VARIANT_TRUE, &newNode );

		this->UpdateRUItemNode( i, newNode );

		hr = domNode->appendChild( newNode, NULL );
	}
}

N2FCORE_API void XmlParserSettings::UpdateRUItemNode( int index, IXMLDOMNode *domNode )
{
	ASSERT( NULL != domNode );
	if ( NULL == domNode )
		return;

	if ( index < 0 || index >= iStoredRecentUploads.GetSize() )
	{
		ASSERT( false );
		return;
	}

	HRESULT hr;
	CString ruItemFilePathName = this->TranslateIDToName( ESIDRecentUploadsItemFilePath );
	CString ruItemFileTitleName = this->TranslateIDToName( ESIDRecentUploadsItemFileTitle );
	CString ruItemDataTimeName = this->TranslateIDToName( ESIDRecentUploadsItemDateTime );
	CString ruItemFinishedName = this->TranslateIDToName( ESIDRecentUploadsItemsFinishedFlag );

	CComPtr<IXMLDOMNode> fpNode, ftNode, dtNode, ifNode;

	hr = XmlUtil::GetNodeByName( domNode, ruItemFilePathName, fpNode );
	if ( FAILED(hr) || NULL == fpNode )
		return;

	hr = XmlUtil::SetNodeValue( fpNode, iStoredRecentUploads[index].filePath );

	hr = XmlUtil::GetNodeByName( domNode, ruItemFileTitleName, ftNode );
	if ( FAILED(hr) || NULL == ftNode )
		return;

	hr = XmlUtil::SetNodeValue( ftNode, iStoredRecentUploads[index].fileTitle );

	hr = XmlUtil::GetNodeByName( domNode, ruItemDataTimeName, dtNode );
	if ( FAILED(hr) || NULL == dtNode )
		return;

	CString strDateTime;
	SYSTEMTIME st = iStoredRecentUploads[index].dateTime;
	strDateTime.Format(_T("%d:%d:%d"), st.wYear, st.wMonth, st.wDay );

	hr = XmlUtil::SetNodeValue( dtNode, strDateTime );

	hr = XmlUtil::GetNodeByName( domNode, ruItemFinishedName, ifNode );
	if ( FAILED(hr) || NULL == ifNode )
		return;

	CString strIsFinished( (iStoredRecentUploads[index].isFinished? "true": "false") ); 

	hr = XmlUtil::SetNodeValue( ifNode, strIsFinished );


}



