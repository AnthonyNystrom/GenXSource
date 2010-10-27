#include "GUIEventDispatcher.h"
#include "GUIEventListener.h"

GUIEventDispatcher::GUIEventDispatcher(void)
{
}

GUIEventDispatcher::~GUIEventDispatcher(void)
{
	while (listeners.Size())
	{
		ListenerData *ls = (ListenerData *)(*(listeners.Begin()));
		delete ls;
		VList::Iterator pos = listeners.Begin();
		listeners.Erase(pos);
	}
}

void GUIEventDispatcher::AddListener( eEventID eventID, GUIEventListener *listener )
{
	for (VList::Iterator it = listeners.Begin(); it != listeners.End(); it++)
	{
		ListenerData *ls = (ListenerData *)(*it);
		if (eventID == ls->eventID && listener == ls->listener)
		{
			UTILS_TRACE("!!! EVENT LISTENER DUBLICATION !!!");
			return;
		}
	}

	ListenerData *ls = new ListenerData();
	ls->eventID = eventID;
	ls->listener = listener;
	listeners.PushBack(ls);
}

void GUIEventDispatcher::RemoveListener( eEventID eventID, GUIEventListener *listener )
{
	for (VList::Iterator it = listeners.Begin(); it != listeners.End(); it++)
	{
		ListenerData *ls = (ListenerData *)(*it);
		if (eventID == ls->eventID && listener == ls->listener)
		{
			delete ls;
			listeners.Erase(it);
			return;
		}
	}
	FASSERT(false);
}

void GUIEventDispatcher::DispatchEvent( eEventID eventID, EventData *pData )
{
	for (VList::Iterator it = listeners.Begin(); it != listeners.End(); it++)
	{
		if (eventID == ((ListenerData *)(*it))->eventID)
		{
			((ListenerData *)(*it))->listener->OnEvent(eventID, pData);
		}
	}
}





