#ifndef __SCREEN_TEMPLATES__
#define __SCREEN_TEMPLATES__

#include "basescreen.h"
#include "N2FData.h"




class GUIMenuItem;
class GUIListView;

class ScreenTemplates :
	public BaseScreen
{
	enum eMenuItems
	{


			EMI_COUNT

	};
	enum ePopUpItems
	{
			EPI_COUNT
	};

public:
	ScreenTemplates(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenTemplates(void);

	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);

	virtual bool OnEvent(eEventID eventID, EventData *pData);
	void	OnItem(int32 index);



private:

	GUIMenuItem *templateItem[ENP_MAX_TEMPLATES];
	GUIListView *list;

};
#endif
