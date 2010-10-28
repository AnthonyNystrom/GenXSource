#pragma once
#include "afxext.h"

class CEGPaneBar :
	public CControlBar
{
	HWND m_hPane;

	int m_iSize;
	BOOL m_bDragging;
	CRect m_dragRect;

	HCURSOR m_curHorzDrag;
	HCURSOR m_curVertDrag;
	
	TCHAR* m_pszCaption;
	HFONT m_fntCaption;

	HFONT m_fntXButton;
	BOOL m_bClosePressed;
	BOOL m_bCloseHover;

	BOOL IsVertical();
	void SetDragCursor();
    BOOL m_bActive;

	CRect GetCaptionRect();
	CRect GetCloseButtonRect();

	void InvalidateCaption();
	void OnInvertTracker(const CRect& rect);

public:
	CEGPaneBar(void);
	~CEGPaneBar(void);

	void SetPane( HWND hPane );
	BOOL Create(TCHAR* pszCaption, CWnd * pParent, int nSize, int iId);
	void SetAlign( DWORD dwSide );
	void SetCaption( TCHAR* pszCaption );

	void HidePane();
	void ShowPane();
	void ToggleVisible();

		
	// Perform the UI idle updating for the check/radio/select subitems
	virtual void OnUpdateCmdUI(CFrameWnd* pTarget, BOOL bDisableIfNoHndler);
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
	afx_msg void OnSetFocus( CWnd* pOldWnd );
};
