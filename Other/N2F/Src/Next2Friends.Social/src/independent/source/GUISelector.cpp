#include "guiselector.h"
#include "GUIData.h"
#include "GUIControlContainer.h"
#include "GUILayoutBox.h"
#include "GUIImage.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"


GUISelector::GUISelector( GUIControlContainer *parent /*= NULL*/, const ControlRect &rc /*= ControlRect()*/ )
:	GUILayoutBox(false, parent, rc)
{
	SetSelectable(true);

	leftArrow = new GUIImage();
	leftArrow->SetAlignType(GUIControl::EA_HCENTRE|GUIControl::EA_VCENTRE);
	//leftArrow->SetMargins(2, 0, 0, 0);
	GUILayoutBox::AddChild(leftArrow);

	pSelectorContainer = new GUIControlContainer();
	GUILayoutBox::AddChild(pSelectorContainer);
	pLayout = new GUILayoutBox(false, pSelectorContainer, ControlRect(), EGD_BOTH);
	pLayout->SetDrawType(EDT_NONE);
	pSelected = NULL;

	rightArrow = new GUIImage();
	rightArrow->SetAlignType(GUIControl::EA_HCENTRE|GUIControl::EA_VCENTRE);
	//rightArrow->SetMargins(0, 2, 0, 0);
	GUILayoutBox::AddChild(rightArrow);


	isCycling = false;
	
}

GUISelector::~GUISelector(void)
{
}

void GUISelector::Update()
{

	Rect rc = pLayout->GetRect();
	if (GUISystem::Instance()->GetFocus() == this)
	{
		Application *pApp = GUISystem::Instance()->GetApp();
		if (pApp->IsKeyDown(Application::EKC_RIGHT) || pApp->IsKeyRepeat(Application::EKC_RIGHT))
		{
			if (isRight)
			{
				//UTILS_TRACE("======================= RIGHT selector id = %d", GetID());
				VList::Iterator it = pLayout->GetChilds()->PosItem(pSelected);
				it++;
				pSelected = (GUIControl*)(*it);
				RecalcArrows();
				EventData ed((uint32)this);
				DispatchEvent(EEI_SELECTOR_CHANGED, &ed);
			}
			else if (isCycling)
			{
				VList::Iterator it = pLayout->GetChilds()->Begin();
				pSelected = (GUIControl*)(*it);
				Rect layoutRect = pLayout->GetRect();
				layoutRect.x = pSelected->GetRect().dx;
				pLayout->SetRect(layoutRect);
				RecalcArrows();
				EventData ed((uint32)this);
				DispatchEvent(EEI_SELECTOR_CHANGED, &ed);
			}
			else
			{
				GUISystem::Instance()->ProcessKey(Application::EKC_RIGHT);
			}

		}
		if (pApp->IsKeyDown(Application::EKC_LEFT)  || pApp->IsKeyRepeat(Application::EKC_LEFT))
		{
			if (isLeft)
			{
				//UTILS_TRACE("======================== LEFT selector id = %d", GetID());
				VList::Iterator it = pLayout->GetChilds()->PosItem(pSelected);
				it--;
				pSelected = (GUIControl*)(*it);
				RecalcArrows();
				EventData ed((uint32)this);
				DispatchEvent(EEI_SELECTOR_CHANGED, &ed);
			}
			else if (isCycling)
			{
				VList::Iterator it = pLayout->GetChilds()->Begin();
				VList::Iterator nIt = it;
				while(++it != pLayout->GetChilds()->End())
				{
					nIt = it;
				}
				pSelected = (GUIControl*)(*nIt);
				Rect layoutRect = pLayout->GetRect();
				layoutRect.x = -layoutRect.dx;//pSelected->GetRect().dx;
				pLayout->SetRect(layoutRect);
				RecalcArrows();
				EventData ed((uint32)this);
				DispatchEvent(EEI_SELECTOR_CHANGED, &ed);
			}
			else
			{
				GUISystem::Instance()->ProcessKey(Application::EKC_LEFT);
			}
		}

		if (pApp->IsKeyDown(Application::EKC_UP)  || pApp->IsKeyRepeat(Application::EKC_UP))
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_UP);
		}
		if (pApp->IsKeyDown(Application::EKC_DOWN)  || pApp->IsKeyRepeat(Application::EKC_DOWN))
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_DOWN);
		}
		if (pApp->IsKeyDown(Application::EKC_CLR)  || pApp->IsKeyRepeat(Application::EKC_CLR))
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_CLR);
		}
		if (pApp->IsKeyDown(Application::EKC_SELECT)  || pApp->IsKeyRepeat(Application::EKC_SELECT))
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_SELECT);
		}
	}
	GUIControlContainer::Update();

	if (pSelected)
	{
		Rect layoutRect = pLayout->GetRect();
		Rect selectedRect = pSelected->GetRect();
		if (selectedRect.x  + layoutRect.x != 0)
		{
			int32 spd = -(selectedRect.x + layoutRect.x) / 3;
			if (selectedRect.x + layoutRect.x > 0)
			{
				spd -= 3;
				layoutRect.x += spd;
				if (selectedRect.x + layoutRect.x < 0)
				{
					layoutRect.x = -selectedRect.x;
				}
			}
			else
			{
				spd += 3;
				layoutRect.x += spd;
				if (selectedRect.x + layoutRect.x > 0)
				{
					layoutRect.x = -selectedRect.x;
				}
			}
			pLayout->SetRect(layoutRect);
		}
	}
}





void GUISelector::OnSetFocus()
{
	GUILayoutBox::SetDrawType(selectedDrawType);
	GUISystem::Instance()->LockKeyboard();
	RecalcArrows();
	VList *ch = pLayout->GetChilds();
	for (VList::Iterator it = ch->Begin(); it != ch->End(); it++)
	{
		GUIControl *ptl = (GUIControl*)(*it);
		ptl->SetDrawType(selectedChildType);
	}

}

void GUISelector::OnLostFocus()
{
	GUILayoutBox::SetDrawType(unselectedDrawType);
	GUISystem::Instance()->UnlockKeyboard();
	RecalcArrows();
	VList *ch = pLayout->GetChilds();
	for (VList::Iterator it = ch->Begin(); it != ch->End(); it++)
	{
		GUIControl *ptl = (GUIControl*)(*it);
		ptl->SetDrawType(unselectedChildType);
	}
}

void GUISelector::AddChild( GUIControl *child )
{
	child->SetControlRect(ControlRect(0, 0, GetRect().dx, GetRect().dy));
	child->SetParent(pLayout);

	if (!pSelected)
	{
		pSelected = child;
	}
	RecalcArrows();
	Invalidate();
}

void GUISelector::RemoveChild( GUIControl *child )
{
	VList *ch = pLayout->GetChilds();
	pLayout->RemoveChild(child);
	if (ch->Empty())
	{
		pSelected = NULL;
	}
	else
	{
		pSelected = (GUIControl*)(*ch->Begin());
	}

	RecalcArrows();
	Invalidate();
}

void GUISelector::InsertChild( GUIControl *child, GUIControl *next )
{
	child->SetControlRect(ControlRect(0, 0, GetRect().dx, GetRect().dy));
	pLayout->InsertChild(child, next);

	RecalcArrows();
	Invalidate();
}

void GUISelector::RecalcArrows()
{
	isLeft = false;
	isRight = false;
	if (!pSelected)
	{
		return;
	}

	VList::Iterator it = pLayout->GetChilds()->PosItem(pSelected);
	if (it != pLayout->GetChilds()->Begin())
	{
		isLeft = true;
		if (GUISystem::Instance()->GetFocus() == this)
		{
			SetImage(leftSelectedID, leftArrow);
		}
		else
		{
			SetImage(leftUnselectedID, leftArrow);
		}
	}
	else
	{
		SetImage(leftNoneID, leftArrow);
	}
	it++;
	if (it != pLayout->GetChilds()->End())
	{
		isRight = true;
		if (GUISystem::Instance()->GetFocus() == this)
		{
			SetImage(rightSelectedID, rightArrow);
		}
		else
		{
			SetImage(rightUnselectedID, rightArrow);
		}
	}
	else
	{
		SetImage(rightNoneID, rightArrow);
	}
}

void GUISelector::SetRect(const Rect &rc )
{
	if (rc == GetRect())
	{
		GUILayoutBox::SetRect(rc);
		return;
	}
	GUILayoutBox::SetRect(rc);
	//UTILS_TRACE("=============================== id = %d             rect x=%d, y=%d, dx=%d, dy=%d", GetID(), rc.x, rc.y, rc.dx, rc.dy);
	VList *ch = pLayout->GetChilds();
	int32 cdx = rc.dx-leftArrow->GetImageWidth() - rightArrow->GetImageWidth()
		- leftArrow->GetMarginLeft() - leftArrow->GetMarginRight()
		- rightArrow->GetMarginLeft() - rightArrow->GetMarginRight();
	pSelectorContainer->SetControlRect(ControlRect(0, 0, cdx, rc.dy));
	for (VList::Iterator it = ch->Begin(); it != ch->End(); it++)
	{
		GUIControl *ptl = (GUIControl*)(*it);
		ControlRect c = ptl->GetControlRect();
		c.maxDx = c.minDx = cdx;
		c.maxDy = c.minDy = rc.dy;
		ptl->SetControlRect(c);
	}
	pLayout->SetControlRect(ControlRect(0, 0, cdx, rc.dy, ControlRect::MAX_D, ControlRect::MAX_D));
}

GUIControl* GUISelector::GetSelected()
{
	return pSelected;
}

int32 GUISelector::GetSelectedIndex()
{
	int i = 0;
	for (VList::Iterator it = pLayout->GetChilds()->Begin(); it != pLayout->GetChilds()->End(); it++)
	{
		if ((GUIControl*)(*it) == pSelected)
		{
			return i;
		}
		i++;
	}
	return 0;
}
void GUISelector::SetSelected( GUIControl* pSel )
{
	pSelected = pSel;
	RecalcArrows();
	Invalidate();
	EventData ed((uint32)this);
	DispatchEvent(EEI_SELECTOR_CHANGED, &ed);
}

void GUISelector::SetSelectedIndex( int32 selected )
{
	pSelected = (GUIControl*)(*pLayout->GetChilds()->Pos(selected));
	RecalcArrows();
	Invalidate();
	EventData ed((uint32)this);
	DispatchEvent(EEI_SELECTOR_CHANGED, &ed);
}

VList * GUISelector::GetSelectorChilds()
{
	return pLayout->GetChilds();
}

void GUISelector::SetLeftArrow( int16 selectedID, int16 unselectedID, int16 noneID )
{
	leftSelectedID = selectedID;
	leftUnselectedID = unselectedID;
	leftNoneID = noneID;
	RecalcArrows();
}

void GUISelector::SetRightArrow( int16 selectedID, int16 unselectedID, int16 noneID )
{
	rightSelectedID = selectedID;
	rightUnselectedID = unselectedID;
	rightNoneID = noneID;
	RecalcArrows();
}

void GUISelector::SetDrawType( eDrawType newType )
{
	unselectedDrawType = newType;
	GUILayoutBox::SetDrawType(unselectedDrawType);
	ControlRect cr = GetControlRect();
	cr.minDy = cr.maxDy = GUISystem::Instance()->GetSkin()->GetSprite(unselectedDrawType, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	SetControlRect(cr);
}

void GUISelector::SetSelectedDrawType( eDrawType newType )
{
	selectedDrawType = newType;
	ControlRect cr = GetControlRect();
	cr.minDy = cr.maxDy = GUISystem::Instance()->GetSkin()->GetSprite(selectedDrawType, GUISkinLocal::ESSH3_CENTER)->GetHeight();
	SetControlRect(cr);
}

void GUISelector::SetImage( int16 imageID, GUIImage *img )
{
	img->SetImage(imageID);
	ControlRect cr = img->GetControlRect();
	cr.minDx = cr.maxDx = img->GetImageWidth();
	cr.minDy = cr.maxDy = img->GetImageHeight();

	img->SetControlRect(cr);
	img->SetAnimationRange(0, img->GetFrameCount() - 1);
	img->StartAnimation();
}


void GUISelector::SetChildsDrawType( eDrawType selected, eDrawType unselected )
{
	selectedChildType = selected;
	unselectedChildType = unselected;
}
//void GUISelector::Draw()
//{
//	//UTILS_TRACE("================================== draw selector id = %d", GetID());
//	GUIControlContainer::Draw();
//}




