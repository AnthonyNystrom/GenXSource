#ifndef _GUI_HEADER_H_
#define _GUI_HEADER_H_

#include "GUIControlContainer.h"
#include "GUIEventListener.h"
#include "IndicatorDelegate.h"

class GUIIndicator;
class GUICapsIndicator;
class GUIImage;

class GUIHeader : public GUIControlContainer, public GUIEventListener, public IndicatorDelegate
{
public:
	enum eNetIndicator
	{
        ENI_OUTBOX = 0,
		ENI_INBOX,
		ENI_DASHBOARD,

		ENI_COUNT
	};

	GUIHeader();
	virtual ~GUIHeader();

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	//void SetText(int16 stringID);
	//void SetText(const char16 * newString);

	virtual void IndicatorMustShow(GUIIndicator *indicator);
	virtual void IndicatorMustHide(GUIIndicator *indicator);

	virtual void Draw();

	void ShowNetInidicator(eNetIndicator indicator, bool show);

protected:

	GUICapsIndicator *caps;

	GUIImage *indicators[GUIHeader::ENI_COUNT];
};


#endif
