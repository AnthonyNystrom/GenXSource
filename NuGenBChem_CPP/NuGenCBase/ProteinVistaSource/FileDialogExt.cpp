// FileDialogExt.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "FileDialogExt.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

// CFileDialogExt 대화 상자입니다.

IMPLEMENT_DYNAMIC(CFileDialogExt, CFileDialog)

CFileDialogExt::CFileDialogExt(BOOL bOpenFileDialog,
							   LPCTSTR lpszDefExt, LPCTSTR lpszFileName, DWORD dwFlags,
							   LPCTSTR lpszFilter, CWnd* pParentWnd)
							   : CFileDialog(bOpenFileDialog, lpszDefExt, lpszFileName,
							   dwFlags, lpszFilter, pParentWnd,0, FALSE )
{
	m_ofn.Flags |= OFN_ENABLETEMPLATE;
	m_ofn.lpTemplateName = MAKEINTRESOURCE(IDD_DIALOG_FILE_EXTENSION);

	m_bDownloadPDB = FALSE;
}

CFileDialogExt::~CFileDialogExt()
{
}

void CFileDialogExt::DoDataExchange(CDataExchange* pDX)
{
	CFileDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CFileDialogExt, CFileDialog)
	ON_BN_CLICKED(IDC_BUTTON_DOWNLOAD_OPEN, OnDownload)
END_MESSAGE_MAP()


// CFileDialogExt 메시지 처리기입니다.
void CFileDialogExt::OnDownload() 
{
	m_bDownloadPDB = TRUE;

	CString strPDBIDEdit;
	GetDlgItemText(IDC_EDIT_PDBID, strPDBIDEdit );


	CString resToken;
	int curPos = 0;

	resToken= strPDBIDEdit.Tokenize(_T(";, \n"),curPos);
	while (resToken != _T(""))
	{
		TRACE(_T("Resulting token: %s\n"), resToken);
		if ( resToken.GetLength() == 4 && isdigit(resToken[0]) != 0 )
			m_strArrayPDBID.Add(resToken);
		resToken = strPDBIDEdit.Tokenize(_T(";, "), curPos);
	};   
	
	GetParent()->SendMessage(WM_COMMAND, IDCANCEL);
}
