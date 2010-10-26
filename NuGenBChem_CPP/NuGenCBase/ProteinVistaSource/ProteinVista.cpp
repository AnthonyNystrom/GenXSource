// ProteinVista.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
 
#include "Utility.h"
#include "FileDialogExt.h"
#include "Afxinet.h"
#include "OpenPDBIDDialog.h"
#include "Utils.h"
#include "pdb.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


/////////////////////////////////////////////////////////////////////////////

BOOL	g_bRequestRender = TRUE;
#ifdef _DEBUG
BOOL	g_bDisplayFPS = TRUE;
#else
BOOL	g_bDisplayFPS = FALSE;
#endif
using namespace System;
BOOL SetRegKey(LPCTSTR lpszKey, LPCTSTR lpszValue, LPCTSTR lpszValueName = NULL);

/////////////////////////////////////////////////////////////////////////////
// CProteinVistaApp initialization
CProteinVistaApp::CProteinVistaApp()
{
	m_bFirstExecute = FALSE;
	m_bRunningPlugin = FALSE;
	this->mCurrentVistaView = new CProteinVistaView();
	m_bOpenWorkSpace =FALSE;
}
CProteinVistaApp::~CProteinVistaApp()
{
	OnWindowCloseAll();
	Gdiplus::GdiplusShutdown(m_gdiplusToken);
	SAFE_DELETE(mCurrentVistaView);
}
BOOL CProteinVistaApp::InitInstance()
{ 	
	Gdiplus::GdiplusStartup(&m_gdiplusToken, &m_gdiplusStartupInput, NULL);

	TCHAR szFilename[512];
	GetModuleFileName(NULL, szFilename, 512);
	m_strModulePath = szFilename;

	TCHAR drive[MAX_PATH];
	TCHAR dir[MAX_PATH];
	_splitpath(szFilename, drive, dir, NULL, NULL );

	CString strCurrentPath;
	strCurrentPath.Format("%s%s" , drive, dir );
	SetCurrentDirectory(strCurrentPath);

	WriteProfileString( "Path" , "ExePath", szFilename );
	{
		TCHAR szFilename[512];
		GetModuleFileName(NULL, szFilename, 512);

		char drive[_MAX_DRIVE];
		char dir[_MAX_DIR];
		char fname[_MAX_FNAME];
		char ext[_MAX_EXT];
		
		_splitpath( szFilename, drive, dir, fname, ext );
		m_strBaseExePath.Format("%s%s" , drive, dir );
		 
		m_strBaseResPath.Format("%s%sD3DResource\\", drive, dir );
		CString m_strBaseResPathTemp;
		m_strBaseResPathTemp.Format("%s%sD3DResource", drive, dir );

		if ( SetCurrentDirectory(m_strBaseResPathTemp) == FALSE )
		{
			//	debug 이나, release 를 빼고 조사한다.
			CString strTemp;
			strTemp.Format("%s%s", drive, dir );
			strTemp = strTemp.Left(strTemp.GetLength()-1);
			long pos = strTemp.ReverseFind('\\');
			strTemp = strTemp.Left(pos+1);

			m_strBaseResPath = strTemp + "D3DResource\\";
			m_strBaseResPathTemp = strTemp + "D3DResource";

			if ( SetCurrentDirectory(m_strBaseResPathTemp) == FALSE )
			{
				//CString strText;
				//strText.Format("Cannot find resource file(%s%sD3DResource). Program will terminate..." , drive, dir );
				//AfxMessageBox(strText);
				//return FALSE;
			}
			m_strBaseExePath = strTemp;
		}

		m_strBasePlugInPath.Format( "%sPlugIns\\", m_strBaseExePath );
		//CreateDirectory(m_strBasePlugInPath, NULL);

		m_strBaseSavePDBSelection.Format( "%sSaveSelection\\", m_strBaseExePath );
		CreateDirectory(m_strBaseSavePDBSelection, NULL);

		m_strBaseDownloadPDB.Format( "%sDownloadPDB\\", m_strBaseExePath );
		CreateDirectory(m_strBaseDownloadPDB, NULL);

		m_strBaseWorkspacePath.Format( "%sWorkspace\\", m_strBaseExePath );
		CreateDirectory(m_strBaseWorkspacePath, NULL);

		m_strBaseSurfacePath.Format( "%sSurface\\", m_strBaseExePath );
		CreateDirectory(m_strBaseSurfacePath, NULL);

		m_strBaseTempPath.Format( "%sTemp\\", m_strBaseExePath );
		CreateDirectory(m_strBaseTempPath, NULL);

		m_strBaseTexturePath.Format( "%sTextures\\", m_strBaseExePath );
		CreateDirectory(m_strBaseTexturePath, NULL);

		m_strBaseTutorialPath.Format( "%sTutorial\\", m_strBaseExePath );
		//CreateDirectory(m_strBaseTutorialPath, NULL);

		m_strBaseScriptHelpPath.Format( "%sScriptHelp\\", m_strBaseExePath );
		//CreateDirectory(m_strBaseScriptHelpPath, NULL);

		m_strBaseScriptPath.Format( "%sScript\\", m_strBaseExePath );
		//CreateDirectory(m_strBaseScriptPath, NULL);

		m_strBaseScriptMovie.Format( "%sMovie\\", m_strBaseExePath );
		//CreateDirectory(m_strBaseScriptMovie, NULL);

		m_strBaseCPPPlugIn.Format( "%sPlugInTemplate\\CPPTemplate\\", m_strBaseExePath );
		m_strBaseCSPlugIn.Format( "%sPlugInTemplate\\CSharpTemplate\\", m_strBaseExePath );
		m_strBaseVBPlugIn.Format( "%sPlugInTemplate\\VBTemplate\\", m_strBaseExePath );

		SetCurrentDirectory(m_strBaseExePath);
	}

	SetRegKey(".ent", "ProteinInsight.Document");
	SetRegKey(".piw", "ProteinInsight.Document");

	CString strIconPath = "\"" + m_strModulePath + "\"";
	SetRegKey("ProteinInsight.Document\\DefaultIcon", strIconPath );

	CString strCommandPath = "\"" + m_strModulePath + "\"" + " \"%1\"";
	SetRegKey("ProteinInsight.Document\\shell\\open\\command", strCommandPath );
	SetRegKey("ProteinInsight.Document", "ProteinInsight.Document");
   
	return TRUE;
}
 

BOOL CProteinVistaApp::OpenDocumentFile(LPCTSTR lpszFileName)
{
	TCHAR ext[MAX_PATH];
	_splitpath(lpszFileName, NULL, NULL, NULL, ext );
	if ( (_stricmp(ext, ".ent") == 0) || (_stricmp(ext, ".pdb") == 0) )
	{
		CString strFilename(lpszFileName);
		CString strNewFilename;
		if (this->CheckExistPDBFile(strFilename, strNewFilename) == TRUE )
		{
		    this->mCurrentVistaView->OnClosePdb(FALSE);
			m_strPathWorkspace.Empty();
			this->AnalizePDBFile(strNewFilename);
			return TRUE;
		}
	}
	else if ( _stricmp(ext, ".piw") == 0 ) 
	{
		CString strFilename(lpszFileName);
		if ( OpenWorkspaceFile(strFilename) == TRUE )
			return TRUE;
	}
	TCHAR buff[MAX_PATH];
	sprintf(buff, "Cannot open %s", lpszFileName);
	AfxMessageBox(buff);
	return TRUE;
}

void CProteinVistaApp::OnWindowCloseAll()
{
	m_strPathWorkspace.Empty();
}
BOOL CProteinVistaApp::AnalizePDBFile(LPCTSTR lpszPathName)
{
	CPDB * pPDB = new CPDB;
	HRESULT hr = pPDB->Load(lpszPathName);
	if ( SUCCEEDED(hr) )
	{
		//	구조 분석을 한다.
		pPDB->AnalizeStructure();
		this->GetActiveProteinVistaView()->m_arrayPDB.push_back(pPDB);
		this->GetActiveProteinVistaView()->CreatePanel();
	}
	else
	{
		delete pPDB;
		return FALSE;
	}
	return TRUE;
}
void CProteinVistaApp::OpenPDBFiles(array<String^>^ pdbFiles )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (pdbFiles == nullptr || pdbFiles->Length ==0)
		return;
	CStringArray strFileArray;
	for(int i=0;i<pdbFiles->Length;i++)
	{
		strFileArray.Add(MStrToCString(pdbFiles[i]));
	}
	if ( strFileArray.GetSize() > 0 )
	{
		//	
		CStringArray	strFileArrayNew;
		for ( int i = 0 ; i < strFileArray.GetSize() ; i++ )
		{
			CString strNewFilename;
			if ( CheckExistPDBFile(strFileArray[i], strNewFilename) == TRUE )
				strFileArrayNew.Add(strNewFilename);
		}

		if ( strFileArrayNew.GetSize() > 0 )
		{
			OpenDocumentFile(strFileArrayNew[0]);
			for(int i = 1; i < strFileArrayNew.GetSize(); i++)
			{
				this->GetActiveProteinVistaView()->AddPDB(strFileArrayNew[i]);
			}
		}
	}
}
void CProteinVistaApp::OnFileOpen()
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CStringArray strFileArray;	// 파일 이름을 저장하기 위한 Array

	// 파일 필터 설정
	static char szFilter[] = "PDB Files (*.ent;*.pdb)|*.ent; *.pdb|All Files (*.*)|*.*||";
   
	CWnd* ownerWnd = CWnd::FromHandle(this->m_CanvsHandle);
	CFileDialogExt	fileDialog( TRUE, "ent" , "*.ent" , OFN_ALLOWMULTISELECT , szFilter , ownerWnd);

	fileDialog.m_ofn.lpstrInitialDir = m_strBaseDownloadPDB;

	long ret = fileDialog.DoModal();
	
	if (ret == IDOK)
	{
		// 선택된 파일이름을 Array에 저장
		POSITION pos = fileDialog.GetStartPosition();
		while(pos != NULL)
		{
			CString name = fileDialog.GetNextPathName(pos);
			POSITION pos = fileDialog.GetStartPosition();
			strFileArray.Add(name);
		}
	}	
	if ( ret == IDCANCEL && fileDialog.m_bDownloadPDB == TRUE )
	{	
		for ( int i = 0 ; i < fileDialog.m_strArrayPDBID.GetSize() ; i++ )
		{
			strFileArray.Add(fileDialog.m_strArrayPDBID[i]);
		}
	}
    if ( strFileArray.GetSize() > 0 )
	{
		//	
		CStringArray	strFileArrayNew;
		for ( int i = 0 ; i < strFileArray.GetSize() ; i++ )
		{
			CString strNewFilename;
			if ( CheckExistPDBFile(strFileArray[i], strNewFilename) == TRUE )
				strFileArrayNew.Add(strNewFilename);
		}

		if ( strFileArrayNew.GetSize() > 0 )
		{
			OpenDocumentFile(strFileArrayNew[0]);
			for(int i = 1; i < strFileArrayNew.GetSize(); i++)
			{
				this->GetActiveProteinVistaView()->AddPDB(strFileArrayNew[i]);
			}
		}
	}
}

 
BOOL CProteinVistaApp::CheckExistPDBFile(CString &strFilename, CString &strNewFilename)
{
	//	file find rule
	//	1. 저장된 full path 를 찾음
	//	1.5 현재 path 에서 찾음.
	//	2. m_strPDBPath 에서 찾음
	//	3. m_strBaseDownloadPDB 에서 찾음
	//	4. rcsb.org 에서 m_strBaseDownloadPDB로 download 해서 filename 저장
	//		(http://www.rcsb.org/pdb/download/downloadFile.do?fileFormat=pdb&compression=NO&structureId=3APR)
	//
	//	step1.
	CFile file;
	if ( file.Open(strFilename, CFile::modeRead) != 0 )
	{	//	exist...
		file.Close();
		strNewFilename = strFilename;
		return TRUE;
	}
	TCHAR drive[MAX_PATH];
	TCHAR dir[MAX_PATH];
	TCHAR fname[MAX_PATH];
	TCHAR ext[MAX_PATH];
	_splitpath(strFilename, drive, dir, fname, ext);

	//	1.5 현재 path 에서 찾음.
	TCHAR buffCurrentPath[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, buffCurrentPath);
	CString strNewPath;
	strNewPath.Format("%s\\%s%s" , buffCurrentPath, fname, ext);
	if ( file.Open(strNewPath, CFile::modeRead) != 0 )
	{	//	exist...
		file.Close();
		strNewFilename = strNewPath;
		return TRUE;
	}

	//	step2.
	TCHAR buffNewPath[MAX_PATH];
	//	step3.
	wsprintf(buffNewPath, "%s%s%s", m_strBaseDownloadPDB, fname, ext);

	if ( file.Open(buffNewPath, CFile::modeRead) != 0 )
	{	//	exist...
		file.Close();
		strNewFilename = CString(buffNewPath);
		return TRUE;
	}

	//	step4.
	//	download pdb...
	//	USE fname
	CString strPDBID(fname);
	strPDBID.MakeUpper();
	if( strPDBID.Left(3) == _T("PDB"))
	{
		strPDBID.Delete(0,3);
	}

	if ( strPDBID.GetLength() == 4 )
	{
		BOOL bRet = DownloadPDB(strPDBID, strNewFilename);
		if ( bRet == TRUE )
			return TRUE;
	}
	return FALSE;
}

BOOL CProteinVistaApp::DownloadPDB(CString & strPDBID, CString & strDownloadPDBFilename)
{	
	BOOL	bResult = TRUE;

	//	download this path: m_strBaseDownloadPDB
	//	http://www.rcsb.org/pdb/download/downloadFile.do?fileFormat=pdb&compression=NO&structureId=4HHB 
	strDownloadPDBFilename.Format("%spdb%s.ent", m_strBaseDownloadPDB, strPDBID);
	if ( CheckFileExist(strDownloadPDBFilename) == TRUE )
	{
		return TRUE;
	}

	//	새로운 파일을 연다.
	CFile	fileDownload;
	BOOL bRet = fileDownload.Open(strDownloadPDBFilename, CFile::modeCreate|CFile::modeWrite);
	if ( bRet == FALSE )
		return FALSE;

	CString strObject;
	strObject.Format("/pdb/download/downloadFile.do?fileFormat=pdb&compression=NO&structureId=%s" , strPDBID.MakeUpper() );

	long buffSize = 4096*10;
	BYTE * szBuff = new BYTE[buffSize];

	//	assumes server, port, and URL names have been initialized
	CInternetSession session("PDBDownload");
	CHttpConnection* pServer = NULL;
	CHttpFile* pFile = NULL;
	try
	{
		CString strServerName(_T("www.rcsb.org"));
		INTERNET_PORT nPort = 80;
		DWORD		  dwRet;

		pServer = session.GetHttpConnection(strServerName, nPort);
		pFile = pServer->OpenRequest(CHttpConnection::HTTP_VERB_GET, strObject);
		pFile->SendRequest();
		pFile->QueryInfoStatusCode(dwRet);

		ULONGLONG lenFile = pFile->GetLength();
		ULONGLONG totalRead = 0;

		if (dwRet == HTTP_STATUS_OK)
		{
			while(1)
			{
				UINT nRead = pFile->Read(szBuff, buffSize);
				fileDownload.Write(szBuff, nRead);
				totalRead += nRead;
				if ( nRead == 0  )
					break;
			}
		}
	}
	catch (CInternetException* pEx)
	{
		//catch errors from WinInet
		CString strMsg(_T("Cannot connect www.rcsb.org server. Check your Internet connection or server") );
		OutputTextMsg(strMsg);
		AfxMessageBox(strMsg);
		bResult = FALSE;
	}

	SAFE_DELETE(pFile);
	SAFE_DELETE(pServer);

	long downloadFileLen = fileDownload.GetLength();

	session.Close();
	fileDownload.Close();
	delete [] szBuff;

	if ( downloadFileLen < 500 )		//	size is 427 (2007.1.2)
	{
		DeleteFile(strDownloadPDBFilename);
		bResult = FALSE;
	}
	if ( bResult == FALSE )
	{	//	size가 0으로 만들어진 파일을 지운다.
		DeleteFile(strDownloadPDBFilename);
	}

	return bResult;
}


BOOL SetRegKey(LPCTSTR lpszKey, LPCTSTR lpszValue, LPCTSTR lpszValueName )
{
	if (lpszValueName == NULL)
	{
		if (::RegSetValue(HKEY_CLASSES_ROOT, lpszKey, REG_SZ,
			lpszValue, lstrlen(lpszValue) * sizeof(TCHAR)) != ERROR_SUCCESS)
		{
			TRACE(traceAppMsg, 0, _T("Warning: registration database update failed for key '%s'.\n"),
				lpszKey);
			return FALSE;
		}
		return TRUE;
	}
	else
	{
		HKEY hKey;

		if(::RegCreateKey(HKEY_CLASSES_ROOT, lpszKey, &hKey) == ERROR_SUCCESS)
		{
			LONG lResult = ::RegSetValueEx(hKey, lpszValueName, 0, REG_SZ,
				(CONST BYTE*)lpszValue, (lstrlen(lpszValue) + 1) * sizeof(TCHAR));

			if(::RegCloseKey(hKey) == ERROR_SUCCESS && lResult == ERROR_SUCCESS)
				return TRUE;
		}
		TRACE(traceAppMsg, 0, _T("Warning: registration database update failed for key '%s'.\n"), lpszKey);
		return FALSE;
	}
}

//////////////////////////////////////////////////////////////////////////////////////////////
void CProteinVistaApp::OnFileOpenPDBID()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	COpenPDBIDDialog	openPDBIDDialog;
	if ( IDOK == openPDBIDDialog.DoModal() )
	{
		//	
		if ( openPDBIDDialog.m_strArrayPDBID.GetSize() > 0 )
		{
			//	
			CStringArray	strFileArrayNew;
			for ( int i = 0 ; i < openPDBIDDialog.m_strArrayPDBID.GetSize() ; i++ )
			{
				CString strNewFilename;
				if ( CheckExistPDBFile(openPDBIDDialog.m_strArrayPDBID[i], strNewFilename) == TRUE )
					strFileArrayNew.Add(strNewFilename);
			}

			if ( strFileArrayNew.GetSize() > 0 )
			{
				// 첫번째 파일은 다큐먼트를 열기.
				OpenDocumentFile(strFileArrayNew[0]);

				// 나머지는 AddPDB한다...
				for(int i = 1; i < strFileArrayNew.GetSize(); i++)
				{
					 GetActiveProteinVistaView()->AddPDB(strFileArrayNew[i]);
				}
			}
		}
	}
}

 
