/* ==========================================================================
	File :			ReportEntityBox.cpp
	
	Class :			CReportEntityBox

	Date :			07/14/04

	Purpose :		"CReportEntityBox" derives from "CDiagramEntity" and 
					is a class representing a simple box.

	Description :	The class implements the necessary ovverides and adds 
					attributes to draw a box.

	Usage :			Use as any "CDiagramEntity" object.

   ========================================================================*/

#include "stdafx.h"
#include "ReportEntityBox.h"
#include "DiagramEditor/Tokenizer.h"
#include "ReportEntitySettings.h"
#include "UnitConversion.h"

////////////////////////////////////////////////////////////////////
// Public functions
//
CReportEntityBox::CReportEntityBox()
/* ============================================================
	Function :		CReportEntityBox::CReportEntityBox
	Description :	Constructor.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	SetType( _T( "report_box" ) );

	CReportEntitySettings::GetRESInstance()->GetBorderSettings( this );
	CReportEntitySettings::GetRESInstance()->GetFillSettings( this );

	SetAttributeDialog( &m_dlg, IDD_REPORT_DIALOG_BOX_PROPERTIES );

	SetMinimumSize( CSize( 0, 0 ) );

}

CReportEntityBox::~CReportEntityBox()
/* ============================================================
	Function :		CReportEntityBox::~CReportEntityBox
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

unsigned int CReportEntityBox::GetBorderThickness() const
/* ============================================================
	Function :		CReportEntityBox::GetBorderThickness
	Description :	Accessor. Getter for "m_borderThickness".
	Access :		Public
					
	Return :		int		-	Current thickness
	Parameters :	none

	Usage :			Call to get the value of "m_borderThickness".

   ============================================================*/
{

	return m_borderThickness;

}

void CReportEntityBox::SetBorderThickness( unsigned int value )
/* ============================================================
	Function :		CReportEntityBox::SetBorderThickness
	Description :	Accessor. Setter for "m_borderThickness".
	Access :		Public
					
	Return :		void
	Parameters :	int value	-	New thickness

	Usage :			Call to set the value of "m_borderThickness".

   ============================================================*/
{

	m_borderThickness = value;

}

unsigned int CReportEntityBox::GetBorderStyle() const
/* ============================================================
	Function :		CReportEntityBox::GetBorderStyle
	Description :	Accessor. Getter for "m_borderStyle".
	Access :		Public
					
	Return :		int		-	Current style
	Parameters :	none

	Usage :			Call to get the value of "m_borderStyle". 
					Can be one of the styles for "CreatePen"

   ============================================================*/
{

	return m_borderStyle;

}

void CReportEntityBox::SetBorderStyle( unsigned int value )
/* ============================================================
	Function :		CReportEntityBox::SetBorderStyle
	Description :	Accessor. Setter for "m_borderStyle".
	Access :		Public
					
	Return :		void
	Parameters :	int value	-	New style

	Usage :			Call to set the value of "m_borderStyle". 
					Can be one of the styles from "CreatePen".

   ============================================================*/
{

	m_borderStyle = value;

}

unsigned int CReportEntityBox::GetBorderColor() const
/* ============================================================
	Function :		CReportEntityBox::GetBorderColor
	Description :	Accessor. Getter for "m_borderColor".
	Access :		Public
					
	Return :		COLORREF	-	Current border color
	Parameters :	none

	Usage :			Call to get the value of "m_borderColor".

   ============================================================*/
{

	return m_borderColor;

}

void CReportEntityBox::SetBorderColor( unsigned int value )
/* ============================================================
	Function :		CReportEntityBox::SetBorderColor
	Description :	Accessor. Setter for "m_borderColor".
	Access :		Public
					
	Return :		void
	Parameters :	COLORREF value	-	New border color

	Usage :			Call to set the value of "m_borderColor".

   ============================================================*/
{

	m_borderColor = value;

}

BOOL CReportEntityBox::GetFill() const
/* ============================================================
	Function :		CReportEntityBox::GetFill
	Description :	Accessor. Getter for "m_fill".
	Access :		Public
					
	Return :		BOOL	-	"TRUE" if the object should 
								be filled.
	Parameters :	none

	Usage :			Call to get the value of "m_fill".

   ============================================================*/
{

	return m_fill;

}

void CReportEntityBox::SetFill( BOOL value )
/* ============================================================
	Function :		CReportEntityBox::SetFill
	Description :	Accessor. Setter for "m_fill".
	Access :		Public
					
	Return :		void
	Parameters :	BOOL value	-	"TRUE" if the object should 
									be filled.

	Usage :			Call to set the value of "m_fill".

   ============================================================*/
{

	m_fill = value;

}

unsigned int CReportEntityBox::GetFillColor() const
/* ============================================================
	Function :		CReportEntityBox::GetFillColor
	Description :	Accessor. Getter for "m_fillColor".
	Access :		Public
					
	Return :		COLORREF	-	Current fill color.
	Parameters :	none

	Usage :			Call to get the value of "m_fillColor".

   ============================================================*/
{

	return m_fillColor;

}

void CReportEntityBox::SetFillColor( unsigned int value )
/* ============================================================
	Function :		CReportEntityBox::SetFillColor
	Description :	Accessor. Setter for "m_fillColor".
	Access :		Public
					
	Return :		void
	Parameters :	COLORREF value	-	New fill color.

	Usage :			Call to set the value of "m_fillColor".

   ============================================================*/
{

	m_fillColor = value;

}

CDiagramEntity* CReportEntityBox::Clone()
/* ============================================================
	Function :		CReportEntityBox::Clone
	Description :	Clones the current object to a new one.
	Access :		Public
					
	Return :		CDiagramEntity*	-	A clone of this object
	Parameters :	none

	Usage :			Call to clone the current object.

   ============================================================*/
{

	CReportEntityBox* obj = new CReportEntityBox;
	obj->Copy( this );
	return obj;

}

void CReportEntityBox::Copy( CDiagramEntity * obj )
/* ============================================================
	Function :		CReportEntityBox::Copy
	Description :	Copies the data from "obj" to this object.
	Access :		Public
					
	Return :		void
	Parameters :	CDiagramEntity * obj	-	Object to copy from

	Usage :			Call to copy data from "obj"

   ============================================================*/
{

	CDiagramEntity::Copy( obj );

	CReportEntityBox* copy = static_cast< CReportEntityBox* >( obj );

	SetBorderThickness( copy->GetBorderThickness() );
	SetBorderStyle( copy->GetBorderStyle() );
	SetBorderColor( copy->GetBorderColor() );
	SetFill( copy->GetFill() );
	SetFillColor( copy->GetFillColor() );

}

BOOL CReportEntityBox::FromString( const CString& str )
/* ============================================================
	Function :		CReportEntityBox::FromString
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

		double		borderthickness;
		unsigned int	borderstyle;
		unsigned int	bordercolor;
		BOOL		fill;
		unsigned int	fillcolor;

		int	count = 0;
		DWORD tmpW;
		tok.GetAt( count++, tmpW );
		borderthickness=tmpW;
		tok.GetAt( count++, tmpW );
		borderstyle=tmpW;
		tok.GetAt( count++, tmpW );
		bordercolor = tmpW;
		tok.GetAt( count++, fill );
		tok.GetAt( count++, tmpW );
		fillcolor =tmpW;

		int bt = CUnitConversion::InchesToPixels( borderthickness );
		SetBorderThickness( bt );
		SetBorderStyle( borderstyle );
		SetBorderColor( bordercolor );
		SetFill( fill );
		SetFillColor( fillcolor );

		int left = CUnitConversion::InchesToPixels( GetLeft() );
		int right = CUnitConversion::InchesToPixels( GetRight() );
		int top = CUnitConversion::InchesToPixels( GetTop() );
		int bottom = CUnitConversion::InchesToPixels( GetBottom() );

		CRect rect( left, top, right, bottom );
		SetRect( rect );

		result = TRUE;

	}

	return result;

}

CString CReportEntityBox::GetString() const
/* ============================================================
	Function :		CReportEntityBox::GetString
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

	CReportEntityBox* const local = const_cast< CReportEntityBox* const >( this );
	local->SetLeft( left );
	local->SetRight( right );
	local->SetTop( top );
	local->SetBottom( bottom );

	CString str;
	double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
	str.Format( _T( ",%f,%i,%i,%i,%i" ),
		thickness,
		GetBorderStyle( ),
		GetBorderColor( ),
		GetFill( ),
		GetFillColor( ) );

	str += _T( ";" );
	str = GetDefaultGetString() + str;

	local->SetLeft( oldleft );
	local->SetRight( oldright );
	local->SetTop( oldtop );
	local->SetBottom( oldbottom );

	return str;

}

CDiagramEntity* CReportEntityBox::CreateFromString( const CString & str )
/* ============================================================
	Function :		CReportEntityBox::CreateFromString
	Description :	Creates and returns an object of this 
					type from "str"
	Access :		Public

	Return :		CDiagramEntity*		-	Object created from "str"
	Parameters :	const CString & str	-	String to create object from
					
	Usage :			Call from a factory class to create 
					instances of this object.

   ============================================================*/
{

	CReportEntityBox* obj = new CReportEntityBox;
	if(!obj->FromString( str ) )
	{

		delete obj;
		obj = NULL;

	}

	return obj;

}


void  CReportEntityBox::Serialize(CArchive& ar)
{
	
	typedef struct
	{
		int a_f;
		int b_f;
	} BOX_RESERVE_FIELDS;

	BOX_RESERVE_FIELDS box_reserve;
	memset(&box_reserve,0,sizeof(BOX_RESERVE_FIELDS));

	CDiagramEntity::Serialize(ar);
	if (ar.IsStoring())
	{
		// Сохраняем
		double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
		ar.Write(&thickness,sizeof(double));
		unsigned int bst = GetBorderStyle( );
		ar.Write(&bst,sizeof(unsigned int));
		unsigned int brC = GetBorderColor( );
		ar.Write(&brC,sizeof(unsigned int));
		BOOL gf = GetFill();
		ar.Write(&gf, sizeof(BOOL));
		brC = GetFillColor();
		ar.Write(&brC,sizeof(unsigned int));
		ar.Write(&box_reserve,sizeof(BOX_RESERVE_FIELDS));
	}
	else
	{
		// Читаем
		double		bordhickness;
		ar.Read(&bordhickness,sizeof(double));
		unsigned int		bordst;
		ar.Read(&bordst,sizeof(unsigned int));
		unsigned int	linecolor;
		ar.Read(&linecolor,sizeof(unsigned int));
		BOOL fil;
		ar.Read(&fil,sizeof(BOOL));
		unsigned int	filcolor;
		ar.Read(&filcolor,sizeof(unsigned int));

		ar.Read(&box_reserve,sizeof(BOX_RESERVE_FIELDS));

		int bt = CUnitConversion::InchesToPixels( bordhickness );
		
		SetBorderThickness( bt );
		SetBorderStyle( bordst );
		SetBorderColor( linecolor );
		SetFill( fil );
		SetFillColor( filcolor );

	}
}

#include "..//Drawer.h"
void CReportEntityBox::Draw( CDC* dc, CRect rect )
/* ============================================================
	Function :		CReportEntityBox::Draw
	Description :	Draws the object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to
					CRect rect	-	True (zoomed) rectangle to 
									draw to.
					
	Usage :			Called from "CDiagramEditor::DrawObjects".

   ============================================================*/
{
	if( GetFill() )
	{
		const float* table = Drawer::GetColorByIndex(GetFillColor());
		COLORREF clr = RGB((int)(table[0]*255.0f),(int)(table[1]*255.0f),(int)(table[2]*255.0f));
		dc->FillSolidRect(rect,clr);
	}

	CPen pen;
	if( GetBorderStyle()!=0 )
	{
		const float* table = Drawer::GetColorByIndex(GetBorderColor());
		COLORREF clr = RGB((int)(table[0]*255.0f),(int)(table[1]*255.0f),(int)(table[2]*255.0f));
		pen.CreatePen( PS_SOLID, GetBorderThickness(), clr );
		dc->SelectObject( &pen );

		if (GetBorderStyle() & DIAGRAM_FRAME_STYLE_LEFT)
		{
			dc->MoveTo(rect.left,rect.bottom);
			dc->LineTo(rect.left,rect.top);
		}
		if (GetBorderStyle() & DIAGRAM_FRAME_STYLE_RIGHT)
		{
			dc->MoveTo(rect.right,rect.bottom);
			dc->LineTo(rect.right,rect.top);
		}

		if (GetBorderStyle() & DIAGRAM_FRAME_STYLE_TOP)
		{
			dc->MoveTo(rect.left,rect.top);
			dc->LineTo(rect.right,rect.top);
		}

		if (GetBorderStyle() & DIAGRAM_FRAME_STYLE_BOTTOM)
		{
			dc->MoveTo(rect.left,rect.bottom);
			dc->LineTo(rect.right,rect.bottom);
		}
	}
	else
		dc->SelectStockObject( NULL_PEN );
	
	dc->SelectStockObject( BLACK_PEN );
	dc->SelectStockObject( WHITE_BRUSH );
}