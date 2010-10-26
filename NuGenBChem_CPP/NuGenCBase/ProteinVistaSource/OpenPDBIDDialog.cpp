// C:\Users\cypark\Development\ProteinInsight\ProteinInsight\GraphicSources\OpenPDBIDDialog.cpp : implementation file
//

#include "stdafx.h"
#include "OpenPDBIDDialog.h"


// COpenPDBIDDialog dialog

IMPLEMENT_DYNAMIC(COpenPDBIDDialog, CDialog)

COpenPDBIDDialog::COpenPDBIDDialog(CWnd* pParent /*=NULL*/)
: CDialog(COpenPDBIDDialog::IDD, pParent)
{

}

COpenPDBIDDialog::~COpenPDBIDDialog()
{
}

void COpenPDBIDDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(COpenPDBIDDialog, CDialog)
	ON_BN_CLICKED(IDOK, &COpenPDBIDDialog::OnBnClickedOk)
END_MESSAGE_MAP()


// COpenPDBIDDialog message handlers

void COpenPDBIDDialog::OnBnClickedOk()
{
	CString strPDBIDEdit;
	GetDlgItemText(IDC_EDIT_PDBID, strPDBIDEdit );

	m_strArrayPDBID.RemoveAll();

	CString resToken;
	int curPos = 0;

	resToken= strPDBIDEdit.Tokenize(_T(";, \n"),curPos);
	while (resToken != _T(""))
	{
		resToken.TrimLeft(_T(";,\r\n "));
		resToken.TrimRight(_T(";,\r\n "));

		if ( resToken.GetLength() == 4 && isdigit(resToken[0]) != 0 )
			m_strArrayPDBID.Add(resToken);
		resToken = strPDBIDEdit.Tokenize(_T(";, \n"), curPos);
	};   

	OnOK();
}
 
