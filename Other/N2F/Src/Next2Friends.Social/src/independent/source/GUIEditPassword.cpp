#include "GUIEditPassword.h"
#include "GUISystem.h"
#include "ResourceSystem.h"
#include "GUISkinLocal.h"
#include "Font.h"
#include "Application.h"


GUIEditPassword::GUIEditPassword(int32 bufSize /*= MAX_SIZE*/, GUIControlContainer *parent /*= NULL*/, const ControlRect &cr /*= ControlRect()*/)
	:	GUIEditText		(bufSize, EETT_ALL, parent, cr)
{
	SAFE_DELETE_ARRAY(passwordBuffer);
	passwordBuffer		=	new char16 [bufSize]; 
}

GUIEditPassword::~GUIEditPassword(void)
{
	SAFE_DELETE_ARRAY(passwordBuffer);
}


void GUIEditPassword::DrawText()
{
	if (passwordBuffer)
	{
		Rect rc;
		GetScreenRect(rc);
		pFont->SetCurrentPalette(fontPlt);
		pFont->DrawString(passwordBuffer, rc.x + dx - xOffset, rc.y + dy, -1, align);
	}
}

void GUIEditPassword::CalcCursorXY()
{
	InvalidateCursor();

	CreatePasswordText();

	int32 strWidth	=	0;
	int32 width		=	0;

	if(passwordBuffer)
	{
		strWidth	=	pFont->GetStringWidth(passwordBuffer);
		width		=	pFont->GetStringWidth(passwordBuffer, currentPos);
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

void GUIEditPassword::CreatePasswordText()
{
	//UTILS_TRACE("currentPos = %d", currentPos);
	int32 iChar = 0;
	for(; iChar < currentPos; ++iChar)
	{
		passwordBuffer[iChar]	=	SYMBOL('*');
	}

	//UTILS_TRACE("GUIInputText::textSize = %d", GUIInputText::textSize);
	for(; iChar < GUIInputText::textSize; ++iChar)
	{
		passwordBuffer[iChar]	=	GUIInputText::textBuffer[iChar];
	}
	
	passwordBuffer[iChar]	=	0;
	Invalidate();
}

void GUIEditPassword::ChangeChar(char16 ch)
{
	GUIEditText::ChangeChar(ch);

	CreatePasswordText();

}