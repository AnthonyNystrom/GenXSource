#include "stdafx.h"
#include "EGTaskBar.h"

#define CAPTION_SIZE 25

CEGTaskBar::CEGTaskBar(void)
{
	m_hActiveTask = NULL;
	m_pszCaption = NULL;

	m_fntCaption = NULL;
	HFONT hFont = (HFONT) GetStockObject( DEFAULT_GUI_FONT );
	if ( NULL != hFont ) {
		LOGFONT lf;
		if ( GetObject( hFont, sizeof( LOGFONT), &lf ) ) {
			lf.lfWeight = FW_BOLD;
			lf.lfHeight = 20;
			m_fntCaption = ::CreateFontIndirect( &lf );
		}
	}
    
	m_bActive = FALSE;
	m_hIcon = NULL;
	m_bAutoFree = TRUE;
}

CEGTaskBar::~CEGTaskBar(void)
{
	if( NULL != m_pszCaption )
		free( m_pszCaption );
	if( NULL != m_fntCaption )
		DeleteObject( m_fntCaption );
	if( NULL != m_hIcon )
		DestroyIcon( m_hIcon );
}

BEGIN_MESSAGE_MAP(CEGTaskBar, CEGControlBar)
	ON_WM_SETFOCUS()
END_MESSAGE_MAP()

void CEGTaskBar::SetActiveTask( HWND hTask, TCHAR* pszTitle ) {

	if ( m_hActiveTask )
		::ShowWindow( m_hActiveTask, SW_HIDE );

	m_hActiveTask = hTask;
	if (NULL != m_pszCaption )
		free ( m_pszCaption );
	if ( pszTitle )
		m_pszCaption = _tcsdup( pszTitle );
	else
		m_pszCaption = NULL;

	if ( m_hActiveTask )
		::ShowWindow( m_hActiveTask, SW_SHOW );

	InvalidateCaption();

	CRect rc;
	GetInsideRect( rc );
	OnResize( rc );
}

HWND CEGTaskBar::GetActiveTask( ) {
	return m_hActiveTask;
}

BOOL CEGTaskBar::SetIcon( UINT nIDResource, HINSTANCE hInst ) {
	return SetIcon( MAKEINTRESOURCE( nIDResource ), hInst );
}

BOOL CEGTaskBar::SetIcon( LPCTSTR lpszResourceName, HINSTANCE hInst ) {
	if ( NULL == hInst )
		hInst = ::AfxFindResourceHandle( lpszResourceName, RT_BITMAP);

	if ( lpszResourceName )
		return SetIcon( (HICON) LoadImage( hInst, lpszResourceName, IMAGE_ICON, 16, 16, 0 ), TRUE );

	return SetIcon( (HICON) NULL, TRUE );
}

BOOL CEGTaskBar::SetIcon( HICON hIcon, BOOL bAutoFree ) {
	if ( m_hIcon && m_bAutoFree)
		::DestroyIcon( m_hIcon );

	m_hIcon = hIcon;
	m_bAutoFree = bAutoFree;

	InvalidateCaption();
	
	return TRUE;
}

void CEGTaskBar::GetCaptionRect( CRect& rc ) {
	GetInsideRect( rc );
	rc.bottom = rc.top + CAPTION_SIZE;
}

void CEGTaskBar::OnDraw( CDC * pDC, CRect& rc ) {

	CEGControlBar::OnDraw( pDC, rc );

	// Draw body
	rc.top++;
	pDC->Draw3dRect( rc, GetSysColor(COLOR_BTNSHADOW), GetSysColor(COLOR_BTNSHADOW) );

	// Draw Caption
	CRect rcCaption( rc );
	rcCaption.bottom = rc.top + CAPTION_SIZE - 1;
	pDC->FillSolidRect(rcCaption, GetSysColor(COLOR_BTNSHADOW));

	// Draw title
	HFONT hOldFont = (HFONT) ::SelectObject( pDC->GetSafeHdc(), m_fntCaption );
	rcCaption.left += SPLITTER_SIZE;
	pDC->SetTextColor( GetSysColor(COLOR_WINDOW) );
	pDC->SetBkMode(TRANSPARENT);
	TCHAR* pszTitle = m_pszCaption ? m_pszCaption : _T("");
	pDC->DrawText( pszTitle, rcCaption, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);
	::SelectObject( pDC->GetSafeHdc(), hOldFont );

	// Draw icon
	if ( m_hIcon ) {
		rcCaption.left = rcCaption.right - CAPTION_SIZE - 1;
		DrawIconEx( pDC->GetSafeHdc(), rcCaption.left + rcCaption.Width()/2 - 8, rcCaption.top + rcCaption.Height()/2-8, m_hIcon, 16, 16, 0, NULL, DI_NORMAL);
	}
}

void CEGTaskBar::InvalidateCaption() {
	CRect rc;
	GetCaptionRect( rc );
	InvalidateRect( &rc, FALSE );
}

void CEGTaskBar::OnResize( CRect& rc )
{
	CEGControlBar::OnResize( rc );

	if ( NULL != m_hActiveTask )
		::MoveWindow( m_hActiveTask, rc.left+1, rc.top+CAPTION_SIZE, rc.Width()-2, rc.Height()-1-CAPTION_SIZE, TRUE); 
}

void CEGTaskBar::OnUpdateCmdUI(CFrameWnd* /* pTarget */, BOOL /* bDisableIfNoHndler */) {

	HWND hActiveWnd = ::GetFocus();
	BOOL bActiveOld = m_bActive;
	
	m_bActive = ( m_hWnd == hActiveWnd || m_hActiveTask == hActiveWnd );

	if ( m_bActive != bActiveOld )
		InvalidateCaption();
}

void CEGTaskBar::OnSetFocus( CWnd * /* pOldWnd */) {
	if ( m_hActiveTask )
		::SetFocus( m_hActiveTask );
}
