#ifndef __SCREEN_INBOX__
#define __SCREEN_INBOX__

#include "basescreen.h"
#include "N2FData.h"
#include "ItemsListDelegate.h"
#include "VList.h"




class GUIMenuItem;
class GUIItemsList;


class ScreenInbox :
	public BaseScreen, public ItemsListDelegate
{
	//enum eMenuItems
	//{


	//		EMI_COUNT

	//};
	enum ePopUpItems
	{
			EPI_READ
		,	EPI_GET_AND_SEND
		,	EPI_BACK

		,	EPI_COUNT
	};

public:
	ScreenInbox(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenInbox(void);

	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);

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

	VList::Iterator inboxIterator;

};
#endif
