/*
============================================================================
Name        : Supappimagecartview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Image cart view
============================================================================
*/

// INCLUDE FILES
#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikclbd.h>
#include <eikspane.h>
#include <f32file.h>
#include <gulicon.h>
#include <akniconarray.h>
#include <senserviceconnection.h>
#include "AafAppUi.h"
#include "aafAppImageCartView.h"
#include "Aaffilebrowserengine.h"
#include "AafUploadServiceProvider.h"
#include "common.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppImageCartView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppImageCartView* CAafAppImageCartView::NewL( const TRect& aRect )
{
	CAafAppImageCartView* self = CAafAppImageCartView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppImageCartView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppImageCartView* CAafAppImageCartView::NewLC( const TRect& aRect )
{
	CAafAppImageCartView* self = new ( ELeave ) CAafAppImageCartView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppImageCartView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppImageCartView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppImageCartView::ConstructL() begins");

	// Create a window for this application view
	CreateWindowL();

	// Instantiate listbox control
	iListBox = new (ELeave)CAknSingleGraphicStyleListBox;

	iListBox->SetContainerWindowL( *this );
	iListBox->ConstructL( this, EAknListBoxMarkableList);

	// Creates a GUI icon array
	SetListboxIcons();

	// Enable marquee effect
	iListBox->ItemDrawer()->ColumnData()->SetMarqueeParams(3, 20, 1000000, 200000);
	iListBox->ItemDrawer()->ColumnData()->EnableMarqueeL(ETrue);

	// Create the scroll indicator
	iListBox->CreateScrollBarFrameL(ETrue);
	iListBox->ScrollBarFrame()->SetScrollBarVisibilityL( CEikScrollBarFrame::EOff, CEikScrollBarFrame::EAuto);

	iListBox->Model()->SetOwnershipType( ELbmOwnsItemArray );

	// Get CCoeEnv instance
	CEikonEnv* eikonEnv = CEikonEnv::Static();

	HBufC* stringHolder = StringLoader::LoadL(R_EMPTY_IMAGE_CART_TEXT, eikonEnv );
	CleanupStack::PushL(stringHolder);

	iListBox->View()->SetListEmptyTextL(*stringHolder);

	CleanupStack::PopAndDestroy(stringHolder);

	iListBox->ActivateL();

	// Initialiaze service provider member variable
	iUploadProvider = CAafUploadServiceProvider::GetInstanceL();

	// Initialiaze file browser engine (to open file)
	iBrowserEngine = new (ELeave) CFileBrowserEngine;

#ifdef __SERIES60_3X__
	iBrowserEngine->ConstructL();
#else
	iBrowserEngine->ConstructL((CEikProcess*)(((CEikAppUi*)iCoeEnv->AppUi())->Application()->Process()));
#endif

	// Set list of files
	SetFileListL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppImageCartView::ConstructL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppImageCartView::CAafAppImageCartView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppImageCartView::CAafAppImageCartView()
{
	// No implementation required
}


// -----------------------------------------------------------------------------
// CAafAppImageCartView::~CAafAppImageCartView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppImageCartView::~CAafAppImageCartView()
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

	if (iWaitDialog)
	{
		delete iWaitDialog;
		iWaitDialog = NULL;
	}
}

// -----------------------------------------------------------------------------
// CAafAppImageCartView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppImageCartView::Draw( const TRect& /*aRect*/ ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );

}

// -----------------------------------------------------------------------------
// CAafAppImageCartView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppImageCartView::SizeChanged()
{  
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

TInt CAafAppImageCartView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CAafAppImageCartView::ComponentControl(TInt /*aIndex*/)  const 
{ 
	return iListBox;
}

void CAafAppImageCartView::CommandMark()
{
	CTextListBoxModel* model = iListBox->Model();
	TInt itemCount = model->NumberOfItems();
	CListBoxView* listBoxView = iListBox->View();

	// Set opposite value
	TBool isSelected = listBoxView->ItemIsSelected(iListBox->CurrentItemIndex());

	if (isSelected)
	{
		listBoxView->DeselectItem(iListBox->CurrentItemIndex());
		iUploadProvider->SetItemMarked(iListBox->CurrentItemIndex(), EFalse);
	}
	else
	{
		listBoxView->SelectItemL(iListBox->CurrentItemIndex());
		iUploadProvider->SetItemMarked(iListBox->CurrentItemIndex(), ETrue);
	}
}

TBool CAafAppImageCartView::IsCurrentItemMarked()
{
	CTextListBoxModel* model = iListBox->Model();
	TInt itemCount = model->NumberOfItems();
	CListBoxView* listBoxView = iListBox->View();

	// Check whether item is marked
	if(itemCount > 0)
	{
		return listBoxView->ItemIsSelected(iListBox->CurrentItemIndex());
	}

	return EFalse;
}

void CAafAppImageCartView::DoUpload()
{
	if (iUploadProvider)
	{
		if (iUploadProvider->GetStatus() == KSenConnectionStatusReady)
		{
			// Initiate upload process if the images cart is not empty
			if (iUploadProvider->GetFilesCount() > 0)
			{
				// Start upload process using appropriate service provider
				TInt retValue = 0;//iUploadProvider->StartUploadL(this);

				// Return value == KErrNone, this indicates that
				// upload process has been started successfully
				if (retValue == KErrNone)
				{
					StartWaitDialog();
				}
				// Upload failure - in this case it means there's no image was selected for upload
				else
				{
					CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

					// Get CCoeEnv instance
					CEikonEnv* eikonEnv = CEikonEnv::Static();

					HBufC* stringHolder = StringLoader::LoadL(R_UPLOAD_FAILURE_NOTE, eikonEnv );

					dialog->SetTextL(*stringHolder);

					if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
					{

					}

					delete stringHolder;
					stringHolder = NULL;
				}
			}
			// Connection is not ready
			else
			{
				CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

				// Get CCoeEnv instance
				CEikonEnv* eikonEnv = CEikonEnv::Static();

				HBufC* stringHolder = StringLoader::LoadL(R_CONNECTION_NOT_READY, eikonEnv );

				dialog->SetTextL(*stringHolder);

				if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
				{

				}

				delete stringHolder;
				stringHolder = NULL;
			}
		}
	}	
}

void CAafAppImageCartView::RemoveSelectedItems()
{
	CTextListBoxModel* listboxModel = iListBox->Model();

	if (listboxModel->NumberOfItems() > 0)
	{
		TBool hasChanged = EFalse;

		CListBoxView* listboxView = iListBox->View();

		// Reverse iteration
		for (TInt i = listboxModel->NumberOfItems()-1; i >= 0; i--)
		{
			// If current item is marked
			// delete from data provider
			if (listboxView->ItemIsSelected(i))
			{
				iUploadProvider->DeleteItem(i);

				hasChanged = ETrue;
			}
		}

		// If any changes happened
		// update listbox
		if (hasChanged)
		{
			SetFileListL();
		}
	}	
}

void CAafAppImageCartView::RemoveAllItems()
{
	if (iUploadProvider->GetFilesCount() > 0)
	{
		// Delete items from service provider
		iUploadProvider->ResetArray();

		// Delete item from listbox data model
		// Set the listbox to use the file list model
		CDesCArray* items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

		// Delete all descriptors
		items->Reset();

		// Clear the listbox
		iListBox->HandleItemRemovalL();

		// Redraw listbox
		iListBox->DrawNow();
	}	
}

TInt CAafAppImageCartView::GetItemsCount()
{
	if (iListBox)
		return iListBox->Model()->NumberOfItems();

	return 0;
}

void CAafAppImageCartView::PreviewCurrentItemL()
{
	__ASSERT_ALWAYS(iUploadProvider, Panic(EProviderNotInitialiazed));
	__ASSERT_ALWAYS(iBrowserEngine, Panic(EProviderNotInitialiazed));

	if (iUploadProvider->GetFilesCount() > 0)
	{
		if (iBrowserEngine->StartCurrentList() == KErrNone)
		{
			iBrowserEngine->LaunchCurrentL(iUploadProvider->GetFilePath(iListBox->CurrentItemIndex()));
		}

		iBrowserEngine->EndFileList();
	}	
}

TKeyResponse CAafAppImageCartView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
{
	// Default return value
	TKeyResponse ret = EKeyWasNotConsumed;

	// See if we have a selection
	switch(aKeyEvent.iCode)
	{
		// is navigator button pressed
	case EKeyOK:
		{
			CommandMark();
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

void CAafAppImageCartView::SetListboxIcons()
{
	// Create icons array with granularity == 2 (file icon and mark icon)
	CAknIconArray* iconArray = new (ELeave)CAknIconArray(2);
	CleanupStack::PushL(iconArray);
	CFbsBitmap* markBitmap = NULL;
	CFbsBitmap* markBitmapMask = NULL;

	//CListItemDrawer is using this logical color as default for its marked icons
	TRgb defaultColor;
	defaultColor = CEikonEnv::Static()->Color(EColorControlText);

	AknsUtils::CreateColorIconLC(AknsUtils::SkinInstance(),
		KAknsIIDQgnIndiMarkedAdd,
		KAknsIIDQsnIconColors,
		EAknsCIQsnIconColorsCG13,
		markBitmap,
		markBitmapMask,
		KAvkonBitmapFile,
		EMbmAvkonQgn_indi_marked_add,
		EMbmAvkonQgn_indi_marked_add_mask,
		defaultColor
		);

	CGulIcon* markIcon = CGulIcon::NewL( markBitmap,markBitmapMask );
	CleanupStack::Pop( 2 ); // markBitmap, markBitmapMask

	CleanupStack::PushL( markIcon );      
	iconArray->AppendL( markIcon );

	// File icons array
	CAknIconArray* fileIcons = new (ELeave)CAknIconArray(1);
	CleanupStack::PushL(fileIcons);
	fileIcons->ConstructFromResourceL(R_CARTVIEW_ICONS);

	iconArray->AppendL((*fileIcons)[0]);

	CleanupStack::Pop(fileIcons);

	// Sets graphics as ListBox icon.
	iListBox->ItemDrawer()->ColumnData()->SetIconArray(iconArray);

	// markIcon, iconArray
	CleanupStack::Pop( 2 );
}

void CAafAppImageCartView::SetFileListL()
{
	if (iUploadProvider->GetFilesCount())
	{
		// Set the listbox to use the file list model
		CDesCArray* items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

		// Delete all descriptors
		items->Reset();

		// Clear the listbox
		iListBox->HandleItemRemovalL();

		// Get filepath list
		CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();
		uploadProvider->GetFileListItemsL(items);

		// Mark items if necessary
		CListBoxView* listBoxView = iListBox->View();
		RArray<TInt> selectionArray = iUploadProvider->GetSelectionArray();

		for (TInt i = 0; i < selectionArray.Count(); i++)
		{
			listBoxView->SelectItemL(selectionArray[i]);
		}

		// Refresh the listbox due to model change
		iListBox->HandleItemAdditionL();
		iListBox->SetCurrentItemIndex(0);

		iListBox->DrawNow();
	}
	// Otherwise
	// just reset listbox view if necessary
	else
	{
		// Delete item from listbox data model
		// Set the listbox to use the file list model
		CDesCArray* items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

		if (items->Count())
		{
			// Delete all descriptors
			items->Reset();

			// Clear the listbox
			iListBox->HandleItemRemovalL();

			// Redraw listbox
			iListBox->DrawNow();
		}
	}
}

void CAafAppImageCartView::StartWaitDialog()
{
	RDebug::Print(_L("CAafAppLoginView::StartWaitDialog()"));

	if(iWaitDialog)
	{
		delete iWaitDialog;
		iWaitDialog = NULL;
	}

	// For the wait dialog
	//create instance
	iWaitDialog = new (ELeave) CAknWaitDialog(REINTERPRET_CAST(CEikDialog**, &iWaitDialog)); 

	iWaitDialog->SetCallback(this);

	// Get CCoeEnv instance
	CEikonEnv* eikonEnv = CEikonEnv::Static();

	HBufC* stringHolder = StringLoader::LoadL(R_UPLOAD_WAITNOTE, eikonEnv );

	iWaitDialog->SetTextL(*stringHolder);

	iWaitDialog->ExecuteLD(R_WAITNOTE);

	delete stringHolder;
	stringHolder = NULL;
}

void CAafAppImageCartView::StopWaitDialog()
{
	// For wait dialog
	iWaitDialog->ProcessFinishedL(); 
	iWaitDialog = NULL;
}

void CAafAppImageCartView::DialogDismissedL(TInt /*aButtonId*/)
{
	// Cancel upload process
	iUploadProvider->CancelUpload();

	StopWaitDialog();
}

void CAafAppImageCartView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CAafAppImageCartView::HandleRequestCompletedL(const TInt& aError)
{
	RDebug::Print(_L("CAafAppImageCartView::HandleRequestCompletedL(%d)"), aError);

	// Stop waitDialog
	StopWaitDialog();

	// If no error
	if (aError == KErrNone)
	{
		// Remove selected items
		RemoveSelectedItems();
	}
	// In case of error, show notification about this
	else
	{
		CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

		// Get CCoeEnv instance
		CEikonEnv* eikonEnv = CEikonEnv::Static();

		HBufC* stringHolder = StringLoader::LoadL(R_UPLOAD_FAILURE_NOTE, eikonEnv );

		dialog->SetTextL(*stringHolder);

		if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
		{

		}

		delete stringHolder;
		stringHolder = NULL;
	}	
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CAafAppImageCartView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KImageCartViewId);
}

// Activates this view, called by framework
void CAafAppImageCartView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
{
	// Set files to be displayed
	SetFileListL();

	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();


	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	CEikButtonGroupContainer* cba = appUiFactory->Cba();

	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	menuBar->SetMenuTitleResourceId(R_AAF_UPLOAD_IMAGE_CART_MENUBAR);

	cba->SetCommandSetL(R_AAF_CBA_STANDART);
	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);

	// Restore listbox item selection (it it's not empty)
	if (GetItemsCount() && iCurrentItemIndex < GetItemsCount())
		iListBox->SetCurrentItemIndex(iCurrentItemIndex);

	// Set view visible
	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CAafAppImageCartView::ViewDeactivated()
{
	// Remember currently selected listbox item index
	if (GetItemsCount())
		iCurrentItemIndex = iListBox->CurrentItemIndex();

	// Set view invisible
	MakeVisible(EFalse);
}
// End of File
