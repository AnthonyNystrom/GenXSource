// BaseTabCtrl.cpp : implementation file
//

#include "stdafx.h"
#include "EGTabCtrl.h"
#include "EGMenu.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define PADDING 3

/////////////////////////////////////////////////////////////////////////////
// CEGTabCtrl

CEGTabCtrl::CEGTabCtrl( )
{
	m_clrBack = RGB( 247, 243, 233 );
	m_clrInactiveTab = RGB( 247, 243, 233 );
	m_clrActiveTab = ::GetSysColor( COLOR_BTNFACE );
	m_clrInactiveText = ::GetSysColor( COLOR_3DSHADOW );
	m_clrActiveText = ::GetSysColor( COLOR_WINDOWTEXT );
	m_clr3DLight = ::GetSysColor( COLOR_3DHILIGHT );
	m_clr3DShadow = ::GetSysColor( COLOR_WINDOWTEXT );
	m_clrSeparator = ::GetSysColor( COLOR_3DSHADOW );

	m_bCustomDraw = FALSE;
}

CEGTabCtrl::~CEGTabCtrl()
{
}


BEGIN_MESSAGE_MAP(CEGTabCtrl, CTabCtrl)
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CBaseTabCtrl message handlers

BOOL CEGTabCtrl::SetCustomDraw( BOOL bValue )
{
	m_bCustomDraw = bValue;

	if (GetSafeHwnd()) {
		if (m_bCustomDraw ) 
			ModifyStyle( 0, TCS_OWNERDRAWFIXED );
		else
			ModifyStyle( TCS_OWNERDRAWFIXED, 0 );

		Invalidate();
	}

	return TRUE;
}

void CEGTabCtrl::DrawItem( CDC* pDC, CRect* lprcBorder, int nTab, BOOL bSelected, BOOL bFocused )
{
	ASSERT ( m_bCustomDraw );

	CString sTemp;

	TC_ITEM     tci;
	tci.mask        = TCIF_TEXT | TCIF_IMAGE;
	tci.pszText     = sTemp.GetBuffer(100);
	tci.cchTextMax  = 99;
	GetItem(nTab, &tci);
	sTemp.ReleaseBuffer();

	//lprcBorder->bottom ++;
	//if( bSelected ) {
	//	lprcBorder->top -=2;
	//	lprcBorder->right +=2;
	//	if ( nTab > 0 )
	//		lprcBorder->left -=2;
	//}
	themeData.DrawTab( pDC, lprcBorder, NULL, (TCHAR*)(LPCTSTR)sTemp, ALIGN_TOP, (bSelected ? STYLE_ACTIVE : 0), themeData.clrBtnFace );
}


void CEGTabCtrl::DrawMainBorder( CDC* pDC, CRect* lprcBorder )
{
	ASSERT ( m_bCustomDraw );
	
	pDC->Draw3dRect( lprcBorder, m_clrInactiveText, m_clrInactiveText );
}

void CEGTabCtrl::PreSubclassWindow() 
{
	CTabCtrl::PreSubclassWindow();

	if ( m_bCustomDraw ) 
		ModifyStyle(0, TCS_OWNERDRAWFIXED);
}

void CEGTabCtrl::DrawHeaderBk( CDC* pDC, CRect* lprcHeader ){
	pDC->FillSolidRect( lprcHeader, m_clrBack );
	pDC->Draw3dRect( lprcHeader, m_clrInactiveText, m_clr3DLight );
}

BOOL CEGTabCtrl::OnEraseBkgnd(CDC* pDC) 
{
	if ( !m_bCustomDraw )
		return CTabCtrl::OnEraseBkgnd(pDC);

	if 	( GetItemCount() > 0 ) {
		CRect rcClient, rcTab;
		GetClientRect(rcClient);
		rcTab = rcClient;
		GetItemRect(0, rcTab);
		rcClient.bottom = rcTab.bottom + 3;

		themeData.DrawTabCtrlBK( pDC, &rcClient, ALIGN_TOP, TRUE, themeData.clrBtnFace );
	}
	return TRUE;
}

void CEGTabCtrl::OnPaint() 
{
	if ( !m_bCustomDraw ) {
		Default();
		return;
	}

	CPaintDC dc(this); // device context for painting
	CRect rcClient, rcItem;

	// prepare dc
	dc.SelectObject(GetFont());

	DRAWITEMSTRUCT dis;
	dis.CtlType = ODT_TAB;
	dis.CtlID = GetDlgCtrlID();
	dis.hwndItem = GetSafeHwnd();
	dis.hDC = dc.GetSafeHdc();
	dis.itemAction = ODA_DRAWENTIRE;

	int nTab = GetItemCount();

	// draw the rest of the border
	GetClientRect(&rcClient);
	rcItem = rcClient;
	if ( 0 == nTab ) {
		DrawMainBorder( &dc, &rcItem );
	} else {
		AdjustRect(FALSE, rcItem);
		rcItem.top += 2;
		rcItem.left = rcClient.left;
		rcItem.right = rcClient.right;
		rcItem.bottom = rcClient.bottom;
		DrawMainBorder( &dc, &rcItem );
	}


	// paint the tabs first and then the borders
	int nSel = GetCurSel();
	int nFocus = ::GetFocus() == m_hWnd ? GetCurFocus() : -1;


	if (!nTab) // no pages added
		return;

	while (nTab--)
		if (nTab != nSel)
			if ( GetItemRect(nTab, &rcItem) )
				DrawItem( &dc, &rcItem, nTab, nTab == nFocus );

	// now selected tab
	if ( GetItemRect(nSel, &rcItem) )
		DrawItem( &dc, &rcItem, nSel, TRUE, nSel == nFocus  );
}

