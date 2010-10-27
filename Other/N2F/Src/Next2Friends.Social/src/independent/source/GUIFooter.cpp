#include "GUIFooter.h"

#include "Graphics.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "GUIText.h"
#include "GUILayoutBox.h"

#include "GUIData.h"


GUIFooter::GUIFooter()
: GUIControlContainer()
{
	int32 h = GUISystem::Instance()->GetSkin()->GetSprite(EDT_FOOTER_BUTTON_LEFT, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	ControlRect r(0, GetApplication()->GetGraphicsSystem()->GetHeight() - h
		, GetApplication()->GetGraphicsSystem()->GetWidth()
		, h);
	SetControlRect(r);
	SetID(ECIDS_FOOTER_ID);
	SetStretch(ECS_STRETCH);

	GUILayoutBox *layout = new GUILayoutBox(false, this);

	GUIText *text = new GUIText((char16*)L" ", layout);
	text->SetID(ECIDS_FOOTER_ID + 1);
	text->SetDrawType(EDT_FOOTER_BUTTON_LEFT);

	text = new GUIText((char16*)L" ", layout);
	text->SetID(ECIDS_FOOTER_ID + 2);
	text->SetDrawType(EDT_FOOTER_BUTTON_RIGHT);

}

GUIFooter::~GUIFooter()
{

}

bool GUIFooter::OnEvent( eEventID eventID, EventData *pData )
{
	return false;
}

void GUIFooter::SetText( int16 leftStringID, int16 rightStringID )
{
	GUIText *text = (GUIText*)GetByID(ECIDS_FOOTER_ID + 1);
	text->SetText(leftStringID);
	text = (GUIText*)GetByID(ECIDS_FOOTER_ID + 2);
	text->SetText(rightStringID);
}

void GUIFooter::SetText( const char16 * leftString, const char16 * rightString )
{
	GUIText *text = (GUIText*)GetByID(ECIDS_FOOTER_ID + 1);
	text->SetText(leftString);
	text = (GUIText*)GetByID(ECIDS_FOOTER_ID + 2);
	text->SetText(rightString);
}

void GUIFooter::SetPositiveText( int16 leftStringID )
{
	GUIText *text = (GUIText*)GetByID(ECIDS_FOOTER_ID + 1);
	text->SetText(leftStringID);
}

void GUIFooter::SetNegativeText( int16 rightStringID )
{
	GUIText *text = (GUIText*)GetByID(ECIDS_FOOTER_ID + 2);
	text->SetText(rightStringID);
}