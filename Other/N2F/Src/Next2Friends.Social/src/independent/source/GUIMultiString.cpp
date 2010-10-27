#include "GUIMultiString.h"
#include "GUISystem.h"
#include "ResourceSystem.h"
#include "Application.h"
#include "Font.h"
#include "GUISkinLocal.h"
#include "Utils.h"


GUIMultiString::GUIMultiString(char16* inpText, GUIControlContainer * parent, const ControlRect &cr)
	:	GUIControlContainer		(parent, cr)
	,	textSize				(0)
	,	bufferSize				(0)
{

	SetDefaults();

	SetText(inpText);	
}

GUIMultiString::GUIMultiString(uint16 resourseID, GUIControlContainer * parent, const ControlRect &cr)
	:	GUIControlContainer		(parent, cr)
	,	textSize				(0)
	,	bufferSize				(0)
{
	SetDefaults();

	SetText(resourseID);	
}

GUIMultiString::~GUIMultiString()
{
	int32 size = parsedStrings.Size();
	for(VList::Iterator iStr = parsedStrings.Begin(); iStr != parsedStrings.End(); ++iStr)
	{
		ParsedString * ps = (ParsedString *)(*iStr);
		SAFE_DELETE(ps);
	}

	if(!showScroll)
	{
		SAFE_DELETE(scrollBar);
	}
	
	if(isString)
	{
		SAFE_RELEASE(pStr);
		textBuffer		=	NULL;
	}
	else
	{
		SAFE_DELETE_ARRAY(textBuffer);
	}
}

const char16 * GUIMultiString::GetText()
{
	return textBuffer;
}

void GUIMultiString::SetText(const char16 * _pText)
{
	if(!_pText)
		return;

	if (isString)
	{
		SAFE_RELEASE(pStr);

		textBuffer		=	NULL;
		isString		=	false;
	}

	int32 inBufSize	= Utils::WStrLen(_pText) + 1;

    if(bufferSize < inBufSize)
	{
		SAFE_DELETE_ARRAY(textBuffer);
		textBuffer	=	new	char16[inBufSize];
		bufferSize	=	inBufSize;
	}

	Utils::WStrNCpy(textBuffer, bufferSize * sizeof(char16), _pText, Utils::WStrLen(_pText));
	textSize = Utils::WStrLen(textBuffer);

	scrollBar->SetPos(yOffset = 0);
	reparse = true;
	Invalidate();
}

void GUIMultiString::SetText(uint16 resourseID)
{
	if(isString)
	{
		SAFE_RELEASE(pStr);
		textBuffer		=	NULL;
	}
	else
	{
		SAFE_DELETE_ARRAY(textBuffer);
		bufferSize		=	0;
		textSize		=	0;
		
		isString		=	true;
	}

	pStr		=	GUISystem::Instance()->GetStringSystem()->CreateString(resourseID);
	textBuffer	=	(char16*)pStr->GetString();
	textSize	=	Utils::WStrLen(textBuffer);

	scrollBar->SetPos(yOffset = 0);
	reparse = true;
	Invalidate();
}

void GUIMultiString::SetTextAlign(uint32 _align)
{
	if(align != _align)
	{
		align		=	_align;
		drawedAlign	=	_align;
		reparse = true;
		Invalidate();
	}
}

uint32 GUIMultiString::GetLenFromCharToWordEnd(char16 *ch)
{
	if(!ch)	return 0;

	uint32	curLen	= 0;
	char16	curChar = *ch;

	if(curChar == ' ')
	{
		// is spaces word
		while(curChar == ' ')
		{
			ch++;
			curLen++;
			curChar = *ch;
		}
		return curLen;
	}
	else if(curChar == '\n')
	{
		return 1;
	}
	else
	{
		// is character word
		while ((curChar != '\n') && (curChar != '\r') && (curChar != '\0') && (curChar != ' '))
		{
			ch++;
			curLen++;
			curChar = *ch;
		}

		return curLen;
	}
}


void GUIMultiString::ParseStrings(int32 scrollW)
{
	reparse = false;

	strCount	=	0;
	if(!pFont)			return;
	if(!textSize)		return;

	Rect	rc = GetRect();
	rc.dx -= scrollW;

	int32 maxX					=	rc.dx - marginLeft - marginRight;
	int32 halfOfMaxX			=	maxX >> 1;
	int32 maxY					=	0;

	if(maxX <= 0)		return;

	int32 listSize				=	parsedStrings.Size();
	VList::Iterator	psIterator	=	parsedStrings.Begin(); 

	bool	textEnd			=	false;
	char16 *curTextPtr		=	(char16*) GetText();
	uint32	curStrPosY		=	0;

	do 
	{
		int32 curStrLen	= GetLenFromCharToWordEnd(curTextPtr);
		int32 lastStrLen	= curStrLen;
		
		// if first word is longer than our width
		
		if(curTextPtr[0] == '\n')
		{
			curStrLen = lastStrLen = 1;
		}
		else
		if(maxX < pFont->GetStringWidth(curTextPtr, curStrLen))
		{
			while (maxX < pFont->GetStringWidth(curTextPtr, curStrLen))
			{
				curStrLen--;
			}
			lastStrLen = curStrLen;

			// control is too small
			if(lastStrLen == 0)
			{
				textEnd = true;
				break;
			}
		}
		else
		{
			while(maxX > pFont->GetStringWidth(curTextPtr, curStrLen))
			{
				lastStrLen = curStrLen;
				curStrLen += GetLenFromCharToWordEnd(curTextPtr + curStrLen);

				if(curTextPtr[lastStrLen] == '\n')
				{
//					curStrLen = lastStrLen;
					lastStrLen = curStrLen;
					break;
				}

				if(lastStrLen == curStrLen)
				{
					textEnd = true;
					break;
				}
			}
		}


		ParsedString *ps;
		if(strCount < listSize)
		{
			ps = (ParsedString *)(*psIterator);
			++psIterator;
		}
		else
		{
			ps = new ParsedString();
			parsedStrings.PushBack((void *)ps);
		}
		++strCount;

		ps->pStr	=	(char16*) curTextPtr;
		ps->count	=	lastStrLen;

		ps->spaceCount	=	0;
		while(		curTextPtr[ps->spaceCount]
				&&	' '	==	curTextPtr[ps->spaceCount]
				&&	ps->spaceCount < lastStrLen )
		{
			++ps->spaceCount;
		}

		if(align & Font::EAP_HCENTER)	
		{
			ps->x = halfOfMaxX; 
		}
		else if(align & Font::EAP_RIGHT)
		{
			ps->x = maxX;
		}
		else if(align & Font::EAP_LEFT)
		{
			ps->x = 0;
		}
		ps->x += marginLeft;

		ps->y = curStrPosY;

		curTextPtr += lastStrLen;
		curStrPosY += pFont->GetHeight();

		maxY = curStrPosY;
	}
	while(!textEnd);

	// set vertical scrolls
	if(align & Font::EAP_BOTTOM)	
	{
		drawedAlign &= ~Font::EAP_BOTTOM;

		SetYOffset(rc.dy - maxY - marginBottom);
	}
	else if(align & Font::EAP_VCENTER)
	{
		drawedAlign &= ~Font::EAP_VCENTER;
		SetYOffset((rc.dy - maxY)>>1);
	}
	else
	{
		SetYOffset(marginTop);
	}
	drawedAlign |= Font::EAP_TOP;

	UpdateScroll();
	//UTILS_TRACE("Before underlines");
	if(drawUnderline)
		UpdateLines();
	//UTILS_TRACE("After underlines");

	Invalidate();
}


void GUIMultiString::Update()
{
	if(GUISystem::Instance()->GetFocus() != this)
		return;

	if(pApp->IsKeyPressed(Application::EKC_UP))
	{
		if(yOffset > 0)
		{
			ScrollUp();
		}
	}
	else if(pApp->IsKeyPressed(Application::EKC_DOWN))
	{
		Rect rc;
		GetScreenRect(rc);
	
		int32 maxHeight = strCount * pFont->GetHeight();
		int32 drawedHeight = rc.dy - marginTop - marginBottom;

		if(yOffset < maxHeight - drawedHeight)
		{
			ScrollDown(maxHeight - drawedHeight);
		}
	}
}

void GUIMultiString::Draw()
{
	GUIControlContainer::Draw();

	Rect rc;
	GetScreenRect(rc);
 
	DrawText(rc, true);
}

void GUIMultiString::SetScrollRect()
{
 	Rect rc = GetRect();

	rc.y		=	0;
	rc.x		=	rc.dx - scrollWidth;
	rc.dx		=	scrollWidth;

	scrollBar->SetRect(rc);

	Invalidate();
}

void GUIMultiString::SetRect(const Rect &rc)
{
	Rect r = GetRect();
	GUIControlContainer::SetRect(rc);

	if (rc.dx != r.dx && rc.dy != r.dy)
	{
		SetScrollRect();
	}
	if (rc.dy != r.dy)
	{
		reparse = true;
	}
}

void GUIMultiString::UpdateScroll()
{
	if (!isScrollNeeded)
	{
		return;
	}
	//UTILS_TRACE("++++++++++ Update scroll");
	SetScrollRect();

	Rect rc = GetRect();

	int32 fontHeight	=	pFont->GetHeight(); 
	int32 viewSize		=	((rc.dy - marginBottom - marginTop) /fontHeight) * fontHeight;
	int32 fullSize		=	strCount * pFont->GetHeight();

	scrollBar->SetSize(viewSize, fullSize);

	if(fullSize <= viewSize)
	{
		if(showScroll)
		{
			scrollBar->SetParent(NULL);
			showScroll = false;	
		}
	}
	else
	{
		if(!showScroll)
		{	
			//UTILS_TRACE("++++ Show scroll");
			showScroll = true;	
			ParseStrings(scrollWidth);

			scrollBar->SetParent(this);
			//UTILS_TRACE("---- Show scroll");
		}
	}
	//UTILS_TRACE("--------- Update scroll");
}

void GUIMultiString::SetYOffset(int32 offs)
{
	if(offs < marginTop)
	{
		offs = marginTop;
	}

	for(VList::Iterator iStr = parsedStrings.Begin(); iStr != parsedStrings.End(); ++iStr)
	{
		ParsedString *ps = (ParsedString *)(*iStr);
		ps->y += offs;
	}
}

void GUIMultiString::SetScrollWidth(int32 sWidth)
{
	scrollWidth		=	sWidth;
	SetScrollRect();
}

void GUIMultiString::SetTextMargins(int32 top, int32 left, int32 bottom, int32 right)
{
	marginTop		=	top;
	marginLeft		=	left;
	marginBottom	=	bottom;
	marginRight		=	right;
	reparse = true;
	Invalidate();
}

void GUIMultiString::SetDefaults()
{
	reparse = false;
	isScrollNeeded = true;
	SetTextAlign(Font::EAP_LEFT);
	SetDrawType(GetDrawType());

	// set default sizes
//	SetMargins(MARGIN_TOP, MARGIN_LEFT, MARGIN_BOTTOM, MARGIN_RIGHT);

	// set font
	pApp				=	GUISystem::Instance()->GetApp();

	// set scroll
	yOffset				=	0;
	SetScrollDelta(pFont->GetHeight());

	// create scrollbar
	showScroll			=	false;
	scrollBar			=	new GUIScrollBar();
	SetScrollWidth(SCROLL_WIDTH);

	isString			=	false;

	textBuffer			=	NULL;
	textSize			=	0;
	bufferSize			=	0;


	drawUnderline		=	false;
	underlineDrawType	=	EDT_NONE;
}

void GUIMultiString::SetScrollDelta(int32 delta)
{
	scrollDelta			=	delta;
}

void GUIMultiString::SetDrawType( eDrawType newType )
{
	GUIControlContainer::SetDrawType(newType);
	pFont = GUISystem::Instance()->GetSkin()->GetFont(GetDrawType(), fontPlt);

	UpdateUnderlineY();

	reparse = true;
	Invalidate();
}

void GUIMultiString::ScrollUp()
{
	yOffset -= scrollDelta;
	if(yOffset < 0)
		yOffset = 0;

	scrollBar->SetPos(yOffset);
	Invalidate();
}

void GUIMultiString::ScrollDown(int32 topY)
{
	yOffset += scrollDelta;
	if(yOffset > topY)
		yOffset = topY;

	scrollBar->SetPos(yOffset);
	Invalidate();
}

void GUIMultiString::SetControlRect(const ControlRect &cr)
{
// 	ControlRect cRect = GetControlRect();
	//Rect cRect = GetRect();
	GUIControlContainer::SetControlRect(cr);

	//if (cr.minDx != cRect.dx || cr.minDy != cRect.dy)
	//{
	//	ParseStrings(0);
	//}
 
// 	ParseStrings(0);
}

void GUIMultiString::SetUnderlined(bool isUnderlined)
{
	drawUnderline	=	isUnderlined;

	UpdateUnderlineY();
}

void GUIMultiString::SetUnderlinedDrawType(eDrawType newType)
{
	underlineDrawType	=	newType;

	UpdateUnderlineY();
}

void GUIMultiString::UpdateUnderlineY()
{
	underlineDy	=	pFont->GetHeight();	

	UpdateLines();
}

void GUIMultiString::DrawText(const Rect &rc, bool skipSpaces)
{
	if (reparse)
	{
		ParseStrings(0);
	}

	if(drawUnderline && pFont)
	{
		Rect underlineRC;
		underlineRC.x	=	rc.x;
		underlineRC.dx	=	(showScroll) ? rc.dx - scrollWidth : rc.dx;
		underlineRC.dy	=	underlineDy;


		underlineRC.y	=	rc.y + startUnderLineY;

		int32 fullHeight = rc.dy - marginBottom + rc.y;
		while(underlineRC.y + underlineDy < fullHeight)
		{
			GUISystem::Instance()->GetSkin()->DrawControl(underlineDrawType, underlineRC);
			underlineRC.y += pFont->GetHeight();
		}
	}

	if(pFont && strCount)
	{
		pFont->SetCurrentPalette(fontPlt);

		int32 x			= rc.x;
		int32 y			= rc.y - yOffset;
		int32 bottom	= rc.y + marginBottom;	
		int32 top		= rc.dy	+ rc.y - marginTop;

		VList::Iterator iStr = parsedStrings.Begin();
		for(int32 i = 0; i < strCount; ++i, ++iStr)
		{
			ParsedString *ps = (ParsedString *)(*iStr);
			int32 drawY = y + ps->y;

			if(drawY < bottom)		continue;
			if(drawY >= top)		break;

			if(skipSpaces)
			{
				pFont->DrawString(ps->pStr + ps->spaceCount, x + ps->x, drawY, ps->count - ps->spaceCount, drawedAlign);
			}
			else
			{
				pFont->DrawString(ps->pStr, x + ps->x, drawY, ps->count, drawedAlign);
			}
		}
	}

}

void GUIMultiString::UpdateLines()
{
	if(!strCount)	return;
	
	Rect rc = GetRect();

	ParsedString *ps = (ParsedString *)(*parsedStrings.Begin());
	startUnderLineY	= ps->y;

	while(startUnderLineY + underlineDy > marginTop)
	{
		startUnderLineY -= underlineDy;
	}
}

int32 GUIMultiString::GetFullHeight()
{
	if (reparse)
	{
		ParseStrings(0);
	}
	return strCount * pFont->GetHeight() + marginBottom + marginTop;
}