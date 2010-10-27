//// =================================================================
/*!	\file GUISystem.h

	Revision History:

	\par [22.10.2007]	16:38 by Alexey Prosin
	Now you can subscribe to the window events ECE_ON_WINDOW_ACTIVATE and ECE_ON_WINDOW_DEACTIVATE
	to now about window activation and deactivation. Messages comes only if SetActiveWindow() calls!
	You can subscribe to the event EGE_ON_UPDATE in the window and recieve this event only whan window active

	\par [9.8.2007]	13:49 by Ivan Petrochenko
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUISYSTEM_H__
#define __FRAMEWORK_GUISYSTEM_H__

#include "framework/BaseTypes.h"
#include "framework/List.h"

#include "GUIControl.h"
#include "GUIWindow.h"
#include "GUIHandlerContainer.h"

class GUIController;
class GUISkin;
class Font;

/*! \brief Class GUISystem.
	Main GUI class.

	Main definitions:

	Window - rectangular area which can contain controls.

	Focus - only one control can be focused. Focused control receive messages from GUISystem (key states etc.).

	Active Window - top-layer window.

	Global Event - message sent to all opened windows and their controls.

	If control is not contained in any window, it will not be drawn or updated.
*/
class GUISystem : public GUIHandlerContainer
{
	friend class GUIWindow;

public:

	// *************************************************************
	//! \brief    	Constructor	
	// *************************************************************
	GUISystem(uint32 _width, uint32 _height);

	// *************************************************************
	//! \brief    	Destructor
	// *************************************************************
	virtual ~GUISystem();

	uint32 GetWidth() { return width; }

	uint32 GetHeight() { return height; }

	// ***************************************************
	//! \brief    	Set invalid rect.
	//!				Invalid rect will be drawn during next Render() call.
	//! \param[in]  r - invalid rect
	// ***************************************************
	void		InvalidateRect(const Rect & r);

	// ***************************************************
	//! \brief    	Make invalid control's rect.
	//! \param[in]  _control - invalidating control
	// ***************************************************
	void		InvalidateControl(GUIControl * _control);

	// ***************************************************
	//! \brief    	Get invalid rects list iterators
	// ***************************************************
	void GetInvalidRectIterators(List<Rect>::Iterator & begin, List<Rect>::Iterator & end);

	// *************************************************************
	//! \brief    	Set focus to control
	//! \param[in]  newFocus - target control
	//! \return   	old focused control
	// *************************************************************
	GUIControl * SetFocus(GUIControl * newFocus);

	// *************************************************************
	//! \brief    	Get current focus
	//! \return   	focused control
	// *************************************************************
	GUIControl * GetFocus() 
	{ 
		return focusControl; 
	}

	// ***************************************************
	//! \brief    	Deferred control deletion. Used in global handlers.
	//! \param		_control - Control for deletion
	// ***************************************************
	void DeleteLater(GUIControl * _control);

	// ***************************************************
	//! \brief    	Make window active
	//! \param      _window
	// ***************************************************
	void SetActiveWindow(GUIWindow * _window);

	void MoveWindowFront(GUIWindow * _window);

	// ***************************************************
	//! \brief    	Get active window
	//! \return   	active window
	// ***************************************************
	GUIWindow * GetActiveWindow();

	// ***************************************************
	//! \brief    	Find control bi it's ID.
	//! \param      id - ID.
	//! \return   	found control, or 0 if not found.
	// ***************************************************
	GUIControl * FindControlById(uint32 id);

	// ***************************************************
	//! \brief    	Set default skin
	//! \param      skin - new skin pointer
	//! \return   	void
	// ***************************************************
	void SetDefaultSkin(GUISkin * skin);

	// ***************************************************
	//! \brief    	Get current default skin
	//! \return   	GUISkin * current default skin
	// ***************************************************
	GUISkin * GetDefaultSkin() { return defaultSkin; }

	// *************************************************************
	//! \brief    	Make controller active
	//! \param[in]  controller
	// *************************************************************
	void AddController(GUIController * controller);

	// *************************************************************
	//! \brief    	Make controller inactive
	//! \param[in]  controller
	// *************************************************************
	void RemoveController(GUIController * controller);

	// *************************************************************
	//! \brief		Send event to controls that have appropriate event handler.
	//! \param[in]	pDestControl - control to which event should be sent
	//! \param[in]	pControl - control on which event has occurred
	//! \param[in]	eventID - event's code from \ref eControlEvent
	//! \param[in]  eventData - event's parameters
	// *************************************************************
	uint32 SendEvent(GUIControl * const pDestControl, GUIControl * const pControl, uint32 eventID, GUIEventData * eventData);

	// ***************************************************
	//! \brief    	Send global event
	//! \param      eventID - global event's ID
	//! \param      eventData - event's parameters
	// ***************************************************
	void SendGlobalEvent(uint32 eventID, GUIEventData * eventData);

	uint32 SendToActiveWindow(uint32 eventID, GUIEventData * eventData);

	void Update();
	
	// *************************************************************
	//! \brief    	Global render. Must be called once a frame.
	// *************************************************************
	void Render();

	//  ***************************************************
	//! \brief    	Get current GUISystem pointer
	//! \return		GUISystem pointer. If no GUISystem instances are created, 0 is returned.
	//!				If few instances exist, pointer to last created is returned.
	//  ***************************************************
	static GUISystem* Instance();

private:

	List<GUIControl*>			deletingControls;
	List<GUIController*>		controllerList;
	List<GUIWindow*>			windowList;
	GUIWindow					* activeWindow;
	GUIControl					* focusControl;
	List<Rect>					invalidRect;
	GUISkin						* defaultSkin;
	Font						* defaultFont;

	uint32						width;
	uint32						height;
	bool						isProcessingEvent;
};


#endif // __FRAMEWORK_GUISYSTEM_H__