#include "ScreenTemplates.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"

#include "ScreenAsk.h"
#include "VList.h"

#include "N2FData.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

ScreenTemplates::ScreenTemplates( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);

	SetStretch(ECS_STRETCH);
	list = new GUIListView(this);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);
	for (int i = 0; i < ENP_MAX_TEMPLATES; i++)
	{
		templateItem[i] = new GUIMenuItem(EDT_SELECTED_ITEM);
		templateItem[i]->AddListener(EEI_BUTTON_PRESSED, this);
		templateItem[i]->SetID(ECIDS_TEMPLATES_ITEM_ID + i);
		GUIText *text = new GUIText((char16*)L" ", templateItem[i]);
		text->SetID(ECIDS_TEXT_ID);
		text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
		text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
		text->SetDrawType(EDT_UNSELECTED_TEXT);
	}
}

ScreenTemplates::~ScreenTemplates( void )
{
	list->RemoveAllItems();
	for (int i = 0; i < ENP_MAX_TEMPLATES; i++)
	{
		SAFE_DELETE(templateItem[i]);
	}
}


const char16	* ScreenTemplates::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	return NULL;
}

void ScreenTemplates::PopUpOnItemSelected( int32 index, int32 id )
{
}

void ScreenTemplates::OnItem( int32 index )
{
	GUIText *t = (GUIText*)(templateItem[index]->GetByID(ECIDS_TEXT_ID));
	N2FMessage *msg = pManager->GetWorkingMessage();
	if (Utils::WStrLen(msg->GetText(ETT_QUESTION_TEXT)) + Utils::WStrLen(t->GetText()) + 1 > ENP_MAX_QUESTION_SIZE)
	{
		msg->SetText(t->GetText(), ETT_QUESTION_TEXT);
	}
	else
	{
		char16 newText[ENP_MAX_QUESTION_SIZE];
		Utils::WSPrintf(newText, ENP_MAX_QUESTION_SIZE * 2, (char16*)L"%s%s", msg->GetText(ETT_QUESTION_TEXT), t->GetText());
		msg->SetText(newText, ETT_QUESTION_TEXT);
	}
	
	BackToPrevSrceen();
}

bool ScreenTemplates::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_BUTTON_PRESSED:
		{
			OnItem(((GUIControl*)(pData->lParam))->GetID() - ECIDS_TEMPLATES_ITEM_ID);
			return true;
		}
		break;
	case EEI_SOFT1_PRESSED:
		{
			OnItem(GUISystem::Instance()->GetFocus()->GetID() - ECIDS_TEMPLATES_ITEM_ID);
			return true;
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			list->RemoveAllItems();
			list->Reset();
			VList *tList = pManager->GetTemplates();
			VList::Iterator it = tList->Begin();
			int32 i = 0;
			for (;it != tList->End(); it++)
			{
				GUIText *t = (GUIText*)(templateItem[i]->GetByID(ECIDS_TEXT_ID));
				t->SetText((char16*)(*it));
				list->AddChild(templateItem[i]);
				i++;
			}
			pLastFocus = templateItem[0];
		}
		break;
	case EEI_WINDOW_DID_ACTIVATE:
		{
			GUISystem::Instance()->SetFocus(pLastFocus);
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}

