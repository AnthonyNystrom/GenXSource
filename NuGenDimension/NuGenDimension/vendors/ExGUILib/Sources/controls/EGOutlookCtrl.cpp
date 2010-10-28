// Outlook2Ctrl.cpp : implementation file
//

#include "stdafx.h"
#include "EGButtonsBar.h"
#include "EGMenu.h"

// CEGButtonsBar

	

void CEGButtonsBar::DrawGradientRect(CDC * pDC, CRect &rect, COLORREF cr1, COLORREF cr2, DWORD dwStyle)
{
#if(WINVER >= 0x0500)
	TRIVERTEX        vert[2] ;
	GRADIENT_RECT    gRect;
	vert [0] .x      = rect.left;
	vert [0] .y      = rect.top;
	vert [0] .Red    = GetRValue(cr1)*0xFF00/255;
	vert [0] .Green  = GetGValue(cr1)*0xFF00/255;
	vert [0] .Blue   = GetBValue(cr1)*0xFF00/255;
	vert [0] .Alpha  = 0;

	vert [1] .x      = rect.right;
	vert [1] .y      = rect.bottom; 
	vert [1] .Red    = GetRValue(cr2)*0xFF00/255;
	vert [1] .Green  = GetGValue(cr2)*0xFF00/255;
	vert [1] .Blue   = GetBValue(cr2)*0xFF00/255;
	vert [1] .Alpha  = 0x0000;

	gRect.UpperLeft  = 0;
	gRect.LowerRight = 1;

	GradientFill(pDC->GetSafeHdc(),vert,2,&gRect,1,dwStyle);
#else
	pDC->FillSolidRect(rect, cr1);
#endif
}


IMPLEMENT_DYNAMIC( CEGButtonsBar, CControlBar )
CEGButtonsBar::CEGButtonsBar()
{
	m_iSize = 200;//AfxGetApp()->GetProfileInt("Settings","OutbarSize",200);
	m_iDragging = 0;
	m_iDragoffset = 0;
	hDragCur = AfxGetApp()->LoadCursor(AFX_IDC_HSPLITBAR); // sometime fails .. 
	if (!hDragCur) hDragCur = AfxGetApp()->LoadStandardCursor(MAKEINTRESOURCE(IDC_SIZEWE)); 

#if(WINVER >= 0x0500)
	hHandCur = LoadCursor(NULL, IDC_HAND);
#else
	hHandCur = LoadCursor(NULL, IDC_ARROW);
#endif

	LOGFONT lf;
	HFONT hf = (HFONT) GetStockObject(DEFAULT_GUI_FONT);
	CFont * gf = CFont::FromHandle(hf);
	gf->GetLogFont(&lf);
	lf.lfUnderline = TRUE;
	ftHotItems.CreateFontIndirect(&lf);
	lf.lfUnderline = FALSE;

	ftItems.CreateFontIndirect(&lf);

	lf.lfWeight = FW_SEMIBOLD;
	ftFolders.CreateFontIndirect(&lf);

	lf.lfHeight = 20;
	ftCaption.CreateFontIndirect(&lf);


	m_csCaption = "";

	 
	SetupColors();

	m_iNumFoldersDisplayed = -1;
	m_iFolderHeight = 24;
	m_iItemHeight = 18;
	m_iSelectedFolder = 0;
	m_iSubItemHeight = 17;

	iHiFolder = iHiLink = -1;
	pLastHilink = NULL;
}

CEGButtonsBar::~CEGButtonsBar()
{
	for (int t = 0; t < m_Folders.GetSize(); t++)
	{
		CEGButtonsFolder * p = (CEGButtonsFolder *) m_Folders.GetAt(t);
		delete p;
	}
	m_Folders.RemoveAll();
	//AfxGetApp()->WriteProfileInt("Settings","OutbarSize",m_iSize);
}

void CEGButtonsBar::SetupColors()
{
	/*
	// classical 
	m_crBackCaption = GetSysColor(COLOR_BTNSHADOW);
	m_crTextCaption = GetSysColor(COLOR_WINDOW);
	m_crCmdOther = GetSysColor(COLOR_WINDOW);
	m_crDisabled = GetSysColor(COLOR_GRAYTEXT);

//	m_crBackground = GetSysColor(COLOR_BTNSHADOW);
	m_crBackground = GetSysColor(COLOR_BTNSHADOW);
	m_crCmdLink = GetSysColor(COLOR_CAPTIONTEXT);
*/

	// modern

	m_crBackCaption = GetSysColor(COLOR_BTNSHADOW);
	m_crTextCaption = GetSysColor(COLOR_WINDOW);
	m_crCmdOther = GetSysColor(COLOR_BTNTEXT);
	m_crDisabled = GetSysColor(COLOR_GRAYTEXT);

	m_crBackground = GetSysColor(COLOR_WINDOW);
#if(WINVER >= 0x0500)
	m_crCmdLink = GetSysColor(COLOR_HOTLIGHT);
#else
	m_crCmdLink = GetSysColor(COLOR_BTNHIGHLIGHT);
#endif /* WINVER >= 0x0400 */

}


BEGIN_MESSAGE_MAP(CEGButtonsBar, CControlBar)
	ON_WM_PAINT()
	ON_WM_ERASEBKGND()
	ON_WM_CREATE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_SETCURSOR()
	ON_WM_SIZE()
	ON_MESSAGE( WM_MOUSELEAVE, OnMouseLeave )
END_MESSAGE_MAP()



// CEGButtonsBar message handlers


bool CEGButtonsBar::Create(CWnd * pParent, int iId)
{
	if (CControlBar::Create(NULL, "", WS_VISIBLE|WS_CHILD, CRect(0,0,0,0), pParent, iId))
	{
		SetOwner(pParent);
		return true;
	}
	return false;
}

void CEGButtonsBar::OnPaint()
{
	ClearRects();
	CPaintDC pdc(this);

	CRect rc;
	GetClientRect(rc);
	if (m_iSize <= 4)
	{
		pdc.FillSolidRect(rc, GetSysColor(COLOR_3DFACE));
		return;
	}
	COMemDC dc(&pdc);
	DrawCaption(&dc, rc);
	CRect rcBdr(rc);
	rc.InflateRect(-1,-1);
	DrawButtons(&dc, rc);
		
	rcInnerRect = rc;
	dc.FillSolidRect(rc, m_crBackground);
	if (m_iSelectedFolder >= 0 && m_iSelectedFolder < m_Folders.GetSize())
	{
		CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt(m_iSelectedFolder);
		DrawItems(&dc, o, rc);
	}

	switch (CEGMenu::GetMenuDrawMode())
	{
	case CEGMenu::STYLE_XP:
	case CEGMenu::STYLE_XP_NOBORDER:
	case CEGMenu::STYLE_XP_2003_NOBORDER:
	case CEGMenu::STYLE_XP_2003:
	case CEGMenu::STYLE_COLORFUL_NOBORDER:
	case CEGMenu::STYLE_COLORFUL:
	case CEGMenu::STYLE_ICY_NOBORDER:
	case CEGMenu::STYLE_ICY:
		dc.Draw3dRect(rcBdr, GetSysColor(COLOR_3DSHADOW), GetSysColor(COLOR_3DSHADOW));
		break;

	default: // NATIVE THEME
		dc.DrawEdge( &rcBdr, EDGE_SUNKEN, BF_ADJUST | BF_RECT );
		//dc.Draw3dRect(rcBdr, GetSysColor(COLOR_3DSHADOW), GetSysColor(COLOR_3DSHADOW));
		break;
	}

}

void CEGButtonsBar::AnimateToFolder(int f)
{
	m_iSelectedFolder = f;
 	pLastHilink = NULL;
	m_csCaption = GetFolder(f)->csName;

	if (m_iSize > 4)
	{

		CRect rc;
		GetClientRect(rc);

		CDC dc;
		CClientDC cdc(this);
		dc.CreateCompatibleDC(&cdc);
		CBitmap bmp;
		bmp.CreateCompatibleBitmap(&cdc, rc.Width(), rc.Height());
		CBitmap * ob = (CBitmap *) dc.SelectObject(&bmp);

		DrawCaption(&dc, rc);
		CRect rcBdr(rc);
		rc.InflateRect(-1,-1);
		DrawButtons(&dc, rc);
		int iBottomInner = rc.bottom;
		dc.Draw3dRect(rcBdr, GetSysColor(COLOR_3DSHADOW), GetSysColor(COLOR_3DHILIGHT));
		rcInnerRect = rc;
		dc.FillSolidRect(rc, m_crBackground);
		if (m_iSelectedFolder >= 0 && m_iSelectedFolder < m_Folders.GetSize())
		{
			CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt(m_iSelectedFolder);
			DrawItems(&dc, o, rc);
		}

		for (int t = iBottomInner - 10; t > 0; t -= (iBottomInner/8))
		{
			cdc.BitBlt(0,t,rc.Width(), iBottomInner - t, &dc, 0,0, SRCCOPY);
			Sleep(30);
		}
		dc.SelectObject(ob);
	}

	Invalidate();
}

BOOL CEGButtonsBar::OnEraseBkgnd(CDC* pDC)
{
	return CControlBar::OnEraseBkgnd(pDC);
}

int CEGButtonsBar::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CControlBar::OnCreate(lpCreateStruct) == -1) 
			return -1;
	
	SetBarStyle (CBRS_LEFT|CBRS_HIDE_INPLACE & ~(CBRS_BORDER_ANY | CBRS_GRIPPER));

	m_bIsMouseInside = FALSE;

	return 0;
}

CSize CEGButtonsBar::CalcFixedLayout (BOOL /*bStretch*/, BOOL /* bHorz */)
{
	CSize size = CSize (m_iSize, 32767);
	return size;
}

void CEGButtonsBar::SetCurFolder(int f, bool bAnimation, BOOL bNotify)
{
	// hide all subwindows
	if (m_iSelectedFolder >= 0 && m_iSelectedFolder != f)
	{
		for (int f = 0; f < m_Folders.GetSize(); f++)
		{
			CEGButtonsFolder * pf = (CEGButtonsFolder *) m_Folders.GetAt(f);
			for (int i = 0; i < pf->m_Items.GetSize(); i++)
			{
				CEGButtonsItem * pi = (CEGButtonsItem *) pf->m_Items.GetAt(i);
				for (int s = 0; s < pi->m_SubItems.GetSize(); s++)
				{
					CEGButtonsSubItem * ps = (CEGButtonsSubItem *) pi->m_SubItems.GetAt(s);
					if (ps->hHostedWnd) ::ShowWindow(ps->hHostedWnd, SW_HIDE);
					ps->rcItem.SetRectEmpty();
				}
			}
		}
	}

	if (m_iSelectedFolder != f)
	{
		if (bAnimation) AnimateToFolder(f);
		else
		{
			m_iSelectedFolder = f;
 			pLastHilink = NULL;
			m_csCaption = GetFolder(f)->csName;
		}

		Invalidate();
	
		if ( bNotify ) {
			UINT nCmd = GetFolder(f)->nCommandID;
			if ( nCmd > 0 )
				AfxGetMainWnd()->SendMessage( WM_COMMAND, MAKEWPARAM( nCmd, 0 ), 0 );
		}
	}
}

void CEGButtonsBar::OnLButtonDown(UINT nFlags, CPoint point)
{
	
	if (m_iSize <= 4 || m_dragRect.PtInRect(point))
	{
		m_iDragoffset = m_iSize - point.x;

		m_iDragging = 1;
		SetCursor(hDragCur);
		SetCapture();
		SetFocus();
		CRect rc;
		GetClientRect(rc);
		OnInvertTracker(m_dragRect);
		return;
	}
	int f,i,s;
	int r = HitTest(f,i,s,point);
	if (r == 1)
	{
		SetCurFolder(f, true, TRUE );
	}
	if (r == 4)
	{
		CEGButtonsSubItem	* ps = GetSubItem(f,i,s);
		if (ps->dwStyle == 2 || ps->dwStyle == 3) // item is hotlinked or checkbox
		{
			AfxGetMainWnd()->SendMessage(WM_COMMAND, MAKELONG(ps->lParam, 0), (LPARAM) GetSafeHwnd());
			InvalidateRect(rcInnerRect, FALSE);
		}
		if (ps->dwStyle == 0 || ps->dwStyle == 1) // item is radio or singlelist .. check before sending
		{
			CEGButtonsCCmdUI pui;
			pui.pSI = ps;
			pui.m_nID = ps->lParam; 
			AfxGetMainWnd()->OnCmdMsg(pui.m_nID, CN_UPDATE_COMMAND_UI, &pui, NULL);
			if (!(pui.iRes & 2)) // not checked yet .. so let's do it
				AfxGetMainWnd()->SendMessage(WM_COMMAND, MAKELONG(ps->lParam, 0), (LPARAM) GetSafeHwnd());
			InvalidateRect(rcInnerRect, FALSE);
		}
	}
	CControlBar::OnLButtonDown(nFlags, point);
}

void CEGButtonsBar::OnLButtonUp(UINT nFlags, CPoint point)
{
	if (m_iDragging)
	{
		ReleaseCapture();
		OnInvertTracker(m_dragRect);
		m_iSize = point.x + m_iDragoffset;
		if (m_iSize < 4) m_iSize = 4;
		CFrameWnd* pParentFrame = GetParentFrame ();
		pParentFrame->RecalcLayout ();
	}
	m_iDragging = 0;

	CControlBar::OnLButtonUp(nFlags, point);
}

void CEGButtonsBar::OnMouseMove(UINT nFlags, CPoint point)
{
	if ( !m_bIsMouseInside ) {
		m_bIsMouseInside = TRUE;

		TRACKMOUSEEVENT tme;
		memset( (LPVOID)&tme, 0, sizeof( TRACKMOUSEEVENT ) );
		tme.cbSize = sizeof( TRACKMOUSEEVENT );
		tme.hwndTrack = m_hWnd;
		tme.dwFlags = TME_LEAVE;
		TrackMouseEvent( &tme ); 
	}

	if (m_iDragging == 1)
	{
		CRect rc1(m_dragRect);
		m_dragRect.SetRect(point.x-5 + m_iDragoffset, rc1.top, point.x + m_iDragoffset, rc1.bottom);
		if (rc1 != m_dragRect)
		{
			OnInvertTracker(rc1);
			OnInvertTracker(m_dragRect);
		}
	}
	else
	{
		int f,i,s;
		int r = HitTest(f,i,s,point);
		if (r == 1)
		{
			CClientDC dc(this); 
			if (iHiFolder >= 0 && iHiFolder != f)
			{
				DrawButton(&dc, (CEGButtonsFolder *) m_Folders.GetAt(iHiFolder), iHiFolder == m_iSelectedFolder, false);
			}
			if (iHiFolder != f)
			{
				iHiFolder = f;
				DrawButton(&dc, (CEGButtonsFolder *) m_Folders.GetAt(iHiFolder), iHiFolder == m_iSelectedFolder, true);
			}
		}
		if (r == 4)
		{
			CEGButtonsSubItem	* ps = GetSubItem(f,i,s);
			CClientDC dc(this); 
			if (pLastHilink && pLastHilink != ps)
			{
				DrawSubItem(&dc, GetFolder(iHilinkFolder), GetItem(iHilinkFolder, iHilinkItem), pLastHilink, false);
			}
			if (pLastHilink != ps)
			{
				pLastHilink = ps;
				DrawSubItem(&dc, GetFolder(f), GetItem(f,i), ps, true);
				iHilinkFolder = f;
				iHilinkItem = i;
			}
		}

		if (r == 0)
		{
			if (iHiFolder >= 0)
			{
				CClientDC dc(this); 
				DrawButton(&dc, (CEGButtonsFolder *) m_Folders.GetAt(iHiFolder), iHiFolder == m_iSelectedFolder, false);
				iHiFolder = -1;
			}
			if (pLastHilink != NULL)
			{
				CClientDC dc(this); 
				DrawSubItem(&dc, GetFolder(iHilinkFolder), GetItem(iHilinkFolder, iHilinkItem), pLastHilink, false);
				pLastHilink = NULL;
			}
		}
	}

	CControlBar::OnMouseMove(nFlags, point);
}

BOOL CEGButtonsBar::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message)
{
	if (nHitTest == HTCLIENT)
	{
		CPoint point;
		::GetCursorPos (&point);
		ScreenToClient (&point);

		if (m_iDragging || m_dragRect.PtInRect(point)) 
		{
			SetCursor(hDragCur);
			return TRUE;
		}
		int f,i,s;
		int r = HitTest(f,i,s,point);
		if (r == 4)
		{
			CEGButtonsSubItem	* ps = GetSubItem(f,i,s);
			if (ps->dwStyle == 2) // item is hotlinked
			{
				SetCursor(hHandCur);
				return TRUE;
			}
		}
		if (r == 1)
		{
			SetCursor(hHandCur);
			return TRUE;
		}
	}

	return CControlBar::OnSetCursor(pWnd, nHitTest, message);
}

void CEGButtonsBar::OnInvertTracker(const CRect& rc)
{
	CFrameWnd* pParentFrame = GetParentFrame ();
	CDC* pDC = pParentFrame->GetDC();
	CRect rect(rc);
    ClientToScreen(rect);
	pParentFrame->ScreenToClient(rect);

	CBrush br;
	br.CreateSolidBrush(GetSysColor(COLOR_HIGHLIGHT));
	HBRUSH hOldBrush = NULL;
	hOldBrush = (HBRUSH)SelectObject(pDC->m_hDC, br.m_hObject);
	pDC->PatBlt(rect.left, rect.top, rect.Width(), rect.Height(), DSTINVERT);
	if (hOldBrush != NULL) SelectObject(pDC->m_hDC, hOldBrush);
	ReleaseDC(pDC);

}

void CEGButtonsBar::DrawCaption(CDC * pDC, CRect & rect)
{
	CRect rc(rect);
	rect.top += 25;
	rc.bottom = rc.top + 25;
	pDC->FillSolidRect(rc, GetSysColor(COLOR_3DFACE));

//	rc.InflateRect(0,-2);

//	rc.bottom -=2;
	rc.top ++;
	pDC->FillSolidRect(rc, m_crBackCaption);
	pDC->SetTextColor(m_crTextCaption);
	pDC->SetBkMode(TRANSPARENT);
	CFont * of = (CFont *) pDC->SelectObject(&ftCaption);
	rc.left += 4;
    pDC->DrawText(m_csCaption, rc, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);
	pDC->SelectObject(of);

	CRect rcSide(rect.right-4,rc.top,rect.right,rect.bottom);

	pDC->FillSolidRect(rcSide, GetSysColor(COLOR_3DFACE));
	rect.right -= 4;
}


CEGButtonsFolder::CEGButtonsFolder()
{
	hIcon = NULL;
	dwStyle = 0;
	rcItem.SetRectEmpty();
	nCommandID = 0;
}

CEGButtonsFolder::~CEGButtonsFolder()
{
	for (int t = 0; t < m_Items.GetSize(); t++)
	{
		CEGButtonsItem * p = (CEGButtonsItem *) m_Items.GetAt(t);
		delete p;
	}
	m_Items.RemoveAll();
}


CEGButtonsItem::CEGButtonsItem()
{
	dwStyle = 0;
	rcItem.SetRectEmpty();
}

CEGButtonsItem::~CEGButtonsItem()
{
	for (int t = 0; t < m_SubItems.GetSize(); t++)
	{
		CEGButtonsSubItem * p = (CEGButtonsSubItem *) m_SubItems.GetAt(t);
		delete p;
	}
	m_SubItems.RemoveAll();
}



CEGButtonsSubItem::CEGButtonsSubItem()
{
	hIcon = NULL;
	hHostedWnd = NULL;
	dwStyle = 0;
	rcItem.SetRectEmpty();
	iLastStatus = 0;
}

CEGButtonsSubItem::~CEGButtonsSubItem()
{
}

int CEGButtonsBar::AddFolderRes(const char * m_strName, UINT iIcoID, DWORD lParam, UINT nCmdID)
{
	HICON hIco = (HICON) LoadImage(AfxGetInstanceHandle(),MAKEINTRESOURCE(iIcoID),IMAGE_ICON,16,16,0);
	return AddFolder(m_strName, hIco, lParam, nCmdID);
}

int CEGButtonsBar::AddSubItem(const char * m_strName, int iIcoID, DWORD dwStyle, DWORD lParam, int iFolder, int iFolderItem)
{
	HICON hIco = (HICON) LoadImage(AfxGetInstanceHandle(),MAKEINTRESOURCE(iIcoID),IMAGE_ICON,16,16,0);
	return AddSubItem(m_strName, hIco, dwStyle, lParam, iFolder, iFolderItem);
}

int CEGButtonsBar::AddFolder(const char * m_strName, HICON hIcon, DWORD lParam, UINT nCmdID)
{
	if (m_csCaption == "") m_csCaption = m_strName;

	CEGButtonsFolder * o = new CEGButtonsFolder;
	o->csName = m_strName;
	o->hIcon = hIcon;
	o->lParam = lParam;
	o->nCommandID = nCmdID;
	return (int) m_Folders.Add(o);
}

int CEGButtonsBar::AddFolderItem(const char * m_strItemName, DWORD dwStyle, int iFolder)
{
	if (iFolder < 0) iFolder = (int) m_Folders.GetSize() - 1;

	CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt(iFolder);
	CEGButtonsItem * i = new CEGButtonsItem;

	i->csName = m_strItemName;
	i->dwStyle = dwStyle;
	return (int) o->m_Items.Add(i);
}
/*
bool CEGButtonsBar::RemoveFolder(int iFolder)
{
	int sz = (int) m_Folders.GetSize();
	if ( iFolder < 0 || iFolder >= sz )
		return false;

	CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt( iFolder );

	int sub_sz = (int) (o->m_Items.GetSize());
	for (int i=0; i<sub_sz; i++)
	{
		CEGButtonsItem * it = (CEGButtonsItem *)(o->m_Items.GetAt(i));
		int sub_sub_sz = (int)(it->m_SubItems.GetSize());
		for (int j=0;j<sub_sub_sz;j++)
			delete it->m_SubItems.GetAt(j);
		it->m_SubItems.RemoveAll();
		delete it;
	}
	o->m_Items.RemoveAll();
	delete o;
	m_Folders.RemoveAt(iFolder);
	return true;
}*/

bool CEGButtonsBar::RemoveFolder(int ind)
{
	int sz = (int) m_Folders.GetSize();
	if (ind<0 || ind>=sz)
		return false;

	CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt(ind);

	int sub_sz = (int) (o->m_Items.GetSize());
	for (int i=0;i<sub_sz;i++)
	{
		CEGButtonsItem * it = (CEGButtonsItem *)(o->m_Items.GetAt(i));
		int sub_sub_sz = (int)(it->m_SubItems.GetSize());
		for (int j=0;j<sub_sub_sz;j++)
		{
			CEGButtonsSubItem * sI = (CEGButtonsSubItem*)(it->m_SubItems.GetAt(i));
			if (sI->dwStyle==OCL_HWND && ::IsWindow(sI->hHostedWnd))
				::ShowWindow(sI->hHostedWnd,SW_HIDE);
			delete it->m_SubItems.GetAt(j);
		}
		it->m_SubItems.RemoveAll();
		delete it;
	}
	o->m_Items.RemoveAll();
	delete o;
	m_Folders.RemoveAt(ind);
	if (m_iSelectedFolder==ind)
		SetCurFolder((ind>1)?(ind-1):0, false, TRUE );
	else
		Invalidate();
	return true;
}


int CEGButtonsBar::AddSubItem(const char * m_strName, HICON hIcon, DWORD dwStyle, DWORD lParam, int iFolder, int iFolderItem)
{
	if (iFolder < 0) iFolder = (int) m_Folders.GetSize() - 1;
	CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt(iFolder);

	if (iFolderItem < 0) iFolderItem = (int) o->m_Items.GetSize() - 1;
	CEGButtonsItem  * i = (CEGButtonsItem *) o->m_Items.GetAt(iFolderItem);

	CEGButtonsSubItem * s = new CEGButtonsSubItem;

	s->csName = m_strName;
	s->dwStyle = dwStyle;
	s->hIcon = hIcon;
	s->lParam = lParam;

	return (int) i->m_SubItems.Add(s);
}

int CEGButtonsBar::AddSubItem(HWND hHosted, bool bStretch, int iFolder, int iFolderItem)
{
	ASSERT(hHosted);

	if (iFolder < 0) iFolder = (int) m_Folders.GetSize() - 1;
	CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt(iFolder);

	if (iFolderItem < 0) iFolderItem = (int) o->m_Items.GetSize() - 1;
	CEGButtonsItem  * i = (CEGButtonsItem *) o->m_Items.GetAt(iFolderItem);

	CEGButtonsSubItem * s = new CEGButtonsSubItem;
	
	CRect rc;
	::GetWindowRect(hHosted, rc);

	s->dwStyle = OCL_HWND;
	s->lParam = rc.Height();
	s->hHostedWnd = hHosted;
	if (bStretch) s->lParam = 0xFFFFFFFF;

	return (int) i->m_SubItems.Add(s);	
}


void CEGButtonsBar::DrawButtons(CDC * pDC, CRect & rect)
{
	if (rect.bottom - m_iFolderHeight < rect.top) return;

	int by = rect.bottom;

	CPoint pt;
	GetCursorPos(&pt);
	ScreenToClient(&pt);

	for (int t = (int) m_Folders.GetSize() - 1; t >= 0; t--)
	{
		CEGButtonsFolder * o = (CEGButtonsFolder *) m_Folders.GetAt(t);
		o->rcItem.SetRect(rect.left, by - m_iFolderHeight, rect.right, by);
		pDC->FillSolidRect(o->rcItem.left,o->rcItem.top-1,o->rcItem.Width(), o->rcItem.Height() + 1, m_crBackCaption);
		DrawButton(pDC, o, t == m_iSelectedFolder ? true : false, o->rcItem.PtInRect(pt) ? true : false);
		by -= (m_iFolderHeight + 1);
		rect.bottom -= (m_iFolderHeight + 1);
		if (rect.top >= rect.bottom - m_iFolderHeight) break;
	}
}

void CEGButtonsBar::DrawButton(CDC * pDC, CEGButtonsFolder * o, bool bSel, bool bOver)
{
	pDC->SetTextColor(m_crCmdOther);
	pDC->SetBkMode(TRANSPARENT);

	CFont * of = (CFont *) pDC->SelectObject(&ftFolders);
	CRect rect(o->rcItem);

	switch (CEGMenu::GetMenuDrawMode())
	{
	case CEGMenu::STYLE_XP:
	case CEGMenu::STYLE_XP_NOBORDER:
		
		if ( bOver ) {
			pDC->FillSolidRect(rect, GetXpHighlightColor());
		} else 	if (bSel)  {
			pDC->FillSolidRect(rect, LightenColor( 120, GetXpHighlightColor() ) );//COLOR_ACTIVECAPTION));
		} else {
			pDC->FillSolidRect(rect, MixedColor(CEGMenu::GetMenuBarColorXP(),GetSysColor(COLOR_WINDOW)) );
		}
		break;

	case CEGMenu::STYLE_XP_2003_NOBORDER:
	case CEGMenu::STYLE_XP_2003:
	case CEGMenu::STYLE_COLORFUL_NOBORDER:
	case CEGMenu::STYLE_COLORFUL:
		if ( bOver ) {
			COLORREF cr1 = GetXpHighlightColor();
			COLORREF cr2 = cr1;
			DrawGradientRect(pDC, rect, cr1, cr2);
		} else 	if (bSel)  {
			COLORREF cr1 = LightenColor( 120, GetXpHighlightColor() );
			COLORREF cr2 = cr1;
			DrawGradientRect(pDC, rect, cr1, cr2);
		} else {
			COLORREF cr1 = GetSysColor(COLOR_WINDOW);
			COLORREF cr2 = GetSysColor(COLOR_BTNFACE);
			DrawGradientRect(pDC, rect, cr1, cr2);
		}
		break;


	case CEGMenu::STYLE_ICY_NOBORDER:
	case CEGMenu::STYLE_ICY:
		if ( bOver ) {
			COLORREF cr1 =  DarkenColor(60, GetSysColor(COLOR_HIGHLIGHT) );//GetXpHighlightColor();
			COLORREF cr2 = LightenColor(60, GetSysColor(COLOR_HIGHLIGHT) );
			DrawGradientRect(pDC, rect, cr2, cr1 );
		} else 	if ( bSel )  {
			COLORREF cr1 =  LightenColor(80, GetSysColor(COLOR_HIGHLIGHT) );
			pDC->FillSolidRect( &rect, cr1 );
		} else {
			COLORREF cr1 = LightenColor( 60, CEGMenu::GetMenuColor() );
			COLORREF cr2 = GetSysColor(COLOR_BTNFACE);
			COLORREF cr3 = DarkenColor( 60, CEGMenu::GetMenuColor() );
			CRect rc = rect;
			rc.bottom /= 2;
			DrawGradientRect(pDC, rect, cr1, cr2);
			rc.top = rc.bottom + 1;
			rc.bottom = rect.bottom;
			DrawGradientRect(pDC, rect, cr2, cr3);
		}
		break;

	default: // NATIVE THEME
		InflateRect( &rect, -1, 0 );
		if ( o == m_Folders.GetAt( 0 ) )
			rect.top--;
		if ( o == m_Folders.GetAt( m_Folders.GetCount()-1 ) )
			rect.bottom--;
		if ( bSel )  {
			//pDC->FillSolidRect(rect, GetSysColor(COLOR_BTNFACE));
/*			CBrush br;
			br.CreateSolidBrush( GetSysColor(COLOR_BTNFACE) );
			CPen pn;
			pn.CreatePen( PS_SOLID, 1, GetSysColor( COLOR_3DSHADOW ) );
			CPen * pOldPen = pDC->SelectObject( &pn );
			CBrush* pOldBrush = pDC->
*/
			pDC->Draw3dRect( &rect, GetSysColor( COLOR_3DSHADOW ) , GetSysColor( COLOR_WINDOW ) );
			InflateRect( &rect, -1, -1 );
			pDC->FillSolidRect( &rect, GetSysColor(COLOR_BTNFACE));
		} else {
			pDC->DrawFrameControl( &rect, DFC_BUTTON, DFCS_BUTTONPUSH );
		}
		break;
	}



	if (o->hIcon && rect.left+20 < rect.right) 
		DrawIconEx(pDC->GetSafeHdc(), rect.left+rect.Height()/2-8, rect.top+rect.Height()/2-8, o->hIcon,16,16,0,NULL, DI_NORMAL);

	if ( ( CEGMenu::GetMenuDrawMode() == CEGMenu::STYLE_ICY_NOBORDER ||
		CEGMenu::GetMenuDrawMode() == CEGMenu::STYLE_ICY ) && ( bSel || bOver ) ) {
		pDC->SetTextColor(GetSysColor(COLOR_WINDOW));
	} else {
		pDC->SetTextColor(GetSysColor(COLOR_BTNTEXT));
	}

	CRect rc1(rect);
	rc1.left += 24;
	pDC->DrawText(o->csName, rc1, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);

	pDC->SelectObject(of);
}

void CEGButtonsBar::DrawItems(CDC * pDC, CEGButtonsFolder * oFolder, CRect & rect)
{
	if (rect.bottom - m_iItemHeight < rect.top) return;

	CFont * of = (CFont *) pDC->SelectObject(&ftItems);

	for (int t = 0; t < oFolder->m_Items.GetSize(); t++)
	{
		if (t != 0) rect.top += 7;
//		if (t == 0) rect.top += 1;

		CEGButtonsItem * i = (CEGButtonsItem *) oFolder->m_Items.GetAt(t);
		if (i->csName != "")	{
//			i->rcItem.SetRect(rect.left+1,rect.top, rect.right-1, rect.top + m_iItemHeight);
			i->rcItem.SetRect(rect.left,rect.top-1, rect.right, rect.top + m_iItemHeight -1 );
			rect.top += m_iItemHeight;
		} else {
//			i->rcItem.SetRect(rect.left+1,rect.top+1, rect.right-1, rect.top + 3);
			i->rcItem.SetRect(rect.left,rect.top, rect.right, rect.top + 3);
			rect.top += 8;
		}

		DrawItem(pDC, oFolder, i);
		DrawSubItems(pDC, oFolder, i, rect);

		if (rect.top >= rect.bottom - m_iItemHeight) break;
	}
	pDC->SelectObject(of);

}

void CEGButtonsBar::DrawItem(CDC * pDC, CEGButtonsFolder * o, CEGButtonsItem * i)
{
	CRect rect(i->rcItem), rc;

	COLORREF cr1, cr2, cr3;

	switch (CEGMenu::GetMenuDrawMode())
	{
	case CEGMenu::STYLE_XP:
	case CEGMenu::STYLE_XP_NOBORDER:
		
		cr1 = MixedColor(CEGMenu::GetMenuBarColorXP(),GetSysColor(COLOR_WINDOW));
		cr2 = GetSysColor(COLOR_BTNFACE);
		cr3 = GetSysColor(COLOR_BTNSHADOW);
		if (i->csName != "") {
			rect.right ++;
			pDC->Draw3dRect( rect, cr2, cr3 );
			rect.right --;
			InflateRect( &rect, 0, -1 );
		}
		pDC->FillSolidRect( rect, cr1 );
		break;

	case CEGMenu::STYLE_XP_2003_NOBORDER:
	case CEGMenu::STYLE_XP_2003:
	case CEGMenu::STYLE_COLORFUL_NOBORDER:
	case CEGMenu::STYLE_COLORFUL:

		cr1 = GetSysColor(COLOR_WINDOW);
		cr2 = GetSysColor(COLOR_BTNFACE);
		cr3 = GetSysColor(COLOR_BTNSHADOW);
		if (i->csName != "") {
			rect.right ++;
			pDC->Draw3dRect( rect, cr2, cr3 );
			rect.right --;
			InflateRect( &rect, 0, -1 );
		}
		DrawGradientRect(pDC, rect, cr1, cr2);
		break;

	case CEGMenu::STYLE_ICY_NOBORDER:
	case CEGMenu::STYLE_ICY:
		cr1 = LightenColor( 60, CEGMenu::GetMenuColor() );
		cr2 = GetSysColor(COLOR_BTNFACE);
		cr3 = DarkenColor( 60, CEGMenu::GetMenuColor() );
		if (i->csName != "") {
			rect.right ++;
			pDC->Draw3dRect( rect, cr2, cr3 );
			rect.right --;
			InflateRect( &rect, 0, -1 );
		}
		rc = rect;
		rc.bottom /= 2;
		DrawGradientRect(pDC, rect, cr1, cr2);
		rc.top = rc.bottom + 1;
		rc.bottom = rect.bottom;
		DrawGradientRect(pDC, rect, cr2, cr3);
		break;

	default: // NATIVE THEME

		InflateRect( &rect, -1, 0 );
		if ( i == o->m_Items.GetAt( 0 ) )
			rect.top +=2;
		if ( i->csName != _T("") ) {
			pDC->DrawFrameControl( &rect, DFC_BUTTON, DFCS_BUTTONPUSH );
		} else {
			pDC->FillSolidRect(rect, GetSysColor(COLOR_BTNFACE));
		}
		

		break;
	}


	if (i->csName != "")
	{
		pDC->SetTextColor(GetSysColor(COLOR_BTNTEXT));
		CRect rc1(rect);
		rc1.left += 4;
		pDC->DrawText(i->csName, rc1, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);
	}
}

void CEGButtonsBar::DrawSubItems(CDC * pDC, CEGButtonsFolder * oFolder, CEGButtonsItem * iItem, CRect & rect)
{
	if (rect.bottom - m_iSubItemHeight < rect.top) return;

	CPoint pt;
	GetCursorPos(&pt);
	ScreenToClient(&pt);
	
	for (int t1 = 0; t1 < iItem->m_SubItems.GetSize(); t1++)
	{
		CEGButtonsSubItem * p = (CEGButtonsSubItem *) iItem->m_SubItems.GetAt(t1);
		if (p->hHostedWnd)
		{
			int dy;
			if (p->lParam == 0xFFFFFFFF) dy = rect.Height(); else dy = (int) p->lParam;

			p->rcItem.SetRect(rect.left,rect.top-1,rect.right,rect.top+dy);

			::SetWindowPos(p->hHostedWnd, NULL,p->rcItem.left, p->rcItem.top, p->rcItem.Width(), p->rcItem.Height(), SWP_NOZORDER);
			if (!::IsWindowVisible(p->hHostedWnd)) ::ShowWindow(p->hHostedWnd, SW_SHOW);
			rect.top += dy;
		}
		else
		{
			p->rcItem.SetRect(rect.left+1,rect.top,rect.right-1,rect.top+m_iSubItemHeight);
			DrawSubItem(pDC, oFolder, iItem, p, p->rcItem.PtInRect(pt) ? true : false);
			rect.top += m_iSubItemHeight;
		}
		
		if (rect.top >= rect.bottom - m_iSubItemHeight) break;
	}
}

void CEGButtonsBar::DrawSubItem(CDC * pDC, CEGButtonsFolder * /* oFolder */, CEGButtonsItem * /* iItem */, CEGButtonsSubItem * pSubItem, bool bOver)
{
	CRect rect(pSubItem->rcItem);
	CFont * of = (CFont *) pDC->SelectObject(&ftItems);
	pDC->SetBkColor(m_crBackground);
	//pDC->SetBkColor( GetSysColor( COLOR_WINDOW) );

	switch (pSubItem->dwStyle)
	{
	case 0:
		{
			pDC->SetTextColor(m_crCmdOther);
			if (bOver)
			{
				pDC->SelectObject(&ftHotItems);
			}

			if (pSubItem->hIcon && rect.left+25 < rect.right)
			{
				if (pSubItem->hIcon) {
					pDC->FillSolidRect( rect.left+8, rect.top+1, 16, 16, m_crBackground );
					DrawIconEx(pDC->GetSafeHdc(), rect.left+8, rect.top+1, pSubItem->hIcon,16,16,0,NULL, DI_NORMAL);
				}
			}
			rect.left += 28;
			
			if (pSubItem->lParam)
			{
				CEGButtonsCCmdUI pui;
				pui.pSI = pSubItem;
				pui.m_nID = pSubItem->lParam;
				GetOwner()->OnCmdMsg(pui.m_nID, CN_UPDATE_COMMAND_UI, &pui, NULL);
				if (pui.iRes&2) pDC->FillSolidRect(rect, GetSysColor(COLOR_3DFACE)); // checked
				pSubItem->iLastStatus = pui.iRes;
			}

			pDC->DrawText(pSubItem->csName, rect, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);
		}
		break;
	case 1:
		{
			pDC->SetTextColor(m_crCmdOther);

			DWORD dwHotStyle = 0;

			if (bOver) 
			{
#if(WINVER >= 0x0500)
				dwHotStyle = DFCS_HOT;
#endif
				pDC->SelectObject(&ftHotItems);
			}
			CRect rci(rect.left+8, rect.top+2, rect.left + 20, rect.top + 14);
			if (pSubItem->lParam)
			{
				CEGButtonsCCmdUI pui;
				pui.pSI = pSubItem;
				pui.m_nID = pSubItem->lParam; 
				GetOwner()->OnCmdMsg(pui.m_nID, CN_UPDATE_COMMAND_UI, &pui, NULL);
				if (!(pui.iRes&2)) pDC->DrawFrameControl(rci, DFC_BUTTON, DFCS_BUTTONRADIO|DFCS_FLAT|dwHotStyle);
				else pDC->DrawFrameControl(rci, DFC_BUTTON, DFCS_BUTTONRADIO|DFCS_FLAT|DFCS_CHECKED|dwHotStyle);
				pSubItem->iLastStatus = pui.iRes;
			}
			else pDC->DrawFrameControl(rci, DFC_BUTTON, DFCS_BUTTONRADIO|DFCS_FLAT|dwHotStyle);
			rect.left += 28;

			pDC->DrawText(pSubItem->csName, rect, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);
		}
		break;
	case 2:
		{
			pDC->SetTextColor(m_crCmdLink);
			if (pSubItem->hIcon && rect.left+25 < rect.right)
				if (pSubItem->hIcon) {
					pDC->FillSolidRect( rect.left+8, rect.top+1, 16, 16, m_crBackground );
					DrawIconEx(pDC->GetSafeHdc(), rect.left+8, rect.top+1, pSubItem->hIcon,16,16,0,NULL, DI_NORMAL);
				}
			rect.left += 28;

			if (bOver) 
				pDC->SelectObject(&ftHotItems);

			if (pSubItem->lParam)
			{
				CEGButtonsCCmdUI pui;
				pui.pSI = pSubItem;
				pui.m_nID = pSubItem->lParam;
				GetOwner()->OnCmdMsg(pui.m_nID, CN_UPDATE_COMMAND_UI, &pui, NULL);
				pSubItem->iLastStatus = pui.iRes;
			}

			pDC->DrawText(pSubItem->csName, rect, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);

		}
		break;
	case 3: // checkbox
		{
			pDC->SetTextColor(m_crCmdOther);

			DWORD dwHotStyle = 0;
			if (bOver) 
			{
#if(WINVER >= 0x0500)
				dwHotStyle = DFCS_HOT;
#endif
				pDC->SelectObject(&ftHotItems);
			}

			CRect rci(rect.left+8, rect.top+2, rect.left + 20, rect.top + 14);
			if (pSubItem->lParam)
			{
				CEGButtonsCCmdUI pui;
				pui.pSI = pSubItem;
				pui.m_nID = pSubItem->lParam; 
				GetOwner()->OnCmdMsg(pui.m_nID, CN_UPDATE_COMMAND_UI, &pui, NULL);
				if (!(pui.iRes&2)) pDC->DrawFrameControl(rci, DFC_BUTTON, DFCS_BUTTONCHECK|DFCS_FLAT|dwHotStyle);
				else pDC->DrawFrameControl(rci, DFC_BUTTON, DFCS_BUTTONCHECK|DFCS_FLAT|DFCS_CHECKED|dwHotStyle);
				pSubItem->iLastStatus = pui.iRes;
			}
			else pDC->DrawFrameControl(rci, DFC_BUTTON, DFCS_BUTTONCHECK|DFCS_FLAT|dwHotStyle);
			rect.left += 28;

			pDC->DrawText(pSubItem->csName, rect, DT_SINGLELINE|DT_END_ELLIPSIS|DT_VCENTER);
		}
		break;
	}
	pDC->SelectObject(of);
}


void CEGButtonsBar::OnSize(UINT nType, int cx, int cy)
{
	CControlBar::OnSize(nType, cx, cy);
	m_dragRect.SetRect(cx-5, 0, cx, cy);
}

int CEGButtonsBar::HitTest(int & iFolder, int & iItem, int & iSubItem, CPoint point)
{
	for (iFolder = 0; iFolder < m_Folders.GetSize(); iFolder++)
	{
		CEGButtonsFolder * pf = (CEGButtonsFolder *) m_Folders.GetAt(iFolder);
		if (pf->rcItem.PtInRect(point)) return 1;
		
		for (iItem = 0; iItem < pf->m_Items.GetSize(); iItem++)
		{
			CEGButtonsItem * pi = (CEGButtonsItem *) pf->m_Items.GetAt(iItem);
			if (pi->rcItem.PtInRect(point)) return 2;

			for (iSubItem = 0; iSubItem < pi->m_SubItems.GetSize(); iSubItem++)
			{
				CEGButtonsSubItem * ps = (CEGButtonsSubItem *) pi->m_SubItems.GetAt(iSubItem);
				if (ps->rcItem.PtInRect(point)) return 4;
			}
		}
	}
	return 0;
}

void CEGButtonsBar::ClearRects(void)
{
	for (int f = 0; f < m_Folders.GetSize(); f++)
	{
		CEGButtonsFolder * pf = (CEGButtonsFolder *) m_Folders.GetAt(f);
		pf->rcItem.SetRectEmpty();
		for (int i = 0; i < pf->m_Items.GetSize(); i++)
		{
			CEGButtonsItem * pi = (CEGButtonsItem *) pf->m_Items.GetAt(i);
			pi->rcItem.SetRectEmpty();
			for (int s = 0; s < pi->m_SubItems.GetSize(); s++)
			{
				CEGButtonsSubItem * ps = (CEGButtonsSubItem *) pi->m_SubItems.GetAt(s);
				ps->rcItem.SetRectEmpty();
			}
		}
	}
}

CEGButtonsSubItem * CEGButtonsBar::GetSubItem(int f, int i, int s)
{
	CEGButtonsItem   * pi = (CEGButtonsItem *)   GetFolder(f)->m_Items.GetAt(i);
    CEGButtonsSubItem *ps = (CEGButtonsSubItem *) pi->m_SubItems.GetAt(s);
	return ps;
}

CEGButtonsFolder * CEGButtonsBar::GetFolder(int f)
{
	return (CEGButtonsFolder *) m_Folders.GetAt(f);
}

CEGButtonsItem * CEGButtonsBar::GetItem(int f, int i)
{
	return (CEGButtonsItem *) GetFolder(f)->m_Items.GetAt(i);
}

void CEGButtonsBar::OnUpdateCmdUI(CFrameWnd* /* pTarget */, BOOL /* bDisableIfNoHndler */)
{
	if (m_iSelectedFolder < 0) return;

	CEGButtonsFolder * oFolder = GetFolder(m_iSelectedFolder);
	for (int i = 0; i < oFolder->m_Items.GetSize(); i++)
	{
		CEGButtonsItem * pi = (CEGButtonsItem *) oFolder->m_Items.GetAt(i);
		for (int s = 0; s < pi->m_SubItems.GetSize(); s++)
		{
			CEGButtonsSubItem * ps = (CEGButtonsSubItem *) pi->m_SubItems.GetAt(s);
			if (ps->dwStyle == OCL_SELECT || ps->dwStyle == OCL_RADIO || ps->dwStyle == OCL_CHECK)
			{
				CEGButtonsCCmdUI pui;
				pui.pSI = ps;
				pui.m_nID = ps->lParam; 
				GetOwner()->OnCmdMsg(pui.m_nID, CN_UPDATE_COMMAND_UI, &pui, NULL);
				if (pui.iRes != ps->iLastStatus && !ps->rcItem.IsRectEmpty())
				{
                    InvalidateRect(ps->rcItem);					
				}
			}
		}
	}

/*	iLastStatus = pui.iRes;

	TRACE1("%d\n", (int) GetTickCount());
	CToolBar b;
	b.OnUpdateCmdUI(*/
}

LRESULT CEGButtonsBar::OnMouseLeave( WPARAM /*wParam*/, LPARAM /*lParam*/ ) {
	// canceling hottrack mode
	if ( m_iDragging == 0 && iHiFolder >= 0 ) {
		CClientDC dc(this); 
		DrawButton(&dc, (CEGButtonsFolder *) m_Folders.GetAt(iHiFolder), iHiFolder == m_iSelectedFolder, false);
		iHiFolder = -1;
	}
	m_bIsMouseInside = FALSE;
	return 0L;
}
