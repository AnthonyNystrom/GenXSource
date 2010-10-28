#if !defined(AFX_BASETABCTRL_H__1E6E4FE9_BE01_4DA1_AFA9_A98527A3769B__INCLUDED_)
#define AFX_BASETABCTRL_H__1E6E4FE9_BE01_4DA1_AFA9_A98527A3769B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// BaseTabCtrl.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CBaseTabCtrl window

class CEGTabCtrl : public CTabCtrl
{
protected:
	BOOL m_bCustomDraw;

// Construction
public:
	CEGTabCtrl();

	BOOL SetCustomDraw( BOOL bValue );

// Color scheme ;)
public:
	COLORREF m_clrBack;
	COLORREF m_clrInactiveTab;
	COLORREF m_clrActiveTab;
	COLORREF m_clrInactiveText;
	COLORREF m_clrActiveText;
	COLORREF m_clr3DLight;
	COLORREF m_clr3DShadow;
	COLORREF m_clrSeparator;


// Implementation
public:
	virtual ~CEGTabCtrl();

// Overrides
protected:
	virtual void DrawHeaderBk( CDC* pDC, CRect* lprcHeader );
	virtual void DrawMainBorder( CDC* pDC, CRect* lprcBorder );
	virtual void DrawItem( CDC* pDC, CRect* lprcBorder, int nTab, BOOL bSelected = FALSE, BOOL bFocused = FALSE );

	virtual void PreSubclassWindow();

// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnPaint();

};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BASETABCTRL_H__1E6E4FE9_BE01_4DA1_AFA9_A98527A3769B__INCLUDED_)
