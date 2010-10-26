#pragma once


// CFileDialogExt 대화 상자입니다.

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

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOG_FILE_EXTENSION };

	CStringArray	m_strArrayPDBID;

	BOOL	m_bDownloadPDB;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.
	void	OnDownload();

	DECLARE_MESSAGE_MAP()
};
