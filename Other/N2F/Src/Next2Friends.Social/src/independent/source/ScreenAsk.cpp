#include "ScreenAsk.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUITabList.h"
#include "GUISelector.h"
#include "GUIEditText.h"
#include "GUIEditMultiString.h"
#include "GUIItemsList.h"
#include "GUIPhotoImage.h"
#include "GUIAlert.h"

#include "N2FMessage.h"
#include "N2FDraftsManager.h"
#include "N2FOutboxManager.h"
#include "SurfacesPool.h"
#include "LibPhotoItem.h"

#include "SkinProperties.h"

ScreenAsk::ScreenAsk(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID)
:BaseScreen(newRect, pAppMng, screenID)
{

	workMsg = new N2FMessage();

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);

	int32 itemHeight = GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	uint8 plt;
	int32 fontHeight = GUISystem::Instance()->GetSkin()->GetFont(EDT_UNSELECTED_TEXT, plt)->GetHeight();


	isAB = false;

	//SetStretch(ECS_STRETCH);
	tabList = new GUITabList(this, newRect);
	tabList->SetHeaderDrawType(EDT_TAB_BACK);
	tabList->SetSelectorDrawType(EDT_TAB_SELECTOR);
	tabList->AddListener(EEI_TAB_PAGE_SWITCHED, this);

	//=========  create first page
	GUIImage *pi = new GUIImage(IDB_ASK_ICONS);
	pi->SetFrame(0);

	GUILayoutBox *c1 = new GUILayoutBox();
	c1->SetID(ECIDS_ASK_ITEM_ID + EMI_PAGE_1);
	GUIText *text = new GUIText(IDS_ENTER_QUESTION, c1, ControlRect(0, 0, ControlRect::MIN_D, fontHeight, ControlRect::MAX_D, fontHeight));
	text->SetTextMargins(0, itemHeight >> 2, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_UNSELECTED_TEXT);

	GUIEditMultiString *ms = new GUIEditMultiString(ENP_MAX_QUESTION_SIZE);
	ms->SetDrawType(EDT_MULTIEDIT);
	ms->SetParent(c1);
	ms->SetID(ECIDS_ASK_ITEM_ID + EMI_QUESTION_TEXT);
	ms->SetTextAlign(Font::EAP_LEFT | Font::EAP_TOP);
	ms->SetMargins((GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_LEFT)->GetWidth() >> 1)
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_RIGHT)->GetWidth() >> 1)
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_TOP_CENTER)->GetHeight() >> 1)
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1));
	ms->SetTextMargins((GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_TOP_CENTER)->GetHeight() >> 1) + 2
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_LEFT)->GetWidth() >> 1) + 2
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1) + 2
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_MULTIEDIT, GUISkinLocal::ESS9_CENTER_RIGHT)->GetWidth() >> 1) + 2);


	tabList->AddPage(c1, pi, ms);
	//====================================



	//=========  create second page
	pi = new GUIImage(IDB_ASK_ICONS);
	pi->SetFrame(1);

	GUIListView *c2 = new GUIListView();
	c2->SetChildsMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);
	c2->SetID(ECIDS_ASK_ITEM_ID + EMI_PAGE_2);

	GUILayoutBox*lay = new GUILayoutBox(false, c2);
	lay->SetID(ECIDS_ASK_ITEM_ID + EMI_TYPE_LAYOUT);

	
	GUIText *t = new GUIText(IDS_TYPE, lay);
	t->SetTextMargins(0, ECP_MENULIST_MARGIN, 0, 0);
	t->SetDrawType(EDT_UNSELECTED_TEXT);
	t->SetTextAlign(Font::EAP_LEFT|Font::EAP_VCENTER);
	t->SetControlRect(ControlRect(0, 0, t->GetTextWidth() + ECP_TEXT_SPACE, ControlRect::MIN_D, t->GetTextWidth() + ECP_TEXT_SPACE, ControlRect::MAX_D));

	GUISelector *sel = new GUISelector(lay);
	sel->SetSelectedDrawType(EDT_SELECTED_ITEM);
	sel->SetLeftArrow(IDB_COMBO_ARROW_SELECTED_LEFT, IDB_COMBO_ARROW_LEFT, IDB_COMBO_ARROW_NONE);
	sel->SetRightArrow(IDB_COMBO_ARROW_SELECTED_RIGHT, IDB_COMBO_ARROW_RIGHT, IDB_COMBO_ARROW_NONE);
	sel->SetChildsDrawType(EDT_SELECTED_TEXT, EDT_UNSELECTED_TEXT);
	sel->SetID(ECIDS_ASK_ITEM_ID + EMI_TYPE_SELECTOR);
	sel->AddListener(EEI_SELECTOR_CHANGED, this);
	t = new GUIText(IDS_YES_OR_NO, sel);
	t->SetID(ECIDS_ASK_ITEM_ID + EMI_TYPE_YES_NO);
	t = new GUIText(IDS_ANSWER_A_OR_B, sel);
	t->SetID(ECIDS_ASK_ITEM_ID + EMI_TYPE_A_OR_B);
	t = new GUIText(IDS_RATE_1_TO_10, sel);
	t->SetID(ECIDS_ASK_ITEM_ID + EMI_TYPE_RATE);
	lay->SetControlRect(sel->GetControlRect());

	tabList->AddPage(c2, pi, sel);

	lay = new GUILayoutBox(false, c2);
	lay->SetID(ECIDS_ASK_ITEM_ID + EMI_DURATION_LAYOUT);

	t = new GUIText(IDS_DURATION, lay);
	t->SetTextMargins(0, ECP_MENULIST_MARGIN, 0, 0);
	t->SetDrawType(EDT_UNSELECTED_TEXT);
	t->SetTextAlign(Font::EAP_LEFT|Font::EAP_VCENTER);
	t->SetControlRect(ControlRect(0, 0, t->GetTextWidth() + ECP_TEXT_SPACE, ControlRect::MIN_D, t->GetTextWidth() + ECP_TEXT_SPACE, ControlRect::MAX_D));

	sel = new GUISelector(lay);
	sel->SetSelectedDrawType(EDT_SELECTED_ITEM);
	sel->SetLeftArrow(IDB_COMBO_ARROW_SELECTED_LEFT, IDB_COMBO_ARROW_LEFT, IDB_COMBO_ARROW_NONE);
	sel->SetRightArrow(IDB_COMBO_ARROW_SELECTED_RIGHT, IDB_COMBO_ARROW_RIGHT, IDB_COMBO_ARROW_NONE);
	sel->SetChildsDrawType(EDT_SELECTED_TEXT, EDT_UNSELECTED_TEXT);
	sel->SetID(ECIDS_ASK_ITEM_ID + EMI_DURATION_SELECTOR);
	t = new GUIText(IDS_5MIN, sel);
	t->SetID(ECIDS_ASK_ITEM_ID + EMI_DURATION_5MIN);
	t = new GUIText(IDS_10MIN, sel);
	t->SetID(ECIDS_ASK_ITEM_ID + EMI_DURATION_10MIN);
	t = new GUIText(IDS_60MIN, sel);
	t->SetID(ECIDS_ASK_ITEM_ID + EMI_DURATION_60MIN);
	t = new GUIText(IDS_24HRS, sel);
	t->SetID(ECIDS_ASK_ITEM_ID + EMI_DURATION_24HRS);
	lay->SetControlRect(sel->GetControlRect());

	GUIMenuItem *mi = new GUIMenuItem(EDT_SELECTED_ITEM, c2);
	mi->AddListener(EEI_BUTTON_PRESSED, this);
	mi->SetID(ECIDS_ASK_ITEM_ID + EMI_PRIVATE_QUESTION);
	GUIImage *i = new GUIImage(IDB_CHECK_BOX, mi);
	i->SetControlRect(ControlRect(0, 0, i->GetImageWidth() * 2, ControlRect::MIN_D, i->GetImageWidth() * 2, ControlRect::MAX_D));
	i->SetID(ECIDS_ASK_ITEM_ID + EMI_PRIVATE_QUESTION_CHECKBOX);
	t = new GUIText(IDS_PRIVATE_QUESTION, mi);
	t->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);


    
	c2->Update();
	//====================================


	//=========  create third page
	pi = new GUIImage(IDB_ASK_ICONS);
	pi->SetFrame(2);

	GUIControlContainer *c3 = new GUIControlContainer();
	c3->SetID(ECIDS_ASK_ITEM_ID + EMI_PAGE_3);
	c3->SetStretch(/*eChildStretch::*/ECS_STRETCH);

	photoList = new GUIItemsList(this, c3);
	photoList->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_PHOTO_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() / 2);


	submitReq = new GUIMultiString(IDS_SUBMIT_REQ, c3);
	submitReq->SetTextAlign(Font::EAP_HCENTER | Font::EAP_VCENTER);
	submitReq->SetTextMargins(fontHeight, fontHeight, fontHeight, fontHeight);

	tabList->AddPage(c3, pi, NULL);
	//====================================

	{
		aLayout = new GUILayoutBox(false);

		GUIText *text = new GUIText(IDS_A, aLayout);
		int32 w = text->GetTextWidth() * 2;
		text->SetDrawType(EDT_UNSELECTED_TEXT);
		text->SetTextMargins(0, ECP_MENULIST_MARGIN, 0, 0);
		text->SetTextAlign(Font::EAP_LEFT|Font::EAP_VCENTER);
		text->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));


		GUIEditText *editText = new GUIEditText(ENP_MAX_ADDTEXT_SIZE, EETT_ALL, aLayout);
		editText->SetSelectable(true);
		editText->SetID(ECIDS_ASK_ITEM_ID + EMI_EDIT_TEXT_A);
		editText->SetDrawType(EDT_EDITBOX);
		editText->SetFocusedDrawType(EDT_EDITBOX_ACTIVE);
		editText->SetTextMargins(ECP_EDITTEXT_MARGIN_TOP, ECP_EDITTEXT_MARGIN_SIDE, ECP_EDITTEXT_MARGIN_BOTTOM, ECP_EDITTEXT_MARGIN_SIDE);
		aLayout->SetControlRect(editText->GetControlRect());


		bLayout = new GUILayoutBox(false);

		text = new GUIText(IDS_B, bLayout);
		text->SetDrawType(EDT_UNSELECTED_TEXT);
		text->SetTextMargins(0, ECP_MENULIST_MARGIN, 0, 0);
		text->SetTextAlign(Font::EAP_LEFT|Font::EAP_VCENTER);
		text->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));


		editText = new GUIEditText(ENP_MAX_ADDTEXT_SIZE, EETT_ALL, bLayout);
		editText->SetSelectable(true);
		editText->SetID(ECIDS_ASK_ITEM_ID + EMI_EDIT_TEXT_B);
		editText->SetDrawType(EDT_EDITBOX);
		editText->SetFocusedDrawType(EDT_EDITBOX_ACTIVE);
		editText->SetTextMargins(ECP_EDITTEXT_MARGIN_TOP, ECP_EDITTEXT_MARGIN_SIDE, ECP_EDITTEXT_MARGIN_BOTTOM, ECP_EDITTEXT_MARGIN_SIDE);
		bLayout->SetControlRect(editText->GetControlRect());
	}

	surfPool = new SurfacesPool(3, 50, 50);

}

ScreenAsk::~ScreenAsk(void)
{
	SAFE_DELETE(workMsg);
	SAFE_DELETE(surfPool);
	
	submitReq->SetParent(NULL);
	SAFE_DELETE(submitReq);

	if (aLayout)
	{
		aLayout->SetParent(NULL);
		SAFE_DELETE(aLayout);
	}
	if (bLayout)
	{
		bLayout->SetParent(NULL);
		SAFE_DELETE(bLayout);
	}

}


const char16	* ScreenAsk::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	switch(tabList->GetActivePageIndex())
	{
	case 0:
		{
			switch(index)
			{
			case 0:
				{
					id = EPI_TEMPLATES;
					return pManager->GetStringWrapper()->GetStringText(IDS_TEMPLATES);
				}
			case 1:
				{
					id = EPI_QUESTION_OPTIONS;
					return pManager->GetStringWrapper()->GetStringText(IDS_QUESTION_OPTIONS);
				}
			case 2:
				{
					id = EPI_ATTACH_PHOTOS;
					return pManager->GetStringWrapper()->GetStringText(IDS_ATTACH_PHOTO);
				}
			}
		}
		break;
	case 1:
		{
			switch(index)
			{
			case 0:
				{
					id = EPI_QUESTION_TEXT;
					return pManager->GetStringWrapper()->GetStringText(IDS_QUESTION_TEXT);
				}
			case 1:
				{
					id = EPI_ATTACH_PHOTOS;
					return pManager->GetStringWrapper()->GetStringText(IDS_ATTACH_PHOTO);
				}
			}
		}
		break;
	case 2:
		{
			if (index == 0)
			{
				if (workMsg->IsPossibleToAttachPhoto())
				{
					id = EPI_ATTACH_FROM_CAMERA;
					return pManager->GetStringWrapper()->GetStringText(IDS_ATTACH_FROM_CAMERA);
				}
				else
				{
					id = EPI_VIEW;
					return pManager->GetStringWrapper()->GetStringText(IDS_VIEW);
				}
			}
			if (prevId == EPI_ATTACH_FROM_CAMERA)
			{
				id = EPI_ATTACH_FROM_FILE;
				return pManager->GetStringWrapper()->GetStringText(IDS_ATTACH_FROM_FILE);
			}
			if (prevId == EPI_ATTACH_FROM_FILE && workMsg->GetNumberOfPhotos() > 0)
			{
				id = EPI_VIEW;
				return pManager->GetStringWrapper()->GetStringText(IDS_VIEW);
			}
			if (prevId == EPI_VIEW)
			{
				id = EPI_REMOVE_PHOTO;
				return pManager->GetStringWrapper()->GetStringText(IDS_REMOVE_PHOTO);
			}
			if (prevId == EPI_QUESTION_TEXT)
			{
				id = EPI_QUESTION_OPTIONS;
				return pManager->GetStringWrapper()->GetStringText(IDS_QUESTION_OPTIONS);
			}
			if (prevId == EPI_REMOVE_PHOTO || prevId == EPI_ATTACH_FROM_FILE)
			{
				id = EPI_QUESTION_TEXT;
				return pManager->GetStringWrapper()->GetStringText(IDS_QUESTION_TEXT);
			}
		}
		break;
	}
	if (prevId == EPI_BACK)
	{
		return NULL;
	}
	if (prevId == EPI_SAVE_TO_DRAFTS)
	{
		id = EPI_BACK;
		return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
	}
	if (prevId == EPI_SUBMIT)
	{
		id = EPI_SAVE_TO_DRAFTS;
		return pManager->GetStringWrapper()->GetStringText(IDS_SAVE_TO_DRAFTS);
	}
	if (workMsg->GetNumberOfPhotos() > 0)
	{
		id = EPI_SUBMIT;
		return pManager->GetStringWrapper()->GetStringText(IDS_SUBMIT);
	}
	else
	{
		id = EPI_SAVE_TO_DRAFTS;
		return pManager->GetStringWrapper()->GetStringText(IDS_SAVE_TO_DRAFTS);
	}
	return NULL;
}

void ScreenAsk::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(id)
	{
	case EPI_BACK:
		{
			BackToPrevSrceen();
		}
		break;
	case EPI_ATTACH_PHOTOS:
		{
			tabList->SwitchPage(2);
		}
		break;
	case EPI_QUESTION_TEXT:
		{
			tabList->SwitchPage(0);
		}
		break;
	case EPI_QUESTION_OPTIONS:
		{
			tabList->SwitchPage(1);
		}
		break;
	case EPI_TEMPLATES:
		{
			pManager->ChangeWindow(ESN_TEMPLATES, false);
		}
		break;
	case EPI_SAVE_TO_DRAFTS:
		{
			PackMessage();
			*saveMsg = *workMsg;
			saveMsg->SetOwner(pManager->GetDraftsManager());
			pManager->SetWorkingMesage(NULL);
			pManager->ChangeWindow(ESN_MAIN_MENU, true);
		}
		break;
	case EPI_ATTACH_FROM_FILE:
		{
			pManager->ChangeWindow(ESN_ATTACH_FROM_FILE, false);
		}
		break;
	case EPI_ATTACH_FROM_CAMERA:
		{
			pManager->ChangeWindow(ESN_CAMERA, false);
		}
		break;
	case EPI_VIEW:
		{
			pManager->SetWorkingPhoto((LibPhotoItem*)photoList->GetFocusedOwner());
			pManager->ChangeWindow(ESN_VIEW_PHOTO, false);
		}
		break;
	case EPI_REMOVE_PHOTO:
		{
			workMsg->RemovePhoto((LibPhotoItem*)photoList->GetFocusedOwner());
			if (workMsg->GetNumberOfPhotos())
			{
				submitReq->SetParent(NULL);
			}
			else
			{
				submitReq->SetParent((GUIControlContainer*)tabList->GetByID(ECIDS_ASK_ITEM_ID + EMI_PAGE_3));
			}
			photoList->RebuildList();
		}
		break;
	case EPI_SUBMIT:
		{
			PackMessage();
			if (!Utils::WStrLen(workMsg->GetText(ETT_QUESTION_TEXT)))
			{
				pManager->GetAlert()->GetText()->SetText(IDS_QUESTION_ALERT);
				pManager->GetAlert()->Show();
				return;
			}
			if (workMsg->GetInnerType() == EIT_QUESTION_A_OR_B)
			{
				if (!Utils::WStrLen(workMsg->GetText(ETT_QUESTION_VARIANT_A)) || !Utils::WStrLen(workMsg->GetText(ETT_QUESTION_VARIANT_B)))
				{
					pManager->GetAlert()->GetText()->SetText(IDS_VARIANTS_ALERT);
					pManager->GetAlert()->Show();
					return;
				}
			}
			*saveMsg = *workMsg;
			pManager->SetWorkingMesage(saveMsg);
			pManager->ChangeWindow(ESN_SEND_SELECTION, false);
		}
		break;
	}
}


bool ScreenAsk::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_BUTTON_PRESSED:
		{
			int32 id = ((GUIControl*)pData->lParam)->GetID();
			switch(id)
			{
			case ECIDS_ASK_ITEM_ID + EMI_PRIVATE_QUESTION:
				{
					GUIImage *checkImage = (GUIImage*)(((GUIControl*)pData->lParam)->GetByID(ECIDS_ASK_ITEM_ID + EMI_PRIVATE_QUESTION_CHECKBOX));
					int32 frm = checkImage->GetFrame();
					if (frm)
					{
						checkImage->SetFrame(0);
					}
					else
					{
						checkImage->SetFrame(1);
					}

				}
				break;
			}
			return true;
		}
		break;
	case EEI_SELECTOR_CHANGED:
		{
			int32 id = ((GUISelector*)pData->lParam)->GetSelected()->GetID();
			if (id == ECIDS_ASK_ITEM_ID + EMI_TYPE_A_OR_B)
			{
				ShowAB();
			}
			else
			{
				HideAB();
			}
			return true;
		}
		break;
	case EEI_WINDOW_SET_MAIN:
	case EEI_WINDOW_DID_ACTIVATE:
		{
			if (tabList->GetActivePageIndex() == 2)
			{
				photoList->ActivateFocus();
			}
		}
		break;
	case EEI_WINDOW_LOST_MAIN:
	case EEI_WINDOW_WILL_DEACTIVATE:
		{

			if (tabList->GetActivePageIndex() == 2)
			{
				photoList->DeactivateFocus();
			}
			if (eventID == EEI_WINDOW_WILL_DEACTIVATE)
			{
				PackMessage();
			}
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			GetByID(ECIDS_ASK_ITEM_ID + EMI_PAGE_3)->Update();
			SetWorkingMessage(pManager->GetWorkingMessage());
			if (workMsg->GetNumberOfPhotos())
			{
				submitReq->SetParent(NULL);
			}
			else
			{
				submitReq->SetParent((GUIControlContainer*)tabList->GetByID(ECIDS_ASK_ITEM_ID + EMI_PAGE_3));
			}
			GUIListView *c2 = (GUIListView *)tabList->GetByID(ECIDS_ASK_ITEM_ID + EMI_PAGE_2);
			c2->Reset();

			photoList->RebuildList();
		}
		break;
	case EEI_TAB_PAGE_SWITCHED:
		{
			if (tabList->GetActivePageIndex() == 2)
			{
				photoList->ActivateFocus();
			}
			else
			{
				photoList->DeactivateFocus();
			}
		}
	}
	return BaseScreen::OnEvent(eventID, pData);
}

void ScreenAsk::ShowAB()
{
	if (isAB)
	{
		return;
	}
	isAB = true;


	GUIListView *lv = (GUIListView*)GetByID(ECIDS_ASK_ITEM_ID + EMI_PAGE_2);
	lv->InsertChild(bLayout, GetByID(ECIDS_ASK_ITEM_ID + EMI_DURATION_LAYOUT));
	lv->InsertChild(aLayout, bLayout);
	lv->ForceRecalc();
}

void ScreenAsk::HideAB()
{
	if (!isAB)
	{
		return;
	}
	isAB = false;

	aLayout->SetParent(NULL);
	bLayout->SetParent(NULL);
	GUIListView *lv = (GUIListView*)GetByID(ECIDS_ASK_ITEM_ID + EMI_PAGE_2);
	lv->ForceRecalc();
}

//void ScreenAsk::InsertTemplate( const char16 *text )
//{
//	GUIEditMultiString *ems = (GUIEditMultiString *)GetByID(ECIDS_ASK_ITEM_ID + EMI_QUESTION_TEXT);
//	ems->InsertText(text);
//}

void ScreenAsk::SetWorkingMessage( N2FMessage *pMsg )
{
	if (pMsg != workMsg)
	{
		saveMsg = pMsg;
		*workMsg = *saveMsg;
	}
	GUIEditMultiString *ems = (GUIEditMultiString *)GetByID(ECIDS_ASK_ITEM_ID + EMI_QUESTION_TEXT);
	ems->SetText(workMsg->GetText(ETT_QUESTION_TEXT));
	GUIEditText *et = (GUIEditText *)(aLayout->GetByID(ECIDS_ASK_ITEM_ID + EMI_EDIT_TEXT_A));
	et->SetText(workMsg->GetText(ETT_QUESTION_VARIANT_A));
	et = (GUIEditText *)(bLayout->GetByID(ECIDS_ASK_ITEM_ID + EMI_EDIT_TEXT_B));
	et->SetText(workMsg->GetText(ETT_QUESTION_VARIANT_B));
	GUISelector *sel = (GUISelector *)GetByID(ECIDS_ASK_ITEM_ID + EMI_TYPE_SELECTOR);
	sel->SetSelected(sel->GetByID(ECIDS_ASK_ITEM_ID + workMsg->GetInnerType() + EMI_TYPE_FIRST));

	sel = (GUISelector *)GetByID(ECIDS_ASK_ITEM_ID + EMI_DURATION_SELECTOR);
	sel->SetSelected(sel->GetByID(ECIDS_ASK_ITEM_ID + workMsg->GetQuestionDuration() + EMI_DURATION_FIRST));

	GUIImage *i = (GUIImage *)GetByID(ECIDS_ASK_ITEM_ID + EMI_PRIVATE_QUESTION_CHECKBOX);
	i->SetFrame(pMsg->IsPrivate()? 1:0);
	pManager->SetWorkingMesage(workMsg);
}

void ScreenAsk::PackMessage()
{
	GUIEditMultiString *ems = (GUIEditMultiString *)GetByID(ECIDS_ASK_ITEM_ID + EMI_QUESTION_TEXT);
	workMsg->SetText(ems->GetText(), ETT_QUESTION_TEXT);
	GUIEditText *et = (GUIEditText *)(aLayout->GetByID(ECIDS_ASK_ITEM_ID + EMI_EDIT_TEXT_A));
	workMsg->SetText(et->GetText(), ETT_QUESTION_VARIANT_A);
	et = (GUIEditText *)(bLayout->GetByID(ECIDS_ASK_ITEM_ID + EMI_EDIT_TEXT_B));
	workMsg->SetText(et->GetText(), ETT_QUESTION_VARIANT_B);
	GUISelector *sel = (GUISelector *)GetByID(ECIDS_ASK_ITEM_ID + EMI_TYPE_SELECTOR);
	workMsg->SetInnerType( (eInnerType)(sel->GetSelected()->GetID() - ECIDS_ASK_ITEM_ID - EMI_TYPE_FIRST) );
	sel = (GUISelector *)GetByID(ECIDS_ASK_ITEM_ID + EMI_DURATION_SELECTOR);
	workMsg->SetQuestionDuration( (eQuestionDuration)(sel->GetSelected()->GetID() - ECIDS_ASK_ITEM_ID - EMI_DURATION_FIRST) );

	GUIImage *i = (GUIImage *)GetByID(ECIDS_ASK_ITEM_ID + EMI_PRIVATE_QUESTION_CHECKBOX);
	workMsg->SetPrivate(i->GetFrame() == 1);

}

void ScreenAsk::ItemsListOnItem( GUIItemsList *forList, void *owner )
{
	pManager->SetWorkingPhoto((LibPhotoItem*)owner);
	pManager->ChangeWindow(ESN_VIEW_PHOTO, false);
}

GUIControl* ScreenAsk::ItemsListCreateListItem( GUIItemsList *forList )
{
	GUIMenuItem *pItem = new GUIMenuItem(EDT_PHOTO_ITEM);
	GUIPhotoImage *img = new GUIPhotoImage(IDB_PHOTO_LOADING_BAR, IDB_BAD_PHOTO, surfPool, pManager, pItem);
	img->SetID(ECIDS_TEXT_ID + 2);
	GUILayoutBox *lay = new GUILayoutBox(true, pItem);
	GUIText *text = new GUIText((char16*)L" ", lay);
	text->SetID(ECIDS_TEXT_ID);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_PHOTO_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 3, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
	text = new GUIText((char16*)L" ", lay);
	text->SetID(ECIDS_TEXT_ID + 1);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_PHOTO_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 3, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	text->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
	return pItem;


}

bool ScreenAsk::ItemsListIsOwnerValid( GUIItemsList *forList, void *owner )
{
	return pManager->GetWorkingMessage()->HasPhoto((LibPhotoItem*)owner);
}

void *ScreenAsk::ItemsListGetFirstOwner( GUIItemsList *forList )
{
	photoCounter = -1;
	return ItemsListGetNextOwner(forList);
}

void *ScreenAsk::ItemsListGetNextOwner( GUIItemsList *forList )
{
	photoCounter++;
	if (pManager->GetWorkingMessage()->GetNumberOfPhotos() > photoCounter)
	{
		return (void *)pManager->GetWorkingMessage()->GetPhoto(photoCounter);
	}
	return NULL;
}

void ScreenAsk::ItemsListTuneItemByOwner( GUIItemsList *forList, GUIControl *item, void *owner )
{
	GUIText *t = (GUIText*)(item->GetByID(ECIDS_TEXT_ID));
	t->SetText(((LibPhotoItem*)owner)->GetName());

	t = (GUIText*)(item->GetByID(ECIDS_TEXT_ID + 1));
	char16 dateText[100];
	char16 timeMod[3];
	int32 hr = ((LibPhotoItem*)owner)->GetDate()->hour;
	if (hr < 12)
	{
		if (hr == 0)
		{
			hr = 12;
		}
		Utils::WSPrintf(timeMod, 3 * 2, (char16*)L"AM");
	}
	else
	{
		hr = hr - 12;
		if (hr == 0)
		{
			hr = 12;
		}

		Utils::WSPrintf(timeMod, 3 * 2, (char16*)L"AM");
	}
	Utils::WSPrintf(dateText, 100 * 2, pManager->GetStringWrapper()->GetStringText(IDS_DATE_FORMAT)
		, ((LibPhotoItem*)owner)->GetDate()->month
		, ((LibPhotoItem*)owner)->GetDate()->day
		, ((LibPhotoItem*)owner)->GetDate()->year
		, hr
		, ((LibPhotoItem*)owner)->GetDate()->minute
		, timeMod);
	t->SetText(dateText);

	GUIPhotoImage *pi = (GUIPhotoImage*)(item->GetByID(ECIDS_TEXT_ID + 2));
	pi->SetPhotoItem((LibPhotoItem*) owner);
}

void ScreenAsk::ItemsListCorrectItemByOwner( GUIItemsList *forList, GUIControl *item, void *owner )
{
	ItemsListTuneItemByOwner(forList, item, owner);
}

void ScreenAsk::BackToPrevSrceen()
{
	pManager->SetWorkingMesage(NULL);
	BaseScreen::BackToPrevSrceen();
}




