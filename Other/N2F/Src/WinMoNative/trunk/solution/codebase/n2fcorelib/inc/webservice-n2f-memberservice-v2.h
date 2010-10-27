#pragma once

#include <webservicebase.h>

namespace WS_MemberServices_v2 {
	class MemberServicesSoap;
}

//! Structure holds params for GetMemberID call
struct WS_MSv2_GetMemberID_InitParams;

//! WebServiceN2FMemberService_v2 class 

/*!
	WebServiceN2FMemberService_v2 class provides wrapper for N2F MemberServices v2 web-service
	@ http://services.next2friends.com/n2fwebservices/memberservices.asmx
*/

class WebServiceN2FMemberService_v2:
	public WebServiceBase
{
public:

	//! WebServiceN2FMemberService constructor
	N2FCORE_API WebServiceN2FMemberService_v2();

	//! WebServiceN2FMemberService destructor
	N2FCORE_API virtual ~WebServiceN2FMemberService_v2();

	//! Web-service initialization method

	/*!
	Wrapped web-service initialization method
	\param customEndPoint a custom end-point to use for web-service, if empty string is passed - default end-point is used
	\return bool as initialization result
	*/
	N2FCORE_API virtual bool Initialize( CString& customEndPoint );


	//! Get Member Id by specified nick & password
	/*! 
	N2F MemberServices service method: GetMemberId
	\param nick member's nick
	\param password member's password
	\param memberId [out] requested member id
	\return bool true - if succeeded, otherwise - false
	*/
	N2FCORE_API virtual bool GetMemberId(CString& nick, CString& password, CString& memberId);

protected:

	//! Initialize parameters for GetMemberID call

	/*!
		Initialize parameters for GetMemberID call
		\param nick member's nick
		\param password member's password
		\param pOut [out] pointer to WS_MS_GetMemberID_InitParams structure, that will hold initialized params
		this structure is allocated by client code
		\see WS_MS_GetMemberID_InitParams
	*/

	N2FCORE_API bool PrepareParams_GetMemberId(CString& nick, CString& password,
		WS_MSv2_GetMemberID_InitParams *pOut);

private:

	WS_MemberServices_v2::MemberServicesSoap	*iMemberServices;

};
