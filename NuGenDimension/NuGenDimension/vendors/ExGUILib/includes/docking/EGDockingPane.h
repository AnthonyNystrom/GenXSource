#pragma once

#include <afxpriv.h>    // for CDockContext
#include <afxtempl.h>   // for CTypedPtrArray

/////////////////////////////////////////////////////////////////////////
// DockingPane styles

#define DPS_EDGELEFT       0x00000001
#define DPS_EDGERIGHT      0x00000002
#define DPS_EDGETOP        0x00000004
#define DPS_EDGEBOTTOM     0x00000008
#define DPS_EDGEALL        0x0000000F
#define DPS_SHOWEDGES      0x00000010
#define DPS_SIZECHILD      0x00000020


#define DOCK_AREA_SIZE 100
#define DROP_AREA_SIZE 20

class CEGDockBorder;

#include <vector>
#include <set>
#include <list>
#include "bitmap32.h"

using std::vector;
using std::set;
using std::list;

#include "EGMenu.h"       // CEGMenu class declaration
#include "EGToolBar.h"

/////////////////////////////////////////////////////////////////////////
// CDockingBar dummy class for access to protected members

class CEGDockSite : public CDockBar
{
    friend class CEGDockingBar;
};

/////////////////////////////////////////////////////////////////////////
// CEGDockingBar class

class CEGDockingBar;
typedef CTypedPtrArray <CPtrArray, CEGDockingBar*> CDPArray;

typedef enum { dctFloat, dctTop, dctLeft, dctBottom, dctRight, dctTab, dctOutOfBorders } DockType;

DockType GetDropTarget( CPoint pt, CRect rcClient );
CRect GetDropRect( DockType dt, CRect rcWindow, int nSize );

// Docking primitive
class CEGDockingPane : 
	public CStatic
{
protected:
	int	m_cyCaption;
	COLORREF m_clrColor;

  friend class CEGDockingBar;

	CEGDockingBar* m_pDockingBar;
	TCHAR* m_pszTitle;
	
	BOOL baseCreate( TCHAR* lpszTitle, CSize size, CFrameWnd* pFrame );
	void CreateBar( TCHAR* lpszTitle, CSize size, CFrameWnd* pFrame );
	
	BOOL m_bHidden;
	CBitmap32 m_bmpIcon;

public:
	CEGDockingPane();
	~CEGDockingPane();

	COLORREF Color(){ return m_clrColor; };
	CEGDockingBar* m_pRemovedFromBar;

// Operations
public:
	DockType GetDropTarget( CPoint pt );
	CRect GetDropRect( DockType dt );

	BOOL CreatePane( TCHAR* lpszTitle, CSize size, CFrameWnd* pFrame, CEGDockingPane *pPane = NULL, DockType nDockStyle = dctBottom, CPoint ptFloat = (0,0) );
	
	void CreateDragBar( CPoint pt, CSize size, double xScale );

	void AppendTab( CEGDockingPane* pPane );

	void RestoreParent();
	void RemoveFromBorder( CEGDockBorder * pBorder, BOOL bShow = TRUE );
	
	void Show();
	void Hide( BOOL bNotifyContainer = TRUE );
	void ToggleVisible();

	BOOL SetIcon( UINT nIDResource, HINSTANCE hInst = NULL );
	BOOL SetIcon( LPCTSTR lpszResourceName, HINSTANCE hInst = NULL );
	CBitmap32 * GetIcon() { return &m_bmpIcon; }

// Attributes
public:
	TCHAR* GetTitle() {return m_pszTitle; }
	CEGDockingBar * GetDockingBar(){ return m_pDockingBar; }
	int Width();
	int Height();
	BOOL IsHidden() {return m_bHidden; }
	
	CEGDockingBar * OwnerBar() { return m_pDockingBar; };

// Message map
	DECLARE_MESSAGE_MAP()
protected:
	afx_msg void OnKillFocus( NMHDR * pNotifyStruct, LRESULT * result );
	afx_msg void OnSetFocus( NMHDR * pNotifyStruct, LRESULT * result );
};

class CEGDockingControlPane :
	public CEGDockingPane
{
	int m_nID;
	CEGToolBar m_wndToolBar;
protected:
	HWND m_hWndControl;

	virtual HWND CreateControl();
	virtual BOOL InitControl();

public:
	CEGDockingControlPane(void);
	~CEGDockingControlPane(void);

	void SetResourceID( int nID );

	CEGToolBar*   GetToolbar()
	{ return &m_wndToolBar;};

	DECLARE_MESSAGE_MAP();
protected:
	afx_msg int OnCreate( LPCREATESTRUCT lpCreateStruct );
	afx_msg void OnSize( UINT nType, int cx, int cy );
	//afx_msg LRESULT OnKickIdle(WPARAM wp, LPARAM lp);
};

//class CEGToolBar;

/*class  CEGToolBaredPane: 
	public CEGDockingPane 
{
	int m_nID;
	CEGToolBar m_wndToolBar;
public:

	CEGToolBaredPane () { m_nID = 0; }

	void SetResourceID( int nID );

	DECLARE_MESSAGE_MAP();
protected:
	afx_msg int OnCreate( LPCREATESTRUCT lpCreateStruct );
	afx_msg void OnSize( UINT nType, int cx, int cy );
	afx_msg LRESULT OnKickIdle(WPARAM wp, LPARAM lp);
};
*/
// docking primitive's container
class CEGDockingTabBtn {
	void Copy( const CEGDockingTabBtn & tab ) {
		m_pPane = tab.m_pPane;
		m_nWidth = tab.m_nWidth;
		m_nLeft = tab.m_nLeft;
	}
public:
	CEGDockingTabBtn( CEGDockingPane * pPane ) {
		m_pPane = pPane;
		m_nWidth = 0;
	}

	CEGDockingTabBtn( const CEGDockingTabBtn & tab ) {
		Copy( tab );
	}

	CEGDockingTabBtn & operator=( const CEGDockingTabBtn & tab ) {
		Copy( tab );
		return *this;
	}

	CEGDockingPane * m_pPane;
	
	TCHAR * GetTitle() { return m_pPane->GetTitle(); }

	int m_nWidth;
	int m_nLeft;
};

typedef list< CEGDockingTabBtn > CEGDockingTabBtns;
typedef list< CEGDockingTabBtn >::iterator CEGDockingTabBtnsIt;

typedef list< CEGDockingPane* > CEGDockingPanes;
typedef CEGDockingPanes::iterator CEGDockingPanesIt;
typedef CEGDockingPanes::reverse_iterator CEGDockingPanesRIt;

class CEGDockingBar : public CControlBar
{
	friend class CEGDockingContext;
	friend class CEGMiniDockFrameWnd;

public:
		CEGDockBorder * m_pBorder;
protected:
		CEGDockingPanes	m_lstDockPanes;
		CEGDockingTabBtns m_lstTabButtons;
		CEGDockingPane * m_pActivePane;

		CRect m_rcSaved;

		BOOL m_bFlyOutMode;
		CFrameWnd *m_pFrameWnd;

		BOOL m_bTabDrag;
		int m_nTabDragStart;

		DECLARE_DYNAMIC(CEGDockingBar);

		void TuneGripper( LPRECT lprcBounds, UINT nDockBarID );
		void CalcButtons();
		void RecalcLocalLayout();
		CEGDockingPane * GetNextActivePane( CEGDockingPane * pActivePane );
		int GetVisibleCount();

// Construction
public:
    CEGDockingBar( CSize size );

    virtual BOOL Create(LPCTSTR lpszWindowName,  UINT nIconID, CWnd* pParentWnd,
        UINT nID, DWORD dwStyle = WS_CHILD | WS_VISIBLE | CBRS_TOP | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC);
		
// Attributes
public:
    const BOOL IsFloating() const;
    const BOOL IsHorzDocked() const;
    const BOOL IsVertDocked() const;
    const BOOL IsSideTracking() const;
    const BOOL GetPaneStyle() const {return m_dwPaneStyle;}
		UINT  GetDockBarID(){ return  m_nDockBarID;};
		void ShowPane( CEGDockingPane* pPane );
		BOOL	IsEmpty(){ return m_lstDockPanes.empty(); }
		BOOL	IsFlyOutMode() { return m_bFlyOutMode; }
		int Width(){ return m_rcSaved.Width(); }
		int Height(){ return m_rcSaved.Height(); }

		DockType GetDropTarget( CPoint ptScreen, CEGDockingPane ** ppPane );
		CRect GetDropRect( DockType nDockType );

// Operations
public:
    void EnableDocking(DWORD dwDockStyle);
		void ShowBar( BOOL bVisible );

		virtual void LoadState(LPCTSTR lpszProfileName);
    virtual void SaveState(LPCTSTR lpszProfileName);
    static void GlobalLoadState(CFrameWnd* pFrame, LPCTSTR lpszProfileName);
    static void GlobalSaveState(CFrameWnd* pFrame, LPCTSTR lpszProfileName);
    void SetPaneStyle(DWORD dwPaneStyle){m_dwPaneStyle = (dwPaneStyle & ~DPS_EDGEALL);}

		void AddToBorder(CEGDockBorder *pBorder);
		void RemoveFromBorder( CEGDockBorder *pBorder, BOOL bShow = TRUE );

		void ActivatePane( CEGDockingPane* pPane );
		void AddPane( CEGDockingPane* pPane );
		void RemovePane( CEGDockingPane* pPane );

// Overridables
    virtual void OnUpdateCmdUI(CFrameWnd* pTarget, BOOL bDisableIfNoHndler);
		void GetGripperRect( LPRECT lprc );
	
// Overrides
public:
    virtual CSize CalcFixedLayout(BOOL bStretch, BOOL bHorz);
    virtual CSize CalcDynamicLayout(int nLength, DWORD dwMode);
		virtual void DrawGripper(CDC* pDC, const CRect& rect);
		virtual void DrawBorders(CDC* pDC, CRect& rect);
		void DrawTabs( CDC * pDC, LPRECT lprcBounds );

		virtual void DoPaint(CDC* pDC);

// Implementation
public:
    virtual ~CEGDockingBar();

		void PinPane();
    

protected:
    UINT GetEdgeHTCode(int nEdge);
    BOOL GetEdgeRect(CRect rcWnd, UINT nHitTest, CRect& rcEdge);
    virtual void StartTracking(UINT nHitTest, CPoint point);
    virtual void StopTracking();
    virtual void OnTrackUpdateSize(CPoint& point);
    virtual void OnTrackInvertTracker();
//    virtual void NcPaintGripper(CDC* pDC);
    virtual void NcCalcClient(LPRECT pRc, UINT nDockBarID);

    virtual void AlignControlBars();
    void GetRowInfo(INT_PTR& nFirst, INT_PTR& nLast, INT_PTR& nThis);
    void GetRowSizingBars(CDPArray& arrSCBars);
    void GetRowSizingBars(CDPArray& arrSCBars, INT_PTR& nThis);
    BOOL NegotiateSpace(INT_PTR nLengthTotal, BOOL bHorz);

		void CheckState();
		void RePaint();

protected:
    DWORD   m_dwPaneStyle;
    UINT    m_htEdge;
		UINT		m_nIconID;

public:
    CSize   m_szHorz;
    CSize   m_szVert;
protected:
    CSize   m_szFloat;
		CSize   m_szMinHorz;
    CSize   m_szMinVert;
    CSize   m_szMinFloat;
    int     m_nTrackPosMin;
    int     m_nTrackPosMax;
    int     m_nTrackPosOld;
    int     m_nTrackEdgeOfs;
    BOOL    m_bTracking;
    BOOL    m_bKeepSize;
    BOOL    m_bParentSizing;
    BOOL    m_bDragShowContent;
		BOOL		m_bActive;
		BOOL		m_bAutoHidePinned;

		BOOL m_bClosePressed;
		BOOL m_bCloseHover;

		BOOL m_bPinPressed;
		BOOL m_bPinHover;

    UINT    m_nDockBarID;
    int     m_cxEdge;
		int			m_cyCaption;
		int			m_cyTabs;
		CRect		m_rcClose;
		CRect		m_rcPin;
		CRect		m_rcCaption;
		CRect		m_rcClient;
		CRect		m_rcNonClient;
		UINT		m_nTimer;

// Generated message map functions
protected:
    afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
		afx_msg void OnDestroy();
		afx_msg void OnTimer( UINT_PTR nIDEvent );
    afx_msg void OnNcPaint();
    afx_msg void OnNcCalcSize(BOOL bCalcValidRects, NCCALCSIZE_PARAMS FAR* lpncsp);
    #if _MFC_VER < 0x0800  
  afx_msg UINT OnNcHitTest(CPoint point);
#else
  afx_msg LRESULT OnNcHitTest(CPoint point);
#endif
    afx_msg void OnCaptureChanged(CWnd *pWnd);
		afx_msg void OnCancelMode( );
    afx_msg void OnSettingChange(UINT uFlags, LPCTSTR lpszSection);
    afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
    afx_msg void OnMouseMove(UINT nFlags, CPoint point);
    afx_msg void OnNcLButtonDown(UINT nHitTest, CPoint point);
    afx_msg void OnNcLButtonUp(UINT nHitTest, CPoint point);
		afx_msg void OnNcMouseMove( UINT, CPoint );
    afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
    afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
    afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
    afx_msg void OnWindowPosChanging(WINDOWPOS FAR* lpwndpos);
    afx_msg void OnClose();
    afx_msg void OnSize(UINT nType, int cx, int cy);
    afx_msg LRESULT OnSetText(WPARAM wParam, LPARAM lParam);
		afx_msg LRESULT OnNcMouseLeave( WPARAM wParam, LPARAM lParam );
		afx_msg LRESULT OnPaneStateChanged( WPARAM wParam, LPARAM lParam );
    DECLARE_MESSAGE_MAP()

    friend class CEGDockingFrameWnd;
};

typedef set< CEGDockingBar* > CEGDockingBars;
typedef CEGDockingBars::iterator CEGDockingBarsIt;

void RecalcBarSizes( CDockBar * pDockBar );
