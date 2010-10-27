/* soapAskAFriendWSSoapProxy.h
   Generated by gSOAP 2.7.10 from output\askafriendservice.h
   Copyright(C) 2000-2008, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#ifndef soapAskAFriendWSSoapProxy_H
#define soapAskAFriendWSSoapProxy_H
#include "soapH.h"

class SOAP_CMAC AskAFriendWSSoapProxy : public soap
{ public:
	/// Endpoint URL of service 'AskAFriendWSSoapProxy' (change as needed)
	const char *soap_endpoint;
	/// Constructor
	AskAFriendWSSoapProxy();
	/// Constructor with copy of another engine state
	AskAFriendWSSoapProxy(const struct soap&);
	/// Constructor with engine input+output mode control
	AskAFriendWSSoapProxy(soap_mode iomode);
	/// Constructor with engine input and output mode control
	AskAFriendWSSoapProxy(soap_mode imode, soap_mode omode);
	/// Destructor frees deserialized data
	virtual	~AskAFriendWSSoapProxy();
	/// Initializer used by constructor
	virtual	void AskAFriendWSSoapProxy_init(soap_mode imode, soap_mode omode);
	/// Disables and removes SOAP Header from message
	virtual	void soap_noheader();
	/// Get SOAP Fault structure (NULL when absent)
	virtual	const SOAP_ENV__Fault *soap_fault();
	/// Get SOAP Fault string (NULL when absent)
	virtual	const char *soap_fault_string();
	/// Get SOAP Fault detail as string (NULL when absent)
	virtual	const char *soap_fault_detail();
	/// Force close connection (normally automatic, except for send_X ops)
	virtual	int soap_close_socket();
	/// Print fault
	virtual	void soap_print_fault(FILE*);
#ifndef WITH_LEAN
	/// Print fault to stream
	virtual	void soap_stream_fault(std::ostream&);
	/// Put fault into buffer
	virtual	char *soap_sprint_fault(char *buf, size_t len);
#endif

	/// Web service operation 'SubmitQuestion' (returns error code or SOAP_OK)
	virtual	int SubmitQuestion(_ns1__SubmitQuestion *ns1__SubmitQuestion, _ns1__SubmitQuestionResponse *ns1__SubmitQuestionResponse);

	/// Web service operation 'AttachPhoto' (returns error code or SOAP_OK)
	virtual	int AttachPhoto(_ns1__AttachPhoto *ns1__AttachPhoto, _ns1__AttachPhotoResponse *ns1__AttachPhotoResponse);

	/// Web service operation 'CompleteQuestion' (returns error code or SOAP_OK)
	virtual	int CompleteQuestion(_ns1__CompleteQuestion *ns1__CompleteQuestion, _ns1__CompleteQuestionResponse *ns1__CompleteQuestionResponse);

	/// Web service operation 'GetPrivateAAFQuestion' (returns error code or SOAP_OK)
	virtual	int GetPrivateAAFQuestion(_ns1__GetPrivateAAFQuestion *ns1__GetPrivateAAFQuestion, _ns1__GetPrivateAAFQuestionResponse *ns1__GetPrivateAAFQuestionResponse);

	/// Web service operation 'GetMyAAFQuestions' (returns error code or SOAP_OK)
	virtual	int GetMyAAFQuestions(_ns1__GetMyAAFQuestions *ns1__GetMyAAFQuestions, _ns1__GetMyAAFQuestionsResponse *ns1__GetMyAAFQuestionsResponse);

	/// Web service operation 'GetNewAAFQuestionCommentIDs' (returns error code or SOAP_OK)
	virtual	int GetNewAAFQuestionCommentIDs(_ns1__GetNewAAFQuestionCommentIDs *ns1__GetNewAAFQuestionCommentIDs, _ns1__GetNewAAFQuestionCommentIDsResponse *ns1__GetNewAAFQuestionCommentIDsResponse);

	/// Web service operation 'GetAAFResponse' (returns error code or SOAP_OK)
	virtual	int GetAAFResponse(_ns1__GetAAFResponse *ns1__GetAAFResponse, _ns1__GetAAFResponseResponse *ns1__GetAAFResponseResponse);

	/// Web service operation 'GetAAFComments' (returns error code or SOAP_OK)
	virtual	int GetAAFComments(_ns1__GetAAFComments *ns1__GetAAFComments, _ns1__GetAAFCommentsResponse *ns1__GetAAFCommentsResponse);
};
#endif
