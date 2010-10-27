#include "guibutton.h"
#include "GUISystem.h"


GUIButton::GUIButton(GUIControlContainer *parent /*= NULL*/, const ControlRect &rc /*= Rect()*/, bool isVertical/* = false*/)
: GUILayoutBox(isVertical, parent, rc)
{
	selectedDrawType = EDT_NONE;
	unselectedDrawType = EDT_NONE;
	pressedDrawType = EDT_NONE;
	disabledDrawType = EDT_NONE;
	SetSelectable(true);
}

GUIButton::~GUIButton(void)
{

}

void GUIButton::Update()
{
	GUILayoutBox::Update();

	if (GUISystem::Instance()->GetFocus() == this)
	{
		if (GetApplication()->IsKeyDown(Application::EKC_SELECT))
		{
			isPressed = true;
			GUILayoutBox::SetDrawType(pressedDrawType);
		}
		if (GetApplication()->IsKeyUp(Application::EKC_SELECT) && isPressed)
		{
			GUILayoutBox::SetDrawType(selectedDrawType);
			EventData ed;
			ed.lParam = (uint32)this;
			GUISystem::Instance()->SendEventToControl(this, EEI_BUTTON_PRESSED, &ed);
		}
	}
	else
	{
		isPressed = false;
	}
}

void GUIButton::OnSetFocus()
{
	GUILayoutBox::SetDrawType(selectedDrawType);
	GUILayoutBox::OnSetFocus();
}

void GUIButton::OnLostFocus()
{
	GUILayoutBox::SetDrawType(unselectedDrawType);
	GUILayoutBox::OnLostFocus();
}

void GUIButton::SetSelectable( bool selectable )
{
	if (selectable)
	{
		GUILayoutBox::SetDrawType(unselectedDrawType);
	}
	else
	{
		GUILayoutBox::SetDrawType(disabledDrawType);
	}
	GUILayoutBox::SetSelectable(selectable);
}

void GUIButton::SetSelectedDrawType( eDrawType drawType )
{
	selectedDrawType = drawType;
}

void GUIButton::SetUnselectedDrawType( eDrawType drawType )
{
	unselectedDrawType = drawType;
}

void GUIButton::SetPressedDrawType( eDrawType drawType )
{
	pressedDrawType = drawType;
}

void GUIButton::SetDisabledDrawType( eDrawType drawType )
{
	disabledDrawType = drawType;
}