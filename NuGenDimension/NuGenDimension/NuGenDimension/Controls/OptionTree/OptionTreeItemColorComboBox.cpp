#include "stdafx.h"
#include "OptionTreeItemColorComboBox.h"

#include "..//..//Drawer.h"
// Added Headers
#include "OptionTree.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// COptionTreeItemColorComboBox

COptionTreeItemColorComboBox::COptionTreeItemColorComboBox()
{
	// Initialize variables
	m_bFocus = FALSE;
	m_lDropDownHeight = OT_COMBO_DROPDOWNHEIGHT;

	m_color_index = 0;


	// Set item type
	SetItemType(OT_ITEM_COMBOBOX);
}

COptionTreeItemColorComboBox::~COptionTreeItemColorComboBox()
{
}


BEGIN_MESSAGE_MAP(COptionTreeItemColorComboBox, CComboBox)
	//{{AFX_MSG_MAP(COptionTreeItemColorComboBox)
	ON_WM_SETFOCUS()
	ON_WM_KILLFOCUS()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// COptionTreeItemColorComboBox message handlers


void COptionTreeItemColorComboBox::DrawAttribute(CDC *pDC, const RECT &rcRect)
{
	// If we don't have focus, text is drawn.
	if (m_bFocus == TRUE)
	{
		return;
	}

	// Make sure options aren't NULL
	if (m_otOption == NULL)
	{
		return;
	}

	// Make sure there is a window
	if (!IsWindow(GetSafeHwnd()))
	{
		return;
	}

	// Set window position
	if (IsWindow(GetSafeHwnd()))
	{
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());
	}

	// Declare variables
	/*HGDIOBJ hOld;
	COLORREF crOld;
	int nOldBack;
	CRect rcText;
	CString strWindowText;
	COLORREF crOldBack;

	// Get window text
	GetWindowText(strWindowText);

	// Select font
	hOld = pDC->SelectObject(m_otOption->GetNormalFont());
	
	// Set text color
	if (IsReadOnly() == TRUE || m_otOption->IsWindowEnabled() == FALSE)
	{
		crOld = pDC->SetTextColor(GetSysColor(COLOR_GRAYTEXT));
	}
	else
	{
		crOld = pDC->SetTextColor(GetTextColor());
	}

	// Set background mode
	nOldBack = pDC->SetBkMode(TRANSPARENT);

	// Set background color
	crOldBack = pDC->SetBkColor(GetBackgroundColor());	

	// Get rectangle
	rcText = rcRect;

	// Draw text
	pDC->DrawText(strWindowText, rcText, DT_SINGLELINE | DT_VCENTER);

	// Restore GDI ojects
	pDC->SelectObject(hOld);
	pDC->SetTextColor(crOld);
	pDC->SetBkMode(nOldBack);
	pDC->SetBkColor(crOldBack);*/

	const float* table = Drawer::GetColorByIndex(m_color_index);
	COLORREF curColor=RGB((int)(table[0]*255.0f),(int)(table[1]*255.0f),(int)(table[2]*255.0f));

	CRect rrr = rcRect;
	CRect rcColor;
	rcColor.left  = rcRect.left + 1;
	rcColor.right = rcColor.left + rrr.Width()-10;//(long) OT_COLOR_SIZE;
	rcColor.top = rcRect.top + OT_SPACE - 2;
	rcColor.bottom = rcColor.top + (long) OT_COLOR_SIZE;

	 CBrush bBrush;
	 bBrush.CreateSolidBrush(curColor);

	 HGDIOBJ hOldBrush;
	 hOldBrush = pDC->SelectObject(GetSysColorBrush(COLOR_BTNSHADOW));

	// Draw color border
	rcColor.InflateRect(1, 1, 1, 1);
	pDC->PatBlt(rcColor.left, rcColor.top, rcColor.Width(), rcColor.Height(), PATCOPY);

	// Draw color
	rcColor.DeflateRect(1, 1, 1, 1);
	pDC->FillRect(rcColor, &bBrush);

	if (bBrush.GetSafeHandle() != NULL)
	{
		bBrush.DeleteObject();
	}
	 pDC->SelectObject(hOldBrush);



}

void COptionTreeItemColorComboBox::OnCommit()
{
	// Hide edit control
	if (IsWindow(GetSafeHwnd()))
	{
		// -- Show window
		ShowWindow(SW_HIDE);
	}
	m_color_index = GetCurSel();
}

void COptionTreeItemColorComboBox::OnRefresh()
{
	// Set the window text
	if (IsWindow(GetSafeHwnd()))
	{
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());
	}
}

void COptionTreeItemColorComboBox::OnMove()
{
	// Set window position
	if (IsWindow(GetSafeHwnd()))
	{
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());
	}

	// Hide window
	if (m_bFocus == FALSE && IsWindow(GetSafeHwnd()))
	{
		// -- Show window
		ShowWindow(SW_HIDE);
	}
}

void COptionTreeItemColorComboBox::OnActivate()
{
	// Make sure window is valid
	if (IsWindow(GetSafeHwnd()))
	{

		// -- Show window
		ShowWindow(SW_SHOW);

		// -- Set window position
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height() + m_lDropDownHeight);

		// -- Set focus
		SetFocus();

		SetCurSel(m_color_index);
	}
}

void COptionTreeItemColorComboBox::OnSetFocus(CWnd* pOldWnd) 
{
	// Mark focus
	m_bFocus = TRUE;

	CComboBox::OnSetFocus(pOldWnd);	
}

void COptionTreeItemColorComboBox::OnKillFocus(CWnd* pNewWnd) 
{
	// Validate
	if (m_otOption == NULL)
	{
		CComboBox::OnKillFocus(pNewWnd);
		return;
	}

	// See if new window is tree of list
	if (m_otOption->IsChild(pNewWnd) == TRUE)
	{
		// -- Mark focus
		m_bFocus = FALSE;

		// -- Commit changes
		CommitChanges();
	}

	CComboBox::OnKillFocus(pNewWnd);	
}

BOOL COptionTreeItemColorComboBox::CreateComboItem(DWORD dwAddStyle)
{
	// Declare variables
	DWORD dwStyle = WS_CHILD | WS_VISIBLE | 
		CBS_DROPDOWNLIST | CBS_OWNERDRAWFIXED|WS_VSCROLL;
	BOOL bRet = FALSE;

	// Make sure options is not NULL
	if (m_otOption == NULL)
	{
		return FALSE;
	}

	// Create edit control
	if (!IsWindow(GetSafeHwnd()))
	{
		// -- Add style
		if (dwAddStyle != 0)
		{
			dwStyle |= dwAddStyle;
		}
		
		// -- Create the combo box
		bRet = Create(dwStyle, m_rcAttribute, m_otOption->GetCtrlParent(), GetCtrlID());

		// -- Setup combo
		if (bRet == TRUE)
		{
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

			// -- -- Set font
			SetFont(m_otOption->GetNormalFont(), TRUE);

			// -- -- Set window position
			MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());		

			// -- -- Hide window
			ShowWindow(SW_HIDE);
		}
	}


	return bRet;
}

void COptionTreeItemColorComboBox::SetDropDownHeight(long lHeight)
{
	// Save variable
	m_lDropDownHeight = lHeight;
}

long COptionTreeItemColorComboBox::GetDropDownHeight()
{
	// Return variable
	return m_lDropDownHeight;
}

void COptionTreeItemColorComboBox::CleanDestroyWindow()
{
	// Destroy window
	if (IsWindow(GetSafeHwnd()))
	{
		// -- Destroy window
		DestroyWindow();
	}
}
void COptionTreeItemColorComboBox::OnDeSelect()
{
	// Hide window
	if (IsWindow(GetSafeHwnd()))
	{
		// -- Show window
		ShowWindow(SW_HIDE);
	}
}

void COptionTreeItemColorComboBox::OnSelect()
{
	// Do nothing here
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

void	COptionTreeItemColorComboBox::DrawItem( LPDRAWITEMSTRUCT pDIStruct )
{
	COLORREF	crColor = 0;
	COLORREF	crNormal = GetSysColor( COLOR_WINDOW );
	COLORREF	crSelected = GetSysColor( COLOR_HIGHLIGHT );
	COLORREF	crText = GetSysColor( COLOR_WINDOWTEXT );
	CBrush		brFrameBrush;
	TCHAR		cColor[ 32 ];				// Color Name Buffer
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
		int		iChars = GetLBText( iItem, cColor );		// Get Color Text
		int		iaTabStops[ 1 ] = { 50 };

		_ASSERTE( iChars != LB_ERR );						// Sanity Check

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
		/*	dcContext.TabbedTextOut( rTextRect.left, 
		rTextRect.top, cColor, iChars, 1, 
		iaTabStops, 0 );	*/						// Draw Color Name

		dcContext.FillSolidRect( &rBlockRect, crColor );	// Draw Color

		dcContext.FrameRect( &rBlockRect, &brFrameBrush );	// Draw Frame
	}
	dcContext.Detach();										// Detach DC From Object

	return;													// Done!
}

void COptionTreeItemColorComboBox::MeasureItem(LPMEASUREITEMSTRUCT lpMeasureItemStruct)
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
