#pragma once

class WebServiceN2FMemberServiceLoginMethodDataProvider
{
public:

	N2FCORE_API virtual void GetUsername( CString& username ) = 0;
	N2FCORE_API virtual void GetPassword( CString& password ) = 0;

};

class WebServiceN2FSnapUpServiceDeviceUploadMethodDataProvider:
			public WebServiceN2FMemberServiceLoginMethodDataProvider
{
public:

	N2FCORE_API virtual void GetFilePathToUpload( CString& filePath ) = 0;
	N2FCORE_API virtual void GetTimeForUpload( SYSTEMTIME& st ) = 0;
};