// SelectPointDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//..//NuGenDimension.h"
#include "SelectPointDlg.h"
#include ".\selectpointdlg.h"


// CSelectPointDlg dialog

IMPLEMENT_DYNAMIC(CSelectPointDlg, CDialog)
CSelectPointDlg::CSelectPointDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSelectPointDlg::IDD, pParent)
{
	m_enable_history = NULL;
	m_was_diasabled = false;
}

CSelectPointDlg::~CSelectPointDlg()
{
	m_points.clear();
	if (m_enable_history)
		delete[] m_enable_history;
}

void CSelectPointDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_SELECT_POINT_LIST, m_list);
}


BEGIN_MESSAGE_MAP(CSelectPointDlg, CDialog)
	ON_WM_SIZE()
	ON_NOTIFY(LVN_GETDISPINFO, IDC_SELECT_POINT_LIST, OnLvnGetdispinfoSelectPointList)
	ON_NOTIFY(NM_CLICK, IDC_SELECT_POINT_LIST, OnNMClickSelectPointList)
	ON_WM_ERASEBKGND()
	ON_BN_CLICKED(IDC_SELECT_POINT_FINISH_BUTTON, OnBnClickedSelectPointFinishButton)
END_MESSAGE_MAP()



IBaseInterfaceOfGetDialogs::DLG_TYPE CSelectPointDlg::GetType()
{
	return IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG;
}

CWnd*   CSelectPointDlg::GetWindow()
{
	return this;
}


void    CSelectPointDlg::EnableControls(bool enbl)
{
	if (m_enable_history==NULL)
		m_enable_history = new bool[2];
	if (enbl)
	{
		GetDlgItem(IDC_SELECT_POINT_LIST)->EnableWindow(m_enable_history[0]);
		GetDlgItem(IDC_SELECT_POINT_FINISH_BUTTON)->EnableWindow(m_enable_history[1]);
		m_was_diasabled = false;
	}
	else
	{
		if (!m_was_diasabled)
		{
			m_enable_history[0] = GetDlgItem(IDC_SELECT_POINT_LIST)->IsWindowEnabled()!=0;
			m_enable_history[1] = GetDlgItem(IDC_SELECT_POINT_FINISH_BUTTON)->IsWindowEnabled()!=0;
		}
		GetDlgItem(IDC_SELECT_POINT_LIST)->EnableWindow(FALSE);
		GetDlgItem(IDC_SELECT_POINT_FINISH_BUTTON)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
	Invalidate();
}


void   CSelectPointDlg::AddPoint(double xP,double yP,double zP)
{
	PNTS tmpS;
	tmpS.pX = xP;
	tmpS.pY = yP;
	tmpS.pZ = zP;
	
	tmpS.pStr.Format("X=%f  Y=%f  Z=%f",xP,yP,zP);
	m_points.push_back(tmpS);
	m_list.SetItemCount(m_points.size());
}

void   CSelectPointDlg::RemoveAllPoints()
{
	m_list.DeleteAllItems();
	m_points.clear();
	m_list.SetItemCount(0);
}

void   CSelectPointDlg::SetCurrentPoint(unsigned int ind)
{
	if (ind>=m_points.size())
		return;
	m_list.SetItemState(ind, LVNI_SELECTED, LVNI_SELECTED);
}

unsigned int  CSelectPointDlg::GetCurrentPoint()
{
	POSITION pos = m_list.GetFirstSelectedItemPosition();
	return m_list.GetNextSelectedItem(pos);
}



// CSelectPointDlg message handlers

void CSelectPointDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnCancel();
}

void CSelectPointDlg::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnOK();
}

BOOL CSelectPointDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	//m_list.SubclassDlgItem(IDC_SELECT_POINT_LIST, this);

	m_list.InsertColumn(0,"");
	CRect rct;
	m_list.GetWindowRect(rct);
	m_list.SetColumnWidth(0,rct.Width());

	// Разрешаем использовать иконки состояния
	//m_list.SendMessage( LVM_SETCALLBACKMASK , LVIS_STATEIMAGEMASK , 0);

	// Выделение во всю строку, рисование сетки
	m_list.SetExtendedStyle(LVS_EX_GRIDLINES|LVS_EX_FULLROWSELECT);

	m_list.SetMultiSelectMode(false);

	
	//m_list.ModifyStyle(WS_BORDER,0, 0);
	//m_list.ModifyStyleEx(WS_EX_CLIENTEDGE,WS_EX_TRANSPARENT,SWP_DRAWFRAME|SWP_FRAMECHANGED);


	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CSelectPointDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	if (::IsWindow(m_list.m_hWnd))
	{
		CRect rrr;
		m_list.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_list.MoveWindow(rrr.left,rrr.top,
			cx-2*rrr.left,rrr.Height());
		m_list.GetWindowRect(rrr);
		m_list.SetColumnWidth(0,rrr.Width()-5);
	}
}

void CSelectPointDlg::OnLvnGetdispinfoSelectPointList(NMHDR *pNMHDR, LRESULT *pResult)
{
	LV_DISPINFO* pDispInfo = (LV_DISPINFO*)pNMHDR;
	LV_ITEM* pItem= &(pDispInfo)->item;

	DWORD n = pItem->iItem;	

	if (n<0 || n>=m_points.size())
		return;

	if (pItem->mask & LVIF_TEXT) // требуется текст элемента?
	{	//strcpy( pItem->pszText, m_points[n].pStr); #OBSOLETE RISK
		strcpy_s(pItem->pszText, 255, m_points[n].pStr);
	}

	/*if (pItem->mask & LVIF_IMAGE) // требуются картинки?
	{
		pItem->iImage = pDoc->GetImage(n);
		pItem->state = pDoc->GetStateImage(n);
	}*/

	*pResult = 0;
}

void CSelectPointDlg::OnNMClickSelectPointList(NMHDR *pNMHDR, LRESULT *pResult)
{
	NMITEMACTIVATE* nm=(NMITEMACTIVATE*)pNMHDR;
	int s = nm->iItem;
	int d=nm->iSubItem;

	if (global_commander)
		global_commander->SendCommanderMessage(ICommander::CM_UPDATE_COMMAND_PANEL,NULL);
}

BOOL CSelectPointDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

void CSelectPointDlg::OnBnClickedSelectPointFinishButton()
{
	if(global_commander)
	{
		MSG ms;
		ms.message = WM_KEYDOWN;
		ms.wParam=VK_RETURN;
		global_commander->PreTranslateMessage(&ms);
	}
	else
	{
		ASSERT(0);
	}
}
