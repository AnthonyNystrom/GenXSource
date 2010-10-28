/* ==========================================================================
	Class :			CReportEntityLabel

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportEntityLabel" derives from "CDiagramEntity" and 
					is a class representing a label.

	Description :	The class implements the necessary ovverides and adds 
					attributes to draw a label.

	Usage :			Use as any "CDiagramEntity" object.

   ========================================================================*/
#include "stdafx.h"
#include "ReportEntityLabel.h"
#include "DiagramEditor/Tokenizer.h"
#include "ReportEntitySettings.h"
#include "UnitConversion.h"

CReportEntityLabel::CReportEntityLabel()
/* ============================================================
	Function :		CReportEntityLabel::CReportEntityLabel
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	SetType( _T( "report_label" ) );
	SetTitle( _T( "Label" ) );

	m_angle = 0;
	CReportEntitySettings::GetRESInstance()->GetFontSettings( this );
	CReportEntitySettings::GetRESInstance()->GetBorderSettings( this );
	SetAttributeDialog( &m_dlg, IDD_REPORT_DIALOG_LABEL_PROPERTIES );

	SetMinimumSize( CSize( 0, 0 ) );

}

CReportEntityLabel::~CReportEntityLabel()
/* ============================================================
	Function :		CReportEntityLabel::~CReportEntityLabel
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	if( m_dlg.m_hWnd )
		m_dlg.DestroyWindow();

}

unsigned int CReportEntityLabel::GetBorderThickness() const
{

	return m_borderThickness;

}

void CReportEntityLabel::SetBorderThickness( unsigned int value )
{

	m_borderThickness = value;

}

unsigned int CReportEntityLabel::GetBorderStyle() const
{

	return m_borderStyle;

}

void CReportEntityLabel::SetBorderStyle( unsigned int value )

{

	m_borderStyle = value;

}

unsigned int CReportEntityLabel::GetBorderColor() const

{

	return m_borderColor;

}

void CReportEntityLabel::SetBorderColor( unsigned int value )

{

	m_borderColor = value;

}


CDiagramEntity* CReportEntityLabel::Clone()
/* ============================================================
	Function :		CReportEntityLabel::Clone
	Description :	Creates a new object of this type, copies the data from this object, and returns the new one.
	Access :		Public

	Return :		CDiagramEntity*	-	A clone of this object
	Parameters :	none

	Usage :			Call to clone this object

   ============================================================*/
{

	CReportEntityLabel* obj = new CReportEntityLabel;
	obj->Copy( this );
	return obj;

}

void CReportEntityLabel::Copy( CDiagramEntity* obj )
/* ============================================================
	Function :		CReportEntityLabel::Copy
	Description :	Copies data from "obj" to this object.
	Access :		Public

	Return :		void
	Parameters :	CDiagramEntity* obj	-	Object to copy from
					
	Usage :			Call to copy data from "obj" to this object.

   ============================================================*/
{

	CDiagramEntity::Copy( obj );

	CReportEntityLabel* copy = static_cast< CReportEntityLabel* >( obj );

	SetFontName( copy->GetFontName() );
	SetCharSet( copy->GetCharSet() );
	SetFontSize( copy->GetFontSize() );
	SetFontBold( copy->GetFontBold() );
	SetFontItalic( copy->GetFontItalic() );
	SetFontUnderline( copy->GetFontUnderline() );
	SetFontStrikeout( copy->GetFontStrikeout() );
	SetFontColor( copy->GetFontColor() );
	SetJustification( copy->GetJustification() );

	SetBorderThickness( copy->GetBorderThickness() );
	SetBorderStyle( copy->GetBorderStyle() );
	SetBorderColor( copy->GetBorderColor() );

	m_angle = copy->m_angle;

}

BOOL CReportEntityLabel::FromString( const CString& str )
/* ============================================================
	Function :		CReportEntityLabel::FromString
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

		CTokenizer	tok( data );

		CString		fontname;
		BYTE        fontcharset;
		int			fontsize;
		BOOL		fontbold;
		BOOL		fontitalic;
		BOOL		fontunderline;
		BOOL		fontstrikeout;
		COLORREF	fontcolor;
		unsigned int			justification;

		double		borderthickness;
		unsigned int	borderstyle;
		unsigned int	bordercolor;

		int count = 0;

		DWORD aaa;
		tok.GetAt( count++, fontname );
		tok.GetAt( count++, aaa);
		fontcharset = (BYTE)aaa;
		tok.GetAt( count++, fontsize );
		tok.GetAt( count++, fontbold );
		tok.GetAt( count++, fontitalic );
		tok.GetAt( count++, fontunderline );
		tok.GetAt( count++, fontstrikeout );
		tok.GetAt( count++, fontcolor );
		
		tok.GetAt( count++, aaa );
		justification = aaa;

		DWORD tmpW;
		tok.GetAt( count++, tmpW );
		borderthickness=tmpW;
		tok.GetAt( count++, tmpW );
		borderstyle=tmpW;
		tok.GetAt( count++, tmpW );
		bordercolor = tmpW;

		int bt = CUnitConversion::InchesToPixels( borderthickness );
		SetBorderThickness( bt );
		SetBorderStyle( borderstyle );
		SetBorderColor( bordercolor );

		SetFontName( fontname );
		SetCharSet(fontcharset);
		SetFontSize( fontsize );
		SetFontBold( fontbold );
		SetFontItalic( fontitalic );
		SetFontUnderline( fontunderline );
		SetFontStrikeout( fontstrikeout );
		SetFontColor( fontcolor );
		SetJustification( justification );

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

CString CReportEntityLabel::GetString() const
/* ============================================================
	Function :		CReportEntityLabel::GetString
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

	CReportEntityLabel* const local = const_cast< CReportEntityLabel* const >( this );
	local->SetLeft( left );
	local->SetRight( right );
	local->SetTop( top );
	local->SetBottom( bottom );

	CString str;
	double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
	str.Format( _T( ",%s,%i,%i,%i,%i,%i,%i,%i,%i,%i,%i,%i,%i,%f,%i,%i;" ),
		GetFontName( ),
		GetCharSet( ),
		GetFontSize( ),
		GetFontBold( ),
		GetFontItalic( ),
		GetFontUnderline( ),
		GetFontStrikeout( ),
		GetFontColor( ),
		GetJustification(),
		thickness,
		GetBorderStyle( ),
		GetBorderColor( ) );

	str += _T( ";" );
	str = GetDefaultGetString() + str;

	local->SetLeft( oldleft );
	local->SetRight( oldright );
	local->SetTop( oldtop );
	local->SetBottom( oldbottom );

	return str;

}

CDiagramEntity* CReportEntityLabel::CreateFromString( const CString & str )
/* ============================================================
	Function :		CReportEntityLabel::CreateFromString
	Description :	Creates and returns an object of this 
					type from "str"
	Access :		Public

	Return :		CDiagramEntity*		-	Object created from "str"
	Parameters :	const CString & str	-	String to create object from
					
	Usage :			Call from a factory class to create 
					instances of this object.

   ============================================================*/
{

	CReportEntityLabel* obj = new CReportEntityLabel;
	if(!obj->FromString( str ) )
	{

		delete obj;
		obj = NULL;

	}

	return obj;

}

CString CReportEntityLabel::GetFontName() const
/* ============================================================
	Function :		CReportEntityLabel::GetFontName
	Description :	Accessor. Getter for "m_fontName".
	Access :		Public
					
	Return :		CString	-	Current font
	Parameters :	none

	Usage :			Call to get the value of "m_fontName".

   ============================================================*/
{

	return m_fontName;

}

void CReportEntityLabel::SetFontName( CString value )
/* ============================================================
	Function :		CReportEntityLabel::SetFontName
	Description :	Accessor. Setter for "m_fontName".
	Access :		Public
					
	Return :		void
	Parameters :	CString value	-	New font
	Usage :			Call to set the value of "m_fontName".

   ============================================================*/
{

	m_fontName = value;

}

int CReportEntityLabel::GetFontSize() const
/* ============================================================
	Function :		CReportEntityLabel::GetFontSize
	Description :	Accessor. Getter for "m_fontSize".
	Access :		Public
					
	Return :		int	- Current size
	Parameters :	none

	Usage :			Call to get the value of "m_fontSize".

   ============================================================*/
{

	return m_fontSize;

}

void CReportEntityLabel::SetFontSize( int value )
/* ============================================================
	Function :		CReportEntityLabel::SetFontSize
	Description :	Accessor. Setter for "m_fontSize".
	Access :		Public
					
	Return :		void
	Parameters :	int value	-	New size
	Usage :			Call to set the value of "m_fontSize".

   ============================================================*/
{

	m_fontSize = value;

}

BOOL CReportEntityLabel::GetFontBold() const
/* ============================================================
	Function :		CReportEntityLabel::GetFontBold
	Description :	Accessor. Getter for "m_fontBold".
	Access :		Public
					
	Return :		BOOL	-	"TRUE" for a bold font
	Parameters :	none

	Usage :			Call to get the value of "m_fontBold".

   ============================================================*/
{

	return m_fontBold;

}

void CReportEntityLabel::SetFontBold( BOOL value )
/* ============================================================
	Function :		CReportEntityLabel::SetFontBold
	Description :	Accessor. Setter for "m_fontBold".
	Access :		Public
					
	Return :		void
	Parameters :	BOOL value	-	"TRUE" for a bold font.
	Usage :			Call to set the value of "m_fontBold".

   ============================================================*/
{

	m_fontBold = value;

}

BOOL CReportEntityLabel::GetFontItalic() const
/* ============================================================
	Function :		CReportEntityLabel::GetFontItalic
	Description :	Accessor. Getter for "m_fontItalic".
	Access :		Public
					
	Return :		BOOL	-	"TRUE" for an italic font
	Parameters :	none

	Usage :			Call to get the value of "m_fontItalic".

   ============================================================*/
{

	return m_fontItalic;

}

void CReportEntityLabel::SetFontItalic( BOOL value )
/* ============================================================
	Function :		CReportEntityLabel::SetFontItalic
	Description :	Accessor. Setter for "m_fontItalic".
	Access :		Public
					
	Return :		void
	Parameters :	BOOL value	-	"TRUE" for an italic font
	Usage :			Call to set the value of "m_fontItalic".

   ============================================================*/
{

	m_fontItalic = value;

}

BOOL CReportEntityLabel::GetFontUnderline() const
/* ============================================================
	Function :		CReportEntityLabel::GetFontUnderline
	Description :	Accessor. Getter for "m_fontUnderline".
	Access :		Public
					
	Return :		BOOL	-	"TRUE" for an underlined font
	Parameters :	none

	Usage :			Call to get the value of "m_fontUnderline".

   ============================================================*/
{

	return m_fontUnderline;

}

void CReportEntityLabel::SetFontUnderline( BOOL value )
/* ============================================================
	Function :		CReportEntityLabel::SetFontUnderline
	Description :	Accessor. Setter for "m_fontUnderline".
	Access :		Public
					
	Return :		void
	Parameters :	BOOL value	-	"TRUE" for an underlined font
	Usage :			Call to set the value of "m_fontUnderline".

   ============================================================*/
{

	m_fontUnderline = value;

}

BOOL CReportEntityLabel::GetFontStrikeout() const
/* ============================================================
	Function :		CReportEntityLabel::GetFontStrikeout
	Description :	Accessor. Getter for "m_fontStrikeout".
	Access :		Public
					
	Return :		BOOL	-	"TRUE" for a strikeout font
	Parameters :	none

	Usage :			Call to get the value of "m_fontStrikeout".

   ============================================================*/
{

	return m_fontStrikeout;

}

void CReportEntityLabel::SetFontStrikeout( BOOL value )
/* ============================================================
	Function :		CReportEntityLabel::SetFontStrikeout
	Description :	Accessor. Setter for "m_fontStrikeout".
	Access :		Public
					
	Return :		void
	Parameters :	BOOL value	-	"TRUE" for a strikeout font
	Usage :			Call to set the value of "m_fontStrikeout".

   ============================================================*/
{

	m_fontStrikeout = value;

}

COLORREF CReportEntityLabel::GetFontColor() const
/* ============================================================
	Function :		CReportEntityLabel::GetFontColor
	Description :	Accessor. Getter for "m_fontColor".
	Access :		Public
					
	Return :		COLORREF	-	Font color
	Parameters :	none

	Usage :			Call to get the value of "m_fontColor".

   ============================================================*/
{

	return m_fontColor;

}

void CReportEntityLabel::SetFontColor( COLORREF value )
/* ============================================================
	Function :		CReportEntityLabel::SetFontColor
	Description :	Accessor. Setter for "m_fontColor".
	Access :		Public
					
	Return :		void
	Parameters :	COLORREF value	-	Font color
	Usage :			Call to set the value of "m_fontColor".

   ============================================================*/
{

	m_fontColor = value;

}

void  CReportEntityLabel::Serialize(CArchive& ar)
{
	typedef struct
	{
		int a_f;
		int b_f;
		int c_f;
		int d_f;
	} LABEL_RESERVE_FIELDS;

	LABEL_RESERVE_FIELDS label_reserve;
	memset(&label_reserve,0,sizeof(LABEL_RESERVE_FIELDS));

	CDiagramEntity::Serialize(ar);
	if (ar.IsStoring())
	{
		// Сохраняем
		CString   fntNm = GetFontName();
		WriteStringToArchive(ar,fntNm);
		BYTE       fntChS = GetCharSet( );
		ar.Write(&fntChS,sizeof(BYTE));
		int       fntSz = GetFontSize( );
		ar.Write(&fntSz,sizeof(int));
		BOOL bl = GetFontBold( );
		ar.Write(&bl, sizeof(BOOL));
		bl = GetFontItalic( );
		ar.Write(&bl, sizeof(BOOL));
		bl = GetFontUnderline( );
		ar.Write(&bl, sizeof(BOOL));
		bl = GetFontStrikeout( );
		ar.Write(&bl, sizeof(BOOL));
		COLORREF fC = GetFontColor( );
		ar.Write(&fC,sizeof(COLORREF));
		unsigned int jstfc = GetJustification();
		ar.Write(&jstfc, sizeof(unsigned int));
		ar.Write(&m_angle,sizeof(int));

		double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
		ar.Write(&thickness,sizeof(double));
		unsigned int bst = GetBorderStyle( );
		ar.Write(&bst,sizeof(unsigned int));
		unsigned int brC = GetBorderColor( );
		ar.Write(&brC,sizeof(unsigned int));

		ar.Write(&label_reserve,sizeof(LABEL_RESERVE_FIELDS));
	}
	else
	{
		CString		fontname;
		BYTE        fontCharSet;
		int			fontsize;
		BOOL		fontbold;
		BOOL		fontitalic;
		BOOL		fontunderline;
		BOOL		fontstrikeout;
		COLORREF	fontcolor;
		unsigned int justification;

		ReadStringFromArchive(ar,fontname);
		ar.Read(&fontCharSet,sizeof(BYTE));
		ar.Read(&fontsize,sizeof(int));
		ar.Read(&fontbold,sizeof(BOOL));
		ar.Read(&fontitalic,sizeof(BOOL));
		ar.Read(&fontunderline,sizeof(BOOL));
		ar.Read(&fontstrikeout,sizeof(BOOL));
		ar.Read(&fontcolor,sizeof(COLORREF));
		ar.Read(&justification,sizeof(unsigned int));
		ar.Read(&m_angle,sizeof(int));

		double		bordhickness;
		ar.Read(&bordhickness,sizeof(double));
		unsigned int		bordst;
		ar.Read(&bordst,sizeof(unsigned int));
		unsigned int	linecolor;
		ar.Read(&linecolor,sizeof(unsigned int));

		ar.Read(&label_reserve,sizeof(LABEL_RESERVE_FIELDS));

		int bt = CUnitConversion::InchesToPixels( bordhickness );

		SetBorderThickness( bt );
		SetBorderStyle( bordst );
		SetBorderColor( linecolor );

		SetFontName( fontname );
		SetCharSet(fontCharSet);
		SetFontSize( fontsize );
		SetFontBold( fontbold );
		SetFontItalic( fontitalic );
		SetFontUnderline( fontunderline );
		SetFontStrikeout( fontstrikeout );
		SetFontColor( fontcolor );
		SetJustification( justification );
	}
}

static CPoint GetStartPointOfText(CDC* pDC, const CString str, CRect rect, 
							double angle, UINT nOptions)
{
	CSize TextSize = pDC->GetTextExtent(str);

	CPoint resP(0,0);

	if (angle==0.0)
	{
		switch(nOptions) {
		case DT_LEFT:
			resP.x = rect.left; 
			resP.y = rect.top;
			break;
		case DT_RIGHT:
			resP.x = rect.right-TextSize.cx; 
			resP.y = rect.top;
			break;
		case DT_CENTER:
			resP.x = (rect.left+rect.right-TextSize.cx)/2; 
			resP.y = rect.top;
			break;
		default:
			ASSERT(0);
			break;
		}
		return resP;	
	}
	if (angle==90.0)
	{
		switch(nOptions) {
		case DT_LEFT:
			resP.x = rect.left; 
			resP.y = rect.bottom;
			break;
		case DT_RIGHT:
			resP.x = rect.left; 
			resP.y = rect.top+TextSize.cx;
			break;
		case DT_CENTER:
			resP.x = rect.left; 
			resP.y = (rect.top+rect.bottom+TextSize.cx)/2;
			break;
		default:
			ASSERT(0);
			break;
		}
		return resP;	
	}
	if (angle==-90.0)
	{
		switch(nOptions) {
		case DT_LEFT:
			resP.x = rect.right; 
			resP.y = rect.top;
			break;
		case DT_RIGHT:
			resP.x = rect.right; 
			resP.y = rect.bottom-TextSize.cx;
			break;
		case DT_CENTER:
			resP.x = rect.right; 
			resP.y = (rect.top+rect.bottom-TextSize.cx)/2;
			break;
		default:
			ASSERT(0);
			break;
		}
		return resP;	
	}
	if (angle==180.0)
	{
		switch(nOptions) {
		case DT_LEFT:
			resP.x = rect.left+TextSize.cx; 
			resP.y = rect.top+TextSize.cy;
			break;
		case DT_RIGHT:
			resP.x = rect.right; 
			resP.y = rect.top+TextSize.cy;
			break;
		case DT_CENTER:
			resP.x = (rect.left+rect.right+TextSize.cx)/2; 
			resP.y = rect.top+TextSize.cy;
			break;
		default:
			ASSERT(0);
			break;
		}
		return resP;	
	}
	return resP;
	
}


#include "..//Drawer.h"
void CReportEntityLabel::Draw( CDC* dc, CRect rect )
/* ============================================================
	Function :		CReportEntityLabel::Draw
	Description :	Draws the object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to
					CRect rect	-	True (zoomed) rectangle to 
									draw to.
					
	Usage :			Called from "CDiagramEditor::DrawObjects".

   ============================================================*/
{
	int fontheight = CUnitConversion::PointsToPixels( GetFontSize() );

	LOGFONT	lf;
	ZeroMemory( &lf, sizeof( lf ) );

	lstrcpy( lf.lfFaceName, GetFontName() );
	lf.lfCharSet = GetCharSet();
	lf.lfHeight = round( static_cast< double>( fontheight ) * GetZoom() );
	lf.lfItalic = ( BYTE ) GetFontItalic();
	lf.lfUnderline = ( BYTE ) GetFontUnderline();
	lf.lfStrikeOut = ( BYTE ) GetFontStrikeout();
	if( GetFontBold() )
		lf.lfWeight = FW_BOLD;
	else
		lf.lfWeight = FW_NORMAL;

	CFont	font;
	lf.lfEscapement = lf.lfOrientation = m_angle*10;
	font.CreateFontIndirect( &lf );

	dc->SelectObject( &font );
	int color = dc->SetTextColor( GetFontColor() );
	int mode = dc->SetBkMode( TRANSPARENT );
	unsigned int justification = GetJustification();

	CString title = GetTitle();

	CPoint startP = GetStartPointOfText(dc,title,rect,m_angle, justification);

	
	CRgn clipBox;
	::GetClipRgn(dc->m_hDC,clipBox);
	dc->IntersectClipRect(rect);
	dc->TextOut(startP.x,startP.y,title);
	dc->SetPixel(startP,RGB(255,0,0));

	//CRgn   rgn;

	//BOOL bSucceeded = rgn.CreateRectRgn( clipBox.left, clipBox.top, 
	//									clipBox.right, clipBox.bottom );
	//ASSERT( bSucceeded == TRUE );

	dc->SelectClipRgn(&clipBox);

	DeleteObject(clipBox);

	
	dc->SetBkMode( mode );
	dc->SetTextColor( color );
	dc->SelectStockObject( ANSI_VAR_FONT );

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

	dc->SelectStockObject( BLACK_PEN );

}

void CReportEntityLabel::SetJustification( unsigned int justification )
/* ============================================================
	Function :		CReportEntityLabel::SetJustification
	Description :	Setter for the justification attribute.
	Access :		Public

	Return :		void
	Parameters :	int justification	-	New justification
					
	Usage :			Call to set the justification for the flag. 
					Values are the valid flags for "DrawText"

   ============================================================*/
{

	m_justification = justification;

}

unsigned int CReportEntityLabel::GetJustification() const
/* ============================================================
	Function :		CReportEntityLabel::GetJustification
	Description :	Getter for the justification attribute
	Access :		Public

	Return :		int	-	Current justification
	Parameters :	none

	Usage :			Call to get the justification for the flag. 
					Values are the valid flags for "DrawText"

   ============================================================*/
{

	return m_justification;

}

