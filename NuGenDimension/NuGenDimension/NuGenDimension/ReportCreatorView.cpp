/* ==========================================================================
	Class :			CReportCreatorView

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportCreatorView" derives from "CView" and is the main 
					view class of the application.

	Description :	The member "m_editor" is the actual editor, and covers 
					all of the client area together with the rulers.

	Usage :			Created by MFC

   ========================================================================*/

#include "stdafx.h"
#include "NuGenDimension.h"

#include "ReportCreatorDoc.h"
#include "ReportCreatorView.h"
#include "ReportEditor/EditorSettingsDialog.h"

#include "ReportEditor/ReportEntityBox.h"
#include "ReportEditor/ReportEntityLine.h"
#include "ReportEditor/ReportEntityLabel.h"
#include "ReportEditor/ReportEntityPicture.h"

#include "ReportEditor/ReportEntitySettings.h"
#include "ReportEditor/UnitConversion.h"
#include ".\reportcreatorview.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define RULER_SIZE	24

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorView

IMPLEMENT_DYNCREATE(CReportCreatorView, CView)

BEGIN_MESSAGE_MAP(CReportCreatorView, CView)
	//{{AFX_MSG_MAP(CReportCreatorView)
	ON_WM_ERASEBKGND()
	ON_WM_SIZE()
/*	ON_COMMAND(ID_BUTTON_SETTINGS, OnButtonSettings)*/
	ON_COMMAND(ID_REPORT_GRID, OnButtonGrid)
	ON_UPDATE_COMMAND_UI(ID_REPORT_GRID, OnUpdateButtonGrid)
/*	ON_COMMAND(ID_BUTTON_MARGIN, OnButtonMargin)
	ON_UPDATE_COMMAND_UI(ID_BUTTON_MARGIN, OnUpdateButtonMargin)*/
	ON_COMMAND(ID_REPORT_SNAP_TO_GRID, OnButtonSnap)
	ON_UPDATE_COMMAND_UI(ID_REPORT_SNAP_TO_GRID, OnUpdateButtonSnap)
	ON_COMMAND(ID_REPORT_RECT, OnButtonAddBox)
	ON_COMMAND(ID_REPORT_LABEL, OnButtonAddLabel)
	ON_COMMAND(ID_REPORT_LINE, OnButtonAddLine)
	ON_COMMAND(ID_REPORT_COPY, OnEditCopy)
	ON_UPDATE_COMMAND_UI(ID_REPORT_COPY, OnUpdateEditCopy)
	ON_COMMAND(ID_REPORT_CUT, OnEditCut)
	ON_UPDATE_COMMAND_UI(ID_REPORT_CUT, OnUpdateEditCut)
	ON_COMMAND(ID_REPORT_PASTE, OnEditPaste)
	ON_UPDATE_COMMAND_UI(ID_REPORT_PASTE, OnUpdateEditPaste)
	ON_COMMAND(ID_REPORT_UNDO, OnEditUndo)
	ON_UPDATE_COMMAND_UI(ID_REPORT_UNDO, OnUpdateEditUndo)
	ON_COMMAND(ID_REPORT_ZOOM_IN, OnZoomIn)
	ON_COMMAND(ID_REPORT_ZOOM_OUT, OnZoomOut)
	ON_COMMAND(ID_REPORT_ALIGN_BOTTOM, OnButtonAlignBottom)
	ON_UPDATE_COMMAND_UI(ID_REPORT_ALIGN_BOTTOM, OnUpdateButtonAlignBottom)
	ON_COMMAND(ID_REPORT_ALIGN_LEFT, OnButtonAlignLeft)
	ON_UPDATE_COMMAND_UI(ID_REPORT_ALIGN_LEFT, OnUpdateButtonAlignLeft)
	ON_COMMAND(ID_REPORT_ALIGN_RIGHT, OnButtonAlignRight)
	ON_UPDATE_COMMAND_UI(ID_REPORT_ALIGN_RIGHT, OnUpdateButtonAlignRight)
	ON_COMMAND(ID_REPORT_ALIGN_TOP, OnButtonAlignTop)
	ON_UPDATE_COMMAND_UI(ID_REPORT_ALIGN_TOP, OnUpdateButtonAlignTop)
	ON_COMMAND(ID_REPORT_ALIGN_SIZES, OnButtonSameSize)
	ON_UPDATE_COMMAND_UI(ID_REPORT_ALIGN_SIZES, OnUpdateButtonSameSize)
	/*ON_COMMAND(ID_BUTTON_ZOOM_TO_FIT, OnButtonZoomToFit)
	ON_COMMAND(ID_BUTTON_PROPERTIES, OnButtonProperties)*/
	ON_COMMAND(ID_REPORT_PICTURE, OnButtonAddPicture)
	ON_WM_DESTROY()
	/*ON_COMMAND(ID_DELETE, OnDelete)
	ON_COMMAND(ID_INSERT, OnInsert)*/
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CView::OnFilePrintPreview)

	ON_REGISTERED_MESSAGE(UWM_HSCROLL, OnEditorHScroll)
	ON_REGISTERED_MESSAGE(UWM_VSCROLL, OnEditorVScroll)
	ON_REGISTERED_MESSAGE(UWM_ZOOM, OnEditorZoom)
	ON_REGISTERED_MESSAGE(UWM_MOUSE, OnEditorMouse)
	ON_REGISTERED_MESSAGE(UWM_MEASUREMENTS, OnRulerMeasurements)

	ON_COMMAND(ID_REPORT_ADD_PAGE, OnReportAddPage)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorView construction/destruction

CReportCreatorView::CReportCreatorView()
/* ============================================================
	Function :		CReportCreatorView::CReportCreatorView
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	m_screenResolutionX = 0;
	m_current_editor = -1;

	m_pages_preview_dlg = NULL;


}

CReportCreatorView::~CReportCreatorView()
/* ============================================================
	Function :		CReportCreatorView::~CReportCreatorView
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

BOOL CReportCreatorView::PreCreateWindow(CREATESTRUCT& cs)
/* ============================================================
	Function :		CReportCreatorView::PreCreateWindow
	Description :	Called befor the window is created
	Access :		Public

	Return :		BOOL				-	Not interested
	Parameters :	CREATESTRUCT& cs	-	Not interested
					
	Usage :			Called from MFC

   ============================================================*/
{
	return CView::PreCreateWindow(cs);
}

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorView drawing

void CReportCreatorView::OnDraw(CDC* pDC)
/* ============================================================
	Function :		CReportCreatorView::OnDraw
	Description :	Draws the view
	Access :		Public

	Return :		void
	Parameters :	CDC* pDC	-	"CDC" to draw to
					
	Usage :			Called from MFC

   ============================================================*/
{
	CReportCreatorDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);

/*	if( pDC->IsPrinting() )
	{

		COLORREF col = m_editors[m_current_editor].editor->GetBackgroundColor();

		// Print zoom is the difference between screen- 
		// and printer resolution.
		double zoom = pDC->GetDeviceCaps( LOGPIXELSX ) / m_screenResolutionX;

		CRect rect( 0,0, 
			round( static_cast< double >( m_editors[m_current_editor].editor->GetVirtualSize().cx ) * zoom ), 
			round( static_cast< double >( m_editors[m_current_editor].editor->GetVirtualSize().cy ) * zoom ) );

		m_editors[m_current_editor].editor->SetRedraw( FALSE );
		m_editors[m_current_editor].editor->SetBackgroundColor( RGB( 255, 255, 255 ) );
		m_editors[m_current_editor].editor->Print( pDC, rect, zoom );
		m_editors[m_current_editor].editor->SetBackgroundColor( col );
		m_editors[m_current_editor].editor->SetRedraw( TRUE );

	}*/


	if( pDC->IsPrinting() )
	{

		// Print zoom is the difference between screen- 
		// and printer resolution.
		/*	double zoom = pDC->GetDeviceCaps( LOGPIXELSX ) / m_screenResolutionX;

		size_t sz = m_editors.size();
		int shftX = 0;
		int shftY = 0;
		for (size_t i=0;i<sz;i++)
		{

		COLORREF col = m_editors[i]->GetBackgroundColor();

		CRect rect( shftX,shftY, 
		shftX+round( static_cast< double >( m_editors[i]->GetVirtualSize().cx ) * zoom ), 
		shftY+round( static_cast< double >( m_editors[i]->GetVirtualSize().cy ) * zoom ) );

		m_editors[i]->SetRedraw( FALSE );
		m_editors[i]->SetBackgroundColor( RGB( 255, 255, 255 ) );
		m_editors[i]->Print( pDC, rect, zoom );
		m_editors[i]->SetBackgroundColor( col );
		m_editors[i]->SetRedraw( TRUE );

		shftX+=round( static_cast< double >( m_editors[i]->GetVirtualSize().cx ) * zoom );
		shftY+=round( static_cast< double >( m_editors[i]->GetVirtualSize().cy)  * zoom) ;
		}
		*/
	}
	else
	{
		CRect rct;
		GetClientRect(rct);
		pDC->FillSolidRect(rct,::GetSysColor(COLOR_3DSHADOW));
	}


}

static void SetPrinterMode(CDC* pDC,int Mode)
{
	if(Mode !=DMORIENT_LANDSCAPE && Mode != DMORIENT_PORTRAIT)
	{
		ASSERT(0);
		return;
	}
	PRINTDLG* pPrintDlg = new PRINTDLG;
	AfxGetApp()->GetPrinterDeviceDefaults(pPrintDlg);
	DEVMODE* lpDevMode = (DEVMODE*)::GlobalLock(pPrintDlg->hDevMode);		
	lpDevMode->dmOrientation = (short)Mode;
	pDC->ResetDC(lpDevMode);	
	::GlobalUnlock(pPrintDlg->hDevMode);	
	delete pPrintDlg;
}


void CReportCreatorView::OnPrepareDC(CDC* pDC, CPrintInfo* pInfo)
{
	CView::OnPrepareDC(pDC, pInfo);
	if (pDC->IsPrinting())
		SetPrinterMode(pDC,
		GetDocument()->GetData(pInfo->m_nCurPage-1)->GetPrinterMode());
}

void CReportCreatorView::OnPrint(CDC* pDC, CPrintInfo* pInfo)
{
	if( pDC->IsPrinting() )
	{

		// Print zoom is the difference between screen- 
		// and printer resolution.
		double zoom = pDC->GetDeviceCaps( LOGPIXELSX ) / m_screenResolutionX;


		{

			COLORREF col = m_editors[pInfo->m_nCurPage-1].editor->GetBackgroundColor();

			CRect rect( 0,0, 
				round( static_cast< double >( m_editors[pInfo->m_nCurPage-1].editor->GetVirtualSize().cx ) * zoom ), 
				round( static_cast< double >( m_editors[pInfo->m_nCurPage-1].editor->GetVirtualSize().cy ) * zoom ) );

			m_editors[pInfo->m_nCurPage-1].editor->SetRedraw( FALSE );
			m_editors[pInfo->m_nCurPage-1].editor->SetBackgroundColor( RGB( 255, 255, 255 ) );
			m_editors[pInfo->m_nCurPage-1].editor->Print( pDC, /*rect*/pInfo->m_rectDraw, zoom );
			m_editors[pInfo->m_nCurPage-1].editor->SetBackgroundColor( col );
			m_editors[pInfo->m_nCurPage-1].editor->SetRedraw( TRUE );

		}

	}
}

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorView printing

BOOL CReportCreatorView::OnPreparePrinting(CPrintInfo* pInfo)
/* ============================================================
	Function :		CReportCreatorView::OnPreparePrinting
	Description :	Called before a printout
	Access :		Public

	Return :		BOOL				-	Not interested
	Parameters :	CPrintInfo* pInfo	-	Not interested	
					
	Usage :			Called from MFC

   ============================================================*/
{
	pInfo->SetMaxPage(m_editors.size());
	return DoPreparePrinting(pInfo);
}

void CReportCreatorView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
/* ============================================================
	Function :		CReportCreatorView::OnBeginPrinting
	Description :	Called before a printout
	Access :		Public

	Return :		void
	Parameters :	CDC*		-	Not interested
					CPrintInfo*	-	Not interested
					
	Usage :			Called from MFC

   ============================================================*/
{
}

void CReportCreatorView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
/* ============================================================
	Function :		CReportCreatorView::OnEndPrinting
	Description :	Called after a printout
	Access :		Public

	Return :		void
	Parameters :	CDC*		-	Not interested
					CPrintInfo*	-	Not interested
					
	Usage :			Called from MFC

   ============================================================*/
{
	SetCurrentPage(m_current_editor);
}

/////////////////////////////////////////////////////////////////////////////
// CReportCreatorView diagnostics

#ifdef _DEBUG
void CReportCreatorView::AssertValid() const
/* ============================================================
	Function :		CReportCreatorView::AssertValid
	Description :	Checks object validity
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Debug function

   ============================================================*/
{
	CView::AssertValid();
}

void CReportCreatorView::Dump(CDumpContext& dc) const
/* ============================================================
	Function :		CReportCreatorView::Dump
	Description :	Dumps object data
	Access :		Public

	Return :		void
	Parameters :	CDumpContext& dc	-	Dump context
					
	Usage :			Debug function

   ============================================================*/
{
	CView::Dump(dc);
}

CReportCreatorDoc* CReportCreatorView::GetDocument()
/* ============================================================
	Function :		CReportCreatorView::GetDocument
	Description :	Returns a pointer to the current document
	Access :		Public

	Return :		CReportCreatorDoc*	-	Current document
	Parameters :	none

	Usage :			Call to get a pointer to the view document.

   ============================================================*/
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CReportCreatorDoc)));
	return (CReportCreatorDoc*)m_pDocument;
}
#endif //_DEBUG


void CReportCreatorView::AddPage(CDiagramEntityContainer* cont, bool vertiacal, bool repaint/*=true*/)
{
	//if( !m_editors[m_current_editor].editor->m_hWnd )
	//{
	int restraintMode = RESTRAINT_MARGIN;//theApp.GetProfileInt( _T( "Application" ), _T( "Restraint" ), 0 );
	int left = 0;//theApp.GetProfileInt( _T( "Application" ), _T( "Left" ), 0 );
	int right = 0;//theApp.GetProfileInt( _T( "Application" ), _T( "Right" ), 0 );
	int top = 0;//theApp.GetProfileInt( _T( "Application" ), _T( "Top" ), 0 );
	int bottom = 0;//theApp.GetProfileInt( _T( "Application" ), _T( "Bottom" ), 0 );

	int width = -1;//theApp.GetProfileInt( _T( "Application" ), _T( "Width" ), -1 );
	int height = -1;//theApp.GetProfileInt( _T( "Application" ), _T( "Height" ), -1 );

	COLORREF colorBg = RGB( 255, 255, 255 );//( COLORREF ) theApp.GetProfileInt( _T( "Application" ), _T( "ColorBg" ), RGB( 255, 255, 255 ) );
	COLORREF colorGrid = RGB( 222, 222, 222 );//( COLORREF ) theApp.GetProfileInt( _T( "Application" ), _T( "ColorGrid" ), RGB( 222, 222, 222 ) );
	COLORREF colorMargin = RGB( 0, 0, 255 );//( COLORREF ) theApp.GetProfileInt( _T( "Application" ), _T( "ColorMargin" ), RGB( 0, 0, 255 ) );

	BOOL margin = TRUE;//( BOOL ) theApp.GetProfileInt( _T( "Application" ), _T( "Margin" ), FALSE );
	BOOL grid = TRUE;//( BOOL ) theApp.GetProfileInt( _T( "Application" ), _T( "Grid" ), FALSE );
	BOOL snap = TRUE;//( BOOL ) theApp.GetProfileInt( _T( "Application" ), _T( "Snap" ), FALSE );
	int measurements = 0;//theApp.GetProfileInt( _T( "Application" ), _T( "Units" ), 0 );

	CReportEntitySettings::GetRESInstance()->SetMeasurementUnits( measurements );

	double zoom = 0.50;//static_cast< double >( theApp.GetProfileInt( _T( "Application" ), _T( "ZoomLevel" ), 100 ) ) / 100;
	double zoomfactor = 0.2;//static_cast< double >( theApp.GetProfileInt( _T( "Application" ), _T( "ZoomFactor" ), 1 ) ) / 100;

	CRect	rect;
	GetClientRect( rect );
	rect.left = RULER_SIZE;
	rect.top = RULER_SIZE;
	CReportEditor* newEditor = new CReportEditor;
	int printer_mode = (vertiacal)?DMORIENT_PORTRAIT:DMORIENT_LANDSCAPE;
	CDiagramEntityContainer* Doc_data = (cont)?cont: 
					GetDocument()->CreateNewDiagramEntityContainer(NULL,printer_mode) ;
	if (!Doc_data)
	{
		ASSERT(Doc_data);
		delete newEditor;
		return;
	}
	newEditor->Create( WS_CHILD | WS_VISIBLE, rect, this,Doc_data);

	CHorzRuler*    newHorzRuler = new CHorzRuler;
	CVertRuler*    newVertRuler = new CVertRuler;
	CCornerBox*    newCornerBox = new CCornerBox;
	
	rect.top = 0;
	rect.bottom = RULER_SIZE;
	newHorzRuler->Create( NULL, NULL, WS_CHILD | WS_VISIBLE, rect, this, 100 );

	GetClientRect( rect );
	rect.right = RULER_SIZE;
	rect.top = RULER_SIZE;
	newVertRuler->Create( NULL, NULL, WS_CHILD | WS_VISIBLE, rect, this, 101 );

	rect.left = 0;
	rect.top = 0;
	rect.right = RULER_SIZE;
	rect.bottom = RULER_SIZE;
	newCornerBox->Create( NULL, NULL, WS_CHILD | WS_VISIBLE, rect, this, 102 );

	newHorzRuler->SetMeasurements( measurements );
	newVertRuler->SetMeasurements( measurements );
	newCornerBox->SetMeasurements( measurements );

	CClientDC dc( this );
	m_screenResolutionX = dc.GetDeviceCaps( LOGPIXELSX );
	CUnitConversion::Init( m_screenResolutionX );

	int step = m_screenResolutionX / 6;
	int gridSize = step;//theApp.GetProfileInt( _T( "Application" ), _T( "GridSize" ), step );

	if( width == -1 )
	{
		CPrintDialog	printer( TRUE, PD_RETURNDC );
		printer.GetDefaults();
		HDC	hdc = printer.GetPrinterDC();
		if (cont==NULL)  // добавляем пустую вертикальную или горизонтальную
		{
						if( hdc )
						{

							double diff = ( double ) GetDeviceCaps( hdc, LOGPIXELSX ) / ( double ) m_screenResolutionX;
							int horzSize = ::GetDeviceCaps( hdc, PHYSICALWIDTH );
							int vertSize = ::GetDeviceCaps( hdc, PHYSICALHEIGHT );

							if(Doc_data->GetPrinterMode()==DMORIENT_LANDSCAPE)
							{
								int ttt = horzSize;
								horzSize = vertSize;
								vertSize = ttt;
							}

							int leftMarg = ::GetDeviceCaps( hdc, PHYSICALOFFSETX );
							int topMarg = ::GetDeviceCaps( hdc, PHYSICALOFFSETY );

							int horzPrintable = ::GetDeviceCaps( hdc, HORZRES );
							int vertPrintable = ::GetDeviceCaps( hdc, VERTRES );

							if(Doc_data->GetPrinterMode()==DMORIENT_LANDSCAPE)
							{
								int ttt = horzPrintable;
								horzPrintable = vertPrintable;
								vertPrintable = ttt;
							}

							int rightMarg = horzSize - ( horzPrintable + leftMarg );
							int bottomMarg = vertSize - ( vertPrintable + topMarg );

							left = ( int ) ( ( double ) leftMarg / diff );
							top = ( int ) ( ( double ) topMarg / diff );
							right = ( int ) ( ( double ) rightMarg / diff );
							bottom = ( int ) ( ( double ) bottomMarg / diff );
							width = ( int ) ( ( double ) horzSize / diff );
							height = ( int ) ( ( double ) vertSize / diff );

							::DeleteDC( hdc );
						}
						else
						{
							// No default printer installed
							width = 8 * m_screenResolutionX;
							height = (int)(1.4142135623730950488016887242097*(double)width);

							if(Doc_data->GetPrinterMode()==DMORIENT_LANDSCAPE)
							{
								int ttt = width;
								width = height;
								height = ttt;
							}

							left = 0;
							top = 0;
							right = 0;
							bottom = 0;
						}
						CSize sss(width,height);
						Doc_data->SetPageSizes(&sss);
		}
		else  // читаем страницу из файла - размеры тоже из файла, а отступы - из настроек принтера
		{
						if( hdc )
						{
							double diff = ( double ) GetDeviceCaps( hdc, LOGPIXELSX ) / ( double ) m_screenResolutionX;
							int horzSize = cont->GetPageSizes()->cx;
							int vertSize = cont->GetPageSizes()->cy;

							int leftMarg = ::GetDeviceCaps( hdc, PHYSICALOFFSETX );
							int topMarg = ::GetDeviceCaps( hdc, PHYSICALOFFSETY );

							int horzPrintable = ::GetDeviceCaps( hdc, HORZRES );
							int vertPrintable = ::GetDeviceCaps( hdc, VERTRES );

							if(Doc_data->GetPrinterMode()==DMORIENT_LANDSCAPE)
							{
								int ttt = horzPrintable;
								horzPrintable = vertPrintable;
								vertPrintable = ttt;
							}

							/*int rightMarg = horzSize*diff - ( horzPrintable + leftMarg );
							int bottomMarg = vertSize*diff - ( vertPrintable + topMarg );#WARNING*/
							int rightMarg = horzSize*(int)diff - ( horzPrintable + leftMarg );
							int bottomMarg = vertSize*(int)diff - ( vertPrintable + topMarg );

							left = ( int ) ( ( double ) leftMarg / diff );
							top = ( int ) ( ( double ) topMarg / diff );
							right = ( int ) ( ( double ) rightMarg / diff );
							bottom = ( int ) ( ( double ) bottomMarg / diff );
							width = horzSize;//( int ) ( ( double ) horzSize / diff );
							height = vertSize;//( int ) ( ( double ) vertSize / diff );

							::DeleteDC( hdc );

						}
						else
						{
							// No default printer installed
							width = cont->GetPageSizes()->cx;//*m_screenResolutionX;
							height = cont->GetPageSizes()->cy;//*m_screenResolutionX;

							left = 0;
							top = 0;
							right = 0;
							bottom = 0;
						}		
		}
	}

	newEditor->SetZoom( zoom );

	newEditor->SetZoomFactor( zoomfactor );
	newEditor->SetVirtualSize( CSize( width, height ) );
	newEditor->SetMargins( left, top, right, bottom );
	newEditor->SetSnapToGrid( snap );
	newEditor->ShowGrid( grid );
	newEditor->ShowMargin( margin );
	newEditor->SetRestraints( restraintMode );
	newEditor->SetBackgroundColor( colorBg );
	newEditor->SetGridColor( colorGrid );
	newEditor->SetMarginColor( colorMargin );

	newEditor->SetScrollWheelMode( WHEEL_SCROLL );

	newEditor->SetGridSize( CSize( gridSize, gridSize ) );

	newEditor->SetZoomMin( .05 );
	newEditor->SetZoomMax( 4 );
	newEditor->SetModified( FALSE );

	newEditor->SetThumbnailer(m_pages_preview_dlg);

	MyEditor tmpEd;
	tmpEd.editor = newEditor;
	tmpEd.Horz_Ruler = newHorzRuler;
	tmpEd.Vert_Ruler = newVertRuler;
	tmpEd.Corner_Box = newCornerBox;

	size_t sz = m_editors.size();
	for (size_t i=0;i<sz;i++)
	{
		m_editors[i].editor->ShowWindow(SW_HIDE);
		m_editors[i].Horz_Ruler->ShowWindow(SW_HIDE);
		m_editors[i].Vert_Ruler->ShowWindow(SW_HIDE);
		m_editors[i].Corner_Box->ShowWindow(SW_HIDE);
	}

	m_editors.push_back(tmpEd);
	if (m_pages_preview_dlg && repaint)
		m_pages_preview_dlg->Invalidate();

	//newEditor->StartDrawingObject( new CReportEntityLine );
}

int  CReportCreatorView::GetCurrentPage()
{
	return m_current_editor;
}

void CReportCreatorView::SetCurrentPage(int cE)
{
	m_current_editor = cE;

	size_t sz = m_editors.size();
	for (size_t i=0;i<sz;i++)
		if (i==m_current_editor)
		{
			m_editors[i].editor->ShowWindow(SW_SHOW);
			m_editors[i].Horz_Ruler->ShowWindow(SW_SHOW);
			m_editors[i].Vert_Ruler->ShowWindow(SW_SHOW);
			m_editors[i].Corner_Box->ShowWindow(SW_SHOW);
		}
		else
		{
			m_editors[i].editor->ShowWindow(SW_HIDE);
			m_editors[i].Horz_Ruler->ShowWindow(SW_HIDE);
			m_editors[i].Vert_Ruler->ShowWindow(SW_HIDE);
			m_editors[i].Corner_Box->ShowWindow(SW_HIDE);
		}

}


/////////////////////////////////////////////////////////////////////////////
// CReportCreatorView message handlers
#include "ReportChildFrame.h"
void CReportCreatorView::OnInitialUpdate() 
/* ============================================================
	Function :		CReportCreatorView::OnInitialUpdate
	Description :	Called before the view is displayed
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. We create the editor.

   ============================================================*/
{
	CView::OnInitialUpdate();
	
	CReportChildFrame* cfr = static_cast<CReportChildFrame*>(GetParentFrame());
	if (cfr)
	{
		SetPagesPreviewDlg(&cfr->m_preview_panel);
		cfr->m_preview_panel.SetView(this);
	}

	size_t sz = m_editors.size();
	for (size_t i=0;i<sz;i++)
	{
		m_editors[i].editor->DestroyWindow();
		m_editors[i].Horz_Ruler->DestroyWindow();
		m_editors[i].Vert_Ruler->DestroyWindow();
		m_editors[i].Corner_Box->DestroyWindow();
		delete m_editors[i].editor;
		delete m_editors[i].Horz_Ruler;
		delete m_editors[i].Vert_Ruler;
		delete m_editors[i].Corner_Box;
	}
	m_editors.clear();
	m_current_editor = -1;

	size_t pCnt = GetDocument()->GetPagesCount();
	if (pCnt>0)
	{
		for (size_t i=0;i<pCnt;i++)
			AddPage(GetDocument()->GetData(i),false,false);
	}
	else
		AddPage(NULL,false,false);
	
    SetCurrentPage(0);

	for (size_t jj=0;jj<m_editors.size();jj++)
		m_editors[jj].editor->RepaintFromThumbnail();

	if (m_pages_preview_dlg)
		m_pages_preview_dlg->Invalidate();
/*	if( !m_editors[m_current_editor].editor->m_hWnd )
	{
		int restraintMode =RESTRAINT_MARGIN;// theApp.GetProfileInt( _T( "Application" ), _T( "Restraint" ), 0 );
		int left = theApp.GetProfileInt( _T( "Application" ), _T( "Left" ), 0 );
		int right = theApp.GetProfileInt( _T( "Application" ), _T( "Right" ), 0 );
		int top = theApp.GetProfileInt( _T( "Application" ), _T( "Top" ), 0 );
		int bottom = theApp.GetProfileInt( _T( "Application" ), _T( "Bottom" ), 0 );

		int width = theApp.GetProfileInt( _T( "Application" ), _T( "Width" ), -1 );
		int height = theApp.GetProfileInt( _T( "Application" ), _T( "Height" ), -1 );

		COLORREF colorBg = ( COLORREF ) theApp.GetProfileInt( _T( "Application" ), _T( "ColorBg" ), RGB( 255, 255, 255 ) );
		COLORREF colorGrid = ( COLORREF ) theApp.GetProfileInt( _T( "Application" ), _T( "ColorGrid" ), RGB( 222, 222, 222 ) );
		COLORREF colorMargin = ( COLORREF ) theApp.GetProfileInt( _T( "Application" ), _T( "ColorMargin" ), RGB( 0, 0, 255 ) );

		BOOL margin = TRUE;//( BOOL ) theApp.GetProfileInt( _T( "Application" ), _T( "Margin" ), FALSE );
		BOOL grid = TRUE;//( BOOL ) theApp.GetProfileInt( _T( "Application" ), _T( "Grid" ), FALSE );
		BOOL snap = ( BOOL ) theApp.GetProfileInt( _T( "Application" ), _T( "Snap" ), FALSE );
		int measurements = theApp.GetProfileInt( _T( "Application" ), _T( "Units" ), 0 );

		CReportEntitySettings::GetRESInstance()->SetMeasurementUnits( measurements );

		double zoom = static_cast< double >( theApp.GetProfileInt( _T( "Application" ), _T( "ZoomLevel" ), 100 ) ) / 100;
		double zoomfactor = static_cast< double >( theApp.GetProfileInt( _T( "Application" ), _T( "ZoomFactor" ), 1 ) ) / 100;

		CRect	rect;
		GetClientRect( rect );
		rect.left = RULER_SIZE;
		rect.top = RULER_SIZE;
		m_editors[m_current_editor].editor->Create( WS_CHILD | WS_VISIBLE, rect, this, GetDocument()->GetData() );

		rect.top = 0;
		rect.bottom = RULER_SIZE;
		m_horzRuler.Create( NULL, NULL, WS_CHILD | WS_VISIBLE, rect, this, 100 );

		GetClientRect( rect );
		rect.right = RULER_SIZE;
		rect.top = RULER_SIZE;
		m_vertRuler.Create( NULL, NULL, WS_CHILD | WS_VISIBLE, rect, this, 101 );

		rect.left = 0;
		rect.top = 0;
		rect.right = RULER_SIZE;
		rect.bottom = RULER_SIZE;
		m_cornerBox.Create( NULL, NULL, WS_CHILD | WS_VISIBLE, rect, this, 102 );

		m_horzRuler.SetMeasurements( measurements );
		m_vertRuler.SetMeasurements( measurements );
		m_cornerBox.SetMeasurements( measurements );

		CClientDC dc( this );
		m_screenResolutionX = dc.GetDeviceCaps( LOGPIXELSX );
		CUnitConversion::Init( m_screenResolutionX );

		int step = m_screenResolutionX / 6;
		int gridSize = theApp.GetProfileInt( _T( "Application" ), _T( "GridSize" ), step );

		if( width == -1 )
		{
			CPrintDialog	printer( TRUE, PD_RETURNDC );
			printer.GetDefaults();
			HDC	hdc = printer.GetPrinterDC();
			if( hdc )
			{

				double diff = ( double ) GetDeviceCaps( hdc, LOGPIXELSX ) / ( double ) m_screenResolutionX;
				int horzSize = ::GetDeviceCaps( hdc, PHYSICALWIDTH );
				int vertSize = ::GetDeviceCaps( hdc, PHYSICALHEIGHT );

				int leftMarg = ::GetDeviceCaps( hdc, PHYSICALOFFSETX );
				int topMarg = ::GetDeviceCaps( hdc, PHYSICALOFFSETY );

				int horzPrintable = ::GetDeviceCaps( hdc, HORZRES );
				int vertPrintable = ::GetDeviceCaps( hdc, VERTRES );

				int rightMarg = horzSize - ( horzPrintable + leftMarg );
				int bottomMarg = vertSize - ( vertPrintable + topMarg );

				left = ( int ) ( ( double ) leftMarg / diff );
				top = ( int ) ( ( double ) topMarg / diff );
				right = ( int ) ( ( double ) rightMarg / diff );
				bottom = ( int ) ( ( double ) bottomMarg / diff );
				width = ( int ) ( ( double ) horzSize / diff );
				height = ( int ) ( ( double ) vertSize / diff );
	
				::DeleteDC( hdc );

			}
			else
			{
				// No default printer installed
				width = 8 * m_screenResolutionX;
				height = 11 * m_screenResolutionX;

				left = 0;
				top = 0;
				right = 0;
				bottom = 0;
			}
		}

		m_editors[m_current_editor].editor->SetZoom( zoom );

		m_editors[m_current_editor].editor->SetZoomFactor( zoomfactor );
		m_editors[m_current_editor].editor->SetVirtualSize( CSize( width, height ) );
		m_editors[m_current_editor].editor->SetMargins( left, top, right, bottom );
		m_editors[m_current_editor].editor->SetSnapToGrid( snap );
		m_editors[m_current_editor].editor->ShowGrid( grid );
		m_editors[m_current_editor].editor->ShowMargin( margin );
		m_editors[m_current_editor].editor->SetRestraints( restraintMode );
		m_editors[m_current_editor].editor->SetBackgroundColor( colorBg );
		m_editors[m_current_editor].editor->SetGridColor( colorGrid );
		m_editors[m_current_editor].editor->SetMarginColor( colorMargin );

		m_editors[m_current_editor].editor->SetScrollWheelMode( WHEEL_ZOOM );

		m_editors[m_current_editor].editor->SetGridSize( CSize( gridSize, gridSize ) );

		m_editors[m_current_editor].editor->SetZoomMin( .05 );
		m_editors[m_current_editor].editor->SetZoomMax( 4 );
		m_editors[m_current_editor].editor->SetModified( FALSE );

		GetDocument()->OnNewDocument();

   	}
	else
		m_editors[m_current_editor].editor->Clear();*/
	
}

BOOL CReportCreatorView::OnEraseBkgnd( CDC* /*pDC*/ ) 
/* ============================================================
	Function :		CReportCreatorView::OnEraseBkgnd
	Description :	Handler for the "WM_ERASEBKND"-message.
	Access :		Protected

	Return :		BOOL		-	Always "TRUE"
	Parameters :	CDC* pDC	-	"CDC" to erase
					
	Usage :			Called from MFC.

   ============================================================*/
{

	return TRUE;

}

void CReportCreatorView::OnSize(UINT nType, int cx, int cy) 
/* ============================================================
	Function :		CReportCreatorView::OnSize
	Description :	Handler for the "WM_SIZE" message.
	Access :		Protected

	Return :		void
	Parameters :	UINT nType	-	Not interested
					int cx		-	New x-size
					int cy		-	New y-size
					
	Usage :			Called from MFC.

   ============================================================*/
{

	CView::OnSize(nType, cx, cy);

	size_t sz = m_editors.size();
	if (sz>0)
	{
		for (size_t i=0;i<sz;i++)
		{
			if( m_editors[i].editor->m_hWnd )
			{
				m_editors[i].editor->MoveWindow( RULER_SIZE, RULER_SIZE, cx - RULER_SIZE, cy - RULER_SIZE, i==m_current_editor );
				m_editors[i].Horz_Ruler->MoveWindow( RULER_SIZE, 0, cx, RULER_SIZE , i==m_current_editor );
				m_editors[i].Vert_Ruler->MoveWindow( 0, RULER_SIZE, RULER_SIZE, cy , i==m_current_editor );
			}
		}
	}


}

void CReportCreatorView::OnButtonSettings() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonSettings
	Description :	Handler for the toolbar button Settings
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	CEditorSettingsDialog	dlg;

	dlg.m_colorBg = m_editors[m_current_editor].editor->GetBackgroundColor();
	dlg.m_colorGrid = m_editors[m_current_editor].editor->GetGridColor();
	dlg.m_colorMargin = m_editors[m_current_editor].editor->GetMarginColor();

	dlg.m_width = m_editors[m_current_editor].editor->GetVirtualSize().cx;
	dlg.m_height = m_editors[m_current_editor].editor->GetVirtualSize().cy;

	dlg.m_margins = m_editors[m_current_editor].editor->IsMarginVisible();
	dlg.m_snap = m_editors[m_current_editor].editor->GetSnapToGrid();
	dlg.m_grid = m_editors[m_current_editor].editor->IsGridVisible();

	m_editors[m_current_editor].editor->GetMargins( dlg.m_left, dlg.m_top, dlg.m_right, dlg.m_bottom );
	int restraintMode = m_editors[m_current_editor].editor->GetRestraints();
	if( restraintMode == RESTRAINT_NONE )
		dlg.m_restraint = 2;
	if( restraintMode == RESTRAINT_VIRTUAL )
		dlg.m_restraint = 1;
	if( restraintMode == RESTRAINT_MARGIN )
		dlg.m_restraint = 0;

	dlg.m_zoom = round( m_editors[m_current_editor].editor->GetZoomFactor() * 100 );
	dlg.m_zoomlevel = round( m_editors[m_current_editor].editor->GetZoom() * 100 );
	dlg.m_gridsize = m_editors[m_current_editor].editor->GetGridSize().cx;

	dlg.m_measurements = CReportEntitySettings::GetRESInstance()->GetMeasurementUnits();

	if( dlg.DoModal() )
	{

		m_editors[m_current_editor].editor->SetVirtualSize( CSize( dlg.m_width, dlg.m_height ) );
		m_editors[m_current_editor].editor->SetBackgroundColor( dlg.m_colorBg );
		m_editors[m_current_editor].editor->SetGridColor( dlg.m_colorGrid );
		m_editors[m_current_editor].editor->SetMarginColor( dlg.m_colorMargin );
		m_editors[m_current_editor].editor->ShowMargin( dlg.m_margins );
		m_editors[m_current_editor].editor->SetSnapToGrid( dlg.m_snap );
		m_editors[m_current_editor].editor->ShowGrid( dlg.m_grid );
		m_editors[m_current_editor].editor->SetGridSize( CSize( dlg.m_gridsize, dlg.m_gridsize ) );
		m_editors[m_current_editor].editor->SetMargins( dlg.m_left, dlg.m_top, dlg.m_right, dlg.m_bottom );

		if( dlg.m_restraint == 2 )
			restraintMode = RESTRAINT_NONE;
		if( dlg.m_restraint == 1 )
			restraintMode = RESTRAINT_VIRTUAL;
		if( dlg.m_restraint == 0 )
			restraintMode = RESTRAINT_MARGIN;

		m_editors[m_current_editor].editor->SetRestraints( restraintMode );
		m_editors[m_current_editor].editor->SetZoomFactor( static_cast< double >( dlg.m_zoom ) / 100 );

		m_editors[m_current_editor].editor->SetZoom( static_cast< double >( dlg.m_zoomlevel ) / 100 );

		CReportEntitySettings::GetRESInstance()->SetMeasurementUnits( dlg.m_measurements );
		
		m_editors[m_current_editor].Horz_Ruler->SetMeasurements( dlg.m_measurements );
		m_editors[m_current_editor].Vert_Ruler->SetMeasurements( dlg.m_measurements );
		m_editors[m_current_editor].Corner_Box->SetMeasurements( dlg.m_measurements );

		SaveSettings();

		m_editors[m_current_editor].editor->RedrawWindow();

	}

}

void CReportCreatorView::SaveSettings()
/* ============================================================
	Function :		CReportCreatorView::SaveSettings
	Description :	Saves the editor settings to the registry.
	Access :		Private

	Return :		void
	Parameters :	none

	Usage :			Call to save the editor settings to the 
					registry.

   ============================================================*/
{

	int left;
	int right;
	int top;
	int bottom;
	int measurements;

	m_editors[m_current_editor].editor->GetMargins( left, top, right, bottom );
	measurements = CReportEntitySettings::GetRESInstance()->GetMeasurementUnits();

	theApp.WriteProfileInt( _T( "Application" ), _T( "Restraint" ), m_editors[m_current_editor].editor->GetRestraints() );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Left" ), left );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Right" ), right );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Top" ), top );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Bottom" ), bottom );

	theApp.WriteProfileInt( _T( "Application" ), _T( "Width" ), m_editors[m_current_editor].editor->GetVirtualSize().cx );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Height" ), m_editors[m_current_editor].editor->GetVirtualSize().cy );
	theApp.WriteProfileInt( _T( "Application" ), _T( "ColorBg" ), m_editors[m_current_editor].editor->GetBackgroundColor() );
	theApp.WriteProfileInt( _T( "Application" ), _T( "ColorGrid" ), m_editors[m_current_editor].editor->GetGridColor() );
	theApp.WriteProfileInt( _T( "Application" ), _T( "ColorMargin" ), m_editors[m_current_editor].editor->GetMarginColor() );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Margin" ), m_editors[m_current_editor].editor->IsMarginVisible() );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Grid" ), m_editors[m_current_editor].editor->IsGridVisible() );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Snap" ), m_editors[m_current_editor].editor->GetSnapToGrid() );
	theApp.WriteProfileInt( _T( "Application" ), _T( "ZoomFactor" ), static_cast< int >( m_editors[m_current_editor].editor->GetZoomFactor() * 100.0 ));
	theApp.WriteProfileInt( _T( "Application" ), _T( "ZoomLevel" ), static_cast< int >( m_editors[m_current_editor].editor->GetZoom() * 100.0 ) );
	theApp.WriteProfileInt( _T( "Application" ), _T( "Units" ), measurements );

}

void CReportCreatorView::OnButtonGrid() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonGrid
	Description :	Handler for the toolbar button Grid
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	m_editors[m_current_editor].editor->ShowGrid( !m_editors[m_current_editor].editor->IsGridVisible() );

}

void CReportCreatorView::OnUpdateButtonGrid(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonGrid
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	pCmdUI->SetCheck( m_editors[m_current_editor].editor->IsGridVisible() );

}

void CReportCreatorView::OnButtonMargin() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonMargin
	Description :	Handler for the toolbar button Margin
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	m_editors[m_current_editor].editor->ShowMargin( !m_editors[m_current_editor].editor->IsMarginVisible() );

}

void CReportCreatorView::OnUpdateButtonMargin(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonMargin
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{
	
	pCmdUI->SetCheck( m_editors[m_current_editor].editor->IsMarginVisible() );

}

void CReportCreatorView::OnButtonSnap() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonSnap
	Description :	Handler for the toolbar button Snap
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	m_editors[m_current_editor].editor->SetSnapToGrid( !m_editors[m_current_editor].editor->GetSnapToGrid() );

}

void CReportCreatorView::OnUpdateButtonSnap(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonSnap
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	pCmdUI->SetCheck( m_editors[m_current_editor].editor->GetSnapToGrid() );

}

void CReportCreatorView::OnButtonAddBox() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAddBox
	Description :	Handler for the toolbar button AddBox
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	m_editors[m_current_editor].editor->StartDrawingObject( new CReportEntityBox );

}

void CReportCreatorView::OnButtonAddLabel() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAddLabel
	Description :	Handler for the toolbar button AddLabel
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	m_editors[m_current_editor].editor->StartDrawingObject( new CReportEntityLabel );

}

void CReportCreatorView::OnButtonAddLine() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAddLine
	Description :	Handler for the toolbar button AddLine
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	m_editors[m_current_editor].editor->StartDrawingObject( new CReportEntityLine );

}

LRESULT CReportCreatorView::OnEditorHScroll( WPARAM pos, LPARAM )
/* ============================================================
	Function :		CReportCreatorView::OnEditorHScroll
	Description :	Handler for the registered message "UWM_HSCROLL"
	Access :		Protected

	Return :		LRESULT		-	Not used
	Parameters :	WPARAM pos	-	New scroll position
					LPARAM	-	
					
	Usage :			Called from MFC. The message is sent when 
					the editor is scrolled horizontally.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].Horz_Ruler->SetStartPos( pos );
	return 0;

}

LRESULT CReportCreatorView::OnEditorVScroll( WPARAM pos, LPARAM )
/* ============================================================
	Function :		CReportCreatorView::OnEditorVScroll
	Description :	Handler for the registered message "UWM_VSCROLL"
	Access :		Protected

	Return :		LRESULT		-	Not used
	Parameters :	WPARAM pos	-	New scroll position
					LPARAM		-	Not used
					
	Usage :			Called from MFC. The message is sent when 
					the editor is scrolled vertically.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].Vert_Ruler->SetStartPos( pos );
	return 0;

}

LRESULT CReportCreatorView::OnEditorZoom( WPARAM z, LPARAM )
/* ============================================================
	Function :		CReportCreatorView::OnEditorZoom
	Description :	Handler for the registered message "UWM_ZOOM"
	Access :		Protected

	Return :		LRESULT		-	Not used
	Parameters :	WPARAM	z	-	New zoom value (*100)
					LPARAM		-	Not used
					
	Usage :			Called from MFC. The message is sent when 
					the editor is zoomed.

   ============================================================*/
{
	if (m_current_editor>-1)
	{
		double zoom = static_cast<double>( z ) / 100;

		if (zoom>0.001 && zoom<100)
		{
			m_editors[m_current_editor].Horz_Ruler->SetZoom( zoom );
			m_editors[m_current_editor].Vert_Ruler->SetZoom( zoom );
		}
	}

	return 0;

}

LRESULT CReportCreatorView::OnEditorMouse( WPARAM z, LPARAM )
/* ============================================================
	Function :		CReportCreatorView::OnEditorMouse
	Description :	Handler for the registered message "UWM_MOUSE"
	Access :		Protected

	Return :		LRESULT		-	Not used
	Parameters :	WPARAM	z	-	New mouse position
					LPARAM		-	Not used
					
	Usage :			Called from MFC. The message is sent when 
					the mouse is moved in the editor.

   ============================================================*/
{

	if (m_current_editor>-1)
	{
		
		CPoint* pt = reinterpret_cast< CPoint* >( z );
		if( pt )
		{
			m_editors[m_current_editor].Horz_Ruler->SetPointerPos( pt->x );
			m_editors[m_current_editor].Vert_Ruler->SetPointerPos( pt->y );
		}
		else
		{
			m_editors[m_current_editor].Horz_Ruler->SetPointerPos( -1 );
			m_editors[m_current_editor].Vert_Ruler->SetPointerPos( -1 );
		}

	}
	return 0;

}

LRESULT CReportCreatorView::OnRulerMeasurements( WPARAM measurements, LPARAM )
/* ============================================================
	Function :		CReportCreatorView::OnRulerMeasurements
	Description :	Handler for the registered message 
					"UWM_MEASUREMENTS" - sent from the corner 
					control when the measurement settings are 
					changed.
	Access :		Protected

	Return :		LRESULT				-	Not used
	Parameters :	WPARAM measurements	-	New measurement units.
					LPARAM				-	Not used
					
	Usage :			Called from MFC

   ============================================================*/
{

	if (m_current_editor>-1)
	{
	m_editors[m_current_editor].Horz_Ruler->SetMeasurements( measurements );
	m_editors[m_current_editor].Vert_Ruler->SetMeasurements( measurements );
	m_editors[m_current_editor].Corner_Box->SetMeasurements( measurements );
	}

	CReportEntitySettings::GetRESInstance()->SetMeasurementUnits( measurements );

	return 0;

}

void CReportCreatorView::OnEditCopy() 
/* ============================================================
	Function :		CReportCreatorView::OnEditCopy
	Description :	Handler for the Copy command
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{
	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->Copy();

}

void CReportCreatorView::OnUpdateEditCopy(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateEditCopy
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{
	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->UpdateCopy( pCmdUI );

}

void CReportCreatorView::OnEditCut() 
/* ============================================================
	Function :		CReportCreatorView::OnEditCut
	Description :	Handler for the Cut command
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->Cut();

}

void CReportCreatorView::OnUpdateEditCut(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateEditCut
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->UpdateCut( pCmdUI );

}

void CReportCreatorView::OnEditPaste() 
/* ============================================================
	Function :		CReportCreatorView::OnEditPaste
	Description :	Handler for the Paste command
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->Paste();

}

void CReportCreatorView::OnUpdateEditPaste(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateEditPaste
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->UpdatePaste( pCmdUI );

}

void CReportCreatorView::OnEditUndo() 
/* ============================================================
	Function :		CReportCreatorView::OnEditUndo
	Description :	Handler for the Undo command
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->Undo();

}

void CReportCreatorView::OnUpdateEditUndo(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateEditUndo
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->UpdateUndo( pCmdUI );

}

void CReportCreatorView::OnZoomIn() 
/* ============================================================
	Function :		CReportCreatorView::OnZoomIn
	Description :	Handler for the zoom in accelerator
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->SetZoom( m_editors[m_current_editor].editor->GetZoom() + m_editors[m_current_editor].editor->GetZoomFactor() );

}

void CReportCreatorView::OnZoomOut() 
/* ============================================================
	Function :		CReportCreatorView::OnZoomOut
	Description :	Handler for the zoom out accelerator
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->SetZoom( m_editors[m_current_editor].editor->GetZoom() - m_editors[m_current_editor].editor->GetZoomFactor() );

}

void CReportCreatorView::OnButtonAlignBottom() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAlignBottom
	Description :	Handler for the toolbar button AlignBottom
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->BottomAlignSelected();

}

void CReportCreatorView::OnUpdateButtonAlignBottom(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonAlignBottom
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		pCmdUI->Enable( ( m_editors[m_current_editor].editor->GetSelectCount() > 1 ) );
	else
		pCmdUI->Enable(FALSE);

}

void CReportCreatorView::OnButtonAlignLeft() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAlignLeft
	Description :	Handler for the toolbar button AlignLeft
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->LeftAlignSelected();

}

void CReportCreatorView::OnUpdateButtonAlignLeft(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonAlignLeft
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		pCmdUI->Enable( ( m_editors[m_current_editor].editor->GetSelectCount() > 1 ) );
	else
		pCmdUI->Enable(FALSE);

}

void CReportCreatorView::OnButtonAlignRight() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAlignRight
	Description :	Handler for the toolbar button AlignRight
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->RightAlignSelected();

}

void CReportCreatorView::OnUpdateButtonAlignRight(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonAlignRight
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		pCmdUI->Enable( ( m_editors[m_current_editor].editor->GetSelectCount() > 1 ) );
	else
		pCmdUI->Enable(FALSE);

}

void CReportCreatorView::OnButtonAlignTop() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAlignTop
	Description :	Handler for the toolbar button AlignTop
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->TopAlignSelected();

}

void CReportCreatorView::OnUpdateButtonAlignTop(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonAlignTop
	Description :	Update-handler for the UI-element
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		pCmdUI->Enable( ( m_editors[m_current_editor].editor->GetSelectCount() > 1 ) );
	else
		pCmdUI->Enable(FALSE);

}

void CReportCreatorView::OnButtonSameSize() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonSameSize
	Description :	Handler for the toolbar button SameSize
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->MakeSameSizeSelected();

}

void CReportCreatorView::OnUpdateButtonSameSize(CCmdUI* pCmdUI) 
/* ============================================================
	Function :		CReportCreatorView::OnUpdateButtonSameSize
	Description :	Update-handler for the toolbar button 
					SameSize.
	Access :		Protected

	Return :		void
	Parameters :	CCmdUI* pCmdUI	-	"CCmdUI" to update.
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		pCmdUI->Enable( ( m_editors[m_current_editor].editor->GetSelectCount() > 1 ) );
	else
		pCmdUI->Enable(FALSE);

}

void CReportCreatorView::OnButtonZoomToFit() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonZoomToFit
	Description :	Handler for the toolbar button ZoomToFit
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->ZoomToFitScreen();

}

void CReportCreatorView::OnButtonProperties() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonProperties
	Description :	Handler for the toolbar button Properties
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->ShowProperties();

}

void CReportCreatorView::OnButtonAddPicture() 
/* ============================================================
	Function :		CReportCreatorView::OnButtonAddPicture
	Description :	Handler for the toolbar button AddPicture
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->StartDrawingObject( new CReportEntityPicture );
	
}


void CReportCreatorView::OnDestroy() 
/* ============================================================
	Function :		CReportCreatorView::OnDestroy
	Description :	Handler for the "WM_DESTROY"-message.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. Saves the editor settings.

   ============================================================*/
{

	//SaveSettings();
	size_t sz = m_editors.size();
	for (size_t i=0;i<sz;i++)
		{
			m_editors[i].editor->DestroyWindow();
			m_editors[i].Horz_Ruler->DestroyWindow();
			m_editors[i].Vert_Ruler->DestroyWindow();
			m_editors[i].Corner_Box->DestroyWindow();
			delete m_editors[i].editor;
			delete m_editors[i].Horz_Ruler;
			delete m_editors[i].Vert_Ruler;
			delete m_editors[i].Corner_Box;
		}
	m_editors.clear();

	CView::OnDestroy();
	
}

void CReportCreatorView::OnDelete() 
/* ============================================================
	Function :		CReportCreatorView::OnDelete
	Description :	Handler for the "ID_DELETE" accelerator
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->DeleteAllSelected();
	
}

void CReportCreatorView::OnInsert() 
/* ============================================================
	Function :		CReportCreatorView::OnInsert
	Description :	Handler for the "ID_INSERT" accelerator
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{

	if (m_current_editor>-1)
		m_editors[m_current_editor].editor->Duplicate();
	
}

#include "Dialogs//ChoiseDraftTemplateDlg.h"
#include "Tools//DraftTemplateLoader.h"
void CReportCreatorView::OnReportAddPage()
{
	CString apP;
	theApp.GetAppPath(apP);
	CChoiseDraftTemplateDlg ddd(apP);
	if (ddd.DoModal()==IDOK)
	{
		switch(ddd.GetSelectTemplateType()) 
		{
		case 0:
			AddPage(NULL,false);
			break;
		case 1:
			AddPage(NULL,true);
			break;
		case 2:
			{
				CDiagramEntityContainer* newCont = CDraftTemplateLoader::LoadDraftTemplate(ddd.GetSelectedTemplatePath());
				GetDocument()->m_objs_pages.push_back(newCont);
				AddPage(newCont,false);
			}
			break;
		default:
			ASSERT(0);
			AddPage(NULL,false);
			break;
		}		
	}
	else
	{
		
	}
	
	SetCurrentPage(GetPagesCount()-1);

}

#include "..//CxImage//xImaWMF.h"

void  CReportCreatorView::InsertPictureToPage(int pag,HENHMETAFILE pict,
											  float leftPerc/*=0.0*/, 
											  float topPerc/*=0.0*/,
											  float WidthPerc/*=100.0*/, 
											  float HeightPerc/*=100.0*/)
{
	//if (!pict || pag<0 || pag>=m_editors.size())#WARNING
	if (!pict || pag<0 || pag>=(int)m_editors.size())
		return;

	CReportEntityPicture* pictEnt = new CReportEntityPicture(pict);
	GetDocument()->GetData(pag)->Add(pictEnt);
	int margL;
	int margT;
	int margR;
	int margB;
	m_editors[pag].editor->GetMargins(margL,margT,margR,margB);
	CSize vSz = m_editors[pag].editor->GetVirtualSize();
	/*CRect picRec(margL+(vSz.cx-margL)*leftPerc/100.0,
				margT+(vSz.cy-margT)*topPerc/100.0,
				margL+(vSz.cx-margL)*leftPerc/100.0+(vSz.cx-margL-margR)*WidthPerc/100.0,
				margT+(vSz.cy-margT)*topPerc/100.0+(vSz.cy-margT-margB)*HeightPerc/100.0);#WARNING*/
	CRect picRec(margL+(vSz.cx-margL)*(int)(leftPerc/100.0),
				margT+(vSz.cy-margT)*(int)(topPerc/100.0),
				margL+(vSz.cx-margL)*(int)(leftPerc/100.0)+(vSz.cx-margL-margR)*(int)(WidthPerc/100.0),
				margT+(vSz.cy-margT)*(int)(topPerc/100.0)+(vSz.cy-margT-margB)*(int)(HeightPerc/100.0));

	picRec = FitFirstRectToSecond(pictEnt->GetPictureSizes(),picRec);
	pictEnt->SetRect(picRec);
}
