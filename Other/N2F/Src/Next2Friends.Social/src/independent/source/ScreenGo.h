#ifndef __SCREEN_GO__
#define __SCREEN_GO__

#include "basescreen.h"


class ScreenGo :
	public BaseScreen
{
	enum eMenuItems
	{
			EMI_ASK_A_QUESTION = 0
		,	EMI_UPLOAD_PHOTO
		,	EMI_CHANGE_STATUS


		,	EMI_COUNT

	};
	enum ePopUpItems
	{
			EPI_SELECT = 0
		,	EPI_BACK


		,	EPI_COUNT
	};

public:
	ScreenGo(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenGo(void);

	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);

	virtual bool OnEvent(eEventID eventID, EventData *pData);
	void	OnItem(int32 index);



private:

};
#endif
