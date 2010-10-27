#include "GUIScrollBar.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "Utils.h"


GUIScrollBar::GUIScrollBar(GUIControlContainer *parent /* = NULL */, const ControlRect &cr /* = ControlRect */,
						   eScrollOrientation _orientation /* = ESO_UPTODOWN */)
:	GUIControl		(parent, cr)
,	orientation		(_orientation)
{
	Rect rc(cr.x, cr.y, cr.minDx, cr.minDy);
	
	SetScrollMargin(MARGIN_SIZE);

	SetDrawType(EDT_SCROLL);
//	CalcRects(rc);
	Recalculate();

	SetElementDrawType(EDT_CONTROL, EDT_CONTROL, EDT_SCROLL_BAR);
}

GUIScrollBar::~GUIScrollBar()
{
}

void GUIScrollBar::SetSize(uint32 _viewSize, uint32 _fullSize)
{
	if (viewSize != _viewSize || fullSize != _fullSize)
	{
		viewSize	=	_viewSize;
		fullSize	=	_fullSize;

		Recalculate();
	}
}

void GUIScrollBar::SetPos(uint32 _pos)
{
	if (_pos != position && fullSize != viewSize)
	{
		position	=	_pos;

		Recalculate();
	}
}

void GUIScrollBar::Draw()
{
	Rect rc;
	GetScreenRect(rc);
	GUISystem::Instance()->GetSkin()->DrawControl(GetDrawType(), rc);
	//GUISystem::Instance()->GetSkin()->DrawControl(topDrawType, rcTop);
	//GUISystem::Instance()->GetSkin()->DrawControl(bottomDrawType, rcBottom);

	if (fullSize - viewSize > 0)
		GUISystem::Instance()->GetSkin()->DrawControl(middleDrawType, rcMiddle);
}

void GUIScrollBar::SetRect(const Rect &rc)
{
	GUIControl::SetRect(rc);

	//
	Recalculate();
//	CalcRects(rc);
}

// void GUIScrollBar::CalcRects(const Rect &rc)
// {
// // 	if(ESO_UPTODOWN	==	orientation)
// // 	{
// // 		int32 rectHeight	=	rc.dy / 10;
// // 
// // 		rcTop.x				=	rc.x;
// // 		rcTop.y				=	rc.y;
// // 		rcTop.dx			=	rc.dx;
// // 		rcTop.dy			=	rectHeight;
// // 
// // 		rcBottom.x			=	rc.x;
// // 		rcBottom.y			=	rc.y + rc.dy - rectHeight;
// // 		rcBottom.dx			=	rc.dx;
// // 		rcBottom.dy			=	rectHeight;
// // 	}
// // 	else
// // 	{
// // 		int32 rectWidth		=	rc.dx / 10;
// // 
// // 		rcTop.x				=	rc.x;
// // 		rcTop.y				=	rc.y;
// // 		rcTop.dx			=	rectWidth;
// // 		rcTop.dy			=	rc.dy;
// // 
// // 		rcBottom.x			=	rc.x + rc.dx - rectWidth;
// // 		rcBottom.y			=	rc.y;
// // 		rcBottom.dx			=	rectWidth;
// // 		rcBottom.dy			=	rc.dy;
// // 	}
// 
// 	Recalculate();
// }

void GUIScrollBar::Recalculate()
{
	GetScreenRect(rcMiddle);

	int32	scrFullSize;
	int32	buttonOffset=	scrollMargin;
	if (ESO_LEFTTORIGHT ==	orientation)
	{
		scrFullSize		=	rcMiddle.dx - ((buttonOffset)<<1);
	}
	else
	{
 		scrFullSize		=	rcMiddle.dy - ((buttonOffset)<<1);
	}
	barMinSize			=	MIN_BUTTON_SIZE; //buttonOffset;
	barMaxSize			=	scrFullSize;

	fullSize			=	MAX(fullSize, 1);
	viewSize			=	CLAMP(viewSize, 1, fullSize);
	position			=	MIN(position, fullSize - viewSize);

	int32 scrSize		=	(scrFullSize * viewSize) / fullSize;
	scrSize				=	CLAMP(scrSize, barMinSize, barMaxSize);
	int32 scrPos		=	0;

	if (fullSize - viewSize)
	{
		scrPos			=	(scrFullSize - scrSize) * position / (fullSize - viewSize);
	}

	if (ESO_LEFTTORIGHT == orientation)
	{
		rcMiddle.x		+=	buttonOffset + scrPos;
		rcMiddle.dx		=	scrSize;
	}
	else
	{
		rcMiddle.y		+=	buttonOffset + scrPos;
		rcMiddle.dy		=	scrSize;
	}

	Invalidate();
}

void GUIScrollBar::SetElementDrawType( eDrawType top, eDrawType bottom, eDrawType middle )
{
	topDrawType = top;
	bottomDrawType = bottom;
	middleDrawType = middle;
	Invalidate();
}

void GUIScrollBar::SetScrollMargin(int32 margin)
{
	scrollMargin	=	margin;
}

void GUIScrollBar::RecalcDrawRect()
{
	GUIControl::RecalcDrawRect();
	Recalculate();
}
