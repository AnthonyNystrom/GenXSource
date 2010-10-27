#include "ScreenInbox.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUIItemsList.h"

#include "ScreenAsk.h"
#include "VList.h"

#include "N2FData.h"
#include "N2FInboxManager.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

ScreenInbox::ScreenInbox( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	AddListener(EEI_SOFT1_PRESSED, this);
	pManager->GetInboxManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);

	SetStretch(ECS_STRETCH);
	list = new GUIItemsList(this, this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);

}

ScreenInbox::~ScreenInbox( void )
{
}


const char16	* ScreenInbox::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	if (pManager->GetInboxManager()->MessagesCount())
	{
		switch(index)
		{
		case EPI_BACK:
			{
				id = EPI_BACK;
				return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
			}
		case EPI_READ:
			{
				id = EPI_READ;
				return pManager->GetStringWrapper()->GetStringText(IDS_READ);
			}
		case EPI_GET_AND_SEND:
			{
				id = EPI_GET_AND_SEND;
				return pManager->GetStringWrapper()->GetStringText(IDS_GET_AND_SEND);
			}
		}
	}
	else if (index == 0)
	{
		switch(index)
		{
		case 1:
			{
				id = EPI_BACK;
				return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
			}
		case 0:
			{
				id = EPI_GET_AND_SEND;
				return pManager->GetStringWrapper()->GetStringText(IDS_GET_AND_SEND);
			}
		}
	}
	return NULL;
}

void ScreenInbox::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(id)
	{
	case EPI_BACK:
		{
			BackToPrevSrceen();
		}
		break;
	case EPI_READ:
		{
			ItemsListOnItem(list, list->GetFocusedOwner());
		}
		break;
	case EPI_GET_AND_SEND:
		{
			pManager->GetInboxManager()->ReciveAll();
		}
		break;
	}
}


void ScreenInbox::ItemsListOnItem( GUIItemsList *forList, void *owner )
{
	pManager->SetWorkingMesage((N2FMessage*)owner);
	pManager->ChangeWindow(ESN_RESPONSE, false);
}

bool ScreenInbox::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_SOFT1_PRESSED:
		{
			N2FMessage* msg = ((N2FMessage*)(list->GetFocusedOwner()));
			if (msg)
			{
				msg->SetOwner(pManager);
			}
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			list->RebuildList();
		}
		break;
	case EEI_WINDOW_SET_MAIN:
	case EEI_WINDOW_DID_ACTIVATE:
		{
			list->ActivateFocus();
		}
		break;
	case EEI_WINDOW_LOST_MAIN:
	case EEI_WINDOW_WILL_DEACTIVATE:
		{
			list->DeactivateFocus();
		}
		break;
	case EEI_N2FMESSAGE_COUNT_CHANGED:
		{
			if (GUISystem::Instance()->GetActiveControl() != this)
			{
				return false;
			}
			list->RebuildList();
			return true;
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}




GUIControl* ScreenInbox::ItemsListCreateListItem(GUIItemsList *forList)
{
	GUIMenuItem *pItem = new GUIMenuItem(EDT_SELECTED_ITEM);
	GUIImage *img = new GUIImage(IDB_INBOX_ICONS, pItem);
	int32 w = img->GetSprite()->GetWidth() + (img->GetSprite()->GetWidth() >> 2);
	img->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));
	img->SetFrame(0);
	img->SetID(ECIDS_TEXT_ID + 1);
	GUIText *text = new GUIText((char16*)L" ", pItem);
	text->SetID(ECIDS_TEXT_ID);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	text->SetDrawType(EDT_UNSELECTED_TEXT);
	return pItem;
}

bool ScreenInbox::ItemsListIsOwnerValid( GUIItemsList *forList, void *owner )
{
	return ((N2FMessage*)owner)->GetOwner() == pManager->GetInboxManager();
}

void *ScreenInbox::ItemsListGetFirstOwner(GUIItemsList *forList)
{
	inboxIterator = pManager->GetInboxManager()->GetMessagesList()->Begin();
	if (inboxIterator != pManager->GetInboxManager()->GetMessagesList()->End())
	{
		return (*inboxIterator);
	}
	else
	{
		return NULL;
	}
}

void *ScreenInbox::ItemsListGetNextOwner(GUIItemsList *forList)
{
	inboxIterator++;
	if (inboxIterator != pManager->GetInboxManager()->GetMessagesList()->End())
	{
		return (*inboxIterator);
	}
	else
	{
		return NULL;
	}
}

void ScreenInbox::ItemsListTuneItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	GUIText *t = (GUIText*)(item->GetByID(ECIDS_TEXT_ID));
	t->SetText(((N2FMessage*)owner)->GetText(ETT_QUESTION_TEXT));//todo: check for question type
	//GUIImage *img = (GUIImage*)(item->GetByID(ECIDS_TEXT_ID + 1));
	//img->SetFrame(((N2FMessage*)owner)->GetType() - 1);

}

void ScreenInbox::ItemsListCorrectItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	ItemsListTuneItemByOwner(forList, item, owner);
}

