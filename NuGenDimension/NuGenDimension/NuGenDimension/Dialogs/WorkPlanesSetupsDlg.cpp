// WorkPlanesSetupsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "WorkPlanesSetupsDlg.h"
#include ".\workplanessetupsdlg.h"


// CWorkPlanesSetupsDlg dialog

IMPLEMENT_DYNAMIC(CWorkPlanesSetupsDlg, CDialog)
CWorkPlanesSetupsDlg::CWorkPlanesSetupsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CWorkPlanesSetupsDlg::IDD, pParent)
			, m_x_pos(0.0f)
			, m_y_pos(0.0f)
			, m_z_pos(0.0f)
			, m_cur_work_plane(0)
			, m_xy_vis(FALSE)
			, m_yz_vis(FALSE)
			, m_xz_vis(FALSE)
{
}

CWorkPlanesSetupsDlg::~CWorkPlanesSetupsDlg()
{
}

void CWorkPlanesSetupsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_X_POS_EDIT, m_x_pos_edit);
	DDX_Control(pDX, IDC_Y_POS_EDIT, m_y_pos_edit);
	DDX_Control(pDX, IDC_Z_POS_EDIT, m_z_pos_edit);
	DDX_Radio(pDX, IDC_CUR_WORK_PLANE_RADIO, m_cur_work_plane);
	DDX_Check(pDX, IDC_WP_XY_VIS_CHECK, m_xy_vis);
	DDX_Check(pDX, IDC_WP_YZ_VIS_CHECK, m_yz_vis);
	DDX_Check(pDX, IDC_WP_XZ_VIS_CHECK, m_xz_vis);
}


BEGIN_MESSAGE_MAP(CWorkPlanesSetupsDlg, CDialog)
	ON_BN_CLICKED(IDC_CUR_WORK_PLANE_RADIO, OnBnClickedCurWorkPlaneRadio)
	ON_BN_CLICKED(IDC_CUR_WORK_PLANE_RADIO2, OnBnClickedCurWorkPlaneRadio2)
	ON_BN_CLICKED(IDC_CUR_WORK_PLANE_RADIO3, OnBnClickedCurWorkPlaneRadio3)
END_MESSAGE_MAP()


BOOL CWorkPlanesSetupsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_x_pos_edit.SetValue(m_x_pos);
	m_y_pos_edit.SetValue(m_y_pos);
	m_z_pos_edit.SetValue(m_z_pos);

	SwitchActivePlane();

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CWorkPlanesSetupsDlg::OnOK()
{
	UpdateData();
	m_x_pos = m_x_pos_edit.GetValue();
	m_y_pos = m_y_pos_edit.GetValue();
	m_z_pos = m_z_pos_edit.GetValue();

	CDialog::OnOK();
}

void CWorkPlanesSetupsDlg::OnBnClickedCurWorkPlaneRadio()
{
	UpdateData();
	SwitchActivePlane();
}

void CWorkPlanesSetupsDlg::OnBnClickedCurWorkPlaneRadio2()
{
	UpdateData();
	SwitchActivePlane();
}

void CWorkPlanesSetupsDlg::OnBnClickedCurWorkPlaneRadio3()
{
	UpdateData();
	SwitchActivePlane();
}

void CWorkPlanesSetupsDlg::SwitchActivePlane()
{
	GetDlgItem(IDC_X_POS_STATIC)->EnableWindow(m_cur_work_plane==2);
	GetDlgItem(IDC_X_POS_EDIT)->EnableWindow(m_cur_work_plane==2);
	GetDlgItem(IDC_WP_YZ_VIS_CHECK)->EnableWindow(m_cur_work_plane==2);
	GetDlgItem(IDC_Y_POS_STATIC)->EnableWindow(m_cur_work_plane==1);
	GetDlgItem(IDC_Y_POS_EDIT)->EnableWindow(m_cur_work_plane==1);
	GetDlgItem(IDC_WP_XZ_VIS_CHECK)->EnableWindow(m_cur_work_plane==1);
	GetDlgItem(IDC_Z_POS_STATIC)->EnableWindow(m_cur_work_plane==0);
	GetDlgItem(IDC_Z_POS_EDIT)->EnableWindow(m_cur_work_plane==0);
	GetDlgItem(IDC_WP_XY_VIS_CHECK)->EnableWindow(m_cur_work_plane==0);
}