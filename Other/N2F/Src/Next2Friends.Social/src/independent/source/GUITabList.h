#ifndef __GUI_TAB_LIST__
#define __GUI_TAB_LIST__

#include "GUILayoutBox.h"
#include "GUIEventListener.h"

#define MAX_PAGES	20

class GUIImage;

class GUITabList : public GUILayoutBox, public GUIEventListener
{
public:
	GUITabList(GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUITabList(void);

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	virtual void Update();

	virtual GUIControl* GetByID(int32 theID);

	
	virtual int32		AddPage(GUIControl *control, GUIImage *pageImage, GUIControl *focus);
	virtual void		SwitchPage(int32 index, bool generateEvent = true);
	virtual GUIControl	*GetPage(int32 index);
	virtual GUIControl	*GetActivePage();
	virtual int32		GetActivePageIndex();

	virtual void		SetHeaderDrawType(eDrawType drawType);
	virtual void		SetSelectorDrawType(eDrawType drawType);


protected:
	virtual void CheckSelectorSize();
	virtual void RestoreFocus(int32 pageNumber);

	GUILayoutBox		*headerLayout;
	GUIControlContainer	*headerContainer;
	GUIControl			*selector;
	GUIControl			*pages[MAX_PAGES];
	GUIImage			*images[MAX_PAGES];
	GUIControl			*focuses[MAX_PAGES];
	GUIControl			*defaultFocus[MAX_PAGES];

	int32 numberOfPages;
	int32 currentPage;
	ControlRect pageRect;
};


#endif