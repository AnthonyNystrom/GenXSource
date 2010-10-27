/*
============================================================================
Name        : Aafuserquestionsserviceprovider.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : User questions stuff controller class implementation
============================================================================
*/

// INCLUDE FILES
#include "aafuserquestionsserviceprovider.h"
#include "Aafsocketutils.h"
#include <es_sock.h>
#include "Aafloginserviceprovider.h"
#include "Aafconnectionmanager.h"
#include "Aafuploadserviceprovider.h"
#include "Aafapputils.h"
#include "aaf.pan"

_LIT(KThreadName, "N2FUserQuestionsThread"); // Name of the new thread


CAafUserQuestionsProvider::CAafUserQuestionsProvider()
:CCoeStatic(KUidAafUserQuestionsController),
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

CAafUserQuestionsProvider* CAafUserQuestionsProvider::GetInstanceL()
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::GetInstanceL() begins");

	CAafUserQuestionsProvider* controllerInstance = static_cast<CAafUserQuestionsProvider*>
		( CCoeEnv::Static( KUidAafUserQuestionsController ) );

	if (!controllerInstance)
	{
		// Create instance
		controllerInstance = new (ELeave)CAafUserQuestionsProvider;
		
		controllerInstance->iAskFriendProxy = new (ELeave)AskAFriendWSSoapProxy;

		controllerInstance->iAskFriendProxy->soap_endpoint = N2F_ASKAFRIEND_ENDPOINT;
		
		controllerInstance->iLoginProvider = CAafLoginServiceProvider::GetInstanceL();
				
		controllerInstance->iConnManager = CAafConnectionManager::GetInstanceL();
				
		controllerInstance->iSocket = new (ELeave)RSocket;
	}

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::GetInstanceL() ends");

	return controllerInstance;
}

CAafUserQuestionsProvider::~CAafUserQuestionsProvider()
{
	// Cancel any outstanding request
	Cancel();

	// Free questions array
	for (TInt i = 0; i < iQuestionsArray.Count(); i++)
	{
		if (iQuestionsArray[i])
		{
			delete iQuestionsArray[i];
			iQuestionsArray[i] = NULL;
		}		
	}

	iQuestionsArray.Close();

	// Free comments array
	for (TInt i = 0; i < iCommentsArray.Count(); i++)
	{
		if (iCommentsArray[i])
		{
			delete iCommentsArray[i];
			iCommentsArray[i] = NULL;
		}		
	}

	iCommentsArray.Close();

	// Soap environment deinitialization
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


TInt CAafUserQuestionsProvider::RequestForQuestions(MRequestObserver* aRequestObserver)
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() begins");

	__ASSERT_ALWAYS(aRequestObserver, Panic(ENoObserver));

	// Set observer of the login request
	iRequestObserver = aRequestObserver;

	if (IsActive())
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() ends because KErrAlreadyExists");

		return KErrAlreadyExists;
	}

	// Create a new thread, passing the thread function and stack sizes
	// No extra parameters are required
	TInt res = iThread.Create(KThreadName, ThreadEntryPoint1, KDefaultStackSize, NULL, this);

	if(res != KErrNone)
	{ 
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() ends because res != KErrNone");

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

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() ends");

		return KErrNone;
	}
}

TInt CAafUserQuestionsProvider::RequestForQuestions()
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() begins");

	CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();

	// If credentials are invalid
	if (!loginProvider || !loginProvider->GetMemberID() || !loginProvider->GetPassword())
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() ends");

		
		return KErrArgument;
	}

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() step 01");

	// Set credentials
	iQuestionsRequestStruct.WebMemberID = CAafUtils::DescriptorToString8L(*loginProvider->GetMemberID());
	iQuestionsRequestStruct.WebPassword = CAafUtils::DescriptorToString8L(*loginProvider->GetPassword());

	// If current questions array is not empty,
	// set last web question id
	if (iQuestionsArray.Count())
	{
		iQuestionsRequestStruct.LastWebAskAFriendID = iQuestionsArray[iQuestionsArray.Count()-1]->WebAskAFriendID;
	}
	// Otherwise
	else
	{
		iQuestionsRequestStruct.LastWebAskAFriendID = NULL;
	}

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() step 03");

	// Set appropriate method pointers etc
	RSocket socket;
	iAskFriendProxy->user = &socket;

	iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
	iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
	iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
	iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

	TInt retValue = KErrNone;

	// Perform a long synchronous task
	int res = iAskFriendProxy->GetMyAAFQuestions(&iQuestionsRequestStruct, &iQuestionsResponseStruct);

	// If questions have been retrieved successfully
	if (res == SOAP_OK)
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForQuestions() step 04");

		if (iQuestionsResponseStruct.GetMyAAFQuestionsResult)
		{
			// Save questions from response to the questions array
			for (int i = 0; i < iQuestionsResponseStruct.GetMyAAFQuestionsResult->__sizeAskAFriendQuestion; i++)
			{
				iQuestionsArray.Append(iQuestionsResponseStruct.GetMyAAFQuestionsResult->AskAFriendQuestion[i]);
			}

			// Init comments array if necessary
			if (!iCommentsArray.Count())
			{
				iCommentsArray = RPointerArray<_ns1__QuestionComments>(GetQuestionCount());
			}
		}		
	}
	else
	{
		retValue = KErrUnknown;	
	}

	__LOGSTR_TOFILE1("CAafUserQuestionsProvider::RequestForQuestions() step 04 with res == %d", res);

	// Free dynamically allocated memory
	delete [] 	iQuestionsRequestStruct.WebMemberID;
	iQuestionsRequestStruct.WebMemberID = NULL;

	delete [] iQuestionsRequestStruct.WebPassword;
	iQuestionsRequestStruct.WebPassword = NULL;

	
	return retValue; // This value is discarded
}

TInt CAafUserQuestionsProvider::RequestForComments(MRequestObserver* aRequestObserver, TInt aQuestionIndex)
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() begins");

	__ASSERT_ALWAYS(aRequestObserver, Panic(ENoObserver));

	// Set observer of the login request
	iRequestObserver = aRequestObserver;

	if (IsActive())
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() ends because KErrAlreadyExists");

		return KErrAlreadyExists;
	}

	
	// Create a new thread, passing the thread function and stack sizes
	// No extra parameters are required
	TInt res = iThread.Create(KThreadName, ThreadEntryPoint2, KDefaultStackSize, NULL, this);

	if(res != KErrNone)
	{ 
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() ends because res != KErrNone");

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

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() ends");

		return KErrNone;
	}
}

TInt CAafUserQuestionsProvider::RequestForComments(TInt aQuestionIndex)
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() begins");

	CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();

	// If credentials are invalid
	if (!loginProvider || !loginProvider->GetMemberID() || !loginProvider->GetPassword())
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() ends");

		return KErrArgument;
	}

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() step 01");


	// Set credentials
	iCommentsRequestStruct.WebMemberID = CAafUtils::DescriptorToString8L(*loginProvider->GetMemberID());
	iCommentsRequestStruct.WebPassword = CAafUtils::DescriptorToString8L(*loginProvider->GetPassword());

	iCommentsRequestStruct.WebAskAFriendID = iQuestionsResponseStruct.GetMyAAFQuestionsResult->AskAFriendQuestion[(int)iCurrentQuestion]->WebAskAFriendID;
	
	// Set last comment web id
	if (iCommentsArray.Count())
	{
		if (iCurrentQuestion < iCommentsArray.Count())
		{
			TInt commentsCount = iCommentsArray[iCurrentQuestion]->iQuestionComments.Count();

			iCommentsRequestStruct.LastWebAskAFriendCommentID = iCommentsArray[iCurrentQuestion]->iQuestionComments[commentsCount-1]->WebAskAFriendCommentID;
		}
		else
		{
			iCommentsRequestStruct.LastWebAskAFriendCommentID = NULL;
		}
	}
	else
	{
		iCommentsRequestStruct.LastWebAskAFriendCommentID = NULL;
	}

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RequestForComments() step 03");

	// Set appropriate method pointers etc
	RSocket socket;
	iAskFriendProxy->user = &socket;

	iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
	iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
	iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
	iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

	TInt retValue = KErrNone;

	// Perform a long synchronous task
	int res = iAskFriendProxy->GetAAFComments(&iCommentsRequestStruct, &iCommentsResponseStruct);

	// If comments have been retrieved successfully
	if (res == SOAP_OK)
	{
		// Save comments from response to the comments array

		// If comments array for the current question has been initialized
		if (iCurrentQuestion < iCommentsArray.Count())
		{
			for (int i = 0; i < iCommentsResponseStruct.GetAAFCommentsResult->__sizeAskAFriendComment; i++)
			{
				iCommentsArray[iCurrentQuestion]->iQuestionComments.Append(iCommentsResponseStruct.GetAAFCommentsResult->AskAFriendComment[i]);
			}
		}
		// Otherwise
		else
		{
			_ns1__QuestionComments* comments = new (ELeave)_ns1__QuestionComments;
			comments->iQuestionId = iQuestionsArray[iCurrentQuestion]->WebAskAFriendID;

			for (int i = 0; i < iCommentsResponseStruct.GetAAFCommentsResult->__sizeAskAFriendComment; i++)
			{
				comments->iQuestionComments.Append(iCommentsResponseStruct.GetAAFCommentsResult->AskAFriendComment[i]);
			}

			iCommentsArray.Append(comments);
		}
	}
	else
	{
		retValue = KErrUnknown;
	}

	__LOGSTR_TOFILE1("CAafUserQuestionsProvider::RequestForComments() step 04 with res == %d", res);
	
	// Free dynamically allocated memory
	delete [] iCommentsRequestStruct.WebMemberID;
	iCommentsRequestStruct.WebMemberID = NULL;

	delete [] iCommentsRequestStruct.WebPassword;
	iCommentsRequestStruct.WebPassword = NULL;

	
	return retValue; // This value is discarded
}

TInt CAafUserQuestionsProvider::SubmitQuestion(MRequestObserver* aRequestObserver)
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::SubmitQuestion() begins");

	__ASSERT_ALWAYS(aRequestObserver, Panic(ENoObserver));

	// Set observer of the login request
	iRequestObserver = aRequestObserver;

	if (IsActive())
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::SubmitQuestion() ends because KErrAlreadyExists");

		return KErrAlreadyExists;
	}

		
	// Create a new thread, passing the thread function and stack sizes
	// No extra parameters are required
	TInt res = iThread.Create(KThreadName, ThreadEntryPoint3, KDefaultStackSize, NULL, this);

	if(res != KErrNone)
	{ 
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::SubmitQuestion() ends because res != KErrNone");

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

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::SubmitQuestion() ends");

		return KErrNone;
	}
}

TInt CAafUserQuestionsProvider::SubmitQuestion()
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::SubmitQuestion() begins");

	CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();

	// If credentials are invalid
	if (!loginProvider->GetMemberID() || !loginProvider->GetPassword())
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::SubmitQuestion() ends");

		return KErrArgument;
	}

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::SubmitQuestion() step 01");


	// Set credentials
	iQuestionSubmitStruct.WebMemberID = CAafUtils::DescriptorToString8L(*loginProvider->GetMemberID());
	iQuestionSubmitStruct.WebPassword = CAafUtils::DescriptorToString8L(*loginProvider->GetPassword());

	// Set question data
	iQuestionSubmitStruct.Question = CAafUtils::DescriptorToStringL(iQuestionData.iQuestionText); // Question text
	// Question type
	iQuestionSubmitStruct.ResponseType = (int)iQuestionData.iQuestionType;

	// Set custom responses if required
	if (iQuestionSubmitStruct.ResponseType == 1)
	{
		iQuestionSubmitStruct.CustomResponses->__sizestring = 2;

		iQuestionSubmitStruct.CustomResponses->string = (char**)malloc(2);

		iQuestionSubmitStruct.CustomResponses->string[0] = CAafUtils::DescriptorToStringL(iQuestionData.iCustomResponseA);
		iQuestionSubmitStruct.CustomResponses->string[1] = CAafUtils::DescriptorToStringL(iQuestionData.iCustomResponseB);
	}

	// Question duration
	iQuestionSubmitStruct.Duration = (int)iQuestionData.iQuestionDuration;

	// Private mark
	iQuestionSubmitStruct.IsPrivate = (bool)iQuestionData.iPrivateMark;

	// Photos count
	CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

	int photosCount = uploadProvider->GetSelectionArray().Count();

	if (photosCount > 3)
		photosCount = 3;

	iQuestionSubmitStruct.NumberOfPhotos = (int)photosCount;

	// Set appropriate method pointers etc
	RSocket socket;
	iAskFriendProxy->user = &socket;

	iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
	iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
	iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
	iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

	TInt retValue = KErrNone;

	// Perform a long synchronous task
	int res = iAskFriendProxy->SubmitQuestion(&iQuestionSubmitStruct, &iSubmitResponseStruct);

	// If question hasn't been submitted successfully
	if (res != SOAP_OK)
	{
		retValue = KErrUnknown;
	}	

	__LOGSTR_TOFILE1("CAafUserQuestionsProvider::SubmitQuestion() step 04 with res == %d", res);

	
	// Free memory if required
	delete [] iQuestionSubmitStruct.WebMemberID;
	iQuestionSubmitStruct.WebMemberID = NULL;

	delete [] iQuestionSubmitStruct.WebPassword;
	iQuestionSubmitStruct.WebPassword = NULL;

	delete [] iQuestionSubmitStruct.Question;
	iQuestionSubmitStruct.Question = NULL;

	// Custom responses
	if (iQuestionSubmitStruct.ResponseType == 1)
	{
		// Free memory if required
		if (iQuestionSubmitStruct.CustomResponses->string)
		{
			delete [] iQuestionSubmitStruct.CustomResponses->string;
			iQuestionSubmitStruct.CustomResponses->string = NULL;
		}

		delete [] iQuestionSubmitStruct.CustomResponses->string[0];
		iQuestionSubmitStruct.CustomResponses->string[0] = NULL;

		delete [] iQuestionSubmitStruct.CustomResponses->string[1];
		iQuestionSubmitStruct.CustomResponses->string[1] = NULL;
	}

	return retValue; // This value is discarded
}

void CAafUserQuestionsProvider::SetQuestionData(_ns1__QuestionData& aQuestionData)
{
	// Set currently submitted question data
	iQuestionData = aQuestionData;
}

HBufC8* CAafUserQuestionsProvider::GetSubmittedQuestionId()
{
	return CAafUtils::StringToDescriptor8LC(iSubmitResponseStruct.SubmitQuestionResult->WebAskAFriendID);
}

TInt CAafUserQuestionsProvider::GetQuestionCount()
{
	return iQuestionsArray.Count();
}

TInt CAafUserQuestionsProvider::GetCommentCount(TInt aQuestionIndex)
{
	TInt retValue = 0;

	// If aQuestionIndex has valid value
	if (aQuestionIndex >= 0 && aQuestionIndex < iCommentsArray.Count())
	{
		iCommentsArray[aQuestionIndex]->iQuestionComments.Count();
	}

	return retValue;
}

void CAafUserQuestionsProvider::GetQuestionListItemsL(CDesCArray* aItems)
{
	__LOGSTR_TOFILE("CAafMyQuestionsServiceProvider::GetFileListItemsL() begins");

	TBuf<260> listboxItem;
	// Set to listbox item question's text, member nickname and question's date and time
	for (TInt i = 0; i < iQuestionsArray.Count(); i++)
	{
		HBufC* questionText = CAafUtils::StringToDescriptorLC(iQuestionsArray[i]->Question);

		if (questionText)
		{
			// Format listbox item string
			listboxItem.Format(KListboxWithIconPattern, 0, questionText);
			aItems->AppendL(listboxItem);	
		}

		CleanupStack::PopAndDestroy(questionText);
	}

	__LOGSTR_TOFILE("CAafMyQuestionsServiceProvider::GetFileListItemsL() ends");
}

void CAafUserQuestionsProvider::GetCommentListItems(TInt aQuestionIndex, CDesCArray* aItems)
{
	// If aQuestionIndex has valid value
	if (aQuestionIndex >= 0 && aQuestionIndex < iCommentsArray.Count())
	{
		TBuf<330> listboxItem;
		TInt commentCount = iCommentsArray[aQuestionIndex]->iQuestionComments.Count();

		for (TInt i = 0; i < commentCount; i++)
		{
			// Form comment time string
			TTime commentTime;

			// Convert string to TTime structure
			HBufC8* dateDescriptor = CAafUtils::StringToDescriptor8LC(iCommentsArray[aQuestionIndex]->iQuestionComments[i]->DateTimePosted);
			TInt64 dateAndTime;
			TLex8 dateTime(dateDescriptor->Des());
			TInt err = dateTime.Val(dateAndTime);

			commentTime = (dateAndTime/TInt64(10));

			// Format date and time string
			TBuf<30> dateString;
			commentTime.FormatL(dateString, KDateString);

			CleanupStack::PopAndDestroy(dateDescriptor);

			HBufC8* nickName = CAafUtils::StringToDescriptor8LC(iCommentsArray[aQuestionIndex]->iQuestionComments[i]->NickName);
			HBufC* commentText = CAafUtils::StringToDescriptorLC(iCommentsArray[aQuestionIndex]->iQuestionComments[i]->Text);

			listboxItem.Format(KDoubleListboxWithoutIconPattern, commentText, nickName, &dateString);
			aItems->AppendL(listboxItem);

			// Free allocated memory
			CleanupStack::PopAndDestroy(commentText);
			CleanupStack::PopAndDestroy(nickName);
		}		
	}
}

ns1__AskAFriendQuestion* CAafUserQuestionsProvider::GetQuestion(TInt aQuestionIndex)
{
	ns1__AskAFriendQuestion* retValue = NULL;

	if (aQuestionIndex >= 0 && aQuestionIndex < iQuestionsArray.Count())
	{
		retValue = iQuestionsArray[aQuestionIndex];
	}

	return retValue;
}

ns1__AskAFriendComment* CAafUserQuestionsProvider::GetComment(TInt aQuestionIndex, TInt aCommentIndex)
{
	ns1__AskAFriendComment* retValue = NULL;

	if ((aQuestionIndex >= 0 && aQuestionIndex < iQuestionsArray.Count()))
	{
		if (aCommentIndex >= 0 && aCommentIndex < iCommentsArray[aQuestionIndex]->iQuestionComments.Count())
		{
			retValue = iCommentsArray[aQuestionIndex]->iQuestionComments[aCommentIndex];
		}
	}

	return retValue;
}

TInt CAafUserQuestionsProvider::ThreadEntryPoint1(TAny* aParameters)
{
	__ASSERT_ALWAYS(aParameters, Panic(EInvalidArgument));

	// Create Cleanup stack
	CTrapCleanup* cleanup = CTrapCleanup::New();

	if (cleanup)
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint1() begins");

		// Get thread entry point data
		 CAafUserQuestionsProvider* self = reinterpret_cast< CAafUserQuestionsProvider*>(aParameters);

		// If credentials are invalid
		if (!self->iLoginProvider->GetMemberID() || !self->iLoginProvider->GetPassword())
		{
			__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint1() ends");

			delete cleanup;

			return KErrArgument;
		}

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint1() step 01");

		TInt err;

		// Set credentials
		TRAP(err, self->iQuestionsRequestStruct.WebMemberID = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetMemberID()));
		TRAP(err, self->iQuestionsRequestStruct.WebPassword = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetPassword()));

		// If current questions array is not empty,
		// set last web question id
		if (self->iQuestionsArray.Count())
		{
			self->iQuestionsRequestStruct.LastWebAskAFriendID = self->iQuestionsArray[self->iQuestionsArray.Count()-1]->WebAskAFriendID;
		}
		// Otherwise
		else
		{
			self->iQuestionsRequestStruct.LastWebAskAFriendID = NULL;
		}

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint1() step 03");

		// Set appropriate method pointers etc
		self->iAskFriendProxy->user = self;

		self->iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
		self->iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
		self->iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
		self->iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

		TInt retValue = KErrNone;

		// Perform a long synchronous task
		int res = self->iAskFriendProxy->GetMyAAFQuestions(&self->iQuestionsRequestStruct, &self->iQuestionsResponseStruct);

		// If questions have been retrieved successfully
		if (res == SOAP_OK)
		{
			// Save questions from response to the questions array
			for (int i = 0; i < self->iQuestionsResponseStruct.GetMyAAFQuestionsResult->__sizeAskAFriendQuestion; i++)
			{
				self->iQuestionsArray.Append(self->iQuestionsResponseStruct.GetMyAAFQuestionsResult->AskAFriendQuestion[i]);
			}

			// Init comments array if necessary
			if (!self->iCommentsArray.Count())
			{
				self->iCommentsArray = RPointerArray<_ns1__QuestionComments>(self->GetQuestionCount());
			}
		}
		else
		{
			retValue = KErrUnknown;	
		}

		__LOGSTR_TOFILE1("CAafUserQuestionsProvider::ThreadEntryPoint1() step 04 with res == %d", res);

		// Task is complete so end this thread with returned error code
		self->iThread.Kill(res);

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint1() ends");

		// Free dynamically allocated memory
		delete [] 	self->iQuestionsRequestStruct.WebMemberID;
		self->iQuestionsRequestStruct.WebMemberID = NULL;

		delete [] self->iQuestionsRequestStruct.WebPassword;
		self->iQuestionsRequestStruct.WebPassword = NULL;

		// Unintialize cleanup stack
		delete cleanup;

		return retValue; // This value is discarded
	}
	else
	{
		return KErrNoMemory;
	}
	
}

TInt CAafUserQuestionsProvider::ThreadEntryPoint2(TAny* aParameters)
{
	__ASSERT_ALWAYS(aParameters, Panic(EInvalidArgument));

	// Create Cleanup stack
	CTrapCleanup* cleanup = CTrapCleanup::New();

	if (cleanup)
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint2() begins");

		// Get thread entry point data
		 CAafUserQuestionsProvider* self = reinterpret_cast< CAafUserQuestionsProvider*>(aParameters);

		// If credentials are invalid
		if (!self->iLoginProvider->GetMemberID() || !self->iLoginProvider->GetPassword())
		{
			__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint1() ends");

			delete cleanup;

			return KErrArgument;
		}

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint2() step 01");

		TInt err;

		// Set credentials
		TRAP(err, self->iCommentsRequestStruct.WebMemberID = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetMemberID()));
		TRAP(err, self->iCommentsRequestStruct.WebPassword = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetPassword()));

		self->iCommentsRequestStruct.WebAskAFriendID = self->iQuestionsResponseStruct.GetMyAAFQuestionsResult->AskAFriendQuestion[(int)self->iCurrentQuestion]->WebAskAFriendID;
		// Set last comment web id
		if (self->iCommentsArray.Count())
		{
			if (self->iCurrentQuestion < self->iCommentsArray.Count())
			{
				TInt commentsCount = self->iCommentsArray[self->iCurrentQuestion]->iQuestionComments.Count();

				self->iCommentsRequestStruct.LastWebAskAFriendCommentID = self->iCommentsArray[self->iCurrentQuestion]->iQuestionComments[commentsCount-1]->WebAskAFriendCommentID;
			}
			else
			{
				self->iCommentsRequestStruct.LastWebAskAFriendCommentID = NULL;
			}
		}
		else
		{
			self->iCommentsRequestStruct.LastWebAskAFriendCommentID = NULL;
		}

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint2() step 03");

		// Set appropriate method pointers etc
		self->iAskFriendProxy->user = self;

		self->iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
		self->iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
		self->iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
		self->iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

		TInt retValue = KErrNone;

		// Perform a long synchronous task
		int res = self->iAskFriendProxy->GetAAFComments(&self->iCommentsRequestStruct, &self->iCommentsResponseStruct);

		// If comments have been retrieved successfully
		if (res == SOAP_OK)
		{
			// Save comments from response to the comments array

			// If comments array for the current question has been initialized
			if (self->iCurrentQuestion < self->iCommentsArray.Count())
			{
				for (int i = 0; i < self->iCommentsResponseStruct.GetAAFCommentsResult->__sizeAskAFriendComment; i++)
				{
					self->iCommentsArray[self->iCurrentQuestion]->iQuestionComments.Append(self->iCommentsResponseStruct.GetAAFCommentsResult->AskAFriendComment[i]);
				}
			}
			// Otherwise
			else
			{
				_ns1__QuestionComments* comments = new (ELeave)_ns1__QuestionComments;
				comments->iQuestionId = self->iQuestionsArray[self->iCurrentQuestion]->WebAskAFriendID;

				for (int i = 0; i < self->iCommentsResponseStruct.GetAAFCommentsResult->__sizeAskAFriendComment; i++)
				{
					comments->iQuestionComments.Append(self->iCommentsResponseStruct.GetAAFCommentsResult->AskAFriendComment[i]);
				}

				self->iCommentsArray.Append(comments);
			}
		}
		else
		{
			retValue = KErrUnknown;
		}

		__LOGSTR_TOFILE1("CAafUserQuestionsProvider::ThreadEntryPoint2() step 04 with res == %d", res);

		// Task is complete so end this thread with returned error code
		self->iThread.Kill(res);

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint2() ends");

		// Free dynamically allocated memory
		delete [] self->iCommentsRequestStruct.WebMemberID;
		self->iCommentsRequestStruct.WebMemberID = NULL;

		delete [] self->iCommentsRequestStruct.WebPassword;
		self->iCommentsRequestStruct.WebPassword = NULL;

		// Uninitialize cleanup stack
		delete cleanup;

		return retValue; // This value is discarded
	}
	else
	{
		return KErrNoMemory;
	}	
}

TInt CAafUserQuestionsProvider::ThreadEntryPoint3(TAny* aParameters)
{
	__ASSERT_ALWAYS(aParameters, Panic(EInvalidArgument));

	// Create Cleanup stack
	CTrapCleanup* cleanup = CTrapCleanup::New();

	if (cleanup)
	{
		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint3() begins");

		// Get thread entry point data
		 CAafUserQuestionsProvider* self = reinterpret_cast< CAafUserQuestionsProvider*>(aParameters);

		// If credentials are invalid
		if (!self->iLoginProvider->GetMemberID() || !self->iLoginProvider->GetPassword())
		{
			__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint1() ends");

			delete cleanup;

			return KErrArgument;
		}

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint3() step 01");

		
		TInt err;

		// Set credentials
		TRAP(err, self->iQuestionSubmitStruct.WebMemberID = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetMemberID()));
		TRAP(err, self->iQuestionSubmitStruct.WebPassword = CAafUtils::DescriptorToString8L(*self->iLoginProvider->GetPassword()));

		// Set question data
		TRAP(err, self->iQuestionSubmitStruct.Question = CAafUtils::DescriptorToStringL(self->iQuestionData.iQuestionText)); // Question text
		// Question type
		self->iQuestionSubmitStruct.ResponseType = (int)self->iQuestionData.iQuestionType;

		// Set custom responses if required
		if (self->iQuestionSubmitStruct.ResponseType == 1)
		{
			self->iQuestionSubmitStruct.CustomResponses->__sizestring = 2;

			self->iQuestionSubmitStruct.CustomResponses->string = (char**)malloc(2);

			TRAP(err, self->iQuestionSubmitStruct.CustomResponses->string[0] = CAafUtils::DescriptorToStringL(self->iQuestionData.iCustomResponseA));
			TRAP(err, self->iQuestionSubmitStruct.CustomResponses->string[1] = CAafUtils::DescriptorToStringL(self->iQuestionData.iCustomResponseB));
		}

		// Question duration
		self->iQuestionSubmitStruct.Duration = (int)self->iQuestionData.iQuestionDuration;

		// Private mark
		self->iQuestionSubmitStruct.IsPrivate = (bool)self->iQuestionData.iPrivateMark;

		// Photos count
		CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

		int photosCount = uploadProvider->GetSelectionArray().Count();

		if (photosCount > 3)
			photosCount = 3;

		self->iQuestionSubmitStruct.NumberOfPhotos = (int)photosCount;

		// Set appropriate method pointers etc
		self->iAskFriendProxy->user = self;

		self->iAskFriendProxy->fopen = CAafSocketUtils::SocketOpen;
		self->iAskFriendProxy->frecv = CAafSocketUtils::SocketRead;
		self->iAskFriendProxy->fsend = CAafSocketUtils::SocketWrite;
		self->iAskFriendProxy->fclose = CAafSocketUtils::SocketClose;

		TInt retValue = KErrNone;

		// Perform a long synchronous task
		int res = self->iAskFriendProxy->SubmitQuestion(&self->iQuestionSubmitStruct, &self->iSubmitResponseStruct);

		// If question hasn't been submitted successfully
		if (res != SOAP_OK)
		{
			retValue = KErrUnknown;
		}	

		__LOGSTR_TOFILE1("CAafUserQuestionsProvider::ThreadEntryPoint3() step 04 with res == %d", res);

		// Task is complete so end this thread with returned error code
		self->iThread.Kill(res);

		__LOGSTR_TOFILE("CAafUserQuestionsProvider::ThreadEntryPoint3() ends");

		// Free memory if required
		delete [] self->iQuestionSubmitStruct.WebMemberID;
		self->iQuestionSubmitStruct.WebMemberID = NULL;

		delete [] self->iQuestionSubmitStruct.WebPassword;
		self->iQuestionSubmitStruct.WebPassword = NULL;

		delete [] self->iQuestionSubmitStruct.Question;
		self->iQuestionSubmitStruct.Question = NULL;

		// Custom responses
		if (self->iQuestionSubmitStruct.ResponseType == 1)
		{
			// Free memory if required
			if (self->iQuestionSubmitStruct.CustomResponses->string)
			{
				delete [] self->iQuestionSubmitStruct.CustomResponses->string;
				self->iQuestionSubmitStruct.CustomResponses->string = NULL;
			}

			delete [] self->iQuestionSubmitStruct.CustomResponses->string[0];
			self->iQuestionSubmitStruct.CustomResponses->string[0] = NULL;

			delete [] self->iQuestionSubmitStruct.CustomResponses->string[1];
			self->iQuestionSubmitStruct.CustomResponses->string[1] = NULL;
		}

		// Uninitialize cleanup stack
		delete cleanup;

		return retValue; // This value is discarded
	}
	{
		return KErrNoMemory;
	}	
}

void CAafUserQuestionsProvider::DoCancel()
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::DoCancel() begins");

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

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::DoCancel() ends");
}

void CAafUserQuestionsProvider::RunL()
{
	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RunL() begins");

	// Check in case thread is still running e.g. if Logon() failed
	TExitType threadExitType = iThread.ExitType();

	if (threadExitType == EExitPending) // Thread is still running, kill it
		iThread.Kill(KErrNone);

	__LOGSTR_TOFILE1("CAafUserQuestionsProvider::RunL() called with error code == %d", iStatus.Int());

	// Complete the caller, passing iStatus value to RThread::Kill()
	iRequestObserver->HandleRequestCompletedL(iStatus.Int());

	iThread.Close(); // Close the thread handle, no need to LogonCancel()

	__LOGSTR_TOFILE("CAafUserQuestionsProvider::RunL() ends");
}

TInt CAafUserQuestionsProvider::RunError(TInt anError)
{
	__LOGSTR_TOFILE1("CAafUserQuestionsProvider::RunError() called with error code == %d", anError);

	iRequestObserver->HandleRequestCompletedL(anError);

	return(KErrNone);
}

// Auxilary class for CAafUserQuestionsProvider class internal usage
CAafUserQuestionsProvider::_ns1__QuestionComments::_ns1__QuestionComments()
:iQuestionId(NULL), iQuestionComments(RPointerArray<ns1__AskAFriendComment>(0))
{
}

CAafUserQuestionsProvider::_ns1__QuestionComments::~_ns1__QuestionComments()
{
	// Perform memory deallocation
	for (TInt i = 0; i < iQuestionComments.Count(); i++)
	{
		if (iQuestionComments[i])
		{
			delete iQuestionComments[i];
			iQuestionComments[i] = NULL;
		}
	}

	iQuestionComments.Close();
}

// end of file
