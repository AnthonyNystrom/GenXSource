#include "ScreenCredentials.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUIEditText.h"
#include "GUIEditPassword.h"
#include "GUIAlert.h"
#include "GUIMultiString.h"

#include "GUILockWindow.h"

#include "SkinProperties.h"

#include "MemberService.h"

#include "N2FNewsManager.h"
#include "N2FInboxManager.h"

ScreenCredentials::ScreenCredentials( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_LOCK_WINDOW_CANCELED, this);

	int32 itemHeight = GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	SetStretch(ECS_STRETCH);
	GUIListView *list = new GUIListView(this);
	list->SetPrescroll(itemHeight * 2);
	list->SetInnerMargins( itemHeight >> 2, itemHeight >> 2
		, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);
	
	GUIText *text = new GUIText(IDS_USERNAME, list, ControlRect(0, 0, ControlRect::MIN_D, newRect.minDy >> 2, ControlRect::MAX_D, newRect.minDy >> 2));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);

	GUIEditText *editText = new GUIEditText(ENP_MAX_LOGIN_SIZE, EETT_ALL, list);
	editText->SetID(ECIDS_CREDENTIALS_ITEM_ID + EMI_USERNAME);
	editText->SetDrawType(EDT_EDITBOX);
	editText->SetFocusedDrawType(EDT_EDITBOX_ACTIVE);
	editText->SetTextMargins(ECP_EDITTEXT_MARGIN_TOP, ECP_EDITTEXT_MARGIN_SIDE, ECP_EDITTEXT_MARGIN_BOTTOM, ECP_EDITTEXT_MARGIN_SIDE);
	pLastFocus = editText;

	text = new GUIText(IDS_PASSWORD, list, ControlRect(0, 0, ControlRect::MIN_D, newRect.minDy >> 2, ControlRect::MAX_D, newRect.minDy >> 2));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);
	editText = new GUIEditPassword(ENP_MAX_PASSWORD_SIZE, list);
	editText->SetID(ECIDS_CREDENTIALS_ITEM_ID + EMI_PASSWORD);
	editText->SetDrawType(EDT_EDITBOX);
	editText->SetFocusedDrawType(EDT_EDITBOX_ACTIVE);
	editText->SetTextMargins(ECP_EDITTEXT_MARGIN_TOP, ECP_EDITTEXT_MARGIN_SIDE, ECP_EDITTEXT_MARGIN_BOTTOM, ECP_EDITTEXT_MARGIN_SIDE);
	isLogin = true;
	packetsSent = 0;
}

ScreenCredentials::~ScreenCredentials( void )
{

}


const char16	* ScreenCredentials::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	switch(index)
	{
	case EPI_BACK:
		{
			return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
		}
		break;
	case EPI_REMIND_PASSWORD:
		{
			return pManager->GetStringWrapper()->GetStringText(IDS_REMIND_PASSWORD);
		}
		break;
	case EPI_SUBMIT:
		{
			return pManager->GetStringWrapper()->GetStringText(IDS_SUBMIT);
		}
		break;
	}
	return NULL;
}

void ScreenCredentials::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(index)
	{
	case EPI_REMIND_PASSWORD:
		{
			pManager->ChangeWindow(ESN_REMIND_PASSWORD, false);
		}
		break;
	case EPI_SUBMIT:
		{
			isCanceled = false;
			pManager->GetLockWindow()->GetText()->SetText(IDS_CONNECTING);
			pManager->GetLockWindow()->Show();
			pManager->GetCore()->GetApplicationData()->isValid = false;
			packetsSent++;
			pManager->GetMemberService()->CheckUserExists(this
				, (char16*)((GUIEditText*)GetByID(ECIDS_CREDENTIALS_ITEM_ID + EMI_USERNAME))->GetText()
				, (char16*)((GUIEditText*)GetByID(ECIDS_CREDENTIALS_ITEM_ID + EMI_PASSWORD))->GetText());
		}
		break;
	case EPI_BACK:
		{
			BackToPrevSrceen();
		}
		break;
	}
}

bool ScreenCredentials::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_LOCK_WINDOW_CANCELED:
		{
			isCanceled = true;
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			isCanceled = false;
			((GUIEditText*)GetByID(ECIDS_CREDENTIALS_ITEM_ID + EMI_USERNAME))->SetText(pManager->GetCore()->GetApplicationData()->login);
			((GUIEditText*)GetByID(ECIDS_CREDENTIALS_ITEM_ID + EMI_PASSWORD))->SetText(pManager->GetCore()->GetApplicationData()->password);
		}
		break;
	case EEI_WINDOW_DID_ACTIVATE:
		{
			if (pManager->GetCore()->GetApplicationData()->isValid && isLogin)
			{
				pManager->GetLockWindow()->GetText()->SetText(IDS_CONNECTING);
				pManager->GetLockWindow()->Show();
				pManager->GetCore()->GetApplicationData()->isValid = false;
				packetsSent++;
				pManager->GetMemberService()->CheckUserExists(this
					, pManager->GetCore()->GetApplicationData()->login
					, pManager->GetCore()->GetApplicationData()->password);

				isLogin = false;
			}
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}

void ScreenCredentials::OnSuccess( int32 packetId )
{
	packetsSent--;
	if (packetsSent > 0)
	{
		return;
	}
	if (isCanceled)
	{
		return;
	}
	switch(packetId)
	{
	case MemberService::EPS_CHECKUSEREXISTS:
		{
			pManager->GetLockWindow()->Hide();
			pManager->SetNetError(false);
			bool isExist = pManager->GetMemberService()->OnCheckUserExists();
			if (isExist)
			{
				pManager->GetCore()->GetApplicationData()->isValid = true;
				Utils::WStrCpy(pManager->GetCore()->GetApplicationData()->login
					, ((GUIEditText*)GetByID(ECIDS_CREDENTIALS_ITEM_ID + EMI_USERNAME))->GetText());
				Utils::WStrCpy(pManager->GetCore()->GetApplicationData()->password
					, ((GUIEditText*)GetByID(ECIDS_CREDENTIALS_ITEM_ID + EMI_PASSWORD))->GetText());
				pManager->GetCore()->SaveApplicationData();
				pManager->ChangeWindow(ESN_MAIN_MENU, true);
				pManager->GetNewsManager()->ReciveAll();
				pManager->GetInboxManager()->ReciveAll();

			}
			else
			{
				pManager->GetAlert()->GetText()->SetText(IDS_INVALID_NAME_OR_PASSWORD);
				pManager->GetAlert()->Show();
			}
			pManager->GetCore()->SaveApplicationData();
		}
		break;
	}
}

void ScreenCredentials::OnFailed( int32 packetId, char16 *errorString )
{
	packetsSent--;
	if (packetsSent > 0)
	{
		return;
	}
	switch(packetId)
	{
	case MemberService::EPS_CHECKUSEREXISTS:
		{
			pManager->GetLockWindow()->Hide();
			pManager->GetAlert()->GetText()->SetText(IDS_UNABLE_TO_CONNECT);
			pManager->GetAlert()->Show();
			pManager->GetCore()->SaveApplicationData();
			//pManager->ChangeWindow(ESN_CREDENTIALS, true);
		}
		break;
	}
}
