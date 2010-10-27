//// =================================================================
/*!	\file GUIComboBox.h

	Revision History:

	\par [9.8.2007]	13:36 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_COMBOBOX__
#define __GUI_COMBOBOX__

#include "GUILayout.h"
#include "GUIPopupBase.h"
#include "GUIListView.h"

/*! \brief Combo box control.

GUIComboBox is a combined button and popup list. A GUIComboBox provides a means of presenting
a list of options to the user in a way that takes up the minimum amount of screen space.
A combobox is a selection control that displays the current item, and can pop up a list of
selectable items.

Example:
\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a comboBox in the window
// Set the area of the drop-down list as a second parameter, significant is only height.
// If the height sum of all the added elements is less than area height of the drop-down list,
// then area height becomes equal to the height sum of all the elements.
comboBox = new GUIComboBox(window, Rect(50, 50, 100, 30), Rect(0, 0, 0, 100));

// Crate text controls
// The significant parameter in the area is only height, as all the other parameters
// will be counted by comboBox.
comboText1 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"text 1");
comboText2 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"------");
comboText3 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"text 3");
comboText4 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"text 4");

// Reset ECS_STATIC property for all the controls,
// which can be focused.
comboText1->SetState(GUIControl::ECS_STATIC, false);
comboText3->SetState(GUIControl::ECS_STATIC, false);
comboText4->SetState(GUIControl::ECS_STATIC, false);

// Add child objects for comboBox
comboBox->AddItem(comboText1);
comboBox->AddItem(comboText2);
comboBox->AddItem(comboText3);
comboBox->AddItem(comboText4);

// Set the control which is selected in the comboBox
comboBox->SetFocusedControl(comboText1);

// Set focus to the comboBox
guiSystem->SetFocus(comboBox);

\endcode
*/
class GUIComboBox: public GUIPopupBase
{
public:

	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent		- parent control
	//! \param[in]	_rect		- control rect
	//! \param[in]	_comboRect	- drop-down list area (only height is significant)
	//  ***************************************************	
	GUIComboBox( GUIControl * _parent, const Rect & _rect, const Rect & _comboRect);
	virtual ~GUIComboBox();

	// ***************************************************
	//! \brief    	Set the control which will be focused in combobox.
	//! \param      _control - control
	// ***************************************************
	virtual void			SetFocusedControl(GUIControl * _control);

	// ***************************************************
	//! \brief    	GetFocusedControl - Get combobox focused control.
	//! \return   	Combobox focused control.
	// ***************************************************
	virtual GUIControl *	GetFocusedControl();

	// ***************************************************
	//! \brief    	SetFocusedControl - set the combobox focused control by index.
	//! \param      _index - Index of new focused control.
	// ***************************************************
	void					SetFocusedControl(int32 _index);

	// ***************************************************
	//! \brief    	GetFocusedControlIndex - Get the combobox focused control index.
	//! \return   	Combobox focused control index.
	// ***************************************************
	int32					GetFocusedControlIndex();

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
	//! \brief    	Add an control to the combo list.
	//! \param[in]	pControl	- Control to add.
	//  ***************************************************
	virtual uint32			AddItem(GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Remove an control from the combo list.
	//! \param[in]	pControl	- Control to remove.
	//  ***************************************************
	virtual void			RemoveItem(GUIControl * const pControl);
	
	//  ***************************************************
	//! \brief    	Get first and last combobox items iterators.
	//! \param[out]	begin	- First item iterator.
	//! \param[out]	end		- Last item iterator.
	//  ***************************************************
	virtual void			GetChildIterators(List<GUIControl*>::Iterator & begin, List<GUIControl*>::Iterator & end);

protected:
	virtual void	OnOpen();	//!< This function is called on combo box opened.
	virtual void	OnClose();	//!< This function is called on combo box closed.

private:
	HANDLER_PROTOTYPE(OnSkinChange);
	HANDLER_PROTOTYPE(OnComboPress);
	HANDLER_PROTOTYPE(OnEscapeSelect);
	HANDLER_PROTOTYPE(OnStateChange);

	GUIListView		* listView;
	GUIControl		* buttonControl;
	GUILayoutBox	* boxLayout;
	GUIControl		* cloneControl;
};

#endif // __GUI_COMBOBOX__