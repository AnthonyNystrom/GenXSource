#pragma once
#include "afxcmn.h"

#include "ThemeSheetDlg.h"
#include "CursorSheetDlg.h"
// CSetupsDlg dialog

class CSetupsDlg : public CDialog
{
	DECLARE_DYNAMIC(CSetupsDlg)

public:
	CSetupsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSetupsDlg();

// Dialog Data
	enum { IDD = IDD_SETUPS_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CEGTabCtrl m_tabs;
	CThemeSheetDlg*   m_theme_sheet;
	CCursorSheetDlg*   m_cursor_sheet;

public:
	virtual BOOL OnInitDialog();
	virtual BOOL DestroyWindow();
	afx_msg void OnTcnSelchangeSetupsTabs(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnTcnSelchangingSetupsTabs(NMHDR *pNMHDR, LRESULT *pResult);
public:
	afx_msg void OnBnClickedOk();
};
