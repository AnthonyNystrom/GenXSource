#ifndef __SCREEN_CAMERA__
#define __SCREEN_CAMERA__

#include "basescreen.h"
#include "Camera.h"

class Camera;
class GUIText;

class ScreenCamera :
	public BaseScreen, public CameraListener
{
	//enum ePopUpItems
	//{
	//		EPI_TAKE_SHOT = 0
	//	,	EPI_BACK


	//	,	EPI_COUNT
	//};

public:
	ScreenCamera(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenCamera(void);

	virtual bool	PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);



	virtual void OnCameraFailed();
	virtual void OnSnaphotCreated();
	virtual void OnPreviewStarted();
	virtual void OnPreviewStopped();
	virtual void OnCameraCancelled();

private:

	int32 prevWidth;
	Camera *camera;

};
#endif
