/*
============================================================================
Name        : Aafuserquestionsserviceprovider.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : User questions stuff controller class declaration
============================================================================
*/

#ifndef __AAFUSERQUESTIONSCONTROLLER_H__
#define __AAFUSERQUESTIONSCONTROLLER_H__

#include <coemain.h>
#include <e32base.h>
#include <aknlists.h>
#include "soapAskAFriendWSSoapProxy.h" // From gSOAP library
#include "common.h"

// FORWARD DECLARATIONS
class MRequestObserver;
class CAafLoginServiceProvider;
class CAafConnectionManager;
class RSocket;

// Singleton class
class CAafUserQuestionsProvider: public CCoeStatic, public CActive, public MGSoapData
{
public:
	/**
	* For singleton stuff
	*/
	static CAafUserQuestionsProvider* GetInstanceL();

	/**
	* Destructor
	*/
	virtual ~CAafUserQuestionsProvider();
	
	/**
	* Asynchronous request function
	*/
	TInt RequestForQuestions(MRequestObserver* aRequestObserver);

	/**
	* Synchronous request function
	*/
	TInt RequestForQuestions();

	/**
	* Asynchronous request function
	*/
	TInt RequestForComments(MRequestObserver* aRequestObserver, TInt aQuestionIndex);

	/**
	* Synchronous request function
	*/
	TInt RequestForComments(TInt aQuestionIndex);

	/**
	* Asynchronous request function
	*/
	TInt SubmitQuestion(MRequestObserver* aRequestObserver);

	/**
	* Synchronous request function
	*/
	TInt SubmitQuestion();

	/**
	* Set question data
	*/
	void SetQuestionData(_ns1__QuestionData& aQuestionData);

	/**
	* Get submitted question 
	*/
	HBufC8* GetSubmittedQuestionId();

	/**
	* Get count of question items
	*/
	TInt GetQuestionCount();

	/**
	* Get count of comment items for the question with the specified order index
	*/
	TInt GetCommentCount(TInt aQuestionIndex);

	/**
	* Get list of question items
	*/
	void GetQuestionListItemsL(CDesCArray* aItems);	

	/**
	* Get list of question comments items
	*/
	void GetCommentListItems(TInt aQuestionIndex, CDesCArray* aItems);

	/**
	* Get pointer to the question struct with specified index
	*/
	ns1__AskAFriendQuestion* GetQuestion(TInt aQuestionIndex);

	/**
	* Get pointer to the comment struct with specified indexes
	*/
	ns1__AskAFriendComment* GetComment(TInt aQuestionIndex, TInt aCommentIndex);
	

protected: //From CActive
	/**
	*
	*/
	virtual void DoCancel();

	/**
	*
	*/
	virtual void RunL();

	/**
	*
	*/
	virtual TInt RunError(TInt anError);

protected: // From MGSoapData
	virtual CAafLoginServiceProvider* GetLoginProvider() const
	{
		return iLoginProvider;
	}

	virtual CAafConnectionManager* GetConnectionManager() const
	{
		return iConnManager;
	}

	virtual RSocket* GetSocketInstance()
	{
		return iSocket;
	}

private:
	/**
	* Default constructor
	*/
	CAafUserQuestionsProvider();

	/**
	* 
	*/
	void ConstructL();
	
	/**
	* Thread start function (questions retrieving)
	*/
	static TInt ThreadEntryPoint1(TAny* aParameters);

	/**
	* Thread start function (comments retrieving)
	*/
	static TInt ThreadEntryPoint2(TAny* aParameters);

	/**
	* Thread start function (question submitting)
	*/
	static TInt ThreadEntryPoint3(TAny* aParameters);

private:
	// Auxillary class for internal usage only
	class _ns1__QuestionComments
	{
	public:
		char* iQuestionId;

		RPointerArray<ns1__AskAFriendComment> iQuestionComments;

		_ns1__QuestionComments();

		~_ns1__QuestionComments();
	};

	// Class member variables
	MRequestObserver* iRequestObserver;

	RThread iThread; //Handle to created thread

	AskAFriendWSSoapProxy* iAskFriendProxy; // From gSOAP
	
	// For GetMyAAFQuestions
	_ns1__GetMyAAFQuestions iQuestionsRequestStruct;

	_ns1__GetMyAAFQuestionsResponse iQuestionsResponseStruct;

	RPointerArray<ns1__AskAFriendQuestion> iQuestionsArray; // Array of user questions

	// For GetAAFComments
	_ns1__GetAAFComments iCommentsRequestStruct;

	_ns1__GetAAFCommentsResponse iCommentsResponseStruct;

	RPointerArray<_ns1__QuestionComments> iCommentsArray; // Array of questions comments
	
	// For questions submitting
	TInt iCurrentQuestion;

	_ns1__QuestionData iQuestionData; // Currently submitted question data

	_ns1__SubmitQuestion iQuestionSubmitStruct;

	_ns1__SubmitQuestionResponse iSubmitResponseStruct;

	// For MGSoapData interface realization
	CAafLoginServiceProvider* iLoginProvider;

	CAafConnectionManager* iConnManager;

	RSocket* iSocket;
};

#endif // __AAFUSERQUESTIONSCONTROLLER_H__