#include "guipopup.h"
#include "GUISystem.h"
#include "PopUpDelegate.h"
#include "GUISkinLocal.h"
#include "GUIText.h"
#include "GUIPopUpItem.h"
#include "Font.h"
#include "GUIData.h"
#include "GUIListView.h"


GUIPopUp::GUIPopUp( int32 baseLine)
{
	SetStretch(ECS_STRETCH);
	popUpDelegate = NULL;
	SetDrawType(EDT_POPUP_BORDER);
	AddListener(EEI_SOFT1_PRESSED, this);
	AddListener(EEI_SOFT2_PRESSED, this);
	AddListener(EEI_CLR_PRESSED, this);
	AddListener(EEI_SELECT_PRESSED, this);
	list = new GUIListView(this);
	yLine = baseLine;
	targetRect = Rect(2, yLine - 120, 120, 120);
	list->SetPrescroll(GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() * 2);
	list->SetMargins((GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_CENTER_LEFT)->GetWidth() >> 1)/* + 1*/
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_CENTER_RIGHT)->GetWidth() >> 1)/* + 1*/
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_TOP_CENTER)->GetHeight() >> 1) + 1
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1) + 1);
}

GUIPopUp::~GUIPopUp( void )
{
	list->RemoveAllItems();
	while (itemsList.Size())
	{
		GUIControl *c = (GUIControl*)(*itemsList.Begin());
		SAFE_DELETE(c);
		itemsList.Erase(itemsList.Begin());
	}
}

void GUIPopUp::Update()
{
	GUIControlContainer::Update();
	switch(state)
	{
	case EPS_RISE:
		{
			Rect rc = GetRect();
			if (rc.dy != targetRect.dy)
			{
				int32 spd = (targetRect.dy - rc.dy) / 3;
				if (targetRect.dy > rc.dy)
				{
					spd++;
					rc.dy += spd;
					if (targetRect.dy < rc.dy)
					{
						rc.dy = targetRect.dy;
					}
				}
				else
				{
					spd--;
					rc.dy += spd;
					if (targetRect.dy > rc.dy)
					{
						rc.dy = targetRect.dy;
					}
				}
				rc.y = yLine - rc.dy;
				SetControlRect(ControlRect(rc.x, rc.y, rc.dx, rc.dy));
			}
			else
			{
				state = EPS_PROCESS;
			}
		}
		break;
	case EPS_CLOSING:
		{
			closeCounter--;
			if (!closeCounter)
			{
				Close();
				return;
			}
			Invalidate();
		}
		break;
	}
}

void GUIPopUp::SetDelegate( PopUpDelegate *newDelegate )
{
	popUpDelegate = newDelegate;
}

void GUIPopUp::Show()
{
	if (popUpDelegate)
	{
		if (!popUpDelegate->PopUpShouldOpen())
		{
			return;
		}
	}
	state = EPS_RISE;
	selectedItem = -1;

	int32 h = GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	int32 leftMargin = h >> 1;
	int32 num = 0;


	
	prevActive = GUISystem::Instance()->GetActiveControl();
	GUISystem::Instance()->AddControl(this);
	GUISystem::Instance()->SetActiveControl(this);

	list->RemoveAllItems();
	list->Reset();

	int32 w = 0;
	VList::Iterator it = itemsList.Begin();
	int32 popupID;
	int32 prevID = -1;
	const char16 *text = NULL;
	while(true)
	{
		popupID = -1;
		if (popUpDelegate)
		{
			text = popUpDelegate->PopUpGetTextByIndex(num, popupID, prevID);
			prevID = popupID;
			if (!text)
			{
				break;
			}
		}
		GUIPopUpItem *c;
		if(itemsList.Size() < num + 1)
		{
			GUIPopUpItem *itm = new GUIPopUpItem();
			//itm->AddListener(EEI_BUTTON_PRESSED, this);
			GUIText *tx = new GUIText((char16*)L"", itm);
			tx->SetID(ECIDS_POPUP_ITEM_TEXT_ID);
			tx->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
			tx->SetMargins(leftMargin, 0, 0, 0);
			tx->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
			itemsList.PushBack(itm);
			c = itm;
		}
		else
		{
			c = (GUIPopUpItem*)(*it);
		}

		c->SetID(ECIDS_POPUP_ITEM_ID + num);
		list->AddChild(c);
		GUIText *t = (GUIText*)c->GetByID(ECIDS_POPUP_ITEM_TEXT_ID);
		t->SetParent(NULL);
		t->SetParent(c);

		t->SetText(text);
		c->SetOuterID(popupID);

		int32 tw = t->GetTextWidth() + (leftMargin << 1);
		if (tw > w)
		{
			w = tw;
		}
		if (num == 0)
		{
			GUISystem::Instance()->SetFocus(c);
		}
		num++;
		if(itemsList.Size() > num)
		{
			it++;
		}
	}
	w += list->GetMarginLeft() + 2;
	w += list->GetMarginRight() + 2;
	if (w < GetApplication()->GetGraphicsSystem()->GetWidth() / 3)
	{
		w = GetApplication()->GetGraphicsSystem()->GetWidth() / 3;
	}
	if (w > (GetApplication()->GetGraphicsSystem()->GetWidth() >> 2) * 3 + list->GetMarginLeft() + list->GetMarginRight())
	{
		w = (GetApplication()->GetGraphicsSystem()->GetWidth() >> 2) * 3 + list->GetMarginLeft() + list->GetMarginRight();
	}

	h *= num;
	h += list->GetMarginBottom();
	h += list->GetMarginTop();
	if (h > GetApplication()->GetGraphicsSystem()->GetHeight() / 3 *2)
	{
		h = GetApplication()->GetGraphicsSystem()->GetHeight() / 3 * 2;
	}

	targetRect = Rect(1, yLine - h, w, h);
	SetControlRect(ControlRect(targetRect.x, yLine, w, list->GetMarginBottom() + list->GetMarginTop()));
}

void GUIPopUp::Close()
{
	GUISystem::Instance()->RemoveControl(this);
	GUISystem::Instance()->SetActiveControl(prevActive);

	if (selectedItem != -1)
	{
		if (popUpDelegate)
		{
			GUIPopUpItem *c = (GUIPopUpItem *)GetByID(selectedItem);
			popUpDelegate->PopUpOnItemSelected(selectedItem - ECIDS_POPUP_ITEM_ID, c->GetOuterID());
		}
	}
}

bool GUIPopUp::OnEvent( eEventID eventID, EventData *pData )
{
	if (state == EPS_PROCESS)
	{
		switch(eventID)
		{
		case EEI_CLR_PRESSED:
		case EEI_SOFT2_PRESSED:
			closeCounter = 6;
			state = EPS_CLOSING;

			break;
		case EEI_SELECT_PRESSED:
		case EEI_SOFT1_PRESSED:
			selectedItem = GUISystem::Instance()->GetFocus()->GetID();
			closeCounter = 6;
			state = EPS_CLOSING;

			break;
		}
	}

	return true;
}

void GUIPopUp::Draw()
{
	switch(state)
	{
	case EPS_RISE:
		{
			Rect rc = GetRect();
			GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_ALPHA);
			GetApplication()->GetGraphicsSystem()->SetAlpha((uint8)((rc.dy) * 15 / targetRect.dy));
		}
		break;
	case EPS_CLOSING:
		{
			GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_ALPHA);
			GetApplication()->GetGraphicsSystem()->SetAlpha((uint8)(closeCounter * 2));
		}
		break;
	}
	GUIControlContainer::Draw();
}

void GUIPopUp::DrawFinished()
{
	GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_NONE);
	GUIControlContainer::DrawFinished();
}