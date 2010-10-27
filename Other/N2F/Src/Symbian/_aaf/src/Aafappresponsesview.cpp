/*
============================================================================
Name        : Aafappmyquestionsview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : User questions view
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
#include "Aafappresponsesview.h"
#include "aafuserquestionsserviceprovider.h"
#include "common.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppMyQuestionsView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppMyQuestionsView* CAafAppMyQuestionsView::NewL( const TRect& aRect )
{
	CAafAppMyQuestionsView* self = CAafAppMyQuestionsView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppMyQuestionsView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppMyQuestionsView* CAafAppMyQuestionsView::NewLC( const TRect& aRect )
{
	CAafAppMyQuestionsView* self = new ( ELeave ) CAafAppMyQuestionsView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppMyQuestionsView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppMyQuestionsView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppMyQuestionsView::ConstructL() begins");

	// Create a window for this application view
	CreateWindowL();

	// Instantiate listbox control
	iListBox = new (ELeave)CAknSingleGraphicStyleListBox;
	iListBox->SetContainerWindowL( *this );
	iListBox->ConstructL(this, EAknListBoxSelectionList);

	// Creates a GUI icon array
	CAknIconArray* icons = new (ELeave)CAknIconArray(1);
	CleanupStack::PushL(icons);
	icons->ConstructFromResourceL(R_USERQUESTIONSVIEW_ICONS);
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

	// Get CCoeEnv instance
	CEikonEnv* eikonEnv = CEikonEnv::Static();

	HBufC* stringHolder = StringLoader::LoadL(R_NO_OWN_QUESTIONS_TEXT, eikonEnv );
	CleanupStack::PushL(stringHolder);

	iListBox->View()->SetListEmptyTextL(*stringHolder);

	CleanupStack::PopAndDestroy(stringHolder);

	iListBox->ActivateL();

	// Instantiate private questions service provider
	//iQuestionsProvider = CAafMyQuestionsServiceProvider::InstanceL();
	iQuestionsProvider = CAafUserQuestionsProvider::GetInstanceL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppMyQuestionsView::ConstructL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppMyQuestionsView::CAafAppMyQuestionsView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppMyQuestionsView::CAafAppMyQuestionsView()
{
	// No implementation required
}


// -----------------------------------------------------------------------------
// CAafAppMyQuestionsView::~CAafAppMyQuestionsView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppMyQuestionsView::~CAafAppMyQuestionsView()
{
	// Free dynamically allocated memory
	if (iListBox)
	{
		delete iListBox;
		iListBox = NULL;
	}
}

// -----------------------------------------------------------------------------
// CAafAppMyQuestionsView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppMyQuestionsView::Draw( const TRect& /*aRect*/ ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );

}

// -----------------------------------------------------------------------------
// CAafAppMyQuestionsView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppMyQuestionsView::SizeChanged()
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

TInt CAafAppMyQuestionsView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CAafAppMyQuestionsView::ComponentControl(TInt /*aIndex*/)  const 
{ 
	return iListBox;
}

TInt CAafAppMyQuestionsView::GetItemsCount()
{
	if (iListBox)
		return iListBox->Model()->NumberOfItems();

	return 0;
}

void CAafAppMyQuestionsView::GetQuestions()
{
	if (iQuestionsProvider)
	{
		/*
		if (iQuestionsProvider->GetStatus() == KSenConnectionStatusReady)
		{
		*/
			StartWaitDialog(R_WAITNOTE_BLOCKING);

			// Start upload process using appropriate service provider
			//TInt retValue = iQuestionsProvider->RequestForQuestionsL(this);
			//TInt retValue = iQuestionsProvider->RequestForQuestions(this);
			TInt retValue = iQuestionsProvider->RequestForQuestions();

			StopWaitDialog();

			// Return value == KErrNone, this indicates that
			// questions have been retrieved successfully
			if (retValue == KErrNone)
			{
				SetItemListL();
			}
			// Retrieving failure
			else
			{
				CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

				// Get CCoeEnv instance
				CEikonEnv* eikonEnv = CEikonEnv::Static();

				HBufC* stringHolder = StringLoader::LoadL(R_MYQUESTIONS_FAILURE_NOTE, eikonEnv );

				//HBufC* stringHolder2 = StringLoader::LoadL(R_UPLOAD_FAILURE_CREDENTIALS, eikonEnv );

				dialog->SetTextL(*stringHolder);

				if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
				{

				}

				delete stringHolder;
				stringHolder = NULL;				
			}
			/*
		}
		// Connection is not ready
		else
		{
			CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

			// Get CCoeEnv instance
			CEikonEnv* eikonEnv = CEikonEnv::Static();

			HBufC* stringHolder = StringLoader::LoadL(R_CONNECTION_NOT_READY, eikonEnv );

			dialog->SetTextL(stringHolder->Des());

			if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
			{

			}

			delete stringHolder;
			stringHolder = NULL;
		}
		*/
	}	
}

TInt CAafAppMyQuestionsView::CurrentItemIndex()
{
	return iListBox->CurrentItemIndex();
}

TKeyResponse CAafAppMyQuestionsView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
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
void CAafAppMyQuestionsView::HandleControlEventL(CCoeControl* aControl, TCoeEvent aEventType)
{
// Should be added later
}
*/
void CAafAppMyQuestionsView::SetItemListL()
{
	if (iQuestionsProvider->GetQuestionCount())
	{
		// Set the listbox to use the file list model
		CDesCArray* items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

		// Delete all descriptors
		items->Reset();

		// Clear the listbox
		iListBox->HandleItemRemovalL();

		// Initialize listbox items
		//CAafMyQuestionsServiceProvider* questionsProvider = CAafMyQuestionsServiceProvider::InstanceL();
		//questionsProvider->GetQuestionsListItemsL(items);
		iQuestionsProvider->GetQuestionListItemsL(items);

		// Refresh the listbox due to model change
		iListBox->HandleItemAdditionL();
		iListBox->SetCurrentItemIndex(0);

		iListBox->DrawNow();
	}
}

void CAafAppMyQuestionsView::StartWaitDialog(TInt aDialogId)
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

	HBufC* stringHolder = StringLoader::LoadL(R_MYQUESTIONS_WAITNOTE, eikonEnv );

	iWaitDialog->SetTextL(*stringHolder);

	iWaitDialog->ExecuteLD(aDialogId);

	delete stringHolder;
	stringHolder = NULL;
}

void CAafAppMyQuestionsView::StopWaitDialog()
{
	// For wait dialog
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}	
}

void CAafAppMyQuestionsView::DialogDismissedL(TInt /*aButtonId*/)
{
	// Cancel retrieving process
	//iQuestionsProvider->Cancel();

	StopWaitDialog();
}

void CAafAppMyQuestionsView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CAafAppMyQuestionsView::HandleRequestCompletedL(const TInt& aError)
{
	RDebug::Print(_L("CAafAppImageCartView::HandleRequestCompletedL(%d)"), aError);


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

		HBufC* stringHolder = StringLoader::LoadL(R_MYQUESTIONS_FAILURE_NOTE, eikonEnv );

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
TVwsViewId CAafAppMyQuestionsView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KMyQuestionsViewId);
}

// Activates this view, called by framework
void CAafAppMyQuestionsView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
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

	menuBar->SetMenuTitleResourceId(R_AAF_MY_QUESTIONS_MENUBAR);

	cba->SetCommandSetL(R_AAF_CBA_STANDART);
	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);

	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CAafAppMyQuestionsView::ViewDeactivated()
{
	MakeVisible(EFalse);
}
// End of File
