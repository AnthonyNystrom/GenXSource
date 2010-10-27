#ifndef __GUI_LOCK_WINDOW__
#define __GUI_LOCK_WINDOW__

#include "GUIControlContainer.h"
#include "GUIEventListener.h"

class GUIMultiString;



class GUILockWindow : public GUIControlContainer
{

public:
	GUILockWindow();
	virtual ~GUILockWindow(void);

	
	virtual void Update();
	virtual void Draw();
	virtual void DrawFinished();



	void Show();
	void Hide();

	GUIMultiString *GetText();

protected:
	
	void Close();
	int32 closeCounter;
	int32 openCounter;

	GUILayoutBox	*layout;
	GUIMultiString	*text;

	GUIControl *prevActive;
};


#endif//__GUI_LOCK_WINDOW__