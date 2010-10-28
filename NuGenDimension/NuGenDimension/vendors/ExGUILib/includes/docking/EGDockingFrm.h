#pragma once

/////////////////////////////////////////////////////////////////////////
// CEGDockingContext class
#include "EGDocking.h"

class CEGDockingContext : public CDockContext
{
    CRect m_rcCaption;
		BOOL m_bDrawing;
		CSize sizeHorz;
    CSize sizeVert;
    CSize sizeFloat;
		
		CEGDockingPane * m_pDropTarget;
		DockType	m_nDockType;
		CRect	m_rcDropArea;

		//DWORD CanDockEx( CRect rect, DWORD dwDockStyle, CDockBar** ppDockBar = NULL );
		DWORD CanDock( CPoint pt );
		CDockBar* GetDockBar(DWORD dwOverDockStyle);

		void GetClientArea( LPRECT lpRect );
public:
// Construction
    CEGDockingContext(CControlBar* pBar) : CDockContext(pBar) {}
		DockType GetDropTarget( CPoint pt, CEGDockingPane ** ppPane );

// Drag Operations
    virtual void StartDrag(CPoint pt, double xScale, int yOffset );
		void DrawFocusRect(BOOL bRemoveRect = FALSE);
		BOOL TrackNew();
		void MoveNew(CPoint pt);
		void EndDragNew();

};

