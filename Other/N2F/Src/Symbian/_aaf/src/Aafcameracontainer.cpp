/*
============================================================================
Name        : Aafcameracontainer.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Camera container control class implementation
============================================================================
*/
#include "AafCameraContainer.h"

#if USE_CAMERA

// INCLUDE FILES
#include <aknwaitdialog.h>
#include "aaf.rsg"


// ================= MEMBER FUNCTIONS =======================
// -----------------------------------------------------------------------------
// CAafCameraContainer::NewLC()
// Symbian New method for Construction.
// -----------------------------------------------------------------------------
//
CAafCameraContainer* CAafCameraContainer::NewLC(const TRect& aRect)
	{
	CAafCameraContainer* self = new (ELeave) CAafCameraContainer(aRect);
	CleanupStack::PushL(self);
	self->ConstructL(aRect);
	return self;
	}
	
// -----------------------------------------------------------------------------
// CAafCameraContainer::NewLC()
// Symbian New method for Construction.
// -----------------------------------------------------------------------------
//
CAafCameraContainer* CAafCameraContainer::NewL(const TRect& aRect)
	{
	CAafCameraContainer* self = CAafCameraContainer::NewLC(aRect);
	CleanupStack::Pop(self);
	return self;
	}
	
// -----------------------------------------------------------------------------
// CAafCameraContainer::CAafCameraContainer()
// Symbian New method for Construction.
// -----------------------------------------------------------------------------
//	
CAafCameraContainer::CAafCameraContainer(const TRect& aRect)
	{
			
	}
// ---------------------------------------------------------
// CAafCameraContainer::ConstructL(const TRect& aRect)
// EPOC two phased constructor
// ---------------------------------------------------------
//
void CAafCameraContainer::ConstructL(const TRect& aRect)
    {
    CreateWindowL();
    SetExtentToWholeScreen();
    SetRect(aRect);
    ActivateL();
    
    iEngine = CAafCameraEngine::NewL(*this, aRect);
    iEngine->InitCameraL();
    }
    
// ---------------------------------------------------------
// CAafCameraContainer::ConstructL(const TRect& aRect)
// Destructor
// ---------------------------------------------------------
//
CAafCameraContainer::~CAafCameraContainer()
    {
    
    if(iEngine)
      	{
		delete iEngine;
		iEngine=NULL;	    	
      	}
    delete iBitmap;
    iBitmap = NULL;
     
    }

void CAafCameraContainer::CaptureL()
{
	if (iEngine)
	{
		iEngine->CaptureL();
	}
}

void CAafCameraContainer::ReleaseCamera()
{
	if (iEngine)
	{
		iEngine->ReleaseCamera();
	}
}

void CAafCameraContainer::PowerOnCamera()
{
	if (iEngine)
	{
		iEngine->PowerOnCamera();
	}
}

EPhotoResolution CAafCameraContainer::GetCurrentResolution()
{
	if (iEngine)
	{
		return iEngine->GetCurrentResolution();
	}

	return KRes640x480;
}

void CAafCameraContainer::SetResolution(const EPhotoResolution &aNewResolution)
{
	if (iEngine)
	{
		iEngine->SetResolution(aNewResolution);
	}
}

TBool CAafCameraContainer::IsImageCaptured()
{
	if (iEngine)
	{
		return iEngine->IsImageCaptured();
	}

	return EFalse;
}

void CAafCameraContainer::SaveImage(const TFileName &aFilePath)
{
	if (iEngine)
	{
		iEngine->SaveImage(aFilePath);
	}
}

void CAafCameraContainer::StartViewFinderL()
{
	if (iEngine)
	{
		iEngine->StartViewFinderL();
	}
}

void CAafCameraContainer::StopViewFinderL()
{
	if (iEngine)
	{
		if (iBitmap)
		{
			delete iBitmap;
			iBitmap = NULL;
		}

		DrawNow();

		iEngine->StopViewFinder();
	}
}

TInt CAafCameraContainer::GetZoomValue()
{
	if (iEngine)
	{
		return iEngine->GetZoomValue();
	}

	return 0;
}

TInt CAafCameraContainer::GetMaxZoomValue()
{
	if (iEngine)
	{
		return iEngine->GetMaxZoomValue();
	}

	return 0;
}

void CAafCameraContainer::SetZoomValue(const TInt &aNewZoom)
{
	if (iEngine)
	{
		iEngine->SetZoomValue(aNewZoom);
	}
}

void CAafCameraContainer::InitCameraL()
{
	if (iEngine)
	{
		iEngine->InitCameraL();
	}
}

// ---------------------------------------------------------
// CAafCameraContainer::SizeChanged()
// Called by framework when the view size is changed
// ---------------------------------------------------------
//
void CAafCameraContainer::SizeChanged()
    {    
		TRect clientRect = Rect();
		Draw(clientRect);
    }

// ---------------------------------------------------------
// CAafCameraContainer::Draw(const TRect& aRect) const
// ---------------------------------------------------------
//
void CAafCameraContainer::Draw(const TRect&/* aRect*/) const
    {
    CWindowGc& gc = SystemGc();
    
	if(iBitmap)
		{
			TRect drawRect( Rect() );
    		gc.DrawBitmap(drawRect, iBitmap);
		}
	}

// ---------------------------------------------------------
// CAafCameraContainer::HandleControlEventL(
//     CCoeControl* aControl,TCoeEvent aEventType)
// ---------------------------------------------------------
//
void CAafCameraContainer::HandleControlEventL(
    CCoeControl* /*aControl*/,TCoeEvent /*aEventType*/)
    {
    // TODO: Add your control event handler code here
    }
    
// ---------------------------------------------------------
// CAafCameraContainer::OfferKeyEventL()
// Handling key events
// ---------------------------------------------------------
//
TKeyResponse CAafCameraContainer::OfferKeyEventL(const TKeyEvent &aKeyEvent, TEventCode aType)
	{
	__LOGSTR_TOFILE("CAafCameraContainer::OfferKeyEventL() begins");

	if(aKeyEvent.iScanCode == EStdKeyDevice3 && aType == EEventKey)
		{
		iEngine->CaptureL();
		return EKeyWasConsumed;
		}
	return EKeyWasNotConsumed;
	}

// ---------------------------------------------------------
// CAafCameraContainer::DrawImage()
// Changes the bitmap and calls draw
// ---------------------------------------------------------
//
void CAafCameraContainer::DrawImage(CFbsBitmap& aBitmap)
	{
	iBitmap = &aBitmap;	
	DrawNow();
	}

#endif