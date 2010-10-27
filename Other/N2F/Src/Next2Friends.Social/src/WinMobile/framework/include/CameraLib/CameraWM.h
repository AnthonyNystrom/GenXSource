/*! =================================================================
	\file Camera.h
================================================================= */

#ifndef __FRAMEWORK_CAMERA_WM__
#define __FRAMEWORK_CAMERA_WM__

#include "Camera.h"

#include "BaseTypes.h"

struct AEECameraNotify;
struct ICamera;

struct _AEEApplet;
struct _IFileMgr;

#define LOGS_ON

/*! \class Camera
*	\brief Class for working with camera.
*
*	Can preview camera with defined size of the preview window.	
*	Saves snapshot to device memory.
*
*	\author Andrey Karpiuk.
*	\version 0.3.0.
*	\date    2005.
*/ 

#define MAX_FILE_PATH 256
class CameraWM : public Camera
{
public:





public:

	CameraWM();
	virtual ~CameraWM();

	virtual bool SetPreviewPos(int32 x, int32 y);
	virtual int32 SetPreviewWidth(uint32 width);
	virtual int32 SetPreviewHeight(uint32 height);
	virtual int32 SetSnapshotWidth(uint32 width);
	virtual int32 SetSnapshotHeight(uint32 height);


	virtual bool StopPreview();
	virtual bool StartPreview();

	virtual bool SetFileForSnapshot(char8* capturedName);
	virtual bool MakeSnapshot();




private:
	char16 fileName[MAX_FILE_PATH];
	char16 fileDir[MAX_FILE_PATH];





private:

};

#endif//__Camera_h__