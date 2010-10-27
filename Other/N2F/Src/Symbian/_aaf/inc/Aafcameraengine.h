/*
============================================================================
Name        : Aafcameraengine.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : On board camera engine class declaration
============================================================================
*/

#include "common.h"

#if USE_CAMERA

#ifndef CAMENGINE_H
#define CAMENGINE_H

// INCLUDES
#include <e32std.h>
#include <e32base.h>
#include <caeengine.h>

#include <MdaAudioSamplePlayer.h>  // For playing sounds


// CLASS DECLARATION
class CImageEncoder;
class CAafCameraContainer;
class CAknWaitDialog;

/**
*  CAafCameraEngine
* 
*/

// Possible photo resolutions which could be used
enum EPhotoResolution
{
	KRes640x480,
	KRes480x320,
	KRes320x240
};

class CAafCameraEngine : public CActive, public MCamAppEngineObserver, public MMdaAudioPlayerCallback

{
public: // Constructors and destructor

	/**
        * Destructor.
        */
	~CAafCameraEngine();

    /**
    * Two-phased constructor.
    */
	static CAafCameraEngine* NewL(CAafCameraContainer& aView,const TRect& aRect);

    /**
    * Two-phased constructor.
    */
	static CAafCameraEngine* NewLC(CAafCameraContainer& aView,const TRect& aRect);

protected:

	// From MMdaAudioPlayerCallback
	void MapcInitComplete(TInt aError, const TTimeIntervalMicroSeconds &aDuration);
	void MapcPlayComplete(TInt aError);
	
	//methods of parent class MCamAppEngineObserver
	void  McaeoInitComplete (TInt aError);
	void  McaeoStillPrepareComplete (TInt aError);
	void  McaeoVideoPrepareComplete (TInt aError);
	void  McaeoViewFinderFrameReady (CFbsBitmap &aFrame, TInt aError);
	void  McaeoSnapImageReady (const CFbsBitmap &aBitmap, TInt aError);
	void  McaeoStillImageReady (CFbsBitmap *aBitmap, HBufC8 *aData, TInt aError);
	void  McaeoVideoRecordingOn (TInt aError);
	void  McaeoVideoRecordingPaused (TInt aError);
	void  McaeoVideoRecordingComplete (TInt aError);
	void  McaeoVideoRecordingTimes (TTimeIntervalMicroSeconds aTimeElapsed, TTimeIntervalMicroSeconds aTimeRemaining, TInt aError);
	
public:
	//own methods;
	// Initialise the camera for still image capture
	void InitCameraL();
	// Capture single still image
	void CaptureSingleShot();
	// Starts capture of image / Video
	void CaptureL();
	// Saves the picture
	void SaveImage(const TFileName &aFilePath);
	// starts the view finder
	void StartViewFinderL();
	// stops the view finder
	void StopViewFinder();
	// releases camer and switch it off
	void ReleaseCamera();
	// switches camera on
	void PowerOnCamera();

	// get currently used photo resolution
	EPhotoResolution GetCurrentResolution()
	{
		return iPhotoResolution;
	}
	// set new photo resolution
	void SetResolution(const EPhotoResolution &aNewResolution);

	// indicates whether image is captured already
	TBool IsImageCaptured()
	{
		return iImageCaptured;
	}

	// get current zoom value
	TInt GetZoomValue();
	// get max available zoom value (0 is min value, by default)
	TInt GetMaxZoomValue();
	// set new zoom value
	void SetZoomValue(const TInt &aNewZoom);
	
	// Change currently used camera (if it's possible)
	void ChangeActiveCamera(TActiveCamera aNewCamera);

	// Get id of currently used camera
	TActiveCamera ActiveCamera()
	{
		return iActiveCamera;
	}
	
private:
	//From CActive
	void RunL();
	//From CActive
	void DoCancel();
	
	/**
    * Constructor for performing 1st stage construction
    */
	CAafCameraEngine(CAafCameraContainer& aView, const TRect& aRect);

	/**
    * EPOC default constructor for performing 2nd stage construction
    */
	void ConstructL();

	/**
	* Initializes snap sound player
	*/
	void CreateSoundPlayerL();

	/**
	* Starts waitnote dialog (with empty buttons)
	*/	
	void StartWaitDialog();

	/**
	* Stops waitnote dialog (with empty buttons)
	*/
	void StopWaitDialog();

private:
	//data members
	CCaeEngine* iCaeEngine;
	CImageEncoder* iEncoder;
	CFbsBitmap* iSaveBmp;
	CAafCameraContainer& iContainer;
	TRect iViewRect;
	RFile iFile;
	RFs iFs;
	TBuf<10> errBuf;
	TSize iViewSize;

	EPhotoResolution iPhotoResolution;
	TBool iImageCaptured;
	TInt iMaxZoom;

	TActiveCamera iActiveCamera;

	TFileName				  iSoundFilePath;
	CMdaAudioPlayerUtility*   iSoundPlayer;

	CAknWaitDialog* iWaitDialog; // Wait dialog
};

#endif // CAMENGINE_H

#endif // USE_CAMERA