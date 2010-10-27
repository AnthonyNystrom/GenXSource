#include "ScreenOutbox.h"
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
#include "N2FOutboxManager.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

ScreenOutbox::ScreenOutbox( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	pManager->GetOutboxManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);

	SetStretch(ECS_STRETCH);
	list = new GUIItemsList(this, this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);

}

ScreenOutbox::~ScreenOutbox( void )
{
}


const char16	* ScreenOutbox::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	if (pManager->GetOutboxManager()->MessagesCount())
	{
		switch(index)
		{
		case EPI_BACK:
			{
				id = EPI_BACK;
				return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
			}
		case EPI_DELETE:
			{
				id = EPI_DELETE;
				return pManager->GetStringWrapper()->GetStringText(IDS_DELETE);
			}
		case EPI_SEND:
			{
				id = EPI_SEND;
				return pManager->GetStringWrapper()->GetStringText(IDS_SEND);
			}
		}
	}
	else if (index == 0)
	{
		id = EPI_BACK;
		return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
	}
	return NULL;
}

void ScreenOutbox::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(id)
	{
	case EPI_BACK:
		{
			BackToPrevSrceen();
		}
		break;
	case EPI_DELETE:
		{
			((N2FMessage*)(list->GetFocusedOwner()))->SetOwner(pManager);
		}
		break;
	case EPI_SEND:
		{
			VList *l = list->GetListChilds();
			for (VList::Iterator it = l->Begin(); it != l->End(); it++)
			{
				GUIControl *item = (GUIControl *)(*it);
				N2FMessage *owner = (N2FMessage *)list->GetOwnerForItem(item);
				GUIImage *img = (GUIImage*)(item->GetByID(ECIDS_TEXT_ID + 2));
				if (img->GetFrame())
				{
					owner->SetSend(true);
				}
				else
				{
					owner->SetSend(false);
				}
			}
		}
		break;
	}
}


void ScreenOutbox::ItemsListOnItem( GUIItemsList *forList, void *owner )
{
	GUIControl *item = forList->GetItemForOwner(owner);
	GUIImage *img = (GUIImage*)(item->GetByID(ECIDS_TEXT_ID + 2));
	if (img->GetFrame())
	{
		img->SetFrame(0);
	}
	else
	{
		img->SetFrame(1);
	}

}

bool ScreenOutbox::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
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




GUIControl* ScreenOutbox::ItemsListCreateListItem(GUIItemsList *forList)
{
	GUIMenuItem *pItem = new GUIMenuItem(EDT_SELECTED_ITEM);
	GUIImage *img = new GUIImage(IDB_CHECK_BOX, pItem);
	int32 w = img->GetSprite()->GetWidth();
	img->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));
	img->SetID(ECIDS_TEXT_ID + 2);

	img = new GUIImage(IDB_INBOX_ICONS, pItem);
	w = img->GetSprite()->GetWidth() + (img->GetSprite()->GetWidth() >> 2);
	img->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));
	img->SetFrame(1);
	img->SetID(ECIDS_TEXT_ID + 1);
	GUIText *text = new GUIText((char16*)L" ", pItem);
	text->SetID(ECIDS_TEXT_ID);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	text->SetDrawType(EDT_UNSELECTED_TEXT);
	return pItem;
}

bool ScreenOutbox::ItemsListIsOwnerValid( GUIItemsList *forList, void *owner )
{
	return ((N2FMessage*)owner)->GetOwner() == pManager->GetOutboxManager();
}

void *ScreenOutbox::ItemsListGetFirstOwner(GUIItemsList *forList)
{
	outboxIterator = pManager->GetOutboxManager()->GetMessagesList()->Begin();
	if (outboxIterator != pManager->GetOutboxManager()->GetMessagesList()->End())
	{
		return (*outboxIterator);
	}
	else
	{
		return NULL;
	}
}

void *ScreenOutbox::ItemsListGetNextOwner(GUIItemsList *forList)
{
	outboxIterator++;
	if (outboxIterator != pManager->GetOutboxManager()->GetMessagesList()->End())
	{
		return (*outboxIterator);
	}
	else
	{
		return NULL;
	}
}

void ScreenOutbox::ItemsListTuneItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	GUIText *t = (GUIText*)(item->GetByID(ECIDS_TEXT_ID));
	if (((N2FMessage*)owner)->GetType() == EMT_QUESTION)
	{
		t->SetText(((N2FMessage*)owner)->GetText(ETT_QUESTION_TEXT));
	}
	else if (((N2FMessage*)owner)->GetType() == EMT_PHOTO)
	{
		t->SetText(((N2FMessage*)owner)->GetText(ETT_PHOTO_TITLE));
	}
	else if (((N2FMessage*)owner)->GetType() == EMT_COMMENT)
	{
		t->SetText(((N2FMessage*)owner)->GetText(ETT_COMMENT_TEXT));
	}

	GUIImage *img = (GUIImage*)(item->GetByID(ECIDS_TEXT_ID + 1));
	img->SetFrame(((N2FMessage*)owner)->GetType() - 1);

	img = (GUIImage*)(item->GetByID(ECIDS_TEXT_ID + 2));
	if (((N2FMessage*)owner)->NeedToSend())
	{
		img->SetFrame(1);
	}
	else
	{
		img->SetFrame(0);
	}

}

void ScreenOutbox::ItemsListCorrectItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	ItemsListTuneItemByOwner(forList, item, owner);
}


