#include "stdafx.h"
#include "NuGenDimension.h"
#include "MainFrm.h"
#include "DocManagerEx.h"


IMPLEMENT_DYNAMIC(CDocManagerEx, CDocManager)

static void AppendFilterSuffix(CString& filter, OPENFILENAME& ofn,
							   CDocTemplate* pTemplate, CString* pstrDefaultExt)
{
	ASSERT_VALID(pTemplate);
	ASSERT_KINDOF(CDocTemplate, pTemplate);
	CString strFilterExt, strFilterName;
	if (pTemplate->GetDocString(strFilterExt, CDocTemplate::filterExt) &&
		!strFilterExt.IsEmpty() &&
		pTemplate->GetDocString(strFilterName, CDocTemplate::filterName) &&
		!strFilterName.IsEmpty())
	{
		// a file based document template - add to filter list
#ifndef _MAC
		ASSERT(strFilterExt[0] == '.');
#endif
		if (pstrDefaultExt != NULL)
		{
			// set the default extension
#ifndef _MAC
			*pstrDefaultExt = ((LPCTSTR)strFilterExt) + 1;  // skip the '.'
#else
			*pstrDefaultExt = strFilterExt;
#endif
			ofn.lpstrDefExt = (LPTSTR)(LPCTSTR)(*pstrDefaultExt);
			ofn.nFilterIndex = ofn.nMaxCustFilter + 1;  // 1 based number
		}
		// add to filter
		filter += strFilterName;
		ASSERT(!filter.IsEmpty());  // must have a file type name
		filter += (TCHAR)'\0';  // next string please
#ifndef _MAC
		filter += (TCHAR)'*';
#endif
		filter += strFilterExt;
		filter += (TCHAR)'\0';  // next string please
		ofn.nMaxCustFilter++;
	}
}


BOOL CDocManagerEx::DoPromptFileName(CString& fileName, UINT nIDSTitle, DWORD lFlags, BOOL bOpenFileDialog, CDocTemplate* pTemplate)
{
	CFileDialog dlgFile(bOpenFileDialog); // this is the only modified line!
	CString title;
	VERIFY(title.LoadString(nIDSTitle));
	dlgFile.m_ofn.Flags |= lFlags;
	CString strFilter;
	CString strDefault;
	if (pTemplate != NULL)
	{
		ASSERT_VALID(pTemplate);
		AppendFilterSuffix(strFilter, dlgFile.m_ofn, pTemplate, &strDefault);
	}
	else
	{
		if (AfxGetMainWnd() && ((CMainFrame*)AfxGetMainWnd())->MDIGetActive())
		{
			CDocument * pActiveDoc = ((CMainFrame*)AfxGetMainWnd())->MDIGetActive()->GetActiveDocument();
			if (pActiveDoc)
			{
				CDocTemplate* pActDocT = pActiveDoc->GetDocTemplate();
				if (pActDocT)
				{
					AppendFilterSuffix(strFilter, dlgFile.m_ofn, pActDocT,
						&strDefault);
					goto yesLbl;
				}
			}
		}
		// do for all doc template
		POSITION pos = m_templateList.GetHeadPosition();
		BOOL bFirst = TRUE;
		while (pos != NULL)
		{
			CDocTemplate* pTemplate = (CDocTemplate*)m_templateList.GetNext(pos);
			AppendFilterSuffix(strFilter, dlgFile.m_ofn, pTemplate,
				bFirst ? &strDefault : NULL);
			bFirst = FALSE;
		}
	}
yesLbl:
	dlgFile.m_ofn.lpstrFilter = strFilter;
#ifndef _MAC
	dlgFile.m_ofn.lpstrTitle = title;
#else
	dlgFile.m_ofn.lpstrPrompt = title;
#endif
	dlgFile.m_ofn.lpstrFile = fileName.GetBuffer(_MAX_PATH);
	BOOL bResult = dlgFile.DoModal() == IDOK ? TRUE : FALSE;
	fileName.ReleaseBuffer();
	return bResult;
}

void   CDocManagerEx::ActivateFrame(CDocTemplate* dT)
{
	CDocTemplate* pTemplate = NULL;
	POSITION posit = m_templateList.GetHeadPosition();
	for (INT_PTR i=0;i<m_templateList.GetCount();i++)
	{
		pTemplate = (CDocTemplate*)m_templateList.GetNext(posit);
		ASSERT(pTemplate != NULL);
		ASSERT_KINDOF(CDocTemplate, pTemplate);
		if (pTemplate==dT)
		{
			POSITION docPos = pTemplate->GetFirstDocPosition();
			CDocument* curDoc =  pTemplate->GetNextDoc(docPos);
			POSITION viewPos = curDoc->GetFirstViewPosition();
			CView*  curView = curDoc->GetNextView(viewPos);	
			((CMainFrame*)(theApp.m_pMainWnd))->MDIActivate(curView->GetParentFrame());
			((CMainFrame*)(theApp.m_pMainWnd))->MDIMaximize(curView->GetParentFrame());

			return;
		}
	}
}

void   CDocManagerEx::StartApplication(CCommandLineInfo& cmdInfo)
{
	if( !cmdInfo.m_strFileName.IsEmpty() && cmdInfo.m_nShellCommand == CCommandLineInfo::FileOpen ) 
	{
		CString flExt = cmdInfo.m_strFileName.Right(3);
		CString curFlEx;
		CDocTemplate*  activeDT = NULL;

		CDocTemplate* pTemplate = NULL;
		POSITION posit = m_templateList.GetHeadPosition();
		for (INT_PTR i=0;i<m_templateList.GetCount();i++)
		{
			pTemplate = (CDocTemplate*)m_templateList.GetNext(posit);
			ASSERT(pTemplate != NULL);
			ASSERT_KINDOF(CDocTemplate, pTemplate);
			pTemplate->GetDocString(curFlEx, CDocTemplate::filterExt);
			if ( flExt ==  curFlEx.Right(3)) 
			{
				pTemplate->OpenDocumentFile(cmdInfo.m_strFileName);
				activeDT  = pTemplate;
			} 
			else 
				pTemplate->OpenDocumentFile(NULL);
		}
		if (activeDT)
			ActivateFrame(activeDT);
	} 
	else 
	{
		CDocTemplate* pTemplate = NULL;
		POSITION posit = m_templateList.GetHeadPosition();
		for (INT_PTR i=0;i<m_templateList.GetCount();i++)
		{
			pTemplate = (CDocTemplate*)m_templateList.GetNext(posit);

			ASSERT(pTemplate != NULL);
			ASSERT_KINDOF(CDocTemplate, pTemplate);

			pTemplate->OpenDocumentFile(NULL);
		}
		pTemplate = (CDocTemplate*)m_templateList.GetHead();
		if (pTemplate)
			ActivateFrame(pTemplate);
	}
}


void CDocManagerEx::OnFileNew()
{
	if (m_templateList.IsEmpty())
	{
		TRACE(traceAppMsg, 0, "Error: no document templates registered with CWinApp.\n");
		AfxMessageBox(AFX_IDP_FAILED_TO_CREATE_DOC);
		return;
	}

	if (AfxGetMainWnd() && ((CMainFrame*)AfxGetMainWnd())->MDIGetActive())
	{
		CDocument * pActiveDoc = ((CMainFrame*)AfxGetMainWnd())->MDIGetActive()->GetActiveDocument();
		if (pActiveDoc)
		{
			CDocTemplate* pActDocT = pActiveDoc->GetDocTemplate();
			if (pActDocT)
			{
				pActDocT->OpenDocumentFile(NULL);
				//ActivateFrame(pActDocT);
			}
		}
	}
}

void CDocManagerEx::OnFileOpen()
{
	CDocManager::OnFileOpen();
	// if returns NULL, the user has already been alerted
}