//// =================================================================
/*!	\file GUIButton.h

	Revision History:

	\par [9.8.2007]	13:15 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUIBUTTON_H__
#define __FRAMEWORK_GUIBUTTON_H__

#include "GUIControl.h"

/*! \brief Button control.

	Button is a control which at receiving eControlEvent::ECE_ON_CONTROLLER_OK_DOWN event sets its state to GUIControl::ECS_CHECKED,
	and at receiving eControlEvent::ECE_ON_CONTROLLER_OK_UP resets GUIControl::ECS_CHECKED.
	
	Button sends eControlEvent::ECE_ON_BUTTON_PRESS event at button release.

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
class GUIButton : public GUIControl
{
private:
	HANDLER_PROTOTYPE(OnButtonPress);

public:
	// ***************************************************
	//! \brief    	Constructor.
	//! \param      _parent	- parent control.
	//! \param      _rect - control rect.
	// ***************************************************
	GUIButton(GUIControl * _parent, const Rect & _rect);
	virtual ~GUIButton();

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

#endif // __FRAMEWORK_GUIBUTTON_H__
