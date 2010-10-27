/* WS_MemberServicesMemberServicesSoapProxy.h
   Generated by gSOAP 2.7.10 from .\wsdl-h\MemberServices.h
   Copyright(C) 2000-2008, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#ifndef WS_MemberServicesMemberServicesSoapProxy_H
#define WS_MemberServicesMemberServicesSoapProxy_H
#include "WS_MemberServicesH.h"
extern SOAP_NMAC struct Namespace WS_MemberServices_namespaces[];

namespace WS_MemberServices {
class MemberServicesSoap
{   public:
	/// Runtime engine context allocated in constructor
	struct soap *soap;
	/// Endpoint URL of service 'MemberServicesSoap' (change as needed)
	const char *endpoint;
	/// Constructor allocates soap engine context, sets default endpoint URL, and sets namespace mapping table
	MemberServicesSoap() { soap = soap_new(); if (soap) soap->namespaces = WS_MemberServices_namespaces; endpoint = "http://next2friends.com:90/MemberServices.asmx"; };
	/// Destructor frees deserialized data and soap engine context
	virtual ~MemberServicesSoap() { if (soap) { soap_destroy(soap); soap_end(soap); soap_free(soap); } };
	/// Invoke 'GetEncryptionKey' of service 'MemberServicesSoap' and return error code (or SOAP_OK)
	virtual int __MemberServices2__GetEncryptionKey(_MemberServices__GetEncryptionKey *MemberServices__GetEncryptionKey, _MemberServices__GetEncryptionKeyResponse *MemberServices__GetEncryptionKeyResponse) { return soap ? soap_call___MemberServices2__GetEncryptionKey(soap, endpoint, NULL, MemberServices__GetEncryptionKey, MemberServices__GetEncryptionKeyResponse) : SOAP_EOM; };
	/// Invoke 'GetMemberID' of service 'MemberServicesSoap' and return error code (or SOAP_OK)
	virtual int __MemberServices2__GetMemberID(_MemberServices__GetMemberID *MemberServices__GetMemberID, _MemberServices__GetMemberIDResponse *MemberServices__GetMemberIDResponse) { return soap ? soap_call___MemberServices2__GetMemberID(soap, endpoint, NULL, MemberServices__GetMemberID, MemberServices__GetMemberIDResponse) : SOAP_EOM; };
	/// Invoke 'GetTagID' of service 'MemberServicesSoap' and return error code (or SOAP_OK)
	virtual int __MemberServices2__GetTagID(_MemberServices__GetTagID *MemberServices__GetTagID, _MemberServices__GetTagIDResponse *MemberServices__GetTagIDResponse) { return soap ? soap_call___MemberServices2__GetTagID(soap, endpoint, NULL, MemberServices__GetTagID, MemberServices__GetTagIDResponse) : SOAP_EOM; };
	/// Invoke 'CheckUserExists' of service 'MemberServicesSoap' and return error code (or SOAP_OK)
	virtual int __MemberServices2__CheckUserExists(_MemberServices__CheckUserExists *MemberServices__CheckUserExists, _MemberServices__CheckUserExistsResponse *MemberServices__CheckUserExistsResponse) { return soap ? soap_call___MemberServices2__CheckUserExists(soap, endpoint, NULL, MemberServices__CheckUserExists, MemberServices__CheckUserExistsResponse) : SOAP_EOM; };
};

} // namespace WS_MemberServices

#endif
