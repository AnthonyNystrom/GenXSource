#include "BaseScreen.h"
#include "GUIPopUp.h"
#include "GUIFooter.h"
#include "FooterProps.h"


BaseScreen::BaseScreen	(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID) 
	:	GUIControlContainer	(NULL, newRect)
{
	SetID(screenID);
	pManager = pAppMng;
	AddListener(EEI_WINDOW_SET_MAIN, this);
	AddListener(EEI_WINDOW_LOST_MAIN, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	AddListener(EEI_SOFT1_PRESSED, this);
	AddListener(EEI_SOFT2_PRESSED, this);
	AddListener(EEI_CLR_PRESSED, this);
}

BaseScreen::~BaseScreen()
{
}


//default event processors
bool BaseScreen::OnEvent(eEventID eventID, EventData *pData)
{
	switch(eventID)
	{
	case EEI_WINDOW_SET_MAIN:
		GUISystem::Instance()->SetFocus(pLastFocus);
		break;
	case EEI_WINDOW_LOST_MAIN:
		if (this == GUISystem::Instance()->GetRoot(GUISystem::Instance()->GetFocus()))
		{
			pLastFocus = GUISystem::Instance()->GetFocus();
			GUISystem::Instance()->SetFocus(NULL);
		}
		break;
	case EEI_WINDOW_WILL_DEACTIVATE:
		{
			if (pManager->GetPopUp())
			{
				pManager->GetPopUp()->SetDelegate(NULL);
			}
		}
		break;
	case EEI_WINDOW_DID_ACTIVATE:
		{
			if (pManager->GetPopUp())
			{
				pManager->GetPopUp()->SetDelegate(this);
			}
			if (pManager->GetFooter())
			{
				FASSERT(SOFT_KEYS_ID[ESN_COUNT * 2] == SOFT_KEYS_CHECK_ANCHOR);
				pManager->GetFooter()->SetText(SOFT_KEYS_ID[(GetID() - ESN_FIRST) * 2], SOFT_KEYS_ID[(GetID() - ESN_FIRST) * 2 + 1]);
			}
		}
		break;
	case EEI_CLR_PRESSED:
	case EEI_SOFT2_PRESSED:
		{
			BackToPrevSrceen();
		}
		break;
	case EEI_SOFT1_PRESSED:
		pManager->GetPopUp()->Show();
		break;
	}

	return true;
}

bool BaseScreen::PopUpShouldOpen()
{
	return true;
}


const char16	* BaseScreen::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	return NULL;
}

void BaseScreen::PopUpOnItemSelected( int32 index, int32 id )
{

}

void BaseScreen::BackToPrevSrceen()
{
	if (prevScreen)
	{
		pManager->ChangeWindow(prevScreen, true);
	}
	else
	{
		pManager->CloseApplication();
	}
}

void BaseScreen::SetPrevScreen( int32 prevID )
{
	prevScreen = prevID;
}