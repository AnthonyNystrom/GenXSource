// RadioListBox.cpp : implementation file
//

#include "stdafx.h"
#include "SelectingListCtrl.h"
#include ".\selectinglistctrl.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSelectingListCtrl

CSelectingListCtrl::CSelectingListCtrl()
{
}

CSelectingListCtrl::~CSelectingListCtrl()
{
}

BEGIN_MESSAGE_MAP(CSelectingListCtrl, CListCtrl)
	//{{AFX_MSG_MAP(CSelectingListCtrl)
	ON_WM_CTLCOLOR_REFLECT()
	//}}AFX_MSG_MAP
	ON_WM_CHAR()
	ON_WM_LBUTTONDOWN()
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CSelectingListCtrl message handlers
void  CSelectingListCtrl::SetMultiSelectMode(bool msm)
{

	DWORD dwStyle = GetWindowLong(m_hWnd, GWL_STYLE); 
    if (msm) 
		dwStyle &= ~(LVS_SINGLESEL); 
	else
		dwStyle |= LVS_SINGLESEL;
	
	SetWindowLong( m_hWnd, GWL_STYLE, dwStyle);

}


void CSelectingListCtrl::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct) 
{
     CDC* pDC = CDC::FromHandle(lpDrawItemStruct->hDC);

	 DWORD dwStyle = GetWindowLong(m_hWnd, GWL_STYLE);

	 bool isMultiSelect = !(dwStyle & LVS_SINGLESEL);

     // just draws focus rectangle when listbox is empty
     if (lpDrawItemStruct->itemID == (UINT)-1)
     {
		/* if (this->IsWindowEnabled())
		 {
          if (lpDrawItemStruct->itemAction & ODA_FOCUS)
               pDC->DrawFocusRect(&lpDrawItemStruct->rcItem);
		 }*/
          return;
     }
     else
     {
          int selChange   = lpDrawItemStruct->itemAction & ODA_SELECT;
          int focusChange = lpDrawItemStruct->itemAction & ODA_FOCUS;
          int drawEntire  = lpDrawItemStruct->itemAction & ODA_DRAWENTIRE;

          if (selChange || drawEntire) {
               BOOL sel = lpDrawItemStruct->itemState & ODS_SELECTED;

               // Draws background rectangle
			   if (this->IsWindowEnabled())
			   {
					pDC->FillSolidRect(&lpDrawItemStruct->rcItem, 
					  ::GetSysColor((GetExStyle()&WS_EX_TRANSPARENT)?COLOR_BTNFACE:COLOR_WINDOW));
			   }
			   else
			   {
				   pDC->FillSolidRect(&lpDrawItemStruct->rcItem,GetSysColor(COLOR_BTNFACE));
			   }

			   // Draw radio button
			   int h = lpDrawItemStruct->rcItem.bottom - lpDrawItemStruct->rcItem.top;
               CRect rect(lpDrawItemStruct->rcItem.left+1, lpDrawItemStruct->rcItem.top+1, 
				   lpDrawItemStruct->rcItem.left+h-2, lpDrawItemStruct->rcItem.top+h-2);
				
			   UINT control = (isMultiSelect)?DFCS_BUTTONCHECK:DFCS_BUTTONRADIO;
			   if (this->IsWindowEnabled())
			      pDC->DrawFrameControl(&rect, DFC_BUTTON, control | (sel?DFCS_CHECKED:0));
			   else
				  pDC->DrawFrameControl(&rect, DFC_BUTTON, control | (sel?DFCS_CHECKED:0) |  DFCS_INACTIVE);	
               // Draws item text
               pDC->SetTextColor(COLOR_WINDOWTEXT);
               pDC->SetBkMode(TRANSPARENT);
               lpDrawItemStruct->rcItem.left += h;
			   CString itT;
			   //GetText(lpDrawItemStruct->itemID,itT);
			   itT = GetItemText(lpDrawItemStruct->itemID,0);
			   if (this->IsWindowEnabled()) 
				pDC->DrawText(/*(LPCTSTR)lpDrawItemStruct->itemData*/itT, &lpDrawItemStruct->rcItem, DT_LEFT);
			   else
			   {
				   COLORREF oldCol  = pDC->GetTextColor();
				   COLORREF col = RGB(GetRValue(::GetSysColor(COLOR_BTNFACE))-20,
					   GetGValue(::GetSysColor(COLOR_BTNFACE))-20,
					   GetBValue(::GetSysColor(COLOR_BTNFACE))-20);
				   pDC->SetTextColor(col);
				   pDC->DrawText(/*(LPCTSTR)lpDrawItemStruct->itemData*/itT, &lpDrawItemStruct->rcItem, DT_LEFT);
				   pDC->SetTextColor(oldCol);
			   }
          }
		  // draws focus rectangle
		 /* if (this->IsWindowEnabled())
             if (focusChange || (drawEntire && (lpDrawItemStruct->itemState & ODS_FOCUS)))
               pDC->DrawFocusRect(&lpDrawItemStruct->rcItem);*/
     }
}

HBRUSH CSelectingListCtrl::CtlColor(CDC* pDC, UINT nCtlColor) 
{
    // If transparent style selected...
	if (nCtlColor==CTLCOLOR_LISTBOX)
	{
		if ( (GetExStyle()&WS_EX_TRANSPARENT))
			return (HBRUSH)::GetSysColorBrush(COLOR_BTNFACE);
		else
		{
			if (!this->IsWindowEnabled())
				return (HBRUSH)::GetSysColorBrush(COLOR_BTNFACE);
			else
				return (HBRUSH)::GetSysColorBrush(COLOR_WINDOW);
		}
	}


	return NULL;
}

void CSelectingListCtrl::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	// TODO: Add your message handler code here and/or call default
	
	//CListCtrl::OnChar(nChar, nRepCnt, nFlags);
}


void CSelectingListCtrl::OnLButtonDown(UINT nFlags, CPoint point)
{
	//CListCtrl::OnLButtonDown(nFlags,point);

	LVHITTESTINFO lvhti;
	int nItem;
	UINT nState;
	lvhti.pt = point;
	nItem = SubItemHitTest(&lvhti);
	if (nItem == -1)
		return;
	nState = GetItemState(nItem, LVIS_SELECTED);

	DWORD dwStyle = GetWindowLong(m_hWnd, GWL_STYLE);
	bool isMultiSelect = !(dwStyle & LVS_SINGLESEL);

		if (nState & LVIS_SELECTED)
			SetItemState(nItem, isMultiSelect?0:LVIS_SELECTED, LVIS_SELECTED);
		else
			SetItemState(nItem, LVIS_FOCUSED | LVIS_SELECTED, LVIS_FOCUSED 
			| LVIS_SELECTED);
	
	NMITEMACTIVATE nm;
	memset(&nm,0,sizeof(NMITEMACTIVATE));
	nm.iItem = nItem;
	nm.hdr.hwndFrom = this->m_hWnd;
	nm.hdr.code = NM_CLICK;
	GetParent()->SendMessage(WM_NOTIFY,0,(LPARAM)&nm);
}
