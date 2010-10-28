// DrawFrame.cpp : implementation file
//

#include "stdafx.h"
#include "EGDockingFlyOut.h"
#include "EGDocking.h"
#include "NewTheme.h"
#include "EgMenu.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

int CALLBACK EnumFontFamProc(ENUMLOGFONT FAR *lpelf,
                             NEWTEXTMETRIC FAR *lpntm,
                             int FontType,
                             LPARAM lParam);
//#define WS_EX_LAYERED 0x80000
//#define LWA_ALPHA 0x2
//#define LWA_COLORKEY 0x1

#define SHOWOFFSTEP 50 
#define STEPTIME 50
#define TRACK_AREA_SIZE 4

/////////////////////////////////////////////////////////////////////////////
// CEGFlyOutPane

CEGFlyOutPane::CEGFlyOutPane( )
{
	CDC dc;
  dc.CreateCompatibleDC(NULL);

  m_sFontFace = (::EnumFontFamilies(dc.m_hDC,
		_T("Tahoma"), (FONTENUMPROC) EnumFontFamProc, 0) == 0) ?
    _T("Tahoma") : _T("Arial");

  int ppi = dc.GetDeviceCaps(LOGPIXELSX);
  int pointsize = MulDiv(85, 96, ppi); // 8.5 points at 96 ppi

	m_fntThin.CreatePointFont(pointsize, m_sFontFace);

	m_Size = 200;
	m_cyCaption = 16;

	m_bShowOff = FALSE;
	m_pPane = NULL;
	m_pBorder = NULL;
	m_bClosingUp = FALSE;
}

CEGFlyOutPane::~CEGFlyOutPane()
{
}

BOOL CEGFlyOutPane::Create( CWnd *pParentWnd, const CRect& rc, int nStyle )
{

	if( !CreateEx( WS_EX_TOPMOST, AfxRegisterWndClass(CS_DBLCLKS | CS_VREDRAW | CS_HREDRAW),
		NULL, WS_CHILD | WS_CLIPCHILDREN | WS_CLIPSIBLINGS, CRect( 0, 0, 0, 0), pParentWnd, 0) )
		return FALSE;

	m_nStyle = nStyle;

	return TRUE;
}

void CEGFlyOutPane::SetPane( CEGDockingPane * pPane ) {
	m_pPane = pPane;

	if ( m_pPane ) {
		SetWindowText( m_pPane->GetTitle() );
		m_pPane->SetParent( this );
		m_pPane->ShowWindow( SW_SHOW );
	}
}

void CEGFlyOutPane::SetBorder( CEGDockBorder * pBorder ) {
	m_pBorder = pBorder;
}

void CEGFlyOutPane::Hide() {
	ShowWindow( SW_HIDE );
	
	ASSERT( m_pBorder != NULL );

	m_pBorder->OnHideFlyOut();
}

BEGIN_MESSAGE_MAP(CEGFlyOutPane, CWnd)
	ON_WM_CREATE()
	ON_WM_PAINT()
	ON_WM_NCACTIVATE()
	ON_WM_TIMER()
	ON_WM_NCHITTEST()
	ON_WM_NCCALCSIZE()
	ON_WM_SIZE()
	ON_WM_NCPAINT()
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_GETMINMAXINFO()
	ON_WM_KILLFOCUS()

	ON_MESSAGE( WM_SHOWIT, ShowOff )
	ON_MESSAGE( WM_SHOWHIDE, ShowHide )
	ON_MESSAGE( WM_SHOWIT_NOFOCUS, ShowOffNoFocus )
	ON_MESSAGE( WM_FOCUS_CHANGED, OnFocusChanged )
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CEGFlyOutPane message handlers

int CEGFlyOutPane::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if (CWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	m_cyCaption = ::GetSystemMetrics(SM_CYSMCAPTION);
	
	SetTimer( 3, 500, NULL );

	return 0;
}

void CEGFlyOutPane::RemoveTrackBorder( CRect * pRc ) {
	switch( m_nStyle ) {
		case ALIGN_TOP:
			break;
		case ALIGN_BOTTOM:
			pRc->top += TRACK_AREA_SIZE;
			break;
		case ALIGN_LEFT:
			pRc->right -= TRACK_AREA_SIZE;
			break;
		case ALIGN_RIGHT:
			pRc->left += TRACK_AREA_SIZE;
			break;
	}
}

void CEGFlyOutPane::DrawTrackBorder( CDC * pDC, CRect rc ) {

	CRect rcBk = rc, rcLine1, rcLine2;

	// Fill Client area
	switch( m_nStyle ) {
		case ALIGN_TOP:
			rc.bottom -= TRACK_AREA_SIZE;
			rcBk.top = rc.bottom;
			rcLine1 = rcBk;
			rcLine1.top += 2;
			rcLine1.bottom = rcLine1.top + 1;
			rcLine2 = rcLine1;
			rcLine2.bottom += 1;
			rcLine2.top += 1;
			pDC->FillSolidRect( rcBk, GetSysColor( COLOR_BTNFACE ) );
			pDC->FillSolidRect( rcLine1, GetSysColor( COLOR_BTNSHADOW ) );
			pDC->FillSolidRect( rcLine2, RGB( 0, 0, 0 ) );
			break;
		case ALIGN_BOTTOM:
			rc.top += TRACK_AREA_SIZE;
			rcBk.bottom = rc.top; 
			rcLine1 = rcBk;
			rcLine1.bottom -= 2;
			rcLine1.top = rcLine1.bottom - 1;
			rcLine2 = rcLine1;
			rcLine2.bottom -= 1;
			rcLine2.top -= 1;
			pDC->FillSolidRect( rcBk, GetSysColor( COLOR_BTNFACE ) );
			pDC->FillSolidRect( rcLine1, RGB( 255, 255, 255 ) );
			pDC->FillSolidRect( rcLine2, GetSysColor( COLOR_3DLIGHT ) );
			break;
		case ALIGN_LEFT:
			rc.right -= TRACK_AREA_SIZE;
			rcBk.left = rc.right;
			rcLine1 = rcBk;
			rcLine1.left += 2;
			rcLine1.right = rcLine1.left + 1;
			rcLine2 = rcLine1;
			rcLine2.left += 1;
			rcLine2.right += 1;
			pDC->FillSolidRect( rcBk, GetSysColor( COLOR_BTNFACE ) );
			pDC->FillSolidRect( rcLine1, GetSysColor( COLOR_BTNSHADOW ) );
			pDC->FillSolidRect( rcLine2, RGB( 0, 0, 0 ) );
			break;
		case ALIGN_RIGHT:
			rc.left += TRACK_AREA_SIZE;
			rcBk.right = rc.left; 
			rcLine1 = rcBk;
			rcLine1.right -= 2;
			rcLine1.left = rcLine1.right - 1;
			rcLine2 = rcLine1;
			rcLine2.right -= 1;
			rcLine2.left -= 1;
			pDC->FillSolidRect( rcBk, GetSysColor( COLOR_BTNFACE ) );
			pDC->FillSolidRect( rcLine1, RGB( 255, 255, 255 ) );
			pDC->FillSolidRect( rcLine2, GetSysColor( COLOR_3DLIGHT ) );
			break;
	}
}

void CEGFlyOutPane::OnPaint() 
{
	CPaintDC dc(this); // device context for painting
	
	CRect rc;
	GetClientRect( rc );
	dc.FillSolidRect( rc, GetSysColor( COLOR_WINDOW ) );
	
	// Do not call CWnd::OnPaint() for painting messages
}


BOOL CEGFlyOutPane::OnNcActivate(BOOL bActive) 
{

	if(!bActive)
		PostMessage(WM_SHOWHIDE);
	return CWnd::OnNcActivate(bActive);

}

void CEGFlyOutPane::OnTimer(UINT nIDEvent) 
{
	if(nIDEvent == 2) {
		m_bShowOff = FALSE;
		KillTimer(2);
		switch( m_nStyle )
		{
			case ALIGN_TOP:
				if(m_rcNow.Height() <= SHOWOFFSTEP ) {
					Hide();
					return;
				}
				m_rcNow.bottom -= SHOWOFFSTEP;
				break;
			case ALIGN_BOTTOM:
				if(m_rcNow.Height() <= SHOWOFFSTEP ) {
					Hide();
					return;
				}
				m_rcNow.top += SHOWOFFSTEP;
				break;
			case ALIGN_LEFT:
				if(m_rcNow.Width() <= SHOWOFFSTEP ) {
					Hide();
					return;
				}
				m_rcNow.right -= SHOWOFFSTEP;
				break;
			case ALIGN_RIGHT:
				if(m_rcNow.Width() <= SHOWOFFSTEP ) {
					Hide();
					return;
				}
				m_rcNow.left += SHOWOFFSTEP;
				break;
		}
		m_bShowOff = TRUE;
		MoveWindow(m_rcNow);
		SetTimer(2, STEPTIME, NULL);
	} else {
		if ( ::GetCapture() != m_hWnd ) {
			CPoint pt;
			::GetCursorPos(&pt);
			ScreenToClient(&pt);
			pt.y += m_cyCaption;
			if ( m_bPinHover && !m_rcPin.PtInRect( pt ) ) {
				m_bPinHover = FALSE;
        RePaint();
			} else if ( m_bCloseHover && !m_rcClose.PtInRect( pt ) ) {
				m_bCloseHover = FALSE;
        RePaint();
			}
		}
	}

	CWnd::OnTimer(nIDEvent);
}

#if _MFC_VER < 0x0800  
  afx_msg UINT CEGFlyOutPane::OnNcHitTest(CPoint point)
  {
#else
  afx_msg LRESULT CEGFlyOutPane::OnNcHitTest(CPoint point)
  {
#endif
  //UINT hit =  CWnd::OnNcHitTest(point);#WARNING
	CWnd::OnNcHitTest(point);

	CPoint ptClient = point;
	ScreenToClient( &ptClient );
	ptClient.y += m_cyCaption;
  if( m_rcCaption.PtInRect( ptClient ) )
		return HTCLIENT;

	CRect rc;
	GetWindowRect( &rc );
	switch(m_nStyle)
	{
		case ALIGN_TOP:
			rc.top = rc.bottom - TRACK_AREA_SIZE; 
			if ( rc.PtInRect( point ) )
				return HTBOTTOM;
			break;

		case ALIGN_BOTTOM:
			rc.bottom = rc.top + TRACK_AREA_SIZE; 
			if ( rc.PtInRect( point ) )
				return HTTOP;
			break;

		case ALIGN_LEFT:
			rc.left = rc.right - TRACK_AREA_SIZE; 
			if ( rc.PtInRect( point ) )
				return HTRIGHT;
			break;

		case ALIGN_RIGHT:
			rc.right = rc.left + TRACK_AREA_SIZE; 
			if ( rc.PtInRect( point ) )
				return HTLEFT;
			break;

	}
		
	return HTNOWHERE;
}

void CEGFlyOutPane::OnNcCalcSize(BOOL bCalcValidRects, NCCALCSIZE_PARAMS FAR* lpncsp) 
{
	CWnd::OnNcCalcSize(bCalcValidRects, lpncsp);
	CRect rcClient = lpncsp->rgrc[0];

	// gripper coordinates
	m_rcCaption = rcClient;
	m_rcCaption.top = 0;
	m_rcCaption.left = 0;
	m_rcCaption.bottom = min( m_cyCaption, rcClient.bottom - rcClient.top );
	m_rcCaption.right = rcClient.Width();

	// Removing gripper from client area
	lpncsp->rgrc[0].top += min( m_cyCaption, rcClient.bottom - rcClient.top );

	// removing tracking border from client area
	switch( m_nStyle ) {
		case ALIGN_TOP:
			lpncsp->rgrc[0].bottom -= min( TRACK_AREA_SIZE, rcClient.bottom - rcClient.top );
			break;
		case ALIGN_BOTTOM:
			lpncsp->rgrc[0].top += min( TRACK_AREA_SIZE, rcClient.bottom - rcClient.top );
			break;
		case ALIGN_LEFT:
			lpncsp->rgrc[0].right -= min( TRACK_AREA_SIZE, rcClient.right - rcClient.left );
			break;
		case ALIGN_RIGHT:
			lpncsp->rgrc[0].left += min( TRACK_AREA_SIZE, rcClient.right - rcClient.left );
			break;
	}

	// Buttons coordinates
	SetRect(&m_rcClose, m_rcCaption.right-m_cyCaption+2, m_rcCaption.top+1, m_rcCaption.right-1, m_rcCaption.bottom-1 );
	SetRect(&m_rcPin,	m_rcClose.left - m_cyCaption+2,	m_rcClose.top,	m_rcClose.left-1, m_rcClose.bottom);

//	ScreenToClient( &m_rcClose );
//	ScreenToClient( &m_rcPin );
}

void CEGFlyOutPane::OnSize(UINT nType, int cx, int cy) 
{
	CWnd::OnSize(nType, cx, cy);


	if ( m_pPane )
		m_pPane->MoveWindow( 0, 0, cx, cy );

	if( m_bShowOff)
		return;
		
	switch(m_nStyle)
	{
		case ALIGN_TOP:
		case ALIGN_BOTTOM:
			m_Size = cy;
			break;
		case ALIGN_LEFT:
		case ALIGN_RIGHT:
			m_Size = cx;
			break;
	}
}

#ifndef COLOR_GRADIENTACTIVECAPTION
#define COLOR_GRADIENTACTIVECAPTION     27
#define COLOR_GRADIENTINACTIVECAPTION   28
#define SPI_GETGRADIENTCAPTIONS         0x1008
#endif

void CEGFlyOutPane::OnNcPaint() 
{
	CWnd::OnNcPaint();
	
	CWindowDC dc(this);

	CRect rc;
  GetWindowRect(rc);
  rc.OffsetRect(-rc.TopLeft());
	
	DrawTrackBorder( &dc, rc );
	RemoveTrackBorder( &rc );

	rc.bottom = rc.top + m_cyCaption;


	CString strTitle;
	GetWindowText( strTitle );
	
	DWORD dwFlags = 0;
	HWND hFocus = ::GetFocus();
	if( m_hWnd == hFocus || ::IsChild( m_hWnd, hFocus ) )
		dwFlags |= DGF_GRIPPER_ACTIVE;
	dwFlags |= DGF_PIN_VISIBLE;
	dwFlags |= DGF_PIN_HORZ;
	if ( m_bPinHover )
		dwFlags |= DGF_PIN_HOVER;
	if ( m_bPinPressed )
		dwFlags |= DGF_PIN_PRESSED;
	if ( m_bCloseHover )
		dwFlags |= DGF_CLOSE_HOVER;
	if ( m_bClosePressed )
		dwFlags |= DGF_CLOSE_PRESSED;

	themeData.DrawGripper( &dc, &rc, /*&m_fntThin,*/ (TCHAR*)(LPCTSTR)strTitle, dwFlags );

	// Do not call CWnd::OnNcPaint() for painting messages
}

void CEGFlyOutPane::RePaint() {
	SendMessage(WM_NCPAINT);	
}

void CEGFlyOutPane::OnMouseMove(UINT nFlags, CPoint point) 
{
	point.y += m_cyCaption;

	if ( m_rcClose.PtInRect( point ) ) {
		if( !m_bCloseHover ) {
			m_bCloseHover = TRUE;
			RePaint(); 
			return;
		}
	} else {
		if ( m_bCloseHover ) {
			m_bCloseHover = FALSE;
			RePaint(); 
			return;
		}
	}

	if ( m_rcPin.PtInRect( point ) ) {
		if( !m_bPinHover ) {
			m_bPinHover = TRUE;
			RePaint(); 
			return;
		}
	} else {
		if ( m_bPinHover ) {
			m_bPinHover = FALSE;
			RePaint(); 
			return;
		}
	}
	
	CWnd::OnMouseMove( nFlags, point);
}



void CEGFlyOutPane::OnLButtonUp(UINT nFlags, CPoint point) 
{
	point.y += m_cyCaption;

	// X-button pressed?
	BOOL bOldClosePressed = m_bClosePressed;
	m_bClosePressed = FALSE;
	if ( bOldClosePressed ) {
		ReleaseCapture();
		if ( m_rcClose.PtInRect( point ) ) {
			m_pPane = NULL;
			ShowWindow( SW_HIDE );
			m_pBorder->FlyDownBar( FALSE );
//			RePaint();////GetParentFrame()->ShowControlBar( this, FALSE, FALSE );
		} else {
			RePaint();
		}
	}

	// Pin-button pressed?
	BOOL bOldPinPressed = m_bPinPressed;
	m_bPinPressed = FALSE;
	if ( bOldPinPressed ) {
		ReleaseCapture();
		if ( m_rcPin.PtInRect( point ) ) {
			m_pPane = NULL;
			ShowWindow( SW_HIDE );
			m_pBorder->FlyDownBar( TRUE );
		} else {
			RePaint();
		}
	}

	CWnd::OnLButtonUp(nFlags, point);
}

void CEGFlyOutPane::OnLButtonDown(UINT nFlags, CPoint point) 
{
	BOOL bNeedRepaint = FALSE;

	HWND hActiveWnd = ::GetFocus();
	if ( hActiveWnd != m_hWnd && !::IsChild( m_hWnd, hActiveWnd ) ) {
		HWND hChild = ::GetWindow( m_hWnd, GW_CHILD );
		hChild = ::GetWindow( hChild, GW_CHILD );
		::SetFocus( hChild != NULL ? hChild : m_hWnd );
		bNeedRepaint = TRUE;
	}

	point.y += m_cyCaption;

	if ( point.y > 0 && point.y < m_cyCaption ) {
		if ( m_rcClose.PtInRect( point ) ) {
			SetCapture();
			m_bClosePressed = TRUE;
			bNeedRepaint = TRUE;
		} else if ( m_rcPin.PtInRect( point ) ) {
			SetCapture();
			m_bPinPressed = TRUE;
			bNeedRepaint = TRUE;
		} else if ( !m_bActive ) {
			bNeedRepaint = TRUE;
		}
	}

	if ( bNeedRepaint )
		RePaint();

	CWnd::OnLButtonDown(nFlags, point);
}

LRESULT CEGFlyOutPane::ShowHide(WPARAM, LPARAM)
{
	ShowWindow(SW_HIDE);
	return 0;
}


void CEGFlyOutPane::SlideHide( ) 
{
	if ( !IsWindowVisible() )
		return;

/*
	m_rcDest = m_rcNow;

	m_bClosePressed = FALSE;
	m_bCloseHover = FALSE;

	m_bPinPressed = FALSE;
	m_bPinHover = FALSE;

	DWORD dwDirection = 0;
	switch( m_nStyle )
	{
		case ALIGN_TOP:
			dwDirection = AW_VER_NEGATIVE;
			m_rcDest.bottom = m_rcDest.top + 2;
			break;
		case ALIGN_BOTTOM:
			dwDirection = AW_VER_POSITIVE;
			m_rcDest.top = m_rcDest.bottom - 2;
			break;
		case ALIGN_LEFT:
			dwDirection = AW_HOR_NEGATIVE;
			m_rcDest.right = m_rcDest.left + 2;
			break;
		case ALIGN_RIGHT:
			dwDirection = AW_HOR_POSITIVE;
			m_rcDest.left = m_rcDest.right - 2;
			break;
	}
	
	m_bClosingUp = TRUE;
	CEGDockingPane * pPane = m_pPane;
	m_pPane = NULL;

	::SetWindowPos( m_hWnd , NULL, m_rcDest.left, m_rcDest.top, m_rcDest.Width() - 1, m_rcDest.Height() - 1 , SWP_HIDEWINDOW);
	::AnimateWindow( m_hWnd , 200, AW_SLIDE | dwDirection );
	::SetWindowPos( m_hWnd , NULL, m_rcDest.left, m_rcDest.top, m_rcDest.Width(), m_rcDest.Height(), SWP_FRAMECHANGED | SWP_DRAWFRAME );
	
	m_bClosingUp = FALSE;
	m_pPane = pPane;
*/

	SetTimer(2, STEPTIME, NULL);	
}

void CEGFlyOutPane::SlideShow( WPARAM wParam, LPARAM lParam ) 
{
	m_rcDest = CRect( LOWORD( wParam ), HIWORD( wParam ), LOWORD( lParam ), HIWORD( lParam ) );
	m_rcNow = m_rcDest;

	m_bClosePressed = FALSE;
	m_bCloseHover = FALSE;

	m_bPinPressed = FALSE;
	m_bPinHover = FALSE;

	DWORD dwDirection = 0;
	switch( m_nStyle )
	{
		case ALIGN_TOP:
			dwDirection = AW_VER_POSITIVE;
			break;
		case ALIGN_BOTTOM:
			dwDirection = AW_VER_NEGATIVE;
			break;
		case ALIGN_LEFT:
			dwDirection = AW_HOR_POSITIVE;
			break;
		case ALIGN_RIGHT:
			dwDirection = AW_HOR_NEGATIVE;
			break;
	}
	::SetWindowPos( m_hWnd , NULL, m_rcDest.left, m_rcDest.top, m_rcDest.Width() - 1, m_rcDest.Height() - 1 , SWP_HIDEWINDOW);
	::AnimateWindow( m_hWnd , 200, AW_ACTIVATE | AW_SLIDE | dwDirection );
	::SetWindowPos( m_hWnd , NULL, m_rcDest.left, m_rcDest.top, m_rcDest.Width(), m_rcDest.Height(), SWP_FRAMECHANGED | SWP_DRAWFRAME );
}

LRESULT CEGFlyOutPane::ShowOff(WPARAM wParam, LPARAM lParam)
{
	SlideShow( wParam, lParam );
	::SetForegroundWindow( m_hWnd );
	::SetFocus( m_hWnd );

	return 0;
}

LRESULT CEGFlyOutPane::ShowOffNoFocus(WPARAM wParam, LPARAM lParam)
{
	SlideShow( wParam, lParam );

	return 0;
}
void CEGFlyOutPane::OnGetMinMaxInfo( LPMINMAXINFO lpMMI ) {
	CWnd::OnGetMinMaxInfo( lpMMI );

	lpMMI->ptMinTrackSize.x = SHOWOFFSTEP;	
	lpMMI->ptMinTrackSize.x = SHOWOFFSTEP;	
}

void CEGFlyOutPane::OnKillFocus( CWnd* pWnd ) {
	if ( !IsChild( pWnd ) )
		SlideHide( );
}

LRESULT CEGFlyOutPane::OnFocusChanged(WPARAM wParam, LPARAM lParam) 
{
	RePaint();
	
	return 0;
}