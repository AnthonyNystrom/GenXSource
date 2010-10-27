#ifndef __SCREEN_VIEW_PHOTO__
#define __SCREEN_VIEW_PHOTO__

#include "basescreen.h"

#include "Graphics.h"

class GUIPhotoImage;
class SurfacesPool;


class ScreenViewPhoto :
	public BaseScreen
{
	//enum eMenuItems
	//{


	//		EMI_COUNT

	//};
	enum ePopUpItems
	{
			EPI_ATTACH = 0
		,	EPI_REMOVE
		,	EPI_BACK
		,	EPI_UPLOAD

		,	EPI_COUNT
	};

public:
	ScreenViewPhoto(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenViewPhoto(void);

	//virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	//virtual void			PopUpOnItemSelected(int32 index, int32 id);
	virtual bool			PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);

private:

	GUIPhotoImage *image;
	SurfacesPool *surfPool;
	LibPhotoItem *oldItem;

	ePopUpItems state;



};
#endif
