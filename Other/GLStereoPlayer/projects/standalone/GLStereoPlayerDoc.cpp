//-----------------------------------------------------------------------------
// GLStereoPlayerDoc.cpp : Implementation of the CGLStereoPlayerDoc class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "GLStereoPlayer.h"

#include "GLStereoPlayerDoc.h"
#include "GLStereoPlayerView.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerDoc

IMPLEMENT_DYNCREATE(CGLStereoPlayerDoc, CDocument)

BEGIN_MESSAGE_MAP(CGLStereoPlayerDoc, CDocument)
    //{{AFX_MSG_MAP(CGLStereoPlayerDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerDoc construction/destruction

CGLStereoPlayerDoc::CGLStereoPlayerDoc()
{
	m_bSaveModified = TRUE;
}

CGLStereoPlayerDoc::~CGLStereoPlayerDoc()
{
}



/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerDoc serialization

void CGLStereoPlayerDoc::Serialize(CArchive& ar)
{
    if (ar.IsStoring())
    {
    }
    else
    {
    }
}

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerDoc diagnostics

#ifdef _DEBUG
void CGLStereoPlayerDoc::AssertValid() const
{
    CDocument::AssertValid();
}

void CGLStereoPlayerDoc::Dump(CDumpContext& dc) const
{
    CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerDoc commands

BOOL CGLStereoPlayerDoc::OnOpenDocument(LPCTSTR lpszPathName) 
{
    // Load a source file or a setting file through the standard Doc/View process
    POSITION pos = GetFirstViewPosition();
    if (pos != NULL)
        return ((CGLStereoPlayerView*)GetNextView(pos))->OpenFile(lpszPathName);

    return FALSE;
}

BOOL CGLStereoPlayerDoc::OnSaveDocument(LPCTSTR lpszPathName) 
{
    // Save a setting file through the standard Doc/View process
    POSITION pos = GetFirstViewPosition();
    if (pos != NULL)
        return ((CGLStereoPlayerView*)GetNextView(pos))->SaveFile(lpszPathName);
    
    return FALSE;
}

void CGLStereoPlayerDoc::SetPathName(LPCTSTR lpszPathName, BOOL bAddToMRU) 
{
    // If a setting file is saved, set it as a current document file
    CString cstrFileName = lpszPathName;
    if (cstrFileName.Right(4).CompareNoCase(".gsp")==0 || cstrFileName.Right(4).CompareNoCase(".xml")==0)
        CDocument::SetPathName(lpszPathName, bAddToMRU);
}

BOOL CGLStereoPlayerDoc::SaveModified() 
{
	if (!((CMainFrame*)AfxGetMainWnd())->m_bSaveModified)
		return TRUE;
	
	return CDocument::SaveModified();
}
