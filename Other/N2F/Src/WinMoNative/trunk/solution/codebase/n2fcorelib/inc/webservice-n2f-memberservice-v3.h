#pragma once

#include <webservicebase.h>

namespace WS_MemberService_v3 {
	class MemberServiceSoap;
}

struct WS_MSv3_CheckUserExists_InitParams;

//! WebServiceN2FMemberService_v3 class 

/*!
	WebServiceN2FMemberService_v3 class provides wrapper for N2F MemberServices v2 web-service
	@ http://next2friends.com:90/MemberService.asmx
*/

class WebServiceN2FMemberService_v3:
				public WebServiceBase
{
public:

	N2FCORE_API	WebServiceN2FMemberService_v3();
	N2FCORE_API	virtual ~WebServiceN2FMemberService_v3();

	N2FCORE_API	virtual bool Initialize( CString& customEndPoint );
	N2FCORE_API virtual bool CheckUserExists( CString& nickname, CString& password, bool& doesUserExist );

protected:

	N2FCORE_API bool PrepareParams_CheckUserExists( CString& nickname, CString& password, 
		WS_MSv3_CheckUserExists_InitParams *pOut );

private:

	WS_MemberService_v3::MemberServiceSoap		*iMemberService;
};
