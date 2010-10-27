#ifndef __SCREEN_REMIND_PASSWORD__
#define __SCREEN_REMIND_PASSWORD__

#include "basescreen.h"
#include "IAnswer.h"


class ScreenRemindPassword :
	public BaseScreen, public IAnswer
{
	enum eMenuItems
	{
			EMI_VIA_SMS = 0
		,	EMI_VIA_EMAIL
		,	EMI_VIA_SMS_CHECKBOX
		,	EMI_VIA_EMAIL_CHECKBOX


		,	EMI_COUNT

	};
	enum ePopUpItems
	{
			EPI_SUBMIT = 0
		,	EPI_BACK


		,	EPI_COUNT
	};

public:
	ScreenRemindPassword(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenRemindPassword(void);

	virtual bool OnEvent(eEventID eventID, EventData *pData);


	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);

	virtual void OnSuccess(int32 packetId);
	virtual void OnFailed(int32 packetId, char16 *errorString);




private:

};
#endif
