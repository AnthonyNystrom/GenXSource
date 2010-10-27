//// =================================================================
/*!	\file GUIAbstractScrollArea.h

	Revision History:

	\par [9.8.2007]	13:15 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __GUI_ABSTRACT_SCROLL_AREA__
#define __GUI_ABSTRACT_SCROLL_AREA__

#include "GUIControl.h"
#include "GUIScrollBar.h"
#include "GUILayoutGrid.h"


/*! \brief The GUIAbstractScrollArea control provides a scrolling area with on-demand scroll bars.

GUIAbstractScrollArea central child control is the scrolling area itself, called viewport.
The viewport control uses all available space. Next to the viewport is a vertical scroll bar,
and below a horizontal scroll bar. Each scroll bar can be either visible or hidden, (TODO: depending on the
scroll bar's policy). When a scroll bar is hidden, the viewport expands in order to cover all available space.
When a scroll bar becomes visible again, the viewport shrinks in order to make room for the scroll bar.
*/
class GUIAbstractScrollArea: public GUIControl
{
public:

	//! GUIAbstractScrollArea scroll bar type.
	enum eScrollBar
	{
		ESB_HORIZONTAL,	//!< Horizontal.
		ESB_VERTICAL	//!< Vertical.
	};

	//! GUIAbstractScrollArea scroll bar politic.
	enum eScrollBarPolitic
	{
		ESBP_AUTOMATIC,		//!< Scroll bar should appear automatically.
		ESBP_ALWAYS_SHOW,	//!< Scroll bar should be always shown.
		ESBP_ALWAYS_HIDE	//!< Never show scroll bar.
	};

	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent	- constructor
	//! \param[in]	_rect	- control rect
	//! \param[in]	_viewPortRect	- rect, specified maximal viewport size.
	//  ***************************************************
	GUIAbstractScrollArea(GUIControl * _parent, const Rect & _rect, const Rect & _viewPortRect);
	virtual ~GUIAbstractScrollArea();

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl * Clone();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool		IsClass(uint32 classType) const;

	// ***************************************************
	//! \brief    	Set maximum viewport size(rect). 
	//! \param      _viewPortRect	- maximal viewport size.
	// ***************************************************
	void				SetViewPortRect(const Rect & _viewPortRect);

	// ***************************************************
	//! \brief    	Get maximum viewport size(rect). 
	//! \return     Maximal viewport size.
	// ***************************************************
	const Rect &		GetViewPortRect();

	
	// ***************************************************
	//! \brief    	Try to make visible given rect inside viewport.
	//! \param     _visibleRect - rect to make visible.
	// ***************************************************
	void				EnsureVisible(const Rect & _visibleRect);

	//  ***************************************************
	//! \brief    	Set politic for specified scrollbar.
	//! \param[in]	scrollBar - Scroll bar type.
	//! \param[in]	politic - Politic to use for specified scroll bar type.
	//  ***************************************************
	void				SetScrollBarPolitic(eScrollBar scrollBar, eScrollBarPolitic politic);

protected:

	GUIControl * container;		//!< Container control.
	GUIControl * viewPort;		//!< View port (child of Container control).

private:
	GUIScrollBar * hScrollBar;
	GUIScrollBar * vScrollBar;
	GUILayoutGrid * gridLayout;

	eScrollBarPolitic hPolitic;
	eScrollBarPolitic vPolitic;

	void				UpdateScrollBars();
	void				UpdateStyles();


	HANDLER_PROTOTYPE(OnContainerChange);
	HANDLER_PROTOTYPE(OnViewPortChange);
	HANDLER_PROTOTYPE(OnSkinChange);

};

#endif // __GUI_ABSTRACT_SCROLL_AREA__