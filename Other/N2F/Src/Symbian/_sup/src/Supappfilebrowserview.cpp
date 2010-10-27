/*
============================================================================
Name        : Supappfilebrowserview.cpp
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
#include "SupAppUi.h"
#include "SupAppFileBrowserView.h"
#include "Supfilebrowserengine.h"
#include "Supuploadserviceprovider.h"
#include "common.h"
#include "Sup.hrh"
#include "sup.rsg"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CSupAppFileBrowserView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppFileBrowserView* CSupAppFileBrowserView::NewL( const TRect& aRect )
{
	CSupAppFileBrowserView* self = CSupAppFileBrowserView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CSupAppFileBrowserView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppFileBrowserView* CSupAppFileBrowserView::NewLC( const TRect& aRect )
{
	CSupAppFileBrowserView* self = new ( ELeave ) CSupAppFileBrowserView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CSupAppFileBrowserView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CSupAppFileBrowserView::ConstructL( const TRect& aRect )
{
	// Create a window for this application view
	CreateWindowL();

	// Instantiate listbox control
	//iListBox = new (ELeave)CAknSingleStyleListBox;
	iListBox = new (ELeave)CAknSingleGraphicStyleListBox;

	iListBox->ConstructL( this, EAknListBoxSelectionList);

	iListBox->SetContainerWindowL( *this );

	// Creates a GUI icon array
	CAknIconArray* icons = new (ELeave)CAknIconArray(5);
	CleanupStack::PushL(icons);
	icons->ConstructFromResourceL(R_BROWSERVIEW_ICONS);
	// Sets graphics as ListBox icon.
	iListBox->ItemDrawer()->ColumnData()->SetIconArray(icons);

	CleanupStack::Pop(); // icons

	// Enable marquee effect
	iListBox->ItemDrawer()->ColumnData()->SetMarqueeParams(3, 20, 1000000, 200000);
	iListBox->ItemDrawer()->ColumnData()->EnableMarqueeL(ETrue);

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
}

// -----------------------------------------------------------------------------
// CSupAppFileBrowserView::CSupAppFileBrowserView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CSupAppFileBrowserView::CSupAppFileBrowserView()
{
	iCurrentItemIndex = 0;
}


// -----------------------------------------------------------------------------
// CSupAppFileBrowserView::~CSupAppFileBrowserView()
// Destructor.
// -----------------------------------------------------------------------------
//
CSupAppFileBrowserView::~CSupAppFileBrowserView()
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
}

void CSupAppFileBrowserView::SetFileListL(TFileName &aDirectory)
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

void CSupAppFileBrowserView::HandleCurrentItemL()
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

void CSupAppFileBrowserView::LaunchCurrentL()
{
	HandleCurrentItemL();
}

void CSupAppFileBrowserView::AddImageToUpload()
{
	if (!iBrowserEngine->IsDir(iListBox->CurrentItemIndex()))
	{
		TFileName folderPath;
		TEntry fileEntry;

		// Get file description
		iBrowserEngine->GetFileDescription2L(iListBox->CurrentItemIndex(), fileEntry, folderPath);

		// Get instance of the upload service provider
		CSupUploadServiceProvider* uploadProvider = CSupUploadServiceProvider::InstanceL();

		// Add file to upload cart
		uploadProvider->AddFile(fileEntry, folderPath);
	}
}

TBool CSupAppFileBrowserView::IsCurrentItemDir()
{
	return iBrowserEngine->IsDir(iListBox->CurrentItemIndex());
}

void CSupAppFileBrowserView::ShowFileProperties()
{
	__LOGSTR_TOFILE("CSupAppFileBrowserView::ShowFileProperties() begins");

	if (!IsCurrentItemDir())
	{
		TEntry fileEntry;
		TFileName folderPath;

		__LOGSTR_TOFILE("CSupAppFileBrowserView::ShowFileProperties() step 01");

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

		__LOGSTR_TOFILE("CSupAppFileBrowserView::ShowFileProperties() step 02");

		// Run dialog
		dlg->RunLD(); 
	}

	__LOGSTR_TOFILE("CSupAppFileBrowserView::ShowFileProperties() ends");
}

// -----------------------------------------------------------------------------
// CSupAppFileBrowserView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CSupAppFileBrowserView::Draw( const TRect& aRect ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );

}

// -----------------------------------------------------------------------------
// CSupAppFileBrowserView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CSupAppFileBrowserView::SizeChanged()
{  
	if (iListBox)
	{/*
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

TInt CSupAppFileBrowserView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CSupAppFileBrowserView::ComponentControl(TInt aIndex)  const 
{ 
	return iListBox;
}

TKeyResponse CSupAppFileBrowserView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
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

void CSupAppFileBrowserView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CSupAppFileBrowserView::HandleControlEventL(CCoeControl* /*aControl*/, TCoeEvent /*aEventType*/)
{
	// Should be added later
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CSupAppFileBrowserView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KFileBrowserViewId);
}

// Activates this view, called by framework
void CSupAppFileBrowserView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
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

	menuBar->SetMenuTitleResourceId(R_SUP_FILEBROWSER_MENUBAR);

	cba->SetCommandSetL(R_SUP_CBA_STANDART);
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
void CSupAppFileBrowserView::ViewDeactivated()
{
	// Remember currently selected listbox item index
	iCurrentItemIndex = iListBox->CurrentItemIndex();

	// Set view invisible
	MakeVisible(EFalse);
}
// End of File