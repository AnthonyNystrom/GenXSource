/* ==========================================================================
	Class :			CExListBox

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-06-14

	Purpose :		"CExListBox", derived from "CListbox", adds dragging to a 
					listbox. It also sends registered messages to the owner
					when DEL is pressed, when a line is double clicked and 
					when the selection is changed (the last two as the 
					dragging kills the normal notification mechanism).

					The class also sets the tab-position to half the 
					horizontal width, and automatically sets the listbox 
					scrollbar width when adding a string.

	Description :	Handles the mouse messages to allow dragging. Sends 
					registered messages to the parent for selected events.
					Hides "AddString" (as it is not virtual) for scrollbar 
					adjustments.

	Usage :			Use as any "CListbox".

   ========================================================================*/

#include "stdafx.h"
#include "ExListBox.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CExListBox

CExListBox::CExListBox()
/* ============================================================
	Function :		CExListBox::CExListBox
	Description :	Constructor
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			Created from resources

   ============================================================*/
{

	m_draggedLine = -1;

}

CExListBox::~CExListBox()
/* ============================================================
	Function :		CExListBox::~CExListBox
	Description :	Destructor
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			No need to destroy manually

   ============================================================*/
{
}

BEGIN_MESSAGE_MAP(CExListBox, CListBox)
	//{{AFX_MSG_MAP(CExListBox)
	ON_CONTROL_REFLECT(LBN_DBLCLK, OnDblclk)
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_KEYDOWN()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CExListBox message handlers

void CExListBox::OnDblclk() 
/* ============================================================
	Function :		CExListBox::OnDblclk
	Description :	Handler for the listbox double-click 
					notification.
	Access :		Protected
					
	Return :		void
	Parameters :	none

	Usage :			Called from MFC. Sends a registered message 
					to the parent.

   ============================================================*/
{

	GetParent()->SendMessage( rwm_EXLISTBOX_DBLCLICK, GetDlgCtrlID() );

}

void CExListBox::OnLButtonDown(UINT nFlags, CPoint point) 
/* ============================================================
	Function :		CExListBox::OnLButtonDown
	Description :	Handler for the "WM_LBUTTONDOWN" message
	Access :		Protected
					
	Return :		void
	Parameters :	UINT nFlags		-	Not used
					CPoint point	-	Not used
					
	Usage :			Called from MFC. Starts the line-dragging.

   ============================================================*/
{

	int count = GetCount();
	if( count > 1 )
		SetCapture();

	CListBox::OnLButtonDown(nFlags, point);

	int index = GetCurSel();
	if( index != LB_ERR && count > 1 )
		m_draggedLine = index;
	else
		ReleaseCapture();

}

void CExListBox::OnLButtonUp(UINT nFlags, CPoint point) 
/* ============================================================
	Function :		CExListBox::OnLButtonUp
	Description :	Handler for the "WM_LBUTTONUP"-message
	Access :		Protected
					
	Return :		void
	Parameters :	UINT nFlags		-	Not used
					CPoint point	-	Current mouse position
					
	Usage :			Called from MFC. Moves the dragged line, 
					if any.

   ============================================================*/
{

	if( m_draggedLine != -1 )
		ReleaseCapture();
	
	CListBox::OnLButtonUp(nFlags, point);

	// Move line under cursor
	if( m_draggedLine != -1 )
	{
		int index = GetCurSel();
		if( index != m_draggedLine )
		{
			CString draggedline;

			GetText( m_draggedLine, draggedline );
			DWORD data = GetItemData( m_draggedLine );
			DeleteString( m_draggedLine );
			index = InsertString( index, draggedline );
			SetItemData( index, data );
			SetCurSel( index );
		}

		m_draggedLine = -1;
	}

	GetParent()->SendMessage( rwm_EXLISTBOX_SELCHANGE, GetDlgCtrlID() );

}

void CExListBox::OnMouseMove(UINT nFlags, CPoint point) 
/* ============================================================
	Function :		CExListBox::OnMouseMove
	Description :	Handler for the "WM_MOUSEMOVE" message
	Access :		Protected

	Return :		void
	Parameters :	UINT nFlags		-	Not used
					CPoint point	-	Not used
					
	Usage :			Shows a special cursor while dragging

   ============================================================*/
{

	CListBox::OnMouseMove(nFlags, point);

	if( m_draggedLine != -1 )
		::SetCursor( ::LoadCursor( NULL, IDC_SIZENS ) );

}


void CExListBox::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags) 
/* ============================================================
	Function :		CExListBox::OnKeyDown
	Description :	Handler for the "WM_KEYDOWN" message. Checks 
					for deletes, and sends it back to the 
					application.
	Access :		Protected
					
	Return :		void
	Parameters :	UINT nChar		-	Key pressed
					UINT nRepCnt	-	Not used
					UINT nFlags		-	Not used
					
	Usage :			Called from MFC.

   ============================================================*/
{

	if( nChar == VK_DELETE )
		GetParent()->SendMessage( rwm_EXLISTBOX_DELETE, GetDlgCtrlID() );
	else
		CListBox::OnKeyDown(nChar, nRepCnt, nFlags);

}


void CExListBox::PreSubclassWindow() 
/* ============================================================
	Function :		CExListBox::PreSubclassWindow
	Description :	Called when the listbox is being subclassed.
	Access :		Protected
					
	Return :		void
	Parameters :	none

	Usage :			Called from MFC. Sets the one and only 
					tabstop.

   ============================================================*/
{

	CListBox::PreSubclassWindow();

	CRect rect;
	GetClientRect( rect );
	int tabs = ( rect.Width() ) / ( LOWORD( GetDialogBaseUnits() ) / 4 );

	SetTabStops( tabs );

}

int CExListBox::AddString( LPCTSTR str )
/* ============================================================
	Function :		CExListBox::AddString
	Description :	Hides "CListbox::AddString", to update the 
					horizontal scrollbar from the added string 
					width.
	Access :		Public
					
	Return :		int			-	Default from "CListbox::AddString"
	Parameters :	LPCTSTR str	-	String to add.
					
	Usage :			Use as "CListbox::AddString"

   ============================================================*/
{

	int result = LB_ERR;

	if( m_hWnd )
	{
		CClientDC	dc( this );
		CFont* font = GetFont();
		CFont* oldfont = dc.SelectObject( font);
		int width = dc.GetTextExtent( str ).cx + 16;
		if( width > GetHorizontalExtent() )
			SetHorizontalExtent( width );
		dc.SelectObject( oldfont );

		result = CListBox::AddString( str );

	}

	return result;

}

UINT rwm_EXLISTBOX_DBLCLICK = ::RegisterWindowMessage( _T( "{CF94DC6E-CE14-4d93-A06C-C67C7A5CBE0B}" ) );
UINT rwm_EXLISTBOX_DELETE = ::RegisterWindowMessage( _T( "{C6B4CD5B-0DB9-4133-BD67-4AA3440C9011}" ) );
UINT rwm_EXLISTBOX_SELCHANGE = ::RegisterWindowMessage( _T( "{CF94DC6E-CE14-4d93-A06C-C67C7A5CBE0C}" ) );

