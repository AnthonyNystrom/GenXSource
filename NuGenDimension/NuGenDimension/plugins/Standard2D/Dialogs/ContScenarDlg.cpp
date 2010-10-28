// ContScenarDlg.cpp : implementation file
//

#include "stdafx.h"
#include "ContScenarDlg.h"
#include ".\contscenardlg.h"


// CContScenarDlg dialog

IMPLEMENT_DYNAMIC(CContScenarDlg, CDialog)
CContScenarDlg::CContScenarDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CContScenarDlg::IDD, pParent)
{
	m_commander = NULL;
}

CContScenarDlg::~CContScenarDlg()
{
}

void CContScenarDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CContScenarDlg, CDialog)
	ON_BN_CLICKED(IDC_CONT_SCENARS_RADIO1, OnBnClickedContScenarsRadio1)
	ON_BN_CLICKED(IDC_CONT_SCENARS_RADIO2, OnBnClickedContScenarsRadio2)
	ON_WM_CTLCOLOR()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()


// CContScenarDlg message handlers

void CContScenarDlg::OnBnClickedContScenarsRadio1()
{
	if (m_commander)
		m_commander->SwitchScenario(0);
	/*else
		ASSERT(0);*/
}

void CContScenarDlg::OnBnClickedContScenarsRadio2()
{
	if (m_commander)
		m_commander->SwitchScenario(1);
	/*else
		ASSERT(0);*/
}

BOOL CContScenarDlg::OnInitDialog()
{
	__super::OnInitDialog();

	((CButton*)GetDlgItem(IDC_CONT_SCENARS_RADIO1))->SetState(TRUE);
	((CButton*)GetDlgItem(IDC_CONT_SCENARS_RADIO2))->SetState(FALSE);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

HBRUSH CContScenarDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = __super::OnCtlColor(pDC, pWnd, nCtlColor);

	pDC->SetTextColor(0);
	//pDC->SetBkColor(RGB(255,255,255));
	pDC->SetBkMode(TRANSPARENT);
	hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);

	// TODO:  Return a different brush if the default is not desired
	return hbr;
}

BOOL CContScenarDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}
