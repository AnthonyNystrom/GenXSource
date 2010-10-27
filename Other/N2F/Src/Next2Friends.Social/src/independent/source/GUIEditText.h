#ifndef __GUI_EDIT_TEXT_
#define __GUI_EDIT_TEXT_

#include "GUIText.h"
#include "GUIInputText.h"
#include "GUIEventListener.h"


class Application;
class GUIEditText : public GUIInputText, public GUIText, public GUIEventListener
{
public:
	GUIEditText(int32 bufSize = MAX_SIZE, eEditTextType eeType = EETT_ALL, GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect());
	
	virtual ~GUIEditText();

	virtual bool OnEvent(eEventID eventID, EventData *pData);


	virtual void Update();

	virtual void Draw();
	virtual void DrawText();

	virtual bool SetText(const char16* _text);
	virtual bool SetText(const uint16 _resID);

	virtual void OnSetFocus();
	virtual void OnLostFocus();

	virtual void SetTextAlign(uint32 _align);

	virtual void SetSelectable(bool selectable);

	void UnfocusedEnter(bool enterWhenUnfocused);

	virtual void SetDrawType(eDrawType newType);

	void SetTextMargins(int32 top, int32 left, int32 bottom, int32 right);

	void SetFocusedDrawType(eDrawType drawType);


	virtual void SetRect(const Rect &rc);
	virtual void SetControlRect(const ControlRect &cr);
	virtual void Invalidate();

protected:
	void InvalidateCursor();


	virtual void CalcCursorXY();

	virtual bool MoveCursorOnUp()		{	return false;	}
	virtual bool MoveCursorOnDown()		{	return false;	}

	virtual bool AddChar(char16	ch);
	virtual void ChangeChar(char16	ch);
	virtual void DeleteSymbol();

	virtual void UpdateTextPos();


	Application			*pApp;

	int32		xOffset;

	eDrawType focusedDrawType;
	eDrawType unfocusedDrawType;

};

#endif//__GUI_EDIT_TEXT_