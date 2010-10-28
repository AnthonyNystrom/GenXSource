// TranslatePanelDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "TranslatePanelDlg.h"
#include "..//Tools//TranslateCommand.h"
#include ".\translatepaneldlg.h"


// CTranslatePanelDlg dialog

IMPLEMENT_DYNAMIC(CTranslatePanelDlg, CDialog)
CTranslatePanelDlg::CTranslatePanelDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CTranslatePanelDlg::IDD, pParent)
	, m_commander(NULL)
	, m_trans_type(0)
	, m_sel_new(FALSE)
	, m_copies_cnt(1)
{
	m_tabs_counter = 1;
	m_enable_history = NULL;
	m_was_diasabled = false;
}

CTranslatePanelDlg::~CTranslatePanelDlg()
{
	if (m_enable_history)
		delete[] m_enable_history;
}

void CTranslatePanelDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Radio(pDX, IDC_TRANS_WITHOUT_DUBLE_RADIO, m_trans_type);
	DDX_Check(pDX, IDC_TRANS_SEL_NEW_CHECK, m_sel_new);
	DDX_Control(pDX, IDC_TRANS_COPIES_CNT_SPIN, m_cnt_spin);
	DDX_Text(pDX, IDC_TRANS_COPIES_CNT_EDIT, m_copies_cnt);
	DDV_MinMaxInt(pDX, m_copies_cnt, 1, 100);
	DDX_Control(pDX, IDC_TRANS_VEC_X_EDIT, m_trans_x_vec);
	DDX_Control(pDX, IDC_TRANS_VEC_Y_EDIT, m_trans_y_vec);
	DDX_Control(pDX, IDC_TRANS_VEC_Z_EDIT, m_trans_z_vec);
}


BEGIN_MESSAGE_MAP(CTranslatePanelDlg, CDialog)
	ON_BN_CLICKED(IDC_TRANS_WITHOUT_DUBLE_RADIO, OnBnClickedTransWithoutDubleRadio)
	ON_BN_CLICKED(IDC_RADIO2, OnBnClickedRadio2)
	ON_NOTIFY(UDN_DELTAPOS, IDC_TRANS_COPIES_CNT_SPIN, OnDeltaposTransCopiesCntSpin)
	ON_BN_CLICKED(IDC_TRANS_SEL_NEW_CHECK, OnBnClickedTransSelNewCheck)
	ON_WM_SIZE()
	ON_WM_CHAR()
	ON_WM_KEYDOWN()
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()


// CTranslatePanelDlg message handlers

void CTranslatePanelDlg::OnOK()
{
	ASSERT(m_commander);
	//m_commander->OnEnter();
}

void CTranslatePanelDlg::OnCancel()
{
	ASSERT(m_commander);
	//m_commander->OnEscape();
}


void    CTranslatePanelDlg::EnableControls(bool enbl)
{
	if (m_enable_history==NULL)
		m_enable_history = new bool[13];
	if (enbl)
	{
		GetDlgItem(IDC_TRANS_TOP_LABEL)->EnableWindow(m_enable_history[0]);
		GetDlgItem(IDC_TRANS_VEC_X_LABEL)->EnableWindow(m_enable_history[1]);
		GetDlgItem(IDC_TRANS_VEC_Y_LABEL)->EnableWindow(m_enable_history[2]);
		GetDlgItem(IDC_TRANS_VEC_Z_LABEL)->EnableWindow(m_enable_history[3]);
		GetDlgItem(IDC_TRANS_VEC_X_EDIT)->EnableWindow(m_enable_history[4]);
		GetDlgItem(IDC_TRANS_VEC_Y_EDIT)->EnableWindow(m_enable_history[5]);
		GetDlgItem(IDC_TRANS_VEC_Z_EDIT)->EnableWindow(m_enable_history[6]);
		GetDlgItem(IDC_TRANS_WITHOUT_DUBLE_RADIO)->EnableWindow(m_enable_history[7]);
		GetDlgItem(IDC_RADIO2)->EnableWindow(m_enable_history[8]);
		GetDlgItem(IDC_TRANS_COPIES_COUNT)->EnableWindow(m_enable_history[9]);
		GetDlgItem(IDC_TRANS_COPIES_CNT_EDIT)->EnableWindow(m_enable_history[10]);
		GetDlgItem(IDC_TRANS_COPIES_CNT_SPIN)->EnableWindow(m_enable_history[11]);
		GetDlgItem(IDC_TRANS_SEL_NEW_CHECK)->EnableWindow(m_enable_history[12]);
		m_was_diasabled = false;
	}
	else
	{
		if (!m_was_diasabled)
		{
			m_enable_history[0] = GetDlgItem(IDC_TRANS_TOP_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[1] = GetDlgItem(IDC_TRANS_VEC_X_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[2] = GetDlgItem(IDC_TRANS_VEC_Y_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[3] = GetDlgItem(IDC_TRANS_VEC_Z_LABEL)->IsWindowEnabled()!=0;
			m_enable_history[4] = GetDlgItem(IDC_TRANS_VEC_X_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[5] = GetDlgItem(IDC_TRANS_VEC_Y_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[6] = GetDlgItem(IDC_TRANS_VEC_Z_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[7] = GetDlgItem(IDC_TRANS_WITHOUT_DUBLE_RADIO)->IsWindowEnabled()!=0;
			m_enable_history[8] = GetDlgItem(IDC_RADIO2)->IsWindowEnabled()!=0;
			m_enable_history[9] = GetDlgItem(IDC_TRANS_COPIES_COUNT)->IsWindowEnabled()!=0;
			m_enable_history[10] = GetDlgItem(IDC_TRANS_COPIES_CNT_EDIT)->IsWindowEnabled()!=0;
			m_enable_history[11] = GetDlgItem(IDC_TRANS_COPIES_CNT_SPIN)->IsWindowEnabled()!=0;
			m_enable_history[12] = GetDlgItem(IDC_TRANS_SEL_NEW_CHECK)->IsWindowEnabled()!=0;
		}
		GetDlgItem(IDC_TRANS_TOP_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_VEC_X_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_VEC_Y_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_VEC_Z_LABEL)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_VEC_X_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_VEC_Y_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_VEC_Z_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_WITHOUT_DUBLE_RADIO)->EnableWindow(FALSE);
		GetDlgItem(IDC_RADIO2)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_COPIES_COUNT)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_COPIES_CNT_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_COPIES_CNT_SPIN)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_SEL_NEW_CHECK)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
}


void  CTranslatePanelDlg::SetVector(const SG_VECTOR& vct)
{
	m_trans_x_vec.SetValue((float)vct.x);
	m_trans_y_vec.SetValue((float)vct.y);
	m_trans_z_vec.SetValue((float)vct.z);
	UpdateData(FALSE);
}

void  CTranslatePanelDlg::GetVector(SG_VECTOR& vct)
{
	UpdateData();
	 vct.x= m_trans_x_vec.GetValue();
	 vct.y= m_trans_y_vec.GetValue();
	 vct.z= m_trans_z_vec.GetValue();
}

bool CTranslatePanelDlg::IsDouble()
{
	UpdateData();
	return (m_trans_type!=0);
}

bool  CTranslatePanelDlg::IsSelectNew()
{
	UpdateData();
	return ((m_sel_new)?true:false);
}

int   CTranslatePanelDlg::GetCopiesCount()
{
	UpdateData();
	return m_copies_cnt;
}

void CTranslatePanelDlg::OnBnClickedTransWithoutDubleRadio()
{
	GetDlgItem(IDC_TRANS_COPIES_COUNT)->EnableWindow(FALSE);
	GetDlgItem(IDC_TRANS_COPIES_CNT_EDIT)->EnableWindow(FALSE);
	GetDlgItem(IDC_TRANS_COPIES_CNT_SPIN)->EnableWindow(FALSE);
	GetDlgItem(IDC_TRANS_SEL_NEW_CHECK)->EnableWindow(FALSE);
	m_trans_type = 0;
}

void CTranslatePanelDlg::OnBnClickedRadio2()
{
	GetDlgItem(IDC_TRANS_COPIES_COUNT)->EnableWindow(TRUE);
	GetDlgItem(IDC_TRANS_COPIES_CNT_EDIT)->EnableWindow(TRUE);
	GetDlgItem(IDC_TRANS_COPIES_CNT_SPIN)->EnableWindow(TRUE);
	GetDlgItem(IDC_TRANS_SEL_NEW_CHECK)->EnableWindow(TRUE);
	m_trans_type = 1;
}

BOOL CTranslatePanelDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	if (m_trans_type==0)
	{
		GetDlgItem(IDC_TRANS_COPIES_COUNT)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_COPIES_CNT_EDIT)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_COPIES_CNT_SPIN)->EnableWindow(FALSE);
		GetDlgItem(IDC_TRANS_SEL_NEW_CHECK)->EnableWindow(FALSE);
	}

	m_cnt_spin.SetRange(1,100);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CTranslatePanelDlg::OnDeltaposTransCopiesCntSpin(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	m_copies_cnt+=pNMUpDown->iDelta;
	if (m_copies_cnt<1) m_copies_cnt=1;
	if (m_copies_cnt>100) m_copies_cnt=100;
	*pResult = 0;
}

void CTranslatePanelDlg::OnBnClickedTransSelNewCheck()
{
	m_sel_new = !m_sel_new;
}

void CTranslatePanelDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	CWnd* x_edit = GetDlgItem(IDC_TRANS_VEC_X_EDIT);
	CWnd* y_edit = GetDlgItem(IDC_TRANS_VEC_Y_EDIT);
	CWnd* z_edit = GetDlgItem(IDC_TRANS_VEC_Z_EDIT);
	CWnd* sep =  GetDlgItem(IDC_TRANS_SEPARATOR);

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
		
		sep->GetWindowRect(rcEd);
		ScreenToClient(rcEd);
		sep->MoveWindow(rcEd.left,rcEd.top,cx-rcEd.left,rcEd.Height());
	

	}
}

void CTranslatePanelDlg::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	switch(nChar) 
	{
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
		return;
	case VK_TAB:
		GetDlgItem(IDC_TRANS_VEC_X_EDIT+((m_tabs_counter++)%3))->SetFocus();
		((CEdit*)GetFocus())->SetSel(0,-1);
		return;
	default:
		{
			CWnd* focW = GetFocus();
			if (focW==&m_trans_y_vec)
			{
				m_trans_y_vec.OnChar(nChar,nRepCnt,nFlags);
				return;
			}
			else
				if (focW==&m_trans_z_vec)
				{
					m_trans_z_vec.OnChar(nChar,nRepCnt,nFlags);
					return;
				}
				else
					if (focW==&m_trans_x_vec)
					{
						m_trans_x_vec.OnChar(nChar,nRepCnt,nFlags);
						return;
					}
					else
					{
						m_trans_x_vec.SendMessage(GET_DIALOGS_MESSAGE_PRE_SET_FOCUS,nChar,nRepCnt);
					}

					return;
		}
		break;
	}

	CDialog::OnChar(nChar, nRepCnt, nFlags);

}

void CTranslatePanelDlg::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	switch(nChar) 
	{
	case VK_RETURN:
	case VK_ESCAPE:
		ASSERT(0);
		return;
	case VK_TAB:
		GetDlgItem(IDC_TRANS_VEC_X_EDIT+((m_tabs_counter++)%3))->SetFocus();
		((CEdit*)GetFocus())->SetSel(0,-1);
		return;
	default:
		break;
	}
	__super::OnKeyDown(nChar, nRepCnt, nFlags);
}

BOOL CTranslatePanelDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

HBRUSH CTranslatePanelDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	// Call the base class implementation first! Otherwise, it may
	// undo what we are trying to accomplish here.
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	CWnd* aa = GetDlgItem(IDC_TRANS_COPIES_CNT_EDIT);

	if (aa->m_hWnd==pWnd->m_hWnd)
		return hbr;
	
	if (nCtlColor==CTLCOLOR_STATIC && (pWnd!=&m_trans_x_vec) 
		&& (pWnd!=&m_trans_y_vec) && (pWnd!=&m_trans_z_vec))
	{
		pDC->SetTextColor(0);
		//pDC->SetBkColor(RGB(255,255,255));
		pDC->SetBkMode(TRANSPARENT);
		hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);
	}

	return hbr;
}
