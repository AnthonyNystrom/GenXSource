// GetObjectsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//..//NuGenDimension.h"
#include "GetObjectsDlg.h"
#include ".\getobjectsdlg.h"


// CGetObjectsDlg dialog

IMPLEMENT_DYNAMIC(CGetObjectsDlg, CDialog)
CGetObjectsDlg::CGetObjectsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CGetObjectsDlg::IDD, pParent)
{
	m_fill_function = NULL;
	m_enable_history = NULL;
	m_was_diasabled = false;
}

CGetObjectsDlg::~CGetObjectsDlg()
{
	m_objcts.clear();
	if (m_enable_history)
		delete[] m_enable_history;
}

void CGetObjectsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_GET_OBJECTS_LIST, m_list);
}


BEGIN_MESSAGE_MAP(CGetObjectsDlg, CDialog)
	ON_WM_SIZE()
	ON_WM_ERASEBKGND()
	ON_NOTIFY(LVN_GETDISPINFO, IDC_GET_OBJECTS_LIST, OnLvnGetdispinfoGetObjectsList)
	ON_NOTIFY(NM_CLICK, IDC_GET_OBJECTS_LIST, OnNMClickGetObjectsList)
	ON_BN_CLICKED(IDC_GET_OBJECTS_FINISH_BUTTON, OnBnClickedGetObjectsFinishButton)
END_MESSAGE_MAP()


// CGetObjectsDlg message handlers

void CGetObjectsDlg::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnOK();
}

void CGetObjectsDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnCancel();
}

IBaseInterfaceOfGetDialogs::DLG_TYPE CGetObjectsDlg::GetType()
{
	return IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG;
}

CWnd*   CGetObjectsDlg::GetWindow()
{
	return this;
}


void    CGetObjectsDlg::EnableControls(bool enbl)
{
	if (m_enable_history==NULL)
		m_enable_history = new bool[2];
	if (enbl)
	{
		GetDlgItem(IDC_GET_OBJECTS_LIST)->EnableWindow(m_enable_history[0]);
		GetDlgItem(IDC_GET_OBJECTS_FINISH_BUTTON)->EnableWindow(m_enable_history[1]);
		m_was_diasabled = false;
	}
	else
	{
		if (!m_was_diasabled)
		{
			m_enable_history[0] = GetDlgItem(IDC_GET_OBJECTS_LIST)->IsWindowEnabled()!=0;
			m_enable_history[1] = GetDlgItem(IDC_GET_OBJECTS_FINISH_BUTTON)->IsWindowEnabled()!=0;
		}
		GetDlgItem(IDC_GET_OBJECTS_LIST)->EnableWindow(FALSE);
		GetDlgItem(IDC_GET_OBJECTS_FINISH_BUTTON)->EnableWindow(FALSE);
		m_was_diasabled = true;
	}
	Invalidate();
}

void CGetObjectsDlg::RemoveAllObjects()
{
	m_list.DeleteAllItems();
	m_list.SetItemCount(0);
	m_objcts.clear();
}

void CGetObjectsDlg::SetMultiselectMode(bool msm)
{
	m_list.SetMultiSelectMode(msm);
}

void CGetObjectsDlg::FillList(LPFUNC_FILL_OBJECTS_LIST isAdd)
{
	if (isAdd)
		m_fill_function = isAdd;
	/*m_objcts.clear();
	m_list.SetItemCount(0);*/
	sgCObject*  curObj = sgGetScene()->GetObjectsList()->GetHead();
	while (curObj) 
	{
		if (!m_fill_function ||
			(m_fill_function && m_fill_function(curObj)))
		{
			OBJCTS tmpS;
			tmpS.objc = curObj;
			tmpS.obName = CString(curObj->GetName());
			m_objcts.push_back(tmpS);
		}
		curObj = sgGetScene()->GetObjectsList()->GetNext(curObj);
	}
	m_list.SetItemCount(m_objcts.size());
}

void  CGetObjectsDlg::AddObject(sgCObject* obj, bool sel)
{
	OBJCTS tmpS;
	tmpS.objc = obj;
	tmpS.obName = CString(obj->GetName());
	m_objcts.push_back(tmpS);
	m_list.SetItemCount(m_objcts.size());
	if (sel)
		m_list.SetItemState(m_objcts.size()-1, LVIS_SELECTED, LVIS_SELECTED);
	else
		m_list.SetItemState(m_objcts.size()-1, 0, LVIS_SELECTED);
}

void  CGetObjectsDlg::RemoveObject(sgCObject* obj)
{
	size_t sz = m_objcts.size();
	for (size_t i=0;i<sz;i++)
		if (m_objcts[i].objc==obj)
		{
			m_objcts.erase(m_objcts.begin()+i);
			m_list.SetItemCount(m_objcts.size());
			return;
		}
}

void  CGetObjectsDlg::SelectObject(sgCObject* obj, bool sel)
{
	size_t sz = m_objcts.size();
	if (obj==NULL)
	{
		for (size_t i=0;i<sz;i++)
			m_list.SetItemState(i, 0, LVIS_SELECTED);
		return;
	}
	
	for (size_t i=0;i<sz;i++)
		if (m_objcts[i].objc==obj)
		{
			if (sel)
				m_list.SetItemState(i, LVIS_SELECTED, LVIS_SELECTED);
			else
				m_list.SetItemState(i, 0, LVIS_SELECTED);
			return;
		}
}

BOOL CGetObjectsDlg::OnInitDialog()
{
	__super::OnInitDialog();


	m_list.InsertColumn(0,"");
	CRect rct;
	m_list.GetWindowRect(rct);
	m_list.SetColumnWidth(0,rct.Width());

	m_list.SetExtendedStyle(LVS_EX_GRIDLINES|LVS_EX_FULLROWSELECT);

	m_list.SetMultiSelectMode(true);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CGetObjectsDlg::OnSize(UINT nType, int cx, int cy)
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

BOOL CGetObjectsDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return  TRUE;//__super::OnEraseBkgnd(pDC);
}

void CGetObjectsDlg::OnLvnGetdispinfoGetObjectsList(NMHDR *pNMHDR, LRESULT *pResult)
{
	LV_DISPINFO* pDispInfo = (LV_DISPINFO*)pNMHDR;
	LV_ITEM* pItem= &(pDispInfo)->item;

	DWORD n = pItem->iItem;	

	if (n<0 || n>=m_objcts.size())
		return;

	if (pItem->mask & LVIF_TEXT) // требуетс€ текст элемента?
	{	//strcpy( pItem->pszText, m_objcts[n].obName);//#OBSOLETE RISK
		strcpy_s(pItem->pszText, 255, m_objcts[n].obName);
	}

	/*if (pItem->mask & LVIF_IMAGE) // требуютс€ картинки?
	{
	pItem->iImage = pDoc->GetImage(n);
	pItem->state = pDoc->GetStateImage(n);
	}*/

	*pResult = 0;
}

void CGetObjectsDlg::OnNMClickGetObjectsList(NMHDR *pNMHDR, LRESULT *pResult)
{
	NMITEMACTIVATE* nm=(NMITEMACTIVATE*)pNMHDR;
	int s = nm->iItem;
	int d=nm->iSubItem;

	if (global_commander)
		global_commander->SendCommanderMessage(ICommander::CM_SELECT_OBJECT,
													m_objcts[s].objc);
}

void CGetObjectsDlg::OnBnClickedGetObjectsFinishButton()
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
