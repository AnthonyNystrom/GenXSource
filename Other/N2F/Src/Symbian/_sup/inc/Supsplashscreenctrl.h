/*
============================================================================
Name        : Supsplashscreen.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Sup application splash screen screen declaration
============================================================================
*/

#ifndef __SUPSPLASHSCREENCTRL_H__
#define __SUPSPLASHSCREENCTRL_H__

#include <coecntrl.h>       // CCoeControl
#include "common.h"

// FORWARD DECLARATION
class CFbsBitmap;

//-------------------------------------------------------------------
//
//-------------------------------------------------------------------
class CSupSplashScreen : public CCoeControl
{
public:
	static CSupSplashScreen* NewL(void);

	~CSupSplashScreen();

public: // From MImageLoaderObserver
	//virtual void ImageLoadingDone(CFbsBitmap *aBitmap, CFbsBitmap *aMask = NULL);

	//virtual void ImageLoadingFailed(TInt aError);


private:
	void ConstructL(void);

	void Draw(const TRect& aRect) const;

private:
	CFbsBitmap*	iBackgroundPic;

	//CSupImageReader *iImageReader;
	//CImageLoader* iImageReader;

	//TBool iImageReady;

	//void HandleRequestCompletedL(const TInt& aError);
};

#endif // __SUPSPLASHSREEN_H__

// End of file