#include "GUIEditMultiString.h"
#include "GUISystem.h"
#include "ResourceSystem.h"
#include "Application.h"
#include "Font.h"
#include "GUISkinLocal.h"
#include "Utils.h"
#include "GUIEditText.h"
#include "Graphics.h"


GUIEditMultiString::GUIEditMultiString(int32 bufSize/* = MAX_SIZE*/, eEditTextType eeType/* = EETT_ALL*/, GUIControlContainer * parent, const ControlRect &cr)
	:	GUIInputText		(eeType)
	,	GUIMultiString		((char16 *)NULL, parent, cr)
	,	isResizeable		(false)
{
	SetEditType(eeType);
	SetSelectable(true);


	SAFE_DELETE_ARRAY(GUIMultiString::textBuffer);
	GUIMultiString::bufferSize		=	bufSize;
	GUIMultiString::textBuffer		=	new char16[bufSize];

	GUIInputText::SetText(GUIMultiString::textBuffer, GUIMultiString::textSize, GUIMultiString::bufferSize);

	SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
}


GUIEditMultiString::~GUIEditMultiString()
{
	SAFE_DELETE_ARRAY(GUIMultiString::textBuffer);
	SAFE_RELEASE(pStr);
}


void GUIEditMultiString::SetText(const char16 * _pText)
{
	if(!_pText)	return;

	int32 strSize				=	Utils::WStrLen(_pText);

	FASSERT(strSize < GUIMultiString::bufferSize);

	Utils::WStrCpy(GUIMultiString::textBuffer, _pText);
	GUIMultiString::textSize	=	strSize;
	
	GUIInputText::SetText(GUIMultiString::textBuffer, GUIMultiString::textSize, GUIMultiString::bufferSize);
		
	yOffset						=	0;
	
	reparse = true;

	if(isResizeable)
		ResizeRect();


// 	if(!strCount && isResizeable)
// 	{
// 		int32 marginHeight = marginTop + marginBottom;
// 		int32 viewSize = marginHeight;
// 		ControlRect cr = GetControlRect();
// 
// 		int32 minHeight = marginHeight	+ pFont->GetHeight();
// 
// 		if(viewSize < minHeight)
// 			viewSize = minHeight;
// 
// 		if(cr.minDy != viewSize)
// 		{
// 			ResizeRect();
// 		}
// 	}
}

void GUIEditMultiString::SetText(uint16 resourseID)
{
	SAFE_RELEASE(pStr);
	pStr = GUISystem::Instance()->GetStringSystem()->CreateString(resourseID);

	reparse = true;

	return SetText(pStr->GetString());
}

void GUIEditMultiString::SetTextAlign(uint32 _align)
{
	GUIMultiString::SetTextAlign(_align);

	CalcCursorXY();
}

void GUIEditMultiString::Update()
{
	if (isResizeable)
	{
		Rect rc = GUIMultiString::GetRect();
		bool changed = false;
		if (rc.dx != realRect.dx)
		{
			int32 spd = (realRect.dx - rc.dx) / 3;
			changed = true;
			if (rc.dx < realRect.dx)
			{
				spd++;
				rc.dx += spd;
				if (rc.dx > realRect.dx)
				{
					rc.dx = realRect.dx;
				}
			}
			else
			{
				spd--;
				rc.dx += spd;
				if (rc.dx < realRect.dx)
				{
					rc.dx = realRect.dx;
				}
			}
		}
		if (rc.dy != realRect.dy)
		{
			int32 spd = (realRect.dy - rc.dy) / 3;
			changed = true;
			if (rc.dy < realRect.dy)
			{
				spd++;
				rc.dy += spd;
				if (rc.dy > realRect.dy)
				{
					rc.dy = realRect.dy;
				}
			}
			else
			{
				spd--;
				rc.dy += spd;
				if (rc.dy < realRect.dy)
				{
					rc.dy = realRect.dy;
				}
			}
		}
		if (changed)
		{
			GUIMultiString::SetRect(rc);
		}
	}

	if(GUISystem::Instance()->GetFocus() != this)
		return;

	GUIInputText::Update();

	InvalidateCursor();
}

void GUIEditMultiString::Draw()
{
	Rect	rc;
	GetScreenRect(rc);

	if(GUISystem::Instance()->GetFocus() == this)
	{
		if(textChanged)
		{
			GUIControlContainer::Draw();
			DrawText(rc, false);
			textChanged	=	false;
		}

		if(showCursor)
		{
			pApp->GetGraphicsSystem()->SetColor(0,0,0);
			pApp->GetGraphicsSystem()->DrawRect(rc.x + cursorX, rc.y + cursorY - yOffset, cursorWidth, pFont->GetHeight());
		}
	}
	else
	{
		GUIControlContainer::Draw();
		DrawText(rc, false);
	}
}

void GUIEditMultiString::OnSetFocus()
{
	GUISystem::Instance()->AddListener(EEI_CHAR, this);

	GUISystem::Instance()->LockKeyboard();

	GUIMultiString::OnSetFocus();
	GUIInputText::OnSetFocus();

	textChanged	=	true;
}

void GUIEditMultiString::OnLostFocus()
{
	GUISystem::Instance()->RemoveListener(EEI_CHAR, this);

	GUISystem::Instance()->UnlockKeyboard();

	GUIMultiString::OnLostFocus();
	GUIInputText::OnLostFocus();
}


void GUIEditMultiString::DeleteSymbol()
{
	GUIInputText::DeleteSymbol();
	reparse = true;

	Invalidate();
}


void GUIEditMultiString::CalcCursorXY()
{
	InvalidateCursor();

	if(!GUIInputText::textBuffer)				return;

	if(!strCount)	
	{
		SetStartCursorPos();
		return;
	}

	if(isResizeable)
		ResizeRect();

	int32 symbolPos			=	currentPos;

	ParsedString *ps		=	(ParsedString *)(*parsedStrings.Begin());
	VList::Iterator iStr	=	parsedStrings.Begin();
	for(int32 yPos = 0; yPos < strCount; ++iStr, ++yPos)
	{
		ps				=	(ParsedString *)(*iStr);
		if(symbolPos	<	ps->count)
		{
			break;
		}
		else if(symbolPos == ps->count)
		{
			VList::Iterator usedStr	=	iStr;
			++usedStr, ++yPos;

			if(		yPos < strCount 
				&&	usedStr != parsedStrings.End() 
				&& isKeyDown )
			{
				ps				=	(ParsedString *)(*usedStr);
				symbolPos	=	0;
			}

			break;
		}
		symbolPos		-=	ps->count;
	}

	int32 strWidth		=	pFont->GetStringWidth(ps->pStr, ps->count);
	int32 width			=	pFont->GetStringWidth(ps->pStr, symbolPos);
	
	if(align & Font::EAP_RIGHT)
	{
		cursorX		=	ps->x - strWidth + width;
	}
	else if(align & Font::EAP_HCENTER)
	{
		cursorX		=	ps->x - (strWidth >> 1) + width;
	}
	else
	{
		cursorX		=	ps->x + width;
	}

	cursorY			=	ps->y;


	int32 oldOff = yOffset;
	Rect rc			=	GetRect();
	int32 height	=	rc.dy	-	marginBottom - pFont->GetHeight();

	if(strCount	*	pFont->GetHeight() < height)
	{
		yOffset		=	0;
	}
	else if(cursorY - yOffset < marginTop)
	{
		yOffset = cursorY - marginTop;
	}
	else if(cursorY - yOffset > height)
	{
		yOffset = cursorY - height;
	}

	//
	int32	fullHeight	=	(strCount-1) * pFont->GetHeight() + marginTop;
	if(		(fullHeight> height)
		&&	(fullHeight - yOffset < height))
	{
		yOffset	=	fullHeight - height;
	}
	//

	if(isResizeable)	yOffset	=	0;
	scrollBar->SetPos(yOffset);
	if (oldOff != yOffset)
	{
		Invalidate();
	}
}


bool GUIEditMultiString::AddChar(char16 ch)
{
	bool ret = GUIInputText::AddChar(ch);
	
	reparse = true;
	Invalidate();
	
	return ret;
}


void GUIEditMultiString::SetStartCursorPos()
{
	Rect rc;
	GetScreenRect(rc);

	int32 rectWidth		=	rc.dx - marginLeft - marginTop;
	if(showScroll)
		rectWidth		-=	scrollWidth;

	int32 rectHeight	=	rc.dy - marginTop - marginBottom;

	if(align & Font::EAP_RIGHT)
	{
		cursorX			=	rectWidth - cursorWidth;
	}
	else if(align & Font::EAP_HCENTER)
	{
		cursorX			=	(rectWidth - cursorWidth)>>1;
	}
	else
	{
		cursorX			=	0;
	}

	if(align & Font::EAP_BOTTOM)
	{
		cursorY		=	rectHeight - pFont->GetHeight();	
	}
	else if(align & Font::EAP_VCENTER)
	{
		cursorY		=	(rectHeight - pFont->GetHeight())>>1;	
	}
	else
	{
		cursorY		=	0;
	}

	cursorY			+=	marginTop;
	cursorX			+=	marginLeft;
}

bool GUIEditMultiString::MoveCursorOnUp()
{
	StopEditSymbol();

	bool isMoved			=	false;

	int32 newSymbolPos		=	0;
	int32 count				=	0;	
	int32 symbolPos			=	currentPos;
	ParsedString *ps		=	(ParsedString *)(*parsedStrings.Begin());
	VList::Iterator iStr	=	parsedStrings.Begin();
	for(int32 yPos = 0; yPos < strCount; ++iStr, ++yPos)
	{
		ps					=	(ParsedString *)(*iStr);

		if(symbolPos	<=	ps->count)
		{
			if(symbolPos > count)
				symbolPos	=	count;

			if(yPos)
			{
				currentPos	=	newSymbolPos + symbolPos;
				CalcCursorXY();

				isMoved		=	true;
			}
			break;
		}

		newSymbolPos		+=	count;
		count				=	ps->count;
		symbolPos			-=	ps->count;
	}

	return	isMoved;
}

bool GUIEditMultiString::MoveCursorOnDown()
{
	StopEditSymbol();
	bool isMoved			=	false;

	int32 yPos				=	0;

	int32 newSymbolPos		=	0;
	int32 symbolPos			=	currentPos;
	ParsedString *ps		=	(ParsedString *)(*parsedStrings.Begin());
	VList::Iterator iStr	=	parsedStrings.Begin();
	for(int32 yPos = 0; yPos < strCount; ++iStr, ++yPos)
	{
		ps				=	(ParsedString *)(*iStr);
		newSymbolPos	+= ps->count;
		if(symbolPos	<=	ps->count)
		{
			++iStr, ++yPos;
			if(		iStr != parsedStrings.End()
				&&	yPos < strCount)
			{
				ps				=	(ParsedString *)(*iStr);
				if(symbolPos > ps->count)
					symbolPos = ps->count;

				currentPos	=	newSymbolPos + symbolPos;

				CalcCursorXY();

				isMoved		=	true;
			}
			break;
		}

		symbolPos		-=	ps->count;
	}

	return	isMoved;
}	

void GUIEditMultiString::ParseStrings(int32 scrollW)
{
	//UTILS_TRACE("++++++ Parse Edit");
	GUIMultiString::textSize	=	GUIInputText::textSize;	
	GUIMultiString::ParseStrings(scrollW);

	CalcCursorXY();
	//UTILS_TRACE("------ Parse Edit");
}

void GUIEditMultiString::SetDrawType(eDrawType newType)
{
	GUIMultiString::SetDrawType(newType);
	ControlRect cr = GetControlRect();
	int32 mDy = pFont->GetHeight() + marginTop + marginBottom;
	if (mDy > cr.minDy)
	{
		cr.minDy = mDy;
		SetControlRect(cr);
	}

	CalcCursorXY();
}

void GUIEditMultiString::SetRect(const Rect &rc)
{
	if (!isResizeable)
	{
		GUIMultiString::SetRect(rc);
		return;
	}
	//UTILS_TRACE("+++++++++++++++++++++++++++++++ Set Rect");
	//UTILS_LOG(EDMP_DEBUG, "real rect before x=%d, y=%d, dx=%d, dy=%d", realRect.x, realRect.y, realRect.dx, realRect.dy);
	//UTILS_LOG(EDMP_DEBUG, "rc before x=%d, y=%d, dx=%d, dy=%d", rc.x, rc.y, rc.dx, rc.dy);
	//UTILS_TRACE("real rect before x=%d, y=%d, dx=%d, dy=%d", realRect.x, realRect.y, realRect.dx, realRect.dy);
	//UTILS_TRACE("rc before x=%d, y=%d, dx=%d, dy=%d", rc.x, rc.y, rc.dx, rc.dy);
	realRect = rc;
	Rect oldRect = GUIMultiString::GetRect();
	//UTILS_TRACE("set rect x=%d, y=%d, dx=%d, dy=%d", realRect.x, realRect.y, realRect.dx, realRect.dy);
	//UTILS_TRACE("old rect x=%d, y=%d, dx=%d, dy=%d", oldRect.x, oldRect.y, oldRect.dx, oldRect.dy);
	oldRect.x = rc.x;
	oldRect.y = rc.y;
	oldRect.dx = rc.dx;
	//UTILS_TRACE("real rect after x=%d, y=%d, dx=%d, dy=%d", realRect.x, realRect.y, realRect.dx, realRect.dy);
	//UTILS_TRACE("old rect after x=%d, y=%d, dx=%d, dy=%d", oldRect.x, oldRect.y, oldRect.dx, oldRect.dy);
	//UTILS_TRACE("------------------------------- Set Rect");
	GUIMultiString::SetRect(oldRect);

	textChanged	=	true;
}

Rect GUIEditMultiString::GetRect()
{
	if (!isResizeable)
	{
		return GUIMultiString::GetRect();
	}
	//UTILS_TRACE("+++++++++++++++++++++++++++++++ get rect");
	//UTILS_TRACE("------------------------------- get rect");

	return realRect;
}

void GUIEditMultiString::ResizeRect()
{
	//UTILS_TRACE("+++++++++++++++++++++++++++++++ resize rect");
	if(!pFont) return;

	int32 marginHeight = marginTop + marginBottom;
	int32 viewSize = strCount * pFont->GetHeight() + marginHeight;
	ControlRect cr = GetControlRect();
	//Rect r = GetRect();
	int32 minHeight = marginHeight	+ pFont->GetHeight();

	if(viewSize < minHeight)
		viewSize = minHeight;

	if(cr.minDy != viewSize)
	{
		//UTILS_TRACE("+++++++++++++++++++++++++++++++++ change multiedit size");

		cr.minDy = viewSize;
		cr.maxDy = viewSize;
		int32 rdx = GetRect().dx;
		if (cr.minDx < rdx)
		{
			cr.minDx = rdx;
		}
		SetControlRect(cr);
		//SetRect(r);

		GUIControlContainer *parent = (GUIControlContainer *)GetParent();
		if(parent)
		{
			VList *childs = parent->GetChilds();
			VList::Iterator currentPos	= childs->PosItem(this);
			VList::Iterator prevPos		= currentPos;

			if(prevPos != childs->Begin())
			{
				--prevPos;
			}

			if(!childs->Size())
			{
				parent->AddChild(this);
			}
			else
			{
				++prevPos;
				if(prevPos != childs->End())
				{
					parent->InsertChild(this, (GUIControl *)(*prevPos));
				}
				else
				{
					parent->AddChild(this);
				}
			}
		}
		//UTILS_TRACE("--------------------------------- change multiedit size");

		textChanged	=	true;
	}
	//UTILS_TRACE("------------------------------- resize rect");
}

void GUIEditMultiString::SetResizeable(bool resizeable)
{
	isResizeable = resizeable;
	isScrollNeeded = false;
}

void GUIEditMultiString::SetTextMargins( int32 top, int32 left, int32 bottom, int32 right )
{
	GUIMultiString::SetTextMargins(top, left, bottom, right);
	ControlRect cr = GetControlRect();
	int32 mDy = pFont->GetHeight() + marginTop + marginBottom;
	if (mDy > cr.minDy)
	{
		cr.minDy = mDy;
		SetControlRect(cr);
		CalcCursorXY();
	}
}

void GUIEditMultiString::SetControlRect(const ControlRect &cr)
{
	GUIMultiString::SetControlRect(cr);

	textChanged	=	true;
}

void GUIEditMultiString::Invalidate()
{
	GUIMultiString::Invalidate();

	textChanged	=	true;
}

void GUIEditMultiString::InvalidateCursor()
{
	Rect rc;
	GetScreenRect(rc);

	rc.x	+=	cursorX;
	rc.y	+=	(cursorY - yOffset);
	rc.dx	=	cursorWidth;
	rc.dy	=	pFont->GetHeight();

 	textChanged	=	true;

	GUISystem::Instance()->InvalidateRect(rc, this);
}

void GUIEditMultiString::ChangeChar(char16 ch)
{
	GUIInputText::ChangeChar(ch);

	Invalidate();
}

void GUIEditMultiString::SetRealRect( const Rect &rc )
{
	realRect = rc;
	GUIMultiString::SetRect(realRect);
}

bool GUIEditMultiString::InsertText( const char16 *textToInsert )
{
	bool ret = GUIInputText::InsertText(textToInsert);
	if (ret)
	{
		reparse = true;
		Invalidate();
	}
	return ret;
}

bool GUIEditMultiString::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_CHAR:
		GUIInputText::OnChar((char16)pData->wParam);
		return true;
	}
	return false;
}