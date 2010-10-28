#if !defined(AFX_EXLISTBOX_H__1E0EF67C_EF0A_4763_A6B4_862F29BC26EE__INCLUDED_)
#define AFX_EXLISTBOX_H__1E0EF67C_EF0A_4763_A6B4_862F29BC26EE__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ExListBox.h : header file
//

extern UINT rwm_EXLISTBOX_DBLCLICK;
extern UINT rwm_EXLISTBOX_DELETE;
extern UINT rwm_EXLISTBOX_SELCHANGE;

/////////////////////////////////////////////////////////////////////////////
// CExListBox window

class CExListBox : public CListBox
{
// Construction
public:
	CExListBox();

// Attributes
public:

// Operations
public:

	int AddString( LPCTSTR str );

// Overrides
	//{{AFX_VIRTUAL(CExListBox)
	protected:
	virtual void PreSubclassWindow();
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CExListBox();

protected:
	//{{AFX_MSG(CExListBox)
	afx_msg void OnDblclk();
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()

private:
	int		m_draggedLine;

};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_EXLISTBOX_H__1E0EF67C_EF0A_4763_A6B4_862F29BC26EE__INCLUDED_)
