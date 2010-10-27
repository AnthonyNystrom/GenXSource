#ifndef __GUI_POPUP_DELEGATE__
#define __GUI_POPUP_DELEGATE__

#include "BaseTypes.h"

class PopUpDelegate
{
	friend class GUIPopUp;
protected:
	virtual bool			PopUpShouldOpen() = 0;
	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId) = 0;
	virtual void			PopUpOnItemSelected(int32 index, int32 id) = 0;
};


#endif//__GUI_BUTTON__