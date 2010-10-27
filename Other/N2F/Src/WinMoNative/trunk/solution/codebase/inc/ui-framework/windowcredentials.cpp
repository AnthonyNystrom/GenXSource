#include "stdafx.h"

#include <windowcredentials.h>


CWindowCredentials::CWindowCredentials()
{
	this->SetWindowID( EWndCredentials );

	iFocusedBack = ControllersHost::GetInstance()->ConfigController()->ImageKey(EGTextBoxFocusedBg);
	iUnfocusedBack = ControllersHost::GetInstance()->ConfigController()->ImageKey(EGTextBoxUnfocusedBg);

	iFocusedBrush.CreateSolidBrush( ControllersHost::GetInstance()->ConfigController()->Color(ECCredentialsFocusedBack) );
	iUnfocusedBrush.CreateSolidBrush( ControllersHost::GetInstance()->ConfigController()->Color(ECCredentialsUnfocusedBack) );

	iLabelTextColor = ControllersHost::GetInstance()->ConfigController()->Color(ECCredentialsLabel);
	iLabelColoredPen.CreatePen(PS_SOLID, 1, iLabelTextColor);
}

CWindowCredentials::~CWindowCredentials()
{
	DeleteObject(iFontTextboxes);
}


BOOL CWindowCredentials::PreTranslateMessage( MSG *pMsg )
{
	return FALSE;
}

LRESULT CWindowCredentials::OnCreate( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	

	SetWindowText( _T("Your credentials...") );

	iLabelName.Create( m_hWnd, NULL, NULL, WS_CHILD|WS_VISIBLE, 0, ID_LABEL_CREDENTIALS_NAME );
	iLabelName.SetWindowText( ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsNameCaption ) );

	iLabelPass.Create( m_hWnd, NULL, NULL, WS_CHILD|WS_VISIBLE, 0, ID_LABEL_CREDENTIALS_PASSWORD );
	iLabelPass.SetWindowText( ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsPassCaption ) );

	iTextboxNameFrame.Create( m_hWnd, NULL, NULL, WS_CHILD|WS_VISIBLE, 0,  ID_IMAGEHOLDER_TEXTBOX_CREDENTIALS_NAME_FRAME );
	iTextboxNameFrame.SetImageHolderStyle( IHS_FIT_HOLDER_FOR_IMAGE );

	iTextboxName.Create( m_hWnd, NULL, NULL, WS_CHILD|WS_VISIBLE|ES_AUTOHSCROLL, 0, ID_TEXTBOX_CREDENTIALS_NAME );
	iTextboxNameFrame.SetImageKey( this->GetImageKeyForBack( &iTextboxName ) );

	iTextboxPassFrame.Create( m_hWnd, NULL, NULL, WS_CHILD|WS_VISIBLE, 0, ID_IMAGEHOLDER_TEXTBOX_CREDENTIALS_PASS_FRAME );
	iTextboxPassFrame.SetImageHolderStyle( IHS_FIT_HOLDER_FOR_IMAGE );

	iTextboxPass.Create( m_hWnd, NULL, NULL, WS_CHILD|WS_VISIBLE|ES_AUTOHSCROLL, 0, ID_TEXTBOX_CREDENTIALS_PASSWORD );
	iTextboxPass.SetPasswordChar( _T('*') );
	iTextboxPassFrame.SetImageKey( this->GetImageKeyForBack( &iTextboxPass ) );

	HFONT hfOld = iTextboxName.GetFont();
	LOGFONT lf ={0};
	::GetObject(hfOld, sizeof(lf), &lf);
	lf.lfHeight = 14;
	lf.lfWeight = FW_BOLD;
	lf.lfStrikeOut = FALSE;
	lf.lfItalic = FALSE;
	lf.lfUnderline = FALSE;
	_tcsncpy( lf.lfFaceName, _T("Tahoma"), LF_FACESIZE );

	iFontTextboxes = ::CreateFontIndirect( &lf );

	iTextboxName.SetFont(iFontTextboxes);
	iTextboxPass.SetFont(iFontTextboxes);

	iLabelName.SetFont( iFontTextboxes );
	iLabelPass.SetFont( iFontTextboxes );

	//iWndSplash.Create( m_hWnd, rcDefault, _T("N2F_WindowSplash"), WS_VISIBLE, 0 );

	iSplashLaunched = false;

	CString username, password;
	ControllersHost::GetInstance()->SettingsController()->GetCurrentUserCredentials( username, password );

	iTextboxName.SetWindowText( username );
	iTextboxPass.SetWindowText( password );

	return TRUE;
}

LRESULT CWindowCredentials::OnSize( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	RECT rcClient = {0};
	GetClientRect( &rcClient );

	if ( rcClient.left == rcClient.right || rcClient.bottom == rcClient.top )
	{
		bHandled = TRUE;
		return 0;
	}

	//iWndSplash.MoveWindow( &rcClient );

	RECT rcNameFrame = {0};
	iTextboxNameFrame.GetWindowRect( &rcNameFrame );
	int frameWidth = rcNameFrame.right - rcNameFrame.left;
	int frameHeight = rcNameFrame.bottom - rcNameFrame.top;
	int leftPos = rcClient.left + (rcClient.right - rcClient.left)/2 - frameWidth/2;
	int verticalOffsetAfterLabel = 20;
	int verticalOffsetAfterEdit = 30;

	int verticalPos = rcClient.top + ( rcClient.bottom - rcClient.top )/4;

	rcNameFrame.left = leftPos;
	rcNameFrame.right = rcNameFrame.left + frameWidth;
	rcNameFrame.top = verticalPos;
	rcNameFrame.bottom = rcNameFrame.top + frameHeight;

	RECT rcLabelName = rcNameFrame;
	::InflateRect( &rcLabelName, 0, -5 );
	iLabelName.MoveWindow( &rcLabelName );

	::OffsetRect( &rcNameFrame, 0, verticalOffsetAfterLabel );
	iTextboxNameFrame.MoveWindow( &rcNameFrame );

	RECT rcNameBox = rcNameFrame;
	::InflateRect( &rcNameBox, -5, -5 );

	iTextboxName.MoveWindow( &rcNameBox );
	iTextboxNameFrame.SetWindowPos( iTextboxName.m_hWnd, 0, 0, 0, 0, SWP_NOACTIVATE|SWP_NOMOVE|SWP_NOSIZE );

	RECT rcLabelPass = rcNameFrame;
	::InflateRect( &rcLabelPass, 0, -5 );
	::OffsetRect( &rcLabelPass, 0, verticalOffsetAfterEdit );
	iLabelPass.MoveWindow( &rcLabelPass );

	RECT rcPassFrame = rcNameFrame;
	::OffsetRect( &rcPassFrame, 0, verticalOffsetAfterEdit+verticalOffsetAfterLabel );
	iTextboxPassFrame.MoveWindow( &rcPassFrame );

	RECT rcPassBox = rcPassFrame;
	::InflateRect(&rcPassBox, -5, -5);
	iTextboxPass.MoveWindow( &rcPassBox );
	iTextboxPassFrame.SetWindowPos( iTextboxPass.m_hWnd, 0, 0, 0, 0, SWP_NOACTIVATE|SWP_NOMOVE|SWP_NOSIZE );


	return 0;
}

LRESULT CWindowCredentials::OnPaint( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	CPaintDC dc( m_hWnd );
	

	return 0;
}

IMAGE_KEY_TYPE CWindowCredentials::GetImageKeyForBack( CEdit *pEditControl )
{
	ATLASSERT( this->IsWindow() );

	if ( NULL == pEditControl )
		return INVALID_IMAGE_KEY_VALUE;

	if ( ::GetFocus() == pEditControl->m_hWnd )
		return iFocusedBack;

	return iUnfocusedBack;
}

LRESULT CWindowCredentials::OnCtlColorEdit( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	HWND ctrlHwnd = (HWND)lParam;
	HBRUSH	hbrResult = (HBRUSH)DefWindowProc();

	int ctrlID = ::GetDlgCtrlID(ctrlHwnd);

	if ( ctrlID == ID_TEXTBOX_CREDENTIALS_NAME ||
		ctrlID == ID_TEXTBOX_CREDENTIALS_PASSWORD )
	{

		HDC hdc = (HDC)wParam;
		::SetBkMode(hdc, TRANSPARENT);
		int focusedWindowId = ::GetDlgCtrlID(::GetFocus());

		if ( ctrlID == focusedWindowId )
		{
			hbrResult = iFocusedBrush;
		}
		else
		{
			hbrResult = iUnfocusedBrush;
		}
	}

	return (LRESULT)hbrResult;
}

LRESULT CWindowCredentials::OnCtlColorStatic( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	HWND ctrlHwnd = (HWND)lParam;
	HBRUSH	hbrResult = (HBRUSH)DefWindowProc();

	int ctrlID = ::GetDlgCtrlID(ctrlHwnd);

	//if ( ID_TEXTBOX_CREDENTIALS_NAME == ctrlID || 
	//		ID_TEXTBOX_CREDENTIALS_PASSWORD == ctrlID )
	//	return (LRESULT)hbrResult;

	HDC hdc = (HDC)wParam;
	if ( ID_LABEL_CREDENTIALS_NAME == ctrlID ||
			ID_LABEL_CREDENTIALS_PASSWORD == ctrlID )
	{
		::SetBkMode( hdc, TRANSPARENT );
		::SetTextColor( hdc, iLabelTextColor );

		//::SelectObject( hdc, iLabelColoredPen );
	}

	return (LRESULT)hbrResult;

}

LRESULT CWindowCredentials::OnEditFocusStateChanged( WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled )
{
	LRESULT lRes = DefWindowProc();

	int ctrlID = ::GetDlgCtrlID(hWndCtl);

	if ( ID_TEXTBOX_CREDENTIALS_NAME == ctrlID )
	{
		iTextboxNameFrame.SetImageKey( GetImageKeyForBack(&iTextboxName) );
		iTextboxName.InvalidateRect(NULL, TRUE);
	}
	else if ( ID_TEXTBOX_CREDENTIALS_PASSWORD == ctrlID  )
	{
		iTextboxPassFrame.SetImageKey( GetImageKeyForBack(&iTextboxPass) );
		iTextboxPass.InvalidateRect(NULL, TRUE);
	}

	return lRes;
}

LRESULT CWindowCredentials::OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	if ( (false == iSplashLaunched) && (::GetFocus() == m_hWnd) )
	{
		LOGMSG("launching splash!!!");
		WindowsManager::GetInstnace()->ShowWindow( EWndSplash );
		::SetFocus( WindowsManager::GetInstnace()->GetWindow( EWndSplash )->m_hWnd );
		
		LOGMSG("Launched...");
		iSplashLaunched = true;
	}

	if ( TRUE == this->IsWindowVisible() )
	{
		bHandled = FALSE;
	}



	//SHMENUBARINFO mbi;
	//ZeroMemory(&mbi, sizeof(SHMENUBARINFO));
	//mbi.cbSize = sizeof(SHMENUBARINFO);
	//mbi.hwndParent = m_hWnd;
	//mbi.nToolBarId = 201;
	//mbi.hInstRes = ModuleHelper::GetResourceInstance();
	//mbi.dwFlags = SHCMBF_HMENU;

	//SHCreateMenuBar( &mbi );

	//bHandled = TRUE;

	

	return 0;
}

LRESULT CWindowCredentials::OnCommand( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	UINT commandID = LOWORD(wParam);

	if ( this->IsLeftButton(commandID) )
	{
		this->DoLogin();
	}
	else if ( EMICredentialsHelp == commandID )
	{
		this->DoHelp();
	}
	else if ( EMICredentialsExitApplication == commandID )
	{
		this->DoExitApplication();
	}

	return 0;
}

void CWindowCredentials::UpdateMenuBar()
{
	CWindowBase::UpdateMenuBar();
}

void CWindowCredentials::GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info )
{
	TMBBInfoStruct btnInfo;

	if ( EBITLeftButton == requestType )
	{
		btnInfo.biButtonID = EMBBCredentialsLogin;
		btnInfo.biButtonName = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsLoginCaption );
	}
	else if ( EBITRightButton == requestType )
	{
		btnInfo.biButtonID = EMBBCredentialsMenu;
		btnInfo.biButtonName = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsMenuCaption );
	}

	info = btnInfo;
}

void CWindowCredentials::GetPopupMenuItems( TPopupMenuItemsCollection& items )
{
	items.RemoveAll();

	TPopupMenuItem tempItem;

	tempItem.caption = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsHelpItem );
	tempItem.isSeparator = false;
	tempItem.id = EMICredentialsHelp;
	items.Add( tempItem );

	tempItem.isSeparator = true;
	tempItem.id = EMICredenitalsSeparator1;
	items.Add( tempItem );

	tempItem.id = EMICredentialsExitApplication;
	tempItem.isSeparator = false;
	tempItem.caption = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsExitItem );
	items.Add( tempItem );
}

bool CWindowCredentials::DoLogin()
{
	LOGMSG("this is me");

	CString mbText = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsLoginFailedMsg );
	CString mbCaption = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsLoginFailedCap );

	ControllerWebServices *ws = ControllersHost::GetInstance()->WebServicesController();
	ASSERT( ws != NULL );

	if ( NULL == ws )
	{
		ShellHelper::ShowErrorMessage( mbCaption, mbText );
		return false;
	}

	CString nick;
	CString password;
	bool doesUserExists = false;

	int maxStringLength = max( iTextboxName.GetWindowTextLength(), iTextboxPass.GetWindowTextLength() );
	TCHAR *textBuffer = NULL;

	this->iTextboxName.GetWindowText( textBuffer );
	nick = textBuffer;
	SysFreeString( textBuffer );
	textBuffer = NULL;

	this->iTextboxPass.GetWindowText( textBuffer );
	password = textBuffer;
	SysFreeString( textBuffer );

	HCURSOR cursorPrev = ::SetCursor( ::LoadCursor(NULL, IDC_WAIT) );
	bool executionResult = false;

#ifdef DEVELOPMENT_BUILD_WS
	//WebServiceN2FMemberService_v3 *wsMS = (WebServiceN2FMemberService_v3 *)ws->GetWebService( EWS_N2F_MemberService_v3 );
	//if ( NULL != wsMS )
	//	executionResult = wsMS->CheckUserExists( nick, password, doesUserExists );
	//else
	//	ASSERT( FALSE );
#endif	//#ifdef DEVELOPMENT_BUILD

	iUsernameToPass = nick;
	iPasswordToPass = password;

	ws->Execute_N2F_MemberService_CheckUserExists( this, doesUserExists );

	::SetCursor( cursorPrev );

	// TODO: check disabled for development proposes, while internet connetion is down
	if ( false && (false == executionResult || false == doesUserExists) )
	{
		LOGMSG("login failed");
		
		ShellHelper::ShowErrorMessage(mbCaption, mbText, m_hWnd);

		return doesUserExists;
	}

	ControllersHost::GetInstance()->SettingsController()->SetCurrentUserCredentials( nick, password );
	ControllersHost::GetInstance()->SettingsController()->SaveSettings();

	LOGMSG("launching operations window");

	this->ShowWindow( SW_HIDE );
	WindowsManager::GetInstnace()->ShowWindow( EWndOperations );
	WindowsManager::GetInstnace()->BringWindowToTop( EWndOperations );
	::SetFocus( WindowsManager::GetInstnace()->GetWindow( EWndOperations )->m_hWnd );

	return doesUserExists;
}

void CWindowCredentials::GetHelpCaption( CString& caption )
{
	caption = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsHelpCaption );
}

void CWindowCredentials::GetHelpText( CString& text )
{
	text = ControllersHost::GetInstance()->ConfigController()->String( ESCredentialsHelpText );
}

void CWindowCredentials::GetUsername( CString& username )
{
	username = iUsernameToPass;
}

void CWindowCredentials::GetPassword( CString& password )
{
	password = iPasswordToPass;
}
