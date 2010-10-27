#ifndef __GUI_BUTTON__
#define __GUI_BUTTON__

#include "GUILayoutBox.h"

class GUIButton : public GUILayoutBox
{
public:
	GUIButton(GUIControlContainer *parent = NULL, const ControlRect &rc = ControlRect(), bool isVertical = false);
	virtual ~GUIButton(void);

	virtual void Update();

	virtual void OnSetFocus();
	virtual void OnLostFocus();
	virtual void SetSelectable(bool selectable);


	void SetSelectedDrawType(eDrawType drawType);
	void SetUnselectedDrawType(eDrawType drawType);
	void SetPressedDrawType(eDrawType drawType);
	void SetDisabledDrawType(eDrawType drawType);
protected:
	bool isPressed;
	eDrawType selectedDrawType;
	eDrawType unselectedDrawType;
	eDrawType pressedDrawType;
	eDrawType disabledDrawType;
};


#endif//__GUI_BUTTON__