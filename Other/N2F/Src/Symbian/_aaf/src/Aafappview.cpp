/*
============================================================================
 Name        : AafView.cpp
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Application view
============================================================================
*/

// INCLUDE FILES
#include <coemain.h>
#include "AafAppView.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppView* CAafAppView::NewL( const TRect& aRect )
	{
	CAafAppView* self = CAafAppView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
	}

// -----------------------------------------------------------------------------
// CAafAppView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppView* CAafAppView::NewLC( const TRect& aRect )
	{
	CAafAppView* self = new ( ELeave ) CAafAppView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
	}

// -----------------------------------------------------------------------------
// CAafAppView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppView::ConstructL( const TRect& aRect )
	{
	// Create a window for this application view
	CreateWindowL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();
	}

// -----------------------------------------------------------------------------
// CAafAppView::CAafAppView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppView::CAafAppView()
	{
	// No implementation required
	}


// -----------------------------------------------------------------------------
// CAafAppView::~CAafAppView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppView::~CAafAppView()
	{
	// No implementation required
	}


// -----------------------------------------------------------------------------
// CAafAppView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppView::Draw( const TRect& /*aRect*/ ) const
	{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );
	
	}

// -----------------------------------------------------------------------------
// CAafAppView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppView::SizeChanged()
	{  
	DrawNow();
	}
// End of File
