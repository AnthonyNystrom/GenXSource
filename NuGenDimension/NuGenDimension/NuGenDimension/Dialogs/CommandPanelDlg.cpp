// CommandPanelDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "CommandPanelDlg.h"

#include "GetDialogs//GetPointDlg.h"
#include "GetDialogs//GetVectorDlg.h"
#include "GetDialogs//GetNumberDlg.h"
#include "GetDialogs//GetObjectsDlg.h"
#include "GetDialogs//GetOneDlg.h"
#include "GetDialogs//SelectPointDlg.h"

// CCommandPanelDlg dialog

IMPLEMENT_DYNAMIC(CCommandPanelDlg, CDialog)
CCommandPanelDlg::CCommandPanelDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CCommandPanelDlg::IDD, pParent)
{
}

CCommandPanelDlg::~CCommandPanelDlg()
{
}

bool CCommandPanelDlg::RemoveAllDialogs()
{
	m_wndRollupCtrl.RemoveAllPages();
	RecalcPlaces();
	return true;
}

void  CCommandPanelDlg::DrawGroupFrame(CDC* pDC, const CRect& rct, 
							   const int leftLab, const int rightLab)
{
	::DrawGroupFrame(pDC,rct,leftLab,rightLab);
}

void CCommandPanelDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CCommandPanelDlg, CDialog)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

CWnd*  CCommandPanelDlg::GetDialogsContainerWindow()
{
	return &m_wndRollupCtrl;
}

bool	CCommandPanelDlg::AddDialog(IBaseInterfaceOfGetDialogs* dlg, const char* cptn,
									bool radio)
{
	if (dlg==NULL)
	{
		ASSERT(0);
		return false;
	}
	m_wndRollupCtrl.InsertPage(cptn, dlg, radio, FALSE);

	return true;
}

IBaseInterfaceOfGetDialogs*  CCommandPanelDlg::AddDialog(IBaseInterfaceOfGetDialogs::DLG_TYPE dlgT, 
														 const char* cptn,
														 bool radio)
{
	APP_SWITCH_RESOURCE
	switch(dlgT) 
	{
	case IBaseInterfaceOfGetDialogs::GET_POINT_DLG:
		{
			CGetPointDlg* dl = new CGetPointDlg;
			dl->Create(MAKEINTRESOURCE(IDD_GET_POINT_DLG), &m_wndRollupCtrl);
			m_wndRollupCtrl.InsertPage(cptn, dl ,radio);
			return dl;
		}
		break;
	case IBaseInterfaceOfGetDialogs::GET_VECTOR_DLG:
		{
			CGetVectorDlg* dl = new CGetVectorDlg;
			dl->Create(MAKEINTRESOURCE(IDD_GET_VECTOR_DLG), &m_wndRollupCtrl);
			m_wndRollupCtrl.InsertPage(cptn, dl ,radio);
			return dl;
		}
		break;
	case IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG:
		{
			CGetNumberDlg* dl = new CGetNumberDlg;
			dl->Create(MAKEINTRESOURCE(IDD_GET_NUMBER_DLG), &m_wndRollupCtrl);
			m_wndRollupCtrl.InsertPage(cptn, dl ,radio);
			return dl;
		}
		break;
	case IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG:
		{
			CGetObjectsDlg* dl = new CGetObjectsDlg;
			dl->Create(MAKEINTRESOURCE(IDD_GET_OBJECTS_DLG), &m_wndRollupCtrl);
			m_wndRollupCtrl.InsertPage(cptn, dl ,radio);
			return dl;
		}
		break;
	case IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG:
		{
			CSelectPointDlg* dl = new CSelectPointDlg;
			dl->Create(MAKEINTRESOURCE(IDD_SELECT_POINT_DLG), &m_wndRollupCtrl);
			m_wndRollupCtrl.InsertPage(cptn, dl ,radio);
			return dl;
		}
		break;
	case IBaseInterfaceOfGetDialogs::COMBO_DLG:
		{
			CGetOneDlg* dl = new CGetOneDlg;
			dl->Create(MAKEINTRESOURCE(IDD_GET_ONE_DGL), &m_wndRollupCtrl);
			m_wndRollupCtrl.InsertPage(cptn, dl ,radio);
			return dl;
		}
		break;
	default:
		return NULL;
	}
	return NULL;
}

bool  CCommandPanelDlg::RemoveDialog(IBaseInterfaceOfGetDialogs* ddd)
{
	return m_wndRollupCtrl.RemovePage(ddd);

}

bool  CCommandPanelDlg::RemoveDialog(unsigned int d_numb)
{
	return m_wndRollupCtrl.RemovePage(d_numb);
}

bool  CCommandPanelDlg::RenameRadio(unsigned int ind, const char* lab)
{
	return m_wndRollupCtrl.RenamePage(ind,lab);
}

void   CCommandPanelDlg::EnableRadio(unsigned int ind,bool enbl)
{
	m_wndRollupCtrl.EnableRadio(ind, enbl);
}

void   CCommandPanelDlg::SetActiveRadio(unsigned int ind)
{
	m_wndRollupCtrl.SetActiveRadio(ind);
}

int CCommandPanelDlg::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;

	m_wndRollupCtrl.Create(WS_VISIBLE|WS_CHILD, 
		CRect(0,0,187,162), this, 1);	
	//Add some pages

	return 0;
}

void  CCommandPanelDlg::RecalcPlaces()
{
	if (!::IsWindow(m_hWnd))
		return;

	RECT  mainRct;
	this->GetWindowRect(&mainRct);

	int   mainXSz = mainRct.right - mainRct.left;
	int   mainYSz = mainRct.bottom - mainRct.top;
	
	int curYPos=0;
	m_wndRollupCtrl.SetWindowPos(&wndTop,0,curYPos,mainXSz,mainYSz-curYPos
		,SWP_SHOWWINDOW);

	Invalidate();

}

void CCommandPanelDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	RecalcPlaces();
}

BOOL CCommandPanelDlg::DestroyWindow()
{
	RemoveAllDialogs();
	return CDialog::DestroyWindow();
}

void CCommandPanelDlg::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnOK();
}

void CCommandPanelDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnCancel();
}

#include "..//MemDC.h"
#define USE_MEM_DC // Comment this out, if you don't want to use CMemDC

BOOL CCommandPanelDlg::OnEraseBkgnd(CDC* pDC)
{
#ifdef USE_MEM_DC
	CMemDC memDC(pDC);
#else
	CDC* memDC = pDC;
#endif

	CRect rrr;
	GetClientRect(rrr);
	themeData.DrawThemedRect(&memDC,&rrr,FALSE);

	return TRUE;//CDialog::OnEraseBkgnd(pDC);
}
