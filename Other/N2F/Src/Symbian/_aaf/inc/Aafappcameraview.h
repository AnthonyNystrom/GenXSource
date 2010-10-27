/*
============================================================================
Name        : Aafcameraview.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Camera view class declaration
============================================================================
*/
#include "common.h"

#if USE_CAMERA

#ifndef __AAFAPPCAMERAVIEW_H__
#define __AAFAPPCAMERAVIEW_H__

// INCLUDES
#include <coecntrl.h>
#include <coeview.h>
#include "Aafcameracontainer.h"

// FORWARD DECLARATIONS
#if USE_SKIN_SUPPORT
class CAknsBasicBackgroundControlContext;
#endif

// CLASS DECLARATION
class CAafAppCameraView : public CCoeControl, public MCoeView
{
public: // New methods

	/**
	* NewL.
	* Two-phased constructor.
	* Create a CAafAppCameraView object, which will draw itself to aRect.
	* @param aRect The rectangle this view will be drawn to.
	* @return a pointer to the created instance of CAafAppCameraView.
	*/
	static CAafAppCameraView* NewL( const TRect& aRect );

	/**
	* NewLC.
	* Two-phased constructor.
	* Create a CAafAppCameraView object, which will draw itself
	* to aRect.
	* @param aRect Rectangle this view will be drawn to.
	* @return A pointer to the created instance of CAafAppCameraView.
	*/
	static CAafAppCameraView* NewLC( const TRect& aRect );

	/**
	* ~CAafAppCameraView
	* Virtual Destructor.
	*/
	virtual ~CAafAppCameraView();

public:
	/**
	* Get currently used photo resolution
	*/
	EPhotoResolution GetCurrentResolution();

	/**
	* Set new photo resolution
	*/
	void SetResolution(const EPhotoResolution &aNewResolution);

	/**
	* Indicates whether image has been captured and is ready for saving
	*/
	TBool IsImageCaptured();

	/**
	* Capture still image
	*/
	void CaptureImageL();

	/**
	* Save captured image
	*/
	void SaveCapturedImageL();

	/**
	* Start view finder
	*/
	void StartViewFinderL();

	/**
	* Get current zoom value
	*/
	TInt GetZoomValue();

	/**
	* Get max available zoom value (0 is min value, by default)
	*/
	TInt GetMaxZoomValue();

	/**
	* Set new zoom value
	*/
	void SetZoomValue(const TInt &aNewZoom);

	/**
	* Zoom in (one step)
	*/
	void ZoomIn();

	/**
	* Zoom out (one step)
	*/
	void ZoomOut();

	/**
	* Set new camera to be used
	*/
	void ChangeActiveCamera(TActiveCamera aNewCamera)
	{
		if (iContainer)
		{
			iContainer->ChangeActiveCamera(aNewCamera);
		}
	}

	/**
	* Get id of currently used camera
	*/
	TActiveCamera ActiveCamera()
	{
		if (iContainer)
			return iContainer->ActiveCamera();

		return KNoneCamera;
	}

protected:  // Functions from base classes

	/**
	* From CCoeControl, Draw
	* Draw this CAafAppCameraView to the screen.
	* @param aRect the rectangle of this view that needs updating
	*/
	void Draw( const TRect& aRect ) const;

#if USE_SKIN_SUPPORT
	/**
	* To enable skin support
	*/
	TTypeUid::Ptr MopSupplyObject(TTypeUid aId);
#endif

	/**
	* From CoeControl, SizeChanged.
	* Called by framework when the view size is changed.
	*/
	virtual void SizeChanged();


public: // From MCoeView
	TVwsViewId ViewId() const;


private: // Constructors

	/**
	* ConstructL
	* 2nd phase constructor.
	* Perform the second phase construction of a
	* CAafAppCameraView object.
	* @param aRect The rectangle this view will be drawn to.
	*/
	void ConstructL(const TRect& aRect);

	/**
	* CAafAppCameraView.
	* C++ default constructor.
	*/
	CAafAppCameraView();

private:
	/**
	* From CCoeControl, CountComponentControls
	*/
	TInt CountComponentControls() const;

	/**
	* From CCoeControl, ComponentControl
	*/
	CCoeControl* ComponentControl(TInt aIndex) const;

	/**
	* From CCoeControl, HandleResourceChange
	*/
	void HandleResourceChange(TInt aType);

	/**
	* From CCoeControl, OfferKeyEventL
	*/
	TKeyResponse OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType);


private: // From MCoeView
	void ViewActivatedL(const TVwsViewId& aPrevViewId, TUid aCustomMessageId, const TDesC8& aCustomMessage);

	void ViewDeactivated();

private: // Member variables
	CAafCameraContainer* iContainer;

#if USE_SKIN_SUPPORT
	CAknsBasicBackgroundControlContext* iBgContext; // To enable skin support
#endif
};

#endif // __AAFAPPCAMERAVIEW_H__

#endif // USE_CAMERA
// End of File
