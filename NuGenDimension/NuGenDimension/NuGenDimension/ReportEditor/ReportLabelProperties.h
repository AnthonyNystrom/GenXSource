#if !defined(AFX_REPORTLABELPROPERTIES_H__CCB51690_71C2_4FF0_9A06_6055EBAAF76F__INCLUDED_)
#define AFX_REPORTLABELPROPERTIES_H__CCB51690_71C2_4FF0_9A06_6055EBAAF76F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ReportLabelProperties.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CReportLabelProperties dialog

#include "DiagramEditor/DiagramPropertyDlg.h"
#include "../resource.h"

#include "..//Controls//ColorPickerCB.h"
#include "..//Controls//LineThiknessCombo.h"
#include "..//Controls//OptionTree//OptionTree.h"

class CReportLabelProperties : public CDiagramPropertyDlg
{
// Construction
public:
	CReportLabelProperties(IThumbnailerStorage* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CReportLabelProperties)
	enum { IDD = IDD_REPORT_DIALOG_LABEL_PROPERTIES };
	
	//}}AFX_DATA

	unsigned int		m_justification;
	int                 m_angle;
	virtual void SetValues();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CReportLabelProperties)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CReportLabelProperties)
	virtual void OnOK();
	virtual void OnCancel();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:

	LOGFONT		m_lf;
	COLORREF	m_color;

	unsigned int	m_borderColor;
	unsigned int	m_borderThickness;
	unsigned int	m_borderStyle;

	COptionTree m_otTree;

	COptionTreeItemFont    *m_otiFont;
	COptionTreeItemEdit    *m_text_edit;

	COptionTreeItemRadio   *m_otiAlign_radio;
	COptionTreeItemRadio   *m_otiAngle_radio;

	COptionTreeItemCheckBox         *m_otiExistLeft;
	COptionTreeItemCheckBox         *m_otiExistRight;
	COptionTreeItemCheckBox         *m_otiExistTop;
	COptionTreeItemCheckBox         *m_otiExistBottom;
	COptionTreeItemColorComboBox    *m_otiFrameColorCombo;
	COptionTreeItemLineThikComboBox *m_otiFrameThicknessCombo;




};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REPORTLABELPROPERTIES_H__CCB51690_71C2_4FF0_9A06_6055EBAAF76F__INCLUDED_)
