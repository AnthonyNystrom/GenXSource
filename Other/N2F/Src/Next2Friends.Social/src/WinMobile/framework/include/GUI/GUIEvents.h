//// =================================================================
/*! \file GUIEvents.h

	Revision History:
	\par [22.10.2007]	16:38 by Alexey Prosin
	Adding events for window activation and deactivation
	Adding events for parets set/lost

	\par [11.6.2007]	16:38 by Sergey Zdanevich
	Comments.

	\par [8.5.2007]	12:46 by Sergey Zdanevich
	File created
*//// ==================================================================

#ifndef __FRAMEWORK_GUIEVENTS_H__
#define __FRAMEWORK_GUIEVENTS_H__

#include "BaseTypes.h"

/*! \page eventsPage GUI Event
For handling different GUI system events there is a mechanism of setting user handlers.
The handler can be set both at the events of any control element - \ref sectionLocal, and
at \ref sectionGlobal GUI. Handler of any "C" control event is set by calling
\code
"C"::AddHandler(uint32 eventID, T* pClass, uint32 (T::*pFunction)(GUIControl * const pControl, uint32 eventID, GUIEventData *pData))
\endcode
function where:
\li eventID	- event ID, which is used to call user handler.
\li pClass - pointer to a class containing user handler.
\li pFunction - pointer to the user handler - function with f(GUIControl * const pControl, uint32 eventID, GUIEventData *pData) prototype
*/

/*!
\page eventsPage
\section sectionLocal Local events
It is event #eControlEvent occurring on any control and which doesn't proceed to its hierarchy elements. At this event "E" the control will run (call functions)
all the user handlers which are corresponding to the given "E" event.
*/
//! Local GUI events.
enum eControlEvent
{
	// GUIControl and derivatives
	ECE_ON_FOCUS_SET,				//!< The element is focused.
	ECE_ON_FOCUS_LOST,				//!< The element has lost the focus.

	ECE_ON_STATE_CHANGE,			//!< Control state changed

	ECE_ON_SIZE_CHANGE,				//!< Element has changed the size or/and the position.

	ECE_ON_SKIN_CHANGE,				//!< The element skin has been changed

	ECE_ON_WINDOW_ACTIVATE,			//!< Sends to a activated window on GUISystem::SetActiveWindow
	ECE_ON_WINDOW_DEACTIVATE,		//!< Sends to a deactivated window on GUISystem::SetActiveWindow

	ECE_ON_PARENT_SET,				//!< Sends to a control when we set parent for control
	ECE_ON_PARENT_LOST,				//!< Sends to a control when we remove parent from control. Event sends before parent is really lost and control has possibility to make some actions with the old parent

	ECE_ON_CONTROLLER_OK_DOWN,		//!< Selection key is pressed
	ECE_ON_CONTROLLER_OK_UP,		//!< Selection key is released
	ECE_ON_CONTROLLER_ESC,			//!<
	ECE_ON_CONTROLLER_CLR,			//!<
	ECE_ON_CONTROLLER_LEFT,			//!<
	ECE_ON_CONTROLLER_RIGHT,		//!<
	ECE_ON_CONTROLLER_UP,			//!<
	ECE_ON_CONTROLLER_DOWN,			//!<

	ECE_ON_CONTROLLER_REACHED_LEFT,		//!<
	ECE_ON_CONTROLLER_REACHED_RIGHT,	//!<
	ECE_ON_CONTROLLER_REACHED_TOP,		//!<
	ECE_ON_CONTROLLER_REACHED_BOTTOM,	//!<

	ECE_ON_KEY_DOWN,				//!< A key press occurred on the element.
	ECE_ON_KEY_UP,					//!< A key release occurred on the element.
	ECE_ON_KEY_TIMEUP,				//!<

	// GUIButton and derivatives
	ECE_ON_BUTTON_PRESS,			//!< Key press.

	// GUINumeric and derivatives
	ECE_ON_NUMERIC_VALUE_CHANGE,	//!< numeric control value has been changed.

	// GUIImage and derivatives
	ECE_ON_IMAGE_CHANGED,			//!< Image changed
	ECE_ON_IMAGE_FRAME_CHANGED,		//!< Image frame changed
	ECE_ON_IMAGE_ANIMATION_DONE,	//!< Animation playback is finished.

	// GUIAbstractScrollArea and derivatives
	ECE_ON_VIEW_PORT_CHANGE,		//!< ViewPort changed

	// GUIScrollView and derivatives
	ECE_ON_SCROLLITEM_SELECT,		//!< Element selection occurred in GUIScrollView
	ECE_ON_SCROLLFOCUS_CHANGE,		//!< Focus changed in GUIScrollView

	// GUIButtonGroup and derivatives
	ECE_ON_BUTTON_GROUP_PRESS,		//!< A group press occurred

	// GUIEditText and derivatives
	ECE_ON_TEXT_EDIT,				//!< Text is being edited.
	ECE_ON_TEXT_CHANGE,				//!< Text has been changed.
	ECE_ON_TEXT_CURSOR_CHANGE,		//!< Cursor position in the text has changed.
	ECE_ON_FINISH_EDIT,				//!< Editing is over.

	ECE_ON_SCROLLBAR_CHANGE,		//!< ScrollBar has been changed.

	ECE_USER = 0x4000				//!< The beginning of user local notifications ID 

};


/*! 
\page eventsPage
\section sectionGlobal Global events
#eGlobalEvent events occurring on all controls (all the GUI windows and its child objects) in the order of their position 
in the hierarchy, from the parent element to the child element. Take an example of update as a global event,
which occurs at every frame.
*/
//! GUI global events.
enum eGlobalEvent
{
	EGE_ON_UPDATE = 0x8000, //!< Update event (comes once a frame)
	EGE_ON_DEFAULTSKIN_CHANGE,		//!< Default skin has been changed

	EGE_USER = 0xC000		//!< The beginning of user global notifications ID
};

#endif // __FRAMEWORK_GUIEVENTS_H__