#include "stdafx.h"
#include "EGpanebar.h"

#define SPLITTER_SIZE 4

CEGPaneBar::CEGPaneBar(void)
{
	m_curHorzDrag = AfxGetApp()->LoadCursor(AFX_IDC_HSPLITBAR); // sometime fails .. 
	m_curVertDrag = AfxGetApp()->LoadCursor(AFX_IDC_VSPLITBAR); // sometime fails .. 
	m_hPane = NULL;
	m_bDragging = FALSE;
	m_pszCaption = NULL;

	m_fntCaption = NULL;
	HFONT hFont = (HFONT) GetStockObject( DEFAULT_GUI_FONT );
	if ( NULL != hFont ) {
		LOGFONT lf;
		if ( GetObject( hFont, sizeof( LOGFONT), &lf ) ) {
//			lf.lfWeight = FW_BOLD;
			m_fntCaption = ::CreateFontIndirect( &lf );
		}
	}
    
    CFont font;
	HDC hdc = ::GetDC( NULL );
	int ppi = ::GetDeviceCaps( hdc, LOGPIXELSX);
	::ReleaseDC ( NULL, hdc );
	int pointsize = MulDiv(  80 , 96, ppi); // 6 points at 96 ppi
    font.CreatePointFont( pointsize , _T("Marlett"));
	m_fntXButton = (HFONT) font.Detach();

	m_bClosePressed = FALSE;
	m_bCloseHover = FALSE;
	
	m_bActive = FALSE;

}

CEGPaneBar::~CEGPaneBar(void)
{
	if( NULL != m_curHorzDrag )
		DestroyCursor( m_curHorzDrag );
	if( NULL != m_curVertDrag )
		DestroyCursor( m_curVertDrag );
	if( NULL != m_pszCaption )
		free( m_pszCaption );
	if( NULL != m_fntCaption )
		DeleteObject( m_fntCaption );
	if( NULL != m_fntXButton )
		DeleteObject( m_fntXButton );
}

BEGIN_MESSAGE_MAP(CEGPaneBar, CControlBar)
	ON_WM_CREATE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_SIZE()
	ON_WM_PAINT()
	ON_WM_SETCURSOR()
	ON_WM_SETFOCUS()
END_MESSAGE_MAP()

BOOL CEGPaneBar::IsVertical() {
	DWORD dwStyle = GetBarStyle();
	return ( dwStyle & CBRS_TOP) == CBRS_TOP || ( dwStyle & CBRS_BOTTOM ) == CBRS_BOTTOM;
}

void CEGPaneBar::SetDragCursor() {
	
	if ( IsVertical() ) {
		SetCursor( m_curVertDrag);
	} else {
		SetCursor( m_curHorzDrag);
	}
}

void CEGPaneBar::SetPane( HWND hPane ){
	m_hPane = hPane;
	CRect rcClient;
	GetClientRect( &rcClient );
	OnSize( 0, rcClient.Width(), rcClient.Height() );
}

void CEGPaneBar::SetCaption( TCHAR* pszCaption ) {
	if ( m_pszCaption )
		free( m_pszCaption );
	m_pszCaption = pszCaption ? _tcsdup( pszCaption ) : NULL;
	Invalidate( TRUE );
}

BOOL CEGPaneBar::Create( TCHAR* pszCaption, CWnd * pParent, int nSize, int iId )
{
	if ( !CControlBar::Create(NULL, "", WS_VISIBLE|WS_CHILD, CRect(0,0,0,0), pParent, iId))
		return FALSE;

	SetCaption( pszCaption );
	SetOwner(pParent);
	m_iSize = nSize;

	return TRUE;
}

int CEGPaneBar::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CControlBar::OnCreate(lpCreateStruct) == -1) 
			return -1;
	SetAlign ( CBRS_BOTTOM );
	return 0;
}

void CEGPaneBar::SetAlign( DWORD dwAlign ) {
	SetBarStyle ( dwAlign | CBRS_HIDE_INPLACE & ~(CBRS_BORDER_ANY | CBRS_GRIPPER));
}

void CEGPaneBar::HidePane() {
	GetParentFrame()->ShowControlBar( this, FALSE, FALSE );
}

void CEGPaneBar::ShowPane() {
	GetParentFrame()->ShowControlBar( this, TRUE, FALSE );
}

void CEGPaneBar::ToggleVisible() {
	if ( IsVisible() ) {
		HidePane();
	} else {
		ShowPane();
	}
}

CRect CEGPaneBar::GetCaptionRect() {
	CRect rc;
	GetClientRect(rc);
	if ( m_pszCaption ) {
		rc.bottom = GetSystemMetrics( SM_CYCAPTION );
		DWORD dwStyle = GetBarStyle();
		if ( (dwStyle & CBRS_LEFT ) == CBRS_LEFT ) {
			rc.right -= SPLITTER_SIZE;
		} else if ( (dwStyle & CBRS_RIGHT ) == CBRS_RIGHT ) {
			rc.left += SPLITTER_SIZE;
		} else if ( (dwStyle & CBRS_TOP ) == CBRS_TOP ) {
			//rc.top -= SPLITTER_SIZE;
		} else {
			rc.bottom += SPLITTER_SIZE;
			rc.top += SPLITTER_SIZE;
		}
	} else {
		rc.SetRect( 0, 0, 0, 0 );
	}
	return rc;
}

CRect CEGPaneBar::GetCloseButtonRect() {
	CRect rc = GetCaptionRect();
	rc.left = rc.right - (rc.bottom - rc.top);
	::InflateRect( &rc, -2, -2 );
	return rc;
}

void CEGPaneBar::OnPaint()
{
	CPaintDC pdc(this);

	CRect rc;
	GetClientRect(rc);
	pdc.FillSolidRect(rc, GetSysColor(COLOR_3DFACE));
	if ( m_pszCaption ) {
		rc = GetCaptionRect();
		HFONT hOldFont = (HFONT) ::SelectObject( pdc.GetSafeHdc(), m_fntCaption );
		pdc.SetBkMode( TRANSPARENT );
		if ( m_bActive ) {
			pdc.FillSolidRect(rc, GetSysColor( COLOR_ACTIVECAPTION ));
			pdc.SetTextColor( GetSysColor( COLOR_CAPTIONTEXT ) );
		} else {
			CPen *pOldPen, pen(PS_SOLID,1,GetSysColor(COLOR_INACTIVECAPTION));
			pOldPen = pdc.SelectObject(&pen);

			pdc.MoveTo( rc.left + 1, rc.top );
			pdc.LineTo( rc.right - 1, rc.top );
			pdc.MoveTo( rc.right - 1, rc.top+1 );
			pdc.LineTo( rc.right - 1, rc.bottom - 1 );
			pdc.MoveTo( rc.right - 2, rc.bottom - 1 );
			pdc.LineTo( rc.left, rc.bottom - 1 );
			pdc.MoveTo( rc.left, rc.bottom - 2 );
			pdc.LineTo( rc.left, rc.top );

			pdc.SelectObject(pOldPen);

			pdc.SetTextColor( GetSysColor( COLOR_WINDOWTEXT ) );
		}
		rc.left += SPLITTER_SIZE;
		pdc.DrawText( m_pszCaption, &rc, DT_LEFT | DT_SINGLELINE | DT_VCENTER );

		// X-button ( Marlett :: 0x72 )
		rc = GetCloseButtonRect();
		::SelectObject( pdc.GetSafeHdc(), m_fntXButton );
	    pdc.DrawText( _T("\x72"), &rc, DT_CENTER | DT_SINGLELINE | DT_VCENTER );
		
		if ( !m_bClosePressed && m_bCloseHover ) {
			pdc.DrawEdge( rc, BDR_RAISEDINNER , BF_RECT | BF_ADJUST  );
		} else if( m_bClosePressed && m_bCloseHover ) {
			pdc.DrawEdge( rc, BDR_SUNKENINNER , BF_RECT | BF_ADJUST  );
		} else if( m_bClosePressed && !m_bCloseHover ) { 
			pdc.DrawEdge( rc, BDR_RAISEDINNER , BF_RECT | BF_ADJUST  );
		}

		::SelectObject( pdc.GetSafeHdc(), hOldFont );
	}
}

CSize CEGPaneBar::CalcFixedLayout (BOOL /*bStretch*/, BOOL bHorz )
{
	return CSize ( bHorz ? 32767 : m_iSize, bHorz ? m_iSize: 32767);
}

void CEGPaneBar::OnLButtonDown(UINT nFlags, CPoint point)
{
	
	if ( m_iSize <= 4 || m_dragRect.PtInRect(point) )
	{
		m_bDragging = TRUE;
		SetDragCursor();
		SetCapture();
		SetFocus();
		CRect rc;
		GetClientRect(rc);
		OnInvertTracker(m_dragRect);
		return;
	} else {
		if( m_hPane ) {
			if( m_hPane != ::SetFocus( m_hPane ) )
				InvalidateCaption();
		}
		CRect rcCloseButton = GetCloseButtonRect();
		if ( rcCloseButton.PtInRect( point ) ) {
			SetCapture();
			m_bClosePressed = TRUE;
			InvalidateCaption();
		}
	}
	CControlBar::OnLButtonDown(nFlags, point);
}

void CEGPaneBar::OnLButtonUp(UINT nFlags, CPoint point)
{
	if ( m_bDragging )
	{
		ReleaseCapture();
		OnInvertTracker(m_dragRect);
		DWORD dwStyle = GetBarStyle();

		CRect rcCaption = GetCaptionRect();
		int nMinHeight = rcCaption.Height() + SPLITTER_SIZE*2;
		int nMinWidth = nMinHeight*4;

		if( ( dwStyle & CBRS_TOP ) == CBRS_TOP ) {
			m_iSize = point.y > nMinHeight ? point.y : nMinHeight;
		} else if( ( dwStyle & CBRS_BOTTOM ) == CBRS_BOTTOM ) {
			m_iSize =  m_iSize - point.y;
			if ( m_iSize < nMinHeight )
				m_iSize = nMinHeight;
		} else if( ( dwStyle & CBRS_RIGHT ) == CBRS_RIGHT ) {
			m_iSize = m_iSize - point.x;
			if ( m_iSize < nMinWidth )
				m_iSize = nMinWidth;
		} else {
			m_iSize = point.x > nMinWidth ? point.x : nMinWidth;
		}
		GetParentFrame()->RecalcLayout();
	}
	m_bDragging = FALSE;
	BOOL bOldClosePressed = m_bClosePressed;
	m_bClosePressed = FALSE;
	if ( bOldClosePressed ) {
		ReleaseCapture();
		CRect rcCloseButton = GetCloseButtonRect();
		if ( rcCloseButton.PtInRect( point ) ) {
			HidePane( );
		} else {
			InvalidateCaption();
		}
	}

	CControlBar::OnLButtonUp(nFlags, point);
}

void CEGPaneBar::InvalidateCaption() {
	CRect rc = GetCaptionRect();
	InvalidateRect( &rc, FALSE );
}

void CEGPaneBar::OnMouseMove(UINT nFlags, CPoint point)
{
	if ( m_bDragging ) {
		CRect rc1( m_dragRect );
		
		if ( IsVertical() ) {
			m_dragRect.SetRect( rc1.left, point.y - 5, rc1.right, point.y );
		} else {
			m_dragRect.SetRect( point.x - 5, rc1.top, point.x, rc1.bottom);
		}
		if (rc1 != m_dragRect)
		{
			OnInvertTracker(rc1);
			OnInvertTracker(m_dragRect);
		}
	} else {
		CRect rc = GetCloseButtonRect();
		if ( rc.PtInRect( point ) ) {
			if( !m_bCloseHover ) {
				m_bCloseHover = TRUE;
				InvalidateCaption(); 
				return;
			}
		} else {
			if ( m_bCloseHover ) {
				m_bCloseHover = FALSE;
				InvalidateCaption(); 
				return;
			}
		}


	}
	CControlBar::OnMouseMove(nFlags, point);
}

BOOL CEGPaneBar::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message)
{
	if (nHitTest == HTCLIENT) {
		CPoint point;
		::GetCursorPos (&point);
		ScreenToClient (&point);
		if ( m_bDragging || m_dragRect.PtInRect(point) ) {
			SetDragCursor();
			return TRUE;
		}
	}

	return CControlBar::OnSetCursor(pWnd, nHitTest, message);
}

void CEGPaneBar::OnInvertTracker(const CRect& rc)
{
	CFrameWnd* pParentFrame = GetParentFrame ();
	CDC* pDC = pParentFrame->GetDC();
	CRect rect(rc);
    ClientToScreen(rect);
	pParentFrame->ScreenToClient(rect);

	CBrush br;
//	br.CreateSolidBrush(GetSysColor(COLOR_HIGHLIGHT));
//	br.CreateSolidBrush( GetSysColor( COLOR_APPWORKSPACE ) );
	br.CreateHatchBrush( HS_DIAGCROSS, GetSysColor( COLOR_APPWORKSPACE ) );

	HBRUSH hOldBrush = NULL;
	hOldBrush = (HBRUSH)SelectObject(pDC->m_hDC, br.m_hObject);
	pDC->PatBlt( rect.left, rect.top, rect.Width(), rect.Height(), DSTINVERT );
	if (hOldBrush != NULL) SelectObject(pDC->m_hDC, hOldBrush);
	ReleaseDC(pDC);

}

void CEGPaneBar::OnSize(UINT nType, int cx, int cy)
{
	CControlBar::OnSize(nType, cx, cy);

	CRect rc;
	DWORD dwStyle = GetBarStyle();
	if ( (dwStyle & CBRS_LEFT ) == CBRS_LEFT ) {
		m_dragRect.SetRect( cx - SPLITTER_SIZE, 0, cx, cy);
		rc.SetRect( 0, 0, cx - SPLITTER_SIZE, cy ) ;
	} else if ( (dwStyle & CBRS_RIGHT ) == CBRS_RIGHT ) {
		m_dragRect.SetRect( 0, 0, SPLITTER_SIZE, cy);
		rc.SetRect( SPLITTER_SIZE, 0, cx, cy ) ;
	} else if ( (dwStyle & CBRS_TOP ) == CBRS_TOP ) {
		m_dragRect.SetRect( 0, cy - SPLITTER_SIZE, cx, cy);
		rc.SetRect( 0, 0, cx, cy - SPLITTER_SIZE ) ;
	} else {
		m_dragRect.SetRect( 0, 0, cx, SPLITTER_SIZE);
		rc.SetRect( 0, SPLITTER_SIZE, cx, cy) ;
	}
	
	if ( NULL != m_pszCaption )
		rc.top += GetSystemMetrics( SM_CYCAPTION ) + SPLITTER_SIZE;

	if ( NULL != m_hPane )
		::MoveWindow( m_hPane, rc.left, rc.top, rc.Width(), rc.Height(), TRUE); 
}

void CEGPaneBar::OnUpdateCmdUI(CFrameWnd* /* pTarget */, BOOL /* bDisableIfNoHndler */) {

	if ( m_pszCaption ) {
		HWND hActiveWnd = ::GetFocus();
		BOOL bActiveOld = m_bActive;
		
		m_bActive = ( m_hWnd == hActiveWnd || m_hPane == hActiveWnd );

		if ( m_bActive != bActiveOld )
			InvalidateCaption();
	}
}

void CEGPaneBar::OnSetFocus( CWnd * /* pOldWnd */) {
	if ( m_hPane )
		::SetFocus( m_hPane );
}
