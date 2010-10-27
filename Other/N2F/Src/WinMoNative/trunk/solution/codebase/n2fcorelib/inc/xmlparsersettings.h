#pragma once

#include <msxml2.h>

#include <xmlelementparser.h>
#include <configuration-data.h>

enum TSettingsID
{
	ESIDUnknown			= 0,

	ESIDUserName,
	ESIDUserPassword,
	ESIDSkinFilePath,
	ESIDLanguageFilePath,
	ESIDColorsFilePath,
	ESIDLoginServiceID,
	ESIDUploadServiceID,
	ESIDRecentUploadsNode,
	ESIDMaxRecentUploadItemsCount,
	ESIDRecentUploadsSingleItem,
	ESIDRecentUploadsItemFileTitle,
	ESIDRecentUploadsItemFilePath,
	ESIDRecentUploadsItemDateTime,
	ESIDRecentUploadsItemsFinishedFlag
};



typedef	CSimpleArray<TSettingsID>			TSupportedSettingsIDs;
typedef CSimpleArray<CString>				TSupportedSettingsNames;
typedef CSimpleMap<TSettingsID, CString>	TSettingsMap;


class XmlParserSettings:
		public XmlElementParser
{
public:
	N2FCORE_API	static XmlParserSettings*	CreateSettingsParser();

	N2FCORE_API CString	TranslateIDToName( TSettingsID id );
	N2FCORE_API TSettingsID TranslateNameToID( CString& name );

	N2FCORE_API bool	GetUsername( CString& userName );
	N2FCORE_API bool	GetPassword( CString& password );
	N2FCORE_API bool	GetSkinFilePath( CString& filepath );
	N2FCORE_API bool	GetLanguageFilePath( CString& filepath );
	N2FCORE_API bool	GetColorsFilePath( CString& filepath );

	N2FCORE_API int		GetMaxRecentUploadsStoredNumber();

	N2FCORE_API void	SetUsername( CString& userName );
	N2FCORE_API void	SetPassword( CString& password );

	N2FCORE_API int		GetLoginServiceID();
	N2FCORE_API int		GetUploadServiceID();

	N2FCORE_API TRecentUploads&	GetStoredRecentUploads();

	N2FCORE_API bool	SaveCurrentValues( CString& fileName );

protected:

	N2FCORE_API	XmlParserSettings( CString& desiredParentNodeName, TSupportedSettingsIDs& supportedSettingsList );

	N2FCORE_API virtual bool ParseNode(IXMLDOMNode *domNode);
	N2FCORE_API virtual bool ParseRecentUploadsNode( IXMLDOMNode *domNode );
	N2FCORE_API virtual bool ParseRecentUploadsSingleItem( IXMLDOMNode *domNode );
	N2FCORE_API virtual bool ParseRecentUploadsItemValue( TSettingsID id, IXMLDOMNode *domNode );

	N2FCORE_API virtual bool IsParentNodeDesired( IXMLDOMNode *domNode );
	N2FCORE_API virtual bool IsChildNodeDesired( IXMLDOMNode *domNode );

	N2FCORE_API bool GetSettingsValueByID( TSettingsID id, CString& value );
	N2FCORE_API void SetSettingsValueByID( TSettingsID id, CString& value );

	N2FCORE_API void SaveRUItems( IXMLDOMNode *domNode );
	N2FCORE_API void UpdateRUItemNode( int index, IXMLDOMNode *domNode );

	CString						iParentNodeName;
	TSupportedSettingsNames		iSupportedSettigsNames;
	TSettingsMap				iSettingsValues;

	TSettingsMap				iSettingsIDNameMap;

	TRecentUploadItem			iCurrentItem;
	TRecentUploads				iStoredRecentUploads;

};
