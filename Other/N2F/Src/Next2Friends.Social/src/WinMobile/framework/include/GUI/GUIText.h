//// =================================================================
/*!	\file GUIText.h

	Revision History:

	\par [9.8.2007]	13:40 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_STATICTEXT_H__
#define __FRAMEWORK_STATICTEXT_H__

#include "BaseTypes.h"

#include "Vector.h"
#include "GUIControl.h"
#include "GUIParsedString.h"

/*! \brief Text control.

GUIText is a control which displays a usual UNICODE string.
It is static on default.

Example:

\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a text control. Set text and alignment.
text = new GUIText(window, Rect(50, 50, 50, 20), (char16 *)L"Text", ETA_HCENTER | ETA_VCENTER);

// Create a text control. Use default parameters.
text2 = new GUIText(window, Rect(50, 100, 50, 20));

// Specify text for the second control.
text2->SetText((char16*)L"Text 2");

// Reset static property for both controls.
// As the text control is static by default, which means thatò it cannot be focused,
// this property should be reset in order to have a possibility to focus these controls
text->SetState(GUIControl::ECS_STATIC, false);
text2->SetState(GUIControl::ECS_STATIC, false);

// Set focus to the button
guiSystem->SetFocus(text);

\endcode
*/
class GUIText : public GUIControl
{
private:
	uint32				align;

	HANDLER_PROTOTYPE(OnSizeChange);
	HANDLER_PROTOTYPE(OnTextChange);

protected:
	Vector<char16>		pText;				//!< Text characters array.
	Vector<ParsedString>parsedStrings;		//!< ParsedString array.
	Point				scrollPos;			//!< Current scroll position.

	//  ***************************************************
	//! \brief	Perform string parsing. After calling this method parsedStrings class member
	//!			will be filled with valid information.
	//  ***************************************************
	virtual void		ParseStrings();

	//  ***************************************************
	//! \brief	Draw control data. For this control text will be drawn.
	//  ***************************************************
	virtual void		DrawData();

public:	
	//  ***************************************************
	//! \brief    	
	//! \param[in]	_parent
	//! \param[in]	_rect
	//! \param[in]	pText
	//  ***************************************************
	GUIText(GUIControl * _parent, const Rect & _rect, char16 * pText = NULL);

	//  ***************************************************
	//! \brief    	
	//! \param[in]	_parent
	//! \param[in]	_rect
	//! \param[in]	isNotStatic
	//! \param[in]	pText
	//  ***************************************************
	GUIText(GUIControl * _parent, const Rect & _rect, bool isNotStatic, char16 * pText);
	virtual ~GUIText();

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

	//  ***************************************************
	//! \brief    	Set text.
	//! \param[in]	pText - Text to set.
	//  ***************************************************
	virtual void				SetText(const char16 * pText);

	//  ***************************************************
	//! \brief    	Get text.
	//! \return		Current text.
	//  ***************************************************
	virtual const char16 *		GetText();

	// ***************************************************
	//! \brief    	SetAlign - Set the alignment for the text.
	//! \param      align - Alignment \ref eALignment
	// ***************************************************
	virtual	void				SetAlign(uint32 align);

	// ***************************************************
	//! \brief    	Get the alignment for the text.
	//! \return     Current text alignment.
	// ***************************************************
	virtual uint32				GetAlign();
};

#endif // __FRAMEWORK_STATICTEXT_H__