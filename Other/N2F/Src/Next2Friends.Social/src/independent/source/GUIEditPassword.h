#ifndef __GUI_EDIT_PASSWORD_
#define __GUI_EDIT_PASSWORD_

#include "GUIEditText.h"

class GUIEditPassword : public GUIEditText
{
public:
	GUIEditPassword(int32 bufSize = MAX_SIZE, GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect());
	
	virtual ~GUIEditPassword();

	virtual void DrawText();

private:

	virtual void ChangeChar(char16	ch);

	void CreatePasswordText();

	virtual void MoveCursorOnPrev()		{	}
	virtual void CalcCursorXY();


	char16		*	passwordBuffer;
};

#endif//__GUI_EDIT_PASSWORD_