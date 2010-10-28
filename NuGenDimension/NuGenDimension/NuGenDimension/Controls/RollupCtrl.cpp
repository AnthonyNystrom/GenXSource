//////////////////////////////////////////////////////////////////////////////
//
// RollupCtrl.cpp
// 
//
// Code Johann Nadalutti
// Mail: jnadalutti@hotmail.com
//
//////////////////////////////////////////////////////////////////////////////
//
// This code is free for personal and commercial use, providing this 
// notice remains intact in the source files and all eventual changes are
// clearly marked with comments.
//
// No warrantee of any kind, express or implied, is included with this
// software; use at your own risk, responsibility for damages (if any) to
// anyone resulting from the use of this software rests entirely with the
// user.
//
//////////////////////////////////////////////////////////////////////////////
#include "stdafx.h"
#include "RollupCtrl.h"
#include "..//NuGenDimension.h"
#include ".\rollupctrl.h"

#pragma warning( disable : 4312 4311 ) 

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CRollupCtrl Message Map

BEGIN_MESSAGE_MAP(CRollupCtrl, CWnd)
	//{{AFX_MSG_MAP(CRollupCtrl)
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_MOUSEWHEEL()
	ON_WM_CONTEXTMENU()
	//}}AFX_MSG_MAP
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRollupCtrl Implementation

IMPLEMENT_DYNCREATE(CRollupCtrl, CWnd)

//---------------------------------------------------------------------------
// Constructor
//---------------------------------------------------------------------------
CRollupCtrl::CRollupCtrl()
{
	m_strMyClass = AfxRegisterWndClass(
		CS_VREDRAW | CS_HREDRAW,
		(HCURSOR)::LoadCursor(NULL, IDC_ARROW),
		(HBRUSH)(COLOR_BTNSHADOW),
		NULL);

	m_StartYPos = m_PageHeight = 0;

	m_nColumnWidth=200;
	m_bEnabledAutoColumns=FALSE;

	COLORREF col = ::GetSysColor(COLOR_WINDOW);
	m_brash = new CBrush(col);
}

//---------------------------------------------------------------------------
// Destructor
//---------------------------------------------------------------------------
CRollupCtrl::~CRollupCtrl()
{
	//Remove all pages allocations
	for (int i=0; i<m_PageList.GetSize(); i++)
	{
		RC_PAGEINFO* pi = m_PageList[i];
	
		if (pi->pwndButton)		delete pi->pwndButton;
		if (pi->pwndGroupBox)	delete pi->pwndGroupBox;
		if (pi->pwndStatic)     delete pi->pwndStatic;
 
		if (pi->pwndTemplate)
		{
			if (pi->bAutoDestroyTpl) {
				pi->pwndTemplate->GetWindow()->DestroyWindow();
				delete pi->pwndTemplate;
			} else {
				//pi->pwndTemplate->ShowWindow(SW_HIDE);
				::SetWindowLong(pi->pwndTemplate->GetWindow()->m_hWnd, 
					DWL_DLGPROC, (LONG)pi->pOldDlgProc);
			}
		}

		delete pi;
	}
	if (m_brash)
		delete m_brash;
	
}

//---------------------------------------------------------------------------
// Create
//---------------------------------------------------------------------------
BOOL CRollupCtrl::Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nID)
{
	BOOL bRet = CWnd::Create(m_strMyClass, "RollupCtrl", dwStyle, rect, pParentWnd, nID);

	return bRet;
}


//---------------------------------------------------------------------------
// Function name	: InsertPage
// Description		: return -1 if an error occurs
//					  Make sure template had WS_CHILD style
//---------------------------------------------------------------------------
int CRollupCtrl::InsertPage(LPCTSTR caption, IBaseInterfaceOfGetDialogs* pwndTemplate, 
							bool withRadio,BOOL bAutoDestroyTpl, int idx)
{
	if (!pwndTemplate)		return -1;

	if (idx>0 && idx>=m_PageList.GetSize())		idx=-1;

	//Insert Page
	return _InsertPage(caption, pwndTemplate, withRadio, idx, bAutoDestroyTpl);
}

void   CRollupCtrl::EnableRadio(unsigned int ind,bool pg)
{
	if (ind>=static_cast<unsigned int>(m_PageList.GetSize()))	
		return;
	if (m_PageList[ind]->pwndButton)
		m_PageList[ind]->pwndButton->EnableWindow((pg)?TRUE:FALSE);
	else
	    m_PageList[ind]->pwndGroupBox->EnableWindow((pg)?TRUE:FALSE);
	if (m_PageList[ind]->pwndStatic)
		m_PageList[ind]->pwndStatic->EnableWindow((pg)?TRUE:FALSE);
}

void   CRollupCtrl::SetActiveRadio(unsigned int ind, bool scroll)
{
	unsigned int sz = static_cast<unsigned int>(m_PageList.GetSize());
	if (ind>=sz)	
		return;
	for (unsigned int i=0;i<sz;i++)
	{
		if (i==ind)
		{
			if (m_PageList[i]->pwndButton)
				m_PageList[i]->pwndButton->EnableWindow(TRUE);
			else
			    m_PageList[i]->pwndGroupBox->EnableWindow(TRUE);
			if (m_PageList[i]->pwndButton)
				m_PageList[i]->pwndButton->SetCheck(BST_CHECKED);
			if (m_PageList[i]->pwndStatic)
				m_PageList[i]->pwndStatic->EnableWindow(TRUE);
			m_PageList[i]->pwndTemplate->EnableControls(true);
		}
		else
		{
			if (m_PageList[i]->pwndButton)
				m_PageList[i]->pwndButton->SetCheck(BST_UNCHECKED);
			m_PageList[i]->pwndTemplate->EnableControls(false);
		}
	}
	if (scroll)
		ScrollToPage(ind);
	Invalidate();

}


//---------------------------------------------------------------------------
// Function name	: _InsertPage
// Description	  : Called by InsertPage(...) methods
//									Return -1 if an error occurs
//---------------------------------------------------------------------------
int CRollupCtrl::_InsertPage(const char* caption, IBaseInterfaceOfGetDialogs* pwndTemplate, 
							 bool withRadio,int idx, BOOL bAutoDestroyTpl)
{
	ASSERT(pwndTemplate!=NULL);
	ASSERT(pwndTemplate->GetWindow()!=NULL);

 	//Get client rect
	CRect r; GetClientRect(r);

	//Create GroupBox
	CButton* groupbox = new CButton;
	CString cpt(caption);
	
	if (!withRadio)
		groupbox->Create("", WS_CHILD|BS_GROUPBOX, r, this, 0 );
	else
		groupbox->Create("", WS_CHILD|BS_GROUPBOX, r, this, 0 );
	groupbox->EnableWindow(FALSE);
	groupbox->ShowWindow(SW_HIDE);
	
	//Create Button
	CButton* but = NULL;
	CStatic* sta = NULL;
	if (withRadio)
	{
		but = new CButton;
		if (cpt.GetLength()<10)
			cpt.Format("   %s",caption);
		but->Create(caption/*""*/, WS_CHILD|/*BS_AUTOCHECKBOX|BS_PUSHLIKE|*/
			BS_FLAT|BS_RADIOBUTTON, r, this, 0 );
		but->EnableWindow(FALSE);
		but->SetCheck(BST_UNCHECKED);
		
		//Change Button's font
		HFONT hfont= (HFONT)::GetStockObject(DEFAULT_GUI_FONT);
		CFont* font = CFont::FromHandle(hfont);
		but->SetFont(font);
	}
	else
	{	
		sta = new CStatic;
		if (cpt.GetLength()<10)
			cpt.Format("   %s",caption);
		sta->Create(caption, WS_CHILD|WS_VISIBLE, r, this, 0 );
		sta->EnableWindow(FALSE);
		
		//Change Button's font
		HFONT hfont= (HFONT)::GetStockObject(DEFAULT_GUI_FONT);
		CFont* font = CFont::FromHandle(hfont);
		sta->SetFont(font);
	}

	
	//groupbox->SetFont(font);

	pwndTemplate->EnableControls(false);
	//Add page at pagelist
	RC_PAGEINFO* pi			= new RC_PAGEINFO;
	pi->cstrCaption			= caption;
	pi->bExpanded				= TRUE;
	pi->bEnable					= FALSE;
	pi->pwndTemplate		= pwndTemplate;
	pi->pwndButton			= but;
	pi->pwndGroupBox		= groupbox;
	pi->pwndStatic          = sta;
	pi->pOldDlgProc			= (WNDPROC)::GetWindowLong(pwndTemplate->GetWindow()->m_hWnd, DWL_DLGPROC);
	if (but)
		pi->pOldButProc			= (WNDPROC)::GetWindowLong(but->m_hWnd, GWL_WNDPROC);
	else
		pi->pOldButProc			= (WNDPROC)NULL;
	pi->bAutoDestroyTpl	= bAutoDestroyTpl;

	if (but )
	{
		CDC* dc = but->GetDC();
		pi->CaptionSizes = dc->GetTextExtent(cpt);
		but->ReleaseDC(dc);
	}
	else
		if (sta)
		{
			CDC* dc = sta->GetDC();
			pi->CaptionSizes = dc->GetTextExtent(cpt);
			sta->ReleaseDC(dc);
		}
		else
			pi->CaptionSizes = CSize(0,0);

	int newidx;
	if (idx<0)	newidx = (int)m_PageList.Add(pi);
	else	{	m_PageList.InsertAt(idx, pi); newidx=idx; }

	//Set Dlg Window datas
	::SetWindowLong(pwndTemplate->GetWindow()->m_hWnd, GWL_USERDATA,	(LONG)m_PageList[newidx]);
	::SetWindowLong(pwndTemplate->GetWindow()->m_hWnd, DWL_USER,		(LONG)this);

	//Set But Window data
	if (but)
		::SetWindowLong(but->m_hWnd, GWL_USERDATA,	(LONG)m_PageList[newidx]);
	
	//SubClass Template window proc
	::SetWindowLong(pwndTemplate->GetWindow()->m_hWnd, DWL_DLGPROC, (LONG)CRollupCtrl::DlgWindowProc);

	//SubClass Button window proc
	if (but)
		::SetWindowLong(but->m_hWnd, GWL_WNDPROC, (LONG)CRollupCtrl::ButWindowProc);
	
	//Update
	m_PageHeight+=RC_PGBUTTONHEIGHT+(RC_GRPBOXINDENT/2);
	RecalLayout();

	return newidx;
}


//---------------------------------------------------------------------------
// Function name	: RemovePage
// Description		: 
//---------------------------------------------------------------------------
bool CRollupCtrl::RemovePage(int idx)
{
	if (idx>=m_PageList.GetSize() || idx<0)				
		return false;

	//Remove
	_RemovePage(idx);

	//Update
	RecalLayout();

	return true;
}

bool  CRollupCtrl::RemovePage(IBaseInterfaceOfGetDialogs* pg)
{
	INT_PTR sz = m_PageList.GetSize();
	for (INT_PTR i=0;i<sz;i++)
	{
		if (m_PageList.GetAt(i)->pwndTemplate == pg)
		{
			_RemovePage(i);
			RecalLayout();
			return true;
		}
	}
	return false;
}

bool  CRollupCtrl::RenamePage(unsigned int ind, const char* lab)
{
	if (ind>=(unsigned int)m_PageList.GetSize() || ind<0)				
		return false;

	RC_PAGEINFO* pi = m_PageList.GetAt(ind);

	pi->cstrCaption			= lab;
	if (pi->pwndButton)
	{
		pi->pwndButton->SetWindowText(lab);
		CDC* dc = pi->pwndButton->GetDC();
		pi->CaptionSizes = dc->GetTextExtent(lab);
		pi->pwndButton->ReleaseDC(dc);
	}
	else
		if (pi->pwndStatic)
		{
			pi->pwndStatic->SetWindowText(lab);
			CDC* dc = pi->pwndStatic->GetDC();
			pi->CaptionSizes = dc->GetTextExtent(lab);
			pi->pwndStatic->ReleaseDC(dc);
		}
		else
			pi->CaptionSizes = CSize(0,0);

	return true;
}


//---------------------------------------------------------------------------
// Function name	: RemoveAllPages
// Description		:
//---------------------------------------------------------------------------
void CRollupCtrl::RemoveAllPages()
{
	//Remove all
	while (m_PageList.GetSize())	_RemovePage(0);

	//Update
	RecalLayout();
}

//---------------------------------------------------------------------------
// Function name	: _RemovePage
// Description		: Called by RemovePage or RemoveAllPages methods
//---------------------------------------------------------------------------
void CRollupCtrl::_RemovePage(int idx)
{
	RC_PAGEINFO* pi = m_PageList[idx];

	//Get Page Rect
	CRect tr; 
	if (pi->pwndTemplate && 
		pi->pwndTemplate->GetType()!=IBaseInterfaceOfGetDialogs::USER_DIALOG)
			pi->pwndTemplate->GetWindow()->GetWindowRect(&tr);

	//Update PageHeight
	m_PageHeight-=RC_PGBUTTONHEIGHT+(RC_GRPBOXINDENT/2);
	if (pi->bExpanded)		m_PageHeight-=tr.Height();

	//Remove wnds
	if (pi->pwndButton)				delete pi->pwndButton;
	if (pi->pwndGroupBox)			delete pi->pwndGroupBox;
	if (pi->pwndStatic)				delete pi->pwndStatic;

	if (pi->pwndTemplate)
	{
		if (pi->bAutoDestroyTpl) {
			pi->pwndTemplate->GetWindow()->DestroyWindow();
			delete pi->pwndTemplate;
		} else {
			pi->pwndTemplate->GetWindow()->ShowWindow(SW_HIDE);
			::SetWindowLong(pi->pwndTemplate->GetWindow()->m_hWnd, DWL_DLGPROC, (LONG)pi->pOldDlgProc);
		}
	}

	//Remove page from array
	m_PageList.RemoveAt(idx);

	//Delete pageinfo
	delete pi;
}


//---------------------------------------------------------------------------
// Function name	: ExpandPage
// Description	  : 
//---------------------------------------------------------------------------
void CRollupCtrl::ExpandPage(int idx, BOOL bExpand, BOOL bScrollToPage)
{
	if (idx>=m_PageList.GetSize() || idx<0)				return;

	//Expand-collapse
	_ExpandPage(m_PageList[idx], bExpand);

	//Update
	RecalLayout();

	//Scroll to this page (Automatic page visibility)
	if (bScrollToPage&&bExpand)		ScrollToPage(idx, FALSE);

}

//---------------------------------------------------------------------------
// Function name	: ExpandAllPages
// Description	  : 
//---------------------------------------------------------------------------
void CRollupCtrl::ExpandAllPages(BOOL bExpand)
{
	//Expand-collapse All
	for (int i=0; i<m_PageList.GetSize(); i++)
		_ExpandPage(m_PageList[i], bExpand);

	//Update
	RecalLayout();
}

//---------------------------------------------------------------------------
// Function name	: _ExpandPage
// Description	  : Called by ExpandPage or ExpandAllPages methods
//---------------------------------------------------------------------------
void	CRollupCtrl::_ExpandPage(RC_PAGEINFO* pi, BOOL bExpand)
{
	//Check if we need to change state
	if (pi->bExpanded==bExpand)					return;
	if (!pi->bEnable)										return;

	//Get Page Rect
	CRect tr; pi->pwndTemplate->GetWindow()->GetWindowRect(&tr);

	//Expand-collapse
	pi->bExpanded = bExpand;

	if (bExpand)	m_PageHeight+=tr.Height();
	else					m_PageHeight-=tr.Height();
}


//---------------------------------------------------------------------------
// Function name	: EnablePage
// Description	  : 
//---------------------------------------------------------------------------
void CRollupCtrl::EnablePage(int idx, BOOL bEnable)
{
	if (idx>=m_PageList.GetSize() || idx<0)				return;

	//Enable-Disable
	_EnablePage(m_PageList[idx], bEnable);

	//Update
	RecalLayout();
}


//---------------------------------------------------------------------------
// Function name	: EnableAllPages
// Description	  : 
//---------------------------------------------------------------------------
void CRollupCtrl::EnableAllPages(BOOL bEnable)
{
	//Enable-disable All
	for (int i=0; i<m_PageList.GetSize(); i++)
		_EnablePage(m_PageList[i], bEnable);

	//Update
	RecalLayout();
}

//---------------------------------------------------------------------------
// Function name	: _EnablePage
// Description	  : Called by EnablePage or EnableAllPages methods
//---------------------------------------------------------------------------
void CRollupCtrl::_EnablePage(RC_PAGEINFO* pi, BOOL bEnable)
{
	//Check if we need to change state
	if (pi->bEnable==bEnable)		return;

	//Get Page Rect
	CRect tr; pi->pwndTemplate->GetWindow()->GetWindowRect(&tr);

	//Change state
	pi->bEnable = bEnable;

	if (pi->bExpanded)		{ m_PageHeight-=tr.Height(); pi->bExpanded=FALSE; }
}


//---------------------------------------------------------------------------
// Function name	: ScrollToPage
// Description		: Scroll a page at the top of the RollupCtrl if bAtTheTop=TRUE
//					  or just ensure page visibility into view if bAtTheTop=FALSE
//---------------------------------------------------------------------------
void CRollupCtrl::ScrollToPage(int idx, BOOL bAtTheTop)
{
	if (idx>=m_PageList.GetSize() || idx<0)				return;

	//Get page infos
	RC_PAGEINFO* pi = m_PageList[idx];

	//Get windows rect
	CRect r; GetWindowRect(&r);
	CRect tr; pi->pwndTemplate->GetWindow()->GetWindowRect(&tr);

	//Check page visibility
	if (bAtTheTop || ((tr.bottom>r.bottom) || (tr.top<r.top)))
	{
		// was ---- pi->pwndButton->GetWindowRect(&tr);
		//Compute new m_nStartYPos
		pi->pwndGroupBox->GetWindowRect(&tr);
		m_StartYPos-= (tr.top-r.top);

		//Update
		RecalLayout();
	}

}


//---------------------------------------------------------------------------
// Function name	: MovePageAt
// Description		: newidx can be equal to -1 (move at end)
//					  Return -1 if an error occurs
//---------------------------------------------------------------------------
int CRollupCtrl::MovePageAt(int idx, int newidx)
{
	if (idx==newidx)									return -1;
	if (idx>=m_PageList.GetSize() || idx<0)				return -1;

	if (newidx>0 && newidx>=m_PageList.GetSize())		newidx=-1;

	//Remove page from its old position
	RC_PAGEINFO* pi = m_PageList[idx];
	m_PageList.RemoveAt(idx);

	//Insert at its new position
	int retidx;
	if (newidx<0)	retidx = (int)m_PageList.Add(pi);
	else	{ m_PageList.InsertAt(newidx, pi); retidx=newidx; }


	//Update
	RecalLayout();
	
	return retidx;
}


//---------------------------------------------------------------------------
// Function name	: IsPageExpanded
// Description		: 
//---------------------------------------------------------------------------
BOOL CRollupCtrl::IsPageExpanded(int idx)
{
	if (idx>=m_PageList.GetSize() || idx<0)				return FALSE;
	return m_PageList[idx]->bExpanded;
}

//---------------------------------------------------------------------------
// Function name	: IsPageEnabled
// Description		: 
//---------------------------------------------------------------------------
BOOL CRollupCtrl::IsPageEnabled(int idx)
{
	if (idx>=m_PageList.GetSize() || idx<0)				return FALSE;
	return m_PageList[idx]->bEnable;
}




//---------------------------------------------------------------------------
// Function name	: RecalLayout
// Description	  : 
//---------------------------------------------------------------------------
void CRollupCtrl::RecalLayout()
{
	//Check StartPosY
	CRect r; GetClientRect(&r);
	int BottomPagePos = m_StartYPos+m_PageHeight;

	int nWidth	= r.Width();
	int nHeight = r.Height();
	if (BottomPagePos<nHeight)		m_StartYPos = nHeight-m_PageHeight;
	if (m_StartYPos>0)						m_StartYPos = 0;

	////////////////////////////////////////////
	//Calc new pages's positions 
	// used column sub-divisions if necessary
	int nPageWidth = nWidth-RC_SCROLLBARWIDTH;

	if (m_bEnabledAutoColumns)
	{
		nPageWidth=m_nColumnWidth;
		if (nPageWidth>nWidth-RC_SCROLLBARWIDTH)
			nPageWidth=nWidth-RC_SCROLLBARWIDTH;
	}

	int posx=0;
	int posy=0;
	int nMaxHeight=-1;

	CArray<CPoint, CPoint&> carrayPos;

	for (int i=0; i<m_PageList.GetSize(); i++)
	{
		RC_PAGEINFO* pi = m_PageList[i];

		//Page Height
		int nCurPageHeight=RC_PGBUTTONHEIGHT+(RC_GRPBOXINDENT/2);
		//if (pi->bExpanded && pi->bEnable)
		{
			CRect tr; pi->pwndTemplate->GetWindow()->GetWindowRect(&tr);
			nCurPageHeight+=tr.Height();
		}

		//Split to a new column ?
		if (m_bEnabledAutoColumns && ((nWidth-posx-m_nColumnWidth-RC_SCROLLBARWIDTH )>m_nColumnWidth) && i!=0 && (posy+(nCurPageHeight/2))>nHeight)
		{
			posx+=m_nColumnWidth;	//New column
			posy=0;
		}

		CPoint cpos(posx, posy);
		carrayPos.Add(cpos);
		
		posy+=nCurPageHeight;
		if (posy>nMaxHeight)	nMaxHeight=posy;
	}

	if (nMaxHeight!=-1)
	{
		m_PageHeight=nMaxHeight;

		BottomPagePos = m_StartYPos+m_PageHeight;
		if (BottomPagePos<nHeight)		m_StartYPos = nHeight-m_PageHeight;
		if (m_StartYPos>0)						m_StartYPos = 0;
	}

	////////////////////////////////////////////
	//Update children windows position
	HDWP hdwp = BeginDeferWindowPos((int)m_PageList.GetSize()*3);	//*3 for pwndButton+pwndTemplate+pwndGroupBox
	if (hdwp)
	{

		int posx=0;
		int posy=m_StartYPos;

		for (int i=0; i<m_PageList.GetSize(); i++)
		{
			RC_PAGEINFO* pi = m_PageList[i];

			posx=carrayPos[i].x;
			posy=carrayPos[i].y+m_StartYPos+2*i;

			//Enable-Disable Button
			//pi->pwndButton->SetCheck(pi->bEnable&pi->bExpanded);
			//pi->pwndButton->EnableWindow(pi->bEnable);

			//Update Button's position and size

			//Expanded
			if (/*pi->bExpanded && pi->bEnable*/TRUE)	
			{
				CRect tr; pi->pwndTemplate->GetWindow()->GetWindowRect(&tr);

				//Update GroupBox position and size
				DeferWindowPos(hdwp, pi->pwndGroupBox->m_hWnd, 0, posx+2, posy, nPageWidth-3, tr.Height()+RC_PGBUTTONHEIGHT+RC_GRPBOXINDENT-4, SWP_NOZORDER|SWP_HIDEWINDOW);

				//Update Template position and size
				DeferWindowPos(hdwp, pi->pwndTemplate->GetWindow()->m_hWnd, 0, posx+RC_GRPBOXINDENT, posy+RC_PGBUTTONHEIGHT, nPageWidth-(RC_GRPBOXINDENT*2), tr.Height(), SWP_NOZORDER);

				//Update Button's position and size
				if (pi->pwndButton)
					DeferWindowPos(hdwp, pi->pwndButton->m_hWnd, 0, posx+RC_GRPBOXINDENT, posy, /*RC_PGBUTTONHEIGHT+*/pi->CaptionSizes.cx/*nPageWidth-(RC_GRPBOXINDENT*2)*/, RC_PGBUTTONHEIGHT, SWP_NOZORDER|SWP_SHOWWINDOW);

				if (pi->pwndStatic)
					DeferWindowPos(hdwp, pi->pwndStatic->m_hWnd, 0, posx+RC_GRPBOXINDENT, posy, /*RC_PGBUTTONHEIGHT+*/pi->CaptionSizes.cx/*nPageWidth-(RC_GRPBOXINDENT*2)*/, RC_PGBUTTONHEIGHT, SWP_NOZORDER|SWP_SHOWWINDOW);

			//Collapsed
			} else {	

				//Update GroupBox position and size
				DeferWindowPos(hdwp, pi->pwndGroupBox->m_hWnd, 0, posx+2, posy, nPageWidth-3, /*16*/20, SWP_NOZORDER|SWP_HIDEWINDOW);

				//Update Template position and size
				DeferWindowPos(hdwp, pi->pwndTemplate->GetWindow()->m_hWnd, 0, posx+RC_GRPBOXINDENT, 0, 0, 0,SWP_NOZORDER|SWP_HIDEWINDOW|SWP_NOSIZE|SWP_NOMOVE);

				//Update Button's position and size
				if (pi->pwndButton)
					DeferWindowPos(hdwp, pi->pwndButton->m_hWnd, 0, posx+RC_GRPBOXINDENT, posy, /*RC_PGBUTTONHEIGHT+*/pi->CaptionSizes.cx/*nPageWidth-(RC_GRPBOXINDENT*2)*/, RC_PGBUTTONHEIGHT, SWP_NOZORDER|SWP_SHOWWINDOW);

				if (pi->pwndStatic)
					DeferWindowPos(hdwp, pi->pwndStatic->m_hWnd, 0, posx+RC_GRPBOXINDENT, posy, /*RC_PGBUTTONHEIGHT+*/pi->CaptionSizes.cx/*nPageWidth-(RC_GRPBOXINDENT*2)*/, RC_PGBUTTONHEIGHT, SWP_NOZORDER|SWP_SHOWWINDOW);

			}

		}
		EndDeferWindowPos(hdwp);
	}

	////////////////////////////////////////////
	//Update children windows visibility
	hdwp = BeginDeferWindowPos((int)m_PageList.GetSize());
	if (hdwp)
	{

		for (int i=0; i<m_PageList.GetSize(); i++){
			RC_PAGEINFO* pi = m_PageList[i];

			//Expanded
			if (/*pi->bExpanded && pi->bEnable*/TRUE) {
				DeferWindowPos(hdwp, pi->pwndTemplate->GetWindow()->m_hWnd, 0, 0, 0, 0, 0, SWP_NOZORDER|SWP_SHOWWINDOW|SWP_NOSIZE|SWP_NOMOVE);
			//Collapsed
			} else {
				DeferWindowPos(hdwp, pi->pwndTemplate->GetWindow()->m_hWnd, 0, 0, 0, 0, 0, SWP_NOZORDER|SWP_HIDEWINDOW|SWP_NOSIZE|SWP_NOMOVE);
			}

		}
		EndDeferWindowPos(hdwp);
	}


	//////////////////////////////////////////
	//Update Scroll Bar
	/*CRect br = CRect(r.right-2*RC_SCROLLBARWIDTH,r.top, r.right, r.bottom);
	InvalidateRect(&br, FALSE);
	br.left = 0;
	br.right = RC_SCROLLBARWIDTH;
	InvalidateRect(&br, TRUE);*/
	Invalidate();
	UpdateWindow();

}

//---------------------------------------------------------------------------
// Function name	: GetPageIdxFromButtonHWND
// Description	  : Return -1 if matching hwnd not found
//---------------------------------------------------------------------------
int CRollupCtrl::GetPageIdxFromButtonHWND(HWND hwnd)
{
	//Search matching button's hwnd
	for (int i=0; i<m_PageList.GetSize(); i++)
		if (m_PageList[i]->pwndButton && (hwnd==m_PageList[i]->pwndButton->m_hWnd))	
				return i;

	return -1;
}

//---------------------------------------------------------------------------
// Function name	: GetPageInfo
// Description	  : Return -1 if an error occurs
//---------------------------------------------------------------------------
const RC_PAGEINFO* CRollupCtrl::GetPageInfo(int idx)
{
	if (idx>=m_PageList.GetSize() || idx<0)				return (RC_PAGEINFO*)-1;

	return m_PageList[idx];
}


//---------------------------------------------------------------------------
// Function name	: EnableAutoColumns
// Description	  : ...
//---------------------------------------------------------------------------
void CRollupCtrl::EnableAutoColumns(BOOL bEnable)
{
	if (m_bEnabledAutoColumns!=bEnable)
	{
		m_bEnabledAutoColumns=bEnable;
		RecalLayout();
	}

}


//---------------------------------------------------------------------------
// Function name	: SetColumnWidth
// Description	  : ...
//---------------------------------------------------------------------------
BOOL CRollupCtrl::SetColumnWidth(int nWidth)
{
	if (nWidth>RC_MINCOLUMNWIDTH)
	{
		m_nColumnWidth=nWidth;
		RecalLayout();
		return TRUE;
	}

	return FALSE;
}

//---------------------------------------------------------------------------
// Function name	: SetPageCaption
// Description	  : ...
//---------------------------------------------------------------------------
BOOL CRollupCtrl::SetPageCaption(int idx, LPCSTR caption)
{
	if (idx>=m_PageList.GetSize() || idx<0)				return FALSE;
	
	if (m_PageList[idx]->pwndButton)
		m_PageList[idx]->pwndButton->SetWindowText(caption);
	else
		m_PageList[idx]->pwndGroupBox->SetWindowText(caption);

	if (m_PageList[idx]->pwndStatic)
		m_PageList[idx]->pwndStatic->SetWindowText(caption);

	m_PageList[idx]->cstrCaption = caption;

	return TRUE;	
}


//---------------------------------------------------------------------------
// SubClasser
//---------------------------------------------------------------------------
LRESULT CALLBACK CRollupCtrl::DlgWindowProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	RC_PAGEINFO* pi		= (RC_PAGEINFO*)GetWindowLong(hWnd, GWL_USERDATA);
	CRollupCtrl*	_this = (CRollupCtrl*)GetWindowLong(hWnd, DWL_USER);

	CRect r; _this->GetClientRect(&r);
	if (_this->m_PageHeight>r.Height())	//Can Scroll ?
	{

		switch (uMsg) {

			case WM_MBUTTONDOWN:
			case WM_LBUTTONDOWN: {
				CPoint pos; GetCursorPos(&pos);
				_this->m_OldMouseYPos = pos.y;
				::SetCapture(hWnd);
				::SetFocus(_this->m_hWnd);
			break; }

			case WM_MBUTTONUP:
			case WM_LBUTTONUP:
				if (::GetCapture()==hWnd) 	::ReleaseCapture();
			break;

			case WM_MOUSEMOVE: {

					if ((wParam==MK_LBUTTON||wParam==MK_MBUTTON) && ::GetCapture()==hWnd) {
						CPoint pos; GetCursorPos(&pos);
						_this->m_StartYPos+=(pos.y-_this->m_OldMouseYPos);
						_this->RecalLayout();
						_this->m_OldMouseYPos = pos.y;
						//return 0;
					}

			break;}

			case WM_SETCURSOR:
			{
				if ((HWND)wParam==hWnd)	{ ::SetCursor(::LoadCursor(NULL, RC_CURSOR)); return TRUE; }
			break;
			}

		}//switch(uMsg)

	}

	return ::CallWindowProc(pi->pOldDlgProc, hWnd, uMsg, wParam, lParam);
}

//---------------------------------------------------------------------------
// Button SubClasser
//---------------------------------------------------------------------------
LRESULT CALLBACK CRollupCtrl::ButWindowProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	if (uMsg==WM_SETFOCUS)		return FALSE;

	RC_PAGEINFO* pi	= (RC_PAGEINFO*)GetWindowLong(hWnd, GWL_USERDATA);
	return ::CallWindowProc(pi->pOldButProc, hWnd, uMsg, wParam, lParam);
}


/////////////////////////////////////////////////////////////////////////////
// CRollupCtrl message handlers

//---------------------------------------------------------------------------
// OnCommand
//---------------------------------------------------------------------------
BOOL CRollupCtrl::OnCommand(WPARAM wParam, LPARAM lParam) 
{
	if (LOWORD(wParam)==RC_MID_COLLAPSEALL) {
		ExpandAllPages(FALSE);
		return TRUE;

	} else if (LOWORD(wParam)==RC_MID_EXPANDALL) {
		ExpandAllPages(TRUE);
		return TRUE;

	}	else if (LOWORD(wParam)>=RC_MID_STARTPAGES
		&&	 LOWORD(wParam)<RC_MID_STARTPAGES+GetPagesCount())
	{
		int idx = LOWORD(wParam)-RC_MID_STARTPAGES;
		ExpandPage(idx, !IsPageExpanded(idx) );

	} else if (HIWORD(wParam)==BN_CLICKED) {
		int idx = GetPageIdxFromButtonHWND((HWND)lParam);
		if (idx!=-1) {
			RC_PAGEINFO*	pi = m_PageList[idx];
			if (pi->pwndButton && pi->pwndButton->IsWindowEnabled())
			{
				SetActiveRadio(idx,false);
				//ExpandPage(idx, !pi->bExpanded);
				SetFocus();
				if (global_commander)
					global_commander->SendCommanderMessage(ICommander::CM_SWITCH_ROLLUP_DIALOG,&idx);
				return TRUE;
			}
		}
	}

	return CWnd::OnCommand(wParam, lParam);
}

//---------------------------------------------------------------------------
// OnPaint
//---------------------------------------------------------------------------
void CRollupCtrl::OnPaint() 
{
	CPaintDC dc(this);

    //Draw ScrollBar
	CRect r; GetClientRect(&r);

	//dc.FillSolidRect(r, GetSysColor(COLOR_BTNFACE));

	INT_PTR plSz = m_PageList.GetSize();
	if (plSz==0)
		return;

	CRect groupRect;
	CRect labRect;
	for (INT_PTR i=0;i<plSz;i++)
	{
		if (m_PageList[i]->pwndGroupBox)
		{
			m_PageList[i]->pwndGroupBox->GetWindowRect(groupRect);
			ScreenToClient(groupRect);
			groupRect.top+=6;
		}
		if (m_PageList[i]->pwndButton)
		{
			m_PageList[i]->pwndButton->GetWindowRect(labRect);
			ScreenToClient(labRect);
			groupRect.top = labRect.top+labRect.Height()/2;
			DrawGroupFrame(&dc,groupRect,labRect.left,labRect.right-5);
		}
		else
		{
			if (m_PageList[i]->pwndStatic)
			{
				m_PageList[i]->pwndStatic->GetWindowRect(labRect);
				ScreenToClient(labRect);
				groupRect.top = labRect.top+labRect.Height()/2;
				DrawGroupFrame(&dc,groupRect,labRect.left,labRect.right-5);
			}
		}

	}

	CRect br = CRect(r.right-RC_SCROLLBARWIDTH,r.top, r.right, r.bottom);
	dc.DrawEdge(&br, EDGE_RAISED, BF_RECT  );

	int SB_Pos	= 0;
	int SB_Size = 0;
	int ClientHeight = r.Height()-4;

	if (m_PageHeight>r.Height()) {
		SB_Size = ClientHeight-(((m_PageHeight-r.Height())*ClientHeight)/m_PageHeight);
		SB_Pos	= -(m_StartYPos*ClientHeight)/m_PageHeight;
	} else {
		SB_Size = ClientHeight;
	}

	br.left		+=2;
	br.right	-=1;
	br.top		= SB_Pos+2;
	br.bottom	= br.top+SB_Size;

	dc.FillSolidRect(&br, RC_SCROLLBARCOLOR);
	dc.FillSolidRect(CRect(br.left,2,br.right,br.top), RGB(0,0,0));
	dc.FillSolidRect(CRect(br.left,br.bottom,br.right,2+ClientHeight), RGB(0,0,0));

	// Do not call CWnd::OnPaint() for painting messages
}

//---------------------------------------------------------------------------
// OnSize
//---------------------------------------------------------------------------
void CRollupCtrl::OnSize(UINT nType, int cx, int cy) 
{
	CWnd::OnSize(nType, cx, cy);

	RecalLayout();
	Invalidate();
}

//---------------------------------------------------------------------------
// OnLButtonDown
//---------------------------------------------------------------------------
void CRollupCtrl::OnLButtonDown(UINT nFlags, CPoint point) 
{
	CRect r; GetClientRect(&r);
	if (m_PageHeight>r.Height())
	{

		//Click on scroll bar client rect
		CRect br = CRect(r.right-RC_SCROLLBARWIDTH,r.top, r.right, r.bottom);
		if ((nFlags&MK_LBUTTON) && br.PtInRect(point)) {

			SetCapture();

			int ClientHeight	= r.Height()-4;

			int SB_Size = ClientHeight-(((m_PageHeight-r.Height())*ClientHeight)/m_PageHeight);
			int	SB_Pos	= -(m_StartYPos*ClientHeight)/m_PageHeight;

			//Click inside scrollbar cursor
			if ((point.y<(SB_Pos+SB_Size)) && (point.y>SB_Pos)) {

				m_SBOffset = SB_Pos-point.y+1;

			//Click outside scrollbar cursor (2 cases => above or below cursor)
			} else {
				int distup		= point.y-SB_Pos;	
				int distdown	= (SB_Pos+SB_Size)-point.y;
				if (distup<distdown)	m_SBOffset = 0;					//above
				else									m_SBOffset = -SB_Size;	//below
			}

			//Calc new m_StartYPos from mouse pos
			int TargetPos	= point.y + m_SBOffset;
			m_StartYPos=-(TargetPos*m_PageHeight)/(ClientHeight);

			//Update
			RecalLayout();
		}


		//Click on scroll bar up button
		br = CRect(r.right-RC_SCROLLBARWIDTH,r.top, r.right, r.top);
		if ((nFlags&MK_LBUTTON) && br.PtInRect(point)) {
			m_StartYPos+=32;
			RecalLayout();
		}

		//Click on scroll bar down button
		br = CRect(r.right-RC_SCROLLBARWIDTH,r.bottom, r.right, r.bottom);
		if ((nFlags&MK_LBUTTON) && br.PtInRect(point)) {
			m_StartYPos-=32;
			RecalLayout();
		}
	}

	CWnd::OnLButtonDown(nFlags, point);
}

//---------------------------------------------------------------------------
// OnLButtonUp
//---------------------------------------------------------------------------
void CRollupCtrl::OnLButtonUp(UINT nFlags, CPoint point) 
{
	ReleaseCapture();
	CWnd::OnLButtonUp(nFlags, point);
}


//---------------------------------------------------------------------------
// OnMouseMove
//---------------------------------------------------------------------------
void CRollupCtrl::OnMouseMove(UINT nFlags, CPoint point) 
{
	CRect r; GetClientRect(&r);
	if (m_PageHeight>r.Height())
	{

		if ((nFlags&MK_LBUTTON) && (GetCapture()==this)) {

			//Calc new m_StartYPos from mouse pos
			int ClientHeight	= r.Height() - 4;
			int TargetPos			= point.y + m_SBOffset;
			m_StartYPos=-(TargetPos*m_PageHeight)/ClientHeight;

			//Update
			RecalLayout();
		}

	}
	
	CWnd::OnMouseMove(nFlags, point);
}

//---------------------------------------------------------------------------
// OnMouseWheel
//---------------------------------------------------------------------------
BOOL CRollupCtrl::OnMouseWheel( UINT nFlags, short zDelta, CPoint pt)
{
	m_StartYPos+=(zDelta/4);
	RecalLayout();

	return CWnd::OnMouseWheel(nFlags, zDelta, pt);
}

//---------------------------------------------------------------------------
// OnContextMenu
//---------------------------------------------------------------------------
void CRollupCtrl::OnContextMenu( CWnd* /*pWnd*/, CPoint pos )
{
	//TRACE("CRollupCtrl::OnContextMenu\n");

/*	if (m_cmenuCtxt.m_hMenu)		m_cmenuCtxt.DestroyMenu();

	if (m_cmenuCtxt.CreatePopupMenu())
	{
		GetCursorPos(&pos);	//Cursor position even with keyboard 'Context key'

		//Add all pages with checked style for expanded ones
		for (int i=0; i<m_PageList.GetSize(); i++) {
			CString cstrPageName;
			if (m_PageList[i]->pwndButton)
				m_PageList[i]->pwndButton->GetWindowText(cstrPageName);
			else
				m_PageList[i]->pwndGroupBox->GetWindowText(cstrPageName);
			m_cmenuCtxt.AppendMenu(MF_STRING, RC_MID_STARTPAGES+i, cstrPageName);	
			if (m_PageList[i]->bExpanded)
				m_cmenuCtxt.CheckMenuItem(RC_MID_STARTPAGES+i, MF_CHECKED);
		}

		m_cmenuCtxt.TrackPopupMenu(TPM_LEFTALIGN|TPM_LEFTBUTTON, pos.x, pos.y, this);
	}*/
}
#include "..//MemDC.h"
#define USE_MEM_DC // Comment this out, if you don't want to use CMemDC

BOOL CRollupCtrl::OnEraseBkgnd(CDC* pDC)
{
#ifdef USE_MEM_DC
	CMemDC memDC(pDC);
#else
	CDC* memDC = pDC;
#endif

	CRect rrr;
	GetClientRect(rrr);
	themeData.DrawThemedRect(&memDC,&rrr,FALSE);

	/*GetDlgItem(IDC_GROUP_FRAME)->GetWindowRect(rrr);
	ScreenToClient(rrr);

	CRect parRR;
	GetDlgItem(IDC_PARAMS_LAB)->GetWindowRect(parRR);
	ScreenToClient(parRR);

	DrawGroupFrame(&memDC,rrr,parRR.left-4,parRR.right);*/

	return  TRUE;//CWnd::OnEraseBkgnd(pDC);
}

HBRUSH CRollupCtrl::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	// Call the base class implementation first! Otherwise, it may
	// undo what we are trying to accomplish here.
	HBRUSH hbr = CWnd::OnCtlColor(pDC, pWnd, nCtlColor);

	// Are we painting the IDC_MYSTATIC control? We can use
	// CWnd::GetDlgCtrlID() to perform the most efficient test.

	/*for (int i=0; i<m_PageList.GetSize(); i++)
	{
		RC_PAGEINFO* pi = m_PageList[i];
		if (pWnd==pi->pwndButton ||
			pWnd==pi->pwndGroupBox)
		{
			pDC->SetBkMode(TRANSPARENT);
			//pDC->SetTextColor(GetSysColor(COLOR_WINDOW));
			return (HBRUSH)m_brash;
		}
	}
*/
//	return hbr;


/*	if (pWnd==&m_red_slider ||
		pWnd==&m_green_slider ||
		pWnd==&m_blue_slider ||
		pWnd==&m_transp_slider ||
		pWnd==&m_shininess_slider)
		return hbr;


	if (pWnd==&m_red_edit ||
		pWnd==&m_green_edit ||
		pWnd==&m_blue_edit ||
		pWnd==&m_transp_edit ||
		pWnd==&m_sh_edit ||
		pWnd==&m_count_edit)
	{
		pDC->SetBkMode(TRANSPARENT);
		return brHollow;
	}
*/
	
	if (nCtlColor!=CTLCOLOR_EDIT &&
		nCtlColor!=CTLCOLOR_LISTBOX &&
		nCtlColor!=CTLCOLOR_SCROLLBAR)
	{
		pDC->SetTextColor(0);
		//pDC->SetBkColor(RGB(255,255,255));
		pDC->SetBkMode(TRANSPARENT);
		hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);
	}
	return hbr;
}
