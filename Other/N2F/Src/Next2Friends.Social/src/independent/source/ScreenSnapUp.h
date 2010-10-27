#ifndef __SCREEN_SNAP_UP__
#define __SCREEN_SNAP_UP__

#include "basescreen.h"


class ScreenSnapUp :
	public BaseScreen
{
	enum eMenuItems
	{
			EMI_UPLOAD_FROM_FILE = 0
		,	EMI_UPLOAD_FROM_CAMERA


		,	EMI_COUNT

	};
	enum ePopUpItems
	{
			EPI_SELECT = 0
		,	EPI_BACK


		,	EPI_COUNT
	};

public:
	ScreenSnapUp(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenSnapUp(void);

	//virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	//virtual void			PopUpOnItemSelected(int32 index, int32 id);
	virtual	bool PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);
	void	OnItem(int32 index);



private:

};
#endif
