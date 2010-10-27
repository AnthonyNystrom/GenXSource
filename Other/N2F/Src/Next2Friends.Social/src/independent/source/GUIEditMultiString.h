#ifndef __GUI_EDIT_MULTI_STRING_H_
#define __GUI_EDIT_MULTI_STRING_H_

#include "GUIMultiString.h"
#include "GUIInputText.h"
#include "GUIEventListener.h"

class GUIEditMultiString : public GUIInputText, public GUIMultiString, public GUIEventListener
{
public:
	GUIEditMultiString(int32 bufSize = MAX_SIZE, eEditTextType eeType = EETT_ALL, GUIControlContainer * parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUIEditMultiString();

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	virtual void SetText(const char16 * pText);
	virtual void SetText(uint16 resourseID);

	virtual	void SetTextAlign(uint32 align);

	virtual void Update();
	virtual void Draw();

	virtual void OnSetFocus();
	virtual void OnLostFocus();

	virtual void SetDrawType(eDrawType newType);
	virtual void SetTextMargins(int32 top, int32 left, int32 bottom, int32 right);


	virtual void SetRect(const Rect &rc);
	virtual Rect GetRect();

	void SetRealRect(const Rect &rc);

	virtual void SetControlRect(const ControlRect &cr);

	void SetResizeable(bool resizeable);
	bool Resizeable()					{	return isResizeable; }

	virtual void Invalidate();

	virtual bool InsertText(const char16 *textToInsert);


private:

	void InvalidateCursor();

	virtual void DeleteSymbol();
	virtual bool MoveCursorOnUp();
	virtual bool MoveCursorOnDown();

	virtual void CalcCursorXY();

	virtual void SetStartCursorPos();

	virtual bool AddChar(char16	ch);
	virtual void ChangeChar(char16	ch);

	virtual void ParseStrings(int32 scrollW);

	void ResizeRect();
	bool isResizeable;

	Rect realRect;

};

#endif // __GUI_EDIT_MULTI_STRING_H_