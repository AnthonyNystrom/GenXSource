#include "stdafx.h"
#include "webservice-n2f-photoorganise.h"

#include <stdsoap2.h>
#include <WS_PhotoOrganisePhotoOrganiseSoapProxy.h>

#include "controllerutil.h"

struct WS_PO_DeviceUploadPhoto_InitParams {
	std::string webMemberID;
	std::string webPassword;
	std::string base64Data;
	std::string webDateTime;
};


N2FCORE_API WebServiceN2FPhotoOrganise::WebServiceN2FPhotoOrganise()
	:WebServiceBase(EWS_N2F_PhotoOrganise),
	iPhotoServices(NULL)
{
	LOGMSG("Constructed");
}

N2FCORE_API WebServiceN2FPhotoOrganise::~WebServiceN2FPhotoOrganise()
{
	if ( NULL != iPhotoServices )
		delete iPhotoServices;

	LOGMSG("Destructed");
}

N2FCORE_API  bool WebServiceN2FPhotoOrganise::Initialize( CString& customEndPoint )
{
	LOGMSG("Initializing!");

	WebServiceBase::Initialize( customEndPoint );

	//iPhotoServices = new WS_PhotoOrganise::PhotoOrganiseSoap();
	//ASSERT(iPhotoServices);
	//if ( NULL == iPhotoServices )
	//{
	//	return false;
	//}

	LOGMSG("Initialized!");

	return true;
}

N2FCORE_API bool WebServiceN2FPhotoOrganise::DeviceUploadPhoto( CString& webMemberID, CString& webPassword, CString& photoFileName, SYSTEMTIME& dateTime )
{
	bool result = false;

	iPhotoServices = new WS_PhotoOrganise::PhotoOrganiseSoap();
	ASSERT(iPhotoServices);
	if ( NULL == iPhotoServices )
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

		strOriginalEndPoint = iPhotoServices->endpoint;
		iPhotoServices->endpoint = strCustomEndPoint;
	}

	WS_PO_DeviceUploadPhoto_InitParams *initParams = new WS_PO_DeviceUploadPhoto_InitParams;
	if ( false == this->PrepareParams_DeviceUploadPhoto(webMemberID, webPassword, 
		photoFileName, dateTime, initParams) )
	{
		LOGMSG("Failed to initialize params");
		delete initParams;
		return result;
	}

	WS_PhotoOrganise::_PhotoOrganise__DeviceUploadPhoto *pParams = NULL;
	WS_PhotoOrganise::_PhotoOrganise__DeviceUploadPhotoResponse *pResponse = NULL;

	size_t szParams = sizeof(WS_PhotoOrganise::_PhotoOrganise__DeviceUploadPhoto);
	size_t szResponse = sizeof(WS_PhotoOrganise::_PhotoOrganise__DeviceUploadPhotoResponse);

	pParams = WS_PhotoOrganise::soap_instantiate__PhotoOrganise__DeviceUploadPhoto(iPhotoServices->soap, 
		SOAP_TYPE_WS_PhotoOrganise__PhotoOrganise__DeviceUploadPhoto, "", "", &szParams);

	pResponse = WS_PhotoOrganise::soap_instantiate__PhotoOrganise__DeviceUploadPhotoResponse(iPhotoServices->soap,
		SOAP_TYPE_WS_PhotoOrganise__PhotoOrganise__DeviceUploadPhotoResponse, "", "", &szParams);

	if ( NULL != pParams && NULL != pResponse )
	{
		pParams->WebMemberID = &(initParams->webMemberID);
		pParams->WebPassword = &(initParams->webPassword);
		pParams->Base64StringPhoto = &(initParams->base64Data);
		pParams->DateTime = &(initParams->webDateTime);

		int soapResult = 0;

		LOGMSG("Calling DeviceUploadPhoto");
		soapResult = iPhotoServices->__PhotoOrganise3__DeviceUploadPhoto(pParams, pResponse);

		if ( SOAP_OK == soapResult )
			result = true;
		else
			LOGMSG("Failure!!!");

		this->SetLastSoapResult(soapResult);
	}
	
	delete initParams;

	if ( NULL != strCustomEndPoint )
	{
		delete [] strCustomEndPoint;
		iPhotoServices->endpoint = NULL;
	}

	if ( NULL != strOriginalEndPoint )
		iPhotoServices->endpoint = strOriginalEndPoint;

	delete iPhotoServices;
	iPhotoServices = NULL;

	return result;
}

N2FCORE_API bool WebServiceN2FPhotoOrganise::PrepareParams_DeviceUploadPhoto( CString& webMemberID, CString& webPassword, CString& photoFileName, SYSTEMTIME &dateTime, WS_PO_DeviceUploadPhoto_InitParams* pOut )
{
	bool result = false;

	char *strMemberID = NULL;
	char *strWebPassword = NULL;
	char *strBase64 = NULL;
	char *strDateTime = NULL;

	ControllerUtil::ConvertWideCharToMultiByte(webMemberID, -1, &strMemberID, NULL);
	if ( NULL == strMemberID )
		return result;

	std::string ssMemberID(strMemberID);
	delete [] strMemberID;

	ControllerUtil::ConvertWideCharToMultiByte(webPassword, -1, &strWebPassword, NULL);
	if ( NULL == strWebPassword )
		return result;

	std::string ssWebPass(strWebPassword);
	delete [] strWebPassword;

	char *readDataBuffer = NULL;
	size_t readDataSize = 0;
	ControllerUtil::ReadBinaryFileToBuffer(photoFileName, &readDataBuffer, &readDataSize);
	if ( NULL == readDataBuffer || 0 == readDataSize )
		return result;
	
	ControllerUtil::EncodeDataWithBase64(readDataBuffer, readDataSize, &strBase64);
	if ( NULL == strBase64 )
	{
		delete [] readDataBuffer;
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
		return result;

	std::string ssDateTime(strDateTime);
	delete [] strDateTime;

	if ( NULL != pOut )
	{
		pOut->webMemberID = ssMemberID;
		pOut->webPassword = ssWebPass;
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

