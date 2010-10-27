/* WS_MemberService_v3Stub.h
   Generated by gSOAP 2.7.10 from .\wsdl-h\MemberService.v3.h
   Copyright(C) 2000-2008, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#ifndef WS_MemberService_v3Stub_H
#define WS_MemberService_v3Stub_H
#include <vector>
#ifndef WITH_NONAMESPACES
#define WITH_NONAMESPACES
#endif
#ifndef WITH_NOGLOBAL
#define WITH_NOGLOBAL
#endif
#include "stdsoap2.h"

namespace WS_MemberService_v3 {

/******************************************************************************\
 *                                                                            *
 * Enumerations                                                               *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * Classes and Structs                                                        *
 *                                                                            *
\******************************************************************************/


#if 0 /* volatile type: do not redeclare here */

#endif

#if 0 /* volatile type: do not redeclare here */

#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExists
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExists (8)
/* MemberServiceV3:CheckUserExists */
class SOAP_CMAC _MemberServiceV3__CheckUserExists
{
public:
	std::string *nickname;	/* optional element of type xsd:string */
	std::string *password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 8; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExists */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__CheckUserExists() : nickname(NULL), password(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__CheckUserExists() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExistsResponse
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExistsResponse (9)
/* MemberServiceV3:CheckUserExistsResponse */
class SOAP_CMAC _MemberServiceV3__CheckUserExistsResponse
{
public:
	bool CheckUserExistsResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* required element of type xsd:boolean */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 9; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__CheckUserExistsResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__CheckUserExistsResponse() : CheckUserExistsResult((bool)0), soap(NULL) { }
	virtual ~_MemberServiceV3__CheckUserExistsResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetEncryptionKey
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetEncryptionKey (10)
/* MemberServiceV3:GetEncryptionKey */
class SOAP_CMAC _MemberServiceV3__GetEncryptionKey
{
public:
	std::string *nickname;	/* optional element of type xsd:string */
	std::string *password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 10; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetEncryptionKey */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__GetEncryptionKey() : nickname(NULL), password(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__GetEncryptionKey() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetEncryptionKeyResponse
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetEncryptionKeyResponse (11)
/* MemberServiceV3:GetEncryptionKeyResponse */
class SOAP_CMAC _MemberServiceV3__GetEncryptionKeyResponse
{
public:
	std::string *GetEncryptionKeyResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 11; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetEncryptionKeyResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__GetEncryptionKeyResponse() : GetEncryptionKeyResult(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__GetEncryptionKeyResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetMemberID
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetMemberID (12)
/* MemberServiceV3:GetMemberID */
class SOAP_CMAC _MemberServiceV3__GetMemberID
{
public:
	std::string *nickname;	/* optional element of type xsd:string */
	std::string *password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 12; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetMemberID */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__GetMemberID() : nickname(NULL), password(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__GetMemberID() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetMemberIDResponse
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetMemberIDResponse (13)
/* MemberServiceV3:GetMemberIDResponse */
class SOAP_CMAC _MemberServiceV3__GetMemberIDResponse
{
public:
	std::string *GetMemberIDResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 13; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetMemberIDResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__GetMemberIDResponse() : GetMemberIDResult(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__GetMemberIDResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetTagID
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetTagID (14)
/* MemberServiceV3:GetTagID */
class SOAP_CMAC _MemberServiceV3__GetTagID
{
public:
	std::string *nickname;	/* optional element of type xsd:string */
	std::string *password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 14; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetTagID */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__GetTagID() : nickname(NULL), password(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__GetTagID() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetTagIDResponse
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetTagIDResponse (15)
/* MemberServiceV3:GetTagIDResponse */
class SOAP_CMAC _MemberServiceV3__GetTagIDResponse
{
public:
	std::string *GetTagIDResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 15; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__GetTagIDResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__GetTagIDResponse() : GetTagIDResult(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__GetTagIDResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__RemindPassword
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__RemindPassword (16)
/* MemberServiceV3:RemindPassword */
class SOAP_CMAC _MemberServiceV3__RemindPassword
{
public:
	std::string *emailAddress;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 16; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__RemindPassword */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__RemindPassword() : emailAddress(NULL), soap(NULL) { }
	virtual ~_MemberServiceV3__RemindPassword() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__RemindPasswordResponse
#define SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__RemindPasswordResponse (17)
/* MemberServiceV3:RemindPasswordResponse */
class SOAP_CMAC _MemberServiceV3__RemindPasswordResponse
{
public:
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 17; } /* = unique id SOAP_TYPE_WS_MemberService_v3__MemberServiceV3__RemindPasswordResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServiceV3__RemindPasswordResponse() : soap(NULL) { }
	virtual ~_MemberServiceV3__RemindPasswordResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__CheckUserExists
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__CheckUserExists (25)
/* Operation wrapper: */
struct __MemberServiceV32__CheckUserExists
{
public:
	_MemberServiceV3__CheckUserExists *MemberServiceV3__CheckUserExists;	/* optional element of type MemberServiceV3:CheckUserExists */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__GetEncryptionKey
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__GetEncryptionKey (29)
/* Operation wrapper: */
struct __MemberServiceV32__GetEncryptionKey
{
public:
	_MemberServiceV3__GetEncryptionKey *MemberServiceV3__GetEncryptionKey;	/* optional element of type MemberServiceV3:GetEncryptionKey */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__GetMemberID
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__GetMemberID (33)
/* Operation wrapper: */
struct __MemberServiceV32__GetMemberID
{
public:
	_MemberServiceV3__GetMemberID *MemberServiceV3__GetMemberID;	/* optional element of type MemberServiceV3:GetMemberID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__GetTagID
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__GetTagID (37)
/* Operation wrapper: */
struct __MemberServiceV32__GetTagID
{
public:
	_MemberServiceV3__GetTagID *MemberServiceV3__GetTagID;	/* optional element of type MemberServiceV3:GetTagID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__RemindPassword
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV32__RemindPassword (41)
/* Operation wrapper: */
struct __MemberServiceV32__RemindPassword
{
public:
	_MemberServiceV3__RemindPassword *MemberServiceV3__RemindPassword;	/* optional element of type MemberServiceV3:RemindPassword */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__CheckUserExists
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__CheckUserExists (43)
/* Operation wrapper: */
struct __MemberServiceV33__CheckUserExists
{
public:
	_MemberServiceV3__CheckUserExists *MemberServiceV3__CheckUserExists;	/* optional element of type MemberServiceV3:CheckUserExists */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__GetEncryptionKey
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__GetEncryptionKey (45)
/* Operation wrapper: */
struct __MemberServiceV33__GetEncryptionKey
{
public:
	_MemberServiceV3__GetEncryptionKey *MemberServiceV3__GetEncryptionKey;	/* optional element of type MemberServiceV3:GetEncryptionKey */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__GetMemberID
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__GetMemberID (47)
/* Operation wrapper: */
struct __MemberServiceV33__GetMemberID
{
public:
	_MemberServiceV3__GetMemberID *MemberServiceV3__GetMemberID;	/* optional element of type MemberServiceV3:GetMemberID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__GetTagID
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__GetTagID (49)
/* Operation wrapper: */
struct __MemberServiceV33__GetTagID
{
public:
	_MemberServiceV3__GetTagID *MemberServiceV3__GetTagID;	/* optional element of type MemberServiceV3:GetTagID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__RemindPassword
#define SOAP_TYPE_WS_MemberService_v3___MemberServiceV33__RemindPassword (51)
/* Operation wrapper: */
struct __MemberServiceV33__RemindPassword
{
public:
	_MemberServiceV3__RemindPassword *MemberServiceV3__RemindPassword;	/* optional element of type MemberServiceV3:RemindPassword */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Header
#define SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Header (52)
/* SOAP Header: */
struct SOAP_ENV__Header
{
#ifdef WITH_NOEMPTYSTRUCT
private:
	char dummy;	/* dummy member to enable compilation */
#endif
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Code
#define SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Code (53)
/* SOAP Fault Code: */
struct SOAP_ENV__Code
{
public:
	char *SOAP_ENV__Value;	/* optional element of type xsd:QName */
	struct SOAP_ENV__Code *SOAP_ENV__Subcode;	/* optional element of type SOAP-ENV:Code */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Detail
#define SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Detail (55)
/* SOAP-ENV:Detail */
struct SOAP_ENV__Detail
{
public:
	int __type;	/* any type of element <fault> (defined below) */
	void *fault;	/* transient */
	char *__any;
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Reason
#define SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Reason (58)
/* SOAP-ENV:Reason */
struct SOAP_ENV__Reason
{
public:
	char *SOAP_ENV__Text;	/* optional element of type xsd:string */
};
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Fault
#define SOAP_TYPE_WS_MemberService_v3_SOAP_ENV__Fault (59)
/* SOAP Fault: */
struct SOAP_ENV__Fault
{
public:
	char *faultcode;	/* optional element of type xsd:QName */
	char *faultstring;	/* optional element of type xsd:string */
	char *faultactor;	/* optional element of type xsd:string */
	struct SOAP_ENV__Detail *detail;	/* optional element of type SOAP-ENV:Detail */
	struct SOAP_ENV__Code *SOAP_ENV__Code;	/* optional element of type SOAP-ENV:Code */
	struct SOAP_ENV__Reason *SOAP_ENV__Reason;	/* optional element of type SOAP-ENV:Reason */
	char *SOAP_ENV__Node;	/* optional element of type xsd:string */
	char *SOAP_ENV__Role;	/* optional element of type xsd:string */
	struct SOAP_ENV__Detail *SOAP_ENV__Detail;	/* optional element of type SOAP-ENV:Detail */
};
#endif

/******************************************************************************\
 *                                                                            *
 * Types with Custom Serializers                                              *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * Typedefs                                                                   *
 *                                                                            *
\******************************************************************************/

#ifndef SOAP_TYPE_WS_MemberService_v3__QName
#define SOAP_TYPE_WS_MemberService_v3__QName (5)
typedef char *_QName;
#endif

#ifndef SOAP_TYPE_WS_MemberService_v3__XML
#define SOAP_TYPE_WS_MemberService_v3__XML (6)
typedef char *_XML;
#endif


/******************************************************************************\
 *                                                                            *
 * Typedef Synonyms                                                           *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * Externals                                                                  *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * Stubs                                                                      *
 *                                                                            *
\******************************************************************************/


SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV32__CheckUserExists(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__CheckUserExists *MemberServiceV3__CheckUserExists, _MemberServiceV3__CheckUserExistsResponse *MemberServiceV3__CheckUserExistsResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV32__GetEncryptionKey(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__GetEncryptionKey *MemberServiceV3__GetEncryptionKey, _MemberServiceV3__GetEncryptionKeyResponse *MemberServiceV3__GetEncryptionKeyResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV32__GetMemberID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__GetMemberID *MemberServiceV3__GetMemberID, _MemberServiceV3__GetMemberIDResponse *MemberServiceV3__GetMemberIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV32__GetTagID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__GetTagID *MemberServiceV3__GetTagID, _MemberServiceV3__GetTagIDResponse *MemberServiceV3__GetTagIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV32__RemindPassword(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__RemindPassword *MemberServiceV3__RemindPassword, _MemberServiceV3__RemindPasswordResponse *MemberServiceV3__RemindPasswordResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV33__CheckUserExists(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__CheckUserExists *MemberServiceV3__CheckUserExists, _MemberServiceV3__CheckUserExistsResponse *MemberServiceV3__CheckUserExistsResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV33__GetEncryptionKey(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__GetEncryptionKey *MemberServiceV3__GetEncryptionKey, _MemberServiceV3__GetEncryptionKeyResponse *MemberServiceV3__GetEncryptionKeyResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV33__GetMemberID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__GetMemberID *MemberServiceV3__GetMemberID, _MemberServiceV3__GetMemberIDResponse *MemberServiceV3__GetMemberIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV33__GetTagID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__GetTagID *MemberServiceV3__GetTagID, _MemberServiceV3__GetTagIDResponse *MemberServiceV3__GetTagIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServiceV33__RemindPassword(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServiceV3__RemindPassword *MemberServiceV3__RemindPassword, _MemberServiceV3__RemindPasswordResponse *MemberServiceV3__RemindPasswordResponse);

} // namespace WS_MemberService_v3


#endif

/* End of WS_MemberService_v3Stub.h */
