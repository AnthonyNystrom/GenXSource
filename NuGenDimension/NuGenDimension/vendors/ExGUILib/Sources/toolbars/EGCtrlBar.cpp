#include "stdafx.h"
#include "EGctrlbar.h"

CEGControlBar::CEGControlBar(void)
{
	m_curHorzDrag = AfxGetApp()->LoadCursor(AFX_IDC_HSPLITBAR); // sometime fails .. 
	m_curVertDrag = AfxGetApp()->LoadCursor(AFX_IDC_VSPLITBAR); // sometime fails .. 
	m_bDragging = FALSE;
}

CEGControlBar::~CEGControlBar(void)
{
	if( NULL != m_curHorzDrag )
		DestroyCursor( m_curHorzDrag );
	if( NULL != m_curVertDrag )
		DestroyCursor( m_curVertDrag );
}

BEGIN_MESSAGE_MAP(CEGControlBar, CControlBar)
	ON_WM_CREATE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_SIZE()
	ON_WM_PAINT()
	ON_WM_SETCURSOR()
	ON_WM_SETFOCUS()
END_MESSAGE_MAP()

BOOL CEGControlBar::IsVertical() {
	DWORD dwStyle = GetBarStyle();
	return ( dwStyle & CBRS_TOP) == CBRS_TOP || ( dwStyle & CBRS_BOTTOM ) == CBRS_BOTTOM;
}

void CEGControlBar::SetDragCursor() {
	
	if ( IsVertical() ) {
		SetCursor( m_curVertDrag);
	} else {
		SetCursor( m_curHorzDrag);
	}
}


void CEGControlBar::GetInsideRect( CRect& rc ) {

	GetClientRect(rc);
	DWORD dwStyle = GetBarStyle();
	if ( (dwStyle & CBRS_LEFT ) == CBRS_LEFT ) {
		rc.right -= SPLITTER_SIZE;
	} else if ( (dwStyle & CBRS_RIGHT ) == CBRS_RIGHT ) {
		rc.left += SPLITTER_SIZE;
	} else if ( (dwStyle & CBRS_TOP ) == CBRS_TOP ) {
		rc.bottom -= SPLITTER_SIZE;
	} else {
		rc.top += SPLITTER_SIZE;
	}
}

BOOL CEGControlBar::Create( CWnd * pParent, int nSize, int iId )
{
	if ( !CControlBar::Create(NULL, "", WS_VISIBLE|WS_CHILD, CRect(0,0,0,0), pParent, iId))
		return FALSE;

	SetOwner(pParent);
	m_iSize = nSize;

	return TRUE;
}

int CEGControlBar::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CControlBar::OnCreate(lpCreateStruct) == -1) 
			return -1;
	SetAlign ( CBRS_BOTTOM );
	return 0;
}

void CEGControlBar::SetAlign( DWORD dwAlign ) {
	SetBarStyle ( dwAlign | CBRS_HIDE_INPLACE & ~(CBRS_BORDER_ANY | CBRS_GRIPPER));
}

void CEGControlBar::HidePane() {
	GetParentFrame()->ShowControlBar( this, FALSE, FALSE );
}

void CEGControlBar::ShowPane() {
	GetParentFrame()->ShowControlBar( this, TRUE, FALSE );
}

void CEGControlBar::ToggleVisible() {
	if ( IsVisible() ) {
		HidePane();
	} else {
		ShowPane();
	}
}

void CEGControlBar::OnPaint()
{
	CPaintDC dc(this);

	CRect rc;
	dc.GetClipBox( &rc );
	dc.FillSolidRect(rc, GetSysColor(COLOR_3DFACE));

	GetInsideRect(rc);
	OnDraw( &dc, rc );
}

void CEGControlBar::OnDraw( CDC * /* pDC */, CRect& /* rc */ ) {

}

CSize CEGControlBar::CalcFixedLayout (BOOL /*bStretch*/, BOOL bHorz )
{
	return CSize ( bHorz ? 32767 : m_iSize, bHorz ? m_iSize: 32767);
}

void CEGControlBar::OnLButtonDown(UINT nFlags, CPoint point)
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
	}
	CControlBar::OnLButtonDown(nFlags, point);
}

void CEGControlBar::OnLButtonUp(UINT nFlags, CPoint point)
{
	if ( m_bDragging )
	{
		ReleaseCapture();
		OnInvertTracker(m_dragRect);
		DWORD dwStyle = GetBarStyle();

		int nMinHeight = GetSystemMetrics( SM_CYCAPTION ) + SPLITTER_SIZE*2;
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
	CControlBar::OnLButtonUp(nFlags, point);
}

void CEGControlBar::OnMouseMove(UINT nFlags, CPoint point)
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
	}
	CControlBar::OnMouseMove(nFlags, point);
}

BOOL CEGControlBar::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message)
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

void CEGControlBar::OnInvertTracker(const CRect& rc)
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

void CEGControlBar::OnResize( CRect& /* rc */ ) {

}


void CEGControlBar::OnSize(UINT nType, int cx, int cy)
{
	CControlBar::OnSize(nType, cx, cy);

	DWORD dwStyle = GetBarStyle();
	if ( (dwStyle & CBRS_LEFT ) == CBRS_LEFT ) {
		m_dragRect.SetRect( cx - SPLITTER_SIZE, 0, cx, cy);
	} else if ( (dwStyle & CBRS_RIGHT ) == CBRS_RIGHT ) {
		m_dragRect.SetRect( 0, 0, SPLITTER_SIZE, cy);
	} else if ( (dwStyle & CBRS_TOP ) == CBRS_TOP ) {
		m_dragRect.SetRect( 0, cy - SPLITTER_SIZE, cx, cy);
	} else {
		m_dragRect.SetRect( 0, 0, cx, SPLITTER_SIZE);
	}
	
	CRect rc;
	GetInsideRect( rc );
	OnResize ( rc );
}

