// SplinePointsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "SplinePointsDlg.h"
#include ".\splinepointsdlg.h"

// CSplinePointsDlg dialog

IMPLEMENT_DYNAMIC(CSplinePointsDlg, CDialog)
CSplinePointsDlg::CSplinePointsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSplinePointsDlg::IDD, pParent)
{
}

CSplinePointsDlg::~CSplinePointsDlg()
{
}

void CSplinePointsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_SPLINE_POINTS_LIST, m_list);
}


BEGIN_MESSAGE_MAP(CSplinePointsDlg, CDialog)
	ON_WM_SIZE()
END_MESSAGE_MAP()

void   CSplinePointsDlg::AddPoint(const SG_POINT& pnt)
{
	CString tmpS;
	tmpS.Format("X=%f  Y=%f  Z=%f",pnt.x,pnt.y,pnt.z);
	m_list.InsertItem(m_list.GetItemCount(),tmpS);
}

void  CSplinePointsDlg::RemoveAllPoints()
{
	m_list.DeleteAllItems();
}

// CSplinePointsDlg message handlers

void CSplinePointsDlg::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class

	//__super::OnOK();
}

void CSplinePointsDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class

	//__super::OnCancel();
}

void CSplinePointsDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);
	
	if (::IsWindow(m_list.m_hWnd))
	{
		m_list.MoveWindow(2,2,cx-4,cy-4);
		m_list.SetColumnWidth(0,cx-9);
	}
}

BOOL CSplinePointsDlg::OnInitDialog()
{
	__super::OnInitDialog();

	m_list.InsertColumn(0,"");
	CRect rct;
	m_list.GetWindowRect(rct);
	m_list.SetColumnWidth(0,rct.Width()-9);
	m_list.SetExtendedStyle(LVS_EX_GRIDLINES|LVS_EX_FULLROWSELECT);


	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
