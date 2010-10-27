#include "stdafx.h"

#include <controllersettings.h>

#include <xmlparserwrapper.h>
#include <xmlparsersettings.h>




N2FCORE_API ControllerSettings::ControllerSettings()
{
	iXmlParser = NULL;
	iSettingsParser = NULL;

	CString appPath;
	ControllerUtil::GetModuleFolder( appPath );

	iSettingsFilePath = appPath + CString("\\data\\n2f-client.xml");

	iMaxRecentUploadsStored = 0;

	LOGMSG("constructed");
}

N2FCORE_API ControllerSettings::~ControllerSettings()
{
	if ( NULL != iSettingsParser )
		delete iSettingsParser;

	if ( NULL != iXmlParser )
		delete iXmlParser;

	LOGMSG("destructed");
}

N2FCORE_API bool ControllerSettings::Initialize()
{
	ASSERT( NULL == iXmlParser );
	iXmlParser = new XmlParserWrapper;
	ASSERT( NULL != iXmlParser );

	bool result = iXmlParser->Initialize();

	ASSERT( NULL == iSettingsParser );
	iSettingsParser = XmlParserSettings::CreateSettingsParser();

	if ( NULL != iSettingsParser )
	{
		result = iXmlParser->ParseXmlFile( iSettingsFilePath, iSettingsParser );
	}

	if ( result )
	{
		result = iSettingsParser->GetUsername( iUserName );
	}

	if ( result )
	{
		result = iSettingsParser->GetPassword( iPassword );
	}

	iMaxRecentUploadsStored = iSettingsParser->GetMaxRecentUploadsStoredNumber();
	iLoginServiceID = (TWebServiceType)(iSettingsParser->GetLoginServiceID());
	iUploadServiceID = (TWebServiceType)(iSettingsParser->GetUploadServiceID());

	if ( result )
	{
		iRecentUploads.RemoveAll();

		TRecentUploads& remoteItems = iSettingsParser->GetStoredRecentUploads();
		for ( int i = 0; i < remoteItems.GetSize(); ++i )
		{
			this->AddNewRecentUpload( remoteItems[i] );
		}
	}

	return result;
}

N2FCORE_API void ControllerSettings::SetCurrentUserCredentials( CString& user, CString& password )
{
	iUserName = user;
	iPassword = password;
}

N2FCORE_API void ControllerSettings::GetCurrentUserCredentials( CString& user, CString& password )
{
	user = iUserName;
	password = iPassword;
}

N2FCORE_API void ControllerSettings::GetClientConfigPathes( CString& pathSkin, CString& pathStrings, CString& pathColors )
{
	ASSERT( iSettingsParser != NULL );
	if ( NULL == iSettingsParser )
		return;

	iSettingsParser->GetSkinFilePath( pathSkin );
	iSettingsParser->GetLanguageFilePath( pathStrings );
	iSettingsParser->GetColorsFilePath( pathColors );

}

N2FCORE_API bool ControllerSettings::SaveSettings()
{
	ASSERT( iSettingsParser != NULL );
	if ( NULL == iSettingsParser )
		return false;

	iSettingsParser->SetUsername( iUserName );
	iSettingsParser->SetPassword( iPassword );

	TRecentUploads& remoteItems = iSettingsParser->GetStoredRecentUploads();
	remoteItems.RemoveAll();
	for ( int i = 0; i < iRecentUploads.GetSize(); ++i )
	{
		remoteItems.Add( iRecentUploads[i] );
	}

	return iSettingsParser->SaveCurrentValues( iSettingsFilePath );
}

N2FCORE_API void ControllerSettings::RememberUploadItem( CString& fileName, CString& filePath, SYSTEMTIME time )
{
	int idx = this->FindRecentUploadByPath( filePath );
	if ( -1 == idx )
	{
		TRecentUploadItem item;
		item.fileTitle = fileName;
		item.filePath = filePath;
		item.dateTime = time;
		item.isFinished = false;

		this->AddNewRecentUpload( item );
	}
	else
	{
		ASSERT( false );
	}
}

N2FCORE_API int ControllerSettings::FindRecentUploadByPath( CString& filePath )
{
	int idx = -1;
	for ( int i = 0; i < iRecentUploads.GetSize(); ++i )
	{
		if ( filePath == iRecentUploads[i].filePath )
		{
			idx = i;
			break;
		}
	}

	return idx;
}

N2FCORE_API void ControllerSettings::AddNewRecentUpload( TRecentUploadItem& item )
{
	iRecentUploads.Add( item );

	while ( iRecentUploads.GetSize() > iMaxRecentUploadsStored )
	{
		iRecentUploads.RemoveAt( 0 );
	}
}

N2FCORE_API void ControllerSettings::UpdateUploadStatus( CString& filePath, bool isFinished )
{
	int idx = this->FindRecentUploadByPath( filePath );
	if ( -1 != idx )
	{
		iRecentUploads[idx].isFinished = isFinished;
	}
	else
	{
		ASSERT( false );
	}

}

N2FCORE_API	int ControllerSettings::GetStoredRecentUploadsCount()
{
	return iRecentUploads.GetSize();
}

N2FCORE_API void ControllerSettings::GetRecentUploadItemByIndex( int index, TRecentUploadItem& item )
{
	TRecentUploadItem result;

	if ( index >= 0 && index < iRecentUploads.GetSize() )
	{
		//item.fileTitle = iRecentUploads[index].fileTitle;
		//item.filePath = iRecentUploads[index].filePath;
		//item.dateTime = iRecentUploads[index].dateTime;
		//item.isFinished = iRecentUploads[index].isFinished;
		result = iRecentUploads[index];
	}
	else
	{
		ASSERT( false );
	}

	item = result;
}

N2FCORE_API TWebServiceType ControllerSettings::GetLoginServiceID()
{
	return iLoginServiceID;
}

N2FCORE_API TWebServiceType ControllerSettings::GetUploadServiceID()
{
	return iUploadServiceID;
}
