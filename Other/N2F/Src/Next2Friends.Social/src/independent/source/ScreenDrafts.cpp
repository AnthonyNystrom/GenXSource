#include "ScreenDrafts.h"
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
#include "N2FDraftsManager.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

ScreenDrafts::ScreenDrafts( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	pManager->GetDraftsManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);

	SetStretch(ECS_STRETCH);
	list = new GUIItemsList(this, this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);

}

ScreenDrafts::~ScreenDrafts( void )
{
}


const char16	* ScreenDrafts::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	if (pManager->GetDraftsManager()->MessagesCount())
	{
		switch(index)
		{
		case EPI_EDIT:
			{
				id = EPI_EDIT;
				return pManager->GetStringWrapper()->GetStringText(IDS_EDIT);
			}
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
		}
	}
	else if (index == 0)
	{
		id = EPI_BACK;
		return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
	}
	return NULL;
}

void ScreenDrafts::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(id)
	{
	case EPI_BACK:
		{
			BackToPrevSrceen();
		}
		break;
	case EPI_EDIT:
		{
			ItemsListOnItem(list, list->GetFocusedOwner());
		}
		break;
	case EPI_DELETE:
		{
			((N2FMessage*)(list->GetFocusedOwner()))->SetOwner(pManager);
		}
		break;
	}
}


void ScreenDrafts::ItemsListOnItem( GUIItemsList *forList, void *owner )
{
	pManager->SetWorkingMesage((N2FMessage*)owner);
	pManager->ChangeWindow(ESN_ASK, false);
}

bool ScreenDrafts::OnEvent( eEventID eventID, EventData *pData )
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




GUIControl* ScreenDrafts::ItemsListCreateListItem(GUIItemsList *forList)
{
	GUIMenuItem *pItem = new GUIMenuItem(EDT_SELECTED_ITEM);
	GUIImage *img = new GUIImage(IDB_INBOX_ICONS, pItem);
	int32 w = img->GetSprite()->GetWidth() + (img->GetSprite()->GetWidth() >> 2);
	img->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));
	img->SetFrame(0);
	GUIText *text = new GUIText((char16*)L" ", pItem);
	text->SetID(ECIDS_TEXT_ID);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	text->SetDrawType(EDT_UNSELECTED_TEXT);
	return pItem;
}

bool ScreenDrafts::ItemsListIsOwnerValid( GUIItemsList *forList, void *owner )
{
	return ((N2FMessage*)owner)->GetOwner() == pManager->GetDraftsManager();
}

void *ScreenDrafts::ItemsListGetFirstOwner(GUIItemsList *forList)
{
	draftsIterator = pManager->GetDraftsManager()->GetMessagesList()->Begin();
	if (draftsIterator != pManager->GetDraftsManager()->GetMessagesList()->End())
	{
		return (*draftsIterator);
	}
	else
	{
		return NULL;
	}
}

void *ScreenDrafts::ItemsListGetNextOwner(GUIItemsList *forList)
{
	draftsIterator++;
	if (draftsIterator != pManager->GetDraftsManager()->GetMessagesList()->End())
	{
		return (*draftsIterator);
	}
	else
	{
		return NULL;
	}
}

void ScreenDrafts::ItemsListTuneItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	GUIText *t = (GUIText*)(item->GetByID(ECIDS_TEXT_ID));
	t->SetText(((N2FMessage*)owner)->GetText(ETT_QUESTION_TEXT));
}

void ScreenDrafts::ItemsListCorrectItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	ItemsListTuneItemByOwner(forList, item, owner);
}


