//// =================================================================
/*!	\file GUIButtonRadio.h

	Revision History:

	\par [9.8.2007]	13:16 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUI_BUTTON_RADIO_H__
#define __FRAMEWORK_GUI_BUTTON_RADIO_H__

#include "GUIControl.h"
#include "GUIText.h"
#include "GUILayoutBox.h"

/*! \brief Radio button control.

	Radio button is a container with a button in front of which there is a text.
	Nested button has eDrawType::EDT_RADIO_IMAGE and shows GUIControl::ECS_CHECKED state.

	Example:

\code

// Create window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create radio button container in the window
buttonGroup = new GUIButtonGroup(window, Rect(50, 20, 100, 160));

// Create radio button
radio1 = new GUIButtonRadio(Rect(0, 0, 80, 30), (char16*)L"radio 1");
radio2 = new GUIButtonRadio(Rect(0, 40, 80, 30), (char16*)L"radio 2");

// Create text button
button1 = new GUIButtonText(Rect(0, 80, 80, 30), (char16*)L"button 1");
button2 = new GUIButtonText(Rect(0, 120, 80, 30), (char16*)L"button 2");

// Add controls to the container
// Any control can be added to the container not only radio buttons
buttonGroup->AddItem(radio1);
buttonGroup->AddItem(radio2);
buttonGroup->AddItem(button1);
buttonGroup->AddItem(button2);

// Set focus to the radio button
guiSystem->SetFocus(radio1);

\endcode
*/
class GUIButtonRadio : public GUIControl
{
private:

	HANDLER_PROTOTYPE(OnSkinChange);
	HANDLER_PROTOTYPE(OnButtonPress);
	HANDLER_PROTOTYPE(OnStateChange);

	void	UpdateStyles();

	GUIControl	*	pRadioControl;
	GUIText *		pText;
	GUILayoutBox *	pBoxLayout;

public:
	// ***************************************************
	//! \brief    	GUIButtonRadio - constructor
	//! \param      _parent - parent control.
	//! \param      _rect - control rect.
	//! \param      pCaption - text.
	// ***************************************************
	GUIButtonRadio(GUIControl * _parent, const Rect & _rect, char16 * const pCaption = NULL);
	virtual ~GUIButtonRadio();

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************	
	virtual GUIControl*		Clone();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool			IsClass(uint32 classType) const;

	// ***************************************************
	//! \brief    	GetText - get text from the button.
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
	//! \param      align - alignment (\todo TODO: referent on alignment type)
	// ***************************************************
	virtual void			SetAlign(uint32 align);
};

#endif // __FRAMEWORK_GUI_BUTTON_RADIO_H__