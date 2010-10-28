#include "stdafx.h"
#include "EGDocking.h"
#include "NewTheme.h"
#include "EGMenu.h"

#include <algorithm>
#include <functional>

#define SPLITTER_SIZE 4
#define TABS_BKGND RGB(247,243,233)
#define DM_PANESTATE_CHANGED WM_USER + 1
#define _AfxGetDlgCtrlID(hWnd)          ((UINT)(WORD)::GetDlgCtrlID(hWnd))

/////////////////////////////////////////////////////////////////////////
// functors

class HotPaneButton {
	int m_nX;
public:
	HotPaneButton( int x ) {
		m_nX = x;	
	}
	bool operator() ( CEGDockingTabBtn btn ) { 
		return ( btn.m_nLeft <= m_nX && m_nX <= btn.m_nLeft + btn.m_nWidth );
	}
};

class MatchPaneButton {
	CEGDockingPane * m_pPane;
public:
	MatchPaneButton( CEGDockingPane * pPane ) {
		m_pPane = pPane;	
	}
	bool operator() ( CEGDockingTabBtn btn ) { 
		return btn.m_pPane == m_pPane; 
	}
};

// stub
int CALLBACK EnumFontFamProc(ENUMLOGFONT FAR *lpelf,
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

/////////////////////////////////////////////////////////////////////////
// CEGDockingPane

CEGDockingPane::CEGDockingPane() {
	m_pDockingBar = NULL;
	m_pszTitle = NULL;
	m_bHidden = FALSE;
	m_clrColor = themeData.GetNewColor();

	// Caption height
	m_cyCaption = ::GetSystemMetrics(SM_CYSMCAPTION);
	m_pRemovedFromBar = NULL;
}

CEGDockingPane::~CEGDockingPane() {
	if ( m_pDockingBar != NULL ) {
		m_pDockingBar->RemovePane( this );
		if ( m_pDockingBar->IsEmpty() ) 
			delete m_pDockingBar;
		m_pDockingBar = NULL;
	}
	if( m_pszTitle != NULL )
		free( m_pszTitle );
}

BEGIN_MESSAGE_MAP(CEGDockingPane, CStatic)
	ON_NOTIFY( NM_KILLFOCUS, 0, OnKillFocus )
	ON_NOTIFY( NM_SETFOCUS, 0, OnSetFocus )
END_MESSAGE_MAP()

BOOL CEGDockingPane::baseCreate( TCHAR* lpszTitle, CSize size, CFrameWnd* pFrame ) {
	m_pszTitle = _tcsdup( lpszTitle );

	if( !Create( NULL, WS_CHILD /*| WS_BORDER */| WS_CLIPCHILDREN | WS_CLIPSIBLINGS , CRect( CPoint(0,0), size ), pFrame, 0 /* nID */ ) )
		return FALSE;

	return TRUE;
}

void CEGDockingPane::CreateBar( TCHAR* lpszTitle, CSize size, CFrameWnd* pFrame ) {
	m_pDockingBar = new CEGDockingBar( size );
	::SendMessage( pFrame->m_hWnd, DM_DOCKING_BAR, (WPARAM) m_pDockingBar, 0 );
	m_pDockingBar->Create( lpszTitle, 0, pFrame, 0 /*nID*/);
	m_pDockingBar->AddPane( this );
}


BOOL CEGDockingPane::CreatePane( TCHAR* lpszTitle, CSize size, CFrameWnd* pFrame, CEGDockingPane *pPane, DockType nDockType, CPoint ptFloat ) {

	// Create pane
	if( !baseCreate( lpszTitle, size, pFrame ) ) {
		TRACE1("FAILED to CEGDockingPane::baseCreate(\"%s\",...)", lpszTitle );
		return FALSE;
	}

	if ( !pPane ) {
		// dock to client area side

		CreateBar( lpszTitle, size, pFrame );

		int nSide = 0; 
		switch( nDockType ) {
			case dctBottom:
				nSide = AFX_IDW_DOCKBAR_BOTTOM;
				break;
			case dctLeft:
				nSide = AFX_IDW_DOCKBAR_LEFT;
				break;
			case dctRight:
				nSide = AFX_IDW_DOCKBAR_RIGHT;
				break;
			case dctTop:
				nSide = AFX_IDW_DOCKBAR_TOP;
				break;
		};
		if ( 0 == nSide  ) {
			pFrame->FloatControlBar( (CControlBar*)m_pDockingBar, ptFloat );
		} else {
			pFrame->DockControlBar( (CControlBar*)m_pDockingBar, nSide );
			m_pDockingBar->m_pDockContext->m_uMRUDockID = nSide;
		}
		pFrame->RecalcLayout();

	} else if ( pPane && dctTab == nDockType ) {
		// append tab to another pane

		pPane->AppendTab( this );
		m_pDockingBar = pPane->m_pDockingBar;

	} else {
		// dock to border of another pane

		CreateBar( lpszTitle, size, pFrame );

		CRect rc;
		pPane->m_pDockingBar->GetWindowRect( &rc );
		switch( nDockType ) {
			case dctBottom:
				rc.OffsetRect( 0, 20 );
				break;
			case dctLeft:
				rc.OffsetRect( -20, 0 );
				break;
			case dctRight:
				rc.OffsetRect( 20, 0 );
				break;
			case dctTop:
				rc.OffsetRect( 0, -20 );
				break;
		};		

		CDockBar * pDockBar = (CDockBar*)pFrame->GetControlBar(pPane->m_pDockingBar->GetDockBarID());
		pDockBar->DockControlBar( m_pDockingBar, &rc);
		pFrame->RecalcLayout();
		m_pDockingBar->m_pDockContext->m_uMRUDockID = _AfxGetDlgCtrlID(pDockBar->m_hWnd);

	}

	return TRUE;
}

DockType GetDropTarget( CPoint pt, CRect rcClient ) {

	CRect rcArea;

	// trying dock on top
	rcArea = rcClient;
	rcArea.bottom = rcArea.top + min( rcClient.Height(), DROP_AREA_SIZE );
	if ( rcArea.PtInRect( pt ) )
		return dctTop; // can dock on top

	// trying dock on bottom
	rcArea = rcClient;
	rcArea.top = rcArea.bottom - min( rcClient.Height(), DROP_AREA_SIZE );
	if ( rcArea.PtInRect( pt ) )
		return dctBottom; // can dock on bottom

	// trying dock on left
	rcArea = rcClient;
	rcArea.right = rcArea.left + min( rcClient.Width(), DROP_AREA_SIZE );
	if ( rcArea.PtInRect( pt ) )
		return dctLeft; // can dock on bottom

	// trying dock on right
	rcArea = rcClient;
	rcArea.left = rcArea.right - min( rcClient.Width(), DROP_AREA_SIZE );
	if ( rcArea.PtInRect( pt ) )
		return dctRight; // can dock on bottom

	return dctFloat;
}

CRect GetDropRect( DockType dt, CRect rcWindow, int nSize ) {

	CRect rcResult = rcWindow; // default for dctTab;

	switch( dt ) {
		case dctBottom:
			rcResult.top = rcResult.bottom - min( rcWindow.Height(), nSize );
			break;
		case dctLeft:
			rcResult.right = rcResult.left + min( rcWindow.Width(), nSize );
			break;
		case dctRight:
			rcResult.left = rcResult.right - min( rcWindow.Width(), nSize );
			break;
		case dctTop:
			rcResult.bottom = rcResult.top + min( rcWindow.Height(), nSize );
			break;
	};

	return rcResult;
}

DockType CEGDockingPane::GetDropTarget( CPoint pt /* client coordinates */ ) {

	CRect rcClient;
	GetWindowRect( &rcClient );
	rcClient.OffsetRect( -rcClient.TopLeft() );

	return ::GetDropTarget( pt, rcClient );

}

CRect CEGDockingPane::GetDropRect( DockType dt ) {

	CRect rcWindow;
	GetParent()->GetWindowRect( &rcWindow );

	return ::GetDropRect( dt, rcWindow, DOCK_AREA_SIZE );

}

void CEGDockingPane::CreateDragBar( CPoint pt, CSize size, double xScale ) {

	CFrameWnd * pFrame = GetTopLevelFrame();

	CreateBar( GetTitle(), size, pFrame );

	pFrame->FloatControlBar( m_pDockingBar, pt );
	m_pDockingBar->ShowBar( FALSE );

	reinterpret_cast<CEGDockingContext*>( m_pDockingBar->m_pDockContext )->StartDrag( pt, xScale, 0 );

}

void CEGDockingPane::AppendTab( CEGDockingPane* pPane ) {

	ASSERT( m_pDockingBar != NULL );

	m_pDockingBar->AddPane( pPane );
}

int CEGDockingPane::Width() {
	return m_pDockingBar->Width();
}

int CEGDockingPane::Height() { 
	return m_pDockingBar->Height();
}

void CEGDockingPane::RestoreParent() {
	SetParent( m_pDockingBar );
}

void CEGDockingPane::RemoveFromBorder( CEGDockBorder * pBorder, BOOL bShow ) {
	ASSERT( pBorder != NULL );

	SetParent( m_pDockingBar );
	m_pDockingBar->ActivatePane( this );
	m_pDockingBar->RemoveFromBorder( pBorder, bShow );
}

void CEGDockingPane::OnKillFocus( NMHDR* pNMHDR, LRESULT* pResult ) {

	HWND hFocus = ::GetFocus();
	if ( hFocus && !::IsChild( hFocus, GetSafeHwnd() ) )
		GetParent()->PostMessage( WM_KILLFOCUS, (WPARAM)hFocus, 0 );
}

void CEGDockingPane::OnSetFocus( NMHDR* pNMHDR, LRESULT* pResult ) {
	GetParent()->PostMessage( WM_FOCUS_CHANGED, 0, 0 );
}

void CEGDockingPane::Show() {
	
	if ( m_pDockingBar->IsFlyOutMode() && m_bHidden ) {

		m_pDockingBar->m_pBorder->AddButton( DRAWBTNSTYLE_GROUP, this );
		m_bHidden = FALSE;
		//m_pDockingBar->m_pBorder->FlyOutPane( this );
		
		m_pDockingBar->GetTopLevelFrame()->RecalcLayout();
		
	} else if ( m_bHidden ) {
		m_bHidden = FALSE;
		m_pDockingBar->PostMessage( DM_PANESTATE_CHANGED, (WPARAM)this, 1 );
	}

}

void CEGDockingPane::Hide( BOOL bNotifyContainer ) {
	
	if ( m_pDockingBar->IsFlyOutMode() && !m_bHidden ) {
		
		m_pDockingBar->m_pBorder->FlyDownPane( this );
		m_pDockingBar->m_pBorder->RemoveButton( this );
		m_bHidden = TRUE;

		m_pDockingBar->GetTopLevelFrame()->DelayRecalcLayout();
		

	} else if ( !m_bHidden ) {
		m_bHidden = TRUE;
		if ( bNotifyContainer )
			m_pDockingBar->PostMessage( DM_PANESTATE_CHANGED, (WPARAM)this, 0 );
	}
}

void CEGDockingPane::ToggleVisible() {
	if ( !m_bHidden ) {
		Hide();
	} else {
		Show();
	}
}

BOOL CEGDockingPane::SetIcon( UINT nIDResource, HINSTANCE hInst ) {
	return m_bmpIcon.LoadBitmap( nIDResource, hInst );
}

BOOL CEGDockingPane::SetIcon( LPCTSTR lpszResourceName, HINSTANCE hInst ) {
	return m_bmpIcon.LoadBitmap( lpszResourceName, hInst );
}

/////////////////////////////////////////////////////////////////////////
// CEGDockingControlPane

CEGDockingControlPane::CEGDockingControlPane(void)
{
	m_hWndControl = NULL;
	m_nID = 0;
}

CEGDockingControlPane::~CEGDockingControlPane(void)
{
}

HWND CEGDockingControlPane::CreateControl() {
	return NULL;
}

BOOL CEGDockingControlPane::InitControl() {
	return TRUE;
}

BEGIN_MESSAGE_MAP(CEGDockingControlPane, CEGDockingPane)
	ON_WM_CREATE()
	ON_WM_SIZE()
//	ON_MESSAGE(WM_USER,OnKickIdle)
END_MESSAGE_MAP()

int CEGDockingControlPane::OnCreate( LPCREATESTRUCT lpCreateStruct ) {

	if( CEGDockingPane::OnCreate( lpCreateStruct) ) {
		TRACE0("Cannot create docking pane");
		return -1;
	}

	m_hWndControl = CreateControl();

	if ( !m_hWndControl ) {
		TRACE0("Cannot create docking control");
		return -1;
	}

	if ( !InitControl() ) {
		TRACE0("Cannot initialize docking control");
		return -1;
	}

	if ( 0 == m_nID ) 
		return 0;

  if (!m_wndToolBar.CreateEx(this, TBSTYLE_AUTOSIZE|TBSTYLE_FLAT|TBSTYLE_LIST, WS_CHILD | WS_VISIBLE | CBRS_TOP
    |CBRS_TOOLTIPS | CBRS_SIZE_DYNAMIC,CRect(1,1,1,1),AFX_IDW_TOOLBAR) ||
    !m_wndToolBar.LoadToolBar(m_nID))
  {
    TRACE0("Failed to create toolbar\n");
    return -1;      // fail to create
  }

  // Prevent the toolbarbutton to be too big after resizing TaskBar!
  for (int n=0;n<m_wndToolBar.GetCount();n++)
  {
    DWORD dwStyle = m_wndToolBar.GetButtonStyle(n);
    if( !(dwStyle&TBBS_SEPARATOR) )
    {
       m_wndToolBar.SetButtonStyle(n,dwStyle|TBBS_AUTOSIZE);
    }
  }

  // adjust border
  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:
  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_ICY:
  case CEGMenu::STYLE_ICY_NOBORDER:
    m_wndToolBar.SetBorders(0,0,0,4);
    break;
  default:
    m_wndToolBar.SetBorders(0,0,0,0);
    break;
  }

  CRect rcClientStart;
	CRect rcClientNow;
	GetClientRect(rcClientStart);
	RepositionBars(AFX_IDW_CONTROLBAR_FIRST,AFX_IDW_CONTROLBAR_LAST,0,reposQuery,rcClientNow);	

	// Set the icon for this dialog.  The framework does this automatically
	CPoint ptoffset(rcClientNow.left - rcClientStart.left,	rcClientNow.top - rcClientStart.top); 
	CRect  rcchild;					
	CWnd* pwndchild = GetWindow(GW_CHILD);
	while (pwndchild)
	{      
		pwndchild->GetWindowRect(rcchild);
		ScreenToClient(rcchild);	
		rcchild.OffsetRect(ptoffset);
		pwndchild->MoveWindow(rcchild, FALSE);
		pwndchild = pwndchild->GetNextWindow();	
	}
	// adjust the dialog window dimensions	
	CRect rcwindow;	
	GetWindowRect(rcwindow);
	rcwindow.right += rcClientStart.Width() - rcClientNow.Width();
	rcwindow.bottom += rcClientStart.Height() - rcClientNow.Height();
	MoveWindow(rcwindow, FALSE);		// and position the control bars
	RepositionBars(AFX_IDW_CONTROLBAR_FIRST, AFX_IDW_CONTROLBAR_LAST, 0);

	return 0;
}

void CEGDockingControlPane::OnSize( UINT nType, int cx, int cy ) {

	CEGDockingPane::OnSize( nType, cx, cy );


	if ( 0 != m_nID ) 
		RepositionBars(AFX_IDW_CONTROLBAR_FIRST, AFX_IDW_CONTROLBAR_LAST, 0);

	if ( m_hWndControl ) {
		int nDelta = 0;
		if ( 0 != m_nID ) {
			CRect rc;
			m_wndToolBar.GetWindowRect( &rc );
			nDelta = rc.Height();
		}
		::MoveWindow( m_hWndControl, 0, 0 + nDelta  , cx, cy - nDelta, TRUE );
	}
}

void CEGDockingControlPane::SetResourceID( int nID ) {
	m_nID = nID;
}

class CToolCmdUI : public CCmdUI 
{    
  virtual void Enable(BOOL bEnable) 
  {
    ::SendMessage(m_pOther->GetSafeHwnd(), TB_ENABLEBUTTON, m_nID , MAKELPARAM(bEnable, 0));
  }
  virtual void SetCheck(int nCheck = 1)
  { 
    ::SendMessage(m_pOther->GetSafeHwnd(), TB_CHECKBUTTON, m_nID, MAKELPARAM(nCheck, 0)); 
  }
};

/*LRESULT CEGDockingControlPane::OnKickIdle(WPARAM wp, LPARAM lp)
{
  UNREFERENCED_PARAMETER(wp);
  UNREFERENCED_PARAMETER(lp);

	if ( 0 == m_nID ) return false;

  CToolCmdUI cmdUI;
  cmdUI.m_pOther = &m_wndToolBar;
  UINT indexMax = m_wndToolBar.GetToolBarCtrl().GetButtonCount();
  cmdUI.m_nIndexMax = indexMax;
  for (UINT i = 0; i < indexMax; i++) 
  {
    if (m_wndToolBar.GetButtonStyle(i) & TBSTYLE_SEP)
      continue;
    cmdUI.m_nIndex = i;
    cmdUI.m_nID = m_wndToolBar.GetItemID(i);
    cmdUI.m_pMenu = NULL;
    cmdUI.DoUpdate(this,FALSE);
  }
	return false;
  //UpdateDialogControls(this,FALSE);
  //AfxCallWndProc(this, m_hWnd,WM_IDLEUPDATECMDUI, (WPARAM)TRUE, 0);
  //SendMessageToDescendants(WM_IDLEUPDATECMDUI,(WPARAM)TRUE, 0, TRUE, TRUE);
  //return 0;
} 
*/

/////////////////////////////////////////////////////////////////////////
// CEGToolBaredPane
/*
BEGIN_MESSAGE_MAP(CEGToolBaredPane, CEGDockingPane)
		ON_WM_CREATE()
		ON_WM_SIZE()
		ON_MESSAGE(WM_USER,OnKickIdle)
END_MESSAGE_MAP()

int CEGToolBaredPane::OnCreate( LPCREATESTRUCT lpCreateStruct ) {
	
	if( CEGDockingPane::OnCreate( lpCreateStruct) ) {
		TRACE0("Cannot create docking pane");
		return -1;
	}
	
	if ( 0 == m_nID ) 
		return 0;

  if (!m_wndToolBar.CreateEx(this, TBSTYLE_AUTOSIZE|TBSTYLE_FLAT|TBSTYLE_LIST, WS_CHILD | WS_VISIBLE | CBRS_TOP
    |CBRS_TOOLTIPS | CBRS_SIZE_DYNAMIC,CRect(1,1,1,1),AFX_IDW_TOOLBAR) ||
    !m_wndToolBar.LoadToolBar(m_nID))
  {
    TRACE0("Failed to create toolbar\n");
    return -1;      // fail to create
  }

  // Prevent the toolbarbutton to be too big after resizing TaskBar!
  for (int n=0;n<m_wndToolBar.GetCount();n++)
  {
    DWORD dwStyle = m_wndToolBar.GetButtonStyle(n);
    if( !(dwStyle&TBBS_SEPARATOR) )
    {
       m_wndToolBar.SetButtonStyle(n,dwStyle|TBBS_AUTOSIZE);
    }
  }

  // adjust border
  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:
  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_ICY:
  case CEGMenu::STYLE_ICY_NOBORDER:
    m_wndToolBar.SetBorders(0,0,0,4);
    break;
  default:
    m_wndToolBar.SetBorders(0,0,0,0);
    break;
  }

  CRect rcClientStart;
	CRect rcClientNow;
	GetClientRect(rcClientStart);
	RepositionBars(AFX_IDW_CONTROLBAR_FIRST,AFX_IDW_CONTROLBAR_LAST,0,reposQuery,rcClientNow);	

	// Set the icon for this dialog.  The framework does this automatically
	CPoint ptoffset(rcClientNow.left - rcClientStart.left,	rcClientNow.top - rcClientStart.top); 
	CRect  rcchild;					
	CWnd* pwndchild = GetWindow(GW_CHILD);
	while (pwndchild)
	{      
		pwndchild->GetWindowRect(rcchild);
		ScreenToClient(rcchild);	
		rcchild.OffsetRect(ptoffset);
		pwndchild->MoveWindow(rcchild, FALSE);
		pwndchild = pwndchild->GetNextWindow();	
	}
	// adjust the dialog window dimensions	
	CRect rcwindow;	
	GetWindowRect(rcwindow);
	rcwindow.right += rcClientStart.Width() - rcClientNow.Width();
	rcwindow.bottom += rcClientStart.Height() - rcClientNow.Height();
	MoveWindow(rcwindow, FALSE);		// and position the control bars
	RepositionBars(AFX_IDW_CONTROLBAR_FIRST, AFX_IDW_CONTROLBAR_LAST, 0);

	return 0;
}


void CEGToolBaredPane::OnSize( UINT nType, int cx, int cy ) {

	CEGDockingPane::OnSize( nType, cx, cy );
	
	if ( 0 != m_nID ) 
		RepositionBars(AFX_IDW_CONTROLBAR_FIRST, AFX_IDW_CONTROLBAR_LAST, 0);
}

void CEGToolBaredPane::SetResourceID( int nID ) {
	m_nID = nID;
}

LRESULT CEGToolBaredPane::OnKickIdle(WPARAM wp, LPARAM lp)
{
  UNREFERENCED_PARAMETER(wp);
  UNREFERENCED_PARAMETER(lp);

	if ( 0 == m_nID ) return false;

  CToolCmdUI cmdUI;
  cmdUI.m_pOther = &m_wndToolBar;
  UINT indexMax = m_wndToolBar.GetToolBarCtrl().GetButtonCount();
  cmdUI.m_nIndexMax = indexMax;
  for (UINT i = 0; i < indexMax; i++) 
  {
    if (m_wndToolBar.GetButtonStyle(i) & TBSTYLE_SEP)
      continue;
    cmdUI.m_nIndex = i;
    cmdUI.m_nID = m_wndToolBar.GetItemID(i);
    cmdUI.m_pMenu = NULL;
    cmdUI.DoUpdate(this,FALSE);
  }
	return false;
  //UpdateDialogControls(this,FALSE);
  //AfxCallWndProc(this, m_hWnd,WM_IDLEUPDATECMDUI, (WPARAM)TRUE, 0);
  //SendMessageToDescendants(WM_IDLEUPDATECMDUI,(WPARAM)TRUE, 0, TRUE, TRUE);
  //return 0;
} 
*/
/////////////////////////////////////////////////////////////////////////
// CEGDockingBar

IMPLEMENT_DYNAMIC(CEGDockingBar, CControlBar);

CEGDockingBar::CEGDockingBar( CSize size )
{
	m_szMinHorz = CSize(33, 32);
	m_szMinVert = CSize(33, 32);
	m_szMinFloat = CSize(37, 32);
	m_szHorz = size;
	m_szVert = size;
	m_szFloat = CSize( size.cx, size.cx );
	m_bTracking = FALSE;
	m_bKeepSize = FALSE;
	m_bParentSizing = FALSE;
	m_cxEdge = 4;
	m_bDragShowContent = FALSE;
	m_nDockBarID = 0;
	m_dwPaneStyle = 0;
	m_cyCaption = 16;
	m_cyTabs = 25;
	m_bActive = FALSE;
	m_bAutoHidePinned = FALSE;

	m_bClosePressed = FALSE;
	m_bCloseHover = FALSE;

	m_bPinPressed = FALSE;
	m_bPinHover = FALSE;


	m_pActivePane = NULL;
	m_bFlyOutMode = FALSE;
	m_pFrameWnd = NULL;
	m_bTabDrag = FALSE;
	m_pBorder = NULL;
}

CEGDockingBar::~CEGDockingBar()
{
}

BEGIN_MESSAGE_MAP(CEGDockingBar, CControlBar)
	ON_WM_CREATE()
	ON_WM_DESTROY()
	ON_WM_TIMER()
	//ON_WM_PAINT()
	ON_WM_NCPAINT()
	ON_WM_NCCALCSIZE()
	ON_WM_WINDOWPOSCHANGING()
	ON_WM_CAPTURECHANGED()
	ON_WM_CANCELMODE()
	ON_WM_SETTINGCHANGE()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_NCLBUTTONDOWN()
	ON_WM_NCLBUTTONUP()
	ON_WM_NCMOUSEMOVE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONDBLCLK()
	ON_WM_RBUTTONDOWN()
	ON_WM_NCHITTEST()
	ON_WM_CLOSE()
	ON_WM_SIZE()

	ON_MESSAGE(	WM_SETTEXT, OnSetText )
	ON_MESSAGE( WM_NCMOUSELEAVE, OnNcMouseLeave )
	ON_MESSAGE( DM_PANESTATE_CHANGED, OnPaneStateChanged )
END_MESSAGE_MAP()

BOOL CEGDockingBar::Create(LPCTSTR lpszWindowName, UINT nIconID,
													 CWnd* pParentWnd, UINT nID,
													 DWORD dwStyle)
{
	m_nIconID = nIconID;

	// must have a parent
	ASSERT_VALID(pParentWnd);
	// cannot be both fixed and dynamic
	// (CBRS_SIZE_DYNAMIC is used for resizng when floating)
	ASSERT (!((dwStyle & CBRS_SIZE_FIXED) &&
		(dwStyle & CBRS_SIZE_DYNAMIC)));

	m_dwStyle = dwStyle & CBRS_ALL; // save the control bar styles

	// register and create the window - skip CControlBar::Create()
	CString wndclass = ::AfxRegisterWndClass(CS_DBLCLKS,
		::LoadCursor(NULL, IDC_ARROW),
		::GetSysColorBrush(COLOR_BTNFACE), 0);

	dwStyle &= ~CBRS_ALL; // keep only the generic window styles
	dwStyle |= WS_CLIPCHILDREN; // prevents flashing
	if (!CWnd::Create(wndclass, lpszWindowName, dwStyle,
		CRect(0, 0, 0, 0), pParentWnd, nID))
		return FALSE;

	EnableDocking(CBRS_ALIGN_ANY);

	return TRUE;
}

/////////////////////////////////////////////////////////////////////////
// CEGDockingBar attributes

DockType CEGDockingBar::GetDropTarget( CPoint ptScreen, CEGDockingPane ** ppPane ) {

	*ppPane = NULL;

	CRect rcWindow;
	GetWindowRect( &rcWindow );
	if ( !rcWindow.PtInRect( ptScreen ) )
		return dctOutOfBorders;

	if ( IsFloating() ) {
		*ppPane = m_pActivePane;
		return dctTab;
	}

	CPoint pt = ptScreen;
	ScreenToClient( &pt );

	CRect rcActivePane;
	m_pActivePane->GetWindowRect( &rcActivePane );
	ScreenToClient( rcActivePane );

	*ppPane = m_pActivePane;
	if ( !rcActivePane.PtInRect( pt ) )
		return dctTab;

	return (*ppPane)->GetDropTarget( pt );	
}

CRect CEGDockingBar::GetDropRect( DockType nDockType ) {

	CRect rc;
	GetWindowRect( &rc ); // default for dctTab

	switch ( nDockType ) {
		case dctTop:
			rc.bottom = rc.top + min( rc.Height(), m_cyCaption * 2 );
			break;
		case dctBottom:
			rc.top = rc.bottom - min( rc.Height(), m_cyCaption * 2 );
			break;
		case dctLeft:
			rc.right = rc.left + min( rc.Width(), m_cyCaption * 2 );
			break;
		case dctRight:
			rc.left = rc.right - min( rc.Width(), m_cyCaption * 2 );
			break;
	}

	return rc;
}

/////////////////////////////////////////////////////////////////////////
// CEGDockingBar operations

void CEGDockingBar::EnableDocking(DWORD dwDockStyle)
{
	// must be CBRS_ALIGN_XXX or CBRS_FLOAT_MULTI only
	ASSERT((dwDockStyle & ~(CBRS_ALIGN_ANY|CBRS_FLOAT_MULTI)) == 0);
	// cannot have the CBRS_FLOAT_MULTI style
	ASSERT((dwDockStyle & CBRS_FLOAT_MULTI) == 0);
	// the bar must have CBRS_SIZE_DYNAMIC style
	ASSERT((m_dwStyle & CBRS_SIZE_DYNAMIC) != 0);

	m_dwDockStyle = dwDockStyle;
	if (m_pDockContext == NULL)
		m_pDockContext = new CEGDockingContext(this);

	// permanently wire the bar's owner to its current parent
	if (m_hWndOwner == NULL)
		m_hWndOwner = ::GetParent(m_hWnd);
}

/////////////////////////////////////////////////////////////////////////
// CEGDockingBar message handlers

int CEGDockingBar::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if (CControlBar::OnCreate(lpCreateStruct) == -1)
		return -1;

	// query SPI_GETDRAGFULLWINDOWS system parameter
	// OnSettingChange() will update m_bDragShowContent
	m_bDragShowContent = FALSE;
	::SystemParametersInfo(SPI_GETDRAGFULLWINDOWS, 0,
		&m_bDragShowContent, 0);

	// Caption height
	m_cyCaption = ::GetSystemMetrics(SM_CYSMCAPTION);


	// uncomment this line if you want raised borders
	//m_dwPaneStyle |= DPS_SHOWEDGES;

	SetPaneStyle(m_dwPaneStyle|DPS_SIZECHILD);

	//		m_tabs.Create( NULL, NULL, WS_CHILD | FTS_ALIGN_BOTTOM, CRect(0,0,0,0), this, 0, NULL ); 

	m_nTimer = (UINT)SetTimer( 1, 250, NULL );

	return 0;
}

void CEGDockingBar::OnDestroy() {
	KillTimer( m_nTimer );
}

LRESULT CEGDockingBar::OnSetText(WPARAM wParam, LPARAM lParam)
{
	UNUSED_ALWAYS(wParam);

	LRESULT lResult = CWnd::Default();
	CFrameWnd * pFrame = GetParentFrame();

	if (IsFloating() && pFrame && pFrame->IsKindOf(RUNTIME_CLASS(CMiniDockFrameWnd)))
	{
		pFrame->SetWindowText( (LPCTSTR) lParam); // update dockbar
		pFrame->DelayRecalcLayout(); // refresh miniframe
	}

	return lResult;
}

const BOOL CEGDockingBar::IsFloating() const
{
	return !IsHorzDocked() && !IsVertDocked();
}

const BOOL CEGDockingBar::IsHorzDocked() const
{
	return (m_nDockBarID == AFX_IDW_DOCKBAR_TOP ||
		m_nDockBarID == AFX_IDW_DOCKBAR_BOTTOM);
}

const BOOL CEGDockingBar::IsVertDocked() const
{
	return (m_nDockBarID == AFX_IDW_DOCKBAR_LEFT ||
		m_nDockBarID == AFX_IDW_DOCKBAR_RIGHT);
}

const BOOL CEGDockingBar::IsSideTracking() const
{
	// don't call this when not tracking
	ASSERT(m_bTracking && !IsFloating());

	return (m_htEdge == HTLEFT || m_htEdge == HTRIGHT) ?
		IsHorzDocked() : IsVertDocked();
}

CSize CEGDockingBar::CalcFixedLayout(BOOL bStretch, BOOL bHorz)
{
	if (bStretch) // the bar is stretched (is not the child of a dockbar)
		if (bHorz)
			return CSize(32767, m_szHorz.cy);
		else
			return CSize(m_szVert.cx, 32767);

	// dirty cast - we need access to protected CDockBar members
	CEGDockSite* pDockBar = reinterpret_cast<CEGDockSite*>( m_pDockBar );

	// force imediate RecalcDelayShow() for all sizing bars on the row
	// with delayShow/delayHide flags set to avoid IsVisible() problems
	CDPArray arrSCBars;
	GetRowSizingBars(arrSCBars);
	AFX_SIZEPARENTPARAMS layout;
	layout.hDWP = pDockBar->m_bLayoutQuery ?
NULL : ::BeginDeferWindowPos(int(arrSCBars.GetSize()));
	for (INT_PTR i = 0; i < arrSCBars.GetSize(); i++)
		if (arrSCBars[i]->m_nStateFlags & (delayHide|delayShow))
			arrSCBars[i]->RecalcDelayShow(&layout);
	if (layout.hDWP != NULL)
		::EndDeferWindowPos(layout.hDWP);

	// get available length
	CRect rc = pDockBar->m_rectLayout;
	if (rc.IsRectEmpty())
		m_pDockSite->GetClientRect(&rc);
	int nLengthTotal = bHorz ? rc.Width() + 2 : rc.Height() - 2;

	if (IsVisible() && !IsFloating() &&
		m_bParentSizing && arrSCBars[0] == this)
		if (NegotiateSpace(nLengthTotal, (bHorz != FALSE)))
			AlignControlBars();

	m_bParentSizing = FALSE;

	if (bHorz)
		return CSize(max(m_szMinHorz.cx, m_szHorz.cx),
		max(m_szMinHorz.cy, m_szHorz.cy));

	return CSize(max(m_szMinVert.cx, m_szVert.cx),
		max(m_szMinVert.cy, m_szVert.cy));
}

CSize CEGDockingBar::CalcDynamicLayout(int nLength, DWORD dwMode)
{
	if (dwMode & (LM_HORZDOCK | LM_VERTDOCK)) // docked ?
	{
		if (nLength == -1)
			m_bParentSizing = TRUE;

		return CControlBar::CalcDynamicLayout(nLength, dwMode);
	}

	if (dwMode & LM_MRUWIDTH) return m_szFloat;
	if (dwMode & LM_COMMIT) return m_szFloat; // already committed

	// check for dialgonal resizing hit test
	int nHitTest = m_pDockContext->m_nHitTest;
	if (IsFloating() &&
		(nHitTest == HTTOPLEFT || nHitTest == HTBOTTOMLEFT ||
		nHitTest == HTTOPRIGHT || nHitTest == HTBOTTOMRIGHT))
	{
		CPoint ptCursor;
		::GetCursorPos(&ptCursor);

		CRect rFrame, rBar;
		GetParentFrame()->GetWindowRect(&rFrame);
		GetWindowRect(&rBar);

		if (nHitTest == HTTOPLEFT || nHitTest == HTBOTTOMLEFT)
		{
			m_szFloat.cx = rFrame.left + rBar.Width() - ptCursor.x;
			m_pDockContext->m_rectFrameDragHorz.left = 
				min(ptCursor.x, rFrame.left + rBar.Width() - m_szMinFloat.cx);
		}

		if (nHitTest == HTTOPLEFT || nHitTest == HTTOPRIGHT)
		{
			m_szFloat.cy = rFrame.top + rBar.Height() - ptCursor.y;
			m_pDockContext->m_rectFrameDragHorz.top =
				min(ptCursor.y, rFrame.top + rBar.Height() - m_szMinFloat.cy);
		}

		if (nHitTest == HTTOPRIGHT || nHitTest == HTBOTTOMRIGHT)
			m_szFloat.cx = rBar.Width() + ptCursor.x - rFrame.right;

		if (nHitTest == HTBOTTOMLEFT || nHitTest == HTBOTTOMRIGHT)
			m_szFloat.cy = rBar.Height() + ptCursor.y - rFrame.bottom;
	}
	else
		((dwMode & LM_LENGTHY) ? m_szFloat.cy : m_szFloat.cx) = nLength;

	m_szFloat.cx = max(m_szFloat.cx, m_szMinFloat.cx);
	m_szFloat.cy = max(m_szFloat.cy, m_szMinFloat.cy);

	return m_szFloat;
}

void CEGDockingBar::OnWindowPosChanging(WINDOWPOS FAR* lpwndpos)
{
	// force non-client recalc if moved or resized
	lpwndpos->flags |= SWP_FRAMECHANGED;

	CControlBar::OnWindowPosChanging(lpwndpos);

	// find on which side are we docked
	m_nDockBarID = GetParent()->GetDlgCtrlID();

	if (!IsFloating())
		if (lpwndpos->flags & SWP_SHOWWINDOW)
			m_bKeepSize = TRUE;
}

/////////////////////////////////////////////////////////////////////////
// Mouse Handling
//
void CEGDockingBar::OnLButtonDown(UINT nFlags, CPoint point)
{
	HWND hActiveWnd = ::GetFocus();
	if ( hActiveWnd && hActiveWnd !=  m_hWnd && !::IsChild( m_hWnd, hActiveWnd ) ) {
		if ( !m_pActivePane ) {
			::SetFocus( m_hWnd );
		} else {
			HWND hChild = ::GetWindow( m_pActivePane->GetSafeHwnd(), GW_CHILD );
			::SetFocus( hChild != NULL ? hChild : m_hWnd );
		}
	}

	if ( point.y < 0 && -point.y < m_cyCaption ) {
		if ( m_rcClose.PtInRect( point ) ) {
			SetCapture();
			m_bClosePressed = TRUE;
			RePaint();
		} else if ( m_rcPin.PtInRect( point ) ) {
			SetCapture();
			m_bPinPressed = TRUE;
			RePaint();
		}
	}
	CWnd::OnLButtonDown(nFlags, point);
}

void CEGDockingBar::OnLButtonDblClk(UINT nFlags, CPoint point)
{
	if (m_pDockBar != NULL)
	{

		// not a buttons?
		if ( !m_rcClose.PtInRect( point ) && !m_rcPin.PtInRect( point ) ) {
			// toggle docking
			ASSERT(m_pDockContext != NULL);
			m_pDockContext->ToggleDocking();
			TRACE0("CEGDockingBar::OnLButtonDblClk\r\n");
		}
	}
	else {
		TRACE0("CEGDockingBar::OnLButtonDblClk - CWnd::OnLButtonDblClk\r\n");
		CWnd::OnLButtonDblClk(nFlags, point);
	}
}

void CEGDockingBar::OnNcLButtonDown(UINT nHitTest, CPoint point) 
{
	UNUSED_ALWAYS(point);

	if (m_bTracking /* || IsFloating() */ )
		return;

	CPoint ptScreen( point );
	ScreenToClient( &point );
	point.y -= m_cxEdge;

	if ((nHitTest >= HTSIZEFIRST) && (nHitTest <= HTSIZELAST)) {
		StartTracking(nHitTest, ptScreen); // sizing edge hit
	} else if ( point.y >= m_rcClient.Height() && point.y <= m_rcClient.Height() + m_cyTabs ) {
		CEGDockingTabBtnsIt it = find_if( m_lstTabButtons.begin(), m_lstTabButtons.end(), HotPaneButton( point.x ) );
		if ( it != m_lstTabButtons.end() && !it->m_pPane->IsHidden() ) {

			ActivatePane( it->m_pPane );

			m_bTabDrag = TRUE;
			m_nTabDragStart = point.x;

			// Активируем трэкинг события выхода WM_NCMOUSELEAVE
			TRACKMOUSEEVENT tme;
			memset( (LPVOID)&tme, 0, sizeof( TRACKMOUSEEVENT ) );
			tme.cbSize = sizeof( TRACKMOUSEEVENT );
			tme.hwndTrack = m_hWnd;

			tme.dwFlags = TME_CANCEL;
			TrackMouseEvent( &tme );

			tme.dwFlags = TME_LEAVE | TME_NONCLIENT;
			if ( !TrackMouseEvent( &tme ) ) {
				TRACE("TrackMouseEvent FAILED\r\n");
			}

			GetCursorPos( &point );
			OnNcMouseMove( nHitTest, point ); 
		}
		return;
	}
}

void CEGDockingBar::OnNcLButtonUp(UINT nHitTest, CPoint point)
{
	m_bTabDrag = FALSE;
	CControlBar::OnNcLButtonUp(nHitTest, point);
}

void CEGDockingBar::OnNcMouseMove( UINT nHitTest, CPoint point) {

	CPoint pt( point );

	ScreenToClient( &point );
	point.y -= m_cxEdge;

	if ( m_bTabDrag ) {
		if( point.y >= m_rcClient.Height() - m_cyTabs && point.y <= m_rcClient.Height() + m_cyTabs &&
			point.x >= m_rcClient.left + m_cxEdge && point.x <= m_rcClient.right - m_cxEdge	) {

				// просто перемещение закладки
				CEGDockingTabBtnsIt itHot = find_if( m_lstTabButtons.begin(), m_lstTabButtons.end(), HotPaneButton( point.x ) );
				if ( itHot != m_lstTabButtons.end() ) {
					CEGDockingTabBtnsIt itActive = find_if( m_lstTabButtons.begin(), m_lstTabButtons.end(), MatchPaneButton( m_pActivePane ) );
					if ( (*itActive).m_pPane != (*itHot).m_pPane && itActive != m_lstTabButtons.end() ) {
						if ( itActive->m_nLeft > itHot->m_nLeft ) {
							itActive->m_nLeft = itHot->m_nLeft;
							itHot->m_nLeft = itActive->m_nLeft + itActive->m_nWidth;
						} else {
							itHot->m_nLeft = itActive->m_nLeft;
							itActive->m_nLeft = itHot->m_nLeft + itHot->m_nWidth;
						}
						iter_swap( itHot, itActive );
						RePaint();
					}
				}
			} 
	}

	CControlBar::OnNcMouseMove(nHitTest, pt);
}

void CEGDockingBar::OnLButtonUp(UINT nFlags, CPoint point)
{
	if (m_bTracking) {

		StopTracking();
		CControlBar::OnLButtonUp(nFlags, point);

	} else {

		// X-button pressed?
		BOOL bOldClosePressed = m_bClosePressed;
		m_bClosePressed = FALSE;
		if ( bOldClosePressed ) {
			ReleaseCapture();
			if ( m_rcClose.PtInRect( point ) ) {
				if ( IsFloating() ) {
					// hide all pane without notify
					CEGDockingTabBtnsIt it = m_lstTabButtons.begin(),
						itLast = m_lstTabButtons.end();
					for( ; it != itLast; ++it ) 
						it->m_pPane->Hide( TRUE ); 
					// hide bar
					ShowBar( FALSE );
				} else {
					m_pActivePane->Hide();
				}
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
				PinPane( );
			} else {
				RePaint();
			}
		}
	}

}

void CEGDockingBar::OnRButtonDown(UINT nFlags, CPoint point)
{
	if (m_bTracking)
		StopTracking();

	CControlBar::OnRButtonDown(nFlags, point);
}

void CEGDockingBar::OnMouseMove(UINT nFlags, CPoint point)
{
	if (m_bTracking) {

		CPoint ptScreen = point;
		ClientToScreen(&ptScreen);
		OnTrackUpdateSize(ptScreen);

	} else if ( !m_bClosePressed && !m_bPinPressed && (nFlags & MK_LBUTTON) ) {

		ASSERT(m_pDockContext != NULL);
		CPoint ptScreen = point;
		ClientToScreen(&ptScreen);
		if( !IsFloating() && HTCLIENT == OnNcHitTest(ptScreen) ) {

			CPoint pt ( m_rcClient.TopLeft() );
			m_pDockBar->ClientToScreen( &pt );
			reinterpret_cast<CEGDockingContext*>( m_pDockContext )->StartDrag( ptScreen, double ( ptScreen.x - pt.x ) / double ( m_rcClient.Width() ), m_cyCaption + point.y );

		} else if( IsFloating() /*|| HTCLIENT == OnNcHitTest(point) */ ) {

			::SetForegroundWindow( m_hWnd );
			::SetActiveWindow( m_hWnd );
			reinterpret_cast<CEGDockingContext*>( m_pDockContext )->StartDrag( ptScreen, double ( point.x - m_rcClient.left) / double ( m_rcClient.Width() ), m_cyCaption + point.y );

		}
		//return;
	} else {
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
	}

	CControlBar::OnMouseMove(nFlags, point);
}

void CEGDockingBar::OnCaptureChanged(CWnd *pWnd)
{

	if (m_bTracking && (pWnd != this))
		StopTracking();

	if ( m_bPinPressed || m_bClosePressed )
		m_bPinPressed = m_bClosePressed = FALSE;

	CControlBar::OnCaptureChanged(pWnd);
}

void CEGDockingBar::OnNcCalcSize(BOOL bCalcValidRects,
																 NCCALCSIZE_PARAMS FAR* lpncsp)
{
	UNUSED_ALWAYS(bCalcValidRects);

	// compute the the client area
	m_dwPaneStyle &= ~DPS_EDGEALL;

	// add resizing edges between bars on the same row
	if (!IsFloating() && m_pDockBar != NULL)
	{
		CDPArray arrSCBars;
		int nThis;
		GetRowSizingBars(arrSCBars, nThis);

		BOOL bHorz = IsHorzDocked();
		if (nThis > 0)
			m_dwPaneStyle |= bHorz ? DPS_EDGELEFT : DPS_EDGETOP;

		if (nThis < arrSCBars.GetUpperBound())
			m_dwPaneStyle |= bHorz ? DPS_EDGERIGHT : DPS_EDGEBOTTOM;
	}

	NcCalcClient(&lpncsp->rgrc[0], m_nDockBarID);
}

void CEGDockingBar::CalcButtons() {

	// Координаты кнопок табуляции
	CRect rcButtons;
	rcButtons.left = m_cxEdge;
	rcButtons.right = m_rcClient.Width() - m_cxEdge;
	TuneGripper( &rcButtons, m_nDockBarID );
	int nAvailableWidth = rcButtons.Width();	
	int nTotalRequiredWidth = 0;
	CEGDockingTabBtnsIt it, itFirst = m_lstTabButtons.begin(),
		itLast = m_lstTabButtons.end();
	int nCount = 0;

	// фактические размеры
	CPaintDC dc( this );
	for( it = itFirst ; it != itLast; ++it ) {

		if ( it->m_pPane->IsHidden() ) {
			it->m_nWidth = 0;
			continue;
		}

		it->m_nWidth = themeData.MeasureTab( &dc, it->m_pPane->m_bmpIcon.GetWidth() > 0, it->GetTitle(), 
			ALIGN_TOP, ( it->m_pPane == m_pActivePane ) ? STYLE_ACTIVE : 0 );

		nTotalRequiredWidth += it->m_nWidth;
		nCount++;
	}

	if ( !nCount )
		return;

	// устанавливаем те размеры что влезут!
	int nAverageWidth = (int)((float)nAvailableWidth/(float)nCount + (float)0.5);
	for( it = itFirst ; it != itLast; ++it ) {
		if ( it->m_pPane->IsHidden() )
			continue;

		//CEGDockingTabBtn btn = (*it);

		int nWidth = nTotalRequiredWidth < nAvailableWidth ? it->m_nWidth : nAverageWidth;

		it->m_nLeft = rcButtons.left;
		it->m_nWidth = nWidth;

		rcButtons.left += nWidth ;
	}
}

void CEGDockingBar::NcCalcClient(LPRECT pRc, UINT nDockBarID)
{
	m_rcNonClient = *pRc;
	m_rcClient = *pRc;
	m_rcClient.DeflateRect(2,2,2,2); // 3D

	switch(nDockBarID)
	{
	case AFX_IDW_DOCKBAR_TOP:
		m_dwPaneStyle |= DPS_EDGEBOTTOM;
		break;
	case AFX_IDW_DOCKBAR_BOTTOM:
		m_dwPaneStyle |= DPS_EDGETOP;
		break;
	case AFX_IDW_DOCKBAR_LEFT:
		m_dwPaneStyle |= DPS_EDGERIGHT;
		break;
	case AFX_IDW_DOCKBAR_RIGHT:
		m_dwPaneStyle |= DPS_EDGELEFT;
		break;
	}

	// Координаты заголовка
	SetRect(&m_rcCaption,0,-m_cyCaption,m_rcClient.Width(),0);
	TuneGripper( &m_rcCaption, nDockBarID );

	// Заголовок не входит в клиентскую область!!!
	m_rcClient.top += m_cyCaption + (nDockBarID == AFX_IDW_DOCKBAR_BOTTOM ? m_cxEdge : 0);
	m_rcClient.left += (nDockBarID == AFX_IDW_DOCKBAR_RIGHT ? m_cxEdge : 0);
	m_rcClient.right -= (nDockBarID == AFX_IDW_DOCKBAR_LEFT ? m_cxEdge : 0);
	//m_rcClient.bottom -= (nDockBarID == AFX_IDW_DOCKBAR_TOP ? m_cxEdge : 0);

	if( GetVisibleCount() > 1 )
		m_rcClient.bottom -= m_cyTabs;

	// Координаты кнопки закрытия
	SetRect(&m_rcClose, m_rcCaption.right-m_cyCaption+2, m_rcCaption.top+1, m_rcCaption.right-1, m_rcCaption.bottom-1 );

	// Координаты кнопки докинга
	if(!IsFloating())
		SetRect(&m_rcPin,m_rcClose.left - m_cyCaption+2,m_rcClose.top,m_rcClose.left-1, m_rcClose.bottom);

	*pRc = m_rcClient;

	// Координаты кнопок
	CalcButtons();
}

void CEGDockingBar::TuneGripper( LPRECT lprcBounds, UINT nDockBarID ) {
	if ( AFX_IDW_DOCKBAR_RIGHT == nDockBarID)  {
		lprcBounds->left += m_cxEdge;
	} else if ( AFX_IDW_DOCKBAR_LEFT == nDockBarID)  {
		lprcBounds->right -= m_cxEdge;
	} else if ( AFX_IDW_DOCKBAR_BOTTOM == nDockBarID) {
		lprcBounds->top += m_cxEdge;
		lprcBounds->bottom += m_cxEdge;
	}
}

void CEGDockingBar::OnNcPaint() {

	// get window DC that is clipped to the non-client area
	CWindowDC dc(this);
	CRect rectClient;
	GetClientRect(rectClient);
	CRect rectWindow;
	GetWindowRect(rectWindow);
	ScreenToClient(rectWindow);
	rectClient.OffsetRect(-rectWindow.left, -rectWindow.top);

	dc.ExcludeClipRect(rectClient);

	if( m_lstDockPanes.size() > 1 )
		rectClient.bottom += m_cyTabs;
	DrawBorders(&dc, rectClient);

	rectWindow.OffsetRect(-rectWindow.left, -rectWindow.top);
	CRect rcGripper( rectWindow );
	rcGripper.bottom = rcGripper.top + m_cyCaption + 2;
	dc.FillSolidRect( &rcGripper, themeData.clrBtnFace );

	// draw gripper in non-client area
	rcGripper.top = 2;
	rcGripper.DeflateRect(2,0);
	TuneGripper( &rcGripper, m_nDockBarID );
	DrawGripper( &dc, rcGripper );

	// draw tabs
	if (m_lstDockPanes.size() > 1 ) {
		CRect rcTabs( rcGripper );
		rcTabs.bottom = rectWindow.bottom - m_cxEdge;
		rcTabs.top = rcTabs.bottom - m_cyTabs/* + m_cxEdge */;

		DrawTabs( &dc, &rcTabs );
	}
}

void CEGDockingBar::DrawTabs( CDC * pDC, LPRECT lprcBounds ) {

	ASSERT( NULL != lprcBounds );


	if( GetVisibleCount() <= 1 )
		return;

	themeData.DrawTabCtrlBK( pDC, lprcBounds, ALIGN_BOTTOM, TRUE, m_pActivePane->Color() );

	// Draw buttons  
	CRect rcButton( *lprcBounds );
	rcButton.bottom-=1;

	CEGDockingTabBtnsIt it = m_lstTabButtons.begin(),
		itLast = m_lstTabButtons.end();
	for( ; it != itLast; ++it ) {
		if ( it->m_pPane->IsHidden() )
			continue;

		CEGDockingTabBtn btn = (*it);

		BOOL bSelected = ( btn.m_pPane == m_pActivePane );

		// surface
		int nWidth = btn.m_nWidth;

		rcButton.left = btn.m_nLeft;
		rcButton.right = rcButton.left + nWidth;

		themeData.DrawTab( pDC, &rcButton, &btn.m_pPane->m_bmpIcon, btn.GetTitle(), ALIGN_BOTTOM,  bSelected ? STYLE_ACTIVE : 0, btn.m_pPane->Color() );

		rcButton.OffsetRect( nWidth, 0 );
	}

}

void CEGDockingBar::GetGripperRect( LPRECT lprc ) {
	CRect rectClient;
	GetClientRect(rectClient);
	CRect rectWindow;
	GetWindowRect(rectWindow);
	ScreenToClient(rectWindow);
	rectClient.OffsetRect(-rectWindow.left, -rectWindow.top);
	rectClient.top +=2;
	rectClient.bottom = rectClient.top + m_cyCaption;

	*lprc = rectClient;
}


void CEGDockingBar::DrawGripper(CDC* pDC, const CRect& rect) {

	DWORD dwFlags = 0;
	if ( IsFloating()||m_bActive ) 
		dwFlags |= DGF_GRIPPER_ACTIVE;
	if ( IsFloating() ) 
		dwFlags |= DGF_GRIPPER_FLOATING;
	if( !IsFloating() )
		dwFlags |= DGF_PIN_VISIBLE;
	if( m_bPinPressed )
		dwFlags |= DGF_PIN_PRESSED;
	if( m_bPinHover )
		dwFlags |= DGF_PIN_HOVER;
	if( m_bClosePressed )
		dwFlags |= DGF_CLOSE_PRESSED;
	if( m_bCloseHover )
		dwFlags |= DGF_CLOSE_HOVER;

	CString strTitle;
	GetWindowText( strTitle );

	themeData.DrawGripper( pDC, rect, /*pGripperFont, */ (TCHAR*)(LPCTSTR)strTitle, dwFlags );
}


#if _MFC_VER < 0x0800  
  afx_msg UINT CEGDockingBar::OnNcHitTest(CPoint point)
  {
#else
  afx_msg LRESULT CEGDockingBar::OnNcHitTest(CPoint point)
  {
#endif

	CRect rcBar, rcEdge;
	GetWindowRect(rcBar);

	if (!IsFloating())
		for (int i = 0; i < 4; i++){
			if (GetEdgeRect(rcBar, GetEdgeHTCode(i), rcEdge)&& rcEdge.PtInRect(point))
				return m_bTabDrag ? HTOBJECT : GetEdgeHTCode(i);
		}

		CRect rc;

		// Gripper
		rc = m_rcCaption;
		rc.OffsetRect(rcBar.TopLeft());
		rc.OffsetRect( 0, m_cyCaption );
		if (rc.PtInRect(point)) {

			/*			rc = m_rcClose;
			rc.OffsetRect(rcBar.TopLeft());
			rc.OffsetRect( 0, m_cyCaption );
			if ( rc.PtInRect(point))
			return HTOBJECT;

			rc = m_rcPin;
			rc.OffsetRect(rcBar.TopLeft());
			rc.OffsetRect( 0, m_cyCaption );
			if ( rc.PtInRect(point))
			return HTOBJECT;
			*/
			return HTCLIENT;
		}

		// Tabs
		ScreenToClient( &point );
		if ( point.y > m_rcClient.Height() && m_lstTabButtons.size() > 1 ) {
			// using only X coord
			int x = point.x;
			CEGDockingTabBtnsIt it = find_if( m_lstTabButtons.begin(), 
				m_lstTabButtons.end(), HotPaneButton(x) );
			if( it != m_lstTabButtons.end() )
				return HTOBJECT;
		}

		return HTNOWHERE;
}

void CEGDockingBar::OnSettingChange(UINT uFlags, LPCTSTR lpszSection)
{
	CControlBar::OnSettingChange(uFlags, lpszSection);

	m_bDragShowContent = FALSE;
	::SystemParametersInfo(SPI_GETDRAGFULLWINDOWS, 0, &m_bDragShowContent, 0);
}

void CEGDockingBar::OnSize(UINT /*nType*/, int /*cx*/, int /*cy*/)
{
	CRect rc( m_rcClient );

	if ( m_pActivePane && m_pActivePane->m_pRemovedFromBar != this ) {
//		TRACE1( "sizing pane(%s)\r\n", m_pActivePane->GetTitle() );
		m_pActivePane->MoveWindow( 0, 2, rc.Width(), rc.Height() - 3 );
	}
	//if ( m_lstDockPanes.size() > 1 ) {
	//	m_tabs.MoveWindow( 0, rc.Height(), rc.Width(), m_cyTabs );
	//	m_tabs.UpdateWindow();
	//}
}

void CEGDockingBar::OnClose()
{
	// do nothing: protection against accidentally destruction by the
	//   child control (i.e. if user hits Esc in a child editctrl)
}

/////////////////////////////////////////////////////////////////////////
// CEGDockingBar implementation helpers

void CEGDockingBar::StartTracking(UINT nHitTest, CPoint point)
{
	SetCapture();

	// make sure no updates are pending
	if (!m_bDragShowContent)
		RedrawWindow(NULL, NULL, RDW_ALLCHILDREN | RDW_UPDATENOW);

	m_htEdge = nHitTest;
	m_bTracking = TRUE;

	BOOL bHorz = IsHorzDocked();
	BOOL bHorzTracking = m_htEdge == HTLEFT || m_htEdge == HTRIGHT;

	m_nTrackPosOld = bHorzTracking ? point.x : point.y;

	CRect rcBar, rcEdge;
	GetWindowRect(rcBar);
	GetEdgeRect(rcBar, m_htEdge, rcEdge);
	m_nTrackEdgeOfs = m_nTrackPosOld -
		(bHorzTracking ? rcEdge.CenterPoint().x : rcEdge.CenterPoint().y);

	CDPArray arrSCBars;
	int nThis;
	GetRowSizingBars(arrSCBars, nThis);

	m_nTrackPosMin = m_nTrackPosMax = m_nTrackPosOld;
	if (!IsSideTracking())
	{
		// calc minwidth as the max minwidth of the sizing bars on row
		int nMinWidth = bHorz ? m_szMinHorz.cy : m_szMinVert.cx;
		for (int i = 0; i < arrSCBars.GetSize(); i++)
			nMinWidth = max(nMinWidth, bHorz ? 
			arrSCBars[i]->m_szMinHorz.cy :
		arrSCBars[i]->m_szMinVert.cx);
		int nExcessWidth = (bHorz ? m_szHorz.cy : m_szVert.cx) - nMinWidth;

		// the control bar cannot grow with more than the width of
		// remaining client area of the mainframe
		CRect rcT;
		m_pDockSite->RepositionBars(0, 0xFFFF, AFX_IDW_PANE_FIRST,
			reposQuery, &rcT, NULL, TRUE);
		int nMaxWidth = bHorz ? rcT.Height() - 2 : rcT.Width() - 2;

		BOOL bTopOrLeft = m_htEdge == HTTOP || m_htEdge == HTLEFT;

		m_nTrackPosMin -= bTopOrLeft ? nMaxWidth : nExcessWidth;
		m_nTrackPosMax += bTopOrLeft ? nExcessWidth : nMaxWidth;
	}
	else
	{
		// side tracking:
		// max size is the actual size plus the amount the other
		// sizing bars can be decreased until they reach their minsize
		if (m_htEdge == HTBOTTOM || m_htEdge == HTRIGHT)
			nThis++;

		for (int i = 0; i < arrSCBars.GetSize(); i++)
		{
			CEGDockingBar* pBar = arrSCBars[i];

			int nExcessWidth = bHorz ? 
				pBar->m_szHorz.cx - pBar->m_szMinHorz.cx :
			pBar->m_szVert.cy - pBar->m_szMinVert.cy;

			if (i < nThis)
				m_nTrackPosMin -= nExcessWidth;
			else
				m_nTrackPosMax += nExcessWidth;
		}
	}

	OnTrackInvertTracker(); // draw tracker
}

void CEGDockingBar::StopTracking()
{
	OnTrackInvertTracker(); // erase tracker

	m_bTracking = FALSE;
	ReleaseCapture();

	m_pDockSite->DelayRecalcLayout();
}

void CEGDockingBar::OnTrackUpdateSize(CPoint& point)
{
	ASSERT(!IsFloating());

	BOOL bHorzTrack = m_htEdge == HTLEFT || m_htEdge == HTRIGHT;

	int nTrackPos = bHorzTrack ? point.x : point.y;
	nTrackPos = max(m_nTrackPosMin, min(m_nTrackPosMax, nTrackPos));

	int nDelta = nTrackPos - m_nTrackPosOld;

	if (nDelta == 0)
		return; // no pos change

	OnTrackInvertTracker(); // erase tracker

	m_nTrackPosOld = nTrackPos;

	BOOL bHorz = IsHorzDocked();

	CSize sizeNew = bHorz ? m_szHorz : m_szVert;
	switch (m_htEdge)
	{
	case HTLEFT:    sizeNew -= CSize(nDelta, 0); break;
	case HTTOP:     sizeNew -= CSize(0, nDelta); break;
	case HTRIGHT:   sizeNew += CSize(nDelta, 0); break;
	case HTBOTTOM:  sizeNew += CSize(0, nDelta); break;
	}

	CDPArray arrSCBars;
	int nThis;
	GetRowSizingBars(arrSCBars, nThis);

	if (!IsSideTracking())
		for (int i = 0; i < arrSCBars.GetSize(); i++)
		{
			CEGDockingBar* pBar = arrSCBars[i];
			// make same width (or height)
			(bHorz ? pBar->m_szHorz.cy : pBar->m_szVert.cx) =
				bHorz ? sizeNew.cy : sizeNew.cx;
		}
	else
	{
		int nGrowingBar = nThis;
		BOOL bBefore = m_htEdge == HTTOP || m_htEdge == HTLEFT;
		if (bBefore && nDelta > 0)
			nGrowingBar--;
		if (!bBefore && nDelta < 0)
			nGrowingBar++;
		if (nGrowingBar != nThis)
			bBefore = !bBefore;

		// nGrowing is growing
		nDelta = abs(nDelta);
		CEGDockingBar* pBar = arrSCBars[nGrowingBar];
		(bHorz ? pBar->m_szHorz.cx : pBar->m_szVert.cy) += nDelta;

		// the others are shrinking
		INT_PTR nFirst = bBefore ? nGrowingBar - 1 : nGrowingBar + 1;
		INT_PTR nLimit = bBefore ? -1 : arrSCBars.GetSize();

		for (INT_PTR i = nFirst; nDelta != 0 && i != nLimit; i += (bBefore ? -1 : 1))
		{
			CEGDockingBar* pBar = arrSCBars[i];

			int nDeltaT = min(nDelta,
				(bHorz ? pBar->m_szHorz.cx : pBar->m_szVert.cy) -
				(bHorz ? pBar->m_szMinHorz.cx : pBar->m_szMinVert.cy));

			(bHorz ? pBar->m_szHorz.cx : pBar->m_szVert.cy) -= nDeltaT;
			nDelta -= nDeltaT;
		}
	}

	OnTrackInvertTracker(); // redraw tracker at new pos

	if (m_bDragShowContent)
		m_pDockSite->DelayRecalcLayout();
}

void CEGDockingBar::OnTrackInvertTracker()
{
	ASSERT(m_bTracking);

	if (m_bDragShowContent)
		return; // don't show tracker if DragFullWindows is on

	BOOL bHorz = IsHorzDocked();
	CRect rc, rcBar, rcDock, rcFrame;
	GetWindowRect(rcBar);
	m_pDockBar->GetWindowRect(rcDock);
	m_pDockSite->GetWindowRect(rcFrame);
	VERIFY(GetEdgeRect(rcBar, m_htEdge, rc));
	if (!IsSideTracking())
		rc = bHorz ? 
		CRect(rcDock.left + 1, rc.top, rcDock.right - 1, rc.bottom) :
	CRect(rc.left, rcDock.top + 1, rc.right, rcDock.bottom - 1);

	BOOL bHorzTracking = m_htEdge == HTLEFT || m_htEdge == HTRIGHT;
	int nOfs = m_nTrackPosOld - m_nTrackEdgeOfs;
	nOfs -= bHorzTracking ? rc.CenterPoint().x : rc.CenterPoint().y;
	rc.OffsetRect(bHorzTracking ? nOfs : 0, bHorzTracking ? 0 : nOfs);
	rc.OffsetRect(-rcFrame.TopLeft());

	CDC *pDC = m_pDockSite->GetDCEx(NULL,
		DCX_WINDOW | DCX_CACHE | DCX_LOCKWINDOWUPDATE);
	CBrush* pBrush = CDC::GetHalftoneBrush();
	CBrush* pBrushOld = pDC->SelectObject(pBrush);

	pDC->PatBlt(rc.left, rc.top, rc.Width(), rc.Height(), PATINVERT);

	pDC->SelectObject(pBrushOld);
	m_pDockSite->ReleaseDC(pDC);
}

BOOL CEGDockingBar::GetEdgeRect(CRect rcWnd, UINT nHitTest,
																CRect& rcEdge)
{
	rcEdge = rcWnd;
	if (m_dwPaneStyle & DPS_SHOWEDGES)
		rcEdge.DeflateRect(1, 1);
	BOOL bHorz = IsHorzDocked();

	switch (nHitTest)
	{
	case HTLEFT:
		if (!(m_dwPaneStyle & DPS_EDGELEFT)) return FALSE;
		rcEdge.right = rcEdge.left + m_cxEdge;
		rcEdge.DeflateRect(0, bHorz ? m_cxEdge: 0);
		break;
	case HTTOP:
		if (!(m_dwPaneStyle & DPS_EDGETOP)) return FALSE;
		rcEdge.bottom = rcEdge.top + m_cxEdge;
		rcEdge.DeflateRect(bHorz ? 0 : m_cxEdge, 0);
		break;
	case HTRIGHT:
		if (!(m_dwPaneStyle & DPS_EDGERIGHT)) return FALSE;
		rcEdge.left = rcEdge.right - m_cxEdge;
		rcEdge.DeflateRect(0, bHorz ? m_cxEdge: 0);
		break;
	case HTBOTTOM:
		if (!(m_dwPaneStyle & DPS_EDGEBOTTOM)) return FALSE;
		rcEdge.top = rcEdge.bottom - m_cxEdge;
		rcEdge.DeflateRect(bHorz ? 0 : m_cxEdge, 0);
		break;
	default:
		ASSERT(FALSE); // invalid hit test code
	}
	return TRUE;
}

UINT CEGDockingBar::GetEdgeHTCode(int nEdge)
{
	if (nEdge == 0) return HTLEFT;
	if (nEdge == 1) return HTTOP;
	if (nEdge == 2) return HTRIGHT;
	if (nEdge == 3) return HTBOTTOM;
	ASSERT(FALSE); // invalid edge code
	return HTNOWHERE;
}

void CEGDockingBar::GetRowInfo(INT_PTR& nFirst, INT_PTR& nLast, INT_PTR& nThis)
{
	ASSERT_VALID(m_pDockBar); // verify bounds

	nThis = m_pDockBar->FindBar(this);
	ASSERT(nThis != -1);

	INT_PTR i, nBars = m_pDockBar->m_arrBars.GetSize();

	// find the first and the last bar in row
	for (nFirst = -1, i = nThis - 1; i >= 0 && nFirst == -1; i--)
		if (m_pDockBar->m_arrBars[i] == NULL)
			nFirst = i + 1;
	for (nLast = -1, i = nThis + 1; i < nBars && nLast == -1; i++)
		if (m_pDockBar->m_arrBars[i] == NULL)
			nLast = i - 1;

	ASSERT((nLast != -1) && (nFirst != -1));
}

void CEGDockingBar::GetRowSizingBars(CDPArray& arrSCBars)
{
	INT_PTR nThis; // dummy
	GetRowSizingBars(arrSCBars, nThis);
}

void CEGDockingBar::GetRowSizingBars(CDPArray& arrSCBars, INT_PTR& nThis)
{
	arrSCBars.RemoveAll();

	int nFirstT, nLastT, nThisT;
	GetRowInfo(nFirstT, nLastT, nThisT);

	nThis = -1;
	for (int i = nFirstT; i <= nLastT; i++)
	{
		CEGDockingBar* pBar =
			(CEGDockingBar*) m_pDockBar->m_arrBars[i];
		if (HIWORD(pBar) == 0) continue; // placeholder
		if (!pBar->IsVisible()) continue;
		if (pBar->IsKindOf(RUNTIME_CLASS(CEGDockingBar)))
		{
			if (pBar == this)
				nThis = arrSCBars.GetSize();

			arrSCBars.Add(pBar);
		}
	}
}

BOOL CEGDockingBar::NegotiateSpace(int nLengthTotal, BOOL bHorz)
{
	ASSERT(bHorz == IsHorzDocked());

	int nFirst, nLast, nThis;
	GetRowInfo(nFirst, nLast, nThis);

	int nLengthAvail = nLengthTotal;
	int nLengthActual = 0;
	int nLengthMin = 2;
	int nWidthMax = 0;
	CEGDockingBar* pBar;

	for (int i = nFirst; i <= nLast; i++)
	{
		pBar = (CEGDockingBar*) m_pDockBar->m_arrBars[i];
		if (HIWORD(pBar) == 0) continue; // placeholder
		if (!pBar->IsVisible()) continue;
		BOOL bIsSizingBar = 
			pBar->IsKindOf(RUNTIME_CLASS(CEGDockingBar));

		int nLengthBar; // minimum length of the bar
		if (bIsSizingBar)
			nLengthBar = bHorz ? pBar->m_szMinHorz.cx - 2 :
		pBar->m_szMinVert.cy - 2;
		else
		{
			CRect rcBar;
			pBar->GetWindowRect(&rcBar);
			nLengthBar = bHorz ? rcBar.Width() - 2 : rcBar.Height() - 2;
		}

		nLengthMin += nLengthBar;
		if (nLengthMin > nLengthTotal)
		{
			// split the row after fixed bar
			if (i < nThis)
			{
				m_pDockBar->m_arrBars.InsertAt(i + 1,
					(CControlBar*) NULL);
				return FALSE;
			}

			// only this sizebar remains on the row, adjust it to minsize
			if (i == nThis)
			{
				if (bHorz)
					m_szHorz.cx = m_szMinHorz.cx;
				else
					m_szVert.cy = m_szMinVert.cy;

				return TRUE; // the dockbar will split the row for us
			}

			// we have enough bars - go negotiate with them
			m_pDockBar->m_arrBars.InsertAt(i, (CControlBar*) NULL);
			nLast = i - 1;
			break;
		}

		if (bIsSizingBar)
		{
			nLengthActual += bHorz ? pBar->m_szHorz.cx - 2 : 
		pBar->m_szVert.cy - 2;
		nWidthMax = max(nWidthMax, bHorz ? pBar->m_szHorz.cy :
		pBar->m_szVert.cx);
		}
		else
			nLengthAvail -= nLengthBar;
	}

	CDPArray arrSCBars;
	GetRowSizingBars(arrSCBars);
	INT_PTR nNumBars = arrSCBars.GetSize();
	int nDelta = nLengthAvail - nLengthActual;

	// return faster when there is only one sizing bar per row (this one)
	if (nNumBars == 1)
	{
		ASSERT(arrSCBars[0] == this);

		if (nDelta == 0)
			return TRUE;

		m_bKeepSize = FALSE;
		(bHorz ? m_szHorz.cx : m_szVert.cy) += nDelta;

		return TRUE;
	}

	// make all the bars the same width
	for (INT_PTR i = 0; i < nNumBars; i++)
		if (bHorz)
			arrSCBars[i]->m_szHorz.cy = nWidthMax;
		else
			arrSCBars[i]->m_szVert.cx = nWidthMax;

	// distribute the difference between the bars,
	// but don't shrink them below their minsizes
	while (nDelta != 0)
	{
		int nDeltaOld = nDelta;
		for (INT_PTR i = 0; i < nNumBars; i++)
		{
			pBar = arrSCBars[i];
			int nLMin = bHorz ?
				pBar->m_szMinHorz.cx : pBar->m_szMinVert.cy;
			int nL = bHorz ? pBar->m_szHorz.cx : pBar->m_szVert.cy;

			if ((nL == nLMin) && (nDelta < 0) || // already at min length
				pBar->m_bKeepSize) // or wants to keep its size
				continue;

			// sign of nDelta
			int nDelta2 = (nDelta == 0) ? 0 : ((nDelta < 0) ? -1 : 1);

			(bHorz ? pBar->m_szHorz.cx : pBar->m_szVert.cy) += nDelta2;
			nDelta -= nDelta2;
			if (nDelta == 0) break;
		}
		// clear m_bKeepSize flags
		if ((nDeltaOld == nDelta) || (nDelta == 0))
			for ( INT_PTR i = 0; i < nNumBars; i++)
				arrSCBars[i]->m_bKeepSize = FALSE;
	}

	return TRUE;
}

void CEGDockingBar::AlignControlBars()
{
	int nFirst, nLast, nThis;
	GetRowInfo(nFirst, nLast, nThis);

	BOOL bHorz = IsHorzDocked();
	BOOL bNeedRecalc = FALSE;
	int nAlign = bHorz ? -2 : 0;

	CRect rc, rcDock;
	m_pDockBar->GetWindowRect(&rcDock);

	for (int i = nFirst; i <= nLast; i++)
	{
		CEGDockingBar* pBar =
			(CEGDockingBar*) m_pDockBar->m_arrBars[i];
		if (HIWORD(pBar) == 0) continue; // placeholder
		if (!pBar->IsVisible()) continue;

		pBar->GetWindowRect(&rc);
		rc.OffsetRect(-rcDock.TopLeft());

		if (pBar->IsKindOf(RUNTIME_CLASS(CEGDockingBar)))
			rc = CRect(rc.TopLeft(),
			bHorz ? pBar->m_szHorz : pBar->m_szVert);

		if ((bHorz ? rc.left : rc.top) != nAlign)
		{
			if (!bHorz)
				rc.OffsetRect(0, nAlign - rc.top - 2);
			else if (m_nDockBarID == AFX_IDW_DOCKBAR_TOP)
				rc.OffsetRect(nAlign - rc.left, -2);
			else
				rc.OffsetRect(nAlign - rc.left, 0);
			pBar->MoveWindow(rc);
			bNeedRecalc = TRUE;
		}
		nAlign += (bHorz ? rc.Width() : rc.Height()) - 2;
	}

	if (bNeedRecalc)
		m_pDockSite->DelayRecalcLayout();
}

void CEGDockingBar::LoadState(LPCTSTR lpszProfileName)
{
	ASSERT_VALID(this);
	ASSERT(GetSafeHwnd()); // must be called after Create()

	// compensate the caption miscalculation in CFrameWnd::SetDockState()
	CDockState state;
	state.LoadState(lpszProfileName);

	UINT nID = GetDlgCtrlID();
	for (int i = 0; i < state.m_arrBarInfo.GetSize(); i++)
	{
		CControlBarInfo* pInfo = (CControlBarInfo*)state.m_arrBarInfo[i];
		ASSERT(pInfo != NULL);
		if (!pInfo->m_bFloating)
			continue;

		// this is a floating dockbar - check the ID array
		for (int j = 0; j < pInfo->m_arrBarID.GetSize(); j++)
			if ((DWORD) (DWORD_PTR)pInfo->m_arrBarID[j] == nID)
			{
				// found this bar - offset origin and save settings
				pInfo->m_pointPos.x++;
				pInfo->m_pointPos.y +=
					::GetSystemMetrics(SM_CYSMCAPTION) + 1;
				pInfo->SaveState(lpszProfileName, i);
			}
	}

	CWinApp* pApp = AfxGetApp();

	TCHAR szSection[256];
	wsprintf(szSection, _T("%s-SCBar-%d"), lpszProfileName,
		GetDlgCtrlID());

	m_szHorz.cx = max(m_szMinHorz.cx, (int) pApp->GetProfileInt(
		szSection, _T("sizeHorzCX"), m_szHorz.cx));
	m_szHorz.cy = max(m_szMinHorz.cy, (int) pApp->GetProfileInt(
		szSection, _T("sizeHorzCY"), m_szHorz.cy));

	m_szVert.cx = max(m_szMinVert.cx, (int) pApp->GetProfileInt(
		szSection, _T("sizeVertCX"), m_szVert.cx));
	m_szVert.cy = max(m_szMinVert.cy, (int) pApp->GetProfileInt(
		szSection, _T("sizeVertCY"), m_szVert.cy));

	m_szFloat.cx = max(m_szMinFloat.cx, (int) pApp->GetProfileInt(
		szSection, _T("sizeFloatCX"), m_szFloat.cx));
	m_szFloat.cy = max(m_szMinFloat.cy, (int) pApp->GetProfileInt(
		szSection, _T("sizeFloatCY"), m_szFloat.cy));
}

void CEGDockingBar::SaveState(LPCTSTR lpszProfileName)
{
	// place your SaveState or GlobalSaveState call in
	// CMainFrame's OnClose() or DestroyWindow(), not in OnDestroy()
	ASSERT_VALID(this);
	ASSERT(GetSafeHwnd());

	CWinApp* pApp = AfxGetApp();

	TCHAR szSection[256];
	wsprintf(szSection, _T("%s-SCBar-%d"), lpszProfileName,
		GetDlgCtrlID());

	pApp->WriteProfileInt(szSection, _T("sizeHorzCX"), m_szHorz.cx);
	pApp->WriteProfileInt(szSection, _T("sizeHorzCY"), m_szHorz.cy);

	pApp->WriteProfileInt(szSection, _T("sizeVertCX"), m_szVert.cx);
	pApp->WriteProfileInt(szSection, _T("sizeVertCY"), m_szVert.cy);

	pApp->WriteProfileInt(szSection, _T("sizeFloatCX"), m_szFloat.cx);
	pApp->WriteProfileInt(szSection, _T("sizeFloatCY"), m_szFloat.cy);
}

void CEGDockingBar::GlobalLoadState(CFrameWnd* pFrame,
																		LPCTSTR lpszProfileName)
{
	POSITION pos = pFrame->m_listControlBars.GetHeadPosition();
	while (pos != NULL)
	{
		CEGDockingBar* pBar = 
			(CEGDockingBar*) pFrame->m_listControlBars.GetNext(pos);
		ASSERT(pBar != NULL);
		if (pBar->IsKindOf(RUNTIME_CLASS(CEGDockingBar)))
			pBar->LoadState(lpszProfileName);
	}
}

void CEGDockingBar::GlobalSaveState(CFrameWnd* pFrame,
																		LPCTSTR lpszProfileName)
{
	POSITION pos = pFrame->m_listControlBars.GetHeadPosition();
	while (pos != NULL)
	{
		CEGDockingBar* pBar =
			(CEGDockingBar*) pFrame->m_listControlBars.GetNext(pos);
		ASSERT(pBar != NULL);
		if (pBar->IsKindOf(RUNTIME_CLASS(CEGDockingBar)))
			pBar->SaveState(lpszProfileName);
	}
}


void CEGDockingBar::AddToBorder(CEGDockBorder *pBorder){

	GetWindowRect(&m_rcSaved);

	m_bFlyOutMode = TRUE;
	m_pFrameWnd = GetParentFrame();
	m_pBorder = pBorder;

	if( m_lstDockPanes.back() != m_lstDockPanes.front() ) {

		CEGDockingPanesIt it = m_lstDockPanes.begin(),
			itLast = m_lstDockPanes.end();

		for ( ; it != itLast; ++it )
			if ( !((*it)->IsHidden()) ) 
				pBorder->AddButton( DRAWBTNSTYLE_GROUP | ( (*it) == m_pActivePane ? DRAWBTNSTYLE_SELECT : 0 ), (*it) );	

	} else {

		pBorder->AddButton( DRAWBTNSTYLE_BTN | DRAWBTNSTYLE_SELECT, m_lstDockPanes.front() );

	}

}

void CEGDockingBar::RemoveFromBorder( CEGDockBorder *pBorder, BOOL bShow ){

	if ( !bShow ) {
	
		pBorder->RemoveButton( m_pActivePane );
		m_pActivePane->Hide( FALSE );
	
	} else {

		m_bFlyOutMode = FALSE;
		m_pFrameWnd = NULL;
		m_pBorder = NULL;

		CEGDockingPanesIt it = m_lstDockPanes.begin(),
			itLast = m_lstDockPanes.end();

		for ( ; it != itLast; ++it )
			pBorder->RemoveButton( (*it) );

		pBorder->CalcLayout();
		pBorder->Invalidate();

		RecalcLocalLayout();
		ShowBar( bShow );
	}
}

void CEGDockingBar::OnUpdateCmdUI(CFrameWnd* pTarget,
																	BOOL bDisableIfNoHndler)
{
	CheckState();

//	if ( m_pActivePane ) 
//		m_pActivePane->SendMessage( WM_USER, bDisableIfNoHndler );

}

void CEGDockingBar::PinPane() {
	GetParentFrame()->PostMessage(DM_AUTOHIDE_ON, 0, (LPARAM)this);
}

void CEGDockingBar::RePaint() {
	SendMessage(WM_NCPAINT);	
}

void CEGDockingBar::ShowPane( CEGDockingPane* pPane ) {

	// Активация панели
	ActivatePane( pPane );	
}

void CEGDockingBar::RecalcLocalLayout() {
	if( m_pActivePane && !m_pActivePane->IsWindowVisible() )
		m_pActivePane->ShowWindow( SW_SHOW );

	SetWindowPos( NULL, 0, 0, 0, 0 , SWP_NOMOVE | SWP_NOSIZE | SWP_DRAWFRAME | SWP_FRAMECHANGED );
	OnSize( 0, 0, 0 );
}

void CEGDockingBar::ActivatePane( CEGDockingPane* pPane ) {

	if ( m_pActivePane == pPane ) {
		HWND hChild = ::GetWindow( m_pActivePane->GetSafeHwnd(), GW_CHILD );
		if ( hChild && ::GetFocus() != hChild )
			::SetFocus( hChild );
		return;
	}

	if( GetSafeHwnd() == 0 )
		return;

	if ( m_pActivePane )
		m_pActivePane->ShowWindow( SW_HIDE );

	m_pActivePane = pPane;
	if ( m_pActivePane ) {
		TRACE1( "ActivatePane(%s)\r\n", m_pActivePane->GetTitle() );
		SetWindowText( m_pActivePane->GetTitle() );

		RecalcLocalLayout();
		m_pActivePane->ShowWindow( SW_SHOW );

		::SetFocus( ::GetWindow( m_pActivePane->GetSafeHwnd(), GW_CHILD ) );
		RePaint();
	}
}

void CEGDockingBar::AddPane( CEGDockingPane* pPane ) {

	ASSERT( pPane != NULL );

	m_lstDockPanes.push_back( pPane );
	m_lstTabButtons.push_back( CEGDockingTabBtn( pPane ) );

	pPane->SetParent( this );
	pPane->m_pDockingBar = this;

	ActivatePane( pPane );
}


void CEGDockingBar::RemovePane( CEGDockingPane* pPane ) {

	ASSERT( pPane != NULL );

	CEGDockingTabBtnsIt itButton = find_if ( m_lstTabButtons.begin(), m_lstTabButtons.end(), MatchPaneButton(pPane) );
	ASSERT( itButton != m_lstTabButtons.end() );
	m_lstTabButtons.erase( itButton );

	CEGDockingPanesIt itPane =  find_if ( m_lstDockPanes.begin(), m_lstDockPanes.end(),  bind1st( std::equal_to< CEGDockingPane* >(), pPane ) );
	ASSERT( itPane != m_lstDockPanes.end() );
	m_lstDockPanes.erase( itPane );

	if ( !m_lstDockPanes.empty() ) {
		::PostMessage( GetSafeHwnd(), DM_PANESTATE_CHANGED, (WPARAM)pPane, 0 );
	} else {
		if ( m_pDockBar != NULL ) {
			m_pDockBar->RemoveControlBar( this );
			m_pDockBar = NULL;
		}
		DestroyWindow();
	}
}

void CEGDockingBar::DrawBorders(CDC* pDC, CRect& rect)
{
	ASSERT_VALID(this);
	ASSERT_VALID(pDC);

	CRect rc;

	// top border
	rc = rect;
	rc.bottom = rc.top - m_cyCaption;
	rc.top = rc.bottom - m_cxEdge - 2;
	pDC->FillSolidRect( &rc, themeData.clrBtnFace );

	// right border
	rc = rect;
	rc.left = rc.right;
	rc.right += m_cxEdge + 2;
	pDC->FillSolidRect( &rc, themeData.clrBtnFace );

	// bottom
	rc = rect;
	rc.top = rc.bottom;
	rc.bottom += m_cxEdge + 2;
	if ( m_lstDockPanes.size() > 1 )
		rc.top -= m_cyTabs;
	pDC->FillSolidRect( &rc, themeData.clrBtnFace );

	// left side
	rc = rect;
	rc.right = rc.left;
	rc.left -= m_cxEdge + 2;
	pDC->FillSolidRect( &rc, themeData.clrBtnFace );
}

void CEGDockingBar::DoPaint(CDC* pDC)
{
	ASSERT_VALID(this);
	ASSERT_VALID(pDC);
}

void CEGDockingBar::CheckState() {
	BOOL bNeedPaint = FALSE;

	// Определение активности панели
	HWND hwndFocus = ::GetFocus();
	BOOL bActiveOld = m_bActive;
	m_bActive = ( hwndFocus && ( ::IsChild( m_hWnd, hwndFocus ) || hwndFocus == m_hWnd ) );
	if (m_bActive != bActiveOld)
		bNeedPaint = TRUE;

	if ( ::GetCapture() != m_hWnd ) {
		CPoint pt;
		::GetCursorPos(&pt);
		ScreenToClient(&pt);
		if ( m_bPinHover && !m_rcPin.PtInRect( pt ) ) {
			m_bPinHover = FALSE;
			bNeedPaint = TRUE;
		} else if ( m_bCloseHover && !m_rcClose.PtInRect( pt ) ) {
			m_bCloseHover = FALSE;
			bNeedPaint = TRUE;
		}
	}

	if ( bNeedPaint )
		RePaint();	
}

void CEGDockingBar::OnTimer( UINT_PTR nIDEvent ) {
	CheckState();
}

void CEGDockingBar::OnCancelMode( ) {
	m_bTabDrag = FALSE;
}

LRESULT CEGDockingBar::OnNcMouseLeave( WPARAM /*wParam*/, LPARAM /*lParam*/ ) {
	// canceling hottrack mode
	if ( m_bTabDrag ) {
		CEGDockingPane * pFloatPane = m_pActivePane;

		RemovePane( pFloatPane );

		ShowPane( GetNextActivePane( pFloatPane ) );
		RecalcLocalLayout();
		RePaint();		

		CPoint pt;
		::GetCursorPos( &pt );
		pFloatPane->CreateDragBar( pt, m_szFloat, double( m_nTabDragStart - m_rcClient.left ) / double ( m_rcClient.Width() ) );
	}
	m_bTabDrag = FALSE;

	return 0L;
}

void CEGDockingBar::ShowBar( BOOL bVisible ) {
	CFrameWnd * pFrameWnd = GetParentFrame();
	int nDockBarID = m_nDockBarID;
	pFrameWnd->ShowControlBar( this, bVisible, FALSE );
	if ( bVisible )
		nDockBarID = m_nDockBarID;
	RecalcBarSizes( (CDockBar*)pFrameWnd->GetControlBar( nDockBarID ) );
}

int CEGDockingBar::GetVisibleCount() {
	int nRes = 0;
	CEGDockingTabBtnsIt it = m_lstTabButtons.begin(),
		itLast = m_lstTabButtons.end();
	for( ; it != itLast; ++it ) 
		if ( !it->m_pPane->IsHidden() ) 
			nRes++;
	return nRes;
}

LRESULT CEGDockingBar::OnPaneStateChanged( WPARAM wParam, LPARAM lParam ) {

	CEGDockingPane * pPane = reinterpret_cast<CEGDockingPane *>( wParam );
	BOOL bWasShowed = lParam == 1;

	int nVisibleCount = GetVisibleCount();

	BOOL bIsVisible = IsWindowVisible();
	if ( nVisibleCount > 0 && !bIsVisible ) {
		// need to show bar
		ShowBar( TRUE );
		ShowPane( pPane );
	} else if ( 0 == nVisibleCount && bIsVisible ) {
		// need to hide bar
		ShowBar( FALSE );
	} else {
		// just need to Repaint
		if ( m_pActivePane == pPane && !bWasShowed )
			ShowPane( GetNextActivePane( pPane ) );
		RecalcLocalLayout();
		RePaint();
	}
	return 0L;
}

CEGDockingPane * CEGDockingBar::GetNextActivePane( CEGDockingPane * pActivePane ) {

	// 1. Try to find active pane
	CEGDockingPanesIt itPane =  find_if ( m_lstDockPanes.begin(), m_lstDockPanes.end(),  bind1st( std::equal_to< CEGDockingPane* >(), pActivePane ) );

	if ( itPane != m_lstDockPanes.end() ) {
		// active pane was hidden - need next visible pane
		CEGDockingPanesIt it = itPane, itLast = m_lstDockPanes.end();
		for ( ; it != itLast; ++it )
			if ( !(*it)->IsHidden() )
				return (*it);
	}

	// active pane was removed or prior visible pane not found - need last visible pane
	CEGDockingPanesRIt it = m_lstDockPanes.rbegin(), 
		itLast = m_lstDockPanes.rend();
	for ( ; it != itLast; ++it )
		if ( !(*it)->IsHidden() )
			return (*it);

	return NULL;
}


void RecalcBarSizes( CDockBar * pDockBar ) {

	if( !pDockBar )
		return;

	// calc average size
	int nVisibleCount = pDockBar->GetDockedVisibleCount();
	if ( 0 == nVisibleCount )
		return;

	CRect rcWindow;
	pDockBar->GetWindowRect( &rcWindow );

	BOOL bHorz = pDockBar->m_dwStyle & CBRS_ORIENT_HORZ;
	int nTotalSize = bHorz ? rcWindow.Width() : rcWindow.Height();
	int nSize = nTotalSize / nVisibleCount;


	// set average size
	CRect rc( rcWindow );
	INT_PTR nCount = pDockBar->m_arrBars.GetSize();
	for ( INT_PTR nIndex = 0; nIndex < nCount; ++nIndex ) {
		CControlBar * pBar = (CControlBar*)pDockBar->m_arrBars.GetAt( nIndex );
		if ( pBar && pBar->IsVisible() && pBar->IsKindOf( RUNTIME_CLASS( CEGDockingBar ) ) ) {

			CEGDockingBar * pdBar = ( CEGDockingBar * ) pBar;
			if ( bHorz ) {
				pdBar->m_szHorz.cx = nSize;
			} else {
				pdBar->m_szVert.cy = nSize;
			}

		}
	}

}