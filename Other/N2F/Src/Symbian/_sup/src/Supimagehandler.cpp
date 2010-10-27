/*
============================================================================
Name        : Supimagehandler.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Image resizing/conversion stuff class implementation
============================================================================
*/
#include "SupImageHandler.h"
#include "common.h"

CSupImageHandler::CSupImageHandler(CFbsBitmap& aBitmap, RFs& aFs, MRequestObserver& aCallback)
	:	iBitmap(aBitmap), iFs(aFs), iCallback(aCallback), CActive(CActive::EPriorityStandard)
	{}

void CSupImageHandler::ConstructL()
	{
	iBitmapRotator = CBitmapRotator::NewL();
	iBitmapScaler = CBitmapScaler::NewL();
	CActiveScheduler::Add(this);
	}


CSupImageHandler::~CSupImageHandler()
	{
	delete iLoadUtil;
	delete iSaveUtil;
	delete iBitmapRotator;
	delete iBitmapScaler;
	}

void CSupImageHandler::LoadFileL(const TFileName& aFileName, TInt aSelectedFrame)
	{
	__ASSERT_ALWAYS(!IsActive(),User::Invariant());
	// create a CImageDecoder to read the specified file
	delete iLoadUtil;
	iLoadUtil = NULL;
	iLoadUtil = CImageDecoder::FileNewL(iFs, aFileName);
	// store the frame information and frame count
	iFrameInfo = iLoadUtil->FrameInfo(aSelectedFrame);
	iFrameCount = iLoadUtil->FrameCount();

	// resize the destination bitmap to fit the required size
	//TRect bitmapSize = iFrameInfo.iFrameCoordsInPixels;

	TSize bitmapSize = iFrameInfo.iOverallSizeInPixels;

	// Calculate scale factor,
	// if required
	if (bitmapSize.iWidth > KImageWidthThreshold || bitmapSize.iHeight > KImageHeightThreshold)
	{
		TUint hScaleFactor = bitmapSize.iWidth/KImageWidthThreshold;
		TUint vScaleFactor = bitmapSize.iHeight/KImageHeightThreshold;
		// apply the same scale factor to both horizontal and vertical
		TUint scaleFactor = 1;
		if (hScaleFactor <= vScaleFactor)
			scaleFactor = hScaleFactor;
		else
			scaleFactor = vScaleFactor;

		if (scaleFactor >= 4)
			scaleFactor = 4;
		else if (scaleFactor >= 2)
			scaleFactor = 2;
		else
			scaleFactor = 1;

		// we need a correction pixel on each coordinate if the size is not divisible with scale factor; the
		// decoder will not work otherwise
		TUint hCorrection = 0;
		if (bitmapSize.iWidth % scaleFactor)
			hCorrection = 1;
		TUint vCorrection = 0;
		if (bitmapSize.iHeight % scaleFactor)
			vCorrection = 1;
		// the magic formula for calculating the final size
		bitmapSize.SetSize(bitmapSize.iWidth/scaleFactor + hCorrection, bitmapSize.iHeight/scaleFactor + vCorrection);

	}

	//iBitmap.Resize(bitmapSize.Size());
	iBitmap.Resize(bitmapSize);
	// start reading the bitmap: RunL called when complete
	iLoadUtil->Convert(&iStatus, iBitmap, aSelectedFrame);
	SetActive();
	}

void CSupImageHandler::SaveImageToFileL(const TFileName& aFileName, const TUid& aImageType, const TUid& aImageSubType)
{
	__ASSERT_ALWAYS(!IsActive(),User::Invariant());
	// create a CImageEncoder to save the bitmap to the specified file in the specified format
	delete iSaveUtil;
	iSaveUtil = NULL;
	iSaveUtil = CImageEncoder::FileNewL(iFs, aFileName, CImageEncoder::EOptionNone, aImageType, aImageSubType);
	// start saving the bitmap: RunL called when complete
	iSaveUtil->Convert(&iStatus, iBitmap);
	SetActive();
}

void CSupImageHandler::LoadBufferL(TDesC8 &aSourceData, TInt aSelectedFrame /* = 0 */)
{
	__ASSERT_ALWAYS(!IsActive(),User::Invariant());
	// create a CImageDecoder to read the specified file
	delete iLoadUtil;
	iLoadUtil = NULL;

	iLoadUtil = CImageDecoder::DataNewL(iFs, aSourceData);
	// store the frame information and frame count
	iFrameInfo = iLoadUtil->FrameInfo(aSelectedFrame);
	iFrameCount = iLoadUtil->FrameCount();

	// resize the destination bitmap to fit the required size
	TRect bitmapSize = iFrameInfo.iFrameCoordsInPixels;
	iBitmap.Resize(bitmapSize.Size());

	// start reading the bitmap: RunL called when complete
	iLoadUtil->Convert(&iStatus, iBitmap, aSelectedFrame);
	SetActive();
}

void CSupImageHandler::SaveImageToBufferL(HBufC8*& aDestinationData, const TDesC8 &aMIMEType)
	{
	__ASSERT_ALWAYS(!IsActive(), User::Invariant());
	// create a CImageEncoder to save the bitmap to the specified descriptor buffer
	delete iSaveUtil;
	iSaveUtil = NULL;
	iSaveUtil = CImageEncoder::DataNewL(aDestinationData, aMIMEType, CImageEncoder::EOptionNone);
	// start saving the bitmap: RunL called when complete
	iSaveUtil->Convert(&iStatus, iBitmap);
	SetActive();
	}

void CSupImageHandler::Mirror()
	{
	__ASSERT_ALWAYS(!IsActive(),User::Invariant());
	// start rotating the bitmap: RunL called when complete
	iBitmapRotator->Rotate(&iStatus, iBitmap, CBitmapRotator::EMirrorVerticalAxis);
	SetActive();
	}

void CSupImageHandler::Flip()
	{
	__ASSERT_ALWAYS(!IsActive(),User::Invariant());
	// start rotating the bitmap: RunL called when complete
	iBitmapRotator->Rotate(&iStatus, iBitmap, CBitmapRotator::EMirrorHorizontalAxis);
	SetActive();
	}

void CSupImageHandler::FrameRotate(TBool aClockwise)
	{
	__ASSERT_ALWAYS(!IsActive(),User::Invariant());
	// start rotating the bitmap: RunL called when complete
	if (aClockwise)
		iBitmapRotator->Rotate(&iStatus, iBitmap, CBitmapRotator::ERotation90DegreesClockwise);
	else
		iBitmapRotator->Rotate(&iStatus, iBitmap, CBitmapRotator::ERotation270DegreesClockwise);
	SetActive();
	}

void CSupImageHandler::ZoomFrame(TBool aZoomIn)
	{
	__ASSERT_ALWAYS(!IsActive(),User::Invariant());
	// Determine target zooming size
	TSize size(iBitmap.SizeInPixels());
	const TSize adjust(size.iWidth/2, size.iHeight/2);
	if (aZoomIn)
		size += adjust;
	else
		size -= adjust;
	// Don't let it go too small
	if (size.iWidth <= 10) size.iWidth = 10;
	if (size.iHeight <= 10) size.iHeight = 10;

	// start scaling the bitmap: RunL called when complete
	iBitmapScaler->Scale(&iStatus, iBitmap, size);
	SetActive();
	}

void CSupImageHandler::FitToThresholds()
	{
		// Determine image size
		TSize size(iBitmap.SizeInPixels());

		// If image size exceeds any of thresholds,
		// zoom it in
		if (size.iWidth > KImageWidthThreshold || size.iHeight > KImageHeightThreshold)
		{
			// Defines scale value
			TInt zoomValue = 1;
			
			// If image ratio equals with the target ratio (i.e. width to height relation),
			// then just set zoomValue == size.iWidth/KImageWidthThreshold
			if (size.iWidth/size.iHeight == KImageWidthThreshold/KImageHeightThreshold)
			{
				zoomValue = size.iWidth/KImageWidthThreshold;
			}
			else if (size.iWidth > size.iHeight)	
			{
				zoomValue = size.iWidth/KImageWidthThreshold;
			}
			else if (size.iWidth < size.iHeight)
			{
				zoomValue = size.iHeight/KImageHeightThreshold;
			}

			// Set new size
			const TSize adjust(size.iWidth/zoomValue, size.iHeight/zoomValue);

			// Set medium quality algorithm
			iBitmapScaler->SetQualityAlgorithm(CBitmapScaler::EMediumQuality);

			// start scaling the bitmap: RunL called when complete
			iBitmapScaler->Scale(&iStatus, iBitmap, adjust);
			SetActive();			
		}
	}

TBool CSupImageHandler::ExceedsThresholds()
{
	TSize size(iBitmap.SizeInPixels());

	if (size.iWidth > KImageWidthThreshold || size.iHeight > KImageHeightThreshold)
		return ETrue;

	return EFalse;
}

void CSupImageHandler::RunL()
	{
	// Operation complete, call back caller
	iCallback.HandleRequestCompletedL(iStatus.Int());
	}
	 
void CSupImageHandler::DoCancel()
	{
	// Cancel whatever is possible
	if (iLoadUtil) iLoadUtil->Cancel();
	if (iSaveUtil) iSaveUtil->Cancel();
	iBitmapRotator->Cancel();
	iBitmapScaler->Cancel(); 
	}
