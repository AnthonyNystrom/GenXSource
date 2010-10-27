/*
============================================================================
Name        : Aafapploginview.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Login view
============================================================================
*/

// INCLUDE FILES
#include <coemain.h>
#include <eikenv.h>
#include <eikmenub.h>
#include <eikspane.h>
#include <barsread.h>
#include <aknutils.h>
#include <txtglobl.h>
#include <aknwaitdialog.h>
#include <senserviceconnection.h>
#include <utf.h>
#include "AafAppLoginView.h"
#include "Aafloginserviceprovider.h"
#include "Aafappui.h"
#include "common.h"

#if USE_SKIN_SUPPORT
#include <AknsDrawUtils.h>
#include <AknsBasicBackgroundControlContext.h>
#endif

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafAppLoginView::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppLoginView* CAafAppLoginView::NewL( const TRect& aRect )
{
	CAafAppLoginView* self = CAafAppLoginView::NewLC( aRect );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppLoginView::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafAppLoginView* CAafAppLoginView::NewLC( const TRect& aRect )
{
	CAafAppLoginView* self = new ( ELeave ) CAafAppLoginView;
	CleanupStack::PushL( self );
	self->ConstructL( aRect );
	return self;
}

// -----------------------------------------------------------------------------
// CAafAppLoginView::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafAppLoginView::ConstructL( const TRect& aRect )
{
	__LOGSTR_TOFILE("CAafAppLoginView::ConstructL() begins");

	// Create a window for this application view
	CreateWindowL();

#if USE_SKIN_SUPPORT
	iBgContext = CAknsBasicBackgroundControlContext::NewL( KAknsIIDQsnBgAreaMain, aRect, ETrue);
#endif

	iFocus = 0; // By default first edwin is focused
	iFocusControls = 2; // Set count of controls which could be focused

	// Instantiate the login label control
	iLoginLabel = new (ELeave) CEikLabel;
	iLoginLabel->SetContainerWindowL( *this );

	// Get CCoeEnv instance
	CEikonEnv* eikonEnv = CEikonEnv::Static();

	HBufC* stringHolder = StringLoader::LoadL(R_LOGIN_LABEL, eikonEnv );

	iLoginLabel->SetTextL( *stringHolder );

	// Free dynamically allocated memory
	delete stringHolder;
	stringHolder = NULL;

	// Instantiate the login editor control
	iLoginEdwin = new (ELeave)CEikEdwin;
	iLoginEdwin->SetContainerWindowL(*this);

	// Set up the resource reader 
	TResourceReader reader; 
	CCoeEnv::Static()->CreateResourceReaderLC(reader, R_LOGIN_BOX); 
	// Call second phase constructor 
	iLoginEdwin->ConstructFromResourceL(reader); 
	CleanupStack::PopAndDestroy();

	// Activate editor control 
	iLoginEdwin->ActivateL();

	// Set login edwin font
	const CFont* editorFont = const_cast<CFont*>(AknLayoutUtils::FontFromId(EAknLogicalFontPrimaryFont));
	TCharFormat charFormat;
	TCharFormatMask charFormatMask;
	// Get the font specification
	charFormat.iFontSpec = editorFont->FontSpecInTwips();
	// Set all attribute flag in character format mask
	charFormatMask.SetAll();

	// Get the text
	CGlobalText* globalText = (CGlobalText*)iLoginEdwin->Text();
	// Apply the character formatting
	globalText->ApplyCharFormatL(charFormat,charFormatMask,0, iLoginEdwin->TextLength());

	// Instantiate the password label control
	// Instantiate the login label control
	iPassLabel = new (ELeave) CEikLabel;
	iPassLabel->SetContainerWindowL( *this );

	stringHolder = StringLoader::LoadL(R_PASSWORD_LABEL, eikonEnv );

	iPassLabel->SetTextL( *stringHolder );

	// Free dynamically allocated memory
	delete stringHolder;
	stringHolder = NULL;

	// Instantiate the password editor control
	iPassEdwin = new (ELeave)CEikSecretEditor;
	iPassEdwin->SetContainerWindowL( *this );

	// Set up recourse reader
	CCoeEnv::Static()->CreateResourceReaderLC(reader, R_PASSWORD_BOX);
	iPassEdwin->ConstructFromResourceL(reader);
	CleanupStack::PopAndDestroy();

	// Activate password control
	iPassEdwin->ActivateL();
	// Set password edwin font
	iPassEdwin->AknSetFont(*editorFont);

	// Set the focus to the appropriate control
	SetControlFocus();

	// Set the windows size
	SetRect( aRect );

	// Activate the window, which makes it ready to be drawn
	ActivateL();

	__LOGSTR_TOFILE("CAafAppLoginView::ConstructL() ends");
}

// -----------------------------------------------------------------------------
// CAafAppLoginView::CAafAppLoginView()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafAppLoginView::CAafAppLoginView()
{
	iFocus = 0;
	iFocusControls = 0;
}


// -----------------------------------------------------------------------------
// CAafAppLoginView::~CAafAppLoginView()
// Destructor.
// -----------------------------------------------------------------------------
//
CAafAppLoginView::~CAafAppLoginView()
{
	// Free dynamically allocated memory
#if USE_SKIN_SUPPORT
	if (iBgContext)
	{
		delete iBgContext;
		iBgContext = NULL;
	}
#endif

	if (iLoginLabel)
	{
		delete iLoginLabel;
		iLoginLabel = NULL;
	}

	if (iLoginEdwin)
	{
		delete iLoginEdwin;
		iLoginEdwin = NULL;
	}

	if (iPassLabel)
	{
		delete iPassLabel;
		iPassLabel = NULL;
	}

	if (iPassEdwin)
	{
		delete iPassEdwin;
		iPassEdwin = NULL;
	}

	if (iWaitDialog)
	{
		delete iWaitDialog;
		iWaitDialog = NULL;
	}
}


// -----------------------------------------------------------------------------
// CAafAppLoginView::Draw()
// Draws the display.
// -----------------------------------------------------------------------------
//
void CAafAppLoginView::Draw( const TRect& aRect ) const
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

	// Login edwin border
	TRect ctrlRect(iLoginEdwin->Rect());
	// Draw login edwin border
	gc.SetPenStyle(CGraphicsContext::ESolidPen);
	gc.SetPenColor(KRgbBlack);
	ctrlRect.Grow(1, 1);
	TGulBorder ctrlBorder(iLoginEdwin->Border());
	ctrlBorder.SetType(TGulBorder::ESingleBlack);
	ctrlBorder.Draw(gc, ctrlRect);

	// Password edwin border
	ctrlRect = iPassEdwin->Rect();
	// Draw pass edwin border
	ctrlRect.Grow(1, 1);
	ctrlBorder = iPassEdwin->Border();
	ctrlBorder.SetType(TGulBorder::ESingleBlack);
	ctrlBorder.Draw(gc, ctrlRect);
}

// -----------------------------------------------------------------------------
// CAafAppLoginView::SizeChanged()
// Called by framework when the view size is changed.
// -----------------------------------------------------------------------------
//
void CAafAppLoginView::SizeChanged()
{  

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

	CAafAppUi* appUi = (CAafAppUi*)iCoeEnv->AppUi();
	TRect clientRect(appUi->ClientRect());

	// iPassEdwin would have max width value
	TInt maxWidth = iPassEdwin->MinimumSize().iWidth;

	// Restrict max width to 3/4 of the total client rect width
	if (maxWidth >= clientRect.Width())
	{
		maxWidth = clientRect.Width()*3/4;
	}

	// edwin control size
	TSize edwinSize = TSize(maxWidth, iPassEdwin->MinimumSize().iHeight);

	// Control position (cX dimension)
	TInt cxPos = (clientRect.Width() - maxWidth)/2;

	// Control position (cX and cY dimensions)
	// As there're 4 controls, choose vertical shift value equils 1/5 of total client rect height
	TPoint controlPosition = TPoint(cxPos, clientRect.Height()/5);

	// Login label
	iLoginLabel->SetExtent(controlPosition, TSize(iLoginLabel->MinimumSize().iWidth, iLoginLabel->MinimumSize().iHeight*3/2));

	// Login edwin
	controlPosition = TPoint(cxPos, clientRect.Height()*2/5);	
	iLoginEdwin->SetExtent(controlPosition, edwinSize);

	// Password label
	controlPosition = TPoint(cxPos, clientRect.Height()*3/5);
	iPassLabel->SetExtent(controlPosition, TSize(iPassLabel->MinimumSize().iWidth, iPassLabel->MinimumSize().iHeight*3/2));

	// Password edwin
	controlPosition = TPoint(cxPos, clientRect.Height()*4/5);
	iPassEdwin->SetExtent(controlPosition, edwinSize);
}

TInt CAafAppLoginView::CountComponentControls() const
{ 
	return 4; // returns number of controls inside the container
}

CCoeControl* CAafAppLoginView::ComponentControl(TInt aIndex)  const 
{ 
	switch ( aIndex ) 
	{ 
	case 0:
		return iLoginLabel; // returns a pointer to the
		// login label
	case 1: 
		return iLoginEdwin; // returns a pointer to the  
		// login field
	case 2:
		return iPassLabel;	// returns a pointer to the
		// password label
	case 3:
		return iPassEdwin; // returns a pointer to the
		// password field
	default: 
		return NULL; 
	} 
} 

TKeyResponse CAafAppLoginView::OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType)
{
	if(aType == EEventKey)
	{
		switch(aKeyEvent.iScanCode) 
		{
		case EStdKeyUpArrow:
			{				
				if (iFocus)
					--iFocus;
				else
					iFocus = 0;

				SetControlFocus();

				DrawNow();

				return EKeyWasConsumed;
			}				
		case EStdKeyDownArrow:
			{
				++iFocus;
				if (iFocus == iFocusControls)
					iFocus = 1;

				SetControlFocus();

				DrawNow();

				return EKeyWasConsumed;
			}

			// If not arrow up or arrow down key has been pressed
		default:
			{
				if (iFocus == 0)
				{
					return iLoginEdwin->OfferKeyEventL(aKeyEvent, aType);
				}
				else if (iFocus == 1)
				{
					return iPassEdwin->OfferKeyEventL(aKeyEvent, aType);
				}
			}
			break;
		}
	}

	return EKeyWasNotConsumed;
}

#if USE_SKIN_SUPPORT
TTypeUid::Ptr CAafAppLoginView::MopSupplyObject(TTypeUid aId)
{
	if (iBgContext )
	{
		return MAknsControlContext::SupplyMopObject( aId, iBgContext );
	}

	return CCoeControl::MopSupplyObject(aId);
}
#endif

void CAafAppLoginView::HandleResourceChange(TInt aType)
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
TVwsViewId CAafAppLoginView::ViewId() const
{
	return TVwsViewId(KUidViewSupApp, KLoginViewId);
}

void CAafAppLoginView::DoLogin()
{
	// If web service connection is ready
	if (CAafLoginServiceProvider::GetInstanceL()->GetStatus() == KSenConnectionStatusReady)
	{
		// Get login and password editor field values
		TBuf<20> loginValue;
		TBuf<20> passValue;

		iLoginEdwin->GetText(loginValue);
		iPassEdwin->GetText(passValue);

		// If login and password fields are not empty
		if (loginValue.Length() && passValue.Length())
		{
			if (CAafLoginServiceProvider::GetInstanceL())
			{
				// Convert to 8-bits strings
				TBuf8<20> loginValue8;
				loginValue8.Copy(loginValue);

				TBuf8<20> passValue8;
				passValue8.Copy(passValue);

				// If new entered credentials differ from current ones,
				// initiate login process
				if (CAafLoginServiceProvider::GetInstanceL()->GetMemberID() == NULL || CAafLoginServiceProvider::GetInstanceL()->GetUsername() == NULL || CAafLoginServiceProvider::GetInstanceL()->GetPassword() == NULL)
				{
					StartWaitDialog();

					// Start login process using appropriate service provider
					CAafLoginServiceProvider::GetInstanceL()->StartLoginL(this, loginValue8, passValue8);
				}
				else if (CAafLoginServiceProvider::GetInstanceL()->GetUsername()->Des() != loginValue8 || CAafLoginServiceProvider::GetInstanceL()->GetPassword()->Des() != passValue8)
				{
					StartWaitDialog();

					// Start login process using appropriate service provider
					CAafLoginServiceProvider::GetInstanceL()->StartLoginL(this, loginValue8, passValue8);
				}
				else
				{
					// Just go back to main view
					CAafAppUi* appUi = (CAafAppUi*)iCoeEnv->AppUi();

					appUi->HandleCommandL(EAknSoftkeyCancel);
				}
			}
		}
		// Else show warning notification
		else
		{
			CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

			// Get CCoeEnv instance
			CEikonEnv* eikonEnv = CEikonEnv::Static();

			HBufC* stringHolder = StringLoader::LoadL(R_LOGIN_EMPTY_FIELDS_NOTE, eikonEnv );

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

// Activates this view, called by framework
void CAafAppLoginView::ViewActivatedL(const TVwsViewId &/*aPrevViewId*/, TUid /*aCustomMessageId*/, const TDesC8 &/*aCustomMessage*/)
{
	__LOGSTR_TOFILE("CAafAppLoginView::ViewActivatedL() ends");

	RDebug::Print(_L("CAafAppLoginView::ViewActivatedL()"));

	// Set menubar and CBA
	CEikonEnv* eikonEnv = CEikonEnv::Static();
	MEikAppUiFactory* appUiFactory = eikonEnv->AppUiFactory();

	CEikMenuBar* menuBar = appUiFactory->MenuBar();
	// Set control panel visible
	CEikButtonGroupContainer* cba = appUiFactory->Cba();
	cba->MakeVisible(ETrue);

	// If any menubar is displayed - stop displaying
	if (menuBar)
		menuBar->StopDisplayingMenuBar();

	// Get current log-in status
	if (CAafLoginServiceProvider::GetInstanceL()->IsLoggedIn())
	{
		// Username value
		HBufC8* username = CAafLoginServiceProvider::GetInstanceL()->GetUsername();

		HBufC* usernameBuf = HBufC::NewLC(username->Length());
		usernameBuf->Des().Copy(*username);

		iLoginEdwin->SetTextL(usernameBuf);

		CleanupStack::PopAndDestroy(); // usernameBuf

		// Password value
		HBufC8* password = CAafLoginServiceProvider::GetInstanceL()->GetPassword();

		HBufC* passBuf = HBufC::NewLC( password->Length());
		passBuf->Des().Copy(*password);

		iPassEdwin->SetText(*passBuf);

		CleanupStack::PopAndDestroy(); // passBuf

		// Set appropriate command buttons
		cba->SetCommandSetL(R_AAF_CBA_CREDENTIALS);
	}
	else
	{
		__LOGSTR_TOFILE("CAafAppLoginView::ViewActivatedL() is not logged in yet");

		menuBar->SetMenuTitleResourceId(R_AAF_LOGINVIEW_MENUBAR);

		cba->SetCommandSetL(R_AAF_CBA_START);
	}

	cba->DrawDeferred();

	// Bring this view to the top of windows stack
	Window().SetOrdinalPosition(0);

	MakeVisible(ETrue);
}

// Deactivates this view, called by framework
void CAafAppLoginView::ViewDeactivated()
{
	MakeVisible(EFalse);
}

void CAafAppLoginView::DialogDismissedL(TInt /*aButtonId*/)
{
	// Cancel login process
	CAafLoginServiceProvider::GetInstanceL()->CancelLogin();

	StopWaitDialog();
}

void CAafAppLoginView::StartWaitDialog()
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

	HBufC* stringHolder = StringLoader::LoadL(R_LOGIN_WAITNOTE, eikonEnv );

	iWaitDialog->SetTextL(*stringHolder);

	iWaitDialog->ExecuteLD(R_WAITNOTE);

	delete stringHolder;
	stringHolder = NULL;
}

void CAafAppLoginView::StopWaitDialog()
{
	// For wait dialog
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}	
}

void CAafAppLoginView::HandleRequestCompletedL(const TInt& aError)
{
	RDebug::Print(_L("CAafAppLoginView::HandleRequestCompletedL() %d"), aError);

	// Stop waitDialog
	StopWaitDialog();

	// If no error - open main view
	if (aError == KErrNone)
	{
		CAafAppUi* appUi = (CAafAppUi*)iCoeEnv->AppUi();

		// Open main view
		appUi->ActivateViewL(TVwsViewId(KUidViewSupApp, KMainViewId));
	}
	// In case of error, show notification about this
	else
	{
		CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

		// Get CCoeEnv instance
		CEikonEnv* eikonEnv = CEikonEnv::Static();

		HBufC* stringHolder = StringLoader::LoadL(R_LOGIN_FAILURE_NOTE, eikonEnv );

		dialog->SetTextL(*stringHolder);

		if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
		{

		}

		delete stringHolder;
		stringHolder = NULL;
	}	
}

void CAafAppLoginView::SetControlFocus()
{
	switch (iFocus)
	{
	case 0:
		{
			iLoginEdwin->SetFocus(ETrue);
			iPassEdwin->SetFocus(EFalse);
		}
		break;
	case 1:
		{
			iLoginEdwin->SetFocus(EFalse);
			iPassEdwin->SetFocus(ETrue, EDrawNow);
		}
		break;
	default:
		{
			iLoginEdwin->SetFocus(EFalse);
			iPassEdwin->SetFocus(EFalse);
		}
	}

}

// End of File