#include "guitext.h"
#include "GUISystem.h"
#include "ResourceSystem.h"
#include "GUISkinLocal.h"
#include "Font.h"
#include "TextBalancer.h"


GUIText::GUIText(const int16 resourseID, GUIControlContainer *parent /* = NULL */, const ControlRect &cr /* = ControlRect */)
	:	GUIControl	(parent, cr)
	,	textSize	(0)
	,	pStr		(NULL)
	,	isChange	(false)
	,	isString	(false)
{
	SetText(resourseID);
	SetDrawType(EDT_TEXT);
	SetTextAlign(Font::EAP_HCENTER | Font::EAP_VCENTER);
}

GUIText::GUIText(const char16 *text /* = NULL */, GUIControlContainer *parent /* = NULL */,
				 const ControlRect &cr /* = ControlRect */, bool _isChange /* = false */)
	:	GUIControl	(parent, cr)
	,	textSize	(0)
	,	pStr		(NULL)
	,	isChange	(_isChange)
	,	isString	(false)
{
	SetText(text);
	SetDrawType(EDT_TEXT);
	SetTextAlign(Font::EAP_HCENTER | Font::EAP_VCENTER);
}

GUIText::~GUIText(void)
{
	if (isString)
	{
		SAFE_RELEASE(pStr);
	}
	else
	{
		SAFE_DELETE_ARRAY(textBuffer);
	}
	SAFE_DELETE(textBalancer);
}

void GUIText::Update()
{
	if (textWidth > GetRect().dx)
	{
		if (!textBalancer)
		{
			textBalancer = new TextBalancer(this);
		}
		textBalancer->Update();	
	}
}

void GUIText::Draw()
{
	GUIControl::Draw();
	
	DrawText();
}

void GUIText::SetTextAlign(uint32 _align)
{
	align = _align;

	UpdateTextPos();
}

uint32 GUIText::GetTextAlign()
{
	return align;
}

bool GUIText::SetText(const char16* _text)
{
	if (!_text)
	{
		return false;
	}
	if (isString)
	{
		SAFE_RELEASE(pStr);
		
		bufferSize	=	0;
		textBuffer	=	NULL;
	}
	isString = false;

	int32 length = Utils::WStrLen(_text) + 1;
	
	if (isChange && length > ELP_DEFAULT_MAX_TEXT)
	{
		while (bufferSize < length)
		{
			bufferSize += ELP_DEFAULT_MAX_TEXT;
		}
	}

	if (!bufferSize)
	{
		if (isChange)
		{
			bufferSize = ELP_DEFAULT_MAX_TEXT;
		}
		else
		{
			bufferSize = length;
		}
		textBuffer = new char16[bufferSize];
	}
	else if(bufferSize < length)
	{
		if (isChange)
		{
			bufferSize *= 2;
		}
		else
		{
			bufferSize = length;
		}
		SAFE_DELETE_ARRAY(textBuffer);
		textBuffer = new char16[bufferSize];
	}

	Utils::WStrCpy(textBuffer, _text);
	if (pFont)
	{
		textWidth = pFont->GetStringWidth(textBuffer);
		if (textBalancer)
		{
			textBalancer->Reset();
		}
	}
	Invalidate();
	UpdateTextPos();
	return true;
}

bool GUIText::SetText(const int16 _resID)
{
	if (isString)
	{
		SAFE_RELEASE(pStr);
	}
	else
	{
		SAFE_DELETE_ARRAY(textBuffer);
	}
	pStr = GUISystem::Instance()->GetStringSystem()->CreateString(_resID);
	if(!pStr)	return false;

	textBuffer = (char16*)pStr->GetString();
	if (!textBuffer)
	{
		return false;
	}
	isString = true;
	bufferSize = Utils::WStrLen(textBuffer);
	if (pFont)
	{
		textWidth = pFont->GetStringWidth(textBuffer);
		if (textBalancer)
		{
			textBalancer->Reset();
		}
	}
	Invalidate();
	UpdateTextPos();
	return true;
}

const char16* GUIText::GetText()
{
	return textBuffer;
}

void GUIText::SetIsCange(bool _isChange)
{
	isChange = _isChange;
}

bool GUIText::GetIsCange()
{
	return isChange;
}

void GUIText::SetDrawType( eDrawType newType )
{
	GUIControl::SetDrawType(newType);
	Font *newFont = GUISystem::Instance()->GetSkin()->GetFont(GetDrawType(), fontPlt);
	if (newFont != pFont)
	{
		pFont = newFont;
		if (textBuffer)
		{
			textWidth = pFont->GetStringWidth(textBuffer);
		}
		if (textBalancer)
		{
			textBalancer->Reset();
		}
	}
}

void GUIText::SetRect(const Rect &rc )
{
	GUIControl::SetRect(rc);
//	SetAlign(GetAlign());

	UpdateTextPos();
}

void GUIText::UpdateTextPos()
{
	if (align & Font::EAP_LEFT)
	{
		dx = marginLeft;
	}
	else if (align & Font::EAP_RIGHT)
	{
		dx = GetRect().dx - marginRight;
	}
	else if (align & Font::EAP_HCENTER)
	{
		int32 dWidth = (GetRect().dx - marginLeft - marginRight)>>1;
		dx = dWidth + marginLeft;
	}

	if (align & Font::EAP_TOP)
	{
		dy = marginTop;
	}
	else if (align & Font::EAP_BOTTOM)
	{
		dy = GetRect().dy - marginBottom;
	}
	else if (align & Font::EAP_VCENTER)
	{
		int32 dHeight = (GetRect().dy - marginTop - marginBottom)>>1;
		dy = dHeight + marginTop;
	}

	Invalidate();
}

void GUIText::DrawText()
{
	//UTILS_LOG(EDMP_DEBUG, "+++++++++ draw text");
	if (textBuffer)
	{
		Rect	rc;
		GetScreenRect(rc);

		//UTILS_LOG(EDMP_DEBUG, "screen rect x=%d, y=%d, dx=%d, dy=%d", rc.x, rc.y, rc.dx, rc.dy);
		pFont->SetCurrentPalette(fontPlt);
		//UTILS_LOG(EDMP_DEBUG, "palette: %d", fontPlt);
		pFont->DrawString(textBuffer, rc.x + dx, rc.y + dy, -1, align);
		//UTILS_LOG(EDMP_DEBUG, "text: %S", textBuffer);
	}
	//UTILS_LOG(EDMP_DEBUG, "--------- draw text");
}

void GUIText::SetTextMargins( int32 top, int32 left, int32 bottom, int32 right )
{
	marginTop		=	top;
	marginLeft		=	left;
	marginBottom	=	bottom;
	marginRight		=	right;
	UpdateTextPos();
}

int32 GUIText::GetTextWidth()
{
	return textWidth;
}