/*********************************************************************
Docking support.
Classes:
- CEGBorderButton
  Info about docking pane on docking border

- CEGDockBorder
  Container of pinned panes

- CEGDockingBar
  Container with tree control for docking pane

- CEGDockingContext
  Drag & drop of docking pane

- CEGDockingFrameWnd
  

- CEGDockingPane
  Unit of docking

- CEGDockingControlPane
  CEGDockingPane with embeded control operations

- CEGDockSite
  Dummy class for acces to protected members of CDockBar
*********************************************************************/

#pragma once

#include "EGDockingPane.h"
#include "EGDockingFrm.h"
#include "EGDockingBorder.h"
#include "EGDockingFlyOut.h"

#define DM_AUTOHIDE_ON	WM_USER + 20
#define DM_AUTOHIDE_OFF WM_USER + 30
#define DM_DOCKING_BAR	WM_USER + 40


#define DECLARE_DOCKING()                                       \
    protected:                                                  \
	CImageList m_lstPaneIcons;	\
	CEGDockBorder m_arrDockingBorders[4];								\
	CEGDockingBars m_lstDockingBars;	\
	void EnablePinning( DWORD dwDockStyle );					\
	afx_msg LRESULT OnAutoHideOn(WPARAM, LPARAM);	\
	afx_msg LRESULT OnDockingBar(WPARAM, LPARAM);

#define ON_DOCKING_MESSAGES()									\
	ON_MESSAGE( DM_AUTOHIDE_ON, OnAutoHideOn)	\
	ON_MESSAGE( DM_DOCKING_BAR, OnDockingBar)


#define IMPLEMENT_DOCKING(theClass)                             \
LRESULT theClass::OnAutoHideOn(WPARAM, LPARAM lParam)			\
{																\
	CEGDockingBar *pWnd = (CEGDockingBar *)lParam;					\
	CEGDockBorder *pDockingBorder;								\
	switch(pWnd->GetDockBarID())								\
	{															\
		case AFX_IDW_DOCKBAR_TOP:								\
			pDockingBorder = &m_arrDockingBorders[0];					\
			break;												\
		case AFX_IDW_DOCKBAR_LEFT:								\
			pDockingBorder = &m_arrDockingBorders[1];					\
			break;												\
		case AFX_IDW_DOCKBAR_RIGHT:								\
			pDockingBorder = &m_arrDockingBorders[2];					\
			break;												\
		case AFX_IDW_DOCKBAR_BOTTOM:							\
			pDockingBorder = &m_arrDockingBorders[3];					\
			break;												\
	};															\
	pDockingBorder->AddButton( DRAWBTNSTYLE_SEP );			\
	pWnd->AddToBorder(pDockingBorder);							\
	pDockingBorder->AddButton( DRAWBTNSTYLE_SEP );			\
	pDockingBorder->CalcLayout();									\
	pDockingBorder->Invalidate();									\
	ShowControlBar(pWnd, FALSE, FALSE);							\
	return 0;													\
}																\
																\
void theClass::EnablePinning(DWORD dwDockStyle)				\
{																\
	if(dwDockStyle & CBRS_ALIGN_TOP )			\
		m_arrDockingBorders[0].Create(ALIGN_TOP, this);						\
	if(dwDockStyle & CBRS_ALIGN_LEFT )			\
		m_arrDockingBorders[1].Create(ALIGN_LEFT, this);						\
	if(dwDockStyle & CBRS_ALIGN_RIGHT )			\
		m_arrDockingBorders[2].Create(ALIGN_RIGHT, this);						\
	if(dwDockStyle & CBRS_ALIGN_BOTTOM )		\
		m_arrDockingBorders[3].Create(ALIGN_BOTTOM, this);						\
	if(dwDockStyle & CBRS_ALIGN_TOP )			\
	{															\
		if(::IsWindow(m_arrDockingBorders[1].GetSafeHwnd()))			\
			m_arrDockingBorders[1].SetWindowPos(GetControlBar(AFX_IDW_DOCKBAR_TOP),0,0,0,0, SWP_NOSIZE | SWP_NOMOVE);		\
		if(::IsWindow(m_arrDockingBorders[2].GetSafeHwnd()))																\
			m_arrDockingBorders[2].SetWindowPos(GetControlBar(AFX_IDW_DOCKBAR_TOP),0,0,0,0, SWP_NOSIZE | SWP_NOMOVE);		\
		if(::IsWindow(m_arrDockingBorders[3].GetSafeHwnd()))																\
			m_arrDockingBorders[3].SetWindowPos(GetControlBar(AFX_IDW_DOCKBAR_TOP),0,0,0,0, SWP_NOSIZE | SWP_NOMOVE);		\
	}																\
}	\
LRESULT theClass::OnDockingBar(WPARAM wParam, LPARAM lParam)			\
{	\
	CEGDockingBar * pBar = (CEGDockingBar*) (wParam );	\
	if ( 0 == lParam ) { \
		m_lstDockingBars.insert( pBar );	\
	} else {	\
		CEGDockingBarsIt it = m_lstDockingBars.find( pBar );	\
		if ( it != m_lstDockingBars.end() ) {	\
			m_lstDockingBars.erase( it );	\
			delete pBar;	\
		}	\
	}	\
	return 0;	\
}
