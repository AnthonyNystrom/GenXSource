#include "StdAfx.h"

#include "EGMDIClient.h"
#include "NewTheme.h"
#include "EGMenu.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define TABS_HEIGHT 24
#define TABS_BKGND RGB(247,243,233)
#define PADDING 3
#define WM_RECALC_BUTTONS WM_USER + 1

#include <algorithm>
#include <functional>

//////////////////////////////////////////////////////////////////////////// 
// Registry keys used for saving/restoring the state
static TCHAR szSection[]      = _T( "MDIClient" );
static TCHAR szDisplayMode[]  = _T( "DisplayMode" );
static TCHAR szBkColor[]      = _T( "BkColor" );
static TCHAR szFileName[]     = _T( "FileName" );
static TCHAR szOriginX[]      = _T( "OriginX" );
static TCHAR szOriginY[]      = _T( "OriginY" );

/////////////////////////////////////////////////////////////////////////////
// CEGMDITabBtn

void CEGMDITabBtn::Copy( const CEGMDITabBtn & tab ) {
	m_hWnd = tab.m_hWnd;
	m_nWidth = tab.m_nWidth;
	m_nLeft = tab.m_nLeft;
	m_clrColor = tab.m_clrColor;
}

CEGMDITabBtn::CEGMDITabBtn( HWND hWnd ) {
	m_hWnd = hWnd;
	m_nWidth = 0;
	m_clrColor = themeData.GetNewColor();
}

CEGMDITabBtn::CEGMDITabBtn( const CEGMDITabBtn & tab ) {
	Copy( tab );
}

CEGMDITabBtn & CEGMDITabBtn::operator=( const CEGMDITabBtn & tab ) {
	Copy( tab );
	return *this;
}

CString CEGMDITabBtn::GetTitle() { 
	CString szTitle;
	GetWindowText( m_hWnd, szTitle.GetBuffer(100), 100 );
	szTitle.ReleaseBuffer();
	return szTitle; 
}

/////////////////////////////////////////////////////////////////////////////
// CEGMDIClient

static int CALLBACK EnumFontFamProc(ENUMLOGFONT FAR *lpelf,
                             NEWTEXTMETRIC FAR *lpntm,
                             int FontType,
                             LPARAM lParam)
{
    UNUSED_ALWAYS(lpelf);
    UNUSED_ALWAYS(lpntm);
    UNUSED_ALWAYS(FontType);
    UNUSED_ALWAYS(lParam);

    return 0;
}

CEGMDIClient::CEGMDIClient()
{   
	Reset();
	m_bAutoSaveRestore = FALSE;
	m_bShowTabs = FALSE;

  m_cxEdge = 6;
	m_cxControls = 45;
	m_nWidth = 0;

	m_bPriorEnabled = FALSE;
	m_bPriorHover = FALSE;
	m_bPriorPressed = FALSE;

	m_bNextEnabled = FALSE;
	m_bNextHover = FALSE;
	m_bNextPressed = FALSE;
		
	m_bCloseEnabled = TRUE;
	m_bCloseHover = FALSE;
	m_bClosePressed = FALSE;
	
	m_cxOffset = 0;
	m_cxBtnsAvailable = 0;
	m_cxBtnsTotal = 0;

	m_bTabDrag = FALSE;
}

CEGMDIClient::~CEGMDIClient()
{
}

void CEGMDIClient::PreSubclassWindow()
{
	CWnd::PreSubclassWindow();
	if ( m_bAutoSaveRestore == TRUE )
		RestoreState();

	m_oldWndProc = (WNDPROC)::GetWindowLongPtr( m_hWnd, GWLP_WNDPROC ); 
}

void CEGMDIClient::Reset()
{
	m_ptOrigin.x = m_ptOrigin.y = 0;
	m_sizeImage.cx = m_sizeImage.cy = 0;
	m_eDisplayMode = dispTile;
	m_strFileName.Empty();      
	SetBkColor( themeData.clrAppWorkSpace );
	m_bitmap.DeleteObject();
	if ( IsWindow( m_hWnd ) )
		Invalidate();
}

BOOL CEGMDIClient::SetBitmap( LPCTSTR lpszFileName, UINT uFlags )
{
	HANDLE handle = ::LoadImage( AfxGetInstanceHandle(),
		lpszFileName,
		IMAGE_BITMAP,
		0, 0,
		uFlags | LR_LOADFROMFILE );

	if ( !handle )    // There were some problems during loading the image
		return FALSE;

	m_bitmap.DeleteObject();
	m_bitmap.Attach( (HBITMAP)handle );
	if ( IsWindow( m_hWnd ) )
		Invalidate();
	m_strFileName = lpszFileName;

	BITMAP bi;
	m_bitmap.GetBitmap( &bi );
	m_sizeImage.cx = bi.bmWidth;
	m_sizeImage.cy = bi.bmHeight;

	return TRUE;
}

BOOL CEGMDIClient::SetBitmap( UINT nBitmapID, COLORMAP* pClrMap, int nCount )
{
	m_bitmap.DeleteObject();
	if ( pClrMap == NULL )  // Load normal
	{
		if ( m_bitmap.LoadBitmap( nBitmapID ) == FALSE )
			return FALSE;
	}
	else                    // Load mapped 
	{      
		if ( m_bitmap.LoadMappedBitmap( nBitmapID, 0, pClrMap, nCount ) == FALSE )
			return FALSE;
	}

	BITMAP bi;
	m_bitmap.GetBitmap( &bi );
	m_sizeImage.cx = bi.bmWidth;
	m_sizeImage.cy = bi.bmHeight;

	if ( IsWindow( m_hWnd ) )
		Invalidate();
	return TRUE;
}

void CEGMDIClient::SetBkColor( COLORREF clrValue )
{
	m_clrBackground = clrValue;
	m_brush.DeleteObject();
	m_brush.CreateSolidBrush( m_clrBackground );
	if ( IsWindow( m_hWnd ) )
		Invalidate();
}

void CEGMDIClient::SetDisplayMode( CEGMDIClient::DisplayModesEnum eDisplayMode )
{
	m_eDisplayMode = eDisplayMode;      
	if ( IsWindow( m_hWnd ) )
		Invalidate();
}

void CEGMDIClient::SaveState()
{
	AfxGetApp()->WriteProfileInt( szSection, szDisplayMode, GetDisplayMode() );
	AfxGetApp()->WriteProfileInt( szSection, szBkColor, GetBkColor() );
	AfxGetApp()->WriteProfileString( szSection, szFileName, GetFileName() );
	AfxGetApp()->WriteProfileInt( szSection, szOriginX, GetOrigin().x );
	AfxGetApp()->WriteProfileInt( szSection, szOriginY, GetOrigin().y );   
}

void CEGMDIClient::RestoreState()
{   
	m_eDisplayMode = (DisplayModesEnum)AfxGetApp()->GetProfileInt( szSection, szDisplayMode, dispTile );      
	m_ptOrigin.x = AfxGetApp()->GetProfileInt( szSection, szOriginX, 0 );
	m_ptOrigin.y = AfxGetApp()->GetProfileInt( szSection, szOriginY, 0 );
	SetBkColor( AfxGetApp()->GetProfileInt( szSection, szBkColor, themeData.clrAppWorkSpace ) );
	CString str = AfxGetApp()->GetProfileString( szSection, szFileName, _T( "" ) );
	if ( str.GetLength() )
		SetBitmap( str );
}

BEGIN_MESSAGE_MAP(CEGMDIClient, CWnd)
   ON_WM_DESTROY()
   ON_WM_PAINT()
   ON_WM_SIZE()
   ON_WM_ERASEBKGND()
	 ON_MESSAGE( WM_MDICREATE, OnMDICreate )
	 ON_MESSAGE( WM_MDIDESTROY, OnMDIDestroy )
	 ON_MESSAGE( WM_MDINEXT, OnMDINext )
	 ON_MESSAGE( WM_MDIACTIVATE, OnMDIActivate )
	 ON_MESSAGE( WM_RECALC_BUTTONS, OnRecalcButtons )

	 ON_WM_NCLBUTTONDOWN()
	 ON_WM_NCLBUTTONUP()
	 ON_WM_LBUTTONUP()
	 ON_WM_NCMOUSEMOVE()
	 ON_WM_CANCELMODE()
	 ON_WM_MOUSEMOVE()

	 ON_WM_STYLECHANGED( )
	 ON_WM_NCCALCSIZE()
	 ON_WM_NCPAINT( )
	 ON_WM_NCHITTEST()
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CEGMDIClient message handlers

void CEGMDIClient::OnPaint() 
{
	CPaintDC dc( this );
	CRect rc;
	GetClientRect( rc );

	if ( !(HBITMAP)m_bitmap )   // If no bitmap is selected
		dc.FillRect( rc, &m_brush );

	if ( (HBITMAP)m_bitmap )
	{
		CDC* pDC;
		CDC memDC;
		CBitmap bmp; 

		BOOL bDrawOnMemDC = ( GetDisplayMode() != dispTile ) ? TRUE : FALSE;
		if ( bDrawOnMemDC )
		{
			if ( GetDisplayMode() != dispStretch )
				bmp.CreateCompatibleBitmap( &dc, rc.right, rc.bottom );
			else
				bmp.CreateCompatibleBitmap( &dc, m_sizeImage.cx, m_sizeImage.cy );
			memDC.CreateCompatibleDC( &dc );
			memDC.SelectObject( &bmp );
			pDC = &memDC;
		}
		else
			pDC = &dc;      

		switch ( GetDisplayMode() )
		{
		case dispCustom:
			{
				pDC->FillRect( rc, &m_brush );               
				pDC->DrawState( m_ptOrigin, m_sizeImage, &m_bitmap, DST_BITMAP | DSS_NORMAL );
			}
			break;

		case dispCenter:
			{
				pDC->FillRect( rc, &m_brush );
				CPoint point( ( rc.Width() - m_sizeImage.cx ) / 2, 
					( rc.Height() - m_sizeImage.cy ) / 2 );
				pDC->DrawState( point, m_sizeImage, &m_bitmap, DST_BITMAP | DSS_NORMAL );
			}
			break;

		case dispTile:
			{
				CPoint point;
				for ( point.y = 0; point.y < rc.Height(); point.y += m_sizeImage.cy )
					for ( point.x = 0; point.x < rc.Width(); point.x += m_sizeImage.cx )
						pDC->DrawState( point, m_sizeImage, &m_bitmap, DST_BITMAP | DSS_NORMAL );
			}
			break;

		case dispStretch:
			{
				memDC.DrawState( CPoint(0,0), m_sizeImage, &m_bitmap, DST_BITMAP | DSS_NORMAL );
				dc.SetStretchBltMode( COLORONCOLOR );
				dc.StretchBlt( 0, 0, rc.right, rc.bottom, &memDC, 
					0, 0, m_sizeImage.cx, m_sizeImage.cy, SRCCOPY );
			}
			return;
		}

		if ( bDrawOnMemDC == TRUE )
			dc.BitBlt( 0, 0, rc.right, rc.bottom, pDC, 0, 0, SRCCOPY );
	}
}

BOOL CEGMDIClient::OnEraseBkgnd( CDC* /*pDC*/ ) 
{   
   return TRUE;    // Return TRUE so background is not erased   
}

void CEGMDIClient::OnSize( UINT nType, int cx, int cy )
{
	if ( nType != SIZE_MINIMIZED && GetDisplayMode() != dispTile )   // Force repainting if center mode is selected
		Invalidate();
	CWnd::OnSize( nType, cx, cy );
}

void CEGMDIClient::OnDestroy()
{   
	if ( m_bAutoSaveRestore == TRUE )
		SaveState();
	CWnd::OnDestroy();
}

LRESULT CEGMDIClient::OnMDICreate ( WPARAM wParam, LPARAM lParam ) {
	
	LRESULT res = m_oldWndProc( m_hWnd, WM_MDICREATE, wParam, lParam );

	if( res ) {
		m_lstTabBtns.push_back( CEGMDITabBtn( (HWND)res ) );
		m_hWndActive = (HWND)res;
		PostMessage( WM_RECALC_BUTTONS, 2 );
	}

	return res;
}

LRESULT CEGMDIClient::OnMDIDestroy ( WPARAM wParam, LPARAM lParam ) {
	LRESULT res = m_oldWndProc( m_hWnd, WM_MDIDESTROY, wParam, lParam );

	CEGMDITabBtnsIt itToDestroy = find_if( m_lstTabBtns.begin(), m_lstTabBtns.end(), MatchWndBtn( (HWND)wParam ) );

	if ( itToDestroy != m_lstTabBtns.end() )
		m_lstTabBtns.erase( itToDestroy );

	if ( m_bShowTabs ) {
		if ( NULL == ::GetWindow( m_hWnd, GW_CHILD ) ) {
			m_bShowTabs = FALSE;
			PostMessage( WM_RECALC_BUTTONS, 1 );
		} else {
			m_hWndActive = (HWND)::SendMessage( m_hWnd, WM_MDIGETACTIVE, 0, 0 );
			PostMessage( WM_RECALC_BUTTONS );
		}
	}

	return res;
}

LRESULT CEGMDIClient::OnMDINext ( WPARAM wParam, LPARAM lParam ) {
	//LRESULT res = m_oldWndProc( m_hWnd, WM_MDINEXT, wParam, lParam );
	if ( m_lstTabBtns.size() == 1 )
		return 0L;

	CEGMDITabBtnsIt itActive = find_if( m_lstTabBtns.begin(), m_lstTabBtns.end(), MatchWndBtn( (HWND)wParam ) );
	if ( itActive == m_lstTabBtns.end() )
		return 0L;

	if( lParam == 0 ) {
		// forward
		if( itActive->m_hWnd != m_lstTabBtns.back().m_hWnd ) {
			++itActive;
		} else {
			itActive = m_lstTabBtns.begin();
		}
	} else {
		// backward
		if( itActive->m_hWnd != m_lstTabBtns.front().m_hWnd ) {
			--itActive;
		} else {
			itActive = --m_lstTabBtns.end();
		}
	}

	if ( itActive->m_nLeft < 0 ) {
		m_cxOffset -= -itActive->m_nLeft;
	} else if ( m_cxBtnsAvailable < ( itActive->m_nLeft + itActive->m_nWidth ) ) {
		m_cxOffset += ( itActive->m_nLeft + itActive->m_nWidth ) - m_cxBtnsAvailable;
	}

	ActivateMDI( itActive->m_hWnd );

	m_hWndActive = itActive->m_hWnd;

	return 0L;
}

LRESULT CEGMDIClient::OnMDIActivate ( WPARAM wParam, LPARAM lParam ) {
	LRESULT res = m_oldWndProc( m_hWnd, WM_MDIACTIVATE, wParam, lParam );

	m_hWndActive = (HWND)wParam;

	OnRecalcButtons( 0, 0 );

	return res;
}

void CEGMDIClient::OnNcCalcSize(
   BOOL bCalcValidRects,
   NCCALCSIZE_PARAMS* lpncsp ) 
{
	CWnd::OnNcCalcSize( bCalcValidRects, lpncsp );
	if ( m_bShowTabs ) {
		lpncsp[0].rgrc->top += TABS_HEIGHT;
		lpncsp[0].rgrc->right -= 2;
		lpncsp[0].rgrc->left += 2;
		lpncsp[0].rgrc->bottom -= 2;

		CRect rc (*lpncsp[0].rgrc);
		rc.OffsetRect( -rc.TopLeft() );
		CalcButtons( &rc );
	}
}

void CEGMDIClient::OnStyleChanged( int nStyleType, LPSTYLESTRUCT lpStyleStruct ) {
	m_hWndActive = (HWND)::SendMessage( m_hWnd, WM_MDIGETACTIVE, 0, (LPARAM) &m_bShowTabs );
	m_bShowTabs = !m_bShowTabs;
}

void CEGMDIClient::OnNcPaint() {

	CWnd::OnNcPaint();

	if ( m_bShowTabs ) {
		// get window DC that is clipped to the non-client area
		CWindowDC dc(this);
		CRect rectClient;
		GetWindowRect(rectClient);
		rectClient.OffsetRect( -rectClient.TopLeft() );

		// Draw border
		BOOL bOldStyle = CEGMenu::GetMenuDrawMode() == CEGMenu::STYLE_ORIGINAL ||
			CEGMenu::GetMenuDrawMode() == CEGMenu::STYLE_ORIGINAL_NOBORDER;

//		if ( !bOldStyle ) {
		CPen pnBorder( PS_SOLID, 1, bOldStyle ? themeData.clrBtnFace : themeData.clr3DShadow );
		CPen * pOldPen = dc.SelectObject( &pnBorder );

		dc.MoveTo( 0 , 0 );
		dc.LineTo( rectClient.right - 1, 0  );
		dc.LineTo( rectClient.right - 1, rectClient.bottom - 1 );
		dc.LineTo( 0, rectClient.bottom - 1 );
		dc.LineTo( 0, 0 );

		CPen pnBorder2( PS_SOLID, 1, themeData.clrBtnFace );
		dc.SelectObject( &pnBorder2 );
		dc.MoveTo( rectClient.right - 2, 1  );
		dc.LineTo( rectClient.right - 2, rectClient.bottom - 2 );
		dc.LineTo( 1, rectClient.bottom - 2 );
		dc.LineTo( 1, 0 );

		dc.SelectObject( pOldPen );

		CRect rcTabs = rectClient;
		rcTabs.top = 1;
		rcTabs.bottom = rcTabs.top + TABS_HEIGHT - 1;

		CSurface surface;

		// tabs
		rcTabs.left = 2;
		rcTabs.right = rectClient.right - m_cxControls - m_cxEdge - 2;

		DrawTabs( surface.Lock( &dc, &rcTabs ), surface.GetBounds() );
		surface.Release();

		// controls
		rcTabs.left = rectClient.right - m_cxControls - m_cxEdge - 2;
		rcTabs.right = rectClient.right - 2;

		DrawControls( surface.Lock( &dc, &rcTabs ), surface.GetBounds() );
		surface.Release();
	}
}

void CEGMDIClient::DrawTabs( CDC * pDC, LPRECT lprcBounds ) {

	ASSERT( NULL != lprcBounds );

	if( m_lstTabBtns.empty() )
		return;

	CEGMDITabBtnsIt it, itFirst = m_lstTabBtns.begin(),
		itLast = m_lstTabBtns.end();

	COLORREF clrActive = themeData.clrBtnFace;
	for( it = itFirst; it != itLast; ++it ) {
		if ( it->m_hWnd == m_hWndActive ) {
			clrActive = it->m_clrColor;
			break;
		}
	}
	themeData.DrawTabCtrlBK( pDC, lprcBounds, ALIGN_TOP, TRUE, clrActive );


	// Draw buttons  
	CRect rcButton( *lprcBounds );
	rcButton.top+=2;

	int nOldBkMode = pDC->SetBkMode(TRANSPARENT);

	//	CFont* pOldFont = pDC->SelectObject( &m_fntThin );

	for( it = itFirst; it != itLast; ++it ) {

		CEGMDITabBtn btn = (*it);

		BOOL bSelected = ( btn.m_hWnd == m_hWndActive );

		int nWidth = btn.m_nWidth;
		rcButton.left = btn.m_nLeft;
		rcButton.right = rcButton.left + nWidth;

		themeData.DrawTab( pDC, &rcButton, NULL, (TCHAR*)(LPCTSTR)btn.GetTitle(), ALIGN_TOP, STYLE_COOL | ( bSelected ? STYLE_ACTIVE : 0 ), btn.m_clrColor );
	}

	// pDC->SelectObject( pOldFont );	
}

void CEGMDIClient::DrawControls( CDC * pDC, LPRECT lprcBounds ) {

	ASSERT( NULL != lprcBounds );

	CEGMDITabBtnsIt it, itFirst = m_lstTabBtns.begin(),
		itLast = m_lstTabBtns.end();

	COLORREF clrActive = themeData.clrBtnFace;
	for( it = itFirst; it != itLast; ++it ) {
		if ( it->m_hWnd == m_hWndActive ) {
			clrActive = it->m_clrColor;
			break;
		}
	}
	themeData.DrawTabCtrlBK( pDC, lprcBounds, ALIGN_TOP, TRUE, clrActive );

	CRect rc( *lprcBounds );

	CPen pnCross( PS_SOLID, 1, themeData.clr3DShadow );
	CPen * pOldPen = pDC->SelectObject( &pnCross );

	// Draw cross
	rc = m_rcClose;
	rc.OffsetRect( -( m_nWidth - ( lprcBounds->right - lprcBounds->left ) ), 0 );
	themeData.DrawX( pDC, &rc ); 
	rc.OffsetRect( -1, 0 );
	themeData.DrawX( pDC, &rc ); 

	if ( m_bCloseHover && m_bClosePressed ) {
		themeData.DrawPressedRect( pDC, &rc );
	} else if ( m_bCloseHover && !m_bClosePressed ) {
		themeData.DrawHoverRect( pDC, &rc );
	}

	// Draw arrows
	CBrush brEnabled, brDisabled, * pbrOld;
	brEnabled.CreateSolidBrush( themeData.clr3DShadow );
	brDisabled.CreateStockObject( NULL_BRUSH );

	rc = m_rcNext;
	rc.OffsetRect( -( m_nWidth - ( lprcBounds->right - lprcBounds->left ) ), 0 );

	POINT pts[3];

	pts[0].x = rc.left; pts[0].y = rc.top;
	pts[1].x = rc.left; pts[1].y = rc.bottom;
	pts[2].x = rc.left + 4; pts[2].y = rc.bottom - 4;

	if( m_bNextEnabled ) {
		pbrOld = pDC->SelectObject( &brEnabled );
	} else {
		pbrOld = pDC->SelectObject( &brDisabled );
	}
	pDC->Polygon( pts, 3);

	if ( m_bNextHover ) {
		rc.top = m_rcClose.top;
		rc.bottom = m_rcClose.bottom;
		rc.left -= 4;
		rc.right = rc.left + m_rcClose.Width();
		if ( m_bNextPressed ) {
			themeData.DrawPressedRect( pDC, &rc );
		} else {
			themeData.DrawHoverRect( pDC, &rc );
		}
	}

	rc = m_rcPrior;
	rc.OffsetRect( -( m_nWidth - ( lprcBounds->right - lprcBounds->left ) ), 0 );

	pts[0].x = rc.right; pts[0].y = rc.top;
	pts[1].x = rc.right; pts[1].y = rc.bottom;
	pts[2].x = rc.right - 4; pts[2].y = rc.bottom - 4;

	if( m_bPriorEnabled ) {
		pDC->SelectObject( &brEnabled );
	} else {
		pDC->SelectObject( &brDisabled );
	}
	pDC->Polygon( pts, 3);

	if ( m_bPriorHover ) {
		rc.top = m_rcClose.top;
		rc.bottom = m_rcClose.bottom;
		rc.left += 2;
		rc.right = rc.left + m_rcClose.Width();
		if ( m_bPriorPressed ) {
			themeData.DrawPressedRect( pDC, &rc );
		} else {
			themeData.DrawHoverRect( pDC, &rc );
		}
	}

	pDC->SelectObject( pbrOld );
	pDC->SelectObject( pOldPen );
}

void CEGMDIClient::CalcButtons( LPRECT lprcBounds ) {

	ASSERT( lprcBounds != NULL );

	// Координаты кнопок табуляции
	CRect rcButtons;
	rcButtons.left = m_cxEdge;
	CRect rcClient( *lprcBounds );
	m_nWidth = rcClient.Width();
	rcButtons.right = rcClient.Width() - m_cxEdge - m_cxControls;
	m_cxBtnsAvailable = rcButtons.Width();
	m_cxBtnsTotal = 0;

	CEGMDITabBtnsIt it, itFirst = m_lstTabBtns.begin(),
		itLast = m_lstTabBtns.end();
	
	CPaintDC dc( this );
	for( it = itFirst ; it != itLast; ++it ) {
	
		UINT nStyle = STYLE_COOL | ( it->m_hWnd == m_hWndActive ? STYLE_ACTIVE : 0 );

		it->m_nWidth = themeData.MeasureTab( &dc, FALSE, (TCHAR*)(LPCTSTR)it->GetTitle(), ALIGN_TOP, nStyle );
		it->m_nLeft = rcButtons.left;
		
		rcButtons.left += it->m_nWidth;
		m_cxBtnsTotal += it->m_nWidth;
	} 
	
	if ( m_cxOffset > 0 && ( m_cxBtnsTotal - m_cxOffset ) <= m_cxBtnsAvailable )
		m_cxOffset = m_cxBtnsTotal - m_cxBtnsAvailable;

	if ( m_cxOffset > 0 && m_cxBtnsTotal > m_cxBtnsAvailable ) {
		int nDelta = m_cxOffset;
		for( it = itFirst ; it != itLast; ++it )
			it->m_nLeft -= nDelta;
	}

	
	m_bNextEnabled = ( m_cxBtnsTotal - m_cxOffset > m_cxBtnsAvailable );
	m_bPriorEnabled = ( m_cxOffset > 0 );

	
	m_rcClose.SetRect( m_nWidth - 18, 3, m_nWidth - 4, 17 );
	m_rcNext.SetRect( m_nWidth - 30, 5, m_nWidth - 20, 13 );
	m_rcPrior.SetRect( m_nWidth - 50, 5, m_nWidth - 40, 13 );
}

LRESULT CEGMDIClient::OnRecalcButtons ( WPARAM wParam, LPARAM lParam ) {

	CRect rc;
	GetClientRect( &rc );
	CalcButtons( &rc );

	if ( 1 == wParam )
		::SetWindowPos( m_hWnd, NULL, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_DRAWFRAME | SWP_FRAMECHANGED );

	if( 2 == wParam && m_cxBtnsTotal > m_cxBtnsAvailable ) {
		m_cxOffset = m_cxBtnsTotal - m_cxBtnsAvailable;
		CalcButtons( &rc );
	}

	SendMessage( WM_NCPAINT );

	return 0L;
}

#define TEST_PRESSED( name, id ) \
	if ( m_b##name##Enabled && ( HTOBJECT + id == nHitTest ) ) {	\
		m_b##name##Pressed = TRUE;	\
		SetCapture();	\
		bNeedRepaint = TRUE;	\
	} else if( m_b##name##Pressed ) {	\
		m_b##name##Pressed = FALSE;	\
		bNeedRepaint = TRUE;	\
	}	\

#define ON_PRESSED( name, id, func ) \
	if ( m_b##name##Enabled && ( HTOBJECT + id == nHitTest ) )	\
		func##();	\

void CEGMDIClient::OnNcLButtonDown(UINT nHitTest, CPoint point) 
{

	ScreenToClient( &point );
	
	if ( HTOBJECT == nHitTest ) {
		// Searching the child tab at this point
		CEGMDITabBtnsIt it, itFirst = m_lstTabBtns.begin(),
				itLast = m_lstTabBtns.end();
		for( it = itFirst; it != itLast; ++it ) {
			if ( it->m_nLeft <= point.x && (it->m_nLeft + it->m_nWidth) >= point.x ) {
				if ( it->m_hWnd != m_hWndActive ) {
					if ( it->m_nLeft < 0 ) {
						if ( it->m_hWnd != itFirst->m_hWnd ) {
							m_cxOffset -= - it->m_nLeft + 30;
						} else {
							m_cxOffset = 0;
						}
					} else if ( it->m_nLeft + it->m_nWidth > m_cxBtnsAvailable ) {
						if ( it->m_hWnd != m_lstTabBtns.back().m_hWnd ) {
							m_cxOffset += it->m_nLeft + it->m_nWidth - m_cxBtnsAvailable + 30;
						} else {
							m_cxOffset += it->m_nLeft + it->m_nWidth - m_cxBtnsAvailable;
						}
					}
					ActivateMDI( it->m_hWnd );
				} 
				m_bTabDrag = TRUE;
				SetCapture();
				break;
			}
		}
	}

	BOOL bNeedRepaint = FALSE;

	TEST_PRESSED( Close, 1 );
	ON_PRESSED( Next, 2, ScrollNext );
	ON_PRESSED( Prior, 3, ScrollPrior );

	if(  bNeedRepaint )
		SendMessage( WM_NCPAINT );

	// Do not call CWnd::OnNcLButtonDown here! It will break mouse events handling
	// CWnd::OnNcLButtonDown( nHitTest, point );
}

#define ON_BUTTON_UP( name, id, func ) \
	if ( m_b##name##Pressed ) {	\
		m_b##name##Pressed = FALSE;	\
		if ( HTOBJECT + id == nHitTest ) {	\
			func##();	\
			return;	\
		} else {	\
			bNeedRepaint = TRUE;	\
		}	\
	}	\

void CEGMDIClient::OnNcLButtonUp(UINT nHitTest, CPoint point) 
{
	m_bTabDrag = FALSE;
	if ( ::GetCapture() == m_hWnd )
		ReleaseCapture();

	BOOL bNeedRepaint = FALSE;

	ON_BUTTON_UP( Close, 1, CloseActiveMDI ); 
	//ON_BUTTON_UP( Next, 2, ScrollNext ); 
	//ON_BUTTON_UP( Prior, 3, ScrollPrior ); 

	if( bNeedRepaint )
		SendMessage( WM_NCPAINT );

	CWnd::OnNcLButtonUp( nHitTest, point );
}

void CEGMDIClient::OnLButtonUp( UINT nFlags, CPoint point ) 
{
	if ( m_bClosePressed || m_bTabDrag ) {
		ClientToScreen( &point );
		//OnNcLButtonUp( (OnNcHitTest( point ), point );#WARNING
		UINT lRes = (UINT)OnNcHitTest( point );
		OnNcLButtonUp(lRes , point );

	} else {
		CWnd::OnLButtonUp( nFlags, point );
	}
}

#if _MFC_VER < 0x0800  
  afx_msg UINT CEGMDIClient::OnNcHitTest(CPoint point)
  {
#else
  afx_msg LRESULT CEGMDIClient::OnNcHitTest(CPoint point)
  {
#endif
	//UINT nRes = CWnd::OnNcHitTest( point );#WARNING
	UINT nRes = (UINT)CWnd::OnNcHitTest( point );

	CPoint ptScreen( point );

	if ( m_bShowTabs && nRes == HTNOWHERE ) {
		ScreenToClient( &point );
		if ( point.x < (m_nWidth - m_cxEdge - m_cxControls ) && point.y < 0 && -point.y > 3 ) {
			CEGMDITabBtnsIt it = m_lstTabBtns.begin(),
					itLast = m_lstTabBtns.end();
			for( ; it != itLast; ++it ) {
				if ( it->m_nLeft <= point.x && (it->m_nLeft + it->m_nWidth) >= point.x ) {
					nRes = HTOBJECT;
					break;
				}
			}		
		} else if ( point.x > (m_nWidth - m_cxEdge - m_cxControls ) ) {
			
			point.y += TABS_HEIGHT;

			if ( m_rcClose.PtInRect( point ) ) {
				nRes = HTOBJECT + 1;
			} else if ( m_rcNext.PtInRect( point ) ) {
				nRes = HTOBJECT + 2;
			} else if ( m_rcPrior.PtInRect( point ) ) {
				nRes = HTOBJECT + 3;
			}
		}
	}

	if ( HTNOWHERE == nRes )
		OnNcMouseMove( HTNOWHERE, ptScreen );
	
	return nRes;
}

#define TEST_HOVER( name, id ) \
	bWasHover = m_b##name##Hover;	\
	m_b##name##Hover = m_b##name##Enabled && ( HTOBJECT + id == nHitTest );	\
	if ( m_b##name##Hover != bWasHover )	\
			bNeedRePaint = TRUE;	\

void CEGMDIClient::OnNcMouseMove( UINT nHitTest, CPoint point) {

	BOOL bNeedRePaint = FALSE;
	
	if ( m_bTabDrag ) {
		if( nHitTest == HTOBJECT	) {
			// просто перемещение закладки
			CPoint ptClient( point );
			ScreenToClient( &ptClient );
			CEGMDITabBtnsIt itHot = find_if( m_lstTabBtns.begin(), m_lstTabBtns.end(), HotWndBtn( ptClient.x ) );
			if ( itHot != m_lstTabBtns.end() && itHot->m_hWnd != m_hWndActive ) {
				CEGMDITabBtnsIt itActive = find_if( m_lstTabBtns.begin(), m_lstTabBtns.end(), MatchWndBtn( m_hWndActive ) );
				if ( itActive->m_nLeft > itHot->m_nLeft ) {
					itActive->m_nLeft = itHot->m_nLeft;
					itHot->m_nLeft = itActive->m_nLeft + itActive->m_nWidth;
				} else {
					itHot->m_nLeft = itActive->m_nLeft;
					itActive->m_nLeft = itHot->m_nLeft + itHot->m_nWidth;
				}
				iter_swap( itHot, itActive );
				bNeedRePaint = TRUE;
			}
		}
	} else {
		BOOL bWasHover = FALSE;

		TEST_HOVER( Close, 1 )
		TEST_HOVER( Next, 2 )
		TEST_HOVER( Prior, 3 )
	}

	if( bNeedRePaint ) 
		SendMessage( WM_NCPAINT );

	CWnd::OnNcMouseMove( nHitTest, point );
}

void CEGMDIClient::OnMouseMove( UINT nFlags, CPoint point) {

	if ( m_bClosePressed || m_bTabDrag ) {
		ClientToScreen( &point );
		UINT lRes = (UINT)OnNcHitTest( point );
		OnNcMouseMove( lRes, point );
	} else {
		CWnd::OnMouseMove( nFlags, point );
	}
}

void CEGMDIClient::OnCancelMode( ) {
	OnNcLButtonUp( HTNOWHERE, CPoint( 0, 0 ) );
}

void CEGMDIClient::ActivateMDI( HWND hWnd ) {
	SendMessage( WM_MDIACTIVATE, (LPARAM)hWnd, 0 );
}

void CEGMDIClient::CloseActiveMDI() {
	if( m_hWndActive )
		SendMessage( WM_MDIDESTROY, (LPARAM)m_hWndActive, 0 );
}

void CEGMDIClient::ScrollNext() {
	m_cxOffset = m_cxBtnsTotal - m_cxBtnsAvailable;
	m_bNextEnabled = FALSE;
	m_bNextHover = FALSE;
	OnRecalcButtons( 0, 0 );
}

void CEGMDIClient::ScrollPrior() {
	m_cxOffset = 0;
	m_bPriorEnabled = FALSE;
	m_bPriorHover = FALSE;
	OnRecalcButtons( 0, 0 );
}
