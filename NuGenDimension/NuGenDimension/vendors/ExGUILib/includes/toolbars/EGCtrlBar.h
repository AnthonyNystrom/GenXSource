#pragma once
#include "afxext.h"

#define SPLITTER_SIZE 4

class CEGControlBar :
	public CControlBar
{
protected:
	int m_iSize;
	BOOL m_bDragging;
	CRect m_dragRect;

	HCURSOR m_curHorzDrag;
	HCURSOR m_curVertDrag;

	BOOL IsVertical();
	void SetDragCursor();
    BOOL m_bActive;

	void OnInvertTracker(const CRect& rect);
	void GetInsideRect( CRect& rc );
public:
	CEGControlBar(void);
	~CEGControlBar(void);

	BOOL Create( CWnd * pParent, int nSize, int iId );
	void SetAlign( DWORD dwSide );

	void HidePane();
	void ShowPane();
	void ToggleVisible();
	
	virtual void OnDraw( CDC * pDC, CRect& rc );
	virtual void OnResize( CRect& rc );
		
	// Perform the UI idle updating for the check/radio/select subitems
	virtual CSize CalcFixedLayout ( BOOL bStretch, BOOL bHorz );
	
protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnPaint();
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);
	afx_msg void OnSize(UINT nType, int cx, int cy);
};
