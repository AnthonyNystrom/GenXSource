//// =================================================================
/*!	\file GUISkinSoftware.h

	Revision History:

	\par [9.8.2007]	13:44 by Alexey Prosin
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUISKINSOFTWARE_H__
#define __FRAMEWORK_GUISKINSOFTWARE_H__

#include "GUISkin.h"

class GraphicsSystem;

//  ***************************************************
//! \brief Skin for drawing all base controls
//  ***************************************************
class GUISkinSoftware : public GUISkin
{
public:

	GUISkinSoftware();
	virtual ~GUISkinSoftware();

	// ***************************************************
	//! \brief    	Draw control on screen
	//! \param      _control - control to draw
	//! \param		_clipRect - rect for clip.
	// ***************************************************
	virtual void DrawControl(const GUIControl * _control, const Rect & _clipRect);

	// ***************************************************
	//! \brief    	Draw focus
	//! \param      _control - focused control.
	//! \param		_clipRect - current clip rect.
	// ***************************************************
	virtual void DrawFocus(const GUIControl * _control, const Rect & _clipRect);

	virtual void DrawTextCursor(const GUIControl * pControl, const Rect & _clipRect, int32 x, int32 y);

	virtual void DrawString(GUIControl *pControl, const Rect & _clipRect, const char16* pStr, int32 x, int32 y, int32 lenght = -1);

	virtual int32 GetFontHeight(const GUIControl *pControl);

	virtual int32 GetStringWidth(const GUIControl *pControl, const char16 *pStr, int32 length = -1);

	void SetDefaultFont(Font *_pFont);

private:

	Font* GetFont(const GUIControl *pControl);

	void DrawButton(const Rect & _rect, const GUIControl * _control);
	GraphicsSystem * graphicsSystem;

	Font *pDefaultFont;
	Font *pFocusedFont;
};

#endif // __FRAMEWORK_GUISKINSOFTWARE_H__