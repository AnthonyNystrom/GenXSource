//// =================================================================
/*!	\file GUITabControl.h

	Revision History:

	\par [9.8.2007]	13:39 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_TAB_CONTROL__
#define __GUI_TAB_CONTROL__

#include "GUIControl.h"
#include "GUILayoutBox.h"
#include "GUIButtonGroup.h"
#include "GUILayoutStacked.h"

/*! \brief The GUITabControl class provides a tab bar, e.g. for use in tabbed dialogs.
*/
class GUITabControl: public GUIControl
{
public:

	//! \brief Tab buttons placement.
	enum eTabPlacement
	{
		ETP_TOP,		//!< Top.
		ETP_BOTTOM,		//!< Bottom.
		ETP_LEFT,		//!< Left.
		ETP_RIGHT		//!< Right.
	};

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	_rect	- Control rect.
	//! \param[in]	_placement - Tab buttons placement.
	//  ***************************************************
	GUITabControl(GUIControl * _parent, const Rect & _rect, eTabPlacement _placement = ETP_TOP);
	virtual ~GUITabControl();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool		IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl* Clone();

	//  ***************************************************
	//! \brief    	Add new tab page.
	//! \param[in]	_pageControl	- Control which represents new page.
	//! \param[in]	_tabControl		- Control which should be used to turn on added page (usually it is simple text button).
	//! \return		Index of created page.
	//  ***************************************************
	int32				AddTab(GUIControl * _pageControl, GUIControl * _tabControl);

	//  ***************************************************
	//! \brief		Remove tab page by its index.	
	//! \param[in]	_index - Index of tab page to remove.
	//  ***************************************************
	void				RemoveTab(int32 _index);

	//  ***************************************************
	//! \brief    	Enable tab page by its index.	
	//! \param[in]	_index - Index of tab page to enable.
	//  ***************************************************
	void				SetTabEnabled(int32 _index);

private:

	eTabPlacement placement;
	GUILayoutBox * boxGroupLayout;
	GUILayoutBox * boxLayout;
	GUIButtonGroup * buttonGroup;
	GUILayoutStacked * stackedLayout;

	HANDLER_PROTOTYPE(OnButtonGroupPress);
	HANDLER_PROTOTYPE(OnSkinChange);

	void UpdateStyles();

};

#endif // __GUI_TAB_CONTROL__