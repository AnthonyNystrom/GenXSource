#ifndef __GUI_POPUP__
#define __GUI_POPUP__

#include "GUIControlContainer.h"
#include "GUIEventListener.h"

class PopUpDelegate;
class GUIListView;

enum ePopUpState
{
	EPS_RISE = 0,
	EPS_PROCESS,
	EPS_CLOSING
};

#define POPUP_ITEM_TEXT_ID 8546

class GUIPopUp : public GUIControlContainer, public GUIEventListener
{

public:
	GUIPopUp(int32 baseLine);
	virtual ~GUIPopUp(void);

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	virtual void Update();
	virtual void Draw();
	virtual void DrawFinished();
    



	void SetDelegate(PopUpDelegate *newDelegate);

	void Show();

protected:

	GUIListView *list;

	ePopUpState state;
	int32 yLine;
	int32 closeCounter;
	Rect targetRect;
	void Close();
	PopUpDelegate *popUpDelegate;

	VList itemsList;
	int32 selectedItem;

	GUIControl *prevActive;
};


#endif//__GUI_BUTTON__