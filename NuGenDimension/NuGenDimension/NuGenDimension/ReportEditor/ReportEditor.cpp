/* ==========================================================================
	Class :			CReportEditor

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportEditor" is a report template editor.

	Description :	The class derives from "CDiagramEditor".

	Usage :			See "CDiagramEditor" for instructions on use.

   ========================================================================*/
#include "stdafx.h"
#include "ReportEditor.h"

UINT UWM_HSCROLL = ::RegisterWindowMessage( _T( "REPORT_EDITOR_HSCROLL" ) );
UINT UWM_VSCROLL = ::RegisterWindowMessage( _T( "REPORT_EDITOR_VSCROLL" ) );
UINT UWM_ZOOM	 = ::RegisterWindowMessage( _T( "REPORT_EDITOR_ZOOM" ) );
UINT UWM_MOUSE	 = ::RegisterWindowMessage( _T( "REPORT_EDITOR_MOUSE" ) );

CReportEditor::CReportEditor()
/* ============================================================
	Function :		CReportEditor::CReportEditor
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

CReportEditor::~CReportEditor()
/* ============================================================
	Function :		CReportEditor::~CReportEditor
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

void CReportEditor::SetHScroll( int pos )
/* ============================================================
	Function :		CReportEditor::SetHScroll
	Description :	Sets the horizontal scroll bar position.
	Access :		Protected

	Return :		void
	Parameters :	int pos	-	New position
					
	Usage :			Overridden to notify the rulers in the view 
					of the new scroll position.
   ============================================================*/
{

	CDiagramEditor::SetHScroll( pos );
	GetParent()->SendMessage( UWM_HSCROLL, pos );

}

void CReportEditor::SetVScroll( int pos )
/* ============================================================
	Function :		CReportEditor::SetVScroll
	Description :	Sets the vertical scroll position.
	Access :		Protected

	Return :		void
	Parameters :	int pos	-	New position
					
	Usage :			Overridden to notify the rulers in the view 
					of the new scroll position.

   ============================================================*/
{

	CDiagramEditor::SetVScroll( pos );
	GetParent()->SendMessage( UWM_VSCROLL, pos );

}

void CReportEditor::SetZoom( double zoom )
/* ============================================================
	Function :		CReportEditor::SetZoom
	Description :	Sets the current zoom level of the editor.
	Access :		Public

	Return :		void
	Parameters :	double zoom	-	New zoom level
					
	Usage :			Overridden to notify the rulers in the view 
					of the new zoom level.

   ============================================================*/
{

	int z = static_cast< int >( zoom * 100.0 );
	CDiagramEditor::SetZoom( zoom );
	GetParent()->SendMessage( UWM_ZOOM, z );

}

void CReportEditor::OnMouseMove(UINT nFlags, CPoint point)
/* ============================================================
	Function :		CReportEditor::OnMouseMove
	Description :	Handler for the "WM_MOUSEMOVE"-message.
	Access :		Protected

	Return :		void
	Parameters :	UINT nFlags		-	Don't care
					CPoint point	-	Don't care
					
	Usage :			Called from MFC. Overridden to notify the 
					rulers in the view of the mouse position.

   ============================================================*/
{

	CDiagramEditor::OnMouseMove( nFlags, point );
	if( GetInteractMode() != MODE_NONE )
		GetParent()->SendMessage( UWM_MOUSE, ( WPARAM ) &point );
	else
		GetParent()->SendMessage( UWM_MOUSE, ( WPARAM ) NULL );

}

