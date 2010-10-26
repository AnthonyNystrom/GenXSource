#pragma once


// CFileDialogExt ��ȭ �����Դϴ�.

class CFileDialogExt : public CFileDialog
{
	DECLARE_DYNAMIC(CFileDialogExt)

public:
	CFileDialogExt(BOOL bOpenFileDialog, 
		LPCTSTR lpszDefExt = NULL,
		LPCTSTR lpszFileName = NULL,
		DWORD dwFlags = OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,
		LPCTSTR lpszFilter = NULL,
		CWnd* pParentWnd = NULL);

	virtual ~CFileDialogExt();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_DIALOG_FILE_EXTENSION };

	CStringArray	m_strArrayPDBID;

	BOOL	m_bDownloadPDB;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.
	void	OnDownload();

	DECLARE_MESSAGE_MAP()
};
