#ifndef __GUI_EVENT_LISTENER__
#define __GUI_EVENT_LISTENER__

#include "GUITypes.h"

class GUIEventListener
{
public:

	GUIEventListener(void)
	{
	}

	virtual ~GUIEventListener(void)
	{
	}

	virtual bool OnEvent(eEventID eventID, EventData *pData) = 0;
};
#endif
