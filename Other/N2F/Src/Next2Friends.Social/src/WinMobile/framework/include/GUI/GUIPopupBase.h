//// =================================================================
/*!	\file GUIPopupBase.h

	Revision History:

	\par [9.8.2007]	13:38 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_POPUPBASE__
#define __GUI_POPUPBASE__

#include "BaseTypes.h"
#include "Font.h"
#include "GUILayout.h"
#include "GUIButton.h"
#include "GUIWindow.h"

/*! \brief Pop-up button control.

Basic class for all derivative classes which open a window when the button is pressed.
*/
class GUIPopupBase: public GUIButton
{
public:

	//! Association of a pop-up window with the button
	enum ePopupAlign
	{
		EPD_NONE,			//!< no association
		EPD_LEFT_DOWN,		//!< Left-down.
		EPD_LEFT_UP,		//!< Left-up.
		EPD_RIGHT_DOWN,		//!< Right-down.
		EPD_RIGHT_UP,		//!< Right-up.
		EPD_UP_LEFT,		//!< Up-left.
		EPD_UP_RIGHT,		//!< Up-right.
		EPD_DOWN_LEFT,		//!< Down-left.
		EPD_DOWN_RIGHT		//!< Down-right.
	};

	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent		- Parent control.
	//! \param[in]	_rect		- Control rect.
	//! \param[in]	_popupRect	- Pop-up window rect.
	//! \param[in]	_align		- Association of a window to a button.
	//  ***************************************************	
	GUIPopupBase(GUIControl * _parent, const Rect & _rect, const Rect & _popupRect, ePopupAlign _align);
	virtual ~GUIPopupBase();

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

	//  ***************************************************
	//! \brief    	Set the control which will be in focused after the popup button is pressed.
	//! \param[in]	_control	- control
	//  ***************************************************	
	virtual void			SetFocusedControl(GUIControl * _control);

	// ***************************************************
	//! \brief    	Get the control which is in pop-up window focus.
	//! \return   	The control which is in pop-up window focus.
	// ***************************************************
	virtual GUIControl *	GetFocusedControl();

	//  ***************************************************
	//! \brief    	Add a control to the pop-up window.
	//! \param[in]	pControl	- Control to add.
	//  ***************************************************
	virtual uint32			AddItem(GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Remove the control from the pop-up window.
	//! \param[in]	pControl	- Control to remove.
	//  ***************************************************
	virtual void			RemoveItem(GUIControl * const pControl);

	// *************************************************************
	//! \brief    	Find an object with the specified ID among the child controls of the pop-up window. Child controls of child controls etc will be taken into account while searching. 
	//! \param[in]  _id	- ID of the child control
	//! \return   	Control found, or NULL if there is no such ID.
	// *************************************************************
	virtual GUIControl*		GetChildById(uint32 _id);

	//  ***************************************************
	//! \brief		Set the pop-up window rect
	//! \param[in]	_rect	- Rect to set.
	//  ***************************************************
	void					SetPopupRect(const Rect & _rect);

	//  ***************************************************
	//! \brief		Get the pop-up window rect.
	//! \return		The pop-up window rect.
	//  ***************************************************
	const Rect &			GetPopupRect();

	//  ***************************************************
	//! \brief    	Get pop-up align.
	//! \return		Return pop-up align.
	//  ***************************************************
	ePopupAlign				GetAlign();

	//  ***************************************************
	//! \brief		Open pop-up window
	//  ***************************************************
	void					OpenPopup();

	//  ***************************************************
	//! \brief		Close pop-up window
	//  ***************************************************
	void					ClosePopup();

protected:

	//  ***************************************************
	//! \brief		The function is called when the pop-up window is opened.
	//!				It can be redefined in the derivative classes.
	//!				It serves for setting the pop-up window before its opening (sizes, position, etc)
	//  ***************************************************
	virtual void	OnOpen();

	//  ***************************************************
	//! \brief		The function is called when the pop-up window is closed.
	//!				It can be redefined in the derivative classes.
	//!				It serves to perform settings before closing the pop-up window (focus setting, etc)
	//  ***************************************************
	virtual void	OnClose();

private:
	uint32			OnPopupPress(GUIControl * const pControl, uint32 eventID, GUIEventData *pData);
	uint32			OnSelectedChange(GUIControl * const pControl, uint32 eventID, GUIEventData *pData);

	void			CreatePopupWindow();
	void			ClosePopupWindow();

	Rect			popupRect;
	GUIWindow *		popupWindow;
	GUIWindow *		exWindow;

	GUIControl *	focusedControl;
	ePopupAlign		align;

	List<GUIControl*> popupChildList;
};

#endif // __GUI_POPUPBASE__