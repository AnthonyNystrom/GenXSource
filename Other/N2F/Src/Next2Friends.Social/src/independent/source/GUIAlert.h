#ifndef __GUI_ALERT__
#define __GUI_ALERT__

#include "GUIControlContainer.h"
#include "GUIEventListener.h"

class GUIListView;
class GUIMenuItem;
class GUIMultiString;

enum eAlertState
{
	EAS_RISE = 0,
	EAS_PROCESS,
	EAS_CLOSING
};


class GUIAlert : public GUIControlContainer, public GUIEventListener
{

public:
	GUIAlert();
	virtual ~GUIAlert(void);

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	virtual void Update();
	virtual void Draw();
	virtual void DrawFinished();
    



	void Show();

	GUIMultiString *GetText();

protected:


	GUILayoutBox	*layout;
	GUIMultiString	*text;
	GUIMenuItem		*button;
	eAlertState state;
	int32 closeCounter;
	Rect targetRect;
	void Close();

	GUIControl *prevActive;
};


#endif//__GUI_BUTTON__