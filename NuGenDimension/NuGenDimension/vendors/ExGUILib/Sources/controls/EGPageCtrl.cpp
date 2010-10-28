#include "StdAfx.h"
#include "EGPageCtrl.h"

CEGPageCtrl::CEGPageCtrl(void)
{
	m_pActivePage = NULL;
}

CEGPageCtrl::~CEGPageCtrl(void)
{
}

BEGIN_MESSAGE_MAP( CEGPageCtrl, CEGTabCtrl )
	ON_NOTIFY_REFLECT( TCN_SELCHANGE, OnTabChanged )
	//ON_WM_GETDLGCODE( )
END_MESSAGE_MAP()

void CEGPageCtrl::OnTabChanged( NMHDR* /* pNotifyStruct */, LRESULT* /* result */ ) {
	// TODO: OnKillActive, OnNext and other..
	if ( m_pActivePage )
		m_pActivePage->ShowWindow( SW_HIDE );

	// TODO: OnSetActive, OnNext and other..
	int nSel = GetCurSel();
	if ( -1 == nSel ){
		m_pActivePage = NULL;
		return;
	} else {
		TC_ITEM tci;
		tci.mask = TCIF_PARAM;
		GetItem( nSel, &tci );
		m_pActivePage = (CPropertyPage*) tci.lParam;
		if ( m_pActivePage ) {
			CRect rcPage = GetPageRect(); 
			m_pActivePage->MoveWindow( &rcPage );
			m_pActivePage->ShowWindow( SW_SHOW );
		}
	}
}

CRect CEGPageCtrl::GetPageRect(){
	
	CRect rcClient, rcPage;
	GetClientRect( &rcClient );
	
	rcPage = rcClient;
	AdjustRect( FALSE, rcPage );
	
	rcPage.top += 3;
	rcPage.bottom = rcClient.bottom - 1;
	rcPage.left = rcClient.left + 1;
	rcPage.right = rcClient.right - 1;
	
	CRect rcWindow;
	GetWindowRect( &rcWindow );
	POINT pt = rcWindow.TopLeft();
	::ScreenToClient( GetParent()->m_hWnd, &pt );

	rcPage.OffsetRect( pt.x, pt.y );

	return rcPage;
}

void CEGPageCtrl::AddPage( CPropertyPage* pPage, UINT nResourceID, TCHAR* lpszTitle ){

	// В список
	m_pages.push_back(pPage);

	// Закладка
	InsertItem( TCIF_PARAM | TCIF_TEXT, GetItemCount(), lpszTitle, -1, (LPARAM) pPage );

	// Страничка
	pPage->Create( nResourceID, GetParent() );
	pPage->SetParent( GetParent() );
	pPage->ShowWindow( SW_HIDE );

	// Корректировка стилей
//	::SetWindowLong( pPage->m_hWnd, GWL_STYLE, WS_CHILD | WS_CLIPCHILDREN | WS_CLIPSIBLINGS );
	
	pPage->ModifyStyle( WS_BORDER, 0 );
	pPage->ModifyStyle( WS_DLGFRAME, 0 );
	pPage->ModifyStyle( WS_POPUPWINDOW, 0 );
	pPage->ModifyStyleEx( WS_EX_DLGMODALFRAME, 0 );
	pPage->ModifyStyleEx( WS_EX_WINDOWEDGE, 0 );
	pPage->ModifyStyle( 0, WS_CHILD );

	// Положение
	if ( !m_pActivePage ) {
		m_pActivePage = pPage;
		CRect rcPage = GetPageRect();
		pPage->MoveWindow( &rcPage );
		pPage->ShowWindow( SW_SHOW );
	}
}

BOOL CEGPageCtrl::TryFinish() {

	int nItem = 0;
	vector<CPropertyPage*>::iterator itPage = m_pages.begin();
	for ( ; itPage != m_pages.end(); ++itPage ) {
		if ( !(*itPage)->OnWizardFinish() ) {
			SetCurSel( nItem );
			OnTabChanged( NULL, NULL );
			return FALSE; // unable to finish
		}
		nItem++;
	}
	return TRUE;
}

/*
UINT CEGPageCtrl::OnGetDlgCode( ) {

	UINT nResult = CEGTabCtrl::OnGetDlgCode( );

	nResult |= DLGC_WANTALLKEYS | DLGC_WANTTAB;

	return nResult;
}
*/