// AppTabManager.cpp : implementation file
//

#include "stdafx.h"
#include "NuGenDimension.h"
#include "AppTabManager.h"
#include "DocManagerEx.h"


// CAppTabManager

IMPLEMENT_DYNAMIC(CAppTabManager, CControlBar)
CAppTabManager::CAppTabManager()
{
}

CAppTabManager::~CAppTabManager()
{
	std::vector<TAB_STRUCT*>::iterator itTabInfo;
	for ( itTabInfo = m_tab_infos.begin(); itTabInfo != m_tab_infos.end(); ++itTabInfo )
		delete (*itTabInfo);
}


#define ID_TAB 1


BEGIN_MESSAGE_MAP(CAppTabManager, CControlBar)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_NOTIFY( TCN_SELCHANGE, ID_TAB, OnTabChanged )
END_MESSAGE_MAP()



// CTabBar message handlers
void CAppTabManager::OnTabChanged( NMHDR * /* pNotifyStruct */, LRESULT * /* result */ ) 
{
	TC_ITEM tci;
	tci.mask = TCIF_PARAM;
	m_tabs.GetItem(m_tabs.GetCurSel(), &tci );
	((CDocManagerEx*)theApp.m_pDocManager)->ActivateFrame((CDocTemplate*)tci.lParam);
}

void CAppTabManager::AddTab( CDocTemplate* dT, const char* str) 
{
	TAB_STRUCT* lpWndTabInfo = new TAB_STRUCT;
	lpWndTabInfo->rtClass = dT;
	memset( lpWndTabInfo->szText, 0x00, (101)* sizeof( TCHAR ) );
//	strcpy(lpWndTabInfo->szText,str);#OBSOLETE
	strcpy_s(lpWndTabInfo->szText, sizeof (lpWndTabInfo->szText), str);
	//::GetWindowText( hWnd, lpWndTabInfo->szText, 101 );
	lpWndTabInfo->nTabId = m_tabs.InsertItem( TCIF_PARAM | TCIF_TEXT, 
		m_tabs.GetItemCount(), lpWndTabInfo->szText, -1, (LPARAM)dT );
	m_tab_infos.push_back( lpWndTabInfo );
}


int CAppTabManager::TabIdByRT( CDocTemplate* dT)
{
	// Get nTabId by HWND-data of tab
	TC_ITEM tci;
	tci.mask = TCIF_PARAM;
	int nCount = m_tabs.GetItemCount();
	for (int i = 0; i < nCount; ++i ) {
		m_tabs.GetItem(i, &tci );
		if ( dT == (CDocTemplate*)tci.lParam )
			return i;
	}
	return -1;
}

void CAppTabManager::RemoveTab(CDocTemplate* dT) 
{
	std::vector<TAB_STRUCT*>::iterator itTabInfo;

	// deleting referenced tab infos
	for ( itTabInfo = m_tab_infos.begin(); itTabInfo != m_tab_infos.end(); ++itTabInfo ) 
		if ( dT == (*itTabInfo)->rtClass ) { // here is
			TAB_STRUCT* lpInfo = (*itTabInfo);
			m_tab_infos.erase( itTabInfo );
			delete lpInfo;
			break;
		}

		// deleting referenced tab
		m_tabs.DeleteItem( TabIdByRT( dT ) );

		// re-initialize nTabId members of WND_TAB_INFO
		for ( itTabInfo = m_tab_infos.begin(); itTabInfo != m_tab_infos.end(); ++itTabInfo )
			(*itTabInfo)->nTabId = TabIdByRT( (*itTabInfo)->rtClass );

		// activate new tab
		/*if ( m_tabs.GetItemCount() > 0 ) {
			ActivateTab( hWndActive );
		}*/


}

void CAppTabManager::ActivateTab( CDocTemplate* dT) {

	TC_ITEM tci;
	tci.mask = TCIF_PARAM;
	m_tabs.GetItem( m_tabs.GetCurSel(), &tci );
	if ( dT != (CDocTemplate*)tci.lParam )
	{
		m_tabs.SetCurSel( TabIdByRT( dT ) );
	}
}


// CAppTabManager message handlers


void CAppTabManager::OnUpdateCmdUI(CFrameWnd* /*pTarget*/, BOOL /*bDisableIfNoHndler*/)
{
}

BOOL CAppTabManager::CreateAppTabManager( CFrameWnd* pTarget ) {

	ASSERT( pTarget );

	m_pFrameWnd = pTarget;
//	m_hWndMDIClient = ( (CMDIFrameWnd*)m_pFrameWnd )->m_hWndMDIClient;

	CString strWndclass = ::AfxRegisterWndClass(CS_DBLCLKS, ::LoadCursor(NULL, IDC_ARROW), ::GetSysColorBrush(COLOR_BTNFACE), 0);
	if (!CWnd::Create(strWndclass, _T("AppManager"), 
		WS_CHILD | CBRS_TOP | WS_VISIBLE|WS_CLIPCHILDREN | CCS_NOPARENTALIGN | CCS_NOMOVEY | CCS_NORESIZE,
		CRect(0, 0, 100, 26), pTarget, 31255))
		return FALSE;

	SetBarStyle(GetBarStyle() | CBRS_TOP | CBRS_TOOLTIPS /* | CBRS_FLYBY | CBRS_SIZE_DYNAMIC */);
	//EnableDocking( CBRS_ALIGN_TOP );
	//pTarget->DockControlBar(this); 

	return TRUE;
}

int CAppTabManager::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CControlBar::OnCreate(lpCreateStruct) == -1)
		return -1;

	// Настроили закладки 
	m_tabs.Create(WS_CHILD | WS_VISIBLE | WS_EX_NOPARENTNOTIFY | TCS_TOOLTIPS | TCS_SINGLELINE | TCS_FORCELABELLEFT, 
		CRect(0, 0, 100, 26), this, ID_TAB);

	m_tabs.SetCustomDraw( TRUE );

	return 0;
}

CSize CAppTabManager::CalcFixedLayout(BOOL /* bStretch */, BOOL bHorz)
{
	ASSERT_VALID(this);
	ASSERT(::IsWindow(m_hWnd));

	CClientDC dc(NULL);
	HFONT hFont    = reinterpret_cast<HFONT>(SendMessage(WM_GETFONT));
	HFONT hOldFont = NULL;
	if (hFont != NULL)
		hOldFont = reinterpret_cast<HFONT>(dc.SelectObject(hFont));
	TEXTMETRIC tm;
	VERIFY(dc.GetTextMetrics(&tm));
	if (hOldFont != NULL)
		dc.SelectObject(hOldFont);

	// get border information
	CSize size;
	CRect rcInside, rcWnd; 
	rcInside.SetRectEmpty();
	CalcInsideRect(rcInside, bHorz);
	GetParentFrame()->GetWindowRect(&rcWnd);
	size.cx = rcWnd.Width();
	size.cy = tm.tmHeight + tm.tmInternalLeading + 
		::GetSystemMetrics(SM_CYBORDER) * 2 - rcInside.Height();

	return size;
}

CSize CAppTabManager::CalcDynamicLayout(int /* nLength */, DWORD nMode)
{
	ASSERT(nMode & LM_HORZ);
	return CalcFixedLayout(nMode & LM_STRETCH, nMode & LM_HORZ);
}
void CAppTabManager::OnSize(UINT nType, int cx, int cy)
{
	CControlBar::OnSize(nType, cx, cy);
	m_tabs.MoveWindow( 0, 0, cx, cy);
}

