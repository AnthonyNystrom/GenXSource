/*
============================================================================
Name        : Aafcameracontainer.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Camera container control class declaration
============================================================================
*/
#include "common.h"

#if USE_CAMERA

#ifndef __AAFCAMERACONTAINER_H__
#define __AAFCAMERACONTAINER_H__

// INCLUDES
#include <coecntrl.h>
#include <FBS.H>
#include <aknprogressdialog.h> 
#include "Aafcameraengine.h"


// FORWARD DECLARATIONS
class CAknWaitDialog;


// CLASS DECLARATION

/**
*  CAafCameraContainer  container control class.
*  
*/
class CAafCameraContainer : public CCoeControl, MCoeControlObserver
    {
    public:
    	/**
        * Two-phased constructor.
        */    
    	static CAafCameraContainer* NewLC(const TRect& aRect);
    	/**
        * Two-phased constructor.
        */		
		static CAafCameraContainer* NewL(const TRect& aRect);
        /**
        * Destructor.
        */
        ~CAafCameraContainer();


	public:
		/**
		* Capture still image
		*/
		void CaptureL();

		/**
		* Switches off and release camera
		*/
		void ReleaseCamera();
		
		/**
		* Switches one camera
		*/
		void PowerOnCamera();

		/**
		* Get currently used photo resolution
		*/
		EPhotoResolution GetCurrentResolution();

		/**
		* Set new resolution
		*/
		void SetResolution(const EPhotoResolution &aNewResolution);

		/**
		* Indicates whether image is captured already
		*/
		TBool IsImageCaptured();

		/**
		* Save captured image to specified file
		*/
		void SaveImage(const TFileName &aFilePath);

		/**
		* Start view finding
		*/
		void StartViewFinderL();

		/**
		* Stop view finding
		*/
		void StopViewFinderL();
		
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
		* Set new camera to be used
		*/
		void ChangeActiveCamera(TActiveCamera aNewCamera)
		{
			if (iEngine)
			{
				iEngine->ChangeActiveCamera(aNewCamera);
			}
		}

		/**
		* Get id of currently used camera
		*/
		TActiveCamera ActiveCamera()
		{
			if (iEngine)
				return iEngine->ActiveCamera();

			return KNoneCamera;
		}

		/**
		* Init camera
		*/
		void InitCameraL();
    
    private: // Functions from base classes

       /**
        * From CoeControl,SizeChanged.
        */
        void SizeChanged();

        /**
        * From CCoeControl,Draw.
        */
        void Draw(const TRect& aRect) const;
        /**
        * From CCoeControl.
        */
        TKeyResponse OfferKeyEventL(const TKeyEvent &aKeyEvent, TEventCode aType);

  		/**
		* From MCoeControlObserver
		* Acts upon changes in the hosted control's state. 
		*
		* @param	aControl	The control changing its state
		* @param	aEventType	The type of control event 
		*/
        void HandleControlEventL(CCoeControl* aControl,TCoeEvent aEventType);
         
    public: //own methods
       // Changes the bitmap and calls draw
       void DrawImage(CFbsBitmap& aBitmap);
       
    private:
    		 // Constructors and destructor
        CAafCameraContainer(const TRect& aRect);
        /**
        * EPOC default constructor.
        * @param aRect Frame rectangle for container.
        */
        void ConstructL(const TRect& aRect);
	       
    private: //data
        
        CFbsBitmap* iBitmap;
    	CAafCameraEngine* iEngine;
    };

#endif // __AAFCAMERACONTAINER_H__

#endif // USE_CAMERA