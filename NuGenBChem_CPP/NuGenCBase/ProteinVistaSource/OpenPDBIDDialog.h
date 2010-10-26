#pragma once


// COpenPDBIDDialog dialog

class COpenPDBIDDialog : public CDialog
{
	DECLARE_DYNAMIC(COpenPDBIDDialog)

public:
	COpenPDBIDDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~COpenPDBIDDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_OPENPDBID };

	CStringArray	m_strArrayPDBID;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
};
