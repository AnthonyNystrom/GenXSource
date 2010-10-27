#include "ScreenGo.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUIMultiString.h"

#include "SkinProperties.h"

#include "ScreenDashboardRead.h"
#include "N2FMessage.h"
#include "N2FOutboxManager.h"


ScreenDashboardRead::ScreenDashboardRead( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{
	AddListener(EEI_SOFT1_PRESSED, this);
	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	SetStretch(ECS_STRETCH);
	GUILayoutBox *lay = new GUILayoutBox(true, this);
	GUILayoutBox *layTop = new GUILayoutBox(false, lay);
	GUIImage *img = new GUIImage(IDB_DASHBOARD_ICONS, layTop);
	int32 h = img->GetImageHeight() << 1;
	int32 w = img->GetImageWidth() << 1;
	img->SetControlRect(ControlRect(0, 0, w, h));
	img->SetID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_IMAGE);
	img->SetImageAlign(GUIControl::EA_VCENTRE|GUIControl::EA_HCENTRE);
	GUILayoutBox *layRight = new GUILayoutBox(true, layTop);
	GUIText *text = new GUIText((char16*)L"", layRight);
	uint8 plt;
	h = GUISystem::Instance()->GetSkin()->GetFont(EDT_UNSELECTED_TEXT, plt)->GetHeight();
	h += h >> 2;
	//text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);
	text->SetControlRect(ControlRect(0, 0, ControlRect::MIN_D, h, ControlRect::MAX_D, h));
	
	text->SetID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_AUTHOR);
	text = new GUIText((char16*)L"", layRight);
	//text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 1, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);
	text->SetID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_DATE);
	text->SetControlRect(ControlRect(0, 0, ControlRect::MIN_D, h, ControlRect::MAX_D, h));

	layTop->SetControlRect(ControlRect(0, 0, ControlRect::MIN_D, h * 2, ControlRect::MAX_D, h * 2));


	GUIMultiString *mt = new GUIMultiString((char16*)L"", lay);
	mt->SetID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_TEXT);
	mt->SetTextMargins(ECP_TEXT_SPACE, ECP_TEXT_SPACE, ECP_TEXT_SPACE, ECP_TEXT_SPACE);
	mt->SetDrawType(EDT_MULTIEDIT);
	mt->SetMargins(ECP_TEXT_SPACE, ECP_TEXT_SPACE, ECP_TEXT_SPACE, ECP_TEXT_SPACE);

	pLastFocus = mt;

}

ScreenDashboardRead::~ScreenDashboardRead( void )
{

}




bool ScreenDashboardRead::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			N2FMessage *msg = pManager->GetWorkingMessage();
			if (pManager->GetWorkingMessage())
			{
				GUIMultiString *mt = (GUIMultiString*)(this->GetByID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_TEXT));
				switch(msg->GetInnerType())
				{
				case EIT_DASHBOARD_FIREND:
					{
						char16 temp[1000];
						Utils::WSPrintf(temp, 1000 * 2
							, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_FRIEND)
							, msg->GetText(ETT_DASHBOARD_NICKNAME2)
							, msg->GetText(ETT_DASHBOARD_NICKNAME1));
						mt->SetText(temp);
					}
					break;
				case EIT_DASHBOARD_WALL:
					{
						char16 temp[1000];
						Utils::WSPrintf(temp, 1000 * 2
							, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_READ_WALL)
							, msg->GetText(ETT_DASHBOARD_NICKNAME1)
							, msg->GetText(ETT_DASHBOARD_NICKNAME2)
							, msg->GetText(ETT_DASHBOARD_TEXT));
						mt->SetText(temp);
					}
					break;
				case EIT_DASHBOARD_PHOTO:
					{
						char16 temp[1000];
						Utils::WSPrintf(temp, 1000 * 2
							, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_READ_PHOTO)
							, msg->GetText(ETT_DASHBOARD_NICKNAME1)
							, msg->GetText(ETT_DASHBOARD_TITLE)
							, msg->GetText(ETT_DASHBOARD_TEXT));
						mt->SetText(temp);
					}
					break;
				case EIT_DASHBOARD_VIDEO:
					{
						char16 temp[1000];
						Utils::WSPrintf(temp, 1000 * 2
							, pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_READ_VIDEO)
							, msg->GetText(ETT_DASHBOARD_NICKNAME1)
							, msg->GetText(ETT_DASHBOARD_TITLE)
							, msg->GetText(ETT_DASHBOARD_TEXT));
						mt->SetText(temp);
					}
					break;
				}
				GUIImage *img = (GUIImage*)(this->GetByID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_IMAGE));
				img->SetFrame(msg->GetInnerType() - EIT_DASHBOARD_FIRST);

				GUIText *tx = (GUIText*)(this->GetByID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_DATE));
				tx->SetText(msg->GetDateTime());
				tx = (GUIText*)(this->GetByID(ECIDS_DASHBOARD_READ_ITEM_ID + EMI_AUTHOR));
				tx->SetText(msg->GetText(ETT_DASHBOARD_NICKNAME1));
			}
		}
		break;
	case EEI_WINDOW_WILL_DEACTIVATE:
		{
			pManager->SetWorkingMesage(NULL);
			return true;
		}
		break;
	case EEI_SOFT1_PRESSED:
		{
			BackToPrevSrceen();
			return true;
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}


bool ScreenDashboardRead::PopUpShouldOpen()
{
	return false;
}