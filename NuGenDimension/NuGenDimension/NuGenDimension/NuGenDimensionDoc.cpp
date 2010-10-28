// NuGenDimensionDoc.cpp : implementation of the CNuGenDimensionDoc class
//

#include "stdafx.h"
#include "NuGenDimension.h"

#include "NuGenDimensionDoc.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CNuGenDimensionDoc

IMPLEMENT_DYNCREATE(CNuGenDimensionDoc, CDocument)

BEGIN_MESSAGE_MAP(CNuGenDimensionDoc, CDocument)
  ON_COMMAND(ID_DXF_EXPORT, OnDxfExport)
  ON_COMMAND(ID_DXF_IMPORT, OnDxfImport)
  ON_COMMAND(ID_STL_EXPORT, OnSTLExport)
  ON_COMMAND(ID_STL_IMPORT, OnSTLImport)
END_MESSAGE_MAP()

CNuGenDimensionDoc*  global_3D_document=NULL;

// CNuGenDimensionDoc construction/destruction

CNuGenDimensionDoc::CNuGenDimensionDoc()
{
	global_3D_document = this;
}

CNuGenDimensionDoc::~CNuGenDimensionDoc()
{
	global_3D_document = NULL;
}

BOOL CNuGenDimensionDoc::IsModified()
{
  return (!sgGetScene()->IsUndoStackEmpty());
}

void CNuGenDimensionDoc::SetSceneSetups(const SCENE_SETUPS&  scS)
{
	if (scS.CurrentColor>239)
		m_scene_setups.CurrentColor = 239;
	else
		m_scene_setups.CurrentColor = scS.CurrentColor;

	m_scene_setups.CurrentLineThickness = scS.CurrentLineThickness;
	m_scene_setups.CurrentLineType = scS.CurrentLineType;
	m_scene_setups.CurrentLayer = scS.CurrentLayer;

	if (scS.CurrentPrecision>=PREC_COUNT)
		m_scene_setups.CurrentPrecision = PREC_COUNT-1;
	else
		m_scene_setups.CurrentPrecision = scS.CurrentPrecision;
}

void CNuGenDimensionDoc::GetSceneSetups(SCENE_SETUPS& scS)
{
	memcpy(&scS, &m_scene_setups, sizeof(SCENE_SETUPS));
}
/*
class AAA:public SG_USER_DYNAMIC_DATA
{
	int* m_aaa;
public:
	AAA() {m_aaa=new int[10];}
	~AAA() {delete[] m_aaa;};
	virtual void Finalize()
	{
		if (m_aaa)
		{
			delete[] m_aaa;
			m_aaa = NULL;
			delete this;
		}
	}
};
*/

BOOL CNuGenDimensionDoc::OnNewDocument()
{
  if (!CDocument::OnNewDocument())
    return FALSE;

  sgGetScene()->Clear();

  memset(&m_scene_setups,0,sizeof(SCENE_SETUPS));
  m_scene_setups.CurrentColor = 8;

  CMainFrame*  mnFr = static_cast<CMainFrame*>(theApp.m_pMainWnd);
  mnFr->ResetNames();
  mnFr->UpdateSystemToolbar();

  return TRUE;
}

void CNuGenDimensionDoc::OnCloseDocument()
{
  sgGetScene()->Clear();
  CDocument::OnCloseDocument();
}

BOOL CNuGenDimensionDoc::OnOpenDocument(LPCTSTR lpszPathName)
{
	sgGetScene()->Clear();

  if (!CDocument::OnOpenDocument(lpszPathName))
    return FALSE;
  
  sgFileManager::Open(sgGetScene(),lpszPathName);

  CMainFrame*  mnFr = static_cast<CMainFrame*>(theApp.m_pMainWnd);
  mnFr->ResetNames();
  mnFr->UpdateSystemToolbar();

  return TRUE;
}

BOOL CNuGenDimensionDoc::OnSaveDocument(LPCTSTR lpszPathName)
{
#ifdef NUGEN_RETAIL
	sgFileManager::Save(sgGetScene(),lpszPathName, NULL, 0);
#else
    AfxMessageBox("This feature is not available in DEMO!");
#endif
  return TRUE;
}

// CNuGenDimensionDoc serialization

void CNuGenDimensionDoc::Serialize(CArchive& ar)
{
  /*if (ar.IsStoring())
  {
	  int aaa = sgGetScene()->GetObjectsList()->GetCount();
	  ar.Write(&aaa,sizeof(int));
	  sgCObject* cO = sgGetScene()->GetObjectsList()->GetHead();
	  while (cO) 
	  {
		  unsigned long sz =  0;
		  const void* bbb  = sgFileManager::ObjectToBitArray(cO,sz);
		  ar.Write(&sz,sizeof(unsigned long));
		  ar.Write(bbb,sz);
		  cO = sgGetScene()->GetObjectsList()->GetNext(cO);
	  }
  }
  else
  {
	  int cnt = 0;
	  unsigned long sz = 0;
	  ar.Read(&cnt,sizeof(int));

	  for (int i=0;i<cnt;i++)
	  {	
		  ar.Read(&sz,sizeof(unsigned long));
		  char* bbb  = new char[sz];
		  memset(bbb,0,sz);
		  ar.Read(bbb,sz);
		  sgGetScene()->AttachObject(sgFileManager::BitArrayToObject(bbb,sz));
		  delete [] bbb;
	  }
  }*/
}


// CNuGenDimensionDoc diagnostics

#ifdef _DEBUG
void CNuGenDimensionDoc::AssertValid() const
{
  CDocument::AssertValid();
}

void CNuGenDimensionDoc::Dump(CDumpContext& dc) const
{
  CDocument::Dump(dc);
}
#endif //_DEBUG


// CNuGenDimensionDoc commands

void CNuGenDimensionDoc::OnDxfExport()
{
#ifdef NUGEN_RETAIL
  CString     Path;

  CFileDialog dlg(
    FALSE,
    NULL,               // Open File Dialog
    _T(""),             // Default extension
    OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, // No default filename
    _T("AutoCAD DXF (*.dxf)|*.dxf||"));// Filter string

  if (dlg.DoModal() != IDOK)
    return;
  Path = dlg.GetPathName();
  if (Path.Right(4)!=".dxf")
    Path+=".dxf";

  sgFileManager::ExportDXF(sgGetScene(),Path.GetBuffer());
#else
    AfxMessageBox("This feature is not available in DEMO!");
#endif
}

void CNuGenDimensionDoc::OnDxfImport()
{
  CString     Path;

  CFileDialog dlg(
    TRUE,
    NULL,               // Open File Dialog
    _T("*.dxf"),              // Default extension
    OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, // No default filename
    _T("AutoCAD DXF (*.dxf)|*.dxf||"));// Filter string

  if (dlg.DoModal() != IDOK)
    return;
  Path = dlg.GetPathName();

  GetDocTemplate()->OpenDocumentFile(NULL);

  sgFileManager::ImportDXF(sgGetScene(),Path.GetBuffer());

  POSITION pos = GetFirstViewPosition();
  while (pos != NULL)
  {
    CView* pView = GetNextView(pos);
    pView->OnInitialUpdate();
  }
	//AfxMessageBox("Sorry, its DEMO");
}

void CNuGenDimensionDoc::OnSTLExport()
{
#ifdef NUGEN_RETAIL
	CString     Path;

	CFileDialog dlg(
		FALSE,
		NULL,               // Open File Dialog
		_T(""),             // Default extension
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, // No default filename
		_T("STL (*.stl)|*.stl||"));// Filter string

	if (dlg.DoModal() != IDOK)
		return;
	Path = dlg.GetPathName();
	if (Path.Right(4)!=".stl")
		Path+=".stl";

	sgFileManager::ExportSTL(sgGetScene(),Path.GetBuffer());
#else
    AfxMessageBox("This feature is not available in DEMO!");
#endif
}

void CNuGenDimensionDoc::OnSTLImport()
{
	CString     Path;

	CFileDialog dlg(
		TRUE,
		NULL,               // Open File Dialog
		_T("*.stl"),              // Default extension
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, // No default filename
		_T("STL (*.stl)|*.stl||"));// Filter string

	if (dlg.DoModal() != IDOK)
		return;
	Path = dlg.GetPathName();

	GetDocTemplate()->OpenDocumentFile(NULL);

	sgFileManager::ImportSTL(sgGetScene(),Path.GetBuffer());

	POSITION pos = GetFirstViewPosition();
	while (pos != NULL)
	{
		CView* pView = GetNextView(pos);
		pView->OnInitialUpdate();
	}
	//AfxMessageBox("Sorry, its DEMO");
}
