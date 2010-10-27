#ifndef __GUI_EVENT_DISPATCHER__
#define __GUI_EVENT_DISPATCHER__

#include "GUITypes.h"
#include "VList.h"

class GUIEventListener;

class GUIEventDispatcher
{
	struct ListenerData 
	{
		eEventID eventID;
		GUIEventListener *listener;
	};

	VList listeners;
public:

	GUIEventDispatcher(void);

	virtual ~GUIEventDispatcher(void);

	void AddListener(eEventID eventID, GUIEventListener *listener);
	void RemoveListener(eEventID eventID, GUIEventListener *listener);

	void DispatchEvent(eEventID eventID, EventData *pData);
};

#endif