// D:\Postdoc\ProteinVistaVer2\ProteinVista\ProteinVistaSource\SaveMovieDialog.cpp : implementation file
//

#include "stdafx.h"
#include "ProteinVista.h"
#include "SaveMovieDialog.h"


// CSaveMovieDialog dialog

IMPLEMENT_DYNAMIC(CSaveMovieDialog, CDialog)

CSaveMovieDialog::CSaveMovieDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CSaveMovieDialog::IDD, pParent)
{
	m_imageWidth = 1280;
	m_imageHeight = 800;

	m_fps = 10;

	m_checkCurrentImageSize= FALSE;
}

CSaveMovieDialog::~CSaveMovieDialog()
{
}

void CSaveMovieDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);

	DDX_Text(pDX, IDC_EDIT_MOVIE_FILENAME , m_strMovieFilename);
	DDX_Text(pDX, IDC_LIST_BITMAP_FILENAME , m_imageHeight);

	DDX_Check(pDX, IDC_CHECK_CURRENT_WINDOW_SIZE, m_checkCurrentImageSize);
	DDX_Text(pDX, IDC_EDIT_IMAGE_WIDTH , m_imageWidth);
	DDX_Text(pDX, IDC_EDIT_IMAGE_HEIGHT , m_imageHeight);

	DDX_Text(pDX, IDC_EDIT_FPS, m_fps);
}

BEGIN_MESSAGE_MAP(CSaveMovieDialog, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_BROWSE, OnButtonSaveFilenameBrowse)
	ON_BN_CLICKED(IDC_BUTTON_INSERT, OnButtonInsert)
	ON_BN_CLICKED(IDC_BUTTON_REMOVE, OnButtonRemove)
	ON_BN_CLICKED(IDC_BUTTON_UP, OnButtonUp)
	ON_BN_CLICKED(IDC_BUTTON_DOWN, OnButtonDown)
	ON_BN_CLICKED(IDC_CHECK_CURRENT_WINDOW_SIZE, OnButtonCheckMovieSize)
END_MESSAGE_MAP()


// CSaveMovieDialog message handlers
void CSaveMovieDialog::OnButtonSaveFilenameBrowse()
{
	static char szFilter[] = "WMV File (*.wmv)|*.wmv|All Files (*.*)|*.*||";
	CFileDialog openDialog(TRUE, "wmv", NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT , szFilter , this);
	if ( openDialog.DoModal() == IDOK )
	{
		m_strMovieFilename = openDialog.GetPathName();
		UpdateData(FALSE);
	}
}

void CSaveMovieDialog::OnButtonInsert()
{
	static char szFilter[] = "PNG File (*.png)|*.png|BMP File (*.bmp)|*.bmp|JPG File (*.jpg)|*.jpg|DIB File (*.dib)|*.dib|All Files (*.*)|*.*||";
	CFileDialog openDialog(TRUE, "png", NULL, OFN_ALLOWMULTISELECT| OFN_HIDEREADONLY , szFilter , this);

	if ( openDialog.DoModal() == IDOK )
	{
		CListBox * pListBox = (CListBox *)GetDlgItem(IDC_LIST_BITMAP_FILENAME);

		POSITION pos = openDialog.GetStartPosition();
		while(pos != NULL)
		{
			CString name = openDialog.GetNextPathName(pos);
			POSITION pos = openDialog.GetStartPosition();

			pListBox->AddString(name);
		}
	}
}

void CSaveMovieDialog::OnButtonRemove()
{
	CListBox * pListBox = (CListBox *)GetDlgItem(IDC_LIST_BITMAP_FILENAME);

	for (int i=0;i < pListBox->GetCount();i++)
	{
		if ( pListBox->GetSel(i) > 0 )
		{	
			//    selected.
			pListBox->DeleteString(i);
			i--;
		}
	}
}

void CSaveMovieDialog::OnButtonUp()
{
	CListBox * pListBox = (CListBox *)GetDlgItem(IDC_LIST_BITMAP_FILENAME);
	if ( pListBox->GetSelCount() != 1 )
		return;

	if ( pListBox->GetCurSel() <= 0 )
		return;

	int sel = pListBox->GetCurSel();
	CString currentText;
	pListBox->GetText (sel, currentText );
	pListBox->InsertString(sel-1 , currentText);
	pListBox->DeleteString(sel+1);

	pListBox->SetSel(sel-1, TRUE);
}

void CSaveMovieDialog::OnButtonDown()
{
	CListBox * pListBox = (CListBox *)GetDlgItem(IDC_LIST_BITMAP_FILENAME);
	if ( pListBox->GetSelCount() != 1 )
		return;

	if ( pListBox->GetCurSel() >=  pListBox->GetCount()-1 )
		return;

	int sel = pListBox->GetCurSel();
	CString currentText;
	pListBox->GetText (sel, currentText);
	pListBox->DeleteString(sel);
	pListBox->InsertString(sel+1 , currentText);
	
	pListBox->SetSel(sel+1, TRUE);
}

void CSaveMovieDialog::OnButtonCheckMovieSize()
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

void CSaveMovieDialog::OnOK()
{
	CListBox * pListBox = (CListBox *)GetDlgItem(IDC_LIST_BITMAP_FILENAME);

	m_strArrayFilename.RemoveAll();

	for (int i=0;i < pListBox->GetCount();i++)
	{
		CString strText;
		pListBox->GetText(i, strText);
		m_strArrayFilename.Add(strText);
	}

	CDialog::OnOK();
}
