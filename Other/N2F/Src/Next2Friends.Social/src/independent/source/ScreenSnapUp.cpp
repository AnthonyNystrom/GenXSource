#include "ScreenSnapUp.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"

#include "SkinProperties.h"

#include "ScreenAsk.h"
#include "ScreenChangeStatus.h"
#include "N2FMessage.h"

const int16 STRING_IDS[] = {IDS_UPLOAD_FROM_FILE, IDS_UPLOAD_FORM_CAMERA};

ScreenSnapUp::ScreenSnapUp( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{
	SetStretch(ECS_STRETCH);
	AddListener(EEI_SOFT1_PRESSED, this);

	GUIListView *list = new GUIListView(this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);
	for (int i = 0; i < EMI_COUNT; i++)
	{
		GUIMenuItem *mi = new GUIMenuItem(EDT_SELECTED_ITEM, list);
		mi->AddListener( EEI_BUTTON_PRESSED, this);
		mi->SetID(ECIDS_GO_ITEM_ID + i);
		GUIText *text = new GUIText(STRING_IDS[i], mi);
		text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
		text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
		text->SetDrawType(EDT_UNSELECTED_TEXT);
		if (i == 0)
		{
			pLastFocus = mi;
		}
	}
}

ScreenSnapUp::~ScreenSnapUp( void )
{

}


//const char16	* ScreenSnapUp::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
//{
//	switch(index)
//	{
//	case EPI_SELECT:
//		{
//			return pManager->GetStringWrapper()->GetStringText(IDS_SELECT);
//		}
//		break;
//	case EPI_BACK:
//		{
//			return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
//		}
//		break;
//	}
//	return NULL;
//}
//
//void ScreenSnapUp::PopUpOnItemSelected( int32 index, int32 id )
//{
//	switch(index)
//	{
//	case EPI_SELECT:
//		{
//			OnItem(pLastFocus->GetID() - ECIDS_GO_ITEM_ID);
//		}
//		break;
//	case EPI_BACK:
//		{
//			BackToPrevSrceen();
//		}
//		break;
//	}
//}

bool ScreenSnapUp::PopUpShouldOpen()
{
	return false;
}


void ScreenSnapUp::OnItem( int32 index )
{
	switch(index)
	{
	case EMI_UPLOAD_FROM_FILE:
		{
			N2FMessage *msg = pManager->GetFreeMessage();
			msg->InitAsPhoto();
			pManager->SetWorkingMesage(msg);

			pManager->ChangeWindow(ESN_ATTACH_FROM_FILE, false);
		}
		break;
	case EMI_UPLOAD_FROM_CAMERA:
		{
			N2FMessage *msg = pManager->GetFreeMessage();
			msg->InitAsPhoto();
			pManager->SetWorkingMesage(msg);

			pManager->ChangeWindow(ESN_CAMERA, false);
		}
		break;
	}
}

bool ScreenSnapUp::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_BUTTON_PRESSED:
		{
			OnItem(((GUIControl*)(pData->lParam))->GetID() - ECIDS_GO_ITEM_ID);
			return true;
		}
		break;
	case EEI_SOFT1_PRESSED:
		{
			OnItem(GUISystem::Instance()->GetFocus()->GetID() - ECIDS_GO_ITEM_ID);
			return true;
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}

