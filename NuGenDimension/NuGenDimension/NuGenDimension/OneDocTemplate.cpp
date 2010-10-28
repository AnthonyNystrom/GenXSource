#include "stdafx.h"
#include "OneDocTemplate.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

IMPLEMENT_DYNAMIC(COneDocTemplate, CEGMultiDocTemplate)

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

COneDocTemplate::COneDocTemplate(UINT nIDResource, CRuntimeClass* pDocClass,
	CRuntimeClass* pFrameClass, CRuntimeClass* pViewClass)
	: CEGMultiDocTemplate(nIDResource, pDocClass, pFrameClass, pViewClass)
{
}

COneDocTemplate::~COneDocTemplate()
{
}

/////////////////////////////////////////////////////////////////////////////
// COneDocTemplate document management (a list of currently open documents)

void 
COneDocTemplate::AddDocument(CDocument* pDoc)
{
	ASSERT( m_docList.GetCount() == 0 ); // one at a time please
	
	CEGMultiDocTemplate::AddDocument(pDoc);
}

/////////////////////////////////////////////////////////////////////////////
// COneDocTemplate commands

CDocument* 
COneDocTemplate::OpenDocumentFile(LPCTSTR lpszPathName, BOOL bMakeVisible)
{
	CDocument* pDocument = NULL;
	CFrameWnd* pFrame = NULL;
	BOOL bCreated = FALSE;      // => doc and frame created
	BOOL bWasModified = FALSE;
	
	if( m_docList.GetCount() != 0 )
	{
		// already have a document - reinit it
		POSITION pos = GetFirstDocPosition();
		pDocument = GetNextDoc(pos);
		if (!pDocument->SaveModified())
			return NULL;        // leave the original one

		// assume 1 frame for the document
		pos = pDocument->GetFirstViewPosition();
		ASSERT( pos ); // at least 1 view must be exists
		CView* pView = pDocument->GetNextView(pos);
		ASSERT_VALID(pView);
		ASSERT(::IsWindow(pView->m_hWnd));
		pFrame = pView->GetParentFrame();
		ASSERT(pFrame != NULL);
		ASSERT_KINDOF(CFrameWnd, pFrame);
		ASSERT_VALID(pFrame);
	}
	else
	{
		// create a new document
		pDocument = CreateNewDocument();
		ASSERT(pFrame == NULL);     // will be created below
		bCreated = TRUE;
	};

	if (pDocument == NULL)
	{
		TRACE0("CDocTemplate::CreateNewDocument returned NULL.\n");
		AfxMessageBox(AFX_IDP_FAILED_TO_CREATE_DOC);
		return NULL;
	}
	ASSERT_VALID(pDocument);

	if (pFrame == NULL)
	{
		ASSERT(bCreated);

		// create frame - set as main document frame
		BOOL bAutoDelete = pDocument->m_bAutoDelete;
		pDocument->m_bAutoDelete = FALSE;
					// don't destroy if something goes wrong
		pFrame = CreateNewFrame(pDocument, NULL);
		pDocument->m_bAutoDelete = bAutoDelete;
		if (pFrame == NULL)
		{
			AfxMessageBox(AFX_IDP_FAILED_TO_CREATE_DOC);
			delete pDocument;       // explicit delete on error
			return NULL;
		}
		ASSERT_VALID(pFrame);
	};
	
	if (lpszPathName == NULL)
	{
		// create a new document - with default document name
		SetDefaultTitle(pDocument);

		// avoid creating temporary compound file when starting up invisible
		if (!bMakeVisible)
			pDocument->m_bEmbedded = TRUE;

		if (!pDocument->OnNewDocument())
		{
			// user has been alerted to what failed in OnNewDocument
			TRACE0("CDocument::OnNewDocument returned FALSE.\n");
			if (bCreated)
			{
				pFrame->DestroyWindow(); // will destroy document
			};
			return NULL;
		}

		// it worked, now bump untitled count
		//m_nUntitledCount++; // no need to count untitled docs!
	}
	else
	{
		// open an existing document
		CWaitCursor wait;

		bWasModified = pDocument->IsModified();
		pDocument->SetModifiedFlag(FALSE);  // not dirty for open

		if (!pDocument->OnOpenDocument(lpszPathName))
		{
			// user has be alerted to what failed in OnOpenDocument
			TRACE0("CDocument::OnOpenDocument returned FALSE.\n");
			if (bCreated)
			{
				pFrame->DestroyWindow();    // will destroy document
			}
			else if (!pDocument->IsModified())
			{
				// original document is untouched
				pDocument->SetModifiedFlag(bWasModified);
			}
			else
			{
				// we corrupted the original document
				SetDefaultTitle(pDocument);

				if (!pDocument->OnNewDocument())
				{
					TRACE0("Error: OnNewDocument failed after trying to open a document - trying to continue.\n");
					// assume we can continue
				}
			}
			return NULL;        // open failed
		}
		pDocument->SetPathName(lpszPathName);
	}

	InitialUpdateFrame(pFrame, pDocument, bMakeVisible);
	return pDocument;
}

void 
COneDocTemplate::SetDefaultTitle(CDocument* pDocument)
{
	CString strDocName;
	if (!GetDocString(strDocName, CDocTemplate::docName) ||
		strDocName.IsEmpty())
	{
		// use generic 'untitled'
		VERIFY(strDocName.LoadString(AFX_IDS_UNTITLED));
	}
	pDocument->SetTitle(strDocName);
}

/////////////////////////////////////////////////////////////////////////////
// COneDocTemplate diagnostics

#ifdef _DEBUG
void 
COneDocTemplate::AssertValid() const
{
	CEGMultiDocTemplate::AssertValid();

	ASSERT( m_docList.GetCount() <= 1 );
}
#endif //_DEBUG