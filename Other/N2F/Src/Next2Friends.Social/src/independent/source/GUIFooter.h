#ifndef _GUI_FOOTER_H_
#define _GUI_FOOTER_H_

#include "GUIControlContainer.h"
#include "GUIEventListener.h"


class GUIFooter : public GUIControlContainer, public GUIEventListener
{
public:
	GUIFooter();
	virtual ~GUIFooter();

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	void SetText(int16 leftStringID, int16 rightStringID);
	void SetText(const char16 * leftString, const char16 * rightString);

	void SetPositiveText(int16 leftStringID);
	void SetNegativeText(int16 rightStringID);

protected:

};


#endif
