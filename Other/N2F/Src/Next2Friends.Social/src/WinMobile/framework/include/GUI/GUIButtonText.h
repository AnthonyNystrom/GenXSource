//// =================================================================
/*!	\file GUIButtonText.h

	Revision History:

	\par [9.8.2007]	13:36 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUIBUTTONTEXT_H__
#define __FRAMEWORK_GUIBUTTONTEXT_H__

#include "GUIButton.h"
#include "GUIText.h"
#include "GUILayoutBox.h"

/*! \brief Button control with text.

GUIButtonText is a simple GUIButton, which contains one GUIText child by default.
GUIButtonText also has GUIBoxLayout. It can be received with GetLayout() function and used 
to control GUIText placement.

Example:

\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a simple button
button = new GUIButton(window, Rect(50, 50, 50, 20));

// Create a text button (it is the same simple button but having GUIText child).
buttonText = new GUIButtonText(window, Rect(100, 100, 50, 20), (char16*)L"Button");

// Create a checkBox.
checkBox = new GUICheckBox(window, Rect(100, 130, 20, 20));

// Check the checkBox
checkBox->SetState(window, GUIControl::ECS_CHECKED, true);

// One can test whether the checkBox is checked in the following way
if (checkBox->GetState() & GUIControl::ECS_CHECKED)
{
	// your code
}

// Set focus to the button
guiSystem->SetFocus(button);

\endcode
*/
class GUIButtonText : public GUIButton
{
private:
	GUILayoutBox *	boxLayout;
	GUIText *		pText;

	void			UpdateStyles();

	HANDLER_PROTOTYPE(OnSkinChange);

public:
	// ***************************************************
	//! \brief    	GUIButtonText - Constructor.
	//! \param      _parent - Parent control.
	//! \param      _rect - Control rect.
	//! \param      pCaption - Text.
	// ***************************************************
	GUIButtonText(GUIControl * _parent, const Rect & _rect, char16 * const pCaption = NULL);
	virtual ~GUIButtonText();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool			IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl *	Clone();

	// ***************************************************
	//! \brief    	GetText - Get text from the button.
	//! \return   	Text.
	// ***************************************************
	virtual const char16 *	GetText();

	// ***************************************************
	//! \brief    	SetText - Set text to the button.
	//! \param      pCaption - Text.
	// ***************************************************
	virtual void			SetText(const char16 * pCaption);

	// ***************************************************
	//! \brief    	SetAlign - Set the alignment for the text.
	//! \param      align - Alignment
	// ***************************************************
	virtual void			SetAlign(uint32 align);
};

#endif // __FRAMEWORK_GUIBUTTONTEXT_H__