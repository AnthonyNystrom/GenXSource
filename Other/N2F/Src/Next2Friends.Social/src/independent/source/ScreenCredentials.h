#ifndef __SCREEN_CREDENTIALS__
#define __SCREEN_CREDENTIALS__

#include "basescreen.h"
#include "IAnswer.h"


class ScreenCredentials :
	public BaseScreen, public IAnswer
{
	enum eMenuItems
	{
			EMI_USERNAME = 0
		,	EMI_PASSWORD


		,	EMI_COUNT

	};
	enum ePopUpItems
	{
			EPI_BACK = 0
		,	EPI_SUBMIT
		,	EPI_REMIND_PASSWORD


		,	EPI_COUNT
	};

public:
	ScreenCredentials(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenCredentials(void);

	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);

	virtual bool OnEvent( eEventID eventID, EventData *pData );

	virtual void OnSuccess(int32 packetId);
	virtual void OnFailed(int32 packetId, char16 *errorString);



private:

	bool isLogin;
	bool isCanceled;
	int32 packetsSent;


};
#endif
