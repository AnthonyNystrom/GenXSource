#include "ScreenMainMenu.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUIAlert.h"
#include "GUILockWindow.h"
#include "ScreenOutbox.h"
#include "GUIMultiString.h"

#include "N2FDraftsManager.h"
#include "N2FOutboxManager.h"
#include "N2FNewsManager.h"
#include "N2FInboxManager.h"

#include "SkinProperties.h"


ScreenMainMenu::ScreenMainMenu(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID)
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	pManager->GetDraftsManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);
	pManager->GetOutboxManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);
	pManager->GetNewsManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);
	pManager->GetInboxManager()->AddListener(EEI_N2FMESSAGE_COUNT_CHANGED, this);
	SetStretch(ECS_STRETCH);
	GUIListView *list = new GUIListView(this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);
	for (int i = 0; i < EMI_COUNT; i++)
	{
		GUIMenuItem *mi = new GUIMenuItem(EDT_SELECTED_ITEM, list);
		mi->AddListener(EEI_BUTTON_PRESSED, this);
		mi->SetID(ECIDS_MENU_ITEM_ID + i);
		GUIImage *img = new GUIImage(IDB_MENU_ICONS, mi);
		int32 w = img->GetSprite()->GetWidth() * 2;
		img->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));
		img->SetFrame(i);
		GUIText *text = new GUIText(IDS_MENU_GO + i, mi);
		text->SetID(ECIDS_TEXT_ID);
		text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
		text->SetDrawType(EDT_UNSELECTED_TEXT);
		if (i == 0)
		{
			pLastFocus = mi;
		}
	}

}

ScreenMainMenu::~ScreenMainMenu(void)
{
}


const char16	* ScreenMainMenu::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	switch(index)
	{
	case EPI_GO_TO_NEXT2FRIENDS:
		{
			return pManager->GetStringWrapper()->GetStringText(IDS_GO_TO_NEXT_2_FRIENDS);
		}
		break;
	case EPI_GET_AND_SEND:
		{
			return pManager->GetStringWrapper()->GetStringText(IDS_GET_AND_SEND);
		}
		break;
	case EPI_SELECT:
		{
			if (pLastFocus->GetID() == ECIDS_MENU_ITEM_ID + 0 || pLastFocus->GetID() == ECIDS_MENU_ITEM_ID + 5)
			{
				return pManager->GetStringWrapper()->GetStringText(IDS_SELECT);
			}
			else
			{
				return pManager->GetStringWrapper()->GetStringText(IDS_OPEN);
			}
		}
		break;
	}
	return NULL;
}

void ScreenMainMenu::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(index)
	{
	case EPI_GO_TO_NEXT2FRIENDS:
		{
			Utils::OpenBrowser("http://www.next2friends.com");
		}
		break;
	case EPI_GET_AND_SEND:
		{
			pManager->GetOutboxManager()->SendAll();
			pManager->GetNewsManager()->ReciveAll();
			pManager->GetInboxManager()->ReciveAll();
		}
		break;
	case EPI_SELECT:
		{
			OnItem(pLastFocus->GetID() - ECIDS_MENU_ITEM_ID);
		}
		break;
	}
}

void ScreenMainMenu::OnItem( int32 index )
{
	switch(index)
	{
	case EMI_GO:
		{
			pManager->ChangeWindow(ESN_GO, false);
		}
		break;
	case EMI_SETTINGS:
		{
			pManager->ChangeWindow(ESN_CREDENTIALS, false);
		}
		break;
	case EMI_DRAFTS:
		{
			pManager->ChangeWindow(ESN_DRAFTS, false);
		}
		break;
	case EMI_OUTBOX:
		{
			pManager->ChangeWindow(ESN_OUTBOX, false);
		}
		break;
	case EMI_INBOX:
		{
			pManager->ChangeWindow(ESN_INBOX, false);
		}
		break;
	case EMI_DASHBOARD:
		{
			pManager->ChangeWindow(ESN_DASHBOARD, false);
		}
		break;
	}
}

bool ScreenMainMenu::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_N2FMESSAGE_COUNT_CHANGED:
		{
			RecalcItems();
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			RecalcItems();
		}
		break;
	case EEI_WINDOW_DID_ACTIVATE:
		{
			if (pManager->NeedToShowErrorAlert(false))
			{
				pManager->GetAlert()->GetText()->SetText(IDS_CONNECTION_PROBLEMS);
				pManager->GetAlert()->Show();
			}
		}
		break;
	case EEI_BUTTON_PRESSED:
		{
			OnItem(((GUIControl*)(pData->lParam))->GetID() - ECIDS_MENU_ITEM_ID);
			return true;
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}

void ScreenMainMenu::RecalcItems()
{
	GUIMenuItem *mi = (GUIMenuItem*)GetByID(ECIDS_MENU_ITEM_ID + EMI_DRAFTS);
	GUIText *text = (GUIText*)mi->GetByID(ECIDS_TEXT_ID);
	if (pManager->GetDraftsManager()->MessagesCount())
	{
		char16 tText[100];
		Utils::WSPrintf(tText, 100, pManager->GetStringWrapper()->GetStringText(IDS_MAIN_MENU_FORMAT)
			, pManager->GetStringWrapper()->GetStringText(IDS_MENU_DRAFTS), pManager->GetDraftsManager()->MessagesCount());
		text->SetText(tText);
	}
	else
	{
		text->SetText(IDS_MENU_DRAFTS);
	}

	mi = (GUIMenuItem*)GetByID(ECIDS_MENU_ITEM_ID + EMI_OUTBOX);
	text = (GUIText*)mi->GetByID(ECIDS_TEXT_ID);
	if (pManager->GetOutboxManager()->MessagesCount())
	{
		char16 tText[100];
		Utils::WSPrintf(tText, 100, pManager->GetStringWrapper()->GetStringText(IDS_MAIN_MENU_FORMAT)
			, pManager->GetStringWrapper()->GetStringText(IDS_MENU_OUTBOX), pManager->GetOutboxManager()->MessagesCount());
		text->SetText(tText);
	}
	else
	{
		text->SetText(IDS_MENU_OUTBOX);
	}

	mi = (GUIMenuItem*)GetByID(ECIDS_MENU_ITEM_ID + EMI_DASHBOARD);
	text = (GUIText*)mi->GetByID(ECIDS_TEXT_ID);
	if (pManager->GetNewsManager()->MessagesCount())
	{
		char16 tText[100];
		Utils::WSPrintf(tText, 100, pManager->GetStringWrapper()->GetStringText(IDS_MAIN_MENU_FORMAT)
			, pManager->GetStringWrapper()->GetStringText(IDS_MENU_DASHBOARD), pManager->GetNewsManager()->MessagesCount());
		text->SetText(tText);
	}
	else
	{
		text->SetText(IDS_MENU_DASHBOARD);
	}

	mi = (GUIMenuItem*)GetByID(ECIDS_MENU_ITEM_ID + EMI_INBOX);
	text = (GUIText*)mi->GetByID(ECIDS_TEXT_ID);
	if (pManager->GetInboxManager()->MessagesCount())
	{
		char16 tText[100];
		Utils::WSPrintf(tText, 100, pManager->GetStringWrapper()->GetStringText(IDS_MAIN_MENU_FORMAT)
			, pManager->GetStringWrapper()->GetStringText(IDS_MENU_INBOX), pManager->GetInboxManager()->MessagesCount());
		text->SetText(tText);
	}
	else
	{
		text->SetText(IDS_MENU_INBOX);
	}

}


