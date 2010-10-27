/* WS_MemberServicesStub.h
   Generated by gSOAP 2.7.10 from .\wsdl-h\MemberServices.h
   Copyright(C) 2000-2008, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#ifndef WS_MemberServicesStub_H
#define WS_MemberServicesStub_H
#include <vector>
#ifndef WITH_NONAMESPACES
#define WITH_NONAMESPACES
#endif
#ifndef WITH_NOGLOBAL
#define WITH_NOGLOBAL
#endif
#include "stdsoap2.h"

namespace WS_MemberServices {

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

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__GetEncryptionKey
#define SOAP_TYPE_WS_MemberServices__MemberServices__GetEncryptionKey (8)
/* MemberServices:GetEncryptionKey */
class SOAP_CMAC _MemberServices__GetEncryptionKey
{
public:
	std::string *WebMemberID;	/* optional element of type xsd:string */
	std::string *Password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 8; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__GetEncryptionKey */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__GetEncryptionKey() : WebMemberID(NULL), Password(NULL), soap(NULL) { }
	virtual ~_MemberServices__GetEncryptionKey() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__GetEncryptionKeyResponse
#define SOAP_TYPE_WS_MemberServices__MemberServices__GetEncryptionKeyResponse (9)
/* MemberServices:GetEncryptionKeyResponse */
class SOAP_CMAC _MemberServices__GetEncryptionKeyResponse
{
public:
	std::string *GetEncryptionKeyResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 9; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__GetEncryptionKeyResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__GetEncryptionKeyResponse() : GetEncryptionKeyResult(NULL), soap(NULL) { }
	virtual ~_MemberServices__GetEncryptionKeyResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__GetMemberID
#define SOAP_TYPE_WS_MemberServices__MemberServices__GetMemberID (10)
/* MemberServices:GetMemberID */
class SOAP_CMAC _MemberServices__GetMemberID
{
public:
	std::string *NickName;	/* optional element of type xsd:string */
	std::string *Password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 10; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__GetMemberID */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__GetMemberID() : NickName(NULL), Password(NULL), soap(NULL) { }
	virtual ~_MemberServices__GetMemberID() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__GetMemberIDResponse
#define SOAP_TYPE_WS_MemberServices__MemberServices__GetMemberIDResponse (11)
/* MemberServices:GetMemberIDResponse */
class SOAP_CMAC _MemberServices__GetMemberIDResponse
{
public:
	std::string *GetMemberIDResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 11; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__GetMemberIDResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__GetMemberIDResponse() : GetMemberIDResult(NULL), soap(NULL) { }
	virtual ~_MemberServices__GetMemberIDResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__GetTagID
#define SOAP_TYPE_WS_MemberServices__MemberServices__GetTagID (12)
/* MemberServices:GetTagID */
class SOAP_CMAC _MemberServices__GetTagID
{
public:
	std::string *WebMemberID;	/* optional element of type xsd:string */
	std::string *Password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 12; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__GetTagID */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__GetTagID() : WebMemberID(NULL), Password(NULL), soap(NULL) { }
	virtual ~_MemberServices__GetTagID() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__GetTagIDResponse
#define SOAP_TYPE_WS_MemberServices__MemberServices__GetTagIDResponse (13)
/* MemberServices:GetTagIDResponse */
class SOAP_CMAC _MemberServices__GetTagIDResponse
{
public:
	std::string *GetTagIDResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 13; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__GetTagIDResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__GetTagIDResponse() : GetTagIDResult(NULL), soap(NULL) { }
	virtual ~_MemberServices__GetTagIDResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__CheckUserExists
#define SOAP_TYPE_WS_MemberServices__MemberServices__CheckUserExists (14)
/* MemberServices:CheckUserExists */
class SOAP_CMAC _MemberServices__CheckUserExists
{
public:
	std::string *NickName;	/* optional element of type xsd:string */
	std::string *Password;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 14; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__CheckUserExists */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__CheckUserExists() : NickName(NULL), Password(NULL), soap(NULL) { }
	virtual ~_MemberServices__CheckUserExists() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices__MemberServices__CheckUserExistsResponse
#define SOAP_TYPE_WS_MemberServices__MemberServices__CheckUserExistsResponse (15)
/* MemberServices:CheckUserExistsResponse */
class SOAP_CMAC _MemberServices__CheckUserExistsResponse
{
public:
	bool CheckUserExistsResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* required element of type xsd:boolean */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 15; } /* = unique id SOAP_TYPE_WS_MemberServices__MemberServices__CheckUserExistsResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _MemberServices__CheckUserExistsResponse() : CheckUserExistsResult((bool)0), soap(NULL) { }
	virtual ~_MemberServices__CheckUserExistsResponse() { }
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices2__GetEncryptionKey
#define SOAP_TYPE_WS_MemberServices___MemberServices2__GetEncryptionKey (23)
/* Operation wrapper: */
struct __MemberServices2__GetEncryptionKey
{
public:
	_MemberServices__GetEncryptionKey *MemberServices__GetEncryptionKey;	/* optional element of type MemberServices:GetEncryptionKey */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices2__GetMemberID
#define SOAP_TYPE_WS_MemberServices___MemberServices2__GetMemberID (27)
/* Operation wrapper: */
struct __MemberServices2__GetMemberID
{
public:
	_MemberServices__GetMemberID *MemberServices__GetMemberID;	/* optional element of type MemberServices:GetMemberID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices2__GetTagID
#define SOAP_TYPE_WS_MemberServices___MemberServices2__GetTagID (31)
/* Operation wrapper: */
struct __MemberServices2__GetTagID
{
public:
	_MemberServices__GetTagID *MemberServices__GetTagID;	/* optional element of type MemberServices:GetTagID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices2__CheckUserExists
#define SOAP_TYPE_WS_MemberServices___MemberServices2__CheckUserExists (35)
/* Operation wrapper: */
struct __MemberServices2__CheckUserExists
{
public:
	_MemberServices__CheckUserExists *MemberServices__CheckUserExists;	/* optional element of type MemberServices:CheckUserExists */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices3__GetEncryptionKey
#define SOAP_TYPE_WS_MemberServices___MemberServices3__GetEncryptionKey (37)
/* Operation wrapper: */
struct __MemberServices3__GetEncryptionKey
{
public:
	_MemberServices__GetEncryptionKey *MemberServices__GetEncryptionKey;	/* optional element of type MemberServices:GetEncryptionKey */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices3__GetMemberID
#define SOAP_TYPE_WS_MemberServices___MemberServices3__GetMemberID (39)
/* Operation wrapper: */
struct __MemberServices3__GetMemberID
{
public:
	_MemberServices__GetMemberID *MemberServices__GetMemberID;	/* optional element of type MemberServices:GetMemberID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices3__GetTagID
#define SOAP_TYPE_WS_MemberServices___MemberServices3__GetTagID (41)
/* Operation wrapper: */
struct __MemberServices3__GetTagID
{
public:
	_MemberServices__GetTagID *MemberServices__GetTagID;	/* optional element of type MemberServices:GetTagID */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices___MemberServices3__CheckUserExists
#define SOAP_TYPE_WS_MemberServices___MemberServices3__CheckUserExists (43)
/* Operation wrapper: */
struct __MemberServices3__CheckUserExists
{
public:
	_MemberServices__CheckUserExists *MemberServices__CheckUserExists;	/* optional element of type MemberServices:CheckUserExists */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices_SOAP_ENV__Header
#define SOAP_TYPE_WS_MemberServices_SOAP_ENV__Header (44)
/* SOAP Header: */
struct SOAP_ENV__Header
{
#ifdef WITH_NOEMPTYSTRUCT
private:
	char dummy;	/* dummy member to enable compilation */
#endif
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices_SOAP_ENV__Code
#define SOAP_TYPE_WS_MemberServices_SOAP_ENV__Code (45)
/* SOAP Fault Code: */
struct SOAP_ENV__Code
{
public:
	char *SOAP_ENV__Value;	/* optional element of type xsd:QName */
	struct SOAP_ENV__Code *SOAP_ENV__Subcode;	/* optional element of type SOAP-ENV:Code */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices_SOAP_ENV__Detail
#define SOAP_TYPE_WS_MemberServices_SOAP_ENV__Detail (47)
/* SOAP-ENV:Detail */
struct SOAP_ENV__Detail
{
public:
	int __type;	/* any type of element <fault> (defined below) */
	void *fault;	/* transient */
	char *__any;
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices_SOAP_ENV__Reason
#define SOAP_TYPE_WS_MemberServices_SOAP_ENV__Reason (50)
/* SOAP-ENV:Reason */
struct SOAP_ENV__Reason
{
public:
	char *SOAP_ENV__Text;	/* optional element of type xsd:string */
};
#endif

#ifndef SOAP_TYPE_WS_MemberServices_SOAP_ENV__Fault
#define SOAP_TYPE_WS_MemberServices_SOAP_ENV__Fault (51)
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

#ifndef SOAP_TYPE_WS_MemberServices__QName
#define SOAP_TYPE_WS_MemberServices__QName (5)
typedef char *_QName;
#endif

#ifndef SOAP_TYPE_WS_MemberServices__XML
#define SOAP_TYPE_WS_MemberServices__XML (6)
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


SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices2__GetEncryptionKey(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__GetEncryptionKey *MemberServices__GetEncryptionKey, _MemberServices__GetEncryptionKeyResponse *MemberServices__GetEncryptionKeyResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices2__GetMemberID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__GetMemberID *MemberServices__GetMemberID, _MemberServices__GetMemberIDResponse *MemberServices__GetMemberIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices2__GetTagID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__GetTagID *MemberServices__GetTagID, _MemberServices__GetTagIDResponse *MemberServices__GetTagIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices2__CheckUserExists(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__CheckUserExists *MemberServices__CheckUserExists, _MemberServices__CheckUserExistsResponse *MemberServices__CheckUserExistsResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices3__GetEncryptionKey(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__GetEncryptionKey *MemberServices__GetEncryptionKey, _MemberServices__GetEncryptionKeyResponse *MemberServices__GetEncryptionKeyResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices3__GetMemberID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__GetMemberID *MemberServices__GetMemberID, _MemberServices__GetMemberIDResponse *MemberServices__GetMemberIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices3__GetTagID(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__GetTagID *MemberServices__GetTagID, _MemberServices__GetTagIDResponse *MemberServices__GetTagIDResponse);

SOAP_FMAC5 int SOAP_FMAC6 soap_call___MemberServices3__CheckUserExists(struct soap *soap, const char *soap_endpoint, const char *soap_action, _MemberServices__CheckUserExists *MemberServices__CheckUserExists, _MemberServices__CheckUserExistsResponse *MemberServices__CheckUserExistsResponse);

} // namespace WS_MemberServices


#endif

/* End of WS_MemberServicesStub.h */
