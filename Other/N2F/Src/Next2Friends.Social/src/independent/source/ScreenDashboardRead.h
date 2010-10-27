#ifndef __SCREEN_DASHBOARD_READ__
#define __SCREEN_DASHBOARD_READ__

#include "basescreen.h"


class ScreenDashboardRead :
	public BaseScreen
{
	enum eMenuItems
	{
			EMI_IMAGE = 0
		,	EMI_AUTHOR
		,	EMI_DATE
		,	EMI_TEXT


		,	EMI_COUNT

	};

public:
	ScreenDashboardRead(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenDashboardRead(void);

	//virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	//virtual void			PopUpOnItemSelected(int32 index, int32 id);
	virtual bool PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);



private:

};
#endif
