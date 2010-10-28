// ToolTipBitmapButton.cpp : implementation file
//

#include "stdafx.h"
#include "ToolTipBitmapButton.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CToolTipBitmapButton

CToolTipBitmapButton::CToolTipBitmapButton()
				:m_bHover(FALSE)
				,m_bTracking(FALSE)
				,m_pressed(false)
				,m_mouse_hover_style(false)
{
}

CToolTipBitmapButton::~CToolTipBitmapButton()
{
}


BEGIN_MESSAGE_MAP(CToolTipBitmapButton, baseCToolTipBitmapButton)
	//{{AFX_MSG_MAP(CToolTipBitmapButton)
		ON_WM_MOUSEMOVE()
		ON_MESSAGE(WM_MOUSELEAVE, OnMouseLeave)
		ON_MESSAGE(WM_MOUSEHOVER, OnMouseHover)
		ON_WM_KEYUP()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CToolTipBitmapButton message handlers

BOOL CToolTipBitmapButton::PreTranslateMessage(MSG* pMsg) 
{
	// TODO: Add your specialized code here and/or call the base class
	
	return baseCToolTipBitmapButton::PreTranslateMessage(pMsg);
}

void CToolTipBitmapButton::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct) 
{
	// TODO: Add your code to draw the specified item

	CDC * pDC = CDC::FromHandle(lpDrawItemStruct->hDC);

	CDC * pMemDC = new CDC;
	pMemDC->CreateCompatibleDC(pDC);



	CBitmap * pOldBitmap;
	pOldBitmap = pMemDC->SelectObject(&m_Bitmap);
	
	CPoint point(0,0);
	
	if (IsWindowEnabled()) {	
		if ( (lpDrawItemStruct->itemState & ODS_SELECTED) || m_pressed)
		{
			//pDC->BitBlt(0, 0, m_ButtonSize.cx, m_ButtonSize.cy, pMemDC, m_ButtonSize.cx, 0, SRCCOPY);
			pDC->TransparentBlt(0, 0, m_ButtonSize.cx, m_ButtonSize.cy, pMemDC, 
				m_ButtonSize.cx, 0,m_ButtonSize.cx, m_ButtonSize.cy,RGB(255,255,0));
		} else {
			if (m_bHover) // focused ?
			{
				//pDC->BitBlt(0, 0, m_ButtonSize.cx, m_ButtonSize.cy, pMemDC, m_ButtonSize.cx*2, 0, SRCCOPY);
				pDC->TransparentBlt(0, 0, m_ButtonSize.cx, m_ButtonSize.cy, pMemDC, 
					m_ButtonSize.cx*2, 0,m_ButtonSize.cx, m_ButtonSize.cy,RGB(255,255,0));
			} else {
				//pDC->BitBlt(0,0,m_ButtonSize.cx,m_ButtonSize.cy,pMemDC,0,0,SRCCOPY);
				pDC->TransparentBlt(0, 0, m_ButtonSize.cx, m_ButtonSize.cy, pMemDC, 
					0, 0,m_ButtonSize.cx, m_ButtonSize.cy,RGB(255,255,0));
			}
		}
	} else {
		//pDC->BitBlt(0, 0, m_ButtonSize.cx, m_ButtonSize.cy, pMemDC, m_ButtonSize.cx*3, 0, SRCCOPY);
		pDC->TransparentBlt(0, 0, m_ButtonSize.cx, m_ButtonSize.cy, pMemDC, 
			m_ButtonSize.cx*3, 0,m_ButtonSize.cx, m_ButtonSize.cy,RGB(255,255,0));
	}
	// clean up
	pMemDC -> SelectObject(pOldBitmap);
	delete pMemDC;
}


// Load a bitmap from the resources in the button, the bitmap has to have 3 buttonsstates next to each other: Up/Down/Hover
BOOL CToolTipBitmapButton::LoadBitmap(HANDLE hbitmap)
{
	m_Bitmap.Attach(hbitmap);
	BITMAP	bitmapbits;
	m_Bitmap.GetBitmap(&bitmapbits);
	m_ButtonSize.cy=bitmapbits.bmHeight;
	m_ButtonSize.cx=bitmapbits.bmWidth/4; // up, down, focused, disabled
	OnSize(0,m_ButtonSize.cx,m_ButtonSize.cy);
	return TRUE;
}


void CToolTipBitmapButton::OnMouseMove(UINT nFlags, CPoint point) 
{
	//	TODO: Add your message handler code here and/or call default

	if (m_mouse_hover_style && !m_bTracking)
	{
		TRACKMOUSEEVENT tme;
		tme.cbSize = sizeof(tme);
		tme.hwndTrack = m_hWnd;
		tme.dwFlags = TME_LEAVE|TME_HOVER;
		tme.dwHoverTime = 1;
		m_bTracking = _TrackMouseEvent(&tme);
	}
	baseCToolTipBitmapButton::OnMouseMove(nFlags, point);
}

LRESULT CToolTipBitmapButton::OnMouseHover(WPARAM wparam, LPARAM lparam) 
{
	// TODO: Add your message handler code here and/or call default
	m_bHover = TRUE;
	Invalidate();
	return 0;
}


LRESULT CToolTipBitmapButton::OnMouseLeave(WPARAM wparam, LPARAM lparam)
{
	m_bTracking = FALSE;
	m_bHover=FALSE;
	Invalidate();
	return 0;
}

void CToolTipBitmapButton::OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags) 
{
	if (nChar==VK_RETURN || nChar==VK_ESCAPE)
		GetParent()->SendMessage(WM_CHAR,nChar,0);
	else
	{
		GetParent()->SendMessage(WM_CHAR,nChar,0);
        baseCToolTipBitmapButton::OnKeyUp(nChar, nRepCnt, nFlags);
	}
}
