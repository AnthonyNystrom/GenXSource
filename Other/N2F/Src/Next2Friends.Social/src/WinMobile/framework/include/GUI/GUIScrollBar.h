//// =================================================================
/*!	\file GUIScrollBar.h

	Revision History:

	\par [9.8.2007]	13:39 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUISCROLLBAR_H__
#define __FRAMEWORK_GUISCROLLBAR_H__

#include "GUITypes.h"
#include "GUIControl.h"
#include "GUIButton.h"
#include "GUIButtonProportional.h"
#include "GUILayoutBox.h"

/*! \brief Scroll bar control.

GUIScrollBar is a control which contains GUIButtonProportional and 2 simple buttons - GUIButton - showing arrows.
A scroll bar is a control that enables the user to access parts of a document that is larger than the
control used to display it. It provides a visual indication of the user's current position within the
document and the amount of the document that is visible. Scroll bars are usually equipped with other controls
that enable more accurate navigation.
*/
class GUIScrollBar : public GUIControl
{
private:

	HANDLER_PROTOTYPE(OnSkinChange);

	uint32					arrowSize;
	GUIButton				* pButtonTop;
	GUIButton				* pButtonBottom;
	GUIButtonProportional	* pPropButton;
	GUILayoutBox			* pScrollBarLayout;

public:

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent			- Parent control.
	//! \param[in]	_rect			- Control rect.
	//! \param[in]	_orientation	- Control orientation.
	//  ***************************************************	
	GUIScrollBar(GUIControl * _parent, const Rect & _rect, eGUIOrientation _orientation = EGO_LEFTTORIGHT);
	virtual ~GUIScrollBar();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Set scroll bar proportions
	//! \param[in]	_viewSize	- visible size
	//! \param[in]	fullSize	- full size
	//  ***************************************************
	void			SetSize(uint32 _viewSize, uint32 fullSize);

	//  ***************************************************
	//! \brief    	Set scroll bar position
	//! \param[in]	_pos	- position
	//  ***************************************************
	void			SetPos(uint32 _pos);
};

#endif // __FRAMEWORK_GUISCROLLBAR_H__
