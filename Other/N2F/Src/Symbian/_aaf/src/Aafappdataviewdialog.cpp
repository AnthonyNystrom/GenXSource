/*
============================================================================
Name        : Aafappdataviewdialog.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : General view dialog to display various data
============================================================================
*/
#include "Aafappdataviewdialog.h"
#include "Aafappui.h"
#include "aafuserquestionsserviceprovider.h"
#include "aafprivatequestionsserviceprovider.h"
#include "Aafapputils.h"
#include "Aafappcommentsview.h"
#include "Aafappprivatequestionsview.h"
#include "Aafappresponsesview.h"
#include "common.h"

CAafAppDataViewDialog* CAafAppDataViewDialog::ConstructL(TViewDialogType aDataType)
{
	CAafAppDataViewDialog* self = new (ELeave)CAafAppDataViewDialog(aDataType);
	CleanupStack::PushL(self);

	// Set dialog read only
	self->SetEditableL(EFalse);

	CleanupStack::Pop(self);

	return self;
}

CAafAppDataViewDialog::CAafAppDataViewDialog()
{
	iCurrentData = EPrivateQuestion;
}

CAafAppDataViewDialog::CAafAppDataViewDialog(TViewDialogType aDataType)
{
	iCurrentData = aDataType;
}

CAafAppDataViewDialog::~CAafAppDataViewDialog()
{
	if (iNickName)
	{
		delete iNickName;
		iNickName = NULL;
	}

	if (iDateTime)
	{
		delete iDateTime;
		iDateTime = NULL;
	}

	if (iText)
	{
		delete iText;
		iText = NULL;
	}
}

TInt CAafAppDataViewDialog::ShowLD()
{
	return ExecuteLD(R_DATAVIEW_DIALOG);
}

void CAafAppDataViewDialog::PreLayoutDynInitL()
{
	// Get pointer to application ui controller class
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	CAafAppUi* appUi = (CAafAppUi*)eikonEnv->AppUi();

	// Perform appropriate action
	switch(iCurrentData)
	{
	case EUserQuestion:
		{
			// Create necessary controls
			// Question text
			CCoeControl* control = CreateLineByTypeL(KNullDesC, EUserQuestionText, EEikCtEdwin, NULL);
			iText = static_cast<CEikEdwin*>(control);

			iText->ConstructL( EEikEdwinNoHorizScrolling |EEikEdwinNoAutoSelection
				|EEikEdwinResizable, 10, 256, 1 );

			iText->CreateTextViewL();

			// Set data from service provider
			CAafUserQuestionsProvider* questionProvider = CAafUserQuestionsProvider::GetInstanceL();

			TInt currentQuestion = appUi->GetUserQuestionsView()->CurrentItemIndex();
			
			ns1__AskAFriendQuestion* questionData = NULL;
			
			questionData = questionProvider->GetQuestion(currentQuestion);

			// If question data has been retrieved successfully
			if (questionData)
			{
				HBufC* questionDescriptor = CAafUtils::StringToDescriptorLC(questionData->Question);
				
				iText->SetTextL(questionDescriptor);

				CleanupStack::PopAndDestroy(questionDescriptor);
			}			
		}
		break;
	case EQuestionComment:
		{
			// Create necessary controls
			// Nickname
			CCoeControl* controlNickname = CreateLineByTypeL(KNullDesC, EPrivateQuestionNickname, EEikCtEdwin, NULL);
			iNickName = static_cast<CEikEdwin*>(controlNickname);

			iNickName->ConstructL( EEikEdwinNoHorizScrolling |EEikEdwinNoAutoSelection
				|EEikEdwinResizable, 10, 50, 1 );

			iNickName->CreateTextViewL();

			// Comment date and time
			CCoeControl* controlDate = CreateLineByTypeL(KNullDesC, EPrivateQuestionDate, EEikCtEdwin, NULL);
			iDateTime = static_cast<CEikEdwin*>(controlDate);

			iDateTime->ConstructL( EEikEdwinNoHorizScrolling |EEikEdwinNoAutoSelection
				|EEikEdwinResizable, 10, 50, 1 );
			
			iDateTime->CreateTextViewL();

			// Comment text
			CCoeControl* controlText = CreateLineByTypeL(KNullDesC, EPrivateQuestionText, EEikCtEdwin, NULL);
			iText = static_cast<CEikEdwin*>(controlText);

			iText->ConstructL( EEikEdwinNoHorizScrolling |EEikEdwinNoAutoSelection
				|EEikEdwinResizable, 10, 256, 1 );

			iText->CreateTextViewL();

			// Set data from service provider
			CAafUserQuestionsProvider* questionProvider = CAafUserQuestionsProvider::GetInstanceL();

			TInt currentQuestion = appUi->GetUserQuestionsView()->CurrentItemIndex();

			TInt currentComment = appUi->GetQuestionCommentsView()->CurrentItemIndex();

			ns1__AskAFriendComment* commentData = NULL;
			commentData = questionProvider->GetComment(currentQuestion, currentComment);

			// If comment data has been retrieved successfully
			if (commentData)
			{
				// Nickname
				HBufC* commentDescriptor = CAafUtils::StringToDescriptorLC(commentData->NickName);

				iNickName->SetTextL(commentDescriptor);

				CleanupStack::PopAndDestroy(commentDescriptor);

				// Comment date and time
				// Form question time string
				TTime commentTime;

				// Convert string to TTime structure
				HBufC8* dateDescriptor = CAafUtils::StringToDescriptor8LC(commentData->DateTimePosted);
				TInt64 dateAndTime;
				TLex8 dateTime(dateDescriptor->Des());
				TInt err = dateTime.Val(dateAndTime);

				commentTime = (dateAndTime/TInt64(10));

				// Format date and time string
				TBuf<30> dateString;
				commentTime.FormatL(dateString, KDateString);

				iDateTime->SetTextL(&dateString);

				CleanupStack::PopAndDestroy(dateDescriptor);

				// Comment text
				commentDescriptor = CAafUtils::StringToDescriptorLC(commentData->Text);

				iText->SetTextL(commentDescriptor);

				CleanupStack::PopAndDestroy(commentDescriptor);
			}
		}
		break;
	case EPrivateQuestion:
	default:
		{
			// Create necessary controls
			// Nickname
			CCoeControl* controlNickname = CreateLineByTypeL(KNullDesC, EPrivateQuestionNickname, EEikCtEdwin, NULL);
			iNickName = static_cast<CEikEdwin*>(controlNickname);

			iNickName->ConstructL( EEikEdwinNoHorizScrolling |EEikEdwinNoAutoSelection
				|EEikEdwinResizable, 10, 50, 1 );

			iNickName->CreateTextViewL();

			// Question date and time
			CCoeControl* controlDate = CreateLineByTypeL(KNullDesC, EPrivateQuestionDate, EEikCtEdwin, NULL);
			iDateTime = static_cast<CEikEdwin*>(controlDate);

			iDateTime->ConstructL( EEikEdwinNoHorizScrolling |EEikEdwinNoAutoSelection
				|EEikEdwinResizable, 10, 50, 1 );

			iDateTime->CreateTextViewL();

			// Question text
			CCoeControl* controlText = CreateLineByTypeL(KNullDesC, EPrivateQuestionText, EEikCtEdwin, NULL);
			iText = static_cast<CEikEdwin*>(controlText);

			iText->ConstructL( EEikEdwinNoHorizScrolling |EEikEdwinNoAutoSelection
				|EEikEdwinResizable, 10, 256, 1 );

			iText->CreateTextViewL();

			// Set data from service provider
			CAafPrivateQuestionsProvider* questionProvider = CAafPrivateQuestionsProvider::GetInstanceL();

			TInt currentQuestion = appUi->GetPrivateQuestionsView()->CurrentItemIndex();

			ns1__PrivateAAFQuestion* questionData = NULL;
			questionData = questionProvider->GetQuestion(currentQuestion);

			// If question data has been retrieved successfully
			if (questionData)
			{
				// Nickname
				HBufC* questionDescriptor = CAafUtils::StringToDescriptorLC(questionData->NickName);

				iNickName->SetTextL(questionDescriptor);

				CleanupStack::PopAndDestroy(questionDescriptor);

				// Comment date and time
				// Form question time string
				TTime questionTime;

				// Convert string to TTime structure
				HBufC8* dateDescriptor = CAafUtils::StringToDescriptor8LC(questionData->DateTimePosted);
				TInt64 dateAndTime;
				TLex8 dateTime(dateDescriptor->Des());
				TInt err = dateTime.Val(dateAndTime);

				questionTime = (dateAndTime/TInt64(10));

				// Format date and time string
				TBuf<30> dateString;
				questionTime.FormatL(dateString, KDateString);

				iDateTime->SetTextL(&dateString);

				CleanupStack::PopAndDestroy(dateDescriptor);

				// Comment text
				questionDescriptor = CAafUtils::StringToDescriptorLC(questionData->Question);

				iText->SetTextL(questionDescriptor);

				CleanupStack::PopAndDestroy(questionDescriptor);
			}
		}
		break;
	}	
}

void CAafAppDataViewDialog::PostLayoutDynInitL()
{
	CEikDialog::PostLayoutDynInitL();

	/*
	switch(iCurrentData)
	{
	case EUserQuestion:
		{

		}
		break;
	case EQuestionComment:
		{

		}
		break;
	case EPrivateQuestion:
	default:
		{
		}
		break;
	}
	*/
}

// end of file