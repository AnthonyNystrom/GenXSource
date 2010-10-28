#pragma once

class CEGTreeCtrl : public CTreeCtrl
{
	DECLARE_DYNAMIC(CEGTreeCtrl)
	int m_nMenu;
	HMENU m_hMenu;
public:
	CEGTreeCtrl();
	virtual ~CEGTreeCtrl();

	void SetMenuID(int nMenu);
protected:	
	//содержит список изображений используемый  во  время переноса
	CImageList*	m_pDragImage;	
	
	BOOL	m_bLDragging;
	HTREEITEM	m_hitemDrag,m_hitemDrop;
	
	void DeleteBranch(HTREEITEM hItem);
public:
	virtual BOOL OnBeginDrag(HTREEITEM hItem);
	virtual BOOL OnDragOver(HTREEITEM hItemDrag, HTREEITEM hItemDrop);
	virtual BOOL OnFinishDrag(HTREEITEM hItemDrag, HTREEITEM hItemDrop);

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnNMDblclk(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnNMRclick(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnTvnBegindrag(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnMButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnCancelMode();
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
};


