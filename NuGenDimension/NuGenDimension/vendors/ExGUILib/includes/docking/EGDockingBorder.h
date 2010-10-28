#pragma once

#define DRAWBTNSTYLE_BTN 1
#define DRAWBTNSTYLE_GROUP 2
#define DRAWBTNSTYLE_SELECT 4
#define DRAWBTNSTYLE_SEP 8

/////////////////////////////////////////////////////////////////////////////
// CEGBorderButton class

#include <vector>
#include "EGDockingFlyOut.h"

using std::vector;

class CEGBorderButton
{
	void CopyObject( CEGBorderButton& object ) {
		m_rcButton = object.m_rcButton;
		m_nStyle = object.m_nStyle;
		m_pPane = object.m_pPane;
	}
public:
	CEGBorderButton( int nStyle, CEGDockingPane * pPane ) {
		m_nStyle = nStyle;
		m_pPane = pPane;
	};

	CEGBorderButton( CEGBorderButton & object ) {
		CopyObject( object );
	}

	CEGBorderButton & operator = (CEGBorderButton& object ) {
		CopyObject( object );
		return *this;
	}
	CRect m_rcButton;
	int m_nStyle;
	CEGDockingPane * m_pPane;

};

bool operator== ( CEGBorderButton & obj1, CEGBorderButton & obj2 );

typedef vector<CEGBorderButton*> CEGBorderButtons;
typedef CEGBorderButtons::iterator CEGBorderButtonsIt;

/////////////////////////////////////////////////////////////////////////////
// CEGDockBorder class

class CEGDockBorder : public CWnd
{
protected:
	UINT m_nTimer;
	
	CEGDockingPane * m_pLastFlyOutPane;
	CEGDockingPane * m_pFlyOutPane;
	CEGDockingPane * m_pPaneForRestore;

	CRect m_rcFlyOut;
  CEGFlyOutPane m_wndFlyOut;
	BOOL m_bForcedFlyOut;
//	BOOL m_bTrackingForMouseLeave;

// Construction
public:
	CEGDockBorder();
	virtual ~CEGDockBorder();

// Attributes
public:
	int m_Style;
	int m_Height;
	CEGBorderButtons m_lstButtons;
	CFont m_Font;
	CFont m_VertFont;
	CImageList* m_pImageList;

// Operations
public:
	void Draw(CDC * pDC);
	BOOL Create(int Style, CWnd * pWnd);
	void AddButton( int nStyle, CEGDockingPane * pPane = NULL);
	void RemoveButton(const CEGDockingPane * pPane);
	
	BOOL NeedSizing();
	void ShowPaneEx( CEGDockingPane * pPane, int nDelay = 500 );
	void ShowPane( BOOL bForced );
	void SetGroupSize( CEGBorderButtons * pButtons, int nMaxSize, LPRECT lprcBorder, int * px, int * py );
	void OnHideFlyOut();
	void FlyDownBar(  BOOL bShow = TRUE );
	void FlyOut( CEGDockingPane* pPane, BOOL bForced );

	void FlyOutPane( CEGDockingPane* pPane );
	void FlyDownPane( CEGDockingPane* pPane );

#ifdef _DEBUG
	void DumpButtons( );
#endif

	void CalcLayout();
	void SetImageList(CImageList* pList) {
		m_pImageList = pList;
	};

	DECLARE_MESSAGE_MAP()
protected:
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown( UINT nFlags, CPoint point );
	LRESULT OnSizeParent(WPARAM, LPARAM);
	afx_msg void OnDestroy();
	afx_msg void OnTimer( UINT_PTR nIDEvent );
//	afx_msg LRESULT OnMouseLeave( WPARAM, LPARAM );
};
