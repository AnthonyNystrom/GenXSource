/* ==========================================================================
	Class :			CEditorSettingsDialog

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CEditorSettingsDialog" derives from "CDialog" and is a 
					settings dialog for the editor itself.

	Description :	Standard ClassWizard-created class.

	Usage :			Dsiplay to change the editor settings.

   ========================================================================*/

#include "stdafx.h"
#include "..//resource.h"
#include "EditorSettingsDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CEditorSettingsDialog dialog

CEditorSettingsDialog::CEditorSettingsDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CEditorSettingsDialog::IDD, pParent)
/* ============================================================
	Function :		CEditorSettingsDialog::CEditorSettingsDialog
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	CWnd* pParent	-	
					
	Usage :			

   ============================================================*/
{
	//{{AFX_DATA_INIT(CEditorSettingsDialog)
	m_grid = FALSE;
	m_snap = FALSE;
	m_editBottom = _T("");
	m_editHeight = _T("");
	m_editLeft = _T("");
	m_editRight = _T("");
	m_editTop = _T("");
	m_editWidth = _T("");
	m_restraint = 2;
	m_measurements = 0;
	m_margins = FALSE;
	m_zoom = 0;
	m_zoomlevel = 0;
	m_editGridsize = _T("");
	//}}AFX_DATA_INIT

	m_left = 0;
	m_right = 0;
	m_top = 0;
	m_bottom = 0;
	m_width = 0;
	m_height = 0;
	m_gridsize = 0;

	m_currentMeasurement = MEASUREMENT_PIXEL;
	m_colorBg = RGB( 0, 0, 0 );
	m_colorGrid = RGB( 0, 0, 0 );
	m_colorMargin = RGB( 0, 0, 0 );

}


void CEditorSettingsDialog::DoDataExchange(CDataExchange* pDX)
/* ============================================================
	Function :		CEditorSettingsDialog::DoDataExchange
	Description :	MFC data exchange handler.
	Access :		Protected

	Return :		void
	Parameters :	CDataExchange* pDX	-	
					
	Usage :			Called from MFC.

   ============================================================*/
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CEditorSettingsDialog)
	DDX_Control(pDX, IDC_REPORT_EDIT_WIDTH, m_ctrlWidth);
	DDX_Control(pDX, IDC_REPORT_EDIT_TOP, m_ctrlTop);
	DDX_Control(pDX, IDC_REPORT_EDIT_RIGHT, m_ctrlRight);
	DDX_Control(pDX, IDC_REPORT_EDIT_LEFT, m_ctrlLeft);
	DDX_Control(pDX, IDC_REPORT_EDIT_HEIGTH, m_ctrlHeight);
	DDX_Control(pDX, IDC_REPORT_EDIT_GRID_SIZE, m_ctrlGridsize);
	DDX_Control(pDX, IDC_REPORT_EDIT_BOTTOM, m_ctrlBottom);
	DDX_Control(pDX, IDC_REPORT_STATIC_MARGIN, m_ctrlMargin);
	DDX_Control(pDX, IDC_REPORT_STATIC_GRID, m_ctrlGrid);
	DDX_Control(pDX, IDC_REPORT_STATIC_BG, m_ctrlBg);
	DDX_Check(pDX, IDC_REPORT_CHECK_GRID_VISIBLE, m_grid);
	DDX_Check(pDX, IDC_REPORT_CHECK_SNAP, m_snap);
	DDX_Text(pDX, IDC_REPORT_EDIT_BOTTOM, m_editBottom);
	DDX_Text(pDX, IDC_REPORT_EDIT_HEIGTH, m_editHeight);
	DDX_Text(pDX, IDC_REPORT_EDIT_LEFT, m_editLeft);
	DDX_Text(pDX, IDC_REPORT_EDIT_RIGHT, m_editRight);
	DDX_Text(pDX, IDC_REPORT_EDIT_TOP, m_editTop);
	DDX_Text(pDX, IDC_REPORT_EDIT_WIDTH, m_editWidth);
	DDX_Radio(pDX, IDC_REPORT_RADIO_MARGIN, m_restraint);
	DDX_CBIndex(pDX, IDC_REPORT_COMBO_MEASUREMENTS, m_measurements);
	DDX_Check(pDX, IDC_REPORT_CHECK_SHOW_MARGINS, m_margins);
	DDX_Text(pDX, IDC_REPORT_EDIT_ZOOM_STEP, m_zoom);
	DDV_MinMaxUInt(pDX, m_zoom, 1, 100);
	DDX_Text(pDX, IDC_REPORT_EDIT_ZOOM_LEVEL, m_zoomlevel);
	DDV_MinMaxUInt(pDX, m_zoomlevel, 5, 400);
	DDX_Text(pDX, IDC_REPORT_EDIT_GRID_SIZE, m_editGridsize);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CEditorSettingsDialog, CDialog)
	//{{AFX_MSG_MAP(CEditorSettingsDialog)
	ON_BN_CLICKED(IDC_REPORT_BUTTON_COLOR_BG, OnButtonColorBg)
	ON_BN_CLICKED(IDC_REPORT_BUTTON_COLOR_GRID, OnButtonColorGrid)
	ON_BN_CLICKED(IDC_REPORT_BUTTON_COLOR_MARGIN, OnButtonColorMargin)
	ON_BN_CLICKED(IDC_REPORT_BUTTON_DEFAULT, OnButtonDefault)
	ON_CBN_SELCHANGE(IDC_REPORT_COMBO_MEASUREMENTS, OnSelchangeComboMeasurements)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CEditorSettingsDialog message handlers

void CEditorSettingsDialog::OnButtonColorBg() 
/* ============================================================
	Function :		CEditorSettingsDialog::OnButtonColorBg
	Description :	Handler for the dialog button ColorBg
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. The Windows color dialog 
					is displayed.

   ============================================================*/
{
	CColorDialog	dlg( m_colorBg );
	
	if( dlg.DoModal() )
	{
		m_colorBg = dlg.GetColor();
		m_ctrlBg.SetColor( m_colorBg );
	}

}

void CEditorSettingsDialog::OnButtonColorGrid() 
/* ============================================================
	Function :		CEditorSettingsDialog::OnButtonColorGrid
	Description :	Handler for the dialog button ColorGrid
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. The Windows color dialog 
					is displayed.

   ============================================================*/
{
	CColorDialog	dlg( m_colorGrid );
	
	if( dlg.DoModal() )
	{
		m_colorGrid = dlg.GetColor();
		m_ctrlGrid.SetColor( m_colorGrid );
	}

}

void CEditorSettingsDialog::OnButtonColorMargin() 
/* ============================================================
	Function :		CEditorSettingsDialog::OnButtonColorMargin
	Description :	Handler for the dialog button ColorMargin
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. The Windows color dialog 
					is displayed.

   ============================================================*/
{

	CColorDialog	dlg( m_colorMargin );

	if( dlg.DoModal() )
	{
		m_colorMargin = dlg.GetColor();
		m_ctrlMargin.SetColor( m_colorMargin );
	}


}

BOOL CEditorSettingsDialog::OnInitDialog() 
/* ============================================================
	Function :		CEditorSettingsDialog::OnInitDialog
	Description :	Handler for the "WM_INITDIALOG" messag
	Access :		Protected

	Return :		BOOL	-	Always "TRUE"
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{

	CDialog::OnInitDialog();
	
	m_ctrlBg.SetColor( m_colorBg );
	m_ctrlGrid.SetColor( m_colorGrid );
	m_ctrlMargin.SetColor( m_colorMargin );

	CWaitCursor wait;

	CClientDC dc( this );

	m_xres = dc.GetDeviceCaps( LOGPIXELSX );
	m_yres = dc.GetDeviceCaps( LOGPIXELSY );

	m_cmx = m_xres / 2.54;
	m_cmy = m_yres / 2.54;

	m_currentMeasurement = m_measurements;
	SetMeasurementEdits();

	return TRUE;

}

void CEditorSettingsDialog::GetMeasurements()
/* ============================================================
	Function :		CEditorSettingsDialog::GetMeasurements
	Description :	Gets the contents of the modified edit 
					fields converted to pixels.
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Call to get the contents of the edit fields.

   ============================================================*/
{
	
	UpdateData();

	char   *stop;
	double width	= _tcstod( m_editWidth, &stop );
	double height	= _tcstod( m_editHeight, &stop );
	double left		= _tcstod( m_editLeft, &stop );
	double right	= _tcstod( m_editRight, &stop );
	double top		= _tcstod( m_editTop, &stop );
	double bottom	= _tcstod( m_editBottom, &stop );
	double gridsize	= _tcstod( m_editGridsize, &stop );

	switch( m_currentMeasurement )
	{
		case MEASUREMENT_PIXEL:
			if( m_ctrlWidth.GetModify() )
				m_width		= round( width );
			if( m_ctrlHeight.GetModify() )
				m_height	= round( height );
			if( m_ctrlLeft.GetModify() )
				m_left		= round( left );
			if( m_ctrlRight.GetModify() )
				m_right		= round( right );
			if( m_ctrlTop.GetModify() )
				m_top		= round( top );
			if( m_ctrlBottom.GetModify() )
				m_bottom	= round( bottom );
			if( m_ctrlGridsize.GetModify() )
				m_gridsize	= round( gridsize );
			break;

		case MEASUREMENT_INCH:
			if( m_ctrlWidth.GetModify() )
				m_width		= round( width * m_xres );
			if( m_ctrlHeight.GetModify() )
				m_height	= round( height * m_yres );
			if( m_ctrlLeft.GetModify() )
				m_left		= round( left * m_xres );
			if( m_ctrlRight.GetModify() )
				m_right		= round( right * m_xres );
			if( m_ctrlTop.GetModify() )
				m_top		= round( top * m_yres );
			if( m_ctrlBottom.GetModify() )
				m_bottom	= round( bottom * m_yres );
			if( m_ctrlGridsize.GetModify() )
				m_gridsize	= round( gridsize * m_yres );
			break;

		case MEASUREMENT_CENTIMETER:
			if( m_ctrlWidth.GetModify() )
				m_width		= round( width * m_cmx );
			if( m_ctrlHeight.GetModify() )
				m_height	= round( height * m_cmy );
			if( m_ctrlLeft.GetModify() )
				m_left		= round( left * m_cmx );
			if( m_ctrlRight.GetModify() )
				m_right		= round( right * m_cmx );
			if( m_ctrlTop.GetModify() )
				m_top		= round( top * m_cmy );
			if( m_ctrlBottom.GetModify() )
				m_bottom	= round( bottom * m_cmy );
			if( m_ctrlGridsize.GetModify() )
				m_gridsize	= round( gridsize * m_cmy );
			break;
	}

}

void CEditorSettingsDialog::SetMeasurementEdits()
/* ============================================================
	Function :		CEditorSettingsDialog::SetMeasurementEdits
	Description :	Sets the contents of the edit fields
					containing measures.
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Call to update the edit fields when - for 
					example - the measurement type has changed.

   ============================================================*/
{

	double width;
	double height;
	double left;
	double right;
	double top;
	double bottom;
	double gridsize;

	switch( m_currentMeasurement )
	{

		case MEASUREMENT_PIXEL:
			m_editWidth.Format( "%d", m_width );
			m_editHeight.Format( "%d", m_height );
			m_editLeft.Format( "%d", m_left );
			m_editRight.Format( "%d", m_right );
			m_editTop.Format( "%d", m_top );
			m_editBottom.Format( "%d", m_bottom );
			m_editGridsize.Format( "%d", m_gridsize );
			break;

		case MEASUREMENT_INCH:
			width		= static_cast< double >( m_width ) / m_xres;
			height		= static_cast< double >( m_height ) / m_yres;
			left		= static_cast< double >( m_left ) / m_xres;
			right		= static_cast< double >( m_right ) / m_xres;
			top			= static_cast< double >( m_top ) / m_yres;
			bottom		= static_cast< double >( m_bottom ) / m_yres;
			gridsize	= static_cast< double >( m_gridsize ) / m_yres;
			m_editWidth.Format( "%.02f", width );
			m_editHeight.Format( "%.02f", height );
			m_editLeft.Format( "%.02f", left );
			m_editRight.Format( "%.02f", right );
			m_editTop.Format( "%.02f", top );
			m_editBottom.Format( "%.02f", bottom );
			m_editGridsize.Format( "%.02f", gridsize );
			break;

		case MEASUREMENT_CENTIMETER:
			width		= static_cast< double >( m_width ) / m_cmx;
			height		= static_cast< double >( m_height ) / m_cmy;
			left		= static_cast< double >( m_left ) / m_cmx;
			right		= static_cast< double >( m_right ) / m_cmx;
			top			= static_cast< double >( m_top ) / m_cmy;
			bottom		= static_cast< double >( m_bottom ) / m_cmy;
			gridsize	= static_cast< double >( m_gridsize ) / m_cmy;
			m_editWidth.Format( "%.02f", width );
			m_editHeight.Format( "%.02f", height );
			m_editLeft.Format( "%.02f", left );
			m_editRight.Format( "%.02f", right );
			m_editTop.Format( "%.02f", top );
			m_editBottom.Format( "%.02f", bottom );
			m_editGridsize.Format( "%.02f", gridsize );
			break;

	}


	UpdateData( FALSE );
}

void CEditorSettingsDialog::OnSelchangeComboMeasurements() 
/* ============================================================
	Function :		CEditorSettingsDialog::OnSelchangeComboMeasurements
	Description :	Handler for selection changes in the 
					measurement combo.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. Updates the values on 
					screen using the selected measurement.

   ============================================================*/
{

	UpdateData();
	GetMeasurements();
	m_currentMeasurement = m_measurements;
	SetMeasurementEdits();

}

void CEditorSettingsDialog::OnButtonDefault() 
/* ============================================================
	Function :		CEditorSettingsDialog::OnButtonDefault
	Description :	Handler for the dialog button Default
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. Gets paper info from the 
					default printer. The values are further 
					modified with the difference between the 
					printer and the screen resolution.

   ============================================================*/
{
	CWaitCursor wait;

	int top;
	int bottom;
	int left;
	int right;
	int width;
	int height;
	int screenResolutionX;

	CClientDC dc( this );
	screenResolutionX = dc.GetDeviceCaps( LOGPIXELSX );

	CPrintDialog	printer( TRUE, PD_RETURNDC );
	printer.GetDefaults();
	HDC	hdc = printer.GetPrinterDC();
	if( hdc )
	{

		double diff = ( double ) GetDeviceCaps( hdc, LOGPIXELSX ) / ( double ) screenResolutionX;
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
		left = 0;
		top = 0;
		right = 0;
		bottom = 0;

		width = 8 * screenResolutionX;
		height = 11 * screenResolutionX;
	}


	m_width = width;
	m_height = height;

	if( left > m_left )
		m_left = left;
	if( top > m_top )
		m_top = top;
	if( right > m_right )
		m_right = right;
	if( bottom > m_bottom )
		m_bottom = bottom;

	SetMeasurementEdits();

}

void CEditorSettingsDialog::OnOK() 
/* ============================================================
	Function :		CEditorSettingsDialog::OnOK
	Description :	Handler for the dialog OK-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	GetMeasurements();
	CDialog::OnOK();
}
