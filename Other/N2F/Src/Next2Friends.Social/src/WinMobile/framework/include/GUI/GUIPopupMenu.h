//// =================================================================
/*!	\file GUIPopupMenu.h

	Revision History:

	\par [9.8.2007]	13:38 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_POPUPMENU__
#define __GUI_POPUPMENU__

#include "GUIText.h"
#include "GUIPopupBase.h"
#include "GUIListView.h"

/*! \brief Pop-up menu control.

GUIPopupMenu is a button pressing on which the menu window is opened.
The user has to set handlers for all the added menu controls himself, i.e. the menu itself 
doesn't generate any events when the controls are pressed.

Example:

\code
// create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a pop-up menu
// The rect of the pop-up menu is specified as the 3rd parameter, only width is a significant one.
// Association of the drop-down menu with the button is set as the 4th parameter.
popupMenu = new GUIPopupMenu(window, Rect(50, 50, 100, 30), Rect(0, 0, 100, 0), GUIPopupMenu::EPD_DOWN_LEFT);

// Create text controls
// The only significant parameter in the rect is height, as the other parameters will be calculated by popupMenu.
popupText1 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"text1");
popupText2 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"text2");
popupText3 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"text3");
popupText4 = new GUIText(NULL, Rect(0, 0, 0, 30), (char16*)L"text4");

// Reset the ECS_STATIC property for those controls,
// which can be in focus.
popupText1->SetState(GUIControl::ECS_STATIC, false);
popupText2->SetState(GUIControl::ECS_STATIC, false);
popupText3->SetState(GUIControl::ECS_STATIC, false);
popupText4->SetState(GUIControl::ECS_STATIC, false);

// Add child controls to popupMenu
popupMenu->AddItem(popupText1);
popupMenu->AddItem(popupText2);
popupMenu->AddItem(popupText3);
popupMenu->AddItem(popupText4);

// Specify the control which will be focused at opening of the pop-up Menu
popupMenu->SetFocusedControl(popupText1);

// Add the event handler at CONTROLLER_OK release,
// at text controls.
// If the user releases phone OK button on any our text control
// TestApp - OnPopupClose class function is called
popupText1->AddHandler(ECE_ON_CONTROLLER_OK_UP, this, &TestApp::OnPopupClose);
popupText2->AddHandler(ECE_ON_CONTROLLER_OK_UP, this, &TestApp::OnPopupClose);
popupText3->AddHandler(ECE_ON_CONTROLLER_OK_UP, this, &TestApp::OnPopupClose);
popupText4->AddHandler(ECE_ON_CONTROLLER_OK_UP, this, &TestApp::OnPopupClose);

// Set focus to the popupMenu
guiSystem->SetFocus(popupMenu);

\endcode
*/
class GUIPopupMenu: public GUIPopupBase
{
public:

	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent		- Parent control.
	//! \param[in]	_rect		- Control rect.
	//! \param[in]	_popupRect	- Pop-up window rect .
	//! \param[in]	_align		- Alignment association of the window with the button.
	//  ***************************************************	
	GUIPopupMenu(GUIControl * _parent, const Rect & _rect, const Rect & _popupRect, ePopupAlign _align);
	virtual ~GUIPopupMenu();

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

	// ***************************************************
	//! \brief    	Set the control which will be in pop-up menu focus.
	//! \param      _control - Control which will be in pop-up menu focus.
	// ***************************************************
	virtual void			SetFocusedControl(GUIControl * _control);

	// ***************************************************
	//! \brief    	Get the pop-up menu focused control.
	//! \return   	Pop-up menu focused control.
	// ***************************************************
	virtual GUIControl *	GetFocusedControl();

	// ***************************************************
	//! \brief    	Set the control which will be in pop-up menu focus
	//! \param      _index - New focused control index.
	// ***************************************************
	void					SetFocusedControl(int32 _index);

	// ***************************************************
	//! \brief    	Get pop-up menu focused control index.
	//! \return   	Pop-up menu focused control index.
	// ***************************************************
	int32					GetFocusedControlIndex();

	//  ***************************************************
	//! \brief    	Add a control to the pop-up menu.
	//! \param[in]	pControl	- Control to add.
	//  ***************************************************
	virtual uint32			AddItem(GUIControl * const pControl);

	//  ***************************************************
	//! \brief    	Remove an control from the pop-up menu.
	//! \param[in]	pControl	- Control to remove.
	//  ***************************************************
	virtual void			RemoveItem(GUIControl * const pControl);

protected:

	virtual void	OnOpen();
	virtual void	OnClose();

private:
	GUIListView * listView;

	HANDLER_PROTOTYPE(OnControllerReached);
	HANDLER_PROTOTYPE(OnMenuSelect);
	HANDLER_PROTOTYPE(OnEscapeSelect);
};

#endif // __GUI_POPUPMENU__
