#pragma once

class	XmlParserWrapper;
class	XmlParserSettings;


class ControllerSettings
{
public:

	N2FCORE_API ControllerSettings();
	N2FCORE_API virtual ~ControllerSettings();

	N2FCORE_API bool	Initialize();
	N2FCORE_API void	SetCurrentUserCredentials( CString& user, CString& password );
	N2FCORE_API void	GetCurrentUserCredentials( CString& user, CString& password );
	N2FCORE_API void	GetClientConfigPathes( CString& pathSkin, CString& pathStrings, CString& pathColors );
	N2FCORE_API TWebServiceType	GetLoginServiceID();
	N2FCORE_API TWebServiceType GetUploadServiceID();

	N2FCORE_API void	RememberUploadItem( CString& fileName, CString& filePath, SYSTEMTIME time );
	N2FCORE_API void	UpdateUploadStatus( CString& filePath, bool isFinished );
	N2FCORE_API	int		GetStoredRecentUploadsCount();
	N2FCORE_API void	GetRecentUploadItemByIndex( int index, TRecentUploadItem& item );

	N2FCORE_API bool	SaveSettings();

private:

	N2FCORE_API int		FindRecentUploadByPath( CString& filePath );
	N2FCORE_API void	AddNewRecentUpload( TRecentUploadItem& item );

	CString	iUserName;
	CString	iPassword;

	TWebServiceType		iLoginServiceID;
	TWebServiceType		iUploadServiceID;

	int		iMaxRecentUploadsStored;

	CString iSettingsFilePath;
	XmlParserSettings *iSettingsParser;
	XmlParserWrapper *iXmlParser;

	TRecentUploads	iRecentUploads;

};
