// GetVectorDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//..//NuGenDimension.h"
#include "GetVectorDlg.h"
#include ".\getvectordlg.h"


// CGetVectorDlg dialog

IMPLEMENT_DYNAMIC(CGetVectorDlg, CDialog)
CGetVectorDlg::CGetVectorDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CGetVectorDlg::IDD, pParent)
{
	m_vector_type = USER_VECTOR;
	m_enable_history = NULL;
	m_was_diasabled = false;
}

CGetVectorDlg::~CGetVectorDlg()
{
	if (m_enable_history)
		delete[] m_enable_history;
}

void CGetVectorDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_VECTOR_X_EDIT, m_x_edit);
	DDX_Control(pDX, IDC_VECTOR_Y_EDIT, m_y_edit);
	DDX_Control(pDX, IDC_VECTOR_Z_EDIT, m_z_edit);
	DDX_Control(pDX, IDC_VECTOR_X_LOCK_BTN, m_x_lock_btn);
	DDX_Control(pDX, IDC_VECTOR_Y_LOCK_BTN, m_y_lock_btn);
	DDX_Control(pDX, IDC_VECTOR_Z_LOCK_BTN, m_z_lock_btn);
	DDX_Control(pDX, IDC_VECTOR_X_RADIO, m_x_radio);
	DDX_Control(pDX, IDC_VECTOR_Y_RADIO, m_y_radio);
	DDX_Control(pDX, IDC_VECTOR_Z_RADIO, m_z_radio);
	DDX_Control(pDX, IDC_VECTOR_USER_RADIO, m_user_radio);
}


BEGIN_MESSAGE_MAP(CGetVectorDlg, CDialog)
	ON_WM_CHAR()
	ON_BN_CLICKED(IDC_VECTOR_X_LOCK_BTN, OnBnClickedDirXLockBtn)
	ON_BN_CLICKED(IDC_VECTOR_Y_LOCK_BTN, OnBnClickedDirYLockBtn)
	ON_BN_CLICKED(IDC_VECTOR_Z_LOCK_BTN, OnBnClickedDirZLockBtn)
	ON_BN_CLICKED(IDC_VECTOR_X_RADIO, OnBnClickedVectorXRadio)
	ON_BN_CLICKED(IDC_VECTOR_Y_RADIO, OnBnClickedVectorYRadio)
	ON_BN_CLICKED(IDC_VECTOR_Z_RADIO, OnBnClickedVectorZRadio)
	ON_BN_CLICKED(IDC_VECTOR_USER_RADIO, OnBnClickedVectorUserRadio)

	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
	ON_WM_SIZE()
	ON_WM_KEYDOWN()
END_MESSAGE_MAP()


IBaseInterfaceOfGetDialogs::DLG_TYPE CGetVectorDlg::GetType()
{
	return IBaseInterfaceOfGetDialogs::GET_VECTOR_DLG;
}

CWnd*   CGetVectorDlg::GetWindow()
{
	return this;
}


void    CGetVectorDlg::EnableControls(bool enbl)
{
	if (m_enable_history==NULL)
		m_enable_history = new bool[13];
	if (enbl)
	{
		GetDlgItem(IDC_VECTOR_X_RADIO)->EnableWindow(m_enable_history[0]);
		GetDlgItem(IDC_VECTOR_Y_RADIO)->EnableWindow(m_enable_history[1]);
		GetDlgItem(IDC_VECTOR_Z_RADIO)->EnableWindow(m_enable_history[2]);
		GetDlgItem(IDC_VECTOR_USER_RADIO)->EnableWindow(m_enable_history[3]);
		GetDlgItem(IDC_VEC_X_STATIC)->EnableWindow(m_enable_history[4]);
		GetDlgItem(IDC_VEC_Y_STATIC)->EnableWindow(m_enable_history[5]);
		GetDlgItem(IDC_VEC_Z_STATIC)->EnableWindow(m_enable_history[6]);
		GetDlgItem(IDC_VECTOR_X_EDIT)->EnableWindow(m_enable_history[7]);
		GetDlgItem(IDC_VECTOR_Y_EDIT)->EnableWindow(m_enable_history[8]);
		GetDlgItem(IDC_VECTOR_Z_EDIT)->EnableWindow(m_enable_history[9]);
		GetDlgItem(IDC_VECTOR_X_LOCK_BTN)->EnableWindow(m_enable_history[10]);
		GetDlgItem(IDC_VECTOR_Y_LOCK_BTN)->EnableWindow(m_enable_history[11]);
		GetDlgItem(IDC_VECTOR_Z_LOCK_BTN)->EnableWindow(m_enable_history[12]);
		m_was_diasabled = false;
		GetDlgItem(IDC_VECTOR_X_EDIT)->SetFocus();
	}
	else
	{
		if (!m_was_diasabled)
		{
			m_enable_history[0] = GetDlgItem(IDC_VECTOR_X_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[1] = GetDlgItem(IDC_VECTOR_Y_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[2] = GetDlgItem(IDC_VECTOR_Z_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[3] = GetDlgItem(IDC_VECTOR_USER_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[4] = GetDlgItem(IDC_VEC_X_STATIC)->IsWindowEnabled()!=0;
			m_enable_history[5] = GetDlgItem(IDC_VEC_Y_STATIC)->IsWindowEnabled()!=0;
			m_enable_history[6] = GetDlgItem(IDC_VEC_Z_STATIC)->IsWindowEnabled()!=0;
			m_enable_history[7] = GetDlgItem(IDC_VECTOR_X_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[8] = GetDlgItem(IDC_VECTOR_Y_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[9] = GetDlgItem(IDC_VECTOR_Z_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[10] = GetDlgItem(IDC_VECTOR_X_LOCK_BTN)->IsWindowEnabled()!=0;
			m_enable_history[11] = GetDlgItem(IDC_VECTOR_Y_LOCK_BTN)->IsWindowEnabled()!=0;
			m_enable_history[12] = GetDlgItem(IDC_VECTOR_Z_LOCK_BTN)->IsWindowEnabled()!=0;
		}
		GetDlgItem(IDC_VECTOR_X_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_Y_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_Z_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_USER_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_VEC_X_STATIC)->EnableWindow(FALSE);
		GetDlgItem(IDC_VEC_Y_STATIC)->EnableWindow(FALSE);
		GetDlgItem(IDC_VEC_Z_STATIC)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_X_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_Y_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_Z_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_X_LOCK_BTN)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_Y_LOCK_BTN)->EnableWindow(FALSE);
		GetDlgItem(IDC_VECTOR_Z_LOCK_BTN)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
}


void   CGetVectorDlg::UpdateRadios()
{
	if (!m_x_radio.m_hWnd ||
		!m_y_radio.m_hWnd ||
		!m_z_radio.m_hWnd ||
		!m_user_radio.m_hWnd)
		return;
	m_x_radio.SetCheck(BST_UNCHECKED);
	m_y_radio.SetCheck(BST_UNCHECKED);
	m_z_radio.SetCheck(BST_UNCHECKED);
	m_user_radio.SetCheck(BST_UNCHECKED);

	switch(m_vector_type) {
	case X_VECTOR:
		m_x_radio.SetCheck(BST_CHECKED);
		break;
	case Y_VECTOR:
		m_y_radio.SetCheck(BST_CHECKED);
		break;
	case Z_VECTOR:
		m_z_radio.SetCheck(BST_CHECKED);
		break;
	case USER_VECTOR:
		m_user_radio.SetCheck(BST_CHECKED);
		break;
	default:
		ASSERT(0);
		break;
	}
}
// CGetVectorDlg message handlers
void    CGetVectorDlg::SetVector(VECTOR_TYPE n_t, 
									 double x, double y, double z)
{
	switch(n_t) {
	case X_VECTOR:
		m_x_edit.SetValue(1.0f);
		m_y_edit.SetValue(0.0f);
		m_z_edit.SetValue(0.0f);
		EnableUserNormalControls(FALSE);
		break;
	case Y_VECTOR:
		m_x_edit.SetValue(0.0f);
		m_y_edit.SetValue(1.0f);
		m_z_edit.SetValue(0.0f);
		EnableUserNormalControls(FALSE);
		break;
	case Z_VECTOR:
		m_x_edit.SetValue(0.0f);
		m_y_edit.SetValue(0.0f);
		m_z_edit.SetValue(1.0f);
		EnableUserNormalControls(FALSE);
		break;
	case USER_VECTOR:
		m_x_edit.SetValue(static_cast<float>(x));
		m_y_edit.SetValue(static_cast<float>(y));
		m_z_edit.SetValue(static_cast<float>(z));
		EnableUserNormalControls(TRUE);
		break;
	default:
		ASSERT(0);
		break;
	}
	UpdateData(FALSE);
	m_vector_type = n_t;
	UpdateRadios();
}

IGetVectorPanel::VECTOR_TYPE  CGetVectorDlg::GetVector(double& x, double& y, double& z)
{
	switch(m_vector_type) {
	case X_VECTOR:
		x = 1.0;
		y = 0.0;
		z = 0.0;
		break;
	case Y_VECTOR:
		x = 0.0;
		y = 1.0;
		z = 0.0;
		break;
	case Z_VECTOR:
		x = 0.0;
		y = 0.0;
		z = 1.0;
		break;
	case USER_VECTOR:
		x = static_cast<double>(m_x_edit.GetValue());
		y = static_cast<double>(m_y_edit.GetValue());
		z = static_cast<double>(m_z_edit.GetValue());
		break;
	default:
		ASSERT(0);
		break;
	}
	return m_vector_type;
}

bool  CGetVectorDlg::IsXFixed()
{
	if (m_vector_type!=USER_VECTOR)
		return true;

	return m_x_edit.IsFixed();
}

bool  CGetVectorDlg::IsYFixed()
{
	if (m_vector_type!=USER_VECTOR)
		return true;

	return m_y_edit.IsFixed();
}

bool  CGetVectorDlg::IsZFixed() 
{
	if (m_vector_type!=USER_VECTOR)
		return true;

	return m_z_edit.IsFixed();
}

bool CGetVectorDlg::XFix(bool fix)
{
	if (m_vector_type!=USER_VECTOR)
		return false;

	m_x_lock_btn.SetPressed(fix);
	return true;
}

bool CGetVectorDlg::YFix(bool fix)
{
	if (m_vector_type!=USER_VECTOR)
		return false;

	m_y_lock_btn.SetPressed(fix);
	return true;
}

bool CGetVectorDlg::ZFix(bool fix)
{
	if (m_vector_type!=USER_VECTOR)
		return false;

	m_z_lock_btn.SetPressed(fix);
	return true;
}

void CGetVectorDlg::OnOK()
{
}

void CGetVectorDlg::OnCancel()
{
}

BOOL CGetVectorDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

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

	UpdateRadios();

	return TRUE;  // return TRUE unless you set the focus to a control

	// EXCEPTION: OCX Property Pages should return FALSE
}

void CGetVectorDlg::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	if (m_vector_type!=USER_VECTOR)
	{
		if(nChar==VK_RETURN || nChar==VK_ESCAPE)
			GetParent()->SendMessage(WM_CHAR,nChar,0);
		//CDialog::OnChar(nChar, nRepCnt, nFlags);
		return;
	}

	switch(nChar) 
	{
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
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

void CGetVectorDlg::EnableUserNormalControls(BOOL enbl)
{
	GetDlgItem(IDC_VEC_X_STATIC)->EnableWindow(enbl);
	GetDlgItem(IDC_VECTOR_X_EDIT)->EnableWindow(enbl);
	GetDlgItem(IDC_VECTOR_X_LOCK_BTN)->EnableWindow(enbl);
	GetDlgItem(IDC_VEC_Y_STATIC)->EnableWindow(enbl);
	GetDlgItem(IDC_VECTOR_Y_EDIT)->EnableWindow(enbl);
	GetDlgItem(IDC_VECTOR_Y_LOCK_BTN)->EnableWindow(enbl);
	GetDlgItem(IDC_VEC_Z_STATIC)->EnableWindow(enbl);
	GetDlgItem(IDC_VECTOR_Z_EDIT)->EnableWindow(enbl);
	GetDlgItem(IDC_VECTOR_Z_LOCK_BTN)->EnableWindow(enbl);
}

void CGetVectorDlg::OnBnClickedDirXLockBtn()
{
	m_x_lock_btn.SetPressed(!m_x_lock_btn.IsPressed());
}

void CGetVectorDlg::OnBnClickedDirYLockBtn()
{
	m_y_lock_btn.SetPressed(!m_y_lock_btn.IsPressed());
}

void CGetVectorDlg::OnBnClickedDirZLockBtn()
{
	m_z_lock_btn.SetPressed(!m_z_lock_btn.IsPressed());
}

void CGetVectorDlg::OnBnClickedVectorXRadio()
{
	if (m_vector_type==X_VECTOR)
		return;
	m_vector_type = X_VECTOR;
	UpdateRadios();
//	GetParent()->PostMessage(WM_CHANGE_VECTOR);
	EnableUserNormalControls(FALSE);
	if (global_commander)
		global_commander->SendCommanderMessage(ICommander::CM_UPDATE_COMMAND_PANEL,NULL);
}

void CGetVectorDlg::OnBnClickedVectorYRadio()
{
	if (m_vector_type==Y_VECTOR)
		return;
	m_vector_type = Y_VECTOR;
	UpdateRadios();
//	GetParent()->PostMessage(WM_CHANGE_VECTOR);
	EnableUserNormalControls(FALSE);
	if (global_commander)
		global_commander->SendCommanderMessage(ICommander::CM_UPDATE_COMMAND_PANEL,NULL);
}

void CGetVectorDlg::OnBnClickedVectorZRadio()
{
	if (m_vector_type==Z_VECTOR)
		return;
	m_vector_type = Z_VECTOR;
	UpdateRadios();
//	GetParent()->PostMessage(WM_CHANGE_VECTOR);
	EnableUserNormalControls(FALSE);
	if (global_commander)
		global_commander->SendCommanderMessage(ICommander::CM_UPDATE_COMMAND_PANEL,NULL);
}

void CGetVectorDlg::OnBnClickedVectorUserRadio()
{
	if (m_vector_type==USER_VECTOR)
		return;
	m_vector_type = USER_VECTOR;
	UpdateRadios();
//	GetParent()->PostMessage(WM_CHANGE_VECTOR);
	EnableUserNormalControls(TRUE);
	if (global_commander)
		global_commander->SendCommanderMessage(ICommander::CM_UPDATE_COMMAND_PANEL,NULL);
}

BOOL CGetVectorDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

HBRUSH CGetVectorDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
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

void CGetVectorDlg::OnSize(UINT nType, int cx, int cy)
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

void CGetVectorDlg::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
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
	default:
		break;
	}
	__super::OnKeyDown(nChar, nRepCnt, nFlags);
}
