#include "GUIHeader.h"

#include "Graphics.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "GUIText.h"
#include "GUIImage.h"
#include "GUILayoutBox.h"

#include "GUIData.h"

#include "GUICapsIndicator.h"
#include "stringres.h"
#include "graphres.h"


GUIHeader::GUIHeader()
: GUIControlContainer()
{
	SetDrawType(EDT_HEADER);
	ControlRect r(0, 0
		, GetApplication()->GetGraphicsSystem()->GetWidth()
		, GUISystem::Instance()->GetSkin()->GetSprite(EDT_HEADER, GUISkinLocal::ESSH3_CENTER)->GetHeight());
	SetControlRect(r);
	SetID(ECIDS_HEADER_ID);
	SetStretch(ECS_STRETCH);

	GUIImage *img = new GUIImage(IDB_LOGO, this);

	caps = new GUICapsIndicator();
	caps->SetDrawType(EDT_NONE);
	caps->SetTextDrawType(EDT_CAPS_TEXT);
	caps->SetDelegate(this);
	caps->SetStringID(ECM_FIRST_BIG, IDS_CAPS_INDICATOR_FIRST_BIG);
	caps->SetStringID(ECM_BIG, IDS_CAPS_INDICATOR_BIG);
	caps->SetStringID(ECM_SMALL, IDS_CAPS_INDICATOR_SMALL);
	caps->SetStringID(ECM_DIGITS, IDS_CAPS_INDICATOR_DIGITS);
	GUISystem::Instance()->SetIndicator(GUIIndicator::EIT_CAPS, caps);


	GUILayoutBox *lay = new GUILayoutBox(false, this);
	lay->SetAlignType(GUIControl::EA_RIGHT | GUIControl::EA_VCENTRE);
	indicators[GUIHeader::ENI_OUTBOX] = new GUIImage(IDB_OUTBOX_INDICATOR, lay);
	indicators[GUIHeader::ENI_INBOX] = new GUIImage(IDB_INBOX_INDICATOR, lay);
	indicators[GUIHeader::ENI_DASHBOARD] = new GUIImage(IDB_DASHBOARD_INDICATOR, lay);
	int32 w = 0;
	for (int i = 0; i < (int)GUIHeader::ENI_COUNT; i++)
	{
		indicators[i]->SetAnimationRange(0, indicators[i]->GetFrameCount() - 1);
		indicators[i]->SetControlRect(ControlRect(0, 0, indicators[i]->GetImageWidth() - 1, ControlRect::MIN_D, indicators[i]->GetImageWidth() - 1, ControlRect::MAX_D));
		w += indicators[i]->GetImageWidth() - 1;
	}
	lay->SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));

}

GUIHeader::~GUIHeader()
{
	caps->SetParent(NULL);
	SAFE_DELETE(caps);
}

bool GUIHeader::OnEvent( eEventID eventID, EventData *pData )
{
	return false;
}

//void GUIHeader::SetText( int16 stringID )
//{
//	GUIText *text = (GUIText*)GetByID(ECIDS_HEADER_ID + 1);
//	text->SetText(stringID);
//}
//
//void GUIHeader::SetText( const char16 * newString )
//{
//	GUIText *text = (GUIText*)GetByID(ECIDS_HEADER_ID + 1);
//	text->SetText(newString);
//}

void GUIHeader::IndicatorMustShow( GUIIndicator *indicator )
{
	caps->SetParent(this);
	caps->SetAlignType(GUIControl::EA_VCENTRE | GUIControl::EA_LEFT);
}

void GUIHeader::IndicatorMustHide( GUIIndicator *indicator )
{
	caps->SetParent(NULL);
}

void GUIHeader::Draw()
{
	GUIControlContainer::Draw();
}

void GUIHeader::ShowNetInidicator( eNetIndicator indicator, bool show )
{
	if (show)
	{
		indicators[indicator]->StartAnimation();
	}
	else
	{
		indicators[indicator]->StopAnimation();
		indicators[indicator]->SetFrame(0);
	}
}


