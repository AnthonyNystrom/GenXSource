#pragma once

#include "webservicebase.h"

namespace WS_MemberServices {
	class MemberServicesSoap;
}

//! Structure holds params for GetMemberID call
struct WS_MS_GetMemberID_InitParams;

//! WebServiceN2FMemberService class 

/*!
	WebServiceN2FMemberService class provides wrapper for N2F MemberServices web-service
	@ http://next2friends.com:90/memberservices.asmx
*/

class WebServiceN2FMemberService:
				public WebServiceBase
{
public:

	//! WebServiceN2FMemberService constructor
	N2FCORE_API WebServiceN2FMemberService();

	//! WebServiceN2FMemberService destructor
	N2FCORE_API virtual ~WebServiceN2FMemberService();

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
		WS_MS_GetMemberID_InitParams *pOut);

private:

	WS_MemberServices::MemberServicesSoap	*iMemberServices;

};
