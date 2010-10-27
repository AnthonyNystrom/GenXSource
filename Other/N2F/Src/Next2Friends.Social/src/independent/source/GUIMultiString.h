#ifndef __GUI_MULTI_STRING_H_
#define __GUI_MULTI_STRING_H_

#include "GUIControlContainer.h"
#include "GUIScrollBar.h"
#include "VList.h"

class String;
struct ParsedString 
{
	char16*	pStr;		//!< Pointer on first string character.
	int32	count;		//!< Count of characters in string.
	int32	spaceCount;	//!< Count of spaces in the beginning of the string.
	int32	x;			//!< x coordinate, where string to draw (corresponding to the control).
	int32	y;			//!< y coordinate, where string to draw (corresponding to the control).

	ParsedString()
	{
		pStr		=	NULL;
		count		=	0;
		spaceCount	=	0;
		x			=	0;
		y			=	0;
	}	
};

class Font;
class Application;

class GUIMultiString : public GUIControlContainer
{
	enum eDefaults
	{
		SCROLL_WIDTH			=	10,
		CURSOR_WIDTH			=	1,

		MARGIN_TOP				=	1,
		MARGIN_LEFT				=	1,
		MARGIN_BOTTOM			=	1,
		MARGIN_RIGHT			=	1
	};

protected:
	Application				*	pApp;
	Font					*	pFont;
	uint8						fontPlt;
	
	//! scroll bar
	GUIScrollBar			*	scrollBar;
	bool						showScroll;

	//! text
	bool						isString;
	String					*	pStr;
	char16					*	textBuffer;
	int32						textSize;
	int32						bufferSize;
	
	//!	text scroll
	int32						scrollWidth;
	int32						scrollDelta;	
	int32						yOffset;

	//! Margins
	int32						marginTop;
	int32						marginLeft;
	int32						marginBottom;
	int32						marginRight;

	//! parsed strings
	VList						parsedStrings;
	int32						strCount;
    
	//! text align
	uint32						align;
	uint32						drawedAlign;
	
public:
	GUIMultiString(char16*	inpText = NULL, GUIControlContainer * parent = NULL, const ControlRect &cr = ControlRect());
	GUIMultiString(uint16	resourseID,		GUIControlContainer * parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUIMultiString();

	virtual void SetText(const char16 * pText);
	virtual void SetText(uint16 resourseID);

	virtual const char16 * GetText();

	virtual	void SetTextAlign(uint32 align);
	virtual uint32 GetTextAlign()			const	{ return align; }

	virtual void Update();
	virtual void Draw();

	virtual void DrawText(const Rect &rc, bool skipSpaces);
	virtual void SetDrawType(eDrawType newType);

	virtual void SetRect(const Rect &rc);
	virtual void SetControlRect(const ControlRect &cr);

	void SetScrollWidth(int32 sWidth);			
	int32 GetScrollWidth()				const	{ return scrollWidth; }

	virtual void SetTextMargins(int32 top, int32 left, int32 bottom, int32 right);

	void SetScrollDelta(int32 delta);

	int32 GetFullHeight();
	
	uint32	GetStringCount()
	{
		return strCount;
	}

	void SetUnderlined(bool	isUnderlined);
	bool GetUnderlined()		const	{	return drawUnderline;	} 
	
	void SetUnderlinedDrawType(eDrawType newType);
	eDrawType GetUnderlinedDrawType() const {	return underlineDrawType;	}

protected:
	void ScrollUp();
	void ScrollDown(int32 topY);

	void SetDefaults();
	void SetYOffset(int32 offs);
	void UpdateScroll();
	void SetScrollRect();
	virtual void ParseStrings(int32 scrollW);
	uint32 GetLenFromCharToWordEnd(char16 *ch);

	void UpdateUnderlineY();

	void UpdateLines();
	//
	bool		drawUnderline;
	eDrawType	underlineDrawType;
	int32		underlineDy;

	int32		startUnderLineY;
	int32		lineCount;

	bool reparse;
	bool isScrollNeeded;
};

#endif // __GUI_MULTI_STRING_H_