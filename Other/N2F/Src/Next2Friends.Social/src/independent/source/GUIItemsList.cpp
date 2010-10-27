#include "GUIItemsList.h"
#include "GUISystem.h"
#include "GUIListView.h"
#include "ItemsListDelegate.h"
#include "GUIData.h"



GUIItemsList::GUIItemsList( ItemsListDelegate* delegate, GUIControlContainer * _parent /*= NULL*/, const ControlRect &cr /*= ControlRect()*/ )
: GUIListView(_parent, cr)
{
	SetDelegate(delegate);
}

GUIItemsList::~GUIItemsList( void )
{
	RemoveAllItems();
	while (itemsList.Size())
	{
		SingleItem *si = (SingleItem*)(*itemsList.Begin());
		SAFE_DELETE(si);
		itemsList.Erase(itemsList.Begin());
	}
}

void GUIItemsList::SetDelegate( ItemsListDelegate *newDelegate )
{
	FASSERT(newDelegate);
	listDelegate = newDelegate;
}

void GUIItemsList::RebuildList()
{
	VList *guiList = GetListChilds();
	VList::Iterator it = guiList->Begin();
	for (;it != guiList->End(); it++)
	{
		GUIControl * guiItem = (GUIControl *)(*it);
		SingleItem *si = GetItemByID(guiItem->GetID() - ECIDS_ILIST_ITEM_ID);
		if (!listDelegate->ItemsListIsOwnerValid(this, si->pOwner))
		{
			si->state = EIS_EMPTY;
		}
	}
	
	ClearListFromEmpty();


	void *ownerItem = listDelegate->ItemsListGetFirstOwner(this);
	int32 i = 0;
	while (ownerItem)
	{
		bool isPresent = false;
		VList::Iterator it = guiList->Begin();
		for (;it != guiList->End(); it++)
		{
			GUIControl * guiItem = (GUIControl *)(*it);
			SingleItem *si = GetItemByID(guiItem->GetID() - ECIDS_ILIST_ITEM_ID);
			if (si->pOwner == ownerItem)
			{
				listDelegate->ItemsListCorrectItemByOwner(this, si->pItem, si->pOwner);
				isPresent = true;
				break;
			}
		}
		if (!isPresent)
		{
			SingleItem *si = GetFreeItem();
			si->state = EIS_PRESENT;
			si->pOwner = ownerItem;
			listDelegate->ItemsListTuneItemByOwner(this, si->pItem, si->pOwner);
			AddChild(si->pItem);
			if (!pLastFocus)
			{
				pLastFocus = si->pItem;
			}
		}
		ownerItem = listDelegate->ItemsListGetNextOwner(this);
	}

	ForceRecalc();
	Reset();

	if (isFocus)
	{
		GUISystem::Instance()->SetFocus(pLastFocus, true, true);
		ScrollToFocus();
	}

}



void GUIItemsList::ClearListFromEmpty()
{
	VList *guiList = GetListChilds();
	VList::Iterator it = guiList->Begin();
	GUIControl *prevGUIItem = NULL;
	for (;it != guiList->End();)
	{
		GUIControl * guiItem = (GUIControl *)(*it);
		SingleItem *si = GetItemByID(guiItem->GetID() - ECIDS_ILIST_ITEM_ID);
		if (si->state == EIS_EMPTY)
		{
			if (pLastFocus == guiItem)
			{
				VList::Iterator tit = it;
				tit++;
				if (tit != guiList->End())
				{
					pLastFocus = (GUIControl *)(*tit);
				}
				else if (prevGUIItem)
				{
					pLastFocus = prevGUIItem;
				}
				else
				{
					pLastFocus = NULL;
				}
			}
			si->state = EIS_FREE;
			RemoveChild(guiItem);
			it = guiList->Begin();
			guiItem = NULL;
			continue;
		}

		prevGUIItem = guiItem;
		it++;
	}
}


void * GUIItemsList::GetOwnerForItem( GUIControl *guiItem )
{
	SingleItem *item = GetItemByID(guiItem->GetID() - ECIDS_ILIST_ITEM_ID);
	if (!item)
	{
		return NULL;
	}
	return item->pOwner;
}

void * GUIItemsList::GetFocusedOwner()
{
	if (isFocus && GUISystem::Instance()->GetFocus())
	{
		return GetOwnerForItem(GUISystem::Instance()->GetFocus());
	}

	if (!pLastFocus)
	{
		return NULL;
	}
	return GetOwnerForItem(pLastFocus);
}

bool GUIItemsList::OnEvent( eEventID eventID, EventData *pData )
{
	if (eventID != EEI_BUTTON_PRESSED)
	{
		return false;
	}
	listDelegate->ItemsListOnItem(this, GetItemByID(((GUIControl*)(pData->lParam))->GetID() - ECIDS_ILIST_ITEM_ID)->pOwner);
	return true;
}


SingleItem * GUIItemsList::AddItem()
{
	SingleItem *d = new SingleItem();

	d->itemID = numberOfItems;

	d->pItem = listDelegate->ItemsListCreateListItem(this);

	d->pItem->AddListener(EEI_BUTTON_PRESSED, this);
	d->pItem->SetID(ECIDS_ILIST_ITEM_ID + numberOfItems);

	d->state = EIS_FREE;

	numberOfItems++;
	itemsList.PushBack(d);
	return d;
}


SingleItem * GUIItemsList::GetFreeItem()
{
	VList::Iterator itemsIt = itemsList.Begin();
	for (;itemsIt != itemsList.End(); itemsIt++)
	{
		SingleItem *si = (SingleItem*)(*itemsIt);
		if (si->state == EIS_FREE)
		{
			return si;
		}
	}

	return AddItem();
}

SingleItem * GUIItemsList::GetItemByID( int32 ID )
{
	VList::Iterator itemsIt = itemsList.Begin();
	for (;itemsIt != itemsList.End(); itemsIt++)
	{
		SingleItem *si = (SingleItem*)(*itemsIt);
		if (si->itemID == ID)
		{
			return si;
		}
	}
	return NULL;
}

void GUIItemsList::ActivateFocus()
{
	if (!isFocus || !GUISystem::Instance()->GetFocus() || GUISystem::Instance()->GetFocus() != pLastFocus)
	{
		isFocus = true;
		GUISystem::Instance()->SetFocus(pLastFocus, true, true);
		ScrollToFocus();
	}
}

void GUIItemsList::DeactivateFocus()
{
	if (isFocus)
	{
		isFocus = false;
		pLastFocus = GUISystem::Instance()->GetFocus();
	}
}

GUIControl *GUIItemsList::GetItemForOwner( void *owner )
{
	VList::Iterator itemsIt = itemsList.Begin();
	for (;itemsIt != itemsList.End(); itemsIt++)
	{
		SingleItem *si = (SingleItem*)(*itemsIt);
		if (si->pOwner == owner)
		{
			return si->pItem;
		}
	}
	return NULL;
}

SingleItem::~SingleItem()
{
	SAFE_DELETE(pItem);
}