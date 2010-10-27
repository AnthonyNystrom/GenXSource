/*
============================================================================
Name        : Aafsplashscreen.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Aaf application splash screen screen declaration
============================================================================
*/

#ifndef __AAFSPLASHSCREENCTRL_H__
#define __AAFSPLASHSCREENCTRL_H__

#include <coecntrl.h>       // CCoeControl
#include "common.h"

// FORWARD DECLARATION
class CFbsBitmap;

//-------------------------------------------------------------------
//
//-------------------------------------------------------------------
class CAafSplashScreen : public CCoeControl
{
public:
	static CAafSplashScreen* NewL(void);

	~CAafSplashScreen();

public: // From MImageLoaderObserver
	//virtual void ImageLoadingDone(CFbsBitmap *aBitmap, CFbsBitmap *aMask = NULL);

	//virtual void ImageLoadingFailed(TInt aError);


private:
	void ConstructL(void);

	void Draw(const TRect& aRect) const;

private:
	CFbsBitmap*	iBackgroundPic;

	//CAafImageReader *iImageReader;
	//CImageLoader* iImageReader;

	//TBool iImageReady;

	//void HandleRequestCompletedL(const TInt& aError);
};

#endif // __AAFSPLASHSREEN_H__

// End of file