/*
============================================================================
Name        : Supsplashscreen.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Sup application splash screen class implementation
============================================================================
*/
#include <eikenv.h>
#include <eikappui.H>
#include <aknutils.h>
#include <akniconutils.h>
#include <eikapp.h>
#include "Supsplashscreenctrl.h"
#include "Supapputils.h"
#include "common.h"

// Screen width for the portrait 176x208 mode
#define SCREEN_WIDTH_1 176
#define SCREEN_WIDTH_1_2 208
// Screen width for the portrait 240x320 mode
#define SCREEN_WIDTH_2 240
#define SCREEN_WIDTH_2_2 320
// Screen width for the portrait 352x416 mode
#define SCREEN_WIDTH_3 352
#define SCREEN_WIDTH_3_2 416

CSupSplashScreen::~CSupSplashScreen()
{
	if (iBackgroundPic)
	{
		delete iBackgroundPic;
		iBackgroundPic = NULL;
	}
}   

CSupSplashScreen* CSupSplashScreen::NewL(void)
{
	CSupSplashScreen* self = new (ELeave)CSupSplashScreen();
	CleanupStack::PushL(self);
	self->ConstructL();
	CleanupStack::Pop(self);
	return self;
}

void CSupSplashScreen::ConstructL(void)
{

	CreateWindowL();

	TFindFile AppFile(CCoeEnv::Static()->FsSession());

	if(KErrNone == AppFile.FindByDir(KSupMbm, KNullDesC))
	{			
		iBackgroundPic = new (ELeave) CFbsBitmap();
		TRect clientRect = CEikonEnv::Static()->EikAppUi()->ApplicationRect();

		// Load commerce logo image depends on the current screen width
		switch(clientRect.Width())
		{
		case SCREEN_WIDTH_1:
		case SCREEN_WIDTH_1_2:
			{
				User::LeaveIfError(iBackgroundPic->Load(AppFile.File(), EMbmSupN2f_commercelogo_1));
			}
			break;
		case SCREEN_WIDTH_2:
		case SCREEN_WIDTH_2_2:
			{
				User::LeaveIfError(iBackgroundPic->Load(AppFile.File(), EMbmSupN2f_commercelogo_2));
			}
			break;
		case SCREEN_WIDTH_3:
		case SCREEN_WIDTH_3_2:
			{
				User::LeaveIfError(iBackgroundPic->Load(AppFile.File(), EMbmSupN2f_commercelogo_3));
			}
			break;
		default:
			{
				User::LeaveIfError(iBackgroundPic->Load(AppFile.File(), EMbmSupN2f_commercelogo_1));
			}
			break;
		}

		// Place image right in the screen center
		TSize iconSize = iBackgroundPic->SizeInPixels();

		// Set client rect
		TInt cX = clientRect.Width()/2 - iconSize.iWidth/2;
		TInt cY = clientRect.Height()/2 - iconSize.iHeight/2;

		// Set client rect == bitmaps rect
		SetRect(TRect(TPoint(cX, cY), iconSize));
	}
	else
	{

		SetRect(CEikonEnv::Static()->EikAppUi()->ApplicationRect());
	}

	ActivateL();
	DrawNow();
}

void CSupSplashScreen::Draw(const TRect& /*aRect*/) const
{
	CWindowGc& gc = SystemGc();

	gc.SetBrushColor(KRgbWhite);
	gc.SetBrushStyle(CGraphicsContext::ESolidBrush);
	gc.Clear(Rect());

	if(iBackgroundPic)
	{
		if(iBackgroundPic->Handle())
		{
			gc.DrawBitmap(Rect(), iBackgroundPic);
		}	
	}
}

/*
void CSupSplashScreen::HandleRequestCompletedL(const TInt& aError)
{
if(aError == KErrNone)
{
iImageReady = ETrue;
iBackgroundPic = iImageReader->Bitmap();
Draw(Rect());
}
}
*/

/*
void CSupSplashScreen::ImageLoadingDone(CFbsBitmap *aBitmap, CFbsBitmap* aMask = NULL)
{
iImageReady = ETrue;
iBackgroundPic = aBitmap;
Draw(Rect());
}

void CSupSplashScreen::ImageLoadingFailed(TInt aError)
{
iImageReady = EFalse;
iBackgroundPic = NULL;
}
*/