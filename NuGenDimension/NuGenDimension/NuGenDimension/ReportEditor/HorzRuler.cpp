/* ==========================================================================
	Class :			CHorzRuler

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-16

	Purpose :		"CHorzRuler" is a "CWnd"-derived class representing a 
					horizontal ruler.

	Description :	The class draws a ruler scale in the selected 
					measurement units starting from "m_startPos".

	Usage :			Create with "Create" from the parent window. Update 
					with "SetStartPos" to scroll the ruler. Change 
					measurement units by calling "SetMeasurements"

   ========================================================================*/

#include "stdafx.h"
#include "..//resource.h"
#include "HorzRuler.h"

#include "StdGrfx.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CHorzRuler

CHorzRuler::CHorzRuler()
/* ============================================================
	Function :		CHorzRuler::CHorzRuler
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

CHorzRuler::~CHorzRuler()
/* ============================================================
	Function :		CHorzRuler::~CHorzRuler
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

BEGIN_MESSAGE_MAP(CHorzRuler, CWnd)
	//{{AFX_MSG_MAP(CHorzRuler)
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CHorzRuler message handlers

BOOL CHorzRuler::OnEraseBkgnd(CDC* /*pDC*/) 
/* ============================================================
	Function :		CHorzRuler::OnEraseBkgnd
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

void CHorzRuler::OnPaint() 
/* ============================================================
	Function :		CHorzRuler::OnPaint
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

	// Create memory CDC
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

	// Draw frame
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

	// Pointer
	if( GetPointerPos() != -1 )
	{

		GetClientRect( rect );
		CRect rc( GetPointerPos() - 2, rect.bottom - 8, GetPointerPos() + 2, rect.bottom - 3 );
		memDC.MoveTo( rc.TopLeft() );
		memDC.LineTo( rc.right, rc.top );
		memDC.LineTo( rc.left + rc.Width() / 2, rc.bottom );
		memDC.LineTo( rc.TopLeft() );

	}

	dc.BitBlt( 0, 0, width, height, &memDC, 0, 0, SRCCOPY );
	memDC.SelectObject( oldbitmap );

}

void CHorzRuler::SetStartPos( int startPos )
/* ============================================================
	Function :		CHorzRuler::SetStartPos
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

int CHorzRuler::GetStartPos() const
/* ============================================================
	Function :		CHorzRuler::GetStartPos
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

void CHorzRuler::SetMeasurements( int measurements )
/* ============================================================
	Function :		CHorzRuler::SetMeasurements
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

int CHorzRuler::GetMeasurements() const
/* ============================================================
	Function :		CHorzRuler::GetMeasurements
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

void CHorzRuler::DrawPixelScale( CDC* dc, CRect rect )
/* ============================================================
	Function :		CHorzRuler::DrawPixelScale
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

void CHorzRuler::DrawInchScale( CDC* dc, CRect rect )
/* ============================================================
	Function :		CHorzRuler::DrawInchScale
	Description :	Draws the ruler scale using inches as the 
					measurement units.
	Access :		Protected

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to.
					CRect rect	-	Rectangle of scale.
					
	Usage :			Call to draw the scale in inches.

   ============================================================*/
{
	double seg = static_cast< double >( dc->GetDeviceCaps( LOGPIXELSX ) );
	DrawScale( dc, rect, seg, 8 );
}

void CHorzRuler::DrawCentimeterScale( CDC* dc, CRect rect )
/* ============================================================
	Function :		CHorzRuler::DrawCentimeterScale
	Description :	Draws the ruler scale with centimeters as 
					measurement units.
	Access :		Protected

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to.
					CRect rect	-	Size of scale
					
	Usage :			Call to draw a centimeter scale.

   ============================================================*/
{
	double seg = static_cast< double >( dc->GetDeviceCaps( LOGPIXELSX ) ) / 2.54;
	DrawScale( dc, rect, seg, 2 );
}

void CHorzRuler::DrawScale( CDC* dc, CRect rect, double seg, double stepno )
/* ============================================================
	Function :		CHorzRuler::DrawScale
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
	for( int t = -GetStartPos() ; t < rect.right ; t += segment )
	{
		for( int i = 0 ; i < static_cast< int >( stepno ); i ++ )
		{
			double pos = t + static_cast< double >( i ) * step;
			dc->MoveTo( round( pos ), rect.top + 11 );
			dc->LineTo( round( pos ), rect.bottom - 11 );
		}

		if( t >= rect.left )
		{
			dc->MoveTo( t, rect.top + 5 );
			dc->LineTo( t, rect.bottom - 5 );

			CRect text( t - rect.Height(), rect.top + 5, t + rect.Height(), rect.bottom - 5 );
			CFont font;
			int height = rect.Height() - 14;
			font.CreateFont( -height, 0, 0, 0, FW_NORMAL, 0, 0, 0, 0, 0, 0, 0, 0, _T( "Arial" ) );
			dc->SelectObject( &font );
			CString str;
			str.Format( _T( "%d" ), count );
			dc->DrawText( str, text, DT_SINGLELINE | DT_CENTER | DT_VCENTER );
			dc->SelectStockObject( ANSI_VAR_FONT );

		}

		count++;

	}
}

void CHorzRuler::SetZoom( double zoom )
/* ============================================================
	Function :		CHorzRuler::SetZoom
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

double CHorzRuler::GetZoom() const
/* ============================================================
	Function :		CHorzRuler::GetZoom
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

void CHorzRuler::SetPointerPos( int pointerPos )
/* ============================================================
	Function :		CHorzRuler::SetPointerPos
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

int CHorzRuler::GetPointerPos() const
/* ============================================================
	Function :		CHorzRuler::GetPointerPos
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
