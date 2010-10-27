#ifndef __FRAMEWORK_CAMERA__
#define __FRAMEWORK_CAMERA__

#include "BaseTypes.h"
class File;

class CameraListener
{
public:
	virtual void OnCameraFailed() = 0;
	virtual void OnSnaphotCreated() = 0;
	virtual void OnPreviewStarted() = 0;
	virtual void OnPreviewStopped() = 0;
	virtual void OnCameraCancelled() = 0;
protected:
private:
};


class Camera
{
public:

	/*! Flags used to describe the state of the camera. */
	enum eCameraState
	{
		ECS_FAILED,			/*!<Some of BREW ICamera methods return not SUCCESS value.*/	
		ECS_READY,			/*!<Camera is ready for work. Can be used in preview or snapshot mode.*/	
		ECS_PREVIEW,			/*!<Camera works in preview mode.*/	
		ECS_SNAPSHOT,		/*!<Camera works in snapshot mode.*/	
		ECS_SNAPSHOT_DONE,	/*!<Snapshot was made without errors.*/	
		ECS_SNAPSHOT_FAILED	/*!<Error occurred during the snapshotting. Snapshot wasn't made.*/		
	};

	static Camera *Create();

	virtual bool SetFileForSnapshot(char8* capturedName) = 0;//must be set before starting preview

	virtual bool StartPreview() = 0;
	virtual bool StopPreview() = 0;
	virtual bool MakeSnapshot() = 0;

	virtual bool SetPreviewPos(int32 x, int32 y) = 0;
	// ***************************************************
	//! \brief    	SetPreviewWidth
	//! 
	//! \param      width
	//! \return   	int32 preview height for required width
	// ***************************************************
	virtual int32 SetPreviewWidth(uint32 width) = 0;

	// ***************************************************
	//! \brief    	SetPreviewHeight
	//! 
	//! \param      height
	//! \return   	int32 preview width for required height
	// ***************************************************
	virtual int32 SetPreviewHeight(uint32 height) = 0;
	// ***************************************************
	//! \brief    	SetSnapshotWidth
	//! 
	//! \param      width
	//! \return   	int32 snapshot height for required width
	// ***************************************************
	virtual int32 SetSnapshotWidth(uint32 width) = 0;
	// ***************************************************
	//! \brief    	SetSnapshotHeight
	//! 
	//! \param      height
	//! \return   	int32 snapshot width for required height
	// ***************************************************
	virtual int32 SetSnapshotHeight(uint32 height) = 0;

	void SetListener(CameraListener *listener);//must be set before starting preview

	eCameraState GetState()
	{
		return state;
	};

	virtual ~Camera();

protected:
	Camera();

	eCameraState state; //!<Current camera mode.

	Rect previewRect;
	int32 snapshotWidth; //!<Width of the snapshot.	
	int32 snapshotHeight; //!<Height of the snapshot.	

	CameraListener *pListener;


private:


};
#endif