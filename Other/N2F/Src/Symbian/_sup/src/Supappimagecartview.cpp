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
#include <aknnotewrappers.h>
#include <f32file.h>
#include <gulicon.h>
#include <akniconarray.h>
#include <senserviceconnection.h>
#include "SupAppUi.h"
#include "SupAppImageCartView.h"
#include "Supfilebrowserengine.h"
#include "SupUploadServiceProvider.h"
#include "common.h"
#include "sup.pan"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CSupAppImageCartView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppImageCartView* CSupAppImageCartView::NewL( const TRect& aRect )
{
	CSupAppImageCartView* self = CSupAppImageCartView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CSupAppImageCartView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupAppImageCartView* CSupAppImageCartView::NewLC( const TRect& aRect )
{
	CSupAppImageCartView* self = new ( ELeave ) CSupAppImageCartView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CSupAppImageCartView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CSupAppImageCartView::ConstructL( const TRect& aRect )
{
	// Create a window for this application view
	CreateWindowL();

	// Instantiate listbox control
	iListBox = new (ELeave)CAknSingleGraphicStyleListBox;

	iListBox->SetContainerWindowL( *this );
	iListBox->ConstructL( this, EAknListBoxMarkableList);

	// Set listbox icons
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
	iUploadProvider = CSupUploadServiceProvider::InstanceL();

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
}

// -----------------------------------------------------------------------------
// CSupAppImageCartView::CSupAppImageCartView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CSupAppImageCartView::CSupAppImageCartView()
{
	// No implementation required
	iCurrentUploadIndex = -1;
	
	iCurrentItemIndex = 0;
}


// -----------------------------------------------------------------------------
// CSupAppImageCartView::~CSupAppImageCartView()
// Destructor.
// -----------------------------------------------------------------------------
//
CSupAppImageCartView::~CSupAppImageCartView()
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
// CSupAppImageCartView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CSupAppImageCartView::Draw( const TRect& /*aRect*/ ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );

}

// -----------------------------------------------------------------------------
// CSupAppImageCartView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CSupAppImageCartView::SizeChanged()
{  
	if (iListBox)
	{
		TRect clientRect = Rect();
		iListBox->SetExtent(TPoint(0, 0), clientRect.Size()); // Set rectangle
		iListBox->DrawNow();
	}
}

TInt CSupAppImageCartView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CSupAppImageCartView::ComponentControl(TInt /*aIndex*/)  const 
{ 
	return iListBox;
}

void CSupAppImageCartView::CommandMark()
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

TBool CSupAppImageCartView::IsCurrentItemMarked()
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

void CSupAppImageCartView::DoUpload()
{
	__LOGSTR_TOFILE("CSupAppImageCartView::DoUpload() begins");

	if (iUploadProvider)
	{
		if (iUploadProvider->GetStatus() == KSenConnectionStatusReady)
		{
			__LOGSTR_TOFILE("CSupAppImageCartView::DoUpload() connection is ready");

			// Initiate upload process if the images cart is not empty
			if (iUploadProvider->GetFilesCount() > 0)
			{
				// Starts wait dialog
				__LOGSTR_TOFILE("CSupAppImageCartView::DoUpload() starts wait dialog");

				// Set currently processed upload order index
				iCurrentUploadIndex = 0;

				// Start upload process using appropriate service provider
				TInt retValue = iUploadProvider->StartUploadL(this, iUploadProvider->GetSelectionArray()[iCurrentUploadIndex]);

				// Upload failure - in this case it means there's no image was selected for upload
				if (retValue != KErrNone)
				{
					__LOGSTR_TOFILE("CSupAppImageCartView::DoUpload() failed to initiate upload process");

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

					// Refresh listbox items
					RefreshSelectionListL();
				}
				else
				{
					StartWaitDialog();
				}
			}
		}
		// Connection is not ready
		else
		{
			__LOGSTR_TOFILE("CSupAppImageCartView::DoUpload() connection is not ready");

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

void CSupAppImageCartView::RemoveSelectedItems()
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

void CSupAppImageCartView::RemoveAllItems()
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

TInt CSupAppImageCartView::GetItemsCount()
{
	if (iListBox)
		return iListBox->Model()->NumberOfItems();

	return 0;
}

void CSupAppImageCartView::PreviewCurrentItemL()
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

TKeyResponse CSupAppImageCartView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
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

void CSupAppImageCartView::SetListboxIcons()
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

void CSupAppImageCartView::SetFileListL()
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
		CSupUploadServiceProvider* uploadProvider = CSupUploadServiceProvider::InstanceL();
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

void CSupAppImageCartView::RefreshSelectionListL()
{
	CListBoxView* listBoxView = iListBox->View();

	for (TInt i = 0; i < iUploadProvider->GetFilesCount(); i++)
	{
		if (iUploadProvider->IsItemMarked(i))
			listBoxView->SelectItemL(i);
		else
			listBoxView->DeselectItem(i);
	}
}

void CSupAppImageCartView::StartWaitDialog()
{
	RDebug::Print(_L("CSupAppLoginView::StartWaitDialog()"));

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

void CSupAppImageCartView::StopWaitDialog()
{
	// For wait dialog
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}
}

void CSupAppImageCartView::DialogDismissedL(TInt /*aButtonId*/)
{
	__LOGSTR_TOFILE("CSupAppImageCartView::DialogDismissedL() is called");

	// Cancel upload process
	iUploadProvider->CancelUpload();

	StopWaitDialog();

	__LOGSTR_TOFILE("CSupAppImageCartView::DialogDismissedL() ends");
}

void CSupAppImageCartView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CSupAppImageCartView::HandleRequestCompletedL(const TInt& aError)
{
	RDebug::Print(_L("CSupAppImageCartView::HandleRequestCompletedL(%d)"), aError);

	__LOGSTR_TOFILE1("CSupAppImageCartView::HandleRequestCompletedL() begins with error code == %d", aError);

	// If no error
	if (aError == KErrNone)
	{
		if (iCurrentUploadIndex == iUploadProvider->GetSelectionArray().Count()-1)
		{
			__LOGSTR_TOFILE("CSupAppImageCartView::HandleRequestCompletedL() complete upload process");

			// Stop waitDialog
			StopWaitDialog();

			// Show confirmation note
			CEikonEnv* eikonEnv = CEikonEnv::Static();
			HBufC* stringHolder = StringLoader::LoadL(R_UPLOAD_CONFIRMATION_NOTE, eikonEnv );
			CleanupStack::PushL(stringHolder);

			CAknConfirmationNote* confirmationNote = new (ELeave) CAknConfirmationNote( ETrue ); //Waiting

			confirmationNote->ExecuteLD(*stringHolder); //Blocks until note dismissed

			CleanupStack::PopAndDestroy(stringHolder);

			
			__LOGSTR_TOFILE("CSupAppImageCartView::HandleRequestCompletedL() before RemoveSelectedItems()");

			// Remove selected items
			RemoveSelectedItems();

			__LOGSTR_TOFILE("CSupAppImageCartView::HandleRequestCompletedL() after RemoveSelectedItems()");
		}
		else
		{
			__LOGSTR_TOFILE("CSupAppImageCartView::HandleRequestCompletedL() go to the next image");

			// Upload next image
			++iCurrentUploadIndex;
			
			iUploadProvider->StartUploadL(this, iUploadProvider->GetSelectionArray()[iCurrentUploadIndex]);
		}		
	}
	// In case of error, show notification about this
	else
	{
		__LOGSTR_TOFILE("CSupAppImageCartView::HandleRequestCompletedL() if some error has occured");

		// Stop waitDialog
		StopWaitDialog();

		// Show warning note
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

	__LOGSTR_TOFILE("CSupAppImageCartView::HandleRequestCompletedL() ends");
}

// -----------------------------------------------------------------------------
// From MCoeView
// -----------------------------------------------------------------------------
//
// Returns the ViewId for the view server
TVwsViewId CSupAppImageCartView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KImageCartViewId);
}

// Activates this view, called by framework
void CSupAppImageCartView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
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

	menuBar->SetMenuTitleResourceId(R_SUP_UPLOAD_IMAGE_CART_MENUBAR);

	cba->SetCommandSetL(R_SUP_CBA_STANDART);
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
void CSupAppImageCartView::ViewDeactivated()
{
	// Remember currently selected listbox item index
	if (GetItemsCount())
		iCurrentItemIndex = iListBox->CurrentItemIndex();

	// Set view invisible
	MakeVisible(EFalse);
}
// End of File
