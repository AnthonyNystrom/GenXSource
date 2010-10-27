/*
============================================================================
 Name        : Supappuploadimagesview.cpp
 Author      : Next2Friends
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Upload images view
============================================================================
*/

// INCLUDE FILES
#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikspane.h>
#include "SupAppUploadImagesView.h"
#include "common.h"
#include "sup.rsg"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CSupAppUploadImagesView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppUploadImagesView* CSupAppUploadImagesView::NewL( const TRect& aRect )
	{
	CSupAppUploadImagesView* self = CSupAppUploadImagesView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
	}

// -----------------------------------------------------------------------------
// CSupAppUploadImagesView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppUploadImagesView* CSupAppUploadImagesView::NewLC( const TRect& aRect )
	{
	CSupAppUploadImagesView* self = new ( ELeave ) CSupAppUploadImagesView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
	}

// -----------------------------------------------------------------------------
// CSupAppUploadImagesView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CSupAppUploadImagesView::ConstructL( const TRect& aRect )
	{
	// Create a window for this application view
	CreateWindowL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();
	}

// -----------------------------------------------------------------------------
// CSupAppUploadImagesView::CSupAppUploadImagesView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CSupAppUploadImagesView::CSupAppUploadImagesView()
	{
	// No implementation required
	}


// -----------------------------------------------------------------------------
// CSupAppUploadImagesView::~CSupAppUploadImagesView()
// Destructor.
// -----------------------------------------------------------------------------
//
CSupAppUploadImagesView::~CSupAppUploadImagesView()
	{
	// No implementation required
	}


// -----------------------------------------------------------------------------
// CSupAppUploadImagesView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CSupAppUploadImagesView::Draw( const TRect& /*aRect*/ ) const
	{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );
	
	}

// -----------------------------------------------------------------------------
// CSupAppUploadImagesView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CSupAppUploadImagesView::SizeChanged()
	{  
	DrawNow();
	}

void CSupAppUploadImagesView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
      SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CSupAppUploadImagesView::ViewId() const
{
   return TVwsViewId(KUidViewSupApp, KUploadImagesViewId);
}

// Activates this view, called by framework
void CSupAppUploadImagesView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &aCustomMessage)
{
	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();

	// Setting usual application status pane - if necessary
	//CEikStatusPane* statusPane = appUiFactory->StatusPane();
	//if(statusPane->CurrentLayoutResId() != R_AVKON_STATUS_PANE_LAYOUT_USUAL)
	//{
	//	statusPane->SwitchLayoutL(R_AVKON_STATUS_PANE_LAYOUT_USUAL);
	//}

	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	CEikButtonGroupContainer* cba = appUiFactory->Cba();
  
	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	menuBar->SetMenuTitleResourceId(R_SUP_UPLOAD_IMAGES_MENUBAR);

	cba->SetCommandSetL(R_SUP_CBA_STANDART);
	cba->DrawDeferred();
  
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
void CSupAppUploadImagesView::ViewDeactivated()
{
	MakeVisible(EFalse);
}
// End of File
