/*
============================================================================
Name        : SupAppUi.cpp
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

#include "Sup.pan"
#include "SupApplication.h"
#include "SupAppUi.h"
#include "SupAppLoginView.h"
#include "SupAppMainView.h"
#include "SupAppFileBrowserView.h"
#include "SupAppImageCartView.h"
#include "Suploginserviceprovider.h"
#include "Supconnectionmanager.h"
#include "Supuploadserviceprovider.h"
#include "Supsplashscreenview.h"
#include "sup.rsg"
#include "Sup.hrh"
#include "common.h"

#include "Sup_0xeb83db8a.hlp.hrh"
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
// CSupAppUi::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CSupAppUi::ConstructL()
{	
	__LOGSTR_TOFILE("CSupAppUi::ConstructL() begins");

	RDebug::Print(_L("CSupAppUi::ConstructL()"));

#if USE_SKIN_SUPPORT
	// Initialize app UI with skin support
	BaseConstructL(EAknEnableSkin);
#else
	// Initialize app without skin support
	BaseConstructL();
#endif

	// Creating view instances
	/*
	iAppMainView = CSupAppMainView::NewL( ClientRect() );

	iAppLoginView = CSupAppLoginView::NewL( ClientRect() );

	iAppFileBrowserView = CSupAppFileBrowserView::NewL( ClientRect() );

	iAppImageCartView = CSupAppImageCartView::NewL( ClientRect() );
	*/

	// Creating splash screen view
	CWsScreenDevice& iScreen = *(iCoeEnv->ScreenDevice());
	TSize originalRes = iScreen.SizeInPixels();

	iAppSplashScreenView = CSupSplashScreenView::NewL( TRect(TPoint(0, 0), originalRes) );

	// Initialization other views
	/*
	RegisterViewL(*iAppMainView);

	RegisterViewL(*iAppLoginView);

	RegisterViewL(*iAppFileBrowserView);

	RegisterViewL(*iAppImageCartView);
	*/

	// Registering views to view server
	RegisterViewL(*iAppSplashScreenView);


	// Adding to app ui stack other views
	/*
	AddToStackL(*iAppMainView, iAppMainView);

	AddToStackL(*iAppLoginView, iAppLoginView);

	AddToStackL(*iAppFileBrowserView, iAppFileBrowserView);

	AddToStackL(*iAppImageCartView, iAppImageCartView);
	*/

	// Adding views to app ui stack
	AddToStackL(*iAppSplashScreenView, iAppSplashScreenView);

	// Set default view,
	// in this case splash screen view
	SetDefaultViewL(*iAppSplashScreenView);

	// Set timer for showing splash screen - by default to 2 seconds
	TTimeIntervalMicroSeconds32 splashInterval = 2000000;

	iTimer = CSplashScreenTimer::NewL(EPriorityHigh, *this);

	iTimer->Start(splashInterval.Int());

	__LOGSTR_TOFILE("CSupAppUi::ConstructL() ends");
}
// -----------------------------------------------------------------------------
// CSupAppUi::CSupAppUi()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CSupAppUi::CSupAppUi()
{
	//
	iSplashScreenShown = ETrue;
}

// -----------------------------------------------------------------------------
// CSupAppUi::~CSupAppUi()
// Destructor.
// -----------------------------------------------------------------------------
//
CSupAppUi::~CSupAppUi()
{
	__LOGSTR_TOFILE("CSupAppUi::~CSupAppUi() begins");

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

	__LOGSTR_TOFILE("CSupAppUi::~CSupAppUi() ends");
}


void CSupAppUi::HandleRequestCompletedL(const TInt& /*aError*/)
{
	__LOGSTR_TOFILE("CSupAppUi::HandleRequestCompletedL() begins");

	CSupLoginServiceProvider* loginProvider = CSupLoginServiceProvider::InstanceL();

	if (iSplashScreenShown)
	{
		// Install internet connection if necessary
		CSupConnectionManager* connMng = CSupConnectionManager::GetInstanceL();

		if (connMng->GetIAPId() != 0)
		{
			// Install connection through selected IAP
			if (connMng->InstallConnectionL() == KErrNone)
			{
				// Set IAP id for the login service provider
				loginProvider->SetIAPL(connMng->GetIAPId());

				// Set IAP id for the images upload service provider
				CSupUploadServiceProvider* uploadProvider = CSupUploadServiceProvider::InstanceL();

				uploadProvider->SetIAPL(connMng->GetIAPId());
			}
		}
	}

	// Construct application views instances
	iAppMainView = CSupAppMainView::NewLC( ClientRect() );

	iAppLoginView = CSupAppLoginView::NewLC( ClientRect() );

	iAppFileBrowserView = CSupAppFileBrowserView::NewLC( ClientRect() );

	iAppImageCartView = CSupAppImageCartView::NewLC( ClientRect() );

	// Register application view instances
	RegisterViewL(*iAppMainView);

	RegisterViewL(*iAppLoginView);

	RegisterViewL(*iAppFileBrowserView);

	RegisterViewL(*iAppImageCartView);

	// Add to application ui stack
	AddToStackL(*iAppMainView, iAppMainView);
	CleanupStack::Pop(iAppMainView);

	AddToStackL(*iAppLoginView, iAppLoginView);
	CleanupStack::Pop(iAppLoginView);

	AddToStackL(*iAppFileBrowserView, iAppFileBrowserView);
	CleanupStack::Pop(iAppFileBrowserView);

	AddToStackL(*iAppImageCartView, iAppImageCartView);
	CleanupStack::Pop(iAppImageCartView);

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

	if (iSplashScreenShown)
	{
		CSupConnectionManager* connMng = CSupConnectionManager::GetInstanceL();

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
					CSupUploadServiceProvider* uploadProvider = CSupUploadServiceProvider::InstanceL();

					uploadProvider->SetIAPL(connMng->GetIAPId());
				}
			}
		}

		iSplashScreenShown = EFalse;
	}

	__LOGSTR_TOFILE("CSupAppUi::HandleRequestCompletedL() ends");
}

void CSupAppUi::DynInitMenuPaneL(TInt aResourceId, CEikMenuPane* aMenuPane)
{
	__LOGSTR_TOFILE1("CSupAppUi::DynInitMenuPaneL() begins with aResourceId == %d", aResourceId);

	switch(aResourceId)
	{
		// File browser menu pane
	case R_SUP_FILEBROWSER_MENU:
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
	case R_SUP_UPLOAD_IMAGE_CART_MENU:
		{
			if (iAppImageCartView)
			{
				if (iAppImageCartView->GetItemsCount() == 0)
				{
					aMenuPane->SetItemDimmed(EUploadImageCartUpload, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartSelect, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartDeselect, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartPreview, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartRemoveSelected, ETrue);
					aMenuPane->SetItemDimmed(EUploadImageCartRemoveAll, ETrue);
				}
				else
				{
					CSupUploadServiceProvider* uploadProvider = CSupUploadServiceProvider::InstanceL();

					// If any of images is selected,
					// display 'Upload' menu item
					if (uploadProvider->GetSelectionArray().Count() > 0)
						aMenuPane->SetItemDimmed(EUploadImageCartUpload, EFalse);
					// otherwise
					else
						aMenuPane->SetItemDimmed(EUploadImageCartUpload, ETrue);

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
	default:
		break;
	}

	__LOGSTR_TOFILE("CSupAppUi::DynInitMenuPaneL() ends");
}

// -----------------------------------------------------------------------------
// CSupAppUi::HandleCommandL()
// Takes care of command handling.
// -----------------------------------------------------------------------------
//
void CSupAppUi::HandleCommandL( TInt aCommand )
{
	__LOGSTR_TOFILE1("CSupAppUi::HandleCommandL() begins with aCommand == %d", aCommand);

	switch( aCommand )
	{
	case EEikCmdExit:
	case EAknSoftkeyExit:
		{
			// Serialize image cart data
			CSupUploadServiceProvider* provider = CSupUploadServiceProvider::InstanceL();
			provider->SerializeDataL();
			
			// Exit the application
			Exit();
		}
		break;
		// Go to main view
	case ESupSoftkeyContinue:
		{
			// add later necessary code
			if (iAppLoginView)
				iAppLoginView->DoLogin();
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
	case ECredentialsView:
		{
			ActivateViewL(TVwsViewId(KUidViewSupApp, KLoginViewId));
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
	case EUploadImageCartUpload:
		{
			iAppImageCartView->DoUpload();
		}
		break;
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
			__LOGSTR_TOFILE("CSupAppUi::HandleCommandL() begins with command id == ESetIAP");

			CSupLoginServiceProvider* loginProvider = CSupLoginServiceProvider::InstanceL();
			CSupConnectionManager* connMng = CSupConnectionManager::GetInstanceL();

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
					CSupUploadServiceProvider* uploadProvider = CSupUploadServiceProvider::InstanceL();

					uploadProvider->SetIAPL(connMng->GetIAPId());
				}
			}

			__LOGSTR_TOFILE("CSupAppUi::HandleCommandL() ends with command id == ESetIAP");
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
			__LOGSTR_TOFILE("CSupAppUi::HandleCommandL() panic == ESupUi");

			Panic( ESupUi );
		}
		break;
	}

	__LOGSTR_TOFILE("CSupAppUi::HandleCommandL() ends");
}
// -----------------------------------------------------------------------------
//  Called by the framework when the application status pane
//  size is changed.  Passes the new client rectangle to the
//  AppView
// -----------------------------------------------------------------------------
//
void CSupAppUi::HandleStatusPaneSizeChange()
{
	iAppLoginView->SetRect( ClientRect() );

}

CArrayFix<TCoeHelpContext>* CSupAppUi::HelpContextL() const
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
	ctxs->AppendL(TCoeHelpContext(KUidSupApp, KGeneral_Information));
	CleanupStack::Pop(ctxs);
	return ctxs;
}

void CSupAppUi::PrivacyStatementL() 
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
		RBuf text(CSupAppUi::ReadUnicodeFileL(fs, fullFileName));
		CleanupClosePushL(text);
		CAknMessageQueryDialog *dialog = CAknMessageQueryDialog::NewL(text);
		if(dialog->ExecuteLD(R_PRIVSTMT_DIALOG) == EAknSoftkeyYes) 
		{
			BaflUtils::DeleteFile(fs, fullFileName);
		}
		CleanupStack::PopAndDestroy(&text);
	}
}

HBufC * CSupAppUi::ReadUnicodeFileL(RFs& aFs, const TDesC& aFileName)
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

// End of File
