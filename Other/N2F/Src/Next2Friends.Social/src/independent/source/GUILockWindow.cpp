#include "GUILockWindow.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "GUIText.h"
#include "Font.h"
#include "GUIData.h"
#include "GUIImage.h"
#include "GUILayoutBox.h"
#include "GUIMenuItem.h"
#include "GUIMultiString.h"
#include "stringres.h"
#include "graphres.h"




GUILockWindow::GUILockWindow()
{
	SetStretch(ECS_STRETCH);
	SetDrawType(EDT_ALERT_BORDER);
	layout = new GUILayoutBox(true, this);
	layout->SetMargins((GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_CENTER_LEFT)->GetWidth() >> 1)/* + 1*/
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_CENTER_RIGHT)->GetWidth() >> 1)/* + 1*/
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_TOP_CENTER)->GetHeight() >> 1) + 1
		, (GUISystem::Instance()->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1) + GUISystem::Instance()->GetSkin()->GetSprite(EDT_SELECTED_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() / 3);

	GUIImage *img = new GUIImage(IDB_PHOTO_LOADING_BAR, layout);
	img->SetAnimationRange(0, img->GetFrameCount() - 1);
	img->StartAnimation();
	img->SetControlRect( ControlRect(0, 0
		, GetApplication()->GetGraphicsSystem()->GetWidth() - layout->GetMarginLeft() - layout->GetMarginRight()
		, GetApplication()->GetGraphicsSystem()->GetHeight() / 2 - layout->GetMarginTop() - layout->GetMarginBottom()) );

	text = new GUIMultiString((char16*)L"", layout);
	//text->SetAlignType(GUIControl::EA_HCENTRE);
	SetControlRect(ControlRect(0, 0, GetApplication()->GetGraphicsSystem()->GetWidth(), GetApplication()->GetGraphicsSystem()->GetHeight()));
	text->SetControlRect( ControlRect(0, 0
		, GetApplication()->GetGraphicsSystem()->GetWidth() - layout->GetMarginLeft() - layout->GetMarginRight()
		, GetApplication()->GetGraphicsSystem()->GetHeight() / 2 - layout->GetMarginTop() - layout->GetMarginBottom()) );
	text->SetTextAlign(Font::EAP_HCENTER | Font::EAP_TOP);
}

GUILockWindow::~GUILockWindow( void )
{
}



void GUILockWindow::Show()
{
	if (closeCounter)
	{
		return;
	}
	prevActive = GUISystem::Instance()->GetActiveControl();
	GUISystem::Instance()->AddControl(this);
	GUISystem::Instance()->SetActiveControl(this);
	GUISystem::Instance()->SetFocus(NULL);
	GUISystem::Instance()->LockKeyboard();
	openCounter = 14;
}

void GUILockWindow::Hide()
{
	if (closeCounter)
	{
		return;
	}
	closeCounter = 15;
	GUISystem::Instance()->SetActiveControl(prevActive);
	GUISystem::Instance()->UnlockKeyboard();
}



GUIMultiString * GUILockWindow::GetText()
{
	return text;
}

void GUILockWindow::Close()
{
	GUISystem::Instance()->RemoveControl(this);
	GUISystem::Instance()->ForceInvalidate();
}

void GUILockWindow::Update()
{
	GUIControlContainer::Update();
	if (closeCounter)
	{
		closeCounter -= 2;
		Invalidate();
		if (closeCounter <= 0)
		{
			closeCounter = 0;
			Close();
		}
		return;
	}
	if (GetApplication()->IsKeyPressed(Application::EKC_CLR))
	{
		Hide();
		GUISystem::Instance()->SendEventToAllActive(EEI_LOCK_WINDOW_CANCELED, NULL);
	}
	if (openCounter)
	{
		openCounter -= 2;
		Invalidate();
		if (openCounter < 0)
		{
			openCounter = 0;
		}
	}
}

void GUILockWindow::Draw()
{
	int32 counter = 0;
	if (openCounter)
	{
		counter = 15 - openCounter;
	}
	if (closeCounter)
	{
		counter = closeCounter;
	}
	if (counter)
	{
		GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_ALPHA);
		GetApplication()->GetGraphicsSystem()->SetAlpha((uint8)(counter));
	}
	GUIControlContainer::Draw();
}

void GUILockWindow::DrawFinished()
{
	if (closeCounter || openCounter)
	{
		GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_NONE);
	}
	GUIControlContainer::DrawFinished();
}




