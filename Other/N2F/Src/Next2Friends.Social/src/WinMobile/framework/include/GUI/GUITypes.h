//// =================================================================	
/*! \file GUITypes.h
	
	Revision History:
	
	\par [15.5.2007]	14:07 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUITYPES_H__
#define __FRAMEWORK_GUITYPES_H__

#include "BaseTypes.h"

//!	Class types.
//! It is used in the internal mechanism on analogy with "dynamic cast".
//!	Each class has its unique type. Use "IsClass" function to check the class type.
enum eClassType
{
	ECT_WINDOW,
	ECT_CONTROL,
	ECT_TEXT,
	ECT_TEXTLINE,
	ECT_TEXTDOCUMENT,
	ECT_EDITLINE,
	ECT_EDITTEXT,
	ECT_BUTTON,
	ECT_BUTTONTEXT,
	ECT_BUTTON_GROUP,
	ECT_CHECKBOX,
	ECT_RADIO_BUTTON,
	ECT_SCROLLVIEW,
	ECT_LISTVIEW,
	ECT_LISTVIEW_CYCLED,
	ECT_SCROLLBAR,
	ECT_NUMERIC,
	ECT_PROGRESSBAR,
	ECT_SLIDER,
	ECT_PROPBUTTON,
	ECT_IMAGE,
	ECT_IMAGE_ANIMATED,
	ECT_POPUPBASE,
	ECT_COMBOBOX,
	ECT_POPUPMENU,
	ECT_LAYOUTBOX,
	ECT_LAYOUTGRID,
	ECT_LAYOUT_STACKED,
	ECT_TAB_CONTROL,
	ECT_SPIN,
	ECT_ABSTRACT_SCROLL_AREA,

	ECT_USER = 0x8000 //!< All the user class types must be defined after ECT_USER
};

//! Control drawing type.
//! Each control should have a drawing type in order for the skin to know how it should be drawn.
//! Drawing types shouldn't necessary be unique.
enum eDrawType
{
	EDT_NONE = 0,
	EDT_DEFAULT = 0,
	EDT_CONTROL,
	EDT_MULTITEXT_PANEL,
	EDT_TEXT,
	EDT_TEXTLINE,
	EDT_TEXTDOCUMENT,
	EDT_EDITLINE,
	EDT_EDITTEXT,
	EDT_WINDOW,
	EDT_POPUP_WINDOW,
	EDT_BUTTON,
	EDT_BUTTON_UP,
	EDT_BUTTON_DOWN,
	EDT_BUTTON_LEFT,
	EDT_BUTTON_RIGHT,
	EDT_BUTTON_TEXT,
	EDT_CHECKBOX,
	EDT_RADIO_IMAGE,
	EDT_BUTTON_GROUP_PANEL,
	EDT_SLIDER_PANEL,
	EDT_SLIDER_BAR,
	EDT_PROP_PANEL_LEFT_TO_RIGHT,
	EDT_PROP_PANEL_UP_TO_DOWN,
	EDT_PROP_BUTTON,
	EDT_PROGRESS_PANEL,
	EDT_PROGRESS_BAR,
	EDT_COMBO_IMAGE,
	EDT_COMBO_PANEL,
	EDT_IMAGE,
	EDT_SPIN,
	EDT_SCROLL_AREA,

	EDT_USER = 0x8000 //!< All the user drawing types must be defined after EDT_USER

};

//! Control orientation.
//! It is used in such controls as
//! ScrollBar, Slider, ProgressBar, ListView, etc.
enum eGUIOrientation
{
	EGO_LEFTTORIGHT,	//!< from left to right
	EGO_UPTODOWN		//!< from top to bottom
};

//! Text alignment.
enum eALignment
{
	EAL_LEFT	= 0x1,
	EAL_RIGHT	= 0x2,
	EAL_HCENTER	= 0x4,
	EAL_TOP		= 0x8,
	EAL_BOTTOM	= 0x10,
	EAL_VCENTER	= 0x20
};
#endif // __FRAMEWORK_GUITYPES_H__
