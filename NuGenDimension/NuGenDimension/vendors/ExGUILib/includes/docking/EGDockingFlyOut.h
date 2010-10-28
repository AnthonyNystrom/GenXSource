#if !defined(AFX_DRAWFRAME_H__EF0EC395_D6D3_42D2_B68C_6FAB10C90B58__INCLUDED_)
#define AFX_DRAWFRAME_H__EF0EC395_D6D3_42D2_B68C_6FAB10C90B58__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "StdAfx.h"
#include "EGDockingPane.h"


#define WM_SHOWIT WM_USER + 1
#define WM_SHOWHIDE WM_USER + 2
#define WM_SHOWIT_NOFOCUS WM_USER + 3
#define WM_FOCUS_CHANGED WM_USER + 4

/////////////////////////////////////////////////////////////////////////////
// CDrawFrame window

class CEGFlyOutPane : public CWnd
{
protected:
	CRect m_rcDest;
	CRect m_rcNow;
	CRect	m_rcClose;
	CRect	m_rcPin;
	CRect	m_rcCaption;

	CFont	m_fntThin;

	BOOL m_bClosePressed;
	BOOL m_bCloseHover;

	BOOL m_bPinPressed;
	BOOL m_bPinHover;
	
	volatile BOOL m_bClosingUp;

	void RePaint();
	void DrawTrackBorder( CDC * pDC, CRect rc );
	void RemoveTrackBorder( CRect * pRc );

	CEGDockingPane * m_pPane;
	CEGDockBorder * m_pBorder;
	void Hide();
	void SlideShow( WPARAM wParam, LPARAM lParam );
	
// Construction
public:
	CEGFlyOutPane();
	
// Attributes
public:
	int	m_Size;
	int m_nStyle;

	int m_cyCaption;
	CString m_Title;
	CString m_sFontFace;

	BOOL m_bShowOff;
	BOOL m_bActive;

// Operations
public:
	BOOL Create( CWnd * pParentWnd, const CRect& rc, int nStyle );
	void SetPane( CEGDockingPane * pPane );
	void SetBorder( CEGDockBorder * pBorder );
	void SlideHide( );

// Overrides

// Implementation
public:
	virtual ~CEGFlyOutPane();

	// Generated message map functions
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnPaint();
	afx_msg BOOL OnNcActivate(BOOL bActive);
	afx_msg void OnTimer(UINT nIDEvent);
	#if _MFC_VER < 0x0800  
  afx_msg UINT OnNcHitTest(CPoint point);
#else
  afx_msg LRESULT OnNcHitTest(CPoint point);
#endif
	afx_msg void OnNcCalcSize(BOOL bCalcValidRects, NCCALCSIZE_PARAMS FAR* lpncsp);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnNcPaint();
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnKillFocus( CWnd* pWnd );

	afx_msg LRESULT ShowOff(WPARAM, LPARAM);
	afx_msg LRESULT ShowHide(WPARAM, LPARAM);
	afx_msg LRESULT ShowOffNoFocus(WPARAM, LPARAM);
	afx_msg void OnGetMinMaxInfo( LPMINMAXINFO lpMMI );
	afx_msg LRESULT OnFocusChanged(WPARAM, LPARAM);

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DRAWFRAME_H__EF0EC395_D6D3_42D2_B68C_6FAB10C90B58__INCLUDED_)
