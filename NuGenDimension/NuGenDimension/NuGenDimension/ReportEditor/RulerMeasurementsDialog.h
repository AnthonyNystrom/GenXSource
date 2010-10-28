#if !defined(AFX_RULERMEASUREMENTSDIALOG_H__7543878E_F028_4DCC_91C9_5355CBFB02EE__INCLUDED_)
#define AFX_RULERMEASUREMENTSDIALOG_H__7543878E_F028_4DCC_91C9_5355CBFB02EE__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// RulerMeasurementsDialog.h : header file
//
#include "..//resource.h"
/////////////////////////////////////////////////////////////////////////////
// CRulerMeasurementsDialog dialog

class CRulerMeasurementsDialog : public CDialog
{
// Construction
public:
	CRulerMeasurementsDialog(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CRulerMeasurementsDialog)
	enum { IDD = IDD_REPORT_DIALOG_RULER_MEASUREMENTS };
	int		m_measurements;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRulerMeasurementsDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CRulerMeasurementsDialog)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RULERMEASUREMENTSDIALOG_H__7543878E_F028_4DCC_91C9_5355CBFB02EE__INCLUDED_)
