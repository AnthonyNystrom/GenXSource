/*
============================================================================
Name        : Aafsplashscreenview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Splash screen view class
============================================================================
*/

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafSplashScreenView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//

#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikspane.h>
#include <w32std.h>
#include "Aafsplashscreenctrl.h"
#include "Aafsplashscreenview.h"
#include "common.h"

CAafSplashScreenView* CAafSplashScreenView::NewL( const TRect& aRect )
{
	CAafSplashScreenView* self = CAafSplashScreenView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafSplashScreenView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafSplashScreenView* CAafSplashScreenView::NewLC( const TRect& aRect )
{
	CAafSplashScreenView* self = new ( ELeave ) CAafSplashScreenView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafSplashScreenView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafSplashScreenView::ConstructL( const TRect& aRect )
{
	// Create a window for this application view
	CreateWindowL();	

	// Creating splash screen control
	iSplashScreen = CAafSplashScreen::NewL();
	iSplashScreen->SetContainerWindowL(*this);

	iSplashScreen->ActivateL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();
}

// -----------------------------------------------------------------------------
// CAafSplashScreenView::CAafSplashScreenView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafSplashScreenView::CAafSplashScreenView()
{
	iSplashScreen = NULL;
}

// -----------------------------------------------------------------------------
// CAafSplashScreenView::~CAafSplashScreenView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafSplashScreenView::~CAafSplashScreenView()
{
	if (iSplashScreen)
	{
		delete iSplashScreen;
		iSplashScreen = NULL;
	}
}


// -----------------------------------------------------------------------------
// CAafSplashScreenView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafSplashScreenView::Draw( const TRect& /*aRect*/ ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();


	// Clears the screen
	gc.Clear( Rect() );

}

// -----------------------------------------------------------------------------
// CAafSplashScreenView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafSplashScreenView::SizeChanged()
{  
	DrawDeferred();
}


TInt CAafSplashScreenView::CountComponentControls() const
{
	return 1;
}

CCoeControl* CAafSplashScreenView::ComponentControl(TInt /*aIndex*/) const
{
	return iSplashScreen;
}

void CAafSplashScreenView::HandleResourceChange(TInt aType)
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
TVwsViewId CAafSplashScreenView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KSplashScreenViewId);
}

// Activates this view, called by framework
void CAafSplashScreenView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
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
void CAafSplashScreenView::ViewDeactivated()
{
	MakeVisible(EFalse);
}

// End of file