/* ==========================================================================
	File :			ReportEntityPicture.cpp
	
	Class :			CReportEntityPicture

	Date :			07/14/04

	Purpose :		"CReportEntityPicture" derives from "CDiagramEntity" and 
					is a class representing a bmp picture.

	Description :	The class implements the necessary ovverides and adds 
					attributes to draw a picture.

	Usage :			Use as any "CDiagramEntity" object.

   ========================================================================*/

#include "stdafx.h"
#include "ReportEntityPicture.h"
#include "DiagramEditor/Tokenizer.h"
#include "ReportEntitySettings.h"
#include "StringHelpers.h"
#include "UnitConversion.h"

////////////////////////////////////////////////////////////////////
// Public functions
//
CReportEntityPicture::CReportEntityPicture()
/* ============================================================
	Function :		CReportEntityPicture::CReportEntityPicture
	Description :	Constructor.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	SetType( _T( "report_picture" ) );

	CReportEntitySettings::GetRESInstance()->GetBorderSettings( this );

	SetAttributeDialog( &m_dlg, IDD_REPORT_DIALOG_PICTURE_PROPERTIES );

	m_image_type = -1;
	m_image_size = 0;
	m_image_bits = NULL;

	m_bitmap = NULL;

	SetMinimumSize( CSize( 0, 0 ) );

}

CReportEntityPicture::CReportEntityPicture(HENHMETAFILE hMF)
{
	SetType( _T( "report_picture" ) );

	CReportEntitySettings::GetRESInstance()->GetBorderSettings( this );

	SetAttributeDialog( &m_dlg, IDD_REPORT_DIALOG_PICTURE_PROPERTIES );

	
	UINT uiSize = ::GetEnhMetaFileBits(hMF, 0, NULL);
	if (uiSize==0)
		return;
	m_image_size = uiSize;
	m_image_bits = new BYTE[m_image_size];
	::GetEnhMetaFileBits(hMF,uiSize,m_image_bits);

	m_image_type = CXIMAGE_FORMAT_WMF;
	m_bitmap = new CxImage(m_image_bits,m_image_size,m_image_type);
	if (!m_bitmap->IsValid())
	{
		AfxMessageBox(m_bitmap->GetLastError());
		delete m_bitmap;
		m_bitmap = NULL;
		m_image_type = -1;
		m_image_size = 0;
		delete[] m_image_bits;
		m_image_bits = NULL;
		ASSERT(0);
		return;
	}

	//SetMinimumSize( CSize( 0, 0 ) );
	//AdjustSize();

}

CReportEntityPicture::~CReportEntityPicture()
/* ============================================================
	Function :		CReportEntityPicture::~CReportEntityPicture
	Description :	Destructor.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	if( m_dlg.m_hWnd )
		m_dlg.DestroyWindow();

	if( m_bitmap )
		delete m_bitmap;

	m_image_type = -1;
	m_image_size = 0;
	if (m_image_bits)
		delete[] m_image_bits;
}

unsigned int CReportEntityPicture::GetBorderThickness() const
/* ============================================================
	Function :		CReportEntityPicture::GetBorderThickness
	Description :	Accessor. Getter for "m_borderThickness".
	Access :		Public
					
	Return :		int		-	Current thickness
	Parameters :	none

	Usage :			Call to get the value of "m_borderThickness".

   ============================================================*/
{

	return m_borderThickness;

}

void CReportEntityPicture::SetBorderThickness( unsigned int value )
/* ============================================================
	Function :		CReportEntityPicture::SetBorderThickness
	Description :	Accessor. Setter for "m_borderThickness".
	Access :		Public
					
	Return :		void
	Parameters :	int value	-	New thickness

	Usage :			Call to set the value of "m_borderThickness".

   ============================================================*/
{

	m_borderThickness = value;

}

unsigned int CReportEntityPicture::GetBorderStyle() const
/* ============================================================
	Function :		CReportEntityPicture::GetBorderStyle
	Description :	Accessor. Getter for "m_borderStyle".
	Access :		Public
					
	Return :		int		-	Current style
	Parameters :	none

	Usage :			Call to get the value of "m_borderStyle".

   ============================================================*/
{

	return m_borderStyle;

}

void CReportEntityPicture::SetBorderStyle( unsigned int value )
/* ============================================================
	Function :		CReportEntityPicture::SetBorderStyle
	Description :	Accessor. Setter for "m_borderStyle".
	Access :		Public
					
	Return :		void
	Parameters :	int value	-	New style

	Usage :			Call to set the value of "m_borderStyle".

   ============================================================*/
{

	m_borderStyle = value;

}

unsigned int CReportEntityPicture::GetBorderColor() const
/* ============================================================
	Function :		CReportEntityPicture::GetBorderColor
	Description :	Accessor. Getter for "m_borderColor".
	Access :		Public
					
	Return :		COLORREF	-	Current color
	Parameters :	none

	Usage :			Call to get the value of "m_borderColor".

   ============================================================*/
{

	return m_borderColor;

}

void CReportEntityPicture::SetBorderColor( unsigned int value )
/* ============================================================
	Function :		CReportEntityPicture::SetBorderColor
	Description :	Accessor. Setter for "m_borderColor".
	Access :		Public
					
	Return :		void
	Parameters :	COLORREF value	-	New color

	Usage :			Call to set the value of "m_borderColor".

   ============================================================*/
{

	m_borderColor = value;

}

CDiagramEntity* CReportEntityPicture::Clone()
/* ============================================================
	Function :		CReportEntityPicture::Clone
	Description :	Clones the current object to a new one.
	Access :		Public
					
	Return :		CDiagramEntity*	-	New object.
	Parameters :	none

	Usage :			Call to clone this object to a new one

   ============================================================*/
{

	CReportEntityPicture* obj = new CReportEntityPicture;
	obj->Copy( this );
	return obj;

}

void CReportEntityPicture::Copy( CDiagramEntity * obj )
/* ============================================================
	Function :		CReportEntityPicture::Copy
	Description :	Copies data from "obj" to this object.
	Access :		Public
					
	Return :		void
	Parameters :	CDiagramEntity * obj	-	Object to copy from

	Usage :			Call to copy data from "obj" to this object.

   ============================================================*/
{

	CDiagramEntity::Copy( obj );

	CReportEntityPicture* copy = static_cast< CReportEntityPicture* >( obj );

	SetBorderThickness( copy->GetBorderThickness() );
	SetBorderStyle( copy->GetBorderStyle() );
	SetBorderColor( copy->GetBorderColor() );
	SetFilename( copy->GetFilename() );

	if( m_bitmap )
		delete m_bitmap;
	m_bitmap = NULL;
	m_image_type = -1;
	m_image_size = 0;
	if (m_image_bits)
		delete[] m_image_bits;
	m_image_bits = NULL;

	if (copy->m_image_size>0 && copy->m_image_bits)
	{
		m_image_type = copy->m_image_type;
		m_image_size = copy->m_image_size;
		m_image_bits = new BYTE[m_image_size];
		memcpy(m_image_bits,copy->m_image_bits,m_image_size);
		m_bitmap = new CxImage(m_image_bits,m_image_size,m_image_type);
		if (!m_bitmap->IsValid())
		{
			AfxMessageBox(m_bitmap->GetLastError());
			delete m_bitmap;
			m_bitmap = NULL;
			m_image_type = -1;
			m_image_size = 0;
			delete[] m_image_bits;
			m_image_bits = NULL;
			ASSERT(0);
			return;
		}
	}
}

BOOL CReportEntityPicture::FromString( const CString& str )
/* ============================================================
	Function :		CReportEntityPicture::FromString
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

		double		 borderthickness;
		unsigned int borderstyle;
		unsigned int  bordercolor;
		CString		filename;

		int	count = 0;
		tok.GetAt( count++, borderthickness );
		int aaa;
		tok.GetAt( count++, aaa );
		borderstyle=aaa;
		tok.GetAt( count++, aaa );
		bordercolor=aaa;
		tok.GetAt( count++, filename );

		int bt = CUnitConversion::InchesToPixels( borderthickness );
		SetBorderThickness( bt );
		SetBorderStyle( borderstyle );
		SetBorderColor( bordercolor );
		UnmakeSaveString( filename );

		SetFilename( filename );

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

CString CReportEntityPicture::GetString() const
/* ============================================================
	Function :		CReportEntityPicture::GetString
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

	CReportEntityPicture* const local = const_cast< CReportEntityPicture* const >( this );
	local->SetLeft( left );
	local->SetRight( right );
	local->SetTop( top );
	local->SetBottom( bottom );

	CString str;
	CString filename = GetFilename();
	MakeSaveString( filename );

	double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
	str.Format( _T( ",%f,%i,%i,%s" ),
		thickness,
		GetBorderStyle( ),
		GetBorderColor( ),
		filename
		);

	str += _T( ";" );
	str = GetDefaultGetString() + str;

	local->SetLeft( oldleft );
	local->SetRight( oldright );
	local->SetTop( oldtop );
	local->SetBottom( oldbottom );

	return str;

}

CDiagramEntity* CReportEntityPicture::CreateFromString( const CString & str )
/* ============================================================
	Function :		CReportEntityPicture::CreateFromString
	Description :	Creates and returns an object of this 
					type from "str"
	Access :		Public

	Return :		CDiagramEntity*		-	Object created from "str"
	Parameters :	const CString & str	-	String to create object from
					
	Usage :			Call from a factory class to create 
					instances of this object.

   ============================================================*/
{

	CReportEntityPicture* obj = new CReportEntityPicture;
	if(!obj->FromString( str ) )
	{

		delete obj;
		obj = NULL;

	}

	return obj;

}

void  CReportEntityPicture::Serialize(CArchive& ar)
{
	typedef struct
	{
		int a_f;
		int b_f;
		int c_f;
		int d_f;
	} PIC_RESERVE_FIELDS;

	PIC_RESERVE_FIELDS pic_reserve;
	memset(&pic_reserve,0,sizeof(PIC_RESERVE_FIELDS));

	CDiagramEntity::Serialize(ar);
	if (ar.IsStoring())
	{
		// Сохраняем
		double thickness = CUnitConversion::PixelsToInches( GetBorderThickness() );
		ar.Write(&thickness,sizeof(double));
		unsigned int bst = GetBorderStyle( );
		ar.Write(&bst,sizeof(unsigned int));
		unsigned int  brC = GetBorderColor( );
		ar.Write(&brC,sizeof(unsigned int ));
		ar.Write(&pic_reserve,sizeof(PIC_RESERVE_FIELDS));

		if (m_image_size>0 && m_image_bits)
		{
			ar.Write(&m_image_type,sizeof(int));
			ar.Write(&m_image_size,sizeof(unsigned int));
			ar.Write(m_image_bits,sizeof(BYTE)*m_image_size);
		}
	}
	else
	{
		// Читаем
		double		bordhickness;
		ar.Read(&bordhickness,sizeof(double));
		unsigned int 		bordst;
		ar.Read(&bordst,sizeof(unsigned int ));
		unsigned int 	linecolor;
		ar.Read(&linecolor,sizeof(unsigned int ));
		ar.Read(&pic_reserve,sizeof(PIC_RESERVE_FIELDS));
		
		int bt = CUnitConversion::InchesToPixels( bordhickness );

		SetBorderThickness( bt );
		SetBorderStyle( bordst );
		SetBorderColor( linecolor );

		if( m_bitmap )
			delete m_bitmap;
		m_bitmap = NULL;
		m_image_type = -1;
		m_image_size = 0;
		if (m_image_bits)
			delete[] m_image_bits;
		m_image_bits = NULL;

		ar.Read(&m_image_type,sizeof(int));
		ar.Read(&m_image_size,sizeof(unsigned int));

		if (m_image_size==0)
			return;
		
		try
		{
			m_image_bits = new BYTE[m_image_size];
		}
		catch (std::bad_alloc) 
		{
			AfxMessageBox("bad_alloc exception in Serialize function");
			ASSERT(0);
			return;
		}

		if (ar.Read(m_image_bits,m_image_size*sizeof(BYTE))!=m_image_size*sizeof(BYTE))
		{
			delete[] m_image_bits;
			m_image_bits=NULL;
			m_image_type = -1;
			m_image_size = 0;
			ASSERT(0);
			return;
		}

		m_bitmap = new CxImage(m_image_bits, m_image_size, m_image_type);

		if (!m_bitmap->IsValid())
		{
			AfxMessageBox(m_bitmap->GetLastError());
			delete m_bitmap;
			m_bitmap = NULL;
			delete[] m_image_bits;
			m_image_bits=NULL;
			m_image_type = -1;
			m_image_size = 0;
			ASSERT(0);
			return;
		}	
	}
}
#include "..//Drawer.h"
void CReportEntityPicture::Draw( CDC* dc, CRect rect )
/* ============================================================
	Function :		CReportEntityPicture::Draw
	Description :	Draws the object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to
					CRect rect	-	True (zoomed) rectangle to 
									draw to.
					
	Usage :			Called from "CDiagramEditor::DrawObjects".

   ============================================================*/
{
	if( m_bitmap )
	{
		if (dc->IsPrinting()) 
		{
			// get size of printer page (in pixels)
			int cxPage = dc->GetDeviceCaps(HORZRES);
			int cyPage = dc->GetDeviceCaps(VERTRES);
			//int dcbpp = pDC->GetDeviceCaps(BITSPIXEL);
			//int dcnc = pDC->GetDeviceCaps(NUMCOLORS);
			//int dcp = pDC->GetDeviceCaps(PLANES);
			// get printer pixels per inch
			int cxInch = dc->GetDeviceCaps(LOGPIXELSX);
			int cyInch = dc->GetDeviceCaps(LOGPIXELSY);
			// Best Fit case: create a rectangle which preserves the aspect ratio
			int cx=m_bitmap->GetXDPI()?(m_bitmap->GetWidth()*cxInch)/m_bitmap->GetXDPI():
							m_bitmap->GetWidth()*cxInch/96;
			int cy=m_bitmap->GetYDPI()?(m_bitmap->GetHeight()*cyInch)/m_bitmap->GetYDPI():
						    m_bitmap->GetHeight()*cyInch/96;

			cx = rect.Width();
			cy = rect.Height();

			// print it!
			/*HDC TmpDC=CreateCompatibleDC(pDC->GetSafeHdc());
			HBITMAP bm =::CreateCompatibleBitmap(pDC->GetSafeHdc(), cx, cy);
			HBITMAP oldbm = (HBITMAP)::SelectObject(TmpDC,bm);
			BitBlt(TmpDC,0,0,cx,cy,0,0,0,WHITENESS);
			ima->Draw(TmpDC,CRect(0,0,cx,cy));
			BitBlt(pDC->GetSafeHdc(),100,100,cx,cy,TmpDC,0,0,SRCCOPY);
			DeleteObject(SelectObject(TmpDC,oldbm));
			DeleteDC(TmpDC);*/
			CxImage tmp;
			tmp.Copy(*m_bitmap);
			RGBQUAD c={255,255,255,0};
			tmp.SetTransColor(c);
			tmp.AlphaStrip();
			tmp.Stretch(dc->GetSafeHdc(), CRect(rect.left,rect.top,rect.left+cx,rect.top+cy));
		}
		else
		{
			rect.InflateRect( -1, -1 );
			m_bitmap->Draw(dc->m_hDC,rect);
			//dc->PlayMetaFile()
			//m_bitmap->GetType()
		}
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
	
	dc->SelectStockObject( BLACK_PEN );
	dc->SelectStockObject( WHITE_BRUSH );

}


static int TypeFromExtension_222(const CString& ext)
{
	int type = 0;
	if (ext == "bmp")					type = CXIMAGE_FORMAT_BMP;
#if CXIMAGE_SUPPORT_JPG
	else if (ext=="jpg"||ext=="jpeg")	type = CXIMAGE_FORMAT_JPG;
#endif
#if CXIMAGE_SUPPORT_GIF
	else if (ext == "gif")				type = CXIMAGE_FORMAT_GIF;
#endif
#if CXIMAGE_SUPPORT_PNG
	else if (ext == "png")				type = CXIMAGE_FORMAT_PNG;
#endif
#if CXIMAGE_SUPPORT_MNG
	else if (ext=="mng"||ext=="jng")	type = CXIMAGE_FORMAT_MNG;
#endif
#if CXIMAGE_SUPPORT_ICO
	else if (ext == "ico")				type = CXIMAGE_FORMAT_ICO;
#endif
#if CXIMAGE_SUPPORT_TIF
	else if (ext=="tiff"||ext=="tif")	type = CXIMAGE_FORMAT_TIF;
#endif
#if CXIMAGE_SUPPORT_TGA
	else if (ext=="tga")				type = CXIMAGE_FORMAT_TGA;
#endif
#if CXIMAGE_SUPPORT_PCX
	else if (ext=="pcx")				type = CXIMAGE_FORMAT_PCX;
#endif
#if CXIMAGE_SUPPORT_WBMP
	else if (ext=="wbmp")				type = CXIMAGE_FORMAT_WBMP;
#endif
#if CXIMAGE_SUPPORT_WMF
	else if (ext=="wmf"||ext=="emf")	type = CXIMAGE_FORMAT_WMF;
#endif
#if CXIMAGE_SUPPORT_J2K
	else if (ext=="j2k"||ext=="jp2")	type = CXIMAGE_FORMAT_J2K;
#endif
#if CXIMAGE_SUPPORT_JBG
	else if (ext=="jbg")				type = CXIMAGE_FORMAT_JBG;
#endif
#if CXIMAGE_SUPPORT_JP2
	else if (ext=="jp2"||ext=="j2k")	type = CXIMAGE_FORMAT_JP2;
#endif
#if CXIMAGE_SUPPORT_JPC
	else if (ext=="jpc"||ext=="j2c")	type = CXIMAGE_FORMAT_JPC;
#endif
#if CXIMAGE_SUPPORT_PGX
	else if (ext=="pgx")				type = CXIMAGE_FORMAT_PGX;
#endif
#if CXIMAGE_SUPPORT_RAS
	else if (ext=="ras")				type = CXIMAGE_FORMAT_RAS;
#endif
#if CXIMAGE_SUPPORT_PNM
	else if (ext=="pnm"||ext=="pgm"||ext=="ppm") type = CXIMAGE_FORMAT_PNM;
#endif
	else type = CXIMAGE_FORMAT_UNKNOWN;

	return type;
}


static CString FindExtension_222(const CString& name)
{
	int len = name.GetLength();
	int i;
	for (i = len-1; i >= 0; i--){
		if (name[i] == '.'){
			return name.Mid(i+1);
		}
	}
	return CString("");
}

void CReportEntityPicture::SetFilename( const CString& filename )
/* ============================================================
	Function :		CReportEntityPicture::SetFilename
	Description :	Accessor. Setter for "m_filename".
	Access :		Public

	Return :		void
	Parameters :	const CString& filename	-	New filename
					
	Usage :			Call to set the value of "m_filename".

   ============================================================*/
{
	m_filename = filename;
	
	if( m_filename.GetLength() )
	{

		delete m_bitmap;
		delete[] m_image_bits;
		m_image_size = 0;
		m_image_type = -1;
		
		int imageType = TypeFromExtension_222(FindExtension_222(m_filename));
		if (imageType==CXIMAGE_FORMAT_UNKNOWN)
		{
			ASSERT(0);
			m_filename = CString("");
			return;
		}

		CFile			file;
		CFileException	fe;

		if (!file.Open(m_filename, CFile::modeRead, &fe))
		{
			ASSERT(0);
			m_filename = CString("");
		}

		m_image_size = (unsigned int)file.GetLength();
		m_image_type = imageType;

		try
		{
			m_image_bits = new BYTE[m_image_size];
		}
		catch (std::bad_alloc) 
		{
			AfxMessageBox("bad_alloc exception in SetFileName function");
			m_image_size = 0;
			m_image_type = -1;
			file.Close();
			m_filename = CString("");
			ASSERT(0);
			return;
		}

		if (file.Read(m_image_bits,m_image_size*sizeof(BYTE))!=m_image_size*sizeof(BYTE))
		{
			delete[] m_image_bits;
			m_image_size = 0;
			m_image_type = -1;
			file.Close();
			m_filename = CString("");
			ASSERT(0);
			return;
		}

		m_bitmap = new CxImage(m_image_bits, m_image_size, m_image_type);

		if (!m_bitmap->IsValid())
		{
			AfxMessageBox(m_bitmap->GetLastError());
			delete m_bitmap;
			delete[] m_image_bits;
			m_image_size = 0;
			m_image_type = -1;
			file.Close();
			m_filename = CString("");
			ASSERT(0);
			return;
		}
		file.Close();
	}
	
	CString title( m_filename );
	int found = title.ReverseFind( _TCHAR( '\\' ) );
	if( found != -1 )
		title = title.Right( title.GetLength() - ( found + 1 ) );

	SetTitle( title );

}

CString CReportEntityPicture::GetFilename() const
/* ============================================================
	Function :		CReportEntityPicture::GetFilename
	Description :	Accessor. Getter for "m_filename".
	Access :		Public

	Return :		CString	-	Current filename
	Parameters :	none

	Usage :			Call to get the value of "m_filename".

   ============================================================*/
{

	return m_filename;

}

void CReportEntityPicture::AdjustSize()
/* ============================================================
	Function :		CReportEntityPicture::AdjustSize
	Description :	The function adjusts the object size to
					the size of the picture.
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Call to make the object the size of the 
					picture. The size is scaled to the output 
					size of the current printer.

   ============================================================*/
{
	if( m_bitmap )
	{

		CClientDC dc( AfxGetMainWnd() );
		int screenResolutionX = dc.GetDeviceCaps( LOGPIXELSX );

		CPrintDialog	printer( TRUE, PD_RETURNDC );
		printer.GetDefaults();
		HDC	hdc = printer.GetPrinterDC();
		double diff = 1;
		if( hdc )
		{
			diff = static_cast< double >( GetDeviceCaps( hdc, LOGPIXELSX ) ) / static_cast< double >( screenResolutionX );
			::DeleteDC( hdc );
		}

		CRect rect = GetRect();
		int width = round( static_cast< double >( m_bitmap->GetWidth() ) / diff );
		int height = round( static_cast< double >( m_bitmap->GetHeight() ) / diff );

		SetRect( rect.left, rect.top, rect.left + width, rect.top + height );

	}
}
