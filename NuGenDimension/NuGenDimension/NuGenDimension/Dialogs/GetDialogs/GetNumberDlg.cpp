// GetNumberDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//..//NuGenDimension.h"
#include "GetNumberDlg.h"
#include ".\getnumberdlg.h"


// CGetNumberDlg dialog

IMPLEMENT_DYNAMIC(CGetNumberDlg, CDialog)
CGetNumberDlg::CGetNumberDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CGetNumberDlg::IDD, pParent)
{
	m_enable_history = NULL;
	m_was_diasabled = false;
	
}

CGetNumberDlg::~CGetNumberDlg()
{
	if (m_enable_history)
		delete[] m_enable_history;
}

void CGetNumberDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_NUMBER_EDIT, m_number_edit);
}


IBaseInterfaceOfGetDialogs::DLG_TYPE CGetNumberDlg::GetType()
{
	return IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG;
}

CWnd*   CGetNumberDlg::GetWindow()
{
	return this;
}

void    CGetNumberDlg::EnableControls(bool enbl)
{
	if (m_enable_history==NULL)
		m_enable_history = new bool[1];
	if (enbl)
	{
	   GetDlgItem(IDC_NUMBER_EDIT)->EnableWindow(m_enable_history[0]);
	   GetDlgItem(IDC_NUMBER_EDIT)->SetFocus();
	   m_was_diasabled = false;
	}
	else
	{
		if (!m_was_diasabled)
			m_enable_history[0] = GetDlgItem(IDC_NUMBER_EDIT)->IsWindowEnabled()!=0;
		GetDlgItem(IDC_NUMBER_EDIT)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
}


BEGIN_MESSAGE_MAP(CGetNumberDlg, CDialog)
	ON_WM_CHAR()
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
	ON_WM_SIZE()
END_MESSAGE_MAP()

double CGetNumberDlg::GetNumber()
{
	return static_cast<double>(m_number_edit.GetValue());
}

void   CGetNumberDlg::SetNumber(double nmbr)
{
	m_number_edit.SetValue(static_cast<float>(nmbr));
}

void CGetNumberDlg::OnOK()
{
}

void CGetNumberDlg::OnCancel()
{
}

BOOL CGetNumberDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	return TRUE;  // return TRUE unless you set the focus to a control

	// EXCEPTION: OCX Property Pages should return FALSE
}

void CGetNumberDlg::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	switch(nChar) 
	{
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
		return;
	default:
		{
			CWnd* focW = GetFocus();
			if (focW==&m_number_edit)
			{
				m_number_edit.OnChar(nChar,nRepCnt,nFlags);
				return;
			}
			else
			{
				m_number_edit.SendMessage(GET_DIALOGS_MESSAGE_PRE_SET_FOCUS,nChar,nRepCnt);
			}
			return;
		}
		break;
	}

	CDialog::OnChar(nChar, nRepCnt, nFlags);
}


BOOL CGetNumberDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

HBRUSH CGetNumberDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	/*int cID = pWnd->GetDlgCtrlID();

	if (cID == IDC_NUMBER_EDIT &&
		!pWnd->IsWindowEnabled())
	{
		pDC->SetBkMode(TRANSPARENT);
		return (HBRUSH)::GetSysColorBrush(COLOR_WINDOW);
	}*/
	return hbr;
}

void CGetNumberDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	if (m_number_edit.m_hWnd && cx>10)
	{
		CRect rcEd;
		m_number_edit.GetWindowRect(rcEd);
		ScreenToClient(rcEd);
	
		m_number_edit.MoveWindow(rcEd.left,rcEd.top,cx-2*rcEd.left,rcEd.Height());
		
	}
}