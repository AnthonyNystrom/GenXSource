#ifndef __GUI_POPUP_ITEM__
#define __GUI_POPUP_ITEM__

#include "GUIButton.h"

class GUIPopUpItem : public GUIButton
{
public:
	GUIPopUpItem(GUIControlContainer *parent = NULL);
	virtual ~GUIPopUpItem(void);

	virtual void OnSetFocus();
	virtual void OnLostFocus();
	
	virtual void Draw();

	virtual void SetOuterID(int32 id)
	{
		outerID = id;
	};

	virtual int32 GetOuterID()
	{
		return outerID;
	};

private:
	int32 outerID;

};


#endif