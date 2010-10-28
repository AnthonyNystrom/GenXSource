// FontPreviewDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "FontPreviewDlgNew.h"
#include ".\fontpreviewdlgnew.h"


// CFontPreviewDlg dialog

IMPLEMENT_DYNAMIC(CFontPreviewDlg, CDialog)
CFontPreviewDlg::CFontPreviewDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CFontPreviewDlg::IDD, pParent)
{
}

CFontPreviewDlg::~CFontPreviewDlg()
{
}

void CFontPreviewDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_FONT_SYMB_LIST, m_symb_list);
	DDX_Control(pDX, IDC_FONTS_NAMES_LIST, m_names_list);
}


BEGIN_MESSAGE_MAP(CFontPreviewDlg, CDialog)
	ON_WM_SIZE()
	ON_WM_CTLCOLOR()
	ON_LBN_SELCHANGE(IDC_FONTS_NAMES_LIST, OnLbnSelchangeFontsNamesList)
END_MESSAGE_MAP()


// CFontPreviewDlg message handlers

void CFontPreviewDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	if (::IsWindow(m_names_list.m_hWnd))
	{
		m_names_list.MoveWindow(3,3,100,cy-6);
		m_symb_list.MoveWindow(106,3,cx-109,cy-6);
	}
}


static DWORD GetTextExtent(HDC hDC, LPCSTR s, int len)
{
	SIZE dim;
	DWORD dw;
	GetTextExtentPoint32(hDC, s, len, &dim);
	dw = ((dim.cy << 16) & 0xFFFF0000)| dim.cx;
	return dw;
}


BOOL CFontPreviewDlg::OnInitDialog()
{
	CDialog::OnInitDialog();


	for (unsigned int kk=0;kk<sgFontManager::GetFontsCount();kk++)
	{
		m_names_list.AddString(sgFontManager::GetFont(kk)->GetFontData()->name);
	}
	m_names_list.SetCurSel(sgFontManager::GetCurrentFont());

	WORD	i, n;
	WORD* tab;

	m_symb_list.ResetContent();
	m_symb_list.SetCurFont(sgFontManager::GetCurrentFont());

	n = sgFontManager::GetFont(sgFontManager::GetCurrentFont())->GetFontData()->table_size;
	tab = (WORD*)sgFontManager::GetFont(sgFontManager::GetCurrentFont())->GetFontData()->symbols_table;
	if(*tab == 0){tab +=2; n -= 1;}
	for(i = 0; i < n; i++)
		SendDlgItemMessage(IDC_FONT_SYMB_LIST, LB_ADDSTRING, 0,
		(LPARAM)*(tab + 2*i));

	CString str;
	CSize   sz;
	int     dx=0;
	CDC*    pDC = m_symb_list.GetDC();
	for (int i=0;i < m_symb_list.GetCount();i++)
	{
		//m_list.GetText( i, str );
		//sz = pDC->GetTextExtent(str);

		m_symb_list.SetItemHeight( i, /*HIWORD(GetTextExtent(pDC->m_hDC,"a",1))+*/ 
			LOWORD(GetTextExtent(pDC->m_hDC, "a",1))*4 + 13 );
	}
	m_symb_list.SetColumnWidth(LOWORD(GetTextExtent(pDC->m_hDC, "a",1))*4 + 13);
	m_symb_list.ReleaseDC(pDC);


	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

HBRUSH CFontPreviewDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	// Call the base class implementation first! Otherwise, it may
	// undo what we are trying to accomplish here.
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	int cID = pWnd->GetDlgCtrlID();

	if (cID == IDC_FONT_SYMB_LIST)
		return (HBRUSH)GetStockObject(GRAY_BRUSH);

	return hbr;}

void CFontPreviewDlg::OnLbnSelchangeFontsNamesList()
{
	WORD	i, n;
	WORD* tab;

	m_symb_list.ResetContent();
	m_symb_list.SetCurFont(m_names_list.GetCurSel());

	n = sgFontManager::GetFont(m_names_list.GetCurSel())->GetFontData()->table_size;
	tab = (WORD*)sgFontManager::GetFont(m_names_list.GetCurSel())->GetFontData()->symbols_table;
	if(*tab == 0){tab +=2; n -= 1;}
	for(i = 0; i < n; i++)
		SendDlgItemMessage(IDC_FONT_SYMB_LIST, LB_ADDSTRING, 0,
		(LPARAM)*(tab + 2*i));
}
