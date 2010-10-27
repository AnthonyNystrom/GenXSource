// SupuippcFrame.cpp : implementation of the CSupuippcFrame class
//
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#ifdef WIN32_PLATFORM_PSPC
#include "resourceppc.h"
#else
#include "resourcesp.h"
#endif

#include "SupuippcFrame.h"

#include <windowsmanager.h>

BOOL CSupuippcFrame::PreTranslateMessage(MSG* pMsg)
{
	if(CFrameWindowImpl<CSupuippcFrame>::PreTranslateMessage(pMsg))
		return TRUE; 

	return FALSE;
}

bool CSupuippcFrame::AppHibernate( bool bHibernate)
{
	// Insert your code here or delete member if not relevant
	return bHibernate;
}

bool CSupuippcFrame::AppNewInstance( LPCTSTR lpstrCmdLine)
{
	// Insert your code here or delete member if not relevant
	return false;
}

void CSupuippcFrame::AppSave()
{
	CAppInfo info;
	// Insert your code here
}

#ifdef WIN32_PLATFORM_WFSP
	void CSupuippcFrame::AppBackKey() 
{
	::SHNavigateBack();
}
#endif

BOOL CSupuippcFrame::OnIdle()
{
	UIUpdateToolBar();
	return FALSE;
}

LRESULT CSupuippcFrame::OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
{
	CAppInfo info;

	//this->SetIcon( ::LoadIcon(NULL, MAKEINTRESOURCE(IDI_APP_ICON) ));

	CreateSimpleCEMenuBar();
#ifdef WIN32_PLATFORM_WFSP // SmartPhone
	AtlActivateBackKey(m_hWndCECommandBar);
#endif 
	UIAddToolBar(m_hWndCECommandBar);

	RECT rcWnd = {0};
	GetWindowRect( &rcWnd );
	WindowsManager::GetInstnace()->InitializeManager( m_hWnd, &rcWnd );
	m_hWndClient = WindowsManager::GetInstnace()->GetWindow( EWndCredentials )->m_hWnd;

	// register object for message filtering and idle updates
	CMessageLoop* pLoop = _Module.GetMessageLoop();
	ATLASSERT(pLoop != NULL);
	pLoop->AddMessageFilter(this);
	pLoop->AddIdleHandler(this);

	return 0;
}

LRESULT CSupuippcFrame::OnCommand( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	WindowsManager::GetInstnace()->SendMessageToWindows( uMsg, wParam, lParam );
	return 0;
}


