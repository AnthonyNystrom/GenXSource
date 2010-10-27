#ifndef __SCREEN_ATTACH_FROM_FILE__
#define __SCREEN_ATTACH_FROM_FILE__

#include "basescreen.h"
#include "N2FData.h"
#include "ItemsListDelegate.h"
#include "VList.h"




class GUIMenuItem;
class GUIItemsList;
class SurfacesPool;
class GUITabList;


class ScreenAttachFromFile :
	public BaseScreen, public ItemsListDelegate
{
	enum eMenuItems
	{


			EMI_COUNT

	};
	enum ePopUpItems
	{
			EPI_ATTACH = 0
		,	EPI_REMOVE
		,   EPI_VIEW
		,	EPI_EXTERNAL_MEMORY
		,	EPI_INTERNAL_MEMORY
		,	EPI_BACK
		,	EPI_UPLOAD

		,	EPI_COUNT
	};

public:
	ScreenAttachFromFile(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenAttachFromFile(void);

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


	GUITabList *tabList;
	GUIItemsList *listPhotos[2];
	SurfacesPool *surfPool;



};
#endif
