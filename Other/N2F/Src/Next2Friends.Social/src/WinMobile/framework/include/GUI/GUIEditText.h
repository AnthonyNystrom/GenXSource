//// =================================================================
/*!	\file GUIEditText.h

	Revision History:

	\par [9.8.2007]	13:37 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUIEDITTEXT_H__
#define __FRAMEWORK_GUIEDITTEXT_H__

#include "GUIAbstractScrollArea.h"
#include "GUITextDocument.h"

/*!	\brief The GUIEditText class provides a control that is used to edit and display plain text.
\todo Add more additional user functions.
*/
class GUIEditText : public GUIAbstractScrollArea
{
private:
	int32			readOnlyScrollY;
	int32			readOnlyScrollX;
	GUITextDocument	*pTextDocument;

	HANDLER_PROTOTYPE(OnCursorPosChange);
	HANDLER_PROTOTYPE(OnContainerChange);
	HANDLER_PROTOTYPE(OnTextSizeChange);
	HANDLER_PROTOTYPE(OnDown);
	HANDLER_PROTOTYPE(OnUp);
	HANDLER_PROTOTYPE(OnLeft);
	HANDLER_PROTOTYPE(OnRight);

	virtual uint32				ProcessEvent(GUIControl * const pControl, uint32 eventID, GUIEventData *pData);
public:
	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent		- Parent control.
	//! \param[in]	_rect		- Control rect.
	//! \param[in]	inpText		- Text.
	//  ***************************************************
	GUIEditText(GUIControl * _parent, const Rect & _rect, char16* inpText = NULL);
	virtual ~GUIEditText();

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
	//! \brief    	Set "word wrap" mode. 
	//! \param[in]	isWordWrap - true turns word wrap mode on.
	//  ***************************************************
	virtual void				WordWrap(bool isWordWrap);

	//  ***************************************************
	//! \brief    	Set "read only" mode. 
	//! \param[in]	isReadOnly - true turns read only mode on.
	//  ***************************************************
	virtual void				ReadOnly(bool isReadOnly);
	
	//  ***************************************************
	//! \brief    	Check if word wrap mode is on.
	//! \return		Return true if word wrap mode is on.
	//  ***************************************************
	virtual bool				IsWordWrap();

	//  ***************************************************
	//! \brief    	Check if read only mode is on.
	//! \return		Return true if read only mode is on.
	//  ***************************************************
	virtual bool				IsReadOnly();

	//  ***************************************************
	//! \brief    	Check if edittext is active.
	//! \return		Return true if edittext is active.
	//  ***************************************************
	virtual bool				IsActive();
};

#endif // __FRAMEWORK_GUIEDITTEXT_H__

