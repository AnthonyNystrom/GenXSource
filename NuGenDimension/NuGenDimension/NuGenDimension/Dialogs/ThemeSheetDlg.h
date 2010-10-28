#pragma once


// CThemeSheetDlg dialog

class CThemeSheetDlg : public CDialog
{
	DECLARE_DYNAMIC(CThemeSheetDlg)

public:
	CThemeSheetDlg(UINT themeID,CWnd* pParent = NULL);   // standard constructor
	virtual ~CThemeSheetDlg();

// Dialog Data
	enum { IDD = IDD_THEME_SHEET_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
protected:
	virtual void OnCancel();
	virtual void OnOK();
public:
	afx_msg void OnBnClickedThemeRadio1();
	afx_msg void OnBnClickedThemeRadio2();
	afx_msg void OnBnClickedThemeRadio3();
	afx_msg void OnBnClickedThemeRadio4();
	BOOL m_radio;
private:
	UINT  m_themeID;
	CxImage   m_imgs[4];
public:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnPaint();
	UINT    GetThemeID() {return m_themeID;};
};
