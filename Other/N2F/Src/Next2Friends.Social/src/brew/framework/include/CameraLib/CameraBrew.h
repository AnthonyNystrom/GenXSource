/*! =================================================================
	\file Camera.h
================================================================= */

#ifndef __FRAMEWORK_CAMERA_BREW__
#define __FRAMEWORK_CAMERA_BREW__

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
class CameraBrew : public Camera
{
public:





public:

	CameraBrew();
	virtual ~CameraBrew();

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
	virtual bool Snapshot();

	_AEEApplet		*pApp;

	ICamera *pICamera; //!<BREW ICamera interface.
	int32 counter;		//!<Is used for debug info only.

	char8 fileName[256];





private:
	// ***************************************************
	//! \brief    	Function is the callback of the preview function.
	//!
	//! \param[in]  pUser - camera data;
	//! \param[in]  pn - AEECameraNotify data.
	//!
	//! \return   	No returned value.
	// ***************************************************
	static void cbPreviewNotify(void *pUser, AEECameraNotify *pn);

	// ***************************************************
	//! \brief    	Function is the callback of the snapshot function.
	//!
	//! \param[in]  User - camera data;
	//! \param[in]  pn - AEECameraNotify data.
	//!
	//! \return   	No returned value.
	// ***************************************************
	static void cbSnapshotNotify(void * User, AEECameraNotify *pn);

};

#endif//__Camera_h__