#ifndef __SCREEN_WRITE_COMMENT__
#define __SCREEN_WRITE_COMMENT__

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

class ScreenWriteComment :
	public BaseScreen
{
	enum eMenuItems
	{
			EMI_COMMENT = 0
		,	EMI_REPLY_COMMENT

		,	EMI_COUNT

	};
	//enum ePopUpItems
	//{
	//		EPI_DELETE
	//	,	EPI_BACK

	//	,	EPI_COUNT
	//};

public:
	ScreenWriteComment(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenWriteComment(void);

	//virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	//virtual void			PopUpOnItemSelected(int32 index, int32 id);
	virtual bool			PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);


private:



};
#endif
