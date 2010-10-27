//-----------------------------------------------------------------------------
// MainFrm.cpp : Implementation of the CMainFrame class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "GLStereoPlayer.h"

#include "MainFrm.h"
#include "GLStereoPlayerDoc.h"
#include "GLStereoPlayerView.h"

#include "XMLUtils.h"

#define DEFAULT_OPTIONS_FILENAME "options.xml"

using namespace glsp;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMainFrame

IMPLEMENT_DYNCREATE(CMainFrame, CFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
    //{{AFX_MSG_MAP(CMainFrame)
    ON_WM_CREATE()
	ON_WM_DESTROY()
    ON_COMMAND(ID_VIEW_RESIZE_HALF, OnViewResizeHalf)
    ON_COMMAND(ID_VIEW_RESIZE_ORIGINAL, OnViewResizeOriginal)
    ON_COMMAND(ID_VIEW_RESIZE_DOUBLE, OnViewResizeDouble)
    ON_COMMAND(ID_VIEW_FULLSCREEN, OnViewFullScreen)
    ON_COMMAND(ID_VIEW_TOOLBAR, OnViewToolBar)
    ON_COMMAND(ID_VIEW_STATUS_BAR, OnViewStatusBar)
    ON_UPDATE_COMMAND_UI(ID_VIEW_FULLSCREEN, OnUpdateViewFullScreen)
	ON_COMMAND(ID_FILE_SAVEMODIFIED, OnFileSaveModified)
	ON_UPDATE_COMMAND_UI(ID_FILE_SAVEMODIFIED, OnUpdateFileSaveModified)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

static UINT indicators[] =
{
    ID_SEPARATOR,           // status line indicator
    ID_INDICATOR_CAPS,
    ID_INDICATOR_NUM,
    ID_INDICATOR_SCRL,
};

/////////////////////////////////////////////////////////////////////////////
// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
    m_hMenu = NULL;
    m_bToolBar = TRUE;
    m_bStatusBar = TRUE;
    m_bFullScreen = FALSE;
}

CMainFrame::~CMainFrame()
{
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
    if (CFrameWnd::OnCreate(lpCreateStruct) == -1)
        return -1;

	LoadOptions();

    DWORD dwTBStyle = TBSTYLE_FLAT;
    DWORD dwStyle = WS_CHILD | CBRS_ALIGN_TOP | CBRS_TOOLTIPS | CBRS_FLYBY;
    if( !m_wndToolBar.CreateEx(this, dwTBStyle, dwStyle ) ) {
        TRACE0("Failed to create toolbar\n");
        return FALSE;    // fail to create
    }
    m_wndToolBar.GetToolBarCtrl().SetExtendedStyle(TBSTYLE_EX_DRAWDDARROWS); 

    // Make a full colored tool bar by myself...
    CImageList img;
    HINSTANCE hInst = AfxFindResourceHandle((LPCTSTR)IDB_TOOLBAR256, RT_BITMAP);
    img.Attach(ImageList_LoadImage(hInst, (LPCTSTR)IDB_TOOLBAR256, 16, 0, RGB(192, 192, 192), IMAGE_BITMAP, LR_CREATEDIBSECTION));
    m_wndToolBar.GetToolBarCtrl().SetImageList(&img);
    img.Detach();

    m_wndToolBar.SetSizes(CSize(16+7, 15+6), CSize(16, 15));
    m_wndToolBar.SetButtons(NULL, 12);
    m_wndToolBar.SetButtonInfo(0, ID_FILE_OPENLEFT, TBSTYLE_BUTTON, 0);
    m_wndToolBar.SetButtonInfo(1, ID_FILE_OPENRIGHT, TBSTYLE_BUTTON, 1);
    m_wndToolBar.SetButtonInfo(2, ID_FILE_OPENSETTING, TBSTYLE_BUTTON, 2);
    m_wndToolBar.SetButtonInfo(3, ID_FILE_SAVESETTING, TBSTYLE_BUTTON, 3);
    m_wndToolBar.SetButtonInfo(4, 0, TBBS_SEPARATOR, 0);
    m_wndToolBar.SetButtonInfo(5, ID_VIEW_CHANGE_FORMAT, TBSTYLE_BUTTON | TBSTYLE_DROPDOWN, 4);
    m_wndToolBar.SetButtonInfo(6, ID_VIEW_CHANGE_STEREOTYPE, TBSTYLE_BUTTON | TBSTYLE_DROPDOWN, 5);
    m_wndToolBar.SetButtonInfo(7, ID_VIEW_SWAP, TBSTYLE_BUTTON, 6);
    m_wndToolBar.SetButtonInfo(8, ID_VIEW_STATISTICS, TBSTYLE_BUTTON, 7);
    m_wndToolBar.SetButtonInfo(9, ID_VIEW_PLAYCONTROL, TBSTYLE_BUTTON | TBSTYLE_DROPDOWN, 8);
    m_wndToolBar.SetButtonInfo(10, 0, TBBS_SEPARATOR, 0);
    m_wndToolBar.SetButtonInfo(11, ID_APP_ABOUT, TBSTYLE_BUTTON, 9);

	dwStyle = WS_CHILD | WS_CLIPSIBLINGS | WS_CLIPCHILDREN | CBRS_TOP;
    if (!m_wndReBar.Create(this) ||
        !m_wndReBar.AddBar(&m_wndToolBar))
    {
        TRACE0("Failed to create rebar\n");
        return -1;
    }

	dwStyle = WS_CHILD | CBRS_BOTTOM;
	if (m_bStatusBar) dwStyle |= WS_VISIBLE;
    if (!m_wndStatusBar.Create(this, dwStyle) ||
        !m_wndStatusBar.SetIndicators(indicators,
          sizeof(indicators)/sizeof(UINT)))
    {
        TRACE0("Failed to create status bar\n");
        return -1;
    }

    m_wndToolBar.SetBarStyle(m_wndToolBar.GetBarStyle() |
        CBRS_TOOLTIPS | CBRS_FLYBY);

	ShowControlBar(&m_wndToolBar, m_bToolBar, TRUE);

    return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
    if( !CFrameWnd::PreCreateWindow(cs) )
        return FALSE;

    // Default window size
    cs.x  = 100;
    cs.y  = 100;
    cs.cx = 640;
    cs.cy = 480;

    return TRUE;
}

void CMainFrame::OnDestroy() 
{
	SaveOptions();

	CFrameWnd::OnDestroy();
}

/////////////////////////////////////////////////////////////////////////////
// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
    CFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
    CFrameWnd::Dump(dc);
}

#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMainFrame message handlers

void CMainFrame::UpdateSize(float scale)
{
    // Resize the window to fit the source

    if (m_bFullScreen) return;

	StereoPlayer* stereoPlayer = ((CGLStereoPlayerApp*)AfxGetApp())->GetPlayer();
    unsigned int newWidth = (unsigned int)(stereoPlayer->getPlayerWidth() * scale);
    unsigned int newHeight = (unsigned int)(stereoPlayer->getPlayerHeight() * scale);
    if (!m_bFullScreen && newWidth>0 && newHeight>0)
    {
        // Calculate the proper client area size
        CRect rect;
        GetClientRect(rect);
        rect.right = rect.left + newWidth + GetSystemMetrics(SM_CXEDGE)*2;
        rect.bottom = rect.top + newHeight + (m_hMenu?GetSystemMetrics(SM_CYMENU):0) +
            (m_bToolBar?24:0) + GetSystemMetrics(SM_CYEDGE)*2 + (m_bStatusBar?19:0);
        CalcWindowRect(&rect);
        SetWindowPos(NULL, -1, -1, rect.right-rect.left, rect.bottom-rect.top, SWP_NOMOVE | SWP_NOZORDER);
    }
}

void CMainFrame::SetFullScreen(BOOL fullScreen)
{
    m_bFullScreen = fullScreen;

    if (m_bFullScreen)
    {
        // Store the current menu/tool/status bar status
        m_bToolBar = m_wndToolBar.IsWindowVisible();
        ShowControlBar(&m_wndToolBar, FALSE, TRUE);
        m_bStatusBar = m_wndStatusBar.IsWindowVisible();
        ShowControlBar(&m_wndStatusBar, FALSE, TRUE);

        ModifyStyle(WS_CAPTION | WS_THICKFRAME, 0);
        GetActiveView()->ModifyStyleEx(WS_EX_CLIENTEDGE, 0);
        m_hMenu = ::GetMenu(GetSafeHwnd());
        SetMenu(NULL);
        ShowWindow(SW_MAXIMIZE);
    }
    else
    {
        // Restore the previous status
        m_bFullScreen = FALSE;
        ModifyStyle(0, WS_CAPTION | WS_THICKFRAME);
        GetActiveView()->ModifyStyleEx(0, WS_EX_CLIENTEDGE);
        ShowControlBar(&m_wndToolBar, m_bToolBar, TRUE);
        ShowControlBar(&m_wndStatusBar, m_bStatusBar, TRUE);
        ::SetMenu(GetSafeHwnd(), m_hMenu);
        ShowWindow(SW_RESTORE);
    }
}

void CMainFrame::OnViewResizeHalf() 
{
    UpdateSize(0.5f);
}

void CMainFrame::OnViewResizeOriginal() 
{
    UpdateSize(1.0f);
}

void CMainFrame::OnViewResizeDouble() 
{
    UpdateSize(2.0f);
}

void CMainFrame::OnViewFullScreen() 
{
    SetFullScreen(!m_bFullScreen);
}

void CMainFrame::OnUpdateViewFullScreen(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_bFullScreen);
}

void CMainFrame::OnViewToolBar() 
{
    m_bToolBar = !m_wndToolBar.IsWindowVisible();
    ShowControlBar(&m_wndToolBar, m_bToolBar, TRUE);
}

void CMainFrame::OnViewStatusBar() 
{
    m_bStatusBar = !m_wndStatusBar.IsWindowVisible();
    ShowControlBar(&m_wndStatusBar, m_bStatusBar, TRUE);
}

void CMainFrame::OnFileSaveModified() 
{
	m_bSaveModified = !m_bSaveModified;
}

void CMainFrame::OnUpdateFileSaveModified(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_bSaveModified);
}

#define lpnm   ((LPNMHDR)lParam)
#define lpnmTB ((LPNMTOOLBAR)lParam)

BOOL CMainFrame::OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult) 
{
	switch(lpnm->code)
	{
	  // Popup menues in the tool bar
	  case TBN_DROPDOWN:
		{
			CRect rect;
			m_wndToolBar.GetToolBarCtrl().GetRect(lpnmTB->iItem, &rect);
			rect.top = rect.bottom;
			::ClientToScreen(lpnmTB->hdr.hwndFrom, &rect.TopLeft());
			CMenu menu;
			menu.LoadMenu(IDR_MAINFRAME);
			if (lpnmTB->iItem == ID_VIEW_CHANGE_FORMAT) {
				CMenu* pPopup = menu.GetSubMenu(1)->GetSubMenu(5);
				pPopup->TrackPopupMenu(TPM_LEFTALIGN | TPM_RIGHTBUTTON, rect.left, rect.top+1, this);
				return FALSE;
			}
			else if (lpnmTB->iItem == ID_VIEW_CHANGE_STEREOTYPE) {
				CMenu* pPopup = menu.GetSubMenu(1)->GetSubMenu(6);
				pPopup->TrackPopupMenu(TPM_LEFTALIGN | TPM_RIGHTBUTTON, rect.left, rect.top+1, this);
				return FALSE;
			}
			else if (lpnmTB->iItem == ID_VIEW_PLAYCONTROL) {
				CMenu* pPopup = menu.GetSubMenu(1)->GetSubMenu(2);
				pPopup->TrackPopupMenu(TPM_LEFTALIGN | TPM_RIGHTBUTTON, rect.left, rect.top+1, this);
				return FALSE;
			}
		}
	}

    return CFrameWnd::OnNotify(wParam, lParam, pResult);
}

BOOL CMainFrame::LoadOptions()
{
	try
	{
		StereoPlayer* stereoPlayer = ((CGLStereoPlayerApp*)AfxGetApp())->GetPlayer();

		// Options setting file  is in the application directory
		char path[_MAX_PATH], drive[_MAX_DRIVE], dir[_MAX_DIR];
		char fname[_MAX_FNAME], ext[_MAX_EXT];
		char filename[_MAX_PATH];

		GetModuleFileName(NULL, path, _MAX_PATH);
		_splitpath(path, drive, dir, fname, ext);
		StringCchPrintf(filename, _MAX_PATH, "%s%s%s", drive, dir, DEFAULT_OPTIONS_FILENAME);

		IXMLDOMDocumentPtr doc("MSXML.DOMDocument");
		doc->validateOnParse = VARIANT_FALSE;
		doc->load(LPCTSTR(filename));

		IXMLDOMNodePtr root = doc->documentElement;
		IXMLDOMNodePtr node;

		// Root node's name must be 'glsp_opts'
		if (root->nodeName != _bstr_t("glsp_opts"))
			return FALSE;

		// Parse each option element
		node = root->firstChild;
		CString value;
		float r, g, b;
		CString fontName;
		int fontSize;
		while (node)
		{
			if (node->nodeName == _bstr_t("fullScreen"))
			{
				m_bFullScreen = atoi(loadElement(node))?TRUE:FALSE;
			}
			else if (node->nodeName == _bstr_t("toolBar"))
			{
				m_bToolBar = atoi(loadElement(node))?TRUE:FALSE;
			}
			else if (node->nodeName == _bstr_t("statusBar"))
			{
				m_bStatusBar = atoi(loadElement(node))?TRUE:FALSE;
			}
			else if (node->nodeName == _bstr_t("saveModified"))
			{
				m_bSaveModified = atoi(loadElement(node))?TRUE:FALSE;
			}
			else if (node->nodeName == _bstr_t("leftColor"))
			{
				loadColorElement(node, r, g, b);
				stereoPlayer->setAnagryphColor(TEXTURE_LEFT, r, g, b);
			}
			else if (node->nodeName == _bstr_t("rightColor"))
			{
				loadColorElement(node, r, g, b);
				stereoPlayer->setAnagryphColor(TEXTURE_RIGHT, r, g, b);
			}
			else if (node->nodeName == _bstr_t("statistics"))
			{
				stereoPlayer->setStatistics((BOOL)atoi(loadElement(node)));
			}
			else if (node->nodeName == _bstr_t("playControl"))
			{
				value = loadElement(node);
				if (value == "hide")
					stereoPlayer->setPlayControl(PLAYCONTROL_HIDE);
				else if (value == "auto")
					stereoPlayer->setPlayControl(PLAYCONTROL_AUTO);
				else if (value == "show")
					stereoPlayer->setPlayControl(PLAYCONTROL_SHOW);
			}
			else if (node->nodeName == _bstr_t("statisticsFont"))
			{
				loadFontElement(node, fontName, fontSize);
				stereoPlayer->setStatisticsFont(fontName, fontSize);
			}
			else if (node->nodeName == _bstr_t("statisticsColor"))
			{
				loadColorElement(node, r, g, b);
				stereoPlayer->setStatisticsColor(r, g, b);
			}
			else if (node->nodeName == _bstr_t("baseColor"))
			{
				loadColorElement(node, r, g, b);
				stereoPlayer->setBaseColor(r, g, b);
			}
			else if (node->nodeName == _bstr_t("forceSync"))
			{
				stereoPlayer->setForceSync((BOOL)atoi(loadElement(node)));
			}
			else if (node->nodeName == _bstr_t("loop"))
			{
				stereoPlayer->setLoop((BOOL)atoi(loadElement(node)));
			}
			else if (node->nodeName == _bstr_t("keepAspectRatio"))
			{
				stereoPlayer->setKeepAspectRatio((BOOL)atoi(loadElement(node)));
			}
			else if (node->nodeName == _bstr_t("transition"))
			{
				stereoPlayer->setTransition((BOOL)atoi(loadElement(node)));
			}
			else if (node->nodeName == _bstr_t("playOnLoad"))
			{
				stereoPlayer->setPlayOnLoad((BOOL)atoi(loadElement(node)));
			}

			node = node->nextSibling;
		}

		return TRUE;
	}
	catch(...) {}
	return FALSE;
}

BOOL CMainFrame::SaveOptions()
{
	try
	{
		StereoPlayer* stereoPlayer = ((CGLStereoPlayerApp*)AfxGetApp())->GetPlayer();

		// Options setting file  is in the application directory
		char path[_MAX_PATH], drive[_MAX_DRIVE], dir[_MAX_DIR];
		char fname[_MAX_FNAME], ext[_MAX_EXT];
		char filename[_MAX_PATH];
		float r, g, b;

		GetModuleFileName(NULL, path, _MAX_PATH);
		_splitpath(path, drive, dir, fname, ext);
		StringCchPrintf(filename, _MAX_PATH, "%s%s%s", drive, dir, DEFAULT_OPTIONS_FILENAME);

		IXMLDOMDocumentPtr doc("MSXML.DOMDocument");
		doc->appendChild(doc->createProcessingInstruction("xml", "version='1.0'"));

		IXMLDOMNodePtr root = doc->createElement("glsp_opts");
		doc->appendChild(root);

		saveAttribute(root, "version", stereoPlayer->getVersion());

		saveElement(root, "fullScreen", m_bFullScreen);
		saveElement(root, "toolBar", m_bToolBar);
		saveElement(root, "statusBar", m_bStatusBar);
		saveElement(root, "saveModified", m_bSaveModified);

		stereoPlayer->getAnagryphColor(TEXTURE_LEFT, &r, &g, &b);
		saveColorElement(root, "leftColor", r, g, b);
		stereoPlayer->getAnagryphColor(TEXTURE_RIGHT, &r, &g, &b);
		saveColorElement(root, "rightColor", r, g, b);
		saveElement(root, "statistics", stereoPlayer->getStatistics()?1:0);
		switch (stereoPlayer->getPlayControl())
		{
			case PLAYCONTROL_HIDE:
				saveElement(root, "playControl", "hide"); break;
			case PLAYCONTROL_AUTO:
				saveElement(root, "playControl", "auto"); break;
			case PLAYCONTROL_SHOW:
				saveElement(root, "playControl", "show"); break;
		}
		saveFontElement(root, "statisticsFont", stereoPlayer->getStatisticsFontName(), stereoPlayer->getStatisticsFontSize());
		stereoPlayer->getStatisticsColor(&r, &g, &b);
		saveColorElement(root, "statisticsColor", r, g, b);
		stereoPlayer->getBaseColor(&r, &g, &b);
		saveColorElement(root, "baseColor", r, g, b);
		saveElement(root, "forceSync", stereoPlayer->getForceSync()?1:0);
		saveElement(root, "loop", stereoPlayer->getLoop()?1:0);
		saveElement(root, "keepAspectRatio", stereoPlayer->getKeepAspectRatio()?1:0);
		saveElement(root, "transition", stereoPlayer->getTransition()?1:0);
		saveElement(root, "playOnLoad", stereoPlayer->getPlayOnLoad()?1:0);

		doc->save(LPCTSTR(filename));

		return TRUE;
	}
	catch(...) {}
	return FALSE;
}
