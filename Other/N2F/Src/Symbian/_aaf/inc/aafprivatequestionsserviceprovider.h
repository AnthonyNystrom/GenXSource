/*
============================================================================
Name        : Aafprivatequestionsserviceprovider.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Private questions stuff controller class declaration
============================================================================
*/

#ifndef __AAFPRIVATEQUESTIONSCONTROLLER_H__
#define __AAFPRIVATEQUESTIONSCONTROLLER_H__

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
class CAafPrivateQuestionsProvider: public CCoeStatic, public CActive, public MGSoapData
{	
public:
	/**
	* For singleton stuff
	*/
	static CAafPrivateQuestionsProvider* GetInstanceL();

	/**
	* Destructor
	*/
	virtual ~CAafPrivateQuestionsProvider();
	
	/**
	* Asynchronous request function
	*/
	TInt RequestForQuestions(MRequestObserver* aRequstObserver);
	
	/**
	* Synchronous request function
	*/
	TInt RequestForQuestions();

	/**
	* Get count of items
	*/
	TInt GetQuestionCount();

	/**
	* Get list of items
	*/
	void GetQuestionListItemsL(CDesCArray* aItems);	

	/**
	* Get pointer to the question struct with specified index
	*/
	ns1__PrivateAAFQuestion* GetQuestion(TInt aQuestionIndex);
	
protected: //From CActive
	virtual void DoCancel();

	virtual void RunL();

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
	CAafPrivateQuestionsProvider();

	void ConstructL();
	
	//Thread start function
	static TInt ThreadEntryPoint(TAny* aParameters);

private:

	MRequestObserver* iRequestObserver;

	RThread iThread; //Handle to created thread
	
	AskAFriendWSSoapProxy* iAskFriendProxy; // From gSOAP

	_ns1__GetPrivateAAFQuestion iRequestStruct;

	_ns1__GetPrivateAAFQuestionResponse iResponseStruct;

	// For MGSoapData interface realization
	CAafLoginServiceProvider* iLoginProvider;

	CAafConnectionManager* iConnManager;

	RSocket* iSocket;
	
};

#endif // __AAFPRIVATEQUESTIONSCONTROLLER_H__