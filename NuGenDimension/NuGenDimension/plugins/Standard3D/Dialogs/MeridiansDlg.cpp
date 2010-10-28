// SphereParamsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "MeridiansDlg.h"


// CMeridiansDlg dialog

IMPLEMENT_DYNAMIC(CMeridiansDlg, CDialog)
CMeridiansDlg::CMeridiansDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CMeridiansDlg::IDD, pParent)
	, m_merid_cnt(3)
	, m_type(CMeridiansDlg::CONE_PARAMS)
{
}

CMeridiansDlg::~CMeridiansDlg()
{
}

void CMeridiansDlg::SetDlgType(DLG_TYPE tp)
{
	m_type = tp;
}

void CMeridiansDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_MERID_SPIN, m_merid_spin);
	DDX_Text(pDX, IDC_MERID_EDIT, m_merid_cnt);
}


BEGIN_MESSAGE_MAP(CMeridiansDlg, CDialog)
	ON_NOTIFY(UDN_DELTAPOS, IDC_MERID_SPIN, OnDeltaposMeridSphSpin)
END_MESSAGE_MAP()

void CMeridiansDlg::OnDeltaposMeridSphSpin(NMHDR *pNMHDR, LRESULT *pResult)
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

BOOL CMeridiansDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_merid_spin.SetRange(3,36);
	
	CString lbl;
	switch(m_type) {
	case CONE_PARAMS:
		lbl.LoadString(IDS_CONE_PARAMS);
		break;
	case CYL_PARAMS:
		lbl.LoadString(IDS_CYL_PARAMS);
		break;
	case SPH_BAND_PARAMS:
		lbl.LoadString(IDS_SPH_B_PARAMS);
		break;
	default:
		ASSERT(0);
		return TRUE;
	}
	SetWindowText(lbl);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
