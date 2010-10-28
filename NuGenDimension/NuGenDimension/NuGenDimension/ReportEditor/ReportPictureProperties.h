#if !defined(AFX_REPORTPICTUREPROPERTIES_H__0CF90F37_2701_475E_AFDF_509A6DCA4320__INCLUDED_)
#define AFX_REPORTPICTUREPROPERTIES_H__0CF90F37_2701_475E_AFDF_509A6DCA4320__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ReportPictureProperties.h : header file
//

#include "../resource.h"
#include "DiagramEditor/DiagramPropertyDlg.h"
#include "ColorStatic.h"


#include "..//Controls//ColorPickerCB.h"
#include "..//Controls//LineThiknessCombo.h"
#include "..//Controls//OptionTree//OptionTree.h"


typedef struct
{
public:
	int nID;
	BOOL bRead;
	BOOL bWrite;
	const char* description;
	const char* ext;
} DocType;


/////////////////////////////////////////////////////////////////////////////
// CReportPictureProperties dialog

class CReportPictureProperties : public CDiagramPropertyDlg
{
// Construction
public:
	CReportPictureProperties(IThumbnailerStorage* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CReportPictureProperties)
	enum { IDD = IDD_REPORT_DIALOG_PICTURE_PROPERTIES };
	
	//}}AFX_DATA

	CString m_filename;
	virtual void SetValues();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CReportPictureProperties)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CReportPictureProperties)
	virtual void OnOK();
	virtual void OnCancel();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	unsigned int	m_borderColor;
	unsigned int	m_borderThickness;
	unsigned int	m_borderStyle;


	COptionTree m_otTree;

	COptionTreeItemFile         *m_otiFile;

	COptionTreeItemCheckBox         *m_otiExistLeft;
	COptionTreeItemCheckBox         *m_otiExistRight;
	COptionTreeItemCheckBox         *m_otiExistTop;
	COptionTreeItemCheckBox         *m_otiExistBottom;
	COptionTreeItemColorComboBox    *m_otiFrameColorCombo;
	COptionTreeItemLineThikComboBox *m_otiFrameThicknessCombo;

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REPORTPICTUREPROPERTIES_H__0CF90F37_2701_475E_AFDF_509A6DCA4320__INCLUDED_)
