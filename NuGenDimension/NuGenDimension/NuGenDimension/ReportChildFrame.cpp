#include "stdafx.h"
#include "NuGenDimension.h"

#include "ReportChildFrame.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CReportChildFrame

IMPLEMENT_DYNCREATE(CReportChildFrame, CMDIChildWnd)

BEGIN_MESSAGE_MAP(CReportChildFrame, CMDIChildWnd)
	//{{AFX_MSG_MAP(CReportChildFrame)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
	ON_WM_MDIACTIVATE()
	ON_WM_CREATE()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportChildFrame construction/destruction

CReportChildFrame::CReportChildFrame()
{
}

CReportChildFrame::~CReportChildFrame()
{
}

BOOL CReportChildFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CMDIChildWnd::PreCreateWindow(cs) )
		return FALSE;

	//cs.style = WS_CHILD | WS_VISIBLE /*| FWS_ADDTOTITLE | WS_THICKFRAME */| WS_MAXIMIZE;

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CReportChildFrame diagnostics

#ifdef _DEBUG
void CReportChildFrame::AssertValid() const
{
	CMDIChildWnd::AssertValid();
}

void CReportChildFrame::Dump(CDumpContext& dc) const
{
	CMDIChildWnd::Dump(dc);
}

#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CReportChildFrame message handlers

void CReportChildFrame::OnMDIActivate(BOOL bActivate, CWnd* pActivateWnd, CWnd* pDeactivateWnd)
{
	CMDIChildWnd::OnMDIActivate(bActivate, pActivateWnd, pDeactivateWnd);

	if (bActivate)
	{
		//ShowWindow(SW_SHOWMAXIMIZED);
	}
}

int CReportChildFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CMDIChildWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	EnableDocking(CBRS_ALIGN_ANY);

	m_wndPreviewPanel_Container.Create(this, 100);

	m_preview_panel.Create(CReportPagesPreviewDlg::IDD, &m_wndPreviewPanel_Container);
	
	CString tttS;
	tttS.LoadString(IDS_PAGES);
	m_wndPreviewPanel_Container.AddFolderRes(tttS, IDR_MAINFRAME);
	m_wndPreviewPanel_Container.AddFolderItem("");
	m_wndPreviewPanel_Container.AddSubItem(m_preview_panel.GetSafeHwnd(), true);

	return 0;
}
