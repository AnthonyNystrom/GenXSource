// UidevtestppcFrame.cpp : implementation of the CUidevtestppcFrame class
//
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#ifdef WIN32_PLATFORM_PSPC
#include "resourceppc.h"
#else
#include "resourcesp.h"
#endif

#include "aboutdlg.h"
#include "UidevtestppcView.h"
#include "UidevtestppcFrame.h"

#include <windowsmanager.h>

CUidevtestppcFrame::CUidevtestppcFrame()
{
	WindowsManager::CreateInstance();
}

CUidevtestppcFrame::~CUidevtestppcFrame()
{
	WindowsManager::DeleteInstance();
}

BOOL CUidevtestppcFrame::PreTranslateMessage(MSG* pMsg)
{
	if(CFrameWindowImpl<CUidevtestppcFrame>::PreTranslateMessage(pMsg))
		return TRUE; 

	return m_view.IsWindow() ? m_view.PreTranslateMessage(pMsg) : FALSE;
}

bool CUidevtestppcFrame::AppHibernate( bool bHibernate)
{
	// Insert your code here or delete member if not relevant
	return bHibernate;
}

bool CUidevtestppcFrame::AppNewInstance( LPCTSTR lpstrCmdLine)
{
	// Insert your code here or delete member if not relevant
	return false;
}

void CUidevtestppcFrame::AppSave()
{
	CAppInfo info;
	// Insert your code here
}

#ifdef WIN32_PLATFORM_WFSP
	void CUidevtestppcFrame::AppBackKey() 
{
	::SHNavigateBack();
}
#endif

BOOL CUidevtestppcFrame::OnIdle()
{
	//UIUpdateToolBar();
	return FALSE;
}

LRESULT CUidevtestppcFrame::OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
{
	CAppInfo info;

	CreateSimpleCEMenuBar();
#ifdef WIN32_PLATFORM_WFSP // SmartPhone
	AtlActivateBackKey(m_hWndCECommandBar);
#endif 
	UIAddToolBar(m_hWndCECommandBar);

	
	//SHMENUBARINFO menuInfo = {0};
	//menuInfo.cbSize = sizeof(SHMENUBARINFO);
	//menuInfo.hwndParent = m_hWnd;
	//menuInfo.dwFlags = SHCMBF_HIDESIPBUTTON|SHCMBF_EMPTYBAR;
	//menuInfo.nToolBarId = 201;
	//menuInfo.hInstRes = ModuleHelper::GetResourceInstance();

	//if ( FALSE == ::SHCreateMenuBar( &menuInfo ) )
	//{
	//	DWORD error = ::GetLastError();
	//	ASSERT(FALSE);
	//}

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

LRESULT CUidevtestppcFrame::OnFileExit(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	PostMessage(WM_CLOSE);
	return 0;
}

LRESULT CUidevtestppcFrame::OnFileNew(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	// TODO: add code to initialize document

	return 0;
}

LRESULT CUidevtestppcFrame::OnAction(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	// TODO: add code

	return 0;
}

LRESULT CUidevtestppcFrame::OnAppAbout(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	CAboutDlg dlg;
	dlg.DoModal();
	return 0;
}

LRESULT CUidevtestppcFrame::OnCommand( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	WindowsManager::GetInstnace()->SendMessageToWindows( uMsg, wParam, lParam );

	return 0;
}
