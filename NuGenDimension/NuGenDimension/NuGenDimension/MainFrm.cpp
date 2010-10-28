// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "NuGenDimension.h"
#include "DocManagerEx.h"

#include "MainFrm.h"

#include "ChildFrm.h"

#include "Dialogs//SetupsDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

static UINT framesToolbarsCurID = START_ID_FOR_FRAMES_TOOLBARS;
// CMainFrame

IMPLEMENT_DYNAMIC(CMainFrame, CEGMDIFrameWnd)
IMPLEMENT_DOCKING(CMainFrame)

BEGIN_MESSAGE_MAP(CMainFrame, CEGMDIFrameWnd)
	ON_WM_CREATE()
	ON_DOCKING_MESSAGES()
	// Global help commands
	ON_COMMAND(ID_HELP_FINDER, CEGMDIFrameWnd::OnHelpFinder)
	ON_COMMAND(ID_HELP, CEGMDIFrameWnd::OnHelp)
	ON_COMMAND(ID_CONTEXT_HELP, CEGMDIFrameWnd::OnContextHelp)
	ON_COMMAND(ID_DEFAULT_HELP, CEGMDIFrameWnd::OnHelpFinder)
	ON_WM_DESTROY()
	ON_MESSAGE_VOID(WM_IDLEUPDATECMDUI, OnIdleUpdateCmdUI)
	ON_NOTIFY_EX_RANGE(TTN_NEEDTEXTW, 0, 0xFFFF, OnToolTipText)
	ON_NOTIFY_EX_RANGE(TTN_NEEDTEXTA, 0, 0xFFFF, OnToolTipText)
	ON_COMMAND(ID_SYSTEM_PANEL, OnSystemPanel)
	ON_UPDATE_COMMAND_UI(ID_SYSTEM_PANEL, OnUpdateSystemPanel)
	ON_COMMAND(ID_SHOW_SCENE_PANEL, OnScenePanel)
	ON_UPDATE_COMMAND_UI(ID_SHOW_SCENE_PANEL, OnUpdateScenePanel)
	ON_COMMAND(ID_SHOW_COMMAND_PANEL, OnCommandPanel)
	ON_UPDATE_COMMAND_UI(ID_SHOW_COMMAND_PANEL, OnUpdateCommandPanel)
	ON_COMMAND(ID_SHOW_SCRIPT_PANEL, OnScriptPanel)
	ON_UPDATE_COMMAND_UI(ID_SHOW_SCRIPT_PANEL, OnUpdateScriptPanel)
	ON_COMMAND(ID_SETUPS, OnSetups)
	ON_MESSAGE( WM_ENTERSIZEMOVE, OnEnterSizeMove) 
	ON_MESSAGE( WM_EXITSIZEMOVE, OnExitSizeMove)
END_MESSAGE_MAP()

static UINT one_indicator[] =
{
	ID_SEPARATOR           // status line indicator
};

static UINT indicators[] =
{
	ID_SEPARATOR,           // status line indicator
		ID_INDICATOR_CAPS,
		ID_INDICATOR_NUM,
		ID_INDICATOR_SCRL,
};


// CMainFrame construction/destruction

CMainFrame::CMainFrame():
	m_exist_child(false)
	, m_PluginsSubMenu(NULL)
	, m_bInsertStringsInGeometryMenu(false)
	, m_last_active_Child(NULL)
	, m_first_raz(true)
{
	m_cursorer = new CCursorer();
	CURSOR_STRUCTURE *c = m_cursorer->GetCursorStructure();
	c->insideColor = theApp.m_reg_manager->m_register_settings.iInColor;
	c->outsideColor= theApp.m_reg_manager->m_register_settings.iOutColor;
	c->isRound     = theApp.m_reg_manager->m_register_settings.bIsCircle;
	c->size        = theApp.m_reg_manager->m_register_settings.iCursorSize;
	m_cursorer->SetCursorStructure(*c);
}

CMainFrame::~CMainFrame()
{
	delete m_cursorer;
	if (m_PluginsSubMenu)
		delete m_PluginsSubMenu;
}

static char  progress_steps_count=0;
static char  progress_cur_step=0;

bool CMainFrame::InitProgresser(unsigned int steps_count)
{
	if (!::IsWindow(m_info_bar.m_hWnd) || steps_count==0)
		return false;

	m_info_bar.SetInfoStyle(INFO_PROGRESS);
	progress_steps_count = steps_count;
	return true;
}

bool CMainFrame::Progress(unsigned int progress_percent)
{
	if (!::IsWindow(m_info_bar.m_hWnd) || progress_steps_count==0)
		return false;

	if (progress_percent==100)
	{
		progress_cur_step++;
		m_info_bar.Progress(progress_cur_step*100/progress_steps_count);
		if (progress_cur_step==progress_steps_count)
		{
			m_info_bar.Progress(100);
			progress_cur_step=0;
			progress_steps_count =0;
			m_info_bar.SetInfoStyle(INFO_TEXT);
		}
	}
	else
	{
		m_info_bar.Progress(progress_cur_step*100/progress_steps_count+
			static_cast<int>(progress_percent/progress_steps_count));
	}
	return true;
}

bool CMainFrame::StopProgresser()
{
	if (!::IsWindow(m_info_bar.m_hWnd) || progress_steps_count==0)
		return false;

	progress_cur_step=0;
	progress_steps_count =0;
	m_info_bar.SetInfoStyle(INFO_TEXT);
	return true;
}

/**/
void CMainFrame::PutMessageFromChildFrame(IApplicationInterface::MESSAGE_TYPE mes_type,
							const char* mes_str)
{
	if (!::IsWindow(m_info_bar.m_hWnd))
		return;

	switch(mes_type) 
	{
	case IApplicationInterface::MT_MESSAGE:
		m_info_bar.SetMessageString(mes_str);
		break;
	case IApplicationInterface::MT_WARNING:
		m_info_bar.SetWarningString(mes_str,NULL);
		break;
	case IApplicationInterface::MT_ERROR:
		m_info_bar.SetErrorString(mes_str,NULL);
		break;
	default:
		break;
	}
}


void CMainFrame::DockControlBarRightOf(CToolBar* Bar, CToolBar* Left)
{
	/*if (!Left)
	{
		DockControlBar(Bar);
		return;
	}

	CRect rect;
	DWORD dw;
	UINT n;

	// get MFC to adjust the dimensions of all docked ToolBars
	// so that GetWindowRect will be accurate
	RecalcLayout(TRUE);

	Left->GetWindowRect(&rect);
	rect.OffsetRect(1,0);
	dw=Left->GetBarStyle();
	n = 0;
	n = (dw&CBRS_ALIGN_TOP) ? AFX_IDW_DOCKBAR_TOP : n;
	n = (dw&CBRS_ALIGN_BOTTOM && n==0) ? AFX_IDW_DOCKBAR_BOTTOM : n;
	n = (dw&CBRS_ALIGN_LEFT && n==0) ? AFX_IDW_DOCKBAR_LEFT : n;
	n = (dw&CBRS_ALIGN_RIGHT && n==0) ? AFX_IDW_DOCKBAR_RIGHT : n;

	// When we take the default parameters on rect, DockControlBar will dock
	// each Toolbar on a seperate line. By calculating a rectangle, we
	// are simulating a Toolbar being dragged to that location and docked.
	DockControlBar(Bar,n,&rect);*/

	if (!Left)
	{
		DockControlBar(Bar);
		return;
	}
	ASSERT(Bar != NULL);
	ASSERT(Bar != Left);
	// the neighbour must be already docked
	CDockBar* pDockBar = Left->m_pDockBar;
	ASSERT(pDockBar != NULL);
	UINT nDockBarID = Left->m_pDockBar->GetDlgCtrlID();
	ASSERT(nDockBarID != AFX_IDW_DOCKBAR_FLOAT);
	bool bHorz = (nDockBarID == AFX_IDW_DOCKBAR_TOP ||
		nDockBarID == AFX_IDW_DOCKBAR_BOTTOM);
	// dock normally (inserts a new row)
	DockControlBar(Bar, nDockBarID);
	// delete the new row (the bar pointer and the row end mark)
	pDockBar->m_arrBars.RemoveAt(pDockBar->m_arrBars.GetSize() - 1);
	pDockBar->m_arrBars.RemoveAt(pDockBar->m_arrBars.GetSize() - 1);
	// find the target bar
	for (int i = 0; i < pDockBar->m_arrBars.GetSize(); i++)
	{
		void* p = pDockBar->m_arrBars[i];
		if (p == Left) // and insert the new bar after it
			pDockBar->m_arrBars.InsertAt(i + 1, Bar);
	}
	// move the new bar into position
	CRect rBar;
	Left->GetWindowRect(rBar);
	rBar.OffsetRect(bHorz ? 1 : 0, bHorz ? 0 : 1);
	Bar->MoveWindow(rBar);
}

/*
CWnd* CMainFrame::GetMessageBar()
{
	return &m_wndStatusBar;
}*/


static WORD menu_icons[] = { IDB_GEOMETRY_TOOLBAR_TC, 
16,16,

ID_FILE_NEW,
ID_FILE_OPEN,

NULL
};


int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CEGMDIFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	/*if (!m_wndStatusBar.Create(this) ||
		!m_wndStatusBar.SetIndicators(indicators,
		  sizeof(indicators)/sizeof(UINT)))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}*/
	
	if (!m_info_bar.Create(this,IDD_INFO_DLG,
		CBRS_TOOLTIPS | CBRS_SIZE_DYNAMIC|CBRS_ALIGN_BOTTOM,1001))
	{
		TRACE0("Failed to create dialogbar\n");
		return -1;      // fail to create
	}
	m_info_bar.EnableDocking(CBRS_ALIGN_BOTTOM);

	if( !m_wndAppTabManager.CreateAppTabManager(this) )
	{
		TRACE0(_T("Failed to create MDI Manager \n"));  
		return FALSE;      // fail to create
	}
	EnableDocking(CBRS_ALIGN_ANY);
	if (!m_wndMenuBar.Create(this,WS_CHILD | WS_VISIBLE | CBRS_ALIGN_TOP|CBRS_SIZE_DYNAMIC|CBRS_TOOLTIPS|CBRS_GRIPPER))
	{
		TRACE0("Failed to create menu bar\n");
		return -1;      // fail to create
	}

	m_wndMenuBar.EnableDocking(CBRS_ALIGN_ANY);
	EnableDocking(CBRS_ALIGN_ANY);
	DockControlBar(&m_wndMenuBar);

	m_DefaultNewMenu.LoadToolBar( menu_icons, RGB( 255, 0, 255 ) );
	m_wndMenuBar.SetMenu(&m_DefaultNewMenu);

	CString tmpLab;
	tmpLab.LoadString(IDS_MENU_PANEL);
	m_wndMenuBar.SetWindowText(tmpLab);
	
	tmpLab.LoadString(IDS_STANDARD_TLB_CAPT);

	/*CreateToolBar(IDR_MAINFRAME,tmpLab, IDB_MAINFRAME_TOOBAR_TC,
			WS_VISIBLE | CBRS_ALIGN_TOP , false,NULL);*/
	CToolBar* prevT = CreateToolBar(IDR_GEOMETRY,tmpLab,  IDB_GEOMETRY_TOOLBAR_TC,
		CBRS_ALIGN_TOP , true,NULL);
	/*tmpLab.LoadString(IDS_PROJECTION_TLB_CAPT);
	prevT = CreateToolBar(IDR_PROJECTION_TOOLBAR,tmpLab, IDB_PROJECTION_TOOLBAR_TC,
		CBRS_ALIGN_TOP,true,prevT);
	tmpLab.LoadString(IDS_NAVIGATE_TLB_CAPT);
	prevT = CreateToolBar(IDR_NAVIGATE_TOOLBAR,tmpLab,IDB_NAVIGATE_TOOLBAR_TC,
		CBRS_ALIGN_TOP,true,prevT);*/
	tmpLab.LoadString(IDS_SNAPS_TLB_CAPT);
	prevT = CreateToolBar(IDR_SNAPS_TOOLBAR,tmpLab,IDB_SNAP_TOOLBAR_TC,
		CBRS_ALIGN_TOP,true,prevT);
	/*tmpLab.LoadString(IDS_MAT_TLB_CAPT);
	prevT = CreateToolBar(IDR_MATERIAL_TOOLBAR,tmpLab,IDB_MAINFRAME_TOOBAR_TC,
		CBRS_ALIGN_TOP,true,prevT);*/

	if (theApp.m_main_pluginer)
	 for (size_t ii=0;ii<theApp.m_main_pluginer->m_toolbar_plugins.size();ii++)
		prevT=AddToolbarPlugin(theApp.m_main_pluginer->m_toolbar_plugins[ii],prevT);

	tmpLab.LoadString(IDS_STANDARD_TLB_CAPT);
	prevT = CreateToolBar(IDR_REPORT_TOOLBAR,tmpLab,  IDB_REPORT_TC,
		CBRS_ALIGN_TOP , false,NULL);
	tmpLab.LoadString(IDS_OBJCTS);
	prevT = CreateToolBar(IDR_REPORT_OBJECTS_TOOLBAR,tmpLab,  IDB_REPORT_OBJECTS,
		CBRS_ALIGN_TOP , false,prevT);
	tmpLab.LoadString(IDS_REPORT_VIEW);
	prevT = CreateToolBar(IDR_REPORT_VIEW,tmpLab,  IDB_REPORT_VIEW,
		CBRS_ALIGN_TOP , false,prevT);
	
	CreateSystemToolbar();

	CDocTemplate* pTemplate = NULL;
	POSITION posit = theApp.m_pDocManager->GetFirstDocTemplatePosition();
	int i=0;
	while (posit)
	{
		pTemplate = theApp.m_pDocManager->GetNextDocTemplate(posit);
		CString srr;
		if (i==0)
			srr = "3D";
		else
			srr = "2D";
		m_wndAppTabManager.AddTab(pTemplate,srr);
		i++;
	}

	EnableDocking(CBRS_ALIGN_ANY);
	/*EnableDocking( CBRS_ALIGN_TOP );
	EnableDocking( CBRS_ALIGN_LEFT );
	EnableDocking( CBRS_ALIGN_BOTTOM );
	EnableDocking( CBRS_ALIGN_RIGHT );*/

	//m_wndPanelsContainer.Create(this, 100);

	//m_script_dlg.Create(CScriptDlg::IDD, &m_wndPanelsContainer);
	//m_scene_tree_dlg.Create(CSceneTreeDlg::IDD,&m_wndPanelsContainer);

	EnablePinning( CBRS_ALIGN_ANY );
	CSize sizeDefault( 200, 200 );
	
	CString tttS;
	tttS.LoadString(IDS_COMMANDER);
	if (!m_commander_docking.CreatePane((TCHAR*)(LPCTSTR)tttS, sizeDefault,  this, NULL, dctLeft) )
	{
		TRACE0("Failed to create Workspace view\n");
		return -1;    // fail to create
	}
	m_commander_docking.SetIcon( IDB_COMMANDER );

	tttS.LoadString(IDS_SCENE);
	if (!m_scene_tree_panel.CreatePane( (TCHAR*)(LPCTSTR)tttS, sizeDefault,  this, NULL, dctRight) )
	{
		TRACE0("Failed to create Workspace view\n");
		return -1;    // fail to create
	}
	m_scene_tree_panel.SetIcon( IDB_SCENE );

	tttS.LoadString(IDS_SCRIPTS);
	 m_script_panel.SetResourceID( IDR_SCRIPT_TLB );
	if (!m_script_panel.CreatePane( (TCHAR*)(LPCTSTR)tttS, sizeDefault,  this, &m_scene_tree_panel, dctTab) )
	{
		TRACE0("Failed to create Workspace view\n");
		return -1;    // fail to create
	}
	m_script_panel.GetToolbar()->LoadHiColor(MAKEINTRESOURCE(IDB_SCRIPT_TLB));
	m_script_panel.SetIcon( IDB_SCRIPT );

	return 0;
}



LRESULT CMainFrame::OnEnterSizeMove(WPARAM,LPARAM)
{
	CMDIChildWnd* pChild = MDIGetActive();

	if (pChild)
	{
		CView* v = pChild->GetActiveView();
		if (v)
			v->PostMessage(WM_ENTERSIZEMOVE);
	}
	return 0L;
}

LRESULT CMainFrame::OnExitSizeMove(WPARAM,LPARAM)
{
	CMDIChildWnd* pChild = MDIGetActive();

	if (pChild)
	{
		CView* v = pChild->GetActiveView();
		if (v)
			v->PostMessage(WM_EXITSIZEMOVE);
	}
	return 0L;
}


void CMainFrame::AddPluginMenu()
{
	if (m_PluginsSubMenu)
		return;
	if (!theApp.m_main_pluginer)
		return;
	if (!theApp.m_main_pluginer->m_toolbar_plugins.empty())
	{
		m_PluginsSubMenu = new  CEGMenu();
		m_PluginsSubMenu->CreatePopupMenu();
		CString plTitle;
		plTitle.LoadString(IDS_PLUGINS_MENU_STRING);
		CWinApp* MyApp=AfxGetApp(); 
		POSITION pos;
		pos=MyApp->GetFirstDocTemplatePosition();
		reinterpret_cast<CEGMultiDocTemplate*>( 
			const_cast<CDocTemplate*>(MyApp->GetNextDocTemplate(pos))
			)->m_NewMenuShared.InsertMenu(5,MF_BYPOSITION|MF_POPUP,
									reinterpret_cast<UINT>(m_PluginsSubMenu->m_hMenu), plTitle);
		size_t sz = theApp.m_main_pluginer->m_toolbar_plugins.size();
		for (size_t i = 0; i < sz; i++)
			m_PluginsSubMenu->AppendMenu(MF_STRING,	theApp.m_main_pluginer->m_toolbar_plugins[i]->m_nID_Menu, 
													theApp.m_main_pluginer->m_toolbar_plugins[i]->m_info.menu_string);
		m_wndMenuBar.RefreshBar();
	}
}

void  CMainFrame::InsertToolbarsStringsInMenu()
{
	size_t sz = m_geometry_toolbars_container.size();
	CString tmpStr;
	if (!m_bInsertStringsInGeometryMenu)
	{
		for (size_t i = 0; i < sz; i++)
			if (m_geometry_toolbars_container[i].menuID>=START_ID_FOR_FRAMES_TOOLBARS &&
				m_geometry_toolbars_container[i].geometry)
			{
				m_geometry_toolbars_container[i].toolbar->GetWindowText(tmpStr);
				CWinApp* MyApp=AfxGetApp(); 
				POSITION pos;
				pos=MyApp->GetFirstDocTemplatePosition();
				reinterpret_cast<CEGMultiDocTemplate*>( 
					const_cast<CDocTemplate*>(MyApp->GetNextDocTemplate(pos))
					)->m_NewMenuShared.GetSubMenu(2)->
					GetSubMenu(0)->AppendMenu(MF_STRING, m_geometry_toolbars_container[i].menuID, 
									tmpStr);
			}
		m_bInsertStringsInGeometryMenu = true;
	}
}

void   CMainFrame::ActivateTab(CDocTemplate* dT)
{
	if (dT && theApp.m_pDocManager)
		((CDocManagerEx*)theApp.m_pDocManager)->ActivateFrame(dT);
}

void CMainFrame::OnDestroy()
{
	int i,j, sz = m_geometry_toolbars_container.size();
	if (theApp.m_main_pluginer)
	{
		int plTsz = theApp.m_main_pluginer->m_toolbar_plugins.size();
		for(i=0;i<sz;i++)
		{
			bool plagT = false;
			for (j=0;j<plTsz;j++)
				if (&theApp.m_main_pluginer->m_toolbar_plugins[j]->m_toolbar==m_geometry_toolbars_container[i].toolbar)
				{
					plagT = true;
					break;
				}
			if (!plagT && m_geometry_toolbars_container[i].toolbar!=&m_system_toolbar)
			{
				m_geometry_toolbars_container[i].toolbar->DestroyWindow();
				delete m_geometry_toolbars_container[i].toolbar;
			}
		}
	}
	m_geometry_toolbars_container.clear();
	CEGMDIFrameWnd::OnDestroy();
}


BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CEGMDIFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	cs.style = WS_OVERLAPPED | WS_CAPTION | FWS_ADDTOTITLE
		 | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_MAXIMIZE | WS_SYSMENU;

	return TRUE;
}


CToolBar* CMainFrame::CreateToolBar(UINT nResID,LPCTSTR t_caption, UINT BitmapID,
									DWORD startSideAndVis, bool geometry,
									CToolBar* leftOf )
{
	// Create a new toolbar object and set the auto delete option
	CEGToolBar* pToolBar = new CEGToolBar;
	
	if ( ! pToolBar->CreateEx(this, TBSTYLE_FLAT, WS_CHILD | startSideAndVis
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC) )
	{
		TRACE("Failed creating toolbar %d\n", nResID);
		return NULL;
	}

	// Load the toolbar
	if ( ! pToolBar->LoadToolBar(nResID) )
	{
		TRACE("Failed loading toolbar %d\n", nResID);
		return NULL;
	}
	pToolBar->LoadHiColor(MAKEINTRESOURCE(BitmapID));
	
	pToolBar->EnableDocking(CBRS_ALIGN_ANY);
	// Dock the toolbar
	DockControlBarRightOf(pToolBar,leftOf);
	// Ok
	pToolBar->SetWindowText(t_caption);

	/*{
		#define BTN_WDTH  16	
		{
			CImageList	cImageList;
			CBitmap		cBitmap;
			BITMAP		bmBitmap;
			CSize		cSize;
			int			nNbBtn;

			if (!cBitmap.Attach(LoadImage(AfxGetResourceHandle(),
									MAKEINTRESOURCE(BitmapID),
									IMAGE_BITMAP, 0, 0,
									LR_DEFAULTSIZE|LR_CREATEDIBSECTION)) ||
									!cBitmap.GetBitmap(&bmBitmap))
			{
				ASSERT(0);
				return NULL;
			}
			cSize  = CSize(bmBitmap.bmWidth, bmBitmap.bmHeight); 
			nNbBtn = cSize.cx/BTN_WDTH;
			RGBTRIPLE* rgb		= (RGBTRIPLE*)(bmBitmap.bmBits);
			COLORREF   rgbMask	= RGB(0,0,0);

			if (!cImageList.Create(BTN_WDTH, cSize.cy,
				ILC_COLOR24|ILC_MASK,
				nNbBtn, 0))
			{
				ASSERT(0);
				return NULL;
			}

			if (cImageList.Add(&cBitmap, rgbMask) == -1)
			{
				ASSERT(0);
				return NULL;
			}

			pToolBar->SendMessage(TB_SETIMAGELIST, 0, (LPARAM)cImageList.m_hImageList);
			cImageList.Detach(); 
			cBitmap.Detach();

		}
	}*/

	TOOLBAR_CONTAINER_ELEMENT  tcEl;
	tcEl.toolbar = pToolBar;
	tcEl.menuID = framesToolbarsCurID++;
	tcEl.visible = TRUE;
	tcEl.geometry = geometry;
	m_geometry_toolbars_container.push_back(tcEl);
	
	return pToolBar;
}

void CMainFrame::CreateSystemToolbar()
{
	if (!m_system_toolbar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC) ||
		!m_system_toolbar.LoadToolBar(IDR_SYSTEM_TOOLBAR))
	{
		TRACE0("Failed to create system toolbar\n");
		ASSERT(0);
		return;   
	}

	m_system_toolbar.LoadHiColor(MAKEINTRESOURCE(IDB_SYSTEM_TC));

	m_system_toolbar.LoadControls();

	m_system_toolbar.EnableDocking(CBRS_ALIGN_TOP);
	// Dock the toolbar
	DockControlBar(&m_system_toolbar);
	// Ok
	CString  tmpLab;
	tmpLab.LoadString(IDS_SYSTEM_TLB_CAPT);
	
	m_system_toolbar.SetWindowText(tmpLab);

	TOOLBAR_CONTAINER_ELEMENT  tcEl;
	tcEl.toolbar = &m_system_toolbar;
	tcEl.menuID = 0;
	tcEl.visible = TRUE;
	tcEl.geometry = true;
	m_geometry_toolbars_container.push_back(tcEl);
}

void CMainFrame::OnSystemPanel()
{
	ShowControlBar(&m_system_toolbar, !m_system_toolbar.IsWindowVisible(), FALSE);
}


void CMainFrame::OnUpdateSystemPanel(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_system_toolbar.IsWindowVisible());
}

void CMainFrame::OnScenePanel()
{
	m_scene_tree_panel.ToggleVisible();
}

void CMainFrame::OnUpdateScenePanel(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(!m_scene_tree_panel.IsHidden());
}

void CMainFrame::OnCommandPanel()
{
	m_commander_docking.ToggleVisible();
}

void CMainFrame::OnUpdateCommandPanel(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(!m_commander_docking.IsHidden());
}

void CMainFrame::OnScriptPanel()
{
	 m_script_panel.ToggleVisible();
}

void CMainFrame::OnUpdateScriptPanel(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(!m_script_panel.IsHidden());
}

void CMainFrame::OnSetups()
{
	CSetupsDlg dlg;
	dlg.DoModal();
}


CToolBar* CMainFrame::AddToolbarPlugin(CToolbarPlugin* pT,CToolBar* leftOf )
{
	static bool firsttlb = true;

	pT->LoadToolbar(this);
	pT->m_toolbar.SetWindowText(pT->m_info.menu_string);

	size_t sz = m_geometry_toolbars_container.size();
	if (sz>0)
	{
		if (firsttlb)
		{
			DockControlBarRightOf(&pT->m_toolbar,leftOf);
			firsttlb = false;
		}
		else
			DockControlBarRightOf(&pT->m_toolbar,m_geometry_toolbars_container[sz-1].toolbar);
		
	}
	
	TOOLBAR_CONTAINER_ELEMENT  tcEl;
	tcEl.toolbar = &pT->m_toolbar;
	tcEl.menuID = 0;
	tcEl.visible = TRUE;
	tcEl.geometry = true;
	m_geometry_toolbars_container.push_back(tcEl);
	return (CToolBar*)&pT->m_toolbar;
}


BOOL CMainFrame::OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo)
{
	if (pHandlerInfo == NULL)
	{
		if ((nID>=START_ID_FOR_FRAMES_TOOLBARS)&&(nID<START_ID_FOR_COMMANDER_CONTEXT_MENU))
		{
			size_t sz = m_geometry_toolbars_container.size();
			for (size_t i = 0; i < sz; i++)
			{
				if (nID == m_geometry_toolbars_container[i].menuID)
				{
					if (nCode == CN_COMMAND)
					{
						ShowControlBar(m_geometry_toolbars_container[i].toolbar, 
							!m_geometry_toolbars_container[i].toolbar->IsWindowVisible(), FALSE);
						SetFocus();
					}
					else if (nCode == CN_UPDATE_COMMAND_UI)
					{
						((CCmdUI *) pExtra)->Enable(TRUE);
						((CCmdUI*)pExtra)->SetCheck(m_geometry_toolbars_container[i].toolbar->IsWindowVisible());
					}
					return TRUE;
				}
			}
		}
		if ((nID>=START_ID_FOR_PLUGINS_MENU)&&(nID<START_ID_FOR_PLUGINS_TOOLBARS))
		{
			if (theApp.m_main_pluginer)
			{
				size_t sz = theApp.m_main_pluginer->m_toolbar_plugins.size();
				for (size_t i = 0; i < sz; i++)
				{
					if (nID == theApp.m_main_pluginer->m_toolbar_plugins[i]->m_nID_Menu)
					{
						if (nCode == CN_COMMAND)
						{
							if(theApp.m_main_pluginer->m_toolbar_plugins[i]->m_was_load)
							{
								ShowControlBar(&theApp.m_main_pluginer->m_toolbar_plugins[i]->m_toolbar, 
										!theApp.m_main_pluginer->m_toolbar_plugins[i]->m_toolbar.IsWindowVisible(), FALSE);
								SetFocus();
							}
						}
						else if (nCode == CN_UPDATE_COMMAND_UI)
						{
							if(theApp.m_main_pluginer->m_toolbar_plugins[i]->m_was_load) 
							{
								((CCmdUI *) pExtra)->Enable(TRUE);
								((CCmdUI*)pExtra)->SetCheck(theApp.m_main_pluginer->m_toolbar_plugins[i]->m_toolbar.IsWindowVisible());
							}
							else
							{
								((CCmdUI *) pExtra)->Enable(FALSE);
								((CCmdUI*)pExtra)->SetCheck(FALSE);
							}
						}
						return TRUE;
					}
				}
			}
		}
	}

	return CEGMDIFrameWnd::OnCmdMsg(nID, nCode, pExtra, pHandlerInfo);
}


void CMainFrame::OnIdleUpdateCmdUI()
{
	int i, sz = m_geometry_toolbars_container.size();

	CMDIChildWnd* pChild = MDIGetActive();

	bool updateTabs = false;
	if (pChild->GetRuntimeClass()!=m_last_active_Child)
	{
		m_last_active_Child = pChild->GetRuntimeClass();
		updateTabs = true;
	}

	if (!updateTabs)
	{
		CEGMDIFrameWnd::OnIdleUpdateCmdUI();
		return;
	}

	if (pChild==NULL)
	{
		ASSERT(0);
		CEGMDIFrameWnd::OnIdleUpdateCmdUI();
		return;
	}

	if (pChild->GetRuntimeClass()==RUNTIME_CLASS(CChildFrame))
	{
		AddPluginMenu();
		InsertToolbarsStringsInMenu();

		if (updateTabs)
		{
			POSITION posit = theApp.m_pDocManager->GetFirstDocTemplatePosition();
			CDocTemplate* pTemplate = theApp.m_pDocManager->GetNextDocTemplate(posit);
			m_wndAppTabManager.ActivateTab(pTemplate);
		}
	
	    if (!m_exist_child)
		{
			for(i=0;i<sz;i++)
				{
					if (m_geometry_toolbars_container[i].geometry)
						ShowControlBar(m_geometry_toolbars_container[i].toolbar, 
							m_geometry_toolbars_container[i].visible, FALSE);
					else
						ShowControlBar(m_geometry_toolbars_container[i].toolbar, 
							FALSE, FALSE);
				}
		    if (m_scene_tree_panel.m_isVisible && m_scene_tree_panel.IsHidden())
				m_scene_tree_panel.ToggleVisible();
			if (m_commander_docking && m_commander_docking.IsHidden())
				m_commander_docking.ToggleVisible();
			if (m_script_panel.m_isVisible && m_script_panel.IsHidden())
				m_script_panel.ToggleVisible();

			m_exist_child = true;
		}
		else
			CEGMDIFrameWnd::OnIdleUpdateCmdUI();
		m_first_raz = false;
	}
	else
	{
		if (updateTabs)
		{

			POSITION posit = theApp.m_pDocManager->GetFirstDocTemplatePosition();
			CDocTemplate* pTemplate = theApp.m_pDocManager->GetNextDocTemplate(posit);
			pTemplate = theApp.m_pDocManager->GetNextDocTemplate(posit);
			m_wndAppTabManager.ActivateTab(pTemplate);
		}

		if (m_exist_child || m_first_raz)
		{
			for(i=0;i<sz;i++)
			{
				if (m_geometry_toolbars_container[i].geometry)
				{
					m_geometry_toolbars_container[i].visible = (m_first_raz)?TRUE:m_geometry_toolbars_container[i].toolbar->IsWindowVisible();
					ShowControlBar(m_geometry_toolbars_container[i].toolbar, FALSE, FALSE);
				}
				else
					ShowControlBar(m_geometry_toolbars_container[i].toolbar, 
								TRUE, FALSE);
			}
			if (m_scene_tree_panel.m_isVisible && !m_scene_tree_panel.IsHidden())
				m_scene_tree_panel.ToggleVisible();
			if (m_commander_docking.m_isVisible && !m_commander_docking.IsHidden())
				m_commander_docking.ToggleVisible();
			if (m_script_panel.m_isVisible && !m_script_panel.IsHidden())
				m_script_panel.ToggleVisible();

			if (!m_first_raz)
				m_exist_child=false;
		}
		else
			CEGMDIFrameWnd::OnIdleUpdateCmdUI();
	}
}








void CMainFrame::GetMessageString(UINT nID, CString& rMessage) const
{
	// load appropriate string
	if (theApp.m_main_pluginer)
	{
		size_t sz = theApp.m_main_pluginer->m_toolbar_plugins.size();
		for (size_t i = 0; i < sz; i++)
		{
			if ((nID>=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_start_ID)&&
				(nID<=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_end_ID))
			{
				rMessage = theApp.m_main_pluginer->m_toolbar_plugins[i]->GetStatusBarMessage(nID);
				return;
			}
		}
	}
	CEGMDIFrameWnd::GetMessageString(nID, rMessage);
}


BOOL CMainFrame::GetToolText( UINT nID, CString& strTipText )
{
	if (theApp.m_main_pluginer)
	{
		size_t sz = theApp.m_main_pluginer->m_toolbar_plugins.size();
		for (size_t i = 0; i < sz; i++)
		{
			if ((nID>=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_start_ID)&&
				(nID<=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_end_ID))
			{
				strTipText = theApp.m_main_pluginer->m_toolbar_plugins[i]->GetTooltipMessage(nID);
				return TRUE;
			}
		}
	}
	return FALSE;
}

//#define _countof(array) (sizeof(array)/sizeof(array[0])) #WARNING

BOOL CMainFrame::OnToolTipText(UINT nID, NMHDR* pNMHDR, LRESULT*pResult)
{
	ASSERT(pNMHDR->code == TTN_NEEDTEXTA || pNMHDR->code == TTN_NEEDTEXTW);

	TOOLTIPTEXTA* pTTTA = (TOOLTIPTEXTA*)pNMHDR;
	TOOLTIPTEXTW* pTTTW = (TOOLTIPTEXTW*)pNMHDR;

	CString strTipText;
	if ( GetToolText( pNMHDR->idFrom, strTipText ) )
	{
#ifndef _UNICODE
		if (pNMHDR->code == TTN_NEEDTEXTA)
			lstrcpyn(pTTTA->szText, strTipText, _countof(pTTTA->szText));
		else
			_mbstowcsz(pTTTW->szText, strTipText, _countof(pTTTW->szText));
#else
		if (pNMHDR->code == TTN_NEEDTEXTA)
			_wcstombsz(pTTTA->szText, strTipText, _countof(pTTTA->szText));
		else
			lstrcpyn(pTTTW->szText, strTipText, _countof(pTTTW->szText));
#endif
		return TRUE;
	}

	return CEGMDIFrameWnd::OnToolTipText( nID, pNMHDR, pResult );
}


BOOL CMainFrame::PreTranslateMessage(MSG* pMsg)
{
	try { //#try
		if (m_wndMenuBar.TranslateFrameMessage(pMsg))
			return TRUE;
	}
	catch(...){
	}

	return CEGMDIFrameWnd::PreTranslateMessage(pMsg);
}
