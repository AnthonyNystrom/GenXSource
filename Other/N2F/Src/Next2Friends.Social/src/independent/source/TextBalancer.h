#ifndef __GUI_TEXT_BALANCER__
#define __GUI_TEXT_BALANCER__

#include "BaseTypes.h"

class GUIText;

enum eTextBalancerState
{
	ETBS_PAUSED = 0,
	ETBS_MOVE_TO_SHOW,
	ETBS_MOVE_BACK

};

class TextBalancer
{
public:
	TextBalancer(GUIText *textToBalance);
	~TextBalancer();

	void Reset();
	void Update();
protected:
	GUIText *text;
	eTextBalancerState state;
	int32 waitCounter;

};


#endif//__GUI_BUTTON__