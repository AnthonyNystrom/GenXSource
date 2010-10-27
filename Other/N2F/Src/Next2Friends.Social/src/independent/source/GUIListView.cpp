#include "GUIListView.h"
#include "GUISystem.h"
#include "GUIData.h"

GUIListView::GUIListView(GUIControlContainer * _parent, const ControlRect & cr)
	:	GUIControlContainer		(_parent, cr)
{
	GUIControlContainer::SetDrawType(EDT_NONE);

	innerMarginLeft = 0;
	innerMarginRight = 0;
	innerMarginTop = 0;
	innerMarginBottom = 0;


	layoutContainer		=	new GUIControlContainer(NULL/*, gccr*/);
	GUIControlContainer::AddChild(layoutContainer);
	layoutContainer->SetDrawType(EDT_NONE);

	scrollBar			=	new	GUIScrollBar(NULL/*, gsbr*/);
	GUIControlContainer::AddChild(scrollBar);

	layout				=	new GUILayoutBox(true, layoutContainer, ControlRect(), EGD_BOTH);
	layout->SetDrawType(EDT_NONE);

	targetY = 0;
	prescroll = ECP_LISTVIEW_PRESCROLL;
	speedModifer = 3;
	itemsAlign = ESP_TOP;
	isScrolled = false;
}

GUIListView::~GUIListView()
{
// 	if(!scrollBar->GetParent())
// 	{
// 		SAFE_DELETE(scrollBar);
// 	}
}



void GUIListView::AddChild(GUIControl *child)
{
	layout->AddChild(child);
}

void GUIListView::InsertChild(GUIControl *child, GUIControl *next)
{
	layout->InsertChild(child, next);
}

void GUIListView::RemoveChild(GUIControl *child)
{
	layout->RemoveChild(child);
}

void GUIListView::RemoveAllItems()
{
	layout->Clear();
	isScrolled = false;

	//layoutChilds->Clear();
	//UpdateScrollSize();
}

void GUIListView::Update()
{
	GUIControlContainer::Update();

	UpdateScrollSize();
}

void GUIListView::UpdateScrollSize()
{

	VList *layoutChilds	=	layout->GetChilds();	

	if(!layoutChilds->Empty())
	{
		int32 vH	=	layoutContainer->GetRect().dy;
		int32 fH	=	layout->GetRect().dy;
		if (fH < vH)
		{
			vH = fH;
		}
		scrollBar->SetSize(vH, fH);

		UpdateScrollPos();
	}
}	

void GUIListView::UpdateScrollPos()
{

	CheckFocused();

	Rect rc			=	layout->GetRect();

	if (targetY != rc.y)
	{
		int32 spd = (targetY - rc.y) / speedModifer;
		if (targetY > rc.y)
		{
			spd++;
			rc.y += spd;
			if (targetY < rc.y)
			{
				rc.y = targetY;
			}
		}
		else
		{
			spd--;
			rc.y += spd;
			if (targetY > rc.y)
			{
				rc.y = targetY;
			}
		}
		layout->SetRect(rc);
	}


	scrollBar->SetPos(-rc.y);

}

void GUIListView::CheckFocused()
{
	GUIControl *focused = GUISystem::Instance()->GetFocus();
	if (!focused)
	{
		return;
	}
	while (focused->GetParent() != layout)
	{
		focused = focused->GetParent();
		if (!focused)
		{
			return;
		}
	}

	GUIControl *cur		= GUISystem::Instance()->GetFocus();

	Rect curRect;
	cur->GetScreenRect(curRect);
	Rect viewRect;
	layoutContainer->GetScreenRect(viewRect);
	Rect layoutRect;
	layout->GetScreenRect(layoutRect);


	Rect rc			=	layout->GetRect();
	if (layoutRect.y + layoutRect.dy < viewRect.y + viewRect.dy && itemsAlign == ESP_BOTTOM)
	{
		targetY = viewRect.dy - layoutRect.dy;
	}
	if(curRect.y - prescroll < viewRect.y && curRect.dy - prescroll < viewRect.dy)
	{
		//UTILS_TRACE("++++++++++++++++++++++++ update scroll pos ");
		//UTILS_TRACE(" === control is up === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		if (layoutRect.y < viewRect.y)
		{
			targetY = -(curRect.y - layoutRect.y);
			targetY += prescroll;
			if (targetY > 0)
			{
				targetY = 0;
			}
		}

		//cur->GetScreenRect(curRect);
		//layout->GetScreenRect(layoutRect);
		//UTILS_TRACE(" === after set === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		//UTILS_TRACE("------------------------- update scroll pos ");
	}
	else if(curRect.y + curRect.dy + prescroll > viewRect.y + viewRect.dy)
	{
		//UTILS_TRACE("++++++++++++++++++++++++ update scroll pos ");
		//UTILS_TRACE(" === control is down === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		if (layoutRect.y + layoutRect.dy > viewRect.y + viewRect.dy )
		{
			targetY = viewRect.dy - (curRect.y + curRect.dy - layoutRect.y);
			targetY -= prescroll;
			if (targetY < viewRect.dy - layoutRect.dy)
			{
				targetY = viewRect.dy - layoutRect.dy;
			}
		}


		//cur->GetScreenRect(curRect);
		//layout->GetScreenRect(layoutRect);
		//UTILS_TRACE(" === after set === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		//UTILS_TRACE("------------------------- update scroll pos ");
	}
	else if (layoutRect.y + layoutRect.dy < viewRect.y + viewRect.dy && itemsAlign == ESP_BOTTOM)
	{
		//UTILS_TRACE("++++++++++++++++++++++++ update scroll pos ");
		//UTILS_TRACE(" === down control is not down === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		targetY = viewRect.dy - layoutRect.dy;

		//cur->GetScreenRect(curRect);
		//layout->GetScreenRect(layoutRect);
		//UTILS_TRACE(" === after set === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		//UTILS_TRACE("------------------------- update scroll pos ");
	}
	else if (layoutRect.y > viewRect.y && itemsAlign == ESP_TOP)
	{
		//UTILS_TRACE("++++++++++++++++++++++++ update scroll pos ");
		//UTILS_TRACE(" === down control is not down === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		targetY = 0;

		//cur->GetScreenRect(curRect);
		//layout->GetScreenRect(layoutRect);
		//UTILS_TRACE(" === after set === ");
		//UTILS_TRACE("current x=%d, y=%d, dx=%d, dy=%d", curRect.x, curRect.y, curRect.dx, curRect.dy);
		//UTILS_TRACE("layout x=%d, y=%d, dx=%d, dy=%d", layoutRect.x, layoutRect.y, layoutRect.dx, layoutRect.dy);
		//UTILS_TRACE("------------------------- update scroll pos ");
	}
}

void GUIListView::SetRect( const Rect &rc )
{
	GUIControlContainer::SetRect(rc);
	RecalcViewRect();
}

void GUIListView::SetInnerMargins( int32 left, int32 right, int32 top, int32 bottom )
{
	innerMarginLeft = left;
	innerMarginRight = right;
	innerMarginBottom = bottom;
	innerMarginTop = top;
	RecalcViewRect();
}

void GUIListView::RecalcViewRect()
{
	if (layoutContainer)
	{
		Rect rc = GetRect();
		rc.x = innerMarginLeft;
		rc.y = innerMarginTop;
		rc.dx -= innerMarginLeft + innerMarginRight;
		rc.dy -= innerMarginTop + innerMarginBottom;
		layoutContainer->SetRect(rc);
	}

	if (scrollBar)
	{
		Rect rc = GetRect();
		rc.x = rc.dx - ECP_SCROLL_WIDTH;
		rc.dx = ECP_SCROLL_WIDTH;
		rc.y = 0;
		scrollBar->SetRect(rc);
	}
	if (layout)
	{
		ControlRect	glbr;
		glbr.x				=	0;
		glbr.y				=	0;
		glbr.maxDy			=	ControlRect::MIN_D;
		glbr.maxDy			=	ControlRect::MAX_D;
		glbr.minDx			=	GetRect().dx - (innerMarginLeft + innerMarginRight);
		glbr.maxDx			=	GetRect().dx - (innerMarginLeft + innerMarginRight);
		layout->SetControlRect(glbr);
		ScrollToFocus();
	}

}

void GUIListView::SetPrescroll( int newValue )
{
	prescroll = newValue;
}

void GUIListView::SetScrollSpeedModifer( int newValue )
{
	speedModifer = newValue;
}

void GUIListView::SetStartPosition( eStartPosition startPos )
{
	Rect r = layout->GetRect();
	if (startPos == ESP_TOP)
	{
		r.y = -r.dy;
	}
	else
	{
		r.y = layoutContainer->GetRect().dy;
		targetY = 0;
	}
	layout->SetRect(r);
}

void GUIListView::SetListAlign( eStartPosition align )
{
	itemsAlign = align;
}

void GUIListView::Reset()
{
	if (layout)
	{
		Rect rc = layout->GetRect();
		rc.y = 0;
		layout->SetRect(rc);
	}
	targetY = 0;
}

void GUIListView::Scroll( int32 delta )
{
	isScrolled = true;
	targetY -= delta;
	Rect viewRect;
	layoutContainer->GetScreenRect(viewRect);
	Rect layoutRect;
	layout->GetScreenRect(layoutRect);

	if (targetY > 0)
	{
		targetY = 0;
	}
	if (targetY < viewRect.dy - layoutRect.dy)
	{
		targetY = viewRect.dy - layoutRect.dy;
	}
}

void GUIListView::MoveToBottom()
{
	Rect viewRect;
	layoutContainer->GetScreenRect(viewRect);
	Rect layoutRect;
	layout->GetScreenRect(layoutRect);
	if (layoutRect.y + layoutRect.dy != viewRect.y + viewRect.dy
		&& (layoutRect.y > viewRect.y || !isScrolled))
	{
		targetY = viewRect.dy - layoutRect.dy;
	}
}

VList * GUIListView::GetListChilds( void )
{
	return layout->GetChilds();
}

void GUIListView::SetChildsMargins( int32 left, int32 right, int32 top, int32 bottom )
{
	layout->SetChildsMargins(left, right, top, bottom);
}

void GUIListView::ForceRecalc()
{
	layout->NeedRecalc();
	layout->Recalc();
}

void GUIListView::ScrollToFocus()
{
	CheckFocused();

	Rect rc			=	layout->GetRect();

	if (targetY != rc.y)
	{
		rc.y = targetY;
		layout->SetRect(rc);
	}

	scrollBar->SetPos(-rc.y);
}

