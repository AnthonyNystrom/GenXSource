#include ".\guilayoutbox.h"

GUILayoutBox::GUILayoutBox(bool isVertical/* = true*/, GUIControlContainer *parent /* = NULL */
						   , const ControlRect &cr /* = ControlRect */, eGrowDirection growDir/* = EGD_INCREASE*/)
:	GUIControlContainer	(parent, cr)
,	recalc				(false)
,	vertical			(isVertical)
,	childsAlign			(EA_HCENTRE | EA_VCENTRE)
,	childsLeftMargin	(0)
,	childsRightMargin	(0)
,	childsTopMargin		(0)
,	childsBottomMargin	(0)
,	growDirection		(growDir)
{
}

GUILayoutBox::~GUILayoutBox(void)
{
}

void GUILayoutBox::SetMargins( int32 left, int32 right, int32 top, int32 bottom )
{
	GUIControlContainer::SetMargins(left, right, top, bottom);
	recalc = true;
}

void GUILayoutBox::SetChildsMargins( int32 left, int32 right, int32 top, int32 bottom )
{
	childsLeftMargin	= left;
	childsRightMargin	= right;
	childsTopMargin		= top;
	childsBottomMargin	= bottom;

	recalc = true;
}

void GUILayoutBox::AddChild( GUIControl *child )
{
	GUIControlContainer::AddChild(child);
	recalc = true;
}

void GUILayoutBox::InsertChild( GUIControl *child, GUIControl *next )
{
	GUIControlContainer::InsertChild(child, next);
	recalc = true;
}

void GUILayoutBox::RemoveChild( GUIControl *child )
{
	GUIControlContainer::RemoveChild(child);
	recalc = true;
}

void GUILayoutBox::Draw()
{
	Recalc();
	//GUIControl::Draw();
	GUIControlContainer::Draw();
}

void GUILayoutBox::Update()
{
	Recalc();
	GUIControlContainer::Update();
}

void GUILayoutBox::Recalc()
{
	if (!recalc)	return;

	//UTILS_TRACE("+++++++++++++++++++++++++++++++++ RECALC");

	Rect tr = GetRect(); // temp rect

	if (vertical)
	{
		// init minimal needed height of layout
		int32 minDy = childs.Size() * (childsTopMargin + childsBottomMargin);

		//UTILS_TRACE("++++++++++ RECALC set min sizes");

		for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
		{
			GUIControl *control = (GUIControl*)(*it);
			Rect cr = control->GetRect(); // child rect

			// calculate control width
			int32 dx = tr.dx - childsLeftMargin - childsRightMargin -
				control->leftMargin - control->rightMargin;
			cr.dx = MIN(control->controlRect.maxDx, MAX(dx, control->controlRect.minDx));

			// calculate control x-position
			{
				cr.x = 0;

				if (control->IsAlignType(EA_RIGHT))
					cr.x = dx - cr.dx;
				else if (control->IsAlignType(EA_HCENTRE))
					cr.x = (dx - cr.dx) / 2;

				// default - EA_LEFT
				cr.x += childsLeftMargin + control->leftMargin;
			}

			// calculate minimal needed height of layout
			cr.dy = control->controlRect.minDy;

			// init height of controls
			minDy += cr.dy + control->topMargin + control->bottomMargin;

			control->SetRect(cr);
		}
		//UTILS_TRACE("--------- RECALC set min sizes");

		int32 d;// = controlRect.minDy - minDy;

		if (minDy >= tr.dy && (growDirection == EGD_INCREASE || growDirection == EGD_BOTH)) // just increase layout height
		{
			//UTILS_TRACE("++++++++++ RECALC inc layout height");
			tr.dy = MAX(MIN(controlRect.maxDy, minDy), tr.dy);
			//UTILS_TRACE("--------- RECALC inc layout height");
		}
		else if (minDy < tr.dy && (growDirection == EGD_DECREASE || growDirection == EGD_BOTH))
		{
			//UTILS_TRACE("++++++++++ RECALC inc layout height");
			tr.dy = MIN(MAX(controlRect.minDy, minDy), tr.dy);
			//UTILS_TRACE("--------- RECALC inc layout height");
		}
		d = tr.dy - minDy;


		if( d > 0 )// try to increase heights of controls
		{
			//UTILS_TRACE("++++++++++ RECALC inc controls height");
			while (d)
			{
				bool isInc = false;

				for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
				{
					GUIControl *control = (GUIControl*)(*it);
					Rect cr = control->GetRect(); // child rect

					if (cr.dy < control->controlRect.maxDy)
					{
						cr.dy++;
						--d;
						isInc = true;
					}
                    
					control->SetRect(cr);
					if (!d)	break;
				}

				if (!isInc)	break;
			}
			//UTILS_TRACE("--------- RECALC inc controls height");
		}

		// calculate vertical controls position
		{
			//UTILS_TRACE("++++++++++ RECALC vertical pos");
			int32 pos = 0; // default - EA_TOP

			if (IsChildsAlignType(EA_BOTTOM))
				pos = d;
			else if (IsChildsAlignType(EA_VCENTRE))
				pos = d / 2;

			for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
			{
				GUIControl *control = (GUIControl*)(*it);
				Rect cr = control->GetRect(); // child rect

				pos += childsTopMargin + control->topMargin;
				cr.y = pos;
				pos += cr.dy + control->bottomMargin + childsBottomMargin;

				control->SetRect(cr);
			}
			//UTILS_TRACE("--------- RECALC vertical pos");
		}
	}
	else
	{
		// init minimal needed width of layout
		int32 minDx = childs.Size() * (childsLeftMargin + childsRightMargin);

		for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
		{
			GUIControl *control = (GUIControl*)(*it);
			Rect cr = control->GetRect(); // child rect

			// calculate control height
			int32 dy = tr.dy - childsTopMargin - childsBottomMargin -
				control->topMargin - control->bottomMargin;
			cr.dy = MIN(control->controlRect.maxDy, MAX(dy, control->controlRect.minDy));
			
			// calculate control y-position
			{
				cr.y = 0;

				if (control->IsAlignType(EA_BOTTOM))
					cr.y = dy - cr.dy;
				else if (control->IsAlignType(EA_VCENTRE))
					cr.y = (dy - cr.dy) / 2;

				// default - EA_TOP
				cr.y += childsTopMargin + control->topMargin;
			}

			// calculate minimal needed width of layout
			cr.dx = control->controlRect.minDx;

			// init width of controls
			minDx += cr.dx + control->leftMargin + control->rightMargin;

			control->SetRect(cr);
		}

		int32 d;// = controlRect.minDy - minDy;

		if (minDx >= tr.dx && (growDirection == EGD_INCREASE || growDirection == EGD_BOTH)) // just increase layout height
		{
			//UTILS_TRACE("++++++++++ RECALC inc layout height");
			tr.dx = MAX(MIN(controlRect.maxDx, minDx), tr.dx);
			//UTILS_TRACE("--------- RECALC inc layout height");
		}
		else if (minDx < tr.dx && (growDirection == EGD_DECREASE || growDirection == EGD_BOTH))
		{
			//UTILS_TRACE("++++++++++ RECALC inc layout height");
			tr.dx = MIN(MAX(controlRect.minDx, minDx), tr.dx);
			//UTILS_TRACE("--------- RECALC inc layout height");
		}
		d = tr.dx - minDx;


		if( d > 0 )// try to increase heights of controls
		{
			while (d)
			{
				bool isInc = false;

				for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
				{
					GUIControl *control = (GUIControl*)(*it);
					Rect cr = control->GetRect(); // child rect

					if (cr.dx < control->controlRect.maxDx)
					{
						cr.dx++;
						--d;
						isInc = true;
					}

                    control->SetRect(cr);
					if (!d)	break;
				}

				if (!isInc)	break;
			}
		}

		// calculate vertical controls position
		{
			int32 pos = 0; // default - EA_LEFT

			if (IsChildsAlignType(EA_RIGHT))
				pos = d;
			else if (IsChildsAlignType(EA_HCENTRE))
				pos = d / 2;

			for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
			{
				GUIControl *control = (GUIControl*)(*it);
				Rect cr = control->GetRect(); // child rect

				pos += childsLeftMargin + control->leftMargin;
				cr.x = pos;
				pos += cr.dx + control->rightMargin + childsRightMargin;

				control->SetRect(cr);
			}
		}
		for (VList::Iterator it = childs.Begin(); it != childs.End(); ++it)
		{
			GUIControl *control = (GUIControl*)(*it);
			UTILS_TRACE("Control %d   dx = %d", control->GetID(), control->GetRect().dx);
		}
	}

	SetRect(tr);

	//UTILS_TRACE("--------------------------------- RECALC");
	recalc = false;
}

void GUILayoutBox::SetControlRect(const ControlRect &cr)
{
	GUIControlContainer::SetControlRect(cr);
	recalc	=	true;
}

void GUILayoutBox::SetGrowDirection( eGrowDirection growDir )
{
	growDirection = growDir;
	recalc = true;
}

void GUILayoutBox::SetDirection(bool isVertical)
{
	vertical	=	isVertical;

	recalc		=	true;
	
}