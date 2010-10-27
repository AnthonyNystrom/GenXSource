#include "stdafx.h"

#include <webservice-n2f-snapupservice.h>

#include <stdsoap2.h>
#include <WS_SnapUpServiceSnapUpServiceSoapProxy.h>

#include <controllerutil.h>

struct WS_SUS_DeviceUploadPhoto_InitParams {
	std::string nickname;
	std::string password;
	std::string base64Data;
	std::string webDateTime;
};


N2FCORE_API WebServiceN2FSnapUpService::WebServiceN2FSnapUpService()
		:WebServiceBase(EWS_N2F_SnapUpService),
		iSnapUpService(NULL)
{
	LOGMSG("Constructed");
}

N2FCORE_API WebServiceN2FSnapUpService::~WebServiceN2FSnapUpService()
{
	if ( NULL != iSnapUpService )
		delete iSnapUpService;

	LOGMSG("Destructed");
}

N2FCORE_API  bool WebServiceN2FSnapUpService::Initialize( CString& customEndPoint )
{
	LOGMSG("Initializing!");

	WebServiceBase::Initialize( customEndPoint );

	LOGMSG("Initialized!");

	return true;
}

N2FCORE_API bool WebServiceN2FSnapUpService::DeviceUploadPhoto( CString& nickname, CString& password, CString& photoFileName, SYSTEMTIME &dateTime )
{
	bool result = false;

	iSnapUpService = new WS_SnapUpService::SnapUpServiceSoap();
	ASSERT(iSnapUpService);
	if ( NULL == iSnapUpService )
		return result;


	char *strCustomEndPoint = NULL;
	const char *strOriginalEndPoint = NULL;

	if ( true == this->IsCustomeEndPointSet() )
	{
		CString csCustomEP;
		this->GetCustomEndPoint(csCustomEP);
		bool convResult = ControllerUtil::ConvertWideCharToMultiByte(csCustomEP, -1,
			&strCustomEndPoint, NULL);
		if ( false == convResult || NULL == strCustomEndPoint )
		{
			return result;
		}

		strOriginalEndPoint = iSnapUpService->endpoint;
		iSnapUpService->endpoint = strCustomEndPoint;
	}

	WS_SUS_DeviceUploadPhoto_InitParams *initParams = new WS_SUS_DeviceUploadPhoto_InitParams;
	if ( false == this->PrepareParams_DeviceUploadPhoto(nickname, password, 
		photoFileName, dateTime, initParams) )
	{
		LOGMSG("Failed to initialize params");
		delete initParams;
		return result;
	}

	WS_SnapUpService::_SnapUpService__DeviceUploadPhoto	*pParams = NULL;
	WS_SnapUpService::_SnapUpService__DeviceUploadPhotoResponse	*pResponse = NULL;

	size_t szParams = sizeof(WS_SnapUpService::_SnapUpService__DeviceUploadPhoto);
	size_t szResponse = sizeof(WS_SnapUpService::_SnapUpService__DeviceUploadPhotoResponse);

	pParams = WS_SnapUpService::soap_instantiate__SnapUpService__DeviceUploadPhoto(iSnapUpService->soap,
		SOAP_TYPE_WS_SnapUpService__SnapUpService__DeviceUploadPhoto, "", "", &szParams);

	pResponse = WS_SnapUpService::soap_instantiate__SnapUpService__DeviceUploadPhotoResponse(iSnapUpService->soap,
		SOAP_TYPE_WS_SnapUpService__SnapUpService__DeviceUploadPhotoResponse, "", "", &szResponse);

	if ( NULL != pParams && NULL != pResponse )
	{
		pParams->nickname = &(initParams->nickname);
		pParams->password = &(initParams->password);
		pParams->base64StringPhoto = &(initParams->base64Data);
		pParams->dateTime = &(initParams->webDateTime);

		int soapResult = 0;

		LOGMSG("Calling DeviceUploadPhoto");
		LOGMSG("Passed params:\n\n%s\n%s\nbuffer len %d%s\n\n", CString(pParams->nickname->c_str()), 
			CString(pParams->password->c_str()), pParams->base64StringPhoto->length(), 
			CString(pParams->dateTime->c_str()) );
		soapResult = iSnapUpService->__SnapUpService2__DeviceUploadPhoto(pParams, pResponse);

		if ( SOAP_OK == soapResult )
			result = true;
		else
			LOGMSG("Failure!!!, gsoap error code: %d", soapResult);

		this->SetLastSoapResult(soapResult);
	}

	delete initParams;

	if ( NULL != strCustomEndPoint )
	{
		delete [] strCustomEndPoint;
		iSnapUpService->endpoint = NULL;
	}

	if ( NULL != strOriginalEndPoint )
		iSnapUpService->endpoint = strOriginalEndPoint;

	delete iSnapUpService;
	iSnapUpService = NULL;

	return result;
}

N2FCORE_API bool WebServiceN2FSnapUpService::PrepareParams_DeviceUploadPhoto( CString& nickname, CString& password, CString& photoFileName, SYSTEMTIME &dateTime, WS_SUS_DeviceUploadPhoto_InitParams* pOut )
{
	bool result = false;

	char *strNickname= NULL;
	char *strPassword = NULL;
	char *strBase64 = NULL;
	char *strDateTime = NULL;

	ControllerUtil::ConvertWideCharToMultiByte(nickname, -1, &strNickname, NULL);
	if ( NULL == strNickname )
	{
		LOGME();
		return result;
	}

	std::string ssNickname(strNickname);
	delete [] strNickname;

	ControllerUtil::ConvertWideCharToMultiByte(password, -1, &strPassword, NULL);
	if ( NULL == strPassword )
	{
		LOGME();
		return result;
	}

	std::string ssPassword(strPassword);
	delete [] strPassword;

	char *readDataBuffer = NULL;
	size_t readDataSize = 0;
	ControllerUtil::ReadBinaryFileToBuffer(photoFileName, &readDataBuffer, &readDataSize);
	if ( NULL == readDataBuffer || 0 == readDataSize )
	{
		LOGME();
		return result;
	}

	ControllerUtil::EncodeDataWithBase64(readDataBuffer, readDataSize, &strBase64);
	if ( NULL == strBase64 )
	{
		delete [] readDataBuffer;
		LOGME();
		return result;
	}

	delete [] readDataBuffer;

	std::string ssBase64(strBase64);
	delete [] strBase64;

	INT64 ticks = 0;
	CString csTicks;
	ControllerUtil::SystemTimeInTicks(dateTime, ticks, csTicks);

	ControllerUtil::ConvertWideCharToMultiByte(csTicks, -1, &strDateTime, NULL);
	if ( NULL == strDateTime )
	{
		LOGME();
		return result;
	}

	std::string ssDateTime(strDateTime);
	delete [] strDateTime;

	if ( NULL != pOut )
	{
		pOut->nickname = ssNickname;
		pOut->password = ssPassword;
		pOut->base64Data = ssBase64;
		pOut->webDateTime = ssDateTime;

		result = true;
	}
	else
	{
		LOGMSG("Invalid parameter detected!");
	}

	return result;
}
