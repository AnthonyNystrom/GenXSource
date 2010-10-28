// GetOneDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//..//NuGenDimension.h"
#include "GetOneDlg.h"
#include ".\getonedlg.h"


// CGetOneDlg dialog

IMPLEMENT_DYNAMIC(CGetOneDlg, CDialog)
CGetOneDlg::CGetOneDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CGetOneDlg::IDD, pParent)
{
	m_enable_history = false;
	m_was_diasabled = false;
}

CGetOneDlg::~CGetOneDlg()
{
}

void CGetOneDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_GET_ONE_COMBO, m_combo);
}


BEGIN_MESSAGE_MAP(CGetOneDlg, CDialog)
	ON_WM_ERASEBKGND()
	ON_WM_SIZE()
	ON_CBN_SELCHANGE(IDC_GET_ONE_COMBO, OnCbnSelchangeGetOneCombo)
END_MESSAGE_MAP()


// CGetOneDlg message handlers

void CGetOneDlg::OnOK()
{
}

void CGetOneDlg::OnCancel()
{
}

IBaseInterfaceOfGetDialogs::DLG_TYPE  CGetOneDlg::GetType()
{
	return IBaseInterfaceOfGetDialogs::COMBO_DLG;
}

CWnd*     CGetOneDlg::GetWindow()
{
	return this;
}

void      CGetOneDlg::EnableControls(bool enbl)
{
	if (enbl)
	{
		GetDlgItem(IDC_GET_ONE_COMBO)->EnableWindow(m_enable_history);
		m_was_diasabled = false;
	}
	else
	{
		if (!m_was_diasabled)
		{
			m_enable_history = GetDlgItem(IDC_GET_ONE_COMBO)->IsWindowEnabled()!=0;
		}
		GetDlgItem(IDC_GET_ONE_COMBO)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
}

void      CGetOneDlg::AddString(const char* str)
{
	m_combo.AddString(str);
}

void      CGetOneDlg::RemoveAllStrings()
{
	m_combo.ResetContent();
}

void      CGetOneDlg::SetCurString(unsigned int ind)
{
	if (ind>=(unsigned int)m_combo.GetCount())
		return;
	m_combo.SetCurSel(ind);
}

unsigned int  CGetOneDlg::GetCurString()
{
	return m_combo.GetCurSel();
}


BOOL CGetOneDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

void CGetOneDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	if (::IsWindow(m_combo.m_hWnd))
	{
		CRect rrr;
		m_combo.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_combo.MoveWindow(rrr.left,rrr.top,
			cx-2*rrr.left,rrr.Height());
	}
}

void CGetOneDlg::OnCbnSelchangeGetOneCombo()
{
	if (global_commander)
		global_commander->SendCommanderMessage(ICommander::CM_CHANGE_COMBO,
													(IComboPanel*)this);
}
