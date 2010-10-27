#ifndef __SKIN_PROPERTIES__
#define __SKIN_PROPERTIES__

#include "GUISkinLocal.h"

static const int32 skinTilesCount = 17;
static const int32 skinProp[skinTilesCount][13] = 
{
	{
		EDT_DEFAULT, GUISkinLocal::ES_NONE, 0, 0
			, 0, 0, 0
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_NONE, GUISkinLocal::ES_NONE, 0, 0
			, 0, 0, 0
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_SCROLL_BAR, GUISkinLocal::ES_V3, 0, 0
			, IDB_SCROLL_BAR_TOP, IDB_SCROLL_BAR, IDB_SCROLL_BAR_BOTTOM
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_HEADER, GUISkinLocal::ES_H3, 0, 0
			, IDB_HEADER_LEFT, IDB_HEADER, IDB_HEADER_RIGHT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_FOOTER_BUTTON_LEFT, GUISkinLocal::ES_H3, 0, 0
			, IDB_FOOTER_LEFT, IDB_FOOTER_CENTER, IDB_FOOTER_CENTER_LEFT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_FOOTER_BUTTON_RIGHT, GUISkinLocal::ES_H3, 0, 0
			, IDB_FOOTER_CENTER_RIGHT, IDB_FOOTER_CENTER, IDB_FOOTER_RIGHT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_LOADING_BAR, GUISkinLocal::ES_H3, 0, 0
			, IDB_LOADING_BAR, IDB_LOADING_BAR, IDB_LOADING_BAR
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_SELECTED_ITEM, GUISkinLocal::ES_H3, 0, 0
			, IDB_SELECTED_ITEM_LEFT, IDB_SELECTED_ITEM_CENTER, IDB_SELECTED_ITEM_RIGHT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_PHOTO_ITEM, GUISkinLocal::ES_H3, 0, 0
			, IDB_PHOTO_ITEM_LEFT, IDB_PHOTO_ITEM_CENTER, IDB_PHOTO_ITEM_RIGHT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_POPUP_ITEM, GUISkinLocal::ES_H3, 0, 0
			, IDB_POPUP_ITEM, IDB_POPUP_ITEM, IDB_POPUP_ITEM
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_POPUP_BORDER, GUISkinLocal::ES_9, 0, 0
			, IDB_POPUP_BORDER_TOP_LEFT, IDB_POPUP_BORDER_TOP_CENTER, IDB_POPUP_BORDER_TOP_RIGHT
			, IDB_POPUP_BORDER_MIDDLE_LEFT, IDB_POPUP_BORDER_MIDDLE_CENTER, IDB_POPUP_BORDER_MIDDLE_RIGHT
			, IDB_POPUP_BORDER_BOTTOM_LEFT_ARROW, IDB_POPUP_BORDER_BOTTOM_CENTER, IDB_POPUP_BORDER_BOTTOM_RIGHT
	}
	,
	{
		EDT_ALERT_BORDER, GUISkinLocal::ES_9, 0, 0
			, IDB_POPUP_BORDER_TOP_LEFT, IDB_POPUP_BORDER_TOP_CENTER, IDB_POPUP_BORDER_TOP_RIGHT
			, IDB_POPUP_BORDER_MIDDLE_LEFT, IDB_POPUP_BORDER_MIDDLE_CENTER, IDB_POPUP_BORDER_MIDDLE_RIGHT
			, IDB_POPUP_BORDER_BOTTOM_LEFT, IDB_POPUP_BORDER_BOTTOM_CENTER, IDB_POPUP_BORDER_BOTTOM_RIGHT
	}
	,
	{
		EDT_EDITBOX, GUISkinLocal::ES_H3, 0, 0
			, IDB_EDITBOX_LEFT, IDB_EDITBOX_CENTER, IDB_EDITBOX_RIGHT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_EDITBOX_ACTIVE, GUISkinLocal::ES_H3, 0, 0
			, IDB_EDITBOX_ACTIVE_LEFT, IDB_EDITBOX_ACTIVE_CENTER, IDB_EDITBOX_ACTIVE_RIGHT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_TAB_BACK, GUISkinLocal::ES_H3, 0, 0
			, IDB_TAB_BACK, IDB_TAB_BACK, IDB_TAB_BACK
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_TAB_SELECTOR, GUISkinLocal::ES_H3, 0, 0
			, IDB_TAB_LEFT, IDB_TAB, IDB_TAB_RIGHT
			, 0, 0, 0
			, 0, 0, 0
	}
	,
	{
		EDT_MULTIEDIT, GUISkinLocal::ES_9, 0, 0
			, IDB_MULTIEDIT_TOP_LEFT, IDB_MULTIEDIT_TOP_CENTER, IDB_MULTIEDIT_TOP_RIGHT
			, IDB_MULTIEDIT_MIDDLE_LEFT, IDB_MULTIEDIT_MIDDLE_CENTER, IDB_MULTIEDIT_MIDDLE_RIGHT
			, IDB_MULTIEDIT_BOTTOM_LEFT, IDB_MULTIEDIT_BOTTOM_CENTER, IDB_MULTIEDIT_BOTTOM_RIGHT
	}
};

static const uint8 gradientProps[][7] = 
{
//	startR	startG	startB	endR	endG	endB	alpha
	{255,	255,	255,	255,	255,	255,	12}//0
	//,{0,	0,		0,		0,		0,		0,		9}//1
	//,{248,	255,	179,	0,		0,		0,		0}//2
	//,{159,	232,	255,	0,		0,		0,		0}//3
	//,{213,	218,	230,	0,		0,		0,		0}//4
	//,{194,	204,	216,	0,		0,		0,		0}//5
	//,{0,	0,		0,		0,		0,		0,		7}//6
	//,{240,	240,	240,	0,		0,		0,		4}//7
	//,{247,	255,	179,	0,		0,		0,		0}//8
	//,{170,	254,	187,	0,		0,		0,		0}//9
	//,{159,	232,	255,	0,		0,		0,		0}//10
	//,{0,	208,	94,		0,		124,	56,		13}//11
	//,{141,	141,	141,	109,	109,	109,	13}//12
	//,{50,	140,	233,	0,		0,		0,		4}//13
};

static const uint8 lineProps[][5] = 
{
//	R		G		B		offset	alpha
	{177,	184,	209,	3,		0}
	//,{223,	198,	109,	3,		0}
};

#endif
