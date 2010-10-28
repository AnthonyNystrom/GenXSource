#if !defined(AFX_REPORTLINEPROPERTIES_H__5296DFB9_BAF5_4D6F_B01A_7B15548A9D44__INCLUDED_)
#define AFX_REPORTLINEPROPERTIES_H__5296DFB9_BAF5_4D6F_B01A_7B15548A9D44__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ReportLineProperties.h : header file
//

#include "DiagramEditor/DiagramPropertyDlg.h"
#include "../resource.h"

#include "..//Controls//ColorPickerCB.h"
#include "..//Controls//LineThiknessCombo.h"
#include "..//Controls//OptionTree//OptionTree.h"


/////////////////////////////////////////////////////////////////////////////
// CReportLineProperties dialog

class CReportLineProperties : public CDiagramPropertyDlg
{
// Construction
public:
	CReportLineProperties(IThumbnailerStorage* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CReportLineProperties)
	enum { IDD = IDD_REPORT_DIALOG_LINE_PROPERTIES };
	unsigned int       m_thickness;
	//}}AFX_DATA

	virtual void SetValues();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CReportLineProperties)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CReportLineProperties)
	virtual void OnOK();
	virtual void OnCancel();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	unsigned int m_color;

	COptionTree m_otTree;

	COptionTreeItemColorComboBox    *m_otiColorCombo;
	COptionTreeItemLineThikComboBox *m_otiThicknessCombo;

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REPORTLINEPROPERTIES_H__5296DFB9_BAF5_4D6F_B01A_7B15548A9D44__INCLUDED_)
