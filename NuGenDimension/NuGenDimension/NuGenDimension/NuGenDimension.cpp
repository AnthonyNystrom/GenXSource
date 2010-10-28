// NuGenDimension.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include <shlwapi.h>

#include "NuGenDimension.h"
#include "MainFrm.h"

#include "DocManagerEx.h"
#include "OneDocTemplate.h"

#include "ChildFrm.h"
#include "NuGenDimension.h"
#include "NuGenDimensionView.h"

#include "ReportChildFrame.h"
#include "ReportCreatorDoc.h"
#include "ReportCreatorView.h"
#include "FileVersionInfo.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// CAboutDlg dialog used for App About



// CNuGenDimensionApp

BEGIN_MESSAGE_MAP(CNuGenDimensionApp, CWinApp)
  ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
  // Standard file based document commands
  ON_COMMAND(ID_FILE_NEW, CWinApp::OnFileNew)
  ON_COMMAND(ID_FILE_OPEN, CWinApp::OnFileOpen)
  ON_COMMAND(ID_FILE_PRINT_SETUP, CWinApp::OnFilePrintSetup)
  ON_COMMAND(ID_REPORT_FILE_NEW, CWinApp::OnFileNew)
  ON_COMMAND(ID_REPORT_FILE_OPEN, CWinApp::OnFileOpen)
  // Standard print setup command
END_MESSAGE_MAP()


// CNuGenDimensionApp construction

CNuGenDimensionApp::CNuGenDimensionApp()
{
  EnableHtmlHelp();
  //m_main_tree_control = new CGlobalTree;
  m_main_pluginer = new CPluginLoader;
  m_reg_manager   = NULL;
}

CNuGenDimensionApp::~CNuGenDimensionApp()
{
  if (m_main_pluginer)
    delete m_main_pluginer;
  if (m_main_tree_control)
    delete m_main_tree_control;
  if (m_reg_manager)
	  delete m_reg_manager;
}


// The one and only CNuGenDimensionApp object

CNuGenDimensionApp theApp;

bool translate_messages_through_app = false;


static UINT menuIDs[] =
{
    1, // STUB
    ID_FILE_OPEN,

    NULL
};

BOOL CNuGenDimensionApp::InitInstance()
{/*
  typedef void(*LPDLLFUNC_START_SPLASH)();
  LPDLLFUNC_START_SPLASH lpfnDllFuncStartSplash = NULL;
  typedef void(*LPDLLFUNC_END_SPLASH)(HWND);
  LPDLLFUNC_END_SPLASH lpfnDllFuncEndSplash = NULL;
  HINSTANCE hDLL = NULL;
  hDLL = AfxLoadLibrary("SplashLib.dll");
  if(hDLL)
  {
    lpfnDllFuncStartSplash = (LPDLLFUNC_START_SPLASH)::GetProcAddress(hDLL,"StartSplash");
    if (!lpfnDllFuncStartSplash)
    {
      AfxFreeLibrary(hDLL);
      hDLL = NULL;
    }
    lpfnDllFuncEndSplash = (LPDLLFUNC_END_SPLASH)::GetProcAddress(hDLL,"EndSplash");
    if (!lpfnDllFuncEndSplash)
    {
      AfxFreeLibrary(hDLL);
      hDLL = NULL;
    }
    lpfnDllFuncStartSplash();
  }
*/
	m_pAboutDlg = new CAboutDlg(true);
	m_pAboutDlg->Create(CAboutDlg::IDD);
	/*m_pAboutDlg->ShowWindow(SW_SHOW);
	m_pAboutDlg->SetWindowPos(&CWnd::wndTopMost,0,0,0,0,SWP_NOMOVE|SWP_NOSIZE);*/
	m_pAboutDlg->UpdateWindow ();

  // InitCommonControls() is required on Windows XP if an application
  // manifest specifies use of ComCtl32.dll version 6 or later to enable
  // visual styles.  Otherwise, any window creation will fail.
  InitCommonControls();

  CWinApp::InitInstance();

  m_main_tree_control = new CGlobalTree;

  // Initialize OLE libraries
  if (!AfxOleInit())
  {
    AfxMessageBox(IDP_OLE_INIT_FAILED);
/*    if(hDLL)
    {
      if (lpfnDllFuncEndSplash)
        lpfnDllFuncEndSplash(NULL);
      AfxFreeLibrary(hDLL);
      hDLL = NULL;
    }*/
    return FALSE;
  }
  AfxInitRichEdit();
  AfxEnableControlContainer();
  // Standard initialization
  // If you are not using these features and wish to reduce the size
  // of your final executable, you should remove from the following
  // the specific initialization routines you do not need
  // Change the registry key under which our settings are stored
  // TODO: You should modify this string to be something appropriate
  // such as the name of your company or organization
  SetRegistryKey(COMPANY_NAME_FOR_REGISTER);

  m_reg_manager = new CRegisterManager(this);
  LoadStdProfileSettings(4);  // Load standard INI file options (including MRU)
  // Register the application's document templates.  Document templates
  //  serve as the connection between documents, frame windows and views

  m_reg_manager->LoadSettings();

  CEGMenu::SetMenuDrawMode( m_reg_manager->m_register_settings.interface_theme );
  CEGMenu::SetXpBlending(TRUE);
  CEGMenu::SetAcceleratorsDraw(TRUE);


  HMODULE h = ::GetModuleHandle( NULL );
  TCHAR szPath[MAX_PATH];
  GetModuleFileName( h, szPath, MAX_PATH );
  PathRemoveFileSpec(szPath);

  CString tmpStr(szPath);
  tmpStr += "\\";
  m_application_Path = tmpStr;
  m_FontsPathsArray.clear();

  sgInitKernel();
  sgC3DObject::AutoTriangulate(true, SG_DELAUNAY_TRIANGULATION);
  LuaRegister();

  sgSetApplicationInterface(this);

  GetFontFiles("");
  for (size_t i=0,sz=m_FontsPathsArray.size();i<sz;i++)
  {
    CString aaa = m_application_Path+m_FontsPathsArray[i];
    sgFontManager::AttachFont(sgCFont::LoadFont(aaa,NULL,0));
  }

  if (m_main_pluginer)
    m_main_pluginer->LoadAllPlugins();

  m_pDocManager = new CDocManagerEx;        // we replace the default doc manager
  COneDocTemplate* pDocTemplateGeometry;
  pDocTemplateGeometry = new COneDocTemplate(IDR_GEOMETRY_TYPE,
    RUNTIME_CLASS(CNuGenDimensionDoc),
    RUNTIME_CLASS(CChildFrame),
    RUNTIME_CLASS(CNuGenDimensionView));
  if (!pDocTemplateGeometry)
  {
    /*if(hDLL)
    {
      if (lpfnDllFuncEndSplash)
        lpfnDllFuncEndSplash(NULL);
      AfxFreeLibrary(hDLL);
      hDLL = NULL;
    }*/
    return FALSE;
  }
  AddDocTemplate(pDocTemplateGeometry);

  COneDocTemplate* pDocTemplateReport;
  pDocTemplateReport = new COneDocTemplate(IDR_REPORT_TYPE,
    RUNTIME_CLASS(CReportCreatorDoc),
    RUNTIME_CLASS(CReportChildFrame),
    RUNTIME_CLASS(CReportCreatorView));
  if (!pDocTemplateReport)
  {
    /*if(hDLL)
    {
      if (lpfnDllFuncEndSplash)
        lpfnDllFuncEndSplash(NULL);
      AfxFreeLibrary(hDLL);
      hDLL = NULL;
    }*/
    return FALSE;
  }
  AddDocTemplate(pDocTemplateReport);

  HBITMAP hBmp = CombineResources( 16, IDB_GEOMETRY_TOOLBAR_TC, NULL );
  BITMAPINFO bi;
  memset( &bi, 0, sizeof(BITMAPINFO) );
  GetBitmapInfo( hBmp, &bi );
  pDocTemplateGeometry->m_NewMenuShared.LoadToolBar( hBmp, CSize( bi.bmiHeader.biWidth, 16 ), menuIDs, CLR_NONE );
  pDocTemplateGeometry->m_NewMenuShared.SetMenuItemBitmap(ID_FILE_OPEN,IDB_DEL_OBJ_TC,AfxGetResourceHandle());
  DeleteObject(hBmp);

  // create main MDI Frame window
  CMainFrame* pMainFrame = new CMainFrame;

  if (!pMainFrame || !pMainFrame->LoadFrame(IDR_MAINFRAME))
  {
    /*if(hDLL)
    {
      if (lpfnDllFuncEndSplash)
        lpfnDllFuncEndSplash(NULL);
      AfxFreeLibrary(hDLL);
      hDLL = NULL;
    }*/
    return FALSE;
  }
  m_pMainWnd = pMainFrame;
  // call DragAcceptFiles only if there's a suffix
  //  In an MDI app, this should occur immediately after setting m_pMainWnd
  // Enable drag/drop open
  m_pMainWnd->DragAcceptFiles();
  // Enable DDE Execute open
  EnableShellOpen();
  RegisterShellFileTypes();
  // Parse command line for standard shell commands, DDE, file open
  CCommandLineInfo cmdInfo;
  ParseCommandLine(cmdInfo);
  // Dispatch commands specified on the command line.  Will return FALSE if
  // app was launched with /RegServer, /Register, /Unregserver or /Unregister.

  //if (cmdInfo.m_strFileName.IsEmpty())
  //  cmdInfo.m_nShellCommand = CCommandLineInfo::FileNothing;

  ((CDocManagerEx*)m_pDocManager)->StartApplication(cmdInfo);



  /*if(hDLL)
  {
    if (lpfnDllFuncEndSplash)
      lpfnDllFuncEndSplash(pMainFrame->m_hWnd);
    AfxFreeLibrary(hDLL);
    hDLL = NULL;
  }*/

  // The main window has been initialized, so show and update it
  pMainFrame->ShowWindow(SW_SHOWMAXIMIZED);
  pMainFrame->UpdateWindow();

  return TRUE;
}

IProgresser*  CNuGenDimensionApp::GetProgresser()
{return  static_cast<CMainFrame*>(m_pMainWnd);}

ISceneTreeControl*  CNuGenDimensionApp::GetSceneTreeControl()
{return m_main_tree_control;}

void  CNuGenDimensionApp::GetFontFiles(CString sPath)
{
  if (m_FontsPathsArray.size()>=512)
    return;

  CString sStr;
  CString sCurFullPath=m_application_Path;

  sCurFullPath+=sPath;
  sCurFullPath+="*";

  WIN32_FIND_DATA FindData;
  HANDLE hFindFiles=FindFirstFile(sCurFullPath,&FindData);

  if (hFindFiles==INVALID_HANDLE_VALUE) return;

  for(;;)
  {
    if ((strcmp(FindData.cFileName,".")!=0) && (strcmp(FindData.cFileName,"..")!=0))
    {
      if (FindData.dwFileAttributes&FILE_ATTRIBUTE_DIRECTORY)
      {
        sStr=sPath;
        sStr+=FindData.cFileName;
        sStr+="\\";
        GetFontFiles(sStr);
      }
      else
      {
        char *ptr=strrchr(FindData.cFileName,'.');
        if (ptr)
        {
          if (strlen(ptr)==4)
          {
            if ((ptr[1]=='s' && ptr[2]=='h' && ptr[3]=='x') ||
              (ptr[1]=='S' && ptr[2]=='H' && ptr[3]=='X'))   //проверка расширения
            {
              CString sPath1=sPath;
              sPath1+=FindData.cFileName;
              m_FontsPathsArray.push_back(sPath1);
            }
          }
        }
      }
    }
    if (!FindNextFile(hFindFiles,&FindData)) break;
  }
  FindClose(hFindFiles);
}

int CNuGenDimensionApp::ExitInstance()
{
  m_reg_manager->SaveSettings();
  LuaUnregister();
  sgFreeKernel();
  delete m_pAboutDlg;

  return CWinApp::ExitInstance();
}


CAboutDlg::CAboutDlg(bool bHideButtonOK) : CDialog(CAboutDlg::IDD)
{
	m_bHideButtonOK=bHideButtonOK;
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BMP_GENTIBASE, m_stGen);
	DDX_Control(pDX, IDC_BMP_GENTIBASE2, m_stGen1);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	ON_WM_TIMER()
END_MESSAGE_MAP()

// App command to run the dialog
void CNuGenDimensionApp::OnAppAbout()
{
  CAboutDlg aboutDlg;
  aboutDlg.DoModal();
}
#include "Drawer.h"
BOOL CAboutDlg::OnInitDialog()
{
  CDialog::OnInitDialog();
  CString csVersion, csApp;
  csApp = "NuGenDimension.exe";
  getVersion(csApp,csVersion);

  int aaa[4];
  sgGetVersion(aaa[0],aaa[1],aaa[2],aaa[3]);

  CString ff;
  ff.Format("%i.%i.%i.%i",aaa[0],aaa[1],aaa[2],aaa[3]);

  if (Drawer::is_VBO_Supported)
	GetDlgItem(IDC_ABOUT_SERIAL)->SetWindowText("1111-123456789");
  else
    GetDlgItem(IDC_ABOUT_SERIAL)->SetWindowText("0000-123456789");

  GetDlgItem(IDC_ABOUT_CORE_VERSION)->SetWindowText(ff);
  GetDlgItem(IDC_VER)->SetWindowText(csVersion);
  if (m_bHideButtonOK)
  { GetDlgItem(IDOK)->ShowWindow(SW_HIDE);
	SetTimer(1,4000,NULL);
	m_stGen.ShowWindow(SW_HIDE);
  }
  else
    m_stGen1.ShowWindow(SW_HIDE);

  return TRUE;  // return TRUE unless you set the focus to a control
  // EXCEPTION: OCX Property Pages should return FALSE
}


void CAboutDlg::getVersion(CString &csFileName, CString &csVersion)
{
    DWORD vSize;
    DWORD vLen,langD;
    BOOL retVal;    
    
    LPVOID version=NULL;
    LPVOID versionInfo=NULL;
    char fileVersion[256];
    bool success = true;
    vSize = GetFileVersionInfoSize(csFileName,&vLen);
    if (vSize) 
    {
        versionInfo = (LPVOID)new BYTE[vSize+1];
        if (GetFileVersionInfo(csFileName,vLen,vSize,versionInfo))
        {            
            sprintf_s(fileVersion,255,"\\VarFileInfo\\Translation");
            retVal = VerQueryValue(versionInfo,fileVersion,&version,(UINT *)&vLen);
            if (retVal && vLen==4) 
            {
                memcpy(&langD,version,4);            
                sprintf_s(fileVersion, 255, "\\StringFileInfo\\%02X%02X%02X%02X\\FileVersion",
                        (langD & 0xff00)>>8,langD & 0xff,(langD & 0xff000000)>>24, 
                        (langD & 0xff0000)>>16);            
            }
            else 
                sprintf_s(fileVersion, 255,"\\StringFileInfo\\%04X04B0\\FileVersion",
                        GetUserDefaultLangID());
            retVal = VerQueryValue(versionInfo,fileVersion,&version,(UINT *)&vLen);
            if (!retVal) success = false;
        }
    else 
            success = false;
    }
    else 
        success=false;    
    
    if (success) 
    {
        if (vLen<256)
            strcpy_s(fileVersion,255,(char *)version);
        else 
        {
            strncpy_s(fileVersion,255,(char *)version,250);
            fileVersion[250]=0;            
        }
        if (versionInfo) free(versionInfo);
        versionInfo = NULL;
        csVersion=fileVersion;
    }
    else 
    {
        if (versionInfo) free(versionInfo);
        versionInfo = NULL;
        csVersion="";    
    }
}

BOOL CNuGenDimensionApp::PreTranslateMessage(MSG* pMsg)
{
	try //#try
	{
		if (translate_messages_through_app)
		return CWinApp::PreTranslateMessage(pMsg);

		if (global_opengl_view && pMsg->message==WM_KEYDOWN && ::GetKeyState(VK_RCONTROL)<0)
		{
		//CRuntimeClass* rtc = CWnd::FromHandle(pMsg->hwnd)->GetRuntimeClass();
		//if (/*rtc!=RUNTIME_CLASS(CEdit) && */rtc!=RUNTIME_CLASS(CRichEditCtrl))
			{
				if (pMsg->wParam==VK_LEFT)
				{
				  if (global_opengl_view->GetHandAction()==HA_MOVE)
				  {
					global_opengl_view->OnMoveLeftAuto();
					return TRUE;
				  }
				  else
					if (global_opengl_view->GetHandAction()==HA_ROTATE)
					{
					  global_opengl_view->RotateCamera(CSize(-1,0));
					  return TRUE;
					}
				}
				if (pMsg->wParam==VK_RIGHT)
				{
				  if (global_opengl_view->GetHandAction()==HA_MOVE)
				  {
					global_opengl_view->OnMoveRightAuto();
					return TRUE;
				  }
				  else
					if (global_opengl_view->GetHandAction()==HA_ROTATE)
					{
					  global_opengl_view->RotateCamera(CSize(1,0));
					  return TRUE;
					}
				}
				if (pMsg->wParam==VK_UP)
				{
				  if (global_opengl_view->GetHandAction()==HA_MOVE)
				  {
					global_opengl_view->OnMoveUpAuto();
					return TRUE;
				  }
				  else
					if (global_opengl_view->GetHandAction()==HA_ROTATE)
					{
					  global_opengl_view->RotateCamera(CSize(0,1));
					  return TRUE;
					}
					else
					  if (global_opengl_view->GetHandAction()==HA_ZOOM)
					  {
						global_opengl_view->OnZoomPlusAuto();
						return TRUE;
					  }
				}
				if (pMsg->wParam==VK_DOWN)
				{
				  if (global_opengl_view->GetHandAction()==HA_MOVE)
				  {
					global_opengl_view->OnMoveDownAuto();
					return TRUE;
				  }
				  else
					if (global_opengl_view->GetHandAction()==HA_ROTATE)
					{
					  global_opengl_view->RotateCamera(CSize(0,-1));
					  return TRUE;
					}
					else
					  if (global_opengl_view->GetHandAction()==HA_ZOOM)
					  {
						global_opengl_view->OnZoomMinusAuto();
						return TRUE;
					  }
				}
			}
		}
		if (pMsg->message==WM_MOUSEMOVE ||
		pMsg->message==WM_LBUTTONDOWN ||
		pMsg->message==WM_LBUTTONUP ||
		pMsg->message==WM_KEYDOWN ||
		//pMsg->message==WM_KEYUP ||
		pMsg->message==WM_CHAR){
		  if (global_commander && global_commander->PreTranslateMessage(pMsg))
			return TRUE;
		}
  }  
  catch(...)
  { 
  }
  

  return CWinApp::PreTranslateMessage(pMsg);
}


sgCObject*   GetObjectTopParent(const sgCObject* ob)
{
  if (ob==NULL)
    return NULL;
  sgCObject* par = const_cast<sgCObject*>(ob->GetParent());
  if (par==NULL)
    return NULL;
  while (par->GetParent()!=NULL)
  {
    par = const_cast<sgCObject*>(par->GetParent());
  }
  return par;
}

CRect        FitFirstRectToSecond(CSize& fitSz, CRect& windRect)
{
  CRect resRect;
  double frame_width = min(windRect.Width(), fitSz.cx);
  double frame_height = min(windRect.Height(), fitSz.cy);
  double scale_w = frame_width / fitSz.cx;
  double scale_h = frame_height / fitSz.cy;
  double scale = min(scale_w, scale_h);
  frame_width = fitSz.cx * scale;
  frame_height = fitSz.cy * scale;
  /*resRect.left   = windRect.CenterPoint().x - frame_width  / 2;
  resRect.right  = windRect.CenterPoint().x + frame_width  / 2;
  resRect.top    = windRect.CenterPoint().y - frame_height / 2;
  resRect.bottom = windRect.CenterPoint().y + frame_height / 2; #WARNING */

  resRect.left   = windRect.CenterPoint().x - (int)frame_width  / 2;
  resRect.right  = windRect.CenterPoint().x + (int)frame_width  / 2;
  resRect.top    = windRect.CenterPoint().y - (int)frame_height / 2;
  resRect.bottom = windRect.CenterPoint().y + (int)frame_height / 2;


  return resRect;
}

CString      GetLeftHalfOfString(UINT nID)
{
  CString str;
  str.LoadString(nID);
  int nIndex = str.ReverseFind(_T('\n'));
  if(nIndex!=-1)
  {
    str=str.Mid(nIndex+1);
  }
  return str;
}

void   DrawGroupFrame(CDC* pDC, const CRect& rct,
               const int leftLab, const int rightLab)
{
  CPen pen1;
  pen1.CreatePen(PS_SOLID, 1, ::GetSysColor(COLOR_3DSHADOW));
  CPen* pOldPen = (CPen*) pDC->SelectObject(&pen1);

  pDC->MoveTo(leftLab,rct.top);
  pDC->LineTo(rct.left,rct.top);
  pDC->LineTo(rct.left,rct.bottom-1);
  pDC->LineTo(rct.right-1,rct.bottom-1);
  pDC->LineTo(rct.right-1,rct.top);
  pDC->LineTo(rightLab,rct.top);

  CPen pen2;
  pen2.CreatePen(PS_SOLID, 1, ::GetSysColor(COLOR_3DHILIGHT));
  pDC->SelectObject(&pen2);
  DeleteObject(pen1);

  pDC->MoveTo(leftLab,rct.top+1);
  pDC->LineTo(rct.left+1,rct.top+1);
  pDC->LineTo(rct.left+1,rct.bottom-1);
  pDC->MoveTo(rct.left,rct.bottom);
  pDC->LineTo(rct.right,rct.bottom);
  pDC->LineTo(rct.right,rct.top);
  pDC->MoveTo(rct.right-1,rct.top+1);
  pDC->LineTo(rightLab,rct.top+1);

  pDC->SelectObject(pOldPen);
  DeleteObject(pen2);
}


void CAboutDlg::OnTimer(UINT_PTR nIDEvent)
{
	// Close splash window
	ShowWindow(SW_HIDE);
	KillTimer(nIDEvent);

	CDialog::OnTimer(nIDEvent);
}
