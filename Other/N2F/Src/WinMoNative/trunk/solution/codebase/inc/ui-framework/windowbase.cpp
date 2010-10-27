#include "stdafx.h"

#include <windowbase.h>

CWindowBase::CWindowBase()
{
	iWindowID = EWndUnknown;

	iLeftButtonID = ID_ACTION;
	iRightButtonID = ID_MENU;
}

CWindowBase::~CWindowBase()
{
	LOGMSG(" window ID: %d", iWindowID);
}

BOOL CWindowBase::PreTranslateMessage( MSG *pMsg )
{
	return FALSE;
}

void CWindowBase::SetWindowID( TWindowID id )
{
	iWindowID = id;
}

TWindowID CWindowBase::GetWindowID()
{
	return iWindowID;
}

LRESULT CWindowBase::OnCreate( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{

	return TRUE;
}

LRESULT CWindowBase::OnPaint( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	return 0;
}

LRESULT CWindowBase::OnSetFocus( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	if ( m_hWnd == ::GetFocus() /*&& this->IsWindowVisible()*/ )
	{
		this->UpdateMenuBar();
	}

	return 0;
}

void CWindowBase::UpdateMenuBar()
{
	//AtlCreateEmptyMenuBar( m_hWnd );
	CreateEmptyMenuBar();
	
	return;
}

void CWindowBase::CreateEmptyMenuBar()
{
	HWND hParent = this->GetParent();
	if ( FALSE == ::IsWindow(hParent) )
	{
		ASSERT(FALSE);
		return;
	}

	TCHAR parentText[100] = {0};
	::GetWindowText( hParent, parentText, 99 );

	HWND hMenuBar = ::SHFindMenuBar( hParent );
	if ( FALSE == ::IsWindow( hMenuBar ) )
	{
		ASSERT(FALSE);
		return;
	}

	TMBBInfoStruct btnLeft, btnRight;
	this->GetMenuBarButtonsInfo( EBITLeftButton, btnLeft );
	this->GetMenuBarButtonsInfo( EBITRightButton, btnRight );

	this->UpdateMenuBarButton( hMenuBar, iLeftButtonID, btnLeft );
	this->UpdateMenuBarButton( hMenuBar, iRightButtonID, btnRight );
	
	::DrawMenuBar( hParent );
}

void CWindowBase::GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info )
{
	TMBBInfoStruct buttonInfo;

	info = buttonInfo;
}

void CWindowBase::UpdateMenuBarButton( HWND hMenuBar, int id, TMBBInfoStruct& btnInfo )
{
	TBBUTTONINFO tbbi;
	tbbi.cbSize = sizeof(TBBUTTONINFO);
	tbbi.dwMask = TBIF_TEXT;
	tbbi.pszText = btnInfo.biButtonName.GetBuffer(btnInfo.biButtonName.GetLength()+10);

	::SendMessage( hMenuBar, TB_SETBUTTONINFO, id, (LPARAM)&tbbi );

	TBBUTTONINFO tbbi2;
	tbbi2.cbSize = sizeof(TBBUTTONINFO);
	tbbi2.dwMask = TBIF_LPARAM;

	if ( id == iLeftButtonID )
		return;

	::SendMessage( hMenuBar, TB_GETBUTTONINFO, id, (LPARAM)&tbbi2 );

	HMENU hPopupMenu = (HMENU)(tbbi2.lParam);
	CMenu menuPopup;
	menuPopup.Attach( hPopupMenu );

	//UINT cntMenuItems = menuPopup.GetMenuItemCount();
	//for ( UINT idx = 0; idx < cntMenuItems; ++idx )
	//{
	//	menuPopup.DeleteMenu( idx, MF_BYPOSITION );
	//}

	UINT idx = 0, cnt = 0;
	BOOL result = TRUE;
	do 
	{
		result = menuPopup.DeleteMenu( idx, MF_BYPOSITION );
		if ( cnt++ > 20 )
		{
			ASSERT(FALSE);
		}

	} while ( result == TRUE );

	//result = menuPopup.DeleteMenu( 0, MF_BYPOSITION );

	TPopupMenuItemsCollection items;
	this->GetPopupMenuItems( items );

	for ( int i = 0; i < items.GetSize(); ++i )
	{
		UINT menuItemFlags = MF_STRING;
		if ( items[i].isSeparator )
		{
			menuItemFlags = MF_SEPARATOR;
		}

		menuPopup.AppendMenu( menuItemFlags, items[i].id, items[i].caption );
	}

	menuPopup.Detach();
}

void CWindowBase::GetPopupMenuItems( TPopupMenuItemsCollection& items )
{
	items.RemoveAll();
}

bool CWindowBase::IsLeftButton( int id )
{
	return (id == iLeftButtonID);
}

void CWindowBase::DoExitApplication()
{
	LOGME();

	ControllersHost::GetInstance()->SettingsController()->SaveSettings();

	this->GetParent().PostMessage(WM_CLOSE);
}

void CWindowBase::DoHelp()
{
	LOGME();

	TCHAR htmlFormat[] = 	_T("<html><body>")
							_T("<p>%s</p>")
							_T("</body></html>");

	CString helpCaption, helpText;

	this->GetHelpCaption( helpCaption );
	this->GetHelpText( helpText );

	CString helpBody;

	helpBody.Format( htmlFormat, helpText );

	ShellHelper::ShowNotification( ESNHelpInformation, 30, m_hWnd, helpBody, helpCaption );

	//this->SetFocus();
}
