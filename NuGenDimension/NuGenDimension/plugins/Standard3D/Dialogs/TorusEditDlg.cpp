// SphereParamsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "TorusEditDlg.h"

// CTorusParamsDlg dialog

IMPLEMENT_DYNAMIC(CTorusParamsDlg, CDialog)
CTorusParamsDlg::CTorusParamsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CTorusParamsDlg::IDD, pParent)
	, m_merid1_cnt(3)
	, m_merid2_cnt(3)
{
}

CTorusParamsDlg::~CTorusParamsDlg()
{
}

void CTorusParamsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TOR_MERID1_SPIN, m_merid1_spin);
	DDX_Control(pDX, IDC_TOR_MERID2_SPIN, m_merid2_spin);
	DDX_Text(pDX, IDC_TOR_MERID1_EDIT, m_merid1_cnt);
	DDX_Text(pDX, IDC_TOR_MERID2_EDIT, m_merid2_cnt);
}


BEGIN_MESSAGE_MAP(CTorusParamsDlg, CDialog)
	ON_NOTIFY(UDN_DELTAPOS, IDC_TOR_MERID1_SPIN, OnDeltaposMerid1SphSpin)
	ON_NOTIFY(UDN_DELTAPOS, IDC_TOR_MERID2_SPIN, OnDeltaposMerid2SphSpin)
END_MESSAGE_MAP()

void CTorusParamsDlg::OnDeltaposMerid1SphSpin(NMHDR *pNMHDR, LRESULT *pResult)
{
	/*LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	m_merid1_cnt-=pNMUpDown->iDelta;
	if (m_merid1_cnt>36)
		m_merid1_cnt=36;
	if (m_merid1_cnt<3)
		m_merid1_cnt=3;
	UpdateData(false);
	*pResult = 0;*/
}

void CTorusParamsDlg::OnDeltaposMerid2SphSpin(NMHDR *pNMHDR, LRESULT *pResult)
{
	/*LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	m_merid2_cnt-=pNMUpDown->iDelta;
	if (m_merid2_cnt>36)
		m_merid2_cnt=36;
	if (m_merid2_cnt<3)
		m_merid2_cnt=3;
	UpdateData(false);
	*pResult = 0;*/
}


BOOL CTorusParamsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_merid1_spin.SetRange(3,36);
	m_merid2_spin.SetRange(3,36);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
