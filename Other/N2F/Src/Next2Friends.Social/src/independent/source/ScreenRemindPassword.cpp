#include "ScreenRemindPassword.h"
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

#include "SkinProperties.h"
#include "MemberService.h"

ScreenRemindPassword::ScreenRemindPassword( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	int32 itemHeight = GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	uint8 plt;
	int32 fontHeight = GUISystem::Instance()->GetSkin()->GetFont(EDT_UNSELECTED_TEXT, plt)->GetHeight();

	AddListener(EEI_SELECT_PRESSED, this);
	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);


	SetStretch(ECS_STRETCH);
	GUIListView *list = new GUIListView(this);
	list->SetPrescroll(itemHeight * 2);
	list->SetInnerMargins( itemHeight >> 2, itemHeight >> 2
		, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);

	GUIText *text = new GUIText(IDS_SEND_TO, list, ControlRect(0, 0, ControlRect::MIN_D, fontHeight, ControlRect::MAX_D, fontHeight));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_HCENTER | Font::EAP_VCENTER);
	text->SetDrawType(EDT_UNSELECTED_TEXT);

	text = new GUIText(IDS_PHONE_NUMBER, list, ControlRect(0, 0, ControlRect::MIN_D, newRect.minDy / 5, ControlRect::MAX_D, newRect.minDy / 5));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);

	GUILayoutBox *layout = new GUILayoutBox(false, list);
	
	GUIImage *check = new GUIImage(IDB_CHECK_BOX, layout);
	check->SetControlRect(ControlRect(0, 0, check->GetImageWidth(), check->GetImageHeight()));
	check->SetID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_SMS_CHECKBOX);
	check->SetAlignType(GUIControl::EA_HCENTRE | GUIControl::EA_VCENTRE);

	GUIEditText *editText = new GUIEditText(ENP_MAX_PHONE_NUMBER_SIZE, EETT_DIGITS, layout);
	editText->SetID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_SMS);
	editText->SetDrawType(EDT_EDITBOX);
	editText->SetFocusedDrawType(EDT_EDITBOX_ACTIVE);
	editText->SetTextMargins(ECP_EDITTEXT_MARGIN_TOP, ECP_EDITTEXT_MARGIN_SIDE, ECP_EDITTEXT_MARGIN_BOTTOM, ECP_EDITTEXT_MARGIN_SIDE);
	layout->SetControlRect(editText->GetControlRect());
	pLastFocus = editText;

	text = new GUIText(IDS_EMAIL, list, ControlRect(0, 0, ControlRect::MIN_D, newRect.minDy / 5, ControlRect::MAX_D, newRect.minDy / 5));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);

	layout = new GUILayoutBox(false, list);
	
	check = new GUIImage(IDB_CHECK_BOX, layout);
	check->SetControlRect(ControlRect(0, 0, check->GetImageWidth(), check->GetImageHeight()));
	check->SetID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_EMAIL_CHECKBOX);
	check->SetAlignType(GUIControl::EA_HCENTRE | GUIControl::EA_VCENTRE);

	editText = new GUIEditText(ENP_MAX_EMAIL_SIZE, EETT_ALL, layout);
	editText->SetID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_EMAIL);
	editText->SetDrawType(EDT_EDITBOX);
	editText->SetFocusedDrawType(EDT_EDITBOX_ACTIVE);
	editText->SetTextMargins(ECP_EDITTEXT_MARGIN_TOP, ECP_EDITTEXT_MARGIN_SIDE, ECP_EDITTEXT_MARGIN_BOTTOM, ECP_EDITTEXT_MARGIN_SIDE);
	layout->SetControlRect(editText->GetControlRect());
}

ScreenRemindPassword::~ScreenRemindPassword( void )
{

}

const char16	* ScreenRemindPassword::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	switch(index)
	{
	case EPI_BACK:
		{
			return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
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

void ScreenRemindPassword::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(index)
	{
	case EPI_SUBMIT:
		{
			Utils::WStrCpy(pManager->GetCore()->GetApplicationData()->remindPhoneNum
				, ((GUIEditText*)GetByID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_SMS))->GetText());
			Utils::WStrCpy(pManager->GetCore()->GetApplicationData()->remindEmail
				, ((GUIEditText*)GetByID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_EMAIL))->GetText());
			pManager->GetCore()->SaveApplicationData();
			if (((GUIImage*)GetByID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_EMAIL_CHECKBOX))->GetFrame() != 0)
			{
				pManager->GetMemberService()->RemindPassword(this, pManager->GetCore()->GetApplicationData()->remindEmail);
			}
		}
		break;
	case EPI_BACK:
		{
			BackToPrevSrceen();
		}
		break;
	}
}

bool ScreenRemindPassword::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_SELECT_PRESSED:
		{
			GUIControl *c = GUISystem::Instance()->GetFocus();
			int32 i = c->GetID();
			GUIImage *img;
			switch(i)
			{
			case ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_SMS:
				{
					img = (GUIImage*)GetByID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_SMS_CHECKBOX);
				}
				break;
			case ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_EMAIL:
				{
					img = (GUIImage*)GetByID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_EMAIL_CHECKBOX);
				}
				break;
			}
			if (img->GetFrame() == 1)
			{
				img->SetFrame(0);
			}
			else
			{
				img->SetFrame(1);
			}
			return true;
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			((GUIEditText*)GetByID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_SMS))->SetText(pManager->GetCore()->GetApplicationData()->remindPhoneNum);
			((GUIEditText*)GetByID(ECIDS_REMIND_PASSWORD_ITEM_ID + EMI_VIA_EMAIL))->SetText(pManager->GetCore()->GetApplicationData()->remindEmail);
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}

void ScreenRemindPassword::OnSuccess( int32 packetId )
{
	switch(packetId)
	{
	case MemberService::EPS_REMINDPASSWORD:
		{
			pManager->GetMemberService()->OnRemindPassword();
			pManager->GetAlert()->GetText()->SetText(IDS_REMIND_PASSWORD_DONE);
			pManager->GetAlert()->Show();
		}
		break;
	}
}

void ScreenRemindPassword::OnFailed( int32 packetId, char16 *errorString )
{
	switch(packetId)
	{
	case MemberService::EPS_REMINDPASSWORD:
		{
			pManager->GetAlert()->GetText()->SetText(IDS_WRONG_EMAIL);
			pManager->GetAlert()->Show();
		}
		break;
	}
}