//// =================================================================
/*!	\file GUIButtonGroup.h

	Revision History:

	\par [9.8.2007]	13:15 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_BUTTON_GROUP__
#define __GUI_BUTTON_GROUP__

#include "GUILayoutGrid.h"
#include "GUIControl.h"

/*! \brief Radio button container control

Radio button container is a container which controls GUIControl::ECS_CHECKED state of all the radio buttons
added to it so that only one radio button would have GUIControl::ECS_CHECKED state

Example:

\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create Radio button container in the window
buttonGroup = new GUIButtonGroup(window, Rect(50, 20, 100, 160));

// Create Radio button
radio1 = new GUIButtonRadio(NULL, Rect(0, 0, 80, 30), (char16*)L"radio 1");
radio2 = new GUIButtonRadio(NULL, Rect(0, 40, 80, 30), (char16*)L"radio 2");

// Create text button
button1 = new GUIButtonText(NULL, Rect(0, 80, 80, 30), (char16*)L"button 1");
button2 = new GUIButtonText(NULL, Rect(0, 120, 80, 30), (char16*)L"button 2");

// Add radio buttons to the container
// Any control can be added to the container not only radio buttons
buttonGroup->AddItem(radio1);
buttonGroup->AddItem(radio2);
buttonGroup->AddItem(button1);
buttonGroup->AddItem(button2);

// Set focus to the radio button
guiSystem->SetFocus(radio1);

\endcode
*/
class GUIButtonGroup: public GUIControl
{
public:

	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent			- parent control
	//! \param[in]	_rect			- control rect
	//  ***************************************************	
	GUIButtonGroup(GUIControl * _parent, const Rect & _rect);
	virtual ~GUIButtonGroup();

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl * Clone();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool		IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Add a child control.
	//! \param[in]	pControl	- Child control being added.
	//  ***************************************************
	void				AddItem(GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Remove child control.
	//! \param[in]	pControl	- Child control being removed.
	//  ***************************************************
	void				RemoveItem(GUIControl * const pControl);

	// ***************************************************
	//! \brief    	RemoveItem - remove child control 
	//! \param      _index - removed control index
	// ***************************************************
	void				RemoveItem(int32 _index);

	// ***************************************************
	//! \brief    	GetPressedButtonIndex - get the index of the currently checked radio button.
	//! \return   	If none of the buttons checked, index is -1
	// ***************************************************
	int32				GetPressedButtonIndex();

private:

	HANDLER_PROTOTYPE(OnButtonPress);
};

#endif //__GUI_BUTTON_GROUP__
