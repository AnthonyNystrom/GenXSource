#include "GUIPopUpItem.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "VList.h"

GUIPopUpItem::GUIPopUpItem( GUIControlContainer *parent /*= NULL*/)
: GUIButton(parent)
{
	SetSelectedDrawType(EDT_POPUP_ITEM);
	SetPressedDrawType(EDT_POPUP_ITEM);
	ControlRect r = GetControlRect();
	r.minDy = r.maxDy = GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	SetControlRect(r);
}

GUIPopUpItem::~GUIPopUpItem( void )
{

}

void GUIPopUpItem::OnSetFocus()
{
	GUIButton::OnSetFocus();
	VList *ch = GetChilds();
	for (VList::Iterator it = ch->Begin(); it != ch->End(); it++)
	{
		((GUIControl*)(*it))->SetDrawType(EDT_POPUP_SELECTED_TEXT);
	}
}

void GUIPopUpItem::OnLostFocus()
{
	GUIButton::OnLostFocus();
	VList *ch = GetChilds();
	for (VList::Iterator it = ch->Begin(); it != ch->End(); it++)
	{
		((GUIControl*)(*it))->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
	}
}

void GUIPopUpItem::Draw()
{
	GUIButton::Draw();	
}