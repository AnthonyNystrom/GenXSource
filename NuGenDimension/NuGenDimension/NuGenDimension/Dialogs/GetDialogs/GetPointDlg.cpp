// GetPointDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//..//NuGenDimension.h"
#include "GetPointDlg.h"
#include ".\getpointdlg.h"



// CGetPointDlg dialog

IMPLEMENT_DYNAMIC(CGetPointDlg, CDialog)
CGetPointDlg::CGetPointDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CGetPointDlg::IDD, pParent)
{
	m_tabs_counter = 1;
	m_enable_history = NULL;
	m_was_diasabled = false;
}

CGetPointDlg::~CGetPointDlg()
{
	if (m_enable_history)
		delete[] m_enable_history;
	
}

void CGetPointDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_GET_POINT_X_EDIT, m_x_edit);
	DDX_Control(pDX, IDC_GET_POINT_Y_EDIT, m_y_edit);
	DDX_Control(pDX, IDC_GET_POINT_Z_EDIT, m_z_edit);
	DDX_Control(pDX, IDC_PNT_X_LOCK_BTN, m_x_lock_btn);
	DDX_Control(pDX, IDC_PNT_Y_LOCK_BTN, m_y_lock_btn);
	DDX_Control(pDX, IDC_PNT_Z_LOCK_BTN, m_z_lock_btn);
}


BEGIN_MESSAGE_MAP(CGetPointDlg, CDialog)
	ON_BN_CLICKED(IDC_PNT_X_LOCK_BTN, OnBnClickedPntXLockBtn)
	ON_BN_CLICKED(IDC_PNT_Y_LOCK_BTN, OnBnClickedPntYLockBtn)
	ON_BN_CLICKED(IDC_PNT_Z_LOCK_BTN, OnBnClickedPntZLockBtn)
	ON_WM_CHAR()
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
	ON_WM_SIZE()
	ON_WM_KEYDOWN()
END_MESSAGE_MAP()

IBaseInterfaceOfGetDialogs::DLG_TYPE CGetPointDlg::GetType()
{
	return IBaseInterfaceOfGetDialogs::GET_POINT_DLG;
}

CWnd*   CGetPointDlg::GetWindow()
{
	return this;
}


void    CGetPointDlg::EnableControls(bool enbl)
{
	if (m_enable_history==NULL)
		m_enable_history = new bool[9];
	if (enbl)
	{
		GetDlgItem(IDC_GET_POINT_X_STATIC)->EnableWindow(m_enable_history[0]);
		GetDlgItem(IDC_GET_POINT_Y_STATIC)->EnableWindow(m_enable_history[1]);
		GetDlgItem(IDC_GET_POINT_Z_STATIC)->EnableWindow(m_enable_history[2]);
		GetDlgItem(IDC_GET_POINT_X_EDIT)->EnableWindow(m_enable_history[3]);
		GetDlgItem(IDC_GET_POINT_Y_EDIT)->EnableWindow(m_enable_history[4]);
		GetDlgItem(IDC_GET_POINT_Z_EDIT)->EnableWindow(m_enable_history[5]);
		GetDlgItem(IDC_PNT_X_LOCK_BTN)->EnableWindow(m_enable_history[6]);
		GetDlgItem(IDC_PNT_Y_LOCK_BTN)->EnableWindow(m_enable_history[7]);
		GetDlgItem(IDC_PNT_Z_LOCK_BTN)->EnableWindow(m_enable_history[8]);	
		m_was_diasabled = false;
		GetDlgItem(IDC_GET_POINT_X_EDIT)->SetFocus();
	}
	else
	{
		if (!m_was_diasabled)
		{
			m_enable_history[0] = GetDlgItem(IDC_GET_POINT_X_STATIC)->IsWindowEnabled()!=0;
			m_enable_history[1] = GetDlgItem(IDC_GET_POINT_Y_STATIC)->IsWindowEnabled()!=0;
			m_enable_history[2] = GetDlgItem(IDC_GET_POINT_Z_STATIC)->IsWindowEnabled()!=0;
			m_enable_history[3] = GetDlgItem(IDC_GET_POINT_X_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[4] = GetDlgItem(IDC_GET_POINT_Y_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[5] = GetDlgItem(IDC_GET_POINT_Z_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[6] = GetDlgItem(IDC_PNT_X_LOCK_BTN)->IsWindowEnabled()!=0;
			m_enable_history[7] = GetDlgItem(IDC_PNT_Y_LOCK_BTN)->IsWindowEnabled()!=0;
			m_enable_history[8] = GetDlgItem(IDC_PNT_Z_LOCK_BTN)->IsWindowEnabled()!=0;
		}
		GetDlgItem(IDC_GET_POINT_X_STATIC)->EnableWindow(FALSE);
		GetDlgItem(IDC_GET_POINT_Y_STATIC)->EnableWindow(FALSE);
		GetDlgItem(IDC_GET_POINT_Z_STATIC)->EnableWindow(FALSE);
		GetDlgItem(IDC_GET_POINT_X_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_GET_POINT_Y_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_GET_POINT_Z_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_PNT_X_LOCK_BTN)->EnableWindow(FALSE);
		GetDlgItem(IDC_PNT_Y_LOCK_BTN)->EnableWindow(FALSE);
		GetDlgItem(IDC_PNT_Z_LOCK_BTN)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
}


bool  CGetPointDlg::SetPoint(double x, double y, double z)
{
	m_x_edit.SetValue(static_cast<float>(x));
	m_y_edit.SetValue(static_cast<float>(y));
	m_z_edit.SetValue(static_cast<float>(z));
	return true;
}

bool  CGetPointDlg::GetPoint(double& x, double& y, double& z)
{
	x = static_cast<double>(m_x_edit.GetValue());
	y = static_cast<double>(m_y_edit.GetValue());
	z = static_cast<double>(m_z_edit.GetValue());
	return true;
}

bool  CGetPointDlg::IsXFixed()
{
	return m_x_edit.IsFixed();
}

bool  CGetPointDlg::IsYFixed()
{
	return m_y_edit.IsFixed();
}

bool  CGetPointDlg::IsZFixed()
{
	return m_z_edit.IsFixed();
}

bool  CGetPointDlg::XFix(bool fix)
{
	m_x_lock_btn.SetPressed(fix);
	return true;
}

bool  CGetPointDlg::YFix(bool fix)
{
	m_y_lock_btn.SetPressed(fix);
	return true;
}

bool  CGetPointDlg::ZFix(bool fix)
{
	m_z_lock_btn.SetPressed(fix);
	return true;
}


void CGetPointDlg::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class

	//__super::OnOK();
}

void CGetPointDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class

	//__super::OnCancel();
}

BOOL CGetPointDlg::OnInitDialog()
{
	__super::OnInitDialog();

	CString lab;
	lab.LoadString(IDS_X_LOCK);
	m_x_lock_btn.SetToolTipText(lab);
	m_x_lock_btn.LoadBitmap(::LoadImage(::AfxGetResourceHandle(),
		MAKEINTRESOURCE(IDB_LOCK_BUTTON_BITMAP), 
		IMAGE_BITMAP,0,0,LR_LOADMAP3DCOLORS));
	m_x_edit.SetFixButton(&m_x_lock_btn);

	lab.LoadString(IDS_Y_LOCK);
	m_y_lock_btn.SetToolTipText(lab);
	m_y_lock_btn.LoadBitmap(::LoadImage(::AfxGetResourceHandle(),
		MAKEINTRESOURCE(IDB_LOCK_BUTTON_BITMAP), 
		IMAGE_BITMAP,0,0,LR_LOADMAP3DCOLORS));
	m_y_edit.SetFixButton(&m_y_lock_btn);

	lab.LoadString(IDS_Z_LOCK);
	m_z_lock_btn.SetToolTipText(lab);
	m_z_lock_btn.LoadBitmap(::LoadImage(::AfxGetResourceHandle(),
		MAKEINTRESOURCE(IDB_LOCK_BUTTON_BITMAP), 
		IMAGE_BITMAP,0,0,LR_LOADMAP3DCOLORS));
	m_z_edit.SetFixButton(&m_z_lock_btn);


	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CGetPointDlg::OnBnClickedPntXLockBtn()
{
	m_x_lock_btn.SetPressed(!m_x_lock_btn.IsPressed());

}

void CGetPointDlg::OnBnClickedPntYLockBtn()
{
	m_y_lock_btn.SetPressed(!m_y_lock_btn.IsPressed());

}

void CGetPointDlg::OnBnClickedPntZLockBtn()
{
	m_z_lock_btn.SetPressed(!m_z_lock_btn.IsPressed());

}

void CGetPointDlg::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	switch(nChar) 
	{
	/*case 120:  // X
	case _T('X'):
		m_x_lock_btn.SetPressed(!m_x_lock_btn.IsPressed());
		break;
	case 121:  // Y
	case _T('Y'):
		m_y_lock_btn.SetPressed(!m_y_lock_btn.IsPressed());
		break;
	case 122:  // Z
	case _T('Z'):
		m_z_lock_btn.SetPressed(!m_z_lock_btn.IsPressed());
		break;*/
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
		return;
	case VK_TAB:
		GetDlgItem(IDC_GET_POINT_X_EDIT+((m_tabs_counter++)%3))->SetFocus();
		((CEdit*)GetFocus())->SetSel(0,-1);
		return;
	default:
		{
			CWnd* focW = GetFocus();
			if (focW==&m_y_edit)
			{
				m_y_edit.OnChar(nChar,nRepCnt,nFlags);
				return;
			}
			else
				if (focW==&m_z_edit)
				{
					m_z_edit.OnChar(nChar,nRepCnt,nFlags);
					return;
				}
				else
					if (focW==&m_x_edit)
					{
						m_x_edit.OnChar(nChar,nRepCnt,nFlags);
						return;
					}
					else
					{
						m_x_edit.SendMessage(GET_DIALOGS_MESSAGE_PRE_SET_FOCUS,nChar,nRepCnt);
					}
					
					return;
		}
		break;
	}

	CDialog::OnChar(nChar, nRepCnt, nFlags);

}

BOOL CGetPointDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

HBRUSH CGetPointDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	// Call the base class implementation first! Otherwise, it may
	// undo what we are trying to accomplish here.
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	if (nCtlColor==CTLCOLOR_STATIC && (pWnd!=&m_x_edit) 
		&& (pWnd!=&m_y_edit) && (pWnd!=&m_z_edit))
	{
		pDC->SetTextColor(0);
		//pDC->SetBkColor(RGB(255,255,255));
		pDC->SetBkMode(TRANSPARENT);
		hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);
	}

	return hbr;

}

void CGetPointDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	if (m_x_edit.m_hWnd && cx>10)
	{
		CRect rcEd;
		CRect rcBt;
		m_x_edit.GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		m_x_lock_btn.GetWindowRect(rcBt);
		ScreenToClient(rcBt);
		m_x_edit.MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left-rcBt.Width()-3,rcEd.Height());
		m_x_edit.GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		m_x_lock_btn.MoveWindow(rcEd.right+1,rcBt.top,rcBt.Width(),rcBt.Height());

		m_y_edit.GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		m_y_lock_btn.GetWindowRect(rcBt);
		ScreenToClient(rcBt);
		m_y_edit.MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left-rcBt.Width()-3,rcEd.Height());
		m_y_edit.GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		m_y_lock_btn.MoveWindow(rcEd.right+1,rcBt.top,rcBt.Width(),rcBt.Height());

		m_z_edit.GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		m_z_lock_btn.GetWindowRect(rcBt);
		ScreenToClient(rcBt);
		m_z_edit.MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left-rcBt.Width()-3,rcEd.Height());
		m_z_edit.GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		m_z_lock_btn.MoveWindow(rcEd.right+1,rcBt.top,rcBt.Width(),rcBt.Height());
	}
}

void CGetPointDlg::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
/*	CWnd* focW = GetFocus();
	if (focW!=GetDlgItem(IDC_GET_POINT_X_EDIT) &&
		focW!=GetDlgItem(IDC_GET_POINT_Y_EDIT) &&
		focW!=GetDlgItem(IDC_GET_POINT_Z_EDIT))
	{
		//GetDlgItem(IDC_PNT_X_EDIT)->SetWindowText("");
		GetDlgItem(IDC_GET_POINT_X_EDIT)->SetFocus();
	}
	GetFocus()->SendMessage(WM_KEYDOWN,nChar,0);
	GetFocus()->SendMessage(WM_CHAR,nChar,0);
*/

	switch(nChar) 
	{
	case 120:  // X
	case _T('X'):
		m_x_lock_btn.SetPressed(!m_x_lock_btn.IsPressed());
		break;
	case 121:  // Y
	case _T('Y'):
		m_y_lock_btn.SetPressed(!m_y_lock_btn.IsPressed());
		break;
	case 122:  // Z
	case _T('Z'):
		m_z_lock_btn.SetPressed(!m_z_lock_btn.IsPressed());
		break;
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
		return;
	case VK_TAB:
		GetDlgItem(IDC_GET_POINT_X_EDIT+((m_tabs_counter++)%3))->SetFocus();
		((CEdit*)GetFocus())->SetSel(0,-1);
		return;
	default:
		{
		/*	CWnd* focW = GetFocus();
			if (focW==&m_y_edit)
			{
				m_y_edit.OnKeyDown(nChar,nRepCnt,nFlags);
				return;
			}
			else
				if (focW==&m_z_edit)
				{
					m_z_edit.OnKeyDown(nChar,nRepCnt,nFlags);
					return;
				}
				else
					if (focW==&m_x_edit)
					{
						m_x_edit.OnKeyDown(nChar,nRepCnt,nFlags);
						return;
					}
					else
					{
						m_x_edit.SendMessage(GET_DIALOGS_MESSAGE_PRE_SET_FOCUS,nChar,nRepCnt);
					}
			
			return;*/
		}
		break;
	}
	__super::OnKeyDown(nChar, nRepCnt, nFlags);
}
