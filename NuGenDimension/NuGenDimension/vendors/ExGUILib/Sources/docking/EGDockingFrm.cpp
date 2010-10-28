#include "stdafx.h"
#include "EGDockingFrm.h"
#include "EGDockingPane.h"


#define CX_BORDER   1
#define CY_BORDER   1
#define _AfxGetDlgCtrlID(hWnd)          ((UINT)(WORD)::GetDlgCtrlID(hWnd))

/////////////////////////////////////////////////////////////////////////////
// CEGDockingContext Drag Operations

#define HORZF(dw) (dw & CBRS_ORIENT_HORZ)
#define VERTF(dw) (dw & CBRS_ORIENT_VERT)


static void AdjustRectangle(CRect& rect, CPoint pt, double xScale, int yOffset )
{
	int x = pt.x - int( double ( rect.Width() ) * xScale ) ;
	int y = 0 == yOffset ? pt.y - rect.Height() : pt.y - yOffset ; 

	rect.MoveToXY( x, y );

}

void CEGDockingContext::StartDrag(CPoint pt, double xScale, int yOffset )
{
    ASSERT_VALID(m_pBar);
    m_bDragging = TRUE;

    InitLoop();

    ASSERT((m_pBar->m_dwStyle & CBRS_SIZE_DYNAMIC) != 0);

    // get true bar size (including borders)
		CRect rect;
    m_pBar->GetWindowRect(rect);
		m_rcCaption = rect;
		m_rcCaption.bottom = m_rcCaption.top + GetSystemMetrics( SM_CYCAPTION ) + 1;

    m_ptLast = pt;
    sizeHorz = m_pBar->CalcDynamicLayout(0, LM_HORZ | LM_HORZDOCK);
    sizeVert = m_pBar->CalcDynamicLayout(0, LM_VERTDOCK);
    sizeFloat = m_pBar->CalcDynamicLayout(0, LM_HORZ | LM_MRUWIDTH);

    m_rectDragHorz = CRect(rect.TopLeft(), sizeHorz);
    m_rectDragVert = CRect(rect.TopLeft(), sizeVert);

    // calculate frame dragging rectangle
    m_rectFrameDragHorz = CRect(rect.TopLeft(), sizeFloat);


    CMiniFrameWnd::CalcBorders(&m_rectFrameDragHorz, WS_THICKFRAME);

    m_rectFrameDragHorz.DeflateRect(2, 2);
    m_rectFrameDragVert = m_rectFrameDragHorz;
    
    // adjust rectangles so that point is inside
		AdjustRectangle(m_rectDragHorz, pt, xScale, yOffset );
    AdjustRectangle(m_rectDragVert, pt, xScale, yOffset );
    AdjustRectangle(m_rectFrameDragHorz, pt, xScale, yOffset );
    AdjustRectangle(m_rectFrameDragVert, pt, xScale, yOffset );

    // initialize tracking state and enter tracking loop
		m_pDropTarget = NULL;
		m_nDockType = GetDropTarget( pt, &m_pDropTarget );
		m_dwOverDockStyle = 0;
		m_bDrawing = m_pBar->IsFloating() ? TRUE : FALSE;
    MoveNew( pt );   // call it here to handle special keys
    TrackNew();
}

BOOL CEGDockingContext::TrackNew() {
	// don't handle if capture already set
	if (::GetCapture() != NULL)
		return FALSE;

	// set capture to the window which received this message
	m_pBar->SetCapture();
	ASSERT(m_pBar == CWnd::GetCapture());

	// get messages until capture lost or cancelled/accepted
	while (CWnd::GetCapture() == m_pBar)
	{
		MSG msg;
		if (!::GetMessage(&msg, NULL, 0, 0))
		{
			AfxPostQuitMessage((int)msg.wParam);
			break;
		}

		switch (msg.message)
		{
		case WM_LBUTTONUP:
			if (m_bDragging)
				EndDragNew();
			else
				EndResize();
			return TRUE;
		case WM_MOUSEMOVE:
			if (m_bDragging) {
				if ( !m_bDrawing ) {
					if ( !m_rcCaption.PtInRect( msg.pt ) )
						m_bDrawing = TRUE;
				} else {
					//TRACE2(" CPoint( %d, %d ) ", msg.pt.x, msg.pt.y );
					MoveNew(msg.pt);
				}
			} else
				Stretch(msg.pt);
			break;
		case WM_KEYUP:
			if (m_bDragging)
				OnKey((int)msg.wParam, FALSE);
			break;
		case WM_KEYDOWN:
			if (m_bDragging)
				OnKey((int)msg.wParam, TRUE);
			if (msg.wParam == VK_ESCAPE)
			{
				CancelLoop();
				return FALSE;
			}
			break;
		case WM_RBUTTONDOWN:
			CancelLoop();
			return FALSE;

		// just dispatch rest of the messages
		default:
			DispatchMessage(&msg);
			break;
		}
	}

	CancelLoop();

	return FALSE;
}

DockType CEGDockingContext::GetDropTarget( CPoint pt, CEGDockingPane ** ppPane ) {
	
	// 1. Try to drop on some bar
	POSITION pos = m_pDockSite->m_listControlBars.GetHeadPosition();
	while (pos != NULL) {

		CDockBar* pDockBar = (CDockBar*) m_pDockSite->m_listControlBars.GetNext(pos);
		if (	pDockBar->IsKindOf( RUNTIME_CLASS( CEGDockingBar ) ) ) {
			if ( pDockBar != m_pBar && pDockBar->IsVisible() ) {
				DockType nDockType = reinterpret_cast<CEGDockingBar*>( pDockBar )->GetDropTarget( pt, ppPane );
				if ( nDockType != dctOutOfBorders )
					return nDockType;
			
				*ppPane = NULL;
			}
		}
	}

	// 2. Try to drop on client area 
	CRect rcClientArea;
	GetClientArea( &rcClientArea );

	*ppPane = NULL;
	return ::GetDropTarget( pt, rcClientArea );
}

DWORD CEGDockingContext::CanDock( CPoint pt ) {
	
	CEGDockingPane * pPane = NULL;

	return dctFloat != GetDropTarget( pt, &pPane );
}

void CEGDockingContext::GetClientArea( LPRECT lpRect ) {
	m_pDockSite->RepositionBars(0, 0xFFFF, AFX_IDW_PANE_FIRST, CWnd::reposQuery, lpRect, 0, TRUE);
	m_pDockSite->ClientToScreen( lpRect );
}

void CEGDockingContext::MoveNew(CPoint pt)
{
	CPoint ptOffset = pt - m_ptLast;

	// offset all drag rects to new position
	m_rectDragHorz.OffsetRect(ptOffset);
	m_rectFrameDragHorz.OffsetRect(ptOffset);
	m_rectDragVert.OffsetRect(ptOffset);
	m_rectFrameDragVert.OffsetRect(ptOffset);
	m_ptLast = pt;

	if ( GetAsyncKeyState( VK_CONTROL ) < 0 ) { 
		m_nDockType = dctFloat;
		m_pDropTarget = NULL;
	} else {
		m_nDockType = GetDropTarget( pt, &m_pDropTarget );
	}

	CWnd* pMainWnd = AfxGetMainWnd();
	

	if ( m_pDropTarget ) {
		m_rcDropArea = m_pDropTarget->GetDropRect( m_nDockType );
	} else {
		CRect rcClientArea;
		GetClientArea( &rcClientArea );

		int nSize = sizeFloat.cy;
		if ( reinterpret_cast<CEGDockingBar*>(m_pBar)->IsVertDocked() )
			nSize = sizeHorz.cx;
		if ( reinterpret_cast<CEGDockingBar*>(m_pBar)->IsHorzDocked() )
			nSize = sizeVert.cy;
		
		m_rcDropArea = ::GetDropRect( m_nDockType, rcClientArea, nSize );
	}

	m_rectDragHorz = m_rectDragVert = m_rcDropArea;

	// update feedback
	if( m_bDrawing )
		DrawFocusRect();
}

void CEGDockingContext::DrawFocusRect(BOOL bRemoveRect)
{
	ASSERT(m_pDC != NULL);

	// default to thin frame
	CSize size(CX_BORDER, CY_BORDER);
	size.cx = GetSystemMetrics(SM_CXFRAME) - CX_BORDER;
	size.cy = GetSystemMetrics(SM_CYFRAME) - CY_BORDER;

	// determine new rect and size
	CRect rect;
	CBrush* pWhiteBrush = CBrush::FromHandle((HBRUSH)::GetStockObject(WHITE_BRUSH));
	CBrush* pDitherBrush = CDC::GetHalftoneBrush();
	CBrush* pBrush = pDitherBrush;

	if ( m_nDockType != dctFloat )
		rect = m_rcDropArea;
	else
		rect = m_rectFrameDragHorz;

	if (bRemoveRect)
		size.cx = size.cy = 0;

	// draw it and remember last size
	m_pDC->DrawDragRect(&rect, size, &m_rectLast, m_sizeLast,
		pBrush, m_bDitherLast ? pDitherBrush : pWhiteBrush);

	m_rectLast = rect;
	m_sizeLast = size;
	m_bDitherLast = (pBrush == pDitherBrush);
}


void CEGDockingContext::EndDragNew()
{
	CancelLoop();

	CPoint pt = m_ptLast;

	CEGDockingBar * pSourceBar = reinterpret_cast<CEGDockingBar*>(m_pBar);

	if ( !m_pDropTarget && m_nDockType != dctFloat ) {

		// MDI Client area side
	
		DWORD dwDockSide = AFX_IDW_DOCKBAR_TOP;
		switch( m_nDockType ) {
			case dctBottom:
				dwDockSide = AFX_IDW_DOCKBAR_BOTTOM;
				break;
			case dctLeft:
				dwDockSide = AFX_IDW_DOCKBAR_LEFT;
				break;
			case dctRight:
				dwDockSide = AFX_IDW_DOCKBAR_RIGHT;
				break;
		};
		
		m_pDockSite->DockControlBar(m_pBar, dwDockSide);
		m_pDockSite->ShowControlBar( m_pBar, TRUE, FALSE );
		SetForegroundWindow( m_pBar->GetSafeHwnd() );
		SetActiveWindow( m_pBar->GetSafeHwnd() );
		m_uMRUDockID = dwDockSide;

	} else if ( dctTab == m_nDockType ) {
			
		// add to bar
		CEGDockingBar * pDestBar = m_pDropTarget->OwnerBar();
		while( !pSourceBar->m_lstDockPanes.empty() ) {
			CEGDockingPanesIt it = pSourceBar->m_lstDockPanes.begin();
			//(*it)->m_pNewDockingBar = pDestBar;
			CEGDockingPane * pPane  = (*it);
//			TRACE1( "ADDING PANE(%s)\r\n", pPane->GetTitle() );
			pDestBar->AddPane( (*it) );
//			TRACE1( "REMOVING PANE(%s)\r\n", pPane->GetTitle() );
			pPane->m_pRemovedFromBar = pSourceBar;
			pSourceBar->RemovePane( (*it) );
			pPane->m_pRemovedFromBar = NULL;
			//(*it)->m_pNewDockingBar = nil;
//			TRACE1( "DONE PANE(%s)\r\n", pPane->GetTitle() );
		}
		m_uMRUDockID = pDestBar->GetDockBarID();
			
		// free source bar
		m_pDockSite->PostMessage( DM_DOCKING_BAR, (WPARAM)pSourceBar, 1 );

	} else if ( dctFloat != m_nDockType ) {

		CRect rc;
		m_pDropTarget->OwnerBar()->GetWindowRect( &rc );
		switch( m_nDockType ) {
			case dctBottom:
				rc.top = rc.bottom - 5;
				rc.bottom = rc.top; // + 5;
				break;
			case dctLeft:
				rc.right = rc.left;
				rc.left = rc.right - 5;
				break;
			case dctRight:
				rc.left = rc.right;
				rc.right = rc.left + 5;
				break;
			case dctTop:
				rc.bottom = rc.top;
				rc.top = rc.bottom - 5;
				break;
		};

		CEGDockingBar * pDestBar = m_pDropTarget->OwnerBar();
		CDockBar *pDockBar = (CDockBar*)m_pDockSite->GetControlBar(pDestBar->GetDockBarID() );
		pDockBar->DockControlBar( pSourceBar, &rc);
		RecalcBarSizes( pDockBar );
//		pDestBar->AlignControlBars();
		m_pDockSite->ShowControlBar( m_pBar, TRUE, FALSE );
		SetForegroundWindow( m_pBar->GetSafeHwnd() );
		SetActiveWindow( m_pBar->GetSafeHwnd() );
		m_uMRUDockID = _AfxGetDlgCtrlID(pDockBar->m_hWnd);
	
	} else {
		// just show floating bar
		m_dwMRUFloatStyle = CBRS_ALIGN_LEFT | (m_dwDockStyle & CBRS_FLOAT_MULTI);
		m_ptMRUFloatPos = m_rectFrameDragVert.TopLeft();
		m_pDockSite->FloatControlBar(m_pBar, m_ptMRUFloatPos, m_dwMRUFloatStyle);
		m_pDockSite->ShowControlBar( m_pBar, TRUE, FALSE );
		SetForegroundWindow( m_pBar->GetSafeHwnd() );
		SetActiveWindow( m_pBar->GetSafeHwnd() );
	
		reinterpret_cast<CEGDockingBar*>( m_pBar )->RecalcLocalLayout();
	}
	
}


