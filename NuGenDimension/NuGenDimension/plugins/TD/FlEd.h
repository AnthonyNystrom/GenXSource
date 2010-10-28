#if !defined(AFX_FLED_H__63FED6ED_C42D_478D_82CC_41065BB99B23__INCLUDED_)
#define AFX_FLED_H__63FED6ED_C42D_478D_82CC_41065BB99B23__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// FloatEdit.h : header file
//
#define  GET_DIALOGS_MESSAGE_POST_SET_FOCUS    WM_USER+103
#define  GET_DIALOGS_MESSAGE_PRE_SET_FOCUS    WM_USER+104
/////////////////////////////////////////////////////////////////////////////
// CFlEd window
// Author: Eugene Dyblenko.
// Purpose: validation of float values in the window.

class CFlEd : public CEdit
{
// Construction
public:
	CFlEd();

private:
// Attributes
public:
	void    SetValue(float val);
	float   GetValue();
// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFlEd)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CFlEd();

	// Generated message map functions
public:
	//{{AFX_MSG(CFlEd)
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnSetFocus(CWnd* pOldWnd);
private:
	afx_msg LRESULT OnPostSetFocus(WPARAM, LPARAM);
	afx_msg LRESULT OnPreSetFocus(WPARAM, LPARAM);
public:
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FLED_H__63FED6ED_C42D_478D_82CC_41065BB99B23__INCLUDED_)
