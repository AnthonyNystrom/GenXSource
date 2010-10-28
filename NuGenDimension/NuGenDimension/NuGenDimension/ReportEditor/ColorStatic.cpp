/* ==========================================================================
	Class :			CColorStatic

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CColorStatic" is a CStatic-derived class representing 
					a colored box 

	Description :	The control is a "CStatic", overriding "OnPaint".

	Usage :			The control can be used to display a specific color in 
					a dialog. Call "SetColor" to set the color to display.

   ========================================================================*/

#include "stdafx.h"
#include "ColorStatic.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CColorStatic

CColorStatic::CColorStatic()
/* ============================================================
	Function :		CColorStatic::CColorStatic
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

CColorStatic::~CColorStatic()
/* ============================================================
	Function :		CColorStatic::~CColorStatic
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

BEGIN_MESSAGE_MAP(CColorStatic, CStatic)
	//{{AFX_MSG_MAP(CColorStatic)
	ON_WM_PAINT()
	ON_WM_ERASEBKGND()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CColorStatic message handlers

void CColorStatic::OnPaint() 
/* ============================================================
	Function :		CColorStatic::OnPaint
	Description :	Handler for the "WM_PAINT" message.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{
	CPaintDC dc(this); // device context for painting
	CRect rect;
	CBrush	brush;
	
	GetClientRect( rect );
	brush.CreateSolidBrush( m_color );

	dc.SelectStockObject( BLACK_PEN );
	dc.SelectObject( &brush );

	dc.Rectangle( rect );

	dc.SelectStockObject( WHITE_BRUSH );

}

void CColorStatic::SetColor( COLORREF color )
/* ============================================================
	Function :		CColorStatic::SetColor
	Description :	Sets the color of the control
	Access :		Public

	Return :		void
	Parameters :	COLORREF color	-	Color to display
					
	Usage :			Call to set the color of this control.

   ============================================================*/
{

	m_color = color;
	if( m_hWnd )
		RedrawWindow();

}

BOOL CColorStatic::OnEraseBkgnd( CDC* /*pDC*/ ) 
/* ============================================================
	Function :		CColorStatic::OnEraseBkgnd
	Description :	Handler for the "WM_ERASEBKGND" message.
	Access :		Protected

	Return :		BOOL		-	Always "TRUE"
	Parameters :	CDC* pDC	-	"CDC" to erase
					
	Usage :			Called from MFC.

   ============================================================*/
{

	return TRUE;

}

