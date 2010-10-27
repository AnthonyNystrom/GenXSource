#ifndef __GUI_ITEMS_LIST__
#define __GUI_ITEMS_LIST__

#include "GUIListView.h"
#include "GUIEventListener.h"

class ItemsListDelegate;

enum eItemState
{
		EIS_FREE = 0
	,	EIS_PRESENT
	,	EIS_EMPTY
};

struct SingleItem 
{
	~SingleItem();
	void *pOwner;
	int32 itemID;
	GUIControl *pItem;
	eItemState state;
};

class GUIItemsList : public GUIListView, public GUIEventListener
{

public:
	GUIItemsList(ItemsListDelegate* delegate, GUIControlContainer * _parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUIItemsList(void);

	virtual bool OnEvent(eEventID eventID, EventData *pData);

  

	void RebuildList();

	void ActivateFocus();
	void DeactivateFocus();


	void *GetOwnerForItem(GUIControl *guiItem);
	void *GetFocusedOwner();
	GUIControl *GetItemForOwner(void *owner);


	void SetDelegate(ItemsListDelegate *newDelegate);



protected:

	int32 numberOfItems;
	VList itemsList;

    SingleItem *AddItem();
	SingleItem *GetFreeItem();
	SingleItem *GetItemByID(int32 ID);
	
	void ClearListFromEmpty();



	ItemsListDelegate *listDelegate;


	bool isFocus;
	GUIControl *pLastFocus;

};


#endif//__GUI_BUTTON__