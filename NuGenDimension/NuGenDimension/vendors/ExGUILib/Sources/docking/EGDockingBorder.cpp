#include "StdAfx.h"
#include "EGDockingPane.h"
#include "EGDockingBorder.h"
#include "EGMenu.h"

/////////////////////////////////////////////////////////////////////////////
// CEGDockBorder

#define BORDER_ICON 24
#define BORDER_SPLITTER 16
#define OUT_OF_FRAME_EVENT 2
bool operator== ( CEGBorderButton & obj1, CEGBorderButton & obj2 ) {
	return obj1.m_pPane == obj2.m_pPane;
}

CEGDockBorder::CEGDockBorder()
{
	m_Height = 24;
	m_pImageList = NULL;
	m_Font.CreateFont(24 - 10 ,0, 0,0, FW_NORMAL, 0,0,0, DEFAULT_CHARSET,
		OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
		DEFAULT_QUALITY, FF_ROMAN , "Arial");
	m_VertFont.CreateFont(24 - 10 ,0, 900,0, FW_NORMAL, 0,0,0, DEFAULT_CHARSET,
		OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
		DEFAULT_QUALITY, FF_ROMAN , "Arial");
	
	m_pFlyOutPane = NULL;
	m_pLastFlyOutPane = NULL;
	m_pPaneForRestore = NULL;
	m_bForcedFlyOut = TRUE;
}

CEGDockBorder::~CEGDockBorder()
{
	CEGBorderButtonsIt it = m_lstButtons.begin(),
			itLast = m_lstButtons.end();
	for ( ; it != itLast; ++it ) 
		delete (*it);
	m_lstButtons.clear();
}



void CEGDockBorder::SetGroupSize( CEGBorderButtons * pButtons, int nMaxSize, LPRECT lprcBorder, int * px, int * py ) {
	
	CEGBorderButtons::iterator it = pButtons->begin(),
		itLast = pButtons->end();

	for ( ; it != itLast; ++it ) {

		int nSize = ( (*it)->m_nStyle & (DRAWBTNSTYLE_SELECT) ) ? nMaxSize : BORDER_ICON;

		switch(m_Style){
			case ALIGN_TOP:
				SetRect( (*it)->m_rcButton, *px, lprcBorder->top, *px + nSize, lprcBorder->bottom - 4 );
				*px += nSize;
				break;

			case ALIGN_LEFT:
				SetRect( (*it)->m_rcButton, lprcBorder->left, *py, lprcBorder->right - 4, *py + nSize);
				*py += nSize;
				break;

			case ALIGN_RIGHT:
				SetRect( (*it)->m_rcButton, lprcBorder->left + 4, *py, lprcBorder->right, *py + nSize);
				*py += nSize;
				break;

			case ALIGN_BOTTOM:
				SetRect( (*it)->m_rcButton, *px, lprcBorder->top + 4, *px + nSize, lprcBorder->bottom);
				*px += nSize;
				break;
		}
	}
}

#ifdef _DEBUG
void CEGDockBorder::DumpButtons( )
{
	TRACE0( "\r\nButtons:\r\n");
	CEGBorderButtonsIt it = m_lstButtons.begin(),
		itLast = m_lstButtons.end();
	for( ; it != itLast;  ++it ) {
		if( (*it)->m_nStyle & DRAWBTNSTYLE_SEP) {
			TRACE0( "---\r\n");
		} else {
			TRACE1( "%s\r\n", (*it)->m_pPane->GetTitle() );
		}
	}	
	TRACE0( "\r\n");
}
#endif

void CEGDockBorder::CalcLayout()
{
	// Начало отсчета
	CRect rcBorder;
	GetClientRect( &rcBorder );
	
	int x = rcBorder.left + 2;
	int y = rcBorder.top + 2;

	// Подбор шрифта
	CDC * pDC= GetDC();
	if(m_Style == ALIGN_TOP || m_Style == ALIGN_BOTTOM)
		pDC->SelectObject(&m_Font);
	else
		pDC->SelectObject(&m_VertFont);

	// Вычисление координат кнопок
	int nSize, nMaxSize = 0;

	
	CEGBorderButtons lstGroup;
	CEGBorderButtonsIt it = m_lstButtons.begin(),
		itLast = m_lstButtons.end();
	
	for( ; it != itLast;  ++it ) {

		// Для разделителя смещаем положение и устанавливаем размер кнопок предыдущей группы
		if( (*it)->m_nStyle & DRAWBTNSTYLE_SEP) {

				if( !lstGroup.empty() ) {
					SetGroupSize( &lstGroup, nMaxSize, &rcBorder, &x, &y );
					lstGroup.clear();
				}

				x += BORDER_SPLITTER;
				y += BORDER_SPLITTER;

				nMaxSize = 0;
		} else {
			// вычисляем максимальную длину кнопки в группе
			// nSize = pDC->GetTextExtent((*it)->m_pPane->GetTitle()).cx + BORDER_ICON + BORDER_ICON;
			nSize = themeData.MeasureTab( pDC, TRUE, (*it)->m_pPane->GetTitle(), m_Style, STYLE_FLAT | STYLE_ACTIVE );
			if( nSize > nMaxSize )	
				nMaxSize = nSize;
			
			lstGroup.push_back( (*it) );
		}
	}
	if( !lstGroup.empty() )
		SetGroupSize( &lstGroup, nMaxSize, &rcBorder, &x, &y );

	ReleaseDC(pDC);
}

BOOL CEGDockBorder::Create(int Style, CWnd * pWnd)
{
	m_Style = Style;
	CRect rt(10, 10, 10, 10);

	if( !CWnd::Create(
		AfxRegisterWndClass( CS_VREDRAW | CS_HREDRAW, ::LoadCursor(NULL, IDC_ARROW), (HBRUSH) ::GetStockObject(WHITE_BRUSH), ::LoadIcon(NULL, IDI_APPLICATION) ),
		"DockBorder",  WS_CHILD , rt, pWnd, 233 + Style) )
		return FALSE;

	if( !m_wndFlyOut.Create( pWnd, rt, Style ) )
		return FALSE;

	m_wndFlyOut.SetBorder( this );

	return TRUE;
}

void CEGDockBorder::Draw(CDC * pDC)
{

	if ( m_lstButtons.empty() )
		return;

	// Рисуем фон
	CRect rt;
	GetClientRect(&rt);

	CEGBorderButtonsIt it, itFirst = m_lstButtons.begin(),
		itLast = m_lstButtons.end();

	COLORREF clrActive = themeData.clrBtnFace;
	for( it = itFirst; it != itLast; ++it ) {
		if( (*it)->m_nStyle & (DRAWBTNSTYLE_SELECT) ) {
			clrActive = (*it)->m_pPane->Color();
			break;
		}
	}
	themeData.DrawTabCtrlBK( pDC, &rt, m_Style, FALSE, clrActive );

	// Рисуем кнопки
	for( it = itFirst; it != itLast; ++it ) {
		
		int nStyle = (*it)->m_nStyle;
		if( nStyle & DRAWBTNSTYLE_SEP )
				continue;

		CRect * prcButton = &(*it)->m_rcButton;

		if ( nStyle & (DRAWBTNSTYLE_BTN | DRAWBTNSTYLE_SELECT) ) {
			themeData.DrawTab( pDC, prcButton, (*it)->m_pPane->GetIcon(), (*it)->m_pPane->GetTitle(), m_Style, STYLE_FLAT | STYLE_ACTIVE, (*it)->m_pPane->Color() );
		} else if ( nStyle & (DRAWBTNSTYLE_BTN |DRAWBTNSTYLE_GROUP) ) {
			themeData.DrawTab( pDC, prcButton, (*it)->m_pPane->GetIcon(), NULL, m_Style, STYLE_FLAT | STYLE_ACTIVE, (*it)->m_pPane->Color() );
		}
	}
}

void CEGDockBorder::RemoveButton(const CEGDockingPane * pPane)
{
	CEGBorderButtonsIt it, itPrior, itFirst = m_lstButtons.begin(),
		itLast = m_lstButtons.end();

	CEGDockingBar * pBar = NULL;
	for ( it = itFirst; it != itLast ; ++it ) {
		if ( (*it)->m_pPane == pPane ) {
			pBar = (*it)->m_pPane->OwnerBar();
			delete (*it);

			// 1. Deleting next separator
			CEGBorderButtonsIt itNext = it;
			if( (++itNext) != itLast && (*itNext)->m_nStyle & DRAWBTNSTYLE_SEP ) {
				// only when next button is separator and ( prev button is first button or separator )
				itPrior = it;
				if ( it == itFirst || ( (*--itPrior )->m_nStyle & DRAWBTNSTYLE_SEP ) ) {
					delete (*itNext);
					m_lstButtons.erase( itNext );
				}
			}
			m_lstButtons.erase( it );
			break;
		}
	}

	// Activate first group button
	it = m_lstButtons.begin();
	itLast = m_lstButtons.end();
	for ( ; it != itLast ; ++it ) {
		if( (*it)->m_pPane && (*it)->m_pPane->OwnerBar() == pBar ) {
			(*it)->m_nStyle = DRAWBTNSTYLE_BTN | DRAWBTNSTYLE_SELECT | DRAWBTNSTYLE_GROUP;
			break;
		}
	}
	

	// delete last splitter
	if ( m_lstButtons.size() == 1 ) {
			it = m_lstButtons.begin();
			delete ( *it );
			m_lstButtons.erase( it );
	}

#ifdef _DEBUG
	DumpButtons();
#endif
	CalcLayout();
	RedrawWindow();
}



void CEGDockBorder::AddButton( int nStyle, CEGDockingPane * pPane )
{
	if ( nStyle & DRAWBTNSTYLE_SEP && m_lstButtons.empty() )
		return ; // separator at begin don't needed

	if ( nStyle & DRAWBTNSTYLE_SEP && !m_lstButtons.empty() && m_lstButtons.back()->m_nStyle & DRAWBTNSTYLE_SEP  )
		return ; // double separator don't needed

	BOOL bIsGrooupFinded = FALSE;

	if ( pPane ) {
		CEGDockingBar * pOwner = pPane->OwnerBar();

		CEGBorderButtonsIt it, itFirst = m_lstButtons.begin(),
			itLast = m_lstButtons.end();
		
		BOOL bNeedInsert = FALSE;
		for ( it = itFirst; it != itLast; ++it ) {
			if ( (*it)->m_pPane && (*it)->m_pPane->OwnerBar() == pOwner && !bIsGrooupFinded ) {
				bIsGrooupFinded = TRUE;
				bNeedInsert = TRUE;
			} else if ( bIsGrooupFinded && ( !(*it)->m_pPane || (*it)->m_pPane->GetOwner() != pOwner ) ) {
				break;
			}
		}
		if ( bNeedInsert ) { // initiali selected
			if ( it == itLast )
				bNeedInsert = FALSE;
			if ( nStyle & DRAWBTNSTYLE_SELECT ) {
				// if last pane of group is active then deactivate other panels of group
				CEGBorderButtonsIt itBtn = it;
				do {
					--itBtn;
					if ( !(*itBtn)->m_pPane || (*itBtn)->m_pPane->GetOwner() != pOwner ) break;
					(*itBtn)->m_nStyle &= ~DRAWBTNSTYLE_SELECT;
				} while( itBtn != itFirst );
			}
			
			if ( bNeedInsert )
				m_lstButtons.insert( it, new CEGBorderButton( nStyle, pPane ) );
			else {
				m_lstButtons.push_back( new CEGBorderButton( nStyle, pPane ) );
				if ( !(nStyle & DRAWBTNSTYLE_SEP ) )
					m_lstButtons.push_back( new CEGBorderButton( DRAWBTNSTYLE_SEP, NULL ) );
			}
		}
	}
	if ( !bIsGrooupFinded ) {// initialli selected 
		m_lstButtons.push_back( new CEGBorderButton( nStyle | DRAWBTNSTYLE_SELECT, pPane ) );
		if ( !(nStyle & DRAWBTNSTYLE_SEP ) )
			m_lstButtons.push_back( new CEGBorderButton( DRAWBTNSTYLE_SEP, NULL ) );
	}

#ifdef _DEBUG
	DumpButtons();
#endif

	CalcLayout();
	RedrawWindow();
}

BEGIN_MESSAGE_MAP(CEGDockBorder, CWnd)
	ON_WM_CREATE()
	ON_WM_DESTROY()
	ON_WM_TIMER()
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONDOWN()
	ON_MESSAGE(WM_SIZEPARENT, OnSizeParent)
//	ON_MESSAGE( WM_MOUSELEAVE, OnMouseLeave )
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CEGDockBorder message handlers
BOOL CEGDockBorder::NeedSizing()
{
	if( m_lstButtons.empty() ) {
		ShowWindow( SW_HIDE );
		return FALSE;
	} else {
		ShowWindow( SW_SHOW );
	}
	return TRUE;
}

LRESULT CEGDockBorder::OnSizeParent(WPARAM, LPARAM lp)
{
	if ( !NeedSizing() )
		return 0L;

	AFX_SIZEPARENTPARAMS* pParams = reinterpret_cast<AFX_SIZEPARENTPARAMS*>(lp);

	const int height = m_Height;

	int rt_height = pParams->rect.bottom - pParams->rect.top;
	int rt_width  = pParams->rect.right - pParams->rect.left;

	switch(m_Style)
    {
		case ALIGN_LEFT:
			pParams->rect.left += height;
			MoveWindow(pParams->rect.left - height, pParams->rect.top, height, rt_height, true);
			pParams->sizeTotal.cx -= height;
			break;

		case ALIGN_RIGHT:
			pParams->rect.right -= height;
			MoveWindow(pParams->rect.right , pParams->rect.top, height, rt_height, true);
			pParams->sizeTotal.cx -= height;
			break;

		case ALIGN_TOP:
			pParams->rect.top += height;
			MoveWindow(pParams->rect.left, pParams->rect.top - height, rt_width, height, true);
			pParams->sizeTotal.cy -= height;
			break;

		case ALIGN_BOTTOM:
			pParams->rect.bottom -= height;
			MoveWindow(pParams->rect.left, pParams->rect.bottom, rt_width, height, true);
			pParams->sizeTotal.cy -= height;
			break;

    }
	
	ShowWindow(SW_NORMAL);
	return 0;
}

void CEGDockBorder::OnPaint() 
{
	CPaintDC dc(this);
	
	Draw(&dc);
	// Do not call CWnd::OnPaint() for painting messages
}

void CEGDockBorder::OnSize(UINT nType, int cx, int cy) 
{
	CWnd::OnSize(nType, cx, cy);
	
	SetRect( m_rcFlyOut, 0, 0, cx, cy );

	CalcLayout();	
}

void CEGDockBorder::OnMouseMove(UINT nFlags, CPoint point) {

	CEGBorderButtonsIt it = m_lstButtons.begin(),
		itLast = m_lstButtons.end();

	BOOL bFocusLost = TRUE;

	CRect rcClient;
	GetClientRect( &rcClient );

	for( ; it != itLast; ++it ) {

		if( (*it)->m_nStyle & DRAWBTNSTYLE_SEP )
			continue;

		CRect rcButtonFull = (*it)->m_rcButton;
		switch ( m_Style ) {
			case ALIGN_TOP:
				rcButtonFull.bottom = rcButtonFull.top + rcClient.Height();
				break;

			case ALIGN_LEFT:
				rcButtonFull.right = rcButtonFull.left + rcClient.Height();
				break;

			case ALIGN_RIGHT:
				rcButtonFull.left = rcButtonFull.right - rcClient.Width();
				break;

			case ALIGN_BOTTOM:
				rcButtonFull.top = rcButtonFull.bottom - rcClient.Height();
				break;
		}

		if( !rcButtonFull.PtInRect(point))
			continue;

		bFocusLost = FALSE;

		if( m_pFlyOutPane != (*it)->m_pPane )
			ShowPaneEx( (*it)->m_pPane );

		break;
		
	}
	
	if ( m_pFlyOutPane && bFocusLost && !m_bForcedFlyOut ) {
		KillTimer( OUT_OF_FRAME_EVENT );
		m_wndFlyOut.SlideHide( );
	}

	CWnd::OnMouseMove(nFlags, point);
}

void CEGDockBorder::OnLButtonDown( UINT nFlags, CPoint point ) {

	CEGBorderButtonsIt it = m_lstButtons.begin(),
		itLast = m_lstButtons.end();

	for( ; it != itLast; ++it ) {

		if( (*it)->m_nStyle & DRAWBTNSTYLE_SEP )
			continue;

		if( !(*it)->m_rcButton.PtInRect(point))
			continue;
		
		if( m_pFlyOutPane != (*it)->m_pPane )
			ShowPaneEx( (*it)->m_pPane, 0 );
		else if ( !m_bForcedFlyOut ) {
			m_bForcedFlyOut = TRUE;
			::SetForegroundWindow( m_pFlyOutPane->GetSafeHwnd() );
			::SetFocus( m_pFlyOutPane->GetSafeHwnd() );
			m_wndFlyOut.SendMessage( WM_FOCUS_CHANGED, 0, 0 );
		}

		break;
		
	}

	CWnd::OnLButtonDown(nFlags, point);
}

void CEGDockBorder::ShowPaneEx( CEGDockingPane * pPane, int nDelay ) {
	if( pPane == m_pLastFlyOutPane && 0 != nDelay )
		return;
  
	// restoring pane state
	if ( m_pLastFlyOutPane )
		m_pPaneForRestore = m_pLastFlyOutPane;

	m_pLastFlyOutPane = pPane;
	if ( m_nTimer ) {
		KillTimer( m_nTimer );
		m_nTimer = 0;
	}
	if ( 0 == nDelay ) {
		ShowPane( nDelay != 0 );
	} else {
		m_nTimer = (UINT) SetTimer( 1, 500, NULL );
	}
}

void CEGDockBorder::ShowPane( BOOL bForced ) {
	
	CPoint point;
	GetCursorPos( &point );
	ScreenToClient( &point );

	CEGBorderButtonsIt it = m_lstButtons.begin(),
		itLast = m_lstButtons.end();

	for( ; it != itLast; ++it ) {

		if( (*it)->m_nStyle & DRAWBTNSTYLE_SEP )
			continue;

		if( !(*it)->m_rcButton.PtInRect(point))
			continue;
		
		if( m_pFlyOutPane != (*it)->m_pPane ) {
			
			m_pFlyOutPane = (*it)->m_pPane;

			// дополнительные действия для групп
			if( (*it)->m_nStyle & DRAWBTNSTYLE_GROUP) {

				// активация текущей кнопки
				(*it)->m_nStyle |= DRAWBTNSTYLE_SELECT;
				
				// деактивацию предыдущих
				CEGBorderButtonsIt itGroup = it, 
					itFirst = m_lstButtons.begin();
				while( itGroup != itFirst ) {
					--itGroup;
					if ( (*itGroup)->m_nStyle & DRAWBTNSTYLE_SEP) 
						break;
					(*itGroup)->m_nStyle &= ~DRAWBTNSTYLE_SELECT;
				}
				// деактивация последующих
				itGroup = it;
				while( ++itGroup != itLast ) {
					if ( (*itGroup)->m_nStyle & DRAWBTNSTYLE_SEP) 
							break;
					(*itGroup)->m_nStyle &= ~DRAWBTNSTYLE_SELECT;
				}
			}
			// ну и пересчитать раскладку,
			CalcLayout();
			// а затем перерисовать то что получилось
			Invalidate();
		}
		FlyOut( m_pFlyOutPane, bForced );

		break;
	}
}

void CEGDockBorder::FlyDownPane( CEGDockingPane* pPane ) {
	if ( m_pFlyOutPane == pPane ) {
		m_wndFlyOut.ShowWindow( SW_HIDE );
		OnHideFlyOut();
	}
}

void CEGDockBorder::FlyOutPane( CEGDockingPane* pPane ) {
	
	CEGBorderButtonsIt it = m_lstButtons.begin(),
		itLast = m_lstButtons.end();

	for( ; it != itLast; ++it ) {

		if( (*it)->m_nStyle & DRAWBTNSTYLE_SEP )
			continue;

		if( (*it)->m_pPane != pPane )
			continue;
		
		if( m_pFlyOutPane != (*it)->m_pPane ) {
			
			m_pFlyOutPane = (*it)->m_pPane;

			// дополнительные действия для групп
			if( (*it)->m_nStyle & DRAWBTNSTYLE_GROUP) {

				// активация текущей кнопки
				(*it)->m_nStyle |= DRAWBTNSTYLE_SELECT;
				
				// деактивацию предыдущих
				CEGBorderButtonsIt itGroup = it, 
					itFirst = m_lstButtons.begin();
				while( itGroup != itFirst ) {
					--itGroup;
					if ( (*itGroup)->m_nStyle & DRAWBTNSTYLE_SEP) 
						break;
					(*itGroup)->m_nStyle &= ~DRAWBTNSTYLE_SELECT;
				}
				// деактивация последующих
				itGroup = it;
				while( ++itGroup != itLast ) {
					if ( (*itGroup)->m_nStyle & DRAWBTNSTYLE_SEP) 
							break;
					(*itGroup)->m_nStyle &= ~DRAWBTNSTYLE_SELECT;
				}
			}
			// ну и пересчитать раскладку,
			CalcLayout();
			// а затем перерисовать то что получилось
			Invalidate();
		}
		FlyOut( m_pFlyOutPane, TRUE );

		break;
	}
}

void CEGDockBorder::FlyOut( CEGDockingPane* pPane, BOOL bForced  ) {
	
	ASSERT( pPane != NULL );
	
	CRect rcFlyOut; 
	GetWindowRect( rcFlyOut );
	GetParent()->ScreenToClient( rcFlyOut );

	switch(m_Style){
		case ALIGN_TOP:
			rcFlyOut.top = rcFlyOut.bottom;
			rcFlyOut.bottom = rcFlyOut.top + pPane->Height();
			break;

		case ALIGN_LEFT:
			rcFlyOut.left = rcFlyOut.right;
			rcFlyOut.right = rcFlyOut.left + pPane->Width();
			break;

		case ALIGN_RIGHT:
			rcFlyOut.right = rcFlyOut.left;
			rcFlyOut.left = rcFlyOut.right - pPane->Width();
			break;

		case ALIGN_BOTTOM:
			rcFlyOut.bottom = rcFlyOut.top;
			rcFlyOut.top = rcFlyOut.bottom - pPane->Height();
			break;
	}

	if ( m_pPaneForRestore )
		m_pPaneForRestore->RestoreParent();
	m_wndFlyOut.SetPane( pPane );

	m_bForcedFlyOut = bForced;
	
	KillTimer( OUT_OF_FRAME_EVENT );
	if( !bForced ) {
		// Активируем трэкинг события выхода WM_NCMOUSELEAVE
		SetTimer ( OUT_OF_FRAME_EVENT, 500, NULL );
	}

	m_wndFlyOut.PostMessage(  bForced ? WM_SHOWIT : WM_SHOWIT_NOFOCUS, 
		MAKEWPARAM( rcFlyOut.left, rcFlyOut.top ), 
		MAKELPARAM( rcFlyOut.right, rcFlyOut.bottom ) );

}

void CEGDockBorder::OnTimer( UINT_PTR nIDEvent ) {
	if ( 1 == nIDEvent ) {
		KillTimer( m_nTimer );
		m_nTimer = 0;
		ShowPane( FALSE );
	} else if ( OUT_OF_FRAME_EVENT == nIDEvent ) {
		CPoint pt;
		::GetCursorPos( &pt );

		CRect rcFlyOut, rcBorder;
		m_wndFlyOut.GetWindowRect( &rcFlyOut );
		GetWindowRect( rcBorder );
		if ( !rcBorder.PtInRect( pt ) && ! rcFlyOut.PtInRect( pt ) ) {
			HWND hActiveWnd = ::GetFocus();
			if ( hActiveWnd != m_wndFlyOut.GetSafeHwnd() && !::IsChild( m_wndFlyOut.GetSafeHwnd(), hActiveWnd ) ) {
				KillTimer( 2 );
				m_wndFlyOut.SlideHide( );
			}
		}
	}
}


void CEGDockBorder::OnDestroy() {
	if ( m_nTimer != 0)
		KillTimer( m_nTimer );
}

void CEGDockBorder::OnHideFlyOut() {
	ASSERT( m_pFlyOutPane != NULL );
	
	m_pFlyOutPane->RestoreParent();
	m_pFlyOutPane = NULL;
	m_pLastFlyOutPane = NULL;
	
	NeedSizing();
}

void CEGDockBorder::FlyDownBar( BOOL bShow ) {
	ASSERT( m_pFlyOutPane != NULL );

	m_pFlyOutPane->RemoveFromBorder( this, bShow );
	m_pFlyOutPane = NULL;
	m_pLastFlyOutPane = NULL;
}
