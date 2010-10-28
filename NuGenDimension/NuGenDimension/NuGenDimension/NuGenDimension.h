// NuGenDimension.h : main header file for the NuGenDimension application
//
#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

// CNuGenDimensionApp:
// See NuGenDimension.cpp for the implementation of this class
//

#include "ReportEditor/DiagramEditor/DiagramClipboardHandler.h"
#include "PluginsClasses//PluginLoader.h"
#include "Dialogs//SceneTreeDlg.h"

#include "Tools//RegManager.h"
#include "afxwin.h"


class CAboutDlg : public CDialog
{
public:
  CAboutDlg(bool bHideButtonOK=false); 
  bool m_bHideButtonOK;
// Dialog Data
  enum { IDD = IDD_ABOUTBOX };

protected:
  virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
  DECLARE_MESSAGE_MAP()
public:
  virtual BOOL OnInitDialog();
public:
	void getVersion(CString &csFileName, CString &csVersion);
public:
	afx_msg void OnTimer(UINT_PTR nIDEvent);
public:
	CStatic m_stGen;
public:
	CStatic m_stGen1;
};


class CNuGenDimensionApp : public CWinApp,
						public ICoreAppInterface
{
public:
	CNuGenDimensionApp();
	~CNuGenDimensionApp();

	CGlobalTree*       m_main_tree_control;
	CPluginLoader*     m_main_pluginer;

	CRegisterManager*  m_reg_manager;

// Overrides
public:

	virtual IProgresser*		 GetProgresser();
	virtual ISceneTreeControl*   GetSceneTreeControl();  
	CAboutDlg					 *m_pAboutDlg; 


	CDiagramClipboardHandler	m_clip;

	virtual BOOL InitInstance();
private:
	std::vector<CString>   m_FontsPathsArray; 
	CString				   m_application_Path; 
	void				   GetFontFiles(CString sPath);

// Implementation
	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
	virtual int ExitInstance();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
public:
	void GetAppPath(CString& aa) {aa=m_application_Path;};
};

extern CNuGenDimensionApp    theApp;
extern ICommander*       global_commander;
extern bool              translate_messages_through_app;
extern int               group_name_index;


class AppDllInstanceSwitcher
{
public:
	AppDllInstanceSwitcher()
	{
		m_hInst = AfxGetResourceHandle();
		AfxSetResourceHandle(theApp.m_hInstance);
	}
	~AppDllInstanceSwitcher()
	{
		AfxSetResourceHandle(m_hInst);
	}
private:
	HINSTANCE m_hInst;
};

#define APP_SWITCH_RESOURCE  AppDllInstanceSwitcher __SwitchInstance;

sgCObject*   GetObjectTopParent(const sgCObject* ob);
CRect        FitFirstRectToSecond(CSize& fitSz, CRect& windRect);
CString      GetLeftHalfOfString(UINT nID);

void         DrawGroupFrame(CDC* pDC, const CRect& rct, 
							 const int leftLab, const int rightLab);
