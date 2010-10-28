/* ==========================================================================
	Class :			CReportCreatorDoc

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportCreatorDoc" derives from "CDocument" and is the 
					document class of the application.

	Description :	The class is automatically created by MSVC++

	Usage :			Created from MFC

   ========================================================================*/

#include "stdafx.h"
#include "NuGenDimension.h"
#include "ReportCreatorDoc.h"

#include "ReportEditor/ReportControlFactory.h"
#include "ReportEditor/UnitConversion.h"
#include "ReportEditor/ReportEntityLine.h"
#include "ReportEditor/ReportEntityBox.h"
#include "ReportEditor/ReportEntityLabel.h"
#include "ReportEditor/ReportEntityPicture.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#pragma warning( disable : 4706 )

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorDoc

IMPLEMENT_DYNCREATE(CReportCreatorDoc, CDocument)

BEGIN_MESSAGE_MAP(CReportCreatorDoc, CDocument)
	//{{AFX_MSG_MAP(CReportCreatorDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorDoc construction/destruction

CReportCreatorDoc::CReportCreatorDoc()
/* ============================================================
	Function :		CReportCreatorDoc::CReportCreatorDoc
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
//	strcpy(m_header.signature,"SG_REPORT_V1.0");#OBSOLETE
	strcpy_s(m_header.signature, sizeof (m_header.signature), "SG_REPORT_V1.0");


	m_header.version_number = 1;
}

CReportCreatorDoc::~CReportCreatorDoc()
/* ============================================================
	Function :		CReportCreatorDoc::~CReportCreatorDoc
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
	size_t sz = m_objs_pages.size();
	for (size_t i=0;i<sz;i++)
	{
		m_objs_pages[i]->Clear();
		delete m_objs_pages[i];
	}
	m_objs_pages.clear();

}

BOOL CReportCreatorDoc::IsModified() 
{
	return FALSE;
}

BOOL CReportCreatorDoc::OnNewDocument()
/* ============================================================
	Function :		CReportCreatorDoc::OnNewDocument
	Description :	Called as soon as a new document is created/loaded.
	Access :		Public

	Return :		BOOL	-	"TRUE" if loaded/created ok.
	Parameters :	none

	Usage :			Overridden to clear the data container.

   ============================================================*/
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	size_t sz = m_objs_pages.size();
	for (size_t i=0;i<sz;i++)
	{
		m_objs_pages[i]->Clear();
		delete m_objs_pages[i];
	}
	m_objs_pages.clear();

	return TRUE;
}

BOOL CReportCreatorDoc::OnOpenDocument(LPCTSTR lpszPathName)
{
	size_t sz = m_objs_pages.size();
	for (size_t i=0;i<sz;i++)
	{
		m_objs_pages[i]->Clear();
		delete m_objs_pages[i];
	}
	m_objs_pages.clear();

	if (!CDocument::OnOpenDocument(lpszPathName))
		return FALSE;
	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CReportCreatorDoc serialization

void CReportCreatorDoc::Serialize(CArchive& ar)
/* ============================================================
	Function :		CReportCreatorDoc::Serialize
	Description :	Called when a documnet is saved/loaded from 
					file.
	Access :		Public

	Return :		void
	Parameters :	CArchive& ar	-	Archive to save/load 
										from.
					
	Usage :			Overridden to save/load to the data 
					container.

   ============================================================*/
{
	// Saving and loading to/from a text file
	typedef struct
	{
		int a_f;
		int b_f;
		int c_f;
		int d_f;
		int e_f;
		double A_f;
		double B_f;
		double C_f;
		double D_f;
		double E_f;
	} DOC_RESERVE_FIELDS;

	DOC_RESERVE_FIELDS doc_reserve;
	memset(&doc_reserve,0,sizeof(DOC_RESERVE_FIELDS));
	if (ar.IsStoring())
	{
		ar.Write(&m_header,sizeof(REPORT_FILE_HEADER));//REPORT_FILE_HEADER save
		// SUB_HEADER_VER_1 save
		ar.Write(&CUnitConversion::s_resolution,sizeof(int));
		ar.Write(&doc_reserve,sizeof(DOC_RESERVE_FIELDS));
		size_t pcnt = m_objs_pages.size();
		ar.Write(&pcnt,sizeof(size_t));// save count of pages
		size_t i;
		for (i=0;i<pcnt;i++)
		{
			int tmpPrM = m_objs_pages[i]->GetPrinterMode();
			ar.Write(&tmpPrM,sizeof(int));// save page orient
			const CSize*  pS = m_objs_pages[i]->GetPageSizes();
			ASSERT(pS->cx>0);
			ASSERT(pS->cy>0);
			ar.Write(&(pS->cx),sizeof(INT));
			ar.Write(&(pS->cy),sizeof(INT));
		}
		// objects
		for (i=0;i<pcnt;i++)
		{
			int objCnt = m_objs_pages[i]->GetSize();
			for (int j=0;j<objCnt;j++)
			{
				CDiagramEntity* de = m_objs_pages[i]->GetAt(j);
				DIAGRAM_OBJECT_TYPE dot = de->GetEntityType();
				switch(dot) 
				{
				case DIAGRAM_LINE:
				case DIAGRAM_RECT:
				case DIAGRAM_LABEL:
				case DIAGRAM_PICTURE:
					ar.Write(&i, sizeof(size_t)); // save object page
					ar.Write(&dot,sizeof(int)); // save object type
					de->Serialize(ar);
					break;
				default:
					ASSERT(0);
					break;
				}
			}
		}
	}
	else
	{
		ar.Read(&m_header,sizeof(REPORT_FILE_HEADER));	
		// SUB_HEADER_VER_1 read
		ar.Read(&CUnitConversion::s_resolution,sizeof(int));
		ar.Read(&doc_reserve,sizeof(DOC_RESERVE_FIELDS));
		size_t pcnt = 0;
		ar.Read(&pcnt,sizeof(size_t));// read count of pages
		size_t i;
		for (i=0;i<pcnt;i++)
		{
			int tmpPrM = DMORIENT_PORTRAIT;
			ar.Read(&tmpPrM,sizeof(int));// read page orient
			CSize nSz;
			ar.Read(&(nSz.cx),sizeof(INT));
			ar.Read(&(nSz.cy),sizeof(INT));
			ASSERT(nSz.cx>0);
			ASSERT(nSz.cy>0);
			CreateNewDiagramEntityContainer(&nSz,tmpPrM);
		}
		// objects
		size_t obj_p = 0;
		int    obj_t = 0;
		while (ar.Read(&obj_p,sizeof(size_t))) 
		{
			if (obj_p>=m_objs_pages.size())
			{
				ASSERT(0);
				return;
			}
			ar.Read(&obj_t,sizeof(int));
			switch(obj_t) 
			{
			case DIAGRAM_LINE:
				{
					CReportEntityLine* nL = new CReportEntityLine;
					nL->Serialize(ar);
					m_objs_pages[obj_p]->Add(nL);
				}
				break;
			case DIAGRAM_RECT:
				{
					CReportEntityBox* bx = new CReportEntityBox;
					bx->Serialize(ar);
					m_objs_pages[obj_p]->Add(bx);
				}
				break;
			case DIAGRAM_LABEL:
				{
					CReportEntityLabel* lb = new CReportEntityLabel;
					lb->Serialize(ar);
					m_objs_pages[obj_p]->Add(lb);
				}
				break;
			case DIAGRAM_PICTURE:
				{
					CReportEntityPicture* pic = new CReportEntityPicture;
					pic->Serialize(ar);
					m_objs_pages[obj_p]->Add(pic);
				}
				break;
			default:
				ASSERT(0);
				break;
			}
		}
		
	}
}

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorDoc diagnostics

#ifdef _DEBUG
void CReportCreatorDoc::AssertValid() const
/* ============================================================
	Function :		CReportCreatorDoc::AssertValid
	Description :	Checks the object validity
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Debug function

   ============================================================*/
{
	CDocument::AssertValid();
}

void CReportCreatorDoc::Dump(CDumpContext& dc) const
/* ============================================================
	Function :		CReportCreatorDoc::Dump
	Description :	Dumps object data
	Access :		Public

	Return :		void
	Parameters :	CDumpContext& dc	-	Dump context
					
	Usage :			Debug function

   ============================================================*/
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorDoc commands

BOOL CReportCreatorDoc::SaveModified() 
/* ============================================================
	Function :		CReportCreatorDoc::SaveModified
	Description :	Called to see if the Data changed, save?-
					dialog should be displayed.
	Access :		Public

	Return :		BOOL	-	Not interested
	Parameters :	none

	Usage :			Overridden to set the dirty-flag from the 
					container.

   ============================================================*/
{

	BOOL mdf = FALSE;
	size_t sz = m_objs_pages.size();
	for (size_t i=0;i<sz;i++)
	{
		if (m_objs_pages[i]->IsModified())
			mdf = TRUE;
	}
	SetModifiedFlag( mdf );
	return CDocument::SaveModified();


}

CDiagramEntityContainer* CReportCreatorDoc::CreateNewDiagramEntityContainer(const CSize* sizes,int printer_mode)
{
	CDiagramEntityContainer* newCont = new CDiagramEntityContainer(printer_mode,sizes,&( theApp.m_clip ));
	m_objs_pages.push_back(newCont);
	return newCont;
}


CDiagramEntityContainer* CReportCreatorDoc::GetData(int ind)
/* ============================================================
Function :		CReportCreatorDoc::GetData
Description :	Gets a pointer to the data container.
Access :		Public

Return :		CDiagramEntityContainer*	-	Data container
Parameters :	none

Usage :			Call to access the data container.

============================================================*/
{
	if (ind<0 || (unsigned int)ind>=m_objs_pages.size())
		return NULL;
	return m_objs_pages[ind];
}


#pragma warning( default : 4706 )
