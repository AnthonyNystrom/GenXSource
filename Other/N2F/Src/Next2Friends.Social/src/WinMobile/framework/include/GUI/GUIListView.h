//// =================================================================
/*!	\file GUIListView.h

	Revision History:
	\par [23.10.2007]	12:38 by Alexey Prosin
	Fix remove all problem.

	\par [9.8.2007]	13:38 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUILISTVIEW_H__
#define __FRAMEWORK_GUILISTVIEW_H__

#include "GUIScrollView.h"
#include "GUILayoutBox.h"

/*! \brief List view control.

GUIListView is a container which places its child objects in a line or in a column.
If all the child objects do not fit GUIListView rect, the automatic scrolling occurs at moving the focus.
Selecting one of the child objects eControlEvent::ECE_ON_LISTITEM_SELECT event is sent.

Example:

\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a listView in the window
// listView area is set as the second parameter
// Orientation is set as the third parameter
listView1 = new GUIListView(window, Rect(0, 0, 100, 80), EGO_UPTODOWN);

// Create controls to be added to listView
// In the area only height is a significant parameter,
// the others will be counted by listView
list1Button1 = new GUIButtonText(NULL, Rect(0, 0, 0, 30), (char16*)L"button 1");
list1Button2 = new GUIButtonText(NULL, Rect(0, 0, 0, 30), (char16*)L"button 2");
list1Button3 = new GUIButtonText(NULL, Rect(0, 0, 0, 30), (char16*)L"button 3");
list1Button4 = new GUIButtonText(NULL, Rect(0, 0, 0, 30), (char16*)L"button 4");

// Add child objects to listView
listView1->AddItem(list1Button1);
listView1->AddItem(list1Button2);
listView1->AddItem(list1Button3);
listView1->AddItem(list1Button4);

// Create another listView
listView2 = new GUIListView(window, Rect(0, 100, 170, 50), EGO_LEFTTORIGHT);

// Create controls for the second listView
list2Button1 = new GUIButtonText(NULL, Rect(0, 0, 80, 0), (char16*)L"button 1");
list2Button2 = new GUIButtonText(NULL, Rect(0, 0, 80, 0), (char16*)L"button 2");
list2Button3 = new GUIButtonText(NULL, Rect(0, 0, 80, 0), (char16*)L"button 3");
list2Button4 = new GUIButtonText(NULL, Rect(0, 0, 80, 0), (char16*)L"button 4");

// Create a comboBox
list2Combo = new GUIComboBox(window, Rect(0, 0, 80, 0), Rect(0, 0, 0, 50));
list2ComboText1 = new GUIText(NULL, Rect(0, 0, 0, 20), (char16*)L"text1");
list2ComboText2 = new GUIText(NULL, Rect(0, 0, 0, 20), (char16*)L"text2");
list2ComboText3 = new GUIText(NULL, Rect(0, 0, 0, 20), (char16*)L"text3");
list2ComboText1->SetState(GUIControl::ECS_STATIC, false);
list2ComboText2->SetState(GUIControl::ECS_STATIC, false);
list2ComboText3->SetState(GUIControl::ECS_STATIC, false);
list2Combo->AddItem(list2ComboText1);
list2Combo->AddItem(list2ComboText2);
list2Combo->AddItem(list2ComboText3);
list2Combo->SetFocusedControl(list2ComboText1);

// Add child objects to the second listView
listView2->AddItem(list2Button1);
listView2->AddItem(list2Button2);
listView2->AddItem(list2Button3);
listView2->AddItem(list2Button4);
listView2->AddItem(list2Combo);

// Set focus to the listView
guiSystem->SetFocus(listView1);
\endcode
*/
class GUIListView : public GUIScrollView
{

protected:
	uint32			curIndex;
	eGUIOrientation listOrientation;

private:
	GUILayoutBox *	pListViewLayout;

	HANDLER_PROTOTYPE(OnSizeChange);

	void			AfterAdd(uint32 index, GUIControl * const pControl);

public:
	//  ***************************************************
	//! \brief    	
	//! \param[in]	_parent		- Parent control.
	//! \param[in]	_rect		- Control rect.	
	//! \param[in]	_orientation- List orientation.
	//  ***************************************************
	GUIListView(GUIControl * _parent, const Rect & _rect, eGUIOrientation _orientation);
	virtual ~GUIListView();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl * Clone();

	//  ***************************************************
	//! \brief    	Add a child control.
	//! \param[in]	pControl	- Child control being added.
	//  ***************************************************
	virtual void	AddItem(GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Add a child control to the specified position
	//! \param[in]	index		- position.
	//! \param[in]	pControl	- Child control being added.
	//  ***************************************************
	virtual void	InsertItem(uint32 index, GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Remove the child control.
	//! \param[in]	pControl	- Child control being removed.
	//  ***************************************************
	virtual void	RemoveItem(GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Remove all controls from the list view.
	//  ***************************************************
	virtual void RemoveItemAll();

	//  ***************************************************
	//! \brief    	Get the index of the given control in GUIListView.
	//! \param[in]	pControl	- the given control.
	//! \return		The Current index of the searched control or -1 if the control is not found.
	//  ***************************************************
	int32			GetItemPos(GUIControl * const pControl);
};

#endif // __FRAMEWORK_GUILISTVIEW_H__
