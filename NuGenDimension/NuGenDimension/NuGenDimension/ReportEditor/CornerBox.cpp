/* ==========================================================================
	Class :			CCornerBox

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-16

	Purpose :		"CCornerBox" is a "CWnd"-derived class, used as a button 
					in the corner of two rulers. When clicked, a dialog box 
					is displayed where the user can select the measurement 
					type for the rulers.

	Description :	The class is an AppWizard-created class. A registered 
					message is sent to the parent when the measurement 
					type is changed.

	Usage :			Add with "Create" to the owning window.

   ========================================================================*/

#include "stdafx.h"
#include "..//resource.h"
#include "CornerBox.h"
#include "StdGrfx.h"

#include "RulerMeasurementsDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

UINT UWM_MEASUREMENTS = ::RegisterWindowMessage( _T( "REPORT_EDITOR_MEASUREMENT" ) );

/////////////////////////////////////////////////////////////////////////////
// CCornerBox

CCornerBox::CCornerBox()
/* ============================================================
	Function :		CCornerBox::CCornerBox
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	SetMeasurements( MEASURE_INCHES );

}

CCornerBox::~CCornerBox()
/* ============================================================
	Function :		CCornerBox::~CCornerBox
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}


BEGIN_MESSAGE_MAP(CCornerBox, CWnd)
	//{{AFX_MSG_MAP(CCornerBox)
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
	ON_WM_LBUTTONUP()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CCornerBox message handlers

BOOL CCornerBox::OnEraseBkgnd( CDC* /*pDC*/ ) 
/* ============================================================
	Function :		CCornerBox::OnEraseBkgnd
	Description :	Handler for the "WM_ERASEBKGND"-message.
	Access :		Protected

	Return :		BOOL		-	Always "TRUE"
	Parameters :	CDC* pDC	-	Not interested
					
	Usage :			Called from MFC. Handled to avoid flicker 
					as we draw the complete control in "OnPaint".

   ============================================================*/
{

	return TRUE;

}

void CCornerBox::OnPaint() 
/* ============================================================
	Function :		CCornerBox::OnPaint
	Description :	Handler for the "WM_PAINT"-message.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. Paints the control.

   ============================================================*/
{

	CPaintDC dc(this);
	
	CRect rect;
	GetClientRect( rect );
	int width = rect.Width();
	int height = rect.Height();

	CDC	memDC;
	memDC.CreateCompatibleDC( &dc );
	CBitmap	bitmap;
	bitmap.CreateCompatibleBitmap( &dc, width, height );
	CBitmap* oldbitmap = memDC.SelectObject( &bitmap );

	memDC.SelectObject( CStdGrfx::dialogPen() );
	memDC.SelectObject( CStdGrfx::dialogBrush() );
	memDC.Rectangle( rect );

	rect.InflateRect( -2, -2 );
	CStdGrfx::draw3dFrame( &memDC, rect );

	dc.BitBlt( 0, 0, width, height, &memDC, 0, 0, SRCCOPY );
	memDC.SelectObject( oldbitmap );

}

void CCornerBox::OnLButtonUp(UINT nFlags, CPoint point) 
/* ============================================================
	Function :		CCornerBox::OnLButtonUp
	Description :	Handler for the "WM_LBUTTONUP"-message.
	Access :		Protected

	Return :		void
	Parameters :	UINT nFlags		-	Not interested
					CPoint point	-	Not interested
					
	Usage :			Called from MFC. Signals to the parent.

   ============================================================*/
{

	/*CRulerMeasurementsDialog	dlg;
	dlg.m_measurements = GetMeasurements();

	if( dlg.DoModal() == IDOK )
	{
		SetMeasurements( dlg.m_measurements );
		GetParent()->SendMessage( UWM_MEASUREMENTS, GetMeasurements() );
	}
*/
	CWnd::OnLButtonUp(nFlags, point);

}

void CCornerBox::SetMeasurements( int measurements )
/* ============================================================
	Function :		CCornerBox::SetMeasurements
	Description :	Sets the current measurements
	Access :		Public

	Return :		void
	Parameters :	int measurements	-	New measurements
					
	Usage :			Call to set the measurements of this 
					control. The measurement can be one of
						"MEASURE_PIXELS" In pixels
						"MEASURE_INCHES" In inches
						"MEASURE_CENTIMETERS" In centimeters
   ============================================================*/
{

	m_measurements = measurements;

}

int CCornerBox::GetMeasurements() const
/* ============================================================
	Function :		CCornerBox::GetMeasurements
	Description :	Gets the current measurements
	Access :		Public

	Return :		int		-	Current measurement units.
	Parameters :	none

	Usage :			Call to get the measurements of this 
					control. The measurement can be one of
						"MEASURE_PIXELS" In pixels
						"MEASURE_INCHES" In inches
						"MEASURE_CENTIMETERS" In centimeters

   ============================================================*/
{

	return m_measurements;

}
