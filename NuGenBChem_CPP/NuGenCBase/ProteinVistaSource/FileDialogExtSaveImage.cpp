

#include "stdafx.h"
#include "FileDialogExtSaveImage.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

 
// CFileDialogExtSaveImage 대화 상자입니다.

IMPLEMENT_DYNAMIC(CFileDialogExtSaveImage, CFileDialog)

CFileDialogExtSaveImage::CFileDialogExtSaveImage(BOOL bOpenFileDialog,
							   LPCTSTR lpszDefExt, LPCTSTR lpszFileName, DWORD dwFlags,
							   LPCTSTR lpszFilter, CWnd* pParentWnd)
							   : CFileDialog(bOpenFileDialog, lpszDefExt, lpszFileName,
							   dwFlags, lpszFilter, pParentWnd, 0, FALSE )
{
	m_ofn.Flags |= OFN_ENABLETEMPLATE;
	m_ofn.lpTemplateName = MAKEINTRESOURCE(IDD_DIALOG_FILE_EXTENSION_SAVE_IMAGE);

	m_imageWidthDefault = m_imageWidth = 1280;
	m_imageHeightDefault = m_imageHeight = 800;

	m_comboBoxImageFormat=0;
	m_checkCurrentImageSize= FALSE;
}

CFileDialogExtSaveImage::~CFileDialogExtSaveImage()
{
}

void CFileDialogExtSaveImage::DoDataExchange(CDataExchange* pDX)
{
	CFileDialog::DoDataExchange(pDX);

	DDX_Text(pDX, IDC_EDIT_IMAGE_WIDTH , m_imageWidth);
	DDX_Text(pDX, IDC_EDIT_IMAGE_HEIGHT , m_imageHeight);
	DDX_Check(pDX, IDC_CHECK_CURRENT_WINDOW_SIZE, m_checkCurrentImageSize);
	DDX_CBIndex(pDX, IDC_COMBO_IMAGE_FORMAT, m_comboBoxImageFormat);
}


BEGIN_MESSAGE_MAP(CFileDialogExtSaveImage, CFileDialog)
	ON_BN_CLICKED(IDC_CHECK_CURRENT_WINDOW_SIZE, OnCurrentImageSize)
	ON_BN_CLICKED(IDOK, OnOK)
	ON_WM_CLOSE()
	ON_WM_DESTROY()
END_MESSAGE_MAP()

BOOL CFileDialogExtSaveImage::OnInitDialog()
{
	CComboBox * pComboBox = (CComboBox *)GetDlgItem(IDC_COMBO_IMAGE_FORMAT);
	pComboBox->AddString(_T("PNG File (*.png)"));
	pComboBox->AddString(_T("BMP File (*.bmp)"));
	pComboBox->AddString(_T("JPG File (*.jpg)"));
	pComboBox->AddString(_T("DIB File (*.dib)"));

	UpdateData(FALSE);

	return TRUE;
}

// CFileDialogExtSaveImage 메시지 처리기입니다.
void CFileDialogExtSaveImage::OnCurrentImageSize() 
{
	UpdateData(TRUE);

	if ( m_checkCurrentImageSize == TRUE )
	{
		m_imageWidth = m_imageWidthDefault;
		m_imageHeight = m_imageHeightDefault;
		GetDlgItem(IDC_EDIT_IMAGE_WIDTH)->EnableWindow(FALSE);
		GetDlgItem(IDC_EDIT_IMAGE_HEIGHT)->EnableWindow(FALSE);
		UpdateData(FALSE);
	}
	else
	{
		GetDlgItem(IDC_EDIT_IMAGE_WIDTH)->EnableWindow(TRUE);
		GetDlgItem(IDC_EDIT_IMAGE_HEIGHT)->EnableWindow(TRUE);
	}
}

void CFileDialogExtSaveImage::OnOK()
{
	UpdateData(TRUE);
	CFileDialog::OnOK();
}

void CFileDialogExtSaveImage::OnClose()
{
	UpdateData(TRUE);

	CFileDialog::OnClose();
}

void CFileDialogExtSaveImage::OnDestroy()
{
	UpdateData(TRUE);

	CFileDialog::OnDestroy();

}
