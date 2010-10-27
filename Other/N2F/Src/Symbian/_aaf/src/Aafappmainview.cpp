/*
============================================================================
Name        : Aafappmainview.cpp
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
#include <ecam.h>
#include <akniconarray.h>
#include "AafAppUi.h"
#include "AafAppMainView.h"
#include "aaf.rsg"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppMainView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppMainView* CAafAppMainView::NewL( const TRect& aRect )
{
	CAafAppMainView* self = CAafAppMainView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppMainView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppMainView* CAafAppMainView::NewLC( const TRect& aRect )
{
	CAafAppMainView* self = new ( ELeave ) CAafAppMainView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppMainView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppMainView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppMainView::ConstructL() begins");

	CreateWindowL();

#if USE_MAINVIEW_LARGE_ICONS
	iListBox = new (ELeave)CAknSingleLargeStyleListBox;
#else
	iListBox = new (ELeave)CAknSingleGraphicStyleListBox;
#endif

	iListBox->SetContainerWindowL( *this );

	// read resource that defines listbox
	TResourceReader reader; 

	// If no on-board camera is present
	// we wouldn't include item "Make a photo"
#if USE_CAMERA
	if (CCamera::CamerasAvailable() == 0)
	{
		CEikonEnv::Static()->CreateResourceReaderLC(reader, R_MAINVIEW_LISTBOX_WITHOUT_CAMERA); 
	}
	else
	{
		CEikonEnv::Static()->CreateResourceReaderLC(reader, R_MAINVIEW_LISTBOX); 
	}
#else
	CEikonEnv::Static()->CreateResourceReaderLC(reader, R_MAINVIEW_LISTBOX_WITHOUT_CAMERA); 
#endif

	iListBox->ConstructFromResourceL(reader); 

	CleanupStack::PopAndDestroy(); // for resource reader

	// Creates a GUI icon array
	CAknIconArray* icons = NULL;
	
#if USE_CAMERA
	// If no on-board camera is present
	if (CCamera::CamerasAvailable() == 0)
	{
		icons = new (ELeave)CAknIconArray(7);
		CleanupStack::PushL(icons);
		icons->ConstructFromResourceL(R_MAINVIEW_ICONS_WITHOUT_CAMERA);	
	}
	// Otherwise
	else
	{
		icons = new (ELeave)CAknIconArray(8);
		CleanupStack::PushL(icons);
		icons->ConstructFromResourceL(R_MAINVIEW_ICONS);		
	}
#else
	icons = new (ELeave)CAknIconArray(7);
	CleanupStack::PushL(icons);
	icons->ConstructFromResourceL(R_MAINVIEW_ICONS_WITHOUT_CAMERA);
#endif

	// Sets graphics as ListBox icon.
	iListBox->ItemDrawer()->ColumnData()->SetIconArray(icons);		
	
	CleanupStack::Pop(); // icons

	// Enable marquee effect
	iListBox->ItemDrawer()->ColumnData()->SetMarqueeParams(3, 20, 1000000, 200000);
	iListBox->ItemDrawer()->ColumnData()->EnableMarqueeL(ETrue);

	// Create the scroll indicator
	iListBox->CreateScrollBarFrameL(ETrue);
	iListBox->ScrollBarFrame()->SetScrollBarVisibilityL( CEikScrollBarFrame::EOn, CEikScrollBarFrame::EAuto);

	iListBox->Model()->SetOwnershipType( ELbmOwnsItemArray );

	iListBox->ActivateL();

	// set container class as an observer of the listbox 

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppMainView::ConstructL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppMainView::CAafAppMainView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppMainView::CAafAppMainView()
{
	// No implementation required
}


// -----------------------------------------------------------------------------
// CAafAppMainView::~CAafAppMainView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppMainView::~CAafAppMainView()
{
	if (iListBox)
	{
		delete iListBox;
		iListBox = NULL;
	}
}


// -----------------------------------------------------------------------------
// CAafAppMainView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppMainView::Draw( const TRect& aRect ) const
{
	CWindowGc& gc = SystemGc();
	// TODO: Add your drawing code here
	// example code...
	gc.SetPenStyle( CGraphicsContext::ENullPen );
	gc.SetBrushColor( KRgbWhite );
	gc.SetBrushStyle( CGraphicsContext::ESolidBrush );
	gc.DrawRect( aRect );
}

// -----------------------------------------------------------------------------
// CAafAppMainView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppMainView::SizeChanged()
{  
	// container control resize code. 
	if (iListBox) 
	{ 
		/*
		CAafAppUi* appUi = (CAafAppUi*)iCoeEnv->AppUi();
		TRect clientRect(appUi->ClientRect());

		iListBox->SetRect(clientRect);
		iListBox->DrawNow();
		*/
		TRect clientRect = Rect();
		iListBox->SetExtent(TPoint(0, 0), clientRect.Size()); // Set rectangle
		iListBox->DrawNow();
	} 
}

TInt CAafAppMainView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CAafAppMainView::ComponentControl(TInt aIndex)  const 
{ 
	return iListBox;
}

TKeyResponse CAafAppMainView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
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
			CAafAppUi* appUi = (CAafAppUi*)iCoeEnv->AppUi();
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

void CAafAppMainView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CAafAppMainView::HandleControlEventL(CCoeControl* /*aControl*/, TCoeEvent /*aEventType*/)
{
	// Should be added later
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CAafAppMainView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KMainViewId);
}

// Called from CAafAppUi
TInt CAafAppMainView::GetKeyEventCode() const
{
	TInt retValue;

	TInt camerasCount = CCamera::CamerasAvailable();

#if USE_CAMERA
	// If no on-board camera is present
	if (camerasCount == 0)
	{
		switch ( iListBox->CurrentItemIndex() )
		{
		case 0:
			{
				retValue = EAskQuestion;
			}
			break;
		case 1:
			{
				retValue = EFileBrowserView;
			}
			break;
		case 2:
			{
				retValue = EImageCartView;
			}
			break;
		case 3:
			{
				retValue = EPrivateQuestions;
			}
			break;
		case 4:
			{
				retValue = EResponses;
			}
			break;
		case 5: 
			{
				retValue = ECredentialsView;
			}
			break;
		case 6:
			{
				retValue = EInviteFriend;
			}
			break;
		case 7:
			{
				retValue = EAknSoftkeyExit;
			}
			break;

		default:
			// Do default handling here…
			break;
		}
	}
	// Otherwise
	else
	{
		switch ( iListBox->CurrentItemIndex() )
		{
		case 0:
			{
				retValue = EAskQuestion;
			}
			break;
		case 1:
			{
				retValue = EMakePhoto;
			}
			break;
		case 2:
			{
				retValue = EFileBrowserView;
			}
			break;
		case 3:
			{
				retValue = EImageCartView;
			}
			break;
		case 4:
			{
				retValue = EPrivateQuestions;
			}
			break;
		case 5:
			{
				retValue = EResponses;
			}
			break;
		case 6: 
			{
				retValue = ECredentialsView;
			}
			break;
		case 7:
			{
				retValue = EInviteFriend;
			}
			break;
		case 8:
			{
				retValue = EAknSoftkeyExit;
			}
			break;

		default:
			// Do default handling here…
			break;
		}
	}
#else
	switch ( iListBox->CurrentItemIndex() )
	{
	case 0:
		{
			retValue = EAskQuestion;
		}
		break;
	case 1:
		{
			retValue = EFileBrowserView;
		}
		break;
	case 2:
		{
			retValue = EImageCartView;
		}
		break;
	case 3:
		{
			retValue = EPrivateQuestions;
		}
		break;
	case 4:
		{
			retValue = EResponses;
		}
		break;
	case 5: 
		{
			retValue = ECredentialsView;
		}
		break;
	case 6:
		{
			retValue = EInviteFriend;
		}
		break;
	case 7:
		{
			retValue = EAknSoftkeyExit;
		}
		break;

	default:
		// Do default handling here…
		break;
	}
#endif
	

	return retValue;
}

// Activates this view, called by framework
void CAafAppMainView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
{
	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();


	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	CEikButtonGroupContainer* cba = appUiFactory->Cba();

	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	menuBar->SetMenuTitleResourceId(R_AAF_MAINVIEW_MENUBAR);

	cba->SetCommandSetL(R_AAF_CBA_MAINVIEW);
	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);
	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CAafAppMainView::ViewDeactivated()
{
	MakeVisible(EFalse);
}
// End of File
