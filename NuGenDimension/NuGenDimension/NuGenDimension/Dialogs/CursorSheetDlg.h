#pragma once
#include "afxwin.h"

#include "..//Controls//ColorPickerCB.h"
#include "..//Tools//Cursorer.h"
// CCursorSheetDlg dialog

class CCursorSheetDlg : public CDialog
{
	DECLARE_DYNAMIC(CCursorSheetDlg)

public:
	CCursorSheetDlg(CCursorer* crsrr, CWnd* pParent = NULL);   // standard constructor
	virtual ~CCursorSheetDlg();

// Dialog Data
	enum { IDD = IDD_CURSOR_SHEET_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
protected:
	virtual void OnCancel();
	virtual void OnOK();
private:
	CCursorer*  m_cursorer;
public:
	CComboBox m_size_combo;
	BOOL m_cur_type;
	CColorPickerCB m_in_color;
	CColorPickerCB m_out_col;

	afx_msg void OnBnClickedCursorTypeRadio1();
	afx_msg void OnBnClickedCursorTypeRadio2();
	afx_msg void OnCbnSelchangeCursorSizeCombo();
	afx_msg void OnCbnSelchangeCursorOutCol();
	afx_msg void OnCbnSelchangeCursorInCol();
	afx_msg void OnPaint();
};
