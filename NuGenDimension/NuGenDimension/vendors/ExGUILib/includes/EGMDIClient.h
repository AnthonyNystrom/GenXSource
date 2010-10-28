#if !defined (__MDIClient_h)
#define __MDIClient_h

/////////////////////////////////////////////////////////////////////////////
// CEGMDIClient window

#include <list>

using std::list;

class CEGMDITabBtn {
	void Copy( const CEGMDITabBtn & tab );
public:
	CEGMDITabBtn( HWND hWnd );
	CEGMDITabBtn( const CEGMDITabBtn & tab );

	CEGMDITabBtn & operator=( const CEGMDITabBtn & tab );

	HWND m_hWnd;
	
	CString GetTitle();

	int m_nWidth;
	int m_nLeft;
	COLORREF m_clrColor;
};

typedef list< CEGMDITabBtn > CEGMDITabBtns;
typedef CEGMDITabBtns::iterator CEGMDITabBtnsIt;

class MatchWndBtn {
	HWND m_hWnd;
public:
	MatchWndBtn( HWND hWnd ) {
		m_hWnd = hWnd;	
	}
	bool operator() ( CEGMDITabBtn btn ) { 
		return btn.m_hWnd == m_hWnd; 
	}
};

class HotWndBtn {
	int m_nX;
public:
	HotWndBtn( int x ) {
		m_nX = x;	
	}
	bool operator() ( CEGMDITabBtn btn ) { 
		return ( btn.m_nLeft <= m_nX && m_nX <= btn.m_nLeft + btn.m_nWidth );
	}
};

class CEGMDIClient : public CWnd
{
public:
   enum DisplayModesEnum {
      dispTile,
      dispCenter,
      dispStretch,
      dispCustom
   };
   
// Construction
public:
   CEGMDIClient();

// Attributes
public:
   // Set background color
   void SetBkColor( COLORREF clrValue );
   COLORREF GetBkColor() const;

   // Load background bitmap from given file
   BOOL SetBitmap( LPCTSTR lpszFileName, UINT uFlags = LR_LOADMAP3DCOLORS );

   // Load background bitmap from resource. You can map some colors using 
   // COLORMAP struct (see LoadMappedBitmap() for details how to use it)
   BOOL SetBitmap( UINT nBitmapID, COLORMAP* pClrMap = NULL, int nCount = 0 );

   // Set desired display mode (tile, center, stretch or custom)
   void SetDisplayMode( DisplayModesEnum eDisplayMode );
   DisplayModesEnum GetDisplayMode() const;

   // Specify coordinates of image's top-left corner. Used when 
   // dispCustom is selected
   void SetOrigin( int x, int y, BOOL bRedraw = TRUE );
   void SetOrigin( const CPoint& point, BOOL bRedraw = TRUE );
   const CPoint& GetOrigin() const;

   // Return the current image size.
   const CSize& GetImageSize() const;

   // Return the filename of the bitmap
   const CString& GetFileName() const;

   // If set to TRUE, the current state is automatically 
   // saved on WM_DESTROY and restored on PreSubclassWindow()
   void SetAutoSaveRestore( BOOL bNewValue );
   BOOL GetAutoSaveRestore() const;

// Operations
public:
   // Restore original (system-wide) settings
   void Reset();

   // Load/Store current settings in registry
   void SaveState();
   void RestoreState();

// Overrides
   // ClassWizard generated virtual function overrides
   //{{AFX_VIRTUAL(CEGMDIClient)
   virtual void PreSubclassWindow();
   //}}AFX_VIRTUAL

// Implementation
public:
   virtual ~CEGMDIClient();

   // Generated message map functions
protected:
   //{{AFX_MSG(CEGMDIClient)
   afx_msg void OnPaint();
   afx_msg void OnDestroy();
   afx_msg BOOL OnEraseBkgnd( CDC* pDC );
   afx_msg void OnSize( UINT nType, int cx, int cy );   
	 afx_msg LRESULT OnMDICreate ( WPARAM wParam, LPARAM lParam );
	 afx_msg LRESULT OnMDIDestroy ( WPARAM wParam, LPARAM lParam );
	 afx_msg LRESULT OnMDINext ( WPARAM wParam, LPARAM lParam );
	 afx_msg LRESULT OnMDIActivate ( WPARAM wParam, LPARAM lParam );
	 afx_msg LRESULT OnRecalcButtons ( WPARAM wParam, LPARAM lParam );
	 afx_msg void OnStyleChanged( int nStyleType, LPSTYLESTRUCT lpStyleStruct );
	 afx_msg void OnNcCalcSize( BOOL bCalcValidRects, NCCALCSIZE_PARAMS* lpncsp );
   afx_msg void OnNcPaint();
	 afx_msg void OnNcLButtonDown(UINT nHitTest, CPoint point);
	 afx_msg void OnNcLButtonUp(UINT nHitTest, CPoint point);
	#if _MFC_VER < 0x0800  
  afx_msg UINT OnNcHitTest(CPoint point);
#else
  afx_msg LRESULT OnNcHitTest(CPoint point);
#endif
	 afx_msg void OnNcMouseMove( UINT nHitTest, CPoint point);
	 afx_msg void OnCancelMode( );
	 afx_msg void OnLButtonUp( UINT nFlags, CPoint point );
	 afx_msg void OnMouseMove( UINT nFlags, CPoint point);
	 //}}AFX_MSG
   DECLARE_MESSAGE_MAP()
   
   CBitmap m_bitmap;             // The background bitmap
   CBrush m_brush;               // Brush used for background painting
   COLORREF m_clrBackground;     // The background color
   BOOL m_bAutoSaveRestore;      // Automatically save/restore the state
   CString m_strFileName;        // The filename if bitmap was loaded from a file
   CPoint m_ptOrigin;            // Coordinates of top-left corner of image. Used when dispCustom is selected
   CSize m_sizeImage;            // Cache the image size;
   DisplayModesEnum m_eDisplayMode;   

		CEGMDITabBtns m_lstTabBtns;
		BOOL m_bShowTabs;
		WNDPROC m_oldWndProc;
		
//		CFont m_fntBold;
//		CFont m_fntThin;

//		COLORREF m_clrLight;
//		COLORREF m_clrShadow;
//		COLORREF m_clrSeparator;

		BOOL m_bPriorEnabled;
		BOOL m_bPriorHover;
		BOOL m_bPriorPressed;

		BOOL m_bNextEnabled;
		BOOL m_bNextHover;
		BOOL m_bNextPressed;
		
		BOOL m_bCloseEnabled;
		BOOL m_bCloseHover;
		BOOL m_bClosePressed;
		BOOL m_bTabDrag;

		CRect m_rcClose;
		CRect m_rcNext;
		CRect m_rcPrior;

		int m_cxEdge;
		int m_cxControls;
		int	m_nWidth;
		int m_cxOffset;
		int m_cxBtnsAvailable;
		int m_cxBtnsTotal;

		HWND m_hWndActive;
		void DrawTabs( CDC * pDC, LPRECT lprcBounds );
		void DrawControls( CDC * pDC, LPRECT lprcBounds );
		void CalcButtons( LPRECT lprcBounds );

		// Actions
		void ActivateMDI( HWND hWnd );
		void CloseActiveMDI();
		void ScrollNext();
		void ScrollPrior();
};

/////////////////////////////////////////////////////////////////////////////
// CEGMDIClient inlines

inline
COLORREF CEGMDIClient::GetBkColor() const
{
   return m_clrBackground;
}

inline
CEGMDIClient::DisplayModesEnum CEGMDIClient::GetDisplayMode() const
{
   return m_eDisplayMode;
}

inline
const CString& CEGMDIClient::GetFileName() const
{
   return m_strFileName;
}

inline
void CEGMDIClient::SetOrigin( int x, int y, BOOL bRedraw )
{
   m_ptOrigin.x = x;
   m_ptOrigin.y = y;
   if ( bRedraw == TRUE && IsWindow( m_hWnd ) )
      Invalidate();   
}

inline
void CEGMDIClient::SetOrigin( const CPoint& point, BOOL bRedraw )
{
   m_ptOrigin = point;
   if ( bRedraw == TRUE && IsWindow( m_hWnd ) )
      Invalidate();      
}

inline
const CPoint& CEGMDIClient::GetOrigin() const
{
   return m_ptOrigin;
}

inline
const CSize& CEGMDIClient::GetImageSize() const
{   
   return m_sizeImage;
}

inline
void CEGMDIClient::SetAutoSaveRestore( BOOL bNewValue )
{
   m_bAutoSaveRestore = bNewValue;
}

inline
BOOL CEGMDIClient::GetAutoSaveRestore() const
{
   return m_bAutoSaveRestore;   
}

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // __MDIClient_h
