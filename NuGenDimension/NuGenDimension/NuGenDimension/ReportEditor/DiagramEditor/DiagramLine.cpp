/* ==========================================================================
	Class :			CDiagramLine

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-03-30

	Purpose :		Encapsulates a line object. Other line objects can be 
					derived from this class.

	Description :	First of all, we do not want constraints to the line 
					size (as we can't move the end points freely if that 
					is the case), so one "SetRect" must be modified. Second,
					we need a non-rectangular body area for hit testing, a 
					line in this case. Third, we need only a subset of the 
					selection markers. All this is implemented in this 
					class, to serve as a model or base class for other line 
					objects.

	Usage :			Use as a model for line objects.

   ========================================================================*/

#include "stdafx.h"
#include "DiagramLine.h"
#include "Tokenizer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

VOID CALLBACK HitTest( int X, int Y, LPARAM data );
VOID CALLBACK HitTestRect( int X, int Y, LPARAM data );

/////////////////////////////////////////////////////////////////////////////
// CDiagramLine
//

CDiagramLine::CDiagramLine()
/* ============================================================
	Function :		CDiagramLine::CDiagramLine
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	SetMinimumSize( CSize( -1, -1 ) );
	SetType( _T( "line" ) );

}

CDiagramLine::~CDiagramLine()
/* ============================================================
	Function :		CDiagramLine::~CDiagramLine
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

CDiagramEntity* CDiagramLine::Clone()
/* ============================================================
	Function :		CDiagramLine::Clone
	Description :	Clones this object and returns a new one.
	Access :		Public

	Return :		CDiagramEntity*	-	The resulting clone
	Parameters :	none

	Usage :			Call to clone the current object.

   ============================================================*/
{

	CDiagramLine* obj = new CDiagramLine;
	obj->Copy( this );
	return obj;

}

void CDiagramLine::SetRect( CRect rect )
/* ============================================================
	Function :		CDiagramLine::SetRect
	Description :	Sets the rectangle of the object.
	Access :		Public

	Return :		void
	Parameters :	CRect rect	-	New rectangle
					
	Usage :			Overriden to avoid normalization.

   ============================================================*/
{

	SetLeft( rect.left );
	SetTop( rect.top );
	SetRight( rect.right );
	SetBottom( rect.bottom );

}

BOOL CDiagramLine::BodyInRect( CRect rect ) const
/* ============================================================
	Function :		CDiagramLine::BodyInRect
	Description :	Checks if some part of the body of this 
					object lies inside the rectangle "rect".
	Access :		Public

	Return :		BOOL		-	"TRUE" if any part of the 
									object lies inside rect.
	Parameters :	CRect rect	-	The rectangle to test.
					
	Usage :			Shows one way to test a non-rectangular 
					object body - in this case a line.

   ============================================================*/
{

	BOOL result = FALSE;
	hitParamsRect hit;
	hit.rect = rect;
	hit.hit = FALSE;

	LineDDA(	static_cast< int >( GetLeft() ), 
				static_cast< int >( GetTop() ), 
				static_cast< int >( GetRight() ), 
				static_cast< int >( GetBottom() ), 
				HitTestRect, 
				( LPARAM ) &hit );

	if( hit.hit )
		result = TRUE;

	return result;

}

int CDiagramLine::GetHitCode( CPoint point ) const
/* ============================================================
	Function :		CDiagramLine::GetHitCode
	Description :	Returns the hit point constant ("DEHT_", 
					defined in DiagramEntity.h) for point. 
	Access :		Public

	Return :		int				-	The resulting hit point 
										constant, "DEHT_NONE" if 
										none.
	Parameters :	CPoint point	-	The point to test.
					
	Usage :			Shows one way to hit point test a non-
					rectangular area. The hit point can be one 
					of the following:
						"DEHT_NONE" No hit-point
						"DEHT_BODY" Inside object body
						"DEHT_TOPLEFT" Top-left corner
						"DEHT_TOPMIDDLE" Middle top-side
						"DEHT_TOPRIGHT" Top-right corner
						"DEHT_BOTTOMLEFT" Bottom-left corner
						"DEHT_BOTTOMMIDDLE" Middle bottom-side
						"DEHT_BOTTOMRIGHT" Bottom-right corner
						"DEHT_LEFTMIDDLE" Middle left-side
						"DEHT_RIGHTMIDDLE" Middle right-side

   ============================================================*/
{

	int result = DEHT_NONE;

	CRect rect = GetRect();

	hitParams hit;
	hit.hit = FALSE;
	hit.x = point.x;
	hit.y = point.y;

	LineDDA(	static_cast< int >( GetLeft() ), 
				static_cast< int >( GetTop() ), 
				static_cast< int >( GetRight() ), 
				static_cast< int >( GetBottom() ), 
				HitTest, 
				( LPARAM ) &hit );

	if( hit.hit )
		result = DEHT_BODY;

	CRect rectTest;

	rectTest = GetSelectionMarkerRect( DEHT_TOPLEFT, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_TOPLEFT;

	rectTest = GetSelectionMarkerRect( DEHT_BOTTOMRIGHT, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_BOTTOMRIGHT;

	return result;

}

void CDiagramLine::Draw( CDC* dc, CRect rect )
/* ============================================================
	Function :		CDiagramLine::Draw
	Description :	Draws the object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to
					CRect rect	-	True (zoomed) rectangle to 
									draw to.
					
	Usage :			Called from "CDiagramEditor::DrawObjects".

   ============================================================*/
{

	dc->SelectStockObject( BLACK_PEN );

	dc->MoveTo( rect.TopLeft() );
	dc->LineTo( rect.BottomRight() );

}

HCURSOR CDiagramLine::GetCursor( int hit ) const
/* ============================================================
	Function :		CDiagramLine::GetCursor
	Description :	Returns the cursor for a specific hit 
					point.
	Access :		Public

	Return :		HCURSOR	-	The cursor to display, or "NULL" 
								if default.
	Parameters :	int hit	-	The hit point constant ("DEHT_", 
								defined in DiagramEntity.h) to 
								show a cursor for.
					
	Usage :			Shows the cursor for a subset of the hit 
					points. Will also show cursors different 
					from the standard ones.
					"hit" can be one of the following:
						"DEHT_NONE" No hit-point
						"DEHT_BODY" Inside object body
						"DEHT_TOPLEFT" Top-left corner
						"DEHT_TOPMIDDLE" Middle top-side
						"DEHT_TOPRIGHT" Top-right corner
						"DEHT_BOTTOMLEFT" Bottom-left corner
						"DEHT_BOTTOMMIDDLE" Middle bottom-side
						"DEHT_BOTTOMRIGHT" Bottom-right corner
						"DEHT_LEFTMIDDLE" Middle left-side
						"DEHT_RIGHTMIDDLE" Middle right-side

   ============================================================*/
{

	HCURSOR cursor = NULL;
	switch( hit )
	{
		case DEHT_BODY:
		cursor = LoadCursor( NULL, IDC_SIZEALL );
		break;
		case DEHT_TOPLEFT:
		cursor = LoadCursor( NULL, IDC_SIZEALL );
		break;
		case DEHT_BOTTOMRIGHT:
		cursor = LoadCursor( NULL, IDC_SIZEALL );
		break;
	}

	return cursor;

}

void CDiagramLine::DrawSelectionMarkers( CDC* dc, CRect rect ) const
/* ============================================================
	Function :		CDiagramLine::DrawSelectionMarkers
	Description :	Draws selection markers for this object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to.
					CRect rect	-	True object rectangle.
					
	Usage :			Draws a subset of the standard selection 
					markers.

   ============================================================*/
{

	// Draw selection markers
	CRect rectSelect;
	dc->SelectStockObject( BLACK_BRUSH );

	rectSelect = GetSelectionMarkerRect( DEHT_TOPLEFT, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_BOTTOMRIGHT, rect );
	dc->Rectangle( rectSelect );

}

CDiagramEntity* CDiagramLine::CreateFromString( const CString& str )
/* ============================================================
	Function :		CDiagramLine::CreateFromString
	Description :	Static factory function to create a 
					"CDiagramLine" object from "str".
	Access :		Public

	Return :		CDiagramEntity*		-	Resulting object, 
											"NULL" if "str" is not 
											a representation of 
											a "CDiagramLine".
	Parameters :	const CString& str	-	String representation 
											to decode.
					
	Usage :			One proposed mechanism for loading/creating 
					"CDiagramEntity"-derived objects from a text 
					stream.

   ============================================================*/
{

	CDiagramLine* obj = new CDiagramLine;
	if(!obj->FromString( str ) )
	{
		delete obj;
		obj = NULL;
	}

	return obj;

}

int CDiagramLine::GetHitCode( const CPoint& point, const CRect& rect ) const
/* ============================================================
	Function :		CDiagramEntity::GetHitCode
	Description :	Returns the hit point constant for "point" 
					assuming the object rectangle "rect".
	Access :		Public

	Return :		int				-	The hit point, 
										"DEHT_NONE" if none.
	Parameters :	CPoint point	-	The point to check
					CRect rect		-	The rect to check
					
	Usage :			Call to see in what part of the object point 
					lies. The hit point can be one of the following:
						"DEHT_NONE" No hit-point
						"DEHT_BODY" Inside object body
						"DEHT_TOPLEFT" Top-left corner
						"DEHT_BOTTOMRIGHT" Bottom-right corner

   ============================================================*/
{
	// The rectangle comes in normalized, 
	// which might no be appropriate for 
	// this line. Check and de-normalize!
	CRect r( rect );

	if( GetTop() > GetBottom() )
	{
		int temp = r.bottom;
		r.bottom = r.top;
		r.top = temp;
	}

	if( GetLeft() > GetRight() )
	{
		int temp = r.right;
		r.right = r.left;
		r.left = temp;
	}

	int result = DEHT_NONE;

	hitParams hit;
	hit.hit = FALSE;
	hit.x = point.x;
	hit.y = point.y;

	LineDDA(	static_cast< int >( r.left ), 
				static_cast< int >( r.top ), 
				static_cast< int >( r.right ), 
				static_cast< int >( r.bottom ), 
				HitTest, 
				( LPARAM ) &hit );

	if( hit.hit )
		result = DEHT_BODY;

	CRect rectTest;

	rectTest = GetSelectionMarkerRect( DEHT_TOPLEFT, r );
	if( rectTest.PtInRect( point ) )
		result = DEHT_TOPLEFT;

	rectTest = GetSelectionMarkerRect( DEHT_BOTTOMRIGHT, r );
	if( rectTest.PtInRect( point ) )
		result = DEHT_BOTTOMRIGHT;

	return result;

}

VOID CALLBACK HitTest( int X, int Y, LPARAM data )
{

	// Checks if the coordinate in the hitParams 
	// struct falls within +/- 1 of the x, y 
	// coordinates of this point of the line.

	hitParams* obj = ( hitParams* ) data;

	if( obj->x >= X - 1 && obj->x <= X + 1 && obj->y >= Y - 1 && obj->y <= Y + 1 )
		obj->hit = TRUE;

}

VOID CALLBACK HitTestRect( int X, int Y, LPARAM data )
{

	// Checks if the x, y coordinates of the line 
	// falls withing the hitParamsRect rectangle.

	hitParamsRect* obj = ( hitParamsRect* ) data;
	CPoint pt( X, Y );

	if( obj->rect.PtInRect( pt ) )
		obj->hit = TRUE;

}

