#include "GUIAlert.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "GUIText.h"
#include "Font.h"
#include "GUIData.h"
#include "GUILayoutBox.h"
#include "GUIMenuItem.h"
#include "GUIMultiString.h"
#include "stringres.h"




GUIAlert::GUIAlert()
{
	SetStretch(ECS_STRETCH);
	SetDrawType(EDT_ALERT_BORDER);
	AddListener(EEI_SOFT1_PRESSED, this);
	AddListener(EEI_SOFT2_PRESSED, this);
	AddListener(EEI_CLR_PRESSED, this);
	AddListener(EEI_SELECT_PRESSED, this);
	layout = new GUILayoutBox(true, this);
	//layout->SetChildsAlignType(GUIControl::EA_HCENTRE);
	layout->SetMargins((GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_CENTER_LEFT)->GetWidth() >> 1)/* + 1*/
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_CENTER_RIGHT)->GetWidth() >> 1)/* + 1*/
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_TOP_CENTER)->GetHeight() >> 1) + 1
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1) + GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() / 3);
	text = new GUIMultiString((char16*)L"", layout);
	text->SetAlignType(GUIControl::EA_HCENTRE);
	button = new GUIMenuItem(EDT_SELECTED_ITEM, layout, ControlRect(0, 0, GetApplication()->GetGraphicsSystem()->GetWidth() / 2, 10));
	button->SetAlignType(GUIControl::EA_HCENTRE);
	GUIText *t = new GUIText(IDS_OK, button);

	int32 w = GetApplication()->GetGraphicsSystem()->GetWidth();
	w -= w / 10;
	int32 h = GetApplication()->GetGraphicsSystem()->GetHeight() / 4 * 3;

	targetRect = Rect((GetApplication()->GetGraphicsSystem()->GetWidth() - w) / 2, (GetApplication()->GetGraphicsSystem()->GetHeight() - h) / 2, w, h);
	text->SetControlRect( ControlRect(0, 0
		, w - layout->GetMarginLeft() - layout->GetMarginRight()
		, h - button->GetControlRect().minDy - layout->GetMarginTop() - layout->GetMarginBottom()) );
	text->SetTextAlign(Font::EAP_HCENTER | Font::EAP_VCENTER);
}

GUIAlert::~GUIAlert( void )
{
}

void GUIAlert::Update()
{
	GUIControlContainer::Update();
	switch(state)
	{
	case EAS_RISE:
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
				rc.y = (GetApplication()->GetGraphicsSystem()->GetHeight() - rc.dy) / 2;
				SetControlRect(ControlRect(rc.x, rc.y, rc.dx, rc.dy));
				layout->ForceRecalc();
			}
			else
			{
				state = EAS_PROCESS;
			}
		}
		break;
	case EAS_CLOSING:
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


void GUIAlert::Show()
{
	state = EAS_RISE;


	
	prevActive = GUISystem::Instance()->GetActiveControl();
	GUISystem::Instance()->AddControl(this);
	GUISystem::Instance()->SetActiveControl(this);
	GUISystem::Instance()->SetFocus(button);

	SetControlRect(ControlRect(0, 0
		, GetApplication()->GetGraphicsSystem()->GetWidth(), GetApplication()->GetGraphicsSystem()->GetHeight()));
}

void GUIAlert::Close()
{
	GUISystem::Instance()->RemoveControl(this);
	GUISystem::Instance()->SetActiveControl(prevActive);
}

bool GUIAlert::OnEvent( eEventID eventID, EventData *pData )
{
	if (state == EAS_PROCESS)
	{
		switch(eventID)
		{
		case EEI_CLR_PRESSED:
		case EEI_SOFT2_PRESSED:
		case EEI_SELECT_PRESSED:
		case EEI_SOFT1_PRESSED:
			{
				closeCounter = 6;
				state = EAS_CLOSING;
			}
			break;
		}
	}

	return true;
}

void GUIAlert::Draw()
{
	switch(state)
	{
	case EAS_RISE:
		{
			Rect rc = GetRect();
			if (rc.dy != targetRect.dy)
			{
				GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_ALPHA);
				int32 al = ((rc.dy - targetRect.dy) * 15 / (208 - targetRect.dy));
				GetApplication()->GetGraphicsSystem()->SetAlpha(15 - (uint8)al);
			}
		}
		break;
	case EAS_CLOSING:
		{
			GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_ALPHA);
			GetApplication()->GetGraphicsSystem()->SetAlpha((uint8)(closeCounter * 2));
		}
		break;
	}
	GUIControlContainer::Draw();
}

void GUIAlert::DrawFinished()
{
	GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_NONE);
	GUIControlContainer::DrawFinished();
}

GUIMultiString * GUIAlert::GetText()
{
	return text;
}