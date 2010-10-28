#include "stdafx.h"
#include "EGListCtrl.h"


BEGIN_MESSAGE_MAP(CEGListCtrl, CListCtrl)
	ON_WM_CREATE()
	ON_WM_CONTEXTMENU()
END_MESSAGE_MAP()

// CEGListCtrl construction/destruction

CEGListCtrl::CEGListCtrl(void)
{
	m_pMenuContext = NULL;
	m_pWndMenuHandler = NULL;
}

CEGListCtrl::~CEGListCtrl(void)
{
}

// CEGListCtrl message handlers

int CEGListCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CListCtrl::OnCreate(lpCreateStruct) == -1)
		return -1;

	ListView_SetExtendedListViewStyle(m_hWnd, LVS_EX_HEADERDRAGDROP | LVS_EX_FULLROWSELECT );

	m_ctlHeader.SubclassWindow( ListView_GetHeader( m_hWnd ) );

	return 0L;
}

void CEGListCtrl::OnContextMenu( CWnd* pWnd, CPoint pt) {
	if ( m_pMenuContext )
		m_pMenuContext->TrackPopupMenu( 0, pt.x, pt.y, m_pWndMenuHandler ? m_pWndMenuHandler : pWnd );
}

void CEGListCtrl::SetMenu( CEGMenu *pMenu, CWnd* pWnd ) {
	m_pMenuContext = pMenu;
	m_pWndMenuHandler = pWnd;
}

