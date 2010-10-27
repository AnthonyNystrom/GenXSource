#ifndef __GUI_TEXT__
#define __GUI_TEXT__

#include "BaseTypes.h"
#include "guicontrol.h"
//#include "String.h"

class Font;
class String;

class GUIText : public GUIControl
{
	friend class TextBalancer;
public:
	GUIText(const char16 *text = NULL, GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect(),
		bool _isChange = false);

	GUIText(const int16 resourseID, GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect());
	
	virtual ~GUIText();

	virtual void	Update();
	virtual void	Draw();

	virtual void	DrawText();

	virtual void	SetDrawType(eDrawType newType);
	virtual void	SetRect(const Rect &rc);


	virtual void	SetTextAlign(uint32 _align);
	virtual uint32	GetTextAlign();

	virtual bool	SetText(const char16* _text);
	virtual bool	SetText(const int16 _resID);
	virtual const char16*	GetText();

	void			SetIsCange(bool _isChange);
	bool			GetIsCange();

	int32			GetTextWidth();

	void SetTextMargins(int32 top, int32 left, int32 bottom, int32 right);

	bool	Empty();

protected:
	virtual void UpdateTextPos();

	Font	*pFont;
	uint8	fontPlt;

	char16	*textBuffer;
	int32	bufferSize;
	int32	textSize;

	uint32	align;
	String	*pStr;
	bool	isChange;	
	bool	isString;

	int32	textWidth;
	TextBalancer *textBalancer;

	int32 marginTop;
	int32 marginBottom;
	int32 marginLeft;
	int32 marginRight;

	int32	dx;
	int32	dy;
};

#endif//__GUI_TEXT__