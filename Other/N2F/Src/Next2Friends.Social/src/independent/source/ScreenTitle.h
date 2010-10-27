#ifndef __SCREEN_TITLE__
#define __SCREEN_TITLE__

#include "basescreen.h"

#define  LOAD_SKINS_PER_FRAME	2

class GUIImage;
class GUIControl;

#ifdef N2F_SCREEN_176X208
	#define LOADBAR_WIDTH		126
	#define LOADBAR_X_OFFSET	25
	#define LOADBAR_Y_OFFSET	190
#endif
#ifdef N2F_SCREEN_320X240
	#define LOADBAR_WIDTH		210
	#define LOADBAR_X_OFFSET	58
	#define LOADBAR_Y_OFFSET	160
#endif


class ScreenTitle :
	public BaseScreen
{
public:
	ScreenTitle(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenTitle(void);


	virtual void Update();

	virtual bool	PopUpShouldOpen();


private:

	GUIImage *img;
	GUIControl *loading;

	void SetTiles();

	int32 skinInitCounter;
};
#endif
