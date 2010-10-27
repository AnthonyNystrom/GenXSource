//// =================================================================
/*!	\file GUIEditLine.h

	Revision History:

	\par [9.8.2007]	13:37 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUIEDITLINE_H__
#define __FRAMEWORK_GUIEDITLINE_H__

#include "GUIControl.h"
#include "GUILayoutBox.h"
#include "GUITextLine.h"

/*!
\brief The GUIEditLine class represents a simple line of editable text.

User should activate control before to be able to edit text. Activation is
performed by pressing OK key on this control.
*/
class GUIEditLine : public GUIControl
{
private:
	GUITextLine		*pTextLine;
	GUILayoutBox	*pTextLayout;

	void			UpdateStyles();

	HANDLER_PROTOTYPE(OnSkinChange);


public:

	virtual uint32	ProcessEvent(GUIControl * const pControl, uint32 eventID, GUIEventData *pData);

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	_rect	- Control rect.
	//! \param[in]	inpText	- Initial text.
	//  ***************************************************
	GUIEditLine(GUIControl * _parent, const Rect & _rect, char16* inpText = NULL);
	virtual ~GUIEditLine();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool				IsClass(uint32 classType) const;

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

	//  ***************************************************
	//! \brief    	Get current cursor position.
	//! \return		Current cursor position.
	//  ***************************************************
	Point						GetCursorPos();

	//  ***************************************************
	//! \brief    	Set current cursor position.
	//! \param[in]	p	- New cursor position.
	//  ***************************************************
	void						SetCursorPos(Point &p);

	//  ***************************************************
	//! \brief    	Check if editline is editable.
	//! \return		Return true if text is editable, else false.
	//  ***************************************************
	bool						IsEditable();

	//  ***************************************************
	//! \brief    	Check if editline is active.
	//! \return		Return true if text is active, else false.
	//  ***************************************************
	bool						IsActive();

	void						Active(bool active);

	//  ***************************************************
	//! \brief    	Check if editline is used for password entering.
	//! \return		bool
	//  ***************************************************
	bool						IsPassword();

	//  ***************************************************
	//! \brief    	Set password entering flag.
	//! \param[in]	isPassword - new flag.
	//  ***************************************************
	void						Password(bool isPassword);

	//  ***************************************************
	//! \brief    	Check if editline is used for entering just number.
	//! \return		Return true if editline is used for entering just number, else false.
	//  ***************************************************
	bool						IsNumeric();

	//  ***************************************************
	//! \brief    	Set numeric entering flag.
	//! \param[in]	isNumeric - new flag.
	//  ***************************************************
	void						Numeric(bool isNumeric);

	//  ***************************************************
	//! \brief    	Set the maximum permitted length of the text in editline.
	//! \param[in]	maxLenghs - the maximum permitted length of the text in editline.
	//!				If maxLenghs == 0, there is no length restrictions.
	//  ***************************************************	
	void						SetMaxLenghs(uint32 maxLenghs);
};

#endif // __FRAMEWORK_GUIEDITLINE_H__