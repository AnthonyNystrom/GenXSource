//// =================================================================
/*!	\file GUITextDocument.h

	Revision History:

	\par [9.8.2007]	13:40 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUITEXTDOCUMENT_H__
#define __FRAMEWORK_GUITEXTDOCUMENT_H__

#include "GUIControl.h"
#include "GUIParsedString.h"
#include "Vector.h"

/*! \brief The GUITextDocument class holds formatted text that can be viewed and edited using a GUIEditText.
*/ 
class GUITextDocument : public GUIControl
{
private:
	Vector<char16>				pText;
	uint32						align;
	Vector<ParsedString>		parsedStrings;

	Vector<char16>::Iterator	cursorPosIterator;
	Point						cursorPos;
	Rect						cursorRect;

	int32						wordWrap;

	bool						isWordWrap;
	bool						isReadOnly;
	bool						isActive;

	virtual	void				UpdatePos();
	virtual void				DrawData();
	uint32						GetLenFromCharToWordEnd(char16 *ch);
	uint32						GetLenFromCharToEndLineChar(char16 *ch);
	void						InvalidateCursorRect();
	virtual void				ParseStrings();

	HANDLER_PROTOTYPE(OnKeyDown);
	HANDLER_PROTOTYPE(OnTextEdit);
	HANDLER_PROTOTYPE(OnTextChange);
	HANDLER_PROTOTYPE(OnLeft);
	HANDLER_PROTOTYPE(OnRight);
	HANDLER_PROTOTYPE(OnDown);
	HANDLER_PROTOTYPE(OnUp);
	HANDLER_PROTOTYPE(OnBackspace);
	HANDLER_PROTOTYPE(OnOk);
	HANDLER_PROTOTYPE(OnSizeChange);
	HANDLER_PROTOTYPE(OnUpdate);

public:
	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent  - Parent control.
	//! \param[in]	_rect	 - Control rect.
	//! \param[in]	inpText	 - Initial text.
	//  ***************************************************
	GUITextDocument(GUIControl * _parent, const Rect & _rect, char16* inpText = NULL);
	virtual ~GUITextDocument();

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
	//! \param      align - Alignment
	// ***************************************************
	virtual	void				SetAlign(uint32 align);

	// ***************************************************
	//! \brief    	Get the alignment for the text.
	//! \return     Current text alignment.
	// ***************************************************
	virtual uint32				GetAlign();

	//  ***************************************************
	//! \brief    	Set word wrap length.
	//! \param[in]	ww - word wrap length.
	//  ***************************************************
	void						SetWordWrap(int32 ww);

	//  ***************************************************
	//! \brief    	Get word wrap length.
	//! \return		Word wrap length.
	//  ***************************************************
	uint32						GetWordWrap();

	//  ***************************************************
	//! \brief    	Get current cursor position.
	//! \return		Current cursor position.
	//  ***************************************************
	Point						GetCursorPos();

	//  ***************************************************
	//! \brief    	Set current cursor position.
	//! \param[in]	p - New cursor position.
	//  ***************************************************
	void						SetCursorPos(Point &p);

	//  ***************************************************
	//! \brief    	Get cursor rect in pixels.
	//! \return		Cursor rect in pixels.
	//  ***************************************************
	virtual	Rect				GetCursorRect();

	//  ***************************************************
	//! \brief    	Check for "word wrap" mode.
	//! \return		Return true if "word wrap" mode is on.
	//  ***************************************************
	virtual bool				IsWordWrap();

	//  ***************************************************
	//! \brief    	Set "word wrap" mode. 
	//! \param[in]	isWordWrap - true turns word wrap mode on.
	//  ***************************************************
	virtual void				WordWrap(bool isWordWrap);

	//  ***************************************************
	//! \brief    	Check if text is editable.
	//! \return		Return true if text is editable, else false.
	//  ***************************************************
	virtual bool				IsReadOnly();

	//  ***************************************************
	//! \brief    	Set "read only" mode. 
	//! \param[in]	isReadOnly - true turns read only mode on.
	//  ***************************************************
	virtual	void				ReadOnly(bool isReadOnly);

	//  ***************************************************
	//! \brief    	Check if text is active.
	//! \return		Return true if text is active, else false.
	//  ***************************************************
	virtual bool				IsActive();

	//  ***************************************************
	//! \brief    	Get vector of parsed strings corresponding to current text.
	//!				It is usually used by skin.
	//! \return		Vector of parsed strings corresponding to current text.
	//  ***************************************************
	Vector<ParsedString>*		GetParsedStrings();
};

#endif // __FRAMEWORK_GUITEXTDOCUMENT_H__