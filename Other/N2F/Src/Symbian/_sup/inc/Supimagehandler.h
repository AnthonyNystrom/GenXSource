/*
============================================================================
Name        : Supimagehandler.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Image resizing/conversion stuff class declaration
============================================================================
*/
#ifndef __SUPIMAGEHANDLER_H__
#define __SUPIMAGEHANDLER_H__

#include <e32base.h> 
#include <fbs.h>

// Image converter library API header
#include <ImageConversion.h>
// Bitmap transforms API header
#include <BitmapTransforms.h>



// FORWARD DECLARATIONS
class MRequestObserver;

// Loads, saves and manipulates a bitmap
class CSupImageHandler : public CActive
    {
public:
	// Construction
	CSupImageHandler(CFbsBitmap& aBitmap, RFs& aFs, MRequestObserver& aCallback);
    void ConstructL();
	~CSupImageHandler();

	// Load/save operations
	// Operations with files
	void LoadFileL(const TFileName& aFileName, TInt aSelectedFrame = 0);
	void SaveImageToFileL(const TFileName& aFileName, const TUid& aImageType, const TUid& aImageSubType);

	// Operations with buffers
	void LoadBufferL(TDesC8 &aSourceData, TInt aSelectedFrame = 0);	
	void SaveImageToBufferL(HBufC8*& aDestinationData, const TDesC8 &aMIMEType);

	// Image command handling functions
	void FrameRotate(TBool aClockwise);
	void Mirror();
	void Flip();
	void ZoomFrame(TBool aZoomIn);

	// Indicates whether image exceeds width or height thresholds
	TBool ExceedsThresholds();

	// Fit image size to the width and height threshold, i.e. not greatet than those values
	void FitToThresholds();

	// Frame information
	const TFrameInfo& FrameInfo() const {return iFrameInfo;}
	// Frame count
	TInt FrameCount() const {return iFrameCount;}

private:
	// Active object interface
	void RunL(); 
	void DoCancel();

private:
	// Callback interface
	MRequestObserver& iCallback;
	// Bitmap
	CFbsBitmap& iBitmap; 
	// File server handle
	RFs& iFs;
	// Frame information
	TFrameInfo iFrameInfo;
	// Frame count
	TInt iFrameCount;
	// Image file loader
	CImageDecoder* iLoadUtil;
	// Image files saver
	CImageEncoder* iSaveUtil;
	// Bitmap rotator
	CBitmapRotator* iBitmapRotator;
	// Bitmap zoomer
	CBitmapScaler* iBitmapScaler;
    };

#endif // __SUPIMAGEHANDLER_H__