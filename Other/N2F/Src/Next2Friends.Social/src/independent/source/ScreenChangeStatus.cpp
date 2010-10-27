#include "ScreenChangeStatus.h"
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
#include "GUIPhotoImage.h"
#include "GUIEditMultiString.h"
#include "GUIMultiString.h"

#include "GUILockWindow.h"

#include "SurfacesPool.h"


#include "ScreenAsk.h"
#include "VList.h"

#include "N2FData.h"
#include "N2FNewsManager.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

#include "MemberService.h"
#include "AskResponse.h"

ScreenChangeStatus::ScreenChangeStatus( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	AddListener(EEI_SOFT1_PRESSED, this);
	AddListener(EEI_LOCK_WINDOW_CANCELED, this);


	SetStretch(ECS_STRETCH);

	int32 itemHeight = GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	uint8 plt;
	int32 fontHeight = GUISystem::Instance()->GetSkin()->GetFont(EDT_UNSELECTED_TEXT, plt)->GetHeight();

	GUILayoutBox *lay = new GUILayoutBox(true, this);

	GUIMultiString *mt = new GUIMultiString((char16*)L"", lay, ControlRect(0, 0
		, ControlRect::MIN_D, GetApplication()->GetGraphicsSystem()->GetHeight() / 3
		, ControlRect::MAX_D, GetApplication()->GetGraphicsSystem()->GetHeight() / 3));
	mt->SetTextAlign(Font::EAP_HCENTER | Font::EAP_VCENTER);
	mt->SetID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_CURRENT_STATUS);

	GUIText *text = new GUIText(IDS_WRITE_YOUR_STATUS, lay, ControlRect(0, 0, ControlRect::MIN_D, fontHeight, ControlRect::MAX_D, fontHeight));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);

	GUIEditMultiString *ms = new GUIEditMultiString(ENP_MAX_STATUS_SIZE);
	ms->SetDrawType(EDT_MULTIEDIT);
	ms->SetParent(lay);
	ms->SetID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_NEW_STATUS);
	ms->SetTextAlign(Font::EAP_LEFT | Font::EAP_TOP);
	ms->SetMargins((GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_LEFT)->GetWidth() >> 1)
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_RIGHT)->GetWidth() >> 1)
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_TOP_CENTER)->GetHeight() >> 1)
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1));
	ms->SetTextMargins((GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_TOP_CENTER)->GetHeight() >> 1) + 2
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_LEFT)->GetWidth() >> 1) + 2
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1) + 2
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_RIGHT)->GetWidth() >> 1) + 2);
	pLastFocus = ms;

	currentStatus[0] = 0;

}

ScreenChangeStatus::~ScreenChangeStatus( void )
{
}


//const char16	* ScreenOutbox::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
//{
//	if (pManager->GetOutboxManager()->MessagesCount())
//	{
//		switch(index)
//		{
//		case EPI_BACK:
//			{
//				id = EPI_BACK;
//				return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
//			}
//		case EPI_DELETE:
//			{
//				id = EPI_DELETE;
//				return pManager->GetStringWrapper()->GetStringText(IDS_DELETE);
//			}
//		}
//	}
//	else if (index == 0)
//	{
//		id = EPI_BACK;
//		return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
//	}
//	return NULL;
//}
//
//void ScreenOutbox::PopUpOnItemSelected( int32 index, int32 id )
//{
//	switch(id)
//	{
//	case EPI_BACK:
//		{
//			BackToPrevSrceen();
//		}
//		break;
//	case EPI_DELETE:
//		{
//			((N2FMessage*)(list->GetFocusedOwner()))->SetOwner(pManager);
//		}
//		break;
//	}
//}



bool ScreenChangeStatus::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_LOCK_WINDOW_CANCELED:
		{
			isCancel = true;
			BackToPrevSrceen();
			return true;
		}
		break;
	case EEI_SOFT1_PRESSED:
		{
			GUIEditMultiString *ms = (GUIEditMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_NEW_STATUS);
			Utils::WSPrintf(currentStatus,ENP_MAX_STATUS_SIZE * 2, (char16*)L"%s", ms->GetText());

			pManager->GetMemberService()->SetMemberStatusText(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password
				, currentStatus);
			pManager->GetLockWindow()->GetText()->SetText(IDS_SAVING_STATUS);
			pManager->GetLockWindow()->Show();
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			isCancel = false;
			GUIEditMultiString *ms = (GUIEditMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_NEW_STATUS);
			ms->SetText((char16*)L"");
			GUIMultiString *mt = (GUIMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_CURRENT_STATUS);
			char16 tempText[ENP_MAX_STATUS_SIZE * 2];
			Utils::WSPrintf(tempText, ENP_MAX_STATUS_SIZE * 4, (char16*)L"%s %s", pManager->GetStringWrapper()->GetStringText(IDS_I_AM), currentStatus);
			mt->SetText(tempText);
			
		}
		break;
	case EEI_WINDOW_SET_MAIN:
		break;
	case EEI_WINDOW_DID_ACTIVATE:
		{
			pManager->GetMemberService()->GetMemberStatusText(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password);
			pManager->GetLockWindow()->GetText()->SetText(IDS_LOADING_STATUS);
			pManager->GetLockWindow()->Show();
		}
		break;
	case EEI_WINDOW_LOST_MAIN:
	case EEI_WINDOW_WILL_DEACTIVATE:
		{
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}

void ScreenChangeStatus::OnSuccess( int32 packetId )
{
	packetsSent--;
	if (packetsSent > 0)
	{
		return;
	}
	if (isCancel)
	{
		return;
	}
	switch(packetId)
	{
	case MemberService::EPS_GETMEMBERSTATUSTEXT:
		{
			char16 *status = pManager->GetMemberService()->OnGetMemberStatusText();
			pManager->GetLockWindow()->Hide();
			Utils::WSPrintf(currentStatus,ENP_MAX_STATUS_SIZE * 2, (char16*)L"%s", status);
			SAFE_DELETE(status);
			GUIMultiString *mt = (GUIMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_CURRENT_STATUS);
			char16 tempText[ENP_MAX_STATUS_SIZE * 2];
			Utils::WSPrintf(tempText, ENP_MAX_STATUS_SIZE * 4, (char16*)L"%s %s", pManager->GetStringWrapper()->GetStringText(IDS_I_AM), currentStatus);
			mt->SetText(tempText);
		}
		break;
	case MemberService::EPS_SETMEMBERSTATUSTEXT:
		{
			pManager->GetMemberService()->OnSetMemberStatusText();
			pManager->GetLockWindow()->Hide();
			GUIMultiString *mt = (GUIMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_CURRENT_STATUS);
			char16 tempText[ENP_MAX_STATUS_SIZE * 2];
			Utils::WSPrintf(tempText, ENP_MAX_STATUS_SIZE * 4, (char16*)L"%s %s", pManager->GetStringWrapper()->GetStringText(IDS_I_AM), currentStatus);
			mt->SetText(tempText);
			BackToPrevSrceen();
		}
		break;

	}
}

void ScreenChangeStatus::OnFailed( int32 packetId, char16 *errorString )
{
	packetsSent--;
	if (packetsSent > 0)
	{
		return;
	}
	if (isCancel)
	{
		return;
	}
	pManager->GetLockWindow()->Hide();
	BackToPrevSrceen();
}




bool ScreenChangeStatus::PopUpShouldOpen()
{
	return false;
}











