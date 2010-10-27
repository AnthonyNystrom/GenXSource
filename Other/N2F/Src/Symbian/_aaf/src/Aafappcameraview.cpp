/*
============================================================================
Name        : Aafcameraview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Camera view implementation
============================================================================
*/
#include "Aafappcameraview.h"

#if USE_CAMERA

// INCLUDE FILES
#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikspane.h>
#include <barsread.h>

#include <caknmemoryselectiondialog.h>
#include <caknfileselectiondialog.h>
#include <pathinfo.h>
#include <aknquerydialog.h> 

#include "Aafappui.h"

#if USE_SKIN_SUPPORT
#include <AknsDrawUtils.h>
#include <AknsBasicBackgroundControlContext.h>
#endif

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppCameraView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppCameraView* CAafAppCameraView::NewL( const TRect& aRect )
{
	CAafAppCameraView* self = CAafAppCameraView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppCameraView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppCameraView* CAafAppCameraView::NewLC( const TRect& aRect )
{
	CAafAppCameraView* self = new ( ELeave ) CAafAppCameraView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppCameraView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppCameraView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppCameraView::ConstructL() begins");

	// Create a window for this application view
	CreateWindowL();

#if USE_SKIN_SUPPORT
	iBgContext = CAknsBasicBackgroundControlContext::NewL( KAknsIIDQsnBgAreaMain, aRect, ETrue);
#endif

	// Set the windows size
	SetRect( aRect );	

	__LOGSTR_TOFILE("CAafAppCameraView::ConstructL() before ActivateL()");

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppCameraView::ConstructL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppCameraView::CAafAppCameraView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppCameraView::CAafAppCameraView()
{
	iContainer = NULL;
}


// -----------------------------------------------------------------------------
// CAafAppCameraView::~CAafAppCameraView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppCameraView::~CAafAppCameraView()
{
	// Free dynamically allocated memory
	if (iContainer)
	{
		delete iContainer;
		iContainer = NULL;
	}

#if USE_SKIN_SUPPORT
	if (iBgContext)
	{
		delete iBgContext;
		iBgContext = NULL;
	}
#endif
}

EPhotoResolution CAafAppCameraView::GetCurrentResolution()
{
	if (iContainer)
	{
		return iContainer->GetCurrentResolution();
	}

	return KRes640x480;
}

void CAafAppCameraView::SetResolution(const EPhotoResolution &aNewResolution)
{
	if (iContainer)
	{
		iContainer->SetResolution(aNewResolution);
	}
}

TBool CAafAppCameraView::IsImageCaptured()
{
	if (iContainer)
	{
		return iContainer->IsImageCaptured();
	}

	return EFalse;
}

void CAafAppCameraView::CaptureImageL()
{
	if (iContainer)
	{
		iContainer->CaptureL();
	}
}

void CAafAppCameraView::SaveCapturedImageL()
{
	if (IsImageCaptured())
	{
		TFileName filePath(KNullDesC);
		
		// Create select memory dialog
		CAknMemorySelectionDialog* memDlg = 
			CAknMemorySelectionDialog::NewL(ECFDDialogTypeSave, ETrue);
		CAknMemorySelectionDialog::TMemory memory = 
			CAknMemorySelectionDialog::EPhoneMemory;

		// Create select folder dialog
		CAknFileSelectionDialog* dlg = 
			CAknFileSelectionDialog::NewL(ECFDDialogTypeCopy);

		// some dialog customizations:
		CEikonEnv* eikonEnv = CEikonEnv::Static();

		HBufC* stringHolder = StringLoader::LoadL(R_SELECT_FOLDER_DIALOG, eikonEnv );

		dlg->SetTitleL(*stringHolder);

		// Free dynamically allocated memory
		delete stringHolder;
		stringHolder = NULL;

		stringHolder = StringLoader::LoadL(R_BACK, eikonEnv );

		dlg->SetRightSoftkeyRootFolderL(*stringHolder); // for root folder

		// Free dynamically allocated memory
		delete stringHolder;
		stringHolder = NULL;

		TBool result = EFalse;

		for (;;)
		{
			if ( memDlg->ExecuteL(memory) == CAknFileSelectionDialog::ERightSoftkey )
			{
				// cancel selection
				break;
			}

			if (memory == CAknMemorySelectionDialog::EMemoryCard)
			{
				// Open images folder
				filePath = PathInfo::MemoryCardRootPath();
				filePath.Append(PathInfo::ImagesPath());
			}
			else
			{
				// Open images folder
				filePath = PathInfo::PhoneMemoryRootPath();
				filePath.Append(PathInfo::ImagesPath());
			}

			
			if (dlg->ExecuteL(filePath))
			{
				// we got our folder and finish loop
				result = ETrue;
				break;
			}
		}

		delete memDlg;
		delete dlg;

		if (filePath.Length() > 0)
		{
			// The descriptor used for the editor 
			TBuf<255> fileName; 
			// create dialog instance  
			CAknTextQueryDialog* dlgFilename = new( ELeave )CAknTextQueryDialog( fileName); 
			// Prepares the dialog, constructing it from the specified resource 
			dlgFilename->PrepareLC( R_FILENAME_QUERY ); 
			// Sets the maximum length of the text editor 
			dlgFilename->SetMaxLength(255); 
			// Launch the dialog 
			if (dlgFilename->RunLD()) 
			{ 
				// ok pressed,  text is the descriptor containing the entered text 
				// in the editor.
				filePath.Append(fileName);

				// ensure that saved file would have .jpg extension
				if (filePath.Right(4).Compare(KJpgExt) != 0)
					filePath.Append(KJpgExt);

				iContainer->SaveImage(filePath);
			} 
		}
	}	
}

void CAafAppCameraView::StartViewFinderL()
{
	__LOGSTR_TOFILE("CAafAppCameraView::StartViewFinderL() begins");

	if (iContainer)
	{
		iContainer->StartViewFinderL();
	}

	__LOGSTR_TOFILE("CAafAppCameraView::StartViewFinderL() ends");
}

TInt CAafAppCameraView::GetZoomValue()
{
	if (iContainer)
	{
		return iContainer->GetZoomValue();
	}

	return 0;
}

TInt CAafAppCameraView::GetMaxZoomValue()
{
	if (iContainer)
	{
		return iContainer->GetMaxZoomValue();
	}

	return 0;
}

void CAafAppCameraView::SetZoomValue(const TInt &aNewZoom)
{
	if (iContainer)
	{
		iContainer->SetZoomValue(aNewZoom);
	}
}

void CAafAppCameraView::ZoomIn()
{
	TInt zoomValue = GetZoomValue()+1;

	SetZoomValue(zoomValue);
}

void CAafAppCameraView::ZoomOut()
{
	TInt zoomValue = GetZoomValue()-1;

	SetZoomValue(zoomValue);
}

// -----------------------------------------------------------------------------
// CAafAppCameraView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppCameraView::Draw( const TRect& aRect ) const
{
	CWindowGc& gc = SystemGc();

	// Draw view's background
#if USE_SKIN_SUPPORT
	// Redraw the background using the default skin
	MAknsSkinInstance* skin = AknsUtils::SkinInstance();
	MAknsControlContext* cc = AknsDrawUtils::ControlContext( this );
	AknsDrawUtils::Background( skin, cc, this, gc, aRect );
#else
	gc.SetPenStyle( CGraphicsContext::ENullPen );
	gc.SetBrushColor( KRgbWhite );
	gc.SetBrushStyle( CGraphicsContext::ESolidBrush );
	gc.DrawRect( aRect );
	gc.Clear(aRect);
#endif
}

#if USE_SKIN_SUPPORT
TTypeUid::Ptr CAafAppCameraView::MopSupplyObject(TTypeUid aId)
{
	if (iBgContext )
	{
		return MAknsControlContext::SupplyMopObject( aId, iBgContext );
	}
	return CCoeControl::MopSupplyObject(aId);
}
#endif

// -----------------------------------------------------------------------------
// CAafAppCameraView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppCameraView::SizeChanged()
{  
	__LOGSTR_TOFILE("CAafAppCameraView::SizeChanged() begins");

#if USE_SKIN_SUPPORT
	if(iBgContext)
	{
		iBgContext->SetRect(Rect());
		if ( &Window() )
		{
			iBgContext->SetParentPos( PositionRelativeToScreen() );
		}
	}
#endif

	if ( iContainer )
	{
		CAafAppUi* appUi = (CAafAppUi*)iCoeEnv->AppUi();
	
		iContainer->SetRect( appUi->ClientRect() );
	}

	__LOGSTR_TOFILE("CAafAppCameraView::SizeChanged() ends");
}

TInt CAafAppCameraView::CountComponentControls() const
{ 
	__LOGSTR_TOFILE("CAafAppCameraView::CountComponentControls() is called");
	if (!iContainer)
		return 0;

	return 1; // returns number of controls inside the container
}

CCoeControl* CAafAppCameraView::ComponentControl(TInt aIndex)  const 
{ 
	__LOGSTR_TOFILE("CAafAppCameraView::ComponentControl() is called");
	return iContainer;
} 

TKeyResponse CAafAppCameraView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
{
	if(aType == EEventKey)
	{
		switch(aKeyEvent.iCode) 
		{
		case EKeyOK:
			{
				if (iContainer)
				{
					if (iContainer->IsImageCaptured())
					{
						iContainer->StartViewFinderL();
					}
					else
					{
						iContainer->CaptureL();
					}
				}
			}
			break;
		case EStdKeyUpArrow:
			{				
				ZoomIn();			

				return EKeyWasConsumed;
			}				
		case EStdKeyDownArrow:
			{
				ZoomOut();

				return EKeyWasConsumed;
			}

			// If not arrow up or arrow down key has been pressed
		default:
			{
				
			}
			break;
		}
	}

	return EKeyWasNotConsumed;
}

void CAafAppCameraView::HandleResourceChange(TInt aType)
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
TVwsViewId CAafAppCameraView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KCameraViewId);
}

// Activates this view, called by framework
void CAafAppCameraView::ViewActivatedL(const TVwsViewId &aPrevViewId, TUid aCustomMessageId, const TDesC8 &aCustomMessage)
{
	__LOGSTR_TOFILE("CAafAppCameraView::ViewActivatedL() ends");

	if (!iContainer)
	{
		__LOGSTR_TOFILE("CAafAppCameraView::ViewActivatedL() iContainer initialization begins");

		iContainer = CAafCameraContainer::NewL( Rect() );
		iContainer->SetContainerWindowL( *this );
		
		iContainer->ActivateL();
		__LOGSTR_TOFILE("CAafAppCameraView::ViewActivatedL() iContainer initialization ends");
	}
	else
	{
		iContainer->ActivateL();
		iContainer->InitCameraL();
	}

	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();


	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	CEikButtonGroupContainer* cba = appUiFactory->Cba();

	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	menuBar->SetMenuTitleResourceId(R_AAF_CAMERA_VIEW_MENUBAR);

	cba->SetCommandSetL(R_AAF_CBA_STANDART);
	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);

	MakeVisible(ETrue);

	__LOGSTR_TOFILE("CAafAppCameraView::ViewActivatedL() ends");
}

// Deactivates this view, called by framework
void CAafAppCameraView::ViewDeactivated()
{
	iContainer->ReleaseCamera();

	MakeVisible(EFalse);
}

#endif // USE_CAMERA
// End of File