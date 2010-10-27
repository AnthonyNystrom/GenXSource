//// =================================================================
/*!	\file GUIButtonProportional.h

	Revision History:

	\par [9.8.2007]	13:16 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_BUTTONPROPORTIONAL_H__
#define __GUI_BUTTONPROPORTIONAL_H__

#include "GUITypes.h"
#include "GUIControl.h"
#include "GUIButton.h"

/*! \brief Proportional button control.

Proportional button is a button which is in the container
and its size and position depends on the proportion set by functions SetSize() and SetPos().

Example:
\code

// Create window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create proportional button with left-to-right orientation in the window
buttonProp = new GUIButtonProportional(window, Rect(10, 60, 130, 30), EGO_LEFTTORIGHT);
// Create proportional button with up-to-down orientation in the window
buttonProp2 = new GUIButtonProportional(window, Rect(100, 100, 20, 100), EGO_UPTODOWN);

// Set the proportion for the first button
// The first parameter is the number of visible elements, the second one is the total number of elements
buttonProp->SetSize(1, 1000);

// First visible element number
buttonProp->SetPos(500);

// Set proportion for the second button
buttonProp2->SetSize(5, 20);
buttonProp2->SetPos(5);

// Set focus to the button
guiSystem->SetFocus(buttonProp);

\endcode
*/
class GUIButtonProportional: public GUIControl
{
public:

	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent			- parent control
	//! \param[in]	_rect			- control rect
	//! \param[in]	_orientation	- control orientation
	//  ***************************************************	
	GUIButtonProportional(GUIControl * _parent, const Rect & _rect, eGUIOrientation _orientation = EGO_LEFTTORIGHT);

	virtual ~GUIButtonProportional();

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
	virtual bool	IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Set button proportions
	//! \param[in]	_viewSize	- visible size
	//! \param[in]	fullSize	- full size
	//  ***************************************************
	void			SetSize(uint32 _viewSize, uint32 fullSize);

	//  ***************************************************
	//! \brief    	Set button position
	//! \param[in]	_pos	- position
	//  ***************************************************
	void			SetPos(uint32 _pos);

private:

	HANDLER_PROTOTYPE(OnSkinChange);
	HANDLER_PROTOTYPE(OnSizeChange);

	void			Recalculate();
	void			UpdateStyles();

	GUIButton		* button;

	eGUIOrientation orientation;
	uint32			viewSize;
	uint32			fullSize;
	uint32			pos;
	uint32			barMinSize;
	uint32			barMaxSize;
};


#endif // __GUI_BUTTONPROPORTIONAL_H__