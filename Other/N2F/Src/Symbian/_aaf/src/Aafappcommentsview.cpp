/*
============================================================================
Name        : Aafappcommentsview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Question comments view
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
#include "aafuserquestionsserviceprovider.h"
#include "Aafappcommentsview.h"
#include "common.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppCommentsView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppCommentsView* CAafAppCommentsView::NewL( const TRect& aRect )
{
	CAafAppCommentsView* self = CAafAppCommentsView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppCommentsView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppCommentsView* CAafAppCommentsView::NewLC( const TRect& aRect )
{
	CAafAppCommentsView* self = new ( ELeave ) CAafAppCommentsView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppCommentsView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppCommentsView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppCommentsView::ConstructL() begins");

	// Create a window for this application view
	CreateWindowL();

	// Instantiate listbox control
	iListBox = new (ELeave)CAknSingleStyleListBox;
	iListBox->SetContainerWindowL( *this );
	iListBox->ConstructL(this, EAknListBoxSelectionList);

	// Enable marquee effect
	iListBox->ItemDrawer()->ColumnData()->SetMarqueeParams(3, 20, 1000000, 200000);
	iListBox->ItemDrawer()->ColumnData()->EnableMarqueeL(ETrue);

	// Create the scroll indicator
	iListBox->CreateScrollBarFrameL(ETrue);
	iListBox->ScrollBarFrame()->SetScrollBarVisibilityL( CEikScrollBarFrame::EOff, CEikScrollBarFrame::EAuto);
	iListBox->Model()->SetOwnershipType( ELbmOwnsItemArray );
	iListBox->ActivateL();

	// Instantiate private questions service provider
	//iQuestionsProvider = CAafMyQuestionsServiceProvider::InstanceL();
	iQuestionsProvider = CAafUserQuestionsProvider::GetInstanceL();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppCommentsView::ConstructL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppCommentsView::CAafAppCommentsView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppCommentsView::CAafAppCommentsView()
{
	// No implementation required
}


// -----------------------------------------------------------------------------
// CAafAppCommentsView::~CAafAppCommentsView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppCommentsView::~CAafAppCommentsView()
{
	// Free dynamically allocated memory
	if (iListBox)
	{
		delete iListBox;
		iListBox = NULL;
	}

	if (iViewParam)
	{
		delete iViewParam;
		iViewParam = NULL;
	}
}

// -----------------------------------------------------------------------------
// CAafAppCommentsView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppCommentsView::Draw( const TRect& /*aRect*/ ) const
{
	// Get the standard graphics context
	CWindowGc& gc = SystemGc();

	// Gets the control's extent
	TRect drawRect( Rect());

	// Clears the screen
	gc.Clear( drawRect );

}

// -----------------------------------------------------------------------------
// CAafAppCommentsView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppCommentsView::SizeChanged()
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

TInt CAafAppCommentsView::CountComponentControls() const
{ 
	return 1; // returns number of controls inside the container
}

CCoeControl* CAafAppCommentsView::ComponentControl(TInt /*aIndex*/)  const 
{ 
	return iListBox;
}

TInt CAafAppCommentsView::GetItemsCount()
{
	if (iListBox)
		return iListBox->Model()->NumberOfItems();

	return 0;
}

void CAafAppCommentsView::GetComments()
{
	if (iQuestionsProvider)
	{
		/*
		if (iQuestionsProvider->GetStatus() == KSenConnectionStatusReady)
		{
		*/
			StartWaitDialog(R_WAITNOTE_BLOCKING);

			// Start upload process using appropriate service provider
			//TInt retValue = iQuestionsProvider->RequestForComments(this, iCurrentQuestion);
			TInt retValue = iQuestionsProvider->RequestForComments(iCurrentQuestion);

			StopWaitDialog();

			// Return value == KErrNone, this indicates that
			// question comments have been retrieved successfully
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

				HBufC* stringHolder = StringLoader::LoadL(R_COMMENTS_FAILURE_NOTE, eikonEnv );

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

TInt CAafAppCommentsView::CurrentItemIndex()
{
	return iListBox->CurrentItemIndex();
}

TKeyResponse CAafAppCommentsView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
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
void CAafAppCommentsView::HandleControlEventL(CCoeControl* aControl, TCoeEvent aEventType)
{
// Should be added later
}
*/
void CAafAppCommentsView::SetItemListL()
{
	if (iQuestionsProvider->GetCommentCount(iCurrentQuestion))
	{
		// Set the listbox to use the file list model
		CDesCArray* items = static_cast<CDesCArray*>(iListBox->Model()->ItemTextArray());

		// Delete all descriptors
		items->Reset();

		// Clear the listbox
		iListBox->HandleItemRemovalL();

		// Get comments list
		//iQuestionsProvider->GetCommentsListItemsL(items, 0);
		iQuestionsProvider->GetCommentListItems(iCurrentQuestion, items);

		// Refresh the listbox due to model change
		iListBox->HandleItemAdditionL();
		iListBox->SetCurrentItemIndex(0);

		iListBox->DrawNow();
	}
}

void CAafAppCommentsView::StartWaitDialog(TInt aDialogId)
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

	HBufC* stringHolder = StringLoader::LoadL(R_COMMENTS_WAITNOTE, eikonEnv );

	iWaitDialog->SetTextL(*stringHolder);

	iWaitDialog->ExecuteLD(aDialogId);

	delete stringHolder;
	stringHolder = NULL;
}

void CAafAppCommentsView::StopWaitDialog()
{
	// For wait dialog
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}	
}

void CAafAppCommentsView::DialogDismissedL(TInt /*aButtonId*/)
{
	// Cancel comments retrieving
	//iQuestionsProvider->Cancel();

	StopWaitDialog();
}

void CAafAppCommentsView::HandleResourceChange(TInt aType)
{
	if ( aType == KEikDynamicLayoutVariantSwitch )
	{
		SizeChanged();
	}

	CCoeControl::HandleResourceChange(aType);
}

void CAafAppCommentsView::HandleRequestCompletedL(const TInt& aError)
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

		HBufC* stringHolder = StringLoader::LoadL(R_COMMENTS_FAILURE_NOTE, eikonEnv );

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
TVwsViewId CAafAppCommentsView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp,  KQuestionCommentsViewId);
}

// Activates this view, called by framework
void CAafAppCommentsView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &aCustomMessage)
{
	iCurrentQuestion = -1;

	if (aCustomMessage.Length() > 0)
	{
		if (iViewParam)
		{
			delete iViewParam;
			iViewParam = NULL;
		}
		iViewParam = HBufC8::NewL(aCustomMessage.Length());
		iViewParam->Des().Copy(aCustomMessage);


		TLex8 orderIndex(iViewParam->Des());
		orderIndex.Val(iCurrentQuestion);
	}

	// Set listbox items
	SetItemListL();

	// If items list is empty,
	// try to get them form server
	if (iListBox->Model()->NumberOfItems() == 0)
	{
		GetComments();
	}	

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);

	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CAafAppCommentsView::ViewDeactivated()
{
	MakeVisible(EFalse);
}
// End of File
