/* ==========================================================================
	File :			ReportEntityLine.cpp
	
	Class :			CReportEntityLine

	Date :			07/14/04

	Purpose :		"CReportEntityLine" derives from "CDiagramLine" and 
					is a class representing a line.

	Description :	The class implements the necessary ovverides and adds 
					attributes to draw a line.

	Usage :			Use as any "CDiagramEntity" object.

   ========================================================================*/

#include "stdafx.h"
#include "ReportEntityLine.h"
#include "DiagramEditor/Tokenizer.h"
#include "ReportEntitySettings.h"
#include "UnitConversion.h"

////////////////////////////////////////////////////////////////////
// Public functions
//
CReportEntityLine::CReportEntityLine()
/* ============================================================
	Function :		CReportEntityLine::CReportEntityLine
	Description :	Constructor.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	SetType( _T( "report_line" ) );

	CReportEntitySettings::GetRESInstance()->GetLineSettings( this );
	SetAttributeDialog( &m_dlg, IDD_REPORT_DIALOG_LINE_PROPERTIES );

}

CReportEntityLine::~CReportEntityLine()
/* ============================================================
	Function :		CReportEntityLine::~CReportEntityLine
	Description :	Destructor.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	if( m_dlg.m_hWnd )
		m_dlg.DestroyWindow();

}

unsigned int CReportEntityLine::GetBorderThickness() const
/* ============================================================
	Function :		CReportEntityLine::GetBorderThickness
	Description :	Accessor. Getter for "m_borderThickness".
	Access :		Public
					
	Return :		int
	Parameters :	none

	Usage :			Call to get the value of "m_borderThickness".

   ============================================================*/
{

	return m_borderThickness;

}

void CReportEntityLine::SetBorderThickness( unsigned int value )
/* ============================================================
	Function :		CReportEntityLine::SetBorderThickness
	Description :	Accessor. Setter for "m_borderThickness".
	Access :		Public
					
	Return :		void
	Parameters :	int value	-	New thickness

	Usage :			Call to set the value of "m_borderThickness".

   ============================================================*/
{

	m_borderThickness = value;

}

unsigned int CReportEntityLine::GetBorderColor() const
/* ============================================================
	Function :		CReportEntityLine::GetBorderColor
	Description :	Accessor. Getter for "m_borderColor".
	Access :		Public
					
	Return :		COLORREF	-	Current color
	Parameters :	none

	Usage :			Call to get the value of "m_borderColor".

   ============================================================*/
{

	return m_borderColor;

}

void CReportEntityLine::SetBorderColor( unsigned int value )
/* ============================================================
	Function :		CReportEntityLine::SetBorderColor
	Description :	Accessor. Setter for "m_borderColor".
	Access :		Public
					
	Return :		void
	Parameters :	COLORREF value	-	New color

	Usage :			Call to set the value of "m_borderColor".

   ============================================================*/
{

	m_borderColor = value;

}

CDiagramEntity* CReportEntityLine::Clone()
/* ============================================================
	Function :		CReportEntityLine::Clone
	Description :	Clones the current object to a new one.
	Access :		Public
					
	Return :		CDiagramEntity*	-	New object.
	Parameters :	none

	Usage :			Call to clone this object to a new one

   ============================================================*/
{

	CReportEntityLine* obj = new CReportEntityLine;
	obj->Copy( this );
	return obj;

}

void CReportEntityLine::Copy( CDiagramEntity * obj )
/* ============================================================
	Function :		CReportEntityLine::Copy
	Description :	Copies data from "obj" to this object.
	Access :		Public
					
	Return :		void
	Parameters :	CDiagramEntity * obj	-	Object to copy from

	Usage :			Call to copy data from "obj" to this object.

   ============================================================*/
{

	CDiagramEntity::Copy( obj );

	CReportEntityLine* copy = static_cast< CReportEntityLine* >( obj );
	SetBorderThickness( copy->GetBorderThickness() );
	SetBorderColor( copy->GetBorderColor() );

}

BOOL CReportEntityLine::FromString( const CString& str )
/* ============================================================
	Function :		CReportEntityLine::FromString
	Description :	Sets the data of the object from "str".
	Access :		Public

	Return :		BOOL				-	"TRUE" if the string
											represents an object 
											of this type.
	Parameters :	const CString& str	-	String to parse
					
	Usage :			Call to load objects from a file.

   ============================================================*/
{

	BOOL result = FALSE;
	CString data( str );
	if( LoadFromString( data ) )
	{

		CTokenizer tok( data );

		double		linethickness;
		unsigned int linecolor;

		int	count = 0;
		DWORD aaa;
		tok.GetAt( count++, aaa );
		linethickness = aaa;
		tok.GetAt( count++, aaa );
		linecolor = aaa;

		int lt = CUnitConversion::InchesToPixels( linethickness );
		SetBorderThickness( lt );
		SetBorderColor( linecolor );

		result = TRUE;

		int left = CUnitConversion::InchesToPixels( GetLeft() );
		int right = CUnitConversion::InchesToPixels( GetRight() );
		int top = CUnitConversion::InchesToPixels( GetTop() );
		int bottom = CUnitConversion::InchesToPixels( GetBottom() );

		CRect rect( left, top, right, bottom );
		SetRect( rect );

	}

	return result;

}

CString CReportEntityLine::GetString() const
/* ============================================================
	Function :		CReportEntityLine::GetString
	Description :	Creates a string representing this object.
	Access :		Public

	Return :		CString	-	Resulting string
	Parameters :	none

	Usage :			Call to save this object to file.

   ============================================================*/
{


	CRect rect = GetRect();

	double oldleft = GetLeft();
	double oldright = GetRight();
	double oldtop = GetTop();
	double oldbottom = GetBottom();

	double left = CUnitConversion::PixelsToInches( rect.left );
	double right = CUnitConversion::PixelsToInches( rect.right );
	double top = CUnitConversion::PixelsToInches( rect.top );
	double bottom = CUnitConversion::PixelsToInches( rect.bottom );

	CReportEntityLine* const local = const_cast< CReportEntityLine* const >( this );
	local->SetLeft( left );
	local->SetRight( right );
	local->SetTop( top );
	local->SetBottom( bottom );

	CString str;

	double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
	str.Format( _T( ",%f,%i" ),
		thickness,
		GetBorderColor( ) );

	str += _T( ";" );
	str = GetDefaultGetString() + str;

	local->SetLeft( oldleft );
	local->SetRight( oldright );
	local->SetTop( oldtop );
	local->SetBottom( oldbottom );

	return str;

}

CDiagramEntity* CReportEntityLine::CreateFromString( const CString & str )
/* ============================================================
	Function :		CReportEntityLine::CreateFromString
	Description :	Creates and returns an object of this 
					type from "str"
	Access :		Public

	Return :		CDiagramEntity*		-	Object created from "str"
	Parameters :	const CString & str	-	String to create object from
					
	Usage :			Call from a factory class to create 
					instances of this object.

   ============================================================*/
{

	CReportEntityLine* obj = new CReportEntityLine;
	if(!obj->FromString( str ) )
	{

		delete obj;
		obj = NULL;

	}

	return obj;

}

void  CReportEntityLine::Serialize(CArchive& ar)
{
	typedef struct
	{
		int a_f;
		int b_f;
	} LINE_RESERVE_FIELDS;

	LINE_RESERVE_FIELDS line_reserve;
	memset(&line_reserve,0,sizeof(LINE_RESERVE_FIELDS));

	CDiagramEntity::Serialize(ar);
	if (ar.IsStoring())
	{
		// Сохраняем
		double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
		ar.Write(&thickness,sizeof(double));
		unsigned int brC = GetBorderColor( );
		ar.Write(&brC,sizeof(unsigned int));

		ar.Write(&line_reserve,sizeof(LINE_RESERVE_FIELDS));
	}
	else
	{
		// Читаем
		double		linethickness;
		ar.Read(&linethickness,sizeof(double));	
		unsigned int	linecolor;
		ar.Read(&linecolor,sizeof(unsigned int));	

		ar.Read(&line_reserve,sizeof(LINE_RESERVE_FIELDS));
		unsigned int lt = CUnitConversion::InchesToPixels( linethickness );
		SetBorderThickness( lt );
		SetBorderColor( linecolor );
	}
}

#include "..//Drawer.h"
void CReportEntityLine::Draw( CDC* dc, CRect rect )
/* ============================================================
	Function :		CReportEntityLine::Draw
	Description :	Draws the object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to
					CRect rect	-	True (zoomed) rectangle to 
									draw to.
					
	Usage :			Called from "CDiagramEditor::DrawObjects".

   ============================================================*/
{
	CPen pen;
	const float* table = Drawer::GetColorByIndex(GetBorderColor());
	COLORREF clr = RGB((int)(table[0]*255.0f),(int)(table[1]*255.0f),(int)(table[2]*255.0f));
	pen.CreatePen( PS_SOLID, GetBorderThickness(), clr );
	dc->SelectObject( &pen );

	dc->MoveTo( rect.TopLeft() );
	dc->LineTo( rect.BottomRight() );

	dc->SelectStockObject( BLACK_PEN );

}

