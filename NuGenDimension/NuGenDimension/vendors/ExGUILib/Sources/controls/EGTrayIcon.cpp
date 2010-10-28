#include "stdafx.h"
#include "EGTrayIcon.h"

DWORD WINAPI AnimateProc(LPVOID lpParameter){
	CEGTrayIcon* pTrayIcon = (CEGTrayIcon*)lpParameter;
	pTrayIcon->Animate();
	return 0;
}

CEGTrayIcon::CEGTrayIcon(){
	m_bMainView = TRUE;
	m_bIconExists = FALSE;
	hStopEvent = ::CreateEvent(NULL,TRUE,false,NULL);
	m_hMain = NULL;
	m_szTitle = NULL;
	m_szSecondTitle = NULL;
	m_hMenu = ::CreatePopupMenu();
	::AppendMenu( m_hMenu, MF_STRING, 5000, _T("Главное окно") );  
	::AppendMenu( m_hMenu, MF_SEPARATOR, 0, _T("-") );  
	::AppendMenu( m_hMenu, MF_STRING, ID_APP_EXIT, _T("Выход") );  
	::SetMenuDefaultItem( m_hMenu, 5000, FALSE );
	m_hSourceMenu = m_hMenu;
}

CEGTrayIcon::~CEGTrayIcon(){
	if(m_hMain) {
		NOTIFYICONDATA nid;
		nid.cbSize = sizeof(NOTIFYICONDATA);
		nid.hWnd = m_hWnd;
		nid.uID = 0;
		nid.uFlags = NIF_ICON;
		Shell_NotifyIcon(NIM_DELETE,&nid);
	}
	if ( m_szTitle )
		free( m_szTitle );
	if ( m_szSecondTitle )
		free( m_szSecondTitle );
	::CloseHandle(hStopEvent);
	if ( m_hSourceMenu )
		::DestroyMenu( m_hSourceMenu );
}

void CEGTrayIcon::_SetIcon(HICON hIcon, TCHAR* pszTitle){
	NOTIFYICONDATA nid;
	nid.cbSize = sizeof(NOTIFYICONDATA);
	nid.hWnd = m_hWnd;
	nid.uID = 0;
	nid.uFlags = NIF_ICON | NIF_TIP | NIF_MESSAGE;
	nid.hIcon = hIcon;
	nid.uCallbackMessage = m_nNotify;
	_tcsncpy(nid.szTip, pszTitle, sizeof(nid.szTip) );
	if (m_bIconExists)
		Shell_NotifyIcon(NIM_MODIFY,&nid);
	else
		m_bIconExists = Shell_NotifyIcon(NIM_ADD,&nid);
}

void CEGTrayIcon::SetIcon(HICON hIcon, TCHAR* szTitle ){
	if( m_szTitle )
		free( m_szTitle );
	m_szTitle = _tcsdup( szTitle );
	m_hMain = hIcon;
	_SetIcon( m_hMain, m_szTitle);
}

void CEGTrayIcon::SetSecondIcon(HICON hIcon, TCHAR* szTitle ){
	if( m_szSecondTitle )
		free( m_szSecondTitle );
	m_szSecondTitle = _tcsdup( szTitle );
	m_hSecond = hIcon;
}

void CEGTrayIcon::SetView( BOOL bMain ){
	if ( bMain == m_bMainView ) 
		return;
	m_bMainView = bMain;
	_SetIcon( m_bMainView ? m_hMain : m_hSecond, m_bMainView ?  m_szTitle : m_szSecondTitle );
}

BOOL CEGTrayIcon::StartAnimate(HICON* hIcons, int nCount, int nInterval){
		m_nInterval = nInterval;
		m_nCount = nCount;
		m_hIcons = hIcons;
		DWORD dwThreadID;
		if (hStopEvent == NULL) return FALSE;
		::ResetEvent(hStopEvent);
		::CreateThread(NULL,0,::AnimateProc,(LPVOID)this,0,&dwThreadID);
		return TRUE;
	}

void CEGTrayIcon::StopAnimate(){
	SetEvent(hStopEvent);
}

void CEGTrayIcon::Animate(){
	int nIndex=0;
	while(true){
		_SetIcon(m_hIcons[nIndex], m_szTitle);
		if (WaitForSingleObject(hStopEvent,m_nInterval)!=WAIT_TIMEOUT) break;
		nIndex++;
		if (nIndex == m_nCount) nIndex = 0;
	}
	_SetIcon(m_hMain, m_szTitle);
}

void CEGTrayIcon::SetMenu( UINT nMenu,  UINT nSubMenu, HINSTANCE hInst ) {
	SetMenu( MAKEINTRESOURCE( nMenu ), nSubMenu, hInst );
}

void CEGTrayIcon::SetMenu( LPCSTR pszMenu,  UINT nSubMenu, HINSTANCE hInst ) {
	HMENU hMenu = ::LoadMenu( hInst, pszMenu );
	HMENU hSubMenu = ::GetSubMenu(hMenu, nSubMenu );
	if ( m_hSourceMenu )
		::DestroyMenu( m_hSourceMenu );
	m_hSourceMenu = hMenu;
	SetMenu( hSubMenu );
}

void CEGTrayIcon::SetMenu( HMENU hMenu ) {
	m_hMenu = hMenu;
	::SetMenuDefaultItem( m_hMenu, 5000, FALSE );
}