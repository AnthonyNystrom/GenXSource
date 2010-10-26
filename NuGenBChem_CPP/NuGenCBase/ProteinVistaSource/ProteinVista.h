// ProteinVista.h : main header file for the ProteinVista application
//

#if !defined(AFX_ProteinVista_H__698DEA13_A6F8_421B_810B_B8C827F5B9E3__INCLUDED_)
#define AFX_ProteinVista_H__698DEA13_A6F8_421B_810B_B8C827F5B9E3__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols
#include "pdb.h"
#include "ProteinVistaView.h"
#include "HTMLListCtrl.h"

using namespace System;
using namespace std;
class CProteinVistaApp  
{
public:
	CProteinVistaApp();
	
	~CProteinVistaApp();
public:
	Gdiplus::GdiplusStartupInput m_gdiplusStartupInput;
	ULONG_PTR m_gdiplusToken;

	CString		m_strCurrentPluginFile;
	 
	CString		m_strBaseExePath;
	CString		m_strBaseResPath;
 
	CString		m_strPDBPath;
	CString		m_strBasePlugInPath;
	CString		m_strBaseSavePDBSelection;	 
	CString		m_strBaseDownloadPDB;		 
	CString		m_strBaseWorkspacePath;		 
	CString		m_strBaseSurfacePath;		 
	CString		m_strBaseTempPath;
	CString		m_strBaseTexturePath;
	CString		m_strBaseTutorialPath;
	CString		m_strBaseScriptHelpPath;
	CString		m_strBaseScriptPath;
	CString		m_strBaseScriptMovie;
	
	CString		m_strBaseCPPPlugIn;
	CString		m_strBaseCSPlugIn;
	CString		m_strBaseVBPlugIn;

	CString		m_strModulePath;
	CString		m_strPathWorkspace;

	BOOL		m_bRunningPlugin;
	BOOL		m_bFirstExecute;

	HWND        m_CanvsHandle;
	HINSTANCE   m_HInstance;

	BOOL  m_bOpenWorkSpace;
 
	CProteinVistaView * GetActiveProteinVistaView()
	{
		return this->mCurrentVistaView;
	}

	HRESULT MakeMovieWithImages(CString movieFilename, CStringArray & strArrayFilename, CSTLIntArray & arrayFrame, int width, int height, int fps);
	void OnMakeMovie();

	BOOL CheckExistPDBFile(CString &strFilename, CString &strNewFilename);
	BOOL DownloadPDB(CString & strPDBID, CString & strDownloadPDBFilename);

public:
	CStringArray m_pComboBoxActivePDB;

	CString m_strEnvIniFileName;
	BOOL	m_bFullScreen;
	CRect	m_rcMainFrame;
	CProteinVistaView*  mCurrentVistaView;
    ////////////////////////////////////
 public:
	BOOL InitInstance();
	void  CreateView();
 
	void OnOpenWorkspace(void);
	BOOL OpenWorkspaceFile(CString &strFilename );
	void OnSaveWorkspace(void);
	void OnSaveAsWorkspace(void);
	void SaveWorkspaceFile(CString filename);

	void OpenPDBFiles(array<String^>^ pdbFiles );

	void OnFileOpen();
	void OnAppAbout();
	void OnWindowCloseAll();
	 
	void OnFileOpenPDBID();
public: 
	BOOL OpenDocumentFile(LPCTSTR lpszPathName);
	void DeleteContents();
	BOOL AnalizePDBFile(LPCTSTR lpszPathName);
 
};
#endif // !defined(AFX_ProteinVista_H__698DEA13_A6F8_421B_810B_B8C827F5B9E3__INCLUDED_)
