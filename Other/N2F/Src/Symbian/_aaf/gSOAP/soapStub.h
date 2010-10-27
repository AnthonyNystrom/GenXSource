/* soapStub.h
   Generated by gSOAP 2.7.10 from askafriend.h
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

#ifndef SOAP_TYPE_ns1__ArrayOfString
#define SOAP_TYPE_ns1__ArrayOfString (7)
/* ns1:ArrayOfString */
class SOAP_CMAC ns1__ArrayOfString
{
public:
	int __sizestring;	/* sequence of elements <string> */
	char **string;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 7; } /* = unique id SOAP_TYPE_ns1__ArrayOfString */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__ArrayOfString() : __sizestring(0), string(NULL), soap(NULL) { }
	virtual ~ns1__ArrayOfString() { }
};
#endif

#ifndef SOAP_TYPE_ns1__AskAFriendConfirm
#define SOAP_TYPE_ns1__AskAFriendConfirm (8)
/* ns1:AskAFriendConfirm */
class SOAP_CMAC ns1__AskAFriendConfirm
{
public:
	char *AdvertURL;	/* optional element of type xsd:string */
	char *AdvertImage;	/* optional element of type xsd:string */
	char *WebAskAFriendID;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 8; } /* = unique id SOAP_TYPE_ns1__AskAFriendConfirm */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__AskAFriendConfirm() : AdvertURL(NULL), AdvertImage(NULL), WebAskAFriendID(NULL), soap(NULL) { }
	virtual ~ns1__AskAFriendConfirm() { }
};
#endif

#ifndef SOAP_TYPE_ns1__ArrayOfPrivateAAFQuestion
#define SOAP_TYPE_ns1__ArrayOfPrivateAAFQuestion (9)
/* ns1:ArrayOfPrivateAAFQuestion */
class SOAP_CMAC ns1__ArrayOfPrivateAAFQuestion
{
public:
	int __sizePrivateAAFQuestion;	/* sequence of elements <PrivateAAFQuestion> */
	class ns1__PrivateAAFQuestion **PrivateAAFQuestion;	/* optional element of type ns1:PrivateAAFQuestion */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 9; } /* = unique id SOAP_TYPE_ns1__ArrayOfPrivateAAFQuestion */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__ArrayOfPrivateAAFQuestion() : __sizePrivateAAFQuestion(0), PrivateAAFQuestion(NULL), soap(NULL) { }
	virtual ~ns1__ArrayOfPrivateAAFQuestion() { }
};
#endif

#ifndef SOAP_TYPE_ns1__PrivateAAFQuestion
#define SOAP_TYPE_ns1__PrivateAAFQuestion (10)
/* ns1:PrivateAAFQuestion */
class SOAP_CMAC ns1__PrivateAAFQuestion
{
public:
	char *NickName;	/* optional element of type xsd:string */
	char *Question;	/* optional element of type xsd:string */
	char *URL;	/* optional element of type xsd:string */
	char *DateTimePosted;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 10; } /* = unique id SOAP_TYPE_ns1__PrivateAAFQuestion */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__PrivateAAFQuestion() : NickName(NULL), Question(NULL), URL(NULL), DateTimePosted(NULL), soap(NULL) { }
	virtual ~ns1__PrivateAAFQuestion() { }
};
#endif

#ifndef SOAP_TYPE_ns1__ArrayOfAskAFriendQuestion
#define SOAP_TYPE_ns1__ArrayOfAskAFriendQuestion (11)
/* ns1:ArrayOfAskAFriendQuestion */
class SOAP_CMAC ns1__ArrayOfAskAFriendQuestion
{
public:
	int __sizeAskAFriendQuestion;	/* sequence of elements <AskAFriendQuestion> */
	class ns1__AskAFriendQuestion **AskAFriendQuestion;	/* optional element of type ns1:AskAFriendQuestion */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 11; } /* = unique id SOAP_TYPE_ns1__ArrayOfAskAFriendQuestion */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__ArrayOfAskAFriendQuestion() : __sizeAskAFriendQuestion(0), AskAFriendQuestion(NULL), soap(NULL) { }
	virtual ~ns1__ArrayOfAskAFriendQuestion() { }
};
#endif

#ifndef SOAP_TYPE_ns1__AskAFriendQuestion
#define SOAP_TYPE_ns1__AskAFriendQuestion (12)
/* ns1:AskAFriendQuestion */
class SOAP_CMAC ns1__AskAFriendQuestion
{
public:
	char *WebAskAFriendID;	/* optional element of type xsd:string */
	char *Question;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 12; } /* = unique id SOAP_TYPE_ns1__AskAFriendQuestion */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__AskAFriendQuestion() : WebAskAFriendID(NULL), Question(NULL), soap(NULL) { }
	virtual ~ns1__AskAFriendQuestion() { }
};
#endif

#ifndef SOAP_TYPE_ns1__AskAFriendResponse
#define SOAP_TYPE_ns1__AskAFriendResponse (13)
/* ns1:AskAFriendResponse */
class SOAP_CMAC ns1__AskAFriendResponse
{
public:
	char *WebAskAFriendID;	/* optional element of type xsd:string */
	char *Question;	/* optional element of type xsd:string */
	char *PhotoBase64Binary;	/* optional element of type xsd:string */
	class ns1__ArrayOfInt *ResponseValues;	/* optional element of type ns1:ArrayOfInt */
	double Average;	/* required element of type xsd:double */
	int ResponseType;	/* required element of type xsd:int */
	ns1__ArrayOfString *CustomResponses;	/* optional element of type ns1:ArrayOfString */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 13; } /* = unique id SOAP_TYPE_ns1__AskAFriendResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__AskAFriendResponse() : WebAskAFriendID(NULL), Question(NULL), PhotoBase64Binary(NULL), ResponseValues(NULL), Average(0), ResponseType(0), CustomResponses(NULL), soap(NULL) { }
	virtual ~ns1__AskAFriendResponse() { }
};
#endif

#ifndef SOAP_TYPE_ns1__ArrayOfInt
#define SOAP_TYPE_ns1__ArrayOfInt (14)
/* ns1:ArrayOfInt */
class SOAP_CMAC ns1__ArrayOfInt
{
public:
	int __sizeint_;	/* sequence of elements <int> */
	int *int_;	/* optional element of type xsd:int */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 14; } /* = unique id SOAP_TYPE_ns1__ArrayOfInt */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__ArrayOfInt() : __sizeint_(0), int_(NULL), soap(NULL) { }
	virtual ~ns1__ArrayOfInt() { }
};
#endif

#ifndef SOAP_TYPE_ns1__ArrayOfAskAFriendComment
#define SOAP_TYPE_ns1__ArrayOfAskAFriendComment (15)
/* ns1:ArrayOfAskAFriendComment */
class SOAP_CMAC ns1__ArrayOfAskAFriendComment
{
public:
	int __sizeAskAFriendComment;	/* sequence of elements <AskAFriendComment> */
	class ns1__AskAFriendComment **AskAFriendComment;	/* optional element of type ns1:AskAFriendComment */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 15; } /* = unique id SOAP_TYPE_ns1__ArrayOfAskAFriendComment */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__ArrayOfAskAFriendComment() : __sizeAskAFriendComment(0), AskAFriendComment(NULL), soap(NULL) { }
	virtual ~ns1__ArrayOfAskAFriendComment() { }
};
#endif

#ifndef SOAP_TYPE_ns1__AskAFriendComment
#define SOAP_TYPE_ns1__AskAFriendComment (16)
/* ns1:AskAFriendComment */
class SOAP_CMAC ns1__AskAFriendComment
{
public:
	char *NickName;	/* optional element of type xsd:string */
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebAskAFriendCommentID;	/* optional element of type xsd:string */
	char *Text;	/* optional element of type xsd:string */
	char *DateTimePosted;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 16; } /* = unique id SOAP_TYPE_ns1__AskAFriendComment */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         ns1__AskAFriendComment() : NickName(NULL), WebMemberID(NULL), WebAskAFriendCommentID(NULL), Text(NULL), DateTimePosted(NULL), soap(NULL) { }
	virtual ~ns1__AskAFriendComment() { }
};
#endif

#ifndef SOAP_TYPE__ns1__SubmitQuestion
#define SOAP_TYPE__ns1__SubmitQuestion (17)
/* ns1:SubmitQuestion */
class SOAP_CMAC _ns1__SubmitQuestion
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	char *Question;	/* optional element of type xsd:string */
	int NumberOfPhotos;	/* required element of type xsd:int */
	int ResponseType;	/* required element of type xsd:int */
	ns1__ArrayOfString *CustomResponses;	/* optional element of type ns1:ArrayOfString */
	int Duration;	/* required element of type xsd:int */
	bool IsPrivate;	/* required element of type xsd:boolean */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 17; } /* = unique id SOAP_TYPE__ns1__SubmitQuestion */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__SubmitQuestion() : WebMemberID(NULL), WebPassword(NULL), Question(NULL), NumberOfPhotos(0), ResponseType(0), CustomResponses(NULL), Duration(0), IsPrivate((bool)0), soap(NULL) { }
	virtual ~_ns1__SubmitQuestion() { }
};
#endif

#ifndef SOAP_TYPE__ns1__SubmitQuestionResponse
#define SOAP_TYPE__ns1__SubmitQuestionResponse (18)
/* ns1:SubmitQuestionResponse */
class SOAP_CMAC _ns1__SubmitQuestionResponse
{
public:
	ns1__AskAFriendConfirm *SubmitQuestionResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type ns1:AskAFriendConfirm */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 18; } /* = unique id SOAP_TYPE__ns1__SubmitQuestionResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__SubmitQuestionResponse() : SubmitQuestionResult(NULL), soap(NULL) { }
	virtual ~_ns1__SubmitQuestionResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__AttachPhoto
#define SOAP_TYPE__ns1__AttachPhoto (19)
/* ns1:AttachPhoto */
class SOAP_CMAC _ns1__AttachPhoto
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	char *WebAskAFriendID;	/* optional element of type xsd:string */
	int IndexOrder;	/* required element of type xsd:int */
	char *PhotoBase64String;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 19; } /* = unique id SOAP_TYPE__ns1__AttachPhoto */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__AttachPhoto() : WebMemberID(NULL), WebPassword(NULL), WebAskAFriendID(NULL), IndexOrder(0), PhotoBase64String(NULL), soap(NULL) { }
	virtual ~_ns1__AttachPhoto() { }
};
#endif

#ifndef SOAP_TYPE__ns1__AttachPhotoResponse
#define SOAP_TYPE__ns1__AttachPhotoResponse (20)
/* ns1:AttachPhotoResponse */
class SOAP_CMAC _ns1__AttachPhotoResponse
{
public:
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 20; } /* = unique id SOAP_TYPE__ns1__AttachPhotoResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__AttachPhotoResponse() : soap(NULL) { }
	virtual ~_ns1__AttachPhotoResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__CompleteQuestion
#define SOAP_TYPE__ns1__CompleteQuestion (21)
/* ns1:CompleteQuestion */
class SOAP_CMAC _ns1__CompleteQuestion
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	char *WebAskAFriendID;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 21; } /* = unique id SOAP_TYPE__ns1__CompleteQuestion */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__CompleteQuestion() : WebMemberID(NULL), WebPassword(NULL), WebAskAFriendID(NULL), soap(NULL) { }
	virtual ~_ns1__CompleteQuestion() { }
};
#endif

#ifndef SOAP_TYPE__ns1__CompleteQuestionResponse
#define SOAP_TYPE__ns1__CompleteQuestionResponse (22)
/* ns1:CompleteQuestionResponse */
class SOAP_CMAC _ns1__CompleteQuestionResponse
{
public:
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 22; } /* = unique id SOAP_TYPE__ns1__CompleteQuestionResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__CompleteQuestionResponse() : soap(NULL) { }
	virtual ~_ns1__CompleteQuestionResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetPrivateAAFQuestion
#define SOAP_TYPE__ns1__GetPrivateAAFQuestion (23)
/* ns1:GetPrivateAAFQuestion */
class SOAP_CMAC _ns1__GetPrivateAAFQuestion
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 23; } /* = unique id SOAP_TYPE__ns1__GetPrivateAAFQuestion */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetPrivateAAFQuestion() : WebMemberID(NULL), WebPassword(NULL), soap(NULL) { }
	virtual ~_ns1__GetPrivateAAFQuestion() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetPrivateAAFQuestionResponse
#define SOAP_TYPE__ns1__GetPrivateAAFQuestionResponse (24)
/* ns1:GetPrivateAAFQuestionResponse */
class SOAP_CMAC _ns1__GetPrivateAAFQuestionResponse
{
public:
	ns1__ArrayOfPrivateAAFQuestion *GetPrivateAAFQuestionResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type ns1:ArrayOfPrivateAAFQuestion */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 24; } /* = unique id SOAP_TYPE__ns1__GetPrivateAAFQuestionResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetPrivateAAFQuestionResponse() : GetPrivateAAFQuestionResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetPrivateAAFQuestionResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetMyAAFQuestions
#define SOAP_TYPE__ns1__GetMyAAFQuestions (25)
/* ns1:GetMyAAFQuestions */
class SOAP_CMAC _ns1__GetMyAAFQuestions
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	char *LastWebAskAFriendID;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 25; } /* = unique id SOAP_TYPE__ns1__GetMyAAFQuestions */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetMyAAFQuestions() : WebMemberID(NULL), WebPassword(NULL), LastWebAskAFriendID(NULL), soap(NULL) { }
	virtual ~_ns1__GetMyAAFQuestions() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetMyAAFQuestionsResponse
#define SOAP_TYPE__ns1__GetMyAAFQuestionsResponse (26)
/* ns1:GetMyAAFQuestionsResponse */
class SOAP_CMAC _ns1__GetMyAAFQuestionsResponse
{
public:
	ns1__ArrayOfAskAFriendQuestion *GetMyAAFQuestionsResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type ns1:ArrayOfAskAFriendQuestion */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 26; } /* = unique id SOAP_TYPE__ns1__GetMyAAFQuestionsResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetMyAAFQuestionsResponse() : GetMyAAFQuestionsResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetMyAAFQuestionsResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetNewAAFQuestionCommentIDs
#define SOAP_TYPE__ns1__GetNewAAFQuestionCommentIDs (27)
/* ns1:GetNewAAFQuestionCommentIDs */
class SOAP_CMAC _ns1__GetNewAAFQuestionCommentIDs
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	ns1__ArrayOfString *WebAskAFriendIDs;	/* optional element of type ns1:ArrayOfString */
	ns1__ArrayOfString *WebLastAskAFriendQuestionIDs;	/* optional element of type ns1:ArrayOfString */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 27; } /* = unique id SOAP_TYPE__ns1__GetNewAAFQuestionCommentIDs */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetNewAAFQuestionCommentIDs() : WebMemberID(NULL), WebPassword(NULL), WebAskAFriendIDs(NULL), WebLastAskAFriendQuestionIDs(NULL), soap(NULL) { }
	virtual ~_ns1__GetNewAAFQuestionCommentIDs() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetNewAAFQuestionCommentIDsResponse
#define SOAP_TYPE__ns1__GetNewAAFQuestionCommentIDsResponse (28)
/* ns1:GetNewAAFQuestionCommentIDsResponse */
class SOAP_CMAC _ns1__GetNewAAFQuestionCommentIDsResponse
{
public:
	ns1__ArrayOfString *GetNewAAFQuestionCommentIDsResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type ns1:ArrayOfString */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 28; } /* = unique id SOAP_TYPE__ns1__GetNewAAFQuestionCommentIDsResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetNewAAFQuestionCommentIDsResponse() : GetNewAAFQuestionCommentIDsResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetNewAAFQuestionCommentIDsResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetAAFResponse
#define SOAP_TYPE__ns1__GetAAFResponse (29)
/* ns1:GetAAFResponse */
class SOAP_CMAC _ns1__GetAAFResponse
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	char *WebAskAFriendID;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 29; } /* = unique id SOAP_TYPE__ns1__GetAAFResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetAAFResponse() : WebMemberID(NULL), WebPassword(NULL), WebAskAFriendID(NULL), soap(NULL) { }
	virtual ~_ns1__GetAAFResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetAAFResponseResponse
#define SOAP_TYPE__ns1__GetAAFResponseResponse (30)
/* ns1:GetAAFResponseResponse */
class SOAP_CMAC _ns1__GetAAFResponseResponse
{
public:
	ns1__AskAFriendResponse *GetAAFResponseResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type ns1:AskAFriendResponse */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 30; } /* = unique id SOAP_TYPE__ns1__GetAAFResponseResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetAAFResponseResponse() : GetAAFResponseResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetAAFResponseResponse() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetAAFComments
#define SOAP_TYPE__ns1__GetAAFComments (31)
/* ns1:GetAAFComments */
class SOAP_CMAC _ns1__GetAAFComments
{
public:
	char *WebMemberID;	/* optional element of type xsd:string */
	char *WebPassword;	/* optional element of type xsd:string */
	char *WebAskAFriendID;	/* optional element of type xsd:string */
	char *LastWebAskAFriendCommentID;	/* optional element of type xsd:string */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 31; } /* = unique id SOAP_TYPE__ns1__GetAAFComments */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetAAFComments() : WebMemberID(NULL), WebPassword(NULL), WebAskAFriendID(NULL), LastWebAskAFriendCommentID(NULL), soap(NULL) { }
	virtual ~_ns1__GetAAFComments() { }
};
#endif

#ifndef SOAP_TYPE__ns1__GetAAFCommentsResponse
#define SOAP_TYPE__ns1__GetAAFCommentsResponse (32)
/* ns1:GetAAFCommentsResponse */
class SOAP_CMAC _ns1__GetAAFCommentsResponse
{
public:
	ns1__ArrayOfAskAFriendComment *GetAAFCommentsResult;	/* SOAP 1.2 RPC return element (when namespace qualified) */	/* optional element of type ns1:ArrayOfAskAFriendComment */
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 32; } /* = unique id SOAP_TYPE__ns1__GetAAFCommentsResponse */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         _ns1__GetAAFCommentsResponse() : GetAAFCommentsResult(NULL), soap(NULL) { }
	virtual ~_ns1__GetAAFCommentsResponse() { }
};
#endif

#ifndef SOAP_TYPE_StringArray
#define SOAP_TYPE_StringArray (33)
/* SOAP encoded array of xsd:string schema type: */
class SOAP_CMAC StringArray
{
public:
	char **__ptrString;
	int __size;
	struct soap *soap;	/* transient */
public:
	virtual int soap_type() const { return 33; } /* = unique id SOAP_TYPE_StringArray */
	virtual void soap_default(struct soap*);
	virtual void soap_serialize(struct soap*) const;
	virtual int soap_put(struct soap*, const char*, const char*) const;
	virtual int soap_out(struct soap*, const char*, int, const char*) const;
	virtual void *soap_get(struct soap*, const char*, const char*);
	virtual void *soap_in(struct soap*, const char*, const char*);
	         StringArray() : __ptrString(NULL), __size(0), soap(NULL) { }
	virtual ~StringArray() { }
};
#endif

#ifndef SOAP_TYPE___ns3__SubmitQuestion
#define SOAP_TYPE___ns3__SubmitQuestion (55)
/* Operation wrapper: */
struct __ns3__SubmitQuestion
{
public:
	_ns1__SubmitQuestion *ns1__SubmitQuestion;	/* optional element of type ns1:SubmitQuestion */
};
#endif

#ifndef SOAP_TYPE___ns3__AttachPhoto
#define SOAP_TYPE___ns3__AttachPhoto (59)
/* Operation wrapper: */
struct __ns3__AttachPhoto
{
public:
	_ns1__AttachPhoto *ns1__AttachPhoto;	/* optional element of type ns1:AttachPhoto */
};
#endif

#ifndef SOAP_TYPE___ns3__CompleteQuestion
#define SOAP_TYPE___ns3__CompleteQuestion (63)
/* Operation wrapper: */
struct __ns3__CompleteQuestion
{
public:
	_ns1__CompleteQuestion *ns1__CompleteQuestion;	/* optional element of type ns1:CompleteQuestion */
};
#endif

#ifndef SOAP_TYPE___ns3__GetPrivateAAFQuestion
#define SOAP_TYPE___ns3__GetPrivateAAFQuestion (67)
/* Operation wrapper: */
struct __ns3__GetPrivateAAFQuestion
{
public:
	_ns1__GetPrivateAAFQuestion *ns1__GetPrivateAAFQuestion;	/* optional element of type ns1:GetPrivateAAFQuestion */
};
#endif

#ifndef SOAP_TYPE___ns3__GetMyAAFQuestions
#define SOAP_TYPE___ns3__GetMyAAFQuestions (71)
/* Operation wrapper: */
struct __ns3__GetMyAAFQuestions
{
public:
	_ns1__GetMyAAFQuestions *ns1__GetMyAAFQuestions;	/* optional element of type ns1:GetMyAAFQuestions */
};
#endif

#ifndef SOAP_TYPE___ns3__GetNewAAFQuestionCommentIDs
#define SOAP_TYPE___ns3__GetNewAAFQuestionCommentIDs (75)
/* Operation wrapper: */
struct __ns3__GetNewAAFQuestionCommentIDs
{
public:
	_ns1__GetNewAAFQuestionCommentIDs *ns1__GetNewAAFQuestionCommentIDs;	/* optional element of type ns1:GetNewAAFQuestionCommentIDs */
};
#endif

#ifndef SOAP_TYPE___ns3__GetAAFResponse
#define SOAP_TYPE___ns3__GetAAFResponse (79)
/* Operation wrapper: */
struct __ns3__GetAAFResponse
{
public:
	_ns1__GetAAFResponse *ns1__GetAAFResponse;	/* optional element of type ns1:GetAAFResponse */
};
#endif

#ifndef SOAP_TYPE___ns3__GetAAFComments
#define SOAP_TYPE___ns3__GetAAFComments (83)
/* Operation wrapper: */
struct __ns3__GetAAFComments
{
public:
	_ns1__GetAAFComments *ns1__GetAAFComments;	/* optional element of type ns1:GetAAFComments */
};
#endif

#ifndef SOAP_TYPE___ns4__SubmitQuestion
#define SOAP_TYPE___ns4__SubmitQuestion (85)
/* Operation wrapper: */
struct __ns4__SubmitQuestion
{
public:
	_ns1__SubmitQuestion *ns1__SubmitQuestion;	/* optional element of type ns1:SubmitQuestion */
};
#endif

#ifndef SOAP_TYPE___ns4__AttachPhoto
#define SOAP_TYPE___ns4__AttachPhoto (87)
/* Operation wrapper: */
struct __ns4__AttachPhoto
{
public:
	_ns1__AttachPhoto *ns1__AttachPhoto;	/* optional element of type ns1:AttachPhoto */
};
#endif

#ifndef SOAP_TYPE___ns4__CompleteQuestion
#define SOAP_TYPE___ns4__CompleteQuestion (89)
/* Operation wrapper: */
struct __ns4__CompleteQuestion
{
public:
	_ns1__CompleteQuestion *ns1__CompleteQuestion;	/* optional element of type ns1:CompleteQuestion */
};
#endif

#ifndef SOAP_TYPE___ns4__GetPrivateAAFQuestion
#define SOAP_TYPE___ns4__GetPrivateAAFQuestion (91)
/* Operation wrapper: */
struct __ns4__GetPrivateAAFQuestion
{
public:
	_ns1__GetPrivateAAFQuestion *ns1__GetPrivateAAFQuestion;	/* optional element of type ns1:GetPrivateAAFQuestion */
};
#endif

#ifndef SOAP_TYPE___ns4__GetMyAAFQuestions
#define SOAP_TYPE___ns4__GetMyAAFQuestions (93)
/* Operation wrapper: */
struct __ns4__GetMyAAFQuestions
{
public:
	_ns1__GetMyAAFQuestions *ns1__GetMyAAFQuestions;	/* optional element of type ns1:GetMyAAFQuestions */
};
#endif

#ifndef SOAP_TYPE___ns4__GetNewAAFQuestionCommentIDs
#define SOAP_TYPE___ns4__GetNewAAFQuestionCommentIDs (95)
/* Operation wrapper: */
struct __ns4__GetNewAAFQuestionCommentIDs
{
public:
	_ns1__GetNewAAFQuestionCommentIDs *ns1__GetNewAAFQuestionCommentIDs;	/* optional element of type ns1:GetNewAAFQuestionCommentIDs */
};
#endif

#ifndef SOAP_TYPE___ns4__GetAAFResponse
#define SOAP_TYPE___ns4__GetAAFResponse (97)
/* Operation wrapper: */
struct __ns4__GetAAFResponse
{
public:
	_ns1__GetAAFResponse *ns1__GetAAFResponse;	/* optional element of type ns1:GetAAFResponse */
};
#endif

#ifndef SOAP_TYPE___ns4__GetAAFComments
#define SOAP_TYPE___ns4__GetAAFComments (99)
/* Operation wrapper: */
struct __ns4__GetAAFComments
{
public:
	_ns1__GetAAFComments *ns1__GetAAFComments;	/* optional element of type ns1:GetAAFComments */
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Header
#define SOAP_TYPE_SOAP_ENV__Header (100)
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
#define SOAP_TYPE_SOAP_ENV__Code (101)
/* SOAP Fault Code: */
struct SOAP_ENV__Code
{
public:
	char *SOAP_ENV__Value;	/* optional element of type xsd:QName */
	struct SOAP_ENV__Code *SOAP_ENV__Subcode;	/* optional element of type SOAP-ENV:Code */
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Detail
#define SOAP_TYPE_SOAP_ENV__Detail (103)
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
#define SOAP_TYPE_SOAP_ENV__Reason (106)
/* SOAP-ENV:Reason */
struct SOAP_ENV__Reason
{
public:
	char *SOAP_ENV__Text;	/* optional element of type xsd:string */
};
#endif

#ifndef SOAP_TYPE_SOAP_ENV__Fault
#define SOAP_TYPE_SOAP_ENV__Fault (107)
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
