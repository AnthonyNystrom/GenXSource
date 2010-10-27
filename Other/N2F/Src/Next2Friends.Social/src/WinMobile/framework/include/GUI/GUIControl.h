//// =================================================================
/*!	\file GUIControl.h

	Revision History:

	\par [9.8.2007]	13:36 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUICONTROL_H__
#define __FRAMEWORK_GUICONTROL_H__

#include "List.h"
#include "BaseTypes.h"
#include "Utils.h"

#include "GUITypes.h"
#include "GUIHandlerContainer.h"

class GUISkin;
class GUILayout;
class GUIWindow;

/*!
\brief Control base class.

GUIControl represents a rectangle area which can receive local or global events.

Any control has states:
Focused or not, static or not, available or not, visible or not, pressed or released, 
selected or not.

Any control can have its own event handler, and it is possible to send an event to any control.

It is possible to add any amount of child objects inherited from GUIControl to any control. 
These child objects will be drawn in their parent area.
GUISkin can be set to any control for its indiviual drawing.
GUILayout can be set to any control for individual placement of its child objects.

Example of work with GUIControl:

\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create the first control in the window
control = new GUIControl(window, Rect(0, 0, 100, 30));
// Create the second control in the first control
control2 = new GUIControl(control, Rect(0, 100, 100, 30));

// Check the class type
if (control->IsClass(ECT_CONTROL))
{
// Your code...
}

// Set an id for the second control
control2->SetId(5);

// Get the id of the second control
uint32 id = control2->GetId();

// Set the checked state
control->SetState(GUIControl::ECS_CHECKED, true);

// Test the state whether it is checked
if (control->GetState() & GUIControl::ECS_CHECKED)
{
// Your code...
}

// Get the amount of child objects
uint32 cnt = control->GetChildCount();

// Get the control according to its id
GUIControl * ctrl = control->GetChildById(5);

// Set the control rect
control->SetRect(Rect(10, 10, 50, 50));

// Get the control rect
Rect r = control->GetRect();

// Set the control drawing type
control->SetDrawType(EDT_BUTTON);

// Get the control drawing type
eDrawType dt = control->GetDrawType();

// Set focus to the control
guiSystem->SetFocus(control);

\endcode
*/
class GUIControl : public GUIHandlerContainer
{
	friend class GUISystem;

public:
	//! Control state
	enum eControlState
	{
		ECS_FOCUSED		= 0x1,		//!< The element is currently focused. The user cannot change this state. This state is controlled by GUIController.
		ECS_STATIC		= 0x2,		//!< The control is static. GUIController doesn't set focus to the static elements.
		ECS_ENABLED		= 0x4,		//!< Control enabled. This state is set on default.
		ECS_VISIBLE		= 0x8,		//!< Control visible. This state is set on default.
		ECS_CHECKED		= 0x10,		//!< Control pressed.
		ECS_SELECTED	= 0x20		//!< Control selected.
	};

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- parent control.
	//! \param[in]	_rect	- control rect.
	//  ***************************************************
	GUIControl(GUIControl * _parent, const Rect & _rect);
	virtual ~GUIControl();

	// ***************************************************
	//! \brief    	SendGlobalEvent - send global event from this control and down according to the hierarchy.
	//! \param      eventID - event
	//! \param      eventData - event data
	// ***************************************************
	void				SendGlobalEvent(uint32 eventID, GUIEventData * eventData);

	// ***************************************************
	//! \brief    	Control deletion is postponed. Is used in global handlers.
	// ***************************************************
	void				DeleteLater();

	//  ***************************************************
	//! \brief    	Return a copy (clone) of this control.
	//! \return		Control clone.
	//  ***************************************************
	virtual GUIControl * Clone();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool		IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Get state of the control.
	//! \return		Control state.
	//  ***************************************************
	uint32				GetState() const;

	//  ***************************************************
	//! \brief    	Set control state. The function cannot set ECS_FOCUSED state.
	//! \param[in]	state	- Flag(s) \ref eControlState.
	//! \param[in]	set		- true - set, false - reset.
	//  ***************************************************	
	void				SetState(uint32 state, bool set);

	//  ***************************************************
	//! \brief    	Set the control state. The function cannot set ECS_FOCUSED flag.
	//! \param[in]	state	- new state.
	//  ***************************************************
	void				SetState(uint32 state);

	//  ***************************************************
	//! \brief    	Get child elements count.
	//! \return		The number of child elements.
	//  ***************************************************
	virtual uint32		GetChildCount();

	//  ***************************************************
	//! \brief		Set a parent for this control.
	//! \param[in]	pParent - parent control or NULL for reset.
	//  ***************************************************
	void				SetParent(GUIControl * pParent);

	//  ***************************************************
	//! \brief    	Get the parent control.
	//! \return		Parent control.
	//  ***************************************************
	GUIControl*			GetParent();

	//  ***************************************************
	//! \brief    	Get the root element in the current control hierarchy.
	//! \return		Root control. 
	//  ***************************************************
	GUIWindow *			GetRootWindow();

	// ***************************************************
	//! \brief		Get the iterators of the child list.
	//! \param[out]	begin - First child iterator.
	//! \param[out]	end - Last child iterator.
	// ***************************************************
	virtual void		GetChildIterators(List<GUIControl*>::Iterator & begin, List<GUIControl*>::Iterator & end);

	// *************************************************************
	//! \brief    	Find an object with the specific ID among the child controls. The search will include child controls of child controls, etc. 
	//! \param[in]  _id	- ID of the child control
	//! \return   	Found control, or NULL if there is no such ID.
	// *************************************************************
	virtual GUIControl*	GetChildById(uint32 _id);

	// *************************************************************
	//! \brief    	Set ID.
	//! \param[in]  _id	- new ID value.
	// *************************************************************
	void				SetId(uint32 _id);

	// *************************************************************
	//! \brief    	Get ID value.
	//! \return   	ID value.
	// *************************************************************
	uint32				GetId();

	//  ***************************************************
	//! \brief    	Get control rect.
	//! \return		Control rect.
	//  ***************************************************
	const Rect &		GetRect();

	//  ***************************************************
	//! \brief    	Set control rect.
	//! \param[in]	rect	- new control rect.
	//  ***************************************************
	void				SetRect(const Rect &rect);

	//  ***************************************************
	//! \brief    	Get the control rect in screen coordinates.
	//! \return		Control rect in screen coordinates.
	//  ***************************************************
	const Rect &		GetScreenRect() const;

	// ***************************************************
	//! \brief    	GetIntersectRect - get the intersection rectangle of the current control and parent one.
	//! \return   	const Rect & - intersection rectangle
	// ***************************************************
	const Rect &		GetIntersectRect() const;

	//  ***************************************************
	//! \brief    	Get GUISkin of the given control.
	//! \return		Control GUISkin, if it is installed, else - NULL.
	//  ***************************************************
	GUISkin*			GetSkin();

	//  ***************************************************
	//! \brief    	Set control skin.
	//! \param[in]	pSkin - GUISkin being set.
	//  ***************************************************
	virtual void		SetSkin(GUISkin *pSkin);

	//  ***************************************************
	//! \brief    	Get control layout.
	//! \return		Control GUILayout, if it is installed, else - NULL.
	//  ***************************************************
	GUILayout *			GetLayout();

	//  ***************************************************
	//! \brief    	Set control layout.
	//! \param[in]	_layout - GUILayout being set.
	//  ***************************************************
	void				SetLayout(GUILayout * _layout);

	// ***************************************************
	//! \brief    	Remove control layout.
	//! \return   	Removed layout pointer.
	// ***************************************************
	GUILayout *			RemoveLayout();

	//  ***************************************************
	//! \brief    	Get the drawing type of the control.
	//! \return		Drawing type.
	//  ***************************************************
	eDrawType			GetDrawType() const;

	//  ***************************************************
	//! \brief    	Set the control drawing type.
	//! \param[in]	_drawType - new drawing type.
	//  ***************************************************
	void				SetDrawType(eDrawType _drawType);

	//  ***************************************************
	//! \brief    	Draw the control and its child controls in the specified screen coordinates.
	//  ***************************************************
	virtual void		Draw();

	// ***************************************************
	//! \brief    	UpdateScreenRect - recount the screen and visible coordinates of the control down the hierarchy.
	//!				The function is called automatically when it is needed and as a rule the user doesn't need to call it.
	// ***************************************************
	virtual void		UpdateScreenRect();

protected:
	virtual void		DrawSkin();		//!< Draw control skin.
	virtual void		DrawData();		//!< Draw control data. This function is called after DrawSkin.

	Rect				rect;			//!< Control rect.
	Rect				screenRect;		//!< Control screen rect (real screen rect).
	Rect				intersectRect;	//!< Control intersect rect.

	GUIControl *		pParent;		//!< Parent control
	List<GUIControl*>	childList;		//!< Child controls list.

	GUIWindow *			rootWindow;		//!< Pointer on root window.

	//  ***************************************************
	//! \brief    	Set the state flag.
	//! \param[in]	state	- Flag(s) \ref eControlState.
	//! \param[in]	set		- true - set, false - reset.
	//  ***************************************************
	void				SetStatePrivate(uint32 state, bool set);

	//  ***************************************************
	//! \brief    	Set the control state.
	//! \param[in]	state - New state.
	//  ***************************************************
	void				SetStatePrivateFull(uint32 state);

private:
	GUISkin *			pSkin;
	GUILayout *			pLayout;

	eDrawType			drawType;
	uint32				state;
	uint32				id;

	HANDLER_PROTOTYPE(OnSkinChange);
	HANDLER_PROTOTYPE(OnStateChange);
};

#endif // __FRAMEWORK_GUICONTROL_H__
