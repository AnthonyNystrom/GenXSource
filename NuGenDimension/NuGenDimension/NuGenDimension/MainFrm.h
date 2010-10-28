// MainFrm.h : interface of the CMainFrame class
//


#pragma once

#include "Controls//SystemToolbar.h"
#include "AppTabManager.h"

#include "Tools//Cursorer.h"

#include "Dialogs//InfoBar.h"

#include "ChildFrm.h"


#include "Dialogs//CommandPanelDlg.h"

#include "EGDocking.h"
#include "WinAddon.h"


class CMainFrame;

#include "Scripting//ScriptPane.h"

typedef struct
{
	CEGToolBar* 			toolbar;
	UINT                    menuID;
	BOOL           			visible;
	bool                    geometry;
} TOOLBAR_CONTAINER_ELEMENT;

class CDockingSceneTreePane : public CEGDockingControlPane
{
	friend class CMainFrame;
protected:
	CMyBirchCtrl m_wndSceneTree;
	bool         m_isVisible;


	virtual HWND CreateControl() 
	{
		m_wndSceneTree.CreateEx(WS_EX_CLIENTEDGE,NULL,NULL,WS_CHILD|WS_VISIBLE, CRect( 0, 0, 0, 0),this,1001);

		m_wndSceneTree.SetBirch(theApp.m_main_tree_control);

		if (theApp.m_main_tree_control)
			theApp.m_main_tree_control->SetWnd(&m_wndSceneTree);

		m_wndSceneTree
			.SetTextFont( 8, FALSE, FALSE, "MS Shell Dlg" )
			.SetDefaultTextColor( RGB(0,64,128) );
	
		m_isVisible = true;
		return m_wndSceneTree.GetSafeHwnd();
	}
};

class CDockingCommanderPane : public CEGDockingControlPane
{
	friend class CMainFrame;
protected:
	CCommandPanelDlg  m_command_panel;
	bool         m_isVisible;

	virtual HWND CreateControl() 
	{
		m_command_panel.Create(CCommandPanelDlg::IDD, this);
		m_isVisible = true;
		return m_command_panel.GetSafeHwnd();
	}
};

class CMainFrame : public CEGMDIFrameWnd,	public IProgresser
{
	DECLARE_DYNAMIC(CMainFrame)
	DECLARE_DOCKING()
private:	
	friend class CChildFrame;

public:
	CMainFrame();

// Attributes
public:

// Operations
public:
	virtual bool InitProgresser(unsigned int steps_count);
	virtual bool Progress(unsigned int progress_percent);
	virtual bool StopProgresser();

	void PutMessageFromChildFrame(IApplicationInterface::MESSAGE_TYPE mes_type,
		const char* mes_str);

// Overrides
public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	//virtual CWnd* GetMessageBar();

// Implementation
public:
	virtual ~CMainFrame();
	
	virtual void GetMessageString(UINT nID,CString& rMessage) const;
	BOOL         GetToolText( UINT nID, CString& strTipText );

private:
	CEGMenu*	m_PluginsSubMenu;
	void        AddPluginMenu();

	bool        m_bInsertStringsInGeometryMenu;
	void        InsertToolbarsStringsInMenu();

	bool        m_exist_child;

	CSystemToolbar   m_system_toolbar;


	CInfoBar      m_info_bar;


	void             CreateSystemToolbar();

	CRuntimeClass*   m_last_active_Child;

	bool             m_first_raz;

	CDockingSceneTreePane m_scene_tree_panel;
	CDockingCommanderPane m_commander_docking;
	CDockingScriptPane    m_script_panel;


public:
	CCursorer*    m_cursorer;

	CCursorer*    GetCursorer() {
		return m_cursorer;};

	ICommandPanel*  GetCommandPanel()
	   {return &(m_commander_docking.m_command_panel);};
protected:  // control bar embedded members
	//CEGStatusBar  m_wndStatusBar;

	CEGMenuBar    m_wndMenuBar;
	CAppTabManager m_wndAppTabManager;

	//CNoResizeableStatusbar  m_core_info_status_Bar;
	
	void	  DockControlBarRightOf(CToolBar* Bar, CToolBar* Left);
	CToolBar* CreateToolBar(UINT nResID,LPCTSTR t_caption, UINT BitmapID,
		DWORD startSideAndVis,bool geometry, CToolBar* leftOf);

	std::vector<TOOLBAR_CONTAINER_ELEMENT>   m_geometry_toolbars_container;
	
	CToolBar* CMainFrame::AddToolbarPlugin(CToolbarPlugin* pT,CToolBar* leftOf );

// Generated message map functions
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnIdleUpdateCmdUI();
	DECLARE_MESSAGE_MAP()
public:
	void    SetView(CNuGenDimensionView* v)
	{
		ASSERT(v);
		m_system_toolbar.SetView(v);
	}
	void    UpdateSystemToolbar()
	{
		m_system_toolbar.UpdateSystemToolbar();
	}
	void    ResetNames()
	{
		if (theApp.m_main_pluginer)
		{
			size_t sz = theApp.m_main_pluginer->m_toolbar_plugins.size();
			for (size_t i = 0; i < sz; i++)
			{
				theApp.m_main_pluginer->m_toolbar_plugins[i]->ResetNames();
			}
		}
		group_name_index = 1;
	}

	void    ActivateTab(CDocTemplate* dT);

	afx_msg void OnDestroy();
	afx_msg BOOL OnToolTipText(UINT nID, NMHDR* pNMHDR, LRESULT*pResult);
	virtual BOOL OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo);
	afx_msg void OnSystemPanel();
	afx_msg void OnUpdateSystemPanel(CCmdUI *pCmdUI);
	afx_msg void OnScenePanel();
	afx_msg void OnUpdateScenePanel(CCmdUI *pCmdUI);
	afx_msg void OnCommandPanel();
	afx_msg void OnUpdateCommandPanel(CCmdUI *pCmdUI);
	afx_msg void OnScriptPanel();
	afx_msg void OnUpdateScriptPanel(CCmdUI *pCmdUI);
	afx_msg void OnSetups();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	afx_msg LRESULT OnEnterSizeMove (WPARAM, LPARAM); 
	afx_msg LRESULT OnExitSizeMove (WPARAM, LPARAM);
};

