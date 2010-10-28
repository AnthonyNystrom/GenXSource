#include "StdAfx.h"
#include "EGMenu.h"
#include "EGTreeCtrl.h"

IMPLEMENT_DYNAMIC(CEGTreeCtrl, CTreeCtrl)
CEGTreeCtrl::CEGTreeCtrl()
{
	m_nMenu = 0;
	m_bLDragging = FALSE;
}

CEGTreeCtrl::~CEGTreeCtrl()
{
}


BEGIN_MESSAGE_MAP(CEGTreeCtrl, CTreeCtrl)
	ON_NOTIFY_REFLECT(NM_DBLCLK, OnNMDblclk)
	ON_NOTIFY_REFLECT(NM_RCLICK, OnNMRclick)
	ON_NOTIFY_REFLECT(TVN_BEGINDRAG, OnTvnBegindrag)
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONUP()
	ON_WM_RBUTTONDOWN()
	ON_WM_MBUTTONDOWN()
	ON_WM_CANCELMODE()
	ON_WM_KEYDOWN()
END_MESSAGE_MAP()



// CEGTreeCtrl message handlers


void CEGTreeCtrl::OnNMDblclk(NMHDR *pNMHDR, LRESULT *pResult)
{
	EditLabel(GetSelectedItem());

	*pResult = 0;
}

void CEGTreeCtrl::SetMenuID(int nMenu){
	m_nMenu = nMenu;
}
void CEGTreeCtrl::OnNMRclick(NMHDR *pNMHDR, LRESULT *pResult)
{
	if ( m_nMenu ) {
		POINT ptMouse;
		::GetCursorPos(&ptMouse);

		CEGMenu mnu;
		mnu.LoadMenu( m_nMenu );
		mnu.LoadToolBar( m_nMenu );
		CEGMenu * pSubMenu = (CEGMenu*)mnu.GetSubMenu(0);
		pSubMenu->TrackPopupMenu(0, ptMouse.x, ptMouse.y, this);
	}

	*pResult = 0;
}

void CEGTreeCtrl::OnTvnBegindrag(NMHDR *pNMHDR, LRESULT *pResult)
{
	NM_TREEVIEW* pNMTreeView = (NM_TREEVIEW*)pNMHDR;
	*pResult = 0;

	if (!OnBeginDrag(pNMTreeView->itemNew.hItem)) return;

	m_hitemDrag = pNMTreeView->itemNew.hItem;
	m_hitemDrop = NULL;

	m_pDragImage = CreateDragImage(m_hitemDrag);  // get the image list for dragging
	// CreateDragImage() возвращает  NULL если нет списка изображаний
	// связанного с деревом
	if( !m_pDragImage )
		return;

	m_bLDragging = TRUE;
	m_pDragImage->BeginDrag(0, CPoint(0,0));
	POINT pt = pNMTreeView->ptDrag;
	ClientToScreen( &pt );
	m_pDragImage->DragEnter(NULL, pt);
	SetFocus();
	SetCapture();
}

void CEGTreeCtrl::OnMouseMove(UINT nFlags, CPoint point)
{
	HTREEITEM	hitem;
	UINT		flags;

	if (m_bLDragging)
	{
		POINT pt = point;
		ClientToScreen( &pt );
		CImageList::DragMove(pt);
		if ((hitem = HitTest(point, &flags)) != NULL)
		{
			CImageList::DragShowNolock(FALSE);
			SelectDropTarget(hitem);
			if (GetParentItem(m_hitemDrag) != m_hitemDrop && OnDragOver(m_hitemDrag,hitem)){
				m_hitemDrop = hitem;
			}else{
				m_hitemDrop = NULL;
			}
			CImageList::DragShowNolock(TRUE);
		}
	}

	CTreeCtrl::OnMouseMove(nFlags, point);
}

void CEGTreeCtrl::OnLButtonUp(UINT nFlags, CPoint point)
{
	CTreeCtrl::OnLButtonUp(nFlags, point);

	if (m_bLDragging)
	{
		m_bLDragging = FALSE;
		CImageList::DragLeave(this);
		CImageList::EndDrag();
		ReleaseCapture();

		delete m_pDragImage;

		// Удаляем подсветку цели
		SelectDropTarget(NULL);

		if (m_hitemDrop==NULL|| // Drop on this item denied
				m_hitemDrag == m_hitemDrop||
				m_hitemDrop==GetParentItem(m_hitemDrag)) 
			return;

		if (OnFinishDrag(m_hitemDrag, m_hitemDrop)) 		
			DeleteBranch(m_hitemDrag);
	}
}

void CEGTreeCtrl::DeleteBranch(HTREEITEM hItem){
	if (ItemHasChildren(hItem)){
		HTREEITEM hChildItem = GetChildItem(hItem);
		while (hChildItem){
			DeleteBranch(hChildItem); 
			hChildItem = GetNextSiblingItem(hChildItem);
		}
	}
	DeleteItem(hItem);
}

BOOL CEGTreeCtrl::OnBeginDrag(HTREEITEM hItem){
	return (BOOL)GetParent()->SendMessage(WM_USER+1,(WPARAM)hItem);
}

BOOL CEGTreeCtrl::OnDragOver(HTREEITEM hItemDrag, HTREEITEM hItemDrop){
	return (BOOL)GetParent()->SendMessage(WM_USER+2,(WPARAM)hItemDrag,(LPARAM)hItemDrop);
}

BOOL CEGTreeCtrl::OnFinishDrag(HTREEITEM hItemDrag, HTREEITEM hItemDrop){
	return (BOOL)GetParent()->SendMessage(WM_USER+3,(WPARAM)hItemDrag,(LPARAM)hItemDrop);
}

void CEGTreeCtrl::OnRButtonDown(UINT nFlags, CPoint point)
{
	if (m_bLDragging)
	{
		m_bLDragging = FALSE;
		CImageList::DragLeave(this);
		CImageList::EndDrag();
		ReleaseCapture();

		delete m_pDragImage;

		// Удаляем подсветку цели
		SelectDropTarget(NULL);
	}

	CTreeCtrl::OnRButtonDown(nFlags, point);
}

void CEGTreeCtrl::OnMButtonDown(UINT nFlags, CPoint point)
{
	if (m_bLDragging)
	{
		m_bLDragging = FALSE;
		CImageList::DragLeave(this);
		CImageList::EndDrag();
		ReleaseCapture();

		delete m_pDragImage;

		// Удаляем подсветку цели
		SelectDropTarget(NULL);
	}

	CTreeCtrl::OnMButtonDown(nFlags, point);
}

void CEGTreeCtrl::OnCancelMode()
{
	CTreeCtrl::OnCancelMode();

	if (m_bLDragging)
	{
		m_bLDragging = FALSE;
		CImageList::DragLeave(this);
		CImageList::EndDrag();
		ReleaseCapture();

		delete m_pDragImage;

		// Удаляем подсветку цели
		SelectDropTarget(NULL);
	}
}

void CEGTreeCtrl::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	if (m_bLDragging)
	{
		if (nChar == VK_ESCAPE){
			m_bLDragging = FALSE;
			CImageList::DragLeave(this);
			CImageList::EndDrag();
			ReleaseCapture();

			delete m_pDragImage;

			// Удаляем подсветку цели
			SelectDropTarget(NULL);
		}
	}else{
		if (nChar == VK_F2)
			EditLabel(GetSelectedItem());
		else
			CTreeCtrl::OnKeyDown(nChar, nRepCnt, nFlags);
	}
}
