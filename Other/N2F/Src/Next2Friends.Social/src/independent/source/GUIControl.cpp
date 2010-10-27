#include ".\guicontrol.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "Utils.h"
#include "VList.h"
#include "Graphics.h"
#include "GUIControlContainer.h"

GUIControl::GUIControl(GUIControlContainer *parent /* = NULL */, const ControlRect &cr /* = ControlRect */,
					   bool selectable /* = false */)
:	controlRect		(cr)
,	rect			(cr.x, cr.y, cr.minDx, cr.minDy)
,	id				(0)
,	pParent			(NULL)
,	drawType		(EDT_CONTROL)
,	isSelectable	(selectable)
,	leftMargin		(0)
,	rightMargin		(0)
,	topMargin		(0)
,	bottomMargin	(0)
,	align			(EA_LEFT | EA_TOP)
{
	//UTILS_LOG(EDMP_DEBUG, "constructor x=%d, y=%d, minx=%d, miny=%d,  maxx=%d, maxy=%d", cr.x, cr.y, cr.minDx, cr.minDy, cr.maxDx, cr.maxDy);
	//UTILS_LOG(EDMP_DEBUG, "constructor x=%d, y=%d, dx=%d, dy=%d", rect.x, rect.y, rect.dx, rect.dy);

	//UTILS_LOG(EDMP_DEBUG, "!!!before set parent");
	SetParent(parent);
	//CalcDrawRect(outputRect);
	//UTILS_LOG(EDMP_DEBUG, "!!!after set parent");
	Invalidate();
}

GUIControl::~GUIControl(void)
{
}

void GUIControl::GetScreenRect( Rect & rc )
{
	if (pParent)
	{
		pParent->GetScreenRect(rc);
		rc.x += rect.x;
		rc.y += rect.y;
		rc.dx = rect.dx;
		rc.dy = rect.dy;
	}
	else
	{
		rc = rect;
	}
}

void GUIControl::Update()
{
}

void GUIControl::Draw()
{
	Rect rc;
	GetScreenRect(rc);
	GUISystem::Instance()->GetSkin()->DrawControl(drawType, rc);

	//UTILS_TRACE("DRAW control 0x%X", (uint32)this);

}

void GUIControl::SetParent( GUIControlContainer *parent, GUIControl *nextBy /*= NULL*/ )
{
	if (pParent)
	{
		pParent->RemoveChild(this);
	}
	if (parent)
	{
		FASSERT(parent != this);

		if (!nextBy)
		{
			parent->AddChild(this);
		}
		else
		{
			parent->InsertChild(this, nextBy);
		}
	}
	else
	{
		//Rect r = GetRect(); 
		////UTILS_LOG(EDMP_DEBUG, "r x=%d, y=%d, dx=%d, dy=%d", r.x, r.y, r.dx, r.dy);
		//SetRect(r);
		RecalcDrawRect();
	}
}

void GUIControl::Invalidate()
{
	GUISystem::Instance()->InvalidateRect(outputRect, this);
}

void GUIControl::SetRect(const Rect &rc )
{
	//UTILS_LOG(EDMP_DEBUG, "set rect rc x=%d, y=%d, dx=%d, dy=%d", rc.x, rc.y, rc.dx, rc.dy);
	//UTILS_LOG(EDMP_DEBUG, "set rect rect x=%d, y=%d, dx=%d, dy=%d", rect.x, rect.y, rect.dx, rect.dy);
	Invalidate();
	rect = rc;
	controlRect.x = rc.x;
	controlRect.y = rc.y;
	//UTILS_LOG(EDMP_DEBUG, "set rect control %d x=%d, y=%d, dx=%d, dy=%d", GetDrawType(),rect.x, rect.y, rect.dx, rect.dy);

	CalcDrawRect(outputRect);
	Invalidate();
	if (GUISystem::Instance()->GetFocus() == this)
	{
		GUISystem::Instance()->RecalcCursor();
	}
}

void GUIControl::AddChild( GUIControl *child )
{
	FASSERT(false);
}

void GUIControl::RemoveChild( GUIControl *child )
{
	FASSERT(false);
}

void GUIControl::SetDrawType( eDrawType newType )
{
	if (drawType != newType)
	{
		Invalidate();
	}
	drawType = newType;
}

void GUIControl::SetSelectable( bool selectable )
{
	isSelectable = selectable;
	Invalidate();
}

Rect GUIControl::GetRect()
{
	return rect;
}

GUIControl * GUIControl::GetParent()
{
	return pParent;
}

void GUIControl::CalcDrawRect( Rect &rc )
{
	if (pParent)
	{
		pParent->CalcDrawRect(rc);
		Rect scrR;
		GetScreenRect(scrR);
		rc.Intersect(scrR);
	}
	else
	{
		rc = rect;
	}
	//UTILS_LOG(EDMP_DEBUG, "calc draw rect control %d x=%d, y=%d, dx=%d, dy=%d", GetDrawType(), rc.x, rc.y, rc.dx, rc.dy);
}

VList * GUIControl::GetChilds( void )
{
	return NULL;
}

Rect GUIControl::GetOutputRect()
{
	return outputRect;
}

bool GUIControl::IsSelectable()
{
	return isSelectable;
}

void GUIControl::OnSetFocus()
{
	Invalidate();
}

void GUIControl::OnLostFocus()
{
	Invalidate();
}

void GUIControl::DrawFinished()
{

}

const VList * GUIControl::GetConstChilds( void )
{
	return NULL;
}

void GUIControl::SetMargins( int32 left, int32 right, int32 top, int32 bottom )
{
	leftMargin		= left;
	rightMargin		= right;
	topMargin		= top;
	bottomMargin	= bottom;
}

void GUIControl::SetControlRect(const ControlRect &cr)
{
	Rect rc(cr.x, cr.y, cr.minDx, cr.minDy);
// 	SetRect(Rect(cr.x, cr.y, cr.minDx, cr.minDy));
	SetRect(rc);
	controlRect	=	cr;
}

ControlRect GUIControl::GetControlRect()
{
	return	controlRect;
}

GUIControl* GUIControl::GetByID( int32 theID )
{
	if (theID == id)
	{
		return this;
	}
	return NULL;
}

int32 GUIControl::GetID()
{
	return id;
}

void GUIControl::SetID( int32 newID )
{
	id = newID;
}

void GUIControl::SetAlignType( int32 a )
{
	align = a;
}

int32 GUIControl::GetAlignType()
{
	return align;
}

bool GUIControl::IsAlignType( int32 a )
{
	return (align & a) == a;
}

eDrawType GUIControl::GetDrawType()
{
	return drawType;
}

int32 GUIControl::GetMarginLeft()
{
	return leftMargin;
}

int32 GUIControl::GetMarginRight()
{
	return rightMargin;
}

int32 GUIControl::GetMarginTop()
{
	return topMargin;
}

int32 GUIControl::GetMarginBottom()
{
	return bottomMargin;
}

void GUIControl::RecalcDrawRect()
{
	CalcDrawRect(outputRect);
	if (GUISystem::Instance()->GetFocus() == this)
	{
		GUISystem::Instance()->NeedToRecalcCursor();
		GUISystem::Instance()->RecalcCursor();
	}
}