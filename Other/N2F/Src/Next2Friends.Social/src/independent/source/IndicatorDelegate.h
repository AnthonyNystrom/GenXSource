#ifndef __GUI_INDICATOR_DELEGATE__
#define __GUI_INDICATOR_DELEGATE__

#include "BaseTypes.h"

class IndicatorDelegate
{
	friend class GUIIndicator;
protected:
	virtual void			IndicatorMustShow(GUIIndicator *indicator) = 0;
	virtual void			IndicatorMustHide(GUIIndicator *indicator) = 0;
};


#endif//__GUI_BUTTON__