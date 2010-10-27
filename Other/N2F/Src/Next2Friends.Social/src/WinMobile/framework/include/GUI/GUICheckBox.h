//// =================================================================
/*!	\file GUICheckBox.h

	Revision History:

	\par [9.8.2007]	13:36 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_CHECKBOX_H__
#define __GUI_CHECKBOX_H__

#include "GUIControl.h"

/*! \brief Checkbox control.

GUICheckBox is a control that works as a trigger.
Receiving an event from eControlEvent::ECE_ON_CONTROLLER_OK_UP control it sets
its state to GUIControl::ECS_CHECKED. When the event is received for the second
time the GUIControl::ECS_CHECKED state is reset.

Example:
\code
// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a simple button in the window
button = new GUIButton(window, Rect(50, 50, 50, 20));

// Create a text button in the window(it is the same simple button but having a child GUIText control).
buttonText = new GUIButtonText(window, Rect(100, 100, 50, 20), (char16*)L"Button");

// Create a checkBox in the window.
checkBox = new GUICheckBox(window, Rect(100, 130, 20, 20));

// Check the checkBox :)
checkBox->SetState(GUIControl::ECS_CHECKED, true);

// We can test whether the checkBox is checked in the folowing way.
if (checkBox->GetState() & GUIControl::ECS_CHECKED)
{
	// your code
}

// Set focus to the button
guiSystem->SetFocus(button);
\endcode
*/
class GUICheckBox : public GUIControl
{
private:
	HANDLER_PROTOTYPE(OnCheckBoxPress);

public:
	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent			- parent control
	//! \param[in]	_rect			- control rect
	//  ***************************************************	
	GUICheckBox(GUIControl * _parent, const Rect & _rect);
	
	virtual ~GUICheckBox();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl * Clone();
};

#endif // __GUI_CHECKBOX_H__
