// SetupsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "SetupsDlg.h"
#include "..//MainFrm.h"


// CSetupsDlg dialog

IMPLEMENT_DYNAMIC(CSetupsDlg, CDialog)
CSetupsDlg::CSetupsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSetupsDlg::IDD, pParent)
{
	m_theme_sheet = new CThemeSheetDlg(theApp.m_reg_manager->m_register_settings.interface_theme);
	m_cursor_sheet = new CCursorSheetDlg((static_cast<CMainFrame*>(theApp.m_pMainWnd))->GetCursorer());
}

CSetupsDlg::~CSetupsDlg()
{
}

void CSetupsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_SETUPS_TABS, m_tabs);
}


BEGIN_MESSAGE_MAP(CSetupsDlg, CDialog)
	ON_NOTIFY(TCN_SELCHANGE, IDC_SETUPS_TABS, OnTcnSelchangeSetupsTabs)
	ON_NOTIFY(TCN_SELCHANGING, IDC_SETUPS_TABS, OnTcnSelchangingSetupsTabs)
	ON_BN_CLICKED(IDOK, &CSetupsDlg::OnBnClickedOk)
END_MESSAGE_MAP()


// CSetupsDlg message handlers

BOOL CSetupsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_tabs.SetCustomDraw( TRUE );

	CRect clR;
	m_tabs.GetWindowRect(clR);
	ScreenToClient(clR);
	clR.top+= 25;
	clR.left+=1;
	clR.bottom-=2;
	clR.right -= 1;
	
	TC_ITEM tci; 
	tci.mask = TCIF_TEXT;
	tci.iImage = -1; 
	tci.pszText = "Style UI";
	m_tabs.InsertItem(0, &tci); 
	tci.pszText = "Pointer"; 
	m_tabs.InsertItem(1, &tci);
	tci.mask = TCIF_PARAM;
	tci.lParam = (LPARAM)m_theme_sheet;
	m_tabs.SetItem(0, &tci);
	m_theme_sheet->Create(CThemeSheetDlg::IDD, &m_tabs);
	m_theme_sheet->SetWindowPos(NULL, clR.left, clR.top, clR.Width(), clR.Height(), SWP_NOSIZE | SWP_NOZORDER);
	m_theme_sheet->ShowWindow(SW_SHOW);
	tci.mask = TCIF_PARAM;
	tci.lParam = (LPARAM)m_cursor_sheet;
	m_tabs.SetItem(1, &tci);
	m_cursor_sheet->Create(CCursorSheetDlg::IDD, &m_tabs);
	m_cursor_sheet->SetWindowPos(NULL, clR.left, clR.top, clR.Width(), clR.Height(), SWP_NOSIZE | SWP_NOZORDER);
	m_cursor_sheet->ShowWindow(SW_HIDE);

	m_theme_sheet->MoveWindow(clR.left, clR.top, clR.Width(), clR.Height());
	m_cursor_sheet->MoveWindow(clR.left, clR.top, clR.Width(), clR.Height());

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

BOOL CSetupsDlg::DestroyWindow()
{
	if (m_theme_sheet)
	{
		m_theme_sheet->DestroyWindow();
		delete m_theme_sheet;
	}
	if (m_cursor_sheet)
	{
		m_cursor_sheet->DestroyWindow();
		delete m_cursor_sheet;
	}

	return CDialog::DestroyWindow();
}

void CSetupsDlg::OnTcnSelchangeSetupsTabs(NMHDR *pNMHDR, LRESULT *pResult)
{
	int iTab = m_tabs.GetCurSel();
	TC_ITEM tci;
	tci.mask = TCIF_PARAM;
	m_tabs.GetItem(iTab, &tci);
	CWnd* pWnd = (CWnd *)tci.lParam;
	pWnd->ShowWindow(SW_SHOW); 
	*pResult = 0;
}

void CSetupsDlg::OnTcnSelchangingSetupsTabs(NMHDR *pNMHDR, LRESULT *pResult)
{
	int iTab = m_tabs.GetCurSel();
	TC_ITEM tci;
	tci.mask = TCIF_PARAM;
	m_tabs.GetItem(iTab, &tci);
	CWnd* pWnd = (CWnd *)tci.lParam;
	pWnd->ShowWindow(SW_HIDE); 
	*pResult = 0;
}

void CSetupsDlg::OnBnClickedOk()
{
	m_theme_sheet->UpdateData(true);
	switch (m_theme_sheet->m_radio)
	{
	case 0:
		theApp.m_reg_manager->m_register_settings.interface_theme = CEGMenu::STYLE_ORIGINAL;
		break;
	case 1:
		theApp.m_reg_manager->m_register_settings.interface_theme = CEGMenu::STYLE_XP_2003;
		break;
	case 2:
		theApp.m_reg_manager->m_register_settings.interface_theme = CEGMenu::STYLE_XP;
		break;
	case 3:
		theApp.m_reg_manager->m_register_settings.interface_theme = CEGMenu::STYLE_ICY;
		break;
	}
		
	CEGMenu::SetMenuDrawMode( theApp.m_reg_manager->m_register_settings.interface_theme );
	theApp.m_pMainWnd->ShowWindow(SW_HIDE);
	theApp.m_pMainWnd->ShowWindow(SW_SHOW);

	CMainFrame*  mnFr = static_cast<CMainFrame*>(theApp.m_pMainWnd);
	CURSOR_STRUCTURE *c = mnFr->GetCursorer()->GetCursorStructure();
	m_cursor_sheet->UpdateData();
	c->isRound  = !m_cursor_sheet->m_cur_type;
	c->insideColor  = m_cursor_sheet->m_in_color.GetCurSel();
	c->outsideColor = m_cursor_sheet->m_out_col.GetCurSel();
	c->size =m_cursor_sheet->m_size_combo.GetCurSel()+2;
	mnFr->GetCursorer()->SetCursorStructure(*c);
	
	OnOK();
}
