#ifndef __SCREEN_OUTBOX__
#define __SCREEN_OUTBOX__

#include "basescreen.h"
#include "N2FData.h"
#include "ItemsListDelegate.h"
#include "VList.h"




class GUIMenuItem;
class GUIItemsList;


class ScreenOutbox :
	public BaseScreen, public ItemsListDelegate
{
	//enum eMenuItems
	//{


	//		EMI_COUNT

	//};
	enum ePopUpItems
	{
			EPI_SEND
		,	EPI_DELETE
		,	EPI_BACK

		,	EPI_COUNT
	};

public:
	ScreenOutbox(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenOutbox(void);

	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);
	//virtual bool			PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);



	virtual void			ItemsListOnItem(GUIItemsList *forList, void *owner);

	virtual GUIControl*		ItemsListCreateListItem(GUIItemsList *forList);

	virtual bool			ItemsListIsOwnerValid(GUIItemsList *forList, void *owner);
	virtual void			*ItemsListGetFirstOwner(GUIItemsList *forList);
	virtual void			*ItemsListGetNextOwner(GUIItemsList *forList);

	virtual void			ItemsListTuneItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner);
	virtual void			ItemsListCorrectItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner);


private:


	GUIItemsList *list;

	VList::Iterator outboxIterator;

};
#endif
