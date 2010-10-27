#include "stdafx.h"
#include "webservice-n2f-memberservice-v2.h"

#include "controllerutil.h"

#include <stdsoap2.h>
#include <WS_MemberServices_v2MemberServicesSoapProxy.h>

struct WS_MSv2_GetMemberID_InitParams
{
	std::string nick;
	std::string pass;
};

N2FCORE_API WebServiceN2FMemberService_v2::WebServiceN2FMemberService_v2()
	:WebServiceBase(EWS_N2F_MemberServices_v2),
	iMemberServices(NULL)
{
	LOGMSG("Constructed");
}

N2FCORE_API WebServiceN2FMemberService_v2::~WebServiceN2FMemberService_v2()
{
	if ( NULL != iMemberServices )
		delete iMemberServices;

	LOGMSG("Destructed");
}

N2FCORE_API bool WebServiceN2FMemberService_v2::Initialize( CString& customEndPoint )
{
	LOGMSG("Initializing!");

	WebServiceBase::Initialize( customEndPoint );

	//iMemberServices = new WS_MemberServices_v2::MemberServicesSoap();
	//ASSERT(iMemberServices);
	//if ( NULL == iMemberServices )
	//{
	//	return false;
	//}

	LOGMSG("Initialized!");

	return true;
}

N2FCORE_API bool WebServiceN2FMemberService_v2::GetMemberId( CString& nick, CString& password, CString& memberId )
{
	bool result = false;

	iMemberServices = new WS_MemberServices_v2::MemberServicesSoap();
	ASSERT(iMemberServices);
	if ( NULL == iMemberServices )
	{
		return result;
	}

	memberId.Empty();


	char *strCustomEndPoint = NULL;
	const char* strOriginalEndPoint = NULL;
	

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

		strOriginalEndPoint = iMemberServices->endpoint;
		iMemberServices->endpoint = strCustomEndPoint;
	}

	WS_MSv2_GetMemberID_InitParams *initParams = new WS_MSv2_GetMemberID_InitParams;
	if ( false == this->PrepareParams_GetMemberId(nick, password, initParams) )
	{
		delete initParams;
		return result;
	}

	// init soap params

	WS_MemberServices_v2::_MemberServicesV2__GetMemberID *pParams = NULL;
	WS_MemberServices_v2::_MemberServicesV2__GetMemberIDResponse *pResponse = NULL;

	size_t szParams = sizeof(WS_MemberServices_v2::_MemberServicesV2__GetMemberID);
	size_t szResponse = sizeof(WS_MemberServices_v2::_MemberServicesV2__GetMemberIDResponse);

	pParams = WS_MemberServices_v2::soap_instantiate__MemberServicesV2__GetMemberID(iMemberServices->soap,
		SOAP_TYPE_WS_MemberServices_v2__MemberServicesV2__GetMemberID, "", "", &szParams);

	

	pResponse = WS_MemberServices_v2::soap_instantiate__MemberServicesV2__GetMemberIDResponse(iMemberServices->soap,
		SOAP_TYPE_WS_MemberServices_v2__MemberServicesV2__GetMemberIDResponse, "", "", &szResponse);

	if ( NULL != pParams && NULL != pResponse )
	{

		pParams->NickName = &(initParams->nick);
		pParams->WebPassword = &(initParams->pass);

		int soapResult = 0;
		LOGMSG("Calling GetMemberId for nick=%s, password=%s", nick, password);
		
		soapResult = iMemberServices->__MemberServicesV22__GetMemberID(pParams, pResponse);

		if ( SOAP_OK == soapResult )
		{
			memberId = CString(pResponse->GetMemberIDResult->c_str());
			LOGMSG("GetMemberId succeeded with result = %s", memberId);
			result = true;
		}
		else
		{
			LOGMSG("GetMemberId failed with code = %d", soapResult);
		}

		this->SetLastSoapResult(soapResult);
	}

	if ( NULL != strCustomEndPoint )
	{
		delete [] strCustomEndPoint;
		iMemberServices->endpoint = NULL;
	}

	if ( NULL != strOriginalEndPoint )
		iMemberServices->endpoint = strOriginalEndPoint;

	delete iMemberServices;
	iMemberServices = NULL;

	return result;

}

N2FCORE_API bool WebServiceN2FMemberService_v2::PrepareParams_GetMemberId( CString& nick, CString& password, WS_MSv2_GetMemberID_InitParams *pOut )
{
	bool result = false;

	char *strNick = NULL;
	char *strPassword = NULL;

	bool convResult = ControllerUtil::ConvertWideCharToMultiByte(nick, -1, &strNick, NULL);
	if ( false == convResult || NULL == strNick )
		return result;

	std::string ssNick(strNick);
	delete [] strNick;

	convResult = ControllerUtil::ConvertWideCharToMultiByte(password, -1, &strPassword, NULL);
	if ( false == convResult || NULL == strPassword )
		return result;

	std::string ssPass(strPassword);
	delete [] strPassword;

	if ( NULL != pOut )
	{
		pOut->nick = ssNick;
		pOut->pass = ssPass;

		result = true;
	}

	return result;
}
