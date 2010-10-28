// LinearDimDlg.cpp : implementation file
//

#include "stdafx.h"
#include "LinearDimDlg.h"
#include ".\lineardimdlg.h"

// CLinearDimDlg dialog

IMPLEMENT_DYNAMIC(CLinearDimDlg, CDialog)
CLinearDimDlg::CLinearDimDlg(ICommandPanel* comPan, SG_DIMENSION_STYLE* ds, 
							 IApplicationInterface* appI,bool withInvert,CWnd* pParent /*=NULL*/)
	: CDialog(CLinearDimDlg::IDD, pParent)
	, m_com_pan(comPan)
	, m_razm_lin_check(ds->dimension_line)
	, m_left_dim_check(ds->first_side_line)
	, m_right_dim_check(ds->second_side_line)
	, m_left_end_out_check(ds->out_first_arrow)
	, m_right_end_out_check(ds->out_second_arrow)
	, m_auto_arr(ds->automatic_arrows)
	, m_app(appI)
{
	m_dim_style_pointer = ds;
	ASSERT(m_com_pan);
	ASSERT(appI);
	m_inFocus = false;
	m_with_invert = withInvert;
}

CLinearDimDlg::~CLinearDimDlg()
{
}

void CLinearDimDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LD_LEFT_END_TYPE_COMBO, m_left_end_type_combo);
	DDX_Control(pDX, IDC_LD_RIGHT_END_TYPE_COMBO, m_right_end_type_combo);
	DDX_Check(pDX, IDC_LD_RAZM_LIN_CHECK, m_razm_lin_check);
	DDX_Check(pDX, IDC_LD_LEFT_DIM_CHECK, m_left_dim_check);
	DDX_Check(pDX, IDC_LD_RIGHT_DIM_CHECK, m_right_dim_check);
	DDX_Check(pDX, IDC_LD_LEFT_END_OUT_CHECK, m_left_end_out_check);
	DDX_Check(pDX, IDC_LD_RIGHT_END_OUT_CHECK, m_right_end_out_check);
	DDX_Check(pDX, IDC_LIN_DIM_AUTO_ARR, m_auto_arr);
	DDX_Control(pDX, IDC_LD_VINOS_EDIT, m_vinos_size);
	DDX_Control(pDX, IDC_LD_END_SIZE_EDIT, m_arrow_size);
}


BEGIN_MESSAGE_MAP(CLinearDimDlg, CDialog)
	ON_BN_CLICKED(IDC_LD_RAZM_LIN_CHECK, OnBnClickedLdRazmLinCheck)
	ON_BN_CLICKED(IDC_LD_LEFT_DIM_CHECK, OnBnClickedLdLeftDimCheck)
	ON_BN_CLICKED(IDC_LD_RIGHT_DIM_CHECK, OnBnClickedLdRightDimCheck)
	ON_BN_CLICKED(IDC_LD_LEFT_END_OUT_CHECK, OnBnClickedLdLeftEndOutCheck)
	ON_BN_CLICKED(IDC_LD_RIGHT_END_OUT_CHECK, OnBnClickedLdRightEndOutCheck)
	ON_CBN_SELCHANGE(IDC_LD_LEFT_END_TYPE_COMBO, OnCbnSelchangeLdLeftEndTypeCombo)
	ON_CBN_SELCHANGE(IDC_LD_RIGHT_END_TYPE_COMBO, OnCbnSelchangeLdRightEndTypeCombo)
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
	ON_BN_CLICKED(IDC_LIN_DIM_AUTO_ARR, OnBnClickedLinDimAutoArr)
	ON_WM_SIZE()
	ON_EN_CHANGE(IDC_LD_VINOS_EDIT, OnEnChangeLdVinosEdit)
	ON_EN_SETFOCUS(IDC_LD_VINOS_EDIT, OnEnSetfocusLdVinosEdit)
	ON_EN_KILLFOCUS(IDC_LD_VINOS_EDIT, OnEnKillfocusLdVinosEdit)
	ON_EN_CHANGE(IDC_LD_END_SIZE_EDIT, OnEnChangeLdEndSizeEdit)
	ON_EN_SETFOCUS(IDC_LD_END_SIZE_EDIT, OnEnSetfocusLdEndSizeEdit)
	ON_EN_KILLFOCUS(IDC_LD_END_SIZE_EDIT, OnEnKillfocusLdEndSizeEdit)
	ON_BN_CLICKED(IDC_LIN_DIM_INVERT, OnBnClickedLinDimInvert)
END_MESSAGE_MAP()


// CLinearDimDlg message handlers

BOOL CLinearDimDlg::OnInitDialog()
{
	__super::OnInitDialog();

	m_left_end_type_combo.SetType(true);
	m_right_end_type_combo.SetType(false);

	m_left_end_type_combo.SetCurSel(m_dim_style_pointer->first_arrow_style);
	m_right_end_type_combo.SetCurSel(m_dim_style_pointer->second_arrow_style);

	GetDlgItem(IDC_LD_LEFT_END_OUT_CHECK)->EnableWindow(FALSE);
	GetDlgItem(IDC_LD_RIGHT_END_OUT_CHECK)->EnableWindow(FALSE);

	m_vinos_size.SetValue((float)m_dim_style_pointer->lug_size);
	m_arrow_size.SetValue((float)m_dim_style_pointer->arrows_size);

	if (m_with_invert)
	{
		GetDlgItem(IDC_LIN_DIM_INVERT)->EnableWindow(TRUE);
		if (m_dim_style_pointer->invert)
			CheckDlgButton(IDC_LIN_DIM_INVERT,BST_CHECKED);
		else
			CheckDlgButton(IDC_LIN_DIM_INVERT,BST_UNCHECKED);
	}
	else
	{
		GetDlgItem(IDC_LIN_DIM_INVERT)->EnableWindow(FALSE);
		CheckDlgButton(IDC_LIN_DIM_INVERT,BST_UNCHECKED);
	}

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CLinearDimDlg::OnBnClickedLdRazmLinCheck()
{
	m_dim_style_pointer->dimension_line = !m_dim_style_pointer->dimension_line;
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnBnClickedLdLeftDimCheck()
{
	m_dim_style_pointer->first_side_line = !m_dim_style_pointer->first_side_line;
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnBnClickedLdRightDimCheck()
{
	m_dim_style_pointer->second_side_line = !m_dim_style_pointer->second_side_line;
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnBnClickedLdLeftEndOutCheck()
{
	m_dim_style_pointer->out_first_arrow = !m_dim_style_pointer->out_first_arrow;
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnBnClickedLdRightEndOutCheck()
{
	m_dim_style_pointer->out_second_arrow = !m_dim_style_pointer->out_second_arrow;
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnCbnSelchangeLdLeftEndTypeCombo()
{
	m_dim_style_pointer->first_arrow_style = m_left_end_type_combo.GetCurSel();
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnCbnSelchangeLdRightEndTypeCombo()
{
	m_dim_style_pointer->second_arrow_style = m_right_end_type_combo.GetCurSel();
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

BOOL CLinearDimDlg::OnEraseBkgnd(CDC* pDC)
{
	CRect grRect;
	CRect labRect;
	GetDlgItem(IDC_LD_LEFT_END_STATIC)->GetWindowRect(grRect);
	ScreenToClient(grRect);
	GetDlgItem(IDC_LEFT_END_STATIC)->GetWindowRect(labRect);
	ScreenToClient(labRect);
	
	grRect.top = labRect.top+labRect.Height()/2;
	m_com_pan->DrawGroupFrame(pDC,grRect,labRect.left,labRect.right-5);

	GetDlgItem(IDC_LD_RIGHT_END_STATIC)->GetWindowRect(grRect);
	ScreenToClient(grRect);
	GetDlgItem(IDC_RIGHT_END_STATIC)->GetWindowRect(labRect);
	ScreenToClient(labRect);

	grRect.top = labRect.top+labRect.Height()/2;
	m_com_pan->DrawGroupFrame(pDC,grRect,labRect.left,labRect.right-5);
	
	return TRUE;//__super::OnEraseBkgnd(pDC);
}

HBRUSH CLinearDimDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = __super::OnCtlColor(pDC, pWnd, nCtlColor);

	if (pWnd->m_hWnd==GetDlgItem(IDC_LD_VINOS_EDIT)->m_hWnd ||
		pWnd->m_hWnd==GetDlgItem(IDC_LD_LEFT_END_TYPE_COMBO)->m_hWnd ||
		pWnd->m_hWnd==GetDlgItem(IDC_LD_RIGHT_END_TYPE_COMBO)->m_hWnd ||
		pWnd->m_hWnd==GetDlgItem(IDC_LD_END_SIZE_EDIT)->m_hWnd)
			return hbr;

	pDC->SetTextColor(0);
	//pDC->SetBkColor(RGB(255,255,255));
	pDC->SetBkMode(TRANSPARENT);
	hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);

	// TODO:  Return a different brush if the default is not desired
	return hbr;
}

void CLinearDimDlg::OnBnClickedLinDimAutoArr()
{
	if (m_dim_style_pointer->automatic_arrows)
	{
		m_dim_style_pointer->automatic_arrows = false;
		GetDlgItem(IDC_LD_LEFT_END_OUT_CHECK)->EnableWindow(TRUE);
		GetDlgItem(IDC_LD_RIGHT_END_OUT_CHECK)->EnableWindow(TRUE);
	}
	else
	{
		m_dim_style_pointer->automatic_arrows = true;
		GetDlgItem(IDC_LD_LEFT_END_OUT_CHECK)->EnableWindow(FALSE);
		GetDlgItem(IDC_LD_RIGHT_END_OUT_CHECK)->EnableWindow(FALSE);
	}
}

void CLinearDimDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	if (::IsWindow(m_vinos_size.m_hWnd))
	{
		CRect rrr;

		m_vinos_size.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_vinos_size.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());

		m_arrow_size.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_arrow_size.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());
	}
}

void CLinearDimDlg::OnEnChangeLdVinosEdit()
{
	if (::IsWindow(m_vinos_size.m_hWnd))
	{
		m_dim_style_pointer->lug_size = m_vinos_size.GetValue();
	}
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnEnSetfocusLdVinosEdit()
{
	m_inFocus = true;
}

void CLinearDimDlg::OnEnKillfocusLdVinosEdit()
{
	m_inFocus = false;
}

void CLinearDimDlg::OnEnChangeLdEndSizeEdit()
{
	if (::IsWindow(m_arrow_size.m_hWnd))
	{
		m_dim_style_pointer->arrows_size = m_arrow_size.GetValue();
	}
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CLinearDimDlg::OnEnSetfocusLdEndSizeEdit()
{
	m_inFocus = true;
}

void CLinearDimDlg::OnEnKillfocusLdEndSizeEdit()
{
	m_inFocus = false;
}

void CLinearDimDlg::OnBnClickedLinDimInvert()
{
	m_dim_style_pointer->invert = !m_dim_style_pointer->invert;
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}
