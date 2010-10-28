// ThemeSheetDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "ThemeSheetDlg.h"
#include ".\themesheetdlg.h"



// CThemeSheetDlg dialog

IMPLEMENT_DYNAMIC(CThemeSheetDlg, CDialog)
CThemeSheetDlg::CThemeSheetDlg(UINT themeID,CWnd* pParent /*=NULL*/)
	: CDialog(CThemeSheetDlg::IDD, pParent)
	, m_radio(FALSE)
{
	m_themeID = themeID;
	switch(m_themeID) 
	{
	case CEGMenu::STYLE_ORIGINAL:
		m_radio = 0;
		break;
	case CEGMenu::STYLE_XP_2003:
		m_radio = 1;
		break;
	case CEGMenu::STYLE_XP:
		m_radio = 2;
		break;
	case CEGMenu::STYLE_ICY:
		m_radio = 3;
		break;
	default:
		ASSERT(0);
	}
}

CThemeSheetDlg::~CThemeSheetDlg()
{
}

void CThemeSheetDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Radio(pDX, IDC_THEME_RADIO1, m_radio);
}


BEGIN_MESSAGE_MAP(CThemeSheetDlg, CDialog)
	ON_BN_CLICKED(IDC_THEME_RADIO1, OnBnClickedThemeRadio1)
	ON_BN_CLICKED(IDC_THEME_RADIO2, OnBnClickedThemeRadio2)
	ON_BN_CLICKED(IDC_THEME_RADIO3, OnBnClickedThemeRadio3)
	ON_BN_CLICKED(IDC_THEME_RADIO4, OnBnClickedThemeRadio4)
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
END_MESSAGE_MAP()


// CThemeSheetDlg message handlers

BOOL CThemeSheetDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	APP_SWITCH_RESOURCE
	
	m_imgs[0].CreateFromHBITMAP(::LoadBitmap(AfxGetInstanceHandle(),
		MAKEINTRESOURCE(IDB_TH_CLAS)));
	m_imgs[1].CreateFromHBITMAP(::LoadBitmap(AfxGetInstanceHandle(),
		MAKEINTRESOURCE(IDB_TH_2003)));
	m_imgs[2].CreateFromHBITMAP(::LoadBitmap(AfxGetInstanceHandle(),
		MAKEINTRESOURCE(IDB_TH_XP)));
	m_imgs[3].CreateFromHBITMAP(::LoadBitmap(AfxGetInstanceHandle(),
		MAKEINTRESOURCE(IDB_TH_ICY)));
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CThemeSheetDlg::OnCancel()
{
}

void CThemeSheetDlg::OnOK()
{
}

void CThemeSheetDlg::OnBnClickedThemeRadio1()
{
	m_themeID =  CEGMenu::STYLE_ORIGINAL;
	CRect rrr;
	GetDlgItem(IDC_THEME_PIC_FRAME)->GetWindowRect(rrr);
	ScreenToClient(rrr);
	InvalidateRect(rrr);
}

void CThemeSheetDlg::OnBnClickedThemeRadio2()
{
	m_themeID =   CEGMenu::STYLE_XP_2003;
	CRect rrr;
	GetDlgItem(IDC_THEME_PIC_FRAME)->GetWindowRect(rrr);
	ScreenToClient(rrr);
	InvalidateRect(rrr);
}

void CThemeSheetDlg::OnBnClickedThemeRadio3()
{
	m_themeID =   CEGMenu::STYLE_XP;
	CRect rrr;
	GetDlgItem(IDC_THEME_PIC_FRAME)->GetWindowRect(rrr);
	ScreenToClient(rrr);
	InvalidateRect(rrr);
}

void CThemeSheetDlg::OnBnClickedThemeRadio4()
{
	m_themeID =   CEGMenu::STYLE_ICY;
	CRect rrr;
	GetDlgItem(IDC_THEME_PIC_FRAME)->GetWindowRect(rrr);
	ScreenToClient(rrr);
	InvalidateRect(rrr);
}

#include "..//MemDC.h"
#define USE_MEM_DC // Comment this out, if you don't want to use CMemDC

BOOL CThemeSheetDlg::OnEraseBkgnd(CDC* pDC)
{/*
#ifdef USE_MEM_DC
	CMemDC memDC(pDC);
#else
	CDC* memDC = pDC;
#endif

	CRect rrr;
	GetClientRect(rrr);
	themeData.DrawThemedRect(&memDC,&rrr,FALSE);
*/
	return CDialog::OnEraseBkgnd(pDC);
}

void CThemeSheetDlg::OnPaint()
{
	CPaintDC dc(this); // device context for painting
/*
#ifdef USE_MEM_DC
	CMemDC memDC(&dc);
#else
	CDC* memDC = &dc;
#endif
*/
	CRect rrr;
	GetDlgItem(IDC_THEME_PIC_FRAME)->GetWindowRect(rrr);
	ScreenToClient(rrr);

	CSize sz(m_imgs[0].GetWidth(),m_imgs[0].GetHeight());

	char imN = 0;

	switch(m_themeID) 
	{
	case CEGMenu::STYLE_ORIGINAL:
		imN = 0;
		break;
	case CEGMenu::STYLE_XP_2003:
		imN = 1;
		break;
	case CEGMenu::STYLE_XP:
		imN = 2;
		break;
	case CEGMenu::STYLE_ICY:
		imN = 3;
		break;
	default:
		ASSERT(0);
	}

	m_imgs[imN].Draw(dc, FitFirstRectToSecond(sz,rrr));

}
