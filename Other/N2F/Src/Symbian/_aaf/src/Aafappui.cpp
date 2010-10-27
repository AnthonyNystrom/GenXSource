/*
============================================================================
Name        : AafAppUi.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Main application UI class (controller)
============================================================================
*/

// INCLUDE FILES
#include <avkon.hrh>
#include <aknmessagequerydialog.h>
#include <aknnotewrappers.h>
#include <stringloader.h>
#include <f32file.h>
#include <w32std.h>
#include <s32file.h>
#include <bautils.h>
#include <charconv.h>
#include <hlplch.h>
#include <e32base.h>
#include <apgcli.h>
#include <hal.h>
#include <ecam.h>

#include <cpbksingleentryfetchdlg.h> 
#include <cpbkcontactengine.h>
#include <rpbkviewresourcefile.h>
#include <cpbkcontactitem.h> 
#include <tpbkcontactitemfield.h> 
#include <sendui.h>
#include <badesca.h>
#include <smut.h>
#include <txtrich.h>
#include <cmessagedata.h>

#include "Aaf.pan"
#include "Aaf.hrh"
#include "AafApplication.h"
#include "AafAppUi.h"
#include "AafAppLoginView.h"
#include "AafAppMainView.h"
#include "AafAppFileBrowserView.h"
#include "AafAppImageCartView.h"
#include "Aafappprivatequestionsview.h"
#include "aafuserquestionsserviceprovider.h"
#include "aafprivatequestionsserviceprovider.h"
#include "Aafappresponsesview.h"
#include "Aafappcommentsview.h"
#include "Aafloginserviceprovider.h"
#include "Aafuploadserviceprovider.h"
#include "Aafconnectionmanager.h"
#include "Aafsplashscreenview.h"


#include "Aafappaskquestionform.h"
#include "Aafappdataviewdialog.h"

#if USE_CAMERA
#include "Aafappcameraview.h"
#include "Aafcameraengine.h"
#endif

#include "common.h"

#include "Aaf_0xe2536f82.hlp.hrh"


// ============================ MEMBER FUNCTIONS ===============================


// Constructor
CSplashScreenTimer::CSplashScreenTimer( const TInt aPriority, MRequestObserver& aNotify)
: CTimer( aPriority ), iNotify( aNotify )
{

}

void CSplashScreenTimer::Start( TInt aTime )
{
	RDebug::Print(_L("Start the CSplashScreenTimer"));
	this->After(aTime);
}

// Destructor
CSplashScreenTimer::~CSplashScreenTimer()
{
}

void CSplashScreenTimer::ConstructL()
{
	CTimer::ConstructL();
	CActiveScheduler::Add( this );
}

CSplashScreenTimer* CSplashScreenTimer::NewL( const TInt aPriority,
											 MRequestObserver& aNotify )
{
	CSplashScreenTimer* self = CSplashScreenTimer::NewLC( aPriority, aNotify );
	CleanupStack::Pop( self );
	return self;
}

CSplashScreenTimer* CSplashScreenTimer::NewLC( const TInt aPriority, MRequestObserver& aNotify )
{
	CSplashScreenTimer* self = new (ELeave) CSplashScreenTimer( aPriority, aNotify );
	CleanupStack::PushL( self );
	self->ConstructL();
	return self;
}

void CSplashScreenTimer::RunL()
{
	RDebug::Print(_L("CSplashScreenTimer::RunL()"));

	if( iStatus == KErrNone )
	{
		iNotify.HandleRequestCompletedL(KErrNone);
	}
	else
	{
		User::Leave(iStatus.Int());
	}
}

TInt CSplashScreenTimer::RunError( TInt aError )
{
	iNotify.HandleRequestCompletedL(aError);

	return KErrNone;
}

// -----------------------------------------------------------------------------
// CAafAppUi::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppUi::ConstructL()
{	
	__LOGSTR_TOFILE("CAafAppUi::ConstructL() begins");

	RDebug::Print(_L("CAafAppUi::ConstructL()"));

#if USE_SKIN_SUPPORT
	// Initialize app UI with skin support
	BaseConstructL(EAknEnableSkin);
#else
	// Initialize app without skin support
	BaseConstructL();
#endif

	// Creating view instances
	__LOGSTR_TOFILE("CAafAppUi::ConstructL() before splash screen construction");

	// Creating splash screen view
	CWsScreenDevice& iScreen = *(iCoeEnv->ScreenDevice());
	TSize originalRes = iScreen.SizeInPixels();

	iAppSplashScreenView = CAafSplashScreenView::NewLC( TRect(TPoint(0, 0), originalRes) );

	__LOGSTR_TOFILE("CAafAppUi::ConstructL() after splash screen construction");

	// Registering splash screen view to view server
	RegisterViewL(*iAppSplashScreenView);

	// Adding splash screen view to app ui stack
	AddToStackL(*iAppSplashScreenView, iAppSplashScreenView);

	// Set default view,
	// in this case splash screen view
	SetDefaultViewL(*iAppSplashScreenView);
	CleanupStack::Pop(iAppSplashScreenView);

	// Set timer for showing splash screen - by default to 2 seconds
	TTimeIntervalMicroSeconds32 splashInterval = 2000000;

	iTimer = CSplashScreenTimer::NewL(EPriorityHigh, *this);

	iTimer->Start(splashInterval.Int());

	__LOGSTR_TOFILE("CAafAppUi::ConstructL() ends");
}
// -----------------------------------------------------------------------------
// CAafAppUi::CAafAppUi()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppUi::CAafAppUi()
{
	//
	iCurrentOperation = KNoneOperation;

	iCurrentPhoto = 0;
}

// -----------------------------------------------------------------------------
// CAafAppUi::~CAafAppUi()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppUi::~CAafAppUi()
{
	__LOGSTR_TOFILE("CAafAppUi::~CAafAppUi() begins");

	// Free dynamically allocated memory
	if (iAppLoginView)
	{
		RemoveFromStack(iAppLoginView);
		DeregisterView(*iAppLoginView);
		delete iAppLoginView;
		iAppLoginView = NULL;
	}

	if (iAppMainView)
	{
		RemoveFromStack(iAppMainView);
		DeregisterView(*iAppMainView);
		delete iAppMainView;
		iAppMainView = NULL;
	}

	if (iAppFileBrowserView)
	{
		RemoveFromStack(iAppFileBrowserView);
		DeregisterView(*iAppFileBrowserView);
		delete iAppFileBrowserView;
		iAppFileBrowserView = NULL;
	}
	
#if USE_CAMERA
	if (iAppCameraView)
	{
		RemoveFromStack(iAppCameraView);
		DeregisterView(*iAppCameraView);
		delete iAppCameraView;
		iAppCameraView = NULL;
	}
#endif

	if (iAppImageCartView)
	{
		RemoveFromStack(iAppImageCartView);
		DeregisterView(*iAppImageCartView);
		delete iAppImageCartView;
		iAppImageCartView = NULL;
	}

	if (iAppSplashScreenView)
	{
		RemoveFromStack(iAppSplashScreenView);
		DeregisterView(*iAppSplashScreenView);
		delete iAppSplashScreenView;
		iAppSplashScreenView = NULL;
	}
	
	if (iAppPQuestionsView)
	{
		RemoveFromStack(iAppPQuestionsView);
		DeregisterView(*iAppPQuestionsView);
		delete iAppPQuestionsView;
		iAppPQuestionsView = NULL;
	}

	if (iAppMyQuestionsView)
	{
		RemoveFromStack(iAppMyQuestionsView);
		DeregisterView(*iAppMyQuestionsView);
		delete iAppMyQuestionsView;
		iAppMyQuestionsView = NULL;
	}

	__LOGSTR_TOFILE("CAafAppUi::~CAafAppUi() ends");
}


void CAafAppUi::HandleRequestCompletedL(const TInt& aError)
{
	__LOGSTR_TOFILE("CAafAppUi::HandleRequestCompletedL() begins");

	if (iCurrentOperation == CAafAppUi::KLogging || iCurrentOperation == CAafAppUi::KNoneOperation)
	{
		CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();

		if (iCurrentOperation == CAafAppUi::KNoneOperation)
		{
			// Install internet connection if necessary
			CAafConnectionManager* connMng = CAafConnectionManager::GetInstanceL();

			if (connMng->GetIAPId() != 0)
			{
				// Install connection through selected IAP
				if (connMng->InstallConnectionL() == KErrNone)
				{
					// Set IAP id for the login service provider
					loginProvider->SetIAPL(connMng->GetIAPId());

					// Set IAP id for the images upload service provider
					CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

					uploadProvider->SetIAPL(connMng->GetIAPId());
				}
			}

			// Construct application views instances
			iAppMainView = CAafAppMainView::NewLC( ClientRect() );

			iAppLoginView = CAafAppLoginView::NewLC( ClientRect() );

			iAppFileBrowserView = CAafAppFileBrowserView::NewLC( ClientRect() );

#if USE_CAMERA
			if (CCaeEngine::CamerasAvailable() > 0)
				iAppCameraView = CAafAppCameraView::NewLC( ClientRect() );
#endif

			iAppImageCartView = CAafAppImageCartView::NewLC( ClientRect() );

			iAppPQuestionsView = CAafAppPQuestionsView::NewLC( ClientRect() );

			iAppMyQuestionsView = CAafAppMyQuestionsView::NewLC( ClientRect() );

			// Register application view instances
			RegisterViewL(*iAppMainView);

			RegisterViewL(*iAppLoginView);

			RegisterViewL(*iAppFileBrowserView);

#if USE_CAMERA
			if (CCaeEngine::CamerasAvailable() > 0)
				RegisterViewL(*iAppCameraView);
#endif

			RegisterViewL(*iAppImageCartView);

			RegisterViewL(*iAppPQuestionsView);

			RegisterViewL(*iAppMyQuestionsView);

			// Add to application ui stack
			AddToStackL(*iAppMainView, iAppMainView);
			CleanupStack::Pop(iAppMainView);

			AddToStackL(*iAppLoginView, iAppLoginView);
			CleanupStack::Pop(iAppLoginView);

			AddToStackL(*iAppFileBrowserView, iAppFileBrowserView);
			CleanupStack::Pop(iAppFileBrowserView);

#if USE_CAMERA
			if (CCaeEngine::CamerasAvailable() > 0)
			{
				AddToStackL(*iAppCameraView, iAppCameraView);
				CleanupStack::Pop(iAppCameraView);
			}
#endif

			AddToStackL(*iAppImageCartView, iAppImageCartView);
			CleanupStack::Pop(iAppImageCartView);

			AddToStackL(*iAppPQuestionsView, iAppPQuestionsView);
			CleanupStack::Pop(iAppPQuestionsView);

			AddToStackL(*iAppMyQuestionsView, iAppMyQuestionsView);
			CleanupStack::Pop(iAppMyQuestionsView);
		}

		// Show status pane
		StatusPane()->MakeVisible(ETrue);

		// Show control pane
		Cba()->MakeVisible( ETrue );

		// If credentials have been stored earlier,
		// open main view
		if (loginProvider->GetMemberID() && loginProvider->GetPassword() && loginProvider->GetUsername())
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KMainViewId));
		}
		// Otherwise,
		// open login view
		else
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KLoginViewId));
		}
		// Delete splash screen view
		if (iAppSplashScreenView)
		{
			RemoveFromStack(iAppSplashScreenView);
			DeregisterView(*iAppSplashScreenView);
			delete iAppSplashScreenView;
			iAppSplashScreenView = NULL;
		}

		// Show privacy statement if necessary
		PrivacyStatementL();

		if (iCurrentOperation == CAafAppUi::KNoneOperation)
		{
			CAafConnectionManager* connMng = CAafConnectionManager::GetInstanceL();

			// Install internet connection if necessary
			if (connMng->GetIAPId() == 0)
			{
				// Select IAP
				connMng->SelectIAPL();

				if (connMng->GetIAPId() != 0)
				{
					// Install connection through selected IAP
					if (connMng->InstallConnectionL() == KErrNone)
					{
						// Set IAP id for the login service provider
						loginProvider->SetIAPL(connMng->GetIAPId());

						// Set IAP id for the images upload service provider
						CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

						uploadProvider->SetIAPL(connMng->GetIAPId());
					}
				}
			}
		}		

		iCurrentOperation = CAafAppUi::KNoneOperation;
	}
	else if (iCurrentOperation == CAafAppUi::KSubmittingQuestion)
	{
		// In case of any error
		if (aError != KErrNone)
		{
			iCurrentPhoto == 0;
			iCurrentOperation = CAafAppUi::KNoneOperation;

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

			return;
		}

		// Complete upload process,
		// if all the photos have been uploaded already
		if (iCurrentPhoto == iPhotosCount)
		{
			iCurrentPhoto == 0;
			iCurrentOperation = CAafAppUi::KNoneOperation;

			StopWaitDialog();

			return;
		}

		// Attach photos to the question
		//CAafMyQuestionsServiceProvider* questionProvider = CAafMyQuestionsServiceProvider::InstanceL();
		CAafUserQuestionsProvider* questionProvider = CAafUserQuestionsProvider::GetInstanceL();
		CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

		HBufC8* questionId = questionProvider->GetSubmittedQuestionId();

		uploadProvider->StartUploadL(this, uploadProvider->GetSelectionArray()[iCurrentPhoto], *questionId);

		CleanupStack::PopAndDestroy(questionId);

		++iCurrentPhoto;		
	}	

	__LOGSTR_TOFILE("CAafAppUi::HandleRequestCompletedL() ends");
}

void CAafAppUi::StartWaitDialog(TInt aDialogId)
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

	iWaitDialog->ExecuteLD(aDialogId);

	delete stringHolder;
	stringHolder = NULL;
}

void CAafAppUi::StopWaitDialog()
{
	// For wait dialog
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}	
}

void CAafAppUi::DialogDismissedL(TInt /*aButtonId*/)
{
	// Cancel upload process
	if (iCurrentOperation == CAafAppUi::KSubmittingQuestion)
	{
		//CAafUserQuestionsProvider* questionProvider = CAafUserQuestionsProvider::GetInstanceL();

		//questionProvider->Cancel();
	}
	else
	{
		CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

		uploadProvider->CancelUpload();
	}
	

	StopWaitDialog();
}

void CAafAppUi::DynInitMenuPaneL(TInt aResourceId, CEikMenuPane* aMenuPane)
{
	__LOGSTR_TOFILE1("CAafAppUi::DynInitMenuPaneL() begins with aResourceId == %d", aResourceId);

	switch(aResourceId)
	{
		// File browser menu pane
	case R_AAF_FILEBROWSER_MENU:
		{
			if (iAppFileBrowserView)
			{
				if (iAppFileBrowserView->IsCurrentItemDir())
				{
					aMenuPane->SetItemDimmed(EFileBrowserAddtoUpload, ETrue);
					aMenuPane->SetItemDimmed(EFileBrowserPreview, ETrue);
					aMenuPane->SetItemDimmed(EFileBrowserOpen, EFalse);
					aMenuPane->SetItemDimmed(EFileBrowserFileProperties, ETrue);
				}
				else
				{
					aMenuPane->SetItemDimmed(EFileBrowserAddtoUpload, EFalse);
					aMenuPane->SetItemDimmed(EFileBrowserPreview, EFalse);
					aMenuPane->SetItemDimmed(EFileBrowserOpen, ETrue);
					aMenuPane->SetItemDimmed(EFileBrowserFileProperties, EFalse);
				}
			}
		}
		break;
		// Image cart menu pane
	case R_AAF_UPLOAD_IMAGE_CART_MENU:
		{
			if (iAppImageCartView)
			{
				if (iAppImageCartView->GetItemsCount() == 0)
				{
					aMenuPane->SetItemDimmed(EAskQuestion, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartSelect, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartDeselect, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartPreview, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartRemoveSelected, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartRemoveAll, ETrue);
				}
				else
				{
					CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

					if (uploadProvider->GetSelectionArray().Count() > 0)
					{
						aMenuPane->SetItemDimmed(EAskQuestion, EFalse);
					}
					else
					{
						aMenuPane->SetItemDimmed(EAskQuestion, ETrue);
					}

					// If current item is selected
					// show 'Deselect' menu item and hide 'Select'
					if (iAppImageCartView->IsCurrentItemMarked())
					{
						aMenuPane->SetItemDimmed(EUploadImageCartDeselect, EFalse);
						aMenuPane->SetItemDimmed(EUploadImageCartSelect, ETrue);
					}
					// otherwise
					else
					{
						aMenuPane->SetItemDimmed(EUploadImageCartDeselect, ETrue);
						aMenuPane->SetItemDimmed(EUploadImageCartSelect, EFalse);
					}

					aMenuPane->SetItemDimmed(EUploadImageCartPreview, EFalse);
					aMenuPane->SetItemDimmed(EUploadImageCartRemoveSelected, EFalse);
					aMenuPane->SetItemDimmed(EUploadImageCartRemoveAll, EFalse);
				}
			}
		}
		break;
	case R_AAF_MY_QUESTIONS_MENU:
		{
			CAafUserQuestionsProvider* provider = CAafUserQuestionsProvider::GetInstanceL();

			// If question list is empty,
			// hide "Details" and "Comments" items
			if (!provider->GetQuestionCount())
			{
				aMenuPane->SetItemDimmed(EMyQuestionDetails, ETrue);
				aMenuPane->SetItemDimmed(EMyQuestionComments, ETrue);
			}
			else
			{
				aMenuPane->SetItemDimmed(EMyQuestionDetails, EFalse);
				aMenuPane->SetItemDimmed(EMyQuestionComments, EFalse);
			}
		}
		break;
	case R_AAF_QUESTION_COMMENTS_MENU:
		{
			if (iAppQuestionCommentsView)
			{
				CAafUserQuestionsProvider* provider = CAafUserQuestionsProvider::GetInstanceL();

				// If question list is empty
				// hide "Details" item
				if (!provider->GetCommentCount(iAppQuestionCommentsView->CurrentItemIndex()))
				{
					aMenuPane->SetItemDimmed(EQuestionCommentDetails, ETrue);
				}
				else
				{
					aMenuPane->SetItemDimmed(EQuestionCommentDetails, EFalse);
				}
			}
		}
		break;
	case R_AAF_PRIVATE_QUESTIONS_MENU:
		{
			CAafPrivateQuestionsProvider* provider = CAafPrivateQuestionsProvider::GetInstanceL();

			// If question list is empty
			// hide "Details" item
			if (!provider->GetQuestionCount())
			{
				aMenuPane->SetItemDimmed(EPQuestionDetails, ETrue);
			}
			else
			{
				aMenuPane->SetItemDimmed(EPQuestionDetails, EFalse);
			}
		}
		break;
#if USE_CAMERA
	case R_AAF_CAMERA_MENU:
		{
			if (iAppCameraView)
			{
				// Show "Save photo" menu item if photo has been captured already
				if (iAppCameraView->IsImageCaptured())
				{
					aMenuPane->SetItemDimmed(EMakePhotoCapture, ETrue);
					aMenuPane->SetItemDimmed(EMakePhotoZoomIn, ETrue);
					aMenuPane->SetItemDimmed(EMakePhotoZoomOut, ETrue);
					aMenuPane->SetItemDimmed(EMakePhotoNew, EFalse);
					aMenuPane->SetItemDimmed(EMakePhotoSave, EFalse);
				}
				// Hide "Save photo" menu item if photo hasn't been captured yet
				else
				{
					aMenuPane->SetItemDimmed(EMakePhotoCapture, EFalse);
					
					// Hide or show zoom menu items (depends on current zoom value)
					if (iAppCameraView->GetZoomValue() < iAppCameraView->GetMaxZoomValue())
						aMenuPane->SetItemDimmed(EMakePhotoZoomIn, EFalse);
					else
						aMenuPane->SetItemDimmed(EMakePhotoZoomIn, ETrue);

					if (iAppCameraView->GetZoomValue() > 0)
						aMenuPane->SetItemDimmed(EMakePhotoZoomOut, EFalse);
					else
						aMenuPane->SetItemDimmed(EMakePhotoZoomOut, ETrue);

					aMenuPane->SetItemDimmed(EMakePhotoNew, ETrue);
					aMenuPane->SetItemDimmed(EMakePhotoSave, ETrue);
				}

				if (CCaeEngine::CamerasAvailable() > 1)
				{
					if (iAppCameraView->ActiveCamera() == KMainCamera)
					{
						aMenuPane->SetItemDimmed(EUseMainCamera, ETrue);
						aMenuPane->SetItemDimmed(EUseSecondaryCamera, EFalse);
					}
					else
					{
						aMenuPane->SetItemDimmed(EUseMainCamera, EFalse);
						aMenuPane->SetItemDimmed(EUseSecondaryCamera, ETrue);
					}
				}
				else
				{
					aMenuPane->SetItemDimmed(EUseMainCamera, ETrue);
					aMenuPane->SetItemDimmed(EUseSecondaryCamera, ETrue);
				}
			}
		}
		break;
	case R_RESOLUTIONS_LIST_MENU:
		{
			if (iAppCameraView)
			{
				// Set mark icon to the appropriate menu item
				EPhotoResolution curResolution = iAppCameraView->GetCurrentResolution();

				switch(curResolution)
				{
				case KRes640x480:
					{
						aMenuPane->SetItemButtonState(EMakePhotoRes640x480, EEikMenuItemSymbolOn);
					}
					break;
				case KRes480x320:
					{
						aMenuPane->SetItemButtonState(EMakePhotoRes480x320, EEikMenuItemSymbolOn);
					}
					break;
				case KRes320x240:
					{
						aMenuPane->SetItemButtonState(EMakePhotoRes320x240, EEikMenuItemSymbolOn);
					}
					break;
				}
			}
		}
		break;
#endif

	default:
		break;
	}

	__LOGSTR_TOFILE("CAafAppUi::DynInitMenuPaneL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppUi::HandleCommandL()
// Takes care of command handling.
// -----------------------------------------------------------------------------
//
void CAafAppUi::HandleCommandL( TInt aCommand )
{
	__LOGSTR_TOFILE1("CAafAppUi::HandleCommandL() begins with aCommand == %d", aCommand);

	switch( aCommand )
	{
	case EEikCmdExit:
	case EAknSoftkeyExit:
			{
				// Serialize image cart data
				CAafUploadServiceProvider* provider = CAafUploadServiceProvider::GetInstanceL();
				provider->SerializeDataL();
				
				// Exit the application
				Exit();
			}
			break;
		// Go to main view
	case EAafSoftkeyContinue:
		{
			if (iAppLoginView)
			{
				iCurrentOperation = KLogging;
				iAppLoginView->DoLogin();
			}
		}
		break;
		// Go back to main view
	case EAknSoftkeyCancel:
	case EAknSoftkeyBack:
		{
			// add later necessary code
			ActivateViewL(TVwsViewId(KUidViewSupApp, KMainViewId));
		}
		break;
	case EAknSoftkeyEmpty:
		{
			// Do nothing
		}
		break;
		// Main view listbox items
	case EAknSoftkeySelect:
		{
			// Recall current method with new command id
			HandleCommandL(iAppMainView->GetKeyEventCode());
		}
		break;
	case EAskQuestion:
		{
			CAafQuestionForm* questionForm = CAafQuestionForm::NewL();

			questionForm->ExecuteLD(R_ASK_QUESTION_FORM_DIALOG);
			//CAknForm* questionForm = new (ELeave)CAknForm();
			//questionForm->ConstructL(R_QUESTION_FORM_MENUBAR);
			//questionForm->ExecuteLD(R_ASK_QUESTION_FORM_DIALOG);
		}
		break;
	case EAskQuestionSubmit:
		{
			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL() begins with commad id == EAskQuestionSubmit");

			//CAafMyQuestionsServiceProvider* questionProvider = CAafMyQuestionsServiceProvider::InstanceL();
			CAafUserQuestionsProvider* questionProvider = CAafUserQuestionsProvider::GetInstanceL();
			CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL()with commad id == EAskQuestionSubmit before getting photos count");

			//iPhotosCount = uploadProvider->GetSelectionArray().Count();
			
			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL()with commad id == EAskQuestionSubmit after getting photos count");

			// If none question is selected,
			// show warning note and return
			//if (iPhotosCount <= 0)
			if (uploadProvider->GetSelectionArray().Count() <= 0)
			{
				CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

				// Get CCoeEnv instance
				CEikonEnv* eikonEnv = CEikonEnv::Static();

				HBufC* stringHolder = StringLoader::LoadL(R_UPLOAD_NOIMAGE_NOTE, eikonEnv );

				dialog->SetTextL(*stringHolder);

				if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
				{

				}

				delete stringHolder;
				stringHolder = NULL;

				return;
			}

			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL()with commad id == EAskQuestionSubmit before SubmitQuestionL()");

			iCurrentOperation = KSubmittingQuestion;
			//questionProvider->SubmitQuestionL(this, iPhotosCount);
			
			StartWaitDialog(R_WAITNOTE_BLOCKING);

			TInt retValue = questionProvider->SubmitQuestion();
				
			HandleRequestCompletedL(retValue);
		}
		break;
#if USE_CAMERA
	case EMakePhoto:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KCameraViewId));
		}
		break;
	case EMakePhotoCapture:
		{
			if (iAppCameraView)
			{
				iAppCameraView->CaptureImageL();
			}
		}
		break;
	case EUseMainCamera:
		{
			if (iAppCameraView)
			{
				iAppCameraView->ChangeActiveCamera(KMainCamera);
			}
		}
		break;
	case EUseSecondaryCamera:
		{
			if (iAppCameraView)
			{
				iAppCameraView->ChangeActiveCamera(KSecondaryCamera);
			}
		}
		break;
	case EMakePhotoZoomIn:
		{
			if (iAppCameraView)
			{
				iAppCameraView->ZoomIn();
			}
		}
		break;
	case EMakePhotoZoomOut:
		{
			if (iAppCameraView)
			{
				iAppCameraView->ZoomOut();
			}

		}
		break;
	case EMakePhotoNew:
		{
			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL() begins with aCommand == EMakePhotoNew");

			if (iAppCameraView)
			{
				iAppCameraView->StartViewFinderL();
			}
		}
		break;
	case EMakePhotoRes640x480:
		{
			if (iAppCameraView)
			{
				iAppCameraView->SetResolution(KRes640x480);
			}
		}
		break;
	case EMakePhotoRes480x320:
		{
			if (iAppCameraView)
			{
				iAppCameraView->SetResolution(KRes480x320);
			}
		}
		break;
	case EMakePhotoRes320x240:
		{
			if (iAppCameraView)
			{
				iAppCameraView->SetResolution(KRes320x240);
			}
		}
		break;
	
	case EMakePhotoSave:
		{
			if (iAppCameraView)
			{
				iAppCameraView->SaveCapturedImageL();
			}
		}
		break;
#endif

	case EFileBrowserView:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KFileBrowserViewId));
		}
		break;
	case EImageCartView:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KImageCartViewId));
		}
		break;
	case EPrivateQuestions:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KPQuestionsViewId));
		}
		break;
	case EResponses:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KMyQuestionsViewId));
		}
		break;
	case EPQuestionsRefresh:
		{
			if (iAppPQuestionsView)
			{
				iAppPQuestionsView->GetQuestions();
			}
		}
		break;
	case EPQuestionDetails:
		{
			CAafAppDataViewDialog* viewDialog = CAafAppDataViewDialog::ConstructL(EPrivateQuestion);

			viewDialog->ShowLD();
		}
		break;
	case EMyQuestionsRefresh:
		{
			if (iAppMyQuestionsView)
			{
				iAppMyQuestionsView->GetQuestions();
			}
		}
		break;
	case EMyQuestionDetails:
		{
			CAafAppDataViewDialog* viewDialog = CAafAppDataViewDialog::ConstructL(EUserQuestion);

			viewDialog->ShowLD();
		}
		break;
	case EMyQuestionComments:
		{
			// Get currently selected question order index
			TInt questionIndex = -1;

			if (iAppMyQuestionsView)
			{
				questionIndex = iAppMyQuestionsView->CurrentItemIndex();
			}

			TBuf8<3> orderIndex;
			orderIndex.AppendNum(questionIndex);

			// Activate comments view (passing to it question index)
			ActivateViewL(TVwsViewId(KUidViewSupApp, KMyQuestionsViewId), TUid::Uid(1), orderIndex);
		}
		break;
	case EQuestionCommentsRefresh:
		{
			if (iAppQuestionCommentsView)
			{
				iAppQuestionCommentsView->GetComments();
			}
		}
		break;
	case EQuestionCommentDetails:
		{
			CAafAppDataViewDialog* viewDialog = CAafAppDataViewDialog::ConstructL(EQuestionComment);

			viewDialog->ShowLD();
		}
		break;
	case ECredentialsView:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KLoginViewId));
		}
		break;
	case EInviteFriend:
		{
			InviteFriendDialog();		
		}
		break;
		// "File Browser" view menu items
	case EFileBrowserGotoUpload:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KImageCartViewId));
		}
		break;
	case EFileBrowserAddtoUpload:
		{
			// Add selected image to upload cart
			if (iAppFileBrowserView)
				iAppFileBrowserView->AddImageToUpload();
		}
		break;
	case EFileBrowserOpen:
	case EFileBrowserPreview:
		{
			// Launch the application associated with the selected file
			// or open the folder
			if (iAppFileBrowserView)
				iAppFileBrowserView->LaunchCurrentL();
		}
		break;
	case EFileBrowserFileProperties:
		{
			if (iAppFileBrowserView)
				iAppFileBrowserView->ShowFileProperties();
		}
		break;
	// "Upload Image Card" view menu items
	case EUploadImageCartFileBrowser:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KFileBrowserViewId));
		}
		break;
	case EUploadImageCartPreview:
		{
			iAppImageCartView->PreviewCurrentItemL();
		}
		break;
	case EUploadImageCartDeselect:
	case EUploadImageCartSelect:
		{
			iAppImageCartView->CommandMark();
		}
		break;
	case EUploadImageCartRemoveSelected:
		{
			iAppImageCartView->RemoveSelectedItems();
		}
		break;
	case EUploadImageCartRemoveAll:
		{
			iAppImageCartView->RemoveAllItems();
		}
		break;
	case ESetIAP:
		{
			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL() begins with command id == ESetIAP");

			CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();
			CAafConnectionManager* connMng = CAafConnectionManager::GetInstanceL();

			// Select IAP
			connMng->SelectIAPL();

			if (connMng->GetIAPId() != 0)
			{
				// Install connection through selected IAP
				if (connMng->InstallConnectionL() == KErrNone)
				{
					// Set IAP id for the login service provider
					loginProvider->SetIAPL(connMng->GetIAPId());

					// Set IAP id for the images upload service provider
					CAafUploadServiceProvider* uploadProvider = CAafUploadServiceProvider::GetInstanceL();

					uploadProvider->SetIAPL(connMng->GetIAPId());
				}
			}

			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL() ends with command id == ESetIAP");
		}
		break;
	case EHelp:
		{
			CArrayFix<TCoeHelpContext>* buf = CCoeAppUi::AppHelpContextL();
			HlpLauncher::LaunchHelpApplicationL(iEikonEnv->WsSession(), buf);
		}
		break;
	case EAbout:
		{
			CAknMessageQueryDialog* dlg = new (ELeave)CAknMessageQueryDialog(); 
			dlg->PrepareLC(R_ABOUT_QUERY_DIALOG);
			HBufC* title = iEikonEnv->AllocReadResourceLC(R_ABOUT_DIALOG_TITLE);
			dlg->QueryHeading()->SetTextL(*title);
			CleanupStack::PopAndDestroy(); //title
			HBufC* msg = iEikonEnv->AllocReadResourceLC(R_ABOUT_DIALOG_TEXT);
			dlg->SetMessageTextL(*msg);
			CleanupStack::PopAndDestroy(); //msg
			dlg->RunLD(); 
		}
		break;

	default:
		{
			__LOGSTR_TOFILE("CAafAppUi::HandleCommandL() panic == EAafUi");

			Panic( EAafUi );
		}
		break;
	}

	__LOGSTR_TOFILE("CAafAppUi::HandleCommandL() ends");
}
// -----------------------------------------------------------------------------
//  Called by the framework when the application status pane
//  size is changed.  Passes the new client rectangle to the
//  AppView
// -----------------------------------------------------------------------------
//
void CAafAppUi::HandleStatusPaneSizeChange()
{
	iAppLoginView->SetRect( ClientRect() );

}

CArrayFix<TCoeHelpContext>* CAafAppUi::HelpContextL() const
{
	// Note: help will not work if the application uid3 is not in the
	// protected range.  The default uid3 range for projects created
	// from this template (0xE0000000 - 0xEFFFFFFF) are not in the protected range so they
	// can be self signed and installed on the device during testing.
	// Once you get your official uid3 from Symbian Ltd. and find/replace
	// all occurrences of uid3 in your project, the context help will
	// work.
	CArrayFixFlat<TCoeHelpContext>* ctxs = new(ELeave)CArrayFixFlat<TCoeHelpContext>(1);
	CleanupStack::PushL(ctxs);
	ctxs->AppendL(TCoeHelpContext(KUidAafApp, KGeneral_Information));
	CleanupStack::Pop(ctxs);
	return ctxs;
}

void CAafAppUi::PrivacyStatementL() 
{
	// Note: to see privacy statement in emulator, copy
	// PrivacyStatement.txt from group/ directory to 
	// %EPOCROOT%\Epoc32\release\winscw\udeb\z\private\e727d056\
	// Also note that on emulator you can't modify files on Z: drive,
	// so even if you answer Yes to privacy statement, it will popup
	// dialog every time you launch application. But on device it will work
	// as expected.
	HBufC * fileName = StringLoader::LoadLC(R_PRIVACY_STATEMENT_FILENAME);
	RFs &fs= iCoeEnv->FsSession();

	// Make full path to privacy statement
	TFileName fullFileName;
	TFileName privatePath;
	fs.PrivatePath(privatePath);
	TParse parser;
	TFileName processFileName(RProcess().FileName());
	User::LeaveIfError(parser.Set(*fileName, &privatePath, &processFileName));
	fullFileName = parser.FullName();
	CleanupStack::PopAndDestroy(fileName);

	if(BaflUtils::FileExists(fs, fullFileName)) 
	{
		RBuf text(CAafAppUi::ReadUnicodeFileL(fs, fullFileName));
		CleanupClosePushL(text);
		CAknMessageQueryDialog *dialog = CAknMessageQueryDialog::NewL(text);
		if(dialog->ExecuteLD(R_PRIVSTMT_DIALOG) == EAknSoftkeyYes) 
		{
			BaflUtils::DeleteFile(fs, fullFileName);
		}
		CleanupStack::PopAndDestroy(&text);
	}
}

HBufC * CAafAppUi::ReadUnicodeFileL(RFs& aFs, const TDesC& aFileName)
{
	RFile file;
	User::LeaveIfError(file.Open(aFs, aFileName, EFileShareReadersOnly | EFileStreamText | EFileRead));
	CleanupClosePushL(file);

	TInt size;
	User::LeaveIfError(file.Size(size));

	RBuf8 tmp;
	tmp.CreateL(size);
	CleanupClosePushL(tmp);
	User::LeaveIfError(file.Read(tmp));

	CCnvCharacterSetConverter * converter = CCnvCharacterSetConverter::NewLC();
	converter->PrepareToConvertToOrFromL(KCharacterSetIdentifierUtf8, aFs);

	HBufC *text = HBufC::NewL(size);

	TInt state = CCnvCharacterSetConverter::KStateDefault;
	TPtrC8 remainderOfForeignText(tmp);
	for(;;)
	{
		TPtr textPtr(text->Des());
		TInt retValue = converter->ConvertToUnicode(textPtr, remainderOfForeignText, state);
		if(retValue == CCnvCharacterSetConverter::EErrorIllFormedInput)
			User::Leave(KErrCorrupt);
		else if(retValue < 0)
			User::Leave(KErrGeneral);

		if(retValue == 0)
			break;

		remainderOfForeignText.Set(remainderOfForeignText.Right(retValue));
	}	

	CleanupStack::PopAndDestroy(converter);
	CleanupStack::PopAndDestroy(2);
	return text;
}

void CAafAppUi::InviteFriendDialog()
{
	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() begins");

	RPbkViewResourceFile phonebookResource( *(CEikonEnv::Static()) );
	phonebookResource.OpenL();

	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 01");

	CPbkSingleEntryFetchDlg::TParams params;

	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 011");

	CPbkContactEngine* iPbkContactEngine = CPbkContactEngine::NewL(&iEikonEnv->FsSession());

	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 0111");

	CleanupStack::PushL(iPbkContactEngine);
	params.iPbkEngine = iPbkContactEngine;

	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 012");

	CPbkSingleEntryFetchDlg* fetcher = CPbkSingleEntryFetchDlg::NewL(params);
	fetcher->SetMopParent(this);

	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 02");

	// Just show the dialog
	TInt retValue = fetcher->ExecuteLD();

	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 03");

	if (retValue)
	{
		__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 04");

		const TContactItemId contactId = params.iSelectedEntry;//get the id of the selected user

		// Read contact details
		// Open the selected contact using phonebook engine,
		// choose correct number (launch list query if needed)
		CPbkContactItem* pbkItem = iPbkContactEngine->ReadContactLC( contactId );
		//TPbkContactItemField* phoneField;

		// Get array of fields for the current contact
		//CPbkFieldArray fieldsArray = pbkItem->CardFields();
		
		__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 05");

		// Create mobile phones array with granularity 5
		CDesCArrayFlat* mobilePhonesArray = new (ELeave)CDesCArrayFlat(5);
		CleanupStack::PushL(mobilePhonesArray);

		__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 06");

		// Get array of mobile phone numbers
		for (TInt i = 0; i < pbkItem->CardFields().Count(); i++)
		{
			if ((pbkItem->CardFields())[i].PbkFieldId() == EPbkFieldIdPhoneNumberMobile)
			{
				mobilePhonesArray->AppendL((pbkItem->CardFields())[i].Text());
			}
		}

		__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 07");

		if (mobilePhonesArray->Count() > 0)
		{
			__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 08");

			TInt selectedIndex;

			// If several mobile phone numbers are available
			// show list query dialog for choosing one number
			if (mobilePhonesArray->Count() > 1)
			{
				__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 09");

				selectedIndex = 0;

				// If not "Ok" button has been pressed
				// just return
				if (!ShowQueryDialogL(mobilePhonesArray, selectedIndex))
					return;
			}
			// Otherwise
			else if (mobilePhonesArray->Count() == 1)
			{
				selectedIndex = 0;
			}

			__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 10");

			TBuf<30> phoneNumber;
			phoneNumber.Copy((*mobilePhonesArray)[selectedIndex]);

			__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 11");

			if(phoneNumber.Length() > 0)
			{
				__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() step 12");

				// Show sms editor dialog
				CSendUi* sendAppUi = CSendUi::NewL();
				CRichText* richText = CRichText::NewL(iEikonEnv->SystemParaFormatLayerL() ,iEikonEnv->SystemCharFormatLayerL());

				// Set the text into the message body
				HBufC* bodyText = StringLoader::LoadLC(R_SMS_BODY_TEXT);
				richText->InsertL(0, *bodyText);
				CleanupStack::PopAndDestroy(bodyText);
				bodyText = NULL;


				CMessageData* messageData = CMessageData::NewL();
				CleanupStack::PushL(messageData);

				// Get Contact name and surname
				// Get contact title
				HBufC* contactTitle = iPbkContactEngine->GetContactTitleL(*pbkItem);
				CleanupStack::PushL(contactTitle);

				// Append recipient mobile phone number with name and surname as alias
				messageData->AppendToAddressL(phoneNumber, *contactTitle);
				
				CleanupStack::PopAndDestroy(contactTitle);
				//contactTitle = NULL;

				messageData->SetBodyTextL(richText);

				// Opens the default editor with phone number and text body.
				sendAppUi->CreateAndSendMessageL(KUidMsgTypeSMS, messageData, KNullUid, EFalse);

				CleanupStack::PopAndDestroy(messageData);

			}
		}
		// Just open sms editor window and show warning note
		else
		{
			// Show sms editor dialog
			CSendUi* sendAppUi = CSendUi::NewL();
			CRichText* richText = CRichText::NewL(iEikonEnv->SystemParaFormatLayerL() ,iEikonEnv->SystemCharFormatLayerL());

			// Set the text into the message body
			HBufC* bodyText = StringLoader::LoadLC(R_SMS_BODY_TEXT);
			richText->InsertL(0, *bodyText);
			CleanupStack::PopAndDestroy(bodyText);
			bodyText = NULL;


			CMessageData* messageData = CMessageData::NewL();
			CleanupStack::PushL(messageData);

			messageData->SetBodyTextL(richText);
			messageData->AppendToAddressL(_L("no mobile number"));

			// Opens the default editor with phone number and text body.
			sendAppUi->CreateAndSendMessageL(KUidMsgTypeSMS, messageData, KNullUid, EFalse);

			CleanupStack::PopAndDestroy(messageData);

		}

		CleanupStack::PopAndDestroy(mobilePhonesArray);
		CleanupStack::PopAndDestroy(pbkItem);
	}			

	//CPbkContactEngine, CPbkMultipleEntryFetchDlg::TParams
	CleanupStack::PopAndDestroy();
	phonebookResource.Close();

	__LOGSTR_TOFILE("CAafAppUi::InviteFriendDialog() ends");
}

TInt CAafAppUi::ShowQueryDialogL(CDesCArrayFlat* aOptionsArrray, TInt &aSelectedOption)
{
	CAknListQueryDialog* dialog = new(ELeave)CAknListQueryDialog(&aSelectedOption);
	dialog->PrepareLC(R_QUERY_DIALOG);

	dialog->SetItemTextArray(aOptionsArrray); 
	dialog->SetOwnershipType(ELbmDoesNotOwnItemArray); 

	TInt retValue = dialog->RunLD();

	return retValue;
}

// End of File
