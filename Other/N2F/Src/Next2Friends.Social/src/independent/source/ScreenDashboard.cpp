#include "ScreenDashboard.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUIItemsList.h"
#include "GUIMultiString.h"

#include "ScreenAsk.h"
#include "VList.h"

#include "N2FData.h"
#include "N2FNewsManager.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

ScreenDashboard::ScreenDashboard( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	pManager->GetNewsManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);

	SetStretch(ECS_STRETCH);
	list = new GUIItemsList(this, this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_PHOTO_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() / 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);

}

ScreenDashboard::~ScreenDashboard( void )
{
}


const char16	* ScreenDashboard::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	if (pManager->GetNewsManager()->MessagesCount())
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

void ScreenDashboard::PopUpOnItemSelected( int32 index, int32 id )
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
			pManager->GetNewsManager()->ReciveAll();
		}
		break;
	}
}


void ScreenDashboard::ItemsListOnItem( GUIItemsList *forList, void *owner )
{
	if (!owner)
	{
		return;
	}
	pManager->SetWorkingMesage((N2FMessage*)owner);
	pManager->ChangeWindow(ESN_DASHBOARD_READ, false);

}

bool ScreenDashboard::OnEvent( eEventID eventID, EventData *pData )
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




GUIControl* ScreenDashboard::ItemsListCreateListItem(GUIItemsList *forList)
{
	GUIMenuItem *pItem = new GUIMenuItem(EDT_PHOTO_ITEM);
	GUIImage *img = new GUIImage(IDB_DASHBOARD_ICONS, pItem);
	int32 w = img->GetSprite()->GetWidth() + (img->GetSprite()->GetWidth() >> 2);
	img->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));
	img->SetFrame(1);
	img->SetID(ECIDS_TEXT_ID + 1);

	GUILayoutBox *lay = new GUILayoutBox(true, pItem);
	GUIMultiString *text = new GUIMultiString((char16*)L" ", lay);
	text->SetID(ECIDS_TEXT_ID);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 3, 0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 3);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	text->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);

	uint8 plt;
	int32 h = GUISystem::Instance()->GetSkin()->GetFont(EDT_POPUP_UNSELECTED_TEXT, plt)->GetHeight();
	GUIText *tx = new GUIText((char16*)L"", lay, ControlRect(0, 0, ControlRect::MIN_D, h, ControlRect::MAX_D, h));
	tx->SetID(ECIDS_TEXT_ID + 2);
	tx->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 2);
	tx->SetTextAlign(Font::EAP_RIGHT | Font::EAP_VCENTER);
	tx->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
	return pItem;
}

bool ScreenDashboard::ItemsListIsOwnerValid( GUIItemsList *forList, void *owner )
{
	return ((N2FMessage*)owner)->GetOwner() == pManager->GetNewsManager();
}

void *ScreenDashboard::ItemsListGetFirstOwner(GUIItemsList *forList)
{
	newsIterator = pManager->GetNewsManager()->GetMessagesList()->Begin();
	if (newsIterator != pManager->GetNewsManager()->GetMessagesList()->End())
	{
		return (*newsIterator);
	}
	else
	{
		return NULL;
	}
}

void *ScreenDashboard::ItemsListGetNextOwner(GUIItemsList *forList)
{
	newsIterator++;
	if (newsIterator != pManager->GetNewsManager()->GetMessagesList()->End())
	{
		return (*newsIterator);
	}
	else
	{
		return NULL;
	}
}

void ScreenDashboard::ItemsListTuneItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	GUIMultiString *t = (GUIMultiString*)(item->GetByID(ECIDS_TEXT_ID));
	//t->SetText(((N2FMessage*)owner)->GetText(ETT_DASHBOARD_NICKNAME1));
	switch(((N2FMessage*)owner)->GetInnerType())
	{
	case EIT_DASHBOARD_FIREND:
		{
			char16 temp[1000];
			Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_FRIEND), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_NICKNAME2), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_NICKNAME1));
			t->SetText(temp);
		}
		break;
	case EIT_DASHBOARD_WALL:
		{
			char16 temp[1000];
			Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_WALL), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_NICKNAME1), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_NICKNAME2));
			t->SetText(temp);
		}
		break;
	case EIT_DASHBOARD_PHOTO:
		{
			char16 temp[1000];
			Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_PHOTO), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_NICKNAME1), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_TITLE));
			t->SetText(temp);
		}
		break;
	case EIT_DASHBOARD_VIDEO:
		{
			char16 temp[1000];
			Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_VIDEO), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_NICKNAME1), ((N2FMessage*)owner)->GetText(ETT_DASHBOARD_TITLE));
			t->SetText(temp);
		}
		break;
	}
	GUIImage *img = (GUIImage*)(item->GetByID(ECIDS_TEXT_ID + 1));
	img->SetFrame(((N2FMessage*)owner)->GetInnerType() - EIT_DASHBOARD_FIRST);

	GUIText *tx = (GUIText*)(item->GetByID(ECIDS_TEXT_ID + 2));
	tx->SetText(((N2FMessage*)owner)->GetDateTime());


}

void ScreenDashboard::ItemsListCorrectItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner )
{
	ItemsListTuneItemByOwner(forList, item, owner);
}


