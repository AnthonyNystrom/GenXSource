#ifndef __GUI_MENU_ITEM__
#define __GUI_MENU_ITEM__

#include "GUIButton.h"

class GUIMenuItem : public GUIButton
{
public:
	GUIMenuItem(eDrawType selDrawType, GUIControlContainer *parent = NULL, const ControlRect &rc = ControlRect(), bool isVertical = false);
	virtual ~GUIMenuItem(void);

	virtual void OnSetFocus();
	virtual void OnLostFocus();

	virtual void Draw();


};


#endif//__GUI_BUTTON__