#include "GUITabList.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "VList.h"
#include "GUIImage.h"


GUITabList::GUITabList( GUIControlContainer *parent /*= NULL*/, const ControlRect &cr/* = ControlRect()*/ )
: GUILayoutBox(true, parent, ControlRect())
{
	ControlRect hr = cr;
	hr.x = hr.y = 0;
	SetControlRect(hr);

	numberOfPages = 0;
	currentPage = -1;
	hr.minDy = hr.maxDy = 30;
	pageRect = cr;
	pageRect.minDy -= hr.minDy;
	pageRect.maxDy -= hr.maxDy;
	headerContainer = new GUIControlContainer(this, hr);
	//headerContainer->SetStretch(/*eChildStretch::*/ECS_STRETCH);
	selector = new GUIControl(headerContainer, hr);
	headerLayout = new GUILayoutBox(false, headerContainer,hr);
	headerLayout->SetAlignType(GUIControl::EA_LEFT | GUIControl::EA_BOTTOM);

	parent->AddListener(EEI_LEFT_REACH, this);
	parent->AddListener(EEI_RIGHT_REACH, this);
	parent->AddListener(EEI_WINDOW_LOST_MAIN, this);
	parent->AddListener(EEI_WINDOW_SET_MAIN, this);
	//parent->AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	//parent->AddListener(EEI_WINDOW_DID_ACTIVATE, this);

}

GUITabList::~GUITabList( void )
{
	Clear();
	SAFE_DELETE(headerContainer);
	for (int i = 0; i< numberOfPages; i++)
	{
		SAFE_DELETE(pages[i]);
	}
}



int32 GUITabList::AddPage(GUIControl *control, GUIImage *pageImage, GUIControl *focus)
{
	FASSERT(numberOfPages < MAX_PAGES);
	control->SetParent(NULL);
	pages[numberOfPages] = control;
	pages[numberOfPages]->SetControlRect(pageRect);
	images[numberOfPages] = pageImage;
	focuses[numberOfPages] = focus;
	defaultFocus[numberOfPages] = focus;
	pageImage->SetControlRect(ControlRect(0, 0, pageImage->GetImageWidth(), pageImage->GetImageHeight(), pageImage->GetImageWidth(), ControlRect::MAX_D));
	headerLayout->AddChild(pageImage);
	numberOfPages++;

	if (currentPage < 0)
	{
		SwitchPage(0, false);
	}

	return numberOfPages - 1;
}

void GUITabList::SwitchPage( int32 index , bool generateEvent/* = true*/)
{
	FASSERT(index >= 0);
	FASSERT(index < numberOfPages);
	if (index != currentPage)
	{
		if (currentPage >= 0)
		{
			GUIControl *focus = GUISystem::Instance()->GetFocus();
			if (GUISystem::Instance()->GetRoot(focus) == GetParent())
			{
				focuses[currentPage] = focus;
			}
			pages[currentPage]->SetParent(NULL);
		}
		currentPage = index;
		AddChild(pages[currentPage]);
		if (GUISystem::Instance()->GetActiveControl() == GetParent())
		{
			RestoreFocus(currentPage);
		}

	}

	if (generateEvent)
	{
		DispatchEvent(EEI_TAB_PAGE_SWITCHED, NULL);
	}
}

GUIControl	*GUITabList::GetPage( int32 index )
{
	FASSERT(index >= 0);
	FASSERT(index < numberOfPages);
	return pages[index];
}

GUIControl	* GUITabList::GetActivePage()
{
	return GetPage(currentPage);
}

int32 GUITabList::GetActivePageIndex()
{
	return currentPage;
}

void GUITabList::SetHeaderDrawType( eDrawType drawType )
{
	headerContainer->SetDrawType(drawType);
}

void GUITabList::SetSelectorDrawType( eDrawType drawType )
{
	selector->SetDrawType(drawType);
}

void GUITabList::Update()
{
	GUILayoutBox::Update();
	CheckSelectorSize();
}

bool GUITabList::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_RIGHT_REACH:
		{
			if (currentPage < numberOfPages - 1)
			{
				SwitchPage(currentPage + 1);
			}
		}
		break;
	case EEI_LEFT_REACH:
		{
			if (currentPage > 0)
			{
				SwitchPage(currentPage - 1);
			}
		}
		break;
	case EEI_WINDOW_LOST_MAIN:
		{
			if (currentPage >= 0)
			{
				focuses[currentPage] = GUISystem::Instance()->GetFocus();
			}
		}
		break;
	case EEI_WINDOW_SET_MAIN:
		{

			if (currentPage >= 0)
			{
				RestoreFocus(currentPage);
			}
		}
		break;
	}
	return true;
}

void GUITabList::CheckSelectorSize()
{
	if (currentPage >= 0)
	{
		Rect imgR;
		images[currentPage]->GetScreenRect(imgR);
		Rect selR;
		selector->GetScreenRect(selR);
		if (selR.dx != imgR.dx || selR.x != imgR.x)
		{
			Rect contRect;
			headerContainer->GetScreenRect(contRect);

			Rect sel = selector->GetRect();
			int32 spd = (imgR.dx - sel.dx) / 3;
			if (imgR.dx > sel.dx)
			{
				spd++;
				sel.dx += spd;
				if (imgR.dx < sel.dx)
				{
					sel.dx = imgR.dx;
				}
			}
			else
			{
				spd--;
				sel.dx += spd;
				if (imgR.dx > sel.dx)
				{
					sel.dx = imgR.dx;
				}
			}

			int32 targetX = imgR.x - contRect.x;
			spd = (targetX - sel.x) / 3;
			if (targetX > sel.x)
			{
				spd++;
				sel.x += spd;
				if (targetX < sel.x)
				{
					sel.x = targetX;
				}
			}
			else
			{
				spd--;
				sel.x += spd;
				if (targetX > sel.x)
				{
					sel.x = targetX;
				}
			}
			selector->SetRect(sel);
		}
	}
}

GUIControl* GUITabList::GetByID( int32 theID )
{
	GUIControl *retVal = GUILayoutBox::GetByID(theID);
	if (retVal)
	{
		return retVal;
	}

	for (int i = 0; i < numberOfPages; i++)
	{
		retVal = pages[i]->GetByID(theID);
		if (retVal)
		{
			return retVal;
		}
	}

	return NULL;

}

void GUITabList::RestoreFocus( int32 pageNumber )
{
	GUIControl *cr = focuses[pageNumber];
	if (cr)
	{
		while (cr != pages[pageNumber])
		{
			cr = cr->GetParent();
			if (!cr)
			{
				focuses[pageNumber] = defaultFocus[pageNumber];
				break;
			}
		}
	}
	else
	{
		focuses[pageNumber] = defaultFocus[pageNumber];
	}
	GUISystem::Instance()->SetFocus(focuses[pageNumber]);

}


