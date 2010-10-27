#include "stdafx.h"
#include <windowoperations.h>

#include <imageholder.h>
#include <imagebutton.h>

#include <windowcredentials.h>

CWindowOperations::CWindowOperations()
{
	this->SetWindowID( EWndOperations );
}

CWindowOperations::~CWindowOperations()
{
	DeleteOperationButtons();
}



LRESULT CWindowOperations::OnCreate( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{

	RECT rcWnd = {0};
	GetWindowRect( &rcWnd );

	LOGMSG("Window rect: %d, %d, %d, %d", rcWnd.left, rcWnd.top, 
		rcWnd.right, rcWnd.bottom );
	
	RECT rcClient = {0};
	this->GetClientRect( &rcClient );


	CreateOperationButtons();

	bHandled = TRUE;
	return TRUE;
}

LRESULT CWindowOperations::OnSize( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	RECT rcWnd = {0};
	GetWindowRect( &rcWnd );

	LOGMSG("Window rect: %d, %d, %d, %d", rcWnd.left, rcWnd.top, 
		rcWnd.right, rcWnd.bottom );

	RECT rc = {0};
	GetClientRect( &rc );

	int clientWidth = rc.right - rc.left;
	int clientHeight = rc.bottom - rc.top;

	//int diffY = 10;
	int diffY = -1;
	//int btnWidth = (clientWidth*4)/5;
	int btnWidth = clientWidth;
	

	if ( clientWidth != 0 )
	{
		int currY = rc.top;

		for ( int i = 0; i < iOperationsButtons.GetSize(); ++i )
		{
			int btnHeight = iOperationsButtons[i]->GetBackImageHeight();

			RECT rcButton = {0};
			rcButton.top = currY;
			rcButton.bottom = rcButton.top + btnHeight;
			rcButton.left = rc.left + clientWidth/2 - btnWidth/2;
			rcButton.right = rcButton.left + btnWidth;

			iOperationsButtons[i]->MoveWindow( &rcButton );

			LOGMSG("Moving button to: %d, %d, %d, %d", rcButton.left, rcButton.top, 
				rcButton.right, rcButton.bottom );

			currY = rcButton.bottom + diffY;
		}
	}

	bHandled = TRUE;
	return 0;
}

LRESULT CWindowOperations::OnPaint( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{	
	CPaintDC dc( m_hWnd );

	RECT rc = {0};
	GetClientRect( &rc );

	TRIVERTEX vert[2];
	vert[0].x = rc.left;
	vert[0].y = rc.top;
	vert[0].Red = vert[0].Green = vert[0].Blue = 0xff00;
	vert[0].Alpha = 0;

	vert[1].x = rc.right;
	vert[1].y = rc.bottom;
	vert[1].Red = vert[1].Green = vert[1].Blue = 0xd600;
	vert[1].Alpha = 0;

	GRADIENT_RECT gRect;
	gRect.UpperLeft = 0;
	gRect.LowerRight = 1;
	
	::GradientFill( dc, vert, 2, &gRect, 1, GRADIENT_FILL_RECT_V);

	return 0;
}

void CWindowOperations::GetSupportedOperationsList( TSupportedOperationsList& list )
{
	list.RemoveAll();

	TSupportedOperation op;
	op.idControl = ID_BUTTON_OPERATION_UPLOAD;
	op.idCaption = ESOperationUpload;
	op.idIcon = EGUploadIcon;

	list.Add( op );

	op.idControl = ID_BUTTON_OPERATION_RECENT_UPLOADS;
	op.idCaption = ESOperationRecentUploads;
	op.idIcon = EGRecentUploadsIcon;

	list.Add( op );

	op.idControl = ID_BUTTON_OPERATION_CREDENTIALS;
	op.idCaption = ESOperationCredentials;
	op.idIcon = EGCredentialsIcon;

	list.Add( op );
}

void CWindowOperations::CreateOperationButtons()
{
	DeleteOperationButtons();

	TSupportedOperationsList list;
	this->GetSupportedOperationsList( list );

	if ( list.GetSize() == 0 )
		return;

	IMAGE_KEY_TYPE keyBtn, keyBtnAlt;
	keyBtn = ControllersHost::GetInstance()->ConfigController()->ImageKey( EGOpBtnBack );
	keyBtnAlt = ControllersHost::GetInstance()->ConfigController()->ImageKey( EGOpBtnBackPressed );

	COLORREF clrBtnUnfocusedText, clrBtnFocusedTextAlt;
	//clrBtnText = RGB(0xff, 0xff, 0xff);
	//clrBtnTextAlt = RGB(0x02, 0x57, 0xae);

	clrBtnUnfocusedText = ControllersHost::GetInstance()->ConfigController()->Color( ECOperationsUnfocusedButtonText );
	clrBtnFocusedTextAlt = ControllersHost::GetInstance()->ConfigController()->Color( ECOperationsFocusedButtonText );

	HFONT hfOld;
	LOGFONT lf;
	HFONT hfNew;

	for ( int i = 0; i < list.GetSize(); ++i )
	{
		CImageButton *btn = new CImageButton;
		if ( NULL == btn )
		{
			ASSERT(FALSE);
			continue;
		}

		btn->Create( m_hWnd, NULL, _T("n2fOperationImageButton"), WS_CHILD|WS_VISIBLE, 0, list[i].idControl);
		CString caption = ControllersHost::GetInstance()->ConfigController()->String( list[i].idCaption );

		if ( i == 0 )
		{
			hfOld = btn->GetFont();
			::GetObject(hfOld, sizeof(lf), &lf);
			lf.lfHeight = 15;
			lf.lfWeight = FW_MEDIUM;
			_tcsncpy( lf.lfFaceName, _T("Tahoma"), LF_FACESIZE );

			hfNew = ::CreateFontIndirect(&lf);
			
		}
		//hfOld = btn->GetFont();
		btn->SetFont(hfNew);

		//::DeleteObject(hfOld);

		btn->SetWindowText( caption );
		btn->SetBorderStyle( IBS_NO_3D );
		btn->SetImageKey( ControllersHost::GetInstance()->ConfigController()->ImageKey( list[i].idIcon ) );
		btn->SetBackImageKey( keyBtn, keyBtnAlt );
		btn->SetButtonTextColor( clrBtnUnfocusedText, clrBtnFocusedTextAlt );
		btn->SetImageOffset( 10 );

		iOperationsButtons.Add( btn );
	}

	LOGMSG("Number of buttons: %d", iOperationsButtons.GetSize());

}

void CWindowOperations::DeleteOperationButtons()
{
	for ( int i = 0; i < iOperationsButtons.GetSize(); ++i )
	{
		delete iOperationsButtons[i];
	}

	iOperationsButtons.RemoveAll();
}

LRESULT CWindowOperations::OnButtonClicked( WORD wNotifyCode, WORD wID, HWND hWnd, BOOL& bHandled )
{
	bHandled = FALSE;

	switch ( wID )
	{
	case ID_BUTTON_OPERATION_UPLOAD:
		OnOperationUpload();
		bHandled = TRUE;
		break;

	case ID_BUTTON_OPERATION_CREDENTIALS:
		OnOperationCredentials();
		bHandled = TRUE;
		break;

	case ID_BUTTON_OPERATION_RECENT_UPLOADS:
		OnOperationRecentUploads();
		bHandled = TRUE;
		break;
	}

	return 0;
}

LRESULT CWindowOperations::OnUploadFile( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	LOGME();
	TRecentUploadItem *item = (TRecentUploadItem*)lParam;

	if ( NULL == item )
	{
		LOGMSG("Error - invalid argument passed");
		ASSERT( false );
		return 0;
	}

	iFilePathToUpload = item->filePath;
	iFileTitleToUpload = item->fileTitle;
	iUploadTime = item->dateTime;

	this->ShowNotification();

	return 0;
}

void CWindowOperations::OnOperationUpload()
{
	LOGME();

	TCHAR bufPath[MAX_PATH] = {0};
	TCHAR bufName[MAX_PATH] = {0};

	OPENFILENAMEEX  ofnex	= {0};

	ofnex.lStructSize		= sizeof(ofnex);
	ofnex.hwndOwner			= m_hWnd;
	ofnex.lpstrFile			= bufPath;
	ofnex.nMaxFile			= sizeof(bufPath)/sizeof(bufPath[0]);
	ofnex.lpstrFileTitle	= bufName;
	ofnex.nMaxFileTitle		= sizeof(bufName)/sizeof(bufName[0]);
	ofnex.lpstrTitle		= TEXT("Select image to upload...");
	ofnex.ExFlags			= OFN_EXFLAG_THUMBNAILVIEW;
	ofnex.lpstrInitialDir	= NULL;

	if ( 0 == ::GetOpenFileNameEx( &ofnex ) )
	{
		DWORD err = ::GetLastError();
		LOGMSG("error: %d", err);

		LOGMSG("No file was selected");
		return;

		//ASSERT( FALSE );
	}

	//this->SetFocus();

	LOGMSG("path: %s\nname: %s", bufPath, bufName);

	iFilePathToUpload = CString(bufPath);
	iFileTitleToUpload = CString(bufName);
	::GetLocalTime( &iUploadTime );

	LOGMSG("remeber file, we're trying to upload");
	ControllersHost::GetInstance()->SettingsController()->RememberUploadItem( iFileTitleToUpload, 
		iFilePathToUpload, iUploadTime );

	WindowsManager::GetInstnace()->GetWindow(EWndRecentUploads)->SendMessage( WM_RECENTUPLOADS_REFRESHLIST );

	this->ShowNotification();

	//this->SetFocus();
}

void CWindowOperations::ShowNotification()
{
	LOGME();

	TCHAR htmlFormat[] = 	_T("<html><body>")
							_T("<div align=\"center\"><p>%s</p></div>")
							_T("<ul>")
							_T("<li>%s</li>")
							_T("</ul>")
							_T("<div align=\"center\">")
							_T("<input type=\"button\" value=\"%s\" name=\"cmd:%d\">")
							_T("<input type=\"button\" value=\"%s\" name=\"cmd:%d\">")
							_T("</div>")
							_T("</body></html>");

	CString notificationQuestion = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsSNBodyText );
	CString btnCaptionYes = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsSNBtnYes );
	CString btnCaptionNo = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsSNBtnNo );
	CString notificationTitle = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsSNTitle );

	CString notificationBodyFormat( htmlFormat );
	CString notificationBodyText;

	notificationBodyText.Format( notificationBodyFormat, notificationQuestion, iFileTitleToUpload, 
		btnCaptionYes, EMBBConfirmUploadYes, btnCaptionNo, EMBBConfirmUploadNo );

	// create notification to confirm upload

	ShellHelper::ShowNotification( ESNConfirmUpload, -1, m_hWnd, notificationBodyText, notificationTitle);

	//SHNOTIFICATIONDATA nd = {0};
	//nd.cbStruct = sizeof(nd);
	//nd.dwID = ESNConfirmUpload;
	//nd.npPriority = SHNP_INFORM;
	//nd.csDuration = -1;
	//nd.grfFlags = SHNF_FORCEMESSAGE|SHNF_DISPLAYON;
	//nd.hwndSink = m_hWnd;
	//nd.pszHTML = notificationBodyText;
	//nd.pszTitle = notificationTitle;

	//if ( ERROR_SUCCESS != ::SHNotificationAdd( &nd ) )
	//{
	//	DWORD err = ::GetLastError();
	//	LOGMSG("error: %d", err);
	//	ASSERT( FALSE );
	//}
}

void CWindowOperations::OnOperationCredentials()
{
	LOGME();

	this->ShowWindow( SW_HIDE );
	WindowsManager::GetInstnace()->ShowWindow( EWndCredentials );
	WindowsManager::GetInstnace()->BringWindowToTop( EWndCredentials );
	WindowsManager::GetInstnace()->GetWindow( EWndCredentials )->SetFocus();

	LOGMSG("done");
}

void CWindowOperations::OnOperationRecentUploads()
{
	LOGME();

	this->ShowWindow( SW_HIDE );
	WindowsManager::GetInstnace()->ShowWindow( EWndRecentUploads );
	WindowsManager::GetInstnace()->BringWindowToTop( EWndRecentUploads );
	WindowsManager::GetInstnace()->GetWindow( EWndRecentUploads )->SetFocus();

	LOGMSG("done");
}

void CWindowOperations::OnOperationExit()
{
	LOGME();
	this->GetParent().PostMessage( WM_CLOSE );
}

void CWindowOperations::UpdateMenuBar()
{
	CWindowBase::UpdateMenuBar();
}


void CWindowOperations::GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info )
{
	TMBBInfoStruct btnInfo;

	if ( EBITLeftButton == requestType )
	{
		btnInfo.biButtonID = EMBBOperationsLeft;
	}
	else if ( EBITRightButton == requestType )
	{
		btnInfo.biButtonID = EMBBOperationsMenu;
		btnInfo.biButtonName = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsMenuCaption );
	}

	info = btnInfo;
}

void CWindowOperations::GetPopupMenuItems( TPopupMenuItemsCollection& items )
{
	items.RemoveAll();

	TPopupMenuItem tempItem;

	tempItem.caption = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsHelpItem );
	tempItem.isSeparator = false;
	tempItem.id = EMIOperationsHelp;
	items.Add( tempItem );

	tempItem.isSeparator = true;
	tempItem.id = EMIOperationsSeparator1;
	items.Add( tempItem );

	tempItem.id = EMIOperationsExitApplication;
	tempItem.isSeparator = false;
	tempItem.caption = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsExitItem );
	items.Add( tempItem );
}

LRESULT CWindowOperations::OnCommand( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	UINT commandID = LOWORD(wParam);

	if ( EMIOperationsHelp == commandID )
	{
		this->DoHelp();
	}
	else if ( EMIOperationsExitApplication == commandID )
	{
		this->DoExitApplication();
	}

	return 0;
}



LRESULT CWindowOperations::OnUploadConfirmResult( WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled )
{
	bHandled = FALSE;
	LOGME();

	if ( EMBBConfirmUploadYes == wID )
	{
		this->DoFileUpload();
		bHandled = TRUE;
	}
	else if ( EMBBConfirmUploadNo == wID )
	{
		bHandled = TRUE;
	}

	//this->SetFocus();

	return 0;
}

bool CWindowOperations::DoFileUpload()
{
	LOGME();

	HCURSOR cursorPrev = ::SetCursor( ::LoadCursor(NULL, IDC_WAIT) );

	CString mbCaption = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsUpFailedMBCapt );
	CString mbText = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsUpFailedMBText );

	CString userName, userPassword, filePath = iFilePathToUpload;

	ControllersHost::GetInstance()->SettingsController()->GetCurrentUserCredentials( userName, userPassword );
	iUsernameToPass = userName;
	iPasswordToPass = userPassword;

	SYSTEMTIME currentTime = iUploadTime;

	ControllerWebServices *ws = ControllersHost::GetInstance()->WebServicesController();
	ASSERT( ws != NULL || (iFilePathToUpload.GetLength() == 0) );
	if ( NULL == ws )
	{
		ShellHelper::ShowErrorMessage(mbCaption, mbText, m_hWnd);
		return false;
	}

	LOGMSG("calling DeviceUploadPhoto with params:\n%s\n%s\n%s\n\n", userName, userPassword, filePath);

	bool executionResult = false;
//#ifdef DEVELOPMENT_BUILD_WS
//	WebServiceN2FSnapUpService *wsSS = (WebServiceN2FSnapUpService *)ws->GetWebService( EWS_N2F_SnapUpService );
//	if ( NULL != wsSS )
//		executionResult = wsSS->DeviceUploadPhoto( userName, userPassword, filePath, currentTime );
//	else
//		ASSERT( FALSE );
//#endif	//#ifdef DEVELOPMENT_BUILD

	executionResult = ws->Execute_N2F_SnapUpService_DeviceUploadPhoto( this );

	if ( false == executionResult )
	{
		ShellHelper::ShowErrorMessage(mbCaption, mbText, m_hWnd);
	}
	
	//{// update upload status
	ControllersHost::GetInstance()->SettingsController()->UpdateUploadStatus( iFilePathToUpload, executionResult );
	WindowsManager::GetInstnace()->GetWindow(EWndRecentUploads)->SendMessage( WM_RECENTUPLOADS_REFRESHLIST );
	//}



	::SetCursor( cursorPrev );

	return false;
}

void CWindowOperations::GetHelpCaption( CString& caption )
{
	caption = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsHelpCaption );
}

void CWindowOperations::GetHelpText( CString& text )
{
	text = ControllersHost::GetInstance()->ConfigController()->String( ESOperationsHelpText );
}

void CWindowOperations::GetUsername( CString& username )
{
	username = iUsernameToPass;
}

void CWindowOperations::GetPassword( CString& password )
{
	password = iPasswordToPass;
}

void CWindowOperations::GetFilePathToUpload( CString& filePath )
{
	filePath = iFilePathToUpload;
}

void CWindowOperations::GetTimeForUpload( SYSTEMTIME& st )
{
	st = iUploadTime;
}

