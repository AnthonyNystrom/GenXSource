#ifndef __GUI_INDICATOR__
#define __GUI_INDICATOR__

#include "GUIControlContainer.h"
#include "IndicatorDelegate.h"



class GUIIndicator : public GUIControlContainer
{
public:

	enum eIndicatorType
	{
		EIT_CAPS = 0
		, EIT_CHARACTER


		, EIT_COUNT
	};

	GUIIndicator(GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUIIndicator(void);
	
	virtual void Show();
	virtual void Hide();

	virtual GUIIndicator::eIndicatorType GetType();

	virtual void SetDelegate(IndicatorDelegate *pDelegate);

protected:
	IndicatorDelegate *indicatorDelegate;
	eIndicatorType type;
};


#endif