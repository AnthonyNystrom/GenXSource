#include "N2FinboxManager.h"
#include "ApplicationManager.h"
#include "AskService.h"
#include "AskQuestion.h"
#include "N2FNewsManager.h"

#include "StringWrapper.h"

#include "N2FMessage.h"
#include "stringres.h"

N2FInboxManager::N2FInboxManager( ApplicationManager *appManager )
: N2FMessageOwner(EOT_DRAFT_MANAGER, appManager)
{

}

N2FInboxManager::~N2FInboxManager()
{
	while (newMessages.Size())
	{
		((N2FMessage *)*newMessages.Begin())->SetOwner(pManager);
		newMessages.Erase(newMessages.Begin());
	}
	SAFE_DELETE(IDs);
}

void N2FInboxManager::OnMessageChanged( N2FMessage *pMsg )
{

}

void N2FInboxManager::OnMessageWillSwitchOwner( N2FMessage *pMsg )
{

}

void N2FInboxManager::OnMessageAdded( N2FMessage *pMsg )
{

}

void N2FInboxManager::ReciveAll()
{
	if (isWorking || !pManager->IsUserLoggedIn())
	{
		return;
	}
	isReciving = true;

}

void N2FInboxManager::OnSuccess( int32 packetId )
{
	switch(packetId)
	{
	case AskService::EPS_GETQUESTIONIDS:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::OnSuccess EPS_GETQUESTIONIDS");
			IDs = pManager->GetAskService()->OnGetQuestionIDs();
			if (IDs->length)
			{
				msgCounter = 0;
				GetNextQuestion();
			}
			else
			{
				SAFE_DELETE(IDs);
				while (GetFirstMessage())
				{
					GetFirstMessage()->SetOwner(pManager);
				}
				pManager->ShowInbox(false);
				isWorking = false;
			}
			UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::OnSuccess done");

		}
		break;
	case AskService::EPS_GETQUESTION:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::OnSuccess EPS_GETQUESTION");
			AskQuestion *question = pManager->GetAskService()->OnGetQuestion();
			
			N2FMessage *msg = pManager->GetFreeMessage();
			msg->InitAsQuestionName();
			msg->SetText(question->Question, ETT_QUESTION_TEXT);
			msg->SetDateTime(question->DTCreated);
			msg->SetID(question->ID);
			msg->SetOwner(NULL);
			newMessages.PushBack(msg);

			SAFE_DELETE(question);
			UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::OnSuccess done");

			GetNextQuestion();

		}
		break;
	}
}

void N2FInboxManager::OnFailed( int32 packetId, char16 *errorString )
{
	UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::OnFailed");
	pManager->ShowInbox(false);
	isWorking = false;
	while (newMessages.Size())
	{
		((N2FMessage *)*newMessages.Begin())->SetOwner(pManager);
		newMessages.Erase(newMessages.Begin());
	}

	SAFE_DELETE(IDs);


}

void N2FInboxManager::GetNextQuestion()
{
	if (!IDs)
	{
		return;
	}
	if (msgCounter >= IDs->length)
	{
		UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::GetNextQuestion merging");
		//MergeLists(&newMessages);
		while (messages->Size())
		{
			((N2FMessage *)*messages->Begin())->SetOwner(pManager);
		}
		while (newMessages.Size())
		{
			((N2FMessage *)*newMessages.Begin())->SetOwner(this);
			newMessages.Erase(newMessages.Begin());
		}

		SAFE_DELETE(IDs);
		pManager->ShowInbox(false);
		isWorking = false;
		UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::GetNextQuestion done");
		return;
	}
	UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::GetNextQuestion ");

	int32 i = IDs->pBuffer[msgCounter];
	pManager->GetAskService()->GetQuestion(this
		, pManager->GetCore()->GetApplicationData()->login
		, pManager->GetCore()->GetApplicationData()->password
		, i);
	msgCounter++;

}

void N2FInboxManager::Init()
{
	Read("inbox.dat", pManager->GetStringWrapper()->GetStringText(IDS_INBOX_INVALID_LOAD));
}

void N2FInboxManager::OwnerUpdate()
{
	if (isMessageCountChanged || isMessagesChanged)
	{
		Write("inbox.dat", pManager->GetStringWrapper()->GetStringText(IDS_INBOX_INVALID_SAVE));
	}
	if (isReciving && !pManager->GetNewsManager()->IsWorking())
	{
		UTILS_LOG(EDMP_DEBUG, "N2FInboxManager::OwnerUpdate isReciving");
		isWorking = true;
		pManager->ShowInbox(true);
		pManager->GetAskService()->GetQuestionIDs(this
			, pManager->GetCore()->GetApplicationData()->login
			, pManager->GetCore()->GetApplicationData()->password);
		isReciving = false;
	}
	N2FMessageOwner::OwnerUpdate();
}










