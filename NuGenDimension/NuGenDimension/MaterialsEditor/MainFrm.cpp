// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "MaterialsEditor.h"

#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMainFrame

IMPLEMENT_DYNCREATE(CMainFrame, CEGFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CEGFrameWnd)
	ON_WM_CREATE()
	// Global help commands
	ON_COMMAND(ID_HELP_FINDER, CEGFrameWnd::OnHelpFinder)
	ON_COMMAND(ID_HELP, CEGFrameWnd::OnHelp)
	ON_COMMAND(ID_CONTEXT_HELP, CEGFrameWnd::OnContextHelp)
	ON_COMMAND(ID_DEFAULT_HELP, CEGFrameWnd::OnHelpFinder)
//	ON_COMMAND(ID_SYSTEM_PANEL, OnSystemPanel)
END_MESSAGE_MAP()

static UINT indicators[] =
{
	ID_SEPARATOR,           // status line indicator
	ID_INDICATOR_CAPS,
	ID_INDICATOR_NUM,
	ID_INDICATOR_SCRL,
};


// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
	// TODO: add member initialization code here
}

CMainFrame::~CMainFrame()
{
}

static WORD menu_icons[] = { IDB_TOOLBAR,
16,16,

ID_FILE_NEW,
ID_FILE_OPEN,
ID_FILE_SAVE,

NULL
};

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CEGFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	EnableDocking(CBRS_ALIGN_ANY);
	if (!m_wndMenuBar.Create(this,WS_CHILD | WS_VISIBLE | CBRS_ALIGN_TOP|CBRS_SIZE_DYNAMIC|CBRS_TOOLTIPS|CBRS_GRIPPER))
	{
		TRACE0("Failed to create menu bar\n");
		return -1;      // fail to create
	}

	m_wndMenuBar.EnableDocking(CBRS_ALIGN_ANY);
	EnableDocking(CBRS_ALIGN_ANY);
	DockControlBar(&m_wndMenuBar);

	//m_DefaultNewMenu.LoadToolBar( menu_icons, RGB( 255, 0, 255 ) );

	m_wndMenuBar.SetMenu( &m_DefaultNewMenu);

	CString tmpLab;
	tmpLab.LoadString(IDS_MENU_PANEL);
	m_wndMenuBar.SetWindowText(tmpLab);

	
	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC) ||
		!m_wndToolBar.LoadToolBar(IDR_MAINFRAME))
	{
		TRACE0("Failed to create toolbar\n");
		return -1;      // fail to create
	}
	m_wndToolBar.LoadHiColor(MAKEINTRESOURCE(IDB_TOOLBAR));

	if (!m_wndStatusBar.Create(this) ||
		!m_wndStatusBar.SetIndicators(indicators,
		  sizeof(indicators)/sizeof(UINT)))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}
	// TODO: Delete these three lines if you don't want the toolbar to be dockable
	m_wndToolBar.EnableDocking(CBRS_ALIGN_ANY);
	EnableDocking(CBRS_ALIGN_ANY);
	DockControlBar(&m_wndToolBar);

	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CEGFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	cs.style = WS_OVERLAPPED | WS_CAPTION | FWS_ADDTOTITLE
		 | WS_THICKFRAME | WS_MINIMIZEBOX | WS_SYSMENU;

	cs.style &= ~(WS_THICKFRAME);
	cs.style &= ~(WS_MAXIMIZEBOX);

	cs.style |= WS_BORDER;

	return TRUE;
}


// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CEGFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CEGFrameWnd::Dump(dc);
}

#endif //_DEBUG


// CMainFrame message handlers


BOOL CMainFrame::Create(LPCTSTR lpszClassName, LPCTSTR lpszWindowName, DWORD dwStyle , const RECT& rect , CWnd* pParentWnd , LPCTSTR lpszMenuName , DWORD dwExStyle , CCreateContext* pContext)
{
	dwStyle &= ~(WS_THICKFRAME);
	dwStyle &= ~(WS_MAXIMIZEBOX);

	dwStyle |= WS_BORDER;

	return CEGFrameWnd::Create(lpszClassName, lpszWindowName, dwStyle, rect, pParentWnd, lpszMenuName, dwExStyle, pContext);
}


BOOL CMainFrame::PreTranslateMessage(MSG* pMsg)
{
	try {  //#try
		if (m_wndMenuBar.TranslateFrameMessage(pMsg))
			return TRUE;
	}
	catch(...){
	}
	return CEGFrameWnd::PreTranslateMessage(pMsg);
}
