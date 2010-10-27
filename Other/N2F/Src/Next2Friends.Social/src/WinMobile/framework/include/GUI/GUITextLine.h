//// =================================================================
/*!	\file GUITextLine.h

	Revision History:

	\par [9.8.2007]	13:40 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUITEXTLINE_H__
#define __FRAMEWORK_GUITEXTLINE_H__

#include "GUIText.h"

/*!
\brief The GUITextLine class represents a simple line of editable text.

User should activate control before to be able to edit text. Activation is
performed by pressing OK key on this control. Use GUIEditLine instead of this class.
*/
class GUITextLine : public GUIText
{
private:
	Vector<char16>::Iterator	cursorPosIterator;
	Point						cursorPos;
	Rect						cursorRect;

	bool						isReadOnly;
	bool						isActive;
	bool						isNumeric;
	bool						isPassword;

	uint32						maxLenghs;

	void UpdatePos();
	void InvalidateCursorRect();
	virtual void DrawData();

	HANDLER_PROTOTYPE(OnFocusLost);
	HANDLER_PROTOTYPE(OnKeyDown);
	HANDLER_PROTOTYPE(OnTextEdit);
	HANDLER_PROTOTYPE(OnTextChange);
	HANDLER_PROTOTYPE(OnLeft);
	HANDLER_PROTOTYPE(OnRight);
	HANDLER_PROTOTYPE(OnBackspace);
	HANDLER_PROTOTYPE(OnOk);
	HANDLER_PROTOTYPE(OnSizeChange);
	HANDLER_PROTOTYPE(OnUpdate);

public:
	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent		- Parent control.
	//! \param[in]	_rect		- Control rect.
	//! \param[in]	inpText		- Text.
	//! \param[in]	readOnly	- Read-only property.
	//  ***************************************************
	GUITextLine(GUIControl * _parent, const Rect & _rect, char16* inpText = NULL, bool readOnly = false);
	virtual ~GUITextLine();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool	IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Get current cursor position.
	//! \return		Current cursor position.
	//  ***************************************************
	Point			GetCursorPos();

	//  ***************************************************
	//! \brief    	Set current cursor position.
	//! \param[in]	p	- New cursor position.
	//  ***************************************************
	void			SetCursorPos(Point &p);

	//  ***************************************************
	//! \brief    	Get cursor rect in pixels.
	//! \return		Cursor rect in pixels.
	//  ***************************************************
	Rect			GetCursorRect();

	//  ***************************************************
	//! \brief    	Get current scroll position.
	//! \return		Current scroll position.
	//  ***************************************************
	Point			GetScrollPos();

	//  ***************************************************
	//! \brief    	Set current scroll position.
	//! \param[in]	p - New scroll position.
	//  ***************************************************
	void			SetScrollPos(Point p);

	//  ***************************************************
	//! \brief    	Check if textline is editable.
	//! \return		Return true if text is editable, else false.
	//  ***************************************************
	bool			IsEditable();

	//  ***************************************************
	//! \brief    	Check if text line is active.
	//! \return		Return true if text is active, else false.
	//  ***************************************************
	bool			IsActive();

	//  ***************************************************
	//! \brief    	Change text line activation.
	//! \param[in]	active - New text line activation state.
	//  ***************************************************
	void			Active(bool active);

	//  ***************************************************
	//! \brief    	Check if text line is used for password entering.
	//! \return		bool
	//  ***************************************************
	bool			IsPassword();

	//  ***************************************************
	//! \brief    	Set password entering flag.
	//! \param[in]	isPassword - new flag.
	//  ***************************************************
	void			Password(bool isPassword);

	//  ***************************************************
	//! \brief    	Check if textline is used for entering just number.
	//! \return		Return true if textline is used for entering just number, else false.
	//  ***************************************************
	bool			IsNumeric();

	//  ***************************************************
	//! \brief    	Set numeric entering flag.
	//! \param[in]	isNumeric - new flag.
	//  ***************************************************
	void			Numeric(bool isNumeric);

	//  ***************************************************
	//! \brief    	Set the maximum permitted length of the text in textline.
	//! \param[in]	maxLenghs - the maximum permitted length of the text in textline.
	//!				If maxLenghs == 0, there is no length restrictions.
	//  ***************************************************
	void			SetMaxLenghs(uint32 maxLenghs);
};

#endif // __FRAMEWORK_GUITEXTLINE_H__