/*
============================================================================
Name        : Aafappfilebrowserview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : File browser view
============================================================================
*/

// INCLUDE FILES
#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikclbd.h>
#include <eikspane.h>
#include <akniconarray.h>
#include <aknmessagequerydialog.h>
#include "AafAppUi.h"
#include "AafAppFileBrowserView.h"
#include "Aaffilebrowserengine.h"
#include "Aafuploadserviceprovider.h"
#include "common.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppFileBrowserView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppFileBrowserView* CAafAppFileBrowserView::NewL( const TRect& aRect )
{
	CAafAppFileBrowserView* self = CAafAppFileBrowserView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppFileBrowserView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppFileBrowserView* CAafAppFileBrowserView::NewLC( const TRect& aRect )
{
	CAafAppFileBrowserView* self = new ( ELeave ) CAafAppFileBrowserView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppFileBrowserView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppFileBrowserView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppFileBrowserView::ConstructL() begins");

	// Create a window for this application view
	CreateWindowL();

	// Instantiate listbox control
	iListBox = new (ELeave)CAknSingleGraphicStyleListBox;

	iListBox->ConstructL( this, EAknListBoxSelectionList);

	iListBox->SetContainerWindowL( *this );

	__LOGSTR_TOFILE("CAafAppFileBrowserView::ConstructL() before setting icon array");

	// Creates a GUI icon array
	CAknIconArray* icons = new (ELeave)CAknIconArray(5);

	__LOGSTR_TOFILE("CAafAppFileBrowserView::ConstructL() before CleanupStack::PushL(icons)");

	CleanupStack::PushL(icons);
	icons->ConstructFromResourceL(R_BROWSERVIEW_ICONS);
	// Sets graphics as listbox icons
	iListBox->ItemDrawer()->ColumnData()->SetIconArray(icons);

	__LOGSTR_TOFILE("CAafAppFileBrowserView::ConstructL() before CleanupStack::Pop()");

	CleanupStack::Pop(); // icons

	// Enable marquee effect
	iListBox->ItemDrawer()->ColumnData()->SetMarqueeParams(3, 20, 1000000, 200000);
	iListBox->ItemDrawer()->ColumnData()->EnableMarqueeL(ETrue);

	__LOGSTR_TOFILE("CAafAppFileBrowserView::ConstructL() after setting icon array");

	// Create the scroll indicator
	iListBox->CreateScrollBarFrameL(ETrue);
	iListBox->ScrollBarFrame()->SetScrollBarVisibilityL( CEikScrollBarFrame::EOff, CEikScrollBarFrame::EAuto);

	iListBox->Model()->SetOwnershipType( ELbmOwnsItemArray );

	iListBox->ActivateL();

	// Create the FileBrowserEngine
	iBrowserEngine = new (ELeave) CFileBrowserEngine;

#ifdef __SERIES60_3X__
	iBrowserEngine->ConstructL();
#else
	iBrowserEngine->ConstructL((CEikProcess*)(((CEikAppUi*)iCoeEnv->AppUi())->Application()->Process()));
#endif

	// Set file browser to display only picture files
	//SetFileListL(EFileBrowserPictures, EFileBrowserDate);

	TFileName aFolder = TFileName(KNullDesC);
	SetFileListL(aFolder);

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppFileBrowserView::ConstructL() begins");
}

// -----------------------------------------------------------------------------
// CAafAppFileBrowserView::CAafAppFileBrowserView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppFileBrowserView::CAafAppFileBrowserView()
{
	iCurrentItemIndex = 0;
}


// -----------------------------------------------------------------------------
// CAafAppFileBrowserView::~CAafAppFileBrowserView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppFileBrowserView::~CAafAppFileBrowserView()
{
	// Free dynamically allocated memory
	if (iBrowserEngine)
	{
		delete iBrowserEngine;
		iBrowserEngine = NULL;
	}

	if (iListBox)
	{
		delete iListBox;
		iListBox = NULL;
	}

	//CleanupStack::Pop(iIconList);
}

void CAafAppFileBrowserView::SetFileListL(TFileName &aDirectory)
{
	TInt sessionError = iBrowserEngine->StartCurrentList();

	if (sessionError == KErrNone)
	{
		// Set the listbox to use the file list model
		CDesCArray* items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

		// If there are items, they will be removed here
		if (iBrowserEngine->RemoveCurrentItems(items))
		{
			// This makes changes to the actual listbox
			iListBox->HandleItemRemovalL();
		}

		// Initialise engine data
		iBrowserEngine->GetEntriesL(aDirectory);

		iBrowserEngine->GetCurrentItemsL(items);	

		// Refresh the listbox due to model change
		iListBox->HandleItemAdditionL();
		iListBox->SetCurrentItemIndex(0);


		iListBox->DrawNow();
	}

	iBrowserEngine->EndFileList();
}

void CAafAppFileBrowserView::HandleCurrentItemL()
{
	TInt sessionError = iBrowserEngine->StartCurrentList();

	if (sessionError == KErrNone)
	{
		TInt currentItem = iListBox->CurrentItemIndex();
		TBool isDir = iBrowserEngine->IsDir(currentItem);

		CDesCArray* items;

		// If current item is folder, rebuild listbox data model
		if (isDir)
		{
			// Set the listbox to use the file list model
			items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

			// If there are items, they will be removed here
			if (iBrowserEngine->RemoveCurrentItems(items))
			{
				// This makes changes to the actual listbox
				iListBox->HandleItemRemovalL();
			}
		}

		// Handle item
		iBrowserEngine->OpenOrLaunchCurrentL(currentItem);

		// If the current item is folder, rebuild listbox view model;
		if (isDir)
		{
			iBrowserEngine->GetCurrentItemsL(items);	

			// Refresh the listbox due to model change
			iListBox->HandleItemAdditionL();
			iListBox->SetCurrentItemIndex(0);


			iListBox->DrawNow();
		}
	}

	iBrowserEngine->EndFileList();	
}
//////////////////////////////////////////////////////////////////////////

void CAafAppFileBrowserView::LaunchCurrentL()
{
	HandleCurrentItemL();
}

void CAafAppFileBrowserView::AddImageToUpload()
{
	if (!iBrowserEngine->IsDir(iListBox->CurrentItemIndex()))
	{
		TFileName folderPath;
		TEntry fileEntry;

		// Get file description
		iBrowserEngine->GetFileDescription2L(iListBox->CurrentItemIndex(), fileEntry, folderPath);

		// Get instance of the upload service provider
		CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

		// Add file to upload cart
		uploadProvider->AddFile(fileEntry, folderPath);
	}
}

TBool CAafAppFileBrowserView::IsCurrentItemDir()
{
	return iBrowserEngine->IsDir(iListBox->CurrentItemIndex());
}

void CAafAppFileBrowserView::ShowFileProperties()
{
	__LOGSTR_TOFILE("CAafAppFileBrowserView::ShowFileProperties() begins");

	if (!IsCurrentItemDir())
	{
		TEntry fileEntry;
		TFileName folderPath;

		__LOGSTR_TOFILE("CAafAppFileBrowserView::ShowFileProperties() step 01");

		// File description
		iBrowserEngine->GetFileDescription2L(iListBox->CurrentItemIndex(), fileEntry, folderPath);

		// Load dialog from resources
		CAknMessageQueryDialog* dlg = new (ELeave)CAknMessageQueryDialog(); 
		dlg->PrepareLC(R_FILEPROPERTIES_DIALOG);
	
		// Set dialog title
		dlg->QueryHeading()->SetTextL(fileEntry.iName);		

		// Form dialog body text
		folderPath = TFileName(KNullDesC);
		_LIT(KSizeString, "Size:\t%d Kb\n\nDate,\ntime:\t");
		folderPath.Format(KSizeString, fileEntry.iSize/(1 << 10));

		TBuf<30> dateString(KNullDesC);
		_LIT(KDateString, "%D%M%Y%/0%1%/1%2%/2%3%/3 %-B%:0%J%:1%T%:2%S%:3%+B");

		fileEntry.iModified.FormatL(dateString, KDateString);		

		folderPath.Append(dateString);		

		dlg->SetMessageTextL(folderPath);

		__LOGSTR_TOFILE("CAafAppFileBrowserView::ShowFileProperties() step 02");

		// Run dialog
		dlg->RunLD(); 
	}

	__LOGSTR_TOFILE("CSupAppFileBrowserView::ShowFileProperties() ends");
}

// -----------------------------------------------------------------------------
// CAafAppFileBrowserView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppFileBrowserView::Draw( const TRect& aRect ) const
{
	CWindowGc& gc = SystemGc();
	// drawing code
	gc.SetPenStyle(CGraphicsContext::ENullPen);
	gc.SetBrushColor(KRgbGray);
	gc.SetBrushStyle(CGraphicsContext::ESolidBrush);
	gc.DrawRect(aRect);	
}

// -----------------------------------------------------------------------------
// CAafAppFileBrowserView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppFileBrowserView::SizeChanged()
{  
	if (iListBox)
	{/*
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

TInt CAafAppFileBrowserView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CAafAppFileBrowserView::ComponentControl(TInt aIndex)  const 
{ 
	return iListBox;
}

TKeyResponse CAafAppFileBrowserView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
{
	// Default return value
	TKeyResponse ret = EKeyWasNotConsumed;

	// See if we have a selection
	switch(aKeyEvent.iCode)
	{
		// If navigator button is pressed
	case EKeyOK:
		{
			HandleCurrentItemL();
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

void CAafAppFileBrowserView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CAafAppFileBrowserView::HandleControlEventL(CCoeControl* /*aControl*/, TCoeEvent /*aEventType*/)
{
	// Should be added later
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CAafAppFileBrowserView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KFileBrowserViewId);
}

// Activates this view, called by framework
void CAafAppFileBrowserView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
{
	// Set files to be displayed
	SetFileListL(iBrowserEngine->GetCurrentDirectory());

	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();

	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	CEikButtonGroupContainer* cba = appUiFactory->Cba();

	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	menuBar->SetMenuTitleResourceId(R_AAF_FILEBROWSER_MENUBAR);

	cba->SetCommandSetL(R_AAF_CBA_STANDART);
	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);

	// Restore listbox item selection (if it's not empty)
	if (iListBox->Model()->NumberOfItems())
		iListBox->SetCurrentItemIndex(iCurrentItemIndex);

	// Set view visible
	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CAafAppFileBrowserView::ViewDeactivated()
{
	// Remember currently selected listbox item index
	iCurrentItemIndex = iListBox->CurrentItemIndex();

	// Set view invisible
	MakeVisible(EFalse);
}
// End of File