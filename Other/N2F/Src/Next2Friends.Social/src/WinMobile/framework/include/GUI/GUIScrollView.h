//// =================================================================
/*!	\file GUIScrollView.h

	Revision History:

	\par [9.8.2007]	13:39 by Sergey Zdanevich
	File created.
*/// ==================================================================
	

#ifndef __FRAMEWORK_GUISCROLLVIEW_H__
#define __FRAMEWORK_GUISCROLLVIEW_H__

#include "Utils.h"
#include "GUIAbstractScrollArea.h"

/*! \brief Scroll view control.

GUIScrollView is a container which places its child objects in the specified virtual area
according to their coordinates. The virtual area can be much bigger than the container size, 
at this the container controls that the focused child element is always visible, scrolling the
virtual area. The scroll bars appear in the container in case they are needed.

Example:

\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a ScrollView control
// ScrollView rect is set as the second parameter
// Virtual control rect is set as the 3rd parameter,
// this rect will be scrolled and all the added controls will appear in it.
scrollView = new GUIScrollView(window, Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()), Rect(0, 0, 300, 300));

// Create controls to be added to ScrollView
buttonScr1 = new GUIButtonText(NULL, Rect(0, 0, 100, 30), (char16*)L"button1");
buttonScr2 = new GUIButtonText(NULL, Rect(0, 150, 100, 30), (char16*)L"button2");
buttonScr3 = new GUIButtonText(NULL, Rect(200, 250, 100, 30), (char16*)L"button3");

// Add child objects to ScrollView
scrollView->AddItem(buttonScr1);
scrollView->AddItem(buttonScr2);
scrollView->AddItem(buttonScr3);

// Set focus to the scrollView
guiSystem->SetFocus(scrollView);

\endcode
*/
class GUIScrollView : public GUIAbstractScrollArea
{
private:
	GUIControl			*focusedControl;

	HANDLER_PROTOTYPE(OnFocusChange);
	HANDLER_PROTOTYPE(OnSizeChange);
	HANDLER_PROTOTYPE(OnControllerOk);
	HANDLER_PROTOTYPE(OnChildSelect);

public:
	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent		- Parent control.
	//! \param[in]	_rect		- Control rect.
	//! \param[in]	virtualRect - Rect of scrolling area.
	//  ***************************************************
	GUIScrollView(GUIControl * _parent, const Rect & _rect, const Rect & virtualRect);
	virtual ~GUIScrollView();

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
	virtual GUIControl * Clone();

	//  ***************************************************
	//! \brief    	Get count of child items.
	//! \return		Count of child items.
	//  ***************************************************
	virtual uint32		GetChildCount();

	//  ***************************************************
	//! \brief    	Add an control to the combo list.
	//! \param[in]	pControl	- Control to add.
	//  ***************************************************
	virtual void		AddItem(GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Remove an control from the combo list.
	//! \param[in]	pControl	- Control to remove.
	//  ***************************************************
	virtual void		RemoveItem(GUIControl * const pControl);
	
	//  ***************************************************
	//! \brief    	Remove all controls from the scroll view.
	//  ***************************************************
	virtual void		RemoveItemAll();

	//  ***************************************************
	//! \brief    	Get first and last combobox items iterators.
	//! \param[out]	begin	- First item iterator.
	//! \param[out]	end		- Last item iterator.
	//  ***************************************************
	virtual void		GetChildIterators(List<GUIControl*>::Iterator & begin, List<GUIControl*>::Iterator & end);

	// ***************************************************
	//! \brief    	Set the control which should be focused in scrollview.
	//! \param      _control - control which should be focused in scrollview.
	// ***************************************************
	virtual void		SetFocusedControl(GUIControl * _control);

	// ***************************************************
	//! \brief    	GetFocusedControl - Get scrollview focused control.
	//! \return   	Combobox scrollview control.
	// ***************************************************
	virtual GUIControl *GetFocusedControl();

	// ***************************************************
	//! \brief    	Set the scrollview focused control by index.
	//! \param      _index - Index of new focused control.
	// ***************************************************
	void				SetFocusedControl(int32 _index);

	// ***************************************************
	//! \brief    	Get the scrollview focused control index.
	//! \return   	Scrollview focused control index.
	// ***************************************************
	int32				GetFocusedControlIndex();

	// ***************************************************
	//! \brief    	Try to make visible given control inside scrollview.
	//! \param		pControl - Control to make visible.
	// ***************************************************
	void				EnsureVisible(GUIControl * pControl);
};

#endif // __FRAMEWORK_GUISCROLLVIEW_H__