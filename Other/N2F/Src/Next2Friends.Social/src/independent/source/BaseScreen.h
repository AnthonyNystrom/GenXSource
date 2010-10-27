#ifndef _SCREEN_BASE_H_
#define _SCREEN_BASE_H_

#include "ApplicationManager.h"
#include "GUIControlContainer.h"
#include "GUIEventListener.h"
#include "GUIData.h"
#include "graphres.h"
#include "GUISystem.h"
#include "PopUpDelegate.h"
#include "StringWrapper.h"

class GUIHeader;
class GUIFooter;


class BaseScreen : public GUIControlContainer, public GUIEventListener, public PopUpDelegate
{
public:
	BaseScreen(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~BaseScreen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	GUIControl* GetLastFocus()
	{
		return pLastFocus;
	};

	ApplicationManager	*GetAppManager()	const 
	{	
		return pManager;
	};

	//popup delegate methods
	virtual bool	PopUpShouldOpen();
	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);


	virtual void	BackToPrevSrceen();
	virtual void	SetPrevScreen(int32 prevID);

	

protected:

	GUIControl		*pLastFocus;

	int32			prevScreen;

	ApplicationManager		*pManager;

};


#endif//_SCREEN_BASE_H_
