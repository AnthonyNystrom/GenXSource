/* ==========================================================================
	Class :			CVertRuler

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-16

	Purpose :		"CVertRuler" is a "CWnd"-derived class representing a 
					vertical ruler.

	Description :	The class draws a ruler scale in the selected 
					measurement units starting from "m_startPos".

	Usage :			Create with "Create" from the parent window. Update 
					with "SetStartPos" to scroll the ruler. Change 
					measurement units by calling "SetMeasurements"

   ========================================================================*/

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "VertRuler.h"
#include "StdGrfx.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CVertRuler

CVertRuler::CVertRuler()
/* ============================================================
	Function :		CVertRuler::CVertRuler
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	SetMeasurements( MEASURE_INCHES );
	SetStartPos( 0 );
	SetZoom( 1 );
	SetPointerPos( -1 );

}

CVertRuler::~CVertRuler()
/* ============================================================
	Function :		CVertRuler::~CVertRuler
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

BEGIN_MESSAGE_MAP(CVertRuler, CWnd)
	//{{AFX_MSG_MAP(CVertRuler)
	ON_WM_PAINT()
	ON_WM_ERASEBKGND()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CVertRuler message handlers

void CVertRuler::OnPaint() 
/* ============================================================
	Function :		CVertRuler::OnPaint
	Description :	Handler for the "WM_PAINT"-message.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC. Paints the control.

   ============================================================*/
{

	CPaintDC dc(this); // device context for painting
	
	CRect rect;
	GetClientRect( rect );
	int width = rect.Width();
	int height = rect.Height();

	// Create a memory CDC
	CDC	memDC;
	memDC.CreateCompatibleDC( &dc );
	CBitmap	bitmap;
	bitmap.CreateCompatibleBitmap( &dc, width, height );
	CBitmap* oldbitmap = memDC.SelectObject( &bitmap );

	memDC.FillSolidRect( rect, RGB( 255, 255, 255 ) );

	// Draw the scale
	switch( GetMeasurements() )
	{

		case MEASURE_PIXELS:
			DrawPixelScale( &memDC, rect );
			break;
		case MEASURE_INCHES:
			DrawInchScale( &memDC, rect );
			break;
		case MEASURE_CENTIMETERS:
			DrawCentimeterScale( &memDC, rect );
			break;

	}

	// The frame
	memDC.SelectObject( CStdGrfx::darkshadowPen() );
	memDC.SelectStockObject( NULL_BRUSH );
	memDC.Rectangle( rect );

	memDC.SelectObject( CStdGrfx::dialogPen() );
	rect.InflateRect( -1, -1 );
	memDC.Rectangle( rect );
	rect.InflateRect( -1, -1 );
	memDC.Rectangle( rect );

	rect.InflateRect( -1, -1 );
	memDC.SelectObject( CStdGrfx::shadowPen() );
	memDC.Rectangle( rect );

	memDC.SelectStockObject( BLACK_PEN );

	if( GetPointerPos() != -1 )
	{

		GetClientRect( rect );
		CRect rc( rect.right - 8, GetPointerPos() - 2, rect.right - 3, GetPointerPos() + 2 );
		memDC.MoveTo( rc.TopLeft() );
		memDC.LineTo( rc.right, rc.top + rc.Height() / 2 );
		memDC.LineTo( rc.left, rc.bottom );
		memDC.LineTo( rc.TopLeft() );

	}

	dc.BitBlt( 0, 0, width, height, &memDC, 0, 0, SRCCOPY );
	memDC.SelectObject( oldbitmap );

}

BOOL CVertRuler::OnEraseBkgnd(CDC* /*pDC*/) 
/* ============================================================
	Function :		CVertRuler::OnEraseBkgnd
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

void CVertRuler::SetStartPos( int startPos )
/* ============================================================
	Function :		CVertRuler::SetStartPos
	Description :	Sets the start position of the ruler
	Access :		Public

	Return :		void
	Parameters :	int startPos	-	New start position
					
	Usage :			Call to set the start position of the ruler 
					scale. Should be - for example - 
					synchronized with the main app scrollbar.

   ============================================================*/
{

	if( m_startPos != startPos )
	{
		m_startPos = startPos;
		SetPointerPos( -1 );
		if( m_hWnd )
			RedrawWindow();
	}

}

int CVertRuler::GetStartPos() const
/* ============================================================
	Function :		CVertRuler::GetStartPos
	Description :	Gets the current starting position of the 
					ruler.
	Access :		Public

	Return :		int	-	The current starting position.
	Parameters :	none

	Usage :			Returns the current starting position of 
					the ruler.

   ============================================================*/
{

	return m_startPos;

}

void CVertRuler::SetMeasurements( int measurements )
/* ============================================================
	Function :		CVertRuler::SetMeasurements
	Description :	Set the current measurement type of the 
					ruler.
	Access :		Public

	Return :		void
	Parameters :	int measurements	-	The new measurement 
											type.
					
	Usage :			Call to set a new measurement type for the ruler. The measurement can be one of:
						"MEASURE_PIXELS" In pixels
						"MEASURE_INCHES" In inches
						"MEASURE_CENTIMETERS" In centimeters

   ============================================================*/
{

	if( measurements != m_measurements )
	{
		m_measurements = measurements;
		SetPointerPos( -1 );
		if( m_hWnd )
			RedrawWindow();
	}

}

int CVertRuler::GetMeasurements() const
/* ============================================================
	Function :		CVertRuler::GetMeasurements
	Description :	Gets the current measurement type of the 
					ruler.
	Access :		Public

	Return :		int	-	Current measurement units
	Parameters :	none

	Usage :			Call to get the measurement type for the ruler. The measurement can be one of:
						"MEASURE_PIXELS" In pixels
						"MEASURE_INCHES" In inches
						"MEASURE_CENTIMETERS" In centimeters

   ============================================================*/
{

	return m_measurements;

}

void CVertRuler::SetZoom( double zoom )
/* ============================================================
	Function :		CVertRuler::SetZoom
	Description :	Sets the current zoom level for the 
					control.
	Access :		Public

	Return :		void
	Parameters :	double zoom	-	New zoom level
					
	Usage :			Call to set a new zoom-level for the 
					control.

   ============================================================*/
{
	if( m_zoom != zoom )
	{
		m_zoom = zoom;
		SetPointerPos( -1 );
		if( m_hWnd )
			RedrawWindow();
	}

}

double CVertRuler::GetZoom() const
/* ============================================================
	Function :		CVertRuler::GetZoom
	Description :	Gets the zoom level for the control
	Access :		Public

	Return :		double	-	Current zoom level
	Parameters :	none

	Usage :			Call to get the current zoom level for the 
					control.

   ============================================================*/
{

	return m_zoom;

}

void CVertRuler::DrawPixelScale( CDC* dc, CRect rect )
/* ============================================================
	Function :		CVertRuler::DrawPixelScale
	Description :	Draws the ruler scale using pixels as the 
					measurement units.
	Access :		Protected

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to.
					CRect rect	-	Rectangle of scale.
					
	Usage :			Call to draw the scale in pixels.

   ============================================================*/
{

	DrawScale( dc, rect, 100, 10 );

}

void CVertRuler::DrawInchScale( CDC* dc, CRect rect )
/* ============================================================
	Function :		CVertRuler::DrawInchScale
	Description :	Draws the ruler scale using inches as the 
					measurement units.
	Access :		Protected

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to.
					CRect rect	-	Rectangle of scale.
					
	Usage :			Call to draw the scale in inches.

   ============================================================*/
{

	double seg = static_cast< double >( dc->GetDeviceCaps( LOGPIXELSY ) );
	DrawScale( dc, rect, seg, 8 );

}

void CVertRuler::DrawCentimeterScale( CDC* dc, CRect rect )
/* ============================================================
	Function :		CVertRuler::DrawCentimeterScale
	Description :	Draws the ruler scale with centimeters as 
					measurement units.
	Access :		Protected

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to.
					CRect rect	-	Size of scale
					
	Usage :			Call to draw a centimeter scale.

   ============================================================*/
{

	double seg = static_cast< double >( dc->GetDeviceCaps( LOGPIXELSY ) ) / 2.54;
	DrawScale( dc, rect, seg, 2 );

}

void CVertRuler::DrawScale( CDC* dc, CRect rect, double seg, double stepno )
/* ============================================================
	Function :		CVertRuler::DrawScale
	Description :	Draws the ruler scale
	Access :		Protected

	Return :		void
	Parameters :	CDC* dc			-	"CDC" to draw to.
					CRect rect		-	Size of scale
					double seg		-	Size of a segment
					double stepno	-	Number of sub-segments
					
	Usage :			Call to draw the ruler scale.

   ============================================================*/
{

	int segment = round( seg * GetZoom() );
	double step = static_cast< double >( segment ) / stepno;

	int count = 0;
	for( int t = -GetStartPos() ; t < rect.bottom ; t += segment )
	{
		for( int i = 0 ; i < static_cast< int >( stepno ); i ++ )
		{
			double pos = t + static_cast< double >( i ) * step;
			dc->MoveTo( rect.left + 11, round( pos ) );
			dc->LineTo( rect.right - 11, round( pos ) );
		}

		if( t >= rect.top )
		{

			CString str;
			str.Format( _T( "%d" ), count );

			dc->MoveTo( rect.left + 3, t );
			dc->LineTo( rect.left + 5, t );

			CFont font;
			int height = rect.Width() - 14;
			font.CreateFont( -height, 0, 0, 0, FW_NORMAL, 0, 0, 0, 0, 0, 0, 0, 0, _T( "Arial" ) );
			dc->SelectObject( &font );
			int hgt = dc->GetTextExtent( str ).cy;
			CRect text( rect.left + 3, t - hgt / 2, rect.right - 3, t + hgt / 2 );
			dc->DrawText( str, text, DT_SINGLELINE | DT_CENTER | DT_VCENTER );
			dc->SelectStockObject( ANSI_VAR_FONT );

			dc->MoveTo( rect.right - 5, t );
			dc->LineTo( rect.right - 3, t );
		}

		count++;

	}

}

void CVertRuler::SetPointerPos( int pointerPos )
/* ============================================================
	Function :		CVertRuler::SetPointerPos
	Description :	Sets the pointer position.
	Access :		Public

	Return :		void
	Parameters :	int pointerPos	-	New pointer position
					
	Usage :			Call to Set the current pointer position. 
					-1 means invisible. The pointer is the small 
					triangle that can be used to display the 
					mouse cursor position.

   ============================================================*/
{
	if( m_pointerPos != pointerPos )
	{

		m_pointerPos = pointerPos;
		if( m_hWnd )
			RedrawWindow();

	}
}

int CVertRuler::GetPointerPos() const
/* ============================================================
	Function :		CVertRuler::GetPointerPos
	Description :	Gets the current pointer position.
	Access :		Public

	Return :		int	-	Current pointer position
	Parameters :	none

	Usage :			Call to get the current pointer position. 
					-1 means invisible. The pointer is the small 
					triangle that can be used to display the 
					mouse cursor position.

   ============================================================*/
{

	return m_pointerPos;

}
