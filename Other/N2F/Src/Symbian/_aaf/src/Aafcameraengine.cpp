/*
============================================================================
Name        : Aafcameraengine.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : On board camera engine class implementation
============================================================================
*/
#include "AafCameraEngine.h"

#if USE_CAMERA

#include <caeengine.h>
#include <ImageConversion.h> 
#include <pathinfo.h> 
#include <eikenv.h>
#include <BAUTILS.H>
#include <eikspmod.h>
#include <aknwaitdialog.h>

#include "AafCameraContainer.h"

// -----------------------------------------------------------------------------
// CAafCameraEngine::CAafCameraEngine()
// Default constructor
// -----------------------------------------------------------------------------
//
CAafCameraEngine::CAafCameraEngine(CAafCameraContainer& aView, const TRect& aRect)
:CActive(EPriorityStandard), iContainer(aView), iViewRect(aRect)
	{
	CActiveScheduler::Add(this);
	iViewSize = aRect.Size();
	iImageCaptured = EFalse;
	iActiveCamera = KNoneCamera;
	}
	
// -----------------------------------------------------------------------------
// CAafCameraEngine::~CAafCameraEngine()
// Destructor
// -----------------------------------------------------------------------------
//
CAafCameraEngine::~CAafCameraEngine()
	{
	if(iEncoder)
		{
		delete iEncoder;
		iEncoder = NULL;			
		}
	
	if(iCaeEngine)
		{
		delete iCaeEngine;
		iCaeEngine = NULL;
		}
		
	delete iSaveBmp;
	iSaveBmp = NULL;

	iFs.Close();
	}
	
// -----------------------------------------------------------------------------
// CAafCameraEngine::NewLC()
// Symbian New method for Construction.
// -----------------------------------------------------------------------------
//
CAafCameraEngine* CAafCameraEngine::NewLC(CAafCameraContainer& aView,const TRect& aRect)
	{
	CAafCameraEngine* self = new (ELeave)CAafCameraEngine(aView ,aRect);
	CleanupStack::PushL(self);
	self->ConstructL();
	return self;
	}
	
// -----------------------------------------------------------------------------
// CAafCameraEngine::NewL()
// Symbian New method for Construction.
// -----------------------------------------------------------------------------
//
CAafCameraEngine* CAafCameraEngine::NewL(CAafCameraContainer& aView, const TRect& aRect)
	{
	CAafCameraEngine* self = CAafCameraEngine::NewLC(aView,aRect);
	CleanupStack::Pop(); // self;
	return self;
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::ConstructL()
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::ConstructL()
	{
	iActiveCamera = KMainCamera;

	iCaeEngine = CCaeEngine::NewL();

	iCaeEngine->SetCamAppEngineObserver(*this);

	iFs.Connect();
	iSaveBmp = new (ELeave) CFbsBitmap;
	
	CreateSoundPlayerL();

   	InitCameraL();
   	}

void CAafCameraEngine::CreateSoundPlayerL()
{
#ifdef __SERIES60_3X__    
	// Only in 3rd Edition. The snap sound file can be found 
	// in \private\<UID3>\ folder.
	User::LeaveIfError( iFs.CreatePrivatePath( EDriveC ) );
	User::LeaveIfError( iFs.SetSessionToPrivate( EDriveC ) );
	User::LeaveIfError( iFs.PrivatePath(iSoundFilePath) );
#else
#ifndef __WINS__  // don't save settings to z-drive in emulator
	// In 2nd Ed device the snap sound file will be in 
	// \system\apps\cameraapp\ folder.
	TFileName appFullName = (iAppUi.Application())->AppFullName();
	TParsePtr appPath(appFullName);
	iSoundFilePath = appPath.DriveAndPath();
#else 
	// For 2nd Ed emulator
	iSoundFilePath.Append(KEmulatorPath);
#endif //__WINS__
#endif

	iSoundPlayer = CMdaAudioPlayerUtility::NewL(*this, EMdaPriorityMin, EMdaPriorityPreferenceNone);
}

void CAafCameraEngine::StartWaitDialog()
{
	if(iWaitDialog)
	{
		delete iWaitDialog;
		iWaitDialog = NULL;
	}

	// For the wait dialog
	//create instance
	iWaitDialog = new (ELeave) CAknWaitDialog(REINTERPRET_CAST(CEikDialog**, &iWaitDialog));

	iWaitDialog->ExecuteLD(R_WAITNOTE_BLOCKING);
}

void CAafCameraEngine::StopWaitDialog()
{
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}
}
   	
// -----------------------------------------------------------------------------
// CAafCameraEngine::InitCameraL()
// Initialise the camera for still image capture
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::InitCameraL()
	{
	iCaeEngine->InitL();
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::CaptureSingleShot()
// Capture single still image
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::CaptureSingleShot()
	{
	TFileName soundFile(iSoundFilePath);
	soundFile.Append(KSnapSoundFile);

	// Open snap sound file for playing (asynchronously),
	// MapcInitComplete() would be called
	TRAPD(ignore, iSoundPlayer->OpenFileL(soundFile));

	iCaeEngine->StopViewFinder();
	iCaeEngine->SetSnapImageCreation(ETrue);
	iCaeEngine->CaptureStill();
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::SaveImage()
// Saves the picture
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::SaveImage(const TFileName &aFilePath)
	{
	if(iEncoder)
		{
		delete iEncoder;
		iEncoder=NULL;
		}
		
	StartWaitDialog();

	iEncoder = CImageEncoder::FileNewL(iFs, aFilePath, KJpegMimeType);

	iEncoder->Convert(&iStatus, *iSaveBmp);
	SetActive();
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::StartViewFinderL()
// starts the view finder
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::StartViewFinderL()
	{
	//StartWaitDialog();

	iImageCaptured = EFalse;
	iCaeEngine->StartViewFinderBitmapsL(iViewSize);
	}

void CAafCameraEngine::StopViewFinder()
	{
	iCaeEngine->StopViewFinder();
	}

void CAafCameraEngine::ReleaseCamera()
{
	iCaeEngine->Release();
}

void CAafCameraEngine::PowerOnCamera()
{
	iCaeEngine->PowerOn();
}

void CAafCameraEngine::SetResolution(const EPhotoResolution &aNewResolution)
{
	iPhotoResolution = aNewResolution;

	if (iCaeEngine)
	{
		TSize newSize;

		switch(iPhotoResolution)
		{
		case KRes640x480:
			newSize = TSize(640, 480);
			break;
		case KRes480x320:
			newSize = TSize(480, 320);
			break;
		case KRes320x240:
			newSize = TSize(320, 240);
			break;
		default:
			newSize = TSize(640, 480);
			break;
		}

		iCaeEngine->SetSnapImageSizeL(newSize);
	}	
}

TInt CAafCameraEngine::GetZoomValue()
{
	TInt zoomValue = 0;

	if (iCaeEngine)
		zoomValue= iCaeEngine->ZoomValue();

	__LOGSTR_TOFILE1("CAafCameraEngine::GetZoomValue() is %d", zoomValue);

	return zoomValue;
}

TInt CAafCameraEngine::GetMaxZoomValue()
{
	return iMaxZoom;
}

void CAafCameraEngine::SetZoomValue(const TInt &aNewZoom)
{
	if (aNewZoom <= iMaxZoom && aNewZoom >= 0)
	{
		if (iCaeEngine)
			iCaeEngine->SetZoomValueL(aNewZoom);
	}
}

void CAafCameraEngine::ChangeActiveCamera(TActiveCamera aNewCamera)
{
	__LOGSTR_TOFILE("CAafCameraEngine::ChangeActiveCamera() begins");

	// If new camera differs from new
	if (aNewCamera != iActiveCamera && CCaeEngine::CamerasAvailable() > 1)
	{
		if (iCaeEngine)
		{
			ReleaseCamera();

			delete iCaeEngine;
			iCaeEngine = NULL;
		}

		if (aNewCamera == KMainCamera)
			iCaeEngine = CCaeEngine::NewL(0);
		else
			iCaeEngine = CCaeEngine::NewL(1);

		iCaeEngine->SetCamAppEngineObserver(*this);

		iActiveCamera = aNewCamera;

		InitCameraL();
	}

	__LOGSTR_TOFILE("CAafCameraEngine::ChangeActiveCamera() ends");
}

// -----------------------------------------------------------------------------
// CAafCameraEngine::RunL()
// From CActive
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::RunL()
	{
	
	StopWaitDialog();

	if(iStatus == KErrNone)
		{
		iCaeEngine->StartViewFinderBitmapsL(iViewSize);		
		}
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::DoCancel()
// From CActive
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::DoCancel()
	{
	iCaeEngine->CancelCaptureStill();
	iEncoder->Cancel();
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::CaptureL()
// Starts capture of image / Video
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::CaptureL()
	{
		StartWaitDialog();

		CaptureSingleShot();
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoInitComplete()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void  CAafCameraEngine::McaeoInitComplete (TInt aError)
	{
	if(!aError)
		{
		
		TInt err;
		//camera mode
		TRAP(err, iCaeEngine->PrepareStillCaptureL(1));

		// Get camera info
		TCamAppEngineInfo cameraInfo;
		iCaeEngine->GetInfo(cameraInfo);

		// Get value of the maximum zoom
		iMaxZoom = cameraInfo.iMaxZoom;

		// Set flash auto mode (if flash stuff is available)
		TUint32 flashModesSupported = cameraInfo.iFlashModesSupported;

		if ((CCamera::EFlashForced & flashModesSupported) != 0)
		{
			TRAP(err, iCaeEngine->SetFlashModeL(CCamera::EFlashForced));
		}
			
		TRAP(err, StartViewFinderL());
		}
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoStillPrepareComplete()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//	
void CAafCameraEngine::McaeoStillPrepareComplete (TInt /*aError*/)
	{
	
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoVideoPrepareComplete()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoVideoPrepareComplete (TInt aError)
	{
	if(!aError)
		{
		
		}
	else 
		{
			
		}
	}

// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoViewFinderFrameReady()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoViewFinderFrameReady (CFbsBitmap &aFrame, TInt aError)
	{	
	//StopWaitDialog();

	if(!aError)
		{
		iContainer.DrawImage(aFrame);
		}
		
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoSnapImageReady()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoSnapImageReady (const CFbsBitmap &aBitmap, TInt /*aError*/)
	{
	StopWaitDialog();

	iSaveBmp->Duplicate(aBitmap.Handle());
	iImageCaptured = ETrue;
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoStillImageReady()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoStillImageReady (CFbsBitmap */*aBitmap*/, HBufC8 */*aData*/, TInt /*aError*/)
	{
	
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoVideoRecordingOn()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoVideoRecordingOn (TInt /*aError*/)
	{
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoVideoRecordingPaused()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoVideoRecordingPaused (TInt /*aError*/)
	{
	
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoVideoRecordingComplete()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoVideoRecordingComplete (TInt /*aError*/)
	{
	}
// -----------------------------------------------------------------------------
// CAafCameraEngine::McaeoVideoRecordingTimes()
// From MCamAppEngineObserver
// -----------------------------------------------------------------------------
//
void CAafCameraEngine::McaeoVideoRecordingTimes (TTimeIntervalMicroSeconds /*aTimeElapsed*/, TTimeIntervalMicroSeconds /*aTimeRemaining*/, TInt /*aError*/)
	{
		
	}

void CAafCameraEngine::MapcInitComplete(TInt aError, const TTimeIntervalMicroSeconds& /*aDuration*/)
{
	if (!aError)
	{
		iSoundPlayer->SetVolume(iSoundPlayer->MaxVolume()/3);
		iSoundPlayer->Play();
	}
}

void CAafCameraEngine::MapcPlayComplete(TInt /*aError*/)
{
	iSoundPlayer->Close();
}

#endif // USE_CAMERA
//End of File

