
#pragma warning (disable : 4127)	// conditional expression is constant

#include "stdafx.h"
#include "ColorPickerCB.h"

#include "..//Drawer.h"
#include ".\colorpickercb.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CColorPickerCB::CColorPickerCB()
{
	return;													// Done!
}


CColorPickerCB::~CColorPickerCB()
{
	return;													// Done!
}


BEGIN_MESSAGE_MAP(CColorPickerCB, CComboBox)
	//{{AFX_MSG_MAP(CColorPickerCB)
	ON_WM_CREATE()
	ON_WM_LBUTTONDOWN()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CColorPickerCB message handlers

int CColorPickerCB::OnCreate( LPCREATESTRUCT pCStruct ) 
{
	if( CComboBox::OnCreate( pCStruct ) == -1 )				// If Create Failed
	{
		return( -1 );										// Return Failure
	}
	_ASSERTE( GetStyle() & CBS_OWNERDRAWFIXED );			// Assert Proper Style Set
	_ASSERTE( GetStyle() & CBS_DROPDOWNLIST );				// Assert Proper Style Set
//	_ASSERTE( GetStyle() & CBS_HASSTRINGS );				// Assert Proper Style Set

	return( 0 );											// Done!
}


void CColorPickerCB::PreSubclassWindow() 
{
	CComboBox::PreSubclassWindow();							// Subclass Control

	_ASSERTE( GetStyle() & CBS_OWNERDRAWFIXED );			// Assert Proper Style Set
	_ASSERTE( GetStyle() & CBS_DROPDOWNLIST );				// Assert Proper Style Set
//	_ASSERTE( GetStyle() & CBS_HASSTRINGS );				// Assert Proper Style Set

	return;													// Done!
}


void	CColorPickerCB::InitializeDefaultColors( void )
{
	_ASSERTE( m_hWnd );										// We Must Be Created First...

	int			iAddedItem = -1;
	unsigned char iColors = 238;			// Get Number Of Colors Set

	ResetContent();											// Clear All Colors

	for( unsigned char iColor = 0; iColor < iColors; iColor++ )		// For All Colors
	{
		const float* table = Drawer::GetColorByIndex(iColor);
		COLORREF curColor=RGB((int)(table[0]*255.0f),(int)(table[1]*255.0f),(int)(table[2]*255.0f));
		iAddedItem = AddString(	"" );					// Set Color Name/Text
		if( iAddedItem == CB_ERRSPACE )						// If Not Added
		{
			ASSERT(0);								// Let 'Em Know What Happened...
			break;											// Stop
		}
		else												// If Added Successfully
		{
			SetItemData( iAddedItem, curColor );					// Set Color RGB Value
		}
	}
	return;													// Done!
}


void	CColorPickerCB::OnLButtonDown( UINT nFlags, CPoint ptPoint ) 
{
	if( GetFocus() != this )								// If Not Focused
	{
		SetFocus();											// Gain Focus
	}
	CComboBox::OnLButtonDown( nFlags, ptPoint );			// Do Default

	return;													// Done!
}


void	CColorPickerCB::DrawItem( LPDRAWITEMSTRUCT pDIStruct )
{
	COLORREF	crColor = 0;
	COLORREF	crNormal = GetSysColor( COLOR_WINDOW );
	COLORREF	crSelected = GetSysColor( COLOR_HIGHLIGHT );
	COLORREF	crText = GetSysColor( COLOR_WINDOWTEXT );
	CBrush		brFrameBrush;
	CRect		rItemRect( pDIStruct -> rcItem );
	CRect		rBlockRect( rItemRect );
	CRect		rTextRect( rBlockRect );
	CDC			dcContext;
	int			iFourthWidth = 0;
	int			iItem = pDIStruct -> itemID;
	int			iState = pDIStruct -> itemState;

	if( !dcContext.Attach( pDIStruct -> hDC ) )				// Attach CDC Object
	{
		return;												// Stop If Attach Failed
	}
	iFourthWidth = ( rBlockRect.Width() );				// Get 1/4 Of Item Area
	brFrameBrush.CreateStockObject( BLACK_BRUSH );			// Create Black Brush

	if( iState & ODS_SELECTED )								// If Selected
	{														// Set Selected Attributes
		dcContext.SetTextColor( ( 0x00FFFFFF & ~( 
				crText ) ) );								// Set Inverted Text Color (With Mask)
		dcContext.SetBkColor( crNormal );					// Set BG To Highlight Color
		dcContext.FillSolidRect( &rBlockRect, crNormal );	// Erase Item
	}
	else													// If Not Selected
	{														// Set Standard Attributes
		dcContext.SetTextColor( crText );					// Set Text Color
		dcContext.SetBkColor( crNormal );					// Set BG Color
		dcContext.FillSolidRect( &rBlockRect, crNormal );	// Erase Item
	}
	if( iState & ODS_FOCUS )								// If Item Has The Focus
	{
		dcContext.DrawFocusRect( &rItemRect );				// Draw Focus Rect
	}
	//
	//	Calculate Text Area...
	//
	rTextRect.left += ( iFourthWidth + 2 );					// Set Start Of Text
	rTextRect.top += 2;										// Offset A Bit

	//
	//	Calculate Color Block Area..
	//
	rBlockRect.DeflateRect( CSize( 2, 2 ) );				// Reduce Color Block Size
	rBlockRect.right = iFourthWidth;						// Set Width Of Color Block

	//
	//	Draw Color Text And Block...
	//
	if( iItem != -1 )										// If Not An Empty Item
	{
		
		if( iState & ODS_DISABLED )							// If Disabled
		{
			crColor = ::GetSysColor( COLOR_GRAYTEXT );		// Get Inactive Text Color
			dcContext.SetTextColor( crColor );				// Set Text Color
		}
		else												// If Normal
		{
			crColor = GetItemData( iItem );					// Get Color Value
		}
		dcContext.SetBkMode( TRANSPARENT );					// Transparent Background
	
		dcContext.FillSolidRect( &rBlockRect, crColor );	// Draw Color
				
		dcContext.FrameRect( &rBlockRect, &brFrameBrush );	// Draw Frame
	}
	dcContext.Detach();										// Detach DC From Object
	
	return;													// Done!
}


COLORREF	CColorPickerCB::GetSelectedColorValue( void )
{
	int		iSelectedItem = GetCurSel();					// Get Selected Item

	if( iSelectedItem == CB_ERR )							// If Nothing Selected
	{
		return( RGB( 0, 0, 0 ) );							// Return Black
	}
	return( GetItemData( iSelectedItem ) );					// Return Selected Color
}


LPCTSTR	CColorPickerCB::GetSelectedColorName( void )
{
	return "error";									// Return Selected Color Name
}


void		CColorPickerCB::SetSelectedColorValue( COLORREF crClr )
{
	int		iItems = GetCount();
	
	for( int iItem = 0; iItem < iItems; iItem++ )
	{
		if( crClr == GetItemData( iItem ) )					// If Match Found
		{
			SetCurSel( iItem );								// Select It
			break;											// Stop Looping
		}
	}
	return;													// Done!
}


void	CColorPickerCB::SetSelectedColorName( LPCTSTR cpColor )
{
	int		iItems = GetCount();
	TCHAR	cColor[ CCB_MAX_COLOR_NAME ];

	for( int iItem = 0; iItem < iItems; iItem++ )
	{
		GetLBText( iItem, cColor );							// Get Color Name
		if( !_tcsicmp( cColor, cpColor ) )					// If Match Found
		{
			SetCurSel( iItem );								// Select It
			break;											// Stop Looping
		}
	}
	return;													// Done!
}


int		CColorPickerCB::AddColor( LPCTSTR cpColor, COLORREF crColor )
{
	int		iIndex = CB_ERR;

	_ASSERTE( cpColor );									// Need This!

#if	defined( _INCLUDE_COLOR_INFO )
	TCHAR	caColor[ 256 ];

	_ASSERTE( _stprintf( caColor, 
			_T( "%s\t\t%02X%02X%02X" ), cpColor, GetRValue( 
				crColor ), GetGValue( crColor ), GetBValue( 
				crColor ) ) < 255 );						// Build The Debug String
		iIndex = AddString(	caColor );						// Set Color Name/Text
#else
		iIndex = AddString(	cpColor );						// Insert Just The Color
#endif
	if( iIndex != CB_ERR )									// If Inserted
	{
		SetItemData( iIndex, (DWORD)crColor );				// Set The Color Value
	}
	return( iIndex );										// Return Insertion Locatiom Or Failure Code
}


bool	CColorPickerCB::RemoveColor( LPCTSTR cpColor )
{
	TCHAR	cColor[ CCB_MAX_COLOR_NAME ];
	bool	bRemoved = false;
	int		iItems = GetCount();

	for( int iItem = 0; iItem < iItems; iItem++ )
	{
		GetLBText( iItem, cColor );							// Get Color Name
		if( !_tcsicmp( cColor, cpColor ) )					// If Match Found
		{
			if( DeleteString( iItem ) != CB_ERR )			// Remove It
			{
				bRemoved = true;							// Set Flag If Removed
				break;										// Stop Checking
			}
		}
	}
	return( bRemoved );										// Done!
}


bool	CColorPickerCB::RemoveColor( COLORREF crClr )
{
	bool	bRemoved = false;
	int		iItems = GetCount();

	for( int iItem = 0; iItem < iItems; iItem++ )
	{
		if( crClr == GetItemData( iItem ) )					// If Desired Color Found
		{
			if( DeleteString( iItem ) != CB_ERR )			// Remove It
			{
				bRemoved = true;							// Set Flag If Removed
				break;										// Stop Checking
			}
		}
	}
	return( bRemoved );										// Done!
}


void	CColorPickerCB::DDX_ColorPicker( CDataExchange *pDX, int iIDC, COLORREF &crColor )
{
	CColorPickerCB	*pPicker = NULL;
	HWND			hWndCtrl = pDX -> PrepareCtrl( iIDC );
	
	_ASSERTE( hWndCtrl );									// (In)Sanity Check

	pPicker = (CColorPickerCB*)CWnd::FromHandle( hWndCtrl );// Get Actual Control
	
	_ASSERTE( pPicker );									// (In)Sanity Check

	if( !( pDX -> m_bSaveAndValidate ) )					// If Setting The Color Value
	{
		pPicker -> SetSelectedColorValue( crColor );		// Set It
	}
	else													// If Getting The Color Value
	{
		crColor = pPicker -> GetSelectedColorValue();		// Get It
	}
	return;													// Done!
}


void	CColorPickerCB::DDX_ColorPicker( CDataExchange *pDX, int iIDC, CString &sName )
{
	CColorPickerCB	*pPicker = NULL;
	HWND			hWndCtrl = pDX -> PrepareCtrl( iIDC );
	
	_ASSERTE( hWndCtrl );									// (In)Sanity Check

	pPicker = (CColorPickerCB*)CWnd::FromHandle( hWndCtrl );// Get Actual Control
	
	_ASSERTE( pPicker );									// (In)Sanity Check

	if( !( pDX -> m_bSaveAndValidate ) )					// If Setting The Color Name
	{
		pPicker -> SetSelectedColorName( sName );			// Set It
	}
	else													// If Getting The Color Name
	{
		sName = pPicker -> GetSelectedColorName();			// Get It
	}
	return;													// Done!
}


#pragma warning (disable : 4018)	// '<':  signed/unsigned mismatch
#pragma warning (disable : 4100)	// unreferenced formal parameter
#pragma warning (disable : 4127)	// conditional expression is constant
#pragma warning (disable : 4244)	// conv from X to Y, possible loss of data
#pragma warning (disable : 4310)	// cast truncates constant value
#pragma warning (disable : 4505)	// X: unreference local function removed
#pragma warning (disable : 4510)	// X: default ctor could not be generated
#pragma warning (disable : 4511)	// X: copy constructor could not be generated
#pragma warning (disable : 4512)	// assignment operator could not be generated
#pragma warning (disable : 4514)	// debug symbol exceeds 255 chars
#pragma warning (disable : 4610)	// union X can never be instantiated
#pragma warning (disable : 4663)	// to explicitly spec class template X use ...
#pragma warning (disable : 4710)	// function 'XXX' not expanded
#pragma	warning	(disable : 4786)	// X: identifier truncated to '255' chars



void CColorPickerCB::MeasureItem(LPMEASUREITEMSTRUCT lpMeasureItemStruct)
{
	ASSERT(lpMeasureItemStruct->CtlType == ODT_COMBOBOX);

	if (lpMeasureItemStruct->itemID != (UINT) -1)
	{
		LPCTSTR lpszText = (LPCTSTR) lpMeasureItemStruct->itemData;
		if(lpszText != NULL)
		{
			CSize   sz;
			CDC*    pDC = GetDC();

			sz = pDC->GetTextExtent(lpszText);

			ReleaseDC(pDC);

			lpMeasureItemStruct->itemHeight = 2*sz.cy;
		}
		/*else
			lpMeasureItemStruct->itemHeight = 12;*/
	}

}
