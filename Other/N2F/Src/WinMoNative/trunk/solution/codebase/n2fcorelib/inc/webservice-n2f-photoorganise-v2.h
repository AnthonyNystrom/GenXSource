#pragma once

#include "webservicebase.h"

namespace WS_PhotoOrganise_v2 {
	class PhotoOrganiseSoap;
}

struct WS_POv2_DeviceUploadPhoto_InitParams;



//! WebServiceN2FPhotoOrganise_v2 class 

/*!
WebServiceN2FPhotoOrganise_v2 class provides wrapper for N2F PhotoOrganise web-service
*/

class WebServiceN2FPhotoOrganise_v2:
	public WebServiceBase
{
public:

	//! WebServiceN2FPhotoOrganise_v2 constructor
	N2FCORE_API WebServiceN2FPhotoOrganise_v2();

	//! WebServiceN2FPhotoOrganise_v2 destructor
	N2FCORE_API virtual ~WebServiceN2FPhotoOrganise_v2();

	//! Web-service initialization method

	/*!
	Wrapped web-service initialization method
	\param customEndPoint a custom end-point to use for web-service, if empty string is passed - default end-point is used
	\return bool as initialization result
	*/
	N2FCORE_API virtual bool Initialize(CString& customEndPoint);


	//! Get Member Id by specified nick & password
	/*! 
	N2F MemberServices service method: GetMemberId
	\param webMemberID web member ID
	\param webPassword web password
	\param base64Photo base64 encoded image
	\param dateTime date-time stamp
	\return bool true - if succeeded, otherwise - false
	*/
	N2FCORE_API bool DeviceUploadPhoto(CString& webMemberID, CString& webPassword, CString& photoFileName, SYSTEMTIME &dateTime);

protected:

	N2FCORE_API bool PrepareParams_DeviceUploadPhoto(CString& webMemberID, CString& webPassword, 
		CString& photoFileName, SYSTEMTIME &dateTime, WS_POv2_DeviceUploadPhoto_InitParams* pOut);


private:

	WS_PhotoOrganise_v2::PhotoOrganiseSoap	*iPhotoServices;

};
