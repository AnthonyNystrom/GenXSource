#ifndef __SCREEN_SEND_SELECTION__
#define __SCREEN_SEND_SELECTION__

#include "basescreen.h"


class ScreenSendSelection :
	public BaseScreen
{
	enum eMenuItems
	{
			EMI_SEND_NOW = 0
		,	EMI_MOVE_TO_OUTBOX


		,	EMI_COUNT

	};

public:
	ScreenSendSelection(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenSendSelection(void);

	//virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	//virtual void			PopUpOnItemSelected(int32 index, int32 id);
	virtual bool PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);
	void	OnItem(int32 index);



private:

};
#endif
