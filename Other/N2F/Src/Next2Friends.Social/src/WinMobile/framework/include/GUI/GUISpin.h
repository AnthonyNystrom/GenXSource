//// =================================================================
/*!	\file GUISpin.h

	Revision History:

	\par [9.8.2007]	13:39 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_SPIN__
#define __GUI_SPIN__

#include "GUINumeric.h"
#include "GUITextLine.h"
#include "GUIButton.h"
#include "GUILayoutGrid.h"

/*! \brief The GUISpin class provides a spin box control.

GUISpin allows the user to choose a value by clicking the up/down buttons or pressing up/down on the keyboard
to increase/decrease the value currently displayed. The user can also type the value in manually. If the
value is entered directly into the spin box, the value will be changed and with the new value when Enter/Return
is pressed, when the spin box looses focus or when the spin box is deactivated.
*/
class GUISpin: public GUINumeric
{
public:

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	_rect	- Control rect.
	//  ***************************************************
	GUISpin(GUIControl * _parent, const Rect & _rect);
	virtual ~GUISpin();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool	IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl * Clone();

	virtual uint32 ProcessEvent(GUIControl * const pControl, uint32 eventID, GUIEventData *pData);
private:

	GUITextLine * editLine;
	GUIButton * upButton;
	GUIButton * downButton;
	GUILayoutGrid * gridLayout;

	void UpdateStyles();

	HANDLER_PROTOTYPE(OnNumericValueChange);
	HANDLER_PROTOTYPE(OnFinishEdit);
	HANDLER_PROTOTYPE(OnControllerUp);
	HANDLER_PROTOTYPE(OnControllerDown);
	HANDLER_PROTOTYPE(OnSkinChange);
};

#endif // __GUI_WINDOW__