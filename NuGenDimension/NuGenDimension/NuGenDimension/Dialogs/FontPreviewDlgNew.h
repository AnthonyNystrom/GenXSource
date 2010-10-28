#pragma once
#include "afxwin.h"

#include "..//Controls//SpecSymbolsListBox.h"
// CFontPreviewDlg dialog

class CFontPreviewDlg : public CDialog
{
	DECLARE_DYNAMIC(CFontPreviewDlg)

public:
	CFontPreviewDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CFontPreviewDlg();

// Dialog Data
	enum { IDD = IDD_FONTS_PREVIEW_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CSpecSymbolsListBox m_symb_list;
	CListBox            m_names_list;
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
	virtual BOOL OnInitDialog();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnLbnSelchangeFontsNamesList();
};
