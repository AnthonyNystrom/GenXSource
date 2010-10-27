#ifndef __SCREEN_MAIN_MENU__
#define __SCREEN_MAIN_MENU__

#include "basescreen.h"


class ScreenMainMenu :
	public BaseScreen
{
	enum eMenuItems
	{
			EMI_GO = 0
		,	EMI_INBOX
		,	EMI_DASHBOARD
		,	EMI_DRAFTS
		,	EMI_OUTBOX
		,	EMI_SETTINGS


		,	EMI_COUNT
	};
	enum ePopUpItems
	{
			EPI_GO_TO_NEXT2FRIENDS = 0
		,	EPI_GET_AND_SEND
		,	EPI_SELECT


		,	EPI_COUNT
	};

public:
	ScreenMainMenu(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenMainMenu(void);

	virtual bool OnEvent(eEventID eventID, EventData *pData);


	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);

	void	OnItem(int32 index);



private:

	void RecalcItems();

};
#endif
