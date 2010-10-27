#include "stdafx.h"

#include <webservice-n2f-memberservice-v3.h>
#include <controllerutil.h>

#include <stdsoap2.h>
#include <WS_MemberService_v3MemberServiceSoapProxy.h>

struct WS_MSv3_CheckUserExists_InitParams
{
	std::string nick;
	std::string pass;
};


N2FCORE_API WebServiceN2FMemberService_v3::WebServiceN2FMemberService_v3()
		:WebServiceBase(EWS_N2F_MemberService_v3),
		iMemberService(NULL)
{
	LOGMSG("Constructed");
}

N2FCORE_API WebServiceN2FMemberService_v3::~WebServiceN2FMemberService_v3()
{
	if ( NULL != iMemberService )
		delete iMemberService;

	LOGMSG("Destructed");
}

N2FCORE_API	 bool WebServiceN2FMemberService_v3::Initialize( CString& customEndPoint )
{
	LOGMSG("Initializing!");

	WebServiceBase::Initialize( customEndPoint );

	LOGMSG("Initialized!");

	return true;
}

N2FCORE_API  bool WebServiceN2FMemberService_v3::CheckUserExists( CString& nickname, CString& password, bool& doesUserExist )
{
	bool result = false;

	iMemberService = new WS_MemberService_v3::MemberServiceSoap();
	ASSERT(iMemberService);
	if ( NULL == iMemberService )
	{
		return result;
	}

	doesUserExist = false;

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

		strOriginalEndPoint = iMemberService->endpoint;
		iMemberService->endpoint = strCustomEndPoint;
	}

	WS_MSv3_CheckUserExists_InitParams *initParams = new WS_MSv3_CheckUserExists_InitParams;
	if ( false == this->PrepareParams_CheckUserExists(nickname, password, initParams) )
	{
		delete initParams;
		return result;
	}

	// init soap params
	WS_MemberService_v3::_MemberServiceV3__CheckUserExists	*pParams = NULL;
	WS_MemberService_v3::_MemberServiceV3__CheckUserExistsResponse	*pResponse = NULL;

	size_t szParams = sizeof(WS_MemberService_v3::_MemberServiceV3__CheckUserExists);
	size_t szResponse = sizeof(WS_MemberService_v3::_MemberServiceV3__CheckUserExistsResponse);

	pParams = WS_MemberService_v3::soap_instantiate__MemberServiceV3__CheckUserExists(iMemberService->soap,
		SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExists, "", "", &szParams);

	pResponse = WS_MemberService_v3::soap_instantiate__MemberServiceV3__CheckUserExistsResponse(iMemberService->soap,
		SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExistsResponse, "", "", &szResponse);

	if ( NULL != pParams && NULL != pResponse )
	{

		pParams->nickname = &(initParams->nick);
		pParams->password = &(initParams->pass);

		int soapResult = 0;
		LOGMSG("Calling CheckUserExists for nick=%s, password=%s", nickname, password);

		soapResult = iMemberService->__MemberServiceV32__CheckUserExists(pParams, pResponse);

		if ( SOAP_OK == soapResult )
		{
			doesUserExist = pResponse->CheckUserExistsResult;
			LOGMSG("CheckUserExists succeeded with result = %s", doesUserExist? _T("true"): _T("false"));
			result = true;
		}
		else
		{
			LOGMSG("CheckUserExists failed with code = %d", soapResult);
		}

		this->SetLastSoapResult(soapResult);
	}

	if ( NULL != strCustomEndPoint )
	{
		delete [] strCustomEndPoint;
		iMemberService->endpoint = NULL;
	}

	if ( NULL != strOriginalEndPoint )
		iMemberService->endpoint = strOriginalEndPoint;

	delete iMemberService;
	iMemberService = NULL;

	return result;
}

N2FCORE_API bool WebServiceN2FMemberService_v3::PrepareParams_CheckUserExists( CString& nickname, CString& password, WS_MSv3_CheckUserExists_InitParams *pOut )
{
	bool result = false;

	char *strNick = NULL;
	char *strPassword = NULL;

	bool convResult = ControllerUtil::ConvertWideCharToMultiByte(nickname, -1, &strNick, NULL);
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
