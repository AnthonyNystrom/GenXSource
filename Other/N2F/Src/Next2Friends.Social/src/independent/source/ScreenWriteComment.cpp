#include "ScreenWriteComment.h"
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

#include "AskService.h"
#include "AskResponse.h"

ScreenWriteComment::ScreenWriteComment( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	AddListener(EEI_SOFT1_PRESSED, this);

	SetStretch(ECS_STRETCH);

	int32 itemHeight = GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	uint8 plt;
	int32 fontHeight = GUISystem::Instance()->GetSkin()->GetFont(EDT_UNSELECTED_TEXT, plt)->GetHeight();

	GUILayoutBox *lay = new GUILayoutBox(true, this);

	GUIMultiString *mt = new GUIMultiString((char16*)L"", lay, ControlRect(0, 0
		, ControlRect::MIN_D, GetApplication()->GetGraphicsSystem()->GetHeight() / 3
		, ControlRect::MAX_D, GetApplication()->GetGraphicsSystem()->GetHeight() / 3));
	mt->SetTextAlign(Font::EAP_HCENTER | Font::EAP_VCENTER);
	mt->SetID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_COMMENT);

	GUIText *text = new GUIText(IDS_WRITE_YOUR_COMMENT, lay, ControlRect(0, 0, ControlRect::MIN_D, fontHeight, ControlRect::MAX_D, fontHeight));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);

	GUIEditMultiString *ms = new GUIEditMultiString(ENP_MAX_QUESTION_SIZE);
	ms->SetDrawType(EDT_MULTIEDIT);
	ms->SetParent(lay);
	ms->SetID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_REPLY_COMMENT);
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

}

ScreenWriteComment::~ScreenWriteComment( void )
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



bool ScreenWriteComment::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_SOFT1_PRESSED:
		{
			N2FMessage *msg = pManager->GetFreeMessage();
			msg->InitAsComment();
			if (pManager->GetWorkingMessage()->GetType() == EMT_QUESTION_NAME)
			{
				msg->SetCommentID(0);
			}
			else if (pManager->GetWorkingMessage()->GetType() == EMT_COMMENT)
			{
				msg->SetCommentID(pManager->GetWorkingMessage()->GetCommentID());
			}
			msg->SetID(pManager->GetWorkingMessage()->GetID());
			GUIEditMultiString *ms = (GUIEditMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_REPLY_COMMENT);
			msg->SetText(ms->GetText(), ETT_COMMENT_TEXT);
			msg->SetText(pManager->GetCore()->GetApplicationData()->login, ETT_COMMENT_NICKNAME);
			pManager->SetWorkingMesage(msg);
			pManager->ChangeWindow(ESN_SEND_SELECTION, false);
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			GUIEditMultiString *ms = (GUIEditMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_REPLY_COMMENT);
			ms->SetText((char16*)L"");
			GUIMultiString *mt = (GUIMultiString *)GetByID(ECIDS_WRITE_COMMENT_ITEM_ID + EMI_COMMENT);
			N2FMessage *msg = pManager->GetWorkingMessage();
			if (msg)
			{
				if (msg->GetType() == EMT_QUESTION_NAME)
				{
					mt->SetText(msg->GetText(ETT_QUESTION_TEXT));
				}
				if (msg->GetType() == EMT_COMMENT)
				{
					mt->SetText(msg->GetText(ETT_COMMENT_TEXT));
				}
			}
			else
			{
				mt->SetText((char16*)L"");
			}
			
		}
		break;
	case EEI_WINDOW_SET_MAIN:
	case EEI_WINDOW_DID_ACTIVATE:
		{
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





bool ScreenWriteComment::PopUpShouldOpen()
{
	return false;
}











