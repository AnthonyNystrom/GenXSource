/*
============================================================================
Name        : Aafappprivatequestionsview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Private questions view
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
#include "Aafappprivatequestionsview.h"
#include "aafprivatequestionsserviceprovider.h"
#include "common.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppPQuestionsView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppPQuestionsView* CAafAppPQuestionsView::NewL( const TRect& aRect )
{
	CAafAppPQuestionsView* self = CAafAppPQuestionsView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppPQuestionsView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppPQuestionsView* CAafAppPQuestionsView::NewLC( const TRect& aRect )
{
	CAafAppPQuestionsView* self = new ( ELeave ) CAafAppPQuestionsView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppPQuestionsView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppPQuestionsView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppPQuestionsView::ConstructL() begins");

	// Create a window for this application view
	CreateWindowL();
	
	__LOGSTR_TOFILE("CAafAppPQuestionsView::ConstructL() before listbox creating");

	// Instantiate listbox control
	iListBox = new (ELeave)CAknDoubleGraphicStyleListBox;
	iListBox->SetContainerWindowL( *this );
	iListBox->ConstructL(this, EAknListBoxSelectionList);
	
	__LOGSTR_TOFILE("CAafAppPQuestionsView::ConstructL() before icons list creating");

	// Creates a GUI icon array
	CAknIconArray* icons = new (ELeave)CAknIconArray(1);
	CleanupStack::PushL(icons);
	icons->ConstructFromResourceL(R_PQUESTIONSVIEW_ICONS);
	// Sets graphics as ListBox icon.
	iListBox->ItemDrawer()->ColumnData()->SetIconArray(icons);

	CleanupStack::Pop(); // icons

	// Enable marquee effect
	iListBox->ItemDrawer()->ColumnData()->SetMarqueeParams(3, 20, 1000000, 200000);
	iListBox->ItemDrawer()->ColumnData()->EnableMarqueeL(ETrue);

	__LOGSTR_TOFILE("CAafAppPQuestionsView::ConstructL() after icon list creating");

	// Create the scroll indicator
	iListBox->CreateScrollBarFrameL(ETrue);
	iListBox->ScrollBarFrame()->SetScrollBarVisibilityL( CEikScrollBarFrame::EOff, CEikScrollBarFrame::EAuto);
	iListBox->Model()->SetOwnershipType( ELbmOwnsItemArray );

	// Get CCoeEnv instance
	CEikonEnv* eikonEnv = CEikonEnv::Static();

	HBufC* stringHolder = StringLoader::LoadL(R_NO_PRIVATE_QUESTIONS_TEXT, eikonEnv );
	CleanupStack::PushL(stringHolder);

	iListBox->View()->SetListEmptyTextL(*stringHolder);

	CleanupStack::PopAndDestroy(stringHolder);

	iListBox->ActivateL();

	// Instantiate private questions service provider
	//iQuestionsProvider = CAafPQuestionsServiceProvider::InstanceL();
	iQuestionsProvider = CAafPrivateQuestionsProvider::GetInstanceL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppPQuestionsView::ConstructL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppPQuestionsView::CAafAppPQuestionsView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppPQuestionsView::CAafAppPQuestionsView()
{
	// No implementation required
}


// -----------------------------------------------------------------------------
// CAafAppPQuestionsView::~CAafAppPQuestionsView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppPQuestionsView::~CAafAppPQuestionsView()
{
	// Free dynamically allocated memory
	if (iListBox)
	{
		delete iListBox;
		iListBox = NULL;
	}
}

// -----------------------------------------------------------------------------
// CAafAppPQuestionsView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppPQuestionsView::Draw( const TRect& /*aRect*/ ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );

}

// -----------------------------------------------------------------------------
// CAafAppPQuestionsView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppPQuestionsView::SizeChanged()
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

TInt CAafAppPQuestionsView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CAafAppPQuestionsView::ComponentControl(TInt /*aIndex*/)  const 
{ 
	return iListBox;
}

TInt CAafAppPQuestionsView::GetItemsCount()
{
	if (iListBox)
		return iListBox->Model()->NumberOfItems();

	return 0;
}

void CAafAppPQuestionsView::GetQuestions()
{
	if (iQuestionsProvider)
	{	
		// Start questions retrieving
		//TInt retValue = iQuestionsProvider->RequestForQuestions(this);
	
		StartWaitDialog(R_WAITNOTE_BLOCKING);
		
		TInt retValue = iQuestionsProvider->RequestForQuestions();
		
		StopWaitDialog();

		// Return value == KErrNone, this indicates that
		// private questions have been retrieved successfully
		if (retValue == KErrNone)
		{
			SetItemListL();
		}
		// Retrieving failure
		else
		{			
			__LOGSTR_TOFILE1("CAafAppPQuestionsView::GetQuestions() failed with error code == %d", retValue);

			CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

			// Get CCoeEnv instance
			CEikonEnv* eikonEnv = CEikonEnv::Static();
			
			HBufC* stringHolder = StringLoader::LoadL(R_PQUESTIONS_FAILURE_NOTE, eikonEnv );

		
			dialog->SetTextL(*stringHolder);

			if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
			{

			}

			delete stringHolder;
			stringHolder = NULL;				
		}			
	}	
}

TInt CAafAppPQuestionsView::CurrentItemIndex()
{
	return iListBox->CurrentItemIndex();
}

TKeyResponse CAafAppPQuestionsView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
{
	// Default return value
	TKeyResponse ret = EKeyWasNotConsumed;

	// See if we have a selection
	switch(aKeyEvent.iCode)
	{
		// is navigator button pressed
	case EKeyOK:
		{
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

/*
void CAafAppPQuestionsView::HandleControlEventL(CCoeControl* aControl, TCoeEvent aEventType)
{
// Should be added later
}
*/
void CAafAppPQuestionsView::SetItemListL()
{
	if (iQuestionsProvider->GetQuestionCount())
	{
		// Set the listbox to use the file list model
		CDesCArray* items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

		// Delete all descriptors
		items->Reset();

		// Clear the listbox
		iListBox->HandleItemRemovalL();

		// Get questions list
		iQuestionsProvider->GetQuestionListItemsL(items);

		// Refresh the listbox due to model change
		iListBox->HandleItemAdditionL();
		iListBox->SetCurrentItemIndex(0);

		iListBox->DrawNow();
	}
}

void CAafAppPQuestionsView::StartWaitDialog(TInt aDialogId)
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

	HBufC* stringHolder = StringLoader::LoadL(R_PQUESTIONS_WAITNOTE, eikonEnv );

	iWaitDialog->SetTextL(*stringHolder);

	iWaitDialog->ExecuteLD(aDialogId);

	delete stringHolder;
	stringHolder = NULL;
}

void CAafAppPQuestionsView::StopWaitDialog()
{
	// For wait dialog
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}	
}

void CAafAppPQuestionsView::DialogDismissedL(TInt /*aButtonId*/)
{
	// Cancel upload process
	//iQuestionsProvider->Cancel();

	StopWaitDialog();
}

void CAafAppPQuestionsView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CAafAppPQuestionsView::HandleRequestCompletedL(const TInt& aError)
{
	RDebug::Print(_L("CAafAppImageCartView::HandleRequestCompletedL(%d)"), aError);

	__LOGSTR_TOFILE1("CAafAppPQuestionsView::HandleRequestCompletedL() called with error code == %d", aError);

	// Stop waitDialog
	StopWaitDialog();

	// If no error
	if (aError == KErrNone)
	{
		SetItemListL();
	}
	// In case of error, show notification about this
	else
	{
		CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

		// Get CCoeEnv instance
		CEikonEnv* eikonEnv = CEikonEnv::Static();

		HBufC* stringHolder = StringLoader::LoadL(R_PQUESTIONS_FAILURE_NOTE, eikonEnv );

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
TVwsViewId CAafAppPQuestionsView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KPQuestionsViewId);
}

// Activates this view, called by framework
void CAafAppPQuestionsView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
{
	//if (iListBox->Model()->NumberOfItems() == 0)
	//{
	//	GetQuestions();
	//}

	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();

	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	CEikButtonGroupContainer* cba = appUiFactory->Cba();

	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	menuBar->SetMenuTitleResourceId(R_AAF_PRIVATE_QUESTIONS_MENUBAR);

	cba->SetCommandSetL(R_AAF_CBA_STANDART);
	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);

	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CAafAppPQuestionsView::ViewDeactivated()
{
	MakeVisible(EFalse);
}
// End of File
