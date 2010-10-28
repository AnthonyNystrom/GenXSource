// RotatePanelDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "RotatePanelDlg.h"
#include "..//Tools//RotateCommand.h"
#include ".\rotatepaneldlg.h"


// CRotatePanelDlg dialog

IMPLEMENT_DYNAMIC(CRotatePanelDlg, CDialog)
CRotatePanelDlg::CRotatePanelDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CRotatePanelDlg::IDD, pParent)
	, m_commander(NULL)
	, m_rotate_type(0)
	, m_rotate_around(0)
	, m_sel_new(FALSE)
	, m_copies_cnt(1)
{
	m_tabs_counter = 1;
	m_enable_history = NULL;
	m_was_diasabled = false;
}

CRotatePanelDlg::~CRotatePanelDlg()
{
	if (m_enable_history)
		delete[] m_enable_history;
}

void CRotatePanelDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Radio(pDX, IDC_ROT_WITHOUT_DUBLE_RADIO, m_rotate_type);
	DDX_Radio(pDX, IDC_ROTATE_GLOBAL_ZERO_RADIO, m_rotate_around);
	DDX_Check(pDX, IDC_ROT_SEL_NEW_CHECK, m_sel_new);
	DDX_Control(pDX, IDC_ROT_COPIES_CNT_SPIN, m_cnt_spin);
	DDX_Text(pDX, IDC_ROT_COPIES_CNT_EDIT, m_copies_cnt);
	DDV_MinMaxInt(pDX, m_copies_cnt, 1, 100);
	DDX_Control(pDX, IDC_ROT_X_ANG_EDIT, m_x_rot_ang);
	DDX_Control(pDX, IDC_ROT_Y_ANG_EDIT, m_y_rot_ang);
	DDX_Control(pDX, IDC_ROT_Z_ANG_EDIT, m_z_rot_ang);
}


BEGIN_MESSAGE_MAP(CRotatePanelDlg, CDialog)
	ON_BN_CLICKED(IDC_ROTATE_GLOBAL_ZERO_RADIO, OnBnClickedRotateGlobalZeroRadio)
	ON_BN_CLICKED(IDC_ROTATE_OBJ_CENTERS_RADIO, OnBnClickedRotateObjCentersRadio)
	ON_BN_CLICKED(IDC_ROT_WITHOUT_DUBLE_RADIO, OnBnClickedRotWithoutDubleRadio)
	ON_BN_CLICKED(IDC_RADIO2, OnBnClickedRadio2)
	ON_NOTIFY(UDN_DELTAPOS, IDC_ROT_COPIES_CNT_SPIN, OnDeltaposRotCopiesCntSpin)
	ON_BN_CLICKED(IDC_ROT_SEL_NEW_CHECK, OnBnClickedRotSelNewCheck)
	ON_WM_SIZE()
	ON_WM_CHAR()
	ON_WM_KEYDOWN()
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()


void CRotatePanelDlg::OnOK()
{
	ASSERT(m_commander);
	//m_commander->OnEnter();
}

void CRotatePanelDlg::OnCancel()
{
	ASSERT(m_commander);
	//m_commander->OnEscape();
}


void    CRotatePanelDlg::EnableControls(bool enbl)
{
	if (m_enable_history==NULL)
		m_enable_history = new bool[15];
	if (enbl)
	{
		GetDlgItem(IDC_ROTATE_GLOBAL_ZERO_RADIO)->EnableWindow(m_enable_history[0]);
		GetDlgItem(IDC_ROTATE_OBJ_CENTERS_RADIO)->EnableWindow(m_enable_history[1]);
		GetDlgItem(IDC_ROT_ANGLES_LABEL)->EnableWindow(m_enable_history[2]);
		GetDlgItem(IDC_ROT_X_LABEL)->EnableWindow(m_enable_history[3]);
		GetDlgItem(IDC_ROT_Y_LABEL)->EnableWindow(m_enable_history[4]);
		GetDlgItem(IDC_ROT_Z_LABEL)->EnableWindow(m_enable_history[5]);
		GetDlgItem(IDC_ROT_X_ANG_EDIT)->EnableWindow(m_enable_history[6]);
		GetDlgItem(IDC_ROT_Y_ANG_EDIT)->EnableWindow(m_enable_history[7]);
		GetDlgItem(IDC_ROT_Z_ANG_EDIT)->EnableWindow(m_enable_history[8]);
		GetDlgItem(IDC_ROT_WITHOUT_DUBLE_RADIO)->EnableWindow(m_enable_history[9]);
		GetDlgItem(IDC_RADIO2)->EnableWindow(m_enable_history[10]);
		GetDlgItem(IDC_ROT_COPIES_COUNT)->EnableWindow(m_enable_history[11]);
		GetDlgItem(IDC_ROT_COPIES_CNT_EDIT)->EnableWindow(m_enable_history[12]);
		GetDlgItem(IDC_ROT_COPIES_CNT_SPIN)->EnableWindow(m_enable_history[13]);
		GetDlgItem(IDC_ROT_SEL_NEW_CHECK)->EnableWindow(m_enable_history[14]);
		m_was_diasabled = false;
	}
	else
	{
		if (!m_was_diasabled)
		{
			m_enable_history[0] = GetDlgItem(IDC_ROTATE_GLOBAL_ZERO_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[1] = GetDlgItem(IDC_ROTATE_OBJ_CENTERS_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[2] = GetDlgItem(IDC_ROT_ANGLES_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[3] = GetDlgItem(IDC_ROT_X_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[4] = GetDlgItem(IDC_ROT_Y_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[5] = GetDlgItem(IDC_ROT_Z_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[6] = GetDlgItem(IDC_ROT_X_ANG_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[7] = GetDlgItem(IDC_ROT_Y_ANG_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[8] = GetDlgItem(IDC_ROT_Z_ANG_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[9] = GetDlgItem(IDC_ROT_WITHOUT_DUBLE_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[10] = GetDlgItem(IDC_RADIO2)->IsWindowEnabled()!=0;
			m_enable_history[11] = GetDlgItem(IDC_ROT_COPIES_COUNT)->IsWindowEnabled()!=0;
			m_enable_history[12] = GetDlgItem(IDC_ROT_COPIES_CNT_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[13] = GetDlgItem(IDC_ROT_COPIES_CNT_SPIN)->IsWindowEnabled()!=0;
			m_enable_history[14] = GetDlgItem(IDC_ROT_SEL_NEW_CHECK)->IsWindowEnabled()!=0;
		}
		GetDlgItem(IDC_ROTATE_GLOBAL_ZERO_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROTATE_OBJ_CENTERS_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_ANGLES_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_X_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_Y_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_Z_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_X_ANG_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_Y_ANG_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_Z_ANG_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_WITHOUT_DUBLE_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_RADIO2)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_COPIES_COUNT)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_COPIES_CNT_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_COPIES_CNT_SPIN)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_SEL_NEW_CHECK)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
}


void  CRotatePanelDlg::SetAngles(const SG_VECTOR& vct)
{
	m_x_rot_ang.SetValue((float)vct.x/3.14159265f*180.0f);
	m_y_rot_ang.SetValue((float)vct.y/3.14159265f*180.0f);
	m_z_rot_ang.SetValue((float)vct.z/3.14159265f*180.0f);
	UpdateData(FALSE);
}

void  CRotatePanelDlg::GetAngles(SG_VECTOR& vct)
{
	UpdateData();
	vct.x= m_x_rot_ang.GetValue()/180.0f*3.14159265f;
	vct.y= m_y_rot_ang.GetValue()/180.0f*3.14159265f;
	vct.z= m_z_rot_ang.GetValue()/180.0f*3.14159265f;
}

bool CRotatePanelDlg::IsDouble()
{
	UpdateData();
	return (m_rotate_type!=0);
}

bool  CRotatePanelDlg::IsSelectNew()
{
	UpdateData();
	return ((m_sel_new)?true:false);
}

int   CRotatePanelDlg::GetCopiesCount()
{
	UpdateData();
	return m_copies_cnt;
}


BOOL CRotatePanelDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	if (m_rotate_type==0)
	{
		GetDlgItem(IDC_ROT_COPIES_COUNT)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_COPIES_CNT_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_COPIES_CNT_SPIN)->EnableWindow(FALSE);
		GetDlgItem(IDC_ROT_SEL_NEW_CHECK)->EnableWindow(FALSE);
	}

	m_cnt_spin.SetRange(1,100);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
// CRotatePanelDlg message handlers

void CRotatePanelDlg::OnBnClickedRotateGlobalZeroRadio()
{
	m_rotate_around = 0;
	m_commander->RedrawFromPanel();
}

void CRotatePanelDlg::OnBnClickedRotateObjCentersRadio()
{
	m_rotate_around = 1;
	m_commander->RedrawFromPanel();
}

void CRotatePanelDlg::OnBnClickedRotWithoutDubleRadio()
{
	GetDlgItem(IDC_ROT_COPIES_COUNT)->EnableWindow(FALSE);
	GetDlgItem(IDC_ROT_COPIES_CNT_EDIT)->EnableWindow(FALSE);
	GetDlgItem(IDC_ROT_COPIES_CNT_SPIN)->EnableWindow(FALSE);
	GetDlgItem(IDC_ROT_SEL_NEW_CHECK)->EnableWindow(FALSE);
	m_rotate_type = 0;
}

void CRotatePanelDlg::OnBnClickedRadio2()
{
	GetDlgItem(IDC_ROT_COPIES_COUNT)->EnableWindow(TRUE);
	GetDlgItem(IDC_ROT_COPIES_CNT_EDIT)->EnableWindow(TRUE);
	GetDlgItem(IDC_ROT_COPIES_CNT_SPIN)->EnableWindow(TRUE);
	GetDlgItem(IDC_ROT_SEL_NEW_CHECK)->EnableWindow(TRUE);
	m_rotate_type = 1;
}


void CRotatePanelDlg::OnDeltaposRotCopiesCntSpin(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	m_copies_cnt+=pNMUpDown->iDelta;
	if (m_copies_cnt<1) m_copies_cnt=1;
	if (m_copies_cnt>100) m_copies_cnt=100;
	*pResult = 0;
}

void CRotatePanelDlg::OnBnClickedRotSelNewCheck()
{
	m_sel_new = !m_sel_new;
}

void CRotatePanelDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	CWnd* x_edit = GetDlgItem(IDC_ROT_X_ANG_EDIT);
	CWnd* y_edit = GetDlgItem(IDC_ROT_Y_ANG_EDIT);
	CWnd* z_edit = GetDlgItem(IDC_ROT_Z_ANG_EDIT);
	CWnd* sep1 =  GetDlgItem(IDC_ROT_SEPARATOR_1);
	CWnd* sep2 =  GetDlgItem(IDC_ROT_SEPARATOR_2);

	if (x_edit && y_edit && z_edit && cx>10)
	{
		CRect rcEd;
		x_edit->GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		x_edit->MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left-3,rcEd.Height());

		y_edit->GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		y_edit->MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left-3,rcEd.Height());

		z_edit->GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		z_edit->MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left-3,rcEd.Height());

		sep1->GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		sep1->MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left,rcEd.Height());

		sep2->GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		sep2->MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left,rcEd.Height());
	}
}

void CRotatePanelDlg::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	switch(nChar) 
	{
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
		return;
	case VK_TAB:
		GetDlgItem(IDC_ROT_X_ANG_EDIT+((m_tabs_counter++)%3))->SetFocus();
		((CEdit*)GetFocus())->SetSel(0,-1);
		return;
	default:
		{
			CWnd* focW = GetFocus();
			if (focW==&m_y_rot_ang)
			{
				m_y_rot_ang.OnChar(nChar,nRepCnt,nFlags);
				return;
			}
			else
				if (focW==&m_z_rot_ang)
				{
					m_z_rot_ang.OnChar(nChar,nRepCnt,nFlags);
					return;
				}
				else
					if (focW==&m_x_rot_ang)
					{
						m_x_rot_ang.OnChar(nChar,nRepCnt,nFlags);
						return;
					}
					else
					{
						m_x_rot_ang.SendMessage(GET_DIALOGS_MESSAGE_PRE_SET_FOCUS,nChar,nRepCnt);
					}

					return;
		}
		break;
	}

	CDialog::OnChar(nChar, nRepCnt, nFlags);
}

void CRotatePanelDlg::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	switch(nChar) 
	{
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
		return;
	case VK_TAB:
		GetDlgItem(IDC_ROT_X_ANG_EDIT+((m_tabs_counter++)%3))->SetFocus();
		((CEdit*)GetFocus())->SetSel(0,-1);
		return;
	default:
		break;
	}
	__super::OnKeyDown(nChar, nRepCnt, nFlags);
}

BOOL CRotatePanelDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

HBRUSH CRotatePanelDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	// Call the base class implementation first! Otherwise, it may
	// undo what we are trying to accomplish here.
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	CWnd* aa = GetDlgItem(IDC_ROT_COPIES_CNT_EDIT);

	if (aa->m_hWnd==pWnd->m_hWnd)
		return hbr;

	if (nCtlColor==CTLCOLOR_STATIC && (pWnd!=&m_x_rot_ang) 
		&& (pWnd!=&m_y_rot_ang) && (pWnd!=&m_z_rot_ang))
	{
		pDC->SetTextColor(0);
		//pDC->SetBkColor(RGB(255,255,255));
		pDC->SetBkMode(TRANSPARENT);
		hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);
	}

	return hbr;

}
