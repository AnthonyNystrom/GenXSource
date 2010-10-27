//-----------------------------------------------------------------------------
// GLStereoPlayer.cpp : Defines the class behaviors for the application.
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "GLStereoPlayer.h"

#include "MainFrm.h"
#include "GLStereoPlayerDoc.h"
#include "GLStereoPlayerView.h"

using namespace glsp;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp

BEGIN_MESSAGE_MAP(CGLStereoPlayerApp, CWinApp)
    //{{AFX_MSG_MAP(CGLStereoPlayerApp)
    ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
    //}}AFX_MSG_MAP
    // Standard file based document commands
    ON_COMMAND(ID_FILE_NEW, CWinApp::OnFileNew)
    ON_COMMAND(ID_FILE_OPEN, CWinApp::OnFileOpen)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp construction

CGLStereoPlayerApp::CGLStereoPlayerApp()
{
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CGLStereoPlayerApp object

CGLStereoPlayerApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp initialization

BOOL CGLStereoPlayerApp::InitInstance()
{
    // Standard initialization
    // If you are not using these features and wish to reduce the size
    //  of your final executable, you should remove from the following
    //  the specific initialization routines you do not need.

#ifdef _AFXDLL
    Enable3dControls();     // Call this when using MFC in a shared DLL
#else
    Enable3dControlsStatic();   // Call this when linking to MFC statically
#endif

    // Change the registry key under which our settings are stored.
    // You should modify this string to be something appropriate
    // such as the name of your company or organization.
    SetRegistryKey(_T("GLStereoPlayer"));

    LoadStdProfileSettings();  // Load standard INI file options (including MRU)

    // Register the application's document templates.  Document templates
    //  serve as the connection between documents, frame windows and views.

    CSingleDocTemplate* pDocTemplate;
    pDocTemplate = new CSingleDocTemplate(
        IDR_MAINFRAME,
        RUNTIME_CLASS(CGLStereoPlayerDoc),
        RUNTIME_CLASS(CMainFrame),       // main SDI frame window
        RUNTIME_CLASS(CGLStereoPlayerView));
    AddDocTemplate(pDocTemplate);

    // Enable DDE Execute open
    EnableShellOpen();
    RegisterShellFileTypes(TRUE);

	m_nCmdShow = FALSE;

    m_stereoPlayer = new StereoPlayer;
    m_slideShow = new SlideShow;
    m_slideShow->setPlayer(m_stereoPlayer);

    char path[_MAX_PATH], drive[_MAX_DRIVE], dir[_MAX_DIR];
    char fname[_MAX_FNAME], ext[_MAX_EXT];
	char homeDir[_MAX_PATH], dllPath[_MAX_PATH];
    GetModuleFileName(NULL, path, _MAX_PATH);
    _splitpath(path, drive, dir, fname, ext);

    // Set the application path as a default home directory
    StringCchPrintf(homeDir, _MAX_PATH, "%s%s", drive, dir);
    m_stereoPlayer->setHomeDir(homeDir);
    
	// Parse command line for standard shell commands, DDE, file open
    CCommandLineInfo cmdInfo;
    ParseCommandLine(cmdInfo);

    // Dispatch commands specified on the command line
    if (!ProcessShellCommand(cmdInfo))
        return FALSE;

    ((CMainFrame*)m_pMainWnd)->UpdateSize();
    if (!((CMainFrame*)m_pMainWnd)->m_bFullScreen)
		m_pMainWnd->ShowWindow(SW_SHOW);
	else
	    ((CMainFrame*)m_pMainWnd)->SetFullScreen(TRUE);
    m_pMainWnd->UpdateWindow();

    // Load a localized resource DLL
    StringCchPrintf(dllPath, _MAX_PATH, "%s%s%s", drive, dir, "localize.dll");
    m_hLangDLL = LoadLibrary(dllPath);
    if (m_hLangDLL)
        AfxSetResourceHandle(m_hLangDLL);
    // Update Menu resource?
    //  ::SetMenu(AfxGetMainWnd()->GetSafeHwnd(), ::LoadMenu( m_hLangDLL, MAKEINTRESOURCE(IDR_MAINFRAME)));

    // Enable drag/drop open
    m_pMainWnd->DragAcceptFiles();

    return TRUE;
}

int CGLStereoPlayerApp::ExitInstance() 
{
    if (m_hLangDLL)
        FreeLibrary(m_hLangDLL);

    delete m_stereoPlayer;
    delete m_slideShow;
    
    return CWinApp::ExitInstance();
}

/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
    CAboutDlg();

// Dialog Data
    //{{AFX_DATA(CAboutDlg)
    enum { IDD = IDD_ABOUTBOX };
    //}}AFX_DATA

    // ClassWizard generated virtual function overrides
    //{{AFX_VIRTUAL(CAboutDlg)
    protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
    //}}AFX_VIRTUAL

// Implementation
protected:
    //{{AFX_MSG(CAboutDlg)
        // No message handlers
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
    //{{AFX_DATA_INIT(CAboutDlg)
    //}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CAboutDlg)
    //}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
    //{{AFX_MSG_MAP(CAboutDlg)
        // No message handlers
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

// App command to run the dialog
void CGLStereoPlayerApp::OnAppAbout()
{
    CAboutDlg aboutDlg;
    aboutDlg.DoModal();
}

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp message handlers
