#pragma once

class COMemDC : public CDC
{
public:

    // constructor sets up the memory DC
    COMemDC(CDC* pDC) : CDC()
    {
        ASSERT(pDC != NULL);

        m_pDC = pDC;
        m_pOldBitmap = NULL;
        m_bMemDC = !pDC->IsPrinting();
              
        if (m_bMemDC)    // Create a Memory DC
        {
            pDC->GetClipBox(&m_rect);
            CreateCompatibleDC(pDC);
            m_bitmap.CreateCompatibleBitmap(pDC, m_rect.Width(), m_rect.Height());
            m_pOldBitmap = SelectObject(&m_bitmap);
            SetWindowOrg(m_rect.left, m_rect.top);
        }
        else        // Make a copy of the relevent parts of the current DC for printing
        {
            m_bPrinting = pDC->m_bPrinting;
            m_hDC       = pDC->m_hDC;
            m_hAttribDC = pDC->m_hAttribDC;
        }
    }
    
    // Destructor copies the contents of the mem DC to the original DC
    ~COMemDC()
    {
        if (m_bMemDC) 
        {    
            // Copy the offscreen bitmap onto the screen.
            m_pDC->BitBlt(m_rect.left, m_rect.top, m_rect.Width(), m_rect.Height(),
                          this, m_rect.left, m_rect.top, SRCCOPY);

            //Swap back the original bitmap.
            SelectObject(m_pOldBitmap);
        } else {
            // All we need to do is replace the DC with an illegal value,
            // this keeps us from accidently deleting the handles associated with
            // the CDC that was passed to the constructor.
            m_hDC = m_hAttribDC = NULL;
        }
    }

    // Allow usage as a pointer
    COMemDC* operator->() {return this;}
        
    // Allow usage as a pointer
    operator COMemDC*() {return this;}

private:
    CBitmap  m_bitmap;      // Offscreen bitmap
    CBitmap* m_pOldBitmap;  // bitmap originally found in CMemDC
    CDC*     m_pDC;         // Saves CDC passed in constructor
    CRect    m_rect;        // Rectangle of drawing area.
    BOOL     m_bMemDC;      // TRUE if CDC really is a Memory DC.
};

/////////////////////////////////////////////////////////////////////////////

/* *****************************************************************
** CEGButtonsBar - a sort-of outlook bar-style control
    ****************************************************************/

// CEGButtonsBar

class CEGButtonsFolder
{
public:
	CEGButtonsFolder();
	virtual ~CEGButtonsFolder();

	CString		csName;
	HICON		hIcon;
	DWORD		dwStyle;
	CRect		rcItem;
	CPtrArray	m_Items;
	DWORD		lParam;
	UINT		nCommandID;
};

class CEGButtonsItem
{
public:
	CEGButtonsItem();
	virtual ~CEGButtonsItem();

	CString		csName;
	DWORD		dwStyle;
	CRect		rcItem;
	CPtrArray	m_SubItems;
};


class CEGButtonsSubItem
{
public:
	CEGButtonsSubItem();
	virtual ~CEGButtonsSubItem();

	CString		csName;
	HICON		hIcon;
	DWORD		dwStyle;
	CRect		rcItem;
	DWORD		lParam;
	HWND		hHostedWnd;
	int			iLastStatus;
};

// Helper class that integrates in the MFC command handler chain 

class CEGButtonsCCmdUI : public CCmdUI
{
public:
	CEGButtonsCCmdUI() { iRes = 0; pSI = NULL; };

	int iRes;
	CEGButtonsSubItem * pSI;

	virtual void Enable(BOOL /* bOn */ = TRUE) 
		{ 
			iRes |= 1; 
		};
	virtual void SetCheck(int nCheck = 1) 
		{ 
			if (nCheck) iRes |= 2;
			
		};
	virtual void SetRadio(BOOL bOn = TRUE)
		{ 
			if (bOn) iRes |= 4;
		};
	virtual void SetText(LPCTSTR lpszText)
		{ 
			pSI->csName = lpszText;
		};
};


class CEGButtonsBar : public CControlBar
{
	DECLARE_DYNAMIC(CEGButtonsBar)
	BOOL m_bIsMouseInside;
public:

	enum { USE_LAST_INSERT = -1 };
	enum { OCL_SELECT = 0, OCL_RADIO = 1, OCL_COMMAND = 2, OCL_CHECK = 3,  OCL_HWND = 4 };

	CEGButtonsBar();
	virtual ~CEGButtonsBar();

	int m_iSize;
	int m_iSplitterSize;
	int m_iDragging;
	int m_iDragoffset;
	CRect m_dragRectBegin;
	CRect m_dragRect;
	HCURSOR hDragCur;
	HCURSOR hHandCur;
	bool  bTrackCur;
	CEGButtonsSubItem * pLastHilink;

	CRect rcInnerRect;

	int iHiFolder, iHiLink;
	int iHilinkFolder, iHilinkItem;

	CFont		ftCaption;
	CString		m_csCaption;
	COLORREF	m_crBackCaption;
	COLORREF	m_crTextCaption;
	COLORREF	m_crCmdLink;
	COLORREF	m_crCmdOther;
	COLORREF	m_crBackground;
	COLORREF	m_crDisabled;

	CPtrArray   m_Folders;
	int			m_iNumFoldersDisplayed;
	int			m_iFolderHeight;
	int			m_iItemHeight;
	int			m_iSubItemHeight;
	int			m_iSelectedFolder;
	CFont		ftFolders;
	CFont		ftItems;
	CFont		ftHotItems;

	// Adds a folder to the control.
	int AddFolderRes(const char * m_strName, UINT iIcoID, DWORD lParam = 0, UINT nCmdID = 0);
	// Adds a folder to the control.
	int AddFolder(const char * m_strName, HICON hIcon, DWORD lParam = 0, UINT nCmdID = 0);
	// Remove a folder from the control.
	bool  RemoveFolder(int ind);

	int AddSubItem(const char * m_strName, int iIcoID, DWORD dwStyle, DWORD lParam = 0, int iFolder = USE_LAST_INSERT, int iFolderItem = USE_LAST_INSERT);
	int AddSubItem(const char * m_strName, HICON hIcon, DWORD dwStyle, DWORD lParam = 0, int iFolder = USE_LAST_INSERT, int iFolderItem = USE_LAST_INSERT);
	// Adds a subitem to a folder.item. Supports the following styles:
	// OCL_SELECT, OCL_RADIO, OCL_COMMAND, OCL_CHECK - or a child window.
	int AddSubItem(HWND hHosted, bool bStretch = false, int iFolder = USE_LAST_INSERT, int iFolderItem = USE_LAST_INSERT);

	// Adds an item to a folder. An item is a separating label / line which will contains subitems.
	int AddFolderItem(const char * m_strItemName, DWORD dwStyle = 0, int iFolder = USE_LAST_INSERT);

	// Overriddable; perform the drawing of a folder.
	virtual void DrawButton(CDC * pDC, CEGButtonsFolder * o, bool bSel, bool bOver);
	// Overriddable; perform the drawing of an item.
	virtual void DrawItem(CDC * pDC, CEGButtonsFolder * o, CEGButtonsItem * i);
	// Overriddable; perform the drawing of a subitem.
	virtual void DrawSubItem(CDC * pDC, CEGButtonsFolder * oFolder, CEGButtonsItem * iItem, CEGButtonsSubItem * pSubItem, bool bOver);

	// Here the standard colors for the control are defined. You can modify them here.
	virtual void SetupColors();

	void OnInvertTracker(const CRect& rect);
	// returns actual size of the control to the docking framework.
	CSize CalcFixedLayout (BOOL /*bStretch*/, BOOL bHorz);
	// Performs the drawing of the gradient graphic if compiled with WINVER >= 0x0500. 
	// It makes use of gdiplus.lib.
	void DrawGradientRect(CDC * pDC, CRect &rect, COLORREF cr1, COLORREF cr2, DWORD dwStyle = GRADIENT_FILL_RECT_V);

	// Allows to select a particular folder. The bAnimation flag tells if we wish a scrolling animation to be performed.
	int GetCurFolder( );
	void SetCurFolder(int f, bool bAnimation = true, BOOL bNotify = FALSE);
	// Executes the animation from current folder to folder F and sets it as current selected folder.
	void AnimateToFolder(int f);

protected:
	DECLARE_MESSAGE_MAP()
public:
	bool Create(CWnd * pParent, int iId);
	afx_msg void OnPaint();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg LRESULT OnMouseLeave( WPARAM wParam, LPARAM lParam );

	// Control which element is under the point. The function returns:
	// 0 = nothing; 1 = folder is valid; 2 = folder.item is valid, 4 = folder.item.subitem is valid
	int HitTest(int & iFolder, int & iItem, int & iSubItem, CPoint point);
	// Invalidates all the folder.items.subitems areas' internal structure (not on video)
	void ClearRects(void);
	
	// Perform the UI idle updating for the check/radio/select subitems
	virtual void OnUpdateCmdUI(CFrameWnd* pTarget, BOOL bDisableIfNoHndler);

	CEGButtonsSubItem * GetSubItem(int f, int i, int s);
	CEGButtonsItem * GetItem(int f, int i);
	CEGButtonsFolder * GetFolder(int f);

	// Draw the big caption above the control. Makes the inner area (passed with rect) smaller on return.
	void DrawCaption(CDC * pDC, CRect & rect);
	// Draw all the folders (calling DrawButton when needed)
	void DrawButtons(CDC * pDC, CRect & rect);
	// Draw all the items (calling DrawItem when needed)
	void DrawItems(CDC * pDC, CEGButtonsFolder * oFolder, CRect & rect);
	// Draw all the subitems (calling DrawSubItem when needed)
	void DrawSubItems(CDC * pDC, CEGButtonsFolder * oFolder, CEGButtonsItem * iItem, CRect & rect);
};

