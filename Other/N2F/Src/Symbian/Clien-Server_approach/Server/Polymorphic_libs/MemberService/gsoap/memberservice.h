/* output\memberservice.h
   Generated by wsdl2h 1.2.10 from http://services.next2friends.com/n2fwebservices/memberservices.asmx?WSDL and typemap.dat
   2008-05-06 20:50:20 GMT
   Copyright (C) 2001-2008 Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL or Genivia's license for commercial use.
*/

/* NOTE:

 - Compile this file with soapcpp2 to complete the code generation process.
 - Use soapcpp2 option -I to specify paths for #import
   To build with STL, 'stlvector.h' is imported from 'import' dir in package.
 - Use wsdl2h options -c and -s to generate pure C code or C++ code without STL.
 - Use 'WS/typemap.dat' to control namespace bindings and type mappings.
   It is strongly recommended to customize the names of the namespace prefixes
   generated by wsdl2h. To do so, modify the prefix bindings in the Namespaces
   section below and add the modified lines to 'typemap.dat' to rerun wsdl2h.
 - Use Doxygen (www.doxygen.org) to browse this file.
 - Use wsdl2h option -l to view the software license terms.

   DO NOT include this file directly into your project.
   Include only the soapcpp2-generated headers and source code files.
*/

//gsoapopt w

/******************************************************************************\
 *                                                                            *
 * http://tempuri.org/                                                        *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * Import                                                                     *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * Schema Namespaces                                                          *
 *                                                                            *
\******************************************************************************/


/* NOTE:

It is strongly recommended to customize the names of the namespace prefixes
generated by wsdl2h. To do so, modify the prefix bindings below and add the
modified lines to typemap.dat to rerun wsdl2h:

ns1 = "http://tempuri.org/"

*/

//gsoap ns1   schema namespace:	http://tempuri.org/
//gsoap ns1   schema elementForm:	qualified
//gsoap ns1   schema attributeForm:	unqualified

/******************************************************************************\
 *                                                                            *
 * Schema Types                                                               *
 *                                                                            *
\******************************************************************************/



//  Forward declaration of class _ns1__GetEncryptionKey.
class _ns1__GetEncryptionKey;

//  Forward declaration of class _ns1__GetEncryptionKeyResponse.
class _ns1__GetEncryptionKeyResponse;

//  Forward declaration of class _ns1__GetMemberID.
class _ns1__GetMemberID;

//  Forward declaration of class _ns1__GetMemberIDResponse.
class _ns1__GetMemberIDResponse;

//  Forward declaration of class _ns1__GetTagID.
class _ns1__GetTagID;

//  Forward declaration of class _ns1__GetTagIDResponse.
class _ns1__GetTagIDResponse;


/// Element "http://tempuri.org/":GetEncryptionKey of complexType.

/// "http://tempuri.org/":GetEncryptionKey is a complexType.
class _ns1__GetEncryptionKey
{ public:
/// Element WebMemberID of type xs:string.
    char*                                WebMemberID                    0;	///< Optional element.
/// Element WebPassword of type xs:string.
    char*                                WebPassword                    0;	///< Optional element.
/// A handle to the soap struct that manages this instance (automatically set)
    struct soap                         *soap                          ;
};


/// Element "http://tempuri.org/":GetEncryptionKeyResponse of complexType.

/// "http://tempuri.org/":GetEncryptionKeyResponse is a complexType.
class _ns1__GetEncryptionKeyResponse
{ public:
/// Element GetEncryptionKeyResult of type xs:string.
    char*                                GetEncryptionKeyResult         0;	///< Optional element.
/// A handle to the soap struct that manages this instance (automatically set)
    struct soap                         *soap                          ;
};


/// Element "http://tempuri.org/":GetMemberID of complexType.

/// "http://tempuri.org/":GetMemberID is a complexType.
class _ns1__GetMemberID
{ public:
/// Element NickName of type xs:string.
    char*                                NickName                       0;	///< Optional element.
/// Element WebPassword of type xs:string.
    char*                                WebPassword                    0;	///< Optional element.
/// A handle to the soap struct that manages this instance (automatically set)
    struct soap                         *soap                          ;
};


/// Element "http://tempuri.org/":GetMemberIDResponse of complexType.

/// "http://tempuri.org/":GetMemberIDResponse is a complexType.
class _ns1__GetMemberIDResponse
{ public:
/// Element GetMemberIDResult of type xs:string.
    char*                                GetMemberIDResult              0;	///< Optional element.
/// A handle to the soap struct that manages this instance (automatically set)
    struct soap                         *soap                          ;
};


/// Element "http://tempuri.org/":GetTagID of complexType.

/// "http://tempuri.org/":GetTagID is a complexType.
class _ns1__GetTagID
{ public:
/// Element WebMemberID of type xs:string.
    char*                                WebMemberID                    0;	///< Optional element.
/// Element WebPassword of type xs:string.
    char*                                WebPassword                    0;	///< Optional element.
/// A handle to the soap struct that manages this instance (automatically set)
    struct soap                         *soap                          ;
};


/// Element "http://tempuri.org/":GetTagIDResponse of complexType.

/// "http://tempuri.org/":GetTagIDResponse is a complexType.
class _ns1__GetTagIDResponse
{ public:
/// Element GetTagIDResult of type xs:string.
    char*                                GetTagIDResult                 0;	///< Optional element.
/// A handle to the soap struct that manages this instance (automatically set)
    struct soap                         *soap                          ;
};

/// Element "http://tempuri.org/":string of type xs:string.
/// Note: use wsdl2h option -g to generate this global element declaration.

/******************************************************************************\
 *                                                                            *
 * Services                                                                   *
 *                                                                            *
\******************************************************************************/


//gsoap ns2  service name:	MemberServicesSoap 
//gsoap ns2  service type:	MemberServicesSoap 
//gsoap ns2  service port:	http://services.next2friends.com/n2fwebservices/memberservices.asmx 
//gsoap ns2  service namespace:	http://tempuri.org/MemberServicesSoap 
//gsoap ns2  service transport:	http://schemas.xmlsoap.org/soap/http 

//gsoap ns3  service name:	MemberServicesSoap12 
//gsoap ns3  service type:	MemberServicesSoap 
//gsoap ns3  service port:	http://services.next2friends.com/n2fwebservices/memberservices.asmx 
//gsoap ns3  service namespace:	http://tempuri.org/MemberServicesSoap12 
//gsoap ns3  service transport:	http://schemas.xmlsoap.org/soap/http 

/** @mainpage Service Definitions

@section Service_bindings Bindings
  - @ref MemberServicesSoap
  - @ref MemberServicesSoap12

*/

/**

@page MemberServicesSoap Binding "MemberServicesSoap"

@section MemberServicesSoap_operations Operations of Binding  "MemberServicesSoap"
  - @ref __ns2__GetEncryptionKey
  - @ref __ns2__GetMemberID
  - @ref __ns2__GetTagID

@section MemberServicesSoap_ports Endpoints of Binding  "MemberServicesSoap"
  - http://services.next2friends.com/n2fwebservices/memberservices.asmx

Note: use wsdl2h option -N to change the service binding prefix name

*/

/**

@page MemberServicesSoap12 Binding "MemberServicesSoap12"

@section MemberServicesSoap12_operations Operations of Binding  "MemberServicesSoap12"
  - @ref __ns3__GetEncryptionKey
  - @ref __ns3__GetMemberID
  - @ref __ns3__GetTagID

@section MemberServicesSoap12_ports Endpoints of Binding  "MemberServicesSoap12"
  - http://services.next2friends.com/n2fwebservices/memberservices.asmx

Note: use wsdl2h option -N to change the service binding prefix name

*/

/******************************************************************************\
 *                                                                            *
 * MemberServicesSoap                                                         *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * __ns2__GetEncryptionKey                                                    *
 *                                                                            *
\******************************************************************************/


/// Operation "__ns2__GetEncryptionKey" of service binding "MemberServicesSoap"

/**

Operation details:

  - SOAP document/literal style
  - SOAP action="http://tempuri.org/GetEncryptionKey"

C stub function (defined in soapClient.c[pp] generated by soapcpp2):
@code
  int soap_call___ns2__GetEncryptionKey(
    struct soap *soap,
    NULL, // char *endpoint = NULL selects default endpoint for this operation
    NULL, // char *action = NULL selects default action for this operation
    // request parameters:
    _ns1__GetEncryptionKey*             ns1__GetEncryptionKey,
    // response parameters:
    _ns1__GetEncryptionKeyResponse*     ns1__GetEncryptionKeyResponse
  );
@endcode

C server function (called from the service dispatcher defined in soapServer.c[pp]):
@code
  int __ns2__GetEncryptionKey(
    struct soap *soap,
    // request parameters:
    _ns1__GetEncryptionKey*             ns1__GetEncryptionKey,
    // response parameters:
    _ns1__GetEncryptionKeyResponse*     ns1__GetEncryptionKeyResponse
  );
@endcode

C++ proxy class (defined in soapMemberServicesSoapProxy.h):
  class MemberServicesSoap;

Note: use soapcpp2 option '-i' to generate improved proxy and service classes;

*/

//gsoap ns2  service method-style:	GetEncryptionKey document
//gsoap ns2  service method-encoding:	GetEncryptionKey literal
//gsoap ns2  service method-action:	GetEncryptionKey http://tempuri.org/GetEncryptionKey
int __ns2__GetEncryptionKey(
    _ns1__GetEncryptionKey*             ns1__GetEncryptionKey,	///< Request parameter
    _ns1__GetEncryptionKeyResponse*     ns1__GetEncryptionKeyResponse	///< Response parameter
);

/******************************************************************************\
 *                                                                            *
 * __ns2__GetMemberID                                                         *
 *                                                                            *
\******************************************************************************/


/// Operation "__ns2__GetMemberID" of service binding "MemberServicesSoap"

/**

Operation details:

  - SOAP document/literal style
  - SOAP action="http://tempuri.org/GetMemberID"

C stub function (defined in soapClient.c[pp] generated by soapcpp2):
@code
  int soap_call___ns2__GetMemberID(
    struct soap *soap,
    NULL, // char *endpoint = NULL selects default endpoint for this operation
    NULL, // char *action = NULL selects default action for this operation
    // request parameters:
    _ns1__GetMemberID*                  ns1__GetMemberID,
    // response parameters:
    _ns1__GetMemberIDResponse*          ns1__GetMemberIDResponse
  );
@endcode

C server function (called from the service dispatcher defined in soapServer.c[pp]):
@code
  int __ns2__GetMemberID(
    struct soap *soap,
    // request parameters:
    _ns1__GetMemberID*                  ns1__GetMemberID,
    // response parameters:
    _ns1__GetMemberIDResponse*          ns1__GetMemberIDResponse
  );
@endcode

C++ proxy class (defined in soapMemberServicesSoapProxy.h):
  class MemberServicesSoap;

Note: use soapcpp2 option '-i' to generate improved proxy and service classes;

*/

//gsoap ns2  service method-style:	GetMemberID document
//gsoap ns2  service method-encoding:	GetMemberID literal
//gsoap ns2  service method-action:	GetMemberID http://tempuri.org/GetMemberID
int __ns2__GetMemberID(
    _ns1__GetMemberID*                  ns1__GetMemberID,	///< Request parameter
    _ns1__GetMemberIDResponse*          ns1__GetMemberIDResponse	///< Response parameter
);

/******************************************************************************\
 *                                                                            *
 * __ns2__GetTagID                                                            *
 *                                                                            *
\******************************************************************************/


/// Operation "__ns2__GetTagID" of service binding "MemberServicesSoap"

/**

Operation details:

  - SOAP document/literal style
  - SOAP action="http://tempuri.org/GetTagID"

C stub function (defined in soapClient.c[pp] generated by soapcpp2):
@code
  int soap_call___ns2__GetTagID(
    struct soap *soap,
    NULL, // char *endpoint = NULL selects default endpoint for this operation
    NULL, // char *action = NULL selects default action for this operation
    // request parameters:
    _ns1__GetTagID*                     ns1__GetTagID,
    // response parameters:
    _ns1__GetTagIDResponse*             ns1__GetTagIDResponse
  );
@endcode

C server function (called from the service dispatcher defined in soapServer.c[pp]):
@code
  int __ns2__GetTagID(
    struct soap *soap,
    // request parameters:
    _ns1__GetTagID*                     ns1__GetTagID,
    // response parameters:
    _ns1__GetTagIDResponse*             ns1__GetTagIDResponse
  );
@endcode

C++ proxy class (defined in soapMemberServicesSoapProxy.h):
  class MemberServicesSoap;

Note: use soapcpp2 option '-i' to generate improved proxy and service classes;

*/

//gsoap ns2  service method-style:	GetTagID document
//gsoap ns2  service method-encoding:	GetTagID literal
//gsoap ns2  service method-action:	GetTagID http://tempuri.org/GetTagID
int __ns2__GetTagID(
    _ns1__GetTagID*                     ns1__GetTagID,	///< Request parameter
    _ns1__GetTagIDResponse*             ns1__GetTagIDResponse	///< Response parameter
);

/******************************************************************************\
 *                                                                            *
 * MemberServicesSoap12                                                       *
 *                                                                            *
\******************************************************************************/


/******************************************************************************\
 *                                                                            *
 * __ns3__GetEncryptionKey                                                    *
 *                                                                            *
\******************************************************************************/


/// Operation "__ns3__GetEncryptionKey" of service binding "MemberServicesSoap12"

/**

Operation details:

  - SOAP document/literal style
  - SOAP action="http://tempuri.org/GetEncryptionKey"

C stub function (defined in soapClient.c[pp] generated by soapcpp2):
@code
  int soap_call___ns3__GetEncryptionKey(
    struct soap *soap,
    NULL, // char *endpoint = NULL selects default endpoint for this operation
    NULL, // char *action = NULL selects default action for this operation
    // request parameters:
    _ns1__GetEncryptionKey*             ns1__GetEncryptionKey,
    // response parameters:
    _ns1__GetEncryptionKeyResponse*     ns1__GetEncryptionKeyResponse
  );
@endcode

C server function (called from the service dispatcher defined in soapServer.c[pp]):
@code
  int __ns3__GetEncryptionKey(
    struct soap *soap,
    // request parameters:
    _ns1__GetEncryptionKey*             ns1__GetEncryptionKey,
    // response parameters:
    _ns1__GetEncryptionKeyResponse*     ns1__GetEncryptionKeyResponse
  );
@endcode

C++ proxy class (defined in soapMemberServicesSoap12Proxy.h):
  class MemberServicesSoap12;

Note: use soapcpp2 option '-i' to generate improved proxy and service classes;

*/

//gsoap ns3  service method-style:	GetEncryptionKey document
//gsoap ns3  service method-encoding:	GetEncryptionKey literal
//gsoap ns3  service method-action:	GetEncryptionKey http://tempuri.org/GetEncryptionKey
int __ns3__GetEncryptionKey(
    _ns1__GetEncryptionKey*             ns1__GetEncryptionKey,	///< Request parameter
    _ns1__GetEncryptionKeyResponse*     ns1__GetEncryptionKeyResponse	///< Response parameter
);

/******************************************************************************\
 *                                                                            *
 * __ns3__GetMemberID                                                         *
 *                                                                            *
\******************************************************************************/


/// Operation "__ns3__GetMemberID" of service binding "MemberServicesSoap12"

/**

Operation details:

  - SOAP document/literal style
  - SOAP action="http://tempuri.org/GetMemberID"

C stub function (defined in soapClient.c[pp] generated by soapcpp2):
@code
  int soap_call___ns3__GetMemberID(
    struct soap *soap,
    NULL, // char *endpoint = NULL selects default endpoint for this operation
    NULL, // char *action = NULL selects default action for this operation
    // request parameters:
    _ns1__GetMemberID*                  ns1__GetMemberID,
    // response parameters:
    _ns1__GetMemberIDResponse*          ns1__GetMemberIDResponse
  );
@endcode

C server function (called from the service dispatcher defined in soapServer.c[pp]):
@code
  int __ns3__GetMemberID(
    struct soap *soap,
    // request parameters:
    _ns1__GetMemberID*                  ns1__GetMemberID,
    // response parameters:
    _ns1__GetMemberIDResponse*          ns1__GetMemberIDResponse
  );
@endcode

C++ proxy class (defined in soapMemberServicesSoap12Proxy.h):
  class MemberServicesSoap12;

Note: use soapcpp2 option '-i' to generate improved proxy and service classes;

*/

//gsoap ns3  service method-style:	GetMemberID document
//gsoap ns3  service method-encoding:	GetMemberID literal
//gsoap ns3  service method-action:	GetMemberID http://tempuri.org/GetMemberID
int __ns3__GetMemberID(
    _ns1__GetMemberID*                  ns1__GetMemberID,	///< Request parameter
    _ns1__GetMemberIDResponse*          ns1__GetMemberIDResponse	///< Response parameter
);

/******************************************************************************\
 *                                                                            *
 * __ns3__GetTagID                                                            *
 *                                                                            *
\******************************************************************************/


/// Operation "__ns3__GetTagID" of service binding "MemberServicesSoap12"

/**

Operation details:

  - SOAP document/literal style
  - SOAP action="http://tempuri.org/GetTagID"

C stub function (defined in soapClient.c[pp] generated by soapcpp2):
@code
  int soap_call___ns3__GetTagID(
    struct soap *soap,
    NULL, // char *endpoint = NULL selects default endpoint for this operation
    NULL, // char *action = NULL selects default action for this operation
    // request parameters:
    _ns1__GetTagID*                     ns1__GetTagID,
    // response parameters:
    _ns1__GetTagIDResponse*             ns1__GetTagIDResponse
  );
@endcode

C server function (called from the service dispatcher defined in soapServer.c[pp]):
@code
  int __ns3__GetTagID(
    struct soap *soap,
    // request parameters:
    _ns1__GetTagID*                     ns1__GetTagID,
    // response parameters:
    _ns1__GetTagIDResponse*             ns1__GetTagIDResponse
  );
@endcode

C++ proxy class (defined in soapMemberServicesSoap12Proxy.h):
  class MemberServicesSoap12;

Note: use soapcpp2 option '-i' to generate improved proxy and service classes;

*/

//gsoap ns3  service method-style:	GetTagID document
//gsoap ns3  service method-encoding:	GetTagID literal
//gsoap ns3  service method-action:	GetTagID http://tempuri.org/GetTagID
int __ns3__GetTagID(
    _ns1__GetTagID*                     ns1__GetTagID,	///< Request parameter
    _ns1__GetTagIDResponse*             ns1__GetTagIDResponse	///< Response parameter
);

/* End of output\memberservice.h */