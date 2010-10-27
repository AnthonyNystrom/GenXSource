#include "ScreenGo.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"

#include "SkinProperties.h"

#include "ScreenSendSelection.h"
#include "N2FMessage.h"
#include "N2FOutboxManager.h"


ScreenSendSelection::ScreenSendSelection( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{
	AddListener(EEI_SOFT1_PRESSED, this);
	SetStretch(ECS_STRETCH);
	GUIListView *list = new GUIListView(this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);
	for (int i = 0; i < EMI_COUNT; i++)
	{
		GUIMenuItem *mi = new GUIMenuItem(EDT_SELECTED_ITEM, list);
		mi->AddListener( EEI_BUTTON_PRESSED, this);
		mi->SetID(ECIDS_SELECT_SEND_ITEM_ID + i);
		GUIText *text = new GUIText(IDS_SEND_NOW + i, mi);
		text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
		text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
		text->SetDrawType(EDT_UNSELECTED_TEXT);
		if (i == 0)
		{
			pLastFocus = mi;
		}
	}
}

ScreenSendSelection::~ScreenSendSelection( void )
{

}




void ScreenSendSelection::OnItem( int32 index )
{
	switch(index)
	{
	case EMI_SEND_NOW:
		{
			pManager->GetWorkingMessage()->SetSend(true);
		}
		break;
	case EMI_MOVE_TO_OUTBOX:
		{
			pManager->GetWorkingMessage()->SetSend(false);
		}
		break;
	}
	pManager->GetWorkingMessage()->SetOwner(pManager->GetOutboxManager());
	pManager->SetWorkingMesage(NULL);

	pManager->ChangeWindow(ESN_MAIN_MENU, true);
}

bool ScreenSendSelection::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_BUTTON_PRESSED:
		{
			OnItem(((GUIControl*)(pData->lParam))->GetID() - ECIDS_SELECT_SEND_ITEM_ID);
			return true;
		}
		break;
	case EEI_SOFT1_PRESSED:
		{
			OnItem(GUISystem::Instance()->GetFocus()->GetID() - ECIDS_SELECT_SEND_ITEM_ID);
			return true;
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}


bool ScreenSendSelection::PopUpShouldOpen()
{
	return false;
}