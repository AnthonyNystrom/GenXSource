/*
============================================================================
Name        : Supappmainview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Application main view
============================================================================
*/

// INCLUDE FILES
#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikspane.h>
#include <barsread.h>
#include <eikclbd.h>
#include <akniconarray.h>
#include "SupAppUi.h"
#include "SupAppMainView.h"
#include "Supimagehandler.h"
#include "Sup.hrh"
#include "sup.rsg"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CSupAppMainView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppMainView* CSupAppMainView::NewL( const TRect& aRect )
{
	CSupAppMainView* self = CSupAppMainView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CSupAppMainView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppMainView* CSupAppMainView::NewLC( const TRect& aRect )
{
	CSupAppMainView* self = new ( ELeave ) CSupAppMainView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CSupAppMainView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CSupAppMainView::ConstructL( const TRect& aRect )
{
	CreateWindowL();

#if USE_MAINVIEW_LARGE_ICONS
	iListBox = new (ELeave)CAknSingleLargeStyleListBox;
#else
	iListBox = new (ELeave)CAknSingleGraphicStyleListBox;
#endif

	iListBox->SetContainerWindowL( *this );

	
	// read resource that defines listbox
	TResourceReader reader; 
	CEikonEnv::Static()->CreateResourceReaderLC(reader, R_MAINVIEW_LISTBOX); 
	iListBox->ConstructFromResourceL(reader); 	

	// Creates a GUI icon array
	CAknIconArray* icons = new (ELeave)CAknIconArray(3);
	CleanupStack::PushL(icons);
	icons->ConstructFromResourceL(R_MAINVIEW_ICONS);
	// Sets graphics as ListBox icon
	iListBox->ItemDrawer()->ColumnData()->SetIconArray(icons);

	CleanupStack::Pop(); // icons

	CleanupStack::PopAndDestroy(); // for resource reader

	// Enable marquee effect
	iListBox->ItemDrawer()->ColumnData()->SetMarqueeParams(3, 20, 1000000, 200000);
	iListBox->ItemDrawer()->ColumnData()->EnableMarqueeL(ETrue);

	// Create the scroll indicator
	iListBox->CreateScrollBarFrameL(ETrue);
	iListBox->ScrollBarFrame()->SetScrollBarVisibilityL( CEikScrollBarFrame::EOn, CEikScrollBarFrame::EAuto);

	iListBox->Model()->SetOwnershipType( ELbmOwnsItemArray );

	iListBox->ActivateL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();
}

// -----------------------------------------------------------------------------
// CSupAppMainView::CSupAppMainView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CSupAppMainView::CSupAppMainView()
{
	// No implementation required
}


// -----------------------------------------------------------------------------
// CSupAppMainView::~CSupAppMainView()
// Destructor.
// -----------------------------------------------------------------------------
//
CSupAppMainView::~CSupAppMainView()
{
	if (iListBox)
	{
		delete iListBox;
		iListBox = NULL;
	}
}


// -----------------------------------------------------------------------------
// CSupAppMainView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CSupAppMainView::Draw( const TRect& aRect ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );
}

// -----------------------------------------------------------------------------
// CSupAppMainView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CSupAppMainView::SizeChanged()
{  
	// container control resize code. 
	if (iListBox) 
	{ 
		/*
		CSupAppUi* appUi = (CSupAppUi*)iCoeEnv->AppUi();
		TRect clientRect(appUi->ClientRect());

		iListBox->SetRect(clientRect);
		iListBox->DrawNow();
		*/
		TRect clientRect = Rect();
		iListBox->SetExtent(TPoint(0, 0), clientRect.Size()); // Set rectangle
		iListBox->DrawNow();
	} 
}

TInt CSupAppMainView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CSupAppMainView::ComponentControl(TInt aIndex)  const 
{ 
	switch ( aIndex ) 
	{ 
	case 0:
		return iListBox;
	default: 
		return NULL; 
	} 
}

TKeyResponse CSupAppMainView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
{
	// Default return value
	TKeyResponse ret = EKeyWasNotConsumed;

	// See if we have a selection
	switch(aKeyEvent.iCode)
	{
		// is navigator button pressed
	case EKeyOK:
		{
			// Redirect message handling to app ui class
			CSupAppUi* appUi = (CSupAppUi*)iCoeEnv->AppUi();
			appUi->HandleCommandL(GetKeyEventCode());

			ret = EKeyWasConsumed;
		}		
		break;

	default:
		// Let Listbox take care of its key handling
		{
			ret = iListBox->OfferKeyEventL(aKeyEvent, aType);
		}		
		break;
	}

	return ret;
}

void CSupAppMainView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CSupAppMainView::HandleControlEventL(CCoeControl* /*aControl*/, TCoeEvent /*aEventType*/)
{
	// Should be added later
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CSupAppMainView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KMainViewId);
}

// Called from CSupAppUi
TInt CSupAppMainView::GetKeyEventCode() const
{
	TInt retValue;

	switch ( iListBox->CurrentItemIndex() )
	{
	case 0:
		{
			retValue = EFileBrowserView;
		}
		break;
	case 1:
		{
			retValue = EImageCartView;
		}
		break;
	case 2: 
		{
			retValue = ECredentialsView;
		}
		break;
	case 3:
		{
			retValue = EAknSoftkeyExit;
		}
		break;

	default:
		// Do default handling here…
		break;
	}

	return retValue;
}

// Activates this view, called by framework
void CSupAppMainView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
{
	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();


	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	CEikButtonGroupContainer* cba = appUiFactory->Cba();

	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	menuBar->SetMenuTitleResourceId(R_SUP_MAINVIEW_MENUBAR);

	cba->SetCommandSetL(R_SUP_CBA_MAINVIEW);
	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);
	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CSupAppMainView::ViewDeactivated()
{
	MakeVisible(EFalse);
}
// End of File
