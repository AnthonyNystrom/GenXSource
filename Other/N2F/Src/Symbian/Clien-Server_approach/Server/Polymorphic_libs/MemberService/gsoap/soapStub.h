/* soapStub.h
   Generated by gSOAP 2.7.10 from output\memberservice.h
   Copyright(C) 2000-2008, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#ifndef soapStub_H
#define soapStub_H
#include "stdsoap2.h"

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

#ifndef SOAP_TYPE__ns1__GetEncryptionKey
#define SOAP_TYPE__ns1__GetEncryptionKey (7)
/* ns1:GetEncryptionKey */
class SOAP_CMAC _ns1__GetEncryptionKey
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 7; } /* = unique id SOAP_TYPE__ns1__GetEncryptionKey */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetEncryptionKey() : WebMemberID(NULL), WebPassword(NULL), soap(NULL) { }
	virtual ~_ns1__GetEncryptionKey() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetEncryptionKeyResponse
#define SOAP_TYPE__ns1__GetEncryptionKeyResponse (8)
/* ns1:GetEncryptionKeyResponse */
class SOAP_CMAC _ns1__GetEncryptionKeyResponse
{
public:
	char *GetEncryptionKeyResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 8; } /* = unique id SOAP_TYPE__ns1__GetEncryptionKeyResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetEncryptionKeyResponse() : GetEncryptionKeyResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetEncryptionKeyResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetMemberID
#define SOAP_TYPE__ns1__GetMemberID (9)
/* ns1:GetMemberID */
class SOAP_CMAC _ns1__GetMemberID
{
public:
	char *NickName;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 9; } /* = unique id SOAP_TYPE__ns1__GetMemberID */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetMemberID() : NickName(NULL), WebPassword(NULL), soap(NULL) { }
	virtual ~_ns1__GetMemberID() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetMemberIDResponse
#define SOAP_TYPE__ns1__GetMemberIDResponse (10)
/* ns1:GetMemberIDResponse */
class SOAP_CMAC _ns1__GetMemberIDResponse
{
public:
	char *GetMemberIDResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 10; } /* = unique id SOAP_TYPE__ns1__GetMemberIDResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetMemberIDResponse() : GetMemberIDResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetMemberIDResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetTagID
#define SOAP_TYPE__ns1__GetTagID (11)
/* ns1:GetTagID */
class SOAP_CMAC _ns1__GetTagID
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 11; } /* = unique id SOAP_TYPE__ns1__GetTagID */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetTagID() : WebMemberID(NULL), WebPassword(NULL), soap(NULL) { }
	virtual ~_ns1__GetTagID() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetTagIDResponse
#define SOAP_TYPE__ns1__GetTagIDResponse (12)
/* ns1:GetTagIDResponse */
class SOAP_CMAC _ns1__GetTagIDResponse
{
public:
	char *GetTagIDResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 12; } /* = unique id SOAP_TYPE__ns1__GetTagIDResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetTagIDResponse() : GetTagIDResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetTagIDResponse() { }
};
#endif

#ifndef SOAP_TYPE___ns2__GetEncryptionKey
#define SOAP_TYPE___ns2__GetEncryptionKey (17)
/* Operation wrapper: */
struct __ns2__GetEncryptionKey
{
public:
	_ns1__GetEncryptionKey *ns1__GetEncryptionKey;	/* optional element of type ns1:GetEncryptionKey */
};
#endif

#ifndef SOAP_TYPE___ns2__GetMemberID
#define SOAP_TYPE___ns2__GetMemberID (21)
/* Operation wrapper: */
struct __ns2__GetMemberID
{
public:
	_ns1__GetMemberID *ns1__GetMemberID;	/* optional element of type ns1:GetMemberID */
};
#endif

#ifndef SOAP_TYPE___ns2__GetTagID
#define SOAP_TYPE___ns2__GetTagID (25)
/* Operation wrapper: */
struct __ns2__GetTagID
{
public:
	_ns1__GetTagID *ns1__GetTagID;	/* optional element of type ns1:GetTagID */
};
#endif

#ifndef SOAP_TYPE___ns3__GetEncryptionKey
#define SOAP_TYPE___ns3__GetEncryptionKey (27)
/* Operation wrapper: */
struct __ns3__GetEncryptionKey
{
public:
	_ns1__GetEncryptionKey *ns1__GetEncryptionKey;	/* optional element of type ns1:GetEncryptionKey */
};
#endif

#ifndef SOAP_TYPE___ns3__GetMemberID
#define SOAP_TYPE___ns3__GetMemberID (29)
/* Operation wrapper: */
struct __ns3__GetMemberID
{
public:
	_ns1__GetMemberID *ns1__GetMemberID;	/* optional element of type ns1:GetMemberID */
};
#endif

#ifndef SOAP_TYPE___ns3__GetTagID
#define SOAP_TYPE___ns3__GetTagID (31)
/* Operation wrapper: */
struct __ns3__GetTagID
{
public:
	_ns1__GetTagID *ns1__GetTagID;	/* optional element of type ns1:GetTagID */
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Header
#define SOAP_TYPE_SOAP_ENV__Header (32)
/* SOAP Header: */
struct SOAP_ENV__Header
{
#ifdef WITH_NOEMPTYSTRUCT
private:
	char dummy;	/* dummy member to enable compilation */
#endif
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Code
#define SOAP_TYPE_SOAP_ENV__Code (33)
/* SOAP Fault Code: */
struct SOAP_ENV__Code
{
public:
	char *SOAP_ENV__Value;	/* optional element of type xsd:QName */
	struct SOAP_ENV__Code *SOAP_ENV__Subcode;	/* optional element of type SOAP-ENV:Code */
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Detail
#define SOAP_TYPE_SOAP_ENV__Detail (35)
/* SOAP-ENV:Detail */
struct SOAP_ENV__Detail
{
public:
	int __type;	/* any type of element <fault> (defined below) */
	void *fault;	/* transient */
	char *__any;
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Reason
#define SOAP_TYPE_SOAP_ENV__Reason (38)
/* SOAP-ENV:Reason */
struct SOAP_ENV__Reason
{
public:
	char *SOAP_ENV__Text;	/* optional element of type xsd:string */
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Fault
#define SOAP_TYPE_SOAP_ENV__Fault (39)
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

#ifndef SOAP_TYPE__QName
#define SOAP_TYPE__QName (5)
typedef char *_QName;
#endif

#ifndef SOAP_TYPE__XML
#define SOAP_TYPE__XML (6)
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


#endif

/* End of soapStub.h */