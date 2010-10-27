#include "GUIMenuItem.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "VList.h"

GUIMenuItem::GUIMenuItem( eDrawType selDrawType, GUIControlContainer *parent /*= NULL*/, const ControlRect &rc /*= ControlRect()*/, bool isVertical/* = false*/)
: GUIButton(parent, rc, isVertical)
{
	SetSelectedDrawType(selDrawType);
	SetPressedDrawType(selDrawType);
	ControlRect r = GetControlRect();
	r.minDy = r.maxDy = GUISystem::Instance()->GetSkin()->GetSprite(selectedDrawType, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	SetControlRect(r);
}

GUIMenuItem::~GUIMenuItem( void )
{

}

void GUIMenuItem::OnSetFocus()
{
	GUIButton::OnSetFocus();
	if (selectedDrawType == EDT_SELECTED_ITEM)
	{
		VList *ch = GetChilds();
		for (VList::Iterator it = ch->Begin(); it != ch->End(); it++)
		{
			if (((GUIControl*)(*it))->GetDrawType() == EDT_UNSELECTED_TEXT)
			{
				((GUIControl*)(*it))->SetDrawType(EDT_SELECTED_TEXT);
			}
			if (((GUIControl*)(*it))->GetDrawType() == EDT_POPUP_UNSELECTED_TEXT)
			{
				((GUIControl*)(*it))->SetDrawType(EDT_POPUP_SELECTED_TEXT);
			}
		}
	}
	else
	{
		VList *ch = GetChilds();
		VList::Iterator it = ch->Begin();
		it++;
		ch = ((GUIControlContainer*)(*it))->GetChilds();
		for (it = ch->Begin(); it != ch->End(); it++)
		{
			((GUIControl*)(*it))->SetDrawType(EDT_POPUP_SELECTED_TEXT);
		}
	}
}

void GUIMenuItem::OnLostFocus()
{
	GUIButton::OnLostFocus();
	if (selectedDrawType == EDT_SELECTED_ITEM)
	{
		VList *ch = GetChilds();
		for (VList::Iterator it = ch->Begin(); it != ch->End(); it++)
		{
			if (((GUIControl*)(*it))->GetDrawType() == EDT_SELECTED_TEXT)
			{
				((GUIControl*)(*it))->SetDrawType(EDT_UNSELECTED_TEXT);
			}
			if (((GUIControl*)(*it))->GetDrawType() == EDT_POPUP_SELECTED_TEXT)
			{
				((GUIControl*)(*it))->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
			}
		}
	}
	else
	{
		VList *ch = GetChilds();
		VList::Iterator it = ch->Begin();
		it++;
		ch = ((GUIControlContainer*)(*it))->GetChilds();
		for (it = ch->Begin(); it != ch->End(); it++)
		{
			((GUIControl*)(*it))->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
		}
	}
}

void GUIMenuItem::Draw()
{
	GUIButton::Draw();
}