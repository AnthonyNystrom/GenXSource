/*
============================================================================
Name        : Aafprivatequestionsserviceprovider.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Private questions stuff controller class implementation
============================================================================
*/

// INCLUDE FILES
#include <es_sock.h>
#include "aafprivatequestionsserviceprovider.h"
#include "aafconnectionmanager.h"
#include "Aafsocketutils.h"
#include "Aafloginserviceprovider.h"
#include "Aafapputils.h"
#include "aaf.pan"


_LIT(KThreadName, "N2FPrivateQuestionsThread"); // Name of the new thread


CAafPrivateQuestionsProvider::CAafPrivateQuestionsProvider()
:CCoeStatic(KUidAafPQuestionsController),
CActive(EPriorityStandard)
{
	// Add to the active scheduler
	CActiveScheduler::Add(this);

	// Set web service end point
	iAskFriendProxy = NULL;

	// For MGSoapData interface realization
	iLoginProvider = NULL;

	iConnManager = NULL;

	iSocket = NULL;
}

CAafPrivateQuestionsProvider* CAafPrivateQuestionsProvider::GetInstanceL()
{
	__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::GetInstanceL() begins");

	CAafPrivateQuestionsProvider* controllerInstance = static_cast<CAafPrivateQuestionsProvider*>
		( CCoeEnv::Static( KUidAafPQuestionsController ) );

	if (!controllerInstance)
	{
		// Create instance
		controllerInstance = new (ELeave)CAafPrivateQuestionsProvider;
	
		controllerInstance->iAskFriendProxy = new (ELeave)AskAFriendWSSoapProxy;
		
		controllerInstance->iAskFriendProxy->soap_endpoint = N2F_ASKAFRIEND_ENDPOINT;
		
		controllerInstance->iLoginProvider = CAafLoginServiceProvider::GetInstanceL();
		
		controllerInstance->iConnManager = CAafConnectionManager::GetInstanceL();
		
		controllerInstance->iSocket = new (ELeave)RSocket;
	}

	__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::GetInstanceL() ends");

	return controllerInstance;
}

CAafPrivateQuestionsProvider::~CAafPrivateQuestionsProvider()
{
	// Cancel any outstanding request
	Cancel();

	// Soap environment uninitialization
	if (iAskFriendProxy)
	{
		delete iAskFriendProxy;
		iAskFriendProxy = NULL;
	}

	if (iSocket)
	{
		iSocket->Close();

		delete iSocket;
		iSocket = NULL;
	}
}

TInt CAafPrivateQuestionsProvider::RequestForQuestions(MRequestObserver* aRequstObserver)
{
	__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() begins");

	__ASSERT_ALWAYS(aRequstObserver, Panic(ENoObserver));

	// Set observer of the login request
	iRequestObserver = aRequstObserver;

	if (IsActive())
	{
		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() ends because KErrAlreadyExists");

		return KErrAlreadyExists;
	}

	// Create a new thread, passing the thread function and stack sizes
	// No extra parameters are required
	// Pass pointer to current class as variable argument
	TInt res = iThread.Create(KThreadName, ThreadEntryPoint, KDefaultStackSize, NULL, this);

	if(res != KErrNone)
	{ 
		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() ends because res != KErrNone");
		
		//delete threadData;
		//threadData = NULL;

		// Complete the caller immediately
		return res;
	}
	else
	{ 
		// Set active; resume new thread to make the synchronous call
		//(Change the priority of the thread here if required)
		// Set the caller and ourselves to KRequestPending
		// so the active scheduler notifies on completion

		iStatus = KRequestPending;

		SetActive();

		iThread.Logon(iStatus); // Request notification when thread dies

		iThread.Resume(); //Start the thread

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() ends");

		return KErrNone;
	}
}

TInt CAafPrivateQuestionsProvider::RequestForQuestions()
{
		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() begins");

		CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();

		// If credentials are invalid
		if (!loginProvider || !loginProvider->GetMemberID() || !loginProvider->GetPassword())
		{
			__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() ends");

			return KErrArgument;
		}

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() step 01");	

		
		// Free previously allocated memory if necessary (response struct)
		if (iResponseStruct.GetPrivateAAFQuestionResult)
		{
			for (int i = 0; i < iResponseStruct.GetPrivateAAFQuestionResult->__sizePrivateAAFQuestion; i++)
			{
				delete [] iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->DateTimePosted;
				iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->DateTimePosted = NULL;

				delete [] iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->NickName;
				iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->NickName = NULL;

				delete [] iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->Question;
				iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->Question = NULL;
			}

			delete iResponseStruct.GetPrivateAAFQuestionResult;
			iResponseStruct.GetPrivateAAFQuestionResult = NULL;
		}

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() step 02");

		iRequestStruct.WebMemberID = CAafUtils::DescriptorToString8L(*loginProvider->GetMemberID());
		iRequestStruct.WebPassword = CAafUtils::DescriptorToString8L(*loginProvider->GetPassword());

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() step 03");
		
		// Set appropriate sockets method pointers etc
		iAskFriendProxy->user = this;

		iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
		iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
		iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
		iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

		TInt retValue = KErrNone;

		// Perform a long synchronous task
		int res = iAskFriendProxy->GetPrivateAAFQuestion(&iRequestStruct, &iResponseStruct);

		__LOGSTR_TOFILE1("CAafPrivateQuestionsProvider::RequestForQuestions() step 04 with res == %d", res);

		if (res != SOAP_OK)
		{
			retValue = KErrUnknown;
		}		

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RequestForQuestions() ends");

		// Free dynamically allocated memory
		delete [] iRequestStruct.WebMemberID;
		iRequestStruct.WebMemberID = NULL;

		delete [] iRequestStruct.WebPassword;
		iRequestStruct.WebPassword = NULL;

		return retValue; // This value is discarded
}

TInt CAafPrivateQuestionsProvider::GetQuestionCount()
{
	TInt retValue = 0;

	// Get question count from currently available web service response
	if (iResponseStruct.GetPrivateAAFQuestionResult)
	{
		retValue = (TInt)iResponseStruct.GetPrivateAAFQuestionResult->__sizePrivateAAFQuestion;
	}

	return retValue;
}

void CAafPrivateQuestionsProvider::GetQuestionListItemsL(CDesCArray* aItems)
{
	TInt questionCount = GetQuestionCount();

	TBuf<330> listboxItem;

	for (TInt i = 0; i < questionCount; i++)
	{
		// Form question time string
		TTime questionTime;

		// Convert string to TTime structure
		HBufC8* dateDescriptor = CAafUtils::StringToDescriptor8LC(iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->DateTimePosted);
		TInt64 dateAndTime;
		TLex8 dateTime(dateDescriptor->Des());
		TInt err = dateTime.Val(dateAndTime);

		questionTime = (dateAndTime/TInt64(10));

		// Format date and time string
		TBuf<30> dateString;
		questionTime.FormatL(dateString, KDateString);

		CleanupStack::PopAndDestroy(dateDescriptor);

		HBufC8* nickName = CAafUtils::StringToDescriptor8LC(iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->NickName);

		HBufC* questionText = CAafUtils::StringToDescriptorLC(iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->Question);


		listboxItem.Format(KDoubleListboxWithIconPattern, 0, questionText, nickName, &dateString);
		aItems->AppendL(listboxItem);

		// Free allocated memory
		CleanupStack::PopAndDestroy(questionText);
		CleanupStack::PopAndDestroy(nickName);
	}
}

ns1__PrivateAAFQuestion* CAafPrivateQuestionsProvider::GetQuestion(TInt aQuestionIndex)
{
	ns1__PrivateAAFQuestion* retValue = NULL;

	if (iResponseStruct.GetPrivateAAFQuestionResult)
	{
		if (aQuestionIndex >= 0 && aQuestionIndex < iResponseStruct.GetPrivateAAFQuestionResult->__sizePrivateAAFQuestion)
		{
			retValue = iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[aQuestionIndex];
		}
	}

	return retValue;
}

TInt CAafPrivateQuestionsProvider::ThreadEntryPoint(TAny* aParameters)
{
	__ASSERT_ALWAYS(aParameters, Panic(EInvalidArgument));

	// Create Cleanup stack
	CTrapCleanup* cleanup = CTrapCleanup::New();

	if (cleanup)
	{
		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::ThreadEntryPoint() begins");

		// Get thread entry point data
		CAafPrivateQuestionsProvider* self = reinterpret_cast<CAafPrivateQuestionsProvider*>(aParameters);

		// If credentials are invalid
		if (!self || !self->iLoginProvider->GetMemberID() || !self->iLoginProvider->GetPassword())
		{
			__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::ThreadEntryPoint() ends");

			// Delete cleanup stack
			delete cleanup;

			return KErrArgument;
		}

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::ThreadEntryPoint() step 01");	

		// Free previously allocated memory if necessary (response struct)
		if (self->iResponseStruct.GetPrivateAAFQuestionResult)
		{
			for (int i = 0; i < self->iResponseStruct.GetPrivateAAFQuestionResult->__sizePrivateAAFQuestion; i++)
			{
				delete [] self->iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->DateTimePosted;
				self->iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->DateTimePosted = NULL;

				delete [] self->iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->NickName;
				self->iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->NickName = NULL;

				delete [] self->iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->Question;
				self->iResponseStruct.GetPrivateAAFQuestionResult->PrivateAAFQuestion[i]->Question = NULL;
			}

			delete self->iResponseStruct.GetPrivateAAFQuestionResult;
			self->iResponseStruct.GetPrivateAAFQuestionResult = NULL;
		}

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::ThreadEntryPoint() step 02");

		TInt err;

		TRAP(err, self->iRequestStruct.WebMemberID = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetMemberID()));
		TRAP(err, self->iRequestStruct.WebPassword = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetPassword()));

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::ThreadEntryPoint() step 03");
		
		// Set appropriate sockets method pointers etc
		self->iAskFriendProxy->user = self;

		self->iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
		self->iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
		self->iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
		self->iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

		TInt retValue = KErrNone;

		// Perform a long synchronous task
		int res = self->iAskFriendProxy->GetPrivateAAFQuestion(&self->iRequestStruct, &self->iResponseStruct);

		__LOGSTR_TOFILE1("CAafPrivateQuestionsProvider::ThreadEntryPoint() step 04 with res == %d", res);

		if (res != SOAP_OK)
		{
			retValue = KErrUnknown;
		}		

		// Task is complete so end this thread with returned error code
		self->iThread.Kill(res);

		__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::ThreadEntryPoint() ends");

			
		delete [] self->iRequestStruct.WebMemberID;
		self->iRequestStruct.WebMemberID = NULL;

		delete [] self->iRequestStruct.WebPassword;
		self->iRequestStruct.WebPassword = NULL;

		// Delete cleanup stack
		delete cleanup;

		return retValue; // This value is discarded
	}
	else
	{
		return KErrNoMemory;
	}	
}

void CAafPrivateQuestionsProvider::DoCancel()
{
	__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::DoCancel() begins");

	// Kill the thread and complete with KErrCancel
	//ONLY if it is still running
	TExitType threadExitType = iThread.ExitType();

	if (threadExitType == EExitPending)
	{
		// Thread is still running
		iThread.LogonCancel(iStatus);

		iThread.Kill(KErrCancel);

		iThread.Close();

		// Complete the caller
		iRequestObserver->HandleRequestCompletedL(KErrCancel);
	}

	__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::DoCancel() ends");
}

void CAafPrivateQuestionsProvider::RunL()
{
	__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RunL() begins");

	// Check in case thread is still running e.g. if Logon() failed
	TExitType threadExitType = iThread.ExitType();

	if (threadExitType == EExitPending) // Thread is still running, kill it
		iThread.Kill(KErrNone);

	__LOGSTR_TOFILE1("CAafPrivateQuestionsProvider::RunL() called with error code == %d", iStatus.Int());

	// Complete the caller, passing iStatus value to RThread::Kill()
	iRequestObserver->HandleRequestCompletedL(iStatus.Int());

	iThread.Close(); // Close the thread handle, no need to LogonCancel()

	__LOGSTR_TOFILE("CAafPrivateQuestionsProvider::RunL() ends");
}

TInt CAafPrivateQuestionsProvider::RunError(TInt anError)
{
	__LOGSTR_TOFILE1("CAafPrivateQuestionsProvider::RunError() called with error code == %d", anError);

	iRequestObserver->HandleRequestCompletedL(anError);

	return KErrNone;
}

// end of file
