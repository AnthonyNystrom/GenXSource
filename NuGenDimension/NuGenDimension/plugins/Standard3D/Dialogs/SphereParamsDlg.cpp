// SphereParamsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "SphereParamsDlg.h"
#include ".\sphereparamsdlg.h"


// CSphereParamsDlg dialog

IMPLEMENT_DYNAMIC(CSphereParamsDlg, CDialog)
CSphereParamsDlg::CSphereParamsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSphereParamsDlg::IDD, pParent)
	, m_merid_cnt(3)
	, m_paral_cnt(3)
	, m_type(CSphereParamsDlg::SPHERE_PARAMS)
{
}

CSphereParamsDlg::~CSphereParamsDlg()
{
}

void CSphereParamsDlg::SetDlgType(DLG_TYPE tp)
{
	m_type = tp;
}

void CSphereParamsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_MERID_SPH_SPIN, m_merid_spin);
	DDX_Control(pDX, IDC_PARAL_SPH_SPIN, m_paral_spin);
	DDX_Text(pDX, IDC_SPH_MERID_EDIT, m_merid_cnt);
	DDX_Text(pDX, IDC_SPH_PARAL_EDIT, m_paral_cnt);
}


BEGIN_MESSAGE_MAP(CSphereParamsDlg, CDialog)
	ON_NOTIFY(UDN_DELTAPOS, IDC_MERID_SPH_SPIN, OnDeltaposMeridSphSpin)
	ON_NOTIFY(UDN_DELTAPOS, IDC_PARAL_SPH_SPIN, OnDeltaposParalSphSpin)
END_MESSAGE_MAP()

void CSphereParamsDlg::OnDeltaposMeridSphSpin(NMHDR *pNMHDR, LRESULT *pResult)
{
	/*LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	m_merid_cnt-=pNMUpDown->iDelta;
	if (m_merid_cnt>36)
		m_merid_cnt=36;
	if (m_merid_cnt<3)
		m_merid_cnt=3;
	UpdateData(false);
	*pResult = 0;*/
}

void CSphereParamsDlg::OnDeltaposParalSphSpin(NMHDR *pNMHDR, LRESULT *pResult)
{
	/*LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	m_paral_cnt-=pNMUpDown->iDelta;
	if (m_paral_cnt>36)
		m_paral_cnt=36;
	if (m_paral_cnt<3)
		m_paral_cnt=3;
	UpdateData(false);
	*pResult = 0;*/
}


BOOL CSphereParamsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_merid_spin.SetRange(3,36);
	m_paral_spin.SetRange(3,36);

	CString lbl;
	if (m_type==ELLIPSOID_PARAMS)
		lbl.LoadString(IDS_ELLIPSOID_PARAMS);
	else
		lbl.LoadString(IDS_SPHERE_PARAMS);
	SetWindowText(lbl);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
