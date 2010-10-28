#if !defined(AFX_EDITORSETTINGSDIALOG_H__A4E7CD33_EE23_4A7A_A3A4_21C4A9565E37__INCLUDED_)
#define AFX_EDITORSETTINGSDIALOG_H__A4E7CD33_EE23_4A7A_A3A4_21C4A9565E37__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// EditorSettingsDialog.h : header file
//
#include "..//resource.h"
#include "ColorStatic.h"

#define MEASUREMENT_PIXEL		0
#define MEASUREMENT_INCH		1
#define MEASUREMENT_CENTIMETER	2

#ifndef round
#define round(a) ( int ) ( a + .5 )
#endif

/////////////////////////////////////////////////////////////////////////////
// CEditorSettingsDialog dialog

class CEditorSettingsDialog : public CDialog
{
// Construction
public:
	CEditorSettingsDialog(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CEditorSettingsDialog)
	enum { IDD = IDD_REPORT_DIALOG_SETTINGS };
	CEdit	m_ctrlWidth;
	CEdit	m_ctrlTop;
	CEdit	m_ctrlRight;
	CEdit	m_ctrlLeft;
	CEdit	m_ctrlHeight;
	CEdit	m_ctrlGridsize;
	CEdit	m_ctrlBottom;
	CColorStatic	m_ctrlMargin;
	CColorStatic	m_ctrlGrid;
	CColorStatic	m_ctrlBg;
	BOOL	m_grid;
	BOOL	m_snap;
	CString	m_editBottom;
	CString	m_editHeight;
	CString	m_editLeft;
	CString	m_editRight;
	CString	m_editTop;
	CString	m_editWidth;
	int		m_restraint;
	int		m_measurements;
	BOOL	m_margins;
	UINT	m_zoom;
	UINT	m_zoomlevel;
	CString m_editGridsize;
	//}}AFX_DATA

	COLORREF	m_colorBg;
	COLORREF	m_colorGrid;
	COLORREF	m_colorMargin;

	int m_left;
	int m_right;
	int m_top;
	int m_bottom;
	int m_width;
	int m_height;
	int m_gridsize;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CEditorSettingsDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CEditorSettingsDialog)
	afx_msg void OnButtonColorBg();
	afx_msg void OnButtonColorGrid();
	afx_msg void OnButtonColorMargin();
	afx_msg void OnButtonDefault();
	virtual BOOL OnInitDialog();
	afx_msg void OnSelchangeComboMeasurements();
	virtual void OnOK();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	int	m_currentMeasurement;

	void	GetMeasurements();
	void	SetMeasurementEdits();

	double m_xres;
	double m_yres;
	double m_cmx;
	double m_cmy;


};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_EDITORSETTINGSDIALOG_H__A4E7CD33_EE23_4A7A_A3A4_21C4A9565E37__INCLUDED_)
