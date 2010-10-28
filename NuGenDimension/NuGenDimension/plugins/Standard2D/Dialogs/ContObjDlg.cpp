// ContObjDlg.cpp : implementation file
//

#include "stdafx.h"
#include "ContObjDlg.h"
#include ".\contobjdlg.h"


// CContObjDlg dialog

IMPLEMENT_DYNAMIC(CContObjDlg, CDialog)
CContObjDlg::CContObjDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CContObjDlg::IDD, pParent)
{
	m_commander = NULL;
}

CContObjDlg::~CContObjDlg()
{
}

void CContObjDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CContObjDlg, CDialog)
	ON_BN_CLICKED(IDC_CONT_OBJ_RADIO1, OnBnClickedContObjRadio1)
	ON_BN_CLICKED(IDC_CONT_OBJ_RADIO2, OnBnClickedContObjRadio2)
	ON_WM_CTLCOLOR()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

void   CContObjDlg::EnableLineType(bool enbl)
{
	GetDlgItem(IDC_CONT_OBJ_RADIO1)->EnableWindow(enbl);
}
// CContObjDlg message handlers

void CContObjDlg::OnBnClickedContObjRadio1()
{
	if (m_commander)
		m_commander->SwitchObjectType(0);
}

void CContObjDlg::OnBnClickedContObjRadio2()
{
	if (m_commander)
		m_commander->SwitchObjectType(1);
}

BOOL CContObjDlg::OnInitDialog()
{
	__super::OnInitDialog();

	((CButton*)GetDlgItem(IDC_CONT_OBJ_RADIO1))->SetState(TRUE);
	((CButton*)GetDlgItem(IDC_CONT_OBJ_RADIO2))->SetState(FALSE);
	
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

HBRUSH CContObjDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = __super::OnCtlColor(pDC, pWnd, nCtlColor);

	pDC->SetTextColor(0);
	//pDC->SetBkColor(RGB(255,255,255));
	pDC->SetBkMode(TRANSPARENT);
	hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);

	// TODO:  Return a different brush if the default is not desired
	return hbr;
}

BOOL CContObjDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}
