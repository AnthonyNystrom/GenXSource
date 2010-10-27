#include ".\guicontrolcontainer.h"
#include "VList.h"

GUIControlContainer::GUIControlContainer(GUIControlContainer *parent /* = NULL */, const ControlRect &cr /* = ControlRect */)
:	GUIControl(parent, cr)
,	stretchState(ECS_NOT_STRETCH)
{

}

GUIControlContainer::~GUIControlContainer(void)
{
	while (childs.Size())
	{
		GUIControl *c = (GUIControl*)(*childs.Begin());
		SAFE_DELETE(c);

		childs.Erase(childs.Begin());
	}
}

void GUIControlContainer::AddChild( GUIControl *child )
{
	if (child->pParent)
	{
		child->pParent->RemoveChild(child);
	}
	child->pParent = this;
	childs.PushBack(child);
	//Rect r = child->GetRect(); 
	//child->SetRect(r);
	child->RecalcDrawRect();

	if (stretchState)
	{
		stretchState = ECS_STRETCH_RECALC;
	}
}

void GUIControlContainer::InsertChild( GUIControl *child, GUIControl *next )
{
	if (child->pParent)
	{
		child->pParent->RemoveChild(child);
	}
	child->pParent = this;
	childs.Insert(childs.PosItem(next), child);
	//Rect r = child->GetRect(); 
	//child->SetRect(r);
	child->RecalcDrawRect();
	if (stretchState)
	{
		stretchState = ECS_STRETCH_RECALC;
	}
}

void GUIControlContainer::RemoveChild( GUIControl *child )
{
	child->Invalidate();
	childs.Erase(childs.PosItem(child));
	child->pParent = NULL;
	if (stretchState)
	{
		stretchState = ECS_STRETCH_RECALC;
	}
}

void GUIControlContainer::Update()
{
	//Restretch();
	if (stretchState == ECS_STRETCH_RECALC)
	{
		Restretch();
	}

	for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
	{
		((GUIControl*)(*it))->Update();
	}
}


VList * GUIControlContainer::GetChilds( void )
{
	return &childs;
}

const VList * GUIControlContainer::GetConstChilds( void )
{
	return &childs;
}

void GUIControlContainer::SetRect(const Rect &rc )
{
	GUIControl::SetRect(rc);
	//for (VList::Iterator it = childs.Begin(); it != childs.End(); it++)
	//{
	//	GUIControl *c = (GUIControl*)(*it);
	//	//Rect r = c->GetRect(); 
	//	//c->SetRect(r);
	//}
	RecalcDrawRect();
	if (stretchState)
	{
		stretchState = ECS_STRETCH_RECALC;
	}
}

void GUIControlContainer::Clear()
{
	if (childs.Empty())
	{
		return;
	}
	while (childs.Size())
	{
		GUIControl *c = (GUIControl*)(*childs.Begin());
		RemoveChild(c);
	}
}

GUIControl* GUIControlContainer::GetByID( int32 theID )
{
	GUIControl *retVal = GUIControl::GetByID(theID);
	if (retVal)
	{
		return retVal;
	}

	for (VList::Iterator it = childs.Begin(); it != childs.End(); it++)
	{
		GUIControl *c = (GUIControl*)(*it);
		retVal = c->GetByID(theID);
		if (retVal)
		{
			return retVal;
		}
	}

	return NULL;
}

void GUIControlContainer::SetStretch( eChildStretch stretch )
{
	stretchState = stretch;
	if (stretchState)
	{
		stretchState = ECS_STRETCH_RECALC;
	}
}

void GUIControlContainer::Restretch()
{
	if (stretchState != ECS_STRETCH_RECALC)
	{
		return;
	}

	stretchState = ECS_STRETCH;

	Rect curRect = GetRect();
	for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
	{
		GUIControl* control = (GUIControl*)(*it);
		ControlRect cntRect = control->GetControlRect();
		Rect rc;
		Rect conainerRect( control->GetMarginLeft()
						, control->GetMarginTop()
						, curRect.dx - control->GetMarginLeft() - control->GetMarginRight()
						, curRect.dy - control->GetMarginTop() - control->GetMarginBottom());

		rc.dx = MAX(cntRect.minDx, conainerRect.dx);
		rc.dx = MIN(cntRect.maxDx, rc.dx);

		rc.dy = MAX(cntRect.minDy, conainerRect.dy);
		rc.dy = MIN(cntRect.maxDy, rc.dy);

		if (rc.dx != curRect.dx)
		{
			rc.x = conainerRect.x;
			if (control->IsAlignType(EA_HCENTRE))
			{
				rc.x += (conainerRect.dx - rc.dx) / 2;
			}
			else if (control->IsAlignType(EA_RIGHT))
			{
				rc.x += conainerRect.dx - rc.dx;
			}
		}
		if (rc.dy != curRect.dy)
		{
			rc.y = conainerRect.y;
			if (control->IsAlignType(EA_VCENTRE))
			{
				rc.y += (conainerRect.dy - rc.dy) / 2;
			}
			else if (control->IsAlignType(EA_BOTTOM))
			{
				rc.y += conainerRect.dy - rc.dy;
			}
		}

		control->SetRect(rc);
	}
}

void GUIControlContainer::RecalcDrawRect()
{
	GUIControl::RecalcDrawRect();

	for (VList::Iterator it = childs.Begin(); it != childs.End(); it++)
	{
		GUIControl *c = (GUIControl*)(*it);
		c->RecalcDrawRect();
	}
}