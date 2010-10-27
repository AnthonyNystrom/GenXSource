//-----------------------------------------------------------------------------
// MainFrm.h : Interface of the CMainFrame class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#if !defined(AFX_MAINFRM_H__57A4B3C2_0CAF_46B4_9DC4_82358D2CB91B__INCLUDED_)
#define AFX_MAINFRM_H__57A4B3C2_0CAF_46B4_9DC4_82358D2CB91B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CMainFrame : public CFrameWnd
{
    
protected: // create from serialization only
    CMainFrame();
    DECLARE_DYNCREATE(CMainFrame)

// Attributes
public:

// Operations
public:

// Overrides
    // ClassWizard generated virtual function overrides
    //{{AFX_VIRTUAL(CMainFrame)
    public:
    virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
    protected:
    virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);
    //}}AFX_VIRTUAL

// Implementation
public:
    virtual ~CMainFrame();
#ifdef _DEBUG
    virtual void AssertValid() const;
    virtual void Dump(CDumpContext& dc) const;
#endif

protected:  // control bar embedded members
    CStatusBar  m_wndStatusBar;
    CToolBar    m_wndToolBar;
    CReBar      m_wndReBar;

// Generated message map functions
protected:
    //{{AFX_MSG(CMainFrame)
    afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnDestroy();
    afx_msg void OnViewResizeHalf();
    afx_msg void OnViewResizeOriginal();
    afx_msg void OnViewResizeDouble();
    afx_msg void OnViewFullScreen();
    afx_msg void OnUpdateViewFullScreen(CCmdUI* pCmdUI);
    afx_msg void OnViewToolBar();
    afx_msg void OnViewStatusBar();
	afx_msg void OnFileSaveModified();
	afx_msg void OnUpdateFileSaveModified(CCmdUI* pCmdUI);
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()

public:
    void UpdateSize(float scale=1.0f);  // Resize the window to fit the source.
	void SetFullScreen(BOOL fullScreen);

	BOOL LoadOptions();
	BOOL SaveOptions();

    HMENU       m_hMenu;        // Menu handler before changed to full screen
    BOOL        m_bToolBar;     // Shown or hidden the tool bar before changed to full screen
    BOOL        m_bStatusBar;   // Shown or hidden the status bar before changed to full screen
    BOOL        m_bFullScreen;  // Full screen or not
	BOOL        m_bSaveModified;// Confirm to save modified settings when exit
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINFRM_H__57A4B3C2_0CAF_46B4_9DC4_82358D2CB91B__INCLUDED_)
