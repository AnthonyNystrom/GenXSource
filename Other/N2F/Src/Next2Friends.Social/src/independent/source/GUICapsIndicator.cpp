#include "GUICapsIndicator.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "VList.h"
#include "GUIText.h"
#include "Font.h"


GUICapsIndicator::GUICapsIndicator( GUIControlContainer *parent /*= NULL*/, const ControlRect &cr/* = ControlRect()*/ )
: GUIIndicator(parent, cr)
{
	SetStretch(/*eChildStretch::*/ECS_STRETCH);
	type = EIT_CAPS;

	text = new GUIText((char16*)L"", this);
	currentMode = ECM_FIRST_BIG;

	SetTextDrawType(text->GetDrawType());
}

GUICapsIndicator::~GUICapsIndicator( void )
{

}


void GUICapsIndicator::SetStringID( eCapsMode mode, int16 stringID )
{
	modeStringID[mode] = stringID;
	if (mode == currentMode)
	{
		text->SetText(modeStringID[currentMode]);
	}
	if (mode == ECM_FIRST_BIG)
	{
		ControlRect r = GetControlRect();
		text->SetText(modeStringID[mode]);
		r.maxDx = r.minDx = text->GetTextWidth() * 3 / 2;
		text->SetText(modeStringID[currentMode]);
		SetControlRect(r);
	}
}

void GUICapsIndicator::SetMode( eCapsMode mode )
{
	currentMode = mode;
	if (modeStringID[currentMode])
	{
		text->SetText(modeStringID[currentMode]);
	}
}

void GUICapsIndicator::SetTextDrawType( eDrawType newDrawType )
{
	text->SetDrawType(newDrawType);
	ControlRect r = GetControlRect();
	if (modeStringID[ECM_FIRST_BIG])
	{
		text->SetText(modeStringID[ECM_FIRST_BIG]);
		r.maxDx = r.minDx = text->GetTextWidth() + text->GetTextWidth() / 4;
		text->SetText(modeStringID[currentMode]);
	}
	uint8 pal;
	Font *pFont = GUISystem::Instance()->GetSkin()->GetFont(newDrawType, pal);
	r.maxDy = r.minDy = pFont->GetHeight() + pFont->GetHeight() / 5;
	SetControlRect(r);
}

void GUICapsIndicator::SetNextMode()
{
    currentMode = (eCapsMode)(currentMode + 1);
	if (currentMode == ECM_COUNT)
	{
		currentMode = ECM_FIRST_BIG;
	}
	SetMode(currentMode);
}


eCapsMode GUICapsIndicator::GetMode()
{
	return currentMode;
}




