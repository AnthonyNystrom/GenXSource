#include "guisystem.h"
#include "GUISkinLocal.h"
#include "Application.h"
#include "Graphics.h"
#include "GUIControl.h"
#include "Application.h"
#include "Sprite.h"

GUISystem::GUISystem(void)
{
	pApp = GetApplication();
	pApp->SetGlobalPointer(0, this);
	pSkin = new GUISkinLocal();
	tempRectCounter = 0;
	isKeyboardLocked = 0;
	pBackSprite = NULL;
	pBackSurface = NULL;
	graphicSystem = pApp->GetGraphicsSystem();
	screenRect.dx = graphicSystem->GetWidth();
	screenRect.dy = graphicSystem->GetHeight();
	screenRect.x = 0;
	screenRect.y = 0;
	isAllInvalid = false;

	isCycling = false;

	isBackSurface = false;

	transitionList.PushBack(&transInvalidRect);

	pBuffer = graphicSystem->CreateNativeSurface((uint16)screenRect.dx, (uint16)screenRect.dy);
	pSurfFrom = graphicSystem->CreateNativeSurface((uint16)screenRect.dx, (uint16)screenRect.dy);
	pSurfTo = graphicSystem->CreateNativeSurface((uint16)screenRect.dx, (uint16)screenRect.dy);

	waitForEvent = EEI_NONE;

	ForceInvalidate();
}

GUISystem::~GUISystem(void)
{
	UTILS_TRACE("+++++++++++GUISystem Destructor");
	graphicSystem->ReleaseNativeSurface(pBuffer);
	graphicSystem->ReleaseNativeSurface(pSurfFrom);
	graphicSystem->ReleaseNativeSurface(pSurfTo);
	delete pSkin;
	windows.Clear();
	SAFE_RELEASE(pBackSprite);
	UTILS_TRACE("------------GUISystem Destructor");
}

GUISkinLocal * GUISystem::GetSkin()
{
	return pSkin;
}

void GUISystem::Update()
{
#ifdef DEBUG_DRAW
	if (!pApp->IsKeyUp(Application::EKC_SEND) && !pApp->IsKeyUp(Application::EKC_SOFT1))
	{
		return;
	}
#endif

	if (isTransition)
	{
		TransitionUpdate();
		return;
	}

//process keys if keyboard unlocked
	ProcessKeyboard();
//---------------------------------

//update all active windows
	if (!windows.Empty())
	{
		VList::Iterator it = windows.End();
		it--;
		for (; it != windows.End(); --it)
		{
			((GUIControl*)(*it))->Update();
		}
	}
//---------------------------------
	UpdateFocus();
}




void GUISystem::Draw()
{

	if (isTransition)
	{
		TransitionDraw();
		return;
	}

	//if (isFreeze)
	//{
	//	ProcessFreeze();
	//}

#ifdef DEBUG_DRAW
	if (!pApp->IsKeyUp(Application::EKC_SEND)/* && !pApp->IsKeyUp(Application::EKC_SOFT1)*/)
	{
		return;
	}
	graphicSystem->SetClip();
	graphicSystem->SetColor(255, 255, 255);
	graphicSystem->FillRect(0, 0, 500, 500);
	if (invalidActiveRects.Begin() != invalidActiveRects.End())
	{
		UTILS_TRACE("-------   Start draw   --------");
	}
#endif// DEBUG_DRAW 

//prepare invalidate rects for correct drawing
#ifdef DEBUG_DRAW
	UTILS_TRACE("----------------- before union");
	for (VList::Iterator it = invalidActiveRects.Begin(); it != invalidActiveRects.End(); it++)
	{
		Rect r = *((Rect*)(*it));
		UTILS_TRACE("rect  x1=%d, y1=%d, x2=%d, y2=%d", r.x, r.y, r.x+r.dx, r.y+r.dy);
	}
#endif// DEBUG_DRAW

	//UTILS_LOG(EDMP_DEBUG, "++++++++ Start draw +++++++++");

	for (VList::Iterator it1 = invalidInactiveRects.Begin(); it1 != invalidInactiveRects.End(); it1++)
	{
		InvalidRect *r1 = ((InvalidRect*)(*it1));
		for (VList::Iterator it2 = invalidInactiveRects.Begin(); it2 != invalidInactiveRects.End();)
		{
			if (it1 == it2)
			{
				it2++;
				continue;
			}
			InvalidRect *r2 = ((InvalidRect*)(*it2));
			Rect rect = r2->rect;
			rect.Intersect(r1->rect);
			if (!rect.IsEmpty())
			{
				r1->rect.Union(r2->rect);
				invalidInactiveRects.Erase(it2);
				it2 = invalidInactiveRects.Begin();
				it1 = invalidInactiveRects.Begin();
				r1 = ((InvalidRect*)(*it1));
#ifdef DEBUG_DRAW
				UTILS_TRACE("after union ------------");
				for (VList::Iterator it = invalidInactiveRects.Begin(); it != invalidInactiveRects.End(); it++)
				{
					Rect r = ((InvalidRect*)(*it))->rect;
					UTILS_TRACE("rect  x1=%d, y1=%d, x2=%d, y2=%d", r.x, r.y, r.x+r.dx, r.y+r.dy);
				}
#endif// DEBUG_DRAW
				continue;
			}
			it2++;
		}
	}
//all invalidate rects ready

	VList::Iterator winIt;
	graphicSystem->SetCurrentSurface(pBuffer);
	for (VList::Iterator it = invalidInactiveRects.Begin(); it != invalidInactiveRects.End(); it++)
	{
		InvalidRect *r = ((InvalidRect*)(*it));
		graphicSystem->SetClip(r->rect.x, r->rect.y, r->rect.dx, r->rect.dy);
		DrawBackground();
	}

	//draw back buffer
	if (!windows.Empty())
	{
		winIt = windows.End();
		winIt--;
		VList::Iterator stopIt = activeIt;
		for (; winIt != stopIt; --winIt)
		{
			DrawControl(((GUIControl*)(*winIt)), &invalidInactiveRects);
		}
	}
	graphicSystem->SetCurrentSurface(NULL);



//теперь объеденим все инвалидные области, не зависимо от того были они на активном слое или нет
	for (VList::Iterator it1 = invalidActiveRects.Begin(); it1 != invalidActiveRects.End(); it1++)
	{
		invalidInactiveRects.PushBack((GUIControl*)(*it1));
	}

	for (VList::Iterator it1 = invalidInactiveRects.Begin(); it1 != invalidInactiveRects.End(); it1++)
	{
		InvalidRect *r1 = ((InvalidRect*)(*it1));
		for (VList::Iterator it2 = invalidInactiveRects.Begin(); it2 != invalidInactiveRects.End();)
		{
			if (it1 == it2)
			{
				it2++;
				continue;
			}
			InvalidRect *r2 = ((InvalidRect*)(*it2));
			Rect rect = r2->rect;
			rect.Intersect(r1->rect);
			if (!rect.IsEmpty())
			{
				r1->rect.Union(r2->rect);
				invalidInactiveRects.Erase(it2);
				it2 = invalidInactiveRects.Begin();
				it1 = invalidInactiveRects.Begin();
				r1 = ((InvalidRect*)(*it1));
#ifdef DEBUG_DRAW
				UTILS_TRACE("after union ------------");
				for (VList::Iterator it = invalidInactiveRects.Begin(); it != invalidInactiveRects.End(); it++)
				{
					Rect r = ((InvalidRect*)(*it))->rect;
					UTILS_TRACE("rect  x1=%d, y1=%d, x2=%d, y2=%d", r.x, r.y, r.x+r.dx, r.y+r.dy);
				}
#endif// DEBUG_DRAW
				continue;
			}
			it2++;
		}
	}

	//draw back buffer
	for (VList::Iterator it = invalidInactiveRects.Begin(); it != invalidInactiveRects.End(); it++)
	{
		InvalidRect *r = ((InvalidRect*)(*it));
		graphicSystem->SetClip(r->rect.x, r->rect.y, r->rect.dx, r->rect.dy);
		graphicSystem->DrawSurface(pBuffer, 0, 0, false, 0, 0);
		//UTILS_LOG(EDMP_DEBUG, "Draw buffer to screen");
		//UTILS_LOG(EDMP_DEBUG, "x=%d, y=%d, dx=%d, dy=%d", r->rect.x, r->rect.y, r->rect.dx, r->rect.dy);
	}
	//---------------------
	if (!windows.Empty())
	{
		for (; winIt != windows.End(); --winIt)
		{
			DrawControl(((GUIControl*)(*winIt)), &invalidInactiveRects);
		}
	}

#ifdef DEBUG_DRAW
	graphicSystem->SetClip();
	graphicSystem->SetColor(255, 0, 0);
	for (VList::Iterator begin = invalidInactiveRects.Begin(); begin != invalidInactiveRects.End(); ++begin)
	{
		Rect r = *((Rect*)(*begin));
		graphicSystem->DrawRect(r.x, r.y, r.dx, r.dy);
	}
#endif// DEBUG_DRAW

	invalidInactiveRects.Clear();
	invalidActiveRects.Clear();
	tempRectCounter = 0;
	isAllInvalid = false;
	//UTILS_LOG(EDMP_DEBUG, "--------  End draw  -----------");
}


void GUISystem::DrawControl( GUIControl *control, VList *pRects)
{
	Rect rect = control->GetOutputRect();
	//UTILS_LOG(EDMP_DEBUG, "+++++++++ draw %d", control->GetDrawType());
	//UTILS_LOG(EDMP_DEBUG, "out rect x=%d, y=%d, dx=%d, dy=%d", rect.x, rect.y, rect.dx, rect.dy);

	bool isDrawn = false;
	Rect _clipRect;
	for (VList::Iterator begin = pRects->Begin(); begin != pRects->End(); ++begin)
	{
		_clipRect = rect;
		_clipRect.Intersect(((InvalidRect*)(*begin))->rect);
		if ( !_clipRect.dx || !_clipRect.dy )
		{
			continue;
		}
		//UTILS_LOG(EDMP_DEBUG, "set clip x=%d, y=%d, dx=%d, dy=%d", _clipRect.x, _clipRect.y, _clipRect.dx, _clipRect.dy);
		graphicSystem->SetClip(_clipRect.x, _clipRect.y, _clipRect.dx, _clipRect.dy);
		control->Draw();
		isDrawn = true;
	}
	VList *chList = control->GetChilds();
	if (isDrawn && chList)
	{
		for (VList::Iterator chIt = chList->Begin(); chIt != chList->End(); ++chIt)
		{
			//UTILS_LOG(EDMP_DEBUG, "draw child %d", ((GUIControl*)(*chIt))->GetDrawType());
			DrawControl(((GUIControl*)(*chIt)), pRects);
		}
	}

	if (isDrawn)
	{
		for (VList::Iterator begin = pRects->Begin(); begin != pRects->End(); ++begin)
		{
			_clipRect = rect;
			_clipRect.Intersect(((InvalidRect*)(*begin))->rect);
			if ( !_clipRect.dx || !_clipRect.dy )
			{
				continue;
			}
			//UTILS_LOG(EDMP_DEBUG, "after draw x=%d, y=%d, dx=%d, dy=%d", _clipRect.x, _clipRect.y, _clipRect.dx, _clipRect.dy);
			graphicSystem->SetClip(_clipRect.x, _clipRect.y, _clipRect.dx, _clipRect.dy);
			control->DrawFinished();
		}
	}
	//UTILS_LOG(EDMP_DEBUG, "-------- draw %d", control->GetDrawType());
}

GUISystem* GUISystem::Instance()
{
	return (GUISystem*) GetApplication()->GetGlobalPointer(0);
}

void GUISystem::InvalidateRect(const Rect & newRect, GUIControl *owner )
{

	FASSERT(owner);
	if (newRect.dx <= 0 || newRect.dy <= 0 || isAllInvalid)
		return;
	
	Rect inRect = newRect;
	inRect.Intersect(screenRect);
	if (inRect.IsEmpty())
	{
		return;
	}


	VList *pRects = &invalidInactiveRects;


	do 
	{
		GUIControl *newOwner = owner->GetParent();
		if (!newOwner)
		{
			for (VList::Iterator it = activeIt; it != windows.End(); it--)
			{
				if (owner == (GUIControl*)(*it))
				{
					pRects = &invalidActiveRects;
					break;
				}
			}
		}
		owner = newOwner;
	} while(owner);

	if (isFreeze)
	{
		if (pRects == &invalidInactiveRects)
		{
			Rect fr = freezeRect;
			fr.Intersect(inRect);
			if (!fr.IsEmpty())
			{
				return;
			}
		}
	}


	InvalidRect *newRc = &tempRects[tempRectCounter];
	newRc->rect = inRect;

	if (!pRects->Empty())
	{
		VList::Iterator begin = pRects->Begin();
		for (; begin != pRects->End(); begin++)
		{
			InvalidRect *lRect = (InvalidRect*)(*begin);
			Rect rect = inRect;
			rect.Intersect(lRect->rect);
			if (!rect.IsEmpty())
			{
				lRect->rect.Union(inRect);
				return;
			}
		}
	}

	tempRectCounter++;
	FASSERT(tempRectCounter < maxTempRects);
	pRects->PushBack(newRc);

}

void GUISystem::AddControl( GUIControl *newControl )
{
	//GUIControl *pOldActive = (GUIControl*)*windows.Begin();
	newControl->DispatchEvent(EEI_WINDOW_WILL_ACTIVATE, NULL);
	windows.PushFront(newControl);
	InvalidateAll();
	newControl->DispatchEvent(EEI_WINDOW_DID_ACTIVATE, NULL);
}

void GUISystem::RemoveControl( GUIControl *control )
{
	control->DispatchEvent(EEI_WINDOW_WILL_DEACTIVATE, NULL);
	windows.Erase(windows.PosItem(control));
	InvalidateAll();
	control->DispatchEvent(EEI_WINDOW_DID_DEACTIVATE, NULL);
}

void GUISystem::MoveControlFront( GUIControl *control )
{
	for (VList::Iterator it = windows.Begin(); it != windows.End(); it++)
	{
		if (((GUIControl*)(*it)) == control)
		{
			windows.Erase(it);
			windows.PushFront(control);
			InvalidateAll();
			return;
		}
	}
}

void GUISystem::SetActiveControl( GUIControl *control )
{
	if (activeWindow)
	{
		activeWindow->DispatchEvent(EEI_WINDOW_LOST_MAIN, NULL);
	}
	activeWindow = control;
	if (activeWindow)
	{
		activeWindow->DispatchEvent(EEI_WINDOW_SET_MAIN,  NULL);
	}
	activeIt = windows.PosItem(activeWindow);
	InvalidateAll();
}

GUIControl * GUISystem::SetFocus( GUIControl *control, bool recalcCursor/* = true*/, bool forceSet/* = false*/)
{
	//if (focused)
	//{
	//	focused->OnLostFocus();
	//}

	GUIControl *oldFocus = focused;
	pNewFocus = control;
	isNeedToRecalcCursor = recalcCursor;

	if (forceSet)
	{
		UpdateFocus();
	}
	//if (focused)
	//{
	//	focused->OnSetFocus();
	//	if (recalcCursor)
	//	{
	//		RecalcCursor();
	//	}
	//}
	return oldFocus;
}

void GUISystem::LockKeyboard()
{
//	UTILS_LOG(EDMP_DEBUG, "GUISystem::LockKeyboard start: value = %d", isKeyboardLocked);

	isKeyboardLocked++;

//	UTILS_LOG(EDMP_DEBUG, "GUISystem::LockKeyboard end: value = %d", isKeyboardLocked);
}

void GUISystem::UnlockKeyboard()
{
//	UTILS_LOG(EDMP_DEBUG, "GUISystem::UnlockKeyboard start: value = %d", isKeyboardLocked);

	FASSERT(isKeyboardLocked >= 0);
	isKeyboardLocked--;

//	UTILS_LOG(EDMP_DEBUG, "GUISystem::UnlockKeyboard end: value = %d", isKeyboardLocked);
}

void GUISystem::ProcessKeyboard()
{
	if (!isKeyboardLocked)
	{
		for(int32 i = Application::EKC_COUNT; i >= 0; --i)
		{
			if(pApp->IsKeyDown((Application::eKeyCode)i) || pApp->IsKeyRepeat((Application::eKeyCode)i))
			{
				ProcessKey((Application::eKeyCode)i);
			}
		}
	}
	if(pApp->IsKeyDown(Application::EKC_SOFT1))
	{
		SendEventToActive(EEI_SOFT1_PRESSED, NULL);
	}
	if(pApp->IsKeyDown(Application::EKC_SOFT2))
	{
		SendEventToActive(EEI_SOFT2_PRESSED, NULL);
	}
}

void GUISystem::ProcessKey( Application::eKeyCode code )
{

	if (code == Application::EKC_CLR)
	{
		if (focused)
		{
			GUIControl *cnt = focused->GetParent();
			while (cnt)
			{
				if (cnt->IsSelectable())
				{
					SetFocus(cnt);
					return;
				}
				cnt = cnt->GetParent();
			}
		}
		SendEventToActive(EEI_CLR_PRESSED, NULL);
		return;
	}


	if (code == Application::EKC_SELECT)
	{
		SendEventToActive(EEI_SELECT_PRESSED, NULL);
		return;
	}

	if (code == Application::EKC_DOWN)
	{
		ProcessDir(EFD_DOWN, EEI_BOTTOM_REACH);
		return;
	}

	if (code == Application::EKC_UP)
	{
		ProcessDir(EFD_UP, EEI_TOP_REACH);
		return;
	}

	if (code == Application::EKC_LEFT)
	{
		ProcessDir(EFD_LEFT, EEI_LEFT_REACH);
		return;
	}

	if (code == Application::EKC_RIGHT)
	{
		ProcessDir(EFD_RIGHT, EEI_RIGHT_REACH);
		return;
	}
}

void GUISystem::SendEventToAll( eEventID eventID, EventData *pData )
{
	DispatchEvent(eventID, pData);
}

void GUISystem::SendEventToControl( GUIControl *control, eEventID eventID, EventData *pData )
{
	control->DispatchEvent(eventID, pData);
}

void GUISystem::SendEventToActive( eEventID eventID, EventData *pData )
{
	//VList::Iterator it = windows.End();
	//it--;
	//for (; it != windows.End(); --it)
	//{
	//	((GUIControl*)(*it))->DispatchEvent(eventID, pData);
	//}
	if (activeWindow)
	{
		activeWindow->DispatchEvent(eventID, pData);
	}
}

void GUISystem::SendEventToAllActive( eEventID eventID, EventData *pData )
{
	if (!windows.Empty())
	{
		VList::Iterator it = windows.End();
		it--;
		for (; it != windows.End(); --it)
		{
			((GUIControl*)(*it))->DispatchEvent(eventID, pData);
		}
	}
}


void GUISystem::SendEventToSystem( eEventID eventID, EventData *pData )
{
	if (eventID == waitForEvent)
	{
		waitForEvent = EEI_NONE;
		return;
	}

	SendEventToActive(eventID, pData);

	if (isCycling)
	{
		eFocusMoveDir dir;
		switch(eventID)
		{
		case EEI_LEFT_REACH:
			{
				waitForEvent = EEI_RIGHT_REACH;
				dir = EFD_RIGHT;
			}break;
		case EEI_RIGHT_REACH:
			{
				waitForEvent = EEI_LEFT_REACH;
				dir = EFD_LEFT;
			}break;
		case EEI_BOTTOM_REACH:
			{
				waitForEvent = EEI_TOP_REACH;
				dir = EFD_UP;
			}break;
		case EEI_TOP_REACH:
			{
				waitForEvent = EEI_BOTTOM_REACH;
				dir = EFD_DOWN;
			}break;
		}
		while (waitForEvent != EEI_NONE)
		{
			ProcessDir(dir, waitForEvent);
		}
	}

}

GUIControl * GUISystem::FindNextControl( eFocusMoveDir dir, GUIControl *parent)
{
	GUIControl *fc = focused;
	if (pNewFocus && fc != pNewFocus)
	{
		fc = pNewFocus;
	}
#ifdef DEBUG_INPUT
	deepInput++;
	char temp[100];
	int i;
	for (i = 0; i < deepInput; i++)
	{
		temp[i] = '0' + i;
	}
	temp[i] = 0;

	UTILS_TRACE("%s ++++++++++", temp);
#endif
	int32 dist = 999999;
	VList *pChildList;
	if (!parent || !(pChildList = parent->GetChilds()))
	{
#ifdef DEBUG_INPUT
		UTILS_TRACE("%s ----------", temp);
		deepInput--;
#endif
		return NULL;
	}
	GUIControl *newFocused = NULL;
	VList::Iterator it;
	int32 childCenterX = 0;
	int32 childCenterY = 0;
	int32 x = 0;
	int32 y = 0;
	for(it = pChildList->Begin(); it != pChildList->End(); it++)
	{
		GUIControl * child = (GUIControl*)(*it);
		if (child == fc)
		{
			continue;
		}
		if (!child->IsSelectable())
		{
			child = FindNextControl(dir, child);
			if (!child)
			{
				continue;
			}
		}
		Rect childRect;
		child->GetScreenRect(childRect);
#ifdef DEBUG_INPUT
		UTILS_TRACE("%s Find rect x=%d, y=%d", temp,  childRect.x, childRect.y);
#endif

		if( (dir == EFD_DOWN && childRect.y > cursorY && childRect.y > focusRect.y && childRect.y + childRect.dy > focusRect.y + focusRect.dy)
			|| (dir == EFD_UP && childRect.y + childRect.dy - 1 < cursorY && childRect.y + childRect.dy < focusRect.y + focusRect.dy && childRect.y < focusRect.y)
			|| (dir == EFD_RIGHT && childRect.x > cursorX && childRect.x > focusRect.x && childRect.x + childRect.dx > focusRect.x + focusRect.dx)
			|| (dir == EFD_LEFT && childRect.x + childRect.dx - 1 <= cursorX && childRect.x + childRect.dx < focusRect.x + focusRect.dx && childRect.x < focusRect.x)
			)
		{
#ifdef DEBUG_INPUT
			UTILS_TRACE("%s Calc rect x=%d, y=%d", temp, childRect.x, childRect.y);
#endif
			switch(dir)
			{
			case EFD_UP:
				childCenterX = childRect.x + (childRect.dx >> 1);//CLAMP(cursorX, childRect.x, childRect.x + (childRect.dx - 1));
				childCenterY = childRect.y + childRect.dy - 1;
				break;
			case EFD_DOWN:
				childCenterX = childRect.x + (childRect.dx >> 1);//CLAMP(cursorX, childRect.x, childRect.x + (childRect.dx - 1));
				childCenterY = childRect.y;
				break;
			case EFD_LEFT:
				childCenterX = childRect.x + childRect.dx - 1;
				childCenterY = childRect.y + (childRect.dy >> 1);//CLAMP(cursorY, childRect.y, childRect.y + (childRect.dy - 1));
				break;
			case EFD_RIGHT:
				childCenterX = childRect.x;
				childCenterY = childRect.y + (childRect.dy >> 1);//CLAMP(cursorY, childRect.y, childRect.y + (childRect.dy - 1));
				break;
			}
			int32 newDist;
			if (dir == EFD_LEFT || dir == EFD_RIGHT)
			{
				newDist = ((childCenterX - cursorX) * (childCenterX - cursorX)) + ((childCenterY - cursorY) * (childCenterY - cursorY) * 2);
			}
			else
			{
				newDist = ((childCenterX - cursorX) * (childCenterX - cursorX) * 2) + ((childCenterY - cursorY) * (childCenterY - cursorY));
			}

#ifdef DEBUG_INPUT
			UTILS_TRACE("%s new d = %d old d = %d", temp, newDist, dist);
#endif
			if(newDist < dist)
			{
#ifdef DEBUG_INPUT
				UTILS_TRACE("%s SELECT rect x=%d, y=%d", temp, childCenterX, childCenterY);
#endif
				x = childCenterX;
				y = childCenterY;
				dist = newDist;
				newFocused = child;
			}
		}
	}
    
	tempCursorX = x;
	tempCursorY = y;
#ifdef DEBUG_INPUT
	UTILS_TRACE("%s ----------", temp);
	deepInput--;
#endif
	return newFocused;
}

void GUISystem::ProcessDir( eFocusMoveDir dir, eEventID borderEvent )
{

	if (focused)
	{
		deepInput = 0;
		GUIControl *cnt = focused;
		if (pNewFocus != cnt)
		{
			cnt = pNewFocus;
		}

		 cnt->GetScreenRect(focusRect);

		while (cnt->GetParent())
		{
			cnt = cnt->GetParent();
		}
		UTILS_TRACE("MOVE CURSOR");
		cnt = FindNextControl(dir, cnt);
		if (cnt)
		{
			cursorX = tempCursorX;
			cursorY = tempCursorY;
#ifdef DEBUG_INPUT
			UTILS_TRACE("cursorX = %d, cursor y = %d", cursorX, cursorY);
#endif
			SetFocus(cnt, false);
			return;
		}
	}
	SendEventToSystem(borderEvent, NULL);
	return;
}

void GUISystem::RecalcCursor()
{
	if (focused)
	{
		Rect focusedRect;
		focused->GetScreenRect(focusedRect);
		cursorX = focusedRect.x + (focusedRect.dx >> 1);
		cursorY = focusedRect.y + (focusedRect.dy >> 1);
	}
}

void GUISystem::SetBackground( Sprite *pSprite )
{
	UTILS_TRACE("Set BG sprite");
	pBackSurface = NULL;
	SAFE_RELEASE(pBackSprite);
	isBackSurface = false;
	pBackSprite = pSprite;
	if (pBackSprite)
	{
		pBackSprite->AddReference();
	}

	ForceInvalidate();
	//InvalidateAll();
}

void GUISystem::SetBackground( GraphicsSystem::Surface *pSurface )
{
	UTILS_TRACE("Set BG surface");
	SAFE_RELEASE(pBackSprite);
	isBackSurface = true;
	pBackSurface = pSurface;

	ForceInvalidate();
	//InvalidateAll();
}

void GUISystem::InvalidateAll()
{
	//UTILS_LOG(EDMP_DEBUG, "Invalidate all");
	//invalidInactiveRects.Clear();
	//invalidActiveRects.Clear();
	//tempRectCounter = 2;
	//tempRects[0].rect = screenRect;
	//tempRects[0].isOnActive = false;
	//tempRects[1].rect = screenRect;
	//tempRects[1].isOnActive = true;
	//invalidInactiveRects.PushBack(&tempRects[0]);
	//invalidActiveRects.PushBack(&tempRects[1]);
	VList::Iterator it = windows.End();
	it--;
	for (;it != windows.End(); it--)
	{
		InvalidateRect(((GUIControl*)*it)->GetOutputRect(), ((GUIControl*)*it));
	}
	UTILS_TRACE("Invalidate All");

	//isAllInvalid = true;
}

void GUISystem::ForceInvalidate()
{
	if (isFreeze)
	{
		return;
	}

	invalidInactiveRects.Clear();
	invalidActiveRects.Clear();
	tempRectCounter = 2;
	tempRects[0].rect = screenRect;
	tempRects[0].isOnActive = false;
	tempRects[1].rect = screenRect;
	tempRects[1].isOnActive = true;
	invalidInactiveRects.PushBack(&tempRects[0]);
	invalidActiveRects.PushBack(&tempRects[1]);
	UTILS_TRACE("Force invalidate");

	isAllInvalid = true;
}
void GUISystem::ChangeControl( GUIControl *from, GUIControl *to, eTransitionType transition /*= ETT_NONE*/ )
{
	FASSERT(to);
	if (!from)
	{
		to->DispatchEvent(EEI_WINDOW_WILL_ACTIVATE, NULL);
		windows.PushFront(to);
		if (!GetActiveControl())
		{
			SetActiveControl(to);
		}
		InvalidateAll();
		return;
	}



		if (transition != ETT_NONE)
		{
			isTransition = true;
			controlFrom = from;
			controlTo = to;
			transitionType = transition;
			transInvalidRect.rect = from->GetRect();
			transInvalidRect.rect.Union(to->GetRect());
			graphicSystem->SetCurrentSurface(pSurfFrom);
			graphicSystem->SetClip(transInvalidRect.rect.x, transInvalidRect.rect.y, transInvalidRect.rect.dx, transInvalidRect.rect.dy);
			DrawBackground();
			DrawControl(controlFrom, &transitionList);
		}
		from->DispatchEvent(EEI_WINDOW_WILL_DEACTIVATE, NULL);

		if (!isTransition)
		{
			to->DispatchEvent(EEI_TRANSITION_FINISHED, NULL);
			from->DispatchEvent(EEI_WINDOW_DID_DEACTIVATE, NULL);
		}

		//changing window in the list
		VList::Iterator it = windows.PosItem(from);
		it++;
		if (it == windows.End())
		{
			windows.PushFront(to);
		}
		else
		{
			windows.Insert(it, to);
		}

		EventData ed((int32)from);
		to->DispatchEvent(EEI_WINDOW_WILL_ACTIVATE, &ed);
		if (GetActiveControl() == from)
		{
			SetActiveControl(to);
		}
		windows.Erase(windows.PosItem(from));
		if (transitionType == ETT_NONE)
		{
			InvalidateAll();
		}

		if (transition != ETT_NONE)
		{
			UpdateFocus();
			to->Update();
			graphicSystem->SetCurrentSurface(pSurfTo);
			graphicSystem->SetClip(transInvalidRect.rect.x, transInvalidRect.rect.y, transInvalidRect.rect.dx, transInvalidRect.rect.dy);
			DrawBackground();
			DrawControl(controlTo, &transitionList);
			graphicSystem->SetCurrentSurface(NULL);

			switch(transitionType)
			{
			case ETT_FROM_UP:
				trX = to->GetRect().x;
				trY = from->GetRect().y - to->GetRect().dy;
				break;
			case ETT_FROM_DOWN:
				trX = to->GetRect().x;
				trY = from->GetRect().y + from->GetRect().dy;
				break;
			case ETT_FROM_LEFT:
				trX = from->GetRect().x - to->GetRect().dx;
				trY = to->GetRect().y;
				break;
			case ETT_FROM_RIGHT:
				trX = from->GetRect().x + from->GetRect().dx;
				trY = to->GetRect().y;
				break;
			case ETT_ALPHA:
				trAlpha = 15;
				break;
			}
		}
		else
		{
			to->DispatchEvent(EEI_WINDOW_DID_ACTIVATE, NULL);
		}



}

void GUISystem::TransitionUpdate()
{
	VList::Iterator it = windows.PosItem(controlFrom);
	it--;
	for (;it != windows.End(); it--)
	{
		GUIControl *c = ((GUIControl*)*it);
		if (c != controlFrom && c != controlTo)
		{
			c->Update();
		}
	}

	UpdateFocus();

	bool isDone = false;
	Rect fr = controlTo->GetRect();
	switch(transitionType)
	{
	case ETT_FROM_UP:
	case ETT_FROM_DOWN:
		if (fr.y != trY)
		{
			int32 spd = (fr.y - trY) / 3;
			if (fr.y > trY)
			{
				spd++;
				trY += spd;
				if (fr.y < trY)
				{
					trY = fr.y;
				}
			}
			else
			{
				spd--;
				trY += spd;
				if (fr.y > trY)
				{
					trY = fr.y;
				}
			}
		}
		else
		{
			isDone = true;
		}
		break;
	case ETT_FROM_LEFT:
	case ETT_FROM_RIGHT:
		if (fr.x != trX)
		{
			int32 spd = (fr.x - trX) / 3;
			if (fr.x > trX)
			{
				spd++;
				trX += spd;
				if (fr.x < trX)
				{
					trX = fr.x;
				}
			}
			else
			{
				spd--;
				trX += spd;
				if (fr.x > trX)
				{
					trY = fr.x;
				}
			}
		}
		else
		{
			isDone = true;
		}
		break;
	case ETT_ALPHA:
		trAlpha-=2;
		if (trAlpha <= 0)
		{
			isDone = true;
		}
		break;
	}

	if (isDone)
	{
		controlTo->Update();
		isTransition = false;
		controlFrom->DispatchEvent(EEI_WINDOW_DID_DEACTIVATE, NULL);
		controlTo->DispatchEvent(EEI_TRANSITION_FINISHED, NULL);
		controlTo->DispatchEvent(EEI_WINDOW_DID_ACTIVATE, NULL);
		ForceInvalidate();
	}
}

void GUISystem::TransitionDraw()
{
	graphicSystem->SetClip();
	graphicSystem->DrawSurface(pBuffer, 0, 0, false, 0, 0);

	graphicSystem->SetClip(transInvalidRect.rect.x, transInvalidRect.rect.y, transInvalidRect.rect.dx, transInvalidRect.rect.dy);
	Rect fromRect = controlFrom->GetRect();
	Rect toRect = controlTo->GetRect();
	switch(transitionType)
	{
	case ETT_FROM_UP:
		graphicSystem->SetClipIntersect(fromRect.x, trY + toRect.dy, fromRect.dx, fromRect.dy);
		graphicSystem->DrawSurface(pSurfFrom, 0, trY + toRect.dy - fromRect.y, false, 0, 0);

		graphicSystem->SetClip(transInvalidRect.rect.x, transInvalidRect.rect.y, transInvalidRect.rect.dx, transInvalidRect.rect.dy);
		graphicSystem->SetClipIntersect(trX, trY, toRect.dx, toRect.dy);
		graphicSystem->DrawSurface(pSurfTo, trX - toRect.x, trY - toRect.y, false, 0, 0);
		break;
	case ETT_FROM_DOWN:
		graphicSystem->SetClipIntersect(fromRect.x, trY - fromRect.dy, fromRect.dx, fromRect.dy);
		graphicSystem->DrawSurface(pSurfFrom, 0, trY - fromRect.dy - fromRect.y, false, 0, 0);

		graphicSystem->SetClip(transInvalidRect.rect.x, transInvalidRect.rect.y, transInvalidRect.rect.dx, transInvalidRect.rect.dy);
		graphicSystem->SetClipIntersect(trX, trY, toRect.dx, toRect.dy);
		graphicSystem->DrawSurface(pSurfTo, trX - toRect.x, trY - toRect.y, false, 0, 0);
		break;
	case ETT_FROM_LEFT:
		graphicSystem->SetClipIntersect(trX + toRect.dx, fromRect.y, fromRect.dx, fromRect.dy);
		graphicSystem->DrawSurface(pSurfFrom, trX + toRect.dx - fromRect.x, 0, false, 0, 0);

		graphicSystem->SetClip(transInvalidRect.rect.x, transInvalidRect.rect.y, transInvalidRect.rect.dx, transInvalidRect.rect.dy);
		graphicSystem->SetClipIntersect(trX, trY, toRect.dx, toRect.dy);
		graphicSystem->DrawSurface(pSurfTo, trX - toRect.x, trY - toRect.y, false, 0, 0);
		break;
	case ETT_FROM_RIGHT:
		graphicSystem->SetClipIntersect(trX - fromRect.dx, fromRect.y, fromRect.dx, fromRect.dy);
		graphicSystem->DrawSurface(pSurfFrom, trX - fromRect.dx - fromRect.x, 0, false, 0, 0);

		graphicSystem->SetClip(transInvalidRect.rect.x, transInvalidRect.rect.y, transInvalidRect.rect.dx, transInvalidRect.rect.dy);
		graphicSystem->SetClipIntersect(trX, trY, toRect.dx, toRect.dy);
		graphicSystem->DrawSurface(pSurfTo, trX - toRect.x, trY - toRect.y, false, 0, 0);
		break;
	case ETT_ALPHA:
		graphicSystem->DrawSurface(pSurfTo, 0, 0, false, 0, 0);
		graphicSystem->SetBlendMode(GraphicsSystem::EBM_ALPHA);
		graphicSystem->SetAlpha((uint8)trAlpha);
		graphicSystem->DrawSurface(pSurfFrom, 0, 0, false, 0, 0);
		graphicSystem->SetBlendMode(GraphicsSystem::EBM_NONE);
	}
	
	VList::Iterator it = windows.PosItem(controlTo);
	it--;
	for (;it != windows.End(); it--)
	{
		InvalidRect tmp;
		tmp.rect = ((GUIControl*)*it)->GetRect();
		tempList.Clear();
		tempList.PushBack(&tmp);
		DrawControl((GUIControl*)*it, &tempList);
	}
}

void GUISystem::UpdateFocus()
{
	//sets new focused item to focus
	if (focused != pNewFocus)
	{
		GUIControl *old = focused;

		focused = pNewFocus;

		if (old)
		{
			old->OnLostFocus();
		}

		if (focused)
		{
			focused->OnSetFocus();
			if (isNeedToRecalcCursor)
			{
				RecalcCursor();
			}
		}
	}
	//------------------------------------
}

void GUISystem::FreezeBackground( const Rect &rect )
{
	if (isFreeze)
	{
		return;
	}

	invalidInactiveRects.Clear();
	invalidActiveRects.Clear();
	tempRectCounter = 0;

	graphicSystem->SetCurrentSurface(pBuffer);
	graphicSystem->SetClip(rect.x, rect.y, rect.dx, rect.dy);
	DrawBackground();
	freezeRect = rect;

	VList::Iterator it = windows.End();
	it--;
	InvalidRect tmp;
	tmp.rect = rect;
	tempList.Clear();
	tempList.PushBack(&tmp);
	for (;it != windows.End(); it--)
	{
		DrawControl((GUIControl*)*it, &tempList);
	}
	graphicSystem->SetCurrentSurface(NULL);
	UTILS_TRACE("Freeze");
	freezeCounter = 0;
	isFreeze = true;
	ProcessFreeze();
}

void GUISystem::UnfreezeBackground()
{
	if (!isFreeze)
	{
		return;
	}

	isFreeze = false;
	UTILS_TRACE("Unfreeze");
	ForceInvalidate();
}

void GUISystem::DrawBackground()
{
	//UTILS_TRACE("DrawBackground");
	//int32 x,y,dx,dy;
	//graphicSystem->GetClip(&x,&y,&dx,&dy);
	//UTILS_TRACE("Clip x=%d, y=%d, dx=%d, dy=%d", x, y, dx, dy);
	if (isBackSurface)
	{
		if (pBackSurface)
		{
			graphicSystem->DrawSurface(pBackSurface, 0, 0, false, 0, 0);
		}
	}
	else
	{
		if (pBackSprite)
		{
			pBackSprite->Draw((graphicSystem->GetWidth() - pBackSprite->GetWidth()) >> 1, (graphicSystem->GetHeight() - pBackSprite->GetHeight()) >> 1);
		}
	}
}

void GUISystem::ProcessFreeze()
{
	//if (freezeCounter > FREEZE_ITERATIONS)
	//{
	//	return;
	//}
	graphicSystem->SetCurrentSurface(pBuffer);
	graphicSystem->SetClip(freezeRect.x, freezeRect.y, freezeRect.dx, freezeRect.dy);
	graphicSystem->SetColor(0, 0, 0);
	graphicSystem->SetAlpha(11);
	graphicSystem->SetBlendMode(GraphicsSystem::EBM_ALPHA);
	graphicSystem->FillRect(freezeRect.x, freezeRect.y, freezeRect.dx, freezeRect.dy);
	graphicSystem->SetBlendMode(GraphicsSystem::EBM_NONE);
	graphicSystem->SetCurrentSurface(NULL);
	InvalidateRect(freezeRect, activeWindow);
	freezeCounter++;

}

GUIControl * GUISystem::GetRoot( GUIControl *child )
{
	while (child && child->GetParent())
	{
		child = child->GetParent();
	}
	return child;
}

GUIControl * GUISystem::GetByID( int32 id )
{
	if (!windows.Empty())
	{
		VList::Iterator it = windows.End();
		it--;
		for (; it != windows.End(); --it)
		{
			GUIControl *cn = ((GUIControl*)(*it))->GetByID(id);
			if (cn)
			{
				return cn;
			}
		}
	}
	return NULL;
}

void GUISystem::NeedToRecalcCursor()
{
	isNeedToRecalcCursor = true;
}

GUIIndicator * GUISystem::GetIndicator( GUIIndicator::eIndicatorType type )
{
	return pIndicators[type];
}

void GUISystem::SetIndicator( GUIIndicator::eIndicatorType type, GUIIndicator *indicator )
{
	pIndicators[type] = indicator;
}

void GUISystem::OnChar( char16 ch )
{
	EventData data;
	data.wParam = ch;
	DispatchEvent(EEI_CHAR, &data);
}

