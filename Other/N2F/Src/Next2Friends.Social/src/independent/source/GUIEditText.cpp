#include "GUIEditText.h"
#include "GUISystem.h"
#include "ResourceSystem.h"
#include "GUISkinLocal.h"
#include "Font.h"
#include "Application.h"
#include "GUIData.h"


GUIEditText::GUIEditText(int32 bufSize /*= MAX_SIZE*/, eEditTextType eeType/* = EETT_ALL*/, GUIControlContainer *parent /*= NULL*/, const ControlRect &cr /*= ControlRect()*/)
	:	GUIInputText(eeType)
	,	GUIText		((char16 *)NULL, parent)
	,	xOffset		(0)
{
	unfocusedEnter = false;
 	SetDrawType(EDT_POPUP_BORDER);
	SetSelectable(true);

	pApp			=	GUISystem::Instance()->GetApp();

	SetEditType(eeType);


	SAFE_DELETE_ARRAY(GUIText::textBuffer);
	GUIText::bufferSize		=	bufSize;
	GUIText::textBuffer		=	new char16[bufSize];

	GUIInputText::SetText(GUIText::textBuffer, GUIText::textSize, GUIText::bufferSize);


	SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	//SetTextMargins(ECP_EDITTEXT_MARGIN_TOP, ECP_EDITTEXT_MARGIN_LEFT, ECP_EDITTEXT_MARGIN_BOTTOM, ECP_EDITTEXT_MARGIN_LEFT);
}

GUIEditText::~GUIEditText(void)
{
	SAFE_DELETE_ARRAY(GUIText::textBuffer);
	SAFE_RELEASE(pStr);
}

void GUIEditText::Update()
{
	if(GUISystem::Instance()->GetFocus() != this && !GUIInputText::unfocusedEnter)
		return;

	GUIInputText::Update();

	InvalidateCursor();

	if (textEntered)
	{
		textEntered = false;
		EventData ed;
		ed.lParam = (uint32)this;
		GUISystem::Instance()->SendEventToControl(this, EEI_TEXT_CHANGED, &ed);
	}
}

void GUIEditText::Draw()
{
	//UTILS_TRACE("EditText Draw ID = %d", GetID());
	Rect	rc;
	GetScreenRect(rc);

	if(GUISystem::Instance()->GetFocus() == this || GUIInputText::unfocusedEnter)
	{
		if(textChanged)
		{
			//GUISystem::Instance()->GetSkin()->DrawControl(GetDrawType(), Rect(rc.x + ECP_EDITTEXT_FOCUS_SIZE, rc.y + ECP_EDITTEXT_FOCUS_SIZE, rc.dx - ECP_EDITTEXT_FOCUS_SIZE * 2, rc.dy - ECP_EDITTEXT_FOCUS_SIZE * 2));
			GUISystem::Instance()->GetSkin()->DrawControl(GetDrawType(), rc);
			DrawText();

			textChanged	=	false;
		}

		if (!GUIInputText::unfocusedEnter)
		{
			//GUISystem::Instance()->GetSkin()->DrawControl(EDT_EDITLINE_FOCUS, rc);
		}
		if (showCursor)
		{
			int32	y	=	rc.y;

			pApp->GetGraphicsSystem()->SetColor(0,0,0);
			pApp->GetGraphicsSystem()->DrawRect(rc.x + cursorX - xOffset, rc.y + cursorY + marginTop, cursorWidth, pFont->GetHeight() - marginTop - marginBottom);
		}

	}
	else
	{
		GUISystem::Instance()->GetSkin()->DrawControl(GetDrawType(), rc);
		//GUISystem::Instance()->GetSkin()->DrawControl(GetDrawType(), Rect(rc.x + ECP_EDITTEXT_FOCUS_SIZE, rc.y + ECP_EDITTEXT_FOCUS_SIZE, rc.dx - ECP_EDITTEXT_FOCUS_SIZE * 2, rc.dy - ECP_EDITTEXT_FOCUS_SIZE * 2));
		DrawText();
	}
}

bool GUIEditText::SetText(const char16* _text)
{
	if(!_text)	return false;

	int32 strSize	=	Utils::WStrLen(_text);
	
	FASSERT(strSize	< GUIText::bufferSize);

	Utils::WStrCpy(GUIText::textBuffer, _text);
	GUIText::textSize		=	strSize;

	GUIInputText::SetText(GUIText::textBuffer, GUIText::textSize, GUIText::bufferSize);

	CalcCursorXY();

	return true;
}

bool GUIEditText::SetText(const uint16 _resID)
{
	SAFE_RELEASE(pStr);
	pStr = GUISystem::Instance()->GetStringSystem()->CreateString(_resID);
	
	return SetText(pStr->GetString());
}

void GUIEditText::OnSetFocus()
{
// 	UTILS_LOG(EDMP_DEBUG, "GUIEditText::OnSetFocus: start");

	GUISystem::Instance()->AddListener(EEI_CHAR, this);


	GUIText::SetDrawType(focusedDrawType);

	GUISystem::Instance()->LockKeyboard();

	GUIText::OnSetFocus();
	GUIInputText::OnSetFocus();

	CalcCursorXY();

// 	UTILS_LOG(EDMP_DEBUG, "GUIEditText::OnSetFocus: end");
}

void GUIEditText::OnLostFocus()
{
//	UTILS_LOG(EDMP_DEBUG, "GUIEditText::OnLostFocus: start");

	GUISystem::Instance()->RemoveListener(EEI_CHAR, this);

	GUIText::SetDrawType(unfocusedDrawType);

	GUISystem::Instance()->UnlockKeyboard();

	GUIText::OnLostFocus();
	GUIInputText::OnLostFocus();

	Invalidate();

	xOffset		=	0;

//	UTILS_LOG(EDMP_DEBUG, "GUIEditText::OnLostFocus: end");
}

void GUIEditText::CalcCursorXY()
{
	InvalidateCursor();

	int32 strWidth	=	0;
	int32 width		=	0;

	if(GUIInputText::textBuffer)
	{
		strWidth	=	pFont->GetStringWidth(GUIInputText::textBuffer);
		width		=	pFont->GetStringWidth(GUIInputText::textBuffer, currentPos);

		Rect rc = GetRect();
		

		int32 drawedWidth = pFont->GetStringWidth(	GUIInputText::textBuffer, 
													currentPos + GUIInputText::isKeyDown);

		int32 rectWidth = rc.dx - marginLeft - marginRight;


		int32 oldOfset = xOffset;
 		if(drawedWidth > rectWidth && drawedWidth && rectWidth > 0)
		{
			xOffset	=	drawedWidth - rectWidth;
		}
		else 
		{
			xOffset = 0;
		}

		if(oldOfset != xOffset)
			Invalidate();
	}

	if(align & Font::EAP_RIGHT)
	{
		cursorX		=	dx - strWidth + width;
	}
	else if(align & Font::EAP_HCENTER)
	{
		cursorX		=	dx - (strWidth >> 1) + width;
	}
	else
	{
		cursorX		=	dx + width;
	}

	if(align & Font::EAP_BOTTOM)
	{
		cursorY		=	dy - pFont->GetHeight();	
	}
	else if(align & Font::EAP_VCENTER)
	{
		cursorY		=	dy - (pFont->GetHeight()>>1);	
	}
	else
	{
		cursorY		=	dy;
	}
}

void GUIEditText::SetTextAlign(uint32 _align)
{
	GUIText::SetTextAlign(_align);

	CalcCursorXY();
}

void GUIEditText::SetDrawType(eDrawType newType)
{
	unfocusedDrawType = newType;
	if (focusedDrawType == EDT_DEFAULT)
	{
		focusedDrawType = unfocusedDrawType;
	}
	GUIText::SetDrawType(newType);
	ControlRect cr = GetControlRect();
	SetControlRect(ControlRect(cr.x, cr.y
		, cr.minDx
		, pFont->GetHeight() + marginTop + marginBottom
		, cr.maxDx
		, pFont->GetHeight() + marginTop + marginBottom));
	CalcCursorXY();

	Invalidate();
}

bool GUIEditText::AddChar(char16 ch)
{
	bool ret = GUIInputText::AddChar(ch);

	if(ret)
	{
		UpdateTextPos();
	}

	return	ret;
}

void GUIEditText::UpdateTextPos()
{
	GUIText::UpdateTextPos();

	CalcCursorXY();
}

void GUIEditText::SetSelectable( bool selectable )
{
	GUIText::SetSelectable(selectable);
	if (selectable)
	{
		//SetDrawType(EDT_EDITLINE);
	}
	else
	{
		//SetDrawType(EDT_EDITLINE_ACTIVE);
	}
}

void GUIEditText::SetRect(const Rect &rc)
{
	GUIText::SetRect(rc);
	//UTILS_TRACE("EditText SetRect");
	textChanged	=	true;
}

void GUIEditText::SetControlRect(const ControlRect &cr)
{
	GUIText::SetControlRect(cr);
	//UTILS_TRACE("EditText SetControlRect");
	textChanged	=	true;
}

void GUIEditText::Invalidate()
{
	GUIText::Invalidate();
	//UTILS_TRACE("EditText Invalidate");

	textChanged	=	true;
}

void GUIEditText::InvalidateCursor()
{
	Rect rc;
	GetScreenRect(rc);

	rc.x	+=	cursorX - xOffset;
	rc.y	+=	cursorY;
	rc.dx	=	cursorWidth;
	rc.dy	=	pFont->GetHeight();

	//UTILS_TRACE("EditText InvalidateCursor");
	textChanged	=	true;
	GUISystem::Instance()->InvalidateRect(rc, this);
}

void GUIEditText::ChangeChar(char16 ch)
{
	GUIInputText::ChangeChar(ch);

	CalcCursorXY();

	Invalidate();
}

void GUIEditText::DeleteSymbol()
{
	GUIInputText::DeleteSymbol();

	Invalidate();
}

void GUIEditText::DrawText()
{
	if (GUIInputText::textBuffer)
	{
		Rect	rc;
		GetScreenRect(rc);

		GraphicsSystem * graphSyst = GetApplication()->GetGraphicsSystem();
		Rect rc2;
		graphSyst->GetClip(&rc2.x, &rc2.y, &rc2.dx, &rc2.dy);

		graphSyst->SetClipIntersect(rc.x + marginLeft, rc.y + marginTop, 
									rc.dx - marginLeft - marginRight, 
									rc.dy - marginTop - marginBottom);

		pFont->SetCurrentPalette(fontPlt);
		pFont->DrawString(GUIInputText::textBuffer, rc.x + dx - xOffset, rc.y + dy, -1, align);

		graphSyst->SetClip(rc2.x, rc2.y, rc2.dx, rc2.dy);
	}
}

void GUIEditText::UnfocusedEnter( bool enterWhenUnfocused )
{
	GUIInputText::unfocusedEnter = enterWhenUnfocused;
	if (enterWhenUnfocused)
	{
		GUISystem::Instance()->LockKeyboard();
		//SetDrawType(EDT_EDITLINE);
		GUIInputText::OnSetFocus();
		CalcCursorXY();
	}
	else
	{
		GUISystem::Instance()->UnlockKeyboard();
		GUIInputText::OnLostFocus();
		//SetDrawType(EDT_EDITLINE_ACTIVE);
	}
}

void GUIEditText::SetTextMargins( int32 top, int32 left, int32 bottom, int32 right )
{
	GUIText::SetTextMargins(top, left, bottom, right);
	SetDrawType(GetDrawType());
}

void GUIEditText::SetFocusedDrawType( eDrawType drawType )
{
	focusedDrawType = drawType;
}

bool GUIEditText::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_CHAR:
		{
			GUIInputText::OnChar((char16)pData->wParam);
			return true;
		}
	}
	return false;
}