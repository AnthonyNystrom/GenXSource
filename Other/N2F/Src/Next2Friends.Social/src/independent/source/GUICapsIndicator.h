#ifndef __GUI_CAPS_INDICATOR__
#define __GUI_CAPS_INDICATOR__

#include "GUIIndicator.h"

class GUIText;

enum eCapsMode
{
	  ECM_FIRST_BIG = 0
	, ECM_SMALL
	, ECM_BIG
	, ECM_DIGITS


	, ECM_COUNT
};

class GUICapsIndicator : public GUIIndicator
{
public:
	GUICapsIndicator(GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUICapsIndicator(void);

	virtual void SetStringID(eCapsMode mode, int16 stringID);

	virtual void SetMode(eCapsMode mode);

	virtual void SetTextDrawType(eDrawType newDrawType);

	virtual void SetNextMode();

	virtual eCapsMode GetMode();

	
	
protected:
	GUIText	*text;

	eCapsMode currentMode;


	int16	modeStringID[3];
};


#endif