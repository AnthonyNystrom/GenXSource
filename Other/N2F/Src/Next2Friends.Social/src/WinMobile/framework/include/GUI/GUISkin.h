//// =================================================================
/*!	\file GUISkin.h

	Revision History:

	\par [9.8.2007]	13:43 by Alexey Prosin
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUISKIN_H__
#define __FRAMEWORK_GUISKIN_H__

#include "Vector.h"
#include "GUIControl.h"

class Font;
//  ***************************************************
//! @brief Base skin interface. 
//! 
//! If you need to implement control rendering with your own skin, you should
//! derive your skin from GUISkin. There is two classes (\ref GUISkinSoftware and \ref GUISkinTiled)
//! which already implements GUISkin interface.
//  ***************************************************
class GUISkin
{
public:

	GUISkin();

	virtual ~GUISkin();

	struct SkinStyle
	{
		SkinStyle():
		cursorWidth(2)
		,spinMarginLeft(3)
		,spinMarginRight(3)
		,spinMarginTop(3)
		,spinMarginBottom(3)
		,editLineMarginLeft(3)
		,editLineMarginRight(3)
		,editLineMarginTop(3)
		,editLineMarginBottom(3)
		,buttonTextMarginLeft(3)
		,buttonTextMarginRight(3)
		,buttonTextMarginTop(3)
		,buttonTextMarginBottom(3)
		,scrBarMinSize(5)
		,scrBarMaxSize(10000)
		,arrowSize(16)
		,sliderMinSize(20)
		,radioImageSize(25)
		,comboImageSize(25)
		,scrollAreaMarginLeft(3)
		,scrollAreaMarginRight(3)
		,scrollAreaMarginTop(3)
		,scrollAreaMarginBottom(3)
		,textCursorTime(15)
		,tabWidth(100)
		,tabHeight(20)
		{};

		uint32 cursorWidth;
		uint32 spinMarginLeft;
		uint32 spinMarginRight;
		uint32 spinMarginTop;
		uint32 spinMarginBottom;
		uint32 editLineMarginLeft;
		uint32 editLineMarginRight;
		uint32 editLineMarginTop;
		uint32 editLineMarginBottom;
		uint32 buttonTextMarginLeft;
		uint32 buttonTextMarginRight;
		uint32 buttonTextMarginTop;
		uint32 buttonTextMarginBottom;
		uint32 scrBarMinSize;
		uint32 scrBarMaxSize;
		uint32 arrowSize;
		uint32 sliderMinSize;
		uint32 radioImageSize;
		uint32 comboImageSize;
		uint32 scrollAreaMarginLeft;
		uint32 scrollAreaMarginRight;
		uint32 scrollAreaMarginTop;
		uint32 scrollAreaMarginBottom;
		uint32 textCursorTime;
		uint32 tabWidth;
		uint32 tabHeight;
	};

	void SetStyle(const SkinStyle & _style) { skinStyle = _style; }

	const SkinStyle * GetStyle() { return &skinStyle; }

	// ***************************************************
	//! \brief    	Draw control on screen
	//! \param      _control - control to draw
	// ***************************************************
	virtual void	DrawControl(const GUIControl * _control);

	virtual void	DrawControl(const GUIControl * _control, const Rect & _clipRect) = 0;

	// ***************************************************
	//! \brief    	Draw focus
	//! \param      _control - focused control
	// ***************************************************
	virtual void	DrawFocus(const GUIControl * _control);

	virtual void	DrawFocus(const GUIControl * _control, const Rect & _clipRect) = 0;

	virtual void	DrawTextCursor(const GUIControl * pControl, int32 x, int32 y);

	virtual void	DrawTextCursor(const GUIControl * pControl, const Rect & _clipRect, int32 x, int32 y) = 0;

	virtual void	DrawString(GUIControl *pControl, const char16* pStr, int32 x, int32 y, int32 lenght = -1);

	virtual void	DrawString(GUIControl *pControl, const Rect & _clipRect, const char16* pStr, int32 x, int32 y, int32 lenght = -1) = 0;
	
	virtual int32	GetFontHeight(const GUIControl *pControl) = 0;

	virtual int32	GetStringWidth(const GUIControl *pControl, const char16 *pStr, int32 length = -1) = 0;

protected:
	int32		cursorCounter;
	bool		cursorDraw;

	SkinStyle	skinStyle;

	HANDLER_PROTOTYPE(OnUpdate);
};

#endif // __FRAMEWORK_GUISKIN_H__
