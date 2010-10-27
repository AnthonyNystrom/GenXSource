#include "N2FMessageOwner.h"
#include "N2FMessage.h"

#include "ApplicationManager.h"
#include "GUIAlert.h"
#include "GUIMultiString.h"


N2FMessageOwner::N2FMessageOwner( eOwnerType ownerType, ApplicationManager *manager )
{
	pManager = manager;
	type = ownerType;
	messages = new VList();
}

N2FMessageOwner::~N2FMessageOwner()
{
	while (messages->Size())
	{
		N2FMessage *msg= (N2FMessage*)(*(messages->Begin()));
		messages->Erase(messages->Begin());
		SAFE_DELETE(msg);
	}
	SAFE_DELETE(messages);
}

void N2FMessageOwner::MessgeChanged( N2FMessage *pMsg )
{
	isMessagesChanged = true;
	OnMessageChanged(pMsg);
}

void N2FMessageOwner::MessgeWillSwitchOwner( N2FMessage *pMsg )
{
	isMessageCountChanged = true;
	OnMessageWillSwitchOwner(pMsg);
	messages->Erase(messages->PosItem(pMsg));
}

void N2FMessageOwner::AddMessage( N2FMessage *pMsg )
{
	isMessageCountChanged = true;
	messages->PushBack(pMsg);
	OnMessageAdded(pMsg);
}

void N2FMessageOwner::OwnerUpdate()
{
	if (isMessagesChanged)
	{
		DispatchEvent(EEI_N2FMESSAGE_CHANGED, NULL);
		isMessagesChanged = false;
	}
	if (isMessageCountChanged)
	{
		DispatchEvent(EEI_N2FMESSAGE_COUNT_CHANGED, NULL);
		isMessageCountChanged = false;
	}
}

int32 N2FMessageOwner::MessagesCount()
{
	return messages->Size();
}

N2FMessage * N2FMessageOwner::GetFirstMessage()
{
	if (messages->Begin() != messages->End())
	{
		return (N2FMessage *)(*(messages->Begin()));
	}
	return NULL;
}

bool N2FMessageOwner::Write( const char8 *fileName, const char16 *alertMsg )
{
	GetApplication()->GetFileSystem()->Remove(fileName);
	File *fl = GetApplication()->GetFileSystem()->Open(fileName, File::EFM_CREATE);
	if (fl)
	{
		int32 count = MessagesCount();
		fl->Write(&count, 4);
		VList::Iterator it = messages->Begin();
		for (; it != messages->End(); it++)
		{
			N2FMessage *msg = (N2FMessage *)(*it);
			if (!msg->WriteToFile(fl))
			{
				pManager->GetAlert()->GetText()->SetText(alertMsg);
				pManager->GetAlert()->Show();
				fl->Release();
				GetApplication()->GetFileSystem()->Remove(fileName);
				return false;
			}
		}
		fl->Release();
	}
	else
	{
		pManager->GetAlert()->GetText()->SetText(alertMsg);
		pManager->GetAlert()->Show();
		return false;
	}
	return true;
}

bool N2FMessageOwner::Read( const char8 *fileName, const char16 *alertMsg )
{
	File *fl = GetApplication()->GetFileSystem()->Open(fileName, File::EFM_READ);
	if (fl)
	{
		int32 count;
		fl->Read(&count, 4);
		for (int i = 0; i < count; i++)
		{
			N2FMessage *msg = pManager->GetFreeMessage();
			if (!msg->ReadFromFile(fl, pManager))
			{
				pManager->GetAlert()->GetText()->SetText(alertMsg);
				pManager->GetAlert()->Show();
				fl->Release();
				GetApplication()->GetFileSystem()->Remove(fileName);
				return false;
			}
			msg->SetOwner(this);
		}
		fl->Release();
	}
	else
	{
		//pManager->GetAlert()->GetText()->SetText(alertMsg);
		//pManager->GetAlert()->Show();
	}
	isMessageCountChanged = false;
	isMessagesChanged = false;
	return true;
}


void N2FMessageOwner::MergeLists( VList *newMessagesList )
{

//find expired messages and remove them
	{
		VList::Iterator oldIt = messages->Begin();
		while (oldIt != messages->End())
		{
			N2FMessage *oldMsg = (N2FMessage *)(*oldIt);
			bool isFind = false;

			VList::Iterator newIt = newMessagesList->Begin();
			while (newIt != newMessagesList->End())
			{
				N2FMessage *newMsg = (N2FMessage *)(*newIt);

				if (*newMsg == *oldMsg)
				{
					isFind = true;
					break;
				}

				newIt++;
			}

			if (isFind)
			{
				oldIt++;
			}
			else
			{
				oldMsg->SetOwner(pManager);
				oldIt = messages->Begin();
			}
		}
	}
//find new messages and add them
	{
		VList::Iterator newIt = newMessagesList->Begin();
		while (newIt != newMessagesList->End())
		{
			N2FMessage *newMsg = (N2FMessage *)(*newIt);
			bool isFind = false;

			VList::Iterator oldIt = messages->Begin();
			while (oldIt != messages->End())
			{
				N2FMessage *oldMsg = (N2FMessage *)(*oldIt);

				if (*newMsg == *oldMsg)
				{
					isFind = true;
					break;
				}

				oldIt++;
			}

			if (isFind)
			{
				newIt++;
			}
			else
			{
				newMessagesList->Erase(newIt);
				newMsg->SetOwner(this);
				newIt = newMessagesList->Begin();
			}
		}
	}
}














