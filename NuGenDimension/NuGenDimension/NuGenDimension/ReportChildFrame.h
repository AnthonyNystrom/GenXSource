// ChildFrm.h : interface of the CChildFrame class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_CHILDFRM_H__211C8F6B_7C08_4EC5_9A73_7B5D9189C89F__INCLUDED_)
#define AFX_CHILDFRM_H__211C8F6B_7C08_4EC5_9A73_7B5D9189C89F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Dialogs//ReportPagesPreviewDlg.h"

class CReportChildFrame : public CMDIChildWnd
{
	DECLARE_DYNCREATE(CReportChildFrame)
public:
	CReportChildFrame();

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CReportChildFrame)
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	//}}AFX_VIRTUAL
private:
	CEGButtonsBar			m_wndPreviewPanel_Container;
public:
	CReportPagesPreviewDlg  m_preview_panel;
// Implementation
public:
	virtual ~CReportChildFrame();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

// Generated message map functions
protected:
	//{{AFX_MSG(CReportChildFrame)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnMDIActivate(BOOL bActivate, CWnd* pActivateWnd, CWnd* pDeactivateWnd);
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CHILDFRM_H__211C8F6B_7C08_4EC5_9A73_7B5D9189C89F__INCLUDED_)
