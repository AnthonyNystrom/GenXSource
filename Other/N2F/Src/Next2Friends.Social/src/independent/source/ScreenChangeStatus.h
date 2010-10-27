#ifndef __SCREEN_CHANGE_STATUS__
#define __SCREEN_CHANGE_STATUS__

#include "basescreen.h"
#include "N2FData.h"
#include "ItemsListDelegate.h"
#include "VList.h"
#include "IAnswer.h"




class GUIMenuItem;
class GUIItemsList;
class SurfacesPool;
struct AskResponse;
struct ArrayOfInt32;

class ScreenChangeStatus :
	public BaseScreen, public IAnswer
{
	enum eMenuItems
	{
			EMI_CURRENT_STATUS = 0
		,	EMI_NEW_STATUS

		,	EMI_COUNT

	};
	//enum ePopUpItems
	//{
	//		EPI_DELETE
	//	,	EPI_BACK

	//	,	EPI_COUNT
	//};

public:
	ScreenChangeStatus(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenChangeStatus(void);

	//virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	//virtual void			PopUpOnItemSelected(int32 index, int32 id);
	virtual bool			PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	virtual	void OnSuccess(int32 packetId);
	virtual	void OnFailed(int32	packetId, char16 *errorString);


private:

	int32 packetsSent;
	bool isCancel;


	char16 currentStatus[ENP_MAX_STATUS_SIZE];


};
#endif
