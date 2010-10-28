#if !defined(AFX_REPORTBOXPROPERTIES_H__41B4B91F_86F8_4392_BAC9_29169E84A68A__INCLUDED_)
#define AFX_REPORTBOXPROPERTIES_H__41B4B91F_86F8_4392_BAC9_29169E84A68A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ReportBoxProperties.h : header file
//

#include "DiagramEditor/DiagramPropertyDlg.h"
#include "../resource.h"

#include "..//Controls//ColorPickerCB.h"
#include "..//Controls//LineThiknessCombo.h"
#include "..//Controls//OptionTree//OptionTree.h"


/////////////////////////////////////////////////////////////////////////////
// CReportBoxProperties dialog

class CReportBoxProperties : public CDiagramPropertyDlg
{
// Construction
public:
	CReportBoxProperties(IThumbnailerStorage* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CReportBoxProperties)
	enum { IDD = IDD_REPORT_DIALOG_BOX_PROPERTIES };
	
	//}}AFX_DATA

	virtual void SetValues();
	
	BOOL	m_fill;
// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CReportBoxProperties)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CReportBoxProperties)
	virtual void OnOK();
	virtual void OnCancel();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	unsigned int	m_borderColor;
	unsigned int	m_borderThickness;
	unsigned int	m_borderStyle;


	unsigned int	m_fillColor;
	COptionTree m_otTree;

	COptionTreeItemCheckBox         *m_otiExistFill;
	COptionTreeItemColorComboBox    *m_otiFillColorCombo;

	COptionTreeItemCheckBox         *m_otiExistLeft;
	COptionTreeItemCheckBox         *m_otiExistRight;
	COptionTreeItemCheckBox         *m_otiExistTop;
	COptionTreeItemCheckBox         *m_otiExistBottom;
	COptionTreeItemColorComboBox    *m_otiFrameColorCombo;
	COptionTreeItemLineThikComboBox *m_otiFrameThicknessCombo;

	//COptionTreeItemCheckBox         *m_otiGabVis_check;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REPORTBOXPROPERTIES_H__41B4B91F_86F8_4392_BAC9_29169E84A68A__INCLUDED_)
