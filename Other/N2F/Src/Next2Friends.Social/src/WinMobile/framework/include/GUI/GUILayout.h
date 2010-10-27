//// =================================================================
/*!	\file GUILayout.h

	Revision History:

	\par [21.5.2007] 23:19 by Vitaliy Borodovsky
	The code is changed in accordance to the new architecture.

	\par [16.5.2007] 23:35 by Lobanov Anton
	Update function is added.

	\par [16.5.2007] 23:34 by Lobanov Anton
	Comments are added.

	\par [15.5.2007] 15:28 by Vitaliy Borodovsky
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUILAYOUT_H__
#define __FRAMEWORK_GUILAYOUT_H__

#include "List.h"
#include "Vector.h"
#include "GUIControl.h"
#include "FixedMath.h"


/*! \brief Basic class for all the Layouts.

In order not to set coordinates for all the controls there is GUILayout.
Layout places the child controls of the control to which it is installed
in accordance to the rules described in the Layout itself.

GUILayout is a basic class from which we should inherit in order to implement our special layout scheme.

There are 2 Layouts of that kind implemented in GUI:

\li GUILayoutBox - arranges the child controls in a vertical or horizontal line.
\li GUILayoutGrid - arranges the child controls in a rectangle table.

Example:

\code

// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a GUIBoxLayout
boxLayout =  new GUILayoutBox(EGO_LEFTTORIGHT);
// Create a GUIGridLayout
gridLayout = new GUILayoutGrid();

// Create a control to which we will place GUIBoxLayout
ltControl1 = new GUIControl(window, Rect(0, 0, graphics->GetWidth(), graphics->GetHeight() >> 1));
// Make it static in order to disable focusing
ltControl1->SetState(GUIControl::ECS_STATIC, true);
// Create a control to which we will place GUIGridLayout
ltControl2 = new GUIControl(window, Rect(0, graphics->GetHeight() >> 1, graphics->GetWidth(), graphics->GetHeight() >> 1));
// Make it static
ltControl2->SetState(GUIControl::ECS_STATIC, true);

// Create buttons which will be placed to our controls
ltButton1 = new GUIButtonText(ltControl1, Rect(), (char16*)L"button1");
ltButton2 = new GUIButtonText(ltControl1, Rect(), (char16*)L"button2");
ltButton3 = new GUIButtonText(ltControl1, Rect(), (char16*)L"button3");
ltButton4 = new GUIButtonText(ltControl2, Rect(), (char16*)L"button4");
ltButton5 = new GUIButtonText(ltControl2, Rect(), (char16*)L"button5");
ltButton6 = new GUIButtonText(ltControl2, Rect(), (char16*)L"button6");
ltButton7 = new GUIButtonText(ltControl2, Rect(), (char16*)L"button7");
ltButton8 = new GUIButtonText(ltControl2, Rect(), (char16*)L"button8");

// Add elements of the first control to the GUIBoxLayout
boxLayout->Add(ltButton1, 0, 1);
boxLayout->Add(ltButton2, 1, 1);
boxLayout->Add(ltButton3, 2, 1);

// Install GUIBoxLayout to the first control
ltControl1->SetLayout(boxLayout);

// Add elements of the second control to the GUIGridLayout
gridLayout->Add(ltButton4, 0, 0, 1, 1);
gridLayout->Add(ltButton5, 1, 0, 1, 1);
gridLayout->Add(ltButton6, 0, 1, 2, 1);
gridLayout->Add(ltButton7, 0, 2, 1, 1);
gridLayout->Add(ltButton8, 1, 2, 1, 1);

// Install GUIGridLayout to the second control
ltControl2->SetLayout(gridLayout);

// Set focus to the button
guiSystem->SetFocus(ltButton1);

\endcode
*/

class GUILayout
{
public:

	//! Control alignment in the cell area
	enum eItemAlign
	{
		EIA_LEFT = 0x0,		//!< align left 
		EIA_HCENTER = 0x1,	//!< align horizontal center
		EIA_RIGHT = 0x2,	//!< align right
		EIA_TOP = 0x4,		//!< align top
		EIA_VCENTER = 0x8,	//!< align vertical center
		EIA_BOTTOM = 0x10	//!< align bottom
	};

	GUILayout();
	virtual ~GUILayout();

	// ***************************************************
	//! \brief    	SetLayoutMargins - set margins for the layout.
	//!				Cell area will be counted from the layout rect deducting margins.
	//! \param      _leftMargin - left margin
	//! \param      _rightMargin - right margin
	//! \param      _topMargin - top margin
	//! \param      _bottomMargin - bottom margin
	// ***************************************************
	void SetLayoutMargins(int32 _leftMargin, int32 _rightMargin, int32 _topMargin, int32 _bottomMargin);

	// ***************************************************
	//! \brief    	SetMargins - set margins for the elements in the cells area.
	//!				The element sizes will be counted basing on the cells sizes deducting margins.
	//! \param      _control - the element for which we should set margins
	//! \param      _leftMargin - left margin
	//! \param      _rightMargin - right margin
	//! \param      _topMargin - top margin
	//! \param      _bottomMargin - bottom margin
	// ***************************************************
	void SetMargins(GUIControl * _control, int32 _leftMargin, int32 _rightMargin, int32 _topMargin, int32 _bottomMargin);

	// ***************************************************
	//! \brief    	SetAlign - set cell alignment
	//! \param      _control - the control for which we should state the cell alignment
	//! \param      _align - alignment
	// ***************************************************
	void SetAlign(GUIControl * _control, uint32 _align);

	// ***************************************************
	//! \brief    	SetPercent - set percent of the control size from the occupied cell part deducting margins
	//! \param      _control - control for which we should state the percent
	//! \param      _percent - percent
	// ***************************************************
	void SetPercent(GUIControl * _control, int32 _percent);

	// ***************************************************
	//! \brief    	Clear - remove all the controls and cells from the Layout
	// ***************************************************
	virtual void Clear();

	// ***************************************************
	//! \brief    	RecalcLayout - recalculate the area coordinates and child control coordinates и координаты детей
	//!				according to the rules described in Layout
	// ***************************************************
	virtual void RecalcLayout(GUIControl * _control) = 0;

protected:

	//! Structure is used to represent cell properties.
	struct CellProp
	{
		CellProp():pos(0),size(-1),scrSize(0) {};
		int32 pos;			
		int32 size;			
		int32 scrSize;		
	};

	//! Structure is used to represent cell item properties.
	struct ItemProp
	{
		ItemProp(GUIControl * _control = NULL): percent(100),
			align(EIA_VCENTER | EIA_HCENTER),
			leftMargin(0),
			rightMargin(0),
			topMargin(0),
			bottomMargin(0),
			control(_control)
		{};
		int32 percent;
		uint32 align;
		int32 leftMargin;
		int32 rightMargin;
		int32 topMargin;
		int32 bottomMargin;
		GUIControl * control;
	};

	void CalcCellProp(Vector<CellProp> & cellProp, int32 _margins, int32 _size);

	void CalcItemProp(Rect & _rect, ItemProp & _itemProp);

	List<ItemProp> itemPropList;

	int32 leftMargin;
	int32 rightMargin;
	int32 topMargin;
	int32 bottomMargin;
};

#endif // __FRAMEWORK_GUILAYOUT_H__
