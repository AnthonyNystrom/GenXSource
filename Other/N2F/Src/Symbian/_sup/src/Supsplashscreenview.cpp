/*
============================================================================
Name        : Supsplashscreenview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Splash screen view class
============================================================================
*/

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CSupSplashScreenView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//

#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikspane.h>
#include <w32std.h>
#include "Supsplashscreenctrl.h"
#include "Supsplashscreenview.h"
#include "common.h"

CSupSplashScreenView* CSupSplashScreenView::NewL( const TRect& aRect )
{
	CSupSplashScreenView* self = CSupSplashScreenView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CSupSplashScreenView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupSplashScreenView* CSupSplashScreenView::NewLC( const TRect& aRect )
{
	CSupSplashScreenView* self = new ( ELeave ) CSupSplashScreenView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CSupSplashScreenView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CSupSplashScreenView::ConstructL( const TRect& aRect )
{
	// Create a window for this application view
	CreateWindowL();	

	// Creating splash screen control
	iSplashScreen = CSupSplashScreen::NewL();
	iSplashScreen->SetContainerWindowL(*this);

	iSplashScreen->ActivateL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();
}

// -----------------------------------------------------------------------------
// CSupSplashScreenView::CSupSplashScreenView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CSupSplashScreenView::CSupSplashScreenView()
{
	iSplashScreen = NULL;
}

// -----------------------------------------------------------------------------
// CSupSplashScreenView::~CSupSplashScreenView()
// Destructor.
// -----------------------------------------------------------------------------
//
CSupSplashScreenView::~CSupSplashScreenView()
{
	if (iSplashScreen)
	{
		delete iSplashScreen;
		iSplashScreen = NULL;
	}
}


// -----------------------------------------------------------------------------
// CSupSplashScreenView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CSupSplashScreenView::Draw( const TRect& /*aRect*/ ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();


	// Clears the screen
	gc.Clear( Rect() );

}

// -----------------------------------------------------------------------------
// CSupSplashScreenView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CSupSplashScreenView::SizeChanged()
{  
	DrawDeferred();
}


TInt CSupSplashScreenView::CountComponentControls() const
{
	return 1;
}

CCoeControl* CSupSplashScreenView::ComponentControl(TInt /*aIndex*/) const
{
	return iSplashScreen;
}

void CSupSplashScreenView::HandleResourceChange(TInt aType)
{
	CCoeControl::HandleResourceChange(aType);

	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SetExtentToWholeScreen();
		//CWsScreenDevice& iScreen = *(iCoeEnv->ScreenDevice());
		//TSize originalRes = iScreen.SizeInPixels();
		//SetRect(TRect(TPoint(0, 0), originalRes));
	}	
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CSupSplashScreenView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KSplashScreenViewId);
}

// Activates this view, called by framework
void CSupSplashScreenView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
{  
	/*
	if (aCustomMessage.Length() > 0)
	{
	if (iViewParam)
	{
	delete iViewParam;
	iViewParam = NULL;
	}
	iViewParam = HBufC::NewL(aCustomMessage.Length());
	iViewParam->Des().Copy(aCustomMessage);
	}
	*/

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);
	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CSupSplashScreenView::ViewDeactivated()
{
	MakeVisible(EFalse);
}

// End of file